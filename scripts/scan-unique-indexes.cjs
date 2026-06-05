/**
 * 扫描非日志实体：缺唯一索引（末位 true）或唯一索引名未以 _unique 结尾
 */
const fs = require('fs');
const path = require('path');

const ENTITIES_ROOT = path.join(__dirname, '../backend/src/Takt.Domain/Entities');

function isLogEntity(filePath) {
  const normalized = filePath.replace(/\\/g, '/');
  return normalized.includes('/Statistics/Logging/') || /ChangeLog\.cs$/i.test(normalized);
}

function walkCsFiles(dir, acc = []) {
  for (const entry of fs.readdirSync(dir, { withFileTypes: true })) {
    const full = path.join(dir, entry.name);
    if (entry.isDirectory()) walkCsFiles(full, acc);
    else if (entry.name.endsWith('.cs')) acc.push(full);
  }
  return acc;
}

function analyzeFile(file) {
  const content = fs.readFileSync(file, 'utf8');
  const lines = content.split('\n');
  const unique = [];
  const badNames = [];
  let hasAnyIndex = false;
  for (const line of lines) {
    const t = line.trim();
    if (!t.startsWith('[SugarIndex(')) continue;
    hasAnyIndex = true;
    if (!/,\s*true\s*\)\]$/.test(t)) continue;
    const m = t.match(/^\[SugarIndex\("([^"]+)"/);
    if (!m) continue;
    unique.push(m[1]);
    if (!m[1].endsWith('_unique')) badNames.push(m[1]);
  }
  return { hasAnyIndex, unique, badNames };
}

const files = walkCsFiles(ENTITIES_ROOT).filter((f) => !isLogEntity(f));
const noUnique = [];
for (const file of files) {
  const rel = path.relative(ENTITIES_ROOT, file).replace(/\\/g, '/');
  const { hasAnyIndex, unique, badNames } = analyzeFile(file);
  if (unique.length === 0) noUnique.push({ rel, hasAnyIndex });
  if (badNames.length > 0) {
    console.log(`${rel}: bad names -> ${badNames.join(', ')}`);
  }
}
console.log('\n--- 无唯一索引的非日志实体 ---');
noUnique.forEach((x) => console.log(`${x.rel} (hasIndex=${x.hasAnyIndex})`));
