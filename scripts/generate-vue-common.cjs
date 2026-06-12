// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/scripts
// 文件名称：generate-vue-common.cjs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：Vue 三脚本共用基础设施（CLI + API/types/字段解析）；不含 index/form 模板
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const fs = require('fs');
const path = require('path');
const {
  writeGeneratedFile,
  getControllerClassName,
  resolveEntityScope,
  findDomainEntityFile,
  logGeneratedFileWritePolicy,
  parseSingleEntityGenerateArgsFromArgv,
  entityClassToSlug,
  buildEntityI18nKey,
} = require('./generate-script-common.cjs');
const {
  shouldExcludeDtoSourceBase,
  shouldExcludeVueGeneration,
  isChangeLogEntity,
  isStandaloneChildVueEntity,
  RBAC_ASSOCIATION_ENTITY_SHORT_NAMES,
} = require('./generate-entity-exclusions.cjs');

/** Vue 生成模板类型 */
const VUE_TEMPLATE = {
  CRUD: 'crud',
  TREE: 'tree',
  MASTER_DETAIL: 'master-detail',
};

const CONFIG = {
  frontendRoot: path.resolve(__dirname, '../frontend'),
  backendRoot: path.resolve(__dirname, '../backend/src'),
  apiDir: 'src/api',
  typesDir: 'src/types',
  viewsDir: 'src/views',
};

/**
 * 模板类型中文标签
 * @param {string} templateType
 */
function templateTypeLabel(templateType) {
  if (templateType === VUE_TEMPLATE.TREE) {
    return '树表 TREE';
  }
  if (templateType === VUE_TEMPLATE.MASTER_DETAIL) {
    return '主子表 Master-Detail';
  }
  return '单表 CRUD';
}

/**
 * PascalCase → kebab-case
 * @param {string} str
 */
function pascalToKebab(str) {
  return str.replace(/([a-z0-9])([A-Z])/g, '$1-$2').toLowerCase();
}

/**
 * @param {string|null} entityPrefix
 * @returns {string[]}
 */
function collectApiFiles(entityPrefix) {
  const root = path.join(CONFIG.frontendRoot, CONFIG.apiDir);
  /** @type {string[]} */
  const files = [];
  function scan(dir) {
    for (const entry of fs.readdirSync(dir, { withFileTypes: true })) {
      const full = path.join(dir, entry.name);
      if (entry.isDirectory()) {
        scan(full);
      } else if (entry.name.endsWith('.ts') && entry.name !== 'request.ts') {
        if (entityPrefix) {
          const expected = `${pascalToKebab(entityPrefix)}.ts`;
          if (entry.name !== expected) {
            continue;
          }
        }
        files.push(full);
      }
    }
  }
  scan(root);
  return files;
}

/**
 * kebab 文件名 → PascalCase（与 generate-vue-crud-from-api 一致）
 * @param {string} str
 */
function kebabToPascal(str) {
  return str
    .split('-')
    .filter(Boolean)
    .map((part) => part.charAt(0).toUpperCase() + part.slice(1))
    .join('');
}

/**
 * 解析 CLI 参数（单实体，禁止 --all）
 * @param {() => void} printUsage
 * @returns {{ entityPrefix: string, force: boolean, dryRun: boolean, viewPath: string|null }}
 */
function parseVueCliArgs(printUsage) {
  return parseSingleEntityGenerateArgsFromArgv(process.argv.slice(2), printUsage, { allowViewPath: true });
}

/**
 * 运行 Vue 生成 CLI
 * @param {object} opts
 * @param {string} opts.banner 启动横幅
 * @param {() => void} opts.printUsage
 * @param {string} opts.templateType VUE_TEMPLATE.*
 * @param {(apiFilePath: string, options: object, registry: Map) => object} opts.processModule
 * @param {() => Map} opts.buildRegistry
 * @param {() => void} [opts.onInit]
 */
function runVueGeneratorCli(opts) {
  console.log(opts.banner);
  logGeneratedFileWritePolicy();
  if (opts.onInit) {
    opts.onInit();
  }
  const registry = opts.buildRegistry();
  const options = parseVueCliArgs(opts.printUsage);
  const apiFiles = collectApiFiles(options.entityPrefix);
  if (apiFiles.length === 0) {
    console.error('❌ 未找到匹配的 API 文件');
    process.exit(1);
  }
  let created = 0;
  let skipped = 0;
  apiFiles.forEach((file) => {
    const result = opts.processModule(file, options, registry);
    if (result.skipped) {
      skipped += 1;
    } else {
      created += 1;
    }
  });
  console.log(`\n✨ 完成（${templateTypeLabel(opts.templateType)}）：生成 ${created} 个模块，跳过 ${skipped} 个`);
}


const SKIP_LIST_FIELDS = new Set([
  'tenantCode',
  'companyCode',
  'companyDefaultCulture',
  'createdAt',
  'updatedAt',
  'createdBy',
  'updatedBy',
  'isDeleted',
  'extFieldJson',
  'orderNum',
  'sortOrder',
]);

const SKIP_FORM_FIELDS = new Set([
  'createdAt',
  'updatedAt',
  'createdBy',
  'updatedBy',
  'isDeleted',
]);

/** CreateDto 上下文隔离字段（与 generate-dtos-from-entity 固定字段对齐，表单只读自动注入） */
const SCOPE_FORM_FIELD_NAMES = ['tenantCode', 'companyCode', 'companyDefaultCulture'];

/** 租户/公司实体本身：表单中隔离字段可编辑 */
const SCOPE_FIELD_EDITABLE_ENTITIES = new Set(['Tenant', 'Company']);

const SKIP_QUERY_FIELDS = new Set([
  'pageIndex',
  'pageSize',
  'keyWords',
  'KeyWords',
  'tenantCode',
  'companyCode',
]);

const TEXTAREA_NAME_HINTS = ['remark', 'quote', 'description', 'content', 'note', 'greeting', 'address', 'scope'];

/** 表单 Tabs 分页标准：每 Tab 最多字段数（2 列布局，约 5 行 × 2 列 = 10 项） */
const FORM_TAB_FIELDS_PER_TAB = 10;

/** @type {Map<string, { componentPath: string, permissionPrefix: string }>} */
const MENU_INDEX = new Map();

/** 与 Domain 实体「导航属性区域」标记对齐 */
const NAVIGATION_REGION_MARKER = '导航属性区域';

/**
 * PascalCase 转 camelCase
 * @param {string} str
 * @returns {string}
 */
function pascalToCamel(str) {
  return str.charAt(0).toLowerCase() + str.slice(1);
}

/**
 * PascalCase 转 kebab-case
 * @param {string} str
 * @returns {string}
 */
function pascalToKebab(str) {
  return str.replace(/([a-z0-9])([A-Z])/g, '$1-$2').toLowerCase();
}

/** 通用实体字段 → common.page.entity.* 完整翻译键（与 TaktCommonI18nSeedData 对齐） */
const COMMON_ENTITY_FIELD_T_KEYS = {
  remark: 'common.page.entity.remark',
  extFieldJson: 'common.page.entity.extfieldjson',
  tenantCode: 'common.page.entity.tenantcode',
  companyCode: 'common.page.entity.companycode',
  companyDefaultCulture: 'common.page.entity.companydefaultculture',
  createdAtStart: 'common.page.entity.createdatstart',
  createdAtEnd: 'common.page.entity.createdatend',
};

/**
 * 解析字段完整 i18n 键（remark / extFieldJson 等走 common.page.entity.*，其余走 entity.{slug}.*）
 * 与 generate-entity-i18n-seed.cjs / TaktXxxI18nSeedData 键规则一致（slug 全小写 + 末段别名）
 * @param {string} name 属性 camelCase
 * @param {string} entityI18nSlug 实体 slug（全小写，如 itasset）
 * @returns {string}
 */
function resolveFieldTranslationKey(name, entityI18nSlug) {
  if (COMMON_ENTITY_FIELD_T_KEYS[name]) {
    return COMMON_ENTITY_FIELD_T_KEYS[name];
  }
  return buildEntityI18nKey(entityI18nSlug, name);
}

/**
 * 生成字段 label 的 t() 表达式（i18nKey 已是完整键，含 common.page.entity.* / entity.*）
 * @param {{ i18nKey: string }} field
 * @returns {string}
 */
function fieldLabelTExpr(field) {
  return `t('${field.i18nKey}')`;
}

/**
 * 生成带字段名的 placeholder t() 表达式（如「请输入备注」= required + field: t('common.page.entity.remark')）
 * @param {{ i18nKey: string }} field
 * @param {string} placeholderKey common.page.form.placeholder.*
 * @returns {string}
 */
function fieldPlaceholderTExpr(field, placeholderKey) {
  return `t('${placeholderKey}', { field: ${fieldLabelTExpr(field)} })`;
}

/**
 * 隔离字段在表单中是否只读（Tenant / Company 实体本身可编辑）
 * @param {string} entityPascal
 * @param {string} fieldName
 * @returns {boolean}
 */
function isScopeFieldReadOnly(entityPascal, fieldName) {
  if (fieldName === 'companyDefaultCulture') {
    return true;
  }
  if (!SCOPE_FORM_FIELD_NAMES.includes(fieldName)) {
    return false;
  }
  return !SCOPE_FIELD_EDITABLE_ENTITIES.has(entityPascal);
}

/**
 * CreateDto 是否含上下文隔离字段（主表或子表）
 * @param {object[]} formFields
 * @param {object[]} [masterDetailChildren]
 * @returns {boolean}
 */
function hasScopeContextFormFields(formFields, masterDetailChildren = []) {
  const hasScope = (fields) => fields.some((f) => SCOPE_FORM_FIELD_NAMES.includes(f.name));
  return hasScope(formFields) || masterDetailChildren.some((c) => hasScope(c.formFields || []));
}

/**
 * 从 Create 属性提取租户/公司隔离字段（置于表单首位）
 * @param {object[]} createProperties
 * @param {string} entityPascal
 * @returns {object[]}
 */
function buildScopeFormFields(createProperties, entityPascal) {
  const propsByName = new Map((createProperties || []).map((p) => [p.name, p]));
  return SCOPE_FORM_FIELD_NAMES
    .filter((name) => propsByName.has(name))
    .map((name) => ({
      ...propsByName.get(name),
      readOnly: isScopeFieldReadOnly(entityPascal, name),
      optional: true,
    }));
}

/**
 * 表单控件只读/disabled 属性（隔离字段展示当前租户/公司）
 * @param {{ readOnly?: boolean, htmlType?: string }} field
 * @param {string} indent
 * @returns {string}
 */
function renderReadOnlyControlAttrs(field, indent) {
  if (!field.readOnly) {
    return '';
  }
  if (field.htmlType === 'switch' || field.htmlType === 'select' || field.htmlType === 'date') {
    return `\n${indent}  disabled`;
  }
  return `\n${indent}  readonly`;
}

/**
 * 表单 Tab 数量（每 Tab 最多 FORM_TAB_FIELDS_PER_TAB 个字段）
 * @param {number} fieldCount
 * @returns {number}
 */
function computeFormTabCount(fieldCount) {
  return Math.max(1, Math.ceil(fieldCount / FORM_TAB_FIELDS_PER_TAB));
}

/**
 * 表单 Tab 标签（多 Tab 时追加 当前/总数）
 * @param {number} tabIndex 1-based
 * @param {number} tabCount
 * @returns {string}
 */
function buildFormTabLabelAttr(tabIndex, tabCount) {
  if (tabCount <= 1) {
    return `:tab="t('common.page.form.tabs.basicinfo')"`;
  }
  return `:tab="t('common.page.form.tabs.basicinfo') + ' (${tabIndex}/${tabCount})'"`;
}

/**
 * 表单内容区高度 class（超过单 Tab 标准容量时用 10 行布局）
 * @returns {string} 生成到 *-form.vue 的 computed 表达式
 */
function buildFormContentClassComputedExpr() {
  return `computed(() => (formFields.length > ${FORM_TAB_FIELDS_PER_TAB} ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))`;
}

/**
 * 扫描菜单种子，建立 entityKebab → ComponentPath / Permission 索引
 */
function buildMenuIndex() {
  MENU_INDEX.clear();
  const seedsDir = path.join(CONFIG.backendRoot, 'Takt.Infrastructure', 'Data', 'Seeds', 'EntitySeedData');
  if (!fs.existsSync(seedsDir)) {
    return;
  }
  const files = fs.readdirSync(seedsDir).filter((f) => f.startsWith('TaktMenu') && f.endsWith('.cs'));
  const componentRe = /ComponentPath\s*=\s*"([^"]+)"/g;
  const permissionRe = /Permission\s*=\s*"([^"]+)"/g;
  files.forEach((file) => {
    const content = fs.readFileSync(path.join(seedsDir, file), 'utf-8');
    const components = [...content.matchAll(componentRe)].map((m) => m[1]);
    const permissions = [...content.matchAll(permissionRe)].map((m) => m[1]);
    components.forEach((componentPath, idx) => {
      if (!componentPath || !componentPath.endsWith('/index')) {
        return;
      }
      const segments = componentPath.split('/');
      const entityKebab = segments[segments.length - 1] === 'index'
        ? segments[segments.length - 2]
        : segments[segments.length - 1];
      if (!entityKebab) {
        return;
      }
      const perm = permissions[idx] || permissions.find((p) => p.endsWith(`:${entityKebab}:list`));
      const permissionPrefix = perm ? perm.replace(/:list$/, '') : '';
      const viewModulePath = componentPath.replace(/\/index$/, '');
      MENU_INDEX.set(entityKebab, { componentPath: viewModulePath, permissionPrefix });
    });
  });
}

/**
 * 从后端控制器解析权限前缀（取 list 权限去掉末段）
 * @param {string} entityShort
 * @returns {string}
 */
function resolvePermissionPrefixFromController(entityShort) {
  const controllerName = getControllerClassName(`Takt${entityShort}`);
  const controllersRoot = path.join(CONFIG.backendRoot, 'Takt.WebApi', 'Controllers');
  let found = '';
  function scan(dir) {
    if (found) {
      return;
    }
    for (const entry of fs.readdirSync(dir, { withFileTypes: true })) {
      const full = path.join(dir, entry.name);
      if (entry.isDirectory()) {
        scan(full);
      } else if (entry.name === `${controllerName}.cs`) {
        const content = fs.readFileSync(full, 'utf-8');
        const match = content.match(/\[TaktPermission\("([^"]+):list"/);
        if (match) {
          found = match[1];
        }
        return;
      }
    }
  }
  if (fs.existsSync(controllersRoot)) {
    scan(controllersRoot);
  }
  return found;
}

/**
 * 解析 .d.ts 中的 export interface
 * @param {string} content
 * @returns {Map<string, { name: string, properties: Array<{ name: string, type: string, optional: boolean, doc: string }> }>}
 */
function parseTypeInterfaces(content) {
  /** @type {Map<string, { name: string, properties: Array<{ name: string, type: string, optional: boolean, doc: string }> }>} */
  const interfaces = new Map();
  const chunks = content.split(/export interface /);
  chunks.slice(1).forEach((chunk) => {
    const nameMatch = chunk.match(/^(\w+)/);
    if (!nameMatch) {
      return;
    }
    const name = nameMatch[1];
    const bodyStart = chunk.indexOf('{');
    const bodyEnd = chunk.indexOf('\n}');
    if (bodyStart < 0 || bodyEnd < 0) {
      return;
    }
    const body = chunk.slice(bodyStart + 1, bodyEnd);
    const properties = [];
    const propRe = /\/\*\*([\s\S]*?)\*\/\s*(\w+)(\?)?:\s*([^;]+);/g;
    let m;
    while ((m = propRe.exec(body)) !== null) {
      properties.push({
        name: m[2],
        optional: Boolean(m[3]),
        type: m[4].trim(),
        doc: m[1].replace(/^\s*\*\s?/gm, '').replace(/\s+/g, ' ').trim(),
      });
    }
    interfaces.set(name, { name, properties });
  });
  return interfaces;
}

/**
 * 从 JSDoc 提取字典类型编码
 * @param {string} doc
 * @returns {string}
 */
function extractDictType(doc) {
  const match = doc.match(/字典\s+([a-z0-9_]+)/i);
  return match ? match[1] : '';
}

/**
 * DTO/类型 JSDoc 是否为响应 DTO 填充字段（服务层 Fill，不参与列表/表单/查询）
 * 与 generate-dtos-from-entity 中「（填充字段）」注释对齐
 * @param {string} doc
 * @returns {boolean}
 */
function isDtoFillField(doc) {
  return /填充字段/.test(doc || '');
}

/** sys_normal_disable：注释含「1=启用…0=禁用」的通用状态字段 */
const COMMON_STATUS_DICT_TYPE = 'sys_normal_disable';

/**
 * 是否为启用/禁用通用状态（对齐 TaktCommonStatus）
 * @param {string} doc
 * @returns {boolean}
 */
function isCommonEnableDisableStatus(doc) {
  if (!doc) {
    return false;
  }
  return /1\s*=\s*启用/.test(doc) && /0\s*=\s*禁用/.test(doc);
}

/**
 * 解析字典类型：显式「字典 xxx」或 TaktCommonStatus 语义
 * @param {{ name: string, doc: string }} field
 * @returns {string}
 */
function resolveDictType(field) {
  const explicit = extractDictType(field.doc);
  if (explicit) {
    return explicit;
  }
  if (isCommonEnableDisableStatus(field.doc)) {
    return COMMON_STATUS_DICT_TYPE;
  }
  return '';
}

/**
 * 推断表单控件类型
 * @param {{ name: string, type: string, doc: string }} field
 * @returns {'select'|'textarea'|'date'|'switch'|'input'}
 */
function inferHtmlType(field) {
  const dict = resolveDictType(field);
  if (dict) {
    return 'select';
  }
  if (field.type === 'boolean') {
    return 'switch';
  }
  const lower = field.name.toLowerCase();
  if (/Start$/.test(field.name) || /End$/.test(field.name)) {
    if (/date|time|validfrom|validto|createdat|updatedat/i.test(lower)) {
      return 'date';
    }
  }
  if (lower.includes('date') && field.type === 'string') {
    return 'date';
  }
  if (TEXTAREA_NAME_HINTS.some((hint) => lower.includes(hint))) {
    return 'textarea';
  }
  if (field.type === 'number') {
    return 'select';
  }
  return 'input';
}

/**
 * 解析 API 文件导出函数与 API_BASE
 * @param {string} content
 * @returns {{ apiBase: string, methods: Record<string, string> }}
 */
function parseApiFile(content) {
  const apiBaseMatch = content.match(/const\s+\w+_API_BASE\s*=\s*'([^']+)'/);
  const apiBase = apiBaseMatch ? apiBaseMatch[1] : '';
  /** @type {Record<string, string>} */
  const methods = {};
  const fnRe = /export function (\w+)\(/g;
  let m;
  while ((m = fnRe.exec(content)) !== null) {
    methods[m[1]] = m[1];
  }
  return { apiBase, methods };
}

/**
 * 根据 API 方法名推断能力开关
 * @param {string} entityPascal
 * @param {Record<string, string>} methods
 */
function detectApiCapabilities(entityPascal, methods) {
  const names = Object.keys(methods);
  const camel = pascalToCamel(entityPascal);
  const pick = (...candidates) => candidates.find((c) => names.includes(c)) || '';
  return {
    hasGetById: Boolean(pick(`get${entityPascal}ById`)),
    apiGetById: pick(`get${entityPascal}ById`),
    hasGetList: Boolean(pick(`get${entityPascal}List`)),
    hasGetOptions: Boolean(pick(`get${entityPascal}Options`)),
    hasCreate: Boolean(pick(`create${entityPascal}`)),
    hasUpdate: Boolean(pick(`update${entityPascal}`)),
    hasDelete: Boolean(pick(`delete${entityPascal}ById`)),
    hasDeleteBatch: Boolean(pick(`delete${entityPascal}Batch`)),
    hasGetTemplate: Boolean(pick(`get${entityPascal}Template`)),
    hasImport: Boolean(pick(`import${entityPascal}`, `import${entityPascal}Data`)),
    hasExport: Boolean(pick(`export${entityPascal}`, `export${entityPascal}Data`)),
    apiGetList: pick(`get${entityPascal}List`),
    apiCreate: pick(`create${entityPascal}`),
    apiUpdate: pick(`update${entityPascal}`),
    apiDelete: pick(`delete${entityPascal}ById`),
    apiDeleteBatch: pick(`delete${entityPascal}Batch`),
    apiGetTemplate: pick(`get${entityPascal}Template`),
    apiImport: pick(`import${entityPascal}`, `import${entityPascal}Data`),
    apiExport: pick(`export${entityPascal}`, `export${entityPascal}Data`),
    entityIdName: `${camel}Id`,
    entityClassName: `Takt${entityPascal}`,
  };
}

/**
 * 解析 Type[] 元素类型名
 * @param {string} typeStr
 * @returns {string|null}
 */
function parseArrayElementType(typeStr) {
  const match = typeStr.match(/^(\w+)\[\]$/);
  return match ? match[1] : null;
}

/**
 * 从 C# 实体文件提取 class 体
 * @param {string} content
 * @param {string} className
 * @returns {string}
 */
function extractClassBlock(content, className) {
  const startRegex = new RegExp(`public\\s+(?:sealed\\s+|partial\\s+)?class\\s+${className}\\b`);
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
 * 拆分实体 class 体：标量区 / 导航属性区
 * @param {string} classBody
 */
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

/**
 * 解析 Domain 实体 OneToMany 导航（与 generate-services-from-dtos 对齐）
 * @param {string} entityFile
 * @returns {Array<{ navPropName: string, childEntity: string, childShort: string, foreignKeyOnChild: string }>}
 */
function parseOneToManyNavigations(entityFile) {
  const content = fs.readFileSync(entityFile, 'utf-8');
  const classMatch = content.match(/public\s+(?:sealed\s+)?class\s+(Takt\w+)\s*:\s*\w+/);
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
  /** @type {Array<{ navPropName: string, childEntity: string, childShort: string, foreignKeyOnChild: string }>} */
  const navigations = [];
  let match;
  while ((match = navRegex.exec(body)) !== null) {
    navigations.push({
      foreignKeyOnChild: match[1],
      childEntity: match[3],
      childShort: match[3].replace(/^Takt/, ''),
      navPropName: match[4],
    });
  }
  return navigations;
}

/**
 * 子表 types 文件路径（跨文件 Create/Entity 类型）
 * @param {string} modulePath
 * @param {string} childPascal
 */
function resolveChildTypesFilePath(modulePath, childPascal) {
  const childKebab = pascalToKebab(childPascal);
  return path.join(CONFIG.frontendRoot, CONFIG.typesDir, modulePath, `${childKebab}.d.ts`);
}

/**
 * 子表 types 是否存在于 frontend/types
 * @param {string} modulePath
 * @param {string} childPascal
 */
function childTypesFileExists(modulePath, childPascal) {
  return fs.existsSync(resolveChildTypesFilePath(modulePath, childPascal));
}

/**
 * 构建主子表子实体元数据（fieldName / childPascal / kebab 等）
 * @param {string} fieldName
 * @param {string} childPascal
 * @param {string} [childTypeOverride]
 */
function buildMasterDetailChildMeta(fieldName, childPascal, childTypeOverride) {
  const childCamel = pascalToCamel(childPascal);
  const childType = childTypeOverride || childPascal;
  return {
    fieldName,
    childPascal,
    childCreateType: `${childPascal}Create`,
    childType,
    childCamel,
    childI18nSlug: entityClassToSlug(`Takt${childPascal}`),
    childKebab: pascalToKebab(childPascal),
    childIdField: `${childCamel}Id`,
  };
}

/**
 * 检测 Create DTO 中的 OneToMany 子表（子 Create 类型可在独立 .d.ts 中）
 * @param {ReturnType<typeof parseTypeInterfaces>} interfaces
 * @param {string} entityPascal
 * @param {string} [modulePath]
 * @returns {Array<{ fieldName: string, childPascal: string, childCreateType: string, childType: string, childCamel: string, childKebab: string, childIdField: string }>}
 */
function detectMasterDetailChildren(interfaces, entityPascal, modulePath = '') {
  const createIface = interfaces.get(`${entityPascal}Create`);
  const entityIface = interfaces.get(entityPascal);
  if (!createIface) {
    return [];
  }
  /** @type {Array<{ fieldName: string, childPascal: string, childCreateType: string, childType: string, childCamel: string, childKebab: string, childIdField: string }>} */
  const children = [];
  createIface.properties.forEach((prop) => {
    const elementType = parseArrayElementType(prop.type);
    if (!elementType || !elementType.endsWith('Create')) {
      return;
    }
    const childPascal = elementType.slice(0, -'Create'.length);
    if (!childPascal || childPascal === entityPascal) {
      return;
    }
    if (RBAC_ASSOCIATION_ENTITY_SHORT_NAMES.has(childPascal)) {
      return;
    }
    const hasCreateType = interfaces.has(elementType) || childTypesFileExists(modulePath, childPascal);
    if (!hasCreateType) {
      return;
    }
    const entityProp = entityIface?.properties.find((p) => p.name === prop.name);
    const entityElement = entityProp ? parseArrayElementType(entityProp.type) : null;
    const isChildEntity = Boolean(
      entityElement
      && entityElement !== entityPascal
      && (interfaces.has(entityElement) || childTypesFileExists(modulePath, childPascal)),
    );
    const docHint = /子表|一对多|级联|外键|关联|主子表/.test(`${prop.doc || ''}${entityProp?.doc || ''}`);
    if (!isChildEntity && !docHint) {
      return;
    }
    children.push(buildMasterDetailChildMeta(prop.name, childPascal, entityElement || childPascal));
  });
  return children;
}

/**
 * 从 Domain 实体 OneToMany 解析主子表子实体（权威来源，对齐字典 DictType ↔ DictData）
 * @param {string} entityPascal
 * @param {string} [modulePath]
 * @param {ReturnType<typeof parseTypeInterfaces>} [interfaces]
 */
function detectMasterDetailChildrenFromEntity(entityPascal, modulePath = '', interfaces = new Map()) {
  const entityFile = findDomainEntityFile(entityPascal, CONFIG.backendRoot);
  if (!entityFile) {
    return [];
  }
  const fromTypes = detectMasterDetailChildren(interfaces, entityPascal, modulePath);
  return parseOneToManyNavigations(entityFile)
    .filter((nav) => !RBAC_ASSOCIATION_ENTITY_SHORT_NAMES.has(nav.childShort))
    .map((nav) => {
      const typesChild = fromTypes.find((c) => c.childPascal === nav.childShort);
      const fieldName = typesChild?.fieldName || pascalToCamel(nav.navPropName);
      return buildMasterDetailChildMeta(fieldName, nav.childShort, typesChild?.childType);
    });
}

/**
 * 合并 Domain 实体与 types 的主子表子实体列表（实体 OneToMany 优先）
 * @param {ReturnType<typeof parseTypeInterfaces>} interfaces
 * @param {string} entityPascal
 * @param {string} modulePath
 */
function resolveMasterDetailChildren(interfaces, entityPascal, modulePath) {
  const fromEntity = detectMasterDetailChildrenFromEntity(entityPascal, modulePath, interfaces);
  if (fromEntity.length) {
    return fromEntity;
  }
  return detectMasterDetailChildren(interfaces, entityPascal, modulePath);
}

/**
 * 子表 Query 中指向主表的外键字段（默认 {masterCamel}Id）
 * @param {ReturnType<typeof parseTypeInterfaces>} interfaces
 * @param {string} childPascal
 * @param {string} masterCamel
 */
function resolveChildMasterFkField(interfaces, childPascal, masterCamel) {
  const query = interfaces.get(`${childPascal}Query`);
  const fkId = `${masterCamel}Id`;
  if (query?.properties.some((p) => p.name === fkId)) {
    return fkId;
  }
  const fkCode = `${masterCamel}Code`;
  if (query?.properties.some((p) => p.name === fkCode)) {
    return fkCode;
  }
  return fkId;
}

/**
 * 解析子表 list API 方法名
 * @param {string} modulePath
 * @param {string} childKebab
 * @param {string} childPascal
 */
function resolveChildListApiMethod(modulePath, childKebab, childPascal) {
  const apiPath = path.join(CONFIG.frontendRoot, CONFIG.apiDir, modulePath, `${childKebab}.ts`);
  if (!fs.existsSync(apiPath)) {
    return '';
  }
  const { methods } = parseApiFile(fs.readFileSync(apiPath, 'utf-8'));
  const expected = `get${childPascal}List`;
  return Object.prototype.hasOwnProperty.call(methods, expected) ? expected : '';
}

/**
 * 递归收集 types 目录下全部实体 .d.ts
 * @param {string} [rootDir]
 */
function collectAllTypesFiles(rootDir = path.join(CONFIG.frontendRoot, CONFIG.typesDir)) {
  /** @type {string[]} */
  const files = [];
  /** @param {string} dir */
  function scan(dir) {
    for (const entry of fs.readdirSync(dir, { withFileTypes: true })) {
      const full = path.join(dir, entry.name);
      if (entry.isDirectory()) {
        scan(full);
      } else if (entry.name.endsWith('.d.ts') && entry.name !== 'common.d.ts') {
        files.push(full);
      }
    }
  }
  if (fs.existsSync(rootDir)) {
    scan(rootDir);
  }
  return files;
}

/**
 * 递归收集 Domain Entities 下全部 Takt*.cs
 * @param {string} [rootDir]
 */
function collectDomainEntityFiles(rootDir = path.join(CONFIG.backendRoot, 'Takt.Domain', 'Entities')) {
  /** @type {string[]} */
  const files = [];
  if (!fs.existsSync(rootDir)) {
    return files;
  }
  /** @param {string} dir */
  function scan(dir) {
    for (const entry of fs.readdirSync(dir, { withFileTypes: true })) {
      const full = path.join(dir, entry.name);
      if (entry.isDirectory()) {
        scan(full);
      } else if (entry.name.startsWith('Takt') && entry.name.endsWith('.cs')) {
        files.push(full);
      }
    }
  }
  scan(rootDir);
  return files;
}

/**
 * 扫描全部 types + Domain 实体，建立「从实体 → 主实体」映射（OneToMany 子表不单独生成 Vue）
 * @returns {Map<string, { masterPascal: string, fieldName: string }>}
 */
function buildMasterDetailChildRegistry() {
  /** @type {Map<string, { masterPascal: string, fieldName: string }>} */
  const registry = new Map();
  collectDomainEntityFiles().forEach((entityFile) => {
    const content = fs.readFileSync(entityFile, 'utf-8');
    const classMatch = content.match(/public\s+(?:sealed\s+)?class\s+Takt(\w+)\s*:/);
    if (!classMatch) {
      return;
    }
    const masterPascal = classMatch[1];
    parseOneToManyNavigations(entityFile).forEach((nav) => {
      if (RBAC_ASSOCIATION_ENTITY_SHORT_NAMES.has(nav.childShort)) {
        return;
      }
      registry.set(nav.childShort, {
        masterPascal,
        fieldName: pascalToCamel(nav.navPropName),
      });
    });
  });
  for (const typesFile of collectAllTypesFiles()) {
    const relTypes = path.relative(path.join(CONFIG.frontendRoot, CONFIG.typesDir), typesFile).replace(/\\/g, '/');
    const modulePath = path.dirname(relTypes);
    const entityKebab = path.basename(typesFile, '.d.ts');
    const entityPascal = kebabToPascal(entityKebab);
    const content = fs.readFileSync(typesFile, 'utf-8');
    const interfaces = parseTypeInterfaces(content);
    if (!interfaces.has(entityPascal)) {
      continue;
    }
    resolveMasterDetailChildren(interfaces, entityPascal, modulePath === '.' ? '' : modulePath).forEach((child) => {
      if (!registry.has(child.childPascal)) {
        registry.set(child.childPascal, {
          masterPascal: entityPascal,
          fieldName: child.fieldName,
        });
      }
    });
  }
  return registry;
}

/**
 * 子表 Create 表单字段（排除主表外键与审计字段）
 * @param {ReturnType<typeof parseTypeInterfaces>} interfaces
 * @param {string} childPascal
 * @param {string} masterCamel
 */
function buildChildFormFieldProps(interfaces, childPascal, masterCamel) {
  const create = interfaces.get(`${childPascal}Create`);
  if (!create) {
    return [];
  }
  const masterFkNames = new Set([`${masterCamel}Id`, `${masterCamel}Code`]);
  const scopeFields = buildScopeFormFields(create.properties || [], childPascal);
  const scopeNames = new Set(scopeFields.map((f) => f.name));
  const rest = (create.properties || []).filter((p) => {
    if (scopeNames.has(p.name) || SKIP_FORM_FIELDS.has(p.name)) {
      return false;
    }
    if (isDtoFillField(p.doc)) {
      return false;
    }
    if (p.name === `${pascalToCamel(childPascal)}Id`) {
      return false;
    }
    if (masterFkNames.has(p.name)) {
      return false;
    }
    return true;
  }).slice(0, 8);
  return [...scopeFields, ...rest];
}

/**
 * 列表列跳过字段（Tenant/Company 管理页保留隔离字段列）
 * @param {string} entityPascal
 * @returns {Set<string>}
 */
function getSkipListFields(entityPascal) {
  const skip = new Set(SKIP_LIST_FIELDS);
  if (entityPascal === 'Tenant') {
    skip.delete('tenantCode');
  }
  if (entityPascal === 'Company') {
    skip.delete('tenantCode');
    skip.delete('companyCode');
  }
  return skip;
}

/**
 * 构建实体字段元数据
 * @param {ReturnType<typeof parseTypeInterfaces>} interfaces
 * @param {string} entityPascal
 * @param {string} [typesContent]
 */
function buildFieldMeta(interfaces, entityPascal, typesContent = '', modulePath = '') {
  const skipListFields = getSkipListFields(entityPascal);
  const entityScope = resolveEntityScope(entityPascal, typesContent, CONFIG.backendRoot);
  const entity = interfaces.get(entityPascal);
  const create = interfaces.get(`${entityPascal}Create`);
  const query = interfaces.get(`${entityPascal}Query`);
  const masterDetailChildren = resolveMasterDetailChildren(interfaces, entityPascal, modulePath);
  const childFieldNames = new Set(masterDetailChildren.map((c) => c.fieldName));
  const entityCamel = pascalToCamel(entityPascal);
  const entityI18nSlug = entityClassToSlug(`Takt${entityPascal}`);
  const enrich = (fields, i18nSlug) => fields.map((f) => ({
    ...f,
    htmlType: inferHtmlType(f),
    dictType: resolveDictType(f),
    i18nKey: resolveFieldTranslationKey(f.name, i18nSlug),
  }));
  const listFields = (entity?.properties || []).filter((p) => {
    if (skipListFields.has(p.name) || childFieldNames.has(p.name)) {
      return false;
    }
    if (isDtoFillField(p.doc)) {
      return false;
    }
    return true;
  });
  const scopeFields = buildScopeFormFields(create?.properties || [], entityPascal);
  const scopeNames = new Set(scopeFields.map((f) => f.name));
  const formFieldsRaw = (create?.properties || []).filter((p) => {
    if (scopeNames.has(p.name) || SKIP_FORM_FIELDS.has(p.name)) {
      return false;
    }
    if (isDtoFillField(p.doc)) {
      return false;
    }
    if (p.name === `${pascalToCamel(entityPascal)}Id`) {
      return false;
    }
    if (childFieldNames.has(p.name)) {
      return false;
    }
    return true;
  });
  const queryFieldsRaw = (query?.properties || []).filter((p) => {
    if (SKIP_QUERY_FIELDS.has(p.name)) {
      return false;
    }
    if (isDtoFillField(p.doc)) {
      return false;
    }
    if (childFieldNames.has(p.name)) {
      return false;
    }
    return true;
  });
  const queryFields = enrich(queryFieldsRaw, entityI18nSlug);
  const enrichedChildren = masterDetailChildren.map((child) => {
    const childTypesPath = path.join(CONFIG.frontendRoot, CONFIG.typesDir, modulePath, `${child.childKebab}.d.ts`);
    const childTypesContent = fs.existsSync(childTypesPath) ? fs.readFileSync(childTypesPath, 'utf-8') : '';
    const childInterfaces = childTypesContent ? parseTypeInterfaces(childTypesContent) : interfaces;
    const childFormRaw = buildChildFormFieldProps(childInterfaces, child.childPascal, entityCamel);
    const childEntity = interfaces.get(child.childType) || childInterfaces.get(child.childType);
    const childListRaw = (childEntity?.properties || []).filter((p) => {
      if (SKIP_LIST_FIELDS.has(p.name)) {
        return false;
      }
      if (isDtoFillField(p.doc)) {
        return false;
      }
      if (p.name === child.childIdField || p.name === `${entityCamel}Id`) {
        return false;
      }
      return true;
    }).slice(0, 8);
    const childI18nSlug = child.childI18nSlug || entityClassToSlug(`Takt${child.childPascal}`);
    return {
      ...child,
      masterFkField: resolveChildMasterFkField(childInterfaces, child.childPascal, entityCamel),
      apiGetList: resolveChildListApiMethod(modulePath, child.childKebab, child.childPascal),
      formFields: enrich(childFormRaw, childI18nSlug),
      listFields: enrich(childListRaw, childI18nSlug),
    };
  });
  return {
    listFields: enrich(listFields, entityI18nSlug),
    formFields: enrich([...scopeFields, ...formFieldsRaw], entityI18nSlug),
    queryFields: enrich(queryFields, entityI18nSlug),
    masterDetailChildren: enrichedChildren,
    entityScope,
  };
}

/**
 * 解析模块上下文
 * @param {string} apiFilePath
 * @param {string} entityShort
 * @param {{ viewPath?: string|null }} overrides
 */
function resolveModuleContext(apiFilePath, entityShort, overrides) {
  const relApi = path.relative(path.join(CONFIG.frontendRoot, CONFIG.apiDir), apiFilePath).replace(/\\/g, '/');
  const modulePath = path.dirname(relApi);
  const entityKebab = path.basename(relApi, '.ts');
  const entityPascal = kebabToPascal(entityKebab);
  const menu = MENU_INDEX.get(entityKebab);
  const viewModulePath = overrides.viewPath
    || menu?.componentPath
    || `${modulePath}/${entityKebab}`;
  let permissionPrefix = menu?.permissionPrefix || resolvePermissionPrefixFromController(entityShort);
  if (!permissionPrefix) {
    permissionPrefix = `${modulePath.replace(/\//g, ':')}:${entityKebab}`;
  }
  const entityCamel = pascalToCamel(entityPascal);
  const entityI18nSlug = entityClassToSlug(`Takt${entityPascal}`);
  return {
    modulePath,
    viewModulePath,
    entityKebab,
    entityPascal,
    entityCamel,
    entityI18nSlug,
    entitySlug: entityCamel,
    permissionPrefix,
    cssRootClass: viewModulePath.replace(/\//g, '-'),
  };
}

/**
 * 生成表单控件模板片段（主子表子行 / 主表共用）
 * @param {{ name: string, htmlType: string, dictType: string, i18nKey: string, optional: boolean }} field
 * @param {string} modelPrefix 如 formState. / record.
 * @param {string} indent
 * @returns {string}
 */
function renderFormControl(field, modelPrefix, indent = '                ') {
  const readOnlyAttrs = renderReadOnlyControlAttrs(field, indent);
  const clearAttr = field.readOnly ? '' : `\n${indent}  allow-clear`;
  if (field.htmlType === 'select' && field.dictType) {
    return `${indent}<TaktSelect
${indent}  v-model:value="${modelPrefix}${field.name}"
${indent}  dict-type="${field.dictType}"
${indent}  :placeholder="${fieldPlaceholderTExpr(field, 'common.page.form.placeholder.select')}"
${indent}  size="small"${readOnlyAttrs}
${indent}/>`;
  }
  if (field.htmlType === 'textarea') {
    return `${indent}<a-textarea
${indent}  v-model:value="${modelPrefix}${field.name}"
${indent}  :placeholder="${fieldPlaceholderTExpr(field, 'common.page.form.placeholder.optional')}"
${indent}  :rows="2"
${indent}  size="small"${readOnlyAttrs}
${indent}/>`;
  }
  if (field.htmlType === 'date') {
    return `${indent}<a-date-picker
${indent}  v-model:value="${modelPrefix}${field.name}"
${indent}  :placeholder="${fieldPlaceholderTExpr(field, 'common.page.form.placeholder.select')}"
${indent}  value-format="YYYY-MM-DD"
${indent}  size="small"
${indent}  style="width: 100%"${readOnlyAttrs}
${indent}/>`;
  }
  if (field.htmlType === 'switch') {
    return `${indent}<a-switch v-model:checked="${modelPrefix}${field.name}" size="small"${readOnlyAttrs} />`;
  }
  if (field.type === 'number') {
    return `${indent}<a-input-number
${indent}  v-model:value="${modelPrefix}${field.name}"
${indent}  :placeholder="${fieldPlaceholderTExpr(field, 'common.page.form.placeholder.required')}"
${indent}  size="small"
${indent}  style="width: 100%"${readOnlyAttrs}
${indent}/>`;
  }
  return `${indent}<a-input
${indent}  v-model:value="${modelPrefix}${field.name}"
${indent}  :placeholder="${fieldPlaceholderTExpr(field, 'common.page.form.placeholder.required')}"
${indent}  size="small"${clearAttr}${readOnlyAttrs}
${indent}/>`;
}

/**
 * 生成高级查询抽屉表单项
 * @param {{ name: string, htmlType: string, dictType: string, i18nKey: string, type: string, optional?: boolean }} field
 * @returns {string}
 */
function renderQueryFormItem(field) {
  const body = renderQueryFormItemBody(field);
  return `      <div v-show="isFieldVisible('${field.name}')">
${body}
      </div>`;
}

/**
 * 生成高级查询抽屉表单项内容（不含显隐包裹）
 * @param {{ name: string, htmlType: string, dictType: string, i18nKey: string, type: string, optional?: boolean }} field
 * @returns {string}
 */
function renderQueryFormItemBody(field) {
  if (field.htmlType === 'select' && field.dictType) {
    return `      <a-form-item :label="${fieldLabelTExpr(field)}">
        <TaktSelect
          v-model:value="advancedQueryForm.${field.name}"
          dict-type="${field.dictType}"
          :placeholder="${fieldPlaceholderTExpr(field, 'common.page.form.placeholder.select')}"
          allow-clear
        />
      </a-form-item>`;
  }
  if (field.htmlType === 'textarea') {
    return `      <a-form-item :label="${fieldLabelTExpr(field)}">
        <a-textarea
          v-model:value="advancedQueryForm.${field.name}"
          :placeholder="${fieldPlaceholderTExpr(field, 'common.page.form.placeholder.optional')}"
          :rows="2"
          allow-clear
        />
      </a-form-item>`;
  }
  if (field.type === 'number') {
    return `      <a-form-item :label="${fieldLabelTExpr(field)}">
        <a-input-number
          v-model:value="advancedQueryForm.${field.name}"
          :placeholder="${fieldPlaceholderTExpr(field, 'common.page.form.placeholder.required')}"
          style="width: 100%"
        />
      </a-form-item>`;
  }
  if (field.htmlType === 'date') {
    const showTime = /createdat|updatedat|time/i.test(field.name);
    const valueFormat = showTime ? 'YYYY-MM-DD HH:mm:ss' : 'YYYY-MM-DD';
    const showTimeAttr = showTime ? '\n          show-time' : '';
    return `      <a-form-item :label="${fieldLabelTExpr(field)}">
        <a-date-picker
          v-model:value="advancedQueryForm.${field.name}"
          :placeholder="${fieldPlaceholderTExpr(field, 'common.page.form.placeholder.select')}"
          value-format="${valueFormat}"${showTimeAttr}
          style="width: 100%"
        />
      </a-form-item>`;
  }
  const placeholderKey = field.htmlType === 'date'
    ? 'common.page.form.placeholder.optional'
    : 'common.page.form.placeholder.required';
  return `      <a-form-item :label="${fieldLabelTExpr(field)}">
        <a-input
          v-model:value="advancedQueryForm.${field.name}"
          :placeholder="${fieldPlaceholderTExpr(field, placeholderKey)}"
          allow-clear
        />
      </a-form-item>`;
}

/**
 * Domain 实体是否含 ParentId（树形判定，三脚本共用）
 * @param {string} entityPascal
 * @param {string} [backendRoot]
 */
function entityHasParentId(entityPascal, backendRoot = CONFIG.backendRoot) {
  const entityFile = findDomainEntityFile(entityPascal, backendRoot);
  if (!entityFile) {
    return false;
  }
  const content = fs.readFileSync(entityFile, 'utf-8');
  return /public\s+long(?:\?)?\s+ParentId\s*\{/.test(content);
}

/**
 * 扩展 API 能力：树接口
 * @param {string} entityPascal
 * @param {Record<string, string>} methods
 */
function extendTreeApiCapabilities(entityPascal, methods) {
  const names = Object.keys(methods);
  const pick = (...candidates) => candidates.find((c) => names.includes(c)) || '';
  return {
    hasGetTree: Boolean(pick(`get${entityPascal}Tree`)),
    apiGetTree: pick(`get${entityPascal}Tree`),
    hasGetTreeOptions: Boolean(pick(`get${entityPascal}TreeOptions`)),
    apiGetTreeOptions: pick(`get${entityPascal}TreeOptions`),
    entityTreeType: `${entityPascal}Tree`,
  };
}

function writeVueModuleOutputs(bundle, indexContent, formContent, options) {
  if (options.dryRun) {
    console.log(`🔍 [dry-run] 将生成:\n  - ${bundle.indexPath}${formContent ? `\n  - ${bundle.formPath}` : ''}`);
    return { skipped: false, dryRun: true };
  }
  writeGeneratedFile(bundle.indexPath, indexContent);
  console.log(`✅ 已生成: ${bundle.indexPath}`);
  if (formContent) {
    writeGeneratedFile(bundle.formPath, formContent);
    console.log(`✅ 已生成: ${bundle.formPath}`);
  }
  return { skipped: false, created: true };
}

/**
 * 加载 API 模块公共上下文（不含模板过滤）
 */
function loadVueModuleContext(apiFilePath, options, masterDetailChildRegistry) {
  const rel = path.relative(path.join(CONFIG.frontendRoot, CONFIG.apiDir), apiFilePath).replace(/\\/g, '/');
  const entityKebab = path.basename(rel, '.ts');
  const entityShort = kebabToPascal(entityKebab);
  const dtoSourceBase = `Takt${entityShort}Dtos`;
  if (shouldExcludeDtoSourceBase(dtoSourceBase)) {
    console.log(`⏭️  跳过手工/排除模块: ${rel}`);
    return { skipped: true };
  }
  if (shouldExcludeVueGeneration(rel, entityShort)) {
    const reason = isChangeLogEntity(entityShort)
      ? 'ChangeLog 从属实体无独立视图/表单'
      : '架构约束跳过';
    console.log(`⏭️  跳过 Vue 生成（${reason}）: ${rel}`);
    return { skipped: true };
  }
  const masterRef = masterDetailChildRegistry.get(entityShort);
  if (masterRef && !isStandaloneChildVueEntity(entityShort)) {
    console.log(`⏭️  跳过主子表从实体: ${rel}（视图由主表 ${masterRef.masterPascal}.${masterRef.fieldName} 承载）`);
    return { skipped: true };
  }
  if (masterRef && isStandaloneChildVueEntity(entityShort)) {
    console.log(`ℹ️  从实体 ${entityShort} 仍有独立菜单页，继续生成 Vue（主表 ${masterRef.masterPascal}.${masterRef.fieldName} 展开区并存）`);
  }
  const typesPath = path.join(CONFIG.frontendRoot, CONFIG.typesDir, `${rel.replace(/\.ts$/, '.d.ts')}`);
  if (!fs.existsSync(typesPath)) {
    console.warn(`⚠️  缺少类型文件，跳过: ${typesPath}`);
    return { skipped: true };
  }
  const apiContent = fs.readFileSync(apiFilePath, 'utf-8');
  const typesContent = fs.readFileSync(typesPath, 'utf-8');
  const { methods, apiBase } = parseApiFile(apiContent);
  const interfaces = parseTypeInterfaces(typesContent);
  if (!interfaces.has(entityShort)) {
    console.warn(`⚠️  类型文件中未找到主实体 interface ${entityShort}，跳过: ${rel}`);
    return { skipped: true };
  }
  const caps = detectApiCapabilities(entityShort, methods);
  const treeCaps = extendTreeApiCapabilities(entityShort, methods);
  const capsMerged = { ...caps, ...treeCaps };
  const ctx = resolveModuleContext(apiFilePath, entityShort, options);
  const fields = buildFieldMeta(interfaces, entityShort, typesContent, ctx.modulePath);
  const comment = parseEntityComment(typesContent, entityShort);
  const create = interfaces.get(`${entityShort}Create`);
  const fullCtx = {
    ...ctx,
    caps: capsMerged,
    fields,
    comment,
    apiBase,
    treeMeta: null,
    updateDtoFields: (create?.properties || []).filter(
      (p) => !SKIP_FORM_FIELDS.has(p.name) && !isDtoFillField(p.doc) && p.name !== `${ctx.entitySlug}Id`,
    ),
  };
  const viewDir = path.join(CONFIG.frontendRoot, CONFIG.viewsDir, ctx.viewModulePath);
  const indexPath = path.join(viewDir, 'index.vue');
  const formPath = path.join(viewDir, 'components', `${ctx.entityKebab}-form.vue`);
  const needsForm = capsMerged.hasCreate || capsMerged.hasUpdate;
  return {
    skipped: false,
    rel,
    entityShort,
    fullCtx,
    indexPath,
    formPath,
    needsForm,
    interfaces,
    capsMerged,
    isTreeEntity: entityHasParentId(entityShort, CONFIG.backendRoot) && capsMerged.hasGetTree,
    isMasterDetailEntity: (fields.masterDetailChildren || []).length > 0,
  };
}

/**
 * 从 types 文件提取主实体 interface 上方 JSDoc 首行
 * @param {string} content
 * @param {string} entityName
 * @returns {string}
 */
function parseEntityComment(content, entityName) {
  const re = new RegExp(`/\\*\\*([\\s\\S]*?)\\*/\\s*export interface ${entityName}\\b`);
  const match = content.match(re);
  if (!match) {
    return entityName;
  }
  const firstLine = match[1]
    .split(/\r?\n/)
    .map((line) => line.replace(/^\s*\*\s?/, '').trim())
    .find((line) => line && !line.startsWith('@'));
  if (!firstLine) {
    return entityName;
  }
  return firstLine.split(/[，,。；;（(]/)[0].trim() || entityName;
}


module.exports = {
  VUE_TEMPLATE,
  CONFIG,
  templateTypeLabel,
  collectApiFiles,
  kebabToPascal,
  pascalToKebab,
  parseVueCliArgs,
  runVueGeneratorCli,
  SKIP_LIST_FIELDS,
  SKIP_FORM_FIELDS,
  SKIP_QUERY_FIELDS,
  FORM_TAB_FIELDS_PER_TAB,
  MENU_INDEX,
  pascalToCamel,
  entityClassToSlug,
  buildEntityI18nKey,
  resolveFieldTranslationKey,
  fieldLabelTExpr,
  fieldPlaceholderTExpr,
  hasScopeContextFormFields,
  buildScopeFormFields,
  renderReadOnlyControlAttrs,
  computeFormTabCount,
  buildFormTabLabelAttr,
  buildFormContentClassComputedExpr,
  buildMenuIndex,
  resolvePermissionPrefixFromController,
  parseApiFile,
  parseTypeInterfaces,
  isDtoFillField,
  detectApiCapabilities,
  entityHasParentId,
  extendTreeApiCapabilities,
  parseOneToManyNavigations,
  buildMasterDetailChildRegistry,
  resolveMasterDetailChildren,
  buildFieldMeta,
  resolveModuleContext,
  renderFormControl,
  renderQueryFormItem,
  parseEntityComment,
  loadVueModuleContext,
  writeVueModuleOutputs,
};
