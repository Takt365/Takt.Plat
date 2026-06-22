// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：scripts
// 文件名称：audit-menu-structure.cjs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：校验页面菜单 MenuCode / I18nKey / Permission 三字段路径分段一致；默认输出 scripts/reports/audit-menu-structure.txt
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const path = require('path');
const {
  REPORTS_DIR,
  resolveReportOutputPath,
  getPageMenuBlocks,
  finishAuditReport,
} = require('./audit-menu-field-common.cjs');
const { collectMenuFieldStructureMismatches } = require('./menu-field-structure.cjs');

const DEFAULT_REPORT_REL = 'scripts/reports/audit-menu-structure.txt';
const DEFAULT_REPORT_FILE = path.join(REPORTS_DIR, 'audit-menu-structure.txt');

function main() {
  const reportPath = resolveReportOutputPath(DEFAULT_REPORT_FILE);
  const pageMenus = getPageMenuBlocks();
  const mismatches = collectMenuFieldStructureMismatches(pageMenus);
  const failCount = mismatches.length;

  const lines = [
    'Takt Menu 三字段结构对账',
    `Report: ${DEFAULT_REPORT_REL}`,
    `Generated: ${new Date().toISOString()}`,
    `Page menus scanned: ${pageMenus.length}`,
    '',
    `RESULT: ${failCount === 0 ? 'PASS' : 'FAIL'} (${failCount} menus)`,
    '',
    '规则（结构始终相同，仅分隔符与大小写不同）：',
    '  MenuCode   → 段间 _ ，全大写，如 HUMAN_RESOURCE_PERSONNEL_EMPLOYEE_CONTRACT',
    '  I18nKey    → menu. + 段间 . ，全小写，如 menu.human.resource.personnel.employee.contract',
    '  Permission → 段间 : ，全小写 + 末段操作，如 human:resource:personnel:employee:contract:list',
    '  权威分段：MenuCode 按 _ 拆分后转小写',
    '',
    `结构不一致: ${failCount}`,
  ];

  if (mismatches.length === 0) {
    lines.push('无');
  } else {
    for (const item of mismatches) {
      lines.push('');
      lines.push(`  [${item.block.menuCode}] ${item.block.menuName || ''} | ${item.block.sourceFile}:${item.block.line}`);
      lines.push(`    route: ${item.block.routePath}`);
      lines.push(`    期望 I18nKey:    menu.${item.menuSegments.join('.')}`);
      lines.push(`    实际 I18nKey:    ${item.block.i18nKey}`);
      lines.push(`    期望 Permission: ${item.menuSegments.join(':')}:list`);
      lines.push(`    实际 Permission: ${item.block.permission}`);
      for (const issue of item.issues) {
        lines.push(`    · ${issue}`);
      }
    }
  }

  console.log(`   页面菜单 ${pageMenus.length} | 结构不一致 ${failCount}`);
  finishAuditReport(reportPath, lines, failCount, 'node scripts/audit-menu-structure.cjs');
}

main();
