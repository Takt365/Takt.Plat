// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds
// 文件名称：TaktMenuSeedData.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt菜单种子数据协调器，统一协调处理所有层级的菜单初始化。
//           按顺序调用 Level1 → Level2 → Level3 → Level4 → Level5 → Button 种子数据。
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Interfaces;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.EntitySeedData;

/// <summary>
/// Takt 菜单种子数据协调器。
/// <para>
/// 统一协调处理所有层级的菜单初始化，按顺序调用：
/// <list type="number">
///   <item><description>一级菜单（顶级目录）：<see cref="TaktMenuLevel1SeedData"/></description></item>
///   <item><description>二级菜单（模块分类）：<see cref="TaktMenuLevel2SeedData"/></description></item>
///   <item><description>三级菜单（页面功能）：<see cref="TaktMenuLevel3SeedData"/></description></item>
///   <item><description>四级菜单（功能细分）：<see cref="TaktMenuLevel4SeedData"/></description></item>
///   <item><description>五级菜单（操作级）：<see cref="TaktMenuLevel5SeedData"/></description></item>
///   <item><description>按钮权限（操作按钮）：<see cref="TaktMenuButtonSeedData"/></description></item>
/// </list>
/// </para>
/// </summary>
public class TaktMenuSeedData : ITaktSeedDataCoordinator
{
    /// <summary>
    /// 执行顺序（在一级菜单之前，作为总体协调器）
    /// </summary>
    public int Order => 39;

    /// <summary>
    /// 初始化菜单种子数据（统一协调处理所有层级）。
    /// </summary>
    /// <param name="serviceProvider">服务提供者。</param>
    /// <param name="tenantCode">租户编码（由协调器传入）。</param>
    /// <returns>元组：(InsertCount, UpdateCount)，所有层级菜单的新增与更新总数。</returns>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(IServiceProvider serviceProvider, string? tenantCode = null)
    {
        // 参数验证：必须使用协调器传入的租户编码
        if (string.IsNullOrEmpty(tenantCode))
        {
            TaktLogger.Warning("租户编码为空，跳过菜单种子数据初始化");
            return (0, 0);
        }

        int totalInsertCount = 0;
        int totalUpdateCount = 0;

        TaktLogger.Information("正在为租户 {TenantCode} 初始化菜单...", tenantCode);

        // 注意：菜单为租户级实体，需要为当前租户创建完整的菜单树
        // 层级依赖关系：Level1 -> Level2 -> Level3 -> Level4 -> Level5 -> Button
        // 每个 Level 执行完后，下一Level 可以直接从数据库查询父级菜单ID

        // ========== 第一级：顶级菜单（ParentId = 0） ==========
        var level1Seed = new TaktMenuLevel1SeedData();
        var (insert1, update1) = await level1Seed.SeedAsync(serviceProvider, tenantCode);
        totalInsertCount += insert1;
        totalUpdateCount += update1;

        // ========== 第二级：二级菜单（依赖顶级菜单） ==========
        var level2Seed = new TaktMenuLevel2SeedData();
        var (insert2, update2) = await level2Seed.SeedAsync(serviceProvider, tenantCode);
        totalInsertCount += insert2;
        totalUpdateCount += update2;

        // ========== 第三级：三级菜单（依赖二级菜单） ==========
        var level3Seed = new TaktMenuLevel3SeedData();
        var (insert3, update3) = await level3Seed.SeedAsync(serviceProvider, tenantCode);
        totalInsertCount += insert3;
        totalUpdateCount += update3;

        // ========== 第四级：四级菜单（依赖三级菜单） ==========
        var level4Seed = new TaktMenuLevel4SeedData();
        var (insert4, update4) = await level4Seed.SeedAsync(serviceProvider, tenantCode);
        totalInsertCount += insert4;
        totalUpdateCount += update4;

        // ========== 第五级：五级菜单（依赖四级菜单） ==========
        var level5Seed = new TaktMenuLevel5SeedData();
        var (insert5, update5) = await level5Seed.SeedAsync(serviceProvider, tenantCode);
        totalInsertCount += insert5;
        totalUpdateCount += update5;

        // ========== 按钮权限（依赖五级菜单） ==========
        var buttonSeed = new TaktMenuButtonSeedData();
        var (insert6, update6) = await buttonSeed.SeedAsync(serviceProvider, tenantCode);
        totalInsertCount += insert6;
        totalUpdateCount += update6;

        TaktLogger.Information("租户 {TenantCode} 菜单种子数据初始化完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条",
            tenantCode, totalInsertCount, totalUpdateCount);

        return (totalInsertCount, totalUpdateCount);
    }
}
