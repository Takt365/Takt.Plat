// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Routine.ConferenceCenter
// 文件名称：TaktConferenceDtos.cs
// 创建时间：2026-08-11
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

namespace Takt.Application.Dtos.Routine.ConferenceCenter;

// ========================================
// Conference 响应 DTO
// ========================================

/// <summary>
/// 会议中心主实体 支持内部/外部/视频/混合会议排期、议程及参与人管理；需审批通过后排期 审批态见基类 ApprovalStatus，字典 sys_approval_status
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
    public string ConferenceTitle { get; set; } = string.Empty;

    /// <summary>
    /// 会议类型（字典 routine_conference_type；0=内部 1=外部 2=视频 3=混合）
    /// </summary>
    public int ConferenceType { get; set; } = 0;

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime EndTime { get; set; }

    /// <summary>
    /// 会议地点（线下会议室名称或地址）
    /// </summary>
    public string? Location { get; set; } = string.Empty;

    /// <summary>
    /// 会议链接（线上会议 URL）
    /// </summary>
    public string? MeetingLink { get; set; } = string.Empty;

    /// <summary>
    /// 会议议程
    /// </summary>
    public string? Agenda { get; set; } = string.Empty;

    /// <summary>
    /// 会议内容（会议纪要正文，富文本 HTML）
    /// </summary>
    public string? ConferenceContent { get; set; } = string.Empty;

    /// <summary>
    /// 会议纪要摘要（用于列表展示）
    /// </summary>
    public string? ConferenceSummary { get; set; } = string.Empty;

    /// <summary>
    /// 标签（逗号分隔或 JSON 数组存储）
    /// </summary>
    public string? ConferenceTags { get; set; } = string.Empty;

    /// <summary>
    /// 组织人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long OrganizerId { get; set; }

    /// <summary>
    /// 组织人姓名
    /// </summary>
    public string OrganizerName { get; set; } = string.Empty;

    /// <summary>
    /// 主办部门 ID（关联 TaktDept.Id，选项 TaktDepts/tree-options）
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
    /// 会议室 ID（选项 TaktConferenceRooms/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ConferenceRoomId { get; set; }

    /// <summary>
    /// 会议室名称（冗余快照）
    /// </summary>
    public string? ConferenceRoomName { get; set; } = string.Empty;

    /// <summary>
    /// 会议状态（字典 routine_conference_status；0=草稿 1=已排期 2=进行中 3=已结束 4=已取消）
    /// </summary>
    public int ConferenceStatus { get; set; } = 0;

    /// <summary>
    /// 参与人列表（主子表关系）
    /// （子表：TaktConferenceParticipant）
    /// </summary>
    public List<TaktConferenceParticipantDto>? Participants { get; set; }

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
    /// 区域文化编码（字典 sys_culture_code）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;


    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 会议编码（租户+公司内唯一）
    /// </summary>
    public string? ConferenceCode { get; set; } = string.Empty;

    /// <summary>
    /// 会议标题
    /// </summary>
    public string? ConferenceTitle { get; set; } = string.Empty;

    /// <summary>
    /// 会议类型（字典 routine_conference_type；0=内部 1=外部 2=视频 3=混合）
    /// </summary>
    public int? ConferenceType { get; set; }

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
    /// 会议地点（线下会议室名称或地址）
    /// </summary>
    public string? Location { get; set; } = string.Empty;

    /// <summary>
    /// 会议链接（线上会议 URL）
    /// </summary>
    public string? MeetingLink { get; set; } = string.Empty;

    /// <summary>
    /// 会议议程
    /// </summary>
    public string? Agenda { get; set; } = string.Empty;

    /// <summary>
    /// 会议内容（会议纪要正文，富文本 HTML）
    /// </summary>
    public string? ConferenceContent { get; set; } = string.Empty;

    /// <summary>
    /// 会议纪要摘要（用于列表展示）
    /// </summary>
    public string? ConferenceSummary { get; set; } = string.Empty;

    /// <summary>
    /// 标签（逗号分隔或 JSON 数组存储）
    /// </summary>
    public string? ConferenceTags { get; set; } = string.Empty;

    /// <summary>
    /// 组织人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? OrganizerId { get; set; }

    /// <summary>
    /// 组织人姓名
    /// </summary>
    public string? OrganizerName { get; set; } = string.Empty;

    /// <summary>
    /// 主办部门 ID（关联 TaktDept.Id，选项 TaktDepts/tree-options）
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
    /// 会议室 ID（选项 TaktConferenceRooms/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ConferenceRoomId { get; set; }

    /// <summary>
    /// 会议室名称（冗余快照）
    /// </summary>
    public string? ConferenceRoomName { get; set; } = string.Empty;

    /// <summary>
    /// 会议状态（字典 routine_conference_status；0=草稿 1=已排期 2=进行中 3=已结束 4=已取消）
    /// </summary>
    public int? ConferenceStatus { get; set; }

    /// <summary>
    /// 审批状态（字典 sys_approval_status；与 TaktApprovalEntityBase.ApprovalStatus 一致）
    /// </summary>
    public TaktApprovalStatus? ApprovalStatus { get; set; }

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
    /// 流程实例 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FlowInstanceId { get; set; }

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
    /// 区域文化编码（登录或公司切换注入，对应实体基类 CultureCode / 公司 culture_code）
    /// </summary>
    public string CultureCode { get; set; } = string.Empty;


    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 会议编码（租户+公司内唯一）
    /// </summary>
    [Required(ErrorMessage = "会议编码（租户+公司内唯一）不能为空")]
    public string ConferenceCode { get; set; } = string.Empty;

    /// <summary>
    /// 会议标题
    /// </summary>
    [Required(ErrorMessage = "会议标题不能为空")]
    public string ConferenceTitle { get; set; } = string.Empty;

    /// <summary>
    /// 会议类型（字典 routine_conference_type；0=内部 1=外部 2=视频 3=混合）
    /// </summary>
    public int ConferenceType { get; set; } = 0;

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime EndTime { get; set; }

    /// <summary>
    /// 会议地点（线下会议室名称或地址）
    /// </summary>
    public string? Location { get; set; } = string.Empty;

    /// <summary>
    /// 会议链接（线上会议 URL）
    /// </summary>
    public string? MeetingLink { get; set; } = string.Empty;

    /// <summary>
    /// 会议议程
    /// </summary>
    public string? Agenda { get; set; } = string.Empty;

    /// <summary>
    /// 会议内容（会议纪要正文，富文本 HTML）
    /// </summary>
    public string? ConferenceContent { get; set; } = string.Empty;

    /// <summary>
    /// 会议纪要摘要（用于列表展示）
    /// </summary>
    public string? ConferenceSummary { get; set; } = string.Empty;

    /// <summary>
    /// 标签（逗号分隔或 JSON 数组存储）
    /// </summary>
    public string? ConferenceTags { get; set; } = string.Empty;

    /// <summary>
    /// 组织人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long OrganizerId { get; set; }

    /// <summary>
    /// 组织人姓名
    /// </summary>
    [Required(ErrorMessage = "组织人姓名不能为空")]
    public string OrganizerName { get; set; } = string.Empty;

    /// <summary>
    /// 主办部门 ID（关联 TaktDept.Id，选项 TaktDepts/tree-options）
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
    /// 会议室 ID（选项 TaktConferenceRooms/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ConferenceRoomId { get; set; }

    /// <summary>
    /// 会议室名称（冗余快照）
    /// </summary>
    public string? ConferenceRoomName { get; set; } = string.Empty;

    /// <summary>
    /// 会议状态（字典 routine_conference_status；0=草稿 1=已排期 2=进行中 3=已结束 4=已取消）
    /// </summary>
    public int ConferenceStatus { get; set; } = 0;

    /// <summary>
    /// 参与人列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktConferenceParticipantCreateDto>? Participants { get; set; }

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

    /// <summary>
    /// 参与人列表（主子表关系）（子表，级联保存）
    /// </summary>
    public new List<TaktConferenceParticipantUpdateDto>? Participants { get; set; }

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
    /// 会议状态（字典 routine_conference_status；0=草稿 1=已排期 2=进行中 3=已结束 4=已取消）
    /// </summary>
    [Required(ErrorMessage = "会议状态（字典 routine_conference_status；0=草稿 1=已排期 2=进行中 3=已结束 4=已取消）不能为空")]
    public int ConferenceStatus { get; set; } = 0;
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
    /// 区域文化编码（登录或公司切换注入，对应实体基类 CultureCode / 公司 culture_code）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;


    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 会议编码（租户+公司内唯一）
    /// </summary>
    public string? ConferenceCode { get; set; } = string.Empty;

    /// <summary>
    /// 会议标题
    /// </summary>
    public string? ConferenceTitle { get; set; } = string.Empty;

    /// <summary>
    /// 会议类型（字典 routine_conference_type；0=内部 1=外部 2=视频 3=混合）
    /// </summary>
    public int? ConferenceType { get; set; }

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// 会议地点（线下会议室名称或地址）
    /// </summary>
    public string? Location { get; set; } = string.Empty;

    /// <summary>
    /// 会议链接（线上会议 URL）
    /// </summary>
    public string? MeetingLink { get; set; } = string.Empty;

    /// <summary>
    /// 会议议程
    /// </summary>
    public string? Agenda { get; set; } = string.Empty;

    /// <summary>
    /// 会议内容（会议纪要正文，富文本 HTML）
    /// </summary>
    public string? ConferenceContent { get; set; } = string.Empty;

    /// <summary>
    /// 会议纪要摘要（用于列表展示）
    /// </summary>
    public string? ConferenceSummary { get; set; } = string.Empty;

    /// <summary>
    /// 标签（逗号分隔或 JSON 数组存储）
    /// </summary>
    public string? ConferenceTags { get; set; } = string.Empty;

    /// <summary>
    /// 组织人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? OrganizerId { get; set; }

    /// <summary>
    /// 组织人姓名
    /// </summary>
    public string? OrganizerName { get; set; } = string.Empty;

    /// <summary>
    /// 主办部门 ID（关联 TaktDept.Id，选项 TaktDepts/tree-options）
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
    /// 会议室 ID（选项 TaktConferenceRooms/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ConferenceRoomId { get; set; }

    /// <summary>
    /// 会议室名称（冗余快照）
    /// </summary>
    public string? ConferenceRoomName { get; set; } = string.Empty;

    /// <summary>
    /// 会议状态（字典 routine_conference_status；0=草稿 1=已排期 2=进行中 3=已结束 4=已取消）
    /// </summary>
    public int? ConferenceStatus { get; set; }

    /// <summary>
    /// 参与人列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktConferenceParticipantCreateDto>? Participants { get; set; }

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
    /// 区域文化编码（登录或公司切换注入，对应实体基类 CultureCode / 公司 culture_code）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;


    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 会议编码（租户+公司内唯一）
    /// </summary>
    public string? ConferenceCode { get; set; } = string.Empty;

    /// <summary>
    /// 会议标题
    /// </summary>
    public string? ConferenceTitle { get; set; } = string.Empty;

    /// <summary>
    /// 会议类型（字典 routine_conference_type；0=内部 1=外部 2=视频 3=混合）
    /// </summary>
    public int? ConferenceType { get; set; }

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// 会议地点（线下会议室名称或地址）
    /// </summary>
    public string? Location { get; set; } = string.Empty;

    /// <summary>
    /// 会议链接（线上会议 URL）
    /// </summary>
    public string? MeetingLink { get; set; } = string.Empty;

    /// <summary>
    /// 会议议程
    /// </summary>
    public string? Agenda { get; set; } = string.Empty;

    /// <summary>
    /// 会议内容（会议纪要正文，富文本 HTML）
    /// </summary>
    public string? ConferenceContent { get; set; } = string.Empty;

    /// <summary>
    /// 会议纪要摘要（用于列表展示）
    /// </summary>
    public string? ConferenceSummary { get; set; } = string.Empty;

    /// <summary>
    /// 标签（逗号分隔或 JSON 数组存储）
    /// </summary>
    public string? ConferenceTags { get; set; } = string.Empty;

    /// <summary>
    /// 组织人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? OrganizerId { get; set; }

    /// <summary>
    /// 组织人姓名
    /// </summary>
    public string? OrganizerName { get; set; } = string.Empty;

    /// <summary>
    /// 主办部门 ID（关联 TaktDept.Id，选项 TaktDepts/tree-options）
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
    /// 会议室 ID（选项 TaktConferenceRooms/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ConferenceRoomId { get; set; }

    /// <summary>
    /// 会议室名称（冗余快照）
    /// </summary>
    public string? ConferenceRoomName { get; set; } = string.Empty;

    /// <summary>
    /// 会议状态（字典 routine_conference_status；0=草稿 1=已排期 2=进行中 3=已结束 4=已取消）
    /// </summary>
    public int? ConferenceStatus { get; set; }

    /// <summary>
    /// 参与人列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktConferenceParticipantCreateDto>? Participants { get; set; }

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
    public string ConferenceTitle { get; set; } = string.Empty;

    /// <summary>
    /// 会议类型（字典 routine_conference_type；0=内部 1=外部 2=视频 3=混合）
    /// </summary>
    public int ConferenceType { get; set; } = 0;

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime EndTime { get; set; }

    /// <summary>
    /// 会议地点（线下会议室名称或地址）
    /// </summary>
    public string? Location { get; set; } = string.Empty;

    /// <summary>
    /// 会议链接（线上会议 URL）
    /// </summary>
    public string? MeetingLink { get; set; } = string.Empty;

    /// <summary>
    /// 会议议程
    /// </summary>
    public string? Agenda { get; set; } = string.Empty;

    /// <summary>
    /// 会议内容（会议纪要正文，富文本 HTML）
    /// </summary>
    public string? ConferenceContent { get; set; } = string.Empty;

    /// <summary>
    /// 会议纪要摘要（用于列表展示）
    /// </summary>
    public string? ConferenceSummary { get; set; } = string.Empty;

    /// <summary>
    /// 标签（逗号分隔或 JSON 数组存储）
    /// </summary>
    public string? ConferenceTags { get; set; } = string.Empty;

    /// <summary>
    /// 组织人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long OrganizerId { get; set; }

    /// <summary>
    /// 组织人姓名
    /// </summary>
    public string OrganizerName { get; set; } = string.Empty;

    /// <summary>
    /// 主办部门 ID（关联 TaktDept.Id，选项 TaktDepts/tree-options）
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
    /// 会议室 ID（选项 TaktConferenceRooms/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ConferenceRoomId { get; set; }

    /// <summary>
    /// 会议室名称（冗余快照）
    /// </summary>
    public string? ConferenceRoomName { get; set; } = string.Empty;

    /// <summary>
    /// 会议状态（字典 routine_conference_status；0=草稿 1=已排期 2=进行中 3=已结束 4=已取消）
    /// </summary>
    public int ConferenceStatus { get; set; } = 0;

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
