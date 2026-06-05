// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Shared.Enums
// 文件名称：TaktLoginEnums.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：登录结果枚举定义
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;

namespace Takt.Shared.Enums;

/// <summary>
/// 登录结果枚举
/// </summary>
public enum TaktLoginResult
{
    /// <summary>
    /// 登录成功
    /// </summary>
    [Display(Name = "登录成功")]
    Success = 1,

    /// <summary>
    /// 失败-密码错误
    /// </summary>
    [Display(Name = "密码错误")]
    PasswordError = 2,

    /// <summary>
    /// 失败-用户不存在
    /// </summary>
    [Display(Name = "用户不存在")]
    UserNotFound = 3,

    /// <summary>
    /// 失败-用户已禁用
    /// </summary>
    [Display(Name = "用户已禁用")]
    UserDisabled = 4,

    /// <summary>
    /// 失败-用户已锁定
    /// </summary>
    [Display(Name = "用户已锁定")]
    UserLocked = 5,

    /// <summary>
    /// 失败-验证码错误
    /// </summary>
    [Display(Name = "验证码错误")]
    CaptchaError = 6
}
