// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds
// 文件名称：TaktMenuLevel4SeedData.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt 四级菜单种子数据。
//           在三级菜单已存在的前提下，主要扩展生产制造（BOM/排程/设变/产出/不良）等更细页面。
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
/// Takt 四级菜单种子数据。
/// <para>
/// 父级通常来自 TaktMenuLevel3SeedData 中的 BOM、排程、设变、产出、不良等三级节点。
/// 由 TaktMenuSeedData 统一协调调用，不直接注册为 ITaktSeedDataCoordinator。
/// </para>
/// </summary>
public class TaktMenuLevel4SeedData
{
    /// <summary>
    /// 初始化四级菜单种子数据。
    /// <para>
    /// 写入 BOM 子项、排程子项、设变相关部门视图、产出与不良下的 PCBA/Assembly 目录等。
    /// </para>
    /// </summary>
    /// <param name="serviceProvider">服务提供者，用于解析 ITaktRepository{TaktMenu}。</param>
    /// <param name="specifiedTenantCode">租户编码（由协调器传入）。</param>
    /// <returns>元组：(InsertCount, UpdateCount)，分别为本次新增与更新的四级菜单条数。</returns>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(IServiceProvider serviceProvider, string? specifiedTenantCode = null)
    {
        var menuRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktMenu>>();
        var sqlSugarContext = serviceProvider.GetRequiredService<TaktSeedContext>();

        // 四级菜单:基于三级菜单的ParentId
        // 注意:菜单为租户级实体,由协调器指定租户,必须传入租户编码
        if (string.IsNullOrWhiteSpace(specifiedTenantCode))
        {
            TaktLogger.Warning("未指定租户编码,跳过四级菜单种子数据初始化");
            return (0, 0);
        }
        
        var tenantCode = specifiedTenantCode;

        int insertCount = 0;
        int updateCount = 0;

        // 获取三级父菜单(使用仓储查询,自动应用租户过滤)
        // 注意:三级菜单已在 TaktMenuLevel3SeedData 中初始化
        var manufacturingBomMenu = await menuRepository.FirstAsync(m => m.MenuCode == "LOGISTICS_MANUFACTURING_BOM");
        var manufacturingPlanningMenu = await menuRepository.FirstAsync(m => m.MenuCode == "LOGISTICS_MANUFACTURING_PLANNING");
        var manufacturingSchedulingMenu = await menuRepository.FirstAsync(m => m.MenuCode == "LOGISTICS_MANUFACTURING_SCHEDULING");
        var manufacturingEngineeringChangeMenu = await menuRepository.FirstAsync(m => m.MenuCode == "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE");
        var manufacturingOutputMenu = await menuRepository.FirstAsync(m => m.MenuCode == "LOGISTICS_MANUFACTURING_OUTPUT");
        var manufacturingDefectMenu = await menuRepository.FirstAsync(m => m.MenuCode == "LOGISTICS_MANUFACTURING_DEFECT");
        var manufacturingSopMenu = await menuRepository.FirstAsync(m =>
            m.MenuCode == "LOGISTICS_MANUFACTURING_SOP" || m.MenuCode == "LOGISTICS_MANUFACTURING_ESOP");
        var qualityCostMenu = await menuRepository.FirstAsync(m => m.MenuCode == "LOGISTICS_QUALITY_COST");
        var qualityAssuranceMenu = await menuRepository.FirstAsync(m => m.MenuCode == "LOGISTICS_QUALITY_OPERATION");
        var qualityComplaintMenu = await menuRepository.FirstAsync(m => m.MenuCode == "LOGISTICS_QUALITY_COMPLAINT");

        // ========== BOM 下的四级菜单 ==========
        if (manufacturingBomMenu != null)
        {
            var (insertBOM2, updateBOM2) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_BOM_BILL_OF_MATERIAL", menu =>
            {
                menu.MenuName = "物料清单";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_BOM_BILL_OF_MATERIAL";
                menu.I18nKey = "menu.logistics.manufacturing.bom.bill.of.material";
                menu.Icon = "RiFileList2Line";
                menu.ParentId = manufacturingBomMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:bom:bill:of:material:list";
                menu.RoutePath = "/logistics/manufacturing/bom/bill-of-material";
                menu.ComponentPath = "logistics/manufacturing/bom/bill-of-material/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertBOM2;
            updateCount += updateBOM2;

            var (insertBOM2Cl, updateBOM2Cl) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_BOM_BILL_OF_MATERIAL_CHANGE_LOG", menu =>
            {
                menu.MenuName = "物料清单变更";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_BOM_BILL_OF_MATERIAL_CHANGE_LOG";
                menu.I18nKey = "menu.logistics.manufacturing.bom.bill.of.material.change.log";
                menu.Icon = "RiFileHistoryLine";
                menu.ParentId = manufacturingBomMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:bom:bill:of:material:change:log:list";
                menu.RoutePath = "/logistics/manufacturing/bom/bill-of-material-change-log";
                menu.ComponentPath = "logistics/manufacturing/bom/bill-of-material-change-log/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertBOM2Cl;
            updateCount += updateBOM2Cl;

            var (insertBOM5, updateBOM5) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_BOM_ROUTING", menu =>
            {
                menu.MenuName = "工艺路线";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_BOM_ROUTING";
                menu.I18nKey = "menu.logistics.manufacturing.bom.routing";
                menu.Icon = "RiRouteLine";
                menu.ParentId = manufacturingBomMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:bom:routing:list";
                menu.RoutePath = "/logistics/manufacturing/bom/routing";
                menu.ComponentPath = "logistics/manufacturing/bom/routing/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertBOM5;
            updateCount += updateBOM5;

            var (insertBOM5Cl, updateBOM5Cl) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_BOM_ROUTING_CHANGE_LOG", menu =>
            {
                menu.MenuName = "工艺路线变更";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_BOM_ROUTING_CHANGE_LOG";
                menu.I18nKey = "menu.logistics.manufacturing.bom.routing.change.log";
                menu.Icon = "RiFileHistoryLine";
                menu.ParentId = manufacturingBomMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:bom:routing:change:log:list";
                menu.RoutePath = "/logistics/manufacturing/bom/routing-change-log";
                menu.ComponentPath = "logistics/manufacturing/bom/routing-change-log/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertBOM5Cl;
            updateCount += updateBOM5Cl;

            var (insertBOM9, updateBOM9) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_BOM_STANDARD_OPERATION_TIME", menu =>
            {
                menu.MenuName = "标准工序时间";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_BOM_STANDARD_OPERATION_TIME";
                menu.I18nKey = "menu.logistics.manufacturing.bom.standard.operation.time";
                menu.Icon = "RiTimerLine";
                menu.ParentId = manufacturingBomMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:bom:standard:operation:time:list";
                menu.RoutePath = "/logistics/manufacturing/bom/standard-operation-time";
                menu.ComponentPath = "logistics/manufacturing/bom/standard-operation-time/index";
                menu.SortOrder = 6;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertBOM9;
            updateCount += updateBOM9;

            var (insertBOM9Cl, updateBOM9Cl) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_BOM_STANDARD_OPERATION_TIME_CHANGE_LOG", menu =>
            {
                menu.MenuName = "标准工序时间变更";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_BOM_STANDARD_OPERATION_TIME_CHANGE_LOG";
                menu.I18nKey = "menu.logistics.manufacturing.bom.standard.operation.time.change.log";
                menu.Icon = "RiFileHistoryLine";
                menu.ParentId = manufacturingBomMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:bom:standard:operation:time:change:log:list";
                menu.RoutePath = "/logistics/manufacturing/bom/standard-operation-time-change-log";
                menu.ComponentPath = "logistics/manufacturing/bom/standard-operation-time-change-log/index";
                menu.SortOrder = 7;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertBOM9Cl;
            updateCount += updateBOM9Cl;
        }

        // ========== MRP计划下的四级菜单 ==========
        if (manufacturingPlanningMenu != null)
        {
            var (insertPLN0, updatePLN0) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_PLANNING_MASTER_DEMAND_SCHEDULE", menu =>
            {
                menu.MenuName = "主需求计划";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_PLANNING_MASTER_DEMAND_SCHEDULE";
                menu.I18nKey = "menu.logistics.manufacturing.planning.master.demand.schedule";
                menu.Icon = "RiBarChartGroupedLine";
                menu.ParentId = manufacturingPlanningMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:planning:master:demand:schedule:list";
                menu.RoutePath = "/logistics/manufacturing/planning/master-demand-schedule";
                menu.ComponentPath = "logistics/manufacturing/planning/master-demand-schedule/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertPLN0;
            updateCount += updatePLN0;

            var (insertPLN0b, updatePLN0b) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_PLANNING_MASTER_PRODUCTION_SCHEDULE", menu =>
            {
                menu.MenuName = "主生产计划";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_PLANNING_MASTER_PRODUCTION_SCHEDULE";
                menu.I18nKey = "menu.logistics.manufacturing.planning.master.production.schedule";
                menu.Icon = "RiCalendarTodoLine";
                menu.ParentId = manufacturingPlanningMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:planning:master:production:schedule:list";
                menu.RoutePath = "/logistics/manufacturing/planning/master-production-schedule";
                menu.ComponentPath = "logistics/manufacturing/planning/master-production-schedule/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertPLN0b;
            updateCount += updatePLN0b;

            var (insertPLN0c, updatePLN0c) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_PLANNING_PLANNED_ORDER", menu =>
            {
                menu.MenuName = "计划订单";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_PLANNING_PLANNED_ORDER";
                menu.I18nKey = "menu.logistics.manufacturing.planning.planned.order";
                menu.Icon = "RiFilePaper2Line";
                menu.ParentId = manufacturingPlanningMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:planning:planned:order:list";
                menu.RoutePath = "/logistics/manufacturing/planning/planned-order";
                menu.ComponentPath = "logistics/manufacturing/planning/planned-order/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertPLN0c;
            updateCount += updatePLN0c;

            var (insertPLN1, updatePLN1) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_PLANNING_SALES_PLAN", menu =>
            {
                menu.MenuName = "销售计划";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_PLANNING_SALES_PLAN";
                menu.I18nKey = "menu.logistics.manufacturing.planning.sales.plan";
                menu.Icon = "RiLineChartLine";
                menu.ParentId = manufacturingPlanningMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:planning:sales:plan:list";
                menu.RoutePath = "/logistics/manufacturing/planning/sales-plan";
                menu.ComponentPath = "logistics/manufacturing/planning/sales-plan/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertPLN1;
            updateCount += updatePLN1;

            var (insertPLN2, updatePLN2) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_PLANNING_PRODUCTION_PLAN", menu =>
            {
                menu.MenuName = "生产计划";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_PLANNING_PRODUCTION_PLAN";
                menu.I18nKey = "menu.logistics.manufacturing.planning.production.plan";
                menu.Icon = "RiCalendarCheckLine";
                menu.ParentId = manufacturingPlanningMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:planning:production:plan:list";
                menu.RoutePath = "/logistics/manufacturing/planning/production-plan";
                menu.ComponentPath = "logistics/manufacturing/planning/production-plan/index";
                menu.SortOrder = 5;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertPLN2;
            updateCount += updatePLN2;

            var (insertPLN3, updatePLN3) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_PLANNING_PURCHASE_PLAN", menu =>
            {
                menu.MenuName = "采购计划";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_PLANNING_PURCHASE_PLAN";
                menu.I18nKey = "menu.logistics.manufacturing.planning.purchase.plan";
                menu.Icon = "RiShoppingCartLine";
                menu.ParentId = manufacturingPlanningMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:planning:purchase:plan:list";
                menu.RoutePath = "/logistics/manufacturing/planning/purchase-plan";
                menu.ComponentPath = "logistics/manufacturing/planning/purchase-plan/index";
                menu.SortOrder = 6;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertPLN3;
            updateCount += updatePLN3;
        }

        // ========== 生产排程下的四级菜单 ==========
        if (manufacturingSchedulingMenu != null)
        {
            var (insertSCH1, updateSCH1) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_SCHEDULING_APS_SCHEDULE", menu =>
            {
                menu.MenuName = "APS排程";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_SCHEDULING_APS_SCHEDULE";
                menu.I18nKey = "menu.logistics.manufacturing.scheduling.aps.schedule";
                menu.Icon = "RiCalendarScheduleLine";
                menu.ParentId = manufacturingSchedulingMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:scheduling:aps:schedule:list";
                menu.RoutePath = "/logistics/manufacturing/scheduling/aps-schedule";
                menu.ComponentPath = "logistics/manufacturing/scheduling/aps-schedule/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertSCH1;
            updateCount += updateSCH1;

            var (insertSCH1Cl, updateSCH1Cl) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_SCHEDULING_APS_SCHEDULE_CHANGE_LOG", menu =>
            {
                menu.MenuName = "APS排程变更";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_SCHEDULING_APS_SCHEDULE_CHANGE_LOG";
                menu.I18nKey = "menu.logistics.manufacturing.scheduling.aps.schedule.change.log";
                menu.Icon = "RiFileHistoryLine";
                menu.ParentId = manufacturingSchedulingMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:scheduling:aps:schedule:change:log:list";
                menu.RoutePath = "/logistics/manufacturing/scheduling/aps-schedule-change-log";
                menu.ComponentPath = "logistics/manufacturing/scheduling/aps-schedule-change-log/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertSCH1Cl;
            updateCount += updateSCH1Cl;

            var (insertSCH2, updateSCH2) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_SCHEDULING_WORK_CENTER", menu =>
            {
                menu.MenuName = "工作中心";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_SCHEDULING_WORK_CENTER";
                menu.I18nKey = "menu.logistics.manufacturing.scheduling.work.center";
                menu.Icon = "RiBuilding4Line";
                menu.ParentId = manufacturingSchedulingMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:scheduling:work:center:list";
                menu.RoutePath = "/logistics/manufacturing/scheduling/work-center";
                menu.ComponentPath = "logistics/manufacturing/scheduling/work-center/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertSCH2;
            updateCount += updateSCH2;

            var (insertSCH3, updateSCH3) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_SCHEDULING_CHANGEOVER_MATRIX", menu =>
            {
                menu.MenuName = "换型矩阵";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_SCHEDULING_CHANGEOVER_MATRIX";
                menu.I18nKey = "menu.logistics.manufacturing.scheduling.changeover.matrix";
                menu.Icon = "RiExchangeLine";
                menu.ParentId = manufacturingSchedulingMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:scheduling:changeover:matrix:list";
                menu.RoutePath = "/logistics/manufacturing/scheduling/changeover-matrix";
                menu.ComponentPath = "logistics/manufacturing/scheduling/changeover-matrix/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertSCH3;
            updateCount += updateSCH3;

            var (insertSCH4, updateSCH4) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_SCHEDULING_APS_ORDER", menu =>
            {
                menu.MenuName = "APS订单";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_SCHEDULING_APS_ORDER";
                menu.I18nKey = "menu.logistics.manufacturing.scheduling.aps.order";
                menu.Icon = "RiListOrdered";
                menu.ParentId = manufacturingSchedulingMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:scheduling:aps:order:list";
                menu.RoutePath = "/logistics/manufacturing/scheduling/aps-order";
                menu.ComponentPath = "logistics/manufacturing/scheduling/aps-order/index";
                menu.SortOrder = 5;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertSCH4;
            updateCount += updateSCH4;

            var (insertSCH5, updateSCH5) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_SCHEDULING_PRODUCTION_DISPATCH", menu =>
            {
                menu.MenuName = "生产派工";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_SCHEDULING_PRODUCTION_DISPATCH";
                menu.I18nKey = "menu.logistics.manufacturing.scheduling.production.dispatch";
                menu.Icon = "RiSendPlaneLine";
                menu.ParentId = manufacturingSchedulingMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:scheduling:production:dispatch:list";
                menu.RoutePath = "/logistics/manufacturing/scheduling/production-dispatch";
                menu.ComponentPath = "logistics/manufacturing/scheduling/production-dispatch/index";
                menu.SortOrder = 6;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertSCH5;
            updateCount += updateSCH5;
        }

        // ========== 设变下的四级菜单 ==========
        if (manufacturingEngineeringChangeMenu != null)
        {
            var (insertECN1, updateECN1) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_KANBAN", menu =>
            {
                menu.MenuName = "设变看板";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_KANBAN";
                menu.I18nKey = "menu.logistics.manufacturing.engineering.change.kanban";
                menu.Icon = "RiDashboardLine";
                menu.ParentId = manufacturingEngineeringChangeMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:engineering:change:kanban:list";
                menu.RoutePath = "/logistics/manufacturing/engineering-change/kanban";
                menu.ComponentPath = "logistics/manufacturing/engineering-change/kanban/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertECN1;
            updateCount += updateECN1;

            var (insertECN2, updateECN2) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_BATCH", menu =>
            {
                menu.MenuName = "投入批次";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_BATCH";
                menu.I18nKey = "menu.logistics.manufacturing.engineering.change.batch";
                menu.Icon = "RiListCheck";
                menu.ParentId = manufacturingEngineeringChangeMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:engineering:change:batch:list";
                menu.RoutePath = "/logistics/manufacturing/engineering-change/batch";
                menu.ComponentPath = "logistics/manufacturing/engineering-change/batch/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertECN2;
            updateCount += updateECN2;

            var (insertECN3, updateECN3) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_KAKUNIN", menu =>
            {
                menu.MenuName = "物料确认";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_KAKUNIN";
                menu.I18nKey = "menu.logistics.manufacturing.engineering.change.kakunin";
                menu.Icon = "RiCheckboxCircleLine";
                menu.ParentId = manufacturingEngineeringChangeMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:engineering:change:kakunin:list";
                menu.RoutePath = "/logistics/manufacturing/engineering-change/kakunin";
                menu.ComponentPath = "logistics/manufacturing/engineering-change/kakunin/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertECN3;
            updateCount += updateECN3;

            var (insertECNNotification, updateECNNotification) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_EC_NOTIFICATION", menu =>
            {
                menu.MenuName = "设变通知";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_EC_NOTIFICATION";
                menu.I18nKey = "menu.logistics.manufacturing.engineering.change.ec.notification";
                menu.Icon = "RiNotificationLine";
                menu.ParentId = manufacturingEngineeringChangeMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:engineering:change:ec:notification:list";
                menu.RoutePath = "/logistics/manufacturing/engineering-change/ec-notification";
                menu.ComponentPath = "logistics/manufacturing/engineering-change/ec-notification/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertECNNotification;
            updateCount += updateECNNotification;

            var (insertECN4, updateECN4) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_GIJUTSU", menu =>
            {
                menu.MenuName = "技术部门";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_GIJUTSU";
                menu.I18nKey = "menu.logistics.manufacturing.engineering.change.gijutsu";
                menu.Icon = "RiCpuLine";
                menu.ParentId = manufacturingEngineeringChangeMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:engineering:change:gijutsu:list";
                menu.RoutePath = "/logistics/manufacturing/engineering-change/gijutsu";
                menu.ComponentPath = "logistics/manufacturing/engineering-change/gijutsu/index";
                menu.SortOrder = 5;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertECN4;
            updateCount += updateECN4;

            var (insertECN5, updateECN5) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_KOUBAI", menu =>
            {
                menu.MenuName = "采购部门";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_KOUBAI";
                menu.I18nKey = "menu.logistics.manufacturing.engineering.change.koubai";
                menu.Icon = "RiShoppingCart2Line";
                menu.ParentId = manufacturingEngineeringChangeMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:engineering:change:koubai:list";
                menu.RoutePath = "/logistics/manufacturing/engineering-change/koubai";
                menu.ComponentPath = "logistics/manufacturing/engineering-change/koubai/index";
                menu.SortOrder = 6;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertECN5;
            updateCount += updateECN5;

            var (insertECN6, updateECN6) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_SEIKAN", menu =>
            {
                menu.MenuName = "生管部门";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_SEIKAN";
                menu.I18nKey = "menu.logistics.manufacturing.engineering.change.seikan";
                menu.Icon = "RiSettings3Line";
                menu.ParentId = manufacturingEngineeringChangeMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:engineering:change:seikan:list";
                menu.RoutePath = "/logistics/manufacturing/engineering-change/seikan";
                menu.ComponentPath = "logistics/manufacturing/engineering-change/seikan/index";
                menu.SortOrder = 7;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertECN6;
            updateCount += updateECN6;

            var (insertECN7, updateECN7) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_UKEKEN", menu =>
            {
                menu.MenuName = "受检部门";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_UKEKEN";
                menu.I18nKey = "menu.logistics.manufacturing.engineering.change.ukeken";
                menu.Icon = "RiSearchEyeLine";
                menu.ParentId = manufacturingEngineeringChangeMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:engineering:change:ukeken:list";
                menu.RoutePath = "/logistics/manufacturing/engineering-change/ukeken";
                menu.ComponentPath = "logistics/manufacturing/engineering-change/ukeken/index";
                menu.SortOrder = 8;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertECN7;
            updateCount += updateECN7;

            var (insertECN8, updateECN8) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_BUKAN", menu =>
            {
                menu.MenuName = "部管部门";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_BUKAN";
                menu.I18nKey = "menu.logistics.manufacturing.engineering.change.bukan";
                menu.Icon = "RiArchiveDrawerLine";
                menu.ParentId = manufacturingEngineeringChangeMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:engineering:change:bukan:list";
                menu.RoutePath = "/logistics/manufacturing/engineering-change/bukan";
                menu.ComponentPath = "logistics/manufacturing/engineering-change/bukan/index";
                menu.SortOrder = 9;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertECN8;
            updateCount += updateECN8;

            var (insertECN9, updateECN9) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_SEIZOUNIKA", menu =>
            {
                menu.MenuName = "制造二课";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_SEIZOUNIKA";
                menu.I18nKey = "menu.logistics.manufacturing.engineering.change.seizounika";
                menu.Icon = "RiSeedlingLine";
                menu.ParentId = manufacturingEngineeringChangeMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:engineering:change:seizounika:list";
                menu.RoutePath = "/logistics/manufacturing/engineering-change/seizounika";
                menu.ComponentPath = "logistics/manufacturing/engineering-change/seizounika/index";
                menu.SortOrder = 10;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertECN9;
            updateCount += updateECN9;

            var (insertECN10, updateECN10) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_SEIZOUIKKA", menu =>
            {
                menu.MenuName = "制造一课";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_SEIZOUIKKA";
                menu.I18nKey = "menu.logistics.manufacturing.engineering.change.seizouikka";
                menu.Icon = "RiPlantLine";
                menu.ParentId = manufacturingEngineeringChangeMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:engineering:change:seizouikka:list";
                menu.RoutePath = "/logistics/manufacturing/engineering-change/seizouikka";
                menu.ComponentPath = "logistics/manufacturing/engineering-change/seizouikka/index";
                menu.SortOrder = 11;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertECN10;
            updateCount += updateECN10;

            var (insertECN11, updateECN11) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_HINKAN", menu =>
            {
                menu.MenuName = "品管部门";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_HINKAN";
                menu.I18nKey = "menu.logistics.manufacturing.engineering.change.hinkan";
                menu.Icon = "RiShieldCheckLine";
                menu.ParentId = manufacturingEngineeringChangeMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:engineering:change:hinkan:list";
                menu.RoutePath = "/logistics/manufacturing/engineering-change/hinkan";
                menu.ComponentPath = "logistics/manufacturing/engineering-change/hinkan/index";
                menu.SortOrder = 12;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertECN11;
            updateCount += updateECN11;

            var (insertECN12, updateECN12) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_LEGACY_PRODUCT", menu =>
            {
                menu.MenuName = "旧品管制";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_LEGACY_PRODUCT";
                menu.I18nKey = "menu.logistics.manufacturing.engineering.change.legacy.product";
                menu.Icon = "RiTimeLine";
                menu.ParentId = manufacturingEngineeringChangeMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:engineering:change:legacy:product:list";
                menu.RoutePath = "/logistics/manufacturing/engineering-change/legacy-product";
                menu.ComponentPath = "logistics/manufacturing/engineering-change/legacy-product/index";
                menu.SortOrder = 13;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertECN12;
            updateCount += updateECN12;
        }

        // ========== 产出管理下的四级菜单 ==========
        if (manufacturingOutputMenu != null)
        {
            var (insertOUT1, updateOUT1) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_OUTPUT_PRODUCTION_ORDER", menu =>
            {
                menu.MenuName = "生产工单";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_OUTPUT_PRODUCTION_ORDER";
                menu.I18nKey = "menu.logistics.manufacturing.output.production.order";
                menu.Icon = "RiFileList3Line";
                menu.ParentId = manufacturingOutputMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:output:production:order:list";
                menu.RoutePath = "/logistics/manufacturing/output/production-order";
                menu.ComponentPath = "logistics/manufacturing/output/production-order/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertOUT1;
            updateCount += updateOUT1;

            var (insertOUT1Cl, updateOUT1Cl) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_OUTPUT_PRODUCTION_ORDER_CHANGE_LOG", menu =>
            {
                menu.MenuName = "生产工单变更";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_OUTPUT_PRODUCTION_ORDER_CHANGE_LOG";
                menu.I18nKey = "menu.logistics.manufacturing.output.production.order.change.log";
                menu.Icon = "RiFileHistoryLine";
                menu.ParentId = manufacturingOutputMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:output:production:order:change:log:list";
                menu.RoutePath = "/logistics/manufacturing/output/production-order-change-log";
                menu.ComponentPath = "logistics/manufacturing/output/production-order-change-log/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertOUT1Cl;
            updateCount += updateOUT1Cl;

            var (insertOUT2, updateOUT2) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_OUTPUT_PCBA", menu =>
            {
                menu.MenuName = "PCBA日报";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_OUTPUT_PCBA";
                menu.I18nKey = "menu.logistics.manufacturing.output.pcba";
                menu.Icon = "RiCpuLine";
                menu.ParentId = manufacturingOutputMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:output:pcba:list";
                menu.RoutePath = "/logistics/manufacturing/output/pcba-output";
                menu.ComponentPath = "logistics/manufacturing/output/pcba-output/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertOUT2;
            updateCount += updateOUT2;

            var (insertOUT4, updateOUT4) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_OUTPUT_ASSY", menu =>
            {
                menu.MenuName = "组立日报";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_OUTPUT_ASSY";
                menu.I18nKey = "menu.logistics.manufacturing.output.assy";
                menu.Icon = "RiSettings4Line";
                menu.ParentId = manufacturingOutputMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:output:assy:list";
                menu.RoutePath = "/logistics/manufacturing/output/assy-output";
                menu.ComponentPath = "logistics/manufacturing/output/assy-output/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertOUT4;
            updateCount += updateOUT4;

            var (insertOUT6, updateOUT6) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_OUTPUT_PRODUCTION_CHANGEOVER", menu =>
            {
                menu.MenuName = "生产切换";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_OUTPUT_PRODUCTION_CHANGEOVER";
                menu.I18nKey = "menu.logistics.manufacturing.output.production.changeover";
                menu.Icon = "RiRefreshLine";
                menu.ParentId = manufacturingOutputMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:output:production:changeover:list";
                menu.RoutePath = "/logistics/manufacturing/output/production-changeover";
                menu.ComponentPath = "logistics/manufacturing/output/production-changeover/index";
                menu.SortOrder = 5;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertOUT6;
            updateCount += updateOUT6;

            var (insertOUT7, updateOUT7) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_OUTPUT_EQUIPMENT_OPERATION_RATE", menu =>
            {
                menu.MenuName = "机器稼动率";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_OUTPUT_EQUIPMENT_OPERATION_RATE";
                menu.I18nKey = "menu.logistics.manufacturing.output.equipment.operation.rate";
                menu.Icon = "RiPulseLine";
                menu.ParentId = manufacturingOutputMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:output:equipment:operation:rate:list";
                menu.RoutePath = "/logistics/manufacturing/output/equipment-operation-rate";
                menu.ComponentPath = "logistics/manufacturing/output/equipment-operation-rate/index";
                menu.SortOrder = 6;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertOUT7;
            updateCount += updateOUT7;

            var (insertOUT7Cl, updateOUT7Cl) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_OUTPUT_EQUIPMENT_OPERATION_RATE_CHANGE_LOG", menu =>
            {
                menu.MenuName = "机器稼动率变更";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_OUTPUT_EQUIPMENT_OPERATION_RATE_CHANGE_LOG";
                menu.I18nKey = "menu.logistics.manufacturing.output.equipment.operation.rate.change.log";
                menu.Icon = "RiFileHistoryLine";
                menu.ParentId = manufacturingOutputMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:output:equipment:operation:rate:change:log:list";
                menu.RoutePath = "/logistics/manufacturing/output/equipment-operation-rate-change-log";
                menu.ComponentPath = "logistics/manufacturing/output/equipment-operation-rate-change-log/index";
                menu.SortOrder = 7;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertOUT7Cl;
            updateCount += updateOUT7Cl;

            var (insertOUT8, updateOUT8) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_OUTPUT_PERSONNEL_OPERATION_RATE", menu =>
            {
                menu.MenuName = "人员稼动率";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_OUTPUT_PERSONNEL_OPERATION_RATE";
                menu.I18nKey = "menu.logistics.manufacturing.output.personnel.operation.rate";
                menu.Icon = "RiUserLine";
                menu.ParentId = manufacturingOutputMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:output:personnel:operation:rate:list";
                menu.RoutePath = "/logistics/manufacturing/output/personnel-operation-rate";
                menu.ComponentPath = "logistics/manufacturing/output/personnel-operation-rate/index";
                menu.SortOrder = 8;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertOUT8;
            updateCount += updateOUT8;

            var (insertOUT8Cl, updateOUT8Cl) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_OUTPUT_PERSONNEL_OPERATION_RATE_CHANGE_LOG", menu =>
            {
                menu.MenuName = "人员稼动率变更";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_OUTPUT_PERSONNEL_OPERATION_RATE_CHANGE_LOG";
                menu.I18nKey = "menu.logistics.manufacturing.output.personnel.operation.rate.change.log";
                menu.Icon = "RiFileHistoryLine";
                menu.ParentId = manufacturingOutputMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:output:personnel:operation:rate:change:log:list";
                menu.RoutePath = "/logistics/manufacturing/output/personnel-operation-rate-change-log";
                menu.ComponentPath = "logistics/manufacturing/output/personnel-operation-rate-change-log/index";
                menu.SortOrder = 9;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertOUT8Cl;
            updateCount += updateOUT8Cl;

            var (insertOUT9, updateOUT9) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_OUTPUT_PRODUCTION_TEAM", menu =>
            {
                menu.MenuName = "生产班组";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_OUTPUT_PRODUCTION_TEAM";
                menu.I18nKey = "menu.logistics.manufacturing.output.production.team";
                menu.Icon = "RiTeamLine";
                menu.ParentId = manufacturingOutputMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:output:production:team:list";
                menu.RoutePath = "/logistics/manufacturing/output/production-team";
                menu.ComponentPath = "logistics/manufacturing/output/production-team/index";
                menu.SortOrder = 10;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertOUT9;
            updateCount += updateOUT9;

            var (insertOUT10, updateOUT10) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_OUTPUT_STANDARD_OPERATION_RATE", menu =>
            {
                menu.MenuName = "标准生产稼动率";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_OUTPUT_STANDARD_OPERATION_RATE";
                menu.I18nKey = "menu.logistics.manufacturing.output.standard.operation.rate";
                menu.Icon = "RiBarChartLine";
                menu.ParentId = manufacturingOutputMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:output:standard:operation:rate:list";
                menu.RoutePath = "/logistics/manufacturing/output/standard-operation-rate";
                menu.ComponentPath = "logistics/manufacturing/output/standard-operation-rate/index";
                menu.SortOrder = 11;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertOUT10;
            updateCount += updateOUT10;

            var (insertOUT10Cl, updateOUT10Cl) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_OUTPUT_STANDARD_OPERATION_RATE_CHANGE_LOG", menu =>
            {
                menu.MenuName = "标准生产稼动率变更";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_OUTPUT_STANDARD_OPERATION_RATE_CHANGE_LOG";
                menu.I18nKey = "menu.logistics.manufacturing.output.standard.operation.rate.change.log";
                menu.Icon = "RiFileHistoryLine";
                menu.ParentId = manufacturingOutputMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:output:standard:operation:rate:change:log:list";
                menu.RoutePath = "/logistics/manufacturing/output/standard-operation-rate-change-log";
                menu.ComponentPath = "logistics/manufacturing/output/standard-operation-rate-change-log/index";
                menu.SortOrder = 12;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertOUT10Cl;
            updateCount += updateOUT10Cl;
        }

        // ========== 不良管理下的四级菜单 ==========
        if (manufacturingDefectMenu != null)
        {
            var (insertDEF1, updateDEF1) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_DEFECT_PCBA_INSPECTION", menu =>
            {
                menu.MenuName = "PCBA检查";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_DEFECT_PCBA_INSPECTION";
                menu.I18nKey = "menu.logistics.manufacturing.defect.pcba.inspection";
                menu.Icon = "RiSearchLine";
                menu.ParentId = manufacturingDefectMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:defect:pcba:inspection:list";
                menu.RoutePath = "/logistics/manufacturing/defect/pcba-inspection";
                menu.ComponentPath = "logistics/manufacturing/defect/pcba-inspection/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertDEF1;
            updateCount += updateDEF1;

            var (insertDEF3, updateDEF3) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_DEFECT_PCBA_REPAIR", menu =>
            {
                menu.MenuName = "PCBA改修";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_DEFECT_PCBA_REPAIR";
                menu.I18nKey = "menu.logistics.manufacturing.defect.pcba.repair";
                menu.Icon = "RiToolsLine";
                menu.ParentId = manufacturingDefectMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:defect:pcba:repair:list";
                menu.RoutePath = "/logistics/manufacturing/defect/pcba-repair";
                menu.ComponentPath = "logistics/manufacturing/defect/pcba-repair/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertDEF3;
            updateCount += updateDEF3;

            var (insertDEF5, updateDEF5) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_DEFECT_ASSY", menu =>
            {
                menu.MenuName = "组立不良";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_DEFECT_ASSY";
                menu.I18nKey = "menu.logistics.manufacturing.defect.assy";
                menu.Icon = "RiAlarmWarningLine";
                menu.ParentId = manufacturingDefectMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:defect:assy:list";
                menu.RoutePath = "/logistics/manufacturing/defect/assy-defect";
                menu.ComponentPath = "logistics/manufacturing/defect/assy-defect/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertDEF5;
            updateCount += updateDEF5;
        }

        // ========== SOP 管理下的四级菜单（对齐 Sop/ 实体：Workstation/Doc/Revision/Ack/Exec/ExecScan/EsdCheck/Call）==========
        if (manufacturingSopMenu != null)
        {
            var (insertSOP0, updateSOP0) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_SOP_WORKSTATION", menu =>
            {
                menu.MenuName = "工位管理";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_SOP_WORKSTATION";
                menu.I18nKey = "menu.logistics.manufacturing.sop.workstation";
                menu.Icon = "RiMapPinLine";
                menu.ParentId = manufacturingSopMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:sop:workstation:list";
                menu.RoutePath = "/logistics/manufacturing/sop/workstation";
                menu.ComponentPath = "logistics/manufacturing/sop/workstation/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertSOP0;
            updateCount += updateSOP0;

            var (insertSOP1, updateSOP1) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_SOP_DOC", menu =>
            {
                menu.MenuName = "SOP文档";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_SOP_DOC";
                menu.I18nKey = "menu.logistics.manufacturing.sop.doc";
                menu.Icon = "RiFilePaper2Line";
                menu.ParentId = manufacturingSopMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:sop:doc:list";
                menu.RoutePath = "/logistics/manufacturing/sop/doc";
                menu.ComponentPath = "logistics/manufacturing/sop/doc/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertSOP1;
            updateCount += updateSOP1;

            var (insertSOP2, updateSOP2) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_SOP_REVISION", menu =>
            {
                menu.MenuName = "SOP版本";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_SOP_REVISION";
                menu.I18nKey = "menu.logistics.manufacturing.sop.revision";
                menu.Icon = "RiGitBranchLine";
                menu.ParentId = manufacturingSopMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:sop:revision:list";
                menu.RoutePath = "/logistics/manufacturing/sop/revision";
                menu.ComponentPath = "logistics/manufacturing/sop/revision/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertSOP2;
            updateCount += updateSOP2;

            var (insertSOP3, updateSOP3) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_SOP_ACK", menu =>
            {
                menu.MenuName = "版本确认";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_SOP_ACK";
                menu.I18nKey = "menu.logistics.manufacturing.sop.ack";
                menu.Icon = "RiCheckboxCircleLine";
                menu.ParentId = manufacturingSopMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:sop:ack:list";
                menu.RoutePath = "/logistics/manufacturing/sop/ack";
                menu.ComponentPath = "logistics/manufacturing/sop/ack/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertSOP3;
            updateCount += updateSOP3;

            var (insertSOP4, updateSOP4) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_SOP_EXEC", menu =>
            {
                menu.MenuName = "工位执行";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_SOP_EXEC";
                menu.I18nKey = "menu.logistics.manufacturing.sop.exec";
                menu.Icon = "RiPlayCircleLine";
                menu.ParentId = manufacturingSopMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:sop:exec:list";
                menu.RoutePath = "/logistics/manufacturing/sop/exec";
                menu.ComponentPath = "logistics/manufacturing/sop/exec/index";
                menu.SortOrder = 5;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertSOP4;
            updateCount += updateSOP4;

            var (insertSOP5, updateSOP5) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_SOP_EXEC_SCAN", menu =>
            {
                menu.MenuName = "扫码记录";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_SOP_EXEC_SCAN";
                menu.I18nKey = "menu.logistics.manufacturing.sop.exec.scan";
                menu.Icon = "RiBarcodeLine";
                menu.ParentId = manufacturingSopMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:sop:exec:scan:list";
                menu.RoutePath = "/logistics/manufacturing/sop/exec-scan";
                menu.ComponentPath = "logistics/manufacturing/sop/exec-scan/index";
                menu.SortOrder = 6;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertSOP5;
            updateCount += updateSOP5;

            var (insertSOP6, updateSOP6) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_SOP_ESD_CHECK", menu =>
            {
                menu.MenuName = "ESD检查";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_SOP_ESD_CHECK";
                menu.I18nKey = "menu.logistics.manufacturing.sop.esd.check";
                menu.Icon = "RiShieldFlashLine";
                menu.ParentId = manufacturingSopMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:sop:esd:check:list";
                menu.RoutePath = "/logistics/manufacturing/sop/esd-check";
                menu.ComponentPath = "logistics/manufacturing/sop/esd-check/index";
                menu.SortOrder = 7;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertSOP6;
            updateCount += updateSOP6;

            var (insertSOP7, updateSOP7) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_SOP_CALL", menu =>
            {
                menu.MenuName = "安灯呼叫";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_SOP_CALL";
                menu.I18nKey = "menu.logistics.manufacturing.sop.call";
                menu.Icon = "RiAlarmWarningLine";
                menu.ParentId = manufacturingSopMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:sop:call:list";
                menu.RoutePath = "/logistics/manufacturing/sop/call";
                menu.ComponentPath = "logistics/manufacturing/sop/call/index";
                menu.SortOrder = 8;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertSOP7;
            updateCount += updateSOP7;
        }

        // ========== 品质成本下的四级菜单 ==========
        if (qualityCostMenu != null)
        {
            var (insertQC1, updateQC1) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_QUALITY_COST_ASSURANCE", menu =>
            {
                menu.MenuName = "品质保证";
                menu.MenuCode = "LOGISTICS_QUALITY_COST_ASSURANCE";
                menu.I18nKey = "menu.logistics.quality.cost.assurance";
                menu.Icon = "RiShieldCheckLine";
                menu.ParentId = qualityCostMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:quality:cost:assurance:list";
                menu.RoutePath = "/logistics/quality/cost/assurance";
                menu.ComponentPath = "logistics/quality/cost/assurance/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertQC1;
            updateCount += updateQC1;

            var (insertQC2, updateQC2) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_QUALITY_COST_ISSUE", menu =>
            {
                menu.MenuName = "品质问题";
                menu.MenuCode = "LOGISTICS_QUALITY_COST_ISSUE";
                menu.I18nKey = "menu.logistics.quality.cost.issue";
                menu.Icon = "RiErrorWarningLine";
                menu.ParentId = qualityCostMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:quality:cost:issue:list";
                menu.RoutePath = "/logistics/quality/cost/issue";
                menu.ComponentPath = "logistics/quality/cost/issue/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertQC2;
            updateCount += updateQC2;

            var (insertQC3, updateQC3) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_QUALITY_COST_INCIDENT", menu =>
            {
                menu.MenuName = "品质事故";
                menu.MenuCode = "LOGISTICS_QUALITY_COST_INCIDENT";
                menu.I18nKey = "menu.logistics.quality.cost.incident";
                menu.Icon = "RiAlarmWarningLine";
                menu.ParentId = qualityCostMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:quality:cost:incident:list";
                menu.RoutePath = "/logistics/quality/cost/incident";
                menu.ComponentPath = "logistics/quality/cost/incident/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertQC3;
            updateCount += updateQC3;
        }

        // ========== 质量业务下的四级菜单 ==========
        if (qualityAssuranceMenu != null)
        {
            var (insertQO1, updateQO1) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_QUALITY_OPERATION_SAMPLING_SCHEME", menu =>
            {
                menu.MenuName = "抽样方案";
                menu.MenuCode = "LOGISTICS_QUALITY_OPERATION_SAMPLING_SCHEME";
                menu.I18nKey = "menu.logistics.quality.operation.sampling.scheme";
                menu.Icon = "RiListCheck";
                menu.ParentId = qualityAssuranceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:quality:operation:sampling:scheme:list";
                menu.RoutePath = "/logistics/quality/operation/sampling-scheme";
                menu.ComponentPath = "logistics/quality/operation/sampling-scheme/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertQO1;
            updateCount += updateQO1;

            var (insertQO2, updateQO2) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_QUALITY_OPERATION_INSPECTION_STANDARD", menu =>
            {
                menu.MenuName = "检验标准";
                menu.MenuCode = "LOGISTICS_QUALITY_OPERATION_INSPECTION_STANDARD";
                menu.I18nKey = "menu.logistics.quality.operation.inspection.standard";
                menu.Icon = "RiFileTextLine";
                menu.ParentId = qualityAssuranceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:quality:operation:inspection:standard:list";
                menu.RoutePath = "/logistics/quality/operation/inspection-standard";
                menu.ComponentPath = "logistics/quality/operation/inspection-standard/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertQO2;
            updateCount += updateQO2;

            var (insertQO3, updateQO3) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_QUALITY_OPERATION_IQC_ORDER", menu =>
            {
                menu.MenuName = "进货检验";
                menu.MenuCode = "LOGISTICS_QUALITY_OPERATION_IQC_ORDER";
                menu.I18nKey = "menu.logistics.quality.operation.iqc.order";
                menu.Icon = "RiInboxArchiveLine";
                menu.ParentId = qualityAssuranceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:quality:operation:iqc:order:list";
                menu.RoutePath = "/logistics/quality/operation/iqc-order";
                menu.ComponentPath = "logistics/quality/operation/iqc-order/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertQO3;
            updateCount += updateQO3;

            var (insertQO3Cl, updateQO3Cl) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_QUALITY_OPERATION_IQC_ORDER_CHANGE_LOG", menu =>
            {
                menu.MenuName = "进货检验变更";
                menu.MenuCode = "LOGISTICS_QUALITY_OPERATION_IQC_ORDER_CHANGE_LOG";
                menu.I18nKey = "menu.logistics.quality.operation.iqc.order.change.log";
                menu.Icon = "RiFileHistoryLine";
                menu.ParentId = qualityAssuranceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:quality:operation:iqc:order:change:log:list";
                menu.RoutePath = "/logistics/quality/operation/iqc-order-change-log";
                menu.ComponentPath = "logistics/quality/operation/iqc-order-change-log/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertQO3Cl;
            updateCount += updateQO3Cl;

            var (insertQO4, updateQO4) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_QUALITY_OPERATION_IPQC_ORDER", menu =>
            {
                menu.MenuName = "制程检验";
                menu.MenuCode = "LOGISTICS_QUALITY_OPERATION_IPQC_ORDER";
                menu.I18nKey = "menu.logistics.quality.operation.ipqc.order";
                menu.Icon = "RiSettings3Line";
                menu.ParentId = qualityAssuranceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:quality:operation:ipqc:order:list";
                menu.RoutePath = "/logistics/quality/operation/ipqc-order";
                menu.ComponentPath = "logistics/quality/operation/ipqc-order/index";
                menu.SortOrder = 5;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertQO4;
            updateCount += updateQO4;

            var (insertQO4Cl, updateQO4Cl) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_QUALITY_OPERATION_IPQC_ORDER_CHANGE_LOG", menu =>
            {
                menu.MenuName = "制程检验变更";
                menu.MenuCode = "LOGISTICS_QUALITY_OPERATION_IPQC_ORDER_CHANGE_LOG";
                menu.I18nKey = "menu.logistics.quality.operation.ipqc.order.change.log";
                menu.Icon = "RiFileHistoryLine";
                menu.ParentId = qualityAssuranceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:quality:operation:ipqc:order:change:log:list";
                menu.RoutePath = "/logistics/quality/operation/ipqc-order-change-log";
                menu.ComponentPath = "logistics/quality/operation/ipqc-order-change-log/index";
                menu.SortOrder = 6;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertQO4Cl;
            updateCount += updateQO4Cl;

            var (insertQO5, updateQO5) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_QUALITY_OPERATION_FQC_ORDER", menu =>
            {
                menu.MenuName = "入库检验";
                menu.MenuCode = "LOGISTICS_QUALITY_OPERATION_FQC_ORDER";
                menu.I18nKey = "menu.logistics.quality.operation.fqc.order";
                menu.Icon = "RiArchiveDrawerLine";
                menu.ParentId = qualityAssuranceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:quality:operation:fqc:order:list";
                menu.RoutePath = "/logistics/quality/operation/fqc-order";
                menu.ComponentPath = "logistics/quality/operation/fqc-order/index";
                menu.SortOrder = 7;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertQO5;
            updateCount += updateQO5;

            var (insertQO5Cl, updateQO5Cl) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_QUALITY_OPERATION_FQC_ORDER_CHANGE_LOG", menu =>
            {
                menu.MenuName = "入库检验变更";
                menu.MenuCode = "LOGISTICS_QUALITY_OPERATION_FQC_ORDER_CHANGE_LOG";
                menu.I18nKey = "menu.logistics.quality.operation.fqc.order.change.log";
                menu.Icon = "RiFileHistoryLine";
                menu.ParentId = qualityAssuranceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:quality:operation:fqc:order:change:log:list";
                menu.RoutePath = "/logistics/quality/operation/fqc-order-change-log";
                menu.ComponentPath = "logistics/quality/operation/fqc-order-change-log/index";
                menu.SortOrder = 8;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertQO5Cl;
            updateCount += updateQO5Cl;
        }

        // ========== 客诉管理下的四级菜单 (LOGISTICS_QUALITY_COMPLAINT) ==========
        if (qualityComplaintMenu != null)
        {
            var (insertCP1, updateCP1) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_QUALITY_COMPLAINT_CUSTOMER", menu =>
            {
                menu.MenuName = "客诉登记";
                menu.MenuCode = "LOGISTICS_QUALITY_COMPLAINT_CUSTOMER";
                menu.I18nKey = "menu.logistics.quality.complaint.customer";
                menu.Icon = "RiMessage3Line";
                menu.ParentId = qualityComplaintMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:quality:complaint:customer:list";
                menu.RoutePath = "/logistics/quality/complaint/customer-complaint";
                menu.ComponentPath = "logistics/quality/complaint/customer-complaint/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertCP1;
            updateCount += updateCP1;

            var (insertCP3, updateCP3) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_QUALITY_COMPLAINT_CUSTOMER_COMPLAINT_HANDLING", menu =>
            {
                menu.MenuName = "客诉处理";
                menu.MenuCode = "LOGISTICS_QUALITY_COMPLAINT_CUSTOMER_COMPLAINT_HANDLING";
                menu.I18nKey = "menu.logistics.quality.complaint.customer.complaint.handling";
                menu.Icon = "RiFileEditLine";
                menu.ParentId = qualityComplaintMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:quality:complaint:customer:complaint:handling:list";
                menu.RoutePath = "/logistics/quality/complaint/customer-complaint-handling";
                menu.ComponentPath = "logistics/quality/complaint/customer-complaint-handling/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertCP3;
            updateCount += updateCP3;

            var (insertCP4, updateCP4) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_QUALITY_COMPLAINT_CUSTOMER_SATISFACTION_SURVEY", menu =>
            {
                menu.MenuName = "客户满意度调查";
                menu.MenuCode = "LOGISTICS_QUALITY_COMPLAINT_CUSTOMER_SATISFACTION_SURVEY";
                menu.I18nKey = "menu.logistics.quality.complaint.customer.satisfaction.survey";
                menu.Icon = "RiSurveyLine";
                menu.ParentId = qualityComplaintMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:quality:complaint:customer:satisfaction:survey:list";
                menu.RoutePath = "/logistics/quality/complaint/customer-satisfaction-survey";
                menu.ComponentPath = "logistics/quality/complaint/customer-satisfaction-survey/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertCP4;
            updateCount += updateCP4;

            var (insertCP6, updateCP6) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_QUALITY_COMPLAINT_SUPPLIER_EVALUATION", menu =>
            {
                menu.MenuName = "供应商评价考核";
                menu.MenuCode = "LOGISTICS_QUALITY_COMPLAINT_SUPPLIER_EVALUATION";
                menu.I18nKey = "menu.logistics.quality.complaint.supplier.evaluation";
                menu.Icon = "RiStarLine";
                menu.ParentId = qualityComplaintMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:quality:complaint:supplier:evaluation:list";
                menu.RoutePath = "/logistics/quality/complaint/supplier-evaluation";
                menu.ComponentPath = "logistics/quality/complaint/supplier-evaluation/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertCP6;
            updateCount += updateCP6;
        }

        return (insertCount, updateCount);
    }

    /// <summary>
    /// 创建或更新菜单。
    /// </summary>
    /// <param name="menuRepository">菜单仓储。</param>
    /// <param name="sqlSugarContext">SqlSugar上下文。</param>
    /// <param name="tenantCode">租户编码。</param>
    /// <param name="menuCode">菜单编码（业务键）。</param>
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
            menu.IsBuiltIn = 1;
            
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
