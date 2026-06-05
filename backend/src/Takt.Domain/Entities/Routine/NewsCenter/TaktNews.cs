// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Routine.NewsCenter
// 文件名称：TaktNews.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：新闻中心主实体，支持分类、置顶、推荐、社交统计；需审批通过后发布
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities;
using Takt.Shared.Enums;

namespace Takt.Domain.Entities.Routine.NewsCenter;

/// <summary>
/// 新闻中心主实体
/// 支持分类、置顶、推荐、社交统计；需审批通过后发布（草稿→审批→发布）
/// </summary>
[SugarTable("takt_routine_news_center", "新闻中心表")]
[SugarIndex("ix_news_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_news_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_news_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(NewsCode), OrderByType.Asc, true)]
[SugarIndex("ix_news_category", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(NewsCategory), OrderByType.Asc, false)]
[SugarIndex("ix_news_publish_time", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PublishTime), OrderByType.Desc, false)]
[SugarIndex("ix_news_status", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(NewsStatus), OrderByType.Asc, false)]
[SugarIndex("ix_news_flow_instance_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(FlowInstanceId), OrderByType.Asc, false)]
public class TaktNews : TaktApprovalEntityBase
{
    /// <summary>
    /// 新闻编码（租户+公司内唯一）
    /// </summary>
    [SugarColumn(ColumnName = "news_code", ColumnDescription = "新闻编码", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string NewsCode { get; set; } = string.Empty;
    /// <summary>
    /// 新闻分类
    /// </summary>
    [SugarColumn(ColumnName = "news_category", ColumnDescription = "新闻分类", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public TaktNewsCategory NewsCategory { get; set; } = TaktNewsCategory.CompanyNews;
    /// <summary>
    /// 新闻标题
    /// </summary>
    [SugarColumn(ColumnName = "news_title", ColumnDescription = "新闻标题", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string NewsTitle { get; set; } = string.Empty;
    /// <summary>
    /// 新闻摘要（用于列表展示）
    /// </summary>
    [SugarColumn(ColumnName = "news_summary", ColumnDescription = "新闻摘要", ColumnDataType = "nvarchar", Length = 2000, IsNullable = true)]
    public string? NewsSummary { get; set; }
    /// <summary>
    /// 标签（逗号分隔或 JSON 数组存储）
    /// </summary>
    [SugarColumn(ColumnName = "tags", ColumnDescription = "标签", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? Tags { get; set; }
    /// <summary>
    /// 新闻内容
    /// </summary>
    [SugarColumn(ColumnName = "news_content", ColumnDescription = "新闻内容", ColumnDataType = "ntext", IsNullable = false)]
    public string NewsContent { get; set; } = string.Empty;
    /// <summary>
    /// 新闻封面图片 URL
    /// </summary>
    [SugarColumn(ColumnName = "news_cover_image", ColumnDescription = "新闻封面图片URL", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? NewsCoverImage { get; set; }
    /// <summary>
    /// 是否置顶
    /// </summary>
    [SugarColumn(ColumnName = "is_top", ColumnDescription = "是否置顶", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public TaktYesNo IsTop { get; set; } = TaktYesNo.No;
    /// <summary>
    /// 是否推荐
    /// </summary>
    [SugarColumn(ColumnName = "is_recommended", ColumnDescription = "是否推荐", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public TaktYesNo IsRecommended { get; set; } = TaktYesNo.No;
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
    /// 阅读次数
    /// </summary>
    [SugarColumn(ColumnName = "read_count", ColumnDescription = "阅读次数", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ReadCount { get; set; } = 0;
    /// <summary>
    /// 点赞次数
    /// </summary>
    [SugarColumn(ColumnName = "like_count", ColumnDescription = "点赞次数", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int LikeCount { get; set; } = 0;
    /// <summary>
    /// 评论次数
    /// </summary>
    [SugarColumn(ColumnName = "comment_count", ColumnDescription = "评论次数", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int CommentCount { get; set; } = 0;
    /// <summary>
    /// 收藏次数
    /// </summary>
    [SugarColumn(ColumnName = "favorite_count", ColumnDescription = "收藏次数", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int FavoriteCount { get; set; } = 0;
    /// <summary>
    /// 分享次数
    /// </summary>
    [SugarColumn(ColumnName = "share_count", ColumnDescription = "分享次数", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ShareCount { get; set; } = 0;
    /// <summary>
    /// 附件数量
    /// </summary>
    [SugarColumn(ColumnName = "attachment_count", ColumnDescription = "附件数量", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int AttachmentCount { get; set; } = 0;
    /// <summary>
    /// 流程实例 ID（关联工作流，如发布审批流程；流程侧 BusinessType=News、BusinessKey=本表 Id）
    /// </summary>
    [SugarColumn(ColumnName = "flow_instance_id", ColumnDescription = "流程实例ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FlowInstanceId { get; set; }
    /// <summary>
    /// 发布部门 ID
    /// </summary>
    [SugarColumn(ColumnName = "dept_id", ColumnDescription = "发布部门ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }
    /// <summary>
    /// 发布部门名称
    /// </summary>
    [SugarColumn(ColumnName = "dept_name", ColumnDescription = "发布部门名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? DeptName { get; set; }
    /// <summary>
    /// 发布人 ID
    /// </summary>
    [SugarColumn(ColumnName = "publisher_id", ColumnDescription = "发布人ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PublisherId { get; set; }
    /// <summary>
    /// 发布人姓名
    /// </summary>
    [SugarColumn(ColumnName = "publisher_name", ColumnDescription = "发布人姓名", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string PublisherName { get; set; } = string.Empty;
    /// <summary>
    /// 发布时间
    /// </summary>
    [SugarColumn(ColumnName = "publish_time", ColumnDescription = "发布时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? PublishTime { get; set; }
    /// <summary>
    /// 排序号（越小越靠前）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;
    /// <summary>
    /// 新闻状态
    /// </summary>
    [SugarColumn(ColumnName = "news_status", ColumnDescription = "新闻状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public TaktNewsStatus NewsStatus { get; set; } = TaktNewsStatus.Draft;
    /// <summary>
    /// 新闻附件列表（主子表关系）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktNewsAttachment.NewsId))]
    public List<TaktNewsAttachment>? Attachments { get; set; }
    /// <summary>
    /// 新闻评论列表（主子表关系）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktNewsComment.NewsId))]
    public List<TaktNewsComment>? Comments { get; set; }
    /// <summary>
    /// 新闻点赞记录列表（主子表关系）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktNewsLike.NewsId))]
    public List<TaktNewsLike>? Likes { get; set; }
    /// <summary>
    /// 新闻阅读记录列表（主子表关系）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktNewsRead.NewsId))]
    public List<TaktNewsRead>? Reads { get; set; }
    /// <summary>
    /// 新闻收藏记录列表（主子表关系）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktNewsFavorite.NewsId))]
    public List<TaktNewsFavorite>? Favorites { get; set; }
    /// <summary>
    /// 新闻分享记录列表（主子表关系）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktNewsShare.NewsId))]
    public List<TaktNewsShare>? Shares { get; set; }
}
