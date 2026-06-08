// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.CustomerService
// 文件名称：TaktServiceOrderDtos.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：ServiceOrder 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktServiceOrder 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.CustomerService;

// ========================================
// ServiceOrder 响应 DTO
// ========================================

/// <summary>
/// 服务订单实体
/// 对应前端 TaktServiceOrderDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktServiceOrderDto : TaktCompanyDtoBase
{
    /// <summary>
    /// ServiceOrderID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ServiceOrderId { get; set; }

    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 服务订单编码（组合唯一索引）
    /// </summary>
    public string ServiceOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ClientId { get; set; }

    /// <summary>
    /// 客户端编码（冗余字段，便于查询）
    /// </summary>
    public string ClientCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户端名称（冗余字段，便于查询）
    /// </summary>
    public string ClientName { get; set; } = string.Empty;

    /// <summary>
    /// 关联服务合同ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ServiceContractId { get; set; }

    /// <summary>
    /// 关联服务合同名称（填充字段）
    /// </summary>
    public string? ServiceContractName { get; set; }

    /// <summary>
    /// 关联服务合同编码（冗余字段，便于查询）
    /// </summary>
    public string? ServiceContractCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联服务请求ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ServiceRequestId { get; set; }

    /// <summary>
    /// 关联服务请求名称（填充字段）
    /// </summary>
    public string? ServiceRequestName { get; set; }

    /// <summary>
    /// 关联服务请求单号（冗余字段，便于查询）
    /// </summary>
    public string? ServiceRequestCode { get; set; } = string.Empty;

    /// <summary>
    /// 订单日期
    /// </summary>
    public DateTime OrderDate { get; set; }

    /// <summary>
    /// 订单类型（0=现场服务，1=远程支持，2=备件更换，3=安装调试，4=其他）
    /// </summary>
    public int OrderType { get; set; } = 0;

    /// <summary>
    /// 订单状态（0=草稿，1=已确认，2=执行中，3=已完成，4=已结算，5=已取消）
    /// </summary>
    public int OrderStatus { get; set; } = 0;

    /// <summary>
    /// 订单总金额
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// 折扣金额
    /// </summary>
    public decimal DiscountAmount { get; set; }

    /// <summary>
    /// 税费
    /// </summary>
    public decimal TaxAmount { get; set; }

    /// <summary>
    /// 订单实付金额
    /// </summary>
    public decimal ActualAmount { get; set; }

    /// <summary>
    /// 结算币种代码
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划开始日期
    /// </summary>
    public DateTime? PlannedStartDate { get; set; }

    /// <summary>
    /// 计划结束日期
    /// </summary>
    public DateTime? PlannedEndDate { get; set; }

    /// <summary>
    /// 实际开始日期
    /// </summary>
    public DateTime? ActualStartDate { get; set; }

    /// <summary>
    /// 实际结束日期
    /// </summary>
    public DateTime? ActualEndDate { get; set; }

    /// <summary>
    /// 服务负责人（人员代码）
    /// </summary>
    public string? ServiceBy { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 服务工单列表（外键在子表 <see cref="TaktServiceTicket.ServiceOrderId"/>）
    /// （子表：TaktServiceTicket）
    /// </summary>
    public List<TaktServiceTicketDto>? Tickets { get; set; }

}

// ========================================
// ServiceOrder 查询 DTO
// ========================================

/// <summary>
/// ServiceOrder 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktServiceOrderQueryDto : TaktPagedQuery
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
    /// 服务订单编码（组合唯一索引）
    /// </summary>
    public string? ServiceOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ClientId { get; set; }

    /// <summary>
    /// 客户端编码（冗余字段，便于查询）
    /// </summary>
    public string? ClientCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户端名称（冗余字段，便于查询）
    /// </summary>
    public string? ClientName { get; set; } = string.Empty;

    /// <summary>
    /// 关联服务合同ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ServiceContractId { get; set; }

    /// <summary>
    /// 关联服务合同编码（冗余字段，便于查询）
    /// </summary>
    public string? ServiceContractCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联服务请求ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ServiceRequestId { get; set; }

    /// <summary>
    /// 关联服务请求单号（冗余字段，便于查询）
    /// </summary>
    public string? ServiceRequestCode { get; set; } = string.Empty;

    /// <summary>
    /// 订单日期（范围查询-开始）
    /// </summary>
    public DateTime? OrderDateStart { get; set; }

    /// <summary>
    /// 订单日期（范围查询-结束）
    /// </summary>
    public DateTime? OrderDateEnd { get; set; }

    /// <summary>
    /// 订单类型（0=现场服务，1=远程支持，2=备件更换，3=安装调试，4=其他）
    /// </summary>
    public int? OrderType { get; set; }

    /// <summary>
    /// 订单状态（0=草稿，1=已确认，2=执行中，3=已完成，4=已结算，5=已取消）
    /// </summary>
    public int? OrderStatus { get; set; }

    /// <summary>
    /// 订单总金额
    /// </summary>
    public decimal? TotalAmount { get; set; }

    /// <summary>
    /// 折扣金额
    /// </summary>
    public decimal? DiscountAmount { get; set; }

    /// <summary>
    /// 税费
    /// </summary>
    public decimal? TaxAmount { get; set; }

    /// <summary>
    /// 订单实付金额
    /// </summary>
    public decimal? ActualAmount { get; set; }

    /// <summary>
    /// 结算币种代码
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划开始日期（范围查询-开始）
    /// </summary>
    public DateTime? PlannedStartDateStart { get; set; }

    /// <summary>
    /// 计划开始日期（范围查询-结束）
    /// </summary>
    public DateTime? PlannedStartDateEnd { get; set; }

    /// <summary>
    /// 计划结束日期（范围查询-开始）
    /// </summary>
    public DateTime? PlannedEndDateStart { get; set; }

    /// <summary>
    /// 计划结束日期（范围查询-结束）
    /// </summary>
    public DateTime? PlannedEndDateEnd { get; set; }

    /// <summary>
    /// 实际开始日期（范围查询-开始）
    /// </summary>
    public DateTime? ActualStartDateStart { get; set; }

    /// <summary>
    /// 实际开始日期（范围查询-结束）
    /// </summary>
    public DateTime? ActualStartDateEnd { get; set; }

    /// <summary>
    /// 实际结束日期（范围查询-开始）
    /// </summary>
    public DateTime? ActualEndDateStart { get; set; }

    /// <summary>
    /// 实际结束日期（范围查询-结束）
    /// </summary>
    public DateTime? ActualEndDateEnd { get; set; }

    /// <summary>
    /// 服务负责人（人员代码）
    /// </summary>
    public string? ServiceBy { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（越小越靠前）
    /// </summary>
    public int? SortOrder { get; set; }

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
// 创建ServiceOrder DTO
// ========================================

/// <summary>
/// 创建ServiceOrder DTO
/// </summary>
public class TaktServiceOrderCreateDto
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
    [Required(ErrorMessage = "工厂代码不能为空")]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 服务订单编码（组合唯一索引）
    /// </summary>
    [Required(ErrorMessage = "服务订单编码（组合唯一索引）不能为空")]
    public string ServiceOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ClientId { get; set; }

    /// <summary>
    /// 客户端编码（冗余字段，便于查询）
    /// </summary>
    [Required(ErrorMessage = "客户端编码（冗余字段，便于查询）不能为空")]
    public string ClientCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户端名称（冗余字段，便于查询）
    /// </summary>
    [Required(ErrorMessage = "客户端名称（冗余字段，便于查询）不能为空")]
    public string ClientName { get; set; } = string.Empty;

    /// <summary>
    /// 关联服务合同ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ServiceContractId { get; set; }

    /// <summary>
    /// 关联服务合同编码（冗余字段，便于查询）
    /// </summary>
    public string? ServiceContractCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联服务请求ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ServiceRequestId { get; set; }

    /// <summary>
    /// 关联服务请求单号（冗余字段，便于查询）
    /// </summary>
    public string? ServiceRequestCode { get; set; } = string.Empty;

    /// <summary>
    /// 订单日期
    /// </summary>
    public DateTime OrderDate { get; set; }

    /// <summary>
    /// 订单类型（0=现场服务，1=远程支持，2=备件更换，3=安装调试，4=其他）
    /// </summary>
    public int OrderType { get; set; } = 0;

    /// <summary>
    /// 订单状态（0=草稿，1=已确认，2=执行中，3=已完成，4=已结算，5=已取消）
    /// </summary>
    public int OrderStatus { get; set; } = 0;

    /// <summary>
    /// 订单总金额
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// 折扣金额
    /// </summary>
    public decimal DiscountAmount { get; set; }

    /// <summary>
    /// 税费
    /// </summary>
    public decimal TaxAmount { get; set; }

    /// <summary>
    /// 订单实付金额
    /// </summary>
    public decimal ActualAmount { get; set; }

    /// <summary>
    /// 结算币种代码
    /// </summary>
    [Required(ErrorMessage = "结算币种代码不能为空")]
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划开始日期
    /// </summary>
    public DateTime? PlannedStartDate { get; set; }

    /// <summary>
    /// 计划结束日期
    /// </summary>
    public DateTime? PlannedEndDate { get; set; }

    /// <summary>
    /// 实际开始日期
    /// </summary>
    public DateTime? ActualStartDate { get; set; }

    /// <summary>
    /// 实际结束日期
    /// </summary>
    public DateTime? ActualEndDate { get; set; }

    /// <summary>
    /// 服务负责人（人员代码）
    /// </summary>
    public string? ServiceBy { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 服务工单列表（外键在子表 <see cref="TaktServiceTicket.ServiceOrderId"/>）（子表，级联保存）
    /// </summary>
    public List<TaktServiceTicketCreateDto>? Tickets { get; set; }

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
// 更新ServiceOrder DTO
// ========================================

/// <summary>
/// 更新ServiceOrder DTO
/// 继承 TaktServiceOrderCreateDto，添加 ServiceOrderId 字段
/// </summary>
public class TaktServiceOrderUpdateDto : TaktServiceOrderCreateDto
{
    /// <summary>
    /// ServiceOrderID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ServiceOrderId { get; set; }

}

// ========================================
// ServiceOrder 状态 DTO
// ========================================

/// <summary>
/// ServiceOrder 状态更新 DTO
/// </summary>
public class TaktServiceOrderStatusDto
{
    /// <summary>
    /// ServiceOrderID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ServiceOrderId { get; set; }

    /// <summary>
    /// 订单状态（0=草稿，1=已确认，2=执行中，3=已完成，4=已结算，5=已取消）
    /// </summary>
    [Required(ErrorMessage = "订单状态（0=草稿，1=已确认，2=执行中，3=已完成，4=已结算，5=已取消）不能为空")]
    public int OrderStatus { get; set; } = 0;
}

// ========================================
// ServiceOrder 排序 DTO
// ========================================

/// <summary>
/// ServiceOrder 排序更新 DTO
/// </summary>
public class TaktServiceOrderSortDto
{
    /// <summary>
    /// ServiceOrderID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ServiceOrderId { get; set; }

    /// <summary>
    /// 排序号（越小越靠前）
    /// </summary>
    [Required(ErrorMessage = "排序号（越小越靠前）不能为空")]
    public int SortOrder { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// ServiceOrder 导入模板行 DTO
/// </summary>
public class TaktServiceOrderTemplateDto
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
    /// 服务订单编码（组合唯一索引）
    /// </summary>
    public string? ServiceOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ClientId { get; set; }

    /// <summary>
    /// 客户端编码（冗余字段，便于查询）
    /// </summary>
    public string? ClientCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户端名称（冗余字段，便于查询）
    /// </summary>
    public string? ClientName { get; set; } = string.Empty;

    /// <summary>
    /// 关联服务合同ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ServiceContractId { get; set; }

    /// <summary>
    /// 关联服务合同编码（冗余字段，便于查询）
    /// </summary>
    public string? ServiceContractCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联服务请求ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ServiceRequestId { get; set; }

    /// <summary>
    /// 关联服务请求单号（冗余字段，便于查询）
    /// </summary>
    public string? ServiceRequestCode { get; set; } = string.Empty;

    /// <summary>
    /// 订单类型（0=现场服务，1=远程支持，2=备件更换，3=安装调试，4=其他）
    /// </summary>
    public int? OrderType { get; set; }

    /// <summary>
    /// 订单状态（0=草稿，1=已确认，2=执行中，3=已完成，4=已结算，5=已取消）
    /// </summary>
    public int? OrderStatus { get; set; }

    /// <summary>
    /// 结算币种代码
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

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
/// ServiceOrder 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktServiceOrderImportDto
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
    /// 服务订单编码（组合唯一索引）
    /// </summary>
    public string? ServiceOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ClientId { get; set; }

    /// <summary>
    /// 客户端编码（冗余字段，便于查询）
    /// </summary>
    public string? ClientCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户端名称（冗余字段，便于查询）
    /// </summary>
    public string? ClientName { get; set; } = string.Empty;

    /// <summary>
    /// 关联服务合同ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ServiceContractId { get; set; }

    /// <summary>
    /// 关联服务合同编码（冗余字段，便于查询）
    /// </summary>
    public string? ServiceContractCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联服务请求ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ServiceRequestId { get; set; }

    /// <summary>
    /// 关联服务请求单号（冗余字段，便于查询）
    /// </summary>
    public string? ServiceRequestCode { get; set; } = string.Empty;

    /// <summary>
    /// 订单类型（0=现场服务，1=远程支持，2=备件更换，3=安装调试，4=其他）
    /// </summary>
    public int? OrderType { get; set; }

    /// <summary>
    /// 订单状态（0=草稿，1=已确认，2=执行中，3=已完成，4=已结算，5=已取消）
    /// </summary>
    public int? OrderStatus { get; set; }

    /// <summary>
    /// 结算币种代码
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

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
/// ServiceOrder 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktServiceOrderExportDto
{
    /// <summary>
    /// ServiceOrderID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ServiceOrderId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 服务订单编码（组合唯一索引）
    /// </summary>
    public string ServiceOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ClientId { get; set; }

    /// <summary>
    /// 客户端编码（冗余字段，便于查询）
    /// </summary>
    public string ClientCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户端名称（冗余字段，便于查询）
    /// </summary>
    public string ClientName { get; set; } = string.Empty;

    /// <summary>
    /// 关联服务合同ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ServiceContractId { get; set; }

    /// <summary>
    /// 关联服务合同编码（冗余字段，便于查询）
    /// </summary>
    public string? ServiceContractCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联服务请求ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ServiceRequestId { get; set; }

    /// <summary>
    /// 关联服务请求单号（冗余字段，便于查询）
    /// </summary>
    public string? ServiceRequestCode { get; set; } = string.Empty;

    /// <summary>
    /// 订单日期
    /// </summary>
    public DateTime OrderDate { get; set; }

    /// <summary>
    /// 订单类型（0=现场服务，1=远程支持，2=备件更换，3=安装调试，4=其他）
    /// </summary>
    public int OrderType { get; set; } = 0;

    /// <summary>
    /// 订单状态（0=草稿，1=已确认，2=执行中，3=已完成，4=已结算，5=已取消）
    /// </summary>
    public int OrderStatus { get; set; } = 0;

    /// <summary>
    /// 订单总金额
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// 折扣金额
    /// </summary>
    public decimal DiscountAmount { get; set; }

    /// <summary>
    /// 税费
    /// </summary>
    public decimal TaxAmount { get; set; }

    /// <summary>
    /// 订单实付金额
    /// </summary>
    public decimal ActualAmount { get; set; }

    /// <summary>
    /// 结算币种代码
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划开始日期
    /// </summary>
    public DateTime? PlannedStartDate { get; set; }

    /// <summary>
    /// 计划结束日期
    /// </summary>
    public DateTime? PlannedEndDate { get; set; }

    /// <summary>
    /// 实际开始日期
    /// </summary>
    public DateTime? ActualStartDate { get; set; }

    /// <summary>
    /// 实际结束日期
    /// </summary>
    public DateTime? ActualEndDate { get; set; }

    /// <summary>
    /// 服务负责人（人员代码）
    /// </summary>
    public string? ServiceBy { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

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
