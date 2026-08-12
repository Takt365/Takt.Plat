'use strict';

/**
 * Global rename: MaterialName → MaterialDescription (and related variants).
 * Unify display fields to MaterialCode / MaterialDescription / MaterialSpecification.
 */
const fs = require('fs');
const path = require('path');

const ROOT = path.resolve(__dirname, '..');
const TARGET_DIRS = [
  path.join(ROOT, 'backend/src'),
  path.join(ROOT, 'frontend/src'),
];

const SKIP_DIR_NAMES = new Set([
  'node_modules',
  'bin',
  'obj',
  'dist',
  'artifacts',
  '.git',
]);

const EXT_OK = new Set([
  '.cs',
  '.ts',
  '.vue',
  '.tsx',
  '.js',
  '.cjs',
  '.json',
  '.sql',
  '.md',
  '.sbn',
]);

/** @type {Array<[RegExp, string]>} */
const RULES = [
  [/ManufacturerMaterialName/g, 'ManufacturerMaterialDescription'],
  [/manufacturerMaterialName/g, 'manufacturerMaterialDescription'],
  [/manufacturer_material_name/g, 'manufacturer_material_description'],
  [/制造商物料名称/g, '制造商物料描述'],
  [/ParentMaterialName/g, 'ParentMaterialDescription'],
  [/parentMaterialName/g, 'parentMaterialDescription'],
  [/parent_material_name/g, 'parent_material_description'],
  [/父物料名称/g, '父物料描述'],
  [/ComponentMaterialName/g, 'ComponentMaterialDescription'],
  [/componentMaterialName/g, 'componentMaterialDescription'],
  [/component_material_name/g, 'component_material_description'],
  [/子物料名称/g, '子物料描述'],
  // bare MaterialName / materialName / material_name (after prefixed variants)
  [/\bMaterialName\b/g, 'MaterialDescription'],
  [/\bmaterialName\b/g, 'materialDescription'],
  [/\bmaterial_name\b/g, 'material_description'],
  [/物料名称/g, '物料描述'],
];

/**
 * @param {string} dir
 * @param {(filePath: string) => void} onFile
 */
function walk(dir, onFile) {
  if (!fs.existsSync(dir)) return;
  for (const entry of fs.readdirSync(dir, { withFileTypes: true })) {
    if (entry.name.startsWith('.')) continue;
    const full = path.join(dir, entry.name);
    if (entry.isDirectory()) {
      if (SKIP_DIR_NAMES.has(entry.name)) continue;
      walk(full, onFile);
      continue;
    }
    const ext = path.extname(entry.name).toLowerCase();
    if (!EXT_OK.has(ext)) continue;
    onFile(full);
  }
}

/**
 * @param {string} content
 * @returns {{ next: string, hits: number }}
 */
function transform(content) {
  let next = content;
  let hits = 0;
  for (const [re, repl] of RULES) {
    const before = next;
    next = next.replace(re, repl);
    if (next !== before) {
      const m = before.match(re);
      hits += m ? m.length : 1;
    }
  }
  return { next, hits };
}

let filesChanged = 0;
let totalHits = 0;
const changed = [];

for (const dir of TARGET_DIRS) {
  walk(dir, (filePath) => {
    const raw = fs.readFileSync(filePath, 'utf8');
    if (!/MaterialName|materialName|material_name|物料名称|ManufacturerMaterialName|ParentMaterialName/.test(raw)) {
      return;
    }
    const { next, hits } = transform(raw);
    if (next === raw) return;
    fs.writeFileSync(filePath, next, 'utf8');
    filesChanged += 1;
    totalHits += hits;
    changed.push(path.relative(ROOT, filePath).replace(/\\/g, '/'));
  });
}

// Also Quartz SQL under WebApi wwwroot
const quartzDir = path.join(ROOT, 'backend/src/Takt.WebApi/wwwroot/Quartz');
walk(quartzDir, (filePath) => {
  const raw = fs.readFileSync(filePath, 'utf8');
  if (!/MaterialName|materialName|material_name|物料名称/.test(raw)) return;
  const { next, hits } = transform(raw);
  if (next === raw) return;
  fs.writeFileSync(filePath, next, 'utf8');
  filesChanged += 1;
  totalHits += hits;
  changed.push(path.relative(ROOT, filePath).replace(/\\/g, '/'));
});

console.log(JSON.stringify({ filesChanged, totalHits, sample: changed.slice(0, 40) }, null, 2));
