// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：scripts
// 文件名称：audit-menu-permission.cjs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：校验菜单种子 Permission：重复、格式；控制器 :list 须与菜单种子对齐（权威：菜单种子）；默认输出 scripts/reports/audit-menu-permission.txt
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const path = require('path');
const {
  REPORTS_DIR,
  resolveReportOutputPath,
  parseAllMenuSeedBlocks,
  groupByKey,
  duplicateGroups,
  formatDuplicateSection,
  collectInvalidMenuPermissions,
  collectMenuPermissionMismatches,
  filterReportableListPermissionDupes,
  formatMenuSeedVsControllerSection,
  finishAuditReport,
} = require('./audit-menu-field-common.cjs');

const DEFAULT_REPORT_REL = 'scripts/reports/audit-menu-permission.txt';
const DEFAULT_REPORT_FILE = path.join(REPORTS_DIR, 'audit-menu-permission.txt');

function main() {
  const reportPath = resolveReportOutputPath(DEFAULT_REPORT_FILE);
  const blocks = parseAllMenuSeedBlocks();
  const withPermission = blocks.filter((block) => block.permission);
  const listPermissions = withPermission.filter((block) => block.permission.endsWith(':list'));
  const listDupes = filterReportableListPermissionDupes(
    duplicateGroups(groupByKey(listPermissions, (block) => block.permission)),
  );
  const invalidPermissions = collectInvalidMenuPermissions(withPermission);
  const controllerMismatches = collectMenuPermissionMismatches();

  const failCount = listDupes.length
    + invalidPermissions.length
    + controllerMismatches.length;

  const lines = [
    'Takt Menu Permission Audit',
    `Report: ${DEFAULT_REPORT_REL}`,
    `Generated: ${new Date().toISOString()}`,
    `Blocks scanned: ${blocks.length} | With Permission: ${withPermission.length}`,
    '',
    `RESULT: ${failCount === 0 ? 'PASS' : 'FAIL'} (${failCount} issues)`,
    '',
    '权威：菜单种子 menu.Permission（控制器/视图须向其对齐，禁止反向）',
    '',
    '检查项:',
    '  1. 页面 :list 权限在菜单种子内重复（排除主表 + ChangeLog 合法共用）',
    '  2. 菜单种子 Permission 格式非法或词干重复',
    '  3. 控制器 [TaktPermission] :list ≠ 对应页面菜单种子',
    '  4. 三字段路径分段与 MenuCode 一致见 audit-menu-structure.cjs',
    '',
    '全栈对账（控制器/视图/前端）: node scripts/audit-permissions.cjs',
    '',
    '=== 菜单种子 :list 重复 ===',
    ...formatDuplicateSection('重复组数', listDupes),
    '',
    `=== 菜单种子格式/词干问题 (${invalidPermissions.length}) ===`,
    ...(invalidPermissions.length === 0 ? ['无'] : invalidPermissions.slice(0, 100).map((item) => {
      const suggest = item.suggested ? ` → 建议 ${item.suggested}` : '';
      return `  [${item.block.menuCode}] ${item.block.permission}${suggest} | ${item.block.sourceFile}:${item.block.line} | ${item.reason}`;
    })),
    '',
    '=== 控制器未与菜单种子对齐 ===',
    ...formatMenuSeedVsControllerSection(controllerMismatches),
  ];

  console.log(`   块数 ${blocks.length} | 种子:list重复 ${listDupes.length} | 格式/词干 ${invalidPermissions.length} | 控制器≠种子 ${controllerMismatches.length}`);
  finishAuditReport(reportPath, lines, failCount, 'node scripts/audit-menu-permission.cjs');
}

main();
