'use strict';
const fs = require('fs');
const path = require('path');
const p = path.join(
  'C:/Users/Davis.Cheng/.cursor/projects/g-AppDevelop-VS2026-Takt-Plat/agent-transcripts',
  'cf488e1d-42b3-4264-aed9-740179802524',
  'cf488e1d-42b3-4264-aed9-740179802524.jsonl'
);
const outDir = path.join(__dirname, '_tmp-recover-bom');
const needle = 'class TaktBomMaterialCostTransposedDto';
const rl = require('readline').createInterface({
  input: fs.createReadStream(p),
  crlfDelay: Infinity,
});
let n = 0;
rl.on('line', (l) => {
  n++;
  if (!l.includes(needle)) return;
  try {
    const o = JSON.parse(l);
    for (const c of o.message?.content || []) {
      if (c.type !== 'tool_use') continue;
      const blob = c.input?.contents || c.input?.new_string || '';
      if (!blob.includes(needle)) continue;
      const start = blob.indexOf('// ========================================\r\n// 转置') >= 0
        ? blob.indexOf('// ========================================\r\n// 转置')
        : blob.indexOf('public class TaktBomMaterialCostTransposedQueryDto');
      // dump from first analysis class-ish
      let idx = blob.indexOf('public class TaktBomMaterialCostTransposedQueryDto');
      if (idx < 0) idx = blob.indexOf(needle);
      const chunk = blob.slice(Math.max(0, idx - 200), Math.min(blob.length, idx + 25000));
      const dest = path.join(outDir, `cf488-L${n}-analysis-dtos-chunk.txt`);
      fs.writeFileSync(dest, chunk, 'utf8');
      console.log('wrote', dest, 'from', c.name, 'blobLen', blob.length, 'chunk', chunk.length);
    }
  } catch (e) {
    /* ignore */
  }
});
rl.on('close', () => console.log('done', n));
