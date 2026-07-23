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
console.log('lines', lines.length);
for (let i = 0; i < lines.length; i++) {
  const l = lines[i];
  let o;
  try {
    o = JSON.parse(l);
  } catch {
    continue;
  }
  const parts = o?.message?.content;
  if (!Array.isArray(parts)) {
    const text = o?.message?.content;
    if (typeof text === 'string' && text.includes('BomMaterialCost')) {
      console.log('line', i + 1, 'role', o.role, 'textLen', text.length, text.slice(0, 120).replace(/\n/g, ' '));
    }
    continue;
  }
  for (const c of parts) {
    if (c.type === 'text' && typeof c.text === 'string' && c.text.includes('generate-all')) {
      console.log('line', i + 1, 'text mention generate-all', c.text.slice(0, 200).replace(/\n/g, ' '));
    }
    if (c.type !== 'tool_use') continue;
    const fp = c.input?.path || c.input?.command || '';
    if (
      String(fp).includes('BomMaterialCost') ||
      String(fp).includes('generate-all') ||
      String(c.input?.command || '').includes('generate-all')
    ) {
      console.log(
        'line',
        i + 1,
        c.name,
        String(fp).slice(-80),
        'new',
        (c.input?.new_string || '').length,
        'contents',
        (c.input?.contents || '').length
      );
    }
  }
}
