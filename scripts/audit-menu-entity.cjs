// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：scripts
// 文件名称：audit-menu-entity.cjs
// 创建时间：2026-06-20
// 创建人：Takt365(Cursor AI)
// 功能描述：对账页面菜单 MenuCode / I18nKey / Permission / RoutePath / ComponentPath 与实体全名是否一致；默认输出 scripts/reports/audit-menu-entity.txt
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const fs = require('fs');
const path = require('path');
const { scanAllServices, parseAllMenuEntries } = require('./audit-permission-scan.cjs');
const { buildMasterDetailChildRegistry } = require('./generate-vue-common.cjs');
const {
  SKIP_MENU_CODES,
  isShellViewPage,
  buildExpectedMenuFields,
  resolveServiceFromViewModulePath,
} = require('./menu-entity-expectations.cjs');

const SCRIPTS_ROOT = path.resolve(__dirname);
const REPORTS_DIR = path.join(SCRIPTS_ROOT, 'reports');
const DEFAULT_REPORT_REL = 'scripts/reports/audit-menu-entity.txt';
const DEFAULT_REPORT_FILE = path.join(REPORTS_DIR, 'audit-menu-entity.txt');

const FIELD_LABELS = ['MenuCode', 'I18nKey', 'Permission', 'RoutePath', 'ComponentPath'];

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
      return path.resolve(SCRIPTS_ROOT, '..', custom);
    }
    return path.resolve(SCRIPTS_ROOT, custom);
  }
  return DEFAULT_REPORT_FILE;
}

/**
 * @param {string} actual
 * @param {string} expected
 * @returns {boolean}
 */
function fieldMatches(actual, expected) {
  return String(actual || '').trim() === String(expected || '').trim();
}

/**
 * @param {ReturnType<typeof parseAllMenuEntries>[0]} menu
 * @param {ReturnType<typeof buildExpectedMenuFields>} expected
 * @returns {Array<{ field: string, actual: string, expected: string }>}
 */
function diffMenuFields(menu, expected) {
  /** @type {ReturnType<typeof diffMenuFields>} */
  const diffs = [];
  const pairs = [
    ['MenuCode', menu.menuCode, expected.menuCode],
    ['I18nKey', menu.i18nKey, expected.i18nKey],
    ['Permission', menu.permission, expected.permission],
    ['RoutePath', menu.routePath, expected.routePath],
    ['ComponentPath', menu.componentPath, expected.componentPath],
  ];
  for (const [field, actual, exp] of pairs) {
    if (!fieldMatches(actual, exp)) {
      diffs.push({ field, actual: String(actual), expected: String(exp) });
    }
  }
  return diffs;
}

function main() {
  const reportPath = resolveReportOutputPath();
  const reportDir = path.dirname(reportPath);
  if (!fs.existsSync(reportDir)) {
    fs.mkdirSync(reportDir, { recursive: true });
  }

  const services = scanAllServices();
  const childRegistry = buildMasterDetailChildRegistry();
  /** @type {Map<string, { pathParts: string[] }>} */
  const serviceByEntity = new Map(services.map((svc) => [svc.entityShort, svc]));
  const menuEntries = parseAllMenuEntries();
  const pageMenus = menuEntries.filter((menu) => menu.menuType === 1
    && menu.componentPath.endsWith('/index')
    && !SKIP_MENU_CODES.has(menu.menuCode)
    && !isShellViewPage(menu.viewModulePath));

  /** @type {Array<{ menu: typeof pageMenus[0], entityShort: string, diffs: ReturnType<typeof diffMenuFields> }>} */
  const mismatches = [];
  /** @type {Array<{ menuCode: string, viewModulePath: string, sourceFile: string }>} */
  const unresolved = [];

  for (const menu of pageMenus) {
    const svc = resolveServiceFromViewModulePath(menu.viewModulePath, services);
    if (!svc) {
      unresolved.push({
        menuCode: menu.menuCode,
        viewModulePath: menu.viewModulePath,
        sourceFile: menu.sourceFile,
      });
      continue;
    }
    const expected = buildExpectedMenuFields(svc, childRegistry, menu.viewModulePath, serviceByEntity);
    const diffs = diffMenuFields(menu, expected);
    if (diffs.length > 0) {
      mismatches.push({ menu, entityShort: svc.entityShort, diffs });
    }
  }

  mismatches.sort((a, b) => a.menu.menuCode.localeCompare(b.menu.menuCode));
  unresolved.sort((a, b) => a.menuCode.localeCompare(b.menuCode));

  const failCount = mismatches.length;
  const lines = [
    'Takt Menu ↔ Entity Alignment Audit',
    `Report: ${DEFAULT_REPORT_REL}`,
    `Generated: ${new Date().toISOString()}`,
    `Page menus scanned: ${pageMenus.length} | Services: ${services.length}`,
    '',
    `RESULT: ${failCount === 0 ? 'PASS' : 'FAIL'} (${failCount} menus with field mismatch)`,
    '',
    '对账字段: MenuCode | I18nKey | Permission | RoutePath | ComponentPath',
    '期望来源: 实体服务路径 + 实体 Pascal 全名（全字匹配；materials 与 material 不视为重复）',
    'Route/Component: 实体 kebab 全名；服务末级目录与实体同名时不再重复（如 routine/announcement/index）',
    'ChangeLog: MenuCode/I18nKey/Route 含 change-log（Route）与 changelog（I18nKey 段内无短横线）；Permission 继承主表 list',
    '',
    `未解析实体（手工页/壳页，不计 FAIL）: ${unresolved.length}`,
  ];

  if (mismatches.length === 0) {
    lines.push('');
    lines.push('全部可解析页面菜单五字段与实体一致 ✅');
  } else {
    lines.push('');
    lines.push(`=== 字段不一致 (${mismatches.length}) ===`);
    for (const item of mismatches) {
      lines.push('');
      lines.push(`[${item.menu.menuCode}] entity=${item.entityShort} | ${item.menu.sourceFile}`);
      lines.push(`  view: ${item.menu.viewModulePath}`);
      for (const diff of item.diffs) {
        lines.push(`  ${diff.field}:`);
        lines.push(`    实际: ${diff.actual}`);
        lines.push(`    期望: ${diff.expected}`);
      }
    }
  }

  if (unresolved.length > 0 && process.argv.includes('--verbose')) {
    lines.push('');
    lines.push(`=== 未解析实体 (${unresolved.length}) ===`);
    for (const row of unresolved.slice(0, 80)) {
      lines.push(`  [${row.menuCode}] ${row.viewModulePath} | ${row.sourceFile}`);
    }
    if (unresolved.length > 80) {
      lines.push(`  ... 共 ${unresolved.length} 条`);
    }
  }

  lines.push('');
  lines.push('重跑: node scripts/audit-menu-entity.cjs');
  lines.push('详细未解析: node scripts/audit-menu-entity.cjs --verbose');

  fs.writeFileSync(reportPath, `${lines.join('\n')}\n`, 'utf-8');
  console.log(`📄 报告: ${reportPath}`);
  console.log(`   页面菜单 ${pageMenus.length} | 不一致 ${mismatches.length} | 未解析 ${unresolved.length}`);
  console.log(`   结果: ${failCount === 0 ? 'PASS ✅' : 'FAIL ❌'}`);

  if (failCount > 0) {
    process.exit(1);
  }
}

main();
