// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.HumanResource.Performance
// 文件名称：TaktObjectiveService.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：绩效目标应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.HumanResource.Performance;
using Takt.Domain.Entities.HumanResource.Performance;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.HumanResource.Performance;

/// <summary>
/// 绩效目标应用服务
/// </summary>
public class TaktObjectiveService : TaktServiceBase, ITaktObjectiveService
{
    private readonly ITaktApprovalRepository<TaktObjective> _objectiveRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="objectiveRepository">绩效目标仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktObjectiveService(
        ITaktApprovalRepository<TaktObjective> objectiveRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _objectiveRepository = objectiveRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取绩效目标列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktObjectiveDto>> GetObjectiveListAsync(TaktObjectiveQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _objectiveRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktObjectiveDto>.Create(
            data.Adapt<List<TaktObjectiveDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取绩效目标
    /// </summary>
    /// <param name="id">绩效目标ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktObjectiveDto?> GetObjectiveByIdAsync(long id)
    {
        var entity = await _objectiveRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktObjectiveDto>();
    }

    /// <summary>
    /// 获取绩效目标选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetObjectiveOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _objectiveRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.EmployeeName,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.EmployeeName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建绩效目标
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktObjectiveDto> CreateObjectiveAsync(TaktObjectiveCreateDto dto)
    {
        var entity = dto.Adapt<TaktObjective>();
        entity = await _objectiveRepository.CreateAsync(entity);
        return await GetObjectiveByIdAsync(entity.Id) ?? entity.Adapt<TaktObjectiveDto>();
    }

    /// <summary>
    /// 更新绩效目标
    /// </summary>
    /// <param name="id">绩效目标ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktObjectiveDto> UpdateObjectiveAsync(long id, TaktObjectiveUpdateDto dto)
    {
        var entity = await _objectiveRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("绩效目标不存在");
        }
        dto.Adapt(entity);
        await _objectiveRepository.UpdateAsync(entity);
        return await GetObjectiveByIdAsync(id) ?? throw new TaktBusinessException("绩效目标不存在");
    }

    /// <summary>
    /// 删除绩效目标
    /// </summary>
    /// <param name="id">绩效目标ID</param>
    /// <returns>任务</returns>
    public async Task DeleteObjectiveByIdAsync(long id)
    {
        var deleted = await _objectiveRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("绩效目标不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除绩效目标
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteObjectiveBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteObjectiveByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新绩效目标状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktObjectiveDto> UpdateObjectiveStatusAsync(TaktObjectiveStatusDto dto)
    {
        var entity = await _objectiveRepository.GetByIdAsync(dto.ObjectiveId);
        if (entity == null)
        {
            throw new TaktBusinessException("绩效目标不存在");
        }
        entity.ObjectiveStatus = dto.ObjectiveStatus;
        await _objectiveRepository.UpdateAsync(entity);
        return await GetObjectiveByIdAsync(dto.ObjectiveId) ?? throw new TaktBusinessException("绩效目标不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetObjectiveTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktObjectiveTemplateDto>(
            sheetName ?? "绩效目标导入模板",
            fileName ?? "绩效目标导入模板.xlsx");
    }

    /// <summary>
    /// 导入绩效目标
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportObjectiveAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktObjectiveImportDto>(fileStream, sheetName ?? "绩效目标导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktObjective>();
                await _objectiveRepository.CreateAsync(entity);
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
    /// 导出绩效目标
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportObjectiveAsync(TaktObjectiveQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktObjectiveQueryDto());
        var list = await _objectiveRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktObjectiveExportDto>(),
                sheetName ?? "绩效目标数据",
                fileName ?? "绩效目标导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktObjectiveExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "绩效目标数据",
            fileName ?? "绩效目标导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建绩效目标查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktObjective, bool>> QueryExpression(TaktObjectiveQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktObjective>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.EmployeeId).Contains(keywords)
                || (x.EmployeeName != null && x.EmployeeName.Contains(keywords))
                || SqlFunc.ToString(x.SchemeMetricId).Contains(keywords)
                || (x.ObjectivePeriod != null && x.ObjectivePeriod.Contains(keywords))
                || (x.ObjectiveDescription != null && x.ObjectiveDescription.Contains(keywords))
                || SqlFunc.ToString(x.TargetValue).Contains(keywords)
                || SqlFunc.ToString(x.ActualValue).Contains(keywords)
                || SqlFunc.ToString(x.CompletionPercentage).Contains(keywords)
                || SqlFunc.ToString(x.ObjectiveWeight).Contains(keywords)
                || (x.AchievementNotes != null && x.AchievementNotes.Contains(keywords))
                || SqlFunc.ToString(x.ObjectiveStatus).Contains(keywords)
                || (x.RelatedPlant != null && x.RelatedPlant.Contains(keywords))
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.StartDate).Contains(keywords)
                || SqlFunc.ToString(x.DueDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.EmployeeId.HasValue == true)
        {
            exp = exp.And(x => x.EmployeeId == queryDto.EmployeeId);
        }

        if (!string.IsNullOrEmpty(queryDto?.EmployeeName))
        {
            exp = exp.And(x => x.EmployeeName != null && x.EmployeeName.Contains(queryDto.EmployeeName));
        }

        if (queryDto?.SchemeMetricId.HasValue == true)
        {
            exp = exp.And(x => x.SchemeMetricId == queryDto.SchemeMetricId);
        }

        if (!string.IsNullOrEmpty(queryDto?.ObjectivePeriod))
        {
            exp = exp.And(x => x.ObjectivePeriod != null && x.ObjectivePeriod.Contains(queryDto.ObjectivePeriod));
        }

        if (!string.IsNullOrEmpty(queryDto?.ObjectiveDescription))
        {
            exp = exp.And(x => x.ObjectiveDescription != null && x.ObjectiveDescription.Contains(queryDto.ObjectiveDescription));
        }

        if (queryDto?.TargetValue.HasValue == true)
        {
            exp = exp.And(x => x.TargetValue == queryDto.TargetValue);
        }

        if (queryDto?.ActualValue.HasValue == true)
        {
            exp = exp.And(x => x.ActualValue == queryDto.ActualValue);
        }

        if (queryDto?.CompletionPercentage.HasValue == true)
        {
            exp = exp.And(x => x.CompletionPercentage == queryDto.CompletionPercentage);
        }

        if (queryDto?.ObjectiveWeight.HasValue == true)
        {
            exp = exp.And(x => x.ObjectiveWeight == queryDto.ObjectiveWeight);
        }

        if (!string.IsNullOrEmpty(queryDto?.AchievementNotes))
        {
            exp = exp.And(x => x.AchievementNotes != null && x.AchievementNotes.Contains(queryDto.AchievementNotes));
        }

        if (queryDto?.ObjectiveStatus.HasValue == true)
        {
            exp = exp.And(x => x.ObjectiveStatus == queryDto.ObjectiveStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.RelatedPlant))
        {
            exp = exp.And(x => x.RelatedPlant != null && x.RelatedPlant.Contains(queryDto.RelatedPlant));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.StartDateStart.HasValue == true)
        {
            exp = exp.And(x => x.StartDate >= queryDto.StartDateStart);
        }

        if (queryDto?.StartDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.StartDate <= queryDto.StartDateEnd);
        }

        if (queryDto?.DueDateStart.HasValue == true)
        {
            exp = exp.And(x => x.DueDate >= queryDto.DueDateStart);
        }

        if (queryDto?.DueDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.DueDate <= queryDto.DueDateEnd);
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
