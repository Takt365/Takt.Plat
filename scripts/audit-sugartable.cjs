// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：scripts
// 文件名称：audit-sugartable.cjs
// 功能描述：扫描 Takt.Domain 全部 [SugarTable] 命名合规性
// ========================================

const fs = require('fs');
const path = require('path');

const DOMAIN_ROOT = path.resolve(__dirname, '../backend/src/Takt.Domain');
const TABLE_RE = /\[SugarTable\("([^"]+)"/g;
const CLASS_RE = /public class (Takt\w+)/;

/** @type {Array<{ file: string, cls: string, table: string }>} */
const items = [];

function walk(dir) {
  for (const entry of fs.readdirSync(dir, { withFileTypes: true })) {
    const full = path.join(dir, entry.name);
    if (entry.isDirectory()) {
      walk(full);
      continue;
    }
    if (!entry.name.endsWith('.cs')) {
      continue;
    }
    const content = fs.readFileSync(full, 'utf8');
    const cls = (content.match(CLASS_RE) || [])[1] || '?';
    let match = TABLE_RE.exec(content);
    while (match) {
      items.push({
        file: full.replace(/\\/g, '/'),
        cls,
        table: match[1],
      });
      match = TABLE_RE.exec(content);
    }
  }
}

walk(DOMAIN_ROOT);

/** @type {Array<{ type: string, file: string, cls: string, table: string }>} */
const violations = [];
const byTable = new Map();

for (const item of items) {
  const { table } = item;
  if (!table.startsWith('takt_')) {
    violations.push({ type: 'no_takt_prefix', ...item });
  }
  if (/[A-Z]/.test(table)) {
    violations.push({ type: 'uppercase', ...item });
  }
  if (/[^a-z0-9_]/.test(table)) {
    violations.push({ type: 'invalid_char', ...item });
  }
  if (/__/.test(table)) {
    violations.push({ type: 'double_underscore', ...item });
  }
  if (/_$/.test(table)) {
    violations.push({ type: 'trailing_underscore', ...item });
  }
  if (!byTable.has(table)) {
    byTable.set(table, []);
  }
  byTable.get(table).push(item.cls);
}

const duplicates = [...byTable.entries()].filter(([, clsList]) => clsList.length > 1);

console.log(`扫描实体: ${items.length} 个 [SugarTable]`);
console.log(`格式违规: ${violations.length} 个`);
console.log(`表名重复: ${duplicates.length} 个`);

if (violations.length) {
  console.log('\n--- 格式违规 ---');
  for (const v of violations) {
    const rel = v.file.split('/Takt.Domain/')[1] || v.file;
    console.log(`[${v.type}] ${v.cls} -> ${v.table} (${rel})`);
  }
}

if (duplicates.length) {
  console.log('\n--- 表名重复 ---');
  for (const [table, clsList] of duplicates) {
    console.log(`${table} -> ${clsList.join(', ')}`);
  }
}

if (!violations.length && !duplicates.length) {
  console.log('\n✅ 全部表名符合：takt_ 前缀 + 全小写 + 下划线分隔，且无重复。');
}

process.exit(violations.length || duplicates.length ? 1 : 0);
