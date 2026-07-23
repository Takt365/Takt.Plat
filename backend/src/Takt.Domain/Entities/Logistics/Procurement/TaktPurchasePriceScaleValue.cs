// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Logistics.Procurement
// 文件名称：TaktPurchasePriceScaleValue.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt采购价格价值等级实体（SAP KONW：定价记录号/定价序号/等级序号/等级值/价格）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Procurement;

/// <summary>
/// Takt采购价格价值等级实体（；主子表：TaktPurchasePriceItem → ScaleValues；与数量等级仅差 ScaleValue↔ScaleQuantity）
/// </summary>
[SugarTable("takt_logistics_materials_purchase_price_scale_value", "采购价格价值等级表")]
[SugarIndex("ix_purchase_price_scale_value_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_purchase_price_scale_value_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_materials_purchase_price_scale_value_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PurchasePriceItemId), OrderByType.Asc, nameof(PurchasePriceCode), OrderByType.Asc, nameof(PurchasePriceSeq), OrderByType.Asc, nameof(PurchaseScaleSeq), OrderByType.Asc, nameof(ScaleValue), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_materials_purchase_price_scale_value_code_seq", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PurchasePriceCode), OrderByType.Asc, nameof(PurchasePriceSeq), OrderByType.Asc, false)]
public class TaktPurchasePriceScaleValue : TaktCompanyEntityBase
{
    /// <summary>
    /// 采购价格明细 ID（主子表关系；选项 TaktPurchasePriceItems/options，DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_price_item_id", ColumnDescription = "采购价格明细ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchasePriceItemId { get; set; }

    /// <summary>
    /// 定价记录号（KNUMH；冗余；与主表/明细 PurchasePriceCode 一致，长度 20）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_price_code", ColumnDescription = "定价记录号", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string PurchasePriceCode { get; set; } = string.Empty;

    /// <summary>
    /// 定价序号（冗余；与明细 PurchasePriceSeq 一致，固定步长=10）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_price_seq", ColumnDescription = "定价序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "10")]
    public int PurchasePriceSeq { get; set; } = 10;

    /// <summary>
    /// 等级序号（KOPOS；同一明细内阶梯序号，固定步长=10）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_scale_seq", ColumnDescription = "等级序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "10")]
    public int PurchaseScaleSeq { get; set; } = 10;

    /// <summary>
    /// 等级值（KSTBW；价值等级门槛；对应数量等级表的 ScaleQuantity）
    /// </summary>
    [SugarColumn(ColumnName = "scale_value", ColumnDescription = "等级值", ColumnDataType = "decimal", Length = 18, DecimalDigits = 5, IsNullable = false, DefaultValue = "0")]
    public decimal ScaleValue { get; set; } = 0;

    /// <summary>
    /// 价格（KBETR）
    /// </summary>
    [SugarColumn(ColumnName = "price", ColumnDescription = "价格", ColumnDataType = "decimal", Length = 18, DecimalDigits = 5, IsNullable = false, DefaultValue = "0")]
    public decimal Price { get; set; } = 0;

    /// <summary>
    /// 未税价格（冗余；可由 Price 与税码推算后回写）
    /// </summary>
    [SugarColumn(ColumnName = "untaxed_price", ColumnDescription = "未税价格", ColumnDataType = "decimal", Length = 18, DecimalDigits = 5, IsNullable = false, DefaultValue = "0")]
    public decimal UntaxedPrice { get; set; } = 0;

    /// <summary>
    /// 含税价格（冗余；可由 Price 与税码推算后回写）
    /// </summary>
    [SugarColumn(ColumnName = "tax_included_price", ColumnDescription = "含税价格", ColumnDataType = "decimal", Length = 18, DecimalDigits = 5, IsNullable = false, DefaultValue = "0")]
    public decimal TaxIncludedPrice { get; set; } = 0;
    /// <summary>
    /// 税费（冗余；含税−未税，打印用）
    /// </summary>
    [SugarColumn(ColumnName = "tax_amount", ColumnDescription = "税费", ColumnDataType = "decimal", Length = 18, DecimalDigits = 5, IsNullable = false, DefaultValue = "0")]
    public decimal TaxAmount { get; set; } = 0;

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    [SugarColumn(ColumnName = "is_obsolete", ColumnDescription = "是否作废", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsObsolete { get; set; } = 0;
}
