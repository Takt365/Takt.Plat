/**
 * 生成设变部门视图 + 特殊视图 的服务接口/实现、控制器、前端 api/types/views
 * 用法: node scripts/generate-ec-view-stack.cjs
 */
'use strict';

const fs = require('fs');
const path = require('path');

const ROOT = path.resolve(__dirname, '..');
const APP_SVC = path.join(ROOT, 'backend/src/Takt.Application/Services/Logistics/Manufacturing/EngineeringChange');
const WEB_CTRL = path.join(ROOT, 'backend/src/Takt.WebApi/Controllers/Logistics/Manufacturing/EngineeringChange');
const FE_API = path.join(ROOT, 'frontend/src/api/logistics/manufacturing/engineering-change');
const FE_TYPES = path.join(ROOT, 'frontend/src/types/logistics/manufacturing/engineering-change');
const FE_VIEWS = path.join(ROOT, 'frontend/src/views/logistics/manufacturing/engineering-change');

const DEPT_VIEWS = [
  { slug: 'gijutsu', pascal: 'Gijutsu', deptCode: 'Eng', label: '技术部门' },
  { slug: 'koubai', pascal: 'Koubai', deptCode: 'Mp', label: '采购部门' },
  { slug: 'seikan', pascal: 'Seikan', deptCode: 'Pmc', label: '生管部门' },
  { slug: 'ukeken', pascal: 'Ukeken', deptCode: 'Iqc', label: '受检部门' },
  { slug: 'bukan', pascal: 'Bukan', deptCode: 'Mc', label: '部管部门' },
  { slug: 'seizounika', pascal: 'Seizounika', deptCode: 'Pcba', label: '制造二课' },
  { slug: 'seizouikka', pascal: 'Seizouikka', deptCode: 'Assy', label: '制造一课' },
  { slug: 'hinkan', pascal: 'Hinkan', deptCode: 'Qa', label: '品管部门' },
];

const SPECIAL_VIEWS = [
  { slug: 'kanban', pascal: 'Kanban', controllerPlural: 'TaktEcKanbans', apiEntity: 'EcKanban', listMethod: 'GetEcKanbanListAsync', getMethod: 'GetEcKanbanByEcIdAsync', getParam: 'ecId', updateMethod: null, exportMethod: 'ExportEcKanbanAsync', queryDto: 'TaktEcKanbanQueryDto', dto: 'TaktEcKanbanDto', updateDto: null, label: '设变看板', idField: 'ecId' },
  { slug: 'batch', pascal: 'Batch', controllerPlural: 'TaktEcBatches', apiEntity: 'EcBatch', listMethod: 'GetEcBatchListAsync', getMethod: 'GetEcBatchByEcDetailIdAsync', getParam: 'ecDetailId', updateMethod: 'UpdateEcBatchAsync', exportMethod: 'ExportEcBatchAsync', queryDto: 'TaktEcBatchQueryDto', dto: 'TaktEcBatchDto', updateDto: 'TaktEcBatchUpdateDto', label: '投入批次', idField: 'ecDetailId' },
  { slug: 'kakunin', pascal: 'Kakunin', controllerPlural: 'TaktEcKakunins', apiEntity: 'EcKakunin', listMethod: 'GetEcKakuninListAsync', getMethod: 'GetEcKakuninByEcDetailIdAsync', getParam: 'ecDetailId', updateMethod: 'UpdateEcKakuninAsync', exportMethod: 'ExportEcKakuninAsync', queryDto: 'TaktEcKakuninQueryDto', dto: 'TaktEcKakuninDto', updateDto: 'TaktEcKakuninUpdateDto', label: '物料确认', idField: 'ecDetailId' },
  { slug: 'legacy-product', pascal: 'LegacyProduct', controllerPlural: 'TaktEcLegacyProducts', apiEntity: 'EcLegacyProduct', listMethod: 'GetEcLegacyProductListAsync', getMethod: 'GetEcLegacyProductByEcDetailIdAsync', getParam: 'ecDetailId', updateMethod: 'UpdateEcLegacyProductAsync', exportMethod: 'ExportEcLegacyProductAsync', queryDto: 'TaktEcLegacyProductQueryDto', dto: 'TaktEcLegacyProductDto', updateDto: 'TaktEcLegacyProductUpdateDto', label: '旧品管制', idField: 'ecDetailId' },
];

function writeFile(filePath, content) {
  fs.mkdirSync(path.dirname(filePath), { recursive: true });
  fs.writeFileSync(filePath, content, 'utf8');
  console.log('  wrote', path.relative(ROOT, filePath));
}

function deptInterface(v) {
  return `// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：ITaktEc${v.pascal}Service.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：设变${v.label}视图应用服务接口（DeptCode=${v.deptCode}）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Shared.Models;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变${v.label}视图应用服务接口
/// </summary>
public interface ITaktEc${v.pascal}Service
{
    /// <summary>获取${v.label}列表（分页）</summary>
    Task<TaktPagedResult<TaktEcDeptViewDto>> GetEc${v.pascal}ListAsync(TaktEcDeptViewQueryDto queryDto);
    /// <summary>根据设变明细 ID 获取${v.label}行</summary>
    Task<TaktEcDeptViewDto?> GetEc${v.pascal}ByEcDetailIdAsync(long ecDetailId);
    /// <summary>更新${v.label}</summary>
    Task<TaktEcDeptViewDto> UpdateEc${v.pascal}Async(long ecDetailId, TaktEcDeptViewUpdateDto dto);
    /// <summary>导出${v.label}</summary>
    Task<(string fileName, byte[] fileContent)> ExportEc${v.pascal}Async(TaktEcDeptViewQueryDto? query = null, string? sheetName = null, string? fileName = null);
}
`;
}

function deptService(v) {
  return `// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEc${v.pascal}Service.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：设变${v.label}视图应用服务（DeptCode=${v.deptCode}）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Constants;
using Takt.Shared.Models;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变${v.label}视图应用服务
/// </summary>
public class TaktEc${v.pascal}Service : TaktEcDeptViewServiceBase, ITaktEc${v.pascal}Service
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public TaktEc${v.pascal}Service(
        ITaktCompanyRepository<TaktEcDetail> ecDetailRepository,
        ITaktCompanyRepository<TaktEcDept> ecDeptRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(TaktEcDeptCodes.${v.deptCode === 'Eng' ? 'Eng' : v.deptCode === 'Mp' ? 'Mp' : v.deptCode === 'Pmc' ? 'Pmc' : v.deptCode === 'Iqc' ? 'Iqc' : v.deptCode === 'Mc' ? 'Mc' : v.deptCode === 'Pcba' ? 'Pcba' : v.deptCode === 'Assy' ? 'Assy' : 'Qa'}, ecDetailRepository, ecDeptRepository, lineNumberGenerator, userContext, localizationService)
    {
    }

    /// <summary>获取${v.label}列表（分页）</summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>分页结果</returns>
    public Task<TaktPagedResult<TaktEcDeptViewDto>> GetEc${v.pascal}ListAsync(TaktEcDeptViewQueryDto queryDto) => GetDeptViewListAsync(queryDto);

    /// <summary>根据设变明细 ID 获取${v.label}行</summary>
    /// <param name="ecDetailId">设变明细 ID</param>
    /// <returns>部门视图 DTO</returns>
    public Task<TaktEcDeptViewDto?> GetEc${v.pascal}ByEcDetailIdAsync(long ecDetailId) => GetDeptViewByEcDetailIdAsync(ecDetailId);

    /// <summary>更新${v.label}</summary>
    /// <param name="ecDetailId">设变明细 ID</param>
    /// <param name="dto">更新 DTO</param>
    /// <returns>部门视图 DTO</returns>
    public Task<TaktEcDeptViewDto> UpdateEc${v.pascal}Async(long ecDetailId, TaktEcDeptViewUpdateDto dto) => UpdateDeptViewAsync(ecDetailId, dto);

    /// <summary>导出${v.label}</summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public Task<(string fileName, byte[] fileContent)> ExportEc${v.pascal}Async(TaktEcDeptViewQueryDto? query = null, string? sheetName = null, string? fileName = null) => ExportDeptViewAsync(query, sheetName, fileName);
}
`;
}

function deptController(v) {
  const perm = `logistics:manufacturing:engineeringchange:${v.slug.replace('-', '')}`;
  return `// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEc${v.pascal}sController.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：设变${v.label}视图控制器
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变${v.label}视图控制器
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "设变${v.label}")]
public class TaktEc${v.pascal}sController : TaktControllerBase
{
    private readonly ITaktEc${v.pascal}Service _service;

    /// <summary>
    /// 构造函数
    /// </summary>
    public TaktEc${v.pascal}sController(ITaktEc${v.pascal}Service service) => _service = service;

    /// <summary>获取${v.label}列表（分页）</summary>
    [TaktPermission("${perm}:list", "${v.label}列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetEc${v.pascal}ListAsync([FromQuery] TaktEcDeptViewQueryDto queryDto)
    {
        try { var result = await _service.GetEc${v.pascal}ListAsync(queryDto); return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功"); }
        catch (Exception ex) { return HandleException(ex); }
    }

    /// <summary>根据设变明细 ID 获取${v.label}行</summary>
    [TaktPermission("${perm}:query", "${v.label}详情")]
    [HttpGet("detail/{ecDetailId}")]
    public async Task<IActionResult> GetEc${v.pascal}ByEcDetailIdAsync(long ecDetailId)
    {
        try { var result = await _service.GetEc${v.pascal}ByEcDetailIdAsync(ecDetailId); if (result == null) return NotFound("${v.label}不存在"); return Success(result, "查询成功"); }
        catch (Exception ex) { return HandleException(ex); }
    }

    /// <summary>更新${v.label}</summary>
    [TaktPermission("${perm}:update", "更新${v.label}")]
    [HttpPut("detail/{ecDetailId}")]
    public async Task<IActionResult> UpdateEc${v.pascal}Async(long ecDetailId, [FromBody] TaktEcDeptViewUpdateDto dto)
    {
        try { var result = await _service.UpdateEc${v.pascal}Async(ecDetailId, dto); return Success(result, "更新成功"); }
        catch (Exception ex) { return HandleException(ex); }
    }

    /// <summary>导出${v.label}</summary>
    [TaktPermission("${perm}:export", "导出${v.label}")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportEc${v.pascal}Async([FromQuery] TaktEcDeptViewQueryDto? query)
    {
        try { var (fileName, fileContent) = await _service.ExportEc${v.pascal}Async(query); return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName); }
        catch (Exception ex) { return HandleException(ex); }
    }
}
`;
}

function specialController(v) {
  const permSlug = v.slug === 'legacy-product' ? 'legacyproduct' : v.slug;
  const perm = `logistics:manufacturing:engineeringchange:${permSlug}`;
  const updateBlock = v.updateMethod ? `
    /// <summary>更新${v.label}</summary>
    [TaktPermission("${perm}:update", "更新${v.label}")]
    [HttpPut("detail/{${v.getParam}}")]
    public async Task<IActionResult> ${v.updateMethod}(long ${v.getParam}, [FromBody] ${v.updateDto} dto)
    {
        try { var result = await _service.${v.updateMethod}(${v.getParam}, dto); return Success(result, "更新成功"); }
        catch (Exception ex) { return HandleException(ex); }
    }` : '';
  const getRoute = v.slug === 'kanban' ? '{ecId}' : `detail/{${v.getParam}}`;
  return `// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.EngineeringChange
// 文件名称：${v.controllerPlural}Controller.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：${v.label}控制器
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// ${v.label}控制器
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "${v.label}")]
public class ${v.controllerPlural}Controller : TaktControllerBase
{
    private readonly ITaktEc${v.pascal}Service _service;

    /// <summary>构造函数</summary>
    public ${v.controllerPlural}Controller(ITaktEc${v.pascal}Service service) => _service = service;

    /// <summary>获取${v.label}列表（分页）</summary>
    [TaktPermission("${perm}:list", "${v.label}列表")]
    [HttpGet("list")]
    public async Task<IActionResult> ${v.listMethod}([FromQuery] ${v.queryDto} queryDto)
    {
        try { var result = await _service.${v.listMethod}(queryDto); return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功"); }
        catch (Exception ex) { return HandleException(ex); }
    }

    /// <summary>获取${v.label}详情</summary>
    [TaktPermission("${perm}:query", "${v.label}详情")]
    [HttpGet("${getRoute}")]
    public async Task<IActionResult> ${v.getMethod}(long ${v.getParam})
    {
        try { var result = await _service.${v.getMethod}(${v.getParam}); if (result == null) return NotFound("${v.label}不存在"); return Success(result, "查询成功"); }
        catch (Exception ex) { return HandleException(ex); }
    }
${updateBlock}

    /// <summary>导出${v.label}</summary>
    [TaktPermission("${perm}:export", "导出${v.label}")]
    [HttpGet("export")]
    public async Task<IActionResult> ${v.exportMethod}([FromQuery] ${v.queryDto}? query)
    {
        try { var (fileName, fileContent) = await _service.${v.exportMethod}(query); return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName); }
        catch (Exception ex) { return HandleException(ex); }
    }
}
`;
}

function feApi(v, isDept) {
  const apiBase = isDept ? `TaktEc${v.pascal}s` : v.controllerPlural;
  const permSlug = isDept ? v.slug : (v.slug === 'legacy-product' ? 'legacyproduct' : v.slug);
  const listFn = isDept ? `getEc${v.pascal}List` : `getEc${v.pascal}List`;
  const getFn = isDept ? `getEc${v.pascal}ByEcDetailId` : `getEc${v.pascal}By${v.slug === 'kanban' ? 'EcId' : 'EcDetailId'}`;
  const updateFn = isDept ? `updateEc${v.pascal}` : (v.updateMethod ? `updateEc${v.pascal}` : null);
  const exportFn = isDept ? `exportEc${v.pascal}Data` : `exportEc${v.pascal}Data`;
  const queryType = isDept ? 'EcDeptViewQuery' : `Ec${v.pascal}Query`;
  const dtoType = isDept ? 'EcDeptView' : `Ec${v.pascal}`;
  const updateType = isDept ? 'EcDeptViewUpdate' : `Ec${v.pascal}Update`;
  const getParam = v.slug === 'kanban' ? 'ecId' : 'ecDetailId';
  const getRoute = v.slug === 'kanban' ? '${ecId}' : 'detail/${ecDetailId}';
  return `// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/manufacturing/engineering-change
// 文件名称：${isDept ? v.slug : v.slug}.ts
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：设变${v.label} API
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type { TaktPagedResult } from '@/types/common';
import type { ${dtoType}${isDept ? '' : ''}, ${queryType}${updateFn ? `, ${updateType}` : ''} } from '@/types/logistics/manufacturing/engineering-change/${isDept ? 'ec-dept-view' : v.slug}';

const ${apiBase.toUpperCase()}_API_BASE = '${apiBase}';

/**
 * 获取${v.label}列表（分页）
 */
export function ${listFn}(queryDto: ${queryType}) {
  return request.get<TaktPagedResult<${dtoType}>>(\`/\${${apiBase.toUpperCase()}_API_BASE}/list\`, { params: queryDto });
}

/**
 * 获取${v.label}详情
 */
export function ${getFn}(${getParam}: string) {
  return request.get<${dtoType}>(\`/\${${apiBase.toUpperCase()}_API_BASE}/${getRoute}\`);
}
${updateFn ? `
/**
 * 更新${v.label}
 */
export function ${updateFn}(${getParam}: string, dto: ${updateType}) {
  return request.put<${dtoType}>(\`/\${${apiBase.toUpperCase()}_API_BASE}/detail/\${${getParam}}\`, dto);
}` : ''}

/**
 * 导出${v.label}
 */
export function ${exportFn}(queryDto?: ${queryType}) {
  return request.get(\`/\${${apiBase.toUpperCase()}_API_BASE}/export\`, { params: queryDto, responseType: 'blob' });
}
`;
}

function typeFileHeader(v) {
  return `// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/engineering-change
// 文件名称：${v.slug}.d.ts
// 创建时间：2026-06-22
// 功能描述：设变${v.label}类型
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

`;
}

function feTypesSpecial(v) {
  if (v.slug === 'kanban') {
    return `${typeFileHeader(v)}import type { CompanyDtoBase, TaktPagedQuery } from '@/types/common';

export interface EcKanbanDeptStage {
  deptCode: string;
  implementedCount: number;
  totalCount: number;
}

export interface EcKanban extends CompanyDtoBase {
  ecId: string;
  ecNo: string;
  ecTitle: string;
  changeStatus: number;
  ecStatus: number;
  ecLeader: string;
  effectiveDate: string;
  detailCount: number;
  deptStages: EcKanbanDeptStage[];
}

export interface EcKanbanQuery extends TaktPagedQuery {
  ecNo?: string;
  changeStatus?: number;
  ecStatus?: number;
}
`;
  }
  const fields = v.slug === 'batch'
    ? `ecDetailId: string; ecNo: string; lineNumber: number; ecModel: string; ecNewItem?: string; scheduledBatch?: string; productionBatch?: string; scheduledProductionDate?: string; productionDate?: string;`
    : v.slug === 'kakunin'
      ? `ecDetailId: string; ecNo: string; lineNumber: number; ecModel: string; ecOldItem?: string; ecNewItem?: string; isCheck: number; isProcurement: number; ecChange?: string; ecNote?: string;`
      : `ecDetailId: string; ecNo: string; lineNumber: number; ecModel: string; ecOldItem?: string; ecOldText?: string; ecOldQty?: number; ecNewItem?: string; oldProductHandling?: string; isEndOfLine: number;`;
  const updateFields = v.slug === 'batch'
    ? `scheduledBatch?: string; productionBatch?: string; scheduledProductionDate?: string; productionDate?: string;`
    : v.slug === 'kakunin'
      ? `isCheck: number; isProcurement: number; ecNote?: string;`
      : `oldProductHandling?: string; isEndOfLine: number; remark?: string;`;
  return `${typeFileHeader(v)}import type { CompanyDtoBase, TaktPagedQuery } from '@/types/common';

export interface Ec${v.pascal} extends CompanyDtoBase {
  ${fields}
}

export interface Ec${v.pascal}Query extends TaktPagedQuery {
  ecNo?: string;
  ecModel?: string;
  ${v.slug === 'batch' ? 'batchNo?: string;' : v.slug === 'kakunin' ? 'isCheck?: number; ecNewItem?: string;' : 'ecOldItem?: string;'}
}

export interface Ec${v.pascal}Update {
  ecDetailId: string;
  ${updateFields}
}
`;
}

function feIndexVue(v, isDept) {
  const permSlug = isDept ? v.slug : (v.slug === 'legacy-product' ? 'legacyproduct' : v.slug);
  const perm = `logistics:manufacturing:engineeringchange:${permSlug}`;
  const listFn = isDept ? `getEc${v.pascal}List` : `getEc${v.pascal}List`;
  const updateFn = isDept ? `updateEc${v.pascal}` : (v.updateMethod ? `updateEc${v.pascal}` : null);
  const exportFn = isDept ? `exportEc${v.pascal}Data` : `exportEc${v.pascal}Data`;
  const idField = v.idField || 'ecDetailId';
  const getIdFn = `get${idField.charAt(0).toUpperCase() + idField.slice(1)}`;
  const formFile = isDept ? 'ec-dept-view-form' : `${v.slug}-form`;
  const formImportName = isDept ? 'EcDeptViewForm' : (v.slug === 'legacy-product' ? 'LegacyProductForm' : `${v.pascal}Form`);
  const formImport = updateFn ? `import ${formImportName} from './components/${formFile}.vue';` : '';
  const columns = v.slug === 'kanban'
    ? `{ title: t('entity.ec.ecno'), dataIndex: 'ecNo', key: 'ecNo', width: 120 }, { title: t('entity.ec.ectitle'), dataIndex: 'ecTitle', key: 'ecTitle', width: 200 }, { title: t('entity.ec.changestatus'), dataIndex: 'changeStatus', key: 'changeStatus', width: 100 }, { title: t('entity.ec.ecstatus'), dataIndex: 'ecStatus', key: 'ecStatus', width: 100 }, { title: t('entity.ec.ecleader'), dataIndex: 'ecLeader', key: 'ecLeader', width: 120 }`
    : `{ title: t('entity.ec.ecno'), dataIndex: 'ecNo', key: 'ecNo', width: 120 }, { title: t('entity.ecdetail.ecmodel'), dataIndex: 'ecModel', key: 'ecModel', width: 140 }, { title: t('entity.ecdetail.ecolditem'), dataIndex: 'ecOldItem', key: 'ecOldItem', width: 140 }, { title: t('entity.ecdetail.ecnewitem'), dataIndex: 'ecNewItem', key: 'ecNewItem', width: 140 }`;
  const rowType = isDept ? 'EcDeptView' : `Ec${v.pascal}`;
  const updateType = isDept ? 'EcDeptViewUpdate' : (updateFn ? `Ec${v.pascal}Update` : null);
  const typeImport = isDept
    ? `import type { EcDeptView, EcDeptViewUpdate } from '@/types/logistics/manufacturing/engineering-change/ec-dept-view';`
    : updateFn
      ? `import type { Ec${v.pascal}, Ec${v.pascal}Update } from '@/types/logistics/manufacturing/engineering-change/${v.slug}';`
      : `import type { Ec${v.pascal} } from '@/types/logistics/manufacturing/engineering-change/${v.slug}';`;
  const formStateBlock = updateFn
    ? `/** 表单可见 */
const formVisible = ref(false);
/** 表单 loading */
const formLoading = ref(false);
/** 编辑数据 */
const formData = ref<${rowType} | null>(null);
/** 表单 ref */
const formRef = ref<InstanceType<typeof ${formImportName}> | null>(null);`
    : '';
  const apiFile = isDept ? v.slug : v.slug;
  return `<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/${v.slug} -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：设变${v.label}页面 -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="p-4">
    <TaktQueryBar v-model="queryKeyword" :placeholder="t('common.page.form.placeholder.search')" :loading="loading" @search="handleSearch" @reset="handleReset" />
    <TaktToolsBar
      :show-create="false"
      :show-update="${!!updateFn}"
      :show-delete="false"
      :show-import="false"
      :show-export="true"
      update-permission="${perm}:update"
      export-permission="${perm}:export"
      :update-disabled="updateDisabled"
      :update-loading="loading"
      :refresh-loading="loading"
      @update="handleUpdate"
      @export="handleExport"
      @refresh="loadData"
    />
    <TaktSingleTable
      entity-scope="company"
      :columns="columns"
      :visible-column-keys="visibleColumnKeys"
      :id-column-key="'${idField}'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="${getIdFn}"
      :row-selection="rowSelection"
      :custom-row="onClickRow"
      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    />
    <TaktPagination v-model:current="currentPage" v-model:page-size="pageSize" :total="total" @change="handlePaginationChange" />
    ${updateFn ? `<TaktModal v-model:open="formVisible" :title="t('common.page.button.update')" width="900px" :confirm-loading="formLoading" @ok="handleFormSubmit">
      <${formImportName} ref="formRef" :form-data="formData" :loading="formLoading" />
    </TaktModal>` : ''}
  </div>
</template>

<script setup lang="ts">
/**
 * 设变${v.label}列表页
 */
import { message } from 'ant-design-vue';
import { useI18n } from 'vue-i18n';
import { ${listFn}${updateFn ? `, ${updateFn}` : ''}, ${exportFn} } from '@/api/logistics/manufacturing/engineering-change/${apiFile}';
${typeImport}
${formImport}

const { t } = useI18n();
/** 列表 loading */
const loading = ref(false);
/** 数据源 */
const dataSource = ref<${rowType}[]>([]);
/** 当前页 */
const currentPage = ref(1);
/** 每页条数 */
const pageSize = ref(20);
/** 总数 */
const total = ref(0);
/** 关键词 */
const queryKeyword = ref('');
/** 选中行 keys */
const selectedRowKeys = ref<(string | number)[]>([]);
/** 选中行 */
const selectedRows = ref<${rowType}[]>([]);
${formStateBlock}
/** 列定义 */
const columns = ref([
  ${columns}
]);
/** 可见列 keys */
const visibleColumnKeys = ref(columns.value.map(c => String(c.key)));
/** 行选择 */
const rowSelection = computed(() => ({ selectedRowKeys: selectedRowKeys.value, onChange: (keys: (string | number)[], rows: ${rowType}[]) => { selectedRowKeys.value = keys; selectedRows.value = rows; } }));
/** 更新按钮禁用 */
const updateDisabled = computed(() => selectedRowKeys.value.length !== 1);
/**
 * 行主键
 */
function ${getIdFn}(record: Record<string, unknown>) {
  return String(record.${idField} ?? '');
}
/** 加载列表 */
async function loadData() {
  loading.value = true;
  try {
    const res = await ${listFn}({ pageIndex: currentPage.value, pageSize: pageSize.value, keyWords: queryKeyword.value || undefined });
    dataSource.value = res.data ?? [];
    total.value = res.total ?? 0;
  } finally {
    loading.value = false;
  }
}
/** 搜索 */
function handleSearch() { currentPage.value = 1; loadData(); }
/** 重置 */
function handleReset() { queryKeyword.value = ''; currentPage.value = 1; loadData(); }
/** 分页变化 */
function handlePaginationChange() { loadData(); }
/** 表格变化 */
function handleTableChange() {}
/** 列宽变化 */
function handleResizeColumn() {}
/** 行点击 */
function onClickRow(record: Record<string, unknown>) {
  return { onClick: () => { const id = ${getIdFn}(record); selectedRowKeys.value = [id]; selectedRows.value = [record as unknown as ${rowType}]; } };
}
${updateFn ? `/** 编辑 */
async function handleUpdate() {
  const row = selectedRows.value[0];
  if (!row) return;
  formData.value = { ...row };
  formVisible.value = true;
}
/** 提交表单 */
async function handleFormSubmit() {
  if (!formRef.value || !formData.value) return;
  await formRef.value.validate();
  const dto: ${updateType} = formRef.value.getValues();
  formLoading.value = true;
  try {
    await ${updateFn}(String(formData.value.${idField}), dto);
    message.success(t('common.page.message.updateSuccess'));
    formVisible.value = false;
    await loadData();
  } finally {
    formLoading.value = false;
  }
}` : ''}
/** 导出 */
async function handleExport() {
  try {
    loading.value = true;
    const blob = await ${exportFn}({ pageIndex: currentPage.value, pageSize: pageSize.value, keyWords: queryKeyword.value || undefined });
    const url = window.URL.createObjectURL(blob as Blob);
    const link = document.createElement('a');
    link.href = url;
    link.download = '${v.label}.xlsx';
    link.style.display = 'none';
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
    setTimeout(() => window.URL.revokeObjectURL(url), 100);
  } finally {
    loading.value = false;
  }
}
useTableRefresh(loadData);
onMounted(loadData);
</script>
`;
}

// dept view shared types
const deptViewTypes = `// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/engineering-change
// 文件名称：ec-dept-view.d.ts
// 创建时间：2026-06-22
// 功能描述：设变部门视图共用类型；引用键 logistics.manufacturing.engineering-change
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { CompanyDtoBase, TaktPagedQuery } from '@/types/common';

export interface EcDeptView extends CompanyDtoBase {
  ecDeptId?: string;
  ecDetailId: string;
  ecId: string;
  ecNo: string;
  lineNumber: number;
  ecModel: string;
  ecChange?: string;
  ecOldItem?: string;
  ecNewItem?: string;
  ecOldText?: string;
  ecNewText?: string;
  deptCode: string;
  isImplemented: number;
  content?: string;
  scheduledProductionDate?: string;
  scheduledBatch?: string;
  poRemainder?: string;
  balance?: string;
  oldProductHandling?: string;
  purchaseOrderIssueDate?: string;
  supplier?: string;
  purchaseOrderNo?: string;
  iqcOrderNo?: string;
  inspectionDate?: string;
  outboundBatch?: string;
  outboundDate?: string;
  productionDate?: string;
  productionBatch?: string;
  outboundOrderNo?: string;
  productionTeam?: string;
  implementationDate?: string;
  inspectionBatch?: string;
  samplingNo?: string;
  isSopUpdated: number;
}

export interface EcDeptViewQuery extends TaktPagedQuery {
  ecNo?: string;
  ecModel?: string;
  isImplemented?: number;
  ecOldItem?: string;
  ecNewItem?: string;
}

export interface EcDeptViewUpdate {
  ecDetailId: string;
  isImplemented: number;
  content?: string;
  scheduledProductionDate?: string;
  scheduledBatch?: string;
  poRemainder?: string;
  balance?: string;
  oldProductHandling?: string;
  purchaseOrderIssueDate?: string;
  supplier?: string;
  purchaseOrderNo?: string;
  iqcOrderNo?: string;
  inspectionDate?: string;
  outboundBatch?: string;
  outboundDate?: string;
  productionDate?: string;
  productionBatch?: string;
  outboundOrderNo?: string;
  productionTeam?: string;
  implementationDate?: string;
  inspectionBatch?: string;
  samplingNo?: string;
  isSopUpdated: number;
  remark?: string;
}
`;

const deptFormVue = (v) => `<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/${v.slug}/components -->
<!-- 文件名称：ec-dept-view-form.vue -->
<!-- 功能描述：设变${v.label}表单；defineExpose validate/getValues/resetFields -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form ref="formRef" :model="formState" layout="horizontal" label-align="right" :label-col="{ span: 6 }" :wrapper-col="{ span: 16 }">
    <a-row :gutter="24">
      <a-col :span="12"><a-form-item :label="t('entity.ec.ecno')"><a-input v-model:value="formState.ecNo" disabled /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="t('entity.ecdetail.ecmodel')"><a-input v-model:value="formState.ecModel" disabled /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="t('entity.ecdept.isimplemented')"><TaktSelect v-model:value="formState.isImplemented" dict-type="sys_yes_no" /></a-form-item></a-col>
      <a-col :span="24"><a-form-item :label="t('entity.ecdept.content')" :label-col="{ span: 3 }" :wrapper-col="{ span: 20 }"><a-textarea v-model:value="formState.content" :rows="3" /></a-form-item></a-col>
    </a-row>
  </a-form>
</template>

<script setup lang="ts">
import { useI18n } from 'vue-i18n';
import type { EcDeptView, EcDeptViewUpdate } from '@/types/logistics/manufacturing/engineering-change/ec-dept-view';

const props = defineProps<{ formData?: EcDeptView | null; loading?: boolean }>();
const { t } = useI18n();
const formRef = ref();
const formState = reactive<EcDeptViewUpdate & { ecNo?: string; ecModel?: string }>({
  ecDetailId: '',
  isImplemented: 0,
  isSopUpdated: 0,
  content: '',
});

watch(() => props.formData, (val) => {
  if (!val) { resetFields(); return; }
  Object.assign(formState, {
    ecDetailId: val.ecDetailId,
    ecNo: val.ecNo,
    ecModel: val.ecModel,
    isImplemented: val.isImplemented ?? 0,
    content: val.content ?? '',
    isSopUpdated: val.isSopUpdated ?? 0,
    scheduledProductionDate: val.scheduledProductionDate,
    scheduledBatch: val.scheduledBatch,
    supplier: val.supplier,
    purchaseOrderNo: val.purchaseOrderNo,
    iqcOrderNo: val.iqcOrderNo,
    outboundBatch: val.outboundBatch,
    productionBatch: val.productionBatch,
    productionTeam: val.productionTeam,
    inspectionBatch: val.inspectionBatch,
    samplingNo: val.samplingNo,
  });
}, { immediate: true });

async function validate() { await formRef.value?.validate(); }
function getValues(): EcDeptViewUpdate {
  const { ecNo, ecModel, ...rest } = formState;
  return rest;
}
function resetFields() {
  Object.assign(formState, { ecDetailId: '', isImplemented: 0, isSopUpdated: 0, content: '', ecNo: '', ecModel: '' });
}
defineExpose({ validate, getValues, resetFields });
</script>
`;

function specialFormVue(v) {
  if (v.slug === 'batch') {
    return `<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/batch/components -->
<!-- 文件名称：batch-form.vue -->
<!-- 功能描述：投入批次编辑表单；defineExpose validate/getValues/resetFields -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form ref="formRef" :model="formState" layout="horizontal" label-align="right" :label-col="{ span: 6 }" :wrapper-col="{ span: 16 }">
    <a-row :gutter="24">
      <a-col :span="12"><a-form-item :label="t('entity.ec.ecno')"><a-input v-model:value="formState.ecNo" disabled /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="t('entity.ecdetail.ecmodel')"><a-input v-model:value="formState.ecModel" disabled /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="t('entity.ecdept.scheduledbatch')"><a-input v-model:value="formState.scheduledBatch" /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="t('entity.ecdept.productionbatch')"><a-input v-model:value="formState.productionBatch" /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="t('entity.ecdept.scheduledproductiondate')"><a-date-picker v-model:value="formState.scheduledProductionDate" value-format="YYYY-MM-DD" class="w-full" /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="t('entity.ecdept.productiondate')"><a-date-picker v-model:value="formState.productionDate" value-format="YYYY-MM-DD" class="w-full" /></a-form-item></a-col>
    </a-row>
  </a-form>
</template>

<script setup lang="ts">
import { useI18n } from 'vue-i18n';
import type { EcBatch, EcBatchUpdate } from '@/types/logistics/manufacturing/engineering-change/batch';

const props = defineProps<{ formData?: EcBatch | null; loading?: boolean }>();
const { t } = useI18n();
const formRef = ref();
const formState = reactive<EcBatchUpdate & { ecNo?: string; ecModel?: string }>({
  ecDetailId: '',
  scheduledBatch: '',
  productionBatch: '',
});

watch(() => props.formData, (val) => {
  if (!val) { resetFields(); return; }
  Object.assign(formState, {
    ecDetailId: val.ecDetailId,
    ecNo: val.ecNo,
    ecModel: val.ecModel,
    scheduledBatch: val.scheduledBatch ?? '',
    productionBatch: val.productionBatch ?? '',
    scheduledProductionDate: val.scheduledProductionDate,
    productionDate: val.productionDate,
  });
}, { immediate: true });

async function validate() { await formRef.value?.validate(); }
function getValues(): EcBatchUpdate {
  const { ecNo, ecModel, ...rest } = formState;
  return rest;
}
function resetFields() {
  Object.assign(formState, { ecDetailId: '', scheduledBatch: '', productionBatch: '', ecNo: '', ecModel: '' });
}
defineExpose({ validate, getValues, resetFields });
</script>
`;
  }
  if (v.slug === 'kakunin') {
    return `<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/kakunin/components -->
<!-- 文件名称：kakunin-form.vue -->
<!-- 功能描述：物料确认编辑表单；defineExpose validate/getValues/resetFields -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form ref="formRef" :model="formState" layout="horizontal" label-align="right" :label-col="{ span: 6 }" :wrapper-col="{ span: 16 }">
    <a-row :gutter="24">
      <a-col :span="12"><a-form-item :label="t('entity.ec.ecno')"><a-input v-model:value="formState.ecNo" disabled /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="t('entity.ecdetail.ecmodel')"><a-input v-model:value="formState.ecModel" disabled /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="t('entity.ecdetail.ischeck')"><TaktSelect v-model:value="formState.isCheck" dict-type="sys_yes_no" /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="t('entity.ecdetail.isprocurement')"><TaktSelect v-model:value="formState.isProcurement" dict-type="sys_yes_no" /></a-form-item></a-col>
      <a-col :span="24"><a-form-item :label="t('entity.ecdetail.ecnote')" :label-col="{ span: 3 }" :wrapper-col="{ span: 20 }"><a-textarea v-model:value="formState.ecNote" :rows="3" /></a-form-item></a-col>
    </a-row>
  </a-form>
</template>

<script setup lang="ts">
import { useI18n } from 'vue-i18n';
import type { EcKakunin, EcKakuninUpdate } from '@/types/logistics/manufacturing/engineering-change/kakunin';

const props = defineProps<{ formData?: EcKakunin | null; loading?: boolean }>();
const { t } = useI18n();
const formRef = ref();
const formState = reactive<EcKakuninUpdate & { ecNo?: string; ecModel?: string }>({
  ecDetailId: '',
  isCheck: 0,
  isProcurement: 0,
});

watch(() => props.formData, (val) => {
  if (!val) { resetFields(); return; }
  Object.assign(formState, {
    ecDetailId: val.ecDetailId,
    ecNo: val.ecNo,
    ecModel: val.ecModel,
    isCheck: val.isCheck ?? 0,
    isProcurement: val.isProcurement ?? 0,
    ecNote: val.ecNote ?? '',
  });
}, { immediate: true });

async function validate() { await formRef.value?.validate(); }
function getValues(): EcKakuninUpdate {
  const { ecNo, ecModel, ...rest } = formState;
  return rest;
}
function resetFields() {
  Object.assign(formState, { ecDetailId: '', isCheck: 0, isProcurement: 0, ecNote: '', ecNo: '', ecModel: '' });
}
defineExpose({ validate, getValues, resetFields });
</script>
`;
  }
  if (v.slug === 'legacy-product') {
    return `<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/legacy-product/components -->
<!-- 文件名称：legacy-product-form.vue -->
<!-- 功能描述：旧品管制编辑表单；defineExpose validate/getValues/resetFields -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form ref="formRef" :model="formState" layout="horizontal" label-align="right" :label-col="{ span: 6 }" :wrapper-col="{ span: 16 }">
    <a-row :gutter="24">
      <a-col :span="12"><a-form-item :label="t('entity.ec.ecno')"><a-input v-model:value="formState.ecNo" disabled /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="t('entity.ecdetail.ecolditem')"><a-input v-model:value="formState.ecOldItem" disabled /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="t('entity.ecdept.oldproducthandling')"><a-input v-model:value="formState.oldProductHandling" /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="t('entity.ecdetail.isendofline')"><TaktSelect v-model:value="formState.isEndOfLine" dict-type="sys_yes_no" /></a-form-item></a-col>
      <a-col :span="24"><a-form-item :label="t('entity.ec.remark')" :label-col="{ span: 3 }" :wrapper-col="{ span: 20 }"><a-textarea v-model:value="formState.remark" :rows="3" /></a-form-item></a-col>
    </a-row>
  </a-form>
</template>

<script setup lang="ts">
import { useI18n } from 'vue-i18n';
import type { EcLegacyProduct, EcLegacyProductUpdate } from '@/types/logistics/manufacturing/engineering-change/legacy-product';

const props = defineProps<{ formData?: EcLegacyProduct | null; loading?: boolean }>();
const { t } = useI18n();
const formRef = ref();
const formState = reactive<EcLegacyProductUpdate & { ecNo?: string; ecOldItem?: string }>({
  ecDetailId: '',
  isEndOfLine: 0,
  oldProductHandling: '',
});

watch(() => props.formData, (val) => {
  if (!val) { resetFields(); return; }
  Object.assign(formState, {
    ecDetailId: val.ecDetailId,
    ecNo: val.ecNo,
    ecOldItem: val.ecOldItem ?? '',
    oldProductHandling: val.oldProductHandling ?? '',
    isEndOfLine: val.isEndOfLine ?? 0,
    remark: val.remark ?? '',
  });
}, { immediate: true });

async function validate() { await formRef.value?.validate(); }
function getValues(): EcLegacyProductUpdate {
  const { ecNo, ecOldItem, ...rest } = formState;
  return rest;
}
function resetFields() {
  Object.assign(formState, { ecDetailId: '', isEndOfLine: 0, oldProductHandling: '', remark: '', ecNo: '', ecOldItem: '' });
}
defineExpose({ validate, getValues, resetFields });
</script>
`;
  }
  return null;
}

function main() {
  console.log('Generating EC view stack...');
  writeFile(path.join(FE_TYPES, 'ec-dept-view.d.ts'), deptViewTypes);

  for (const v of DEPT_VIEWS) {
    writeFile(path.join(APP_SVC, `ITaktEc${v.pascal}Service.cs`), deptInterface(v));
    writeFile(path.join(APP_SVC, `TaktEc${v.pascal}Service.cs`), deptService(v));
    writeFile(path.join(WEB_CTRL, `TaktEc${v.pascal}sController.cs`), deptController(v));
    writeFile(path.join(FE_API, `${v.slug}.ts`), feApi(v, true));
    writeFile(path.join(FE_VIEWS, v.slug, 'index.vue'), feIndexVue(v, true));
    writeFile(path.join(FE_VIEWS, v.slug, 'components', 'ec-dept-view-form.vue'), deptFormVue(v));
  }

  for (const v of SPECIAL_VIEWS) {
    writeFile(path.join(WEB_CTRL, `${v.controllerPlural}Controller.cs`), specialController(v));
    writeFile(path.join(FE_TYPES, `${v.slug}.d.ts`), feTypesSpecial(v));
    writeFile(path.join(FE_API, `${v.slug}.ts`), feApi(v, false));
    writeFile(path.join(FE_VIEWS, v.slug, 'index.vue'), feIndexVue(v, false));
    const formContent = specialFormVue(v);
    if (formContent) {
      writeFile(path.join(FE_VIEWS, v.slug, 'components', `${v.slug}-form.vue`), formContent);
    }
  }

  console.log('Done.');
}

main();
