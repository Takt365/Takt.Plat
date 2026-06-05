/**
 * 按 TaktCompany.cs 标准规范化实体 SugarIndex（一次性手工规则落盘）：
 * 1. 字段顺序：TenantCode → CompanyCode（公司/审批级）→ 业务字段 → true/false
 * 2. 声明顺序：tenant 隔离 → is_deleted → 唯一索引 → 其它普通索引
 * 3. 公司/审批级首条：ix_{short}_tenant（TenantCode + CompanyCode + false）
 * 4. 租户级首条：ix_{short}_tenant（TenantCode + false）
 */
const fs = require('fs');
const path = require('path');

const TARGET_DIRS = [
  path.join(__dirname, '../backend/src/Takt.Domain/Entities/Foundation'),
  path.join(__dirname, '../backend/src/Takt.Domain/Entities/Accounting'),
  path.join(__dirname, '../backend/src/Takt.Domain/Entities/Code'),
  path.join(__dirname, '../backend/src/Takt.Domain/Entities/HumanResource'),
  path.join(__dirname, '../backend/src/Takt.Domain/Entities/Identity'),
  path.join(__dirname, '../backend/src/Takt.Domain/Entities/Logistics'),
  path.join(__dirname, '../backend/src/Takt.Domain/Entities/Routine'),
  path.join(__dirname, '../backend/src/Takt.Domain/Entities/Statistics'),
];

const SCOPE_BY_BASE = {
  TaktTenantEntityBase: ['TenantCode'],
  TaktCompanyEntityBase: ['TenantCode', 'CompanyCode'],
  TaktApprovalEntityBase: ['TenantCode', 'CompanyCode'],
};

/** @returns {string} 实体短前缀（PascalCase → snake_case，如 DictData → dict_data） */
function toShortPrefix(className) {
  const name = className.replace(/^Takt/, '');
  return name
    .replace(/([a-z0-9])([A-Z])/g, '$1_$2')
    .replace(/([A-Z])([A-Z][a-z])/g, '$1_$2')
    .toLowerCase();
}

/** @returns {string[]} */
function walkCsFiles(dir, acc = []) {
  if (!fs.existsSync(dir)) {
    return acc;
  }
  for (const entry of fs.readdirSync(dir, { withFileTypes: true })) {
    const full = path.join(dir, entry.name);
    if (entry.isDirectory()) {
      walkCsFiles(full, acc);
    } else if (entry.name.endsWith('.cs') && entry.name !== 'TaktEntityBase.cs') {
      acc.push(full);
    }
  }
  return acc;
}

/**
 * @param {string} content
 * @returns {{ entityBase: string, className: string } | null}
 */
function detectEntity(content) {
  const m = content.match(/public\s+(?:partial\s+)?class\s+(Takt\w+)\s*:\s*(Takt(?:Tenant|Company|Approval)EntityBase)\b/);
  if (!m) {
    return null;
  }
  return { className: m[1], entityBase: m[2] };
}

/**
 * @param {string} line
 */
function parseIndexLine(line) {
  const trimmed = line.trim();
  if (!trimmed.startsWith('[SugarIndex(') || !trimmed.endsWith(')]')) {
    return null;
  }
  const inner = trimmed.slice('[SugarIndex('.length, -2);
  const indexNameMatch = inner.match(/^"([^"]+)"/);
  if (!indexNameMatch) {
    return null;
  }
  const indexName = indexNameMatch[1];
  const afterName = inner.slice(indexNameMatch[0].length).replace(/^,/, '').trim();
  const isUnique = /,\s*true\s*$/.test(afterName);
  const body = afterName.replace(/,\s*(true|false)\s*$/, '').trim();
  const entries = [];
  const partRegex = /nameof\((\w+)\)\s*,\s*(OrderByType\.\w+)/g;
  let pm;
  while ((pm = partRegex.exec(body)) !== null) {
    entries.push({ field: pm[1], order: pm[2] });
  }
  if (entries.length === 0) {
    return null;
  }
  return { indexName, entries, isUnique, raw: trimmed };
}

/**
 * @param {Array<{ field: string, order: string }>} entries
 * @param {string} entityBase
 */
function normalizeEntries(entries, entityBase) {
  const scopeFields = SCOPE_BY_BASE[entityBase];
  const entryMap = new Map(entries.map((e) => [e.field, e]));
  const scopeEntries = scopeFields.map((field) => entryMap.get(field) || { field, order: 'OrderByType.Asc' });
  const others = entries.filter((e) => !scopeFields.includes(e.field));
  return [...scopeEntries, ...others];
}

/**
 * @param {Array<{ field: string, order: string }>} entries
 * @param {string} entityBase
 */
function isScopeOnlyIndex(entries, entityBase) {
  const scopeFields = SCOPE_BY_BASE[entityBase];
  if (entries.length !== scopeFields.length) {
    return false;
  }
  return scopeFields.every((field, i) => entries[i].field === field);
}

/**
 * @param {Array<{ field: string, order: string }>} entries
 */
function isIsDeletedIndex(entries) {
  return entries.length >= 2 && entries[entries.length - 1].field === 'IsDeleted';
}

/**
 * @param {string} indexName
 * @param {boolean} isUnique
 */
function normalizeUniqueName(indexName, isUnique) {
  if (!isUnique) {
    return indexName;
  }
  if (indexName.endsWith('_unique')) {
    return indexName;
  }
  return `${indexName}_unique`;
}

/**
 * @param {string} indexName
 * @param {Array<{ field: string, order: string }>} entries
 * @param {boolean} isUnique
 */
function buildIndexLine(indexName, entries, isUnique) {
  const parts = [`"${indexName}"`];
  for (const e of entries) {
    parts.push(`nameof(${e.field})`, e.order);
  }
  parts.push(isUnique ? 'true' : 'false');
  return `[SugarIndex(${parts.join(', ')})]`;
}

/**
 * @param {string} content
 * @param {string} entityBase
 * @param {string} shortPrefix
 */
function normalizeFile(content, entityBase, shortPrefix) {
  const lines = content.split('\n');
  const indexLineNos = [];
  for (let i = 0; i < lines.length; i++) {
    if (lines[i].trim().startsWith('[SugarIndex(')) {
      indexLineNos.push(i);
    }
  }
  if (indexLineNos.length === 0) {
    return { content, changed: false };
  }
  const scopeFields = SCOPE_BY_BASE[entityBase];
  const parsed = [];
  const unparsedLineNos = [];
  for (const lineNo of indexLineNos) {
    const p = parseIndexLine(lines[lineNo]);
    if (!p) {
      unparsedLineNos.push(lineNo);
      continue;
    }
    const entries = normalizeEntries(p.entries, entityBase);
    const name = normalizeUniqueName(p.indexName, p.isUnique);
    parsed.push({
      lineNo,
      indexName: name,
      entries,
      isUnique: p.isUnique,
      isScopeOnly: isScopeOnlyIndex(entries, entityBase),
      isIsDeleted: isIsDeletedIndex(entries),
    });
  }
  if (unparsedLineNos.length > 0) {
    return { content, changed: false, skipped: true, unparsed: unparsedLineNos.length };
  }
  const tenantIndexName = `ix_${shortPrefix}_tenant`;
  const isDeletedIndexName = `ix_${shortPrefix}_is_deleted`;
  let scopeIndex = parsed.find((p) => p.isScopeOnly);
  let isDeletedIndex = parsed.find((p) => p.isIsDeleted);
  const uniqueIndexes = parsed.filter((p) => p.isUnique);
  const otherIndexes = parsed.filter((p) => !p.isUnique && !p.isScopeOnly && !p.isIsDeleted);
  if (!scopeIndex) {
    scopeIndex = {
      indexName: tenantIndexName,
      entries: scopeFields.map((f) => ({ field: f, order: 'OrderByType.Asc' })),
      isUnique: false,
      isScopeOnly: true,
      isIsDeleted: false,
      isNew: true,
    };
  } else {
    scopeIndex.indexName = tenantIndexName;
    scopeIndex.entries = scopeFields.map((f) => ({ field: f, order: 'OrderByType.Asc' }));
  }
  if (!isDeletedIndex) {
    isDeletedIndex = {
      indexName: isDeletedIndexName,
      entries: [...scopeFields.map((f) => ({ field: f, order: 'OrderByType.Asc' })), { field: 'IsDeleted', order: 'OrderByType.Asc' }],
      isUnique: false,
      isScopeOnly: false,
      isIsDeleted: true,
      isNew: true,
    };
  } else {
    isDeletedIndex.indexName = isDeletedIndexName;
    isDeletedIndex.entries = [
      ...scopeFields.map((f) => ({ field: f, order: 'OrderByType.Asc' })),
      { field: 'IsDeleted', order: 'OrderByType.Asc' },
    ];
  }
  for (const u of uniqueIndexes) {
    u.entries = normalizeEntries(u.entries, entityBase);
    u.indexName = normalizeUniqueName(u.indexName, true);
  }
  for (const o of otherIndexes) {
    o.entries = normalizeEntries(o.entries, entityBase);
  }
  uniqueIndexes.sort((a, b) => a.indexName.localeCompare(b.indexName));
  otherIndexes.sort((a, b) => a.indexName.localeCompare(b.indexName));
  const ordered = [scopeIndex, isDeletedIndex, ...uniqueIndexes, ...otherIndexes];
  const indent = lines[indexLineNos[0]].match(/^\s*/)[0];
  const newIndexLines = ordered.map((p) => indent + buildIndexLine(p.indexName, p.entries, p.isUnique));
  const start = indexLineNos[0];
  const end = indexLineNos[indexLineNos.length - 1];
  const newLines = [...lines.slice(0, start), ...newIndexLines, ...lines.slice(end + 1)];
  const newContent = newLines.join('\n');
  return { content: newContent, changed: newContent !== content };
}

function main() {
  const dryRun = process.argv.includes('--dry-run');
  let fileCount = 0;
  let skipCount = 0;
  const files = [];
  for (const dir of TARGET_DIRS) {
    walkCsFiles(dir, files);
  }
  for (const file of files) {
    const original = fs.readFileSync(file, 'utf8');
    const entity = detectEntity(original);
    if (!entity) {
      continue;
    }
    if (entity.className === 'TaktTenant') {
      continue;
    }
    const shortPrefix = toShortPrefix(entity.className);
    const { content, changed, skipped } = normalizeFile(original, entity.entityBase, shortPrefix);
    if (skipped) {
      skipCount += 1;
      const rel = path.relative(path.join(__dirname, '../backend/src/Takt.Domain/Entities'), file).replace(/\\/g, '/');
      console.log(`[skip-unparsed] ${rel}`);
      continue;
    }
    if (!changed) {
      continue;
    }
    fileCount += 1;
    const rel = path.relative(path.join(__dirname, '../backend/src/Takt.Domain/Entities'), file).replace(/\\/g, '/');
    console.log(rel);
    if (!dryRun) {
      fs.writeFileSync(file, content, 'utf8');
    }
  }
  console.log(`\n${dryRun ? '[dry-run] ' : ''}共 ${fileCount} 个文件已规范化，${skipCount} 个跳过（含无法解析索引行）`);
}

main();
