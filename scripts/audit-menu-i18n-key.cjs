// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：scripts
// 文件名称：audit-menu-i18n-key.cjs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：校验菜单种子 I18nKey：重复、格式、menu.* 种子覆盖、与 MenuCode 分段一致；默认输出 scripts/reports/audit-menu-i18n-key.txt
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
  parseMenuI18nSeedKeys,
  collectInvalidI18nKeys,
  collectMissingMenuI18nSeed,
  formatEntityMismatchSection,
  finishAuditReport,
} = require('./audit-menu-field-common.cjs');
const { collectDirectoryMenuI18nMismatches } = require('./menu-field-structure.cjs');

const DEFAULT_REPORT_REL = 'scripts/reports/audit-menu-i18n-key.txt';
const DEFAULT_REPORT_FILE = path.join(REPORTS_DIR, 'audit-menu-i18n-key.txt');

function main() {
  const reportPath = resolveReportOutputPath(DEFAULT_REPORT_FILE);
  const blocks = parseAllMenuSeedBlocks();
  const withI18n = blocks.filter((b) => b.i18nKey);
  const seedKeys = parseMenuI18nSeedKeys();
  const i18nDupes = duplicateGroups(groupByKey(withI18n, (b) => b.i18nKey));
  const invalidI18n = collectInvalidI18nKeys(withI18n);
  const missingSeed = collectMissingMenuI18nSeed(withI18n, seedKeys);
  const entityMismatches = collectEntityFieldMismatches(blocks, 'i18nKey');
  const directoryMismatches = collectDirectoryMenuI18nMismatches(blocks);

  const failCount = i18nDupes.length + invalidI18n.length + missingSeed.length + entityMismatches.length + directoryMismatches.length;

  const lines = [
    'Takt Menu I18nKey Audit',
    `Report: ${DEFAULT_REPORT_REL}`,
    `Generated: ${new Date().toISOString()}`,
    `Blocks scanned: ${blocks.length} | With I18nKey: ${withI18n.length} | Menu i18n seed keys: ${seedKeys.size}`,
    '',
    `RESULT: ${failCount === 0 ? 'PASS' : 'FAIL'} (${failCount} issues)`,
    '',
    '检查项:',
    '  1. menu.I18nKey 重复',
    '  2. I18nKey 格式（须 menu.* 小写点号，段内无下划线）',
    '  3. menu.* 键在 TaktMenuI18nSeedData.cs 中缺失',
    '  4. 页面菜单 I18nKey 与 MenuCode 分段推导不一致',
    '  5. 目录菜单（MenuType=0）I18nKey 须为 MenuCode 分段 + ._self',
    '  6. 三字段路径分段与 MenuCode 一致见 audit-menu-structure.cjs',
    '',
    '=== I18nKey 重复 ===',
    ...formatDuplicateSection('重复组数', i18nDupes),
    '',
    `=== I18nKey 格式非法 (${invalidI18n.length}) ===`,
    ...(invalidI18n.length === 0 ? ['无'] : invalidI18n.map((item) => `  [${item.block.menuCode}] ${item.block.i18nKey} | ${item.block.sourceFile}:${item.block.line} | ${item.reason}`)),
    '',
    `=== TaktMenuI18nSeedData 缺失 (${missingSeed.length}) ===`,
    ...(missingSeed.length === 0 ? ['无'] : missingSeed.slice(0, 100).map((item) => `  [${item.block.menuCode}] ${item.block.i18nKey} | ${item.block.sourceFile}:${item.block.line}`)),
    '',
    '=== 实体期望不一致 ===',
    ...formatEntityMismatchSection(entityMismatches, 'I18nKey'),
    '',
    `=== 目录菜单 I18nKey 不一致 (${directoryMismatches.length}) ===`,
    ...(directoryMismatches.length === 0
      ? ['无']
      : directoryMismatches.map((item) => {
        const lines = [
          `  [${item.block.menuCode}] ${item.block.menuName || ''} | ${item.block.sourceFile}:${item.block.line}`,
        ];
        for (const issue of item.issues) {
          lines.push(`    · ${issue}`);
        }
        return lines.join('\n');
      })),
  ];

  console.log(`   块数 ${blocks.length} | I18nKey重复 ${i18nDupes.length} | 格式非法 ${invalidI18n.length} | 种子缺失 ${missingSeed.length} | 实体不一致 ${entityMismatches.length} | 目录不一致 ${directoryMismatches.length}`);
  finishAuditReport(reportPath, lines, failCount, 'node scripts/audit-menu-i18n-key.cjs');
}

main();
