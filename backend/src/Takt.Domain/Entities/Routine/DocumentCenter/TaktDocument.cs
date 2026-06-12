// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Routine.DocumentCenter
// 文件名称：TaktDocument.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：文管中心主实体，支持制度、流程、模板等文档的分类、版本与权限控制；需审批通过后发布
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Routine.DocumentCenter;

/// <summary>
/// 文管中心主实体
/// 支持制度、流程、模板等文档的分类、版本与权限控制；需审批通过后发布（草稿→审批→发布）
/// </summary>
[SugarTable("takt_routine_document_center", "文管中心表")]
[SugarIndex("ix_document_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_document_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_document_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(DocumentCode), OrderByType.Asc, true)]
[SugarIndex("ix_document_category", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(DocumentCategory), OrderByType.Asc, false)]
[SugarIndex("ix_document_status", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(DocumentStatus), OrderByType.Asc, false)]
[SugarIndex("ix_document_publish_time", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PublishTime), OrderByType.Desc, false)]
public class TaktDocument : TaktApprovalEntityBase
{
    /// <summary>
    /// 文档编码（租户+公司内唯一）
    /// </summary>
    [SugarColumn(ColumnName = "document_code", ColumnDescription = "文档编码", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string DocumentCode { get; set; } = string.Empty;
    /// <summary>
    /// 文档标题
    /// </summary>
    [SugarColumn(ColumnName = "title", ColumnDescription = "文档标题", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string Title { get; set; } = string.Empty;
    /// <summary>
    /// 文档分类
    /// </summary>
    [SugarColumn(ColumnName = "document_category", ColumnDescription = "文档分类", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int DocumentCategory { get; set; } = 0;
    /// <summary>
    /// 文档状态
    /// </summary>
    [SugarColumn(ColumnName = "document_status", ColumnDescription = "文档状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int DocumentStatus { get; set; } = 0;
    /// <summary>
    /// 密级
    /// </summary>
    [SugarColumn(ColumnName = "confidential_level", ColumnDescription = "密级", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ConfidentialLevel { get; set; } = 0;
    /// <summary>
    /// 当前版本号
    /// </summary>
    [SugarColumn(ColumnName = "version", ColumnDescription = "当前版本号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int Version { get; set; } = 0;
    /// <summary>
    /// 文档内容（富文本 HTML）
    /// </summary>
    [SugarColumn(ColumnName = "content", ColumnDescription = "文档内容", ColumnDataType = "ntext", IsNullable = true)]
    public string? Content { get; set; }
    /// <summary>
    /// 文档摘要（用于列表展示）
    /// </summary>
    [SugarColumn(ColumnName = "summary", ColumnDescription = "文档摘要", ColumnDataType = "nvarchar", Length = 2000, IsNullable = true)]
    public string? Summary { get; set; }
    /// <summary>
    /// 标签（逗号分隔或 JSON 数组存储）
    /// </summary>
    [SugarColumn(ColumnName = "tags", ColumnDescription = "标签", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? Tags { get; set; }
    /// <summary>
    /// 当前文件 ID
    /// </summary>
    [SugarColumn(ColumnName = "file_id", ColumnDescription = "当前文件ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FileId { get; set; }
    /// <summary>
    /// 当前文件名称
    /// </summary>
    [SugarColumn(ColumnName = "file_name", ColumnDescription = "当前文件名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? FileName { get; set; }
    /// <summary>
    /// 当前文件路径
    /// </summary>
    [SugarColumn(ColumnName = "file_path", ColumnDescription = "当前文件路径", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? FilePath { get; set; }
    /// <summary>
    /// 当前文件大小（字节）
    /// </summary>
    [SugarColumn(ColumnName = "file_size", ColumnDescription = "当前文件大小", ColumnDataType = "bigint", IsNullable = false, DefaultValue = "0")]
    public long FileSize { get; set; } = 0;
    /// <summary>
    /// 当前文件类型（MIME）
    /// </summary>
    [SugarColumn(ColumnName = "file_type", ColumnDescription = "当前文件类型", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? FileType { get; set; }
    /// <summary>
    /// 当前文件扩展名
    /// </summary>
    [SugarColumn(ColumnName = "file_extension", ColumnDescription = "当前文件扩展名", ColumnDataType = "nvarchar", Length = 20, IsNullable = true)]
    public string? FileExtension { get; set; }
    /// <summary>
    /// 生效时间
    /// </summary>
    [SugarColumn(ColumnName = "effective_time", ColumnDescription = "生效时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? EffectiveTime { get; set; }
    /// <summary>
    /// 失效时间
    /// </summary>
    [SugarColumn(ColumnName = "expire_time", ColumnDescription = "失效时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? ExpireTime { get; set; }
    /// <summary>
    /// 发布时间
    /// </summary>
    [SugarColumn(ColumnName = "publish_time", ColumnDescription = "发布时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? PublishTime { get; set; }
    /// <summary>
    /// 发布人 ID
    /// </summary>
    [SugarColumn(ColumnName = "publisher_id", ColumnDescription = "发布人ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PublisherId { get; set; }
    /// <summary>
    /// 发布人姓名
    /// </summary>
    [SugarColumn(ColumnName = "publisher_name", ColumnDescription = "发布人姓名", ColumnDataType = "varchar", Length = 20, IsNullable = false)]
    public string PublisherName { get; set; } = string.Empty;
    /// <summary>
    /// 归属部门 ID
    /// </summary>
    [SugarColumn(ColumnName = "dept_id", ColumnDescription = "归属部门ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }
    /// <summary>
    /// 归属部门名称
    /// </summary>
    [SugarColumn(ColumnName = "dept_name", ColumnDescription = "归属部门名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? DeptName { get; set; }
    /// <summary>
    /// 是否置顶
    /// </summary>
    [SugarColumn(ColumnName = "is_top", ColumnDescription = "是否置顶", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsTop { get; set; } = 0;
    /// <summary>
    /// 排序号
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;
    /// <summary>
    /// 浏览次数
    /// </summary>
    [SugarColumn(ColumnName = "view_count", ColumnDescription = "浏览次数", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ViewCount { get; set; } = 0;
    /// <summary>
    /// 下载次数
    /// </summary>
    [SugarColumn(ColumnName = "download_count", ColumnDescription = "下载次数", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int DownloadCount { get; set; } = 0;
    /// <summary>
    /// 目标范围（all=全员，company=本公司，department=本部门，custom=自定义）
    /// </summary>
    [SugarColumn(ColumnName = "target_scope", ColumnDescription = "目标范围", ColumnDataType = "nvarchar", Length = 20, IsNullable = false, DefaultValue = "all")]
    public string TargetScope { get; set; } = "all";
    /// <summary>
    /// 目标部门编码（多个用逗号分隔）
    /// </summary>
    [SugarColumn(ColumnName = "target_departments", ColumnDescription = "目标部门编码", ColumnDataType = "nvarchar", Length = 1000, IsNullable = true)]
    public string? TargetDepartments { get; set; }
    /// <summary>
    /// 目标用户 ID（多个用逗号分隔）
    /// </summary>
    [SugarColumn(ColumnName = "target_users", ColumnDescription = "目标用户ID", ColumnDataType = "nvarchar", Length = 2000, IsNullable = true)]
    public string? TargetUsers { get; set; }
    /// <summary>
    /// 版本历史列表（主子表关系）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktDocumentVersion.DocumentId))]
    public List<TaktDocumentVersion>? Versions { get; set; }
    /// <summary>
    /// 变更日志列表（主子表关系）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktDocumentChangeLog.DocumentId))]
    public List<TaktDocumentChangeLog>? ChangeLogs { get; set; }
}
