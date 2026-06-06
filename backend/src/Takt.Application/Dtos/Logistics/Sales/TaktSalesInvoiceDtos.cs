// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Sales
// 文件名称：TaktSalesInvoiceDtos.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：SalesInvoice 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktSalesInvoice 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Sales;

// ========================================
// SalesInvoice 响应 DTO
// ========================================

/// <summary>
/// Takt销售发票实体
/// 对应前端 TaktSalesInvoiceDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktSalesInvoiceDto : TaktCompanyDtoBase
{
    /// <summary>
    /// SalesInvoiceID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesInvoiceId { get; set; }

    /// <summary>
    /// 工厂代码
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 销售发票编码（唯一索引）
    /// </summary>
    public string SalesInvoiceCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联销售订单编码
    /// </summary>
    public string? SalesOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码
    /// </summary>
    public string CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户名称
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// 开票日期
    /// </summary>
    public DateTime InvoiceDate { get; set; }

    /// <summary>
    /// 发票总金额
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// 税费
    /// </summary>
    public decimal TaxAmount { get; set; }

    /// <summary>
    /// 发票实付金额
    /// </summary>
    public decimal ActualAmount { get; set; }

    /// <summary>
    /// 发票状态（0=草稿，1=已开票，2=已收款，3=已作废）
    /// </summary>
    public int InvoiceStatus { get; set; } = 0;

    /// <summary>
    /// 收款方式（0=现金，1=银行转账，2=支票，3=信用证，4=其他）
    /// </summary>
    public int PaymentMethod { get; set; } = 0;

    /// <summary>
    /// 发票号码（税务系统票号）
    /// </summary>
    public string? TaxInvoiceNo { get; set; } = string.Empty;

    /// <summary>
    /// 销售发票明细列表（主子表关系，一张发票可有多个明细行）
    /// （子表：TaktSalesInvoiceItem）
    /// </summary>
    public List<TaktSalesInvoiceItemDto>? Items { get; set; }

}

// ========================================
// SalesInvoice 查询 DTO
// ========================================

/// <summary>
/// SalesInvoice 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktSalesInvoiceQueryDto : TaktPagedQuery
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
    /// 工厂代码
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 销售发票编码（唯一索引）
    /// </summary>
    public string? SalesInvoiceCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联销售订单编码
    /// </summary>
    public string? SalesOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户名称
    /// </summary>
    public string? CustomerName { get; set; } = string.Empty;

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
    /// 税费
    /// </summary>
    public decimal? TaxAmount { get; set; }

    /// <summary>
    /// 发票实付金额
    /// </summary>
    public decimal? ActualAmount { get; set; }

    /// <summary>
    /// 发票状态（0=草稿，1=已开票，2=已收款，3=已作废）
    /// </summary>
    public int? InvoiceStatus { get; set; }

    /// <summary>
    /// 收款方式（0=现金，1=银行转账，2=支票，3=信用证，4=其他）
    /// </summary>
    public int? PaymentMethod { get; set; }

    /// <summary>
    /// 发票号码（税务系统票号）
    /// </summary>
    public string? TaxInvoiceNo { get; set; } = string.Empty;

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
    public string? ExtFieldJson { get; set; }

    /// <summary>
    /// 备注（模糊查询）
    /// </summary>
    public string? Remark { get; set; }
}

// ========================================
// 创建SalesInvoice DTO
// ========================================

/// <summary>
/// 创建SalesInvoice DTO
/// </summary>
public class TaktSalesInvoiceCreateDto
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
    /// 当前公司默认区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
    /// </summary>
    public string CompanyDefaultCulture { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 销售发票编码（唯一索引）
    /// </summary>
    [Required(ErrorMessage = "销售发票编码（唯一索引）不能为空")]
    public string SalesInvoiceCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联销售订单编码
    /// </summary>
    public string? SalesOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码
    /// </summary>
    [Required(ErrorMessage = "客户编码不能为空")]
    public string CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户名称
    /// </summary>
    [Required(ErrorMessage = "客户名称不能为空")]
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// 开票日期
    /// </summary>
    public DateTime InvoiceDate { get; set; }

    /// <summary>
    /// 发票总金额
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// 税费
    /// </summary>
    public decimal TaxAmount { get; set; }

    /// <summary>
    /// 发票实付金额
    /// </summary>
    public decimal ActualAmount { get; set; }

    /// <summary>
    /// 发票状态（0=草稿，1=已开票，2=已收款，3=已作废）
    /// </summary>
    public int InvoiceStatus { get; set; } = 0;

    /// <summary>
    /// 收款方式（0=现金，1=银行转账，2=支票，3=信用证，4=其他）
    /// </summary>
    public int PaymentMethod { get; set; } = 0;

    /// <summary>
    /// 发票号码（税务系统票号）
    /// </summary>
    public string? TaxInvoiceNo { get; set; } = string.Empty;

    /// <summary>
    /// 销售发票明细列表（主子表关系，一张发票可有多个明细行）（子表，级联保存）
    /// </summary>
    public List<TaktSalesInvoiceItemCreateDto>? Items { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtFieldJson { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 更新SalesInvoice DTO
// ========================================

/// <summary>
/// 更新SalesInvoice DTO
/// 继承 TaktSalesInvoiceCreateDto，添加 SalesInvoiceId 字段
/// </summary>
public class TaktSalesInvoiceUpdateDto : TaktSalesInvoiceCreateDto
{
    /// <summary>
    /// SalesInvoiceID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesInvoiceId { get; set; }

}

// ========================================
// SalesInvoice 状态 DTO
// ========================================

/// <summary>
/// SalesInvoice 状态更新 DTO
/// </summary>
public class TaktSalesInvoiceStatusDto
{
    /// <summary>
    /// SalesInvoiceID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesInvoiceId { get; set; }

    /// <summary>
    /// 发票状态（0=草稿，1=已开票，2=已收款，3=已作废）
    /// </summary>
    [Required(ErrorMessage = "发票状态（0=草稿，1=已开票，2=已收款，3=已作废）不能为空")]
    public int InvoiceStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// SalesInvoice 导入模板行 DTO
/// </summary>
public class TaktSalesInvoiceTemplateDto
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
    /// 工厂代码
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 销售发票编码（唯一索引）
    /// </summary>
    public string? SalesInvoiceCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联销售订单编码
    /// </summary>
    public string? SalesOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户名称
    /// </summary>
    public string? CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// 发票状态（0=草稿，1=已开票，2=已收款，3=已作废）
    /// </summary>
    public int? InvoiceStatus { get; set; }

    /// <summary>
    /// 收款方式（0=现金，1=银行转账，2=支票，3=信用证，4=其他）
    /// </summary>
    public int? PaymentMethod { get; set; }

    /// <summary>
    /// 发票号码（税务系统票号）
    /// </summary>
    public string? TaxInvoiceNo { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtFieldJson { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

/// <summary>
/// SalesInvoice 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktSalesInvoiceImportDto
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
    /// 当前公司默认区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
    /// </summary>
    public string? CompanyDefaultCulture { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 销售发票编码（唯一索引）
    /// </summary>
    public string? SalesInvoiceCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联销售订单编码
    /// </summary>
    public string? SalesOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户名称
    /// </summary>
    public string? CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// 发票状态（0=草稿，1=已开票，2=已收款，3=已作废）
    /// </summary>
    public int? InvoiceStatus { get; set; }

    /// <summary>
    /// 收款方式（0=现金，1=银行转账，2=支票，3=信用证，4=其他）
    /// </summary>
    public int? PaymentMethod { get; set; }

    /// <summary>
    /// 发票号码（税务系统票号）
    /// </summary>
    public string? TaxInvoiceNo { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtFieldJson { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 导出 DTO
// ========================================

/// <summary>
/// SalesInvoice 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktSalesInvoiceExportDto
{
    /// <summary>
    /// SalesInvoiceID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesInvoiceId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 销售发票编码（唯一索引）
    /// </summary>
    public string SalesInvoiceCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联销售订单编码
    /// </summary>
    public string? SalesOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码
    /// </summary>
    public string CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户名称
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// 开票日期
    /// </summary>
    public DateTime InvoiceDate { get; set; }

    /// <summary>
    /// 发票总金额
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// 税费
    /// </summary>
    public decimal TaxAmount { get; set; }

    /// <summary>
    /// 发票实付金额
    /// </summary>
    public decimal ActualAmount { get; set; }

    /// <summary>
    /// 发票状态（0=草稿，1=已开票，2=已收款，3=已作废）
    /// </summary>
    public int InvoiceStatus { get; set; } = 0;

    /// <summary>
    /// 收款方式（0=现金，1=银行转账，2=支票，3=信用证，4=其他）
    /// </summary>
    public int PaymentMethod { get; set; } = 0;

    /// <summary>
    /// 发票号码（税务系统票号）
    /// </summary>
    public string? TaxInvoiceNo { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtFieldJson { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
