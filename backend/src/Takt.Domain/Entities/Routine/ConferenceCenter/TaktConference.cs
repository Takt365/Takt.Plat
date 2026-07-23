// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Routine.ConferenceCenter
// 文件名称：TaktConference.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：会议中心主实体，支持内部/外部/视频/混合会议排期、议程及参与人管理
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Routine.ConferenceCenter;

/// <summary>
/// 会议中心主实体
/// 支持内部/外部/视频/混合会议排期、议程及参与人管理；需审批通过后排期
/// 审批态见基类 ApprovalStatus，字典 sys_approval_status
/// </summary>
[SugarTable("takt_routine_conference_center", "会议中心表")]
[SugarIndex("ix_conference_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_conference_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_conference_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ConferenceCode), OrderByType.Asc, true)]
[SugarIndex("ix_conference_status", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ConferenceStatus), OrderByType.Asc, false)]
[SugarIndex("ix_conference_start_time", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(StartTime), OrderByType.Asc, false)]
[SugarIndex("ix_conference_flow_instance_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(FlowInstanceId), OrderByType.Asc, false)]
public class TaktConference : TaktApprovalEntityBase
{
    /// <summary>
    /// 会议编码（租户+公司内唯一）
    /// </summary>
    [SugarColumn(ColumnName = "conference_code", ColumnDescription = "会议编码", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string ConferenceCode { get; set; } = string.Empty;
    /// <summary>
    /// 会议标题
    /// </summary>
    [SugarColumn(ColumnName = "conference_title", ColumnDescription = "会议标题", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string ConferenceTitle { get; set; } = string.Empty;
    /// <summary>
    /// 会议类型（字典 routine_conference_type；0=内部 1=外部 2=视频 3=混合）
    /// </summary>
    [SugarColumn(ColumnName = "conference_type", ColumnDescription = "会议类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ConferenceType { get; set; } = 0;
    /// <summary>
    /// 开始时间
    /// </summary>
    [SugarColumn(ColumnName = "start_time", ColumnDescription = "开始时间", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime StartTime { get; set; }
    /// <summary>
    /// 结束时间
    /// </summary>
    [SugarColumn(ColumnName = "end_time", ColumnDescription = "结束时间", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime EndTime { get; set; }
    /// <summary>
    /// 会议地点（线下会议室名称或地址）
    /// </summary>
    [SugarColumn(ColumnName = "location", ColumnDescription = "会议地点", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? Location { get; set; }
    /// <summary>
    /// 会议链接（线上会议 URL）
    /// </summary>
    [SugarColumn(ColumnName = "meeting_link", ColumnDescription = "会议链接", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? MeetingLink { get; set; }
    /// <summary>
    /// 会议议程
    /// </summary>
    [SugarColumn(ColumnName = "agenda", ColumnDescription = "会议议程", ColumnDataType = "ntext", IsNullable = true)]
    public string? Agenda { get; set; }
    /// <summary>
    /// 会议内容（会议纪要正文，富文本 HTML）
    /// </summary>
    [SugarColumn(ColumnName = "conference_content", ColumnDescription = "会议内容", ColumnDataType = "ntext", IsNullable = true)]
    public string? ConferenceContent { get; set; }
    /// <summary>
    /// 会议纪要摘要（用于列表展示）
    /// </summary>
    [SugarColumn(ColumnName = "conference_summary", ColumnDescription = "会议纪要摘要", ColumnDataType = "nvarchar", Length = 2000, IsNullable = true)]
    public string? ConferenceSummary { get; set; }
    /// <summary>
    /// 标签（逗号分隔或 JSON 数组存储）
    /// </summary>
    [SugarColumn(ColumnName = "conference_tags", ColumnDescription = "标签", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? ConferenceTags { get; set; }
    /// <summary>
    /// 组织人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "organizer_id", ColumnDescription = "组织人ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long OrganizerId { get; set; }
    /// <summary>
    /// 组织人姓名
    /// </summary>
    [SugarColumn(ColumnName = "organizer_name", ColumnDescription = "组织人姓名", ColumnDataType = "varchar", Length = 20, IsNullable = false)]
    public string OrganizerName { get; set; } = string.Empty;
    /// <summary>
    /// 主办部门 ID（关联 TaktDept.Id，选项 TaktDepts/tree-options）
    /// </summary>
    [SugarColumn(ColumnName = "dept_id", ColumnDescription = "主办部门ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }
    /// <summary>
    /// 主办部门名称
    /// </summary>
    [SugarColumn(ColumnName = "dept_name", ColumnDescription = "主办部门名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? DeptName { get; set; }
    /// <summary>
    /// 最大参会人数（0 表示不限）
    /// </summary>
    [SugarColumn(ColumnName = "max_participants", ColumnDescription = "最大参会人数", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int MaxParticipants { get; set; } = 0;
    /// <summary>
    /// 提前提醒分钟数（0 表示不提醒）
    /// </summary>
    [SugarColumn(ColumnName = "reminder_minutes", ColumnDescription = "提前提醒分钟数", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ReminderMinutes { get; set; } = 0;
    /// <summary>
    /// 会议室 ID（选项 TaktConferenceRooms/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "conference_room_id", ColumnDescription = "会议室ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ConferenceRoomId { get; set; }
    /// <summary>
    /// 会议室名称（冗余快照）
    /// </summary>
    [SugarColumn(ColumnName = "conference_room_name", ColumnDescription = "会议室名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? ConferenceRoomName { get; set; }
    /// <summary>
    /// 会议状态（字典 routine_conference_status；0=草稿 1=已排期 2=进行中 3=已结束 4=已取消）
    /// </summary>
    [SugarColumn(ColumnName = "conference_status", ColumnDescription = "会议状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ConferenceStatus { get; set; } = 0;

    // ========================================
    // 导航属性区域
    // ========================================
    /// <summary>
    /// 参与人列表（主子表关系）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktConferenceParticipant.ConferenceId))]
    public List<TaktConferenceParticipant>? Participants { get; set; }
}
