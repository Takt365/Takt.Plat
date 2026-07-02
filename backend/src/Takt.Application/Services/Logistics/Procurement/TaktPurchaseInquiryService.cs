// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Procurement
// 文件名称：TaktPurchaseInquiryService.cs
// 创建时间：2026-06-24
// 创建人：Takt365(Cursor AI)
// 功能描述：采购询价应用服务实现
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
/// 采购询价应用服务
/// </summary>
public class TaktPurchaseInquiryService : TaktServiceBase, ITaktPurchaseInquiryService
{
    private readonly ITaktCompanyRepository<TaktPurchaseInquiry> _purchaseInquiryRepository;
    private readonly ITaktCompanyRepository<TaktPurchaseInquiryItem> _purchaseInquiryItemRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="purchaseInquiryRepository">采购询价仓储</param>
    /// <param name="purchaseInquiryItemRepository">PurchaseInquiryItem仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPurchaseInquiryService(
        ITaktCompanyRepository<TaktPurchaseInquiry> purchaseInquiryRepository,
        ITaktCompanyRepository<TaktPurchaseInquiryItem> purchaseInquiryItemRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _purchaseInquiryRepository = purchaseInquiryRepository;
        _purchaseInquiryItemRepository = purchaseInquiryItemRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取采购询价列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktPurchaseInquiryDto>> GetPurchaseInquiryListAsync(TaktPurchaseInquiryQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _purchaseInquiryRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktPurchaseInquiryDto>.Create(
            data.Adapt<List<TaktPurchaseInquiryDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取采购询价
    /// </summary>
    /// <param name="id">采购询价ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseInquiryDto?> GetPurchaseInquiryByIdAsync(long id)
    {
        var entity = await _purchaseInquiryRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktPurchaseInquiryDto>();
        await FillPurchaseInquiryDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取采购询价选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetPurchaseInquiryOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _purchaseInquiryRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.InquiryStatus == 1,
            x => x.SupplierName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.SupplierName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建采购询价
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseInquiryDto> CreatePurchaseInquiryAsync(TaktPurchaseInquiryCreateDto dto)
    {
        var entity = dto.Adapt<TaktPurchaseInquiry>();
        var isUnique_ix_takt_logistics_materials_purchase_inquiry_code_unique = await _uniqueValidator.IsUniqueAsync(
            _purchaseInquiryRepository,
            x => x.PlantCode == entity.PlantCode
                && x.PurchaseInquiryCode == entity.PurchaseInquiryCode
                && x.InquiryDate == entity.InquiryDate);
        if (!isUnique_ix_takt_logistics_materials_purchase_inquiry_code_unique)
        {
            throw new TaktBusinessException("采购询价的PlantCode、PurchaseInquiryCode、InquiryDate已存在");
        }
        entity = await _purchaseInquiryRepository.CreateAsync(entity);
                await SavePurchaseInquiryChildrenAsync(entity, dto);
        return await GetPurchaseInquiryByIdAsync(entity.Id) ?? entity.Adapt<TaktPurchaseInquiryDto>();
    }

    /// <summary>
    /// 更新采购询价
    /// </summary>
    /// <param name="id">采购询价ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseInquiryDto> UpdatePurchaseInquiryAsync(long id, TaktPurchaseInquiryUpdateDto dto)
    {
        var entity = await _purchaseInquiryRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("采购询价不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_materials_purchase_inquiry_code_unique = await _uniqueValidator.IsUniqueAsync(
            _purchaseInquiryRepository,
            x => x.PlantCode == entity.PlantCode
                && x.PurchaseInquiryCode == entity.PurchaseInquiryCode
                && x.InquiryDate == entity.InquiryDate,
            id);
        if (!isUnique_ix_takt_logistics_materials_purchase_inquiry_code_unique)
        {
            throw new TaktBusinessException("采购询价的PlantCode、PurchaseInquiryCode、InquiryDate已存在");
        }
        await _purchaseInquiryRepository.UpdateAsync(entity);
                await SavePurchaseInquiryChildrenAsync(entity, dto);
        return await GetPurchaseInquiryByIdAsync(id) ?? throw new TaktBusinessException("采购询价不存在");
    }

    /// <summary>
    /// 删除采购询价
    /// </summary>
    /// <param name="id">采购询价ID</param>
    /// <returns>任务</returns>
    public async Task DeletePurchaseInquiryByIdAsync(long id)
    {
        var entity = await _purchaseInquiryRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("采购询价不存在或已删除");
        }
        await _purchaseInquiryItemRepository.DeleteAsync(x => x.PurchaseInquiryId == entity.Id);
        var deleted = await _purchaseInquiryRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("采购询价不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除采购询价
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeletePurchaseInquiryBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeletePurchaseInquiryByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新采购询价状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseInquiryDto> UpdatePurchaseInquiryStatusAsync(TaktPurchaseInquiryStatusDto dto)
    {
        var entity = await _purchaseInquiryRepository.GetByIdAsync(dto.PurchaseInquiryId);
        if (entity == null)
        {
            throw new TaktBusinessException("采购询价不存在");
        }
        entity.InquiryStatus = dto.InquiryStatus;
        await _purchaseInquiryRepository.UpdateAsync(entity);
        return await GetPurchaseInquiryByIdAsync(dto.PurchaseInquiryId) ?? throw new TaktBusinessException("采购询价不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetPurchaseInquiryTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktPurchaseInquiryTemplateDto>(
            sheetName ?? "采购询价导入模板",
            fileName ?? "采购询价导入模板.xlsx");
    }

    /// <summary>
    /// 导入采购询价
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportPurchaseInquiryAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktPurchaseInquiryImportDto>(fileStream, sheetName ?? "采购询价导入模板");
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
                var entity = rows[i].Adapt<TaktPurchaseInquiry>();
                var importKey = $"{entity.PlantCode}|{entity.PurchaseInquiryCode}|{entity.InquiryDate}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、PurchaseInquiryCode、InquiryDate）");
                }
                var isUnique_ix_takt_logistics_materials_purchase_inquiry_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _purchaseInquiryRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.PurchaseInquiryCode == entity.PurchaseInquiryCode
                        && x.InquiryDate == entity.InquiryDate);
                if (!isUnique_ix_takt_logistics_materials_purchase_inquiry_code_unique)
                {
                    throw new TaktBusinessException("采购询价的PlantCode、PurchaseInquiryCode、InquiryDate已存在");
                }
                await _purchaseInquiryRepository.CreateAsync(entity);
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
    /// 导出采购询价
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportPurchaseInquiryAsync(TaktPurchaseInquiryQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktPurchaseInquiryQueryDto());
        var list = await _purchaseInquiryRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPurchaseInquiryExportDto>(),
                sheetName ?? "采购询价数据",
                fileName ?? "采购询价导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktPurchaseInquiryExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "采购询价数据",
            fileName ?? "采购询价导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充采购询价详情（加载 OneToMany 子表：采购询价明细）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillPurchaseInquiryDetailsAsync(TaktPurchaseInquiryDto dto, TaktPurchaseInquiry entity)
    {
        if (dto == null)
        {
            return;
        }
        // 采购询价明细 → dto.Items
        var items = await _purchaseInquiryItemRepository.GetListAsync(x => x.PurchaseInquiryId == entity.Id);
        dto.Items = items.Adapt<List<TaktPurchaseInquiryItemDto>>();
    }

    /// <summary>
    /// 保存采购询价子表级联（采购询价明细；Create/Update 后按主表 Id 先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SavePurchaseInquiryChildrenAsync(TaktPurchaseInquiry entity, TaktPurchaseInquiryCreateDto dto)
    {
        // 采购询价明细（Items）
        if (dto.Items is not { Count: > 0 })
        {
            await _purchaseInquiryItemRepository.DeleteAsync(x => x.PurchaseInquiryId == entity.Id);
        }
        else
        {
            var items = dto.Items.Adapt<List<TaktPurchaseInquiryItem>>();
            foreach (var child in items)
            {
                child.PurchaseInquiryId = entity.Id;
            }
            var itemsNeedLine = items.Where(c => c.LineNumber <= 0).ToList();
            if (itemsNeedLine.Count > 0)
            {
                var businessCode = !string.IsNullOrWhiteSpace(entity.PurchaseInquiryCode) ? entity.PurchaseInquiryCode : entity.Id.ToString();
                var maxLine = await _purchaseInquiryItemRepository.GetMaxIntAsync(
                    x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.PurchaseInquiryId == entity.Id,
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
                            var key = $"{items[i].CompanyCode}|{items[i].PurchaseInquiryId}|{items[i].LineNumber}|{items[i].MaterialCode}";
                            if (!seenKeys.Add(key))
                            {
                                throw new TaktBusinessException($"采购询价明细第{i + 1}项与本次提交的其他项重复（CompanyCode、PurchaseInquiryId、LineNumber、MaterialCode）");
                            }
                        }
            await _purchaseInquiryItemRepository.DeleteAsync(x => x.PurchaseInquiryId == entity.Id);
            foreach (var child in items)
            {
            var isUnique_ix_takt_logistics_materials_purchase_inquiry_item_line_unique = await _uniqueValidator.IsUniqueAsync(
                _purchaseInquiryItemRepository,
                x => x.CompanyCode == child.CompanyCode
                    && x.PurchaseInquiryId == child.PurchaseInquiryId
                    && x.LineNumber == child.LineNumber
                    && x.MaterialCode == child.MaterialCode);
            if (!isUnique_ix_takt_logistics_materials_purchase_inquiry_item_line_unique)
            {
                throw new TaktBusinessException("采购询价明细的CompanyCode、PurchaseInquiryId、LineNumber、MaterialCode已存在");
            }
            }
            await _purchaseInquiryItemRepository.CreateRangeAsync(items);
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建采购询价查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktPurchaseInquiry, bool>> QueryExpression(TaktPurchaseInquiryQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktPurchaseInquiry>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.PurchaseInquiryCode != null && x.PurchaseInquiryCode.Contains(keywords))
                || SqlFunc.ToString(x.InquiryId).Contains(keywords)
                || (x.InquiryBy != null && x.InquiryBy.Contains(keywords))
                || (x.SupplierCode != null && x.SupplierCode.Contains(keywords))
                || (x.SupplierName != null && x.SupplierName.Contains(keywords))
                || SqlFunc.ToString(x.TotalQuantity).Contains(keywords)
                || SqlFunc.ToString(x.TotalAmount).Contains(keywords)
                || SqlFunc.ToString(x.ConvertedQuantity).Contains(keywords)
                || SqlFunc.ToString(x.ConvertedAmount).Contains(keywords)
                || (x.InquiryReason != null && x.InquiryReason.Contains(keywords))
                || SqlFunc.ToString(x.InquiryStatus).Contains(keywords)
                || SqlFunc.ToString(x.ConvertedStatus).Contains(keywords)
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.InquiryDate).Contains(keywords)
                || SqlFunc.ToString(x.QuoteDeadlineDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.PurchaseInquiryCode))
        {
            exp = exp.And(x => x.PurchaseInquiryCode != null && x.PurchaseInquiryCode.Contains(queryDto.PurchaseInquiryCode));
        }

        if (queryDto?.InquiryId.HasValue == true)
        {
            exp = exp.And(x => x.InquiryId == queryDto.InquiryId);
        }

        if (!string.IsNullOrEmpty(queryDto?.InquiryBy))
        {
            exp = exp.And(x => x.InquiryBy != null && x.InquiryBy.Contains(queryDto.InquiryBy));
        }

        if (!string.IsNullOrEmpty(queryDto?.SupplierCode))
        {
            exp = exp.And(x => x.SupplierCode != null && x.SupplierCode.Contains(queryDto.SupplierCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.SupplierName))
        {
            exp = exp.And(x => x.SupplierName != null && x.SupplierName.Contains(queryDto.SupplierName));
        }

        if (queryDto?.TotalQuantity.HasValue == true)
        {
            exp = exp.And(x => x.TotalQuantity == queryDto.TotalQuantity);
        }

        if (queryDto?.TotalAmount.HasValue == true)
        {
            exp = exp.And(x => x.TotalAmount == queryDto.TotalAmount);
        }

        if (queryDto?.ConvertedQuantity.HasValue == true)
        {
            exp = exp.And(x => x.ConvertedQuantity == queryDto.ConvertedQuantity);
        }

        if (queryDto?.ConvertedAmount.HasValue == true)
        {
            exp = exp.And(x => x.ConvertedAmount == queryDto.ConvertedAmount);
        }

        if (!string.IsNullOrEmpty(queryDto?.InquiryReason))
        {
            exp = exp.And(x => x.InquiryReason != null && x.InquiryReason.Contains(queryDto.InquiryReason));
        }

        if (queryDto?.InquiryStatus.HasValue == true)
        {
            exp = exp.And(x => x.InquiryStatus == queryDto.InquiryStatus);
        }

        if (queryDto?.ConvertedStatus.HasValue == true)
        {
            exp = exp.And(x => x.ConvertedStatus == queryDto.ConvertedStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.InquiryDateStart.HasValue == true)
        {
            exp = exp.And(x => x.InquiryDate >= queryDto.InquiryDateStart);
        }

        if (queryDto?.InquiryDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.InquiryDate <= queryDto.InquiryDateEnd);
        }

        if (queryDto?.QuoteDeadlineDateStart.HasValue == true)
        {
            exp = exp.And(x => x.QuoteDeadlineDate >= queryDto.QuoteDeadlineDateStart);
        }

        if (queryDto?.QuoteDeadlineDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.QuoteDeadlineDate <= queryDto.QuoteDeadlineDateEnd);
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
