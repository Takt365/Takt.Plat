// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds
// 文件名称：TaktEmployeeSeedData.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：员工档案种子数据初始化（跨租户：DEV/QAS/PRD）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Entities.Accounting.Financial;
using Takt.Domain.Entities.HumanResource.Personnel;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Enums;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Data.Seeds.EntitySeedData;

/// <summary>
/// 员工档案种子数据初始化
/// 幂等性操作：存在则更新，不存在则创建
/// </summary>
public class TaktEmployeeSeedData : ITaktSeedDataCoordinator
{
    /// <summary>
    /// 执行顺序（在公司之后，用户之前）
    /// </summary>
    public int Order => 10;

    /// <summary>
    /// 初始化员工档案种子数据
    /// </summary>
    /// <param name="serviceProvider">服务提供者</param>
    /// <param name="tenantCode">租户编码（由协调器传入）</param>
    /// <returns>返回插入和更新的记录数（插入数, 更新数）</returns>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(IServiceProvider serviceProvider, string? tenantCode = null)
    {
        TaktLogger.Information("开始初始化员工档案种子数据...");

        if (string.IsNullOrEmpty(tenantCode))
        {
            TaktLogger.Warning("租户编码为空，跳过员工档案种子数据初始化");
            return (0, 0);
        }

        var repository = serviceProvider.GetRequiredService<ITaktCompanySeedRepository<TaktEmployee>>();
        var companyRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktCompany>>();
        var sqlSugarContext = serviceProvider.GetRequiredService<TaktSeedContext>();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var configuredCompanyCodes = configuration.RequireDatabase().CompanyCodes;

        int insertCount = 0;
        int updateCount = 0;

        var companies = await companyRepository.GetListAsync(c => c.TenantCode == tenantCode && c.CompanyStatus == 1);

        if (companies == null || companies.Count == 0)
        {
            TaktLogger.Warning("租户 {TenantCode} 未找到启用的公司，跳过员工档案种子数据初始化", tenantCode);
            return (0, 0);
        }

        var orderedCompanies = TaktDatabaseOptions.OrderByConfiguredCodes(
            configuredCompanyCodes,
            companies,
            c => c.CompanyCode);

        TaktLogger.Information("正在为租户 {TenantCode} 初始化员工档案数据...", tenantCode);

        foreach (var company in orderedCompanies)
        {
            TaktLogger.Information("正在为公司 {CompanyCode} ({CompanyName}) 初始化员工档案...", company.CompanyCode, company.CompanyName);
            var employees = GetStandardEmployees();
            for (var index = 0; index < employees.Count; index++)
            {
                var employeeData = employees[index];
                var (employee, i, u) = await CreateOrUpdateEmployeeAsync(
                    repository,
                    sqlSugarContext,
                    tenantCode,
                    company.CompanyCode,
                    employeeData.EmployeeNo,
                    employeeData.Name,
                    index);
                insertCount += i;
                updateCount += u;
            }
        }

        TaktLogger.Information("员工档案种子数据初始化完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);

        return (insertCount, updateCount);
    }

    /// <summary>
    /// 获取标准员工档案列表
    /// </summary>
    private static List<(string EmployeeNo, string Name)> GetStandardEmployees()
    {
        return new List<(string, string)>
        {
            ("900001", "管理员"),
            ("900002", "访客"),
            ("900003", "演示用户")
        };
    }

    /// <summary>
    /// 填充人事档案必填个人信息（种子演示数据）
    /// </summary>
    private static void ApplySeedPersonalProfile(TaktEmployee employee, int sequence)
    {
        employee.BirthDate = new DateTime(1990, 1, 1).AddDays(sequence);
        employee.IdCardNo = sequence switch
        {
            0 => "110101199001011237",
            1 => "110101199002021242",
            _ => "110101199003031258"
        };
        employee.Mobile = $"1380013800{sequence}";
        employee.NativePlace = "110000";
        employee.Ethnicity = 1;
        employee.PoliticalStatus = 0;
        employee.MaritalStatus = 0;
        employee.EmergencyContactName = "紧急联系人";
        employee.EmergencyContactPhone = $"1390013900{sequence}";
        employee.HomeAddress = "北京市朝阳区种子路1号";
    }

    /// <summary>
    /// 创建或更新员工档案
    /// </summary>
    private static async Task<(TaktEmployee Employee, int InsertCount, int UpdateCount)> CreateOrUpdateEmployeeAsync(
        ITaktCompanySeedRepository<TaktEmployee> repository,
        TaktSeedContext sqlSugarContext,
        string tenantCode,
        string companyCode,
        string employeeNo,
        string name,
        int sequence)
    {
        var employee = await repository.FirstAsync(e => e.TenantCode == tenantCode && e.CompanyCode == companyCode && e.EmployeeNo == employeeNo);
        if (employee == null)
        {
            employee = new TaktEmployee
            {
                TenantCode = tenantCode,
                CompanyCode = companyCode,
                EmployeeNo = employeeNo,
                Name = name,
                Gender = 1,
                EmployeeStatus = 2,
                JoinedDate = DateTime.Now,
                IsBuiltIn = 1
            };
            ApplySeedPersonalProfile(employee, sequence);
            employee = await repository.CreateAsync(employee);
            return (employee, 1, 0);
        }
        employee.Name = name;
        employee.EmployeeStatus = 2;
        employee.IsBuiltIn = 1;
        ApplySeedPersonalProfile(employee, sequence);
        await sqlSugarContext.Db.Updateable(employee)
            .IgnoreColumns(x => x.EmployeeNo)
            .Where(x => x.TenantCode == tenantCode && x.CompanyCode == companyCode)
            .ExecuteCommandAsync();
        return (employee, 0, 1);
    }
}
