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
const targetLine = Number(process.argv[2] || 83);
const rl = require('readline').createInterface({
  input: fs.createReadStream(p),
  crlfDelay: Infinity,
});
let n = 0;
rl.on('line', (l) => {
  n++;
  if (n !== targetLine) return;
  const o = JSON.parse(l);
  let i = 0;
  for (const c of o.message.content || []) {
    if (c.type !== 'tool_use' || c.name !== 'StrReplace') continue;
    const filePath = c.input?.path || '';
    if (!filePath.includes('TaktBomMaterialCostService.cs')) continue;
    i++;
    fs.writeFileSync(
      path.join(outDir, `line${targetLine}-patch${i}-new.cs`),
      c.input.new_string || '',
      'utf8'
    );
    fs.writeFileSync(
      path.join(outDir, `line${targetLine}-patch${i}-old.cs`),
      c.input.old_string || '',
      'utf8'
    );
    console.log('wrote patch', i, 'newLen', (c.input.new_string || '').length);
  }
});
rl.on('close', () => console.log('done'));
