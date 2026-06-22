// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：scripts
// 文件名称：audit-vue-views.cjs
// 功能描述：对照菜单 ComponentPath，检查哪些页面菜单尚未有 views/**/index.vue，并输出 TXT 报告
// ========================================

const fs = require('fs');
const path = require('path');
const { kebabToPascal, pascalToKebab } = require('./generate-vue-common.cjs');
const { resolveApiFilePathForEntity } = require('./generate-master-detail-associations.cjs');

const FRONTEND_ROOT = path.resolve(__dirname, '../frontend');
const VIEWS_ROOT = path.join(FRONTEND_ROOT, 'src/views');
const API_ROOT = path.join(FRONTEND_ROOT, 'src/api');
const TYPES_ROOT = path.join(FRONTEND_ROOT, 'src/types');
const SEEDS_DIR = path.resolve(__dirname, '../backend/src/Takt.Infrastructure/Data/Seeds/EntitySeedData');
const CONTROLLERS_ROOT = path.resolve(__dirname, '../backend/src/Takt.WebApi/Controllers');
const REPORTS_DIR = path.resolve(__dirname, 'reports');
const DEFAULT_REPORT_FILE = path.join(REPORTS_DIR, 'audit-vue-views.txt');
/** 建议命令前缀（在 scripts 目录下执行） */
const CMD = {
  audit: 'node audit-vue-views.cjs',
  generateAll: 'node generate-all.cjs',
  generateVueAll: 'node generate-vue-all-from-api.cjs',
  generateFromBackend: 'node generate-from-backend.cjs',
};
const { entityShortFromControllerName } = require('./generate-entity-exclusions.cjs');
const { entityShortFromControllerClassName } = require('./generate-script-common.cjs');

/**
 * 解析命令行 --out=path
 * @returns {string}
 */
function resolveReportOutputPath() {
  const outArg = process.argv.find((arg) => arg.startsWith('--out='));
  if (outArg) {
    const custom = outArg.slice('--out='.length).trim();
    return path.isAbsolute(custom) ? custom : path.resolve(process.cwd(), custom);
  }
  return DEFAULT_REPORT_FILE;
}

/**
 * @param {boolean} ok
 * @returns {string}
 */
function mark(ok) {
  return ok ? 'Y' : 'N';
}

/** 菜单种子来源（Level1~Level5） */
const MENU_LEVEL_FILES = [
  'TaktMenuLevel1SeedData.cs',
  'TaktMenuLevel2SeedData.cs',
  'TaktMenuLevel3SeedData.cs',
  'TaktMenuLevel4SeedData.cs',
  'TaktMenuLevel5SeedData.cs',
];

/** 工作台/流程/壳页面：有菜单与手工 views，通常无标准 CRUD api/types */
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

/**
 * 扫描控制器 list 权限 → 实体短名
 * @returns {Map<string, string>}
 */
function buildListPermissionEntityIndex() {
  /** @type {Map<string, string>} */
  const map = new Map();
  function scan(dir) {
    for (const entry of fs.readdirSync(dir, { withFileTypes: true })) {
      const full = path.join(dir, entry.name);
      if (entry.isDirectory()) {
        scan(full);
        continue;
      }
      if (!entry.name.endsWith('Controller.cs')) {
        continue;
      }
      const content = fs.readFileSync(full, 'utf-8');
      const match = content.match(/\[TaktPermission\("([^"]+):list"/);
      if (!match) {
        continue;
      }
      const entityShort = entityShortFromControllerName(entry.name.replace(/\.cs$/, ''))
        || entityShortFromControllerClassName(entry.name.replace(/\.cs$/, ''));
      if (entityShort) {
        map.set(match[1], entityShort);
      }
    }
  }
  if (fs.existsSync(CONTROLLERS_ROOT)) {
    scan(CONTROLLERS_ROOT);
  }
  return map;
}

/**
 * @param {string} viewModulePath
 * @returns {boolean}
 */
function isShellViewPage(viewModulePath) {
  return SHELL_VIEW_PREFIXES.some((prefix) => viewModulePath === prefix.replace(/\/$/, '')
    || viewModulePath.startsWith(prefix));
}

/**
 * 从菜单种子解析页面菜单（MenuType=1 且 ComponentPath 以 /index 结尾）
 * @returns {Array<{ menuCode: string, menuType: number, permission: string, routePath: string, componentPath: string, viewModulePath: string, sourceFile: string }>}
 */
function parseMenuPageEntries() {
  /** @type {ReturnType<typeof parseMenuPageEntries>} */
  const pages = [];
  const files = MENU_LEVEL_FILES.map((name) => {
    const fullPath = path.join(SEEDS_DIR, name);
    if (!fs.existsSync(fullPath)) {
      return null;
    }
    return name;
  }).filter(Boolean);
  const componentRe = /menu\.ComponentPath\s*=\s*"([^"]+)"/g;
  const menuCodeRe = /CreateOrUpdateMenuAsync\([\s\S]*?,\s*"([^"]+)"/g;

  for (const file of files) {
    const fullPath = path.join(SEEDS_DIR, file);
    const content = fs.readFileSync(fullPath, 'utf-8');
    let match = componentRe.exec(content);
    while (match) {
      const componentPath = match[1];
      const blockStart = Math.max(0, match.index - 1600);
      const blockEnd = Math.min(content.length, match.index + 400);
      const block = content.slice(blockStart, blockEnd);
      const menuTypeMatch = [...block.matchAll(/menu\.MenuType\s*=\s*(\d+)/g)].pop();
      const menuType = menuTypeMatch ? Number(menuTypeMatch[1]) : 0;
      if (menuType !== 1 || !componentPath.endsWith('/index')) {
        match = componentRe.exec(content);
        continue;
      }
      const permission = ([...block.matchAll(/menu\.Permission\s*=\s*"([^"]+)"/g)].pop() || [])[1] || '';
      const routePath = ([...block.matchAll(/menu\.RoutePath\s*=\s*"([^"]+)"/g)].pop() || [])[1] || '';
      const blockBeforeCode = content.slice(Math.max(0, match.index - 2200), match.index);
      const menuCodeMatch = [...blockBeforeCode.matchAll(menuCodeRe)].pop();
      const menuCode = menuCodeMatch ? menuCodeMatch[1] : '';
      pages.push({
        menuCode,
        menuType,
        permission,
        routePath,
        componentPath,
        viewModulePath: componentPath.replace(/\/index$/, ''),
        sourceFile: file,
      });
      match = componentRe.exec(content);
    }
  }
  const unique = new Map();
  for (const page of pages) {
    if (!unique.has(page.viewModulePath)) {
      unique.set(page.viewModulePath, page);
    }
  }
  return [...unique.values()].sort((a, b) => a.viewModulePath.localeCompare(b.viewModulePath));
}

/**
 * @param {string} viewModulePath
 * @returns {boolean}
 */
function hasIndexVue(viewModulePath) {
  return fs.existsSync(path.join(VIEWS_ROOT, viewModulePath, 'index.vue'));
}

/** 菜单 views 路径与后端实体短名不一致时的显式映射 */
const VIEW_ENTITY_OVERRIDES = {
  'logistics/procurement/invoice': 'PurchaseInvoice',
  'logistics/sales/invoice': 'SalesInvoice',
};

/**
 * api 相对路径 → 同结构 types .d.ts
 * @param {string} apiFilePath
 * @returns {string}
 */
function apiFileToTypesFile(apiFilePath) {
  const rel = path.relative(API_ROOT, apiFilePath).replace(/\\/g, '/').replace(/\.ts$/, '.d.ts');
  return path.join(TYPES_ROOT, rel);
}

/**
 * 在指定目录下递归查找 {basename}.ts
 * @param {string} rootDir
 * @param {string} basename
 * @returns {string[]}
 */
function findApiFilesUnder(rootDir, basename) {
  /** @type {string[]} */
  const hits = [];
  function scan(currentDir) {
    if (!fs.existsSync(currentDir)) {
      return;
    }
    for (const entry of fs.readdirSync(currentDir, { withFileTypes: true })) {
      const full = path.join(currentDir, entry.name);
      if (entry.isDirectory()) {
        scan(full);
        continue;
      }
      if (entry.name === `${basename}.ts`) {
        hits.push(full);
      }
    }
  }
  scan(rootDir);
  return hits;
}

/**
 * 菜单 permission 与控制器 permission 末段不一致时模糊匹配实体（如 invoice↔purchaseinvoice）
 * @param {string} menuPermission
 * @param {Map<string, string>} permissionEntityIndex
 * @returns {string[]}
 */
function resolveEntityCandidatesFromPermissionFuzzy(menuPermission, permissionEntityIndex) {
  const perm = menuPermission.replace(/:list$/, '');
  if (perm && permissionEntityIndex.has(perm)) {
    return [permissionEntityIndex.get(perm)];
  }
  const menuParts = perm.split(':');
  const menuEntitySeg = menuParts[menuParts.length - 1] || '';
  const menuPrefix = menuParts.slice(0, -1).join(':');
  const normMenu = menuEntitySeg.replace(/-/g, '').toLowerCase();
  /** @type {string[]} */
  const matches = [];
  for (const [ctrlPerm, entity] of permissionEntityIndex.entries()) {
    const ctrlParts = ctrlPerm.split(':');
    if (ctrlParts.slice(0, -1).join(':') !== menuPrefix) {
      continue;
    }
    const ctrlEntitySeg = ctrlParts[ctrlParts.length - 1] || '';
    const normCtrl = ctrlEntitySeg.replace(/-/g, '').toLowerCase();
    if (ctrlEntitySeg.includes(menuEntitySeg)
      || menuEntitySeg.includes(ctrlEntitySeg)
      || normCtrl.includes(normMenu)
      || normMenu.includes(normCtrl)) {
      matches.push(entity);
    }
  }
  return [...new Set(matches)];
}

/**
 * 多个候选实体时按菜单码 / views 末段 / 是否已有 api 文件择优
 * @param {string[]} candidates
 * @param {{ menuCode?: string, viewModulePath: string }} page
 * @returns {string|null}
 */
function pickBestEntityCandidate(candidates, page) {
  if (!candidates.length) {
    return null;
  }
  if (candidates.length === 1) {
    return candidates[0];
  }
  const menuCode = (page.menuCode || '').toLowerCase();
  const viewLeaf = page.viewModulePath.split('/').pop() || '';
  let best = candidates[0];
  let bestScore = -1;
  for (const entity of candidates) {
    let score = 0;
    const entityKebab = pascalToKebab(entity);
    const entityLower = entity.toLowerCase();
    if (entityKebab === viewLeaf) {
      score += 15;
    }
    if (menuCode.includes(entityLower)) {
      score += 10;
    }
    if (menuCode.includes(entityKebab.replace(/-/g, ''))) {
      score += 8;
    }
    if (resolveApiFilePathForEntity(entity)) {
      score += 20;
    }
    if (score > bestScore) {
      bestScore = score;
      best = entity;
    }
  }
  return best;
}

/**
 * 检测页面是否已有 api/types（与 generate-from-backend 输出路径及模块内递归查找对齐）
 * @param {string} viewModulePath
 * @param {string} [entityArg]
 * @returns {{ api: boolean, types: boolean }}
 */
function hasApiTypes(viewModulePath, entityArg) {
  const segments = viewModulePath.split('/');
  const menuLeaf = segments[segments.length - 1] || '';
  const modulePath = segments.slice(0, -1).join('/');
  /** @type {Set<string>} */
  const apiHits = new Set();
  /**
   * @param {string|null|undefined} apiPath
   */
  const registerApi = (apiPath) => {
    if (!apiPath || !fs.existsSync(apiPath)) {
      return;
    }
    apiHits.add(apiPath);
  };
  registerApi(path.join(API_ROOT, viewModulePath, `${menuLeaf}.ts`));
  registerApi(path.join(API_ROOT, modulePath, `${menuLeaf}.ts`));
  registerApi(path.join(API_ROOT, `${menuLeaf}.ts`));
  if (entityArg) {
    registerApi(resolveApiFilePathForEntity(entityArg));
    const entityKebab = pascalToKebab(entityArg);
    registerApi(path.join(API_ROOT, modulePath, `${entityKebab}.ts`));
    registerApi(path.join(API_ROOT, viewModulePath, `${entityKebab}.ts`));
    findApiFilesUnder(path.join(API_ROOT, modulePath), entityKebab).forEach(registerApi);
  }
  findApiFilesUnder(path.join(API_ROOT, modulePath), menuLeaf).forEach(registerApi);
  const moduleApiRoot = modulePath ? path.join(API_ROOT, modulePath) : API_ROOT;
  findApiFilesUnder(moduleApiRoot, menuLeaf).forEach(registerApi);
  if (apiHits.size === 0 && menuLeaf.includes('-')) {
    findApiFilesUnder(API_ROOT, menuLeaf).forEach(registerApi);
  }
  let typesOk = false;
  for (const apiPath of apiHits) {
    if (fs.existsSync(apiFileToTypesFile(apiPath))) {
      typesOk = true;
      break;
    }
  }
  return {
    api: apiHits.size > 0,
    types: typesOk,
  };
}

/**
 * 从 viewModulePath / permission 推断 generate-from-backend / generate-all 实体参数
 * @param {{ viewModulePath: string, permission: string, menuCode?: string }} page
 * @param {Map<string, string>} permissionEntityIndex
 * @returns {string}
 */
function suggestEntityArg(page, permissionEntityIndex) {
  if (VIEW_ENTITY_OVERRIDES[page.viewModulePath]) {
    return VIEW_ENTITY_OVERRIDES[page.viewModulePath];
  }
  const perm = page.permission.replace(/:list$/, '');
  if (perm && permissionEntityIndex.has(perm)) {
    return permissionEntityIndex.get(perm);
  }
  const segments = page.viewModulePath.split('/');
  let entityKebab = segments[segments.length - 1] || '';
  if (entityKebab.endsWith('-change-log')) {
    entityKebab = entityKebab.replace(/-change-log$/, '');
  }
  const entityFromView = kebabToPascal(entityKebab);
  const fuzzyCandidates = resolveEntityCandidatesFromPermissionFuzzy(page.permission, permissionEntityIndex);
  const fuzzyPick = pickBestEntityCandidate(fuzzyCandidates, page);
  if (fuzzyPick && resolveApiFilePathForEntity(fuzzyPick)) {
    return fuzzyPick;
  }
  if (resolveApiFilePathForEntity(entityFromView)) {
    return entityFromView;
  }
  if (fuzzyPick) {
    return fuzzyPick;
  }
  return entityFromView;
}

/**
 * 在同父级 views 目录下查找与菜单末段/实体 kebab 最接近的子目录
 * @param {string} menuViewPath
 * @param {string} entityArg
 * @returns {string|null}
 */
function findSiblingViewMatch(menuViewPath, entityArg) {
  const segments = menuViewPath.split('/');
  const menuLeaf = segments[segments.length - 1] || '';
  const parentPath = segments.slice(0, -1).join('/');
  const siblings = parentPath ? listChildViewModules(parentPath) : [];
  if (!siblings.length) {
    return null;
  }
  const entityKebab = pascalToKebab(entityArg);
  const exact = siblings.find((viewPath) => viewPath.split('/').pop() === entityKebab);
  if (exact) {
    return exact;
  }
  const fuzzy = siblings.find((viewPath) => {
    const leaf = viewPath.split('/').pop() || '';
    return leaf.includes(menuLeaf)
      || menuLeaf.includes(leaf)
      || leaf.replace(/^standard-/, '') === menuLeaf
      || leaf.replace(/^sop-/, '') === menuLeaf;
  });
  return fuzzy || null;
}

/**
 * 优先取与菜单 viewModulePath 末段一致的 api 文件，避免控制器权限前缀误导（如 StorageLocation→warehouse）
 * @param {{ viewModulePath: string }} page
 * @param {string} entityArg
 * @returns {string|null}
 */
function resolveApiFilePathForMenu(page, entityArg) {
  const segments = page.viewModulePath.split('/');
  const menuLeaf = segments[segments.length - 1] || '';
  const modulePath = segments.slice(0, -1).join('/');
  if (menuLeaf) {
    const menuAlignedApi = path.join(API_ROOT, modulePath, `${menuLeaf}.ts`);
    if (fs.existsSync(menuAlignedApi)) {
      return menuAlignedApi;
    }
  }
  return resolveApiFilePathForEntity(entityArg);
}

/**
 * RoutePath → 约定 ComponentPath（与 menu-routes 一致：views/{path}/index.vue）
 * @param {string} routePath
 * @returns {string}
 */
function expectedComponentPathFromRoute(routePath) {
  const normalized = String(routePath || '').trim().replace(/^\/+/, '').replace(/\/+$/, '');
  return normalized ? `${normalized}/index` : '';
}

/**
 * @param {string} apiFilePath
 * @returns {string}
 */
function viewModulePathFromApiFile(apiFilePath) {
  return path.relative(API_ROOT, apiFilePath).replace(/\\/g, '/').replace(/\.ts$/, '');
}

/**
 * 列出菜单路径下已有 index.vue 的直接子目录
 * @param {string} parentViewModulePath
 * @returns {string[]}
 */
function listChildViewModules(parentViewModulePath) {
  const dir = path.join(VIEWS_ROOT, parentViewModulePath);
  if (!fs.existsSync(dir)) {
    return [];
  }
  return fs.readdirSync(dir, { withFileTypes: true })
    .filter((entry) => entry.isDirectory()
      && fs.existsSync(path.join(dir, entry.name, 'index.vue')))
    .map((entry) => `${parentViewModulePath}/${entry.name}`)
    .sort();
}

/**
 * @typedef {{
 *   type: 'ROUTE_COMPONENT' | 'VIEW_DIR_MISMATCH' | 'VIEW_HUB_SUBPAGES',
 *   page: ReturnType<typeof parseMenuPageEntries>[0],
 *   entityArg: string,
 *   menuViewPath: string,
 *   expectedComponentPath?: string,
 *   actualViewPath?: string,
 *   childViewPaths?: string[],
 *   menuFix?: { componentPath: string, routePath: string },
 * }} PathAlignmentIssue
 */

/**
 * 对照菜单 ComponentPath / RoutePath 与 views 目录
 * @param {ReturnType<typeof parseMenuPageEntries>[0]} page
 * @param {string} entityArg
 * @returns {PathAlignmentIssue[]}
 */
function auditMenuViewPathAlignment(page, entityArg) {
  /** @type {PathAlignmentIssue[]} */
  const issues = [];
  const menuViewPath = page.viewModulePath;
  const expectedComponentPath = expectedComponentPathFromRoute(page.routePath);
  const base = {
    page,
    entityArg,
    menuViewPath,
  };

  if (expectedComponentPath && page.componentPath !== expectedComponentPath) {
    issues.push({
      ...base,
      type: 'ROUTE_COMPONENT',
      expectedComponentPath,
    });
  }

  if (hasIndexVue(menuViewPath)) {
    return issues;
  }

  const apiFile = resolveApiFilePathForMenu(page, entityArg);
  if (apiFile) {
    const actualViewPath = viewModulePathFromApiFile(apiFile);
    if (actualViewPath === menuViewPath) {
      return issues;
    }
    if (hasIndexVue(actualViewPath)) {
      issues.push({
        ...base,
        type: 'VIEW_DIR_MISMATCH',
        actualViewPath,
        menuFix: {
          componentPath: `${actualViewPath}/index`,
          routePath: `/${actualViewPath}`,
        },
      });
      return issues;
    }
  }

  const entityKebab = pascalToKebab(entityArg);
  if (entityKebab) {
    const segments = menuViewPath.split('/');
    const parentPath = segments.slice(0, -1).join('/');
    const entityViewPath = parentPath ? `${parentPath}/${entityKebab}` : entityKebab;
    if (entityViewPath !== menuViewPath && hasIndexVue(entityViewPath)) {
      issues.push({
        ...base,
        type: 'VIEW_DIR_MISMATCH',
        actualViewPath: entityViewPath,
        menuFix: {
          componentPath: `${entityViewPath}/index`,
          routePath: `/${entityViewPath}`,
        },
      });
      return issues;
    }
  }

  const siblingViewPath = findSiblingViewMatch(menuViewPath, entityArg);
  if (siblingViewPath && siblingViewPath !== menuViewPath && hasIndexVue(siblingViewPath)) {
    issues.push({
      ...base,
      type: 'VIEW_DIR_MISMATCH',
      actualViewPath: siblingViewPath,
      menuFix: {
        componentPath: `${siblingViewPath}/index`,
        routePath: `/${siblingViewPath}`,
      },
    });
    return issues;
  }

  const childViewPaths = listChildViewModules(menuViewPath);
  if (childViewPaths.length) {
    issues.push({
      ...base,
      type: 'VIEW_HUB_SUBPAGES',
      childViewPaths,
    });
    return issues;
  }

  return issues;
}

/**
 * @param {PathAlignmentIssue} issue
 * @returns {string}
 */
function formatPathIssueDetail(issue) {
  const lines = [
    `[${issue.page.viewModulePath}]`,
    `  菜单: ${issue.page.menuCode || '(未知)'}`,
    `  种子: ${issue.page.sourceFile}`,
    `  权限: ${issue.page.permission}`,
    `  RoutePath: ${issue.page.routePath}`,
    `  ComponentPath: ${issue.page.componentPath}`,
  ];
  if (issue.type === 'ROUTE_COMPONENT') {
    lines.push(`  期望 ComponentPath: ${issue.expectedComponentPath}`);
  }
  if (issue.type === 'VIEW_DIR_MISMATCH' && issue.actualViewPath) {
    lines.push(`  实际 views: ${issue.actualViewPath}/index.vue`);
    if (issue.menuFix) {
      lines.push(`  应对齐 RoutePath: ${issue.page.routePath} → ${issue.menuFix.routePath}`);
      lines.push(`  应对齐 ComponentPath: ${issue.page.componentPath} → ${issue.menuFix.componentPath}`);
    }
  }
  if (issue.type === 'VIEW_HUB_SUBPAGES' && issue.childViewPaths?.length) {
    lines.push(`  子目录已有 views: ${issue.childViewPaths.join(', ')}`);
    lines.push('  说明: 菜单指向父级 index，但 views 在子目录；应改 MenuType=0 目录或新增子菜单');
  }
  lines.push('');
  return lines.join('\n');
}

/**
 * @param {PathAlignmentIssue} issue
 * @returns {string|null}
 */
function formatPathIssueChecklistLine(issue) {
  const seed = issue.page.sourceFile;
  const code = issue.page.menuCode || '(未知)';
  if (issue.type === 'ROUTE_COMPONENT' && issue.expectedComponentPath) {
    const routePath = `/${issue.expectedComponentPath.replace(/\/index$/, '')}`;
    return `${seed} | ${code} | RoutePath: ${issue.page.routePath} → ${routePath} | ComponentPath: ${issue.page.componentPath} → ${issue.expectedComponentPath}`;
  }
  if (issue.type === 'VIEW_DIR_MISMATCH' && issue.menuFix) {
    return `${seed} | ${code} | RoutePath: ${issue.page.routePath} → ${issue.menuFix.routePath} | ComponentPath: ${issue.page.componentPath} → ${issue.menuFix.componentPath}`;
  }
  if (issue.type === 'VIEW_HUB_SUBPAGES' && issue.childViewPaths?.length) {
    const childMenus = issue.childViewPaths.map((p) => `${p}/index`).join('; ');
    return `${seed} | ${code} | 改 MenuType=0 目录或新增子菜单 ComponentPath: ${childMenus}`;
  }
  return null;
}

/**
 * @typedef {{ page: ReturnType<typeof parseMenuPageEntries>[0], api: boolean, types: boolean, entityArg: string, suggest: string }} AuditMissingViewRow
 */

/**
 * 执行审计并生成报告文本
 * @returns {{ reportText: string, missingViewCount: number, reportPath: string, summary: { menuTotal: number, crudTotal: number, shellTotal: number, hasView: number, missingView: number, missingViewReady: number, missingViewNoStack: number, missingApiTypes: number } }}
 */
function runAudit() {
  const permissionEntityIndex = buildListPermissionEntityIndex();
  const pages = parseMenuPageEntries();
  /** @type {AuditMissingViewRow[]} */
  const missingViewRows = [];
  /** @type {Array<typeof pages[0] & { api: boolean, types: boolean, entityArg: string }>} */
  const missingApiTypes = [];
  /** @type {PathAlignmentIssue[]} */
  const pathAlignmentIssues = [];
  const crudPages = pages.filter((page) => !isShellViewPage(page.viewModulePath));

  for (const page of crudPages) {
    const entityArg = suggestEntityArg(page, permissionEntityIndex);
    pathAlignmentIssues.push(...auditMenuViewPathAlignment(page, entityArg));
  }

  /** @type {Map<string, PathAlignmentIssue>} */
  const primaryPathIssueByMenu = new Map();
  for (const issue of pathAlignmentIssues) {
    if (!primaryPathIssueByMenu.has(issue.menuViewPath)) {
      primaryPathIssueByMenu.set(issue.menuViewPath, issue);
    }
  }

  for (const page of crudPages) {
    const entityArg = suggestEntityArg(page, permissionEntityIndex);
    const stack = hasApiTypes(page.viewModulePath, entityArg);
    const hasView = hasIndexVue(page.viewModulePath);
    const pathIssue = primaryPathIssueByMenu.get(page.viewModulePath);
    if (!hasView) {
      if (pathIssue?.type === 'VIEW_DIR_MISMATCH' || pathIssue?.type === 'VIEW_HUB_SUBPAGES') {
        continue;
      }
      const suggest = stack.api && stack.types
        ? `${CMD.generateVueAll} --${entityArg}`
        : `${CMD.generateAll} --${entityArg} && ${CMD.generateVueAll} --${entityArg}`;
      missingViewRows.push({ page, ...stack, entityArg, suggest });
      continue;
    }
    if (!stack.api || !stack.types) {
      missingApiTypes.push({ ...page, ...stack, entityArg });
    }
  }

  const missingViewReady = missingViewRows.filter((row) => row.api && row.types);
  const missingViewNoStack = missingViewRows.filter((row) => !row.api || !row.types);
  const shellTotal = pages.length - crudPages.length;
  const pathIssueCount = pathAlignmentIssues.length;
  const stackIssueCount = missingViewRows.length + missingApiTypes.length;
  const totalIssueCount = stackIssueCount + pathIssueCount;
  const summary = {
    menuTotal: pages.length,
    crudTotal: crudPages.length,
    shellTotal,
    hasView: crudPages.length - missingViewRows.length - [...primaryPathIssueByMenu.keys()].filter((k) => {
      const issue = primaryPathIssueByMenu.get(k);
      return issue && (issue.type === 'VIEW_DIR_MISMATCH' || issue.type === 'VIEW_HUB_SUBPAGES');
    }).length,
    missingView: missingViewRows.length,
    missingViewReady: missingViewReady.length,
    missingViewNoStack: missingViewNoStack.length,
    missingApiTypes: missingApiTypes.length,
    pathIssueCount,
    stackIssueCount,
    issueCount: totalIssueCount,
  };

  const pathRouteComponent = pathAlignmentIssues.filter((i) => i.type === 'ROUTE_COMPONENT');
  const pathViewDirMismatch = pathAlignmentIssues.filter((i) => i.type === 'VIEW_DIR_MISMATCH');
  const pathViewHub = pathAlignmentIssues.filter((i) => i.type === 'VIEW_HUB_SUBPAGES');
  const menuChecklistLines = pathAlignmentIssues
    .map((issue) => formatPathIssueChecklistLine(issue))
    .filter(Boolean);

  const lines = [];
  const now = new Date();
  const ts = now.toISOString().replace('T', ' ').slice(0, 19);
  lines.push('Takt Plat - 菜单视图审计报告（仅输出缺失/不一致项）');
  lines.push(`生成时间: ${ts}`);
  lines.push(`菜单种子: ${MENU_LEVEL_FILES.join(', ')}`);
  lines.push(`命令: ${CMD.audit} [--out=路径]  （请在 scripts 目录下执行）`);
  lines.push('');
  lines.push('=== 汇总 ===');
  lines.push(`菜单页面(MenuType=1): ${summary.menuTotal}`);
  lines.push(`CRUD/业务页(已排除壳页 ${summary.shellTotal}): ${summary.crudTotal}`);
  lines.push(`菜单↔views 目录不一致: ${pathIssueCount}`);
  lines.push(`  - RoutePath 与 ComponentPath 不一致: ${pathRouteComponent.length}`);
  lines.push(`  - 菜单路径与 views 目录不一致: ${pathViewDirMismatch.length}`);
  lines.push(`  - 父级菜单缺 index 但子目录有 views: ${pathViewHub.length}`);
  lines.push(`视图/api 栈异常: ${stackIssueCount}`);
  lines.push(`  - 缺少 index.vue: ${summary.missingView}`);
  lines.push(`  - 有视图缺 api/types: ${summary.missingApiTypes}`);
  lines.push(`异常项合计: ${totalIssueCount}`);
  lines.push(`审计结论: ${totalIssueCount === 0 ? 'PASS' : 'FAIL'}`);
  lines.push('');

  if (totalIssueCount === 0) {
    lines.push('无缺失项，菜单 ComponentPath/RoutePath 与 views 目录均已对齐。');
  } else {
    if (pathIssueCount) {
      if (pathRouteComponent.length) {
        lines.push(`=== RoutePath 与 ComponentPath 不一致 (${pathRouteComponent.length}) ===`);
        for (const issue of pathRouteComponent) {
          lines.push(formatPathIssueDetail(issue));
        }
      }
      if (pathViewDirMismatch.length) {
        lines.push(`=== 菜单路径与 views 目录不一致 (${pathViewDirMismatch.length}) ===`);
        for (const issue of pathViewDirMismatch) {
          lines.push(formatPathIssueDetail(issue));
        }
      }
      if (pathViewHub.length) {
        lines.push(`=== 父级菜单缺 index 但子目录有 views (${pathViewHub.length}) ===`);
        for (const issue of pathViewHub) {
          lines.push(formatPathIssueDetail(issue));
        }
      }
    }

    /**
     * @param {string} title
     * @param {AuditMissingViewRow[]} rows
     */
    function appendIssueDetailSection(title, rows) {
      if (!rows.length) {
        return;
      }
      lines.push(`=== ${title} (${rows.length}) ===`);
      for (const row of rows) {
        lines.push(`[${row.page.viewModulePath}]`);
        lines.push(`  菜单: ${row.page.menuCode || '(未知)'}`);
        lines.push(`  权限: ${row.page.permission}`);
        lines.push(`  路由: ${row.page.routePath}`);
        lines.push(`  种子: ${row.page.sourceFile}`);
        lines.push(`  实体: ${row.entityArg}`);
        lines.push(`  api: ${mark(row.api)}  types: ${mark(row.types)}  view: N`);
        lines.push('');
      }
    }

    appendIssueDetailSection('缺少 index.vue - 仅缺视图', missingViewReady);
    appendIssueDetailSection('缺少 index.vue - 缺 API/types', missingViewNoStack);

    if (missingApiTypes.length) {
      lines.push(`=== 有 index.vue 但缺 api/types (${missingApiTypes.length}) ===`);
      for (const page of missingApiTypes) {
        lines.push(`[${page.viewModulePath}] api=${mark(page.api)} types=${mark(page.types)} | ${page.permission} | entity=${page.entityArg}`);
      }
      lines.push('');
    }

    if (stackIssueCount) {
      lines.push(`=== 异常清单 TSV (${stackIssueCount}, 可导入 Excel) ===`);
      lines.push('status\tviewModulePath\tmenuCode\tpermission\troutePath\tapi\ttypes\tview\tentityArg');
      for (const row of missingViewReady) {
        lines.push([
          'MISSING_VIEW',
          row.page.viewModulePath,
          row.page.menuCode || '',
          row.page.permission,
          row.page.routePath,
          mark(row.api),
          mark(row.types),
          'N',
          row.entityArg,
        ].join('\t'));
      }
      for (const row of missingViewNoStack) {
        lines.push([
          'MISSING_STACK',
          row.page.viewModulePath,
          row.page.menuCode || '',
          row.page.permission,
          row.page.routePath,
          mark(row.api),
          mark(row.types),
          'N',
          row.entityArg,
        ].join('\t'));
      }
      for (const page of missingApiTypes) {
        lines.push([
          'PARTIAL_STACK',
          page.viewModulePath,
          page.menuCode || '',
          page.permission,
          page.routePath,
          mark(page.api),
          mark(page.types),
          'Y',
          page.entityArg,
        ].join('\t'));
      }
      lines.push('');
    }

    lines.push('=== 修改清单（汇总；工作目录 scripts/）===');
    if (menuChecklistLines.length) {
      lines.push('');
      lines.push(`# 1. 菜单种子 ${MENU_LEVEL_FILES.join(' / ')}（RoutePath / ComponentPath / MenuType）`);
      for (const line of menuChecklistLines) {
        lines.push(line);
      }
    }
    if (missingViewReady.length || missingViewNoStack.length || missingApiTypes.length) {
      if (missingViewReady.length) {
        lines.push('');
        lines.push(`# 2. 仅缺视图 (${missingViewReady.length}) → generate-vue-all-from-api`);
        for (const row of missingViewReady) {
          lines.push(`${CMD.generateVueAll} --${row.entityArg}`);
        }
      }
      if (missingViewNoStack.length) {
        lines.push('');
        lines.push(`# ${missingViewReady.length ? '3' : '2'}. 缺全栈 (${missingViewNoStack.length}) → generate-all`);
        for (const row of missingViewNoStack) {
          lines.push(`${CMD.generateAll} --${row.entityArg}`);
        }
      }
      if (missingApiTypes.length) {
        const step = (missingViewReady.length ? 1 : 0) + (missingViewNoStack.length ? 1 : 0) + 2;
        lines.push('');
        lines.push(`# ${step}. 有视图缺 api/types (${missingApiTypes.length}) → generate-from-backend`);
        for (const page of missingApiTypes) {
          lines.push(`${CMD.generateFromBackend} --${page.entityArg}`);
        }
      }
    }
    if (!menuChecklistLines.length && !missingViewReady.length && !missingViewNoStack.length && !missingApiTypes.length) {
      lines.push('(无)');
    }
    lines.push('');
    lines.push('字段说明: 菜单对照 TaktMenuLevel1~5SeedData；views 路径约定为 frontend/src/views/{ComponentPath 去 /index}.vue');
  }

  const reportPath = resolveReportOutputPath();
  const reportText = `${lines.join('\n')}\n`;
  fs.mkdirSync(path.dirname(reportPath), { recursive: true });
  fs.writeFileSync(reportPath, reportText, 'utf-8');

  return {
    reportText,
    missingViewCount: missingViewRows.length,
    issueCount: totalIssueCount,
    reportPath,
    summary,
  };
}

function main() {
  const { reportPath, issueCount, summary } = runAudit();
  console.log(`CRUD/业务页: ${summary.crudTotal} | 目录不一致: ${summary.pathIssueCount} | 栈异常: ${summary.stackIssueCount}`);
  console.log(`报告已写入: ${reportPath}`);
  if (issueCount === 0) {
    console.log('审计结论: PASS');
  } else {
    console.log(`审计结论: FAIL（合计 ${issueCount} 项）`);
  }
  process.exit(issueCount ? 1 : 0);
}

if (require.main === module) {
  main();
}

module.exports = {
  parseMenuPageEntries,
  hasIndexVue,
  auditMenuViewPathAlignment,
  runAudit,
  DEFAULT_REPORT_FILE,
  MENU_LEVEL_FILES,
};
