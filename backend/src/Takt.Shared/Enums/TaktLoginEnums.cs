// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Shared.Enums
// 文件名称：TaktLoginEnums.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：认证/登录审计专用枚举（非字典项）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;

namespace Takt.Shared.Enums;

/// <summary>
/// 登录方式（认证流程写入登录日志）
/// </summary>
public enum TaktLoginType
{
    /// <summary>账号密码</summary>
    [Display(Name = "账号密码")]
    Password = 1,
    /// <summary>刷新令牌</summary>
    [Display(Name = "刷新令牌")]
    RefreshToken = 2,
    /// <summary>客户端凭证</summary>
    [Display(Name = "客户端凭证")]
    ClientCredentials = 3,
    /// <summary>授权码换令牌</summary>
    [Display(Name = "授权码换令牌")]
    AuthorizationCode = 4,
    /// <summary>OAuth 授权页登录</summary>
    [Display(Name = "OAuth授权登录")]
    OAuthAuthorize = 5,
    /// <summary>登录预检验密</summary>
    [Display(Name = "预检验密")]
    VerifyPassword = 6,
    /// <summary>注销会话</summary>
    [Display(Name = "注销会话")]
    SignOut = 7,
}

/// <summary>
/// 登录结果
/// </summary>
public enum TaktLoginResult
{
    /// <summary>登录成功</summary>
    [Display(Name = "登录成功")]
    Success = 1,
    /// <summary>密码错误</summary>
    [Display(Name = "密码错误")]
    PasswordError = 2,
    /// <summary>用户不存在</summary>
    [Display(Name = "用户不存在")]
    UserNotFound = 3,
    /// <summary>用户已禁用</summary>
    [Display(Name = "用户已禁用")]
    UserDisabled = 4,
    /// <summary>用户已锁定</summary>
    [Display(Name = "用户已锁定")]
    UserLocked = 5,
    /// <summary>验证码错误</summary>
    [Display(Name = "验证码错误")]
    CaptchaError = 6,
}

/// <summary>
/// 浏览器类型（User-Agent 解析，登录/在线审计）
/// </summary>
public enum TaktBrowserType
{
    /// <summary>未知</summary>
    [Display(Name = "未知")]
    Unknown = 0,
    /// <summary>Chrome</summary>
    [Display(Name = "Chrome")]
    Chrome = 1,
    /// <summary>Firefox</summary>
    [Display(Name = "Firefox")]
    Firefox = 2,
    /// <summary>Safari</summary>
    [Display(Name = "Safari")]
    Safari = 3,
    /// <summary>Edge</summary>
    [Display(Name = "Edge")]
    Edge = 4,
}

/// <summary>
/// 操作系统类型（User-Agent 解析，登录/在线审计）
/// </summary>
public enum TaktOperatingSystem
{
    /// <summary>未知</summary>
    [Display(Name = "未知")]
    Unknown = 0,
    /// <summary>Windows</summary>
    [Display(Name = "Windows")]
    Windows = 1,
    /// <summary>macOS</summary>
    [Display(Name = "macOS")]
    MacOS = 2,
    /// <summary>Linux</summary>
    [Display(Name = "Linux")]
    Linux = 3,
    /// <summary>Android</summary>
    [Display(Name = "Android")]
    Android = 4,
    /// <summary>iOS</summary>
    [Display(Name = "iOS")]
    IOS = 5,
}
