// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：scripts
// 文件名称：generate-entity-exclusions.cjs
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：代码生成架构约束（RBAC 八表、独立服务、）；单实体模式（--Entity）下不再维护「防 --all 覆盖」手工 CRUD 排除表
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const path = require('path');
const { entityShortFromControllerClassName } = require('./generate-script-common.cjs');

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
 * 无实体、手工维护的独立应用服务（跳过服务/控制器脚本扫描）
 */
const MANUAL_STANDALONE_SERVICE_ENTITY_NAMES = new Set([
  'TaktAuth',
  'TaktRbac',
  'TaktFlowEngine',
  'TaktFileUploadEngine',
]);

/**
 * 手工维护 DTO 的实体（含脚本无法生成的附加 DTO 类，禁止 generate-dtos-from-entity 覆盖）
 * TaktHoliday：TaktHolidayThemeDto（登录前假日主题预览，与实体字段并列但非实体映射）
 * TableArchive / DatabaseBackup：数据归档与备份编排（CRUD 外另有 preview/execute/run/schedule，禁止流水线覆盖）
 */
const MANUAL_DTO_ENTITY_SHORT_NAMES = new Set([
  'Holiday',
  'TableArchive',
  'DatabaseBackup',
]);

/**
 * OneToMany 从实体但仍保留独立 CRUD 菜单页（不因 masterDetailChildRegistry 跳过 Vue 生成）
 * 例：TaktQuartzTask.QuartzLogs 与 statistics/logging/quartz-log 独立页并存
 */
const STANDALONE_CHILD_VUE_ENTITY_SHORT_NAMES = new Set([
  'QuartzLog',
  'CustomerServiceRequest',
  'CustomerServiceOrder',
  'CustomerServiceTicket',
]);

/**
 * @param {string} entityShort 实体短名（全字）
 * @returns {boolean}
 */
function isRbacJunctionEntity(entityShort) {
  return RBAC_ASSOCIATION_ENTITY_SHORT_NAMES.has(entityShort);
}

/**
 * 是否为手工维护 DTO 的实体（禁止 DTO/前端 types/i18n 流水脚本覆盖）
 * @param {string} entityShort
 * @returns {boolean}
 */
function isManualDtoEntity(entityShort) {
  return MANUAL_DTO_ENTITY_SHORT_NAMES.has(entityShort);
}

/**
 * 单实体 CLI：禁止对手工维护实体运行任何代码生成流水脚本
 * @param {string} entityShort
 */
function assertNotManualDtoEntityCli(entityShort) {
  if (!isManualDtoEntity(entityShort)) {
    return;
  }
  console.error(
    `❌ 实体 ${entityShort} 为手工维护模块（含附加 DTO/业务逻辑），禁止代码生成流水脚本（generate-all / generate-dtos / generate-from-backend / generate-services / generate-controllers / generate-vue-* 等）。`,
  );
  console.error('   例：TaktHoliday 含 TaktHolidayThemeDto 与主题预览 API，须手工同步实体、DTO、服务、i18n 种子与前端 types/api/views。');
  process.exit(1);
}

/**
 * 单实体 CLI：禁止对 RBAC 关联表生成独立 DTO/服务/控制器
 * @param {string} entityShort
 */
function assertNotRbacJunctionEntityCli(entityShort) {
  if (!isRbacJunctionEntity(entityShort)) {
    return;
  }
  console.error(
    `❌ 实体 ${entityShort} 为 RBAC 关联表，由 ITaktRbacService 统一维护，禁止本脚本生成独立 CRUD。`,
  );
  console.error(`   关联表: ${[...RBAC_ASSOCIATION_ENTITY_SHORT_NAMES].join('、')}`);
  process.exit(1);
}

/**
 * DTO 文件名 → 实体短名
 * @param {string} dtoFileName 如 TaktUserRoleDtos.cs
 * @returns {string|null}
 */
function entityShortFromDtoFileName(dtoFileName) {
  const base = path.basename(dtoFileName, '.cs');
  if (!base.endsWith('Dtos') || !base.startsWith('Takt')) {
    return null;
  }
  return base.slice(4, -'Dtos'.length);
}

/**
 * @param {string} entityName
 * @returns {boolean}
 */
function shouldExcludeStandaloneService(entityName) {
  return MANUAL_STANDALONE_SERVICE_ENTITY_NAMES.has(entityName);
}

/**
 * 服务脚本：是否跳过 *Dtos.cs（RBAC 八表）
 * @param {string} dtoFile
 * @returns {boolean}
 */
function shouldExcludeDtoFile(dtoFile) {
  const entityShort = entityShortFromDtoFileName(path.basename(dtoFile));
  if (entityShort != null && isRbacJunctionEntity(entityShort)) {
    return true;
  }
  return entityShort != null && isManualDtoEntity(entityShort);
}

/**
 * 前端脚本：是否跳过 *Dtos.cs 源文件（RBAC 八表）
 * @param {string} sourceFileBase 如 TaktUserRoleDtos
 * @returns {boolean}
 */
function shouldExcludeDtoSourceBase(sourceFileBase) {
  const entityShort = entityShortFromDtoFileName(`${sourceFileBase}.cs`);
  if (entityShort != null && isRbacJunctionEntity(entityShort)) {
    return true;
  }
  return entityShort != null && isManualDtoEntity(entityShort);
}

/**
 * @param {string} controllerName
 * @returns {string}
 */
function entityShortFromControllerName(controllerName) {
  return entityShortFromControllerClassName(controllerName);
}

/**
 * 前端脚本：是否跳过控制器（RBAC 八表 + 独立服务 + 手工维护模块）
 * @param {string} controllerName
 * @returns {boolean}
 */
function shouldExcludeController(controllerName) {
  const entityShort = entityShortFromControllerName(controllerName);
  if (isRbacJunctionEntity(entityShort)) {
    return true;
  }
  if (isManualDtoEntity(entityShort)) {
    return true;
  }
  const entityName = entityShort ? `Takt${entityShort}` : '';
  return MANUAL_STANDALONE_SERVICE_ENTITY_NAMES.has(entityName);
}


/**
 * 是否为「从实体但仍有独立 index/form」的 Vue 模块（覆盖 masterDetailChildRegistry 跳过）
 * @param {string} entityShort 实体短名（如 QuartzLog）
 * @returns {boolean}
 */
function isStandaloneChildVueEntity(entityShort) {
  return STANDALONE_CHILD_VUE_ENTITY_SHORT_NAMES.has(entityShort);
}

/**
 * Vue 视图/表单生成脚本：是否跳过该 API 模块
 * @param {string} _apiRelPath 相对 frontend/src/api 的路径（保留参数以兼容调用方）
 * @param {string} entityShort 实体短名（如 Menu、FlowScheme）
 * @returns {boolean}
 */
function shouldExcludeVueGeneration(_apiRelPath, entityShort) {
  return isManualDtoEntity(entityShort);
}

module.exports = {
  RBAC_ASSOCIATION_ENTITY_SHORT_NAMES,
  MANUAL_STANDALONE_SERVICE_ENTITY_NAMES,
  MANUAL_DTO_ENTITY_SHORT_NAMES,
  STANDALONE_CHILD_VUE_ENTITY_SHORT_NAMES,
  isRbacJunctionEntity,
  isManualDtoEntity,
  assertNotRbacJunctionEntityCli,
  assertNotManualDtoEntityCli,
  shouldExcludeStandaloneService,
  shouldExcludeDtoFile,
  shouldExcludeDtoSourceBase,
  entityShortFromControllerName,
  shouldExcludeController,
  shouldExcludeVueGeneration,
  isStandaloneChildVueEntity,
};
