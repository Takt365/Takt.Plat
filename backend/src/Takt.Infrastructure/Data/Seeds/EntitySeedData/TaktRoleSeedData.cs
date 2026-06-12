// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds
// 文件名称：TaktRoleSeedData.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：角色种子数据初始化
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Entities.Identity;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Enums;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.EntitySeedData;

/// <summary>
/// 角色种子数据初始化
/// 幂等性操作：存在则更新，不存在则创建
/// 注意：每个租户数据库只初始化自己的角色数据
/// Program.cs 会为每个租户数据库调用此方法，因此只需为当前租户初始化角色
/// </summary>
public class TaktRoleSeedData : ITaktSeedDataCoordinator
{
    /// <summary>
    /// 执行顺序（在用户之后，部门之前）
    /// </summary>
    public int Order => 25;

    /// <summary>
    /// 初始化角色种子数据
    /// </summary>
    /// <param name="serviceProvider">服务提供者</param>
    /// <param name="tenantCode">租户编码（由协调器传入）</param>
    /// <returns>返回插入和更新的记录数（插入数, 更新数）</returns>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(IServiceProvider serviceProvider, string? tenantCode = null)
    {
        TaktLogger.Information("开始初始化角色种子数据...");

        // 参数验证
        if (string.IsNullOrEmpty(tenantCode))
        {
            TaktLogger.Warning("租户编码为空，跳过角色种子数据初始化");
            return (0, 0);
        }

        var repository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktRole>>();

        int insertCount = 0;
        int updateCount = 0;

        TaktLogger.Information("正在为租户 {TenantCode} 初始化角色数据...", tenantCode);

        // 为当前租户初始化标准角色
        var roles = GetStandardRoles(tenantCode);
        
        foreach (var roleData in roles)
        {
            var (role, i, u) = await CreateOrUpdateRoleAsync(
                repository,
                tenantCode,
                roleData.RoleCode,
                roleData.RoleName,
                roleData.DataScope,
                roleData.SortOrder);
            
            insertCount += i;
            updateCount += u;
        }

        TaktLogger.Information("角色种子数据初始化完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);

        return (insertCount, updateCount);
    }

    /// <summary>
    /// 获取当前租户的标准角色列表
    /// </summary>
    private static List<(string RoleCode, string RoleName, int DataScope, int SortOrder)> GetStandardRoles(string tenantCode)
    {
        return new List<(string, string, int, int)>
        {
            ("ROLE_SUPER_ADMIN", "超级管理员", (int)1, 1),
            ("ROLE_DEPT_ADMIN", "部门管理员", (int)3, 2),
            ("ROLE_EMPLOYEE", "普通员工", (int)4, 3)
        };
    }

    /// <summary>
    /// 创建或更新角色
    /// </summary>
    private static async Task<(TaktRole Role, int InsertCount, int UpdateCount)> CreateOrUpdateRoleAsync(
        ITaktTenantSeedRepository<TaktRole> repository,
        string tenantCode,
        string roleCode,
        string roleName,
        int dataScope,
        int sortOrder)
    {
        var role = await repository.FirstAsync(r => r.TenantCode == tenantCode && r.RoleCode == roleCode);
        
        if (role == null)
        {
            // 不存在：创建新记录（仓储会自动生成雪花ID和审计字段）
            role = new TaktRole
            {
                TenantCode = tenantCode,
                RoleCode = roleCode,
                RoleName = roleName,
                DataScope = dataScope,
                IsBuiltIn = 1,
                RoleStatus = 1,
                SortOrder = sortOrder
            };
            role = await repository.CreateAsync(role);
            return (role, 1, 0);
        }
        else
        {
            // 存在：更新记录
            role.RoleName = roleName;
            role.DataScope = dataScope;
            role.IsBuiltIn = 1;
            role.RoleStatus = 1;
            role.SortOrder = sortOrder;

            await repository.UpdateAsync(role);
            return (role, 0, 1);
        }
    }
}
