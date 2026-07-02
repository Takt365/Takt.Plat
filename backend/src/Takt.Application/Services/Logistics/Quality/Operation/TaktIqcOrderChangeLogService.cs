// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Quality.Operation
// 文件名称：TaktIqcOrderChangeLogService.cs
// 创建时间：2026-06-30
// 创建人：Takt365(Cursor AI)
// 功能描述：进货检验单变更日志应用服务实现
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

namespace Takt.Application.Services.Logistics.Quality.Operation;

/// <summary>
/// 进货检验单变更日志应用服务
/// </summary>
public class TaktIqcOrderChangeLogService : TaktServiceBase, ITaktIqcOrderChangeLogService
{
    private readonly ITaktCompanyRepository<TaktIqcOrderChangeLog> _iqcOrderChangeLogRepository;
    private readonly ITaktCompanyRepository<TaktIqcOrder> _iqcOrderRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="iqcOrderChangeLogRepository">进货检验单变更日志仓储</param>
    /// <param name="iqcOrderRepository">进货检验单仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktIqcOrderChangeLogService(
        ITaktCompanyRepository<TaktIqcOrderChangeLog> iqcOrderChangeLogRepository,
        ITaktCompanyRepository<TaktIqcOrder> iqcOrderRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _iqcOrderChangeLogRepository = iqcOrderChangeLogRepository;
        _iqcOrderRepository = iqcOrderRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取进货检验单变更日志列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktIqcOrderChangeLogDto>> GetIqcOrderChangeLogListAsync(TaktIqcOrderChangeLogQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _iqcOrderChangeLogRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktIqcOrderChangeLogDto>.Create(
            data.Adapt<List<TaktIqcOrderChangeLogDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取进货检验单变更日志
    /// </summary>
    /// <param name="id">进货检验单变更日志ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktIqcOrderChangeLogDto?> GetIqcOrderChangeLogByIdAsync(long id)
    {
        var entity = await _iqcOrderChangeLogRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktIqcOrderChangeLogDto>();
    }

    /// <summary>
    /// 获取进货检验单变更日志选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetIqcOrderChangeLogOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _iqcOrderChangeLogRepository.GetListAsync(
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
    /// 创建进货检验单变更日志
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktIqcOrderChangeLogDto> CreateIqcOrderChangeLogAsync(TaktIqcOrderChangeLogCreateDto dto)
    {
        var entity = dto.Adapt<TaktIqcOrderChangeLog>();
        await StampIqcOrderChangeLogIqcOrderAsync(entity, dto);
        entity = await _iqcOrderChangeLogRepository.CreateAsync(entity);
        return await GetIqcOrderChangeLogByIdAsync(entity.Id) ?? entity.Adapt<TaktIqcOrderChangeLogDto>();
    }

    /// <summary>
    /// 更新进货检验单变更日志
    /// </summary>
    /// <param name="id">进货检验单变更日志ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktIqcOrderChangeLogDto> UpdateIqcOrderChangeLogAsync(long id, TaktIqcOrderChangeLogUpdateDto dto)
    {
        var entity = await _iqcOrderChangeLogRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("进货检验单变更日志不存在");
        }
        dto.Adapt(entity);
        await StampIqcOrderChangeLogIqcOrderAsync(entity, dto);
        await _iqcOrderChangeLogRepository.UpdateAsync(entity);
        return await GetIqcOrderChangeLogByIdAsync(id) ?? throw new TaktBusinessException("进货检验单变更日志不存在");
    }

    /// <summary>
    /// 删除进货检验单变更日志
    /// </summary>
    /// <param name="id">进货检验单变更日志ID</param>
    /// <returns>任务</returns>
    public async Task DeleteIqcOrderChangeLogByIdAsync(long id)
    {
        var deleted = await _iqcOrderChangeLogRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("进货检验单变更日志不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除进货检验单变更日志
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteIqcOrderChangeLogBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteIqcOrderChangeLogByIdAsync(id);
        }
    }

    /// <summary>
    /// 导出进货检验单变更日志
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportIqcOrderChangeLogAsync(TaktIqcOrderChangeLogQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktIqcOrderChangeLogQueryDto());
        var list = await _iqcOrderChangeLogRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktIqcOrderChangeLogExportDto>(),
                sheetName ?? "进货检验单变更日志数据",
                fileName ?? "进货检验单变更日志导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktIqcOrderChangeLogExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "进货检验单变更日志数据",
            fileName ?? "进货检验单变更日志导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步进货检验单变更日志主表外键（ManyToOne → 进货检验单）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampIqcOrderChangeLogIqcOrderAsync(TaktIqcOrderChangeLog entity, TaktIqcOrderChangeLogCreateDto dto)
    {
        if (dto.IqcOrderId <= 0)
        {
            return;
        }
        var master = await _iqcOrderRepository.GetByIdAsync(dto.IqcOrderId);
        if (master == null)
        {
            throw new TaktBusinessException("进货检验单不存在");
        }
        entity.IqcOrderId = master.Id;
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建进货检验单变更日志查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktIqcOrderChangeLog, bool>> QueryExpression(TaktIqcOrderChangeLogQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktIqcOrderChangeLog>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.IqcOrderId).Contains(keywords)
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

        if (queryDto?.IqcOrderId.HasValue == true)
        {
            exp = exp.And(x => x.IqcOrderId == queryDto.IqcOrderId);
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
