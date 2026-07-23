// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Materials
// 文件名称：TaktMaterialDtos.cs
// 创建时间：2026-07-23
// 创建人：Takt365(Auto Generated)
// 功能描述：Material 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktMaterial 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Materials;

// ========================================
// Material 响应 DTO
// ========================================

/// <summary>
/// Takt全局物料实体（租户内共享；字段对齐 SAP MARA；多语言描述见子表 TaktMaterialDescription / SAP MAKT）
/// 对应前端 TaktMaterialDto
/// 继承 TaktTenantDtoBase
/// </summary>
public class TaktMaterialDto : TaktTenantDtoBase
{
    /// <summary>
    /// MaterialID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialId { get; set; }

    /// <summary>
    /// 物料编码（SAP MARA.MATNR）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 完整维护状态（SAP MARA.VPSTA）
    /// </summary>
    public string? CompleteMaintenanceStatus { get; set; } = string.Empty;

    /// <summary>
    /// 维护状态（SAP MARA.PSTAT）
    /// </summary>
    public string? MaintenanceStatus { get; set; } = string.Empty;

    /// <summary>
    /// 客户级删除标记（SAP MARA.LVORM）
    /// </summary>
    public string? ClientDeletionFlag { get; set; } = string.Empty;

    /// <summary>
    /// 物料类型（SAP MARA.MTART）
    /// </summary>
    public string MaterialType { get; set; } = string.Empty;

    /// <summary>
    /// 行业领域（SAP MARA.MBRSH）
    /// </summary>
    public string IndustrySector { get; set; } = string.Empty;

    /// <summary>
    /// 物料组（SAP MARA.MATKL）
    /// </summary>
    public string MaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料号（SAP MARA.BISMT）
    /// </summary>
    public string? OldMaterialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 基本计量单位（SAP MARA.MEINS）
    /// </summary>
    public string BaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单单位（SAP MARA.BSTME）
    /// </summary>
    public string? OrderUnit { get; set; } = string.Empty;

    /// <summary>
    /// 单据号（SAP MARA.ZEINR）
    /// </summary>
    public string? DocumentNumber { get; set; } = string.Empty;

    /// <summary>
    /// 单据类型（SAP MARA.ZEIAR）
    /// </summary>
    public string? DocumentType { get; set; } = string.Empty;

    /// <summary>
    /// 单据版本（SAP MARA.ZEIVR）
    /// </summary>
    public string? DocumentVersion { get; set; } = string.Empty;

    /// <summary>
    /// 单据页格式（SAP MARA.ZEIFO）
    /// </summary>
    public string? DocumentPageFormat { get; set; } = string.Empty;

    /// <summary>
    /// 单据更改号（SAP MARA.AESZN）
    /// </summary>
    public string? DocumentChangeNumber { get; set; } = string.Empty;

    /// <summary>
    /// 单据页号（SAP MARA.BLATT）
    /// </summary>
    public string? DocumentPageNumber { get; set; } = string.Empty;

    /// <summary>
    /// 单据页数（SAP MARA.BLANZ）
    /// </summary>
    public string? DocumentSheetCount { get; set; } = string.Empty;

    /// <summary>
    /// 生产/检验备忘（SAP MARA.FERTH）
    /// </summary>
    public string? ProductionInspectionMemo { get; set; } = string.Empty;

    /// <summary>
    /// 生产备忘页格式（SAP MARA.FORMT）
    /// </summary>
    public string? ProductionMemoPageFormat { get; set; } = string.Empty;

    /// <summary>
    /// 尺寸/规格（SAP MARA.GROES）
    /// </summary>
    public string? SizeDimensions { get; set; } = string.Empty;

    /// <summary>
    /// 基本物料（材质）（SAP MARA.WRKST）
    /// </summary>
    public string? BasicMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 行业标准描述（SAP MARA.NORMT）
    /// </summary>
    public string? IndustryStandardDescription { get; set; } = string.Empty;

    /// <summary>
    /// 实验室/设计室（SAP MARA.LABOR）
    /// </summary>
    public string? LaboratoryDesignOffice { get; set; } = string.Empty;

    /// <summary>
    /// 采购价值码（SAP MARA.EKWSL）
    /// </summary>
    public string? PurchasingValueKey { get; set; } = string.Empty;

    /// <summary>
    /// 毛重（SAP MARA.BRGEW）
    /// </summary>
    public decimal? GrossWeight { get; set; }

    /// <summary>
    /// 净重（SAP MARA.NTGEW）
    /// </summary>
    public decimal? NetWeight { get; set; }

    /// <summary>
    /// 重量单位（SAP MARA.GEWEI）
    /// </summary>
    public string? WeightUnit { get; set; } = string.Empty;

    /// <summary>
    /// 体积（SAP MARA.VOLUM）
    /// </summary>
    public decimal? Volume { get; set; }

    /// <summary>
    /// 体积单位（SAP MARA.VOLEH）
    /// </summary>
    public string? VolumeUnit { get; set; } = string.Empty;

    /// <summary>
    /// 容器要求（SAP MARA.BEHVO）
    /// </summary>
    public string? ContainerRequirements { get; set; } = string.Empty;

    /// <summary>
    /// 仓储条件（SAP MARA.RAUBE）
    /// </summary>
    public string? StorageConditions { get; set; } = string.Empty;

    /// <summary>
    /// 温度条件（SAP MARA.TEMPB）
    /// </summary>
    public string? TemperatureConditions { get; set; } = string.Empty;

    /// <summary>
    /// 低层码（SAP MARA.DISST）
    /// </summary>
    public string? LowLevelCode { get; set; } = string.Empty;

    /// <summary>
    /// 运输组（SAP MARA.TRAGR）
    /// </summary>
    public string? TransportationGroup { get; set; } = string.Empty;

    /// <summary>
    /// 危险品编码（SAP MARA.STOFF）
    /// </summary>
    public string? HazardousMaterialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 产品组（SAP MARA.SPART）
    /// </summary>
    public string? Division { get; set; } = string.Empty;

    /// <summary>
    /// 竞争对手（SAP MARA.KUNNR）
    /// </summary>
    public string? Competitor { get; set; } = string.Empty;

    /// <summary>
    /// 欧洲商品号（旧）（SAP MARA.EANNR）
    /// </summary>
    public string? EuropeanArticleNumberObsolete { get; set; } = string.Empty;

    /// <summary>
    /// 收发货凭证打印数量（SAP MARA.WESCH）
    /// </summary>
    public decimal? GrGiSlipQuantity { get; set; }

    /// <summary>
    /// 采购规则（SAP MARA.BWVOR）
    /// </summary>
    public string? ProcurementRule { get; set; } = string.Empty;

    /// <summary>
    /// 货源（SAP MARA.BWSCL）
    /// </summary>
    public string? SourceOfSupply { get; set; } = string.Empty;

    /// <summary>
    /// 季节类别（SAP MARA.SAISO）
    /// </summary>
    public string? SeasonCategory { get; set; } = string.Empty;

    /// <summary>
    /// 标签类型（SAP MARA.ETIAR）
    /// </summary>
    public string? LabelType { get; set; } = string.Empty;

    /// <summary>
    /// 标签格式（SAP MARA.ETIFO）
    /// </summary>
    public string? LabelForm { get; set; } = string.Empty;

    /// <summary>
    /// 已停用字段（SAP MARA.ENTAR）
    /// </summary>
    public string? DeactivatedField { get; set; } = string.Empty;

    /// <summary>
    /// 国际商品编码EAN/UPC（SAP MARA.EAN11）
    /// </summary>
    public string? InternationalArticleNumber { get; set; } = string.Empty;

    /// <summary>
    /// EAN类别（SAP MARA.NUMTP）
    /// </summary>
    public string? EanCategory { get; set; } = string.Empty;

    /// <summary>
    /// 长度（SAP MARA.LAENG）
    /// </summary>
    public decimal? Length { get; set; }

    /// <summary>
    /// 宽度（SAP MARA.BREIT）
    /// </summary>
    public decimal? Width { get; set; }

    /// <summary>
    /// 高度（SAP MARA.HOEHE）
    /// </summary>
    public decimal? Height { get; set; }

    /// <summary>
    /// 长宽高单位（SAP MARA.MEABM）
    /// </summary>
    public string? DimensionUnit { get; set; } = string.Empty;

    /// <summary>
    /// 产品层次（SAP MARA.PRDHA）
    /// </summary>
    public string? ProductHierarchy { get; set; } = string.Empty;

    /// <summary>
    /// 库存调拨净更改成本核算（SAP MARA.AEKLK）
    /// </summary>
    public string? StockTransferNetChangeCosting { get; set; } = string.Empty;

    /// <summary>
    /// CAD标识（SAP MARA.CADKZ）
    /// </summary>
    public string? CadIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 采购QM激活（SAP MARA.QMPUR）
    /// </summary>
    public string? QmInProcurement { get; set; } = string.Empty;

    /// <summary>
    /// 允许包装重量（SAP MARA.ERGEW）
    /// </summary>
    public decimal? AllowedPackagingWeight { get; set; }

    /// <summary>
    /// 允许包装重量单位（SAP MARA.ERGEI）
    /// </summary>
    public string? AllowedPackagingWeightUnit { get; set; } = string.Empty;

    /// <summary>
    /// 允许包装体积（SAP MARA.ERVOL）
    /// </summary>
    public decimal? AllowedPackagingVolume { get; set; }

    /// <summary>
    /// 允许包装体积单位（SAP MARA.ERVOE）
    /// </summary>
    public string? AllowedPackagingVolumeUnit { get; set; } = string.Empty;

    /// <summary>
    /// 超重容差（SAP MARA.GEWTO）
    /// </summary>
    public decimal? ExcessWeightTolerance { get; set; }

    /// <summary>
    /// 超体积容差（SAP MARA.VOLTO）
    /// </summary>
    public decimal? ExcessVolumeTolerance { get; set; }

    /// <summary>
    /// 可变采购订单单位（SAP MARA.VABME）
    /// </summary>
    public string? VariablePurchaseOrderUnit { get; set; } = string.Empty;

    /// <summary>
    /// 已分配修订级别（SAP MARA.KZREV）
    /// </summary>
    public string? RevisionLevelAssigned { get; set; } = string.Empty;

    /// <summary>
    /// 可配置物料（SAP MARA.KZKFG）
    /// </summary>
    public string? ConfigurableMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 批次管理要求（SAP MARA.XCHPF）
    /// </summary>
    public string? BatchManagementRequired { get; set; } = string.Empty;

    /// <summary>
    /// 包装物料类型（SAP MARA.VHART）
    /// </summary>
    public string? PackagingMaterialType { get; set; } = string.Empty;

    /// <summary>
    /// 最大装载量（体积）（SAP MARA.FUELG）
    /// </summary>
    public decimal? MaximumLevelByVolume { get; set; }

    /// <summary>
    /// 堆叠因子（SAP MARA.STFAK）
    /// </summary>
    public int? StackingFactor { get; set; }

    /// <summary>
    /// 包装物料组（SAP MARA.MAGRV）
    /// </summary>
    public string? PackagingMaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 权限组（SAP MARA.BEGRU）
    /// </summary>
    public string? AuthorizationGroup { get; set; } = string.Empty;

    /// <summary>
    /// 有效起始日期（SAP MARA.DATAB）
    /// </summary>
    public DateTime? ValidFromDate { get; set; }

    /// <summary>
    /// 季节年份（SAP MARA.SAISJ）
    /// </summary>
    public string? SeasonYear { get; set; } = string.Empty;

    /// <summary>
    /// 价格带类别（SAP MARA.PLGTP）
    /// </summary>
    public string? PriceBandCategory { get; set; } = string.Empty;

    /// <summary>
    /// 空容器BOM（SAP MARA.MLGUT）
    /// </summary>
    public string? EmptiesBillOfMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 外部物料组（SAP MARA.EXTWG）
    /// </summary>
    public string? ExternalMaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 跨工厂可配置物料（SAP MARA.SATNR）
    /// </summary>
    public string? CrossPlantConfigurableMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 物料类别（SAP MARA.ATTYP）
    /// </summary>
    public string? MaterialCategory { get; set; } = string.Empty;

    /// <summary>
    /// 联产品标识（SAP MARA.KZKUP）
    /// </summary>
    public string? CoProductIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 后续物料标识（SAP MARA.KZNFM）
    /// </summary>
    public string? FollowUpMaterialIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 定价参考物料（SAP MARA.PMATA）
    /// </summary>
    public string? PricingReferenceMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 跨工厂物料状态（SAP MARA.MSTAE）
    /// </summary>
    public string? CrossPlantMaterialStatus { get; set; } = string.Empty;

    /// <summary>
    /// 跨分销链物料状态（SAP MARA.MSTAV）
    /// </summary>
    public string? CrossDistributionChainStatus { get; set; } = string.Empty;

    /// <summary>
    /// 跨工厂状态生效日期（SAP MARA.MSTDE）
    /// </summary>
    public DateTime? CrossPlantStatusValidFrom { get; set; }

    /// <summary>
    /// 跨分销链状态生效日期（SAP MARA.MSTDV）
    /// </summary>
    public DateTime? CrossDistributionStatusValidFrom { get; set; }

    /// <summary>
    /// 物料税分类（SAP MARA.TAKLV）
    /// </summary>
    public string? TaxClassification { get; set; } = string.Empty;

    /// <summary>
    /// 目录参数文件（SAP MARA.RBNRM）
    /// </summary>
    public string? CatalogProfile { get; set; } = string.Empty;

    /// <summary>
    /// 最短剩余货架寿命（SAP MARA.MHDRZ）
    /// </summary>
    public decimal? MinimumRemainingShelfLife { get; set; }

    /// <summary>
    /// 总货架寿命（SAP MARA.MHDHB）
    /// </summary>
    public decimal? TotalShelfLife { get; set; }

    /// <summary>
    /// 仓储百分比（SAP MARA.MHDLP）
    /// </summary>
    public decimal? StoragePercentage { get; set; }

    /// <summary>
    /// 含量单位（SAP MARA.INHME）
    /// </summary>
    public string? ContentUnit { get; set; } = string.Empty;

    /// <summary>
    /// 净含量（SAP MARA.INHAL）
    /// </summary>
    public decimal? NetContents { get; set; }

    /// <summary>
    /// 比较价格单位（SAP MARA.VPREH）
    /// </summary>
    public decimal? ComparisonPriceUnit { get; set; }

    /// <summary>
    /// 标签物料分组（SAP MARA.ETIAG）
    /// </summary>
    public string? LabelingMaterialGrouping { get; set; } = string.Empty;

    /// <summary>
    /// 毛含量（SAP MARA.INHBR）
    /// </summary>
    public decimal? GrossContents { get; set; }

    /// <summary>
    /// 数量换算方法（SAP MARA.CMETH）
    /// </summary>
    public string? QuantityConversionMethod { get; set; } = string.Empty;

    /// <summary>
    /// 内部对象号（SAP MARA.CUOBF）
    /// </summary>
    public string? InternalObjectNumber { get; set; } = string.Empty;

    /// <summary>
    /// 环境相关（SAP MARA.KZUMW）
    /// </summary>
    public string? EnvironmentallyRelevant { get; set; } = string.Empty;

    /// <summary>
    /// 产品分配确定过程（SAP MARA.KOSCH）
    /// </summary>
    public string? ProductAllocationProcedure { get; set; } = string.Empty;

    /// <summary>
    /// 变式定价参数文件（SAP MARA.SPROF）
    /// </summary>
    public string? VariantPricingProfile { get; set; } = string.Empty;

    /// <summary>
    /// 实物折扣资格（SAP MARA.NRFHG）
    /// </summary>
    public string? DiscountInKind { get; set; } = string.Empty;

    /// <summary>
    /// 制造商零件号（SAP MARA.MFRPN）
    /// </summary>
    public string? ManufacturerPartNumber { get; set; } = string.Empty;

    /// <summary>
    /// 制造商编码（SAP MARA.MFRNR）
    /// </summary>
    public string? ManufacturerNumber { get; set; } = string.Empty;

    /// <summary>
    /// 自有库存管理物料号（SAP MARA.BMATN）
    /// </summary>
    public string? InventoryManagedMaterialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 制造商零件参数文件（SAP MARA.MPROF）
    /// </summary>
    public string? ManufacturerPartProfile { get; set; } = string.Empty;

    /// <summary>
    /// 计量单位用途（SAP MARA.KZWSM）
    /// </summary>
    public string? UnitsOfMeasureUsage { get; set; } = string.Empty;

    /// <summary>
    /// 季节推出（SAP MARA.SAITY）
    /// </summary>
    public string? SeasonRollout { get; set; } = string.Empty;

    /// <summary>
    /// 危险品参数文件（SAP MARA.PROFL）
    /// </summary>
    public string? DangerousGoodsProfile { get; set; } = string.Empty;

    /// <summary>
    /// 高粘度（SAP MARA.IHIVI）
    /// </summary>
    public string? HighlyViscous { get; set; } = string.Empty;

    /// <summary>
    /// 散装/液体（SAP MARA.ILOOS）
    /// </summary>
    public string? InBulkLiquid { get; set; } = string.Empty;

    /// <summary>
    /// 序列号明确级别（SAP MARA.SERLV）
    /// </summary>
    public string? SerialNumberExplicitness { get; set; } = string.Empty;

    /// <summary>
    /// 封闭包装（SAP MARA.KZGVH）
    /// </summary>
    public string? ClosedPackaging { get; set; } = string.Empty;

    /// <summary>
    /// 需批准批次记录（SAP MARA.XGCHP）
    /// </summary>
    public string? ApprovedBatchRecordRequired { get; set; } = string.Empty;

    /// <summary>
    /// 有效性参数覆盖（SAP MARA.KZEFF）
    /// </summary>
    public string? EffectivityParameterOverride { get; set; } = string.Empty;

    /// <summary>
    /// 物料完成级别（SAP MARA.COMPL）
    /// </summary>
    public string? MaterialCompletionLevel { get; set; } = string.Empty;

    /// <summary>
    /// 货架寿命期间标识（SAP MARA.IPRKZ）
    /// </summary>
    public string? ShelfLifePeriodIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 货架寿命舍入规则（SAP MARA.RDMHD）
    /// </summary>
    public string? ShelfLifeRoundingRule { get; set; } = string.Empty;

    /// <summary>
    /// 包装打印产品成分（SAP MARA.PRZUS）
    /// </summary>
    public string? ProductCompositionOnPackaging { get; set; } = string.Empty;

    /// <summary>
    /// 通用项目类别组（SAP MARA.MTPOS_MARA）
    /// </summary>
    public string? GeneralItemCategoryGroup { get; set; } = string.Empty;

    /// <summary>
    /// 后勤变式通用物料（SAP MARA.BFLME）
    /// </summary>
    public string? LogisticalVariants { get; set; } = string.Empty;

    /// <summary>
    /// 物料锁定（SAP MARA.MATFI）
    /// </summary>
    public string? MaterialLocked { get; set; } = string.Empty;

    /// <summary>
    /// 配置管理相关（SAP MARA.CMREL）
    /// </summary>
    public string? ConfigurationManagementRelevant { get; set; } = string.Empty;

    /// <summary>
    /// 品种清单类型（SAP MARA.BBTYP）
    /// </summary>
    public string? AssortmentListType { get; set; } = string.Empty;

    /// <summary>
    /// 到期日期类型（SAP MARA.SLED_BBD）
    /// </summary>
    public string? ExpirationDateType { get; set; } = string.Empty;

    /// <summary>
    /// GTIN变式（SAP MARA.GTIN_VARIANT）
    /// </summary>
    public string? GtinVariant { get; set; } = string.Empty;

    /// <summary>
    /// 通用物料号（SAP MARA.GENNR）
    /// </summary>
    public string? GenericMaterialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 相同包装参考物料（SAP MARA.RMATP）
    /// </summary>
    public string? SamePackingReferenceMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 全球数据同步相关（SAP MARA.GDS_RELEVANT）
    /// </summary>
    public string? GlobalDataSyncRelevant { get; set; } = string.Empty;

    /// <summary>
    /// 原产地验收（SAP MARA.WEORA）
    /// </summary>
    public string? AcceptanceAtOrigin { get; set; } = string.Empty;

    /// <summary>
    /// 标准HU类型（SAP MARA.HUTYP_DFLT）
    /// </summary>
    public string? StandardHuType { get; set; } = string.Empty;

    /// <summary>
    /// 易被盗（SAP MARA.PILFERABLE）
    /// </summary>
    public string? Pilferable { get; set; } = string.Empty;

    /// <summary>
    /// 仓储存储条件（SAP MARA.WHSTC）
    /// </summary>
    public string? WarehouseStorageCondition { get; set; } = string.Empty;

    /// <summary>
    /// 仓储物料组（SAP MARA.WHMATGR）
    /// </summary>
    public string? WarehouseMaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 处理标识（SAP MARA.HNDLCODE）
    /// </summary>
    public string? HandlingIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 危险物质相关（SAP MARA.HAZMAT）
    /// </summary>
    public string? HazardousSubstancesRelevant { get; set; } = string.Empty;

    /// <summary>
    /// 处理单元类型（SAP MARA.HUTYP）
    /// </summary>
    public string? HandlingUnitType { get; set; } = string.Empty;

    /// <summary>
    /// 可变皮重（SAP MARA.TARE_VAR）
    /// </summary>
    public string? VariableTareWeight { get; set; } = string.Empty;

    /// <summary>
    /// 最大允许容量（SAP MARA.MAXC）
    /// </summary>
    public decimal? MaximumAllowedCapacity { get; set; }

    /// <summary>
    /// 超容量容差（SAP MARA.MAXC_TOL）
    /// </summary>
    public decimal? OvercapacityTolerance { get; set; }

    /// <summary>
    /// 最大包装长度（SAP MARA.MAXL）
    /// </summary>
    public decimal? MaximumPackingLength { get; set; }

    /// <summary>
    /// 最大包装宽度（SAP MARA.MAXB）
    /// </summary>
    public decimal? MaximumPackingWidth { get; set; }

    /// <summary>
    /// 最大包装高度（SAP MARA.MAXH）
    /// </summary>
    public decimal? MaximumPackingHeight { get; set; }

    /// <summary>
    /// 最大包装尺寸单位（SAP MARA.MAXDIM_UOM）
    /// </summary>
    public string? MaximumPackingDimensionUnit { get; set; } = string.Empty;

    /// <summary>
    /// 原产国（SAP MARA.HERKL）
    /// </summary>
    public string? CountryOfOrigin { get; set; } = string.Empty;

    /// <summary>
    /// 物料运费组（SAP MARA.MFRGR）
    /// </summary>
    public string? MaterialFreightGroup { get; set; } = string.Empty;

    /// <summary>
    /// 隔离期（SAP MARA.QQTIME）
    /// </summary>
    public decimal? QuarantinePeriod { get; set; }

    /// <summary>
    /// 隔离期单位（SAP MARA.QQTIMEUOM）
    /// </summary>
    public string? QuarantinePeriodUnit { get; set; } = string.Empty;

    /// <summary>
    /// 质检组（SAP MARA.QGRP）
    /// </summary>
    public string? QualityInspectionGroup { get; set; } = string.Empty;

    /// <summary>
    /// 序列号参数文件（SAP MARA.SERIAL）
    /// </summary>
    public string? SerialNumberProfile { get; set; } = string.Empty;

    /// <summary>
    /// 表单名称（SAP MARA.PS_SMARTFORM）
    /// </summary>
    public string? FormName { get; set; } = string.Empty;

    /// <summary>
    /// 后勤计量单位（SAP MARA.LOGUNIT）
    /// </summary>
    public string? LogisticsUnitOfMeasure { get; set; } = string.Empty;

    /// <summary>
    /// 捕捞重量物料（SAP MARA.CWQREL）
    /// </summary>
    public string? CatchWeightMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 捕捞重量参数文件（SAP MARA.CWQPROC）
    /// </summary>
    public string? CatchWeightProfile { get; set; } = string.Empty;

    /// <summary>
    /// 捕捞重量容差组（SAP MARA.CWQTOLGR）
    /// </summary>
    public string? CatchWeightToleranceGroup { get; set; } = string.Empty;

    /// <summary>
    /// 调整参数文件（SAP MARA.ADPROF）
    /// </summary>
    public string? AdjustmentProfile { get; set; } = string.Empty;

    /// <summary>
    /// 知识产权ID（SAP MARA.IPMIPPRODUCT）
    /// </summary>
    public string? IntellectualPropertyId { get; set; } = string.Empty;

    /// <summary>
    /// 知识产权名称（填充字段）
    /// </summary>
    public string? IntellectualPropertyName { get; set; }

    /// <summary>
    /// 允许变式价格（SAP MARA.ALLOW_PMAT_IGNO）
    /// </summary>
    public string? VariantPriceAllowed { get; set; } = string.Empty;

    /// <summary>
    /// 介质（SAP MARA.MEDIUM）
    /// </summary>
    public string? Medium { get; set; } = string.Empty;

    /// <summary>
    /// 实物商品（SAP MARA.COMMODITY）
    /// </summary>
    public string? PhysicalCommodity { get; set; } = string.Empty;

    /// <summary>
    /// 动物源（SAP MARA.ANIMAL_ORIGIN）
    /// </summary>
    public string? AnimalOrigin { get; set; } = string.Empty;

    /// <summary>
    /// 纺织成分功能（SAP MARA.TEXTILE_COMP_IND）
    /// </summary>
    public string? TextileCompositionFunction { get; set; } = string.Empty;

    /// <summary>
    /// 细分结构（SAP MARA.SGT_CSGR）
    /// </summary>
    public string? SegmentationStructure { get; set; } = string.Empty;

    /// <summary>
    /// 细分策略（SAP MARA.SGT_COVSA）
    /// </summary>
    public string? SegmentationStrategy { get; set; } = string.Empty;

    /// <summary>
    /// 细分状态（SAP MARA.SGT_STAT）
    /// </summary>
    public string? SegmentationStatus { get; set; } = string.Empty;

    /// <summary>
    /// 细分范围（SAP MARA.SGT_SCOPE）
    /// </summary>
    public string? SegmentationScope { get; set; } = string.Empty;

    /// <summary>
    /// 细分相关（SAP MARA.SGT_REL）
    /// </summary>
    public string? SegmentationRelevant { get; set; } = string.Empty;

    /// <summary>
    /// 时装属性1（SAP MARA.FSH_MG_AT1）
    /// </summary>
    public string? FashionAttribute1 { get; set; } = string.Empty;

    /// <summary>
    /// 时装属性2（SAP MARA.FSH_MG_AT2）
    /// </summary>
    public string? FashionAttribute2 { get; set; } = string.Empty;

    /// <summary>
    /// 时装属性3（SAP MARA.FSH_MG_AT3）
    /// </summary>
    public string? FashionAttribute3 { get; set; } = string.Empty;

    /// <summary>
    /// 季节使用标识（SAP MARA.FSH_SEALV）
    /// </summary>
    public string? SeasonUsageIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 库存季节激活（SAP MARA.FSH_SEAIM）
    /// </summary>
    public string? SeasonActiveInInventory { get; set; } = string.Empty;

    /// <summary>
    /// 特性转换ID（SAP MARA.FSH_SC_MID）
    /// </summary>
    public string? CharacteristicConversionId { get; set; } = string.Empty;

    /// <summary>
    /// 特性转换名称（填充字段）
    /// </summary>
    public string? CharacteristicConversionName { get; set; }

    /// <summary>
    /// ANP代码（SAP MARA.ANP）
    /// </summary>
    public string? AnpCode { get; set; } = string.Empty;

    /// <summary>
    /// 危险品包装状态（SAP MARA.DG_PACK_STATUS）
    /// </summary>
    public string? DangerousGoodsPackagingStatus { get; set; } = string.Empty;

    /// <summary>
    /// 物料条件管理（SAP MARA.MCOND）
    /// </summary>
    public string? MaterialConditionManagement { get; set; } = string.Empty;

    /// <summary>
    /// 退货代码（SAP MARA.RETDELC）
    /// </summary>
    public string? ReturnCode { get; set; } = string.Empty;

    /// <summary>
    /// 退回后勤级别（SAP MARA.LOGLEV_RETO）
    /// </summary>
    public string? ReturnToLogisticsLevel { get; set; } = string.Empty;

    /// <summary>
    /// NATO物料识别号（SAP MARA.NSNID）
    /// </summary>
    public string? NatoItemIdentificationNumber { get; set; } = string.Empty;

    /// <summary>
    /// FFF类别（SAP MARA.IMATN）
    /// </summary>
    public string? FffClass { get; set; } = string.Empty;

    /// <summary>
    /// 替代链编码（SAP MARA.PICNUM）
    /// </summary>
    public string? SupersessionChainNumber { get; set; } = string.Empty;

    /// <summary>
    /// 季节采购创建状态（SAP MARA.BSTAT）
    /// </summary>
    public string? SeasonalProcurementCreationStatus { get; set; } = string.Empty;

    /// <summary>
    /// 颜色特性内部号（SAP MARA.COLOR_ATINN）
    /// </summary>
    public string? ColorCharacteristicInternalNumber { get; set; } = string.Empty;

    /// <summary>
    /// 主尺码特性内部号（SAP MARA.SIZE1_ATINN）
    /// </summary>
    public string? MainSizeCharacteristicInternalNumber { get; set; } = string.Empty;

    /// <summary>
    /// 次尺码特性内部号（SAP MARA.SIZE2_ATINN）
    /// </summary>
    public string? SecondSizeCharacteristicInternalNumber { get; set; } = string.Empty;

    /// <summary>
    /// 颜色（SAP MARA.COLOR）
    /// </summary>
    public string? Color { get; set; } = string.Empty;

    /// <summary>
    /// 主尺码（SAP MARA.SIZE1）
    /// </summary>
    public string? MainSize { get; set; } = string.Empty;

    /// <summary>
    /// 次尺码（SAP MARA.SIZE2）
    /// </summary>
    public string? SecondSize { get; set; } = string.Empty;

    /// <summary>
    /// 评估特性值（SAP MARA.FREE_CHAR）
    /// </summary>
    public string? EvaluationCharacteristicValue { get; set; } = string.Empty;

    /// <summary>
    /// 护理代码（SAP MARA.CARE_CODE）
    /// </summary>
    public string? CareCode { get; set; } = string.Empty;

    /// <summary>
    /// 品牌（SAP MARA.BRAND_ID）
    /// </summary>
    public string? BrandId { get; set; } = string.Empty;

    /// <summary>
    /// 品牌（SAP MARA.BRAND_名称（填充字段）
    /// </summary>
    public string? BrandName { get; set; }

    /// <summary>
    /// 纤维代码1（SAP MARA.FIBER_CODE1）
    /// </summary>
    public string? FiberCode1 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维占比1（SAP MARA.FIBER_PART1）
    /// </summary>
    public string? FiberPart1 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维代码2（SAP MARA.FIBER_CODE2）
    /// </summary>
    public string? FiberCode2 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维占比2（SAP MARA.FIBER_PART2）
    /// </summary>
    public string? FiberPart2 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维代码3（SAP MARA.FIBER_CODE3）
    /// </summary>
    public string? FiberCode3 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维占比3（SAP MARA.FIBER_PART3）
    /// </summary>
    public string? FiberPart3 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维代码4（SAP MARA.FIBER_CODE4）
    /// </summary>
    public string? FiberCode4 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维占比4（SAP MARA.FIBER_PART4）
    /// </summary>
    public string? FiberPart4 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维代码5（SAP MARA.FIBER_CODE5）
    /// </summary>
    public string? FiberCode5 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维占比5（SAP MARA.FIBER_PART5）
    /// </summary>
    public string? FiberPart5 { get; set; } = string.Empty;

    /// <summary>
    /// 时装等级（SAP MARA.FASHGRD）
    /// </summary>
    public string? FashionGrade { get; set; } = string.Empty;

    /// <summary>
    /// 物料状态（字典 sys_normal_disable_status；0=禁用，1=启用，2=锁定；平台启用态，非 SAP MSTAE）
    /// </summary>
    public int MaterialStatus { get; set; } = 0;

    /// <summary>
    /// 多语言描述列表（主子表关系；对齐 SAP MAKT）
    /// （子表：TaktMaterialDescription）
    /// </summary>
    public List<TaktMaterialDescriptionDto>? Descriptions { get; set; }

}

// ========================================
// Material 查询 DTO
// ========================================

/// <summary>
/// Material 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktMaterialQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（SAP MARA.MATNR）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 完整维护状态（SAP MARA.VPSTA）
    /// </summary>
    public string? CompleteMaintenanceStatus { get; set; } = string.Empty;

    /// <summary>
    /// 维护状态（SAP MARA.PSTAT）
    /// </summary>
    public string? MaintenanceStatus { get; set; } = string.Empty;

    /// <summary>
    /// 客户级删除标记（SAP MARA.LVORM）
    /// </summary>
    public string? ClientDeletionFlag { get; set; } = string.Empty;

    /// <summary>
    /// 物料类型（SAP MARA.MTART）
    /// </summary>
    public string? MaterialType { get; set; } = string.Empty;

    /// <summary>
    /// 行业领域（SAP MARA.MBRSH）
    /// </summary>
    public string? IndustrySector { get; set; } = string.Empty;

    /// <summary>
    /// 物料组（SAP MARA.MATKL）
    /// </summary>
    public string? MaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料号（SAP MARA.BISMT）
    /// </summary>
    public string? OldMaterialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 基本计量单位（SAP MARA.MEINS）
    /// </summary>
    public string? BaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单单位（SAP MARA.BSTME）
    /// </summary>
    public string? OrderUnit { get; set; } = string.Empty;

    /// <summary>
    /// 单据号（SAP MARA.ZEINR）
    /// </summary>
    public string? DocumentNumber { get; set; } = string.Empty;

    /// <summary>
    /// 单据类型（SAP MARA.ZEIAR）
    /// </summary>
    public string? DocumentType { get; set; } = string.Empty;

    /// <summary>
    /// 单据版本（SAP MARA.ZEIVR）
    /// </summary>
    public string? DocumentVersion { get; set; } = string.Empty;

    /// <summary>
    /// 单据页格式（SAP MARA.ZEIFO）
    /// </summary>
    public string? DocumentPageFormat { get; set; } = string.Empty;

    /// <summary>
    /// 单据更改号（SAP MARA.AESZN）
    /// </summary>
    public string? DocumentChangeNumber { get; set; } = string.Empty;

    /// <summary>
    /// 单据页号（SAP MARA.BLATT）
    /// </summary>
    public string? DocumentPageNumber { get; set; } = string.Empty;

    /// <summary>
    /// 单据页数（SAP MARA.BLANZ）
    /// </summary>
    public string? DocumentSheetCount { get; set; } = string.Empty;

    /// <summary>
    /// 生产/检验备忘（SAP MARA.FERTH）
    /// </summary>
    public string? ProductionInspectionMemo { get; set; } = string.Empty;

    /// <summary>
    /// 生产备忘页格式（SAP MARA.FORMT）
    /// </summary>
    public string? ProductionMemoPageFormat { get; set; } = string.Empty;

    /// <summary>
    /// 尺寸/规格（SAP MARA.GROES）
    /// </summary>
    public string? SizeDimensions { get; set; } = string.Empty;

    /// <summary>
    /// 基本物料（材质）（SAP MARA.WRKST）
    /// </summary>
    public string? BasicMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 行业标准描述（SAP MARA.NORMT）
    /// </summary>
    public string? IndustryStandardDescription { get; set; } = string.Empty;

    /// <summary>
    /// 实验室/设计室（SAP MARA.LABOR）
    /// </summary>
    public string? LaboratoryDesignOffice { get; set; } = string.Empty;

    /// <summary>
    /// 采购价值码（SAP MARA.EKWSL）
    /// </summary>
    public string? PurchasingValueKey { get; set; } = string.Empty;

    /// <summary>
    /// 毛重（SAP MARA.BRGEW）
    /// </summary>
    public decimal? GrossWeight { get; set; }

    /// <summary>
    /// 净重（SAP MARA.NTGEW）
    /// </summary>
    public decimal? NetWeight { get; set; }

    /// <summary>
    /// 重量单位（SAP MARA.GEWEI）
    /// </summary>
    public string? WeightUnit { get; set; } = string.Empty;

    /// <summary>
    /// 体积（SAP MARA.VOLUM）
    /// </summary>
    public decimal? Volume { get; set; }

    /// <summary>
    /// 体积单位（SAP MARA.VOLEH）
    /// </summary>
    public string? VolumeUnit { get; set; } = string.Empty;

    /// <summary>
    /// 容器要求（SAP MARA.BEHVO）
    /// </summary>
    public string? ContainerRequirements { get; set; } = string.Empty;

    /// <summary>
    /// 仓储条件（SAP MARA.RAUBE）
    /// </summary>
    public string? StorageConditions { get; set; } = string.Empty;

    /// <summary>
    /// 温度条件（SAP MARA.TEMPB）
    /// </summary>
    public string? TemperatureConditions { get; set; } = string.Empty;

    /// <summary>
    /// 低层码（SAP MARA.DISST）
    /// </summary>
    public string? LowLevelCode { get; set; } = string.Empty;

    /// <summary>
    /// 运输组（SAP MARA.TRAGR）
    /// </summary>
    public string? TransportationGroup { get; set; } = string.Empty;

    /// <summary>
    /// 危险品编码（SAP MARA.STOFF）
    /// </summary>
    public string? HazardousMaterialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 产品组（SAP MARA.SPART）
    /// </summary>
    public string? Division { get; set; } = string.Empty;

    /// <summary>
    /// 竞争对手（SAP MARA.KUNNR）
    /// </summary>
    public string? Competitor { get; set; } = string.Empty;

    /// <summary>
    /// 欧洲商品号（旧）（SAP MARA.EANNR）
    /// </summary>
    public string? EuropeanArticleNumberObsolete { get; set; } = string.Empty;

    /// <summary>
    /// 收发货凭证打印数量（SAP MARA.WESCH）
    /// </summary>
    public decimal? GrGiSlipQuantity { get; set; }

    /// <summary>
    /// 采购规则（SAP MARA.BWVOR）
    /// </summary>
    public string? ProcurementRule { get; set; } = string.Empty;

    /// <summary>
    /// 货源（SAP MARA.BWSCL）
    /// </summary>
    public string? SourceOfSupply { get; set; } = string.Empty;

    /// <summary>
    /// 季节类别（SAP MARA.SAISO）
    /// </summary>
    public string? SeasonCategory { get; set; } = string.Empty;

    /// <summary>
    /// 标签类型（SAP MARA.ETIAR）
    /// </summary>
    public string? LabelType { get; set; } = string.Empty;

    /// <summary>
    /// 标签格式（SAP MARA.ETIFO）
    /// </summary>
    public string? LabelForm { get; set; } = string.Empty;

    /// <summary>
    /// 已停用字段（SAP MARA.ENTAR）
    /// </summary>
    public string? DeactivatedField { get; set; } = string.Empty;

    /// <summary>
    /// 国际商品编码EAN/UPC（SAP MARA.EAN11）
    /// </summary>
    public string? InternationalArticleNumber { get; set; } = string.Empty;

    /// <summary>
    /// EAN类别（SAP MARA.NUMTP）
    /// </summary>
    public string? EanCategory { get; set; } = string.Empty;

    /// <summary>
    /// 长度（SAP MARA.LAENG）
    /// </summary>
    public decimal? Length { get; set; }

    /// <summary>
    /// 宽度（SAP MARA.BREIT）
    /// </summary>
    public decimal? Width { get; set; }

    /// <summary>
    /// 高度（SAP MARA.HOEHE）
    /// </summary>
    public decimal? Height { get; set; }

    /// <summary>
    /// 长宽高单位（SAP MARA.MEABM）
    /// </summary>
    public string? DimensionUnit { get; set; } = string.Empty;

    /// <summary>
    /// 产品层次（SAP MARA.PRDHA）
    /// </summary>
    public string? ProductHierarchy { get; set; } = string.Empty;

    /// <summary>
    /// 库存调拨净更改成本核算（SAP MARA.AEKLK）
    /// </summary>
    public string? StockTransferNetChangeCosting { get; set; } = string.Empty;

    /// <summary>
    /// CAD标识（SAP MARA.CADKZ）
    /// </summary>
    public string? CadIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 采购QM激活（SAP MARA.QMPUR）
    /// </summary>
    public string? QmInProcurement { get; set; } = string.Empty;

    /// <summary>
    /// 允许包装重量（SAP MARA.ERGEW）
    /// </summary>
    public decimal? AllowedPackagingWeight { get; set; }

    /// <summary>
    /// 允许包装重量单位（SAP MARA.ERGEI）
    /// </summary>
    public string? AllowedPackagingWeightUnit { get; set; } = string.Empty;

    /// <summary>
    /// 允许包装体积（SAP MARA.ERVOL）
    /// </summary>
    public decimal? AllowedPackagingVolume { get; set; }

    /// <summary>
    /// 允许包装体积单位（SAP MARA.ERVOE）
    /// </summary>
    public string? AllowedPackagingVolumeUnit { get; set; } = string.Empty;

    /// <summary>
    /// 超重容差（SAP MARA.GEWTO）
    /// </summary>
    public decimal? ExcessWeightTolerance { get; set; }

    /// <summary>
    /// 超体积容差（SAP MARA.VOLTO）
    /// </summary>
    public decimal? ExcessVolumeTolerance { get; set; }

    /// <summary>
    /// 可变采购订单单位（SAP MARA.VABME）
    /// </summary>
    public string? VariablePurchaseOrderUnit { get; set; } = string.Empty;

    /// <summary>
    /// 已分配修订级别（SAP MARA.KZREV）
    /// </summary>
    public string? RevisionLevelAssigned { get; set; } = string.Empty;

    /// <summary>
    /// 可配置物料（SAP MARA.KZKFG）
    /// </summary>
    public string? ConfigurableMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 批次管理要求（SAP MARA.XCHPF）
    /// </summary>
    public string? BatchManagementRequired { get; set; } = string.Empty;

    /// <summary>
    /// 包装物料类型（SAP MARA.VHART）
    /// </summary>
    public string? PackagingMaterialType { get; set; } = string.Empty;

    /// <summary>
    /// 最大装载量（体积）（SAP MARA.FUELG）
    /// </summary>
    public decimal? MaximumLevelByVolume { get; set; }

    /// <summary>
    /// 堆叠因子（SAP MARA.STFAK）
    /// </summary>
    public int? StackingFactor { get; set; }

    /// <summary>
    /// 包装物料组（SAP MARA.MAGRV）
    /// </summary>
    public string? PackagingMaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 权限组（SAP MARA.BEGRU）
    /// </summary>
    public string? AuthorizationGroup { get; set; } = string.Empty;

    /// <summary>
    /// 有效起始日期（SAP MARA.DATAB）（范围查询-开始）
    /// </summary>
    public DateTime? ValidFromDateStart { get; set; }

    /// <summary>
    /// 有效起始日期（SAP MARA.DATAB）（范围查询-结束）
    /// </summary>
    public DateTime? ValidFromDateEnd { get; set; }

    /// <summary>
    /// 季节年份（SAP MARA.SAISJ）
    /// </summary>
    public string? SeasonYear { get; set; } = string.Empty;

    /// <summary>
    /// 价格带类别（SAP MARA.PLGTP）
    /// </summary>
    public string? PriceBandCategory { get; set; } = string.Empty;

    /// <summary>
    /// 空容器BOM（SAP MARA.MLGUT）
    /// </summary>
    public string? EmptiesBillOfMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 外部物料组（SAP MARA.EXTWG）
    /// </summary>
    public string? ExternalMaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 跨工厂可配置物料（SAP MARA.SATNR）
    /// </summary>
    public string? CrossPlantConfigurableMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 物料类别（SAP MARA.ATTYP）
    /// </summary>
    public string? MaterialCategory { get; set; } = string.Empty;

    /// <summary>
    /// 联产品标识（SAP MARA.KZKUP）
    /// </summary>
    public string? CoProductIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 后续物料标识（SAP MARA.KZNFM）
    /// </summary>
    public string? FollowUpMaterialIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 定价参考物料（SAP MARA.PMATA）
    /// </summary>
    public string? PricingReferenceMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 跨工厂物料状态（SAP MARA.MSTAE）
    /// </summary>
    public string? CrossPlantMaterialStatus { get; set; } = string.Empty;

    /// <summary>
    /// 跨分销链物料状态（SAP MARA.MSTAV）
    /// </summary>
    public string? CrossDistributionChainStatus { get; set; } = string.Empty;

    /// <summary>
    /// 跨工厂状态生效日期（SAP MARA.MSTDE）（范围查询-开始）
    /// </summary>
    public DateTime? CrossPlantStatusValidFromStart { get; set; }

    /// <summary>
    /// 跨工厂状态生效日期（SAP MARA.MSTDE）（范围查询-结束）
    /// </summary>
    public DateTime? CrossPlantStatusValidFromEnd { get; set; }

    /// <summary>
    /// 跨分销链状态生效日期（SAP MARA.MSTDV）（范围查询-开始）
    /// </summary>
    public DateTime? CrossDistributionStatusValidFromStart { get; set; }

    /// <summary>
    /// 跨分销链状态生效日期（SAP MARA.MSTDV）（范围查询-结束）
    /// </summary>
    public DateTime? CrossDistributionStatusValidFromEnd { get; set; }

    /// <summary>
    /// 物料税分类（SAP MARA.TAKLV）
    /// </summary>
    public string? TaxClassification { get; set; } = string.Empty;

    /// <summary>
    /// 目录参数文件（SAP MARA.RBNRM）
    /// </summary>
    public string? CatalogProfile { get; set; } = string.Empty;

    /// <summary>
    /// 最短剩余货架寿命（SAP MARA.MHDRZ）
    /// </summary>
    public decimal? MinimumRemainingShelfLife { get; set; }

    /// <summary>
    /// 总货架寿命（SAP MARA.MHDHB）
    /// </summary>
    public decimal? TotalShelfLife { get; set; }

    /// <summary>
    /// 仓储百分比（SAP MARA.MHDLP）
    /// </summary>
    public decimal? StoragePercentage { get; set; }

    /// <summary>
    /// 含量单位（SAP MARA.INHME）
    /// </summary>
    public string? ContentUnit { get; set; } = string.Empty;

    /// <summary>
    /// 净含量（SAP MARA.INHAL）
    /// </summary>
    public decimal? NetContents { get; set; }

    /// <summary>
    /// 比较价格单位（SAP MARA.VPREH）
    /// </summary>
    public decimal? ComparisonPriceUnit { get; set; }

    /// <summary>
    /// 标签物料分组（SAP MARA.ETIAG）
    /// </summary>
    public string? LabelingMaterialGrouping { get; set; } = string.Empty;

    /// <summary>
    /// 毛含量（SAP MARA.INHBR）
    /// </summary>
    public decimal? GrossContents { get; set; }

    /// <summary>
    /// 数量换算方法（SAP MARA.CMETH）
    /// </summary>
    public string? QuantityConversionMethod { get; set; } = string.Empty;

    /// <summary>
    /// 内部对象号（SAP MARA.CUOBF）
    /// </summary>
    public string? InternalObjectNumber { get; set; } = string.Empty;

    /// <summary>
    /// 环境相关（SAP MARA.KZUMW）
    /// </summary>
    public string? EnvironmentallyRelevant { get; set; } = string.Empty;

    /// <summary>
    /// 产品分配确定过程（SAP MARA.KOSCH）
    /// </summary>
    public string? ProductAllocationProcedure { get; set; } = string.Empty;

    /// <summary>
    /// 变式定价参数文件（SAP MARA.SPROF）
    /// </summary>
    public string? VariantPricingProfile { get; set; } = string.Empty;

    /// <summary>
    /// 实物折扣资格（SAP MARA.NRFHG）
    /// </summary>
    public string? DiscountInKind { get; set; } = string.Empty;

    /// <summary>
    /// 制造商零件号（SAP MARA.MFRPN）
    /// </summary>
    public string? ManufacturerPartNumber { get; set; } = string.Empty;

    /// <summary>
    /// 制造商编码（SAP MARA.MFRNR）
    /// </summary>
    public string? ManufacturerNumber { get; set; } = string.Empty;

    /// <summary>
    /// 自有库存管理物料号（SAP MARA.BMATN）
    /// </summary>
    public string? InventoryManagedMaterialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 制造商零件参数文件（SAP MARA.MPROF）
    /// </summary>
    public string? ManufacturerPartProfile { get; set; } = string.Empty;

    /// <summary>
    /// 计量单位用途（SAP MARA.KZWSM）
    /// </summary>
    public string? UnitsOfMeasureUsage { get; set; } = string.Empty;

    /// <summary>
    /// 季节推出（SAP MARA.SAITY）
    /// </summary>
    public string? SeasonRollout { get; set; } = string.Empty;

    /// <summary>
    /// 危险品参数文件（SAP MARA.PROFL）
    /// </summary>
    public string? DangerousGoodsProfile { get; set; } = string.Empty;

    /// <summary>
    /// 高粘度（SAP MARA.IHIVI）
    /// </summary>
    public string? HighlyViscous { get; set; } = string.Empty;

    /// <summary>
    /// 散装/液体（SAP MARA.ILOOS）
    /// </summary>
    public string? InBulkLiquid { get; set; } = string.Empty;

    /// <summary>
    /// 序列号明确级别（SAP MARA.SERLV）
    /// </summary>
    public string? SerialNumberExplicitness { get; set; } = string.Empty;

    /// <summary>
    /// 封闭包装（SAP MARA.KZGVH）
    /// </summary>
    public string? ClosedPackaging { get; set; } = string.Empty;

    /// <summary>
    /// 需批准批次记录（SAP MARA.XGCHP）
    /// </summary>
    public string? ApprovedBatchRecordRequired { get; set; } = string.Empty;

    /// <summary>
    /// 有效性参数覆盖（SAP MARA.KZEFF）
    /// </summary>
    public string? EffectivityParameterOverride { get; set; } = string.Empty;

    /// <summary>
    /// 物料完成级别（SAP MARA.COMPL）
    /// </summary>
    public string? MaterialCompletionLevel { get; set; } = string.Empty;

    /// <summary>
    /// 货架寿命期间标识（SAP MARA.IPRKZ）
    /// </summary>
    public string? ShelfLifePeriodIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 货架寿命舍入规则（SAP MARA.RDMHD）
    /// </summary>
    public string? ShelfLifeRoundingRule { get; set; } = string.Empty;

    /// <summary>
    /// 包装打印产品成分（SAP MARA.PRZUS）
    /// </summary>
    public string? ProductCompositionOnPackaging { get; set; } = string.Empty;

    /// <summary>
    /// 通用项目类别组（SAP MARA.MTPOS_MARA）
    /// </summary>
    public string? GeneralItemCategoryGroup { get; set; } = string.Empty;

    /// <summary>
    /// 后勤变式通用物料（SAP MARA.BFLME）
    /// </summary>
    public string? LogisticalVariants { get; set; } = string.Empty;

    /// <summary>
    /// 物料锁定（SAP MARA.MATFI）
    /// </summary>
    public string? MaterialLocked { get; set; } = string.Empty;

    /// <summary>
    /// 配置管理相关（SAP MARA.CMREL）
    /// </summary>
    public string? ConfigurationManagementRelevant { get; set; } = string.Empty;

    /// <summary>
    /// 品种清单类型（SAP MARA.BBTYP）
    /// </summary>
    public string? AssortmentListType { get; set; } = string.Empty;

    /// <summary>
    /// 到期日期类型（SAP MARA.SLED_BBD）
    /// </summary>
    public string? ExpirationDateType { get; set; } = string.Empty;

    /// <summary>
    /// GTIN变式（SAP MARA.GTIN_VARIANT）
    /// </summary>
    public string? GtinVariant { get; set; } = string.Empty;

    /// <summary>
    /// 通用物料号（SAP MARA.GENNR）
    /// </summary>
    public string? GenericMaterialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 相同包装参考物料（SAP MARA.RMATP）
    /// </summary>
    public string? SamePackingReferenceMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 全球数据同步相关（SAP MARA.GDS_RELEVANT）
    /// </summary>
    public string? GlobalDataSyncRelevant { get; set; } = string.Empty;

    /// <summary>
    /// 原产地验收（SAP MARA.WEORA）
    /// </summary>
    public string? AcceptanceAtOrigin { get; set; } = string.Empty;

    /// <summary>
    /// 标准HU类型（SAP MARA.HUTYP_DFLT）
    /// </summary>
    public string? StandardHuType { get; set; } = string.Empty;

    /// <summary>
    /// 易被盗（SAP MARA.PILFERABLE）
    /// </summary>
    public string? Pilferable { get; set; } = string.Empty;

    /// <summary>
    /// 仓储存储条件（SAP MARA.WHSTC）
    /// </summary>
    public string? WarehouseStorageCondition { get; set; } = string.Empty;

    /// <summary>
    /// 仓储物料组（SAP MARA.WHMATGR）
    /// </summary>
    public string? WarehouseMaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 处理标识（SAP MARA.HNDLCODE）
    /// </summary>
    public string? HandlingIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 危险物质相关（SAP MARA.HAZMAT）
    /// </summary>
    public string? HazardousSubstancesRelevant { get; set; } = string.Empty;

    /// <summary>
    /// 处理单元类型（SAP MARA.HUTYP）
    /// </summary>
    public string? HandlingUnitType { get; set; } = string.Empty;

    /// <summary>
    /// 可变皮重（SAP MARA.TARE_VAR）
    /// </summary>
    public string? VariableTareWeight { get; set; } = string.Empty;

    /// <summary>
    /// 最大允许容量（SAP MARA.MAXC）
    /// </summary>
    public decimal? MaximumAllowedCapacity { get; set; }

    /// <summary>
    /// 超容量容差（SAP MARA.MAXC_TOL）
    /// </summary>
    public decimal? OvercapacityTolerance { get; set; }

    /// <summary>
    /// 最大包装长度（SAP MARA.MAXL）
    /// </summary>
    public decimal? MaximumPackingLength { get; set; }

    /// <summary>
    /// 最大包装宽度（SAP MARA.MAXB）
    /// </summary>
    public decimal? MaximumPackingWidth { get; set; }

    /// <summary>
    /// 最大包装高度（SAP MARA.MAXH）
    /// </summary>
    public decimal? MaximumPackingHeight { get; set; }

    /// <summary>
    /// 最大包装尺寸单位（SAP MARA.MAXDIM_UOM）
    /// </summary>
    public string? MaximumPackingDimensionUnit { get; set; } = string.Empty;

    /// <summary>
    /// 原产国（SAP MARA.HERKL）
    /// </summary>
    public string? CountryOfOrigin { get; set; } = string.Empty;

    /// <summary>
    /// 物料运费组（SAP MARA.MFRGR）
    /// </summary>
    public string? MaterialFreightGroup { get; set; } = string.Empty;

    /// <summary>
    /// 隔离期（SAP MARA.QQTIME）
    /// </summary>
    public decimal? QuarantinePeriod { get; set; }

    /// <summary>
    /// 隔离期单位（SAP MARA.QQTIMEUOM）
    /// </summary>
    public string? QuarantinePeriodUnit { get; set; } = string.Empty;

    /// <summary>
    /// 质检组（SAP MARA.QGRP）
    /// </summary>
    public string? QualityInspectionGroup { get; set; } = string.Empty;

    /// <summary>
    /// 序列号参数文件（SAP MARA.SERIAL）
    /// </summary>
    public string? SerialNumberProfile { get; set; } = string.Empty;

    /// <summary>
    /// 表单名称（SAP MARA.PS_SMARTFORM）
    /// </summary>
    public string? FormName { get; set; } = string.Empty;

    /// <summary>
    /// 后勤计量单位（SAP MARA.LOGUNIT）
    /// </summary>
    public string? LogisticsUnitOfMeasure { get; set; } = string.Empty;

    /// <summary>
    /// 捕捞重量物料（SAP MARA.CWQREL）
    /// </summary>
    public string? CatchWeightMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 捕捞重量参数文件（SAP MARA.CWQPROC）
    /// </summary>
    public string? CatchWeightProfile { get; set; } = string.Empty;

    /// <summary>
    /// 捕捞重量容差组（SAP MARA.CWQTOLGR）
    /// </summary>
    public string? CatchWeightToleranceGroup { get; set; } = string.Empty;

    /// <summary>
    /// 调整参数文件（SAP MARA.ADPROF）
    /// </summary>
    public string? AdjustmentProfile { get; set; } = string.Empty;

    /// <summary>
    /// 知识产权ID（SAP MARA.IPMIPPRODUCT）
    /// </summary>
    public string? IntellectualPropertyId { get; set; } = string.Empty;

    /// <summary>
    /// 允许变式价格（SAP MARA.ALLOW_PMAT_IGNO）
    /// </summary>
    public string? VariantPriceAllowed { get; set; } = string.Empty;

    /// <summary>
    /// 介质（SAP MARA.MEDIUM）
    /// </summary>
    public string? Medium { get; set; } = string.Empty;

    /// <summary>
    /// 实物商品（SAP MARA.COMMODITY）
    /// </summary>
    public string? PhysicalCommodity { get; set; } = string.Empty;

    /// <summary>
    /// 动物源（SAP MARA.ANIMAL_ORIGIN）
    /// </summary>
    public string? AnimalOrigin { get; set; } = string.Empty;

    /// <summary>
    /// 纺织成分功能（SAP MARA.TEXTILE_COMP_IND）
    /// </summary>
    public string? TextileCompositionFunction { get; set; } = string.Empty;

    /// <summary>
    /// 细分结构（SAP MARA.SGT_CSGR）
    /// </summary>
    public string? SegmentationStructure { get; set; } = string.Empty;

    /// <summary>
    /// 细分策略（SAP MARA.SGT_COVSA）
    /// </summary>
    public string? SegmentationStrategy { get; set; } = string.Empty;

    /// <summary>
    /// 细分状态（SAP MARA.SGT_STAT）
    /// </summary>
    public string? SegmentationStatus { get; set; } = string.Empty;

    /// <summary>
    /// 细分范围（SAP MARA.SGT_SCOPE）
    /// </summary>
    public string? SegmentationScope { get; set; } = string.Empty;

    /// <summary>
    /// 细分相关（SAP MARA.SGT_REL）
    /// </summary>
    public string? SegmentationRelevant { get; set; } = string.Empty;

    /// <summary>
    /// 时装属性1（SAP MARA.FSH_MG_AT1）
    /// </summary>
    public string? FashionAttribute1 { get; set; } = string.Empty;

    /// <summary>
    /// 时装属性2（SAP MARA.FSH_MG_AT2）
    /// </summary>
    public string? FashionAttribute2 { get; set; } = string.Empty;

    /// <summary>
    /// 时装属性3（SAP MARA.FSH_MG_AT3）
    /// </summary>
    public string? FashionAttribute3 { get; set; } = string.Empty;

    /// <summary>
    /// 季节使用标识（SAP MARA.FSH_SEALV）
    /// </summary>
    public string? SeasonUsageIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 库存季节激活（SAP MARA.FSH_SEAIM）
    /// </summary>
    public string? SeasonActiveInInventory { get; set; } = string.Empty;

    /// <summary>
    /// 特性转换ID（SAP MARA.FSH_SC_MID）
    /// </summary>
    public string? CharacteristicConversionId { get; set; } = string.Empty;

    /// <summary>
    /// ANP代码（SAP MARA.ANP）
    /// </summary>
    public string? AnpCode { get; set; } = string.Empty;

    /// <summary>
    /// 危险品包装状态（SAP MARA.DG_PACK_STATUS）
    /// </summary>
    public string? DangerousGoodsPackagingStatus { get; set; } = string.Empty;

    /// <summary>
    /// 物料条件管理（SAP MARA.MCOND）
    /// </summary>
    public string? MaterialConditionManagement { get; set; } = string.Empty;

    /// <summary>
    /// 退货代码（SAP MARA.RETDELC）
    /// </summary>
    public string? ReturnCode { get; set; } = string.Empty;

    /// <summary>
    /// 退回后勤级别（SAP MARA.LOGLEV_RETO）
    /// </summary>
    public string? ReturnToLogisticsLevel { get; set; } = string.Empty;

    /// <summary>
    /// NATO物料识别号（SAP MARA.NSNID）
    /// </summary>
    public string? NatoItemIdentificationNumber { get; set; } = string.Empty;

    /// <summary>
    /// FFF类别（SAP MARA.IMATN）
    /// </summary>
    public string? FffClass { get; set; } = string.Empty;

    /// <summary>
    /// 替代链编码（SAP MARA.PICNUM）
    /// </summary>
    public string? SupersessionChainNumber { get; set; } = string.Empty;

    /// <summary>
    /// 季节采购创建状态（SAP MARA.BSTAT）
    /// </summary>
    public string? SeasonalProcurementCreationStatus { get; set; } = string.Empty;

    /// <summary>
    /// 颜色特性内部号（SAP MARA.COLOR_ATINN）
    /// </summary>
    public string? ColorCharacteristicInternalNumber { get; set; } = string.Empty;

    /// <summary>
    /// 主尺码特性内部号（SAP MARA.SIZE1_ATINN）
    /// </summary>
    public string? MainSizeCharacteristicInternalNumber { get; set; } = string.Empty;

    /// <summary>
    /// 次尺码特性内部号（SAP MARA.SIZE2_ATINN）
    /// </summary>
    public string? SecondSizeCharacteristicInternalNumber { get; set; } = string.Empty;

    /// <summary>
    /// 颜色（SAP MARA.COLOR）
    /// </summary>
    public string? Color { get; set; } = string.Empty;

    /// <summary>
    /// 主尺码（SAP MARA.SIZE1）
    /// </summary>
    public string? MainSize { get; set; } = string.Empty;

    /// <summary>
    /// 次尺码（SAP MARA.SIZE2）
    /// </summary>
    public string? SecondSize { get; set; } = string.Empty;

    /// <summary>
    /// 评估特性值（SAP MARA.FREE_CHAR）
    /// </summary>
    public string? EvaluationCharacteristicValue { get; set; } = string.Empty;

    /// <summary>
    /// 护理代码（SAP MARA.CARE_CODE）
    /// </summary>
    public string? CareCode { get; set; } = string.Empty;

    /// <summary>
    /// 品牌（SAP MARA.BRAND_ID）
    /// </summary>
    public string? BrandId { get; set; } = string.Empty;

    /// <summary>
    /// 纤维代码1（SAP MARA.FIBER_CODE1）
    /// </summary>
    public string? FiberCode1 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维占比1（SAP MARA.FIBER_PART1）
    /// </summary>
    public string? FiberPart1 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维代码2（SAP MARA.FIBER_CODE2）
    /// </summary>
    public string? FiberCode2 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维占比2（SAP MARA.FIBER_PART2）
    /// </summary>
    public string? FiberPart2 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维代码3（SAP MARA.FIBER_CODE3）
    /// </summary>
    public string? FiberCode3 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维占比3（SAP MARA.FIBER_PART3）
    /// </summary>
    public string? FiberPart3 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维代码4（SAP MARA.FIBER_CODE4）
    /// </summary>
    public string? FiberCode4 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维占比4（SAP MARA.FIBER_PART4）
    /// </summary>
    public string? FiberPart4 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维代码5（SAP MARA.FIBER_CODE5）
    /// </summary>
    public string? FiberCode5 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维占比5（SAP MARA.FIBER_PART5）
    /// </summary>
    public string? FiberPart5 { get; set; } = string.Empty;

    /// <summary>
    /// 时装等级（SAP MARA.FASHGRD）
    /// </summary>
    public string? FashionGrade { get; set; } = string.Empty;

    /// <summary>
    /// 物料状态（字典 sys_normal_disable_status；0=禁用，1=启用，2=锁定；平台启用态，非 SAP MSTAE）
    /// </summary>
    public int? MaterialStatus { get; set; }

    /// <summary>
    /// 创建时间（范围查询-开始）
    /// </summary>
    public DateTime? CreatedAtStart { get; set; }

    /// <summary>
    /// 创建时间（范围查询-结束）
    /// </summary>
    public DateTime? CreatedAtEnd { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注（模糊查询）
    /// </summary>
    public string? Remark { get; set; }
}

// ========================================
// 创建Material DTO
// ========================================

/// <summary>
/// 创建Material DTO
/// </summary>
public class TaktMaterialCreateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（SAP MARA.MATNR）
    /// </summary>
    [Required(ErrorMessage = "物料编码（SAP MARA.MATNR）不能为空")]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 完整维护状态（SAP MARA.VPSTA）
    /// </summary>
    public string? CompleteMaintenanceStatus { get; set; } = string.Empty;

    /// <summary>
    /// 维护状态（SAP MARA.PSTAT）
    /// </summary>
    public string? MaintenanceStatus { get; set; } = string.Empty;

    /// <summary>
    /// 客户级删除标记（SAP MARA.LVORM）
    /// </summary>
    public string? ClientDeletionFlag { get; set; } = string.Empty;

    /// <summary>
    /// 物料类型（SAP MARA.MTART）
    /// </summary>
    [Required(ErrorMessage = "物料类型（SAP MARA.MTART）不能为空")]
    public string MaterialType { get; set; } = string.Empty;

    /// <summary>
    /// 行业领域（SAP MARA.MBRSH）
    /// </summary>
    [Required(ErrorMessage = "行业领域（SAP MARA.MBRSH）不能为空")]
    public string IndustrySector { get; set; } = string.Empty;

    /// <summary>
    /// 物料组（SAP MARA.MATKL）
    /// </summary>
    [Required(ErrorMessage = "物料组（SAP MARA.MATKL）不能为空")]
    public string MaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料号（SAP MARA.BISMT）
    /// </summary>
    public string? OldMaterialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 基本计量单位（SAP MARA.MEINS）
    /// </summary>
    [Required(ErrorMessage = "基本计量单位（SAP MARA.MEINS）不能为空")]
    public string BaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单单位（SAP MARA.BSTME）
    /// </summary>
    public string? OrderUnit { get; set; } = string.Empty;

    /// <summary>
    /// 单据号（SAP MARA.ZEINR）
    /// </summary>
    public string? DocumentNumber { get; set; } = string.Empty;

    /// <summary>
    /// 单据类型（SAP MARA.ZEIAR）
    /// </summary>
    public string? DocumentType { get; set; } = string.Empty;

    /// <summary>
    /// 单据版本（SAP MARA.ZEIVR）
    /// </summary>
    public string? DocumentVersion { get; set; } = string.Empty;

    /// <summary>
    /// 单据页格式（SAP MARA.ZEIFO）
    /// </summary>
    public string? DocumentPageFormat { get; set; } = string.Empty;

    /// <summary>
    /// 单据更改号（SAP MARA.AESZN）
    /// </summary>
    public string? DocumentChangeNumber { get; set; } = string.Empty;

    /// <summary>
    /// 单据页号（SAP MARA.BLATT）
    /// </summary>
    public string? DocumentPageNumber { get; set; } = string.Empty;

    /// <summary>
    /// 单据页数（SAP MARA.BLANZ）
    /// </summary>
    public string? DocumentSheetCount { get; set; } = string.Empty;

    /// <summary>
    /// 生产/检验备忘（SAP MARA.FERTH）
    /// </summary>
    public string? ProductionInspectionMemo { get; set; } = string.Empty;

    /// <summary>
    /// 生产备忘页格式（SAP MARA.FORMT）
    /// </summary>
    public string? ProductionMemoPageFormat { get; set; } = string.Empty;

    /// <summary>
    /// 尺寸/规格（SAP MARA.GROES）
    /// </summary>
    public string? SizeDimensions { get; set; } = string.Empty;

    /// <summary>
    /// 基本物料（材质）（SAP MARA.WRKST）
    /// </summary>
    public string? BasicMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 行业标准描述（SAP MARA.NORMT）
    /// </summary>
    public string? IndustryStandardDescription { get; set; } = string.Empty;

    /// <summary>
    /// 实验室/设计室（SAP MARA.LABOR）
    /// </summary>
    public string? LaboratoryDesignOffice { get; set; } = string.Empty;

    /// <summary>
    /// 采购价值码（SAP MARA.EKWSL）
    /// </summary>
    public string? PurchasingValueKey { get; set; } = string.Empty;

    /// <summary>
    /// 毛重（SAP MARA.BRGEW）
    /// </summary>
    public decimal? GrossWeight { get; set; }

    /// <summary>
    /// 净重（SAP MARA.NTGEW）
    /// </summary>
    public decimal? NetWeight { get; set; }

    /// <summary>
    /// 重量单位（SAP MARA.GEWEI）
    /// </summary>
    public string? WeightUnit { get; set; } = string.Empty;

    /// <summary>
    /// 体积（SAP MARA.VOLUM）
    /// </summary>
    public decimal? Volume { get; set; }

    /// <summary>
    /// 体积单位（SAP MARA.VOLEH）
    /// </summary>
    public string? VolumeUnit { get; set; } = string.Empty;

    /// <summary>
    /// 容器要求（SAP MARA.BEHVO）
    /// </summary>
    public string? ContainerRequirements { get; set; } = string.Empty;

    /// <summary>
    /// 仓储条件（SAP MARA.RAUBE）
    /// </summary>
    public string? StorageConditions { get; set; } = string.Empty;

    /// <summary>
    /// 温度条件（SAP MARA.TEMPB）
    /// </summary>
    public string? TemperatureConditions { get; set; } = string.Empty;

    /// <summary>
    /// 低层码（SAP MARA.DISST）
    /// </summary>
    public string? LowLevelCode { get; set; } = string.Empty;

    /// <summary>
    /// 运输组（SAP MARA.TRAGR）
    /// </summary>
    public string? TransportationGroup { get; set; } = string.Empty;

    /// <summary>
    /// 危险品编码（SAP MARA.STOFF）
    /// </summary>
    public string? HazardousMaterialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 产品组（SAP MARA.SPART）
    /// </summary>
    public string? Division { get; set; } = string.Empty;

    /// <summary>
    /// 竞争对手（SAP MARA.KUNNR）
    /// </summary>
    public string? Competitor { get; set; } = string.Empty;

    /// <summary>
    /// 欧洲商品号（旧）（SAP MARA.EANNR）
    /// </summary>
    public string? EuropeanArticleNumberObsolete { get; set; } = string.Empty;

    /// <summary>
    /// 收发货凭证打印数量（SAP MARA.WESCH）
    /// </summary>
    public decimal? GrGiSlipQuantity { get; set; }

    /// <summary>
    /// 采购规则（SAP MARA.BWVOR）
    /// </summary>
    public string? ProcurementRule { get; set; } = string.Empty;

    /// <summary>
    /// 货源（SAP MARA.BWSCL）
    /// </summary>
    public string? SourceOfSupply { get; set; } = string.Empty;

    /// <summary>
    /// 季节类别（SAP MARA.SAISO）
    /// </summary>
    public string? SeasonCategory { get; set; } = string.Empty;

    /// <summary>
    /// 标签类型（SAP MARA.ETIAR）
    /// </summary>
    public string? LabelType { get; set; } = string.Empty;

    /// <summary>
    /// 标签格式（SAP MARA.ETIFO）
    /// </summary>
    public string? LabelForm { get; set; } = string.Empty;

    /// <summary>
    /// 已停用字段（SAP MARA.ENTAR）
    /// </summary>
    public string? DeactivatedField { get; set; } = string.Empty;

    /// <summary>
    /// 国际商品编码EAN/UPC（SAP MARA.EAN11）
    /// </summary>
    public string? InternationalArticleNumber { get; set; } = string.Empty;

    /// <summary>
    /// EAN类别（SAP MARA.NUMTP）
    /// </summary>
    public string? EanCategory { get; set; } = string.Empty;

    /// <summary>
    /// 长度（SAP MARA.LAENG）
    /// </summary>
    public decimal? Length { get; set; }

    /// <summary>
    /// 宽度（SAP MARA.BREIT）
    /// </summary>
    public decimal? Width { get; set; }

    /// <summary>
    /// 高度（SAP MARA.HOEHE）
    /// </summary>
    public decimal? Height { get; set; }

    /// <summary>
    /// 长宽高单位（SAP MARA.MEABM）
    /// </summary>
    public string? DimensionUnit { get; set; } = string.Empty;

    /// <summary>
    /// 产品层次（SAP MARA.PRDHA）
    /// </summary>
    public string? ProductHierarchy { get; set; } = string.Empty;

    /// <summary>
    /// 库存调拨净更改成本核算（SAP MARA.AEKLK）
    /// </summary>
    public string? StockTransferNetChangeCosting { get; set; } = string.Empty;

    /// <summary>
    /// CAD标识（SAP MARA.CADKZ）
    /// </summary>
    public string? CadIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 采购QM激活（SAP MARA.QMPUR）
    /// </summary>
    public string? QmInProcurement { get; set; } = string.Empty;

    /// <summary>
    /// 允许包装重量（SAP MARA.ERGEW）
    /// </summary>
    public decimal? AllowedPackagingWeight { get; set; }

    /// <summary>
    /// 允许包装重量单位（SAP MARA.ERGEI）
    /// </summary>
    public string? AllowedPackagingWeightUnit { get; set; } = string.Empty;

    /// <summary>
    /// 允许包装体积（SAP MARA.ERVOL）
    /// </summary>
    public decimal? AllowedPackagingVolume { get; set; }

    /// <summary>
    /// 允许包装体积单位（SAP MARA.ERVOE）
    /// </summary>
    public string? AllowedPackagingVolumeUnit { get; set; } = string.Empty;

    /// <summary>
    /// 超重容差（SAP MARA.GEWTO）
    /// </summary>
    public decimal? ExcessWeightTolerance { get; set; }

    /// <summary>
    /// 超体积容差（SAP MARA.VOLTO）
    /// </summary>
    public decimal? ExcessVolumeTolerance { get; set; }

    /// <summary>
    /// 可变采购订单单位（SAP MARA.VABME）
    /// </summary>
    public string? VariablePurchaseOrderUnit { get; set; } = string.Empty;

    /// <summary>
    /// 已分配修订级别（SAP MARA.KZREV）
    /// </summary>
    public string? RevisionLevelAssigned { get; set; } = string.Empty;

    /// <summary>
    /// 可配置物料（SAP MARA.KZKFG）
    /// </summary>
    public string? ConfigurableMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 批次管理要求（SAP MARA.XCHPF）
    /// </summary>
    public string? BatchManagementRequired { get; set; } = string.Empty;

    /// <summary>
    /// 包装物料类型（SAP MARA.VHART）
    /// </summary>
    public string? PackagingMaterialType { get; set; } = string.Empty;

    /// <summary>
    /// 最大装载量（体积）（SAP MARA.FUELG）
    /// </summary>
    public decimal? MaximumLevelByVolume { get; set; }

    /// <summary>
    /// 堆叠因子（SAP MARA.STFAK）
    /// </summary>
    public int? StackingFactor { get; set; }

    /// <summary>
    /// 包装物料组（SAP MARA.MAGRV）
    /// </summary>
    public string? PackagingMaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 权限组（SAP MARA.BEGRU）
    /// </summary>
    public string? AuthorizationGroup { get; set; } = string.Empty;

    /// <summary>
    /// 有效起始日期（SAP MARA.DATAB）
    /// </summary>
    public DateTime? ValidFromDate { get; set; }

    /// <summary>
    /// 季节年份（SAP MARA.SAISJ）
    /// </summary>
    public string? SeasonYear { get; set; } = string.Empty;

    /// <summary>
    /// 价格带类别（SAP MARA.PLGTP）
    /// </summary>
    public string? PriceBandCategory { get; set; } = string.Empty;

    /// <summary>
    /// 空容器BOM（SAP MARA.MLGUT）
    /// </summary>
    public string? EmptiesBillOfMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 外部物料组（SAP MARA.EXTWG）
    /// </summary>
    public string? ExternalMaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 跨工厂可配置物料（SAP MARA.SATNR）
    /// </summary>
    public string? CrossPlantConfigurableMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 物料类别（SAP MARA.ATTYP）
    /// </summary>
    public string? MaterialCategory { get; set; } = string.Empty;

    /// <summary>
    /// 联产品标识（SAP MARA.KZKUP）
    /// </summary>
    public string? CoProductIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 后续物料标识（SAP MARA.KZNFM）
    /// </summary>
    public string? FollowUpMaterialIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 定价参考物料（SAP MARA.PMATA）
    /// </summary>
    public string? PricingReferenceMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 跨工厂物料状态（SAP MARA.MSTAE）
    /// </summary>
    public string? CrossPlantMaterialStatus { get; set; } = string.Empty;

    /// <summary>
    /// 跨分销链物料状态（SAP MARA.MSTAV）
    /// </summary>
    public string? CrossDistributionChainStatus { get; set; } = string.Empty;

    /// <summary>
    /// 跨工厂状态生效日期（SAP MARA.MSTDE）
    /// </summary>
    public DateTime? CrossPlantStatusValidFrom { get; set; }

    /// <summary>
    /// 跨分销链状态生效日期（SAP MARA.MSTDV）
    /// </summary>
    public DateTime? CrossDistributionStatusValidFrom { get; set; }

    /// <summary>
    /// 物料税分类（SAP MARA.TAKLV）
    /// </summary>
    public string? TaxClassification { get; set; } = string.Empty;

    /// <summary>
    /// 目录参数文件（SAP MARA.RBNRM）
    /// </summary>
    public string? CatalogProfile { get; set; } = string.Empty;

    /// <summary>
    /// 最短剩余货架寿命（SAP MARA.MHDRZ）
    /// </summary>
    public decimal? MinimumRemainingShelfLife { get; set; }

    /// <summary>
    /// 总货架寿命（SAP MARA.MHDHB）
    /// </summary>
    public decimal? TotalShelfLife { get; set; }

    /// <summary>
    /// 仓储百分比（SAP MARA.MHDLP）
    /// </summary>
    public decimal? StoragePercentage { get; set; }

    /// <summary>
    /// 含量单位（SAP MARA.INHME）
    /// </summary>
    public string? ContentUnit { get; set; } = string.Empty;

    /// <summary>
    /// 净含量（SAP MARA.INHAL）
    /// </summary>
    public decimal? NetContents { get; set; }

    /// <summary>
    /// 比较价格单位（SAP MARA.VPREH）
    /// </summary>
    public decimal? ComparisonPriceUnit { get; set; }

    /// <summary>
    /// 标签物料分组（SAP MARA.ETIAG）
    /// </summary>
    public string? LabelingMaterialGrouping { get; set; } = string.Empty;

    /// <summary>
    /// 毛含量（SAP MARA.INHBR）
    /// </summary>
    public decimal? GrossContents { get; set; }

    /// <summary>
    /// 数量换算方法（SAP MARA.CMETH）
    /// </summary>
    public string? QuantityConversionMethod { get; set; } = string.Empty;

    /// <summary>
    /// 内部对象号（SAP MARA.CUOBF）
    /// </summary>
    public string? InternalObjectNumber { get; set; } = string.Empty;

    /// <summary>
    /// 环境相关（SAP MARA.KZUMW）
    /// </summary>
    public string? EnvironmentallyRelevant { get; set; } = string.Empty;

    /// <summary>
    /// 产品分配确定过程（SAP MARA.KOSCH）
    /// </summary>
    public string? ProductAllocationProcedure { get; set; } = string.Empty;

    /// <summary>
    /// 变式定价参数文件（SAP MARA.SPROF）
    /// </summary>
    public string? VariantPricingProfile { get; set; } = string.Empty;

    /// <summary>
    /// 实物折扣资格（SAP MARA.NRFHG）
    /// </summary>
    public string? DiscountInKind { get; set; } = string.Empty;

    /// <summary>
    /// 制造商零件号（SAP MARA.MFRPN）
    /// </summary>
    public string? ManufacturerPartNumber { get; set; } = string.Empty;

    /// <summary>
    /// 制造商编码（SAP MARA.MFRNR）
    /// </summary>
    public string? ManufacturerNumber { get; set; } = string.Empty;

    /// <summary>
    /// 自有库存管理物料号（SAP MARA.BMATN）
    /// </summary>
    public string? InventoryManagedMaterialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 制造商零件参数文件（SAP MARA.MPROF）
    /// </summary>
    public string? ManufacturerPartProfile { get; set; } = string.Empty;

    /// <summary>
    /// 计量单位用途（SAP MARA.KZWSM）
    /// </summary>
    public string? UnitsOfMeasureUsage { get; set; } = string.Empty;

    /// <summary>
    /// 季节推出（SAP MARA.SAITY）
    /// </summary>
    public string? SeasonRollout { get; set; } = string.Empty;

    /// <summary>
    /// 危险品参数文件（SAP MARA.PROFL）
    /// </summary>
    public string? DangerousGoodsProfile { get; set; } = string.Empty;

    /// <summary>
    /// 高粘度（SAP MARA.IHIVI）
    /// </summary>
    public string? HighlyViscous { get; set; } = string.Empty;

    /// <summary>
    /// 散装/液体（SAP MARA.ILOOS）
    /// </summary>
    public string? InBulkLiquid { get; set; } = string.Empty;

    /// <summary>
    /// 序列号明确级别（SAP MARA.SERLV）
    /// </summary>
    public string? SerialNumberExplicitness { get; set; } = string.Empty;

    /// <summary>
    /// 封闭包装（SAP MARA.KZGVH）
    /// </summary>
    public string? ClosedPackaging { get; set; } = string.Empty;

    /// <summary>
    /// 需批准批次记录（SAP MARA.XGCHP）
    /// </summary>
    public string? ApprovedBatchRecordRequired { get; set; } = string.Empty;

    /// <summary>
    /// 有效性参数覆盖（SAP MARA.KZEFF）
    /// </summary>
    public string? EffectivityParameterOverride { get; set; } = string.Empty;

    /// <summary>
    /// 物料完成级别（SAP MARA.COMPL）
    /// </summary>
    public string? MaterialCompletionLevel { get; set; } = string.Empty;

    /// <summary>
    /// 货架寿命期间标识（SAP MARA.IPRKZ）
    /// </summary>
    public string? ShelfLifePeriodIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 货架寿命舍入规则（SAP MARA.RDMHD）
    /// </summary>
    public string? ShelfLifeRoundingRule { get; set; } = string.Empty;

    /// <summary>
    /// 包装打印产品成分（SAP MARA.PRZUS）
    /// </summary>
    public string? ProductCompositionOnPackaging { get; set; } = string.Empty;

    /// <summary>
    /// 通用项目类别组（SAP MARA.MTPOS_MARA）
    /// </summary>
    public string? GeneralItemCategoryGroup { get; set; } = string.Empty;

    /// <summary>
    /// 后勤变式通用物料（SAP MARA.BFLME）
    /// </summary>
    public string? LogisticalVariants { get; set; } = string.Empty;

    /// <summary>
    /// 物料锁定（SAP MARA.MATFI）
    /// </summary>
    public string? MaterialLocked { get; set; } = string.Empty;

    /// <summary>
    /// 配置管理相关（SAP MARA.CMREL）
    /// </summary>
    public string? ConfigurationManagementRelevant { get; set; } = string.Empty;

    /// <summary>
    /// 品种清单类型（SAP MARA.BBTYP）
    /// </summary>
    public string? AssortmentListType { get; set; } = string.Empty;

    /// <summary>
    /// 到期日期类型（SAP MARA.SLED_BBD）
    /// </summary>
    public string? ExpirationDateType { get; set; } = string.Empty;

    /// <summary>
    /// GTIN变式（SAP MARA.GTIN_VARIANT）
    /// </summary>
    public string? GtinVariant { get; set; } = string.Empty;

    /// <summary>
    /// 通用物料号（SAP MARA.GENNR）
    /// </summary>
    public string? GenericMaterialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 相同包装参考物料（SAP MARA.RMATP）
    /// </summary>
    public string? SamePackingReferenceMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 全球数据同步相关（SAP MARA.GDS_RELEVANT）
    /// </summary>
    public string? GlobalDataSyncRelevant { get; set; } = string.Empty;

    /// <summary>
    /// 原产地验收（SAP MARA.WEORA）
    /// </summary>
    public string? AcceptanceAtOrigin { get; set; } = string.Empty;

    /// <summary>
    /// 标准HU类型（SAP MARA.HUTYP_DFLT）
    /// </summary>
    public string? StandardHuType { get; set; } = string.Empty;

    /// <summary>
    /// 易被盗（SAP MARA.PILFERABLE）
    /// </summary>
    public string? Pilferable { get; set; } = string.Empty;

    /// <summary>
    /// 仓储存储条件（SAP MARA.WHSTC）
    /// </summary>
    public string? WarehouseStorageCondition { get; set; } = string.Empty;

    /// <summary>
    /// 仓储物料组（SAP MARA.WHMATGR）
    /// </summary>
    public string? WarehouseMaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 处理标识（SAP MARA.HNDLCODE）
    /// </summary>
    public string? HandlingIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 危险物质相关（SAP MARA.HAZMAT）
    /// </summary>
    public string? HazardousSubstancesRelevant { get; set; } = string.Empty;

    /// <summary>
    /// 处理单元类型（SAP MARA.HUTYP）
    /// </summary>
    public string? HandlingUnitType { get; set; } = string.Empty;

    /// <summary>
    /// 可变皮重（SAP MARA.TARE_VAR）
    /// </summary>
    public string? VariableTareWeight { get; set; } = string.Empty;

    /// <summary>
    /// 最大允许容量（SAP MARA.MAXC）
    /// </summary>
    public decimal? MaximumAllowedCapacity { get; set; }

    /// <summary>
    /// 超容量容差（SAP MARA.MAXC_TOL）
    /// </summary>
    public decimal? OvercapacityTolerance { get; set; }

    /// <summary>
    /// 最大包装长度（SAP MARA.MAXL）
    /// </summary>
    public decimal? MaximumPackingLength { get; set; }

    /// <summary>
    /// 最大包装宽度（SAP MARA.MAXB）
    /// </summary>
    public decimal? MaximumPackingWidth { get; set; }

    /// <summary>
    /// 最大包装高度（SAP MARA.MAXH）
    /// </summary>
    public decimal? MaximumPackingHeight { get; set; }

    /// <summary>
    /// 最大包装尺寸单位（SAP MARA.MAXDIM_UOM）
    /// </summary>
    public string? MaximumPackingDimensionUnit { get; set; } = string.Empty;

    /// <summary>
    /// 原产国（SAP MARA.HERKL）
    /// </summary>
    public string? CountryOfOrigin { get; set; } = string.Empty;

    /// <summary>
    /// 物料运费组（SAP MARA.MFRGR）
    /// </summary>
    public string? MaterialFreightGroup { get; set; } = string.Empty;

    /// <summary>
    /// 隔离期（SAP MARA.QQTIME）
    /// </summary>
    public decimal? QuarantinePeriod { get; set; }

    /// <summary>
    /// 隔离期单位（SAP MARA.QQTIMEUOM）
    /// </summary>
    public string? QuarantinePeriodUnit { get; set; } = string.Empty;

    /// <summary>
    /// 质检组（SAP MARA.QGRP）
    /// </summary>
    public string? QualityInspectionGroup { get; set; } = string.Empty;

    /// <summary>
    /// 序列号参数文件（SAP MARA.SERIAL）
    /// </summary>
    public string? SerialNumberProfile { get; set; } = string.Empty;

    /// <summary>
    /// 表单名称（SAP MARA.PS_SMARTFORM）
    /// </summary>
    public string? FormName { get; set; } = string.Empty;

    /// <summary>
    /// 后勤计量单位（SAP MARA.LOGUNIT）
    /// </summary>
    public string? LogisticsUnitOfMeasure { get; set; } = string.Empty;

    /// <summary>
    /// 捕捞重量物料（SAP MARA.CWQREL）
    /// </summary>
    public string? CatchWeightMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 捕捞重量参数文件（SAP MARA.CWQPROC）
    /// </summary>
    public string? CatchWeightProfile { get; set; } = string.Empty;

    /// <summary>
    /// 捕捞重量容差组（SAP MARA.CWQTOLGR）
    /// </summary>
    public string? CatchWeightToleranceGroup { get; set; } = string.Empty;

    /// <summary>
    /// 调整参数文件（SAP MARA.ADPROF）
    /// </summary>
    public string? AdjustmentProfile { get; set; } = string.Empty;

    /// <summary>
    /// 知识产权ID（SAP MARA.IPMIPPRODUCT）
    /// </summary>
    public string? IntellectualPropertyId { get; set; } = string.Empty;

    /// <summary>
    /// 允许变式价格（SAP MARA.ALLOW_PMAT_IGNO）
    /// </summary>
    public string? VariantPriceAllowed { get; set; } = string.Empty;

    /// <summary>
    /// 介质（SAP MARA.MEDIUM）
    /// </summary>
    public string? Medium { get; set; } = string.Empty;

    /// <summary>
    /// 实物商品（SAP MARA.COMMODITY）
    /// </summary>
    public string? PhysicalCommodity { get; set; } = string.Empty;

    /// <summary>
    /// 动物源（SAP MARA.ANIMAL_ORIGIN）
    /// </summary>
    public string? AnimalOrigin { get; set; } = string.Empty;

    /// <summary>
    /// 纺织成分功能（SAP MARA.TEXTILE_COMP_IND）
    /// </summary>
    public string? TextileCompositionFunction { get; set; } = string.Empty;

    /// <summary>
    /// 细分结构（SAP MARA.SGT_CSGR）
    /// </summary>
    public string? SegmentationStructure { get; set; } = string.Empty;

    /// <summary>
    /// 细分策略（SAP MARA.SGT_COVSA）
    /// </summary>
    public string? SegmentationStrategy { get; set; } = string.Empty;

    /// <summary>
    /// 细分状态（SAP MARA.SGT_STAT）
    /// </summary>
    public string? SegmentationStatus { get; set; } = string.Empty;

    /// <summary>
    /// 细分范围（SAP MARA.SGT_SCOPE）
    /// </summary>
    public string? SegmentationScope { get; set; } = string.Empty;

    /// <summary>
    /// 细分相关（SAP MARA.SGT_REL）
    /// </summary>
    public string? SegmentationRelevant { get; set; } = string.Empty;

    /// <summary>
    /// 时装属性1（SAP MARA.FSH_MG_AT1）
    /// </summary>
    public string? FashionAttribute1 { get; set; } = string.Empty;

    /// <summary>
    /// 时装属性2（SAP MARA.FSH_MG_AT2）
    /// </summary>
    public string? FashionAttribute2 { get; set; } = string.Empty;

    /// <summary>
    /// 时装属性3（SAP MARA.FSH_MG_AT3）
    /// </summary>
    public string? FashionAttribute3 { get; set; } = string.Empty;

    /// <summary>
    /// 季节使用标识（SAP MARA.FSH_SEALV）
    /// </summary>
    public string? SeasonUsageIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 库存季节激活（SAP MARA.FSH_SEAIM）
    /// </summary>
    public string? SeasonActiveInInventory { get; set; } = string.Empty;

    /// <summary>
    /// 特性转换ID（SAP MARA.FSH_SC_MID）
    /// </summary>
    public string? CharacteristicConversionId { get; set; } = string.Empty;

    /// <summary>
    /// ANP代码（SAP MARA.ANP）
    /// </summary>
    public string? AnpCode { get; set; } = string.Empty;

    /// <summary>
    /// 危险品包装状态（SAP MARA.DG_PACK_STATUS）
    /// </summary>
    public string? DangerousGoodsPackagingStatus { get; set; } = string.Empty;

    /// <summary>
    /// 物料条件管理（SAP MARA.MCOND）
    /// </summary>
    public string? MaterialConditionManagement { get; set; } = string.Empty;

    /// <summary>
    /// 退货代码（SAP MARA.RETDELC）
    /// </summary>
    public string? ReturnCode { get; set; } = string.Empty;

    /// <summary>
    /// 退回后勤级别（SAP MARA.LOGLEV_RETO）
    /// </summary>
    public string? ReturnToLogisticsLevel { get; set; } = string.Empty;

    /// <summary>
    /// NATO物料识别号（SAP MARA.NSNID）
    /// </summary>
    public string? NatoItemIdentificationNumber { get; set; } = string.Empty;

    /// <summary>
    /// FFF类别（SAP MARA.IMATN）
    /// </summary>
    public string? FffClass { get; set; } = string.Empty;

    /// <summary>
    /// 替代链编码（SAP MARA.PICNUM）
    /// </summary>
    public string? SupersessionChainNumber { get; set; } = string.Empty;

    /// <summary>
    /// 季节采购创建状态（SAP MARA.BSTAT）
    /// </summary>
    public string? SeasonalProcurementCreationStatus { get; set; } = string.Empty;

    /// <summary>
    /// 颜色特性内部号（SAP MARA.COLOR_ATINN）
    /// </summary>
    public string? ColorCharacteristicInternalNumber { get; set; } = string.Empty;

    /// <summary>
    /// 主尺码特性内部号（SAP MARA.SIZE1_ATINN）
    /// </summary>
    public string? MainSizeCharacteristicInternalNumber { get; set; } = string.Empty;

    /// <summary>
    /// 次尺码特性内部号（SAP MARA.SIZE2_ATINN）
    /// </summary>
    public string? SecondSizeCharacteristicInternalNumber { get; set; } = string.Empty;

    /// <summary>
    /// 颜色（SAP MARA.COLOR）
    /// </summary>
    public string? Color { get; set; } = string.Empty;

    /// <summary>
    /// 主尺码（SAP MARA.SIZE1）
    /// </summary>
    public string? MainSize { get; set; } = string.Empty;

    /// <summary>
    /// 次尺码（SAP MARA.SIZE2）
    /// </summary>
    public string? SecondSize { get; set; } = string.Empty;

    /// <summary>
    /// 评估特性值（SAP MARA.FREE_CHAR）
    /// </summary>
    public string? EvaluationCharacteristicValue { get; set; } = string.Empty;

    /// <summary>
    /// 护理代码（SAP MARA.CARE_CODE）
    /// </summary>
    public string? CareCode { get; set; } = string.Empty;

    /// <summary>
    /// 品牌（SAP MARA.BRAND_ID）
    /// </summary>
    public string? BrandId { get; set; } = string.Empty;

    /// <summary>
    /// 纤维代码1（SAP MARA.FIBER_CODE1）
    /// </summary>
    public string? FiberCode1 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维占比1（SAP MARA.FIBER_PART1）
    /// </summary>
    public string? FiberPart1 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维代码2（SAP MARA.FIBER_CODE2）
    /// </summary>
    public string? FiberCode2 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维占比2（SAP MARA.FIBER_PART2）
    /// </summary>
    public string? FiberPart2 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维代码3（SAP MARA.FIBER_CODE3）
    /// </summary>
    public string? FiberCode3 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维占比3（SAP MARA.FIBER_PART3）
    /// </summary>
    public string? FiberPart3 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维代码4（SAP MARA.FIBER_CODE4）
    /// </summary>
    public string? FiberCode4 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维占比4（SAP MARA.FIBER_PART4）
    /// </summary>
    public string? FiberPart4 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维代码5（SAP MARA.FIBER_CODE5）
    /// </summary>
    public string? FiberCode5 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维占比5（SAP MARA.FIBER_PART5）
    /// </summary>
    public string? FiberPart5 { get; set; } = string.Empty;

    /// <summary>
    /// 时装等级（SAP MARA.FASHGRD）
    /// </summary>
    public string? FashionGrade { get; set; } = string.Empty;

    /// <summary>
    /// 物料状态（字典 sys_normal_disable_status；0=禁用，1=启用，2=锁定；平台启用态，非 SAP MSTAE）
    /// </summary>
    public int MaterialStatus { get; set; } = 0;

    /// <summary>
    /// 多语言描述列表（主子表关系；对齐 SAP MAKT）（子表，级联保存）
    /// </summary>
    public List<TaktMaterialDescriptionCreateDto>? Descriptions { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 更新Material DTO
// ========================================

/// <summary>
/// 更新Material DTO
/// 继承 TaktMaterialCreateDto，添加 MaterialId 字段
/// </summary>
public class TaktMaterialUpdateDto : TaktMaterialCreateDto
{
    /// <summary>
    /// MaterialID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialId { get; set; }

    /// <summary>
    /// 多语言描述列表（主子表关系；对齐 SAP MAKT）（子表，级联保存）
    /// </summary>
    public new List<TaktMaterialDescriptionUpdateDto>? Descriptions { get; set; }

}

// ========================================
// Material 状态 DTO
// ========================================

/// <summary>
/// Material 状态更新 DTO
/// </summary>
public class TaktMaterialStatusDto
{
    /// <summary>
    /// MaterialID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialId { get; set; }

    /// <summary>
    /// 物料状态（字典 sys_normal_disable_status；0=禁用，1=启用，2=锁定；平台启用态，非 SAP MSTAE）
    /// </summary>
    [Required(ErrorMessage = "物料状态（字典 sys_normal_disable_status；0=禁用，1=启用，2=锁定；平台启用态，非 SAP MSTAE）不能为空")]
    public int MaterialStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// Material 导入模板行 DTO
/// </summary>
public class TaktMaterialTemplateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（SAP MARA.MATNR）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 完整维护状态（SAP MARA.VPSTA）
    /// </summary>
    public string? CompleteMaintenanceStatus { get; set; } = string.Empty;

    /// <summary>
    /// 维护状态（SAP MARA.PSTAT）
    /// </summary>
    public string? MaintenanceStatus { get; set; } = string.Empty;

    /// <summary>
    /// 客户级删除标记（SAP MARA.LVORM）
    /// </summary>
    public string? ClientDeletionFlag { get; set; } = string.Empty;

    /// <summary>
    /// 物料类型（SAP MARA.MTART）
    /// </summary>
    public string? MaterialType { get; set; } = string.Empty;

    /// <summary>
    /// 行业领域（SAP MARA.MBRSH）
    /// </summary>
    public string? IndustrySector { get; set; } = string.Empty;

    /// <summary>
    /// 物料组（SAP MARA.MATKL）
    /// </summary>
    public string? MaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料号（SAP MARA.BISMT）
    /// </summary>
    public string? OldMaterialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 基本计量单位（SAP MARA.MEINS）
    /// </summary>
    public string? BaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单单位（SAP MARA.BSTME）
    /// </summary>
    public string? OrderUnit { get; set; } = string.Empty;

    /// <summary>
    /// 单据号（SAP MARA.ZEINR）
    /// </summary>
    public string? DocumentNumber { get; set; } = string.Empty;

    /// <summary>
    /// 单据类型（SAP MARA.ZEIAR）
    /// </summary>
    public string? DocumentType { get; set; } = string.Empty;

    /// <summary>
    /// 单据版本（SAP MARA.ZEIVR）
    /// </summary>
    public string? DocumentVersion { get; set; } = string.Empty;

    /// <summary>
    /// 单据页格式（SAP MARA.ZEIFO）
    /// </summary>
    public string? DocumentPageFormat { get; set; } = string.Empty;

    /// <summary>
    /// 单据更改号（SAP MARA.AESZN）
    /// </summary>
    public string? DocumentChangeNumber { get; set; } = string.Empty;

    /// <summary>
    /// 单据页号（SAP MARA.BLATT）
    /// </summary>
    public string? DocumentPageNumber { get; set; } = string.Empty;

    /// <summary>
    /// 单据页数（SAP MARA.BLANZ）
    /// </summary>
    public string? DocumentSheetCount { get; set; } = string.Empty;

    /// <summary>
    /// 生产/检验备忘（SAP MARA.FERTH）
    /// </summary>
    public string? ProductionInspectionMemo { get; set; } = string.Empty;

    /// <summary>
    /// 生产备忘页格式（SAP MARA.FORMT）
    /// </summary>
    public string? ProductionMemoPageFormat { get; set; } = string.Empty;

    /// <summary>
    /// 尺寸/规格（SAP MARA.GROES）
    /// </summary>
    public string? SizeDimensions { get; set; } = string.Empty;

    /// <summary>
    /// 基本物料（材质）（SAP MARA.WRKST）
    /// </summary>
    public string? BasicMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 行业标准描述（SAP MARA.NORMT）
    /// </summary>
    public string? IndustryStandardDescription { get; set; } = string.Empty;

    /// <summary>
    /// 实验室/设计室（SAP MARA.LABOR）
    /// </summary>
    public string? LaboratoryDesignOffice { get; set; } = string.Empty;

    /// <summary>
    /// 采购价值码（SAP MARA.EKWSL）
    /// </summary>
    public string? PurchasingValueKey { get; set; } = string.Empty;

    /// <summary>
    /// 毛重（SAP MARA.BRGEW）
    /// </summary>
    public decimal? GrossWeight { get; set; }

    /// <summary>
    /// 净重（SAP MARA.NTGEW）
    /// </summary>
    public decimal? NetWeight { get; set; }

    /// <summary>
    /// 重量单位（SAP MARA.GEWEI）
    /// </summary>
    public string? WeightUnit { get; set; } = string.Empty;

    /// <summary>
    /// 体积（SAP MARA.VOLUM）
    /// </summary>
    public decimal? Volume { get; set; }

    /// <summary>
    /// 体积单位（SAP MARA.VOLEH）
    /// </summary>
    public string? VolumeUnit { get; set; } = string.Empty;

    /// <summary>
    /// 容器要求（SAP MARA.BEHVO）
    /// </summary>
    public string? ContainerRequirements { get; set; } = string.Empty;

    /// <summary>
    /// 仓储条件（SAP MARA.RAUBE）
    /// </summary>
    public string? StorageConditions { get; set; } = string.Empty;

    /// <summary>
    /// 温度条件（SAP MARA.TEMPB）
    /// </summary>
    public string? TemperatureConditions { get; set; } = string.Empty;

    /// <summary>
    /// 低层码（SAP MARA.DISST）
    /// </summary>
    public string? LowLevelCode { get; set; } = string.Empty;

    /// <summary>
    /// 运输组（SAP MARA.TRAGR）
    /// </summary>
    public string? TransportationGroup { get; set; } = string.Empty;

    /// <summary>
    /// 危险品编码（SAP MARA.STOFF）
    /// </summary>
    public string? HazardousMaterialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 产品组（SAP MARA.SPART）
    /// </summary>
    public string? Division { get; set; } = string.Empty;

    /// <summary>
    /// 竞争对手（SAP MARA.KUNNR）
    /// </summary>
    public string? Competitor { get; set; } = string.Empty;

    /// <summary>
    /// 欧洲商品号（旧）（SAP MARA.EANNR）
    /// </summary>
    public string? EuropeanArticleNumberObsolete { get; set; } = string.Empty;

    /// <summary>
    /// 收发货凭证打印数量（SAP MARA.WESCH）
    /// </summary>
    public decimal? GrGiSlipQuantity { get; set; }

    /// <summary>
    /// 采购规则（SAP MARA.BWVOR）
    /// </summary>
    public string? ProcurementRule { get; set; } = string.Empty;

    /// <summary>
    /// 货源（SAP MARA.BWSCL）
    /// </summary>
    public string? SourceOfSupply { get; set; } = string.Empty;

    /// <summary>
    /// 季节类别（SAP MARA.SAISO）
    /// </summary>
    public string? SeasonCategory { get; set; } = string.Empty;

    /// <summary>
    /// 标签类型（SAP MARA.ETIAR）
    /// </summary>
    public string? LabelType { get; set; } = string.Empty;

    /// <summary>
    /// 标签格式（SAP MARA.ETIFO）
    /// </summary>
    public string? LabelForm { get; set; } = string.Empty;

    /// <summary>
    /// 已停用字段（SAP MARA.ENTAR）
    /// </summary>
    public string? DeactivatedField { get; set; } = string.Empty;

    /// <summary>
    /// 国际商品编码EAN/UPC（SAP MARA.EAN11）
    /// </summary>
    public string? InternationalArticleNumber { get; set; } = string.Empty;

    /// <summary>
    /// EAN类别（SAP MARA.NUMTP）
    /// </summary>
    public string? EanCategory { get; set; } = string.Empty;

    /// <summary>
    /// 长度（SAP MARA.LAENG）
    /// </summary>
    public decimal? Length { get; set; }

    /// <summary>
    /// 宽度（SAP MARA.BREIT）
    /// </summary>
    public decimal? Width { get; set; }

    /// <summary>
    /// 高度（SAP MARA.HOEHE）
    /// </summary>
    public decimal? Height { get; set; }

    /// <summary>
    /// 长宽高单位（SAP MARA.MEABM）
    /// </summary>
    public string? DimensionUnit { get; set; } = string.Empty;

    /// <summary>
    /// 产品层次（SAP MARA.PRDHA）
    /// </summary>
    public string? ProductHierarchy { get; set; } = string.Empty;

    /// <summary>
    /// 库存调拨净更改成本核算（SAP MARA.AEKLK）
    /// </summary>
    public string? StockTransferNetChangeCosting { get; set; } = string.Empty;

    /// <summary>
    /// CAD标识（SAP MARA.CADKZ）
    /// </summary>
    public string? CadIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 采购QM激活（SAP MARA.QMPUR）
    /// </summary>
    public string? QmInProcurement { get; set; } = string.Empty;

    /// <summary>
    /// 允许包装重量（SAP MARA.ERGEW）
    /// </summary>
    public decimal? AllowedPackagingWeight { get; set; }

    /// <summary>
    /// 允许包装重量单位（SAP MARA.ERGEI）
    /// </summary>
    public string? AllowedPackagingWeightUnit { get; set; } = string.Empty;

    /// <summary>
    /// 允许包装体积（SAP MARA.ERVOL）
    /// </summary>
    public decimal? AllowedPackagingVolume { get; set; }

    /// <summary>
    /// 允许包装体积单位（SAP MARA.ERVOE）
    /// </summary>
    public string? AllowedPackagingVolumeUnit { get; set; } = string.Empty;

    /// <summary>
    /// 超重容差（SAP MARA.GEWTO）
    /// </summary>
    public decimal? ExcessWeightTolerance { get; set; }

    /// <summary>
    /// 超体积容差（SAP MARA.VOLTO）
    /// </summary>
    public decimal? ExcessVolumeTolerance { get; set; }

    /// <summary>
    /// 可变采购订单单位（SAP MARA.VABME）
    /// </summary>
    public string? VariablePurchaseOrderUnit { get; set; } = string.Empty;

    /// <summary>
    /// 已分配修订级别（SAP MARA.KZREV）
    /// </summary>
    public string? RevisionLevelAssigned { get; set; } = string.Empty;

    /// <summary>
    /// 可配置物料（SAP MARA.KZKFG）
    /// </summary>
    public string? ConfigurableMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 批次管理要求（SAP MARA.XCHPF）
    /// </summary>
    public string? BatchManagementRequired { get; set; } = string.Empty;

    /// <summary>
    /// 包装物料类型（SAP MARA.VHART）
    /// </summary>
    public string? PackagingMaterialType { get; set; } = string.Empty;

    /// <summary>
    /// 最大装载量（体积）（SAP MARA.FUELG）
    /// </summary>
    public decimal? MaximumLevelByVolume { get; set; }

    /// <summary>
    /// 堆叠因子（SAP MARA.STFAK）
    /// </summary>
    public int? StackingFactor { get; set; }

    /// <summary>
    /// 包装物料组（SAP MARA.MAGRV）
    /// </summary>
    public string? PackagingMaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 权限组（SAP MARA.BEGRU）
    /// </summary>
    public string? AuthorizationGroup { get; set; } = string.Empty;

    /// <summary>
    /// 有效起始日期（SAP MARA.DATAB）
    /// </summary>
    public DateTime? ValidFromDate { get; set; }

    /// <summary>
    /// 季节年份（SAP MARA.SAISJ）
    /// </summary>
    public string? SeasonYear { get; set; } = string.Empty;

    /// <summary>
    /// 价格带类别（SAP MARA.PLGTP）
    /// </summary>
    public string? PriceBandCategory { get; set; } = string.Empty;

    /// <summary>
    /// 空容器BOM（SAP MARA.MLGUT）
    /// </summary>
    public string? EmptiesBillOfMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 外部物料组（SAP MARA.EXTWG）
    /// </summary>
    public string? ExternalMaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 跨工厂可配置物料（SAP MARA.SATNR）
    /// </summary>
    public string? CrossPlantConfigurableMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 物料类别（SAP MARA.ATTYP）
    /// </summary>
    public string? MaterialCategory { get; set; } = string.Empty;

    /// <summary>
    /// 联产品标识（SAP MARA.KZKUP）
    /// </summary>
    public string? CoProductIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 后续物料标识（SAP MARA.KZNFM）
    /// </summary>
    public string? FollowUpMaterialIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 定价参考物料（SAP MARA.PMATA）
    /// </summary>
    public string? PricingReferenceMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 跨工厂物料状态（SAP MARA.MSTAE）
    /// </summary>
    public string? CrossPlantMaterialStatus { get; set; } = string.Empty;

    /// <summary>
    /// 跨分销链物料状态（SAP MARA.MSTAV）
    /// </summary>
    public string? CrossDistributionChainStatus { get; set; } = string.Empty;

    /// <summary>
    /// 跨工厂状态生效日期（SAP MARA.MSTDE）
    /// </summary>
    public DateTime? CrossPlantStatusValidFrom { get; set; }

    /// <summary>
    /// 跨分销链状态生效日期（SAP MARA.MSTDV）
    /// </summary>
    public DateTime? CrossDistributionStatusValidFrom { get; set; }

    /// <summary>
    /// 物料税分类（SAP MARA.TAKLV）
    /// </summary>
    public string? TaxClassification { get; set; } = string.Empty;

    /// <summary>
    /// 目录参数文件（SAP MARA.RBNRM）
    /// </summary>
    public string? CatalogProfile { get; set; } = string.Empty;

    /// <summary>
    /// 最短剩余货架寿命（SAP MARA.MHDRZ）
    /// </summary>
    public decimal? MinimumRemainingShelfLife { get; set; }

    /// <summary>
    /// 总货架寿命（SAP MARA.MHDHB）
    /// </summary>
    public decimal? TotalShelfLife { get; set; }

    /// <summary>
    /// 仓储百分比（SAP MARA.MHDLP）
    /// </summary>
    public decimal? StoragePercentage { get; set; }

    /// <summary>
    /// 含量单位（SAP MARA.INHME）
    /// </summary>
    public string? ContentUnit { get; set; } = string.Empty;

    /// <summary>
    /// 净含量（SAP MARA.INHAL）
    /// </summary>
    public decimal? NetContents { get; set; }

    /// <summary>
    /// 比较价格单位（SAP MARA.VPREH）
    /// </summary>
    public decimal? ComparisonPriceUnit { get; set; }

    /// <summary>
    /// 标签物料分组（SAP MARA.ETIAG）
    /// </summary>
    public string? LabelingMaterialGrouping { get; set; } = string.Empty;

    /// <summary>
    /// 毛含量（SAP MARA.INHBR）
    /// </summary>
    public decimal? GrossContents { get; set; }

    /// <summary>
    /// 数量换算方法（SAP MARA.CMETH）
    /// </summary>
    public string? QuantityConversionMethod { get; set; } = string.Empty;

    /// <summary>
    /// 内部对象号（SAP MARA.CUOBF）
    /// </summary>
    public string? InternalObjectNumber { get; set; } = string.Empty;

    /// <summary>
    /// 环境相关（SAP MARA.KZUMW）
    /// </summary>
    public string? EnvironmentallyRelevant { get; set; } = string.Empty;

    /// <summary>
    /// 产品分配确定过程（SAP MARA.KOSCH）
    /// </summary>
    public string? ProductAllocationProcedure { get; set; } = string.Empty;

    /// <summary>
    /// 变式定价参数文件（SAP MARA.SPROF）
    /// </summary>
    public string? VariantPricingProfile { get; set; } = string.Empty;

    /// <summary>
    /// 实物折扣资格（SAP MARA.NRFHG）
    /// </summary>
    public string? DiscountInKind { get; set; } = string.Empty;

    /// <summary>
    /// 制造商零件号（SAP MARA.MFRPN）
    /// </summary>
    public string? ManufacturerPartNumber { get; set; } = string.Empty;

    /// <summary>
    /// 制造商编码（SAP MARA.MFRNR）
    /// </summary>
    public string? ManufacturerNumber { get; set; } = string.Empty;

    /// <summary>
    /// 自有库存管理物料号（SAP MARA.BMATN）
    /// </summary>
    public string? InventoryManagedMaterialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 制造商零件参数文件（SAP MARA.MPROF）
    /// </summary>
    public string? ManufacturerPartProfile { get; set; } = string.Empty;

    /// <summary>
    /// 计量单位用途（SAP MARA.KZWSM）
    /// </summary>
    public string? UnitsOfMeasureUsage { get; set; } = string.Empty;

    /// <summary>
    /// 季节推出（SAP MARA.SAITY）
    /// </summary>
    public string? SeasonRollout { get; set; } = string.Empty;

    /// <summary>
    /// 危险品参数文件（SAP MARA.PROFL）
    /// </summary>
    public string? DangerousGoodsProfile { get; set; } = string.Empty;

    /// <summary>
    /// 高粘度（SAP MARA.IHIVI）
    /// </summary>
    public string? HighlyViscous { get; set; } = string.Empty;

    /// <summary>
    /// 散装/液体（SAP MARA.ILOOS）
    /// </summary>
    public string? InBulkLiquid { get; set; } = string.Empty;

    /// <summary>
    /// 序列号明确级别（SAP MARA.SERLV）
    /// </summary>
    public string? SerialNumberExplicitness { get; set; } = string.Empty;

    /// <summary>
    /// 封闭包装（SAP MARA.KZGVH）
    /// </summary>
    public string? ClosedPackaging { get; set; } = string.Empty;

    /// <summary>
    /// 需批准批次记录（SAP MARA.XGCHP）
    /// </summary>
    public string? ApprovedBatchRecordRequired { get; set; } = string.Empty;

    /// <summary>
    /// 有效性参数覆盖（SAP MARA.KZEFF）
    /// </summary>
    public string? EffectivityParameterOverride { get; set; } = string.Empty;

    /// <summary>
    /// 物料完成级别（SAP MARA.COMPL）
    /// </summary>
    public string? MaterialCompletionLevel { get; set; } = string.Empty;

    /// <summary>
    /// 货架寿命期间标识（SAP MARA.IPRKZ）
    /// </summary>
    public string? ShelfLifePeriodIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 货架寿命舍入规则（SAP MARA.RDMHD）
    /// </summary>
    public string? ShelfLifeRoundingRule { get; set; } = string.Empty;

    /// <summary>
    /// 包装打印产品成分（SAP MARA.PRZUS）
    /// </summary>
    public string? ProductCompositionOnPackaging { get; set; } = string.Empty;

    /// <summary>
    /// 通用项目类别组（SAP MARA.MTPOS_MARA）
    /// </summary>
    public string? GeneralItemCategoryGroup { get; set; } = string.Empty;

    /// <summary>
    /// 后勤变式通用物料（SAP MARA.BFLME）
    /// </summary>
    public string? LogisticalVariants { get; set; } = string.Empty;

    /// <summary>
    /// 物料锁定（SAP MARA.MATFI）
    /// </summary>
    public string? MaterialLocked { get; set; } = string.Empty;

    /// <summary>
    /// 配置管理相关（SAP MARA.CMREL）
    /// </summary>
    public string? ConfigurationManagementRelevant { get; set; } = string.Empty;

    /// <summary>
    /// 品种清单类型（SAP MARA.BBTYP）
    /// </summary>
    public string? AssortmentListType { get; set; } = string.Empty;

    /// <summary>
    /// 到期日期类型（SAP MARA.SLED_BBD）
    /// </summary>
    public string? ExpirationDateType { get; set; } = string.Empty;

    /// <summary>
    /// GTIN变式（SAP MARA.GTIN_VARIANT）
    /// </summary>
    public string? GtinVariant { get; set; } = string.Empty;

    /// <summary>
    /// 通用物料号（SAP MARA.GENNR）
    /// </summary>
    public string? GenericMaterialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 相同包装参考物料（SAP MARA.RMATP）
    /// </summary>
    public string? SamePackingReferenceMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 全球数据同步相关（SAP MARA.GDS_RELEVANT）
    /// </summary>
    public string? GlobalDataSyncRelevant { get; set; } = string.Empty;

    /// <summary>
    /// 原产地验收（SAP MARA.WEORA）
    /// </summary>
    public string? AcceptanceAtOrigin { get; set; } = string.Empty;

    /// <summary>
    /// 标准HU类型（SAP MARA.HUTYP_DFLT）
    /// </summary>
    public string? StandardHuType { get; set; } = string.Empty;

    /// <summary>
    /// 易被盗（SAP MARA.PILFERABLE）
    /// </summary>
    public string? Pilferable { get; set; } = string.Empty;

    /// <summary>
    /// 仓储存储条件（SAP MARA.WHSTC）
    /// </summary>
    public string? WarehouseStorageCondition { get; set; } = string.Empty;

    /// <summary>
    /// 仓储物料组（SAP MARA.WHMATGR）
    /// </summary>
    public string? WarehouseMaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 处理标识（SAP MARA.HNDLCODE）
    /// </summary>
    public string? HandlingIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 危险物质相关（SAP MARA.HAZMAT）
    /// </summary>
    public string? HazardousSubstancesRelevant { get; set; } = string.Empty;

    /// <summary>
    /// 处理单元类型（SAP MARA.HUTYP）
    /// </summary>
    public string? HandlingUnitType { get; set; } = string.Empty;

    /// <summary>
    /// 可变皮重（SAP MARA.TARE_VAR）
    /// </summary>
    public string? VariableTareWeight { get; set; } = string.Empty;

    /// <summary>
    /// 最大允许容量（SAP MARA.MAXC）
    /// </summary>
    public decimal? MaximumAllowedCapacity { get; set; }

    /// <summary>
    /// 超容量容差（SAP MARA.MAXC_TOL）
    /// </summary>
    public decimal? OvercapacityTolerance { get; set; }

    /// <summary>
    /// 最大包装长度（SAP MARA.MAXL）
    /// </summary>
    public decimal? MaximumPackingLength { get; set; }

    /// <summary>
    /// 最大包装宽度（SAP MARA.MAXB）
    /// </summary>
    public decimal? MaximumPackingWidth { get; set; }

    /// <summary>
    /// 最大包装高度（SAP MARA.MAXH）
    /// </summary>
    public decimal? MaximumPackingHeight { get; set; }

    /// <summary>
    /// 最大包装尺寸单位（SAP MARA.MAXDIM_UOM）
    /// </summary>
    public string? MaximumPackingDimensionUnit { get; set; } = string.Empty;

    /// <summary>
    /// 原产国（SAP MARA.HERKL）
    /// </summary>
    public string? CountryOfOrigin { get; set; } = string.Empty;

    /// <summary>
    /// 物料运费组（SAP MARA.MFRGR）
    /// </summary>
    public string? MaterialFreightGroup { get; set; } = string.Empty;

    /// <summary>
    /// 隔离期（SAP MARA.QQTIME）
    /// </summary>
    public decimal? QuarantinePeriod { get; set; }

    /// <summary>
    /// 隔离期单位（SAP MARA.QQTIMEUOM）
    /// </summary>
    public string? QuarantinePeriodUnit { get; set; } = string.Empty;

    /// <summary>
    /// 质检组（SAP MARA.QGRP）
    /// </summary>
    public string? QualityInspectionGroup { get; set; } = string.Empty;

    /// <summary>
    /// 序列号参数文件（SAP MARA.SERIAL）
    /// </summary>
    public string? SerialNumberProfile { get; set; } = string.Empty;

    /// <summary>
    /// 表单名称（SAP MARA.PS_SMARTFORM）
    /// </summary>
    public string? FormName { get; set; } = string.Empty;

    /// <summary>
    /// 后勤计量单位（SAP MARA.LOGUNIT）
    /// </summary>
    public string? LogisticsUnitOfMeasure { get; set; } = string.Empty;

    /// <summary>
    /// 捕捞重量物料（SAP MARA.CWQREL）
    /// </summary>
    public string? CatchWeightMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 捕捞重量参数文件（SAP MARA.CWQPROC）
    /// </summary>
    public string? CatchWeightProfile { get; set; } = string.Empty;

    /// <summary>
    /// 捕捞重量容差组（SAP MARA.CWQTOLGR）
    /// </summary>
    public string? CatchWeightToleranceGroup { get; set; } = string.Empty;

    /// <summary>
    /// 调整参数文件（SAP MARA.ADPROF）
    /// </summary>
    public string? AdjustmentProfile { get; set; } = string.Empty;

    /// <summary>
    /// 知识产权ID（SAP MARA.IPMIPPRODUCT）
    /// </summary>
    public string? IntellectualPropertyId { get; set; } = string.Empty;

    /// <summary>
    /// 允许变式价格（SAP MARA.ALLOW_PMAT_IGNO）
    /// </summary>
    public string? VariantPriceAllowed { get; set; } = string.Empty;

    /// <summary>
    /// 介质（SAP MARA.MEDIUM）
    /// </summary>
    public string? Medium { get; set; } = string.Empty;

    /// <summary>
    /// 实物商品（SAP MARA.COMMODITY）
    /// </summary>
    public string? PhysicalCommodity { get; set; } = string.Empty;

    /// <summary>
    /// 动物源（SAP MARA.ANIMAL_ORIGIN）
    /// </summary>
    public string? AnimalOrigin { get; set; } = string.Empty;

    /// <summary>
    /// 纺织成分功能（SAP MARA.TEXTILE_COMP_IND）
    /// </summary>
    public string? TextileCompositionFunction { get; set; } = string.Empty;

    /// <summary>
    /// 细分结构（SAP MARA.SGT_CSGR）
    /// </summary>
    public string? SegmentationStructure { get; set; } = string.Empty;

    /// <summary>
    /// 细分策略（SAP MARA.SGT_COVSA）
    /// </summary>
    public string? SegmentationStrategy { get; set; } = string.Empty;

    /// <summary>
    /// 细分状态（SAP MARA.SGT_STAT）
    /// </summary>
    public string? SegmentationStatus { get; set; } = string.Empty;

    /// <summary>
    /// 细分范围（SAP MARA.SGT_SCOPE）
    /// </summary>
    public string? SegmentationScope { get; set; } = string.Empty;

    /// <summary>
    /// 细分相关（SAP MARA.SGT_REL）
    /// </summary>
    public string? SegmentationRelevant { get; set; } = string.Empty;

    /// <summary>
    /// 时装属性1（SAP MARA.FSH_MG_AT1）
    /// </summary>
    public string? FashionAttribute1 { get; set; } = string.Empty;

    /// <summary>
    /// 时装属性2（SAP MARA.FSH_MG_AT2）
    /// </summary>
    public string? FashionAttribute2 { get; set; } = string.Empty;

    /// <summary>
    /// 时装属性3（SAP MARA.FSH_MG_AT3）
    /// </summary>
    public string? FashionAttribute3 { get; set; } = string.Empty;

    /// <summary>
    /// 季节使用标识（SAP MARA.FSH_SEALV）
    /// </summary>
    public string? SeasonUsageIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 库存季节激活（SAP MARA.FSH_SEAIM）
    /// </summary>
    public string? SeasonActiveInInventory { get; set; } = string.Empty;

    /// <summary>
    /// 特性转换ID（SAP MARA.FSH_SC_MID）
    /// </summary>
    public string? CharacteristicConversionId { get; set; } = string.Empty;

    /// <summary>
    /// ANP代码（SAP MARA.ANP）
    /// </summary>
    public string? AnpCode { get; set; } = string.Empty;

    /// <summary>
    /// 危险品包装状态（SAP MARA.DG_PACK_STATUS）
    /// </summary>
    public string? DangerousGoodsPackagingStatus { get; set; } = string.Empty;

    /// <summary>
    /// 物料条件管理（SAP MARA.MCOND）
    /// </summary>
    public string? MaterialConditionManagement { get; set; } = string.Empty;

    /// <summary>
    /// 退货代码（SAP MARA.RETDELC）
    /// </summary>
    public string? ReturnCode { get; set; } = string.Empty;

    /// <summary>
    /// 退回后勤级别（SAP MARA.LOGLEV_RETO）
    /// </summary>
    public string? ReturnToLogisticsLevel { get; set; } = string.Empty;

    /// <summary>
    /// NATO物料识别号（SAP MARA.NSNID）
    /// </summary>
    public string? NatoItemIdentificationNumber { get; set; } = string.Empty;

    /// <summary>
    /// FFF类别（SAP MARA.IMATN）
    /// </summary>
    public string? FffClass { get; set; } = string.Empty;

    /// <summary>
    /// 替代链编码（SAP MARA.PICNUM）
    /// </summary>
    public string? SupersessionChainNumber { get; set; } = string.Empty;

    /// <summary>
    /// 季节采购创建状态（SAP MARA.BSTAT）
    /// </summary>
    public string? SeasonalProcurementCreationStatus { get; set; } = string.Empty;

    /// <summary>
    /// 颜色特性内部号（SAP MARA.COLOR_ATINN）
    /// </summary>
    public string? ColorCharacteristicInternalNumber { get; set; } = string.Empty;

    /// <summary>
    /// 主尺码特性内部号（SAP MARA.SIZE1_ATINN）
    /// </summary>
    public string? MainSizeCharacteristicInternalNumber { get; set; } = string.Empty;

    /// <summary>
    /// 次尺码特性内部号（SAP MARA.SIZE2_ATINN）
    /// </summary>
    public string? SecondSizeCharacteristicInternalNumber { get; set; } = string.Empty;

    /// <summary>
    /// 颜色（SAP MARA.COLOR）
    /// </summary>
    public string? Color { get; set; } = string.Empty;

    /// <summary>
    /// 主尺码（SAP MARA.SIZE1）
    /// </summary>
    public string? MainSize { get; set; } = string.Empty;

    /// <summary>
    /// 次尺码（SAP MARA.SIZE2）
    /// </summary>
    public string? SecondSize { get; set; } = string.Empty;

    /// <summary>
    /// 评估特性值（SAP MARA.FREE_CHAR）
    /// </summary>
    public string? EvaluationCharacteristicValue { get; set; } = string.Empty;

    /// <summary>
    /// 护理代码（SAP MARA.CARE_CODE）
    /// </summary>
    public string? CareCode { get; set; } = string.Empty;

    /// <summary>
    /// 品牌（SAP MARA.BRAND_ID）
    /// </summary>
    public string? BrandId { get; set; } = string.Empty;

    /// <summary>
    /// 纤维代码1（SAP MARA.FIBER_CODE1）
    /// </summary>
    public string? FiberCode1 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维占比1（SAP MARA.FIBER_PART1）
    /// </summary>
    public string? FiberPart1 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维代码2（SAP MARA.FIBER_CODE2）
    /// </summary>
    public string? FiberCode2 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维占比2（SAP MARA.FIBER_PART2）
    /// </summary>
    public string? FiberPart2 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维代码3（SAP MARA.FIBER_CODE3）
    /// </summary>
    public string? FiberCode3 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维占比3（SAP MARA.FIBER_PART3）
    /// </summary>
    public string? FiberPart3 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维代码4（SAP MARA.FIBER_CODE4）
    /// </summary>
    public string? FiberCode4 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维占比4（SAP MARA.FIBER_PART4）
    /// </summary>
    public string? FiberPart4 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维代码5（SAP MARA.FIBER_CODE5）
    /// </summary>
    public string? FiberCode5 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维占比5（SAP MARA.FIBER_PART5）
    /// </summary>
    public string? FiberPart5 { get; set; } = string.Empty;

    /// <summary>
    /// 时装等级（SAP MARA.FASHGRD）
    /// </summary>
    public string? FashionGrade { get; set; } = string.Empty;

    /// <summary>
    /// 物料状态（字典 sys_normal_disable_status；0=禁用，1=启用，2=锁定；平台启用态，非 SAP MSTAE）
    /// </summary>
    public int? MaterialStatus { get; set; }

    /// <summary>
    /// 多语言描述列表（主子表关系；对齐 SAP MAKT）（子表，级联保存）
    /// </summary>
    public List<TaktMaterialDescriptionCreateDto>? Descriptions { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

/// <summary>
/// Material 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktMaterialImportDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（SAP MARA.MATNR）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 完整维护状态（SAP MARA.VPSTA）
    /// </summary>
    public string? CompleteMaintenanceStatus { get; set; } = string.Empty;

    /// <summary>
    /// 维护状态（SAP MARA.PSTAT）
    /// </summary>
    public string? MaintenanceStatus { get; set; } = string.Empty;

    /// <summary>
    /// 客户级删除标记（SAP MARA.LVORM）
    /// </summary>
    public string? ClientDeletionFlag { get; set; } = string.Empty;

    /// <summary>
    /// 物料类型（SAP MARA.MTART）
    /// </summary>
    public string? MaterialType { get; set; } = string.Empty;

    /// <summary>
    /// 行业领域（SAP MARA.MBRSH）
    /// </summary>
    public string? IndustrySector { get; set; } = string.Empty;

    /// <summary>
    /// 物料组（SAP MARA.MATKL）
    /// </summary>
    public string? MaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料号（SAP MARA.BISMT）
    /// </summary>
    public string? OldMaterialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 基本计量单位（SAP MARA.MEINS）
    /// </summary>
    public string? BaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单单位（SAP MARA.BSTME）
    /// </summary>
    public string? OrderUnit { get; set; } = string.Empty;

    /// <summary>
    /// 单据号（SAP MARA.ZEINR）
    /// </summary>
    public string? DocumentNumber { get; set; } = string.Empty;

    /// <summary>
    /// 单据类型（SAP MARA.ZEIAR）
    /// </summary>
    public string? DocumentType { get; set; } = string.Empty;

    /// <summary>
    /// 单据版本（SAP MARA.ZEIVR）
    /// </summary>
    public string? DocumentVersion { get; set; } = string.Empty;

    /// <summary>
    /// 单据页格式（SAP MARA.ZEIFO）
    /// </summary>
    public string? DocumentPageFormat { get; set; } = string.Empty;

    /// <summary>
    /// 单据更改号（SAP MARA.AESZN）
    /// </summary>
    public string? DocumentChangeNumber { get; set; } = string.Empty;

    /// <summary>
    /// 单据页号（SAP MARA.BLATT）
    /// </summary>
    public string? DocumentPageNumber { get; set; } = string.Empty;

    /// <summary>
    /// 单据页数（SAP MARA.BLANZ）
    /// </summary>
    public string? DocumentSheetCount { get; set; } = string.Empty;

    /// <summary>
    /// 生产/检验备忘（SAP MARA.FERTH）
    /// </summary>
    public string? ProductionInspectionMemo { get; set; } = string.Empty;

    /// <summary>
    /// 生产备忘页格式（SAP MARA.FORMT）
    /// </summary>
    public string? ProductionMemoPageFormat { get; set; } = string.Empty;

    /// <summary>
    /// 尺寸/规格（SAP MARA.GROES）
    /// </summary>
    public string? SizeDimensions { get; set; } = string.Empty;

    /// <summary>
    /// 基本物料（材质）（SAP MARA.WRKST）
    /// </summary>
    public string? BasicMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 行业标准描述（SAP MARA.NORMT）
    /// </summary>
    public string? IndustryStandardDescription { get; set; } = string.Empty;

    /// <summary>
    /// 实验室/设计室（SAP MARA.LABOR）
    /// </summary>
    public string? LaboratoryDesignOffice { get; set; } = string.Empty;

    /// <summary>
    /// 采购价值码（SAP MARA.EKWSL）
    /// </summary>
    public string? PurchasingValueKey { get; set; } = string.Empty;

    /// <summary>
    /// 毛重（SAP MARA.BRGEW）
    /// </summary>
    public decimal? GrossWeight { get; set; }

    /// <summary>
    /// 净重（SAP MARA.NTGEW）
    /// </summary>
    public decimal? NetWeight { get; set; }

    /// <summary>
    /// 重量单位（SAP MARA.GEWEI）
    /// </summary>
    public string? WeightUnit { get; set; } = string.Empty;

    /// <summary>
    /// 体积（SAP MARA.VOLUM）
    /// </summary>
    public decimal? Volume { get; set; }

    /// <summary>
    /// 体积单位（SAP MARA.VOLEH）
    /// </summary>
    public string? VolumeUnit { get; set; } = string.Empty;

    /// <summary>
    /// 容器要求（SAP MARA.BEHVO）
    /// </summary>
    public string? ContainerRequirements { get; set; } = string.Empty;

    /// <summary>
    /// 仓储条件（SAP MARA.RAUBE）
    /// </summary>
    public string? StorageConditions { get; set; } = string.Empty;

    /// <summary>
    /// 温度条件（SAP MARA.TEMPB）
    /// </summary>
    public string? TemperatureConditions { get; set; } = string.Empty;

    /// <summary>
    /// 低层码（SAP MARA.DISST）
    /// </summary>
    public string? LowLevelCode { get; set; } = string.Empty;

    /// <summary>
    /// 运输组（SAP MARA.TRAGR）
    /// </summary>
    public string? TransportationGroup { get; set; } = string.Empty;

    /// <summary>
    /// 危险品编码（SAP MARA.STOFF）
    /// </summary>
    public string? HazardousMaterialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 产品组（SAP MARA.SPART）
    /// </summary>
    public string? Division { get; set; } = string.Empty;

    /// <summary>
    /// 竞争对手（SAP MARA.KUNNR）
    /// </summary>
    public string? Competitor { get; set; } = string.Empty;

    /// <summary>
    /// 欧洲商品号（旧）（SAP MARA.EANNR）
    /// </summary>
    public string? EuropeanArticleNumberObsolete { get; set; } = string.Empty;

    /// <summary>
    /// 收发货凭证打印数量（SAP MARA.WESCH）
    /// </summary>
    public decimal? GrGiSlipQuantity { get; set; }

    /// <summary>
    /// 采购规则（SAP MARA.BWVOR）
    /// </summary>
    public string? ProcurementRule { get; set; } = string.Empty;

    /// <summary>
    /// 货源（SAP MARA.BWSCL）
    /// </summary>
    public string? SourceOfSupply { get; set; } = string.Empty;

    /// <summary>
    /// 季节类别（SAP MARA.SAISO）
    /// </summary>
    public string? SeasonCategory { get; set; } = string.Empty;

    /// <summary>
    /// 标签类型（SAP MARA.ETIAR）
    /// </summary>
    public string? LabelType { get; set; } = string.Empty;

    /// <summary>
    /// 标签格式（SAP MARA.ETIFO）
    /// </summary>
    public string? LabelForm { get; set; } = string.Empty;

    /// <summary>
    /// 已停用字段（SAP MARA.ENTAR）
    /// </summary>
    public string? DeactivatedField { get; set; } = string.Empty;

    /// <summary>
    /// 国际商品编码EAN/UPC（SAP MARA.EAN11）
    /// </summary>
    public string? InternationalArticleNumber { get; set; } = string.Empty;

    /// <summary>
    /// EAN类别（SAP MARA.NUMTP）
    /// </summary>
    public string? EanCategory { get; set; } = string.Empty;

    /// <summary>
    /// 长度（SAP MARA.LAENG）
    /// </summary>
    public decimal? Length { get; set; }

    /// <summary>
    /// 宽度（SAP MARA.BREIT）
    /// </summary>
    public decimal? Width { get; set; }

    /// <summary>
    /// 高度（SAP MARA.HOEHE）
    /// </summary>
    public decimal? Height { get; set; }

    /// <summary>
    /// 长宽高单位（SAP MARA.MEABM）
    /// </summary>
    public string? DimensionUnit { get; set; } = string.Empty;

    /// <summary>
    /// 产品层次（SAP MARA.PRDHA）
    /// </summary>
    public string? ProductHierarchy { get; set; } = string.Empty;

    /// <summary>
    /// 库存调拨净更改成本核算（SAP MARA.AEKLK）
    /// </summary>
    public string? StockTransferNetChangeCosting { get; set; } = string.Empty;

    /// <summary>
    /// CAD标识（SAP MARA.CADKZ）
    /// </summary>
    public string? CadIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 采购QM激活（SAP MARA.QMPUR）
    /// </summary>
    public string? QmInProcurement { get; set; } = string.Empty;

    /// <summary>
    /// 允许包装重量（SAP MARA.ERGEW）
    /// </summary>
    public decimal? AllowedPackagingWeight { get; set; }

    /// <summary>
    /// 允许包装重量单位（SAP MARA.ERGEI）
    /// </summary>
    public string? AllowedPackagingWeightUnit { get; set; } = string.Empty;

    /// <summary>
    /// 允许包装体积（SAP MARA.ERVOL）
    /// </summary>
    public decimal? AllowedPackagingVolume { get; set; }

    /// <summary>
    /// 允许包装体积单位（SAP MARA.ERVOE）
    /// </summary>
    public string? AllowedPackagingVolumeUnit { get; set; } = string.Empty;

    /// <summary>
    /// 超重容差（SAP MARA.GEWTO）
    /// </summary>
    public decimal? ExcessWeightTolerance { get; set; }

    /// <summary>
    /// 超体积容差（SAP MARA.VOLTO）
    /// </summary>
    public decimal? ExcessVolumeTolerance { get; set; }

    /// <summary>
    /// 可变采购订单单位（SAP MARA.VABME）
    /// </summary>
    public string? VariablePurchaseOrderUnit { get; set; } = string.Empty;

    /// <summary>
    /// 已分配修订级别（SAP MARA.KZREV）
    /// </summary>
    public string? RevisionLevelAssigned { get; set; } = string.Empty;

    /// <summary>
    /// 可配置物料（SAP MARA.KZKFG）
    /// </summary>
    public string? ConfigurableMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 批次管理要求（SAP MARA.XCHPF）
    /// </summary>
    public string? BatchManagementRequired { get; set; } = string.Empty;

    /// <summary>
    /// 包装物料类型（SAP MARA.VHART）
    /// </summary>
    public string? PackagingMaterialType { get; set; } = string.Empty;

    /// <summary>
    /// 最大装载量（体积）（SAP MARA.FUELG）
    /// </summary>
    public decimal? MaximumLevelByVolume { get; set; }

    /// <summary>
    /// 堆叠因子（SAP MARA.STFAK）
    /// </summary>
    public int? StackingFactor { get; set; }

    /// <summary>
    /// 包装物料组（SAP MARA.MAGRV）
    /// </summary>
    public string? PackagingMaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 权限组（SAP MARA.BEGRU）
    /// </summary>
    public string? AuthorizationGroup { get; set; } = string.Empty;

    /// <summary>
    /// 有效起始日期（SAP MARA.DATAB）
    /// </summary>
    public DateTime? ValidFromDate { get; set; }

    /// <summary>
    /// 季节年份（SAP MARA.SAISJ）
    /// </summary>
    public string? SeasonYear { get; set; } = string.Empty;

    /// <summary>
    /// 价格带类别（SAP MARA.PLGTP）
    /// </summary>
    public string? PriceBandCategory { get; set; } = string.Empty;

    /// <summary>
    /// 空容器BOM（SAP MARA.MLGUT）
    /// </summary>
    public string? EmptiesBillOfMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 外部物料组（SAP MARA.EXTWG）
    /// </summary>
    public string? ExternalMaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 跨工厂可配置物料（SAP MARA.SATNR）
    /// </summary>
    public string? CrossPlantConfigurableMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 物料类别（SAP MARA.ATTYP）
    /// </summary>
    public string? MaterialCategory { get; set; } = string.Empty;

    /// <summary>
    /// 联产品标识（SAP MARA.KZKUP）
    /// </summary>
    public string? CoProductIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 后续物料标识（SAP MARA.KZNFM）
    /// </summary>
    public string? FollowUpMaterialIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 定价参考物料（SAP MARA.PMATA）
    /// </summary>
    public string? PricingReferenceMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 跨工厂物料状态（SAP MARA.MSTAE）
    /// </summary>
    public string? CrossPlantMaterialStatus { get; set; } = string.Empty;

    /// <summary>
    /// 跨分销链物料状态（SAP MARA.MSTAV）
    /// </summary>
    public string? CrossDistributionChainStatus { get; set; } = string.Empty;

    /// <summary>
    /// 跨工厂状态生效日期（SAP MARA.MSTDE）
    /// </summary>
    public DateTime? CrossPlantStatusValidFrom { get; set; }

    /// <summary>
    /// 跨分销链状态生效日期（SAP MARA.MSTDV）
    /// </summary>
    public DateTime? CrossDistributionStatusValidFrom { get; set; }

    /// <summary>
    /// 物料税分类（SAP MARA.TAKLV）
    /// </summary>
    public string? TaxClassification { get; set; } = string.Empty;

    /// <summary>
    /// 目录参数文件（SAP MARA.RBNRM）
    /// </summary>
    public string? CatalogProfile { get; set; } = string.Empty;

    /// <summary>
    /// 最短剩余货架寿命（SAP MARA.MHDRZ）
    /// </summary>
    public decimal? MinimumRemainingShelfLife { get; set; }

    /// <summary>
    /// 总货架寿命（SAP MARA.MHDHB）
    /// </summary>
    public decimal? TotalShelfLife { get; set; }

    /// <summary>
    /// 仓储百分比（SAP MARA.MHDLP）
    /// </summary>
    public decimal? StoragePercentage { get; set; }

    /// <summary>
    /// 含量单位（SAP MARA.INHME）
    /// </summary>
    public string? ContentUnit { get; set; } = string.Empty;

    /// <summary>
    /// 净含量（SAP MARA.INHAL）
    /// </summary>
    public decimal? NetContents { get; set; }

    /// <summary>
    /// 比较价格单位（SAP MARA.VPREH）
    /// </summary>
    public decimal? ComparisonPriceUnit { get; set; }

    /// <summary>
    /// 标签物料分组（SAP MARA.ETIAG）
    /// </summary>
    public string? LabelingMaterialGrouping { get; set; } = string.Empty;

    /// <summary>
    /// 毛含量（SAP MARA.INHBR）
    /// </summary>
    public decimal? GrossContents { get; set; }

    /// <summary>
    /// 数量换算方法（SAP MARA.CMETH）
    /// </summary>
    public string? QuantityConversionMethod { get; set; } = string.Empty;

    /// <summary>
    /// 内部对象号（SAP MARA.CUOBF）
    /// </summary>
    public string? InternalObjectNumber { get; set; } = string.Empty;

    /// <summary>
    /// 环境相关（SAP MARA.KZUMW）
    /// </summary>
    public string? EnvironmentallyRelevant { get; set; } = string.Empty;

    /// <summary>
    /// 产品分配确定过程（SAP MARA.KOSCH）
    /// </summary>
    public string? ProductAllocationProcedure { get; set; } = string.Empty;

    /// <summary>
    /// 变式定价参数文件（SAP MARA.SPROF）
    /// </summary>
    public string? VariantPricingProfile { get; set; } = string.Empty;

    /// <summary>
    /// 实物折扣资格（SAP MARA.NRFHG）
    /// </summary>
    public string? DiscountInKind { get; set; } = string.Empty;

    /// <summary>
    /// 制造商零件号（SAP MARA.MFRPN）
    /// </summary>
    public string? ManufacturerPartNumber { get; set; } = string.Empty;

    /// <summary>
    /// 制造商编码（SAP MARA.MFRNR）
    /// </summary>
    public string? ManufacturerNumber { get; set; } = string.Empty;

    /// <summary>
    /// 自有库存管理物料号（SAP MARA.BMATN）
    /// </summary>
    public string? InventoryManagedMaterialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 制造商零件参数文件（SAP MARA.MPROF）
    /// </summary>
    public string? ManufacturerPartProfile { get; set; } = string.Empty;

    /// <summary>
    /// 计量单位用途（SAP MARA.KZWSM）
    /// </summary>
    public string? UnitsOfMeasureUsage { get; set; } = string.Empty;

    /// <summary>
    /// 季节推出（SAP MARA.SAITY）
    /// </summary>
    public string? SeasonRollout { get; set; } = string.Empty;

    /// <summary>
    /// 危险品参数文件（SAP MARA.PROFL）
    /// </summary>
    public string? DangerousGoodsProfile { get; set; } = string.Empty;

    /// <summary>
    /// 高粘度（SAP MARA.IHIVI）
    /// </summary>
    public string? HighlyViscous { get; set; } = string.Empty;

    /// <summary>
    /// 散装/液体（SAP MARA.ILOOS）
    /// </summary>
    public string? InBulkLiquid { get; set; } = string.Empty;

    /// <summary>
    /// 序列号明确级别（SAP MARA.SERLV）
    /// </summary>
    public string? SerialNumberExplicitness { get; set; } = string.Empty;

    /// <summary>
    /// 封闭包装（SAP MARA.KZGVH）
    /// </summary>
    public string? ClosedPackaging { get; set; } = string.Empty;

    /// <summary>
    /// 需批准批次记录（SAP MARA.XGCHP）
    /// </summary>
    public string? ApprovedBatchRecordRequired { get; set; } = string.Empty;

    /// <summary>
    /// 有效性参数覆盖（SAP MARA.KZEFF）
    /// </summary>
    public string? EffectivityParameterOverride { get; set; } = string.Empty;

    /// <summary>
    /// 物料完成级别（SAP MARA.COMPL）
    /// </summary>
    public string? MaterialCompletionLevel { get; set; } = string.Empty;

    /// <summary>
    /// 货架寿命期间标识（SAP MARA.IPRKZ）
    /// </summary>
    public string? ShelfLifePeriodIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 货架寿命舍入规则（SAP MARA.RDMHD）
    /// </summary>
    public string? ShelfLifeRoundingRule { get; set; } = string.Empty;

    /// <summary>
    /// 包装打印产品成分（SAP MARA.PRZUS）
    /// </summary>
    public string? ProductCompositionOnPackaging { get; set; } = string.Empty;

    /// <summary>
    /// 通用项目类别组（SAP MARA.MTPOS_MARA）
    /// </summary>
    public string? GeneralItemCategoryGroup { get; set; } = string.Empty;

    /// <summary>
    /// 后勤变式通用物料（SAP MARA.BFLME）
    /// </summary>
    public string? LogisticalVariants { get; set; } = string.Empty;

    /// <summary>
    /// 物料锁定（SAP MARA.MATFI）
    /// </summary>
    public string? MaterialLocked { get; set; } = string.Empty;

    /// <summary>
    /// 配置管理相关（SAP MARA.CMREL）
    /// </summary>
    public string? ConfigurationManagementRelevant { get; set; } = string.Empty;

    /// <summary>
    /// 品种清单类型（SAP MARA.BBTYP）
    /// </summary>
    public string? AssortmentListType { get; set; } = string.Empty;

    /// <summary>
    /// 到期日期类型（SAP MARA.SLED_BBD）
    /// </summary>
    public string? ExpirationDateType { get; set; } = string.Empty;

    /// <summary>
    /// GTIN变式（SAP MARA.GTIN_VARIANT）
    /// </summary>
    public string? GtinVariant { get; set; } = string.Empty;

    /// <summary>
    /// 通用物料号（SAP MARA.GENNR）
    /// </summary>
    public string? GenericMaterialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 相同包装参考物料（SAP MARA.RMATP）
    /// </summary>
    public string? SamePackingReferenceMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 全球数据同步相关（SAP MARA.GDS_RELEVANT）
    /// </summary>
    public string? GlobalDataSyncRelevant { get; set; } = string.Empty;

    /// <summary>
    /// 原产地验收（SAP MARA.WEORA）
    /// </summary>
    public string? AcceptanceAtOrigin { get; set; } = string.Empty;

    /// <summary>
    /// 标准HU类型（SAP MARA.HUTYP_DFLT）
    /// </summary>
    public string? StandardHuType { get; set; } = string.Empty;

    /// <summary>
    /// 易被盗（SAP MARA.PILFERABLE）
    /// </summary>
    public string? Pilferable { get; set; } = string.Empty;

    /// <summary>
    /// 仓储存储条件（SAP MARA.WHSTC）
    /// </summary>
    public string? WarehouseStorageCondition { get; set; } = string.Empty;

    /// <summary>
    /// 仓储物料组（SAP MARA.WHMATGR）
    /// </summary>
    public string? WarehouseMaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 处理标识（SAP MARA.HNDLCODE）
    /// </summary>
    public string? HandlingIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 危险物质相关（SAP MARA.HAZMAT）
    /// </summary>
    public string? HazardousSubstancesRelevant { get; set; } = string.Empty;

    /// <summary>
    /// 处理单元类型（SAP MARA.HUTYP）
    /// </summary>
    public string? HandlingUnitType { get; set; } = string.Empty;

    /// <summary>
    /// 可变皮重（SAP MARA.TARE_VAR）
    /// </summary>
    public string? VariableTareWeight { get; set; } = string.Empty;

    /// <summary>
    /// 最大允许容量（SAP MARA.MAXC）
    /// </summary>
    public decimal? MaximumAllowedCapacity { get; set; }

    /// <summary>
    /// 超容量容差（SAP MARA.MAXC_TOL）
    /// </summary>
    public decimal? OvercapacityTolerance { get; set; }

    /// <summary>
    /// 最大包装长度（SAP MARA.MAXL）
    /// </summary>
    public decimal? MaximumPackingLength { get; set; }

    /// <summary>
    /// 最大包装宽度（SAP MARA.MAXB）
    /// </summary>
    public decimal? MaximumPackingWidth { get; set; }

    /// <summary>
    /// 最大包装高度（SAP MARA.MAXH）
    /// </summary>
    public decimal? MaximumPackingHeight { get; set; }

    /// <summary>
    /// 最大包装尺寸单位（SAP MARA.MAXDIM_UOM）
    /// </summary>
    public string? MaximumPackingDimensionUnit { get; set; } = string.Empty;

    /// <summary>
    /// 原产国（SAP MARA.HERKL）
    /// </summary>
    public string? CountryOfOrigin { get; set; } = string.Empty;

    /// <summary>
    /// 物料运费组（SAP MARA.MFRGR）
    /// </summary>
    public string? MaterialFreightGroup { get; set; } = string.Empty;

    /// <summary>
    /// 隔离期（SAP MARA.QQTIME）
    /// </summary>
    public decimal? QuarantinePeriod { get; set; }

    /// <summary>
    /// 隔离期单位（SAP MARA.QQTIMEUOM）
    /// </summary>
    public string? QuarantinePeriodUnit { get; set; } = string.Empty;

    /// <summary>
    /// 质检组（SAP MARA.QGRP）
    /// </summary>
    public string? QualityInspectionGroup { get; set; } = string.Empty;

    /// <summary>
    /// 序列号参数文件（SAP MARA.SERIAL）
    /// </summary>
    public string? SerialNumberProfile { get; set; } = string.Empty;

    /// <summary>
    /// 表单名称（SAP MARA.PS_SMARTFORM）
    /// </summary>
    public string? FormName { get; set; } = string.Empty;

    /// <summary>
    /// 后勤计量单位（SAP MARA.LOGUNIT）
    /// </summary>
    public string? LogisticsUnitOfMeasure { get; set; } = string.Empty;

    /// <summary>
    /// 捕捞重量物料（SAP MARA.CWQREL）
    /// </summary>
    public string? CatchWeightMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 捕捞重量参数文件（SAP MARA.CWQPROC）
    /// </summary>
    public string? CatchWeightProfile { get; set; } = string.Empty;

    /// <summary>
    /// 捕捞重量容差组（SAP MARA.CWQTOLGR）
    /// </summary>
    public string? CatchWeightToleranceGroup { get; set; } = string.Empty;

    /// <summary>
    /// 调整参数文件（SAP MARA.ADPROF）
    /// </summary>
    public string? AdjustmentProfile { get; set; } = string.Empty;

    /// <summary>
    /// 知识产权ID（SAP MARA.IPMIPPRODUCT）
    /// </summary>
    public string? IntellectualPropertyId { get; set; } = string.Empty;

    /// <summary>
    /// 允许变式价格（SAP MARA.ALLOW_PMAT_IGNO）
    /// </summary>
    public string? VariantPriceAllowed { get; set; } = string.Empty;

    /// <summary>
    /// 介质（SAP MARA.MEDIUM）
    /// </summary>
    public string? Medium { get; set; } = string.Empty;

    /// <summary>
    /// 实物商品（SAP MARA.COMMODITY）
    /// </summary>
    public string? PhysicalCommodity { get; set; } = string.Empty;

    /// <summary>
    /// 动物源（SAP MARA.ANIMAL_ORIGIN）
    /// </summary>
    public string? AnimalOrigin { get; set; } = string.Empty;

    /// <summary>
    /// 纺织成分功能（SAP MARA.TEXTILE_COMP_IND）
    /// </summary>
    public string? TextileCompositionFunction { get; set; } = string.Empty;

    /// <summary>
    /// 细分结构（SAP MARA.SGT_CSGR）
    /// </summary>
    public string? SegmentationStructure { get; set; } = string.Empty;

    /// <summary>
    /// 细分策略（SAP MARA.SGT_COVSA）
    /// </summary>
    public string? SegmentationStrategy { get; set; } = string.Empty;

    /// <summary>
    /// 细分状态（SAP MARA.SGT_STAT）
    /// </summary>
    public string? SegmentationStatus { get; set; } = string.Empty;

    /// <summary>
    /// 细分范围（SAP MARA.SGT_SCOPE）
    /// </summary>
    public string? SegmentationScope { get; set; } = string.Empty;

    /// <summary>
    /// 细分相关（SAP MARA.SGT_REL）
    /// </summary>
    public string? SegmentationRelevant { get; set; } = string.Empty;

    /// <summary>
    /// 时装属性1（SAP MARA.FSH_MG_AT1）
    /// </summary>
    public string? FashionAttribute1 { get; set; } = string.Empty;

    /// <summary>
    /// 时装属性2（SAP MARA.FSH_MG_AT2）
    /// </summary>
    public string? FashionAttribute2 { get; set; } = string.Empty;

    /// <summary>
    /// 时装属性3（SAP MARA.FSH_MG_AT3）
    /// </summary>
    public string? FashionAttribute3 { get; set; } = string.Empty;

    /// <summary>
    /// 季节使用标识（SAP MARA.FSH_SEALV）
    /// </summary>
    public string? SeasonUsageIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 库存季节激活（SAP MARA.FSH_SEAIM）
    /// </summary>
    public string? SeasonActiveInInventory { get; set; } = string.Empty;

    /// <summary>
    /// 特性转换ID（SAP MARA.FSH_SC_MID）
    /// </summary>
    public string? CharacteristicConversionId { get; set; } = string.Empty;

    /// <summary>
    /// ANP代码（SAP MARA.ANP）
    /// </summary>
    public string? AnpCode { get; set; } = string.Empty;

    /// <summary>
    /// 危险品包装状态（SAP MARA.DG_PACK_STATUS）
    /// </summary>
    public string? DangerousGoodsPackagingStatus { get; set; } = string.Empty;

    /// <summary>
    /// 物料条件管理（SAP MARA.MCOND）
    /// </summary>
    public string? MaterialConditionManagement { get; set; } = string.Empty;

    /// <summary>
    /// 退货代码（SAP MARA.RETDELC）
    /// </summary>
    public string? ReturnCode { get; set; } = string.Empty;

    /// <summary>
    /// 退回后勤级别（SAP MARA.LOGLEV_RETO）
    /// </summary>
    public string? ReturnToLogisticsLevel { get; set; } = string.Empty;

    /// <summary>
    /// NATO物料识别号（SAP MARA.NSNID）
    /// </summary>
    public string? NatoItemIdentificationNumber { get; set; } = string.Empty;

    /// <summary>
    /// FFF类别（SAP MARA.IMATN）
    /// </summary>
    public string? FffClass { get; set; } = string.Empty;

    /// <summary>
    /// 替代链编码（SAP MARA.PICNUM）
    /// </summary>
    public string? SupersessionChainNumber { get; set; } = string.Empty;

    /// <summary>
    /// 季节采购创建状态（SAP MARA.BSTAT）
    /// </summary>
    public string? SeasonalProcurementCreationStatus { get; set; } = string.Empty;

    /// <summary>
    /// 颜色特性内部号（SAP MARA.COLOR_ATINN）
    /// </summary>
    public string? ColorCharacteristicInternalNumber { get; set; } = string.Empty;

    /// <summary>
    /// 主尺码特性内部号（SAP MARA.SIZE1_ATINN）
    /// </summary>
    public string? MainSizeCharacteristicInternalNumber { get; set; } = string.Empty;

    /// <summary>
    /// 次尺码特性内部号（SAP MARA.SIZE2_ATINN）
    /// </summary>
    public string? SecondSizeCharacteristicInternalNumber { get; set; } = string.Empty;

    /// <summary>
    /// 颜色（SAP MARA.COLOR）
    /// </summary>
    public string? Color { get; set; } = string.Empty;

    /// <summary>
    /// 主尺码（SAP MARA.SIZE1）
    /// </summary>
    public string? MainSize { get; set; } = string.Empty;

    /// <summary>
    /// 次尺码（SAP MARA.SIZE2）
    /// </summary>
    public string? SecondSize { get; set; } = string.Empty;

    /// <summary>
    /// 评估特性值（SAP MARA.FREE_CHAR）
    /// </summary>
    public string? EvaluationCharacteristicValue { get; set; } = string.Empty;

    /// <summary>
    /// 护理代码（SAP MARA.CARE_CODE）
    /// </summary>
    public string? CareCode { get; set; } = string.Empty;

    /// <summary>
    /// 品牌（SAP MARA.BRAND_ID）
    /// </summary>
    public string? BrandId { get; set; } = string.Empty;

    /// <summary>
    /// 纤维代码1（SAP MARA.FIBER_CODE1）
    /// </summary>
    public string? FiberCode1 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维占比1（SAP MARA.FIBER_PART1）
    /// </summary>
    public string? FiberPart1 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维代码2（SAP MARA.FIBER_CODE2）
    /// </summary>
    public string? FiberCode2 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维占比2（SAP MARA.FIBER_PART2）
    /// </summary>
    public string? FiberPart2 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维代码3（SAP MARA.FIBER_CODE3）
    /// </summary>
    public string? FiberCode3 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维占比3（SAP MARA.FIBER_PART3）
    /// </summary>
    public string? FiberPart3 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维代码4（SAP MARA.FIBER_CODE4）
    /// </summary>
    public string? FiberCode4 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维占比4（SAP MARA.FIBER_PART4）
    /// </summary>
    public string? FiberPart4 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维代码5（SAP MARA.FIBER_CODE5）
    /// </summary>
    public string? FiberCode5 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维占比5（SAP MARA.FIBER_PART5）
    /// </summary>
    public string? FiberPart5 { get; set; } = string.Empty;

    /// <summary>
    /// 时装等级（SAP MARA.FASHGRD）
    /// </summary>
    public string? FashionGrade { get; set; } = string.Empty;

    /// <summary>
    /// 物料状态（字典 sys_normal_disable_status；0=禁用，1=启用，2=锁定；平台启用态，非 SAP MSTAE）
    /// </summary>
    public int? MaterialStatus { get; set; }

    /// <summary>
    /// 多语言描述列表（主子表关系；对齐 SAP MAKT）（子表，级联保存）
    /// </summary>
    public List<TaktMaterialDescriptionCreateDto>? Descriptions { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 导出 DTO
// ========================================

/// <summary>
/// Material 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktMaterialExportDto
{
    /// <summary>
    /// MaterialID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialId { get; set; }

    /// <summary>
    /// 物料编码（SAP MARA.MATNR）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 完整维护状态（SAP MARA.VPSTA）
    /// </summary>
    public string? CompleteMaintenanceStatus { get; set; } = string.Empty;

    /// <summary>
    /// 维护状态（SAP MARA.PSTAT）
    /// </summary>
    public string? MaintenanceStatus { get; set; } = string.Empty;

    /// <summary>
    /// 客户级删除标记（SAP MARA.LVORM）
    /// </summary>
    public string? ClientDeletionFlag { get; set; } = string.Empty;

    /// <summary>
    /// 物料类型（SAP MARA.MTART）
    /// </summary>
    public string MaterialType { get; set; } = string.Empty;

    /// <summary>
    /// 行业领域（SAP MARA.MBRSH）
    /// </summary>
    public string IndustrySector { get; set; } = string.Empty;

    /// <summary>
    /// 物料组（SAP MARA.MATKL）
    /// </summary>
    public string MaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料号（SAP MARA.BISMT）
    /// </summary>
    public string? OldMaterialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 基本计量单位（SAP MARA.MEINS）
    /// </summary>
    public string BaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单单位（SAP MARA.BSTME）
    /// </summary>
    public string? OrderUnit { get; set; } = string.Empty;

    /// <summary>
    /// 单据号（SAP MARA.ZEINR）
    /// </summary>
    public string? DocumentNumber { get; set; } = string.Empty;

    /// <summary>
    /// 单据类型（SAP MARA.ZEIAR）
    /// </summary>
    public string? DocumentType { get; set; } = string.Empty;

    /// <summary>
    /// 单据版本（SAP MARA.ZEIVR）
    /// </summary>
    public string? DocumentVersion { get; set; } = string.Empty;

    /// <summary>
    /// 单据页格式（SAP MARA.ZEIFO）
    /// </summary>
    public string? DocumentPageFormat { get; set; } = string.Empty;

    /// <summary>
    /// 单据更改号（SAP MARA.AESZN）
    /// </summary>
    public string? DocumentChangeNumber { get; set; } = string.Empty;

    /// <summary>
    /// 单据页号（SAP MARA.BLATT）
    /// </summary>
    public string? DocumentPageNumber { get; set; } = string.Empty;

    /// <summary>
    /// 单据页数（SAP MARA.BLANZ）
    /// </summary>
    public string? DocumentSheetCount { get; set; } = string.Empty;

    /// <summary>
    /// 生产/检验备忘（SAP MARA.FERTH）
    /// </summary>
    public string? ProductionInspectionMemo { get; set; } = string.Empty;

    /// <summary>
    /// 生产备忘页格式（SAP MARA.FORMT）
    /// </summary>
    public string? ProductionMemoPageFormat { get; set; } = string.Empty;

    /// <summary>
    /// 尺寸/规格（SAP MARA.GROES）
    /// </summary>
    public string? SizeDimensions { get; set; } = string.Empty;

    /// <summary>
    /// 基本物料（材质）（SAP MARA.WRKST）
    /// </summary>
    public string? BasicMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 行业标准描述（SAP MARA.NORMT）
    /// </summary>
    public string? IndustryStandardDescription { get; set; } = string.Empty;

    /// <summary>
    /// 实验室/设计室（SAP MARA.LABOR）
    /// </summary>
    public string? LaboratoryDesignOffice { get; set; } = string.Empty;

    /// <summary>
    /// 采购价值码（SAP MARA.EKWSL）
    /// </summary>
    public string? PurchasingValueKey { get; set; } = string.Empty;

    /// <summary>
    /// 毛重（SAP MARA.BRGEW）
    /// </summary>
    public decimal? GrossWeight { get; set; }

    /// <summary>
    /// 净重（SAP MARA.NTGEW）
    /// </summary>
    public decimal? NetWeight { get; set; }

    /// <summary>
    /// 重量单位（SAP MARA.GEWEI）
    /// </summary>
    public string? WeightUnit { get; set; } = string.Empty;

    /// <summary>
    /// 体积（SAP MARA.VOLUM）
    /// </summary>
    public decimal? Volume { get; set; }

    /// <summary>
    /// 体积单位（SAP MARA.VOLEH）
    /// </summary>
    public string? VolumeUnit { get; set; } = string.Empty;

    /// <summary>
    /// 容器要求（SAP MARA.BEHVO）
    /// </summary>
    public string? ContainerRequirements { get; set; } = string.Empty;

    /// <summary>
    /// 仓储条件（SAP MARA.RAUBE）
    /// </summary>
    public string? StorageConditions { get; set; } = string.Empty;

    /// <summary>
    /// 温度条件（SAP MARA.TEMPB）
    /// </summary>
    public string? TemperatureConditions { get; set; } = string.Empty;

    /// <summary>
    /// 低层码（SAP MARA.DISST）
    /// </summary>
    public string? LowLevelCode { get; set; } = string.Empty;

    /// <summary>
    /// 运输组（SAP MARA.TRAGR）
    /// </summary>
    public string? TransportationGroup { get; set; } = string.Empty;

    /// <summary>
    /// 危险品编码（SAP MARA.STOFF）
    /// </summary>
    public string? HazardousMaterialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 产品组（SAP MARA.SPART）
    /// </summary>
    public string? Division { get; set; } = string.Empty;

    /// <summary>
    /// 竞争对手（SAP MARA.KUNNR）
    /// </summary>
    public string? Competitor { get; set; } = string.Empty;

    /// <summary>
    /// 欧洲商品号（旧）（SAP MARA.EANNR）
    /// </summary>
    public string? EuropeanArticleNumberObsolete { get; set; } = string.Empty;

    /// <summary>
    /// 收发货凭证打印数量（SAP MARA.WESCH）
    /// </summary>
    public decimal? GrGiSlipQuantity { get; set; }

    /// <summary>
    /// 采购规则（SAP MARA.BWVOR）
    /// </summary>
    public string? ProcurementRule { get; set; } = string.Empty;

    /// <summary>
    /// 货源（SAP MARA.BWSCL）
    /// </summary>
    public string? SourceOfSupply { get; set; } = string.Empty;

    /// <summary>
    /// 季节类别（SAP MARA.SAISO）
    /// </summary>
    public string? SeasonCategory { get; set; } = string.Empty;

    /// <summary>
    /// 标签类型（SAP MARA.ETIAR）
    /// </summary>
    public string? LabelType { get; set; } = string.Empty;

    /// <summary>
    /// 标签格式（SAP MARA.ETIFO）
    /// </summary>
    public string? LabelForm { get; set; } = string.Empty;

    /// <summary>
    /// 已停用字段（SAP MARA.ENTAR）
    /// </summary>
    public string? DeactivatedField { get; set; } = string.Empty;

    /// <summary>
    /// 国际商品编码EAN/UPC（SAP MARA.EAN11）
    /// </summary>
    public string? InternationalArticleNumber { get; set; } = string.Empty;

    /// <summary>
    /// EAN类别（SAP MARA.NUMTP）
    /// </summary>
    public string? EanCategory { get; set; } = string.Empty;

    /// <summary>
    /// 长度（SAP MARA.LAENG）
    /// </summary>
    public decimal? Length { get; set; }

    /// <summary>
    /// 宽度（SAP MARA.BREIT）
    /// </summary>
    public decimal? Width { get; set; }

    /// <summary>
    /// 高度（SAP MARA.HOEHE）
    /// </summary>
    public decimal? Height { get; set; }

    /// <summary>
    /// 长宽高单位（SAP MARA.MEABM）
    /// </summary>
    public string? DimensionUnit { get; set; } = string.Empty;

    /// <summary>
    /// 产品层次（SAP MARA.PRDHA）
    /// </summary>
    public string? ProductHierarchy { get; set; } = string.Empty;

    /// <summary>
    /// 库存调拨净更改成本核算（SAP MARA.AEKLK）
    /// </summary>
    public string? StockTransferNetChangeCosting { get; set; } = string.Empty;

    /// <summary>
    /// CAD标识（SAP MARA.CADKZ）
    /// </summary>
    public string? CadIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 采购QM激活（SAP MARA.QMPUR）
    /// </summary>
    public string? QmInProcurement { get; set; } = string.Empty;

    /// <summary>
    /// 允许包装重量（SAP MARA.ERGEW）
    /// </summary>
    public decimal? AllowedPackagingWeight { get; set; }

    /// <summary>
    /// 允许包装重量单位（SAP MARA.ERGEI）
    /// </summary>
    public string? AllowedPackagingWeightUnit { get; set; } = string.Empty;

    /// <summary>
    /// 允许包装体积（SAP MARA.ERVOL）
    /// </summary>
    public decimal? AllowedPackagingVolume { get; set; }

    /// <summary>
    /// 允许包装体积单位（SAP MARA.ERVOE）
    /// </summary>
    public string? AllowedPackagingVolumeUnit { get; set; } = string.Empty;

    /// <summary>
    /// 超重容差（SAP MARA.GEWTO）
    /// </summary>
    public decimal? ExcessWeightTolerance { get; set; }

    /// <summary>
    /// 超体积容差（SAP MARA.VOLTO）
    /// </summary>
    public decimal? ExcessVolumeTolerance { get; set; }

    /// <summary>
    /// 可变采购订单单位（SAP MARA.VABME）
    /// </summary>
    public string? VariablePurchaseOrderUnit { get; set; } = string.Empty;

    /// <summary>
    /// 已分配修订级别（SAP MARA.KZREV）
    /// </summary>
    public string? RevisionLevelAssigned { get; set; } = string.Empty;

    /// <summary>
    /// 可配置物料（SAP MARA.KZKFG）
    /// </summary>
    public string? ConfigurableMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 批次管理要求（SAP MARA.XCHPF）
    /// </summary>
    public string? BatchManagementRequired { get; set; } = string.Empty;

    /// <summary>
    /// 包装物料类型（SAP MARA.VHART）
    /// </summary>
    public string? PackagingMaterialType { get; set; } = string.Empty;

    /// <summary>
    /// 最大装载量（体积）（SAP MARA.FUELG）
    /// </summary>
    public decimal? MaximumLevelByVolume { get; set; }

    /// <summary>
    /// 堆叠因子（SAP MARA.STFAK）
    /// </summary>
    public int? StackingFactor { get; set; }

    /// <summary>
    /// 包装物料组（SAP MARA.MAGRV）
    /// </summary>
    public string? PackagingMaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 权限组（SAP MARA.BEGRU）
    /// </summary>
    public string? AuthorizationGroup { get; set; } = string.Empty;

    /// <summary>
    /// 有效起始日期（SAP MARA.DATAB）
    /// </summary>
    public DateTime? ValidFromDate { get; set; }

    /// <summary>
    /// 季节年份（SAP MARA.SAISJ）
    /// </summary>
    public string? SeasonYear { get; set; } = string.Empty;

    /// <summary>
    /// 价格带类别（SAP MARA.PLGTP）
    /// </summary>
    public string? PriceBandCategory { get; set; } = string.Empty;

    /// <summary>
    /// 空容器BOM（SAP MARA.MLGUT）
    /// </summary>
    public string? EmptiesBillOfMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 外部物料组（SAP MARA.EXTWG）
    /// </summary>
    public string? ExternalMaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 跨工厂可配置物料（SAP MARA.SATNR）
    /// </summary>
    public string? CrossPlantConfigurableMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 物料类别（SAP MARA.ATTYP）
    /// </summary>
    public string? MaterialCategory { get; set; } = string.Empty;

    /// <summary>
    /// 联产品标识（SAP MARA.KZKUP）
    /// </summary>
    public string? CoProductIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 后续物料标识（SAP MARA.KZNFM）
    /// </summary>
    public string? FollowUpMaterialIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 定价参考物料（SAP MARA.PMATA）
    /// </summary>
    public string? PricingReferenceMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 跨工厂物料状态（SAP MARA.MSTAE）
    /// </summary>
    public string? CrossPlantMaterialStatus { get; set; } = string.Empty;

    /// <summary>
    /// 跨分销链物料状态（SAP MARA.MSTAV）
    /// </summary>
    public string? CrossDistributionChainStatus { get; set; } = string.Empty;

    /// <summary>
    /// 跨工厂状态生效日期（SAP MARA.MSTDE）
    /// </summary>
    public DateTime? CrossPlantStatusValidFrom { get; set; }

    /// <summary>
    /// 跨分销链状态生效日期（SAP MARA.MSTDV）
    /// </summary>
    public DateTime? CrossDistributionStatusValidFrom { get; set; }

    /// <summary>
    /// 物料税分类（SAP MARA.TAKLV）
    /// </summary>
    public string? TaxClassification { get; set; } = string.Empty;

    /// <summary>
    /// 目录参数文件（SAP MARA.RBNRM）
    /// </summary>
    public string? CatalogProfile { get; set; } = string.Empty;

    /// <summary>
    /// 最短剩余货架寿命（SAP MARA.MHDRZ）
    /// </summary>
    public decimal? MinimumRemainingShelfLife { get; set; }

    /// <summary>
    /// 总货架寿命（SAP MARA.MHDHB）
    /// </summary>
    public decimal? TotalShelfLife { get; set; }

    /// <summary>
    /// 仓储百分比（SAP MARA.MHDLP）
    /// </summary>
    public decimal? StoragePercentage { get; set; }

    /// <summary>
    /// 含量单位（SAP MARA.INHME）
    /// </summary>
    public string? ContentUnit { get; set; } = string.Empty;

    /// <summary>
    /// 净含量（SAP MARA.INHAL）
    /// </summary>
    public decimal? NetContents { get; set; }

    /// <summary>
    /// 比较价格单位（SAP MARA.VPREH）
    /// </summary>
    public decimal? ComparisonPriceUnit { get; set; }

    /// <summary>
    /// 标签物料分组（SAP MARA.ETIAG）
    /// </summary>
    public string? LabelingMaterialGrouping { get; set; } = string.Empty;

    /// <summary>
    /// 毛含量（SAP MARA.INHBR）
    /// </summary>
    public decimal? GrossContents { get; set; }

    /// <summary>
    /// 数量换算方法（SAP MARA.CMETH）
    /// </summary>
    public string? QuantityConversionMethod { get; set; } = string.Empty;

    /// <summary>
    /// 内部对象号（SAP MARA.CUOBF）
    /// </summary>
    public string? InternalObjectNumber { get; set; } = string.Empty;

    /// <summary>
    /// 环境相关（SAP MARA.KZUMW）
    /// </summary>
    public string? EnvironmentallyRelevant { get; set; } = string.Empty;

    /// <summary>
    /// 产品分配确定过程（SAP MARA.KOSCH）
    /// </summary>
    public string? ProductAllocationProcedure { get; set; } = string.Empty;

    /// <summary>
    /// 变式定价参数文件（SAP MARA.SPROF）
    /// </summary>
    public string? VariantPricingProfile { get; set; } = string.Empty;

    /// <summary>
    /// 实物折扣资格（SAP MARA.NRFHG）
    /// </summary>
    public string? DiscountInKind { get; set; } = string.Empty;

    /// <summary>
    /// 制造商零件号（SAP MARA.MFRPN）
    /// </summary>
    public string? ManufacturerPartNumber { get; set; } = string.Empty;

    /// <summary>
    /// 制造商编码（SAP MARA.MFRNR）
    /// </summary>
    public string? ManufacturerNumber { get; set; } = string.Empty;

    /// <summary>
    /// 自有库存管理物料号（SAP MARA.BMATN）
    /// </summary>
    public string? InventoryManagedMaterialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 制造商零件参数文件（SAP MARA.MPROF）
    /// </summary>
    public string? ManufacturerPartProfile { get; set; } = string.Empty;

    /// <summary>
    /// 计量单位用途（SAP MARA.KZWSM）
    /// </summary>
    public string? UnitsOfMeasureUsage { get; set; } = string.Empty;

    /// <summary>
    /// 季节推出（SAP MARA.SAITY）
    /// </summary>
    public string? SeasonRollout { get; set; } = string.Empty;

    /// <summary>
    /// 危险品参数文件（SAP MARA.PROFL）
    /// </summary>
    public string? DangerousGoodsProfile { get; set; } = string.Empty;

    /// <summary>
    /// 高粘度（SAP MARA.IHIVI）
    /// </summary>
    public string? HighlyViscous { get; set; } = string.Empty;

    /// <summary>
    /// 散装/液体（SAP MARA.ILOOS）
    /// </summary>
    public string? InBulkLiquid { get; set; } = string.Empty;

    /// <summary>
    /// 序列号明确级别（SAP MARA.SERLV）
    /// </summary>
    public string? SerialNumberExplicitness { get; set; } = string.Empty;

    /// <summary>
    /// 封闭包装（SAP MARA.KZGVH）
    /// </summary>
    public string? ClosedPackaging { get; set; } = string.Empty;

    /// <summary>
    /// 需批准批次记录（SAP MARA.XGCHP）
    /// </summary>
    public string? ApprovedBatchRecordRequired { get; set; } = string.Empty;

    /// <summary>
    /// 有效性参数覆盖（SAP MARA.KZEFF）
    /// </summary>
    public string? EffectivityParameterOverride { get; set; } = string.Empty;

    /// <summary>
    /// 物料完成级别（SAP MARA.COMPL）
    /// </summary>
    public string? MaterialCompletionLevel { get; set; } = string.Empty;

    /// <summary>
    /// 货架寿命期间标识（SAP MARA.IPRKZ）
    /// </summary>
    public string? ShelfLifePeriodIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 货架寿命舍入规则（SAP MARA.RDMHD）
    /// </summary>
    public string? ShelfLifeRoundingRule { get; set; } = string.Empty;

    /// <summary>
    /// 包装打印产品成分（SAP MARA.PRZUS）
    /// </summary>
    public string? ProductCompositionOnPackaging { get; set; } = string.Empty;

    /// <summary>
    /// 通用项目类别组（SAP MARA.MTPOS_MARA）
    /// </summary>
    public string? GeneralItemCategoryGroup { get; set; } = string.Empty;

    /// <summary>
    /// 后勤变式通用物料（SAP MARA.BFLME）
    /// </summary>
    public string? LogisticalVariants { get; set; } = string.Empty;

    /// <summary>
    /// 物料锁定（SAP MARA.MATFI）
    /// </summary>
    public string? MaterialLocked { get; set; } = string.Empty;

    /// <summary>
    /// 配置管理相关（SAP MARA.CMREL）
    /// </summary>
    public string? ConfigurationManagementRelevant { get; set; } = string.Empty;

    /// <summary>
    /// 品种清单类型（SAP MARA.BBTYP）
    /// </summary>
    public string? AssortmentListType { get; set; } = string.Empty;

    /// <summary>
    /// 到期日期类型（SAP MARA.SLED_BBD）
    /// </summary>
    public string? ExpirationDateType { get; set; } = string.Empty;

    /// <summary>
    /// GTIN变式（SAP MARA.GTIN_VARIANT）
    /// </summary>
    public string? GtinVariant { get; set; } = string.Empty;

    /// <summary>
    /// 通用物料号（SAP MARA.GENNR）
    /// </summary>
    public string? GenericMaterialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 相同包装参考物料（SAP MARA.RMATP）
    /// </summary>
    public string? SamePackingReferenceMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 全球数据同步相关（SAP MARA.GDS_RELEVANT）
    /// </summary>
    public string? GlobalDataSyncRelevant { get; set; } = string.Empty;

    /// <summary>
    /// 原产地验收（SAP MARA.WEORA）
    /// </summary>
    public string? AcceptanceAtOrigin { get; set; } = string.Empty;

    /// <summary>
    /// 标准HU类型（SAP MARA.HUTYP_DFLT）
    /// </summary>
    public string? StandardHuType { get; set; } = string.Empty;

    /// <summary>
    /// 易被盗（SAP MARA.PILFERABLE）
    /// </summary>
    public string? Pilferable { get; set; } = string.Empty;

    /// <summary>
    /// 仓储存储条件（SAP MARA.WHSTC）
    /// </summary>
    public string? WarehouseStorageCondition { get; set; } = string.Empty;

    /// <summary>
    /// 仓储物料组（SAP MARA.WHMATGR）
    /// </summary>
    public string? WarehouseMaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 处理标识（SAP MARA.HNDLCODE）
    /// </summary>
    public string? HandlingIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 危险物质相关（SAP MARA.HAZMAT）
    /// </summary>
    public string? HazardousSubstancesRelevant { get; set; } = string.Empty;

    /// <summary>
    /// 处理单元类型（SAP MARA.HUTYP）
    /// </summary>
    public string? HandlingUnitType { get; set; } = string.Empty;

    /// <summary>
    /// 可变皮重（SAP MARA.TARE_VAR）
    /// </summary>
    public string? VariableTareWeight { get; set; } = string.Empty;

    /// <summary>
    /// 最大允许容量（SAP MARA.MAXC）
    /// </summary>
    public decimal? MaximumAllowedCapacity { get; set; }

    /// <summary>
    /// 超容量容差（SAP MARA.MAXC_TOL）
    /// </summary>
    public decimal? OvercapacityTolerance { get; set; }

    /// <summary>
    /// 最大包装长度（SAP MARA.MAXL）
    /// </summary>
    public decimal? MaximumPackingLength { get; set; }

    /// <summary>
    /// 最大包装宽度（SAP MARA.MAXB）
    /// </summary>
    public decimal? MaximumPackingWidth { get; set; }

    /// <summary>
    /// 最大包装高度（SAP MARA.MAXH）
    /// </summary>
    public decimal? MaximumPackingHeight { get; set; }

    /// <summary>
    /// 最大包装尺寸单位（SAP MARA.MAXDIM_UOM）
    /// </summary>
    public string? MaximumPackingDimensionUnit { get; set; } = string.Empty;

    /// <summary>
    /// 原产国（SAP MARA.HERKL）
    /// </summary>
    public string? CountryOfOrigin { get; set; } = string.Empty;

    /// <summary>
    /// 物料运费组（SAP MARA.MFRGR）
    /// </summary>
    public string? MaterialFreightGroup { get; set; } = string.Empty;

    /// <summary>
    /// 隔离期（SAP MARA.QQTIME）
    /// </summary>
    public decimal? QuarantinePeriod { get; set; }

    /// <summary>
    /// 隔离期单位（SAP MARA.QQTIMEUOM）
    /// </summary>
    public string? QuarantinePeriodUnit { get; set; } = string.Empty;

    /// <summary>
    /// 质检组（SAP MARA.QGRP）
    /// </summary>
    public string? QualityInspectionGroup { get; set; } = string.Empty;

    /// <summary>
    /// 序列号参数文件（SAP MARA.SERIAL）
    /// </summary>
    public string? SerialNumberProfile { get; set; } = string.Empty;

    /// <summary>
    /// 表单名称（SAP MARA.PS_SMARTFORM）
    /// </summary>
    public string? FormName { get; set; } = string.Empty;

    /// <summary>
    /// 后勤计量单位（SAP MARA.LOGUNIT）
    /// </summary>
    public string? LogisticsUnitOfMeasure { get; set; } = string.Empty;

    /// <summary>
    /// 捕捞重量物料（SAP MARA.CWQREL）
    /// </summary>
    public string? CatchWeightMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 捕捞重量参数文件（SAP MARA.CWQPROC）
    /// </summary>
    public string? CatchWeightProfile { get; set; } = string.Empty;

    /// <summary>
    /// 捕捞重量容差组（SAP MARA.CWQTOLGR）
    /// </summary>
    public string? CatchWeightToleranceGroup { get; set; } = string.Empty;

    /// <summary>
    /// 调整参数文件（SAP MARA.ADPROF）
    /// </summary>
    public string? AdjustmentProfile { get; set; } = string.Empty;

    /// <summary>
    /// 知识产权ID（SAP MARA.IPMIPPRODUCT）
    /// </summary>
    public string? IntellectualPropertyId { get; set; } = string.Empty;

    /// <summary>
    /// 允许变式价格（SAP MARA.ALLOW_PMAT_IGNO）
    /// </summary>
    public string? VariantPriceAllowed { get; set; } = string.Empty;

    /// <summary>
    /// 介质（SAP MARA.MEDIUM）
    /// </summary>
    public string? Medium { get; set; } = string.Empty;

    /// <summary>
    /// 实物商品（SAP MARA.COMMODITY）
    /// </summary>
    public string? PhysicalCommodity { get; set; } = string.Empty;

    /// <summary>
    /// 动物源（SAP MARA.ANIMAL_ORIGIN）
    /// </summary>
    public string? AnimalOrigin { get; set; } = string.Empty;

    /// <summary>
    /// 纺织成分功能（SAP MARA.TEXTILE_COMP_IND）
    /// </summary>
    public string? TextileCompositionFunction { get; set; } = string.Empty;

    /// <summary>
    /// 细分结构（SAP MARA.SGT_CSGR）
    /// </summary>
    public string? SegmentationStructure { get; set; } = string.Empty;

    /// <summary>
    /// 细分策略（SAP MARA.SGT_COVSA）
    /// </summary>
    public string? SegmentationStrategy { get; set; } = string.Empty;

    /// <summary>
    /// 细分状态（SAP MARA.SGT_STAT）
    /// </summary>
    public string? SegmentationStatus { get; set; } = string.Empty;

    /// <summary>
    /// 细分范围（SAP MARA.SGT_SCOPE）
    /// </summary>
    public string? SegmentationScope { get; set; } = string.Empty;

    /// <summary>
    /// 细分相关（SAP MARA.SGT_REL）
    /// </summary>
    public string? SegmentationRelevant { get; set; } = string.Empty;

    /// <summary>
    /// 时装属性1（SAP MARA.FSH_MG_AT1）
    /// </summary>
    public string? FashionAttribute1 { get; set; } = string.Empty;

    /// <summary>
    /// 时装属性2（SAP MARA.FSH_MG_AT2）
    /// </summary>
    public string? FashionAttribute2 { get; set; } = string.Empty;

    /// <summary>
    /// 时装属性3（SAP MARA.FSH_MG_AT3）
    /// </summary>
    public string? FashionAttribute3 { get; set; } = string.Empty;

    /// <summary>
    /// 季节使用标识（SAP MARA.FSH_SEALV）
    /// </summary>
    public string? SeasonUsageIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 库存季节激活（SAP MARA.FSH_SEAIM）
    /// </summary>
    public string? SeasonActiveInInventory { get; set; } = string.Empty;

    /// <summary>
    /// 特性转换ID（SAP MARA.FSH_SC_MID）
    /// </summary>
    public string? CharacteristicConversionId { get; set; } = string.Empty;

    /// <summary>
    /// ANP代码（SAP MARA.ANP）
    /// </summary>
    public string? AnpCode { get; set; } = string.Empty;

    /// <summary>
    /// 危险品包装状态（SAP MARA.DG_PACK_STATUS）
    /// </summary>
    public string? DangerousGoodsPackagingStatus { get; set; } = string.Empty;

    /// <summary>
    /// 物料条件管理（SAP MARA.MCOND）
    /// </summary>
    public string? MaterialConditionManagement { get; set; } = string.Empty;

    /// <summary>
    /// 退货代码（SAP MARA.RETDELC）
    /// </summary>
    public string? ReturnCode { get; set; } = string.Empty;

    /// <summary>
    /// 退回后勤级别（SAP MARA.LOGLEV_RETO）
    /// </summary>
    public string? ReturnToLogisticsLevel { get; set; } = string.Empty;

    /// <summary>
    /// NATO物料识别号（SAP MARA.NSNID）
    /// </summary>
    public string? NatoItemIdentificationNumber { get; set; } = string.Empty;

    /// <summary>
    /// FFF类别（SAP MARA.IMATN）
    /// </summary>
    public string? FffClass { get; set; } = string.Empty;

    /// <summary>
    /// 替代链编码（SAP MARA.PICNUM）
    /// </summary>
    public string? SupersessionChainNumber { get; set; } = string.Empty;

    /// <summary>
    /// 季节采购创建状态（SAP MARA.BSTAT）
    /// </summary>
    public string? SeasonalProcurementCreationStatus { get; set; } = string.Empty;

    /// <summary>
    /// 颜色特性内部号（SAP MARA.COLOR_ATINN）
    /// </summary>
    public string? ColorCharacteristicInternalNumber { get; set; } = string.Empty;

    /// <summary>
    /// 主尺码特性内部号（SAP MARA.SIZE1_ATINN）
    /// </summary>
    public string? MainSizeCharacteristicInternalNumber { get; set; } = string.Empty;

    /// <summary>
    /// 次尺码特性内部号（SAP MARA.SIZE2_ATINN）
    /// </summary>
    public string? SecondSizeCharacteristicInternalNumber { get; set; } = string.Empty;

    /// <summary>
    /// 颜色（SAP MARA.COLOR）
    /// </summary>
    public string? Color { get; set; } = string.Empty;

    /// <summary>
    /// 主尺码（SAP MARA.SIZE1）
    /// </summary>
    public string? MainSize { get; set; } = string.Empty;

    /// <summary>
    /// 次尺码（SAP MARA.SIZE2）
    /// </summary>
    public string? SecondSize { get; set; } = string.Empty;

    /// <summary>
    /// 评估特性值（SAP MARA.FREE_CHAR）
    /// </summary>
    public string? EvaluationCharacteristicValue { get; set; } = string.Empty;

    /// <summary>
    /// 护理代码（SAP MARA.CARE_CODE）
    /// </summary>
    public string? CareCode { get; set; } = string.Empty;

    /// <summary>
    /// 品牌（SAP MARA.BRAND_ID）
    /// </summary>
    public string? BrandId { get; set; } = string.Empty;

    /// <summary>
    /// 纤维代码1（SAP MARA.FIBER_CODE1）
    /// </summary>
    public string? FiberCode1 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维占比1（SAP MARA.FIBER_PART1）
    /// </summary>
    public string? FiberPart1 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维代码2（SAP MARA.FIBER_CODE2）
    /// </summary>
    public string? FiberCode2 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维占比2（SAP MARA.FIBER_PART2）
    /// </summary>
    public string? FiberPart2 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维代码3（SAP MARA.FIBER_CODE3）
    /// </summary>
    public string? FiberCode3 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维占比3（SAP MARA.FIBER_PART3）
    /// </summary>
    public string? FiberPart3 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维代码4（SAP MARA.FIBER_CODE4）
    /// </summary>
    public string? FiberCode4 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维占比4（SAP MARA.FIBER_PART4）
    /// </summary>
    public string? FiberPart4 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维代码5（SAP MARA.FIBER_CODE5）
    /// </summary>
    public string? FiberCode5 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维占比5（SAP MARA.FIBER_PART5）
    /// </summary>
    public string? FiberPart5 { get; set; } = string.Empty;

    /// <summary>
    /// 时装等级（SAP MARA.FASHGRD）
    /// </summary>
    public string? FashionGrade { get; set; } = string.Empty;

    /// <summary>
    /// 物料状态（字典 sys_normal_disable_status；0=禁用，1=启用，2=锁定；平台启用态，非 SAP MSTAE）
    /// </summary>
    public int MaterialStatus { get; set; } = 0;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
