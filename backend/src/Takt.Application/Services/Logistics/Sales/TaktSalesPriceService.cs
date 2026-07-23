// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Sales
// 文件名称：TaktSalesPriceService.cs
// 创建时间：2026-07-23
// 创建人：Takt365(Cursor AI)
// 功能描述：销售价格应用服务实现
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
/// 销售价格应用服务
/// </summary>
public class TaktSalesPriceService : TaktServiceBase, ITaktSalesPriceService
{
    private readonly ITaktCompanyRepository<TaktSalesPrice> _salesPriceRepository;
    private readonly ITaktCompanyRepository<TaktSalesPriceItem> _salesPriceItemRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="salesPriceRepository">销售价格仓储</param>
    /// <param name="salesPriceItemRepository">SalesPriceItem仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSalesPriceService(
        ITaktCompanyRepository<TaktSalesPrice> salesPriceRepository,
        ITaktCompanyRepository<TaktSalesPriceItem> salesPriceItemRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _salesPriceRepository = salesPriceRepository;
        _salesPriceItemRepository = salesPriceItemRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取销售价格列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSalesPriceDto>> GetSalesPriceListAsync(TaktSalesPriceQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _salesPriceRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSalesPriceDto>.Create(
            data.Adapt<List<TaktSalesPriceDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取销售价格
    /// </summary>
    /// <param name="id">销售价格ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesPriceDto?> GetSalesPriceByIdAsync(long id)
    {
        var entity = await _salesPriceRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktSalesPriceDto>();
        await FillSalesPriceDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取销售价格选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSalesPriceOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _salesPriceRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.SalesPriceCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.SalesPriceCode,
            DictLabel = e.SalesPriceCode,
        }).ToList();
    }

    /// <summary>
    /// 创建销售价格
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesPriceDto> CreateSalesPriceAsync(TaktSalesPriceCreateDto dto)
    {
        var entity = dto.Adapt<TaktSalesPrice>();
        var isUnique_ix_takt_logistics_sales_price_code_unique = await _uniqueValidator.IsUniqueAsync(
            _salesPriceRepository,
            x => x.PlantCode == entity.PlantCode
                && x.SalesPriceCode == entity.SalesPriceCode);
        if (!isUnique_ix_takt_logistics_sales_price_code_unique)
        {
            throw new TaktBusinessException("销售价格的PlantCode、SalesPriceCode已存在");
        }
        entity = await _salesPriceRepository.CreateAsync(entity);
                await SaveSalesPriceChildrenAsync(entity, dto);
        return await GetSalesPriceByIdAsync(entity.Id) ?? entity.Adapt<TaktSalesPriceDto>();
    }

    /// <summary>
    /// 更新销售价格
    /// </summary>
    /// <param name="id">销售价格ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesPriceDto> UpdateSalesPriceAsync(long id, TaktSalesPriceUpdateDto dto)
    {
        var entity = await _salesPriceRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("销售价格不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_sales_price_code_unique = await _uniqueValidator.IsUniqueAsync(
            _salesPriceRepository,
            x => x.PlantCode == entity.PlantCode
                && x.SalesPriceCode == entity.SalesPriceCode,
            id);
        if (!isUnique_ix_takt_logistics_sales_price_code_unique)
        {
            throw new TaktBusinessException("销售价格的PlantCode、SalesPriceCode已存在");
        }
        await _salesPriceRepository.UpdateAsync(entity);
                await SaveSalesPriceChildrenAsync(entity, dto);
        return await GetSalesPriceByIdAsync(id) ?? throw new TaktBusinessException("销售价格不存在");
    }

    /// <summary>
    /// 删除销售价格
    /// </summary>
    /// <param name="id">销售价格ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSalesPriceByIdAsync(long id)
    {
        var entity = await _salesPriceRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("销售价格不存在或已删除");
        }
        await _salesPriceItemRepository.DeleteAsync(x => x.SalesPriceId == entity.Id);
        var deleted = await _salesPriceRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("销售价格不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除销售价格
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSalesPriceBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSalesPriceByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSalesPriceTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSalesPriceTemplateDto>(
            sheetName ?? "销售价格导入模板",
            fileName ?? "销售价格导入模板.xlsx");
    }

    /// <summary>
    /// 导入销售价格
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSalesPriceAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSalesPriceImportDto>(fileStream, sheetName ?? "销售价格导入模板");
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
                var entity = rows[i].Adapt<TaktSalesPrice>();
                var importKey = $"{entity.PlantCode}|{entity.SalesPriceCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、SalesPriceCode）");
                }
                var isUnique_ix_takt_logistics_sales_price_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _salesPriceRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.SalesPriceCode == entity.SalesPriceCode);
                if (!isUnique_ix_takt_logistics_sales_price_code_unique)
                {
                    throw new TaktBusinessException("销售价格的PlantCode、SalesPriceCode已存在");
                }
                await _salesPriceRepository.CreateAsync(entity);
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
    /// 导出销售价格
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSalesPriceAsync(TaktSalesPriceQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktSalesPriceQueryDto());
        var list = await _salesPriceRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSalesPriceExportDto>(),
                sheetName ?? "销售价格数据",
                fileName ?? "销售价格导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSalesPriceExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "销售价格数据",
            fileName ?? "销售价格导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 将指定主表下全部未作废销售价格明细标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="salesPriceId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkSalesPriceItemsObsoleteAsync(long salesPriceId)
    {
        if (salesPriceId <= 0)
        {
            return;
        }
        var rows = await _salesPriceItemRepository.GetListAsync(
            x => x.SalesPriceId == salesPriceId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _salesPriceItemRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 填充销售价格详情（加载 OneToMany 子表：销售价格明细）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillSalesPriceDetailsAsync(TaktSalesPriceDto dto, TaktSalesPrice entity)
    {
        if (dto == null)
        {
            return;
        }
        // 销售价格明细 → dto.Items（含作废行）
        var items = await _salesPriceItemRepository.GetListAsync(x => x.SalesPriceId == entity.Id);
        dto.Items = items.Adapt<List<TaktSalesPriceItemDto>>();
    }

    /// <summary>
    /// 保存销售价格子表级联（销售价格明细；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveSalesPriceChildrenAsync(TaktSalesPrice entity, TaktSalesPriceCreateDto dto)
    {
        // 销售价格明细（Items）
        List<TaktSalesPriceItemUpdateDto>? itemsForSave;
        if (dto is TaktSalesPriceUpdateDto updateDtoForItems && updateDtoForItems.Items != null)
        {
            itemsForSave = updateDtoForItems.Items;
        }
        else if (dto.Items != null)
        {
            itemsForSave = dto.Items.Adapt<List<TaktSalesPriceItemUpdateDto>>();
        }
        else
        {
            itemsForSave = null;
        }
        if (itemsForSave is not { Count: > 0 })
        {
            await MarkSalesPriceItemsObsoleteAsync(entity.Id);
            return;
        }
        else
        {
            var existingList = await _salesPriceItemRepository.GetListAsync(x => x.SalesPriceId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktSalesPriceItem>();
            for (var i = 0; i < itemsForSave.Count; i++)
            {
                var childDto = itemsForSave[i];
                childDto.SalesPriceId = entity.Id;
                if (childDto.SalesPriceItemId > 0)
                {
                    if (!existingById.TryGetValue(childDto.SalesPriceItemId, out var target))
                    {
                        throw new TaktBusinessException("销售价格明细不存在（SalesPriceItemId={childDto.SalesPriceItemId}）");
                    }
                    if (target.SalesPriceId != entity.Id)
                    {
                        throw new TaktBusinessException("销售价格明细不属于当前主表（SalesPriceItemId={childDto.SalesPriceItemId}）");
                    }
                    submittedIds.Add(childDto.SalesPriceItemId);
                    childDto.Adapt(target);
                    target.Id = childDto.SalesPriceItemId;
                    target.SalesPriceId = entity.Id;
                    target.IsObsolete = 0;
                    await _salesPriceItemRepository.UpdateAsync(target);
                }
                else
                {
                    var child = childDto.Adapt<TaktSalesPriceItem>();
                    child.Id = 0;
                    child.SalesPriceId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _salesPriceItemRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                await _salesPriceItemRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建销售价格查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSalesPrice, bool>> QueryExpression(TaktSalesPriceQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSalesPrice>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.SalesPriceCode != null && x.SalesPriceCode.Contains(keywords))
                || (x.PriceType != null && x.PriceType.Contains(keywords))
                || (x.CustomerCode != null && x.CustomerCode.Contains(keywords))
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || (x.SalesGroup != null && x.SalesGroup.Contains(keywords))
                || (x.TaxCode != null && x.TaxCode.Contains(keywords))
                || SqlFunc.ToString(x.GrBasedInvoiceInspection).Contains(keywords)
                || SqlFunc.ToString(x.PricingDateControl).Contains(keywords)
                || SqlFunc.ToString(x.SalesQuotationId).Contains(keywords)
                || (x.SalesQuotationCode != null && x.SalesQuotationCode.Contains(keywords))
                || (x.VariableKey != null && x.VariableKey.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.ValidFrom).Contains(keywords)
                || SqlFunc.ToString(x.ValidTo).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.SalesPriceCode))
        {
            exp = exp.And(x => x.SalesPriceCode != null && x.SalesPriceCode.Contains(queryDto.SalesPriceCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.PriceType))
        {
            exp = exp.And(x => x.PriceType != null && x.PriceType.Contains(queryDto.PriceType));
        }

        if (!string.IsNullOrEmpty(queryDto?.CustomerCode))
        {
            exp = exp.And(x => x.CustomerCode != null && x.CustomerCode.Contains(queryDto.CustomerCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialCode))
        {
            exp = exp.And(x => x.MaterialCode != null && x.MaterialCode.Contains(queryDto.MaterialCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.SalesGroup))
        {
            exp = exp.And(x => x.SalesGroup != null && x.SalesGroup.Contains(queryDto.SalesGroup));
        }

        if (!string.IsNullOrEmpty(queryDto?.TaxCode))
        {
            exp = exp.And(x => x.TaxCode != null && x.TaxCode.Contains(queryDto.TaxCode));
        }

        if (queryDto?.GrBasedInvoiceInspection.HasValue == true)
        {
            exp = exp.And(x => x.GrBasedInvoiceInspection == queryDto.GrBasedInvoiceInspection);
        }

        if (queryDto?.PricingDateControl.HasValue == true)
        {
            exp = exp.And(x => x.PricingDateControl == queryDto.PricingDateControl);
        }

        if (queryDto?.SalesQuotationId.HasValue == true)
        {
            exp = exp.And(x => x.SalesQuotationId == queryDto.SalesQuotationId);
        }

        if (!string.IsNullOrEmpty(queryDto?.SalesQuotationCode))
        {
            exp = exp.And(x => x.SalesQuotationCode != null && x.SalesQuotationCode.Contains(queryDto.SalesQuotationCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.VariableKey))
        {
            exp = exp.And(x => x.VariableKey != null && x.VariableKey.Contains(queryDto.VariableKey));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.ValidFromStart.HasValue == true)
        {
            exp = exp.And(x => x.ValidFrom >= queryDto.ValidFromStart);
        }

        if (queryDto?.ValidFromEnd.HasValue == true)
        {
            exp = exp.And(x => x.ValidFrom <= queryDto.ValidFromEnd);
        }

        if (queryDto?.ValidToStart.HasValue == true)
        {
            exp = exp.And(x => x.ValidTo >= queryDto.ValidToStart);
        }

        if (queryDto?.ValidToEnd.HasValue == true)
        {
            exp = exp.And(x => x.ValidTo <= queryDto.ValidToEnd);
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
