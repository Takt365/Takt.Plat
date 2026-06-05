// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Foundation
// 文件名称：TaktQuartzTaskService.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Cursor AI)
// 功能描述：定时任务应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Foundation;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Foundation;

/// <summary>
/// 定时任务应用服务
/// </summary>
public class TaktQuartzTaskService : TaktServiceBase, ITaktQuartzTaskService
{
    private readonly ITaktCompanyRepository<TaktQuartzTask> _quartzTaskRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="quartzTaskRepository">定时任务仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktQuartzTaskService(
        ITaktCompanyRepository<TaktQuartzTask> quartzTaskRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _quartzTaskRepository = quartzTaskRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取定时任务列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktQuartzTaskDto>> GetQuartzTaskListAsync(TaktQuartzTaskQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _quartzTaskRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktQuartzTaskDto>.Create(
            data.Adapt<List<TaktQuartzTaskDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取定时任务
    /// </summary>
    /// <param name="id">定时任务ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktQuartzTaskDto?> GetQuartzTaskByIdAsync(long id)
    {
        var entity = await _quartzTaskRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktQuartzTaskDto>();
    }

    /// <summary>
    /// 获取定时任务选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetQuartzTaskOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _quartzTaskRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.TaskName,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.TaskName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建定时任务
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktQuartzTaskDto> CreateQuartzTaskAsync(TaktQuartzTaskCreateDto dto)
    {
        var entity = dto.Adapt<TaktQuartzTask>();
        var isUnique_ix_quartz_task_code_unique = await _uniqueValidator.IsUniqueAsync(
            _quartzTaskRepository,
            x => x.TaskCode == entity.TaskCode);
        if (!isUnique_ix_quartz_task_code_unique)
        {
            throw new TaktBusinessException("定时任务的TaskCode已存在");
        }
        entity = await _quartzTaskRepository.CreateAsync(entity);
        return await GetQuartzTaskByIdAsync(entity.Id) ?? entity.Adapt<TaktQuartzTaskDto>();
    }

    /// <summary>
    /// 更新定时任务
    /// </summary>
    /// <param name="id">定时任务ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktQuartzTaskDto> UpdateQuartzTaskAsync(long id, TaktQuartzTaskUpdateDto dto)
    {
        var entity = await _quartzTaskRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("定时任务不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_quartz_task_code_unique = await _uniqueValidator.IsUniqueAsync(
            _quartzTaskRepository,
            x => x.TaskCode == entity.TaskCode,
            id);
        if (!isUnique_ix_quartz_task_code_unique)
        {
            throw new TaktBusinessException("定时任务的TaskCode已存在");
        }
        await _quartzTaskRepository.UpdateAsync(entity);
        return await GetQuartzTaskByIdAsync(id) ?? throw new TaktBusinessException("定时任务不存在");
    }

    /// <summary>
    /// 删除定时任务
    /// </summary>
    /// <param name="id">定时任务ID</param>
    /// <returns>任务</returns>
    public async Task DeleteQuartzTaskByIdAsync(long id)
    {
        var deleted = await _quartzTaskRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("定时任务不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除定时任务
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteQuartzTaskBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteQuartzTaskByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新定时任务状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktQuartzTaskDto> UpdateQuartzTaskStatusAsync(TaktQuartzTaskStatusDto dto)
    {
        var entity = await _quartzTaskRepository.GetByIdAsync(dto.QuartzTaskId);
        if (entity == null)
        {
            throw new TaktBusinessException("定时任务不存在");
        }
        entity.TaskStatus = dto.TaskStatus;
        await _quartzTaskRepository.UpdateAsync(entity);
        return await GetQuartzTaskByIdAsync(dto.QuartzTaskId) ?? throw new TaktBusinessException("定时任务不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetQuartzTaskTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktQuartzTaskTemplateDto>(
            sheetName ?? "定时任务导入模板",
            fileName ?? "定时任务导入模板.xlsx");
    }

    /// <summary>
    /// 导入定时任务
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportQuartzTaskAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktQuartzTaskImportDto>(fileStream, sheetName ?? "定时任务导入模板");
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
                var entity = rows[i].Adapt<TaktQuartzTask>();
                var importKey = $"{entity.TaskCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（TaskCode）");
                }
                var isUnique_ix_quartz_task_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _quartzTaskRepository,
                    x => x.TaskCode == entity.TaskCode);
                if (!isUnique_ix_quartz_task_code_unique)
                {
                    throw new TaktBusinessException("定时任务的TaskCode已存在");
                }
                await _quartzTaskRepository.CreateAsync(entity);
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
    /// 导出定时任务
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportQuartzTaskAsync(TaktQuartzTaskQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktQuartzTaskQueryDto());
        var list = await _quartzTaskRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktQuartzTaskExportDto>(),
                sheetName ?? "定时任务数据",
                fileName ?? "定时任务导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktQuartzTaskExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "定时任务数据",
            fileName ?? "定时任务导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建定时任务查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktQuartzTask, bool>> QueryExpression(TaktQuartzTaskQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktQuartzTask>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.TaskCode != null && x.TaskCode.Contains(keywords))
                || (x.TaskName != null && x.TaskName.Contains(keywords))
                || (x.JobName != null && x.JobName.Contains(keywords))
                || (x.JobGroup != null && x.JobGroup.Contains(keywords))
                || (x.CronExpression != null && x.CronExpression.Contains(keywords))
                || (x.JobType != null && x.JobType.Contains(keywords))
                || (x.JobParams != null && x.JobParams.Contains(keywords))
                || SqlFunc.ToString(x.TaskStatus).Contains(keywords)
                || SqlFunc.ToString(x.Concurrent).Contains(keywords)
                || SqlFunc.ToString(x.MisfirePolicy).Contains(keywords)
                || (x.Description != null && x.Description.Contains(keywords))
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.LastRunAt).Contains(keywords)
                || SqlFunc.ToString(x.NextRunAt).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.TaskCode))
        {
            exp = exp.And(x => x.TaskCode != null && x.TaskCode.Contains(queryDto.TaskCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.TaskName))
        {
            exp = exp.And(x => x.TaskName != null && x.TaskName.Contains(queryDto.TaskName));
        }

        if (!string.IsNullOrEmpty(queryDto?.JobName))
        {
            exp = exp.And(x => x.JobName != null && x.JobName.Contains(queryDto.JobName));
        }

        if (!string.IsNullOrEmpty(queryDto?.JobGroup))
        {
            exp = exp.And(x => x.JobGroup != null && x.JobGroup.Contains(queryDto.JobGroup));
        }

        if (!string.IsNullOrEmpty(queryDto?.CronExpression))
        {
            exp = exp.And(x => x.CronExpression != null && x.CronExpression.Contains(queryDto.CronExpression));
        }

        if (!string.IsNullOrEmpty(queryDto?.JobType))
        {
            exp = exp.And(x => x.JobType != null && x.JobType.Contains(queryDto.JobType));
        }

        if (!string.IsNullOrEmpty(queryDto?.JobParams))
        {
            exp = exp.And(x => x.JobParams != null && x.JobParams.Contains(queryDto.JobParams));
        }

        if (queryDto?.TaskStatus.HasValue == true)
        {
            exp = exp.And(x => x.TaskStatus == queryDto.TaskStatus);
        }

        if (queryDto?.Concurrent.HasValue == true)
        {
            exp = exp.And(x => x.Concurrent == queryDto.Concurrent);
        }

        if (queryDto?.MisfirePolicy.HasValue == true)
        {
            exp = exp.And(x => x.MisfirePolicy == queryDto.MisfirePolicy);
        }

        if (!string.IsNullOrEmpty(queryDto?.Description))
        {
            exp = exp.And(x => x.Description != null && x.Description.Contains(queryDto.Description));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.LastRunAtStart.HasValue == true)
        {
            exp = exp.And(x => x.LastRunAt >= queryDto.LastRunAtStart);
        }

        if (queryDto?.LastRunAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.LastRunAt <= queryDto.LastRunAtEnd);
        }

        if (queryDto?.NextRunAtStart.HasValue == true)
        {
            exp = exp.And(x => x.NextRunAt >= queryDto.NextRunAtStart);
        }

        if (queryDto?.NextRunAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.NextRunAt <= queryDto.NextRunAtEnd);
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
