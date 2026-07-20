// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Logistics.Sales
// 文件名称：TaktSalesPriceScaleValue.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt销售价格价值等级实体（SAP KONW：定价记录号/条件序列号/行号/等级值/金额）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Sales;

/// <summary>
/// Takt销售价格价值等级实体（SAP KONW；主子表：TaktSalesPriceItem → ScaleValues；与数量等级仅差 ScaleValue↔ScaleQuantity）
/// </summary>
[SugarTable("takt_logistics_sales_price_scale_value", "销售价格价值等级表")]
[SugarIndex("ix_sales_price_scale_value_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_sales_price_scale_value_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_sales_price_scale_value_line_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(SalesPriceItemId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_sales_price_scale_value_code_seq", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(SalesPriceCode), OrderByType.Asc, nameof(SalesPriceSeq), OrderByType.Asc, false)]
public class TaktSalesPriceScaleValue : TaktCompanyEntityBase
{
    /// <summary>
    /// 销售价格明细 ID（主子表关系；选项 TaktSalesPriceItems/options，DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "sales_price_item_id", ColumnDescription = "销售价格明细ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesPriceItemId { get; set; }

    /// <summary>
    /// 定价记录号（KNUMH；冗余；与主表/明细 SalesPriceCode 一致，长度 20）
    /// </summary>
    [SugarColumn(ColumnName = "sales_price_code", ColumnDescription = "定价记录号", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string SalesPriceCode { get; set; } = string.Empty;

    /// <summary>
    /// 条件序列号（KOPOS；冗余；与明细 SalesPriceSeq 一致）
    /// </summary>
    [SugarColumn(ColumnName = "sales_price_seq", ColumnDescription = "条件序列号", ColumnDataType = "int", IsNullable = false, DefaultValue = "10")]
    public int SalesPriceSeq { get; set; } = 10;

    /// <summary>
    /// 行号（KLFN1；阶梯行序号，固定步长=10）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "行号", ColumnDataType = "int", IsNullable = false, DefaultValue = "10")]
    public int LineNumber { get; set; } = 10;

    /// <summary>
    /// 等级值（KSTBW；价值等级门槛；对应数量等级表的 ScaleQuantity）
    /// </summary>
    [SugarColumn(ColumnName = "scale_value", ColumnDescription = "等级值", ColumnDataType = "decimal", Length = 18, DecimalDigits = 5, IsNullable = false, DefaultValue = "0")]
    public decimal ScaleValue { get; set; } = 0;

    /// <summary>
    /// 金额（KBETR）
    /// </summary>
    [SugarColumn(ColumnName = "amount", ColumnDescription = "金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 5, IsNullable = false, DefaultValue = "0")]
    public decimal Amount { get; set; } = 0;

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    [SugarColumn(ColumnName = "is_obsolete", ColumnDescription = "是否作废", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsObsolete { get; set; } = 0;
}
