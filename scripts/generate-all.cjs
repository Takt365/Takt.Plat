// ========================================

// 项目名称：节拍工厂·Takt Plat

// 命名空间：scripts

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

const {

  logGeneratedFileWritePolicy,

  parseSingleEntityGenerateArgs,

  buildSingleEntityChildArgs,

} = require('./generate-script-common.cjs');

const {

  validateEntityMasterDetailAssociations,

  listAssociationsForMaster,

  forEachPairedChildAssociation,

} = require('./generate-master-detail-associations.cjs');



/** 手工独立服务/控制器（单实体生成时须 --force 才覆盖） */

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

  },

  {

    key: 'validators',

    label: '验证器（实体 → Application/Validators）',

    script: 'generate-validators-from-entity.cjs',

    alwaysAll: true,

  },

  {

    key: 'services',

    label: '服务接口/实现（DTO → Application/Services）',

    script: 'generate-services-from-dtos.cjs',

  },

  {

    key: 'controllers',

    label: '控制器（服务接口 → WebApi/Controllers）',

    script: 'generate-controllers-from-services.cjs',

  },

  {

    key: 'frontend',

    label: '前端类型与 API（后端 DTO/控制器 → frontend）',

    script: 'generate-from-backend.cjs',

  },

  {

    key: 'i18n',

    label: 'i18n 翻译种子（实体 → Infrastructure/Seeds）',

    script: 'generate-entity-i18n-seed.cjs',

    alwaysAll: true,

  },

  {

    key: 'dict-i18n',

    label: 'i18n 翻译种子（字典项 dict.* → Infrastructure/Seeds）',

    script: 'generate-dict-i18n-seed.cjs',

    alwaysAll: true,

  },

  {

    key: 'menu-i18n',

    label: 'i18n 翻译种子（菜单导航 menu.* → Infrastructure/Seeds）',

    script: 'generate-menu-i18n-seed.cjs',

    alwaysAll: true,

  },

  {

    key: 'vue',

    label: '前端视图与表单（types/api → views；CRUD + TREE + Master-Detail）',

    script: 'generate-vue-all-from-api.cjs',

  },

];


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

  node scripts/generate-all.cjs --CostCenter

  node scripts/generate-all.cjs --Holiday [--force] [--dry-run]



参数:

  --<实体名>         单实体生成，如 --Holiday、--Dept（不要带 Takt 前缀）

  --force            已废弃（普通文件默认覆盖；仅 TaktAuth/TaktRbac 手工服务须 --force 才覆盖）

  --dry-run          传递给子脚本，仅预览不写盘



说明:

  - 已禁用 --all；每次必须指定一个实体

  - 主实体 OneToMany 子表将级联执行完整流水线（DTO/服务/控制器/types/api/Vue），须与子表 ManyToOne 成对



流水线顺序:

  0. generate-entity-rbac-navigations.cjs（rbac-parent-config → 主实体导航）

  1. generate-dtos-from-entity.cjs（含上一步；单跑 DTO 脚本时也会先同步导航）

  3. generate-validators-from-entity.cjs（全量）

  4. generate-services-from-dtos.cjs

  5. generate-controllers-from-services.cjs

  6. generate-from-backend.cjs

  7. generate-entity-i18n-seed.cjs（全量）

  8. generate-dict-i18n-seed.cjs（全量）

  9. generate-menu-i18n-seed.cjs（全量）

  10. generate-vue-all-from-api.cjs（串联 CRUD / TREE / Master-Detail；*ChangeLog 无独立 Vue，见 generate-entity-exclusions.cjs）



示例:

  node scripts/generate-all.cjs --Holiday

  node scripts/generate-all.cjs --CostCenter

`);

}



/**

 * @param {typeof PIPELINE[number]} step

 * @param {{ entityPrefix: string, force: boolean, dryRun: boolean }} options

 * @returns {'ran'|'skipped'}

 */

function runPipelineStep(step, options) {

  const childArgs = step.alwaysAll
    ? ['--all']
    : buildSingleEntityChildArgs(options);

  console.log(`\n${'═'.repeat(60)}`);

  console.log(`▶ ${step.label}`);

  console.log(`  node scripts/${step.script} ${childArgs.join(' ')}`);

  console.log(`${'═'.repeat(60)}\n`);

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

  const options = parseSingleEntityGenerateArgs(printUsage);

  const childArgs = buildSingleEntityChildArgs(options);



  console.log(`📦 模式: 单实体（--${options.entityPrefix}）`);

  if (options.force) {

    console.log('⚙️  --force：将覆盖 TaktAuth/TaktRbac 等手工服务（普通模块默认无需此参数）');

  }

  if (options.dryRun) {

    console.log('🔍 --dry-run 已启用（子脚本仅预览）');

  }

  console.log(`\n将依次执行 ${PIPELINE.length} 个步骤，参数: ${childArgs.join(' ')}`);



  const summary = { ran: 0, skipped: 0 };

  /** @type {Set<string>} */
  const processedEntities = new Set();

  /**
   * 单实体 + OneToMany 级联子实体完整流水线
   * @param {string} entityPrefix
   */
  function runFullPipelineForEntity(entityPrefix) {
    if (processedEntities.has(entityPrefix)) {
      console.log(`⏭️  已执行过 Takt${entityPrefix}，跳过重复级联`);
      return;
    }
    processedEntities.add(entityPrefix);
    validateEntityMasterDetailAssociations(entityPrefix);
    console.log(`\n${'═'.repeat(60)}`);
    console.log(`📦 实体 Takt${entityPrefix}`);
    console.log(`${'═'.repeat(60)}\n`);
    const entityOptions = { ...options, entityPrefix };
    for (const step of PIPELINE) {
      const status = runPipelineStep(step, entityOptions);
      if (status === 'skipped') {
        summary.skipped += 1;
      } else {
        summary.ran += 1;
      }
    }
    forEachPairedChildAssociation(entityPrefix, (childShort, assoc) => {
      console.log(
        `\n── 关联对 [OneToMany ↔ ManyToOne] Takt${assoc.masterPascal}.${assoc.masterNavProp} → ` +
        `Takt${assoc.childPascal}.${assoc.childNavProp}（外键 ${assoc.fkFieldOnChild}）完整流水线 ──\n`,
      );
      runFullPipelineForEntity(childShort);
    });
  }

  runFullPipelineForEntity(options.entityPrefix);



  console.log(`\n${'═'.repeat(60)}`);

  console.log(`✨ 全部完成：执行 ${summary.ran} 步，跳过 ${summary.skipped} 步`);

  console.log('请编译 backend 解决方案，并人工审阅 QueryExpression、权限码与 Mapster 配置。');

  console.log(`${'═'.repeat(60)}\n`);

} catch (error) {

  console.error('❌ generate-all 失败:', error);

  process.exit(1);

}

