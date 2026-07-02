// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：scripts
// 文件名称：fix-related-plant-non-nullable.cjs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：RelatedPlant 统一为非空 string（= string.Empty），列 IsNullable=false 且无 DefaultValue
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const fs = require('fs');
const path = require('path');

const ENTITIES_ROOT = path.resolve(__dirname, '../backend/src/Takt.Domain/Entities');
const RELATED_PLANT_PROP = 'public string RelatedPlant { get; set; } = string.Empty;';

function walkEntityFiles(dir, acc = []) {
  for (const entry of fs.readdirSync(dir, { withFileTypes: true })) {
    const full = path.join(dir, entry.name);
    if (entry.isDirectory()) {
      walkEntityFiles(full, acc);
    } else if (entry.name.endsWith('.cs') && entry.name.startsWith('Takt')) {
      acc.push(full);
    }
  }
  return acc;
}

function fixFile(filePath) {
  let content = fs.readFileSync(filePath, 'utf-8');
  if (!content.includes('RelatedPlant')) {
    return { changed: false };
  }
  const before = content;
  content = content.replace(
    /\[SugarColumn\(ColumnName = "related_plant"[^\]]*IsNullable = true[^\]]*\)\]/g,
    (m) => m.replace('IsNullable = true', 'IsNullable = false').replace(/, DefaultValue = "[^"]*"/g, ''),
  );
  content = content.replace(
    /\[SugarColumn\(ColumnName = "related_plant"[^\]]*DefaultValue = "[^"]*"[^\]]*\)\]/g,
    (m) => m.replace(/, DefaultValue = "[^"]*"/g, ''),
  );
  content = content.replace(
    /public string\? RelatedPlant \{ get; set; \}(?:\s*=\s*[^;\r\n]+)?;?/g,
    RELATED_PLANT_PROP,
  );
  content = content.replace(
    /public string RelatedPlant \{ get; set; \}(?:\s*=\s*[^;\r\n]+)?;?/g,
    RELATED_PLANT_PROP,
  );
  if (content === before) {
    return { changed: false };
  }
  fs.writeFileSync(filePath, content, 'utf-8');
  return { changed: true, entity: path.basename(filePath, '.cs').replace(/^Takt/, '') };
}

function main() {
  const changed = [];
  for (const file of walkEntityFiles(ENTITIES_ROOT)) {
    const result = fixFile(file);
    if (result.changed) {
      changed.push(result.entity);
    }
  }
  console.log(`Fixed ${changed.length} entity file(s).`);
  changed.forEach((name) => console.log(`  ${name}`));
}

main();
