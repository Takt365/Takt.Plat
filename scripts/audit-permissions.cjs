// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：scripts
// 文件名称：audit-permissions.cjs
// 创建时间：2026-06-20
// 创建人：Takt365(Cursor AI)
// 功能描述：全量对账 TaktMenuLevel1~5 种子、全部控制器 TaktPermission、views 权限；格式 领域:目录:实体:操作，目录与实体重复时去重
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const fs = require('fs');
const path = require('path');
const { getControllerClassName } = require('./generate-script-common.cjs');
const { buildPermissionBase, detectPermissionStemRedundancy } = require('./permission-base.cjs');
const {
  BACKEND_ROOT,
  scanAllServices,
  findControllerFile,
  buildControllerPermissionIndex,
  parseAllMenuEntries,
  scanVueViewPermissions,
  permissionToBase,
  resolveEntityFromMenuListPermission,
  resolveEntityFromViewPath,
} = require('./audit-permission-scan.cjs');

const REPORTS_DIR = path.resolve(__dirname, 'reports');
const DEFAULT_REPORT_REL = 'scripts/reports/audit-permissions.txt';
const DEFAULT_REPORT_FILE = path.join(REPORTS_DIR, 'audit-permissions.txt');

const CRUD_ACTIONS = ['list', 'query', 'create', 'update', 'delete', 'import', 'export'];

/**
 * @returns {string}
 */
function resolveReportOutputPath() {
  const outArg = process.argv.find((arg) => arg.startsWith('--out='));
  if (outArg) {
    const custom = outArg.slice('--out='.length).trim();
    if (path.isAbsolute(custom)) {
      return custom;
    }
    if (custom.startsWith('scripts/') || custom.startsWith('scripts\\')) {
      return path.resolve(__dirname, '..', custom);
    }
    return path.resolve(__dirname, custom);
  }
  return DEFAULT_REPORT_FILE;
}

/**
 * @param {string[]} lines
 * @param {string} title
 * @param {Array<{ key: string, detail: string }>} items
 */
function appendSection(lines, title, items) {
  lines.push('');
  lines.push(`=== ${title} (${items.length}) ===`);
  if (items.length === 0) {
    lines.push('  (none)');
    return;
  }
  for (const item of items) {
    lines.push(`  [${item.key}] ${item.detail}`);
  }
}

/**
 * @param {string} permissionCode
 * @returns {boolean}
 */
function isValidPermissionFormat(permissionCode) {
  if (!permissionCode || permissionCode.includes('.')) {
    return false;
  }
  return /^[a-z0-9]+(:[a-z0-9]+)*$/.test(permissionCode);
}

function main() {
  const reportPath = resolveReportOutputPath();
  const reportDir = path.dirname(reportPath);
  if (!fs.existsSync(reportDir)) {
    fs.mkdirSync(reportDir, { recursive: true });
  }

  const services = scanAllServices();
  const controllerIndex = buildControllerPermissionIndex();
  const menuEntries = parseAllMenuEntries();
  const vueRows = scanVueViewPermissions();

  /** @type {Map<string, string>} */
  const expectedBaseByEntity = new Map();
  for (const svc of services) {
    expectedBaseByEntity.set(svc.entityShort, buildPermissionBase(svc.pathParts, svc.entityShort).toLowerCase());
  }

  /** @type {Array<{ key: string, detail: string }>} */
  const controllerExpectedMismatch = [];
  /** @type {Array<{ key: string, detail: string }>} */
  const menuPageMismatch = [];
  /** @type {Array<{ key: string, detail: string }>} */
  const vueControllerMismatch = [];
  /** @type {Array<{ key: string, detail: string }>} */
  const stemRedundant = [];
  /** @type {Array<{ key: string, detail: string }>} */
  const invalidFormat = [];
  /** @type {Array<{ key: string, detail: string }>} */
  const controllerActionGap = [];

  for (const svc of services) {
    const expected = expectedBaseByEntity.get(svc.entityShort);
    const actual = controllerIndex.listBaseByEntity.get(svc.entityShort);
    const controllerFile = findControllerFile(svc.entityShort);
    if (!controllerFile) {
      controllerExpectedMismatch.push({
        key: svc.entityShort,
        detail: `缺少 ${getControllerClassName(svc.entityShort)}；期望 list 前缀 ${expected}`,
      });
      continue;
    }
    if (actual !== expected) {
      controllerExpectedMismatch.push({
        key: svc.entityShort,
        detail: `期望 ${expected}:list，实际 ${actual || '(none)'}:list | ${path.relative(BACKEND_ROOT, controllerFile).replace(/\\/g, '/')}`,
      });
    }
  }

  /** @type {Set<string>} */
  const stemSeen = new Set();

  /**
   * @param {string} source
   * @param {string} file
   * @param {string} permission
   */
  function checkStemAndFormat(source, file, permission) {
    if (!isValidPermissionFormat(permission)) {
      invalidFormat.push({
        key: source,
        detail: `${permission} 格式非法（须小写冒号分段）| ${file}`,
      });
      return;
    }
    const hit = detectPermissionStemRedundancy(permission);
    if (!hit) {
      return;
    }
    const dedupeKey = `${permission}|${file}`;
    if (stemSeen.has(dedupeKey)) {
      return;
    }
    stemSeen.add(dedupeKey);
    stemRedundant.push({
      key: source,
      detail: `${permission} → 建议 ${hit.suggestedPrefix}:${permission.split(':').pop()} | ${file}`,
    });
  }

  for (const menu of menuEntries) {
    checkStemAndFormat('menu', `EntitySeedData/${menu.sourceFile} [${menu.menuCode}]`, menu.permission);
    if (menu.menuType !== 1 || !menu.permission.endsWith(':list')) {
      continue;
    }
    const entityShort = resolveEntityFromMenuListPermission(menu.permission, controllerIndex.entityByListBase);
    if (!entityShort) {
      continue;
    }
    const expected = expectedBaseByEntity.get(entityShort);
    const actualBase = menu.permission.replace(/:list$/, '');
    if (expected && actualBase !== expected) {
      menuPageMismatch.push({
        key: menu.menuCode,
        detail: `菜单 ${menu.permission} 应对齐 ${expected}:list | ${menu.sourceFile} | entity=${entityShort}`,
      });
    }
  }

  for (const row of vueRows) {
    for (const perm of row.permissions) {
      checkStemAndFormat('vue', row.file, perm);
    }
    if (!row.toolbarBase) {
      continue;
    }
    let entityShort = resolveEntityFromViewPath(row.viewModulePath, controllerIndex.listBaseByEntity);
    if (!entityShort && controllerIndex.entityByListBase.has(row.toolbarBase)) {
      entityShort = controllerIndex.entityByListBase.get(row.toolbarBase);
    }
    if (!entityShort) {
      continue;
    }
    const controllerBase = controllerIndex.listBaseByEntity.get(entityShort);
    const expected = expectedBaseByEntity.get(entityShort);
    if (controllerBase && row.toolbarBase !== controllerBase) {
      vueControllerMismatch.push({
        key: row.viewModulePath,
        detail: `视图工具栏前缀 ${row.toolbarBase} ≠ 控制器 ${controllerBase} | ${row.file}`,
      });
    } else if (expected && row.toolbarBase !== expected) {
      vueControllerMismatch.push({
        key: row.viewModulePath,
        detail: `视图工具栏前缀 ${row.toolbarBase} ≠ 期望 ${expected} | ${row.file}`,
      });
    }
  }

  for (const [entityShort, listBase] of controllerIndex.listBaseByEntity.entries()) {
    const expected = expectedBaseByEntity.get(entityShort);
    if (!expected || listBase === expected) {
      continue;
    }
    const controllerFile = controllerIndex.controllerFileByEntity.get(entityShort);
    for (const perm of controllerIndex.allControllerPermissions) {
      if (!perm.startsWith(`${listBase}:`)) {
        continue;
      }
      checkStemAndFormat('controller', path.relative(BACKEND_ROOT, controllerFile || '').replace(/\\/g, '/'), perm);
    }
  }

  for (const svc of services) {
    const expected = expectedBaseByEntity.get(svc.entityShort);
    const actualBase = controllerIndex.listBaseByEntity.get(svc.entityShort);
    if (!expected || !actualBase || actualBase === expected) {
      continue;
    }
    for (const action of CRUD_ACTIONS) {
      const expectedPerm = `${expected}:${action}`;
      if (!controllerIndex.allControllerPermissions.has(expectedPerm)
        && controllerIndex.allControllerPermissions.has(`${actualBase}:${action}`)) {
        controllerActionGap.push({
          key: svc.entityShort,
          detail: `缺少 ${expectedPerm}（仍使用 ${actualBase}:${action}）`,
        });
        break;
      }
    }
  }

  const failCount = controllerExpectedMismatch.length
    + menuPageMismatch.length
    + vueControllerMismatch.length
    + stemRedundant.length
    + invalidFormat.length;

  const lines = [
    'Takt Permission Audit Report',
    `Report: ${DEFAULT_REPORT_REL}`,
    `Generated: ${new Date().toISOString()}`,
    `Services: ${services.length} | Menu entries: ${menuEntries.length} | Vue views: ${vueRows.length}`,
    '',
    `RESULT: ${failCount === 0 ? 'PASS' : 'FAIL'}`,
    '',
    '权限格式: 领域:目录:实体词:…:操作（PascalCase 实体拆为冒号词段，如 cost:center、standard:wage:rate）',
    'ChangeLog: 主实体前缀 + :change:log',
    '主子表: 子实体名以主表名为前缀时继承主表权限 + 子表剩余词段（如 overtime:item、news:attachment）',
    '  例: accounting:controlling:cost:center:list',
    '  例: humanresource:attendance:overtime:item:list',
    '  例: routine:announcement:announcement:list',
    '',
    '对账范围:',
    '  - TaktMenuLevel1~5SeedData.cs 全部 menu.Permission',
    '  - Controllers/** [TaktPermission]',
    '  - frontend/src/views/** index.vue / *-panel.vue 工具栏与 v-permission',
    '  - 期望前缀: buildPermissionBase(服务路径, 实体) — 与 generate-controllers-from-services 一致',
  ];

  appendSection(lines, 'CONTROLLER_VS_EXPECTED（控制器 :list 前缀 ≠ buildPermissionBase）', controllerExpectedMismatch);
  appendSection(lines, 'MENU_PAGE_VS_EXPECTED（页面菜单 :list ≠ 实体期望前缀）', menuPageMismatch);
  appendSection(lines, 'VUE_VS_CONTROLLER（视图工具栏前缀 ≠ 控制器/期望）', vueControllerMismatch);
  appendSection(lines, 'STEM_REDUNDANT（模块/实体词干重复，如 newscenter:news）', stemRedundant);
  appendSection(lines, 'INVALID_FORMAT（权限码格式非法）', invalidFormat);
  appendSection(lines, 'CONTROLLER_ACTION_GAP（控制器仍用旧前缀 CRUD 码）', controllerActionGap.slice(0, 50));

  if (controllerExpectedMismatch.length > 0) {
    lines.push('');
    lines.push('批量修复控制器（逐实体）:');
    const entities = [...new Set(controllerExpectedMismatch.map((x) => x.key))].sort();
    for (const entityShort of entities.slice(0, 40)) {
      lines.push(`  node scripts/generate-controllers-from-services.cjs --${entityShort}`);
    }
    if (entities.length > 40) {
      lines.push(`  ... 共 ${entities.length} 个实体`);
    }
  }

  lines.push('');
  lines.push('修复后重跑: node scripts/audit-permissions.cjs');

  fs.writeFileSync(reportPath, `${lines.join('\n')}\n`, 'utf-8');
  console.log(`📄 报告: ${reportPath}`);
  console.log(`   控制器期望不一致 ${controllerExpectedMismatch.length}`);
  console.log(`   菜单页面不一致 ${menuPageMismatch.length}`);
  console.log(`   视图不一致 ${vueControllerMismatch.length}`);
  console.log(`   词干重复 ${stemRedundant.length}`);
  console.log(`   结果: ${failCount === 0 ? 'PASS ✅' : 'FAIL ❌'}`);

  if (failCount > 0) {
    process.exit(1);
  }
}

main();
