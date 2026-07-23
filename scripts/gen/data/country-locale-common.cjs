'use strict';

/**
 * 国家/区域文化种子共用：语言码、中文名、转义
 */

const path = require('path');

const annexare = require(path.join(__dirname, 'countries-annexare.min.json'));
const zhMap = require(path.join(__dirname, 'countries-zh-CN.json'));

/** 中文简称覆盖 */
const zhOverride = {
  CN: '中国',
  HK: '香港',
  MO: '澳门',
  TW: '台湾',
  US: '美国',
  GB: '英国',
  KR: '韩国',
  KP: '朝鲜',
  RU: '俄罗斯',
  AE: '阿联酋',
  AC: '阿森松岛',
  TA: '特里斯坦-达库尼亚',
  XK: '科索沃',
};

/** 语言 BCP47 覆盖（与国家 ExtValue.language / 文化 DictValue 对齐） */
const languageOverride = {
  CN: 'zh-CN',
  HK: 'zh-HK',
  MO: 'zh-MO',
  TW: 'zh-TW',
  JP: 'ja-JP',
  US: 'en-US',
  GB: 'en-GB',
  AU: 'en-AU',
  NZ: 'en-NZ',
  CA: 'en-CA',
  IE: 'en-IE',
  SG: 'zh-SG',
  IN: 'en-IN',
  DE: 'de-DE',
  AT: 'de-AT',
  CH: 'de-CH',
  FR: 'fr-FR',
  BE: 'nl-BE',
  NL: 'nl-NL',
  ES: 'es-ES',
  MX: 'es-MX',
  PT: 'pt-PT',
  BR: 'pt-BR',
  IT: 'it-IT',
  KR: 'ko-KR',
  TH: 'th-TH',
  VN: 'vi-VN',
  MY: 'ms-MY',
  ID: 'id-ID',
  PH: 'en-PH',
  SA: 'ar-SA',
  AE: 'ar-AE',
  EG: 'ar-EG',
  TR: 'tr-TR',
  RU: 'ru-RU',
  PL: 'pl-PL',
  SE: 'sv-SE',
  NO: 'nb-NO',
  DK: 'da-DK',
  FI: 'fi-FI',
  GR: 'el-GR',
  IL: 'he-IL',
  CZ: 'cs-CZ',
  HU: 'hu-HU',
  RO: 'ro-RO',
  UA: 'uk-UA',
  EH: 'ar-EH',
  GL: 'kl-GL',
};

/** 中文区域文化备注覆盖（字典 Remark 全文后缀，不含前缀「区域文化编码.」） */
const cultureRemarkZhOverride = {
  CN: '中文(简体)',
  HK: '中文(香港)',
  MO: '中文(澳门)',
  TW: '中文(繁體)',
  SG: '中文(新加坡)',
};

/** ISO 639-1 → 中文语言名（备注用） */
const languageZhMap = {
  af: '阿非利堪斯语',
  am: '阿姆哈拉语',
  ar: '阿拉伯语',
  ay: '艾马拉语',
  az: '阿塞拜疆语',
  be: '白俄罗斯语',
  bg: '保加利亚语',
  bi: '比斯拉马语',
  bn: '孟加拉语',
  bs: '波斯尼亚语',
  ca: '加泰罗尼亚语',
  ch: '查莫罗语',
  cs: '捷克语',
  da: '丹麦语',
  de: '德语',
  dv: '迪维希语',
  dz: '宗卡语',
  el: '希腊语',
  en: '英语',
  es: '西班牙语',
  et: '爱沙尼亚语',
  eu: '巴斯克语',
  fa: '波斯语',
  ff: '富拉语',
  fi: '芬兰语',
  fj: '斐济语',
  fo: '法罗语',
  fr: '法语',
  ga: '爱尔兰语',
  gl: '加利西亚语',
  gn: '瓜拉尼语',
  gv: '马恩岛语',
  he: '希伯来语',
  hi: '印地语',
  hr: '克罗地亚语',
  ht: '海地克里奥尔语',
  hu: '匈牙利语',
  hy: '亚美尼亚语',
  id: '印尼语',
  is: '冰岛语',
  it: '意大利语',
  ja: '日语',
  ka: '格鲁吉亚语',
  kg: '刚果语',
  kk: '哈萨克语',
  kl: '格陵兰语',
  km: '高棉语',
  ko: '韩语',
  ku: '库尔德语',
  ky: '吉尔吉斯语',
  la: '拉丁语',
  lb: '卢森堡语',
  ln: '林加拉语',
  lo: '老挝语',
  lt: '立陶宛语',
  lu: '卢巴语',
  lv: '拉脱维亚语',
  mg: '马达加斯加语',
  mh: '马绍尔语',
  mi: '毛利语',
  mk: '马其顿语',
  mn: '蒙古语',
  ms: '马来语',
  mt: '马耳他语',
  my: '缅甸语',
  na: '瑙鲁语',
  nb: '书面挪威语',
  nd: '北恩德贝勒语',
  ne: '尼泊尔语',
  nl: '荷兰语',
  nn: '新挪威语',
  no: '挪威语',
  nr: '南恩德贝勒语',
  ny: '齐切瓦语',
  oc: '奥克语',
  pa: '旁遮普语',
  pl: '波兰语',
  ps: '普什图语',
  pt: '葡萄牙语',
  qu: '克丘亚语',
  rn: '基隆迪语',
  ro: '罗马尼亚语',
  ru: '俄语',
  rw: '卢旺达语',
  sg: '桑戈语',
  si: '僧伽罗语',
  sk: '斯洛伐克语',
  sl: '斯洛文尼亚语',
  sm: '萨摩亚语',
  sn: '绍纳语',
  so: '索马里语',
  sq: '阿尔巴尼亚语',
  sr: '塞尔维亚语',
  ss: '斯瓦特语',
  st: '南索托语',
  sv: '瑞典语',
  sw: '斯瓦希里语',
  ta: '泰米尔语',
  tg: '塔吉克语',
  th: '泰语',
  ti: '提格利尼亚语',
  tk: '土库曼语',
  tl: '他加禄语',
  tn: '茨瓦纳语',
  to: '汤加语',
  tr: '土耳其语',
  ts: '聪加语',
  uk: '乌克兰语',
  ur: '乌尔都语',
  uz: '乌兹别克语',
  ve: '文达语',
  vi: '越南语',
  xh: '科萨语',
  zh: '中文',
  zu: '祖鲁语',
};

/** DictLabel 覆盖（本族语展示，对齐历史种子） */
const cultureLabelOverride = {
  CN: '中文(简体)',
  HK: '中文(香港)',
  MO: '中文(澳门)',
  TW: '中文(繁體)',
  SG: '中文(新加坡)',
  US: 'English(US)',
  GB: 'English(UK)',
  AU: 'English(AU)',
  JP: '日本語(JP)',
};

function escapeCs(s) {
  return String(s ?? '')
    .replace(/\\/g, '\\\\')
    .replace(/"/g, '\\"');
}

function resolveLanguage(code, languages) {
  if (languageOverride[code]) return languageOverride[code];
  const lang = Array.isArray(languages) && languages.length ? languages[0] : 'en';
  return `${lang}-${code}`;
}

function resolveZhCountryName(code, enName) {
  if (zhOverride[code]) return zhOverride[code];
  if (zhMap[code]) return zhMap[code];
  return enName || code;
}

/**
 * 区域文化备注中文：语言(地区)，如 日语(日本)、英语(美国)
 * @param {string} code 国家码
 * @param {string} bcp47 如 ja-JP
 * @param {string} enCountryName 英文国名（缺中文时回退）
 */
function resolveCultureRemarkZh(code, bcp47, enCountryName) {
  if (cultureRemarkZhOverride[code]) {
    return cultureRemarkZhOverride[code];
  }
  const langKey = String(bcp47 || '').split('-')[0].toLowerCase();
  const langZh = languageZhMap[langKey] || langKey;
  const countryZh = resolveZhCountryName(code, enCountryName);
  return `${langZh}(${countryZh})`;
}

function resolvePhone(phone) {
  if (!Array.isArray(phone) || phone.length === 0) return '';
  return `+${phone[0]}`;
}

function resolveCurrency(currency) {
  if (!Array.isArray(currency) || currency.length === 0) return '';
  return String(currency[0]);
}

function getSortedCountryCodes() {
  return Object.keys(annexare).sort((a, b) => a.localeCompare(b));
}

module.exports = {
  annexare,
  zhMap,
  zhOverride,
  languageOverride,
  languageZhMap,
  cultureRemarkZhOverride,
  cultureLabelOverride,
  escapeCs,
  resolveLanguage,
  resolveZhCountryName,
  resolveCultureRemarkZh,
  resolvePhone,
  resolveCurrency,
  getSortedCountryCodes,
};
