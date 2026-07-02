// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Procurement
// 文件名称：TaktPurchaseInvoiceItemDtos.cs
// 创建时间：2026-06-30
// 创建人：Takt365(Auto Generated)
// 功能描述：PurchaseInvoiceItem 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktPurchaseInvoiceItem 生成，请按需审阅）
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
// PurchaseInvoiceItem 响应 DTO
// ========================================

/// <summary>
/// Takt采购发票明细实体
/// 对应前端 TaktPurchaseInvoiceItemDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktPurchaseInvoiceItemDto : TaktCompanyDtoBase
{
    /// <summary>
    /// PurchaseInvoiceItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseInvoiceItemId { get; set; }

    /// <summary>
    /// 采购发票 ID（关联 TaktPurchaseInvoice.Id，选项 TaktPurchaseInvoices/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseInvoiceId { get; set; }

    /// <summary>
    /// 采购发票 名称（填充字段）
    /// </summary>
    public string? PurchaseInvoiceName { get; set; }

    /// <summary>
    /// 采购发票编码（冗余字段，便于查询）
    /// </summary>
    public string PurchaseInvoiceCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 来源采购订单编码
    /// </summary>
    public string? PurchaseOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源采购订单行号
    /// </summary>
    public int? PurchaseOrderLineNumber { get; set; }

    /// <summary>
    /// 物料编码
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称
    /// </summary>
    public string MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格
    /// </summary>
    public string? MaterialSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 采购单位
    /// </summary>
    public string PurchaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 开票数量（基本单位数量）
    /// </summary>
    public decimal InvoiceQuantity { get; set; }

    /// <summary>
    /// 单价
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// 折扣率（字典 logistics_discount_rate_param 预设或手输；0-100，表示折扣百分比）
    /// </summary>
    public decimal DiscountRate { get; set; }

    /// <summary>
    /// 折扣金额
    /// </summary>
    public decimal DiscountAmount { get; set; }

    /// <summary>
    /// 税费率（字典 accounting_tax_rate_param 预设或手输；0-100，表示税费百分比）
    /// </summary>
    public decimal TaxRate { get; set; }

    /// <summary>
    /// 税费
    /// </summary>
    public decimal TaxAmount { get; set; }

    /// <summary>
    /// 小计金额
    /// </summary>
    public decimal SubtotalAmount { get; set; }

}

// ========================================
// PurchaseInvoiceItem 查询 DTO
// ========================================

/// <summary>
/// PurchaseInvoiceItem 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktPurchaseInvoiceItemQueryDto : TaktPagedQuery
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
    /// 采购发票 ID（关联 TaktPurchaseInvoice.Id，选项 TaktPurchaseInvoices/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PurchaseInvoiceId { get; set; }

    /// <summary>
    /// 采购发票编码（冗余字段，便于查询）
    /// </summary>
    public string? PurchaseInvoiceCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 来源采购订单编码
    /// </summary>
    public string? PurchaseOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源采购订单行号
    /// </summary>
    public int? PurchaseOrderLineNumber { get; set; }

    /// <summary>
    /// 物料编码
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称
    /// </summary>
    public string? MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格
    /// </summary>
    public string? MaterialSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 采购单位
    /// </summary>
    public string? PurchaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 开票数量（基本单位数量）
    /// </summary>
    public decimal? InvoiceQuantity { get; set; }

    /// <summary>
    /// 单价
    /// </summary>
    public decimal? UnitPrice { get; set; }

    /// <summary>
    /// 折扣率（字典 logistics_discount_rate_param 预设或手输；0-100，表示折扣百分比）
    /// </summary>
    public decimal? DiscountRate { get; set; }

    /// <summary>
    /// 折扣金额
    /// </summary>
    public decimal? DiscountAmount { get; set; }

    /// <summary>
    /// 税费率（字典 accounting_tax_rate_param 预设或手输；0-100，表示税费百分比）
    /// </summary>
    public decimal? TaxRate { get; set; }

    /// <summary>
    /// 税费
    /// </summary>
    public decimal? TaxAmount { get; set; }

    /// <summary>
    /// 小计金额
    /// </summary>
    public decimal? SubtotalAmount { get; set; }

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
// 创建PurchaseInvoiceItem DTO
// ========================================

/// <summary>
/// 创建PurchaseInvoiceItem DTO
/// </summary>
public class TaktPurchaseInvoiceItemCreateDto
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
    /// 采购发票 ID（关联 TaktPurchaseInvoice.Id，选项 TaktPurchaseInvoices/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseInvoiceId { get; set; }

    /// <summary>
    /// 采购发票编码（冗余字段，便于查询）
    /// </summary>
    [Required(ErrorMessage = "采购发票编码（冗余字段，便于查询）不能为空")]
    public string PurchaseInvoiceCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 来源采购订单编码
    /// </summary>
    public string? PurchaseOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源采购订单行号
    /// </summary>
    public int? PurchaseOrderLineNumber { get; set; }

    /// <summary>
    /// 物料编码
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称
    /// </summary>
    [Required(ErrorMessage = "物料名称不能为空")]
    public string MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格
    /// </summary>
    public string? MaterialSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 采购单位
    /// </summary>
    [Required(ErrorMessage = "采购单位不能为空")]
    public string PurchaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 开票数量（基本单位数量）
    /// </summary>
    public decimal InvoiceQuantity { get; set; }

    /// <summary>
    /// 单价
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// 折扣率（字典 logistics_discount_rate_param 预设或手输；0-100，表示折扣百分比）
    /// </summary>
    public decimal DiscountRate { get; set; }

    /// <summary>
    /// 折扣金额
    /// </summary>
    public decimal DiscountAmount { get; set; }

    /// <summary>
    /// 税费率（字典 accounting_tax_rate_param 预设或手输；0-100，表示税费百分比）
    /// </summary>
    public decimal TaxRate { get; set; }

    /// <summary>
    /// 税费
    /// </summary>
    public decimal TaxAmount { get; set; }

    /// <summary>
    /// 小计金额
    /// </summary>
    public decimal SubtotalAmount { get; set; }

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
// 更新PurchaseInvoiceItem DTO
// ========================================

/// <summary>
/// 更新PurchaseInvoiceItem DTO
/// 继承 TaktPurchaseInvoiceItemCreateDto，添加 PurchaseInvoiceItemId 字段
/// </summary>
public class TaktPurchaseInvoiceItemUpdateDto : TaktPurchaseInvoiceItemCreateDto
{
    /// <summary>
    /// PurchaseInvoiceItemID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseInvoiceItemId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// PurchaseInvoiceItem 导入模板行 DTO
/// </summary>
public class TaktPurchaseInvoiceItemTemplateDto
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
    /// 采购发票 ID（关联 TaktPurchaseInvoice.Id，选项 TaktPurchaseInvoices/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PurchaseInvoiceId { get; set; }

    /// <summary>
    /// 采购发票编码（冗余字段，便于查询）
    /// </summary>
    public string? PurchaseInvoiceCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 来源采购订单编码
    /// </summary>
    public string? PurchaseOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源采购订单行号
    /// </summary>
    public int? PurchaseOrderLineNumber { get; set; }

    /// <summary>
    /// 物料编码
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称
    /// </summary>
    public string? MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格
    /// </summary>
    public string? MaterialSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 采购单位
    /// </summary>
    public string? PurchaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 开票数量（基本单位数量）
    /// </summary>
    public decimal? InvoiceQuantity { get; set; }

    /// <summary>
    /// 单价
    /// </summary>
    public decimal? UnitPrice { get; set; }

    /// <summary>
    /// 折扣率（字典 logistics_discount_rate_param 预设或手输；0-100，表示折扣百分比）
    /// </summary>
    public decimal? DiscountRate { get; set; }

    /// <summary>
    /// 折扣金额
    /// </summary>
    public decimal? DiscountAmount { get; set; }

    /// <summary>
    /// 税费率（字典 accounting_tax_rate_param 预设或手输；0-100，表示税费百分比）
    /// </summary>
    public decimal? TaxRate { get; set; }

    /// <summary>
    /// 税费
    /// </summary>
    public decimal? TaxAmount { get; set; }

    /// <summary>
    /// 小计金额
    /// </summary>
    public decimal? SubtotalAmount { get; set; }

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
/// PurchaseInvoiceItem 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktPurchaseInvoiceItemImportDto
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
    /// 采购发票 ID（关联 TaktPurchaseInvoice.Id，选项 TaktPurchaseInvoices/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PurchaseInvoiceId { get; set; }

    /// <summary>
    /// 采购发票编码（冗余字段，便于查询）
    /// </summary>
    public string? PurchaseInvoiceCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 来源采购订单编码
    /// </summary>
    public string? PurchaseOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源采购订单行号
    /// </summary>
    public int? PurchaseOrderLineNumber { get; set; }

    /// <summary>
    /// 物料编码
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称
    /// </summary>
    public string? MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格
    /// </summary>
    public string? MaterialSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 采购单位
    /// </summary>
    public string? PurchaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 开票数量（基本单位数量）
    /// </summary>
    public decimal? InvoiceQuantity { get; set; }

    /// <summary>
    /// 单价
    /// </summary>
    public decimal? UnitPrice { get; set; }

    /// <summary>
    /// 折扣率（字典 logistics_discount_rate_param 预设或手输；0-100，表示折扣百分比）
    /// </summary>
    public decimal? DiscountRate { get; set; }

    /// <summary>
    /// 折扣金额
    /// </summary>
    public decimal? DiscountAmount { get; set; }

    /// <summary>
    /// 税费率（字典 accounting_tax_rate_param 预设或手输；0-100，表示税费百分比）
    /// </summary>
    public decimal? TaxRate { get; set; }

    /// <summary>
    /// 税费
    /// </summary>
    public decimal? TaxAmount { get; set; }

    /// <summary>
    /// 小计金额
    /// </summary>
    public decimal? SubtotalAmount { get; set; }

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
/// PurchaseInvoiceItem 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktPurchaseInvoiceItemExportDto
{
    /// <summary>
    /// PurchaseInvoiceItemID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseInvoiceItemId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 采购发票 ID（关联 TaktPurchaseInvoice.Id，选项 TaktPurchaseInvoices/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseInvoiceId { get; set; }

    /// <summary>
    /// 采购发票编码（冗余字段，便于查询）
    /// </summary>
    public string PurchaseInvoiceCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 来源采购订单编码
    /// </summary>
    public string? PurchaseOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源采购订单行号
    /// </summary>
    public int? PurchaseOrderLineNumber { get; set; }

    /// <summary>
    /// 物料编码
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称
    /// </summary>
    public string MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格
    /// </summary>
    public string? MaterialSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 采购单位
    /// </summary>
    public string PurchaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 开票数量（基本单位数量）
    /// </summary>
    public decimal InvoiceQuantity { get; set; }

    /// <summary>
    /// 单价
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// 折扣率（字典 logistics_discount_rate_param 预设或手输；0-100，表示折扣百分比）
    /// </summary>
    public decimal DiscountRate { get; set; }

    /// <summary>
    /// 折扣金额
    /// </summary>
    public decimal DiscountAmount { get; set; }

    /// <summary>
    /// 税费率（字典 accounting_tax_rate_param 预设或手输；0-100，表示税费百分比）
    /// </summary>
    public decimal TaxRate { get; set; }

    /// <summary>
    /// 税费
    /// </summary>
    public decimal TaxAmount { get; set; }

    /// <summary>
    /// 小计金额
    /// </summary>
    public decimal SubtotalAmount { get; set; }

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
