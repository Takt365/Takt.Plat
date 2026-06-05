// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Shared.Enums
// 文件名称：TaktMenuEnums.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：菜单相关枚举
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;

namespace Takt.Shared.Enums;

/// <summary>
/// 菜单类型枚举
/// </summary>
public enum TaktMenuType
{
    /// <summary>
    /// 目录（导航分组）
    /// </summary>
    [Display(Name = "目录")]
    Directory = 0,

    /// <summary>
    /// 菜单（页面链接）
    /// </summary>
    [Display(Name = "菜单")]
    Menu = 1,

    /// <summary>
    /// 按钮（操作权限）
    /// </summary>
    [Display(Name = "按钮")]
    Button = 2
}
