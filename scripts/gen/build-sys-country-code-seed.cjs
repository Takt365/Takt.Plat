'use strict';

/**
 * 从 annexare + zh_CN 国家列表生成完整 sys_country_code 种子行并写入 TaktDictDataSeedData.cs
 * 约定：
 * - DictLabel = 本国语言官方名称（native）
 * - DictValue = ISO 3166-1 alpha-2
 * - Remark = 国家地区.{中文名}
 * - ExtLabel = 英文名；ExtValue = phoneCode/currency/language JSON
 * 用法: node scripts/gen/build-sys-country-code-seed.cjs
 */

const fs = require('fs');
const path = require('path');
const {
  annexare,
  escapeCs,
  resolveLanguage,
  resolveZhCountryName,
  resolvePhone,
  resolveCurrency,
  getSortedCountryCodes,
} = require('./data/country-locale-common.cjs');

const seedFile = path.join(
  __dirname,
  '../../backend/src/Takt.Infrastructure/Data/Seeds/EntitySeedData/TaktDictDataSeedData.cs',
);

const codes = getSortedCountryCodes();
const lines = [];
let sort = 1;
for (const code of codes) {
  const row = annexare[code];
  const enName = row.name || code;
  const nativeLabel = ((row.native || '').trim() || enName || code);
  const zhName = resolveZhCountryName(code, enName);
  const phoneCode = resolvePhone(row.phone);
  const currency = resolveCurrency(row.currency);
  const language = resolveLanguage(code, row.languages);
  const extValue = JSON.stringify({ phoneCode, currency, language });
  const isDefault = code === 'CN' ? 1 : 0;
  const i18nKey = `dict.sys.country.code.${code.toLowerCase()}`;
  lines.push(
    `            ("sys_country_code","${escapeCs(nativeLabel)}","${code}","${i18nKey}",1,1,${isDefault},"国家地区.${escapeCs(zhName)}",${sort},"mul","${escapeCs(enName)}","${escapeCs(extValue)}"),`,
  );
  sort += 1;
}

const block = lines.join('\n');
let text = fs.readFileSync(seedFile, 'utf8');
const marked =
  /(\s*)\/\/ === sys_country_code BEGIN ===[\s\S]*?\/\/ === sys_country_code END ===/;
if (marked.test(text)) {
  text = text.replace(
    marked,
    `$1// === sys_country_code BEGIN ===\n${block}\n$1// === sys_country_code END ===`,
  );
} else {
  const re2 =
    /(\s*)\("sys_country_code",[\s\S]*?(?=\s*\("sys_culture_code"|\/\/ === sys_culture_code)/;
  if (!re2.test(text)) {
    throw new Error('Cannot locate sys_country_code block in TaktDictDataSeedData.cs');
  }
  text = text.replace(
    re2,
    `$1// === sys_country_code BEGIN ===\n${block}\n$1// === sys_country_code END ===\n`,
  );
}

fs.writeFileSync(seedFile, text);
console.log(`Generated ${codes.length} sys_country_code rows -> TaktDictDataSeedData.cs`);
