/**
 * 修复 sync-plant-related-plant-incremental 误删响应 DTO 主键：
 * strip PlantCode/RelatedPlant 的正则因 [\s\S]*? 回溯，从首个 summary（*Id）一直匹配到 PlantCode，整块被删。
 *
 * 本脚本：对 TaktXxxDto : Takt*DtoBase，若类体内无 XxxId，则在开括号后插入标准主键块。
 *
 * 用法：
 *   node scripts/restore-dto-response-ids.cjs
 *   node scripts/restore-dto-response-ids.cjs --dry-run
 */
'use strict';

const fs = require('fs');
const path = require('path');

const REPO_ROOT = path.join(__dirname, '..');
const DTOS_ROOT = path.join(REPO_ROOT, 'backend', 'src', 'Takt.Application', 'Dtos');
const DRY_RUN = process.argv.includes('--dry-run');

/** @type {string[]} */
const changed = [];

/**
 * @param {string} dir
 * @param {(f: string) => boolean} pred
 * @returns {string[]}
 */
function walk(dir, pred) {
  /** @type {string[]} */
  const out = [];
  if (!fs.existsSync(dir)) return out;
  for (const ent of fs.readdirSync(dir, { withFileTypes: true })) {
    const p = path.join(dir, ent.name);
    if (ent.isDirectory()) out.push(...walk(p, pred));
    else if (pred(p)) out.push(p);
  }
  return out;
}

/**
 * @param {string} content
 * @param {number} openBraceIndex
 * @returns {number} exclusive end index of closing brace
 */
function findClosingBrace(content, openBraceIndex) {
  let depth = 1;
  let i = openBraceIndex + 1;
  while (i < content.length && depth > 0) {
    if (content[i] === '{') depth += 1;
    else if (content[i] === '}') depth -= 1;
    i += 1;
  }
  return i;
}

/**
 * @param {string} entityShort
 * @returns {string}
 */
function buildIdBlock(entityShort) {
  const idProp = `${entityShort}Id`;
  return [
    '',
    '    /// <summary>',
    `    /// ${entityShort}ID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）`,
    '    /// </summary>',
    '    [AdaptMember("Id")]',
    '    [JsonConverter(typeof(ValueToStringConverter))]',
    `    public long ${idProp} { get; set; }`,
    '',
  ].join('\n');
}

/**
 * @param {string} filePath
 */
function patchFile(filePath) {
  let content = fs.readFileSync(filePath, 'utf8');
  const before = content;
  const classRe =
    /public\s+class\s+(Takt(\w+))Dto\s*:\s*Takt(?:Company|Approval|Tenant)DtoBase\b[^{]*\{/g;
  /** @type {Array<{ full: string, entityShort: string, openIdx: number }>} */
  const hits = [];
  let m;
  while ((m = classRe.exec(content)) !== null) {
    hits.push({
      full: m[1],
      entityShort: m[2],
      openIdx: m.index + m[0].length - 1,
    });
  }
  // 从后往前插，避免偏移
  for (let i = hits.length - 1; i >= 0; i -= 1) {
    const { entityShort, openIdx } = hits[i];
    const closeExcl = findClosingBrace(content, openIdx);
    const body = content.slice(openIdx + 1, closeExcl - 1);
    const idProp = `${entityShort}Id`;
    if (new RegExp(`\\bpublic\\s+long\\s+${idProp}\\s*\\{`).test(body)) continue;
    if (new RegExp(`\\bpublic\\s+long\\?\\s+${idProp}\\s*\\{`).test(body)) continue;
    const insert = buildIdBlock(entityShort);
    content = content.slice(0, openIdx + 1) + insert + content.slice(openIdx + 1);
  }
  if (content === before) return;
  if (!DRY_RUN) fs.writeFileSync(filePath, content, 'utf8');
  changed.push(path.relative(REPO_ROOT, filePath).replace(/\\/g, '/'));
}

const files = walk(DTOS_ROOT, (f) => /Takt.+Dtos\.cs$/.test(path.basename(f)));
for (const f of files) patchFile(f);

console.log(`${DRY_RUN ? 'dry-run ' : ''}restored response Id in ${changed.length} files`);
for (const f of changed.slice(0, 30)) console.log(' -', f);
if (changed.length > 30) console.log(` ... +${changed.length - 30} more`);
