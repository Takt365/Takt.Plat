// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Routine.ConferenceCenter
// 文件名称：TaktConferenceAgenda.cs
// 创建时间：2026-06-11
// 创建人：Takt365(Cursor AI)
// 功能描述：会议议程/纪要实体，按会议维护议程项与会议纪要正文
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Routine.ConferenceCenter;

/// <summary>
/// 会议议程/纪要实体
/// RecordType=议程项时多行维护议题；RecordType=会议纪要时通常一条记录承载正文与摘要
/// </summary>
[SugarTable("takt_routine_conference_center_agenda", "会议议程纪要表")]
[SugarIndex("ix_conference_agenda_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_conference_agenda_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_conference_agenda_conference_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ConferenceId), OrderByType.Asc, false)]
[SugarIndex("ix_conference_agenda_record_type", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ConferenceId), OrderByType.Asc, nameof(RecordType), OrderByType.Asc, false)]
public class TaktConferenceAgenda : TaktCompanyEntityBase
{
    /// <summary>
    /// 会议 ID（关联 TaktConference.Id，选项 TaktConferences/options）
    /// </summary>
    [SugarColumn(ColumnName = "conference_id", ColumnDescription = "会议ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConferenceId { get; set; }
    /// <summary>
    /// 记录类型（字典 routine_conference_record_type；0=议程项 1=会议纪要）
    /// </summary>
    [SugarColumn(ColumnName = "record_type", ColumnDescription = "记录类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int RecordType { get; set; } = 0;
    /// <summary>
    /// 行号（议程项序号，固定步长=10；纪要通常为 10）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "行号", ColumnDataType = "int", IsNullable = false, DefaultValue = "10")]
    public int LineNumber { get; set; } = 10;
    /// <summary>
    /// 标题（议程议题或纪要标题）
    /// </summary>
    [SugarColumn(ColumnName = "conference_agenda_title", ColumnDescription = "标题", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string ConferenceAgendaTitle { get; set; } = string.Empty;
    /// <summary>
    /// 正文（议程说明或会议纪要富文本 HTML）
    /// </summary>
    [SugarColumn(ColumnName = "conference_agenda_content", ColumnDescription = "正文", ColumnDataType = "ntext", IsNullable = true)]
    public string? ConferenceAgendaContent { get; set; }
    /// <summary>
    /// 摘要（纪要列表展示用）
    /// </summary>
    [SugarColumn(ColumnName = "conference_agenda_summary", ColumnDescription = "摘要", ColumnDataType = "nvarchar", Length = 2000, IsNullable = true)]
    public string? ConferenceAgendaSummary { get; set; }
    /// <summary>
    /// 主讲人/汇报人 ID（关联 TaktUser.Id，选项 TaktUsers/options）
    /// </summary>
    [SugarColumn(ColumnName = "presenter_id", ColumnDescription = "主讲人ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PresenterId { get; set; }
    /// <summary>
    /// 主讲人姓名（议程项）
    /// </summary>
    [SugarColumn(ColumnName = "presenter_name", ColumnDescription = "主讲人姓名", ColumnDataType = "varchar", Length = 40, IsNullable = true)]
    public string? PresenterName { get; set; }
    /// <summary>
    /// 计划开始时间（议程项）
    /// </summary>
    [SugarColumn(ColumnName = "planned_start_time", ColumnDescription = "计划开始时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? PlannedStartTime { get; set; }
    /// <summary>
    /// 计划时长（分钟，议程项）
    /// </summary>
    [SugarColumn(ColumnName = "duration_minutes", ColumnDescription = "计划时长分钟", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int DurationMinutes { get; set; } = 0;
    /// <summary>
    /// 记录人 ID（关联 TaktUser.Id，选项 TaktUsers/options）
    /// </summary>
    [SugarColumn(ColumnName = "recorder_id", ColumnDescription = "记录人ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RecorderId { get; set; }
    /// <summary>
    /// 记录人姓名（会议纪要）
    /// </summary>
    [SugarColumn(ColumnName = "recorder_name", ColumnDescription = "记录人姓名", ColumnDataType = "varchar", Length = 40, IsNullable = true)]
    public string? RecorderName { get; set; }
    /// <summary>
    /// 附件（JSON 列表形式，由 TaktFile 统一上传到服务器）
    /// </summary>
    [SugarColumn(ColumnName = "attachments", ColumnDescription = "附件JSON", ColumnDataType = "nvarchar", Length = -1, IsNullable = true)]
    public string? Attachments { get; set; }

    // ========================================
    // 导航属性区域
    // ========================================
    /// <summary>
    /// 会议（主表）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(ConferenceId))]
    public TaktConference? Conference { get; set; }
}
