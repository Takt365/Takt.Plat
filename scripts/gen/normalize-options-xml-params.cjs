#!/usr/bin/env node
/**
 * 补全 *OptionsAsync / *TreeOptionsAsync 方法 XML：签名有参则须有对应 <param>。
 * 修复批量加 plantCode/keyword 后仅更新签名、未更新注释导致的 CS1573。
 *
 * 用法：node scripts/gen/normalize-options-xml-params.cjs
 */
'use strict';

const fs = require('fs');
const path = require('path');

const ROOT = path.resolve(__dirname, '../..');
const SERVICES_ROOT = path.join(ROOT, 'backend/src/Takt.Application/Services');

const PARAM_DESC = {
  parentId: '父级ID（0=根）',
  plantCode: '工厂代码（可选，用于按工厂过滤）',
  keyword: '搜索关键字（可选，模糊匹配）',
  includeDisabled: '是否包含禁用项',
  valueBy: 'DictValue 取值字段（可选）',
  teamCategory: '班组类别（可选）',
  defectCategory: '不良类别（可选）',
  queryDto: '查询DTO',
  documentType: '单据类型（可选）',
};

/**
 * @param {string} dir
 * @param {string[]} out
 */
function walkCs(dir, out = []) {
  if (!fs.existsSync(dir)) {
    return out;
  }
  for (const entry of fs.readdirSync(dir, { withFileTypes: true })) {
    const full = path.join(dir, entry.name);
    if (entry.isDirectory()) {
      walkCs(full, out);
    } else if (entry.name.endsWith('.cs')) {
      out.push(full);
    }
  }
  return out;
}

/**
 * @param {string} signature
 * @returns {string[]}
 */
function parseParamNames(signature) {
  const open = signature.indexOf('(');
  const close = signature.lastIndexOf(')');
  if (open < 0 || close <= open) {
    return [];
  }
  const inner = signature.slice(open + 1, close).trim();
  if (!inner) {
    return [];
  }
  const parts = [];
  let depth = 0;
  let buf = '';
  for (const ch of inner) {
    if (ch === '<' || ch === '(') {
      depth += 1;
      buf += ch;
    } else if (ch === '>' || ch === ')') {
      depth -= 1;
      buf += ch;
    } else if (ch === ',' && depth === 0) {
      parts.push(buf.trim());
      buf = '';
    } else {
      buf += ch;
    }
  }
  if (buf.trim()) {
    parts.push(buf.trim());
  }
  return parts
    .map((p) => {
      const noDefault = p.split('=')[0].trim();
      const tokens = noDefault.split(/\s+/);
      return tokens[tokens.length - 1].replace(/^[*&]/, '');
    })
    .filter(Boolean);
}

/**
 * @param {string} xmlBlock
 * @param {string} name
 */
function hasParamTag(xmlBlock, name) {
  return new RegExp(`///\\s*<param\\s+name="${name}"\\s*>`).test(xmlBlock);
}

/**
 * @param {string} name
 */
function describeParam(name) {
  if (PARAM_DESC[name]) {
    return PARAM_DESC[name];
  }
  return `${name}（可选）`;
}

/**
 * 自方法行向前收集连续 /// XML 注释块起止行号（含）
 * @param {string[]} lines
 * @param {number} methodLineIdx
 * @returns {{ start: number, end: number } | null}
 */
function findXmlBlockRange(lines, methodLineIdx) {
  let end = methodLineIdx - 1;
  while (end >= 0 && lines[end].trim() === '') {
    end -= 1;
  }
  if (end < 0 || !lines[end].trimStart().startsWith('///')) {
    return null;
  }
  let start = end;
  while (start > 0 && lines[start - 1].trimStart().startsWith('///')) {
    start -= 1;
  }
  return { start, end };
}

/**
 * @param {string} content
 * @returns {{ content: string, changed: number }}
 */
function patchFile(content) {
  const lines = content.split(/\r?\n/);
  let changed = 0;
  const methodLineRe =
    /^(\s*)(?:(?:public\s+async\s+|public\s+))?Task\s*<.+>\s+(Get\w+(?:Tree)?OptionsAsync)\s*\(/;

  for (let i = 0; i < lines.length; i += 1) {
    const m = lines[i].match(methodLineRe);
    if (!m) {
      continue;
    }
    let sig = lines[i];
    let j = i;
    while (!sig.includes(')') && j + 1 < lines.length) {
      j += 1;
      sig += ` ${lines[j].trim()}`;
    }
    const names = parseParamNames(sig);
    if (!names.length) {
      continue;
    }
    const range = findXmlBlockRange(lines, i);
    if (!range) {
      continue;
    }
    const xmlText = lines.slice(range.start, range.end + 1).join('\n');
    const missing = names.filter((n) => !hasParamTag(xmlText, n));
    if (!missing.length) {
      continue;
    }
    const indent = m[1] || '    ';
    const insertLines = missing.map((n) => `${indent}/// <param name="${n}">${describeParam(n)}</param>`);
    let insertAt = -1;
    for (let k = range.start; k <= range.end; k += 1) {
      if (/\/\/\/\s*<returns>/.test(lines[k])) {
        insertAt = k;
        break;
      }
    }
    if (insertAt < 0) {
      insertAt = range.end + 1;
    }
    lines.splice(insertAt, 0, ...insertLines);
    changed += 1;
    i += insertLines.length;
  }

  return { content: lines.join('\n'), changed };
}

function main() {
  const files = walkCs(SERVICES_ROOT);
  let fileCount = 0;
  let methodCount = 0;
  for (const file of files) {
    const raw = fs.readFileSync(file, 'utf8');
    if (!/Get\w+(?:Tree)?OptionsAsync\s*\(/.test(raw)) {
      continue;
    }
    const { content, changed } = patchFile(raw);
    if (changed > 0) {
      fs.writeFileSync(file, content, 'utf8');
      fileCount += 1;
      methodCount += changed;
      console.log(`patched ${path.relative(ROOT, file)} (+${changed})`);
    }
  }
  console.log(`Done: ${fileCount} files, ${methodCount} Options methods XML updated.`);
}

main();
