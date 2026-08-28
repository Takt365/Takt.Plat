'use strict';

/**
 * 生成项目 UI 支持的 sys_culture_code 种子行
 * 当前仅：EN-US / JA-JP / ZH-CN / ZH-HK（与 TaktCultureSeedData 对齐）
 * 其余区域文化由 TaktDictDataSeedData 播种后软删除（IsDeleted=1）
 * 约定：
 * - DictLabel = 本族语语言名
 * - DictValue = BCP47 大写（如 JA-JP）
 * - Remark = 区域文化编码.{中文语言(中文地区)}
 * - ExtLabel = 英文语言名；ExtValue = JSON{countryCode,language}
 * 用法: node scripts/gen/build-sys-culture-code-seed.cjs
 */

const fs = require('fs');
const path = require('path');
const {
  annexare,
  escapeCs,
  resolveLanguage,
  resolveCultureRemarkZh,
  cultureLabelOverride,
} = require('./data/country-locale-common.cjs');

const languages = require(path.join(__dirname, 'data/languages-annexare.min.json'));
const seedFile = path.join(
  __dirname,
  '../../backend/src/Takt.Infrastructure/Data/Seeds/EntitySeedData/TaktDictDataSeedData.cs',
);

/** 与 TaktCultureSeedData / 前端 UI 语言一致 */
const UI_SUPPORTED_COUNTRY_CODES = ['US', 'JP', 'HK', 'CN'];

function resolveLangKey(bcp47) {
  const parts = String(bcp47 || '').split('-');
  return (parts[0] || 'en').toLowerCase();
}

function resolveCultureLabel(code, bcp47) {
  if (cultureLabelOverride[code]) return cultureLabelOverride[code];
  const langKey = resolveLangKey(bcp47);
  const lang = languages[langKey];
  const native = (lang && lang.native) || (lang && lang.name) || langKey;
  return `${native}(${code})`;
}

function resolveCultureExtLabel(bcp47) {
  const langKey = resolveLangKey(bcp47);
  const lang = languages[langKey];
  return (lang && lang.name) || langKey;
}

const rows = [];
for (const code of UI_SUPPORTED_COUNTRY_CODES) {
  const row = annexare[code];
  if (!row) {
    throw new Error(`Missing annexare country: ${code}`);
  }
  const enCountry = row.name || code;
  const bcp47 = resolveLanguage(code, row.languages);
  const dictValue = bcp47.toUpperCase();
  if (dictValue.length > 5) {
    throw new Error(`Culture DictValue too long (>5): ${dictValue} for ${code}`);
  }
  rows.push({
    code,
    enCountry,
    bcp47,
    dictValue,
    label: resolveCultureLabel(code, bcp47),
    remarkZh: resolveCultureRemarkZh(code, bcp47, enCountry),
    extLabel: resolveCultureExtLabel(bcp47),
    extValue: JSON.stringify({ countryCode: code, language: bcp47 }),
    isDefault: code === 'CN' ? 1 : 0,
    i18nKey: `dict.sys.culture.code.${bcp47.toLowerCase()}`,
  });
}

rows.sort((a, b) => a.dictValue.localeCompare(b.dictValue));

const lines = [];
let sort = 1;
for (const item of rows) {
  lines.push(
    `            ("sys_culture_code","${escapeCs(item.label)}","${item.dictValue}","${item.i18nKey}",1,1,${item.isDefault},"区域文化编码.${escapeCs(item.remarkZh)}",${sort},"mul","${escapeCs(item.extLabel)}","${escapeCs(item.extValue)}"),`,
  );
  sort += 1;
}

const block = lines.join('\n');
let text = fs.readFileSync(seedFile, 'utf8');
const marked =
  /(\s*)\/\/ === sys_culture_code BEGIN ===[\s\S]*?\/\/ === sys_culture_code END ===/;
if (marked.test(text)) {
  text = text.replace(
    marked,
    `$1// === sys_culture_code BEGIN ===\n${block}\n$1// === sys_culture_code END ===`,
  );
} else {
  const re2 =
    /(\s*)\("sys_culture_code",[\s\S]*?(?=\s*\("sys_data_scope")/;
  if (!re2.test(text)) {
    throw new Error('Cannot locate sys_culture_code block in TaktDictDataSeedData.cs');
  }
  text = text.replace(
    re2,
    `$1// === sys_culture_code BEGIN ===\n${block}\n$1// === sys_culture_code END ===\n`,
  );
}

fs.writeFileSync(seedFile, text);
console.log(
  `Generated ${rows.length} UI-supported sys_culture_code rows (${rows.map((r) => r.dictValue).join(', ')}) -> TaktDictDataSeedData.cs`,
);
