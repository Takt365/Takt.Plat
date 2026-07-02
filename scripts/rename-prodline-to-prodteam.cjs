/**
 * ProdLine → ProdTeam（生产班组）全栈重命名
 * 用法：node scripts/rename-prodline-to-prodteam.cjs
 */
const fs = require('fs');
const path = require('path');

const root = path.resolve(__dirname, '..');

/** @type {{ from: RegExp; to: string }[]} */
const RULES = [
  { from: /prod_line_name/g, to: 'prod_team_name' },
  { from: /ProdLineName/g, to: 'ProdTeamName' },
  { from: /prodLineName/g, to: 'prodTeamName' },
  { from: /prodlinename/g, to: 'prodteamname' },
  { from: /_prod_line/g, to: '_prod_team' },
  { from: /prod_line/g, to: 'prod_team' },
  { from: /cell-prodLine/g, to: 'cell-prodTeam' },
  { from: /ProdLine/g, to: 'ProdTeam' },
  { from: /prodLine/g, to: 'prodTeam' },
  { from: /ColumnDescription = "生产线"/g, to: 'ColumnDescription = "生产班组"' },
  { from: /生产线（选项/g, to: '生产班组（选项' },
  { from: /生产线名称/g, to: '生产班组名称' },
  { from: /"生产线_us"/g, to: '"生产班组_us"' },
  { from: /"生产线_jp"/g, to: '"生产班组_jp"' },
  { from: /"生产线_hk"/g, to: '"生产班组_hk"' },
  { from: /"生产线", "生产/g, to: '"生产班组", "生产' },
  { from: /\/\/ 生产线/g, to: '// 生产班组' },
  { from: /\/\/ entity\.([a-z0-9]+)\.prodline/g, to: '// entity.$1.prodteam' },
  { from: /entity\.([a-z0-9]+)\.prodline/g, to: 'entity.$1.prodteam' },
  { from: /entity\.([a-z0-9]+)\.prodlinename/g, to: 'entity.$1.prodteamname' },
  { from: /、ProdTeam、/g, to: '、生产班组、' },
  { from: /ProdTeam已存在/g, to: '生产班组已存在' },
  { from: /、ProdTeam\)/g, to: '、生产班组)' },
  { from: /（ProdTeam）/g, to: '（生产班组）' },
  { from: /ProdTeam、ShiftNo/g, to: '生产班组、ShiftNo' },
  { from: /ProdTeam、TimeCategory/g, to: '生产班组、TimeCategory' },
];

/** @type {string[]} */
const TARGET_DIRS = [
  'backend/src/Takt.Domain/Entities/Logistics/Manufacturing',
  'backend/src/Takt.Application/Dtos/Logistics/Manufacturing',
  'backend/src/Takt.Application/Services/Logistics/Manufacturing',
  'backend/src/Takt.Application/Validators/Logistics/Manufacturing',
  'backend/src/Takt.Infrastructure/Data/Seeds/I18nSeedData/Logistics/Manufacturing',
  'frontend/src/types/logistics/manufacturing',
  'frontend/src/views/logistics/manufacturing',
];

function walk(dir, out = []) {
  if (!fs.existsSync(dir)) return out;
  for (const name of fs.readdirSync(dir)) {
    const full = path.join(dir, name);
    const stat = fs.statSync(full);
    if (stat.isDirectory()) walk(full, out);
    else if (/\.(cs|ts|vue)$/.test(name)) out.push(full);
  }
  return out;
}

let changed = 0;
for (const rel of TARGET_DIRS) {
  const abs = path.join(root, rel);
  for (const file of walk(abs)) {
    let text = fs.readFileSync(file, 'utf8');
    const orig = text;
    for (const rule of RULES) {
      text = text.replace(rule.from, rule.to);
    }
    if (text !== orig) {
      fs.writeFileSync(file, text, 'utf8');
      changed++;
      console.log('updated:', path.relative(root, file));
    }
  }
}
console.log(`done, ${changed} files updated`);
