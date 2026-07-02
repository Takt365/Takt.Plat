// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Logistics.Procurement
// 文件名称：TaktPurchasePriceScale.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt采购价格阶梯实体，定义采购价格阶梯领域模型
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Procurement;

/// <summary>
/// Takt采购价格阶梯实体
/// </summary>
[SugarTable("takt_logistics_materials_purchase_price_scale", "采购价格阶梯表")]
[SugarIndex("ix_purchase_price_scale_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_purchase_price_scale_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_materials_purchase_price_scale_item_line_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PurchasePriceItemId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, nameof(StartQuantity), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_materials_purchase_price_scale_purchase_price_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PurchasePriceCode), OrderByType.Asc, false)]
public class TaktPurchasePriceScale : TaktCompanyEntityBase
{
    /// <summary>
    /// 采购价格明细 ID（关联 TaktPurchasePriceItem.Id，选项 TaktPurchasePriceItems/options）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_price_item_id", ColumnDescription = "采购价格明细ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchasePriceItemId { get; set; }

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
    /// 起始数量（基本单位数量，包含此数量）
    /// </summary>
    [SugarColumn(ColumnName = "start_quantity", ColumnDescription = "起始数量", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int StartQuantity { get; set; } = 0;

    /// <summary>
    /// 结束数量（基本单位数量，包含此数量，0表示无上限）
    /// </summary>
    [SugarColumn(ColumnName = "end_quantity", ColumnDescription = "结束数量", ColumnDataType = "int",  IsNullable = false, DefaultValue = "0")]
    public int EndQuantity { get; set; } = 0;

    /// <summary>
    /// 阶梯价格（精确到分，存储为整数，单位为分）
    /// </summary>
    [SugarColumn(ColumnName = "scale_price", ColumnDescription = "阶梯价格", ColumnDataType = "decimal", Length = 18, DecimalDigits = 5, IsNullable = false, DefaultValue = "0")]
    public decimal ScalePrice { get; set; } = 0;

    /// <summary>
    /// 排序号（越小越靠前）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;
}
