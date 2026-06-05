// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/scripts
// 文件名称：generate-script-common.cjs
// 创建时间：2026-05-23
// 创建人：Takt365(Cursor AI)
// 功能描述：代码生成脚本公共工具（文件写入、Service 单数 / Controller 复数命名）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const fs = require('fs');
const path = require('path');

/**
 * REST 控制器复数特例（键为实体短名单数，与 ITaktXxxService 一致）
 */
const CONTROLLER_PLURAL_OVERRIDES = {
  User: 'Users',
  Auth: 'Auths',
  Company: 'Companies',
  Culture: 'Cultures',
  TranslationMessage: 'TranslationMessages',
  HolidayTheme: 'HolidayThemes',
  DataDictAll: 'DataDictAlls',
};

/**
 * 将实体短名复数化（用于 Takt{Plural}Controller 与 api/Takt{Plural} 路由）
 * @param {string} entityShort 如 Holiday、DictData、HolidayTheme
 * @returns {string} 如 Holidays、DictDatas、HolidayThemes
 */
function pluralizeEntityShort(entityShort) {
  if (!entityShort) {
    return entityShort;
  }
  if (CONTROLLER_PLURAL_OVERRIDES[entityShort]) {
    return CONTROLLER_PLURAL_OVERRIDES[entityShort];
  }
  if (entityShort.endsWith('y') && !/[aeiou]y$/i.test(entityShort)) {
    return `${entityShort.slice(0, -1)}ies`;
  }
  if (entityShort.endsWith('Company')) {
    return `${entityShort.slice(0, -7)}Companies`;
  }
  if (/(s|x|z|ch|sh)$/i.test(entityShort)) {
    return `${entityShort}es`;
  }
  return `${entityShort}s`;
}

/**
 * 从控制器路由段（无 Takt 前缀）还原实体短名单数
 * @param {string} segment 如 Holidays、Users、Companies
 * @returns {string} 如 Holiday、User、Company
 */
function singularizeControllerSegment(segment) {
  if (!segment) {
    return segment;
  }
  for (const [singular, plural] of Object.entries(CONTROLLER_PLURAL_OVERRIDES)) {
    if (segment === plural) {
      return singular;
    }
  }
  if (segment.endsWith('ies')) {
    return `${segment.slice(0, -3)}y`;
  }
  if (segment.endsWith('Companies')) {
    return `${segment.slice(0, -10)}Company`;
  }
  if (segment.endsWith('es')) {
    const base = segment.slice(0, -2);
    if (/(s|x|z|ch|sh)$/i.test(base)) {
      return base;
    }
  }
  if (segment.endsWith('s') && segment.length > 1) {
    return segment.slice(0, -1);
  }
  return segment;
}

/**
 * 由实体完整类名生成控制器类名（复数 + Controller）
 * @param {string} entityName 如 TaktHoliday、TaktUser
 * @returns {string} 如 TaktHolidaysController、TaktUsersController
 */
function getControllerClassName(entityName) {
  const entityShort = entityName.replace(/^Takt/, '');
  return `Takt${pluralizeEntityShort(entityShort)}Controller`;
}

/**
 * 由实体完整类名生成 [controller] 路由段（含 Takt 前缀）
 * @param {string} entityName 如 TaktHoliday
 * @returns {string} 如 TaktHolidays
 */
function getControllerRouteSegment(entityName) {
  const entityShort = entityName.replace(/^Takt/, '');
  return `Takt${pluralizeEntityShort(entityShort)}`;
}

/**
 * 从控制器类名解析实体短名（单数，用于 DTO/权限/前端 types 文件名）
 * @param {string} controllerClassName 如 TaktHolidaysController
 * @returns {string} 如 Holiday
 */
function entityShortFromControllerClassName(controllerClassName) {
  const segment = controllerClassName.replace(/^Takt/, '').replace(/Controller$/, '');
  return singularizeControllerSegment(segment);
}

/**
 * CLI 实体前缀是否匹配控制器类名（仅接受复数控制器名）
 * @param {string} controllerClassName 如 TaktHolidaysController
 * @param {string} entityPrefix 如 Holiday
 * @returns {boolean}
 */
function matchControllerForEntityPrefix(controllerClassName, entityPrefix) {
  return controllerClassName === getControllerClassName(`Takt${entityPrefix}`);
}

/**
 * 全栈 generate-*.cjs / generate-all.cjs 统一文件写入策略
 * @type {string}
 */
const GENERATED_FILE_WRITE_POLICY = '不存在则创建，已存在则覆盖更新';

/**
 * 启动时打印统一写入策略
 */
function logGeneratedFileWritePolicy() {
  console.log(`📝 写入策略：${GENERATED_FILE_WRITE_POLICY}\n`);
}

/**
 * 写入生成文件：目录不存在则创建；文件已存在则覆盖更新
 * @param {string} filePath 目标文件绝对或相对路径
 * @param {string} content 文件内容
 * @returns {{ created: boolean, updated: boolean }}
 */
function writeGeneratedFile(filePath, content) {
  const dir = path.dirname(filePath);
  if (!fs.existsSync(dir)) {
    fs.mkdirSync(dir, { recursive: true });
  }
  const existed = fs.existsSync(filePath);
  fs.writeFileSync(filePath, content, 'utf-8');
  return { created: !existed, updated: existed };
}

module.exports = {
  GENERATED_FILE_WRITE_POLICY,
  logGeneratedFileWritePolicy,
  writeGeneratedFile,
  CONTROLLER_PLURAL_OVERRIDES,
  pluralizeEntityShort,
  singularizeControllerSegment,
  getControllerClassName,
  getControllerRouteSegment,
  entityShortFromControllerClassName,
  matchControllerForEntityPrefix,
};
