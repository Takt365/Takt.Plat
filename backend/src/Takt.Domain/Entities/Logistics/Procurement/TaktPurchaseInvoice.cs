// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Logistics.Procurement
// 文件名称：TaktPurchaseInvoice.cs
// 创建时间：2026-06-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt采购发票实体，定义采购发票领域模型
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Procurement;

/// <summary>
/// Takt采购发票实体
/// </summary>
[SugarTable("takt_logistics_procurement_purchase_invoice", "采购发票表")]
[SugarIndex("ix_purchase_invoice_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_purchase_invoice_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_procurement_purchase_invoice_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(PurchaseInvoiceCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_procurement_purchase_invoice_supplier_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(SupplierCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_procurement_purchase_invoice_invoice_date", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(InvoiceDate), OrderByType.Desc, false)]
[SugarIndex("ix_takt_logistics_procurement_purchase_invoice_invoice_status", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(InvoiceStatus), OrderByType.Asc, false)]
public class TaktPurchaseInvoice : TaktCompanyEntityBase
{
    /// <summary>
    /// 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "plant_code", ColumnDescription = "工厂代码", ColumnDataType = "nvarchar", Length = 4, IsNullable = false)]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 采购发票编码（唯一索引）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_invoice_code", ColumnDescription = "采购发票编码", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string PurchaseInvoiceCode { get; set; } = string.Empty;
    /// <summary>
    /// 关联采购订单编码（选项 TaktPurchaseOrders/options，DictValue=PurchaseOrderCode）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_order_code", ColumnDescription = "采购订单编码", ColumnDataType = "nvarchar", Length = 10, IsNullable = true)]
    public string? PurchaseOrderCode { get; set; }
    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options，DictValue=SupplierCode）
    /// </summary>
    [SugarColumn(ColumnName = "supplier_code", ColumnDescription = "供应商编码", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string SupplierCode { get; set; } = string.Empty;
    /// <summary>
    /// 供应商名称
    /// </summary>
    [SugarColumn(ColumnName = "supplier_name", ColumnDescription = "供应商名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string SupplierName { get; set; } = string.Empty;
    /// <summary>
    /// 开票日期
    /// </summary>
    [SugarColumn(ColumnName = "invoice_date", ColumnDescription = "开票日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime InvoiceDate { get; set; } = DateTime.Now;
    /// <summary>
    /// 发票总金额
    /// </summary>
    [SugarColumn(ColumnName = "total_amount", ColumnDescription = "发票总金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal TotalAmount { get; set; } = 0;
    /// <summary>
    /// 税费
    /// </summary>
    [SugarColumn(ColumnName = "tax_amount", ColumnDescription = "税费", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal TaxAmount { get; set; } = 0;
    /// <summary>
    /// 发票应付金额
    /// </summary>
    [SugarColumn(ColumnName = "actual_amount", ColumnDescription = "发票应付金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal ActualAmount { get; set; } = 0;
    /// <summary>
    /// 已付款金额
    /// </summary>
    [SugarColumn(ColumnName = "paid_amount", ColumnDescription = "已付款金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal PaidAmount { get; set; } = 0;
    /// <summary>
    /// 付款方式（字典 accounting_payment_method_type；0=现金，1=银行转账，2=支票，3=信用证，4=其他）
    /// </summary>
    [SugarColumn(ColumnName = "payment_method", ColumnDescription = "付款方式", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int PaymentMethod { get; set; } = 0;
    /// <summary>
    /// 税务发票号码
    /// </summary>
    [SugarColumn(ColumnName = "tax_invoice_no", ColumnDescription = "税务发票号码", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? TaxInvoiceNo { get; set; }
    /// <summary>
    /// 发票状态（字典 logistics_invoice_status；0=草稿，1=已开票，2=已收款，3=已作废）
    /// </summary>
    [SugarColumn(ColumnName = "invoice_status", ColumnDescription = "发票状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int InvoiceStatus { get; set; } = 0;

    /// <summary>
    /// 采购发票明细列表（主子表关系，一张发票可有多个明细行）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktPurchaseInvoiceItem.PurchaseInvoiceId))]
    public List<TaktPurchaseInvoiceItem>? Items { get; set; }
}
