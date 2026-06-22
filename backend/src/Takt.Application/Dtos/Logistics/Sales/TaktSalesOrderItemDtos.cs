// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Sales
// 文件名称：TaktSalesOrderItemDtos.cs
// 创建时间：2026-06-20
// 创建人：Takt365(Auto Generated)
// 功能描述：SalesOrderItem 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktSalesOrderItem 生成，请按需审阅）
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
// SalesOrderItem 响应 DTO
// ========================================

/// <summary>
/// Takt销售订单明细实体
/// 对应前端 TaktSalesOrderItemDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktSalesOrderItemDto : TaktCompanyDtoBase
{
    /// <summary>
    /// SalesOrderItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesOrderItemId { get; set; }

    /// <summary>
    /// 销售订单ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesOrderId { get; set; }

    /// <summary>
    /// 销售订单名称（填充字段）
    /// </summary>
    public string? SalesOrderName { get; set; }

    /// <summary>
    /// 销售订单编码（冗余字段，便于查询）
    /// </summary>
    public string SalesOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 物料编码
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称
    /// </summary>
    public string MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格
    /// </summary>
    public string? MaterialSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 销售单位
    /// </summary>
    public string SalesUnit { get; set; } = string.Empty;

    /// <summary>
    /// 订购数量（基本单位数量）
    /// </summary>
    public decimal OrderQuantity { get; set; }

    /// <summary>
    /// 已发货数量（基本单位数量）
    /// </summary>
    public decimal ShippedQuantity { get; set; }

    /// <summary>
    /// 单价（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// 折扣率（0-100，表示折扣百分比）
    /// </summary>
    public decimal DiscountRate { get; set; }

    /// <summary>
    /// 折扣金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal DiscountAmount { get; set; }

    /// <summary>
    /// 税费率（0-100，表示税费百分比）
    /// </summary>
    public decimal TaxRate { get; set; }

    /// <summary>
    /// 税费（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal TaxAmount { get; set; }

    /// <summary>
    /// 小计金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal SubtotalAmount { get; set; }

    /// <summary>
    /// 行交货状态（0=未交货，1=部分交货，2=全部交货）
    /// </summary>
    public int DeliveryStatus { get; set; } = 0;

    /// <summary>
    /// 销售订单主表
    /// （主表：TaktSalesOrder）
    /// </summary>
    public TaktSalesOrderDto? SalesOrder { get; set; }

}

// ========================================
// SalesOrderItem 查询 DTO
// ========================================

/// <summary>
/// SalesOrderItem 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktSalesOrderItemQueryDto : TaktPagedQuery
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
    /// 销售订单ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesOrderId { get; set; }

    /// <summary>
    /// 销售订单编码（冗余字段，便于查询）
    /// </summary>
    public string? SalesOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

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
    /// 销售单位
    /// </summary>
    public string? SalesUnit { get; set; } = string.Empty;

    /// <summary>
    /// 订购数量（基本单位数量）
    /// </summary>
    public decimal? OrderQuantity { get; set; }

    /// <summary>
    /// 已发货数量（基本单位数量）
    /// </summary>
    public decimal? ShippedQuantity { get; set; }

    /// <summary>
    /// 单价（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal? UnitPrice { get; set; }

    /// <summary>
    /// 折扣率（0-100，表示折扣百分比）
    /// </summary>
    public decimal? DiscountRate { get; set; }

    /// <summary>
    /// 折扣金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal? DiscountAmount { get; set; }

    /// <summary>
    /// 税费率（0-100，表示税费百分比）
    /// </summary>
    public decimal? TaxRate { get; set; }

    /// <summary>
    /// 税费（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal? TaxAmount { get; set; }

    /// <summary>
    /// 小计金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal? SubtotalAmount { get; set; }

    /// <summary>
    /// 行交货状态（0=未交货，1=部分交货，2=全部交货）
    /// </summary>
    public int? DeliveryStatus { get; set; }

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
// 创建SalesOrderItem DTO
// ========================================

/// <summary>
/// 创建SalesOrderItem DTO
/// </summary>
public class TaktSalesOrderItemCreateDto
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
    /// 销售订单ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesOrderId { get; set; }

    /// <summary>
    /// 销售订单编码（冗余字段，便于查询）
    /// </summary>
    [Required(ErrorMessage = "销售订单编码（冗余字段，便于查询）不能为空")]
    public string SalesOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 物料编码
    /// </summary>
    [Required(ErrorMessage = "物料编码不能为空")]
    public string MaterialCode { get; set; } = string.Empty;

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
    /// 销售单位
    /// </summary>
    [Required(ErrorMessage = "销售单位不能为空")]
    public string SalesUnit { get; set; } = string.Empty;

    /// <summary>
    /// 订购数量（基本单位数量）
    /// </summary>
    public decimal OrderQuantity { get; set; }

    /// <summary>
    /// 已发货数量（基本单位数量）
    /// </summary>
    public decimal ShippedQuantity { get; set; }

    /// <summary>
    /// 单价（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// 折扣率（0-100，表示折扣百分比）
    /// </summary>
    public decimal DiscountRate { get; set; }

    /// <summary>
    /// 折扣金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal DiscountAmount { get; set; }

    /// <summary>
    /// 税费率（0-100，表示税费百分比）
    /// </summary>
    public decimal TaxRate { get; set; }

    /// <summary>
    /// 税费（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal TaxAmount { get; set; }

    /// <summary>
    /// 小计金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal SubtotalAmount { get; set; }

    /// <summary>
    /// 行交货状态（0=未交货，1=部分交货，2=全部交货）
    /// </summary>
    public int DeliveryStatus { get; set; } = 0;

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
// 更新SalesOrderItem DTO
// ========================================

/// <summary>
/// 更新SalesOrderItem DTO
/// 继承 TaktSalesOrderItemCreateDto，添加 SalesOrderItemId 字段
/// </summary>
public class TaktSalesOrderItemUpdateDto : TaktSalesOrderItemCreateDto
{
    /// <summary>
    /// SalesOrderItemID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesOrderItemId { get; set; }

}

// ========================================
// SalesOrderItem 状态 DTO
// ========================================

/// <summary>
/// SalesOrderItem 状态更新 DTO
/// </summary>
public class TaktSalesOrderItemStatusDto
{
    /// <summary>
    /// SalesOrderItemID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesOrderItemId { get; set; }

    /// <summary>
    /// 行交货状态（0=未交货，1=部分交货，2=全部交货）
    /// </summary>
    [Required(ErrorMessage = "行交货状态（0=未交货，1=部分交货，2=全部交货）不能为空")]
    public int DeliveryStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// SalesOrderItem 导入模板行 DTO
/// </summary>
public class TaktSalesOrderItemTemplateDto
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
    /// 销售订单ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesOrderId { get; set; }

    /// <summary>
    /// 销售订单编码（冗余字段，便于查询）
    /// </summary>
    public string? SalesOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

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
    /// 销售单位
    /// </summary>
    public string? SalesUnit { get; set; } = string.Empty;

    /// <summary>
    /// 行交货状态（0=未交货，1=部分交货，2=全部交货）
    /// </summary>
    public int? DeliveryStatus { get; set; }

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
/// SalesOrderItem 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktSalesOrderItemImportDto
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
    /// 销售订单ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesOrderId { get; set; }

    /// <summary>
    /// 销售订单编码（冗余字段，便于查询）
    /// </summary>
    public string? SalesOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

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
    /// 销售单位
    /// </summary>
    public string? SalesUnit { get; set; } = string.Empty;

    /// <summary>
    /// 行交货状态（0=未交货，1=部分交货，2=全部交货）
    /// </summary>
    public int? DeliveryStatus { get; set; }

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
/// SalesOrderItem 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktSalesOrderItemExportDto
{
    /// <summary>
    /// SalesOrderItemID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesOrderItemId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 销售订单ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesOrderId { get; set; }

    /// <summary>
    /// 销售订单编码（冗余字段，便于查询）
    /// </summary>
    public string SalesOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 物料编码
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称
    /// </summary>
    public string MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格
    /// </summary>
    public string? MaterialSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 销售单位
    /// </summary>
    public string SalesUnit { get; set; } = string.Empty;

    /// <summary>
    /// 订购数量（基本单位数量）
    /// </summary>
    public decimal OrderQuantity { get; set; }

    /// <summary>
    /// 已发货数量（基本单位数量）
    /// </summary>
    public decimal ShippedQuantity { get; set; }

    /// <summary>
    /// 单价（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// 折扣率（0-100，表示折扣百分比）
    /// </summary>
    public decimal DiscountRate { get; set; }

    /// <summary>
    /// 折扣金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal DiscountAmount { get; set; }

    /// <summary>
    /// 税费率（0-100，表示税费百分比）
    /// </summary>
    public decimal TaxRate { get; set; }

    /// <summary>
    /// 税费（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal TaxAmount { get; set; }

    /// <summary>
    /// 小计金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal SubtotalAmount { get; set; }

    /// <summary>
    /// 行交货状态（0=未交货，1=部分交货，2=全部交货）
    /// </summary>
    public int DeliveryStatus { get; set; } = 0;

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
