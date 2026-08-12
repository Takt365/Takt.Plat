// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Materials
// 文件名称：TaktGeneralMaterialService.cs
// 创建时间：2026-08-05
// 创建人：Takt365(Cursor AI)
// 功能描述：全局物料应用服务实现
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
/// 全局物料应用服务
/// </summary>
public class TaktGeneralMaterialService : TaktServiceBase, ITaktGeneralMaterialService
{
    private readonly ITaktTenantRepository<TaktGeneralMaterial> _generalMaterialRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="generalMaterialRepository">全局物料仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktGeneralMaterialService(
        ITaktTenantRepository<TaktGeneralMaterial> generalMaterialRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _generalMaterialRepository = generalMaterialRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取全局物料列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktGeneralMaterialDto>> GetGeneralMaterialListAsync(TaktGeneralMaterialQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _generalMaterialRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktGeneralMaterialDto>.Create(
            data.Adapt<List<TaktGeneralMaterialDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取全局物料
    /// </summary>
    /// <param name="id">全局物料ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktGeneralMaterialDto?> GetGeneralMaterialByIdAsync(long id)
    {
        var entity = await _generalMaterialRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode)
        {
            return null;
        }
        return entity.Adapt<TaktGeneralMaterialDto>();
    }

    /// <summary>
    /// 获取全局物料选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetGeneralMaterialOptionsAsync()
    {
        var list = await _generalMaterialRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode,
            x => x.MaterialCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.MaterialCode,
            DictLabel = e.MaterialCode,
        }).ToList();
    }

    /// <summary>
    /// 创建全局物料
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktGeneralMaterialDto> CreateGeneralMaterialAsync(TaktGeneralMaterialCreateDto dto)
    {
        var entity = dto.Adapt<TaktGeneralMaterial>();
        var isUnique_ix_takt_logistics_materials_general_material_unique = await _uniqueValidator.IsUniqueAsync(
            _generalMaterialRepository,
            x => x.MaterialCode == entity.MaterialCode);
        if (!isUnique_ix_takt_logistics_materials_general_material_unique)
        {
            throw new TaktBusinessException("全局物料的MaterialCode已存在");
        }
        entity = await _generalMaterialRepository.CreateAsync(entity);
        return await GetGeneralMaterialByIdAsync(entity.Id) ?? entity.Adapt<TaktGeneralMaterialDto>();
    }

    /// <summary>
    /// 更新全局物料
    /// </summary>
    /// <param name="id">全局物料ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktGeneralMaterialDto> UpdateGeneralMaterialAsync(long id, TaktGeneralMaterialUpdateDto dto)
    {
        var entity = await _generalMaterialRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("全局物料不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_materials_general_material_unique = await _uniqueValidator.IsUniqueAsync(
            _generalMaterialRepository,
            x => x.MaterialCode == entity.MaterialCode,
            id);
        if (!isUnique_ix_takt_logistics_materials_general_material_unique)
        {
            throw new TaktBusinessException("全局物料的MaterialCode已存在");
        }
        await _generalMaterialRepository.UpdateAsync(entity);
        return await GetGeneralMaterialByIdAsync(id) ?? throw new TaktBusinessException("全局物料不存在");
    }

    /// <summary>
    /// 删除全局物料
    /// </summary>
    /// <param name="id">全局物料ID</param>
    /// <returns>任务</returns>
    public async Task DeleteGeneralMaterialByIdAsync(long id)
    {
        var deleted = await _generalMaterialRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("全局物料不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除全局物料
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteGeneralMaterialBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteGeneralMaterialByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新全局物料状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktGeneralMaterialDto> UpdateGeneralMaterialStatusAsync(TaktGeneralMaterialStatusDto dto)
    {
        var entity = await _generalMaterialRepository.GetByIdAsync(dto.GeneralMaterialId);
        if (entity == null)
        {
            throw new TaktBusinessException("全局物料不存在");
        }
        entity.CompleteMaintenanceStatus = dto.CompleteMaintenanceStatus;
        await _generalMaterialRepository.UpdateAsync(entity);
        return await GetGeneralMaterialByIdAsync(dto.GeneralMaterialId) ?? throw new TaktBusinessException("全局物料不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetGeneralMaterialTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktGeneralMaterialTemplateDto>(
            sheetName ?? "全局物料导入模板",
            fileName ?? "全局物料导入模板.xlsx");
    }

    /// <summary>
    /// 导入全局物料
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportGeneralMaterialAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktGeneralMaterialImportDto>(fileStream, sheetName ?? "全局物料导入模板");
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
                var entity = rows[i].Adapt<TaktGeneralMaterial>();
                var importKey = $"{entity.MaterialCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（MaterialCode）");
                }
                var isUnique_ix_takt_logistics_materials_general_material_unique = await _uniqueValidator.IsUniqueAsync(
                    _generalMaterialRepository,
                    x => x.MaterialCode == entity.MaterialCode);
                if (!isUnique_ix_takt_logistics_materials_general_material_unique)
                {
                    throw new TaktBusinessException("全局物料的MaterialCode已存在");
                }
                await _generalMaterialRepository.CreateAsync(entity);
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
    /// 导出全局物料
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportGeneralMaterialAsync(TaktGeneralMaterialQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktGeneralMaterialQueryDto());
        var list = await _generalMaterialRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktGeneralMaterialExportDto>(),
                sheetName ?? "全局物料数据",
                fileName ?? "全局物料导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktGeneralMaterialExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "全局物料数据",
            fileName ?? "全局物料导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建全局物料查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktGeneralMaterial, bool>> QueryExpression(TaktGeneralMaterialQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktGeneralMaterial>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || (x.CompleteMaintenanceStatus != null && x.CompleteMaintenanceStatus.Contains(keywords))
                || (x.MaintenanceStatus != null && x.MaintenanceStatus.Contains(keywords))
                || (x.ClientDeletionFlag != null && x.ClientDeletionFlag.Contains(keywords))
                || (x.MaterialType != null && x.MaterialType.Contains(keywords))
                || (x.IndustrySector != null && x.IndustrySector.Contains(keywords))
                || (x.MaterialGroup != null && x.MaterialGroup.Contains(keywords))
                || (x.OldMaterialNumber != null && x.OldMaterialNumber.Contains(keywords))
                || (x.BaseUnit != null && x.BaseUnit.Contains(keywords))
                || (x.OrderUnit != null && x.OrderUnit.Contains(keywords))
                || (x.DocumentNumber != null && x.DocumentNumber.Contains(keywords))
                || (x.DocumentType != null && x.DocumentType.Contains(keywords))
                || (x.DocumentVersion != null && x.DocumentVersion.Contains(keywords))
                || (x.DocumentPageFormat != null && x.DocumentPageFormat.Contains(keywords))
                || (x.DocumentChangeNumber != null && x.DocumentChangeNumber.Contains(keywords))
                || (x.DocumentPageNumber != null && x.DocumentPageNumber.Contains(keywords))
                || (x.DocumentSheetCount != null && x.DocumentSheetCount.Contains(keywords))
                || (x.ProductionInspectionMemo != null && x.ProductionInspectionMemo.Contains(keywords))
                || (x.ProductionMemoPageFormat != null && x.ProductionMemoPageFormat.Contains(keywords))
                || (x.SizeDimensions != null && x.SizeDimensions.Contains(keywords))
                || (x.BasicMaterial != null && x.BasicMaterial.Contains(keywords))
                || (x.IndustryStandardDescription != null && x.IndustryStandardDescription.Contains(keywords))
                || (x.LaboratoryDesignOffice != null && x.LaboratoryDesignOffice.Contains(keywords))
                || (x.PurchasingValueKey != null && x.PurchasingValueKey.Contains(keywords))
                || SqlFunc.ToString(x.GrossWeight).Contains(keywords)
                || SqlFunc.ToString(x.NetWeight).Contains(keywords)
                || (x.WeightUnit != null && x.WeightUnit.Contains(keywords))
                || SqlFunc.ToString(x.Volume).Contains(keywords)
                || (x.VolumeUnit != null && x.VolumeUnit.Contains(keywords))
                || (x.ContainerRequirements != null && x.ContainerRequirements.Contains(keywords))
                || (x.StorageConditions != null && x.StorageConditions.Contains(keywords))
                || (x.TemperatureConditions != null && x.TemperatureConditions.Contains(keywords))
                || (x.LowLevelCode != null && x.LowLevelCode.Contains(keywords))
                || (x.TransportationGroup != null && x.TransportationGroup.Contains(keywords))
                || (x.HazardousMaterialNumber != null && x.HazardousMaterialNumber.Contains(keywords))
                || (x.Division != null && x.Division.Contains(keywords))
                || (x.Competitor != null && x.Competitor.Contains(keywords))
                || (x.EuropeanArticleNumberObsolete != null && x.EuropeanArticleNumberObsolete.Contains(keywords))
                || SqlFunc.ToString(x.GrGiSlipQuantity).Contains(keywords)
                || (x.ProcurementRule != null && x.ProcurementRule.Contains(keywords))
                || (x.SourceOfSupply != null && x.SourceOfSupply.Contains(keywords))
                || (x.SeasonCategory != null && x.SeasonCategory.Contains(keywords))
                || (x.LabelType != null && x.LabelType.Contains(keywords))
                || (x.LabelForm != null && x.LabelForm.Contains(keywords))
                || (x.DeactivatedField != null && x.DeactivatedField.Contains(keywords))
                || (x.InternationalArticleNumber != null && x.InternationalArticleNumber.Contains(keywords))
                || (x.EanCategory != null && x.EanCategory.Contains(keywords))
                || SqlFunc.ToString(x.Length).Contains(keywords)
                || SqlFunc.ToString(x.Width).Contains(keywords)
                || SqlFunc.ToString(x.Height).Contains(keywords)
                || (x.DimensionUnit != null && x.DimensionUnit.Contains(keywords))
                || (x.ProductHierarchy != null && x.ProductHierarchy.Contains(keywords))
                || (x.StockTransferNetChangeCosting != null && x.StockTransferNetChangeCosting.Contains(keywords))
                || (x.CadIndicator != null && x.CadIndicator.Contains(keywords))
                || (x.QmInProcurement != null && x.QmInProcurement.Contains(keywords))
                || SqlFunc.ToString(x.AllowedPackagingWeight).Contains(keywords)
                || (x.AllowedPackagingWeightUnit != null && x.AllowedPackagingWeightUnit.Contains(keywords))
                || SqlFunc.ToString(x.AllowedPackagingVolume).Contains(keywords)
                || (x.AllowedPackagingVolumeUnit != null && x.AllowedPackagingVolumeUnit.Contains(keywords))
                || SqlFunc.ToString(x.ExcessWeightTolerance).Contains(keywords)
                || SqlFunc.ToString(x.ExcessVolumeTolerance).Contains(keywords)
                || (x.VariablePurchaseOrderUnit != null && x.VariablePurchaseOrderUnit.Contains(keywords))
                || (x.RevisionLevelAssigned != null && x.RevisionLevelAssigned.Contains(keywords))
                || (x.ConfigurableMaterial != null && x.ConfigurableMaterial.Contains(keywords))
                || (x.BatchManagementRequired != null && x.BatchManagementRequired.Contains(keywords))
                || (x.PackagingMaterialType != null && x.PackagingMaterialType.Contains(keywords))
                || SqlFunc.ToString(x.MaximumLevelByVolume).Contains(keywords)
                || SqlFunc.ToString(x.StackingFactor).Contains(keywords)
                || (x.PackagingMaterialGroup != null && x.PackagingMaterialGroup.Contains(keywords))
                || (x.AuthorizationGroup != null && x.AuthorizationGroup.Contains(keywords))
                || (x.SeasonYear != null && x.SeasonYear.Contains(keywords))
                || (x.PriceBandCategory != null && x.PriceBandCategory.Contains(keywords))
                || (x.EmptiesBillOfMaterial != null && x.EmptiesBillOfMaterial.Contains(keywords))
                || (x.ExternalMaterialGroup != null && x.ExternalMaterialGroup.Contains(keywords))
                || (x.CrossPlantConfigurableMaterial != null && x.CrossPlantConfigurableMaterial.Contains(keywords))
                || (x.MaterialCategory != null && x.MaterialCategory.Contains(keywords))
                || (x.CoProductIndicator != null && x.CoProductIndicator.Contains(keywords))
                || (x.FollowUpMaterialIndicator != null && x.FollowUpMaterialIndicator.Contains(keywords))
                || (x.PricingReferenceMaterial != null && x.PricingReferenceMaterial.Contains(keywords))
                || (x.CrossPlantMaterialStatus != null && x.CrossPlantMaterialStatus.Contains(keywords))
                || (x.CrossDistributionChainStatus != null && x.CrossDistributionChainStatus.Contains(keywords))
                || (x.TaxClassification != null && x.TaxClassification.Contains(keywords))
                || (x.CatalogProfile != null && x.CatalogProfile.Contains(keywords))
                || SqlFunc.ToString(x.MinimumRemainingShelfLife).Contains(keywords)
                || SqlFunc.ToString(x.TotalShelfLife).Contains(keywords)
                || SqlFunc.ToString(x.StoragePercentage).Contains(keywords)
                || (x.ContentUnit != null && x.ContentUnit.Contains(keywords))
                || SqlFunc.ToString(x.NetContents).Contains(keywords)
                || SqlFunc.ToString(x.ComparisonPriceUnit).Contains(keywords)
                || (x.LabelingMaterialGrouping != null && x.LabelingMaterialGrouping.Contains(keywords))
                || SqlFunc.ToString(x.GrossContents).Contains(keywords)
                || (x.QuantityConversionMethod != null && x.QuantityConversionMethod.Contains(keywords))
                || (x.InternalObjectNumber != null && x.InternalObjectNumber.Contains(keywords))
                || (x.EnvironmentallyRelevant != null && x.EnvironmentallyRelevant.Contains(keywords))
                || (x.ProductAllocationProcedure != null && x.ProductAllocationProcedure.Contains(keywords))
                || (x.VariantPricingProfile != null && x.VariantPricingProfile.Contains(keywords))
                || (x.DiscountInKind != null && x.DiscountInKind.Contains(keywords))
                || (x.ManufacturerPartNumber != null && x.ManufacturerPartNumber.Contains(keywords))
                || (x.ManufacturerNumber != null && x.ManufacturerNumber.Contains(keywords))
                || (x.InventoryManagedMaterialNumber != null && x.InventoryManagedMaterialNumber.Contains(keywords))
                || (x.ManufacturerPartProfile != null && x.ManufacturerPartProfile.Contains(keywords))
                || (x.UnitsOfMeasureUsage != null && x.UnitsOfMeasureUsage.Contains(keywords))
                || (x.SeasonRollout != null && x.SeasonRollout.Contains(keywords))
                || (x.DangerousGoodsProfile != null && x.DangerousGoodsProfile.Contains(keywords))
                || (x.HighlyViscous != null && x.HighlyViscous.Contains(keywords))
                || (x.InBulkLiquid != null && x.InBulkLiquid.Contains(keywords))
                || (x.SerialNumberExplicitness != null && x.SerialNumberExplicitness.Contains(keywords))
                || (x.ClosedPackaging != null && x.ClosedPackaging.Contains(keywords))
                || (x.ApprovedBatchRecordRequired != null && x.ApprovedBatchRecordRequired.Contains(keywords))
                || (x.EffectivityParameterOverride != null && x.EffectivityParameterOverride.Contains(keywords))
                || (x.MaterialCompletionLevel != null && x.MaterialCompletionLevel.Contains(keywords))
                || (x.ShelfLifePeriodIndicator != null && x.ShelfLifePeriodIndicator.Contains(keywords))
                || (x.ShelfLifeRoundingRule != null && x.ShelfLifeRoundingRule.Contains(keywords))
                || (x.ProductCompositionOnPackaging != null && x.ProductCompositionOnPackaging.Contains(keywords))
                || (x.GeneralItemCategoryGroup != null && x.GeneralItemCategoryGroup.Contains(keywords))
                || (x.LogisticalVariants != null && x.LogisticalVariants.Contains(keywords))
                || (x.MaterialLocked != null && x.MaterialLocked.Contains(keywords))
                || (x.ConfigurationManagementRelevant != null && x.ConfigurationManagementRelevant.Contains(keywords))
                || (x.AssortmentListType != null && x.AssortmentListType.Contains(keywords))
                || (x.ExpirationDateType != null && x.ExpirationDateType.Contains(keywords))
                || (x.GtinVariant != null && x.GtinVariant.Contains(keywords))
                || (x.GenericMaterialNumber != null && x.GenericMaterialNumber.Contains(keywords))
                || (x.SamePackingReferenceMaterial != null && x.SamePackingReferenceMaterial.Contains(keywords))
                || (x.GlobalDataSyncRelevant != null && x.GlobalDataSyncRelevant.Contains(keywords))
                || (x.AcceptanceAtOrigin != null && x.AcceptanceAtOrigin.Contains(keywords))
                || (x.StandardHuType != null && x.StandardHuType.Contains(keywords))
                || (x.Pilferable != null && x.Pilferable.Contains(keywords))
                || (x.WarehouseStorageCondition != null && x.WarehouseStorageCondition.Contains(keywords))
                || (x.WarehouseMaterialGroup != null && x.WarehouseMaterialGroup.Contains(keywords))
                || (x.HandlingIndicator != null && x.HandlingIndicator.Contains(keywords))
                || (x.HazardousSubstancesRelevant != null && x.HazardousSubstancesRelevant.Contains(keywords))
                || (x.HandlingUnitType != null && x.HandlingUnitType.Contains(keywords))
                || (x.VariableTareWeight != null && x.VariableTareWeight.Contains(keywords))
                || SqlFunc.ToString(x.MaximumAllowedCapacity).Contains(keywords)
                || SqlFunc.ToString(x.OvercapacityTolerance).Contains(keywords)
                || SqlFunc.ToString(x.MaximumPackingLength).Contains(keywords)
                || SqlFunc.ToString(x.MaximumPackingWidth).Contains(keywords)
                || SqlFunc.ToString(x.MaximumPackingHeight).Contains(keywords)
                || (x.MaximumPackingDimensionUnit != null && x.MaximumPackingDimensionUnit.Contains(keywords))
                || (x.CountryOfOrigin != null && x.CountryOfOrigin.Contains(keywords))
                || (x.MaterialFreightGroup != null && x.MaterialFreightGroup.Contains(keywords))
                || SqlFunc.ToString(x.QuarantinePeriod).Contains(keywords)
                || (x.QuarantinePeriodUnit != null && x.QuarantinePeriodUnit.Contains(keywords))
                || (x.QualityInspectionGroup != null && x.QualityInspectionGroup.Contains(keywords))
                || (x.SerialNumberProfile != null && x.SerialNumberProfile.Contains(keywords))
                || (x.FormName != null && x.FormName.Contains(keywords))
                || (x.LogisticsUnitOfMeasure != null && x.LogisticsUnitOfMeasure.Contains(keywords))
                || (x.CatchWeightMaterial != null && x.CatchWeightMaterial.Contains(keywords))
                || (x.CatchWeightProfile != null && x.CatchWeightProfile.Contains(keywords))
                || (x.CatchWeightToleranceGroup != null && x.CatchWeightToleranceGroup.Contains(keywords))
                || (x.AdjustmentProfile != null && x.AdjustmentProfile.Contains(keywords))
                || (x.IntellectualPropertyId != null && x.IntellectualPropertyId.Contains(keywords))
                || (x.VariantPriceAllowed != null && x.VariantPriceAllowed.Contains(keywords))
                || (x.Medium != null && x.Medium.Contains(keywords))
                || (x.PhysicalCommodity != null && x.PhysicalCommodity.Contains(keywords))
                || (x.AnimalOrigin != null && x.AnimalOrigin.Contains(keywords))
                || (x.TextileCompositionFunction != null && x.TextileCompositionFunction.Contains(keywords))
                || (x.SegmentationStructure != null && x.SegmentationStructure.Contains(keywords))
                || (x.SegmentationStrategy != null && x.SegmentationStrategy.Contains(keywords))
                || (x.SegmentationStatus != null && x.SegmentationStatus.Contains(keywords))
                || (x.SegmentationScope != null && x.SegmentationScope.Contains(keywords))
                || (x.SegmentationRelevant != null && x.SegmentationRelevant.Contains(keywords))
                || (x.AnpCode != null && x.AnpCode.Contains(keywords))
                || (x.FashionAttribute1 != null && x.FashionAttribute1.Contains(keywords))
                || (x.FashionAttribute2 != null && x.FashionAttribute2.Contains(keywords))
                || (x.FashionAttribute3 != null && x.FashionAttribute3.Contains(keywords))
                || (x.SeasonUsageIndicator != null && x.SeasonUsageIndicator.Contains(keywords))
                || (x.SeasonActiveInInventory != null && x.SeasonActiveInInventory.Contains(keywords))
                || (x.CharacteristicConversionId != null && x.CharacteristicConversionId.Contains(keywords))
                || (x.PackagingCode != null && x.PackagingCode.Contains(keywords))
                || (x.DangerousGoodsPackagingStatus != null && x.DangerousGoodsPackagingStatus.Contains(keywords))
                || (x.MaterialConditionManagement != null && x.MaterialConditionManagement.Contains(keywords))
                || (x.ReturnCode != null && x.ReturnCode.Contains(keywords))
                || (x.ReturnToLogisticsLevel != null && x.ReturnToLogisticsLevel.Contains(keywords))
                || (x.NatoItemIdentificationNumber != null && x.NatoItemIdentificationNumber.Contains(keywords))
                || (x.FffClass != null && x.FffClass.Contains(keywords))
                || (x.SupersessionChainNumber != null && x.SupersessionChainNumber.Contains(keywords))
                || (x.SeasonalProcurementCreationStatus != null && x.SeasonalProcurementCreationStatus.Contains(keywords))
                || (x.ColorCharacteristicInternalNumber != null && x.ColorCharacteristicInternalNumber.Contains(keywords))
                || (x.MainSizeCharacteristicInternalNumber != null && x.MainSizeCharacteristicInternalNumber.Contains(keywords))
                || (x.SecondSizeCharacteristicInternalNumber != null && x.SecondSizeCharacteristicInternalNumber.Contains(keywords))
                || (x.Color != null && x.Color.Contains(keywords))
                || (x.MainSize != null && x.MainSize.Contains(keywords))
                || (x.SecondSize != null && x.SecondSize.Contains(keywords))
                || (x.EvaluationCharacteristicValue != null && x.EvaluationCharacteristicValue.Contains(keywords))
                || (x.CareCode != null && x.CareCode.Contains(keywords))
                || (x.BrandId != null && x.BrandId.Contains(keywords))
                || (x.FiberCode1 != null && x.FiberCode1.Contains(keywords))
                || (x.FiberPart1 != null && x.FiberPart1.Contains(keywords))
                || (x.FiberCode2 != null && x.FiberCode2.Contains(keywords))
                || (x.FiberPart2 != null && x.FiberPart2.Contains(keywords))
                || (x.FiberCode3 != null && x.FiberCode3.Contains(keywords))
                || (x.FiberPart3 != null && x.FiberPart3.Contains(keywords))
                || (x.FiberCode4 != null && x.FiberCode4.Contains(keywords))
                || (x.FiberPart4 != null && x.FiberPart4.Contains(keywords))
                || (x.FiberCode5 != null && x.FiberCode5.Contains(keywords))
                || (x.FiberPart5 != null && x.FiberPart5.Contains(keywords))
                || (x.FashionGrade != null && x.FashionGrade.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.ValidFromDate).Contains(keywords)
                || SqlFunc.ToString(x.ValidToDate).Contains(keywords)
                || SqlFunc.ToString(x.CrossPlantStatusValidFrom).Contains(keywords)
                || SqlFunc.ToString(x.CrossDistributionStatusValidFrom).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialCode))
        {
            exp = exp.And(x => x.MaterialCode != null && x.MaterialCode.Contains(queryDto.MaterialCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.CompleteMaintenanceStatus))
        {
            exp = exp.And(x => x.CompleteMaintenanceStatus != null && x.CompleteMaintenanceStatus.Contains(queryDto.CompleteMaintenanceStatus));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaintenanceStatus))
        {
            exp = exp.And(x => x.MaintenanceStatus != null && x.MaintenanceStatus.Contains(queryDto.MaintenanceStatus));
        }

        if (!string.IsNullOrEmpty(queryDto?.ClientDeletionFlag))
        {
            exp = exp.And(x => x.ClientDeletionFlag != null && x.ClientDeletionFlag.Contains(queryDto.ClientDeletionFlag));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialType))
        {
            exp = exp.And(x => x.MaterialType != null && x.MaterialType.Contains(queryDto.MaterialType));
        }

        if (!string.IsNullOrEmpty(queryDto?.IndustrySector))
        {
            exp = exp.And(x => x.IndustrySector != null && x.IndustrySector.Contains(queryDto.IndustrySector));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialGroup))
        {
            exp = exp.And(x => x.MaterialGroup != null && x.MaterialGroup.Contains(queryDto.MaterialGroup));
        }

        if (!string.IsNullOrEmpty(queryDto?.OldMaterialNumber))
        {
            exp = exp.And(x => x.OldMaterialNumber != null && x.OldMaterialNumber.Contains(queryDto.OldMaterialNumber));
        }

        if (!string.IsNullOrEmpty(queryDto?.BaseUnit))
        {
            exp = exp.And(x => x.BaseUnit != null && x.BaseUnit.Contains(queryDto.BaseUnit));
        }

        if (!string.IsNullOrEmpty(queryDto?.OrderUnit))
        {
            exp = exp.And(x => x.OrderUnit != null && x.OrderUnit.Contains(queryDto.OrderUnit));
        }

        if (!string.IsNullOrEmpty(queryDto?.DocumentNumber))
        {
            exp = exp.And(x => x.DocumentNumber != null && x.DocumentNumber.Contains(queryDto.DocumentNumber));
        }

        if (!string.IsNullOrEmpty(queryDto?.DocumentType))
        {
            exp = exp.And(x => x.DocumentType != null && x.DocumentType.Contains(queryDto.DocumentType));
        }

        if (!string.IsNullOrEmpty(queryDto?.DocumentVersion))
        {
            exp = exp.And(x => x.DocumentVersion != null && x.DocumentVersion.Contains(queryDto.DocumentVersion));
        }

        if (!string.IsNullOrEmpty(queryDto?.DocumentPageFormat))
        {
            exp = exp.And(x => x.DocumentPageFormat != null && x.DocumentPageFormat.Contains(queryDto.DocumentPageFormat));
        }

        if (!string.IsNullOrEmpty(queryDto?.DocumentChangeNumber))
        {
            exp = exp.And(x => x.DocumentChangeNumber != null && x.DocumentChangeNumber.Contains(queryDto.DocumentChangeNumber));
        }

        if (!string.IsNullOrEmpty(queryDto?.DocumentPageNumber))
        {
            exp = exp.And(x => x.DocumentPageNumber != null && x.DocumentPageNumber.Contains(queryDto.DocumentPageNumber));
        }

        if (!string.IsNullOrEmpty(queryDto?.DocumentSheetCount))
        {
            exp = exp.And(x => x.DocumentSheetCount != null && x.DocumentSheetCount.Contains(queryDto.DocumentSheetCount));
        }

        if (!string.IsNullOrEmpty(queryDto?.ProductionInspectionMemo))
        {
            exp = exp.And(x => x.ProductionInspectionMemo != null && x.ProductionInspectionMemo.Contains(queryDto.ProductionInspectionMemo));
        }

        if (!string.IsNullOrEmpty(queryDto?.ProductionMemoPageFormat))
        {
            exp = exp.And(x => x.ProductionMemoPageFormat != null && x.ProductionMemoPageFormat.Contains(queryDto.ProductionMemoPageFormat));
        }

        if (!string.IsNullOrEmpty(queryDto?.SizeDimensions))
        {
            exp = exp.And(x => x.SizeDimensions != null && x.SizeDimensions.Contains(queryDto.SizeDimensions));
        }

        if (!string.IsNullOrEmpty(queryDto?.BasicMaterial))
        {
            exp = exp.And(x => x.BasicMaterial != null && x.BasicMaterial.Contains(queryDto.BasicMaterial));
        }

        if (!string.IsNullOrEmpty(queryDto?.IndustryStandardDescription))
        {
            exp = exp.And(x => x.IndustryStandardDescription != null && x.IndustryStandardDescription.Contains(queryDto.IndustryStandardDescription));
        }

        if (!string.IsNullOrEmpty(queryDto?.LaboratoryDesignOffice))
        {
            exp = exp.And(x => x.LaboratoryDesignOffice != null && x.LaboratoryDesignOffice.Contains(queryDto.LaboratoryDesignOffice));
        }

        if (!string.IsNullOrEmpty(queryDto?.PurchasingValueKey))
        {
            exp = exp.And(x => x.PurchasingValueKey != null && x.PurchasingValueKey.Contains(queryDto.PurchasingValueKey));
        }

        if (queryDto?.GrossWeight.HasValue == true)
        {
            exp = exp.And(x => x.GrossWeight == queryDto.GrossWeight);
        }

        if (queryDto?.NetWeight.HasValue == true)
        {
            exp = exp.And(x => x.NetWeight == queryDto.NetWeight);
        }

        if (!string.IsNullOrEmpty(queryDto?.WeightUnit))
        {
            exp = exp.And(x => x.WeightUnit != null && x.WeightUnit.Contains(queryDto.WeightUnit));
        }

        if (queryDto?.Volume.HasValue == true)
        {
            exp = exp.And(x => x.Volume == queryDto.Volume);
        }

        if (!string.IsNullOrEmpty(queryDto?.VolumeUnit))
        {
            exp = exp.And(x => x.VolumeUnit != null && x.VolumeUnit.Contains(queryDto.VolumeUnit));
        }

        if (!string.IsNullOrEmpty(queryDto?.ContainerRequirements))
        {
            exp = exp.And(x => x.ContainerRequirements != null && x.ContainerRequirements.Contains(queryDto.ContainerRequirements));
        }

        if (!string.IsNullOrEmpty(queryDto?.StorageConditions))
        {
            exp = exp.And(x => x.StorageConditions != null && x.StorageConditions.Contains(queryDto.StorageConditions));
        }

        if (!string.IsNullOrEmpty(queryDto?.TemperatureConditions))
        {
            exp = exp.And(x => x.TemperatureConditions != null && x.TemperatureConditions.Contains(queryDto.TemperatureConditions));
        }

        if (!string.IsNullOrEmpty(queryDto?.LowLevelCode))
        {
            exp = exp.And(x => x.LowLevelCode != null && x.LowLevelCode.Contains(queryDto.LowLevelCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.TransportationGroup))
        {
            exp = exp.And(x => x.TransportationGroup != null && x.TransportationGroup.Contains(queryDto.TransportationGroup));
        }

        if (!string.IsNullOrEmpty(queryDto?.HazardousMaterialNumber))
        {
            exp = exp.And(x => x.HazardousMaterialNumber != null && x.HazardousMaterialNumber.Contains(queryDto.HazardousMaterialNumber));
        }

        if (!string.IsNullOrEmpty(queryDto?.Division))
        {
            exp = exp.And(x => x.Division != null && x.Division.Contains(queryDto.Division));
        }

        if (!string.IsNullOrEmpty(queryDto?.Competitor))
        {
            exp = exp.And(x => x.Competitor != null && x.Competitor.Contains(queryDto.Competitor));
        }

        if (!string.IsNullOrEmpty(queryDto?.EuropeanArticleNumberObsolete))
        {
            exp = exp.And(x => x.EuropeanArticleNumberObsolete != null && x.EuropeanArticleNumberObsolete.Contains(queryDto.EuropeanArticleNumberObsolete));
        }

        if (queryDto?.GrGiSlipQuantity.HasValue == true)
        {
            exp = exp.And(x => x.GrGiSlipQuantity == queryDto.GrGiSlipQuantity);
        }

        if (!string.IsNullOrEmpty(queryDto?.ProcurementRule))
        {
            exp = exp.And(x => x.ProcurementRule != null && x.ProcurementRule.Contains(queryDto.ProcurementRule));
        }

        if (!string.IsNullOrEmpty(queryDto?.SourceOfSupply))
        {
            exp = exp.And(x => x.SourceOfSupply != null && x.SourceOfSupply.Contains(queryDto.SourceOfSupply));
        }

        if (!string.IsNullOrEmpty(queryDto?.SeasonCategory))
        {
            exp = exp.And(x => x.SeasonCategory != null && x.SeasonCategory.Contains(queryDto.SeasonCategory));
        }

        if (!string.IsNullOrEmpty(queryDto?.LabelType))
        {
            exp = exp.And(x => x.LabelType != null && x.LabelType.Contains(queryDto.LabelType));
        }

        if (!string.IsNullOrEmpty(queryDto?.LabelForm))
        {
            exp = exp.And(x => x.LabelForm != null && x.LabelForm.Contains(queryDto.LabelForm));
        }

        if (!string.IsNullOrEmpty(queryDto?.DeactivatedField))
        {
            exp = exp.And(x => x.DeactivatedField != null && x.DeactivatedField.Contains(queryDto.DeactivatedField));
        }

        if (!string.IsNullOrEmpty(queryDto?.InternationalArticleNumber))
        {
            exp = exp.And(x => x.InternationalArticleNumber != null && x.InternationalArticleNumber.Contains(queryDto.InternationalArticleNumber));
        }

        if (!string.IsNullOrEmpty(queryDto?.EanCategory))
        {
            exp = exp.And(x => x.EanCategory != null && x.EanCategory.Contains(queryDto.EanCategory));
        }

        if (queryDto?.Length.HasValue == true)
        {
            exp = exp.And(x => x.Length == queryDto.Length);
        }

        if (queryDto?.Width.HasValue == true)
        {
            exp = exp.And(x => x.Width == queryDto.Width);
        }

        if (queryDto?.Height.HasValue == true)
        {
            exp = exp.And(x => x.Height == queryDto.Height);
        }

        if (!string.IsNullOrEmpty(queryDto?.DimensionUnit))
        {
            exp = exp.And(x => x.DimensionUnit != null && x.DimensionUnit.Contains(queryDto.DimensionUnit));
        }

        if (!string.IsNullOrEmpty(queryDto?.ProductHierarchy))
        {
            exp = exp.And(x => x.ProductHierarchy != null && x.ProductHierarchy.Contains(queryDto.ProductHierarchy));
        }

        if (!string.IsNullOrEmpty(queryDto?.StockTransferNetChangeCosting))
        {
            exp = exp.And(x => x.StockTransferNetChangeCosting != null && x.StockTransferNetChangeCosting.Contains(queryDto.StockTransferNetChangeCosting));
        }

        if (!string.IsNullOrEmpty(queryDto?.CadIndicator))
        {
            exp = exp.And(x => x.CadIndicator != null && x.CadIndicator.Contains(queryDto.CadIndicator));
        }

        if (!string.IsNullOrEmpty(queryDto?.QmInProcurement))
        {
            exp = exp.And(x => x.QmInProcurement != null && x.QmInProcurement.Contains(queryDto.QmInProcurement));
        }

        if (queryDto?.AllowedPackagingWeight.HasValue == true)
        {
            exp = exp.And(x => x.AllowedPackagingWeight == queryDto.AllowedPackagingWeight);
        }

        if (!string.IsNullOrEmpty(queryDto?.AllowedPackagingWeightUnit))
        {
            exp = exp.And(x => x.AllowedPackagingWeightUnit != null && x.AllowedPackagingWeightUnit.Contains(queryDto.AllowedPackagingWeightUnit));
        }

        if (queryDto?.AllowedPackagingVolume.HasValue == true)
        {
            exp = exp.And(x => x.AllowedPackagingVolume == queryDto.AllowedPackagingVolume);
        }

        if (!string.IsNullOrEmpty(queryDto?.AllowedPackagingVolumeUnit))
        {
            exp = exp.And(x => x.AllowedPackagingVolumeUnit != null && x.AllowedPackagingVolumeUnit.Contains(queryDto.AllowedPackagingVolumeUnit));
        }

        if (queryDto?.ExcessWeightTolerance.HasValue == true)
        {
            exp = exp.And(x => x.ExcessWeightTolerance == queryDto.ExcessWeightTolerance);
        }

        if (queryDto?.ExcessVolumeTolerance.HasValue == true)
        {
            exp = exp.And(x => x.ExcessVolumeTolerance == queryDto.ExcessVolumeTolerance);
        }

        if (!string.IsNullOrEmpty(queryDto?.VariablePurchaseOrderUnit))
        {
            exp = exp.And(x => x.VariablePurchaseOrderUnit != null && x.VariablePurchaseOrderUnit.Contains(queryDto.VariablePurchaseOrderUnit));
        }

        if (!string.IsNullOrEmpty(queryDto?.RevisionLevelAssigned))
        {
            exp = exp.And(x => x.RevisionLevelAssigned != null && x.RevisionLevelAssigned.Contains(queryDto.RevisionLevelAssigned));
        }

        if (!string.IsNullOrEmpty(queryDto?.ConfigurableMaterial))
        {
            exp = exp.And(x => x.ConfigurableMaterial != null && x.ConfigurableMaterial.Contains(queryDto.ConfigurableMaterial));
        }

        if (!string.IsNullOrEmpty(queryDto?.BatchManagementRequired))
        {
            exp = exp.And(x => x.BatchManagementRequired != null && x.BatchManagementRequired.Contains(queryDto.BatchManagementRequired));
        }

        if (!string.IsNullOrEmpty(queryDto?.PackagingMaterialType))
        {
            exp = exp.And(x => x.PackagingMaterialType != null && x.PackagingMaterialType.Contains(queryDto.PackagingMaterialType));
        }

        if (queryDto?.MaximumLevelByVolume.HasValue == true)
        {
            exp = exp.And(x => x.MaximumLevelByVolume == queryDto.MaximumLevelByVolume);
        }

        if (queryDto?.StackingFactor.HasValue == true)
        {
            exp = exp.And(x => x.StackingFactor == queryDto.StackingFactor);
        }

        if (!string.IsNullOrEmpty(queryDto?.PackagingMaterialGroup))
        {
            exp = exp.And(x => x.PackagingMaterialGroup != null && x.PackagingMaterialGroup.Contains(queryDto.PackagingMaterialGroup));
        }

        if (!string.IsNullOrEmpty(queryDto?.AuthorizationGroup))
        {
            exp = exp.And(x => x.AuthorizationGroup != null && x.AuthorizationGroup.Contains(queryDto.AuthorizationGroup));
        }

        if (!string.IsNullOrEmpty(queryDto?.SeasonYear))
        {
            exp = exp.And(x => x.SeasonYear != null && x.SeasonYear.Contains(queryDto.SeasonYear));
        }

        if (!string.IsNullOrEmpty(queryDto?.PriceBandCategory))
        {
            exp = exp.And(x => x.PriceBandCategory != null && x.PriceBandCategory.Contains(queryDto.PriceBandCategory));
        }

        if (!string.IsNullOrEmpty(queryDto?.EmptiesBillOfMaterial))
        {
            exp = exp.And(x => x.EmptiesBillOfMaterial != null && x.EmptiesBillOfMaterial.Contains(queryDto.EmptiesBillOfMaterial));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExternalMaterialGroup))
        {
            exp = exp.And(x => x.ExternalMaterialGroup != null && x.ExternalMaterialGroup.Contains(queryDto.ExternalMaterialGroup));
        }

        if (!string.IsNullOrEmpty(queryDto?.CrossPlantConfigurableMaterial))
        {
            exp = exp.And(x => x.CrossPlantConfigurableMaterial != null && x.CrossPlantConfigurableMaterial.Contains(queryDto.CrossPlantConfigurableMaterial));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialCategory))
        {
            exp = exp.And(x => x.MaterialCategory != null && x.MaterialCategory.Contains(queryDto.MaterialCategory));
        }

        if (!string.IsNullOrEmpty(queryDto?.CoProductIndicator))
        {
            exp = exp.And(x => x.CoProductIndicator != null && x.CoProductIndicator.Contains(queryDto.CoProductIndicator));
        }

        if (!string.IsNullOrEmpty(queryDto?.FollowUpMaterialIndicator))
        {
            exp = exp.And(x => x.FollowUpMaterialIndicator != null && x.FollowUpMaterialIndicator.Contains(queryDto.FollowUpMaterialIndicator));
        }

        if (!string.IsNullOrEmpty(queryDto?.PricingReferenceMaterial))
        {
            exp = exp.And(x => x.PricingReferenceMaterial != null && x.PricingReferenceMaterial.Contains(queryDto.PricingReferenceMaterial));
        }

        if (!string.IsNullOrEmpty(queryDto?.CrossPlantMaterialStatus))
        {
            exp = exp.And(x => x.CrossPlantMaterialStatus != null && x.CrossPlantMaterialStatus.Contains(queryDto.CrossPlantMaterialStatus));
        }

        if (!string.IsNullOrEmpty(queryDto?.CrossDistributionChainStatus))
        {
            exp = exp.And(x => x.CrossDistributionChainStatus != null && x.CrossDistributionChainStatus.Contains(queryDto.CrossDistributionChainStatus));
        }

        if (!string.IsNullOrEmpty(queryDto?.TaxClassification))
        {
            exp = exp.And(x => x.TaxClassification != null && x.TaxClassification.Contains(queryDto.TaxClassification));
        }

        if (!string.IsNullOrEmpty(queryDto?.CatalogProfile))
        {
            exp = exp.And(x => x.CatalogProfile != null && x.CatalogProfile.Contains(queryDto.CatalogProfile));
        }

        if (queryDto?.MinimumRemainingShelfLife.HasValue == true)
        {
            exp = exp.And(x => x.MinimumRemainingShelfLife == queryDto.MinimumRemainingShelfLife);
        }

        if (queryDto?.TotalShelfLife.HasValue == true)
        {
            exp = exp.And(x => x.TotalShelfLife == queryDto.TotalShelfLife);
        }

        if (queryDto?.StoragePercentage.HasValue == true)
        {
            exp = exp.And(x => x.StoragePercentage == queryDto.StoragePercentage);
        }

        if (!string.IsNullOrEmpty(queryDto?.ContentUnit))
        {
            exp = exp.And(x => x.ContentUnit != null && x.ContentUnit.Contains(queryDto.ContentUnit));
        }

        if (queryDto?.NetContents.HasValue == true)
        {
            exp = exp.And(x => x.NetContents == queryDto.NetContents);
        }

        if (queryDto?.ComparisonPriceUnit.HasValue == true)
        {
            exp = exp.And(x => x.ComparisonPriceUnit == queryDto.ComparisonPriceUnit);
        }

        if (!string.IsNullOrEmpty(queryDto?.LabelingMaterialGrouping))
        {
            exp = exp.And(x => x.LabelingMaterialGrouping != null && x.LabelingMaterialGrouping.Contains(queryDto.LabelingMaterialGrouping));
        }

        if (queryDto?.GrossContents.HasValue == true)
        {
            exp = exp.And(x => x.GrossContents == queryDto.GrossContents);
        }

        if (!string.IsNullOrEmpty(queryDto?.QuantityConversionMethod))
        {
            exp = exp.And(x => x.QuantityConversionMethod != null && x.QuantityConversionMethod.Contains(queryDto.QuantityConversionMethod));
        }

        if (!string.IsNullOrEmpty(queryDto?.InternalObjectNumber))
        {
            exp = exp.And(x => x.InternalObjectNumber != null && x.InternalObjectNumber.Contains(queryDto.InternalObjectNumber));
        }

        if (!string.IsNullOrEmpty(queryDto?.EnvironmentallyRelevant))
        {
            exp = exp.And(x => x.EnvironmentallyRelevant != null && x.EnvironmentallyRelevant.Contains(queryDto.EnvironmentallyRelevant));
        }

        if (!string.IsNullOrEmpty(queryDto?.ProductAllocationProcedure))
        {
            exp = exp.And(x => x.ProductAllocationProcedure != null && x.ProductAllocationProcedure.Contains(queryDto.ProductAllocationProcedure));
        }

        if (!string.IsNullOrEmpty(queryDto?.VariantPricingProfile))
        {
            exp = exp.And(x => x.VariantPricingProfile != null && x.VariantPricingProfile.Contains(queryDto.VariantPricingProfile));
        }

        if (!string.IsNullOrEmpty(queryDto?.DiscountInKind))
        {
            exp = exp.And(x => x.DiscountInKind != null && x.DiscountInKind.Contains(queryDto.DiscountInKind));
        }

        if (!string.IsNullOrEmpty(queryDto?.ManufacturerPartNumber))
        {
            exp = exp.And(x => x.ManufacturerPartNumber != null && x.ManufacturerPartNumber.Contains(queryDto.ManufacturerPartNumber));
        }

        if (!string.IsNullOrEmpty(queryDto?.ManufacturerNumber))
        {
            exp = exp.And(x => x.ManufacturerNumber != null && x.ManufacturerNumber.Contains(queryDto.ManufacturerNumber));
        }

        if (!string.IsNullOrEmpty(queryDto?.InventoryManagedMaterialNumber))
        {
            exp = exp.And(x => x.InventoryManagedMaterialNumber != null && x.InventoryManagedMaterialNumber.Contains(queryDto.InventoryManagedMaterialNumber));
        }

        if (!string.IsNullOrEmpty(queryDto?.ManufacturerPartProfile))
        {
            exp = exp.And(x => x.ManufacturerPartProfile != null && x.ManufacturerPartProfile.Contains(queryDto.ManufacturerPartProfile));
        }

        if (!string.IsNullOrEmpty(queryDto?.UnitsOfMeasureUsage))
        {
            exp = exp.And(x => x.UnitsOfMeasureUsage != null && x.UnitsOfMeasureUsage.Contains(queryDto.UnitsOfMeasureUsage));
        }

        if (!string.IsNullOrEmpty(queryDto?.SeasonRollout))
        {
            exp = exp.And(x => x.SeasonRollout != null && x.SeasonRollout.Contains(queryDto.SeasonRollout));
        }

        if (!string.IsNullOrEmpty(queryDto?.DangerousGoodsProfile))
        {
            exp = exp.And(x => x.DangerousGoodsProfile != null && x.DangerousGoodsProfile.Contains(queryDto.DangerousGoodsProfile));
        }

        if (!string.IsNullOrEmpty(queryDto?.HighlyViscous))
        {
            exp = exp.And(x => x.HighlyViscous != null && x.HighlyViscous.Contains(queryDto.HighlyViscous));
        }

        if (!string.IsNullOrEmpty(queryDto?.InBulkLiquid))
        {
            exp = exp.And(x => x.InBulkLiquid != null && x.InBulkLiquid.Contains(queryDto.InBulkLiquid));
        }

        if (!string.IsNullOrEmpty(queryDto?.SerialNumberExplicitness))
        {
            exp = exp.And(x => x.SerialNumberExplicitness != null && x.SerialNumberExplicitness.Contains(queryDto.SerialNumberExplicitness));
        }

        if (!string.IsNullOrEmpty(queryDto?.ClosedPackaging))
        {
            exp = exp.And(x => x.ClosedPackaging != null && x.ClosedPackaging.Contains(queryDto.ClosedPackaging));
        }

        if (!string.IsNullOrEmpty(queryDto?.ApprovedBatchRecordRequired))
        {
            exp = exp.And(x => x.ApprovedBatchRecordRequired != null && x.ApprovedBatchRecordRequired.Contains(queryDto.ApprovedBatchRecordRequired));
        }

        if (!string.IsNullOrEmpty(queryDto?.EffectivityParameterOverride))
        {
            exp = exp.And(x => x.EffectivityParameterOverride != null && x.EffectivityParameterOverride.Contains(queryDto.EffectivityParameterOverride));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialCompletionLevel))
        {
            exp = exp.And(x => x.MaterialCompletionLevel != null && x.MaterialCompletionLevel.Contains(queryDto.MaterialCompletionLevel));
        }

        if (!string.IsNullOrEmpty(queryDto?.ShelfLifePeriodIndicator))
        {
            exp = exp.And(x => x.ShelfLifePeriodIndicator != null && x.ShelfLifePeriodIndicator.Contains(queryDto.ShelfLifePeriodIndicator));
        }

        if (!string.IsNullOrEmpty(queryDto?.ShelfLifeRoundingRule))
        {
            exp = exp.And(x => x.ShelfLifeRoundingRule != null && x.ShelfLifeRoundingRule.Contains(queryDto.ShelfLifeRoundingRule));
        }

        if (!string.IsNullOrEmpty(queryDto?.ProductCompositionOnPackaging))
        {
            exp = exp.And(x => x.ProductCompositionOnPackaging != null && x.ProductCompositionOnPackaging.Contains(queryDto.ProductCompositionOnPackaging));
        }

        if (!string.IsNullOrEmpty(queryDto?.GeneralItemCategoryGroup))
        {
            exp = exp.And(x => x.GeneralItemCategoryGroup != null && x.GeneralItemCategoryGroup.Contains(queryDto.GeneralItemCategoryGroup));
        }

        if (!string.IsNullOrEmpty(queryDto?.LogisticalVariants))
        {
            exp = exp.And(x => x.LogisticalVariants != null && x.LogisticalVariants.Contains(queryDto.LogisticalVariants));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialLocked))
        {
            exp = exp.And(x => x.MaterialLocked != null && x.MaterialLocked.Contains(queryDto.MaterialLocked));
        }

        if (!string.IsNullOrEmpty(queryDto?.ConfigurationManagementRelevant))
        {
            exp = exp.And(x => x.ConfigurationManagementRelevant != null && x.ConfigurationManagementRelevant.Contains(queryDto.ConfigurationManagementRelevant));
        }

        if (!string.IsNullOrEmpty(queryDto?.AssortmentListType))
        {
            exp = exp.And(x => x.AssortmentListType != null && x.AssortmentListType.Contains(queryDto.AssortmentListType));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExpirationDateType))
        {
            exp = exp.And(x => x.ExpirationDateType != null && x.ExpirationDateType.Contains(queryDto.ExpirationDateType));
        }

        if (!string.IsNullOrEmpty(queryDto?.GtinVariant))
        {
            exp = exp.And(x => x.GtinVariant != null && x.GtinVariant.Contains(queryDto.GtinVariant));
        }

        if (!string.IsNullOrEmpty(queryDto?.GenericMaterialNumber))
        {
            exp = exp.And(x => x.GenericMaterialNumber != null && x.GenericMaterialNumber.Contains(queryDto.GenericMaterialNumber));
        }

        if (!string.IsNullOrEmpty(queryDto?.SamePackingReferenceMaterial))
        {
            exp = exp.And(x => x.SamePackingReferenceMaterial != null && x.SamePackingReferenceMaterial.Contains(queryDto.SamePackingReferenceMaterial));
        }

        if (!string.IsNullOrEmpty(queryDto?.GlobalDataSyncRelevant))
        {
            exp = exp.And(x => x.GlobalDataSyncRelevant != null && x.GlobalDataSyncRelevant.Contains(queryDto.GlobalDataSyncRelevant));
        }

        if (!string.IsNullOrEmpty(queryDto?.AcceptanceAtOrigin))
        {
            exp = exp.And(x => x.AcceptanceAtOrigin != null && x.AcceptanceAtOrigin.Contains(queryDto.AcceptanceAtOrigin));
        }

        if (!string.IsNullOrEmpty(queryDto?.StandardHuType))
        {
            exp = exp.And(x => x.StandardHuType != null && x.StandardHuType.Contains(queryDto.StandardHuType));
        }

        if (!string.IsNullOrEmpty(queryDto?.Pilferable))
        {
            exp = exp.And(x => x.Pilferable != null && x.Pilferable.Contains(queryDto.Pilferable));
        }

        if (!string.IsNullOrEmpty(queryDto?.WarehouseStorageCondition))
        {
            exp = exp.And(x => x.WarehouseStorageCondition != null && x.WarehouseStorageCondition.Contains(queryDto.WarehouseStorageCondition));
        }

        if (!string.IsNullOrEmpty(queryDto?.WarehouseMaterialGroup))
        {
            exp = exp.And(x => x.WarehouseMaterialGroup != null && x.WarehouseMaterialGroup.Contains(queryDto.WarehouseMaterialGroup));
        }

        if (!string.IsNullOrEmpty(queryDto?.HandlingIndicator))
        {
            exp = exp.And(x => x.HandlingIndicator != null && x.HandlingIndicator.Contains(queryDto.HandlingIndicator));
        }

        if (!string.IsNullOrEmpty(queryDto?.HazardousSubstancesRelevant))
        {
            exp = exp.And(x => x.HazardousSubstancesRelevant != null && x.HazardousSubstancesRelevant.Contains(queryDto.HazardousSubstancesRelevant));
        }

        if (!string.IsNullOrEmpty(queryDto?.HandlingUnitType))
        {
            exp = exp.And(x => x.HandlingUnitType != null && x.HandlingUnitType.Contains(queryDto.HandlingUnitType));
        }

        if (!string.IsNullOrEmpty(queryDto?.VariableTareWeight))
        {
            exp = exp.And(x => x.VariableTareWeight != null && x.VariableTareWeight.Contains(queryDto.VariableTareWeight));
        }

        if (queryDto?.MaximumAllowedCapacity.HasValue == true)
        {
            exp = exp.And(x => x.MaximumAllowedCapacity == queryDto.MaximumAllowedCapacity);
        }

        if (queryDto?.OvercapacityTolerance.HasValue == true)
        {
            exp = exp.And(x => x.OvercapacityTolerance == queryDto.OvercapacityTolerance);
        }

        if (queryDto?.MaximumPackingLength.HasValue == true)
        {
            exp = exp.And(x => x.MaximumPackingLength == queryDto.MaximumPackingLength);
        }

        if (queryDto?.MaximumPackingWidth.HasValue == true)
        {
            exp = exp.And(x => x.MaximumPackingWidth == queryDto.MaximumPackingWidth);
        }

        if (queryDto?.MaximumPackingHeight.HasValue == true)
        {
            exp = exp.And(x => x.MaximumPackingHeight == queryDto.MaximumPackingHeight);
        }

        if (!string.IsNullOrEmpty(queryDto?.MaximumPackingDimensionUnit))
        {
            exp = exp.And(x => x.MaximumPackingDimensionUnit != null && x.MaximumPackingDimensionUnit.Contains(queryDto.MaximumPackingDimensionUnit));
        }

        if (!string.IsNullOrEmpty(queryDto?.CountryOfOrigin))
        {
            exp = exp.And(x => x.CountryOfOrigin != null && x.CountryOfOrigin.Contains(queryDto.CountryOfOrigin));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialFreightGroup))
        {
            exp = exp.And(x => x.MaterialFreightGroup != null && x.MaterialFreightGroup.Contains(queryDto.MaterialFreightGroup));
        }

        if (queryDto?.QuarantinePeriod.HasValue == true)
        {
            exp = exp.And(x => x.QuarantinePeriod == queryDto.QuarantinePeriod);
        }

        if (!string.IsNullOrEmpty(queryDto?.QuarantinePeriodUnit))
        {
            exp = exp.And(x => x.QuarantinePeriodUnit != null && x.QuarantinePeriodUnit.Contains(queryDto.QuarantinePeriodUnit));
        }

        if (!string.IsNullOrEmpty(queryDto?.QualityInspectionGroup))
        {
            exp = exp.And(x => x.QualityInspectionGroup != null && x.QualityInspectionGroup.Contains(queryDto.QualityInspectionGroup));
        }

        if (!string.IsNullOrEmpty(queryDto?.SerialNumberProfile))
        {
            exp = exp.And(x => x.SerialNumberProfile != null && x.SerialNumberProfile.Contains(queryDto.SerialNumberProfile));
        }

        if (!string.IsNullOrEmpty(queryDto?.FormName))
        {
            exp = exp.And(x => x.FormName != null && x.FormName.Contains(queryDto.FormName));
        }

        if (!string.IsNullOrEmpty(queryDto?.LogisticsUnitOfMeasure))
        {
            exp = exp.And(x => x.LogisticsUnitOfMeasure != null && x.LogisticsUnitOfMeasure.Contains(queryDto.LogisticsUnitOfMeasure));
        }

        if (!string.IsNullOrEmpty(queryDto?.CatchWeightMaterial))
        {
            exp = exp.And(x => x.CatchWeightMaterial != null && x.CatchWeightMaterial.Contains(queryDto.CatchWeightMaterial));
        }

        if (!string.IsNullOrEmpty(queryDto?.CatchWeightProfile))
        {
            exp = exp.And(x => x.CatchWeightProfile != null && x.CatchWeightProfile.Contains(queryDto.CatchWeightProfile));
        }

        if (!string.IsNullOrEmpty(queryDto?.CatchWeightToleranceGroup))
        {
            exp = exp.And(x => x.CatchWeightToleranceGroup != null && x.CatchWeightToleranceGroup.Contains(queryDto.CatchWeightToleranceGroup));
        }

        if (!string.IsNullOrEmpty(queryDto?.AdjustmentProfile))
        {
            exp = exp.And(x => x.AdjustmentProfile != null && x.AdjustmentProfile.Contains(queryDto.AdjustmentProfile));
        }

        if (!string.IsNullOrEmpty(queryDto?.IntellectualPropertyId))
        {
            exp = exp.And(x => x.IntellectualPropertyId != null && x.IntellectualPropertyId.Contains(queryDto.IntellectualPropertyId));
        }

        if (!string.IsNullOrEmpty(queryDto?.VariantPriceAllowed))
        {
            exp = exp.And(x => x.VariantPriceAllowed != null && x.VariantPriceAllowed.Contains(queryDto.VariantPriceAllowed));
        }

        if (!string.IsNullOrEmpty(queryDto?.Medium))
        {
            exp = exp.And(x => x.Medium != null && x.Medium.Contains(queryDto.Medium));
        }

        if (!string.IsNullOrEmpty(queryDto?.PhysicalCommodity))
        {
            exp = exp.And(x => x.PhysicalCommodity != null && x.PhysicalCommodity.Contains(queryDto.PhysicalCommodity));
        }

        if (!string.IsNullOrEmpty(queryDto?.AnimalOrigin))
        {
            exp = exp.And(x => x.AnimalOrigin != null && x.AnimalOrigin.Contains(queryDto.AnimalOrigin));
        }

        if (!string.IsNullOrEmpty(queryDto?.TextileCompositionFunction))
        {
            exp = exp.And(x => x.TextileCompositionFunction != null && x.TextileCompositionFunction.Contains(queryDto.TextileCompositionFunction));
        }

        if (!string.IsNullOrEmpty(queryDto?.SegmentationStructure))
        {
            exp = exp.And(x => x.SegmentationStructure != null && x.SegmentationStructure.Contains(queryDto.SegmentationStructure));
        }

        if (!string.IsNullOrEmpty(queryDto?.SegmentationStrategy))
        {
            exp = exp.And(x => x.SegmentationStrategy != null && x.SegmentationStrategy.Contains(queryDto.SegmentationStrategy));
        }

        if (!string.IsNullOrEmpty(queryDto?.SegmentationStatus))
        {
            exp = exp.And(x => x.SegmentationStatus != null && x.SegmentationStatus.Contains(queryDto.SegmentationStatus));
        }

        if (!string.IsNullOrEmpty(queryDto?.SegmentationScope))
        {
            exp = exp.And(x => x.SegmentationScope != null && x.SegmentationScope.Contains(queryDto.SegmentationScope));
        }

        if (!string.IsNullOrEmpty(queryDto?.SegmentationRelevant))
        {
            exp = exp.And(x => x.SegmentationRelevant != null && x.SegmentationRelevant.Contains(queryDto.SegmentationRelevant));
        }

        if (!string.IsNullOrEmpty(queryDto?.AnpCode))
        {
            exp = exp.And(x => x.AnpCode != null && x.AnpCode.Contains(queryDto.AnpCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.FashionAttribute1))
        {
            exp = exp.And(x => x.FashionAttribute1 != null && x.FashionAttribute1.Contains(queryDto.FashionAttribute1));
        }

        if (!string.IsNullOrEmpty(queryDto?.FashionAttribute2))
        {
            exp = exp.And(x => x.FashionAttribute2 != null && x.FashionAttribute2.Contains(queryDto.FashionAttribute2));
        }

        if (!string.IsNullOrEmpty(queryDto?.FashionAttribute3))
        {
            exp = exp.And(x => x.FashionAttribute3 != null && x.FashionAttribute3.Contains(queryDto.FashionAttribute3));
        }

        if (!string.IsNullOrEmpty(queryDto?.SeasonUsageIndicator))
        {
            exp = exp.And(x => x.SeasonUsageIndicator != null && x.SeasonUsageIndicator.Contains(queryDto.SeasonUsageIndicator));
        }

        if (!string.IsNullOrEmpty(queryDto?.SeasonActiveInInventory))
        {
            exp = exp.And(x => x.SeasonActiveInInventory != null && x.SeasonActiveInInventory.Contains(queryDto.SeasonActiveInInventory));
        }

        if (!string.IsNullOrEmpty(queryDto?.CharacteristicConversionId))
        {
            exp = exp.And(x => x.CharacteristicConversionId != null && x.CharacteristicConversionId.Contains(queryDto.CharacteristicConversionId));
        }

        if (!string.IsNullOrEmpty(queryDto?.PackagingCode))
        {
            exp = exp.And(x => x.PackagingCode != null && x.PackagingCode.Contains(queryDto.PackagingCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.DangerousGoodsPackagingStatus))
        {
            exp = exp.And(x => x.DangerousGoodsPackagingStatus != null && x.DangerousGoodsPackagingStatus.Contains(queryDto.DangerousGoodsPackagingStatus));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialConditionManagement))
        {
            exp = exp.And(x => x.MaterialConditionManagement != null && x.MaterialConditionManagement.Contains(queryDto.MaterialConditionManagement));
        }

        if (!string.IsNullOrEmpty(queryDto?.ReturnCode))
        {
            exp = exp.And(x => x.ReturnCode != null && x.ReturnCode.Contains(queryDto.ReturnCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ReturnToLogisticsLevel))
        {
            exp = exp.And(x => x.ReturnToLogisticsLevel != null && x.ReturnToLogisticsLevel.Contains(queryDto.ReturnToLogisticsLevel));
        }

        if (!string.IsNullOrEmpty(queryDto?.NatoItemIdentificationNumber))
        {
            exp = exp.And(x => x.NatoItemIdentificationNumber != null && x.NatoItemIdentificationNumber.Contains(queryDto.NatoItemIdentificationNumber));
        }

        if (!string.IsNullOrEmpty(queryDto?.FffClass))
        {
            exp = exp.And(x => x.FffClass != null && x.FffClass.Contains(queryDto.FffClass));
        }

        if (!string.IsNullOrEmpty(queryDto?.SupersessionChainNumber))
        {
            exp = exp.And(x => x.SupersessionChainNumber != null && x.SupersessionChainNumber.Contains(queryDto.SupersessionChainNumber));
        }

        if (!string.IsNullOrEmpty(queryDto?.SeasonalProcurementCreationStatus))
        {
            exp = exp.And(x => x.SeasonalProcurementCreationStatus != null && x.SeasonalProcurementCreationStatus.Contains(queryDto.SeasonalProcurementCreationStatus));
        }

        if (!string.IsNullOrEmpty(queryDto?.ColorCharacteristicInternalNumber))
        {
            exp = exp.And(x => x.ColorCharacteristicInternalNumber != null && x.ColorCharacteristicInternalNumber.Contains(queryDto.ColorCharacteristicInternalNumber));
        }

        if (!string.IsNullOrEmpty(queryDto?.MainSizeCharacteristicInternalNumber))
        {
            exp = exp.And(x => x.MainSizeCharacteristicInternalNumber != null && x.MainSizeCharacteristicInternalNumber.Contains(queryDto.MainSizeCharacteristicInternalNumber));
        }

        if (!string.IsNullOrEmpty(queryDto?.SecondSizeCharacteristicInternalNumber))
        {
            exp = exp.And(x => x.SecondSizeCharacteristicInternalNumber != null && x.SecondSizeCharacteristicInternalNumber.Contains(queryDto.SecondSizeCharacteristicInternalNumber));
        }

        if (!string.IsNullOrEmpty(queryDto?.Color))
        {
            exp = exp.And(x => x.Color != null && x.Color.Contains(queryDto.Color));
        }

        if (!string.IsNullOrEmpty(queryDto?.MainSize))
        {
            exp = exp.And(x => x.MainSize != null && x.MainSize.Contains(queryDto.MainSize));
        }

        if (!string.IsNullOrEmpty(queryDto?.SecondSize))
        {
            exp = exp.And(x => x.SecondSize != null && x.SecondSize.Contains(queryDto.SecondSize));
        }

        if (!string.IsNullOrEmpty(queryDto?.EvaluationCharacteristicValue))
        {
            exp = exp.And(x => x.EvaluationCharacteristicValue != null && x.EvaluationCharacteristicValue.Contains(queryDto.EvaluationCharacteristicValue));
        }

        if (!string.IsNullOrEmpty(queryDto?.CareCode))
        {
            exp = exp.And(x => x.CareCode != null && x.CareCode.Contains(queryDto.CareCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.BrandId))
        {
            exp = exp.And(x => x.BrandId != null && x.BrandId.Contains(queryDto.BrandId));
        }

        if (!string.IsNullOrEmpty(queryDto?.FiberCode1))
        {
            exp = exp.And(x => x.FiberCode1 != null && x.FiberCode1.Contains(queryDto.FiberCode1));
        }

        if (!string.IsNullOrEmpty(queryDto?.FiberPart1))
        {
            exp = exp.And(x => x.FiberPart1 != null && x.FiberPart1.Contains(queryDto.FiberPart1));
        }

        if (!string.IsNullOrEmpty(queryDto?.FiberCode2))
        {
            exp = exp.And(x => x.FiberCode2 != null && x.FiberCode2.Contains(queryDto.FiberCode2));
        }

        if (!string.IsNullOrEmpty(queryDto?.FiberPart2))
        {
            exp = exp.And(x => x.FiberPart2 != null && x.FiberPart2.Contains(queryDto.FiberPart2));
        }

        if (!string.IsNullOrEmpty(queryDto?.FiberCode3))
        {
            exp = exp.And(x => x.FiberCode3 != null && x.FiberCode3.Contains(queryDto.FiberCode3));
        }

        if (!string.IsNullOrEmpty(queryDto?.FiberPart3))
        {
            exp = exp.And(x => x.FiberPart3 != null && x.FiberPart3.Contains(queryDto.FiberPart3));
        }

        if (!string.IsNullOrEmpty(queryDto?.FiberCode4))
        {
            exp = exp.And(x => x.FiberCode4 != null && x.FiberCode4.Contains(queryDto.FiberCode4));
        }

        if (!string.IsNullOrEmpty(queryDto?.FiberPart4))
        {
            exp = exp.And(x => x.FiberPart4 != null && x.FiberPart4.Contains(queryDto.FiberPart4));
        }

        if (!string.IsNullOrEmpty(queryDto?.FiberCode5))
        {
            exp = exp.And(x => x.FiberCode5 != null && x.FiberCode5.Contains(queryDto.FiberCode5));
        }

        if (!string.IsNullOrEmpty(queryDto?.FiberPart5))
        {
            exp = exp.And(x => x.FiberPart5 != null && x.FiberPart5.Contains(queryDto.FiberPart5));
        }

        if (!string.IsNullOrEmpty(queryDto?.FashionGrade))
        {
            exp = exp.And(x => x.FashionGrade != null && x.FashionGrade.Contains(queryDto.FashionGrade));
        }
        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        var rangeStart = queryDto?.ValidFromDateStart;
        var rangeEnd = queryDto?.ValidFromDateEnd;
        if (!rangeStart.HasValue && !rangeEnd.HasValue && !HasFiltersBesidesDefaultListScope(queryDto))
        {
            var monthBounds = GetCurrentMonthRangeBounds();
            rangeStart = monthBounds.Start;
            rangeEnd = monthBounds.End;
        }

        if (rangeStart.HasValue)
        {
            exp = exp.And(x => x.ValidFromDate >= rangeStart.Value);
        }

        if (rangeEnd.HasValue)
        {
            exp = exp.And(x => x.ValidFromDate <= rangeEnd.Value);
        }

        if (queryDto?.ValidToDateStart.HasValue == true)
        {
            exp = exp.And(x => x.ValidToDate >= queryDto.ValidToDateStart);
        }

        if (queryDto?.ValidToDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.ValidToDate <= queryDto.ValidToDateEnd);
        }

        if (queryDto?.CrossPlantStatusValidFromStart.HasValue == true)
        {
            exp = exp.And(x => x.CrossPlantStatusValidFrom >= queryDto.CrossPlantStatusValidFromStart);
        }

        if (queryDto?.CrossPlantStatusValidFromEnd.HasValue == true)
        {
            exp = exp.And(x => x.CrossPlantStatusValidFrom <= queryDto.CrossPlantStatusValidFromEnd);
        }

        if (queryDto?.CrossDistributionStatusValidFromStart.HasValue == true)
        {
            exp = exp.And(x => x.CrossDistributionStatusValidFrom >= queryDto.CrossDistributionStatusValidFromStart);
        }

        if (queryDto?.CrossDistributionStatusValidFromEnd.HasValue == true)
        {
            exp = exp.And(x => x.CrossDistributionStatusValidFrom <= queryDto.CrossDistributionStatusValidFromEnd);
        }

        if (queryDto?.CreatedAtStart.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt >= queryDto.CreatedAtStart);
        }

        if (queryDto?.CreatedAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt <= queryDto.CreatedAtEnd);
        }
        if (!string.IsNullOrWhiteSpace(queryDto?.RelatedPlant))
        {
            var relatedPlant = queryDto.RelatedPlant;
            exp = exp.And(x => x.RelatedPlant != null && x.RelatedPlant.Contains(relatedPlant));
        }


        return exp.ToExpression();
    }

    /// <summary>
    /// 当前自然月起止（含月末最后一刻），用于列表无参默认过滤、避免全表扫描
    /// </summary>
    /// <returns>起、止</returns>
    private static (DateTime Start, DateTime End) GetCurrentMonthRangeBounds()
    {
        var today = DateTime.Today;
        var start = new DateTime(today.Year, today.Month, 1);
        var end = start.AddMonths(1).AddTicks(-1);
        return (start, end);
    }
    /// <summary>
    /// 是否存在除默认当前月/当前期间外的查询条件（有参则不强制默认范围）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>有其它条件为 true</returns>
    private static bool HasFiltersBesidesDefaultListScope(TaktGeneralMaterialQueryDto? queryDto)
    {
        if (queryDto == null)
        {
            return false;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.KeyWords))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CompleteMaintenanceStatus))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaintenanceStatus))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ClientDeletionFlag))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialType))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.IndustrySector))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialGroup))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.OldMaterialNumber))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BaseUnit))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.OrderUnit))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DocumentNumber))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DocumentType))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DocumentVersion))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DocumentPageFormat))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DocumentChangeNumber))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DocumentPageNumber))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DocumentSheetCount))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ProductionInspectionMemo))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ProductionMemoPageFormat))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SizeDimensions))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BasicMaterial))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.IndustryStandardDescription))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.LaboratoryDesignOffice))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PurchasingValueKey))
        {
            return true;
        }
        if (queryDto.GrossWeight.HasValue)
        {
            return true;
        }
        if (queryDto.NetWeight.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.WeightUnit))
        {
            return true;
        }
        if (queryDto.Volume.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.VolumeUnit))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ContainerRequirements))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.StorageConditions))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TemperatureConditions))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.LowLevelCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TransportationGroup))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.HazardousMaterialNumber))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Division))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Competitor))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EuropeanArticleNumberObsolete))
        {
            return true;
        }
        if (queryDto.GrGiSlipQuantity.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ProcurementRule))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SourceOfSupply))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SeasonCategory))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.LabelType))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.LabelForm))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DeactivatedField))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.InternationalArticleNumber))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EanCategory))
        {
            return true;
        }
        if (queryDto.Length.HasValue)
        {
            return true;
        }
        if (queryDto.Width.HasValue)
        {
            return true;
        }
        if (queryDto.Height.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DimensionUnit))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ProductHierarchy))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.StockTransferNetChangeCosting))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CadIndicator))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.QmInProcurement))
        {
            return true;
        }
        if (queryDto.AllowedPackagingWeight.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.AllowedPackagingWeightUnit))
        {
            return true;
        }
        if (queryDto.AllowedPackagingVolume.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.AllowedPackagingVolumeUnit))
        {
            return true;
        }
        if (queryDto.ExcessWeightTolerance.HasValue)
        {
            return true;
        }
        if (queryDto.ExcessVolumeTolerance.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.VariablePurchaseOrderUnit))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.RevisionLevelAssigned))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ConfigurableMaterial))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BatchManagementRequired))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PackagingMaterialType))
        {
            return true;
        }
        if (queryDto.MaximumLevelByVolume.HasValue)
        {
            return true;
        }
        if (queryDto.StackingFactor.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PackagingMaterialGroup))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.AuthorizationGroup))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SeasonYear))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PriceBandCategory))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EmptiesBillOfMaterial))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ExternalMaterialGroup))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CrossPlantConfigurableMaterial))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialCategory))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CoProductIndicator))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.FollowUpMaterialIndicator))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PricingReferenceMaterial))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CrossPlantMaterialStatus))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CrossDistributionChainStatus))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TaxClassification))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CatalogProfile))
        {
            return true;
        }
        if (queryDto.MinimumRemainingShelfLife.HasValue)
        {
            return true;
        }
        if (queryDto.TotalShelfLife.HasValue)
        {
            return true;
        }
        if (queryDto.StoragePercentage.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ContentUnit))
        {
            return true;
        }
        if (queryDto.NetContents.HasValue)
        {
            return true;
        }
        if (queryDto.ComparisonPriceUnit.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.LabelingMaterialGrouping))
        {
            return true;
        }
        if (queryDto.GrossContents.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.QuantityConversionMethod))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.InternalObjectNumber))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EnvironmentallyRelevant))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ProductAllocationProcedure))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.VariantPricingProfile))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DiscountInKind))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ManufacturerPartNumber))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ManufacturerNumber))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.InventoryManagedMaterialNumber))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ManufacturerPartProfile))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.UnitsOfMeasureUsage))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SeasonRollout))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DangerousGoodsProfile))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.HighlyViscous))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.InBulkLiquid))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SerialNumberExplicitness))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ClosedPackaging))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ApprovedBatchRecordRequired))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EffectivityParameterOverride))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialCompletionLevel))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ShelfLifePeriodIndicator))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ShelfLifeRoundingRule))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ProductCompositionOnPackaging))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.GeneralItemCategoryGroup))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.LogisticalVariants))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialLocked))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ConfigurationManagementRelevant))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.AssortmentListType))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ExpirationDateType))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.GtinVariant))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.GenericMaterialNumber))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SamePackingReferenceMaterial))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.GlobalDataSyncRelevant))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.AcceptanceAtOrigin))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.StandardHuType))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Pilferable))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.WarehouseStorageCondition))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.WarehouseMaterialGroup))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.HandlingIndicator))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.HazardousSubstancesRelevant))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.HandlingUnitType))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.VariableTareWeight))
        {
            return true;
        }
        if (queryDto.MaximumAllowedCapacity.HasValue)
        {
            return true;
        }
        if (queryDto.OvercapacityTolerance.HasValue)
        {
            return true;
        }
        if (queryDto.MaximumPackingLength.HasValue)
        {
            return true;
        }
        if (queryDto.MaximumPackingWidth.HasValue)
        {
            return true;
        }
        if (queryDto.MaximumPackingHeight.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaximumPackingDimensionUnit))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CountryOfOrigin))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialFreightGroup))
        {
            return true;
        }
        if (queryDto.QuarantinePeriod.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.QuarantinePeriodUnit))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.QualityInspectionGroup))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SerialNumberProfile))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.FormName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.LogisticsUnitOfMeasure))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CatchWeightMaterial))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CatchWeightProfile))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CatchWeightToleranceGroup))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.AdjustmentProfile))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.IntellectualPropertyId))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.VariantPriceAllowed))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Medium))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PhysicalCommodity))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.AnimalOrigin))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TextileCompositionFunction))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SegmentationStructure))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SegmentationStrategy))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SegmentationStatus))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SegmentationScope))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SegmentationRelevant))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.AnpCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.FashionAttribute1))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.FashionAttribute2))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.FashionAttribute3))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SeasonUsageIndicator))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SeasonActiveInInventory))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CharacteristicConversionId))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PackagingCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DangerousGoodsPackagingStatus))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialConditionManagement))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ReturnCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ReturnToLogisticsLevel))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.NatoItemIdentificationNumber))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.FffClass))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SupersessionChainNumber))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SeasonalProcurementCreationStatus))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ColorCharacteristicInternalNumber))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MainSizeCharacteristicInternalNumber))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SecondSizeCharacteristicInternalNumber))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Color))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MainSize))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SecondSize))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EvaluationCharacteristicValue))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CareCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BrandId))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.FiberCode1))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.FiberPart1))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.FiberCode2))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.FiberPart2))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.FiberCode3))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.FiberPart3))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.FiberCode4))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.FiberPart4))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.FiberCode5))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.FiberPart5))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.FashionGrade))
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
        if (queryDto.ValidToDateStart.HasValue || queryDto.ValidToDateEnd.HasValue)
        {
            return true;
        }
        if (queryDto.CrossPlantStatusValidFromStart.HasValue || queryDto.CrossPlantStatusValidFromEnd.HasValue)
        {
            return true;
        }
        if (queryDto.CrossDistributionStatusValidFromStart.HasValue || queryDto.CrossDistributionStatusValidFromEnd.HasValue)
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
