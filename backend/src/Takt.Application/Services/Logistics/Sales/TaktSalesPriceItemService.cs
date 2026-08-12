// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Sales
// 文件名称：TaktSalesPriceItemService.cs
// 创建时间：2026-08-06
// 创建人：Takt365(Cursor AI)
// 功能描述：销售价格明细应用服务实现
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
/// 销售价格明细应用服务
/// </summary>
public class TaktSalesPriceItemService : TaktServiceBase, ITaktSalesPriceItemService
{
    private readonly ITaktCompanyRepository<TaktSalesPriceItem> _salesPriceItemRepository;
    private readonly ITaktCompanyRepository<TaktSalesPriceScaleQuantity> _salesPriceScaleQuantityRepository;
    private readonly ITaktCompanyRepository<TaktSalesPriceScaleValue> _salesPriceScaleValueRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="salesPriceItemRepository">销售价格明细仓储</param>
    /// <param name="salesPriceScaleQuantityRepository">SalesPriceScaleQuantity仓储</param>
    /// <param name="salesPriceScaleValueRepository">SalesPriceScaleValue仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSalesPriceItemService(
        ITaktCompanyRepository<TaktSalesPriceItem> salesPriceItemRepository,
        ITaktCompanyRepository<TaktSalesPriceScaleQuantity> salesPriceScaleQuantityRepository,
        ITaktCompanyRepository<TaktSalesPriceScaleValue> salesPriceScaleValueRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _salesPriceItemRepository = salesPriceItemRepository;
        _salesPriceScaleQuantityRepository = salesPriceScaleQuantityRepository;
        _salesPriceScaleValueRepository = salesPriceScaleValueRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取销售价格明细列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSalesPriceItemDto>> GetSalesPriceItemListAsync(TaktSalesPriceItemQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktSalesPriceItemDto>.Create(
                new List<TaktSalesPriceItemDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _salesPriceItemRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSalesPriceItemDto>.Create(
            data.Adapt<List<TaktSalesPriceItemDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取销售价格明细
    /// </summary>
    /// <param name="id">销售价格明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesPriceItemDto?> GetSalesPriceItemByIdAsync(long id)
    {
        var entity = await _salesPriceItemRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktSalesPriceItemDto>();
        await FillSalesPriceItemDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取销售价格明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSalesPriceItemOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _salesPriceItemRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.IsObsolete == 0,
            x => x.SalesPriceCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.SalesPriceCode,
            DictLabel = e.SalesPriceCode,
        }).ToList();
    }

    /// <summary>
    /// 创建销售价格明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesPriceItemDto> CreateSalesPriceItemAsync(TaktSalesPriceItemCreateDto dto)
    {
        var entity = dto.Adapt<TaktSalesPriceItem>();
        entity.IsObsolete = 0;
        var isUnique_ix_takt_logistics_sales_price_item_seq_unique = await _uniqueValidator.IsUniqueAsync(
            _salesPriceItemRepository,
            x => x.SalesPriceId == entity.SalesPriceId
                && x.SalesPriceSeq == entity.SalesPriceSeq);
        if (!isUnique_ix_takt_logistics_sales_price_item_seq_unique)
        {
            throw new TaktBusinessException("销售价格明细的SalesPriceId、SalesPriceSeq已存在");
        }
        entity = await _salesPriceItemRepository.CreateAsync(entity);
                await SaveSalesPriceItemChildrenAsync(entity, dto);
        return await GetSalesPriceItemByIdAsync(entity.Id) ?? entity.Adapt<TaktSalesPriceItemDto>();
    }

    /// <summary>
    /// 更新销售价格明细
    /// </summary>
    /// <param name="id">销售价格明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesPriceItemDto> UpdateSalesPriceItemAsync(long id, TaktSalesPriceItemUpdateDto dto)
    {
        var entity = await _salesPriceItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("销售价格明细不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_sales_price_item_seq_unique = await _uniqueValidator.IsUniqueAsync(
            _salesPriceItemRepository,
            x => x.SalesPriceId == entity.SalesPriceId
                && x.SalesPriceSeq == entity.SalesPriceSeq,
            id);
        if (!isUnique_ix_takt_logistics_sales_price_item_seq_unique)
        {
            throw new TaktBusinessException("销售价格明细的SalesPriceId、SalesPriceSeq已存在");
        }
        await _salesPriceItemRepository.UpdateAsync(entity);
                await SaveSalesPriceItemChildrenAsync(entity, dto);
        return await GetSalesPriceItemByIdAsync(id) ?? throw new TaktBusinessException("销售价格明细不存在");
    }

    /// <summary>
    /// 删除销售价格明细
    /// </summary>
    /// <param name="id">销售价格明细ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSalesPriceItemByIdAsync(long id)
    {
        var entity = await _salesPriceItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("销售价格明细不存在或已删除");
        }
        await _salesPriceScaleQuantityRepository.DeleteAsync(x => x.SalesPriceItemId == entity.Id);
        await _salesPriceScaleValueRepository.DeleteAsync(x => x.SalesPriceItemId == entity.Id);
        var deleted = await _salesPriceItemRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("销售价格明细不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除销售价格明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSalesPriceItemBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSalesPriceItemByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新销售价格明细作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesPriceItemDto> UpdateSalesPriceItemObsoleteAsync(TaktSalesPriceItemObsoleteDto dto)
    {
        var entity = await _salesPriceItemRepository.GetByIdAsync(dto.SalesPriceItemId);
        if (entity == null)
        {
            throw new TaktBusinessException("销售价格明细不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("销售价格明细不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _salesPriceItemRepository.UpdateAsync(entity);
        return await GetSalesPriceItemByIdAsync(dto.SalesPriceItemId) ?? throw new TaktBusinessException("销售价格明细不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSalesPriceItemTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSalesPriceItemTemplateDto>(
            sheetName ?? "销售价格明细导入模板",
            fileName ?? "销售价格明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入销售价格明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSalesPriceItemAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSalesPriceItemImportDto>(fileStream, sheetName ?? "销售价格明细导入模板");
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
                var entity = rows[i].Adapt<TaktSalesPriceItem>();
                var importKey = $"{entity.SalesPriceId}|{entity.SalesPriceSeq}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（SalesPriceId、SalesPriceSeq）");
                }
                var isUnique_ix_takt_logistics_sales_price_item_seq_unique = await _uniqueValidator.IsUniqueAsync(
                    _salesPriceItemRepository,
                    x => x.SalesPriceId == entity.SalesPriceId
                        && x.SalesPriceSeq == entity.SalesPriceSeq);
                if (!isUnique_ix_takt_logistics_sales_price_item_seq_unique)
                {
                    throw new TaktBusinessException("销售价格明细的SalesPriceId、SalesPriceSeq已存在");
                }
                await _salesPriceItemRepository.CreateAsync(entity);
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
    /// 导出销售价格明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSalesPriceItemAsync(TaktSalesPriceItemQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktSalesPriceItemQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSalesPriceItemExportDto>(),
                sheetName ?? "销售价格明细数据",
                fileName ?? "销售价格明细导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _salesPriceItemRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSalesPriceItemExportDto>(),
                sheetName ?? "销售价格明细数据",
                fileName ?? "销售价格明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSalesPriceItemExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "销售价格明细数据",
            fileName ?? "销售价格明细导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 将指定主表下全部未作废销售价格数量等级标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="salesPriceItemId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkSalesPriceScaleQuantitysObsoleteAsync(long salesPriceItemId)
    {
        if (salesPriceItemId <= 0)
        {
            return;
        }
        var rows = await _salesPriceScaleQuantityRepository.GetListAsync(
            x => x.SalesPriceItemId == salesPriceItemId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _salesPriceScaleQuantityRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 将指定主表下全部未作废销售价格价值等级标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="salesPriceItemId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkSalesPriceScaleValuesObsoleteAsync(long salesPriceItemId)
    {
        if (salesPriceItemId <= 0)
        {
            return;
        }
        var rows = await _salesPriceScaleValueRepository.GetListAsync(
            x => x.SalesPriceItemId == salesPriceItemId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _salesPriceScaleValueRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 填充销售价格明细详情（加载 OneToMany 子表：销售价格数量等级、销售价格价值等级）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillSalesPriceItemDetailsAsync(TaktSalesPriceItemDto dto, TaktSalesPriceItem entity)
    {
        if (dto == null)
        {
            return;
        }
        // 销售价格数量等级 → dto.ScaleQuantities（含作废行）
        var scalequantities = await _salesPriceScaleQuantityRepository.GetListAsync(x => x.SalesPriceItemId == entity.Id);
        dto.ScaleQuantities = scalequantities.Adapt<List<TaktSalesPriceScaleQuantityDto>>();
        // 销售价格价值等级 → dto.ScaleValues（含作废行）
        var scalevalues = await _salesPriceScaleValueRepository.GetListAsync(x => x.SalesPriceItemId == entity.Id);
        dto.ScaleValues = scalevalues.Adapt<List<TaktSalesPriceScaleValueDto>>();
    }

    /// <summary>
    /// 保存销售价格明细子表级联（销售价格数量等级、销售价格价值等级；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveSalesPriceItemChildrenAsync(TaktSalesPriceItem entity, TaktSalesPriceItemCreateDto dto)
    {
        // 销售价格数量等级（ScaleQuantities）
        List<TaktSalesPriceScaleQuantityUpdateDto>? scaleQuantitiesForSave;
        if (dto is TaktSalesPriceItemUpdateDto updateDtoForScaleQuantities && updateDtoForScaleQuantities.ScaleQuantities != null)
        {
            scaleQuantitiesForSave = updateDtoForScaleQuantities.ScaleQuantities;
        }
        else if (dto.ScaleQuantities != null)
        {
            scaleQuantitiesForSave = dto.ScaleQuantities.Adapt<List<TaktSalesPriceScaleQuantityUpdateDto>>();
        }
        else
        {
            scaleQuantitiesForSave = null;
        }
        if (scaleQuantitiesForSave is not { Count: > 0 })
        {
            await MarkSalesPriceScaleQuantitysObsoleteAsync(entity.Id);
        }
        else
        {
            var existingList = await _salesPriceScaleQuantityRepository.GetListAsync(x => x.SalesPriceItemId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktSalesPriceScaleQuantity>();
            for (var i = 0; i < scaleQuantitiesForSave.Count; i++)
            {
                var childDto = scaleQuantitiesForSave[i];
                childDto.SalesPriceItemId = entity.Id;
                if (childDto.SalesPriceScaleQuantityId > 0)
                {
                    if (!existingById.TryGetValue(childDto.SalesPriceScaleQuantityId, out var target))
                    {
                        throw new TaktBusinessException("销售价格数量等级不存在（SalesPriceScaleQuantityId={childDto.SalesPriceScaleQuantityId}）");
                    }
                    if (target.SalesPriceItemId != entity.Id)
                    {
                        throw new TaktBusinessException("销售价格数量等级不属于当前主表（SalesPriceScaleQuantityId={childDto.SalesPriceScaleQuantityId}）");
                    }
                    submittedIds.Add(childDto.SalesPriceScaleQuantityId);
                    childDto.Adapt(target);
                    target.Id = childDto.SalesPriceScaleQuantityId;
                    target.SalesPriceItemId = entity.Id;
                    target.IsObsolete = 0;
                    await _salesPriceScaleQuantityRepository.UpdateAsync(target);
                }
                else
                {
                    var child = childDto.Adapt<TaktSalesPriceScaleQuantity>();
                    child.Id = 0;
                    child.SalesPriceItemId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _salesPriceScaleQuantityRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                await _salesPriceScaleQuantityRepository.CreateRangeAsync(toCreate);
            }
        }
        // 销售价格价值等级（ScaleValues）
        List<TaktSalesPriceScaleValueUpdateDto>? scaleValuesForSave;
        if (dto is TaktSalesPriceItemUpdateDto updateDtoForScaleValues && updateDtoForScaleValues.ScaleValues != null)
        {
            scaleValuesForSave = updateDtoForScaleValues.ScaleValues;
        }
        else if (dto.ScaleValues != null)
        {
            scaleValuesForSave = dto.ScaleValues.Adapt<List<TaktSalesPriceScaleValueUpdateDto>>();
        }
        else
        {
            scaleValuesForSave = null;
        }
        if (scaleValuesForSave is not { Count: > 0 })
        {
            await MarkSalesPriceScaleValuesObsoleteAsync(entity.Id);
        }
        else
        {
            var existingList = await _salesPriceScaleValueRepository.GetListAsync(x => x.SalesPriceItemId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktSalesPriceScaleValue>();
            for (var i = 0; i < scaleValuesForSave.Count; i++)
            {
                var childDto = scaleValuesForSave[i];
                childDto.SalesPriceItemId = entity.Id;
                if (childDto.SalesPriceScaleValueId > 0)
                {
                    if (!existingById.TryGetValue(childDto.SalesPriceScaleValueId, out var target))
                    {
                        throw new TaktBusinessException("销售价格价值等级不存在（SalesPriceScaleValueId={childDto.SalesPriceScaleValueId}）");
                    }
                    if (target.SalesPriceItemId != entity.Id)
                    {
                        throw new TaktBusinessException("销售价格价值等级不属于当前主表（SalesPriceScaleValueId={childDto.SalesPriceScaleValueId}）");
                    }
                    submittedIds.Add(childDto.SalesPriceScaleValueId);
                    childDto.Adapt(target);
                    target.Id = childDto.SalesPriceScaleValueId;
                    target.SalesPriceItemId = entity.Id;
                    target.IsObsolete = 0;
                    await _salesPriceScaleValueRepository.UpdateAsync(target);
                }
                else
                {
                    var child = childDto.Adapt<TaktSalesPriceScaleValue>();
                    child.Id = 0;
                    child.SalesPriceItemId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _salesPriceScaleValueRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                await _salesPriceScaleValueRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建销售价格明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSalesPriceItem, bool>> QueryExpression(TaktSalesPriceItemQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSalesPriceItem>();

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
                (x.SalesPriceCode != null && x.SalesPriceCode.Contains(keywords))
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

        if (queryDto?.SalesPriceId.HasValue == true)
        {
            var salesPriceId = queryDto.SalesPriceId;
            exp = exp.And(x => x.SalesPriceId == salesPriceId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SalesPriceCode))
        {
            var salesPriceCode = queryDto.SalesPriceCode;
            exp = exp.And(x => x.SalesPriceCode != null && x.SalesPriceCode.Contains(salesPriceCode));
        }

        if (queryDto?.SalesPriceSeq.HasValue == true)
        {
            var salesPriceSeq = queryDto.SalesPriceSeq;
            exp = exp.And(x => x.SalesPriceSeq == salesPriceSeq);
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
    private static bool HasAnyListQueryFilter(TaktSalesPriceItemQueryDto? queryDto)
    {
        if (queryDto == null)
        {
            return false;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.KeyWords))
        {
            return true;
        }
        if (queryDto.SalesPriceId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SalesPriceCode))
        {
            return true;
        }
        if (queryDto.SalesPriceSeq.HasValue)
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
