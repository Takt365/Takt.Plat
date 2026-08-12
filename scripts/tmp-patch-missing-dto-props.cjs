// ========================================
// 临时：删除误加的伪属性，并正确补全缺失 DTO 属性
// ========================================
'use strict';
const fs = require('fs');
const path = require('path');
const { execSync } = require('child_process');

const ROOT = path.resolve(__dirname, '..');
const ERR_FILE = path.join(ROOT, 'backend/artifacts/_app-errors-utf8.txt');
const XML = path.join(ROOT, 'backend/artifacts/effective-rate-verify/Takt.Application.xml');

const FAKE_PROP =
  /^(Takt\w+(Validators|Mapper|Persistence|ServiceBase|Service|Helper)|G|AppDevelop|VS2026|Plat)$/;

function walk(dir, acc = []) {
  for (const e of fs.readdirSync(dir, { withFileTypes: true })) {
    const p = path.join(dir, e.name);
    if (e.isDirectory()) walk(p, acc);
    else if (e.name.endsWith('.cs')) acc.push(p);
  }
  return acc;
}

function stripFakeProps() {
  const dtoRoot = path.join(ROOT, 'backend/src/Takt.Application/Dtos');
  let removed = 0;
  for (const file of walk(dtoRoot)) {
    let text = fs.readFileSync(file, 'utf8');
    const orig = text;
    text = text.replace(
      /\n    \/\*\*[\s\S]*?\*\/\n    public [^\n]+ Takt\w+(?:Validators|Mapper|Persistence|ServiceBase|Service|Helper) \{ get; set; \}[^\n]*\n/g,
      '\n',
    );
    text = text.replace(
      /\n    \/\/\/ <summary>\n    \/\/\/ Takt\w+(?:Validators|Mapper|Persistence|ServiceBase|Service|Helper)\n    \/\/\/ <\/summary>\n    public [^\n]+ Takt\w+(?:Validators|Mapper|Persistence|ServiceBase|Service|Helper) \{ get; set; \}[^\n]*\n/g,
      '\n',
    );
    if (text !== orig) {
      fs.writeFileSync(file, text, 'utf8');
      removed++;
      console.log('STRIP', path.relative(ROOT, file));
    }
  }
  console.log('stripped_files', removed);
}

function rebuildErrors() {
  try {
    execSync('dotnet build backend/src/Takt.Application/Takt.Application.csproj --no-restore -v q', {
      cwd: ROOT,
      encoding: 'utf8',
      stdio: ['ignore', 'pipe', 'pipe'],
      maxBuffer: 20 * 1024 * 1024,
    });
    fs.writeFileSync(ERR_FILE, '', 'utf8');
    return '';
  } catch (e) {
    const out = `${e.stdout || ''}\n${e.stderr || ''}`;
    fs.writeFileSync(ERR_FILE, out, 'utf8');
    return out;
  }
}

function parseMissing(text) {
  const missing = new Map();
  for (const line of text.split(/\r?\n/)) {
    const errIdx = line.search(/error CS(1061|0117):/);
    if (errIdx < 0) continue;
    const msg = line.slice(errIdx);
    const ids = [...msg.matchAll(/\b([A-Z][A-Za-z0-9]+)\b/g)].map((m) => m[1]);
    const type = ids.find((x) => /Dto$/.test(x));
    if (!type) continue;
    let prop = null;
    let seenType = false;
    for (const id of ids) {
      if (id === type) {
        seenType = true;
        continue;
      }
      if (!seenType) continue;
      if (/Dto$/.test(id)) continue;
      if (FAKE_PROP.test(id)) continue;
      if (/(Validators|Mapper|Persistence|ServiceBase|Service|Helper|Controller)$/.test(id)) continue;
      if (id === 'Error' || id === 'CS1061' || id === 'CS0117') continue;
      prop = id;
      break;
    }
    if (!prop) continue;
    if (!missing.has(type)) missing.set(type, new Set());
    missing.get(type).add(prop);
  }
  return missing;
}

function loadEntityProps() {
  const map = new Map();
  const entityRoot = path.join(ROOT, 'backend/src/Takt.Domain/Entities');
  for (const file of walk(entityRoot)) {
    const text = fs.readFileSync(file, 'utf8');
    const re =
      /\/\/\/\s*<summary>\s*([\s\S]*?)\s*<\/summary>[\s\S]*?public\s+([\w\?\<\>\[\],\s]+?)\s+(\w+)\s*\{\s*get;/g;
    let m;
    while ((m = re.exec(text))) {
      const name = m[3];
      if (!map.has(name)) {
        map.set(name, {
          type: m[2].replace(/\s+/g, ' ').trim(),
          summary: m[1].replace(/\s+/g, ' ').trim(),
        });
      }
    }
  }
  return map;
}

function loadXmlProps() {
  const map = new Map();
  if (!fs.existsSync(XML)) return map;
  const text = fs.readFileSync(XML, 'utf8');
  const re =
    /<member name="P:Takt\.Application\.Dtos\.(?:[\w]+\.)*?(Takt\w+)\.(\w+)">\s*<summary>\s*([\s\S]*?)\s*<\/summary>/g;
  let m;
  while ((m = re.exec(text))) {
    map.set(`${m[1]}.${m[2]}`, m[3].replace(/\s+/g, ' ').trim());
  }
  return map;
}

function findDtoFile(typeName) {
  const dtoRoot = path.join(ROOT, 'backend/src/Takt.Application/Dtos');
  for (const file of walk(dtoRoot)) {
    const text = fs.readFileSync(file, 'utf8');
    if (new RegExp(`public class ${typeName}\\b`).test(text)) return file;
  }
  return null;
}

function resolveTargetType(typeName, fileText) {
  if (!typeName.endsWith('UpdateDto')) return typeName;
  const m = fileText.match(new RegExp(`public class ${typeName}\\s*:\\s*(Takt\\w+CreateDto)\\b`));
  return m ? m[1] : typeName;
}

function specialPropType(typeName, prop) {
  if (prop === 'Replies' && typeName === 'TaktTicketDto') return 'List<TaktTicketReplyDto>?';
  if (prop === 'CultureCode' || prop === 'CompanyDefaultCulture') return 'string?';
  if (prop === 'ProductCodes' && typeName.includes('Query')) return 'List<string>?';
  if (prop === 'IncludeInternal') return 'bool';
  if (prop === 'SortOrder') return 'int';
  if (prop.endsWith('Id') && typeName.includes('CreateDto')) return 'long';
  return null;
}

function inferPropType(typeName, prop, entityProps, fileText) {
  const sibling = fileText.match(
    new RegExp(`public\\s+([\\w\\?\\<\\>\\[\\],\\s]+?)\\s+${prop}\\s*\\{\\s*get;`),
  );
  if (sibling) return sibling[1].replace(/\s+/g, ' ').trim();
  if (entityProps.has(prop)) return entityProps.get(prop).type;
  const sp = specialPropType(typeName, prop);
  if (sp) return sp;
  if (prop.endsWith('Id') || prop === 'SortOrder' || prop.endsWith('Count') || prop.endsWith('Shorts'))
    return prop.endsWith('Id') ? 'long' : 'int';
  if (/Start$|End$|Time$|At$|Date$/.test(prop)) return 'DateTime?';
  if (/^Is|^Has|Include/.test(prop)) return 'bool';
  if (/Codes$/.test(prop)) return 'List<string>?';
  if (/Price|Amount|Cost|Quantity|Rate/.test(prop)) return 'decimal?';
  return 'string';
}

function insertProp(fileText, typeName, prop, propType, summary) {
  if (
    new RegExp(
      `public class ${typeName}\\b[\\s\\S]*?public\\s+[\\w\\?\\<\\>\\[\\],\\s]+\\s+${prop}\\s*\\{`,
    ).test(fileText)
  ) {
    return { text: fileText, changed: false };
  }
  const start = fileText.search(new RegExp(`public class ${typeName}\\b`));
  if (start < 0) return { text: fileText, changed: false };
  const braceStart = fileText.indexOf('{', start);
  let depth = 0;
  let end = -1;
  for (let i = braceStart; i < fileText.length; i++) {
    if (fileText[i] === '{') depth++;
    else if (fileText[i] === '}') {
      depth--;
      if (depth === 0) {
        end = i;
        break;
      }
    }
  }
  if (end < 0) return { text: fileText, changed: false };
  let decl;
  if (propType === 'string') {
    decl = `public string ${prop} { get; set; } = string.Empty;`;
  } else if (propType === 'string?') {
    decl = `public string? ${prop} { get; set; }`;
  } else if (propType.startsWith('List<')) {
    decl = `public ${propType} ${prop} { get; set; } = new();`;
  } else {
    decl = `public ${propType} ${prop} { get; set; }`;
  }
  const snippet =
    `\n    /// <summary>\n    /// ${summary || prop}\n    /// </summary>\n    ${decl}\n`;
  return { text: fileText.slice(0, end) + snippet + fileText.slice(end), changed: true };
}

function main() {
  console.log('strip fakes...');
  stripFakeProps();
  console.log('building...');
  const errText = rebuildErrors();
  const missing = parseMissing(errText);
  const rows = [];
  for (const [t, ps] of missing) for (const p of ps) rows.push(`${t}|${p}`);
  rows.sort();
  fs.writeFileSync(path.join(ROOT, 'backend/artifacts/_missing-props.txt'), rows.join('\n') + '\n');
  console.log('types_with_missing', missing.size, 'pairs', rows.length);
  console.log(rows.slice(0, 40).join('\n'));
  const entityProps = loadEntityProps();
  const xmlProps = loadXmlProps();
  let propsAdded = 0;
  for (const [typeName, props] of [...missing.entries()].sort((a, b) => a[0].localeCompare(b[0]))) {
    const file = findDtoFile(typeName);
    if (!file) {
      console.log('NO_FILE', typeName, [...props].join(','));
      continue;
    }
    for (const prop of [...props].sort()) {
      if (FAKE_PROP.test(prop)) continue;
      let text = fs.readFileSync(file, 'utf8');
      const targetType = resolveTargetType(typeName, text);
      const targetFile = targetType === typeName ? file : findDtoFile(targetType) || file;
      text = fs.readFileSync(targetFile, 'utf8');
      const propType =
        specialPropType(targetType, prop) ||
        specialPropType(typeName, prop) ||
        inferPropType(targetType, prop, entityProps, text);
      const summary =
        xmlProps.get(`${typeName}.${prop}`) ||
        xmlProps.get(`${targetType}.${prop}`) ||
        (entityProps.has(prop) ? entityProps.get(prop).summary : prop);
      const res = insertProp(text, targetType, prop, propType, summary);
      if (res.changed) {
        fs.writeFileSync(targetFile, res.text, 'utf8');
        propsAdded++;
        console.log('ADD', targetType, prop, propType, typeName === targetType ? '' : `(via ${typeName})`);
      }
    }
  }
  console.log('propsAdded', propsAdded);
}

main();
