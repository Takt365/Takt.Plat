// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/scripts
// 文件名称：strip-component-imports-in-vue.mjs
// 功能描述：移除 .vue 中已由 unplugin-vue-components 全局注册的重复组件 import
// ========================================

import fs from 'node:fs';
import path from 'node:path';
import { fileURLToPath } from 'node:url';

const FRONTEND_ROOT = path.resolve(path.dirname(fileURLToPath(import.meta.url)), '..');
const SRC_ROOT = path.join(FRONTEND_ROOT, 'src');
const COMPONENTS_DTS = path.join(SRC_ROOT, 'components.d.ts');

/**
 * 从 components.d.ts 解析 GlobalComponents 名称
 * @returns {Set<string>}
 */
function loadGlobalComponentNames() {
  const content = fs.readFileSync(COMPONENTS_DTS, 'utf8');
  const names = new Set();
  const re = /^\s+([A-Z][A-Za-z0-9]*):\s+typeof import/gm;
  let m;
  while ((m = re.exec(content)) !== null) {
    names.add(m[1]);
  }
  return names;
}

/**
 * @param {string} script
 * @param {Set<string>} globalNames
 * @returns {string}
 */
function stripRedundantComponentImports(script, globalNames) {
  const lines = script.split(/\r?\n/);
  const out = [];

  for (const line of lines) {
    const m = line.match(/^import\s+([A-Z][A-Za-z0-9]*)\s+from\s+['"]([^'"]+\.vue)['"];?\s*$/);
    if (!m) {
      out.push(line);
      continue;
    }

    const name = m[1];
    if (!globalNames.has(name)) {
      out.push(line);
      continue;
    }

    const rest = script
      .replace(line, '')
      .replace(/\/\*[\s\S]*?\*\//g, '')
      .replace(/\/\/.*$/gm, '');
    const usedInScript = new RegExp(`\\b${name}\\b`).test(rest);
    if (usedInScript) {
      out.push(line);
      continue;
    }

    // 仅模板使用 → 由 unplugin-vue-components 自动注册，删除 import
  }

  return out.join('\n');
}

/**
 * @param {string} filePath
 * @param {Set<string>} globalNames
 */
function processFile(filePath, globalNames) {
  let content = fs.readFileSync(filePath, 'utf8');
  const scriptRe = /(<script\s+setup[^>]*>)([\s\S]*?)(<\/script>)/i;
  const match = content.match(scriptRe);
  if (!match) return;

  const nextScript = stripRedundantComponentImports(match[2], globalNames);
  if (nextScript === match[2]) return;

  const nextContent = content.replace(scriptRe, `${match[1]}${nextScript}${match[3]}`);
  fs.writeFileSync(filePath, nextContent, 'utf8');
  console.log('updated:', path.relative(SRC_ROOT, filePath));
}

/**
 * @param {string} dir
 * @param {Set<string>} globalNames
 */
function walk(dir, globalNames) {
  for (const ent of fs.readdirSync(dir, { withFileTypes: true })) {
    const p = path.join(dir, ent.name);
    if (ent.isDirectory()) walk(p, globalNames);
    else if (ent.name.endsWith('.vue')) processFile(p, globalNames);
  }
}

const globalNames = loadGlobalComponentNames();
walk(SRC_ROOT, globalNames);
