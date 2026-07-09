// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：scripts
// 文件名称：regen-is-obsolete-dtos.cjs
// 创建时间：2026-07-09
// 创建人：Takt365(Cursor AI)
// 功能描述：为含 IsObsolete 的子表明细实体批量重生 Application DTO
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const path = require('path');
const { execSync } = require('child_process');
const { REPO_ROOT, collectIsObsoleteEntityShorts } = require('./is-obsolete-entities.cjs');

const GEN_SCRIPT = path.join(REPO_ROOT, 'scripts/gen/generate-dtos-from-entity.cjs');

/**
 * @param {{ dryRun?: boolean }} options
 */
function main(options = {}) {
  const dryRun = options.dryRun ?? process.argv.includes('--dry-run');
  const entityShorts = collectIsObsoleteEntityShorts();
  console.log(`📦 含 IsObsolete 实体 ${entityShorts.length} 个`);
  for (const entityShort of entityShorts) {
    if (dryRun) {
      console.log(`[dry-run] --${entityShort}`);
      continue;
    }
    console.log(`\n🔄 重生 DTO: ${entityShort}`);
    execSync(`node "${GEN_SCRIPT}" --${entityShort} --force`, {
      cwd: REPO_ROOT,
      stdio: 'inherit',
    });
  }
  console.log('\n✨ 完成');
}

main();
