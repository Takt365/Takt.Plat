// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Routine.HelpDesk
// 文件名称：TaktTicketChangeLog.cs
// 创建时间：2025-02-26
// 创建人：Takt365(Cursor AI)
// 功能描述：工单变更日志实体
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Routine.HelpDesk;

/// <summary>
/// 工单变更日志实体
/// </summary>
[SugarTable("takt_routine_help_desk_ticket_change_log", "工单变更日志表")]
[SugarIndex("ix_ticket_change_log_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_ticket_change_log_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_ticket_change_log_ticket_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(TicketId), OrderByType.Asc, false)]
[SugarIndex("ix_ticket_change_log_created_at", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(CreatedAt), OrderByType.Desc, false)]
[SugarIndex("ix_ticket_change_log_change_type", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ChangeType), OrderByType.Asc, false)]
public class TaktTicketChangeLog : TaktCompanyEntityBase
{
    /// <summary>
    /// 工单 ID（关联 TaktTicket.Id，选项 TaktTickets/options）
    /// </summary>
    [SugarColumn(ColumnName = "ticket_id", ColumnDescription = "工单ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TicketId { get; set; }

    /// <summary>
    /// 工单编号（冗余，便于日志列表展示）
    /// </summary>
    [SugarColumn(ColumnName = "ticket_no", ColumnDescription = "工单编号", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? TicketNo { get; set; }

    /// <summary>
    /// 变更类型（字典 sys_entity_change_type；0=创建 1=更新 2=删除 3=状态变更）
    /// </summary>
    [SugarColumn(ColumnName = "change_type", ColumnDescription = "变更类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int ChangeType { get; set; } = 1;

    /// <summary>
    /// 修改工单内容摘要
    /// </summary>
    [SugarColumn(ColumnName = "change_summary", ColumnDescription = "修改工单内容摘要", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? ChangeSummary { get; set; }

    /// <summary>
    /// 变更字段列表（JSON 数组）
    /// </summary>
    [SugarColumn(ColumnName = "change_fields", ColumnDescription = "变更字段列表", ColumnDataType = "nvarchar", Length = 4000, IsNullable = true)]
    public string? ChangeFields { get; set; }

    /// <summary>
    /// 变更原因或备注
    /// </summary>
    [SugarColumn(ColumnName = "change_reason", ColumnDescription = "变更原因", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? ChangeReason { get; set; }

    /// <summary>
    /// 工单（主表）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(TicketId))]
    public TaktTicket? Ticket { get; set; }
}
