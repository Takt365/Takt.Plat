// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/scripts
// 文件名称：generate-enum-common.cjs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：代码生成脚本共享枚举工具（扫描 Takt.Shared.Enums、识别实体/DTO 枚举字段）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const fs = require('fs');
const path = require('path');

const DEFAULT_ENUMS_ROOT = path.resolve(__dirname, '../backend/src/Takt.Shared/Enums');

/** @type {{ names: Set<string>, members: Map<string, string[]> } | null} */
let sharedEnumRegistryCache = null;

/** 下拉/树选项「启用」语义成员名（按优先级） */
const ENABLED_MEMBER_CANDIDATES = [
  'Enabled',
  'Normal',
  'Active',
  'Subsisting',
  'Running',
  'Published',
  'Unread',
];

const NAVIGATION_REGION_MARKER = '导航属性区域';

/**
 * 扫描 Takt.Shared/Enums 下 public enum 定义
 * @param {string} [enumsRoot]
 * @returns {{ names: Set<string>, members: Map<string, string[]> }}
 */
function loadSharedEnumRegistry(enumsRoot = DEFAULT_ENUMS_ROOT) {
  if (sharedEnumRegistryCache) {
    return sharedEnumRegistryCache;
  }
  const names = new Set();
  const members = new Map();
  if (!fs.existsSync(enumsRoot)) {
    sharedEnumRegistryCache = { names, members };
    return sharedEnumRegistryCache;
  }
  for (const file of fs.readdirSync(enumsRoot)) {
    if (!file.endsWith('.cs')) {
      continue;
    }
    const content = fs.readFileSync(path.join(enumsRoot, file), 'utf8');
    const enumBlocks = content.matchAll(/public\s+enum\s+(Takt\w+)\s*\{([\s\S]*?)\n\}/g);
    for (const block of enumBlocks) {
      const enumName = block[1];
      names.add(enumName);
      const memberNames = [];
      const memberRegex = /^\s*(\w+)\s*=/gm;
      let memberMatch;
      while ((memberMatch = memberRegex.exec(block[2])) !== null) {
        memberNames.push(memberMatch[1]);
      }
      members.set(enumName, memberNames);
    }
  }
  sharedEnumRegistryCache = { names, members };
  return sharedEnumRegistryCache;
}

/**
 * 是否为 Takt.Shared.Enums 中定义的枚举类型
 * @param {string} typeName 可含尾部 ?
 */
function isSharedEnumType(typeName) {
  if (!typeName) {
    return false;
  }
  const bare = typeName.replace('?', '').trim();
  return loadSharedEnumRegistry().names.has(bare);
}

/**
 * 解析枚举的「启用/正常」字面量（用于 Options/Tree 过滤）；无合适成员时返回 null
 * @param {string} enumType
 * @returns {string|null} 如 TaktCommonStatus.Enabled
 */
function getSharedEnumEnabledLiteral(enumType) {
  const bare = enumType.replace('?', '').trim();
  const { members } = loadSharedEnumRegistry();
  const enumMembers = members.get(bare) || [];
  for (const candidate of ENABLED_MEMBER_CANDIDATES) {
    if (enumMembers.includes(candidate)) {
      return `${bare}.${candidate}`;
    }
  }
  return null;
}

/**
 * 解析实体标量属性（导航属性区域之前）
 * @param {string} entityContent
 * @returns {Array<{ name: string, bareType: string, summary: string }>}
 */
function parseEntityScalarProperties(entityContent) {
  let body = entityContent;
  const markerIdx = entityContent.indexOf(NAVIGATION_REGION_MARKER);
  if (markerIdx !== -1) {
    body = entityContent.slice(0, markerIdx);
  }
  const properties = [];
  const propertyRegex =
    /\/\/\/\s*<summary>([\s\S]*?)<\/summary>[\s\S]*?public\s+((?:List<)?(?:Takt\w+|[a-zA-Z][\w]*)(?:>)?(?:\?)?)\s+(\w+)\s*\{\s*get;\s*set;/g;
  let match;
  while ((match = propertyRegex.exec(body)) !== null) {
    const summary = match[1].replace(/\s+/g, ' ').trim();
    const csharpType = match[2].trim();
    const name = match[3];
    if (csharpType.startsWith('List<') || /\[Navigate\s*\(/.test(match[0])) {
      continue;
    }
    properties.push({
      name,
      bareType: csharpType.replace('?', ''),
      summary,
    });
  }
  return properties;
}

/**
 * 查找实体的状态字段（优先 TaktCommonStatus 的 *Status）
 * @param {object[]} properties parseEntityScalarProperties 或 DTO 解析结果
 */
function findEntityStatusProperty(properties) {
  const statuses = properties.filter((p) => /Status$/i.test(p.name) || p.name === 'Status');
  return (
    statuses.find((p) => p.bareType === 'TaktCommonStatus')
    || statuses.find((p) => isSharedEnumType(p.bareType))
    || statuses.find((p) => p.bareType === 'int')
    || statuses[0]
    || null
  );
}

/**
 * 实体属性列表是否引用 Shared 枚举
 * @param {object[]} properties
 */
function entityUsesSharedEnumsFromProperties(properties) {
  return properties.some((p) => isSharedEnumType(p.bareType));
}

/**
 * C# 源码是否引用 Shared 枚举类型名
 * @param {string} content
 */
function contentUsesSharedEnums(content) {
  if (!content) {
    return false;
  }
  const { names } = loadSharedEnumRegistry();
  for (const name of names) {
    if (new RegExp(`\\b${name}\\b`).test(content)) {
      return true;
    }
  }
  return false;
}

/**
 * 提取用于 Options/Tree「仅启用项」过滤的状态字段元数据
 * @param {string} entityContent
 * @param {string} [entityShort]
 * @returns {{ field: string, kind: 'sharedEnum'|'int', enumType?: string, enabledLiteral?: string, intEnabled?: number }|null}
 */
function extractPrimaryEnableStatusMeta(entityContent, entityShort = '') {
  const props = parseEntityScalarProperties(entityContent);
  const statusProp = findEntityStatusProperty(props);
  if (!statusProp) {
    return null;
  }
  if (statusProp.bareType === 'TaktCommonStatus') {
    return {
      field: statusProp.name,
      kind: 'sharedEnum',
      enumType: 'TaktCommonStatus',
      enabledLiteral: 'TaktCommonStatus.Enabled',
    };
  }
  if (isSharedEnumType(statusProp.bareType)) {
    const enabledLiteral = getSharedEnumEnabledLiteral(statusProp.bareType);
    if (enabledLiteral) {
      return {
        field: statusProp.name,
        kind: 'sharedEnum',
        enumType: statusProp.bareType,
        enabledLiteral,
      };
    }
    return null;
  }
  if (statusProp.bareType === 'int') {
    return {
      field: statusProp.name,
      kind: 'int',
      intEnabled: 1,
    };
  }
  return null;
}

/**
 * 内置项禁用保护：解析状态字段（TaktCommonStatus / int EmployeeStatus 等）
 * @param {string} entityContent
 * @returns {{ field: string, kind: 'commonStatus'|'intEnabled'|'employeeResigned' }|null}
 */
function extractBuiltInDisableStatusMeta(entityContent) {
  if (!/public\s+TaktYesNo\s+IsBuiltIn\s*\{/.test(entityContent)) {
    return null;
  }
  const props = parseEntityScalarProperties(entityContent);
  if (props.some((p) => p.name === 'EmployeeStatus')) {
    return { field: 'EmployeeStatus', kind: 'employeeResigned' };
  }
  const statusProp = findEntityStatusProperty(props);
  if (!statusProp) {
    return null;
  }
  if (statusProp.bareType === 'TaktCommonStatus') {
    return { field: statusProp.name, kind: 'commonStatus' };
  }
  if (statusProp.bareType === 'int') {
    return { field: statusProp.name, kind: 'intEnabled' };
  }
  return null;
}

/**
 * Options 实现是否含过时的 int 字面量比较（enum 字段却写 == 1）
 * @param {string|null} block
 * @param {{ field?: string, kind?: string }|null} statusMeta
 */
function optionsBlockUsesStaleIntStatusCompare(block, statusMeta) {
  if (!block || !statusMeta?.field || statusMeta.kind !== 'sharedEnum') {
    return false;
  }
  return new RegExp(`\\b${statusMeta.field}\\s*==\\s*1\\b`).test(block);
}

module.exports = {
  loadSharedEnumRegistry,
  isSharedEnumType,
  getSharedEnumEnabledLiteral,
  parseEntityScalarProperties,
  findEntityStatusProperty,
  entityUsesSharedEnumsFromProperties,
  contentUsesSharedEnums,
  extractPrimaryEnableStatusMeta,
  extractBuiltInDisableStatusMeta,
  optionsBlockUsesStaleIntStatusCompare,
  ENABLED_MEMBER_CANDIDATES,
};
