'use strict';
const fs = require('fs');
const path = require('path');
const p = path.join(
  'C:/Users/Davis.Cheng/.cursor/projects/g-AppDevelop-VS2026-Takt-Plat/agent-transcripts',
  '8fc3cf57-89cd-4355-9b2b-9a0f70154ca6',
  '8fc3cf57-89cd-4355-9b2b-9a0f70154ca6.jsonl'
);
const outDir = path.join(__dirname, '_tmp-recover-bom');
fs.mkdirSync(outDir, { recursive: true });
const lines = fs.readFileSync(p, 'utf8').split(/\n/).filter(Boolean);
for (let i = 0; i < lines.length; i++) {
  const o = JSON.parse(lines[i]);
  for (const c of o?.message?.content || []) {
    if (c.type !== 'tool_use' || c.name !== 'StrReplace') continue;
    const fp = c.input?.path || '';
    if (!fp.includes('TaktBomMaterialCostService.cs') && !fp.includes('ITaktBomMaterialCostService.cs'))
      continue;
    const base = path.basename(fp).replace('.cs', '');
    fs.writeFileSync(
      path.join(outDir, `8fc3-L${i + 1}-${base}-old.txt`),
      c.input.old_string || '',
      'utf8'
    );
    fs.writeFileSync(
      path.join(outDir, `8fc3-L${i + 1}-${base}-new.txt`),
      c.input.new_string || '',
      'utf8'
    );
    console.log(
      'L',
      i + 1,
      base,
      'old',
      (c.input.old_string || '').length,
      'new',
      (c.input.new_string || '').length
    );
  }
}
