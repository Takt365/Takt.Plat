// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Procurement
// 文件名称：TaktPurchaseInvoiceItemService.cs
// 创建时间：2026-06-30
// 创建人：Takt365(Cursor AI)
// 功能描述：采购发票明细应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Procurement;
using Takt.Domain.Entities.Logistics.Procurement;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Procurement;

/// <summary>
/// 采购发票明细应用服务
/// </summary>
public class TaktPurchaseInvoiceItemService : TaktServiceBase, ITaktPurchaseInvoiceItemService
{
    private readonly ITaktCompanyRepository<TaktPurchaseInvoiceItem> _purchaseInvoiceItemRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="purchaseInvoiceItemRepository">采购发票明细仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPurchaseInvoiceItemService(
        ITaktCompanyRepository<TaktPurchaseInvoiceItem> purchaseInvoiceItemRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _purchaseInvoiceItemRepository = purchaseInvoiceItemRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取采购发票明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktPurchaseInvoiceItemDto>> GetPurchaseInvoiceItemListAsync(TaktPurchaseInvoiceItemQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _purchaseInvoiceItemRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktPurchaseInvoiceItemDto>.Create(
            data.Adapt<List<TaktPurchaseInvoiceItemDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取采购发票明细
    /// </summary>
    /// <param name="id">采购发票明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseInvoiceItemDto?> GetPurchaseInvoiceItemByIdAsync(long id)
    {
        var entity = await _purchaseInvoiceItemRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktPurchaseInvoiceItemDto>();
    }

    /// <summary>
    /// 获取采购发票明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetPurchaseInvoiceItemOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _purchaseInvoiceItemRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.MaterialName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.MaterialName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建采购发票明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseInvoiceItemDto> CreatePurchaseInvoiceItemAsync(TaktPurchaseInvoiceItemCreateDto dto)
    {
        var entity = dto.Adapt<TaktPurchaseInvoiceItem>();
        var isUnique_ix_takt_logistics_procurement_purchase_invoice_item_invoice_line_unique = await _uniqueValidator.IsUniqueAsync(
            _purchaseInvoiceItemRepository,
            x => x.PurchaseInvoiceId == entity.PurchaseInvoiceId
                && x.LineNumber == entity.LineNumber);
        if (!isUnique_ix_takt_logistics_procurement_purchase_invoice_item_invoice_line_unique)
        {
            throw new TaktBusinessException("采购发票明细的PurchaseInvoiceId、LineNumber已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _purchaseInvoiceItemRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.PurchaseInvoiceId == entity.PurchaseInvoiceId,
                x => x.LineNumber);
            var businessCode = !string.IsNullOrWhiteSpace(entity.PurchaseInvoiceCode) ? entity.PurchaseInvoiceCode : entity.PurchaseInvoiceId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _purchaseInvoiceItemRepository.CreateAsync(entity);
        return await GetPurchaseInvoiceItemByIdAsync(entity.Id) ?? entity.Adapt<TaktPurchaseInvoiceItemDto>();
    }

    /// <summary>
    /// 更新采购发票明细
    /// </summary>
    /// <param name="id">采购发票明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseInvoiceItemDto> UpdatePurchaseInvoiceItemAsync(long id, TaktPurchaseInvoiceItemUpdateDto dto)
    {
        var entity = await _purchaseInvoiceItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("采购发票明细不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_procurement_purchase_invoice_item_invoice_line_unique = await _uniqueValidator.IsUniqueAsync(
            _purchaseInvoiceItemRepository,
            x => x.PurchaseInvoiceId == entity.PurchaseInvoiceId
                && x.LineNumber == entity.LineNumber,
            id);
        if (!isUnique_ix_takt_logistics_procurement_purchase_invoice_item_invoice_line_unique)
        {
            throw new TaktBusinessException("采购发票明细的PurchaseInvoiceId、LineNumber已存在");
        }
        await _purchaseInvoiceItemRepository.UpdateAsync(entity);
        return await GetPurchaseInvoiceItemByIdAsync(id) ?? throw new TaktBusinessException("采购发票明细不存在");
    }

    /// <summary>
    /// 删除采购发票明细
    /// </summary>
    /// <param name="id">采购发票明细ID</param>
    /// <returns>任务</returns>
    public async Task DeletePurchaseInvoiceItemByIdAsync(long id)
    {
        var deleted = await _purchaseInvoiceItemRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("采购发票明细不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除采购发票明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeletePurchaseInvoiceItemBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeletePurchaseInvoiceItemByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetPurchaseInvoiceItemTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktPurchaseInvoiceItemTemplateDto>(
            sheetName ?? "采购发票明细导入模板",
            fileName ?? "采购发票明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入采购发票明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportPurchaseInvoiceItemAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktPurchaseInvoiceItemImportDto>(fileStream, sheetName ?? "采购发票明细导入模板");
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
                var entity = rows[i].Adapt<TaktPurchaseInvoiceItem>();
                var importKey = $"{entity.PurchaseInvoiceId}|{entity.LineNumber}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PurchaseInvoiceId、LineNumber）");
                }
                var isUnique_ix_takt_logistics_procurement_purchase_invoice_item_invoice_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _purchaseInvoiceItemRepository,
                    x => x.PurchaseInvoiceId == entity.PurchaseInvoiceId
                        && x.LineNumber == entity.LineNumber);
                if (!isUnique_ix_takt_logistics_procurement_purchase_invoice_item_invoice_line_unique)
                {
                    throw new TaktBusinessException("采购发票明细的PurchaseInvoiceId、LineNumber已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _purchaseInvoiceItemRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.PurchaseInvoiceId == entity.PurchaseInvoiceId,
                        x => x.LineNumber);
                    var businessCode = !string.IsNullOrWhiteSpace(entity.PurchaseInvoiceCode) ? entity.PurchaseInvoiceCode : entity.PurchaseInvoiceId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _purchaseInvoiceItemRepository.CreateAsync(entity);
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
    /// 导出采购发票明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportPurchaseInvoiceItemAsync(TaktPurchaseInvoiceItemQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktPurchaseInvoiceItemQueryDto());
        var list = await _purchaseInvoiceItemRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPurchaseInvoiceItemExportDto>(),
                sheetName ?? "采购发票明细数据",
                fileName ?? "采购发票明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktPurchaseInvoiceItemExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "采购发票明细数据",
            fileName ?? "采购发票明细导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建采购发票明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktPurchaseInvoiceItem, bool>> QueryExpression(TaktPurchaseInvoiceItemQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktPurchaseInvoiceItem>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.PurchaseInvoiceId).Contains(keywords)
                || (x.PurchaseInvoiceCode != null && x.PurchaseInvoiceCode.Contains(keywords))
                || SqlFunc.ToString(x.LineNumber).Contains(keywords)
                || (x.PurchaseOrderCode != null && x.PurchaseOrderCode.Contains(keywords))
                || SqlFunc.ToString(x.PurchaseOrderLineNumber).Contains(keywords)
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || (x.MaterialName != null && x.MaterialName.Contains(keywords))
                || (x.MaterialSpecification != null && x.MaterialSpecification.Contains(keywords))
                || (x.PurchaseUnit != null && x.PurchaseUnit.Contains(keywords))
                || SqlFunc.ToString(x.InvoiceQuantity).Contains(keywords)
                || SqlFunc.ToString(x.UnitPrice).Contains(keywords)
                || SqlFunc.ToString(x.DiscountRate).Contains(keywords)
                || SqlFunc.ToString(x.DiscountAmount).Contains(keywords)
                || SqlFunc.ToString(x.TaxRate).Contains(keywords)
                || SqlFunc.ToString(x.TaxAmount).Contains(keywords)
                || SqlFunc.ToString(x.SubtotalAmount).Contains(keywords)
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.PurchaseInvoiceId.HasValue == true)
        {
            exp = exp.And(x => x.PurchaseInvoiceId == queryDto.PurchaseInvoiceId);
        }

        if (!string.IsNullOrEmpty(queryDto?.PurchaseInvoiceCode))
        {
            exp = exp.And(x => x.PurchaseInvoiceCode != null && x.PurchaseInvoiceCode.Contains(queryDto.PurchaseInvoiceCode));
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            exp = exp.And(x => x.LineNumber == queryDto.LineNumber);
        }

        if (!string.IsNullOrEmpty(queryDto?.PurchaseOrderCode))
        {
            exp = exp.And(x => x.PurchaseOrderCode != null && x.PurchaseOrderCode.Contains(queryDto.PurchaseOrderCode));
        }

        if (queryDto?.PurchaseOrderLineNumber.HasValue == true)
        {
            exp = exp.And(x => x.PurchaseOrderLineNumber == queryDto.PurchaseOrderLineNumber);
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialCode))
        {
            exp = exp.And(x => x.MaterialCode != null && x.MaterialCode.Contains(queryDto.MaterialCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialName))
        {
            exp = exp.And(x => x.MaterialName != null && x.MaterialName.Contains(queryDto.MaterialName));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialSpecification))
        {
            exp = exp.And(x => x.MaterialSpecification != null && x.MaterialSpecification.Contains(queryDto.MaterialSpecification));
        }

        if (!string.IsNullOrEmpty(queryDto?.PurchaseUnit))
        {
            exp = exp.And(x => x.PurchaseUnit != null && x.PurchaseUnit.Contains(queryDto.PurchaseUnit));
        }

        if (queryDto?.InvoiceQuantity.HasValue == true)
        {
            exp = exp.And(x => x.InvoiceQuantity == queryDto.InvoiceQuantity);
        }

        if (queryDto?.UnitPrice.HasValue == true)
        {
            exp = exp.And(x => x.UnitPrice == queryDto.UnitPrice);
        }

        if (queryDto?.DiscountRate.HasValue == true)
        {
            exp = exp.And(x => x.DiscountRate == queryDto.DiscountRate);
        }

        if (queryDto?.DiscountAmount.HasValue == true)
        {
            exp = exp.And(x => x.DiscountAmount == queryDto.DiscountAmount);
        }

        if (queryDto?.TaxRate.HasValue == true)
        {
            exp = exp.And(x => x.TaxRate == queryDto.TaxRate);
        }

        if (queryDto?.TaxAmount.HasValue == true)
        {
            exp = exp.And(x => x.TaxAmount == queryDto.TaxAmount);
        }

        if (queryDto?.SubtotalAmount.HasValue == true)
        {
            exp = exp.And(x => x.SubtotalAmount == queryDto.SubtotalAmount);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
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
