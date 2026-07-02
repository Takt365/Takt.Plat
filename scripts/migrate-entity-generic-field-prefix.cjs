// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：scripts
// 文件名称：migrate-entity-generic-field-prefix.cjs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：按 01-backend §六「实体属性命名」为通用字段补实体前缀，并同步 ColumnName / 全栈引用
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const fs = require('fs');
const path = require('path');
const { execSync } = require('child_process');
const {
  DEFAULT_BACKEND_ROOT,
  writeGeneratedFile,
} = require('./generate-script-common.cjs');

const ROOT = path.resolve(__dirname, '..');
const ENTITIES_ROOT = path.join(DEFAULT_BACKEND_ROOT, 'Takt.Domain', 'Entities');
const RENAME_MAP_PATH = path.join(__dirname, '.migrate-entity-field-renames.json');

/** 与 .cursor/rules/01-backend.mdc §六 对齐：仅核心标识字段须加实体前缀 */
const GENERIC_PROPERTY_NAMES = new Set([
  'Code',
  'Type',
  'Status',
  'No',
  'Title',
  'Name',
]);

/** 基类字段，不参与重命名 */
const BASE_PROPERTY_NAMES = new Set([
  'Id',
  'TenantCode',
  'CompanyCode',
  'ExtField',
  'Remark',
  'CreatedBy',
  'CreatedAt',
  'UpdatedBy',
  'UpdatedAt',
  'IsDeleted',
  'DeletedBy',
  'DeletedAt',
  'ApprovalStatus',
  'InitiatorId',
  'InitiatedAt',
  'ApprovalOpinion',
  'ApprovedBy',
  'ApprovedAt',
  'FlowInstanceId',
  'ParentId',
  'SortOrder',
  'Level',
  'DeptPath',
  'IsLeaf',
]);

const SKIP_ENTITY_FILES = new Set([
  'TaktEntityBase.cs',
  'TaktTenantEntityBase.cs',
  'TaktCompanyEntityBase.cs',
  'TaktApprovalEntityBase.cs',
]);

/**
 * PascalCase → snake_case（对齐 TaktStringHelper.ToSnakeCase）
 * @param {string} input
 * @returns {string}
 */
function toSnakeCase(input) {
  if (!input) {
    return input;
  }
  let result = '';
  for (let i = 0; i < input.length; i += 1) {
    const c = input[i];
    if (c >= 'A' && c <= 'Z') {
      if (i > 0) {
        result += '_';
      }
      result += c.toLowerCase();
    } else {
      result += c;
    }
  }
  return result;
}

/**
 * @param {string} dir
 * @returns {string[]}
 */
function walkEntityFiles(dir) {
  /** @type {string[]} */
  const files = [];
  for (const entry of fs.readdirSync(dir, { withFileTypes: true })) {
    const full = path.join(dir, entry.name);
    if (entry.isDirectory()) {
      files.push(...walkEntityFiles(full));
    } else if (entry.name.startsWith('Takt') && entry.name.endsWith('.cs') && !SKIP_ENTITY_FILES.has(entry.name)) {
      files.push(full);
    }
  }
  return files;
}

/**
 * @param {string} content
 * @returns {string|null}
 */
function parseEntityClassName(content) {
  const match = content.match(/public\s+(?:sealed\s+|abstract\s+)?class\s+(Takt\w+)/);
  return match ? match[1] : null;
}

/**
 * @param {string} className
 * @returns {string}
 */
function entityShortFromClass(className) {
  return className.replace(/^Takt/, '');
}

/**
 * @param {string} propName
 * @param {string} entityShort
 * @returns {boolean}
 */
function alreadyHasEntityPrefix(propName, entityShort) {
  return propName.startsWith(entityShort);
}

/**
 * @param {string} content
 * @returns {{ propName: string, typePart: string, hasNavigate: boolean }[]}
 */
function parseEntityProperties(content) {
  /** @type {{ propName: string, typePart: string, hasNavigate: boolean }[]} */
  const props = [];
  const propRegex = /public\s+([\w?<>,\s\[\]]+?)\s+(\w+)\s*\{/g;
  let match;
  while ((match = propRegex.exec(content)) !== null) {
    const typePart = match[1].trim();
    const propName = match[2];
    const before = content.slice(Math.max(0, match.index - 800), match.index);
    const hasNavigate = /\[Navigate\b/.test(before);
    props.push({ propName, typePart, hasNavigate });
  }
  return props;
}

/**
 * @param {string} filePath
 * @returns {{ className: string, entityShort: string, renames: Record<string, string> }}
 */
function analyzeEntityFile(filePath) {
  const content = fs.readFileSync(filePath, 'utf-8');
  const className = parseEntityClassName(content);
  if (!className) {
    return { className: '', entityShort: '', renames: {} };
  }
  const entityShort = entityShortFromClass(className);
  /** @type {Record<string, string>} */
  const renames = {};

  for (const { propName, typePart, hasNavigate } of parseEntityProperties(content)) {
    if (BASE_PROPERTY_NAMES.has(propName)) {
      continue;
    }
    if (!GENERIC_PROPERTY_NAMES.has(propName)) {
      continue;
    }
    if (alreadyHasEntityPrefix(propName, entityShort)) {
      continue;
    }
    if (hasNavigate) {
      continue;
    }
    if (/List\s*</.test(typePart) || /IEnumerable\s*</.test(typePart) || /ICollection\s*</.test(typePart)) {
      continue;
    }
    renames[propName] = `${entityShort}${propName}`;
  }
  return { className, entityShort, renames };
}

/**
 * @param {string} content
 * @param {string} oldName
 * @param {string} newName
 * @returns {string}
 */
function updateColumnNameForProperty(content, oldName, newName) {
  const snake = toSnakeCase(newName);
  const propPattern = new RegExp(`public\\s+[\\w?<>,\\s\\[\\]]+?\\s+${oldName}\\s*\\{`);
  const propMatch = propPattern.exec(content);
  if (!propMatch) {
    return content;
  }
  const before = content.slice(0, propMatch.index);
  const colRegex = /ColumnName\s*=\s*"[^"]*"/g;
  let lastCol = null;
  let m;
  while ((m = colRegex.exec(before)) !== null) {
    lastCol = m;
  }
  if (!lastCol) {
    return content;
  }
  const start = lastCol.index;
  const end = lastCol.index + lastCol[0].length;
  return `${content.slice(0, start)}ColumnName = "${snake}"${content.slice(end)}`;
}

/**
 * @param {string} content
 * @param {Record<string, string>} renames
 * @returns {string}
 */
function applyRenamesToEntityContent(content, renames) {
  let next = content;
  const entries = Object.entries(renames).sort((a, b) => b[0].length - a[0].length);
  for (const [oldName, newName] of entries) {
    next = updateColumnNameForProperty(next, oldName, newName);
    next = next.replace(new RegExp(`\\bnameof\\(${oldName}\\)`, 'g'), `nameof(${newName})`);
    next = next.replace(
      new RegExp(`(public\\s+[\\w?<>,\\s\\[\\]]+?\\s+)${oldName}(\\s*\\{)`, 'g'),
      `$1${newName}$2`,
    );
  }
  return next;
}

/**
 * @param {string} dir
 * @param {(filePath: string) => void} visitor
 */
function walkCodeFiles(dir, visitor) {
  if (!fs.existsSync(dir)) {
    return;
  }
  for (const entry of fs.readdirSync(dir, { withFileTypes: true })) {
    const full = path.join(dir, entry.name);
    if (entry.isDirectory()) {
      if (entry.name === 'node_modules' || entry.name === 'bin' || entry.name === 'obj') {
        continue;
      }
      walkCodeFiles(full, visitor);
    } else if (/\.(cs|ts|vue|cjs)$/.test(entry.name) && !entry.name.includes('migrate-entity-generic-field-prefix')) {
      visitor(full);
    }
  }
}

/**
 * @param {string} content
 * @param {string} oldName
 * @param {string} newName
 * @returns {string}
 */
function replacePropertyIdentifier(content, oldName, newName) {
  if (oldName === newName) {
    return content;
  }
  const patterns = [
    new RegExp(`\\.${oldName}\\b`, 'g'),
    new RegExp(`\\b${oldName}\\s*==`, 'g'),
    new RegExp(`\\b${oldName}\\s*!=`, 'g'),
    new RegExp(`\\b${oldName}\\s*=>`, 'g'),
    new RegExp(`\\(\\s*${oldName}\\s*[,)]`, 'g'),
    new RegExp(`\\{\\s*${oldName}\\s*[,}]`, 'g'),
    new RegExp(`,\\s*${oldName}\\s*[,}]`, 'g'),
    new RegExp(`nameof\\(${oldName}\\)`, 'g'),
    new RegExp(`\\[JsonPropertyName\\("${oldName.charAt(0).toLowerCase() + oldName.slice(1)}"\\)\\]`, 'g'),
  ];
  let next = content;
  next = next.replace(patterns[0], `.${newName}`);
  next = next.replace(patterns[1], `${newName} ==`);
  next = next.replace(patterns[2], `${newName} !=`);
  next = next.replace(patterns[3], `${newName} =>`);
  next = next.replace(patterns[4], (m) => m.replace(oldName, newName));
  next = next.replace(patterns[5], (m) => m.replace(oldName, newName));
  next = next.replace(patterns[6], (m) => m.replace(oldName, newName));
  next = next.replace(patterns[7], `nameof(${newName})`);
  const camelNew = newName.charAt(0).toLowerCase() + newName.slice(1);
  next = next.replace(patterns[8], `[JsonPropertyName("${camelNew}")]`);
  return next;
}

/**
 * @param {Record<string, Record<string, string>>} fullMap
 * @param {string[]} roots
 */
function applyRenamesToCodebase(fullMap, roots) {
  /** @type {Map<string, { oldName: string, newName: string }[]>} */
  const fileHints = new Map();
  for (const [className, renames] of Object.entries(fullMap)) {
    const entityShort = entityShortFromClass(className);
    const hints = Object.entries(renames).map(([oldName, newName]) => ({ oldName, newName }));
    for (const root of roots) {
      walkCodeFiles(root, (filePath) => {
        const base = path.basename(filePath);
        if (
          base.includes(entityShort)
          || base.includes(className)
          || filePath.includes(entityShort.toLowerCase())
        ) {
          const list = fileHints.get(filePath) || [];
          list.push(...hints);
          fileHints.set(filePath, list);
        }
      });
    }
  }

  let changedFiles = 0;
  for (const [filePath, hints] of fileHints.entries()) {
    let content = fs.readFileSync(filePath, 'utf-8');
    const original = content;
    const unique = [...new Map(hints.map((h) => [h.oldName, h])).values()].sort((a, b) => b.oldName.length - a.oldName.length);
    for (const { oldName, newName } of unique) {
      content = replacePropertyIdentifier(content, oldName, newName);
    }
    if (content !== original) {
      writeGeneratedFile(filePath, content, { skipHeader: true });
      changedFiles += 1;
    }
  }
  return changedFiles;
}

function printUsage() {
  console.log(`
用法: node scripts/migrate-entity-generic-field-prefix.cjs [参数]

参数:
  --dry-run           仅分析并输出将重命名的字段（默认）
  --apply-entities    写入实体 .cs 并生成 ${path.basename(RENAME_MAP_PATH)}
  --apply-codebase    按映射表更新 backend / frontend 相关引用
  --regen-dtos        为受影响实体执行 generate-dtos-from-entity.cjs
  --regen-validators  为受影响实体执行 generate-validators-from-entity.cjs
  --regen-frontend    为受影响实体执行 generate-from-backend.cjs
  --all               依次执行 apply-entities → apply-codebase → regen-dtos → regen-validators → regen-frontend
`);
}

function main() {
  const args = process.argv.slice(2);
  if (args.includes('--help') || args.includes('-h')) {
    printUsage();
    return;
  }

  const dryRun = !args.some((a) => a.startsWith('--apply') || a === '--all' || a.startsWith('--regen'));
  const applyEntities = args.includes('--apply-entities') || args.includes('--all');
  const applyCodebase = args.includes('--apply-codebase') || args.includes('--all');
  const regenDtos = args.includes('--regen-dtos') || args.includes('--all');
  const regenValidators = args.includes('--regen-validators') || args.includes('--all');
  const regenFrontend = args.includes('--regen-frontend') || args.includes('--all');

  const entityFiles = walkEntityFiles(ENTITIES_ROOT);
  /** @type {Record<string, Record<string, string>>} */
  const fullMap = {};
  let totalRenames = 0;

  for (const filePath of entityFiles) {
    const { className, entityShort, renames } = analyzeEntityFile(filePath);
    if (!className || Object.keys(renames).length === 0) {
      continue;
    }
    fullMap[className] = renames;
    totalRenames += Object.keys(renames).length;
    const rel = path.relative(ROOT, filePath);
    console.log(`\n${className} (${rel})`);
    for (const [oldName, newName] of Object.entries(renames)) {
      console.log(`  ${oldName} → ${newName} (列: ${toSnakeCase(newName)})`);
    }
    if (applyEntities) {
      const content = fs.readFileSync(filePath, 'utf-8');
      const updated = applyRenamesToEntityContent(content, renames);
      if (updated !== content) {
        writeGeneratedFile(filePath, updated, { skipHeader: true });
        console.log(`  ✅ 已更新实体文件`);
      }
    }
  }

  console.log(`\n共 ${Object.keys(fullMap).length} 个实体、${totalRenames} 处字段待迁移`);

  if (applyEntities) {
    writeGeneratedFile(RENAME_MAP_PATH, `${JSON.stringify(fullMap, null, 2)}\n`, { skipHeader: true });
    console.log(`\n映射表已写入 ${path.relative(ROOT, RENAME_MAP_PATH)}`);
  }

  const map = applyEntities
    ? fullMap
    : (fs.existsSync(RENAME_MAP_PATH) ? JSON.parse(fs.readFileSync(RENAME_MAP_PATH, 'utf-8')) : fullMap);

  if (applyCodebase && Object.keys(map).length > 0) {
    const changed = applyRenamesToCodebase(map, [
      path.join(ROOT, 'backend', 'src'),
      path.join(ROOT, 'frontend', 'src'),
    ]);
    console.log(`\n全栈引用已更新 ${changed} 个文件`);
  }

  const affectedShorts = [...new Set(Object.keys(map).map(entityShortFromClass))];
  if (regenDtos) {
    for (const entityShort of affectedShorts) {
      console.log(`\n▶ generate-dtos --${entityShort}`);
      execSync(`node scripts/generate-dtos-from-entity.cjs --${entityShort}`, { cwd: ROOT, stdio: 'inherit' });
    }
  }
  if (regenValidators) {
    console.log('\n▶ generate-validators --all');
    execSync('node scripts/generate-validators-from-entity.cjs --all', { cwd: ROOT, stdio: 'inherit' });
  }
  if (regenFrontend) {
    for (const entityShort of affectedShorts) {
      console.log(`\n▶ generate-from-backend --${entityShort}`);
      try {
        execSync(`node scripts/generate-from-backend.cjs --${entityShort}`, { cwd: ROOT, stdio: 'inherit' });
      } catch (err) {
        console.warn(`⚠️  跳过前端生成 ${entityShort}: ${err.message}`);
      }
    }
  }

  if (dryRun) {
    console.log('\n（dry-run）加 --apply-entities 写入实体，或 --all 执行完整迁移');
  }
}

main();
