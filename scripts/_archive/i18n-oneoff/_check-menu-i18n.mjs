import fs from 'fs';
import path from 'path';
import { fileURLToPath } from 'url';

const root = path.dirname(fileURLToPath(import.meta.url));
const menuI18n = fs.readFileSync(
  path.join(root, '../backend/src/Takt.Infrastructure/Data/Seeds/I18nSeedData/TaktMenuI18nSeedData.cs'),
  'utf8',
);
const re = /\("([^"]+)",\s*"(zh-CN|en-US|ja-JP|zh-HK)"/g;
const byKey = new Map();
let m;
while ((m = re.exec(menuI18n))) {
  const [k, lang] = m.slice(1);
  if (!byKey.has(k)) byKey.set(k, new Set());
  byKey.get(k).add(lang);
}
const langs = ['zh-CN', 'en-US', 'ja-JP', 'zh-HK'];
const missing = [];
for (const [k, s] of byKey) {
  for (const l of langs) {
    if (!s.has(l)) missing.push(`${k}:${l}`);
  }
}
console.log('keys', byKey.size);
for (const l of langs) {
  console.log(l, [...byKey.values()].filter((s) => s.has(l)).length);
}
console.log('missing', missing.length);
if (missing.length) console.log(missing.join('\n'));

// menu seed i18n keys
const menuFiles = fs
  .readdirSync(path.join(root, '../backend/src/Takt.Infrastructure/Data/Seeds/EntitySeedData'))
  .filter((f) => f.startsWith('TaktMenu') && f.endsWith('.cs'));
const menuKeys = new Set();
for (const f of menuFiles) {
  const content = fs.readFileSync(path.join(root, '../backend/src/Takt.Infrastructure/Data/Seeds/EntitySeedData', f), 'utf8');
  const km = content.matchAll(/I18nKey\s*=\s*"([^"]+)"/g);
  for (const x of km) menuKeys.add(x[1]);
}
const i18nKeys = new Set(byKey.keys());
const menuMissing = [...menuKeys].filter((k) => !i18nKeys.has(k)).sort();
const i18nExtra = [...i18nKeys].filter((k) => !menuKeys.has(k)).sort();
console.log('menu seed keys', menuKeys.size);
console.log('menu keys missing i18n', menuMissing.length);
if (menuMissing.length) console.log(menuMissing.join('\n'));
console.log('i18n keys not in menu', i18nExtra.length);
