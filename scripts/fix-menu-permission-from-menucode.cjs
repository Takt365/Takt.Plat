// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：scripts
// 文件名称：fix-menu-permission-from-menucode.cjs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：按 MenuCode 分段逐块修正菜单种子页面 :list Permission
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const fs = require('fs');
const path = require('path');
const { parseAllMenuSeedBlocks, BACKEND_ROOT } = require('./audit-permission-scan.cjs');
const { expectedListPermissionFromMenuCode } = require('./menu-field-structure.cjs');

const SEEDS_DIR = path.join(BACKEND_ROOT, 'Takt.Infrastructure/Data/Seeds/EntitySeedData');

/**
 * @param {string} content
 * @returns {{ content: string, fixCount: number }}
 */
function fixPermissionsInSeedFileContent(content) {
  const marker = 'CreateOrUpdateMenuAsync(';
  const parts = content.split(marker);
  if (parts.length <= 1) {
    return { content, fixCount: 0 };
  }
  let fixCount = 0;
  const out = [parts[0]];
  for (let i = 1; i < parts.length; i += 1) {
    let block = parts[i];
    const menuCodeMatch = block.match(/menu\.MenuCode\s*=\s*"([^"]+)"/);
    const menuTypeMatch = block.match(/menu\.MenuType\s*=\s*(\d+)/);
    const permMatch = block.match(/menu\.Permission\s*=\s*"([^"]+)"/);
    const menuCode = menuCodeMatch ? menuCodeMatch[1] : '';
    const menuType = menuTypeMatch ? Number(menuTypeMatch[1]) : 0;
    if (menuCode && menuType === 1 && permMatch) {
      const actual = permMatch[1].toLowerCase();
      if (actual.endsWith(':list')) {
        const expected = expectedListPermissionFromMenuCode(menuCode);
        if (expected && actual !== expected) {
          block = block.replace(
            /menu\.Permission\s*=\s*"[^"]+"/,
            `menu.Permission = "${expected}"`,
          );
          fixCount += 1;
        }
      }
    }
    out.push(marker + block);
  }
  return { content: out.join(''), fixCount };
}

function main() {
  const blocks = parseAllMenuSeedBlocks();
  /** @type {Array<{ menuCode: string, actual: string, expected: string }>} */
  const planned = [];

  for (const block of blocks) {
    if (!block.menuCode || !block.permission || block.menuType !== 1) {
      continue;
    }
    if (!block.permission.endsWith(':list')) {
      continue;
    }
    const expected = expectedListPermissionFromMenuCode(block.menuCode);
    const actual = block.permission.toLowerCase();
    if (expected && actual !== expected) {
      planned.push({ menuCode: block.menuCode, actual, expected });
    }
  }

  console.log(`待修正 :list Permission: ${planned.length} 块`);
  for (const item of planned.sort((a, b) => a.menuCode.localeCompare(b.menuCode))) {
    console.log(`  [${item.menuCode}] ${item.actual} → ${item.expected}`);
  }

  let fileCount = 0;
  let totalFix = 0;
  for (const fileName of fs.readdirSync(SEEDS_DIR)) {
    if (!/^TaktMenuLevel\d+SeedData\.cs$/.test(fileName)) {
      continue;
    }
    const fullPath = path.join(SEEDS_DIR, fileName);
    const raw = fs.readFileSync(fullPath, 'utf-8');
    const { content, fixCount } = fixPermissionsInSeedFileContent(raw);
    if (fixCount > 0) {
      fs.writeFileSync(fullPath, content, 'utf-8');
      fileCount += 1;
      totalFix += fixCount;
    }
  }
  console.log(`已写入菜单种子文件: ${fileCount} 个 | 修正块数: ${totalFix}`);
}

main();
