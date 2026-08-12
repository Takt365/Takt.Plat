// ========================================
// 临时：按编译错误清单精确补 DTO 缺失属性（不从远程恢复）
// ========================================
'use strict';
const fs = require('fs');
const path = require('path');

const ROOT = path.resolve(__dirname, '..');
const DTO_ROOT = path.join(ROOT, 'backend/src/Takt.Application/Dtos');

/** @type {Array<[string, string, string]>} type, prop, csharpType */
const ADDS = [
  // SortOrder（Create/Import；Update 继承 Create）
  ['TaktFlowFormCreateDto', 'SortOrder', 'int'],
  ['TaktFlowFormImportDto', 'SortOrder', 'int'],
  ['TaktConfigurableFieldCreateDto', 'SortOrder', 'int'],
  ['TaktConfigurableFieldImportDto', 'SortOrder', 'int'],
  ['TaktConfigurableSourceCreateDto', 'SortOrder', 'int'],
  ['TaktConfigurableSourceImportDto', 'SortOrder', 'int'],
  ['TaktConfigurableSelectionCreateDto', 'SortOrder', 'int'],
  ['TaktConfigurableSelectionImportDto', 'SortOrder', 'int'],
  ['TaktConfigurableOrderByCreateDto', 'SortOrder', 'int'],
  ['TaktConfigurableOrderByImportDto', 'SortOrder', 'int'],
  ['TaktConfigurableGroupByCreateDto', 'SortOrder', 'int'],
  ['TaktConfigurableGroupByImportDto', 'SortOrder', 'int'],
  ['TaktConfigurableJoinCreateDto', 'SortOrder', 'int'],
  ['TaktConfigurableJoinImportDto', 'SortOrder', 'int'],
  // EcCode
  ['TaktEcExecCreateDto', 'EcCode', 'string'],
  ['TaktEcExecImportDto', 'EcCode', 'string'],
  ['TaktEcDeptViewImportDto', 'EcCode', 'string'],
  ['TaktEcDeptViewQueryDto', 'EcCode', 'string?'],
  ['TaktEcKanbanQueryDto', 'EcCode', 'string?'],
  ['TaktEcMonthlyTrendQueryDto', 'EcCode', 'string?'],
  ['TaktEcExecTransposedDto', 'EcCode', 'string'],
  ['TaktEcExecTransposedQueryDto', 'EcCode', 'string?'],
  ['TaktEcExecBatchTransposedStageDto', 'BatchCode', 'string'],
  // CultureCode
  ['TaktUserInfoResponseDto', 'CultureCode', 'string'],
  ['TaktEcGijutsuImportFromSourceDto', 'CultureCode', 'string?'],
  ['TaktEcKanbanQueryDto', 'CultureCode', 'string?'],
  // Trend
  ['TaktSalesPriceMonthlyTrendDto', 'MaterialDescription', 'string'],
  ['TaktSalesPriceMonthlyTrendDto', 'CurrencyCode', 'string'],
  ['TaktPurchasePriceMonthlyTrendDto', 'MaterialDescription', 'string'],
  ['TaktPurchasePriceMonthlyTrendDto', 'CurrencyCode', 'string'],
  ['TaktQualityCostTrendQueryDto', 'CurrencyCode', 'string?'],
  // Master-detail child Id on Create（级联保存用）
  ['TaktSerialOutboundItemCreateDto', 'SerialOutboundItemId', 'long'],
  ['TaktSerialInboundItemCreateDto', 'SerialInboundItemId', 'long'],
  ['TaktRoutingItemCreateDto', 'RoutingItemId', 'long'],
  ['TaktRoutingItemArgumentCreateDto', 'RoutingItemArgumentId', 'long'],
  ['TaktPcbaRepairDetailCreateDto', 'PcbaRepairDetailId', 'long'],
  ['TaktPcbaInspectionDetailCreateDto', 'PcbaInspectionDetailId', 'long'],
  ['TaktIqcDefectHandlingCreateDto', 'IqcDefectHandlingId', 'long'],
  ['TaktIpqcDefectHandlingCreateDto', 'IpqcDefectHandlingId', 'long'],
  ['TaktAssyDefectDetailCreateDto', 'AssyDefectDetailId', 'long'],
  ['TaktQualityIncidentItemCreateDto', 'QualityIncidentItemId', 'long'],
  ['TaktInspectionStandardItemCreateDto', 'InspectionStandardItemId', 'long'],
  // EcDeptView 扩展码
  ['TaktEcDeptViewDto', 'PurchaseOrderCode', 'string?'],
  ['TaktEcDeptViewDto', 'IqcOrderCode', 'string?'],
  ['TaktEcDeptViewDto', 'OutboundOrderCode', 'string?'],
  ['TaktEcDeptViewDto', 'SamplingCode', 'string?'],
  ['TaktEcDeptViewUpdateDto', 'PurchaseOrderCode', 'string?'],
  ['TaktEcDeptViewUpdateDto', 'IqcOrderCode', 'string?'],
  ['TaktEcDeptViewUpdateDto', 'OutboundOrderCode', 'string?'],
  ['TaktEcDeptViewUpdateDto', 'SamplingCode', 'string?'],
  ['TaktEcDeptViewImportDto', 'PurchaseOrderCode', 'string?'],
  ['TaktEcDeptViewImportDto', 'IqcOrderCode', 'string?'],
  ['TaktEcDeptViewImportDto', 'OutboundOrderCode', 'string?'],
  ['TaktEcDeptViewImportDto', 'SamplingCode', 'string?'],
];

/** CreateDto 子表集合改为 UpdateDto 列表（对齐本地服务赋值） */
const LIST_FIXES = [
  [
    'TaktAssyOutputCreateDto',
    'AssyOutputDetails',
    'List<TaktAssyOutputDetailCreateDto>?',
    'List<TaktAssyOutputDetailUpdateDto>?',
  ],
  [
    'TaktEcGijutsuCreateDto',
    'EcDetails',
    'List<TaktEcDetailCreateDto>?',
    'List<TaktEcDetailUpdateDto>?',
  ],
  [
    'TaktEcGijutsuCreateDto',
    'Attachments',
    'List<TaktEcAttachmentCreateDto>?',
    'List<TaktEcAttachmentUpdateDto>?',
  ],
];

function walk(dir, acc = []) {
  for (const e of fs.readdirSync(dir, { withFileTypes: true })) {
    const p = path.join(dir, e.name);
    if (e.isDirectory()) walk(p, acc);
    else if (e.name.endsWith('.cs')) acc.push(p);
  }
  return acc;
}

const fileCache = new Map();
function findFile(typeName) {
  if (fileCache.has(typeName)) return fileCache.get(typeName);
  for (const file of walk(DTO_ROOT)) {
    const text = fs.readFileSync(file, 'utf8');
    if (new RegExp(`public class ${typeName}\\b`).test(text)) {
      fileCache.set(typeName, file);
      return file;
    }
  }
  fileCache.set(typeName, null);
  return null;
}

function classSpan(text, typeName) {
  const start = text.search(new RegExp(`public class ${typeName}\\b`));
  if (start < 0) return null;
  const braceStart = text.indexOf('{', start);
  let depth = 0;
  for (let i = braceStart; i < text.length; i++) {
    if (text[i] === '{') depth++;
    else if (text[i] === '}') {
      depth--;
      if (depth === 0) return { start, braceStart, end: i };
    }
  }
  return null;
}

function hasProp(text, typeName, prop) {
  const span = classSpan(text, typeName);
  if (!span) return false;
  const body = text.slice(span.braceStart, span.end);
  return new RegExp(`public\\s+[\\w\\?\\<\\>\\[\\],\\s]+\\s+${prop}\\s*\\{`).test(body);
}

function declOf(prop, csharpType) {
  if (csharpType === 'string') return `public string ${prop} { get; set; } = string.Empty;`;
  if (csharpType === 'string?') return `public string? ${prop} { get; set; }`;
  if (csharpType === 'int') return `public int ${prop} { get; set; }`;
  if (csharpType === 'long') return `public long ${prop} { get; set; }`;
  if (csharpType.startsWith('List<')) return `public ${csharpType} ${prop} { get; set; }`;
  return `public ${csharpType} ${prop} { get; set; }`;
}

function insertProp(text, typeName, prop, csharpType) {
  if (hasProp(text, typeName, prop)) return { text, changed: false };
  const span = classSpan(text, typeName);
  if (!span) return { text, changed: false, missing: true };
  const snippet =
    `\n    /// <summary>\n    /// ${prop}\n    /// </summary>\n    ${declOf(prop, csharpType)}\n`;
  return { text: text.slice(0, span.end) + snippet + text.slice(span.end), changed: true };
}

function fixListType(text, typeName, prop, fromType, toType) {
  const span = classSpan(text, typeName);
  if (!span) return { text, changed: false };
  const body = text.slice(span.braceStart, span.end);
  const re = new RegExp(`public\\s+${fromType.replace(/[?<>]/g, '\\$&')}\\s+${prop}\\s*\\{`);
  if (!re.test(body)) return { text, changed: false };
  const next = text.slice(0, span.braceStart) + body.replace(
    new RegExp(`public\\s+${fromType.replace(/[?<>]/g, '\\$&')}\\s+${prop}`),
    `public ${toType} ${prop}`,
  ) + text.slice(span.end);
  return { text: next, changed: true };
}

let added = 0;
let listFixed = 0;
const dirty = new Map();

for (const [typeName, prop, csharpType] of ADDS) {
  const file = findFile(typeName);
  if (!file) {
    console.log('NO_FILE', typeName, prop);
    continue;
  }
  let text = dirty.get(file) || fs.readFileSync(file, 'utf8');
  const res = insertProp(text, typeName, prop, csharpType);
  if (res.missing) console.log('NO_CLASS', typeName);
  if (res.changed) {
    dirty.set(file, res.text);
    added++;
    console.log('ADD', typeName, prop, csharpType);
  }
}

for (const [typeName, prop, fromType, toType] of LIST_FIXES) {
  const file = findFile(typeName);
  if (!file) continue;
  let text = dirty.get(file) || fs.readFileSync(file, 'utf8');
  const res = fixListType(text, typeName, prop, fromType, toType);
  if (res.changed) {
    dirty.set(file, res.text);
    listFixed++;
    console.log('LIST', typeName, prop, toType);
  }
}

for (const [file, text] of dirty) {
  fs.writeFileSync(file, text, 'utf8');
}
console.log('added', added, 'listFixed', listFixed, 'files', dirty.size);
