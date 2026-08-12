/**
 * 临时脚本：按实体基类向现有 DTO / Validator / Service / 前端 types·组件
 * 增量补齐 PlantCode（公司/审批）或 RelatedPlant（租户）。
 * ❌ 不整文件覆盖、不重写方法体；仅在缺失时插入。
 *
 * 用法：
 *   node scripts/sync-plant-related-plant-incremental.cjs
 *   node scripts/sync-plant-related-plant-incremental.cjs --dry-run
 *   node scripts/sync-plant-related-plant-incremental.cjs --Entity BalanceSheet
 *
 * 完成后可删除本脚本。
 */
'use strict';

const fs = require('fs');
const path = require('path');
const {
  parseEntityClassHeaderFromCsContent,
  isCompanyOrApprovalEntityBase,
  isTenantEntityBase,
  REPO_ROOT,
} = require('./gen/generate-script-common.cjs');

const ENTITIES_ROOT = path.join(REPO_ROOT, 'backend', 'src', 'Takt.Domain', 'Entities');
const DTOS_ROOT = path.join(REPO_ROOT, 'backend', 'src', 'Takt.Application', 'Dtos');
const VALIDATORS_ROOT = path.join(REPO_ROOT, 'backend', 'src', 'Takt.Application', 'Validators');
const SERVICES_ROOT = path.join(REPO_ROOT, 'backend', 'src', 'Takt.Application', 'Services');
const TYPES_ROOT = path.join(REPO_ROOT, 'frontend', 'src', 'types');
const VIEWS_ROOT = path.join(REPO_ROOT, 'frontend', 'src', 'views');

const args = process.argv.slice(2);
const DRY_RUN = args.includes('--dry-run');
const entityFilterIdx = args.indexOf('--Entity');
const ENTITY_FILTER =
  entityFilterIdx >= 0 && args[entityFilterIdx + 1]
    ? args[entityFilterIdx + 1].replace(/^Takt/, '')
    : null;

/** @type {{ file: string, change: string }[]} */
const report = [];

/**
 * @param {string} dir
 * @param {(f: string) => boolean} pred
 * @returns {string[]}
 */
function walk(dir, pred) {
  if (!fs.existsSync(dir)) return [];
  /** @type {string[]} */
  const out = [];
  for (const name of fs.readdirSync(dir)) {
    const full = path.join(dir, name);
    const st = fs.statSync(full);
    if (st.isDirectory()) out.push(...walk(full, pred));
    else if (pred(full)) out.push(full);
  }
  return out;
}

/**
 * @param {string} content
 * @param {number} openBraceIndex
 * @returns {string}
 */
function extractBraceBody(content, openBraceIndex) {
  let depth = 1;
  let i = openBraceIndex + 1;
  while (i < content.length && depth > 0) {
    const ch = content[i];
    if (ch === '{') depth += 1;
    else if (ch === '}') depth -= 1;
    i += 1;
  }
  return content.slice(openBraceIndex + 1, i - 1);
}

/**
 * @param {string} content
 * @param {string} className
 * @returns {{ start: number, end: number, headerEnd: number, body: string }|null}
 */
function findClassSpan(content, className) {
  const re = new RegExp(`public\\s+class\\s+${className}\\b[^{]*\\{`, 'm');
  const m = content.match(re);
  if (!m || m.index == null) return null;
  const open = m.index + m[0].length - 1;
  const body = extractBraceBody(content, open);
  const end = open + 1 + body.length + 1;
  return { start: m.index, end, headerEnd: open + 1, body };
}

/**
 * @param {string} body
 * @param {string} propName
 * @returns {boolean}
 */
function classBodyHasProp(body, propName) {
  // 禁止 [\w?<>,\s]+ 含换行，否则会跨属性误判
  return new RegExp(`\\bpublic\\s+[\\w?<>]+\\s+${propName}\\s*\\{`).test(body);
}

/**
 * @param {string} content
 * @param {string} className
 * @param {(body: string) => string} transform
 * @returns {string}
 */
function transformClassBody(content, className, transform) {
  const span = findClassSpan(content, className);
  if (!span) return content;
  const newBody = transform(span.body);
  if (newBody === span.body) return content;
  return content.slice(0, span.headerEnd) + newBody + content.slice(span.end - 1);
}

/**
 * @param {'company'|'tenant'} kind
 * @param {'query'|'create'|'import'} mode
 * @returns {string}
 */
function buildCsPropertyBlock(kind, mode) {
  if (kind === 'company') {
    const nullable = mode === 'query' || mode === 'import';
    const type = nullable ? 'string?' : 'string';
    return [
      '    /// <summary>',
      '    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）',
      '    /// </summary>',
      `    public ${type} PlantCode { get; set; } = string.Empty;`,
      '',
    ].join('\n');
  }
  const nullable = mode === 'query' || mode === 'import';
  const type = nullable ? 'string?' : 'string';
  return [
    '    /// <summary>',
    '    /// 关联工厂（选项 TaktPlants/options；DictValue=PlantCode）',
    '    /// </summary>',
    `    public ${type} RelatedPlant { get; set; } = string.Empty;`,
    '',
  ].join('\n');
}

/**
 * @param {string} body
 * @param {string} insertBlock
 * @param {string} propName
 * @returns {string}
 */
function insertAfterScopeProps(body, insertBlock, propName) {
  if (classBodyHasProp(body, propName)) return body;
  const anchors = ['CultureCode', 'CompanyCode', 'TenantCode'];
  for (const anchor of anchors) {
    const re = new RegExp(
      `(public\\s+[\\w?<>,\\s]+\\s+${anchor}\\s*\\{\\s*get;\\s*set;\\s*\\}\\s*=\\s*string\\.Empty;\\s*\\r?\\n)`,
    );
    if (re.test(body)) {
      return body.replace(re, `$1\n${insertBlock}`);
    }
  }
  return `\n${insertBlock}${body}`;
}

/**
 * 仅删除「属性名恰好为 propName」的声明块（summary+特性+属性）。
 * ❌ 禁止用跨越多个 summary 的 [\s\S]*?（会回溯吞掉前面的 *Id）。
 * @param {string} body
 * @param {string} propName
 * @returns {string}
 */
function stripDuplicateBaseProp(body, propName) {
  if (!new RegExp(`\\bpublic\\s+(?:string\\??)\\s+${propName}\\s*\\{`).test(body)) {
    return body;
  }
  const re = new RegExp(
    `(?:\\r?\\n)[ \\t]*\\/\\/\\/ <summary>\\r?\\n[ \\t]*\\/\\/\\/ [^\\r\\n]*\\r?\\n[ \\t]*\\/\\/\\/ <\\/summary>\\r?\\n(?:[ \\t]*\\[[^\\]]*\\]\\r?\\n)*[ \\t]*public\\s+string\\??\\s+${propName}\\s*\\{\\s*get;\\s*set;\\s*\\}(?:\\s*=\\s*string\\.Empty)?;\\r?\\n?`,
  );
  return body.replace(re, '\n');
}

/**
 * @param {string} filePath
 * @param {string} next
 * @param {string} change
 * @returns {boolean}
 */
function writeIfChanged(filePath, next, change) {
  const prev = fs.readFileSync(filePath, 'utf8');
  if (prev === next) return false;
  if (!DRY_RUN) fs.writeFileSync(filePath, next, 'utf8');
  report.push({ file: path.relative(REPO_ROOT, filePath).replace(/\\/g, '/'), change });
  return true;
}

/**
 * @param {string} entityShort
 * @param {'company'|'tenant'} kind
 * @param {string} dtoPath
 */
function patchDtoFile(entityShort, kind, dtoPath) {
  let content = fs.readFileSync(dtoPath, 'utf8');
  const prop = kind === 'company' ? 'PlantCode' : 'RelatedPlant';
  const before = content;

  content = transformClassBody(content, `Takt${entityShort}Dto`, (body) =>
    stripDuplicateBaseProp(body, prop),
  );

  /** @type {Array<{ suffix: string, mode: 'query'|'create'|'import' }>} */
  const targets = [
    { suffix: 'QueryDto', mode: 'query' },
    { suffix: 'CreateDto', mode: 'create' },
    { suffix: 'TemplateDto', mode: 'import' },
    { suffix: 'ImportDto', mode: 'import' },
  ];
  for (const t of targets) {
    const className = `Takt${entityShort}${t.suffix}`;
    const block = buildCsPropertyBlock(kind, t.mode);
    content = transformClassBody(content, className, (body) =>
      insertAfterScopeProps(body, block, prop),
    );
  }

  if (content !== before) writeIfChanged(dtoPath, content, `DTO +${prop}`);
}

/**
 * @param {string} entityShort
 * @param {'company'|'tenant'} kind
 * @param {string} validatorPath
 */
function patchValidatorFile(entityShort, kind, validatorPath) {
  if (!fs.existsSync(validatorPath)) return;
  let content = fs.readFileSync(validatorPath, 'utf8');
  const prop = kind === 'company' ? 'PlantCode' : 'RelatedPlant';
  const label = kind === 'company' ? '工厂代码' : '关联工厂';
  const before = content;

  const createRules = [
    `        RuleFor(x => x.${prop})`,
    `            .NotEmpty().WithMessage("${label}不能为空")`,
    `            .MaximumLength(4).WithMessage("${label}长度不能超过4个字符");`,
    '',
  ].join('\n');
  const importRules = [
    `        RuleFor(x => x.${prop})`,
    `            .MaximumLength(4).WithMessage("${label}长度不能超过4个字符")`,
    `            .When(x => !string.IsNullOrWhiteSpace(x.${prop}));`,
    '',
  ].join('\n');

  /**
   * @param {string} className
   * @param {string} rules
   */
  const inject = (className, rules) => {
    content = transformClassBody(content, className, (body) => {
      if (body.includes(`RuleFor(x => x.${prop})`)) return body;
      if (/RuleFor\(x => x\.ExtField\)/.test(body)) {
        return body.replace(/(\s*)RuleFor\(x => x\.ExtField\)/, `\n${rules}$1RuleFor(x => x.ExtField)`);
      }
      if (/RuleFor\(x => x\.Remark\)/.test(body)) {
        return body.replace(/(\s*)RuleFor\(x => x\.Remark\)/, `\n${rules}$1RuleFor(x => x.Remark)`);
      }
      return `${body}\n${rules}`;
    });
  };

  inject(`Takt${entityShort}CreateDtoValidator`, createRules);
  inject(`Takt${entityShort}CreateValidator`, createRules);
  inject(`Takt${entityShort}ImportDtoValidator`, importRules);
  inject(`Takt${entityShort}ImportValidator`, importRules);

  if (content !== before) writeIfChanged(validatorPath, content, `Validator +${prop}`);
}

/**
 * @param {string} entityShort
 * @param {'company'|'tenant'} kind
 * @param {string} servicePath
 */
function patchServiceFile(entityShort, kind, servicePath) {
  if (!fs.existsSync(servicePath)) return;
  const content = fs.readFileSync(servicePath, 'utf8');
  const prop = kind === 'company' ? 'PlantCode' : 'RelatedPlant';
  const local = kind === 'company' ? 'plantCode' : 'relatedPlant';
  if (new RegExp(`queryDto\\??\\.${prop}\\b`).test(content)) return;
  if (!content.includes('return exp.ToExpression()')) return;

  const filterBlock = [
    `        if (!string.IsNullOrWhiteSpace(queryDto?.${prop}))`,
    '        {',
    `            var ${local} = queryDto.${prop};`,
    `            exp = exp.And(x => x.${prop} != null && x.${prop}.Contains(${local}));`,
    '        }',
    '',
  ].join('\n');

  const next = content.replace(
    /(\s*)return exp\.ToExpression\(\);/,
    `\n${filterBlock}$1return exp.ToExpression();`,
  );
  writeIfChanged(servicePath, next, `Service QueryExpression +${prop}`);
}

/**
 * @param {string} content
 * @param {string} ifaceName
 */
function findTsInterfaceSpan(content, ifaceName) {
  const re = new RegExp(`export\\s+interface\\s+${ifaceName}\\b[^{]*\\{`, 'm');
  const m = content.match(re);
  if (!m || m.index == null) return null;
  const open = m.index + m[0].length - 1;
  const body = extractBraceBody(content, open);
  const end = open + 1 + body.length + 1;
  return { start: m.index, end, headerEnd: open + 1, body };
}

/**
 * @param {string} content
 * @param {string} ifaceName
 * @param {(body: string) => string} transform
 */
function transformTsInterface(content, ifaceName, transform) {
  const span = findTsInterfaceSpan(content, ifaceName);
  if (!span) return content;
  const newBody = transform(span.body);
  if (newBody === span.body) return content;
  return content.slice(0, span.headerEnd) + newBody + content.slice(span.end - 1);
}

/**
 * @param {string} body
 * @param {string} prop
 */
function stripTsProp(body, prop) {
  const re = new RegExp(
    `(?:\\r?\\n)?[ \\t]*\\/\\*\\*[\\s\\S]*?\\*\\/[ \\t]*\\r?\\n[ \\t]*${prop}\\s*\\??:\\s*string;\\r?\\n?`,
  );
  return body.replace(re, '\n');
}

/**
 * @param {'company'|'tenant'} kind
 * @param {boolean} optional
 */
function buildTsPropBlock(kind, optional) {
  const name = kind === 'company' ? 'plantCode' : 'relatedPlant';
  const summary =
    kind === 'company'
      ? '工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）'
      : '关联工厂（选项 TaktPlants/options；DictValue=PlantCode）';
  const opt = optional ? '?' : '';
  return [
    '  /**',
    `   * ${summary}`,
    '   */',
    `  ${name}${opt}: string;`,
    '',
  ].join('\n');
}

/**
 * @param {string} body
 * @param {string} block
 * @param {string} prop
 */
function insertTsAfterScope(body, block, prop) {
  if (new RegExp(`\\b${prop}\\s*[?]?:`).test(body)) return body;
  for (const anchor of ['cultureCode', 'companyCode', 'tenantCode']) {
    const re = new RegExp(`(${anchor}\\s*\\??:\\s*string;\\r?\\n)`);
    if (re.test(body)) return body.replace(re, `$1\n${block}`);
  }
  return `\n${block}${body}`;
}

/**
 * @param {string} typesPath
 * @param {string} entityPascal
 * @param {'company'|'tenant'} kind
 */
function patchTypesFile(typesPath, entityPascal, kind) {
  if (!fs.existsSync(typesPath)) return;
  let content = fs.readFileSync(typesPath, 'utf8');
  const prop = kind === 'company' ? 'plantCode' : 'relatedPlant';
  const before = content;

  // 主实体：若 extends *DtoBase 且重复声明 prop → 去掉
  const mainSpan = findTsInterfaceSpan(content, entityPascal);
  if (mainSpan) {
    const header = content.slice(mainSpan.start, mainSpan.headerEnd);
    if (/extends\s+(CompanyDtoBase|ApprovalDtoBase|TenantDtoBase)/.test(header)) {
      content = transformTsInterface(content, entityPascal, (body) => stripTsProp(body, prop));
    }
  }

  /** @type {Array<{ name: string, optional: boolean }>} */
  const suffixes = [
    { name: `${entityPascal}Query`, optional: true },
    { name: `${entityPascal}Create`, optional: false },
    { name: `${entityPascal}Template`, optional: true },
    { name: `${entityPascal}Import`, optional: true },
  ];
  for (const s of suffixes) {
    const block = buildTsPropBlock(kind, s.optional);
    content = transformTsInterface(content, s.name, (body) =>
      insertTsAfterScope(body, block, prop),
    );
  }

  if (content !== before) writeIfChanged(typesPath, content, `types +${prop}`);
}

/**
 * @param {string} viewsDir
 * @param {'company'|'tenant'} kind
 */
function patchVueI18nLists(viewsDir, kind) {
  const prop = kind === 'company' ? 'plantCode' : 'relatedPlant';
  const files = walk(viewsDir, (f) => /use-.*-i18n\.ts$/.test(path.basename(f)));
  for (const file of files) {
    let content = fs.readFileSync(file, 'utf8');
    if (content.includes(`'${prop}'`)) continue;
    let next = content;
    next = next.replace(
      /(=\s*\[[\s\S]*?)(\n\])/m,
      (m, a, b) => {
        // 仅改第一个看起来像字段列表的数组（含 tenantCode 等）
        if (!/tenantCode|companyCode|cultureCode/.test(a)) return m;
        if (a.includes(`'${prop}'`)) return m;
        return `${a}\n  '${prop}',${b}`;
      },
    );
    next = next.replace(
      /(PLACEHOLDER[^=]*=\s*\{[\s\S]*?)(\n\})/m,
      (m, a, b) => (a.includes(`${prop}:`) ? m : `${a}\n  ${prop}: 'select',${b}`),
    );
    if (next !== content) writeIfChanged(file, next, `vue-i18n +${prop}`);
  }

  const forms = walk(viewsDir, (f) => /-form\.vue$/.test(path.basename(f)));
  for (const file of forms) {
    let content = fs.readFileSync(file, 'utf8');
    if (!content.includes('function applyScopeDefaults')) continue;
    if (content.includes(`target.${prop}`)) continue;
    if (!content.includes('tenantStore')) continue;
    const inject =
      prop === 'plantCode'
        ? `  if (force || !target.plantCode) {\n    target.plantCode = tenantStore.currentCompanyRelatedPlant || ''\n  }\n`
        : `  if (force || !target.relatedPlant) {\n    target.relatedPlant = tenantStore.currentCompanyRelatedPlant || ''\n  }\n`;
    const next = content.replace(
      /(function applyScopeDefaults\([^)]*\)\s*\{)([\s\S]*?)(\n\})/,
      (m, a, body, c) => {
        if (body.includes(`target.${prop}`)) return m;
        // 插在函数体末尾、闭合前（保证 body 末尾换行，避免 `}  if` 粘连）
        const trimmedBody = body.replace(/\s*$/, '\n');
        return `${a}${trimmedBody}${inject}${c}`;
      },
    );
    if (next !== content) writeIfChanged(file, next, `form applyScopeDefaults +${prop}`);
  }
}

/**
 * @param {string} entityFile
 */
function processEntity(entityFile) {
  const raw = fs.readFileSync(entityFile, 'utf8');
  const header = parseEntityClassHeaderFromCsContent(raw);
  if (!header) return;
  const short = header.className.replace(/^Takt/, '');
  if (ENTITY_FILTER && short !== ENTITY_FILTER) return;

  /** @type {'company'|'tenant'|null} */
  let kind = null;
  if (isCompanyOrApprovalEntityBase(header.entityBase)) kind = 'company';
  else if (isTenantEntityBase(header.entityBase)) kind = 'tenant';
  if (!kind) return;

  const relDir = path.relative(ENTITIES_ROOT, path.dirname(entityFile));
  const dtoPath = path.join(DTOS_ROOT, relDir, `Takt${short}Dtos.cs`);
  const validatorPath = path.join(VALIDATORS_ROOT, relDir, `Takt${short}Validators.cs`);
  const servicePath = path.join(SERVICES_ROOT, relDir, `Takt${short}Service.cs`);

  if (fs.existsSync(dtoPath)) patchDtoFile(short, kind, dtoPath);
  if (fs.existsSync(validatorPath)) patchValidatorFile(short, kind, validatorPath);
  if (fs.existsSync(servicePath)) patchServiceFile(short, kind, servicePath);

  const entityKebab = short
    .replace(/([a-z0-9])([A-Z])/g, '$1-$2')
    .replace(/_/g, '-')
    .toLowerCase();
  const typeFiles = walk(TYPES_ROOT, (f) => path.basename(f) === `${entityKebab}.d.ts`);
  for (const tf of typeFiles) {
    patchTypesFile(tf, short, kind);
  }

  /** @param {string} dir @param {string[]} acc */
  function findViewDirs(dir, acc = []) {
    if (!fs.existsSync(dir)) return acc;
    for (const name of fs.readdirSync(dir)) {
      const full = path.join(dir, name);
      if (!fs.statSync(full).isDirectory()) continue;
      if (name === entityKebab) acc.push(full);
      findViewDirs(full, acc);
    }
    return acc;
  }
  for (const vd of findViewDirs(VIEWS_ROOT)) {
    patchVueI18nLists(vd, kind);
  }
}

console.log(
  DRY_RUN
    ? 'dry-run：仅报告将修改的文件'
    : '增量同步 PlantCode / RelatedPlant（不覆盖原有实现）',
);

const entityFiles = walk(ENTITIES_ROOT, (f) => f.endsWith('.cs'));
for (const f of entityFiles) {
  processEntity(f);
}

console.log(`\n完成 changes=${report.length}${DRY_RUN ? ' (dry-run)' : ''}`);
const byChange = {};
for (const r of report) {
  byChange[r.change] = (byChange[r.change] || 0) + 1;
}
console.log('汇总:', byChange);
for (const r of report.slice(0, 60)) {
  console.log(` - ${r.change}: ${r.file}`);
}
if (report.length > 60) console.log(` ... 另有 ${report.length - 60} 项`);
