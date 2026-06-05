// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Routine.ConferenceCenter
// 文件名称：TaktConferenceParticipant.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：会议参与人子实体
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities;
using Takt.Shared.Enums;

namespace Takt.Domain.Entities.Routine.ConferenceCenter;

/// <summary>
/// 会议参与人子实体
/// </summary>
[SugarTable("takt_routine_conference_center_participant", "会议参与人表")]
[SugarIndex("ix_conference_participant_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_conference_participant_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_conference_participant_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ConferenceId), OrderByType.Asc, nameof(UserId), OrderByType.Asc, true)]
[SugarIndex("ix_conference_participant_conference_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ConferenceId), OrderByType.Asc, false)]
[SugarIndex("ix_conference_participant_user_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(UserId), OrderByType.Asc, false)]
public class TaktConferenceParticipant : TaktCompanyEntityBase
{
    /// <summary>
    /// 会议 ID
    /// </summary>
    [SugarColumn(ColumnName = "conference_id", ColumnDescription = "会议ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConferenceId { get; set; }
    /// <summary>
    /// 用户 ID
    /// </summary>
    [SugarColumn(ColumnName = "user_id", ColumnDescription = "用户ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long UserId { get; set; }
    /// <summary>
    /// 用户姓名
    /// </summary>
    [SugarColumn(ColumnName = "user_name", ColumnDescription = "用户姓名", ColumnDataType = "varchar", Length = 20, IsNullable = false)]
    public string UserName { get; set; } = string.Empty;
    /// <summary>
    /// 参与角色
    /// </summary>
    [SugarColumn(ColumnName = "participant_role", ColumnDescription = "参与角色", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public TaktConferenceParticipantRole ParticipantRole { get; set; } = TaktConferenceParticipantRole.Participant;
    /// <summary>
    /// 出席状态
    /// </summary>
    [SugarColumn(ColumnName = "attendance_status", ColumnDescription = "出席状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public TaktConferenceAttendanceStatus AttendanceStatus { get; set; } = TaktConferenceAttendanceStatus.Pending;
    /// <summary>
    /// 签到时间
    /// </summary>
    [SugarColumn(ColumnName = "check_in_time", ColumnDescription = "签到时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? CheckInTime { get; set; }
    /// <summary>
    /// 签退时间
    /// </summary>
    [SugarColumn(ColumnName = "check_out_time", ColumnDescription = "签退时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? CheckOutTime { get; set; }
    /// <summary>
    /// 会议（主表）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(ConferenceId))]
    public TaktConference? Conference { get; set; }
}
