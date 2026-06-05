// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Extensions
// 文件名称：TaktLoggingCollectionExtensions.cs
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：Serilog 统一配置扩展（格式化模板、采集配置、上报初始化）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.WebApi.Extensions;

/// <summary>
/// 统一日志 DI / Serilog 扩展
/// </summary>
public static class TaktLoggingCollectionExtensions
{
    /// <summary>
    /// 创建全局 Serilog Logger（应用启动前调用）
    /// </summary>
    /// <param name="configuration">配置</param>
    /// <returns>Serilog Logger</returns>
    public static Serilog.ILogger CreateTaktGlobalLogger(IConfiguration configuration)
    {
        var loggingOptions = BindLoggingOptions(configuration);
        TaktLogger.Configure(loggingOptions);

        return new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithThreadId()
            .Enrich.WithProperty("AppName", loggingOptions.AppName)
            .Enrich.WithProperty("AppVersion", loggingOptions.AppVersion)
            .Enrich.WithProperty("Environment", loggingOptions.Environment)
            .CreateLogger();
    }

    /// <summary>
    /// 注册 Takt 统一日志配置
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <param name="configuration">配置</param>
    /// <param name="environmentName">环境名（仅用于注册，不覆盖配置项）</param>
    /// <returns>服务集合</returns>
    public static IServiceCollection AddTaktLogging(
        this IServiceCollection services,
        IConfiguration configuration,
        string environmentName)
    {
        _ = environmentName;
        var loggingOptions = BindLoggingOptions(configuration);
        services.AddSingleton(loggingOptions);
        services.Configure<TaktLoggingOptions>(configuration.GetSection(TaktLoggingOptions.SectionName));
        TaktLogger.Configure(loggingOptions);
        return services;
    }

    /// <summary>
    /// 启用 Serilog 作为 Host 日志提供程序
    /// </summary>
    /// <param name="hostBuilder">Host 构建器</param>
    /// <returns>Host 构建器</returns>
    public static IHostBuilder UseTaktSerilog(this IHostBuilder hostBuilder)
    {
        return hostBuilder.UseSerilog();
    }

    private static TaktLoggingOptions BindLoggingOptions(IConfiguration configuration)
    {
        var options = configuration.RequireOptions<TaktLoggingOptions>(TaktLoggingOptions.SectionName);
        options.Validate();
        return options;
    }
}
