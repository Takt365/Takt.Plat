// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds
// 文件名称：TaktMenuLevel5SeedData.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt 五级菜单种子数据。
//           在四级菜单已存在的前提下，挂载 OPH（产出）与不良处理等在 PCBA/Assembly 维度下的最细页面。
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Entities.Identity;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Enums;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.EntitySeedData;

/// <summary>
/// Takt 五级菜单种子数据。
/// <para>
/// 父级通常来自 <see cref="TaktMenuLevel4SeedData"/> 中 OUTPUT/不良 下的 PCBA、Assembly 目录节点。
/// 由 TaktMenuSeedData 统一协调调用，不直接注册为 ITaktSeedDataCoordinator。
/// </para>
/// </summary>
public class TaktMenuLevel5SeedData
{
    /// <summary>
    /// 初始化五级菜单种子数据。
    /// <para>
    /// 分别在各 PCBA/Assembly 父节点下写入生产 OPH、改修、返工、EPP 以及不良相关的检验与处理页面等。
    /// </para>
    /// </summary>
    /// <param name="serviceProvider">服务提供者，用于解析 <see cref="ITaktRepository{TaktMenu}"/>。</param>
    /// <param name="specifiedTenantCode">租户编码（由协调器传入）。</param>
    /// <returns>元组：(InsertCount, UpdateCount)，分别为本次新增与更新的五级菜单条数。</returns>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(IServiceProvider serviceProvider, string? specifiedTenantCode = null)
    {
        var menuRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktMenu>>();
        var sqlSugarContext = serviceProvider.GetRequiredService<TaktSeedContext>();

        // 五级菜单:基于四级菜单的ParentId
        // 注意:菜单为租户级实体,由协调器指定租户,必须传入租户编码
        if (string.IsNullOrWhiteSpace(specifiedTenantCode))
        {
            TaktLogger.Warning("未指定租户编码,跳过五级菜单种子数据初始化");
            return (0, 0);
        }
        
        var tenantCode = specifiedTenantCode;

        int insertCount = 0;
        int updateCount = 0;

        // 获取四级父菜单(使用仓储查询,自动应用租户过滤)
        // 注意:四级菜单已在 TaktMenuLevel4SeedData 中初始化
        var manufacturingOutputPcbaMenu = await menuRepository.FirstAsync(m => m.MenuCode == "MANUFACTURING_OUTPUT_PCBA");
        var manufacturingOutputAssemblyMenu = await menuRepository.FirstAsync(m => m.MenuCode == "MANUFACTURING_OUTPUT_ASSEMBLY");
        var manufacturingDefectPcbaMenu = await menuRepository.FirstAsync(m => m.MenuCode == "MANUFACTURING_DEFECT_PCBA");
        var manufacturingDefectAssemblyMenu = await menuRepository.FirstAsync(m => m.MenuCode == "MANUFACTURING_DEFECT_ASSEMBLY");

        // ========== OPH / PCBA 下的五级菜单 ==========
        if (manufacturingOutputPcbaMenu != null)
        {
            var (insert1, update1) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "MANUFACTURING_OUTPUT_PCBA_PRODUCTION", menu =>
            {
                menu.MenuName = "PCBA日报";
                menu.MenuCode = "MANUFACTURING_OUTPUT_PCBA_PRODUCTION";
                menu.I18nKey = "menu.logistics.manufacturing.output.pcba.production";
                menu.Icon = "RiFlashlightLine";
                menu.ParentId = manufacturingOutputPcbaMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "manufacturing:output:pcba:production:list";
                menu.RoutePath = "/manufacturing/output/pcba/production";
                menu.ComponentPath = "manufacturing/output/pcba/production/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insert1;
            updateCount += update1;

            var (insert2, update2) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "MANUFACTURING_OUTPUT_PCBA_REPAIR", menu =>
            {
                menu.MenuName = "PCBA改修";
                menu.MenuCode = "MANUFACTURING_OUTPUT_PCBA_REPAIR";
                menu.I18nKey = "menu.logistics.manufacturing.output.pcba.repair";
                menu.Icon = "RiSettings5Line";
                menu.ParentId = manufacturingOutputPcbaMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "manufacturing:output:pcba:repair:list";
                menu.RoutePath = "/manufacturing/output/pcba/repair";
                menu.ComponentPath = "manufacturing/output/pcba/repair/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insert2;
            updateCount += update2;

            var (insert3, update3) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "MANUFACTURING_OUTPUT_PCBA_REWORK", menu =>
            {
                menu.MenuName = "PCBA返工";
                menu.MenuCode = "MANUFACTURING_OUTPUT_PCBA_REWORK";
                menu.I18nKey = "menu.logistics.manufacturing.output.pcba.rework";
                menu.Icon = "RiRefreshLine";
                menu.ParentId = manufacturingOutputPcbaMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "manufacturing:output:pcba:rework:list";
                menu.RoutePath = "/manufacturing/output/pcba/rework";
                menu.ComponentPath = "manufacturing/output/pcba/rework/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insert3;
            updateCount += update3;

            var (insert4, update4) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "MANUFACTURING_OUTPUT_PCBA_EPP", menu =>
            {
                menu.MenuName = "PCBA EPP";
                menu.MenuCode = "MANUFACTURING_OUTPUT_PCBA_EPP";
                menu.I18nKey = "menu.logistics.manufacturing.output.pcba.epp";
                menu.Icon = "RiCodeSSlashLine";
                menu.ParentId = manufacturingOutputPcbaMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "manufacturing:output:pcba:epp:list";
                menu.RoutePath = "/manufacturing/output/pcba/epp";
                menu.ComponentPath = "manufacturing/output/pcba/epp/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insert4;
            updateCount += update4;
        }

        // ========== OPH / Assembly 下的五级菜单 ==========
        if (manufacturingOutputAssemblyMenu != null)
        {
            var (insert5, update5) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "MANUFACTURING_OUTPUT_ASSEMBLY_PRODUCTION", menu =>
            {
                menu.MenuName = "组立日报";
                menu.MenuCode = "MANUFACTURING_OUTPUT_ASSEMBLY_PRODUCTION";
                menu.I18nKey = "menu.logistics.manufacturing.output.assembly.production";
                menu.Icon = "RiSpeedLine";
                menu.ParentId = manufacturingOutputAssemblyMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "manufacturing:output:assembly:production:list";
                menu.RoutePath = "/manufacturing/output/assembly/production";
                menu.ComponentPath = "manufacturing/output/assembly/production/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insert5;
            updateCount += update5;

            var (insert6, update6) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "MANUFACTURING_OUTPUT_ASSEMBLY_REPAIR", menu =>
            {
                menu.MenuName = "组立改修";
                menu.MenuCode = "MANUFACTURING_OUTPUT_ASSEMBLY_REPAIR";
                menu.I18nKey = "menu.logistics.manufacturing.output.assembly.repair";
                menu.Icon = "RiSettings4Line";
                menu.ParentId = manufacturingOutputAssemblyMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "manufacturing:output:assembly:repair:list";
                menu.RoutePath = "/manufacturing/output/assembly/repair";
                menu.ComponentPath = "manufacturing/output/assembly/repair/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insert6;
            updateCount += update6;

            var (insert7, update7) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "MANUFACTURING_OUTPUT_ASSEMBLY_REWORK", menu =>
            {
                menu.MenuName = "组立返工";
                menu.MenuCode = "MANUFACTURING_OUTPUT_ASSEMBLY_REWORK";
                menu.I18nKey = "menu.logistics.manufacturing.output.assembly.rework";
                menu.Icon = "RiRestartLine";
                menu.ParentId = manufacturingOutputAssemblyMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "manufacturing:output:assembly:rework:list";
                menu.RoutePath = "/manufacturing/output/assembly/rework";
                menu.ComponentPath = "manufacturing/output/assembly/rework/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insert7;
            updateCount += update7;

            var (insert8, update8) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "MANUFACTURING_OUTPUT_ASSEMBLY_EPP", menu =>
            {
                menu.MenuName = "组立EPP";
                menu.MenuCode = "MANUFACTURING_OUTPUT_ASSEMBLY_EPP";
                menu.I18nKey = "menu.logistics.manufacturing.output.assembly.epp";
                menu.Icon = "RiBracesLine";
                menu.ParentId = manufacturingOutputAssemblyMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "manufacturing:output:assembly:epp:list";
                menu.RoutePath = "/manufacturing/output/assembly/epp";
                menu.ComponentPath = "manufacturing/output/assembly/epp/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insert8;
            updateCount += update8;
        }

        // ========== 不良 / PCBA 下的五级菜单 ==========
        if (manufacturingDefectPcbaMenu != null)
        {
            var (insert9, update9) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "MANUFACTURING_DEFECT_PCBA_SMT", menu =>
            {
                menu.MenuName = "SMT检查";
                menu.MenuCode = "MANUFACTURING_DEFECT_PCBA_SMT";
                menu.I18nKey = "menu.logistics.manufacturing.defect.pcba.smt";
                menu.Icon = "RiSearchLine";
                menu.ParentId = manufacturingDefectPcbaMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "manufacturing:defect:pcba:smt:list";
                menu.RoutePath = "/manufacturing/defect/pcba/smt";
                menu.ComponentPath = "manufacturing/defect/pcba/smt/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insert9;
            updateCount += update9;

            var (insert10, update10) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "MANUFACTURING_DEFECT_PCBA_REPAIR", menu =>
            {
                menu.MenuName = "PCBA修理";
                menu.MenuCode = "MANUFACTURING_DEFECT_PCBA_REPAIR";
                menu.I18nKey = "menu.logistics.manufacturing.defect.pcba.repair";
                menu.Icon = "RiToolsLine";
                menu.ParentId = manufacturingDefectPcbaMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "manufacturing:defect:pcba:repair:list";
                menu.RoutePath = "/manufacturing/defect/pcba/repair";
                menu.ComponentPath = "manufacturing/defect/pcba/repair/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insert10;
            updateCount += update10;
        }

        // ========== 不良 / Assembly 下的五级菜单 ==========
        if (manufacturingDefectAssemblyMenu != null)
        {
            var (insert11, update11) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "MANUFACTURING_DEFECT_ASSEMBLY_PRODUCTION", menu =>
            {
                menu.MenuName = "组立生产不良";
                menu.MenuCode = "MANUFACTURING_DEFECT_ASSEMBLY_PRODUCTION";
                menu.I18nKey = "menu.logistics.manufacturing.defect.assembly.production";
                menu.Icon = "RiSpeedLine";
                menu.ParentId = manufacturingDefectAssemblyMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "manufacturing:defect:assembly:production:list";
                menu.RoutePath = "/manufacturing/defect/assembly/production";
                menu.ComponentPath = "manufacturing/defect/assembly/production/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insert11;
            updateCount += update11;

            var (insert12, update12) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "MANUFACTURING_DEFECT_ASSEMBLY_REPAIR", menu =>
            {
                menu.MenuName = "组立改修不良";
                menu.MenuCode = "MANUFACTURING_DEFECT_ASSEMBLY_REPAIR";
                menu.I18nKey = "menu.logistics.manufacturing.defect.assembly.repair";
                menu.Icon = "RiSettings4Line";
                menu.ParentId = manufacturingDefectAssemblyMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "manufacturing:defect:assembly:repair:list";
                menu.RoutePath = "/manufacturing/defect/assembly/repair";
                menu.ComponentPath = "manufacturing/defect/assembly/repair/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insert12;
            updateCount += update12;

            var (insert13, update13) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "MANUFACTURING_DEFECT_ASSEMBLY_REWORK", menu =>
            {
                menu.MenuName = "组立返工不良";
                menu.MenuCode = "MANUFACTURING_DEFECT_ASSEMBLY_REWORK";
                menu.I18nKey = "menu.logistics.manufacturing.defect.assembly.rework";
                menu.Icon = "RiRestartLine";
                menu.ParentId = manufacturingDefectAssemblyMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "manufacturing:defect:assembly:rework:list";
                menu.RoutePath = "/manufacturing/defect/assembly/rework";
                menu.ComponentPath = "manufacturing/defect/assembly/rework/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insert13;
            updateCount += update13;

            var (insert14, update14) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "MANUFACTURING_DEFECT_ASSEMBLY_EPP", menu =>
            {
                menu.MenuName = "组立EPP不良";
                menu.MenuCode = "MANUFACTURING_DEFECT_ASSEMBLY_EPP";
                menu.I18nKey = "menu.logistics.manufacturing.defect.assembly.epp";
                menu.Icon = "RiBracesLine";
                menu.ParentId = manufacturingDefectAssemblyMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "manufacturing:defect:assembly:epp:list";
                menu.RoutePath = "/manufacturing/defect/assembly/epp";
                menu.ComponentPath = "manufacturing/defect/assembly/epp/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insert14;
            updateCount += update14;
        }

        return (insertCount, updateCount);
    }

    /// <summary>
    /// 创建或更新菜单。
    /// </summary>
    /// <param name="menuRepository">菜单仓储。</param>
    /// <param name="sqlSugarContext">SqlSugar上下文。</param>
    /// <param name="tenantCode">租户编码。</param>
    /// <param name="menuCode">菜单编码(业务键)。</param>
    /// <param name="configure">菜单配置委托。</param>
    /// <returns>元组:(InsertCount, UpdateCount),本条菜单新增或更新条数(0或1)。</returns>
    private static async Task<(int InsertCount, int UpdateCount)> CreateOrUpdateMenuAsync(
        ITaktTenantSeedRepository<TaktMenu> menuRepository,
        TaktSeedContext sqlSugarContext,
        string tenantCode,
        string menuCode,
        Action<TaktMenu> configure)
    {
        // 注意：种子数据必须使用仓储查询（带租户过滤），确保数据隔离
        var menu = await menuRepository.FirstAsync(m => m.MenuCode == menuCode);
        
        if (menu == null)
        {
            menu = new TaktMenu();
            
            // 必须先设置 TenantCode（租户级实体）
            menu.TenantCode = tenantCode;
            
            configure(menu);
            menu.IsBuiltIn = TaktYesNo.Yes;
            
            // 自动计算 Level（根节点为 1）
            menu.Level = menu.ParentId > 0 ? 0 : 1; // 稍后根据父级计算
            menu.IsLeaf = 1; // 默认为叶子节点，后续创建子菜单时会自动更新
            
            menu.CreatedBy = 900001;
            menu.CreatedAt = DateTime.Now;
            
            menu = await menuRepository.CreateAsync(menu);
            
            // 更新 MenuPath 和 Level
            if (menu.ParentId > 0)
            {
                // 使用仓储查询父级（带租户过滤）
                var parentMenu = await menuRepository.FirstAsync(m => m.Id == menu.ParentId);
                if (parentMenu != null)
                {
                    menu.MenuPath = $"{parentMenu.MenuPath}{menu.Id}/";
                    menu.Level = parentMenu.Level + 1;
                    
                    // 更新父级 IsLeaf 为非叶子
                    if (parentMenu.IsLeaf == 1)
                    {
                        parentMenu.IsLeaf = 0;
                        parentMenu.UpdatedAt = DateTime.Now;
                        await menuRepository.UpdateAsync(parentMenu);
                    }
                }
            }
            else
            {
                menu.MenuPath = $"/{menu.Id}/";
                menu.Level = 1;
            }
            
            menu.UpdatedAt = DateTime.Now;
            await menuRepository.UpdateAsync(menu);
            return (1, 0);
        }
        else
        {
            configure(menu);
            menu.IsBuiltIn = TaktYesNo.Yes;
            
            // 重新计算 Level 和 MenuPath（如果 ParentId 发生变化）
            if (menu.ParentId > 0)
            {
                // 使用仓储查询父级（带租户过滤）
                var parentMenu = await menuRepository.FirstAsync(m => m.Id == menu.ParentId);
                if (parentMenu != null)
                {
                    menu.MenuPath = $"{parentMenu.MenuPath}{menu.Id}/";
                    menu.Level = parentMenu.Level + 1;
                    
                    // 更新父级 IsLeaf 为非叶子
                    if (parentMenu.IsLeaf == 1)
                    {
                        parentMenu.IsLeaf = 0;
                        parentMenu.UpdatedAt = DateTime.Now;
                        await menuRepository.UpdateAsync(parentMenu);
                    }
                }
            }
            else
            {
                menu.MenuPath = $"/{menu.Id}/";
                menu.Level = 1;
            }
            
            menu.UpdatedAt = DateTime.Now;
            menu.UpdatedBy = 900001;
            
            await menuRepository.UpdateAsync(menu);
            return (0, 1);
        }
    }
}
