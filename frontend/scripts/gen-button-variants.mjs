/**
 * 仅更新 button-base.css 内 @takt-generated variants 标记段（配色 token）。
 * 通用交互见 button-base.rules.css。
 *
 * 用法：node frontend/scripts/gen-button-variants.mjs
 */
import { readFileSync, writeFileSync, unlinkSync, existsSync } from 'node:fs';
import { dirname, join } from 'node:path';
import { fileURLToPath } from 'node:url';

const stylesDir = join(dirname(fileURLToPath(import.meta.url)), '../src/styles');
const colorBasePath = join(stylesDir, 'color-base.css');
const cssPath = join(stylesDir, 'button-base.css');
const legacyVariantsPath = join(stylesDir, 'button-variants.css');

const VARIANTS_BEGIN = '/* @takt-generated variants-begin */';
const VARIANTS_END = '/* @takt-generated variants-end */';

/**
 * 配色表：每项为 [action键, color-base 色名, 可选实心按钮文字色]
 * - action键：对应 action.key，类名 .takt-button-{action键}
 * - 色名：解析为 var(--takt-cn-* | --takt-jp-* | --takt-*)
 * - 第三项 '#1a1a1a'：浅色底实心按钮用 --takt-btn-fg-on-light
 */
const variants = [
  ['query', 'qunqing'],
  ['create', 'baolan'],
  ['update', 'chengqian'],
  ['delete', 'shoujou'],
  ['detail', 'qingse'],
  ['unlock', 'zhusha'],
  ['preview', 'weilan', '#1a1a1a'],
  ['print', 'chengqian'],
  ['import', 'qinglian'],
  ['export', 'iroha'],
  ['template', 'lanqing'],
  ['approve', 'zhusha'],
  ['revoke', 'fenhong'],
  ['copy', 'gunjyo'],
  ['insert', 'gunjyo'],
  ['clone', 'murasaki'],
  ['suspend', 'zhujiao'],
  ['resume', 'lvse'],
  ['submit', 'zhusha'],
  ['withdraw', 'juhong'],
  ['transfer', 'feise'],
  ['delegate', 'taohong'],
  ['return', 'momo'],
  ['urge', 'beni'],
  ['addsign', 'momoji'],
  ['subsign', 'kurenai'],
  ['progress', 'dianqing'],
  ['history', 'qingbi'],
  ['publish', 'zangqing'],
  ['disable', 'memorial-gray'],
  ['enable', 'lvse'],
  ['version', 'zitan'],
  ['design', 'qingse'],
  ['config', 'zijiang'],
  ['validate', 'qinglian'],
  ['start', 'qunqing'],
  ['terminate', 'jiangzi'],
  ['field', 'xueqing', '#1a1a1a'],
  ['permission', 'murasaki', '#1a1a1a'],
  ['datasource', 'usumurasaki', '#1a1a1a'],
  ['theme', 'wakamurasaki', '#1a1a1a'],
  ['data', 'huohong'],
  ['archive', 'jujube'],
  ['clean', 'zhujiao'],
  ['draft', 'juhuang'],
  ['deletedraft', 'youkan'],
  ['send', 'honggan'],
  ['forward', 'hong'],
  ['reply', 'fujimurasaki'],
  ['read', 'honggan'],
  ['unread', 'chairo'],
  ['circulate', 'sakura'],
  ['sign', 'jujube'],
  ['confirm', 'kurenai'],
  ['like', 'shoujou'],
  ['unlike', 'beni'],
  ['favorite', 'kurenai'],
  ['unfavorite', 'lvse'],
  ['share', 'shoubuiro'],
  ['unshare', 'kikyou', '#1a1a1a'],
  ['comment', 'qiuhuang', '#1a1a1a'],
  ['uncomment', 'nenlv', '#1a1a1a'],
  ['flagging', 'liulv', '#1a1a1a'],
  ['unflagging', 'shengqi', '#1a1a1a'],
  ['follow', 'conglv', '#1a1a1a'],
  ['unfollow', 'caolv', '#1a1a1a'],
  ['upload', 'cuiqing', '#1a1a1a'],
  ['download', 'youlv'],
  ['destroy', 'chengqian'],
  ['run', 'qingbi'],
  ['stop', 'bise'],
  ['restart', 'songbeilv'],
  ['refresh', 'midori'],
  ['reset', 'nezumi'],
  ['empty', 'mizu', '#1a1a1a'],
  ['authorize', 'koubai'],
  ['allocate', 'fujimurasaki'],
  ['allocate-user-role', 'chengqian'],
  ['allocate-role-menu', 'qingbi'],
  ['allocate-dept-role', 'bise'],
  ['allocate-dept-user', 'songbeilv'],
  ['allocate-post-user', 'midori'],
  ['allocate-tenant-user', 'moegi'],
  ['allocate-user-tenant', 'moegi'],
  ['allocate-user-company', 'baolan'],
  ['allocate-role-company', 'bise'],
  ['resetpwd', 'juhong'],
  ['changepwd', 'huangse', '#1a1a1a'],
  ['change', 'asagi', '#1a1a1a'],
  ['truncate', 'warabi'],
  ['calculate', 'kurenai'],
  ['fullscreen', 'gunjyo'],
  ['expand', 'kon'],
  ['more', 'gunjyo'],
  ['transpose', 'sora', '#1a1a1a'],
  ['create-row', 'moegi'],
  ['delete-row', 'feise'],
  ['book', 'chengse'],
  ['closing', 'juhuang', '#1a1a1a'],
  ['reconcile', 'tangerine', '#1a1a1a'],
  ['payment', 'huangse', '#1a1a1a'],
  ['depreciation', 'yahuang', '#1a1a1a'],
  ['reimburse', 'tangerine', '#1a1a1a'],
  ['reversal', 'jianghuang', '#1a1a1a'],
  ['accrual', 'kuxiang'],
  ['period', 'feicui'],
  ['carryforward', 'yunu', '#1a1a1a'],
  ['cancel', 'haikese'],
  ['generate', 'klein-blue'],
  ['sync', 'jujube'],
  ['columns', 'zise'],
  ['tables', 'kaki'],
  ['databases', 'mame'],
  ['initialize', 'memorial-gray'],
];

const colorBaseText = readFileSync(colorBasePath, 'utf8');
const cnColors = new Set([...colorBaseText.matchAll(/--takt-cn-([a-z0-9-]+):/g)].map((m) => m[1]));
const jpColors = new Set([...colorBaseText.matchAll(/--takt-jp-([a-z0-9-]+):/g)].map((m) => m[1]));
const reservedTaktTokens = new Set(['white', 'black']);
const paletteColors = new Set(
  [...colorBaseText.matchAll(/--takt-([a-z0-9-]+):/g)]
    .map((m) => m[1])
    .filter((name) => !name.startsWith('cn-') && !name.startsWith('jp-') && !reservedTaktTokens.has(name)),
);

/**
 * @param {string} color
 * @returns {string}
 */
function resolveColorToken(color) {
  if (color.startsWith('#')) {
    return color;
  }
  if (paletteColors.has(color)) {
    return `var(--takt-${color})`;
  }
  if (jpColors.has(color)) {
    return `var(--takt-jp-${color})`;
  }
  if (cnColors.has(color)) {
    return `var(--takt-cn-${color})`;
  }
  throw new Error(`color-base.css 中未找到色名「${color}」，请检查 gen-button-variants.mjs 的 variants 配置`);
}

const lines = [
  `/* 共 ${variants.length} 个 .takt-button-{action}；每类仅注入 --takt-btn-*，交互见 button-base.rules.css */`,
  '',
];

for (const [name, color, fg] of variants) {
  const v = resolveColorToken(color);
  const text = fg === '#1a1a1a' ? 'var(--takt-btn-fg-on-light)' : 'var(--takt-btn-fg-solid)';
  lines.push(
    `/* ${name} · ${v} · fg ${text} */`,
    `.takt-button-${name} {`,
    `  --takt-btn-bg: ${v};`,
    `  --takt-btn-border: ${v};`,
    `  --takt-btn-fg: ${text};`,
    `  --takt-btn-tone: ${v};`,
    '}',
    '',
  );
}

const variantBlock = lines.join('\n').trimEnd();

let css = readFileSync(cssPath, 'utf8');
const beginIdx = css.indexOf(VARIANTS_BEGIN);
const endIdx = css.indexOf(VARIANTS_END);

if (beginIdx === -1 || endIdx === -1 || endIdx < beginIdx) {
  throw new Error(
    `button-base.css 缺少标记：${VARIANTS_BEGIN} / ${VARIANTS_END}，请先保留这对标记再运行本脚本。`,
  );
}

const before = css.slice(0, beginIdx + VARIANTS_BEGIN.length);
css = `${before}\n${variantBlock}\n${VARIANTS_END}\n`;

writeFileSync(cssPath, css, 'utf8');

if (existsSync(legacyVariantsPath)) {
  unlinkSync(legacyVariantsPath);
}

console.log(`Updated variants in ${cssPath} (${variants.length} actions)`);
