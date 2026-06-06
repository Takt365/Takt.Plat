// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Extensions
// 文件名称：TaktCacheCollectionExtensions.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：缓存服务注册扩展方法（按 Provider 自动启用 Memory 或 Redis）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Takt.Domain.Interfaces;
using Takt.Infrastructure.Services;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Extensions;

/// <summary>
/// 缓存服务注册扩展方法
/// </summary>
public static class TaktCacheCollectionExtensions
{
    /// <summary>
    /// 添加缓存服务（Provider=Memory 仅内存；Provider=Redis 注册 StackExchange Redis）
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <param name="configuration">配置</param>
    /// <returns>服务集合</returns>
    public static IServiceCollection AddTaktCache(this IServiceCollection services, IConfiguration configuration)
    {
        var cacheOptions = configuration.RequireOptions<TaktCacheOptions>(TaktCacheOptions.SectionName);

        cacheOptions.Validate(configuration);

        services.Configure<TaktCacheOptions>(configuration.GetSection(TaktCacheOptions.SectionName));

        var memoryOptions = cacheOptions.Memory;

        // 全应用唯一 IMemoryCache：业务 ITaktCacheService 与 OpenIddict 等框架共用；选项来自 Cache:Memory
        services.AddMemoryCache(options =>
        {
            options.SizeLimit = memoryOptions.SizeLimit;
            options.CompactionPercentage = memoryOptions.CompactionPercentage;
            options.ExpirationScanFrequency = TimeSpan.FromSeconds(
                Math.Max(1, memoryOptions.ExpirationScanFrequency));
        });

        if (cacheOptions.IsRedisProvider)
        {
            var redisConnectionString = cacheOptions.ResolveRedisConnectionString(configuration);

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConnectionString;
                options.InstanceName = cacheOptions.Redis.InstanceName.TrimEnd(':') + ":";
            });

            TaktLogger.Information(
                "Redis 缓存已启用 - InstanceName={InstanceName}",
                cacheOptions.Redis.InstanceName);
        }
        else
        {
            TaktLogger.Information("内存缓存已启用（Provider=Memory）");
        }

        services.AddSingleton<ITaktCacheService, TaktCacheService>();

        TaktLogger.Information(
            "缓存已配置 - Provider={Provider}, RedisEnabled={RedisEnabled}, DefaultExpirationMinutes={Minutes}, Sliding={Sliding}",
            cacheOptions.Provider,
            cacheOptions.Redis.Enabled,
            cacheOptions.DefaultExpirationMinutes,
            cacheOptions.EnableSlidingExpiration);

        return services;
    }
}
