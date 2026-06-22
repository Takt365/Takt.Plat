// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：scripts
// 文件名称：audit-menu-field-common.cjs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：菜单种子 MenuCode / I18nKey / Permission 审计共用逻辑
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const fs = require('fs');
const path = require('path');
const {
  scanAllServices,
  parseAllMenuSeedBlocks,
  BACKEND_ROOT,
  buildControllerPermissionIndex,
} = require('./audit-permission-scan.cjs');
const { buildMasterDetailChildRegistry } = require('./generate-vue-common.cjs');
const { detectPermissionStemRedundancy } = require('./permission-base.cjs');
const {
  SKIP_MENU_CODES,
  isShellViewPage,
  buildExpectedMenuFields,
  resolveServiceFromViewModulePath,
} = require('./menu-entity-expectations.cjs');
const {
  expectedListPermissionFromMenuCode,
  expectedI18nKeyFromMenuCode,
} = require('./menu-field-structure.cjs');

const SCRIPTS_ROOT = path.resolve(__dirname);
const REPORTS_DIR = path.join(SCRIPTS_ROOT, 'reports');
const MENU_I18N_SEED = path.join(
  BACKEND_ROOT,
  'Takt.Infrastructure/Data/Seeds/I18nSeedData/TaktMenuI18nSeedData.cs',
);

/**
 * @param {string} defaultReportFile
 * @returns {string}
 */
function resolveReportOutputPath(defaultReportFile) {
  const outArg = process.argv.find((arg) => arg.startsWith('--out='));
  if (outArg) {
    const custom = outArg.slice('--out='.length).trim();
    if (path.isAbsolute(custom)) {
      return custom;
    }
    if (custom.startsWith('scripts/') || custom.startsWith('scripts\\')) {
      return path.resolve(SCRIPTS_ROOT, '..', custom);
    }
    return path.resolve(SCRIPTS_ROOT, custom);
  }
  return defaultReportFile;
}

/**
 * @param {string} reportPath
 */
function ensureReportDir(reportPath) {
  const reportDir = path.dirname(reportPath);
  if (!fs.existsSync(reportDir)) {
    fs.mkdirSync(reportDir, { recursive: true });
  }
}

/**
 * @returns {ReturnType<typeof parseAllMenuSeedBlocks>}
 */
function getPageMenuBlocks() {
  return parseAllMenuSeedBlocks().filter((block) => block.menuType === 1
    && block.componentPath.endsWith('/index')
    && !SKIP_MENU_CODES.has(block.menuCode)
    && !isShellViewPage(block.viewModulePath));
}

/**
 * @param {readonly T[]} items
 * @param {(item: T) => string} keyFn
 * @returns {Map<string, T[]>}
 * @template T
 */
function groupByKey(items, keyFn) {
  /** @type {Map<string, T[]>} */
  const map = new Map();
  for (const item of items) {
    const key = keyFn(item);
    if (!map.has(key)) {
      map.set(key, []);
    }
    map.get(key).push(item);
  }
  return map;
}

/**
 * @param {Map<string, unknown[]>} groups
 * @returns {Array<{ key: string, items: unknown[] }>}
 */
function duplicateGroups(groups) {
  /** @type {Array<{ key: string, items: unknown[] }>} */
  const dupes = [];
  for (const [key, items] of groups) {
    if (items.length > 1) {
      dupes.push({ key, items });
    }
  }
  dupes.sort((a, b) => a.key.localeCompare(b.key));
  return dupes;
}

/**
 * @param {string} title
 * @param {Array<{ key: string, items: Array<{ menuCode?: string, sourceFile?: string, line?: number, lookupKey?: string }> }>} dupes
 * @returns {string[]}
 */
function formatDuplicateSection(title, dupes) {
  /** @type {string[]} */
  const lines = [`${title}: ${dupes.length}`];
  if (dupes.length === 0) {
    return lines;
  }
  for (const group of dupes) {
    lines.push('');
    lines.push(`  "${group.key}" × ${group.items.length}`);
    for (const item of group.items) {
      const code = item.menuCode || item.lookupKey || '';
      lines.push(`    [${code}] ${item.sourceFile || ''}:${item.line || ''}`);
    }
  }
  return lines;
}

/**
 * @param {ReturnType<typeof parseAllMenuSeedBlocks>} blocks
 * @param {'menuCode'|'i18nKey'|'permission'} fieldName
 * @returns {Array<{ block: ReturnType<typeof parseAllMenuSeedBlocks>[0], entityShort: string, actual: string, expected: string, source: string }>}
 */
function collectEntityFieldMismatches(blocks, fieldName) {
  const services = scanAllServices();
  const childRegistry = buildMasterDetailChildRegistry();
  /** @type {Map<string, { pathParts: string[] }>} */
  const serviceByEntity = new Map(services.map((svc) => [svc.entityShort, svc]));
  /** @type {ReturnType<typeof collectEntityFieldMismatches>} */
  const mismatches = [];

  for (const block of blocks) {
    if (block.menuType !== 1 || !block.viewModulePath || isShellViewPage(block.viewModulePath)) {
      continue;
    }
    if (SKIP_MENU_CODES.has(block.menuCode)) {
      continue;
    }
    const svc = resolveServiceFromViewModulePath(block.viewModulePath, services);
    if (!svc) {
      continue;
    }
    let expected = '';
    let actual = '';
    let source = '实体服务路径推导';
    if (fieldName === 'menuCode') {
      expected = buildExpectedMenuFields(svc, childRegistry, block.viewModulePath, serviceByEntity).menuCode;
      actual = block.menuCode;
    } else if (fieldName === 'i18nKey') {
      expected = expectedI18nKeyFromMenuCode(block.menuCode);
      actual = block.i18nKey;
      source = 'MenuCode 分段推导';
    } else if (fieldName === 'permission') {
      if (!block.permission?.endsWith(':list')) {
        continue;
      }
      expected = expectedListPermissionFromMenuCode(block.menuCode);
      actual = block.permission;
      source = 'MenuCode 分段推导';
    }
    if (actual !== expected) {
      mismatches.push({
        block,
        entityShort: svc.entityShort,
        actual,
        expected,
        source,
      });
    }
  }

  mismatches.sort((a, b) => a.block.menuCode.localeCompare(b.block.menuCode));
  return mismatches;
}

/**
 * @returns {Set<string>}
 */
function parseMenuI18nSeedKeys() {
  /** @type {Set<string>} */
  const keys = new Set();
  if (!fs.existsSync(MENU_I18N_SEED)) {
    return keys;
  }
  const content = fs.readFileSync(MENU_I18N_SEED, 'utf-8');
  const regex = /\("(menu\.[^"]+)"/g;
  let match;
  while ((match = regex.exec(content)) !== null) {
    keys.add(match[1].toLowerCase());
  }
  return keys;
}

/**
 * @param {string} i18nKey
 * @returns {boolean}
 */
function isValidMenuI18nKeyFormat(i18nKey) {
  if (!i18nKey || !i18nKey.startsWith('menu.')) {
    return false;
  }
  const segments = i18nKey.split('.').slice(1);
  return segments.length > 0 && segments.every((seg) => seg === '_self' || /^[a-z0-9]+$/.test(seg));
}

/**
 * @param {ReturnType<typeof parseAllMenuSeedBlocks>} blocks
 * @returns {Array<{ block: ReturnType<typeof parseAllMenuSeedBlocks>[0], reason: string }>}
 */
function collectInvalidI18nKeys(blocks) {
  /** @type {ReturnType<typeof collectInvalidI18nKeys>} */
  const invalid = [];
  for (const block of blocks) {
    if (!block.i18nKey) {
      continue;
    }
    if (!isValidMenuI18nKeyFormat(block.i18nKey)) {
      invalid.push({ block, reason: '格式非法（须 menu.* 小写点号，段内无下划线）' });
    }
  }
  return invalid;
}

/**
 * @param {ReturnType<typeof parseAllMenuSeedBlocks>} blocks
 * @param {Set<string>} seedKeys
 * @returns {Array<{ block: ReturnType<typeof parseAllMenuSeedBlocks>[0] }>}
 */
function collectMissingMenuI18nSeed(blocks, seedKeys) {
  /** @type {ReturnType<typeof collectMissingMenuI18nSeed>} */
  const missing = [];
  for (const block of blocks) {
    if (!block.i18nKey || !block.i18nKey.startsWith('menu.')) {
      continue;
    }
    if (!seedKeys.has(block.i18nKey.toLowerCase())) {
      missing.push({ block });
    }
  }
  return missing;
}

/**
 * @param {ReturnType<typeof parseAllMenuSeedBlocks>} blocks
 * @returns {Array<{ block: ReturnType<typeof parseAllMenuSeedBlocks>[0], reason: string, suggested?: string }>}
 */
function collectInvalidMenuPermissions(blocks) {
  /** @type {ReturnType<typeof collectInvalidMenuPermissions>} */
  const invalid = [];
  for (const block of blocks) {
    if (!block.permission) {
      continue;
    }
    if (block.permission.includes('.')) {
      invalid.push({ block, reason: 'Permission 含点号（须冒号分段）' });
      continue;
    }
    if (!/^[a-z0-9]+(:[a-z0-9]+)*$/.test(block.permission)) {
      invalid.push({ block, reason: 'Permission 格式非法' });
      continue;
    }
    if (block.menuType === 1 && block.viewModulePath && isShellViewPage(block.viewModulePath)) {
      continue;
    }
    if (block.menuCode && block.permission.endsWith(':list')) {
      const expectedPrefix = expectedListPermissionFromMenuCode(block.menuCode).replace(/:list$/, '');
      const hit = detectPermissionStemRedundancy(block.permission, expectedPrefix);
      if (hit) {
        invalid.push({
          block,
          reason: `词干与 MenuCode 推导不一致（实际 ${hit.actualPrefix}）`,
          suggested: `${hit.suggestedPrefix}:list`,
        });
      }
    }
  }
  return invalid;
}

/**
 * @param {Array<{ block: ReturnType<typeof parseAllMenuSeedBlocks>[0] }>} a
 * @param {Array<{ block: ReturnType<typeof parseAllMenuSeedBlocks>[0] }>} b
 * @returns {boolean}
 */
function isChangeLogListPermissionPair(a, b) {
  const codeA = a.block.menuCode || '';
  const codeB = b.block.menuCode || '';
  if (!codeA.endsWith('_CHANGE_LOG') && !codeB.endsWith('_CHANGE_LOG')) {
    return false;
  }
  const masterCode = codeA.endsWith('_CHANGE_LOG') ? codeA.replace(/_CHANGE_LOG$/, '') : codeA;
  const changeCode = codeB.endsWith('_CHANGE_LOG') ? codeB : `${codeB}_CHANGE_LOG`;
  return masterCode === changeCode.replace(/_CHANGE_LOG$/, '');
}

/**
 * @param {Array<{ key: string, items: ReturnType<typeof parseAllMenuSeedBlocks> }>} dupes
 * @returns {Array<{ key: string, items: ReturnType<typeof parseAllMenuSeedBlocks> }>}
 */
function filterReportableListPermissionDupes(dupes) {
  return dupes.filter((group) => {
    if (group.items.length !== 2) {
      return true;
    }
    return !isChangeLogListPermissionPair(
      { block: group.items[0] },
      { block: group.items[1] },
    );
  });
}

/**
 * @returns {Array<{ block: ReturnType<typeof parseAllMenuSeedBlocks>[0], entityShort: string, expected: string, actual: string, controllerFile: string }>}
 */
function collectMenuPermissionMismatches() {
  const services = scanAllServices();
  const controllerIndex = buildControllerPermissionIndex();
  /** @type {ReturnType<typeof collectMenuPermissionMismatches>} */
  const mismatches = [];

  for (const block of getPageMenuBlocks()) {
    if (!block.permission.endsWith(':list')) {
      continue;
    }
    const svc = resolveServiceFromViewModulePath(block.viewModulePath, services);
    if (!svc) {
      continue;
    }
    const controllerBase = controllerIndex.listBaseByEntity.get(svc.entityShort);
    if (!controllerBase) {
      continue;
    }
    const controllerList = `${controllerBase}:list`;
    const menuSeedList = block.permission;
    if (controllerList !== menuSeedList) {
      const controllerFile = controllerIndex.controllerFileByEntity.get(svc.entityShort);
      mismatches.push({
        block,
        entityShort: svc.entityShort,
        expected: menuSeedList,
        actual: controllerList,
        controllerFile: controllerFile
          ? path.relative(BACKEND_ROOT, controllerFile).replace(/\\/g, '/')
          : '',
      });
    }
  }

  mismatches.sort((a, b) => a.block.menuCode.localeCompare(b.block.menuCode));
  return mismatches;
}

/**
 * @param {ReturnType<typeof collectMenuPermissionMismatches>} mismatches
 * @returns {string[]}
 */
function formatMenuSeedVsControllerSection(mismatches) {
  if (mismatches.length === 0) {
    return ['控制器 :list 与菜单种子不一致: 0'];
  }
  /** @type {string[]} */
  const lines = [`控制器 :list 与菜单种子不一致: ${mismatches.length}（权威：菜单种子）`];
  for (const item of mismatches) {
    lines.push('');
    lines.push(`  [${item.block.menuCode}] entity=${item.entityShort} | ${item.block.sourceFile}:${item.block.line}`);
    lines.push(`    view: ${item.block.viewModulePath}`);
    lines.push(`    菜单种子: ${item.expected}`);
    lines.push(`    控制器:   ${item.actual}`);
    if (item.controllerFile) {
      lines.push(`    控制器文件: ${item.controllerFile}`);
    }
  }
  return lines;
}

/**
 * @param {Array<{ block: ReturnType<typeof parseAllMenuSeedBlocks>[0], entityShort: string, actual: string, expected: string, source?: string }>} mismatches
 * @param {string} fieldLabel
 * @returns {string[]}
 */
function formatEntityMismatchSection(mismatches, fieldLabel) {
  if (mismatches.length === 0) {
    return [`${fieldLabel} 与期望不一致: 0`];
  }
  /** @type {string[]} */
  const lines = [`${fieldLabel} 与期望不一致: ${mismatches.length}`];
  for (const item of mismatches) {
    lines.push('');
    lines.push(`  [${item.block.menuCode}] entity=${item.entityShort} | ${item.block.sourceFile}:${item.block.line}`);
    lines.push(`    view: ${item.block.viewModulePath}`);
    if (item.source) {
      lines.push(`    期望来源: ${item.source}`);
    }
    lines.push(`    实际: ${item.actual}`);
    lines.push(`    期望: ${item.expected}`);
  }
  return lines;
}

/**
 * @param {string} reportPath
 * @param {string[]} lines
 * @param {number} failCount
 * @param {string} rerunCmd
 */
function finishAuditReport(reportPath, lines, failCount, rerunCmd) {
  lines.push('');
  lines.push(`重跑: ${rerunCmd}`);
  ensureReportDir(reportPath);
  fs.writeFileSync(reportPath, `${lines.join('\n')}\n`, 'utf-8');
  console.log(`📄 报告: ${reportPath}`);
  console.log(`   结果: ${failCount === 0 ? 'PASS ✅' : 'FAIL ❌'}`);
  if (failCount > 0) {
    process.exit(1);
  }
}

module.exports = {
  REPORTS_DIR,
  resolveReportOutputPath,
  ensureReportDir,
  getPageMenuBlocks,
  parseAllMenuSeedBlocks,
  groupByKey,
  duplicateGroups,
  formatDuplicateSection,
  collectEntityFieldMismatches,
  parseMenuI18nSeedKeys,
  collectInvalidI18nKeys,
  collectMissingMenuI18nSeed,
  collectInvalidMenuPermissions,
  collectMenuPermissionMismatches,
  filterReportableListPermissionDupes,
  isChangeLogListPermissionPair,
  formatMenuSeedVsControllerSection,
  formatEntityMismatchSection,
  finishAuditReport,
};
