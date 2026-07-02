// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：scripts
// 文件名称：patch-related-plant-company-column-desc.cjs
// 创建时间：2026-06-29
// 创建人：Takt365(Cursor AI)
// 功能描述：RelatedPlant/RelatedCompany：XML 注释保留关联说明，ColumnDescription 仅保留「关联工厂」「关联公司」
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const fs = require('fs');
const path = require('path');

const ENTITIES_ROOT = path.resolve(__dirname, '../backend/src/Takt.Domain/Entities');

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

/**
 * 修正单文件 related_plant / related_company 的 ColumnDescription 与 RelatedCompany 摘要
 * @param {string} filePath 实体路径
 * @returns {{ changed: boolean, entity?: string }}
 */
function patchFile(filePath) {
  let content = fs.readFileSync(filePath, 'utf-8');
  const before = content;
  content = content.replace(
    /ColumnDescription = "关联工厂（关联 TaktPlant\.PlantCode）"/g,
    'ColumnDescription = "关联工厂"',
  );
  content = content.replace(
    /ColumnDescription = "（关联 TaktPlant\.Pl关联工厂antCode）"/g,
    'ColumnDescription = "关联工厂"',
  );
  content = content.replace(
    /ColumnDescription = "关联工厂编码"/g,
    'ColumnDescription = "关联工厂"',
  );
  content = content.replace(
    /ColumnDescription = "关联公司（关联 TaktCompany\.CompanyCode）"/g,
    'ColumnDescription = "关联公司"',
  );
  content = content.replace(
    /ColumnDescription = "关联公司代码"/g,
    'ColumnDescription = "关联公司"',
  );
  if (content.includes('public string RelatedCompany')) {
    content = content.replace(
      /(\s*\/\/\/ <summary>\s*\r?\n\s*\/\/\/ )关联公司(\s*\r?\n\s*\/\/\/ <\/summary>\s*\r?\n\s*\[SugarColumn\(ColumnName = "related_company")/g,
      '$1关联公司（关联 TaktCompany.CompanyCode）$2',
    );
  }
  if (content === before) {
    return { changed: false };
  }
  fs.writeFileSync(filePath, content, 'utf-8');
  return { changed: true, entity: path.basename(filePath, '.cs') };
}

function main() {
  const files = walkEntityFiles(ENTITIES_ROOT);
  const updated = [];
  for (const file of files) {
    const result = patchFile(file);
    if (result.changed) {
      updated.push(result.entity);
    }
  }
  console.log(`Updated ${updated.length} entity file(s):`);
  updated.forEach((name) => console.log(`  - ${name}`));
}

main();
