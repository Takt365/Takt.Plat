// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Logistics.Materials
// 文件名称：TaktMaterial.cs
// 创建时间：2026-06-15
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt全局物料实体（对齐 SAP MARA 通用物料主数据；租户级；多语言描述见 TaktMaterialDescription）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Materials;

/// <summary>
/// Takt全局物料实体（租户内共享；字段对齐 SAP MARA；多语言描述见子表 TaktMaterialDescription / SAP MAKT）
/// </summary>
[SugarTable("takt_logistics_materials_material", "全局物料表")]
[SugarIndex("ix_takt_logistics_materials_material_tenant", nameof(TenantCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_materials_material_unique", nameof(TenantCode), OrderByType.Asc, nameof(MaterialCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_materials_material_status", nameof(TenantCode), OrderByType.Asc, nameof(MaterialStatus), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_materials_material_type", nameof(TenantCode), OrderByType.Asc, nameof(MaterialType), OrderByType.Asc, false)]
public class TaktMaterial : TaktTenantEntityBase
{
    /// <summary>
    /// 物料编码（SAP MARA.MATNR）
    /// </summary>
    [SugarColumn(ColumnName = "material_code", ColumnDescription = "物料编码", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 完整维护状态（SAP MARA.VPSTA）
    /// </summary>
    [SugarColumn(ColumnName = "complete_maintenance_status", ColumnDescription = "完整维护状态", ColumnDataType = "nvarchar", Length = 15, IsNullable = true)]
    public string? CompleteMaintenanceStatus { get; set; }

    /// <summary>
    /// 维护状态（SAP MARA.PSTAT）
    /// </summary>
    [SugarColumn(ColumnName = "maintenance_status", ColumnDescription = "维护状态", ColumnDataType = "nvarchar", Length = 15, IsNullable = true)]
    public string? MaintenanceStatus { get; set; }

    /// <summary>
    /// 客户级删除标记（SAP MARA.LVORM）
    /// </summary>
    [SugarColumn(ColumnName = "client_deletion_flag", ColumnDescription = "客户级删除标记", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? ClientDeletionFlag { get; set; }

    /// <summary>
    /// 物料类型（SAP MARA.MTART）
    /// </summary>
    [SugarColumn(ColumnName = "material_type", ColumnDescription = "物料类型", ColumnDataType = "nvarchar", Length = 4, IsNullable = false, DefaultValue = "ROH")]
    public string MaterialType { get; set; } = "ROH";

    /// <summary>
    /// 行业领域（SAP MARA.MBRSH）
    /// </summary>
    [SugarColumn(ColumnName = "industry_sector", ColumnDescription = "行业领域", ColumnDataType = "nvarchar", Length = 1, IsNullable = false)]
    public string IndustrySector { get; set; } = string.Empty;

    /// <summary>
    /// 物料组（SAP MARA.MATKL）
    /// </summary>
    [SugarColumn(ColumnName = "material_group", ColumnDescription = "物料组", ColumnDataType = "nvarchar", Length = 9, IsNullable = false)]
    public string MaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料号（SAP MARA.BISMT）
    /// </summary>
    [SugarColumn(ColumnName = "old_material_number", ColumnDescription = "旧物料号", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? OldMaterialNumber { get; set; }

    /// <summary>
    /// 基本计量单位（SAP MARA.MEINS）
    /// </summary>
    [SugarColumn(ColumnName = "base_unit", ColumnDescription = "基本计量单位", ColumnDataType = "nvarchar", Length = 3, IsNullable = false, DefaultValue = "PC")]
    public string BaseUnit { get; set; } = "PC";

    /// <summary>
    /// 采购订单单位（SAP MARA.BSTME）
    /// </summary>
    [SugarColumn(ColumnName = "order_unit", ColumnDescription = "采购订单单位", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? OrderUnit { get; set; }

    /// <summary>
    /// 单据号（SAP MARA.ZEINR）
    /// </summary>
    [SugarColumn(ColumnName = "document_number", ColumnDescription = "单据号", ColumnDataType = "nvarchar", Length = 22, IsNullable = true)]
    public string? DocumentNumber { get; set; }

    /// <summary>
    /// 单据类型（SAP MARA.ZEIAR）
    /// </summary>
    [SugarColumn(ColumnName = "document_type", ColumnDescription = "单据类型", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? DocumentType { get; set; }

    /// <summary>
    /// 单据版本（SAP MARA.ZEIVR）
    /// </summary>
    [SugarColumn(ColumnName = "document_version", ColumnDescription = "单据版本", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? DocumentVersion { get; set; }

    /// <summary>
    /// 单据页格式（SAP MARA.ZEIFO）
    /// </summary>
    [SugarColumn(ColumnName = "document_page_format", ColumnDescription = "单据页格式", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? DocumentPageFormat { get; set; }

    /// <summary>
    /// 单据更改号（SAP MARA.AESZN）
    /// </summary>
    [SugarColumn(ColumnName = "document_change_number", ColumnDescription = "单据更改号", ColumnDataType = "nvarchar", Length = 6, IsNullable = true)]
    public string? DocumentChangeNumber { get; set; }

    /// <summary>
    /// 单据页号（SAP MARA.BLATT）
    /// </summary>
    [SugarColumn(ColumnName = "document_page_number", ColumnDescription = "单据页号", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? DocumentPageNumber { get; set; }

    /// <summary>
    /// 单据页数（SAP MARA.BLANZ）
    /// </summary>
    [SugarColumn(ColumnName = "document_sheet_count", ColumnDescription = "单据页数", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? DocumentSheetCount { get; set; }

    /// <summary>
    /// 生产/检验备忘（SAP MARA.FERTH）
    /// </summary>
    [SugarColumn(ColumnName = "production_inspection_memo", ColumnDescription = "生产/检验备忘", ColumnDataType = "nvarchar", Length = 18, IsNullable = true)]
    public string? ProductionInspectionMemo { get; set; }

    /// <summary>
    /// 生产备忘页格式（SAP MARA.FORMT）
    /// </summary>
    [SugarColumn(ColumnName = "production_memo_page_format", ColumnDescription = "生产备忘页格式", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? ProductionMemoPageFormat { get; set; }

    /// <summary>
    /// 尺寸/规格（SAP MARA.GROES）
    /// </summary>
    [SugarColumn(ColumnName = "size_dimensions", ColumnDescription = "尺寸/规格", ColumnDataType = "nvarchar", Length = 32, IsNullable = true)]
    public string? SizeDimensions { get; set; }

    /// <summary>
    /// 基本物料（材质）（SAP MARA.WRKST）
    /// </summary>
    [SugarColumn(ColumnName = "basic_material", ColumnDescription = "基本物料（材质）", ColumnDataType = "nvarchar", Length = 48, IsNullable = true)]
    public string? BasicMaterial { get; set; }

    /// <summary>
    /// 行业标准描述（SAP MARA.NORMT）
    /// </summary>
    [SugarColumn(ColumnName = "industry_standard_description", ColumnDescription = "行业标准描述", ColumnDataType = "nvarchar", Length = 18, IsNullable = true)]
    public string? IndustryStandardDescription { get; set; }

    /// <summary>
    /// 实验室/设计室（SAP MARA.LABOR）
    /// </summary>
    [SugarColumn(ColumnName = "laboratory_design_office", ColumnDescription = "实验室/设计室", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? LaboratoryDesignOffice { get; set; }

    /// <summary>
    /// 采购价值码（SAP MARA.EKWSL）
    /// </summary>
    [SugarColumn(ColumnName = "purchasing_value_key", ColumnDescription = "采购价值码", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? PurchasingValueKey { get; set; }

    /// <summary>
    /// 毛重（SAP MARA.BRGEW）
    /// </summary>
    [SugarColumn(ColumnName = "gross_weight", ColumnDescription = "毛重", ColumnDataType = "decimal", Length = 18, DecimalDigits = 3, IsNullable = true)]
    public decimal? GrossWeight { get; set; }

    /// <summary>
    /// 净重（SAP MARA.NTGEW）
    /// </summary>
    [SugarColumn(ColumnName = "net_weight", ColumnDescription = "净重", ColumnDataType = "decimal", Length = 18, DecimalDigits = 3, IsNullable = true)]
    public decimal? NetWeight { get; set; }

    /// <summary>
    /// 重量单位（SAP MARA.GEWEI）
    /// </summary>
    [SugarColumn(ColumnName = "weight_unit", ColumnDescription = "重量单位", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? WeightUnit { get; set; }

    /// <summary>
    /// 体积（SAP MARA.VOLUM）
    /// </summary>
    [SugarColumn(ColumnName = "volume", ColumnDescription = "体积", ColumnDataType = "decimal", Length = 18, DecimalDigits = 3, IsNullable = true)]
    public decimal? Volume { get; set; }

    /// <summary>
    /// 体积单位（SAP MARA.VOLEH）
    /// </summary>
    [SugarColumn(ColumnName = "volume_unit", ColumnDescription = "体积单位", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? VolumeUnit { get; set; }

    /// <summary>
    /// 容器要求（SAP MARA.BEHVO）
    /// </summary>
    [SugarColumn(ColumnName = "container_requirements", ColumnDescription = "容器要求", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? ContainerRequirements { get; set; }

    /// <summary>
    /// 仓储条件（SAP MARA.RAUBE）
    /// </summary>
    [SugarColumn(ColumnName = "storage_conditions", ColumnDescription = "仓储条件", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? StorageConditions { get; set; }

    /// <summary>
    /// 温度条件（SAP MARA.TEMPB）
    /// </summary>
    [SugarColumn(ColumnName = "temperature_conditions", ColumnDescription = "温度条件", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? TemperatureConditions { get; set; }

    /// <summary>
    /// 低层码（SAP MARA.DISST）
    /// </summary>
    [SugarColumn(ColumnName = "low_level_code", ColumnDescription = "低层码", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? LowLevelCode { get; set; }

    /// <summary>
    /// 运输组（SAP MARA.TRAGR）
    /// </summary>
    [SugarColumn(ColumnName = "transportation_group", ColumnDescription = "运输组", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? TransportationGroup { get; set; }

    /// <summary>
    /// 危险品编码（SAP MARA.STOFF）
    /// </summary>
    [SugarColumn(ColumnName = "hazardous_material_number", ColumnDescription = "危险品编码", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? HazardousMaterialNumber { get; set; }

    /// <summary>
    /// 产品组（SAP MARA.SPART）
    /// </summary>
    [SugarColumn(ColumnName = "division", ColumnDescription = "产品组", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? Division { get; set; }

    /// <summary>
    /// 竞争对手（SAP MARA.KUNNR）
    /// </summary>
    [SugarColumn(ColumnName = "competitor", ColumnDescription = "竞争对手", ColumnDataType = "nvarchar", Length = 10, IsNullable = true)]
    public string? Competitor { get; set; }

    /// <summary>
    /// 欧洲商品号（旧）（SAP MARA.EANNR）
    /// </summary>
    [SugarColumn(ColumnName = "european_article_number_obsolete", ColumnDescription = "欧洲商品号（旧）", ColumnDataType = "nvarchar", Length = 13, IsNullable = true)]
    public string? EuropeanArticleNumberObsolete { get; set; }

    /// <summary>
    /// 收发货凭证打印数量（SAP MARA.WESCH）
    /// </summary>
    [SugarColumn(ColumnName = "gr_gi_slip_quantity", ColumnDescription = "收发货凭证打印数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 3, IsNullable = true)]
    public decimal? GrGiSlipQuantity { get; set; }

    /// <summary>
    /// 采购规则（SAP MARA.BWVOR）
    /// </summary>
    [SugarColumn(ColumnName = "procurement_rule", ColumnDescription = "采购规则", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? ProcurementRule { get; set; }

    /// <summary>
    /// 货源（SAP MARA.BWSCL）
    /// </summary>
    [SugarColumn(ColumnName = "source_of_supply", ColumnDescription = "货源", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? SourceOfSupply { get; set; }

    /// <summary>
    /// 季节类别（SAP MARA.SAISO）
    /// </summary>
    [SugarColumn(ColumnName = "season_category", ColumnDescription = "季节类别", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? SeasonCategory { get; set; }

    /// <summary>
    /// 标签类型（SAP MARA.ETIAR）
    /// </summary>
    [SugarColumn(ColumnName = "label_type", ColumnDescription = "标签类型", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? LabelType { get; set; }

    /// <summary>
    /// 标签格式（SAP MARA.ETIFO）
    /// </summary>
    [SugarColumn(ColumnName = "label_form", ColumnDescription = "标签格式", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? LabelForm { get; set; }

    /// <summary>
    /// 已停用字段（SAP MARA.ENTAR）
    /// </summary>
    [SugarColumn(ColumnName = "deactivated_field", ColumnDescription = "已停用字段", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? DeactivatedField { get; set; }

    /// <summary>
    /// 国际商品编码EAN/UPC（SAP MARA.EAN11）
    /// </summary>
    [SugarColumn(ColumnName = "international_article_number", ColumnDescription = "国际商品编码EAN/UPC", ColumnDataType = "nvarchar", Length = 18, IsNullable = true)]
    public string? InternationalArticleNumber { get; set; }

    /// <summary>
    /// EAN类别（SAP MARA.NUMTP）
    /// </summary>
    [SugarColumn(ColumnName = "ean_category", ColumnDescription = "EAN类别", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? EanCategory { get; set; }

    /// <summary>
    /// 长度（SAP MARA.LAENG）
    /// </summary>
    [SugarColumn(ColumnName = "length", ColumnDescription = "长度", ColumnDataType = "decimal", Length = 18, DecimalDigits = 3, IsNullable = true)]
    public decimal? Length { get; set; }

    /// <summary>
    /// 宽度（SAP MARA.BREIT）
    /// </summary>
    [SugarColumn(ColumnName = "width", ColumnDescription = "宽度", ColumnDataType = "decimal", Length = 18, DecimalDigits = 3, IsNullable = true)]
    public decimal? Width { get; set; }

    /// <summary>
    /// 高度（SAP MARA.HOEHE）
    /// </summary>
    [SugarColumn(ColumnName = "height", ColumnDescription = "高度", ColumnDataType = "decimal", Length = 18, DecimalDigits = 3, IsNullable = true)]
    public decimal? Height { get; set; }

    /// <summary>
    /// 长宽高单位（SAP MARA.MEABM）
    /// </summary>
    [SugarColumn(ColumnName = "dimension_unit", ColumnDescription = "长宽高单位", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? DimensionUnit { get; set; }

    /// <summary>
    /// 产品层次（SAP MARA.PRDHA）
    /// </summary>
    [SugarColumn(ColumnName = "product_hierarchy", ColumnDescription = "产品层次", ColumnDataType = "nvarchar", Length = 18, IsNullable = true)]
    public string? ProductHierarchy { get; set; }

    /// <summary>
    /// 库存调拨净更改成本核算（SAP MARA.AEKLK）
    /// </summary>
    [SugarColumn(ColumnName = "stock_transfer_net_change_costing", ColumnDescription = "库存调拨净更改成本核算", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? StockTransferNetChangeCosting { get; set; }

    /// <summary>
    /// CAD标识（SAP MARA.CADKZ）
    /// </summary>
    [SugarColumn(ColumnName = "cad_indicator", ColumnDescription = "CAD标识", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? CadIndicator { get; set; }

    /// <summary>
    /// 采购QM激活（SAP MARA.QMPUR）
    /// </summary>
    [SugarColumn(ColumnName = "qm_in_procurement", ColumnDescription = "采购QM激活", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? QmInProcurement { get; set; }

    /// <summary>
    /// 允许包装重量（SAP MARA.ERGEW）
    /// </summary>
    [SugarColumn(ColumnName = "allowed_packaging_weight", ColumnDescription = "允许包装重量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 3, IsNullable = true)]
    public decimal? AllowedPackagingWeight { get; set; }

    /// <summary>
    /// 允许包装重量单位（SAP MARA.ERGEI）
    /// </summary>
    [SugarColumn(ColumnName = "allowed_packaging_weight_unit", ColumnDescription = "允许包装重量单位", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? AllowedPackagingWeightUnit { get; set; }

    /// <summary>
    /// 允许包装体积（SAP MARA.ERVOL）
    /// </summary>
    [SugarColumn(ColumnName = "allowed_packaging_volume", ColumnDescription = "允许包装体积", ColumnDataType = "decimal", Length = 18, DecimalDigits = 3, IsNullable = true)]
    public decimal? AllowedPackagingVolume { get; set; }

    /// <summary>
    /// 允许包装体积单位（SAP MARA.ERVOE）
    /// </summary>
    [SugarColumn(ColumnName = "allowed_packaging_volume_unit", ColumnDescription = "允许包装体积单位", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? AllowedPackagingVolumeUnit { get; set; }

    /// <summary>
    /// 超重容差（SAP MARA.GEWTO）
    /// </summary>
    [SugarColumn(ColumnName = "excess_weight_tolerance", ColumnDescription = "超重容差", ColumnDataType = "decimal", Length = 18, DecimalDigits = 1, IsNullable = true)]
    public decimal? ExcessWeightTolerance { get; set; }

    /// <summary>
    /// 超体积容差（SAP MARA.VOLTO）
    /// </summary>
    [SugarColumn(ColumnName = "excess_volume_tolerance", ColumnDescription = "超体积容差", ColumnDataType = "decimal", Length = 18, DecimalDigits = 1, IsNullable = true)]
    public decimal? ExcessVolumeTolerance { get; set; }

    /// <summary>
    /// 可变采购订单单位（SAP MARA.VABME）
    /// </summary>
    [SugarColumn(ColumnName = "variable_purchase_order_unit", ColumnDescription = "可变采购订单单位", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? VariablePurchaseOrderUnit { get; set; }

    /// <summary>
    /// 已分配修订级别（SAP MARA.KZREV）
    /// </summary>
    [SugarColumn(ColumnName = "revision_level_assigned", ColumnDescription = "已分配修订级别", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? RevisionLevelAssigned { get; set; }

    /// <summary>
    /// 可配置物料（SAP MARA.KZKFG）
    /// </summary>
    [SugarColumn(ColumnName = "configurable_material", ColumnDescription = "可配置物料", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? ConfigurableMaterial { get; set; }

    /// <summary>
    /// 批次管理要求（SAP MARA.XCHPF）
    /// </summary>
    [SugarColumn(ColumnName = "batch_management_required", ColumnDescription = "批次管理要求", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? BatchManagementRequired { get; set; }

    /// <summary>
    /// 包装物料类型（SAP MARA.VHART）
    /// </summary>
    [SugarColumn(ColumnName = "packaging_material_type", ColumnDescription = "包装物料类型", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? PackagingMaterialType { get; set; }

    /// <summary>
    /// 最大装载量（体积）（SAP MARA.FUELG）
    /// </summary>
    [SugarColumn(ColumnName = "maximum_level_by_volume", ColumnDescription = "最大装载量（体积）", ColumnDataType = "decimal", Length = 18, DecimalDigits = 0, IsNullable = true)]
    public decimal? MaximumLevelByVolume { get; set; }

    /// <summary>
    /// 堆叠因子（SAP MARA.STFAK）
    /// </summary>
    [SugarColumn(ColumnName = "stacking_factor", ColumnDescription = "堆叠因子", ColumnDataType = "int", IsNullable = true)]
    public int? StackingFactor { get; set; }

    /// <summary>
    /// 包装物料组（SAP MARA.MAGRV）
    /// </summary>
    [SugarColumn(ColumnName = "packaging_material_group", ColumnDescription = "包装物料组", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? PackagingMaterialGroup { get; set; }

    /// <summary>
    /// 权限组（SAP MARA.BEGRU）
    /// </summary>
    [SugarColumn(ColumnName = "authorization_group", ColumnDescription = "权限组", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? AuthorizationGroup { get; set; }

    /// <summary>
    /// 有效起始日期（SAP MARA.DATAB）
    /// </summary>
    [SugarColumn(ColumnName = "valid_from_date", ColumnDescription = "有效起始日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? ValidFromDate { get; set; }

    /// <summary>
    /// 季节年份（SAP MARA.SAISJ）
    /// </summary>
    [SugarColumn(ColumnName = "season_year", ColumnDescription = "季节年份", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? SeasonYear { get; set; }

    /// <summary>
    /// 价格带类别（SAP MARA.PLGTP）
    /// </summary>
    [SugarColumn(ColumnName = "price_band_category", ColumnDescription = "价格带类别", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? PriceBandCategory { get; set; }

    /// <summary>
    /// 空容器BOM（SAP MARA.MLGUT）
    /// </summary>
    [SugarColumn(ColumnName = "empties_bill_of_material", ColumnDescription = "空容器BOM", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? EmptiesBillOfMaterial { get; set; }

    /// <summary>
    /// 外部物料组（SAP MARA.EXTWG）
    /// </summary>
    [SugarColumn(ColumnName = "external_material_group", ColumnDescription = "外部物料组", ColumnDataType = "nvarchar", Length = 18, IsNullable = true)]
    public string? ExternalMaterialGroup { get; set; }

    /// <summary>
    /// 跨工厂可配置物料（SAP MARA.SATNR）
    /// </summary>
    [SugarColumn(ColumnName = "cross_plant_configurable_material", ColumnDescription = "跨工厂可配置物料", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? CrossPlantConfigurableMaterial { get; set; }

    /// <summary>
    /// 物料类别（SAP MARA.ATTYP）
    /// </summary>
    [SugarColumn(ColumnName = "material_category", ColumnDescription = "物料类别", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? MaterialCategory { get; set; }

    /// <summary>
    /// 联产品标识（SAP MARA.KZKUP）
    /// </summary>
    [SugarColumn(ColumnName = "co_product_indicator", ColumnDescription = "联产品标识", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? CoProductIndicator { get; set; }

    /// <summary>
    /// 后续物料标识（SAP MARA.KZNFM）
    /// </summary>
    [SugarColumn(ColumnName = "follow_up_material_indicator", ColumnDescription = "后续物料标识", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? FollowUpMaterialIndicator { get; set; }

    /// <summary>
    /// 定价参考物料（SAP MARA.PMATA）
    /// </summary>
    [SugarColumn(ColumnName = "pricing_reference_material", ColumnDescription = "定价参考物料", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? PricingReferenceMaterial { get; set; }

    /// <summary>
    /// 跨工厂物料状态（SAP MARA.MSTAE）
    /// </summary>
    [SugarColumn(ColumnName = "cross_plant_material_status", ColumnDescription = "跨工厂物料状态", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? CrossPlantMaterialStatus { get; set; }

    /// <summary>
    /// 跨分销链物料状态（SAP MARA.MSTAV）
    /// </summary>
    [SugarColumn(ColumnName = "cross_distribution_chain_status", ColumnDescription = "跨分销链物料状态", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? CrossDistributionChainStatus { get; set; }

    /// <summary>
    /// 跨工厂状态生效日期（SAP MARA.MSTDE）
    /// </summary>
    [SugarColumn(ColumnName = "cross_plant_status_valid_from", ColumnDescription = "跨工厂状态生效日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? CrossPlantStatusValidFrom { get; set; }

    /// <summary>
    /// 跨分销链状态生效日期（SAP MARA.MSTDV）
    /// </summary>
    [SugarColumn(ColumnName = "cross_distribution_status_valid_from", ColumnDescription = "跨分销链状态生效日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? CrossDistributionStatusValidFrom { get; set; }

    /// <summary>
    /// 物料税分类（SAP MARA.TAKLV）
    /// </summary>
    [SugarColumn(ColumnName = "tax_classification", ColumnDescription = "物料税分类", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? TaxClassification { get; set; }

    /// <summary>
    /// 目录参数文件（SAP MARA.RBNRM）
    /// </summary>
    [SugarColumn(ColumnName = "catalog_profile", ColumnDescription = "目录参数文件", ColumnDataType = "nvarchar", Length = 9, IsNullable = true)]
    public string? CatalogProfile { get; set; }

    /// <summary>
    /// 最短剩余货架寿命（SAP MARA.MHDRZ）
    /// </summary>
    [SugarColumn(ColumnName = "minimum_remaining_shelf_life", ColumnDescription = "最短剩余货架寿命", ColumnDataType = "decimal", Length = 18, DecimalDigits = 0, IsNullable = true)]
    public decimal? MinimumRemainingShelfLife { get; set; }

    /// <summary>
    /// 总货架寿命（SAP MARA.MHDHB）
    /// </summary>
    [SugarColumn(ColumnName = "total_shelf_life", ColumnDescription = "总货架寿命", ColumnDataType = "decimal", Length = 18, DecimalDigits = 0, IsNullable = true)]
    public decimal? TotalShelfLife { get; set; }

    /// <summary>
    /// 仓储百分比（SAP MARA.MHDLP）
    /// </summary>
    [SugarColumn(ColumnName = "storage_percentage", ColumnDescription = "仓储百分比", ColumnDataType = "decimal", Length = 18, DecimalDigits = 0, IsNullable = true)]
    public decimal? StoragePercentage { get; set; }

    /// <summary>
    /// 含量单位（SAP MARA.INHME）
    /// </summary>
    [SugarColumn(ColumnName = "content_unit", ColumnDescription = "含量单位", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? ContentUnit { get; set; }

    /// <summary>
    /// 净含量（SAP MARA.INHAL）
    /// </summary>
    [SugarColumn(ColumnName = "net_contents", ColumnDescription = "净含量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 3, IsNullable = true)]
    public decimal? NetContents { get; set; }

    /// <summary>
    /// 比较价格单位（SAP MARA.VPREH）
    /// </summary>
    [SugarColumn(ColumnName = "comparison_price_unit", ColumnDescription = "比较价格单位", ColumnDataType = "decimal", Length = 18, DecimalDigits = 0, IsNullable = true)]
    public decimal? ComparisonPriceUnit { get; set; }

    /// <summary>
    /// 标签物料分组（SAP MARA.ETIAG）
    /// </summary>
    [SugarColumn(ColumnName = "labeling_material_grouping", ColumnDescription = "标签物料分组", ColumnDataType = "nvarchar", Length = 18, IsNullable = true)]
    public string? LabelingMaterialGrouping { get; set; }

    /// <summary>
    /// 毛含量（SAP MARA.INHBR）
    /// </summary>
    [SugarColumn(ColumnName = "gross_contents", ColumnDescription = "毛含量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 3, IsNullable = true)]
    public decimal? GrossContents { get; set; }

    /// <summary>
    /// 数量换算方法（SAP MARA.CMETH）
    /// </summary>
    [SugarColumn(ColumnName = "quantity_conversion_method", ColumnDescription = "数量换算方法", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? QuantityConversionMethod { get; set; }

    /// <summary>
    /// 内部对象号（SAP MARA.CUOBF）
    /// </summary>
    [SugarColumn(ColumnName = "internal_object_number", ColumnDescription = "内部对象号", ColumnDataType = "nvarchar", Length = 18, IsNullable = true)]
    public string? InternalObjectNumber { get; set; }

    /// <summary>
    /// 环境相关（SAP MARA.KZUMW）
    /// </summary>
    [SugarColumn(ColumnName = "environmentally_relevant", ColumnDescription = "环境相关", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? EnvironmentallyRelevant { get; set; }

    /// <summary>
    /// 产品分配确定过程（SAP MARA.KOSCH）
    /// </summary>
    [SugarColumn(ColumnName = "product_allocation_procedure", ColumnDescription = "产品分配确定过程", ColumnDataType = "nvarchar", Length = 18, IsNullable = true)]
    public string? ProductAllocationProcedure { get; set; }

    /// <summary>
    /// 变式定价参数文件（SAP MARA.SPROF）
    /// </summary>
    [SugarColumn(ColumnName = "variant_pricing_profile", ColumnDescription = "变式定价参数文件", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? VariantPricingProfile { get; set; }

    /// <summary>
    /// 实物折扣资格（SAP MARA.NRFHG）
    /// </summary>
    [SugarColumn(ColumnName = "discount_in_kind", ColumnDescription = "实物折扣资格", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? DiscountInKind { get; set; }

    /// <summary>
    /// 制造商零件号（SAP MARA.MFRPN）
    /// </summary>
    [SugarColumn(ColumnName = "manufacturer_part_number", ColumnDescription = "制造商零件号", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? ManufacturerPartNumber { get; set; }

    /// <summary>
    /// 制造商编码（SAP MARA.MFRNR）
    /// </summary>
    [SugarColumn(ColumnName = "manufacturer_number", ColumnDescription = "制造商编码", ColumnDataType = "nvarchar", Length = 10, IsNullable = true)]
    public string? ManufacturerNumber { get; set; }

    /// <summary>
    /// 自有库存管理物料号（SAP MARA.BMATN）
    /// </summary>
    [SugarColumn(ColumnName = "inventory_managed_material_number", ColumnDescription = "自有库存管理物料号", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? InventoryManagedMaterialNumber { get; set; }

    /// <summary>
    /// 制造商零件参数文件（SAP MARA.MPROF）
    /// </summary>
    [SugarColumn(ColumnName = "manufacturer_part_profile", ColumnDescription = "制造商零件参数文件", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? ManufacturerPartProfile { get; set; }

    /// <summary>
    /// 计量单位用途（SAP MARA.KZWSM）
    /// </summary>
    [SugarColumn(ColumnName = "units_of_measure_usage", ColumnDescription = "计量单位用途", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? UnitsOfMeasureUsage { get; set; }

    /// <summary>
    /// 季节推出（SAP MARA.SAITY）
    /// </summary>
    [SugarColumn(ColumnName = "season_rollout", ColumnDescription = "季节推出", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? SeasonRollout { get; set; }

    /// <summary>
    /// 危险品参数文件（SAP MARA.PROFL）
    /// </summary>
    [SugarColumn(ColumnName = "dangerous_goods_profile", ColumnDescription = "危险品参数文件", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? DangerousGoodsProfile { get; set; }

    /// <summary>
    /// 高粘度（SAP MARA.IHIVI）
    /// </summary>
    [SugarColumn(ColumnName = "highly_viscous", ColumnDescription = "高粘度", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? HighlyViscous { get; set; }

    /// <summary>
    /// 散装/液体（SAP MARA.ILOOS）
    /// </summary>
    [SugarColumn(ColumnName = "in_bulk_liquid", ColumnDescription = "散装/液体", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? InBulkLiquid { get; set; }

    /// <summary>
    /// 序列号明确级别（SAP MARA.SERLV）
    /// </summary>
    [SugarColumn(ColumnName = "serial_number_explicitness", ColumnDescription = "序列号明确级别", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? SerialNumberExplicitness { get; set; }

    /// <summary>
    /// 封闭包装（SAP MARA.KZGVH）
    /// </summary>
    [SugarColumn(ColumnName = "closed_packaging", ColumnDescription = "封闭包装", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? ClosedPackaging { get; set; }

    /// <summary>
    /// 需批准批次记录（SAP MARA.XGCHP）
    /// </summary>
    [SugarColumn(ColumnName = "approved_batch_record_required", ColumnDescription = "需批准批次记录", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? ApprovedBatchRecordRequired { get; set; }

    /// <summary>
    /// 有效性参数覆盖（SAP MARA.KZEFF）
    /// </summary>
    [SugarColumn(ColumnName = "effectivity_parameter_override", ColumnDescription = "有效性参数覆盖", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? EffectivityParameterOverride { get; set; }

    /// <summary>
    /// 物料完成级别（SAP MARA.COMPL）
    /// </summary>
    [SugarColumn(ColumnName = "material_completion_level", ColumnDescription = "物料完成级别", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? MaterialCompletionLevel { get; set; }

    /// <summary>
    /// 货架寿命期间标识（SAP MARA.IPRKZ）
    /// </summary>
    [SugarColumn(ColumnName = "shelf_life_period_indicator", ColumnDescription = "货架寿命期间标识", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? ShelfLifePeriodIndicator { get; set; }

    /// <summary>
    /// 货架寿命舍入规则（SAP MARA.RDMHD）
    /// </summary>
    [SugarColumn(ColumnName = "shelf_life_rounding_rule", ColumnDescription = "货架寿命舍入规则", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? ShelfLifeRoundingRule { get; set; }

    /// <summary>
    /// 包装打印产品成分（SAP MARA.PRZUS）
    /// </summary>
    [SugarColumn(ColumnName = "product_composition_on_packaging", ColumnDescription = "包装打印产品成分", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? ProductCompositionOnPackaging { get; set; }

    /// <summary>
    /// 通用项目类别组（SAP MARA.MTPOS_MARA）
    /// </summary>
    [SugarColumn(ColumnName = "general_item_category_group", ColumnDescription = "通用项目类别组", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? GeneralItemCategoryGroup { get; set; }

    /// <summary>
    /// 后勤变式通用物料（SAP MARA.BFLME）
    /// </summary>
    [SugarColumn(ColumnName = "logistical_variants", ColumnDescription = "后勤变式通用物料", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? LogisticalVariants { get; set; }

    /// <summary>
    /// 物料锁定（SAP MARA.MATFI）
    /// </summary>
    [SugarColumn(ColumnName = "material_locked", ColumnDescription = "物料锁定", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? MaterialLocked { get; set; }

    /// <summary>
    /// 配置管理相关（SAP MARA.CMREL）
    /// </summary>
    [SugarColumn(ColumnName = "configuration_management_relevant", ColumnDescription = "配置管理相关", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? ConfigurationManagementRelevant { get; set; }

    /// <summary>
    /// 品种清单类型（SAP MARA.BBTYP）
    /// </summary>
    [SugarColumn(ColumnName = "assortment_list_type", ColumnDescription = "品种清单类型", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? AssortmentListType { get; set; }

    /// <summary>
    /// 到期日期类型（SAP MARA.SLED_BBD）
    /// </summary>
    [SugarColumn(ColumnName = "expiration_date_type", ColumnDescription = "到期日期类型", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? ExpirationDateType { get; set; }

    /// <summary>
    /// GTIN变式（SAP MARA.GTIN_VARIANT）
    /// </summary>
    [SugarColumn(ColumnName = "gtin_variant", ColumnDescription = "GTIN变式", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? GtinVariant { get; set; }

    /// <summary>
    /// 通用物料号（SAP MARA.GENNR）
    /// </summary>
    [SugarColumn(ColumnName = "generic_material_number", ColumnDescription = "通用物料号", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? GenericMaterialNumber { get; set; }

    /// <summary>
    /// 相同包装参考物料（SAP MARA.RMATP）
    /// </summary>
    [SugarColumn(ColumnName = "same_packing_reference_material", ColumnDescription = "相同包装参考物料", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? SamePackingReferenceMaterial { get; set; }

    /// <summary>
    /// 全球数据同步相关（SAP MARA.GDS_RELEVANT）
    /// </summary>
    [SugarColumn(ColumnName = "global_data_sync_relevant", ColumnDescription = "全球数据同步相关", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? GlobalDataSyncRelevant { get; set; }

    /// <summary>
    /// 原产地验收（SAP MARA.WEORA）
    /// </summary>
    [SugarColumn(ColumnName = "acceptance_at_origin", ColumnDescription = "原产地验收", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? AcceptanceAtOrigin { get; set; }

    /// <summary>
    /// 标准HU类型（SAP MARA.HUTYP_DFLT）
    /// </summary>
    [SugarColumn(ColumnName = "standard_hu_type", ColumnDescription = "标准HU类型", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? StandardHuType { get; set; }

    /// <summary>
    /// 易被盗（SAP MARA.PILFERABLE）
    /// </summary>
    [SugarColumn(ColumnName = "pilferable", ColumnDescription = "易被盗", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? Pilferable { get; set; }

    /// <summary>
    /// 仓储存储条件（SAP MARA.WHSTC）
    /// </summary>
    [SugarColumn(ColumnName = "warehouse_storage_condition", ColumnDescription = "仓储存储条件", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? WarehouseStorageCondition { get; set; }

    /// <summary>
    /// 仓储物料组（SAP MARA.WHMATGR）
    /// </summary>
    [SugarColumn(ColumnName = "warehouse_material_group", ColumnDescription = "仓储物料组", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? WarehouseMaterialGroup { get; set; }

    /// <summary>
    /// 处理标识（SAP MARA.HNDLCODE）
    /// </summary>
    [SugarColumn(ColumnName = "handling_indicator", ColumnDescription = "处理标识", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? HandlingIndicator { get; set; }

    /// <summary>
    /// 危险物质相关（SAP MARA.HAZMAT）
    /// </summary>
    [SugarColumn(ColumnName = "hazardous_substances_relevant", ColumnDescription = "危险物质相关", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? HazardousSubstancesRelevant { get; set; }

    /// <summary>
    /// 处理单元类型（SAP MARA.HUTYP）
    /// </summary>
    [SugarColumn(ColumnName = "handling_unit_type", ColumnDescription = "处理单元类型", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? HandlingUnitType { get; set; }

    /// <summary>
    /// 可变皮重（SAP MARA.TARE_VAR）
    /// </summary>
    [SugarColumn(ColumnName = "variable_tare_weight", ColumnDescription = "可变皮重", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? VariableTareWeight { get; set; }

    /// <summary>
    /// 最大允许容量（SAP MARA.MAXC）
    /// </summary>
    [SugarColumn(ColumnName = "maximum_allowed_capacity", ColumnDescription = "最大允许容量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 3, IsNullable = true)]
    public decimal? MaximumAllowedCapacity { get; set; }

    /// <summary>
    /// 超容量容差（SAP MARA.MAXC_TOL）
    /// </summary>
    [SugarColumn(ColumnName = "overcapacity_tolerance", ColumnDescription = "超容量容差", ColumnDataType = "decimal", Length = 18, DecimalDigits = 1, IsNullable = true)]
    public decimal? OvercapacityTolerance { get; set; }

    /// <summary>
    /// 最大包装长度（SAP MARA.MAXL）
    /// </summary>
    [SugarColumn(ColumnName = "maximum_packing_length", ColumnDescription = "最大包装长度", ColumnDataType = "decimal", Length = 18, DecimalDigits = 3, IsNullable = true)]
    public decimal? MaximumPackingLength { get; set; }

    /// <summary>
    /// 最大包装宽度（SAP MARA.MAXB）
    /// </summary>
    [SugarColumn(ColumnName = "maximum_packing_width", ColumnDescription = "最大包装宽度", ColumnDataType = "decimal", Length = 18, DecimalDigits = 3, IsNullable = true)]
    public decimal? MaximumPackingWidth { get; set; }

    /// <summary>
    /// 最大包装高度（SAP MARA.MAXH）
    /// </summary>
    [SugarColumn(ColumnName = "maximum_packing_height", ColumnDescription = "最大包装高度", ColumnDataType = "decimal", Length = 18, DecimalDigits = 3, IsNullable = true)]
    public decimal? MaximumPackingHeight { get; set; }

    /// <summary>
    /// 最大包装尺寸单位（SAP MARA.MAXDIM_UOM）
    /// </summary>
    [SugarColumn(ColumnName = "maximum_packing_dimension_unit", ColumnDescription = "最大包装尺寸单位", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? MaximumPackingDimensionUnit { get; set; }

    /// <summary>
    /// 原产国（SAP MARA.HERKL）
    /// </summary>
    [SugarColumn(ColumnName = "country_of_origin", ColumnDescription = "原产国", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? CountryOfOrigin { get; set; }

    /// <summary>
    /// 物料运费组（SAP MARA.MFRGR）
    /// </summary>
    [SugarColumn(ColumnName = "material_freight_group", ColumnDescription = "物料运费组", ColumnDataType = "nvarchar", Length = 8, IsNullable = true)]
    public string? MaterialFreightGroup { get; set; }

    /// <summary>
    /// 隔离期（SAP MARA.QQTIME）
    /// </summary>
    [SugarColumn(ColumnName = "quarantine_period", ColumnDescription = "隔离期", ColumnDataType = "decimal", Length = 18, DecimalDigits = 0, IsNullable = true)]
    public decimal? QuarantinePeriod { get; set; }

    /// <summary>
    /// 隔离期单位（SAP MARA.QQTIMEUOM）
    /// </summary>
    [SugarColumn(ColumnName = "quarantine_period_unit", ColumnDescription = "隔离期单位", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? QuarantinePeriodUnit { get; set; }

    /// <summary>
    /// 质检组（SAP MARA.QGRP）
    /// </summary>
    [SugarColumn(ColumnName = "quality_inspection_group", ColumnDescription = "质检组", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? QualityInspectionGroup { get; set; }

    /// <summary>
    /// 序列号参数文件（SAP MARA.SERIAL）
    /// </summary>
    [SugarColumn(ColumnName = "serial_number_profile", ColumnDescription = "序列号参数文件", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? SerialNumberProfile { get; set; }

    /// <summary>
    /// 表单名称（SAP MARA.PS_SMARTFORM）
    /// </summary>
    [SugarColumn(ColumnName = "form_name", ColumnDescription = "表单名称", ColumnDataType = "nvarchar", Length = 30, IsNullable = true)]
    public string? FormName { get; set; }

    /// <summary>
    /// 后勤计量单位（SAP MARA.LOGUNIT）
    /// </summary>
    [SugarColumn(ColumnName = "logistics_unit_of_measure", ColumnDescription = "后勤计量单位", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? LogisticsUnitOfMeasure { get; set; }

    /// <summary>
    /// 捕捞重量物料（SAP MARA.CWQREL）
    /// </summary>
    [SugarColumn(ColumnName = "catch_weight_material", ColumnDescription = "捕捞重量物料", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? CatchWeightMaterial { get; set; }

    /// <summary>
    /// 捕捞重量参数文件（SAP MARA.CWQPROC）
    /// </summary>
    [SugarColumn(ColumnName = "catch_weight_profile", ColumnDescription = "捕捞重量参数文件", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? CatchWeightProfile { get; set; }

    /// <summary>
    /// 捕捞重量容差组（SAP MARA.CWQTOLGR）
    /// </summary>
    [SugarColumn(ColumnName = "catch_weight_tolerance_group", ColumnDescription = "捕捞重量容差组", ColumnDataType = "nvarchar", Length = 9, IsNullable = true)]
    public string? CatchWeightToleranceGroup { get; set; }

    /// <summary>
    /// 调整参数文件（SAP MARA.ADPROF）
    /// </summary>
    [SugarColumn(ColumnName = "adjustment_profile", ColumnDescription = "调整参数文件", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? AdjustmentProfile { get; set; }

    /// <summary>
    /// 知识产权ID（SAP MARA.IPMIPPRODUCT）
    /// </summary>
    [SugarColumn(ColumnName = "intellectual_property_id", ColumnDescription = "知识产权ID", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? IntellectualPropertyId { get; set; }

    /// <summary>
    /// 允许变式价格（SAP MARA.ALLOW_PMAT_IGNO）
    /// </summary>
    [SugarColumn(ColumnName = "variant_price_allowed", ColumnDescription = "允许变式价格", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? VariantPriceAllowed { get; set; }

    /// <summary>
    /// 介质（SAP MARA.MEDIUM）
    /// </summary>
    [SugarColumn(ColumnName = "medium", ColumnDescription = "介质", ColumnDataType = "nvarchar", Length = 6, IsNullable = true)]
    public string? Medium { get; set; }

    /// <summary>
    /// 实物商品（SAP MARA.COMMODITY）
    /// </summary>
    [SugarColumn(ColumnName = "physical_commodity", ColumnDescription = "实物商品", ColumnDataType = "nvarchar", Length = 18, IsNullable = true)]
    public string? PhysicalCommodity { get; set; }

    /// <summary>
    /// 动物源（SAP MARA.ANIMAL_ORIGIN）
    /// </summary>
    [SugarColumn(ColumnName = "animal_origin", ColumnDescription = "动物源", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? AnimalOrigin { get; set; }

    /// <summary>
    /// 纺织成分功能（SAP MARA.TEXTILE_COMP_IND）
    /// </summary>
    [SugarColumn(ColumnName = "textile_composition_function", ColumnDescription = "纺织成分功能", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? TextileCompositionFunction { get; set; }

    /// <summary>
    /// 细分结构（SAP MARA.SGT_CSGR）
    /// </summary>
    [SugarColumn(ColumnName = "segmentation_structure", ColumnDescription = "细分结构", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? SegmentationStructure { get; set; }

    /// <summary>
    /// 细分策略（SAP MARA.SGT_COVSA）
    /// </summary>
    [SugarColumn(ColumnName = "segmentation_strategy", ColumnDescription = "细分策略", ColumnDataType = "nvarchar", Length = 8, IsNullable = true)]
    public string? SegmentationStrategy { get; set; }

    /// <summary>
    /// 细分状态（SAP MARA.SGT_STAT）
    /// </summary>
    [SugarColumn(ColumnName = "segmentation_status", ColumnDescription = "细分状态", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? SegmentationStatus { get; set; }

    /// <summary>
    /// 细分范围（SAP MARA.SGT_SCOPE）
    /// </summary>
    [SugarColumn(ColumnName = "segmentation_scope", ColumnDescription = "细分范围", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? SegmentationScope { get; set; }

    /// <summary>
    /// 细分相关（SAP MARA.SGT_REL）
    /// </summary>
    [SugarColumn(ColumnName = "segmentation_relevant", ColumnDescription = "细分相关", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? SegmentationRelevant { get; set; }

    /// <summary>
    /// 时装属性1（SAP MARA.FSH_MG_AT1）
    /// </summary>
    [SugarColumn(ColumnName = "fashion_attribute1", ColumnDescription = "时装属性1", ColumnDataType = "nvarchar", Length = 10, IsNullable = true)]
    public string? FashionAttribute1 { get; set; }

    /// <summary>
    /// 时装属性2（SAP MARA.FSH_MG_AT2）
    /// </summary>
    [SugarColumn(ColumnName = "fashion_attribute2", ColumnDescription = "时装属性2", ColumnDataType = "nvarchar", Length = 10, IsNullable = true)]
    public string? FashionAttribute2 { get; set; }

    /// <summary>
    /// 时装属性3（SAP MARA.FSH_MG_AT3）
    /// </summary>
    [SugarColumn(ColumnName = "fashion_attribute3", ColumnDescription = "时装属性3", ColumnDataType = "nvarchar", Length = 6, IsNullable = true)]
    public string? FashionAttribute3 { get; set; }

    /// <summary>
    /// 季节使用标识（SAP MARA.FSH_SEALV）
    /// </summary>
    [SugarColumn(ColumnName = "season_usage_indicator", ColumnDescription = "季节使用标识", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? SeasonUsageIndicator { get; set; }

    /// <summary>
    /// 库存季节激活（SAP MARA.FSH_SEAIM）
    /// </summary>
    [SugarColumn(ColumnName = "season_active_in_inventory", ColumnDescription = "库存季节激活", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? SeasonActiveInInventory { get; set; }

    /// <summary>
    /// 特性转换ID（SAP MARA.FSH_SC_MID）
    /// </summary>
    [SugarColumn(ColumnName = "characteristic_conversion_id", ColumnDescription = "特性转换ID", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? CharacteristicConversionId { get; set; }

    /// <summary>
    /// ANP代码（SAP MARA.ANP）
    /// </summary>
    [SugarColumn(ColumnName = "anp_code", ColumnDescription = "ANP代码", ColumnDataType = "nvarchar", Length = 9, IsNullable = true)]
    public string? AnpCode { get; set; }

    /// <summary>
    /// 危险品包装状态（SAP MARA.DG_PACK_STATUS）
    /// </summary>
    [SugarColumn(ColumnName = "dangerous_goods_packaging_status", ColumnDescription = "危险品包装状态", ColumnDataType = "nvarchar", Length = 10, IsNullable = true)]
    public string? DangerousGoodsPackagingStatus { get; set; }

    /// <summary>
    /// 物料条件管理（SAP MARA.MCOND）
    /// </summary>
    [SugarColumn(ColumnName = "material_condition_management", ColumnDescription = "物料条件管理", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? MaterialConditionManagement { get; set; }

    /// <summary>
    /// 退货代码（SAP MARA.RETDELC）
    /// </summary>
    [SugarColumn(ColumnName = "return_code", ColumnDescription = "退货代码", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? ReturnCode { get; set; }

    /// <summary>
    /// 退回后勤级别（SAP MARA.LOGLEV_RETO）
    /// </summary>
    [SugarColumn(ColumnName = "return_to_logistics_level", ColumnDescription = "退回后勤级别", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? ReturnToLogisticsLevel { get; set; }

    /// <summary>
    /// NATO物料识别号（SAP MARA.NSNID）
    /// </summary>
    [SugarColumn(ColumnName = "nato_item_identification_number", ColumnDescription = "NATO物料识别号", ColumnDataType = "nvarchar", Length = 9, IsNullable = true)]
    public string? NatoItemIdentificationNumber { get; set; }

    /// <summary>
    /// FFF类别（SAP MARA.IMATN）
    /// </summary>
    [SugarColumn(ColumnName = "fff_class", ColumnDescription = "FFF类别", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? FffClass { get; set; }

    /// <summary>
    /// 替代链编码（SAP MARA.PICNUM）
    /// </summary>
    [SugarColumn(ColumnName = "supersession_chain_number", ColumnDescription = "替代链编码", ColumnDataType = "nvarchar", Length = 18, IsNullable = true)]
    public string? SupersessionChainNumber { get; set; }

    /// <summary>
    /// 季节采购创建状态（SAP MARA.BSTAT）
    /// </summary>
    [SugarColumn(ColumnName = "seasonal_procurement_creation_status", ColumnDescription = "季节采购创建状态", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? SeasonalProcurementCreationStatus { get; set; }

    /// <summary>
    /// 颜色特性内部号（SAP MARA.COLOR_ATINN）
    /// </summary>
    [SugarColumn(ColumnName = "color_characteristic_internal_number", ColumnDescription = "颜色特性内部号", ColumnDataType = "nvarchar", Length = 10, IsNullable = true)]
    public string? ColorCharacteristicInternalNumber { get; set; }

    /// <summary>
    /// 主尺码特性内部号（SAP MARA.SIZE1_ATINN）
    /// </summary>
    [SugarColumn(ColumnName = "main_size_characteristic_internal_number", ColumnDescription = "主尺码特性内部号", ColumnDataType = "nvarchar", Length = 10, IsNullable = true)]
    public string? MainSizeCharacteristicInternalNumber { get; set; }

    /// <summary>
    /// 次尺码特性内部号（SAP MARA.SIZE2_ATINN）
    /// </summary>
    [SugarColumn(ColumnName = "second_size_characteristic_internal_number", ColumnDescription = "次尺码特性内部号", ColumnDataType = "nvarchar", Length = 10, IsNullable = true)]
    public string? SecondSizeCharacteristicInternalNumber { get; set; }

    /// <summary>
    /// 颜色（SAP MARA.COLOR）
    /// </summary>
    [SugarColumn(ColumnName = "color", ColumnDescription = "颜色", ColumnDataType = "nvarchar", Length = 18, IsNullable = true)]
    public string? Color { get; set; }

    /// <summary>
    /// 主尺码（SAP MARA.SIZE1）
    /// </summary>
    [SugarColumn(ColumnName = "main_size", ColumnDescription = "主尺码", ColumnDataType = "nvarchar", Length = 18, IsNullable = true)]
    public string? MainSize { get; set; }

    /// <summary>
    /// 次尺码（SAP MARA.SIZE2）
    /// </summary>
    [SugarColumn(ColumnName = "second_size", ColumnDescription = "次尺码", ColumnDataType = "nvarchar", Length = 18, IsNullable = true)]
    public string? SecondSize { get; set; }

    /// <summary>
    /// 评估特性值（SAP MARA.FREE_CHAR）
    /// </summary>
    [SugarColumn(ColumnName = "evaluation_characteristic_value", ColumnDescription = "评估特性值", ColumnDataType = "nvarchar", Length = 18, IsNullable = true)]
    public string? EvaluationCharacteristicValue { get; set; }

    /// <summary>
    /// 护理代码（SAP MARA.CARE_CODE）
    /// </summary>
    [SugarColumn(ColumnName = "care_code", ColumnDescription = "护理代码", ColumnDataType = "nvarchar", Length = 16, IsNullable = true)]
    public string? CareCode { get; set; }

    /// <summary>
    /// 品牌（SAP MARA.BRAND_ID）
    /// </summary>
    [SugarColumn(ColumnName = "brand_id", ColumnDescription = "品牌", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? BrandId { get; set; }

    /// <summary>
    /// 纤维代码1（SAP MARA.FIBER_CODE1）
    /// </summary>
    [SugarColumn(ColumnName = "fiber_code1", ColumnDescription = "纤维代码1", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? FiberCode1 { get; set; }

    /// <summary>
    /// 纤维占比1（SAP MARA.FIBER_PART1）
    /// </summary>
    [SugarColumn(ColumnName = "fiber_part1", ColumnDescription = "纤维占比1", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? FiberPart1 { get; set; }

    /// <summary>
    /// 纤维代码2（SAP MARA.FIBER_CODE2）
    /// </summary>
    [SugarColumn(ColumnName = "fiber_code2", ColumnDescription = "纤维代码2", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? FiberCode2 { get; set; }

    /// <summary>
    /// 纤维占比2（SAP MARA.FIBER_PART2）
    /// </summary>
    [SugarColumn(ColumnName = "fiber_part2", ColumnDescription = "纤维占比2", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? FiberPart2 { get; set; }

    /// <summary>
    /// 纤维代码3（SAP MARA.FIBER_CODE3）
    /// </summary>
    [SugarColumn(ColumnName = "fiber_code3", ColumnDescription = "纤维代码3", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? FiberCode3 { get; set; }

    /// <summary>
    /// 纤维占比3（SAP MARA.FIBER_PART3）
    /// </summary>
    [SugarColumn(ColumnName = "fiber_part3", ColumnDescription = "纤维占比3", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? FiberPart3 { get; set; }

    /// <summary>
    /// 纤维代码4（SAP MARA.FIBER_CODE4）
    /// </summary>
    [SugarColumn(ColumnName = "fiber_code4", ColumnDescription = "纤维代码4", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? FiberCode4 { get; set; }

    /// <summary>
    /// 纤维占比4（SAP MARA.FIBER_PART4）
    /// </summary>
    [SugarColumn(ColumnName = "fiber_part4", ColumnDescription = "纤维占比4", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? FiberPart4 { get; set; }

    /// <summary>
    /// 纤维代码5（SAP MARA.FIBER_CODE5）
    /// </summary>
    [SugarColumn(ColumnName = "fiber_code5", ColumnDescription = "纤维代码5", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? FiberCode5 { get; set; }

    /// <summary>
    /// 纤维占比5（SAP MARA.FIBER_PART5）
    /// </summary>
    [SugarColumn(ColumnName = "fiber_part5", ColumnDescription = "纤维占比5", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? FiberPart5 { get; set; }

    /// <summary>
    /// 时装等级（SAP MARA.FASHGRD）
    /// </summary>
    [SugarColumn(ColumnName = "fashion_grade", ColumnDescription = "时装等级", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? FashionGrade { get; set; }

    /// <summary>
    /// 物料状态（字典 sys_normal_disable_status；0=禁用，1=启用，2=锁定；平台启用态，非 SAP MSTAE）
    /// </summary>
    [SugarColumn(ColumnName = "material_status", ColumnDescription = "物料状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int MaterialStatus { get; set; } = 1;

    // ========================================
    // 导航属性区域
    // ========================================

    /// <summary>
    /// 多语言描述列表（主子表关系；对齐 SAP MAKT）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktMaterialDescription.MaterialId))]
    public List<TaktMaterialDescription>? Descriptions { get; set; }
}

