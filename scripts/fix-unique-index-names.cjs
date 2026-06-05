/**
 * 批量修正实体 SugarIndex：唯一索引（末位 true）名称必须以 _unique 结尾
 * 排除：Statistics/Logging 日志实体、*ChangeLog* 变更日志实体
 */
const fs = require('fs');
const path = require('path');

const ENTITIES_ROOT = path.join(__dirname, '../backend/src/Takt.Domain/Entities');

/** @returns {boolean} */
function isLogEntity(filePath) {
  const normalized = filePath.replace(/\\/g, '/');
  if (normalized.includes('/Statistics/Logging/')) {
    return true;
  }
  if (/ChangeLog\.cs$/i.test(normalized)) {
    return true;
  }
  return false;
}

/** @returns {string[]} */
function walkCsFiles(dir, acc = []) {
  for (const entry of fs.readdirSync(dir, { withFileTypes: true })) {
    const full = path.join(dir, entry.name);
    if (entry.isDirectory()) {
      walkCsFiles(full, acc);
    } else if (entry.name.endsWith('.cs')) {
      acc.push(full);
    }
  }
  return acc;
}

/**
 * @param {string} line
 * @returns {{ line: string, change: { oldName: string, newName: string } | null }}
 */
function fixUniqueIndexLine(line) {
  const trimmed = line.trim();
  if (!trimmed.startsWith('[SugarIndex(') || !trimmed.endsWith(')]')) {
    return { line, change: null };
  }
  if (!/,\s*true\s*\)\]$/.test(trimmed)) {
    return { line, change: null };
  }
  const nameMatch = trimmed.match(/^\[SugarIndex\("([^"]+)"/);
  if (!nameMatch) {
    return { line, change: null };
  }
  const indexName = nameMatch[1];
  if (indexName.endsWith('_unique')) {
    return { line, change: null };
  }
  const newName = `${indexName}_unique`;
  const newLine = line.replace(`"${indexName}"`, `"${newName}"`);
  return { line: newLine, change: { oldName: indexName, newName } };
}

/**
 * @param {string} content
 * @returns {{ content: string, changes: Array<{ oldName: string, newName: string }> }}
 */
function fixUniqueIndexNames(content) {
  const changes = [];
  const lines = content.split('\n');
  const newLines = lines.map((line) => {
    const { line: newLine, change } = fixUniqueIndexLine(line);
    if (change) {
      changes.push(change);
    }
    return newLine;
  });
  return { content: newLines.join('\n'), changes };
}

function main() {
  const dryRun = process.argv.includes('--dry-run');
  const files = walkCsFiles(ENTITIES_ROOT).filter((f) => !isLogEntity(f));
  let fileCount = 0;
  let changeCount = 0;
  for (const file of files) {
    const original = fs.readFileSync(file, 'utf8');
    const { content, changes } = fixUniqueIndexNames(original);
    if (changes.length === 0) {
      continue;
    }
    fileCount += 1;
    changeCount += changes.length;
    const rel = path.relative(ENTITIES_ROOT, file).replace(/\\/g, '/');
    console.log(`\n${rel}`);
    for (const c of changes) {
      console.log(`  ${c.oldName} -> ${c.newName}`);
    }
    if (!dryRun) {
      fs.writeFileSync(file, content, 'utf8');
    }
  }
  console.log(`\n${dryRun ? '[dry-run] ' : ''}共 ${fileCount} 个文件、${changeCount} 处唯一索引已${dryRun ? '待' : ''}修正`);
}

main();
