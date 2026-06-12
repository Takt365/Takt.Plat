// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：scripts
// 文件名称：remove-xml-cref.cjs
// 创建时间：2026-06-11
// 创建人：Takt365(Cursor AI)
// 功能描述：全项目移除 XML/JSDoc 中的 see cref，转为纯文字（符合 00-project 注释规范）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const fs = require('fs');
const path = require('path');
const { stripSeeCref } = require('./xml-cref-strip.cjs');

const ROOT = path.resolve(__dirname, '..');
const TARGET_DIRS = [
  path.join(ROOT, 'backend', 'src'),
  path.join(ROOT, 'frontend', 'src'),
];

const SKIP_DIR_NAMES = new Set(['node_modules', '_build_out', 'bin', 'obj']);
const TARGET_EXT = new Set(['.cs', '.ts', '.vue', '.tsx', '.js', '.cjs']);

function walkDir(dir, onFile) {
  if (!fs.existsSync(dir)) {
    return;
  }
  for (const entry of fs.readdirSync(dir, { withFileTypes: true })) {
    const full = path.join(dir, entry.name);
    if (entry.isDirectory()) {
      if (SKIP_DIR_NAMES.has(entry.name)) {
        continue;
      }
      walkDir(full, onFile);
    } else {
      const ext = path.extname(entry.name);
      if (TARGET_EXT.has(ext)) {
        onFile(full);
      }
    }
  }
}

let changed = 0;
let remaining = 0;

for (const base of TARGET_DIRS) {
  walkDir(base, (filePath) => {
    const original = fs.readFileSync(filePath, 'utf8');
    if (!/see\s+cref/i.test(original)) {
      return;
    }
    const updated = stripSeeCref(original);
    if (updated !== original) {
      fs.writeFileSync(filePath, updated, 'utf8');
      changed += 1;
      console.log(path.relative(ROOT, filePath));
    }
    if (/see\s+cref/i.test(updated)) {
      remaining += 1;
      console.warn('STILL HAS CREF:', path.relative(ROOT, filePath));
    }
  });
}

console.log(`Done. ${changed} files updated. ${remaining} files still contain see cref.`);
