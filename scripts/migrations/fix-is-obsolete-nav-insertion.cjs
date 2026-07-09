// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：scripts
// 文件名称：fix-is-obsolete-nav-insertion.cjs
// 创建时间：2026-07-09
// 创建人：Takt365(Cursor AI)
// 功能描述：修复 IsObsolete 误插入导航属性 XML 注释中间的问题
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const fs = require('fs');
const path = require('path');
const { writeGeneratedFile } = require('../gen/generate-script-common.cjs');

const ENTITIES_ROOT = path.join(path.resolve(__dirname, '../../backend/src'), 'Takt.Domain', 'Entities');

const BROKEN_PATTERN = /    \/\/\/ <summary>\r?\n    \/\/\/ ([^\r\n]+)\r?\n\r?\n    \/\/\/ <summary>\r?\n    \/\/\/ 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）\r?\n    \/\/\/ <\/summary>\r?\n    \[SugarColumn\(ColumnName = "is_obsolete", ColumnDescription = "是否作废", ColumnDataType = "int", IsNullable = false, DefaultValue = "0"\)\]\r?\n    public int IsObsolete \{ get; set; \} = 0;\r?\n\r?\n\r?\n    \/\/\/ <\/summary>\r?\n    (\[Navigate)/g;

const IS_OBSOLETE_BLOCK = `    /// <summary>
    /// 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    [SugarColumn(ColumnName = "is_obsolete", ColumnDescription = "是否作废", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// $1
    /// </summary>
    $2`;

/**
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

let fixed = 0;
for (const filePath of collectEntityFiles(ENTITIES_ROOT)) {
  const content = fs.readFileSync(filePath, 'utf-8');
  if (!BROKEN_PATTERN.test(content)) {
    continue;
  }
  const next = content.replace(BROKEN_PATTERN, IS_OBSOLETE_BLOCK);
  writeGeneratedFile(filePath, next, { force: true });
  console.log(`🔧 ${path.relative(ENTITIES_ROOT, filePath)}`);
  fixed += 1;
}
console.log(`\n📊 已修复 ${fixed} 个文件`);
