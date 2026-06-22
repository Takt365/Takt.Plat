// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Statistics.Logging
// 文件名称：TaktQuartzLogService.cs
// 创建时间：2026-06-12
// 创建人：Takt365(Cursor AI)
// 功能描述：任务执行日志应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Statistics.Logging;
using Takt.Domain.Entities.Statistics.Logging;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;
using Takt.Shared.Enums;

namespace Takt.Application.Services.Statistics.Logging;

/// <summary>
/// 任务执行日志应用服务
/// </summary>
public class TaktQuartzLogService : TaktServiceBase, ITaktQuartzLogService
{
    private readonly ITaktCompanyRepository<TaktQuartzLog> _quartzLogRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="quartzLogRepository">任务执行日志仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktQuartzLogService(
        ITaktCompanyRepository<TaktQuartzLog> quartzLogRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _quartzLogRepository = quartzLogRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取任务执行日志列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktQuartzLogDto>> GetQuartzLogListAsync(TaktQuartzLogQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _quartzLogRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktQuartzLogDto>.Create(
            data.Adapt<List<TaktQuartzLogDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取任务执行日志
    /// </summary>
    /// <param name="id">任务执行日志ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktQuartzLogDto?> GetQuartzLogByIdAsync(long id)
    {
        var entity = await _quartzLogRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktQuartzLogDto>();
    }

    /// <summary>
    /// 获取任务执行日志选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetQuartzLogOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _quartzLogRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.TaskName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.TaskName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建任务执行日志
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktQuartzLogDto> CreateQuartzLogAsync(TaktQuartzLogCreateDto dto)
    {
        var entity = dto.Adapt<TaktQuartzLog>();
        entity = await _quartzLogRepository.CreateAsync(entity);
        return await GetQuartzLogByIdAsync(entity.Id) ?? entity.Adapt<TaktQuartzLogDto>();
    }

    /// <summary>
    /// 更新任务执行日志
    /// </summary>
    /// <param name="id">任务执行日志ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktQuartzLogDto> UpdateQuartzLogAsync(long id, TaktQuartzLogUpdateDto dto)
    {
        var entity = await _quartzLogRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("任务执行日志不存在");
        }
        dto.Adapt(entity);
        await _quartzLogRepository.UpdateAsync(entity);
        return await GetQuartzLogByIdAsync(id) ?? throw new TaktBusinessException("任务执行日志不存在");
    }

    /// <summary>
    /// 删除任务执行日志
    /// </summary>
    /// <param name="id">任务执行日志ID</param>
    /// <returns>任务</returns>
    public async Task DeleteQuartzLogByIdAsync(long id)
    {
        var deleted = await _quartzLogRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("任务执行日志不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除任务执行日志
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteQuartzLogBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteQuartzLogByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新任务执行日志状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktQuartzLogDto> UpdateQuartzLogStatusAsync(TaktQuartzLogStatusDto dto)
    {
        var entity = await _quartzLogRepository.GetByIdAsync(dto.QuartzLogId);
        if (entity == null)
        {
            throw new TaktBusinessException("任务执行日志不存在");
        }
        entity.ExecuteStatus = dto.ExecuteStatus;
        await _quartzLogRepository.UpdateAsync(entity);
        return await GetQuartzLogByIdAsync(dto.QuartzLogId) ?? throw new TaktBusinessException("任务执行日志不存在");
    }

    /// <summary>
    /// 导出任务执行日志
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportQuartzLogAsync(TaktQuartzLogQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktQuartzLogQueryDto());
        var list = await _quartzLogRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktQuartzLogExportDto>(),
                sheetName ?? "任务执行日志数据",
                fileName ?? "任务执行日志导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktQuartzLogExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "任务执行日志数据",
            fileName ?? "任务执行日志导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建任务执行日志查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktQuartzLog, bool>> QueryExpression(TaktQuartzLogQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktQuartzLog>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.QuartzTaskId).Contains(keywords)
                || (x.TaskName != null && x.TaskName.Contains(keywords))
                || SqlFunc.ToString(x.JobGroup).Contains(keywords)
                || SqlFunc.ToString(x.TaskType).Contains(keywords)
                || SqlFunc.ToString(x.ExecuteDuration).Contains(keywords)
                || (x.ExecuteParams != null && x.ExecuteParams.Contains(keywords))
                || (x.ExecuteMessage != null && x.ExecuteMessage.Contains(keywords))
                || (x.ErrorInfo != null && x.ErrorInfo.Contains(keywords))
                || (x.ExecuteIp != null && x.ExecuteIp.Contains(keywords))
                || (x.ExecuteHost != null && x.ExecuteHost.Contains(keywords))
                || SqlFunc.ToString(x.ExecuteStatus).Contains(keywords)
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.ExecuteTime).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.QuartzTaskId.HasValue == true)
        {
            exp = exp.And(x => x.QuartzTaskId == queryDto.QuartzTaskId);
        }

        if (!string.IsNullOrEmpty(queryDto?.TaskName))
        {
            exp = exp.And(x => x.TaskName != null && x.TaskName.Contains(queryDto.TaskName));
        }

        if (queryDto?.JobGroup.HasValue == true)
        {
            exp = exp.And(x => x.JobGroup == queryDto.JobGroup);
        }

        if (queryDto?.TaskType.HasValue == true)
        {
            exp = exp.And(x => x.TaskType == queryDto.TaskType);
        }

        if (queryDto?.ExecuteDuration.HasValue == true)
        {
            exp = exp.And(x => x.ExecuteDuration == queryDto.ExecuteDuration);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExecuteParams))
        {
            exp = exp.And(x => x.ExecuteParams != null && x.ExecuteParams.Contains(queryDto.ExecuteParams));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExecuteMessage))
        {
            exp = exp.And(x => x.ExecuteMessage != null && x.ExecuteMessage.Contains(queryDto.ExecuteMessage));
        }

        if (!string.IsNullOrEmpty(queryDto?.ErrorInfo))
        {
            exp = exp.And(x => x.ErrorInfo != null && x.ErrorInfo.Contains(queryDto.ErrorInfo));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExecuteIp))
        {
            exp = exp.And(x => x.ExecuteIp != null && x.ExecuteIp.Contains(queryDto.ExecuteIp));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExecuteHost))
        {
            exp = exp.And(x => x.ExecuteHost != null && x.ExecuteHost.Contains(queryDto.ExecuteHost));
        }

        if (queryDto?.ExecuteStatus.HasValue == true)
        {
            exp = exp.And(x => x.ExecuteStatus == queryDto.ExecuteStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.ExecuteTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.ExecuteTime >= queryDto.ExecuteTimeStart);
        }

        if (queryDto?.ExecuteTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.ExecuteTime <= queryDto.ExecuteTimeEnd);
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
