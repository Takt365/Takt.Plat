/**
 * 对比 Application Service 中 isUnique_ix_* 与实体 SugarIndex 是否一致
 */
const fs = require('fs');
const path = require('path');

const svcRoot = path.join(__dirname, '../backend/src/Takt.Application/Services');
const entRoot = path.join(__dirname, '../backend/src/Takt.Domain/Entities');
const dirs = ['HumanResource', 'Identity', 'Logistics', 'Routine', 'Statistics'];

/** @param {string} dir @returns {string[]} */
function walk(dir, acc = []) {
  for (const f of fs.readdirSync(dir)) {
    const p = path.join(dir, f);
    if (fs.statSync(p).isDirectory()) {
      walk(p, acc);
    } else if (f.endsWith('.cs')) {
      acc.push(p);
    }
  }
  return acc;
}

/** @param {string} serviceClass */
function toEntityName(serviceClass) {
  return serviceClass.replace(/Service$/, '');
}

const entityFiles = new Map();
for (const sd of dirs) {
  const root = path.join(entRoot, sd);
  if (!fs.existsSync(root)) {
    continue;
  }
  for (const ef of walk(root)) {
    entityFiles.set(path.basename(ef, '.cs'), ef);
  }
}

const missing = [];
for (const sd of dirs) {
  const root = path.join(svcRoot, sd);
  if (!fs.existsSync(root)) {
    continue;
  }
  for (const sf of walk(root)) {
    if (!sf.endsWith('Service.cs') || sf.includes('TaktServiceBase')) {
      continue;
    }
    const sc = fs.readFileSync(sf, 'utf8');
    const cm = sc.match(/class\s+(Takt\w+Service)\b/);
    if (!cm) {
      continue;
    }
    const entity = toEntityName(cm[1]);
    const re = /isUnique_(ix_[a-zA-Z0-9_]+)\s*=\s*await[\s\S]*?x\s*=>\s*([^;]+);/g;
    let rm;
    while ((rm = re.exec(sc)) !== null) {
      const idx = rm[1];
      const expr = rm[2];
      const fields = [...expr.matchAll(/x\.(\w+)\s*==/g)].map((x) => x[1]);
      const entFile = entityFiles.get(entity);
      if (!entFile) {
        missing.push({ entity, idx, fields, reason: 'entity not found' });
        continue;
      }
      const ec = fs.readFileSync(entFile, 'utf8');
      if (!ec.includes(`"${idx}"`)) {
        missing.push({ entity, idx, fields, file: path.relative(entRoot, entFile) });
      }
    }
  }
}

console.log(`Missing unique indexes: ${missing.length}`);
for (const m of missing) {
  console.log(`${m.entity}\t${m.idx}\t${m.fields.join(',')}\t${m.file || m.reason || ''}`);
}
