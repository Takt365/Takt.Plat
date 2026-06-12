// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Routine.DocumentCenter
// 文件名称：TaktDocumentChangeLog.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：文管文档变更日志实体，记录创建、修订、发布、归档、删除等历史
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Routine.DocumentCenter;

/// <summary>
/// 文管文档变更日志实体
/// 完整记录文档的创建、修订、发布、归档、删除等历史
/// </summary>
[SugarTable("takt_routine_document_center_change_log", "文管文档变更日志表")]
[SugarIndex("ix_document_change_log_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_document_change_log_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_document_change_log_document_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(DocumentId), OrderByType.Asc, false)]
[SugarIndex("ix_document_change_log_change_type", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ChangeType), OrderByType.Asc, false)]
public class TaktDocumentChangeLog : TaktCompanyEntityBase
{
    /// <summary>
    /// 文档 ID
    /// </summary>
    [SugarColumn(ColumnName = "document_id", ColumnDescription = "文档ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DocumentId { get; set; }
    /// <summary>
    /// 文档编码（冗余，便于日志列表展示）
    /// </summary>
    [SugarColumn(ColumnName = "document_code", ColumnDescription = "文档编码", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? DocumentCode { get; set; }
    /// <summary>
    /// 文档标题（冗余，便于日志列表展示）
    /// </summary>
    [SugarColumn(ColumnName = "document_title", ColumnDescription = "文档标题", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? DocumentTitle { get; set; }
    /// <summary>
    /// 变更类型
    /// </summary>
    [SugarColumn(ColumnName = "change_type", ColumnDescription = "变更类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ChangeType { get; set; } = 0;
    /// <summary>
    /// 变更内容摘要
    /// </summary>
    [SugarColumn(ColumnName = "change_summary", ColumnDescription = "变更内容摘要", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? ChangeSummary { get; set; }
    /// <summary>
    /// 变更字段列表（JSON 数组）
    /// </summary>
    [SugarColumn(ColumnName = "change_fields", ColumnDescription = "变更字段列表", ColumnDataType = "nvarchar", Length = -1, IsNullable = true)]
    public string? ChangeFields { get; set; }
    /// <summary>
    /// 变更原因或备注
    /// </summary>
    [SugarColumn(ColumnName = "change_reason", ColumnDescription = "变更原因", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? ChangeReason { get; set; }
    /// <summary>
    /// 变更时文档版本号
    /// </summary>
    [SugarColumn(ColumnName = "version_at_change", ColumnDescription = "变更时文档版本号", ColumnDataType = "int", IsNullable = true)]
    public int? VersionAtChange { get; set; }
    /// <summary>
    /// 文档（主表）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(DocumentId))]
    public TaktDocument? Document { get; set; }
}
