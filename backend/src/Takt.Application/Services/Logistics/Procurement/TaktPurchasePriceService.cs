// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Procurement
// 文件名称：TaktPurchasePriceService.cs
// 创建时间：2026-07-21
// 创建人：Takt365(Cursor AI)
// 功能描述：采购价格应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Procurement;
using Takt.Domain.Entities.Logistics.Materials;
using Takt.Domain.Entities.Logistics.Procurement;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Procurement;

/// <summary>
/// 采购价格应用服务
/// </summary>
public class TaktPurchasePriceService : TaktServiceBase, ITaktPurchasePriceService
{
    /// <summary>物料/供应商名称按编码分批查询，避免超长 IN 列表</summary>
    private const int MaterialNameLookupBatchSize = 500;

    private readonly ITaktCompanyRepository<TaktPurchasePrice> _purchasePriceRepository;
    private readonly ITaktCompanyRepository<TaktPurchasePriceItem> _purchasePriceItemRepository;
    private readonly ITaktCompanyRepository<TaktMaterialPlant> _materialPlantRepository;
    private readonly ITaktCompanyRepository<TaktSupplier> _supplierRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="purchasePriceRepository">采购价格仓储</param>
    /// <param name="purchasePriceItemRepository">PurchasePriceItem仓储</param>
    /// <param name="materialPlantRepository">工厂物料仓储</param>
    /// <param name="supplierRepository">供应商仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPurchasePriceService(
        ITaktCompanyRepository<TaktPurchasePrice> purchasePriceRepository,
        ITaktCompanyRepository<TaktPurchasePriceItem> purchasePriceItemRepository,
        ITaktCompanyRepository<TaktMaterialPlant> materialPlantRepository,
        ITaktCompanyRepository<TaktSupplier> supplierRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _purchasePriceRepository = purchasePriceRepository;
        _purchasePriceItemRepository = purchasePriceItemRepository;
        _materialPlantRepository = materialPlantRepository;
        _supplierRepository = supplierRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取采购价格列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktPurchasePriceDto>> GetPurchasePriceListAsync(TaktPurchasePriceQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _purchasePriceRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktPurchasePriceDto>.Create(
            data.Adapt<List<TaktPurchasePriceDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取采购价格
    /// </summary>
    /// <param name="id">采购价格ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchasePriceDto?> GetPurchasePriceByIdAsync(long id)
    {
        var entity = await _purchasePriceRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktPurchasePriceDto>();
        await FillPurchasePriceDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取采购价格选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetPurchasePriceOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _purchasePriceRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.PurchasePriceCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.PurchasePriceCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <inheritdoc />
    public async Task<List<TaktSelectOption>> GetPurchasePriceSupplierOptionsAsync(string? plantCode = null)
    {
        EnsureThreeLayerContext();
        var trimmedPlant = plantCode?.Trim();
        if (string.IsNullOrWhiteSpace(trimmedPlant))
        {
            return new List<TaktSelectOption>();
        }
        var list = await _purchasePriceRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode == trimmedPlant
                && x.SupplierCode != null
                && x.SupplierCode != string.Empty,
            x => x.SupplierCode,
            false);
        var codes = list
            .Select(x => x.SupplierCode.Trim())
            .Where(c => !string.IsNullOrWhiteSpace(c))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(c => c, StringComparer.OrdinalIgnoreCase)
            .ToList();
        var nameLookup = await LoadSupplierNameLookupAsync(codes);
        return codes.Select(code =>
        {
            var name = nameLookup.TryGetValue(code, out var n) ? n : string.Empty;
            var label = string.IsNullOrWhiteSpace(name) ? code : $"{code} - {name}";
            return new TaktSelectOption
            {
                DictValue = code,
                DictLabel = label,
            };
        }).ToList();
    }

    /// <inheritdoc />
    public async Task<List<TaktSelectOption>> GetPurchasePriceMaterialOptionsAsync(string? plantCode = null)
    {
        EnsureThreeLayerContext();
        var trimmedPlant = plantCode?.Trim();
        if (string.IsNullOrWhiteSpace(trimmedPlant))
        {
            return new List<TaktSelectOption>();
        }
        var list = await _purchasePriceRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode == trimmedPlant
                && x.MaterialCode != null
                && x.MaterialCode != string.Empty,
            x => x.MaterialCode,
            false);
        var codes = list
            .Select(x => x.MaterialCode.Trim())
            .Where(c => !string.IsNullOrWhiteSpace(c))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(c => c, StringComparer.OrdinalIgnoreCase)
            .ToList();
        var nameLookup = await LoadMaterialNameLookupAsync(trimmedPlant, codes);
        return codes.Select(code =>
        {
            var name = nameLookup.TryGetValue(code, out var n) ? n : string.Empty;
            var label = string.IsNullOrWhiteSpace(name) ? code : $"{code} - {name}";
            return new TaktSelectOption
            {
                DictValue = code,
                DictLabel = label,
            };
        }).ToList();
    }

    /// <summary>
    /// 创建采购价格
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchasePriceDto> CreatePurchasePriceAsync(TaktPurchasePriceCreateDto dto)
    {
        var entity = dto.Adapt<TaktPurchasePrice>();
        var isUnique_ix_takt_logistics_procurement_purchase_price_code_unique = await _uniqueValidator.IsUniqueAsync(
            _purchasePriceRepository,
            x => x.PlantCode == entity.PlantCode
                && x.PurchasePriceCode == entity.PurchasePriceCode);
        if (!isUnique_ix_takt_logistics_procurement_purchase_price_code_unique)
        {
            throw new TaktBusinessException("采购价格的PlantCode、PurchasePriceCode已存在");
        }
        entity = await _purchasePriceRepository.CreateAsync(entity);
                await SavePurchasePriceChildrenAsync(entity, dto);
        return await GetPurchasePriceByIdAsync(entity.Id) ?? entity.Adapt<TaktPurchasePriceDto>();
    }

    /// <summary>
    /// 更新采购价格
    /// </summary>
    /// <param name="id">采购价格ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchasePriceDto> UpdatePurchasePriceAsync(long id, TaktPurchasePriceUpdateDto dto)
    {
        var entity = await _purchasePriceRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("采购价格不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_procurement_purchase_price_code_unique = await _uniqueValidator.IsUniqueAsync(
            _purchasePriceRepository,
            x => x.PlantCode == entity.PlantCode
                && x.PurchasePriceCode == entity.PurchasePriceCode,
            id);
        if (!isUnique_ix_takt_logistics_procurement_purchase_price_code_unique)
        {
            throw new TaktBusinessException("采购价格的PlantCode、PurchasePriceCode已存在");
        }
        await _purchasePriceRepository.UpdateAsync(entity);
                await SavePurchasePriceChildrenAsync(entity, dto);
        return await GetPurchasePriceByIdAsync(id) ?? throw new TaktBusinessException("采购价格不存在");
    }

    /// <summary>
    /// 删除采购价格
    /// </summary>
    /// <param name="id">采购价格ID</param>
    /// <returns>任务</returns>
    public async Task DeletePurchasePriceByIdAsync(long id)
    {
        var entity = await _purchasePriceRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("采购价格不存在或已删除");
        }
        await _purchasePriceItemRepository.DeleteAsync(x => x.PurchasePriceId == entity.Id);
        var deleted = await _purchasePriceRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("采购价格不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除采购价格
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeletePurchasePriceBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeletePurchasePriceByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetPurchasePriceTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktPurchasePriceTemplateDto>(
            sheetName ?? "采购价格导入模板",
            fileName ?? "采购价格导入模板.xlsx");
    }

    /// <summary>
    /// 导入采购价格
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportPurchasePriceAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktPurchasePriceImportDto>(fileStream, sheetName ?? "采购价格导入模板");
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
                var entity = rows[i].Adapt<TaktPurchasePrice>();
                var importKey = $"{entity.PlantCode}|{entity.PurchasePriceCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、PurchasePriceCode）");
                }
                var isUnique_ix_takt_logistics_procurement_purchase_price_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _purchasePriceRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.PurchasePriceCode == entity.PurchasePriceCode);
                if (!isUnique_ix_takt_logistics_procurement_purchase_price_code_unique)
                {
                    throw new TaktBusinessException("采购价格的PlantCode、PurchasePriceCode已存在");
                }
                await _purchasePriceRepository.CreateAsync(entity);
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
    /// 导出采购价格
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportPurchasePriceAsync(TaktPurchasePriceQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktPurchasePriceQueryDto());
        var list = await _purchasePriceRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPurchasePriceExportDto>(),
                sheetName ?? "采购价格数据",
                fileName ?? "采购价格导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktPurchasePriceExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "采购价格数据",
            fileName ?? "采购价格导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 将指定主表下全部未作废采购价格明细标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="purchasePriceId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkPurchasePriceItemsObsoleteAsync(long purchasePriceId)
    {
        if (purchasePriceId <= 0)
        {
            return;
        }
        var rows = await _purchasePriceItemRepository.GetListAsync(
            x => x.PurchasePriceId == purchasePriceId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _purchasePriceItemRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 填充采购价格详情（加载 OneToMany 子表：采购价格明细）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillPurchasePriceDetailsAsync(TaktPurchasePriceDto dto, TaktPurchasePrice entity)
    {
        if (dto == null)
        {
            return;
        }
        // 采购价格明细 → dto.Items（含作废行）
        var items = await _purchasePriceItemRepository.GetListAsync(x => x.PurchasePriceId == entity.Id);
        dto.Items = items.Adapt<List<TaktPurchasePriceItemDto>>();
    }

    /// <summary>
    /// 保存采购价格子表级联（采购价格明细；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SavePurchasePriceChildrenAsync(TaktPurchasePrice entity, TaktPurchasePriceCreateDto dto)
    {
        // 采购价格明细（Items）
        List<TaktPurchasePriceItemUpdateDto>? itemsForSave;
        if (dto is TaktPurchasePriceUpdateDto updateDtoForItems && updateDtoForItems.Items != null)
        {
            itemsForSave = updateDtoForItems.Items;
        }
        else if (dto.Items != null)
        {
            itemsForSave = dto.Items.Adapt<List<TaktPurchasePriceItemUpdateDto>>();
        }
        else
        {
            itemsForSave = null;
        }
        if (itemsForSave is not { Count: > 0 })
        {
            await MarkPurchasePriceItemsObsoleteAsync(entity.Id);
            return;
        }
        else
        {
            var existingList = await _purchasePriceItemRepository.GetListAsync(x => x.PurchasePriceId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktPurchasePriceItem>();
            for (var i = 0; i < itemsForSave.Count; i++)
            {
                var childDto = itemsForSave[i];
                childDto.PurchasePriceId = entity.Id;
                if (childDto.PurchasePriceItemId > 0)
                {
                    if (!existingById.TryGetValue(childDto.PurchasePriceItemId, out var target))
                    {
                        throw new TaktBusinessException("采购价格明细不存在（PurchasePriceItemId={childDto.PurchasePriceItemId}）");
                    }
                    if (target.PurchasePriceId != entity.Id)
                    {
                        throw new TaktBusinessException("采购价格明细不属于当前主表（PurchasePriceItemId={childDto.PurchasePriceItemId}）");
                    }
                    submittedIds.Add(childDto.PurchasePriceItemId);
                    childDto.Adapt(target);
                    target.Id = childDto.PurchasePriceItemId;
                    target.PurchasePriceId = entity.Id;
                    target.IsObsolete = 0;
                    await _purchasePriceItemRepository.UpdateAsync(target);
                }
                else
                {
                    var child = childDto.Adapt<TaktPurchasePriceItem>();
                    child.Id = 0;
                    child.PurchasePriceId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _purchasePriceItemRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                await _purchasePriceItemRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建采购价格查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktPurchasePrice, bool>> QueryExpression(TaktPurchasePriceQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktPurchasePrice>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.PurchasePriceCode != null && x.PurchasePriceCode.Contains(keywords))
                || (x.PriceType != null && x.PriceType.Contains(keywords))
                || (x.SupplierCode != null && x.SupplierCode.Contains(keywords))
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || (x.PurchaseGroup != null && x.PurchaseGroup.Contains(keywords))
                || (x.TaxCode != null && x.TaxCode.Contains(keywords))
                || SqlFunc.ToString(x.GrBasedInvoiceInspection).Contains(keywords)
                || SqlFunc.ToString(x.PricingDateControl).Contains(keywords)
                || SqlFunc.ToString(x.PurchaseInquiryId).Contains(keywords)
                || (x.PurchaseInquiryCode != null && x.PurchaseInquiryCode.Contains(keywords))
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

        if (!string.IsNullOrEmpty(queryDto?.PurchasePriceCode))
        {
            exp = exp.And(x => x.PurchasePriceCode != null && x.PurchasePriceCode.Contains(queryDto.PurchasePriceCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.PriceType))
        {
            exp = exp.And(x => x.PriceType != null && x.PriceType.Contains(queryDto.PriceType));
        }

        if (!string.IsNullOrEmpty(queryDto?.SupplierCode))
        {
            exp = exp.And(x => x.SupplierCode != null && x.SupplierCode.Contains(queryDto.SupplierCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialCode))
        {
            exp = exp.And(x => x.MaterialCode != null && x.MaterialCode.Contains(queryDto.MaterialCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.PurchaseGroup))
        {
            exp = exp.And(x => x.PurchaseGroup != null && x.PurchaseGroup.Contains(queryDto.PurchaseGroup));
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

        if (queryDto?.PurchaseInquiryId.HasValue == true)
        {
            exp = exp.And(x => x.PurchaseInquiryId == queryDto.PurchaseInquiryId);
        }

        if (!string.IsNullOrEmpty(queryDto?.PurchaseInquiryCode))
        {
            exp = exp.And(x => x.PurchaseInquiryCode != null && x.PurchaseInquiryCode.Contains(queryDto.PurchaseInquiryCode));
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

    /// <summary>
    /// 加载工厂物料名称字典
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="materialCodes">物料编码</param>
    /// <returns>编码→名称</returns>
    private async Task<Dictionary<string, string>> LoadMaterialNameLookupAsync(
        string plantCode,
        IReadOnlyList<string> materialCodes)
    {
        if (materialCodes.Count == 0)
        {
            return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }
        var codes = materialCodes.Where(c => !string.IsNullOrWhiteSpace(c)).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
        var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        for (var offset = 0; offset < codes.Count; offset = checked(offset + MaterialNameLookupBatchSize))
        {
            var batch = codes.Skip(offset).Take(MaterialNameLookupBatchSize).ToList();
            var plants = await _materialPlantRepository.GetListAsync(
                x => x.TenantCode == CurrentTenantCode
                    && x.CompanyCode == CurrentCompanyCode
                    && x.PlantCode == plantCode
                    && batch.Contains(x.MaterialCode));
            foreach (var group in plants
                .Where(p => !string.IsNullOrWhiteSpace(p.MaterialCode))
                .GroupBy(p => p.MaterialCode.Trim(), StringComparer.OrdinalIgnoreCase))
            {
                if (map.ContainsKey(group.Key))
                {
                    continue;
                }
                map[group.Key] = group.Select(x => x.MaterialName)
                    .FirstOrDefault(n => !string.IsNullOrWhiteSpace(n))?.Trim() ?? string.Empty;
            }
        }
        return map;
    }

    /// <summary>
    /// 加载供应商名称字典
    /// </summary>
    /// <param name="supplierCodes">供应商编码</param>
    /// <returns>编码→名称</returns>
    private async Task<Dictionary<string, string>> LoadSupplierNameLookupAsync(IReadOnlyList<string> supplierCodes)
    {
        if (supplierCodes.Count == 0)
        {
            return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }
        var codes = supplierCodes.Where(c => !string.IsNullOrWhiteSpace(c)).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
        var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        for (var offset = 0; offset < codes.Count; offset = checked(offset + MaterialNameLookupBatchSize))
        {
            var batch = codes.Skip(offset).Take(MaterialNameLookupBatchSize).ToList();
            var suppliers = await _supplierRepository.GetListAsync(
                x => x.TenantCode == CurrentTenantCode
                    && x.CompanyCode == CurrentCompanyCode
                    && batch.Contains(x.SupplierCode));
            foreach (var group in suppliers
                .Where(s => !string.IsNullOrWhiteSpace(s.SupplierCode))
                .GroupBy(s => s.SupplierCode.Trim(), StringComparer.OrdinalIgnoreCase))
            {
                if (map.ContainsKey(group.Key))
                {
                    continue;
                }
                map[group.Key] = group.Select(x => x.SupplierName1)
                    .FirstOrDefault(n => !string.IsNullOrWhiteSpace(n))?.Trim() ?? string.Empty;
            }
        }
        return map;
    }
}
