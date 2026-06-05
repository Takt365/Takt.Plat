// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Shared.Enums
// 文件名称：TaktAnnouncementEnums.cs
// 创建时间：2026-05-18
// 创建人：Takt365(Cursor AI)
// 功能描述：公告通知相关枚举
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;

namespace Takt.Shared.Enums;

/// <summary>
/// 公告类型枚举
/// </summary>
public enum TaktAnnouncementType
{
    /// <summary>
    /// 公告
    /// </summary>
    [Display(Name = "公告")]
    Announcement = 1,

    /// <summary>
    /// 通知
    /// </summary>
    [Display(Name = "通知")]
    Notification = 2,

    /// <summary>
    /// 新闻
    /// </summary>
    [Display(Name = "新闻")]
    News = 3,

    /// <summary>
    /// 紧急通知
    /// </summary>
    [Display(Name = "紧急通知")]
    Emergency = 4
}

/// <summary>
/// 公告状态枚举
/// </summary>
public enum TaktAnnouncementStatus
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
