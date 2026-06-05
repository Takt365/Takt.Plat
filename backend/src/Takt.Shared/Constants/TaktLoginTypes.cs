// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Shared.Constants
// 文件名称：TaktLoginTypes.cs
// 创建时间：2026-05-23
// 创建人：Takt365(Cursor AI)
// 功能描述：登录方式常量（与登录日志 LoginType 字段一致）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Constants;

/// <summary>
/// 登录方式常量
/// </summary>
public static class TaktLoginTypes
{
    /// <summary>
    /// 账号密码（grant_type=password）
    /// </summary>
    public const string Password = "Password";

    /// <summary>
    /// 刷新令牌（grant_type=refresh_token）
    /// </summary>
    public const string RefreshToken = "RefreshToken";

    /// <summary>
    /// 客户端凭证（grant_type=client_credentials）
    /// </summary>
    public const string ClientCredentials = "ClientCredentials";

    /// <summary>
    /// 授权码换令牌（grant_type=authorization_code）
    /// </summary>
    public const string AuthorizationCode = "AuthorizationCode";

    /// <summary>
    /// OAuth 授权页登录（/connect/authorize）
    /// </summary>
    public const string OAuthAuthorize = "OAuthAuthorize";

    /// <summary>
    /// 登录预检验密（session/verify-password）
    /// </summary>
    public const string VerifyPassword = "VerifyPassword";

    /// <summary>
    /// 注销会话（session/signout）
    /// </summary>
    public const string SignOut = "SignOut";
}
