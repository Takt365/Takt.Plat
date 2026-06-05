// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Materials
// 文件名称：TaktPurchaseOrderDtos.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：PurchaseOrder 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktPurchaseOrder 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Materials;

// ========================================
// PurchaseOrder 响应 DTO
// ========================================

/// <summary>
/// Takt采购订单实体
/// 对应前端 TaktPurchaseOrderDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktPurchaseOrderDto : TaktCompanyDtoBase
{
    /// <summary>
    /// PurchaseOrderID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseOrderId { get; set; }

    /// <summary>
    /// 工厂代码（不可空）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单编码（唯一索引）
    /// </summary>
    public string PurchaseOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码
    /// </summary>
    public string SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商名称
    /// </summary>
    public string SupplierName { get; set; } = string.Empty;

    /// <summary>
    /// 订单日期
    /// </summary>
    public DateTime OrderDate { get; set; }

    /// <summary>
    /// 要求到货日期
    /// </summary>
    public DateTime? RequiredArrivalDate { get; set; }

    /// <summary>
    /// 实际到货日期
    /// </summary>
    public DateTime? ActualArrivalDate { get; set; }

    /// <summary>
    /// 采购组代码
    /// </summary>
    public string? PurchaseGroup { get; set; } = string.Empty;

    /// <summary>
    /// 订单总数量（基本单位数量）
    /// </summary>
    public decimal TotalQuantity { get; set; }

    /// <summary>
    /// 订单总金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// 折扣金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal DiscountAmount { get; set; }

    /// <summary>
    /// 税费（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal TaxAmount { get; set; }

    /// <summary>
    /// 订单实付金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal ActualAmount { get; set; }

    /// <summary>
    /// 已入库数量（基本单位数量）
    /// </summary>
    public decimal ReceivedQuantity { get; set; }

    /// <summary>
    /// 已入库金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal ReceivedAmount { get; set; }

    /// <summary>
    /// 已付款金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal PaidAmount { get; set; }

    /// <summary>
    /// 订单状态（1=启用，0=禁用）
    /// </summary>
    public int OrderStatus { get; set; } = 0;

    /// <summary>
    /// 交货状态（0=未交货，1=部分交货，2=全部交货）
    /// </summary>
    public int DeliveryStatus { get; set; } = 0;

    /// <summary>
    /// 支付方式（0=现金，1=银行转账，2=支票，3=信用证，4=其他）
    /// </summary>
    public int PaymentMethod { get; set; } = 0;

    /// <summary>
    /// 交货方式（0=自提，1=供应商送货，2=物流配送，3=快递）
    /// </summary>
    public int DeliveryMethod { get; set; } = 0;

    /// <summary>
    /// 交货地址
    /// </summary>
    public string? DeliveryAddress { get; set; } = string.Empty;

    /// <summary>
    /// 订单明细列表（主子表关系，一个订单可以有多个明细）
    /// （子表：TaktPurchaseOrderItem）
    /// </summary>
    public List<TaktPurchaseOrderItemDto>? Items { get; set; }

    /// <summary>
    /// 采购订单变更记录列表（外键在子表 <see cref="TaktPurchaseOrderChangeLog.OrderId"/>）
    /// （子表：TaktPurchaseOrderChangeLog）
    /// </summary>
    public List<TaktPurchaseOrderChangeLogDto>? ChangeLogs { get; set; }

}

// ========================================
// PurchaseOrder 查询 DTO
// ========================================

/// <summary>
/// PurchaseOrder 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktPurchaseOrderQueryDto : TaktPagedQuery
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
    /// 工厂代码（不可空）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单编码（唯一索引）
    /// </summary>
    public string? PurchaseOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商名称
    /// </summary>
    public string? SupplierName { get; set; } = string.Empty;

    /// <summary>
    /// 订单日期（范围查询-开始）
    /// </summary>
    public DateTime? OrderDateStart { get; set; }

    /// <summary>
    /// 订单日期（范围查询-结束）
    /// </summary>
    public DateTime? OrderDateEnd { get; set; }

    /// <summary>
    /// 要求到货日期（范围查询-开始）
    /// </summary>
    public DateTime? RequiredArrivalDateStart { get; set; }

    /// <summary>
    /// 要求到货日期（范围查询-结束）
    /// </summary>
    public DateTime? RequiredArrivalDateEnd { get; set; }

    /// <summary>
    /// 实际到货日期（范围查询-开始）
    /// </summary>
    public DateTime? ActualArrivalDateStart { get; set; }

    /// <summary>
    /// 实际到货日期（范围查询-结束）
    /// </summary>
    public DateTime? ActualArrivalDateEnd { get; set; }

    /// <summary>
    /// 采购组代码
    /// </summary>
    public string? PurchaseGroup { get; set; } = string.Empty;

    /// <summary>
    /// 订单总数量（基本单位数量）
    /// </summary>
    public decimal? TotalQuantity { get; set; }

    /// <summary>
    /// 订单总金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal? TotalAmount { get; set; }

    /// <summary>
    /// 折扣金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal? DiscountAmount { get; set; }

    /// <summary>
    /// 税费（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal? TaxAmount { get; set; }

    /// <summary>
    /// 订单实付金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal? ActualAmount { get; set; }

    /// <summary>
    /// 已入库数量（基本单位数量）
    /// </summary>
    public decimal? ReceivedQuantity { get; set; }

    /// <summary>
    /// 已入库金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal? ReceivedAmount { get; set; }

    /// <summary>
    /// 已付款金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal? PaidAmount { get; set; }

    /// <summary>
    /// 订单状态（1=启用，0=禁用）
    /// </summary>
    public int? OrderStatus { get; set; }

    /// <summary>
    /// 交货状态（0=未交货，1=部分交货，2=全部交货）
    /// </summary>
    public int? DeliveryStatus { get; set; }

    /// <summary>
    /// 支付方式（0=现金，1=银行转账，2=支票，3=信用证，4=其他）
    /// </summary>
    public int? PaymentMethod { get; set; }

    /// <summary>
    /// 交货方式（0=自提，1=供应商送货，2=物流配送，3=快递）
    /// </summary>
    public int? DeliveryMethod { get; set; }

    /// <summary>
    /// 交货地址
    /// </summary>
    public string? DeliveryAddress { get; set; } = string.Empty;

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
// 创建PurchaseOrder DTO
// ========================================

/// <summary>
/// 创建PurchaseOrder DTO
/// </summary>
public class TaktPurchaseOrderCreateDto
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
    /// 工厂代码（不可空）
    /// </summary>
    [Required(ErrorMessage = "工厂代码（不可空）不能为空")]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单编码（唯一索引）
    /// </summary>
    [Required(ErrorMessage = "采购订单编码（唯一索引）不能为空")]
    public string PurchaseOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码
    /// </summary>
    [Required(ErrorMessage = "供应商编码不能为空")]
    public string SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商名称
    /// </summary>
    [Required(ErrorMessage = "供应商名称不能为空")]
    public string SupplierName { get; set; } = string.Empty;

    /// <summary>
    /// 订单日期
    /// </summary>
    public DateTime OrderDate { get; set; }

    /// <summary>
    /// 要求到货日期
    /// </summary>
    public DateTime? RequiredArrivalDate { get; set; }

    /// <summary>
    /// 实际到货日期
    /// </summary>
    public DateTime? ActualArrivalDate { get; set; }

    /// <summary>
    /// 采购组代码
    /// </summary>
    public string? PurchaseGroup { get; set; } = string.Empty;

    /// <summary>
    /// 订单总数量（基本单位数量）
    /// </summary>
    public decimal TotalQuantity { get; set; }

    /// <summary>
    /// 订单总金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// 折扣金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal DiscountAmount { get; set; }

    /// <summary>
    /// 税费（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal TaxAmount { get; set; }

    /// <summary>
    /// 订单实付金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal ActualAmount { get; set; }

    /// <summary>
    /// 已入库数量（基本单位数量）
    /// </summary>
    public decimal ReceivedQuantity { get; set; }

    /// <summary>
    /// 已入库金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal ReceivedAmount { get; set; }

    /// <summary>
    /// 已付款金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal PaidAmount { get; set; }

    /// <summary>
    /// 订单状态（1=启用，0=禁用）
    /// </summary>
    public int OrderStatus { get; set; } = 0;

    /// <summary>
    /// 交货状态（0=未交货，1=部分交货，2=全部交货）
    /// </summary>
    public int DeliveryStatus { get; set; } = 0;

    /// <summary>
    /// 支付方式（0=现金，1=银行转账，2=支票，3=信用证，4=其他）
    /// </summary>
    public int PaymentMethod { get; set; } = 0;

    /// <summary>
    /// 交货方式（0=自提，1=供应商送货，2=物流配送，3=快递）
    /// </summary>
    public int DeliveryMethod { get; set; } = 0;

    /// <summary>
    /// 交货地址
    /// </summary>
    public string? DeliveryAddress { get; set; } = string.Empty;

    /// <summary>
    /// 订单明细列表（主子表关系，一个订单可以有多个明细）（子表，级联保存）
    /// </summary>
    public List<TaktPurchaseOrderItemCreateDto>? Items { get; set; }

    /// <summary>
    /// 采购订单变更记录列表（外键在子表 <see cref="TaktPurchaseOrderChangeLog.OrderId"/>）（子表，级联保存）
    /// </summary>
    public List<TaktPurchaseOrderChangeLogCreateDto>? ChangeLogs { get; set; }

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
// 更新PurchaseOrder DTO
// ========================================

/// <summary>
/// 更新PurchaseOrder DTO
/// 继承 TaktPurchaseOrderCreateDto，添加 PurchaseOrderId 字段
/// </summary>
public class TaktPurchaseOrderUpdateDto : TaktPurchaseOrderCreateDto
{
    /// <summary>
    /// PurchaseOrderID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseOrderId { get; set; }

}

// ========================================
// PurchaseOrder 状态 DTO
// ========================================

/// <summary>
/// PurchaseOrder 状态更新 DTO
/// </summary>
public class TaktPurchaseOrderStatusDto
{
    /// <summary>
    /// PurchaseOrderID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseOrderId { get; set; }

    /// <summary>
    /// 订单状态（1=启用，0=禁用）
    /// </summary>
    [Required(ErrorMessage = "订单状态（1=启用，0=禁用）不能为空")]
    public int OrderStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// PurchaseOrder 导入模板行 DTO
/// </summary>
public class TaktPurchaseOrderTemplateDto
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
    /// 工厂代码（不可空）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单编码（唯一索引）
    /// </summary>
    public string? PurchaseOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商名称
    /// </summary>
    public string? SupplierName { get; set; } = string.Empty;

    /// <summary>
    /// 采购组代码
    /// </summary>
    public string? PurchaseGroup { get; set; } = string.Empty;

    /// <summary>
    /// 订单状态（1=启用，0=禁用）
    /// </summary>
    public int? OrderStatus { get; set; }

    /// <summary>
    /// 交货状态（0=未交货，1=部分交货，2=全部交货）
    /// </summary>
    public int? DeliveryStatus { get; set; }

    /// <summary>
    /// 支付方式（0=现金，1=银行转账，2=支票，3=信用证，4=其他）
    /// </summary>
    public int? PaymentMethod { get; set; }

    /// <summary>
    /// 交货方式（0=自提，1=供应商送货，2=物流配送，3=快递）
    /// </summary>
    public int? DeliveryMethod { get; set; }

    /// <summary>
    /// 交货地址
    /// </summary>
    public string? DeliveryAddress { get; set; } = string.Empty;

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
/// PurchaseOrder 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktPurchaseOrderImportDto
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
    /// 工厂代码（不可空）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单编码（唯一索引）
    /// </summary>
    public string? PurchaseOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商名称
    /// </summary>
    public string? SupplierName { get; set; } = string.Empty;

    /// <summary>
    /// 采购组代码
    /// </summary>
    public string? PurchaseGroup { get; set; } = string.Empty;

    /// <summary>
    /// 订单状态（1=启用，0=禁用）
    /// </summary>
    public int? OrderStatus { get; set; }

    /// <summary>
    /// 交货状态（0=未交货，1=部分交货，2=全部交货）
    /// </summary>
    public int? DeliveryStatus { get; set; }

    /// <summary>
    /// 支付方式（0=现金，1=银行转账，2=支票，3=信用证，4=其他）
    /// </summary>
    public int? PaymentMethod { get; set; }

    /// <summary>
    /// 交货方式（0=自提，1=供应商送货，2=物流配送，3=快递）
    /// </summary>
    public int? DeliveryMethod { get; set; }

    /// <summary>
    /// 交货地址
    /// </summary>
    public string? DeliveryAddress { get; set; } = string.Empty;

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
/// PurchaseOrder 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktPurchaseOrderExportDto
{
    /// <summary>
    /// PurchaseOrderID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseOrderId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（不可空）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单编码（唯一索引）
    /// </summary>
    public string PurchaseOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码
    /// </summary>
    public string SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商名称
    /// </summary>
    public string SupplierName { get; set; } = string.Empty;

    /// <summary>
    /// 订单日期
    /// </summary>
    public DateTime OrderDate { get; set; }

    /// <summary>
    /// 要求到货日期
    /// </summary>
    public DateTime? RequiredArrivalDate { get; set; }

    /// <summary>
    /// 实际到货日期
    /// </summary>
    public DateTime? ActualArrivalDate { get; set; }

    /// <summary>
    /// 采购组代码
    /// </summary>
    public string? PurchaseGroup { get; set; } = string.Empty;

    /// <summary>
    /// 订单总数量（基本单位数量）
    /// </summary>
    public decimal TotalQuantity { get; set; }

    /// <summary>
    /// 订单总金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// 折扣金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal DiscountAmount { get; set; }

    /// <summary>
    /// 税费（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal TaxAmount { get; set; }

    /// <summary>
    /// 订单实付金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal ActualAmount { get; set; }

    /// <summary>
    /// 已入库数量（基本单位数量）
    /// </summary>
    public decimal ReceivedQuantity { get; set; }

    /// <summary>
    /// 已入库金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal ReceivedAmount { get; set; }

    /// <summary>
    /// 已付款金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal PaidAmount { get; set; }

    /// <summary>
    /// 订单状态（1=启用，0=禁用）
    /// </summary>
    public int OrderStatus { get; set; } = 0;

    /// <summary>
    /// 交货状态（0=未交货，1=部分交货，2=全部交货）
    /// </summary>
    public int DeliveryStatus { get; set; } = 0;

    /// <summary>
    /// 支付方式（0=现金，1=银行转账，2=支票，3=信用证，4=其他）
    /// </summary>
    public int PaymentMethod { get; set; } = 0;

    /// <summary>
    /// 交货方式（0=自提，1=供应商送货，2=物流配送，3=快递）
    /// </summary>
    public int DeliveryMethod { get; set; } = 0;

    /// <summary>
    /// 交货地址
    /// </summary>
    public string? DeliveryAddress { get; set; } = string.Empty;

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
