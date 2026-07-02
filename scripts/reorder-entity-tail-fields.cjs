// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：scripts
// 文件名称：reorder-entity-tail-fields.cjs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：实体标量属性末段四种固定顺序（导航属性之前）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const fs = require('fs');
const path = require('path');
const { findEntityStatusProperty } = require('./generate-enum-common.cjs');

const ENTITIES_ROOT = path.resolve(__dirname, '../backend/src/Takt.Domain/Entities');
const NAVIGATION_REGION_MARKER = '导航属性区域';

/**
 * 遍历实体目录
 * @param {string} dir
 * @param {string[]} acc
 * @returns {string[]}
 */
function walkEntityFiles(dir, acc = []) {
  for (const entry of fs.readdirSync(dir, { withFileTypes: true })) {
    const full = path.join(dir, entry.name);
    if (entry.isDirectory()) {
      walkEntityFiles(full, acc);
    } else if (entry.name.endsWith('.cs') && entry.name.startsWith('Takt')) {
      acc.push(full);
    }
  }
  return acc;
}

/**
 * 提取类体
 * @param {string} content
 */
function splitEntityClass(content) {
  const classMatch = content.match(/public\s+class\s+(Takt\w+)\s*:\s*(\w+)\s*\{/);
  if (!classMatch) {
    return null;
  }
  const openIdx = classMatch.index + classMatch[0].length - 1;
  let depth = 0;
  let closeIdx = -1;
  for (let i = openIdx; i < content.length; i += 1) {
    if (content[i] === '{') {
      depth += 1;
    } else if (content[i] === '}') {
      depth -= 1;
      if (depth === 0) {
        closeIdx = i;
        break;
      }
    }
  }
  if (closeIdx < 0) {
    return null;
  }
  return {
    before: content.slice(0, openIdx + 1),
    body: content.slice(openIdx + 1, closeIdx),
    after: content.slice(closeIdx),
    classMatch,
  };
}

/**
 * 拆分标量区与导航区
 * @param {string} classBody
 */
function splitScalarAndNavigation(classBody) {
  const lines = classBody.split('\n');
  let markerLineIdx = -1;
  for (let i = 0; i < lines.length; i += 1) {
    if (lines[i].includes(NAVIGATION_REGION_MARKER)) {
      markerLineIdx = i;
      break;
    }
  }
  if (markerLineIdx === -1) {
    const navIdx = lines.findIndex((l) => /\[Navigate\s*\(/.test(l));
    if (navIdx === -1) {
      return { scalarBody: classBody, navigationBody: '' };
    }
    let navStart = navIdx;
    while (navStart > 0 && (/^\s*\/\/\//.test(lines[navStart - 1]) || /^\s*\[/.test(lines[navStart - 1]))) {
      navStart -= 1;
    }
    return {
      scalarBody: lines.slice(0, navStart).join('\n'),
      navigationBody: lines.slice(navStart).join('\n'),
    };
  }
  let navStartLine = markerLineIdx;
  while (navStartLine > 0 && /^\s*\/\/\s*=+/.test(lines[navStartLine - 1])) {
    navStartLine -= 1;
  }
  return {
    scalarBody: lines.slice(0, navStartLine).join('\n'),
    navigationBody: lines.slice(navStartLine).join('\n'),
  };
}

/**
 * 解析属性块
 * @param {string} scalarBody
 */
function parsePropertyBlocks(scalarBody) {
  const lines = scalarBody.split('\n');
  const blocks = [];
  let i = 0;
  while (i < lines.length) {
    const line = lines[i];
    if (!line.trim()) {
      i += 1;
      continue;
    }
    const isPropStart = /^\s*\/\/\/\s*<summary>/.test(line) || /^\s*\[SugarColumn/.test(line);
    if (!isPropStart) {
      i += 1;
      continue;
    }
    const start = i;
    while (i < lines.length && !/public\s+.+\{\s*get;\s*set;/.test(lines[i])) {
      i += 1;
    }
    if (i >= lines.length) {
      break;
    }
    const end = i + 1;
    const text = lines.slice(start, end).join('\n');
    const nameMatch = lines[i].match(/public\s+(?:[\w<>,.?]+)\s+(\w+)\s*\{/);
    const name = nameMatch ? nameMatch[1] : '';
    blocks.push({ name, text });
    i = end;
  }
  return blocks;
}

/**
 * 从 blocks 中抽出指定属性
 * @param {Array<{ name: string, text: string }>} blocks
 * @param {string} name
 */
function pullBlock(blocks, name) {
  const idx = blocks.findIndex((b) => b.name === name);
  if (idx < 0) {
    return null;
  }
  const [block] = blocks.splice(idx, 1);
  return block;
}

/**
 * 四种末段模式 → 末段字段名（严格顺序）
 * @param {boolean} hasRelatedPlant
 * @param {boolean} hasSortOrder
 * @param {string|null} statusField
 * @returns {string[]|null}
 */
function resolveTailFieldOrder(hasRelatedPlant, hasSortOrder, statusField) {
  const hasStatus = !!statusField;
  if (hasRelatedPlant && hasSortOrder && hasStatus) {
    return ['RelatedPlant', 'SortOrder', statusField];
  }
  if (hasSortOrder && hasStatus && !hasRelatedPlant) {
    return ['SortOrder', statusField];
  }
  if (hasRelatedPlant && hasStatus && !hasSortOrder) {
    return ['RelatedPlant', statusField];
  }
  if (hasStatus && !hasRelatedPlant && !hasSortOrder) {
    return [statusField];
  }
  return null;
}

/**
 * 重排标量属性末段（仅 RelatedPlant / SortOrder / *Status）
 * @param {string} scalarBody
 */
function reorderScalarBody(scalarBody) {
  const blocks = parsePropertyBlocks(scalarBody);
  if (!blocks.length) {
    return { changed: false, body: scalarBody };
  }
  const oldNames = blocks.map((b) => b.name).join('|');
  const propMeta = blocks.map((b) => ({
    name: b.name,
    bareType: (b.text.match(/public\s+((?:[\w<>,.?]+))\s+\w+\s*\{/) || [])[1] || '',
  }));
  const statusProp = findEntityStatusProperty(
    propMeta.map((p) => ({ name: p.name, bareType: p.bareType.replace('?', '') })),
  );
  const statusField = statusProp?.name || null;
  const hasRelatedPlant = blocks.some((b) => b.name === 'RelatedPlant');
  const hasSortOrder = blocks.some((b) => b.name === 'SortOrder');
  const tailOrder = resolveTailFieldOrder(hasRelatedPlant, hasSortOrder, statusField);
  if (!tailOrder) {
    return { changed: false, body: scalarBody };
  }
  const tailBlocks = [];
  tailOrder.forEach((name) => {
    const b = pullBlock(blocks, name);
    if (b) {
      tailBlocks.push(b);
    }
  });
  const newBlocks = [...blocks, ...tailBlocks];
  const newNames = newBlocks.map((b) => b.name).join('|');
  if (oldNames === newNames) {
    return { changed: false, body: scalarBody };
  }
  const prefixNewline = scalarBody.startsWith('\n') ? '\n' : '';
  return { changed: true, body: `${prefixNewline}${newBlocks.map((b) => b.text).join('\n')}\n` };
}

/**
 * 处理单个实体文件
 * @param {string} filePath
 */
function processEntityFile(filePath) {
  const content = fs.readFileSync(filePath, 'utf-8');
  const parts = splitEntityClass(content);
  if (!parts) {
    return { filePath, changed: false, reason: 'no-class' };
  }
  const { scalarBody, navigationBody } = splitScalarAndNavigation(parts.body);
  const { changed, body: newScalar } = reorderScalarBody(scalarBody);
  if (!changed) {
    return { filePath, changed: false, reason: 'already-ok' };
  }
  const navPart = navigationBody ? `\n${navigationBody}` : '';
  const newContent = `${parts.before}${newScalar}${navPart}${parts.after}`;
  fs.writeFileSync(filePath, newContent, 'utf-8');
  return { filePath, changed: true };
}

function main() {
  const files = walkEntityFiles(ENTITIES_ROOT);
  const changed = [];
  const skipped = [];
  for (const file of files) {
    const result = processEntityFile(file);
    if (result.changed) {
      changed.push(path.relative(ENTITIES_ROOT, result.filePath));
    } else if (result.reason !== 'already-ok') {
      skipped.push(result);
    }
  }
  console.log(`Reordered ${changed.length} entity file(s).`);
  changed.forEach((f) => console.log(`  ${f}`));
  const manifestPath = path.join(__dirname, '.tmp', 'reorder-entity-tail-fields-last.txt');
  fs.mkdirSync(path.dirname(manifestPath), { recursive: true });
  fs.writeFileSync(manifestPath, changed.join('\n') + (changed.length ? '\n' : ''), 'utf-8');
  if (skipped.length) {
    console.log(`Skipped ${skipped.length} file(s) (no class).`);
  }
}

main();
