'use strict';
const fs = require('fs');
const t = fs.readFileSync(
  'g:/AppDevelop/VS2026/Takt.Plat/backend/src/Takt.Application/Services/Logistics/Manufacturing/Bom/TaktBomMaterialCostItemService.cs',
  'utf8',
);
const markers = [
  '    // ========================================\n    // 转置 / 差异 / 月度涨跌分析',
  '    // 转置 / 差异 / 月度涨跌分析',
  'GetBomMaterialCostItemZeroMovingPriceMergedAsync',
  '    /// <summary>\n    /// 加载转置/月度涨跌用成本汇总行',
  '加载转置/月度涨跌用成本汇总行',
  'PrepareZeroMovingPriceCostingMonth',
  '转置行全量合计',
];
for (const m of markers) {
  console.log(JSON.stringify(m.slice(0, 40)), t.indexOf(m));
}
let i = 0;
while ((i = t.indexOf('查询表达式', i)) >= 0) {
  console.log('qe', i, JSON.stringify(t.slice(i - 40, i + 20)));
  i++;
}
