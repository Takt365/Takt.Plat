// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Logistics.Sales
// 文件名称：TaktSalesInvoice.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt销售发票实体，定义销售发票领域模型
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Sales;

/// <summary>
/// Takt销售发票实体
/// </summary>
[SugarTable("takt_logistics_sales_invoice", "销售发票表")]
[SugarIndex("ix_sales_invoice_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_sales_invoice_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_sales_invoice_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(SalesInvoiceCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_sales_invoice_customer_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(CustomerCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_sales_invoice_invoice_date", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(InvoiceDate), OrderByType.Desc, false)]
[SugarIndex("ix_takt_logistics_sales_invoice_invoice_status", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(InvoiceStatus), OrderByType.Asc, false)]
public class TaktSalesInvoice : TaktCompanyEntityBase
{
    /// <summary>
    /// 工厂代码
    /// </summary>
    [SugarColumn(ColumnName = "plant_code", ColumnDescription = "工厂代码", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? PlantCode { get; set; }

    /// <summary>
    /// 销售发票编码（唯一索引）
    /// </summary>
    [SugarColumn(ColumnName = "sales_invoice_code", ColumnDescription = "销售发票编码", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string SalesInvoiceCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联销售订单编码
    /// </summary>
    [SugarColumn(ColumnName = "sales_order_code", ColumnDescription = "销售订单编码", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? SalesOrderCode { get; set; }

    /// <summary>
    /// 客户编码
    /// </summary>
    [SugarColumn(ColumnName = "customer_code", ColumnDescription = "客户编码", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户名称
    /// </summary>
    [SugarColumn(ColumnName = "customer_name", ColumnDescription = "客户名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string CustomerName { get; set; } = string.Empty;

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
    /// 发票实付金额
    /// </summary>
    [SugarColumn(ColumnName = "actual_amount", ColumnDescription = "发票实付金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal ActualAmount { get; set; } = 0;

    /// <summary>
    /// 发票状态（0=草稿，1=已开票，2=已收款，3=已作废）
    /// </summary>
    [SugarColumn(ColumnName = "invoice_status", ColumnDescription = "发票状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int InvoiceStatus { get; set; } = 0;

    /// <summary>
    /// 收款方式（0=现金，1=银行转账，2=支票，3=信用证，4=其他）
    /// </summary>
    [SugarColumn(ColumnName = "payment_method", ColumnDescription = "收款方式", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int PaymentMethod { get; set; } = 0;

    /// <summary>
    /// 发票号码（税务系统票号）
    /// </summary>
    [SugarColumn(ColumnName = "tax_invoice_no", ColumnDescription = "税务发票号码", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? TaxInvoiceNo { get; set; }

    /// <summary>
    /// 销售发票明细列表（主子表关系，一张发票可有多个明细行）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktSalesInvoiceItem.SalesInvoiceId))]
    public List<TaktSalesInvoiceItem>? Items { get; set; }
}
