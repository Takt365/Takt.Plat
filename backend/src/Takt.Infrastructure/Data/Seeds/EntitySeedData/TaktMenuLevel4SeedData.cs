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
        var manufacturingBomMenu = await menuRepository.FirstAsync(m => m.MenuCode == "MANUFACTURING_BOM");
        var manufacturingSchedulingMenu = await menuRepository.FirstAsync(m => m.MenuCode == "MANUFACTURING_SCHEDULING");
        var manufacturingEcnMenu = await menuRepository.FirstAsync(m => m.MenuCode == "MANUFACTURING_ec");
        var manufacturingOutputMenu = await menuRepository.FirstAsync(m => m.MenuCode == "MANUFACTURING_OUTPUT");
        var manufacturingDefectMenu = await menuRepository.FirstAsync(m => m.MenuCode == "MANUFACTURING_DEFECT");
        var qualityCostMenu = await menuRepository.FirstAsync(m => m.MenuCode == "LOGISTICS_QUALITY_COST");
        var qualityOperationMenu = await menuRepository.FirstAsync(m => m.MenuCode == "LOGISTICS_QUALITY_OPERATION");
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
            var (insert00, update00) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "MANUFACTURING_BOM_MODEL_DESTINATION", menu =>
            {
                menu.MenuName = "机种仕向";
                menu.MenuCode = "MANUFACTURING_BOM_MODEL_DESTINATION";
                menu.I18nKey = "menu.logistics.manufacturing.bom.modeldestination";
                menu.Icon = "RiEarthLine";
                menu.ParentId = manufacturingBomMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "manufacturing:bom:modeldestination:list";
                menu.RoutePath = "/manufacturing/bom/model-destination";
                menu.ComponentPath = "manufacturing/bom/model-destination/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insert00;
            updateCount += update00;

            var (insertBOM2, updateBOM2) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "MANUFACTURING_BOM_LIST", menu =>
            {
                menu.MenuName = "物料清单";
                menu.MenuCode = "MANUFACTURING_BOM_LIST";
                menu.I18nKey = "menu.logistics.manufacturing.bom.list";
                menu.Icon = "RiFileList2Line";
                menu.ParentId = manufacturingBomMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "manufacturing:bom:list:list";
                menu.RoutePath = "/manufacturing/bom/list";
                menu.ComponentPath = "manufacturing/bom/list/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertBOM2;
            updateCount += updateBOM2;

            var (insertBOM3, updateBOM3) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "MANUFACTURING_BOM_ROUTING", menu =>
            {
                menu.MenuName = "工艺路线";
                menu.MenuCode = "MANUFACTURING_BOM_ROUTING";
                menu.I18nKey = "menu.logistics.manufacturing.bom.routin";
                menu.Icon = "RiRouteLine";
                menu.ParentId = manufacturingBomMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "manufacturing:bom:routing:list";
                menu.RoutePath = "/manufacturing/bom/routing";
                menu.ComponentPath = "manufacturing/bom/routing/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertBOM3;
            updateCount += updateBOM3;
        }

        // ========== 生产排程下的四级菜单 ==========
        if (manufacturingSchedulingMenu != null)
        {
            var (insertSCH1, updateSCH1) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "MANUFACTURING_SCHEDULING_WEEKLY", menu =>
            {
                menu.MenuName = "周排程";
                menu.MenuCode = "MANUFACTURING_SCHEDULING_WEEKLY";
                menu.I18nKey = "menu.logistics.manufacturing.scheduling.weekly";
                menu.Icon = "RiCalendarScheduleLine";
                menu.ParentId = manufacturingSchedulingMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "manufacturing:scheduling:weekly:list";
                menu.RoutePath = "/manufacturing/scheduling/weekly";
                menu.ComponentPath = "manufacturing/scheduling/weekly/index";
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
        if (manufacturingEcnMenu != null)
        {
            var (insertECN1, updateECN1) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "MANUFACTURING_ec_KANBAN", menu =>
            {
                menu.MenuName = "设变看板";
                menu.MenuCode = "MANUFACTURING_ec_KANBAN";
                menu.I18nKey = "menu.logistics.manufacturing.ecn.kanban";
                menu.Icon = "RiDashboardLine";
                menu.ParentId = manufacturingEcnMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "manufacturing:ecn:kanban:list";
                menu.RoutePath = "/manufacturing/ecn/kanban";
                menu.ComponentPath = "manufacturing/ecn/kanban/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertECN1;
            updateCount += updateECN1;

            var (insertECN2, updateECN2) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "MANUFACTURING_ec_BATCH", menu =>
            {
                menu.MenuName = "投入批次";
                menu.MenuCode = "MANUFACTURING_ec_BATCH";
                menu.I18nKey = "menu.logistics.manufacturing.ecn.batch";
                menu.Icon = "RiListCheck";
                menu.ParentId = manufacturingEcnMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "manufacturing:ecn:batch:list";
                menu.RoutePath = "/manufacturing/ecn/batch";
                menu.ComponentPath = "manufacturing/ecn/batch/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertECN2;
            updateCount += updateECN2;

            var (insertECN3, updateECN3) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "MANUFACTURING_ec_KAKUNIN", menu =>
            {
                menu.MenuName = "物料确认";
                menu.MenuCode = "MANUFACTURING_ec_KAKUNIN";
                menu.I18nKey = "menu.logistics.manufacturing.ecn.kakunin";
                menu.Icon = "RiCheckboxCircleLine";
                menu.ParentId = manufacturingEcnMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "manufacturing:ecn:kakunin:list";
                menu.RoutePath = "/manufacturing/ecn/kakunin";
                menu.ComponentPath = "manufacturing/ecn/kakunin/index";
                menu.SortOrder = 3;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertECN3;
            updateCount += updateECN3;

            var (insertECN4, updateECN4) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "MANUFACTURING_ec_GIJUTSU", menu =>
            {
                menu.MenuName = "技术部门";
                menu.MenuCode = "MANUFACTURING_ec_GIJUTSU";
                menu.I18nKey = "menu.logistics.manufacturing.ecn.gijutsu";
                menu.Icon = "RiCpuLine";
                menu.ParentId = manufacturingEcnMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "manufacturing:ecn:gijutsu:list";
                menu.RoutePath = "/manufacturing/ecn/gijutsu";
                menu.ComponentPath = "manufacturing/ecn/gijutsu/index";
                menu.SortOrder = 4;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertECN4;
            updateCount += updateECN4;

            var (insertECN5, updateECN5) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "MANUFACTURING_ec_KOUBAI", menu =>
            {
                menu.MenuName = "采购部门";
                menu.MenuCode = "MANUFACTURING_ec_KOUBAI";
                menu.I18nKey = "menu.logistics.manufacturing.ecn.koubai";
                menu.Icon = "RiShoppingCart2Line";
                menu.ParentId = manufacturingEcnMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "manufacturing:ecn:koubai:list";
                menu.RoutePath = "/manufacturing/ecn/koubai";
                menu.ComponentPath = "manufacturing/ecn/koubai/index";
                menu.SortOrder = 5;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertECN5;
            updateCount += updateECN5;

            var (insertECN6, updateECN6) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "MANUFACTURING_ec_SEIKAN", menu =>
            {
                menu.MenuName = "生管部门";
                menu.MenuCode = "MANUFACTURING_ec_SEIKAN";
                menu.I18nKey = "menu.logistics.manufacturing.ecn.seikan";
                menu.Icon = "RiSettings3Line";
                menu.ParentId = manufacturingEcnMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "manufacturing:ecn:seikan:list";
                menu.RoutePath = "/manufacturing/ecn/seikan";
                menu.ComponentPath = "manufacturing/ecn/seikan/index";
                menu.SortOrder = 6;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertECN6;
            updateCount += updateECN6;

            var (insertECN7, updateECN7) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "MANUFACTURING_ec_UKEKEN", menu =>
            {
                menu.MenuName = "受检部门";
                menu.MenuCode = "MANUFACTURING_ec_UKEKEN";
                menu.I18nKey = "menu.logistics.manufacturing.ecn.ukeken";
                menu.Icon = "RiSearchEyeLine";
                menu.ParentId = manufacturingEcnMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "manufacturing:ecn:ukeken:list";
                menu.RoutePath = "/manufacturing/ecn/ukeken";
                menu.ComponentPath = "manufacturing/ecn/ukeken/index";
                menu.SortOrder = 7;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertECN7;
            updateCount += updateECN7;

            var (insertECN8, updateECN8) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "MANUFACTURING_ec_BUKAN", menu =>
            {
                menu.MenuName = "部管部门";
                menu.MenuCode = "MANUFACTURING_ec_BUKAN";
                menu.I18nKey = "menu.logistics.manufacturing.ecn.bukan";
                menu.Icon = "RiArchiveDrawerLine";
                menu.ParentId = manufacturingEcnMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "manufacturing:ecn:bukan:list";
                menu.RoutePath = "/manufacturing/ecn/bukan";
                menu.ComponentPath = "manufacturing/ecn/bukan/index";
                menu.SortOrder = 8;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertECN8;
            updateCount += updateECN8;

            var (insertECN9, updateECN9) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "MANUFACTURING_ec_SEIZONIKA", menu =>
            {
                menu.MenuName = "制造二课";
                menu.MenuCode = "MANUFACTURING_ec_SEIZONIKA";
                menu.I18nKey = "menu.logistics.manufacturing.ecn.seizonika";
                menu.Icon = "RiFactoryLine";
                menu.ParentId = manufacturingEcnMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "manufacturing:ecn:seizonika:list";
                menu.RoutePath = "/manufacturing/ecn/seizonika";
                menu.ComponentPath = "manufacturing/ecn/seizonika/index";
                menu.SortOrder = 9;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertECN9;
            updateCount += updateECN9;

            var (insertECN10, updateECN10) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "MANUFACTURING_ec_SEIZOIKKA", menu =>
            {
                menu.MenuName = "制造一课";
                menu.MenuCode = "MANUFACTURING_ec_SEIZOIKKA";
                menu.I18nKey = "menu.logistics.manufacturing.ecn.seizoikka";
                menu.Icon = "RiPlantLine";
                menu.ParentId = manufacturingEcnMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "manufacturing:ecn:seizoikka:list";
                menu.RoutePath = "/manufacturing/ecn/seizoikka";
                menu.ComponentPath = "manufacturing/ecn/seizoikka/index";
                menu.SortOrder = 10;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertECN10;
            updateCount += updateECN10;

            var (insertECN11, updateECN11) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "MANUFACTURING_ec_HINKAN", menu =>
            {
                menu.MenuName = "品管部门";
                menu.MenuCode = "MANUFACTURING_ec_HINKAN";
                menu.I18nKey = "menu.logistics.manufacturing.ecn.hinkan";
                menu.Icon = "RiShieldCheckLine";
                menu.ParentId = manufacturingEcnMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "manufacturing:ecn:hinkan:list";
                menu.RoutePath = "/manufacturing/ecn/hinkan";
                menu.ComponentPath = "manufacturing/ecn/hinkan/index";
                menu.SortOrder = 11;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertECN11;
            updateCount += updateECN11;

            var (insertECN12, updateECN12) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "MANUFACTURING_ec_LEGACY_PRODUCT", menu =>
            {
                menu.MenuName = "旧品管制";
                menu.MenuCode = "MANUFACTURING_ec_LEGACY_PRODUCT";
                menu.I18nKey = "menu.logistics.manufacturing.ecn.legacyproduct";
                menu.Icon = "RiTimeLine";
                menu.ParentId = manufacturingEcnMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "manufacturing:ecn:legacyproduct:list";
                menu.RoutePath = "/manufacturing/ecn/legacy-product";
                menu.ComponentPath = "manufacturing/ecn/legacy-product/index";
                menu.SortOrder = 12;
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
            var (insertOUT1, updateOUT1) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "MANUFACTURING_OUTPUT_PCBA", menu =>
            {
                menu.MenuName = "PCB生产";
                menu.MenuCode = "MANUFACTURING_OUTPUT_PCBA";
                menu.I18nKey = "menu.logistics.manufacturing.output.pcba._self";
                menu.Icon = "RiCpuLine";
                menu.ParentId = manufacturingOutputMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "manufacturing:output:pcba:list";
                menu.RoutePath = "/manufacturing/output/pcba";
                menu.ComponentPath = "manufacturing/output/pcba/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertOUT1;
            updateCount += updateOUT1;

            var (insertOUT2, updateOUT2) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "MANUFACTURING_OUTPUT_ASSEMBLY", menu =>
            {
                menu.MenuName = "组立生产";
                menu.MenuCode = "MANUFACTURING_OUTPUT_ASSEMBLY";
                menu.I18nKey = "menu.logistics.manufacturing.output.assembly._self";
                menu.Icon = "RiSettings4Line";
                menu.ParentId = manufacturingOutputMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "manufacturing:output:assembly:list";
                menu.RoutePath = "/manufacturing/output/assembly";
                menu.ComponentPath = "manufacturing/output/assembly/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertOUT2;
            updateCount += updateOUT2;
        }

        // ========== 不良管理下的四级菜单 ==========
        if (manufacturingDefectMenu != null)
        {
            var (insertDEF1, updateDEF1) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "MANUFACTURING_DEFECT_PCBA", menu =>
            {
                menu.MenuName = "PCBA不良";
                menu.MenuCode = "MANUFACTURING_DEFECT_PCBA";
                menu.I18nKey = "menu.logistics.manufacturing.defect.pcba._self";
                menu.Icon = "RiErrorWarningLine";
                menu.ParentId = manufacturingDefectMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "manufacturing:defect:pcba:list";
                menu.RoutePath = "/manufacturing/defect/pcba";
                menu.ComponentPath = "manufacturing/defect/pcba/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertDEF1;
            updateCount += updateDEF1;

            var (insertDEF2, updateDEF2) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "MANUFACTURING_DEFECT_ASSEMBLY", menu =>
            {
                menu.MenuName = "组立不良";
                menu.MenuCode = "MANUFACTURING_DEFECT_ASSEMBLY";
                menu.I18nKey = "menu.logistics.manufacturing.defect.assembly._self";
                menu.Icon = "RiAlarmWarningLine";
                menu.ParentId = manufacturingDefectMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "manufacturing:defect:assembly:list";
                menu.RoutePath = "/manufacturing/defect/assembly";
                menu.ComponentPath = "manufacturing/defect/assembly/index";
                menu.SortOrder = 2;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertDEF2;
            updateCount += updateDEF2;
        }

        // ========== 品质成本下的四级菜单 ==========
        if (qualityCostMenu != null)
        {
            var (insertQC1, updateQC1) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "QUALITY_COST_OPERATION", menu =>
            {
                menu.MenuName = "品质业务";
                menu.MenuCode = "QUALITY_COST_OPERATION";
                menu.I18nKey = "menu.logistics.quality.cost.operation";
                menu.Icon = "RiShieldCheckLine";
                menu.ParentId = qualityCostMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:quality:cost:operation:list";
                menu.RoutePath = "/logistics/quality/cost/operation";
                menu.ComponentPath = "logistics/quality/cost/operation/index";
                menu.SortOrder = 1;
                menu.MenuStatus = 1;
                menu.IsVisible = 1;
                menu.IsCached = 0;
                menu.IsExternal = 0;
            });
            insertCount += insertQC1;
            updateCount += updateQC1;

            var (insertQC2, updateQC2) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "QUALITY_COST_ISSUE", menu =>
            {
                menu.MenuName = "品质问题";
                menu.MenuCode = "QUALITY_COST_ISSUE";
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

            var (insertQC3, updateQC3) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "QUALITY_COST_SCRAP", menu =>
            {
                menu.MenuName = "品质事故";
                menu.MenuCode = "QUALITY_COST_SCRAP";
                menu.I18nKey = "menu.logistics.quality.cost.scrap";
                menu.Icon = "RiAlarmWarningLine";
                menu.ParentId = qualityCostMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "logistics:quality:cost:scrap:list";
                menu.RoutePath = "/logistics/quality/cost/scrap";
                menu.ComponentPath = "logistics/quality/cost/scrap/index";
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
            var (insertQO1, updateQO1) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "QUALITY_OPERATION_SAMPLING_SCHEME", menu =>
            {
                menu.MenuName = "抽样方案";
                menu.MenuCode = "QUALITY_OPERATION_SAMPLING_SCHEME";
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

            var (insertQO2, updateQO2) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "QUALITY_OPERATION_INSPECTION_STANDARD", menu =>
            {
                menu.MenuName = "检验标准";
                menu.MenuCode = "QUALITY_OPERATION_INSPECTION_STANDARD";
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

            var (insertQO3, updateQO3) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "QUALITY_OPERATION_IQC_ORDER", menu =>
            {
                menu.MenuName = "进货检验";
                menu.MenuCode = "QUALITY_OPERATION_IQC_ORDER";
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

            var (insertQO4, updateQO4) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "QUALITY_OPERATION_IPQC_ORDER", menu =>
            {
                menu.MenuName = "制程检验";
                menu.MenuCode = "QUALITY_OPERATION_IPQC_ORDER";
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

            var (insertQO5, updateQO5) = await CreateOrUpdateMenuAsync(menuRepository, sqlSugarContext, tenantCode, "QUALITY_OPERATION_FQC_ORDER", menu =>
            {
                menu.MenuName = "入库检验";
                menu.MenuCode = "QUALITY_OPERATION_FQC_ORDER";
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
                menu.Permission = "statistics:report:financial:management:query";
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
                menu.Permission = "statistics:report:financial:controlling:query";
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
                menu.Permission = "statistics:report:humanresource:attendance:query";
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
                menu.Permission = "statistics:report:humanresource:personnel:query";
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
                menu.Permission = "statistics:report:humanresource:talent:query";
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
                menu.Permission = "statistics:report:logistics:maintenance:query";
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
                menu.Icon = "RiFactoryLine";
                menu.ParentId = statisticsReportLogisticsMenu.Id;
                menu.MenuType = 1;
                menu.Permission = "statistics:report:logistics:manufacturing:query";
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
                menu.Permission = "statistics:report:logistics:material:query";
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
                menu.Permission = "statistics:report:logistics:quality:query";
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
                menu.Permission = "statistics:report:logistics:sales:query";
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
                menu.Permission = "statistics:report:logistics:serial:query";
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
