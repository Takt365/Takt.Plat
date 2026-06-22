// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Logistics.Sales
// 文件名称：TaktSalesQuotationItem.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt销售报价明细实体，定义销售报价明细领域模型
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Sales;

/// <summary>
/// Takt销售报价明细实体
/// </summary>
[SugarTable("takt_logistics_sales_quotation_item", "销售报价明细表")]
[SugarIndex("ix_sales_quotation_item_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_sales_quotation_item_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_sales_quotation_item_line_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(SalesQuotationId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, true)]
public class TaktSalesQuotationItem : TaktCompanyEntityBase
{
    /// <summary>
    /// 销售报价ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [SugarColumn(ColumnName = "sales_quotation_id", ColumnDescription = "销售报价ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesQuotationId { get; set; }

    /// <summary>
    /// 销售报价编码（冗余字段，便于查询）
    /// </summary>
    [SugarColumn(ColumnName = "sales_quotation_code", ColumnDescription = "销售报价编码", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string SalesQuotationCode { get; set; } = string.Empty;

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
    [SugarColumn(ColumnName = "material_name", ColumnDescription = "物料名称", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格
    /// </summary>
    [SugarColumn(ColumnName = "material_specification", ColumnDescription = "物料规格", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? MaterialSpecification { get; set; }

    /// <summary>
    /// 销售单位
    /// </summary>
    [SugarColumn(ColumnName = "sales_unit", ColumnDescription = "销售单位", ColumnDataType = "nvarchar", Length = 20, IsNullable = false, DefaultValue = "个")]
    public string SalesUnit { get; set; } = "个";

    /// <summary>
    /// 报价数量（基本单位数量）
    /// </summary>
    [SugarColumn(ColumnName = "quotation_quantity", ColumnDescription = "报价数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal QuotationQuantity { get; set; } = 0;

    /// <summary>
    /// 单价
    /// </summary>
    [SugarColumn(ColumnName = "unit_price", ColumnDescription = "单价", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal UnitPrice { get; set; } = 0;

    /// <summary>
    /// 折扣率（0-100，表示折扣百分比）
    /// </summary>
    [SugarColumn(ColumnName = "discount_rate", ColumnDescription = "折扣率", ColumnDataType = "decimal", Length = 5, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal DiscountRate { get; set; } = 0;

    /// <summary>
    /// 折扣金额
    /// </summary>
    [SugarColumn(ColumnName = "discount_amount", ColumnDescription = "折扣金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal DiscountAmount { get; set; } = 0;

    /// <summary>
    /// 税费率（0-100，表示税费百分比）
    /// </summary>
    [SugarColumn(ColumnName = "tax_rate", ColumnDescription = "税费率", ColumnDataType = "decimal", Length = 5, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal TaxRate { get; set; } = 0;

    /// <summary>
    /// 税费
    /// </summary>
    [SugarColumn(ColumnName = "tax_amount", ColumnDescription = "税费", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal TaxAmount { get; set; } = 0;

    /// <summary>
    /// 小计金额
    /// </summary>
    [SugarColumn(ColumnName = "subtotal_amount", ColumnDescription = "小计金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal SubtotalAmount { get; set; } = 0;

    /// <summary>
    /// 销售报价主表
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(SalesQuotationId))]
    public TaktSalesQuotation? SalesQuotation { get; set; }
}
