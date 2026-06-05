// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Routine.Announcement
// 文件名称：TaktAnnouncement.cs
// 创建时间：2026-05-18
// 创建人：Takt365(Cursor AI)
// 功能描述：公告通知实体，用于发布系统公告、通知、新闻等
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;
using Takt.Shared.Enums;

namespace Takt.Domain.Entities.Routine.Announcement;

/// <summary>
/// 公告通知实体
/// 用于发布系统公告、通知、新闻等信息
/// 支持富文本内容、附件、置顶、定时发布等功能
/// 需要审批流程：草稿→审批→发布
/// </summary>
[SugarTable("takt_routine_announcement", "公告通知表")]
[SugarIndex("ix_announcement_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_announcement_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_announcement_publish_time", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PublishTime), OrderByType.Desc, false)]
public class TaktAnnouncement : TaktApprovalEntityBase
{
    /// <summary>
    /// 公告标题
    /// </summary>
    [SugarColumn(ColumnName = "title", ColumnDescription = "公告标题", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 公告类型（1=公告，2=通知，3=新闻，4=紧急通知）
    /// </summary>
    [SugarColumn(ColumnName = "announcement_type", ColumnDescription = "公告类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public TaktAnnouncementType AnnouncementType { get; set; } = TaktAnnouncementType.Announcement;

    /// <summary>
    /// 公告内容（富文本 HTML）
    /// </summary>
    [SugarColumn(ColumnName = "content", ColumnDescription = "公告内容", ColumnDataType = "ntext", IsNullable = false)]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 公告摘要（用于列表展示）
    /// </summary>
    [SugarColumn(ColumnName = "summary", ColumnDescription = "公告摘要", ColumnDataType = "nvarchar", Length = 2000, IsNullable = true)]
    public string? Summary { get; set; }

    /// <summary>
    /// 标签（逗号分隔或 JSON 数组存储）
    /// </summary>
    [SugarColumn(ColumnName = "tags", ColumnDescription = "标签", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? Tags { get; set; }

    /// <summary>
    /// 附件路径（多个附件用逗号分隔）
    /// </summary>
    [SugarColumn(ColumnName = "attachments", ColumnDescription = "附件路径", ColumnDataType = "nvarchar", Length = 2000, IsNullable = true)]
    public string? Attachments { get; set; }

    /// <summary>
    /// 发布时间（定时发布时使用）
    /// </summary>
    [SugarColumn(ColumnName = "publish_time", ColumnDescription = "发布时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? PublishTime { get; set; }

    /// <summary>
    /// 是否定时发布（1=是，0=否）
    /// </summary>
    [SugarColumn(ColumnName = "is_scheduled", ColumnDescription = "是否定时发布", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public TaktYesNo IsScheduled { get; set; } = TaktYesNo.No;

    /// <summary>
    /// 是否置顶（1=是，0=否）
    /// </summary>
    [SugarColumn(ColumnName = "is_top", ColumnDescription = "是否置顶", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public TaktYesNo IsTop { get; set; } = TaktYesNo.No;

    /// <summary>
    /// 置顶优先级（数字越大越靠前）
    /// </summary>
    [SugarColumn(ColumnName = "top_priority", ColumnDescription = "置顶优先级", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int TopPriority { get; set; } = 0;

    /// <summary>
    /// 过期时间（过期后自动隐藏）
    /// </summary>
    [SugarColumn(ColumnName = "expire_time", ColumnDescription = "过期时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? ExpireTime { get; set; }

    /// <summary>
    /// 查看次数
    /// </summary>
    [SugarColumn(ColumnName = "view_count", ColumnDescription = "查看次数", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ViewCount { get; set; } = 0;

    /// <summary>
    /// 目标范围（all=全员，company=本公司，department=本部门，custom=自定义）
    /// </summary>
    [SugarColumn(ColumnName = "target_scope", ColumnDescription = "目标范围", ColumnDataType = "varchar", Length = 20, IsNullable = false, DefaultValue = "all")]
    public string TargetScope { get; set; } = "all";

    /// <summary>
    /// 目标部门编码（多个用逗号分隔，当 target_scope=department 时使用）
    /// </summary>
    [SugarColumn(ColumnName = "target_departments", ColumnDescription = "目标部门编码", ColumnDataType = "nvarchar", Length = 1000, IsNullable = true)]
    public string? TargetDepartments { get; set; }

    /// <summary>
    /// 目标用户 ID（多个用逗号分隔，当 target_scope=custom 时使用）
    /// </summary>
    [SugarColumn(ColumnName = "target_users", ColumnDescription = "目标用户ID", ColumnDataType = "nvarchar", Length = 2000, IsNullable = true)]
    public string? TargetUsers { get; set; }

    /// <summary>
    /// 状态（0=草稿，1=已发布，2=已撤回，3=已过期）
    /// </summary>
    [SugarColumn(ColumnName = "announcement_status", ColumnDescription = "状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public TaktAnnouncementStatus AnnouncementStatus { get; set; } = TaktAnnouncementStatus.Draft;
}
