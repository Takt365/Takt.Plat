// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds
// 文件名称：TaktMenuLevel3SeedData.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt 三级菜单种子数据。
//           在二级菜单（TaktMenuLevel2SeedData）已存在的前提下，按父级 MenuCode 挂载更细粒度的页面或目录：
//           含日常业务页、基础任务页、财务/物料/销售/生产/质量/人力各子模块等。
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
using Takt.Infrastructure.Data.Context;

namespace Takt.Infrastructure.Data.Seeds.EntitySeedData;

/// <summary>
/// Takt 三级菜单种子数据。
/// <para>
/// 父级为 <see cref="TaktMenuLevel2SeedData"/> 中定义的二级目录或分组（如 ROUTINE_NEWS_CENTER、LOGISTICS_SALES、HUMANRESOURCE_TALENT 等）。
/// 页面类型菜单需配置以 <c>:list</c> 结尾的权限串，供 <see cref="TaktMenuButtonSeedData"/> 生成按钮。
/// 由 TaktMenuSeedData 统一协调调用，不直接注册为 ITaktSeedDataCoordinator。
/// </para>
/// </summary>
public class TaktMenuLevel3SeedData
{
    /// <summary>
    /// 初始化三级菜单种子数据。
    /// <para>
    /// 分块写入：日常业务、基础任务、财务会计/管理会计、物料与采购、销售、生产制造、质量、
    /// 人力资源（组织/人才/人事/考勤/薪酬/绩效/培训）及统计日志/报表/看板等；不存在则创建，存在则更新。
    /// </para>
    /// </summary>
    /// <param name="serviceProvider">服务提供者，用于解析 <see cref="ITaktRepository{TaktMenu}"/>。</param>
    /// <param name="specifiedTenantCode">租户编码（由协调器传入）。</param>
    /// <returns>元组：(InsertCount, UpdateCount)，分别为本次新增与更新的三级菜单条数。</returns>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(IServiceProvider serviceProvider, string? specifiedTenantCode = null)
    {
        var menuRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktMenu>>();
        var seedContext = serviceProvider.GetRequiredService<TaktSeedContext>();

        // 三级菜单:基于二级菜单的ParentId
        // 注意:菜单为租户级实体,由协调器指定租户,必须传入租户编码
        if (string.IsNullOrWhiteSpace(specifiedTenantCode))
        {
            TaktLogger.Warning("未指定租户编码,跳过三级菜单种子数据初始化");
            return (0, 0);
        }
        
        var tenantCode = specifiedTenantCode;

        int insertCount = 0;
        int updateCount = 0;

        // 获取二级父菜单(注意：必须绕过仓储的租户过滤，直接查询)
        // 严格按照二级菜单 SortOrder 顺序排列
        var accountingFinancialMenu = await seedContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "ACCOUNTING_FINANCIAL" && m.IsDeleted == 0)
            .FirstAsync();
        var accountingControllingMenu = await seedContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "ACCOUNTING_CONTROLLING" && m.IsDeleted == 0)
            .FirstAsync();
        var logisticsMaterialMenu = await seedContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "LOGISTICS_MATERIAL" && m.IsDeleted == 0)
            .FirstAsync();
        var manufacturingMenu = await seedContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "MANUFACTURING" && m.IsDeleted == 0)
            .FirstAsync();
        var logisticsQualityMenu = await seedContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "LOGISTICS_QUALITY" && m.IsDeleted == 0)
            .FirstAsync();
        var logisticsServiceMenu = await seedContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "LOGISTICS_SERVICE" && m.IsDeleted == 0)
            .FirstAsync();
        var logisticsMaintenanceMenu = await seedContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "LOGISTICS_MAINTENANCE" && m.IsDeleted == 0)
            .FirstAsync();
        var logisticsSalesMenu = await seedContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "LOGISTICS_SALES" && m.IsDeleted == 0)
            .FirstAsync();
        var hrOrganizationMenu = await seedContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "HUMANRESOURCE_ORGANIZATION" && m.IsDeleted == 0)
            .FirstAsync();
        var hrPersonnelMenu = await seedContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "HUMANRESOURCE_PERSONNEL" && m.IsDeleted == 0)
            .FirstAsync();
        var hrAttendanceLeaveMenu = await seedContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "HUMANRESOURCE_ATTENDANCE_LEAVE" && m.IsDeleted == 0)
            .FirstAsync();
        var hrCompensationBenefitsMenu = await seedContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "HUMANRESOURCE_COMPENSATION_BENEFITS" && m.IsDeleted == 0)
            .FirstAsync();
        var hrPerformanceMenu = await seedContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "HUMANRESOURCE_PERFORMANCE" && m.IsDeleted == 0)
            .FirstAsync();
        var hrTrainingDevelopmentMenu = await seedContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "HUMANRESOURCE_TRAINING_DEVELOPMENT" && m.IsDeleted == 0)
            .FirstAsync();
        var hrTalentMenu = await seedContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "HUMANRESOURCE_TALENT" && m.IsDeleted == 0)
            .FirstAsync();
        var statisticsReportMenu = await seedContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "STATISTICS_REPORT" && m.IsDeleted == 0)
            .FirstAsync();
        var statisticsLoggingMenu = await seedContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "STATISTICS_LOGGING" && m.IsDeleted == 0)
            .FirstAsync();

        // ========== 管理会计下的三级菜单 (ACCOUNTING_FINANCIAL) ==========
        if (accountingFinancialMenu != null)
        {
            var (insertAF1, updateAF1) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "ACCOUNTING_FINANCIAL_ACCOUNT_TITLE", menu =>
            {
                menu.MenuName = "会计科目";
                menu.MenuCode = "ACCOUNTING_FINANCIAL_ACCOUNT_TITLE";
                menu.I18nKey = "menu.accounting.financial.accounttitle";
                menu.Icon = "RiBookletLine";
                menu.ParentId = accountingFinancialMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "accounting:financial:accounttitle:list";
                menu.RoutePath = "/accounting/financial/account-title";
                menu.ComponentPath = "accounting/financial/account-title/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertAF1;
            updateCount += updateAF1;

            var (insertAF2, updateAF2) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "ACCOUNTING_FINANCIAL_ASSET", menu =>
            {
                menu.MenuName = "固定资产";
                menu.MenuCode = "ACCOUNTING_FINANCIAL_ASSET";
                menu.I18nKey = "menu.accounting.financial.asset";
                menu.Icon = "RiBuilding2Line";
                menu.ParentId = accountingFinancialMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "accounting:financial:asset:list";
                menu.RoutePath = "/accounting/financial/asset";
                menu.ComponentPath = "accounting/financial/asset/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertAF2;
            updateCount += updateAF2;

            var (insertAF3, updateAF3) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "ACCOUNTING_FINANCIAL_COUNTERSIGN", menu =>
            {
                menu.MenuName = "会签管理";
                menu.MenuCode = "ACCOUNTING_FINANCIAL_COUNTERSIGN";
                menu.I18nKey = "menu.accounting.financial.countersign";
                menu.Icon = "RiFileCheckLine";
                menu.ParentId = accountingFinancialMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "accounting:financial:countersign:list";
                menu.RoutePath = "/accounting/financial/countersign";
                menu.ComponentPath = "accounting/financial/countersign/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertAF3;
            updateCount += updateAF3;

            var (insertAF4, updateAF4) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "ACCOUNTING_FINANCIAL_COMPANY", menu =>
            {
                menu.MenuName = "公司管理";
                menu.MenuCode = "ACCOUNTING_FINANCIAL_COMPANY";
                menu.I18nKey = "menu.accounting.financial.company";
                menu.Icon = "RiBuildingLine";
                menu.ParentId = accountingFinancialMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "accounting:financial:company:list";
                menu.RoutePath = "/accounting/financial/company";
                menu.ComponentPath = "accounting/financial/company/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertAF4;
            updateCount += updateAF4;
        }

        // ========== 控制会计下的三级菜单 (ACCOUNTING_CONTROLLING) ==========
        if (accountingControllingMenu != null)
        {
            var (insertAC1, updateAC1) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "ACCOUNTING_CONTROLLING_PROFIT_CENTER", menu =>
            {
                menu.MenuName = "利润中心";
                menu.MenuCode = "ACCOUNTING_CONTROLLING_PROFIT_CENTER";
                menu.I18nKey = "menu.accounting.controlling.profitcenter";
                menu.Icon = "RiMoneyDollarCircleLine";
                menu.ParentId = accountingControllingMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "accounting:controlling:profitcenter:list";
                menu.RoutePath = "/accounting/controlling/profit-center";
                menu.ComponentPath = "accounting/controlling/profit-center/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertAC1;
            updateCount += updateAC1;

            var (insertAC2, updateAC2) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "ACCOUNTING_CONTROLLING_COST_CENTER", menu =>
            {
                menu.MenuName = "成本中心";
                menu.MenuCode = "ACCOUNTING_CONTROLLING_COST_CENTER";
                menu.I18nKey = "menu.accounting.controlling.costcenter";
                menu.Icon = "RiPieChart2Line";
                menu.ParentId = accountingControllingMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "accounting:controlling:costcenter:list";
                menu.RoutePath = "/accounting/controlling/cost-center";
                menu.ComponentPath = "accounting/controlling/cost-center/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertAC2;
            updateCount += updateAC2;

            var (insertAC3, updateAC3) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "ACCOUNTING_CONTROLLING_COST_ELEMENT", menu =>
            {
                menu.MenuName = "成本要素";
                menu.MenuCode = "ACCOUNTING_CONTROLLING_COST_ELEMENT";
                menu.I18nKey = "menu.accounting.controlling.costelement";
                menu.Icon = "RiListCheck";
                menu.ParentId = accountingControllingMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "accounting:controlling:costelement:list";
                menu.RoutePath = "/accounting/controlling/cost-element";
                menu.ComponentPath = "accounting/controlling/cost-element/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertAC3;
            updateCount += updateAC3;

            var (insertAC4, updateAC4) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "ACCOUNTING_CONTROLLING_WAGE_RATE", menu =>
            {
                menu.MenuName = "工资率";
                menu.MenuCode = "ACCOUNTING_CONTROLLING_WAGE_RATE";
                menu.I18nKey = "menu.accounting.controlling.wagerate";
                menu.Icon = "RiCalculatorLine";
                menu.ParentId = accountingControllingMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "accounting:controlling:wagerate:list";
                menu.RoutePath = "/accounting/controlling/wage-rate";
                menu.ComponentPath = "accounting/controlling/wage-rate/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertAC4;
            updateCount += updateAC4;
        }

        // ========== 物料管理下的三级菜单 (LOGISTICS_MATERIAL) ==========
        if (logisticsMaterialMenu != null)
        {
            var (insertLM1, updateLM1) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_MATERIAL_PLANT", menu =>
            {
                menu.MenuName = "工厂信息";
                menu.MenuCode = "LOGISTICS_MATERIAL_PLANT";
                menu.I18nKey = "menu.logistics.materials.plant";
                menu.Icon = "RiBuilding2Line";
                menu.ParentId = logisticsMaterialMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:materials:plant:list";
                menu.RoutePath = "/logistics/materials/plant";
                menu.ComponentPath = "logistics/materials/plant/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLM1;
            updateCount += updateLM1;

            var (insertLM2, updateLM2) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_MATERIAL_MATERIAL", menu =>
            {
                menu.MenuName = "物料";
                menu.MenuCode = "LOGISTICS_MATERIAL_MATERIAL";
                menu.I18nKey = "menu.logistics.materials.material";
                menu.Icon = "RiArchiveStackLine";
                menu.ParentId = logisticsMaterialMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:materials:material:list";
                menu.RoutePath = "/logistics/materials/material";
                menu.ComponentPath = "logistics/materials/material/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLM2;
            updateCount += updateLM2;

            var (insertLM3, updateLM3) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_MATERIAL_PURCHASING", menu =>
            {
                menu.MenuName = "采购管理";
                menu.MenuCode = "LOGISTICS_MATERIAL_PURCHASING";
                menu.I18nKey = "menu.logistics.materials.purchasing._self";
                menu.Icon = "RiShoppingBagLine";
                menu.ParentId = logisticsMaterialMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:materials:purchasing:list";
                menu.RoutePath = "/logistics/materials/purchasing";
                menu.ComponentPath = "logistics/materials/purchasing/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLM3;
            updateCount += updateLM3;
        }

        // ========== 生产执行下的三级菜单 (MANUFACTURING) ==========
        if (manufacturingMenu != null)
        {
            var (insertMFG1, updateMFG1) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "MANUFACTURING_BOM", menu =>
            {
                menu.MenuName = "BOM管理";
                menu.MenuCode = "MANUFACTURING_BOM";
                menu.I18nKey = "menu.logistics.manufacturing.bom._self";
                menu.Icon = "RiTreeLine";
                menu.ParentId = manufacturingMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:bom:list";
                menu.RoutePath = "/logistics/manufacturing/bom";
                menu.ComponentPath = "logistics/manufacturing/bom/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertMFG1;
            updateCount += updateMFG1;

            var (insertMFG2, updateMFG2) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "MANUFACTURING_WORK_ORDER", menu =>
            {
                menu.MenuName = "工单管理";
                menu.MenuCode = "MANUFACTURING_WORK_ORDER";
                menu.I18nKey = "menu.logistics.manufacturing.workorder";
                menu.Icon = "RiFileList3Line";
                menu.ParentId = manufacturingMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:workorder:list";
                menu.RoutePath = "/logistics/manufacturing/work-order";
                menu.ComponentPath = "logistics/manufacturing/work-order/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertMFG2;
            updateCount += updateMFG2;

            var (insertMFG3, updateMFG3) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "MANUFACTURING_SCHEDULING", menu =>
            {
                menu.MenuName = "生产排程";
                menu.MenuCode = "MANUFACTURING_SCHEDULING";
                menu.I18nKey = "menu.logistics.manufacturing.scheduling._self";
                menu.Icon = "RiCalendarScheduleLine";
                menu.ParentId = manufacturingMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:scheduling:list";
                menu.RoutePath = "/logistics/manufacturing/scheduling";
                menu.ComponentPath = "logistics/manufacturing/scheduling/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertMFG3;
            updateCount += updateMFG3;

            var (insertMFG4, updateMFG4) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "MANUFACTURING_ec", menu =>
            {
                menu.MenuName = "设变";
                menu.MenuCode = "MANUFACTURING_ec";
                menu.I18nKey = "menu.logistics.manufacturing.ecn._self";
                menu.Icon = "RiEditCircleLine";
                menu.ParentId = manufacturingMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:ecn:list";
                menu.RoutePath = "/logistics/manufacturing/ecn";
                menu.ComponentPath = "logistics/manufacturing/ecn/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertMFG4;
            updateCount += updateMFG4;

            var (insertMFG5, updateMFG5) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "MANUFACTURING_OUTPUT", menu =>
            {
                menu.MenuName = "OPH管理";
                menu.MenuCode = "MANUFACTURING_OUTPUT";
                menu.I18nKey = "menu.logistics.manufacturing.output._self";
                menu.Icon = "RiBarChart2Line";
                menu.ParentId = manufacturingMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:output:list";
                menu.RoutePath = "/logistics/manufacturing/output";
                menu.ComponentPath = "logistics/manufacturing/output/index";
                menu.SortOrder = 5;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertMFG5;
            updateCount += updateMFG5;

            var (insertMFG6, updateMFG6) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "MANUFACTURING_DEFECT", menu =>
            {
                menu.MenuName = "不良";
                menu.MenuCode = "MANUFACTURING_DEFECT";
                menu.I18nKey = "menu.logistics.manufacturing.defect._self";
                menu.Icon = "RiErrorWarningLine";
                menu.ParentId = manufacturingMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:defect:list";
                menu.RoutePath = "/logistics/manufacturing/defect";
                menu.ComponentPath = "logistics/manufacturing/defect/index";
                menu.SortOrder = 6;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertMFG6;
            updateCount += updateMFG6;
        }

        // ========== 质量管理下的三级菜单 (LOGISTICS_QUALITY) ==========
        if (logisticsQualityMenu != null)
        {
            var (insertLQ1, updateLQ1) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_QUALITY_COST", menu =>
            {
                menu.MenuName = "品质成本";
                menu.MenuCode = "LOGISTICS_QUALITY_COST";
                menu.I18nKey = "menu.logistics.quality.cost._self";
                menu.Icon = "RiMoneyCnyCircleLine";
                menu.ParentId = logisticsQualityMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:quality:cost:list";
                menu.RoutePath = "/logistics/quality/cost";
                menu.ComponentPath = "logistics/quality/cost/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLQ1;
            updateCount += updateLQ1;

            var (insertLQ2, updateLQ2) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_QUALITY_OPERATION", menu =>
            {
                menu.MenuName = "质量业务";
                menu.MenuCode = "LOGISTICS_QUALITY_OPERATION";
                menu.I18nKey = "menu.logistics.quality.operation._self";
                menu.Icon = "RiShieldCheckLine";
                menu.ParentId = logisticsQualityMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:quality:operation:list";
                menu.RoutePath = "/logistics/quality/operation";
                menu.ComponentPath = "logistics/quality/operation/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLQ2;
            updateCount += updateLQ2;
        }

        // ========== 客户服务下的三级菜单 (LOGISTICS_SERVICE) ==========
        if (logisticsServiceMenu != null)
        {
            var (insertLS1, updateLS1) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_SERVICE_COMPLAINT", menu =>
            {
                menu.MenuName = "客诉管理";
                menu.MenuCode = "LOGISTICS_SERVICE_COMPLAINT";
                menu.I18nKey = "menu.logistics.service.complaint";
                menu.Icon = "RiMessage3Line";
                menu.ParentId = logisticsServiceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:service:complaint:list";
                menu.RoutePath = "/logistics/service/complaint";
                menu.ComponentPath = "logistics/service/complaint/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLS1;
            updateCount += updateLS1;
        }

        // ========== 工厂维护下的三级菜单 (LOGISTICS_MAINTENANCE) ==========
        if (logisticsMaintenanceMenu != null)
        {
            var (insertLM1, updateLM1) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_MAINTENANCE_REPAIR", menu =>
            {
                menu.MenuName = "维修管理";
                menu.MenuCode = "LOGISTICS_MAINTENANCE_REPAIR";
                menu.I18nKey = "menu.logistics.maintenance.repair";
                menu.Icon = "RiToolsLine";
                menu.ParentId = logisticsMaintenanceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:maintenance:repair:list";
                menu.RoutePath = "/logistics/maintenance/repair";
                menu.ComponentPath = "logistics/maintenance/repair/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLM1;
            updateCount += updateLM1;
        }

        // ========== 销售管理下的三级菜单 (LOGISTICS_SALES) ==========
        if (logisticsSalesMenu != null)
        {
            var (insertLS1, updateLS1) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_SALES_CUSTOMER", menu =>
            {
                menu.MenuName = "客户信息";
                menu.MenuCode = "LOGISTICS_SALES_CUSTOMER";
                menu.I18nKey = "menu.logistics.sales.customer";
                menu.Icon = "RiBuilding2Line";
                menu.ParentId = logisticsSalesMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:sales:customer:list";
                menu.RoutePath = "/logistics/sales/customer";
                menu.ComponentPath = "logistics/sales/customer/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLS1;
            updateCount += updateLS1;

            var (insertLS2, updateLS2) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_SALES_CLIENT", menu =>
            {
                menu.MenuName = "顾客信息";
                menu.MenuCode = "LOGISTICS_SALES_CLIENT";
                menu.I18nKey = "menu.logistics.sales.client";
                menu.Icon = "RiUserBusinessLine";
                menu.ParentId = logisticsSalesMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:sales:client:list";
                menu.RoutePath = "/logistics/sales/client";
                menu.ComponentPath = "logistics/sales/client/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLS2;
            updateCount += updateLS2;

            var (insertLS3, updateLS3) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_SALES_QUOTATION", menu =>
            {
                menu.MenuName = "销售报价";
                menu.MenuCode = "LOGISTICS_SALES_QUOTATION";
                menu.I18nKey = "menu.logistics.sales.quotation";
                menu.Icon = "RiFileList3Line";
                menu.ParentId = logisticsSalesMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:sales:quotation:list";
                menu.RoutePath = "/logistics/sales/quotation";
                menu.ComponentPath = "logistics/sales/quotation/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLS3;
            updateCount += updateLS3;

            var (insertLS4, updateLS4) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_SALES_PRICE", menu =>
            {
                menu.MenuName = "销售价格";
                menu.MenuCode = "LOGISTICS_SALES_PRICE";
                menu.I18nKey = "menu.logistics.sales.price";
                menu.Icon = "RiMoneyCnyCircleLine";
                menu.ParentId = logisticsSalesMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:sales:price:list";
                menu.RoutePath = "/logistics/sales/price";
                menu.ComponentPath = "logistics/sales/price/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLS4;
            updateCount += updateLS4;

            var (insertLS5, updateLS5) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_SALES_ORDER", menu =>
            {
                menu.MenuName = "销售订单";
                menu.MenuCode = "LOGISTICS_SALES_ORDER";
                menu.I18nKey = "menu.logistics.sales.order";
                menu.Icon = "RiShoppingCart2Line";
                menu.ParentId = logisticsSalesMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:sales:order:list";
                menu.RoutePath = "/logistics/sales/order";
                menu.ComponentPath = "logistics/sales/order/index";
                menu.SortOrder = 5;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLS5;
            updateCount += updateLS5;

            var (insertLS6, updateLS6) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_SALES_INVOICE", menu =>
            {
                menu.MenuName = "销售发票";
                menu.MenuCode = "LOGISTICS_SALES_INVOICE";
                menu.I18nKey = "menu.logistics.sales.invoice";
                menu.Icon = "RiBillLine";
                menu.ParentId = logisticsSalesMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:sales:invoice:list";
                menu.RoutePath = "/logistics/sales/invoice";
                menu.ComponentPath = "logistics/sales/invoice/index";
                menu.SortOrder = 6;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLS6;
            updateCount += updateLS6;

            var (insertLS7, updateLS7) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_SALES_FORECAST", menu =>
            {
                menu.MenuName = "销售预测";
                menu.MenuCode = "LOGISTICS_SALES_FORECAST";
                menu.I18nKey = "menu.logistics.sales.forecast";
                menu.Icon = "RiLineChartLine";
                menu.ParentId = logisticsSalesMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:sales:forecast:list";
                menu.RoutePath = "/logistics/sales/forecast";
                menu.ComponentPath = "logistics/sales/forecast/index";
                menu.SortOrder = 7;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLS7;
            updateCount += updateLS7;
        }

        // ========== 组织管理下的三级菜单 (HUMANRESOURCE_ORGANIZATION) ==========
        if (hrOrganizationMenu != null)
        {
            var (insertHRO1, updateHRO1) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMANRESOURCE_ORGANIZATION_DEPT", menu =>
            {
                menu.MenuName = "部门管理";
                menu.MenuCode = "HUMANRESOURCE_ORGANIZATION_DEPT";
                menu.I18nKey = "menu.humanresource.organization.dept";
                menu.Icon = "RiGroupLine";
                menu.ParentId = hrOrganizationMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "humanresource:organization:dept:list";
                menu.RoutePath = "/human-resource/organization/dept";
                menu.ComponentPath = "human-resource/organization/dept/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRO1;
            updateCount += updateHRO1;

            var (insertHRO2, updateHRO2) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMANRESOURCE_ORGANIZATION_POST", menu =>
            {
                menu.MenuName = "岗位管理";
                menu.MenuCode = "HUMANRESOURCE_ORGANIZATION_POST";
                menu.I18nKey = "menu.humanresource.organization.post";
                menu.Icon = "RiAdminLine";
                menu.ParentId = hrOrganizationMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "humanresource:organization:post:list";
                menu.RoutePath = "/human-resource/organization/post";
                menu.ComponentPath = "human-resource/organization/post/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRO2;
            updateCount += updateHRO2;
        }

        // ========== 人事管理下的三级菜单 (HUMANRESOURCE_PERSONNEL) ==========
        if (hrPersonnelMenu != null)
        {
            var (insertHRP1, updateHRP1) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMANRESOURCE_EMPLOYEE", menu =>
            {
                menu.MenuName = "员工档案";
                menu.MenuCode = "HUMANRESOURCE_EMPLOYEE";
                menu.I18nKey = "menu.humanresource.personnel.employee";
                menu.Icon = "RiUserFollowLine";
                menu.ParentId = hrPersonnelMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "humanresource:personnel:employee:list";
                menu.RoutePath = "/human-resource/personnel/employee";
                menu.ComponentPath = "human-resource/personnel/employee/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRP1;
            updateCount += updateHRP1;

            var (insertHRP2, updateHRP2) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMANRESOURCE_EMPLOYEE_CONTRACT", menu =>
            {
                menu.MenuName = "员工合同";
                menu.MenuCode = "HUMANRESOURCE_EMPLOYEE_CONTRACT";
                menu.I18nKey = "menu.humanresource.personnel.employeecontract";
                menu.Icon = "RiFilePaperLine";
                menu.ParentId = hrPersonnelMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "humanresource:personnel:employeecontract:list";
                menu.RoutePath = "/human-resource/personnel/employee-contract";
                menu.ComponentPath = "human-resource/personnel/employee-contract/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRP2;
            updateCount += updateHRP2;

            var (insertHRP3, updateHRP3) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMANRESOURCE_EMPLOYEE_DELEGATE", menu =>
            {
                menu.MenuName = "员工代理";
                menu.MenuCode = "HUMANRESOURCE_EMPLOYEE_DELEGATE";
                menu.I18nKey = "menu.humanresource.personnel.employeedelegate";
                menu.Icon = "RiUserSharedLine";
                menu.ParentId = hrPersonnelMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "humanresource:personnel:employeedelegation:list";
                menu.RoutePath = "/human-resource/personnel/employee-delegate";
                menu.ComponentPath = "human-resource/personnel/employee-delegate/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRP3;
            updateCount += updateHRP3;

            var (insertHRP4, updateHRP4) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMANRESOURCE_EMPLOYEE_TRANSFER", menu =>
            {
                menu.MenuName = "员工调动";
                menu.MenuCode = "HUMANRESOURCE_EMPLOYEE_TRANSFER";
                menu.I18nKey = "menu.humanresource.personnel.employeetransfer";
                menu.Icon = "RiExchangeLine";
                menu.ParentId = hrPersonnelMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "humanresource:personnel:employeetransfer:list";
                menu.RoutePath = "/human-resource/personnel/employee-transfer";
                menu.ComponentPath = "human-resource/personnel/employee-transfer/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRP4;
            updateCount += updateHRP4;

            var (insertHRP5, updateHRP5) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMANRESOURCE_EMPLOYEE_ONBOARDING_TODO", menu =>
            {
                menu.MenuName = "入职待办";
                menu.MenuCode = "HUMANRESOURCE_EMPLOYEE_ONBOARDING_TODO";
                menu.I18nKey = "menu.humanresource.personnel.employeeonboardingtodo";
                menu.Icon = "RiClipboardLine";
                menu.ParentId = hrPersonnelMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "humanresource:personnel:employeeonboardingtodo:list";
                menu.RoutePath = "/human-resource/personnel/employee-onboarding-todo";
                menu.ComponentPath = "human-resource/personnel/employee-onboarding-todo/index";
                menu.SortOrder = 5;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRP5;
            updateCount += updateHRP5;
        }

        // ========== 考勤假期下的三级菜单 (HUMANRESOURCE_ATTENDANCE_LEAVE) ==========
        if (hrAttendanceLeaveMenu != null)
        {
            var (insertHRAL1, updateHRAL1) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMANRESOURCE_ATTENDANCE_LEAVE_HOLIDAY", menu =>
            {
                menu.MenuName = "假期管理";
                menu.MenuCode = "HUMANRESOURCE_ATTENDANCE_LEAVE_HOLIDAY";
                menu.I18nKey = "menu.humanresource.attendanceleave.holiday";
                menu.Icon = "RiCalendarEventLine";
                menu.ParentId = hrAttendanceLeaveMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "humanresource:attendanceleave:holiday:list";
                menu.RoutePath = "/human-resource/attendance-leave/holiday";
                menu.ComponentPath = "human-resource/attendance-leave/holiday/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRAL1;
            updateCount += updateHRAL1;

            var (insertHRAL2, updateHRAL2) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMANRESOURCE_ATTENDANCE_LEAVE_LEAVE", menu =>
            {
                menu.MenuName = "请假管理";
                menu.MenuCode = "HUMANRESOURCE_ATTENDANCE_LEAVE_LEAVE";
                menu.I18nKey = "menu.humanresource.attendanceleave.leave";
                menu.Icon = "RiCalendarCheckLine";
                menu.ParentId = hrAttendanceLeaveMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "humanresource:attendanceleave:leave:list";
                menu.RoutePath = "/human-resource/attendance-leave/leave";
                menu.ComponentPath = "human-resource/attendance-leave/leave/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRAL2;
            updateCount += updateHRAL2;

            var (insertHRAL3, updateHRAL3) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMANRESOURCE_ATTENDANCE_LEAVE_OVERTIME", menu =>
            {
                menu.MenuName = "加班管理";
                menu.MenuCode = "HUMANRESOURCE_ATTENDANCE_LEAVE_OVERTIME";
                menu.I18nKey = "menu.humanresource.attendanceleave.overtime";
                menu.Icon = "RiTimeLine";
                menu.ParentId = hrAttendanceLeaveMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "humanresource:attendanceleave:overtime:list";
                menu.RoutePath = "/human-resource/attendance-leave/overtime";
                menu.ComponentPath = "human-resource/attendance-leave/overtime/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRAL3;
            updateCount += updateHRAL3;

            var (insertHRAL4, updateHRAL4) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMANRESOURCE_ATTENDANCE_LEAVE_ATTENDANCE_CORRECTION", menu =>
            {
                menu.MenuName = "补卡管理";
                menu.MenuCode = "HUMANRESOURCE_ATTENDANCE_LEAVE_ATTENDANCE_CORRECTION";
                menu.I18nKey = "menu.humanresource.attendanceleave.attendancecorrection";
                menu.Icon = "RiEditLine";
                menu.ParentId = hrAttendanceLeaveMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "humanresource:attendanceleave:attendancecorrection:list";
                menu.RoutePath = "/human-resource/attendance-leave/attendance-correction";
                menu.ComponentPath = "human-resource/attendance-leave/attendance-correction/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRAL4;
            updateCount += updateHRAL4;

            var (insertHRAL5, updateHRAL5) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMANRESOURCE_ATTENDANCE_LEAVE_ATTENDANCE_SETTINGS", menu =>
            {
                menu.MenuName = "考勤设置";
                menu.MenuCode = "HUMANRESOURCE_ATTENDANCE_LEAVE_ATTENDANCE_SETTINGS";
                menu.I18nKey = "menu.humanresource.attendanceleave.attendancesettings";
                menu.Icon = "RiSettings3Line";
                menu.ParentId = hrAttendanceLeaveMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "humanresource:attendanceleave:attendancesettings:list";
                menu.RoutePath = "/human-resource/attendance-leave/attendance-settings";
                menu.ComponentPath = "human-resource/attendance-leave/attendance-settings/index";
                menu.SortOrder = 5;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRAL5;
            updateCount += updateHRAL5;

            var (insertHRAL6, updateHRAL6) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMANRESOURCE_ATTENDANCE_LEAVE_SCHEDULE", menu =>
            {
                menu.MenuName = "排班管理";
                menu.MenuCode = "HUMANRESOURCE_ATTENDANCE_LEAVE_SCHEDULE";
                menu.I18nKey = "menu.humanresource.attendanceleave.schedule";
                menu.Icon = "RiCalendarScheduleLine";
                menu.ParentId = hrAttendanceLeaveMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "humanresource:attendanceleave:schedule:list";
                menu.RoutePath = "/human-resource/attendance-leave/schedule";
                menu.ComponentPath = "human-resource/attendance-leave/schedule/index";
                menu.SortOrder = 6;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRAL6;
            updateCount += updateHRAL6;
        }

        // ========== 薪酬福利下的三级菜单 (HUMANRESOURCE_COMPENSATION_BENEFITS) ==========
        if (hrCompensationBenefitsMenu != null)
        {
            var (insertHRCB1, updateHRCB1) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMANRESOURCE_COMPENSATION_SALARY_CALC", menu =>
            {
                menu.MenuName = "薪资核算";
                menu.MenuCode = "HUMANRESOURCE_COMPENSATION_SALARY_CALC";
                menu.I18nKey = "menu.humanresource.compensationbenefits.salarycalc";
                menu.Icon = "RiCalculatorLine";
                menu.ParentId = hrCompensationBenefitsMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "humanresource:compensationbenefits:salarycalc:list";
                menu.RoutePath = "/human-resource/compensation-benefits/salary-calc";
                menu.ComponentPath = "human-resource/compensation-benefits/salary-calc/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRCB1;
            updateCount += updateHRCB1;

            var (insertHRCB2, updateHRCB2) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMANRESOURCE_COMPENSATION_TAX_CALC", menu =>
            {
                menu.MenuName = "个税计算";
                menu.MenuCode = "HUMANRESOURCE_COMPENSATION_TAX_CALC";
                menu.I18nKey = "menu.humanresource.compensationbenefits.taxcalc";
                menu.Icon = "RiPercentLine";
                menu.ParentId = hrCompensationBenefitsMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "humanresource:compensationbenefits:taxcalc:list";
                menu.RoutePath = "/human-resource/compensation-benefits/tax-calc";
                menu.ComponentPath = "human-resource/compensation-benefits/tax-calc/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRCB2;
            updateCount += updateHRCB2;

            var (insertHRCB3, updateHRCB3) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMANRESOURCE_COMPENSATION_SOCIAL_SECURITY", menu =>
            {
                menu.MenuName = "社保缴纳";
                menu.MenuCode = "HUMANRESOURCE_COMPENSATION_SOCIAL_SECURITY";
                menu.I18nKey = "menu.humanresource.compensationbenefits.socialsecurity";
                menu.Icon = "RiShieldLine";
                menu.ParentId = hrCompensationBenefitsMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "humanresource:compensationbenefits:socialsecurity:list";
                menu.RoutePath = "/human-resource/compensation-benefits/social-security";
                menu.ComponentPath = "human-resource/compensation-benefits/social-security/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRCB3;
            updateCount += updateHRCB3;

            var (insertHRCB4, updateHRCB4) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMANRESOURCE_COMPENSATION_PAYSLIP", menu =>
            {
                menu.MenuName = "薪资条发放";
                menu.MenuCode = "HUMANRESOURCE_COMPENSATION_PAYSLIP";
                menu.I18nKey = "menu.humanresource.compensationbenefits.payslip";
                menu.Icon = "RiMailSendLine";
                menu.ParentId = hrCompensationBenefitsMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "humanresource:compensationbenefits:payslip:list";
                menu.RoutePath = "/human-resource/compensation-benefits/payslip";
                menu.ComponentPath = "human-resource/compensation-benefits/payslip/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRCB4;
            updateCount += updateHRCB4;
        }

        // ========== 绩效管理下的三级菜单 (HUMANRESOURCE_PERFORMANCE) ==========
        if (hrPerformanceMenu != null)
        {
            var (insertHRP1, updateHRP1) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMANRESOURCE_PERFORMANCE_SCHEME_METRIC", menu =>
            {
                menu.MenuName = "方案指标";
                menu.MenuCode = "HUMANRESOURCE_PERFORMANCE_SCHEME_METRIC";
                menu.I18nKey = "menu.humanresource.performance.schememetric";
                menu.Icon = "RiListSettingsLine";
                menu.ParentId = hrPerformanceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "humanresource:performance:schememetric:list";
                menu.RoutePath = "/human-resource/performance/scheme-metric";
                menu.ComponentPath = "human-resource/performance/scheme-metric/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRP1;
            updateCount += updateHRP1;

            var (insertHRP2, updateHRP2) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMANRESOURCE_PERFORMANCE_CYCLE_SCHEDULE", menu =>
            {
                menu.MenuName = "周期日程";
                menu.MenuCode = "HUMANRESOURCE_PERFORMANCE_CYCLE_SCHEDULE";
                menu.I18nKey = "menu.humanresource.performance.cycleschedule";
                menu.Icon = "RiCalendarLine";
                menu.ParentId = hrPerformanceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "humanresource:performance:cycleschedule:list";
                menu.RoutePath = "/human-resource/performance/cycle-schedule";
                menu.ComponentPath = "human-resource/performance/cycle-schedule/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRP2;
            updateCount += updateHRP2;

            var (insertHRP3, updateHRP3) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMANRESOURCE_PERFORMANCE_OBJECTIVE", menu =>
            {
                menu.MenuName = "目标管理";
                menu.MenuCode = "HUMANRESOURCE_PERFORMANCE_OBJECTIVE";
                menu.I18nKey = "menu.humanresource.performance.objective";
                menu.Icon = "RiTargetLine";
                menu.ParentId = hrPerformanceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "humanresource:performance:objective:list";
                menu.RoutePath = "/human-resource/performance/objective";
                menu.ComponentPath = "human-resource/performance/objective/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRP3;
            updateCount += updateHRP3;

            var (insertHRP4, updateHRP4) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMANRESOURCE_PERFORMANCE_ASSESSMENT", menu =>
            {
                menu.MenuName = "考核评估";
                menu.MenuCode = "HUMANRESOURCE_PERFORMANCE_ASSESSMENT";
                menu.I18nKey = "menu.humanresource.performance.assessment";
                menu.Icon = "RiClipboardLine";
                menu.ParentId = hrPerformanceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "humanresource:performance:assessment:list";
                menu.RoutePath = "/human-resource/performance/assessment";
                menu.ComponentPath = "human-resource/performance/assessment/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRP4;
            updateCount += updateHRP4;

            var (insertHRP5, updateHRP5) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMANRESOURCE_PERFORMANCE_ANALYSIS_IMPROVEMENT", menu =>
            {
                menu.MenuName = "分析改进";
                menu.MenuCode = "HUMANRESOURCE_PERFORMANCE_ANALYSIS_IMPROVEMENT";
                menu.I18nKey = "menu.humanresource.performance.analysisimprovement";
                menu.Icon = "RiLineChartLine";
                menu.ParentId = hrPerformanceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "humanresource:performance:analysisimprovement:list";
                menu.RoutePath = "/human-resource/performance/analysis-improvement";
                menu.ComponentPath = "human-resource/performance/analysis-improvement/index";
                menu.SortOrder = 5;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRP5;
            updateCount += updateHRP5;
        }

        // ========== 培训发展下的三级菜单 (HUMANRESOURCE_TRAINING_DEVELOPMENT) ==========
        if (hrTrainingDevelopmentMenu != null)
        {
            var (insertHRTD1, updateHRTD1) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMANRESOURCE_TRAINING_PLAN", menu =>
            {
                menu.MenuName = "培训计划";
                menu.MenuCode = "HUMANRESOURCE_TRAINING_PLAN";
                menu.I18nKey = "menu.humanresource.trainingdevelopment.plan";
                menu.Icon = "RiCalendarPlanLine";
                menu.ParentId = hrTrainingDevelopmentMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "humanresource:trainingdevelopment:plan:list";
                menu.RoutePath = "/human-resource/training-development/plan";
                menu.ComponentPath = "human-resource/training-development/plan/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRTD1;
            updateCount += updateHRTD1;

            var (insertHRTD2, updateHRTD2) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMANRESOURCE_TRAINING_COURSE", menu =>
            {
                menu.MenuName = "培训课程";
                menu.MenuCode = "HUMANRESOURCE_TRAINING_COURSE";
                menu.I18nKey = "menu.humanresource.trainingdevelopment.course";
                menu.Icon = "RiBookOpenLine";
                menu.ParentId = hrTrainingDevelopmentMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "humanresource:trainingdevelopment:course:list";
                menu.RoutePath = "/human-resource/training-development/course";
                menu.ComponentPath = "human-resource/training-development/course/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRTD2;
            updateCount += updateHRTD2;

            var (insertHRTD3, updateHRTD3) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMANRESOURCE_TRAINING_RESULT", menu =>
            {
                menu.MenuName = "培训结果";
                menu.MenuCode = "HUMANRESOURCE_TRAINING_RESULT";
                menu.I18nKey = "menu.humanresource.trainingdevelopment.result";
                menu.Icon = "RiMedalLine";
                menu.ParentId = hrTrainingDevelopmentMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "humanresource:trainingdevelopment:result:list";
                menu.RoutePath = "/human-resource/training-development/result";
                menu.ComponentPath = "human-resource/training-development/result/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRTD3;
            updateCount += updateHRTD3;

            var (insertHRTD4, updateHRTD4) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMANRESOURCE_TRAINING_CAREER_DEVELOPMENT", menu =>
            {
                menu.MenuName = "职业发展";
                menu.MenuCode = "HUMANRESOURCE_TRAINING_CAREER_DEVELOPMENT";
                menu.I18nKey = "menu.humanresource.trainingdevelopment.career";
                menu.Icon = "RiRocketLine";
                menu.ParentId = hrTrainingDevelopmentMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "humanresource:trainingdevelopment:career:list";
                menu.RoutePath = "/human-resource/training-development/career";
                menu.ComponentPath = "human-resource/training-development/career/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRTD4;
            updateCount += updateHRTD4;
        }

        // ========== 人才管理下的三级菜单 (HUMANRESOURCE_TALENT) ==========
        if (hrTalentMenu != null)
        {
            var (insertHRT0, updateHRT0) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMANRESOURCE_TALENT_STAFFING_REQUIREMENT", menu =>
            {
                menu.MenuName = "用人需求";
                menu.MenuCode = "HUMANRESOURCE_TALENT_STAFFING_REQUIREMENT";
                menu.I18nKey = "menu.humanresource.talent.staffingrequirement";
                menu.Icon = "RiFileList3Line";
                menu.ParentId = hrTalentMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "humanresource:talent:staffingrequirement:list";
                menu.RoutePath = "/human-resource/talent/staffing-requirement";
                menu.ComponentPath = "human-resource/talent/staffing-requirement/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRT0;
            updateCount += updateHRT0;

            var (insertHRT1, updateHRT1) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMANRESOURCE_TALENT_RECRUITMENT_PLAN", menu =>
            {
                menu.MenuName = "招聘计划";
                menu.MenuCode = "HUMANRESOURCE_TALENT_RECRUITMENT_PLAN";
                menu.I18nKey = "menu.humanresource.talent.recruitmentplan";
                menu.Icon = "RiCalendarScheduleLine";
                menu.ParentId = hrTalentMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "humanresource:talent:recruitmentplan:list";
                menu.RoutePath = "/human-resource/talent/recruitment-plan";
                menu.ComponentPath = "human-resource/talent/recruitment-plan/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRT1;
            updateCount += updateHRT1;

            var (insertHRT2, updateHRT2) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMANRESOURCE_TALENT_JOB_POSTING", menu =>
            {
                menu.MenuName = "职位发布";
                menu.MenuCode = "HUMANRESOURCE_TALENT_JOB_POSTING";
                menu.I18nKey = "menu.humanresource.talent.jobposting";
                menu.Icon = "RiMegaphoneLine";
                menu.ParentId = hrTalentMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "humanresource:talent:jobposting:list";
                menu.RoutePath = "/human-resource/talent/job-posting";
                menu.ComponentPath = "human-resource/talent/job-posting/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRT2;
            updateCount += updateHRT2;

            var (insertHRT3, updateHRT3) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMANRESOURCE_TALENT_RESUME_FILTER", menu =>
            {
                menu.MenuName = "简历筛选";
                menu.MenuCode = "HUMANRESOURCE_TALENT_RESUME_FILTER";
                menu.I18nKey = "menu.humanresource.talent.resumefilter";
                menu.Icon = "RiFilter3Line";
                menu.ParentId = hrTalentMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "humanresource:talent:resumefilter:list";
                menu.RoutePath = "/human-resource/talent/resume-filter";
                menu.ComponentPath = "human-resource/talent/resume-filter/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRT3;
            updateCount += updateHRT3;

            var (insertHRT4, updateHRT4) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMANRESOURCE_TALENT_INTERVIEW", menu =>
            {
                menu.MenuName = "面试安排";
                menu.MenuCode = "HUMANRESOURCE_TALENT_INTERVIEW";
                menu.I18nKey = "menu.humanresource.talent.interview";
                menu.Icon = "RiCalendarEventLine";
                menu.ParentId = hrTalentMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "humanresource:talent:interview:list";
                menu.RoutePath = "/human-resource/talent/interview";
                menu.ComponentPath = "human-resource/talent/interview/index";
                menu.SortOrder = 5;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRT4;
            updateCount += updateHRT4;

            var (insertHRT5, updateHRT5) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMANRESOURCE_TALENT_OFFER", menu =>
            {
                menu.MenuName = "录用";
                menu.MenuCode = "HUMANRESOURCE_TALENT_OFFER";
                menu.I18nKey = "menu.humanresource.talent.offer";
                menu.Icon = "RiCheckboxCircleLine";
                menu.ParentId = hrTalentMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "humanresource:talent:offer:list";
                menu.RoutePath = "/human-resource/talent/offer";
                menu.ComponentPath = "human-resource/talent/offer/index";
                menu.SortOrder = 6;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRT5;
            updateCount += updateHRT5;
        }

        // ========== 报表管理下的三级菜单 (STATISTICS_REPORT) ==========
        if (statisticsReportMenu != null)
        {
            var (insertSR1, updateSR1) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "STATISTICS_REPORT_FINANCIAL", menu =>
            {
                menu.MenuName = "财务统计";
                menu.MenuCode = "STATISTICS_REPORT_FINANCIAL";
                menu.I18nKey = "menu.statistics.report.financial._self";
                menu.Icon = "RiMoneyDollarCircleLine";
                menu.ParentId = statisticsReportMenu.Id;
                menu.MenuType = 0;
                menu.RoutePath = "/statistics/report/financial";
                menu.ComponentPath = "";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertSR1;
            updateCount += updateSR1;

            var (insertSR2, updateSR2) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "STATISTICS_REPORT_HUMANRESOURCE", menu =>
            {
                menu.MenuName = "人力统计";
                menu.MenuCode = "STATISTICS_REPORT_HUMANRESOURCE";
                menu.I18nKey = "menu.statistics.report.humanresource._self";
                menu.Icon = "RiTeamLine";
                menu.ParentId = statisticsReportMenu.Id;
                menu.MenuType = 0;
                menu.RoutePath = "/statistics/report/human-resource";
                menu.ComponentPath = "";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertSR2;
            updateCount += updateSR2;

            var (insertSR3, updateSR3) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "STATISTICS_REPORT_LOGISTICS", menu =>
            {
                menu.MenuName = "后勤统计";
                menu.MenuCode = "STATISTICS_REPORT_LOGISTICS";
                menu.I18nKey = "menu.statistics.report.logistics._self";
                menu.Icon = "RiTruckLine";
                menu.ParentId = statisticsReportMenu.Id;
                menu.MenuType = 0;
                menu.RoutePath = "/statistics/report/logistics";
                menu.ComponentPath = "";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertSR3;
            updateCount += updateSR3;

        }

        // ========== 日志管理下的三级菜单 (STATISTICS_LOGGING) ==========
        if (statisticsLoggingMenu != null)
        {
            var (insertSL1, updateSL1) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "STATISTICS_LOGGING_LOGIN_LOG", menu =>
            {
                menu.MenuName = "登录日志";
                menu.MenuCode = "STATISTICS_LOGGING_LOGIN_LOG";
                menu.I18nKey = "menu.statistics.logging.loginlog";
                menu.Icon = "RiLoginBoxLine";
                menu.ParentId = statisticsLoggingMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "statistics:logging:loginlog:list";
                menu.RoutePath = "/statistics/logging/login-log";
                menu.ComponentPath = "statistics/logging/login-log/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertSL1;
            updateCount += updateSL1;

            var (insertSL2, updateSL2) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "STATISTICS_LOGGING_OPER_LOG", menu =>
            {
                menu.MenuName = "操作日志";
                menu.MenuCode = "STATISTICS_LOGGING_OPER_LOG";
                menu.I18nKey = "menu.statistics.logging.operlog";
                menu.Icon = "RiHistoryLine";
                menu.ParentId = statisticsLoggingMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "statistics:logging:operlog:list";
                menu.RoutePath = "/statistics/logging/oper-log";
                menu.ComponentPath = "statistics/logging/oper-log/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertSL2;
            updateCount += updateSL2;

            var (insertSL3, updateSL3) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "STATISTICS_LOGGING_DELTA_LOG", menu =>
            {
                menu.MenuName = "差异日志";
                menu.MenuCode = "STATISTICS_LOGGING_DELTA_LOG";
                menu.I18nKey = "menu.statistics.logging.deltalog";
                menu.Icon = "RiGitCommitLine";
                menu.ParentId = statisticsLoggingMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "statistics:logging:deltalog:list";
                menu.RoutePath = "/statistics/logging/delta-log";
                menu.ComponentPath = "statistics/logging/delta-log/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertSL3;
            updateCount += updateSL3;

            var (insertSL4, updateSL4) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "STATISTICS_LOGGING_QUARTZ_LOG", menu =>
            {
                menu.MenuName = "任务日志";
                menu.MenuCode = "STATISTICS_LOGGING_QUARTZ_LOG";
                menu.I18nKey = "menu.statistics.logging.quartzlog";
                menu.Icon = "RiTimerLine";
                menu.ParentId = statisticsLoggingMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "statistics:logging:quartzlog:list";
                menu.RoutePath = "/statistics/logging/quartz-log";
                menu.ComponentPath = "statistics/logging/quartz-log/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertSL4;
            updateCount += updateSL4;

            var (insertSL5, updateSL5) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "STATISTICS_LOGGING_SERVER_MONITOR", menu =>
            {
                menu.MenuName = "服务监控";
                menu.MenuCode = "STATISTICS_LOGGING_SERVER_MONITOR";
                menu.I18nKey = "menu.statistics.logging.servermonitor";
                menu.Icon = "RiServerLine";
                menu.ParentId = statisticsLoggingMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "statistics:logging:servermonitor:query";
                menu.RoutePath = "/statistics/logging/server-monitor";
                menu.ComponentPath = "statistics/logging/server-monitor/index";
                menu.SortOrder = 5;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertSL5;
            updateCount += updateSL5;
        }

        return (insertCount, updateCount);
    }

    /// <summary>
    /// 创建或更新菜单。
    /// </summary>
    /// <param name="menuRepository">菜单仓储。</param>
    /// <param name="seedContext">数据库上下文。</param>
    /// <param name="tenantCode">租户编码。</param>
    /// <param name="menuCode">菜单编码(业务键)。</param>
    /// <param name="configure">菜单配置委托。</param>
    /// <returns>元组:(InsertCount, UpdateCount),本条菜单新增或更新条数(0或1)。</returns>
    private static async Task<(int InsertCount, int UpdateCount)> CreateOrUpdateMenuAsync(
        ITaktTenantSeedRepository<TaktMenu> menuRepository,
        TaktSeedContext seedContext,
        string tenantCode,
        string menuCode,
        Action<TaktMenu> configure)
    {
        var menu = await menuRepository.FirstAsync(m => m.TenantCode == tenantCode && m.MenuCode == menuCode);
        
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
                var parentMenu = await menuRepository.FirstAsync(m => m.TenantCode == tenantCode && m.Id == menu.ParentId);
                if (parentMenu != null)
                {
                    menu.MenuPath = $"{parentMenu.MenuPath}{menu.Id}/";
                    menu.Level = parentMenu.Level + 1;
                    
                    // 更新父级 IsLeaf 为非叶子
                    if (parentMenu.IsLeaf == 1)
                    {
                        parentMenu.IsLeaf = 0;
                        await menuRepository.UpdateAsync(parentMenu);
                    }
                }
            }
            else
            {
                menu.MenuPath = $"/{menu.Id}/";
                menu.Level = 1;
            }
            
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
                var parentMenu = await menuRepository.FirstAsync(m => m.TenantCode == tenantCode && m.Id == menu.ParentId);
                if (parentMenu != null)
                {
                    menu.MenuPath = $"{parentMenu.MenuPath}{menu.Id}/";
                    menu.Level = parentMenu.Level + 1;
                    
                    // 更新父级 IsLeaf 为非叶子
                    if (parentMenu.IsLeaf == 1)
                    {
                        parentMenu.IsLeaf = 0;
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
