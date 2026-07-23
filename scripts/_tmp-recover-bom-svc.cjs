'use strict';
const fs = require('fs');
const path = require('path');
const p = path.join(
  'C:/Users/Davis.Cheng/.cursor/projects/g-AppDevelop-VS2026-Takt-Plat/agent-transcripts',
  'cf488e1d-42b3-4264-aed9-740179802524',
  'cf488e1d-42b3-4264-aed9-740179802524.jsonl'
);
const outDir = path.join(__dirname, '_tmp-recover-bom');
fs.mkdirSync(outDir, { recursive: true });
const rl = require('readline').createInterface({
  input: fs.createReadStream(p),
  crlfDelay: Infinity,
});
let n = 0;
const writes = [];
const largePatches = [];
rl.on('line', (l) => {
  n++;
  if (!l.includes('BomMaterialCost')) return;
  let o;
  try {
    o = JSON.parse(l);
  } catch {
    return;
  }
  const parts = o?.message?.content;
  if (!Array.isArray(parts)) return;
  for (const c of parts) {
    if (c.type !== 'tool_use') continue;
    const filePath = c.input?.path || '';
    if (!/BomMaterialCost(Item)?(Service|Controller|Dtos)/.test(filePath)) continue;
    if (c.name === 'Write' && c.input?.contents) {
      const base = path.basename(filePath);
      const dest = path.join(outDir, `${n}-${base}`);
      fs.writeFileSync(dest, c.input.contents, 'utf8');
      writes.push({ n, dest, len: c.input.contents.length });
    }
    if (c.name === 'StrReplace' && (c.input?.new_string || '').length > 8000) {
      largePatches.push({
        n,
        file: path.basename(filePath),
        newLen: c.input.new_string.length,
        hasPrepare: c.input.new_string.includes('PrepareRecalculate'),
        hasTransposed: c.input.new_string.includes('Transposed'),
        hasBackfill: c.input.new_string.includes('Backfill'),
      });
    }
  }
});
rl.on('close', () => {
  console.log(JSON.stringify({ lines: n, writes, largePatches }, null, 2));
});
