// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：scripts
// 文件名称：menu-field-structure.cjs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：MenuCode / I18nKey / Permission 三字段同结构互转（MenuCode 权威分段；_ 大写 / . 小写 / : 小写）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/** 页面菜单 Permission 末段操作（结构对账时剥离） */
const PERMISSION_ACTION_SUFFIXES = new Set([
  'list',
  'query',
  'create',
  'update',
  'delete',
  'import',
  'export',
  'design',
  'publish',
  'tree',
  'withdraw',
  'approve',
  'transfer',
  'addsign',
  'reducesign',
  'suspend',
  'resume',
  'terminate',
  'start',
]);

/**
 * MenuCode → 路径段（全小写，按 _ 拆分）
 * @param {string} menuCode
 * @returns {string[]}
 */
function pathSegmentsFromMenuCode(menuCode) {
  return String(menuCode || '')
    .split('_')
    .map((seg) => seg.trim().toLowerCase())
    .filter(Boolean);
}

/**
 * I18nKey → 路径段（去掉 menu. 前缀，按 . 拆分）
 * @param {string} i18nKey
 * @returns {string[]}
 */
function pathSegmentsFromI18nKey(i18nKey) {
  const raw = String(i18nKey || '').trim().toLowerCase();
  if (!raw.startsWith('menu.')) {
    return [];
  }
  return raw.slice('menu.'.length).split('.').filter(Boolean);
}

/**
 * Permission → 路径段（去掉末段操作，按 : 拆分）
 * @param {string} permission
 * @returns {string[]}
 */
function pathSegmentsFromPermission(permission) {
  const parts = String(permission || '')
    .split(':')
    .map((seg) => seg.trim().toLowerCase())
    .filter(Boolean);
  if (parts.length === 0) {
    return [];
  }
  const last = parts[parts.length - 1];
  if (PERMISSION_ACTION_SUFFIXES.has(last)) {
    return parts.slice(0, -1);
  }
  return parts;
}

/**
 * 路径段 → menu.I18nKey
 * @param {readonly string[]} segments
 * @returns {string}
 */
function i18nKeyFromPathSegments(segments) {
  if (!segments.length) {
    return '';
  }
  return `menu.${segments.join('.')}`;
}

/**
 * 路径段 → 页面 :list Permission
 * @param {readonly string[]} segments
 * @returns {string}
 */
function listPermissionFromPathSegments(segments) {
  if (!segments.length) {
    return '';
  }
  return `${segments.join(':')}:list`;
}

/**
 * MenuCode → 期望 I18nKey
 * @param {string} menuCode
 * @returns {string}
 */
function expectedI18nKeyFromMenuCode(menuCode) {
  return i18nKeyFromPathSegments(pathSegmentsFromMenuCode(menuCode));
}

/**
 * MenuCode → 期望页面 :list Permission
 * @param {string} menuCode
 * @returns {string}
 */
function expectedListPermissionFromMenuCode(menuCode) {
  return listPermissionFromPathSegments(pathSegmentsFromMenuCode(menuCode));
}

/**
 * MenuCode → 目录菜单（MenuType=0）期望 I18nKey
 * @param {string} menuCode
 * @returns {string}
 */
function expectedDirectoryI18nKeyFromMenuCode(menuCode) {
  return `${expectedI18nKeyFromMenuCode(menuCode)}._self`;
}

/**
 * 目录 I18nKey → 路径段（剥离末段 _self）
 * @param {string} i18nKey
 * @returns {string[]}
 */
function pathSegmentsFromDirectoryI18nKey(i18nKey) {
  const segments = pathSegmentsFromI18nKey(i18nKey);
  if (segments.length > 0 && segments[segments.length - 1] === '_self') {
    return segments.slice(0, -1);
  }
  return segments;
}

/**
 * 两段路径是否一致
 * @param {readonly string[]} a
 * @param {readonly string[]} b
 * @returns {boolean}
 */
function pathSegmentsEqual(a, b) {
  if (a.length !== b.length) {
    return false;
  }
  return a.every((seg, idx) => seg === b[idx]);
}

/**
 * @param {readonly string[]} segments
 * @returns {string}
 */
function formatPathSegments(segments) {
  return segments.length ? segments.join(' / ') : '(空)';
}

/**
 * 页面菜单三字段结构须与 MenuCode 分段一致
 * @param {Array<{ menuCode?: string, i18nKey?: string, permission?: string, menuType?: number, sourceFile?: string, line?: number, menuName?: string, routePath?: string }>} blocks
 * @returns {Array<{ block: typeof blocks[0], menuSegments: string[], i18nSegments: string[], permissionSegments: string[], issues: string[] }>}
 */
function collectMenuFieldStructureMismatches(blocks) {
  /** @type {ReturnType<typeof collectMenuFieldStructureMismatches>} */
  const mismatches = [];

  for (const block of blocks) {
    if (!block.menuCode || block.menuType !== 1) {
      continue;
    }
    const menuSegments = pathSegmentsFromMenuCode(block.menuCode);
    if (!menuSegments.length) {
      continue;
    }
    const expectedI18n = i18nKeyFromPathSegments(menuSegments);
    const expectedList = listPermissionFromPathSegments(menuSegments);
    const i18nSegments = pathSegmentsFromI18nKey(block.i18nKey || '');
    const permissionSegments = block.permission?.endsWith(':list')
      ? pathSegmentsFromPermission(block.permission)
      : [];

    /** @type {string[]} */
    const issues = [];
    if (block.i18nKey && block.i18nKey !== expectedI18n) {
      issues.push(`I18nKey 应为 ${expectedI18n}，实际 ${block.i18nKey}`);
    }
    if (block.permission?.endsWith(':list') && block.permission !== expectedList) {
      issues.push(`Permission 应为 ${expectedList}，实际 ${block.permission}`);
    }
    if (block.i18nKey && !pathSegmentsEqual(menuSegments, i18nSegments)) {
      issues.push(`I18nKey 分段 ${formatPathSegments(i18nSegments)} ≠ MenuCode 分段 ${formatPathSegments(menuSegments)}`);
    }
    if (block.permission?.endsWith(':list') && !pathSegmentsEqual(menuSegments, permissionSegments)) {
      issues.push(`Permission 分段 ${formatPathSegments(permissionSegments)} ≠ MenuCode 分段 ${formatPathSegments(menuSegments)}`);
    }

    if (issues.length > 0) {
      mismatches.push({
        block,
        menuSegments,
        i18nSegments,
        permissionSegments,
        issues: [...new Set(issues)],
      });
    }
  }

  mismatches.sort((a, b) => String(a.block.menuCode).localeCompare(String(b.block.menuCode)));
  return mismatches;
}

/**
 * 目录菜单（MenuType=0）I18nKey 须为 MenuCode 分段 + ._self
 * @param {Array<{ menuCode?: string, i18nKey?: string, menuType?: number, sourceFile?: string, line?: number, menuName?: string, routePath?: string }>} blocks
 * @returns {Array<{ block: typeof blocks[0], menuSegments: string[], i18nSegments: string[], issues: string[] }>}
 */
function collectDirectoryMenuI18nMismatches(blocks) {
  /** @type {ReturnType<typeof collectDirectoryMenuI18nMismatches>} */
  const mismatches = [];

  for (const block of blocks) {
    if (!block.menuCode || block.menuType !== 0 || !block.i18nKey) {
      continue;
    }
    const menuSegments = pathSegmentsFromMenuCode(block.menuCode);
    if (!menuSegments.length) {
      continue;
    }
    const expectedI18n = expectedDirectoryI18nKeyFromMenuCode(block.menuCode);
    const i18nSegments = pathSegmentsFromDirectoryI18nKey(block.i18nKey);

    /** @type {string[]} */
    const issues = [];
    if (block.i18nKey !== expectedI18n) {
      issues.push(`I18nKey 应为 ${expectedI18n}，实际 ${block.i18nKey}`);
    }
    if (!block.i18nKey.endsWith('._self')) {
      issues.push('目录菜单 I18nKey 末段须为 ._self');
    }
    if (!pathSegmentsEqual(menuSegments, i18nSegments)) {
      issues.push(`I18nKey 分段 ${formatPathSegments(i18nSegments)} ≠ MenuCode 分段 ${formatPathSegments(menuSegments)}`);
    }

    if (issues.length > 0) {
      mismatches.push({
        block,
        menuSegments,
        i18nSegments,
        issues: [...new Set(issues)],
      });
    }
  }

  mismatches.sort((a, b) => String(a.block.menuCode).localeCompare(String(b.block.menuCode)));
  return mismatches;
}

module.exports = {
  PERMISSION_ACTION_SUFFIXES,
  pathSegmentsFromMenuCode,
  pathSegmentsFromI18nKey,
  pathSegmentsFromDirectoryI18nKey,
  pathSegmentsFromPermission,
  i18nKeyFromPathSegments,
  listPermissionFromPathSegments,
  expectedI18nKeyFromMenuCode,
  expectedDirectoryI18nKeyFromMenuCode,
  expectedListPermissionFromMenuCode,
  pathSegmentsEqual,
  collectMenuFieldStructureMismatches,
  collectDirectoryMenuI18nMismatches,
};
