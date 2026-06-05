// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds
// 文件名称：TaktRoleMenuSeedData.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt角色-菜单关联种子数据，初始化RBAC菜单权限关联
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
/// 角色-菜单关联种子数据初始化
/// 幂等性操作：存在则跳过，不存在则创建
/// </summary>
public class TaktRoleMenuSeedData : ITaktSeedDataCoordinator
{
    /// <summary>
    /// 执行顺序（在菜单种子之后，其他关联种子之前）
    /// </summary>
    public int Order => 50;

    /// <summary>
    /// 初始化角色-菜单关联种子数据
    /// </summary>
    /// <param name="serviceProvider">服务提供者</param>
    /// <param name="tenantCode">租户编码（由协调器传入）</param>
    /// <returns>返回插入和更新的记录数（插入数, 更新数）</returns>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(IServiceProvider serviceProvider, string? tenantCode = null)
    {
        TaktLogger.Information("开始初始化角色-菜单关联种子数据...");

        // 参数验证
        if (string.IsNullOrEmpty(tenantCode))
        {
            TaktLogger.Warning("租户编码为空，跳过角色-菜单关联种子数据初始化");
            return (0, 0);
        }

        var repository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktRoleMenu>>();
        var roleRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktRole>>();
        var menuRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktMenu>>();

        int insertCount = 0;

        TaktLogger.Information("正在为租户 {TenantCode} 初始化角色-菜单关联数据...", tenantCode);

        // 为当前租户初始化标准角色-菜单关联
        var roleMenus = GetStandardRoleMenus();
        
        foreach (var roleMenuData in roleMenus)
        {
            if (roleMenuData.AssignAll)
            {
                // 超级管理员：分配所有菜单
                insertCount += await AssignAllMenusToRoleAsync(
                    repository,
                    roleRepository,
                    menuRepository,
                    tenantCode,
                    roleMenuData.RoleCode);
            }
            else
            {
                // 其他角色：分配指定菜单
                insertCount += await AssignMenuByCodesToRoleAsync(
                    repository,
                    roleRepository,
                    menuRepository,
                    tenantCode,
                    roleMenuData.RoleCode,
                    roleMenuData.MenuCodes!);
            }
        }

        TaktLogger.Information("角色-菜单关联种子数据初始化完成: 插入 {InsertCount} 条", insertCount);

        return (insertCount, 0);
    }

    /// <summary>
    /// 获取标准角色-菜单关联列表
    /// </summary>
    private static List<(string RoleCode, bool AssignAll, string[]? MenuCodes)> GetStandardRoleMenus()
    {
        return new List<(string, bool, string[]?)>
        {
            ("ROLE_SUPER_ADMIN", true, null),  // 超级管理员：所有菜单
            ("ROLE_DEPT_ADMIN", false, new[] { "HOME", "ABOUT", "ROUTINE", "IDENTITY" }),  // 部门管理员
            ("ROLE_EMPLOYEE", false, new[] { "HOME", "ABOUT", "ROUTINE" })  // 普通员工
        };
    }

    /// <summary>
    /// 为角色分配所有菜单（超级管理员专用）
    /// </summary>
    private static async Task<int> AssignAllMenusToRoleAsync(
        ITaktTenantSeedRepository<TaktRoleMenu> repository,
        ITaktTenantSeedRepository<TaktRole> roleRepository,
        ITaktTenantSeedRepository<TaktMenu> menuRepository,
        string tenantCode,
        string roleCode)
    {
        var role = await roleRepository.FirstAsync(r => r.TenantCode == tenantCode && r.RoleCode == roleCode);
        if (role == null) return 0;

        var allMenus = await menuRepository.GetListAsync(m => m.TenantCode == tenantCode);
        if (allMenus == null || allMenus.Count == 0) return 0;

        int insertCount = 0;
        foreach (var menu in allMenus)
        {
            insertCount += await CreateRoleMenuAsync(repository, tenantCode, role.Id, menu.Id);
        }

        return insertCount;
    }

    /// <summary>
    /// 为角色分配指定编码的菜单
    /// </summary>
    private static async Task<int> AssignMenuByCodesToRoleAsync(
        ITaktTenantSeedRepository<TaktRoleMenu> repository,
        ITaktTenantSeedRepository<TaktRole> roleRepository,
        ITaktTenantSeedRepository<TaktMenu> menuRepository,
        string tenantCode,
        string roleCode,
        params string[] menuCodes)
    {
        var role = await roleRepository.FirstAsync(r => r.TenantCode == tenantCode && r.RoleCode == roleCode);
        if (role == null) return 0;

        int insertCount = 0;
        foreach (var menuCode in menuCodes)
        {
            var menu = await menuRepository.FirstAsync(m => m.TenantCode == tenantCode && m.MenuCode == menuCode);
            if (menu == null) continue;

            insertCount += await CreateRoleMenuAsync(repository, tenantCode, role.Id, menu.Id);
        }

        return insertCount;
    }

    /// <summary>
    /// 创建角色-菜单关联（幂等：存在则跳过）
    /// </summary>
    private static async Task<int> CreateRoleMenuAsync(
        ITaktTenantSeedRepository<TaktRoleMenu> repository,
        string tenantCode,
        long roleId,
        long menuId)
    {
        var exists = await repository.FirstAsync(rm =>
            rm.TenantCode == tenantCode &&
            rm.RoleId == roleId &&
            rm.MenuId == menuId);
        if (exists != null) return 0;

        await repository.CreateAsync(new TaktRoleMenu
        {
            TenantCode = tenantCode,
            RoleId = roleId,
            MenuId = menuId
        });

        return 1;
    }
}
