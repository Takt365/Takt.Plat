/**
 * 一次性迁移：TaktEcDept 宽表 → TaktEcExec 头表 + 子表（后端引用批量替换）
 * 执行：node scripts/migrate-ec-dept-to-ec-exec.cjs
 */
const fs = require('fs');
const path = require('path');

const ROOT = path.join(__dirname, '..');
const BACKEND = path.join(ROOT, 'backend', 'src');

const VIEW_SERVICES = [
  'TaktEcKoubaiService.cs',
  'TaktEcSeikanService.cs',
  'TaktEcUkekenService.cs',
  'TaktEcBukanService.cs',
  'TaktEcSeizounikaService.cs',
  'TaktEcSeizouikkaService.cs',
  'TaktEcHinkanService.cs',
];

function walk(dir, acc = []) {
  for (const name of fs.readdirSync(dir)) {
    const p = path.join(dir, name);
    const st = fs.statSync(p);
    if (st.isDirectory()) walk(p, acc);
    else if (p.endsWith('.cs')) acc.push(p);
  }
  return acc;
}

function patchViewService(filePath) {
  let s = fs.readFileSync(filePath, 'utf8');
  if (!s.includes('TaktEcDeptViewServiceBase')) return false;
  s = s.replace(
    /ITaktCompanyRepository<TaktEcDept> ecDeptRepository,/g,
    'TaktEcExecPersistence ecExecPersistence,'
  );
  s = s.replace(
    /ecDetailRepository, ecDeptRepository, lineNumberGenerator/g,
    'ecDetailRepository, ecExecPersistence, lineNumberGenerator'
  );
  fs.writeFileSync(filePath, s, 'utf8');
  return true;
}

function patchCsFile(filePath) {
  if (filePath.includes('TaktEcDept.cs')) return false;
  if (filePath.includes('migrate-ec-dept-to-ec-exec.cjs')) return false;
  let s = fs.readFileSync(filePath, 'utf8');
  const orig = s;
  // 实体与仓储类型（保留 TaktEcDeptCodes / TaktEcDeptView / TaktEcDeptTransposed 等视图命名）
  s = s.replace(/\bTaktEcDept\b(?!Codes|View|Transposed|Batch|I18n|Validator|Service|Query|Create|Update|Export|Import|Template|Stat|Dto|Flat|Panel)/g, (m, offset, str) => {
    const after = str.slice(offset, offset + 20);
    if (after.startsWith('TaktEcDeptCodes') || after.startsWith('TaktEcDeptView') ||
        after.startsWith('TaktEcDeptTransposed') || after.startsWith('TaktEcDeptBatch') ||
        after.startsWith('TaktEcDeptI18n') || after.startsWith('TaktEcDeptValidator') ||
        after.startsWith('TaktEcDeptService') || after.startsWith('TaktEcDeptQuery') ||
        after.startsWith('TaktEcDeptCreate') || after.startsWith('TaktEcDeptUpdate') ||
        after.startsWith('TaktEcDeptExport') || after.startsWith('TaktEcDeptImport') ||
        after.startsWith('TaktEcDeptTemplate') || after.startsWith('TaktEcDeptStat') ||
        after.startsWith('TaktEcDeptDto') || after.startsWith('TaktEcDeptFlat')) {
      return m;
    }
    return 'TaktEcExec';
  });
  s = s.replace(/\bEcDeptContent\b/g, 'ExecContent');
  s = s.replace(/\bec_dept_content\b/g, 'exec_content');
  s = s.replace(/\bEcDeptId\b/g, 'EcExecId');
  s = s.replace(/\bec_dept_id\b/g, 'ec_exec_id');
  s = s.replace(/\btakt_logistics_manufacturing_ec_dept\b/g, 'takt_logistics_manufacturing_ec_exec');
  s = s.replace(/\bix_takt_logistics_manufacturing_ec_dept_unique\b/g, 'ix_takt_logistics_manufacturing_ec_exec_unique');
  s = s.replace(/\bDeptRecords\b/g, 'ExecRecords');
  if (s !== orig) {
    fs.writeFileSync(filePath, s, 'utf8');
    return true;
  }
  return false;
}

const ecDir = path.join(BACKEND, 'Takt.Application', 'Services', 'Logistics', 'Manufacturing', 'EngineeringChange');
let n = 0;
for (const f of VIEW_SERVICES) {
  const p = path.join(ecDir, f);
  if (fs.existsSync(p) && patchViewService(p)) n++;
}

const files = walk(BACKEND);
let patched = 0;
for (const f of files) {
  if (patchCsFile(f)) patched++;
}

// Autofac 注册 TaktEcExecPersistence
const autofacPath = path.join(BACKEND, 'Takt.Infrastructure', 'DependencyInjection', 'TaktAutofacModule.cs');
let autofac = fs.readFileSync(autofacPath, 'utf8');
if (!autofac.includes('TaktEcExecPersistence')) {
  autofac = autofac.replace(
    'builder.RegisterType<TaktLineNumberGenerator>()',
    'builder.RegisterType<Takt.Application.Services.Logistics.Manufacturing.EngineeringChange.TaktEcExecPersistence>()\n            .InstancePerLifetimeScope();\n\n        builder.RegisterType<TaktLineNumberGenerator>()'
  );
  fs.writeFileSync(autofacPath, autofac, 'utf8');
}

console.log(`View services: ${n}, CS files patched: ${patched}, Autofac: ${autofac.includes('TaktEcExecPersistence') ? 'ok' : 'skip'}`);
