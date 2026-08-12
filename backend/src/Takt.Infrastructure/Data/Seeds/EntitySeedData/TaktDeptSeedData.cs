// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds
// 文件名称：TaktDeptSeedData.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt部门种子数据，按 Database:CompanyCodes 初始化各公司组织架构
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Takt.Domain.Entities.HumanResource.Organization;
using Takt.Domain.Entities.Accounting.Financial;
using Takt.Domain.Entities.Identity;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Enums;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Data.Seeds.EntitySeedData;

/// <summary>
/// 部门种子数据初始化（按 Database:CompanyCodes 顺序）
/// 幂等性操作：存在则更新，不存在则创建
/// </summary>
public class TaktDeptSeedData : ITaktSeedDataCoordinator
{
    /// <summary>
    /// 执行顺序（在菜单之后，岗位之前）
    /// </summary>
    public int Order => 30;

    /// <summary>
    /// 初始化部门种子数据
    /// 注意：每个租户数据库只初始化自己的部门数据
    /// Program.cs 会为每个租户数据库调用此方法，因此只需为当前租户初始化部门
    /// </summary>
    /// <param name="serviceProvider">服务提供者</param>
    /// <param name="tenantCode">租户编码（由协调器传入）</param>
    /// <returns>返回插入和更新的记录数（插入数, 更新数）</returns>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(IServiceProvider serviceProvider, string? tenantCode = null)
    {
        TaktLogger.Information("开始初始化部门种子数据...");

        // 参数验证：必须使用协调器传入的租户编码
        if (string.IsNullOrEmpty(tenantCode))
        {
            TaktLogger.Warning("租户编码为空，跳过部门种子数据初始化");
            return (0, 0);
        }

        var repository = serviceProvider.GetRequiredService<ITaktCompanySeedRepository<TaktDept>>();
        var companyRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktCompany>>();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var database = configuration.RequireDatabase();
        var configuredCompanyCodes = database.CompanyCodes;

        int insertCount = 0;
        int updateCount = 0;

        var companies = await companyRepository.GetListAsync(c => c.TenantCode == tenantCode && c.CompanyStatus == 1);

        if (companies == null || companies.Count == 0)
        {
            TaktLogger.Warning("租户 {TenantCode} 未找到启用的公司，跳过部门种子数据初始化", tenantCode);
            return (0, 0);
        }

        var orderedCompanies = TaktDatabaseOptions.OrderByConfiguredCodes(
            configuredCompanyCodes,
            companies,
            c => c.CompanyCode);

        TaktLogger.Information(
            "正在为租户 {TenantCode} 初始化部门数据（顺序: {CompanyCodes}）...",
            tenantCode,
            string.Join(", ", configuredCompanyCodes));

        foreach (var company in orderedCompanies)
        {
            TaktLogger.Information("正在为公司 {CompanyCode} ({CompanyName1}) 初始化部门...", company.CompanyCode, company.CompanyName1);
            
            var result = await SeedDeptsForCompanyAsync(repository, tenantCode, company.CompanyCode, company.CultureCode);
            insertCount += result.InsertCount;
            updateCount += result.UpdateCount;
        }

        TaktLogger.Information("租户 {TenantCode} 部门种子数据初始化完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", 
            tenantCode, insertCount, updateCount);

        TaktLogger.Information("部门种子数据初始化完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);

        return (insertCount, updateCount);
    }



    /// <summary>
    /// 为指定公司创建部门架构
    /// </summary>
    private static async Task<(int InsertCount, int UpdateCount)> SeedDeptsForCompanyAsync(
        ITaktCompanySeedRepository<TaktDept> repository,
        string tenantCode,
        string companyCode,
        string cultureCode)
    {
        int insertCount = 0;
        int updateCount = 0;
        
        // 根：总公司 TEAC (ParentId=0)
        var (headOffice, i0a) = await CreateOrUpdateDeptAsync(repository, tenantCode, companyCode, cultureCode, "HEAD_OFFICE", "TEAC", 1, 0, 0);
        insertCount += (i0a ? 1 : 0); updateCount += (i0a ? 0 : 1);

        // DTA（组织编码 D0000, ParentId=headOffice.Id）
        var (dta, iD0) = await CreateOrUpdateDeptAsync(repository, tenantCode, companyCode, cultureCode, "D0000", "DTA", 1, headOffice.Id, 1);
        insertCount += (iD0 ? 1 : 0); updateCount += (iD0 ? 0 : 1);

        // —— DTA 下一级（D1000～D0900, ParentId=dta.Id）——
        var (d1000, i1) = await CreateOrUpdateDeptAsync(repository, tenantCode, companyCode, cultureCode, "D1000", "总经理室", 1, dta.Id, 1);
        insertCount += (i1 ? 1 : 0); updateCount += (i1 ? 0 : 1);
        
        var (d0100, i2) = await CreateOrUpdateDeptAsync(repository, tenantCode, companyCode, cultureCode, "D0100", "总务部", 1, dta.Id, 2);
        insertCount += (i2 ? 1 : 0); updateCount += (i2 ? 0 : 1);
        
        var (d0200, i3) = await CreateOrUpdateDeptAsync(repository, tenantCode, companyCode, cultureCode, "D0200", "财务部", 1, dta.Id, 3);
        insertCount += (i3 ? 1 : 0); updateCount += (i3 ? 0 : 1);
        
        var (d0300, i4) = await CreateOrUpdateDeptAsync(repository, tenantCode, companyCode, cultureCode, "D0300", "IT部", 2, dta.Id, 4);
        insertCount += (i4 ? 1 : 0); updateCount += (i4 ? 0 : 1);
        
        var (d0400, i5) = await CreateOrUpdateDeptAsync(repository, tenantCode, companyCode, cultureCode, "D0400", "管理部", 1, dta.Id, 5);
        insertCount += (i5 ? 1 : 0); updateCount += (i5 ? 0 : 1);
        
        var (d0500, i6) = await CreateOrUpdateDeptAsync(repository, tenantCode, companyCode, cultureCode, "D0500", "资材部", 1, dta.Id, 6);
        insertCount += (i6 ? 1 : 0); updateCount += (i6 ? 0 : 1);
        
        var (d0600, i7) = await CreateOrUpdateDeptAsync(repository, tenantCode, companyCode, cultureCode, "D0600", "生产部", 2, dta.Id, 7);
        insertCount += (i7 ? 1 : 0); updateCount += (i7 ? 0 : 1);
        
        var (d0700, i8) = await CreateOrUpdateDeptAsync(repository, tenantCode, companyCode, cultureCode, "D0700", "技术部", 2, dta.Id, 8);
        insertCount += (i8 ? 1 : 0); updateCount += (i8 ? 0 : 1);
        
        var (d0800, i9) = await CreateOrUpdateDeptAsync(repository, tenantCode, companyCode, cultureCode, "D0800", "品保部", 2, dta.Id, 9);
        insertCount += (i9 ? 1 : 0); updateCount += (i9 ? 0 : 1);
        
        var (d0900, i10) = await CreateOrUpdateDeptAsync(repository, tenantCode, companyCode, cultureCode, "D0900", "OEM部", 1, dta.Id, 10);
        insertCount += (i10 ? 1 : 0); updateCount += (i10 ? 0 : 1);

        // —— 总务部 D0100 → D0110 (ParentId=d0100.Id) ——
        var (_, i11) = await CreateOrUpdateDeptAsync(repository, tenantCode, companyCode, cultureCode, "D0110", "总务课", 1, d0100.Id, 1);
        insertCount += (i11 ? 1 : 0); updateCount += (i11 ? 0 : 1);

        // —— 财务部 D0200 → D0210 (ParentId=d0200.Id) ——
        var (_, i12) = await CreateOrUpdateDeptAsync(repository, tenantCode, companyCode, cultureCode, "D0210", "财务课", 1, d0200.Id, 1);
        insertCount += (i12 ? 1 : 0); updateCount += (i12 ? 0 : 1);

        // —— IT 部 D0300 → D0310 (ParentId=d0300.Id) ——
        var (_, i13) = await CreateOrUpdateDeptAsync(repository, tenantCode, companyCode, cultureCode, "D0310", "电脑课", 2, d0300.Id, 1);
        insertCount += (i13 ? 1 : 0); updateCount += (i13 ? 0 : 1);

        // —— 管理部 D0400 → 报关 / 生管 / 部管 (ParentId=d0400.Id) ——
        var (_, i14) = await CreateOrUpdateDeptAsync(repository, tenantCode, companyCode, cultureCode, "D0410", "报关课", 1, d0400.Id, 1);
        insertCount += (i14 ? 1 : 0); updateCount += (i14 ? 0 : 1);
        
        var (_, i15) = await CreateOrUpdateDeptAsync(repository, tenantCode, companyCode, cultureCode, "D0420", "生管课", 1, d0400.Id, 2);
        insertCount += (i15 ? 1 : 0); updateCount += (i15 ? 0 : 1);
        
        var (_, i16) = await CreateOrUpdateDeptAsync(repository, tenantCode, companyCode, cultureCode, "D0430", "部管课", 1, d0400.Id, 3);
        insertCount += (i16 ? 1 : 0); updateCount += (i16 ? 0 : 1);

        // —— 资材部 D0500 → D0510 (ParentId=d0500.Id) ——
        var (_, i17) = await CreateOrUpdateDeptAsync(repository, tenantCode, companyCode, cultureCode, "D0510", "采购课", 1, d0500.Id, 1);
        insertCount += (i17 ? 1 : 0); updateCount += (i17 ? 0 : 1);

        // —— 生产部 D0600 → 制造一课 / 制造二课 / 制造技术课 (ParentId=d0600.Id) ——
        var (_, i18) = await CreateOrUpdateDeptAsync(repository, tenantCode, companyCode, cultureCode, "D0610", "制造1课", 2, d0600.Id, 1);
        insertCount += (i18 ? 1 : 0); updateCount += (i18 ? 0 : 1);
        
        var (d0620, i19) = await CreateOrUpdateDeptAsync(repository, tenantCode, companyCode, cultureCode, "D0620", "制造2课", 2, d0600.Id, 2);
        insertCount += (i19 ? 1 : 0); updateCount += (i19 ? 0 : 1);
        
        var (_, i20) = await CreateOrUpdateDeptAsync(repository, tenantCode, companyCode, cultureCode, "D0630", "制造技术课", 2, d0600.Id, 3);
        insertCount += (i20 ? 1 : 0); updateCount += (i20 ? 0 : 1);

        // —— 制造2课 D0620 下级：SMT / 自插 / 修正 / 手插 / 物料 / 制造2课-间接 (ParentId=d0620.Id) ——
        var (_, i21) = await CreateOrUpdateDeptAsync(repository, tenantCode, companyCode, cultureCode, "D0621", "SMT", 2, d0620.Id, 1);
        insertCount += (i21 ? 1 : 0); updateCount += (i21 ? 0 : 1);
        
        var (_, i22) = await CreateOrUpdateDeptAsync(repository, tenantCode, companyCode, cultureCode, "D0622", "自插", 2, d0620.Id, 2);
        insertCount += (i22 ? 1 : 0); updateCount += (i22 ? 0 : 1);
        
        var (_, i23) = await CreateOrUpdateDeptAsync(repository, tenantCode, companyCode, cultureCode, "D0623", "修正", 2, d0620.Id, 3);
        insertCount += (i23 ? 1 : 0); updateCount += (i23 ? 0 : 1);
        
        var (_, i24) = await CreateOrUpdateDeptAsync(repository, tenantCode, companyCode, cultureCode, "D0624", "手插", 2, d0620.Id, 4);
        insertCount += (i24 ? 1 : 0); updateCount += (i24 ? 0 : 1);
        
        var (_, i25) = await CreateOrUpdateDeptAsync(repository, tenantCode, companyCode, cultureCode, "D0625", "物料", 1, d0620.Id, 5);
        insertCount += (i25 ? 1 : 0); updateCount += (i25 ? 0 : 1);
        
        var (_, i26) = await CreateOrUpdateDeptAsync(repository, tenantCode, companyCode, cultureCode, "D0626", "制造2课-间接", 2, d0620.Id, 6);
        insertCount += (i26 ? 1 : 0); updateCount += (i26 ? 0 : 1);

        // —— 技术部 D0700 → D0710 (ParentId=d0700.Id) ——
        var (_, i27) = await CreateOrUpdateDeptAsync(repository, tenantCode, companyCode, cultureCode, "D0710", "技术课", 2, d0700.Id, 1);
        insertCount += (i27 ? 1 : 0); updateCount += (i27 ? 0 : 1);

        // —— 品保部 D0800 → 受检课 / 品管课 (ParentId=d0800.Id) ——
        var (_, i28) = await CreateOrUpdateDeptAsync(repository, tenantCode, companyCode, cultureCode, "D0810", "受检课", 2, d0800.Id, 1);
        insertCount += (i28 ? 1 : 0); updateCount += (i28 ? 0 : 1);
        
        var (_, i29) = await CreateOrUpdateDeptAsync(repository, tenantCode, companyCode, cultureCode, "D0820", "品管课", 2, d0800.Id, 2);
        insertCount += (i29 ? 1 : 0); updateCount += (i29 ? 0 : 1);

        // —— OEM 部 D0900 → OEM QA课 / OEM管理课 (ParentId=d0900.Id) ——
        var (_, i30) = await CreateOrUpdateDeptAsync(repository, tenantCode, companyCode, cultureCode, "D0910", "OEM QA课", 2, d0900.Id, 1);
        insertCount += (i30 ? 1 : 0); updateCount += (i30 ? 0 : 1);
        
        var (_, i31) = await CreateOrUpdateDeptAsync(repository, tenantCode, companyCode, cultureCode, "D0920", "OEM管理课", 1, d0900.Id, 2);
        insertCount += (i31 ? 1 : 0); updateCount += (i31 ? 0 : 1);

        return (insertCount, updateCount);
    }

    /// <summary>
    /// 创建或更新部门
    /// </summary>
    /// <param name="repository">部门仓储</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司代码</param>
    /// <param name="deptCode">部门编码</param>
    /// <param name="deptName">部门名称</param>
    /// <param name="costCategory">费用类别</param>
    /// <param name="parentId">父部门ID（0表示根部门）</param>
    /// <param name="sortOrder">排序</param>
    /// <returns>(部门实体, 是否为新插入)</returns>
    private static async Task<(TaktDept Dept, bool IsInserted)> CreateOrUpdateDeptAsync(
        ITaktCompanySeedRepository<TaktDept> repository,
        string tenantCode,
        string companyCode,
        string cultureCode,
        string deptCode,
        string deptName,
        int costCategory,
        long parentId,
        int sortOrder)
    {
        // 使用仓储查询，自动应用租户过滤
        var dept = await repository.FirstAsync(d => d.TenantCode == tenantCode && d.CompanyCode == companyCode && d.DeptCode == deptCode);
        
        if (dept == null)
        {
            // 不存在：创建新记录
            dept = new TaktDept
            {
                TenantCode = tenantCode,
                CompanyCode = companyCode,
                DeptCode = deptCode,
                DeptName = deptName,
                CostCenterCode = "",
                CostCategory = costCategory,
                ParentId = parentId,
                Level = 0,
                IsLeaf = 1,
                DeptPath = "",
                DeptStatus = 1,
                SortOrder = sortOrder,
                IsBuiltIn = 1,
                CultureCode = cultureCode
            };
            
            // 使用仓储插入（自动生成雪花ID和审计字段）
            await repository.CreateAsync(dept);
            
            // 自动计算 Level 和 DeptPath
            if (dept.ParentId > 0)
            {
                // 使用仓储查询父部门，自动应用租户过滤
                var parentDept = await repository.GetByIdAsync(dept.ParentId);
                if (parentDept != null)
                {
                    dept.DeptPath = $"{parentDept.DeptPath}{dept.Id}/";
                    dept.Level = parentDept.Level + 1;
                    
                    // 更新父级 IsLeaf 为非叶子
                    if (parentDept.IsLeaf == 1)
                    {
                        parentDept.IsLeaf = 0;
                        await repository.UpdateAsync(parentDept);
                    }
                }
            }
            else
            {
                dept.DeptPath = $"/{dept.Id}/";
                dept.Level = 1;
            }
            
            // 更新 Level 和 DeptPath
            await repository.UpdateAsync(dept);
            return (dept, true);
        }
        else
        {
            // 存在：检查是否需要更新
            bool needUpdate = false;
            
            if (dept.DeptName != deptName)
            {
                dept.DeptName = deptName;
                needUpdate = true;
            }
            
            if (dept.CostCategory != costCategory)
            {
                dept.CostCategory = costCategory;
                needUpdate = true;
            }
            
            if (dept.SortOrder != sortOrder)
            {
                dept.SortOrder = sortOrder;
                needUpdate = true;
            }
            if (dept.IsBuiltIn != 1)
            {
                dept.IsBuiltIn = 1;
                needUpdate = true;
            }

            if (dept.CultureCode != cultureCode)
            {
                dept.CultureCode = cultureCode;
                needUpdate = true;
            }
            
            // 重新计算 Level 和 DeptPath（如果 ParentId 发生变化）
            if (dept.ParentId != parentId)
            {
                dept.ParentId = parentId;
                needUpdate = true;
                
                if (dept.ParentId > 0)
                {
                    // 使用仓储查询父部门，自动应用租户过滤
                    var parentDept = await repository.GetByIdAsync(dept.ParentId);
                    if (parentDept != null)
                    {
                        dept.DeptPath = $"{parentDept.DeptPath}{dept.Id}/";
                        dept.Level = parentDept.Level + 1;
                        
                        // 更新父级 IsLeaf 为非叶子
                        if (parentDept.IsLeaf == 1)
                        {
                            parentDept.IsLeaf = 0;
                            await repository.UpdateAsync(parentDept);
                        }
                    }
                }
                else
                {
                    dept.DeptPath = $"/{dept.Id}/";
                    dept.Level = 1;
                }
            }

            // 只有数据发生变化时才更新
            if (needUpdate)
            {
                dept.DeptStatus = 1;
                await repository.UpdateAsync(dept);
            }
            
            return (dept, false);
        }
    }
}
