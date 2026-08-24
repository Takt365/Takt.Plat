// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：scripts/gen
// 文件名称：append-git-service-methods.cjs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：从 git 提交追加缺失的应用服务扩展方法（generate 前无历史文件时使用）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const fs = require('fs');
const path = require('path');
const { execSync } = require('child_process');
const {
  extractServiceMethodBlock,
  listServiceMethodNames,
} = require('./service-method-preservation.cjs');
const { writeGeneratedFile } = require('./generate-script-common.cjs');

const GIT_REF = process.argv[2] || 'd545e92f';
const REPO_ROOT = path.resolve(__dirname, '../..');

/** @type {Record<string, { repoRelIface: string, repoRelImpl: string, methods: string[] }>} */
const TARGETS = {
  AssyOutput: {
    repoRelIface: 'backend/src/Takt.Application/Services/Logistics/Manufacturing/Output/ITaktAssyOutputService.cs',
    repoRelImpl: 'backend/src/Takt.Application/Services/Logistics/Manufacturing/Output/TaktAssyOutputService.cs',
    methods: ['GetAssyOutputProdOrderOptionsAsync', 'GetAssyOutputDefaultTimePeriodsAsync'],
  },
  EcGijutsu: {
    repoRelIface:
      'backend/src/Takt.Application/Services/Logistics/Manufacturing/EngineeringChange/ITaktEcGijutsuService.cs',
    repoRelImpl:
      'backend/src/Takt.Application/Services/Logistics/Manufacturing/EngineeringChange/TaktEcGijutsuService.cs',
    methods: ['GetUnimportedSourceEcGijutsuListAsync'],
  },
};

/**
 * @param {string} repoRelPath
 * @returns {string|null}
 */
function readGitFile(repoRelPath) {
  try {
    return execSync(`git show ${GIT_REF}:${repoRelPath}`, {
      cwd: REPO_ROOT,
      encoding: 'utf-8',
      stdio: ['ignore', 'pipe', 'ignore'],
    });
  } catch {
    return null;
  }
}

/**
 * @param {string} content
 * @param {string} block
 * @returns {string}
 */
function insertBeforeClassClose(content, block) {
  const marker = '    // 扩展方法（保留）';
  if (content.includes(marker) && content.includes(block.trim().slice(0, 40))) {
    return content;
  }
  const lastBrace = content.lastIndexOf('}');
  if (lastBrace < 0) {
    return content + block;
  }
  let insert = block;
  if (!content.includes(marker)) {
    insert = `\n    // ========================================\n    // 扩展方法（保留）\n    // ========================================\n\n${insert}`;
  }
  return `${content.slice(0, lastBrace)}${insert}\n${content.slice(lastBrace)}`;
}

/**
 * @param {string} key
 * @param {{ repoRelIface: string, repoRelImpl: string, methods: string[] }} target
 */
function appendForEntity(key, target) {
  const ifacePath = path.join(REPO_ROOT, target.repoRelIface);
  const implPath = path.join(REPO_ROOT, target.repoRelImpl);
  const gitIface = readGitFile(target.repoRelIface);
  const gitImpl = readGitFile(target.repoRelImpl);
  if (!gitIface || !gitImpl) {
    console.log(`⏭️  ${key}：git 无历史`);
    return;
  }
  let iface = fs.readFileSync(ifacePath, 'utf-8');
  let impl = fs.readFileSync(implPath, 'utf-8');
  const ifaceNames = new Set(listServiceMethodNames(iface, 'interface'));
  const implNames = new Set(listServiceMethodNames(impl, 'implementation'));
  const merged = [];
  for (const name of target.methods) {
    if (!ifaceNames.has(name)) {
      const block = extractServiceMethodBlock(gitIface, name, 'interface');
      if (block) {
        iface = insertBeforeClassClose(iface, block);
        merged.push(`${name}(iface)`);
      }
    }
    if (!implNames.has(name)) {
      const block = extractServiceMethodBlock(gitImpl, name, 'implementation');
      if (block) {
        impl = insertBeforeClassClose(impl, block);
        merged.push(`${name}(impl)`);
      }
    }
  }
  if (merged.length === 0) {
    console.log(`✓ ${key}：无缺失方法`);
    return;
  }
  writeGeneratedFile(ifacePath, iface);
  writeGeneratedFile(implPath, impl);
  console.log(`✅ ${key}：${merged.join(', ')}`);
}

console.log(`从 git ${GIT_REF} 追加缺失扩展方法…\n`);
for (const [key, target] of Object.entries(TARGETS)) {
  appendForEntity(key, target);
}
console.log('\n完成。');
