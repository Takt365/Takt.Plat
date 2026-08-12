// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Procurement
// 文件名称：TaktPurchasePriceItemService.cs
// 创建时间：2026-08-10
// 创建人：Takt365(Cursor AI)
// 功能描述：采购价格明细应用服务实现
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
/// 采购价格明细应用服务
/// </summary>
public class TaktPurchasePriceItemService : TaktServiceBase, ITaktPurchasePriceItemService
{
    private readonly ITaktCompanyRepository<TaktPurchasePriceItem> _purchasePriceItemRepository;
    private readonly ITaktCompanyRepository<TaktPurchasePriceScaleQuantity> _purchasePriceScaleQuantityRepository;
    private readonly ITaktCompanyRepository<TaktPurchasePriceScaleValue> _purchasePriceScaleValueRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="purchasePriceItemRepository">采购价格明细仓储</param>
    /// <param name="purchasePriceScaleQuantityRepository">PurchasePriceScaleQuantity仓储</param>
    /// <param name="purchasePriceScaleValueRepository">PurchasePriceScaleValue仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPurchasePriceItemService(
        ITaktCompanyRepository<TaktPurchasePriceItem> purchasePriceItemRepository,
        ITaktCompanyRepository<TaktPurchasePriceScaleQuantity> purchasePriceScaleQuantityRepository,
        ITaktCompanyRepository<TaktPurchasePriceScaleValue> purchasePriceScaleValueRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _purchasePriceItemRepository = purchasePriceItemRepository;
        _purchasePriceScaleQuantityRepository = purchasePriceScaleQuantityRepository;
        _purchasePriceScaleValueRepository = purchasePriceScaleValueRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取采购价格明细列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktPurchasePriceItemDto>> GetPurchasePriceItemListAsync(TaktPurchasePriceItemQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktPurchasePriceItemDto>.Create(
                new List<TaktPurchasePriceItemDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
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
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.IsObsolete == 0,
            x => x.PurchasePriceCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.PurchasePriceCode,
            DictLabel = e.PurchasePriceCode,
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
        entity.IsObsolete = 0;
        var isUnique_ix_takt_logistics_procurement_purchase_price_item_seq_unique = await _uniqueValidator.IsUniqueAsync(
            _purchasePriceItemRepository,
            x => x.PurchasePriceId == entity.PurchasePriceId
                && x.PurchasePriceSeq == entity.PurchasePriceSeq);
        if (!isUnique_ix_takt_logistics_procurement_purchase_price_item_seq_unique)
        {
            throw new TaktBusinessException("采购价格明细的PurchasePriceId、PurchasePriceSeq已存在");
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
        var isUnique_ix_takt_logistics_procurement_purchase_price_item_seq_unique = await _uniqueValidator.IsUniqueAsync(
            _purchasePriceItemRepository,
            x => x.PurchasePriceId == entity.PurchasePriceId
                && x.PurchasePriceSeq == entity.PurchasePriceSeq,
            id);
        if (!isUnique_ix_takt_logistics_procurement_purchase_price_item_seq_unique)
        {
            throw new TaktBusinessException("采购价格明细的PurchasePriceId、PurchasePriceSeq已存在");
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
        await _purchasePriceScaleQuantityRepository.DeleteAsync(x => x.PurchasePriceItemId == entity.Id);
        await _purchasePriceScaleValueRepository.DeleteAsync(x => x.PurchasePriceItemId == entity.Id);
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
    /// 更新采购价格明细作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchasePriceItemDto> UpdatePurchasePriceItemObsoleteAsync(TaktPurchasePriceItemObsoleteDto dto)
    {
        var entity = await _purchasePriceItemRepository.GetByIdAsync(dto.PurchasePriceItemId);
        if (entity == null)
        {
            throw new TaktBusinessException("采购价格明细不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("采购价格明细不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
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
                var importKey = $"{entity.PurchasePriceId}|{entity.PurchasePriceSeq}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PurchasePriceId、PurchasePriceSeq）");
                }
                var isUnique_ix_takt_logistics_procurement_purchase_price_item_seq_unique = await _uniqueValidator.IsUniqueAsync(
                    _purchasePriceItemRepository,
                    x => x.PurchasePriceId == entity.PurchasePriceId
                        && x.PurchasePriceSeq == entity.PurchasePriceSeq);
                if (!isUnique_ix_takt_logistics_procurement_purchase_price_item_seq_unique)
                {
                    throw new TaktBusinessException("采购价格明细的PurchasePriceId、PurchasePriceSeq已存在");
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
        var queryDto = query ?? new TaktPurchasePriceItemQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPurchasePriceItemExportDto>(),
                sheetName ?? "采购价格明细数据",
                fileName ?? "采购价格明细导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _purchasePriceItemRepository.GetListAsync(predicate);
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
    /// 将指定主表下全部未作废采购价格数量等级标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="purchasePriceItemId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkPurchasePriceScaleQuantitysObsoleteAsync(long purchasePriceItemId)
    {
        if (purchasePriceItemId <= 0)
        {
            return;
        }
        var rows = await _purchasePriceScaleQuantityRepository.GetListAsync(
            x => x.PurchasePriceItemId == purchasePriceItemId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _purchasePriceScaleQuantityRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 将指定主表下全部未作废采购价格价值等级标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="purchasePriceItemId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkPurchasePriceScaleValuesObsoleteAsync(long purchasePriceItemId)
    {
        if (purchasePriceItemId <= 0)
        {
            return;
        }
        var rows = await _purchasePriceScaleValueRepository.GetListAsync(
            x => x.PurchasePriceItemId == purchasePriceItemId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _purchasePriceScaleValueRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 填充采购价格明细详情（加载 OneToMany 子表：采购价格数量等级、采购价格价值等级）
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
        // 采购价格数量等级 → dto.ScaleQuantities（含作废行）
        var scalequantities = await _purchasePriceScaleQuantityRepository.GetListAsync(x => x.PurchasePriceItemId == entity.Id);
        dto.ScaleQuantities = scalequantities.Adapt<List<TaktPurchasePriceScaleQuantityDto>>();
        // 采购价格价值等级 → dto.ScaleValues（含作废行）
        var scalevalues = await _purchasePriceScaleValueRepository.GetListAsync(x => x.PurchasePriceItemId == entity.Id);
        dto.ScaleValues = scalevalues.Adapt<List<TaktPurchasePriceScaleValueDto>>();
    }

    /// <summary>
    /// 保存采购价格明细子表级联（采购价格数量等级、采购价格价值等级；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SavePurchasePriceItemChildrenAsync(TaktPurchasePriceItem entity, TaktPurchasePriceItemCreateDto dto)
    {
        // 采购价格数量等级（ScaleQuantities）
        List<TaktPurchasePriceScaleQuantityUpdateDto>? scaleQuantitiesForSave;
        if (dto is TaktPurchasePriceItemUpdateDto updateDtoForScaleQuantities && updateDtoForScaleQuantities.ScaleQuantities != null)
        {
            scaleQuantitiesForSave = updateDtoForScaleQuantities.ScaleQuantities;
        }
        else if (dto.ScaleQuantities != null)
        {
            scaleQuantitiesForSave = dto.ScaleQuantities.Adapt<List<TaktPurchasePriceScaleQuantityUpdateDto>>();
        }
        else
        {
            scaleQuantitiesForSave = null;
        }
        if (scaleQuantitiesForSave is not { Count: > 0 })
        {
            await MarkPurchasePriceScaleQuantitysObsoleteAsync(entity.Id);
        }
        else
        {
            var existingList = await _purchasePriceScaleQuantityRepository.GetListAsync(x => x.PurchasePriceItemId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktPurchasePriceScaleQuantity>();
            for (var i = 0; i < scaleQuantitiesForSave.Count; i++)
            {
                var childDto = scaleQuantitiesForSave[i];
                childDto.PurchasePriceItemId = entity.Id;
                if (childDto.PurchasePriceScaleQuantityId > 0)
                {
                    if (!existingById.TryGetValue(childDto.PurchasePriceScaleQuantityId, out var target))
                    {
                        throw new TaktBusinessException("采购价格数量等级不存在（PurchasePriceScaleQuantityId={childDto.PurchasePriceScaleQuantityId}）");
                    }
                    if (target.PurchasePriceItemId != entity.Id)
                    {
                        throw new TaktBusinessException("采购价格数量等级不属于当前主表（PurchasePriceScaleQuantityId={childDto.PurchasePriceScaleQuantityId}）");
                    }
                    submittedIds.Add(childDto.PurchasePriceScaleQuantityId);
                    childDto.Adapt(target);
                    target.Id = childDto.PurchasePriceScaleQuantityId;
                    target.PurchasePriceItemId = entity.Id;
                    target.IsObsolete = 0;
                    await _purchasePriceScaleQuantityRepository.UpdateAsync(target);
                }
                else
                {
                    var child = childDto.Adapt<TaktPurchasePriceScaleQuantity>();
                    child.Id = 0;
                    child.PurchasePriceItemId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _purchasePriceScaleQuantityRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                await _purchasePriceScaleQuantityRepository.CreateRangeAsync(toCreate);
            }
        }
        // 采购价格价值等级（ScaleValues）
        List<TaktPurchasePriceScaleValueUpdateDto>? scaleValuesForSave;
        if (dto is TaktPurchasePriceItemUpdateDto updateDtoForScaleValues && updateDtoForScaleValues.ScaleValues != null)
        {
            scaleValuesForSave = updateDtoForScaleValues.ScaleValues;
        }
        else if (dto.ScaleValues != null)
        {
            scaleValuesForSave = dto.ScaleValues.Adapt<List<TaktPurchasePriceScaleValueUpdateDto>>();
        }
        else
        {
            scaleValuesForSave = null;
        }
        if (scaleValuesForSave is not { Count: > 0 })
        {
            await MarkPurchasePriceScaleValuesObsoleteAsync(entity.Id);
        }
        else
        {
            var existingList = await _purchasePriceScaleValueRepository.GetListAsync(x => x.PurchasePriceItemId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktPurchasePriceScaleValue>();
            for (var i = 0; i < scaleValuesForSave.Count; i++)
            {
                var childDto = scaleValuesForSave[i];
                childDto.PurchasePriceItemId = entity.Id;
                if (childDto.PurchasePriceScaleValueId > 0)
                {
                    if (!existingById.TryGetValue(childDto.PurchasePriceScaleValueId, out var target))
                    {
                        throw new TaktBusinessException("采购价格价值等级不存在（PurchasePriceScaleValueId={childDto.PurchasePriceScaleValueId}）");
                    }
                    if (target.PurchasePriceItemId != entity.Id)
                    {
                        throw new TaktBusinessException("采购价格价值等级不属于当前主表（PurchasePriceScaleValueId={childDto.PurchasePriceScaleValueId}）");
                    }
                    submittedIds.Add(childDto.PurchasePriceScaleValueId);
                    childDto.Adapt(target);
                    target.Id = childDto.PurchasePriceScaleValueId;
                    target.PurchasePriceItemId = entity.Id;
                    target.IsObsolete = 0;
                    await _purchasePriceScaleValueRepository.UpdateAsync(target);
                }
                else
                {
                    var child = childDto.Adapt<TaktPurchasePriceScaleValue>();
                    child.Id = 0;
                    child.PurchasePriceItemId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _purchasePriceScaleValueRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                await _purchasePriceScaleValueRepository.CreateRangeAsync(toCreate);
            }
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

        if (queryDto?.IsObsolete.HasValue == true)
        {
            exp = exp.And(x => x.IsObsolete == queryDto.IsObsolete);
        }
        else
        {
            exp = exp.And(x => x.IsObsolete == 0);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.PurchasePriceCode != null && x.PurchasePriceCode.Contains(keywords))
                || (x.PriceType != null && x.PriceType.Contains(keywords))
                || (x.ScaleType != null && x.ScaleType.Contains(keywords))
                || (x.ScaleBasis != null && x.ScaleBasis.Contains(keywords))
                || (x.ScaleUnit != null && x.ScaleUnit.Contains(keywords))
                || (x.ScaleCurrencyCode != null && x.ScaleCurrencyCode.Contains(keywords))
                || (x.CalculationType != null && x.CalculationType.Contains(keywords))
                || (x.ConditionCurrencyCode != null && x.ConditionCurrencyCode.Contains(keywords))
                || (x.UnitOfMeasure != null && x.UnitOfMeasure.Contains(keywords))
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
            );
        }

        if (queryDto?.PurchasePriceId.HasValue == true)
        {
            var purchasePriceId = queryDto.PurchasePriceId;
            exp = exp.And(x => x.PurchasePriceId == purchasePriceId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PurchasePriceCode))
        {
            var purchasePriceCode = queryDto.PurchasePriceCode;
            exp = exp.And(x => x.PurchasePriceCode != null && x.PurchasePriceCode.Contains(purchasePriceCode));
        }

        if (queryDto?.PurchasePriceSeq.HasValue == true)
        {
            var purchasePriceSeq = queryDto.PurchasePriceSeq;
            exp = exp.And(x => x.PurchasePriceSeq == purchasePriceSeq);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PriceType))
        {
            var priceType = queryDto.PriceType;
            exp = exp.And(x => x.PriceType != null && x.PriceType.Contains(priceType));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ScaleType))
        {
            var scaleType = queryDto.ScaleType;
            exp = exp.And(x => x.ScaleType != null && x.ScaleType.Contains(scaleType));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ScaleBasis))
        {
            var scaleBasis = queryDto.ScaleBasis;
            exp = exp.And(x => x.ScaleBasis != null && x.ScaleBasis.Contains(scaleBasis));
        }

        if (queryDto?.ScaleQuantity.HasValue == true)
        {
            var scaleQuantity = queryDto.ScaleQuantity;
            exp = exp.And(x => x.ScaleQuantity == scaleQuantity);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ScaleUnit))
        {
            var scaleUnit = queryDto.ScaleUnit;
            exp = exp.And(x => x.ScaleUnit != null && x.ScaleUnit.Contains(scaleUnit));
        }

        if (queryDto?.ScaleValue.HasValue == true)
        {
            var scaleValue = queryDto.ScaleValue;
            exp = exp.And(x => x.ScaleValue == scaleValue);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ScaleCurrencyCode))
        {
            var scaleCurrencyCode = queryDto.ScaleCurrencyCode;
            exp = exp.And(x => x.ScaleCurrencyCode != null && x.ScaleCurrencyCode.Contains(scaleCurrencyCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CalculationType))
        {
            var calculationType = queryDto.CalculationType;
            exp = exp.And(x => x.CalculationType != null && x.CalculationType.Contains(calculationType));
        }

        if (queryDto?.Price.HasValue == true)
        {
            var price = queryDto.Price;
            exp = exp.And(x => x.Price == price);
        }

        if (queryDto?.UntaxedPrice.HasValue == true)
        {
            var untaxedPrice = queryDto.UntaxedPrice;
            exp = exp.And(x => x.UntaxedPrice == untaxedPrice);
        }

        if (queryDto?.TaxIncludedPrice.HasValue == true)
        {
            var taxIncludedPrice = queryDto.TaxIncludedPrice;
            exp = exp.And(x => x.TaxIncludedPrice == taxIncludedPrice);
        }

        if (queryDto?.TaxAmount.HasValue == true)
        {
            var taxAmount = queryDto.TaxAmount;
            exp = exp.And(x => x.TaxAmount == taxAmount);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ConditionCurrencyCode))
        {
            var conditionCurrencyCode = queryDto.ConditionCurrencyCode;
            exp = exp.And(x => x.ConditionCurrencyCode != null && x.ConditionCurrencyCode.Contains(conditionCurrencyCode));
        }

        if (queryDto?.PriceUnit.HasValue == true)
        {
            var priceUnit = queryDto.PriceUnit;
            exp = exp.And(x => x.PriceUnit == priceUnit);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.UnitOfMeasure))
        {
            var unitOfMeasure = queryDto.UnitOfMeasure;
            exp = exp.And(x => x.UnitOfMeasure != null && x.UnitOfMeasure.Contains(unitOfMeasure));
        }

        if (queryDto?.MinOrderQuantity.HasValue == true)
        {
            var minOrderQuantity = queryDto.MinOrderQuantity;
            exp = exp.And(x => x.MinOrderQuantity == minOrderQuantity);
        }

        if (queryDto?.RoundingValue.HasValue == true)
        {
            var roundingValue = queryDto.RoundingValue;
            exp = exp.And(x => x.RoundingValue == roundingValue);
        }

        if (queryDto?.PlannedDeliveryTimeDays.HasValue == true)
        {
            var plannedDeliveryTimeDays = queryDto.PlannedDeliveryTimeDays;
            exp = exp.And(x => x.PlannedDeliveryTimeDays == plannedDeliveryTimeDays);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ExtField))
        {
            var extField = queryDto.ExtField;
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(extField));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Remark))
        {
            var remark = queryDto.Remark;
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(remark));
        }

        if (queryDto?.CreatedAtStart.HasValue == true)
        {
            var createdAtStart = queryDto.CreatedAtStart;
            exp = exp.And(x => x.CreatedAt >= createdAtStart);
        }

        if (queryDto?.CreatedAtEnd.HasValue == true)
        {
            var createdAtEnd = queryDto.CreatedAtEnd;
            exp = exp.And(x => x.CreatedAt <= createdAtEnd);
        }

        if (!string.IsNullOrEmpty(queryDto?.CultureCode))
        {
            exp = exp.And(x => x.CultureCode != null && x.CultureCode.Contains(queryDto.CultureCode));
        }
        if (!string.IsNullOrWhiteSpace(queryDto?.PlantCode))
        {
            var plantCode = queryDto.PlantCode;
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(plantCode));
        }


        return exp.ToExpression();
    }

    /// <summary>
    /// 是否存在任一业务查询条件（KeyWords / 字段 / 日期范围）；无参时列表与导出返回空，避免全表扫描
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>有条件为 true</returns>
    private static bool HasAnyListQueryFilter(TaktPurchasePriceItemQueryDto? queryDto)
    {
        if (queryDto == null)
        {
            return false;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.KeyWords))
        {
            return true;
        }
        if (queryDto.PurchasePriceId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PurchasePriceCode))
        {
            return true;
        }
        if (queryDto.PurchasePriceSeq.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PriceType))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ScaleType))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ScaleBasis))
        {
            return true;
        }
        if (queryDto.ScaleQuantity.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ScaleUnit))
        {
            return true;
        }
        if (queryDto.ScaleValue.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ScaleCurrencyCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CalculationType))
        {
            return true;
        }
        if (queryDto.Price.HasValue)
        {
            return true;
        }
        if (queryDto.UntaxedPrice.HasValue)
        {
            return true;
        }
        if (queryDto.TaxIncludedPrice.HasValue)
        {
            return true;
        }
        if (queryDto.TaxAmount.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ConditionCurrencyCode))
        {
            return true;
        }
        if (queryDto.PriceUnit.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.UnitOfMeasure))
        {
            return true;
        }
        if (queryDto.MinOrderQuantity.HasValue)
        {
            return true;
        }
        if (queryDto.RoundingValue.HasValue)
        {
            return true;
        }
        if (queryDto.PlannedDeliveryTimeDays.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ExtField))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Remark))
        {
            return true;
        }
        if (queryDto.IsObsolete.HasValue)
        {
            return true;
        }
        if (queryDto.CreatedAtStart.HasValue || queryDto.CreatedAtEnd.HasValue)
        {
            return true;
        }
        return false;
    }
}
