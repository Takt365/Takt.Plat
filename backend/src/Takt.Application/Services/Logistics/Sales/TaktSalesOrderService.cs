// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Sales
// 文件名称：TaktSalesOrderService.cs
// 创建时间：2026-07-09
// 创建人：Takt365(Cursor AI)
// 功能描述：销售订单应用服务实现
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
/// 销售订单应用服务
/// </summary>
public class TaktSalesOrderService : TaktServiceBase, ITaktSalesOrderService
{
    private readonly ITaktCompanyRepository<TaktSalesOrder> _salesOrderRepository;
    private readonly ITaktCompanyRepository<TaktSalesOrderItem> _salesOrderItemRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="salesOrderRepository">销售订单仓储</param>
    /// <param name="salesOrderItemRepository">SalesOrderItem仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSalesOrderService(
        ITaktCompanyRepository<TaktSalesOrder> salesOrderRepository,
        ITaktCompanyRepository<TaktSalesOrderItem> salesOrderItemRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _salesOrderRepository = salesOrderRepository;
        _salesOrderItemRepository = salesOrderItemRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取销售订单列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSalesOrderDto>> GetSalesOrderListAsync(TaktSalesOrderQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _salesOrderRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSalesOrderDto>.Create(
            data.Adapt<List<TaktSalesOrderDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取销售订单
    /// </summary>
    /// <param name="id">销售订单ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesOrderDto?> GetSalesOrderByIdAsync(long id)
    {
        var entity = await _salesOrderRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktSalesOrderDto>();
        await FillSalesOrderDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取销售订单选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSalesOrderOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _salesOrderRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.OrderStatus == 1,
            x => x.SalesOrderCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.SalesOrderCode,
            DictLabel = string.IsNullOrWhiteSpace(e.CustomerName)
                ? e.SalesOrderCode
                : $"{e.SalesOrderCode} {e.CustomerName}",
        }).ToList();
    }

    /// <summary>
    /// 创建销售订单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesOrderDto> CreateSalesOrderAsync(TaktSalesOrderCreateDto dto)
    {
        var entity = dto.Adapt<TaktSalesOrder>();
        var isUnique_ix_takt_logistics_sales_order_so_unique = await _uniqueValidator.IsUniqueAsync(
            _salesOrderRepository,
            x => x.PlantCode == entity.PlantCode
                && x.SalesOrderCode == entity.SalesOrderCode);
        if (!isUnique_ix_takt_logistics_sales_order_so_unique)
        {
            throw new TaktBusinessException("销售订单的PlantCode、SalesOrderCode已存在");
        }
        entity = await _salesOrderRepository.CreateAsync(entity);
                await SaveSalesOrderChildrenAsync(entity, dto);
        return await GetSalesOrderByIdAsync(entity.Id) ?? entity.Adapt<TaktSalesOrderDto>();
    }

    /// <summary>
    /// 更新销售订单
    /// </summary>
    /// <param name="id">销售订单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesOrderDto> UpdateSalesOrderAsync(long id, TaktSalesOrderUpdateDto dto)
    {
        var entity = await _salesOrderRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("销售订单不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_sales_order_so_unique = await _uniqueValidator.IsUniqueAsync(
            _salesOrderRepository,
            x => x.PlantCode == entity.PlantCode
                && x.SalesOrderCode == entity.SalesOrderCode,
            id);
        if (!isUnique_ix_takt_logistics_sales_order_so_unique)
        {
            throw new TaktBusinessException("销售订单的PlantCode、SalesOrderCode已存在");
        }
        await _salesOrderRepository.UpdateAsync(entity);
                await SaveSalesOrderChildrenAsync(entity, dto);
        return await GetSalesOrderByIdAsync(id) ?? throw new TaktBusinessException("销售订单不存在");
    }

    /// <summary>
    /// 删除销售订单
    /// </summary>
    /// <param name="id">销售订单ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSalesOrderByIdAsync(long id)
    {
        var entity = await _salesOrderRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("销售订单不存在或已删除");
        }
        await _salesOrderItemRepository.DeleteAsync(x => x.SalesOrderId == entity.Id);
        var deleted = await _salesOrderRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("销售订单不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除销售订单
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSalesOrderBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSalesOrderByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新销售订单状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesOrderDto> UpdateSalesOrderStatusAsync(TaktSalesOrderStatusDto dto)
    {
        var entity = await _salesOrderRepository.GetByIdAsync(dto.SalesOrderId);
        if (entity == null)
        {
            throw new TaktBusinessException("销售订单不存在");
        }
        entity.OrderStatus = dto.OrderStatus;
        await _salesOrderRepository.UpdateAsync(entity);
        return await GetSalesOrderByIdAsync(dto.SalesOrderId) ?? throw new TaktBusinessException("销售订单不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSalesOrderTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSalesOrderTemplateDto>(
            sheetName ?? "销售订单导入模板",
            fileName ?? "销售订单导入模板.xlsx");
    }

    /// <summary>
    /// 导入销售订单
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSalesOrderAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSalesOrderImportDto>(fileStream, sheetName ?? "销售订单导入模板");
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
                var entity = rows[i].Adapt<TaktSalesOrder>();
                var importKey = $"{entity.PlantCode}|{entity.SalesOrderCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、SalesOrderCode）");
                }
                var isUnique_ix_takt_logistics_sales_order_so_unique = await _uniqueValidator.IsUniqueAsync(
                    _salesOrderRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.SalesOrderCode == entity.SalesOrderCode);
                if (!isUnique_ix_takt_logistics_sales_order_so_unique)
                {
                    throw new TaktBusinessException("销售订单的PlantCode、SalesOrderCode已存在");
                }
                await _salesOrderRepository.CreateAsync(entity);
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
    /// 导出销售订单
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSalesOrderAsync(TaktSalesOrderQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktSalesOrderQueryDto());
        var list = await _salesOrderRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSalesOrderExportDto>(),
                sheetName ?? "销售订单数据",
                fileName ?? "销售订单导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSalesOrderExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "销售订单数据",
            fileName ?? "销售订单导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 将指定主表下全部未作废销售订单明细标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="salesOrderId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkSalesOrderItemsObsoleteAsync(long salesOrderId)
    {
        if (salesOrderId <= 0)
        {
            return;
        }
        var rows = await _salesOrderItemRepository.GetListAsync(
            x => x.SalesOrderId == salesOrderId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _salesOrderItemRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 填充销售订单详情（加载 OneToMany 子表：销售订单明细）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillSalesOrderDetailsAsync(TaktSalesOrderDto dto, TaktSalesOrder entity)
    {
        if (dto == null)
        {
            return;
        }
        // 销售订单明细 → dto.Items（含作废行）
        var items = await _salesOrderItemRepository.GetListAsync(x => x.SalesOrderId == entity.Id);
        dto.Items = items.Adapt<List<TaktSalesOrderItemDto>>();
    }

    /// <summary>
    /// 保存销售订单子表级联（销售订单明细；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveSalesOrderChildrenAsync(TaktSalesOrder entity, TaktSalesOrderCreateDto dto)
    {
        // 销售订单明细（Items）
        if (dto.Items is not { Count: > 0 })
        {
            await MarkSalesOrderItemsObsoleteAsync(entity.Id);
            return;
        }
        else
        {
            var existingList = await _salesOrderItemRepository.GetListAsync(x => x.SalesOrderId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktSalesOrderItem>();
            var seenLineKeys = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < dto.Items.Count; i++)
            {
                var childDto = dto.Items[i];
                childDto.SalesOrderId = entity.Id;
                var lineKey = $"{entity.CompanyCode}|{entity.Id}|{childDto.LineNumber}";
                if (!seenLineKeys.Add(lineKey))
                {
                    throw new TaktBusinessException("销售订单明细第{i + 1}项与本次提交的其他项重复（CompanyCode、SalesOrderId、LineNumber）");
                }
                if (childDto.SalesOrderItemId > 0)
                {
                    if (!existingById.TryGetValue(childDto.SalesOrderItemId, out var target))
                    {
                        throw new TaktBusinessException("销售订单明细不存在（SalesOrderItemId={childDto.SalesOrderItemId}）");
                    }
                    if (target.SalesOrderId != entity.Id)
                    {
                        throw new TaktBusinessException("销售订单明细不属于当前主表（SalesOrderItemId={childDto.SalesOrderItemId}）");
                    }
                    submittedIds.Add(childDto.SalesOrderItemId);
                    var isUniqueUpdate_ix_takt_logistics_sales_order_item_order_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _salesOrderItemRepository,
                        x => x.CompanyCode == x.CompanyCode
                && x.SalesOrderId == x.SalesOrderId
                && x.LineNumber == x.LineNumber,
                        childDto.SalesOrderItemId);
                    if (!isUniqueUpdate_ix_takt_logistics_sales_order_item_order_line_unique)
                    {
                        throw new TaktBusinessException("销售订单明细的CompanyCode、SalesOrderId、LineNumber已存在");
                    }
                    childDto.Adapt(target);
                    target.Id = childDto.SalesOrderItemId;
                    target.SalesOrderId = entity.Id;
                    target.IsObsolete = 0;
                    await _salesOrderItemRepository.UpdateAsync(target);
                }
                else
                {
                    var isUniqueCreate_ix_takt_logistics_sales_order_item_order_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _salesOrderItemRepository,
                        x => x.CompanyCode == x.CompanyCode
                && x.SalesOrderId == x.SalesOrderId
                && x.LineNumber == x.LineNumber);
                    if (!isUniqueCreate_ix_takt_logistics_sales_order_item_order_line_unique)
                    {
                        throw new TaktBusinessException("销售订单明细的CompanyCode、SalesOrderId、LineNumber已存在");
                    }
                    var child = childDto.Adapt<TaktSalesOrderItem>();
                    child.Id = 0;
                    child.SalesOrderId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _salesOrderItemRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                var needLine = toCreate.Where(c => c.LineNumber <= 0).ToList();
                if (needLine.Count > 0)
                {
                    var businessCode = !string.IsNullOrWhiteSpace(entity.SalesOrderCode) ? entity.SalesOrderCode : entity.Id.ToString();
                    var maxLine = existingList.Count > 0 ? existingList.Max(x => x.LineNumber) : 0;
                    var lineSeq = _lineNumberGenerator.GenerateSequence(businessCode, needLine.Count, maxLine).ToList();
                    var lineIdx = 0;
                    foreach (var child in toCreate)
                    {
                        if (child.LineNumber <= 0)
                        {
                            child.LineNumber = lineSeq[lineIdx++];
                        }
                    }
                }
                await _salesOrderItemRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建销售订单查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSalesOrder, bool>> QueryExpression(TaktSalesOrderQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSalesOrder>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.SalesOrderCode != null && x.SalesOrderCode.Contains(keywords))
                || (x.CustomerCode != null && x.CustomerCode.Contains(keywords))
                || (x.CustomerName != null && x.CustomerName.Contains(keywords))
                || (x.SalesBy != null && x.SalesBy.Contains(keywords))
                || SqlFunc.ToString(x.TotalQuantity).Contains(keywords)
                || SqlFunc.ToString(x.TotalAmount).Contains(keywords)
                || SqlFunc.ToString(x.DiscountAmount).Contains(keywords)
                || SqlFunc.ToString(x.TaxAmount).Contains(keywords)
                || SqlFunc.ToString(x.ActualAmount).Contains(keywords)
                || SqlFunc.ToString(x.ShippedQuantity).Contains(keywords)
                || SqlFunc.ToString(x.ShippedAmount).Contains(keywords)
                || SqlFunc.ToString(x.ReceivedAmount).Contains(keywords)
                || SqlFunc.ToString(x.DeliveryMethod).Contains(keywords)
                || SqlFunc.ToString(x.PaymentMethod).Contains(keywords)
                || (x.DeliveryAddress != null && x.DeliveryAddress.Contains(keywords))
                || SqlFunc.ToString(x.OrderStatus).Contains(keywords)
                || SqlFunc.ToString(x.DeliveryStatus).Contains(keywords)
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.OrderDate).Contains(keywords)
                || SqlFunc.ToString(x.RequiredDeliveryDate).Contains(keywords)
                || SqlFunc.ToString(x.ActualDeliveryDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.SalesOrderCode))
        {
            exp = exp.And(x => x.SalesOrderCode != null && x.SalesOrderCode.Contains(queryDto.SalesOrderCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.CustomerCode))
        {
            exp = exp.And(x => x.CustomerCode != null && x.CustomerCode.Contains(queryDto.CustomerCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.CustomerName))
        {
            exp = exp.And(x => x.CustomerName != null && x.CustomerName.Contains(queryDto.CustomerName));
        }

        if (!string.IsNullOrEmpty(queryDto?.SalesBy))
        {
            exp = exp.And(x => x.SalesBy != null && x.SalesBy.Contains(queryDto.SalesBy));
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

        if (queryDto?.ShippedQuantity.HasValue == true)
        {
            exp = exp.And(x => x.ShippedQuantity == queryDto.ShippedQuantity);
        }

        if (queryDto?.ShippedAmount.HasValue == true)
        {
            exp = exp.And(x => x.ShippedAmount == queryDto.ShippedAmount);
        }

        if (queryDto?.ReceivedAmount.HasValue == true)
        {
            exp = exp.And(x => x.ReceivedAmount == queryDto.ReceivedAmount);
        }

        if (queryDto?.DeliveryMethod.HasValue == true)
        {
            exp = exp.And(x => x.DeliveryMethod == queryDto.DeliveryMethod);
        }

        if (queryDto?.PaymentMethod.HasValue == true)
        {
            exp = exp.And(x => x.PaymentMethod == queryDto.PaymentMethod);
        }

        if (!string.IsNullOrEmpty(queryDto?.DeliveryAddress))
        {
            exp = exp.And(x => x.DeliveryAddress != null && x.DeliveryAddress.Contains(queryDto.DeliveryAddress));
        }

        if (queryDto?.OrderStatus.HasValue == true)
        {
            exp = exp.And(x => x.OrderStatus == queryDto.OrderStatus);
        }

        if (queryDto?.DeliveryStatus.HasValue == true)
        {
            exp = exp.And(x => x.DeliveryStatus == queryDto.DeliveryStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
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

        if (queryDto?.RequiredDeliveryDateStart.HasValue == true)
        {
            exp = exp.And(x => x.RequiredDeliveryDate >= queryDto.RequiredDeliveryDateStart);
        }

        if (queryDto?.RequiredDeliveryDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.RequiredDeliveryDate <= queryDto.RequiredDeliveryDateEnd);
        }

        if (queryDto?.ActualDeliveryDateStart.HasValue == true)
        {
            exp = exp.And(x => x.ActualDeliveryDate >= queryDto.ActualDeliveryDateStart);
        }

        if (queryDto?.ActualDeliveryDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.ActualDeliveryDate <= queryDto.ActualDeliveryDateEnd);
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
