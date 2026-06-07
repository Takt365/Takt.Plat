// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds
// 文件名称：TaktMenuLevel4SeedData.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt 四级菜单种子数据。
//           在三级菜单已存在的前提下，主要扩展后勤-物料采购、生产制造（BOM/排程/设变/产出/不良）等更细页面。
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
/// 父级通常来自 <see cref="TaktMenuLevel3SeedData"/> 中的采购目录、BOM、排程、设变、产出、不良等三级节点。
/// 由 TaktMenuSeedData 统一协调调用，不直接注册为 ITaktSeedDataCoordinator。
/// </para>
/// </summary>
public class TaktMenuLevel4SeedData
{
    /// <summary>
    /// 初始化四级菜单种子数据。
    /// <para>
    /// 写入采购子项、BOM 子项、排程子项、设变相关部门视图、产出与不良下的 PCBA/Assembly 目录等。
    /// </para>
    /// </summary>
    /// <param name="serviceProvider">服务提供者，用于解析 <see cref="ITaktRepository{TaktMenu}"/>。</param>
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
        var logisticsMaterialPurchasingMenu = await menuRepository.FirstAsync(m => m.MenuCode == "LOGISTICS_MATERIAL_PURCHASING");
        var manufacturingBomMenu = await menuRepository.FirstAsync(m => m.MenuCode == "LOGISTICS_MANUFACTURING_BOM");
        var manufacturingSchedulingMenu = await menuRepository.FirstAsync(m => m.MenuCode == "LOGISTICS_MANUFACTURING_SCHEDULING");
        var manufacturingEngineeringChangeMenu = await menuRepository.FirstAsync(m => m.MenuCode == "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE");
        var manufacturingOutputMenu = await menuRepository.FirstAsync(m => m.MenuCode == "LOGISTICS_MANUFACTURING_OUTPUT");
        var manufacturingDefectMenu = await menuRepository.FirstAsync(m => m.MenuCode == "LOGISTICS_MANUFACTURING_DEFECT");
        var qualityCostMenu = await menuRepository.FirstAsync(m => m.MenuCode == "LOGISTICS_QUALITY_COST");
        var qualityOperationMenu = await menuRepository.FirstAsync(m => m.MenuCode == "LOGISTICS_QUALITY_OPERATION");
        var qualityComplaintMenu = await menuRepository.FirstAsync(m => m.MenuCode == "LOGISTICS_QUALITY_COMPLAINT");
        var statisticsReportFinancialMenu = await menuRepository.FirstAsync(m => m.MenuCode == "STATISTICS_REPORT_FINANCIAL");
        var statisticsReportHumanResourceMenu = await menuRepository.FirstAsync(m => m.MenuCode == "STATISTICS_REPORT_HUMANRESOURCE");
        var statisticsReportLogisticsMenu = await menuRepository.FirstAsync(m => m.MenuCode == "STATISTICS_REPORT_LOGISTICS");

        // ========== 采购管理下的四级菜单 ==========
        if (logisticsMaterialPurchasingMenu != null)
        {
            var (insert03, update03) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MATERIAL_PURCHASING_SUPPLIER", menu =>
            {
                menu.MenuName = "供应商";
                menu.MenuCode = "LOGISTICS_MATERIAL_PURCHASING_SUPPLIER";
                menu.I18nKey = "menu.logistics.materials.purchasing.supplier";
                menu.Icon = "RiTruckLine";
                menu.ParentId = logisticsMaterialPurchasingMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:materials:supplier:list";
                menu.RoutePath = "/logistics/materials/purchasing/supplier";
                menu.ComponentPath = "logistics/materials/purchasing/supplier/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insert03;
            updateCount += update03;

            var (insert04, update04) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MATERIAL_PURCHASING_VENDOR", menu =>
            {
                menu.MenuName = "经销商";
                menu.MenuCode = "LOGISTICS_MATERIAL_PURCHASING_VENDOR";
                menu.I18nKey = "menu.logistics.materials.purchasing.vendor";
                menu.Icon = "RiRegisteredLine";
                menu.ParentId = logisticsMaterialPurchasingMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:materials:vendor:list";
                menu.RoutePath = "/logistics/materials/purchasing/vendor";
                menu.ComponentPath = "logistics/materials/purchasing/vendor/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insert04;
            updateCount += update04;

            var (insert05, update05) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MATERIAL_PURCHASING_INFO", menu =>
            {
                menu.MenuName = "采购信息";
                menu.MenuCode = "LOGISTICS_MATERIAL_PURCHASING_INFO";
                menu.I18nKey = "menu.logistics.materials.purchasing.info";
                menu.Icon = "RiQuestionLine";
                menu.ParentId = logisticsMaterialPurchasingMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:materials:purchasing:info:list";
                menu.RoutePath = "/logistics/materials/purchasing/info";
                menu.ComponentPath = "logistics/materials/purchasing/info/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insert05;
            updateCount += update05;

            var (insert06, update06) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MATERIAL_PURCHASING_SOURCE", menu =>
            {
                menu.MenuName = "货源信息";
                menu.MenuCode = "LOGISTICS_MATERIAL_PURCHASING_SOURCE";
                menu.I18nKey = "menu.logistics.materials.purchasing.source";
                menu.Icon = "RiLinksLine";
                menu.ParentId = logisticsMaterialPurchasingMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:materials:purchasing:source:list";
                menu.RoutePath = "/logistics/materials/purchasing/source";
                menu.ComponentPath = "logistics/materials/purchasing/source/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insert06;
            updateCount += update06;

            var (insert07, update07) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MATERIAL_PURCHASING_REQUEST", menu =>
            {
                menu.MenuName = "采购申请";
                menu.MenuCode = "LOGISTICS_MATERIAL_PURCHASING_REQUEST";
                menu.I18nKey = "menu.logistics.materials.purchasing.request";
                menu.Icon = "RiFileAddLine";
                menu.ParentId = logisticsMaterialPurchasingMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:materials:purchaserequest:list";
                menu.RoutePath = "/logistics/materials/purchasing/request";
                menu.ComponentPath = "logistics/materials/purchasing/request/index";
                menu.SortOrder = 5;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insert07;
            updateCount += update07;

            var (insert08, update08) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MATERIAL_PURCHASING_ORDER", menu =>
            {
                menu.MenuName = "采购订单";
                menu.MenuCode = "LOGISTICS_MATERIAL_PURCHASING_ORDER";
                menu.I18nKey = "menu.logistics.materials.purchasing.order";
                menu.Icon = "RiListOrdered";
                menu.ParentId = logisticsMaterialPurchasingMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:materials:purchaseorder:list";
                menu.RoutePath = "/logistics/materials/purchasing/order";
                menu.ComponentPath = "logistics/materials/purchasing/order/index";
                menu.SortOrder = 6;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insert08;
            updateCount += update08;

            var (insert09, update09) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MATERIAL_PURCHASING_INVOICE", menu =>
            {
                menu.MenuName = "采购发票";
                menu.MenuCode = "LOGISTICS_MATERIAL_PURCHASING_INVOICE";
                menu.I18nKey = "menu.logistics.materials.purchasing.invoice";
                menu.Icon = "RiFilePaper2Line";
                menu.ParentId = logisticsMaterialPurchasingMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:materials:purchasing:invoice:list";
                menu.RoutePath = "/logistics/materials/purchasing/invoice";
                menu.ComponentPath = "logistics/materials/purchasing/invoice/index";
                menu.SortOrder = 7;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insert09;
            updateCount += update09;

            var (insert010, update010) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MATERIAL_PURCHASING_PLAN", menu =>
            {
                menu.MenuName = "采购计划";
                menu.MenuCode = "LOGISTICS_MATERIAL_PURCHASING_PLAN";
                menu.I18nKey = "menu.logistics.materials.purchasing.plan";
                menu.Icon = "RiCalendarTodoLine";
                menu.ParentId = logisticsMaterialPurchasingMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:materials:purchasing:plan:list";
                menu.RoutePath = "/logistics/materials/purchasing/plan";
                menu.ComponentPath = "logistics/materials/purchasing/plan/index";
                menu.SortOrder = 8;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insert010;
            updateCount += update010;
        }

        // ========== BOM 下的四级菜单 ==========
        if (manufacturingBomMenu != null)
        {
            var (insert00, update00) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_BOM_MODEL_DESTINATION", menu =>
            {
                menu.MenuName = "机种仕向";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_BOM_MODEL_DESTINATION";
                menu.I18nKey = "menu.logistics.manufacturing.bom.modeldestination";
                menu.Icon = "RiEarthLine";
                menu.ParentId = manufacturingBomMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:bom:modeldestination:list";
                menu.RoutePath = "/logistics/manufacturing/bom/model-destination";
                menu.ComponentPath = "logistics/manufacturing/bom/model-destination/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insert00;
            updateCount += update00;

            var (insertBOM2, updateBOM2) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_BOM_BILL_OF_MATERIAL", menu =>
            {
                menu.MenuName = "物料清单";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_BOM_BILL_OF_MATERIAL";
                menu.I18nKey = "menu.logistics.manufacturing.bom.billofmaterial";
                menu.Icon = "RiFileList2Line";
                menu.ParentId = manufacturingBomMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:bom:billofmaterial:list";
                menu.RoutePath = "/logistics/manufacturing/bom/bill-of-material";
                menu.ComponentPath = "logistics/manufacturing/bom/bill-of-material/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertBOM2;
            updateCount += updateBOM2;

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

            var (insertBOM8, updateBOM8) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_BOM_PACKAGING", menu =>
            {
                menu.MenuName = "物料包装";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_BOM_PACKAGING";
                menu.I18nKey = "menu.logistics.manufacturing.bom.packaging";
                menu.Icon = "RiBox3Line";
                menu.ParentId = manufacturingBomMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:bom:packaging:list";
                menu.RoutePath = "/logistics/manufacturing/bom/packaging";
                menu.ComponentPath = "logistics/manufacturing/bom/packaging/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertBOM8;
            updateCount += updateBOM8;

            var (insertBOM9, updateBOM9) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_BOM_STANDARD_OPERATION_TIME", menu =>
            {
                menu.MenuName = "标准工序时间";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_BOM_STANDARD_OPERATION_TIME";
                menu.I18nKey = "menu.logistics.manufacturing.bom.standardoperationtime";
                menu.Icon = "RiTimerLine";
                menu.ParentId = manufacturingBomMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:bom:standardoperationtime:list";
                menu.RoutePath = "/logistics/manufacturing/bom/standard-operation-time";
                menu.ComponentPath = "logistics/manufacturing/bom/standard-operation-time/index";
                menu.SortOrder = 5;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertBOM9;
            updateCount += updateBOM9;
        }

        // ========== 生产排程下的四级菜单 ==========
        if (manufacturingSchedulingMenu != null)
        {
            var (insertSCH1, updateSCH1) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_SCHEDULING_APS_SCHEDULE", menu =>
            {
                menu.MenuName = "APS排程";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_SCHEDULING_APS_SCHEDULE";
                menu.I18nKey = "menu.logistics.manufacturing.scheduling.apsschedule";
                menu.Icon = "RiCalendarScheduleLine";
                menu.ParentId = manufacturingSchedulingMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:scheduling:apsschedule:list";
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
        }

        // ========== 设变下的四级菜单 ==========
        if (manufacturingEngineeringChangeMenu != null)
        {
            var (insertECN1, updateECN1) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_KANBAN", menu =>
            {
                menu.MenuName = "设变看板";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_KANBAN";
                menu.I18nKey = "menu.logistics.manufacturing.engineeringchange.kanban";
                menu.Icon = "RiDashboardLine";
                menu.ParentId = manufacturingEngineeringChangeMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:engineeringchange:kanban:list";
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
                menu.I18nKey = "menu.logistics.manufacturing.engineeringchange.batch";
                menu.Icon = "RiListCheck";
                menu.ParentId = manufacturingEngineeringChangeMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:engineeringchange:batch:list";
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
                menu.I18nKey = "menu.logistics.manufacturing.engineeringchange.kakunin";
                menu.Icon = "RiCheckboxCircleLine";
                menu.ParentId = manufacturingEngineeringChangeMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:engineeringchange:kakunin:list";
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

            var (insertECNNotice, updateECNNotice) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_EC_NOTICE", menu =>
            {
                menu.MenuName = "设变通知";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_EC_NOTICE";
                menu.I18nKey = "menu.logistics.manufacturing.engineeringchange.ecnotice";
                menu.Icon = "RiNotificationLine";
                menu.ParentId = manufacturingEngineeringChangeMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:engineeringchange:ecnotice:list";
                menu.RoutePath = "/logistics/manufacturing/engineering-change/ec-notice";
                menu.ComponentPath = "logistics/manufacturing/engineering-change/ec-notice/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertECNNotice;
            updateCount += updateECNNotice;

            var (insertECN4, updateECN4) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_GIJUTSU", menu =>
            {
                menu.MenuName = "技术部门";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_GIJUTSU";
                menu.I18nKey = "menu.logistics.manufacturing.engineeringchange.gijutsu";
                menu.Icon = "RiCpuLine";
                menu.ParentId = manufacturingEngineeringChangeMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:engineeringchange:gijutsu:list";
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
                menu.I18nKey = "menu.logistics.manufacturing.engineeringchange.koubai";
                menu.Icon = "RiShoppingCart2Line";
                menu.ParentId = manufacturingEngineeringChangeMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:engineeringchange:koubai:list";
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
                menu.I18nKey = "menu.logistics.manufacturing.engineeringchange.seikan";
                menu.Icon = "RiSettings3Line";
                menu.ParentId = manufacturingEngineeringChangeMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:engineeringchange:seikan:list";
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
                menu.I18nKey = "menu.logistics.manufacturing.engineeringchange.ukeken";
                menu.Icon = "RiSearchEyeLine";
                menu.ParentId = manufacturingEngineeringChangeMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:engineeringchange:ukeken:list";
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
                menu.I18nKey = "menu.logistics.manufacturing.engineeringchange.bukan";
                menu.Icon = "RiArchiveDrawerLine";
                menu.ParentId = manufacturingEngineeringChangeMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:engineeringchange:bukan:list";
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

            var (insertECN9, updateECN9) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_SEIZONIKA", menu =>
            {
                menu.MenuName = "制造二课";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_SEIZONIKA";
                menu.I18nKey = "menu.logistics.manufacturing.engineeringchange.seizonika";
                menu.Icon = "RiSeedlingLine";
                menu.ParentId = manufacturingEngineeringChangeMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:engineeringchange:seizonika:list";
                menu.RoutePath = "/logistics/manufacturing/engineering-change/seizonika";
                menu.ComponentPath = "logistics/manufacturing/engineering-change/seizonika/index";
                menu.SortOrder = 10;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertECN9;
            updateCount += updateECN9;

            var (insertECN10, updateECN10) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_SEIZOIKKA", menu =>
            {
                menu.MenuName = "制造一课";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_SEIZOIKKA";
                menu.I18nKey = "menu.logistics.manufacturing.engineeringchange.seizoikka";
                menu.Icon = "RiPlantLine";
                menu.ParentId = manufacturingEngineeringChangeMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:engineeringchange:seizoikka:list";
                menu.RoutePath = "/logistics/manufacturing/engineering-change/seizoikka";
                menu.ComponentPath = "logistics/manufacturing/engineering-change/seizoikka/index";
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
                menu.I18nKey = "menu.logistics.manufacturing.engineeringchange.hinkan";
                menu.Icon = "RiShieldCheckLine";
                menu.ParentId = manufacturingEngineeringChangeMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:engineeringchange:hinkan:list";
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
                menu.I18nKey = "menu.logistics.manufacturing.engineeringchange.legacyproduct";
                menu.Icon = "RiTimeLine";
                menu.ParentId = manufacturingEngineeringChangeMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:engineeringchange:legacyproduct:list";
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
                menu.I18nKey = "menu.logistics.manufacturing.output.productionorder";
                menu.Icon = "RiFileList3Line";
                menu.ParentId = manufacturingOutputMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:output:productionorder:list";
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

            var (insertOUT2, updateOUT2) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_OUTPUT_PCBA_OUTPUT", menu =>
            {
                menu.MenuName = "PCBA日报";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_OUTPUT_PCBA_OUTPUT";
                menu.I18nKey = "menu.logistics.manufacturing.output.pcbaoutput";
                menu.Icon = "RiCpuLine";
                menu.ParentId = manufacturingOutputMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:output:pcbaoutput:list";
                menu.RoutePath = "/logistics/manufacturing/output/pcba-output";
                menu.ComponentPath = "logistics/manufacturing/output/pcba-output/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertOUT2;
            updateCount += updateOUT2;

            var (insertOUT4, updateOUT4) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_OUTPUT_ASSY_OUTPUT", menu =>
            {
                menu.MenuName = "组立日报";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_OUTPUT_ASSY_OUTPUT";
                menu.I18nKey = "menu.logistics.manufacturing.output.assyoutput";
                menu.Icon = "RiSettings4Line";
                menu.ParentId = manufacturingOutputMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:output:assyoutput:list";
                menu.RoutePath = "/logistics/manufacturing/output/assy-output";
                menu.ComponentPath = "logistics/manufacturing/output/assy-output/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertOUT4;
            updateCount += updateOUT4;

            var (insertOUT6, updateOUT6) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_OUTPUT_CHANGEOVER", menu =>
            {
                menu.MenuName = "切换记录";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_OUTPUT_CHANGEOVER";
                menu.I18nKey = "menu.logistics.manufacturing.output.changeover";
                menu.Icon = "RiRefreshLine";
                menu.ParentId = manufacturingOutputMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:output:changeover:list";
                menu.RoutePath = "/logistics/manufacturing/output/changeover";
                menu.ComponentPath = "logistics/manufacturing/output/changeover/index";
                menu.SortOrder = 4;
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
                menu.I18nKey = "menu.logistics.manufacturing.output.equipmentoperationrate";
                menu.Icon = "RiPulseLine";
                menu.ParentId = manufacturingOutputMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:output:equipmentoperationrate:list";
                menu.RoutePath = "/logistics/manufacturing/output/equipment-operation-rate";
                menu.ComponentPath = "logistics/manufacturing/output/equipment-operation-rate/index";
                menu.SortOrder = 5;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertOUT7;
            updateCount += updateOUT7;

            var (insertOUT8, updateOUT8) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_OUTPUT_PERSONNEL_OPERATION_RATE", menu =>
            {
                menu.MenuName = "人员稼动率";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_OUTPUT_PERSONNEL_OPERATION_RATE";
                menu.I18nKey = "menu.logistics.manufacturing.output.personneloperationrate";
                menu.Icon = "RiUserLine";
                menu.ParentId = manufacturingOutputMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:output:personneloperationrate:list";
                menu.RoutePath = "/logistics/manufacturing/output/personnel-operation-rate";
                menu.ComponentPath = "logistics/manufacturing/output/personnel-operation-rate/index";
                menu.SortOrder = 6;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertOUT8;
            updateCount += updateOUT8;

            var (insertOUT9, updateOUT9) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_OUTPUT_PRODUCTION_TEAM", menu =>
            {
                menu.MenuName = "生产班组";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_OUTPUT_PRODUCTION_TEAM";
                menu.I18nKey = "menu.logistics.manufacturing.output.productionteam";
                menu.Icon = "RiTeamLine";
                menu.ParentId = manufacturingOutputMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:output:productionteam:list";
                menu.RoutePath = "/logistics/manufacturing/output/production-team";
                menu.ComponentPath = "logistics/manufacturing/output/production-team/index";
                menu.SortOrder = 7;
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
                menu.I18nKey = "menu.logistics.manufacturing.output.standardoperationrate";
                menu.Icon = "RiBarChartLine";
                menu.ParentId = manufacturingOutputMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:output:standardoperationrate:list";
                menu.RoutePath = "/logistics/manufacturing/output/standard-operation-rate";
                menu.ComponentPath = "logistics/manufacturing/output/standard-operation-rate/index";
                menu.SortOrder = 8;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertOUT10;
            updateCount += updateOUT10;
        }

        // ========== 不良管理下的四级菜单 ==========
        if (manufacturingDefectMenu != null)
        {
            var (insertDEF1, updateDEF1) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_DEFECT_PCBA_INSPECTION", menu =>
            {
                menu.MenuName = "PCBA检查";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_DEFECT_PCBA_INSPECTION";
                menu.I18nKey = "menu.logistics.manufacturing.defect.pcbainspection";
                menu.Icon = "RiSearchLine";
                menu.ParentId = manufacturingDefectMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:defect:pcbainspection:list";
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
                menu.I18nKey = "menu.logistics.manufacturing.defect.pcbarepair";
                menu.Icon = "RiToolsLine";
                menu.ParentId = manufacturingDefectMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:defect:pcbarepair:list";
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

            var (insertDEF5, updateDEF5) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_MANUFACTURING_DEFECT_ASSY_DEFECT", menu =>
            {
                menu.MenuName = "组立不良";
                menu.MenuCode = "LOGISTICS_MANUFACTURING_DEFECT_ASSY_DEFECT";
                menu.I18nKey = "menu.logistics.manufacturing.defect.assydefect";
                menu.Icon = "RiAlarmWarningLine";
                menu.ParentId = manufacturingDefectMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:manufacturing:defect:assydefect:list";
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

        // ========== 品质成本下的四级菜单 ==========
        if (qualityCostMenu != null)
        {
            var (insertQC1, updateQC1) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_QUALITY_COST_QUALITY_OPERATION", menu =>
            {
                menu.MenuName = "品质业务";
                menu.MenuCode = "LOGISTICS_QUALITY_COST_QUALITY_OPERATION";
                menu.I18nKey = "menu.logistics.quality.cost.qualityoperation";
                menu.Icon = "RiShieldCheckLine";
                menu.ParentId = qualityCostMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:quality:cost:qualityoperation:list";
                menu.RoutePath = "/logistics/quality/cost/quality-operation";
                menu.ComponentPath = "logistics/quality/cost/quality-operation/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertQC1;
            updateCount += updateQC1;

            var (insertQC2, updateQC2) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_QUALITY_COST_QUALITY_FAILURE", menu =>
            {
                menu.MenuName = "品质问题";
                menu.MenuCode = "LOGISTICS_QUALITY_COST_QUALITY_FAILURE";
                menu.I18nKey = "menu.logistics.quality.cost.qualityfailure";
                menu.Icon = "RiErrorWarningLine";
                menu.ParentId = qualityCostMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:quality:cost:qualityfailure:list";
                menu.RoutePath = "/logistics/quality/cost/quality-failure";
                menu.ComponentPath = "logistics/quality/cost/quality-failure/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertQC2;
            updateCount += updateQC2;

            var (insertQC3, updateQC3) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_QUALITY_COST_QUALITY_INCIDENT", menu =>
            {
                menu.MenuName = "品质事故";
                menu.MenuCode = "LOGISTICS_QUALITY_COST_QUALITY_INCIDENT";
                menu.I18nKey = "menu.logistics.quality.cost.qualityincident";
                menu.Icon = "RiAlarmWarningLine";
                menu.ParentId = qualityCostMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:quality:cost:qualityincident:list";
                menu.RoutePath = "/logistics/quality/cost/quality-incident";
                menu.ComponentPath = "logistics/quality/cost/quality-incident/index";
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
        if (qualityOperationMenu != null)
        {
            var (insertQO1, updateQO1) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_QUALITY_OPERATION_SAMPLING_SCHEME", menu =>
            {
                menu.MenuName = "抽样方案";
                menu.MenuCode = "LOGISTICS_QUALITY_OPERATION_SAMPLING_SCHEME";
                menu.I18nKey = "menu.logistics.quality.operation.samplingscheme";
                menu.Icon = "RiListCheck";
                menu.ParentId = qualityOperationMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:quality:operation:samplingscheme:list";
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
                menu.I18nKey = "menu.logistics.quality.operation.inspectionstandard";
                menu.Icon = "RiFileTextLine";
                menu.ParentId = qualityOperationMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:quality:operation:inspectionstandard:list";
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
                menu.I18nKey = "menu.logistics.quality.operation.iqcorder";
                menu.Icon = "RiInboxArchiveLine";
                menu.ParentId = qualityOperationMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:quality:operation:iqcorder:list";
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

            var (insertQO4, updateQO4) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_QUALITY_OPERATION_IPQC_ORDER", menu =>
            {
                menu.MenuName = "制程检验";
                menu.MenuCode = "LOGISTICS_QUALITY_OPERATION_IPQC_ORDER";
                menu.I18nKey = "menu.logistics.quality.operation.ipqcorder";
                menu.Icon = "RiSettings3Line";
                menu.ParentId = qualityOperationMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:quality:operation:ipqcorder:list";
                menu.RoutePath = "/logistics/quality/operation/ipqc-order";
                menu.ComponentPath = "logistics/quality/operation/ipqc-order/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertQO4;
            updateCount += updateQO4;

            var (insertQO5, updateQO5) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_QUALITY_OPERATION_FQC_ORDER", menu =>
            {
                menu.MenuName = "入库检验";
                menu.MenuCode = "LOGISTICS_QUALITY_OPERATION_FQC_ORDER";
                menu.I18nKey = "menu.logistics.quality.operation.fqcorder";
                menu.Icon = "RiArchiveDrawerLine";
                menu.ParentId = qualityOperationMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:quality:operation:fqcorder:list";
                menu.RoutePath = "/logistics/quality/operation/fqc-order";
                menu.ComponentPath = "logistics/quality/operation/fqc-order/index";
                menu.SortOrder = 5;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertQO5;
            updateCount += updateQO5;
        }

        // ========== 客诉管理下的四级菜单 (LOGISTICS_QUALITY_COMPLAINT) ==========
        if (qualityComplaintMenu != null)
        {
            var (insertCP1, updateCP1) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "LOGISTICS_QUALITY_COMPLAINT_REGISTRATION", menu =>
            {
                menu.MenuName = "客诉登记";
                menu.MenuCode = "LOGISTICS_QUALITY_COMPLAINT_REGISTRATION";
                menu.I18nKey = "menu.logistics.quality.complaint.registration";
                menu.Icon = "RiMessage3Line";
                menu.ParentId = qualityComplaintMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:quality:complaint:customercomplaint:list";
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
                menu.I18nKey = "menu.logistics.quality.complaint.customercomplainthandling";
                menu.Icon = "RiFileEditLine";
                menu.ParentId = qualityComplaintMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:quality:complaint:customercomplainthandling:list";
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
                menu.I18nKey = "menu.logistics.quality.complaint.customersatisfactionsurvey";
                menu.Icon = "RiSurveyLine";
                menu.ParentId = qualityComplaintMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:quality:complaint:customersatisfactionsurvey:list";
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
                menu.I18nKey = "menu.logistics.quality.complaint.supplierevaluation";
                menu.Icon = "RiStarLine";
                menu.ParentId = qualityComplaintMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:quality:complaint:supplierevaluation:list";
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

        // ========== 财务统计下的四级菜单 (STATISTICS_REPORT_FINANCIAL) ==========
        if (statisticsReportFinancialMenu != null)
        {
            var (insertSRF1, updateSRF1) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "STATISTICS_REPORT_FINANCIAL_MANAGEMENT", menu =>
            {
                menu.MenuName = "管理统计";
                menu.MenuCode = "STATISTICS_REPORT_FINANCIAL_MANAGEMENT";
                menu.I18nKey = "menu.statistics.report.financial.management";
                menu.Icon = "RiPieChartLine";
                menu.ParentId = statisticsReportFinancialMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "statistics:report:financial:management:list";
                menu.RoutePath = "/statistics/report/financial/management";
                menu.ComponentPath = "statistics/report/financial/management/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertSRF1;
            updateCount += updateSRF1;

            var (insertSRF2, updateSRF2) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "STATISTICS_REPORT_FINANCIAL_CONTROLLING", menu =>
            {
                menu.MenuName = "控制统计";
                menu.MenuCode = "STATISTICS_REPORT_FINANCIAL_CONTROLLING";
                menu.I18nKey = "menu.statistics.report.financial.controlling";
                menu.Icon = "RiFundsLine";
                menu.ParentId = statisticsReportFinancialMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "statistics:report:financial:controlling:list";
                menu.RoutePath = "/statistics/report/financial/controlling";
                menu.ComponentPath = "statistics/report/financial/controlling/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertSRF2;
            updateCount += updateSRF2;
        }

        // ========== 人力统计下的四级菜单 (STATISTICS_REPORT_HUMANRESOURCE) ==========
        if (statisticsReportHumanResourceMenu != null)
        {
            var (insertSRH1, updateSRH1) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "STATISTICS_REPORT_HUMANRESOURCE_ATTENDANCE", menu =>
            {
                menu.MenuName = "考勤统计";
                menu.MenuCode = "STATISTICS_REPORT_HUMANRESOURCE_ATTENDANCE";
                menu.I18nKey = "menu.statistics.report.humanresource.attendance";
                menu.Icon = "RiCalendarCheckLine";
                menu.ParentId = statisticsReportHumanResourceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "statistics:report:humanresource:attendance:list";
                menu.RoutePath = "/statistics/report/human-resource/attendance";
                menu.ComponentPath = "statistics/report/human-resource/attendance/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertSRH1;
            updateCount += updateSRH1;

            var (insertSRH2, updateSRH2) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "STATISTICS_REPORT_HUMANRESOURCE_PERSONNEL", menu =>
            {
                menu.MenuName = "人事统计";
                menu.MenuCode = "STATISTICS_REPORT_HUMANRESOURCE_PERSONNEL";
                menu.I18nKey = "menu.statistics.report.humanresource.personnel";
                menu.Icon = "RiUserSettingsLine";
                menu.ParentId = statisticsReportHumanResourceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "statistics:report:humanresource:personnel:list";
                menu.RoutePath = "/statistics/report/human-resource/personnel";
                menu.ComponentPath = "statistics/report/human-resource/personnel/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertSRH2;
            updateCount += updateSRH2;

            var (insertSRH3, updateSRH3) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "STATISTICS_REPORT_HUMANRESOURCE_TALENT", menu =>
            {
                menu.MenuName = "人才统计";
                menu.MenuCode = "STATISTICS_REPORT_HUMANRESOURCE_TALENT";
                menu.I18nKey = "menu.statistics.report.humanresource.talent";
                menu.Icon = "RiUserStarLine";
                menu.ParentId = statisticsReportHumanResourceMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "statistics:report:humanresource:talent:list";
                menu.RoutePath = "/statistics/report/human-resource/talent";
                menu.ComponentPath = "statistics/report/human-resource/talent/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertSRH3;
            updateCount += updateSRH3;
        }

        // ========== 后勤统计下的四级菜单 (STATISTICS_REPORT_LOGISTICS) ==========
        if (statisticsReportLogisticsMenu != null)
        {
            var (insertSRL1, updateSRL1) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "STATISTICS_REPORT_LOGISTICS_MAINTENANCE", menu =>
            {
                menu.MenuName = "维护统计";
                menu.MenuCode = "STATISTICS_REPORT_LOGISTICS_MAINTENANCE";
                menu.I18nKey = "menu.statistics.report.logistics.maintenance";
                menu.Icon = "RiToolsLine";
                menu.ParentId = statisticsReportLogisticsMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "statistics:report:logistics:maintenance:list";
                menu.RoutePath = "/statistics/report/logistics/maintenance";
                menu.ComponentPath = "statistics/report/logistics/maintenance/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertSRL1;
            updateCount += updateSRL1;

            var (insertSRL2, updateSRL2) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "STATISTICS_REPORT_LOGISTICS_MANUFACTURING", menu =>
            {
                menu.MenuName = "生产统计";
                menu.MenuCode = "STATISTICS_REPORT_LOGISTICS_MANUFACTURING";
                menu.I18nKey = "menu.statistics.report.logistics.manufacturing";
                menu.Icon = "RiPlantLine";
                menu.ParentId = statisticsReportLogisticsMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "statistics:report:logistics:manufacturing:list";
                menu.RoutePath = "/statistics/report/logistics/manufacturing";
                menu.ComponentPath = "statistics/report/logistics/manufacturing/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertSRL2;
            updateCount += updateSRL2;

            var (insertSRL3, updateSRL3) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "STATISTICS_REPORT_LOGISTICS_MATERIAL", menu =>
            {
                menu.MenuName = "物料统计";
                menu.MenuCode = "STATISTICS_REPORT_LOGISTICS_MATERIAL";
                menu.I18nKey = "menu.statistics.report.logistics.material";
                menu.Icon = "RiBox3Line";
                menu.ParentId = statisticsReportLogisticsMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "statistics:report:logistics:material:list";
                menu.RoutePath = "/statistics/report/logistics/material";
                menu.ComponentPath = "statistics/report/logistics/material/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertSRL3;
            updateCount += updateSRL3;

            var (insertSRL4, updateSRL4) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "STATISTICS_REPORT_LOGISTICS_QUALITY", menu =>
            {
                menu.MenuName = "质量统计";
                menu.MenuCode = "STATISTICS_REPORT_LOGISTICS_QUALITY";
                menu.I18nKey = "menu.statistics.report.logistics.quality";
                menu.Icon = "RiShieldCheckLine";
                menu.ParentId = statisticsReportLogisticsMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "statistics:report:logistics:quality:list";
                menu.RoutePath = "/statistics/report/logistics/quality";
                menu.ComponentPath = "statistics/report/logistics/quality/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertSRL4;
            updateCount += updateSRL4;

            var (insertSRL5, updateSRL5) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "STATISTICS_REPORT_LOGISTICS_SALES", menu =>
            {
                menu.MenuName = "销售统计";
                menu.MenuCode = "STATISTICS_REPORT_LOGISTICS_SALES";
                menu.I18nKey = "menu.statistics.report.logistics.sales";
                menu.Icon = "RiShoppingCartLine";
                menu.ParentId = statisticsReportLogisticsMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "statistics:report:logistics:sales:list";
                menu.RoutePath = "/statistics/report/logistics/sales";
                menu.ComponentPath = "statistics/report/logistics/sales/index";
                menu.SortOrder = 5;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertSRL5;
            updateCount += updateSRL5;

            var (insertSRL6, updateSRL6) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "STATISTICS_REPORT_LOGISTICS_SERIAL", menu =>
            {
                menu.MenuName = "序列号统计";
                menu.MenuCode = "STATISTICS_REPORT_LOGISTICS_SERIAL";
                menu.I18nKey = "menu.statistics.report.logistics.serial";
                menu.Icon = "RiBarcodeLine";
                menu.ParentId = statisticsReportLogisticsMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "statistics:report:logistics:serial:list";
                menu.RoutePath = "/statistics/report/logistics/serial";
                menu.ComponentPath = "statistics/report/logistics/serial/index";
                menu.SortOrder = 6;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertSRL6;
            updateCount += updateSRL6;
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
