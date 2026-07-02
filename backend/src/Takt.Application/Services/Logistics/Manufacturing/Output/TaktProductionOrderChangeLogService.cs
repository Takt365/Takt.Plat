// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Output
// 文件名称：TaktProductionOrderChangeLogService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：生产工单变更记录应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Output;
using Takt.Domain.Entities.Logistics.Manufacturing.Output;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Output;

/// <summary>
/// 生产工单变更记录应用服务
/// </summary>
public class TaktProductionOrderChangeLogService : TaktServiceBase, ITaktProductionOrderChangeLogService
{
    private readonly ITaktCompanyRepository<TaktProductionOrderChangeLog> _productionOrderChangeLogRepository;
    private readonly ITaktCompanyRepository<TaktProductionOrder> _productionOrderRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="productionOrderChangeLogRepository">生产工单变更记录仓储</param>
    /// <param name="productionOrderRepository">生产工单仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktProductionOrderChangeLogService(
        ITaktCompanyRepository<TaktProductionOrderChangeLog> productionOrderChangeLogRepository,
        ITaktCompanyRepository<TaktProductionOrder> productionOrderRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _productionOrderChangeLogRepository = productionOrderChangeLogRepository;
        _productionOrderRepository = productionOrderRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取生产工单变更记录列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktProductionOrderChangeLogDto>> GetProductionOrderChangeLogListAsync(TaktProductionOrderChangeLogQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _productionOrderChangeLogRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktProductionOrderChangeLogDto>.Create(
            data.Adapt<List<TaktProductionOrderChangeLogDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取生产工单变更记录
    /// </summary>
    /// <param name="id">生产工单变更记录ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktProductionOrderChangeLogDto?> GetProductionOrderChangeLogByIdAsync(long id)
    {
        var entity = await _productionOrderChangeLogRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktProductionOrderChangeLogDto>();
    }

    /// <summary>
    /// 获取生产工单变更记录选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetProductionOrderChangeLogOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _productionOrderChangeLogRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.ProdOrderCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.ProdOrderCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建生产工单变更记录
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktProductionOrderChangeLogDto> CreateProductionOrderChangeLogAsync(TaktProductionOrderChangeLogCreateDto dto)
    {
        var entity = dto.Adapt<TaktProductionOrderChangeLog>();
        await StampProductionOrderChangeLogProductionOrderAsync(entity, dto);
        entity = await _productionOrderChangeLogRepository.CreateAsync(entity);
        return await GetProductionOrderChangeLogByIdAsync(entity.Id) ?? entity.Adapt<TaktProductionOrderChangeLogDto>();
    }

    /// <summary>
    /// 更新生产工单变更记录
    /// </summary>
    /// <param name="id">生产工单变更记录ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktProductionOrderChangeLogDto> UpdateProductionOrderChangeLogAsync(long id, TaktProductionOrderChangeLogUpdateDto dto)
    {
        var entity = await _productionOrderChangeLogRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("生产工单变更记录不存在");
        }
        dto.Adapt(entity);
        await StampProductionOrderChangeLogProductionOrderAsync(entity, dto);
        await _productionOrderChangeLogRepository.UpdateAsync(entity);
        return await GetProductionOrderChangeLogByIdAsync(id) ?? throw new TaktBusinessException("生产工单变更记录不存在");
    }

    /// <summary>
    /// 删除生产工单变更记录
    /// </summary>
    /// <param name="id">生产工单变更记录ID</param>
    /// <returns>任务</returns>
    public async Task DeleteProductionOrderChangeLogByIdAsync(long id)
    {
        var deleted = await _productionOrderChangeLogRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("生产工单变更记录不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除生产工单变更记录
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteProductionOrderChangeLogBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteProductionOrderChangeLogByIdAsync(id);
        }
    }

    /// <summary>
    /// 导出生产工单变更记录
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportProductionOrderChangeLogAsync(TaktProductionOrderChangeLogQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktProductionOrderChangeLogQueryDto());
        var list = await _productionOrderChangeLogRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktProductionOrderChangeLogExportDto>(),
                sheetName ?? "生产工单变更记录数据",
                fileName ?? "生产工单变更记录导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktProductionOrderChangeLogExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "生产工单变更记录数据",
            fileName ?? "生产工单变更记录导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步生产工单变更记录主表外键（ManyToOne → 生产工单）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampProductionOrderChangeLogProductionOrderAsync(TaktProductionOrderChangeLog entity, TaktProductionOrderChangeLogCreateDto dto)
    {
        if (dto.ProductionOrderId <= 0)
        {
            return;
        }
        var master = await _productionOrderRepository.GetByIdAsync(dto.ProductionOrderId);
        if (master == null)
        {
            throw new TaktBusinessException("生产工单不存在");
        }
        entity.ProductionOrderId = master.Id;
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建生产工单变更记录查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktProductionOrderChangeLog, bool>> QueryExpression(TaktProductionOrderChangeLogQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktProductionOrderChangeLog>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.ProductionOrderId).Contains(keywords)
                || (x.ProdOrderCode != null && x.ProdOrderCode.Contains(keywords))
                || (x.ChangeFields != null && x.ChangeFields.Contains(keywords))
                || (x.ChangeBy != null && x.ChangeBy.Contains(keywords))
                || (x.ChangeReason != null && x.ChangeReason.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.ChangeTime).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.ProductionOrderId.HasValue == true)
        {
            exp = exp.And(x => x.ProductionOrderId == queryDto.ProductionOrderId);
        }

        if (!string.IsNullOrEmpty(queryDto?.ProdOrderCode))
        {
            exp = exp.And(x => x.ProdOrderCode != null && x.ProdOrderCode.Contains(queryDto.ProdOrderCode));
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
