// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Services
// 文件名称：TaktLoginSessionService.cs
// 创建时间：2026-05-26
// 创建人：Takt365(Cursor AI)
// 功能描述：登录前租户选项与租户内用户校验（TaktTenant / TaktUser 数据表）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.Configuration;
using SqlSugar;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Entities.Identity;
using Takt.Domain.Interfaces;
using Takt.Infrastructure.Data.Context;
using Takt.Shared.Enums;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Services;

/// <summary>
/// <see cref="ITaktLoginSessionService"/> 实现
/// 登录页租户与用户校验均查询对应租户库实体表，不按配置白名单兜底
/// </summary>
public class TaktLoginSessionService : ITaktLoginSessionService
{
    private readonly IConfiguration _configuration;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="configuration">配置</param>
    public TaktLoginSessionService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// 获取登录页可选租户列表（遍历 ConnectionStrings:Tenant_*，仅返回 TaktTenant 存在且启用的项）
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>下拉选项（DictValue=TenantCode）</returns>
    public async Task<List<TaktSelectOption>> GetLoginTenantOptionsAsync(CancellationToken cancellationToken = default)
    {
        var options = new List<TaktSelectOption>();
        var sortOrder = 0;
        var connectionFailures = 0;
        var configuredCount = 0;

        foreach (var code in GetConfiguredTenantConnectionCodes())
        {
            configuredCount++;
            try
            {
                using var seedContext = CreateSeedContext(code);
                var tenant = await seedContext.Query<TaktTenant>()
                    .Where(t => t.TenantCode == code && t.TenantStatus == 1)
                    .FirstAsync(cancellationToken);
                if (tenant == null)
                {
                    continue;
                }

                var label = string.IsNullOrWhiteSpace(tenant.TenantName)
                    ? code
                    : tenant.TenantName.Trim();

                options.Add(new TaktSelectOption
                {
                    DictValue = code,
                    DictLabel = label,
                    SortOrder = sortOrder++,
                });
            }
            catch (Exception ex) when (IsSqlSugarInfrastructureFailure(ex))
            {
                connectionFailures++;
                TaktLogger.Warning(ex, "读取租户 {TenantCode} 登录选项失败（库/表未就绪）", code);
            }
            catch (Exception ex)
            {
                TaktLogger.Warning(ex, "读取租户 {TenantCode} 实体失败，跳过登录选项", code);
            }
        }

        if (configuredCount > 0 && connectionFailures == configuredCount)
        {
            ThrowTenantDatabaseFailure("全部配置租户", null);
        }

        if (options.Count == 0)
        {
            throw new InvalidOperationException("未在 TaktTenant 表中找到任何启用的租户");
        }

        return options;
    }

    /// <summary>
    /// 校验用户名在指定租户库 TaktUser 中是否存在且启用
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="username">用户名</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>存在且启用为 true</returns>
    public async Task<bool> HasUserLoginAccessInTenantAsync(
        string tenantCode,
        string username,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(tenantCode) || string.IsNullOrWhiteSpace(username))
        {
            return false;
        }

        var trimmedTenant = tenantCode.Trim();
        var trimmedUsername = username.Trim();

        try
        {
            using var seedContext = CreateSeedContext(trimmedTenant);
            return await seedContext.Query<TaktUser>()
                .AnyAsync(
                    u =>
                        u.TenantCode == trimmedTenant
                        && u.Username == trimmedUsername
                        && u.UserStatus == TaktCommonStatus.Enabled,
                    cancellationToken);
        }
        catch (Exception ex) when (IsSqlSugarInfrastructureFailure(ex))
        {
            ThrowTenantDatabaseFailure(trimmedTenant, ex);
            throw;
        }
    }

    /// <summary>
    /// 校验登录页输入的租户编码在 TaktTenant 中是否存在且启用
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>存在且启用为 true；库/表缺失时抛出 <see cref="TaktBusinessException"/></returns>
    public async Task<bool> ValidateLoginTenantCodeAsync(
        string tenantCode,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(tenantCode))
        {
            return false;
        }

        var trimmedTenant = tenantCode.Trim();

        try
        {
            using var seedContext = CreateSeedContext(trimmedTenant);
            return await seedContext.Query<TaktTenant>()
                .AnyAsync(
                    t => t.TenantCode == trimmedTenant && t.TenantStatus == 1,
                    cancellationToken);
        }
        catch (Exception ex) when (IsSqlSugarInfrastructureFailure(ex))
        {
            ThrowTenantDatabaseFailure(trimmedTenant, ex);
            throw;
        }
    }

    /// <summary>
    /// 获取登录页语言切换选项（匿名；须指定租户，仅查询该租户库）
    /// </summary>
    /// <param name="tenantCode">租户编码（必填）</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetLoginCultureOptionsAsync(
        string? tenantCode = null,
        CancellationToken cancellationToken = default)
    {
        var trimmedTenant = tenantCode?.Trim();
        if (string.IsNullOrWhiteSpace(trimmedTenant))
        {
            throw new ArgumentException("租户编码不能为空", nameof(tenantCode));
        }

        if (!await ValidateLoginTenantCodeAsync(trimmedTenant, cancellationToken))
        {
            return new List<TaktSelectOption>();
        }

        return await LoadCultureOptionsForTenantAsync(trimmedTenant, cancellationToken);
    }

    /// <summary>
    /// 读取指定租户库中启用的区域文化选项
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>语言下拉选项</returns>
    private async Task<List<TaktSelectOption>> LoadCultureOptionsForTenantAsync(
        string tenantCode,
        CancellationToken cancellationToken)
    {
        try
        {
            using var seedContext = CreateSeedContext(tenantCode);
            var list = await seedContext.Query<TaktCulture>()
                .Where(c =>
                    c.TenantCode == tenantCode
                    && c.LanguageStatus == TaktCommonStatus.Enabled
                    && c.IsDeleted == 0)
                .OrderBy(c => c.SortOrder)
                .ToListAsync(cancellationToken);

            return list.Select(e => new TaktSelectOption
            {
                DictValue = e.CultureCode,
                DictLabel = e.LanguageName,
                ExtValue = e.Icon,
                ExtLabel = ((int)e.IsDefault).ToString(),
                SortOrder = e.SortOrder,
            }).ToList();
        }
        catch (Exception ex) when (IsSqlSugarInfrastructureFailure(ex))
        {
            ThrowTenantDatabaseFailure(tenantCode, ex);
            throw;
        }
    }

    /// <summary>
    /// 创建指定租户的种子上下文
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <returns>种子上下文</returns>
    private TaktSeedContext CreateSeedContext(string tenantCode)
    {
        return new TaktSeedContext(_configuration, tenantCode);
    }

    /// <summary>
    /// 从 ConnectionStrings 读取已配置租户连接编码（Tenant_{code}）
    /// </summary>
    /// <returns>租户编码序列</returns>
    private IEnumerable<string> GetConfiguredTenantConnectionCodes()
    {
        return _configuration.GetTenantCodes();
    }

    /// <summary>
    /// 解析租户连接串中的数据库名
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <returns>数据库名</returns>
    private string ResolveTenantDatabaseName(string tenantCode)
    {
        var trimmed = tenantCode.Trim();
        var connectionString = _configuration.GetConnectionString($"Tenant_{trimmed}");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return $"Takt_{trimmed}_Dev";
        }

        const string key = "Database=";
        var idx = connectionString.IndexOf(key, StringComparison.OrdinalIgnoreCase);
        if (idx < 0)
        {
            return $"Takt_{trimmed}_Dev";
        }

        var start = idx + key.Length;
        var end = connectionString.IndexOf(';', start);
        var dbName = end > start ? connectionString[start..end] : connectionString[start..];
        return string.IsNullOrWhiteSpace(dbName) ? $"Takt_{trimmed}_Dev" : dbName.Trim();
    }

    /// <summary>
    /// 判断异常是否为 SqlSugar 连库/缺表类基础设施错误
    /// </summary>
    /// <param name="ex">捕获的异常</param>
    /// <returns>基础设施错误为 true</returns>
    private static bool IsSqlSugarInfrastructureFailure(Exception ex)
    {
        for (var current = ex; current != null; current = current.InnerException)
        {
            if (current is SqlSugarException)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 租户库基础设施失败类型
    /// </summary>
    private enum TenantDatabaseFailureKind
    {
        DatabaseMissing,
        TableMissing,
        LoginFailed,
        ConnectionFailed,
    }

    /// <summary>
    /// 根据 SqlSugar 异常文本分类库/表/登录失败
    /// </summary>
    /// <param name="ex">SqlSugar 异常</param>
    /// <returns>失败类型</returns>
    private static TenantDatabaseFailureKind ClassifyTenantDatabaseFailure(Exception ex)
    {
        var text = CollectExceptionText(ex);
        if (text.Contains("无法打开登录所请求的数据库", StringComparison.Ordinal)
            || text.Contains("Cannot open database", StringComparison.OrdinalIgnoreCase))
        {
            return TenantDatabaseFailureKind.DatabaseMissing;
        }

        if (text.Contains("Invalid object name", StringComparison.OrdinalIgnoreCase)
            || text.Contains("对象名", StringComparison.Ordinal)
            || text.Contains("invalid object", StringComparison.OrdinalIgnoreCase))
        {
            return TenantDatabaseFailureKind.TableMissing;
        }

        if (text.Contains("登录失败", StringComparison.Ordinal)
            || text.Contains("Login failed", StringComparison.OrdinalIgnoreCase))
        {
            return TenantDatabaseFailureKind.LoginFailed;
        }

        return TenantDatabaseFailureKind.ConnectionFailed;
    }

    /// <summary>
    /// 收集异常链全部 Message 文本
    /// </summary>
    /// <param name="ex">异常</param>
    /// <returns>合并文本</returns>
    private static string CollectExceptionText(Exception ex)
    {
        var parts = new List<string>();
        for (var current = ex; current != null; current = current.InnerException)
        {
            if (!string.IsNullOrWhiteSpace(current.Message))
            {
                parts.Add(current.Message);
            }
        }

        return string.Join(' ', parts);
    }

    /// <summary>
    /// 抛出租户库/表未就绪业务异常（供 API 与前端直接展示）
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="innerException">内部异常</param>
    private void ThrowTenantDatabaseFailure(string tenantCode, Exception? innerException)
    {
        var trimmed = tenantCode.Trim();
        var databaseName = ResolveTenantDatabaseName(trimmed);
        var kind = innerException == null
            ? TenantDatabaseFailureKind.ConnectionFailed
            : ClassifyTenantDatabaseFailure(innerException);

        string message;
        string errorCode;
        switch (kind)
        {
            case TenantDatabaseFailureKind.DatabaseMissing:
                errorCode = "error.tenant.database.missing";
                message =
                    $"租户 {trimmed} 的业务数据库不存在（库名：{databaseName}）。请在 appsettings 将 Init.InitDb 设为 true 后重启后端建库，或手动创建该数据库。";
                break;
            case TenantDatabaseFailureKind.TableMissing:
                errorCode = "error.tenant.database.tables.missing";
                message =
                    $"租户 {trimmed} 的业务数据库 {databaseName} 已连接，但缺少业务数据表。请将 Init.InitDb 设为 true 后重启建表，并视需要开启 Init.SeedData 写入种子。";
                break;
            case TenantDatabaseFailureKind.LoginFailed:
                errorCode = "error.tenant.database.login.failed";
                message =
                    $"租户 {trimmed} 无法登录 SQL Server（库名：{databaseName}）。请检查 ConnectionStrings:Tenant_{trimmed} 中的服务器地址与 sa 密码。";
                break;
            default:
                errorCode = "error.tenant.database.connection";
                message =
                    $"租户 {trimmed} 的业务数据库无法连接（库名：{databaseName}）。请检查 ConnectionStrings:Tenant_{trimmed}，或执行 InitDb 建库。";
                break;
        }

        if (innerException == null)
        {
            throw new TaktBusinessException(message, errorCode);
        }

        throw new TaktBusinessException(message, errorCode, innerException);
    }
}
