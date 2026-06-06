// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Materials
// 文件名称：TaktPurchaseOrderService.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Cursor AI)
// 功能描述：采购订单应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Materials;
using Takt.Domain.Entities.Logistics.Materials;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;
using Takt.Domain.Entities.Logistics.Materials;

namespace Takt.Application.Services.Logistics.Materials;

/// <summary>
/// 采购订单应用服务
/// </summary>
public class TaktPurchaseOrderService : TaktServiceBase, ITaktPurchaseOrderService
{
    private readonly ITaktCompanyRepository<TaktPurchaseOrder> _purchaseOrderRepository;
    private readonly ITaktCompanyRepository<TaktPurchaseOrderItem> _purchaseOrderItemRepository;
    private readonly ITaktCompanyRepository<TaktPurchaseOrderChangeLog> _purchaseOrderChangeLogRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="purchaseOrderRepository">采购订单仓储</param>
    /// <param name="purchaseOrderItemRepository">PurchaseOrderItem仓储</param>
    /// <param name="purchaseOrderChangeLogRepository">PurchaseOrderChangeLog仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPurchaseOrderService(
        ITaktCompanyRepository<TaktPurchaseOrder> purchaseOrderRepository,
        ITaktCompanyRepository<TaktPurchaseOrderItem> purchaseOrderItemRepository,
        ITaktCompanyRepository<TaktPurchaseOrderChangeLog> purchaseOrderChangeLogRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _purchaseOrderRepository = purchaseOrderRepository;
        _purchaseOrderItemRepository = purchaseOrderItemRepository;
        _purchaseOrderChangeLogRepository = purchaseOrderChangeLogRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取采购订单列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktPurchaseOrderDto>> GetPurchaseOrderListAsync(TaktPurchaseOrderQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _purchaseOrderRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktPurchaseOrderDto>.Create(
            data.Adapt<List<TaktPurchaseOrderDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取采购订单
    /// </summary>
    /// <param name="id">采购订单ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseOrderDto?> GetPurchaseOrderByIdAsync(long id)
    {
        var entity = await _purchaseOrderRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktPurchaseOrderDto>();
        await FillPurchaseOrderDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取采购订单选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetPurchaseOrderOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _purchaseOrderRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.SupplierName,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.SupplierName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建采购订单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseOrderDto> CreatePurchaseOrderAsync(TaktPurchaseOrderCreateDto dto)
    {
        var entity = dto.Adapt<TaktPurchaseOrder>();
        var isUnique_ix_takt_logistics_materials_purchase_order_po_unique = await _uniqueValidator.IsUniqueAsync(
            _purchaseOrderRepository,
            x => x.PlantCode == entity.PlantCode
                && x.PurchaseOrderCode == entity.PurchaseOrderCode
                && x.SupplierCode == entity.SupplierCode
                && x.OrderDate == entity.OrderDate);
        if (!isUnique_ix_takt_logistics_materials_purchase_order_po_unique)
        {
            throw new TaktBusinessException("采购订单的PlantCode、PurchaseOrderCode、SupplierCode、OrderDate已存在");
        }
        entity = await _purchaseOrderRepository.CreateAsync(entity);
                await SavePurchaseOrderChildrenAsync(entity, dto);
        return await GetPurchaseOrderByIdAsync(entity.Id) ?? entity.Adapt<TaktPurchaseOrderDto>();
    }

    /// <summary>
    /// 更新采购订单
    /// </summary>
    /// <param name="id">采购订单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseOrderDto> UpdatePurchaseOrderAsync(long id, TaktPurchaseOrderUpdateDto dto)
    {
        var entity = await _purchaseOrderRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("采购订单不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_materials_purchase_order_po_unique = await _uniqueValidator.IsUniqueAsync(
            _purchaseOrderRepository,
            x => x.PlantCode == entity.PlantCode
                && x.PurchaseOrderCode == entity.PurchaseOrderCode
                && x.SupplierCode == entity.SupplierCode
                && x.OrderDate == entity.OrderDate,
            id);
        if (!isUnique_ix_takt_logistics_materials_purchase_order_po_unique)
        {
            throw new TaktBusinessException("采购订单的PlantCode、PurchaseOrderCode、SupplierCode、OrderDate已存在");
        }
        await _purchaseOrderRepository.UpdateAsync(entity);
                await SavePurchaseOrderChildrenAsync(entity, dto);
        return await GetPurchaseOrderByIdAsync(id) ?? throw new TaktBusinessException("采购订单不存在");
    }

    /// <summary>
    /// 删除采购订单
    /// </summary>
    /// <param name="id">采购订单ID</param>
    /// <returns>任务</returns>
    public async Task DeletePurchaseOrderByIdAsync(long id)
    {
        var entity = await _purchaseOrderRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("采购订单不存在或已删除");
        }
        await _purchaseOrderItemRepository.DeleteAsync(x => x.PurchaseOrderId == entity.Id);
        await _purchaseOrderChangeLogRepository.DeleteAsync(x => x.PurchaseOrderId == entity.Id);
        var deleted = await _purchaseOrderRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("采购订单不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除采购订单
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeletePurchaseOrderBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeletePurchaseOrderByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新采购订单状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseOrderDto> UpdatePurchaseOrderStatusAsync(TaktPurchaseOrderStatusDto dto)
    {
        var entity = await _purchaseOrderRepository.GetByIdAsync(dto.PurchaseOrderId);
        if (entity == null)
        {
            throw new TaktBusinessException("采购订单不存在");
        }
        entity.OrderStatus = dto.OrderStatus;
        await _purchaseOrderRepository.UpdateAsync(entity);
        return await GetPurchaseOrderByIdAsync(dto.PurchaseOrderId) ?? throw new TaktBusinessException("采购订单不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetPurchaseOrderTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktPurchaseOrderTemplateDto>(
            sheetName ?? "采购订单导入模板",
            fileName ?? "采购订单导入模板.xlsx");
    }

    /// <summary>
    /// 导入采购订单
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportPurchaseOrderAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktPurchaseOrderImportDto>(fileStream, sheetName ?? "采购订单导入模板");
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
                var entity = rows[i].Adapt<TaktPurchaseOrder>();
                var importKey = $"{entity.PlantCode}|{entity.PurchaseOrderCode}|{entity.SupplierCode}|{entity.OrderDate}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、PurchaseOrderCode、SupplierCode、OrderDate）");
                }
                var isUnique_ix_takt_logistics_materials_purchase_order_po_unique = await _uniqueValidator.IsUniqueAsync(
                    _purchaseOrderRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.PurchaseOrderCode == entity.PurchaseOrderCode
                        && x.SupplierCode == entity.SupplierCode
                        && x.OrderDate == entity.OrderDate);
                if (!isUnique_ix_takt_logistics_materials_purchase_order_po_unique)
                {
                    throw new TaktBusinessException("采购订单的PlantCode、PurchaseOrderCode、SupplierCode、OrderDate已存在");
                }
                await _purchaseOrderRepository.CreateAsync(entity);
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
    /// 导出采购订单
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportPurchaseOrderAsync(TaktPurchaseOrderQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktPurchaseOrderQueryDto());
        var list = await _purchaseOrderRepository.GetListForExportAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPurchaseOrderExportDto>(),
                sheetName ?? "采购订单数据",
                fileName ?? "采购订单导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktPurchaseOrderExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "采购订单数据",
            fileName ?? "采购订单导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充采购订单详情（加载 OneToMany 子表：采购订单明细、采购订单变更记录）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillPurchaseOrderDetailsAsync(TaktPurchaseOrderDto dto, TaktPurchaseOrder entity)
    {
        if (dto == null)
        {
            return;
        }
        // 采购订单明细 → dto.Items
        var items = await _purchaseOrderItemRepository.GetListAsync(x => x.PurchaseOrderId == entity.Id);
        dto.Items = items.Adapt<List<TaktPurchaseOrderItemDto>>();
        // 采购订单变更记录 → dto.ChangeLogs
        var changelogs = await _purchaseOrderChangeLogRepository.GetListAsync(x => x.PurchaseOrderId == entity.Id);
        dto.ChangeLogs = changelogs.Adapt<List<TaktPurchaseOrderChangeLogDto>>();
    }

    /// <summary>
    /// 保存采购订单子表级联（采购订单明细、采购订单变更记录；Create/Update 后按主表 Id 先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SavePurchaseOrderChildrenAsync(TaktPurchaseOrder entity, TaktPurchaseOrderCreateDto dto)
    {
        // 采购订单明细（Items）
        if (dto.Items is not { Count: > 0 })
        {
            await _purchaseOrderItemRepository.DeleteAsync(x => x.PurchaseOrderId == entity.Id);
        }
        else
        {
            var items = dto.Items.Adapt<List<TaktPurchaseOrderItem>>();
            foreach (var child in items)
            {
                child.PurchaseOrderId = entity.Id;
            }
            var itemsNeedLine = items.Where(c => c.LineNumber <= 0).ToList();
            if (itemsNeedLine.Count > 0)
            {
                var businessCode = !string.IsNullOrWhiteSpace(entity.PurchaseOrderCode) ? entity.PurchaseOrderCode : entity.Id.ToString();
                var maxLine = await _purchaseOrderItemRepository.GetMaxIntAsync(
                    x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.PurchaseOrderId == entity.Id,
                    x => x.LineNumber);
                var lineSeq = _lineNumberGenerator.GenerateSequence(businessCode, itemsNeedLine.Count, maxLine).ToList();
                var lineIdx = 0;
                foreach (var child in items)
                {
                    if (child.LineNumber <= 0)
                    {
                        child.LineNumber = lineSeq[lineIdx++];
                    }
                }
            }
                        var seenKeys = new HashSet<string>(StringComparer.Ordinal);
                        for (var i = 0; i < items.Count; i++)
                        {
                            var key = $"{items[i].CompanyCode}|{items[i].PurchaseOrderId}|{items[i].LineNumber}|{items[i].MaterialCode}";
                            if (!seenKeys.Add(key))
                            {
                                throw new TaktBusinessException($"采购订单明细第{i + 1}项与本次提交的其他项重复（CompanyCode、PurchaseOrderId、LineNumber、MaterialCode）");
                            }
                        }
            await _purchaseOrderItemRepository.DeleteAsync(x => x.PurchaseOrderId == entity.Id);
            foreach (var child in items)
            {
            var isUnique_ix_takt_logistics_materials_purchase_order_item_order_line_unique = await _uniqueValidator.IsUniqueAsync(
                _purchaseOrderItemRepository,
                x => x.CompanyCode == child.CompanyCode
                    && x.PurchaseOrderId == child.PurchaseOrderId
                    && x.LineNumber == child.LineNumber
                    && x.MaterialCode == child.MaterialCode);
            if (!isUnique_ix_takt_logistics_materials_purchase_order_item_order_line_unique)
            {
                throw new TaktBusinessException("采购订单明细的CompanyCode、PurchaseOrderId、LineNumber、MaterialCode已存在");
            }
            }
            await _purchaseOrderItemRepository.CreateRangeAsync(items);
        }
        // 采购订单变更记录（ChangeLogs）
        if (dto.ChangeLogs is not { Count: > 0 })
        {
            await _purchaseOrderChangeLogRepository.DeleteAsync(x => x.PurchaseOrderId == entity.Id);
        }
        else
        {
            var changelogs = dto.ChangeLogs.Adapt<List<TaktPurchaseOrderChangeLog>>();
            foreach (var child in changelogs)
            {
                child.PurchaseOrderId = entity.Id;
            }
            await _purchaseOrderChangeLogRepository.DeleteAsync(x => x.PurchaseOrderId == entity.Id);
            foreach (var child in changelogs)
            {
            }
            await _purchaseOrderChangeLogRepository.CreateRangeAsync(changelogs);
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建采购订单查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktPurchaseOrder, bool>> QueryExpression(TaktPurchaseOrderQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktPurchaseOrder>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.PurchaseOrderCode != null && x.PurchaseOrderCode.Contains(keywords))
                || (x.SupplierCode != null && x.SupplierCode.Contains(keywords))
                || (x.SupplierName != null && x.SupplierName.Contains(keywords))
                || (x.PurchaseGroup != null && x.PurchaseGroup.Contains(keywords))
                || SqlFunc.ToString(x.TotalQuantity).Contains(keywords)
                || SqlFunc.ToString(x.TotalAmount).Contains(keywords)
                || SqlFunc.ToString(x.DiscountAmount).Contains(keywords)
                || SqlFunc.ToString(x.TaxAmount).Contains(keywords)
                || SqlFunc.ToString(x.ActualAmount).Contains(keywords)
                || SqlFunc.ToString(x.ReceivedQuantity).Contains(keywords)
                || SqlFunc.ToString(x.ReceivedAmount).Contains(keywords)
                || SqlFunc.ToString(x.PaidAmount).Contains(keywords)
                || SqlFunc.ToString(x.OrderStatus).Contains(keywords)
                || SqlFunc.ToString(x.DeliveryStatus).Contains(keywords)
                || SqlFunc.ToString(x.PaymentMethod).Contains(keywords)
                || SqlFunc.ToString(x.DeliveryMethod).Contains(keywords)
                || (x.DeliveryAddress != null && x.DeliveryAddress.Contains(keywords))
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.OrderDate).Contains(keywords)
                || SqlFunc.ToString(x.RequiredArrivalDate).Contains(keywords)
                || SqlFunc.ToString(x.ActualArrivalDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.PurchaseOrderCode))
        {
            exp = exp.And(x => x.PurchaseOrderCode != null && x.PurchaseOrderCode.Contains(queryDto.PurchaseOrderCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.SupplierCode))
        {
            exp = exp.And(x => x.SupplierCode != null && x.SupplierCode.Contains(queryDto.SupplierCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.SupplierName))
        {
            exp = exp.And(x => x.SupplierName != null && x.SupplierName.Contains(queryDto.SupplierName));
        }

        if (!string.IsNullOrEmpty(queryDto?.PurchaseGroup))
        {
            exp = exp.And(x => x.PurchaseGroup != null && x.PurchaseGroup.Contains(queryDto.PurchaseGroup));
        }

        if (queryDto?.TotalQuantity.HasValue == true)
        {
            exp = exp.And(x => x.TotalQuantity == queryDto.TotalQuantity);
        }

        if (queryDto?.TotalAmount.HasValue == true)
        {
            exp = exp.And(x => x.TotalAmount == queryDto.TotalAmount);
        }

        if (queryDto?.DiscountAmount.HasValue == true)
        {
            exp = exp.And(x => x.DiscountAmount == queryDto.DiscountAmount);
        }

        if (queryDto?.TaxAmount.HasValue == true)
        {
            exp = exp.And(x => x.TaxAmount == queryDto.TaxAmount);
        }

        if (queryDto?.ActualAmount.HasValue == true)
        {
            exp = exp.And(x => x.ActualAmount == queryDto.ActualAmount);
        }

        if (queryDto?.ReceivedQuantity.HasValue == true)
        {
            exp = exp.And(x => x.ReceivedQuantity == queryDto.ReceivedQuantity);
        }

        if (queryDto?.ReceivedAmount.HasValue == true)
        {
            exp = exp.And(x => x.ReceivedAmount == queryDto.ReceivedAmount);
        }

        if (queryDto?.PaidAmount.HasValue == true)
        {
            exp = exp.And(x => x.PaidAmount == queryDto.PaidAmount);
        }

        if (queryDto?.OrderStatus.HasValue == true)
        {
            exp = exp.And(x => x.OrderStatus == queryDto.OrderStatus);
        }

        if (queryDto?.DeliveryStatus.HasValue == true)
        {
            exp = exp.And(x => x.DeliveryStatus == queryDto.DeliveryStatus);
        }

        if (queryDto?.PaymentMethod.HasValue == true)
        {
            exp = exp.And(x => x.PaymentMethod == queryDto.PaymentMethod);
        }

        if (queryDto?.DeliveryMethod.HasValue == true)
        {
            exp = exp.And(x => x.DeliveryMethod == queryDto.DeliveryMethod);
        }

        if (!string.IsNullOrEmpty(queryDto?.DeliveryAddress))
        {
            exp = exp.And(x => x.DeliveryAddress != null && x.DeliveryAddress.Contains(queryDto.DeliveryAddress));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.OrderDateStart.HasValue == true)
        {
            exp = exp.And(x => x.OrderDate >= queryDto.OrderDateStart);
        }

        if (queryDto?.OrderDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.OrderDate <= queryDto.OrderDateEnd);
        }

        if (queryDto?.RequiredArrivalDateStart.HasValue == true)
        {
            exp = exp.And(x => x.RequiredArrivalDate >= queryDto.RequiredArrivalDateStart);
        }

        if (queryDto?.RequiredArrivalDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.RequiredArrivalDate <= queryDto.RequiredArrivalDateEnd);
        }

        if (queryDto?.ActualArrivalDateStart.HasValue == true)
        {
            exp = exp.And(x => x.ActualArrivalDate >= queryDto.ActualArrivalDateStart);
        }

        if (queryDto?.ActualArrivalDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.ActualArrivalDate <= queryDto.ActualArrivalDateEnd);
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
