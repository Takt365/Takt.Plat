'use strict';
const fs = require('fs');
const path = require('path');
const root =
  'C:/Users/Davis.Cheng/.cursor/projects/g-AppDevelop-VS2026-Takt-Plat/agent-transcripts';
function walk(d, acc = []) {
  for (const e of fs.readdirSync(d, { withFileTypes: true })) {
    const p = path.join(d, e.name);
    if (e.isDirectory()) walk(p, acc);
    else if (e.name.endsWith('.jsonl')) acc.push(p);
  }
  return acc;
}
const needles = [
  'PrepareRecalculateModelAverageQuery',
  'LoadAnalysisRowsAsync',
  'class TaktBomMaterialCostService',
];
for (const f of walk(root)) {
  const data = fs.readFileSync(f, 'utf8').split(/\n/);
  for (let i = 0; i < data.length; i++) {
    const l = data[i];
    if (l.length < 80000) continue;
    if (!needles.every((n) => l.includes(n))) continue;
    let role = '?';
    try {
      role = JSON.parse(l).role;
    } catch {
      /* ignore */
    }
    console.log(
      path.basename(path.dirname(f)),
      'line',
      i + 1,
      'role',
      role,
      'len',
      l.length
    );
  }
}
console.log('scan done files', walk(root).length);
