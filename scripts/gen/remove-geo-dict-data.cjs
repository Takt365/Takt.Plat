// ========================================
// 临时脚本：移除已废弃的 sys_country/region/city_code 字典数据行
// 用法: node scripts/gen/remove-geo-dict-data.cjs
// ========================================
const fs = require('fs');
const path = require('path');

const filePath = path.join(
  __dirname,
  '../../backend/src/Takt.Infrastructure/Data/Seeds/EntitySeedData/TaktDictDataSeedData.cs',
);
let content = fs.readFileSync(filePath, 'utf8');
const lineRe = /^\s*\("(sys_country_code|sys_region_code|sys_city_code)",.*$/gm;
const matches = content.match(lineRe) || [];
content = content.replace(lineRe, '');
content = content.replace(/\n{3,}/g, '\n\n');
fs.writeFileSync(filePath, content);
console.log(`Removed ${matches.length} geo dict data lines from TaktDictDataSeedData.cs`);
