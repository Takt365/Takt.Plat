'use strict';

/**
 * 统一实体 XML summary 字典/选项注释格式，并去除外部系统字段码说明。
 *
 * 标准（见 .cursor/rules/01-backend.mdc）：
 *   （字典 {code}）
 *   （字典 {code}；0=a 1=b …） / （字典 {code}；DictValue=…）
 *   （选项 TaktXxxs/options；DictValue=…）
 * 字典/选项编码后一律用「；」；禁止在 summary 写外部系统字段码。
 *
 * 用法: node scripts/gen/normalize-entity-dict-xml-comments.cjs
 */

const fs = require('fs');
const path = require('path');

const entitiesRoot = path.join(
  __dirname,
  '../../backend/src/Takt.Domain/Entities',
);

/**
 * 清洗单行 summary 文案
 * @param {string} text
 * @returns {string}
 */
function normalizeSummaryText(text) {
  let s = text.trim();

  // 去掉外部系统字段码片段（保留其后的中文业务说明）
  // 例：；外系统字段码 → 空；字段码后中文说明予以保留
  s = s.replace(/[；，、/]?\s*SAP\s+[A-Za-z0-9_./]+(?:\s*\/\s*[A-Za-z0-9_./]+)*/g, '');

  // 字典/选项后：逗号统一为分号
  s = s.replace(/（字典\s+([a-z0-9_]+)，/g, '（字典 $1；');
  s = s.replace(/（选项\s+([^；，）]+)，\s*DictValue=/g, '（选项 $1；DictValue=');
  s = s.replace(/（字典\s+([a-z0-9_]+)，(\d+=)/g, '（字典 $1；$2');

  // 清理残留
  s = s.replace(/；{2,}/g, '；');
  s = s.replace(/，{2,}/g, '，');
  s = s.replace(/（，+/g, '（');
  s = s.replace(/；）/g, '）');
  s = s.replace(/，）/g, '）');
  s = s.replace(/（\s*）/g, '');
  s = s.replace(/；\s*$/g, '');
  s = s.replace(/，\s*$/g, '');
  s = s.replace(/\s{2,}/g, ' ').trim();

  return s;
}

/**
 * @param {string} filePath
 * @returns {{ changed: boolean, count: number }}
 */
function processFile(filePath) {
  const raw = fs.readFileSync(filePath, 'utf8');
  let count = 0;
  const next = raw.replace(
    /^([ \t]*)\/\/\/ <summary>\r?\n\1\/\/\/ (.*?)\r?\n\1\/\/\/ <\/summary>/gm,
    (full, indent, body) => {
      const normalized = normalizeSummaryText(body);
      if (normalized === body.trim()) {
        return full;
      }
      count += 1;
      return `${indent}/// <summary>\n${indent}/// ${normalized}\n${indent}/// </summary>`;
    },
  );

  if (count > 0 && next !== raw) {
    fs.writeFileSync(filePath, next);
    return { changed: true, count };
  }
  return { changed: false, count: 0 };
}

/**
 * @param {string} dir
 * @returns {string[]}
 */
function walkCs(dir) {
  const out = [];
  for (const name of fs.readdirSync(dir, { withFileTypes: true })) {
    const p = path.join(dir, name.name);
    if (name.isDirectory()) {
      out.push(...walkCs(p));
    } else if (name.isFile() && name.name.endsWith('.cs')) {
      out.push(p);
    }
  }
  return out;
}

const files = walkCs(entitiesRoot);
let fileCount = 0;
let commentCount = 0;
for (const f of files) {
  const r = processFile(f);
  if (r.changed) {
    fileCount += 1;
    commentCount += r.count;
    console.log(`ok ${path.relative(entitiesRoot, f)} (${r.count})`);
  }
}
console.log(`\nDone: ${fileCount} files, ${commentCount} summaries`);
