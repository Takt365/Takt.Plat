// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：scripts
// 文件名称：sync-import-template-from-create.cjs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：从 CreateDto 批量同步补齐 TemplateDto / ImportDto（按「导入 DTO」区段替换，保留其它 DTO）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const fs = require('fs');
const path = require('path');

const DTOS_ROOT = path.join(path.resolve(__dirname, '../backend/src'), 'Takt.Application', 'Dtos');

const IMPORT_SECTION_START =
  /\/\/ ========================================\r?\n\/\/ 导入 DTO\r?\n\/\/ ========================================/;
const EXPORT_SECTION_START =
  /\/\/ ========================================\r?\n\/\/ 导出 DTO\r?\n\/\/ ========================================/;

/**
 * 提取类体（不含外层花括号）
 * @param {string} content
 * @param {string} className
 */
function extractClassBody(content, className) {
  const startRegex = new RegExp(`public\\s+(?:partial\\s+)?class\\s+${className}\\b`);
  const startMatch = startRegex.exec(content);
  if (!startMatch) {
    return null;
  }
  const braceStart = content.indexOf('{', startMatch.index);
  if (braceStart < 0) {
    return null;
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
  return null;
}

/**
 * 拆分 CreateDto 属性块（含 XML 注释与特性）
 * @param {string} classBody
 */
function splitPropertyBlocks(classBody) {
  const lines = classBody.split('\n');
  const blocks = [];
  let i = 0;
  while (i < lines.length) {
    const trimmed = lines[i].trim();
    if (!trimmed.startsWith('///') && !trimmed.startsWith('[') && !lines[i].includes('public ')) {
      i += 1;
      continue;
    }
    const start = i;
    while (i < lines.length && !lines[i].includes('{ get; set;')) {
      i += 1;
    }
    if (i >= lines.length) {
      break;
    }
    while (i < lines.length && !lines[i].trim().endsWith(';')) {
      i += 1;
    }
    const block = lines.slice(start, i + 1).join('\n').trimEnd();
    if (block.includes('public ')) {
      blocks.push(block);
    }
    i += 1;
  }
  return blocks;
}

/**
 * 去掉 [Required] 特性行
 * @param {string} block
 */
function stripRequiredAttributes(block) {
  return block
    .split('\n')
    .filter((line) => !/^\s*\[Required\(/.test(line))
    .join('\n');
}

/**
 * 将 CreateDto 属性块转为 Template/Import 可空写法
 * @param {string} block
 * @param {{ includeCompanyDefaultCulture: boolean }} options
 */
function transformPropertyBlock(block, options) {
  const { includeCompanyDefaultCulture } = options;
  let text = stripRequiredAttributes(block);
  const propMatch = text.match(/public\s+([\s\S]+?\{ get; set; \}[^;]*;)/);
  if (!propMatch) {
    return text;
  }
  const propLine = propMatch[1].trim();
  const nameMatch = propLine.match(/(\w+)\s*\{\s*get;\s*set;/);
  if (!nameMatch) {
    return text;
  }
  const propName = nameMatch[1];
  if (propName === 'CompanyDefaultCulture' && !includeCompanyDefaultCulture) {
    return '';
  }
  let newProp = propLine;
  if (propName === 'TenantCode' || propName === 'CompanyCode' || propName === 'CompanyDefaultCulture') {
    newProp = newProp.replace(/^public\s+string\s+(\w+)/, 'public string? $1');
    return text.replace(propLine, newProp);
  }
  if (/^public\s+List<[\s\S]+?\{ get; set;/.test(newProp)) {
    return text;
  }
  if (/^public\s+string\?\s/.test(newProp)) {
    return text;
  }
  if (/^public\s+int\?\s/.test(newProp) || /^public\s+long\?\s/.test(newProp)) {
    return text;
  }
  newProp = newProp
    .replace(/^public\s+string\s+(\w+)\s*\{ get; set; \}\s*=\s*string\.Empty;/, 'public string? $1 { get; set; } = string.Empty;')
    .replace(/^public\s+string\s+(\w+)\s*\{ get; set; \}/, 'public string? $1 { get; set; } = string.Empty;')
    .replace(/^public\s+int\s+(\w+)\s*\{ get; set; \}\s*=\s*0;/, 'public int? $1 { get; set; }')
    .replace(/^public\s+int\s+(\w+)\s*\{ get; set; \}/, 'public int? $1 { get; set; }')
    .replace(/^public\s+long\s+(\w+)\s*\{ get; set; \}/, 'public long? $1 { get; set; }')
    .replace(/^public\s+DateTime\s+(\w+)\s*\{ get; set; \}/, 'public DateTime? $1 { get; set; }')
    .replace(/^public\s+DateOnly\s+(\w+)\s*\{ get; set; \}/, 'public DateOnly? $1 { get; set; }')
    .replace(/^public\s+decimal\s+(\w+)\s*\{ get; set; \}\s*=\s*0;/, 'public decimal? $1 { get; set; }')
    .replace(/^public\s+decimal\s+(\w+)\s*\{ get; set; \}/, 'public decimal? $1 { get; set; }')
    .replace(/^public\s+double\s+(\w+)\s*\{ get; set; \}/, 'public double? $1 { get; set; }')
    .replace(/^public\s+bool\s+(\w+)\s*\{ get; set; \}/, 'public bool? $1 { get; set; }')
    .replace(/^public\s+Guid\s+(\w+)\s*\{ get; set; \}/, 'public Guid? $1 { get; set; }');
  return text.replace(propLine, newProp);
}

/**
 * 由 CreateDto 类体生成 Template/Import 类体
 * @param {string} createBody
 * @param {{ includeCompanyDefaultCulture: boolean }} options
 */
function buildImportTemplateBody(createBody, options) {
  return splitPropertyBlocks(createBody)
    .map((block) => transformPropertyBlock(block, options))
    .filter(Boolean)
    .join('\n\n');
}

/**
 * 组装完整 DTO 类文本
 * @param {string} entityShort
 * @param {'Template'|'Import'} kind
 * @param {string} body
 */
function buildDtoClassText(entityShort, kind, body) {
  if (kind === 'Template') {
    return [
      '/// <summary>',
      `/// ${entityShort} 导入模板行 DTO`,
      '/// </summary>',
      `public class Takt${entityShort}TemplateDto`,
      '{',
      body,
      '}',
    ].join('\n');
  }
  return [
    '/// <summary>',
    `/// ${entityShort} 导入 DTO（独立实现，不继承 TemplateDto）`,
    '/// </summary>',
    `public class Takt${entityShort}ImportDto`,
    '{',
    body,
    '}',
  ].join('\n');
}

/**
 * 处理单个 Dtos 文件
 * @param {string} filePath
 */
function syncDtoFile(filePath) {
  const base = path.basename(filePath, '.cs');
  if (!base.startsWith('Takt') || !base.endsWith('Dtos')) {
    return { skipped: true, reason: 'not-dtos' };
  }
  const entityShort = base.slice(4, -4);
  const createClass = `Takt${entityShort}CreateDto`;
  let content = fs.readFileSync(filePath, 'utf-8');
  if (!content.includes(`class ${createClass}`)) {
    return { skipped: true, reason: 'no-create' };
  }
  const importMatch = IMPORT_SECTION_START.exec(content);
  if (!importMatch) {
    return { skipped: true, reason: 'no-import-section' };
  }
  const importIdx = importMatch.index;
  const exportMatch = EXPORT_SECTION_START.exec(content);
  const exportIdx = exportMatch ? exportMatch.index : content.length;
  const createBody = extractClassBody(content, createClass);
  if (createBody == null) {
    return { skipped: true, reason: 'create-parse-failed' };
  }
  const templateBody = buildImportTemplateBody(createBody, { includeCompanyDefaultCulture: false });
  const importBody = buildImportTemplateBody(createBody, { includeCompanyDefaultCulture: true });
  const newSection = [
    '// ========================================',
    '// 导入 DTO',
    '// ========================================',
    '',
    buildDtoClassText(entityShort, 'Template', templateBody),
    '',
    buildDtoClassText(entityShort, 'Import', importBody),
    '',
  ].join('\n');
  const updated = content.slice(0, importIdx) + newSection + content.slice(exportIdx);
  if (updated === content) {
    return { skipped: true, reason: 'unchanged', entityShort };
  }
  fs.writeFileSync(filePath, updated, 'utf-8');
  return { updated: true, entityShort, path: filePath };
}

/**
 * 扫描 Dtos 目录
 * @param {string} dir
 * @param {string[]} acc
 */
function walkDtoFiles(dir, acc = []) {
  fs.readdirSync(dir, { withFileTypes: true }).forEach((entry) => {
    const full = path.join(dir, entry.name);
    if (entry.isDirectory()) {
      walkDtoFiles(full, acc);
      return;
    }
    if (entry.name.startsWith('Takt') && entry.name.endsWith('Dtos.cs')) {
      acc.push(full);
    }
  });
  return acc;
}

function main() {
  const dryRun = process.argv.includes('--dry-run');
  const only = process.argv.find((a) => a.startsWith('--only='))?.slice(7);
  const files = walkDtoFiles(DTOS_ROOT);
  let updated = 0;
  let skipped = 0;
  const skipReasons = {};
  files.forEach((filePath) => {
    const base = path.basename(filePath, '.cs');
    const entityShort = base.slice(4, -4);
    if (only && entityShort !== only) {
      return;
    }
    if (dryRun) {
      const content = fs.readFileSync(filePath, 'utf-8');
      if (IMPORT_SECTION_START.test(content) && content.includes(`Takt${entityShort}CreateDto`)) {
        console.log(`[dry-run] would sync: ${entityShort}`);
        updated += 1;
      } else {
        skipped += 1;
      }
      return;
    }
    const result = syncDtoFile(filePath);
    if (result.updated) {
      updated += 1;
      console.log(`✅ ${result.entityShort}`);
    } else {
      skipped += 1;
      skipReasons[result.reason] = (skipReasons[result.reason] || 0) + 1;
    }
  });
  console.log(`\n📊 已同步 ${updated} 个，跳过 ${skipped} 个`);
  if (!dryRun && Object.keys(skipReasons).length) {
    console.log('跳过原因:', skipReasons);
  }
}

main();
