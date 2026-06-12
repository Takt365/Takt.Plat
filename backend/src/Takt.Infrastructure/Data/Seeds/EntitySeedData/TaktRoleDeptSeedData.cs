// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds
// 文件名称：TaktRoleDeptSeedData.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt角色-部门关联种子数据，定义角色数据权限范围
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Entities.Accounting.Financial;
using Takt.Domain.Entities.HumanResource.Organization;
using Takt.Domain.Entities.Identity;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Enums;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Data.Seeds.EntitySeedData;

/// <summary>
/// 角色-部门关联种子数据初始化
/// 幂等性操作：存在则跳过，不存在则创建
/// </summary>
public class TaktRoleDeptSeedData : ITaktSeedDataCoordinator
{
    public int Order => 62;

    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(IServiceProvider serviceProvider, string? tenantCode = null)
    {
        TaktLogger.Information("开始初始化角色-部门关联种子数据...");

        if (string.IsNullOrEmpty(tenantCode))
        {
            TaktLogger.Warning("租户编码为空，跳过角色-部门关联种子数据初始化");
            return (0, 0);
        }

        var repository = serviceProvider.GetRequiredService<ITaktCompanySeedRepository<TaktRoleDept>>();
        var roleRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktRole>>();
        var deptRepository = serviceProvider.GetRequiredService<ITaktCompanySeedRepository<TaktDept>>();
        var companyRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktCompany>>();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var configuredCompanyCodes = configuration.RequireDatabase().CompanyCodes;
        var companies = await companyRepository.GetListAsync(
            c => c.TenantCode == tenantCode && c.CompanyStatus == 1);
        if (companies == null || companies.Count == 0)
        {
            TaktLogger.Warning("租户 {TenantCode} 未找到启用的公司，跳过角色-部门关联种子数据初始化", tenantCode);
            return (0, 0);
        }
        var orderedCompanies = TaktDatabaseOptions.OrderByConfiguredCodes(
            configuredCompanyCodes,
            companies,
            c => c.CompanyCode);
        int insertCount = 0;
        TaktLogger.Information("正在为租户 {TenantCode} 初始化角色-部门关联数据...", tenantCode);
        foreach (var company in orderedCompanies)
        {
            foreach (var (roleCode, deptCode) in GetRoleDeptTemplates())
            {
                insertCount += await CreateRoleDeptAsync(
                    repository,
                    roleRepository,
                    deptRepository,
                    tenantCode,
                    company.CompanyCode,
                    roleCode,
                    deptCode);
            }
        }

        TaktLogger.Information("角色-部门关联种子数据初始化完成: 插入 {InsertCount} 条", insertCount);

        return (insertCount, 0);
    }

    /// <summary>
    /// 标准角色-部门关联模板（公司编码由 Database:CompanyCodes 驱动）
    /// </summary>
    private static IEnumerable<(string RoleCode, string DeptCode)> GetRoleDeptTemplates()
    {
        yield return ("ROLE_SUPER_ADMIN", "HEAD_OFFICE");
        yield return ("ROLE_SUPER_ADMIN", "D0000");
        yield return ("ROLE_DEPT_ADMIN", "D0000");
        yield return ("ROLE_EMPLOYEE", "D0620");
    }

    private static async Task<int> CreateRoleDeptAsync(
        ITaktCompanySeedRepository<TaktRoleDept> repository,
        ITaktTenantSeedRepository<TaktRole> roleRepository,
        ITaktCompanySeedRepository<TaktDept> deptRepository,
        string tenantCode,
        string companyCode,
        string roleCode,
        string deptCode)
    {
        var role = await roleRepository.FirstAsync(r =>
            r.TenantCode == tenantCode &&
            r.RoleCode == roleCode);
        if (role == null) return 0;

        var dept = await deptRepository.FirstAsync(d =>
            d.TenantCode == tenantCode &&
            d.CompanyCode == companyCode &&
            d.DeptCode == deptCode);
        if (dept == null) return 0;

        var exists = await repository.FirstAsync(rd =>
            rd.TenantCode == tenantCode &&
            rd.CompanyCode == companyCode &&
            rd.RoleId == role.Id &&
            rd.DeptId == dept.Id);
        if (exists == null)
        {
            await repository.CreateAsync(new TaktRoleDept
            {
                TenantCode = tenantCode,
                CompanyCode = companyCode,
                RoleId = role.Id,
                DeptId = dept.Id
            });
            return 1;
        }
        return 0;
    }
}
