// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Bom
// 文件名称：TaktBomMaterialCostItem.cs
// 创建时间：2026-07-13
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 物料成本展开行（产品-组件用量与移动平均价/采购净价快照，对齐 SAP CK40N 成本构成清单字段）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Bom;

/// <summary>
/// BOM 物料成本明细行（业务源数据：先导入/维护明细，再按工厂+产品+核算月聚合写入 TaktBomMaterialCost；无线上主表外键）
/// </summary>
[SugarTable("takt_logistics_manufacturing_bom_material_cost_item", "BOM物料成本明细表")]
[SugarIndex("ix_bom_material_cost_item_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_bom_material_cost_item_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex(
    "ix_takt_logistics_manufacturing_bom_material_cost_item_line_unique",
    new[]
    {
        nameof(TenantCode), nameof(CompanyCode), nameof(PlantCode), nameof(ProductCode),
        nameof(SequenceNo), nameof(BomLevel), nameof(BomItemNo), nameof(ComponentCode),
        nameof(ComponentQuantity), nameof(BatchIndicator), nameof(ProductionRelated),
        nameof(PurchaseType), nameof(SpecialProcurementType), nameof(CostingDate)
    },
    new[]
    {
        OrderByType.Asc, OrderByType.Asc, OrderByType.Asc, OrderByType.Asc,
        OrderByType.Asc, OrderByType.Asc, OrderByType.Asc, OrderByType.Asc,
        OrderByType.Asc, OrderByType.Asc, OrderByType.Asc, OrderByType.Asc,
        OrderByType.Asc, OrderByType.Asc
    },
    true)]
[SugarIndex("ix_takt_logistics_manufacturing_bom_material_cost_item_plant_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_bom_material_cost_item_product_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ProductCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_bom_material_cost_item_component_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ComponentCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_bom_material_cost_item_costing_date", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(CostingDate), OrderByType.Asc, false)]
public class TaktBomMaterialCostItem : TaktCompanyEntityBase
{
    /// <summary>
    /// 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "plant_code", ColumnDescription = "工厂代码", ColumnDataType = "nvarchar", Length = 4, IsNullable = false)]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品编码（父件物料编码，选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode）；导入时 18 位纯数字自动归一化为后 10 位
    /// </summary>
    [SugarColumn(ColumnName = "product_code", ColumnDescription = "产品编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// 序号（展开行序号，如 0010）
    /// </summary>
    [SugarColumn(ColumnName = "sequence_no", ColumnDescription = "序号", ColumnDataType = "nvarchar", Length = 4, IsNullable = false)]
    public string SequenceNo { get; set; } = string.Empty;

    /// <summary>
    /// 产品描述
    /// </summary>
    [SugarColumn(ColumnName = "product_description", ColumnDescription = "产品描述", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string ProductDescription { get; set; } = string.Empty;

    /// <summary>
    /// 层级（BOM 展开层级，如 01/02）
    /// </summary>
    [SugarColumn(ColumnName = "bom_level", ColumnDescription = "层级", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string BomLevel { get; set; } = string.Empty;

    /// <summary>
    /// BOM 项目号（子件行项目号，如 0010）
    /// </summary>
    [SugarColumn(ColumnName = "bom_item_no", ColumnDescription = "BOM项目号", ColumnDataType = "nvarchar", Length = 4, IsNullable = false)]
    public string BomItemNo { get; set; } = string.Empty;

    /// <summary>
    /// 组件编码（子件物料编码，选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode）；导入时 18 位纯数字自动归一化为后 10 位
    /// </summary>
    [SugarColumn(ColumnName = "component_code", ColumnDescription = "组件编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string ComponentCode { get; set; } = string.Empty;

    /// <summary>
    /// 组件描述
    /// </summary>
    [SugarColumn(ColumnName = "component_description", ColumnDescription = "组件描述", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string ComponentDescription { get; set; } = string.Empty;

    /// <summary>
    /// 组件数量
    /// </summary>
    [SugarColumn(ColumnName = "component_quantity", ColumnDescription = "组件数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal ComponentQuantity { get; set; } = 0;

    /// <summary>
    /// 批量标识（空或 X）
    /// </summary>
    [SugarColumn(ColumnName = "batch_indicator", ColumnDescription = "批量标识", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? BatchIndicator { get; set; }

    /// <summary>
    /// 生产相关（空或 X）
    /// </summary>
    [SugarColumn(ColumnName = "production_related", ColumnDescription = "生产相关", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? ProductionRelated { get; set; }

    /// <summary>
    /// 采购类型（F=外部采购，E=自制生产）；仅生产相关=X 且 F 行参与产品 BOM 材料成本汇总，行成本=组件数量×(移动平均价÷移动价格单位) 保留 5 位小数
    /// </summary>
    [SugarColumn(ColumnName = "purchase_type", ColumnDescription = "采购类型", ColumnDataType = "nvarchar", Length = 1, IsNullable = false, DefaultValue = "F")]
    public string PurchaseType { get; set; } = "F";

    /// <summary>
    /// 特殊采购类（空或业务码，最长 50）
    /// </summary>
    [SugarColumn(ColumnName = "special_procurement_type", ColumnDescription = "特殊采购类", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? SpecialProcurementType { get; set; }

    /// <summary>
    /// 利润中心（选项 TaktProfitCenters/options，DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "profit_center_code", ColumnDescription = "利润中心", ColumnDataType = "nvarchar", Length = 4, IsNullable = false)]
    public string ProfitCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 移动平均价（5 位小数）
    /// </summary>
    [SugarColumn(ColumnName = "moving_average_price", ColumnDescription = "移动平均价", ColumnDataType = "decimal", Length = 18, DecimalDigits = 5, IsNullable = false, DefaultValue = "0")]
    public decimal MovingAveragePrice { get; set; } = 0;

    /// <summary>
    /// 移动价格单位
    /// </summary>
    [SugarColumn(ColumnName = "moving_price_unit", ColumnDescription = "移动价格单位", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int MovingPriceUnit { get; set; } = 1;

    /// <summary>
    /// 移动价格货币（字典 accounting_currency_code，如 CNY/USD）
    /// </summary>
    [SugarColumn(ColumnName = "moving_price_currency", ColumnDescription = "移动价格货币", ColumnDataType = "nvarchar", Length = 3, IsNullable = false)]
    public string MovingPriceCurrency { get; set; } = string.Empty;

    /// <summary>
    /// 采购组织
    /// </summary>
    [SugarColumn(ColumnName = "purchase_organization", ColumnDescription = "采购组织", ColumnDataType = "nvarchar", Length = 4, IsNullable = false)]
    public string PurchaseOrganization { get; set; } = string.Empty;

    /// <summary>
    /// 采购组（选项 TaktPurchaseGroups/options，DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_group", ColumnDescription = "采购组", ColumnDataType = "nvarchar", Length = 3, IsNullable = false)]
    public string PurchaseGroup { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options，DictValue=SupplierCode）
    /// </summary>
    [SugarColumn(ColumnName = "supplier_code", ColumnDescription = "供应商", ColumnDataType = "nvarchar", Length = 8, IsNullable = false)]
    public string SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 净价（采购价格，5 位小数）
    /// </summary>
    [SugarColumn(ColumnName = "net_purchase_price", ColumnDescription = "净价", ColumnDataType = "decimal", Length = 18, DecimalDigits = 5, IsNullable = false, DefaultValue = "0")]
    public decimal NetPurchasePrice { get; set; } = 0;

    /// <summary>
    /// 采购价格单位
    /// </summary>
    [SugarColumn(ColumnName = "purchase_price_unit", ColumnDescription = "采购价格单位", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int PurchasePriceUnit { get; set; } = 1;

    /// <summary>
    /// 采购货币（字典 accounting_currency_code，如 CNY/USD）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_currency", ColumnDescription = "采购货币", ColumnDataType = "nvarchar", Length = 3, IsNullable = false)]
    public string PurchaseCurrency { get; set; } = string.Empty;

    /// <summary>
    /// 核算日期
    /// </summary>
    [SugarColumn(ColumnName = "costing_date", ColumnDescription = "核算日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime CostingDate { get; set; }
}
