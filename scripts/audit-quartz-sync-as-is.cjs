'use strict';
const fs = require('fs');
const path = require('path');
const root = path.join(__dirname, '..', 'backend/src/Takt.WebApi/wwwroot/Quartz');
const exclude = new Set([
  'sync_st.sql', 'sync_mo.sql', 'sync_matplt.sql', 'sync_mdl.sql', 'sync_ec.sql',
  'sync_data_create_tables.sql',
  'sync_pup_bk.sql',
  'sync_sp_bk.sql',
  'sync_bv_bk.sql',
  'sync_bc_bk.sql',
]);
const inventRe = /N'CNY'|N'PC'|,\s*N'F'\)|N'101'|N'PR00'|N'PB00'|N'FOB'|N'150'|N'prepayship'|N'cod'|N'ROH'|N'HERS'|N'VERP'|N'KG'|N'M3'|N'CAR'|,\s*N'A'\)|COALESCE\(TRY_CAST\(R\.\[tax_rate\] AS INT\), 13\)|COALESCE\(TRY_CAST\(R\.\[(?:price_unit|sales_per_unit|purchase_per_unit)\] AS INT\), 1000\)|zh_desc|src_mat_desc|tgt_mat_desc|N'Product' AS/;

for (const f of fs.readdirSync(root).filter((x) => /^sync_.*\.sql$/i.test(x) && !exclude.has(x)).sort()) {
  const s = fs.readFileSync(path.join(root, f), 'utf8');
  const lines = s.split(/\n/);
  const hits = [];
  lines.forEach((line, i) => {
    if (inventRe.test(line) && !/N'SYNC'|SYSTEM_SYNC|QUARTZ_SYNC/.test(line)) {
      hits.push(`${i + 1}:${line.trim().slice(0, 120)}`);
    }
  });
  const itemOk = !/#item\b/.test(s) || /INSERT INTO #item\b/.test(s);
  console.log(`\n=== ${f} itemOk=${itemOk} hits=${hits.length}`);
  hits.slice(0, 20).forEach((h) => console.log(h));
}
