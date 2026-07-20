// ========================================
// 项目名称：Takt.Plat
// 文件名称：move-mps-master-data.cjs
// 功能描述：将稼动率/生产班组主数据从 Aps 迁至 Mps（全栈）
// 执行：node scripts/move-mps-master-data.cjs
// ========================================

const fs = require('fs');
const path = require('path');

const ROOT = path.join(__dirname, '..');

const FILE_PATTERNS = [
  'EquipmentOperationRate',
  'PersonnelOperationRate',
  'ProductionTeam',
  'StandardOperationRate',
];

const FRONTEND_SUBDIRS = [
  'equipment-operation-rate',
  'personnel-operation-rate',
  'production-team',
  'standard-operation-rate',
];

const BACKEND_LAYERS = [
  'backend/src/Takt.Domain/Entities/Logistics/Manufacturing',
  'backend/src/Takt.Application/Dtos/Logistics/Manufacturing',
  'backend/src/Takt.Application/Services/Logistics/Manufacturing',
  'backend/src/Takt.Application/Validators/Logistics/Manufacturing',
  'backend/src/Takt.WebApi/Controllers/Logistics/Manufacturing',
  'backend/src/Takt.Infrastructure/Data/Seeds/I18nSeedData/Logistics/Manufacturing',
];

const NS_PREFIXES = [
  'Takt.Domain.Entities.Logistics.Manufacturing',
  'Takt.Application.Dtos.Logistics.Manufacturing',
  'Takt.Application.Services.Logistics.Manufacturing',
  'Takt.Application.Validators.Logistics.Manufacturing',
  'Takt.WebApi.Controllers.Logistics.Manufacturing',
  'Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing',
];

function ensureDir(dir) {
  if (!fs.existsSync(dir)) fs.mkdirSync(dir, { recursive: true });
}

function matches(name) {
  return FILE_PATTERNS.some((p) => name.includes(p));
}

function moveFile(src, dest) {
  if (!fs.existsSync(src)) return false;
  ensureDir(path.dirname(dest));
  if (fs.existsSync(dest)) fs.unlinkSync(src);
  else fs.renameSync(src, dest);
  console.log(`[move] ${path.relative(ROOT, src)} → ${path.relative(ROOT, dest)}`);
  return true;
}

function moveDirContents(srcDir, destDir) {
  if (!fs.existsSync(srcDir)) return;
  ensureDir(destDir);
  for (const entry of fs.readdirSync(srcDir, { withFileTypes: true })) {
    const src = path.join(srcDir, entry.name);
    const dest = path.join(destDir, entry.name);
    if (entry.isDirectory()) moveDirContents(src, dest);
    else moveFile(src, dest);
  }
  try { fs.rmdirSync(srcDir); } catch (_) { /* not empty */ }
}

function fixApsToMpsNamespace(filePath) {
  let content = fs.readFileSync(filePath, 'utf8');
  const original = content;
  for (const prefix of NS_PREFIXES) {
    content = content.split(`${prefix}.Aps`).join(`${prefix}.Mps`);
  }
  if (content !== original) {
    fs.writeFileSync(filePath, content, 'utf8');
    console.log(`[ns] ${path.relative(ROOT, filePath)}`);
  }
}

function migrateBackend() {
  for (const layer of BACKEND_LAYERS) {
    const apsDir = path.join(ROOT, layer, 'Aps');
    const mpsDir = path.join(ROOT, layer, 'Mps');
    if (!fs.existsSync(apsDir)) continue;
    for (const file of fs.readdirSync(apsDir).filter((f) => f.endsWith('.cs') && matches(f))) {
      moveFile(path.join(apsDir, file), path.join(mpsDir, file));
    }
  }
  for (const layer of BACKEND_LAYERS) {
    const mpsDir = path.join(ROOT, layer, 'Mps');
    if (!fs.existsSync(mpsDir)) continue;
    for (const file of fs.readdirSync(mpsDir).filter((f) => f.endsWith('.cs') && matches(f))) {
      fixApsToMpsNamespace(path.join(mpsDir, file));
    }
  }
}

function migrateFrontend() {
  for (const sub of FRONTEND_SUBDIRS) {
    for (const base of ['views', 'api', 'types']) {
      const prefix = `frontend/src/${base}/logistics/manufacturing`;
      const src = path.join(ROOT, prefix, 'aps', sub);
      const dest = path.join(ROOT, prefix, 'mps', sub);
      if (fs.existsSync(src)) moveDirContents(src, dest);
      const srcFile = path.join(ROOT, prefix, 'aps', `${sub}.ts`);
      const srcDts = path.join(ROOT, prefix, 'aps', `${sub}.d.ts`);
      ensureDir(path.join(ROOT, prefix, 'mps'));
      moveFile(srcFile, path.join(ROOT, prefix, 'mps', `${sub}.ts`));
      moveFile(srcDts, path.join(ROOT, prefix, 'mps', `${sub}.d.ts`));
    }
  }
}

const PATH_REPLACEMENTS = [
  ['logistics/manufacturing/aps/production-team', 'logistics/manufacturing/mps/production-team'],
  ['logistics/manufacturing/aps/standard-operation-rate', 'logistics/manufacturing/mps/standard-operation-rate'],
  ['logistics/manufacturing/aps/equipment-operation-rate', 'logistics/manufacturing/mps/equipment-operation-rate'],
  ['logistics/manufacturing/aps/personnel-operation-rate', 'logistics/manufacturing/mps/personnel-operation-rate'],
  ['menu.logistics.manufacturing.aps.production.team', 'menu.logistics.manufacturing.mps.production.team'],
  ['menu.logistics.manufacturing.aps.standard.operation.rate', 'menu.logistics.manufacturing.mps.standard.operation.rate'],
  ['menu.logistics.manufacturing.aps.equipment.operation.rate', 'menu.logistics.manufacturing.mps.equipment.operation.rate'],
  ['menu.logistics.manufacturing.aps.personnel.operation.rate', 'menu.logistics.manufacturing.mps.personnel.operation.rate'],
];

const SCAN_EXTENSIONS = new Set(['.cs', '.ts', '.vue', '.d.ts']);

function walkDir(dir, files = []) {
  if (!fs.existsSync(dir)) return files;
  for (const entry of fs.readdirSync(dir, { withFileTypes: true })) {
    if (entry.name === 'node_modules' || entry.name === 'bin' || entry.name === 'obj' || entry.name === '.git') continue;
    const full = path.join(dir, entry.name);
    if (entry.isDirectory()) walkDir(full, files);
    else if (SCAN_EXTENSIONS.has(path.extname(entry.name)) || entry.name.endsWith('.d.ts')) files.push(full);
  }
  return files;
}

function applyReplacements() {
  let changed = 0;
  for (const file of walkDir(ROOT)) {
    if (file.includes('move-mps-master-data.cjs')) continue;
    let content = fs.readFileSync(file, 'utf8');
    const original = content;
    for (const [from, to] of PATH_REPLACEMENTS) {
      content = content.split(from).join(to);
    }
    if (content !== original) {
      fs.writeFileSync(file, content, 'utf8');
      changed++;
    }
  }
  console.log(`[text] updated ${changed} files`);
}

function patchGlobalUsings() {
  for (const rel of ['backend/src/Takt.Application/GlobalUsings.cs', 'backend/src/Takt.Infrastructure/GlobalUsings.cs']) {
    const file = path.join(ROOT, rel);
    if (!fs.existsSync(file)) continue;
    let content = fs.readFileSync(file, 'utf8');
    const inserts = [
      ['global using Takt.Domain.Entities.Logistics.Manufacturing.Aps;', 'global using Takt.Domain.Entities.Logistics.Manufacturing.Mps;\n'],
      ['global using Takt.Application.Dtos.Logistics.Manufacturing.Aps;', 'global using Takt.Application.Dtos.Logistics.Manufacturing.Mps;\n'],
      ['global using Takt.Application.Services.Logistics.Manufacturing.Aps;', 'global using Takt.Application.Services.Logistics.Manufacturing.Mps;\n'],
    ];
    for (const [anchor, line] of inserts) {
      if (!content.includes('Manufacturing.Mps') && content.includes(anchor)) {
        content = content.replace(anchor, anchor + '\n' + line.trim());
      } else if (!content.includes(line.trim())) {
        const mdsAnchor = anchor.replace('.Aps', '.Mds');
        if (content.includes(mdsAnchor) && !content.includes('Manufacturing.Mps')) {
          content = content.replace(mdsAnchor, mdsAnchor + '\n' + line.trim());
        }
      }
    }
    fs.writeFileSync(file, content, 'utf8');
    console.log(`[patch] ${rel}`);
  }
}

function main() {
  console.log('=== move-mps-master-data ===');
  migrateBackend();
  migrateFrontend();
  applyReplacements();
  patchGlobalUsings();
  console.log('=== done ===');
}

main();
