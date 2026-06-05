/**
 * 全量审计：实体 SugarIndex 唯一约束 ↔ Application Service isUnique_ix_* 查重
 */
const fs = require('fs');
const path = require('path');

const ENT_ROOT = path.join(__dirname, '../backend/src/Takt.Domain/Entities');
const SVC_ROOT = path.join(__dirname, '../backend/src/Takt.Application/Services');

const SKIP_LOG = /Statistics\/Logging\/|ChangeLog\.cs$/i;
const SKIP_SCOPE = new Set(['TenantCode', 'CompanyCode', 'Id', 'IsDeleted', 'CreatedAt', 'UpdatedAt', 'CreatedBy', 'UpdatedBy', 'DeletedBy', 'DeletedAt', 'ApprovalStatus', 'InitiatorId', 'InitiatedAt', 'ApprovedBy', 'ApprovedAt']);

const SCOPE_BY_BASE = {
  TaktTenantEntityBase: new Set(['TenantCode']),
  TaktCompanyEntityBase: new Set(['TenantCode', 'CompanyCode']),
  TaktApprovalEntityBase: new Set(['TenantCode', 'CompanyCode']),
};

/** @param {string} dir @returns {string[]} */
function walk(dir, acc = []) {
  for (const f of fs.readdirSync(dir, { withFileTypes: true })) {
    const p = path.join(dir, f.name);
    if (f.isDirectory()) walk(p, acc);
    else if (f.name.endsWith('.cs')) acc.push(p);
  }
  return acc;
}

/** @param {string} content */
function detectBase(content) {
  const m = content.match(/class\s+Takt\w+\s*:\s*(Takt(?:Tenant|Company|Approval)EntityBase)\b/);
  return m ? m[1] : null;
}

/** @param {string} content */
function parseUniqueIndexes(content, entityBase) {
  const scope = entityBase ? SCOPE_BY_BASE[entityBase] || new Set(['TenantCode']) : new Set(['TenantCode']);
  const out = [];
  for (const line of content.split('\n')) {
    const t = line.trim();
    if (!t.startsWith('[SugarIndex(') || !/,\s*true\s*\)\]$/.test(t)) continue;
    const nm = t.match(/^\[SugarIndex\("([^"]+)"/);
    if (!nm) continue;
    const fields = [...t.matchAll(/nameof\((\w+)\)/g)].map((x) => x[1]);
    const biz = fields.filter((f) => !SKIP_SCOPE.has(f) && !scope.has(f));
    out.push({ name: nm[1], fields, biz, raw: t });
  }
  return out;
}

/** @param {string} svcContent */
function parseServiceUniques(svcContent) {
  const out = [];
  const re = /var\s+isUnique_(ix_[a-zA-Z0-9_]+)\s*=\s*await\s+_uniqueValidator\.IsUniqueAsync\([\s\S]*?x\s*=>\s*([\s\S]*?)\);/g;
  let m;
  while ((m = re.exec(svcContent)) !== null) {
    const idx = m[1];
    const expr = m[2].replace(/\s+/g, ' ').trim();
    const fields = [...expr.matchAll(/x\.(\w+)\s*==/g)].map((x) => x[1]);
    out.push({ idx, fields: [...new Set(fields)] });
  }
  return out;
}

function bizKey(fields) {
  return [...fields].sort().join('|');
}

const entityFiles = walk(ENT_ROOT).filter((f) => !SKIP_LOG.test(f.replace(/\\/g, '/')));
const entityMap = new Map(entityFiles.map((f) => [path.basename(f, '.cs'), f]));

const svcFiles = walk(SVC_ROOT).filter((f) => f.endsWith('Service.cs') && !f.includes('TaktServiceBase'));
const issues = [];

for (const ef of entityFiles) {
  const rel = path.relative(ENT_ROOT, ef).replace(/\\/g, '/');
  const content = fs.readFileSync(ef, 'utf8');
  const className = path.basename(ef, '.cs');
  const base = detectBase(content);
  const uniques = parseUniqueIndexes(content, base);
  for (const u of uniques) {
    if (!u.name.endsWith('_unique')) {
      issues.push({ type: 'bad-index-name', entity: className, file: rel, detail: u.name });
    }
    if (u.biz.length === 0) {
      issues.push({ type: 'empty-biz-unique', entity: className, file: rel, detail: u.name });
    }
  }
}

for (const sf of svcFiles) {
  const sc = fs.readFileSync(sf, 'utf8');
  const cm = sc.match(/class\s+(Takt\w+Service)\b/);
  if (!cm) continue;
  const entity = cm[1].replace(/Service$/, '');
  const entFile = entityMap.get(entity);
  if (!entFile) continue;
  const ec = fs.readFileSync(entFile, 'utf8');
  const base = detectBase(ec);
  const entUniques = parseUniqueIndexes(ec, base);
  const svcUniques = parseServiceUniques(sc);
  const entByName = new Map(entUniques.map((u) => [u.name, u]));
  const entByBiz = new Map(entUniques.map((u) => [bizKey(u.biz), u]));

  for (const su of svcUniques) {
    if (!entByName.has(su.idx)) {
      const match = entByBiz.get(bizKey(su.fields));
      issues.push({
        type: 'svc-index-missing',
        entity,
        file: path.relative(SVC_ROOT, sf).replace(/\\/g, '/'),
        detail: `${su.idx} fields=[${su.fields.join(',')}]${match ? ` (biz match ${match.name})` : ''}`,
      });
    } else {
      const eu = entByName.get(su.idx);
      if (bizKey(eu.biz) !== bizKey(su.fields)) {
        issues.push({
          type: 'svc-fields-mismatch',
          entity,
          file: path.relative(SVC_ROOT, sf).replace(/\\/g, '/'),
          detail: `${su.idx}: entity=[${eu.biz.join(',')}] svc=[${su.fields.join(',')}]`,
        });
      }
    }
  }

  for (const eu of entUniques) {
    const hasSvc = svcUniques.some((su) => su.idx === eu.name || bizKey(su.fields) === bizKey(eu.biz));
    if (!hasSvc && !['TaktDictData', 'TaktDictType', 'TaktUser', 'TaktOnline'].includes(entity)) {
      issues.push({
        type: 'entity-no-svc-check',
        entity,
        file: path.relative(ENT_ROOT, entFile).replace(/\\/g, '/'),
        detail: `${eu.name} biz=[${eu.biz.join(',')}]`,
      });
    }
  }
}

const grouped = {};
for (const i of issues) {
  grouped[i.type] = (grouped[i.type] || 0) + 1;
}
console.log('Issue counts:', grouped);
console.log(`Total: ${issues.length}\n`);
for (const i of issues) {
  console.log(`${i.type}\t${i.entity}\t${i.detail}\t${i.file || ''}`);
}
