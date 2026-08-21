'use strict';
const fs = require('fs');
const path = require('path');
const dir = path.join(__dirname, '../backend/src/Takt.WebApi/wwwroot/Quartz');
const files = fs
  .readdirSync(dir)
  .filter((f) => /^sync_.*\.sql$/i.test(f) && !/_bk/i.test(f) && !f.includes('create_tables'))
  .sort();

for (const f of files) {
  const c = fs.readFileSync(path.join(dir, f), 'utf8');
  const merges = [];
  const re =
    /MERGE(?:\s+INTO)?\s+(?:\[dbo\]\.\s*)?\[?([\w_]+)\]?\s+AS\s+T[\s\S]*?\nON\s+([\s\S]*?)(?=\nWHEN\s)/gi;
  let m;
  while ((m = re.exec(c))) {
    const table = m[1];
    const on = m[2];
    const cols = [];
    const seen = new Set();
    const colRe = /T\.\[([^\]]+)\]/g;
    let cm;
    while ((cm = colRe.exec(on))) {
      if (!seen.has(cm[1])) {
        seen.add(cm[1]);
        cols.push(cm[1]);
      }
    }
    merges.push({ table, cols });
  }
  console.log('### ' + f);
  if (!merges.length) console.log('(no MERGE)');
  for (const x of merges) {
    console.log('  ' + x.table + ' => ' + (x.cols.length ? x.cols.join(' + ') : '(parse miss)'));
  }
  console.log('');
}
