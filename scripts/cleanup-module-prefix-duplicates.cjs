// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：scripts
// 文件名称：cleanup-module-prefix-duplicates.cjs
// 创建时间：2026-06-20
// 创建人：Takt365(Cursor AI)
// 功能描述：删除 api/types/views 中与 modulePath 路径段重复的带前缀副本（保留 strip 后短名）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const fs = require('fs');
const path = require('path');
const { stripModulePrefixFromEntityKebab } = require('./generate-script-common.cjs');

const REPO_ROOT = path.resolve(__dirname, '..');
const FRONTEND_ROOT = path.join(REPO_ROOT, 'frontend', 'src');

/**
 * 递归删除目录
 * @param {string} target
 */
function removeDirRecursive(target) {
  if (!fs.existsSync(target)) {
    return;
  }
  fs.rmSync(target, { recursive: true, force: true });
}

/**
 * 清理单模块下带重复路径前缀的文件/目录
 * @param {string} modulePath 如 logistics/quality/cost
 * @param {'api'|'types'|'views'} layer
 * @param {boolean} dryRun
 * @returns {string[]}
 */
function cleanupLayer(modulePath, layer, dryRun) {
  const root = path.join(FRONTEND_ROOT, layer, modulePath);
  if (!fs.existsSync(root)) {
    return [];
  }
  /** @type {string[]} */
  const removed = [];
  const entries = fs.readdirSync(root, { withFileTypes: true });
  entries.forEach((entry) => {
    const full = path.join(root, entry.name);
    const baseName = entry.isDirectory() ? entry.name : entry.name.replace(/\.d\.ts$|\.ts$/, '');
    const stripped = stripModulePrefixFromEntityKebab(baseName, modulePath);
    if (stripped === baseName) {
      return;
    }
    if (layer === 'views') {
      removed.push(path.relative(REPO_ROOT, full).replace(/\\/g, '/'));
      if (!dryRun) {
        if (entry.isDirectory()) {
          removeDirRecursive(full);
        } else {
          fs.unlinkSync(full);
        }
      }
      return;
    }
    const strippedPath = path.join(root, entry.name.endsWith('.d.ts') ? `${stripped}.d.ts` : `${stripped}.ts`);
    if (!fs.existsSync(strippedPath)) {
      return;
    }
    removed.push(path.relative(REPO_ROOT, full).replace(/\\/g, '/'));
    if (!dryRun) {
      fs.unlinkSync(full);
    }
  });
  return removed;
}

function printUsage() {
  console.log(`
用法: node scripts/cleanup-module-prefix-duplicates.cjs --module <path> [--dry-run]

示例:
  node scripts/cleanup-module-prefix-duplicates.cjs --module logistics/quality/cost
  node scripts/cleanup-module-prefix-duplicates.cjs --module logistics/quality/cost --dry-run
`);
}

const args = process.argv.slice(2);
const dryRun = args.includes('--dry-run');
const moduleIdx = args.indexOf('--module');
if (moduleIdx < 0 || !args[moduleIdx + 1]) {
  printUsage();
  process.exit(1);
}
const modulePath = args[moduleIdx + 1].replace(/\\/g, '/').toLowerCase();

console.log(`${dryRun ? '🔍 [dry-run]' : '🧹'} 清理模块 ${modulePath} 中带路径重复前缀的 api/types/views 副本\n`);

['api', 'types', 'views'].forEach((layer) => {
  const removed = cleanupLayer(modulePath, layer, dryRun);
  if (!removed.length) {
    console.log(`  ${layer}: 无待删副本`);
    return;
  }
  removed.forEach((rel) => console.log(`  ${dryRun ? '将删除' : '已删除'} ${rel}`));
});

console.log('\n完成');
