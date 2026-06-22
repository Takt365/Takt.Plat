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
/// 父级为 TaktMenuLevel2SeedData 中定义的二级目录或分组（如 ROUTINE_NEWS_CENTER、LOGISTICS_SALES、HUMANRESOURCE_TALENT 等）。
/// 页面类型菜单需配置以 <c>:list</c> 结尾的权限串，供 TaktMenuButtonSeedData 生成按钮。
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
    /// <param name="serviceProvider">服务提供者，用于解析 ITaktRepository{TaktMenu}。</param>
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
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "LOGISTICS_MATERIALS" && m.IsDeleted == 0)
            .FirstAsync();
        var logisticsProcurementMenu = await seedContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "LOGISTICS_PROCUREMENT" && m.IsDeleted == 0)
            .FirstAsync();
        var manufacturingMenu = await seedContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "LOGISTICS_MANUFACTURING" && m.IsDeleted == 0)
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
        var logisticsSerialMenu = await seedContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "LOGISTICS_SERIAL" && m.IsDeleted == 0)
            .FirstAsync();
        var hrOrganizationMenu = await seedContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "HUMANRESOURCE_ORGANIZATION" && m.IsDeleted == 0)
            .FirstAsync();
        var hrPersonnelMenu = await seedContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "HUMANRESOURCE_PERSONNEL" && m.IsDeleted == 0)
            .FirstAsync();
        var hrAttendanceMenu = await seedContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "HUMANRESOURCE_ATTENDANCE" && m.IsDeleted == 0)
            .FirstAsync();
        var hrCompensationMenu = await seedContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "HUMANRESOURCE_COMPENSATION" && m.IsDeleted == 0)
            .FirstAsync();
        var hrBenefitsMenu = await seedContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "HUMANRESOURCE_BENEFITS" && m.IsDeleted == 0)
            .FirstAsync();
        var hrPerformanceMenu = await seedContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "HUMANRESOURCE_PERFORMANCE" && m.IsDeleted == 0)
            .FirstAsync();
        var hrTrainingMenu = await seedContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "HUMANRESOURCE_TRAINING" && m.IsDeleted == 0)
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
        var routineHelpDeskMenu = await seedContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "ROUTINE_HELPDESK" && m.IsDeleted == 0)
            .FirstAsync();
        var routineDocumentCenterMenu = await seedContext.Db.Queryable<TaktMenu>()
            .Where(m => m.TenantCode == tenantCode && m.MenuCode == "ROUTINE_DOCUMENT_CENTER" && m.IsDeleted == 0)
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
                menu.Permission = "accounting:financial:account:title:list";
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

            var (insertAF1Cl, updateAF1Cl) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "ACCOUNTING_FINANCIAL_ACCOUNT_TITLE_CHANGE_LOG", menu =>
            {
                menu.MenuName = "会计科目变更";
                menu.MenuCode = "ACCOUNTING_FINANCIAL_ACCOUNT_TITLE_CHANGE_LOG";
                menu.I18nKey = "menu.accounting.financial.accounttitle.changelog";
                menu.Icon = "RiFileHistoryLine";
                menu.ParentId = accountingFinancialMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "accounting:financial:account:title:list";
                menu.RoutePath = "/accounting/financial/account-title-change-log";
                menu.ComponentPath = "accounting/financial/account-title-change-log/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertAF1Cl;
            updateCount += updateAF1Cl;

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
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertAF2;
            updateCount += updateAF2;

            var (insertAF2Cl, updateAF2Cl) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "ACCOUNTING_FINANCIAL_ASSET_CHANGE_LOG", menu =>
            {
                menu.MenuName = "固定资产变更";
                menu.MenuCode = "ACCOUNTING_FINANCIAL_ASSET_CHANGE_LOG";
                menu.I18nKey = "menu.accounting.financial.asset.changelog";
                menu.Icon = "RiFileHistoryLine";
                menu.ParentId = accountingFinancialMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "accounting:financial:asset:list";
                menu.RoutePath = "/accounting/financial/asset-change-log";
                menu.ComponentPath = "accounting/financial/asset-change-log/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertAF2Cl;
            updateCount += updateAF2Cl;

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
                menu.SortOrder = 5;
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
                menu.SortOrder = 6;
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
                menu.Permission = "accounting:controlling:profit:center:list";
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

            var (insertAC1Cl, updateAC1Cl) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "ACCOUNTING_CONTROLLING_PROFIT_CENTER_CHANGE_LOG", menu =>
            {
                menu.MenuName = "利润中心变更";
                menu.MenuCode = "ACCOUNTING_CONTROLLING_PROFIT_CENTER_CHANGE_LOG";
                menu.I18nKey = "menu.accounting.controlling.profitcenter.changelog";
                menu.Icon = "RiFileHistoryLine";
                menu.ParentId = accountingControllingMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "accounting:controlling:profit:center:list";
                menu.RoutePath = "/accounting/controlling/profit-center-change-log";
                menu.ComponentPath = "accounting/controlling/profit-center-change-log/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertAC1Cl;
            updateCount += updateAC1Cl;

            var (insertAC2, updateAC2) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "ACCOUNTING_CONTROLLING_COST_CENTER", menu =>
            {
                menu.MenuName = "成本中心";
                menu.MenuCode = "ACCOUNTING_CONTROLLING_COST_CENTER";
                menu.I18nKey = "menu.accounting.controlling.costcenter";
                menu.Icon = "RiPieChart2Line";
                menu.ParentId = accountingControllingMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "accounting:controlling:cost:center:list";
                menu.RoutePath = "/accounting/controlling/cost-center";
                menu.ComponentPath = "accounting/controlling/cost-center/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertAC2;
            updateCount += updateAC2;

            var (insertAC2Cl, updateAC2Cl) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "ACCOUNTING_CONTROLLING_COST_CENTER_CHANGE_LOG", menu =>
            {
                menu.MenuName = "成本中心变更";
                menu.MenuCode = "ACCOUNTING_CONTROLLING_COST_CENTER_CHANGE_LOG";
                menu.I18nKey = "menu.accounting.controlling.costcenter.changelog";
                menu.Icon = "RiFileHistoryLine";
                menu.ParentId = accountingControllingMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "accounting:controlling:cost:center:list";
                menu.RoutePath = "/accounting/controlling/cost-center-change-log";
                menu.ComponentPath = "accounting/controlling/cost-center-change-log/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertAC2Cl;
            updateCount += updateAC2Cl;

            var (insertAC3, updateAC3) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "ACCOUNTING_CONTROLLING_COST_ELEMENT", menu =>
            {
                menu.MenuName = "成本要素";
                menu.MenuCode = "ACCOUNTING_CONTROLLING_COST_ELEMENT";
                menu.I18nKey = "menu.accounting.controlling.costelement";
                menu.Icon = "RiListCheck";
                menu.ParentId = accountingControllingMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "accounting:controlling:cost:element:list";
                menu.RoutePath = "/accounting/controlling/cost-element";
                menu.ComponentPath = "accounting/controlling/cost-element/index";
                menu.SortOrder = 5;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertAC3;
            updateCount += updateAC3;

            var (insertAC3Cl, updateAC3Cl) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "ACCOUNTING_CONTROLLING_COST_ELEMENT_CHANGE_LOG", menu =>
            {
                menu.MenuName = "成本要素变更";
                menu.MenuCode = "ACCOUNTING_CONTROLLING_COST_ELEMENT_CHANGE_LOG";
                menu.I18nKey = "menu.accounting.controlling.costelement.changelog";
                menu.Icon = "RiFileHistoryLine";
                menu.ParentId = accountingControllingMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "accounting:controlling:cost:element:list";
                menu.RoutePath = "/accounting/controlling/cost-element-change-log";
                menu.ComponentPath = "accounting/controlling/cost-element-change-log/index";
                menu.SortOrder = 6;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertAC3Cl;
            updateCount += updateAC3Cl;

            var (insertAC4, updateAC4) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "ACCOUNTING_CONTROLLING_STANDARD_WAGE_RATE", menu =>
            {
                menu.MenuName = "标准工资率";
                menu.MenuCode = "ACCOUNTING_CONTROLLING_STANDARD_WAGE_RATE";
                menu.I18nKey = "menu.accounting.controlling.standardwagerate";
                menu.Icon = "RiCalculatorLine";
                menu.ParentId = accountingControllingMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "accounting:controlling:standard:wage:rate:list";
                menu.RoutePath = "/accounting/controlling/standard-wage-rate";
                menu.ComponentPath = "accounting/controlling/standard-wage-rate/index";
                menu.SortOrder = 7;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertAC4;
            updateCount += updateAC4;
        }

        // ========== 物料管理下的三级菜单 (LOGISTICS_MATERIALS) ==========
        if (logisticsMaterialMenu != null)
        {
            var (insertLM1, updateLM1) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_MATERIALS_PLANT", menu =>
            {
                menu.MenuName = "工厂信息";
                menu.MenuCode = "LOGISTICS_MATERIALS_PLANT";
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

            var (insertLM2, updateLM2) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_MATERIALS_MATERIAL", menu =>
            {
                menu.MenuName = "全局物料";
                menu.MenuCode = "LOGISTICS_MATERIALS_MATERIAL";
                menu.I18nKey = "menu.logistics.materials.material";
                menu.Icon = "RiArchiveLine";
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

            var (insertLM2Cl, updateLM2Cl) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_MATERIALS_MATERIAL_CHANGE_LOG", menu =>
            {
                menu.MenuName = "全局物料变更";
                menu.MenuCode = "LOGISTICS_MATERIALS_MATERIAL_CHANGE_LOG";
                menu.I18nKey = "menu.logistics.materials.material.changelog";
                menu.Icon = "RiFileHistoryLine";
                menu.ParentId = logisticsMaterialMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:materials:material:list";
                menu.RoutePath = "/logistics/materials/material-change-log";
                menu.ComponentPath = "logistics/materials/material-change-log/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLM2Cl;
            updateCount += updateLM2Cl;

            var (insertLM3, updateLM3) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_MATERIALS_MATERIAL_PLANT", menu =>
            {
                menu.MenuName = "工厂物料";
                menu.MenuCode = "LOGISTICS_MATERIALS_PLANT";
                menu.I18nKey = "menu.logistics.materials.materialplant";
                menu.Icon = "RiArchiveStackLine";
                menu.ParentId = logisticsMaterialMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:materials:material:plant:list";
                menu.RoutePath = "/logistics/materials/material-plant";
                menu.ComponentPath = "logistics/materials/material-plant/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLM3;
            updateCount += updateLM3;

            var (insertLM3Cl, updateLM3Cl) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_MATERIALS_MATERIAL_PLANT_CHANGE_LOG", menu =>
            {
                menu.MenuName = "工厂物料变更";
                menu.MenuCode = "LOGISTICS_MATERIALS_MATERIAL_PLANT_CHANGE_LOG";
                menu.I18nKey = "menu.logistics.materials.materialplant.changelog";
                menu.Icon = "RiFileHistoryLine";
                menu.ParentId = logisticsMaterialMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:materials:material:plant:list";
                menu.RoutePath = "/logistics/materials/material-plant-change-log";
                menu.ComponentPath = "logistics/materials/material-plant-change-log/index";
                menu.SortOrder = 5;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLM3Cl;
            updateCount += updateLM3Cl;

            var (insertLM3Wh, updateLM3Wh) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_MATERIALS_WAREHOUSE", menu =>
            {
                menu.MenuName = "仓库信息";
                menu.MenuCode = "LOGISTICS_MATERIALS_WAREHOUSE";
                menu.I18nKey = "menu.logistics.materials.warehouse";
                menu.Icon = "RiStore2Line";
                menu.ParentId = logisticsMaterialMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:materials:warehouse:list";
                menu.RoutePath = "/logistics/materials/warehouse";
                menu.ComponentPath = "logistics/materials/warehouse/index";
                menu.SortOrder = 6;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLM3Wh;
            updateCount += updateLM3Wh;

            var (insertLM3Mg, updateLM3Mg) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_MATERIALS_MATERIAL_GROUP", menu =>
            {
                menu.MenuName = "物料组";
                menu.MenuCode = "LOGISTICS_MATERIALS_GROUP";
                menu.I18nKey = "menu.logistics.materials.materialgroup";
                menu.Icon = "RiStackLine";
                menu.ParentId = logisticsMaterialMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:materials:material:group:list";
                menu.RoutePath = "/logistics/materials/material-group";
                menu.ComponentPath = "logistics/materials/material-group/index";
                menu.SortOrder = 7;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLM3Mg;
            updateCount += updateLM3Mg;

            var (insertLM3Sl, updateLM3Sl) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_MATERIALS_STORAGE_LOCATION", menu =>
            {
                menu.MenuName = "库位信息";
                menu.MenuCode = "LOGISTICS_MATERIALS_STORAGE_LOCATION";
                menu.I18nKey = "menu.logistics.materials.storagelocation";
                menu.Icon = "RiLayoutGridLine";
                menu.ParentId = logisticsMaterialMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:materials:storage:location:list";
                menu.RoutePath = "/logistics/materials/storage-location";
                menu.ComponentPath = "logistics/materials/storage-location/index";
                menu.SortOrder = 8;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLM3Sl;
            updateCount += updateLM3Sl;

            var (insertLM4, updateLM4) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_MATERIALS_PACKAGING", menu =>
            {
                menu.MenuName = "包装物料";
                menu.MenuCode = "LOGISTICS_MATERIALS_PACKAGING";
                menu.I18nKey = "menu.logistics.materials.packaging";
                menu.Icon = "RiBox3Line";
                menu.ParentId = logisticsMaterialMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:materials:packaging:list";
                menu.RoutePath = "/logistics/materials/packaging";
                menu.ComponentPath = "logistics/materials/packaging/index";
                menu.SortOrder = 9;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLM4;
            updateCount += updateLM4;

            var (insertLM5, updateLM5) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_MATERIALS_MODEL_DESTINATION", menu =>
            {
                menu.MenuName = "机种仕向";
                menu.MenuCode = "LOGISTICS_MATERIALS_MODEL_DESTINATION";
                menu.I18nKey = "menu.logistics.materials.modeldestination";
                menu.Icon = "RiEarthLine";
                menu.ParentId = logisticsMaterialMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:materials:model:destination:list";
                menu.RoutePath = "/logistics/materials/model-destination";
                menu.ComponentPath = "logistics/materials/model-destination/index";
                menu.SortOrder = 10;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLM5;
            updateCount += updateLM5;

            var (insertLM6, updateLM6) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_MATERIALS_MANUFACTURER_MATERIAL", menu =>
            {
                menu.MenuName = "制造商物料";
                menu.MenuCode = "LOGISTICS_MATERIALS_MANUFACTURER_MATERIAL";
                menu.I18nKey = "menu.logistics.materials.manufacturer";
                menu.Icon = "RiBuilding4Line";
                menu.ParentId = logisticsMaterialMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:materials:manufacturer:list";
                menu.RoutePath = "/logistics/materials/manufacturer";
                menu.ComponentPath = "logistics/materials/manufacturer/index";
                menu.SortOrder = 11;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLM6;
            updateCount += updateLM6;

            var (insertLM7, updateLM7) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_MATERIALS_MATERIAL_TRANSACTION", menu =>
            {
                menu.MenuName = "物料交易";
                menu.MenuCode = "LOGISTICS_MATERIALS_TRANSACTION";
                menu.I18nKey = "menu.logistics.materials.materialtransaction";
                menu.Icon = "RiExchangeLine";
                menu.ParentId = logisticsMaterialMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:materials:material:transaction:list";
                menu.RoutePath = "/logistics/materials/material-transaction";
                menu.ComponentPath = "logistics/materials/material-transaction/index";
                menu.SortOrder = 12;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLM7;
            updateCount += updateLM7;
        }

        // ========== 采购管理下的三级菜单 (LOGISTICS_PROCUREMENT) ==========
        if (logisticsProcurementMenu != null)
        {
            var (insert03, update03) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_PROCUREMENT_SUPPLIER", menu =>
            {
                menu.MenuName = "供应商";
                menu.MenuCode = "LOGISTICS_PROCUREMENT_SUPPLIER";
                menu.I18nKey = "menu.logistics.procurement.supplier";
                menu.Icon = "RiTruckLine";
                menu.ParentId = logisticsProcurementMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:procurement:supplier:list";
                menu.RoutePath = "/logistics/procurement/supplier";
                menu.ComponentPath = "logistics/procurement/supplier/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insert03;
            updateCount += update03;

            var (insert04, update04) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_PROCUREMENT_VENDOR", menu =>
            {
                menu.MenuName = "经销商";
                menu.MenuCode = "LOGISTICS_PROCUREMENT_VENDOR";
                menu.I18nKey = "menu.logistics.procurement.vendor";
                menu.Icon = "RiRegisteredLine";
                menu.ParentId = logisticsProcurementMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:procurement:vendor:list";
                menu.RoutePath = "/logistics/procurement/vendor";
                menu.ComponentPath = "logistics/procurement/vendor/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insert04;
            updateCount += update04;

            var (insert06, update06) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_PROCUREMENT_SOURCE_OF_SUPPLY", menu =>
            {
                menu.MenuName = "货源";
                menu.MenuCode = "LOGISTICS_PROCUREMENT_SOURCE_OF_SUPPLY";
                menu.I18nKey = "menu.logistics.procurement.sourceofsupply";
                menu.Icon = "RiLinksLine";
                menu.ParentId = logisticsProcurementMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:procurement:source:of:supply:list";
                menu.RoutePath = "/logistics/procurement/source-of-supply";
                menu.ComponentPath = "logistics/procurement/source-of-supply/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insert06;
            updateCount += update06;

            var (insert07, update07) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_PROCUREMENT_PURCHASE_REQUEST", menu =>
            {
                menu.MenuName = "采购申请";
                menu.MenuCode = "LOGISTICS_PROCUREMENT_PURCHASE_REQUEST";
                menu.I18nKey = "menu.logistics.procurement.purchaserequest";
                menu.Icon = "RiFileAddLine";
                menu.ParentId = logisticsProcurementMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:procurement:purchase:request:list";
                menu.RoutePath = "/logistics/procurement/purchase-request";
                menu.ComponentPath = "logistics/procurement/purchase-request/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insert07;
            updateCount += update07;

            var (insert07Cl, update07Cl) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_PROCUREMENT_PURCHASE_REQUEST_CHANGE_LOG", menu =>
            {
                menu.MenuName = "采购申请变更";
                menu.MenuCode = "LOGISTICS_PROCUREMENT_PURCHASE_REQUEST_CHANGE_LOG";
                menu.I18nKey = "menu.logistics.procurement.purchaserequest.changelog";
                menu.Icon = "RiFileHistoryLine";
                menu.ParentId = logisticsProcurementMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:procurement:purchase:request:list";
                menu.RoutePath = "/logistics/procurement/purchase-request-change-log";
                menu.ComponentPath = "logistics/procurement/purchase-request-change-log/index";
                menu.SortOrder = 5;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insert07Cl;
            updateCount += update07Cl;

            var (insert08, update08) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_PROCUREMENT_PURCHASE_ORDER", menu =>
            {
                menu.MenuName = "采购订单";
                menu.MenuCode = "LOGISTICS_PROCUREMENT_PURCHASE_ORDER";
                menu.I18nKey = "menu.logistics.procurement.purchaseorder";
                menu.Icon = "RiListOrdered";
                menu.ParentId = logisticsProcurementMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:procurement:purchase:order:list";
                menu.RoutePath = "/logistics/procurement/purchase-order";
                menu.ComponentPath = "logistics/procurement/purchase-order/index";
                menu.SortOrder = 6;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insert08;
            updateCount += update08;

            var (insert08Cl, update08Cl) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_PROCUREMENT_PURCHASE_ORDER_CHANGE_LOG", menu =>
            {
                menu.MenuName = "采购订单变更";
                menu.MenuCode = "LOGISTICS_PROCUREMENT_PURCHASE_ORDER_CHANGE_LOG";
                menu.I18nKey = "menu.logistics.procurement.purchaseorder.changelog";
                menu.Icon = "RiFileHistoryLine";
                menu.ParentId = logisticsProcurementMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:procurement:purchase:order:list";
                menu.RoutePath = "/logistics/procurement/purchase-order-change-log";
                menu.ComponentPath = "logistics/procurement/purchase-order-change-log/index";
                menu.SortOrder = 7;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insert08Cl;
            updateCount += update08Cl;

            var (insert08Price, update08Price) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_PROCUREMENT_PURCHASE_PRICE", menu =>
            {
                menu.MenuName = "采购价格";
                menu.MenuCode = "LOGISTICS_PROCUREMENT_PURCHASE_PRICE";
                menu.I18nKey = "menu.logistics.procurement.purchaseprice";
                menu.Icon = "RiPriceTag3Line";
                menu.ParentId = logisticsProcurementMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:procurement:purchase:price:list";
                menu.RoutePath = "/logistics/procurement/purchase-price";
                menu.ComponentPath = "logistics/procurement/purchase-price/index";
                menu.SortOrder = 8;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insert08Price;
            updateCount += update08Price;

            var (insert08PriceCl, update08PriceCl) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_PROCUREMENT_PURCHASE_PRICE_CHANGE_LOG", menu =>
            {
                menu.MenuName = "采购价格变更";
                menu.MenuCode = "LOGISTICS_PROCUREMENT_PURCHASE_PRICE_CHANGE_LOG";
                menu.I18nKey = "menu.logistics.procurement.purchaseprice.changelog";
                menu.Icon = "RiFileHistoryLine";
                menu.ParentId = logisticsProcurementMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:procurement:purchase:price:list";
                menu.RoutePath = "/logistics/procurement/purchase-price-change-log";
                menu.ComponentPath = "logistics/procurement/purchase-price-change-log/index";
                menu.SortOrder = 9;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insert08PriceCl;
            updateCount += update08PriceCl;

            var (insert09, update09) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_PROCUREMENT_PURCHASE_INVOICE", menu =>
            {
                menu.MenuName = "采购发票";
                menu.MenuCode = "LOGISTICS_PROCUREMENT_PURCHASE_INVOICE";
                menu.I18nKey = "menu.logistics.procurement.purchaseinvoice";
                menu.Icon = "RiFilePaper2Line";
                menu.ParentId = logisticsProcurementMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:procurement:purchase:invoice:list";
                menu.RoutePath = "/logistics/procurement/purchase-invoice";
                menu.ComponentPath = "logistics/procurement/purchase-invoice/index";
                menu.SortOrder = 10;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insert09;
            updateCount += update09;

            var (insertPg, updatePg) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_PROCUREMENT_PURCHASE_GROUP", menu =>
            {
                menu.MenuName = "采购组";
                menu.MenuCode = "LOGISTICS_PROCUREMENT_PURCHASE_GROUP";
                menu.I18nKey = "menu.logistics.procurement.purchasegroup";
                menu.Icon = "RiGroupLine";
                menu.ParentId = logisticsProcurementMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:procurement:purchase:group:list";
                menu.RoutePath = "/logistics/procurement/purchase-group";
                menu.ComponentPath = "logistics/procurement/purchase-group/index";
                menu.SortOrder = 11;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertPg;
            updateCount += updatePg;
        }

        // ========== 生产执行下的三级菜单 (LOGISTICS_MANUFACTURING) ==========
        if (manufacturingMenu != null)
        {
            var (insertMFG1, updateMFG1) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_MANUFACTURING_BOM", menu =>
            {
                menu.MenuName = "BOM管理";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_BOM";
                menu.I18nKey = "menu.logistics.manufacturing.bom._self";
                menu.Icon = "RiTreeLine";
                menu.ParentId = manufacturingMenu.Id;
                menu.MenuType = 0;
                menu.RoutePath = "/logistics/manufacturing/bom";
                menu.ComponentPath = "logistics/manufacturing/bom";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertMFG1;
            updateCount += updateMFG1;

            var (insertMFG2, updateMFG2) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_MANUFACTURING_PLANNING", menu =>
            {
                menu.MenuName = "MRP计划";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_PLANNING";
                menu.I18nKey = "menu.logistics.manufacturing.planning._self";
                menu.Icon = "RiCalendarTodoLine";
                menu.ParentId = manufacturingMenu.Id;
                menu.MenuType = 0;
                menu.RoutePath = "/logistics/manufacturing/planning";
                menu.ComponentPath = "logistics/manufacturing/planning";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertMFG2;
            updateCount += updateMFG2;

            var (insertMFG3, updateMFG3) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_MANUFACTURING_SCHEDULING", menu =>
            {
                menu.MenuName = "生产排程";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_SCHEDULING";
                menu.I18nKey = "menu.logistics.manufacturing.scheduling._self";
                menu.Icon = "RiCalendarScheduleLine";
                menu.ParentId = manufacturingMenu.Id;
                menu.MenuType = 0;
                menu.RoutePath = "/logistics/manufacturing/scheduling";
                menu.ComponentPath = "logistics/manufacturing/scheduling";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertMFG3;
            updateCount += updateMFG3;

            var (insertMFG4, updateMFG4) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE", menu =>
            {
                menu.MenuName = "设变";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE";
                menu.I18nKey = "menu.logistics.manufacturing.engineeringchange._self";
                menu.Icon = "RiEditCircleLine";
                menu.ParentId = manufacturingMenu.Id;
                menu.MenuType = 0;
                menu.RoutePath = "/logistics/manufacturing/engineering-change";
                menu.ComponentPath = "logistics/manufacturing/engineering-change";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertMFG4;
            updateCount += updateMFG4;

            var (insertMFG5, updateMFG5) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_MANUFACTURING_OUTPUT", menu =>
            {
                menu.MenuName = "产出管理";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_OUTPUT";
                menu.I18nKey = "menu.logistics.manufacturing.output._self";
                menu.Icon = "RiBarChart2Line";
                menu.ParentId = manufacturingMenu.Id;
                menu.MenuType = 0;
                menu.RoutePath = "/logistics/manufacturing/output";
                menu.ComponentPath = "logistics/manufacturing/output";
                menu.SortOrder = 5;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertMFG5;
            updateCount += updateMFG5;

            var (insertMFG6, updateMFG6) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_MANUFACTURING_DEFECT", menu =>
            {
                menu.MenuName = "不良";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_DEFECT";
                menu.I18nKey = "menu.logistics.manufacturing.defect._self";
                menu.Icon = "RiErrorWarningLine";
                menu.ParentId = manufacturingMenu.Id;
                menu.MenuType = 0;
                menu.RoutePath = "/logistics/manufacturing/defect";
                menu.ComponentPath = "logistics/manufacturing/defect";
                menu.SortOrder = 6;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertMFG6;
            updateCount += updateMFG6;

            var (insertMFG7, updateMFG7) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_MANUFACTURING_SOP", menu =>
            {
                menu.MenuName = "SOP管理";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_SOP";
                menu.I18nKey = "menu.logistics.manufacturing.sop._self";
                menu.Icon = "RiBookOpenLine";
                menu.ParentId = manufacturingMenu.Id;
                menu.MenuType = 0;
                menu.RoutePath = "/logistics/manufacturing/sop";
                menu.ComponentPath = "logistics/manufacturing/sop";
                menu.SortOrder = 7;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertMFG7;
            updateCount += updateMFG7;
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
                menu.MenuType = 0;
                menu.RoutePath = "/logistics/quality/cost";
                menu.ComponentPath = "logistics/quality/cost";
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
                menu.MenuType = 0;
                menu.RoutePath = "/logistics/quality/operation";
                menu.ComponentPath = "logistics/quality/operation";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLQ2;
            updateCount += updateLQ2;

            var (insertLQ3, updateLQ3) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_QUALITY_COMPLAINT", menu =>
            {
                menu.MenuName = "客诉管理";
                menu.MenuCode = "LOGISTICS_QUALITY_COMPLAINT";
                menu.I18nKey = "menu.logistics.quality.complaint._self";
                menu.Icon = "RiMessage3Line";
                menu.ParentId = logisticsQualityMenu.Id;
                menu.MenuType = 0;
                menu.RoutePath = "/logistics/quality/complaint";
                menu.ComponentPath = "logistics/quality/complaint";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLQ3;
            updateCount += updateLQ3;
        }

        // ========== 客户服务下的三级菜单 (LOGISTICS_SERVICE，不含客诉；客诉见 LOGISTICS_QUALITY_COMPLAINT) ==========
        if (logisticsServiceMenu != null)
        {
            var (insertLS1, updateLS1) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_SERVICE_REQUEST", menu =>
            {
                menu.MenuName = "服务请求";
                menu.MenuCode = "LOGISTICS_SERVICE_REQUEST";
                menu.I18nKey = "menu.logistics.service.request";
                menu.Icon = "RiQuestionAnswerLine";
                menu.ParentId = logisticsServiceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:service:request:list";
                menu.RoutePath = "/logistics/service/service-request";
                menu.ComponentPath = "logistics/service/service-request/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLS1;
            updateCount += updateLS1;

            var (insertLS2, updateLS2) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_SERVICE_CONTRACT", menu =>
            {
                menu.MenuName = "服务合同";
                menu.MenuCode = "LOGISTICS_SERVICE_CONTRACT";
                menu.I18nKey = "menu.logistics.service.contract";
                menu.Icon = "RiFileTextLine";
                menu.ParentId = logisticsServiceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:service:contract:list";
                menu.RoutePath = "/logistics/service/service-contract";
                menu.ComponentPath = "logistics/service/service-contract/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLS2;
            updateCount += updateLS2;

            var (insertLS3, updateLS3) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_SERVICE_ORDER", menu =>
            {
                menu.MenuName = "服务订单";
                menu.MenuCode = "LOGISTICS_SERVICE_ORDER";
                menu.I18nKey = "menu.logistics.service.order";
                menu.Icon = "RiFileList3Line";
                menu.ParentId = logisticsServiceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:service:order:list";
                menu.RoutePath = "/logistics/service/service-order";
                menu.ComponentPath = "logistics/service/service-order/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLS3;
            updateCount += updateLS3;

            var (insertLS4, updateLS4) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_SERVICE_TICKET", menu =>
            {
                menu.MenuName = "服务工单";
                menu.MenuCode = "LOGISTICS_SERVICE_TICKET";
                menu.I18nKey = "menu.logistics.service.ticket";
                menu.Icon = "RiTicketLine";
                menu.ParentId = logisticsServiceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:service:ticket:list";
                menu.RoutePath = "/logistics/service/service-ticket";
                menu.ComponentPath = "logistics/service/service-ticket/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLS4;
            updateCount += updateLS4;
        }

        // ========== 工厂维护下的三级菜单 (LOGISTICS_MAINTENANCE) ==========
        if (logisticsMaintenanceMenu != null)
        {
            var (insertLM1, updateLM1) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_MAINTENANCE_EQUIPMENT", menu =>
            {
                menu.MenuName = "设备信息";
                menu.MenuCode = "LOGISTICS_MAINTENANCE_EQUIPMENT";
                menu.I18nKey = "menu.logistics.maintenance.equipment";
                menu.Icon = "RiCpuLine";
                menu.ParentId = logisticsMaintenanceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:maintenance:equipment:list";
                menu.RoutePath = "/logistics/maintenance/equipment";
                menu.ComponentPath = "logistics/maintenance/equipment/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLM1;
            updateCount += updateLM1;

            var (insertLM2, updateLM2) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_MAINTENANCE_NOTIFICATION", menu =>
            {
                menu.MenuName = "维护通知";
                menu.MenuCode = "LOGISTICS_MAINTENANCE_NOTIFICATION";
                menu.I18nKey = "menu.logistics.maintenance.notification";
                menu.Icon = "RiNotificationLine";
                menu.ParentId = logisticsMaintenanceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:maintenance:notification:list";
                menu.RoutePath = "/logistics/maintenance/notification";
                menu.ComponentPath = "logistics/maintenance/notification/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLM2;
            updateCount += updateLM2;

            var (insertLM3, updateLM3) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_MAINTENANCE_WORKORDER", menu =>
            {
                menu.MenuName = "维护工单";
                menu.MenuCode = "LOGISTICS_MAINTENANCE_WORKORDER";
                menu.I18nKey = "menu.logistics.maintenance.workorder";
                menu.Icon = "RiToolsLine";
                menu.ParentId = logisticsMaintenanceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:maintenance:workorder:list";
                menu.RoutePath = "/logistics/maintenance/work-order";
                menu.ComponentPath = "logistics/maintenance/work-order/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLM3;
            updateCount += updateLM3;

            var (insertLM4, updateLM4) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_MAINTENANCE_HISTORY", menu =>
            {
                menu.MenuName = "维护履历";
                menu.MenuCode = "LOGISTICS_MAINTENANCE_HISTORY";
                menu.I18nKey = "menu.logistics.maintenance.history";
                menu.Icon = "RiHistoryLine";
                menu.ParentId = logisticsMaintenanceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:maintenance:history:list";
                menu.RoutePath = "/logistics/maintenance/history";
                menu.ComponentPath = "logistics/maintenance/history/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLM4;
            updateCount += updateLM4;
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
                menu.Icon = "RiContactsLine";
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

            var (insertLS3Cl, updateLS3Cl) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_SALES_QUOTATION_CHANGE_LOG", menu =>
            {
                menu.MenuName = "销售报价变更";
                menu.MenuCode = "LOGISTICS_SALES_QUOTATION_CHANGE_LOG";
                menu.I18nKey = "menu.logistics.sales.quotation.changelog";
                menu.Icon = "RiFileHistoryLine";
                menu.ParentId = logisticsSalesMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:sales:quotation:list";
                menu.RoutePath = "/logistics/sales/quotation-change-log";
                menu.ComponentPath = "logistics/sales/quotation-change-log/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLS3Cl;
            updateCount += updateLS3Cl;

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
                menu.SortOrder = 5;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLS4;
            updateCount += updateLS4;

            var (insertLS4Cl, updateLS4Cl) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_SALES_PRICE_CHANGE_LOG", menu =>
            {
                menu.MenuName = "销售价格变更";
                menu.MenuCode = "LOGISTICS_SALES_PRICE_CHANGE_LOG";
                menu.I18nKey = "menu.logistics.sales.price.changelog";
                menu.Icon = "RiFileHistoryLine";
                menu.ParentId = logisticsSalesMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:sales:price:list";
                menu.RoutePath = "/logistics/sales/price-change-log";
                menu.ComponentPath = "logistics/sales/price-change-log/index";
                menu.SortOrder = 6;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLS4Cl;
            updateCount += updateLS4Cl;

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
                menu.SortOrder = 7;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLS5;
            updateCount += updateLS5;

            var (insertLS5Cl, updateLS5Cl) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_SALES_ORDER_CHANGE_LOG", menu =>
            {
                menu.MenuName = "销售订单变更";
                menu.MenuCode = "LOGISTICS_SALES_ORDER_CHANGE_LOG";
                menu.I18nKey = "menu.logistics.sales.order.changelog";
                menu.Icon = "RiFileHistoryLine";
                menu.ParentId = logisticsSalesMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:sales:order:list";
                menu.RoutePath = "/logistics/sales/order-change-log";
                menu.ComponentPath = "logistics/sales/order-change-log/index";
                menu.SortOrder = 8;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLS5Cl;
            updateCount += updateLS5Cl;

            var (insertLS6, updateLS6) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_SALES_INVOICE", menu =>
            {
                menu.MenuName = "销售发票";
                menu.MenuCode = "LOGISTICS_SALES_INVOICE";
                menu.I18nKey = "menu.logistics.sales.invoice";
                menu.Icon = "RiBillLine";
                menu.ParentId = logisticsSalesMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:sales:invoice:list";
                menu.RoutePath = "/logistics/sales/sales-invoice";
                menu.ComponentPath = "logistics/sales/sales-invoice/index";
                menu.SortOrder = 9;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertLS6;
            updateCount += updateLS6;
        }

        // ========== 序列号管理下的三级菜单 (LOGISTICS_SERIAL) ==========
        if (logisticsSerialMenu != null)
        {
            var (insertSER1, updateSER1) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_SERIAL_INBOUND", menu =>
            {
                menu.MenuName = "序列号入库";
                menu.MenuCode = "LOGISTICS_SERIAL_INBOUND";
                menu.I18nKey = "menu.logistics.serial.inbound";
                menu.Icon = "RiInboxArchiveLine";
                menu.ParentId = logisticsSerialMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:serial:inbound:list";
                menu.RoutePath = "/logistics/serial/inbound";
                menu.ComponentPath = "logistics/serial/inbound/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertSER1;
            updateCount += updateSER1;

            var (insertSER3, updateSER3) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "LOGISTICS_SERIAL_OUTBOUND", menu =>
            {
                menu.MenuName = "序列号出库";
                menu.MenuCode = "LOGISTICS_SERIAL_OUTBOUND";
                menu.I18nKey = "menu.logistics.serial.outbound";
                menu.Icon = "RiInboxUnarchiveLine";
                menu.ParentId = logisticsSerialMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:serial:outbound:list";
                menu.RoutePath = "/logistics/serial/outbound";
                menu.ComponentPath = "logistics/serial/outbound/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertSER3;
            updateCount += updateSER3;
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
            var (insertHRP1, updateHRP1) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMANRESOURCE_PERSONNEL_EMPLOYEE", menu =>
            {
                menu.MenuName = "员工档案";
                menu.MenuCode = "HUMANRESOURCE_PERSONNEL_EMPLOYEE";
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

            var (insertHRP2, updateHRP2) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMANRESOURCE_PERSONNEL_EMPLOYEE_CONTRACT", menu =>
            {
                menu.MenuName = "员工合同";
                menu.MenuCode = "HUMANRESOURCE_PERSONNEL_EMPLOYEE_CONTRACT";
                menu.I18nKey = "menu.humanresource.personnel.employeecontract";
                menu.Icon = "RiFilePaperLine";
                menu.ParentId = hrPersonnelMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "humanresource:personnel:employee:contract:list";
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

            var (insertHRP3, updateHRP3) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMANRESOURCE_PERSONNEL_EMPLOYEE_DELEGATE", menu =>
            {
                menu.MenuName = "员工代理";
                menu.MenuCode = "HUMANRESOURCE_PERSONNEL_EMPLOYEE_DELEGATE";
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

            var (insertHRP4, updateHRP4) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMANRESOURCE_PERSONNEL_EMPLOYEE_REASSIGNMENT", menu =>
            {
                menu.MenuName = "员工调动";
                menu.MenuCode = "HUMANRESOURCE_PERSONNEL_EMPLOYEE_REASSIGNMENT";
                menu.I18nKey = "menu.humanresource.personnel.employeereassignment";
                menu.Icon = "RiExchangeLine";
                menu.ParentId = hrPersonnelMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "humanresource:personnel:employee:reassignment:list";
                menu.RoutePath = "/human-resource/personnel/employee-reassignment";
                menu.ComponentPath = "human-resource/personnel/employee-reassignment/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRP4;
            updateCount += updateHRP4;

            var (insertHRP5, updateHRP5) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMANRESOURCE_PERSONNEL_EMPLOYEE_ONBOARDING", menu =>
            {
                menu.MenuName = "入职待办";
                menu.MenuCode = "HUMANRESOURCE_PERSONNEL_EMPLOYEE_ONBOARDING";
                menu.I18nKey = "menu.humanresource.personnel.employeeonboarding";
                menu.Icon = "RiClipboardLine";
                menu.ParentId = hrPersonnelMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "humanresource:talent:offer:list";
                menu.RoutePath = "/human-resource/personnel/employee-onboarding";
                menu.ComponentPath = "human-resource/personnel/employee-onboarding/index";
                menu.SortOrder = 5;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRP5;
            updateCount += updateHRP5;
        }

        // ========== 考勤管理下的三级菜单 (HUMANRESOURCE_ATTENDANCE，与 HumanResource/Attendance 实体及控制器对齐) ==========
        if (hrAttendanceMenu != null)
        {
            var (insertHRAL1, updateHRAL1) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMANRESOURCE_ATTENDANCE_CALENDAR", menu =>
            {
                menu.MenuName = "工厂日历";
                menu.MenuCode = "HUMANRESOURCE_ATTENDANCE_CALENDAR";
                menu.I18nKey = "menu.humanresource.attendance.calendar";
                menu.Icon = "RiCalendarLine";
                menu.ParentId = hrAttendanceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "humanresource:attendance:calendar:list";
                menu.RoutePath = "/human-resource/attendance/calendar";
                menu.ComponentPath = "human-resource/attendance/calendar/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRAL1;
            updateCount += updateHRAL1;

            var (insertHRAL2, updateHRAL2) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMANRESOURCE_ATTENDANCE_HOLIDAY", menu =>
            {
                menu.MenuName = "假期管理";
                menu.MenuCode = "HUMANRESOURCE_ATTENDANCE_HOLIDAY";
                menu.I18nKey = "menu.humanresource.attendance.holiday";
                menu.Icon = "RiCalendarEventLine";
                menu.ParentId = hrAttendanceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "humanresource:attendance:holiday:list";
                menu.RoutePath = "/human-resource/attendance/holiday";
                menu.ComponentPath = "human-resource/attendance/holiday/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRAL2;
            updateCount += updateHRAL2;

            var (insertHRAL3, updateHRAL3) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMANRESOURCE_ATTENDANCE_SHIFT_SCHEDULE", menu =>
            {
                menu.MenuName = "排班计划";
                menu.MenuCode = "HUMANRESOURCE_ATTENDANCE_SHIFT_SCHEDULE";
                menu.I18nKey = "menu.humanresource.attendance.shiftschedule";
                menu.Icon = "RiCalendarScheduleLine";
                menu.ParentId = hrAttendanceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "humanresource:attendance:shift:schedule:list";
                menu.RoutePath = "/human-resource/attendance/shift-schedule";
                menu.ComponentPath = "human-resource/attendance/shift-schedule/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRAL3;
            updateCount += updateHRAL3;

            var (insertHRAL4, updateHRAL4) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMANRESOURCE_ATTENDANCE_WORK_SHIFT", menu =>
            {
                menu.MenuName = "班次管理";
                menu.MenuCode = "HUMANRESOURCE_ATTENDANCE_WORK_SHIFT";
                menu.I18nKey = "menu.humanresource.attendance.workshift";
                menu.Icon = "RiTimeZoneLine";
                menu.ParentId = hrAttendanceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "humanresource:attendance:work:shift:list";
                menu.RoutePath = "/human-resource/attendance/work-shift";
                menu.ComponentPath = "human-resource/attendance/work-shift/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRAL4;
            updateCount += updateHRAL4;

            var (insertHRAL5, updateHRAL5) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMANRESOURCE_ATTENDANCE_LEAVE", menu =>
            {
                menu.MenuName = "请假管理";
                menu.MenuCode = "HUMANRESOURCE_ATTENDANCE_LEAVE";
                menu.I18nKey = "menu.humanresource.attendance.leave";
                menu.Icon = "RiCalendarCheckLine";
                menu.ParentId = hrAttendanceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "humanresource:attendance:leave:list";
                menu.RoutePath = "/human-resource/attendance/leave";
                menu.ComponentPath = "human-resource/attendance/leave/index";
                menu.SortOrder = 5;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRAL5;
            updateCount += updateHRAL5;

            var (insertHRAL6, updateHRAL6) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMANRESOURCE_ATTENDANCE_OVERTIME", menu =>
            {
                menu.MenuName = "加班管理";
                menu.MenuCode = "HUMANRESOURCE_ATTENDANCE_OVERTIME";
                menu.I18nKey = "menu.humanresource.attendance.overtime";
                menu.Icon = "RiTimeLine";
                menu.ParentId = hrAttendanceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "humanresource:attendance:overtime:list";
                menu.RoutePath = "/human-resource/attendance/overtime";
                menu.ComponentPath = "human-resource/attendance/overtime/index";
                menu.SortOrder = 6;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRAL6;
            updateCount += updateHRAL6;
        }
        // ========== 薪酬管理下的三级菜单 (HUMANRESOURCE_COMPENSATION，与 Compensation 实体对齐) ==========
        if (hrCompensationMenu != null)
        {
            var (insertHRC1, updateHRC1) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMANRESOURCE_COMPENSATION_SALARY_ITEM", menu =>
            {
                menu.MenuName = "薪资项目";
                menu.MenuCode = "HUMANRESOURCE_COMPENSATION_SALARY_ITEM";
                menu.I18nKey = "menu.humanresource.compensation.salaryitem";
                menu.Icon = "RiPriceTag3Line";
                menu.ParentId = hrCompensationMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "humanresource:compensation:salary:item:list";
                menu.RoutePath = "/human-resource/compensation/salary-item";
                menu.ComponentPath = "human-resource/compensation/salary-item/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRC1;
            updateCount += updateHRC1;

            var (insertHRC2, updateHRC2) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMANRESOURCE_COMPENSATION_PAYROLL", menu =>
            {
                menu.MenuName = "薪酬体系";
                menu.MenuCode = "HUMANRESOURCE_COMPENSATION_PAYROLL";
                menu.I18nKey = "menu.humanresource.compensation.payroll";
                menu.Icon = "RiFileList3Line";
                menu.ParentId = hrCompensationMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "humanresource:compensation:payroll:list";
                menu.RoutePath = "/human-resource/compensation/payroll";
                menu.ComponentPath = "human-resource/compensation/payroll/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRC2;
            updateCount += updateHRC2;

            var (insertHRC3, updateHRC3) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMANRESOURCE_COMPENSATION_PAY_SCALE", menu =>
            {
                menu.MenuName = "薪级";
                menu.MenuCode = "HUMANRESOURCE_COMPENSATION_PAY_SCALE";
                menu.I18nKey = "menu.humanresource.compensation.payscale";
                menu.Icon = "RiStackLine";
                menu.ParentId = hrCompensationMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "humanresource:compensation:pay:scale:list";
                menu.RoutePath = "/human-resource/compensation/pay-scale";
                menu.ComponentPath = "human-resource/compensation/pay-scale/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRC3;
            updateCount += updateHRC3;

            var (insertHRC4, updateHRC4) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMANRESOURCE_COMPENSATION_EMP_SALARY", menu =>
            {
                menu.MenuName = "员工定薪";
                menu.MenuCode = "HUMANRESOURCE_COMPENSATION_EMP_SALARY";
                menu.I18nKey = "menu.humanresource.compensation.empsalary";
                menu.Icon = "RiUserSettingsLine";
                menu.ParentId = hrCompensationMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "humanresource:compensation:emp:salary:list";
                menu.RoutePath = "/human-resource/compensation/emp-salary";
                menu.ComponentPath = "human-resource/compensation/emp-salary/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRC4;
            updateCount += updateHRC4;

            var (insertHRC5, updateHRC5) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMANRESOURCE_COMPENSATION_BONUS_PLAN", menu =>
            {
                menu.MenuName = "奖金方案";
                menu.MenuCode = "HUMANRESOURCE_COMPENSATION_BONUS_PLAN";
                menu.I18nKey = "menu.humanresource.compensation.bonusplan";
                menu.Icon = "RiAwardLine";
                menu.ParentId = hrCompensationMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "humanresource:compensation:bonus:plan:list";
                menu.RoutePath = "/human-resource/compensation/bonus-plan";
                menu.ComponentPath = "human-resource/compensation/bonus-plan/index";
                menu.SortOrder = 5;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRC5;
            updateCount += updateHRC5;

            var (insertHRC6, updateHRC6) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMANRESOURCE_COMPENSATION_SALARY_FORMULA", menu =>
            {
                menu.MenuName = "薪资计算公式";
                menu.MenuCode = "HUMANRESOURCE_COMPENSATION_SALARY_FORMULA";
                menu.I18nKey = "menu.humanresource.compensation.salaryformula";
                menu.Icon = "RiFunctionsLine";
                menu.ParentId = hrCompensationMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "humanresource:compensation:salary:formula:list";
                menu.RoutePath = "/human-resource/compensation/salary-formula";
                menu.ComponentPath = "human-resource/compensation/salary-formula/index";
                menu.SortOrder = 6;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRC6;
            updateCount += updateHRC6;

            var (insertHRC7, updateHRC7) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMANRESOURCE_COMPENSATION_PAYSLIP", menu =>
            {
                menu.MenuName = "工资条";
                menu.MenuCode = "HUMANRESOURCE_COMPENSATION_PAYSLIP";
                menu.I18nKey = "menu.humanresource.compensation.payslip";
                menu.Icon = "RiBillLine";
                menu.ParentId = hrCompensationMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "humanresource:compensation:payslip:list";
                menu.RoutePath = "/human-resource/compensation/payslip";
                menu.ComponentPath = "human-resource/compensation/payslip/index";
                menu.SortOrder = 7;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRC7;
            updateCount += updateHRC7;
        }

        // ========== 福利管理下的三级菜单 (HUMANRESOURCE_BENEFITS，与 Benefits 实体对齐) ==========
        if (hrBenefitsMenu != null)
        {
            var (insertHRB1, updateHRB1) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMANRESOURCE_BENEFITS_BENEFIT_ITEM", menu =>
            {
                menu.MenuName = "福利项目";
                menu.MenuCode = "HUMANRESOURCE_BENEFITS_BENEFIT_ITEM";
                menu.I18nKey = "menu.humanresource.benefits.benefititem";
                menu.Icon = "RiGiftLine";
                menu.ParentId = hrBenefitsMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "humanresource:benefits:benefit:item:list";
                menu.RoutePath = "/human-resource/benefits/benefit-item";
                menu.ComponentPath = "human-resource/benefits/benefit-item/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRB1;
            updateCount += updateHRB1;

            var (insertHRB2, updateHRB2) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMANRESOURCE_BENEFITS_EMP_BENEFIT_PLAN", menu =>
            {
                menu.MenuName = "员工福利方案";
                menu.MenuCode = "HUMANRESOURCE_BENEFITS_EMP_BENEFIT_PLAN";
                menu.I18nKey = "menu.humanresource.benefits.empbenefitplan";
                menu.Icon = "RiUserHeartLine";
                menu.ParentId = hrBenefitsMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "humanresource:benefits:emp:benefit:plan:list";
                menu.RoutePath = "/human-resource/benefits/emp-benefit-plan";
                menu.ComponentPath = "human-resource/benefits/emp-benefit-plan/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRB2;
            updateCount += updateHRB2;

            var (insertHRB3, updateHRB3) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMANRESOURCE_BENEFITS_SOCIAL_INSURANCE", menu =>
            {
                menu.MenuName = "社保公积金";
                menu.MenuCode = "HUMANRESOURCE_BENEFITS_SOCIAL_INSURANCE";
                menu.I18nKey = "menu.humanresource.benefits.socialinsurance";
                menu.Icon = "RiShieldCheckLine";
                menu.ParentId = hrBenefitsMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "humanresource:benefits:social:insurance:list";
                menu.RoutePath = "/human-resource/benefits/social-insurance";
                menu.ComponentPath = "human-resource/benefits/social-insurance/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRB3;
            updateCount += updateHRB3;
        }

        // ========== 绩效管理下的三级菜单 (HUMANRESOURCE_PERFORMANCE，与 5 个实体对齐) ==========
        if (hrPerformanceMenu != null)
        {
            var (insertHRP1, updateHRP1) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMANRESOURCE_PERFORMANCE_PERF_CYCLE", menu =>
            {
                menu.MenuName = "绩效周期";
                menu.MenuCode = "HUMANRESOURCE_PERFORMANCE_PERF_CYCLE";
                menu.I18nKey = "menu.humanresource.performance.perfcycle";
                menu.Icon = "RiCalendarLine";
                menu.ParentId = hrPerformanceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "humanresource:performance:perf:cycle:list";
                menu.RoutePath = "/human-resource/performance/perf-cycle";
                menu.ComponentPath = "human-resource/performance/perf-cycle/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRP1;
            updateCount += updateHRP1;

            var (insertHRP2, updateHRP2) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMANRESOURCE_PERFORMANCE_PERF_SCHEME", menu =>
            {
                menu.MenuName = "绩效方案";
                menu.MenuCode = "HUMANRESOURCE_PERFORMANCE_PERF_SCHEME";
                menu.I18nKey = "menu.humanresource.performance.perfscheme";
                menu.Icon = "RiFileChartLine";
                menu.ParentId = hrPerformanceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "humanresource:performance:perf:scheme:list";
                menu.RoutePath = "/human-resource/performance/perf-scheme";
                menu.ComponentPath = "human-resource/performance/perf-scheme/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRP2;
            updateCount += updateHRP2;

            var (insertHRP3, updateHRP3) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMANRESOURCE_PERFORMANCE_PERF_OBJECTIVE", menu =>
            {
                menu.MenuName = "绩效目标";
                menu.MenuCode = "HUMANRESOURCE_PERFORMANCE_PERF_OBJECTIVE";
                menu.I18nKey = "menu.humanresource.performance.perfobjective";
                menu.Icon = "RiTargetLine";
                menu.ParentId = hrPerformanceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "humanresource:performance:perf:objective:list";
                menu.RoutePath = "/human-resource/performance/perf-objective";
                menu.ComponentPath = "human-resource/performance/perf-objective/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRP3;
            updateCount += updateHRP3;

            var (insertHRP4, updateHRP4) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMANRESOURCE_PERFORMANCE_PERF_ASSESSMENT", menu =>
            {
                menu.MenuName = "绩效考核";
                menu.MenuCode = "HUMANRESOURCE_PERFORMANCE_PERF_ASSESSMENT";
                menu.I18nKey = "menu.humanresource.performance.perfassessment";
                menu.Icon = "RiClipboardLine";
                menu.ParentId = hrPerformanceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "humanresource:performance:perf:assessment:list";
                menu.RoutePath = "/human-resource/performance/perf-assessment";
                menu.ComponentPath = "human-resource/performance/perf-assessment/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRP4;
            updateCount += updateHRP4;

            var (insertHRP5, updateHRP5) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMANRESOURCE_PERFORMANCE_PERF_ANALYSIS", menu =>
            {
                menu.MenuName = "分析改进";
                menu.MenuCode = "HUMANRESOURCE_PERFORMANCE_PERF_ANALYSIS";
                menu.I18nKey = "menu.humanresource.performance.perfanalysis";
                menu.Icon = "RiLightbulbLine";
                menu.ParentId = hrPerformanceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "humanresource:performance:perf:analysis:list";
                menu.RoutePath = "/human-resource/performance/perf-analysis";
                menu.ComponentPath = "human-resource/performance/perf-analysis/index";
                menu.SortOrder = 5;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRP5;
            updateCount += updateHRP5;
        }

        // ========== 教育培训下的三级菜单 (HUMANRESOURCE_TRAINING，与 TrainingCourse / TrainingPlan / TrainingAttendee 对齐) ==========
        if (hrTrainingMenu != null)
        {
            var (insertHRT1, updateHRT1) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMANRESOURCE_TRAINING_COURSE", menu =>
            {
                menu.MenuName = "培训课程";
                menu.MenuCode = "HUMANRESOURCE_TRAINING_COURSE";
                menu.I18nKey = "menu.humanresource.training.course";
                menu.Icon = "RiBookOpenLine";
                menu.ParentId = hrTrainingMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "humanresource:training:course:list";
                menu.RoutePath = "/human-resource/training/training-course";
                menu.ComponentPath = "human-resource/training/training-course/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRT1;
            updateCount += updateHRT1;

            var (insertHRT2, updateHRT2) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMANRESOURCE_TRAINING_PLAN", menu =>
            {
                menu.MenuName = "年度计划";
                menu.MenuCode = "HUMANRESOURCE_TRAINING_PLAN";
                menu.I18nKey = "menu.humanresource.training.plan";
                menu.Icon = "RiCalendarScheduleLine";
                menu.ParentId = hrTrainingMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "humanresource:training:plan:list";
                menu.RoutePath = "/human-resource/training/training-plan";
                menu.ComponentPath = "human-resource/training/training-plan/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRT2;
            updateCount += updateHRT2;

            var (insertHRT3, updateHRT3) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "HUMANRESOURCE_TRAINING_ATTENDEE", menu =>
            {
                menu.MenuName = "参训记录";
                menu.MenuCode = "HUMANRESOURCE_TRAINING_ATTENDEE";
                menu.I18nKey = "menu.humanresource.training.attendee";
                menu.Icon = "RiUserFollowLine";
                menu.ParentId = hrTrainingMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "humanresource:training:attendee:list";
                menu.RoutePath = "/human-resource/training/training-attendee";
                menu.ComponentPath = "human-resource/training/training-attendee/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHRT3;
            updateCount += updateHRT3;
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
                menu.SortOrder = 4;
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
                menu.SortOrder = 5;
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
            var (insertSR1, updateSR1) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "STATISTICS_REPORT_CONFIGURABLE", menu =>
            {
                menu.MenuName = "SQVI报表";
                menu.MenuCode = "STATISTICS_REPORT_CONFIGURABLE";
                menu.I18nKey = "menu.statistics.report.configurable";
                menu.Icon = "RiFileChartLine";
                menu.ParentId = statisticsReportMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "statistics:report:configurable:list";
                menu.RoutePath = "/statistics/report/configurable";
                menu.ComponentPath = "statistics/report/configurable/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertSR1;
            updateCount += updateSR1;
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
                menu.Permission = "statistics:logging:login:log:list";
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
                menu.Permission = "statistics:logging:oper:log:list";
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
                menu.Permission = "statistics:logging:delta:log:list";
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
                menu.Permission = "statistics:logging:quartz:log:list";
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
                menu.Permission = "statistics:logging:server:monitor:list";
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

        // ========== 服务台下的三级菜单 (ROUTINE_HELPDESK) ==========
        if (routineHelpDeskMenu != null)
        {
            var (insertHd1, updateHd1) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "ROUTINE_HELP_DESK_MY_TICKET", menu =>
            {
                menu.MenuName = "我的工单";
                menu.MenuCode = "ROUTINE_HELP_DESK_MY_TICKET";
                menu.I18nKey = "menu.routine.helpdesk.myticket";
                menu.Icon = "RiTicketLine";
                menu.ParentId = routineHelpDeskMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "routine:helpdesk:myticket:list";
                menu.RoutePath = "/routine/help-desk/my-ticket";
                menu.ComponentPath = "routine/help-desk/my-ticket/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHd1;
            updateCount += updateHd1;

            var (insertHd2, updateHd2) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "ROUTINE_HELP_DESK_TICKET", menu =>
            {
                menu.MenuName = "工单管理";
                menu.MenuCode = "ROUTINE_HELP_DESK_TICKET";
                menu.I18nKey = "menu.routine.helpdesk.ticket";
                menu.Icon = "RiCustomerService2Line";
                menu.ParentId = routineHelpDeskMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "routine:helpdesk:ticket:list";
                menu.RoutePath = "/routine/help-desk/ticket";
                menu.ComponentPath = "routine/help-desk/ticket/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHd2;
            updateCount += updateHd2;

            var (insertHd2Cl, updateHd2Cl) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "ROUTINE_HELP_DESK_TICKET_CHANGE_LOG", menu =>
            {
                menu.MenuName = "工单变更";
                menu.MenuCode = "ROUTINE_HELP_DESK_TICKET_CHANGE_LOG";
                menu.I18nKey = "menu.routine.helpdesk.ticket.changelog";
                menu.Icon = "RiFileHistoryLine";
                menu.ParentId = routineHelpDeskMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "routine:helpdesk:ticket:list";
                menu.RoutePath = "/routine/help-desk/ticket-change-log";
                menu.ComponentPath = "routine/help-desk/ticket-change-log/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHd2Cl;
            updateCount += updateHd2Cl;

            var (insertHd3, updateHd3) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "ROUTINE_HELP_DESK_KNOWLEDGE", menu =>
            {
                menu.MenuName = "知识库（FAQ）";
                menu.MenuCode = "ROUTINE_HELP_DESK_KNOWLEDGE";
                menu.I18nKey = "menu.routine.helpdesk.knowledge";
                menu.Icon = "RiBookOpenLine";
                menu.ParentId = routineHelpDeskMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "routine:helpdesk:knowledge:list";
                menu.RoutePath = "/routine/help-desk/knowledge";
                menu.ComponentPath = "routine/help-desk/knowledge/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHd3;
            updateCount += updateHd3;

            var (insertHd3Cl, updateHd3Cl) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "ROUTINE_HELP_DESK_KNOWLEDGE_CHANGE_LOG", menu =>
            {
                menu.MenuName = "知识库变更";
                menu.MenuCode = "ROUTINE_HELP_DESK_KNOWLEDGE_CHANGE_LOG";
                menu.I18nKey = "menu.routine.helpdesk.knowledge.changelog";
                menu.Icon = "RiFileHistoryLine";
                menu.ParentId = routineHelpDeskMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "routine:helpdesk:knowledge:list";
                menu.RoutePath = "/routine/help-desk/knowledge-change-log";
                menu.ComponentPath = "routine/help-desk/knowledge-change-log/index";
                menu.SortOrder = 5;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHd3Cl;
            updateCount += updateHd3Cl;

            var (insertHd4, updateHd4) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "ROUTINE_HELP_DESK_MY_ASSET", menu =>
            {
                menu.MenuName = "我的资产";
                menu.MenuCode = "ROUTINE_HELP_DESK_MY_ASSET";
                menu.I18nKey = "menu.routine.helpdesk.myasset";
                menu.Icon = "RiDeviceLine";
                menu.ParentId = routineHelpDeskMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "routine:helpdesk:myasset:list";
                menu.RoutePath = "/routine/help-desk/my-asset";
                menu.ComponentPath = "routine/help-desk/my-asset/index";
                menu.SortOrder = 6;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHd4;
            updateCount += updateHd4;

            var (insertHd5, updateHd5) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "ROUTINE_HELP_DESK_IT_ASSET", menu =>
            {
                menu.MenuName = "IT设备保修";
                menu.MenuCode = "ROUTINE_HELP_DESK_IT_ASSET";
                menu.I18nKey = "menu.routine.helpdesk.itasset";
                menu.Icon = "RiDeviceLine";
                menu.ParentId = routineHelpDeskMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "routine:helpdesk:it:asset:list";
                menu.RoutePath = "/routine/help-desk/it-asset";
                menu.ComponentPath = "routine/help-desk/it-asset/index";
                menu.SortOrder = 7;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHd5;
            updateCount += updateHd5;

            var (insertHd5Cl, updateHd5Cl) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "ROUTINE_HELP_DESK_IT_ASSET_CHANGE_LOG", menu =>
            {
                menu.MenuName = "IT设备保修变更";
                menu.MenuCode = "ROUTINE_HELP_DESK_IT_ASSET_CHANGE_LOG";
                menu.I18nKey = "menu.routine.helpdesk.itasset.changelog";
                menu.Icon = "RiFileHistoryLine";
                menu.ParentId = routineHelpDeskMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "routine:helpdesk:it:asset:list";
                menu.RoutePath = "/routine/help-desk/it-asset-change-log";
                menu.ComponentPath = "routine/help-desk/it-asset-change-log/index";
                menu.SortOrder = 8;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertHd5Cl;
            updateCount += updateHd5Cl;
        }

        // ========== 文管中心下的三级菜单 (ROUTINE_DOCUMENT_CENTER) ==========
        if (routineDocumentCenterMenu != null)
        {
            var (insertDc1, updateDc1) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "ROUTINE_DOCUMENT_CENTER_DOCUMENT", menu =>
            {
                menu.MenuName = "文档管理";
                menu.MenuCode = "ROUTINE_DOCUMENT_CENTER_DOCUMENT";
                menu.I18nKey = "menu.routine.documentcenter.document";
                menu.Icon = "RiFileTextLine";
                menu.ParentId = routineDocumentCenterMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "routine:documentcenter:document:list";
                menu.RoutePath = "/routine/document-center/document";
                menu.ComponentPath = "routine/document-center/document/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertDc1;
            updateCount += updateDc1;

            var (insertDc1Cl, updateDc1Cl) = await CreateOrUpdateMenuAsync(menuRepository, seedContext, tenantCode, "ROUTINE_DOCUMENT_CENTER_DOCUMENT_CHANGE_LOG", menu =>
            {
                menu.MenuName = "文档变更";
                menu.MenuCode = "ROUTINE_DOCUMENT_CENTER_DOCUMENT_CHANGE_LOG";
                menu.I18nKey = "menu.routine.documentcenter.document.changelog";
                menu.Icon = "RiFileHistoryLine";
                menu.ParentId = routineDocumentCenterMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "routine:documentcenter:document:list";
                menu.RoutePath = "/routine/document-center/document-change-log";
                menu.ComponentPath = "routine/document-center/document-change-log/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertDc1Cl;
            updateCount += updateDc1Cl;
        }

        return (insertCount, updateCount);
    }

    /// <summary>
    /// 创建或更新菜单。
    /// </summary>
    /// <param name="menuRepository">菜单仓储。</param>
    /// <param name="seedContext">数据库上下文。</param>
    /// <param name="tenantCode">租户编码。</param>
    /// <param name="menuCode">菜单编码（业务键）。</param>
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
            menu.IsBuiltIn = 1;
            
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
            menu.IsBuiltIn = 1;
            
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
