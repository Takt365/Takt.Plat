// 按菜单分段 + 四语分块重组 TaktMenuI18nSeedData.cs
const fs = require('fs');
const path = require('path');

const TARGET = path.join(
  __dirname,
  '../backend/src/Takt.Infrastructure/Data/Seeds/I18nSeedData/TaktMenuI18nSeedData.cs',
);

const ENTRY_RE =
  /^\s*\("([^"]+)",\s*"(zh-CN|en-US|ja-JP|zh-HK)"\s*,\s*"((?:[^"\\]|\\.)*)"\s*,\s*"((?:[^"\\]|\\.)*)"\s*\),?\s*$/;

const LANGS = ['zh-CN', 'en-US', 'ja-JP', 'zh-HK'];
const LANG_LABEL = {
  'zh-CN': '简体中文 (zh-CN)',
  'en-US': '英文 (en-US)',
  'ja-JP': '日文 (ja-JP)',
  'zh-HK': '香港繁体 (zh-HK)',
};

function escapeCsharp(s) {
  return (s || '').replace(/\\/g, '\\\\').replace(/"/g, '\\"');
}

function parse(content) {
  const lines = content.split(/\r?\n/);
  const sections = [];
  let current = null;
  let pendingTitle = false;

  for (let i = 0; i < lines.length; i++) {
    const line = lines[i];
    if (/^\s*\/\/\s*=+\s*$/.test(line)) {
      pendingTitle = true;
      continue;
    }
    if (pendingTitle && /^\s*\/\/\s*.+/.test(line) && !/^\s*\/\/\s*=+/.test(line)) {
      if (current && current.entries.length > 0) {
        sections.push(current);
      }
      current = { title: line.replace(/^\s*\/\/\s*/, '').trim(), entries: [] };
      pendingTitle = false;
      continue;
    }
    pendingTitle = false;

    const m = line.match(ENTRY_RE);
    if (m && current) {
      current.entries.push({
        key: m[1],
        culture: m[2],
        text: m[3],
        note: m[4],
      });
    }
  }
  if (current && current.entries.length > 0) {
    sections.push(current);
  }
  return { lines, sections };
}

function buildListBody(sections) {
  const out = [];
  for (const section of sections) {
    out.push('            // ========================================');
    out.push(`            // ${section.title}`);
    out.push('            // ========================================');
    out.push('');

    const byKey = new Map();
    for (const e of section.entries) {
      if (!byKey.has(e.key)) {
        byKey.set(e.key, new Map());
      }
      byKey.get(e.key).set(e.culture, e);
    }

    const keys = [...byKey.keys()];
    const zhOrder = section.entries.filter((e) => e.culture === 'zh-CN').map((e) => e.key);
    const orderedKeys = [...new Set([...zhOrder, ...keys])];

    for (const lang of LANGS) {
      out.push(`            // ${LANG_LABEL[lang]}`);
      for (const key of orderedKeys) {
        const row = byKey.get(key)?.get(lang);
        if (!row) {
          throw new Error(`缺少翻译: [${section.title}] ${key} / ${lang}`);
        }
        out.push(
          `            ("${row.key}", "${row.culture}", "${row.text}", "${row.note}"),`,
        );
      }
      out.push('');
    }
  }
  return out.join('\n').replace(/\n+$/, '\n');
}

function run() {
  const content = fs.readFileSync(TARGET, 'utf8');
  const start = content.indexOf('return new List<(string, string, string, string?)>');
  const openBrace = content.indexOf('{', start);
  const closeBrace = content.lastIndexOf('        };');
  if (start < 0 || openBrace < 0 || closeBrace < 0) {
    throw new Error('无法定位 GetStandardMenuTranslations 列表体');
  }

  const { sections } = parse(content);
  const body = buildListBody(sections);
  const next =
    content.slice(0, openBrace + 1) +
    '\n' +
    body +
    content.slice(closeBrace);

  fs.writeFileSync(TARGET, next, 'utf8');
  console.log(`已重组 ${sections.length} 个菜单分段，四语分块完成。`);
}

run();
