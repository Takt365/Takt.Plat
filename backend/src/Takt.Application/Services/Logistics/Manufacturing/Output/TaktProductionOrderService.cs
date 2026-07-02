// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Output
// 文件名称：TaktProductionOrderService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：生产工单应用服务实现
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
/// 生产工单应用服务
/// </summary>
public class TaktProductionOrderService : TaktServiceBase, ITaktProductionOrderService
{
    private readonly ITaktCompanyRepository<TaktProductionOrder> _productionOrderRepository;
    private readonly ITaktCompanyRepository<TaktProductionOrderChangeLog> _productionOrderChangeLogRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="productionOrderRepository">生产工单仓储</param>
    /// <param name="productionOrderChangeLogRepository">ProductionOrderChangeLog仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktProductionOrderService(
        ITaktCompanyRepository<TaktProductionOrder> productionOrderRepository,
        ITaktCompanyRepository<TaktProductionOrderChangeLog> productionOrderChangeLogRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _productionOrderRepository = productionOrderRepository;
        _productionOrderChangeLogRepository = productionOrderChangeLogRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取生产工单列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktProductionOrderDto>> GetProductionOrderListAsync(TaktProductionOrderQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _productionOrderRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktProductionOrderDto>.Create(
            data.Adapt<List<TaktProductionOrderDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取生产工单
    /// </summary>
    /// <param name="id">生产工单ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktProductionOrderDto?> GetProductionOrderByIdAsync(long id)
    {
        var entity = await _productionOrderRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktProductionOrderDto>();
        await FillProductionOrderDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取生产工单选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetProductionOrderOptionsAsync(string? plantCode = null)
    {
        EnsureThreeLayerContext();
        var list = await _productionOrderRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.Status == 1
                && (string.IsNullOrWhiteSpace(plantCode) || x.PlantCode == plantCode),
            x => x.ProdOrderCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.ProdOrderCode,
            DictLabel = string.IsNullOrWhiteSpace(e.MaterialCode)
                ? e.ProdOrderCode
                : $"{e.ProdOrderCode}{e.MaterialCode}",
            ExtValue = e.ProdOrderQty,
            ExtLabel = e.ProdOrderType,
        }).ToList();
    }

    /// <summary>
    /// 创建生产工单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktProductionOrderDto> CreateProductionOrderAsync(TaktProductionOrderCreateDto dto)
    {
        var entity = dto.Adapt<TaktProductionOrder>();
        var isUnique_ix_takt_logistics_manufacturing_output_production_order_plant_order_unique = await _uniqueValidator.IsUniqueAsync(
            _productionOrderRepository,
            x => x.PlantCode == entity.PlantCode
                && x.ProdOrderType == entity.ProdOrderType
                && x.ProdOrderCode == entity.ProdOrderCode
                && x.MaterialCode == entity.MaterialCode);
        if (!isUnique_ix_takt_logistics_manufacturing_output_production_order_plant_order_unique)
        {
            throw new TaktBusinessException("生产工单的PlantCode、ProdOrderType、ProdOrderCode、MaterialCode已存在");
        }
        entity = await _productionOrderRepository.CreateAsync(entity);
                await SaveProductionOrderChildrenAsync(entity, dto);
        return await GetProductionOrderByIdAsync(entity.Id) ?? entity.Adapt<TaktProductionOrderDto>();
    }

    /// <summary>
    /// 更新生产工单
    /// </summary>
    /// <param name="id">生产工单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktProductionOrderDto> UpdateProductionOrderAsync(long id, TaktProductionOrderUpdateDto dto)
    {
        var entity = await _productionOrderRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("生产工单不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_output_production_order_plant_order_unique = await _uniqueValidator.IsUniqueAsync(
            _productionOrderRepository,
            x => x.PlantCode == entity.PlantCode
                && x.ProdOrderType == entity.ProdOrderType
                && x.ProdOrderCode == entity.ProdOrderCode
                && x.MaterialCode == entity.MaterialCode,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_output_production_order_plant_order_unique)
        {
            throw new TaktBusinessException("生产工单的PlantCode、ProdOrderType、ProdOrderCode、MaterialCode已存在");
        }
        await _productionOrderRepository.UpdateAsync(entity);
                await SaveProductionOrderChildrenAsync(entity, dto);
        return await GetProductionOrderByIdAsync(id) ?? throw new TaktBusinessException("生产工单不存在");
    }

    /// <summary>
    /// 删除生产工单
    /// </summary>
    /// <param name="id">生产工单ID</param>
    /// <returns>任务</returns>
    public async Task DeleteProductionOrderByIdAsync(long id)
    {
        var entity = await _productionOrderRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("生产工单不存在或已删除");
        }
        await _productionOrderChangeLogRepository.DeleteAsync(x => x.ProductionOrderId == entity.Id);
        var deleted = await _productionOrderRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("生产工单不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除生产工单
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteProductionOrderBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteProductionOrderByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新生产工单状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktProductionOrderDto> UpdateProductionOrderStatusAsync(TaktProductionOrderStatusDto dto)
    {
        var entity = await _productionOrderRepository.GetByIdAsync(dto.ProductionOrderId);
        if (entity == null)
        {
            throw new TaktBusinessException("生产工单不存在");
        }
        entity.Status = dto.ProductionOrderStatus;
        await _productionOrderRepository.UpdateAsync(entity);
        return await GetProductionOrderByIdAsync(dto.ProductionOrderId) ?? throw new TaktBusinessException("生产工单不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetProductionOrderTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktProductionOrderTemplateDto>(
            sheetName ?? "生产工单导入模板",
            fileName ?? "生产工单导入模板.xlsx");
    }

    /// <summary>
    /// 导入生产工单
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportProductionOrderAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktProductionOrderImportDto>(fileStream, sheetName ?? "生产工单导入模板");
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
                var entity = rows[i].Adapt<TaktProductionOrder>();
                var importKey = $"{entity.PlantCode}|{entity.ProdOrderType}|{entity.ProdOrderCode}|{entity.MaterialCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、ProdOrderType、ProdOrderCode、MaterialCode）");
                }
                var isUnique_ix_takt_logistics_manufacturing_output_production_order_plant_order_unique = await _uniqueValidator.IsUniqueAsync(
                    _productionOrderRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.ProdOrderType == entity.ProdOrderType
                        && x.ProdOrderCode == entity.ProdOrderCode
                        && x.MaterialCode == entity.MaterialCode);
                if (!isUnique_ix_takt_logistics_manufacturing_output_production_order_plant_order_unique)
                {
                    throw new TaktBusinessException("生产工单的PlantCode、ProdOrderType、ProdOrderCode、MaterialCode已存在");
                }
                await _productionOrderRepository.CreateAsync(entity);
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
    /// 导出生产工单
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportProductionOrderAsync(TaktProductionOrderQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktProductionOrderQueryDto());
        var list = await _productionOrderRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktProductionOrderExportDto>(),
                sheetName ?? "生产工单数据",
                fileName ?? "生产工单导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktProductionOrderExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "生产工单数据",
            fileName ?? "生产工单导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充生产工单详情（加载 OneToMany 子表：生产工单变更记录）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillProductionOrderDetailsAsync(TaktProductionOrderDto dto, TaktProductionOrder entity)
    {
        if (dto == null)
        {
            return;
        }
        // 生产工单变更记录 → dto.ChangeLogs
        var changelogs = await _productionOrderChangeLogRepository.GetListAsync(x => x.ProductionOrderId == entity.Id);
        dto.ChangeLogs = changelogs.Adapt<List<TaktProductionOrderChangeLogDto>>();
    }

    /// <summary>
    /// 保存生产工单子表级联（生产工单变更记录；Create/Update 后按主表 Id 先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveProductionOrderChildrenAsync(TaktProductionOrder entity, TaktProductionOrderCreateDto dto)
    {
        // 生产工单变更记录（ChangeLogs）
        if (dto.ChangeLogs is not { Count: > 0 })
        {
            await _productionOrderChangeLogRepository.DeleteAsync(x => x.ProductionOrderId == entity.Id);
        }
        else
        {
            var changelogs = dto.ChangeLogs.Adapt<List<TaktProductionOrderChangeLog>>();
            foreach (var child in changelogs)
            {
                child.ProductionOrderId = entity.Id;
            }
            await _productionOrderChangeLogRepository.DeleteAsync(x => x.ProductionOrderId == entity.Id);
            foreach (var child in changelogs)
            {
            }
            await _productionOrderChangeLogRepository.CreateRangeAsync(changelogs);
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建生产工单查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktProductionOrder, bool>> QueryExpression(TaktProductionOrderQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktProductionOrder>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.ProdOrderType != null && x.ProdOrderType.Contains(keywords))
                || (x.ProdOrderCode != null && x.ProdOrderCode.Contains(keywords))
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || SqlFunc.ToString(x.ProdOrderQty).Contains(keywords)
                || SqlFunc.ToString(x.ProducedQty).Contains(keywords)
                || (x.UnitOfMeasure != null && x.UnitOfMeasure.Contains(keywords))
                || SqlFunc.ToString(x.Priority).Contains(keywords)
                || (x.WorkCenter != null && x.WorkCenter.Contains(keywords))
                || (x.ProdBatch != null && x.ProdBatch.Contains(keywords))
                || (x.SerialNo != null && x.SerialNo.Contains(keywords))
                || (x.RoutingCode != null && x.RoutingCode.Contains(keywords))
                || SqlFunc.ToString(x.PlannedOrderId).Contains(keywords)
                || SqlFunc.ToString(x.ApsOrderId).Contains(keywords)
                || SqlFunc.ToString(x.Status).Contains(keywords)
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.ActualStartDate).Contains(keywords)
                || SqlFunc.ToString(x.ActualEndDate).Contains(keywords)
                || SqlFunc.ToString(x.PlannedStartTime).Contains(keywords)
                || SqlFunc.ToString(x.PlannedEndTime).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ProdOrderType))
        {
            exp = exp.And(x => x.ProdOrderType != null && x.ProdOrderType.Contains(queryDto.ProdOrderType));
        }

        if (!string.IsNullOrEmpty(queryDto?.ProdOrderCode))
        {
            exp = exp.And(x => x.ProdOrderCode != null && x.ProdOrderCode.Contains(queryDto.ProdOrderCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialCode))
        {
            exp = exp.And(x => x.MaterialCode != null && x.MaterialCode.Contains(queryDto.MaterialCode));
        }

        if (queryDto?.ProdOrderQty.HasValue == true)
        {
            exp = exp.And(x => x.ProdOrderQty == queryDto.ProdOrderQty);
        }

        if (queryDto?.ProducedQty.HasValue == true)
        {
            exp = exp.And(x => x.ProducedQty == queryDto.ProducedQty);
        }

        if (!string.IsNullOrEmpty(queryDto?.UnitOfMeasure))
        {
            exp = exp.And(x => x.UnitOfMeasure != null && x.UnitOfMeasure.Contains(queryDto.UnitOfMeasure));
        }

        if (queryDto?.Priority.HasValue == true)
        {
            exp = exp.And(x => x.Priority == queryDto.Priority);
        }

        if (!string.IsNullOrEmpty(queryDto?.WorkCenter))
        {
            exp = exp.And(x => x.WorkCenter != null && x.WorkCenter.Contains(queryDto.WorkCenter));
        }

        if (!string.IsNullOrEmpty(queryDto?.ProdBatch))
        {
            exp = exp.And(x => x.ProdBatch != null && x.ProdBatch.Contains(queryDto.ProdBatch));
        }

        if (!string.IsNullOrEmpty(queryDto?.SerialNo))
        {
            exp = exp.And(x => x.SerialNo != null && x.SerialNo.Contains(queryDto.SerialNo));
        }

        if (!string.IsNullOrEmpty(queryDto?.RoutingCode))
        {
            exp = exp.And(x => x.RoutingCode != null && x.RoutingCode.Contains(queryDto.RoutingCode));
        }

        if (queryDto?.PlannedOrderId.HasValue == true)
        {
            exp = exp.And(x => x.PlannedOrderId == queryDto.PlannedOrderId);
        }

        if (queryDto?.ApsOrderId.HasValue == true)
        {
            exp = exp.And(x => x.ApsOrderId == queryDto.ApsOrderId);
        }

        if (queryDto?.ProductionOrderStatus.HasValue == true)
        {
            exp = exp.And(x => x.Status == queryDto.ProductionOrderStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.ActualStartDateStart.HasValue == true)
        {
            exp = exp.And(x => x.ActualStartDate >= queryDto.ActualStartDateStart);
        }

        if (queryDto?.ActualStartDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.ActualStartDate <= queryDto.ActualStartDateEnd);
        }

        if (queryDto?.ActualEndDateStart.HasValue == true)
        {
            exp = exp.And(x => x.ActualEndDate >= queryDto.ActualEndDateStart);
        }

        if (queryDto?.ActualEndDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.ActualEndDate <= queryDto.ActualEndDateEnd);
        }

        if (queryDto?.PlannedStartTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.PlannedStartTime >= queryDto.PlannedStartTimeStart);
        }

        if (queryDto?.PlannedStartTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.PlannedStartTime <= queryDto.PlannedStartTimeEnd);
        }

        if (queryDto?.PlannedEndTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.PlannedEndTime >= queryDto.PlannedEndTimeStart);
        }

        if (queryDto?.PlannedEndTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.PlannedEndTime <= queryDto.PlannedEndTimeEnd);
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
