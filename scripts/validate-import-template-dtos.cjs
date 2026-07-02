// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：scripts
// 文件名称：validate-import-template-dtos.cjs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：校验 ImportDto / TemplateDto 相对实体与 CreateDto 的字段缺失
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const fs = require('fs');
const path = require('path');
const { parseEntityClassHeaderFromCsContent, ENTITY_CLASS_HEADER_REGEX } = require('./generate-script-common.cjs');
const { isRbacJunctionEntity } = require('./generate-entity-exclusions.cjs');
const { getInverseCreateFields, getRbacParentNavigations } = require('./rbac-parent-config.cjs');

const BACKEND_ROOT = path.resolve(__dirname, '../backend/src');
const ENTITIES_ROOT = path.join(BACKEND_ROOT, 'Takt.Domain', 'Entities');
const DTOS_ROOT = path.join(BACKEND_ROOT, 'Takt.Application', 'Dtos');

const ENTITY_BASE_FIELDS = new Set([
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
]);

const CREATE_EXCLUDE_NAME_PATTERNS = [
  /^LastLogin/i,
  /^LoginCount$/i,
  /^LoginFail/i,
  /^LockedUntil$/i,
  /^SessionDuration$/i,
  /^Level$/i,
  /^DeptPath$/i,
  /^IsLeaf$/i,
];

const CREATE_EXCLUDE_PROPERTY_NAMES = new Set(['SortOrder']);

const NAVIGATION_REGION_MARKER = '导航属性区域';

function normalizeDocText(text) {
  return (text || '')
    .replace(/\/\/\/?/g, '')
    .split('\n')
    .map((l) => l.trim())
    .filter(Boolean)
    .join(' ')
    .trim();
}

function splitClassBodyByNavigationRegion(classBody) {
  const lines = classBody.split('\n');
  let markerLineIdx = -1;
  for (let i = 0; i < lines.length; i += 1) {
    if (lines[i].includes(NAVIGATION_REGION_MARKER)) {
      markerLineIdx = i;
      break;
    }
  }
  if (markerLineIdx === -1) {
    return { scalarBody: classBody, navigationBody: '' };
  }
  let navStartLine = markerLineIdx;
  while (navStartLine > 0 && /^\s*\/\/\s*=+/.test(lines[navStartLine - 1])) {
    navStartLine -= 1;
  }
  return {
    scalarBody: lines.slice(0, navStartLine).join('\n'),
    navigationBody: lines.slice(navStartLine).join('\n'),
  };
}

function parseScalarProperties(classBody, options = {}) {
  const { allowListTypes = false } = options;
  const properties = [];
  const propertyRegex =
    /\/\/\/\s*<summary>([\s\S]*?)<\/summary>[\s\S]*?public\s+((?:List<)?(?:Takt\w+|[a-zA-Z][\w]*)(?:>)?(?:\?)?)\s+(\w+)\s*\{\s*get;\s*set;/g;
  let match;
  while ((match = propertyRegex.exec(classBody)) !== null) {
    const summary = normalizeDocText(match[1]);
    const csharpType = match[2].trim();
    const name = match[3];
    if (!csharpType || csharpType.includes('Navigate')) {
      continue;
    }
    if (!allowListTypes && csharpType.startsWith('List<')) {
      continue;
    }
    if (!allowListTypes && /\[Navigate\s*\(/.test(match[0])) {
      continue;
    }
    if (summary.includes('SugarColumn') || summary.includes('public ')) {
      continue;
    }
    properties.push({ name, csharpType });
  }
  return properties;
}

function parseNavigateTypeFromSegment(segment) {
  const matches = [...segment.matchAll(/NavigateType\.(OneToMany|ManyToOne)/g)];
  if (!matches.length) {
    return null;
  }
  return matches[matches.length - 1][1];
}

function parseNavigationProperties(navigationBody) {
  if (!navigationBody.trim()) {
    return [];
  }
  const rawProps = parseScalarProperties(navigationBody, { allowListTypes: true });
  const navigations = [];
  rawProps.forEach((prop) => {
    const nameIdx = navigationBody.search(new RegExp(`\\b${prop.name}\\s*\\{`));
    const segment = nameIdx >= 0 ? navigationBody.slice(Math.max(0, nameIdx - 500), nameIdx) : '';
    const navigateType =
      parseNavigateTypeFromSegment(segment) ||
      (prop.csharpType.startsWith('List<') ? 'OneToMany' : 'ManyToOne');
    navigations.push({
      name: prop.name,
      navigateType,
      isCollection: navigateType === 'OneToMany',
    });
  });
  return navigations;
}

function stripNavigatePropertyBlocks(classBody) {
  const navAnchorRegex =
    /\[Navigate\(\s*NavigateType\.(?:OneToMany|ManyToOne)[\s\S]*?public\s+(?:List<)?(?:Takt\w+|[a-zA-Z][\w]*)(?:>)?(?:\?)?\s+\w+\s*\{\s*get;\s*set;/g;
  const matches = [...classBody.matchAll(navAnchorRegex)];
  if (!matches.length) {
    return classBody;
  }
  let result = classBody;
  for (let i = matches.length - 1; i >= 0; i -= 1) {
    const m = matches[i];
    const navEnd = m.index + m[0].length;
    const beforeNav = result.slice(0, m.index);
    const tail = beforeNav.slice(-800);
    const blocks = [...tail.matchAll(/\s*\/\/\/\s*<summary>[\s\S]*?<\/summary>\s*/g)];
    const lastBlock = blocks.length ? blocks[blocks.length - 1][0] : '';
    const removeStart = lastBlock ? beforeNav.length - lastBlock.length : m.index;
    result = result.slice(0, removeStart) + result.slice(navEnd);
  }
  return result;
}

function extractClassBody(content, openBraceIndex) {
  let depth = 1;
  let i = openBraceIndex + 1;
  while (i < content.length && depth > 0) {
    if (content[i] === '{') {
      depth += 1;
    } else if (content[i] === '}') {
      depth -= 1;
    }
    i += 1;
  }
  return content.slice(openBraceIndex + 1, i - 1);
}

function entityNamespaceToDirParts(entityNamespace) {
  const suffix = entityNamespace.replace(/^Takt\.Domain\.Entities\.?/, '');
  return suffix ? suffix.split('.').filter(Boolean) : [];
}

function parseEntityFile(filePath) {
  const content = fs.readFileSync(filePath, 'utf-8');
  const classHeader = parseEntityClassHeaderFromCsContent(content);
  if (!classHeader) {
    return null;
  }
  const className = classHeader.className;
  const entityBase = classHeader.entityBase;
  const classHeaderMatch = content.match(ENTITY_CLASS_HEADER_REGEX);
  const openBraceIndex = classHeaderMatch.index + classHeaderMatch[0].length - 1;
  const classBody = extractClassBody(content, openBraceIndex);
  const namespaceMatch = content.match(/namespace\s+([\w.]+);/);
  const entityNamespace = namespaceMatch ? namespaceMatch[1] : '';
  const dtoDirParts = entityNamespaceToDirParts(entityNamespace);
  const { scalarBody, navigationBody } = splitClassBodyByNavigationRegion(classBody);
  let navigationProperties = parseNavigationProperties(navigationBody);
  if (navigationProperties.length === 0) {
    navigationProperties = [];
  }
  const navNames = new Set(navigationProperties.map((n) => n.name));
  const scalarParseBody = navigationBody.trim() ? scalarBody : stripNavigatePropertyBlocks(classBody);
  const allScalar = parseScalarProperties(scalarParseBody);
  const properties = allScalar
    .filter((p) => !ENTITY_BASE_FIELDS.has(p.name))
    .filter((p) => !navNames.has(p.name));
  return {
    className,
    entityBase,
    entityNamespace,
    dtoDirParts,
    properties,
    navigationProperties,
    filePath,
  };
}

function isLogSuffixEntity(className) {
  return className.replace(/^Takt/, '').endsWith('Log');
}

function shouldGenerateTemplateImport(className) {
  const entityShort = className.replace(/^Takt/, '');
  return !isRbacJunctionEntity(entityShort) && !isLogSuffixEntity(className);
}

function getCreateProps(entity) {
  return entity.properties.filter((p) => {
    if (CREATE_EXCLUDE_PROPERTY_NAMES.has(p.name)) {
      return false;
    }
    if (CREATE_EXCLUDE_NAME_PATTERNS.some((re) => re.test(p.name))) {
      return false;
    }
    return true;
  });
}

function getExpectedExtraFields(entity) {
  const extras = new Set(['TenantCode', 'ExtField', 'Remark']);
  if (entity.entityBase === 'TaktCompanyEntityBase' || entity.entityBase === 'TaktApprovalEntityBase') {
    extras.add('CompanyCode');
  }
  const entityShort = entity.className.replace(/^Takt/, '');
  getInverseCreateFields(entityShort).forEach((f) => extras.add(f.prop));
  entity.navigationProperties
    .filter((nav) => nav.navigateType === 'OneToMany' || nav.isCollection)
    .forEach((nav) => {
      if (!isRbacJunctionEntity(nav.name.replace(/List$/, ''))) {
        extras.add(nav.name);
      }
    });
  getRbacParentNavigations(entityShort).forEach((spec) => {
    if (spec.includeOnCreate !== false && spec.assignFromParent && spec.createIdsProp) {
      extras.add(spec.createIdsProp);
    }
  });
  return extras;
}

function buildExpectedFields(entity, includeCompanyDefaultCulture) {
  const names = new Set(getCreateProps(entity).map((p) => p.name));
  getExpectedExtraFields(entity).forEach((n) => names.add(n));
  if (includeCompanyDefaultCulture) {
    if (entity.entityBase === 'TaktCompanyEntityBase' || entity.entityBase === 'TaktApprovalEntityBase') {
      names.add('CompanyDefaultCulture');
    }
  }
  return names;
}

function extractClassBlock(content, className) {
  const startRegex = new RegExp(`public\\s+(?:partial\\s+)?class\\s+${className}\\b`);
  const startMatch = startRegex.exec(content);
  if (!startMatch) {
    return '';
  }
  const braceStart = content.indexOf('{', startMatch.index);
  if (braceStart < 0) {
    return '';
  }
  let depth = 0;
  for (let i = braceStart; i < content.length; i += 1) {
    if (content[i] === '{') {
      depth += 1;
    } else if (content[i] === '}') {
      depth -= 1;
      if (depth === 0) {
        return content.slice(braceStart + 1, i);
      }
    }
  }
  return '';
}

function extractDtoPropertyNames(dtoContent, className) {
  const names = new Set();
  const startRegex = new RegExp(`public\\s+(?:partial\\s+)?class\\s+${className}\\b[^\\{]*(?::\\s*(\\w+))?`);
  const startMatch = startRegex.exec(dtoContent);
  if (!startMatch) {
    return names;
  }
  const baseClass = startMatch[1];
  if (baseClass) {
    for (const name of extractDtoPropertyNames(dtoContent, baseClass)) {
      names.add(name);
    }
  }
  const block = extractClassBlock(dtoContent, className);
  const propRegex = /public\s+[\w?<>,\s]+\s+(\w+)\s*\{/g;
  let m;
  while ((m = propRegex.exec(block)) !== null) {
    names.add(m[1]);
  }
  return names;
}

function diffSets(expected, actual) {
  const missing = [...expected].filter((n) => !actual.has(n)).sort();
  const extra = [...actual].filter((n) => !expected.has(n)).sort();
  return { missing, extra };
}

function isIntentionalEntityOnlyField(name) {
  if (ENTITY_BASE_FIELDS.has(name)) {
    return true;
  }
  if (CREATE_EXCLUDE_PROPERTY_NAMES.has(name)) {
    return true;
  }
  if (CREATE_EXCLUDE_NAME_PATTERNS.some((re) => re.test(name))) {
    return true;
  }
  return false;
}

function scanEntities() {
  const results = [];
  function walk(dir) {
    fs.readdirSync(dir, { withFileTypes: true }).forEach((entry) => {
      const fullPath = path.join(dir, entry.name);
      if (entry.isDirectory()) {
        walk(fullPath);
        return;
      }
      if (!entry.name.startsWith('Takt') || !entry.name.endsWith('.cs')) {
        return;
      }
      if (entry.name === 'TaktCompanyEntityBase.cs') {
        return;
      }
      const entityShort = entry.name.replace(/^Takt/, '').replace(/\.cs$/, '');
      if (isRbacJunctionEntity(entityShort)) {
        return;
      }
      const parsed = parseEntityFile(fullPath);
      if (parsed) {
        results.push(parsed);
      }
    });
  }
  walk(ENTITIES_ROOT);
  return results;
}

/** CreateDto 标量业务字段（排除导航集合、RBAC、CompanyDefaultCulture） */
function getCreateScalarBusinessFields(createActual, entity) {
  const navNames = new Set((entity.navigationProperties || []).map((n) => n.name));
  const rbacProps = new Set(
    getInverseCreateFields(entity.className.replace(/^Takt/, '')).map((f) => f.prop),
  );
  getRbacParentNavigations(entity.className.replace(/^Takt/, '')).forEach((spec) => {
    if (spec.createIdsProp) {
      rbacProps.add(spec.createIdsProp);
    }
  });
  const skip = new Set(['TenantCode', 'CompanyCode', 'CompanyDefaultCulture', 'ExtField', 'Remark', ...navNames, ...rbacProps]);
  return new Set([...createActual].filter((n) => !skip.has(n)));
}

function main() {
  const entities = scanEntities();
  const missingDtoFiles = [];
  const entityMissingRows = [];
  const createMisalignedRows = [];
  const importTemplateDiffRows = [];
  let checked = 0;
  let skippedNoDto = 0;
  let skippedNoImport = 0;
  let alignedCount = 0;

  entities.forEach((entity) => {
    if (!shouldGenerateTemplateImport(entity.className)) {
      return;
    }
    const entityShort = entity.className.replace(/^Takt/, '');
    const dtoFile = path.join(DTOS_ROOT, ...entity.dtoDirParts, `Takt${entityShort}Dtos.cs`);
    if (!fs.existsSync(dtoFile)) {
      skippedNoDto += 1;
      missingDtoFiles.push({ entity: entityShort, path: dtoFile });
      return;
    }
    const dtoContent = fs.readFileSync(dtoFile, 'utf-8');
    const templateClass = `Takt${entityShort}TemplateDto`;
    const importClass = `Takt${entityShort}ImportDto`;
    const createClass = `Takt${entityShort}CreateDto`;
    if (!dtoContent.includes(`class ${templateClass}`) || !dtoContent.includes(`class ${importClass}`)) {
      skippedNoImport += 1;
      return;
    }
    checked += 1;
    const templateActual = extractDtoPropertyNames(dtoContent, templateClass);
    const importActual = extractDtoPropertyNames(dtoContent, importClass);
    const createActual = extractDtoPropertyNames(dtoContent, createClass);
    const createScalars = getCreateScalarBusinessFields(createActual, entity);
    const entityImportable = new Set(
      entity.properties.filter((p) => !isIntentionalEntityOnlyField(p.name)).map((p) => p.name),
    );
    const missingTemplateVsEntity = [...entityImportable].filter((n) => !templateActual.has(n)).sort();
    const missingImportVsEntity = [...entityImportable].filter((n) => !importActual.has(n)).sort();
    const missingTemplateVsCreate = [...createScalars].filter((n) => !templateActual.has(n)).sort();
    const missingImportVsCreate = [...createScalars].filter((n) => !importActual.has(n)).sort();
    const importOnly = [...importActual].filter((n) => !templateActual.has(n) && n !== 'CompanyDefaultCulture').sort();
    const templateOnly = [...templateActual].filter((n) => !importActual.has(n)).sort();
    const navNames = new Set((entity.navigationProperties || []).map((n) => n.name));
    const templateExtraNav = [...templateActual].filter((n) => navNames.has(n)).sort();
    const importExtraNav = [...importActual].filter((n) => navNames.has(n)).sort();
    const hasEntityGap = missingTemplateVsEntity.length || missingImportVsEntity.length;
    const hasCreateGap = missingTemplateVsCreate.length || missingImportVsCreate.length;
    const hasImportTemplateGap = importOnly.length || templateOnly.length;
    const hasNavLeak = templateExtraNav.length || importExtraNav.length;
    if (!hasEntityGap && !hasCreateGap && !hasImportTemplateGap && !hasNavLeak) {
      alignedCount += 1;
      return;
    }
    if (hasEntityGap) {
      entityMissingRows.push({
        entity: entityShort,
        templateMissing: missingTemplateVsEntity,
        importMissing: missingImportVsEntity,
      });
    }
    if (hasCreateGap) {
      createMisalignedRows.push({
        entity: entityShort,
        templateMissing: missingTemplateVsCreate,
        importMissing: missingImportVsCreate,
      });
    }
    if (hasImportTemplateGap) {
      importTemplateDiffRows.push({
        entity: entityShort,
        importOnly,
        templateOnly,
      });
    }
    if (hasNavLeak) {
      importTemplateDiffRows.push({
        entity: entityShort,
        templateExtraNav,
        importExtraNav,
      });
    }
  });

  console.log('=== ImportDto / TemplateDto 字段校验报告 ===\n');
  console.log(`扫描实体: ${entities.length}`);
  console.log(`应含 Import/Template: ${checked}（无 DTO 文件 ${skippedNoDto}，无 Import 类 ${skippedNoImport}）`);
  console.log(`字段完全对齐: ${alignedCount}`);
  console.log(`相对实体缺字段: ${entityMissingRows.length}`);
  console.log(`相对 CreateDto 缺字段: ${createMisalignedRows.length}`);
  console.log(`Import/Template 互差或含导航: ${importTemplateDiffRows.length}\n`);

  console.log('判定规则（来源: generate-dtos-from-entity.cjs）:');
  console.log('- 实体对比: 实体标量字段 − 基类字段(Id/TenantCode/Remark/审计/审批等) − SortOrder/Level/DeptPath/IsLeaf/LastLogin*');
  console.log('- CreateDto 对比: CreateDto 标量业务字段（不含 TenantCode/CompanyCode/CompanyDefaultCulture/ExtField/Remark/导航/RBAC）');
  console.log('- TemplateDto 不含 CompanyDefaultCulture 为规范行为\n');

  if (entityMissingRows.length) {
    console.log('--- 【A】相对实体缺少的业务字段 ---');
    entityMissingRows.forEach((row) => {
      console.log(`\n[${row.entity}]`);
      if (row.templateMissing.length) {
        console.log(`  TemplateDto 缺: ${row.templateMissing.join(', ')}`);
      }
      if (row.importMissing.length) {
        console.log(`  ImportDto 缺: ${row.importMissing.join(', ')}`);
      }
    });
  }

  if (createMisalignedRows.length) {
    console.log('\n--- 【B】CreateDto 有而 Import/Template 缺（同 A 时重复列出）---');
    createMisalignedRows.forEach((row) => {
      console.log(`\n[${row.entity}]`);
      if (row.templateMissing.length) {
        console.log(`  TemplateDto 缺: ${row.templateMissing.join(', ')}`);
      }
      if (row.importMissing.length) {
        console.log(`  ImportDto 缺: ${row.importMissing.join(', ')}`);
      }
    });
  }

  if (importTemplateDiffRows.length) {
    console.log('\n--- 【C】ImportDto 与 TemplateDto 不一致 / 误含导航 ---');
    importTemplateDiffRows.forEach((row) => {
      console.log(`\n[${row.entity}]`);
      if (row.importOnly?.length) {
        console.log(`  仅 ImportDto 有: ${row.importOnly.join(', ')}`);
      }
      if (row.templateOnly?.length) {
        console.log(`  仅 TemplateDto 有: ${row.templateOnly.join(', ')}`);
      }
      if (row.templateExtraNav?.length) {
        console.log(`  TemplateDto 误含导航: ${row.templateExtraNav.join(', ')}`);
      }
      if (row.importExtraNav?.length) {
        console.log(`  ImportDto 误含导航: ${row.importExtraNav.join(', ')}`);
      }
    });
  }

  if (missingDtoFiles.length) {
    console.log('\n--- 【D】缺少 Dtos 文件 ---');
    missingDtoFiles.forEach((item) => console.log(`  ${item.entity} → ${item.path}`));
  }

  if (!entityMissingRows.length && !createMisalignedRows.length && !importTemplateDiffRows.length && !missingDtoFiles.length) {
    console.log('✅ 全部 ImportDto / TemplateDto 与实体及 CreateDto 一致。');
  }

  const outJson = path.join(__dirname, '_validate-import-template-report.json');
  fs.writeFileSync(
    outJson,
    JSON.stringify(
      {
        scannedEntities: entities.length,
        checked,
        alignedCount,
        entityMissingRows,
        createMisalignedRows,
        importTemplateDiffRows,
        missingDtoFiles,
      },
      null,
      2,
    ),
    'utf-8',
  );
  console.log(`\n详细 JSON: ${outJson}`);

  process.exit(entityMissingRows.length || createMisalignedRows.length || importTemplateDiffRows.length ? 1 : 0);
}

main();
