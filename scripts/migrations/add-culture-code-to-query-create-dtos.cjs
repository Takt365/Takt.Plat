// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：scripts
// 文件名称：add-culture-code-to-query-create-dtos.cjs
// 创建时间：2026-08-11
// 创建人：Takt365(Cursor AI)
// 功能描述：为已有 *QueryDto / *CreateDto 在 ExtField 前补齐 CultureCode（对齐 generate-dtos-from-entity）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const fs = require('fs');
const path = require('path');

const DTOS_ROOT = path.resolve(__dirname, '../../backend/src/Takt.Application/Dtos');

/** 与 generate-dtos-from-entity.appendExtFieldAndRemark / QueryDto 尾部一致（末尾保留空行分隔下一属性） */
const CULTURE_CODE_BLOCK = [
  '    /// <summary>',
  '    /// 区域文化编码（字典 sys_culture_code；租户→公司→工厂固定映射）',
  '    /// </summary>',
  '    public string CultureCode { get; set; } = string.Empty;',
  '',
  '',
].join('\n');

/**
 * 递归收集 *Dtos.cs
 * @param {string} dir
 * @param {string[]} acc
 * @returns {string[]}
 */
function collectDtoFiles(dir, acc = []) {
  for (const entry of fs.readdirSync(dir, { withFileTypes: true })) {
    const full = path.join(dir, entry.name);
    if (entry.isDirectory()) {
      collectDtoFiles(full, acc);
    } else if (entry.name.endsWith('Dtos.cs')) {
      acc.push(full);
    }
  }
  return acc;
}

/**
 * 定位 public class 声明起止（含类体）
 * @param {string} text
 * @returns {{ name: string, start: number, bodyStart: number, end: number }[]}
 */
function findPublicClasses(text) {
  const results = [];
  const re = /public class (\w+)\b[^{]*\{/g;
  let match;
  while ((match = re.exec(text)) !== null) {
    results.push({
      name: match[1],
      start: match.index,
      bodyStart: match.index + match[0].length,
    });
  }
  for (let i = 0; i < results.length; i++) {
    const nextStart = i + 1 < results.length ? results[i + 1].start : text.length;
    results[i].end = nextStart;
  }
  return results;
}

/**
 * 在 QueryDto / CreateDto 类体中、ExtField 属性前插入 CultureCode（若尚无）
 * @param {string} classBody
 * @returns {{ body: string, changed: boolean }}
 */
function patchClassBody(classBody) {
  if (/\bCultureCode\b/.test(classBody)) {
    return { body: classBody, changed: false };
  }
  // 匹配 ExtField 前的 /// summary 三行 + 属性声明
  const extFieldRe =
    /(\n)(    \/\/\/ <summary>\r?\n    \/\/\/[^\n]*\r?\n    \/\/\/ <\/summary>\r?\n)(    public string\? ExtField \{ get; set; \})/;
  if (!extFieldRe.test(classBody)) {
    return { body: classBody, changed: false };
  }
  const body = classBody.replace(extFieldRe, `$1${CULTURE_CODE_BLOCK}$2$3`);
  return { body, changed: body !== classBody };
}

/**
 * @returns {{ patchedFiles: string[], patchedClasses: string[], skippedNoExtField: string[] }}
 */
function run(dryRun) {
  const files = collectDtoFiles(DTOS_ROOT);
  const patchedFiles = [];
  const patchedClasses = [];
  const skippedNoExtField = [];
  for (const file of files) {
    const original = fs.readFileSync(file, 'utf8');
    const classes = findPublicClasses(original);
    const parts = [];
    let cursor = 0;
    let fileChanged = false;
    for (const cls of classes) {
      parts.push(original.slice(cursor, cls.bodyStart));
      if (!/(QueryDto|CreateDto)$/.test(cls.name)) {
        parts.push(original.slice(cls.bodyStart, cls.end));
        cursor = cls.end;
        continue;
      }
      const classBody = original.slice(cls.bodyStart, cls.end);
      if (/\bCultureCode\b/.test(classBody)) {
        parts.push(classBody);
        cursor = cls.end;
        continue;
      }
      if (!/\bpublic string\? ExtField \{ get; set; \}/.test(classBody)) {
        skippedNoExtField.push(`${path.relative(DTOS_ROOT, file)}::${cls.name}`);
        parts.push(classBody);
        cursor = cls.end;
        continue;
      }
      const { body, changed } = patchClassBody(classBody);
      if (!changed) {
        skippedNoExtField.push(`${path.relative(DTOS_ROOT, file)}::${cls.name}`);
        parts.push(classBody);
        cursor = cls.end;
        continue;
      }
      parts.push(body);
      fileChanged = true;
      patchedClasses.push(`${path.relative(DTOS_ROOT, file)}::${cls.name}`);
      cursor = cls.end;
    }
    parts.push(original.slice(cursor));
    if (fileChanged) {
      patchedFiles.push(path.relative(DTOS_ROOT, file));
      if (!dryRun) {
        fs.writeFileSync(file, parts.join(''), 'utf8');
      }
    }
  }
  return { patchedFiles, patchedClasses, skippedNoExtField };
}

function main() {
  const dryRun = process.argv.includes('--dry-run');
  const { patchedFiles, patchedClasses, skippedNoExtField } = run(dryRun);
  console.log(dryRun ? '🔍 dry-run' : '✍️ 写入');
  console.log(`补丁类 ${patchedClasses.length}:`);
  patchedClasses.forEach((c) => console.log(`  + ${c}`));
  console.log(`涉及文件 ${patchedFiles.length}`);
  console.log(`跳过（无 ExtField 或未匹配） ${skippedNoExtField.length}`);
  skippedNoExtField.forEach((c) => console.log(`  · ${c}`));
}

main();
