// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：scripts
// 文件名称：menu-entity-expectations.cjs
// 创建时间：2026-06-20
// 创建人：Takt365(Cursor AI)
// 功能描述：由实体服务路径推导 MenuCode / I18nKey / Permission / RoutePath / ComponentPath 期望值
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const {
  entityShortToPermissionSlug,
  resolvePermissionEntitySlugFromModulePath,
  isModuleLeafSameAsEntityKebab,
  resolveViewModulePath,
} = require('./generate-script-common.cjs');
const { buildPermissionBase, buildPathPermissionSegments } = require('./permission-base.cjs');
const { buildMasterDetailChildRegistry } = require('./generate-vue-common.cjs');
const { isStandaloneChildVueEntity } = require('./generate-entity-exclusions.cjs');

/** MenuCode 与实体短名不一致时的显式映射（主子表页含子导航等） */
const MENU_CODE_OVERRIDES = {
  'logistics/materials/manufacturer': 'LOGISTICS_MATERIALS_MANUFACTURER_MATERIAL',
};

/** ComponentPath 末段与实体短名不一致时的显式映射（与 fix-menu-permissions 一致） */
const VIEW_MODULE_PATH_ENTITY_OVERRIDES = {
  'logistics/procurement/purchase-request': 'PurchaseRequest',
  'logistics/procurement/purchase-request-change-log': 'PurchaseRequestChangeLog',
  'logistics/procurement/purchase-inquiry': 'PurchaseInquiry',
  'logistics/procurement/purchase-order': 'PurchaseOrder',
  'logistics/procurement/purchase-order-change-log': 'PurchaseOrderChangeLog',
  'logistics/procurement/purchase-price': 'PurchasePrice',
  'logistics/procurement/purchase-price-change-log': 'PurchasePriceChangeLog',
  'logistics/procurement/purchase-invoice': 'PurchaseInvoice',
  'logistics/sales/invoice': 'SalesInvoice',
  'logistics/materials/manufacturer': 'Manufacturer',
  'code/database/table-clone': 'TableClone',
  'code/database/data-clone': 'DataClone',
  'code/database/database-info': 'DatabaseInfo',
};

/** 服务目录段 → 前端 Route 目录段（与菜单种子一致） */
const SERVICE_PATH_PART_ALIASES = {
  CustomerService: 'service',
};

/** @deprecated 权限与 buildPermissionBase 一致，不再单独维护 */
const MENU_OWN_PERMISSION_ENTITIES = new Set();

/** 不参与实体五字段对账的 MenuCode */
const SKIP_MENU_CODES = new Set([
  'TAKT_HOME',
  'TAKT_ABOUT',
  'WORKFLOW_TODO',
  'WORKFLOW_MY',
  'WORKFLOW_PROCESSED',
]);

/** 壳页面 / 手工页：无标准 CRUD 实体期望 */
const SHELL_VIEW_PREFIXES = [
  'about/',
  'home',
  'dashboard/',
  'foundation/dict',
  'foundation/i18n',
  'workflow/',
  'code/generator',
  'routine/help-desk/my-',
];

/**
 * PascalCase → kebab-case
 * @param {string} value
 * @returns {string}
 */
function pascalToKebab(value) {
  return String(value).replace(/([a-z0-9])([A-Z])/g, '$1-$2').toLowerCase();
}

/**
 * PascalCase → UPPER_SNAKE
 * @param {string} value
 * @returns {string}
 */
function pascalToUpperSnake(value) {
  return pascalToKebab(value).replace(/-/g, '_').toUpperCase();
}

/**
 * @param {string} viewModulePath
 * @returns {boolean}
 */
function isShellViewPage(viewModulePath) {
  if (!viewModulePath) {
    return true;
  }
  return SHELL_VIEW_PREFIXES.some((prefix) => viewModulePath === prefix.replace(/\/$/, '')
    || viewModulePath.startsWith(prefix));
}

/**
 * @param {string[]} pathParts
 * @returns {string}
 */
function buildViewModulePrefix(pathParts) {
  return pathParts.map((part) => SERVICE_PATH_PART_ALIASES[part] || pascalToKebab(part)).join('/');
}

/**
 * @param {string} viewModulePath
 * @returns {string}
 */
function buildI18nPrefixFromViewParent(viewModulePath) {
  const parts = viewModulePath.split('/');
  parts.pop();
  return parts.flatMap((segment) => segment.split('-').filter(Boolean)).join('.');
}

/**
 * 由服务路径 + 菜单 view 路径推导 MenuCode 前缀（与种子 HUMAN_RESOURCE / DOCUMENT_CENTER / LOGISTICS_SERVICE 一致）
 * @param {{ pathParts: string[] }} svc
 * @param {string} menuViewModulePath
 * @returns {string}
 */
function buildMenuCodePrefixForService(svc, menuViewModulePath) {
  const viewPath = menuViewModulePath || computeDefaultViewModulePath(svc);
  const parentSegments = viewPath.split('/').slice(0, -1);
  /** @type {string[]} */
  const parts = [];
  for (let i = 0; i < parentSegments.length; i += 1) {
    const seg = parentSegments[i];
    const pathPart = svc.pathParts[i];
    if (pathPart === 'CustomerService') {
      parts.push('SERVICE');
      continue;
    }
    if (pathPart && pascalToKebab(pathPart) === seg) {
      parts.push(pascalToUpperSnake(pathPart));
      continue;
    }
    parts.push(seg.split('-').map((w) => w.toUpperCase()).join('_'));
  }
  return parts.join('_');
}

/**
 * @param {{ pathParts: string[] }} svc
 * @param {string} menuViewModulePath
 * @returns {string}
 */
function buildI18nPrefixForService(svc, menuViewModulePath) {
  if (svc.pathParts[1] === 'CustomerService') {
    return buildI18nPrefixFromViewParent(menuViewModulePath || computeDefaultViewModulePath(svc));
  }
  return svc.pathParts.flatMap((part) => pascalToKebab(part).split('-').filter(Boolean)).join('.');
}

/**
 * @param {string} entityShort
 * @returns {{ isChangeLog: boolean, masterShort: string }}
 */
function parseChangeLogEntity(entityShort) {
  if (entityShort.endsWith('ChangeLog')) {
    return { isChangeLog: true, masterShort: entityShort.slice(0, -'ChangeLog'.length) };
  }
  return { isChangeLog: false, masterShort: entityShort };
}

/**
 * I18n 末段实体 slug（模块目录与实体前缀全字相同时去重，如 Database+DatabaseInfo→info；materials≠material）
 * @param {string[]} pathParts
 * @param {string} entityShort
 * @returns {string}
 */
function resolveI18nEntitySlug(pathParts, entityShort) {
  const { isChangeLog, masterShort } = parseChangeLogEntity(entityShort);
  const target = isChangeLog ? masterShort : entityShort;
  const modulePath = buildViewModulePrefix(pathParts);
  const deduped = resolvePermissionEntitySlugFromModulePath(target, modulePath);
  if (deduped) {
    return deduped;
  }
  return entityShortToPermissionSlug(target);
}

/**
 * MenuCode 实体段（模块目录与实体前缀全字相同时去重剩余 Pascal 段）
 * @param {string[]} pathParts
 * @param {string} entityShort
 * @returns {string}
 */
function buildMenuCodeEntitySegment(pathParts, entityShort) {
  const { isChangeLog } = parseChangeLogEntity(entityShort);
  if (isChangeLog) {
    return pascalToUpperSnake(entityShort);
  }
  const modulePath = buildViewModulePrefix(pathParts);
  const dedupedSlug = resolvePermissionEntitySlugFromModulePath(entityShort, modulePath);
  const fullSlug = entityShortToPermissionSlug(entityShort);
  if (!dedupedSlug || dedupedSlug === fullSlug) {
    return pascalToUpperSnake(entityShort);
  }
  for (let i = 1; i < entityShort.length; i += 1) {
    if (!/[A-Z]/.test(entityShort[i])) {
      continue;
    }
    const suffix = entityShort.slice(i);
    if (entityShortToPermissionSlug(suffix) === dedupedSlug) {
      return pascalToUpperSnake(suffix);
    }
  }
  return dedupedSlug.replace(/([a-z])([A-Z])/g, '$1_$2').toUpperCase();
}

/**
 * 菜单 list 权限前缀：ChangeLog / OneToMany 从实体（非独立子页）继承主表，不追加 :change:log 或子表词段
 * @param {{ pathParts: string[], entityShort: string }} svc
 * @param {Map<string, { masterPascal: string }>} childRegistry
 * @param {Map<string, { pathParts: string[] }>} [serviceByEntity]
 * @returns {string}
 */
function resolveMenuListPermissionBase(svc, childRegistry, serviceByEntity) {
  const { isChangeLog, masterShort } = parseChangeLogEntity(svc.entityShort);
  if (isChangeLog) {
    const masterSvc = serviceByEntity?.get(masterShort);
    const pathParts = masterSvc ? masterSvc.pathParts : svc.pathParts;
    return buildPermissionBase(pathParts, masterShort).toLowerCase();
  }
  const masterRef = childRegistry.get(svc.entityShort);
  if (masterRef && !isStandaloneChildVueEntity(svc.entityShort)) {
    const masterSvc = serviceByEntity?.get(masterRef.masterPascal);
    const pathParts = masterSvc ? masterSvc.pathParts : svc.pathParts;
    return buildPermissionBase(pathParts, masterRef.masterPascal).toLowerCase();
  }
  if (isServiceLeafSameAsEntity(svc)) {
    return buildPathPermissionSegments(svc.pathParts).join(':');
  }
  return buildPermissionBase(svc.pathParts, svc.entityShort).toLowerCase();
}

/**
 * 服务末级目录与实体同名（如 Routine/Announcement）→ 视图/I18n 不再重复实体段
 * @param {{ pathParts: string[], entityShort: string }} svc
 * @returns {boolean}
 */
function isServiceLeafSameAsEntity(svc) {
  return isModuleLeafSameAsEntityKebab(pascalToKebab(svc.entityShort), buildViewModulePrefix(svc.pathParts));
}

/**
 * @param {{ pathParts: string[], entityShort: string }} svc
 * @param {string} i18nPrefix
 * @param {string} entityShort
 * @returns {string}
 */
function buildExpectedI18nKey(svc, i18nPrefix, entityShort) {
  if (isServiceLeafSameAsEntity(svc)) {
    return `menu.${i18nPrefix}`;
  }
  return `menu.${i18nPrefix}.${resolveI18nEntitySlug(svc.pathParts, entityShort)}`;
}

/**
 * @param {{ pathParts: string[], entityShort: string }} svc
 * @returns {string}
 */
function computeDefaultViewModulePath(svc) {
  const { isChangeLog, masterShort } = parseChangeLogEntity(svc.entityShort);
  const viewPrefix = buildViewModulePrefix(svc.pathParts);
  const entityKebab = pascalToKebab(isChangeLog ? masterShort : svc.entityShort);
  if (isChangeLog) {
    return resolveViewModulePath(viewPrefix, `${entityKebab}-change-log`);
  }
  return resolveViewModulePath(viewPrefix, entityKebab);
}

/**
 * @param {{ pathParts: string[], entityShort: string }} svc
 * @param {Map<string, { masterPascal: string }>} childRegistry
 * @param {string} [menuViewModulePath] 菜单实际 ComponentPath（用于 MenuCode/I18n 前缀与种子一致）
 * @param {Map<string, { pathParts: string[] }>} [serviceByEntity] 实体短名 → 服务路径
 * @returns {{
 *   menuCode: string,
 *   i18nKey: string,
 *   permission: string,
 *   routePath: string,
 *   componentPath: string,
 *   viewModulePath: string,
 *   entityShort: string,
 * }}
 */
function buildExpectedMenuFields(svc, childRegistry, menuViewModulePath, serviceByEntity) {
  const viewModulePath = computeDefaultViewModulePath(svc);
  const prefixSource = menuViewModulePath || viewModulePath;
  const menuCodePrefix = buildMenuCodePrefixForService(svc, prefixSource);
  const i18nPrefix = buildI18nPrefixForService(svc, prefixSource);
  const permissionBase = resolveMenuListPermissionBase(svc, childRegistry, serviceByEntity);
  const { isChangeLog, masterShort } = parseChangeLogEntity(svc.entityShort);

  if (isChangeLog) {
    const masterSlug = resolveI18nEntitySlug(svc.pathParts, masterShort);
    const menuCode = MENU_CODE_OVERRIDES[prefixSource]
      || `${menuCodePrefix}_${pascalToUpperSnake(svc.entityShort)}`;
    return {
      menuCode,
      i18nKey: `menu.${i18nPrefix}.${masterSlug}.changelog`,
      permission: `${permissionBase}:list`,
      routePath: `/${viewModulePath}`,
      componentPath: `${viewModulePath}/index`,
      viewModulePath,
      entityShort: svc.entityShort,
    };
  }

  const menuEntitySegment = buildMenuCodeEntitySegment(svc.pathParts, svc.entityShort);
  const menuCode = MENU_CODE_OVERRIDES[prefixSource]
    || `${menuCodePrefix}_${menuEntitySegment}`;
  return {
    menuCode,
    i18nKey: buildExpectedI18nKey(svc, i18nPrefix, svc.entityShort),
    permission: `${permissionBase}:list`,
    routePath: `/${viewModulePath}`,
    componentPath: `${viewModulePath}/index`,
    viewModulePath,
    entityShort: svc.entityShort,
  };
}

/**
 * @param {string} viewModulePath
 * @param {Array<{ entityShort: string, pathParts: string[] }>} services
 * @returns {{ entityShort: string, pathParts: string[] }|null}
 */
function resolveServiceFromViewModulePath(viewModulePath, services) {
  if (!viewModulePath || isShellViewPage(viewModulePath)) {
    return null;
  }
  if (VIEW_MODULE_PATH_ENTITY_OVERRIDES[viewModulePath]) {
    const entityShort = VIEW_MODULE_PATH_ENTITY_OVERRIDES[viewModulePath];
    return services.find((svc) => svc.entityShort === entityShort) || null;
  }
  const leaf = viewModulePath.split('/').pop() || '';
  const leafNorm = leaf.replace(/-/g, '').toLowerCase();
  for (const svc of services) {
    if (pascalToKebab(svc.entityShort) === leaf) {
      return svc;
    }
  }
  for (const svc of services) {
    const kebabNorm = pascalToKebab(svc.entityShort).replace(/-/g, '');
    if (kebabNorm === leafNorm) {
      return svc;
    }
  }
  if (leaf.endsWith('-change-log')) {
    const masterKebab = leaf.slice(0, -'-change-log'.length);
    const masterNorm = masterKebab.replace(/-/g, '').toLowerCase();
    for (const svc of services) {
      if (!svc.entityShort.endsWith('ChangeLog')) {
        continue;
      }
      const masterShort = svc.entityShort.slice(0, -'ChangeLog'.length);
      const masterKebabFromEntity = pascalToKebab(masterShort);
      if (masterKebabFromEntity === masterKebab
        || masterKebabFromEntity.replace(/-/g, '') === masterNorm) {
        return svc;
      }
    }
  }
  return null;
}

module.exports = {
  SKIP_MENU_CODES,
  VIEW_MODULE_PATH_ENTITY_OVERRIDES,
  pascalToKebab,
  isShellViewPage,
  buildExpectedMenuFields,
  resolveMenuListPermissionBase,
  resolveServiceFromViewModulePath,
};
