// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Routine.HelpDesk
// 文件名称：TaktTicketChangeLogService.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：工单变更日志应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Routine.HelpDesk;
using Takt.Domain.Entities.Routine.HelpDesk;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;
using Takt.Domain.Entities.Routine.HelpDesk;

namespace Takt.Application.Services.Routine.HelpDesk;

/// <summary>
/// 工单变更日志应用服务
/// </summary>
public class TaktTicketChangeLogService : TaktServiceBase, ITaktTicketChangeLogService
{
    private readonly ITaktCompanyRepository<TaktTicketChangeLog> _ticketChangeLogRepository;
    private readonly ITaktCompanyRepository<TaktTicket> _ticketRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ticketChangeLogRepository">工单变更日志仓储</param>
    /// <param name="ticketRepository">工单仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktTicketChangeLogService(
        ITaktCompanyRepository<TaktTicketChangeLog> ticketChangeLogRepository,
        ITaktCompanyRepository<TaktTicket> ticketRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _ticketChangeLogRepository = ticketChangeLogRepository;
        _ticketRepository = ticketRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取工单变更日志列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktTicketChangeLogDto>> GetTicketChangeLogListAsync(TaktTicketChangeLogQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _ticketChangeLogRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktTicketChangeLogDto>.Create(
            data.Adapt<List<TaktTicketChangeLogDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取工单变更日志
    /// </summary>
    /// <param name="id">工单变更日志ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktTicketChangeLogDto?> GetTicketChangeLogByIdAsync(long id)
    {
        var entity = await _ticketChangeLogRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktTicketChangeLogDto>();
    }

    /// <summary>
    /// 获取工单变更日志选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetTicketChangeLogOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _ticketChangeLogRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.TicketNo,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.TicketNo ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建工单变更日志
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTicketChangeLogDto> CreateTicketChangeLogAsync(TaktTicketChangeLogCreateDto dto)
    {
        var entity = dto.Adapt<TaktTicketChangeLog>();
                await StampTicketChangeLogTicketAsync(entity, dto);
        entity = await _ticketChangeLogRepository.CreateAsync(entity);
        return await GetTicketChangeLogByIdAsync(entity.Id) ?? entity.Adapt<TaktTicketChangeLogDto>();
    }

    /// <summary>
    /// 更新工单变更日志
    /// </summary>
    /// <param name="id">工单变更日志ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTicketChangeLogDto> UpdateTicketChangeLogAsync(long id, TaktTicketChangeLogUpdateDto dto)
    {
        var entity = await _ticketChangeLogRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("工单变更日志不存在");
        }
        dto.Adapt(entity);
                await StampTicketChangeLogTicketAsync(entity, dto);
        await _ticketChangeLogRepository.UpdateAsync(entity);
        return await GetTicketChangeLogByIdAsync(id) ?? throw new TaktBusinessException("工单变更日志不存在");
    }

    /// <summary>
    /// 删除工单变更日志
    /// </summary>
    /// <param name="id">工单变更日志ID</param>
    /// <returns>任务</returns>
    public async Task DeleteTicketChangeLogByIdAsync(long id)
    {
        var deleted = await _ticketChangeLogRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("工单变更日志不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除工单变更日志
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteTicketChangeLogBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteTicketChangeLogByIdAsync(id);
        }
    }

    /// <summary>
    /// 导出工单变更日志
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportTicketChangeLogAsync(TaktTicketChangeLogQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktTicketChangeLogQueryDto());
        var list = await _ticketChangeLogRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktTicketChangeLogExportDto>(),
                sheetName ?? "工单变更日志数据",
                fileName ?? "工单变更日志导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktTicketChangeLogExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "工单变更日志数据",
            fileName ?? "工单变更日志导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步工单变更日志主表外键（ManyToOne → 工单）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampTicketChangeLogTicketAsync(TaktTicketChangeLog entity, TaktTicketChangeLogCreateDto dto)
    {
        if (dto.TicketId <= 0)
        {
            return;
        }
        var master = await _ticketRepository.GetByIdAsync(dto.TicketId);
        if (master == null)
        {
            throw new TaktBusinessException("工单不存在");
        }
        entity.TicketId = master.Id;
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建工单变更日志查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktTicketChangeLog, bool>> QueryExpression(TaktTicketChangeLogQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktTicketChangeLog>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.TicketId).Contains(keywords)
                || (x.TicketNo != null && x.TicketNo.Contains(keywords))
                || SqlFunc.ToString(x.ChangeType).Contains(keywords)
                || (x.ChangeSummary != null && x.ChangeSummary.Contains(keywords))
                || (x.ChangeFields != null && x.ChangeFields.Contains(keywords))
                || (x.ChangeReason != null && x.ChangeReason.Contains(keywords))
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.TicketId.HasValue == true)
        {
            exp = exp.And(x => x.TicketId == queryDto.TicketId);
        }

        if (!string.IsNullOrEmpty(queryDto?.TicketNo))
        {
            exp = exp.And(x => x.TicketNo != null && x.TicketNo.Contains(queryDto.TicketNo));
        }

        if (queryDto?.ChangeType.HasValue == true)
        {
            exp = exp.And(x => x.ChangeType == queryDto.ChangeType);
        }

        if (!string.IsNullOrEmpty(queryDto?.ChangeSummary))
        {
            exp = exp.And(x => x.ChangeSummary != null && x.ChangeSummary.Contains(queryDto.ChangeSummary));
        }

        if (!string.IsNullOrEmpty(queryDto?.ChangeFields))
        {
            exp = exp.And(x => x.ChangeFields != null && x.ChangeFields.Contains(queryDto.ChangeFields));
        }

        if (!string.IsNullOrEmpty(queryDto?.ChangeReason))
        {
            exp = exp.And(x => x.ChangeReason != null && x.ChangeReason.Contains(queryDto.ChangeReason));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
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
