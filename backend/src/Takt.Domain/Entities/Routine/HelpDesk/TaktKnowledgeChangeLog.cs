// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Routine.HelpDesk
// 文件名称：TaktKnowledgeChangeLog.cs
// 创建时间：2025-02-26
// 创建人：Takt365(Cursor AI)
// 功能描述：知识库变更日志实体
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Routine.HelpDesk;

/// <summary>
/// 知识库变更日志实体
/// </summary>
[SugarTable("takt_routine_help_desk_knowledge_change_log", "知识库变更日志表")]
[SugarIndex("ix_knowledge_change_log_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_knowledge_change_log_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_knowledge_change_log_knowledge_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(KnowledgeId), OrderByType.Asc, false)]
[SugarIndex("ix_knowledge_change_log_created_at", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(CreatedAt), OrderByType.Desc, false)]
[SugarIndex("ix_knowledge_change_log_change_type", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ChangeType), OrderByType.Asc, false)]
public class TaktKnowledgeChangeLog : TaktCompanyEntityBase
{
    /// <summary>
    /// 知识 ID
    /// </summary>
    [SugarColumn(ColumnName = "knowledge_id", ColumnDescription = "知识ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long KnowledgeId { get; set; }

    /// <summary>
    /// 知识标题（冗余）
    /// </summary>
    [SugarColumn(ColumnName = "knowledge_title", ColumnDescription = "知识标题", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? KnowledgeTitle { get; set; }

    /// <summary>
    /// 变更类型（0=创建，1=更新，2=删除）
    /// </summary>
    [SugarColumn(ColumnName = "change_type", ColumnDescription = "变更类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int ChangeType { get; set; } = 1;

    /// <summary>
    /// 修改内容摘要
    /// </summary>
    [SugarColumn(ColumnName = "change_summary", ColumnDescription = "修改内容摘要", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
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
    /// 变更时知识版本号
    /// </summary>
    [SugarColumn(ColumnName = "version_at_change", ColumnDescription = "变更时版本号", ColumnDataType = "int", IsNullable = true)]
    public int? VersionAtChange { get; set; }

    /// <summary>
    /// 知识库（主表）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(KnowledgeId))]
    public TaktKnowledge? Knowledge { get; set; }
}
