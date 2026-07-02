// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Sales
// 文件名称：TaktSalesOrderDtos.cs
// 创建时间：2026-07-01
// 创建人：Takt365(Auto Generated)
// 功能描述：SalesOrder 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktSalesOrder 生成，请按需审阅）
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
// SalesOrder 响应 DTO
// ========================================

/// <summary>
/// Takt销售订单实体
/// 对应前端 TaktSalesOrderDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktSalesOrderDto : TaktCompanyDtoBase
{
    /// <summary>
    /// SalesOrderID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesOrderId { get; set; }

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 销售订单编码（唯一索引）
    /// </summary>
    public string SalesOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码（选项 TaktCustomers/options，DictValue=CustomerCode）
    /// </summary>
    public string CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户名称
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// 订单日期
    /// </summary>
    public DateTime OrderDate { get; set; }

    /// <summary>
    /// 要求交货日期
    /// </summary>
    public DateTime? RequiredDeliveryDate { get; set; }

    /// <summary>
    /// 实际交货日期
    /// </summary>
    public DateTime? ActualDeliveryDate { get; set; }

    /// <summary>
    /// 销售员（选项 TaktEmployees/options，DictValue=EmployeeCode）
    /// </summary>
    public string? SalesBy { get; set; } = string.Empty;

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
    /// 已发货数量（基本单位数量）
    /// </summary>
    public decimal ShippedQuantity { get; set; }

    /// <summary>
    /// 已发货金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal ShippedAmount { get; set; }

    /// <summary>
    /// 已收款金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal ReceivedAmount { get; set; }

    /// <summary>
    /// 交货方式（字典 logistics_delivery_method_type；0=自提 1=送货上门 2=物流配送 3=快递）
    /// </summary>
    public int DeliveryMethod { get; set; } = 0;

    /// <summary>
    /// 收款方式（字典 accounting_payment_method_type；0=现金 1=银行转账 2=支票 3=信用证 4=其他）
    /// </summary>
    public int PaymentMethod { get; set; } = 0;

    /// <summary>
    /// 交货地址
    /// </summary>
    public string? DeliveryAddress { get; set; } = string.Empty;

    /// <summary>
    /// 订单状态（字典 sys_normal_disable_status；1=启用 0=禁用）
    /// </summary>
    public int OrderStatus { get; set; } = 0;

    /// <summary>
    /// 交货状态（字典 logistics_delivery_status；0=未交货 1=部分交货 2=全部交货）
    /// </summary>
    public int DeliveryStatus { get; set; } = 0;

    /// <summary>
    /// 销售订单明细列表（主子表关系，一个订单可以有多个明细）
    /// （子表：TaktSalesOrderItem）
    /// </summary>
    public List<TaktSalesOrderItemDto>? Items { get; set; }

    /// <summary>
    /// 销售订单变更记录列表（外键在子表 TaktSalesOrderChangeLog.SalesOrderId）
    /// （子表：TaktSalesOrderChangeLog）
    /// </summary>
    public List<TaktSalesOrderChangeLogDto>? ChangeLogs { get; set; }

}

// ========================================
// SalesOrder 查询 DTO
// ========================================

/// <summary>
/// SalesOrder 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktSalesOrderQueryDto : TaktPagedQuery
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
    /// 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 销售订单编码（唯一索引）
    /// </summary>
    public string? SalesOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码（选项 TaktCustomers/options，DictValue=CustomerCode）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户名称
    /// </summary>
    public string? CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// 订单日期（范围查询-开始）
    /// </summary>
    public DateTime? OrderDateStart { get; set; }

    /// <summary>
    /// 订单日期（范围查询-结束）
    /// </summary>
    public DateTime? OrderDateEnd { get; set; }

    /// <summary>
    /// 要求交货日期（范围查询-开始）
    /// </summary>
    public DateTime? RequiredDeliveryDateStart { get; set; }

    /// <summary>
    /// 要求交货日期（范围查询-结束）
    /// </summary>
    public DateTime? RequiredDeliveryDateEnd { get; set; }

    /// <summary>
    /// 实际交货日期（范围查询-开始）
    /// </summary>
    public DateTime? ActualDeliveryDateStart { get; set; }

    /// <summary>
    /// 实际交货日期（范围查询-结束）
    /// </summary>
    public DateTime? ActualDeliveryDateEnd { get; set; }

    /// <summary>
    /// 销售员（选项 TaktEmployees/options，DictValue=EmployeeCode）
    /// </summary>
    public string? SalesBy { get; set; } = string.Empty;

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
    /// 已发货数量（基本单位数量）
    /// </summary>
    public decimal? ShippedQuantity { get; set; }

    /// <summary>
    /// 已发货金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal? ShippedAmount { get; set; }

    /// <summary>
    /// 已收款金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal? ReceivedAmount { get; set; }

    /// <summary>
    /// 交货方式（字典 logistics_delivery_method_type；0=自提 1=送货上门 2=物流配送 3=快递）
    /// </summary>
    public int? DeliveryMethod { get; set; }

    /// <summary>
    /// 收款方式（字典 accounting_payment_method_type；0=现金 1=银行转账 2=支票 3=信用证 4=其他）
    /// </summary>
    public int? PaymentMethod { get; set; }

    /// <summary>
    /// 交货地址
    /// </summary>
    public string? DeliveryAddress { get; set; } = string.Empty;

    /// <summary>
    /// 订单状态（字典 sys_normal_disable_status；1=启用 0=禁用）
    /// </summary>
    public int? OrderStatus { get; set; }

    /// <summary>
    /// 交货状态（字典 logistics_delivery_status；0=未交货 1=部分交货 2=全部交货）
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
// 创建SalesOrder DTO
// ========================================

/// <summary>
/// 创建SalesOrder DTO
/// </summary>
public class TaktSalesOrderCreateDto
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
    /// 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
    /// </summary>
    [Required(ErrorMessage = "工厂代码（选项 TaktPlants/options，DictValue=PlantCode）不能为空")]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 销售订单编码（唯一索引）
    /// </summary>
    [Required(ErrorMessage = "销售订单编码（唯一索引）不能为空")]
    public string SalesOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码（选项 TaktCustomers/options，DictValue=CustomerCode）
    /// </summary>
    [Required(ErrorMessage = "客户编码（选项 TaktCustomers/options，DictValue=CustomerCode）不能为空")]
    public string CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户名称
    /// </summary>
    [Required(ErrorMessage = "客户名称不能为空")]
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// 订单日期
    /// </summary>
    public DateTime OrderDate { get; set; }

    /// <summary>
    /// 要求交货日期
    /// </summary>
    public DateTime? RequiredDeliveryDate { get; set; }

    /// <summary>
    /// 实际交货日期
    /// </summary>
    public DateTime? ActualDeliveryDate { get; set; }

    /// <summary>
    /// 销售员（选项 TaktEmployees/options，DictValue=EmployeeCode）
    /// </summary>
    public string? SalesBy { get; set; } = string.Empty;

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
    /// 已发货数量（基本单位数量）
    /// </summary>
    public decimal ShippedQuantity { get; set; }

    /// <summary>
    /// 已发货金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal ShippedAmount { get; set; }

    /// <summary>
    /// 已收款金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal ReceivedAmount { get; set; }

    /// <summary>
    /// 交货方式（字典 logistics_delivery_method_type；0=自提 1=送货上门 2=物流配送 3=快递）
    /// </summary>
    public int DeliveryMethod { get; set; } = 0;

    /// <summary>
    /// 收款方式（字典 accounting_payment_method_type；0=现金 1=银行转账 2=支票 3=信用证 4=其他）
    /// </summary>
    public int PaymentMethod { get; set; } = 0;

    /// <summary>
    /// 交货地址
    /// </summary>
    public string? DeliveryAddress { get; set; } = string.Empty;

    /// <summary>
    /// 订单状态（字典 sys_normal_disable_status；1=启用 0=禁用）
    /// </summary>
    public int OrderStatus { get; set; } = 0;

    /// <summary>
    /// 交货状态（字典 logistics_delivery_status；0=未交货 1=部分交货 2=全部交货）
    /// </summary>
    public int DeliveryStatus { get; set; } = 0;

    /// <summary>
    /// 销售订单明细列表（主子表关系，一个订单可以有多个明细）（子表，级联保存）
    /// </summary>
    public List<TaktSalesOrderItemCreateDto>? Items { get; set; }

    /// <summary>
    /// 销售订单变更记录列表（外键在子表 TaktSalesOrderChangeLog.SalesOrderId）（子表，级联保存）
    /// </summary>
    public List<TaktSalesOrderChangeLogCreateDto>? ChangeLogs { get; set; }

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
// 更新SalesOrder DTO
// ========================================

/// <summary>
/// 更新SalesOrder DTO
/// 继承 TaktSalesOrderCreateDto，添加 SalesOrderId 字段
/// </summary>
public class TaktSalesOrderUpdateDto : TaktSalesOrderCreateDto
{
    /// <summary>
    /// SalesOrderID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesOrderId { get; set; }

}

// ========================================
// SalesOrder 状态 DTO
// ========================================

/// <summary>
/// SalesOrder 状态更新 DTO
/// </summary>
public class TaktSalesOrderStatusDto
{
    /// <summary>
    /// SalesOrderID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesOrderId { get; set; }

    /// <summary>
    /// 订单状态（字典 sys_normal_disable_status；1=启用 0=禁用）
    /// </summary>
    [Required(ErrorMessage = "订单状态（字典 sys_normal_disable_status；1=启用 0=禁用）不能为空")]
    public int OrderStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// SalesOrder 导入模板行 DTO
/// </summary>
public class TaktSalesOrderTemplateDto
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
    /// 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 销售订单编码（唯一索引）
    /// </summary>
    public string? SalesOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码（选项 TaktCustomers/options，DictValue=CustomerCode）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户名称
    /// </summary>
    public string? CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// 订单日期
    /// </summary>
    public DateTime? OrderDate { get; set; }

    /// <summary>
    /// 要求交货日期
    /// </summary>
    public DateTime? RequiredDeliveryDate { get; set; }

    /// <summary>
    /// 实际交货日期
    /// </summary>
    public DateTime? ActualDeliveryDate { get; set; }

    /// <summary>
    /// 销售员（选项 TaktEmployees/options，DictValue=EmployeeCode）
    /// </summary>
    public string? SalesBy { get; set; } = string.Empty;

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
    /// 已发货数量（基本单位数量）
    /// </summary>
    public decimal? ShippedQuantity { get; set; }

    /// <summary>
    /// 已发货金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal? ShippedAmount { get; set; }

    /// <summary>
    /// 已收款金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal? ReceivedAmount { get; set; }

    /// <summary>
    /// 交货方式（字典 logistics_delivery_method_type；0=自提 1=送货上门 2=物流配送 3=快递）
    /// </summary>
    public int? DeliveryMethod { get; set; }

    /// <summary>
    /// 收款方式（字典 accounting_payment_method_type；0=现金 1=银行转账 2=支票 3=信用证 4=其他）
    /// </summary>
    public int? PaymentMethod { get; set; }

    /// <summary>
    /// 交货地址
    /// </summary>
    public string? DeliveryAddress { get; set; } = string.Empty;

    /// <summary>
    /// 订单状态（字典 sys_normal_disable_status；1=启用 0=禁用）
    /// </summary>
    public int? OrderStatus { get; set; }

    /// <summary>
    /// 交货状态（字典 logistics_delivery_status；0=未交货 1=部分交货 2=全部交货）
    /// </summary>
    public int? DeliveryStatus { get; set; }

    /// <summary>
    /// 销售订单明细列表（主子表关系，一个订单可以有多个明细）（子表，级联保存）
    /// </summary>
    public List<TaktSalesOrderItemCreateDto>? Items { get; set; }

    /// <summary>
    /// 销售订单变更记录列表（外键在子表 TaktSalesOrderChangeLog.SalesOrderId）（子表，级联保存）
    /// </summary>
    public List<TaktSalesOrderChangeLogCreateDto>? ChangeLogs { get; set; }

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
/// SalesOrder 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktSalesOrderImportDto
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
    /// 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 销售订单编码（唯一索引）
    /// </summary>
    public string? SalesOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码（选项 TaktCustomers/options，DictValue=CustomerCode）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户名称
    /// </summary>
    public string? CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// 订单日期
    /// </summary>
    public DateTime? OrderDate { get; set; }

    /// <summary>
    /// 要求交货日期
    /// </summary>
    public DateTime? RequiredDeliveryDate { get; set; }

    /// <summary>
    /// 实际交货日期
    /// </summary>
    public DateTime? ActualDeliveryDate { get; set; }

    /// <summary>
    /// 销售员（选项 TaktEmployees/options，DictValue=EmployeeCode）
    /// </summary>
    public string? SalesBy { get; set; } = string.Empty;

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
    /// 已发货数量（基本单位数量）
    /// </summary>
    public decimal? ShippedQuantity { get; set; }

    /// <summary>
    /// 已发货金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal? ShippedAmount { get; set; }

    /// <summary>
    /// 已收款金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal? ReceivedAmount { get; set; }

    /// <summary>
    /// 交货方式（字典 logistics_delivery_method_type；0=自提 1=送货上门 2=物流配送 3=快递）
    /// </summary>
    public int? DeliveryMethod { get; set; }

    /// <summary>
    /// 收款方式（字典 accounting_payment_method_type；0=现金 1=银行转账 2=支票 3=信用证 4=其他）
    /// </summary>
    public int? PaymentMethod { get; set; }

    /// <summary>
    /// 交货地址
    /// </summary>
    public string? DeliveryAddress { get; set; } = string.Empty;

    /// <summary>
    /// 订单状态（字典 sys_normal_disable_status；1=启用 0=禁用）
    /// </summary>
    public int? OrderStatus { get; set; }

    /// <summary>
    /// 交货状态（字典 logistics_delivery_status；0=未交货 1=部分交货 2=全部交货）
    /// </summary>
    public int? DeliveryStatus { get; set; }

    /// <summary>
    /// 销售订单明细列表（主子表关系，一个订单可以有多个明细）（子表，级联保存）
    /// </summary>
    public List<TaktSalesOrderItemCreateDto>? Items { get; set; }

    /// <summary>
    /// 销售订单变更记录列表（外键在子表 TaktSalesOrderChangeLog.SalesOrderId）（子表，级联保存）
    /// </summary>
    public List<TaktSalesOrderChangeLogCreateDto>? ChangeLogs { get; set; }

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
/// SalesOrder 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktSalesOrderExportDto
{
    /// <summary>
    /// SalesOrderID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesOrderId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 销售订单编码（唯一索引）
    /// </summary>
    public string SalesOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码（选项 TaktCustomers/options，DictValue=CustomerCode）
    /// </summary>
    public string CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户名称
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// 订单日期
    /// </summary>
    public DateTime OrderDate { get; set; }

    /// <summary>
    /// 要求交货日期
    /// </summary>
    public DateTime? RequiredDeliveryDate { get; set; }

    /// <summary>
    /// 实际交货日期
    /// </summary>
    public DateTime? ActualDeliveryDate { get; set; }

    /// <summary>
    /// 销售员（选项 TaktEmployees/options，DictValue=EmployeeCode）
    /// </summary>
    public string? SalesBy { get; set; } = string.Empty;

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
    /// 已发货数量（基本单位数量）
    /// </summary>
    public decimal ShippedQuantity { get; set; }

    /// <summary>
    /// 已发货金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal ShippedAmount { get; set; }

    /// <summary>
    /// 已收款金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal ReceivedAmount { get; set; }

    /// <summary>
    /// 交货方式（字典 logistics_delivery_method_type；0=自提 1=送货上门 2=物流配送 3=快递）
    /// </summary>
    public int DeliveryMethod { get; set; } = 0;

    /// <summary>
    /// 收款方式（字典 accounting_payment_method_type；0=现金 1=银行转账 2=支票 3=信用证 4=其他）
    /// </summary>
    public int PaymentMethod { get; set; } = 0;

    /// <summary>
    /// 交货地址
    /// </summary>
    public string? DeliveryAddress { get; set; } = string.Empty;

    /// <summary>
    /// 订单状态（字典 sys_normal_disable_status；1=启用 0=禁用）
    /// </summary>
    public int OrderStatus { get; set; } = 0;

    /// <summary>
    /// 交货状态（字典 logistics_delivery_status；0=未交货 1=部分交货 2=全部交货）
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
