'use strict';

const fs = require('fs');
const path = require('path');

const ROOT = path.join(__dirname, '..', 'frontend', 'src');
const TYPE_PAIRS = [
  ['EcGijutsuStatQuery', 'EcGijutsuStatQuery'],
  ['EcGijutsuStat', 'EcGijutsuStat'],
  ['EcGijutsuTemplate', 'EcGijutsuTemplate'],
  ['EcGijutsuImport', 'EcGijutsuImport'],
  ['EcGijutsuExport', 'EcGijutsuExport'],
  ['EcGijutsuStatus', 'EcGijutsuStatus'],
  ['EcGijutsuUpdate', 'EcGijutsuUpdate'],
  ['EcGijutsuCreate', 'EcGijutsuCreate'],
  ['EcGijutsuQuery', 'EcGijutsuQuery'],
  ['EcGijutsuSourceEcInputItem', 'EcGijutsuSourceEcInputItem'],
  ['EcGijutsuSourceEcInputQuery', 'EcGijutsuSourceEcInputQuery'],
  ['EcGijutsuImportFromSourceResult', 'EcGijutsuImportFromSourceResult'],
  ['EcGijutsuImportFromSource', 'EcGijutsuImportFromSource'],
  ['EcGijutsu', 'EcGijutsu'],
];
const API_PAIRS = [
  ['getUnimportedSourceEcGijutsuList', 'getUnimportedSourceEcGijutsuList'],
  ['importEcGijutsuFromSource', 'importEcGijutsuFromSource'],
  ['getEcGijutsuStat', 'getEcGijutsuStat'],
  ['getEcGijutsuOptions', 'getEcGijutsuOptions'],
  ['getEcGijutsuTemplate', 'getEcGijutsuTemplate'],
  ['importEcGijutsu', 'importEcGijutsu'],
  ['exportEcGijutsu', 'exportEcGijutsu'],
  ['updateEcGijutsuStatus', 'updateEcGijutsuStatus'],
  ['deleteEcGijutsuBatch', 'deleteEcGijutsuBatch'],
  ['deleteEcGijutsuById', 'deleteEcGijutsuById'],
  ['updateEcGijutsu', 'updateEcGijutsu'],
  ['createEcGijutsu', 'createEcGijutsu'],
  ['getEcGijutsuById', 'getEcGijutsuById'],
  ['getEcGijutsuList', 'getEcGijutsuList'],
  ['EC_GIJUTSU_API_BASE', 'EC_GIJUTSU_API_BASE'],
];

function rep(text, pairs) {
  let out = text;
  for (const [from, to] of pairs) out = out.split(from).join(to);
  return out;
}

function writeGijutsuTypes() {
  const src = path.join(ROOT, 'types/logistics/manufacturing/engineering-change/ec-gijutsu.d.ts');
  let types = fs.readFileSync(src, 'utf8');
  types = types.replace('文件名称：ec-gijutsu.d.ts', '文件名称：ec-gijutsu.d.ts');
  types = types.replace(
    '功能描述：logistics/manufacturing/engineering-change 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）',
    '功能描述：设变技术部门页面类型（对应后端 TaktEcGijutsu / TaktEcGijutsuDto；主键字段 ecGijutsuId）',
  );
  types = types.replace('对应前端 EcGijutsu', '对应前端 EcGijutsu');
  types = types.replace(
    "import type {\n  CompanyDtoBase,\n  TaktPagedQuery\n} from '@/types/common';",
    "import type {\n  CompanyDtoBase,\n  TaktPagedQuery\n} from '@/types/common';\nimport type { EcAttachment, EcAttachmentCreate } from './ec-attachment';\nimport type { EcDetail, EcDetailCreate } from './ec-detail';",
  );
  types = rep(types, TYPE_PAIRS);
  fs.writeFileSync(path.join(ROOT, 'types/logistics/manufacturing/engineering-change/ec-gijutsu.d.ts'), types);
}

function writeGijutsuSourceInputTypes() {
  const src = path.join(ROOT, 'types/logistics/manufacturing/engineering-change/ec-gijutsu-source-input.d.ts');
  let types = fs.readFileSync(src, 'utf8');
  types = types.replace('ec-gijutsu-source-input', 'ec-gijutsu-source-input');
  types = rep(types, TYPE_PAIRS);
  fs.writeFileSync(path.join(ROOT, 'types/logistics/manufacturing/engineering-change/ec-gijutsu-source-input.d.ts'), types);
}

function writeGijutsuApi() {
  const src = path.join(ROOT, 'api/logistics/manufacturing/engineering-change/ec-gijutsu.ts');
  let api = fs.readFileSync(src, 'utf8');
  api = api.replace('文件名称：ec-gijutsu.ts', '文件名称：ec-gijutsu.ts');
  api = api.replace(
    '功能描述：设变技术课主表 API（对应 TaktEcGijutsusController）',
    '功能描述：设变技术部门 API（后端 TaktEcGijutsusController / 实体 TaktEcGijutsu）',
  );
  api = api.replace(/ec-gijutsu-source-input/g, 'ec-gijutsu-source-input');
  api = api.replace(/ec-eng'/g, "ec-gijutsu'");
  api = rep(api, [...TYPE_PAIRS, ...API_PAIRS]);
  fs.writeFileSync(path.join(ROOT, 'api/logistics/manufacturing/engineering-change/ec-gijutsu.ts'), api);
}

function walk(dir, cb) {
  if (!fs.existsSync(dir)) return;
  for (const entry of fs.readdirSync(dir, { withFileTypes: true })) {
    const full = path.join(dir, entry.name);
    if (entry.isDirectory()) walk(full, cb);
    else if (/\.(vue|ts)$/.test(full)) cb(full);
  }
}

function patchViews() {
  const dirs = [
    path.join(ROOT, 'views/logistics/manufacturing/engineering-change'),
    path.join(ROOT, 'views/dashboard/data-board'),
  ];
  for (const dir of dirs) {
    walk(dir, (filePath) => {
      let text = fs.readFileSync(filePath, 'utf8');
      const before = text;
      text = text.replace(/\/ec-gijutsu-source-input/g, '/ec-gijutsu-source-input');
      text = text.replace(/\/ec-eng'/g, "/ec-gijutsu'");
      text = rep(text, [...TYPE_PAIRS, ...API_PAIRS]);
      text = text.replace(/getEcGijutsuId/g, 'getEcGijutsuId');
      text = text.replace(/getEcGijutsuField/g, 'getEcGijutsuField');
      text = text.replace(/loadEcGijutsuDetail/g, 'loadEcGijutsuDetail');
      text = text.replace(/\[EcGijutsu\]/g, '[EcGijutsu]');
      text = text.replace(/EcGijutsuDetail/g, 'EcDetail');
      if (text !== before) fs.writeFileSync(filePath, text, 'utf8');
    });
  }
}

function removeLegacy() {
  for (const rel of [
    'types/logistics/manufacturing/engineering-change/ec-gijutsu.d.ts',
    'types/logistics/manufacturing/engineering-change/ec-gijutsu-source-input.d.ts',
    'api/logistics/manufacturing/engineering-change/ec-gijutsu.ts',
  ]) {
    const full = path.join(ROOT, rel);
    if (fs.existsSync(full)) fs.unlinkSync(full);
  }
}

writeGijutsuTypes();
writeGijutsuSourceInputTypes();
writeGijutsuApi();
patchViews();
removeLegacy();
console.log('migrate ec-eng -> ec-gijutsu done');
