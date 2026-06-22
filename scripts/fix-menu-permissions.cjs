// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：scripts
// 文件名称：fix-menu-permissions.cjs
// 创建时间：2026-06-21
// 创建人：Takt365(Cursor AI)
// 功能描述：按 buildPermissionBase 与主子表主表权限规则，修正 TaktMenuLevel1~5 种子 menu.Permission
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
  resolveMenuListPermissionBase,
  resolveServiceFromViewModulePath,
} = require('./menu-entity-expectations.cjs');

const SEEDS_DIR = path.resolve(__dirname, '../backend/src/Takt.Infrastructure/Data/Seeds/EntitySeedData');

const SEEDS_DIR = path.resolve(__dirname, '../backend/src/Takt.Infrastructure/Data/Seeds/EntitySeedData');
const REPORTS_DIR = path.resolve(__dirname, 'reports');
const REPORT_FILE = path.join(REPORTS_DIR, 'fix-menu-permissions.txt');

const MENU_LEVEL_FILES = [
  'TaktMenuLevel1SeedData.cs',
  'TaktMenuLevel2SeedData.cs',
  'TaktMenuLevel3SeedData.cs',
  'TaktMenuLevel4SeedData.cs',
  'TaktMenuLevel5SeedData.cs',
];

/** 壳页面/工作流待办等：不参与 buildPermissionBase 自动修正 */
const SKIP_MENU_CODES = new Set([
  'TAKT_HOME',
  'TAKT_ABOUT',
  'WORKFLOW_TODO',
  'WORKFLOW_MY',
  'WORKFLOW_PROCESSED',
]);

/** @deprecated 权限与 buildPermissionBase 一致 */
const MENU_OWN_PERMISSION_ENTITIES = new Set();

/** ComponentPath 无法自动匹配实体时的显式映射（viewModulePath → 实体短名） */
const VIEW_MODULE_PATH_ENTITY_OVERRIDES = {
  'logistics/procurement/purchase-request': 'PurchaseRequest',
  'logistics/procurement/purchase-request-change-log': 'PurchaseRequestChangeLog',
  'logistics/procurement/purchase-order': 'PurchaseOrder',
  'logistics/procurement/purchase-order-change-log': 'PurchaseOrderChangeLog',
  'logistics/procurement/purchase-price': 'PurchasePrice',
  'logistics/procurement/purchase-price-change-log': 'PurchasePriceChangeLog',
  'logistics/procurement/purchase-invoice': 'PurchaseInvoice',
  'logistics/sales/invoice': 'SalesInvoice',
  'code/database/table-clone': 'TableClone',
  'code/database/data-clone': 'DataClone',
  'code/database/database-info': 'DatabaseInfo',
};

/**
 * PascalCase → kebab-case
 * @param {string} value
 * @returns {string}
 */
function pascalToKebab(value) {
  return String(value).replace(/([a-z0-9])([A-Z])/g, '$1-$2').toLowerCase();
}

/**
 * @param {string} viewModulePath
 * @param {ReturnType<typeof scanAllServices>} services
 * @returns {string|null}
 */
function resolveEntityFromViewModulePath(viewModulePath, services) {
  if (!viewModulePath) {
    return null;
  }
  if (VIEW_MODULE_PATH_ENTITY_OVERRIDES[viewModulePath]) {
    return VIEW_MODULE_PATH_ENTITY_OVERRIDES[viewModulePath];
  }
  const leaf = viewModulePath.split('/').pop() || '';
  const leafNorm = leaf.replace(/-/g, '').toLowerCase();
  for (const svc of services) {
    if (pascalToKebab(svc.entityShort) === leaf) {
      return svc.entityShort;
    }
  }
  for (const svc of services) {
    const kebabNorm = pascalToKebab(svc.entityShort).replace(/-/g, '');
    if (kebabNorm === leafNorm || svc.entityShort.toLowerCase() === leafNorm) {
      return svc.entityShort;
    }
  }
  return null;
}

/**
 * 计算菜单页面应对齐的 list 权限前缀（ChangeLog / 导航从实体继承主表）
 * @param {string} entityShort
 * @param {Map<string, { entityName: string, entityShort: string, pathParts: string[] }>} serviceByEntity
 * @param {Map<string, { masterPascal: string }>} childRegistry
 * @returns {string|null}
 */
function resolveMenuListPermissionForEntity(entityShort, serviceByEntity, childRegistry) {
  const svc = serviceByEntity.get(entityShort);
  if (!svc) {
    return null;
  }
  return resolveMenuListPermissionBase(svc, childRegistry, serviceByEntity);
}

/**
 * 修正单个种子文件中的 menu.Permission
 * @param {string} fileName
 * @param {Map<string, { entityName: string, entityShort: string, pathParts: string[] }>} serviceByEntity
 * @param {Map<string, { masterPascal: string }>} childRegistry
 * @returns {Array<{ menuCode: string, oldPermission: string, newPermission: string }>}
 */
function fixMenuSeedFile(fileName, serviceByEntity, childRegistry) {
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
    const permMatch = block.match(/menu\.Permission\s*=\s*"([^"]+)"/);
    const componentMatch = block.match(/menu\.ComponentPath\s*=\s*"([^"]+)"/);
    if (codeMatch && permMatch) {
      const menuCode = codeMatch[1];
      const oldPermission = permMatch[1];
      const componentPath = componentMatch ? componentMatch[1] : '';
      const viewModulePath = componentPath.endsWith('/index')
        ? componentPath.replace(/\/index$/, '')
        : '';
      if (!SKIP_MENU_CODES.has(menuCode)) {
        const entityShort = resolveEntityFromViewModulePath(viewModulePath, [...serviceByEntity.values()]);
        if (entityShort) {
          const base = resolveMenuListPermissionForEntity(entityShort, serviceByEntity, childRegistry);
          if (base) {
            const newPermission = `${base}:list`;
            if (oldPermission !== newPermission) {
              block = block.replace(
                /menu\.Permission\s*=\s*"[^"]+"/,
                `menu.Permission = "${newPermission}"`,
              );
              changes.push({ menuCode, oldPermission, newPermission });
            }
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
  /** @type {Map<string, { entityName: string, entityShort: string, pathParts: string[] }>} */
  const serviceByEntity = new Map(services.map((svc) => [svc.entityShort, svc]));
  const childRegistry = buildMasterDetailChildRegistry();

  /** @type {Array<{ file: string, menuCode: string, oldPermission: string, newPermission: string }>} */
  const allChanges = [];
  for (const fileName of MENU_LEVEL_FILES) {
    const changes = fixMenuSeedFile(fileName, serviceByEntity, childRegistry);
    for (const change of changes) {
      allChanges.push({ file: fileName, ...change });
    }
  }

  const lines = [
    'Takt Menu Permission Fix Report',
    `Generated: ${new Date().toISOString()}`,
    `Total changes: ${allChanges.length}`,
    '',
  ];
  for (const row of allChanges) {
    lines.push(`[${row.file}] ${row.menuCode}`);
    lines.push(`  ${row.oldPermission}`);
    lines.push(`  -> ${row.newPermission}`);
  }
  fs.writeFileSync(REPORT_FILE, `${lines.join('\n')}\n`, 'utf-8');

  console.log(`📄 修复报告: ${REPORT_FILE}`);
  console.log(`   共修正 ${allChanges.length} 处 menu.Permission`);
  if (allChanges.length > 0) {
    for (const row of allChanges.slice(0, 15)) {
      console.log(`   ${row.menuCode}: ${row.oldPermission} -> ${row.newPermission}`);
    }
    if (allChanges.length > 15) {
      console.log(`   ... 其余 ${allChanges.length - 15} 条见报告`);
    }
  }
}

main();
