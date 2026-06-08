// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/scripts
// 文件名称：generate-script-common.cjs
// 创建时间：2026-05-23
// 创建人：Takt365(Cursor AI)
// 功能描述：代码生成脚本公共工具（文件写入、Service 单数 / Controller 复数命名）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const fs = require('fs');
const path = require('path');

/**
 * REST 控制器复数特例（键为实体短名单数，与 ITaktXxxService 一致）
 */
const CONTROLLER_PLURAL_OVERRIDES = {
  User: 'Users',
  Auth: 'Auths',
  Company: 'Companies',
  Culture: 'Cultures',
  TranslationMessage: 'TranslationMessages',
  HolidayTheme: 'HolidayThemes',
  DataDictAll: 'DataDictAlls',
};

/**
 * 将实体短名复数化（用于 Takt{Plural}Controller 与 api/Takt{Plural} 路由）
 * @param {string} entityShort 如 Holiday、DictData、HolidayTheme
 * @returns {string} 如 Holidays、DictDatas、HolidayThemes
 */
function pluralizeEntityShort(entityShort) {
  if (!entityShort) {
    return entityShort;
  }
  if (CONTROLLER_PLURAL_OVERRIDES[entityShort]) {
    return CONTROLLER_PLURAL_OVERRIDES[entityShort];
  }
  if (entityShort.endsWith('y') && !/[aeiou]y$/i.test(entityShort)) {
    return `${entityShort.slice(0, -1)}ies`;
  }
  if (entityShort.endsWith('Company')) {
    return `${entityShort.slice(0, -7)}Companies`;
  }
  if (/(s|x|z|ch|sh)$/i.test(entityShort)) {
    return `${entityShort}es`;
  }
  return `${entityShort}s`;
}

/**
 * 从控制器路由段（无 Takt 前缀）还原实体短名单数
 * @param {string} segment 如 Holidays、Users、Companies
 * @returns {string} 如 Holiday、User、Company
 */
function singularizeControllerSegment(segment) {
  if (!segment) {
    return segment;
  }
  for (const [singular, plural] of Object.entries(CONTROLLER_PLURAL_OVERRIDES)) {
    if (segment === plural) {
      return singular;
    }
  }
  if (segment.endsWith('ies')) {
    return `${segment.slice(0, -3)}y`;
  }
  if (segment.endsWith('Companies')) {
    return `${segment.slice(0, -10)}Company`;
  }
  if (segment.endsWith('es')) {
    const base = segment.slice(0, -2);
    if (/(s|x|z|ch|sh)$/i.test(base)) {
      return base;
    }
  }
  if (segment.endsWith('s') && segment.length > 1) {
    return segment.slice(0, -1);
  }
  return segment;
}

/**
 * 由实体完整类名生成控制器类名（复数 + Controller）
 * @param {string} entityName 如 TaktHoliday、TaktUser
 * @returns {string} 如 TaktHolidaysController、TaktUsersController
 */
function getControllerClassName(entityName) {
  const entityShort = entityName.replace(/^Takt/, '');
  return `Takt${pluralizeEntityShort(entityShort)}Controller`;
}

/**
 * 由实体完整类名生成 [controller] 路由段（含 Takt 前缀）
 * @param {string} entityName 如 TaktHoliday
 * @returns {string} 如 TaktHolidays
 */
function getControllerRouteSegment(entityName) {
  const entityShort = entityName.replace(/^Takt/, '');
  return `Takt${pluralizeEntityShort(entityShort)}`;
}

/**
 * 从控制器类名解析实体短名（单数，用于 DTO/权限/前端 types 文件名）
 * @param {string} controllerClassName 如 TaktHolidaysController
 * @returns {string} 如 Holiday
 */
function entityShortFromControllerClassName(controllerClassName) {
  const segment = controllerClassName.replace(/^Takt/, '').replace(/Controller$/, '');
  return singularizeControllerSegment(segment);
}

/**
 * CLI 实体前缀是否匹配控制器类名（仅接受复数控制器名）
 * @param {string} controllerClassName 如 TaktHolidaysController
 * @param {string} entityPrefix 如 Holiday
 * @returns {boolean}
 */
function matchControllerForEntityPrefix(controllerClassName, entityPrefix) {
  return controllerClassName === getControllerClassName(`Takt${entityPrefix}`);
}

/**
 * 全栈 generate-*.cjs / generate-all.cjs 统一文件写入策略
 * @type {string}
 */
const GENERATED_FILE_WRITE_POLICY = '不存在则创建，已存在则覆盖更新';

/**
 * 启动时打印统一写入策略
 */
function logGeneratedFileWritePolicy() {
  console.log(`📝 写入策略：${GENERATED_FILE_WRITE_POLICY}\n`);
}

/**
 * 写入生成文件：目录不存在则创建；文件已存在则覆盖更新
 * @param {string} filePath 目标文件绝对或相对路径
 * @param {string} content 文件内容
 * @returns {{ created: boolean, updated: boolean }}
 */
function writeGeneratedFile(filePath, content) {
  const dir = path.dirname(filePath);
  if (!fs.existsSync(dir)) {
    fs.mkdirSync(dir, { recursive: true });
  }
  const existed = fs.existsSync(filePath);
  fs.writeFileSync(filePath, content, 'utf-8');
  return { created: !existed, updated: existed };
}

const DEFAULT_BACKEND_ROOT = path.resolve(__dirname, '../backend/src');

/**
 * 将 DtoBase / EntityBase 名称映射为表格 entityScope
 * @param {string} baseName
 * @returns {'tenant'|'company'|'approval'}
 */
function entityBaseNameToScope(baseName) {
  if (!baseName) {
    return 'company';
  }
  if (baseName.includes('Approval')) {
    return 'approval';
  }
  if (baseName.includes('Company')) {
    return 'company';
  }
  if (baseName.includes('Tenant')) {
    return 'tenant';
  }
  return 'company';
}

/**
 * 查找 Domain 实体文件
 * @param {string} entityPascal 如 CostCenter
 * @param {string} [backendRoot]
 * @returns {string|null}
 */
function findDomainEntityFile(entityPascal, backendRoot = DEFAULT_BACKEND_ROOT) {
  const entityFileName = `Takt${entityPascal}.cs`;
  const entitiesRoot = path.join(backendRoot, 'Takt.Domain', 'Entities');
  if (!fs.existsSync(entitiesRoot)) {
    return null;
  }
  /** @param {string} dir */
  function search(dir) {
    for (const entry of fs.readdirSync(dir, { withFileTypes: true })) {
      const full = path.join(dir, entry.name);
      if (entry.isDirectory()) {
        const found = search(full);
        if (found) {
          return found;
        }
      } else if (entry.name === entityFileName) {
        return full;
      }
    }
    return null;
  }
  return search(entitiesRoot);
}

/**
 * 从 C# 实体文件解析 EntityBase
 * @param {string} entityFilePath
 * @returns {'TaktTenantEntityBase'|'TaktCompanyEntityBase'|'TaktApprovalEntityBase'}
 */
function parseEntityBaseFromCsFile(entityFilePath) {
  const content = fs.readFileSync(entityFilePath, 'utf-8');
  const match = content.match(/public\s+(?:sealed\s+|abstract\s+)?class\s+Takt\w+\s*:\s*(Takt(?:Approval|Company|Tenant)EntityBase)/);
  if (match) {
    return match[1];
  }
  if (content.includes(': TaktApprovalEntityBase')) {
    return 'TaktApprovalEntityBase';
  }
  if (content.includes(': TaktCompanyEntityBase')) {
    return 'TaktCompanyEntityBase';
  }
  if (content.includes(': TaktTenantEntityBase')) {
    return 'TaktTenantEntityBase';
  }
  return 'TaktCompanyEntityBase';
}

/**
 * 从 types 主实体 interface 块解析 entityScope
 * @param {string} typesContent
 * @param {string} entityPascal
 * @returns {'tenant'|'company'|'approval'|null}
 */
function resolveEntityScopeFromTypesInterface(typesContent, entityPascal) {
  const ifaceRe = new RegExp(
    `/\\*\\*[\\s\\S]*?\\*/\\s*export interface ${entityPascal}\\b(?:\\s+extends\\s+(\\w+))?`,
  );
  const match = typesContent.match(ifaceRe);
  if (!match) {
    return null;
  }
  const jsdocBase = match[0].match(/继承\s+Takt(\w+DtoBase)/);
  if (jsdocBase) {
    return entityBaseNameToScope(`Takt${jsdocBase[1]}`);
  }
  if (match[1]) {
    return entityBaseNameToScope(match[1]);
  }
  return null;
}

/**
 * 解析实体基类作用域（优先 Domain 实体，其次 types JSDoc / extends）
 * @param {string} entityPascal
 * @param {string} [typesContent]
 * @param {string} [backendRoot]
 * @returns {'tenant'|'company'|'approval'}
 */
function resolveEntityScope(entityPascal, typesContent = '', backendRoot = DEFAULT_BACKEND_ROOT) {
  const entityFile = findDomainEntityFile(entityPascal, backendRoot);
  if (entityFile) {
    return entityBaseNameToScope(parseEntityBaseFromCsFile(entityFile));
  }
  const fromTypes = resolveEntityScopeFromTypesInterface(typesContent, entityPascal);
  if (fromTypes) {
    return fromTypes;
  }
  return 'company';
}

/**
 * 三个实体基类字段（与 frontend/src/utils/table-columns.ts ENTITY_BASE_FIELDS 保持同步，不含 id）
 */
const ENTITY_BASE_FIELDS = {
  tenant: [
    'tenantCode', 'extFieldJson', 'remark',
    'createdBy', 'createdAt', 'updatedBy', 'updatedAt', 'isDeleted', 'deletedBy', 'deletedAt',
  ],
  company: [
    'tenantCode', 'companyCode', 'extFieldJson', 'remark',
    'createdBy', 'createdAt', 'updatedBy', 'updatedAt', 'isDeleted', 'deletedBy', 'deletedAt',
  ],
  approval: [
    'tenantCode', 'companyCode', 'extFieldJson', 'remark',
    'approvalStatus', 'initiatorId', 'initiatedAt', 'approvalOpinion', 'approvedBy', 'approvedAt',
    'createdBy', 'createdAt', 'updatedBy', 'updatedAt', 'isDeleted', 'deletedBy', 'deletedAt',
  ],
};

module.exports = {
  GENERATED_FILE_WRITE_POLICY,
  logGeneratedFileWritePolicy,
  writeGeneratedFile,
  CONTROLLER_PLURAL_OVERRIDES,
  pluralizeEntityShort,
  singularizeControllerSegment,
  getControllerClassName,
  getControllerRouteSegment,
  entityShortFromControllerClassName,
  matchControllerForEntityPrefix,
  entityBaseNameToScope,
  findDomainEntityFile,
  parseEntityBaseFromCsFile,
  resolveEntityScopeFromTypesInterface,
  resolveEntityScope,
  ENTITY_BASE_FIELDS,
  DEFAULT_BACKEND_ROOT,
};
