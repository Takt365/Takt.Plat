// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Scheduling
// 文件名称：TaktApsScheduleChangeLogService.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：APS排程变更日志应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Scheduling;
using Takt.Domain.Entities.Logistics.Manufacturing.Scheduling;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Scheduling;

/// <summary>
/// APS排程变更日志应用服务
/// </summary>
public class TaktApsScheduleChangeLogService : TaktServiceBase, ITaktApsScheduleChangeLogService
{
    private readonly ITaktCompanyRepository<TaktApsScheduleChangeLog> _apsScheduleChangeLogRepository;
    private readonly ITaktCompanyRepository<TaktApsSchedule> _apsScheduleRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="apsScheduleChangeLogRepository">APS排程变更日志仓储</param>
    /// <param name="apsScheduleRepository">APS排程主仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktApsScheduleChangeLogService(
        ITaktCompanyRepository<TaktApsScheduleChangeLog> apsScheduleChangeLogRepository,
        ITaktCompanyRepository<TaktApsSchedule> apsScheduleRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _apsScheduleChangeLogRepository = apsScheduleChangeLogRepository;
        _apsScheduleRepository = apsScheduleRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取APS排程变更日志列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktApsScheduleChangeLogDto>> GetApsScheduleChangeLogListAsync(TaktApsScheduleChangeLogQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _apsScheduleChangeLogRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktApsScheduleChangeLogDto>.Create(
            data.Adapt<List<TaktApsScheduleChangeLogDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取APS排程变更日志
    /// </summary>
    /// <param name="id">APS排程变更日志ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktApsScheduleChangeLogDto?> GetApsScheduleChangeLogByIdAsync(long id)
    {
        var entity = await _apsScheduleChangeLogRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktApsScheduleChangeLogDto>();
    }

    /// <summary>
    /// 获取APS排程变更日志选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetApsScheduleChangeLogOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _apsScheduleChangeLogRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.ChangeFields ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.ChangeFields ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建APS排程变更日志
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktApsScheduleChangeLogDto> CreateApsScheduleChangeLogAsync(TaktApsScheduleChangeLogCreateDto dto)
    {
        var entity = dto.Adapt<TaktApsScheduleChangeLog>();
        await StampApsScheduleChangeLogApsScheduleAsync(entity, dto);
        entity = await _apsScheduleChangeLogRepository.CreateAsync(entity);
        return await GetApsScheduleChangeLogByIdAsync(entity.Id) ?? entity.Adapt<TaktApsScheduleChangeLogDto>();
    }

    /// <summary>
    /// 更新APS排程变更日志
    /// </summary>
    /// <param name="id">APS排程变更日志ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktApsScheduleChangeLogDto> UpdateApsScheduleChangeLogAsync(long id, TaktApsScheduleChangeLogUpdateDto dto)
    {
        var entity = await _apsScheduleChangeLogRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("APS排程变更日志不存在");
        }
        dto.Adapt(entity);
        await StampApsScheduleChangeLogApsScheduleAsync(entity, dto);
        await _apsScheduleChangeLogRepository.UpdateAsync(entity);
        return await GetApsScheduleChangeLogByIdAsync(id) ?? throw new TaktBusinessException("APS排程变更日志不存在");
    }

    /// <summary>
    /// 删除APS排程变更日志
    /// </summary>
    /// <param name="id">APS排程变更日志ID</param>
    /// <returns>任务</returns>
    public async Task DeleteApsScheduleChangeLogByIdAsync(long id)
    {
        var deleted = await _apsScheduleChangeLogRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("APS排程变更日志不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除APS排程变更日志
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteApsScheduleChangeLogBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteApsScheduleChangeLogByIdAsync(id);
        }
    }

    /// <summary>
    /// 导出APS排程变更日志
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportApsScheduleChangeLogAsync(TaktApsScheduleChangeLogQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktApsScheduleChangeLogQueryDto());
        var list = await _apsScheduleChangeLogRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktApsScheduleChangeLogExportDto>(),
                sheetName ?? "APS排程变更日志数据",
                fileName ?? "APS排程变更日志导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktApsScheduleChangeLogExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "APS排程变更日志数据",
            fileName ?? "APS排程变更日志导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步APS排程变更日志主表外键（ManyToOne → APS排程主）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampApsScheduleChangeLogApsScheduleAsync(TaktApsScheduleChangeLog entity, TaktApsScheduleChangeLogCreateDto dto)
    {
        if (dto.ApsScheduleId <= 0)
        {
            return;
        }
        var master = await _apsScheduleRepository.GetByIdAsync(dto.ApsScheduleId);
        if (master == null)
        {
            throw new TaktBusinessException("APS排程主不存在");
        }
        entity.ApsScheduleId = master.Id;
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建APS排程变更日志查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktApsScheduleChangeLog, bool>> QueryExpression(TaktApsScheduleChangeLogQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktApsScheduleChangeLog>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.ApsScheduleId).Contains(keywords)
                || (x.ChangeFields != null && x.ChangeFields.Contains(keywords))
                || SqlFunc.ToString(x.ChangeType).Contains(keywords)
                || (x.ChangeReason != null && x.ChangeReason.Contains(keywords))
                || (x.ChangeBy != null && x.ChangeBy.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.ChangeTime).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.ApsScheduleId.HasValue == true)
        {
            exp = exp.And(x => x.ApsScheduleId == queryDto.ApsScheduleId);
        }

        if (!string.IsNullOrEmpty(queryDto?.ChangeFields))
        {
            exp = exp.And(x => x.ChangeFields != null && x.ChangeFields.Contains(queryDto.ChangeFields));
        }

        if (queryDto?.ChangeType.HasValue == true)
        {
            exp = exp.And(x => x.ChangeType == queryDto.ChangeType);
        }

        if (!string.IsNullOrEmpty(queryDto?.ChangeReason))
        {
            exp = exp.And(x => x.ChangeReason != null && x.ChangeReason.Contains(queryDto.ChangeReason));
        }

        if (!string.IsNullOrEmpty(queryDto?.ChangeBy))
        {
            exp = exp.And(x => x.ChangeBy != null && x.ChangeBy.Contains(queryDto.ChangeBy));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.ChangeTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.ChangeTime >= queryDto.ChangeTimeStart);
        }

        if (queryDto?.ChangeTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.ChangeTime <= queryDto.ChangeTimeEnd);
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
