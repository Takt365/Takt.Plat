// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/scripts
// 文件名称：generate-entity-rbac-navigations.cjs
// 创建时间：2026-06-01
// 创建人：Takt365(Cursor AI)
// 功能描述：按 rbac-parent-config 为主实体写入 RBAC OneToMany 导航属性区域
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const fs = require('fs');
const path = require('path');
const { writeGeneratedFile, logGeneratedFileWritePolicy } = require('./generate-script-common.cjs');
const {
  NAVIGATION_REGION_MARKER,
  RBAC_PARENT_NAVIGATIONS,
  getRbacParentNavigations,
  collectRbacJunctionUsings,
} = require('./rbac-parent-config.cjs');

const ENTITIES_ROOT = path.join(path.resolve(__dirname, '../backend/src'), 'Takt.Domain', 'Entities');

/**
 * @param {string} dir
 * @param {string} entityShort
 */
function findEntityFile(dir, entityShort) {
  const target = `Takt${entityShort}.cs`;
  const entries = fs.readdirSync(dir, { withFileTypes: true });
  for (const entry of entries) {
    const full = path.join(dir, entry.name);
    if (entry.isDirectory()) {
      const found = findEntityFile(full, entityShort);
      if (found) {
        return found;
      }
      continue;
    }
    if (entry.name === target) {
      return full;
    }
  }
  return null;
}

/**
 * @param {string} content
 */
function parseEntityNamespace(content) {
  const m = content.match(/namespace\s+([\w.]+)\s*;/);
  return m ? m[1] : '';
}

/**
 * @param {string} content
 */
function mergeUsings(content, entityNamespace, entityShort) {
  const needed = collectRbacJunctionUsings(entityNamespace, entityShort);
  if (!needed.length) {
    return content;
  }
  const lines = content.split('\n');
  let insertAt = 0;
  for (let i = 0; i < lines.length; i += 1) {
    if (lines[i].startsWith('using ')) {
      insertAt = i + 1;
    } else if (lines[i].trim() === '' && insertAt > 0) {
      break;
    } else if (!lines[i].startsWith('//') && lines[i].trim() !== '' && insertAt > 0) {
      break;
    }
  }
  const existing = new Set(
    lines.filter((l) => l.startsWith('using ')).map((l) => l.trim()),
  );
  const toAdd = needed
    .map((ns) => `using ${ns};`)
    .filter((u) => !existing.has(u));
  if (!toAdd.length) {
    return content;
  }
  const out = [...lines.slice(0, insertAt), ...toAdd, ...lines.slice(insertAt)];
  return out.join('\n');
}

/**
 * @param {object[]} navSpecs
 */
function buildNavigationRegionLines(navSpecs) {
  const lines = [];
  lines.push('    // ========================================');
  lines.push(`    // ${NAVIGATION_REGION_MARKER}`);
  lines.push('    // ========================================');
  lines.push('');
  navSpecs.forEach((spec) => {
    lines.push('    /// <summary>');
    lines.push(`    /// ${spec.summary}（RBAC，表 ${spec.table}）`);
    lines.push('    /// </summary>');
    lines.push(
      `    [Navigate(NavigateType.OneToMany, nameof(Takt${spec.junction}.${spec.fkOnChild}))]`,
    );
    lines.push(`    public List<Takt${spec.junction}>? ${spec.navProp} { get; set; }`);
    lines.push('');
  });
  return lines.join('\n');
}

/**
 * @param {string} content
 * @param {string} navigationBlock
 */
function replaceNavigationRegion(content, navigationBlock) {
  const classMatch = content.match(/public\s+class\s+Takt\w+[\s\S]*?\{/);
  if (!classMatch) {
    return null;
  }
  const openIdx = classMatch.index + classMatch[0].length - 1;
  let depth = 1;
  let i = openIdx + 1;
  while (i < content.length && depth > 0) {
    if (content[i] === '{') {
      depth += 1;
    } else if (content[i] === '}') {
      depth -= 1;
    }
    i += 1;
  }
  const classEndIdx = i - 1;
  const classBody = content.slice(openIdx + 1, classEndIdx);
  const markerIdx = classBody.indexOf(NAVIGATION_REGION_MARKER);
  let newBody;
  if (markerIdx >= 0) {
    let navStart = classBody.lastIndexOf('// ========================================', markerIdx);
    if (navStart < 0) {
      navStart = markerIdx;
    }
    const scalarBody = classBody.slice(0, navStart).replace(/\s+$/, '');
    const sep = scalarBody.length ? '\n\n' : '\n';
    newBody = `${scalarBody}${sep}${navigationBlock}\n`;
  } else {
    const trimmed = classBody.replace(/\s+$/, '');
    const sep = trimmed.length ? '\n\n' : '\n';
    newBody = `${trimmed}${sep}${navigationBlock}\n`;
  }
  return content.slice(0, openIdx + 1) + newBody + content.slice(classEndIdx);
}

/**
 * @param {string} entityShort
 * @param {{ dryRun?: boolean }} [options]
 */
function syncEntityRbacNavigations(entityShort, options = {}) {
  const navSpecs = getRbacParentNavigations(entityShort);
  if (!navSpecs.length) {
    return { status: 'skipped', reason: 'no-config' };
  }
  const entityFile = findEntityFile(ENTITIES_ROOT, entityShort);
  if (!entityFile) {
    return { status: 'failed', reason: 'file-not-found' };
  }
  let content = fs.readFileSync(entityFile, 'utf-8');
  const entityNamespace = parseEntityNamespace(content);
  content = mergeUsings(content, entityNamespace, entityShort);
  const navigationBlock = buildNavigationRegionLines(navSpecs);
  const updated = replaceNavigationRegion(content, navigationBlock);
  if (!updated) {
    return { status: 'failed', reason: 'parse-failed' };
  }
  if (options.dryRun) {
    return { status: 'dry-run', path: entityFile };
  }
  const writeResult = writeGeneratedFile(entityFile, updated);
  return {
    status: writeResult.created ? 'created' : 'updated',
    path: entityFile,
    created: writeResult.created,
    updated: writeResult.updated,
  };
}

/**
 * @param {string} entitiesRoot
 * @param {{ dryRun?: boolean, entityPrefix?: string|null }} [options]
 */
function syncAllRbacParentEntityNavigations(entitiesRoot, options = {}) {
  const root = entitiesRoot || ENTITIES_ROOT;
  const results = [];
  Object.keys(RBAC_PARENT_NAVIGATIONS).forEach((entityShort) => {
    if (options.entityPrefix && options.entityPrefix !== entityShort) {
      return;
    }
    const file = findEntityFile(root, entityShort);
    const r = syncEntityRbacNavigations(entityShort, { ...options, entityFile: file });
    results.push({ entityShort, ...r });
  });
  return results;
}

function printUsage() {
  console.log(`
用法: node scripts/generate-entity-rbac-navigations.cjs [参数]

参数:
  --all              同步全部 RBAC 主实体（User/Tenant/Role/Menu/Company/Employee/Dept/Post）
  --<实体名>         仅同步指定实体，如 --Role
  --dry-run          仅打印，不写入

说明:
  - 配置源：scripts/rbac-parent-config.cjs → RBAC_PARENT_NAVIGATIONS
  - 写入「导航属性区域」内 SqlSugar [Navigate] OneToMany
  - 须先于 generate-dtos-from-entity.cjs（DTO 从实体导航生成 List<关联Dto> 与 Create *Ids）
`);
}

function parseArgs() {
  const args = process.argv.slice(2);
  const options = { all: false, entityPrefix: null, dryRun: false };
  args.forEach((arg) => {
    if (arg === '--dry-run') {
      options.dryRun = true;
      return;
    }
    if (arg.startsWith('--')) {
      const value = arg.slice(2);
      if (value.toLowerCase() === 'all') {
        options.all = true;
        return;
      }
      if (value.startsWith('Takt')) {
        console.error('❌ 实体名不要带 Takt 前缀');
        process.exit(1);
      }
      options.entityPrefix = value;
    }
  });
  if (!options.all && !options.entityPrefix) {
    console.error('❌ 请指定 --all 或 --<实体名>');
    printUsage();
    process.exit(1);
  }
  return options;
}

if (require.main === module) {
  console.log('🚀 同步主实体 RBAC 导航属性...\n');
  logGeneratedFileWritePolicy();
  const options = parseArgs();
  const results = syncAllRbacParentEntityNavigations(ENTITIES_ROOT, options);
  results.forEach((r) => {
    if (r.status === 'skipped') {
      console.log(`  ⏭️  ${r.entityShort}: 无配置`);
    } else if (r.status === 'failed') {
      console.log(`  ❌ ${r.entityShort}: ${r.reason}`);
    } else if (r.status === 'dry-run') {
      console.log(`  📄 [dry-run] ${r.path}`);
    } else {
      console.log(`  ✅ ${r.entityShort}: ${r.path}`);
    }
  });
  console.log('\n✨ 完成');
}

module.exports = {
  syncEntityRbacNavigations,
  syncAllRbacParentEntityNavigations,
  findEntityFile,
};
