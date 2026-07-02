/**
 * 一次性迁移：TaktEcDept* CRUD/DTO/服务/控制器 → TaktEcExec*
 * 保留 TaktEcDeptView*、TaktEcDeptCodes、ec-dept-view 等部门视图命名
 * 执行：node scripts/rename-ec-dept-to-ec-exec.cjs
 */
const fs = require('fs');
const path = require('path');

const ROOT = path.join(__dirname, '..');

const RENAMES = [
  ['TaktEcDeptBatchTransposedQueryDto', 'TaktEcExecBatchTransposedQueryDto'],
  ['TaktEcDeptBatchTransposedResultDto', 'TaktEcExecBatchTransposedResultDto'],
  ['TaktEcDeptBatchTransposedStageDto', 'TaktEcExecBatchTransposedStageDto'],
  ['TaktEcDeptBatchTransposedDto', 'TaktEcExecBatchTransposedDto'],
  ['TaktEcDeptTransposedResultDto', 'TaktEcExecTransposedResultDto'],
  ['TaktEcDeptTransposedQueryDto', 'TaktEcExecTransposedQueryDto'],
  ['TaktEcDeptTransposedCellDto', 'TaktEcExecTransposedCellDto'],
  ['TaktEcDeptTransposedDto', 'TaktEcExecTransposedDto'],
  ['TaktEcDeptStatQueryDto', 'TaktEcExecStatQueryDto'],
  ['TaktEcDeptStatDto', 'TaktEcExecStatDto'],
  ['TaktEcDeptExportDto', 'TaktEcExecExportDto'],
  ['TaktEcDeptImportDto', 'TaktEcExecImportDto'],
  ['TaktEcDeptTemplateDto', 'TaktEcSeizougijutsumplateDto'],
  ['TaktEcDeptUpdateDto', 'TaktEcExecUpdateDto'],
  ['TaktEcDeptCreateDto', 'TaktEcExecCreateDto'],
  ['TaktEcDeptQueryDto', 'TaktEcExecQueryDto'],
  ['TaktEcDeptDto', 'TaktEcExecDto'],
  ['TaktEcDeptValidators', 'TaktEcExecValidators'],
  ['TaktEcDeptsController', 'TaktEcExecsController'],
  ['TaktEcDeptService', 'TaktEcExecService'],
  ['ITaktEcDeptService', 'ITaktEcExecService'],
  ['TaktEcDeptBatchTransposedHelper', 'TaktEcExecBatchTransposedHelper'],
  ['TaktEcDeptTransposedHelper', 'TaktEcExecTransposedHelper'],
  ['GetEcDeptBatchTransposedListAsync', 'GetEcExecBatchTransposedListAsync'],
  ['GetEcDeptTransposedListAsync', 'GetEcExecTransposedListAsync'],
  ['GetEcDeptStatAsync', 'GetEcExecStatAsync'],
  ['ExportEcDeptAsync', 'ExportEcExecAsync'],
  ['ImportEcDeptAsync', 'ImportEcExecAsync'],
  ['GetEcDeptTemplateAsync', 'GetEcSeizougijutsumplateAsync'],
  ['DeleteEcDeptBatchAsync', 'DeleteEcExecBatchAsync'],
  ['DeleteEcDeptByIdAsync', 'DeleteEcExecByIdAsync'],
  ['UpdateEcDeptAsync', 'UpdateEcExecAsync'],
  ['CreateEcDeptAsync', 'CreateEcExecAsync'],
  ['GetEcDeptOptionsAsync', 'GetEcExecOptionsAsync'],
  ['GetEcDeptByIdAsync', 'GetEcExecByIdAsync'],
  ['GetEcDeptListAsync', 'GetEcExecListAsync'],
  ['TaktEcDeptDtos.cs', 'TaktEcExecDtos.cs'],
  ['TaktEcDeptValidators.cs', 'TaktEcExecValidators.cs'],
  ['TaktEcDeptsController.cs', 'TaktEcExecsController.cs'],
  ['TaktEcDeptService.cs', 'TaktEcExecService.cs'],
  ['ITaktEcDeptService.cs', 'ITaktEcExecService.cs'],
  ['TaktEcDeptBatchTransposedHelper.cs', 'TaktEcExecBatchTransposedHelper.cs'],
  ['TaktEcDeptTransposedHelper.cs', 'TaktEcExecTransposedHelper.cs'],
];

const FE_RENAMES = [
  ['EcDeptBatchTransposedQuery', 'EcExecBatchTransposedQuery'],
  ['EcDeptBatchTransposedResult', 'EcExecBatchTransposedResult'],
  ['EcDeptBatchTransposedStage', 'EcExecBatchTransposedStage'],
  ['EcDeptBatchTransposed', 'EcExecBatchTransposed'],
  ['EcDeptTransposedQuery', 'EcExecTransposedQuery'],
  ['EcDeptTransposedResult', 'EcExecTransposedResult'],
  ['EcDeptTransposedCell', 'EcExecTransposedCell'],
  ['EcDeptTransposed', 'EcExecTransposed'],
  ['EcDeptStatQuery', 'EcExecStatQuery'],
  ['EcDeptStat', 'EcExecStat'],
  ['EcDeptExport', 'EcExecExport'],
  ['EcDeptImport', 'EcExecImport'],
  ['EcDeptTemplate', 'EcSeizougijutsumplate'],
  ['EcDeptUpdate', 'EcExecUpdate'],
  ['EcDeptCreate', 'EcExecCreate'],
  ['EcDeptQuery', 'EcExecQuery'],
  ['EcDept', 'EcExec'],
  ['getEcDeptBatchTransposedList', 'getEcExecBatchTransposedList'],
  ['getEcDeptTransposedList', 'getEcExecTransposedList'],
  ['getEcDeptStat', 'getEcExecStat'],
  ['exportEcDeptData', 'exportEcExecData'],
  ['importEcDeptData', 'importEcExecData'],
  ['getEcDeptTemplate', 'getEcSeizougijutsumplate'],
  ['deleteEcDeptBatch', 'deleteEcExecBatch'],
  ['deleteEcDeptById', 'deleteEcExecById'],
  ['updateEcDept', 'updateEcExec'],
  ['createEcDept', 'createEcExec'],
  ['getEcDeptOptions', 'getEcExecOptions'],
  ['getEcDeptById', 'getEcExecById'],
  ['getEcDeptList', 'getEcExecList'],
  ['EC_DEPT_API_BASE', 'EC_EXEC_API_BASE'],
  ["'TaktEcDepts'", "'TaktEcExecs'"],
  ['ec-dept-transposed.d.ts', 'ec-exec-transposed.d.ts'],
  ['ec-dept-transposed', 'ec-exec-transposed'],
  ['ec-dept.d.ts', 'ec-exec.d.ts'],
  ['/ec-dept', '/ec-exec'],
  ['ec-dept.ts', 'ec-exec.ts'],
  ['from \'@/api/logistics/manufacturing/engineering-change/ec-dept\'', 'from \'@/api/logistics/manufacturing/engineering-change/ec-exec\''],
];

function applyRenames(content, pairs) {
  let s = content;
  for (const [from, to] of pairs) {
    s = s.split(from).join(to);
  }
  return s;
}

function walk(dir, exts, acc = []) {
  if (!fs.existsSync(dir)) return acc;
  for (const name of fs.readdirSync(dir)) {
    const p = path.join(dir, name);
    const st = fs.statSync(p);
    if (st.isDirectory()) {
      if (name === 'node_modules' || name === 'bin' || name === 'obj') continue;
      walk(p, exts, acc);
    } else if (exts.some((e) => p.endsWith(e))) acc.push(p);
  }
  return acc;
}

function renameFileIfExists(oldRel, newRel) {
  const oldPath = path.join(ROOT, oldRel);
  const newPath = path.join(ROOT, newRel);
  if (fs.existsSync(oldPath) && !fs.existsSync(newPath)) {
    fs.renameSync(oldPath, newPath);
    return true;
  }
  return false;
}

// 1. 重命名后端文件
const backendRenames = [
  ['backend/src/Takt.Application/Dtos/Logistics/Manufacturing/EngineeringChange/TaktEcDeptDtos.cs', 'backend/src/Takt.Application/Dtos/Logistics/Manufacturing/EngineeringChange/TaktEcExecDtos.cs'],
  ['backend/src/Takt.Application/Validators/Logistics/Manufacturing/EngineeringChange/TaktEcDeptValidators.cs', 'backend/src/Takt.Application/Validators/Logistics/Manufacturing/EngineeringChange/TaktEcExecValidators.cs'],
  ['backend/src/Takt.Application/Services/Logistics/Manufacturing/EngineeringChange/TaktEcDeptService.cs', 'backend/src/Takt.Application/Services/Logistics/Manufacturing/EngineeringChange/TaktEcExecService.cs'],
  ['backend/src/Takt.Application/Services/Logistics/Manufacturing/EngineeringChange/ITaktEcDeptService.cs', 'backend/src/Takt.Application/Services/Logistics/Manufacturing/EngineeringChange/ITaktEcExecService.cs'],
  ['backend/src/Takt.WebApi/Controllers/Logistics/Manufacturing/EngineeringChange/TaktEcDeptsController.cs', 'backend/src/Takt.WebApi/Controllers/Logistics/Manufacturing/EngineeringChange/TaktEcExecsController.cs'],
  ['backend/src/Takt.Shared/Helpers/TaktEcDeptTransposedHelper.cs', 'backend/src/Takt.Shared/Helpers/TaktEcExecTransposedHelper.cs'],
  ['backend/src/Takt.Shared/Helpers/TaktEcDeptBatchTransposedHelper.cs', 'backend/src/Takt.Shared/Helpers/TaktEcExecBatchTransposedHelper.cs'],
];

// 2. 重命名前端文件
const frontendRenames = [
  ['frontend/src/types/logistics/manufacturing/engineering-change/ec-dept.d.ts', 'frontend/src/types/logistics/manufacturing/engineering-change/ec-exec.d.ts'],
  ['frontend/src/types/logistics/manufacturing/engineering-change/ec-dept-transposed.d.ts', 'frontend/src/types/logistics/manufacturing/engineering-change/ec-exec-transposed.d.ts'],
  ['frontend/src/api/logistics/manufacturing/engineering-change/ec-dept.ts', 'frontend/src/api/logistics/manufacturing/engineering-change/ec-exec.ts'],
];

for (const [oldRel, newRel] of [...backendRenames, ...frontendRenames]) {
  if (renameFileIfExists(oldRel, newRel)) {
    console.log('renamed:', oldRel, '->', newRel);
  }
}

// 3. 批量替换后端 .cs
const backendFiles = walk(path.join(ROOT, 'backend', 'src'), ['.cs']);
let backendPatched = 0;
for (const f of backendFiles) {
  const orig = fs.readFileSync(f, 'utf8');
  let s = applyRenames(orig, RENAMES);
  s = s.replace(/设变部门执行 DTO\. DeptCode/g, '设变部门执行 DTO（扁平聚合）. DeptCode');
  s = s.replace(/EcDept 模块 DTO/g, 'EcExec 模块 DTO');
  s = s.replace(/EcDept 响应 DTO/g, 'EcExec 响应 DTO');
  s = s.replace(/设变部门 DTO/g, '设变部门执行 DTO');
  s = s.replace(/设变部门应用服务/g, '设变部门执行应用服务');
  s = s.replace(/设变部门控制器/g, '设变部门执行控制器');
  s = s.replace(/设变部门服务/g, '设变部门执行服务');
  s = s.replace(/设变部门列表/g, '设变部门执行列表');
  s = s.replace(/设变部门ID/g, '设变部门执行ID');
  s = s.replace(/设变部门不存在/g, '设变部门执行记录不存在');
  s = s.replace(/设变部门统计/g, '设变部门执行统计');
  s = s.replace(/设变部门转置/g, '设变部门执行转置');
  s = s.replace(/_ecDeptService/g, '_ecExecService');
  s = s.replace(/ecDeptService/g, 'ecExecService');
  if (s !== orig) {
    fs.writeFileSync(f, s, 'utf8');
    backendPatched++;
  }
}

// 4. 修正 TaktEcExecDto 主键字段：EcExecId → Id (string)
const execDtoPath = path.join(ROOT, 'backend/src/Takt.Application/Dtos/Logistics/Manufacturing/EngineeringChange/TaktEcExecDtos.cs');
if (fs.existsSync(execDtoPath)) {
  let dto = fs.readFileSync(execDtoPath, 'utf8');
  dto = dto.replace(
    /\/\/\/ EcDeptID[\s\S]*?public long EcExecId \{ get; set; \}/,
    `/// <summary>
    /// 设变部门执行 ID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public string Id { get; set; } = string.Empty;`
  );
  dto = dto.replace(/对应前端 TaktEcExecDto/g, '对应前端 EcExec');
  fs.writeFileSync(execDtoPath, dto, 'utf8');
}

// 5. 批量替换前端
const frontendFiles = walk(path.join(ROOT, 'frontend', 'src'), ['.ts', '.vue', '.d.ts']);
let frontendPatched = 0;
for (const f of frontendFiles) {
  if (f.includes('ec-dept-view') || f.includes('ec-dept-codes')) continue;
  const orig = fs.readFileSync(f, 'utf8');
  let s = applyRenames(orig, FE_RENAMES);
  if (s !== orig) {
    fs.writeFileSync(f, s, 'utf8');
    frontendPatched++;
  }
}

console.log(`Backend patched: ${backendPatched}, Frontend patched: ${frontendPatched}`);
