// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktFileStorageConfigHelper.cs
// 创建时间：2026-06-12
// 创建人：Takt365(Cursor AI)
// 功能描述：TaktFile.StorageConfig JSON 解析与 OSS/FTP 提供商标识解析（纯函数）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Newtonsoft.Json;
using Takt.Shared.Models;

namespace Takt.Shared.Helpers;

/// <summary>
/// 文件存储配置 JSON 解析（与 frontend takt-file-storage-config.ts 对齐）
/// </summary>
public static class TaktFileStorageConfigHelper
{
    /// <summary>
    /// 本地存储（字典 sys_storage_type=0）
    /// </summary>
    public const int StorageTypeLocal = 0;

    /// <summary>
    /// OSS 对象存储（字典 sys_storage_type=1）
    /// </summary>
    public const int StorageTypeOss = 1;

    /// <summary>
    /// FTP 存储（字典 sys_storage_type=2）
    /// </summary>
    public const int StorageTypeFtp = 2;

    /// <summary>
    /// 默认 OSS 提供商标识（字典 sys_oss_provider_type=aliyun）
    /// </summary>
    public const string DefaultOssProvider = "aliyun";

    /// <summary>
    /// 默认 FTP 提供商标识（字典 sys_ftp_provider_type=teac_cn）
    /// </summary>
    public const string DefaultFtpProvider = "teac_cn";

    /// <summary>
    /// 从 StorageConfig JSON 解析存储配置载荷
    /// </summary>
    /// <param name="jsonConfig">StorageConfig 原始 JSON</param>
    /// <returns>解析结果；非法 JSON 返回空载荷</returns>
    public static TaktFileStorageConfigPayload Parse(string? jsonConfig)
    {
        if (string.IsNullOrWhiteSpace(jsonConfig))
        {
            return new TaktFileStorageConfigPayload();
        }

        try
        {
            var parsed = JsonConvert.DeserializeObject<TaktFileStorageConfigPayload>(jsonConfig);
            return parsed ?? new TaktFileStorageConfigPayload();
        }
        catch (JsonException ex)
        {
            TaktLogger.Warning(ex, "[TaktFileStorageConfigHelper] StorageConfig JSON 解析失败");
            return new TaktFileStorageConfigPayload();
        }
    }

    /// <summary>
    /// 解析 OSS 提供商标识
    /// </summary>
    /// <param name="jsonConfig">StorageConfig JSON</param>
    /// <returns>提供商标识（小写）</returns>
    public static string ResolveOssProvider(string? jsonConfig)
    {
        var payload = Parse(jsonConfig);
        var provider = string.IsNullOrWhiteSpace(payload.OssProvider)
            ? DefaultOssProvider
            : payload.OssProvider.Trim();
        return provider.ToLowerInvariant();
    }

    /// <summary>
    /// 解析 FTP 提供商标识
    /// </summary>
    /// <param name="jsonConfig">StorageConfig JSON</param>
    /// <returns>提供商标识（小写）</returns>
    public static string ResolveFtpProvider(string? jsonConfig)
    {
        var payload = Parse(jsonConfig);
        var provider = string.IsNullOrWhiteSpace(payload.FtpProvider)
            ? DefaultFtpProvider
            : payload.FtpProvider.Trim();
        return provider.ToLowerInvariant();
    }

    /// <summary>
    /// 规范化远程对象/文件键（统一斜杠、去首尾 /、禁止 ..）
    /// </summary>
    /// <param name="relativePath">相对存储路径</param>
    /// <returns>安全远程键</returns>
    /// <exception cref="ArgumentException">路径非法</exception>
    public static string NormalizeRemoteObjectKey(string relativePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(relativePath);
        var normalized = relativePath.Replace('\\', '/').TrimStart('/');
        if (normalized.Contains("..", StringComparison.Ordinal))
        {
            throw new ArgumentException("远程存储路径非法");
        }

        return normalized;
    }
}
