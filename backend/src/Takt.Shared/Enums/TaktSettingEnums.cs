// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Shared.Enums
// 文件名称：TaktSettingEnums.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：系统设置相关枚举
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;

namespace Takt.Shared.Enums;

/// <summary>
/// 设置值类型枚举
/// </summary>
public enum TaktSettingValueType
{
    /// <summary>
    /// 字符串
    /// </summary>
    [Display(Name = "字符串")]
    String = 0,

    /// <summary>
    /// 数字
    /// </summary>
    [Display(Name = "数字")]
    Number = 1,

    /// <summary>
    /// 布尔值
    /// </summary>
    [Display(Name = "布尔值")]
    Boolean = 2,

    /// <summary>
    /// JSON对象
    /// </summary>
    [Display(Name = "JSON对象")]
    Json = 3,

    /// <summary>
    /// 数组
    /// </summary>
    [Display(Name = "数组")]
    Array = 4
}
