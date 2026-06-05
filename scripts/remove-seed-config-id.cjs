/**
 * 一次性：清理 Seeds 目录中过时的 configId 文档与签名残留
 * 用法：node scripts/remove-seed-config-id.cjs
 */
const fs = require('fs');
const path = require('path');

const seedsRoot = path.join(__dirname, '../backend/src/Takt.Infrastructure/Data/Seeds');

function patchFile(filePath) {
  let text = fs.readFileSync(filePath, 'utf8');
  const next = text
    .replace(
      /SeedAsync\(\s*\r?\n\s*IServiceProvider serviceProvider,\s*\r?\n\s*string configId,\s*\r?\n\s*string\?/g,
      'SeedAsync(\n        IServiceProvider serviceProvider,\n        string?')
    .replace(
      /SeedAsync\(IServiceProvider serviceProvider, string configId, string\?/g,
      'SeedAsync(IServiceProvider serviceProvider, string?')
    .replace(/SeedAsync\(serviceProvider, configId,/g, 'SeedAsync(serviceProvider,')
    .replace(/\r?\n\s*\/\/\/ <param name="configId">[^\r\n]*/g, '');
  if (next !== text) {
    fs.writeFileSync(filePath, next, 'utf8');
    return true;
  }
  return false;
}

function walk(dir) {
  let count = 0;
  for (const name of fs.readdirSync(dir, { withFileTypes: true })) {
    const full = path.join(dir, name.name);
    if (name.isDirectory()) {
      count += walk(full);
    } else if (name.name.endsWith('.cs')) {
      if (patchFile(full)) {
        count += 1;
      }
    }
  }
  return count;
}

const updated = walk(seedsRoot);
console.log(`updated ${updated} files under Seeds`);
