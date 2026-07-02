// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Sop
// 文件名称：TaktSopExecService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：SOP工位执行应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Sop;
using Takt.Domain.Entities.Logistics.Manufacturing.Sop;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Sop;

/// <summary>
/// SOP工位执行应用服务
/// </summary>
public class TaktSopExecService : TaktServiceBase, ITaktSopExecService
{
    private readonly ITaktCompanyRepository<TaktSopExec> _sopExecRepository;
    private readonly ITaktCompanyRepository<TaktSopExecStep> _sopExecStepRepository;
    private readonly ITaktCompanyRepository<TaktSopExecScan> _sopExecScanRepository;
    private readonly ITaktCompanyRepository<TaktSopArgument> _sopArgumentRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="sopExecRepository">SOP工位执行仓储</param>
    /// <param name="sopExecStepRepository">SopExecStep仓储</param>
    /// <param name="sopExecScanRepository">SopExecScan仓储</param>
    /// <param name="sopArgumentRepository">SopArgument仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSopExecService(
        ITaktCompanyRepository<TaktSopExec> sopExecRepository,
        ITaktCompanyRepository<TaktSopExecStep> sopExecStepRepository,
        ITaktCompanyRepository<TaktSopExecScan> sopExecScanRepository,
        ITaktCompanyRepository<TaktSopArgument> sopArgumentRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _sopExecRepository = sopExecRepository;
        _sopExecStepRepository = sopExecStepRepository;
        _sopExecScanRepository = sopExecScanRepository;
        _sopArgumentRepository = sopArgumentRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取SOP工位执行列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSopExecDto>> GetSopExecListAsync(TaktSopExecQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _sopExecRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSopExecDto>.Create(
            data.Adapt<List<TaktSopExecDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取SOP工位执行
    /// </summary>
    /// <param name="id">SOP工位执行ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopExecDto?> GetSopExecByIdAsync(long id)
    {
        var entity = await _sopExecRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktSopExecDto>();
        await FillSopExecDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取SOP工位执行选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSopExecOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _sopExecRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ExecStatus == 1,
            x => x.MaterialCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.MaterialCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建SOP工位执行
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopExecDto> CreateSopExecAsync(TaktSopExecCreateDto dto)
    {
        var entity = dto.Adapt<TaktSopExec>();
        entity = await _sopExecRepository.CreateAsync(entity);
                await SaveSopExecChildrenAsync(entity, dto);
        return await GetSopExecByIdAsync(entity.Id) ?? entity.Adapt<TaktSopExecDto>();
    }

    /// <summary>
    /// 更新SOP工位执行
    /// </summary>
    /// <param name="id">SOP工位执行ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopExecDto> UpdateSopExecAsync(long id, TaktSopExecUpdateDto dto)
    {
        var entity = await _sopExecRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("SOP工位执行不存在");
        }
        dto.Adapt(entity);
        await _sopExecRepository.UpdateAsync(entity);
                await SaveSopExecChildrenAsync(entity, dto);
        return await GetSopExecByIdAsync(id) ?? throw new TaktBusinessException("SOP工位执行不存在");
    }

    /// <summary>
    /// 删除SOP工位执行
    /// </summary>
    /// <param name="id">SOP工位执行ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSopExecByIdAsync(long id)
    {
        var entity = await _sopExecRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("SOP工位执行不存在或已删除");
        }
        await _sopExecStepRepository.DeleteAsync(x => x.ExecId == entity.Id);
        await _sopExecScanRepository.DeleteAsync(x => x.ExecId == entity.Id);
        await _sopArgumentRepository.DeleteAsync(x => x.ExecId == entity.Id);
        var deleted = await _sopExecRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("SOP工位执行不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除SOP工位执行
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSopExecBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSopExecByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新SOP工位执行状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopExecDto> UpdateSopExecStatusAsync(TaktSopExecStatusDto dto)
    {
        var entity = await _sopExecRepository.GetByIdAsync(dto.SopExecId);
        if (entity == null)
        {
            throw new TaktBusinessException("SOP工位执行不存在");
        }
        entity.ExecStatus = dto.ExecStatus;
        await _sopExecRepository.UpdateAsync(entity);
        return await GetSopExecByIdAsync(dto.SopExecId) ?? throw new TaktBusinessException("SOP工位执行不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSopExecTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSopExecTemplateDto>(
            sheetName ?? "SOP工位执行导入模板",
            fileName ?? "SOP工位执行导入模板.xlsx");
    }

    /// <summary>
    /// 导入SOP工位执行
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSopExecAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSopExecImportDto>(fileStream, sheetName ?? "SOP工位执行导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktSopExec>();
                await _sopExecRepository.CreateAsync(entity);
                success += 1;
            }
            catch (Exception ex)
            {
                fail += 1;
                errors.Add($"第{i + 2}行: {ex.Message}");
            }
        }
        return (success, fail, errors);
    }

    /// <summary>
    /// 导出SOP工位执行
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSopExecAsync(TaktSopExecQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktSopExecQueryDto());
        var list = await _sopExecRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSopExecExportDto>(),
                sheetName ?? "SOP工位执行数据",
                fileName ?? "SOP工位执行导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSopExecExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "SOP工位执行数据",
            fileName ?? "SOP工位执行导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充SOP工位执行详情（加载 OneToMany 子表：SOP工步执行明细、SOP物料扫码记录、SOP作业参数）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillSopExecDetailsAsync(TaktSopExecDto dto, TaktSopExec entity)
    {
        if (dto == null)
        {
            return;
        }
        // SOP工步执行明细 → dto.Steps
        var steps = await _sopExecStepRepository.GetListAsync(x => x.ExecId == entity.Id);
        dto.Steps = steps.Adapt<List<TaktSopExecStepDto>>();
        // SOP物料扫码记录 → dto.Scans
        var scans = await _sopExecScanRepository.GetListAsync(x => x.ExecId == entity.Id);
        dto.Scans = scans.Adapt<List<TaktSopExecScanDto>>();
        // SOP作业参数 → dto.Arguments
        var arguments = await _sopArgumentRepository.GetListAsync(x => x.ExecId == entity.Id);
        dto.Arguments = arguments.Adapt<List<TaktSopArgumentDto>>();
    }

    /// <summary>
    /// 保存SOP工位执行子表级联（SOP工步执行明细、SOP物料扫码记录、SOP作业参数；Create/Update 后按主表 Id 先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveSopExecChildrenAsync(TaktSopExec entity, TaktSopExecCreateDto dto)
    {
        // SOP工步执行明细（Steps）
        if (dto.Steps is not { Count: > 0 })
        {
            await _sopExecStepRepository.DeleteAsync(x => x.ExecId == entity.Id);
        }
        else
        {
            var steps = dto.Steps.Adapt<List<TaktSopExecStep>>();
            foreach (var child in steps)
            {
                child.ExecId = entity.Id;
            }
            await _sopExecStepRepository.DeleteAsync(x => x.ExecId == entity.Id);
            foreach (var child in steps)
            {
            }
            await _sopExecStepRepository.CreateRangeAsync(steps);
        }
        // SOP物料扫码记录（Scans）
        if (dto.Scans is not { Count: > 0 })
        {
            await _sopExecScanRepository.DeleteAsync(x => x.ExecId == entity.Id);
        }
        else
        {
            var scans = dto.Scans.Adapt<List<TaktSopExecScan>>();
            foreach (var child in scans)
            {
                child.ExecId = entity.Id;
            }
            await _sopExecScanRepository.DeleteAsync(x => x.ExecId == entity.Id);
            foreach (var child in scans)
            {
            }
            await _sopExecScanRepository.CreateRangeAsync(scans);
        }
        // SOP作业参数（Arguments）
        if (dto.Arguments is not { Count: > 0 })
        {
            await _sopArgumentRepository.DeleteAsync(x => x.ExecId == entity.Id);
        }
        else
        {
            var arguments = dto.Arguments.Adapt<List<TaktSopArgument>>();
            foreach (var child in arguments)
            {
                child.ExecId = entity.Id;
            }
            await _sopArgumentRepository.DeleteAsync(x => x.ExecId == entity.Id);
            foreach (var child in arguments)
            {
            }
            await _sopArgumentRepository.CreateRangeAsync(arguments);
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建SOP工位执行查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSopExec, bool>> QueryExpression(TaktSopExecQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSopExec>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || SqlFunc.ToString(x.ProductionOrderId).Contains(keywords)
                || (x.WorkOrderNo != null && x.WorkOrderNo.Contains(keywords))
                || (x.SerialNumber != null && x.SerialNumber.Contains(keywords))
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || SqlFunc.ToString(x.RoutingItemId).Contains(keywords)
                || SqlFunc.ToString(x.ProcessSegmentType).Contains(keywords)
                || SqlFunc.ToString(x.WorkstationId).Contains(keywords)
                || SqlFunc.ToString(x.EmployeeId).Contains(keywords)
                || SqlFunc.ToString(x.SopId).Contains(keywords)
                || SqlFunc.ToString(x.RevisionId).Contains(keywords)
                || (x.Revision != null && x.Revision.Contains(keywords))
                || (x.ContentLang != null && x.ContentLang.Contains(keywords))
                || SqlFunc.ToString(x.SelfCheckResult).Contains(keywords)
                || SqlFunc.ToString(x.ExecStatus).Contains(keywords)
                || SqlFunc.ToString(x.CurrentStepId).Contains(keywords)
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.StartedAt).Contains(keywords)
                || SqlFunc.ToString(x.EndedAt).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (queryDto?.ProductionOrderId.HasValue == true)
        {
            exp = exp.And(x => x.ProductionOrderId == queryDto.ProductionOrderId);
        }

        if (!string.IsNullOrEmpty(queryDto?.WorkOrderNo))
        {
            exp = exp.And(x => x.WorkOrderNo != null && x.WorkOrderNo.Contains(queryDto.WorkOrderNo));
        }

        if (!string.IsNullOrEmpty(queryDto?.SerialNumber))
        {
            exp = exp.And(x => x.SerialNumber != null && x.SerialNumber.Contains(queryDto.SerialNumber));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialCode))
        {
            exp = exp.And(x => x.MaterialCode != null && x.MaterialCode.Contains(queryDto.MaterialCode));
        }

        if (queryDto?.RoutingItemId.HasValue == true)
        {
            exp = exp.And(x => x.RoutingItemId == queryDto.RoutingItemId);
        }

        if (queryDto?.ProcessSegmentType.HasValue == true)
        {
            exp = exp.And(x => x.ProcessSegmentType == queryDto.ProcessSegmentType);
        }

        if (queryDto?.WorkstationId.HasValue == true)
        {
            exp = exp.And(x => x.WorkstationId == queryDto.WorkstationId);
        }

        if (queryDto?.EmployeeId.HasValue == true)
        {
            exp = exp.And(x => x.EmployeeId == queryDto.EmployeeId);
        }

        if (queryDto?.SopId.HasValue == true)
        {
            exp = exp.And(x => x.SopId == queryDto.SopId);
        }

        if (queryDto?.RevisionId.HasValue == true)
        {
            exp = exp.And(x => x.RevisionId == queryDto.RevisionId);
        }

        if (!string.IsNullOrEmpty(queryDto?.Revision))
        {
            exp = exp.And(x => x.Revision != null && x.Revision.Contains(queryDto.Revision));
        }

        if (!string.IsNullOrEmpty(queryDto?.ContentLang))
        {
            exp = exp.And(x => x.ContentLang != null && x.ContentLang.Contains(queryDto.ContentLang));
        }

        if (queryDto?.SelfCheckResult.HasValue == true)
        {
            exp = exp.And(x => x.SelfCheckResult == queryDto.SelfCheckResult);
        }

        if (queryDto?.ExecStatus.HasValue == true)
        {
            exp = exp.And(x => x.ExecStatus == queryDto.ExecStatus);
        }

        if (queryDto?.CurrentStepId.HasValue == true)
        {
            exp = exp.And(x => x.CurrentStepId == queryDto.CurrentStepId);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.StartedAtStart.HasValue == true)
        {
            exp = exp.And(x => x.StartedAt >= queryDto.StartedAtStart);
        }

        if (queryDto?.StartedAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.StartedAt <= queryDto.StartedAtEnd);
        }

        if (queryDto?.EndedAtStart.HasValue == true)
        {
            exp = exp.And(x => x.EndedAt >= queryDto.EndedAtStart);
        }

        if (queryDto?.EndedAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.EndedAt <= queryDto.EndedAtEnd);
        }

        if (queryDto?.CreatedAtStart.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt >= queryDto.CreatedAtStart);
        }

        if (queryDto?.CreatedAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt <= queryDto.CreatedAtEnd);
        }

        return exp.ToExpression();
    }
}
