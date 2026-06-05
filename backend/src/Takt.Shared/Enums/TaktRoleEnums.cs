// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Shared.Enums
// 文件名称：TaktRoleEnums.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：角色类型枚举
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;

namespace Takt.Shared.Enums;

/// <summary>
/// 角色类型枚举
/// </summary>
public enum TaktRoleType
{
    /// <summary>
    /// 普通角色
    /// </summary>
    [Display(Name = "普通角色")]
    Normal = 0,

    /// <summary>
    /// 管理员角色
    /// </summary>
    [Display(Name = "管理员角色")]
    Admin = 1,

    /// <summary>
    /// 超级管理员角色
    /// </summary>
    [Display(Name = "超级管理员角色")]
    SuperAdmin = 2
}
