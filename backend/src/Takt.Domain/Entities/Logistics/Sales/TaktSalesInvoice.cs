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
[SugarIndex("ix_takt_logistics_sales_invoice_accounting_doc_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(AccountingDocumentCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_sales_invoice_plant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_sales_invoice_year_month", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(YearMonth), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_sales_invoice_customer_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(CustomerCode), OrderByType.Asc, false)]
public class TaktSalesInvoice : TaktCompanyEntityBase
{
    /// <summary>
    /// 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "plant_code", ColumnDescription = "工厂代码", ColumnDataType = "nvarchar", Length = 4, IsNullable = false)]
    public string PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 年度期间（yyyyMM）
    /// </summary>
    [SugarColumn(ColumnName = "year_month", ColumnDescription = "年度期间", ColumnDataType = "varchar", Length = 6, IsNullable = false)]
    public string YearMonth { get; set; } = string.Empty;
    /// <summary>
    /// 客户编码（选项 TaktCustomers/options，DictValue=CustomerCode）
    /// </summary>
    [SugarColumn(ColumnName = "customer_code", ColumnDescription = "客户编码", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string CustomerCode { get; set; } = string.Empty;
    /// <summary>
    /// 客户名称
    /// </summary>
    [SugarColumn(ColumnName = "customer_name", ColumnDescription = "客户名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string CustomerName { get; set; } = string.Empty;
    /// <summary>
    /// 会计凭证编号（租户+公司+工厂内唯一）
    /// </summary>
    [SugarColumn(ColumnName = "accounting_document_code", ColumnDescription = "会计凭证编号", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string AccountingDocumentCode { get; set; } = string.Empty;

    // ========================================
    // 导航属性区域
    // ========================================

    /// <summary>
    /// 销售发票明细列表（主子表关系，一张发票可有多个明细行）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktSalesInvoiceItem.SalesInvoiceId))]
    public List<TaktSalesInvoiceItem>? Items { get; set; }
}
