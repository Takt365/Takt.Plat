// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Shared.Enums
// 文件名称：TaktNewsEnums.cs
// 创建时间：2026-06-04
// 创建人：Takt365(Cursor AI)
// 功能描述：新闻中心相关枚举
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;

namespace Takt.Shared.Enums;

/// <summary>
/// 新闻分类枚举
/// </summary>
public enum TaktNewsCategory
{
    /// <summary>
    /// 公司新闻
    /// </summary>
    [Display(Name = "公司新闻")]
    CompanyNews = 0,
    /// <summary>
    /// 行业动态
    /// </summary>
    [Display(Name = "行业动态")]
    IndustryTrend = 1,
    /// <summary>
    /// 技术分享
    /// </summary>
    [Display(Name = "技术分享")]
    TechShare = 2,
    /// <summary>
    /// 产品发布
    /// </summary>
    [Display(Name = "产品发布")]
    ProductRelease = 3,
    /// <summary>
    /// 活动资讯
    /// </summary>
    [Display(Name = "活动资讯")]
    EventInfo = 4,
    /// <summary>
    /// 其他
    /// </summary>
    [Display(Name = "其他")]
    Other = 5
}

/// <summary>
/// 新闻状态枚举
/// </summary>
public enum TaktNewsStatus
{
    /// <summary>
    /// 草稿
    /// </summary>
    [Display(Name = "草稿")]
    Draft = 0,
    /// <summary>
    /// 已发布
    /// </summary>
    [Display(Name = "已发布")]
    Published = 1,
    /// <summary>
    /// 已撤回
    /// </summary>
    [Display(Name = "已撤回")]
    Withdrawn = 2,
    /// <summary>
    /// 已过期
    /// </summary>
    [Display(Name = "已过期")]
    Expired = 3
}

/// <summary>
/// 新闻评论状态枚举
/// </summary>
public enum TaktNewsCommentStatus
{
    /// <summary>
    /// 正常
    /// </summary>
    [Display(Name = "正常")]
    Normal = 0,
    /// <summary>
    /// 已隐藏
    /// </summary>
    [Display(Name = "已隐藏")]
    Hidden = 1
}
