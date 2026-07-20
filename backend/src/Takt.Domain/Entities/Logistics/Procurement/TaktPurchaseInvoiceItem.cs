// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Logistics.Procurement
// 文件名称：TaktPurchaseInvoiceItem.cs
// 创建时间：2026-06-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt采购发票明细实体，定义采购发票行项目领域模型
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Procurement;

/// <summary>
/// Takt采购发票明细实体
/// </summary>
[SugarTable("takt_logistics_procurement_purchase_invoice_item", "采购发票明细表")]
[SugarIndex("ix_purchase_invoice_item_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_purchase_invoice_item_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_procurement_purchase_invoice_item_invoice_line_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PurchaseInvoiceId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_procurement_purchase_invoice_item_purchase_order_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PurchaseOrderCode), OrderByType.Asc, false)]
public class TaktPurchaseInvoiceItem : TaktCompanyEntityBase
{
    /// <summary>
    /// 采购发票 ID（选项 TaktPurchaseInvoices/options，DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_invoice_id", ColumnDescription = "采购发票ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseInvoiceId { get; set; }

    /// <summary>
    /// 采购发票编码（冗余字段，便于查询）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_invoice_code", ColumnDescription = "采购发票编码", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string PurchaseInvoiceCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "行号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 来源采购订单编码
    /// </summary>
    [SugarColumn(ColumnName = "purchase_order_code", ColumnDescription = "来源采购订单编码", ColumnDataType = "nvarchar", Length = 10, IsNullable = true)]
    public string? PurchaseOrderCode { get; set; }

    /// <summary>
    /// 来源采购订单行号
    /// </summary>
    [SugarColumn(ColumnName = "purchase_order_line_number", ColumnDescription = "来源采购订单行号", ColumnDataType = "int", IsNullable = true)]
    public int? PurchaseOrderLineNumber { get; set; }

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "material_code", ColumnDescription = "物料编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = true)]
    public string? MaterialCode { get; set; }

    /// <summary>
    /// 物料名称（回填：随物料）
    /// </summary>
    [SugarColumn(ColumnName = "material_name", ColumnDescription = "物料名称", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格（回填：随物料）
    /// </summary>
    [SugarColumn(ColumnName = "material_specification", ColumnDescription = "物料规格", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? MaterialSpecification { get; set; }

    /// <summary>
    /// 采购单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_unit", ColumnDescription = "采购单位", ColumnDataType = "nvarchar", Length = 20, IsNullable = false, DefaultValue = "PC")]
    public string PurchaseUnit { get; set; } = "PC";

    /// <summary>
    /// 开票数量（基本单位数量）
    /// </summary>
    [SugarColumn(ColumnName = "invoice_quantity", ColumnDescription = "开票数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 5, IsNullable = false, DefaultValue = "0")]
    public decimal InvoiceQuantity { get; set; } = 0;

    /// <summary>
    /// 单价
    /// </summary>
    [SugarColumn(ColumnName = "unit_price", ColumnDescription = "单价", ColumnDataType = "decimal", Length = 18, DecimalDigits = 5, IsNullable = false, DefaultValue = "0")]
    public decimal UnitPrice { get; set; } = 0;

    /// <summary>
    /// 折扣率（字典 logistics_discount_rate_param 预设或手输；0-100，表示折扣百分比）
    /// </summary>
    [SugarColumn(ColumnName = "discount_rate", ColumnDescription = "折扣率", ColumnDataType = "decimal", Length = 5, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal DiscountRate { get; set; } = 0;

    /// <summary>
    /// 折扣金额
    /// </summary>
    [SugarColumn(ColumnName = "discount_amount", ColumnDescription = "折扣金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 5, IsNullable = false, DefaultValue = "0")]
    public decimal DiscountAmount { get; set; } = 0;

    /// <summary>
    /// 税费率（字典 accounting_tax_rate_param 预设或手输；0-100，表示税费百分比）
    /// </summary>
    [SugarColumn(ColumnName = "tax_rate", ColumnDescription = "税费率", ColumnDataType = "decimal", Length = 5, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal TaxRate { get; set; } = 0;

    /// <summary>
    /// 税费
    /// </summary>
    [SugarColumn(ColumnName = "tax_amount", ColumnDescription = "税费", ColumnDataType = "decimal", Length = 18, DecimalDigits = 5, IsNullable = false, DefaultValue = "0")]
    public decimal TaxAmount { get; set; } = 0;

    /// <summary>
    /// 小计金额
    /// </summary>
    [SugarColumn(ColumnName = "subtotal_amount", ColumnDescription = "小计金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 5, IsNullable = false, DefaultValue = "0")]
    public decimal SubtotalAmount { get; set; } = 0;

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    [SugarColumn(ColumnName = "is_obsolete", ColumnDescription = "是否作废", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsObsolete { get; set; } = 0;

}
