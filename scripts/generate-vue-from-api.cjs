// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/scripts
// 文件名称：generate-vue-from-api.cjs
// 创建时间：2026-06-03
// 创建人：Takt365(Cursor AI)
// 功能描述：根据 frontend/src/types 与 frontend/src/api 自动生成 CRUD 视图 index.vue 与表单 *-form.vue
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const fs = require('fs');
const path = require('path');
const {
  writeGeneratedFile,
  getControllerClassName,
  logGeneratedFileWritePolicy,
} = require('./generate-script-common.cjs');
const {
  shouldExcludeDtoSourceBase,
  shouldExcludeVueGeneration,
  RBAC_ASSOCIATION_ENTITY_SHORT_NAMES,
} = require('./generate-entity-exclusions.cjs');

const CONFIG = {
  frontendRoot: path.resolve(__dirname, '../frontend'),
  backendRoot: path.resolve(__dirname, '../backend/src'),
  apiDir: 'src/api',
  typesDir: 'src/types',
  viewsDir: 'src/views',
};

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
  'createdAtStart',
  'createdAtEnd',
  'updatedAtStart',
  'updatedAtEnd',
  'extFieldJson',
]);

const TEXTAREA_NAME_HINTS = ['remark', 'quote', 'description', 'content', 'note', 'greeting', 'address', 'scope'];

/** 表单 Tabs 分页标准：每 Tab 最多字段数（2 列布局，约 5 行 × 2 列 = 10 项） */
const FORM_TAB_FIELDS_PER_TAB = 10;

/** @type {Map<string, { componentPath: string, permissionPrefix: string }>} */
const MENU_INDEX = new Map();

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

/**
 * kebab-case 转 PascalCase
 * @param {string} str
 * @returns {string}
 */
function kebabToPascal(str) {
  return str
    .split('-')
    .filter(Boolean)
    .map((part) => part.charAt(0).toUpperCase() + part.slice(1))
    .join('');
}

/**
 * 字段名转 entity i18n 末段（全小写）
 * @param {string} name
 * @returns {string}
 */
function fieldI18nKey(name, entitySlug) {
  if (!entitySlug) {
    return name.toLowerCase();
  }
  const prefix = entitySlug.toLowerCase();
  const lower = name.toLowerCase();
  if (lower.startsWith(prefix) && name.length > prefix.length) {
    const rest = name.slice(prefix.length);
    return (rest.charAt(0).toLowerCase() + rest.slice(1)).toLowerCase();
  }
  return name.toLowerCase();
}

/** 通用实体字段 → common.page.entity.* 完整翻译键（与 TaktCommonI18nSeedData 对齐） */
const COMMON_ENTITY_FIELD_T_KEYS = {
  remark: 'common.page.entity.remark',
  extFieldJson: 'common.page.entity.extfieldjson',
  tenantCode: 'common.page.entity.tenantcode',
  companyCode: 'common.page.entity.companycode',
  companyDefaultCulture: 'common.page.entity.companydefaultculture',
};

/**
 * 解析字段完整 i18n 键（remark / extFieldJson 等走 common.page.entity.*，其余走 entity.{slug}.*）
 * @param {string} name
 * @param {string} entitySlug
 * @returns {string}
 */
function resolveFieldTranslationKey(name, entitySlug) {
  if (COMMON_ENTITY_FIELD_T_KEYS[name]) {
    return COMMON_ENTITY_FIELD_T_KEYS[name];
  }
  return `entity.${entitySlug}.${fieldI18nKey(name, entitySlug)}`;
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
 * 推断表单控件类型
 * @param {{ name: string, type: string, doc: string }} field
 * @returns {'select'|'textarea'|'date'|'switch'|'input'}
 */
function inferHtmlType(field) {
  const dict = extractDictType(field.doc);
  if (dict) {
    return 'select';
  }
  if (field.type === 'boolean') {
    return 'switch';
  }
  const lower = field.name.toLowerCase();
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
 * 检测 Create DTO 中的 OneToMany 子表（与后端 [Navigate] / List&lt;子CreateDto&gt; 对齐）
 * @param {ReturnType<typeof parseTypeInterfaces>} interfaces
 * @param {string} entityPascal
 * @returns {Array<{ fieldName: string, childPascal: string, childCreateType: string, childType: string, childCamel: string, childKebab: string, childIdField: string }>}
 */
function detectMasterDetailChildren(interfaces, entityPascal) {
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
    if (!interfaces.has(elementType)) {
      return;
    }
    if (RBAC_ASSOCIATION_ENTITY_SHORT_NAMES.has(childPascal)) {
      return;
    }
    const entityProp = entityIface?.properties.find((p) => p.name === prop.name);
    const entityElement = entityProp ? parseArrayElementType(entityProp.type) : null;
    const isChildEntity = Boolean(
      entityElement && interfaces.has(entityElement) && entityElement !== entityPascal,
    );
    const docHint = /子表|一对多|级联|外键|关联/.test(`${prop.doc || ''}${entityProp?.doc || ''}`);
    if (!isChildEntity && !docHint) {
      return;
    }
    const childSlug = pascalToCamel(childPascal);
    children.push({
      fieldName: prop.name,
      childPascal,
      childCreateType: elementType,
      childType: entityElement || childPascal,
      childCamel: childSlug,
      childKebab: pascalToKebab(childPascal),
      childIdField: `${childSlug}Id`,
    });
  });
  return children;
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
 * 构建实体字段元数据
 * @param {ReturnType<typeof parseTypeInterfaces>} interfaces
 * @param {string} entityPascal
 */
function buildFieldMeta(interfaces, entityPascal) {
  const entity = interfaces.get(entityPascal);
  const create = interfaces.get(`${entityPascal}Create`);
  const query = interfaces.get(`${entityPascal}Query`);
  const masterDetailChildren = detectMasterDetailChildren(interfaces, entityPascal);
  const childFieldNames = new Set(masterDetailChildren.map((c) => c.fieldName));
  const entitySlug = pascalToCamel(entityPascal);
  const enrich = (fields, slug) => fields.map((f) => ({
    ...f,
    htmlType: inferHtmlType(f),
    dictType: extractDictType(f.doc),
    i18nKey: resolveFieldTranslationKey(f.name, slug),
  }));
  const listFields = (entity?.properties || []).filter((p) => !SKIP_LIST_FIELDS.has(p.name) && !childFieldNames.has(p.name));
  const scopeFields = buildScopeFormFields(create?.properties || [], entityPascal);
  const scopeNames = new Set(scopeFields.map((f) => f.name));
  const formFieldsRaw = (create?.properties || []).filter((p) => {
    if (scopeNames.has(p.name) || SKIP_FORM_FIELDS.has(p.name)) {
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
  const queryFields = (query?.properties || []).filter((p) => {
    if (SKIP_QUERY_FIELDS.has(p.name)) {
      return false;
    }
    if (/Start$/.test(p.name) || /End$/.test(p.name)) {
      return false;
    }
    if (childFieldNames.has(p.name)) {
      return false;
    }
    return true;
  }).slice(0, 8);
  const enrichedChildren = masterDetailChildren.map((child) => {
    const childFormRaw = buildChildFormFieldProps(interfaces, child.childPascal, entitySlug);
    const childEntity = interfaces.get(child.childType);
    const childListRaw = (childEntity?.properties || []).filter((p) => {
      if (SKIP_LIST_FIELDS.has(p.name)) {
        return false;
      }
      if (p.name === child.childIdField) {
        return false;
      }
      return true;
    }).slice(0, 8);
    const childSlug = pascalToCamel(child.childPascal);
    return {
      ...child,
      formFields: enrich(childFormRaw, childSlug),
      listFields: enrich(childListRaw, childSlug),
    };
  });
  return {
    listFields: enrich(listFields, entitySlug),
    formFields: enrich([...scopeFields, ...formFieldsRaw], entitySlug),
    queryFields: enrich(queryFields, entitySlug),
    masterDetailChildren: enrichedChildren,
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
  return {
    modulePath,
    viewModulePath,
    entityKebab,
    entityPascal,
    entityCamel: pascalToCamel(entityPascal),
    entitySlug: pascalToCamel(entityPascal),
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
 * 生成 index.vue 主子表展开区模板
 * @param {object[]} children
 * @returns {string}
 */
function generateExpandedRowTemplate(children) {
  if (!children.length) {
    return '';
  }
  const tables = children.map((child) => `          <div class="mb-2 text-sm font-medium">{{ t('entity.${child.childCamel}._self') }}</div>
          <a-table
            :columns="${child.childCamel}ExpandColumns"
            :data-source="get${child.childPascal}List(record)"
            :row-key="(row: ${child.childType}, index: number) => row?.${child.childIdField} || String(index)"
            :pagination="false"
            size="small"
            bordered
            class="mb-4"
          />`).join('\n');
  return `      <template #expandedRowRender="{ record }">
        <div class="p-4">
${tables}
        </div>
      </template>`;
}

/**
 * 生成 index.vue 主子表 script 片段
 * @param {object} ctx
 * @returns {{ state: string, columns: string, helpers: string, handlers: string }}
 */
function generateMasterDetailIndexScript(ctx) {
  const { entityPascal, entityCamel, caps, fields } = ctx;
  const children = fields.masterDetailChildren || [];
  if (!children.length || !caps.hasGetById) {
    return { state: '', columns: '', helpers: '', handlers: '', expandProps: '', expandTemplate: '' };
  }
  const state = 'const expandedRowKeys = ref<string[]>([])\n';
  const columns = children.map((child) => {
    const cols = child.listFields.map((f) => `  {
    title: ${fieldLabelTExpr(f)},
    dataIndex: '${f.name}',
    key: '${f.name}',
    ellipsis: true,
  },`).join('\n');
    return `const ${child.childCamel}ExpandColumns = computed(() => [
${cols}
])`;
  }).join('\n\n');
  const helpers = children.map((child) => `function get${child.childPascal}List(record: ${entityPascal}): ${child.childType}[] {
  return (record as any)?.${child.fieldName} ?? []
}`).join('\n\n');
  const loadFns = children.map((child) => `    if (!detail?.${child.fieldName}?.length) {
      detail.${child.fieldName} = []
    }`).join('\n');
  const handlers = `
async function load${entityPascal}Detail(record: ${entityPascal}): Promise<${entityPascal} | null> {
  const id = get${entityPascal}Id(record)
  if (!id) {
    return null
  }
  try {
    const detail = await ${caps.apiGetById}(id)
${loadFns}
    const index = dataSource.value.findIndex((row) => get${entityPascal}Id(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as ${entityPascal}
    }
    return detail
  } catch (error: any) {
    logger.error('[${entityPascal}] 加载详情失败', { error })
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return null
  }
}

function has${entityPascal}ChildList(record: ${entityPascal}): boolean {
  return ${children.map((c) => `get${c.childPascal}List(record).length > 0`).join(' || ')}
}

async function handleExpand(expanded: boolean, record: ${entityPascal}) {
  const key = get${entityPascal}Id(record)
  if (!expanded || !key) {
    expandedRowKeys.value = []
    return
  }
  if (!has${entityPascal}ChildList(record)) {
    await load${entityPascal}Detail(record)
  }
  expandedRowKeys.value = [key]
}`;
  return {
    state,
    columns,
    helpers,
    handlers,
    expandProps: `
      :expanded-row-keys="expandedRowKeys"
      @expand="handleExpand"`,
    expandTemplate: generateExpandedRowTemplate(children),
  };
}

/**
 * 生成 *-form.vue 主子表 Tab 与 script
 * @param {object} ctx
 * @returns {{ tabs: string, script: string, needsTaktSelect: boolean }}
 */
function generateMasterDetailFormParts(ctx) {
  const { entityPascal, entityCamel, fields } = ctx;
  const children = fields.masterDetailChildren || [];
  if (!children.length) {
    return { tabs: '', script: '', needsTaktSelect: false };
  }
  const tabs = children.map((child) => {
    const cols = child.formFields.map((f) => `  {
    title: ${fieldLabelTExpr(f)},
    dataIndex: '${f.name}',
    key: '${f.name}',
    width: 140,
  },`).join('\n');
    const bodyCells = child.formFields.map((f) => `            <template v-else-if="column.key === '${f.name}'">
${renderFormControl(f, 'record.', '              ')}
            </template>`).join('\n');
    return `      <a-tab-pane
        key="child-${child.fieldName}"
        :tab="t('entity.${child.childCamel}._self')"
        force-render
      >
        <div class="mb-2">
          <a-button type="primary" size="small" @click="handleAdd${child.childPascal}Row">
            {{ t('common.page.button.create') }}{{ t('entity.${child.childCamel}._self') }}
          </a-button>
        </div>
        <a-table
          :columns="${child.childCamel}FormColumns"
          :data-source="child${child.childPascal}Rows"
          :pagination="false"
          :row-key="(row: Record<string, unknown>, index: number) => String(row.__rowKey ?? index)"
          size="small"
          bordered
        >
          <template #bodyCell="{ column, record, index }">
${bodyCells}
            <template v-else-if="column.key === '__action'">
              <a-button type="link" danger size="small" @click="handleRemove${child.childPascal}Row(index)">
                {{ t('common.page.button.delete') }}
              </a-button>
            </template>
          </template>
        </a-table>
      </a-tab-pane>`;
  }).join('\n');
  const columnDefs = children.map((child) => {
    const cols = child.formFields.map((f) => `  {
    title: ${fieldLabelTExpr(f)},
    dataIndex: '${f.name}',
    key: '${f.name}',
    width: 140,
  },`).join('\n');
    return `const ${child.childCamel}FormColumns = computed(() => [
${cols}
  {
    title: t('common.page.entity.action'),
    key: '__action',
    width: 80,
    fixed: 'right',
  },
])`;
  }).join('\n\n');
  const rowRefs = children.map((child) => `const child${child.childPascal}Rows = ref<Record<string, unknown>[]>([])`).join('\n');
  const syncFromForm = children.map((child) => `  child${child.childPascal}Rows.value = ((val as any)?.${child.fieldName} ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.${child.childIdField} ?? \`new-\${index}\`,
  }))`).join('\n');
  const clearRows = children.map((child) => `  child${child.childPascal}Rows.value = []`).join('\n');
  const addHandlers = children.map((child) => {
    const defaults = child.formFields.map((f) => {
      if (f.readOnly && f.name === 'tenantCode') {
        return '      tenantCode: tenantStore.tenantCode,';
      }
      if (f.readOnly && f.name === 'companyCode') {
        return '      companyCode: tenantStore.companyCode,';
      }
      if (f.readOnly && f.name === 'companyDefaultCulture') {
        return "      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',";
      }
      const val = f.type === 'number' ? '0' : "''";
      return `      ${f.name}: ${val},`;
    }).join('\n');
    return `function handleAdd${child.childPascal}Row() {
  child${child.childPascal}Rows.value.push({
    __rowKey: \`new-\${Date.now()}\`,
${defaults}
  })
}

function handleRemove${child.childPascal}Row(index: number) {
  child${child.childPascal}Rows.value.splice(index, 1)
}`;
  }).join('\n\n');
  const getValuesMerge = children.map((child) => `    ${child.fieldName}: child${child.childPascal}Rows.value.map(({ __rowKey, ...rest }) => rest),`).join('\n');
  const script = `
${rowRefs}

${columnDefs}

function syncChildRowsFromFormData(val: Partial<${entityPascal}Create & { ${pascalToCamel(entityPascal)}Id?: string }> | null | undefined) {
${syncFromForm}
}

${addHandlers}

function buildSubmitPayload() {
  return {
    ...formState,
${getValuesMerge}
  }
}`;
  const needsTaktSelect = children.some((c) => c.formFields.some((f) => f.htmlType === 'select' && f.dictType));
  return { tabs, script, needsTaktSelect };
}

/**
 * 生成 index.vue
 * @param {object} ctx
 */
function generateIndexVue(ctx) {
  const {
    entityPascal,
    entityCamel,
    entityKebab,
    modulePath,
    viewModulePath,
    permissionPrefix,
    cssRootClass,
    caps,
    fields,
    comment,
  } = ctx;
  const importApiNames = [
    caps.apiGetList,
    caps.apiGetById,
    caps.apiCreate,
    caps.apiUpdate,
    caps.apiDelete,
    caps.apiDeleteBatch,
    caps.apiGetTemplate,
    caps.apiImport,
    caps.apiExport,
  ].filter(Boolean);
  const mdParts = generateMasterDetailIndexScript(ctx);
  const hasMasterDetail = Boolean(mdParts.state);
  const typeImports = [`${entityPascal}`, `${entityPascal}Query`, `${entityPascal}Create`, `${entityPascal}Update`]
    .filter((name, idx, arr) => arr.indexOf(name) === idx);
  const listCols = fields.listFields.filter((f) => f.name !== caps.entityIdName).slice(0, 12);
  const queryItems = fields.queryFields.map((f) => {
    if (f.htmlType === 'select' && f.dictType) {
      return `      <a-form-item :label="${fieldLabelTExpr(f)}">
        <TaktSelect
          v-model:value="advancedQueryForm.${f.name}"
          dict-type="${f.dictType}"
          :placeholder="${fieldPlaceholderTExpr(f, 'common.page.form.placeholder.select')}"
          allow-clear
        />
      </a-form-item>`;
    }
    return `      <a-form-item :label="${fieldLabelTExpr(f)}">
        <a-input
          v-model:value="advancedQueryForm.${f.name}"
          :placeholder="${fieldPlaceholderTExpr(f, 'common.page.form.placeholder.required')}"
          allow-clear
        />
      </a-form-item>`;
  }).join('\n');
  const queryInit = fields.queryFields.map((f) => {
    const val = f.type === 'number' ? 'undefined as number | undefined' : "''";
    return `  ${f.name}: ${val},`;
  }).join('\n');
  const columnBlocks = listCols.map((f) => `  {
    title: ${fieldLabelTExpr(f)},
    dataIndex: '${f.name}',
    key: '${f.name}',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => get${entityPascal}Field(record, '${f.name}') ?? ''
  },`).join('\n');
  const actionItems = [];
  if (caps.hasUpdate) {
    actionItems.push(`      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: '${permissionPrefix}:update',
        onClick: (record: ${entityPascal}) => handleEdit(record)
      },`);
  }
  if (caps.hasDelete) {
    actionItems.push(`      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: '${permissionPrefix}:delete',
        onClick: (record: ${entityPascal}) => handleDeleteOne(record)
      }`);
  }
  const formBlock = (caps.hasCreate || caps.hasUpdate) ? `
    <!-- 新增/编辑对话框 -->
    <TaktModal
      v-model:open="formVisible"
      :title="formTitle"
      width="50%"
      wrap-class-name="takt-form-modal-resizable"
      :confirm-loading="formLoading"
      @ok="handleFormSubmit"
      @cancel="handleFormCancel"
    >
      <${entityPascal}Form
        ref="formRef"
        :form-data="formData"
        :loading="formLoading"
      />
    </TaktModal>` : '';
  const importBlock = (caps.hasImport && caps.hasGetTemplate) ? `
    <!-- 导入对话框 -->
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.page.button.import') + t('entity.${entityCamel}._self')"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.${entityCamel}._self"
        file-type="xlsx"
        :sheet-name="excelNames.sheet"
        :template-file-name="excelNames.fileBase"
        :download-template="handleDownloadTemplate"
        :import-file="handleImportFile"
        :max-size="10"
        :max-rows="1000"
        @success="handleImportSuccess"
      />
    </TaktModal>` : '';
  const formImports = (caps.hasCreate || caps.hasUpdate)
    ? `import ${entityPascal}Form from './components/${entityKebab}-form.vue'\n`
    : '';
  const iconImports = (caps.hasUpdate || caps.hasDelete)
    ? "import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'\n"
    : '';
  const excelImport = (caps.hasImport || caps.hasExport)
    ? "import { taktExcelEntityNames } from '@/utils/naming'\n"
    : '';
  const exportImport = caps.hasExport
    ? "import { resolveExportDownloadFileName } from '@/utils/export-download-name'\n"
    : '';
  const excelConst = (caps.hasImport || caps.hasExport)
    ? `const excelNames = taktExcelEntityNames('${caps.entityClassName}')\n`
    : '';
  const formStateBlock = (caps.hasCreate || caps.hasUpdate) ? `
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<${entityPascal}>>({})
const formLoading = ref(false)
const formRef = ref()` : '';
  const importState = (caps.hasImport && caps.hasGetTemplate) ? 'const importVisible = ref(false)\n' : '';
  const updateDisabled = caps.hasUpdate ? 'const updateDisabled = computed(() => selectedRows.value.length !== 1)\n' : '';
  const deleteDisabled = caps.hasDelete ? 'const deleteDisabled = computed(() => selectedRows.value.length === 0)\n' : '';
  const createHandler = caps.hasCreate ? `
function handleCreate() {
  formTitle.value = t('common.page.button.create') + t('entity.${entityCamel}._self')
  formData.value = {}
  formVisible.value = true
}` : '';
  const updateHandler = caps.hasUpdate ? (hasMasterDetail ? `
async function handleEdit(record: ${entityPascal}) {
  formTitle.value = t('common.page.button.edit') + t('entity.${entityCamel}._self')
  formLoading.value = true
  try {
    const detail = await load${entityPascal}Detail(record)
    formData.value = detail ? { ...detail } : { ...record }
    formVisible.value = true
  } finally {
    formLoading.value = false
  }
}

function handleUpdate() {
  if (selectedRow.value) {
    void handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.${entityCamel}._self') }))
  }
}` : `
function handleEdit(record: ${entityPascal}) {
  formTitle.value = t('common.page.button.edit') + t('entity.${entityCamel}._self')
  formData.value = { ...record }
  formVisible.value = true
}

function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.${entityCamel}._self') }))
  }
}`) : '';
  const formSubmitHandler = (caps.hasCreate || caps.hasUpdate) ? `
async function handleFormSubmit() {
  const refInst = formRef.value
  if (!refInst?.validate) return
  try {
    await refInst.validate()
  } catch {
    return
  }
  formLoading.value = true
  try {
    const payload = refInst.getValues?.() ?? { ...(formData.value as any) }
    const id = (formData.value as any)?.[entityIdName]
    if (id) {
${caps.hasUpdate ? `      await ${caps.apiUpdate}(id, payload as any)\n      message.success(t('common.feedback.updated', { target: t('entity.${entityCamel}._self') }))` : ''}
    } else {
${caps.hasCreate ? `      await ${caps.apiCreate}(payload as any)\n      message.success(t('common.feedback.created', { target: t('entity.${entityCamel}._self') }))` : ''}
    }
    formVisible.value = false
    loadData()
  } finally {
    formLoading.value = false
  }
}

function handleFormCancel() {
  formVisible.value = false
}` : '';
  const importHandlers = (caps.hasImport && caps.hasGetTemplate) ? `
function handleImport() {
  importVisible.value = true
}

async function handleDownloadTemplate(sheetName?: string, fileName?: string): Promise<Blob> {
  const res = await ${caps.apiGetTemplate}(sheetName, fileName)
  return (res as any)?.data ?? res
}

async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await ${caps.apiImport}(file, sheetName)
}

function handleImportSuccess(result: { success: number; fail: number; errors: string[] }) {
  loadData()
  if (result.fail === 0) setTimeout(() => { importVisible.value = false }, 2000)
}

function handleImportCancel() {
  importVisible.value = false
}` : '';
  const exportHandler = caps.hasExport ? `
async function handleExport() {
  try {
    loading.value = true
    const kw = (queryKeyword.value ?? '').trim()
    const exportQuery: ${entityPascal}Query = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await ${caps.apiExport}(exportQuery, excelNames.sheet, excelNames.fileBase)
    const ts = new Date()
    const pad = (n: number, w = 2) => String(n).padStart(w, '0')
    const fallbackBase = \`\${excelNames.fileBase}_\${ts.getFullYear()}\${pad(ts.getMonth() + 1)}\${pad(ts.getDate())}\${pad(ts.getHours())}\${pad(ts.getMinutes())}\${pad(ts.getSeconds())}\`
    const fileName = resolveExportDownloadFileName({
      contentDisposition: (exportMeta as any).contentDisposition ?? null,
      contentType: (exportMeta as any).contentType ?? null,
      fallbackBase
    })
    const blob = (exportMeta as any).blob ?? exportMeta
    const url = window.URL.createObjectURL(blob)
    const link = document.createElement('a')
    link.href = url
    link.download = fileName
    link.style.display = 'none'
    document.body.appendChild(link)
    link.click()
    document.body.removeChild(link)
    setTimeout(() => window.URL.revokeObjectURL(url), 100)
    message.success(t('common.feedback.export.success', { target: t('entity.${entityCamel}._self') }))
  } catch (error: any) {
    logger.error('[${entityPascal}] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.${entityCamel}._self') }))
  } finally {
    loading.value = false
  }
}` : '';
  const deleteOneHandler = caps.hasDelete ? `
async function handleDeleteOne(record: ${entityPascal}) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.${entityCamel}._self'), name: t('common.tip.this.target', { target: t('entity.${entityCamel}._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await ${caps.apiDelete}((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.${entityCamel}._self') }))
      loadData()
    }
  })
}` : '';
  const deleteBatchHandler = caps.hasDeleteBatch ? `
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.${entityCamel}._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.${entityCamel}._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await ${caps.apiDeleteBatch}(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.${entityCamel}._self') }))
      loadData()
    }
  })
}` : '';
  const loadDataBody = caps.hasGetList ? `    const kw = (queryKeyword.value ?? '').trim()
    const params: ${entityPascal}Query = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await ${caps.apiGetList}(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0` : `    dataSource.value = []
    total.value = 0`;
  return `<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/${viewModulePath} -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：${comment}管理页面，含查询、增删改，由 generate-vue-from-api 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="${cssRootClass}">
    <!-- 查询栏 -->
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleReset"
    />

    <!-- 工具栏 -->
    <TaktToolsBar
${caps.hasCreate ? `      create-permission="${permissionPrefix}:create"` : ''}
${caps.hasUpdate ? `      update-permission="${permissionPrefix}:update"` : ''}
${caps.hasDelete ? `      delete-permission="${permissionPrefix}:delete"` : ''}
${caps.hasImport ? `      import-permission="${permissionPrefix}:import"` : ''}
${caps.hasExport ? `      export-permission="${permissionPrefix}:export"` : ''}
      :show-create="${caps.hasCreate}"
      :show-update="${caps.hasUpdate}"
      :show-delete="${caps.hasDelete || caps.hasDeleteBatch}"
      :show-import="${caps.hasImport && caps.hasGetTemplate}"
      :show-export="${caps.hasExport}"
      :show-expand="${hasMasterDetail}"
      :show-advanced-query="true"
      :show-column-setting="true"
      :show-fullscreen="true"
      :show-refresh="true"
${caps.hasCreate ? '      :create-disabled="false"\n      :create-loading="loading"' : ''}
${caps.hasUpdate ? '      :update-disabled="updateDisabled"\n      :update-loading="loading"' : ''}
${caps.hasDelete || caps.hasDeleteBatch ? '      :delete-disabled="deleteDisabled"\n      :delete-loading="loading"' : ''}
      :refresh-loading="loading"
${caps.hasCreate ? '      @create="handleCreate"' : ''}
${caps.hasUpdate ? '      @update="handleUpdate"' : ''}
${caps.hasDelete || caps.hasDeleteBatch ? '      @delete="handleDelete"' : ''}
${caps.hasImport && caps.hasGetTemplate ? '      @import="handleImport"' : ''}
${caps.hasExport ? '      @export="handleExport"' : ''}
      @advanced-query="handleAdvancedQuery"
      @column-setting="handleColumnSetting"
      @refresh="handleRefresh"
    />

    <!-- 表格 -->
    <TaktSingleTable
      :columns="displayColumns"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="get${entityPascal}Id"
      :row-selection="rowSelection"
      :custom-row="onClickRow"
      :large-screen-column-count="9"
      :small-screen-column-count="5"
${mdParts.expandProps}
      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
${mdParts.expandTemplate}
    </TaktSingleTable>

    <!-- 分页组件 -->
    <TaktPagination
      v-model:current="currentPage"
      v-model:page-size="pageSize"
      :total="total"
      @change="handlePaginationChange"
      @show-size-change="handlePaginationSizeChange"
    />
${formBlock}
    <!-- 高级查询抽屉 -->
    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
${queryItems}
    </TaktQueryDrawer>
${importBlock}
    <!-- 列设置抽屉 -->
    <TaktColumnDrawer
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      :id-column-key="'${caps.entityIdName}'"
      :action-column-key="'action'"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * ${comment}管理页 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/${viewModulePath}
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { mergeDefaultColumns } from '@/utils/table-columns'
import { useI18n } from 'vue-i18n'
${formImports}import { ${importApiNames.join(', ')} } from '@/api/${modulePath}/${entityKebab}'
import type { ${typeImports.join(', ')} } from '@/types/${modulePath}/${entityKebab}'
${excelImport}${exportImport}${iconImports}
const { t } = useI18n()
${excelConst}const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.${entityCamel}._self') })
)

const queryKeyword = ref('')
const loading = ref(false)
const dataSource = ref<${entityPascal}[]>([])
const currentPage = ref(1)
const pageSize = ref(20)
const total = ref(0)
const selectedRow = ref<${entityPascal} | null>(null)
const selectedRows = ref<${entityPascal}[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
${formStateBlock}
const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
${queryInit}
})
const columnSettingVisible = ref(false)
${importState}const visibleColumnKeys = ref<string[]>([])
const entityIdName = '${caps.entityIdName}'
${updateDisabled}${deleteDisabled}${mdParts.state}
onMounted(() => {
  loadData()
})

${mdParts.columns}

${mdParts.helpers}
${mdParts.handlers}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: '${caps.entityIdName}',
    key: '${caps.entityIdName}',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => get${entityPascal}Field(record, '${caps.entityIdName}') ?? ''
  },
${columnBlocks}
  CreateActionColumn({
    actions: [
${actionItems.join('\n')}
    ]
  })
])

const get${entityPascal}Id = (record: any): string => record?.[entityIdName] ?? ''
const get${entityPascal}Field = (record: any, field: string): any => record?.[field]

const mergedColumns = computed((): any => mergeDefaultColumns(columns.value as any, t, true))
const displayColumns = computed(() => {
  const keys = visibleColumnKeys.value || []
  const merged = mergedColumns.value || []
  if (keys.length === 0) return merged
  const keysSet = new Set(keys.map((k: any) => String(k)))
  return merged.filter((col: any) => {
    const colKey = col.key || col.dataIndex || col.title
    return colKey && keysSet.has(String(colKey))
  })
})

const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: ${entityPascal}[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: ${entityPascal}, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (get${entityPascal}Id(selectedRow.value) === get${entityPascal}Id(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: ${entityPascal}[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

const onClickRow = (record: ${entityPascal}) => ({
  onClick: () => {
    const key = get${entityPascal}Id(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(get${entityPascal}Id(item)))
    selectedRow.value = selectedRowKeys.value.length === 1 ? (selectedRows.value[0] ?? null) : null
    if (rowSelection.value.onChange) {
      rowSelection.value.onChange(selectedRowKeys.value, selectedRows.value)
    }
  }
})

async function loadData() {
  loading.value = true
  try {
${loadDataBody}
  } catch (error: any) {
    logger.error('[${entityPascal}] 加载数据失败', { error })
    message.error(error?.message || t('common.feedback.load.data.failed'))
    dataSource.value = []
    total.value = 0
  } finally {
    loading.value = false
  }
}

function handleSearch() {
  currentPage.value = 1
  loadData()
}

function handleReset() {
  queryKeyword.value = ''
  advancedQueryForm.value = {
${queryInit}
  }
  currentPage.value = 1
  loadData()
}
${createHandler}${updateHandler}${formSubmitHandler}${importHandlers}${exportHandler}${deleteOneHandler}${deleteBatchHandler}
function handleAdvancedQuery() {
  advancedQueryVisible.value = true
}

function handleAdvancedQuerySubmit() {
  advancedQueryVisible.value = false
  currentPage.value = 1
  loadData()
}

function handleAdvancedQueryReset() {
  advancedQueryForm.value = {
${queryInit}
  }
}

function handleColumnSetting() {
  columnSettingVisible.value = true
}

function handleColumnKeysChange(keys: string[]) {
  visibleColumnKeys.value = keys
}

function handleColumnSettingReset() {
  visibleColumnKeys.value = columns.value.map((c: any) => c.key || c.dataIndex).filter(Boolean)
}

function handleRefresh() {
  loadData()
}

function handleTableChange() {}
function handleResizeColumn() {}
function handlePaginationChange(page: number) {
  currentPage.value = page
  loadData()
}
function handlePaginationSizeChange(_current: number, size: number) {
  pageSize.value = size
  currentPage.value = 1
  loadData()
}
</script>

<style scoped lang="css">
.${cssRootClass} {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
`;
}

/**
 * 生成 *-form.vue
 * @param {object} ctx
 */
function generateFormVue(ctx) {
  const { entityPascal, entityCamel, entityKebab, modulePath, viewModulePath, fields, comment } = ctx;
  const formFields = fields.formFields;
  const mdFormParts = generateMasterDetailFormParts(ctx);
  const hasMasterDetail = Boolean(mdFormParts.tabs);
  const tabCount = computeFormTabCount(formFields.length);
  const formContentClassExpr = buildFormContentClassComputedExpr();
  const tabs = [];
  for (let tabIndex = 1; tabIndex <= tabCount; tabIndex += 1) {
    const start = (tabIndex - 1) * FORM_TAB_FIELDS_PER_TAB;
    const end = tabIndex * FORM_TAB_FIELDS_PER_TAB;
    const tabFields = formFields.slice(start, end);
    const items = tabFields.map((f) => {
      const colSpan = f.htmlType === 'textarea' ? 24 : 12;
      const control = renderFormControl(f, 'formState.', '                ');
      return `            <a-col :span="${colSpan}">
              <a-form-item
                :label="${fieldLabelTExpr(f)}"
                name="${f.name}"
              >
${control}
              </a-form-item>
            </a-col>`;
    }).join('\n');
    const tabLabel = buildFormTabLabelAttr(tabIndex, tabCount);
    tabs.push(`      <a-tab-pane
        key="tab-${tabIndex - 1}"
        ${tabLabel}
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
${items}
          </a-row>
        </div>
      </a-tab-pane>`);
  }
  const needsTaktSelect = formFields.some((f) => f.htmlType === 'select' && f.dictType) || mdFormParts.needsTaktSelect;
  const masterDetailChildren = fields.masterDetailChildren || [];
  const hasScopeContextFields = hasScopeContextFormFields(formFields, masterDetailChildren);
  const entityIdField = `${pascalToCamel(entityPascal)}Id`;
  const scopeStoreImports = hasScopeContextFields
    ? "import { useTenantStore } from '@/stores/identity/tenant'\nimport { useUserStore } from '@/stores/identity/user'\n"
    : '';
  const scopeStoreScript = hasScopeContextFields ? `
const tenantStore = useTenantStore()
const userStore = useUserStore()

/**
 * 上下文隔离字段：租户 / 公司 / 公司默认语言（登录或公司切换注入，表单只读）
 * @param target 表单数据
 * @param force 为 true 时强制覆盖（新增态或公司切换）
 */
function applyScopeDefaults(target: Record<string, unknown>, force = false) {
  if (formFields.includes('tenantCode') && (force || !target.tenantCode)) {
    target.tenantCode = tenantStore.tenantCode
  }
  if (formFields.includes('companyCode') && (force || !target.companyCode)) {
    target.companyCode = tenantStore.companyCode
  }
  if (formFields.includes('companyDefaultCulture') && (force || !target.companyDefaultCulture)) {
    target.companyDefaultCulture = userStore.userInfo?.companyDefaultCulture ?? ''
  }
}
` : '';
  const scopeContextWatch = hasScopeContextFields ? `
watch(
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture] as const,
  () => {
    const isCreate = !props.formData?.${entityIdField}
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)
` : '';
  const childTypeImports = hasMasterDetail
    ? [...new Set((fields.masterDetailChildren || []).flatMap((c) => [c.childCreateType, c.childType]))]
    : [];
  const typeImportLine = [`${entityPascal}Create`, ...childTypeImports]
    .filter((name, idx, arr) => arr.indexOf(name) === idx)
    .join(', ');
  const childFieldStrip = hasMasterDetail
    ? (fields.masterDetailChildren || []).map((c) => `    delete (next as any).${c.fieldName}`).join('\n')
    : '';
  const watchSyncChild = hasMasterDetail ? '    syncChildRowsFromFormData(val)\n' : '';
  const resetChildRows = hasMasterDetail
    ? (fields.masterDetailChildren || []).map((c) => `  child${c.childPascal}Rows.value = []`).join('\n')
    : '';
  const getValuesBody = hasMasterDetail ? '  return buildSubmitPayload()' : '  return { ...formState }';
  const taktSelectImport = needsTaktSelect
    ? "import TaktSelect from '@/components/business/takt-select/index.vue'\n"
    : '';
  const requiredRules = formFields
    .filter((f) => !f.optional && f.name !== 'remark' && !f.readOnly)
    .map((f) => {
      const trigger = f.htmlType === 'select' || f.htmlType === 'date' || f.htmlType === 'switch' ? 'change' : 'blur';
      const placeholderKey = f.htmlType === 'select' || f.htmlType === 'date'
        ? 'common.page.form.placeholder.select'
        : 'common.page.form.placeholder.required';
      return `  ${f.name}: [
    {
      required: true,
      message: ${fieldPlaceholderTExpr(f, placeholderKey)},
      trigger: '${trigger}'
    }
  ],`;
    }).join('\n');
  return `<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/${viewModulePath}/components -->
<!-- 文件名称：${entityKebab}-form.vue -->
<!-- 功能描述：${comment}维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="${entityKebab}-form-tabs"
    >
${tabs.join('\n')}
${mdFormParts.tabs}
    </a-tabs>
  </a-form>
</template>

<script setup lang="ts">
/**
 * ${comment}维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/${viewModulePath}/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { ${typeImportLine} } from '@/types/${modulePath}/${entityKebab}'
${taktSelectImport}${scopeStoreImports}
const { t } = useI18n()
${scopeStoreScript}const formContentClass = ${formContentClassExpr}
const activeTab = ref('tab-0')
const formFields = ${JSON.stringify(formFields.map((f) => f.name))}
${mdFormParts.script}

interface Props {
  formData?: Partial<${entityPascal}Create & { ${pascalToCamel(entityPascal)}Id?: string }> | null
  loading?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: () => ({}),
  loading: false
})

const formRef = ref()
const formState = reactive<Record<string, any>>({})

watch(
  () => props.formData,
  (val) => {
    const next = val ? { ...val } : {}
    Object.keys(formState).forEach((k) => delete formState[k])
${childFieldStrip}
${hasScopeContextFields ? '    applyScopeDefaults(next)\n' : ''}    Object.assign(formState, next)
${watchSyncChild}  },
  { immediate: true, deep: true }
)
${scopeContextWatch}
const rules = computed<Record<string, Rule[]>>(() => ({
${requiredRules}
}))

async function validate() {
  await formRef.value?.validate()
  return formState
}

function getValues(): Record<string, any> {
${getValuesBody}
}

function resetFields() {
  formRef.value?.resetFields()
  Object.keys(formState).forEach((k) => delete formState[k])
${resetChildRows}
  activeTab.value = 'tab-0'
}

defineExpose({ validate, getValues, resetFields })
</script>

<style scoped lang="css">
:deep(.ant-tabs-content-holder) {
  min-height: 50vh;
}

:deep(.ant-tabs-tabpane) {
  min-height: 50vh;
}
</style>
`;
}

/**
 * 扫描 API 目录
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

/**
 * 处理单个 API 模块
 * @param {string} apiFilePath
 * @param {{ dryRun: boolean, viewPath?: string|null }} options
 */
function processApiModule(apiFilePath, options) {
  const rel = path.relative(path.join(CONFIG.frontendRoot, CONFIG.apiDir), apiFilePath).replace(/\\/g, '/');
  const entityKebab = path.basename(rel, '.ts');
  const entityShort = kebabToPascal(entityKebab);
  const dtoSourceBase = `Takt${entityShort}Dtos`;
  if (shouldExcludeDtoSourceBase(dtoSourceBase) || shouldExcludeVueGeneration(rel, entityShort)) {
    console.log(`⏭️  跳过手工/排除模块: ${rel}`);
    return { skipped: true };
  }
  const typesPath = path.join(CONFIG.frontendRoot, CONFIG.typesDir, `${rel.replace(/\.ts$/, '.d.ts')}`);
  if (!fs.existsSync(typesPath)) {
    console.warn(`⚠️  缺少类型文件，跳过: ${typesPath}`);
    return { skipped: true };
  }
  const apiContent = fs.readFileSync(apiFilePath, 'utf-8');
  const typesContent = fs.readFileSync(typesPath, 'utf-8');
  const { methods } = parseApiFile(apiContent);
  const interfaces = parseTypeInterfaces(typesContent);
  if (!interfaces.has(entityShort)) {
    console.warn(`⚠️  类型文件中未找到主实体 interface ${entityShort}，跳过: ${rel}`);
    return { skipped: true };
  }
  const caps = detectApiCapabilities(entityShort, methods);
  if (!caps.hasGetList && !caps.hasCreate && !caps.hasUpdate) {
    console.warn(`⚠️  非标准 CRUD API，跳过: ${rel}`);
    return { skipped: true };
  }
  const ctx = resolveModuleContext(apiFilePath, entityShort, options);
  const fields = buildFieldMeta(interfaces, entityShort);
  const comment = parseEntityComment(typesContent, entityShort);
  const fullCtx = { ...ctx, caps, fields, comment };
  const viewDir = path.join(CONFIG.frontendRoot, CONFIG.viewsDir, ctx.viewModulePath);
  const indexPath = path.join(viewDir, 'index.vue');
  const formPath = path.join(viewDir, 'components', `${ctx.entityKebab}-form.vue`);
  const needsForm = caps.hasCreate || caps.hasUpdate;
  const indexContent = generateIndexVue(fullCtx);
  const formContent = needsForm ? generateFormVue(fullCtx) : '';
  if (options.dryRun) {
    console.log(`🔍 [dry-run] 将生成:\n  - ${indexPath}${formContent ? `\n  - ${formPath}` : ''}`);
    return { skipped: false, dryRun: true };
  }
  writeGeneratedFile(indexPath, indexContent);
  console.log(`✅ 已生成: ${indexPath}`);
  if (formContent) {
    writeGeneratedFile(formPath, formContent);
    console.log(`✅ 已生成: ${formPath}`);
  }
  return { skipped: false, created: true };
}

function printUsage() {
  console.log(`
用法: node scripts/generate-vue-from-api.cjs [参数]

参数:
  --all                 扫描 frontend/src/api 下全部模块
  --<实体名>            单实体，如 --Plant、--Holiday（不带 Takt 前缀）
  --view-path <路径>    覆盖 views 输出目录（相对 src/views），如 human-resource/attendance-leave/holiday
  --force               已废弃（与默认行为相同，仅为兼容 generate-all.cjs 传参保留）
  --dry-run             仅预览，不写盘

说明:
  - 输入：frontend/src/types/**/{entity}.d.ts + frontend/src/api/**/{entity}.ts
  - 输出：frontend/src/views/{viewPath}/{entity}/index.vue 与 components/{entity}-form.vue
  - 写入策略：目标文件不存在则创建，已存在则整文件覆盖更新（无需 --force）
  - 权限前缀：优先菜单种子 ComponentPath/Permission，其次后端控制器 TaktPermission
  - 列表列：主实体字段（排除审计/租户公司字段）
  - 表单列：Create DTO 字段；tenantCode/companyCode/companyDefaultCulture 置于首位
    · Tenant / Company 实体本身：tenantCode/companyCode 可编辑
    · 其它实体：上下文隔离字段 readonly，无 [Required]；新增时从 useTenantStore / useUserStore 自动注入
    · 公司切换后新增表单随 userInfo.companyDefaultCulture 更新（依赖 /me 刷新）
  - 表单 Tabs：每 Tab 最多 ${FORM_TAB_FIELDS_PER_TAB} 个字段（2 列布局）；超出自动分页，标签为「基本信息 (当前/总数)」
  - 排除（不生成）：User/Menu/Dept/DictType/DictData/GenTable/GenTableColumn/Culture/Translation/Numbering、workflow/** 全部（见 generate-entity-exclusions.cjs）

示例:
  node scripts/generate-vue-from-api.cjs --Plant
  node scripts/generate-vue-from-api.cjs --Holiday --view-path human-resource/attendance-leave/holiday
  node scripts/generate-vue-from-api.cjs --all
`);
}

/**
 * @returns {{ all: boolean, entityPrefix: string|null, force: boolean, dryRun: boolean, viewPath: string|null }}
 */
function parseArgs() {
  const args = process.argv.slice(2);
  const options = { all: false, entityPrefix: null, force: false, dryRun: false, viewPath: null };
  for (let i = 0; i < args.length; i += 1) {
    const arg = args[i];
    if (arg === '--force') {
      options.force = true;
      continue;
    }
    if (arg === '--dry-run') {
      options.dryRun = true;
      continue;
    }
    if (arg === '--view-path') {
      options.viewPath = args[i + 1] || null;
      i += 1;
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
      console.error('❌ 实体名不要带 Takt 前缀，例如 --Plant');
      process.exit(1);
    }
    if (options.entityPrefix) {
      console.error('❌ 只能指定 --all 或一个实体名');
      process.exit(1);
    }
    options.entityPrefix = value;
  }
  if (!options.all && !options.entityPrefix) {
    printUsage();
    process.exit(1);
  }
  return options;
}

console.log('🚀 开始从 types/api 生成 Vue 视图...\n');
logGeneratedFileWritePolicy();
buildMenuIndex();

try {
  const options = parseArgs();
  const apiFiles = collectApiFiles(options.entityPrefix);
  if (apiFiles.length === 0) {
    console.error('❌ 未找到匹配的 API 文件');
    process.exit(1);
  }
  let created = 0;
  let skipped = 0;
  apiFiles.forEach((file) => {
    const result = processApiModule(file, options);
    if (result.skipped) {
      skipped += 1;
    } else {
      created += 1;
    }
  });
  console.log(`\n✨ 完成：生成 ${created} 个模块，跳过 ${skipped} 个`);
} catch (error) {
  console.error('❌ 生成失败:', error);
  process.exit(1);
}
