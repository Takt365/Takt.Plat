// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：scripts
// 文件名称：is-obsolete-entities.cjs
// 创建时间：2026-07-09
// 创建人：Takt365(Cursor AI)
// 功能描述：扫描含 IsObsolete 的实体及关联主表，供 IsObsolete 批量重生脚本复用
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const fs = require('fs');
const path = require('path');

const REPO_ROOT = path.resolve(__dirname, '../..');
const ENTITIES_ROOT = path.join(REPO_ROOT, 'backend/src/Takt.Domain/Entities');

const IS_OBSOLETE_PROP_RE = /\bpublic int IsObsolete\s*\{/;
const ONE_TO_MANY_CHILD_RE = /Navigate\(NavigateType\.OneToMany,\s*nameof\((Takt\w+)\./g;

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

/**
 * @returns {string[]} 含 IsObsolete 的实体短名（如 AssyDefectDetail）
 */
function collectIsObsoleteEntityShorts() {
  return collectEntityFiles(ENTITIES_ROOT)
    .filter((filePath) => IS_OBSOLETE_PROP_RE.test(fs.readFileSync(filePath, 'utf8')))
    .map((filePath) => path.basename(filePath, '.cs').replace(/^Takt/, ''))
    .sort((a, b) => a.localeCompare(b));
}

/**
 * @param {Set<string>} obsoleteChildClassNames 如 TaktAssyDefectDetail
 * @returns {string[]} 子表含 IsObsolete 的主表实体短名
 */
function collectMasterEntityShortsForObsoleteChildren(obsoleteChildClassNames) {
  const masters = new Set();
  for (const filePath of collectEntityFiles(ENTITIES_ROOT)) {
    const content = fs.readFileSync(filePath, 'utf8');
    let match;
    ONE_TO_MANY_CHILD_RE.lastIndex = 0;
    while ((match = ONE_TO_MANY_CHILD_RE.exec(content)) !== null) {
      if (obsoleteChildClassNames.has(match[1])) {
        masters.add(path.basename(filePath, '.cs').replace(/^Takt/, ''));
      }
    }
  }
  return [...masters].sort((a, b) => a.localeCompare(b));
}

/**
 * @returns {{ obsoleteShorts: string[], masterShorts: string[], allServiceShorts: string[] }}
 */
function collectIsObsoleteStackTargets() {
  const obsoleteShorts = collectIsObsoleteEntityShorts();
  const obsoleteChildClassNames = new Set(obsoleteShorts.map((s) => `Takt${s}`));
  const masterShorts = collectMasterEntityShortsForObsoleteChildren(obsoleteChildClassNames);
  const allServiceShorts = [...new Set([...obsoleteShorts, ...masterShorts])].sort((a, b) =>
    a.localeCompare(b),
  );
  return { obsoleteShorts, masterShorts, allServiceShorts };
}

module.exports = {
  REPO_ROOT,
  collectIsObsoleteEntityShorts,
  collectMasterEntityShortsForObsoleteChildren,
  collectIsObsoleteStackTargets,
};
