// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Procurement
// 文件名称：TaktPurchaseInvoiceDtos.cs
// 创建时间：2026-07-23
// 创建人：Takt365(Auto Generated)
// 功能描述：PurchaseInvoice 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktPurchaseInvoice 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Procurement;

// ========================================
// PurchaseInvoice 响应 DTO
// ========================================

/// <summary>
/// Takt采购发票实体
/// 对应前端 TaktPurchaseInvoiceDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktPurchaseInvoiceDto : TaktCompanyDtoBase
{
    /// <summary>
    /// PurchaseInvoiceID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseInvoiceId { get; set; }

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 采购发票编码（唯一索引）
    /// </summary>
    public string PurchaseInvoiceCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联采购订单编码（选项 TaktPurchaseOrders/options；DictValue=PurchaseOrderCode）
    /// </summary>
    public string? PurchaseOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）
    /// </summary>
    public string SupplierName1 { get; set; } = string.Empty;

    /// <summary>
    /// 开票日期
    /// </summary>
    public DateTime InvoiceDate { get; set; }

    /// <summary>
    /// 发票总金额
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// 结算币种（字典 accounting_currency_code；DictValue=CNY/USD 等；一单一币种）
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 税率（字典 accounting_tax_rate_param；13=13%，9=9%，0=0% 等；一单一税率）
    /// </summary>
    public int TaxRate { get; set; } = 0;

    /// <summary>
    /// 税费
    /// </summary>
    public decimal TaxAmount { get; set; }

    /// <summary>
    /// 发票应付金额
    /// </summary>
    public decimal ActualAmount { get; set; }

    /// <summary>
    /// 已付款金额
    /// </summary>
    public decimal PaidAmount { get; set; }

    /// <summary>
    /// 付款方式（字典 accounting_payment_method_type；0=现金，1=银行转账，2=支票，3=信用证，4=其他）
    /// </summary>
    public int PaymentMethod { get; set; } = 0;

    /// <summary>
    /// 税务发票号码
    /// </summary>
    public string? TaxInvoiceNo { get; set; } = string.Empty;

    /// <summary>
    /// 发票状态（字典 logistics_invoice_status；0=草稿，1=已开票，2=已收款，3=已作废）
    /// </summary>
    public int InvoiceStatus { get; set; } = 0;

    /// <summary>
    /// 采购发票明细列表（主子表关系，一张发票可有多个明细行）
    /// （子表：TaktPurchaseInvoiceItem）
    /// </summary>
    public List<TaktPurchaseInvoiceItemDto>? Items { get; set; }

}

// ========================================
// PurchaseInvoice 查询 DTO
// ========================================

/// <summary>
/// PurchaseInvoice 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktPurchaseInvoiceQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司代码
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 采购发票编码（唯一索引）
    /// </summary>
    public string? PurchaseInvoiceCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联采购订单编码（选项 TaktPurchaseOrders/options；DictValue=PurchaseOrderCode）
    /// </summary>
    public string? PurchaseOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）
    /// </summary>
    public string? SupplierName1 { get; set; } = string.Empty;

    /// <summary>
    /// 开票日期（范围查询-开始）
    /// </summary>
    public DateTime? InvoiceDateStart { get; set; }

    /// <summary>
    /// 开票日期（范围查询-结束）
    /// </summary>
    public DateTime? InvoiceDateEnd { get; set; }

    /// <summary>
    /// 发票总金额
    /// </summary>
    public decimal? TotalAmount { get; set; }

    /// <summary>
    /// 结算币种（字典 accounting_currency_code；DictValue=CNY/USD 等；一单一币种）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 税率（字典 accounting_tax_rate_param；13=13%，9=9%，0=0% 等；一单一税率）
    /// </summary>
    public int? TaxRate { get; set; }

    /// <summary>
    /// 税费
    /// </summary>
    public decimal? TaxAmount { get; set; }

    /// <summary>
    /// 发票应付金额
    /// </summary>
    public decimal? ActualAmount { get; set; }

    /// <summary>
    /// 已付款金额
    /// </summary>
    public decimal? PaidAmount { get; set; }

    /// <summary>
    /// 付款方式（字典 accounting_payment_method_type；0=现金，1=银行转账，2=支票，3=信用证，4=其他）
    /// </summary>
    public int? PaymentMethod { get; set; }

    /// <summary>
    /// 税务发票号码
    /// </summary>
    public string? TaxInvoiceNo { get; set; } = string.Empty;

    /// <summary>
    /// 发票状态（字典 logistics_invoice_status；0=草稿，1=已开票，2=已收款，3=已作废）
    /// </summary>
    public int? InvoiceStatus { get; set; }

    /// <summary>
    /// 创建时间（范围查询-开始）
    /// </summary>
    public DateTime? CreatedAtStart { get; set; }

    /// <summary>
    /// 创建时间（范围查询-结束）
    /// </summary>
    public DateTime? CreatedAtEnd { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注（模糊查询）
    /// </summary>
    public string? Remark { get; set; }
}

// ========================================
// 创建PurchaseInvoice DTO
// ========================================

/// <summary>
/// 创建PurchaseInvoice DTO
/// </summary>
public class TaktPurchaseInvoiceCreateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
    /// </summary>
    public string CompanyDefaultCulture { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    [Required(ErrorMessage = "工厂代码（选项 TaktPlants/options；DictValue=PlantCode）不能为空")]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 采购发票编码（唯一索引）
    /// </summary>
    [Required(ErrorMessage = "采购发票编码（唯一索引）不能为空")]
    public string PurchaseInvoiceCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联采购订单编码（选项 TaktPurchaseOrders/options；DictValue=PurchaseOrderCode）
    /// </summary>
    public string? PurchaseOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    [Required(ErrorMessage = "供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）不能为空")]
    public string SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）
    /// </summary>
    [Required(ErrorMessage = "供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）不能为空")]
    public string SupplierName1 { get; set; } = string.Empty;

    /// <summary>
    /// 开票日期
    /// </summary>
    public DateTime InvoiceDate { get; set; }

    /// <summary>
    /// 发票总金额
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// 结算币种（字典 accounting_currency_code；DictValue=CNY/USD 等；一单一币种）
    /// </summary>
    [Required(ErrorMessage = "结算币种（字典 accounting_currency_code；DictValue=CNY/USD 等；一单一币种）不能为空")]
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 税率（字典 accounting_tax_rate_param；13=13%，9=9%，0=0% 等；一单一税率）
    /// </summary>
    public int TaxRate { get; set; } = 0;

    /// <summary>
    /// 税费
    /// </summary>
    public decimal TaxAmount { get; set; }

    /// <summary>
    /// 发票应付金额
    /// </summary>
    public decimal ActualAmount { get; set; }

    /// <summary>
    /// 已付款金额
    /// </summary>
    public decimal PaidAmount { get; set; }

    /// <summary>
    /// 付款方式（字典 accounting_payment_method_type；0=现金，1=银行转账，2=支票，3=信用证，4=其他）
    /// </summary>
    public int PaymentMethod { get; set; } = 0;

    /// <summary>
    /// 税务发票号码
    /// </summary>
    public string? TaxInvoiceNo { get; set; } = string.Empty;

    /// <summary>
    /// 发票状态（字典 logistics_invoice_status；0=草稿，1=已开票，2=已收款，3=已作废）
    /// </summary>
    public int InvoiceStatus { get; set; } = 0;

    /// <summary>
    /// 采购发票明细列表（主子表关系，一张发票可有多个明细行）（子表，级联保存）
    /// </summary>
    public List<TaktPurchaseInvoiceItemCreateDto>? Items { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 更新PurchaseInvoice DTO
// ========================================

/// <summary>
/// 更新PurchaseInvoice DTO
/// 继承 TaktPurchaseInvoiceCreateDto，添加 PurchaseInvoiceId 字段
/// </summary>
public class TaktPurchaseInvoiceUpdateDto : TaktPurchaseInvoiceCreateDto
{
    /// <summary>
    /// PurchaseInvoiceID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseInvoiceId { get; set; }

    /// <summary>
    /// 采购发票明细列表（主子表关系，一张发票可有多个明细行）（子表，级联保存）
    /// </summary>
    public new List<TaktPurchaseInvoiceItemUpdateDto>? Items { get; set; }

}

// ========================================
// PurchaseInvoice 状态 DTO
// ========================================

/// <summary>
/// PurchaseInvoice 状态更新 DTO
/// </summary>
public class TaktPurchaseInvoiceStatusDto
{
    /// <summary>
    /// PurchaseInvoiceID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseInvoiceId { get; set; }

    /// <summary>
    /// 发票状态（字典 logistics_invoice_status；0=草稿，1=已开票，2=已收款，3=已作废）
    /// </summary>
    [Required(ErrorMessage = "发票状态（字典 logistics_invoice_status；0=草稿，1=已开票，2=已收款，3=已作废）不能为空")]
    public int InvoiceStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// PurchaseInvoice 导入模板行 DTO
/// </summary>
public class TaktPurchaseInvoiceTemplateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 采购发票编码（唯一索引）
    /// </summary>
    public string? PurchaseInvoiceCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联采购订单编码（选项 TaktPurchaseOrders/options；DictValue=PurchaseOrderCode）
    /// </summary>
    public string? PurchaseOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）
    /// </summary>
    public string? SupplierName1 { get; set; } = string.Empty;

    /// <summary>
    /// 开票日期
    /// </summary>
    public DateTime? InvoiceDate { get; set; }

    /// <summary>
    /// 发票总金额
    /// </summary>
    public decimal? TotalAmount { get; set; }

    /// <summary>
    /// 结算币种（字典 accounting_currency_code；DictValue=CNY/USD 等；一单一币种）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 税率（字典 accounting_tax_rate_param；13=13%，9=9%，0=0% 等；一单一税率）
    /// </summary>
    public int? TaxRate { get; set; }

    /// <summary>
    /// 税费
    /// </summary>
    public decimal? TaxAmount { get; set; }

    /// <summary>
    /// 发票应付金额
    /// </summary>
    public decimal? ActualAmount { get; set; }

    /// <summary>
    /// 已付款金额
    /// </summary>
    public decimal? PaidAmount { get; set; }

    /// <summary>
    /// 付款方式（字典 accounting_payment_method_type；0=现金，1=银行转账，2=支票，3=信用证，4=其他）
    /// </summary>
    public int? PaymentMethod { get; set; }

    /// <summary>
    /// 税务发票号码
    /// </summary>
    public string? TaxInvoiceNo { get; set; } = string.Empty;

    /// <summary>
    /// 发票状态（字典 logistics_invoice_status；0=草稿，1=已开票，2=已收款，3=已作废）
    /// </summary>
    public int? InvoiceStatus { get; set; }

    /// <summary>
    /// 采购发票明细列表（主子表关系，一张发票可有多个明细行）（子表，级联保存）
    /// </summary>
    public List<TaktPurchaseInvoiceItemCreateDto>? Items { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

/// <summary>
/// PurchaseInvoice 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktPurchaseInvoiceImportDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
    /// </summary>
    public string? CompanyDefaultCulture { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 采购发票编码（唯一索引）
    /// </summary>
    public string? PurchaseInvoiceCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联采购订单编码（选项 TaktPurchaseOrders/options；DictValue=PurchaseOrderCode）
    /// </summary>
    public string? PurchaseOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）
    /// </summary>
    public string? SupplierName1 { get; set; } = string.Empty;

    /// <summary>
    /// 开票日期
    /// </summary>
    public DateTime? InvoiceDate { get; set; }

    /// <summary>
    /// 发票总金额
    /// </summary>
    public decimal? TotalAmount { get; set; }

    /// <summary>
    /// 结算币种（字典 accounting_currency_code；DictValue=CNY/USD 等；一单一币种）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 税率（字典 accounting_tax_rate_param；13=13%，9=9%，0=0% 等；一单一税率）
    /// </summary>
    public int? TaxRate { get; set; }

    /// <summary>
    /// 税费
    /// </summary>
    public decimal? TaxAmount { get; set; }

    /// <summary>
    /// 发票应付金额
    /// </summary>
    public decimal? ActualAmount { get; set; }

    /// <summary>
    /// 已付款金额
    /// </summary>
    public decimal? PaidAmount { get; set; }

    /// <summary>
    /// 付款方式（字典 accounting_payment_method_type；0=现金，1=银行转账，2=支票，3=信用证，4=其他）
    /// </summary>
    public int? PaymentMethod { get; set; }

    /// <summary>
    /// 税务发票号码
    /// </summary>
    public string? TaxInvoiceNo { get; set; } = string.Empty;

    /// <summary>
    /// 发票状态（字典 logistics_invoice_status；0=草稿，1=已开票，2=已收款，3=已作废）
    /// </summary>
    public int? InvoiceStatus { get; set; }

    /// <summary>
    /// 采购发票明细列表（主子表关系，一张发票可有多个明细行）（子表，级联保存）
    /// </summary>
    public List<TaktPurchaseInvoiceItemCreateDto>? Items { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 导出 DTO
// ========================================

/// <summary>
/// PurchaseInvoice 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktPurchaseInvoiceExportDto
{
    /// <summary>
    /// PurchaseInvoiceID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseInvoiceId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 采购发票编码（唯一索引）
    /// </summary>
    public string PurchaseInvoiceCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联采购订单编码（选项 TaktPurchaseOrders/options；DictValue=PurchaseOrderCode）
    /// </summary>
    public string? PurchaseOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）
    /// </summary>
    public string SupplierName1 { get; set; } = string.Empty;

    /// <summary>
    /// 开票日期
    /// </summary>
    public DateTime InvoiceDate { get; set; }

    /// <summary>
    /// 发票总金额
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// 结算币种（字典 accounting_currency_code；DictValue=CNY/USD 等；一单一币种）
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 税率（字典 accounting_tax_rate_param；13=13%，9=9%，0=0% 等；一单一税率）
    /// </summary>
    public int TaxRate { get; set; } = 0;

    /// <summary>
    /// 税费
    /// </summary>
    public decimal TaxAmount { get; set; }

    /// <summary>
    /// 发票应付金额
    /// </summary>
    public decimal ActualAmount { get; set; }

    /// <summary>
    /// 已付款金额
    /// </summary>
    public decimal PaidAmount { get; set; }

    /// <summary>
    /// 付款方式（字典 accounting_payment_method_type；0=现金，1=银行转账，2=支票，3=信用证，4=其他）
    /// </summary>
    public int PaymentMethod { get; set; } = 0;

    /// <summary>
    /// 税务发票号码
    /// </summary>
    public string? TaxInvoiceNo { get; set; } = string.Empty;

    /// <summary>
    /// 发票状态（字典 logistics_invoice_status；0=草稿，1=已开票，2=已收款，3=已作废）
    /// </summary>
    public int InvoiceStatus { get; set; } = 0;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
