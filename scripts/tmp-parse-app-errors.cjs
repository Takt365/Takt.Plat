// ========================================
// 临时：解析 Application 编译错误中的缺失属性
// ========================================
'use strict';
const fs = require('fs');
const lines = fs.readFileSync('backend/artifacts/_app-errors.txt', 'utf8').split(/\r?\n/).filter(Boolean);
const missing = [];
const typeMismatch = [];
for (const line of lines) {
  let m = line.match(/\u201C([^\u201D]+)\u201D\u672A\u5305\u542B\u201C([^\u201D]+)\u201D\u7684\u5B9A\u4E49/);
  if (m) {
    missing.push(`${m[1]}|${m[2]}`);
    continue;
  }
  // ASCII quotes fallback
  m = line.match(/"([^"]+)"\u672A\u5305\u542B"([^"]+)"\u7684\u5B9A\u4E49/);
  if (m) {
    missing.push(`${m[1]}|${m[2]}`);
    continue;
  }
  m = line.match(/\u672A\u5305\u542B\u201C([^\u201D]+)\u201D\u7684\u5B9A\u4E49/);
  if (m) {
    const typeM = line.match(/\u201C([^\u201D]+)\u201D\u672A\u5305\u542B/);
    if (typeM) missing.push(`${typeM[1]}|${m[1]}`);
    continue;
  }
  if (line.includes('CS0029') || line.includes('CS0019')) typeMismatch.push(line);
}
const uniq = [...new Set(missing)].sort();
fs.writeFileSync('backend/artifacts/_missing-props.txt', uniq.join('\n') + '\n');
console.log('missing_props', uniq.length);
console.log(uniq.slice(0, 80).join('\n'));
console.log('---mismatch---', typeMismatch.length);
