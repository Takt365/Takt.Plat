// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/scripts
// 文件名称：generate-all.cjs
// 创建时间：2026-05-23
// 创建人：Takt365(Cursor AI)
// 功能描述：一键串联后端/前端代码生成（DTO、验证器、服务、控制器、前端、i18n）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const path = require('path');
const { spawnSync } = require('child_process');
const { MANUAL_CRUD_ENTITY_SHORT_NAMES, isManualCrudEntity, isManualFrontendEntity } = require('./generate-entity-exclusions.cjs');
const { logGeneratedFileWritePolicy } = require('./generate-script-common.cjs');

/** 手工独立服务/控制器（--all 时不生成/不覆盖，与文件写入策略无关） */
const EXISTING_MANUAL_SERVICE_ENTITIES = ['TaktAuth', 'TaktRbac', 'TaktFlowEngine'];

const REPO_ROOT = path.resolve(__dirname, '..');
const SCRIPTS_DIR = __dirname;

/**
 * 生成流水线步骤（按依赖顺序）
 * @type {Array<{ key: string, label: string, script: string, skipForSpecialEntity?: boolean }>}
 */
const PIPELINE = [
  {
    key: 'entity-rbac-nav',
    label: '主实体 RBAC 导航（rbac-parent-config → Domain/Entities）',
    script: 'generate-entity-rbac-navigations.cjs',
    skipForSpecialEntity: false,
  },
  {
    key: 'dtos',
    label: 'DTO（实体 → Application/Dtos）',
    script: 'generate-dtos-from-entity.cjs',
    skipForSpecialEntity: true,
  },
  {
    key: 'validators',
    label: '验证器（实体 → Application/Validators）',
    script: 'generate-validators-from-entity.cjs',
  },
  {
    key: 'services',
    label: '服务接口/实现（DTO → Application/Services）',
    script: 'generate-services-from-dtos.cjs',
    skipForSpecialEntity: true,
  },
  {
    key: 'controllers',
    label: '控制器（服务接口 → WebApi/Controllers）',
    script: 'generate-controllers-from-services.cjs',
    skipForSpecialEntity: true,
  },
  {
    key: 'frontend',
    label: '前端类型与 API（后端 DTO/控制器 → frontend）',
    script: 'generate-from-backend.cjs',
    skipForManualFrontend: true,
  },
  {
    key: 'i18n',
    label: 'i18n 翻译种子（实体 → Infrastructure/Seeds）',
    script: 'generate-entity-i18n-seed.cjs',
  },
  {
    key: 'vue',
    label: '前端视图与表单（types/api → views）',
    script: 'generate-vue-from-api.cjs',
    skipForManualFrontend: true,
  },
];

/**
 * @param {string} entityShort
 * @returns {boolean}
 */
function isSpecialEntity(entityShort) {
  return isManualCrudEntity(entityShort);
}

/**
 * @param {{ all: boolean, entityPrefix: string|null, force: boolean, dryRun: boolean }} options
 * @returns {string[]}
 */
function buildChildArgs(options) {
  const args = [];
  if (options.all) {
    args.push('--all');
  } else if (options.entityPrefix) {
    args.push(`--${options.entityPrefix}`);
  }
  if (options.force) {
    args.push('--force');
  }
  if (options.dryRun) {
    args.push('--dry-run');
  }
  return args;
}

/**
 * @param {string} scriptName
 * @param {string[]} args
 * @returns {number} 进程退出码
 */
function runChildScript(scriptName, args) {
  const scriptPath = path.join(SCRIPTS_DIR, scriptName);
  const result = spawnSync(process.execPath, [scriptPath, ...args], {
    cwd: REPO_ROOT,
    stdio: 'inherit',
    env: process.env,
  });
  if (result.error) {
    throw result.error;
  }
  return result.status ?? 1;
}

function printUsage() {
  console.log(`
用法:
  node scripts/generate-all.cjs --all
  node scripts/generate-all.cjs --Holiday
  node scripts/generate-all.cjs --User [--force] [--dry-run]

参数:
  --all              全量生成（跳过 User/Online/Message、RBAC 八表、手工独立服务）
  --<实体名>         单实体生成，如 --Holiday、--Dept（不要带 Takt 前缀）
  --force            已废弃（普通文件默认覆盖；仅 TaktAuth/TaktRbac 手工服务须 --force 才覆盖）
  --dry-run          传递给子脚本，仅预览不写盘

流水线顺序:
  0. generate-entity-rbac-navigations.cjs（rbac-parent-config → 主实体导航）
  1. generate-dtos-from-entity.cjs（含上一步；单跑 DTO 脚本时也会先同步导航）
  3. generate-validators-from-entity.cjs
  4. generate-services-from-dtos.cjs
  5. generate-controllers-from-services.cjs
  6. generate-from-backend.cjs
  7. generate-entity-i18n-seed.cjs
  8. generate-vue-from-api.cjs（最后；User/Menu/workflow 等见 generate-entity-exclusions.cjs）

说明:
  - 写入策略（各子脚本统一）：目标文件不存在则创建，已存在则整文件覆盖更新（无需 --force）
  - 例外：TaktAuth 等手工独立服务/控制器，仅在使用 --force 时才会被服务/控制器脚本覆盖
  - 手工 CRUD 实体：User（密码等）、Online、Message（跳过 DTO/服务/控制器/前端）
  - RBAC 八表：UserRole/UserTenant/…/EmployeePost（仅 TaktRbac，跳过独立 CRUD）
  - 独立服务：TaktAuth、TaktRbac 等（见 generate-entity-exclusions.cjs）
  - 主子表（OneToMany，如 Culture+TranslationList、DictType+DictDataList）：
      DTO：响应 List<子Dto>；Create/Update List<子CreateDto>
      服务：Fill*DetailsAsync、Save*ChildrenAsync 级联
      控制器/类型/API：与标准 CRUD 一并生成
  - 转置（仅 Translation）：
      DTO：*TransposedDto / *TransposedQueryDto / *TransposedResultDto / *TransposedBatchDto
      服务：Get*TransposedListAsync、Save*TransposedBatchAsync
      控制器：GET /transposed、POST /transposed/batch
      类型/API：get*TransposedList、save*TransposedBatch
  - 手工服务/控制器仅 TaktAuth
  - 任一步骤非零退出码则中止后续步骤

示例:
  node scripts/generate-all.cjs --Holiday
  node scripts/generate-all.cjs --all
  node scripts/generate-all.cjs --User
`);
}

/**
 * @returns {{ all: boolean, entityPrefix: string|null, force: boolean, dryRun: boolean }}
 */
function parseArgs() {
  const args = process.argv.slice(2);
  const options = { all: false, entityPrefix: null, force: false, dryRun: false };

  for (const arg of args) {
    if (arg === '--force') {
      options.force = true;
      continue;
    }
    if (arg === '--dry-run') {
      options.dryRun = true;
      continue;
    }
    if (!arg.startsWith('--')) {
      console.error(`❌ 未知参数: ${arg}`);
      process.exit(1);
    }
    const value = arg.slice(2);
    if (value.toLowerCase() === 'all') {
      options.all = true;
      continue;
    }
    if (value.startsWith('Takt')) {
      console.error('❌ 实体名不要带 Takt 前缀，例如 --Holiday');
      process.exit(1);
    }
    if (options.entityPrefix) {
      console.error('❌ 只能指定 --all 或一个实体名');
      process.exit(1);
    }
    options.entityPrefix = value;
  }

  if (!options.all && !options.entityPrefix) {
    console.error('❌ 请指定 --all 或 --<实体名>');
    printUsage();
    process.exit(1);
  }

  return options;
}

/**
 * @param {typeof PIPELINE[number]} step
 * @param {{ all: boolean, entityPrefix: string|null, force: boolean, dryRun: boolean }} options
 * @returns {'ran'|'skipped'}
 */
function runPipelineStep(step, options) {
  const childArgs = buildChildArgs(options);
  const isSingleSpecial =
    !options.all &&
    options.entityPrefix &&
    step.skipForSpecialEntity &&
    isManualCrudEntity(options.entityPrefix);

  const isSingleManualFrontend =
    !options.all &&
    options.entityPrefix &&
    step.skipForManualFrontend &&
    isManualFrontendEntity(options.entityPrefix);

  console.log(`\n${'═'.repeat(60)}`);
  console.log(`▶ ${step.label}`);
  console.log(`  node scripts/${step.script} ${childArgs.join(' ')}`);
  console.log(`${'═'.repeat(60)}\n`);

  if (isSingleSpecial) {
    console.log(
      `⏭️  跳过：实体 ${options.entityPrefix} 为手工维护 CRUD 模块（${[...MANUAL_CRUD_ENTITY_SHORT_NAMES].join('、')}），`,
    );
    console.log('   本子步骤不生成 DTO/服务/控制器（验证器 / i18n 子步骤仍会执行）。');
    return 'skipped';
  }

  if (isSingleManualFrontend) {
    console.log(
      `⏭️  跳过：实体 ${options.entityPrefix} 为手工维护前端模块（Online、Message），`,
    );
    console.log('   本子步骤不覆盖 foundation 下在线用户/消息与 SignalR 相关前端。');
    return 'skipped';
  }

  const isManualServiceOrController =
    options.all &&
    (step.key === 'services' || step.key === 'controllers') &&
    !options.force;

  if (isManualServiceOrController) {
    console.log(
      `ℹ️  本步骤不覆盖手工服务/控制器实体：${EXISTING_MANUAL_SERVICE_ENTITIES.join('、')}（须 --force 才生成）`,
    );
    console.log('   其余实体文件：不存在则创建，已存在则覆盖更新。');
  }

  const exitCode = runChildScript(step.script, childArgs);
  if (exitCode !== 0) {
    console.error(`\n❌ 步骤失败: ${step.label}（退出码 ${exitCode}）`);
    process.exit(exitCode);
  }
  return 'ran';
}

// ========================================
// 主流程
// ========================================

console.log('🚀 Takt 全栈代码生成（generate-all）\n');
logGeneratedFileWritePolicy();

try {
  const options = parseArgs();
  const childArgs = buildChildArgs(options);

  if (options.all) {
    console.log('📦 模式: 全量（--all）');
  } else {
    console.log(`📦 模式: 单实体（--${options.entityPrefix}）`);
    if (isSpecialEntity(options.entityPrefix)) {
      console.log(
        `ℹ️  ${options.entityPrefix} 为手工 CRUD 实体：DTO/服务/控制器/前端子步骤将跳过；验证器与 i18n 仍会生成。`,
      );
    }
  }
  if (options.force) {
    console.log('⚙️  --force：将覆盖 TaktAuth/TaktRbac 等手工服务（普通模块默认无需此参数）');
  }
  if (options.dryRun) {
    console.log('🔍 --dry-run 已启用（子脚本仅预览）');
  }
  console.log(`\n将依次执行 ${PIPELINE.length} 个步骤，参数: ${childArgs.join(' ') || '(无)'}`);

  const summary = { ran: 0, skipped: 0 };

  for (const step of PIPELINE) {
    const status = runPipelineStep(step, options);
    if (status === 'skipped') {
      summary.skipped += 1;
    } else {
      summary.ran += 1;
    }
  }

  console.log(`\n${'═'.repeat(60)}`);
  console.log(`✨ 全部完成：执行 ${summary.ran} 步，跳过 ${summary.skipped} 步`);
  console.log('请编译 backend 解决方案，并人工审阅 QueryExpression、权限码与 Mapster 配置。');
  console.log(`${'═'.repeat(60)}\n`);
} catch (error) {
  console.error('❌ generate-all 失败:', error);
  process.exit(1);
}
