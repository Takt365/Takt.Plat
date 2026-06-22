// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：scripts
// 文件名称：audit-permission-scan.cjs
// 创建时间：2026-06-20
// 创建人：Takt365(Cursor AI)
// 功能描述：菜单/控制器/视图权限扫描共用函数，供 audit-permissions.cjs 使用
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const fs = require('fs');
const path = require('path');
const {
  getControllerClassName,
  entityShortFromControllerClassName,
} = require('./generate-script-common.cjs');
const {
  isRbacJunctionEntity,
  shouldExcludeStandaloneService,
} = require('./generate-entity-exclusions.cjs');
const { buildPermissionBase } = require('./permission-base.cjs');

const BACKEND_ROOT = path.resolve(__dirname, '../backend/src');
const SERVICES_ROOT = path.join(BACKEND_ROOT, 'Takt.Application', 'Services');
const CONTROLLERS_ROOT = path.join(BACKEND_ROOT, 'Takt.WebApi', 'Controllers');
const SEEDS_DIR = path.join(BACKEND_ROOT, 'Takt.Infrastructure', 'Data', 'Seeds', 'EntitySeedData');
const FRONTEND_VIEWS_ROOT = path.resolve(__dirname, '../frontend/src/views');

const MENU_LEVEL_FILES = [
  'TaktMenuLevel1SeedData.cs',
  'TaktMenuLevel2SeedData.cs',
  'TaktMenuLevel3SeedData.cs',
  'TaktMenuLevel4SeedData.cs',
  'TaktMenuLevel5SeedData.cs',
];

const PERMISSION_ATTR_REGEX = /\[TaktPermission\("([^"]+)"/g;

const SHELL_VIEW_PREFIXES = [
  'about/',
  'home',
  'dashboard/',
  'foundation/dict',
  'foundation/i18n',
  'workflow/',
  'code/generator',
  'routine/help-desk/my-',
];

const MANUAL_CONTROLLER_PREFIXES = [
  'TaktFlowEngineController',
  'TaktAuthController',
  'TaktRbacController',
  'TaktOpenIddictController',
];

/**
 * @param {string} filePath
 * @returns {boolean}
 */
function isInEngineDirectory(filePath) {
  const normalizedPath = filePath.replace(/\\/g, '/');
  return /\/\w*[Ee]ngine($|\/)/i.test(normalizedPath);
}

/**
 * @param {string} interfaceFile
 * @returns {string|null}
 */
function entityNameFromInterfaceFile(interfaceFile) {
  const base = path.basename(interfaceFile, '.cs');
  const match = base.match(/^I(Takt\w+)Service$/);
  return match ? match[1] : null;
}

/**
 * 扫描全部 ITaktXxxService
 * @returns {Array<{ interfaceFile: string, entityName: string, entityShort: string, pathParts: string[] }>}
 */
function scanAllServices() {
  /** @type {ReturnType<typeof scanAllServices>} */
  const results = [];

  function walk(dir) {
    if (!fs.existsSync(dir)) {
      return;
    }
    for (const entry of fs.readdirSync(dir, { withFileTypes: true })) {
      const fullPath = path.join(dir, entry.name);
      if (entry.isDirectory()) {
        if (isInEngineDirectory(fullPath)) {
          continue;
        }
        walk(fullPath);
        continue;
      }
      if (!entry.name.startsWith('ITakt') || !entry.name.endsWith('Service.cs')) {
        continue;
      }
      if (isInEngineDirectory(fullPath)) {
        continue;
      }
      const entityName = entityNameFromInterfaceFile(fullPath);
      if (!entityName) {
        continue;
      }
      if (shouldExcludeStandaloneService(entityName)) {
        continue;
      }
      const entityShort = entityName.replace(/^Takt/, '');
      if (isRbacJunctionEntity(entityShort)) {
        continue;
      }
      const relDir = path.relative(SERVICES_ROOT, path.dirname(fullPath));
      const pathParts = relDir.split(path.sep).filter(Boolean);
      results.push({ interfaceFile: fullPath, entityName, entityShort, pathParts });
    }
  }

  walk(SERVICES_ROOT);
  return results.sort((a, b) => a.entityShort.localeCompare(b.entityShort));
}

/**
 * @param {string} entityShort
 * @returns {string|null}
 */
function findControllerFile(entityShort) {
  const controllerClass = getControllerClassName(entityShort);
  function walk(dir) {
    if (!fs.existsSync(dir)) {
      return null;
    }
    for (const entry of fs.readdirSync(dir, { withFileTypes: true })) {
      const fullPath = path.join(dir, entry.name);
      if (entry.isDirectory()) {
        const found = walk(fullPath);
        if (found) {
          return found;
        }
        continue;
      }
      if (entry.name === `${controllerClass}.cs`) {
        return fullPath;
      }
    }
    return null;
  }
  return walk(CONTROLLERS_ROOT);
}

/**
 * 扫描控制器 → list 前缀与全部权限码
 * @returns {{
 *   listBaseByEntity: Map<string, string>,
 *   entityByListBase: Map<string, string>,
 *   allControllerPermissions: Set<string>,
 *   controllerFileByEntity: Map<string, string>,
 * }}
 */
function buildControllerPermissionIndex() {
  /** @type {Map<string, string>} */
  const listBaseByEntity = new Map();
  /** @type {Map<string, string>} */
  const entityByListBase = new Map();
  /** @type {Set<string>} */
  const allControllerPermissions = new Set();
  /** @type {Map<string, string>} */
  const controllerFileByEntity = new Map();

  function walk(dir) {
    if (!fs.existsSync(dir)) {
      return;
    }
    for (const entry of fs.readdirSync(dir, { withFileTypes: true })) {
      const fullPath = path.join(dir, entry.name);
      if (entry.isDirectory()) {
        walk(fullPath);
        continue;
      }
      if (!entry.name.endsWith('Controller.cs')) {
        continue;
      }
      if (MANUAL_CONTROLLER_PREFIXES.some((name) => entry.name === name)) {
        continue;
      }
      const content = fs.readFileSync(fullPath, 'utf-8');
      PERMISSION_ATTR_REGEX.lastIndex = 0;
      let match;
      /** @type {string[]} */
      const perms = [];
      while ((match = PERMISSION_ATTR_REGEX.exec(content)) !== null) {
        const perm = match[1].toLowerCase();
        perms.push(perm);
        allControllerPermissions.add(perm);
      }
      const listMatch = content.match(/\[TaktPermission\("([^"]+):list"/);
      if (!listMatch) {
        continue;
      }
      const listBase = listMatch[1].toLowerCase();
      const entityShort = entityShortFromControllerClassName(entry.name.replace(/\.cs$/, ''));
      if (!entityShort) {
        continue;
      }
      listBaseByEntity.set(entityShort, listBase);
      entityByListBase.set(listBase, entityShort);
      controllerFileByEntity.set(entityShort, fullPath);
    }
  }

  walk(CONTROLLERS_ROOT);
  return { listBaseByEntity, entityByListBase, allControllerPermissions, controllerFileByEntity };
}

/**
 * 从菜单种子解析 CreateOrUpdateMenuAsync 块（含 lookupKey、行号）
 * @returns {Array<{ lookupKey: string, menuCode: string, menuName: string, i18nKey: string, permission: string, menuType: number, routePath: string, componentPath: string, viewModulePath: string, sourceFile: string, line: number }>}
 */
function parseAllMenuSeedBlocks() {
  /** @type {ReturnType<typeof parseAllMenuSeedBlocks>} */
  const blocks = [];
  for (const fileName of MENU_LEVEL_FILES) {
    const fullPath = path.join(SEEDS_DIR, fileName);
    if (!fs.existsSync(fullPath)) {
      continue;
    }
    const content = fs.readFileSync(fullPath, 'utf-8');
    const parts = content.split(/CreateOrUpdateMenuAsync\(/);
    let searchFrom = 0;
    for (let i = 1; i < parts.length; i += 1) {
      const block = parts[i];
      const blockStart = content.indexOf('CreateOrUpdateMenuAsync(', searchFrom);
      searchFrom = blockStart >= 0 ? blockStart + 1 : searchFrom;
      const line = blockStart >= 0 ? content.slice(0, blockStart).split('\n').length : 0;
      const lookupMatch = block.match(/,\s*"([^"]+)"\s*,\s*menu\s*=>/);
      const menuCodeMatch = block.match(/menu\.MenuCode\s*=\s*"([^"]+)"/);
      const menuNameMatch = block.match(/menu\.MenuName\s*=\s*"([^"]+)"/);
      const i18nMatch = block.match(/menu\.I18nKey\s*=\s*"([^"]+)"/);
      const permMatch = block.match(/menu\.Permission\s*=\s*"([^"]+)"/);
      const menuTypeMatch = block.match(/menu\.MenuType\s*=\s*(\d+)/);
      const routeMatch = block.match(/menu\.RoutePath\s*=\s*"([^"]+)"/);
      const componentMatch = block.match(/menu\.ComponentPath\s*=\s*"([^"]+)"/);
      const componentPath = componentMatch ? componentMatch[1] : '';
      if (!lookupMatch) {
        continue;
      }
      blocks.push({
        lookupKey: lookupMatch[1],
        menuCode: menuCodeMatch ? menuCodeMatch[1] : '',
        menuName: menuNameMatch ? menuNameMatch[1] : '',
        i18nKey: i18nMatch ? i18nMatch[1].toLowerCase() : '',
        permission: permMatch ? permMatch[1].toLowerCase() : '',
        menuType: menuTypeMatch ? Number(menuTypeMatch[1]) : 0,
        routePath: routeMatch ? routeMatch[1] : '',
        componentPath,
        viewModulePath: componentPath.endsWith('/index') ? componentPath.replace(/\/index$/, '') : '',
        sourceFile: fileName,
        line,
      });
    }
  }
  return blocks;
}

/**
 * 从菜单种子解析全部带 Permission 的菜单项
 * @returns {Array<{ menuCode: string, i18nKey: string, menuType: number, permission: string, routePath: string, componentPath: string, viewModulePath: string, sourceFile: string }>}
 */
function parseAllMenuEntries() {
  /** @type {ReturnType<typeof parseAllMenuEntries>} */
  const entries = [];
  for (const fileName of MENU_LEVEL_FILES) {
    const fullPath = path.join(SEEDS_DIR, fileName);
    if (!fs.existsSync(fullPath)) {
      continue;
    }
    const content = fs.readFileSync(fullPath, 'utf-8');
    const blocks = content.split(/CreateOrUpdateMenuAsync\(/);
    for (let i = 1; i < blocks.length; i += 1) {
      const block = blocks[i];
      const codeMatch = block.match(/,\s*"([^"]+)"\s*,\s*menu\s*=>/);
      const permMatch = block.match(/menu\.Permission\s*=\s*"([^"]+)"/);
      if (!permMatch) {
        continue;
      }
      const i18nMatch = block.match(/menu\.I18nKey\s*=\s*"([^"]+)"/);
      const menuTypeMatch = block.match(/menu\.MenuType\s*=\s*(\d+)/);
      const routeMatch = block.match(/menu\.RoutePath\s*=\s*"([^"]+)"/);
      const componentMatch = block.match(/menu\.ComponentPath\s*=\s*"([^"]+)"/);
      const componentPath = componentMatch ? componentMatch[1] : '';
      entries.push({
        menuCode: codeMatch ? codeMatch[1] : '',
        i18nKey: i18nMatch ? i18nMatch[1].toLowerCase() : '',
        menuType: menuTypeMatch ? Number(menuTypeMatch[1]) : 0,
        permission: permMatch[1].toLowerCase(),
        routePath: routeMatch ? routeMatch[1] : '',
        componentPath,
        viewModulePath: componentPath.endsWith('/index') ? componentPath.replace(/\/index$/, '') : '',
        sourceFile: fileName,
      });
    }
  }
  return entries;
}

/**
 * @param {string} viewModulePath
 * @returns {boolean}
 */
function isShellViewPage(viewModulePath) {
  if (!viewModulePath) {
    return true;
  }
  return SHELL_VIEW_PREFIXES.some((prefix) => viewModulePath === prefix.replace(/\/$/, '')
    || viewModulePath.startsWith(prefix));
}

/**
 * 从 Vue 文件提取权限码
 * @param {string} vueFile
 * @returns {string[]}
 */
function extractVuePermissions(vueFile) {
  const content = fs.readFileSync(vueFile, 'utf-8');
  /** @type {Set<string>} */
  const perms = new Set();
  const patterns = [
    /(?:create|update|delete|import|export|query|list)-permission="([^"]+)"/g,
    /v-permission="'([^']+)'"/g,
    /v-permission="([^"]+)"/g,
    /permission:\s*'([^']+)'/g,
  ];
  for (const regex of patterns) {
    regex.lastIndex = 0;
    let match;
    while ((match = regex.exec(content)) !== null) {
      const perm = (match[1] || '').trim().toLowerCase();
      if (perm && perm.includes(':') && !perm.includes('.')) {
        perms.add(perm);
      }
    }
  }
  return [...perms];
}

/**
 * 扫描 views 下 index.vue 与子组件面板权限
 * @returns {Array<{ viewModulePath: string, file: string, permissions: string[], toolbarBase: string|null }>}
 */
function scanVueViewPermissions() {
  /** @type {ReturnType<typeof scanVueViewPermissions>} */
  const rows = [];

  function walk(dir, relParts) {
    if (!fs.existsSync(dir)) {
      return;
    }
    for (const entry of fs.readdirSync(dir, { withFileTypes: true })) {
      const fullPath = path.join(dir, entry.name);
      if (entry.isDirectory()) {
        walk(fullPath, [...relParts, entry.name]);
        continue;
      }
      if (!entry.name.endsWith('.vue')) {
        continue;
      }
      if (entry.name !== 'index.vue' && !entry.name.endsWith('-panel.vue')) {
        continue;
      }
      const viewModulePath = relParts.join('/');
      if (isShellViewPage(viewModulePath)) {
        continue;
      }
      const permissions = extractVuePermissions(fullPath);
      if (permissions.length === 0) {
        continue;
      }
      const createPerm = permissions.find((p) => p.endsWith(':create'));
      const toolbarBase = createPerm ? createPerm.replace(/:create$/, '') : null;
      rows.push({
        viewModulePath,
        file: path.relative(path.resolve(__dirname, '..'), fullPath).replace(/\\/g, '/'),
        permissions,
        toolbarBase,
      });
    }
  }

  walk(FRONTEND_VIEWS_ROOT, []);
  return rows.sort((a, b) => a.viewModulePath.localeCompare(b.viewModulePath));
}

/**
 * 权限码 → 前缀（去掉末段操作）
 * @param {string} permissionCode
 * @returns {string}
 */
function permissionToBase(permissionCode) {
  const parts = String(permissionCode || '').split(':').filter(Boolean);
  if (parts.length < 2) {
    return parts.join(':');
  }
  return parts.slice(0, -1).join(':');
}

/**
 * 由菜单页面 permission 解析关联实体
 * @param {string} menuListPermission
 * @param {Map<string, string>} entityByListBase
 * @returns {string|null}
 */
function resolveEntityFromMenuListPermission(menuListPermission, entityByListBase) {
  const base = menuListPermission.replace(/:list$/, '');
  if (entityByListBase.has(base)) {
    return entityByListBase.get(base);
  }
  return null;
}

/**
 * 由视图路径 + 控制器索引推断实体
 * @param {string} viewModulePath
 * @param {Map<string, string>} listBaseByEntity
 * @returns {string|null}
 */
function resolveEntityFromViewPath(viewModulePath, listBaseByEntity) {
  const leaf = viewModulePath.split('/').pop() || '';
  const leafNorm = leaf.replace(/-/g, '').toLowerCase();
  /** @type {string[]} */
  const hits = [];
  for (const entityShort of listBaseByEntity.keys()) {
    const kebab = entityShort.replace(/([a-z0-9])([A-Z])/g, '$1-$2').toLowerCase();
    const entityNorm = entityShort.toLowerCase();
    if (kebab === leaf || entityNorm === leafNorm || kebab.replace(/-/g, '') === leafNorm) {
      hits.push(entityShort);
    }
  }
  if (hits.length === 1) {
    return hits[0];
  }
  return null;
}

module.exports = {
  BACKEND_ROOT,
  SERVICES_ROOT,
  CONTROLLERS_ROOT,
  SEEDS_DIR,
  FRONTEND_VIEWS_ROOT,
  MENU_LEVEL_FILES,
  scanAllServices,
  findControllerFile,
  buildControllerPermissionIndex,
  parseAllMenuEntries,
  parseAllMenuSeedBlocks,
  scanVueViewPermissions,
  permissionToBase,
  resolveEntityFromMenuListPermission,
  resolveEntityFromViewPath,
  isShellViewPage,
};
