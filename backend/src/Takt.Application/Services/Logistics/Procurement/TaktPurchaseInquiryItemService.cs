// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Procurement
// 文件名称：TaktPurchaseInquiryItemService.cs
// 创建时间：2026-07-09
// 创建人：Takt365(Cursor AI)
// 功能描述：采购询价明细应用服务实现
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
/// 采购询价明细应用服务
/// </summary>
public class TaktPurchaseInquiryItemService : TaktServiceBase, ITaktPurchaseInquiryItemService
{
    private readonly ITaktCompanyRepository<TaktPurchaseInquiryItem> _purchaseInquiryItemRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="purchaseInquiryItemRepository">采购询价明细仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPurchaseInquiryItemService(
        ITaktCompanyRepository<TaktPurchaseInquiryItem> purchaseInquiryItemRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _purchaseInquiryItemRepository = purchaseInquiryItemRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取采购询价明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktPurchaseInquiryItemDto>> GetPurchaseInquiryItemListAsync(TaktPurchaseInquiryItemQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _purchaseInquiryItemRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktPurchaseInquiryItemDto>.Create(
            data.Adapt<List<TaktPurchaseInquiryItemDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取采购询价明细
    /// </summary>
    /// <param name="id">采购询价明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseInquiryItemDto?> GetPurchaseInquiryItemByIdAsync(long id)
    {
        var entity = await _purchaseInquiryItemRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktPurchaseInquiryItemDto>();
    }

    /// <summary>
    /// 获取采购询价明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetPurchaseInquiryItemOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _purchaseInquiryItemRepository.GetListAsync(
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
    /// 创建采购询价明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseInquiryItemDto> CreatePurchaseInquiryItemAsync(TaktPurchaseInquiryItemCreateDto dto)
    {
        var entity = dto.Adapt<TaktPurchaseInquiryItem>();
        entity.IsObsolete = 0;
        var isUnique_ix_takt_logistics_materials_purchase_inquiry_item_line_unique = await _uniqueValidator.IsUniqueAsync(
            _purchaseInquiryItemRepository,
            x => x.PurchaseInquiryId == entity.PurchaseInquiryId
                && x.LineNumber == entity.LineNumber
                && x.MaterialCode == entity.MaterialCode);
        if (!isUnique_ix_takt_logistics_materials_purchase_inquiry_item_line_unique)
        {
            throw new TaktBusinessException("采购询价明细的PurchaseInquiryId、LineNumber、MaterialCode已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _purchaseInquiryItemRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.PurchaseInquiryId == entity.PurchaseInquiryId,
                x => x.LineNumber);
            var businessCode = !string.IsNullOrWhiteSpace(entity.PurchaseInquiryCode) ? entity.PurchaseInquiryCode : entity.PurchaseInquiryId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _purchaseInquiryItemRepository.CreateAsync(entity);
        return await GetPurchaseInquiryItemByIdAsync(entity.Id) ?? entity.Adapt<TaktPurchaseInquiryItemDto>();
    }

    /// <summary>
    /// 更新采购询价明细
    /// </summary>
    /// <param name="id">采购询价明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseInquiryItemDto> UpdatePurchaseInquiryItemAsync(long id, TaktPurchaseInquiryItemUpdateDto dto)
    {
        var entity = await _purchaseInquiryItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("采购询价明细不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_materials_purchase_inquiry_item_line_unique = await _uniqueValidator.IsUniqueAsync(
            _purchaseInquiryItemRepository,
            x => x.PurchaseInquiryId == entity.PurchaseInquiryId
                && x.LineNumber == entity.LineNumber
                && x.MaterialCode == entity.MaterialCode,
            id);
        if (!isUnique_ix_takt_logistics_materials_purchase_inquiry_item_line_unique)
        {
            throw new TaktBusinessException("采购询价明细的PurchaseInquiryId、LineNumber、MaterialCode已存在");
        }
        await _purchaseInquiryItemRepository.UpdateAsync(entity);
        return await GetPurchaseInquiryItemByIdAsync(id) ?? throw new TaktBusinessException("采购询价明细不存在");
    }

    /// <summary>
    /// 删除采购询价明细
    /// </summary>
    /// <param name="id">采购询价明细ID</param>
    /// <returns>任务</returns>
    public async Task DeletePurchaseInquiryItemByIdAsync(long id)
    {
        var entity = await _purchaseInquiryItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("采购询价明细不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("采购询价明细不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("采购询价明细已作废");
        }
        entity.IsObsolete = 1;
        await _purchaseInquiryItemRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 批量删除采购询价明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeletePurchaseInquiryItemBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeletePurchaseInquiryItemByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新采购询价明细作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseInquiryItemDto> UpdatePurchaseInquiryItemObsoleteAsync(TaktPurchaseInquiryItemObsoleteDto dto)
    {
        var entity = await _purchaseInquiryItemRepository.GetByIdAsync(dto.PurchaseInquiryItemId);
        if (entity == null)
        {
            throw new TaktBusinessException("采购询价明细不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("采购询价明细不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _purchaseInquiryItemRepository.UpdateAsync(entity);
        return await GetPurchaseInquiryItemByIdAsync(dto.PurchaseInquiryItemId) ?? throw new TaktBusinessException("采购询价明细不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetPurchaseInquiryItemTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktPurchaseInquiryItemTemplateDto>(
            sheetName ?? "采购询价明细导入模板",
            fileName ?? "采购询价明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入采购询价明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportPurchaseInquiryItemAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktPurchaseInquiryItemImportDto>(fileStream, sheetName ?? "采购询价明细导入模板");
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
                var entity = rows[i].Adapt<TaktPurchaseInquiryItem>();
                var importKey = $"{entity.PurchaseInquiryId}|{entity.LineNumber}|{entity.MaterialCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PurchaseInquiryId、LineNumber、MaterialCode）");
                }
                var isUnique_ix_takt_logistics_materials_purchase_inquiry_item_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _purchaseInquiryItemRepository,
                    x => x.PurchaseInquiryId == entity.PurchaseInquiryId
                        && x.LineNumber == entity.LineNumber
                        && x.MaterialCode == entity.MaterialCode);
                if (!isUnique_ix_takt_logistics_materials_purchase_inquiry_item_line_unique)
                {
                    throw new TaktBusinessException("采购询价明细的PurchaseInquiryId、LineNumber、MaterialCode已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _purchaseInquiryItemRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.PurchaseInquiryId == entity.PurchaseInquiryId,
                        x => x.LineNumber);
                    var businessCode = !string.IsNullOrWhiteSpace(entity.PurchaseInquiryCode) ? entity.PurchaseInquiryCode : entity.PurchaseInquiryId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _purchaseInquiryItemRepository.CreateAsync(entity);
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
    /// 导出采购询价明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportPurchaseInquiryItemAsync(TaktPurchaseInquiryItemQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktPurchaseInquiryItemQueryDto());
        var list = await _purchaseInquiryItemRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPurchaseInquiryItemExportDto>(),
                sheetName ?? "采购询价明细数据",
                fileName ?? "采购询价明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktPurchaseInquiryItemExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "采购询价明细数据",
            fileName ?? "采购询价明细导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建采购询价明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktPurchaseInquiryItem, bool>> QueryExpression(TaktPurchaseInquiryItemQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktPurchaseInquiryItem>();

        if (queryDto?.IsObsolete.HasValue == true)
        {
            exp = exp.And(x => x.IsObsolete == queryDto.IsObsolete);
        }
        else
        {
            exp = exp.And(x => x.IsObsolete == 0);
        }

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.PurchaseInquiryId).Contains(keywords)
                || (x.PurchaseInquiryCode != null && x.PurchaseInquiryCode.Contains(keywords))
                || SqlFunc.ToString(x.LineNumber).Contains(keywords)
                || (x.AllocationCategory != null && x.AllocationCategory.Contains(keywords))
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || (x.MaterialName != null && x.MaterialName.Contains(keywords))
                || (x.MaterialSpecification != null && x.MaterialSpecification.Contains(keywords))
                || (x.InquiryUnit != null && x.InquiryUnit.Contains(keywords))
                || SqlFunc.ToString(x.InquiryQuantity).Contains(keywords)
                || SqlFunc.ToString(x.PurchasePerUnit).Contains(keywords)
                || SqlFunc.ToString(x.QuotedUnitPrice).Contains(keywords)
                || SqlFunc.ToString(x.QuotedAmount).Contains(keywords)
                || (x.TargetSupplierCode != null && x.TargetSupplierCode.Contains(keywords))
                || (x.TargetSupplierName != null && x.TargetSupplierName.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.PurchaseInquiryId.HasValue == true)
        {
            exp = exp.And(x => x.PurchaseInquiryId == queryDto.PurchaseInquiryId);
        }

        if (!string.IsNullOrEmpty(queryDto?.PurchaseInquiryCode))
        {
            exp = exp.And(x => x.PurchaseInquiryCode != null && x.PurchaseInquiryCode.Contains(queryDto.PurchaseInquiryCode));
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            exp = exp.And(x => x.LineNumber == queryDto.LineNumber);
        }

        if (!string.IsNullOrEmpty(queryDto?.AllocationCategory))
        {
            exp = exp.And(x => x.AllocationCategory != null && x.AllocationCategory.Contains(queryDto.AllocationCategory));
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

        if (!string.IsNullOrEmpty(queryDto?.InquiryUnit))
        {
            exp = exp.And(x => x.InquiryUnit != null && x.InquiryUnit.Contains(queryDto.InquiryUnit));
        }

        if (queryDto?.InquiryQuantity.HasValue == true)
        {
            exp = exp.And(x => x.InquiryQuantity == queryDto.InquiryQuantity);
        }

        if (queryDto?.PurchasePerUnit.HasValue == true)
        {
            exp = exp.And(x => x.PurchasePerUnit == queryDto.PurchasePerUnit);
        }

        if (queryDto?.QuotedUnitPrice.HasValue == true)
        {
            exp = exp.And(x => x.QuotedUnitPrice == queryDto.QuotedUnitPrice);
        }

        if (queryDto?.QuotedAmount.HasValue == true)
        {
            exp = exp.And(x => x.QuotedAmount == queryDto.QuotedAmount);
        }

        if (!string.IsNullOrEmpty(queryDto?.TargetSupplierCode))
        {
            exp = exp.And(x => x.TargetSupplierCode != null && x.TargetSupplierCode.Contains(queryDto.TargetSupplierCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.TargetSupplierName))
        {
            exp = exp.And(x => x.TargetSupplierName != null && x.TargetSupplierName.Contains(queryDto.TargetSupplierName));
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
