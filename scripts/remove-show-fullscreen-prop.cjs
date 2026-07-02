// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：scripts
// 文件名称：remove-show-fullscreen-prop.cjs
// 创建时间：2026-06-28
// 创建人：Takt365(Cursor AI)
// 功能描述：移除 TaktModal 已废弃的 :show-fullscreen 属性（全屏按钮改为始终显示）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const fs = require('fs');
const path = require('path');

const ROOT = path.join(__dirname, '..');
const TARGET_DIRS = [
  path.join(ROOT, 'frontend', 'src'),
  path.join(ROOT, 'scripts'),
  path.join(ROOT, 'backend', 'src', 'Takt.WebApi', 'wwwroot', 'Generator'),
];

const SHOW_FULLSCREEN_RE = /\s*:show-fullscreen="(?:true|false)"/g;

/**
 * 递归收集目录下 .vue / .cjs / .sbn 文件
 * @param {string} dir 目录
 * @returns {string[]}
 */
function collectFiles(dir) {
  if (!fs.existsSync(dir)) {
    return [];
  }
  const entries = fs.readdirSync(dir, { withFileTypes: true });
  const files = [];
  for (const entry of entries) {
    const fullPath = path.join(dir, entry.name);
    if (entry.isDirectory()) {
      if (entry.name === 'node_modules' || entry.name === 'dist') {
        continue;
      }
      files.push(...collectFiles(fullPath));
      continue;
    }
    if (/\.(vue|cjs|sbn)$/.test(entry.name)) {
      files.push(fullPath);
    }
  }
  return files;
}

let changed = 0;
for (const dir of TARGET_DIRS) {
  for (const filePath of collectFiles(dir)) {
    const original = fs.readFileSync(filePath, 'utf8');
    const next = original.replace(SHOW_FULLSCREEN_RE, '');
    if (next !== original) {
      fs.writeFileSync(filePath, next, 'utf8');
      changed += 1;
      console.log(`updated: ${path.relative(ROOT, filePath)}`);
    }
  }
}
console.log(`done, ${changed} file(s) updated.`);
