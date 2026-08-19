'use strict';
/**
 * 同结构 sync_*.sql：去掉发明默认码与他表描述回填；不做贪婪删除整段脚本。
 * 排除：st/mo/matplt/mdl/ec；不动 sync_data_create_tables / *_bk。
 * sync_bv 已手工完成，仍可再跑一遍安全替换。
 */
const fs = require('fs');
const path = require('path');

const root = path.join(__dirname, '..', 'backend/src/Takt.WebApi/wwwroot/Quartz');
const exclude = new Set([
  'sync_st.sql',
  'sync_mo.sql',
  'sync_matplt.sql',
  'sync_mdl.sql',
  'sync_ec.sql',
  'sync_data_create_tables.sql',
  'sync_pup_bk.sql',
  'sync_sp_bk.sql',
  'sync_bv_bk.sql',
  'sync_bc_bk.sql',
]);

/** @type {Array<[RegExp, string]>} */
const replacements = [
  [/ISNULL\(NULLIF\(LEFT\(LTRIM\(RTRIM\(R\.\[currency_code\]\)\), 3\), N''\), N'CNY'\)/g,
    "ISNULL(NULLIF(LEFT(LTRIM(RTRIM(R.[currency_code])), 3), N''), N'')"],
  [/ISNULL\(NULLIF\(LTRIM\(RTRIM\(R\.\[currency_code\]\)\), N''\), N'CNY'\)/g,
    "ISNULL(NULLIF(LTRIM(RTRIM(R.[currency_code])), N''), N'')"],
  [/ISNULL\(NULLIF\(LTRIM\(RTRIM\(R\.\[currency_code\]\)\), ''\), 'CNY'\)/g,
    "ISNULL(NULLIF(LTRIM(RTRIM(R.[currency_code])), ''), '')"],
  [/ISNULL\(NULLIF\(LEFT\(LTRIM\(RTRIM\(R\.\[sales_unit\]\)\), 5\), N''\), N'PC'\)/g,
    "ISNULL(NULLIF(LEFT(LTRIM(RTRIM(R.[sales_unit])), 5), N''), N'')"],
  [/ISNULL\(NULLIF\(LEFT\(LTRIM\(RTRIM\(R\.\[purchase_unit\]\)\), 20\), N''\), N'PC'\)/g,
    "ISNULL(NULLIF(LEFT(LTRIM(RTRIM(R.[purchase_unit])), 20), N''), N'')"],
  [/ISNULL\(NULLIF\(LTRIM\(RTRIM\(R\.\[unit_of_measure\]\)\), N''\), N'PC'\)/g,
    "ISNULL(NULLIF(LTRIM(RTRIM(R.[unit_of_measure])), N''), N'')"],
  [/ISNULL\(NULLIF\(LEFT\(LTRIM\(RTRIM\(R\.\[base_unit\]\)\), 3\), N''\), N'PC'\)/g,
    "ISNULL(NULLIF(LEFT(LTRIM(RTRIM(R.[base_unit])), 3), N''), N'')"],
  [/ISNULL\(NULLIF\(LTRIM\(RTRIM\(R\.\[base_unit\]\)\), N''\), N'PC'\)/g,
    "ISNULL(NULLIF(LTRIM(RTRIM(R.[base_unit])), N''), N'')"],
  [/ISNULL\(NULLIF\(LTRIM\(RTRIM\(R\.\[purchase_type\]\)\), N''\), N'F'\)/g,
    "ISNULL(NULLIF(LTRIM(RTRIM(R.[purchase_type])), N''), N'')"],
  [/ISNULL\(NULLIF\(LEFT\(LTRIM\(RTRIM\(R\.\[movement_type\]\)\), 3\), N''\), N'101'\)/g,
    "ISNULL(NULLIF(LEFT(LTRIM(RTRIM(R.[movement_type])), 3), N''), N'')"],
  [/ISNULL\(NULLIF\(LTRIM\(RTRIM\(R\.\[price_type\]\)\), N''\), N'PR00'\)/g,
    "ISNULL(NULLIF(LTRIM(RTRIM(R.[price_type])), N''), N'')"],
  [/ISNULL\(NULLIF\(LTRIM\(RTRIM\(R\.\[price_type\]\)\), N''\), N'PB00'\)/g,
    "ISNULL(NULLIF(LTRIM(RTRIM(R.[price_type])), N''), N'')"],
  [/ISNULL\(NULLIF\(LTRIM\(RTRIM\(R\.\[condition_currency_code\]\)\), N''\), N'CNY'\)/g,
    "ISNULL(NULLIF(LTRIM(RTRIM(R.[condition_currency_code])), N''), N'')"],
  [/ISNULL\(NULLIF\(LTRIM\(RTRIM\(R\.\[price_control\]\)\), ''\), 'V'\)/g,
    "ISNULL(NULLIF(LTRIM(RTRIM(R.[price_control])), ''), '')"],
  [/ISNULL\(NULLIF\(LTRIM\(RTRIM\(R\.\[price_control\]\)\), N''\), N'V'\)/g,
    "ISNULL(NULLIF(LTRIM(RTRIM(R.[price_control])), N''), N'')"],
  [/ISNULL\(NULLIF\(LEFT\(LTRIM\(RTRIM\(R\.\[material_type\]\)\), 4\), N''\), N'ROH'\)/g,
    "ISNULL(NULLIF(LEFT(LTRIM(RTRIM(R.[material_type])), 4), N''), N'')"],
  [/ISNULL\(NULLIF\(LTRIM\(RTRIM\(R\.\[material_type\]\)\), N''\), N'ROH'\)/g,
    "ISNULL(NULLIF(LTRIM(RTRIM(R.[material_type])), N''), N'')"],
  [/ISNULL\(NULLIF\(LEFT\(LTRIM\(RTRIM\(ISNULL\(R\.\[material_type\], N''\)\)\), 4\), N''\), N'HERS'\)/g,
    "ISNULL(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[material_type], N''))), 4), N''), N'')"],
  [/ISNULL\(NULLIF\(LTRIM\(RTRIM\(R\.\[enterprise_nature\]\)\), N''\), N'150'\)/g,
    "ISNULL(NULLIF(LTRIM(RTRIM(R.[enterprise_nature])), N''), N'')"],
  [/ISNULL\(NULLIF\(LTRIM\(RTRIM\(R\.\[industry_attribute\]\)\), N''\), N'C'\)/g,
    "ISNULL(NULLIF(LTRIM(RTRIM(R.[industry_attribute])), N''), N'')"],
  [/ISNULL\(NULLIF\(LTRIM\(RTRIM\(R\.\[payment_terms\]\)\), N''\), N'prepayship'\)/g,
    "ISNULL(NULLIF(LTRIM(RTRIM(R.[payment_terms])), N''), N'')"],
  [/ISNULL\(NULLIF\(LTRIM\(RTRIM\(R\.\[payment_terms\]\)\), N''\), N'cod'\)/g,
    "ISNULL(NULLIF(LTRIM(RTRIM(R.[payment_terms])), N''), N'')"],
  [/ISNULL\(NULLIF\(LTRIM\(RTRIM\(R\.\[incoterms1\]\)\), N''\), N'FOB'\)/g,
    "ISNULL(NULLIF(LTRIM(RTRIM(R.[incoterms1])), N''), N'')"],
  [/ISNULL\(NULLIF\(LEFT\(LTRIM\(RTRIM\(ISNULL\(R\.\[weight_unit\], N''\)\)\), 10\), N''\), N'KG'\)/g,
    "ISNULL(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[weight_unit], N''))), 10), N''), N'')"],
  [/ISNULL\(NULLIF\(LEFT\(LTRIM\(RTRIM\(ISNULL\(R\.\[volume_unit\], N''\)\)\), 10\), N''\), N'M3'\)/g,
    "ISNULL(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[volume_unit], N''))), 10), N''), N'')"],
  [/ISNULL\(NULLIF\(LEFT\(LTRIM\(RTRIM\(ISNULL\(R\.\[packaging_type\], N''\)\)\), 40\), N''\), N'VERP'\)/g,
    "ISNULL(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[packaging_type], N''))), 40), N''), N'')"],
  [/ISNULL\(NULLIF\(LEFT\(LTRIM\(RTRIM\(ISNULL\(R\.\[packing_unit\], N''\)\)\), 20\), N''\), N'CAR'\)/g,
    "ISNULL(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[packing_unit], N''))), 20), N''), N'')"],
  [/LEFT\(ISNULL\(NULLIF\(LTRIM\(RTRIM\(R\.\[plan_by\]\)\), N''\), N'SYNC'\), 50\)/g,
    "LEFT(ISNULL(NULLIF(LTRIM(RTRIM(R.[plan_by])), N''), N''), 50)"],
  [/N'Product' AS \[sales_product\]/g,
    "ISNULL(NULLIF(LTRIM(RTRIM(R.[sales_product])), N''), N'') AS [sales_product]"],
  [/COALESCE\(TRY_CAST\(R\.\[tax_rate\] AS INT\), 13\)/g,
    'COALESCE(TRY_CAST(R.[tax_rate] AS INT), 0)'],
  [/COALESCE\(TRY_CAST\(R\.\[price_unit\] AS INT\), 1000\)/g,
    'COALESCE(TRY_CAST(R.[price_unit] AS INT), 0)'],
  [/COALESCE\(TRY_CAST\(R\.\[sales_per_unit\] AS INT\), 1000\)/g,
    'COALESCE(TRY_CAST(R.[sales_per_unit] AS INT), 0)'],
  [/COALESCE\(TRY_CAST\(R\.\[purchase_per_unit\] AS INT\), 1000\)/g,
    'COALESCE(TRY_CAST(R.[purchase_per_unit] AS INT), 0)'],
  [/COALESCE\(TRY_CAST\(R\.\[moving_price_unit\] AS INT\), 1\)/g,
    'COALESCE(TRY_CAST(R.[moving_price_unit] AS INT), 0)'],
  [/COALESCE\(TRY_CAST\(R\.\[purchase_price_unit\] AS INT\), 1\)/g,
    'COALESCE(TRY_CAST(R.[purchase_price_unit] AS INT), 0)'],
  [/ISNULL\(NULLIF\(LTRIM\(RTRIM\(R\.\[customer_pricing_procedure\]\)\), N''\), N'1'\)/g,
    "ISNULL(NULLIF(LTRIM(RTRIM(R.[customer_pricing_procedure])), N''), N'')"],
  [/ISNULL\(NULLIF\(LTRIM\(RTRIM\(R\.\[shipping_conditions\]\)\), N''\), N'01'\)/g,
    "ISNULL(NULLIF(LTRIM(RTRIM(R.[shipping_conditions])), N''), N'')"],
];

/**
 * 仅删除「空 material_description ← 物料描述表」的 WITH…UPDATE 块（src_mat_desc / tgt_mat_desc）。
 * @param {string} sql
 * @returns {string}
 */
function stripMatDescBackfill(sql) {
  // 非贪婪：从 ;WITH src_mat_desc|tgt_mat_desc 到对应 UPDATE 语句结束的分号
  return sql.replace(
    /;WITH\s+(src_mat_desc|tgt_mat_desc)\s+AS\s*\([\s\S]*?\)\s*UPDATE\s+\w+\s+SET[\s\S]*?COALESCE\(m\.\[zh_desc\][\s\S]*?;\s*/gi,
    '',
  );
}

/**
 * 删除文件头关于描述回填的注释行。
 * @param {string} sql
 * @returns {string}
 */
function stripDescComments(sql) {
  return sql
    .replace(/\r?\n--\s*空 material_description：[^\r\n]*/g, '')
    .replace(/\r?\n--\s*源库回填：空 material_description[^\r\n]*/g, '')
    .replace(/\r?\n--\s*目标库回填[：:]?[^\r\n]*material_description[^\r\n]*/g, '')
    .replace(/\r?\n--\s*目标库回填：空 material_description[^\r\n]*/g, '')
    .replace(/\r?\n{3,}/g, '\n\n');
}

const report = [];
for (const file of fs.readdirSync(root).filter((f) => /^sync_.*\.sql$/i.test(f) && !exclude.has(f)).sort()) {
  const fp = path.join(root, file);
  let sql = fs.readFileSync(fp, 'utf8');
  const before = sql;
  const beforeLen = sql.length;
  for (const [re, to] of replacements) sql = sql.replace(re, to);
  sql = stripMatDescBackfill(sql);
  sql = stripDescComments(sql);
  if (sql !== before) {
    // 安全阀：删除后不得短于 70%（防止再误删主逻辑）
    if (sql.length < beforeLen * 0.7) {
      report.push(`ABORT ${file}: ${beforeLen} -> ${sql.length}`);
      continue;
    }
    fs.writeFileSync(fp, sql, 'utf8');
    report.push(`UPDATED ${file}: ${beforeLen} -> ${sql.length}`);
  } else {
    report.push(`UNCHANGED ${file}`);
  }
}

console.log(report.join('\n'));
