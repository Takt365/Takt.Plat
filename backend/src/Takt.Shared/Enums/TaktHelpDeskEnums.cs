// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Shared.Enums
// 文件名称：TaktHelpDeskEnums.cs
// 创建时间：2026-06-04
// 创建人：Takt365(Cursor AI)
// 功能描述：服务台（HelpDesk）相关枚举
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;

namespace Takt.Shared.Enums;

/// <summary>
/// 知识库状态枚举
/// </summary>
public enum TaktKnowledgeStatus
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
    /// 已下架
    /// </summary>
    [Display(Name = "已下架")]
    Unpublished = 2
}

/// <summary>
/// 自助服务类型枚举
/// </summary>
public enum TaktSelfServiceType
{
    /// <summary>
    /// 链接
    /// </summary>
    [Display(Name = "链接")]
    Link = 0,
    /// <summary>
    /// 表单
    /// </summary>
    [Display(Name = "表单")]
    Form = 1,
    /// <summary>
    /// 知识引导
    /// </summary>
    [Display(Name = "知识引导")]
    KnowledgeGuide = 2
}

/// <summary>
/// 服务台变更类型枚举（工单/知识库变更日志共用）
/// </summary>
public enum TaktHelpDeskChangeType
{
    /// <summary>
    /// 创建
    /// </summary>
    [Display(Name = "创建")]
    Create = 0,
    /// <summary>
    /// 更新
    /// </summary>
    [Display(Name = "更新")]
    Update = 1,
    /// <summary>
    /// 关闭或删除
    /// </summary>
    [Display(Name = "关闭或删除")]
    CloseOrDelete = 2
}
