// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Shared.Options
// 文件名称：TaktConfigurationExtensions.cs
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：配置节绑定扩展（Database / Init / TenantContext 统一入口）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.Configuration;

namespace Takt.Shared.Options;

/// <summary>
/// 配置节绑定扩展（<see cref="TaktDatabaseOptions"/>、<see cref="TaktInitOptions"/> 等唯一绑定入口）
/// </summary>
public static class TaktConfigurationExtensions
{
    /// <summary>
    /// 绑定配置节，缺失或绑定失败时抛出异常
    /// </summary>
    /// <typeparam name="T">配置类型</typeparam>
    /// <param name="configuration">应用配置</param>
    /// <param name="sectionName">配置节名称</param>
    /// <returns>绑定结果</returns>
    public static T RequireOptions<T>(this IConfiguration configuration, string sectionName)
        where T : class
    {
        ArgumentNullException.ThrowIfNull(configuration);
        var options = configuration.GetSection(sectionName).Get<T>();
        if (options == null)
        {
            throw new InvalidOperationException($"配置节 {sectionName} 缺失或无法绑定");
        }
        return options;
    }

    /// <summary>
    /// 绑定并校验 <c>Database</c> 节（租户/公司/工厂范围与种子源）
    /// </summary>
    /// <param name="configuration">应用配置</param>
    /// <returns>已规范化并校验的数据库配置</returns>
    public static TaktDatabaseOptions RequireDatabase(this IConfiguration configuration)
    {
        var options = configuration.RequireOptions<TaktDatabaseOptions>(TaktDatabaseOptions.SectionName);
        options.NormalizeAndValidate();
        return options;
    }

    /// <summary>
    /// 绑定 <c>Init</c> 节（建表/种子/克隆开关，与 <see cref="TaktDatabaseOptions"/> 数据范围分离）
    /// </summary>
    /// <param name="configuration">应用配置</param>
    /// <returns>初始化开关配置</returns>
    public static TaktInitOptions RequireInit(this IConfiguration configuration) =>
        configuration.RequireOptions<TaktInitOptions>(TaktInitOptions.SectionName);

    /// <summary>
    /// 绑定并校验 <c>TenantContext</c> 节（请求头名称）
    /// </summary>
    /// <param name="configuration">应用配置</param>
    /// <returns>租户上下文配置</returns>
    public static TaktTenantContextOptions RequireTenantContext(this IConfiguration configuration)
    {
        var options = configuration.RequireOptions<TaktTenantContextOptions>(TaktTenantContextOptions.SectionName);
        options.Validate();
        return options;
    }

    /// <summary>
    /// 租户编码列表（优先 <c>Database:TenantCodes</c>；否则回退 <c>ConnectionStrings:Tenant_*</c>）
    /// </summary>
    /// <param name="configuration">应用配置</param>
    /// <returns>租户编码只读列表</returns>
    public static IReadOnlyList<string> GetTenantCodes(this IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        var raw = configuration.GetSection($"{TaktDatabaseOptions.SectionName}:TenantCodes").Get<List<string>>();
        if (raw != null && raw.Exists(code => !string.IsNullOrWhiteSpace(code)))
        {
            return configuration.RequireDatabase().TenantCodes;
        }
        var fallback = new List<string>();
        foreach (var child in configuration.GetSection("ConnectionStrings").GetChildren())
        {
            if (!child.Key.StartsWith("Tenant_", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }
            var tenantCode = child.Key["Tenant_".Length..].Trim();
            if (!string.IsNullOrEmpty(tenantCode) && !string.IsNullOrWhiteSpace(child.Value))
            {
                fallback.Add(tenantCode);
            }
        }
        if (fallback.Count == 0)
        {
            throw new InvalidOperationException("未找到任何租户配置（Database:TenantCodes 或 ConnectionStrings:Tenant_*）");
        }
        return fallback;
    }

    /// <summary>
    /// 租户编码与连接字符串（顺序与 <see cref="GetTenantCodes"/> 一致）
    /// </summary>
    /// <param name="configuration">应用配置</param>
    /// <returns>租户编码与连接字符串列表</returns>
    public static List<(string TenantCode, string ConnectionString)> GetTenantConnections(this IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        var result = new List<(string TenantCode, string ConnectionString)>();
        foreach (var tenantCode in configuration.GetTenantCodes())
        {
            var connectionString = configuration.GetConnectionString($"Tenant_{tenantCode}");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    $"未找到租户 {tenantCode} 的连接字符串配置 (ConnectionStrings:Tenant_{tenantCode})");
            }
            result.Add((tenantCode, connectionString));
        }
        return result;
    }

    /// <summary>
    /// 绑定并校验 <c>Ftp:{provider}</c> 节（提供商标识与 <c>sys_ftp_provider</c> 字典值一致）
    /// </summary>
    /// <param name="configuration">应用配置</param>
    /// <param name="provider">FTP 提供商标识，如 <c>teac_cn</c></param>
    /// <returns>FTP 提供商配置</returns>
    public static TaktFtpOptions RequireFtpProvider(this IConfiguration configuration, string provider)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentException.ThrowIfNullOrWhiteSpace(provider);

        var options = configuration.GetSection($"{TaktFtpOptions.SectionName}:{provider}").Get<TaktFtpOptions>();
        if (options == null)
        {
            throw new InvalidOperationException($"配置节 {TaktFtpOptions.SectionName}:{provider} 缺失或无法绑定");
        }

        options.Validate(provider);
        return options;
    }

    /// <summary>
    /// 绑定并校验 <c>Oss:{provider}</c> 节（提供商标识与 <c>sys_oss_provider</c> 字典值一致）
    /// </summary>
    /// <param name="configuration">应用配置</param>
    /// <param name="provider">OSS 提供商标识，如 <c>aliyun</c></param>
    /// <returns>OSS 提供商配置</returns>
    public static TaktOssOptions RequireOssProvider(this IConfiguration configuration, string provider)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentException.ThrowIfNullOrWhiteSpace(provider);

        var options = configuration.GetSection($"{TaktOssOptions.SectionName}:{provider}").Get<TaktOssOptions>();
        if (options == null)
        {
            throw new InvalidOperationException($"配置节 {TaktOssOptions.SectionName}:{provider} 缺失或无法绑定");
        }

        options.Validate(provider);
        return options;
    }

    /// <summary>
    /// 绑定并校验 <c>Quartz</c> 节（定时任务调度开关与调度器标识）
    /// </summary>
    /// <param name="configuration">应用配置</param>
    /// <returns>Quartz 配置</returns>
    public static TaktQuartzOptions RequireQuartz(this IConfiguration configuration)
    {
        var options = configuration.RequireOptions<TaktQuartzOptions>(TaktQuartzOptions.SectionName);
        options.Validate();
        return options;
    }
}

