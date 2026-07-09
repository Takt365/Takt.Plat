// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：scripts
// 文件名称：regen-detail-child-dtos.cjs
// 创建时间：2026-07-09
// 创建人：Takt365(Cursor AI)
// 功能描述：批量重生子表明细 DTO 及关联主表 DTO（修正 CreateDto 不含子表主键）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const { execSync } = require('child_process');
const path = require('path');
const {
  REPO_ROOT,
  collectIsObsoleteStackTargets,
} = require('./is-obsolete-entities.cjs');

const GEN_DTOS = path.join(REPO_ROOT, 'scripts/gen/generate-dtos-from-entity.cjs');

/**
 * @param {{ dryRun?: boolean }} [options]
 */
function main(options = {}) {
  const dryRun = options.dryRun ?? process.argv.includes('--dry-run');
  const { obsoleteShorts, masterShorts } = collectIsObsoleteStackTargets();
  const dtoShorts = [...new Set([...obsoleteShorts, ...masterShorts])].sort((a, b) =>
    a.localeCompare(b),
  );
  console.log(`📦 重生 DTO ${dtoShorts.length} 个（子表 ${obsoleteShorts.length} + 主表 ${masterShorts.length}）`);
  for (const entityShort of dtoShorts) {
    const cmd = `node "${GEN_DTOS}" --${entityShort} --force`;
    if (dryRun) {
      console.log(`[dry-run] --${entityShort}`);
      continue;
    }
    console.log(`\n🔄 DTO ${entityShort}`);
    execSync(cmd, { cwd: REPO_ROOT, stdio: 'inherit' });
  }
  console.log('\n✨ 完成');
}

main();
