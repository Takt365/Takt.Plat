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
[SugarIndex("ix_takt_logistics_sales_quotation_plant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_sales_quotation_customer_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(CustomerCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_sales_quotation_quotation_date", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(QuotationDate), OrderByType.Desc, false)]
[SugarIndex("ix_takt_logistics_sales_quotation_quotation_status", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(QuotationStatus), OrderByType.Asc, false)]
public class TaktSalesQuotation : TaktCompanyEntityBase
{
    /// <summary>
    /// 销售报价编码（唯一索引）
    /// </summary>
    [SugarColumn(ColumnName = "sales_quotation_code", ColumnDescription = "销售报价编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string SalesQuotationCode { get; set; } = string.Empty;
    /// <summary>
    /// 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    [SugarColumn(ColumnName = "customer_code", ColumnDescription = "客户编码", ColumnDataType = "nvarchar", Length = 10, IsNullable = false)]
    public string CustomerCode { get; set; } = string.Empty;
    /// <summary>
    /// 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
    /// </summary>
    [SugarColumn(ColumnName = "customer_name1", ColumnDescription = "客户名称1", ColumnDataType = "nvarchar", Length = 140, IsNullable = false)]
    public string CustomerName1 { get; set; } = string.Empty;
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
    /// 销售员（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "sales_employee_id", ColumnDescription = "销售员ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesEmployeeId { get; set; }
    /// <summary>
    /// 销售员名称（冗余：按 SalesEmployeeId 取 TaktEmployee.EmployeeName 联动）
    /// </summary>
    [SugarColumn(ColumnName = "sales_employee_name", ColumnDescription = "销售员名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? SalesEmployeeName { get; set; }
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
    /// 结算币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；一单一币种）
    /// </summary>
    [SugarColumn(ColumnName = "currency_code", ColumnDescription = "结算币种", ColumnDataType = "nvarchar", Length = 3, IsNullable = false, DefaultValue = "CNY")]
    public string CurrencyCode { get; set; } = "CNY";
    /// <summary>
    /// 税码（字典 accounting_financial_tax_code；按 CultureCode 匹配 TaktDictData.CultureCode；DictValue 随区域变化）
    /// </summary>
    [SugarColumn(ColumnName = "tax_code", ColumnDescription = "税码", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? TaxCode { get; set; }
    /// <summary>
    /// 税率（百分比整数；一单一税率；由税码 TaxCode / 字典 accounting_financial_tax_code.ExtValue 回填，如 J2→13）
    /// </summary>
    [SugarColumn(ColumnName = "tax_rate", ColumnDescription = "税率", ColumnDataType = "int", IsNullable = false, DefaultValue = "13")]
    public int TaxRate { get; set; } = 13;
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
    /// 关联销售订单编码（报价转订单后回填；选项 TaktSalesOrders/options，DictValue=SalesOrderCode）
    /// </summary>
    [SugarColumn(ColumnName = "sales_order_code", ColumnDescription = "销售订单编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = true)]
    public string? SalesOrderCode { get; set; }
    /// <summary>
    /// 报价状态（字典 logistics_sales_quotation_status；0=草稿 1=已发送 2=已接受 3=已拒绝 4=已过期 5=已作废）
    /// </summary>
    [SugarColumn(ColumnName = "quotation_status", ColumnDescription = "报价状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int QuotationStatus { get; set; } = 0;

    // ========================================
    // 导航属性区域
    // ========================================

    /// <summary>
    /// 销售报价明细列表（主子表关系）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktSalesQuotationItem.SalesQuotationId))]
    public List<TaktSalesQuotationItem>? Items { get; set; }
}
