// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.HumanResource.Performance
// 文件名称：TaktCycleScheduleService.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Cursor AI)
// 功能描述：绩效周期日程应用服务实现
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
/// 绩效周期日程应用服务
/// </summary>
public class TaktCycleScheduleService : TaktServiceBase, ITaktCycleScheduleService
{
    private readonly ITaktCompanyRepository<TaktCycleSchedule> _cycleScheduleRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="cycleScheduleRepository">绩效周期日程仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktCycleScheduleService(
        ITaktCompanyRepository<TaktCycleSchedule> cycleScheduleRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _cycleScheduleRepository = cycleScheduleRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取绩效周期日程列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktCycleScheduleDto>> GetCycleScheduleListAsync(TaktCycleScheduleQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _cycleScheduleRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktCycleScheduleDto>.Create(
            data.Adapt<List<TaktCycleScheduleDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取绩效周期日程
    /// </summary>
    /// <param name="id">绩效周期日程ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktCycleScheduleDto?> GetCycleScheduleByIdAsync(long id)
    {
        var entity = await _cycleScheduleRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktCycleScheduleDto>();
    }

    /// <summary>
    /// 获取绩效周期日程选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetCycleScheduleOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _cycleScheduleRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.CycleName,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.CycleName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建绩效周期日程
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCycleScheduleDto> CreateCycleScheduleAsync(TaktCycleScheduleCreateDto dto)
    {
        var entity = dto.Adapt<TaktCycleSchedule>();
        var isUnique_ix_cycle_schedule_code_unique = await _uniqueValidator.IsUniqueAsync(
            _cycleScheduleRepository,
            x => x.CycleCode == entity.CycleCode);
        if (!isUnique_ix_cycle_schedule_code_unique)
        {
            throw new TaktBusinessException("绩效周期日程的CycleCode已存在");
        }
        entity = await _cycleScheduleRepository.CreateAsync(entity);
        return await GetCycleScheduleByIdAsync(entity.Id) ?? entity.Adapt<TaktCycleScheduleDto>();
    }

    /// <summary>
    /// 更新绩效周期日程
    /// </summary>
    /// <param name="id">绩效周期日程ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCycleScheduleDto> UpdateCycleScheduleAsync(long id, TaktCycleScheduleUpdateDto dto)
    {
        var entity = await _cycleScheduleRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("绩效周期日程不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_cycle_schedule_code_unique = await _uniqueValidator.IsUniqueAsync(
            _cycleScheduleRepository,
            x => x.CycleCode == entity.CycleCode,
            id);
        if (!isUnique_ix_cycle_schedule_code_unique)
        {
            throw new TaktBusinessException("绩效周期日程的CycleCode已存在");
        }
        await _cycleScheduleRepository.UpdateAsync(entity);
        return await GetCycleScheduleByIdAsync(id) ?? throw new TaktBusinessException("绩效周期日程不存在");
    }

    /// <summary>
    /// 删除绩效周期日程
    /// </summary>
    /// <param name="id">绩效周期日程ID</param>
    /// <returns>任务</returns>
    public async Task DeleteCycleScheduleByIdAsync(long id)
    {
        var deleted = await _cycleScheduleRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("绩效周期日程不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除绩效周期日程
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteCycleScheduleBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteCycleScheduleByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新绩效周期日程状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCycleScheduleDto> UpdateCycleScheduleStatusAsync(TaktCycleScheduleStatusDto dto)
    {
        var entity = await _cycleScheduleRepository.GetByIdAsync(dto.CycleScheduleId);
        if (entity == null)
        {
            throw new TaktBusinessException("绩效周期日程不存在");
        }
        entity.CycleScheduleStatus = dto.CycleScheduleStatus;
        await _cycleScheduleRepository.UpdateAsync(entity);
        return await GetCycleScheduleByIdAsync(dto.CycleScheduleId) ?? throw new TaktBusinessException("绩效周期日程不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetCycleScheduleTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktCycleScheduleTemplateDto>(
            sheetName ?? "绩效周期日程导入模板",
            fileName ?? "绩效周期日程导入模板.xlsx");
    }

    /// <summary>
    /// 导入绩效周期日程
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportCycleScheduleAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktCycleScheduleImportDto>(fileStream, sheetName ?? "绩效周期日程导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktCycleSchedule>();
                var importKey = $"{entity.CycleCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（CycleCode）");
                }
                var isUnique_ix_cycle_schedule_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _cycleScheduleRepository,
                    x => x.CycleCode == entity.CycleCode);
                if (!isUnique_ix_cycle_schedule_code_unique)
                {
                    throw new TaktBusinessException("绩效周期日程的CycleCode已存在");
                }
                await _cycleScheduleRepository.CreateAsync(entity);
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
    /// 导出绩效周期日程
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportCycleScheduleAsync(TaktCycleScheduleQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktCycleScheduleQueryDto());
        var list = await _cycleScheduleRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktCycleScheduleExportDto>(),
                sheetName ?? "绩效周期日程数据",
                fileName ?? "绩效周期日程导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktCycleScheduleExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "绩效周期日程数据",
            fileName ?? "绩效周期日程导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建绩效周期日程查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktCycleSchedule, bool>> QueryExpression(TaktCycleScheduleQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktCycleSchedule>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.CycleCode != null && x.CycleCode.Contains(keywords))
                || (x.CycleName != null && x.CycleName.Contains(keywords))
                || (x.CycleType != null && x.CycleType.Contains(keywords))
                || SqlFunc.ToString(x.CycleYear).Contains(keywords)
                || SqlFunc.ToString(x.CycleSequence).Contains(keywords)
                || (x.ApplicableDepartment != null && x.ApplicableDepartment.Contains(keywords))
                || (x.Description != null && x.Description.Contains(keywords))
                || SqlFunc.ToString(x.CycleScheduleStatus).Contains(keywords)
                || (x.RelatedPlant != null && x.RelatedPlant.Contains(keywords))
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.StartDate).Contains(keywords)
                || SqlFunc.ToString(x.EndDate).Contains(keywords)
                || SqlFunc.ToString(x.GoalSettingDueDate).Contains(keywords)
                || SqlFunc.ToString(x.SelfEvaluationDueDate).Contains(keywords)
                || SqlFunc.ToString(x.SupervisorReviewDueDate).Contains(keywords)
                || SqlFunc.ToString(x.InterviewDueDate).Contains(keywords)
                || SqlFunc.ToString(x.ResultConfirmationDueDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.CycleCode))
        {
            exp = exp.And(x => x.CycleCode != null && x.CycleCode.Contains(queryDto.CycleCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.CycleName))
        {
            exp = exp.And(x => x.CycleName != null && x.CycleName.Contains(queryDto.CycleName));
        }

        if (!string.IsNullOrEmpty(queryDto?.CycleType))
        {
            exp = exp.And(x => x.CycleType != null && x.CycleType.Contains(queryDto.CycleType));
        }

        if (queryDto?.CycleYear.HasValue == true)
        {
            exp = exp.And(x => x.CycleYear == queryDto.CycleYear);
        }

        if (queryDto?.CycleSequence.HasValue == true)
        {
            exp = exp.And(x => x.CycleSequence == queryDto.CycleSequence);
        }

        if (!string.IsNullOrEmpty(queryDto?.ApplicableDepartment))
        {
            exp = exp.And(x => x.ApplicableDepartment != null && x.ApplicableDepartment.Contains(queryDto.ApplicableDepartment));
        }

        if (!string.IsNullOrEmpty(queryDto?.Description))
        {
            exp = exp.And(x => x.Description != null && x.Description.Contains(queryDto.Description));
        }

        if (queryDto?.CycleScheduleStatus.HasValue == true)
        {
            exp = exp.And(x => x.CycleScheduleStatus == queryDto.CycleScheduleStatus);
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

        if (queryDto?.EndDateStart.HasValue == true)
        {
            exp = exp.And(x => x.EndDate >= queryDto.EndDateStart);
        }

        if (queryDto?.EndDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.EndDate <= queryDto.EndDateEnd);
        }

        if (queryDto?.GoalSettingDueDateStart.HasValue == true)
        {
            exp = exp.And(x => x.GoalSettingDueDate >= queryDto.GoalSettingDueDateStart);
        }

        if (queryDto?.GoalSettingDueDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.GoalSettingDueDate <= queryDto.GoalSettingDueDateEnd);
        }

        if (queryDto?.SelfEvaluationDueDateStart.HasValue == true)
        {
            exp = exp.And(x => x.SelfEvaluationDueDate >= queryDto.SelfEvaluationDueDateStart);
        }

        if (queryDto?.SelfEvaluationDueDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.SelfEvaluationDueDate <= queryDto.SelfEvaluationDueDateEnd);
        }

        if (queryDto?.SupervisorReviewDueDateStart.HasValue == true)
        {
            exp = exp.And(x => x.SupervisorReviewDueDate >= queryDto.SupervisorReviewDueDateStart);
        }

        if (queryDto?.SupervisorReviewDueDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.SupervisorReviewDueDate <= queryDto.SupervisorReviewDueDateEnd);
        }

        if (queryDto?.InterviewDueDateStart.HasValue == true)
        {
            exp = exp.And(x => x.InterviewDueDate >= queryDto.InterviewDueDateStart);
        }

        if (queryDto?.InterviewDueDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.InterviewDueDate <= queryDto.InterviewDueDateEnd);
        }

        if (queryDto?.ResultConfirmationDueDateStart.HasValue == true)
        {
            exp = exp.And(x => x.ResultConfirmationDueDate >= queryDto.ResultConfirmationDueDateStart);
        }

        if (queryDto?.ResultConfirmationDueDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.ResultConfirmationDueDate <= queryDto.ResultConfirmationDueDateEnd);
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
