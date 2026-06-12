// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Shared.Constants
// 文件名称：TaktQuartzConstants.cs
// 创建时间：2026-06-11
// 创建人：Takt365(Cursor AI)
// 功能描述：Quartz 调度跨层共享常量（HTTP 客户端名等）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Constants;

/// <summary>
/// Quartz 调度跨层共享常量
/// </summary>
public static class TaktQuartzConstants
{
    /// <summary>
    /// Quartz HTTP 任务 <see cref="System.Net.Http.IHttpClientFactory"/> 客户端名称
    /// </summary>
    public const string HttpClientName = "TaktQuartzHttp";
}
