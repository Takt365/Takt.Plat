// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Services
// 文件名称：TaktLoginLogTenantWriter.cs
// 创建时间：2026-06-03
// 创建人：Takt365(Cursor AI)
// 功能描述：按指定租户库写入登录日志（认证流程专用）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Takt.Domain.Entities.Accounting.Financial;
using Takt.Domain.Entities.Identity;
using Takt.Domain.Entities.Statistics.Logging;
using Takt.Domain.Interfaces;
using Takt.Infrastructure.Data.Context;
using Takt.Shared.Enums;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Services;

/// <summary>
/// ITaktLoginLogTenantWriter 实现
/// </summary>
public class TaktLoginLogTenantWriter : ITaktLoginLogTenantWriter
{
    private readonly IConfiguration _configuration;
    private readonly PrimaryKeyTypeOptions _primaryKeyTypeOptions;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="configuration">配置</param>
    /// <param name="primaryKeyTypeOptions">主键类型配置</param>
    public TaktLoginLogTenantWriter(
        IConfiguration configuration,
        IOptions<PrimaryKeyTypeOptions> primaryKeyTypeOptions)
    {
        _configuration = configuration;
        _primaryKeyTypeOptions = primaryKeyTypeOptions.Value;
    }

    /// <summary>
    /// 在指定租户业务库中插入登录日志
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="entity">登录日志实体</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>新记录主键 ID</returns>
    public async Task<long> CreateInTenantAsync(
        string tenantCode,
        TaktLoginLog entity,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(tenantCode))
        {
            throw new ArgumentException("租户编码不能为空", nameof(tenantCode));
        }

        var trimmedTenant = tenantCode.Trim();
        if (string.IsNullOrWhiteSpace(entity.CompanyCode))
        {
            throw new ArgumentException("公司编码不能为空", nameof(entity));
        }

        if (!string.IsNullOrWhiteSpace(entity.TenantCode)
            && !string.Equals(entity.TenantCode.Trim(), trimmedTenant, StringComparison.Ordinal))
        {
            throw new InvalidOperationException(
                $"租户库硬隔离：ConnectionStrings:Tenant_{trimmedTenant} 库内 TenantCode 必须恒为 {trimmedTenant}，"
                + $"不允许写入 TenantCode={entity.TenantCode.Trim()}。");
        }

        using var seedContext = new TaktSeedContext(_configuration, trimmedTenant);
        entity.TenantCode = trimmedTenant;
        entity.CompanyCode = entity.CompanyCode.Trim();
        var now = DateTime.Now;
        entity.CreatedAt = now;
        entity.UpdatedAt = now;
        entity.CreatedBy = 0;
        entity.UpdatedBy = 0;
        entity.IsDeleted = 0;

        if (_primaryKeyTypeOptions.Snowflake.Enabled)
        {
            return await seedContext.Db.Insertable(entity)
                .ExecuteReturnSnowflakeIdAsync(cancellationToken);
        }

        await seedContext.Db.Insertable(entity).ExecuteCommandAsync(cancellationToken);
        return entity.Id;
    }

    /// <summary>
    /// 在指定租户库中解析用户默认登录公司
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="userId">用户 ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>公司编码；无默认关联时返回 null</returns>
    public async Task<string?> ResolveUserDefaultCompanyCodeAsync(
        string tenantCode,
        long userId,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(tenantCode) || userId <= 0)
        {
            return null;
        }

        var trimmedTenant = tenantCode.Trim();
        try
        {
            using var seedContext = new TaktSeedContext(_configuration, trimmedTenant);
            var defaultLinkCodes = (await seedContext.Query<TaktUserCompany>()
                .Where(uc =>
                    uc.TenantCode == trimmedTenant
                    && uc.UserId == userId
                    && uc.IsDefault == 1)
                .Select(uc => uc.CompanyCode)
                .ToListAsync(cancellationToken))
                .Where(code => !string.IsNullOrWhiteSpace(code))
                .Select(code => code.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (defaultLinkCodes.Count == 0)
            {
                return null;
            }

            var companies = await seedContext.Query<TaktCompany>()
                .Where(c =>
                    c.TenantCode == trimmedTenant
                    && defaultLinkCodes.Contains(c.CompanyCode)
                    && c.CompanyStatus == 1)
                .ToListAsync(cancellationToken);

            var company = companies
                .OrderBy(c => c.SortOrder)
                .ThenBy(c => c.CompanyCode, StringComparer.Ordinal)
                .FirstOrDefault();
            return company?.CompanyCode;
        }
        catch (Exception ex)
        {
            TaktLogger.Warning(
                ex,
                "[LoginLogTenantWriter] 解析租户 {TenantCode} 用户 {UserId} 默认公司失败",
                trimmedTenant,
                userId);
            return null;
        }
    }

    /// <summary>
    /// 是否已配置指定租户业务库连接
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <returns>已配置为 true</returns>
    public bool IsTenantDatabaseConfigured(string tenantCode)
    {
        if (string.IsNullOrWhiteSpace(tenantCode))
        {
            return false;
        }

        var trimmedTenant = tenantCode.Trim();
        return !string.IsNullOrWhiteSpace(_configuration.GetConnectionString($"Tenant_{trimmedTenant}"));
    }
}
