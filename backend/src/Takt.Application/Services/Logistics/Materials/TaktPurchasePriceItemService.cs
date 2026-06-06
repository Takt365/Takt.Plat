// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Materials
// 文件名称：TaktPurchasePriceItemService.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Cursor AI)
// 功能描述：采购价格明细应用服务实现
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
/// 采购价格明细应用服务
/// </summary>
public class TaktPurchasePriceItemService : TaktServiceBase, ITaktPurchasePriceItemService
{
    private readonly ITaktCompanyRepository<TaktPurchasePriceItem> _purchasePriceItemRepository;
    private readonly ITaktCompanyRepository<TaktPurchasePriceScale> _purchasePriceScaleRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="purchasePriceItemRepository">采购价格明细仓储</param>
    /// <param name="purchasePriceScaleRepository">PurchasePriceScale仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPurchasePriceItemService(
        ITaktCompanyRepository<TaktPurchasePriceItem> purchasePriceItemRepository,
        ITaktCompanyRepository<TaktPurchasePriceScale> purchasePriceScaleRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _purchasePriceItemRepository = purchasePriceItemRepository;
        _purchasePriceScaleRepository = purchasePriceScaleRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取采购价格明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktPurchasePriceItemDto>> GetPurchasePriceItemListAsync(TaktPurchasePriceItemQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _purchasePriceItemRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktPurchasePriceItemDto>.Create(
            data.Adapt<List<TaktPurchasePriceItemDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取采购价格明细
    /// </summary>
    /// <param name="id">采购价格明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchasePriceItemDto?> GetPurchasePriceItemByIdAsync(long id)
    {
        var entity = await _purchasePriceItemRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktPurchasePriceItemDto>();
        await FillPurchasePriceItemDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取采购价格明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetPurchasePriceItemOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _purchasePriceItemRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.MaterialName,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.MaterialName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建采购价格明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchasePriceItemDto> CreatePurchasePriceItemAsync(TaktPurchasePriceItemCreateDto dto)
    {
        var entity = dto.Adapt<TaktPurchasePriceItem>();
        var isUnique_ix_takt_logistics_materials_purchase_price_item_price_line_unique = await _uniqueValidator.IsUniqueAsync(
            _purchasePriceItemRepository,
            x => x.PurchasePriceId == entity.PurchasePriceId
                && x.LineNumber == entity.LineNumber
                && x.MaterialCode == entity.MaterialCode);
        if (!isUnique_ix_takt_logistics_materials_purchase_price_item_price_line_unique)
        {
            throw new TaktBusinessException("采购价格明细的PurchasePriceId、LineNumber、MaterialCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _purchasePriceItemRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.PurchasePriceId == entity.PurchasePriceId,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.PurchasePriceId, maxSort);
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _purchasePriceItemRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.PurchasePriceId == entity.PurchasePriceId,
                x => x.LineNumber);
            var businessCode = !string.IsNullOrWhiteSpace(entity.PurchasePriceCode) ? entity.PurchasePriceCode : entity.PurchasePriceId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _purchasePriceItemRepository.CreateAsync(entity);
                await SavePurchasePriceItemChildrenAsync(entity, dto);
        return await GetPurchasePriceItemByIdAsync(entity.Id) ?? entity.Adapt<TaktPurchasePriceItemDto>();
    }

    /// <summary>
    /// 更新采购价格明细
    /// </summary>
    /// <param name="id">采购价格明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchasePriceItemDto> UpdatePurchasePriceItemAsync(long id, TaktPurchasePriceItemUpdateDto dto)
    {
        var entity = await _purchasePriceItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("采购价格明细不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_materials_purchase_price_item_price_line_unique = await _uniqueValidator.IsUniqueAsync(
            _purchasePriceItemRepository,
            x => x.PurchasePriceId == entity.PurchasePriceId
                && x.LineNumber == entity.LineNumber
                && x.MaterialCode == entity.MaterialCode,
            id);
        if (!isUnique_ix_takt_logistics_materials_purchase_price_item_price_line_unique)
        {
            throw new TaktBusinessException("采购价格明细的PurchasePriceId、LineNumber、MaterialCode已存在");
        }
        await _purchasePriceItemRepository.UpdateAsync(entity);
                await SavePurchasePriceItemChildrenAsync(entity, dto);
        return await GetPurchasePriceItemByIdAsync(id) ?? throw new TaktBusinessException("采购价格明细不存在");
    }

    /// <summary>
    /// 删除采购价格明细
    /// </summary>
    /// <param name="id">采购价格明细ID</param>
    /// <returns>任务</returns>
    public async Task DeletePurchasePriceItemByIdAsync(long id)
    {
        var entity = await _purchasePriceItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("采购价格明细不存在或已删除");
        }
        await _purchasePriceScaleRepository.DeleteAsync(x => x.PurchasePriceItemId == entity.Id);
        var deleted = await _purchasePriceItemRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("采购价格明细不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除采购价格明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeletePurchasePriceItemBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeletePurchasePriceItemByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新采购价格明细排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchasePriceItemDto> UpdatePurchasePriceItemSortAsync(TaktPurchasePriceItemSortDto dto)
    {
        var entity = await _purchasePriceItemRepository.GetByIdAsync(dto.PurchasePriceItemId);
        if (entity == null)
        {
            throw new TaktBusinessException("采购价格明细不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _purchasePriceItemRepository.UpdateAsync(entity);
        return await GetPurchasePriceItemByIdAsync(dto.PurchasePriceItemId) ?? throw new TaktBusinessException("采购价格明细不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetPurchasePriceItemTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktPurchasePriceItemTemplateDto>(
            sheetName ?? "采购价格明细导入模板",
            fileName ?? "采购价格明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入采购价格明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportPurchasePriceItemAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktPurchasePriceItemImportDto>(fileStream, sheetName ?? "采购价格明细导入模板");
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
                var entity = rows[i].Adapt<TaktPurchasePriceItem>();
                var importKey = $"{entity.PurchasePriceId}|{entity.LineNumber}|{entity.MaterialCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PurchasePriceId、LineNumber、MaterialCode）");
                }
                var isUnique_ix_takt_logistics_materials_purchase_price_item_price_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _purchasePriceItemRepository,
                    x => x.PurchasePriceId == entity.PurchasePriceId
                        && x.LineNumber == entity.LineNumber
                        && x.MaterialCode == entity.MaterialCode);
                if (!isUnique_ix_takt_logistics_materials_purchase_price_item_price_line_unique)
                {
                    throw new TaktBusinessException("采购价格明细的PurchasePriceId、LineNumber、MaterialCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    var maxSort = await _purchasePriceItemRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.PurchasePriceId == entity.PurchasePriceId,
                        x => x.SortOrder);
                    entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.PurchasePriceId, maxSort);
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _purchasePriceItemRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.PurchasePriceId == entity.PurchasePriceId,
                        x => x.LineNumber);
                    var businessCode = !string.IsNullOrWhiteSpace(entity.PurchasePriceCode) ? entity.PurchasePriceCode : entity.PurchasePriceId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _purchasePriceItemRepository.CreateAsync(entity);
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
    /// 导出采购价格明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportPurchasePriceItemAsync(TaktPurchasePriceItemQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktPurchasePriceItemQueryDto());
        var list = await _purchasePriceItemRepository.GetListForExportAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPurchasePriceItemExportDto>(),
                sheetName ?? "采购价格明细数据",
                fileName ?? "采购价格明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktPurchasePriceItemExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "采购价格明细数据",
            fileName ?? "采购价格明细导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充采购价格明细详情（加载 OneToMany 子表：采购价格阶梯）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillPurchasePriceItemDetailsAsync(TaktPurchasePriceItemDto dto, TaktPurchasePriceItem entity)
    {
        if (dto == null)
        {
            return;
        }
        // 采购价格阶梯 → dto.Scales
        var scales = await _purchasePriceScaleRepository.GetListAsync(x => x.PurchasePriceItemId == entity.Id);
        dto.Scales = scales.Adapt<List<TaktPurchasePriceScaleDto>>();
    }

    /// <summary>
    /// 保存采购价格明细子表级联（采购价格阶梯；Create/Update 后按主表 Id 先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SavePurchasePriceItemChildrenAsync(TaktPurchasePriceItem entity, TaktPurchasePriceItemCreateDto dto)
    {
        // 采购价格阶梯（Scales）
        if (dto.Scales is not { Count: > 0 })
        {
            await _purchasePriceScaleRepository.DeleteAsync(x => x.PurchasePriceItemId == entity.Id);
        }
        else
        {
            var scales = dto.Scales.Adapt<List<TaktPurchasePriceScale>>();
            foreach (var child in scales)
            {
                child.PurchasePriceItemId = entity.Id;
            }
            var scalesNeedSort = scales.Where(c => c.SortOrder <= 0).ToList();
            if (scalesNeedSort.Count > 0)
            {
                var maxSort = await _purchasePriceScaleRepository.GetMaxIntAsync(
                    x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.PurchasePriceItemId == entity.Id,
                    x => x.SortOrder);
                var sortSeq = _sortOrderGenerator.GenerateSequenceForMaster(entity.Id, scalesNeedSort.Count, maxSort).ToList();
                var sortIdx = 0;
                foreach (var child in scales)
                {
                    if (child.SortOrder <= 0)
                    {
                        child.SortOrder = sortSeq[sortIdx++];
                    }
                }
            }
            var scalesNeedLine = scales.Where(c => c.LineNumber <= 0).ToList();
            if (scalesNeedLine.Count > 0)
            {
                var businessCode = !string.IsNullOrWhiteSpace(entity.PurchasePriceCode) ? entity.PurchasePriceCode : entity.Id.ToString();
                var maxLine = await _purchasePriceScaleRepository.GetMaxIntAsync(
                    x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.PurchasePriceItemId == entity.Id,
                    x => x.LineNumber);
                var lineSeq = _lineNumberGenerator.GenerateSequence(businessCode, scalesNeedLine.Count, maxLine).ToList();
                var lineIdx = 0;
                foreach (var child in scales)
                {
                    if (child.LineNumber <= 0)
                    {
                        child.LineNumber = lineSeq[lineIdx++];
                    }
                }
            }
                        var seenKeys = new HashSet<string>(StringComparer.Ordinal);
                        for (var i = 0; i < scales.Count; i++)
                        {
                            var key = $"{scales[i].CompanyCode}|{scales[i].PurchasePriceItemId}|{scales[i].LineNumber}|{scales[i].StartQuantity}";
                            if (!seenKeys.Add(key))
                            {
                                throw new TaktBusinessException($"采购价格阶梯第{i + 1}项与本次提交的其他项重复（CompanyCode、PurchasePriceItemId、LineNumber、StartQuantity）");
                            }
                        }
            await _purchasePriceScaleRepository.DeleteAsync(x => x.PurchasePriceItemId == entity.Id);
            foreach (var child in scales)
            {
            var isUnique_ix_takt_logistics_materials_purchase_price_scale_item_line_unique = await _uniqueValidator.IsUniqueAsync(
                _purchasePriceScaleRepository,
                x => x.CompanyCode == child.CompanyCode
                    && x.PurchasePriceItemId == child.PurchasePriceItemId
                    && x.LineNumber == child.LineNumber
                    && x.StartQuantity == child.StartQuantity);
            if (!isUnique_ix_takt_logistics_materials_purchase_price_scale_item_line_unique)
            {
                throw new TaktBusinessException("采购价格阶梯的CompanyCode、PurchasePriceItemId、LineNumber、StartQuantity已存在");
            }
            }
            await _purchasePriceScaleRepository.CreateRangeAsync(scales);
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建采购价格明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktPurchasePriceItem, bool>> QueryExpression(TaktPurchasePriceItemQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktPurchasePriceItem>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.PurchasePriceId).Contains(keywords)
                || (x.PurchasePriceCode != null && x.PurchasePriceCode.Contains(keywords))
                || SqlFunc.ToString(x.LineNumber).Contains(keywords)
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || (x.MaterialName != null && x.MaterialName.Contains(keywords))
                || (x.MaterialSpecification != null && x.MaterialSpecification.Contains(keywords))
                || (x.PurchaseUnit != null && x.PurchaseUnit.Contains(keywords))
                || SqlFunc.ToString(x.PurchasePrice).Contains(keywords)
                || SqlFunc.ToString(x.MinPurchaseQuantity).Contains(keywords)
                || SqlFunc.ToString(x.MaxPurchaseQuantity).Contains(keywords)
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.PurchasePriceId.HasValue == true)
        {
            exp = exp.And(x => x.PurchasePriceId == queryDto.PurchasePriceId);
        }

        if (!string.IsNullOrEmpty(queryDto?.PurchasePriceCode))
        {
            exp = exp.And(x => x.PurchasePriceCode != null && x.PurchasePriceCode.Contains(queryDto.PurchasePriceCode));
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

        if (!string.IsNullOrEmpty(queryDto?.PurchaseUnit))
        {
            exp = exp.And(x => x.PurchaseUnit != null && x.PurchaseUnit.Contains(queryDto.PurchaseUnit));
        }

        if (queryDto?.PurchasePrice.HasValue == true)
        {
            exp = exp.And(x => x.PurchasePrice == queryDto.PurchasePrice);
        }

        if (queryDto?.MinPurchaseQuantity.HasValue == true)
        {
            exp = exp.And(x => x.MinPurchaseQuantity == queryDto.MinPurchaseQuantity);
        }

        if (queryDto?.MaxPurchaseQuantity.HasValue == true)
        {
            exp = exp.And(x => x.MaxPurchaseQuantity == queryDto.MaxPurchaseQuantity);
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            exp = exp.And(x => x.SortOrder == queryDto.SortOrder);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
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
