/**
 * 从 dict-tag.less 编译结果合并为 0-69 顺序的 takt-dict-tag.css（14-69 取日本色系覆盖块）
 */
import fs from 'node:fs';
import path from 'node:path';
import { fileURLToPath } from 'node:url';
import { execSync } from 'node:child_process';

const __dirname = path.dirname(fileURLToPath(import.meta.url));
const frontendRoot = path.resolve(__dirname, '..');
const lessSource = path.resolve(
  'g:/AppDevelop/VS2026/Takt.Net/frontend/takt.antd/src/assets/styles/dict-tag.less'
);
const outCss = path.join(frontendRoot, 'src/styles/takt-dict-tag.css');
const tempCss = path.join(frontendRoot, 'node_modules/.cache/takt-dict-tag.compiled.css');

/** 0-13 语义注释（参照 dict-tag.less） */
const SEMANTIC_COMMENTS = {
  0: 'default - 默认（苍苍）',
  1: 'success - 成功（青葱）',
  2: 'error - 错误（檆丹）',
  3: 'warning - 警告（黄栗留）',
  4: 'processing - 处理中（群青）',
  5: 'info - 信息（宝蓝）',
  6: 'blue - 蓝色（石青）',
  7: 'cyan - 青色（蔚蓝）',
  8: 'green - 绿色（油绿）',
  9: 'orange - 橙色（朱膘）',
  10: 'red - 红色（大红）',
  11: 'purple - 紫色（青莲）',
  12: 'pink - 粉色（桃红）',
  13: 'gold - 金色（栀子）',
};

execSync(`npx --yes lessc "${lessSource}" "${tempCss}"`, { stdio: 'inherit', cwd: frontendRoot });

const cssText = fs.readFileSync(tempCss, 'utf8');

/** @type {Record<number, string>} */
const ruleByIndex = {};

const blockPattern = /\.takt-dict-tag-(\d+),\s*\n\.ant-tag\.takt-dict-tag-\1\s*\{[^}]+\}/g;
let blockMatch;

while ((blockMatch = blockPattern.exec(cssText)) !== null) {
  const index = Number(blockMatch[1]);
  ruleByIndex[index] = blockMatch[0];
}

const header = `/* ========================================
 * 项目名称：节拍工厂·Takt Plat
 * 命名空间：frontend/src/styles
 * 文件名称：takt-dict-tag.css
 * 创建时间：2026-05-25
 * 创建人：Takt365(Cursor AI)
 * 功能描述：字典标签样式（CssClass / ListClass 0-69，中日传统色系）
 *
 * 版权信息：Copyright (c) 2025 Takt  All rights reserved.
 * 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
 * ======================================== */

`;

const baseBlock = cssText.match(/\.takt-dict-tag\s*\{[\s\S]*?\}\s*\.takt-dict-tag:hover\s*\{[^}]+\}/)?.[0] ?? '';

const sizeBlock = `
/* 标签尺寸 */
.takt-dict-tag-small {
  font-size: 12px;
  padding: 0 6px;
  height: 20px;
  line-height: 20px;
}

.takt-dict-tag-middle {
  font-size: 14px;
  padding: 0 8px;
  height: 24px;
  line-height: 24px;
}

.takt-dict-tag-large {
  font-size: 16px;
  padding: 0 10px;
  height: 28px;
  line-height: 28px;
}
`;

/**
 * 生成序号注释
 * @param {number} index 样式索引
 */
function buildIndexComment(index) {
  if (SEMANTIC_COMMENTS[index]) {
    return `${index}. ${SEMANTIC_COMMENTS[index]}`;
  }

  if (index >= 14) {
    return `${index}. 字典标签颜色（日本传统色系）`;
  }

  return `${index}. 字典标签颜色（中国传统色系）`;
}

let output = header;

if (baseBlock) {
  output += '\n/* 字典标签基础样式 */\n';
  output += `${baseBlock.trim()}\n`;
}

output += '\n/* 70种字典标签颜色样式（对应 CssClass 或 ListClass 值 0-69） */\n';
output += '/* 使用中国传统色系和日本传统色系；14-69 采用日本传统色系覆盖 */\n';

for (let i = 0; i <= 69; i += 1) {
  const rule = ruleByIndex[i];
  if (!rule) {
    throw new Error(`缺少 .takt-dict-tag-${i} 规则`);
  }

  output += `\n/* ${buildIndexComment(i)} */\n`;
  output += `${rule.trim()}\n`;
}

output += sizeBlock;
fs.mkdirSync(path.dirname(tempCss), { recursive: true });
fs.writeFileSync(outCss, output, 'utf8');

console.log(`已生成 ${outCss}（0-69 顺序，共 ${Object.keys(ruleByIndex).length} 条颜色规则）`);
