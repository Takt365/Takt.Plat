// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：scripts
// 文件名称：fix-menu-i18n-from-menucode.cjs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：按 MenuCode 分段逐块修正菜单种子 I18nKey，并同步 TaktMenuI18nSeedData.cs 键名
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const fs = require('fs');
const path = require('path');
const { parseAllMenuSeedBlocks, BACKEND_ROOT } = require('./audit-permission-scan.cjs');
const { expectedI18nKeyFromMenuCode } = require('./menu-field-structure.cjs');

const SEEDS_DIR = path.join(BACKEND_ROOT, 'Takt.Infrastructure/Data/Seeds/EntitySeedData');
const MENU_I18N_SEED = path.join(
  BACKEND_ROOT,
  'Takt.Infrastructure/Data/Seeds/I18nSeedData/TaktMenuI18nSeedData.cs',
);

/**
 * @param {string} value
 * @returns {string}
 */
function escapeRegExp(value) {
  return String(value).replace(/[.*+?^${}()|[\]\\]/g, '\\$&');
}

/**
 * 按 CreateOrUpdateMenuAsync 块逐段修正 I18nKey
 * @param {string} content
 * @returns {{ content: string, fixCount: number }}
 */
function fixI18nKeysInSeedFileContent(content) {
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
    const i18nMatch = block.match(/menu\.I18nKey\s*=\s*"([^"]+)"/);
    const menuCode = menuCodeMatch ? menuCodeMatch[1] : '';
    if (menuCode && i18nMatch) {
      const actual = i18nMatch[1].toLowerCase();
      const expected = expectedI18nKeyFromMenuCode(menuCode);
      if (expected && actual !== expected) {
        block = block.replace(
          /menu\.I18nKey\s*=\s*"[^"]+"/,
          `menu.I18nKey = "${expected}"`,
        );
        fixCount += 1;
      }
    }
    out.push(marker + block);
  }
  return { content: out.join(''), fixCount };
}

/**
 * @param {Map<string, string>} replacements oldI18nKey -> newI18nKey
 * @returns {number}
 */
function applyMenuI18nSeedReplacements(replacements) {
  if (!fs.existsSync(MENU_I18N_SEED) || replacements.size === 0) {
    return 0;
  }
  let content = fs.readFileSync(MENU_I18N_SEED, 'utf-8');
  let count = 0;
  const ordered = [...replacements.entries()].sort((a, b) => b[0].length - a[0].length);
  for (const [oldKey, newKey] of ordered) {
    if (oldKey === newKey) {
      continue;
    }
    const tuplePattern = new RegExp(`\\("${escapeRegExp(oldKey)}",`, 'g');
    const commentPattern = new RegExp(`// ${escapeRegExp(oldKey)}`, 'g');
    if (!tuplePattern.test(content) && !content.includes(`"${oldKey}"`)) {
      continue;
    }
    content = content.replace(tuplePattern, `("${newKey}",`);
    content = content.replace(commentPattern, `// ${newKey}`);
    count += 1;
  }
  fs.writeFileSync(MENU_I18N_SEED, content, 'utf-8');
  return count;
}

function main() {
  const blocks = parseAllMenuSeedBlocks();
  /** @type {Map<string, string>} */
  const replacements = new Map();

  for (const block of blocks) {
    if (!block.menuCode || !block.i18nKey) {
      continue;
    }
    const expected = expectedI18nKeyFromMenuCode(block.menuCode);
    const actual = block.i18nKey.toLowerCase();
    if (expected && actual !== expected) {
      replacements.set(actual, expected);
    }
  }

  console.log(`待修正 I18nKey: ${replacements.size} 组`);
  for (const [oldKey, newKey] of [...replacements.entries()].sort((a, b) => a[0].localeCompare(b[0]))) {
    console.log(`  ${oldKey} → ${newKey}`);
  }

  let seedFileCount = 0;
  let blockFixCount = 0;
  for (const fileName of fs.readdirSync(SEEDS_DIR)) {
    if (!/^TaktMenuLevel\d+SeedData\.cs$/.test(fileName)) {
      continue;
    }
    const fullPath = path.join(SEEDS_DIR, fileName);
    const raw = fs.readFileSync(fullPath, 'utf-8');
    const { content, fixCount } = fixI18nKeysInSeedFileContent(raw);
    if (fixCount > 0) {
      fs.writeFileSync(fullPath, content, 'utf-8');
      seedFileCount += 1;
      blockFixCount += fixCount;
    }
  }

  const i18nSeedKeys = applyMenuI18nSeedReplacements(replacements);
  console.log(`已写入菜单种子文件: ${seedFileCount} 个 | 修正块数: ${blockFixCount}`);
  console.log(`TaktMenuI18nSeedData 键替换: ${i18nSeedKeys} 组`);
}

main();
