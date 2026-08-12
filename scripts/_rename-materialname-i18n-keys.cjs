'use strict';

const fs = require('fs');
const path = require('path');

const ROOT = path.resolve(__dirname, '..');
const TARGET_DIRS = [
  path.join(ROOT, 'backend/src'),
  path.join(ROOT, 'frontend/src'),
];

const SKIP = new Set(['node_modules', 'bin', 'obj', 'dist', 'artifacts', '.git']);
const EXT_OK = new Set(['.cs', '.ts', '.vue', '.tsx', '.js', '.cjs', '.json']);

const RULES = [
  [/parentmaterialname/g, 'parentmaterialdescription'],
  [/manufacturermaterialname/g, 'manufacturermaterialdescription'],
  [/componentmaterialname/g, 'componentmaterialdescription'],
  // bare materialname key segment — after prefixed
  [/\.materialname\b/g, '.materialdescription'],
  [/entity\.([a-z0-9]+)\.materialname\b/g, 'entity.$1.materialdescription'],
  [/'([a-z0-9.]*?)materialname'/g, "'$1materialdescription'"],
  [/"([a-z0-9.]*?)materialname"/g, '"$1materialdescription"'],
  [/materialname/g, 'materialdescription'],
];

function walk(dir, onFile) {
  if (!fs.existsSync(dir)) return;
  for (const e of fs.readdirSync(dir, { withFileTypes: true })) {
    if (e.name.startsWith('.')) continue;
    const full = path.join(dir, e.name);
    if (e.isDirectory()) {
      if (SKIP.has(e.name)) continue;
      walk(full, onFile);
      continue;
    }
    if (!EXT_OK.has(path.extname(e.name).toLowerCase())) continue;
    onFile(full);
  }
}

let n = 0;
for (const dir of TARGET_DIRS) {
  walk(dir, (file) => {
    let s = fs.readFileSync(file, 'utf8');
    if (!/materialname/i.test(s)) return;
    const before = s;
    for (const [re, repl] of RULES) s = s.replace(re, repl);
    // undo accidental double: materialdescriptiondescription
    s = s.replace(/materialdescriptiondescription/g, 'materialdescription');
    s = s.replace(/MaterialDescriptionDescription/g, 'MaterialDescription');
    if (s !== before) {
      fs.writeFileSync(file, s);
      n += 1;
    }
  });
}
console.log('i18n key files changed', n);
