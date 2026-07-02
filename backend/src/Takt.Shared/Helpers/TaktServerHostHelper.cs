// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktServerHostHelper.cs
// 创建时间：2026-06-25
// 创建人：Takt365(Cursor AI)
// 功能描述：服务端本机 IP/主机名解析（Quartz 执行日志等后台任务唯一入口）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Net;
using System.Net.Sockets;

namespace Takt.Shared.Helpers;

/// <summary>
/// 服务端本机网络标识辅助类（非 HTTP 客户端 UA 场景）
/// </summary>
public static class TaktServerHostHelper
{
    /// <summary>
    /// 解析本机主机名
    /// </summary>
    /// <returns>主机名；无法解析时返回空串</returns>
    public static string ResolveLocalMachineName()
    {
        return Environment.MachineName ?? string.Empty;
    }

    /// <summary>
    /// 解析本机 IPv4 地址
    /// </summary>
    /// <returns>IPv4 字符串；失败时回退 127.0.0.1</returns>
    public static string ResolveLocalIPv4()
    {
        try
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            var ip = host.AddressList.FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork);
            return ip?.ToString() ?? "127.0.0.1";
        }
        catch
        {
            return "127.0.0.1";
        }
    }

    /// <summary>
    /// Quartz 任务执行日志用的本机 IP 与主机名
    /// </summary>
    /// <returns>ExecuteIp 与 ExecuteHost</returns>
    public static (string ExecuteIp, string ExecuteHost) ResolveQuartzExecuteEndpoint()
    {
        return (ResolveLocalIPv4(), ResolveLocalMachineName());
    }
}
