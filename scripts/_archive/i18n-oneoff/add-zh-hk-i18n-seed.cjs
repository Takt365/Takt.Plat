// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：scripts
// 文件名称：add-zh-hk-i18n-seed.cjs
// 功能描述：为手工 i18n 种子 C# 文件按 zh-CN 补全 zh-HK 行（OpenCC 简→港繁）
// ========================================

const fs = require('fs');
const path = require('path');

let OpenCC;
try {
  OpenCC = require('opencc-js');
} catch {
  console.error('请先安装依赖: npm install opencc-js --prefix scripts');
  process.exit(1);
}

const converter = OpenCC.Converter({ from: 'cn', to: 'hk' });

const ENTRY_RE =
  /^(\s*)(?:new TranslationSeedItem)?\(\s*"([^"]+)"\s*,\s*"(zh-CN|en-US|ja-JP|zh-HK)"\s*,\s*"((?:[^"\\]|\\.)*)"\s*,\s*"((?:[^"\\]|\\.)*)"\s*\),?\s*$/;

function unescapeCsharp(s) {
  return s.replace(/\\"/g, '"').replace(/\\\\/g, '\\');
}

function escapeCsharp(s) {
  return (s || '').replace(/\\/g, '\\\\').replace(/"/g, '\\"');
}

function parseEntries(content) {
  const lines = content.split(/\r?\n/);
  const entries = [];
  for (let i = 0; i < lines.length; i++) {
    const m = lines[i].match(ENTRY_RE);
    if (!m) continue;
    const isNewForm = lines[i].includes('new TranslationSeedItem(');
    entries.push({
      lineIndex: i,
      indent: m[1],
      i18nKey: m[2],
      culture: m[3],
      text: unescapeCsharp(m[4]),
      note: unescapeCsharp(m[5]),
      trailingComma: m[6],
      isNewForm,
    });
  }
  return { lines, entries };
}

function toSimplifiedTraditional(text) {
  return converter(text);
}

function buildZhHkLine(entry, zhCnText) {
  const hkText = toSimplifiedTraditional(zhCnText);
  const text = escapeCsharp(hkText);
  const note = escapeCsharp(entry.note);
  if (entry.isNewForm) {
    return `${entry.indent}new TranslationSeedItem("${entry.i18nKey}", "zh-HK", "${text}", "${note}"),`;
  }
  return `${entry.indent}("${entry.i18nKey}", "zh-HK", "${text}", "${note}"),`;
}

function processFile(filePath) {
  const content = fs.readFileSync(filePath, 'utf8');
  const { lines, entries } = parseEntries(content);
  const zhByKey = new Map();
  const hasHk = new Set();
  const hkByKey = new Map();
  for (const e of entries) {
    if (e.culture === 'zh-CN') zhByKey.set(e.i18nKey, e);
    if (e.culture === 'zh-HK') {
      hasHk.add(e.i18nKey);
      hkByKey.set(e.i18nKey, e);
    }
  }

  let updated = 0;
  const patchedLines = [...lines];
  for (const [key, hkEntry] of hkByKey) {
    const zh = zhByKey.get(key);
    if (!zh || hkEntry.text.trim() !== '') continue;
    patchedLines[hkEntry.lineIndex] = buildZhHkLine(hkEntry, zh.text);
    updated++;
  }

  const insertions = [];
  for (const e of entries) {
    if (e.culture !== 'ja-JP') continue;
    if (hasHk.has(e.i18nKey)) continue;
    const zh = zhByKey.get(e.i18nKey);
    if (!zh) continue;
    insertions.push({
      afterLine: e.lineIndex,
      line: buildZhHkLine({ ...e, note: zh.note, isNewForm: zh.isNewForm, indent: zh.indent }, zh.text),
    });
    hasHk.add(e.i18nKey);
  }

  if (insertions.length === 0 && updated === 0) {
    console.log('  已含完整 zh-HK，跳过');
    return { added: 0, updated: 0 };
  }

  insertions.sort((a, b) => b.afterLine - a.afterLine);
  const newLines = patchedLines;
  for (const ins of insertions) {
    newLines.splice(ins.afterLine + 1, 0, ins.line);
  }

  let updatedContent = newLines.join('\n');
  updatedContent = updatedContent.replace(
    /(\d+)\s*种语言/g,
    (m, n) => `${Number(n) + (Number(n) === 3 ? 1 : 0)} 种语言`
  );
  updatedContent = updatedContent.replace(
    /英、日、中三语/g,
    '英、日、中、港繁四语'
  );
  updatedContent = updatedContent.replace(
    /英日中三语/g,
    '英日中港繁四语'
  );
  updatedContent = updatedContent.replace(
    /× 3种语言/g,
    '× 4种语言'
  );
  updatedContent = updatedContent.replace(
    /× 3 种语言/g,
    '× 4 种语言'
  );
  updatedContent = updatedContent.replace(
    /3种语言/g,
    '4种语言'
  );

  fs.writeFileSync(filePath, updatedContent, 'utf8');
  if (insertions.length > 0) console.log(`  新增 zh-HK: ${insertions.length} 条`);
  if (updated > 0) console.log(`  更新空 zh-HK: ${updated} 条`);
  return { added: insertions.length, updated };
}

const root = path.resolve(__dirname, '..');
const targets = [
  'backend/src/Takt.Infrastructure/Data/Seeds/I18nSeedData/TaktCommonI18nSeedData.cs',
  'backend/src/Takt.Infrastructure/Data/Seeds/I18nSeedData/TaktDeptI18nSeedData.cs',
  'backend/src/Takt.Infrastructure/Data/Seeds/I18nSeedData/TaktGreetingsI18nSeedData.cs',
  'backend/src/Takt.Infrastructure/Data/Seeds/I18nSeedData/TaktMenuI18nSeedData.cs',
  'backend/src/Takt.Infrastructure/Data/Seeds/I18nSeedData/TaktQuotesI18nSeedData.cs',
];

console.log('补全 zh-HK 翻译种子...\n');
let total = 0;
for (const rel of targets) {
  const filePath = path.join(root, rel);
  console.log(rel);
  const r = processFile(filePath);
  total += r.added + r.updated;
}
console.log(`\n完成，共处理 ${total} 条。`);
