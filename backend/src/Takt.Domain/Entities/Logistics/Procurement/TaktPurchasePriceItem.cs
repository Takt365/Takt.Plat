// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Logistics.Procurement
// 文件名称：TaktPurchasePriceItem.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt采购价格明细实体，定义供应商物料价格领域模型
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Procurement;

/// <summary>
/// Takt采购价格明细实体（供应商物料价格明细表）
/// </summary>
[SugarTable("takt_logistics_materials_purchase_price_item", "采购价格明细表")]
[SugarIndex("ix_purchase_price_item_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_purchase_price_item_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_materials_purchase_price_item_price_line_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PurchasePriceId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, nameof(MaterialCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_materials_purchase_price_item_material_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(MaterialCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_materials_purchase_price_item_purchase_price_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PurchasePriceCode), OrderByType.Asc, false)]
public class TaktPurchasePriceItem : TaktCompanyEntityBase
{
    /// <summary>
    /// 采购价格 ID（关联 TaktPurchasePrice.Id，选项 TaktPurchasePrices/options）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_price_id", ColumnDescription = "采购价格ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchasePriceId { get; set; }

    /// <summary>
    /// 采购价格编码（冗余字段，便于查询）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_price_code", ColumnDescription = "采购价格编码", ColumnDataType = "nvarchar", Length = 10, IsNullable = false)]
    public string PurchasePriceCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "行号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 物料编码
    /// </summary>
    [SugarColumn(ColumnName = "material_code", ColumnDescription = "物料编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称
    /// </summary>
    [SugarColumn(ColumnName = "material_name", ColumnDescription = "物料名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? MaterialName { get; set; }

    /// <summary>
    /// 物料规格
    /// </summary>
    [SugarColumn(ColumnName = "material_specification", ColumnDescription = "物料规格", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? MaterialSpecification { get; set; }

    /// <summary>
    /// 采购单位
    /// </summary>
    [SugarColumn(ColumnName = "purchase_unit", ColumnDescription = "采购单位", ColumnDataType = "nvarchar", Length = 20, IsNullable = false, DefaultValue = "PC")]
    public string PurchaseUnit { get; set; } = "PC";

    /// <summary>
    /// 价格单位（字典 logistics_price_unit_param：1/100/1000/10000；默认 1000）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_per_unit", ColumnDescription = "价格单位", ColumnDataType = "int", IsNullable = false, DefaultValue = "1000")]
    public int PurchasePerUnit { get; set; } = 1000;

    /// <summary>
    /// 采购价格（decimal(18,5)）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_price", ColumnDescription = "采购价格", ColumnDataType = "decimal", Length = 18, DecimalDigits = 5, IsNullable = false, DefaultValue = "0")]
    public decimal PurchasePrice { get; set; } = 0;

    /// <summary>
    /// 最小采购量（基本单位数量）
    /// </summary>
    [SugarColumn(ColumnName = "min_purchase_quantity", ColumnDescription = "最小采购量", ColumnDataType = "int",  IsNullable = false, DefaultValue = "0")]
    public int MinPurchaseQuantity { get; set; } = 0;

    /// <summary>
    /// 最大采购量（基本单位数量，0表示无限制）
    /// </summary>
    [SugarColumn(ColumnName = "max_purchase_quantity", ColumnDescription = "最大采购量", ColumnDataType = "int",  IsNullable = false, DefaultValue = "0")]
    public int MaxPurchaseQuantity { get; set; } = 0;

    /// <summary>
    /// 排序号（越小越靠前）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    [SugarColumn(ColumnName = "is_obsolete", ColumnDescription = "是否作废", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 价格阶梯列表（主子表关系，一个物料价格可以有多个阶梯）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktPurchasePriceScale.PurchasePriceItemId))]
    public List<TaktPurchasePriceScale>? Scales { get; set; }
}
