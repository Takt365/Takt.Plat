// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Services
// 文件名称：TaktCacheService.cs
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：统一缓存服务实现（Memory + 可选 Redis 分布式缓存）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Takt.Domain.Interfaces;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Services;

/// <summary>
/// <see cref="ITaktCacheService"/> 实现
/// 读取 <see cref="TaktCacheOptions"/>，内存层始终启用；Provider 为 Redis 时同步读写分布式层
/// </summary>
public class TaktCacheService : ITaktCacheService
{
    /// <summary>
    /// 分布式缓存 JSON 序列化选项（camelCase）
    /// </summary>
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    /// <summary>
    /// 进程内内存缓存
    /// </summary>
    private readonly IMemoryCache _memoryCache;

    /// <summary>
    /// 分布式缓存（Redis 时注入；Memory 提供者时为 null）
    /// </summary>
    private readonly IDistributedCache? _distributedCache;

    /// <summary>
    /// 缓存配置（提供者、默认过期、滑动过期等）
    /// </summary>
    private readonly TaktCacheOptions _options;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="memoryCache">应用统一 IMemoryCache（与 appsettings Cache:Memory 绑定，见 AddTaktCache）</param>
    /// <param name="options">缓存配置</param>
    /// <param name="distributedCache">分布式缓存（Redis 时注入）</param>
    public TaktCacheService(
        IMemoryCache memoryCache,
        IOptions<TaktCacheOptions> options,
        IDistributedCache? distributedCache = null)
    {
        _memoryCache = memoryCache;
        _options = options.Value;
        _distributedCache = distributedCache;
    }

    /// <summary>
    /// 获取缓存项（先读内存，Redis 提供者时再读分布式层并回填内存）
    /// </summary>
    /// <typeparam name="T">值类型</typeparam>
    /// <param name="key">缓存键</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>缓存值；不存在时返回 null</returns>
    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
    {
        if (_memoryCache.TryGetValue(key, out T? memoryValue) && memoryValue != null)
        {
            return memoryValue;
        }

        if (!ShouldUseDistributedCache() || _distributedCache == null)
        {
            return null;
        }

        var bytes = await _distributedCache.GetAsync(key, cancellationToken);
        if (bytes == null || bytes.Length == 0)
        {
            return null;
        }

        var distributedValue = JsonSerializer.Deserialize<T>(bytes, JsonOptions);
        if (distributedValue != null)
        {
            _memoryCache.Set(key, distributedValue, CreateMemoryEntryOptions(null));
        }

        return distributedValue;
    }

    /// <summary>
    /// 写入缓存项（使用配置中的默认过期策略）
    /// </summary>
    /// <typeparam name="T">值类型</typeparam>
    /// <param name="key">缓存键</param>
    /// <param name="value">缓存值</param>
    /// <param name="absoluteExpiration">绝对过期时间；为空时使用配置默认分钟数</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>任务</returns>
    public async Task SetAsync<T>(
        string key,
        T value,
        TimeSpan? absoluteExpiration = null,
        CancellationToken cancellationToken = default) where T : class
    {
        _memoryCache.Set(key, value, CreateMemoryEntryOptions(absoluteExpiration));

        if (!ShouldUseDistributedCache() || _distributedCache == null)
        {
            return;
        }

        var bytes = JsonSerializer.SerializeToUtf8Bytes(value, JsonOptions);
        await _distributedCache.SetAsync(
            key,
            bytes,
            CreateDistributedEntryOptions(absoluteExpiration),
            cancellationToken);
    }

    /// <summary>
    /// 移除缓存项（内存与分布式层同步移除）
    /// </summary>
    /// <param name="key">缓存键</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>任务</returns>
    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        _memoryCache.Remove(key);

        if (ShouldUseDistributedCache() && _distributedCache != null)
        {
            await _distributedCache.RemoveAsync(key, cancellationToken);
        }
    }

    /// <summary>
    /// 获取或创建缓存项（未命中时执行工厂方法并写入缓存）
    /// </summary>
    /// <typeparam name="T">值类型</typeparam>
    /// <param name="key">缓存键</param>
    /// <param name="factory">未命中时的工厂方法</param>
    /// <param name="absoluteExpiration">绝对过期时间；为空时使用配置默认分钟数</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>缓存值</returns>
    public async Task<T> GetOrCreateAsync<T>(
        string key,
        Func<Task<T>> factory,
        TimeSpan? absoluteExpiration = null,
        CancellationToken cancellationToken = default) where T : class
    {
        var cached = await GetAsync<T>(key, cancellationToken);
        if (cached != null)
        {
            return cached;
        }

        var value = await factory();
        await SetAsync(key, value, absoluteExpiration, cancellationToken);
        return value;
    }

    /// <summary>
    /// 是否使用分布式缓存层（Redis 提供者且已注入 <see cref="IDistributedCache"/>）
    /// </summary>
    /// <returns>为 true 时读写 Redis</returns>
    private bool ShouldUseDistributedCache()
    {
        return _distributedCache != null && _options.IsRedisProvider;
    }

    /// <summary>
    /// 创建内存缓存项选项（绝对或滑动过期由配置决定）
    /// </summary>
    /// <param name="absoluteExpiration">绝对过期时间；为空时使用 <see cref="TaktCacheOptions.DefaultExpirationMinutes"/></param>
    /// <returns>内存缓存项选项</returns>
    private MemoryCacheEntryOptions CreateMemoryEntryOptions(TimeSpan? absoluteExpiration)
    {
        var expiration = absoluteExpiration ?? TimeSpan.FromMinutes(_options.DefaultExpirationMinutes);
        var entry = new MemoryCacheEntryOptions();

        if (_options.EnableSlidingExpiration)
        {
            entry.SlidingExpiration = expiration;
        }
        else
        {
            entry.AbsoluteExpirationRelativeToNow = expiration;
        }

        return entry;
    }

    /// <summary>
    /// 创建分布式缓存项选项（绝对或滑动过期由配置决定）
    /// </summary>
    /// <param name="absoluteExpiration">绝对过期时间；为空时使用 <see cref="TaktCacheOptions.DefaultExpirationMinutes"/></param>
    /// <returns>分布式缓存项选项</returns>
    private DistributedCacheEntryOptions CreateDistributedEntryOptions(TimeSpan? absoluteExpiration)
    {
        var expiration = absoluteExpiration ?? TimeSpan.FromMinutes(_options.DefaultExpirationMinutes);
        var entry = new DistributedCacheEntryOptions();

        if (_options.EnableSlidingExpiration)
        {
            entry.SlidingExpiration = expiration;
        }
        else
        {
            entry.AbsoluteExpirationRelativeToNow = expiration;
        }

        return entry;
    }
}
