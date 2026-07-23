// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Routine.NewsCenter
// 文件名称：TaktNewsAttachment.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：新闻中心附件实体
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Routine.NewsCenter;

/// <summary>
/// 新闻中心附件实体
/// </summary>
[SugarTable("takt_routine_news_center_attachment", "新闻中心附件表")]
[SugarIndex("ix_news_attachment_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_news_attachment_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_news_attachment_news_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(NewsId), OrderByType.Asc, false)]
[SugarIndex("ix_news_attachment_file_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(FileId), OrderByType.Asc, false)]
public class TaktNewsAttachment : TaktCompanyEntityBase
{
    /// <summary>
    /// 新闻 ID（选项 TaktNews/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "news_id", ColumnDescription = "新闻ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long NewsId { get; set; }
    /// <summary>
    /// 文件 ID（选项 TaktFiles/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "file_id", ColumnDescription = "文件ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FileId { get; set; }
    /// <summary>
    /// 文件名称
    /// </summary>
    [SugarColumn(ColumnName = "file_name", ColumnDescription = "文件名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string FileName { get; set; } = string.Empty;
    /// <summary>
    /// 文件路径
    /// </summary>
    [SugarColumn(ColumnName = "file_path", ColumnDescription = "文件路径", ColumnDataType = "nvarchar", Length = 500, IsNullable = false)]
    public string FilePath { get; set; } = string.Empty;
    /// <summary>
    /// 文件大小（字节）
    /// </summary>
    [SugarColumn(ColumnName = "file_size", ColumnDescription = "文件大小（字节）", ColumnDataType = "bigint", IsNullable = false, DefaultValue = "0")]
    public long FileSize { get; set; } = 0;
    /// <summary>
    /// 文件类型（MIME 类型）
    /// </summary>
    [SugarColumn(ColumnName = "file_type", ColumnDescription = "文件类型", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? FileType { get; set; }
    /// <summary>
    /// 文件扩展名
    /// </summary>
    [SugarColumn(ColumnName = "file_extension", ColumnDescription = "文件扩展名", ColumnDataType = "nvarchar", Length = 20, IsNullable = true)]
    public string? FileExtension { get; set; }
    /// <summary>
    /// 排序号（越小越靠前）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;
    /// <summary>
    /// 新闻（主表）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(NewsId))]
    public TaktNews? News { get; set; }
}
