// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktOssHelper.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Cursor AI)
// 功能描述：OSS 对象存储配置读取帮助类
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Takt.Shared.Options;

namespace Takt.Shared.Helpers;

/// <summary>
/// OSS 对象存储配置帮助类
/// </summary>
/// <remarks>无状态；从配置解析选项，不缓存连接。</remarks>
public static class TaktOssHelper
{
    /// <summary>
    /// 从配置中读取 OSS 设置（键与字典 <c>sys_oss_provider</c> 一致，如 <c>aliyun</c>）。
    /// </summary>
    /// <param name="configuration">配置</param>
    /// <param name="provider">OSS 提供商标识，对应 <c>Oss:{provider}</c> 节点</param>
    /// <returns>OSS 配置</returns>
    /// <exception cref="ArgumentNullException"><paramref name="configuration"/> 为 null</exception>
    /// <exception cref="ArgumentException"><paramref name="provider"/> 为空</exception>
    public static TaktOssOptions GetOssOptionsFromConfiguration(IConfiguration configuration, string provider)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentException.ThrowIfNullOrWhiteSpace(provider);
        return configuration.RequireOssProvider(provider);
    }

    /// <summary>
    /// 从 JSON 字符串解析 OSS 配置
    /// </summary>
    /// <param name="jsonConfig">JSON 配置字符串</param>
    /// <returns>OSS 配置；解析失败返回 null</returns>
    public static TaktOssOptions? GetOssOptionsFromJson(string? jsonConfig)
    {
        if (string.IsNullOrWhiteSpace(jsonConfig))
            return null;

        try
        {
            return JsonConvert.DeserializeObject<TaktOssOptions>(jsonConfig);
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "[TaktOssHelper] 解析 OSS 配置 JSON 失败: {JsonConfig}", jsonConfig);
            return null;
        }
    }
}
