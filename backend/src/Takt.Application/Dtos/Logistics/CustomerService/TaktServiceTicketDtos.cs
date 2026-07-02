// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.CustomerService
// 文件名称：TaktServiceTicketDtos.cs
// 创建时间：2026-06-24
// 创建人：Takt365(Auto Generated)
// 功能描述：ServiceTicket 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktServiceTicket 生成，请按需审阅）
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
// ServiceTicket 响应 DTO
// ========================================

/// <summary>
/// 服务工单实体
/// 对应前端 TaktServiceTicketDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktServiceTicketDto : TaktCompanyDtoBase
{
    /// <summary>
    /// ServiceTicketID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ServiceTicketId { get; set; }

    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 服务工单编码（组合唯一索引）
    /// </summary>
    public string ServiceTicketCode { get; set; } = string.Empty;

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
    /// 关联服务订单ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ServiceOrderId { get; set; }

    /// <summary>
    /// 关联服务订单名称（填充字段）
    /// </summary>
    public string? ServiceOrderName { get; set; }

    /// <summary>
    /// 关联服务订单编码（冗余字段，便于查询）
    /// </summary>
    public string? ServiceOrderCode { get; set; } = string.Empty;

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
    /// 工单类型（0=维修，1=巡检，2=安装，3=升级，4=其他）
    /// </summary>
    public int TicketType { get; set; } = 0;

    /// <summary>
    /// 优先级（字典 sys_priority_level_category）
    /// </summary>
    public int Priority { get; set; } = 0;

    /// <summary>
    /// 工单主题
    /// </summary>
    public string TicketSubject { get; set; } = string.Empty;

    /// <summary>
    /// 故障/问题描述
    /// </summary>
    public string? FaultDescription { get; set; } = string.Empty;

    /// <summary>
    /// 处理方案/解决说明
    /// </summary>
    public string? SolutionDescription { get; set; } = string.Empty;

    /// <summary>
    /// 服务地点
    /// </summary>
    public string? ServiceLocation { get; set; } = string.Empty;

    /// <summary>
    /// 指派服务人员工ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AssignedEmployeeId { get; set; }

    /// <summary>
    /// 指派服务人员姓名
    /// </summary>
    public string? AssignedEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 计划开始时间
    /// </summary>
    public DateTime? ScheduledStartTime { get; set; }

    /// <summary>
    /// 计划结束时间
    /// </summary>
    public DateTime? ScheduledEndTime { get; set; }

    /// <summary>
    /// 实际开始时间
    /// </summary>
    public DateTime? ActualStartTime { get; set; }

    /// <summary>
    /// 实际结束时间
    /// </summary>
    public DateTime? ActualEndTime { get; set; }

    /// <summary>
    /// 验收结果（0=不合格，1=合格，2=部分合格）
    /// </summary>
    public int? AcceptanceResult { get; set; }

    /// <summary>
    /// 验收人
    /// </summary>
    public string? AcceptedBy { get; set; } = string.Empty;

    /// <summary>
    /// 验收时间
    /// </summary>
    public DateTime? AcceptedAt { get; set; }

    /// <summary>
    /// 排序号（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 工单状态（字典 sys_ticket_status；0=新建，1=已分配，2=处理中，3=待确认，4=已完成，5=已关闭，6=已取消）
    /// </summary>
    public int TicketStatus { get; set; } = 0;

    /// <summary>
    /// 关联服务请求
    /// （主表：TaktServiceRequest）
    /// </summary>
    public TaktServiceRequestDto? ServiceRequest { get; set; }

    /// <summary>
    /// 关联服务订单
    /// （主表：TaktServiceOrder）
    /// </summary>
    public TaktServiceOrderDto? ServiceOrder { get; set; }

    /// <summary>
    /// 关联服务合同
    /// （主表：TaktServiceContract）
    /// </summary>
    public TaktServiceContractDto? ServiceContract { get; set; }

}

// ========================================
// ServiceTicket 查询 DTO
// ========================================

/// <summary>
/// ServiceTicket 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktServiceTicketQueryDto : TaktPagedQuery
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
    /// 服务工单编码（组合唯一索引）
    /// </summary>
    public string? ServiceTicketCode { get; set; } = string.Empty;

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
    /// 关联服务请求ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ServiceRequestId { get; set; }

    /// <summary>
    /// 关联服务请求单号（冗余字段，便于查询）
    /// </summary>
    public string? ServiceRequestCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联服务订单ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ServiceOrderId { get; set; }

    /// <summary>
    /// 关联服务订单编码（冗余字段，便于查询）
    /// </summary>
    public string? ServiceOrderCode { get; set; } = string.Empty;

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
    /// 工单类型（0=维修，1=巡检，2=安装，3=升级，4=其他）
    /// </summary>
    public int? TicketType { get; set; }

    /// <summary>
    /// 优先级（字典 sys_priority_level_category）
    /// </summary>
    public int? Priority { get; set; }

    /// <summary>
    /// 工单主题
    /// </summary>
    public string? TicketSubject { get; set; } = string.Empty;

    /// <summary>
    /// 故障/问题描述
    /// </summary>
    public string? FaultDescription { get; set; } = string.Empty;

    /// <summary>
    /// 处理方案/解决说明
    /// </summary>
    public string? SolutionDescription { get; set; } = string.Empty;

    /// <summary>
    /// 服务地点
    /// </summary>
    public string? ServiceLocation { get; set; } = string.Empty;

    /// <summary>
    /// 指派服务人员工ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AssignedEmployeeId { get; set; }

    /// <summary>
    /// 指派服务人员姓名
    /// </summary>
    public string? AssignedEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 计划开始时间（范围查询-开始）
    /// </summary>
    public DateTime? ScheduledStartTimeStart { get; set; }

    /// <summary>
    /// 计划开始时间（范围查询-结束）
    /// </summary>
    public DateTime? ScheduledStartTimeEnd { get; set; }

    /// <summary>
    /// 计划结束时间（范围查询-开始）
    /// </summary>
    public DateTime? ScheduledEndTimeStart { get; set; }

    /// <summary>
    /// 计划结束时间（范围查询-结束）
    /// </summary>
    public DateTime? ScheduledEndTimeEnd { get; set; }

    /// <summary>
    /// 实际开始时间（范围查询-开始）
    /// </summary>
    public DateTime? ActualStartTimeStart { get; set; }

    /// <summary>
    /// 实际开始时间（范围查询-结束）
    /// </summary>
    public DateTime? ActualStartTimeEnd { get; set; }

    /// <summary>
    /// 实际结束时间（范围查询-开始）
    /// </summary>
    public DateTime? ActualEndTimeStart { get; set; }

    /// <summary>
    /// 实际结束时间（范围查询-结束）
    /// </summary>
    public DateTime? ActualEndTimeEnd { get; set; }

    /// <summary>
    /// 验收结果（0=不合格，1=合格，2=部分合格）
    /// </summary>
    public int? AcceptanceResult { get; set; }

    /// <summary>
    /// 验收人
    /// </summary>
    public string? AcceptedBy { get; set; } = string.Empty;

    /// <summary>
    /// 验收时间（范围查询-开始）
    /// </summary>
    public DateTime? AcceptedAtStart { get; set; }

    /// <summary>
    /// 验收时间（范围查询-结束）
    /// </summary>
    public DateTime? AcceptedAtEnd { get; set; }

    /// <summary>
    /// 排序号（越小越靠前）
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 工单状态（字典 sys_ticket_status；0=新建，1=已分配，2=处理中，3=待确认，4=已完成，5=已关闭，6=已取消）
    /// </summary>
    public int? TicketStatus { get; set; }

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
// 创建ServiceTicket DTO
// ========================================

/// <summary>
/// 创建ServiceTicket DTO
/// </summary>
public class TaktServiceTicketCreateDto
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
    /// 服务工单编码（组合唯一索引）
    /// </summary>
    [Required(ErrorMessage = "服务工单编码（组合唯一索引）不能为空")]
    public string ServiceTicketCode { get; set; } = string.Empty;

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
    /// 关联服务请求ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ServiceRequestId { get; set; }

    /// <summary>
    /// 关联服务请求单号（冗余字段，便于查询）
    /// </summary>
    public string? ServiceRequestCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联服务订单ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ServiceOrderId { get; set; }

    /// <summary>
    /// 关联服务订单编码（冗余字段，便于查询）
    /// </summary>
    public string? ServiceOrderCode { get; set; } = string.Empty;

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
    /// 工单类型（0=维修，1=巡检，2=安装，3=升级，4=其他）
    /// </summary>
    public int TicketType { get; set; } = 0;

    /// <summary>
    /// 优先级（字典 sys_priority_level_category）
    /// </summary>
    public int Priority { get; set; } = 0;

    /// <summary>
    /// 工单主题
    /// </summary>
    [Required(ErrorMessage = "工单主题不能为空")]
    public string TicketSubject { get; set; } = string.Empty;

    /// <summary>
    /// 故障/问题描述
    /// </summary>
    public string? FaultDescription { get; set; } = string.Empty;

    /// <summary>
    /// 处理方案/解决说明
    /// </summary>
    public string? SolutionDescription { get; set; } = string.Empty;

    /// <summary>
    /// 服务地点
    /// </summary>
    public string? ServiceLocation { get; set; } = string.Empty;

    /// <summary>
    /// 指派服务人员工ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AssignedEmployeeId { get; set; }

    /// <summary>
    /// 指派服务人员姓名
    /// </summary>
    public string? AssignedEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 计划开始时间
    /// </summary>
    public DateTime? ScheduledStartTime { get; set; }

    /// <summary>
    /// 计划结束时间
    /// </summary>
    public DateTime? ScheduledEndTime { get; set; }

    /// <summary>
    /// 实际开始时间
    /// </summary>
    public DateTime? ActualStartTime { get; set; }

    /// <summary>
    /// 实际结束时间
    /// </summary>
    public DateTime? ActualEndTime { get; set; }

    /// <summary>
    /// 验收结果（0=不合格，1=合格，2=部分合格）
    /// </summary>
    public int? AcceptanceResult { get; set; }

    /// <summary>
    /// 验收人
    /// </summary>
    public string? AcceptedBy { get; set; } = string.Empty;

    /// <summary>
    /// 验收时间
    /// </summary>
    public DateTime? AcceptedAt { get; set; }

    /// <summary>
    /// 工单状态（字典 sys_ticket_status；0=新建，1=已分配，2=处理中，3=待确认，4=已完成，5=已关闭，6=已取消）
    /// </summary>
    public int TicketStatus { get; set; } = 0;

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
// 更新ServiceTicket DTO
// ========================================

/// <summary>
/// 更新ServiceTicket DTO
/// 继承 TaktServiceTicketCreateDto，添加 ServiceTicketId 字段
/// </summary>
public class TaktServiceTicketUpdateDto : TaktServiceTicketCreateDto
{
    /// <summary>
    /// ServiceTicketID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ServiceTicketId { get; set; }

}

// ========================================
// ServiceTicket 状态 DTO
// ========================================

/// <summary>
/// ServiceTicket 状态更新 DTO
/// </summary>
public class TaktServiceTicketStatusDto
{
    /// <summary>
    /// ServiceTicketID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ServiceTicketId { get; set; }

    /// <summary>
    /// 工单状态（字典 sys_ticket_status；0=新建，1=已分配，2=处理中，3=待确认，4=已完成，5=已关闭，6=已取消）
    /// </summary>
    [Required(ErrorMessage = "工单状态（字典 sys_ticket_status；0=新建，1=已分配，2=处理中，3=待确认，4=已完成，5=已关闭，6=已取消）不能为空")]
    public int TicketStatus { get; set; } = 0;
}

// ========================================
// ServiceTicket 排序 DTO
// ========================================

/// <summary>
/// ServiceTicket 排序更新 DTO
/// </summary>
public class TaktServiceTicketSortDto
{
    /// <summary>
    /// ServiceTicketID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ServiceTicketId { get; set; }

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
/// ServiceTicket 导入模板行 DTO
/// </summary>
public class TaktServiceTicketTemplateDto
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
    /// 服务工单编码（组合唯一索引）
    /// </summary>
    public string? ServiceTicketCode { get; set; } = string.Empty;

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
    /// 关联服务请求ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ServiceRequestId { get; set; }

    /// <summary>
    /// 关联服务请求单号（冗余字段，便于查询）
    /// </summary>
    public string? ServiceRequestCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联服务订单ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ServiceOrderId { get; set; }

    /// <summary>
    /// 关联服务订单编码（冗余字段，便于查询）
    /// </summary>
    public string? ServiceOrderCode { get; set; } = string.Empty;

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
    /// 工单类型（0=维修，1=巡检，2=安装，3=升级，4=其他）
    /// </summary>
    public int? TicketType { get; set; }

    /// <summary>
    /// 优先级（字典 sys_priority_level_category）
    /// </summary>
    public int? Priority { get; set; }

    /// <summary>
    /// 工单主题
    /// </summary>
    public string? TicketSubject { get; set; } = string.Empty;

    /// <summary>
    /// 故障/问题描述
    /// </summary>
    public string? FaultDescription { get; set; } = string.Empty;

    /// <summary>
    /// 处理方案/解决说明
    /// </summary>
    public string? SolutionDescription { get; set; } = string.Empty;

    /// <summary>
    /// 服务地点
    /// </summary>
    public string? ServiceLocation { get; set; } = string.Empty;

    /// <summary>
    /// 指派服务人员工ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AssignedEmployeeId { get; set; }

    /// <summary>
    /// 指派服务人员姓名
    /// </summary>
    public string? AssignedEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 计划开始时间
    /// </summary>
    public DateTime? ScheduledStartTime { get; set; }

    /// <summary>
    /// 计划结束时间
    /// </summary>
    public DateTime? ScheduledEndTime { get; set; }

    /// <summary>
    /// 实际开始时间
    /// </summary>
    public DateTime? ActualStartTime { get; set; }

    /// <summary>
    /// 实际结束时间
    /// </summary>
    public DateTime? ActualEndTime { get; set; }

    /// <summary>
    /// 验收结果（0=不合格，1=合格，2=部分合格）
    /// </summary>
    public int? AcceptanceResult { get; set; }

    /// <summary>
    /// 验收人
    /// </summary>
    public string? AcceptedBy { get; set; } = string.Empty;

    /// <summary>
    /// 验收时间
    /// </summary>
    public DateTime? AcceptedAt { get; set; }

    /// <summary>
    /// 工单状态（字典 sys_ticket_status；0=新建，1=已分配，2=处理中，3=待确认，4=已完成，5=已关闭，6=已取消）
    /// </summary>
    public int? TicketStatus { get; set; }

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
/// ServiceTicket 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktServiceTicketImportDto
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
    /// 服务工单编码（组合唯一索引）
    /// </summary>
    public string? ServiceTicketCode { get; set; } = string.Empty;

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
    /// 关联服务请求ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ServiceRequestId { get; set; }

    /// <summary>
    /// 关联服务请求单号（冗余字段，便于查询）
    /// </summary>
    public string? ServiceRequestCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联服务订单ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ServiceOrderId { get; set; }

    /// <summary>
    /// 关联服务订单编码（冗余字段，便于查询）
    /// </summary>
    public string? ServiceOrderCode { get; set; } = string.Empty;

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
    /// 工单类型（0=维修，1=巡检，2=安装，3=升级，4=其他）
    /// </summary>
    public int? TicketType { get; set; }

    /// <summary>
    /// 优先级（字典 sys_priority_level_category）
    /// </summary>
    public int? Priority { get; set; }

    /// <summary>
    /// 工单主题
    /// </summary>
    public string? TicketSubject { get; set; } = string.Empty;

    /// <summary>
    /// 故障/问题描述
    /// </summary>
    public string? FaultDescription { get; set; } = string.Empty;

    /// <summary>
    /// 处理方案/解决说明
    /// </summary>
    public string? SolutionDescription { get; set; } = string.Empty;

    /// <summary>
    /// 服务地点
    /// </summary>
    public string? ServiceLocation { get; set; } = string.Empty;

    /// <summary>
    /// 指派服务人员工ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AssignedEmployeeId { get; set; }

    /// <summary>
    /// 指派服务人员姓名
    /// </summary>
    public string? AssignedEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 计划开始时间
    /// </summary>
    public DateTime? ScheduledStartTime { get; set; }

    /// <summary>
    /// 计划结束时间
    /// </summary>
    public DateTime? ScheduledEndTime { get; set; }

    /// <summary>
    /// 实际开始时间
    /// </summary>
    public DateTime? ActualStartTime { get; set; }

    /// <summary>
    /// 实际结束时间
    /// </summary>
    public DateTime? ActualEndTime { get; set; }

    /// <summary>
    /// 验收结果（0=不合格，1=合格，2=部分合格）
    /// </summary>
    public int? AcceptanceResult { get; set; }

    /// <summary>
    /// 验收人
    /// </summary>
    public string? AcceptedBy { get; set; } = string.Empty;

    /// <summary>
    /// 验收时间
    /// </summary>
    public DateTime? AcceptedAt { get; set; }

    /// <summary>
    /// 工单状态（字典 sys_ticket_status；0=新建，1=已分配，2=处理中，3=待确认，4=已完成，5=已关闭，6=已取消）
    /// </summary>
    public int? TicketStatus { get; set; }

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
/// ServiceTicket 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktServiceTicketExportDto
{
    /// <summary>
    /// ServiceTicketID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ServiceTicketId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 服务工单编码（组合唯一索引）
    /// </summary>
    public string ServiceTicketCode { get; set; } = string.Empty;

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
    /// 关联服务请求ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ServiceRequestId { get; set; }

    /// <summary>
    /// 关联服务请求单号（冗余字段，便于查询）
    /// </summary>
    public string? ServiceRequestCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联服务订单ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ServiceOrderId { get; set; }

    /// <summary>
    /// 关联服务订单编码（冗余字段，便于查询）
    /// </summary>
    public string? ServiceOrderCode { get; set; } = string.Empty;

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
    /// 工单类型（0=维修，1=巡检，2=安装，3=升级，4=其他）
    /// </summary>
    public int TicketType { get; set; } = 0;

    /// <summary>
    /// 优先级（字典 sys_priority_level_category）
    /// </summary>
    public int Priority { get; set; } = 0;

    /// <summary>
    /// 工单主题
    /// </summary>
    public string TicketSubject { get; set; } = string.Empty;

    /// <summary>
    /// 故障/问题描述
    /// </summary>
    public string? FaultDescription { get; set; } = string.Empty;

    /// <summary>
    /// 处理方案/解决说明
    /// </summary>
    public string? SolutionDescription { get; set; } = string.Empty;

    /// <summary>
    /// 服务地点
    /// </summary>
    public string? ServiceLocation { get; set; } = string.Empty;

    /// <summary>
    /// 指派服务人员工ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AssignedEmployeeId { get; set; }

    /// <summary>
    /// 指派服务人员姓名
    /// </summary>
    public string? AssignedEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 计划开始时间
    /// </summary>
    public DateTime? ScheduledStartTime { get; set; }

    /// <summary>
    /// 计划结束时间
    /// </summary>
    public DateTime? ScheduledEndTime { get; set; }

    /// <summary>
    /// 实际开始时间
    /// </summary>
    public DateTime? ActualStartTime { get; set; }

    /// <summary>
    /// 实际结束时间
    /// </summary>
    public DateTime? ActualEndTime { get; set; }

    /// <summary>
    /// 验收结果（0=不合格，1=合格，2=部分合格）
    /// </summary>
    public int? AcceptanceResult { get; set; }

    /// <summary>
    /// 验收人
    /// </summary>
    public string? AcceptedBy { get; set; } = string.Empty;

    /// <summary>
    /// 验收时间
    /// </summary>
    public DateTime? AcceptedAt { get; set; }

    /// <summary>
    /// 排序号（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 工单状态（字典 sys_ticket_status；0=新建，1=已分配，2=处理中，3=待确认，4=已完成，5=已关闭，6=已取消）
    /// </summary>
    public int TicketStatus { get; set; } = 0;

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
