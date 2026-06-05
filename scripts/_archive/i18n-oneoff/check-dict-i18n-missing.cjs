const fs = require('fs');
const path = require('path');

const dictSeedPath = path.join(
  __dirname,
  '../backend/src/Takt.Infrastructure/Data/Seeds/EntitySeedData/TaktDictDataSeedData.cs',
);
const content = fs.readFileSync(dictSeedPath, 'utf8');
const lineRegex =
  /\("([^"]*)","([^"]*)","([^"]*)","(dict\.[^"]*)"[^)]*?"([^"]*)",\s*\d+\),/g;

const parsed = new Set();
let match;
while ((match = lineRegex.exec(content)) !== null) {
  parsed.add(match[4]);
}

const all = [...content.matchAll(/"(dict\.[^"]+)"/g)].map((x) => x[1]);
const uniqueAll = [...new Set(all)];
const missing = uniqueAll.filter((k) => !parsed.has(k));

console.log('source', uniqueAll.length, 'parsed', parsed.size, 'missing', missing.length);
missing.forEach((k) => {
  const idx = content.indexOf(`"${k}"`);
  console.log(k);
  console.log(content.slice(Math.max(0, idx - 120), idx + 120));
  console.log('---');
});
