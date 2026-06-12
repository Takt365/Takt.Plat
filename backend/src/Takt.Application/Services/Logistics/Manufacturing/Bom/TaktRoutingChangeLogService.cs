// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：TaktRoutingChangeLogService.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：工艺路线变更日志应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Bom;
using Takt.Domain.Entities.Logistics.Manufacturing.Bom;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Bom;

/// <summary>
/// 工艺路线变更日志应用服务
/// </summary>
public class TaktRoutingChangeLogService : TaktServiceBase, ITaktRoutingChangeLogService
{
    private readonly ITaktCompanyRepository<TaktRoutingChangeLog> _routingChangeLogRepository;
    private readonly ITaktApprovalRepository<TaktRouting> _routingRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="routingChangeLogRepository">工艺路线变更日志仓储</param>
    /// <param name="routingRepository">工艺路线主仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktRoutingChangeLogService(
        ITaktCompanyRepository<TaktRoutingChangeLog> routingChangeLogRepository,
        ITaktApprovalRepository<TaktRouting> routingRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _routingChangeLogRepository = routingChangeLogRepository;
        _routingRepository = routingRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取工艺路线变更日志列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktRoutingChangeLogDto>> GetRoutingChangeLogListAsync(TaktRoutingChangeLogQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _routingChangeLogRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktRoutingChangeLogDto>.Create(
            data.Adapt<List<TaktRoutingChangeLogDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取工艺路线变更日志
    /// </summary>
    /// <param name="id">工艺路线变更日志ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktRoutingChangeLogDto?> GetRoutingChangeLogByIdAsync(long id)
    {
        var entity = await _routingChangeLogRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktRoutingChangeLogDto>();
    }

    /// <summary>
    /// 获取工艺路线变更日志选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetRoutingChangeLogOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _routingChangeLogRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.ChangeFields,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.ChangeFields ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建工艺路线变更日志
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktRoutingChangeLogDto> CreateRoutingChangeLogAsync(TaktRoutingChangeLogCreateDto dto)
    {
        var entity = dto.Adapt<TaktRoutingChangeLog>();
        await StampRoutingChangeLogRoutingAsync(entity, dto);
        entity = await _routingChangeLogRepository.CreateAsync(entity);
        return await GetRoutingChangeLogByIdAsync(entity.Id) ?? entity.Adapt<TaktRoutingChangeLogDto>();
    }

    /// <summary>
    /// 更新工艺路线变更日志
    /// </summary>
    /// <param name="id">工艺路线变更日志ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktRoutingChangeLogDto> UpdateRoutingChangeLogAsync(long id, TaktRoutingChangeLogUpdateDto dto)
    {
        var entity = await _routingChangeLogRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("工艺路线变更日志不存在");
        }
        dto.Adapt(entity);
        await StampRoutingChangeLogRoutingAsync(entity, dto);
        await _routingChangeLogRepository.UpdateAsync(entity);
        return await GetRoutingChangeLogByIdAsync(id) ?? throw new TaktBusinessException("工艺路线变更日志不存在");
    }

    /// <summary>
    /// 删除工艺路线变更日志
    /// </summary>
    /// <param name="id">工艺路线变更日志ID</param>
    /// <returns>任务</returns>
    public async Task DeleteRoutingChangeLogByIdAsync(long id)
    {
        var deleted = await _routingChangeLogRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("工艺路线变更日志不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除工艺路线变更日志
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteRoutingChangeLogBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteRoutingChangeLogByIdAsync(id);
        }
    }

    /// <summary>
    /// 导出工艺路线变更日志
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportRoutingChangeLogAsync(TaktRoutingChangeLogQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktRoutingChangeLogQueryDto());
        var list = await _routingChangeLogRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktRoutingChangeLogExportDto>(),
                sheetName ?? "工艺路线变更日志数据",
                fileName ?? "工艺路线变更日志导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktRoutingChangeLogExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "工艺路线变更日志数据",
            fileName ?? "工艺路线变更日志导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步工艺路线变更日志主表外键（ManyToOne → 工艺路线主）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampRoutingChangeLogRoutingAsync(TaktRoutingChangeLog entity, TaktRoutingChangeLogCreateDto dto)
    {
        if (dto.RoutingId <= 0)
        {
            return;
        }
        var master = await _routingRepository.GetByIdAsync(dto.RoutingId);
        if (master == null)
        {
            throw new TaktBusinessException("工艺路线主不存在");
        }
        entity.RoutingId = master.Id;
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建工艺路线变更日志查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktRoutingChangeLog, bool>> QueryExpression(TaktRoutingChangeLogQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktRoutingChangeLog>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.RoutingId).Contains(keywords)
                || (x.ChangeFields != null && x.ChangeFields.Contains(keywords))
                || SqlFunc.ToString(x.ChangeType).Contains(keywords)
                || (x.ChangeReason != null && x.ChangeReason.Contains(keywords))
                || (x.ChangeBy != null && x.ChangeBy.Contains(keywords))
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.ChangeTime).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.RoutingId.HasValue == true)
        {
            exp = exp.And(x => x.RoutingId == queryDto.RoutingId);
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

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
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
