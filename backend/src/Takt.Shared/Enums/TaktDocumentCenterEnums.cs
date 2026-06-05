// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Shared.Enums
// 文件名称：TaktDocumentCenterEnums.cs
// 创建时间：2026-06-04
// 创建人：Takt365(Cursor AI)
// 功能描述：文管中心相关枚举
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;

namespace Takt.Shared.Enums;

/// <summary>
/// 文管文档分类枚举
/// </summary>
public enum TaktDocumentCategory
{
    /// <summary>
    /// 制度
    /// </summary>
    [Display(Name = "制度")]
    Policy = 0,
    /// <summary>
    /// 流程
    /// </summary>
    [Display(Name = "流程")]
    Process = 1,
    /// <summary>
    /// 模板
    /// </summary>
    [Display(Name = "模板")]
    Template = 2,
    /// <summary>
    /// 手册
    /// </summary>
    [Display(Name = "手册")]
    Manual = 3,
    /// <summary>
    /// 其他
    /// </summary>
    [Display(Name = "其他")]
    Other = 4
}

/// <summary>
/// 文管文档状态枚举
/// </summary>
public enum TaktDocumentStatus
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
    /// 已归档
    /// </summary>
    [Display(Name = "已归档")]
    Archived = 2,
    /// <summary>
    /// 已作废
    /// </summary>
    [Display(Name = "已作废")]
    Obsolete = 3
}

/// <summary>
/// 文管文档密级枚举
/// </summary>
public enum TaktDocumentConfidentialLevel
{
    /// <summary>
    /// 公开
    /// </summary>
    [Display(Name = "公开")]
    Public = 0,
    /// <summary>
    /// 内部
    /// </summary>
    [Display(Name = "内部")]
    Internal = 1,
    /// <summary>
    /// 机密
    /// </summary>
    [Display(Name = "机密")]
    Confidential = 2,
    /// <summary>
    /// 绝密
    /// </summary>
    [Display(Name = "绝密")]
    Secret = 3
}

/// <summary>
/// 文管文档变更类型枚举
/// </summary>
public enum TaktDocumentChangeType
{
    /// <summary>
    /// 创建
    /// </summary>
    [Display(Name = "创建")]
    Create = 0,
    /// <summary>
    /// 修订
    /// </summary>
    [Display(Name = "修订")]
    Revise = 1,
    /// <summary>
    /// 发布
    /// </summary>
    [Display(Name = "发布")]
    Publish = 2,
    /// <summary>
    /// 归档
    /// </summary>
    [Display(Name = "归档")]
    Archive = 3,
    /// <summary>
    /// 删除
    /// </summary>
    [Display(Name = "删除")]
    Delete = 4
}
