'use strict';

/**
 * 设变实体/服务/DTO 重命名：TaktEcEng、TaktEcExec* → 与四级菜单 ec-gijutsu、ec-bukan 等一致
 * 执行：node scripts/rename-ec-entities-to-dept-slugs.cjs
 */
const fs = require('fs');
const path = require('path');

const ROOT = path.join(__dirname, '..');

/** 文本替换（长串优先，避免部分匹配） */
const TEXT_PAIRS = [
  // 实体类 / 表名 / 索引片段
  ['TaktEcExecAssy', 'TaktEcSeizouikka'],
  ['TaktEcExecPcba', 'TaktEcSeizounika'],
  ['TaktEcExecPmc', 'TaktEcSeikan'],
  ['TaktEcExecIqc', 'TaktEcUkeken'],
  ['TaktEcExecMp', 'TaktEcKoubai'],
  ['TaktEcExecMc', 'TaktEcBukan'],
  ['TaktEcExecQa', 'TaktEcHinkan'],
  ['TaktEcExecTe', 'TaktEcSeizougijutsu'],
  ['TaktEcEng', 'TaktEcGijutsu'],
  ['takt_logistics_manufacturing_ec_exec_assy', 'takt_logistics_manufacturing_ec_seizouikka'],
  ['takt_logistics_manufacturing_ec_exec_pcba', 'takt_logistics_manufacturing_ec_seizounika'],
  ['takt_logistics_manufacturing_ec_exec_pmc', 'takt_logistics_manufacturing_ec_seikan'],
  ['takt_logistics_manufacturing_ec_exec_iqc', 'takt_logistics_manufacturing_ec_ukeken'],
  ['takt_logistics_manufacturing_ec_exec_mp', 'takt_logistics_manufacturing_ec_koubai'],
  ['takt_logistics_manufacturing_ec_exec_mc', 'takt_logistics_manufacturing_ec_bukan'],
  ['takt_logistics_manufacturing_ec_exec_qa', 'takt_logistics_manufacturing_ec_hinkan'],
  ['takt_logistics_manufacturing_ec_exec_te', 'takt_logistics_manufacturing_ec_seizougijutsu'],
  ['takt_logistics_manufacturing_ec_eng', 'takt_logistics_manufacturing_ec_gijutsu'],
  ['ix_ec_exec_assy', 'ix_ec_seizouikka'],
  ['ix_ec_exec_pcba', 'ix_ec_seizounika'],
  ['ix_ec_exec_pmc', 'ix_ec_seikan'],
  ['ix_ec_exec_iqc', 'ix_ec_ukeken'],
  ['ix_ec_exec_mp', 'ix_ec_koubai'],
  ['ix_ec_exec_mc', 'ix_ec_bukan'],
  ['ix_ec_exec_qa', 'ix_ec_hinkan'],
  ['ix_ec_exec_te', 'ix_ec_seizougijutsu'],
  ['ix_ec_eng', 'ix_ec_gijutsu'],
  // DTO / 导航属性（EcExec* 先于 EcEng）
  ['EcExecAssy', 'EcSeizouikka'],
  ['EcExecPcba', 'EcSeizounika'],
  ['EcExecPmc', 'EcSeikan'],
  ['EcExecIqc', 'EcUkeken'],
  ['EcExecMp', 'EcKoubai'],
  ['EcExecMc', 'EcBukan'],
  ['EcExecQa', 'EcHinkan'],
  ['EcExecTe', 'EcSeizougijutsu'],
  ['EcEngId', 'EcGijutsuId'],
  ['EcEng', 'EcGijutsu'],
  // i18n 键 entity.*
  ['entity.ecexecassy', 'entity.ecseizouikka'],
  ['entity.ecexecpcba', 'entity.ecseizounika'],
  ['entity.ecexecpmc', 'entity.ecseikan'],
  ['entity.ecexeciqc', 'entity.ecukeken'],
  ['entity.ecexecmp', 'entity.eckoubai'],
  ['entity.ecexecmc', 'entity.ecbukan'],
  ['entity.ecexecqa', 'entity.echinkan'],
  ['entity.ecexecte', 'entity.ecseizougijutsu'],
  ['entity.eceng', 'entity.ecgijutsu'],
  // 权限（旧 ec:eng → gijutsu，与菜单一致）
  ['logistics:manufacturing:engineering:change:ec:eng:', 'logistics:manufacturing:engineering:change:gijutsu:'],
  // 前端 types/api（EcEng → EcGijutsu 已在上面；ec-eng 文件名）
  ['ec-eng-source-input', 'ec-gijutsu-source-input'],
  ['ec-eng.d.ts', 'ec-gijutsu.d.ts'],
  ['ec-eng.ts', 'ec-gijutsu.ts'],
  ['ecEngId', 'ecGijutsuId'],
  ['getEcEng', 'getEcGijutsu'],
  ['createEcEng', 'createEcGijutsu'],
  ['updateEcEng', 'updateEcGijutsu'],
  ['deleteEcEng', 'deleteEcGijutsu'],
  ['importEcEng', 'importEcGijutsu'],
  ['exportEcEng', 'exportEcGijutsu'],
  ['EcEngCreate', 'EcGijutsuCreate'],
  ['EcEngUpdate', 'EcGijutsuUpdate'],
  ['EcEngQuery', 'EcGijutsuQuery'],
  ['EcEngStatus', 'EcGijutsuStatus'],
  ['EcEngTemplate', 'EcGijutsuTemplate'],
  ['EcEngImport', 'EcGijutsuImport'],
  ['EcEngExport', 'EcGijutsuExport'],
  ['EcEngStatQuery', 'EcGijutsuStatQuery'],
  ['EcEngStat', 'EcGijutsuStat'],
  ['EcEngSourceEcInput', 'EcGijutsuSourceEcInput'],
  ['EcEngImportFromSource', 'EcGijutsuImportFromSource'],
  ['createdEcEngIds', 'createdEcGijutsuIds'],
  ['TaktEcEngs', 'TaktEcGijutsus'],
  ['provideEcEngMasterContext', 'provideEcGijutsuMasterContext'],
  ['useEcEngMasterContext', 'useEcGijutsuMasterContext'],
  ['EcEngForm', 'EcGijutsuForm'],
  ['ec-eng-form', 'ec-gijutsu-form'],
  ['ec-eng-form.vue', 'ec-gijutsu-form.vue'],
  ['engineering-change/ec-eng', 'engineering-change/ec-gijutsu'],
  ['loadEcEngDetail', 'loadEcGijutsuDetail'],
  ['getEcEngId', 'getEcGijutsuId'],
  ['getEcEngField', 'getEcGijutsuField'],
  ['[EcEng]', '[EcGijutsu]'],
  ['EC_ENG_API_BASE', 'EC_GIJUTSU_API_BASE'],
];

/** 文件名重命名（basename 含扩展名） */
const FILE_RENAMES = [
  ['TaktEcExecAssy.cs', 'TaktEcSeizouikka.cs'],
  ['TaktEcExecPcba.cs', 'TaktEcSeizounika.cs'],
  ['TaktEcExecPmc.cs', 'TaktEcSeikan.cs'],
  ['TaktEcExecIqc.cs', 'TaktEcUkeken.cs'],
  ['TaktEcExecMp.cs', 'TaktEcKoubai.cs'],
  ['TaktEcExecMc.cs', 'TaktEcBukan.cs'],
  ['TaktEcExecQa.cs', 'TaktEcHinkan.cs'],
  ['TaktEcExecTe.cs', 'TaktEcSeizougijutsu.cs'],
  ['TaktEcEng.cs', 'TaktEcGijutsu.cs'],
  ['TaktEcExecAssyValidators.cs', 'TaktEcSeizouikkaValidators.cs'],
  ['TaktEcExecPcbaValidators.cs', 'TaktEcSeizounikaValidators.cs'],
  ['TaktEcExecPmcValidators.cs', 'TaktEcSeikanValidators.cs'],
  ['TaktEcExecIqcValidators.cs', 'TaktEcUkekenValidators.cs'],
  ['TaktEcExecMpValidators.cs', 'TaktEcKoubaiValidators.cs'],
  ['TaktEcExecMcValidators.cs', 'TaktEcBukanValidators.cs'],
  ['TaktEcExecQaValidators.cs', 'TaktEcHinkanValidators.cs'],
  ['TaktEcExecTeValidators.cs', 'TaktEcSeizougijutsuValidators.cs'],
  ['TaktEcEngValidators.cs', 'TaktEcGijutsuValidators.cs'],
  ['TaktEcEngDtos.cs', 'TaktEcGijutsuDtos.cs'],
  ['TaktEcEngService.cs', 'TaktEcGijutsuService.cs'],
  ['ITaktEcEngService.cs', 'ITaktEcGijutsuService.cs'],
  ['TaktEcEngsController.cs', 'TaktEcGijutsusController.cs'],
  ['TaktEcExecAssyI18nSeedData.cs', 'TaktEcSeizouikkaI18nSeedData.cs'],
  ['TaktEcExecPcbaI18nSeedData.cs', 'TaktEcSeizounikaI18nSeedData.cs'],
  ['TaktEcExecPmcI18nSeedData.cs', 'TaktEcSeikanI18nSeedData.cs'],
  ['TaktEcExecIqcI18nSeedData.cs', 'TaktEcUkekenI18nSeedData.cs'],
  ['TaktEcExecMpI18nSeedData.cs', 'TaktEcKoubaiI18nSeedData.cs'],
  ['TaktEcExecMcI18nSeedData.cs', 'TaktEcBukanI18nSeedData.cs'],
  ['TaktEcExecQaI18nSeedData.cs', 'TaktEcHinkanI18nSeedData.cs'],
  ['TaktEcExecTeI18nSeedData.cs', 'TaktEcSeizougijutsuI18nSeedData.cs'],
  ['TaktEcEngI18nSeedData.cs', 'TaktEcGijutsuI18nSeedData.cs'],
  ['ec-eng.d.ts', 'ec-gijutsu.d.ts'],
  ['ec-eng.ts', 'ec-gijutsu.ts'],
  ['ec-eng-form.vue', 'ec-gijutsu-form.vue'],
  ['use-ec-eng-master-context.ts', 'use-ec-gijutsu-master-context.ts'],
];

const SCAN_DIRS = [
  path.join(ROOT, 'backend', 'src'),
  path.join(ROOT, 'frontend', 'src'),
  path.join(ROOT, 'scripts'),
];

const SKIP_DIR = new Set(['node_modules', 'bin', 'obj', '.git']);

function rep(text) {
  let out = text;
  for (const [from, to] of TEXT_PAIRS) {
    out = out.split(from).join(to);
  }
  return out;
}

function walkFiles(dir, out) {
  if (!fs.existsSync(dir)) return;
  for (const entry of fs.readdirSync(dir, { withFileTypes: true })) {
    if (SKIP_DIR.has(entry.name)) continue;
    const full = path.join(dir, entry.name);
    if (entry.isDirectory()) walkFiles(full, out);
    else if (/\.(cs|ts|vue|cjs|json|mdc)$/.test(entry.name)) out.push(full);
  }
}

function patchContents() {
  const files = [];
  for (const d of SCAN_DIRS) walkFiles(d, files);
  let changed = 0;
  for (const filePath of files) {
    if (filePath.endsWith('rename-ec-entities-to-dept-slugs.cjs')) continue;
    const raw = fs.readFileSync(filePath, 'utf8');
    const next = rep(raw);
    if (next !== raw) {
      fs.writeFileSync(filePath, next, 'utf8');
      changed += 1;
    }
  }
  return changed;
}

function renameFiles() {
  const allFiles = [];
  for (const d of SCAN_DIRS) walkFiles(d, allFiles);
  const byDir = new Map();
  for (const f of allFiles) {
    const dir = path.dirname(f);
    const base = path.basename(f);
    if (!byDir.has(dir)) byDir.set(dir, new Set());
    byDir.get(dir).add(base);
  }
  let renamed = 0;
  for (const [from, to] of FILE_RENAMES) {
    for (const [dir, names] of byDir) {
      if (!names.has(from)) continue;
      const src = path.join(dir, from);
      const dest = path.join(dir, to);
      if (fs.existsSync(dest)) {
        fs.unlinkSync(src);
        console.log(`  删除重复: ${src}（目标已存在 ${dest}）`);
      } else {
        fs.renameSync(src, dest);
        console.log(`  重命名: ${src} → ${dest}`);
        renamed += 1;
      }
    }
  }
  return renamed;
}

function renameEcEngViewDir() {
  const engDir = path.join(ROOT, 'frontend', 'src', 'views', 'logistics', 'manufacturing', 'engineering-change', 'ec-eng');
  const gijutsuDir = path.join(ROOT, 'frontend', 'src', 'views', 'logistics', 'manufacturing', 'engineering-change', 'ec-gijutsu');
  if (!fs.existsSync(engDir)) return;
  if (fs.existsSync(gijutsuDir)) {
    fs.rmSync(engDir, { recursive: true, force: true });
    console.log(`  删除重复视图目录: ${engDir}`);
  } else {
    fs.renameSync(engDir, gijutsuDir);
    console.log(`  重命名视图目录: ec-eng → ec-gijutsu`);
  }
}

function renameEcEngLocalesDir() {
  const engDir = path.join(ROOT, 'frontend', 'src', 'locales', 'logistics', 'manufacturing', 'engineering-change', 'ec-eng');
  const gijutsuDir = path.join(ROOT, 'frontend', 'src', 'locales', 'logistics', 'manufacturing', 'engineering-change', 'ec-gijutsu');
  if (!fs.existsSync(engDir)) return;
  if (fs.existsSync(gijutsuDir)) {
    fs.rmSync(engDir, { recursive: true, force: true });
    console.log(`  删除重复 locales: ${engDir}`);
  } else {
    fs.renameSync(engDir, gijutsuDir);
    console.log(`  重命名 locales: ec-eng → ec-gijutsu`);
  }
}

console.log('1/4 文本替换…');
const patched = patchContents();
console.log(`  已更新 ${patched} 个文件`);

console.log('2/4 文件重命名…');
const renamed = renameFiles();
console.log(`  已重命名 ${renamed} 个文件`);

console.log('3/4 视图/locales 目录…');
renameEcEngViewDir();
renameEcEngLocalesDir();

console.log('4/4 完成 rename-ec-entities-to-dept-slugs');
