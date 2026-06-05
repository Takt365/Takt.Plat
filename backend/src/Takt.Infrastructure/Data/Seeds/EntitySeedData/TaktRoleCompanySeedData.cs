// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds
// 文件名称：TaktRoleCompanySeedData.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt角色-公司关联种子数据，定义角色可访问的公司范围
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Entities.Accounting.Financial;
using Takt.Domain.Entities.Identity;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Enums;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Data.Seeds.EntitySeedData;

/// <summary>
/// 角色-公司关联种子数据初始化
/// 幂等性操作：存在则跳过，不存在则创建
/// </summary>
public class TaktRoleCompanySeedData : ITaktSeedDataCoordinator
{
    public int Order => 61;

    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(IServiceProvider serviceProvider, string? tenantCode = null)
    {
        TaktLogger.Information("开始初始化角色-公司关联种子数据...");

        if (string.IsNullOrEmpty(tenantCode))
        {
            TaktLogger.Warning("租户编码为空，跳过角色-公司关联种子数据初始化");
            return (0, 0);
        }

        var repository = serviceProvider.GetRequiredService<ITaktCompanySeedRepository<TaktRoleCompany>>();
        var roleRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktRole>>();
        var companyRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktCompany>>();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var database = configuration.RequireDatabase();
        var configuredCompanyCodes = database.CompanyCodes;
        var companies = await companyRepository.GetListAsync(
            c => c.TenantCode == tenantCode && c.CompanyStatus == TaktCommonStatus.Enabled);
        var orderedCompanies = TaktDatabaseOptions.OrderByConfiguredCodes(
            configuredCompanyCodes,
            companies,
            c => c.CompanyCode);

        if (orderedCompanies.Count == 0)
        {
            TaktLogger.Warning("租户 {TenantCode} 未找到 Database:CompanyCodes 对应的公司，跳过角色-公司关联种子", tenantCode);
            return (0, 0);
        }

        int insertCount = 0;

        TaktLogger.Information("正在为租户 {TenantCode} 初始化角色-公司关联数据...", tenantCode);

        foreach (var company in orderedCompanies)
        {
            insertCount += await CreateRoleCompanyAsync(
                repository,
                roleRepository,
                companyRepository,
                tenantCode,
                "ROLE_SUPER_ADMIN",
                company.CompanyCode);
        }

        var seedCompanyCode = database.GetSeedCompanyCode();
        var companyMap = orderedCompanies.ToDictionary(c => c.CompanyCode, StringComparer.Ordinal);
        var demoScopeCompanyCode = companyMap.ContainsKey(seedCompanyCode) ? seedCompanyCode : null;
        if (demoScopeCompanyCode != null)
        {
            insertCount += await CreateRoleCompanyAsync(
                repository,
                roleRepository,
                companyRepository,
                tenantCode,
                "ROLE_DEPT_ADMIN",
                demoScopeCompanyCode);
            insertCount += await CreateRoleCompanyAsync(
                repository,
                roleRepository,
                companyRepository,
                tenantCode,
                "ROLE_EMPLOYEE",
                demoScopeCompanyCode);
        }

        TaktLogger.Information("角色-公司关联种子数据初始化完成: 插入 {InsertCount} 条", insertCount);

        return (insertCount, 0);
    }

    private static async Task<int> CreateRoleCompanyAsync(
        ITaktCompanySeedRepository<TaktRoleCompany> repository,
        ITaktTenantSeedRepository<TaktRole> roleRepository,
        ITaktTenantSeedRepository<TaktCompany> companyRepository,
        string tenantCode,
        string roleCode,
        string companyCode)
    {
        var role = await roleRepository.FirstAsync(r => r.TenantCode == tenantCode && r.RoleCode == roleCode);
        if (role == null) return 0;

        var company = await companyRepository.FirstAsync(c =>
            c.TenantCode == tenantCode && c.CompanyCode == companyCode);
        if (company == null) return 0;

        var exists = await repository.FirstAsync(rc =>
            rc.TenantCode == tenantCode
            && rc.RoleId == role.Id
            && rc.CompanyCode == companyCode);
        if (exists != null) return 0;

        await repository.CreateAsync(new TaktRoleCompany
        {
            TenantCode = tenantCode,
            RoleId = role.Id,
            CompanyCode = companyCode,
        });
        return 1;
    }
}
