// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Routine.ConferenceCenter
// 文件名称：TaktConferenceDtos.cs
// 创建时间：2026-06-11
// 创建人：Takt365(Auto Generated)
// 功能描述：Conference 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktConference 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Enums;

namespace Takt.Application.Dtos.Routine.ConferenceCenter;

// ========================================
// Conference 响应 DTO
// ========================================

/// <summary>
/// 会议实体（审批单） 继承 TaktApprovalEntityBase；排期、会议室关联；参与人（含签到）、议程/纪要由子实体维护 FlowInstanceId 由业务在发起流程后写入；流程引擎 BusinessType=Conference、BusinessKey=本表 Id
/// 对应前端 TaktConferenceDto
/// 继承 TaktApprovalDtoBase
/// </summary>
public class TaktConferenceDto : TaktApprovalDtoBase
{
    /// <summary>
    /// ConferenceID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConferenceId { get; set; }

    /// <summary>
    /// 会议编码（租户+公司内唯一）
    /// </summary>
    public string ConferenceCode { get; set; } = string.Empty;

    /// <summary>
    /// 会议标题
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 会议类型
    /// </summary>
    public int ConferenceType { get; set; }

    /// <summary>
    /// 会议状态（草稿/已排期/进行中/已结束/已取消）
    /// </summary>
    public int ConferenceStatus { get; set; }

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime EndTime { get; set; }

    /// <summary>
    /// 会议室 ID（线下预约，可空表示外部场地或纯线上）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ConferenceRoomId { get; set; }

    /// <summary>
    /// 会议室名称（冗余，便于列表展示）
    /// </summary>
    public string? ConferenceRoomName { get; set; } = string.Empty;

    /// <summary>
    /// 会议地点（外部场地或补充地址，与会议室可并存）
    /// </summary>
    public string? Location { get; set; } = string.Empty;

    /// <summary>
    /// 会议链接（线上会议 URL）
    /// </summary>
    public string? MeetingLink { get; set; } = string.Empty;

    /// <summary>
    /// 标签（逗号分隔或 JSON 数组存储）
    /// </summary>
    public string? Tags { get; set; } = string.Empty;

    /// <summary>
    /// 组织人 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long OrganizerId { get; set; }

    /// <summary>
    /// 组织人姓名
    /// </summary>
    public string OrganizerName { get; set; } = string.Empty;

    /// <summary>
    /// 主办部门 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

    /// <summary>
    /// 主办部门名称
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 最大参会人数（0 表示不限）
    /// </summary>
    public int MaxParticipants { get; set; } = 0;

    /// <summary>
    /// 提前提醒分钟数（0 表示不提醒）
    /// </summary>
    public int ReminderMinutes { get; set; } = 0;

    /// <summary>
    /// 流程实例 名称（填充字段）
    /// </summary>
    public string? FlowInstanceName { get; set; }

    /// <summary>
    /// 会议室
    /// （主表：TaktConferenceRoom）
    /// </summary>
    public TaktConferenceRoomDto? ConferenceRoom { get; set; }

    /// <summary>
    /// 参与人列表（主子表关系）
    /// （子表：TaktConferenceParticipant）
    /// </summary>
    public List<TaktConferenceParticipantDto>? Participants { get; set; }

    /// <summary>
    /// 议程/纪要列表（主子表关系）
    /// （子表：TaktConferenceAgenda）
    /// </summary>
    public List<TaktConferenceAgendaDto>? AgendaRecords { get; set; }

}

// ========================================
// Conference 查询 DTO
// ========================================

/// <summary>
/// Conference 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktConferenceQueryDto : TaktPagedQuery
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
    /// 会议编码（租户+公司内唯一）
    /// </summary>
    public string? ConferenceCode { get; set; } = string.Empty;

    /// <summary>
    /// 会议标题
    /// </summary>
    public string? Title { get; set; } = string.Empty;

    /// <summary>
    /// 会议类型
    /// </summary>
    public int? ConferenceType { get; set; }

    /// <summary>
    /// 会议状态（草稿/已排期/进行中/已结束/已取消）
    /// </summary>
    public int? ConferenceStatus { get; set; }

    /// <summary>
    /// 开始时间（范围查询-开始）
    /// </summary>
    public DateTime? StartTimeStart { get; set; }

    /// <summary>
    /// 开始时间（范围查询-结束）
    /// </summary>
    public DateTime? StartTimeEnd { get; set; }

    /// <summary>
    /// 结束时间（范围查询-开始）
    /// </summary>
    public DateTime? EndTimeStart { get; set; }

    /// <summary>
    /// 结束时间（范围查询-结束）
    /// </summary>
    public DateTime? EndTimeEnd { get; set; }

    /// <summary>
    /// 会议室 ID（线下预约，可空表示外部场地或纯线上）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ConferenceRoomId { get; set; }

    /// <summary>
    /// 会议室名称（冗余，便于列表展示）
    /// </summary>
    public string? ConferenceRoomName { get; set; } = string.Empty;

    /// <summary>
    /// 会议地点（外部场地或补充地址，与会议室可并存）
    /// </summary>
    public string? Location { get; set; } = string.Empty;

    /// <summary>
    /// 会议链接（线上会议 URL）
    /// </summary>
    public string? MeetingLink { get; set; } = string.Empty;

    /// <summary>
    /// 标签（逗号分隔或 JSON 数组存储）
    /// </summary>
    public string? Tags { get; set; } = string.Empty;

    /// <summary>
    /// 组织人 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? OrganizerId { get; set; }

    /// <summary>
    /// 组织人姓名
    /// </summary>
    public string? OrganizerName { get; set; } = string.Empty;

    /// <summary>
    /// 主办部门 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

    /// <summary>
    /// 主办部门名称
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 最大参会人数（0 表示不限）
    /// </summary>
    public int? MaxParticipants { get; set; }

    /// <summary>
    /// 提前提醒分钟数（0 表示不提醒）
    /// </summary>
    public int? ReminderMinutes { get; set; }

    /// <summary>
    /// 流程实例 ID（关联工作流流程实例表 takt_workflow_instance；BusinessType=Conference、BusinessKey=本表 Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FlowInstanceId { get; set; }

    /// <summary>
    /// 审批状态（TaktApprovalStatus）
    /// </summary>
    public int? ApprovalStatus { get; set; }

    /// <summary>
    /// 发起人ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? InitiatorId { get; set; }

    /// <summary>
    /// 发起时间（范围查询-开始）
    /// </summary>
    public DateTime? InitiatedAtStart { get; set; }

    /// <summary>
    /// 发起时间（范围查询-结束）
    /// </summary>
    public DateTime? InitiatedAtEnd { get; set; }

    /// <summary>
    /// 最终审批人ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApprovedBy { get; set; }

    /// <summary>
    /// 最终审批时间（范围查询-开始）
    /// </summary>
    public DateTime? ApprovedAtStart { get; set; }

    /// <summary>
    /// 最终审批时间（范围查询-结束）
    /// </summary>
    public DateTime? ApprovedAtEnd { get; set; }

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
// 创建Conference DTO
// ========================================

/// <summary>
/// 创建Conference DTO
/// </summary>
public class TaktConferenceCreateDto
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
    /// 会议编码（租户+公司内唯一）
    /// </summary>
    [Required(ErrorMessage = "会议编码（租户+公司内唯一）不能为空")]
    public string ConferenceCode { get; set; } = string.Empty;

    /// <summary>
    /// 会议标题
    /// </summary>
    [Required(ErrorMessage = "会议标题不能为空")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 会议类型
    /// </summary>
    public int ConferenceType { get; set; }

    /// <summary>
    /// 会议状态（草稿/已排期/进行中/已结束/已取消）
    /// </summary>
    public int ConferenceStatus { get; set; }

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime EndTime { get; set; }

    /// <summary>
    /// 会议室 ID（线下预约，可空表示外部场地或纯线上）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ConferenceRoomId { get; set; }

    /// <summary>
    /// 会议室名称（冗余，便于列表展示）
    /// </summary>
    public string? ConferenceRoomName { get; set; } = string.Empty;

    /// <summary>
    /// 会议地点（外部场地或补充地址，与会议室可并存）
    /// </summary>
    public string? Location { get; set; } = string.Empty;

    /// <summary>
    /// 会议链接（线上会议 URL）
    /// </summary>
    public string? MeetingLink { get; set; } = string.Empty;

    /// <summary>
    /// 标签（逗号分隔或 JSON 数组存储）
    /// </summary>
    public string? Tags { get; set; } = string.Empty;

    /// <summary>
    /// 组织人 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long OrganizerId { get; set; }

    /// <summary>
    /// 组织人姓名
    /// </summary>
    [Required(ErrorMessage = "组织人姓名不能为空")]
    public string OrganizerName { get; set; } = string.Empty;

    /// <summary>
    /// 主办部门 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

    /// <summary>
    /// 主办部门名称
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 最大参会人数（0 表示不限）
    /// </summary>
    public int MaxParticipants { get; set; } = 0;

    /// <summary>
    /// 提前提醒分钟数（0 表示不提醒）
    /// </summary>
    public int ReminderMinutes { get; set; } = 0;

    /// <summary>
    /// 流程实例 ID（关联工作流流程实例表 takt_workflow_instance；BusinessType=Conference、BusinessKey=本表 Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FlowInstanceId { get; set; }

    /// <summary>
    /// 参与人列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktConferenceParticipantCreateDto>? Participants { get; set; }

    /// <summary>
    /// 议程/纪要列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktConferenceAgendaCreateDto>? AgendaRecords { get; set; }

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
// 更新Conference DTO
// ========================================

/// <summary>
/// 更新Conference DTO
/// 继承 TaktConferenceCreateDto，添加 ConferenceId 字段
/// </summary>
public class TaktConferenceUpdateDto : TaktConferenceCreateDto
{
    /// <summary>
    /// ConferenceID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConferenceId { get; set; }

}

// ========================================
// Conference 状态 DTO
// ========================================

/// <summary>
/// Conference 状态更新 DTO
/// </summary>
public class TaktConferenceStatusDto
{
    /// <summary>
    /// ConferenceID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConferenceId { get; set; }

    /// <summary>
    /// 会议状态（草稿/已排期/进行中/已结束/已取消）
    /// </summary>
    [Required(ErrorMessage = "会议状态（草稿/已排期/进行中/已结束/已取消）不能为空")]
    public int ConferenceStatus { get; set; }
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// Conference 导入模板行 DTO
/// </summary>
public class TaktConferenceTemplateDto
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
    /// 会议编码（租户+公司内唯一）
    /// </summary>
    public string? ConferenceCode { get; set; } = string.Empty;

    /// <summary>
    /// 会议标题
    /// </summary>
    public string? Title { get; set; } = string.Empty;

    /// <summary>
    /// 会议类型
    /// </summary>
    public int? ConferenceType { get; set; }

    /// <summary>
    /// 会议状态（草稿/已排期/进行中/已结束/已取消）
    /// </summary>
    public int? ConferenceStatus { get; set; }

    /// <summary>
    /// 会议室 ID（线下预约，可空表示外部场地或纯线上）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ConferenceRoomId { get; set; }

    /// <summary>
    /// 会议室名称（冗余，便于列表展示）
    /// </summary>
    public string? ConferenceRoomName { get; set; } = string.Empty;

    /// <summary>
    /// 会议地点（外部场地或补充地址，与会议室可并存）
    /// </summary>
    public string? Location { get; set; } = string.Empty;

    /// <summary>
    /// 会议链接（线上会议 URL）
    /// </summary>
    public string? MeetingLink { get; set; } = string.Empty;

    /// <summary>
    /// 标签（逗号分隔或 JSON 数组存储）
    /// </summary>
    public string? Tags { get; set; } = string.Empty;

    /// <summary>
    /// 组织人 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? OrganizerId { get; set; }

    /// <summary>
    /// 组织人姓名
    /// </summary>
    public string? OrganizerName { get; set; } = string.Empty;

    /// <summary>
    /// 主办部门 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

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
/// Conference 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktConferenceImportDto
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
    /// 会议编码（租户+公司内唯一）
    /// </summary>
    public string? ConferenceCode { get; set; } = string.Empty;

    /// <summary>
    /// 会议标题
    /// </summary>
    public string? Title { get; set; } = string.Empty;

    /// <summary>
    /// 会议类型
    /// </summary>
    public int? ConferenceType { get; set; }

    /// <summary>
    /// 会议状态（草稿/已排期/进行中/已结束/已取消）
    /// </summary>
    public int? ConferenceStatus { get; set; }

    /// <summary>
    /// 会议室 ID（线下预约，可空表示外部场地或纯线上）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ConferenceRoomId { get; set; }

    /// <summary>
    /// 会议室名称（冗余，便于列表展示）
    /// </summary>
    public string? ConferenceRoomName { get; set; } = string.Empty;

    /// <summary>
    /// 会议地点（外部场地或补充地址，与会议室可并存）
    /// </summary>
    public string? Location { get; set; } = string.Empty;

    /// <summary>
    /// 会议链接（线上会议 URL）
    /// </summary>
    public string? MeetingLink { get; set; } = string.Empty;

    /// <summary>
    /// 标签（逗号分隔或 JSON 数组存储）
    /// </summary>
    public string? Tags { get; set; } = string.Empty;

    /// <summary>
    /// 组织人 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? OrganizerId { get; set; }

    /// <summary>
    /// 组织人姓名
    /// </summary>
    public string? OrganizerName { get; set; } = string.Empty;

    /// <summary>
    /// 主办部门 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

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
/// Conference 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktConferenceExportDto
{
    /// <summary>
    /// ConferenceID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConferenceId { get; set; }

    /// <summary>
    /// 会议编码（租户+公司内唯一）
    /// </summary>
    public string ConferenceCode { get; set; } = string.Empty;

    /// <summary>
    /// 会议标题
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 会议类型
    /// </summary>
    public int ConferenceType { get; set; }

    /// <summary>
    /// 会议状态（草稿/已排期/进行中/已结束/已取消）
    /// </summary>
    public int ConferenceStatus { get; set; }

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime EndTime { get; set; }

    /// <summary>
    /// 会议室 ID（线下预约，可空表示外部场地或纯线上）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ConferenceRoomId { get; set; }

    /// <summary>
    /// 会议室名称（冗余，便于列表展示）
    /// </summary>
    public string? ConferenceRoomName { get; set; } = string.Empty;

    /// <summary>
    /// 会议地点（外部场地或补充地址，与会议室可并存）
    /// </summary>
    public string? Location { get; set; } = string.Empty;

    /// <summary>
    /// 会议链接（线上会议 URL）
    /// </summary>
    public string? MeetingLink { get; set; } = string.Empty;

    /// <summary>
    /// 标签（逗号分隔或 JSON 数组存储）
    /// </summary>
    public string? Tags { get; set; } = string.Empty;

    /// <summary>
    /// 组织人 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long OrganizerId { get; set; }

    /// <summary>
    /// 组织人姓名
    /// </summary>
    public string OrganizerName { get; set; } = string.Empty;

    /// <summary>
    /// 主办部门 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

    /// <summary>
    /// 主办部门名称
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 最大参会人数（0 表示不限）
    /// </summary>
    public int MaxParticipants { get; set; } = 0;

    /// <summary>
    /// 提前提醒分钟数（0 表示不提醒）
    /// </summary>
    public int ReminderMinutes { get; set; } = 0;

    /// <summary>
    /// 流程实例 ID（关联工作流流程实例表 takt_workflow_instance；BusinessType=Conference、BusinessKey=本表 Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FlowInstanceId { get; set; }

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
