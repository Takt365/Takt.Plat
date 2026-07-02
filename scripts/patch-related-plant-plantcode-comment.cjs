// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：scripts
// 文件名称：patch-related-plant-plantcode-comment.cjs
// 创建时间：2026-06-29
// 创建人：Takt365(Cursor AI)
// 功能描述：实体 RelatedPlant/RelatedCompany：XML 摘要含关联实体与 options API（无 ParentId 不用 tree-options）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const fs = require('fs');
const path = require('path');

const ENTITIES_ROOT = path.resolve(__dirname, '../backend/src/Takt.Domain/Entities');
const TARGET_SUMMARY_PLANT = '/// 关联工厂（关联 TaktPlant.PlantCode，选项 TaktPlants/options）';
const TARGET_COLUMN_DESC_PLANT = '关联工厂';
const TARGET_SUMMARY_COMPANY = '/// 关联公司（关联 TaktCompany.CompanyCode，选项 TaktCompanies/options）';
const TARGET_COLUMN_DESC_COMPANY = '关联公司';

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

function patchFile(filePath) {
  let content = fs.readFileSync(filePath, 'utf-8');
  const before = content;
  if (content.includes('public string RelatedPlant')) {
    const blockRe = /(\s*\/\/\/ <summary>\s*\r?\n\s*\/\/\/ )[^\r\n]+(\s*\r?\n\s*\/\/\/ <\/summary>\s*\r?\n\s*\[SugarColumn\(ColumnName = "related_plant", ColumnDescription = ")[^"]+(")/g;
    content = content.replace(
      blockRe,
      `$1${TARGET_SUMMARY_PLANT.replace('/// ', '')}$2${TARGET_COLUMN_DESC_PLANT}$3`,
    );
  }
  if (content.includes('public string RelatedCompany')) {
    const blockRe = /(\s*\/\/\/ <summary>\s*\r?\n\s*\/\/\/ )[^\r\n]+(\s*\r?\n\s*\/\/\/ <\/summary>\s*\r?\n\s*\[SugarColumn\(ColumnName = "related_company", ColumnDescription = ")[^"]+(")/g;
    content = content.replace(
      blockRe,
      `$1${TARGET_SUMMARY_COMPANY.replace('/// ', '')}$2${TARGET_COLUMN_DESC_COMPANY}$3`,
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
