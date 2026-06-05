/**
 * 规范化实体 SugarIndex：
 * 1. 字段顺序：TenantCode → CompanyCode（公司/审批级）→ 其它业务字段 → true/false
 * 2. 唯一索引（true）名称必须以 _unique 结尾
 * 3. 租户级仅含 TenantCode 的普通索引：名称以 _tenant 结尾（非 _tenant_code）
 * 4. 公司/审批级仅含 TenantCode+CompanyCode 的普通索引：名称以 _tenant_company 结尾
 * 5. 类上索引声明顺序：隔离普通索引 → 唯一索引 → 其它普通索引
 */
const fs = require('fs');
const path = require('path');

const ENTITIES_ROOT = path.join(__dirname, '../backend/src/Takt.Domain/Entities');

const SCOPE_BY_BASE = {
  TaktTenantEntityBase: ['TenantCode'],
  TaktCompanyEntityBase: ['TenantCode', 'CompanyCode'],
  TaktApprovalEntityBase: ['TenantCode', 'CompanyCode'],
};

/** @returns {string[]} */
function walkCsFiles(dir, acc = []) {
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
 * @returns {'TaktTenantEntityBase'|'TaktCompanyEntityBase'|'TaktApprovalEntityBase'|null}
 */
function detectEntityBase(content) {
  const m = content.match(/public\s+(?:partial\s+)?class\s+\w+\s*:\s*(Takt(?:Tenant|Company|Approval)EntityBase)\b/);
  return m ? m[1] : null;
}

/**
 * @param {string} line
 * @returns {{ indexName: string, entries: Array<{ field: string, order: string }>, isUnique: boolean, hasExplicitFlag: boolean } | null}
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
  const hasExplicitFlag = /,\s*(true|false)\s*$/.test(afterName);
  const isUnique = /,\s*true\s*$/.test(afterName);
  const body = hasExplicitFlag ? afterName.replace(/,\s*(true|false)\s*$/, '').trim() : afterName;
  const entries = [];
  const partRegex = /nameof\((\w+)\)\s*,\s*(OrderByType\.\w+)/g;
  let pm;
  while ((pm = partRegex.exec(body)) !== null) {
    entries.push({ field: pm[1], order: pm[2] });
  }
  if (entries.length === 0) {
    return null;
  }
  return { indexName, entries, isUnique, hasExplicitFlag };
}

/**
 * @param {Array<{ field: string, order: string }>} entries
 * @param {'TaktTenantEntityBase'|'TaktCompanyEntityBase'|'TaktApprovalEntityBase'} entityBase
 */
function isScopeOnlyIndex(entries, entityBase) {
  const scopeFields = SCOPE_BY_BASE[entityBase];
  if (entries.length !== scopeFields.length) {
    return false;
  }
  return scopeFields.every((field, index) => entries[index].field === field);
}

/**
 * @param {Array<{ field: string, order: string }>} entries
 * @param {'TaktTenantEntityBase'|'TaktCompanyEntityBase'|'TaktApprovalEntityBase'} entityBase
 */
function normalizeEntries(entries, entityBase) {
  const scopeFields = SCOPE_BY_BASE[entityBase];
  const entryMap = new Map(entries.map((e) => [e.field, e]));
  const scopeEntries = scopeFields.map((field) => {
    if (entryMap.has(field)) {
      return entryMap.get(field);
    }
    return { field, order: 'OrderByType.Asc' };
  });
  const others = entries.filter((e) => !scopeFields.includes(e.field));
  return [...scopeEntries, ...others];
}

/**
 * @param {string} indexName
 * @param {boolean} isUnique
 * @param {Array<{ field: string, order: string }>} entries
 * @param {'TaktTenantEntityBase'|'TaktCompanyEntityBase'|'TaktApprovalEntityBase'} entityBase
 */
function normalizeIndexName(indexName, isUnique, entries, entityBase) {
  if (isUnique) {
    if (indexName.endsWith('_unique')) {
      return indexName;
    }
    return `${indexName}_unique`;
  }
  if (isScopeOnlyIndex(entries, entityBase)) {
    if (entityBase === 'TaktTenantEntityBase') {
      if (indexName.endsWith('_tenant_code')) {
        return indexName.slice(0, -'_tenant_code'.length) + '_tenant';
      }
      if (!indexName.endsWith('_tenant')) {
        return indexName.replace(/_tenant_code$/, '_tenant');
      }
    }
    if (entityBase === 'TaktCompanyEntityBase' || entityBase === 'TaktApprovalEntityBase') {
      if (indexName.endsWith('_tenant_code')) {
        return indexName.slice(0, -'_tenant_code'.length) + '_tenant_company';
      }
      if (!indexName.endsWith('_tenant_company') && indexName.endsWith('_tenant')) {
        return `${indexName}_company`;
      }
    }
  }
  return indexName;
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
 * @param {string} line
 * @param {'TaktTenantEntityBase'|'TaktCompanyEntityBase'|'TaktApprovalEntityBase'} entityBase
 */
function fixIndexLine(line, entityBase) {
  const parsed = parseIndexLine(line);
  if (!parsed) {
    return { line, changed: false, sortKey: null };
  }
  const normalizedEntries = normalizeEntries(parsed.entries, entityBase);
  const normalizedName = normalizeIndexName(parsed.indexName, parsed.isUnique, normalizedEntries, entityBase);
  const newLine = buildIndexLine(normalizedName, normalizedEntries, parsed.isUnique);
  const indent = line.match(/^\s*/)[0];
  const result = indent + newLine;
  const scopeOnly = isScopeOnlyIndex(normalizedEntries, entityBase);
  let sortKey;
  if (parsed.isUnique) {
    sortKey = `1:${normalizedName}`;
  } else if (scopeOnly) {
    sortKey = `0:${normalizedName}`;
  } else {
    sortKey = `2:${normalizedName}`;
  }
  const changed = result.trim() !== line.trim();
  return { line: result, changed, sortKey };
}

/**
 * @param {string} content
 * @param {'TaktTenantEntityBase'|'TaktCompanyEntityBase'|'TaktApprovalEntityBase'} entityBase
 */
function fixFileIndexes(content, entityBase) {
  const lines = content.split('\n');
  const indexLineNumbers = [];
  for (let i = 0; i < lines.length; i++) {
    if (lines[i].trim().startsWith('[SugarIndex(')) {
      indexLineNumbers.push(i);
    }
  }
  if (indexLineNumbers.length === 0) {
    return { content, changed: 0 };
  }
  const fixedIndexes = [];
  let changed = 0;
  for (const lineNo of indexLineNumbers) {
    const { line, changed: lineChanged, sortKey } = fixIndexLine(lines[lineNo], entityBase);
    if (lineChanged) {
      changed += 1;
    }
    fixedIndexes.push({ lineNo, line, sortKey });
  }
  const sorted = [...fixedIndexes].sort((a, b) => a.sortKey.localeCompare(b.sortKey));
  const orderChanged = sorted.some((item, index) => item.lineNo !== fixedIndexes[index].lineNo);
  if (orderChanged) {
    changed += 1;
  }
  for (let i = 0; i < fixedIndexes.length; i++) {
    lines[sorted[i].lineNo] = sorted[i].line;
  }
  return { content: lines.join('\n'), changed };
}

function main() {
  const dryRun = process.argv.includes('--dry-run');
  const files = walkCsFiles(ENTITIES_ROOT);
  let fileCount = 0;
  let indexCount = 0;
  for (const file of files) {
    const original = fs.readFileSync(file, 'utf8');
    const entityBase = detectEntityBase(original);
    if (!entityBase) {
      continue;
    }
    const { content, changed } = fixFileIndexes(original, entityBase);
    if (changed === 0) {
      continue;
    }
    fileCount += 1;
    indexCount += changed;
    const rel = path.relative(ENTITIES_ROOT, file).replace(/\\/g, '/');
    console.log(`${rel}: ${changed} 处`);
    if (!dryRun) {
      fs.writeFileSync(file, content, 'utf8');
    }
  }
  console.log(`\n${dryRun ? '[dry-run] ' : ''}共 ${fileCount} 个文件、${indexCount} 处索引已规范化`);
}

module.exports = {
  SCOPE_BY_BASE,
  parseIndexLine,
  normalizeEntries,
  normalizeIndexName,
  buildIndexLine,
  fixIndexLine,
  fixFileIndexes,
};

if (require.main === module) {
  main();
}
