// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Logistics.Sales
// 文件名称：TaktSalesInvoiceItem.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt销售发票明细实体，定义销售发票行项目领域模型
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Sales;

/// <summary>
/// Takt销售发票明细实体
/// </summary>
[SugarTable("takt_logistics_sales_invoice_item", "销售发票明细表")]
[SugarIndex("ix_sales_invoice_item_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_sales_invoice_item_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_sales_invoice_item_invoice_line_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(SalesInvoiceId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, true)]
public class TaktSalesInvoiceItem : TaktCompanyEntityBase
{
    /// <summary>
    /// 销售发票（关联 TaktSalesInvoice.Id，选项 TaktSalesInvoices/options）
    /// </summary>
    [SugarColumn(ColumnName = "sales_invoice_id", ColumnDescription = "销售发票ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesInvoiceId { get; set; }
    /// <summary>
    /// 会计凭证编号（冗余，与主表 AccountingDocumentCode 一致）
    /// </summary>
    [SugarColumn(ColumnName = "accounting_document_code", ColumnDescription = "会计凭证编号", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string AccountingDocumentCode { get; set; } = string.Empty;
    /// <summary>
    /// 行号（项目/序号，固定步长=10）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "项目", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int LineNumber { get; set; } = 0;
    /// <summary>
    /// 过帐日期
    /// </summary>
    [SugarColumn(ColumnName = "posting_date", ColumnDescription = "过帐日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime PostingDate { get; set; }
    /// <summary>
    /// 货币（字典 accounting_currency_code，DictValue=CNY/USD 等）
    /// </summary>
    [SugarColumn(ColumnName = "currency", ColumnDescription = "货币", ColumnDataType = "nvarchar", Length = 3, IsNullable = false, DefaultValue = "CNY")]
    public string Currency { get; set; } = "CNY";
    /// <summary>
    /// 机种名称
    /// </summary>
    [SugarColumn(ColumnName = "model_name", ColumnDescription = "机种名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? ModelName { get; set; }
    /// <summary>
    /// 物料编码（选项 TaktMaterials/options，DictValue=MaterialCode）
    /// </summary>
    [SugarColumn(ColumnName = "material_code", ColumnDescription = "物料编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string MaterialCode { get; set; } = string.Empty;
    /// <summary>
    /// 物料类型（字典 logistics_material_type，DictValue=ROH/HALB 等；默认 ROH）
    /// </summary>
    [SugarColumn(ColumnName = "material_type", ColumnDescription = "物料类型", ColumnDataType = "nvarchar", Length = 4, IsNullable = false, DefaultValue = "ROH")]
    public string MaterialType { get; set; } = "ROH";
    /// <summary>
    /// 物料名称
    /// </summary>
    [SugarColumn(ColumnName = "material_name", ColumnDescription = "物料名称", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string MaterialName { get; set; } = string.Empty;
    /// <summary>
    /// 利润中心（关联 TaktProfitCenter.ProfitCenterCode，选项 TaktProfitCenters/options，DictValue=ProfitCenterCode）
    /// </summary>
    [SugarColumn(ColumnName = "profit_center_code", ColumnDescription = "利润中心", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? ProfitCenterCode { get; set; }
    /// <summary>
    /// 会计科目（关联 TaktAccountTitle.AccountTitleCode，选项 TaktAccountTitles/options）
    /// </summary>
    [SugarColumn(ColumnName = "account_title", ColumnDescription = "会计科目", ColumnDataType = "varchar", Length = 40, IsNullable = true)]
    public string? AccountTitle { get; set; }
    /// <summary>
    /// 数量
    /// </summary>
    [SugarColumn(ColumnName = "quantity", ColumnDescription = "数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal Quantity { get; set; } = 0;
    /// <summary>
    /// 单位
    /// </summary>
    [SugarColumn(ColumnName = "unit", ColumnDescription = "单位", ColumnDataType = "nvarchar", Length = 20, IsNullable = false, DefaultValue = "PC")]
    public string Unit { get; set; } = "PC";
    /// <summary>
    /// 本位币金额
    /// </summary>
    [SugarColumn(ColumnName = "local_currency_amount", ColumnDescription = "本位币金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal LocalCurrencyAmount { get; set; } = 0;
    /// <summary>
    /// 业务货币计价的金额
    /// </summary>
    [SugarColumn(ColumnName = "transaction_currency_amount", ColumnDescription = "业务货币金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal TransactionCurrencyAmount { get; set; } = 0;
    /// <summary>
    /// 凭证类型（字典 logistics_accounting_document_type，DictValue=AA/AB/…）
    /// </summary>
    [SugarColumn(ColumnName = "document_type", ColumnDescription = "凭证类型", ColumnDataType = "varchar", Length = 2, IsNullable = false)]
    public string DocumentType { get; set; } = string.Empty;
    /// <summary>
    /// 参考凭证
    /// </summary>
    [SugarColumn(ColumnName = "reference_document_code", ColumnDescription = "参考凭证", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? ReferenceDocumentCode { get; set; }
    /// <summary>
    /// 参考凭证项目（行号）
    /// </summary>
    [SugarColumn(ColumnName = "reference_document_item", ColumnDescription = "参考凭证项目", ColumnDataType = "int", IsNullable = true)]
    public int? ReferenceDocumentItem { get; set; }

    // ========================================
    // 导航属性区域
    // ========================================

    /// <summary>
    /// 销售发票主表
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(SalesInvoiceId))]
    public TaktSalesInvoice? SalesInvoice { get; set; }
}
