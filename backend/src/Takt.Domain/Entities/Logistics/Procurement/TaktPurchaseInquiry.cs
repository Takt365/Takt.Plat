// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Logistics.Procurement
// 文件名称：TaktPurchaseInquiry.cs
// 创建时间：2026-06-24
// 创建人：Takt365(Cursor AI)
// 功能描述：采购询价实体（主子表主表，含询价明细 Items）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Procurement;

/// <summary>
/// 采购询价实体
/// </summary>
[SugarTable("takt_logistics_materials_purchase_inquiry", "采购询价表")]
[SugarIndex("ix_purchase_inquiry_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_purchase_inquiry_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_materials_purchase_inquiry_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(PurchaseInquiryCode), OrderByType.Asc, nameof(InquiryDate), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_materials_purchase_inquiry_inquiry_date", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(InquiryDate), OrderByType.Desc, false)]
[SugarIndex("ix_takt_logistics_materials_purchase_inquiry_inquiry_by", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(InquiryBy), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_materials_purchase_inquiry_supplier_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(SupplierCode), OrderByType.Asc, false)]
public class TaktPurchaseInquiry : TaktCompanyEntityBase
{
    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "plant_code", ColumnDescription = "工厂代码", ColumnDataType = "nvarchar", Length = 4, IsNullable = false)]
    public string PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 采购询价编码（租户+公司+工厂内业务唯一）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_inquiry_code", ColumnDescription = "采购询价编码", ColumnDataType = "varchar", Length = 40, IsNullable = false)]
    public string PurchaseInquiryCode { get; set; } = string.Empty;
    /// <summary>
    /// 询价日期
    /// </summary>
    [SugarColumn(ColumnName = "inquiry_date", ColumnDescription = "询价日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime InquiryDate { get; set; } = DateTime.Now;
    /// <summary>
    /// 报价截止日期
    /// </summary>
    [SugarColumn(ColumnName = "quote_deadline_date", ColumnDescription = "报价截止日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? QuoteDeadlineDate { get; set; }
    /// <summary>
    /// 询价人员工 ID（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "inquiry_id", ColumnDescription = "询价人员工ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? InquiryId { get; set; }
    /// <summary>
    /// 询价人（人员代码）
    /// </summary>
    [SugarColumn(ColumnName = "inquiry_by", ColumnDescription = "询价人", ColumnDataType = "varchar", Length = 40, IsNullable = false)]
    public string InquiryBy { get; set; } = string.Empty;
    /// <summary>
    /// 询价供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode；一单一供应商，明细禁止再挂供应商）
    /// </summary>
    [SugarColumn(ColumnName = "supplier_code", ColumnDescription = "询价供应商编码", ColumnDataType = "varchar", Length = 40, IsNullable = false)]
    public string SupplierCode { get; set; } = string.Empty;
    /// <summary>
    /// 询价供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）
    /// </summary>
    [SugarColumn(ColumnName = "supplier_name1", ColumnDescription = "询价供应商名称1", ColumnDataType = "nvarchar", Length = 140, IsNullable = false)]
    public string SupplierName1 { get; set; } = string.Empty;
    /// <summary>
    /// 结算币种（字典 accounting_currency_code；DictValue=CNY/USD 等；一单一币种）
    /// </summary>
    [SugarColumn(ColumnName = "currency_code", ColumnDescription = "结算币种", ColumnDataType = "nvarchar", Length = 3, IsNullable = false, DefaultValue = "CNY")]
    public string CurrencyCode { get; set; } = "CNY";
    /// <summary>
    /// 税率（字典 accounting_tax_rate_param；13=13%，9=9%，0=0% 等；一单一税率）
    /// </summary>
    [SugarColumn(ColumnName = "tax_rate", ColumnDescription = "税率", ColumnDataType = "int", IsNullable = false, DefaultValue = "13")]
    public int TaxRate { get; set; } = 13;
    /// <summary>
    /// 税费
    /// </summary>
    [SugarColumn(ColumnName = "tax_amount", ColumnDescription = "税费", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal TaxAmount { get; set; } = 0;
    /// <summary>
    /// 付款方式（字典 logistics_payment_mode：vendorpay=供应商付款，employeereimburse=员工报销）
    /// </summary>
    [SugarColumn(ColumnName = "payment_mode", ColumnDescription = "付款方式", ColumnDataType = "varchar", Length = 40, IsNullable = false, DefaultValue = "vendorpay")]
    public string PaymentMode { get; set; } = "vendorpay";
    /// <summary>
    /// 采购链路方案（字典 logistics_procurement_chain_scheme；1=方案一含报销，2=方案二仅 PO）
    /// </summary>
    [SugarColumn(ColumnName = "chain_scheme", ColumnDescription = "采购链路方案", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int ChainScheme { get; set; } = 1;
    /// <summary>
    /// 询价总数量（基本单位数量）
    /// </summary>
    [SugarColumn(ColumnName = "total_quantity", ColumnDescription = "询价总数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal TotalQuantity { get; set; }
    /// <summary>
    /// 询价总金额
    /// </summary>
    [SugarColumn(ColumnName = "total_amount", ColumnDescription = "询价总金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal TotalAmount { get; set; }
    /// <summary>
    /// 已转价格数量（基本单位数量）
    /// </summary>
    [SugarColumn(ColumnName = "converted_quantity", ColumnDescription = "已转价格数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal ConvertedQuantity { get; set; }
    /// <summary>
    /// 已转价格金额
    /// </summary>
    [SugarColumn(ColumnName = "converted_amount", ColumnDescription = "已转价格金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal ConvertedAmount { get; set; }
    /// <summary>
    /// 询价原因
    /// </summary>
    [SugarColumn(ColumnName = "inquiry_reason", ColumnDescription = "询价原因", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? InquiryReason { get; set; }
    /// <summary>
    /// 询价状态（字典 sys_normal_disable_status；1=启用，0=禁用）
    /// </summary>
    [SugarColumn(ColumnName = "inquiry_status", ColumnDescription = "询价状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int InquiryStatus { get; set; } = 1;
    /// <summary>
    /// 转价格状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
    /// </summary>
    [SugarColumn(ColumnName = "converted_status", ColumnDescription = "转价格状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ConvertedStatus { get; set; }

    // ========================================
    // 导航属性区域
    // ========================================

    /// <summary>
    /// 采购询价明细列表（主子表关系）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktPurchaseInquiryItem.PurchaseInquiryId))]
    public List<TaktPurchaseInquiryItem>? Items { get; set; }
}
