// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Materials
// 文件名称：TaktMaterialService.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Cursor AI)
// 功能描述：物料应用服务实现
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

namespace Takt.Application.Services.Logistics.Materials;

/// <summary>
/// 物料应用服务
/// </summary>
public class TaktMaterialService : TaktServiceBase, ITaktMaterialService
{
    private readonly ITaktCompanyRepository<TaktMaterial> _materialRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="materialRepository">物料仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktMaterialService(
        ITaktCompanyRepository<TaktMaterial> materialRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _materialRepository = materialRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取物料列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktMaterialDto>> GetMaterialListAsync(TaktMaterialQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _materialRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktMaterialDto>.Create(
            data.Adapt<List<TaktMaterialDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取物料
    /// </summary>
    /// <param name="id">物料ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaterialDto?> GetMaterialByIdAsync(long id)
    {
        var entity = await _materialRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktMaterialDto>();
    }

    /// <summary>
    /// 获取物料选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetMaterialOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _materialRepository.GetListAsync(
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
    /// 创建物料
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaterialDto> CreateMaterialAsync(TaktMaterialCreateDto dto)
    {
        var entity = dto.Adapt<TaktMaterial>();
        var isUnique_ix_takt_logistics_materials_material_unique = await _uniqueValidator.IsUniqueAsync(
            _materialRepository,
            x => x.PlantCode == entity.PlantCode
                && x.MaterialCode == entity.MaterialCode);
        if (!isUnique_ix_takt_logistics_materials_material_unique)
        {
            throw new TaktBusinessException("物料的PlantCode、MaterialCode已存在");
        }
        entity = await _materialRepository.CreateAsync(entity);
        return await GetMaterialByIdAsync(entity.Id) ?? entity.Adapt<TaktMaterialDto>();
    }

    /// <summary>
    /// 更新物料
    /// </summary>
    /// <param name="id">物料ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaterialDto> UpdateMaterialAsync(long id, TaktMaterialUpdateDto dto)
    {
        var entity = await _materialRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("物料不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_materials_material_unique = await _uniqueValidator.IsUniqueAsync(
            _materialRepository,
            x => x.PlantCode == entity.PlantCode
                && x.MaterialCode == entity.MaterialCode,
            id);
        if (!isUnique_ix_takt_logistics_materials_material_unique)
        {
            throw new TaktBusinessException("物料的PlantCode、MaterialCode已存在");
        }
        await _materialRepository.UpdateAsync(entity);
        return await GetMaterialByIdAsync(id) ?? throw new TaktBusinessException("物料不存在");
    }

    /// <summary>
    /// 删除物料
    /// </summary>
    /// <param name="id">物料ID</param>
    /// <returns>任务</returns>
    public async Task DeleteMaterialByIdAsync(long id)
    {
        var deleted = await _materialRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("物料不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除物料
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteMaterialBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteMaterialByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新物料状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaterialDto> UpdateMaterialStatusAsync(TaktMaterialStatusDto dto)
    {
        var entity = await _materialRepository.GetByIdAsync(dto.MaterialId);
        if (entity == null)
        {
            throw new TaktBusinessException("物料不存在");
        }
        entity.MaterialStatus = dto.MaterialStatus;
        await _materialRepository.UpdateAsync(entity);
        return await GetMaterialByIdAsync(dto.MaterialId) ?? throw new TaktBusinessException("物料不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetMaterialTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktMaterialTemplateDto>(
            sheetName ?? "物料导入模板",
            fileName ?? "物料导入模板.xlsx");
    }

    /// <summary>
    /// 导入物料
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportMaterialAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktMaterialImportDto>(fileStream, sheetName ?? "物料导入模板");
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
                var entity = rows[i].Adapt<TaktMaterial>();
                var importKey = $"{entity.PlantCode}|{entity.MaterialCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、MaterialCode）");
                }
                var isUnique_ix_takt_logistics_materials_material_unique = await _uniqueValidator.IsUniqueAsync(
                    _materialRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.MaterialCode == entity.MaterialCode);
                if (!isUnique_ix_takt_logistics_materials_material_unique)
                {
                    throw new TaktBusinessException("物料的PlantCode、MaterialCode已存在");
                }
                await _materialRepository.CreateAsync(entity);
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
    /// 导出物料
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportMaterialAsync(TaktMaterialQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktMaterialQueryDto());
        var list = await _materialRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktMaterialExportDto>(),
                sheetName ?? "物料数据",
                fileName ?? "物料导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktMaterialExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "物料数据",
            fileName ?? "物料导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建物料查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktMaterial, bool>> QueryExpression(TaktMaterialQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktMaterial>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || (x.MaterialName != null && x.MaterialName.Contains(keywords))
                || (x.MaterialSpecification != null && x.MaterialSpecification.Contains(keywords))
                || (x.MaterialDescription != null && x.MaterialDescription.Contains(keywords))
                || (x.IndustrySector != null && x.IndustrySector.Contains(keywords))
                || (x.MaterialHierarchy != null && x.MaterialHierarchy.Contains(keywords))
                || (x.MaterialGroupCode != null && x.MaterialGroupCode.Contains(keywords))
                || SqlFunc.ToString(x.MaterialType).Contains(keywords)
                || (x.MaterialModel != null && x.MaterialModel.Contains(keywords))
                || (x.MaterialBrand != null && x.MaterialBrand.Contains(keywords))
                || (x.BaseUnit != null && x.BaseUnit.Contains(keywords))
                || (x.PurchaseGroup != null && x.PurchaseGroup.Contains(keywords))
                || SqlFunc.ToString(x.PurchaseType).Contains(keywords)
                || SqlFunc.ToString(x.SpecialProcurement).Contains(keywords)
                || SqlFunc.ToString(x.IsBulk).Contains(keywords)
                || SqlFunc.ToString(x.MinOrderQuantity).Contains(keywords)
                || SqlFunc.ToString(x.RoundingValue).Contains(keywords)
                || SqlFunc.ToString(x.PlannedDeliveryTimeDays).Contains(keywords)
                || SqlFunc.ToString(x.InHouseProductionDays).Contains(keywords)
                || (x.Manufacturer != null && x.Manufacturer.Contains(keywords))
                || (x.ManufacturerPartNumber != null && x.ManufacturerPartNumber.Contains(keywords))
                || (x.CurrencyCode != null && x.CurrencyCode.Contains(keywords))
                || SqlFunc.ToString(x.PriceControl).Contains(keywords)
                || SqlFunc.ToString(x.PriceUnit).Contains(keywords)
                || (x.ValuationCategory != null && x.ValuationCategory.Contains(keywords))
                || (x.DifferenceCode != null && x.DifferenceCode.Contains(keywords))
                || (x.ProfitCenter != null && x.ProfitCenter.Contains(keywords))
                || SqlFunc.ToString(x.LatestPurchasePrice).Contains(keywords)
                || SqlFunc.ToString(x.SalesPrice).Contains(keywords)
                || SqlFunc.ToString(x.SafetyStock).Contains(keywords)
                || SqlFunc.ToString(x.MaxStock).Contains(keywords)
                || SqlFunc.ToString(x.MinStock).Contains(keywords)
                || SqlFunc.ToString(x.CurrentStock).Contains(keywords)
                || (x.ProductionLocation != null && x.ProductionLocation.Contains(keywords))
                || (x.PurchasingLocation != null && x.PurchasingLocation.Contains(keywords))
                || SqlFunc.ToString(x.InspectionRequired).Contains(keywords)
                || SqlFunc.ToString(x.IsBatch).Contains(keywords)
                || SqlFunc.ToString(x.IsExpiry).Contains(keywords)
                || SqlFunc.ToString(x.ExpiryDays).Contains(keywords)
                || SqlFunc.ToString(x.MaterialStatus).Contains(keywords)
                || (x.MaterialAttributes != null && x.MaterialAttributes.Contains(keywords))
                || (x.IsEndOfLife != null && x.IsEndOfLife.Contains(keywords))
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.EndOfLifeDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
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

        if (!string.IsNullOrEmpty(queryDto?.MaterialDescription))
        {
            exp = exp.And(x => x.MaterialDescription != null && x.MaterialDescription.Contains(queryDto.MaterialDescription));
        }

        if (!string.IsNullOrEmpty(queryDto?.IndustrySector))
        {
            exp = exp.And(x => x.IndustrySector != null && x.IndustrySector.Contains(queryDto.IndustrySector));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialHierarchy))
        {
            exp = exp.And(x => x.MaterialHierarchy != null && x.MaterialHierarchy.Contains(queryDto.MaterialHierarchy));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialGroupCode))
        {
            exp = exp.And(x => x.MaterialGroupCode != null && x.MaterialGroupCode.Contains(queryDto.MaterialGroupCode));
        }

        if (queryDto?.MaterialType.HasValue == true)
        {
            exp = exp.And(x => x.MaterialType == queryDto.MaterialType);
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialModel))
        {
            exp = exp.And(x => x.MaterialModel != null && x.MaterialModel.Contains(queryDto.MaterialModel));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialBrand))
        {
            exp = exp.And(x => x.MaterialBrand != null && x.MaterialBrand.Contains(queryDto.MaterialBrand));
        }

        if (!string.IsNullOrEmpty(queryDto?.BaseUnit))
        {
            exp = exp.And(x => x.BaseUnit != null && x.BaseUnit.Contains(queryDto.BaseUnit));
        }

        if (!string.IsNullOrEmpty(queryDto?.PurchaseGroup))
        {
            exp = exp.And(x => x.PurchaseGroup != null && x.PurchaseGroup.Contains(queryDto.PurchaseGroup));
        }

        if (queryDto?.PurchaseType.HasValue == true)
        {
            exp = exp.And(x => x.PurchaseType == queryDto.PurchaseType);
        }

        if (queryDto?.SpecialProcurement.HasValue == true)
        {
            exp = exp.And(x => x.SpecialProcurement == queryDto.SpecialProcurement);
        }

        if (queryDto?.IsBulk.HasValue == true)
        {
            exp = exp.And(x => x.IsBulk == queryDto.IsBulk);
        }

        if (queryDto?.MinOrderQuantity.HasValue == true)
        {
            exp = exp.And(x => x.MinOrderQuantity == queryDto.MinOrderQuantity);
        }

        if (queryDto?.RoundingValue.HasValue == true)
        {
            exp = exp.And(x => x.RoundingValue == queryDto.RoundingValue);
        }

        if (queryDto?.PlannedDeliveryTimeDays.HasValue == true)
        {
            exp = exp.And(x => x.PlannedDeliveryTimeDays == queryDto.PlannedDeliveryTimeDays);
        }

        if (queryDto?.InHouseProductionDays.HasValue == true)
        {
            exp = exp.And(x => x.InHouseProductionDays == queryDto.InHouseProductionDays);
        }

        if (!string.IsNullOrEmpty(queryDto?.Manufacturer))
        {
            exp = exp.And(x => x.Manufacturer != null && x.Manufacturer.Contains(queryDto.Manufacturer));
        }

        if (!string.IsNullOrEmpty(queryDto?.ManufacturerPartNumber))
        {
            exp = exp.And(x => x.ManufacturerPartNumber != null && x.ManufacturerPartNumber.Contains(queryDto.ManufacturerPartNumber));
        }

        if (!string.IsNullOrEmpty(queryDto?.CurrencyCode))
        {
            exp = exp.And(x => x.CurrencyCode != null && x.CurrencyCode.Contains(queryDto.CurrencyCode));
        }

        if (queryDto?.PriceControl.HasValue == true)
        {
            exp = exp.And(x => x.PriceControl == queryDto.PriceControl);
        }

        if (queryDto?.PriceUnit.HasValue == true)
        {
            exp = exp.And(x => x.PriceUnit == queryDto.PriceUnit);
        }

        if (!string.IsNullOrEmpty(queryDto?.ValuationCategory))
        {
            exp = exp.And(x => x.ValuationCategory != null && x.ValuationCategory.Contains(queryDto.ValuationCategory));
        }

        if (!string.IsNullOrEmpty(queryDto?.DifferenceCode))
        {
            exp = exp.And(x => x.DifferenceCode != null && x.DifferenceCode.Contains(queryDto.DifferenceCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ProfitCenter))
        {
            exp = exp.And(x => x.ProfitCenter != null && x.ProfitCenter.Contains(queryDto.ProfitCenter));
        }

        if (queryDto?.LatestPurchasePrice.HasValue == true)
        {
            exp = exp.And(x => x.LatestPurchasePrice == queryDto.LatestPurchasePrice);
        }

        if (queryDto?.SalesPrice.HasValue == true)
        {
            exp = exp.And(x => x.SalesPrice == queryDto.SalesPrice);
        }

        if (queryDto?.SafetyStock.HasValue == true)
        {
            exp = exp.And(x => x.SafetyStock == queryDto.SafetyStock);
        }

        if (queryDto?.MaxStock.HasValue == true)
        {
            exp = exp.And(x => x.MaxStock == queryDto.MaxStock);
        }

        if (queryDto?.MinStock.HasValue == true)
        {
            exp = exp.And(x => x.MinStock == queryDto.MinStock);
        }

        if (queryDto?.CurrentStock.HasValue == true)
        {
            exp = exp.And(x => x.CurrentStock == queryDto.CurrentStock);
        }

        if (!string.IsNullOrEmpty(queryDto?.ProductionLocation))
        {
            exp = exp.And(x => x.ProductionLocation != null && x.ProductionLocation.Contains(queryDto.ProductionLocation));
        }

        if (!string.IsNullOrEmpty(queryDto?.PurchasingLocation))
        {
            exp = exp.And(x => x.PurchasingLocation != null && x.PurchasingLocation.Contains(queryDto.PurchasingLocation));
        }

        if (queryDto?.InspectionRequired.HasValue == true)
        {
            exp = exp.And(x => x.InspectionRequired == queryDto.InspectionRequired);
        }

        if (queryDto?.IsBatch.HasValue == true)
        {
            exp = exp.And(x => x.IsBatch == queryDto.IsBatch);
        }

        if (queryDto?.IsExpiry.HasValue == true)
        {
            exp = exp.And(x => x.IsExpiry == queryDto.IsExpiry);
        }

        if (queryDto?.ExpiryDays.HasValue == true)
        {
            exp = exp.And(x => x.ExpiryDays == queryDto.ExpiryDays);
        }

        if (queryDto?.MaterialStatus.HasValue == true)
        {
            exp = exp.And(x => x.MaterialStatus == queryDto.MaterialStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialAttributes))
        {
            exp = exp.And(x => x.MaterialAttributes != null && x.MaterialAttributes.Contains(queryDto.MaterialAttributes));
        }

        if (!string.IsNullOrEmpty(queryDto?.IsEndOfLife))
        {
            exp = exp.And(x => x.IsEndOfLife != null && x.IsEndOfLife.Contains(queryDto.IsEndOfLife));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.EndOfLifeDateStart.HasValue == true)
        {
            exp = exp.And(x => x.EndOfLifeDate >= queryDto.EndOfLifeDateStart);
        }

        if (queryDto?.EndOfLifeDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.EndOfLifeDate <= queryDto.EndOfLifeDateEnd);
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
