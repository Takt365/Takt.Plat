// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：scripts
// 文件名称：audit-menu-code.cjs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：校验菜单种子 MenuCode：重复、lookupKey 不一致、与实体期望不一致；默认输出 scripts/reports/audit-menu-code.txt
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
  collectEntityFieldMismatches,
  formatEntityMismatchSection,
  finishAuditReport,
} = require('./audit-menu-field-common.cjs');

const DEFAULT_REPORT_REL = 'scripts/reports/audit-menu-code.txt';
const DEFAULT_REPORT_FILE = path.join(REPORTS_DIR, 'audit-menu-code.txt');

function main() {
  const reportPath = resolveReportOutputPath(DEFAULT_REPORT_FILE);
  const blocks = parseAllMenuSeedBlocks();
  const menuCodeDupes = duplicateGroups(groupByKey(blocks.filter((b) => b.menuCode), (b) => b.menuCode));
  const lookupDupes = duplicateGroups(groupByKey(blocks.filter((b) => b.lookupKey), (b) => b.lookupKey));
  const lookupMismatch = blocks.filter((b) => b.lookupKey && b.menuCode && b.lookupKey !== b.menuCode);
  const missingMenuCode = blocks.filter((b) => !b.menuCode).length;
  const entityMismatches = collectEntityFieldMismatches(blocks, 'menuCode');

  const failCount = menuCodeDupes.length + lookupDupes.length + lookupMismatch.length + entityMismatches.length;

  const lines = [
    'Takt MenuCode Audit',
    `Report: ${DEFAULT_REPORT_REL}`,
    `Generated: ${new Date().toISOString()}`,
    `Blocks scanned: ${blocks.length}`,
    '',
    `RESULT: ${failCount === 0 ? 'PASS' : 'FAIL'} (${failCount} issue groups)`,
    '',
    '检查项:',
    '  1. menu.MenuCode 重复',
    '  2. CreateOrUpdateMenuAsync lookupKey 重复',
    '  3. lookupKey !== menu.MenuCode',
    '  4. 页面菜单 MenuCode 与实体服务路径推导不一致（materials 与 material 全字区分）',
    '  5. 三字段结构见 audit-menu-structure.cjs（MenuCode _ 大写 / I18nKey . 小写 / Permission : 小写）',
    '',
    `menu.MenuCode 缺失: ${missingMenuCode}`,
    '',
    '=== menu.MenuCode 重复 ===',
    ...formatDuplicateSection('重复组数', menuCodeDupes),
    '',
    '=== lookupKey 重复 ===',
    ...formatDuplicateSection('重复组数', lookupDupes),
    '',
    `=== lookupKey !== menu.MenuCode (${lookupMismatch.length}) ===`,
    ...(lookupMismatch.length === 0 ? ['无'] : lookupMismatch.map((b) => `  [${b.lookupKey}] menuCode=${b.menuCode} | ${b.sourceFile}:${b.line}`)),
    '',
    '=== 实体期望不一致 ===',
    ...formatEntityMismatchSection(entityMismatches, 'MenuCode'),
  ];

  console.log(`   块数 ${blocks.length} | MenuCode重复 ${menuCodeDupes.length} | lookup重复 ${lookupDupes.length} | lookup≠menuCode ${lookupMismatch.length} | 实体不一致 ${entityMismatches.length}`);
  finishAuditReport(reportPath, lines, failCount, 'node scripts/audit-menu-code.cjs');
}

main();
