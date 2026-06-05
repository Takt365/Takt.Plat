// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.SignalR
// 文件名称：TaktSignalRGroupNames.cs
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：SignalR Hub 组名约定（租户+公司隔离）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Infrastructure.SignalR;

/// <summary>
/// SignalR Hub 组名工具
/// </summary>
internal static class TaktSignalRGroupNames
{
    /// <summary>
    /// 公司内用户组（私信、强退）
    /// </summary>
    /// <param name="companyCode">公司编码</param>
    /// <param name="userName">用户名</param>
    /// <returns>组名</returns>
    public static string UserGroup(string companyCode, string userName) =>
        $"Company_{companyCode}_User_{userName}";

    /// <summary>
    /// 公司内广播通知组
    /// </summary>
    /// <param name="companyCode">公司编码</param>
    /// <returns>组名</returns>
    public static string NotificationsGroup(string companyCode) =>
        $"Company_{companyCode}_Notifications";
}
