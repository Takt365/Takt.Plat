// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：scripts
// 文件名称：add-is-obsolete-to-detail-entities.cjs
// 创建时间：2026-07-09
// 创建人：Takt365(Cursor AI)
// 功能描述：为所有子表明细实体（含 LineNumber + ManyToOne）在导航属性前追加 IsObsolete 字段
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const fs = require('fs');
const path = require('path');
const { writeGeneratedFile } = require('../gen/generate-script-common.cjs');

const ENTITIES_ROOT = path.join(path.resolve(__dirname, '../../backend/src'), 'Takt.Domain', 'Entities');
const NAVIGATION_REGION_MARKER = '导航属性区域';

const IS_OBSOLETE_PROPERTY_BLOCK = `    /// <summary>
    /// 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    [SugarColumn(ColumnName = "is_obsolete", ColumnDescription = "是否作废", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsObsolete { get; set; } = 0;
`;

/**
 * 递归收集实体 .cs 文件
 * @param {string} dir
 * @returns {string[]}
 */
function collectEntityFiles(dir) {
  const files = [];
  for (const entry of fs.readdirSync(dir, { withFileTypes: true })) {
    const full = path.join(dir, entry.name);
    if (entry.isDirectory()) {
      files.push(...collectEntityFiles(full));
      continue;
    }
    if (entry.name.startsWith('Takt') && entry.name.endsWith('.cs')) {
      files.push(full);
    }
  }
  return files;
}

/** 非主子表外键的 long Id 字段（树 ParentId、审批流等） */
const NON_MASTER_FK_IDS = new Set([
  'ParentId',
  'FlowInstanceId',
  'InitiatorId',
  'ApprovedBy',
]);

/**
 * 是否为子表明细实体：含 LineNumber 且含指向主表的外键 long *Id
 * @param {string} content
 * @param {string} filePath
 * @returns {boolean}
 */
function isDetailChildEntityContent(content, filePath) {
  if (!/\bpublic\s+int\s+LineNumber\s*\{/.test(content)) {
    return false;
  }
  if (/\bIsObsolete\s*\{/.test(content)) {
    return false;
  }
  if (filePath.includes(`${path.sep}Code${path.sep}Generator${path.sep}TaktGenTableColumn.cs`)) {
    return false;
  }
  const fkRegex = /public\s+long\s+(\w+Id)\s*\{/g;
  let match;
  while ((match = fkRegex.exec(content)) !== null) {
    const fkName = match[1];
    if (!NON_MASTER_FK_IDS.has(fkName)) {
      return true;
    }
  }
  return false;
}

/**
 * 在导航属性区域前插入 IsObsolete
 * @param {string} content
 * @returns {string|null} 更新后内容；null 表示无需修改
 */
function insertIsObsoleteBeforeNavigation(content) {
  if (/\bIsObsolete\s*\{/.test(content)) {
    return null;
  }
  const markerIdx = content.indexOf(NAVIGATION_REGION_MARKER);
  if (markerIdx !== -1) {
    let insertPos = markerIdx;
    const beforeMarker = content.slice(0, markerIdx);
    const regionStart = beforeMarker.lastIndexOf('// ========================================');
    if (regionStart !== -1) {
      insertPos = regionStart;
    }
    const before = content.slice(0, insertPos).replace(/\s+$/, '');
    const after = content.slice(insertPos);
    return `${before}\n\n${IS_OBSOLETE_PROPERTY_BLOCK}\n${after.startsWith('\n') ? after : `\n${after}`}`;
  }
  const navigateMatch = content.match(/\n(\s*\/\/\/[^\n]*\n\s*\[Navigate)/);
  if (navigateMatch && navigateMatch.index !== undefined) {
    const insertPos = navigateMatch.index;
    const before = content.slice(0, insertPos).replace(/\s+$/, '');
    const after = content.slice(insertPos);
    return `${before}\n\n${IS_OBSOLETE_PROPERTY_BLOCK}\n${after}`;
  }
  const classClose = content.lastIndexOf('\n}');
  if (classClose === -1) {
    return null;
  }
  const before = content.slice(0, classClose).replace(/\s+$/, '');
  const after = content.slice(classClose);
  return `${before}\n\n${IS_OBSOLETE_PROPERTY_BLOCK}${after}`;
}

/**
 * @param {{ dryRun?: boolean }} options
 */
function main(options = {}) {
  const files = collectEntityFiles(ENTITIES_ROOT);
  const matched = [];
  const updated = [];
  const skipped = [];
  for (const filePath of files) {
    const content = fs.readFileSync(filePath, 'utf-8');
    if (!isDetailChildEntityContent(content, filePath)) {
      continue;
    }
    matched.push(filePath);
    const next = insertIsObsoleteBeforeNavigation(content);
    if (!next) {
      skipped.push(filePath);
      continue;
    }
    if (options.dryRun) {
      console.log(`[dry-run] ${path.relative(ENTITIES_ROOT, filePath)}`);
      updated.push(filePath);
      continue;
    }
    writeGeneratedFile(filePath, next, { force: true });
    console.log(`✅ ${path.relative(ENTITIES_ROOT, filePath)}`);
    updated.push(filePath);
  }
  console.log(`\n📊 匹配子表 ${matched.length} 个，已更新 ${updated.length} 个，跳过 ${skipped.length} 个`);
}

const dryRun = process.argv.includes('--dry-run');
main({ dryRun });
