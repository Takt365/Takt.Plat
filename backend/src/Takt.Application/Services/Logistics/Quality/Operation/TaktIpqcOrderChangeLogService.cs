// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Quality.Operation
// 文件名称：TaktIpqcOrderChangeLogService.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Cursor AI)
// 功能描述：制程检验单变更日志应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Quality.Operation;
using Takt.Domain.Entities.Logistics.Quality.Operation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;
using Takt.Domain.Entities.Logistics.Quality.Operation;

namespace Takt.Application.Services.Logistics.Quality.Operation;

/// <summary>
/// 制程检验单变更日志应用服务
/// </summary>
public class TaktIpqcOrderChangeLogService : TaktServiceBase, ITaktIpqcOrderChangeLogService
{
    private readonly ITaktCompanyRepository<TaktIpqcOrderChangeLog> _ipqcOrderChangeLogRepository;
    private readonly ITaktCompanyRepository<TaktIpqcOrder> _ipqcOrderRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ipqcOrderChangeLogRepository">制程检验单变更日志仓储</param>
    /// <param name="ipqcOrderRepository">制程检验单仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktIpqcOrderChangeLogService(
        ITaktCompanyRepository<TaktIpqcOrderChangeLog> ipqcOrderChangeLogRepository,
        ITaktCompanyRepository<TaktIpqcOrder> ipqcOrderRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _ipqcOrderChangeLogRepository = ipqcOrderChangeLogRepository;
        _ipqcOrderRepository = ipqcOrderRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取制程检验单变更日志列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktIpqcOrderChangeLogDto>> GetIpqcOrderChangeLogListAsync(TaktIpqcOrderChangeLogQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _ipqcOrderChangeLogRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktIpqcOrderChangeLogDto>.Create(
            data.Adapt<List<TaktIpqcOrderChangeLogDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取制程检验单变更日志
    /// </summary>
    /// <param name="id">制程检验单变更日志ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktIpqcOrderChangeLogDto?> GetIpqcOrderChangeLogByIdAsync(long id)
    {
        var entity = await _ipqcOrderChangeLogRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktIpqcOrderChangeLogDto>();
    }

    /// <summary>
    /// 获取制程检验单变更日志选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetIpqcOrderChangeLogOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _ipqcOrderChangeLogRepository.GetListAsync(
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
    /// 创建制程检验单变更日志
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktIpqcOrderChangeLogDto> CreateIpqcOrderChangeLogAsync(TaktIpqcOrderChangeLogCreateDto dto)
    {
        var entity = dto.Adapt<TaktIpqcOrderChangeLog>();
                await StampIpqcOrderChangeLogIpqcOrderAsync(entity, dto);
        entity = await _ipqcOrderChangeLogRepository.CreateAsync(entity);
        return await GetIpqcOrderChangeLogByIdAsync(entity.Id) ?? entity.Adapt<TaktIpqcOrderChangeLogDto>();
    }

    /// <summary>
    /// 更新制程检验单变更日志
    /// </summary>
    /// <param name="id">制程检验单变更日志ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktIpqcOrderChangeLogDto> UpdateIpqcOrderChangeLogAsync(long id, TaktIpqcOrderChangeLogUpdateDto dto)
    {
        var entity = await _ipqcOrderChangeLogRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("制程检验单变更日志不存在");
        }
        dto.Adapt(entity);
                await StampIpqcOrderChangeLogIpqcOrderAsync(entity, dto);
        await _ipqcOrderChangeLogRepository.UpdateAsync(entity);
        return await GetIpqcOrderChangeLogByIdAsync(id) ?? throw new TaktBusinessException("制程检验单变更日志不存在");
    }

    /// <summary>
    /// 删除制程检验单变更日志
    /// </summary>
    /// <param name="id">制程检验单变更日志ID</param>
    /// <returns>任务</returns>
    public async Task DeleteIpqcOrderChangeLogByIdAsync(long id)
    {
        var deleted = await _ipqcOrderChangeLogRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("制程检验单变更日志不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除制程检验单变更日志
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteIpqcOrderChangeLogBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteIpqcOrderChangeLogByIdAsync(id);
        }
    }

    /// <summary>
    /// 导出制程检验单变更日志
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportIpqcOrderChangeLogAsync(TaktIpqcOrderChangeLogQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktIpqcOrderChangeLogQueryDto());
        var list = await _ipqcOrderChangeLogRepository.GetListForExportAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktIpqcOrderChangeLogExportDto>(),
                sheetName ?? "制程检验单变更日志数据",
                fileName ?? "制程检验单变更日志导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktIpqcOrderChangeLogExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "制程检验单变更日志数据",
            fileName ?? "制程检验单变更日志导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步制程检验单变更日志主表外键（ManyToOne → 制程检验单）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampIpqcOrderChangeLogIpqcOrderAsync(TaktIpqcOrderChangeLog entity, TaktIpqcOrderChangeLogCreateDto dto)
    {
        if (dto.IpqcOrderId <= 0)
        {
            return;
        }
        var master = await _ipqcOrderRepository.GetByIdAsync(dto.IpqcOrderId);
        if (master == null)
        {
            throw new TaktBusinessException("制程检验单不存在");
        }
        entity.IpqcOrderId = master.Id;
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建制程检验单变更日志查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktIpqcOrderChangeLog, bool>> QueryExpression(TaktIpqcOrderChangeLogQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktIpqcOrderChangeLog>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.IpqcOrderId).Contains(keywords)
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

        if (queryDto?.IpqcOrderId.HasValue == true)
        {
            exp = exp.And(x => x.IpqcOrderId == queryDto.IpqcOrderId);
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
