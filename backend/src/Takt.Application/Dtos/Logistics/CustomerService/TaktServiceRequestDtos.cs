// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.CustomerService
// 文件名称：TaktServiceRequestDtos.cs
// 创建时间：2026-07-23
// 创建人：Takt365(Auto Generated)
// 功能描述：ServiceRequest 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktServiceRequest 生成，请按需审阅）
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
// ServiceRequest 响应 DTO
// ========================================

/// <summary>
/// 服务请求实体
/// 对应前端 TaktServiceRequestDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktServiceRequestDto : TaktCompanyDtoBase
{
    /// <summary>
    /// ServiceRequestID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ServiceRequestId { get; set; }

    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 服务请求单号（组合唯一索引）
    /// </summary>
    public string ServiceRequestCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ClientId { get; set; }

    /// <summary>
    /// 客户端名称（填充字段）
    /// </summary>
    public string? ClientName { get; set; }

    /// <summary>
    /// 客户端编码（冗余字段，便于查询）
    /// </summary>
    public string ClientCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户端名称（冗余字段，便于查询）
    /// </summary>
    public string ClientName1 { get; set; } = string.Empty;

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
    /// 请求日期
    /// </summary>
    public DateTime RequestDate { get; set; }

    /// <summary>
    /// 期望服务日期
    /// </summary>
    public DateTime? ExpectedServiceDate { get; set; }

    /// <summary>
    /// 请求类型（0=咨询，1=报修，2=投诉，3=安装，4=巡检，5=其他）
    /// </summary>
    public int RequestType { get; set; } = 0;

    /// <summary>
    /// 请求来源（0=电话，1=邮件，2=门户，3=现场，4=其他）
    /// </summary>
    public int SourceChannel { get; set; } = 0;

    /// <summary>
    /// 优先级（字典 sys_priority_level_category）
    /// </summary>
    public int Priority { get; set; } = 0;

    /// <summary>
    /// 请求状态（0=草稿，1=已提交，2=处理中，3=已完成，4=已关闭，5=已取消）
    /// </summary>
    public int RequestStatus { get; set; } = 0;

    /// <summary>
    /// 请求主题
    /// </summary>
    public string RequestSubject { get; set; } = string.Empty;

    /// <summary>
    /// 请求描述
    /// </summary>
    public string RequestDescription { get; set; } = string.Empty;

    /// <summary>
    /// 联系人
    /// </summary>
    public string? ContactPerson { get; set; } = string.Empty;

    /// <summary>
    /// 联系电话
    /// </summary>
    public string? ContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// 联系邮箱
    /// </summary>
    public string? ContactEmail { get; set; } = string.Empty;

    /// <summary>
    /// 服务地址
    /// </summary>
    public string? ServiceAddress { get; set; } = string.Empty;

    /// <summary>
    /// 受理人员工ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AssignedEmployeeId { get; set; }

    /// <summary>
    /// 受理人姓名
    /// </summary>
    public string? AssignedEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 受理时间
    /// </summary>
    public DateTime? AssignedAt { get; set; }

    /// <summary>
    /// 关闭时间
    /// </summary>
    public DateTime? ClosedAt { get; set; }

    /// <summary>
    /// 排序号（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 关联服务合同
    /// （主表：TaktServiceContract）
    /// </summary>
    public TaktServiceContractDto? ServiceContract { get; set; }

    /// <summary>
    /// 关联服务订单列表（外键在子表 TaktServiceOrder.ServiceRequestId）
    /// （子表：TaktServiceOrder）
    /// </summary>
    public List<TaktServiceOrderDto>? ServiceOrders { get; set; }

    /// <summary>
    /// 服务工单列表（外键在子表 TaktServiceTicket.ServiceRequestId）
    /// （子表：TaktServiceTicket）
    /// </summary>
    public List<TaktServiceTicketDto>? Tickets { get; set; }

}

// ========================================
// ServiceRequest 查询 DTO
// ========================================

/// <summary>
/// ServiceRequest 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktServiceRequestQueryDto : TaktPagedQuery
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
    /// 服务请求单号（组合唯一索引）
    /// </summary>
    public string? ServiceRequestCode { get; set; } = string.Empty;

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
    public string? ClientName1 { get; set; } = string.Empty;

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
    /// 请求日期（范围查询-开始）
    /// </summary>
    public DateTime? RequestDateStart { get; set; }

    /// <summary>
    /// 请求日期（范围查询-结束）
    /// </summary>
    public DateTime? RequestDateEnd { get; set; }

    /// <summary>
    /// 期望服务日期（范围查询-开始）
    /// </summary>
    public DateTime? ExpectedServiceDateStart { get; set; }

    /// <summary>
    /// 期望服务日期（范围查询-结束）
    /// </summary>
    public DateTime? ExpectedServiceDateEnd { get; set; }

    /// <summary>
    /// 请求类型（0=咨询，1=报修，2=投诉，3=安装，4=巡检，5=其他）
    /// </summary>
    public int? RequestType { get; set; }

    /// <summary>
    /// 请求来源（0=电话，1=邮件，2=门户，3=现场，4=其他）
    /// </summary>
    public int? SourceChannel { get; set; }

    /// <summary>
    /// 优先级（字典 sys_priority_level_category）
    /// </summary>
    public int? Priority { get; set; }

    /// <summary>
    /// 请求状态（0=草稿，1=已提交，2=处理中，3=已完成，4=已关闭，5=已取消）
    /// </summary>
    public int? RequestStatus { get; set; }

    /// <summary>
    /// 请求主题
    /// </summary>
    public string? RequestSubject { get; set; } = string.Empty;

    /// <summary>
    /// 请求描述
    /// </summary>
    public string? RequestDescription { get; set; } = string.Empty;

    /// <summary>
    /// 联系人
    /// </summary>
    public string? ContactPerson { get; set; } = string.Empty;

    /// <summary>
    /// 联系电话
    /// </summary>
    public string? ContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// 联系邮箱
    /// </summary>
    public string? ContactEmail { get; set; } = string.Empty;

    /// <summary>
    /// 服务地址
    /// </summary>
    public string? ServiceAddress { get; set; } = string.Empty;

    /// <summary>
    /// 受理人员工ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AssignedEmployeeId { get; set; }

    /// <summary>
    /// 受理人姓名
    /// </summary>
    public string? AssignedEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 受理时间（范围查询-开始）
    /// </summary>
    public DateTime? AssignedAtStart { get; set; }

    /// <summary>
    /// 受理时间（范围查询-结束）
    /// </summary>
    public DateTime? AssignedAtEnd { get; set; }

    /// <summary>
    /// 关闭时间（范围查询-开始）
    /// </summary>
    public DateTime? ClosedAtStart { get; set; }

    /// <summary>
    /// 关闭时间（范围查询-结束）
    /// </summary>
    public DateTime? ClosedAtEnd { get; set; }

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
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注（模糊查询）
    /// </summary>
    public string? Remark { get; set; }
}

// ========================================
// 创建ServiceRequest DTO
// ========================================

/// <summary>
/// 创建ServiceRequest DTO
/// </summary>
public class TaktServiceRequestCreateDto
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
    /// 工厂代码
    /// </summary>
    [Required(ErrorMessage = "工厂代码不能为空")]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 服务请求单号（组合唯一索引）
    /// </summary>
    [Required(ErrorMessage = "服务请求单号（组合唯一索引）不能为空")]
    public string ServiceRequestCode { get; set; } = string.Empty;

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
    public string ClientName1 { get; set; } = string.Empty;

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
    /// 请求日期
    /// </summary>
    public DateTime RequestDate { get; set; }

    /// <summary>
    /// 期望服务日期
    /// </summary>
    public DateTime? ExpectedServiceDate { get; set; }

    /// <summary>
    /// 请求类型（0=咨询，1=报修，2=投诉，3=安装，4=巡检，5=其他）
    /// </summary>
    public int RequestType { get; set; } = 0;

    /// <summary>
    /// 请求来源（0=电话，1=邮件，2=门户，3=现场，4=其他）
    /// </summary>
    public int SourceChannel { get; set; } = 0;

    /// <summary>
    /// 优先级（字典 sys_priority_level_category）
    /// </summary>
    public int Priority { get; set; } = 0;

    /// <summary>
    /// 请求状态（0=草稿，1=已提交，2=处理中，3=已完成，4=已关闭，5=已取消）
    /// </summary>
    public int RequestStatus { get; set; } = 0;

    /// <summary>
    /// 请求主题
    /// </summary>
    [Required(ErrorMessage = "请求主题不能为空")]
    public string RequestSubject { get; set; } = string.Empty;

    /// <summary>
    /// 请求描述
    /// </summary>
    [Required(ErrorMessage = "请求描述不能为空")]
    public string RequestDescription { get; set; } = string.Empty;

    /// <summary>
    /// 联系人
    /// </summary>
    public string? ContactPerson { get; set; } = string.Empty;

    /// <summary>
    /// 联系电话
    /// </summary>
    public string? ContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// 联系邮箱
    /// </summary>
    public string? ContactEmail { get; set; } = string.Empty;

    /// <summary>
    /// 服务地址
    /// </summary>
    public string? ServiceAddress { get; set; } = string.Empty;

    /// <summary>
    /// 受理人员工ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AssignedEmployeeId { get; set; }

    /// <summary>
    /// 受理人姓名
    /// </summary>
    public string? AssignedEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 受理时间
    /// </summary>
    public DateTime? AssignedAt { get; set; }

    /// <summary>
    /// 关闭时间
    /// </summary>
    public DateTime? ClosedAt { get; set; }

    /// <summary>
    /// 关联服务订单列表（外键在子表 TaktServiceOrder.ServiceRequestId）（子表，级联保存）
    /// </summary>
    public List<TaktServiceOrderCreateDto>? ServiceOrders { get; set; }

    /// <summary>
    /// 服务工单列表（外键在子表 TaktServiceTicket.ServiceRequestId）（子表，级联保存）
    /// </summary>
    public List<TaktServiceTicketCreateDto>? Tickets { get; set; }

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
// 更新ServiceRequest DTO
// ========================================

/// <summary>
/// 更新ServiceRequest DTO
/// 继承 TaktServiceRequestCreateDto，添加 ServiceRequestId 字段
/// </summary>
public class TaktServiceRequestUpdateDto : TaktServiceRequestCreateDto
{
    /// <summary>
    /// ServiceRequestID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ServiceRequestId { get; set; }

    /// <summary>
    /// 关联服务订单列表（外键在子表 TaktServiceOrder.ServiceRequestId）（子表，级联保存）
    /// </summary>
    public new List<TaktServiceOrderUpdateDto>? ServiceOrders { get; set; }

    /// <summary>
    /// 服务工单列表（外键在子表 TaktServiceTicket.ServiceRequestId）（子表，级联保存）
    /// </summary>
    public new List<TaktServiceTicketUpdateDto>? Tickets { get; set; }

}

// ========================================
// ServiceRequest 状态 DTO
// ========================================

/// <summary>
/// ServiceRequest 状态更新 DTO
/// </summary>
public class TaktServiceRequestStatusDto
{
    /// <summary>
    /// ServiceRequestID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ServiceRequestId { get; set; }

    /// <summary>
    /// 请求状态（0=草稿，1=已提交，2=处理中，3=已完成，4=已关闭，5=已取消）
    /// </summary>
    [Required(ErrorMessage = "请求状态（0=草稿，1=已提交，2=处理中，3=已完成，4=已关闭，5=已取消）不能为空")]
    public int RequestStatus { get; set; } = 0;
}

// ========================================
// ServiceRequest 排序 DTO
// ========================================

/// <summary>
/// ServiceRequest 排序更新 DTO
/// </summary>
public class TaktServiceRequestSortDto
{
    /// <summary>
    /// ServiceRequestID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ServiceRequestId { get; set; }

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
/// ServiceRequest 导入模板行 DTO
/// </summary>
public class TaktServiceRequestTemplateDto
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
    /// 服务请求单号（组合唯一索引）
    /// </summary>
    public string? ServiceRequestCode { get; set; } = string.Empty;

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
    public string? ClientName1 { get; set; } = string.Empty;

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
    /// 请求日期
    /// </summary>
    public DateTime? RequestDate { get; set; }

    /// <summary>
    /// 期望服务日期
    /// </summary>
    public DateTime? ExpectedServiceDate { get; set; }

    /// <summary>
    /// 请求类型（0=咨询，1=报修，2=投诉，3=安装，4=巡检，5=其他）
    /// </summary>
    public int? RequestType { get; set; }

    /// <summary>
    /// 请求来源（0=电话，1=邮件，2=门户，3=现场，4=其他）
    /// </summary>
    public int? SourceChannel { get; set; }

    /// <summary>
    /// 优先级（字典 sys_priority_level_category）
    /// </summary>
    public int? Priority { get; set; }

    /// <summary>
    /// 请求状态（0=草稿，1=已提交，2=处理中，3=已完成，4=已关闭，5=已取消）
    /// </summary>
    public int? RequestStatus { get; set; }

    /// <summary>
    /// 请求主题
    /// </summary>
    public string? RequestSubject { get; set; } = string.Empty;

    /// <summary>
    /// 请求描述
    /// </summary>
    public string? RequestDescription { get; set; } = string.Empty;

    /// <summary>
    /// 联系人
    /// </summary>
    public string? ContactPerson { get; set; } = string.Empty;

    /// <summary>
    /// 联系电话
    /// </summary>
    public string? ContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// 联系邮箱
    /// </summary>
    public string? ContactEmail { get; set; } = string.Empty;

    /// <summary>
    /// 服务地址
    /// </summary>
    public string? ServiceAddress { get; set; } = string.Empty;

    /// <summary>
    /// 受理人员工ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AssignedEmployeeId { get; set; }

    /// <summary>
    /// 受理人姓名
    /// </summary>
    public string? AssignedEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 受理时间
    /// </summary>
    public DateTime? AssignedAt { get; set; }

    /// <summary>
    /// 关闭时间
    /// </summary>
    public DateTime? ClosedAt { get; set; }

    /// <summary>
    /// 关联服务订单列表（外键在子表 TaktServiceOrder.ServiceRequestId）（子表，级联保存）
    /// </summary>
    public List<TaktServiceOrderCreateDto>? ServiceOrders { get; set; }

    /// <summary>
    /// 服务工单列表（外键在子表 TaktServiceTicket.ServiceRequestId）（子表，级联保存）
    /// </summary>
    public List<TaktServiceTicketCreateDto>? Tickets { get; set; }

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
/// ServiceRequest 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktServiceRequestImportDto
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
    /// 工厂代码
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 服务请求单号（组合唯一索引）
    /// </summary>
    public string? ServiceRequestCode { get; set; } = string.Empty;

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
    public string? ClientName1 { get; set; } = string.Empty;

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
    /// 请求日期
    /// </summary>
    public DateTime? RequestDate { get; set; }

    /// <summary>
    /// 期望服务日期
    /// </summary>
    public DateTime? ExpectedServiceDate { get; set; }

    /// <summary>
    /// 请求类型（0=咨询，1=报修，2=投诉，3=安装，4=巡检，5=其他）
    /// </summary>
    public int? RequestType { get; set; }

    /// <summary>
    /// 请求来源（0=电话，1=邮件，2=门户，3=现场，4=其他）
    /// </summary>
    public int? SourceChannel { get; set; }

    /// <summary>
    /// 优先级（字典 sys_priority_level_category）
    /// </summary>
    public int? Priority { get; set; }

    /// <summary>
    /// 请求状态（0=草稿，1=已提交，2=处理中，3=已完成，4=已关闭，5=已取消）
    /// </summary>
    public int? RequestStatus { get; set; }

    /// <summary>
    /// 请求主题
    /// </summary>
    public string? RequestSubject { get; set; } = string.Empty;

    /// <summary>
    /// 请求描述
    /// </summary>
    public string? RequestDescription { get; set; } = string.Empty;

    /// <summary>
    /// 联系人
    /// </summary>
    public string? ContactPerson { get; set; } = string.Empty;

    /// <summary>
    /// 联系电话
    /// </summary>
    public string? ContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// 联系邮箱
    /// </summary>
    public string? ContactEmail { get; set; } = string.Empty;

    /// <summary>
    /// 服务地址
    /// </summary>
    public string? ServiceAddress { get; set; } = string.Empty;

    /// <summary>
    /// 受理人员工ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AssignedEmployeeId { get; set; }

    /// <summary>
    /// 受理人姓名
    /// </summary>
    public string? AssignedEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 受理时间
    /// </summary>
    public DateTime? AssignedAt { get; set; }

    /// <summary>
    /// 关闭时间
    /// </summary>
    public DateTime? ClosedAt { get; set; }

    /// <summary>
    /// 关联服务订单列表（外键在子表 TaktServiceOrder.ServiceRequestId）（子表，级联保存）
    /// </summary>
    public List<TaktServiceOrderCreateDto>? ServiceOrders { get; set; }

    /// <summary>
    /// 服务工单列表（外键在子表 TaktServiceTicket.ServiceRequestId）（子表，级联保存）
    /// </summary>
    public List<TaktServiceTicketCreateDto>? Tickets { get; set; }

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
/// ServiceRequest 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktServiceRequestExportDto
{
    /// <summary>
    /// ServiceRequestID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ServiceRequestId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 服务请求单号（组合唯一索引）
    /// </summary>
    public string ServiceRequestCode { get; set; } = string.Empty;

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
    public string ClientName1 { get; set; } = string.Empty;

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
    /// 请求日期
    /// </summary>
    public DateTime RequestDate { get; set; }

    /// <summary>
    /// 期望服务日期
    /// </summary>
    public DateTime? ExpectedServiceDate { get; set; }

    /// <summary>
    /// 请求类型（0=咨询，1=报修，2=投诉，3=安装，4=巡检，5=其他）
    /// </summary>
    public int RequestType { get; set; } = 0;

    /// <summary>
    /// 请求来源（0=电话，1=邮件，2=门户，3=现场，4=其他）
    /// </summary>
    public int SourceChannel { get; set; } = 0;

    /// <summary>
    /// 优先级（字典 sys_priority_level_category）
    /// </summary>
    public int Priority { get; set; } = 0;

    /// <summary>
    /// 请求状态（0=草稿，1=已提交，2=处理中，3=已完成，4=已关闭，5=已取消）
    /// </summary>
    public int RequestStatus { get; set; } = 0;

    /// <summary>
    /// 请求主题
    /// </summary>
    public string RequestSubject { get; set; } = string.Empty;

    /// <summary>
    /// 请求描述
    /// </summary>
    public string RequestDescription { get; set; } = string.Empty;

    /// <summary>
    /// 联系人
    /// </summary>
    public string? ContactPerson { get; set; } = string.Empty;

    /// <summary>
    /// 联系电话
    /// </summary>
    public string? ContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// 联系邮箱
    /// </summary>
    public string? ContactEmail { get; set; } = string.Empty;

    /// <summary>
    /// 服务地址
    /// </summary>
    public string? ServiceAddress { get; set; } = string.Empty;

    /// <summary>
    /// 受理人员工ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AssignedEmployeeId { get; set; }

    /// <summary>
    /// 受理人姓名
    /// </summary>
    public string? AssignedEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 受理时间
    /// </summary>
    public DateTime? AssignedAt { get; set; }

    /// <summary>
    /// 关闭时间
    /// </summary>
    public DateTime? ClosedAt { get; set; }

    /// <summary>
    /// 排序号（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

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
