// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Logistics.Sales
// 文件名称：TaktSalesQuotation.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt销售报价实体，定义销售报价领域模型
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Sales;

/// <summary>
/// Takt销售报价实体
/// </summary>
[SugarTable("takt_logistics_sales_quotation", "销售报价表")]
[SugarIndex("ix_sales_quotation_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_sales_quotation_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_sales_quotation_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(SalesQuotationCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_sales_quotation_customer_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(CustomerCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_sales_quotation_quotation_date", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(QuotationDate), OrderByType.Desc, false)]
[SugarIndex("ix_takt_logistics_sales_quotation_quotation_status", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(QuotationStatus), OrderByType.Asc, false)]
public class TaktSalesQuotation : TaktCompanyEntityBase
{
    /// <summary>
    /// 工厂代码
    /// </summary>
    [SugarColumn(ColumnName = "plant_code", ColumnDescription = "工厂代码", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 销售报价编码（唯一索引）
    /// </summary>
    [SugarColumn(ColumnName = "sales_quotation_code", ColumnDescription = "销售报价编码", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string SalesQuotationCode { get; set; } = string.Empty;

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
    /// 报价日期
    /// </summary>
    [SugarColumn(ColumnName = "quotation_date", ColumnDescription = "报价日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime QuotationDate { get; set; } = DateTime.Now;

    /// <summary>
    /// 报价有效期至
    /// </summary>
    [SugarColumn(ColumnName = "valid_until_date", ColumnDescription = "报价有效期至", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? ValidUntilDate { get; set; }

    /// <summary>
    /// 销售员（人员代码）
    /// </summary>
    [SugarColumn(ColumnName = "sales_by", ColumnDescription = "销售员", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? SalesBy { get; set; }

    /// <summary>
    /// 报价总数量（基本单位数量）
    /// </summary>
    [SugarColumn(ColumnName = "total_quantity", ColumnDescription = "报价总数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal TotalQuantity { get; set; } = 0;

    /// <summary>
    /// 报价总金额
    /// </summary>
    [SugarColumn(ColumnName = "total_amount", ColumnDescription = "报价总金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal TotalAmount { get; set; } = 0;

    /// <summary>
    /// 折扣金额
    /// </summary>
    [SugarColumn(ColumnName = "discount_amount", ColumnDescription = "折扣金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal DiscountAmount { get; set; } = 0;

    /// <summary>
    /// 税费
    /// </summary>
    [SugarColumn(ColumnName = "tax_amount", ColumnDescription = "税费", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal TaxAmount { get; set; } = 0;

    /// <summary>
    /// 报价实付金额
    /// </summary>
    [SugarColumn(ColumnName = "actual_amount", ColumnDescription = "报价实付金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal ActualAmount { get; set; } = 0;

    /// <summary>
    /// 报价状态（字典 logistics_quotation_status；0=草稿，1=已发送，2=已接受，3=已拒绝，4=已过期，5=已作废）
    /// </summary>
    [SugarColumn(ColumnName = "quotation_status", ColumnDescription = "报价状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int QuotationStatus { get; set; } = 0;

    /// <summary>
    /// 关联销售订单编码（报价转订单后回填）
    /// </summary>
    [SugarColumn(ColumnName = "sales_order_code", ColumnDescription = "销售订单编码", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? SalesOrderCode { get; set; }


    /// <summary>
    /// 销售报价明细列表（主子表关系）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktSalesQuotationItem.SalesQuotationId))]
    public List<TaktSalesQuotationItem>? Items { get; set; }

    /// <summary>
    /// 销售报价变更记录列表（外键在子表 TaktSalesQuotationChangeLog.SalesQuotationId）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktSalesQuotationChangeLog.SalesQuotationId))]
    public List<TaktSalesQuotationChangeLog>? ChangeLogs { get; set; }
}
