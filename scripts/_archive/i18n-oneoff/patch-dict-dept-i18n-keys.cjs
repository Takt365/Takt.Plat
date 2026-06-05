const fs = require('fs');
const path = require('path');

const dictSeedPath = path.join(
  __dirname,
  '../backend/src/Takt.Infrastructure/Data/Seeds/EntitySeedData/TaktDictDataSeedData.cs',
);

let content = fs.readFileSync(dictSeedPath, 'utf8');

const deptCodes = [
  'd1000', 'd0100', 'd0200', 'd0300', 'd0400', 'd0500', 'd0600', 'd0700', 'd0800', 'd0900',
  'd0110', 'd0210', 'd0310', 'd0410', 'd0420', 'd0430', 'd0510', 'd0610', 'd0620', 'd0630',
  'd0621', 'd0622', 'd0623', 'd0624', 'd0625', 'd0626', 'd0710', 'd0810', 'd0820', 'd0910', 'd0920',
];

let replaceCount = 0;
for (const code of deptCodes) {
  const costKey = `dict.accounting.cost.center.category.${code}`;
  const profitKey = `dict.accounting.profit.center.category.${code}`;
  const orgKey = `org.dept.${code}`;
  if (content.includes(costKey)) {
    content = content.split(costKey).join(orgKey);
    replaceCount += 1;
  }
  if (content.includes(profitKey)) {
    content = content.split(profitKey).join(orgKey);
    replaceCount += 1;
  }
}

fs.writeFileSync(dictSeedPath, content, 'utf8');
console.log(`Updated ${replaceCount} I18nKey references to org.dept.*`);
