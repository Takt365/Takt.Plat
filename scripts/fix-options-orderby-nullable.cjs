/**
 * 修复 Get*OptionsAsync 中 orderBy 表达式 CS8603（nullable string 排序键）
 */
const fs = require('fs');
const path = require('path');

const servicesRoot = path.join(__dirname, '..', 'backend', 'src', 'Takt.Application', 'Services');

function walk(dir, files = []) {
  for (const name of fs.readdirSync(dir)) {
    const full = path.join(dir, name);
    const stat = fs.statSync(full);
    if (stat.isDirectory()) walk(full, files);
    else if (name.endsWith('Service.cs')) files.push(full);
  }
  return files;
}

const optionsMethodRe = /public async Task<List<Takt(?:Select|TreeSelect)Option>> Get\w+OptionsAsync\(\)[\s\S]*?\n    \}/g;
const orderByRe = /(x => x\.(\w+))(,(\r?\n\s*false\)))/g;

let changedFiles = 0;
let changedCount = 0;

for (const file of walk(servicesRoot)) {
  let content = fs.readFileSync(file, 'utf8');
  let fileChanged = false;
  const next = content.replace(optionsMethodRe, (methodBlock) =>
    methodBlock.replace(orderByRe, (full, expr, field, tail) => {
      if (expr.includes('??')) return full;
      if (/^(Id|SortOrder|LineNumber|CreatedAt|UpdatedAt)$/.test(field)) return full;
      changedCount += 1;
      fileChanged = true;
      return `${expr} ?? string.Empty${tail}`;
    }),
  );
  if (fileChanged) {
    fs.writeFileSync(file, next, 'utf8');
    changedFiles += 1;
    console.log('fixed:', path.relative(servicesRoot, file));
  }
}

console.log(`done: ${changedFiles} files, ${changedCount} orderBy expressions`);
