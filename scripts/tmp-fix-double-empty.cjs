// ========================================
// 临时：修复 = string.Empty; = string.Empty; 语法错误
// ========================================
'use strict';
const fs = require('fs');
const path = require('path');
const ROOT = path.resolve(__dirname, '..');
const dtoRoot = path.join(ROOT, 'backend/src/Takt.Application/Dtos');

function walk(dir, acc = []) {
  for (const e of fs.readdirSync(dir, { withFileTypes: true })) {
    const p = path.join(dir, e.name);
    if (e.isDirectory()) walk(p, acc);
    else if (e.name.endsWith('.cs')) acc.push(p);
  }
  return acc;
}

let n = 0;
for (const file of walk(dtoRoot)) {
  let t = fs.readFileSync(file, 'utf8');
  const o = t;
  t = t.replace(/= string\.Empty; = string\.Empty;/g, '= string.Empty;');
  if (t !== o) {
    fs.writeFileSync(file, t, 'utf8');
    n++;
    console.log('FIX', path.relative(ROOT, file));
  }
}
console.log('fixed_files', n);
