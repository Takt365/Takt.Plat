// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：scripts
// 文件名称：patch-related-plant-i18n-options.cjs
// 创建时间：2026-06-29
// 创建人：Takt365(Cursor AI)
// 功能描述：entity.*.relatedplant i18n ContextNote：PlantCode + TaktPlants/options（非树形）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const fs = require('fs');
const path = require('path');

const I18N_ROOT = path.resolve(__dirname, '../backend/src/Takt.Infrastructure/Data/Seeds/I18nSeedData');
const CONTEXT = '关联工厂（关联 TaktPlant.PlantCode，选项 TaktPlants/options）';

function walkFiles(dir, acc = []) {
  for (const entry of fs.readdirSync(dir, { withFileTypes: true })) {
    const full = path.join(dir, entry.name);
    if (entry.isDirectory()) {
      walkFiles(full, acc);
    } else if (entry.name.endsWith('I18nSeedData.cs')) {
      acc.push(full);
    }
  }
  return acc;
}

function patchFile(filePath) {
  let content = fs.readFileSync(filePath, 'utf-8');
  if (!content.includes('.relatedplant')) {
    return false;
  }
  const before = content;
  content = content.replace(
    /new TranslationSeedItem\("(entity\.[^"]+\.relatedplant)", "([^"]+)", "([^"]+)", "[^"]*"\)/g,
    (_m, key, culture, label) => {
      const base = label.replace(/_us$|_jp$|_hk$/, '').replace(/关联工厂编码/g, '关联工厂');
      const suffix = label.endsWith('_us') ? '_us' : label.endsWith('_jp') ? '_jp' : label.endsWith('_hk') ? '_hk' : '';
      const text = suffix ? `${base}${suffix}` : base;
      return `new TranslationSeedItem("${key}", "${culture}", "${text}", "${CONTEXT}")`;
    },
  );
  if (content === before) {
    return false;
  }
  fs.writeFileSync(filePath, content, 'utf-8');
  return true;
}

function main() {
  const files = walkFiles(I18N_ROOT);
  let count = 0;
  for (const file of files) {
    if (patchFile(file)) {
      count += 1;
      console.log(path.relative(I18N_ROOT, file));
    }
  }
  console.log(`Patched ${count} i18n seed file(s).`);
}

main();
