// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/scripts
// 文件名称：generate-services-from-dtos.cjs
// 创建时间：2026-05-23
// 创建人：Takt365(Cursor AI)
// 功能描述：根据 Takt.Application/Dtos/*Dtos.cs 自动生成服务接口与实现（独立脚本）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const fs = require('fs');
const path = require('path');
const { writeGeneratedFile, logGeneratedFileWritePolicy } = require('./generate-script-common.cjs');
const {
  MANUAL_CRUD_ENTITY_SHORT_NAMES,
  MANUAL_CRUD_DTO_FILE_NAMES,
  isManualCrudEntity,
  isRbacJunctionEntity,
  shouldExcludeDtoFile: shouldExcludeManualCrudDtoFile,
  shouldExcludeStandaloneService,
} = require('./generate-entity-exclusions.cjs');

const {
  hasRbacParentConfig,
  generateRbacParentDelegationExtras,
} = require('./rbac-parent-config.cjs');
const {
  isTransposableEntity,
  getTransposableConfig,
  generateTransposedInterfaceMethods,
  generateTransposedServiceImplementation,
} = require('./generate-transposed-support.cjs');

// ========================================
// 配置
// ========================================

const CONFIG = {
  backendRoot: path.resolve(__dirname, '../backend/src'),
  entitiesRoot: path.join(path.resolve(__dirname, '../backend/src'), 'Takt.Domain', 'Entities'),
  dtosRoot: path.join(path.resolve(__dirname, '../backend/src'), 'Takt.Application', 'Dtos'),
  servicesRoot: path.join(path.resolve(__dirname, '../backend/src'), 'Takt.Application', 'Services'),
};

/**
 * 手工维护的特殊实体（禁止脚本生成服务）
 * --all 时自动跳过；--User / --Online / --Message 将直接报错退出
 */
function isSpecialEntity(entityShort) {
  return isManualCrudEntity(entityShort);
}

/** 特殊实体对应的 Dtos 文件名（双重排除，防止误扫） */
const SPECIAL_ENTITY_DTO_FILES = MANUAL_CRUD_DTO_FILE_NAMES;

/**
 * 已有手工服务（实体类名精确匹配）
 * - TaktAuth：无 TaktAuthDtos 标准聚合，认证在 TaktAuthsController
 * TaktLoginLog 走标准生成；Identity 的 TaktLoginDtos.cs 见下方排除列表
 */
const EXISTING_MANUAL_SERVICE_ENTITIES = new Set(['TaktAuth', 'TaktRbac', 'TaktUser']);

/**
 * 禁止进入扫描列表的 *Dtos.cs（仅文件名全字匹配）
 * - TaktLoginDtos.cs：Identity 登录/令牌 DTO（特殊，非实体 CRUD），与统计域 TaktLoginLog 无关
 */
const EXCLUDED_DTO_FILE_NAMES = new Set([
  'TaktLoginDtos.cs',
  'TaktCacheDtos.cs',
  'TaktServerMonitorDtos.cs',
  ...SPECIAL_ENTITY_DTO_FILES,
]);

/** QueryDto 中继承自 TaktPagedQuery 的字段，不参与 QueryExpression */
const PAGED_QUERY_FIELDS = new Set(['PageIndex', 'PageSize', 'KeyWords']);

/** DTO 基类（与 TaktDtoBase.cs 一致，驱动隔离过滤与仓储接口） */
const DTO_BASE_NAMES = ['TaktTenantDtoBase', 'TaktCompanyDtoBase', 'TaktApprovalDtoBase'];

const DTO_BASE_TO_ENTITY_BASE = {
  TaktTenantDtoBase: 'TaktTenantEntityBase',
  TaktCompanyDtoBase: 'TaktCompanyEntityBase',
  TaktApprovalDtoBase: 'TaktApprovalEntityBase',
};

const DTO_BASE_TO_REPOSITORY = {
  TaktTenantDtoBase: 'ITaktTenantRepository',
  TaktCompanyDtoBase: 'ITaktCompanyRepository',
  TaktApprovalDtoBase: 'ITaktApprovalRepository',
};

/** 实体基类 → 仓储接口（子表注入） */
const ENTITY_BASE_TO_REPOSITORY = {
  TaktTenantEntityBase: 'ITaktTenantRepository',
  TaktCompanyEntityBase: 'ITaktCompanyRepository',
  TaktApprovalEntityBase: 'ITaktApprovalRepository',
};

const NAVIGATION_REGION_MARKER = '导航属性区域';

/** 唯一索引中不参与查重的系统/审计字段（TenantCode 亦由仓储隔离，见 getUniqueIndexScopeFields） */
const UNIQUE_INDEX_SKIP_FIELDS = new Set([
  'TenantCode',
  'Id',
  'IsDeleted',
  'CreatedAt',
  'UpdatedAt',
  'CreatedBy',
  'UpdatedBy',
  'DeletedBy',
  'DeletedAt',
  'ApprovalStatus',
  'InitiatorId',
  'InitiatedAt',
  'ApprovedBy',
  'ApprovedAt',
]);

// ========================================
// 工具
// ========================================

/**
 * 是否为特殊实体（服务需手工维护）
 * @param {string} entityShort 不含 Takt 前缀，如 User、Online
 */
function isSpecialEntity(entityShort) {
  return isManualCrudEntity(entityShort);
}

/**
 * 是否应跳过该 Dtos 文件
 * @param {string} dtoFile 绝对路径
 */
function shouldExcludeDtoFile(dtoFile) {
  const fileName = path.basename(dtoFile);
  if (EXCLUDED_DTO_FILE_NAMES.has(fileName)) {
    return true;
  }
  return shouldExcludeManualCrudDtoFile(dtoFile);
}

/**
 * CLI 指定单实体时，禁止对特殊实体生成
 * @param {string} entityShort
 */
function assertNotSpecialEntityCli(entityShort) {
  if (!isSpecialEntity(entityShort)) {
    return;
  }
  console.error(
    `❌ 实体 ${entityShort} 为手工维护的特殊模块（如 TaktUserService、TaktDictDataService），禁止本脚本生成。`,
  );
  console.error(`   已排除: ${[...MANUAL_CRUD_ENTITY_SHORT_NAMES].join('、')}`);
  process.exit(1);
}

function isInEngineDirectory(filePath) {
  const normalizedPath = filePath.replace(/\\/g, '/');
  return /\/\w*[Ee]ngine($|\/)/i.test(normalizedPath);
}

function readUtf8(filePath) {
  return fs.readFileSync(filePath, 'utf-8');
}

function entityNameFromDtoFile(dtoFile) {
  const base = path.basename(dtoFile, '.cs');
  if (!base.endsWith('Dtos')) {
    return null;
  }
  return base.slice(0, -'Dtos'.length);
}

function extractClassBlock(content, className) {
  const startRegex = new RegExp(`public\\s+(?:partial\\s+)?class\\s+${className}\\b`);
  const startMatch = startRegex.exec(content);
  if (!startMatch) {
    return '';
  }
  const braceStart = content.indexOf('{', startMatch.index);
  if (braceStart < 0) {
    return '';
  }
  let depth = 0;
  for (let i = braceStart; i < content.length; i += 1) {
    if (content[i] === '{') {
      depth += 1;
    } else if (content[i] === '}') {
      depth -= 1;
      if (depth === 0) {
        return content.slice(braceStart + 1, i);
      }
    }
  }
  return '';
}

/**
 * 列出文件中所有 Get*OptionsAsync 方法名（含 GetXxxTreeOptionsAsync）
 * @param {string} content
 * @returns {string[]}
 */
function listGetOptionsAsyncMethodNames(content) {
  const names = new Set();
  const re = /\b(Get\w+OptionsAsync)\s*\(/g;
  let match;
  while ((match = re.exec(content)) !== null) {
    names.add(match[1]);
  }
  return [...names];
}

/**
 * 从方法签名位置向前提取紧邻的 XML 文档注释
 * @param {string} content
 * @param {number} signatureIndex
 */
function extractLeadingXmlDocBeforeSignature(content, signatureIndex) {
  const before = content.slice(0, signatureIndex);
  const lines = before.split('\n');
  const docLines = [];
  for (let i = lines.length - 1; i >= 0; i -= 1) {
    const line = lines[i];
    const trimmed = line.trim();
    if (trimmed === '') {
      continue;
    }
    if (/^\/\/\//.test(trimmed)) {
      docLines.unshift(line);
      continue;
    }
    break;
  }
  return docLines.length > 0 ? `${docLines.join('\n')}\n` : '';
}

/**
 * 按大括号深度截取方法体（含首尾花括号）
 * @param {string} content
 * @param {number} openBraceIndex
 */
function sliceBalancedBraceBlock(content, openBraceIndex) {
  let depth = 0;
  for (let i = openBraceIndex; i < content.length; i += 1) {
    if (content[i] === '{') {
      depth += 1;
    } else if (content[i] === '}') {
      depth -= 1;
      if (depth === 0) {
        return content.slice(openBraceIndex, i + 1);
      }
    }
  }
  return null;
}

/**
 * 提取单个 Get*OptionsAsync 方法块（含 XML 注释）
 * @param {string} content
 * @param {string} methodName
 * @param {'interface'|'implementation'} variant
 */
function extractGetOptionsAsyncMethodBlock(content, methodName, variant) {
  const anchorRe = new RegExp(`\\b${methodName}\\s*\\(`);
  const anchorMatch = anchorRe.exec(content);
  if (!anchorMatch) {
    return null;
  }

  const anchorIndex = anchorMatch.index;
  const lineStart = content.lastIndexOf('\n', anchorIndex) + 1;
  const lineEnd = content.indexOf('\n', anchorIndex);
  const line = content.slice(lineStart, lineEnd < 0 ? content.length : lineEnd);

  if (variant === 'interface') {
    if (!/\bTask\s*</.test(line) || !line.includes(';')) {
      return null;
    }
    const semi = content.indexOf(';', anchorIndex);
    if (semi < 0) {
      return null;
    }
    let sigStart = lineStart;
    const prevLineEnd = content.lastIndexOf('\n', lineStart - 2);
    const prevLine = content.slice(prevLineEnd + 1, lineStart - 1);
    if (/\bTask\s*</.test(prevLine) && !/^\s*\/\/\//.test(line.trim())) {
      sigStart = prevLineEnd + 1;
    }
    const doc = extractLeadingXmlDocBeforeSignature(content, sigStart);
    let block = doc + content.slice(sigStart, semi + 1);
    if (!block.endsWith('\n\n')) {
      block += block.endsWith('\n') ? '\n' : '\n\n';
    }
    return block;
  }

  if (!/^\s*public\b/.test(line) || !/\bTask\s*</.test(line)) {
    return null;
  }
  const doc = extractLeadingXmlDocBeforeSignature(content, lineStart);
  const openBrace = content.indexOf('{', anchorIndex);
  if (openBrace < 0) {
    return null;
  }
  const body = sliceBalancedBraceBlock(content, openBrace);
  if (!body) {
    return null;
  }
  let block = doc + content.slice(lineStart, openBrace) + body;
  if (!block.endsWith('\n\n')) {
    block += block.endsWith('\n') ? '\n' : '\n\n';
  }
  return block;
}

/**
 * 目标文件中是否已声明指定 Get*OptionsAsync 方法
 * @param {string|null|undefined} content
 * @param {string} methodName
 */
function hasGetOptionsAsyncMethod(content, methodName) {
  if (!content) {
    return false;
  }
  return new RegExp(`\\b${methodName}\\s*\\(`).test(content);
}

/**
 * Get*OptionsAsync / Get*TreeOptionsAsync 实现是否通过仓储查询（拒绝遗留的 GetTenantXxxListAsync 等）
 * @param {string|null} block 方法块全文
 * @param {string} repoField 如 _menuRepository
 * @returns {boolean}
 */
function isValidOptionsImplementationBlock(block, repoField) {
  if (!block || !block.trim()) {
    return false;
  }
  if (!block.includes(`${repoField}.GetListAsync`)) {
    return false;
  }
  if (/\bawait\s+Get(?:Tenant|Company)?\w+ListAsync\s*\(/.test(block)) {
    return false;
  }
  return true;
}

/**
 * 解析 Options 方法实现：有效则保留，否则输出模板
 * @param {object} params
 * @param {string|null|undefined} params.existingContent
 * @param {string} params.methodName
 * @param {string} params.repoField
 * @param {string} params.freshTemplate
 * @param {boolean} [params.refreshOptions]
 * @returns {{ block: string, preserved: boolean, regenerated: boolean }}
 */
function resolveOptionsImplementationBlock({
  existingContent,
  methodName,
  repoField,
  freshTemplate,
  refreshOptions = false,
}) {
  if (!hasGetOptionsAsyncMethod(existingContent, methodName)) {
    return { block: freshTemplate, preserved: false, regenerated: false };
  }
  if (refreshOptions) {
    return { block: freshTemplate, preserved: false, regenerated: true };
  }
  const preserved = extractGetOptionsAsyncMethodBlock(
    existingContent,
    methodName,
    'implementation',
  );
  if (preserved && isValidOptionsImplementationBlock(preserved, repoField)) {
    return { block: preserved, preserved: true, regenerated: false };
  }
  return { block: freshTemplate, preserved: false, regenerated: true };
}

/**
 * 生成阶段：已有则原样拷贝，没有才输出模板（接口）
 * @returns {{ block: string, preserved: boolean, methodName: string }}
 */
function buildGetOptionsAsyncInterfaceSection(entityShort, hasTree, dtoInfo, desc, existingContent) {
  const methodName =
    hasTree && dtoInfo.tree ? `Get${entityShort}TreeOptionsAsync` : `Get${entityShort}OptionsAsync`;
  if (hasGetOptionsAsyncMethod(existingContent, methodName)) {
    const preserved = extractGetOptionsAsyncMethodBlock(existingContent, methodName, 'interface');
    if (preserved) {
      return { block: preserved, preserved: true, methodName };
    }
  }
  let block = '';
  if (hasTree && dtoInfo.tree) {
    block += buildMethodXmlDoc({ summary: `获取${desc}树形选项列表`, returns: '树形选项' });
    block += `    Task<List<TaktTreeSelectOption>> ${methodName}();\n\n`;
  } else {
    block += buildMethodXmlDoc({ summary: `获取${desc}选项列表`, returns: '下拉选项' });
    block += `    Task<List<TaktSelectOption>> ${methodName}();\n\n`;
  }
  return { block, preserved: false, methodName };
}

/**
 * 非树形实体：GetXxxOptionsAsync 默认实现模板
 */
function buildFlatOptionsAsyncImplTemplate(
  entityShort,
  desc,
  repoField,
  ensureContextLine,
  optionsListPredicate,
  nameField,
) {
  let block = '';
  block += buildMethodXmlDoc({ summary: `获取${desc}选项列表`, returns: '下拉选项' });
  block += `    public async Task<List<TaktSelectOption>> Get${entityShort}OptionsAsync()\n`;
  block += '    {\n';
  block += ensureContextLine;
  block += `        var list = await ${repoField}.GetListAsync(\n`;
  block += `            ${optionsListPredicate},\n`;
  block += `            x => x.${nameField},\n`;
  block += '            false);\n';
  block += '        return list.Select(e => new TaktSelectOption\n';
  block += '        {\n';
  block += '            DictValue = e.Id,\n';
  block += `            DictLabel = e.${nameField} ?? e.Id.ToString(),\n`;
  block += '        }).ToList();\n';
  block += '    }\n\n';
  return block;
}

/**
 * 从 *Dtos.cs 提取 DTO 元信息（标准：TaktXxxCreateDto / TaktXxxUpdateDto；兼容旧版 TaktCreateXxxDto）
 * @param {string} dtoFile
 */
function extractDtoInfo(dtoFile) {
  const content = readUtf8(dtoFile);
  const entityName = entityNameFromDtoFile(dtoFile);
  const dtos = {
    entityName,
    base: null,
    query: null,
    create: null,
    update: null,
    statuses: [],
    sort: null,
    tree: null,
    template: null,
    import: null,
    export: null,
    transposedQuery: null,
    transposedResult: null,
    transposedBatch: null,
  };

  const classRegex = /public\s+(?:partial\s+)?class\s+(\w+)\s*(?::|\{)/g;
  let match;
  while ((match = classRegex.exec(content)) !== null) {
    const className = match[1];
    if (!className.includes('Dto')) {
      continue;
    }

    if (className.endsWith('TransposedQueryDto')) {
      dtos.transposedQuery = className;
    } else if (className.endsWith('TransposedResultDto')) {
      dtos.transposedResult = className;
    } else if (className.endsWith('TransposedBatchDto')) {
      dtos.transposedBatch = className;
    } else if (className.endsWith('QueryDto')) {
      dtos.query = className;
    } else if (/^Takt\w+CreateDto$/.test(className)) {
      dtos.create = className;
    } else if (/^TaktCreate\w+Dto$/.test(className)) {
      dtos.create = className;
    } else if (/^Takt\w+UpdateDto$/.test(className)) {
      dtos.update = className;
    } else if (/^TaktUpdate\w+Dto$/.test(className)) {
      dtos.update = className;
    } else if (className.endsWith('StatusDto')) {
      dtos.statuses.push(className);
    } else if (className.endsWith('SortDto')) {
      dtos.sort = className;
    } else if (className.endsWith('TreeDto')) {
      dtos.tree = className;
    } else if (className.endsWith('TemplateDto')) {
      dtos.template = className;
    } else if (className.endsWith('ImportDto')) {
      dtos.import = className;
    } else if (className.endsWith('ExportDto')) {
      dtos.export = className;
    } else if (entityName && className === `${entityName}Dto`) {
      dtos.base = className;
    }
  }

  return dtos;
}

function isAggregatable(dtoInfo) {
  return Boolean(dtoInfo.base && dtoInfo.query && dtoInfo.create && dtoInfo.update && dtoInfo.entityName);
}

function findEntityFile(entityName) {
  function searchDir(dir) {
    if (!fs.existsSync(dir)) {
      return null;
    }
    const entries = fs.readdirSync(dir, { withFileTypes: true });
    for (const entry of entries) {
      const fullPath = path.join(dir, entry.name);
      if (entry.isDirectory()) {
        if (isInEngineDirectory(fullPath)) {
          continue;
        }
        const found = searchDir(fullPath);
        if (found) {
          return found;
        }
      } else if (entry.name === `${entityName}.cs`) {
        return fullPath;
      }
    }
    return null;
  }
  return searchDir(CONFIG.entitiesRoot);
}

function parseEntityBase(entityFile) {
  const content = readUtf8(entityFile);
  if (content.includes(': TaktApprovalEntityBase')) {
    return 'TaktApprovalEntityBase';
  }
  if (content.includes(': TaktCompanyEntityBase')) {
    return 'TaktCompanyEntityBase';
  }
  return 'TaktTenantEntityBase';
}

/**
 * 从主响应 DTO 类声明解析继承的 DtoBase（TaktTenantDtoBase / TaktCompanyDtoBase / TaktApprovalDtoBase）
 * @param {string} dtoFile
 * @param {{ base: string|null }} dtoInfo
 * @returns {string|null}
 */
function parseDtoBase(dtoFile, dtoInfo) {
  if (!dtoInfo.base) {
    return null;
  }
  const content = readUtf8(dtoFile);
  const declRegex = new RegExp(`public\\s+class\\s+${dtoInfo.base}\\s*:\\s*(\\w+)`);
  const match = content.match(declRegex);
  if (!match) {
    return null;
  }
  const base = match[1];
  return DTO_BASE_NAMES.includes(base) ? base : null;
}

/**
 * QueryExpression / Options 等 lambda 的数据隔离前缀
 * @param {string} dtoBase
 * @param {string} varName 实体参数名（如 holiday）
 * @returns {string[]}
 */
function buildIsolationFilterLines(dtoBase, varName) {
  if (dtoBase === 'TaktTenantDtoBase') {
    return [`        return ${varName} => ${varName}.TenantCode == CurrentTenantCode`];
  }
  return [
    `        return ${varName} => ${varName}.TenantCode == CurrentTenantCode`,
    `                    && ${varName}.CompanyCode == CurrentCompanyCode`,
  ];
}

/**
 * GetById 等详情校验：租户/公司不匹配则视为不存在
 * @param {string} dtoBase
 */
function buildEntityScopeGuard(dtoBase) {
  if (dtoBase === 'TaktTenantDtoBase') {
    return 'entity.TenantCode != CurrentTenantCode';
  }
  return 'entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode';
}

/**
 * Options 列表查询 predicate
 * @param {string} dtoBase
 */
function buildOptionsListPredicate(dtoBase) {
  if (dtoBase === 'TaktTenantDtoBase') {
    return 'x => x.TenantCode == CurrentTenantCode';
  }
  return 'x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode';
}

/**
 * 写入选项方法前的上下文校验
 * @param {string} dtoBase
 */
function buildEnsureContextLine(dtoBase) {
  if (dtoBase === 'TaktTenantDtoBase') {
    return '';
  }
  return '        EnsureThreeLayerContext();\n';
}

function getEntityNamespace(entityFile) {
  const content = readUtf8(entityFile);
  const nsMatch = content.match(/namespace\s+([\w.]+);/);
  return nsMatch ? nsMatch[1] : 'Takt.Domain.Entities';
}

function getModuleRelativePath(entityFile) {
  const relativePath = path.relative(CONFIG.entitiesRoot, entityFile);
  const parts = relativePath.split(path.sep);
  parts.pop();
  return parts;
}

function buildNamespace(prefix, moduleParts) {
  let ns = prefix;
  for (const part of moduleParts) {
    ns += `.${part}`;
  }
  return ns;
}

function extractEntityDescription(entityFile) {
  if (!entityFile) {
    return null;
  }
  const content = readUtf8(entityFile);
  const sugarTableMatch = content.match(/SugarTable\([^,]*,\s*"([^"]+)"/);
  if (sugarTableMatch) {
    return sugarTableMatch[1].replace(/表$/, '');
  }
  const xmlMatch = content.match(/\/\/\/\s*<summary>\s*\n\s*\/\/\/\s*(.+?)\s*\n\s*\/\/\/\s*<\/summary>/s);
  if (xmlMatch) {
    return xmlMatch[1].trim().replace(/实体$/, '');
  }
  return null;
}

function identifyCrudType(entityFile) {
  if (!entityFile) {
    return 'Single';
  }
  const content = readUtf8(entityFile);
  if (/\[Navigate\(\s*NavigateType\.OneToMany/.test(content)) {
    return 'MasterDetail';
  }
  if (/public\s+\w+\??\s+ParentId\s*\{/.test(content)) {
    return 'Tree';
  }
  return 'Single';
}

function parseNavigateForeignKeyFromSegment(segment) {
  const matches = [...segment.matchAll(/nameof\(\w+\.(\w+)\)/g)];
  if (!matches.length) {
    return null;
  }
  return matches[matches.length - 1][1];
}

function isEntityLongPropertyNullable(entityFile, propName) {
  const content = readUtf8(entityFile);
  return new RegExp(`public\\s+long\\?\\s+${propName}\\s*\\{`).test(content);
}

/**
 * DTO 类（含基类）上是否为可空 long 外键
 * @param {string} dtoContent
 * @param {string} className
 * @param {string} propName
 * @returns {boolean}
 */
function isDtoClassLongPropertyNullable(dtoContent, className, propName) {
  if (!dtoContent || !className) {
    return false;
  }
  const block = extractClassBlock(dtoContent, className);
  if (block && new RegExp(`public\\s+long\\?\\s+${propName}\\s*\\{`).test(block)) {
    return true;
  }
  const startRegex = new RegExp(
    `public\\s+(?:partial\\s+)?class\\s+${className}\\b[^\\{]*(?::\\s*(\\w+))?`,
  );
  const startMatch = startRegex.exec(dtoContent);
  if (startMatch?.[1]) {
    return isDtoClassLongPropertyNullable(dtoContent, startMatch[1], propName);
  }
  return false;
}

/**
 * ManyToOne Stamp 方法参数为 CreateDto，仅依据 CreateDto 判定外键是否 long?
 * @param {string} dtoContent
 * @param {object} dtoInfo
 * @param {string} propName
 * @returns {boolean}
 */
function isStampLongIdFieldNullable(dtoContent, dtoInfo, propName) {
  if (!dtoInfo?.create) {
    return false;
  }
  return isDtoClassLongPropertyNullable(dtoContent, dtoInfo.create, propName);
}

function wrapMasterIdExpr(entityFile, entityVar, fieldName) {
  const expr = `${entityVar}.${fieldName}`;
  if (isEntityLongPropertyNullable(entityFile, fieldName)) {
    return `${expr}.GetValueOrDefault()`;
  }
  return expr;
}

function splitClassBodyByNavigationRegion(classBody) {
  const lines = classBody.split('\n');
  let markerLineIdx = -1;
  for (let i = 0; i < lines.length; i += 1) {
    if (lines[i].includes(NAVIGATION_REGION_MARKER)) {
      markerLineIdx = i;
      break;
    }
  }
  if (markerLineIdx === -1) {
    return { scalarBody: classBody, navigationBody: '' };
  }
  let navStartLine = markerLineIdx;
  while (navStartLine > 0 && /^\s*\/\/\s*=+/.test(lines[navStartLine - 1])) {
    navStartLine -= 1;
  }
  return {
    scalarBody: lines.slice(0, navStartLine).join('\n'),
    navigationBody: lines.slice(navStartLine).join('\n'),
  };
}

function parseOneToManyNavigations(entityFile) {
  const content = readUtf8(entityFile);
  const classMatch = content.match(/public\s+class\s+(Takt\w+)\s*:\s*\w+/);
  if (!classMatch) {
    return [];
  }
  const classBody = extractClassBlock(content, classMatch[1]);
  if (!classBody) {
    return [];
  }
  const { navigationBody } = splitClassBodyByNavigationRegion(classBody);
  const body = navigationBody.trim() ? navigationBody : classBody;
  const navRegex =
    /\[Navigate\(\s*NavigateType\.OneToMany\s*,\s*nameof\(\w+\.(\w+)\)\s*\)\]([\s\S]*?)public\s+List<(Takt\w+)>\??\s+(\w+)\s*\{\s*get;\s*set;/g;
  const navigations = [];
  let match;
  while ((match = navRegex.exec(body)) !== null) {
    const foreignKeyOnChild = match[1];
    const childEntity = match[3];
    const navPropName = match[4];
    const childShort = childEntity.replace(/^Takt/, '');
    navigations.push({
      navPropName,
      childEntity,
      childShort,
      foreignKeyOnChild,
    });
  }
  return navigations;
}

function getChildStampFields(childEntityFile, foreignKeyOnChild, masterIdField) {
  const props = extractEntityPropertyNames(childEntityFile);
  const stamps = [];
  // 外键由 masterIdField 单独赋 entity.Id，不可再从主表拷贝同名字段（如 GenTableId 主表不存在）
  if (props.has('TenantCode')) {
    stamps.push('TenantCode');
  }
  if (props.has('CompanyCode')) {
    stamps.push('CompanyCode');
  }
  return stamps.filter((f) => f !== masterIdField && f !== foreignKeyOnChild);
}

/** 子表上的主表 Id 外键字段名（如 DictData.DictTypeId、SalesOrderItem.SalesOrderId） */
function resolveChildMasterIdField(childEntityFile, foreignKeyOnChild, entityShort) {
  const props = extractEntityPropertyNames(childEntityFile);
  if (foreignKeyOnChild?.endsWith('Code')) {
    const derived = foreignKeyOnChild.replace(/Code$/, 'Id');
    if (props.has(derived)) {
      return derived;
    }
  }
  if (foreignKeyOnChild && props.has(foreignKeyOnChild)) {
    return foreignKeyOnChild;
  }
  const byMasterShort = `${entityShort}Id`;
  if (props.has(byMasterShort)) {
    return byMasterShort;
  }
  return foreignKeyOnChild || null;
}

function buildChildLinkPredicate(childVar, entityVar, foreignKeyOnChild, childFkField) {
  const fk = childFkField || foreignKeyOnChild;
  if (!fk) {
    return `${childVar}.Id == ${entityVar}.Id`;
  }
  if (fk.endsWith('Code')) {
    return `${childVar}.${fk} == ${entityVar}.${fk}`;
  }
  return `${childVar}.${fk} == ${entityVar}.Id`;
}

/**
 * 子表保存时写入主表外键（*Id → entity.Id；*Code → entity.*Code）
 * @param {string} childVar
 * @param {string} entityVar
 * @param {string} fkField
 */
function buildChildForeignKeyAssignment(childVar, entityVar, fkField) {
  if (!fkField) {
    return null;
  }
  if (fkField.endsWith('Code')) {
    return `${childVar}.${fkField} = ${entityVar}.${fkField};`;
  }
  return `${childVar}.${fkField} = ${entityVar}.Id;`;
}

function parseManyToOneMasterRecord(entityFile, fkField, masterEntity) {
  const masterShort = masterEntity.replace(/^Takt/, '');
  const masterFile = findEntityFile(masterEntity);
  if (!masterFile) {
    return null;
  }
  const masterBase = parseEntityBase(masterFile);
  const masterRepoInterface = ENTITY_BASE_TO_REPOSITORY[masterBase];
  if (!masterRepoInterface) {
    return null;
  }
  const masterIdField = resolveChildMasterIdField(entityFile, fkField, masterShort);
  if (!masterIdField) {
    return null;
  }
  return {
    fkField,
    masterEntity,
    masterShort,
    masterIdField,
    masterRepoInterface,
    masterRepoField: repositoryFieldName(masterShort),
  };
}

/**
 * 解析实体上全部 ManyToOne 导航（按声明顺序）
 * @param {string} entityFile
 * @returns {Array<object>}
 */
function parseManyToOneMasters(entityFile) {
  const content = readUtf8(entityFile);
  const classMatch = content.match(/public\s+class\s+(Takt\w+)\s*:\s*\w+/);
  if (!classMatch) {
    return [];
  }
  const classBody = extractClassBlock(content, classMatch[1]);
  if (!classBody) {
    return [];
  }
  const { navigationBody } = splitClassBodyByNavigationRegion(classBody);
  const body = navigationBody.trim() ? navigationBody : classBody;
  const navRegex =
    /\[Navigate\(\s*NavigateType\.ManyToOne\s*,\s*nameof\((\w+)\)\s*\)\][\s\S]*?public\s+(Takt\w+)\??\s+\w+\s*\{/g;
  const masters = [];
  const seenFk = new Set();
  let navMatch;
  while ((navMatch = navRegex.exec(body)) !== null) {
    const fkField = navMatch[1];
    if (seenFk.has(fkField)) {
      continue;
    }
    seenFk.add(fkField);
    const record = parseManyToOneMasterRecord(entityFile, fkField, navMatch[2]);
    if (record) {
      masters.push(record);
    }
  }
  return masters;
}

/** @deprecated 使用 parseManyToOneMasters；保留首条以兼容旧调用 */
function parseManyToOneMaster(entityFile) {
  const masters = parseManyToOneMasters(entityFile);
  return masters.length ? masters[0] : null;
}

function buildManyToOneStampMethodBody(master, childEntityFile, masterDesc, dtoContent, dtoInfo) {
  const stampFields = getChildStampFields(childEntityFile, master.fkField, master.masterIdField);
  const stampSync = stampFields
    .map(
      (field) => `        if (string.IsNullOrEmpty(entity.${field}))
        {
            entity.${field} = master.${field};
        }`,
    )
    .join('\n');
  const fk = master.fkField;
  const dtoLookupField = master.masterIdField || fk;
  if (fk.endsWith('Id')) {
    const nullableLongId = isStampLongIdFieldNullable(dtoContent, dtoInfo, dtoLookupField);
    const idGuard = nullableLongId
      ? `if (dto.${dtoLookupField} is not > 0)`
      : `if (dto.${dtoLookupField} <= 0)`;
    const idArg = nullableLongId ? `dto.${dtoLookupField}.Value` : `dto.${dtoLookupField}`;
    return `        ${idGuard}
        {
            return;
        }
        var master = await ${master.masterRepoField}.GetByIdAsync(${idArg});
        if (master == null)
        {
            throw new TaktBusinessException("${masterDesc}不存在");
        }
        entity.${fk} = master.Id;
${stampSync ? `${stampSync}\n` : ''}`;
  }
  const idAssign =
    master.masterIdField && master.masterIdField !== fk
      ? `        entity.${master.masterIdField} = master.Id;\n`
      : '';
  return `        if (string.IsNullOrEmpty(dto.${fk}))
        {
            return;
        }
        var master = await ${master.masterRepoField}.FirstAsync(x => x.${fk} == dto.${fk} && x.TenantCode == CurrentTenantCode);
        if (master == null)
        {
            throw new TaktBusinessException("${masterDesc}不存在");
        }
        entity.${fk} = master.${fk};
${idAssign}${stampSync ? `${stampSync}\n` : ''}`;
}

function generateManyToOneMasterStampExtras(entityFile, entityName, entityShort, dtoInfo, desc, dtoContent) {
  const masters = parseManyToOneMasters(entityFile);
  if (!masters.length) {
    return null;
  }

  const seenRepoFields = new Set();
  const stampParts = [];
  const extraUsings = [];
  for (const master of masters) {
    if (seenRepoFields.has(master.masterRepoField)) {
      continue;
    }
    seenRepoFields.add(master.masterRepoField);
    const masterEntityFile = findEntityFile(master.masterEntity);
    const masterDesc = extractEntityDescription(masterEntityFile) || master.masterShort;
    const stampBody = buildManyToOneStampMethodBody(
      master,
      entityFile,
      masterDesc,
      dtoContent,
      dtoInfo,
    );
    const masterEntityNs = masterEntityFile ? getEntityNamespace(masterEntityFile) : null;
    if (masterEntityNs) {
      extraUsings.push(masterEntityNs);
    }
    const repoParam = `${master.masterShort.charAt(0).toLowerCase()}${master.masterShort.slice(1)}Repository`;
    stampParts.push({
      master,
      masterDesc,
      stampBody,
      repoParam,
    });
  }
  if (!stampParts.length) {
    return null;
  }

  return {
    masterEntity: stampParts[0].master.masterEntity,
    masterShort: stampParts[0].master.masterShort,
    masterEntities: stampParts.map((part) => part.master.masterEntity),
    extraUsings,
    ctorFields: stampParts
      .map(
        (part) =>
          `    private readonly ${part.master.masterRepoInterface}<${part.master.masterEntity}> ${part.master.masterRepoField};`,
      )
      .join('\n'),
    ctorParams: stampParts
      .map(
        (part) =>
          `        ${part.master.masterRepoInterface}<${part.master.masterEntity}> ${part.repoParam},`,
      )
      .join('\n'),
    ctorParamDocs: stampParts
      .map(
        (part) =>
          `    /// <param name="${part.repoParam}">${part.masterDesc}仓储</param>`,
      )
      .join('\n'),
    ctorAssigns: stampParts
      .map((part) => `        ${part.master.masterRepoField} = ${part.repoParam};`)
      .join('\n'),
    privateMethods: stampParts
      .map(
        (part) => `${buildMethodXmlDoc({
          summary: `同步${desc}主表外键（ManyToOne → ${part.masterDesc}）`,
          params: [
            { name: 'entity', desc: '当前实体' },
            { name: 'dto', desc: '创建 DTO' },
          ],
          returns: '任务',
        }).trimEnd()}
    private async Task Stamp${entityShort}${part.master.masterShort}Async(${entityName} entity, ${dtoInfo.create} dto)
    {
${part.stampBody}    }
`,
      )
      .join('\n'),
    createBeforeSave: stampParts
      .map((part) => `await Stamp${entityShort}${part.master.masterShort}Async(entity, dto);`)
      .join('\n        '),
    updateBeforeSave: stampParts
      .map((part) => `await Stamp${entityShort}${part.master.masterShort}Async(entity, dto);`)
      .join('\n        '),
    importBeforeSave: stampParts
      .map(
        (part) => `await Stamp${entityShort}${part.master.masterShort}Async(entity, importDto);`,
      )
      .join('\n                '),
  };
}

/**
 * 主子表（OneToMany）服务级联代码块
 * @param {string} entityFile
 * @param {string} entityName
 * @param {string} entityShort
 * @param {object} dtoInfo
 * @param {string} dtoBase
 * @param {string} masterRepoField
 */
function generateMasterDetailServiceExtras(
  entityFile,
  entityName,
  entityShort,
  dtoInfo,
  dtoBase,
  masterRepoField,
  desc,
) {
  const rawChildren = parseOneToManyNavigations(entityFile).filter(
    (nav) => !isRbacJunctionEntity(nav.childShort),
  );
  if (!rawChildren.length) {
    return null;
  }

  const children = rawChildren
    .map((nav) => {
      const childFile = findEntityFile(nav.childEntity);
      if (!childFile || !nav.foreignKeyOnChild) {
        return null;
      }
      const childBase = parseEntityBase(childFile);
      const childRepoInterface = ENTITY_BASE_TO_REPOSITORY[childBase];
      if (!childRepoInterface) {
        return null;
      }
      const childModuleParts = getModuleRelativePath(childFile);
      const childDtoNs = buildNamespace('Takt.Application.Dtos', childModuleParts);
      const masterIdField = resolveChildMasterIdField(
        childFile,
        nav.foreignKeyOnChild,
        entityShort,
      );
      const stampFields = getChildStampFields(childFile, nav.foreignKeyOnChild, masterIdField);
      const childSeq = analyzeEntitySequenceFields(childFile, nav.childShort, 'Single', childBase);
      return {
        ...nav,
        childFile,
        childBase,
        childRepoInterface,
        childRepoField: repositoryFieldName(nav.childShort),
        childDtoNs,
        childResponseDto: `Takt${nav.childShort}Dto`,
        childCreateDto: `Takt${nav.childShort}CreateDto`,
        masterIdField,
        stampFields,
        childSeq,
        linkPredicate: buildChildLinkPredicate(
          'x',
          'entity',
          nav.foreignKeyOnChild,
          masterIdField,
        ),
      };
    })
    .filter(Boolean);

  if (!children.length) {
    return null;
  }

  const extraUsings = [
    ...new Set(
      children
        .flatMap((c) => [c.childDtoNs, c.childFile ? getEntityNamespace(c.childFile) : null])
        .filter((ns) => ns),
    ),
  ];

  const ctorFields = children
    .map(
      (c) =>
        `    private readonly ${c.childRepoInterface}<${c.childEntity}> ${c.childRepoField};`,
    )
    .join('\n');

  const ctorParams = children
    .map((c) => {
      const paramName = `${c.childShort.charAt(0).toLowerCase()}${c.childShort.slice(1)}Repository`;
      return `        ${c.childRepoInterface}<${c.childEntity}> ${paramName},`;
    })
    .join('\n');

  const ctorParamDocs = children
    .map((c) => `    /// <param name="${c.childShort.charAt(0).toLowerCase()}${c.childShort.slice(1)}Repository">${c.childShort}仓储</param>`)
    .join('\n');

  const ctorAssigns = children
    .map((c) => {
      const paramName = `${c.childShort.charAt(0).toLowerCase()}${c.childShort.slice(1)}Repository`;
      return `        ${c.childRepoField} = ${paramName};`;
    })
    .join('\n');

  const childDescSummary = children
    .map((c) => extractEntityDescription(c.childFile) || c.navPropName)
    .join('、');

  const fillDoc = buildMethodXmlDoc({
    summary: `填充${desc}详情（加载 OneToMany 子表：${childDescSummary}）`,
    params: [
      { name: 'dto', desc: '响应 DTO' },
      { name: 'entity', desc: '主表实体' },
    ],
    returns: '任务',
  });

  const saveDoc = buildMethodXmlDoc({
    summary: `保存${desc}子表级联（${childDescSummary}；Create/Update 后按主表 Id 先删后插）`,
    params: [
      { name: 'entity', desc: '主表实体' },
      { name: 'dto', desc: '创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）' },
    ],
    returns: '任务',
  });

  const fillMethodLines = [];
  fillMethodLines.push(fillDoc.trimEnd());
  fillMethodLines.push(`    private async Task Fill${entityShort}DetailsAsync(${dtoInfo.base} dto, ${entityName} entity)`);
  fillMethodLines.push('    {');
  fillMethodLines.push('        if (dto == null)');
  fillMethodLines.push('        {');
  fillMethodLines.push('            return;');
  fillMethodLines.push('        }');
  for (const c of children) {
    const childDesc = extractEntityDescription(c.childFile) || c.childEntity;
    fillMethodLines.push(`        // ${childDesc} → dto.${c.navPropName}`);
    fillMethodLines.push(`        var ${c.navPropName.toLowerCase()} = await ${c.childRepoField}.GetListAsync(x => ${c.linkPredicate});`);
    fillMethodLines.push(`        dto.${c.navPropName} = ${c.navPropName.toLowerCase()}.Adapt<List<${c.childResponseDto}>>();`);
  }
  fillMethodLines.push('    }');
  fillMethodLines.push('');

  const saveMethodLines = [];
  saveMethodLines.push(saveDoc.trimEnd());
  saveMethodLines.push(`    private async Task Save${entityShort}ChildrenAsync(${entityName} entity, ${dtoInfo.create} dto)`);
  saveMethodLines.push('    {');
  for (const c of children) {
    const childDesc = extractEntityDescription(c.childFile) || c.childEntity;
    const childUniqueIndexes = extractUniqueIndexes(c.childFile, c.childBase);
    const listVar = c.navPropName.toLowerCase();
    saveMethodLines.push(`        // ${childDesc}（${c.navPropName}）`);
    saveMethodLines.push(`        if (dto.${c.navPropName} is not { Count: > 0 })`);
    saveMethodLines.push('        {');
    saveMethodLines.push(`            await ${c.childRepoField}.DeleteAsync(x => ${c.linkPredicate});`);
    saveMethodLines.push('        }');
    saveMethodLines.push('        else');
    saveMethodLines.push('        {');
    saveMethodLines.push(`            var ${listVar} = dto.${c.navPropName}.Adapt<List<${c.childEntity}>>();`);
    saveMethodLines.push(`            foreach (var child in ${listVar})`);
    saveMethodLines.push('            {');
    if (c.masterIdField) {
      const fkAssign = buildChildForeignKeyAssignment('child', 'entity', c.masterIdField);
      if (fkAssign) {
        saveMethodLines.push(`                ${fkAssign}`);
      }
    }
    for (const field of c.stampFields) {
      saveMethodLines.push(`                child.${field} = entity.${field};`);
    }
    if (c.foreignKeyOnChild && c.foreignKeyOnChild !== c.masterIdField) {
      saveMethodLines.push(`                child.${c.foreignKeyOnChild} = entity.${c.foreignKeyOnChild};`);
    }
    saveMethodLines.push('            }');
    const childSortBlock = buildAssignChildSortOrdersInSave(c, 'entity', listVar, '            ', dtoBase);
    if (childSortBlock) {
      for (const line of childSortBlock.split('\n').filter((l) => l.length > 0)) {
        saveMethodLines.push(line);
      }
    }
    const childLineBlock = buildAssignChildLineNumbersInSave(
      c,
      'entity',
      listVar,
      '            ',
      entityFile,
      entityShort,
      dtoBase,
    );
    if (childLineBlock) {
      for (const line of childLineBlock.split('\n').filter((l) => l.length > 0)) {
        saveMethodLines.push(line);
      }
    }
    for (const line of buildChildBatchDuplicateGuardLines(childUniqueIndexes, listVar, childDesc)) {
      saveMethodLines.push(`            ${line}`);
    }
    saveMethodLines.push(`            await ${c.childRepoField}.DeleteAsync(x => ${c.linkPredicate});`);
    saveMethodLines.push(`            foreach (var child in ${listVar})`);
    saveMethodLines.push('            {');
    const childUniqueBlock = buildUniqueValidationBlock(
      childUniqueIndexes,
      c.childRepoField,
      childDesc,
      'child',
      'child',
    );
    if (childUniqueBlock) {
      for (const line of childUniqueBlock.split('\n').filter(Boolean)) {
        saveMethodLines.push(line);
      }
    }
    saveMethodLines.push('            }');
    saveMethodLines.push(`            await ${c.childRepoField}.CreateRangeAsync(${listVar});`);
    saveMethodLines.push('        }');
  }
  saveMethodLines.push('    }');
  saveMethodLines.push('');

  const deletePrefixLines = [];
  deletePrefixLines.push(`        var entity = await ${masterRepoField}.GetByIdAsync(id);`);
  deletePrefixLines.push('        if (entity == null)');
  deletePrefixLines.push('        {');
  deletePrefixLines.push(`            throw new TaktBusinessException("${desc}不存在或已删除");`);
  deletePrefixLines.push('        }');
  if (entityHasIsBuiltIn(entityFile)) {
    deletePrefixLines.push(buildBuiltInDeleteGuardLines(desc).trimEnd());
  }
  for (const c of children) {
    deletePrefixLines.push(`        await ${c.childRepoField}.DeleteAsync(x => ${c.linkPredicate});`);
  }

  return {
    children,
    extraUsings,
    ctorFields,
    ctorParams,
    ctorParamDocs,
    ctorAssigns,
    privateMethods: [...fillMethodLines, ...saveMethodLines].join('\n'),
    getByIdReturn: `        var dto = entity.Adapt<${dtoInfo.base}>();\n        await Fill${entityShort}DetailsAsync(dto, entity);\n        return dto;`,
    createAfterSave: `        await Save${entityShort}ChildrenAsync(entity, dto);`,
    updateAfterSave: `        await Save${entityShort}ChildrenAsync(entity, dto);`,
    deletePrefix: deletePrefixLines.join('\n'),
    skipDirectDelete: true,
  };
}

function getNameFieldName(entityFile) {
  const content = readUtf8(entityFile);
  const stringRegex = /public\s+string\??\s+(\w+)\s*\{/g;
  const standard = new Set([
    'TenantCode',
    'CompanyCode',
    'ExtFieldJson',
    'Remark',
    'CreatedBy',
    'UpdatedBy',
    'DeletedBy',
    'ApprovalOpinion',
  ]);
  const names = [];
  let m;
  while ((m = stringRegex.exec(content)) !== null) {
    names.push(m[1]);
  }
  const nameField = names.find((f) => f.endsWith('Name') && !standard.has(f));
  if (nameField) {
    return nameField;
  }
  const codeField = names.find((f) => f.endsWith('Code') && !standard.has(f));
  if (codeField) {
    return codeField;
  }
  return names.find((f) => !standard.has(f)) || 'Id';
}

function extractEntityPropertyNames(entityFile) {
  const content = readUtf8(entityFile);
  const propRegex = /public\s+[\w<>,\?\s]+\s+(\w+)\s*\{/g;
  const names = new Set();
  let m;
  while ((m = propRegex.exec(content)) !== null) {
    names.add(m[1]);
  }
  return names;
}

function entityHasIntProperty(entityFile, propName) {
  const content = readUtf8(entityFile);
  return new RegExp(`public\\s+int\\s+${propName}\\s*\\{`).test(content);
}

function entityHasParentId(entityFile) {
  const content = readUtf8(entityFile);
  return /public\s+long(?:\?)?\s+ParentId\s*\{/.test(content);
}

/**
 * 租户/公司隔离谓词（与 buildOptionsListPredicate 一致）
 * @param {string} dtoBase
 * @param {string} varName
 */
function buildTenantCompanyScope(dtoBase, varName) {
  if (dtoBase === 'TaktTenantDtoBase') {
    return `${varName}.TenantCode == CurrentTenantCode`;
  }
  return `${varName}.TenantCode == CurrentTenantCode && ${varName}.CompanyCode == CurrentCompanyCode`;
}

/**
 * 主表/当前实体上的业务编码字段（用于行号生成）
 * @param {string} entityFile
 * @param {string} entityShort 不含 Takt 前缀
 */
function resolvePrimaryBusinessCodeField(entityFile, entityShort) {
  const props = extractEntityPropertyNames(entityFile);
  const exact = `${entityShort}Code`;
  if (props.has(exact)) {
    return exact;
  }
  const skip = new Set(['TenantCode', 'CompanyCode', 'PlantCode', 'WarehouseCode', 'LocationCode']);
  const candidates = [...props].filter((p) => p.endsWith('Code') && !skip.has(p));
  const shortLower = entityShort.toLowerCase();
  const matched = candidates.find((p) => p.toLowerCase().includes(shortLower));
  return matched || candidates[0] || null;
}

/**
 * 解析 SortOrder 自动生成作用域
 * @param {string} entityFile
 * @param {string} crudType
 * @param {string} dtoBase
 */
function detectSortOrderConfig(entityFile, crudType, dtoBase) {
  if (!entityHasIntProperty(entityFile, 'SortOrder')) {
    return null;
  }
  if (crudType === 'Tree' || entityHasParentId(entityFile)) {
    return { mode: 'tree' };
  }
  const props = extractEntityPropertyNames(entityFile);
  if (props.has('GroupCode')) {
    const lineIndexes = extractUniqueIndexes(entityFile, dtoBase).filter((idx) =>
      idx.fields.includes('SortOrder'),
    );
    const masterIdField = lineIndexes.length
      ? lineIndexes[0].fields.find((f) => f.endsWith('Id') && f !== 'Id')
      : null;
    if (masterIdField) {
      return { mode: 'group', masterIdField, groupCodeField: 'GroupCode' };
    }
  }
  const sortIndexes = extractUniqueIndexes(entityFile, dtoBase).filter((idx) =>
    idx.fields.includes('SortOrder'),
  );
  let masterIdField = null;
  if (sortIndexes.length) {
    masterIdField = sortIndexes[0].fields.find((f) => f.endsWith('Id') && f !== 'Id') || null;
  }
  if (!masterIdField) {
    const content = readUtf8(entityFile);
    const idFields = [...content.matchAll(/public\s+long(?:\?)?\s+(\w+Id)\s*\{/g)]
      .map((m) => m[1])
      .filter((n) => n !== 'Id' && n !== 'ParentId' && props.has(n));
    masterIdField =
      idFields.find((n) => /TypeId$|CategoryId$|GroupId$|HeaderId$|MasterId$/.test(n)) ||
      idFields[0] ||
      null;
  }
  if (masterIdField) {
    return { mode: 'master', masterIdField };
  }
  return { mode: 'flat' };
}

/**
 * 解析 LineNumber 自动生成作用域
 * @param {string} entityFile
 * @param {string} dtoBase
 */
function detectLineNumberConfig(entityFile, dtoBase) {
  if (!entityHasIntProperty(entityFile, 'LineNumber')) {
    return null;
  }
  const props = extractEntityPropertyNames(entityFile);
  const lineIndexes = extractUniqueIndexes(entityFile, dtoBase).filter((idx) =>
    idx.fields.includes('LineNumber'),
  );
  let masterIdField = null;
  let groupCodeField = null;
  if (lineIndexes.length) {
    const fields = lineIndexes[0].fields;
    const linePos = fields.indexOf('LineNumber');
    masterIdField =
      fields.slice(0, linePos).find((f) => f.endsWith('Id') && f !== 'Id') || null;
    const between = fields.slice(0, linePos).filter((f) => f !== masterIdField && !f.endsWith('Id'));
    if (between.length === 1 && props.has(between[0]) && between[0] !== 'LineNumber') {
      groupCodeField = between[0];
    }
  }
  if (!masterIdField) {
    const content = readUtf8(entityFile);
    const idFields = [...content.matchAll(/public\s+long(?:\?)?\s+(\w+Id)\s*\{/g)]
      .map((m) => m[1])
      .filter((n) => n !== 'Id' && n !== 'ParentId' && props.has(n));
    masterIdField =
      idFields.find((n) => /ItemId$|OrderId$|OperationId$|StandardId$/.test(n)) || idFields[0] || null;
  }
  const prefix = masterIdField ? masterIdField.replace(/Id$/, '') : '';
  const businessCodeField = prefix && props.has(`${prefix}Code`) ? `${prefix}Code` : null;
  if (groupCodeField) {
    return {
      mode: 'group',
      masterIdField,
      businessCodeField,
      groupCodeField,
    };
  }
  return {
    mode: 'businessCode',
    masterIdField,
    businessCodeField,
  };
}

/**
 * @param {string} entityFile
 * @param {string} entityShort
 * @param {string} crudType
 * @param {string} dtoBase
 */
function analyzeEntitySequenceFields(entityFile, entityShort, crudType, dtoBase) {
  return {
    sortOrder: detectSortOrderConfig(entityFile, crudType, dtoBase),
    lineNumber: detectLineNumberConfig(entityFile, dtoBase),
  };
}

/**
 * @param {object} ctx
 */
function buildSequenceMetaForService(ctx) {
  const main = analyzeEntitySequenceFields(
    ctx.entityFile,
    ctx.entityShort,
    ctx.crudType,
    ctx.dtoBase,
  );
  const childAssignments = [];
  if (ctx.masterDetail?.children) {
    for (const c of ctx.masterDetail.children) {
      const childSeq = analyzeEntitySequenceFields(c.childFile, c.childShort, 'Single', c.childBase);
      if (childSeq.sortOrder || childSeq.lineNumber) {
        childAssignments.push({ child: c, seq: childSeq });
      }
    }
  }
  const needsSort = Boolean(main.sortOrder) || childAssignments.some((x) => x.seq.sortOrder);
  const needsLine = Boolean(main.lineNumber) || childAssignments.some((x) => x.seq.lineNumber);
  return { main, childAssignments, needsSort, needsLine };
}

/**
 * @param {object} sortConfig
 * @param {string} repoField
 * @param {string} dtoBase
 * @param {string} entityVar
 * @param {string} indent
 */
function buildAssignSortOrderBlock(sortConfig, repoField, dtoBase, entityVar, indent, entityFile) {
  const scope = buildTenantCompanyScope(dtoBase, 'x');
  let maxPredicate = scope;
  let generateCall;
  if (sortConfig.mode === 'tree') {
    maxPredicate = `${scope} && x.ParentId == ${entityVar}.ParentId`;
    generateCall = `_sortOrderGenerator.GenerateNext(${entityVar}.ParentId, maxSort)`;
  } else if (sortConfig.mode === 'master') {
    maxPredicate = `${scope} && x.${sortConfig.masterIdField} == ${entityVar}.${sortConfig.masterIdField}`;
    generateCall = `_sortOrderGenerator.GenerateNextForMaster(${wrapMasterIdExpr(entityFile, entityVar, sortConfig.masterIdField)}, maxSort)`;
  } else if (sortConfig.mode === 'group') {
    maxPredicate = `${scope} && x.${sortConfig.masterIdField} == ${entityVar}.${sortConfig.masterIdField} && x.${sortConfig.groupCodeField} == ${entityVar}.${sortConfig.groupCodeField}`;
    generateCall = `_sortOrderGenerator.GenerateNextForGroup(${wrapMasterIdExpr(entityFile, entityVar, sortConfig.masterIdField)}, ${entityVar}.${sortConfig.groupCodeField}, maxSort)`;
  } else {
    generateCall = '_sortOrderGenerator.GenerateNext(maxSort)';
  }
  return `${indent}if (${entityVar}.SortOrder <= 0)
${indent}{
${indent}    var maxSort = await ${repoField}.GetMaxIntAsync(
${indent}        x => ${maxPredicate},
${indent}        x => x.SortOrder);
${indent}    ${entityVar}.SortOrder = ${generateCall};
${indent}}
`;
}

/**
 * 导入（扁平 SortOrder）在循环外预取最大值，循环内递增
 */
function buildImportSortOrderDeclare(sortConfig, repoField, dtoBase) {
  if (!sortConfig || sortConfig.mode !== 'flat') {
    return '';
  }
  const scope = buildTenantCompanyScope(dtoBase, 'x');
  return `        var importSortMax = await ${repoField}.GetMaxIntAsync(
            x => ${scope},
            x => x.SortOrder);
`;
}

function buildImportSortOrderAssign(sortConfig, entityVar, indent) {
  return `${indent}if (${entityVar}.SortOrder <= 0)
${indent}{
${indent}    ${entityVar}.SortOrder = _sortOrderGenerator.GenerateNext(importSortMax);
${indent}    importSortMax = ${entityVar}.SortOrder;
${indent}}
`;
}

/**
 * @param {object} lineConfig
 * @param {string} repoField
 * @param {string} dtoBase
 * @param {string} entityVar
 * @param {string} indent
 */
function buildAssignLineNumberBlock(lineConfig, repoField, dtoBase, entityVar, indent) {
  const scope = buildTenantCompanyScope(dtoBase, 'x');
  let maxPredicate = scope;
  if (lineConfig.masterIdField) {
    maxPredicate = `${scope} && x.${lineConfig.masterIdField} == ${entityVar}.${lineConfig.masterIdField}`;
  }
  let businessCodeExpr;
  if (lineConfig.businessCodeField) {
    businessCodeExpr = `!string.IsNullOrWhiteSpace(${entityVar}.${lineConfig.businessCodeField}) ? ${entityVar}.${lineConfig.businessCodeField} : ${entityVar}.${lineConfig.masterIdField}.ToString()`;
  } else if (lineConfig.masterIdField) {
    businessCodeExpr = `${entityVar}.${lineConfig.masterIdField}.ToString()`;
  } else {
    businessCodeExpr = `${entityVar}.Id.ToString()`;
  }
  if (lineConfig.mode === 'group' && lineConfig.groupCodeField) {
    maxPredicate = `${maxPredicate} && x.${lineConfig.groupCodeField} == ${entityVar}.${lineConfig.groupCodeField}`;
    return `${indent}if (${entityVar}.LineNumber <= 0)
${indent}{
${indent}  var maxLine = await ${repoField}.GetMaxIntAsync(
${indent}      x => ${maxPredicate},
${indent}      x => x.LineNumber);
${indent}  var businessCode = ${businessCodeExpr};
${indent}  ${entityVar}.LineNumber = _lineNumberGenerator.GenerateNextForGroup(businessCode, ${entityVar}.${lineConfig.groupCodeField}, maxLine);
${indent}}
`;
  }
  return `${indent}if (${entityVar}.LineNumber <= 0)
${indent}{
${indent}    var maxLine = await ${repoField}.GetMaxIntAsync(
${indent}        x => ${maxPredicate},
${indent}        x => x.LineNumber);
${indent}    var businessCode = ${businessCodeExpr};
${indent}    ${entityVar}.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
${indent}}
`;
}

/**
 * 主子表 SaveChildren：批量分配子表 SortOrder
 */
function buildAssignChildSortOrdersInSave(c, masterVar, listVar, indent, dtoBase) {
  const sortConfig = c.childSeq?.sortOrder;
  if (!sortConfig) {
    return '';
  }
  const scope = buildTenantCompanyScope(dtoBase, 'x');
  const masterFk = c.masterIdField;
  let maxPredicate = `${scope} && x.${masterFk} == ${masterVar}.Id`;
  let sequenceCall;
  if (sortConfig.mode === 'master' && sortConfig.masterIdField === masterFk) {
    sequenceCall = `_sortOrderGenerator.GenerateSequenceForMaster(${masterVar}.Id, ${listVar}NeedSort.Count, maxSort)`;
  } else if (sortConfig.mode === 'tree') {
    maxPredicate = `${scope} && x.ParentId == ${masterVar}.Id`;
    sequenceCall = `_sortOrderGenerator.GenerateSequence(${masterVar}.Id, ${listVar}NeedSort.Count, maxSort)`;
  } else {
    sequenceCall = `_sortOrderGenerator.GenerateSequence(${listVar}NeedSort.Count, maxSort)`;
  }
  return `${indent}var ${listVar}NeedSort = ${listVar}.Where(c => c.SortOrder <= 0).ToList();
${indent}if (${listVar}NeedSort.Count > 0)
${indent}{
${indent}    var maxSort = await ${c.childRepoField}.GetMaxIntAsync(
${indent}        x => ${maxPredicate},
${indent}        x => x.SortOrder);
${indent}    var sortSeq = ${sequenceCall}.ToList();
${indent}    var sortIdx = 0;
${indent}    foreach (var child in ${listVar})
${indent}    {
${indent}        if (child.SortOrder <= 0)
${indent}        {
${indent}            child.SortOrder = sortSeq[sortIdx++];
${indent}        }
${indent}    }
${indent}}
`;
}

/**
 * 主子表 SaveChildren：批量分配子表 LineNumber
 */
function buildAssignChildLineNumbersInSave(c, masterVar, listVar, indent, parentEntityFile, parentShort, dtoBase) {
  const lineConfig = c.childSeq?.lineNumber;
  if (!lineConfig) {
    return '';
  }
  const masterCodeField = resolvePrimaryBusinessCodeField(parentEntityFile, parentShort);
  const masterFk = c.masterIdField;
  const scope = buildTenantCompanyScope(dtoBase, 'x');
  const maxPredicate = `${scope} && x.${masterFk} == ${masterVar}.Id`;
  let businessCodeFromMaster;
  if (masterCodeField) {
    businessCodeFromMaster = `!string.IsNullOrWhiteSpace(${masterVar}.${masterCodeField}) ? ${masterVar}.${masterCodeField} : ${masterVar}.Id.ToString()`;
  } else {
    businessCodeFromMaster = `${masterVar}.Id.ToString()`;
  }
  const childCodeAssign =
    lineConfig.businessCodeField && lineConfig.businessCodeField !== masterCodeField
      ? `${indent}        if (string.IsNullOrWhiteSpace(child.${lineConfig.businessCodeField}))
${indent}        {
${indent}            child.${lineConfig.businessCodeField} = ${businessCodeFromMaster};
${indent}        }
`
      : '';
  return `${indent}var ${listVar}NeedLine = ${listVar}.Where(c => c.LineNumber <= 0).ToList();
${indent}if (${listVar}NeedLine.Count > 0)
${indent}{
${indent}    var businessCode = ${businessCodeFromMaster};
${indent}    var maxLine = await ${c.childRepoField}.GetMaxIntAsync(
${indent}        x => ${maxPredicate},
${indent}        x => x.LineNumber);
${indent}    var lineSeq = _lineNumberGenerator.GenerateSequence(businessCode, ${listVar}NeedLine.Count, maxLine).ToList();
${indent}    var lineIdx = 0;
${indent}    foreach (var child in ${listVar})
${indent}    {
${childCodeAssign}${indent}        if (child.LineNumber <= 0)
${indent}        {
${indent}            child.LineNumber = lineSeq[lineIdx++];
${indent}        }
${indent}    }
${indent}}
`;
}

/**
 * 提取 DTO 类中的属性名
 * @param {string} dtoContent Dtos 文件内容
 * @param {string} className DTO 类名
 * @returns {Set<string>}
 */
function extractDtoPropertyNames(dtoContent, className) {
  const names = new Set();
  const startRegex = new RegExp(`public\\s+(?:partial\\s+)?class\\s+${className}\\b[^\\{]*(?::\\s*(\\w+))?`);
  const startMatch = startRegex.exec(dtoContent);
  if (!startMatch) {
    return names;
  }
  const baseClass = startMatch[1];
  if (baseClass) {
    for (const name of extractDtoPropertyNames(dtoContent, baseClass)) {
      names.add(name);
    }
  }
  const block = extractClassBlock(dtoContent, className);
  const propRegex = /public\s+[\w?<>,\s]+\s+(\w+)\s*\{/g;
  let m;
  while ((m = propRegex.exec(block)) !== null) {
    names.add(m[1]);
  }
  return names;
}

/**
 * 按 DTO 基类返回唯一索引中由仓储自动隔离的字段（不参与应用层查重条件）
 * @param {string} dtoBase TaktTenantDtoBase / TaktCompanyDtoBase / TaktApprovalDtoBase
 * @returns {Set<string>}
 */
function getUniqueIndexScopeFields(dtoBase) {
  const scopeFields = new Set(['TenantCode']);
  if (dtoBase === 'TaktCompanyDtoBase' || dtoBase === 'TaktApprovalDtoBase') {
    scopeFields.add('CompanyCode');
  }
  return scopeFields;
}

/**
 * 从实体文件解析 SugarIndex 唯一索引（末位参数为 true，名称以 _unique 结尾）
 * 索引字段顺序约定：TenantCode → CompanyCode（公司/审批级）→ 业务字段；末位 true/false 为唯一约束
 * @param {string} entityFile 实体文件路径
 * @param {string} dtoBase DTO 基类（决定 TenantCode/CompanyCode 是否作为隔离字段跳过查重）
 * @returns {Array<{ indexKey: string, fields: string[] }>}
 */
function extractUniqueIndexes(entityFile, dtoBase) {
  const content = readUtf8(entityFile);
  const scopeFields = getUniqueIndexScopeFields(dtoBase);
  const results = [];
  for (const line of content.split('\n')) {
    const trimmed = line.trim();
    if (!trimmed.startsWith('[SugarIndex(') || !/,\s*true\s*\)\]$/.test(trimmed)) {
      continue;
    }
    const indexKeyMatch = trimmed.match(/^\[SugarIndex\("([^"]+)"/);
    if (!indexKeyMatch) {
      continue;
    }
    const indexKey = indexKeyMatch[1];
    const fields = [];
    const fieldRegex = /nameof\((\w+)\)/g;
    let fm;
    while ((fm = fieldRegex.exec(trimmed)) !== null) {
      fields.push(fm[1]);
    }
    const validationFields = fields.filter(
      (f) => !UNIQUE_INDEX_SKIP_FIELDS.has(f) && !scopeFields.has(f),
    );
    if (validationFields.length === 0) {
      continue;
    }
    results.push({ indexKey, fields: validationFields });
  }
  const seen = new Set();
  return results.filter((item) => {
    const key = item.fields.join('|');
    if (seen.has(key)) {
      return false;
    }
    seen.add(key);
    return true;
  });
}

/**
 * 唯一索引变量名后缀（与 ix_*_unique 索引名对齐）
 * @param {string} indexKey
 * @returns {string}
 */
function sanitizeUniqueIndexVarSuffix(indexKey) {
  return indexKey.replace(/[^a-zA-Z0-9_]/g, '_');
}

/**
 * 组合唯一条件表达式（x.Field == valueVar.Field && …）
 * @param {string[]} fields 实体唯一索引业务字段
 * @param {string} valueVar 取值变量（entity / child）
 * @param {string} [lineIndent] 换行续行缩进
 * @returns {string}
 */
function buildUniquePredicate(fields, valueVar, lineIndent = '') {
  if (!fields.length) {
    return 'true';
  }
  return fields
    .map((f, i) => (i === 0 ? `x.${f} == ${valueVar}.${f}` : `${lineIndent}&& x.${f} == ${valueVar}.${f}`))
    .join('\n');
}

/**
 * 生成 _uniqueValidator.IsUniqueAsync 组合查重语句（Create/Update/Import/子表级联）
 * @param {Array<{ indexKey: string, fields: string[] }>} uniqueIndexes 实体唯一索引
 * @param {string} repoField 仓储字段名
 * @param {string} desc 实体描述
 * @param {'create'|'update'|'import'|'child'} mode
 * @param {string} [valueVar='entity'] 取值变量名
 * @returns {string}
 */
function buildUniqueValidationBlock(uniqueIndexes, repoField, desc, mode, valueVar = 'entity') {
  if (!uniqueIndexes.length) {
    return '';
  }
  const indent =
    mode === 'import' ? '                ' : mode === 'child' ? '            ' : '        ';
  const throwIndent =
    mode === 'import' ? '                    ' : mode === 'child' ? '                ' : '            ';
  const callIndent = `${indent}    `;
  const predIndent = mode === 'import' || mode === 'child' ? `${callIndent}    ` : `${callIndent}    `;
  let block = '';
  for (const idx of uniqueIndexes) {
    const fields = idx.fields;
    if (!fields.length) {
      continue;
    }
    const varSuffix = sanitizeUniqueIndexVarSuffix(idx.indexKey);
    const fieldLabel = fields.join('、');
    const predicate = buildUniquePredicate(fields, valueVar, `${predIndent}`);
    block += `${indent}var isUnique_${varSuffix} = await _uniqueValidator.IsUniqueAsync(\n`;
    block += `${callIndent}${repoField},\n`;
    if (mode === 'update') {
      block += `${callIndent}x => ${predicate},\n`;
      block += `${callIndent}id);\n`;
    } else {
      block += `${callIndent}x => ${predicate});\n`;
    }
    block += `${indent}if (!isUnique_${varSuffix})\n`;
    block += `${indent}{\n`;
    block += `${throwIndent}throw new TaktBusinessException("${desc}的${fieldLabel}已存在");\n`;
    block += `${indent}}\n`;
  }
  return block;
}

/**
 * 导入 Excel 批次内组合键去重（与实体唯一索引业务字段对齐）
 * @param {Array<{ indexKey: string, fields: string[] }>} uniqueIndexes
 * @param {string} valueVar 取值变量名
 * @returns {{ declare: string, check: string }}
 */
function buildImportBatchDuplicateGuard(uniqueIndexes, valueVar = 'entity') {
  if (!uniqueIndexes.length) {
    return { declare: '', check: '' };
  }
  const idx = uniqueIndexes[0];
  if (!idx.fields.length) {
    return { declare: '', check: '' };
  }
  const fieldLabel = idx.fields.join('、');
  const keyInterpol = idx.fields.map((f) => `{${valueVar}.${f}}`).join('|');
  return {
    declare: '        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);\n',
    check:
      `                var importKey = $"${keyInterpol}";\n` +
      '                if (!importSeenKeys.Add(importKey))\n' +
      '                {\n' +
      `                    throw new TaktBusinessException("与Excel中其他行重复（${fieldLabel}）");\n` +
      '                }\n',
  };
}

/**
 * 主子表 SaveChildren 内子表批次组合键去重
 * @param {Array<{ indexKey: string, fields: string[] }>} uniqueIndexes
 * @param {string} listVar 子表列表变量名
 * @param {string} itemLabel 错误消息中的项名称
 * @returns {string[]}
 */
function buildChildBatchDuplicateGuardLines(uniqueIndexes, listVar, itemLabel) {
  if (!uniqueIndexes.length) {
    return [];
  }
  const idx = uniqueIndexes[0];
  if (!idx.fields.length) {
    return [];
  }
  const fieldLabel = idx.fields.join('、');
  const keyInterpol = idx.fields.map((f) => `{${listVar}[i].${f}}`).join('|');
  return [
    '            var seenKeys = new HashSet<string>(StringComparer.Ordinal);',
    `            for (var i = 0; i < ${listVar}.Count; i++)`,
    '            {',
    `                var key = $"${keyInterpol}";`,
    '                if (!seenKeys.Add(key))',
    '                {',
    `                    throw new TaktBusinessException($"${itemLabel}第{i + 1}项与本次提交的其他项重复（${fieldLabel}）");`,
    '                }',
    '            }',
  ];
}

function extractQueryDtoProperties(dtoContent, queryDtoName, entityPropertyNames) {
  const block = extractClassBlock(dtoContent, queryDtoName);
  const props = [];
  const propRegex = /public\s+([\w?<>,\s]+)\s+(\w+)\s*\{/g;
  let m;
  while ((m = propRegex.exec(block)) !== null) {
    const rawType = m[1].trim();
    const name = m[2];
    if (PAGED_QUERY_FIELDS.has(name)) {
      continue;
    }
    if (name.endsWith('Start') || name.endsWith('End')) {
      continue;
    }
    if (!entityPropertyNames.has(name) && name !== 'Remark' && name !== 'ExtFieldJson') {
      continue;
    }
    props.push({
      name,
      rawType,
      isNullableEnum: /\?\s*$/.test(rawType) && !rawType.startsWith('string'),
      isEnum: !rawType.startsWith('string') && !rawType.includes('DateTime') && !rawType.includes('bool') && !rawType.includes('int') && !rawType.includes('long') && !rawType.includes('decimal'),
      isString: rawType.startsWith('string'),
      isDateTime: rawType.includes('DateTime'),
      isBool: rawType.includes('bool'),
      isNumeric: /^(int|long|decimal)/.test(rawType),
    });
  }
  return props;
}

function extractDateRangeFields(dtoContent, queryDtoName, entityPropertyNames) {
  const block = extractClassBlock(dtoContent, queryDtoName);
  const ranges = [];
  const propRegex = /public\s+DateTime\?\s+(\w+)\s*\{/g;
  const startEndNames = [];
  let m;
  while ((m = propRegex.exec(block)) !== null) {
    startEndNames.push(m[1]);
  }
  for (const fieldName of startEndNames) {
    if (fieldName.endsWith('Start')) {
      const baseName = fieldName.slice(0, -'Start'.length);
      const endField = `${baseName}End`;
      if (startEndNames.includes(endField) && (entityPropertyNames.has(baseName) || baseName === 'CreatedAt')) {
        ranges.push({ baseName, startField: fieldName, endField });
      }
    }
  }
  return ranges;
}

function parseStatusDtoFields(dtoContent, statusDtoName) {
  const block = extractClassBlock(dtoContent, statusDtoName);
  const idMatch = block.match(/public\s+long\s+(\w+Id)\s*\{/);
  const props = [];
  const propRegex = /public\s+([\w?<>,\s]+)\s+(\w+)\s*\{/g;
  let m;
  while ((m = propRegex.exec(block)) !== null) {
    if (m[2].endsWith('Id')) {
      continue;
    }
    props.push({ type: m[1].trim(), name: m[2] });
  }
  return {
    idProperty: idMatch ? idMatch[1] : null,
    valueProperties: props,
  };
}

function buildStatusMethodSuffix(statusDtoName, entityName) {
  let suffix = statusDtoName.replace(entityName, '').replace(/StatusDto$/, '');
  const entityShort = entityName.replace(/^Takt/, '');
  if (suffix.startsWith(entityShort)) {
    suffix = suffix.slice(entityShort.length);
  }
  if (!suffix) {
    suffix = 'Status';
  }
  return suffix;
}

function repositoryFieldName(entityShort) {
  return `_${entityShort.charAt(0).toLowerCase()}${entityShort.slice(1)}Repository`;
}

/**
 * 转置列头主表与 ManyToOne 主表相同时，避免重复注入同一仓储
 * @param {ReturnType<typeof generateManyToOneMasterStampExtras>|null} manyToOneMaster
 * @param {ReturnType<typeof generateTransposedServiceImplementation>|null} transposedGen
 * @param {string} entityShort
 */
function omitDuplicateTransposedCtorExtras(manyToOneMaster, transposedGen, entityShort) {
  if (!manyToOneMaster || !transposedGen) {
    return transposedGen;
  }
  const cfg = getTransposableConfig(entityShort);
  const stampMasterEntities =
    manyToOneMaster.masterEntities ??
    (manyToOneMaster.masterEntity ? [manyToOneMaster.masterEntity] : []);
  if (!cfg?.masterTable || !stampMasterEntities.includes(cfg.masterTable.entity)) {
    return transposedGen;
  }
  return {
    ...transposedGen,
    ctorFields: '',
    ctorParams: '',
    ctorAssigns: '',
    ctorParamDocs: '',
  };
}

/**
 * 树形实体上的「启用」状态字段（用于 includeDisabled 过滤）
 * @param {string} entityFile
 * @param {string} entityShort
 * @returns {{ field: string, type: 'enum'|'int' }|null}
 */
function extractTreeStatusField(entityFile, entityShort) {
  const content = readUtf8(entityFile);
  const enumMatch = content.match(
    new RegExp(`public\\s+TaktCommonStatus\\s+(${entityShort}\\w*Status|\\w+Status)\\s*\\{`),
  );
  if (enumMatch) {
    return { field: enumMatch[1], type: 'enum' };
  }
  const intMatch = content.match(
    new RegExp(`public\\s+int\\s+(${entityShort}\\w*Status|\\w+Status)\\s*\\{`),
  );
  if (intMatch) {
    return { field: intMatch[1], type: 'int' };
  }
  return null;
}

/**
 * 实体是否含 IsBuiltIn（种子内置项，禁止删除/禁用）
 * @param {string} entityFile
 * @returns {boolean}
 */
function entityHasIsBuiltIn(entityFile) {
  return extractEntityPropertyNames(entityFile).has('IsBuiltIn');
}

/**
 * 内置项「禁用/离职」保护所用的状态字段元数据
 * @param {string} entityFile
 * @returns {{ field: string, kind: 'commonStatus'|'intEnabled'|'employeeResigned' }|null}
 */
function extractBuiltInDisableStatusMeta(entityFile) {
  if (!entityHasIsBuiltIn(entityFile)) {
    return null;
  }
  const props = extractEntityPropertyNames(entityFile);
  if (props.has('EmployeeStatus')) {
    return { field: 'EmployeeStatus', kind: 'employeeResigned' };
  }
  const content = readUtf8(entityFile);
  const commonMatch = content.match(/public\s+TaktCommonStatus\s+(\w+Status)\s*\{/);
  if (commonMatch) {
    return { field: commonMatch[1], kind: 'commonStatus' };
  }
  const intMatch = content.match(/public\s+int\s+(\w+Status)\s*\{/);
  if (intMatch) {
    return { field: intMatch[1], kind: 'intEnabled' };
  }
  return null;
}

/**
 * 创建时强制非内置
 * @returns {string}
 */
function buildBuiltInCreateAssignLine() {
  return '        entity.IsBuiltIn = TaktYesNo.No;\n';
}

/**
 * 更新前保存 IsBuiltIn（及员工状态）快照
 * @param {{ field: string, kind: string }|null} builtInStatusMeta
 * @returns {string}
 */
function buildBuiltInUpdateBeforeAdaptLines(builtInStatusMeta) {
  let block = '        var originalIsBuiltIn = entity.IsBuiltIn;\n';
  if (builtInStatusMeta?.kind === 'employeeResigned') {
    block += '        var originalEmployeeStatus = entity.EmployeeStatus;\n';
  }
  return block;
}

/**
 * 更新后恢复 IsBuiltIn，并校验内置员工不可离职/退休
 * @param {string} desc
 * @param {{ field: string, kind: string }|null} builtInStatusMeta
 * @returns {string}
 */
function buildBuiltInUpdateAfterAdaptLines(desc, builtInStatusMeta) {
  let block = '        entity.IsBuiltIn = originalIsBuiltIn;\n';
  if (builtInStatusMeta?.kind === 'employeeResigned') {
    block += `        if (entity.IsBuiltIn == TaktYesNo.Yes && entity.EmployeeStatus != originalEmployeeStatus
            && (entity.EmployeeStatus == 3 || entity.EmployeeStatus == 4))
        {
            throw new TaktBusinessException("不允许将内置${desc}设为离职或退休");
        }
`;
  }
  return block;
}

/**
 * 删除内置项保护
 * @param {string} desc
 * @returns {string}
 */
function buildBuiltInDeleteGuardLines(desc) {
  return `        if (entity.IsBuiltIn == TaktYesNo.Yes)
        {
            throw new TaktBusinessException("内置${desc}不允许删除");
        }
`;
}

/**
 * 批量删除前：所选 ID 中不得含内置项（整批拒绝，避免部分删除后失败）
 * @param {string} desc
 * @param {string} repoField
 * @returns {string}
 */
function buildBuiltInBatchDeleteGuardLines(desc, repoField) {
  return `        if (await ${repoField}.ExistsAsync(x => idList.Contains(x.Id) && x.IsBuiltIn == TaktYesNo.Yes))
        {
            throw new TaktBusinessException("内置${desc}不允许删除");
        }
`;
}

/**
 * 状态更新：禁止将内置项设为禁用/非启用
 * @param {string} desc
 * @param {{ field: string, kind: string }} builtInStatusMeta
 * @param {string} dtoPropName StatusDto 中的状态属性名
 * @returns {string}
 */
function buildBuiltInStatusDisableGuardLines(desc, builtInStatusMeta, dtoPropName) {
  if (!builtInStatusMeta || builtInStatusMeta.field !== dtoPropName) {
    return '';
  }
  if (builtInStatusMeta.kind === 'commonStatus') {
    return `        if (entity.IsBuiltIn == TaktYesNo.Yes && dto.${dtoPropName} != TaktCommonStatus.Enabled)
        {
            throw new TaktBusinessException("不允许禁用内置${desc}");
        }
`;
  }
  if (builtInStatusMeta.kind === 'intEnabled') {
    return `        if (entity.IsBuiltIn == TaktYesNo.Yes && dto.${dtoPropName} != (int)TaktCommonStatus.Enabled)
        {
            throw new TaktBusinessException("不允许禁用内置${desc}");
        }
`;
  }
  return '';
}

/**
 * 树形选项列表查询条件（仅启用项，用于 GetXxxTreeOptionsAsync）
 */
function buildTreeOptionsListPredicate(dtoBase, statusMeta) {
  const scope =
    dtoBase === 'TaktTenantDtoBase'
      ? 'x.TenantCode == CurrentTenantCode'
      : 'x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode';
  if (statusMeta?.type === 'enum') {
    return `x => ${scope} && x.${statusMeta.field} == TaktCommonStatus.Enabled`;
  }
  if (statusMeta?.type === 'int') {
    return `x => ${scope} && x.${statusMeta.field} == 1`;
  }
  return `x => ${scope}`;
}

/**
 * 生成 GetXxxTreeOptionsAsync、GetXxxTreeAsync 及对应 Build 私有方法（树形实体不生成 GetXxxOptionsAsync）
 */
function generateTreeServiceMethods(
  entityName,
  entityShort,
  dtoInfo,
  dtoBase,
  repoField,
  entityFile,
  desc,
  nameField,
) {
  const treeDto = dtoInfo.tree;
  const statusMeta = extractTreeStatusField(entityFile, entityShort);
  const entityContent = readUtf8(entityFile);
  const hasSortOrder = /public\s+int\s+SortOrder\s*\{/.test(entityContent);
  const orderField = hasSortOrder ? 'SortOrder' : 'Id';
  const listPredicate =
    dtoBase === 'TaktTenantDtoBase'
      ? 'x => x.TenantCode == CurrentTenantCode'
      : 'x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode';
  const treeOptionsPredicate = buildTreeOptionsListPredicate(dtoBase, statusMeta);
  const ensureLine =
    dtoBase === 'TaktTenantDtoBase' ? '' : '        EnsureThreeLayerContext();\n';

  let block = '';
  block += buildMethodXmlDoc({
    summary: `获取${desc}树形选项列表`,
    returns: '树形选项',
  });
  block += `    public async Task<List<TaktTreeSelectOption>> Get${entityShort}TreeOptionsAsync()\n`;
  block += '    {\n';
  block += ensureLine;
  block += `        var list = await ${repoField}.GetListAsync(${treeOptionsPredicate});\n`;
  block += `        return Build${entityShort}TreeOptions(list, 0);\n`;
  block += '    }\n\n';
  const treeOptionsBlock = block;
  block = '';
  block += buildMethodXmlDoc({
    summary: `在内存中构建${desc}树形选项（递归，按 ParentId）`,
  });
  block += `    private List<TaktTreeSelectOption> Build${entityShort}TreeOptions(List<${entityName}> all, long parentId)\n`;
  block += '    {\n';
  block += '        var result = new List<TaktTreeSelectOption>();\n';
  block += `        foreach (var item in all.Where(x => x.ParentId == parentId).OrderBy(x => x.${orderField}))\n`;
  block += '        {\n';
  block += '            var option = new TaktTreeSelectOption\n';
  block += '            {\n';
  block += '                DictValue = item.Id,\n';
  block += `                DictLabel = item.${nameField} ?? item.Id.ToString(),\n`;
  block += hasSortOrder
    ? '                SortOrder = item.SortOrder,\n'
    : '                SortOrder = 0,\n';
  block += '            };\n';
  block += `            var children = Build${entityShort}TreeOptions(all, item.Id);\n`;
  block += '            if (children.Count > 0)\n';
  block += '            {\n';
  block += '                option.Children = children;\n';
  block += '            }\n';
  block += '            result.Add(option);\n';
  block += '        }\n';
  block += '        return result;\n';
  block += '    }\n\n';

  let filterBlock;
  if (statusMeta?.type === 'enum') {
    filterBlock = `        var filtered = includeDisabled
            ? list
            : list.Where(x => x.${statusMeta.field} == TaktCommonStatus.Enabled).ToList();`;
  } else if (statusMeta?.type === 'int') {
    filterBlock = `        var filtered = includeDisabled
            ? list
            : list.Where(x => x.${statusMeta.field} == 1).ToList();`;
  } else {
    filterBlock = '        var filtered = list;';
  }

  block += buildMethodXmlDoc({
    summary: `获取${desc}树形列表`,
    params: [
      { name: 'parentId', desc: '父级ID' },
      { name: 'includeDisabled', desc: '是否包含禁用项' },
    ],
    returns: '树形列表',
  });
  block += `    public async Task<List<${treeDto}>> Get${entityShort}TreeAsync(long parentId = 0, bool includeDisabled = false)\n`;
  block += '    {\n';
  block += ensureLine;
  block += `        var list = await ${repoField}.GetListAsync(${listPredicate});\n`;
  block += `${filterBlock}\n`;
  block += `        return Build${entityShort}Tree(filtered, parentId);\n`;
  block += '    }\n\n';
  block += buildMethodXmlDoc({
    summary: `在内存中构建${desc}树（递归，按 ParentId）`,
  });
  block += `    private List<${treeDto}> Build${entityShort}Tree(List<${entityName}> allRecords, long parentId)\n`;
  block += '    {\n';
  block += '        var children = allRecords\n';
  block += `            .Where(x => x.ParentId == parentId)\n`;
  block += `            .OrderBy(x => x.${orderField})\n`;
  block += '            .ToList();\n';
  block += `        var treeList = new List<${treeDto}>();\n`;
  block += '        foreach (var item in children)\n';
  block += '        {\n';
  block += `            var treeDto = item.Adapt<${treeDto}>();\n`;
  block += `            var childTree = Build${entityShort}Tree(allRecords, item.Id);\n`;
  block += '            if (childTree.Count > 0)\n';
  block += '            {\n';
  block += '                treeDto.Children = childTree;\n';
  block += '            }\n';
  block += '            treeList.Add(treeDto);\n';
  block += '        }\n';
  block += '        return treeList;\n';
  block += '    }\n\n';
  return {
    treeOptionsBlock,
    treeRemainderBlock: block,
    block: treeOptionsBlock + block,
    needsEnumsUsing: statusMeta?.type === 'enum',
  };
}

function todayFileHeaderDate() {
  return new Date().toISOString().split('T')[0];
}

// ========================================
// 接口 / 实现生成
// ========================================

/**
 * 生成公共方法 XML 注释（实现类与 TaktLoginLogService 一致，禁止 inheritdoc）
 * @param {{ summary: string, params?: Array<{ name: string, desc: string }>, returns?: string }} doc
 * @returns {string}
 */
function buildMethodXmlDoc(doc) {
  let block = '    /// <summary>\n';
  block += `    /// ${doc.summary}\n`;
  block += '    /// </summary>\n';
  if (doc.params) {
    for (const p of doc.params) {
      block += `    /// <param name="${p.name}">${p.desc}</param>\n`;
    }
  }
  if (doc.returns != null && doc.returns !== '') {
    block += `    /// <returns>${doc.returns}</returns>\n`;
  }
  return block;
}

function generateServiceInterface(
  entityName,
  dtoInfo,
  description,
  entityFile,
  crudType,
  existingContent = null,
) {
  const entityShort = entityName.replace(/^Takt/, '');
  const moduleParts = getModuleRelativePath(entityFile);
  const serviceNs = buildNamespace('Takt.Application.Services', moduleParts);
  const dtoNs = buildNamespace('Takt.Application.Dtos', moduleParts);
  const desc = description || entityShort;
  const hasTree = crudType === 'Tree' || dtoInfo.tree !== null;
  const preservedOptionsMethods = [];

  let content = '';
  content += '// ========================================\n';
  content += '// 项目名称：节拍工厂·Takt Plat\n';
  content += `// 命名空间：${serviceNs}\n`;
  content += `// 文件名称：I${entityName}Service.cs\n`;
  content += `// 创建时间：${todayFileHeaderDate()}\n`;
  content += '// 创建人：Takt365(Cursor AI)\n';
  content += `// 功能描述：${desc}应用服务接口\n`;
  content += '// \n';
  content += `// 版权信息：Copyright (c) ${new Date().getFullYear()} Takt  All rights reserved.\n`;
  content += '// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。\n';
  content += '// ========================================\n\n';
  content += `using ${dtoNs};\n`;
  content += 'using Takt.Shared.Models;\n';
  content += 'using Takt.Shared.Options;\n\n';
  content += `namespace ${serviceNs};\n\n`;
  content += '/// <summary>\n';
  content += `/// ${desc}应用服务接口\n`;
  content += '/// </summary>\n';
  content += `public interface I${entityName}Service\n`;
  content += '{\n';

  content += buildMethodXmlDoc({
    summary: `获取${desc}列表（分页）`,
    params: [{ name: 'queryDto', desc: '查询DTO' }],
    returns: '分页结果',
  });
  content += `    Task<TaktPagedResult<${dtoInfo.base}>> Get${entityShort}ListAsync(${dtoInfo.query} queryDto);\n\n`;

  content += buildMethodXmlDoc({
    summary: `根据ID获取${desc}`,
    params: [{ name: 'id', desc: `${desc}ID` }],
    returns: 'DTO',
  });
  content += `    Task<${dtoInfo.base}?> Get${entityShort}ByIdAsync(long id);\n\n`;

  const optionsIface = buildGetOptionsAsyncInterfaceSection(
    entityShort,
    hasTree,
    dtoInfo,
    desc,
    existingContent,
  );
  content += optionsIface.block;
  if (optionsIface.preserved) {
    preservedOptionsMethods.push(optionsIface.methodName);
  }

  if (hasTree && dtoInfo.tree) {
    content += buildMethodXmlDoc({
      summary: `获取${desc}树形列表`,
      params: [
        { name: 'parentId', desc: '父级ID' },
        { name: 'includeDisabled', desc: '是否包含禁用项' },
      ],
      returns: '树形列表',
    });
    content += `    Task<List<${dtoInfo.tree}>> Get${entityShort}TreeAsync(long parentId = 0, bool includeDisabled = false);\n\n`;
  }

  content += buildMethodXmlDoc({
    summary: `创建${desc}`,
    params: [{ name: 'dto', desc: '创建DTO' }],
    returns: 'DTO',
  });
  content += `    Task<${dtoInfo.base}> Create${entityShort}Async(${dtoInfo.create} dto);\n\n`;

  content += buildMethodXmlDoc({
    summary: `更新${desc}`,
    params: [
      { name: 'id', desc: `${desc}ID` },
      { name: 'dto', desc: '更新DTO' },
    ],
    returns: 'DTO',
  });
  content += `    Task<${dtoInfo.base}> Update${entityShort}Async(long id, ${dtoInfo.update} dto);\n\n`;

  content += buildMethodXmlDoc({
    summary: `删除${desc}`,
    params: [{ name: 'id', desc: `${desc}ID` }],
    returns: '任务',
  });
  content += `    Task Delete${entityShort}ByIdAsync(long id);\n\n`;

  content += buildMethodXmlDoc({
    summary: `批量删除${desc}`,
    params: [{ name: 'ids', desc: 'ID列表' }],
    returns: '任务',
  });
  content += `    Task Delete${entityShort}BatchAsync(IEnumerable<long> ids);\n\n`;

  for (const statusDto of dtoInfo.statuses) {
    const suffix = buildStatusMethodSuffix(statusDto, entityName);
    const statusLabel = suffix === 'Status' ? '状态' : suffix;
    content += buildMethodXmlDoc({
      summary: `更新${desc}${statusLabel}`,
      params: [{ name: 'dto', desc: '状态DTO' }],
      returns: 'DTO',
    });
    content += `    Task<${dtoInfo.base}> Update${entityShort}${suffix}Async(${statusDto} dto);\n\n`;
  }

  if (dtoInfo.sort) {
    content += buildMethodXmlDoc({
      summary: `更新${desc}排序`,
      params: [{ name: 'dto', desc: '排序DTO' }],
      returns: 'DTO',
    });
    content += `    Task<${dtoInfo.base}> Update${entityShort}SortAsync(${dtoInfo.sort} dto);\n\n`;
  }

  if (dtoInfo.template) {
    content += buildMethodXmlDoc({
      summary: '获取导入模板',
      params: [
        { name: 'sheetName', desc: '工作表名称' },
        { name: 'fileName', desc: '文件名' },
      ],
      returns: 'Excel 文件',
    });
    content += `    Task<(string fileName, byte[] content)> Get${entityShort}TemplateAsync(string? sheetName = null, string? fileName = null);\n\n`;
  }

  if (dtoInfo.import) {
    content += buildMethodXmlDoc({
      summary: `导入${desc}`,
      params: [
        { name: 'fileStream', desc: 'Excel 文件流' },
        { name: 'sheetName', desc: '工作表名称' },
      ],
      returns: '导入结果',
    });
    content += `    Task<(int success, int fail, List<string> errors)> Import${entityShort}Async(Stream fileStream, string? sheetName = null);\n\n`;
  }

  content += buildMethodXmlDoc({
    summary: `导出${desc}`,
    params: [
      { name: 'query', desc: '查询条件' },
      { name: 'sheetName', desc: '工作表名称' },
      { name: 'fileName', desc: '文件名' },
    ],
    returns: 'Excel 文件',
  });
  content += `    Task<(string fileName, byte[] fileContent)> Export${entityShort}Async(${dtoInfo.query}? query = null, string? sheetName = null, string? fileName = null);\n\n`;

  if (isTransposableEntity(entityShort)) {
    content += generateTransposedInterfaceMethods(entityShort, desc);
  }

  content += '}\n';
  return { content, serviceNs, dtoNs, preservedOptionsMethods };
}

/**
 * 汇总 QueryDto 中参与 KeyWords 模糊匹配的实体字段（与独立查询条件字段一致）
 * @param {Array} queryProps 查询 DTO 属性（不含范围 Start/End）
 * @param {Array} dateRanges 日期范围字段
 * @returns {Array<{ name: string, isString: boolean }>}
 */
function collectKeyWordsSearchFields(queryProps, dateRanges) {
  const fields = [];
  const seen = new Set();
  for (const prop of queryProps) {
    if (seen.has(prop.name)) {
      continue;
    }
    seen.add(prop.name);
    fields.push({ name: prop.name, isString: prop.isString });
  }
  for (const range of dateRanges) {
    if (seen.has(range.baseName)) {
      continue;
    }
    seen.add(range.baseName);
    fields.push({ name: range.baseName, isString: false });
  }
  return fields;
}

/**
 * 生成 KeyWords 模糊匹配代码块（覆盖 QueryDto 全部可搜索项）
 * @param {Array} queryProps 查询 DTO 属性
 * @param {Array} dateRanges 日期范围字段
 * @returns {string[]}
 */
function buildKeyWordsExpressionLines(queryProps, dateRanges) {
  const keyWordFields = collectKeyWordsSearchFields(queryProps, dateRanges);
  if (keyWordFields.length === 0) {
    return [];
  }
  const lines = [];
  lines.push('        if (!string.IsNullOrEmpty(queryDto?.KeyWords))');
  lines.push('        {');
  lines.push('            var keywords = queryDto.KeyWords;');
  lines.push('            exp = exp.And(x =>');
  keyWordFields.forEach((field, index) => {
    const prefix = index === 0 ? '                ' : '                || ';
    if (field.isString) {
      lines.push(`${prefix}(x.${field.name} != null && x.${field.name}.Contains(keywords))`);
      return;
    }
    lines.push(`${prefix}SqlFunc.ToString(x.${field.name}).Contains(keywords)`);
  });
  lines.push('            );');
  lines.push('        }');
  lines.push('');
  return lines;
}

/**
 * 生成 QueryExpression 方法体（SqlSugar Expressionable，租户/公司隔离由仓储 Where 处理）
 * @param {string} entityName 实体类名
 * @param {Array} queryProps 查询 DTO 属性（不含范围 Start/End）
 * @param {Array} dateRanges 日期范围字段
 * @returns {string} 方法体 C# 代码
 */
function buildQueryExpressionBody(entityName, queryProps, dateRanges) {
  const lines = [];
  lines.push(`        var exp = Expressionable.Create<${entityName}>();`);
  lines.push('');
  lines.push(...buildKeyWordsExpressionLines(queryProps, dateRanges));

  for (const prop of queryProps) {
    if (prop.isString) {
      lines.push(`        if (!string.IsNullOrEmpty(queryDto?.${prop.name}))`);
      lines.push('        {');
      lines.push(
        `            exp = exp.And(x => x.${prop.name} != null && x.${prop.name}.Contains(queryDto.${prop.name}));`,
      );
      lines.push('        }');
      lines.push('');
      continue;
    }

    if (prop.isDateTime) {
      lines.push(`        if (queryDto?.${prop.name}.HasValue == true)`);
      lines.push('        {');
      lines.push(`            exp = exp.And(x => x.${prop.name} == queryDto.${prop.name});`);
      lines.push('        }');
      lines.push('');
      continue;
    }

    if (prop.isNullableEnum || prop.isEnum || prop.isBool || prop.isNumeric) {
      lines.push(`        if (queryDto?.${prop.name}.HasValue == true)`);
      lines.push('        {');
      lines.push(`            exp = exp.And(x => x.${prop.name} == queryDto.${prop.name});`);
      lines.push('        }');
      lines.push('');
    }
  }

  for (const range of dateRanges) {
    lines.push(`        if (queryDto?.${range.startField}.HasValue == true)`);
    lines.push('        {');
    lines.push(`            exp = exp.And(x => x.${range.baseName} >= queryDto.${range.startField});`);
    lines.push('        }');
    lines.push('');
    lines.push(`        if (queryDto?.${range.endField}.HasValue == true)`);
    lines.push('        {');
    lines.push(`            exp = exp.And(x => x.${range.baseName} <= queryDto.${range.endField});`);
    lines.push('        }');
    lines.push('');
  }

  lines.push('        return exp.ToExpression();');
  return lines.join('\n');
}

function generateServiceImplementation(
  entityName,
  dtoInfo,
  description,
  entityFile,
  crudType,
  dtoBase,
  existingContent = null,
  genOptions = {},
) {
  const entityShort = entityName.replace(/^Takt/, '');
  const moduleParts = getModuleRelativePath(entityFile);
  const serviceNs = buildNamespace('Takt.Application.Services', moduleParts);
  const dtoNs = buildNamespace('Takt.Application.Dtos', moduleParts);
  const entityNs = getEntityNamespace(entityFile);
  const repoInterface = DTO_BASE_TO_REPOSITORY[dtoBase] || 'ITaktTenantRepository';
  const repoField = repositoryFieldName(entityShort);
  const desc = description || entityShort;
  const preservedOptionsMethods = [];
  const regeneratedOptionsMethods = [];
  const dtoContent = readUtf8(findDtoFileByEntity(entityName));
  const entityProps = extractEntityPropertyNames(entityFile);
  const queryProps = extractQueryDtoProperties(dtoContent, dtoInfo.query, entityProps);
  const dateRanges = extractDateRangeFields(dtoContent, dtoInfo.query, entityProps);
  const nameField = getNameFieldName(entityFile);
  const importDtoName = dtoInfo.import;
  const uniqueIndexes = extractUniqueIndexes(entityFile, dtoBase);
  const manyToOneMasterEarly =
    crudType === 'Single'
      ? generateManyToOneMasterStampExtras(
          entityFile,
          entityName,
          entityShort,
          dtoInfo,
          desc,
          dtoContent,
        )
      : null;
  const createUniqueBlock = buildUniqueValidationBlock(
    uniqueIndexes,
    repoField,
    desc,
    'create',
    'entity',
  );
  const updateUniqueBlock = buildUniqueValidationBlock(
    uniqueIndexes,
    repoField,
    desc,
    'update',
    'entity',
  );
  const importUniqueBlock = importDtoName
    ? buildUniqueValidationBlock(uniqueIndexes, repoField, desc, 'import', 'entity')
    : '';
  const importBatchGuard = importDtoName
    ? buildImportBatchDuplicateGuard(uniqueIndexes, 'entity')
    : { declare: '', check: '' };
  const hasTree = crudType === 'Tree' || dtoInfo.tree !== null;
  const treeGen = hasTree && dtoInfo.tree
    ? generateTreeServiceMethods(
        entityName,
        entityShort,
        dtoInfo,
        dtoBase,
        repoField,
        entityFile,
        desc,
        nameField,
      )
    : null;
  const rbacDelegation = hasRbacParentConfig(entityShort)
    ? generateRbacParentDelegationExtras(
        entityShort,
        dtoInfo,
        repoField,
        entityFile,
        desc,
        buildBuiltInDeleteGuardLines,
      )
    : null;
  const masterDetail =
    crudType === 'MasterDetail'
      ? generateMasterDetailServiceExtras(
          entityFile,
          entityName,
          entityShort,
          dtoInfo,
          dtoBase,
          repoField,
          desc,
        )
      : null;
  const sequenceMeta = buildSequenceMetaForService({
    entityFile,
    entityShort,
    crudType,
    dtoBase,
    masterDetail,
  });
  const manyToOneMaster = manyToOneMasterEarly;
  const transposedGenRaw = isTransposableEntity(entityShort)
    ? generateTransposedServiceImplementation(entityShort, desc, repoField, entityName, dtoBase)
    : null;
  const transposedGen = omitDuplicateTransposedCtorExtras(manyToOneMaster, transposedGenRaw, entityShort);
  const templateDto = dtoInfo.template;
  const importDto = dtoInfo.import;
  const exportDto = dtoInfo.export || dtoInfo.base;
  const queryExprBody = buildQueryExpressionBody(
    entityName,
    queryProps,
    dateRanges,
  );

  const entityScopeGuard = buildEntityScopeGuard(dtoBase);
  const optionsListPredicate = buildOptionsListPredicate(dtoBase);
  const ensureContextLine = buildEnsureContextLine(dtoBase);
  const hasBuiltIn = entityHasIsBuiltIn(entityFile);
  const builtInStatusMeta = hasBuiltIn ? extractBuiltInDisableStatusMeta(entityFile) : null;

  let content = '';
  content += '// ========================================\n';
  content += '// 项目名称：节拍工厂·Takt Plat\n';
  content += `// 命名空间：${serviceNs}\n`;
  content += `// 文件名称：${entityName}Service.cs\n`;
  content += `// 创建时间：${todayFileHeaderDate()}\n`;
  content += '// 创建人：Takt365(Cursor AI)\n';
  content += `// 功能描述：${desc}应用服务实现\n`;
  content += '// \n';
  content += `// 版权信息：Copyright (c) ${new Date().getFullYear()} Takt  All rights reserved.\n`;
  content += '// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。\n';
  content += '// ========================================\n\n';
  content += 'using System.Linq.Expressions;\n';
  content += 'using Mapster;\n';
  content += 'using SqlSugar;\n';
  content += `using ${dtoNs};\n`;
  content += `using ${entityNs};\n`;
  content += 'using Takt.Domain.Interfaces;\n';
  content += 'using Takt.Domain.Repositories;\n';
  content += 'using Takt.Shared.Exceptions;\n';
  content += 'using Takt.Shared.Helpers;\n';
  content += 'using Takt.Shared.Models;\n';
  content += 'using Takt.Shared.Options;\n';
  if (hasBuiltIn || treeGen?.needsEnumsUsing || transposedGen?.needsEnumsUsing) {
    content += 'using Takt.Shared.Enums;\n';
  }
  const extraUsingNs = new Set([
    ...(rbacDelegation?.extraUsings ?? []),
    ...(masterDetail?.extraUsings ?? []),
    ...(manyToOneMaster?.extraUsings ?? []),
  ].map((ns) => ns.replace(/^\s*using\s+/, '').replace(/;\s*$/, '').trim()).filter(Boolean));
  extraUsingNs.forEach((ns) => {
    if (ns !== dtoNs && ns !== serviceNs && ns !== entityNs) {
      content += `using ${ns};\n`;
    }
  });
  content += '\n';
  content += `namespace ${serviceNs};\n\n`;
  content += '/// <summary>\n';
  content += `/// ${desc}应用服务\n`;
  content += '/// </summary>\n';
  content += `public class ${entityName}Service : TaktServiceBase, I${entityName}Service\n`;
  content += '{\n';
  content += `    private readonly ${repoInterface}<${entityName}> ${repoField};\n`;
  if (rbacDelegation?.ctorFields) {
    content += `${rbacDelegation.ctorFields}\n`;
  }
  if (masterDetail?.ctorFields) {
    content += `${masterDetail.ctorFields}\n`;
  }
  if (manyToOneMaster?.ctorFields) {
    content += `${manyToOneMaster.ctorFields}\n`;
  }
  if (transposedGen?.ctorFields) {
    content += `${transposedGen.ctorFields}\n`;
  }
  if (sequenceMeta.needsSort) {
    content += '    private readonly ITaktSortOrderGenerator _sortOrderGenerator;\n';
  }
  if (sequenceMeta.needsLine) {
    content += '    private readonly ITaktLineNumberGenerator _lineNumberGenerator;\n';
  }
  content += '    private readonly ITaktUniqueValidator _uniqueValidator;\n\n';
  content += '    /// <summary>\n';
  content += '    /// 构造函数\n';
  content += '    /// </summary>\n';
  content += `    /// <param name="${entityShort.charAt(0).toLowerCase() + entityShort.slice(1)}Repository">${desc}仓储</param>\n`;
  if (rbacDelegation?.ctorParamDocs) {
    content += `${rbacDelegation.ctorParamDocs}\n`;
  }
  if (masterDetail?.ctorParamDocs) {
    content += `${masterDetail.ctorParamDocs}\n`;
  }
  if (manyToOneMaster?.ctorParamDocs) {
    content += `${manyToOneMaster.ctorParamDocs}\n`;
  }
  if (transposedGen?.ctorParamDocs) {
    content += `${transposedGen.ctorParamDocs}\n`;
  }
  if (sequenceMeta.needsSort) {
    content += '    /// <param name="sortOrderGenerator">排序号生成器</param>\n';
  }
  if (sequenceMeta.needsLine) {
    content += '    /// <param name="lineNumberGenerator">明细行号生成器</param>\n';
  }
  content += '    /// <param name="uniqueValidator">唯一性验证器</param>\n';
  content += '    /// <param name="userContext">用户上下文</param>\n';
  content += '    /// <param name="localizationService">本地化服务</param>\n';
  content += `    public ${entityName}Service(\n`;
  content += `        ${repoInterface}<${entityName}> ${entityShort.charAt(0).toLowerCase() + entityShort.slice(1)}Repository,\n`;
  if (rbacDelegation?.ctorParams) {
    content += `${rbacDelegation.ctorParams}\n`;
  }
  if (masterDetail?.ctorParams) {
    content += `${masterDetail.ctorParams}\n`;
  }
  if (manyToOneMaster?.ctorParams) {
    content += `${manyToOneMaster.ctorParams}\n`;
  }
  if (transposedGen?.ctorParams) {
    content += `${transposedGen.ctorParams}\n`;
  }
  if (sequenceMeta.needsSort) {
    content += '        ITaktSortOrderGenerator sortOrderGenerator,\n';
  }
  if (sequenceMeta.needsLine) {
    content += '        ITaktLineNumberGenerator lineNumberGenerator,\n';
  }
  content += '        ITaktUniqueValidator uniqueValidator,\n';
  content += '        ITaktUserContext? userContext = null,\n';
  content += '        ITaktLocalizationService? localizationService = null)\n';
  content += '        : base(userContext, localizationService)\n';
  content += '    {\n';
  content += `        ${repoField} = ${entityShort.charAt(0).toLowerCase() + entityShort.slice(1)}Repository;\n`;
  if (rbacDelegation?.ctorAssigns) {
    content += `${rbacDelegation.ctorAssigns}\n`;
  }
  if (masterDetail?.ctorAssigns) {
    content += `${masterDetail.ctorAssigns}\n`;
  }
  if (manyToOneMaster?.ctorAssigns) {
    content += `${manyToOneMaster.ctorAssigns}\n`;
  }
  if (transposedGen?.ctorAssigns) {
    content += `${transposedGen.ctorAssigns}\n`;
  }
  if (sequenceMeta.needsSort) {
    content += '        _sortOrderGenerator = sortOrderGenerator;\n';
  }
  if (sequenceMeta.needsLine) {
    content += '        _lineNumberGenerator = lineNumberGenerator;\n';
  }
  content += '        _uniqueValidator = uniqueValidator;\n';
  content += '    }\n\n';

  // List
  content += buildMethodXmlDoc({
    summary: `获取${desc}列表（分页）`,
    params: [{ name: 'queryDto', desc: '查询DTO' }],
    returns: '分页结果',
  });
  content += `    public async Task<TaktPagedResult<${dtoInfo.base}>> Get${entityShort}ListAsync(${dtoInfo.query} queryDto)\n`;
  content += '    {\n';
  content += '        var predicate = QueryExpression(queryDto);\n';
  content += `        var (data, total) = await ${repoField}.GetPagedAsync(\n`;
  content += '            queryDto.PageIndex,\n';
  content += '            queryDto.PageSize,\n';
  content += '            predicate);\n';
  content += `        return TaktPagedResult<${dtoInfo.base}>.Create(\n`;
  content += `            data.Adapt<List<${dtoInfo.base}>>(),\n`;
  content += '            total,\n';
  content += '            queryDto.PageIndex,\n';
  content += '            queryDto.PageSize);\n';
  content += '    }\n\n';

  // GetById
  content += buildMethodXmlDoc({
    summary: `根据ID获取${desc}`,
    params: [{ name: 'id', desc: `${desc}ID` }],
    returns: 'DTO',
  });
  content += `    public async Task<${dtoInfo.base}?> Get${entityShort}ByIdAsync(long id)\n`;
  content += '    {\n';
  content += `        var entity = await ${repoField}.GetByIdAsync(id);\n`;
  content += `        if (entity == null || ${entityScopeGuard})\n`;
  content += '        {\n';
  content += '            return null;\n';
  content += '        }\n';
  content +=
    rbacDelegation?.getByIdReturn
    || masterDetail?.getByIdReturn
    || `        return entity.Adapt<${dtoInfo.base}>();\n`;
  content += '    }\n\n';

  if (treeGen) {
    const treeOptionsMethodName = `Get${entityShort}TreeOptionsAsync`;
    const treeOptionsResolved = resolveOptionsImplementationBlock({
      existingContent,
      methodName: treeOptionsMethodName,
      repoField,
      freshTemplate: treeGen.treeOptionsBlock,
      refreshOptions: genOptions.refreshOptions === true,
    });
    content += treeOptionsResolved.block;
    if (treeOptionsResolved.preserved) {
      preservedOptionsMethods.push(treeOptionsMethodName);
    } else if (treeOptionsResolved.regenerated) {
      regeneratedOptionsMethods.push(treeOptionsMethodName);
    }
    content += treeGen.treeRemainderBlock;
  } else {
    const flatOptionsMethodName = `Get${entityShort}OptionsAsync`;
    const flatOptionsTemplate = buildFlatOptionsAsyncImplTemplate(
      entityShort,
      desc,
      repoField,
      ensureContextLine,
      optionsListPredicate,
      nameField,
    );
    const flatOptionsResolved = resolveOptionsImplementationBlock({
      existingContent,
      methodName: flatOptionsMethodName,
      repoField,
      freshTemplate: flatOptionsTemplate,
      refreshOptions: genOptions.refreshOptions === true,
    });
    content += flatOptionsResolved.block;
    if (flatOptionsResolved.preserved) {
      preservedOptionsMethods.push(flatOptionsMethodName);
    } else if (flatOptionsResolved.regenerated) {
      regeneratedOptionsMethods.push(flatOptionsMethodName);
    }
  }

  // Create
  content += buildMethodXmlDoc({
    summary: `创建${desc}`,
    params: [{ name: 'dto', desc: '创建DTO' }],
    returns: 'DTO',
  });
  content += `    public async Task<${dtoInfo.base}> Create${entityShort}Async(${dtoInfo.create} dto)\n`;
  content += '    {\n';
  content += `        var entity = dto.Adapt<${entityName}>();\n`;
  if (hasBuiltIn) {
    content += buildBuiltInCreateAssignLine();
  }
  if (manyToOneMaster?.createBeforeSave) {
    content += `        ${manyToOneMaster.createBeforeSave}\n`;
  }
  if (createUniqueBlock) {
    content += createUniqueBlock;
  }
  if (sequenceMeta.main.sortOrder) {
    content += buildAssignSortOrderBlock(
      sequenceMeta.main.sortOrder,
      repoField,
      dtoBase,
      'entity',
      '        ',
      entityFile,
    );
  }
  if (sequenceMeta.main.lineNumber) {
    content += buildAssignLineNumberBlock(
      sequenceMeta.main.lineNumber,
      repoField,
      dtoBase,
      'entity',
      '        ',
    );
  }
  content += `        entity = await ${repoField}.CreateAsync(entity);\n`;
  if (rbacDelegation?.createAfterSave) {
    content += `${rbacDelegation.createAfterSave}\n`;
  }
  if (masterDetail?.createAfterSave) {
    content += `        ${masterDetail.createAfterSave}\n`;
  }
  content += `        return await Get${entityShort}ByIdAsync(entity.Id) ?? entity.Adapt<${dtoInfo.base}>();\n`;
  content += '    }\n\n';

  // Update
  content += buildMethodXmlDoc({
    summary: `更新${desc}`,
    params: [
      { name: 'id', desc: `${desc}ID` },
      { name: 'dto', desc: '更新DTO' },
    ],
    returns: 'DTO',
  });
  content += `    public async Task<${dtoInfo.base}> Update${entityShort}Async(long id, ${dtoInfo.update} dto)\n`;
  content += '    {\n';
  content += `        var entity = await ${repoField}.GetByIdAsync(id);\n`;
  content += '        if (entity == null)\n';
  content += '        {\n';
  content += `            throw new TaktBusinessException("${desc}不存在");\n`;
  content += '        }\n';
  if (hasBuiltIn) {
    content += buildBuiltInUpdateBeforeAdaptLines(builtInStatusMeta);
  }
  content += '        dto.Adapt(entity);\n';
  if (hasBuiltIn) {
    content += buildBuiltInUpdateAfterAdaptLines(desc, builtInStatusMeta);
  }
  if (manyToOneMaster?.updateBeforeSave) {
    content += `        ${manyToOneMaster.updateBeforeSave}\n`;
  }
  if (updateUniqueBlock) {
    content += updateUniqueBlock;
  }
  content += `        await ${repoField}.UpdateAsync(entity);\n`;
  if (rbacDelegation?.updateAfterSave) {
    content += `${rbacDelegation.updateAfterSave}\n`;
  }
  if (masterDetail?.updateAfterSave) {
    content += `        ${masterDetail.updateAfterSave}\n`;
  }
  content += `        return await Get${entityShort}ByIdAsync(id) ?? throw new TaktBusinessException("${desc}不存在");\n`;
  content += '    }\n\n';

  // Delete
  let treeDeleteGuard = '';
  if (crudType === 'Tree') {
    treeDeleteGuard = `
        var hasChildren = await ${repoField}.ExistsAsync(x => x.ParentId == id);
        if (hasChildren)
        {
            throw new TaktBusinessException("存在子节点，无法删除");
        }
`;
  }
  content += buildMethodXmlDoc({
    summary: `删除${desc}`,
    params: [{ name: 'id', desc: `${desc}ID` }],
    returns: '任务',
  });
  content += `    public async Task Delete${entityShort}ByIdAsync(long id)\n`;
  content += '    {\n';
  if (hasBuiltIn && !masterDetail?.deletePrefix && !rbacDelegation?.deletePrefix) {
    content += `        var entity = await ${repoField}.GetByIdAsync(id);\n`;
    content += '        if (entity == null)\n';
    content += '        {\n';
    content += `            throw new TaktBusinessException("${desc}不存在或已删除");\n`;
    content += '        }\n';
    content += buildBuiltInDeleteGuardLines(desc);
  }
  content += treeDeleteGuard;
  if (rbacDelegation?.deletePrefix) {
    content += `${rbacDelegation.deletePrefix}\n`;
    content += `        var deleted = await ${repoField}.DeleteAsync(id);\n`;
    content += '        if (!deleted)\n';
    content += '        {\n';
    content += `            throw new TaktBusinessException("${desc}不存在或已删除");\n`;
    content += '        }\n';
  } else if (masterDetail?.deletePrefix) {
    content += `${masterDetail.deletePrefix}\n`;
    content += `        var deleted = await ${repoField}.DeleteAsync(id);\n`;
    content += '        if (!deleted)\n';
    content += '        {\n';
    content += `            throw new TaktBusinessException("${desc}不存在或已删除");\n`;
    content += '        }\n';
  } else if (hasBuiltIn) {
    content += `        var deleted = await ${repoField}.DeleteAsync(id);\n`;
    content += '        if (!deleted)\n';
    content += '        {\n';
    content += `            throw new TaktBusinessException("${desc}不存在或已删除");\n`;
    content += '        }\n';
  } else {
    content += `        var deleted = await ${repoField}.DeleteAsync(id);\n`;
    content += '        if (!deleted)\n';
    content += '        {\n';
    content += `            throw new TaktBusinessException("${desc}不存在或已删除");\n`;
    content += '        }\n';
  }
  content += '    }\n\n';

  content += buildMethodXmlDoc({
    summary: `批量删除${desc}`,
    params: [{ name: 'ids', desc: 'ID列表' }],
    returns: '任务',
  });
  content += `    public async Task Delete${entityShort}BatchAsync(IEnumerable<long> ids)\n`;
  content += '    {\n';
  content += '        var idList = ids?.Distinct().ToList() ?? new List<long>();\n';
  content += '        if (idList.Count == 0)\n';
  content += '        {\n';
  content += '            return;\n';
  content += '        }\n';
  if (hasBuiltIn) {
    content += buildBuiltInBatchDeleteGuardLines(desc, repoField);
  }
  content += '        foreach (var id in idList)\n';
  content += '        {\n';
  content += `            await Delete${entityShort}ByIdAsync(id);\n`;
  content += '        }\n';
  content += '    }\n\n';

  // Status
  for (const statusDto of dtoInfo.statuses) {
    const suffix = buildStatusMethodSuffix(statusDto, entityName);
    const statusFields = parseStatusDtoFields(dtoContent, statusDto);
    const idProp = statusFields.idProperty || `${entityShort}Id`;
    const valueProp = statusFields.valueProperties[0];
    if (!valueProp) {
      continue;
    }
    const statusLabel = suffix === 'Status' ? '状态' : suffix;
    content += buildMethodXmlDoc({
      summary: `更新${desc}${statusLabel}`,
      params: [{ name: 'dto', desc: '状态DTO' }],
      returns: 'DTO',
    });
    content += `    public async Task<${dtoInfo.base}> Update${entityShort}${suffix}Async(${statusDto} dto)\n`;
    content += '    {\n';
    content += `        var entity = await ${repoField}.GetByIdAsync(dto.${idProp});\n`;
    content += '        if (entity == null)\n';
    content += '        {\n';
    content += `            throw new TaktBusinessException("${desc}不存在");\n`;
    content += '        }\n';
    if (hasBuiltIn) {
      content += buildBuiltInStatusDisableGuardLines(desc, builtInStatusMeta, valueProp.name);
    }
    content += `        entity.${valueProp.name} = dto.${valueProp.name};\n`;
    content += `        await ${repoField}.UpdateAsync(entity);\n`;
    content += `        return await Get${entityShort}ByIdAsync(dto.${idProp}) ?? throw new TaktBusinessException("${desc}不存在");\n`;
    content += '    }\n\n';
  }

  // Sort
  if (dtoInfo.sort) {
    const sortBlock = extractClassBlock(dtoContent, dtoInfo.sort);
    const sortIdMatch = sortBlock.match(/public\s+long\s+(\w+Id)\s*\{/);
    const sortIdProp = sortIdMatch ? sortIdMatch[1] : `${entityShort}Id`;
    content += buildMethodXmlDoc({
      summary: `更新${desc}排序`,
      params: [{ name: 'dto', desc: '排序DTO' }],
      returns: 'DTO',
    });
    content += `    public async Task<${dtoInfo.base}> Update${entityShort}SortAsync(${dtoInfo.sort} dto)\n`;
    content += '    {\n';
    content += `        var entity = await ${repoField}.GetByIdAsync(dto.${sortIdProp});\n`;
    content += '        if (entity == null)\n';
    content += '        {\n';
    content += `            throw new TaktBusinessException("${desc}不存在");\n`;
    content += '        }\n';
    content += '        entity.SortOrder = dto.SortOrder;\n';
    content += `        await ${repoField}.UpdateAsync(entity);\n`;
    content += `        return await Get${entityShort}ByIdAsync(dto.${sortIdProp}) ?? throw new TaktBusinessException("${desc}不存在");\n`;
    content += '    }\n\n';
  }

  if (templateDto) {
    content += buildMethodXmlDoc({
      summary: '获取导入模板',
      params: [
        { name: 'sheetName', desc: '工作表名称' },
        { name: 'fileName', desc: '文件名' },
      ],
      returns: 'Excel 文件',
    });
    content += `    public async Task<(string fileName, byte[] content)> Get${entityShort}TemplateAsync(string? sheetName = null, string? fileName = null)\n`;
    content += '    {\n';
    content += `        return await TaktExcelHelper.GenerateTemplateAsync<${templateDto}>(\n`;
    content += `            sheetName ?? "${desc}导入模板",\n`;
    content += `            fileName ?? "${desc}导入模板.xlsx");\n`;
    content += '    }\n\n';
  }

  if (importDto) {
    content += buildMethodXmlDoc({
      summary: `导入${desc}`,
      params: [
        { name: 'fileStream', desc: 'Excel 文件流' },
        { name: 'sheetName', desc: '工作表名称' },
      ],
      returns: '导入结果',
    });
    content += `    public async Task<(int success, int fail, List<string> errors)> Import${entityShort}Async(Stream fileStream, string? sheetName = null)\n`;
    content += '    {\n';
    content += '        var errors = new List<string>();\n';
    content += '        var success = 0;\n';
    content += '        var fail = 0;\n';
    content += `        var rows = await TaktExcelHelper.ImportAsync<${importDto}>(fileStream, sheetName ?? "${desc}导入模板");\n`;
    content += '        if (rows == null || rows.Count == 0)\n';
    content += '        {\n';
    content += '            errors.Add("Excel文件中没有数据");\n';
    content += '            return (0, 0, errors);\n';
    content += '        }\n';
    content += importBatchGuard.declare;
    if (sequenceMeta.main.sortOrder) {
      content += buildImportSortOrderDeclare(sequenceMeta.main.sortOrder, repoField, dtoBase);
    }
    content += '        for (var i = 0; i < rows.Count; i++)\n';
    content += '        {\n';
    content += '            try\n';
    content += '            {\n';
    content += `                var entity = rows[i].Adapt<${entityName}>();\n`;
    if (hasBuiltIn) {
      content += '                entity.IsBuiltIn = TaktYesNo.No;\n';
    }
    if (manyToOneMaster) {
      content += `                var importDto = rows[i].Adapt<${dtoInfo.create}>();\n`;
      content += `                ${manyToOneMaster.importBeforeSave}\n`;
    }
    if (importBatchGuard.check) {
      content += importBatchGuard.check;
    }
    if (importUniqueBlock) {
      content += importUniqueBlock;
    }
    if (sequenceMeta.main.sortOrder) {
      if (sequenceMeta.main.sortOrder.mode === 'flat') {
        content += buildImportSortOrderAssign(sequenceMeta.main.sortOrder, 'entity', '                ');
      } else {
        content += buildAssignSortOrderBlock(
          sequenceMeta.main.sortOrder,
          repoField,
          dtoBase,
          'entity',
          '                ',
          entityFile,
        );
      }
    }
    if (sequenceMeta.main.lineNumber) {
      content += buildAssignLineNumberBlock(
        sequenceMeta.main.lineNumber,
        repoField,
        dtoBase,
        'entity',
        '                ',
      );
    }
    content += `                await ${repoField}.CreateAsync(entity);\n`;
    content += '                success += 1;\n';
    content += '            }\n';
    content += '            catch (Exception ex)\n';
    content += '            {\n';
    content += '                fail += 1;\n';
    content += '                errors.Add($"第{i + 2}行: {ex.Message}");\n';
    content += '            }\n';
    content += '        }\n';
    content += '        return (success, fail, errors);\n';
    content += '    }\n\n';
  }

  // Export
  content += buildMethodXmlDoc({
    summary: `导出${desc}`,
    params: [
      { name: 'query', desc: '查询条件' },
      { name: 'sheetName', desc: '工作表名称' },
      { name: 'fileName', desc: '文件名' },
    ],
    returns: 'Excel 文件',
  });
  content += `    public async Task<(string fileName, byte[] fileContent)> Export${entityShort}Async(${dtoInfo.query}? query = null, string? sheetName = null, string? fileName = null)\n`;
  content += '    {\n';
  content += `        var predicate = QueryExpression(query ?? new ${dtoInfo.query}());\n`;
  content += `        var list = await ${repoField}.GetListAsync(predicate);\n`;
  content += '        if (list == null || list.Count == 0)\n';
  content += '        {\n';
  content += `            return await TaktExcelHelper.ExportAsync(\n`;
  content += `                new List<${exportDto}>(),\n`;
  content += `                sheetName ?? "${desc}数据",\n`;
  content += `                fileName ?? "${desc}导出.xlsx");\n`;
  content += '        }\n';
  content += `        var exportData = list.Adapt<List<${exportDto}>>();\n`;
  content += '        return await TaktExcelHelper.ExportAsync(\n';
  content += '            exportData,\n';
  content += `            sheetName ?? "${desc}数据",\n`;
  content += `            fileName ?? "${desc}导出.xlsx");\n`;
  content += '    }\n\n';

  if (masterDetail?.privateMethods) {
    content += '    // ========================================\n';
    content += '    // 主子表级联（OneToMany）\n';
    content += '    // ========================================\n\n';
    content += masterDetail.privateMethods;
  }

  if (manyToOneMaster?.privateMethods) {
    content += '    // ========================================\n';
    content += '    // 主表外键同步（ManyToOne）\n';
    content += '    // ========================================\n\n';
    content += manyToOneMaster.privateMethods;
  }

  if (transposedGen?.methods) {
    content += '    // ========================================\n';
    content += '    // 转置（多语言表格）\n';
    content += '    // ========================================\n\n';
    content += transposedGen.methods;
    content += '\n';
  }

  // QueryExpression
  content += '    // ========================================\n';
  content += '    // 查询表达式\n';
  content += '    // ========================================\n\n';
  content += buildMethodXmlDoc({
    summary: `构建${desc}查询表达式`,
    params: [{ name: 'queryDto', desc: '查询DTO' }],
    returns: '查询表达式',
  });
  content += `    private static Expression<Func<${entityName}, bool>> QueryExpression(${dtoInfo.query}? queryDto)\n`;
  content += '    {\n';
  content += `${queryExprBody}\n`;
  content += '    }\n';
  if (transposedGen?.transposedQueryExpr) {
    content += '\n';
    content += '    /// <summary>\n';
    content += `    /// 构建${desc}转置查询表达式\n`;
    content += '    /// </summary>\n';
    content += `    private Expression<Func<${entityName}, bool>> TransposedQueryExpression(${dtoInfo.transposedQuery} queryDto)\n`;
    content += '    {\n';
    content += `${transposedGen.transposedQueryExpr};\n`;
    content += '    }\n';
  }

  content += '}\n';
  return { content, preservedOptionsMethods, regeneratedOptionsMethods, sequenceMeta };
}

function findDtoFileByEntity(entityName) {
  const fileName = `${entityName}Dtos.cs`;
  function searchDir(dir) {
    if (!fs.existsSync(dir)) {
      return null;
    }
    const entries = fs.readdirSync(dir, { withFileTypes: true });
    for (const entry of entries) {
      const fullPath = path.join(dir, entry.name);
      if (entry.isDirectory()) {
        if (isInEngineDirectory(fullPath)) {
          continue;
        }
        const found = searchDir(fullPath);
        if (found) {
          return found;
        }
      } else if (entry.name === fileName) {
        return fullPath;
      }
    }
    return null;
  }
  return searchDir(CONFIG.dtosRoot);
}

function scanDtoFiles(entityPrefix) {
  const results = [];
  function walk(dir) {
    if (!fs.existsSync(dir)) {
      return;
    }
    const entries = fs.readdirSync(dir, { withFileTypes: true });
    for (const entry of entries) {
      const fullPath = path.join(dir, entry.name);
      if (entry.isDirectory()) {
        if (isInEngineDirectory(fullPath)) {
          continue;
        }
        walk(fullPath);
      } else if (entry.name.endsWith('Dtos.cs')) {
        if (shouldExcludeDtoFile(fullPath)) {
          continue;
        }
        const entityName = entityNameFromDtoFile(fullPath);
        if (!entityName) {
          continue;
        }
        const entityShort = entityName.replace(/^Takt/, '');
        if (entityPrefix && entityShort !== entityPrefix) {
          continue;
        }
        results.push(fullPath);
      }
    }
  }
  walk(CONFIG.dtosRoot);
  return results.sort();
}

function getServiceOutputPaths(entityFile, entityName) {
  const moduleParts = getModuleRelativePath(entityFile);
  const serviceDir = path.join(CONFIG.servicesRoot, ...moduleParts);
  return {
    interfaceFile: path.join(serviceDir, `I${entityName}Service.cs`),
    implFile: path.join(serviceDir, `${entityName}Service.cs`),
    serviceDir,
  };
}

function processDtoFile(dtoFile, options) {
  if (shouldExcludeDtoFile(dtoFile)) {
    const entityName = entityNameFromDtoFile(dtoFile);
    const entityShort = entityName ? entityName.replace(/^Takt/, '') : path.basename(dtoFile);
    console.log(`\n⏭️  跳过特殊实体: ${entityShort}（${path.relative(CONFIG.dtosRoot, dtoFile)}）`);
    return { status: 'skipped' };
  }

  const entityName = entityNameFromDtoFile(dtoFile);
  const entityShort = entityName.replace(/^Takt/, '');
  const relDto = path.relative(CONFIG.dtosRoot, dtoFile);

  console.log(`\n${'='.repeat(60)}`);
  console.log(`📦 ${entityName}  ←  ${relDto}`);

  const dtoInfo = extractDtoInfo(dtoFile);
  if (!isAggregatable(dtoInfo)) {
    console.log('  ⏭️  跳过：非标准聚合 DTO（需同时具备 Dto / QueryDto / CreateDto / UpdateDto）');
    return { status: 'skipped' };
  }

  const entityFile = findEntityFile(entityName);
  if (!entityFile) {
    console.log(`  ❌ 未找到实体文件: ${entityName}.cs`);
    return { status: 'failed' };
  }

  const crudType = identifyCrudType(entityFile);
  if (crudType === 'MasterDetail') {
    const navChildren = parseOneToManyNavigations(entityFile);
    const rbacNavs = navChildren.filter((n) => isRbacJunctionEntity(n.childShort));
    const masterNavs = navChildren.filter((n) => !isRbacJunctionEntity(n.childShort));
    if (rbacNavs.length > 0 || hasRbacParentConfig(entityShort)) {
      console.log(
        `  ℹ️  RBAC 关联委托 ITaktRbacService（${hasRbacParentConfig(entityShort) ? 'rbac-parent-config' : rbacNavs.map((n) => n.childShort).join('、')}）`,
      );
    }
    if (masterNavs.length > 0) {
      console.log(
        `  ℹ️  主子表：Create/Update 含子表集合，服务级联查询/保存/删除（子表×${masterNavs.length}）`,
      );
    }
  } else if (hasRbacParentConfig(entityShort)) {
    console.log('  ℹ️  RBAC 关联委托 ITaktRbacService（rbac-parent-config）');
  }

  if (!options.force && (EXISTING_MANUAL_SERVICE_ENTITIES.has(entityName) || shouldExcludeStandaloneService(entityName))) {
    console.log(`  ⏭️  跳过：已有手工服务（实体 ${entityName}），使用 --force 可覆盖`);
    return { status: 'skipped' };
  }

  const output = getServiceOutputPaths(entityFile, entityName);

  const description = extractEntityDescription(entityFile) || entityShort;
  const dtoBase = parseDtoBase(dtoFile, dtoInfo);
  if (!dtoBase) {
    console.log(
      `  ❌ 无法识别 DTO 基类：${dtoInfo.base} 须继承 TaktTenantDtoBase / TaktCompanyDtoBase / TaktApprovalDtoBase`,
    );
    return { status: 'failed' };
  }
  const entityBase = DTO_BASE_TO_ENTITY_BASE[dtoBase];
  const entityBaseFromFile = parseEntityBase(entityFile);
  if (entityBaseFromFile !== entityBase) {
    console.log(
      `  ⚠️  DTO 基类 ${dtoBase}（→${entityBase}）与实体基类 ${entityBaseFromFile} 不一致，以 DTO 为准`,
    );
  }
  console.log(`  DtoBase: ${dtoBase}  →  ${DTO_BASE_TO_REPOSITORY[dtoBase]}`);
  if (entityHasIsBuiltIn(entityFile)) {
    console.log('  ℹ️  实体含 IsBuiltIn：已生成创建/更新/单删/批删/状态更新内置保护');
  }

  const existingIfaceContent = fs.existsSync(output.interfaceFile) ? readUtf8(output.interfaceFile) : null;
  const existingImplContent = fs.existsSync(output.implFile) ? readUtf8(output.implFile) : null;

  const iface = generateServiceInterface(
    entityName,
    dtoInfo,
    description,
    entityFile,
    crudType,
    existingIfaceContent,
  );
  const implResult = generateServiceImplementation(
    entityName,
    dtoInfo,
    description,
    entityFile,
    crudType,
    dtoBase,
    existingImplContent,
    { refreshOptions: options.refreshOptions === true },
  );
  const ifaceContent = iface.content;
  const impl = implResult.content;

  const preservedOptions = [
    ...new Set([...(iface.preservedOptionsMethods || []), ...(implResult.preservedOptionsMethods || [])]),
  ];
  if (preservedOptions.length > 0) {
    console.log(`  ℹ️  已保留已有 Get*OptionsAsync（未重新生成）: ${preservedOptions.join(', ')}`);
  }
  if (implResult.regeneratedOptionsMethods?.length > 0) {
    console.log(
      `  ℹ️  已重新生成 Get*OptionsAsync（未使用 ${DTO_BASE_TO_REPOSITORY[dtoBase]} 或含遗留无效调用）: ${implResult.regeneratedOptionsMethods.join(', ')}`,
    );
  }
  if (
    preservedOptions.length === 0 &&
    !(implResult.regeneratedOptionsMethods?.length > 0) &&
    (existingImplContent || existingIfaceContent)
  ) {
    const optionsMethodName =
      crudType === 'Tree' || dtoInfo.tree
        ? `Get${entityShort}TreeOptionsAsync`
        : `Get${entityShort}OptionsAsync`;
    if (!hasGetOptionsAsyncMethod(existingImplContent, optionsMethodName)) {
      console.log(`  ℹ️  已生成默认 ${optionsMethodName}`);
    }
  }

  if (options.dryRun) {
    console.log(`  🔍 [dry-run] ${path.relative(CONFIG.backendRoot, output.interfaceFile)}`);
    console.log(`  🔍 [dry-run] ${path.relative(CONFIG.backendRoot, output.implFile)}`);
    if (preservedOptions.length > 0) {
      console.log(`  🔍 [dry-run] 将保留 Options 方法: ${preservedOptions.join(', ')}`);
    }
    return { status: 'dry-run' };
  }

  const ifaceWrite = writeGeneratedFile(output.interfaceFile, ifaceContent);
  const implWrite = writeGeneratedFile(output.implFile, impl);
  const ifaceLabel = ifaceWrite.created ? '已创建' : '已更新';
  const implLabel = implWrite.created ? '已创建' : '已更新';
  console.log(`  ✅ ${ifaceLabel}: ${path.relative(CONFIG.backendRoot, output.interfaceFile)}`);
  console.log(`  ✅ ${implLabel}: ${path.relative(CONFIG.backendRoot, output.implFile)}`);
  if (crudType === 'Tree' || dtoInfo.tree) {
    console.log('  ℹ️  已生成 Get*TreeAsync（内存递归构建，includeDisabled 按实体状态字段过滤）');
  }
  if (crudType === 'MasterDetail') {
    console.log('  ℹ️  已生成 Fill*DetailsAsync / Save*ChildrenAsync 级联方法');
  }
  if (implResult.sequenceMeta?.needsSort || implResult.sequenceMeta?.needsLine) {
    const parts = [];
    if (implResult.sequenceMeta.needsSort) {
      parts.push('SortOrder');
    }
    if (implResult.sequenceMeta.needsLine) {
      parts.push('LineNumber');
    }
    console.log(`  ℹ️  已接入 ${parts.join('、')} 自动生成（Create/Import/子表级联）`);
  }
  if (isTransposableEntity(entityShort)) {
    console.log('  ℹ️  已生成 Get*TransposedListAsync / Save*TransposedBatchAsync 转置方法');
  }
  return {
    status: 'written',
    created: ifaceWrite.created || implWrite.created,
    updated: ifaceWrite.updated || implWrite.updated,
  };
}

function printUsage() {
  console.log(`
用法:
  node scripts/generate-services-from-dtos.cjs --Holiday
  node scripts/generate-services-from-dtos.cjs --all
  node scripts/generate-services-from-dtos.cjs --Holiday --force
  node scripts/generate-services-from-dtos.cjs --Holiday --refresh-options
  node scripts/generate-services-from-dtos.cjs --Holiday --dry-run

说明:
  - 扫描 Takt.Application/Dtos/**/*Dtos.cs
  - 仅处理同时具备 TaktXxxDto / QueryDto / CreateDto / UpdateDto 的聚合模块
  - 隔离与仓储由主 DTO 继承的 DtoBase 决定：
      TaktTenantDtoBase → ITaktTenantRepository，仅 TenantCode
      TaktCompanyDtoBase → ITaktCompanyRepository，TenantCode + CompanyCode
      TaktApprovalDtoBase → ITaktApprovalRepository，TenantCode + CompanyCode
  - 排除 User（与 generate-dtos-from-entity.cjs 一致，禁止生成/覆盖）
  - Translation：额外生成转置查询/批量保存（多语言表格）
  - 输出策略：文件不存在则创建，已存在则覆盖更新（无需 --force）
  - 主子表（OneToMany）：Create/Update 含子表 List，服务级联查询/保存/删除
  - RBAC 八表：主实体 Create/Update/Delete 委托 ITaktRbacService（见 scripts/rbac-parent-config.cjs，User 除外）
  - Auth 等手工服务仅在不带 --force 时跳过
  - 树形实体（含 ParentId）：生成 GetXxxTreeOptionsAsync + GetXxxTreeAsync，不生成 GetXxxOptionsAsync
  - Get*OptionsAsync：磁盘上无该方法 → 生成默认模板；已存在且实现含 \${repo}.GetListAsync → 原样保留
  - 已存在但含遗留无效调用（如 GetTenantMenuListAsync）→ 自动改用仓储查询模板重新生成
  - --refresh-options：强制重新生成所有 Get*OptionsAsync / Get*TreeOptionsAsync 实现
  - 实体含 IsBuiltIn 时：创建强制 No；更新保留原值；单删/批删前校验（批删含任一内置则整批拒绝）；状态更新禁止禁用内置项；员工更新禁止离职(3)/退休(4)
  - 实体含 SortOrder / LineNumber：Create、Import、主子表 Save*ChildrenAsync 在值 <= 0 时经 ITaktSortOrderGenerator / ITaktLineNumberGenerator 自动生成（先仓储 GetMaxIntAsync）
`);
}

function parseArgs() {
  const args = process.argv.slice(2);
  const options = {
    all: false,
    entityPrefix: null,
    force: false,
    dryRun: false,
    refreshOptions: false,
  };
  for (const arg of args) {
    if (arg === '--force') {
      options.force = true;
      continue;
    }
    if (arg === '--refresh-options') {
      options.refreshOptions = true;
      continue;
    }
    if (arg === '--dry-run') {
      options.dryRun = true;
      continue;
    }
    if (!arg.startsWith('--')) {
      console.error(`❌ 未知参数: ${arg}`);
      process.exit(1);
    }
    const value = arg.slice(2);
    if (value.toLowerCase() === 'all') {
      options.all = true;
      continue;
    }
    if (value.startsWith('Takt')) {
      console.error('❌ 实体名不要带 Takt 前缀，例如 --Holiday');
      process.exit(1);
    }
    if (options.entityPrefix) {
      console.error('❌ 只能指定一个实体，或使用 --all');
      process.exit(1);
    }
    options.entityPrefix = value;
  }
  if (!options.all && !options.entityPrefix) {
    console.error('❌ 请指定 --all 或 --<实体名>');
    printUsage();
    process.exit(1);
  }
  if (options.entityPrefix) {
    assertNotSpecialEntityCli(options.entityPrefix);
  }
  return options;
}

// ========================================
// 主流程
// ========================================

console.log('🚀 从 DTO 生成 Application 服务接口与实现...\n');
logGeneratedFileWritePolicy();
console.log(`⏭️  排除特殊实体: ${[...MANUAL_CRUD_ENTITY_SHORT_NAMES].join('、')}\n`);

try {
  const options = parseArgs();
  const dtoFiles = scanDtoFiles(options.all ? null : options.entityPrefix);

  if (dtoFiles.length === 0) {
    console.error('❌ 未找到匹配的 DTO 文件');
    process.exit(1);
  }

  console.log(`📄 匹配 DTO 文件 ${dtoFiles.length} 个`);

  let created = 0;
  let updated = 0;
  let skipped = 0;
  let failed = 0;

  for (const dtoFile of dtoFiles) {
    const result = processDtoFile(dtoFile, options);
    if (result.status === 'dry-run') {
      created += 1;
    } else if (result.status === 'written') {
      if (result.updated && !result.created) {
        updated += 1;
      } else {
        created += 1;
      }
    } else if (result.status === 'failed') {
      failed += 1;
    } else {
      skipped += 1;
    }
  }

  const processed = created + updated + skipped + failed;
  console.log(`\n📊 已创建 ${created} 个，已更新 ${updated} 个，跳过 ${skipped} 个，失败 ${failed} 个（共处理 ${processed} 个 *Dtos.cs）`);
  if (skipped > 0) {
    console.log(
      '   跳过原因：① 关联表无完整 CRUD 聚合；② 手工服务实体 TaktAuth；TaktLoginDtos.cs 不进入扫描；',
    );
    console.log('   User/Online/Message、RBAC 八表等 Dtos 不会进入扫描列表。');
  }
  console.log('✨ 完成！请编译解决方案，并人工审阅 QueryExpression、导入校验与特殊树形业务逻辑。');
} catch (error) {
  console.error('❌ 生成失败:', error);
  process.exit(1);
}
