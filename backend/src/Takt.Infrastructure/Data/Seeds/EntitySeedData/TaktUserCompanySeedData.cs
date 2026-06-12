// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds
// 文件名称：TaktUserCompanySeedData.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt用户-公司关联种子数据，支持用户跨公司访问及默认登录公司
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Takt.Infrastructure.Data.Seeds;
using Takt.Domain.Entities.Identity;
using Takt.Domain.Entities.Accounting.Financial;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Enums;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Data.Seeds.EntitySeedData;

/// <summary>
/// 用户-公司关联种子数据初始化
/// 幂等性操作：存在则更新 IsDefault，不存在则创建
/// </summary>
public class TaktUserCompanySeedData : ITaktSeedDataCoordinator
{
    public int Order => 65;

    /// <summary>
    /// 初始化用户-公司关联种子数据
    /// </summary>
    /// <param name="serviceProvider">服务提供者</param>
    /// <param name="tenantCode">租户编码</param>
    /// <returns>插入数与更新数</returns>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(IServiceProvider serviceProvider, string? tenantCode = null)
    {
        TaktLogger.Information("开始初始化用户-公司关联种子数据...");

        if (string.IsNullOrEmpty(tenantCode))
        {
            TaktLogger.Warning("租户编码为空，跳过用户-公司关联种子数据初始化");
            return (0, 0);
        }

        var userRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktUser>>();
        var userCompanyRepository = serviceProvider.GetRequiredService<ITaktCompanySeedRepository<TaktUserCompany>>();
        var companyRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktCompany>>();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var database = configuration.RequireDatabase();
        var configuredCompanyCodes = database.CompanyCodes;
        int insertCount = 0;
        int updateCount = 0;

        TaktLogger.Information("正在为租户 {TenantCode} 初始化用户-公司关联数据...", tenantCode);

        var companies = await companyRepository.GetListAsync(
            c => c.TenantCode == tenantCode && c.CompanyStatus == 1);

        if (companies == null || companies.Count == 0)
        {
            TaktLogger.Warning("租户 {TenantCode} 未找到启用的公司，跳过用户-公司关联种子数据初始化", tenantCode);
            return (0, 0);
        }

        var companyMap = companies.ToDictionary(c => c.CompanyCode, StringComparer.Ordinal);
        var orderedCompanies = TaktDatabaseOptions.OrderByConfiguredCodes(
            configuredCompanyCodes,
            companies,
            c => c.CompanyCode);

        if (orderedCompanies.Count == 0)
        {
            TaktLogger.Warning(
                "租户 {TenantCode} 未找到 Database:CompanyCodes 对应的公司，跳过用户-公司关联种子数据初始化",
                tenantCode);
            return (0, 0);
        }

        var primaryCompanyCode = database.GetSeedCompanyCode();
        if (!companyMap.TryGetValue(primaryCompanyCode, out var primaryCompany))
        {
            primaryCompany = orderedCompanies[0];
        }

        var users = await userRepository.GetListAsync(
            u => u.TenantCode == tenantCode && u.UserStatus == 1);

        foreach (var user in users)
        {
            if (user.Username == "admin")
            {
                foreach (var company in orderedCompanies)
                {
                    var isDefault = string.Equals(company.CompanyCode, primaryCompany.CompanyCode, StringComparison.Ordinal);
                    var (inserted, updated) = await CreateOrUpdateUserCompanyAsync(
                        userRepository,
                        userCompanyRepository,
                        tenantCode,
                        user.Username,
                        company.CompanyCode,
                        isDefault);
                    insertCount += inserted;
                    updateCount += updated;
                }
            }
            else
            {
                var (inserted, updated) = await CreateOrUpdateUserCompanyAsync(
                    userRepository,
                    userCompanyRepository,
                    tenantCode,
                    user.Username,
                    primaryCompany.CompanyCode,
                    true);
                insertCount += inserted;
                updateCount += updated;
            }
        }

        TaktLogger.Information(
            "用户-公司关联种子数据初始化完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条",
            insertCount,
            updateCount);

        return (insertCount, updateCount);
    }

    /// <summary>
    /// 创建或更新用户-公司关联（幂等）
    /// </summary>
    private static async Task<(int Inserted, int Updated)> CreateOrUpdateUserCompanyAsync(
        ITaktTenantSeedRepository<TaktUser> userRepository,
        ITaktCompanySeedRepository<TaktUserCompany> userCompanyRepository,
        string tenantCode,
        string username,
        string companyCode,
        bool isDefault)
    {
        var user = await userRepository.FirstAsync(u => u.TenantCode == tenantCode && u.Username == username);
        if (user == null)
        {
            TaktLogger.Warning("用户 {TenantCode}/{Username} 不存在，跳过公司关联", tenantCode, username);
            return (0, 0);
        }

        var isDefaultValue = isDefault ? 1 : 0;
        var link = await userCompanyRepository.FirstAsync(uc =>
            uc.TenantCode == tenantCode
            && uc.UserId == user.Id
            && uc.CompanyCode == companyCode);

        if (link == null)
        {
            await userCompanyRepository.CreateAsync(new TaktUserCompany
            {
                TenantCode = tenantCode,
                UserId = user.Id,
                CompanyCode = companyCode,
                IsDefault = isDefaultValue,
            });
            return (1, 0);
        }

        if (link.IsDefault == isDefaultValue)
        {
            return (0, 0);
        }

        link.IsDefault = isDefaultValue;
        await userCompanyRepository.UpdateAsync(link);
        return (0, 1);
    }
}
