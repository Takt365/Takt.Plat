// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Sop
// 文件名称：TaktSopExecStepService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：SOP工步执行明细应用服务实现
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
/// SOP工步执行明细应用服务
/// </summary>
public class TaktSopExecStepService : TaktServiceBase, ITaktSopExecStepService
{
    private readonly ITaktCompanyRepository<TaktSopExecStep> _sopExecStepRepository;
    private readonly ITaktCompanyRepository<TaktSopExec> _sopExecRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="sopExecStepRepository">SOP工步执行明细仓储</param>
    /// <param name="sopExecRepository">SOP工位执行仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSopExecStepService(
        ITaktCompanyRepository<TaktSopExecStep> sopExecStepRepository,
        ITaktCompanyRepository<TaktSopExec> sopExecRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _sopExecStepRepository = sopExecStepRepository;
        _sopExecRepository = sopExecRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取SOP工步执行明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSopExecStepDto>> GetSopExecStepListAsync(TaktSopExecStepQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _sopExecStepRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSopExecStepDto>.Create(
            data.Adapt<List<TaktSopExecStepDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取SOP工步执行明细
    /// </summary>
    /// <param name="id">SOP工步执行明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopExecStepDto?> GetSopExecStepByIdAsync(long id)
    {
        var entity = await _sopExecStepRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktSopExecStepDto>();
    }

    /// <summary>
    /// 获取SOP工步执行明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSopExecStepOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _sopExecStepRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.Id,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建SOP工步执行明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopExecStepDto> CreateSopExecStepAsync(TaktSopExecStepCreateDto dto)
    {
        var entity = dto.Adapt<TaktSopExecStep>();
        await StampSopExecStepSopExecAsync(entity, dto);
        entity = await _sopExecStepRepository.CreateAsync(entity);
        return await GetSopExecStepByIdAsync(entity.Id) ?? entity.Adapt<TaktSopExecStepDto>();
    }

    /// <summary>
    /// 更新SOP工步执行明细
    /// </summary>
    /// <param name="id">SOP工步执行明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopExecStepDto> UpdateSopExecStepAsync(long id, TaktSopExecStepUpdateDto dto)
    {
        var entity = await _sopExecStepRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("SOP工步执行明细不存在");
        }
        dto.Adapt(entity);
        await StampSopExecStepSopExecAsync(entity, dto);
        await _sopExecStepRepository.UpdateAsync(entity);
        return await GetSopExecStepByIdAsync(id) ?? throw new TaktBusinessException("SOP工步执行明细不存在");
    }

    /// <summary>
    /// 删除SOP工步执行明细
    /// </summary>
    /// <param name="id">SOP工步执行明细ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSopExecStepByIdAsync(long id)
    {
        var deleted = await _sopExecStepRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("SOP工步执行明细不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除SOP工步执行明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSopExecStepBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSopExecStepByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSopExecStepTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSopExecStepTemplateDto>(
            sheetName ?? "SOP工步执行明细导入模板",
            fileName ?? "SOP工步执行明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入SOP工步执行明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSopExecStepAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSopExecStepImportDto>(fileStream, sheetName ?? "SOP工步执行明细导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktSopExecStep>();
                var importDto = rows[i].Adapt<TaktSopExecStepCreateDto>();
                await StampSopExecStepSopExecAsync(entity, importDto);
                await _sopExecStepRepository.CreateAsync(entity);
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
    /// 导出SOP工步执行明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSopExecStepAsync(TaktSopExecStepQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktSopExecStepQueryDto());
        var list = await _sopExecStepRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSopExecStepExportDto>(),
                sheetName ?? "SOP工步执行明细数据",
                fileName ?? "SOP工步执行明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSopExecStepExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "SOP工步执行明细数据",
            fileName ?? "SOP工步执行明细导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步SOP工步执行明细主表外键（ManyToOne → SOP工位执行）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampSopExecStepSopExecAsync(TaktSopExecStep entity, TaktSopExecStepCreateDto dto)
    {
        if (dto.ExecId <= 0)
        {
            return;
        }
        var master = await _sopExecRepository.GetByIdAsync(dto.ExecId);
        if (master == null)
        {
            throw new TaktBusinessException("SOP工位执行不存在");
        }
        entity.ExecId = master.Id;
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建SOP工步执行明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSopExecStep, bool>> QueryExpression(TaktSopExecStepQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSopExecStep>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.ExecId).Contains(keywords)
                || SqlFunc.ToString(x.StepId).Contains(keywords)
                || SqlFunc.ToString(x.StepNo).Contains(keywords)
                || SqlFunc.ToString(x.StepResult).Contains(keywords)
                || SqlFunc.ToString(x.ConfirmedBy).Contains(keywords)
                || SqlFunc.ToString(x.BlockNextStep).Contains(keywords)
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.StartedAt).Contains(keywords)
                || SqlFunc.ToString(x.EndedAt).Contains(keywords)
                || SqlFunc.ToString(x.ConfirmedAt).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.ExecId.HasValue == true)
        {
            exp = exp.And(x => x.ExecId == queryDto.ExecId);
        }

        if (queryDto?.StepId.HasValue == true)
        {
            exp = exp.And(x => x.StepId == queryDto.StepId);
        }

        if (queryDto?.StepNo.HasValue == true)
        {
            exp = exp.And(x => x.StepNo == queryDto.StepNo);
        }

        if (queryDto?.StepResult.HasValue == true)
        {
            exp = exp.And(x => x.StepResult == queryDto.StepResult);
        }

        if (queryDto?.ConfirmedBy.HasValue == true)
        {
            exp = exp.And(x => x.ConfirmedBy == queryDto.ConfirmedBy);
        }

        if (queryDto?.BlockNextStep.HasValue == true)
        {
            exp = exp.And(x => x.BlockNextStep == queryDto.BlockNextStep);
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

        if (queryDto?.ConfirmedAtStart.HasValue == true)
        {
            exp = exp.And(x => x.ConfirmedAt >= queryDto.ConfirmedAtStart);
        }

        if (queryDto?.ConfirmedAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.ConfirmedAt <= queryDto.ConfirmedAtEnd);
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
