// ========================================
// 项目名称：Takt.Plat
// 文件名称：migrate-manufacturing-domains.cjs
// 功能描述：制造域目录重组 Demand→Mds、Planning→Mrp、Scheduling→Mps+Aps（全栈）
// 执行：node scripts/migrate-manufacturing-domains.cjs
// ========================================

const fs = require('fs');
const path = require('path');

const ROOT = path.join(__dirname, '..');

const MPS_PATTERNS = [
  'MasterProductionSchedule',
  'EquipmentOperationRate',
  'PersonnelOperationRate',
  'ProductionTeam',
  'StandardOperationRate',
];
const MRP_PATTERNS = ['MaterialRequirementsPlanning', 'ProductionPlan', 'PurchasePlan'];
const MDS_PATTERNS = ['MasterDemandSchedule', 'SalesForecast'];
const APS_PATTERNS = [
  'ApsSchedule', 'ApsOrder', 'ApsOperation', 'PlannedOrder', 'ProductionOrder',
  'ProductionDispatch', 'WorkCenter', 'ChangeoverMatrix', 'WorkCenterResource',
];

function ensureDir(dir) {
  if (!fs.existsSync(dir)) {
    fs.mkdirSync(dir, { recursive: true });
  }
}

function matchesAny(name, patterns) {
  return patterns.some((p) => name.includes(p));
}

function resolveTargetSegment(fileName, sourceSegment) {
  if (matchesAny(fileName, MDS_PATTERNS)) return 'Mds';
  if (matchesAny(fileName, MRP_PATTERNS)) return 'Mrp';
  if (matchesAny(fileName, MPS_PATTERNS)) return 'Mps';
  if (matchesAny(fileName, APS_PATTERNS)) return 'Aps';
  if (sourceSegment === 'Demand') return 'Mds';
  if (sourceSegment === 'Planning') return 'Mrp';
  if (sourceSegment === 'Scheduling') return 'Aps';
  return null;
}

function moveFile(src, dest) {
  if (!fs.existsSync(src)) return false;
  ensureDir(path.dirname(dest));
  if (fs.existsSync(dest)) {
    fs.unlinkSync(src);
    return false;
  }
  fs.renameSync(src, dest);
  return true;
}

function moveDirContents(srcDir, destDir) {
  if (!fs.existsSync(srcDir)) return;
  ensureDir(destDir);
  for (const entry of fs.readdirSync(srcDir, { withFileTypes: true })) {
    const src = path.join(srcDir, entry.name);
    const dest = path.join(destDir, entry.name);
    if (entry.isDirectory()) {
      moveDirContents(src, dest);
      try { fs.rmdirSync(src); } catch (_) { /* non-empty */ }
    } else {
      moveFile(src, dest);
    }
  }
  try { fs.rmdirSync(srcDir); } catch (_) { /* not empty */ }
}

const BACKEND_LAYERS = [
  'backend/src/Takt.Domain/Entities/Logistics/Manufacturing',
  'backend/src/Takt.Application/Dtos/Logistics/Manufacturing',
  'backend/src/Takt.Application/Services/Logistics/Manufacturing',
  'backend/src/Takt.Application/Validators/Logistics/Manufacturing',
  'backend/src/Takt.WebApi/Controllers/Logistics/Manufacturing',
  'backend/src/Takt.Infrastructure/Data/Seeds/I18nSeedData/Logistics/Manufacturing',
];

function migrateBackendLayer(layerRoot) {
  const absLayer = path.join(ROOT, layerRoot);
  if (!fs.existsSync(absLayer)) return;

  for (const oldSeg of ['Demand', 'Planning', 'Scheduling']) {
    const oldDir = path.join(absLayer, oldSeg);
    if (!fs.existsSync(oldDir)) continue;

    const files = fs.readdirSync(oldDir).filter((f) => f.endsWith('.cs'));
    for (const file of files) {
      const targetSeg = resolveTargetSegment(file, oldSeg);
      if (!targetSeg) {
        console.warn(`[skip] ${path.join(layerRoot, oldSeg, file)} — no target segment`);
        continue;
      }
      const src = path.join(oldDir, file);
      const dest = path.join(absLayer, targetSeg, file);
      if (moveFile(src, dest)) {
        console.log(`[move] ${path.relative(ROOT, src)} → ${path.relative(ROOT, dest)}`);
      } else if (fs.existsSync(dest)) {
        fs.unlinkSync(src);
        console.log(`[drop-dup] ${path.relative(ROOT, src)}`);
      }
    }
    try {
      const remaining = fs.readdirSync(oldDir);
      if (remaining.length === 0) fs.rmdirSync(oldDir);
    } catch (_) { /* ignore */ }
  }
}

function cleanupStaleBackendDirs() {
  for (const layer of BACKEND_LAYERS) {
    for (const oldSeg of ['Demand', 'Planning', 'Scheduling']) {
      const abs = path.join(ROOT, layer, oldSeg);
      if (fs.existsSync(abs)) {
        removeDirRecursive(abs);
        console.log(`[rmdir] ${path.relative(ROOT, abs)}`);
      }
    }
  }
}

const FRONTEND_MOVES = [
  { src: 'frontend/src/views/logistics/manufacturing/demand', dest: 'frontend/src/views/logistics/manufacturing/mds' },
  { src: 'frontend/src/api/logistics/manufacturing/demand', dest: 'frontend/src/api/logistics/manufacturing/mds' },
  { src: 'frontend/src/types/logistics/manufacturing/demand', dest: 'frontend/src/types/logistics/manufacturing/mds' },
  { src: 'frontend/src/locales/logistics/manufacturing/demand', dest: 'frontend/src/locales/logistics/manufacturing/mds' },
];

function migratePlanningToMds(baseType, subdir) {
  const prefix = baseType === 'locales'
    ? 'frontend/src/locales/logistics/manufacturing'
    : `frontend/src/${baseType}/logistics/manufacturing`;
  const src = path.join(ROOT, prefix, 'planning', subdir);
  const dest = path.join(ROOT, prefix, 'mds', subdir);
  if (fs.existsSync(src)) {
    moveDirContents(src, dest);
    console.log(`[move-dir] ${path.relative(ROOT, src)} → ${path.relative(ROOT, dest)}`);
  }
  const plain = path.join(ROOT, prefix, 'planning', `${subdir}.ts`);
  const dts = path.join(ROOT, prefix, 'planning', `${subdir}.d.ts`);
  const itemTs = path.join(ROOT, prefix, 'planning', `${subdir}-item.ts`);
  const itemDts = path.join(ROOT, prefix, 'planning', `${subdir}-item.d.ts`);
  for (const srcFile of [plain, dts, itemTs, itemDts]) {
    if (!fs.existsSync(srcFile)) continue;
    ensureDir(path.join(ROOT, prefix, 'mds'));
    moveFile(srcFile, path.join(ROOT, prefix, 'mds', path.basename(srcFile)));
  }
}

const PLANNING_TO_MRP = [
  'material-requirements-planning',
  'production-plan',
  'purchase-plan',
];

const PLANNING_TO_MDS = ['master-demand-schedule'];

const SCHEDULING_TO_MPS = [
  'master-production-schedule',
  'production-team',
  'standard-operation-rate',
  'equipment-operation-rate',
  'personnel-operation-rate',
];

const SCHEDULING_TO_APS = [
  'planned-order', 'production-order', 'aps-schedule', 'aps-order', 'work-center',
  'production-dispatch', 'changeover-matrix',
];

function moveFrontendSubdir(baseType, subdir, destParent) {
  const src = path.join(ROOT, `frontend/src/${baseType}/logistics/manufacturing`, subdir);
  const dest = path.join(ROOT, `frontend/src/${baseType}/logistics/manufacturing`, destParent);
  if (fs.existsSync(src)) {
    moveDirContents(src, dest);
    console.log(`[move-dir] ${path.relative(ROOT, src)} → ${path.relative(ROOT, dest)}`);
  }
}

function removeDirRecursive(dir) {
  if (!fs.existsSync(dir)) return;
  for (const entry of fs.readdirSync(dir, { withFileTypes: true })) {
    const full = path.join(dir, entry.name);
    if (entry.isDirectory()) removeDirRecursive(full);
    else fs.unlinkSync(full);
  }
  fs.rmdirSync(dir);
}

function migrateFrontendApiAndTypes() {
  for (const sub of PLANNING_TO_MRP) {
    for (const base of ['api', 'types']) {
      const prefix = `frontend/src/${base}/logistics/manufacturing`;
      const src = path.join(ROOT, prefix, 'planning', `${sub}.ts`);
      const srcDts = path.join(ROOT, prefix, 'planning', `${sub.replace(/-/g, '-')}.d.ts`);
      const destDir = path.join(ROOT, prefix, 'mrp');
      ensureDir(destDir);
      for (const srcFile of [src, srcDts, path.join(ROOT, prefix, 'planning', `${sub}-item.ts`), path.join(ROOT, prefix, 'planning', `${sub}-item.d.ts`)]) {
        if (!fs.existsSync(srcFile)) continue;
        const dest = path.join(destDir, path.basename(srcFile));
        moveFile(srcFile, dest);
      }
    }
  }

  for (const sub of PLANNING_TO_MDS) {
    for (const base of ['api', 'types']) {
      migratePlanningToMds(base, sub);
    }
  }

  const apsApiFiles = [
    'planned-order', 'production-order', 'aps-schedule', 'aps-schedule-item', 'aps-order', 'aps-operation',
    'work-center', 'work-center-resource', 'production-dispatch', 'changeover-matrix', 'standard-operation-rate',
    'personnel-operation-rate', 'equipment-operation-rate', 'production-team',
  ];
  const mpsApiFiles = ['master-production-schedule', 'master-production-schedule-line'];

  for (const base of ['api', 'types']) {
    const prefix = path.join(ROOT, `frontend/src/${base}/logistics/manufacturing`);
    const schedDir = path.join(prefix, 'scheduling');
    if (!fs.existsSync(schedDir)) continue;
    for (const file of fs.readdirSync(schedDir)) {
      const stem = file.replace(/\.(ts|d\.ts)$/, '').replace(/-item$/, '');
      const isMps = mpsApiFiles.some((p) => file.startsWith(p));
      const target = isMps ? 'mps' : 'aps';
      moveFile(path.join(schedDir, file), path.join(prefix, target, file));
    }
  }
}

function cleanupStaleFrontendDirs() {
  const stale = [
    'frontend/src/views/logistics/manufacturing/demand',
    'frontend/src/views/logistics/manufacturing/planning',
    'frontend/src/views/logistics/manufacturing/scheduling',
    'frontend/src/api/logistics/manufacturing/demand',
    'frontend/src/api/logistics/manufacturing/planning',
    'frontend/src/api/logistics/manufacturing/scheduling',
    'frontend/src/types/logistics/manufacturing/demand',
    'frontend/src/types/logistics/manufacturing/planning',
    'frontend/src/types/logistics/manufacturing/scheduling',
    'frontend/src/locales/logistics/manufacturing/demand',
    'frontend/src/locales/logistics/manufacturing/planning',
    'frontend/src/locales/logistics/manufacturing/scheduling',
  ];
  for (const rel of stale) {
    const abs = path.join(ROOT, rel);
    if (fs.existsSync(abs)) {
      removeDirRecursive(abs);
      console.log(`[rmdir] ${rel}`);
    }
  }
}

function migrateFrontend() {
  for (const { src, dest } of FRONTEND_MOVES) {
    const absSrc = path.join(ROOT, src);
    const absDest = path.join(ROOT, dest);
    if (fs.existsSync(absSrc)) {
      moveDirContents(absSrc, absDest);
      console.log(`[move-dir] ${src} → ${dest}`);
    }
  }

  for (const sub of PLANNING_TO_MRP) {
    for (const base of ['views', 'api', 'types', 'locales']) {
      const prefix = base === 'locales' ? 'frontend/src/locales/logistics/manufacturing' : `frontend/src/${base}/logistics/manufacturing`;
      const src = path.join(ROOT, prefix, 'planning', sub);
      const dest = path.join(ROOT, prefix, 'mrp', sub);
      if (fs.existsSync(src)) {
        moveDirContents(src, dest);
        console.log(`[move-dir] ${path.relative(ROOT, src)} → ${path.relative(ROOT, dest)}`);
      }
    }
  }

  for (const sub of PLANNING_TO_MDS) {
    for (const base of ['views', 'api', 'types', 'locales']) {
      migratePlanningToMds(base, sub);
    }
  }

  for (const sub of SCHEDULING_TO_MPS) {
    for (const base of ['views', 'api', 'types', 'locales']) {
      const prefix = base === 'locales' ? 'frontend/src/locales/logistics/manufacturing' : `frontend/src/${base}/logistics/manufacturing`;
      const src = path.join(ROOT, prefix, 'scheduling', sub);
      const dest = path.join(ROOT, prefix, 'mps', sub);
      if (fs.existsSync(src)) {
        moveDirContents(src, dest);
        console.log(`[move-dir] ${path.relative(ROOT, src)} → ${path.relative(ROOT, dest)}`);
      }
    }
  }

  for (const sub of SCHEDULING_TO_APS) {
    for (const base of ['views', 'api', 'types', 'locales']) {
      const prefix = base === 'locales' ? 'frontend/src/locales/logistics/manufacturing' : `frontend/src/${base}/logistics/manufacturing`;
      const src = path.join(ROOT, prefix, 'scheduling', sub);
      const dest = path.join(ROOT, prefix, 'aps', sub);
      if (fs.existsSync(src)) {
        moveDirContents(src, dest);
        console.log(`[move-dir] ${path.relative(ROOT, src)} → ${path.relative(ROOT, dest)}`);
      }
    }
  }
}

const PATH_REPLACEMENTS = [
  ['logistics/manufacturing/demand/', 'logistics/manufacturing/mds/'],
  ['logistics/manufacturing/demand', 'logistics/manufacturing/mds'],
  ['logistics/manufacturing/planning/', 'logistics/manufacturing/mrp/'],
  ['logistics/manufacturing/planning', 'logistics/manufacturing/mrp'],
  ['logistics/manufacturing/scheduling/master-production-schedule', 'logistics/manufacturing/mps/master-production-schedule'],
  ['logistics/manufacturing/scheduling/', 'logistics/manufacturing/aps/'],
  ['logistics/manufacturing/scheduling', 'logistics/manufacturing/aps'],
  ['/logistics/manufacturing/demand/', '/logistics/manufacturing/mds/'],
  ['/logistics/manufacturing/planning/', '/logistics/manufacturing/mrp/'],
  ['/logistics/manufacturing/scheduling/master-production-schedule', '/logistics/manufacturing/mps/master-production-schedule'],
  ['/logistics/manufacturing/scheduling/', '/logistics/manufacturing/aps/'],
  ['menu.logistics.manufacturing.demand.', 'menu.logistics.manufacturing.mds.'],
  ['menu.logistics.manufacturing.planning.', 'menu.logistics.manufacturing.mrp.'],
  ['menu.logistics.manufacturing.scheduling.master.production.schedule', 'menu.logistics.manufacturing.mps.master.production.schedule'],
  ['menu.logistics.manufacturing.scheduling.', 'menu.logistics.manufacturing.aps.'],
];

const NS_SUFFIXES = ['Demand', 'Planning', 'Scheduling', 'Mds', 'Mrp', 'Mps', 'Aps'];
const NS_PREFIXES = [
  'Takt.Domain.Entities.Logistics.Manufacturing',
  'Takt.Application.Dtos.Logistics.Manufacturing',
  'Takt.Application.Services.Logistics.Manufacturing',
  'Takt.Application.Validators.Logistics.Manufacturing',
  'Takt.WebApi.Controllers.Logistics.Manufacturing',
  'Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing',
];

function fixNamespacesInFile(filePath, targetSeg) {
  let content = fs.readFileSync(filePath, 'utf8');
  const original = content;
  for (const prefix of NS_PREFIXES) {
    for (const old of NS_SUFFIXES) {
      if (old === targetSeg) continue;
      content = content.split(`${prefix}.${old}`).join(`${prefix}.${targetSeg}`);
    }
  }
  if (content !== original) {
    fs.writeFileSync(filePath, content, 'utf8');
    return true;
  }
  return false;
}

function fixNamespacesByFolder() {
  for (const layer of BACKEND_LAYERS) {
    for (const seg of ['Mds', 'Mrp', 'Mps', 'Aps']) {
      const dir = path.join(ROOT, layer, seg);
      if (!fs.existsSync(dir)) continue;
      for (const file of fs.readdirSync(dir).filter((f) => f.endsWith('.cs'))) {
        const full = path.join(dir, file);
        if (fixNamespacesInFile(full, seg)) {
          console.log(`[ns] ${path.relative(ROOT, full)} → ${seg}`);
        }
      }
    }
  }
}

function applyPathReplacements(filePath) {
  let content = fs.readFileSync(filePath, 'utf8');
  const original = content;
  for (const [from, to] of PATH_REPLACEMENTS) {
    content = content.split(from).join(to);
  }
  if (content !== original) {
    fs.writeFileSync(filePath, content, 'utf8');
    return true;
  }
  return false;
}

const SCAN_EXTENSIONS = new Set(['.cs', '.ts', '.vue', '.cjs', '.mdc', '.md', '.json']);

function walkDir(dir, files = []) {
  if (!fs.existsSync(dir)) return files;
  for (const entry of fs.readdirSync(dir, { withFileTypes: true })) {
    if (entry.name === 'node_modules' || entry.name === 'bin' || entry.name === 'obj' || entry.name === '.git') continue;
    const full = path.join(dir, entry.name);
    if (entry.isDirectory()) walkDir(full, files);
    else if (SCAN_EXTENSIONS.has(path.extname(entry.name))) files.push(full);
  }
  return files;
}

function applyReplacements(filePath) {
  return applyPathReplacements(filePath);
}

function fixRemainingGlobalNamespaces() {
  const globals = [
    ['.Manufacturing.Demand', '.Manufacturing.Mds'],
    ['.Manufacturing.Planning', '.Manufacturing.Mrp'],
    ['.Manufacturing.Scheduling', '.Manufacturing.Aps'],
  ];
  for (const f of walkDir(ROOT)) {
    if (!f.endsWith('.cs') || f.includes('migrate-manufacturing-domains')) continue;
    let c = fs.readFileSync(f, 'utf8');
    const o = c;
    for (const [a, b] of globals) c = c.split(a).join(b);
    if (c !== o) fs.writeFileSync(f, c, 'utf8');
  }
}

function fixMpsCrossReferences() {
  const mpsOnlyFiles = [];
  for (const f of walkDir(ROOT)) {
    if (!f.endsWith('.cs')) continue;
    const c = fs.readFileSync(f, 'utf8');
    if (!c.includes('MasterProductionSchedule')) continue;
    const hasApsOther = /TaktAps(Schedule|Order|Operation)/.test(c);
    if (!hasApsOther) mpsOnlyFiles.push(f);
  }
  for (const f of mpsOnlyFiles) {
    let c = fs.readFileSync(f, 'utf8');
    const o = c;
    for (const prefix of NS_PREFIXES) {
      c = c.split(`${prefix}.Aps`).join(`${prefix}.Mps`);
    }
    if (c !== o) {
      fs.writeFileSync(f, c, 'utf8');
      console.log(`[mps-ref] ${path.relative(ROOT, f)}`);
    }
  }
}

function patchGlobalUsings() {
  for (const rel of ['backend/src/Takt.Infrastructure/GlobalUsings.cs', 'backend/src/Takt.Application/GlobalUsings.cs']) {
    const file = path.join(ROOT, rel);
    if (!fs.existsSync(file)) continue;
    let content = fs.readFileSync(file, 'utf8');
    content = content
      .replace('global using Takt.Domain.Entities.Logistics.Manufacturing.Scheduling;\n', '')
      .replace('global using Takt.Domain.Entities.Logistics.Manufacturing.Demand;\n', '')
      .replace('global using Takt.Domain.Entities.Logistics.Manufacturing.Planning;\n', '')
      .replace('global using Takt.Application.Dtos.Logistics.Manufacturing.Scheduling;\n', '')
      .replace('global using Takt.Application.Dtos.Logistics.Manufacturing.Demand;\n', '')
      .replace('global using Takt.Application.Dtos.Logistics.Manufacturing.Planning;\n', '')
      .replace('global using Takt.Application.Services.Logistics.Manufacturing.Scheduling;\n', '')
      .replace('global using Takt.Application.Services.Logistics.Manufacturing.Demand;\n', '')
      .replace('global using Takt.Application.Services.Logistics.Manufacturing.Planning;\n', '');

    const entityInsert = 'global using Takt.Domain.Entities.Logistics.Manufacturing.Sop;';
    const dtoInsert = 'global using Takt.Application.Dtos.Logistics.Manufacturing.Sop;';
    const svcInsert = 'global using Takt.Application.Services.Logistics.Manufacturing.Sop;';

    if (!content.includes('Manufacturing.Mds')) {
      content = content.replace(
        entityInsert,
        'global using Takt.Domain.Entities.Logistics.Manufacturing.Mds;\n'
        + 'global using Takt.Domain.Entities.Logistics.Manufacturing.Mrp;\n'
        + 'global using Takt.Domain.Entities.Logistics.Manufacturing.Mps;\n'
        + 'global using Takt.Domain.Entities.Logistics.Manufacturing.Aps;\n'
        + entityInsert
      );
    }
    if (!content.includes('Dtos.Logistics.Manufacturing.Mds')) {
      content = content.replace(
        dtoInsert,
        'global using Takt.Application.Dtos.Logistics.Manufacturing.Mds;\n'
        + 'global using Takt.Application.Dtos.Logistics.Manufacturing.Mrp;\n'
        + 'global using Takt.Application.Dtos.Logistics.Manufacturing.Mps;\n'
        + 'global using Takt.Application.Dtos.Logistics.Manufacturing.Aps;\n'
        + dtoInsert
      );
    }
    if (!content.includes('Services.Logistics.Manufacturing.Mds')) {
      content = content.replace(
        svcInsert,
        'global using Takt.Application.Services.Logistics.Manufacturing.Mds;\n'
        + 'global using Takt.Application.Services.Logistics.Manufacturing.Mrp;\n'
        + 'global using Takt.Application.Services.Logistics.Manufacturing.Mps;\n'
        + 'global using Takt.Application.Services.Logistics.Manufacturing.Aps;\n'
        + svcInsert
      );
    }
    fs.writeFileSync(file, content, 'utf8');
    console.log(`[patch] ${rel}`);
  }
}

function main() {
  console.log('=== migrate-manufacturing-domains ===');

  for (const layer of BACKEND_LAYERS) {
    migrateBackendLayer(layer);
  }
  cleanupStaleBackendDirs();

  migrateFrontend();
  migrateFrontendApiAndTypes();
  cleanupStaleFrontendDirs();

  fixNamespacesByFolder();
  fixRemainingGlobalNamespaces();
  fixMpsCrossReferences();

  const files = walkDir(ROOT);
  let changed = 0;
  for (const file of files) {
    if (file.includes('migrate-manufacturing-domains.cjs')) continue;
    if (applyReplacements(file)) {
      changed++;
    }
  }
  console.log(`[text] updated ${changed} files`);

  patchGlobalUsings();

  console.log('=== done ===');
}

main();
