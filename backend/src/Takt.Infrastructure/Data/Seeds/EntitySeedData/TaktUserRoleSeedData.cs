// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds
// 文件名称：TaktUserRoleSeedData.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt用户-角色关联种子数据，初始化RBAC权限关联
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Entities.Identity;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.EntitySeedData;

/// <summary>
/// 用户-角色关联种子数据初始化
/// 幂等性操作：存在则跳过，不存在则创建
/// </summary>
public class TaktUserRoleSeedData : ITaktSeedDataCoordinator
{
    /// <summary>
    /// 执行顺序（在 User 和 Role 之后）
    /// </summary>
    public int Order => 60;

    /// <summary>
    /// 初始化用户-角色关联种子数据
    /// </summary>
    /// <param name="serviceProvider">服务提供者</param>
    /// <param name="tenantCode">租户编码（由协调器传入）</param>
    /// <returns>返回插入和更新的记录数（插入数, 更新数）</returns>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(IServiceProvider serviceProvider, string? tenantCode = null)
    {
        TaktLogger.Information("开始初始化用户-角色关联种子数据...");
        
        // 参数验证
        if (string.IsNullOrEmpty(tenantCode))
        {
            TaktLogger.Warning("租户编码为空，跳过用户-角色关联种子数据初始化");
            return (0, 0);
        }

        var userRoleRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktUserRole>>();
        var userRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktUser>>();
        var roleRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktRole>>();
        
        int insertCount = 0;
        
        TaktLogger.Information("正在为租户 {TenantCode} 初始化用户-角色关联数据...", tenantCode);
        
        // 为当前租户初始化标准用户-角色关联
        var userRoles = GetStandardUserRoles();
        
        foreach (var userRoleData in userRoles)
        {
            insertCount += await CreateUserRoleAsync(
                userRoleRepository,
                userRepository,
                roleRepository,
                tenantCode,
                userRoleData.Username,
                userRoleData.RoleCode);
        }
        
        TaktLogger.Information("用户-角色关联种子数据初始化完成: 插入 {InsertCount} 条", insertCount);
        
        return (insertCount, 0);
    }

    /// <summary>
    /// 获取标准用户-角色关联列表
    /// </summary>
    private static List<(string Username, string RoleCode)> GetStandardUserRoles()
    {
        return new List<(string, string)>
        {
            ("admin", "ROLE_SUPER_ADMIN"),
            ("guest", "ROLE_EMPLOYEE"),
            ("demo", "ROLE_EMPLOYEE")
        };
    }

    /// <summary>
    /// 创建用户-角色关联（幂等：存在则跳过）
    /// </summary>
    private static async Task<int> CreateUserRoleAsync(
        ITaktTenantSeedRepository<TaktUserRole> userRoleRepository,
        ITaktTenantSeedRepository<TaktUser> userRepository,
        ITaktTenantSeedRepository<TaktRole> roleRepository,
        string tenantCode,
        string username,
        string roleCode)
    {
        // 1. 查找用户ID
        var user = await userRepository.FirstAsync(u => u.TenantCode == tenantCode && u.Username == username);
        if (user == null)
        {
            TaktLogger.Warning("用户 {TenantCode}/{Username} 不存在，跳过角色关联", tenantCode, username);
            return 0;
        }

        // 2. 查找角色ID
        var role = await roleRepository.FirstAsync(r => r.TenantCode == tenantCode && r.RoleCode == roleCode);
        if (role == null)
        {
            TaktLogger.Warning("角色 {TenantCode}/{RoleCode} 不存在，跳过用户关联", tenantCode, roleCode);
            return 0;
        }

        // 3. 检查关联是否已存在
        var exists = await userRoleRepository.FirstAsync(ur => 
            ur.TenantCode == tenantCode && 
            ur.UserId == user.Id && 
            ur.RoleId == role.Id);

        if (exists != null)
        {
            return 0; // 已存在，跳过
        }

        // 4. 创建关联
        await userRoleRepository.CreateAsync(new TaktUserRole
        {
            TenantCode = tenantCode,
            UserId = user.Id,
            RoleId = role.Id
        });
        
        return 1;
    }
}
