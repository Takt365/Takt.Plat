'use strict';

/**
 * 审计 Domain 实体唯一索引隔离前缀（方案1：合理分区，非基类全字段硬塞）
 *
 * 规则：
 * - TaktTenantCoreEntityBase：须含 TenantCode
 * - TaktTenantCultureEntityBase：
 *   - i18n 内容表（默认）：须含 TenantCode + CultureCode
 *   - 登录/主档码表白名单（User、Plant）：仅须 TenantCode（禁止要求 Culture）
 * - TaktTenantEntityBase：须含 TenantCode；若有 CompanyCode 业务列则须含（公司主档）
 * - TaktCompanyEntityBase / TaktApprovalEntityBase：
 *   - 须含 TenantCode + CompanyCode
 *   - 禁止将 CultureCode 列为「缺失必填」（Culture 不作唯一分区）
 *   - 若唯一索引已含 PlantCode，或实体名/业务属工厂维，PlantCode 须在唯一索引中
 * - 无 [SugarIndex(..., true)]：仅列出，不判违规
 *
 * 用法: node scripts/audit-entity-unique-isolation.cjs
 * 退出码: 有违规=1，否则=0
 */

const fs = require('fs');
const path = require('path');

const ENTITIES_ROOT = path.join(__dirname, '../backend/src/Takt.Domain/Entities');

/** 登录/主档：Culture 不进唯一键 */
const CULTURE_BASE_NO_CULTURE_IN_UNIQUE = new Set(['TaktUser', 'TaktPlant']);

/** 明确工厂维业务（唯一键应含 PlantCode），即使用法上可能暂时缺 Plant */
const PLANT_DIMENSION_ENTITY_HINTS = [
  /^TaktMaterialPlant$/,
  /^TaktMaterialMovingPrice$/,
  /^TaktBomMaterialCost/,
  /^TaktBillOfMaterial$/,
  /^TaktRouting$/,
  /^TaktWorkCenter$/,
  /^TaktProductionOrder$/,
  /^TaktPlannedOrder$/,
  /^TaktApsOrder$/,
  /^TaktApsSchedule$/,
  /^TaktEquipment$/,
  /^TaktStorageLocation$/,
  /^TaktWarehouse$/,
  /LaborHour$/,
  /DefectGroup$/,
  /ChangeoverMatrix$/,
  /StandardOperation/,
  /ProductionDispatch$/,
  /ProductionTeam$/,
  /MasterProductionSchedule$/,
  /MasterDemandSchedule$/,
  /MaterialRequirementsPlanning$/,
  /PurchasePlan$/,
  /ProductionPlan$/,
  /SalesForecast$/,
  /QualityAssurance$/,
  /QualityIncident$/,
  /IqcOrder$/,
  /IpqcOrder$/,
  /FqcOrder$/,
  /^TaktCustomerComplaint$/,
  /^TaktCustomerComplaintHandling$/,
  /^TaktCustomerSatisfactionSurvey$/,
  /^TaktCustomerService(Contract|Order|Request|Ticket)$/,
  /SerialInbound$/,
  /SerialOutbound$/,
  /SerialSummary$/,
  /StandardWageRate$/,
  /BalanceSheet$/,
  /BudgetActual$/,
  /ProfitLoss$/,
  /PurchaseSalesInventory$/,
  /Calendar$/,
  /^TaktAssyOutput$/,
  /^TaktPcbaOutput$/,
];

const BASE_PREFIX = {
  TaktTenantCoreEntityBase: ['TenantCode'],
  TaktTenantCultureEntityBase: ['TenantCode', 'CultureCode'],
  TaktTenantEntityBase: ['TenantCode'],
  TaktTenantPlantEntityBase: ['TenantCode', 'RelatedPlant'],
  TaktCompanyEntityBase: ['TenantCode', 'CompanyCode'],
  TaktApprovalEntityBase: ['TenantCode', 'CompanyCode'],
};

/**
 * @param {string} dir
 * @returns {string[]}
 */
function walkCsFiles(dir) {
  const out = [];
  if (!fs.existsSync(dir)) return out;
  for (const name of fs.readdirSync(dir)) {
    const full = path.join(dir, name);
    const st = fs.statSync(full);
    if (st.isDirectory()) out.push(...walkCsFiles(full));
    else if (name.endsWith('.cs') && !name.includes('Base')) out.push(full);
  }
  return out;
}

/**
 * @param {string} content
 * @returns {string|null}
 */
function parseBaseClass(content) {
  const m = content.match(
    /:\s*(Takt(?:Company|Approval|TenantCore|TenantCulture|TenantPlant|Tenant)EntityBase)\b/,
  );
  return m ? m[1] : null;
}

/**
 * @param {string} content
 * @returns {string|null}
 */
function parseEntityName(content) {
  const m = content.match(/public\s+class\s+(Takt\w+)\s*:/);
  return m ? m[1] : null;
}

/**
 * 解析所有唯一 SugarIndex 的列名列表
 * @param {string} content
 * @returns {{ name: string, cols: string[] }[]}
 */
function parseUniqueIndexes(content) {
  const results = [];
  const attrRe = /\[SugarIndex\s*\(/g;
  let m;
  while ((m = attrRe.exec(content)) !== null) {
    let i = m.index + m[0].length;
    let depth = 1;
    let end = i;
    for (; end < content.length; end++) {
      const ch = content[end];
      if (ch === '(') depth++;
      else if (ch === ')') {
        depth--;
        if (depth === 0) break;
      }
    }
    const inner = content.slice(i, end);
    if (!/,\s*true\s*$/.test(inner.replace(/\s+/g, ' ')) && !/,\s*true\s*\)?\s*$/.test(inner)) {
      // 多行数组形式：最后一个参数 true 在括号外层已截断；检查 inner 末尾
      if (!/true\s*$/.test(inner.trim())) continue;
    }
    const nameMatch = inner.match(/^"([^"]+)"/);
    const name = nameMatch ? nameMatch[1] : '(unnamed)';
    const cols = [];
    const nameofRe = /nameof\s*\(\s*(\w+)\s*\)/g;
    let nm;
    while ((nm = nameofRe.exec(inner)) !== null) {
      cols.push(nm[1]);
    }
    if (cols.length === 0) continue;
    // 非唯一索引：末参数 false
    const tail = inner.trim().replace(/\s+/g, ' ');
    if (/, false\s*$/.test(tail) || /false\s*$/.test(tail) && !/true\s*$/.test(tail)) continue;
    if (!/\btrue\b/.test(tail)) continue;
    if (/, false\b/.test(tail) && !/, true\b/.test(tail)) continue;
    // 若同时出现 true/false，以最后一个布尔为准
    const bools = [...tail.matchAll(/\b(true|false)\b/g)].map((x) => x[1]);
    if (bools.length && bools[bools.length - 1] !== 'true') continue;
    results.push({ name, cols });
  }
  return results;
}

/**
 * @param {string} entityName
 * @returns {boolean}
 */
function isPlantDimensionEntity(entityName) {
  return PLANT_DIMENSION_ENTITY_HINTS.some((re) => re.test(entityName));
}

/**
 * @param {object} args
 * @returns {{ ok: boolean, missing: string[], notes: string[] }}
 */
function evaluateEntity({ entityName, base, uniqueIndexes }) {
  const notes = [];
  if (!uniqueIndexes.length) {
    return { ok: true, missing: [], notes: ['no_unique_index'], skipped: true };
  }

  const missing = new Set();
  let required = [...(BASE_PREFIX[base] || [])];

  if (base === 'TaktTenantCultureEntityBase') {
    if (CULTURE_BASE_NO_CULTURE_IN_UNIQUE.has(entityName)) {
      required = ['TenantCode'];
      notes.push('culture_base_master_no_culture_in_unique');
    }
  }

  if (base === 'TaktTenantEntityBase' && entityName === 'TaktCompany') {
    required = ['TenantCode', 'CompanyCode'];
    notes.push('company_master');
  }

  for (const ix of uniqueIndexes) {
    const colSet = new Set(ix.cols);
    for (const r of required) {
      if (!colSet.has(r)) missing.add(`${ix.name}:${r}`);
    }
    // Culture 不得作为 Company/Approval「必填缺失」；若误含也不当违规
    if (
      (base === 'TaktCompanyEntityBase' || base === 'TaktApprovalEntityBase') &&
      colSet.has('CultureCode')
    ) {
      notes.push(`${ix.name}:has_culture_in_unique_redundant`);
    }
    // 工厂维：已含 Plant 或名单命中 → 必须含 PlantCode
    const needsPlant =
      colSet.has('PlantCode') ||
      isPlantDimensionEntity(entityName);
    if (
      (base === 'TaktCompanyEntityBase' || base === 'TaktApprovalEntityBase') &&
      needsPlant &&
      !colSet.has('PlantCode')
    ) {
      missing.add(`${ix.name}:PlantCode`);
    }
    // 名单工厂维但索引无 Plant：上面已记
    // 非工厂维却缺 Plant：合规
  }

  // 汇总缺失字段名（去索引名）
  const missingFields = [...new Set([...missing].map((x) => x.split(':')[1]))];
  return {
    ok: missingFields.length === 0,
    missing: missingFields,
    missingDetail: [...missing],
    notes,
    skipped: false,
  };
}

function main() {
  const files = walkCsFiles(ENTITIES_ROOT);
  const byBase = {};
  const violations = [];
  const noUnique = [];
  const cultureRedundant = [];

  for (const file of files) {
    const content = fs.readFileSync(file, 'utf8');
    const base = parseBaseClass(content);
    if (!base || !BASE_PREFIX[base]) continue;
    const entityName = parseEntityName(content);
    if (!entityName) continue;
    const uniqueIndexes = parseUniqueIndexes(content);
    const rel = path.relative(ENTITIES_ROOT, file).replace(/\\/g, '/');

    if (!byBase[base]) {
      byBase[base] = { total: 0, withUnique: 0, violate: 0, ok: 0, noUnique: 0 };
    }
    byBase[base].total++;

    const ev = evaluateEntity({ entityName, base, uniqueIndexes });
    if (!uniqueIndexes.length) {
      byBase[base].noUnique++;
      noUnique.push({ entityName, base, file: rel });
      continue;
    }
    byBase[base].withUnique++;
    if (ev.notes.some((n) => n.includes('has_culture_in_unique_redundant'))) {
      cultureRedundant.push({ entityName, base, file: rel });
    }
    if (ev.ok) {
      byBase[base].ok++;
    } else {
      byBase[base].violate++;
      violations.push({
        entityName,
        base,
        file: rel,
        missing: ev.missing,
        indexes: uniqueIndexes.map((x) => ({ name: x.name, cols: x.cols })),
      });
    }
  }

  console.log('=== Unique isolation audit (scheme 1) ===\n');
  console.log('By base:');
  for (const [base, s] of Object.entries(byBase)) {
    console.log(
      `  ${base}: total=${s.total} withUnique=${s.withUnique} ok=${s.ok} violate=${s.violate} noUnique=${s.noUnique}`,
    );
  }
  console.log(`\nViolations: ${violations.length}`);
  for (const v of violations.slice(0, 80)) {
    console.log(
      `  - ${v.entityName} [${v.base}] missing=[${v.missing.join(',')}] @ ${v.file}`,
    );
    for (const ix of v.indexes) {
      console.log(`      ${ix.name}: ${ix.cols.join('+')}`);
    }
  }
  if (violations.length > 80) {
    console.log(`  ... and ${violations.length - 80} more`);
  }

  console.log(`\nNo unique index (listed only): ${noUnique.length}`);
  for (const n of noUnique.slice(0, 40)) {
    console.log(`  - ${n.entityName} [${n.base}] @ ${n.file}`);
  }
  if (noUnique.length > 40) {
    console.log(`  ... and ${noUnique.length - 40} more`);
  }

  console.log(
    `\nCompany/Approval unique indexes that include CultureCode (redundant, not fail): ${cultureRedundant.length}`,
  );

  const outDir = path.join(__dirname, 'out');
  if (!fs.existsSync(outDir)) fs.mkdirSync(outDir, { recursive: true });
  const reportPath = path.join(outDir, 'entity-unique-isolation-audit.json');
  fs.writeFileSync(
    reportPath,
    JSON.stringify({ byBase, violations, noUnique, cultureRedundant }, null, 2),
    'utf8',
  );
  console.log(`\nReport: ${reportPath}`);

  process.exit(violations.length > 0 ? 1 : 0);
}

main();
