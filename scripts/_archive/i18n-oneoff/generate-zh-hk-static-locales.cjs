// ========================================
// 功能描述：由 zh-CN.ts 生成 locales/**/zh-HK.ts（OpenCC 简→港繁）
// ========================================

const fs = require('fs');
const path = require('path');

let OpenCC;
try {
  OpenCC = require('opencc-js');
} catch {
  console.error('请先执行: npm install opencc-js --prefix scripts');
  process.exit(1);
}

const converter = OpenCC.Converter({ from: 'cn', to: 'hk' });

/** OpenCC hk 未覆盖的常用港繁用字（在 hk 转换后再替换） */
const HK_TERM_FIXES = [
  ['用户', '用戶'],
  ['户', '戶'],
  ['启', '啟'],
  ['里', '裏'],
  ['面', '麵'],
  ['台', '臺'],
  ['群', '羣'],
];

/**
 * 简转港繁并修补常见未转换词
 * @param {string} text
 */
function toHongKongChinese(text) {
  let result = converter(text);
  for (const [from, to] of HK_TERM_FIXES) {
    result = result.split(from).join(to);
  }
  return result;
}
const localesRoot = path.resolve(__dirname, '../frontend/src/locales');
const forceOverwrite = process.argv.includes('--force');

/**
 * 转换 TS 源文件中的中文引号字符串
 * @param {string} content
 */
function convertQuotedChinese(content) {
  return content.replace(/(['"])((?:\\.|(?!\1)[^\\])*?)\1/g, (full, quote, inner) => {
    if (!/[\u4e00-\u9fff]/.test(inner)) {
      return full;
    }
    return `${quote}${toHongKongChinese(inner)}${quote}`;
  });
}

/**
 * 更新文件头中的语言描述
 * @param {string} content
 * @param {string} relDir
 */
function patchFileHeader(content, relDir) {
  return content
    .replace(/文件名称：zh-CN\.ts/g, '文件名称：zh-HK.ts')
    .replace(/中文语言包/g, '香港繁体语言包')
    .replace(/通用中文/g, '通用香港繁体')
    .replace(/· 中文/g, '· 香港繁体')
    .replace(/功能描述：([^·\n]+)（引用键 ([^）]+)）/g, (_, desc, keyPrefix) => {
      return `功能描述：${toHongKongChinese(desc)}（引用键 ${keyPrefix}）`;
    });
}

function walkZhCnFiles(dir, list = []) {
  for (const name of fs.readdirSync(dir)) {
    const full = path.join(dir, name);
    if (fs.statSync(full).isDirectory()) {
      walkZhCnFiles(full, list);
      continue;
    }
    if (name === 'zh-CN.ts') {
      list.push(full);
    }
  }
  return list;
}

let created = 0;
let skipped = 0;

for (const zhCnPath of walkZhCnFiles(localesRoot)) {
  const zhHkPath = zhCnPath.replace(/zh-CN\.ts$/, 'zh-HK.ts');
  const relDir = path.relative(localesRoot, path.dirname(zhCnPath)).replace(/\\/g, '/');

  if (fs.existsSync(zhHkPath) && !forceOverwrite) {
    const existing = fs.readFileSync(zhHkPath, 'utf8');
    if (/import\s+.+\s+from\s+['"]\.\/zh-TW['"]/.test(existing)) {
      skipped++;
      continue;
    }
  }

  let content = fs.readFileSync(zhCnPath, 'utf8');
  content = patchFileHeader(content, relDir);
  content = convertQuotedChinese(content);
  fs.writeFileSync(zhHkPath, content, 'utf8');
  created++;
  console.log(`  ${path.relative(localesRoot, zhHkPath).replace(/\\/g, '/')}`);
}

console.log(`\n生成 ${created} 个 zh-HK.ts，跳过 ${skipped} 个（已用 zh-TW 复用）。`);
