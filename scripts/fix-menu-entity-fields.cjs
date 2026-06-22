// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：scripts
// 文件名称：fix-menu-entity-fields.cjs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：按 menu-entity-expectations 修正 TaktMenuLevel1~5 种子 MenuCode/I18nKey/Permission/RoutePath/ComponentPath
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const fs = require('fs');
const path = require('path');
const { scanAllServices, parseAllMenuEntries } = require('./audit-permission-scan.cjs');
const { buildMasterDetailChildRegistry } = require('./generate-vue-common.cjs');
const {
  SKIP_MENU_CODES,
  isShellViewPage,
  buildExpectedMenuFields,
  resolveServiceFromViewModulePath,
} = require('./menu-entity-expectations.cjs');

const SEEDS_DIR = path.resolve(__dirname, '../backend/src/Takt.Infrastructure/Data/Seeds/EntitySeedData');
const REPORTS_DIR = path.resolve(__dirname, 'reports');
const REPORT_FILE = path.join(REPORTS_DIR, 'fix-menu-entity-fields.txt');

const MENU_LEVEL_FILES = [
  'TaktMenuLevel1SeedData.cs',
  'TaktMenuLevel2SeedData.cs',
  'TaktMenuLevel3SeedData.cs',
  'TaktMenuLevel4SeedData.cs',
  'TaktMenuLevel5SeedData.cs',
];

const FIELD_PATTERNS = {
  MenuCode: /menu\.MenuCode\s*=\s*"[^"]+"/,
  I18nKey: /menu\.I18nKey\s*=\s*"[^"]+"/,
  Permission: /menu\.Permission\s*=\s*"[^"]+"/,
  RoutePath: /menu\.RoutePath\s*=\s*"[^"]+"/,
  ComponentPath: /menu\.ComponentPath\s*=\s*"[^"]+"/,
};

/**
 * @param {string} block
 * @param {string} field
 * @param {string} value
 * @param {string} [lookupMenuCode]
 * @returns {string}
 */
function replaceMenuField(block, field, value, lookupMenuCode) {
  if (field === 'MenuCode' && lookupMenuCode && lookupMenuCode !== value) {
    block = block.replace(
      /,\s*"([^"]+)"\s*,\s*menu\s*=>/,
      `, "${value}", menu =>`,
    );
  }
  const pattern = FIELD_PATTERNS[field];
  if (!pattern.test(block)) {
    return block;
  }
  return block.replace(pattern, `menu.${field} = "${value}"`);
}

/**
 * @param {string} fileName
 * @param {ReturnType<typeof scanAllServices>} services
 * @param {ReturnType<typeof buildMasterDetailChildRegistry>} childRegistry
 * @param {Map<string, { pathParts: string[] }>} serviceByEntity
 * @returns {Array<{ menuCode: string, field: string, oldValue: string, newValue: string }>}
 */
function fixMenuSeedFile(fileName, services, childRegistry, serviceByEntity) {
  const filePath = path.join(SEEDS_DIR, fileName);
  if (!fs.existsSync(filePath)) {
    return [];
  }
  let content = fs.readFileSync(filePath, 'utf-8');
  /** @type {ReturnType<typeof fixMenuSeedFile>} */
  const changes = [];
  const blocks = content.split(/CreateOrUpdateMenuAsync\(/);
  /** @type {string[]} */
  const rebuilt = [blocks[0]];

  for (let i = 1; i < blocks.length; i += 1) {
    let block = blocks[i];
    const codeMatch = block.match(/,\s*"([^"]+)"\s*,\s*menu\s*=>/);
    const menuTypeMatch = block.match(/menu\.MenuType\s*=\s*(\d+)/);
    const componentMatch = block.match(/menu\.ComponentPath\s*=\s*"([^"]+)"/);
    const menuCode = codeMatch ? codeMatch[1] : '';
    const menuType = menuTypeMatch ? Number(menuTypeMatch[1]) : 0;
    const componentPath = componentMatch ? componentMatch[1] : '';
    const viewModulePath = componentPath.endsWith('/index')
      ? componentPath.replace(/\/index$/, '')
      : '';

    if (menuCode
      && !SKIP_MENU_CODES.has(menuCode)
      && menuType === 1
      && componentPath.endsWith('/index')
      && !isShellViewPage(viewModulePath)) {
      const svc = resolveServiceFromViewModulePath(viewModulePath, services);
      if (svc) {
        const expected = buildExpectedMenuFields(svc, childRegistry, viewModulePath, serviceByEntity);
        const pairs = [
          ['MenuCode', menuCode, expected.menuCode],
          ['I18nKey', block.match(/menu\.I18nKey\s*=\s*"([^"]+)"/)?.[1] || '', expected.i18nKey],
          ['Permission', block.match(/menu\.Permission\s*=\s*"([^"]+)"/)?.[1] || '', expected.permission],
          ['RoutePath', block.match(/menu\.RoutePath\s*=\s*"([^"]+)"/)?.[1] || '', expected.routePath],
          ['ComponentPath', componentPath, expected.componentPath],
        ];
        for (const [field, oldValue, newValue] of pairs) {
          if (oldValue && newValue && oldValue !== newValue) {
            block = replaceMenuField(block, field, newValue, menuCode);
            changes.push({ menuCode, field, oldValue, newValue });
          }
        }
      }
    }
    rebuilt.push(`CreateOrUpdateMenuAsync(${block}`);
  }

  if (changes.length > 0) {
    content = rebuilt.join('');
    fs.writeFileSync(filePath, content, 'utf-8');
  }
  return changes;
}

function main() {
  if (!fs.existsSync(REPORTS_DIR)) {
    fs.mkdirSync(REPORTS_DIR, { recursive: true });
  }

  const services = scanAllServices();
  const childRegistry = buildMasterDetailChildRegistry();
  /** @type {Map<string, { pathParts: string[] }>} */
  const serviceByEntity = new Map(services.map((svc) => [svc.entityShort, svc]));
  /** @type {Array<{ file: string, menuCode: string, field: string, oldValue: string, newValue: string }>} */
  const allChanges = [];

  for (const fileName of MENU_LEVEL_FILES) {
    const changes = fixMenuSeedFile(fileName, services, childRegistry, serviceByEntity);
    for (const change of changes) {
      allChanges.push({ file: fileName, ...change });
    }
  }

  const lines = [
    'Takt Menu Entity Fields Fix Report',
    `Generated: ${new Date().toISOString()}`,
    `Total field changes: ${allChanges.length}`,
    '',
  ];
  for (const row of allChanges) {
    lines.push(`[${row.file}] ${row.menuCode} ${row.field}`);
    lines.push(`  ${row.oldValue}`);
    lines.push(`  -> ${row.newValue}`);
  }
  fs.writeFileSync(REPORT_FILE, `${lines.join('\n')}\n`, 'utf-8');

  console.log(`📄 修复报告: ${REPORT_FILE}`);
  console.log(`   共修正 ${allChanges.length} 处菜单字段`);
  if (allChanges.length > 0) {
    const preview = allChanges.slice(0, 12);
    for (const row of preview) {
      console.log(`   ${row.menuCode}.${row.field}: ${row.oldValue} -> ${row.newValue}`);
    }
    if (allChanges.length > 12) {
      console.log(`   ... 其余 ${allChanges.length - 12} 条见报告`);
    }
  }
}

main();
