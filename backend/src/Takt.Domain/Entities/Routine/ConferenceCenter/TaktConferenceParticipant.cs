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

using SqlSugar;
using Takt.Domain.Entities;

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
    /// 会议 ID（关联 TaktConference.Id，选项 TaktConferences/options）
    /// </summary>
    [SugarColumn(ColumnName = "conference_id", ColumnDescription = "会议ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConferenceId { get; set; }
    /// <summary>
    /// 用户 ID（关联 TaktUser.Id，选项 TaktUsers/options）
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
    /// 参与角色（字典 routine_conference_participant_role；0=参会人 1=主持人 2=记录人 3=嘉宾）
    /// </summary>
    [SugarColumn(ColumnName = "participant_role", ColumnDescription = "参与角色", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ParticipantRole { get; set; } = 0;
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
    /// 签到方式（字典 routine_conference_check_in_method；0=手动 1=扫码 2=人脸 3=门禁）
    /// </summary>
    [SugarColumn(ColumnName = "check_in_method", ColumnDescription = "签到方式", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int CheckInMethod { get; set; } = 0;
    /// <summary>
    /// 出席状态（字典 routine_conference_attendance_status；0=待确认 1=已出席 2=缺席 3=迟到 4=请假）
    /// </summary>
    [SugarColumn(ColumnName = "attendance_status", ColumnDescription = "出席状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int AttendanceStatus { get; set; } = 0;

    // ========================================
    // 导航属性区域
    // ========================================
    /// <summary>
    /// 会议（主表）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(ConferenceId))]
    public TaktConference? Conference { get; set; }
}
