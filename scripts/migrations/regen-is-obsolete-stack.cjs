// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：scripts
// 文件名称：regen-is-obsolete-stack.cjs
// 创建时间：2026-07-09
// 创建人：Takt365(Cursor AI)
// 功能描述：IsObsolete 一站式批量：DTO（含 ObsoleteDto）→ Service → Controller → 前端 types/api
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
const GEN_SERVICES = path.join(REPO_ROOT, 'scripts/gen/generate-services-from-dtos.cjs');
const GEN_CONTROLLERS = path.join(REPO_ROOT, 'scripts/gen/generate-controllers-from-services.cjs');
const GEN_FRONTEND = path.join(REPO_ROOT, 'scripts/gen/generate-from-backend.cjs');

/** 含手工业务（产出同步等），禁止 --force 覆盖服务/控制器 */
const SERVICE_REGEN_SKIP = new Set([
  'AssyDefect',
  'AssyDefectDetail',
  'AssyOutput',
  'AssyOutputDetail',
]);

/**
 * @param {string} label
 * @param {string} cmd
 */
function runStep(label, cmd) {
  console.log(`\n🔄 ${label}`);
  execSync(cmd, { cwd: REPO_ROOT, stdio: 'inherit' });
}

/**
 * @param {{ dryRun?: boolean, skipDto?: boolean, skipService?: boolean, skipController?: boolean, skipFrontend?: boolean }} options
 */
function main(options = {}) {
  const argv = process.argv.slice(2);
  const dryRun = options.dryRun ?? argv.includes('--dry-run');
  const skipDto = options.skipDto ?? argv.includes('--skip-dto');
  const skipService = options.skipService ?? argv.includes('--skip-service');
  const skipController = options.skipController ?? argv.includes('--skip-controller');
  const skipFrontend = options.skipFrontend ?? argv.includes('--skip-frontend');

  const { obsoleteShorts, masterShorts, allServiceShorts } = collectIsObsoleteStackTargets();
  const dtoShorts = [...new Set([...obsoleteShorts, ...masterShorts])].sort((a, b) => a.localeCompare(b));

  console.log(`📦 含 IsObsolete 子表实体 ${obsoleteShorts.length} 个`);
  console.log(`📦 关联主表 ${masterShorts.length} 个`);
  console.log(`📦 服务/控制器重生目标 ${allServiceShorts.length} 个（跳过手工 ${SERVICE_REGEN_SKIP.size} 个）`);

  if (!skipDto) {
    for (const entityShort of dtoShorts) {
      const cmd = `node "${GEN_DTOS}" --${entityShort} --force`;
      if (dryRun) {
        console.log(`[dry-run dto] --${entityShort}`);
        continue;
      }
      runStep(`DTO ${entityShort}`, cmd);
    }
  }

  if (!skipService) {
    for (const entityShort of allServiceShorts) {
      if (SERVICE_REGEN_SKIP.has(entityShort)) {
        console.log(`⏭️  跳过手工服务: ${entityShort}`);
        continue;
      }
      const cmd = `node "${GEN_SERVICES}" --${entityShort} --force`;
      if (dryRun) {
        console.log(`[dry-run service] --${entityShort}`);
        continue;
      }
      runStep(`Service ${entityShort}`, cmd);
    }
  }

  if (!skipController) {
    for (const entityShort of obsoleteShorts) {
      if (SERVICE_REGEN_SKIP.has(entityShort)) {
        console.log(`⏭️  跳过手工控制器: ${entityShort}`);
        continue;
      }
      const cmd = `node "${GEN_CONTROLLERS}" --${entityShort} --force`;
      if (dryRun) {
        console.log(`[dry-run controller] --${entityShort}`);
        continue;
      }
      runStep(`Controller ${entityShort}`, cmd);
    }
  }

  if (!skipFrontend) {
    for (const entityShort of obsoleteShorts) {
      const cmd = `node "${GEN_FRONTEND}" --${entityShort} --force`;
      if (dryRun) {
        console.log(`[dry-run frontend] --${entityShort}`);
        continue;
      }
      runStep(`Frontend types/api ${entityShort}`, cmd);
    }
  }

  console.log('\n✨ IsObsolete 全栈批量完成');
}

main();
