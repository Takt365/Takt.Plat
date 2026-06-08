// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/scripts
// 文件名称：generate-entity-exclusions.cjs
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：代码生成脚本共享排除规则（User/Online/Message 手工 CRUD；RBAC 八表；独立服务；Vue 视图排除；*ChangeLog 无独立视图）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const path = require('path');
const { entityShortFromControllerClassName } = require('./generate-script-common.cjs');

/**
 * 禁止脚本自动生成 DTO/服务/控制器的实体短名（全字匹配，禁止模糊）
 * User（含密码等字段）、Online、Message 手工维护；不得匹配 UserCompany、UserRole 等
 * （后者由 RBAC_ASSOCIATION_ENTITY_SHORT_NAMES 处理）
 */
const EXCLUDED_ENTITY_SHORT_NAMES = new Set(['User', 'Online', 'Message', 'DictData', 'DictType']);

/**
 * 禁止脚本扫描的 DTO 源文件名（全字匹配，必须为 TaktXxxDtos.cs，见 00-project §1.1）
 * 类名仍为 TaktXxxDto / TaktXxxQueryDto 等（禁止类名使用 Dtos 后缀）
 */
const EXCLUDED_DTO_FILE_NAMES = new Set([
  'TaktUserDtos.cs',
  'TaktOnlineDtos.cs',
  'TaktMessageDtos.cs',
  'TaktDictDataDtos.cs',
  'TaktDictTypeDtos.cs',
  'TaktUserRoleDtos.cs',
  'TaktUserTenantDtos.cs',
  'TaktUserCompanyDtos.cs',
  'TaktRoleMenuDtos.cs',
  'TaktRoleCompanyDtos.cs',
  'TaktRoleDeptDtos.cs',
  'TaktEmployeeDeptDtos.cs',
  'TaktEmployeePostDtos.cs',
  // 独立模块：仅含响应 DTO，前端 types/api 手工维护
  'TaktDataDictAllDtos.cs',
  'TaktTranslationMessagesDtos.cs',
  'TaktHolidayThemeDtos.cs',
]);

/**
 * RBAC 八张关联表：仅 TaktRbacService / TaktRbacsController，禁止脚本生成独立 CRUD
 */
const RBAC_ASSOCIATION_ENTITY_SHORT_NAMES = new Set([
  'UserRole',
  'UserTenant',
  'UserCompany',
  'RoleMenu',
  'RoleCompany',
  'RoleDept',
  'EmployeeDept',
  'EmployeePost',
]);

/**
 * 无实体、手工维护的独立应用服务（跳过服务/控制器脚本扫描，避免覆盖）
 */
const MANUAL_STANDALONE_SERVICE_ENTITY_NAMES = new Set([
  'TaktAuth',
  'TaktRbac',
  'TaktFlowEngine',
  'TaktFileUploadEngine',
  'TaktHolidayTheme',
  'TaktTranslationMessage',
  'TaktDataDictAll',
]);

/** Vue 视图/表单生成排除的实体短名（手工 CRUD 或专用 UI，见 generate-vue-*-from-api.cjs） */
const EXCLUDED_VUE_ENTITY_SHORT_NAMES = new Set([
  'User',
  'Users',
  'Menu',
  'Dept',
  'DictType',
  'DictData',
  'GenTable',
  'GenTableColumn',
  'Culture',
  'Translation',
  'Numbering',
  'Online',
  'Message',
]);

/** Vue 生成：整目录排除（相对 frontend/src/api 的路径前缀） */
const EXCLUDED_VUE_API_PATH_PREFIXES = [
  'workflow/',
];

/** @deprecated 与 EXCLUDED_ENTITY_SHORT_NAMES 同义 */
const MANUAL_CRUD_ENTITY_SHORT_NAMES = EXCLUDED_ENTITY_SHORT_NAMES;

/** @deprecated 与 EXCLUDED_DTO_FILE_NAMES 同义 */
const MANUAL_CRUD_DTO_FILE_NAMES = EXCLUDED_DTO_FILE_NAMES;

/** @deprecated 使用 RBAC_ASSOCIATION_ENTITY_SHORT_NAMES */
const RBAC_JUNCTION_ENTITY_SHORT_NAMES = RBAC_ASSOCIATION_ENTITY_SHORT_NAMES;

/** @deprecated 不再使用虚构 DTO 排除列表 */
const PHANTOM_DTO_ENTITY_SHORT_NAMES = new Set();

/** @deprecated */
const MANUAL_VALIDATOR_ENTITY_SHORT_NAMES = new Set([]);

/** @deprecated 前端 DTO 排除与 EXCLUDED_DTO_FILE_NAMES 一致 */
const MANUAL_FRONTEND_DTO_FILE_NAMES = EXCLUDED_DTO_FILE_NAMES;

/**
 * @param {string} entityShort 实体短名（全字）
 * @returns {boolean}
 */
function isExcludedEntity(entityShort) {
  return EXCLUDED_ENTITY_SHORT_NAMES.has(entityShort)
    || RBAC_ASSOCIATION_ENTITY_SHORT_NAMES.has(entityShort);
}

/** @deprecated */
function isManualCrudEntity(entityShort) {
  return isExcludedEntity(entityShort);
}

/**
 * @param {string} entityShort
 * @returns {boolean}
 */
function isRbacJunctionEntity(entityShort) {
  return RBAC_ASSOCIATION_ENTITY_SHORT_NAMES.has(entityShort);
}

/** @deprecated 始终 false */
function isPhantomDtoEntity() {
  return false;
}

/** @deprecated */
function isManualValidatorEntity() {
  return false;
}

/**
 * @param {string} entityShort
 * @returns {boolean}
 */
function isManualFrontendEntity(entityShort) {
  return EXCLUDED_ENTITY_SHORT_NAMES.has(entityShort);
}

/** @deprecated */
function isSpecialEntity(entityShort) {
  return isExcludedEntity(entityShort);
}

/**
 * 服务脚本：是否跳过 *Dtos.cs（仅文件名全字匹配）
 * @param {string} dtoFile
 * @returns {boolean}
 */
function shouldExcludeDtoFile(dtoFile) {
  return EXCLUDED_DTO_FILE_NAMES.has(path.basename(dtoFile));
}

/**
 * 前端脚本：是否跳过 *Dtos.cs 源文件（User/Online/Message、RBAC 八表等）
 * @param {string} sourceFileBase 如 TaktOnlineDtos
 * @returns {boolean}
 */
function shouldExcludeDtoSourceBase(sourceFileBase) {
  return EXCLUDED_DTO_FILE_NAMES.has(`${sourceFileBase}.cs`);
}

/**
 * @param {string} controllerName
 * @returns {string}
 */
function entityShortFromControllerName(controllerName) {
  return entityShortFromControllerClassName(controllerName);
}

/**
 * 前端脚本：是否跳过控制器
 * @param {string} controllerName
 * @returns {boolean}
 */
function shouldExcludeController(controllerName) {
  const entityShort = entityShortFromControllerName(controllerName);
  if (isExcludedEntity(entityShort)) {
    return true;
  }
  const entityName = entityShort ? `Takt${entityShort}` : '';
  return MANUAL_STANDALONE_SERVICE_ENTITY_NAMES.has(entityName);
}

/**
 * @param {string} entityName
 * @returns {boolean}
 */
function shouldExcludeStandaloneService(entityName) {
  return MANUAL_STANDALONE_SERVICE_ENTITY_NAMES.has(entityName);
}

/**
 * 是否为变更日志从属实体（*ChangeLog：无独立 index.vue / *-form.vue，由主实体页承载）
 * @param {string} entityShort 实体短名（如 CostCenterChangeLog）
 * @returns {boolean}
 */
function isChangeLogEntity(entityShort) {
  return Boolean(entityShort && entityShort.endsWith('ChangeLog'));
}

/**
 * Vue 视图/表单生成脚本：是否跳过该 API 模块
 * @param {string} apiRelPath 相对 frontend/src/api 的路径（如 identity/menu.ts）
 * @param {string} entityShort 实体短名（如 Menu、FlowScheme）
 * @returns {boolean}
 */
function shouldExcludeVueGeneration(apiRelPath, entityShort) {
  const normalized = apiRelPath.replace(/\\/g, '/');
  if (EXCLUDED_VUE_ENTITY_SHORT_NAMES.has(entityShort)) {
    return true;
  }
  if (isChangeLogEntity(entityShort)) {
    return true;
  }
  return EXCLUDED_VUE_API_PATH_PREFIXES.some((prefix) => normalized.startsWith(prefix));
}

module.exports = {
  EXCLUDED_ENTITY_SHORT_NAMES,
  EXCLUDED_DTO_FILE_NAMES,
  MANUAL_CRUD_ENTITY_SHORT_NAMES,
  RBAC_ASSOCIATION_ENTITY_SHORT_NAMES,
  RBAC_JUNCTION_ENTITY_SHORT_NAMES,
  PHANTOM_DTO_ENTITY_SHORT_NAMES,
  MANUAL_VALIDATOR_ENTITY_SHORT_NAMES,
  MANUAL_FRONTEND_DTO_FILE_NAMES,
  MANUAL_CRUD_DTO_FILE_NAMES,
  MANUAL_STANDALONE_SERVICE_ENTITY_NAMES,
  EXCLUDED_VUE_ENTITY_SHORT_NAMES,
  EXCLUDED_VUE_API_PATH_PREFIXES,
  isExcludedEntity,
  isManualCrudEntity,
  isRbacJunctionEntity,
  isPhantomDtoEntity,
  isManualValidatorEntity,
  isManualFrontendEntity,
  isSpecialEntity,
  shouldExcludeDtoFile,
  shouldExcludeDtoSourceBase,
  entityShortFromControllerName,
  shouldExcludeController,
  shouldExcludeStandaloneService,
  shouldExcludeVueGeneration,
  isChangeLogEntity,
};
