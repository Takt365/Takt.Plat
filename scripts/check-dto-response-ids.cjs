/**
 * 核对响应 DTO 是否仍缺 *Id（校验 restore-dto-response-ids 结果）
 */
'use strict';
const fs = require('fs');
const path = require('path');
const root = path.join(__dirname, '..', 'backend', 'src', 'Takt.Application', 'Dtos');

/**
 * @param {string} dir
 * @param {string[]} acc
 */
function walk(dir, acc = []) {
  for (const e of fs.readdirSync(dir, { withFileTypes: true })) {
    const p = path.join(dir, e.name);
    if (e.isDirectory()) walk(p, acc);
    else if (/Takt.+Dtos\.cs$/.test(e.name)) acc.push(p);
  }
  return acc;
}

/** @type {string[]} */
const missing = [];
for (const f of walk(root)) {
  const t = fs.readFileSync(f, 'utf8');
  const re = /public\s+class\s+Takt(\w+)Dto\s*:\s*Takt(?:Company|Approval|Tenant)DtoBase\b[^{]*\{/g;
  let m;
  while ((m = re.exec(t)) !== null) {
    const short = m[1];
    const idProp = `${short}Id`;
    const open = m.index + m[0].length - 1;
    let depth = 1;
    let i = open + 1;
    while (i < t.length && depth > 0) {
      if (t[i] === '{') depth += 1;
      else if (t[i] === '}') depth -= 1;
      i += 1;
    }
    const body = t.slice(open + 1, i - 1);
    if (!new RegExp(`public\\s+long\\s+${idProp}\\s*\\{`).test(body)) {
      missing.push(`${path.relative(path.join(__dirname, '..'), f).replace(/\\/g, '/')}: ${idProp}`);
    }
  }
}
console.log(`missing ${missing.length}`);
missing.forEach((x) => console.log(x));
