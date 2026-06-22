// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Sales
// 文件名称：TaktSalesInvoiceItemService.cs
// 创建时间：2026-06-20
// 创建人：Takt365(Cursor AI)
// 功能描述：销售发票明细应用服务实现
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
/// 销售发票明细应用服务
/// </summary>
public class TaktSalesInvoiceItemService : TaktServiceBase, ITaktSalesInvoiceItemService
{
    private readonly ITaktCompanyRepository<TaktSalesInvoiceItem> _salesInvoiceItemRepository;
    private readonly ITaktCompanyRepository<TaktSalesInvoice> _salesInvoiceRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="salesInvoiceItemRepository">销售发票明细仓储</param>
    /// <param name="salesInvoiceRepository">销售发票仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSalesInvoiceItemService(
        ITaktCompanyRepository<TaktSalesInvoiceItem> salesInvoiceItemRepository,
        ITaktCompanyRepository<TaktSalesInvoice> salesInvoiceRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _salesInvoiceItemRepository = salesInvoiceItemRepository;
        _salesInvoiceRepository = salesInvoiceRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取销售发票明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSalesInvoiceItemDto>> GetSalesInvoiceItemListAsync(TaktSalesInvoiceItemQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _salesInvoiceItemRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSalesInvoiceItemDto>.Create(
            data.Adapt<List<TaktSalesInvoiceItemDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取销售发票明细
    /// </summary>
    /// <param name="id">销售发票明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesInvoiceItemDto?> GetSalesInvoiceItemByIdAsync(long id)
    {
        var entity = await _salesInvoiceItemRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktSalesInvoiceItemDto>();
    }

    /// <summary>
    /// 获取销售发票明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSalesInvoiceItemOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _salesInvoiceItemRepository.GetListAsync(
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
    /// 创建销售发票明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesInvoiceItemDto> CreateSalesInvoiceItemAsync(TaktSalesInvoiceItemCreateDto dto)
    {
        var entity = dto.Adapt<TaktSalesInvoiceItem>();
        await StampSalesInvoiceItemSalesInvoiceAsync(entity, dto);
        var isUnique_ix_takt_logistics_sales_invoice_item_invoice_line_unique = await _uniqueValidator.IsUniqueAsync(
            _salesInvoiceItemRepository,
            x => x.SalesInvoiceId == entity.SalesInvoiceId
                && x.LineNumber == entity.LineNumber);
        if (!isUnique_ix_takt_logistics_sales_invoice_item_invoice_line_unique)
        {
            throw new TaktBusinessException("销售发票明细的SalesInvoiceId、LineNumber已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _salesInvoiceItemRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.SalesInvoiceId == entity.SalesInvoiceId,
                x => x.LineNumber);
            var businessCode = !string.IsNullOrWhiteSpace(entity.SalesInvoiceCode) ? entity.SalesInvoiceCode : entity.SalesInvoiceId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _salesInvoiceItemRepository.CreateAsync(entity);
        return await GetSalesInvoiceItemByIdAsync(entity.Id) ?? entity.Adapt<TaktSalesInvoiceItemDto>();
    }

    /// <summary>
    /// 更新销售发票明细
    /// </summary>
    /// <param name="id">销售发票明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesInvoiceItemDto> UpdateSalesInvoiceItemAsync(long id, TaktSalesInvoiceItemUpdateDto dto)
    {
        var entity = await _salesInvoiceItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("销售发票明细不存在");
        }
        dto.Adapt(entity);
        await StampSalesInvoiceItemSalesInvoiceAsync(entity, dto);
        var isUnique_ix_takt_logistics_sales_invoice_item_invoice_line_unique = await _uniqueValidator.IsUniqueAsync(
            _salesInvoiceItemRepository,
            x => x.SalesInvoiceId == entity.SalesInvoiceId
                && x.LineNumber == entity.LineNumber,
            id);
        if (!isUnique_ix_takt_logistics_sales_invoice_item_invoice_line_unique)
        {
            throw new TaktBusinessException("销售发票明细的SalesInvoiceId、LineNumber已存在");
        }
        await _salesInvoiceItemRepository.UpdateAsync(entity);
        return await GetSalesInvoiceItemByIdAsync(id) ?? throw new TaktBusinessException("销售发票明细不存在");
    }

    /// <summary>
    /// 删除销售发票明细
    /// </summary>
    /// <param name="id">销售发票明细ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSalesInvoiceItemByIdAsync(long id)
    {
        var deleted = await _salesInvoiceItemRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("销售发票明细不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除销售发票明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSalesInvoiceItemBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSalesInvoiceItemByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSalesInvoiceItemTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSalesInvoiceItemTemplateDto>(
            sheetName ?? "销售发票明细导入模板",
            fileName ?? "销售发票明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入销售发票明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSalesInvoiceItemAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSalesInvoiceItemImportDto>(fileStream, sheetName ?? "销售发票明细导入模板");
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
                var entity = rows[i].Adapt<TaktSalesInvoiceItem>();
                var importDto = rows[i].Adapt<TaktSalesInvoiceItemCreateDto>();
                await StampSalesInvoiceItemSalesInvoiceAsync(entity, importDto);
                var importKey = $"{entity.SalesInvoiceId}|{entity.LineNumber}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（SalesInvoiceId、LineNumber）");
                }
                var isUnique_ix_takt_logistics_sales_invoice_item_invoice_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _salesInvoiceItemRepository,
                    x => x.SalesInvoiceId == entity.SalesInvoiceId
                        && x.LineNumber == entity.LineNumber);
                if (!isUnique_ix_takt_logistics_sales_invoice_item_invoice_line_unique)
                {
                    throw new TaktBusinessException("销售发票明细的SalesInvoiceId、LineNumber已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _salesInvoiceItemRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.SalesInvoiceId == entity.SalesInvoiceId,
                        x => x.LineNumber);
                    var businessCode = !string.IsNullOrWhiteSpace(entity.SalesInvoiceCode) ? entity.SalesInvoiceCode : entity.SalesInvoiceId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _salesInvoiceItemRepository.CreateAsync(entity);
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
    /// 导出销售发票明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSalesInvoiceItemAsync(TaktSalesInvoiceItemQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktSalesInvoiceItemQueryDto());
        var list = await _salesInvoiceItemRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSalesInvoiceItemExportDto>(),
                sheetName ?? "销售发票明细数据",
                fileName ?? "销售发票明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSalesInvoiceItemExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "销售发票明细数据",
            fileName ?? "销售发票明细导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步销售发票明细主表外键（ManyToOne → 销售发票）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampSalesInvoiceItemSalesInvoiceAsync(TaktSalesInvoiceItem entity, TaktSalesInvoiceItemCreateDto dto)
    {
        if (dto.SalesInvoiceId <= 0)
        {
            return;
        }
        var master = await _salesInvoiceRepository.GetByIdAsync(dto.SalesInvoiceId);
        if (master == null)
        {
            throw new TaktBusinessException("销售发票不存在");
        }
        entity.SalesInvoiceId = master.Id;
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建销售发票明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSalesInvoiceItem, bool>> QueryExpression(TaktSalesInvoiceItemQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSalesInvoiceItem>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.SalesInvoiceId).Contains(keywords)
                || (x.SalesInvoiceCode != null && x.SalesInvoiceCode.Contains(keywords))
                || SqlFunc.ToString(x.LineNumber).Contains(keywords)
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || (x.MaterialName != null && x.MaterialName.Contains(keywords))
                || (x.MaterialSpecification != null && x.MaterialSpecification.Contains(keywords))
                || (x.SalesUnit != null && x.SalesUnit.Contains(keywords))
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

        if (queryDto?.SalesInvoiceId.HasValue == true)
        {
            exp = exp.And(x => x.SalesInvoiceId == queryDto.SalesInvoiceId);
        }

        if (!string.IsNullOrEmpty(queryDto?.SalesInvoiceCode))
        {
            exp = exp.And(x => x.SalesInvoiceCode != null && x.SalesInvoiceCode.Contains(queryDto.SalesInvoiceCode));
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            exp = exp.And(x => x.LineNumber == queryDto.LineNumber);
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

        if (!string.IsNullOrEmpty(queryDto?.SalesUnit))
        {
            exp = exp.And(x => x.SalesUnit != null && x.SalesUnit.Contains(queryDto.SalesUnit));
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
