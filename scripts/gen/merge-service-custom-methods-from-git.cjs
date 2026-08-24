// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：scripts/gen
// 文件名称：merge-service-custom-methods-from-git.cjs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：从指定 git 提交合并非标准应用服务方法到当前文件（一次性修复 regenerate 丢失）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const fs = require('fs');
const path = require('path');
const { execSync } = require('child_process');
const {
  extractPreservedExtraServiceMethods,
  listServiceMethodNames,
} = require('./service-method-preservation.cjs');
const { writeGeneratedFile } = require('./generate-script-common.cjs');

const GIT_REF = process.argv[2] || 'd545e92f';
const BACKEND_ROOT = path.resolve(__dirname, '../../backend/src');
const SERVICES_ROOT = path.join(BACKEND_ROOT, 'Takt.Application', 'Services');

/** @type {string[]} */
const ENTITY_SHORT_NAMES = [
  'Translation',
  'DictData',
  'FlowInstance',
  'Configurable',
  'Equipment',
  'MaintenanceHistory',
  'MaintenanceNotification',
  'IqcOrder',
  'IpqcOrder',
  'EcGijutsu',
  'AssyOutput',
];

/**
 * 读取 git 中文件内容
 * @param {string} repoRelPath
 * @returns {string|null}
 */
function readGitFile(repoRelPath) {
  try {
    return execSync(`git show ${GIT_REF}:${repoRelPath}`, {
      cwd: path.resolve(__dirname, '../..'),
      encoding: 'utf-8',
      stdio: ['ignore', 'pipe', 'ignore'],
    });
  } catch {
    return null;
  }
}

/**
 * 递归查找服务文件
 * @param {string} fileName
 * @returns {string|null}
 */
function findServiceFile(fileName) {
  function walk(dir) {
    if (!fs.existsSync(dir)) {
      return null;
    }
    for (const entry of fs.readdirSync(dir, { withFileTypes: true })) {
      const full = path.join(dir, entry.name);
      if (entry.isDirectory()) {
        const found = walk(full);
        if (found) {
          return found;
        }
      } else if (entry.name === fileName) {
        return full;
      }
    }
    return null;
  }
  return walk(SERVICES_ROOT);
}

/**
 * 在类结束前插入块
 * @param {string} content
 * @param {string} insertBlock
 * @returns {string}
 */
function insertBeforeClassClose(content, insertBlock) {
  const lastBrace = content.lastIndexOf('}');
  if (lastBrace < 0) {
    return content + insertBlock;
  }
  return `${content.slice(0, lastBrace)}${insertBlock}\n${content.slice(lastBrace)}`;
}

/**
 * 合并单个实体的扩展方法
 * @param {string} entityShort
 */
function mergeEntity(entityShort) {
  const entityName = `Takt${entityShort}`;
  const ifaceName = `I${entityName}Service.cs`;
  const implName = `${entityName}Service.cs`;
  const ifacePath = findServiceFile(ifaceName);
  const implPath = findServiceFile(implName);
  if (!ifacePath || !implPath) {
    console.log(`⏭️  跳过 ${entityShort}：未找到服务文件`);
    return;
  }
  const repoRelIface = path.relative(path.resolve(__dirname, '../..'), ifacePath).replace(/\\/g, '/');
  const repoRelImpl = path.relative(path.resolve(__dirname, '../..'), implPath).replace(/\\/g, '/');
  const gitIface = readGitFile(repoRelIface);
  const gitImpl = readGitFile(repoRelImpl);
  if (!gitIface || !gitImpl) {
    console.log(`⏭️  跳过 ${entityShort}：git ${GIT_REF} 无历史文件`);
    return;
  }
  const currentIface = fs.readFileSync(ifacePath, 'utf-8');
  const currentImpl = fs.readFileSync(implPath, 'utf-8');
  const dtoInfo = { entityName, statuses: [], sort: null, builtIn: null, obsolete: null, tree: null, template: null, import: null };
  const gitExtraIface = extractPreservedExtraServiceMethods(gitIface, entityShort, dtoInfo, '', 'interface');
  const gitExtraImpl = extractPreservedExtraServiceMethods(gitImpl, entityShort, dtoInfo, '', 'implementation');
  const currentIfaceMethods = new Set(listServiceMethodNames(currentIface, 'interface'));
  const currentImplMethods = new Set(listServiceMethodNames(currentImpl, 'implementation'));
  let ifaceBlocks = '';
  let implBlocks = '';
  const merged = [];
  for (const name of gitExtraIface.methodNames) {
    if (currentIfaceMethods.has(name)) {
      continue;
    }
    const block = extractPreservedExtraServiceMethods(gitIface, entityShort, dtoInfo, '', 'interface');
    const single = require('./service-method-preservation.cjs').extractServiceMethodBlock(gitIface, name, 'interface');
    if (single) {
      ifaceBlocks += single;
      merged.push(name);
    }
  }
  for (const name of gitExtraImpl.methodNames) {
    if (currentImplMethods.has(name)) {
      continue;
    }
    const single = require('./service-method-preservation.cjs').extractServiceMethodBlock(gitImpl, name, 'implementation');
    if (single) {
      implBlocks += single;
      if (!merged.includes(name)) {
        merged.push(name);
      }
    }
  }
  if (!ifaceBlocks && !implBlocks) {
    console.log(`✓ ${entityShort}：无缺失扩展方法`);
    return;
  }
  let newIface = currentIface;
  let newImpl = currentImpl;
  if (ifaceBlocks) {
    const marker = '    // ========================================\n    // 扩展方法（保留）\n';
    if (!newIface.includes(marker)) {
      ifaceBlocks = `\n    // ========================================\n    // 扩展方法（保留）\n    // ========================================\n\n${ifaceBlocks}`;
    }
    newIface = insertBeforeClassClose(newIface, ifaceBlocks);
  }
  if (implBlocks) {
    const marker = '    // ========================================\n    // 扩展方法（保留）\n';
    if (!newImpl.includes(marker)) {
      implBlocks = `\n    // ========================================\n    // 扩展方法（保留）\n    // ========================================\n\n${implBlocks}`;
    }
    newImpl = insertBeforeClassClose(newImpl, implBlocks);
  }
  writeGeneratedFile(ifacePath, newIface);
  writeGeneratedFile(implPath, newImpl);
  console.log(`✅ ${entityShort}：已合并 ${merged.join(', ')}`);
}

console.log(`从 git ${GIT_REF} 合并应用服务扩展方法…\n`);
ENTITY_SHORT_NAMES.forEach(mergeEntity);
console.log('\n完成。请运行 node scripts/gen/generate-services-from-dtos.cjs --<Entity> 验证保留逻辑。');
