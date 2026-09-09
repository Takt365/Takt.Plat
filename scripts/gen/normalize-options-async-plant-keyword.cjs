// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：scripts/gen
// 文件名称：normalize-options-async-plant-keyword.cjs
// 功能描述：将全部 Get*OptionsAsync / Get*TreeOptionsAsync 统一为标准 plantCode+keyword 签名；同步接口、实现、控制器、前端 api
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

'use strict';

const fs = require('fs');
const path = require('path');

const ROOT = path.resolve(__dirname, '../..');
const APP_SERVICES = path.join(ROOT, 'backend/src/Takt.Application/Services');
const WEB_CONTROLLERS = path.join(ROOT, 'backend/src/Takt.WebApi/Controllers');
const FRONTEND_API = path.join(ROOT, 'frontend/src/api');

const args = process.argv.slice(2);
const dryRun = args.includes('--dry-run');

/**
 * 递归收集 .cs / .ts 文件
 * @param {string} dir
 * @param {RegExp} extRe
 * @returns {string[]}
 */
function walkFiles(dir, extRe) {
  if (!fs.existsSync(dir)) {
    return [];
  }
  const out = [];
  for (const name of fs.readdirSync(dir, { withFileTypes: true })) {
    const full = path.join(dir, name.name);
    if (name.isDirectory()) {
      out.push(...walkFiles(full, extRe));
    } else if (extRe.test(name.name)) {
      out.push(full);
    }
  }
  return out;
}

/**
 * 拆分 C# 参数列表（忽略泛型逗号）
 * @param {string} params
 * @returns {string[]}
 */
function splitCsharpParams(params) {
  const raw = String(params || '').trim();
  if (!raw) {
    return [];
  }
  const parts = [];
  let depth = 0;
  let buf = '';
  for (let i = 0; i < raw.length; i += 1) {
    const ch = raw[i];
    if (ch === '<') {
      depth += 1;
      buf += ch;
      continue;
    }
    if (ch === '>') {
      depth = Math.max(0, depth - 1);
      buf += ch;
      continue;
    }
    if (ch === ',' && depth === 0) {
      parts.push(buf.trim());
      buf = '';
      continue;
    }
    buf += ch;
  }
  if (buf.trim()) {
    parts.push(buf.trim());
  }
  return parts;
}

/**
 * 参数名
 * @param {string} param
 * @returns {string}
 */
function paramName(param) {
  const m = String(param).match(/([A-Za-z_][\w]*)\s*(?:=\s*[^,]+)?$/);
  return m ? m[1] : '';
}

/**
 * 将无默认值的参数改为可空可选（便于标准 plantCode/keyword 置前）
 * @param {string} param
 * @returns {string}
 */
function makeNullableOptionalParam(param) {
  const p = String(param).trim();
  if (/=/.test(p)) {
    return p;
  }
  const m = p.match(/^(.*?)\s+([A-Za-z_][\w]*)$/);
  if (!m) {
    return `${p} = null`;
  }
  let type = m[1].trim();
  const name = m[2];
  if (type === 'string') {
    type = 'string?';
  } else if (/^(bool|int|long|decimal|double|float|byte|short|DateTime|Guid)$/.test(type)) {
    type = `${type}?`;
  } else if (!type.endsWith('?')) {
    type = `${type}?`;
  }
  return `${type} ${name} = null`;
}

/**
 * 将参数列表规范为：可选 parentId 在前，然后 plantCode、keyword，再其余（一律可空可选）
 * @param {string} paramsInner
 * @param {{ isTree?: boolean }} [opts]
 * @returns {string}
 */
function normalizeOptionsParams(paramsInner, opts = {}) {
  const parts = splitCsharpParams(paramsInner);
  const byName = new Map();
  for (const p of parts) {
    const n = paramName(p);
    if (n) {
      byName.set(n, p);
    }
  }

  const ordered = [];
  if (opts.isTree || byName.has('parentId')) {
    ordered.push(byName.get('parentId') || 'long parentId = 0');
    byName.delete('parentId');
  }

  // 标准：plantCode / keyword 始终存在且可空（置前，其余参数全部改为可选）
  ordered.push('string? plantCode = null');
  ordered.push('string? keyword = null');
  byName.delete('plantCode');
  byName.delete('keyword');

  for (const [, p] of byName) {
    ordered.push(makeNullableOptionalParam(p));
  }
  return ordered.join(', ');
}

/**
 * 规范化 C# Options 方法签名（接口或实现）
 * @param {string} content
 * @returns {{ content: string, count: number }}
 */
function normalizeCsharpOptionsSignatures(content) {
  let count = 0;
  let next = content;

  // 接口：Task<...> GetXxxOptionsAsync(...);
  next = next.replace(
    /(Task<(?:List<)?Takt(?:Tree)?SelectOption(?:>)?>\s+Get\w+OptionsAsync)\s*\(([^)]*)\)\s*;/g,
    (full, head, params) => {
      const isTree = /TreeOptionsAsync/.test(head);
      const normalized = normalizeOptionsParams(params, { isTree });
      if (normalized === String(params).trim()) {
        return full;
      }
      count += 1;
      return `${head}(${normalized});`;
    },
  );

  // 实现：public async Task / public Task（表达式体转发也覆盖）
  next = next.replace(
    /(public\s+(?:async\s+)?Task<(?:List<)?Takt(?:Tree)?SelectOption(?:>)?>\s+Get\w+OptionsAsync)\s*\(([^)]*)\)/g,
    (full, head, params) => {
      const isTree = /TreeOptionsAsync/.test(head);
      const normalized = normalizeOptionsParams(params, { isTree });
      if (normalized === String(params).trim()) {
        return full;
      }
      count += 1;
      return `${head}(${normalized})`;
    },
  );

  return { content: next, count };
}

/**
 * 控制器 Options Action：补 FromQuery plantCode/keyword 并传给服务
 * @param {string} content
 * @returns {{ content: string, count: number }}
 */
function normalizeControllerOptions(content) {
  let count = 0;
  let next = content;

  // public async Task<IActionResult> GetXxxOptionsAsync(...)
  next = next.replace(
    /(public\s+async\s+Task<IActionResult>\s+(Get\w+OptionsAsync))\s*\(([^)]*)\)\s*\{([\s\S]*?)await\s+(_\w+)\.(\2)\s*\(([^)]*)\)/g,
    (full, head, methodName, params, bodyMid, serviceField, _m2, callArgs) => {
      const isTree = /TreeOptionsAsync/.test(methodName);
      const normalized = normalizeOptionsParams(params, { isTree });
      const paramParts = splitCsharpParams(normalized).map((p) => {
        if (/\[FromQuery\]/.test(p) || /\[FromBody\]/.test(p) || /\[FromRoute\]/.test(p)) {
          return p;
        }
        // 简单类型加 FromQuery
        return `[FromQuery] ${p}`;
      });
      const paramDecl = paramParts.join(', ');
      const names = splitCsharpParams(normalized).map(paramName).filter(Boolean);
      const newCall = names.join(', ');
      if (paramDecl === String(params).trim() && newCall === String(callArgs).trim()) {
        return full;
      }
      count += 1;
      return `${head}(${paramDecl})\n    {${bodyMid}await ${serviceField}.${methodName}(${newCall})`;
    },
  );

  return { content: next, count };
}

/**
 * 实现方法体内补 normalizedPlantCode / normalizedKeyword（若缺失）
 * @param {string} content
 * @returns {{ content: string, count: number }}
 */
function injectNormalizeLocals(content) {
  let count = 0;
  const next = content.replace(
    /(public\s+async\s+Task<(?:List<)?Takt(?:Tree)?SelectOption(?:>)?>\s+Get\w+OptionsAsync\s*\([^)]*\)\s*\{)(\s*)/g,
    (full, head, ws) => {
      // 找方法体前 400 字符是否已有
      const start = content.indexOf(full);
      if (start < 0) {
        return full;
      }
      const snippet = content.slice(start, start + 600);
      if (/normalizedPlantCode/.test(snippet) && /normalizedKeyword/.test(snippet)) {
        return full;
      }
      count += 1;
      const indent = '        ';
      return `${head}\n${indent}var normalizedPlantCode = plantCode?.Trim();\n${indent}var normalizedKeyword = keyword?.Trim();\n${ws}`;
    },
  );
  return { content: next, count };
}

/**
 * 前端 getXxxOptions：补 plantCode/keyword 查询参数
 * @param {string} content
 * @returns {{ content: string, count: number }}
 */
function normalizeFrontendOptionsApi(content) {
  let count = 0;
  let next = content;

  next = next.replace(
    /export function (get\w+Options)\(([^)]*)\)\s*:\s*Promise<TaktSelectOption\[\]>\s*\{[\s\S]*?url:\s*`\$\{[^}]+\}(\/(?:tree-)?options)`[\s\S]*?method:\s*'get',[\s\S]*?\}\);?\s*\}/g,
    (full, fnName, params, route) => {
      const isTree = /tree-options/.test(route) || /TreeOptions/.test(fnName);
      const hasPlant = /\bplantCode\b/.test(params);
      const hasKeyword = /\bkeyword\b/.test(params);
      if (hasPlant && hasKeyword && /params\s*:/.test(full)) {
        return full;
      }
      count += 1;
      const treePrefix = isTree
        ? 'parentId: string | number = 0, '
        : '';
      const apiBaseMatch = full.match(/url:\s*`\$\{([^}]+)\}/);
      const apiBase = apiBaseMatch ? apiBaseMatch[1] : 'API_BASE';
      const pathSuffix = route;
      return `export function ${fnName}(
  ${treePrefix}plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: \`\${${apiBase}}${pathSuffix}\`,
    method: 'get',
    params: {
      ${isTree ? '...(parentId !== undefined && parentId !== null ? { parentId } : {}),' : ''}
      ...(plant ? { plantCode: plant } : {}),
      ...(kw ? { keyword: kw } : {}),
    },
  });
}`;
    },
  );

  // 无参简写：export function getXxxOptions(): Promise... { return request({ url: .../options, method: 'get' }); }
  next = next.replace(
    /export function (get\w+Options)\(\)\s*:\s*Promise<TaktSelectOption\[\]>\s*\{\s*return request(?:<TaktSelectOption\[\]>)?\(\{\s*url:\s*`\$\{([^}]+)\}(\/(?:tree-)?options)`\s*,\s*method:\s*'get'\s*,?\s*\}\);\s*\}/g,
    (full, fnName, apiBase, route) => {
      count += 1;
      const isTree = /tree-options/.test(route);
      return `export function ${fnName}(
  ${isTree ? 'parentId: string | number = 0, ' : ''}plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: \`\${${apiBase}}${route}\`,
    method: 'get',
    params: {
      ${isTree ? '...(parentId !== undefined && parentId !== null ? { parentId } : {}),' : ''}
      ...(plant ? { plantCode: plant } : {}),
      ...(kw ? { keyword: kw } : {}),
    },
  });
}`;
    },
  );

  return { content: next, count };
}

/**
 * 处理单文件
 * @param {string} filePath
 * @param {'iface'|'impl'|'controller'|'frontend'} kind
 * @returns {number}
 */
function processFile(filePath, kind) {
  const original = fs.readFileSync(filePath, 'utf8');
  let content = original;
  let changed = 0;

  if (kind === 'iface' || kind === 'impl') {
    const r = normalizeCsharpOptionsSignatures(content);
    content = r.content;
    changed += r.count;
  }
  // 实现体过滤逻辑由 generate-services 模板在下次 --refreshOptions / 失效重生成时写入；
  // 本脚本只统一签名，避免无 PlantCode 实体注入未使用局部变量。
  if (kind === 'controller') {
    const r1 = normalizeCsharpOptionsSignatures(content);
    content = r1.content;
    changed += r1.count;
    const r2 = normalizeControllerOptions(content);
    content = r2.content;
    changed += r2.count;
  }
  if (kind === 'frontend') {
    const r = normalizeFrontendOptionsApi(content);
    content = r.content;
    changed += r.count;
  }

  if (content !== original) {
    if (!dryRun) {
      fs.writeFileSync(filePath, content, 'utf8');
    }
    return changed || 1;
  }
  return 0;
}

function main() {
  const stats = { iface: 0, impl: 0, controller: 0, frontend: 0, files: 0 };

  for (const file of walkFiles(APP_SERVICES, /^ITakt\w+Service\.cs$/)) {
    const n = processFile(file, 'iface');
    if (n) {
      stats.iface += n;
      stats.files += 1;
      console.log(`[iface] ${path.relative(ROOT, file)} (+${n})`);
    }
  }

  for (const file of walkFiles(APP_SERVICES, /^Takt\w+Service\.cs$/)) {
    if (/ITakt\w+Service\.cs$/.test(path.basename(file))) {
      continue;
    }
    const n = processFile(file, 'impl');
    if (n) {
      stats.impl += n;
      stats.files += 1;
      console.log(`[impl] ${path.relative(ROOT, file)} (+${n})`);
    }
  }

  for (const file of walkFiles(WEB_CONTROLLERS, /^Takt\w+Controller\.cs$/)) {
    const n = processFile(file, 'controller');
    if (n) {
      stats.controller += n;
      stats.files += 1;
      console.log(`[controller] ${path.relative(ROOT, file)} (+${n})`);
    }
  }

  for (const file of walkFiles(FRONTEND_API, /\.ts$/)) {
    const n = processFile(file, 'frontend');
    if (n) {
      stats.frontend += n;
      stats.files += 1;
      console.log(`[frontend] ${path.relative(ROOT, file)} (+${n})`);
    }
  }

  console.log(
    `\nDone${dryRun ? ' (dry-run)' : ''}: files=${stats.files} iface=${stats.iface} impl=${stats.impl} controller=${stats.controller} frontend=${stats.frontend}`,
  );
}

main();
