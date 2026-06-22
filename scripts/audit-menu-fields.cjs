// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：scripts
// 文件名称：audit-menu-fields.cjs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：串联菜单三字段审计（MenuCode / I18nKey / Permission / 结构）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const { spawnSync } = require('child_process');
const path = require('path');

const SCRIPTS_ROOT = path.resolve(__dirname);

/** @type {Array<{ name: string, script: string }>} */
const STEPS = [
  { name: 'audit-menu-code', script: 'audit-menu-code.cjs' },
  { name: 'audit-menu-i18n-key', script: 'audit-menu-i18n-key.cjs' },
  { name: 'audit-menu-permission', script: 'audit-menu-permission.cjs' },
  { name: 'audit-menu-structure', script: 'audit-menu-structure.cjs' },
];

function main() {
  console.log('菜单三字段审计（约 30~90 秒，请等待）…\n');
  const started = Date.now();
  let failed = false;

  for (const step of STEPS) {
    const stepStart = Date.now();
    console.log(`▶ node scripts/${step.script}`);
    const result = spawnSync(process.execPath, [path.join(SCRIPTS_ROOT, step.script)], {
      cwd: path.resolve(SCRIPTS_ROOT, '..'),
      encoding: 'utf-8',
      stdio: ['ignore', 'pipe', 'pipe'],
    });
    const output = `${result.stdout || ''}${result.stderr || ''}`.trim();
    if (output) {
      console.log(output.split('\n').map((line) => `   ${line}`).join('\n'));
    }
    const elapsed = ((Date.now() - stepStart) / 1000).toFixed(1);
    console.log(`   耗时 ${elapsed}s\n`);
    if (result.status !== 0) {
      failed = true;
      break;
    }
  }

  const total = ((Date.now() - started) / 1000).toFixed(1);
  console.log(`汇总: ${failed ? 'FAIL ❌' : 'PASS ✅'} | 总耗时 ${total}s`);
  if (failed) {
    process.exit(1);
  }
}

main();
