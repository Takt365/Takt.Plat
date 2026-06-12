// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Sales
// 文件名称：TaktSalesPriceChangeLogService.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：销售价格变更记录应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Sales;
using Takt.Domain.Entities.Logistics.Sales;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Sales;

/// <summary>
/// 销售价格变更记录应用服务
/// </summary>
public class TaktSalesPriceChangeLogService : TaktServiceBase, ITaktSalesPriceChangeLogService
{
    private readonly ITaktCompanyRepository<TaktSalesPriceChangeLog> _salesPriceChangeLogRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="salesPriceChangeLogRepository">销售价格变更记录仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSalesPriceChangeLogService(
        ITaktCompanyRepository<TaktSalesPriceChangeLog> salesPriceChangeLogRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _salesPriceChangeLogRepository = salesPriceChangeLogRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取销售价格变更记录列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSalesPriceChangeLogDto>> GetSalesPriceChangeLogListAsync(TaktSalesPriceChangeLogQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _salesPriceChangeLogRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSalesPriceChangeLogDto>.Create(
            data.Adapt<List<TaktSalesPriceChangeLogDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取销售价格变更记录
    /// </summary>
    /// <param name="id">销售价格变更记录ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesPriceChangeLogDto?> GetSalesPriceChangeLogByIdAsync(long id)
    {
        var entity = await _salesPriceChangeLogRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktSalesPriceChangeLogDto>();
    }

    /// <summary>
    /// 获取销售价格变更记录选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSalesPriceChangeLogOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _salesPriceChangeLogRepository.GetListAsync(
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
    /// 创建销售价格变更记录
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesPriceChangeLogDto> CreateSalesPriceChangeLogAsync(TaktSalesPriceChangeLogCreateDto dto)
    {
        var entity = dto.Adapt<TaktSalesPriceChangeLog>();
        entity = await _salesPriceChangeLogRepository.CreateAsync(entity);
        return await GetSalesPriceChangeLogByIdAsync(entity.Id) ?? entity.Adapt<TaktSalesPriceChangeLogDto>();
    }

    /// <summary>
    /// 更新销售价格变更记录
    /// </summary>
    /// <param name="id">销售价格变更记录ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesPriceChangeLogDto> UpdateSalesPriceChangeLogAsync(long id, TaktSalesPriceChangeLogUpdateDto dto)
    {
        var entity = await _salesPriceChangeLogRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("销售价格变更记录不存在");
        }
        dto.Adapt(entity);
        await _salesPriceChangeLogRepository.UpdateAsync(entity);
        return await GetSalesPriceChangeLogByIdAsync(id) ?? throw new TaktBusinessException("销售价格变更记录不存在");
    }

    /// <summary>
    /// 删除销售价格变更记录
    /// </summary>
    /// <param name="id">销售价格变更记录ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSalesPriceChangeLogByIdAsync(long id)
    {
        var deleted = await _salesPriceChangeLogRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("销售价格变更记录不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除销售价格变更记录
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSalesPriceChangeLogBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSalesPriceChangeLogByIdAsync(id);
        }
    }

    /// <summary>
    /// 导出销售价格变更记录
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSalesPriceChangeLogAsync(TaktSalesPriceChangeLogQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktSalesPriceChangeLogQueryDto());
        var list = await _salesPriceChangeLogRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSalesPriceChangeLogExportDto>(),
                sheetName ?? "销售价格变更记录数据",
                fileName ?? "销售价格变更记录导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSalesPriceChangeLogExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "销售价格变更记录数据",
            fileName ?? "销售价格变更记录导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建销售价格变更记录查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSalesPriceChangeLog, bool>> QueryExpression(TaktSalesPriceChangeLogQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSalesPriceChangeLog>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.SalesPriceId).Contains(keywords)
                || (x.ChangeFields != null && x.ChangeFields.Contains(keywords))
                || (x.ChangeBy != null && x.ChangeBy.Contains(keywords))
                || (x.ChangeReason != null && x.ChangeReason.Contains(keywords))
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.ChangeTime).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.SalesPriceId.HasValue == true)
        {
            exp = exp.And(x => x.SalesPriceId == queryDto.SalesPriceId);
        }

        if (!string.IsNullOrEmpty(queryDto?.ChangeFields))
        {
            exp = exp.And(x => x.ChangeFields != null && x.ChangeFields.Contains(queryDto.ChangeFields));
        }

        if (!string.IsNullOrEmpty(queryDto?.ChangeBy))
        {
            exp = exp.And(x => x.ChangeBy != null && x.ChangeBy.Contains(queryDto.ChangeBy));
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
