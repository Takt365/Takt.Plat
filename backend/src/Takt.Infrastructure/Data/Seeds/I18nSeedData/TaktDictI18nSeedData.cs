// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData
// 文件名称：TaktDictI18nSeedData.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：字典项国际化翻译种子（dict.* 键，与 TaktDictDataSeedData I18nKey 对齐）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险.
// ========================================

using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData;

/// <summary>
/// 字典项国际化翻译种子（键前缀 dict.*）
/// 幂等性：存在则更新，不存在则创建
/// TranslationText 为字典显示标签；ContextNote 为 TaktDictDataSeedData.Remark
/// 部门类字典项 I18nKey 复用 org.dept.*，由 TaktDeptI18nSeedData 提供翻译，不在此重复
/// </summary>
public class TaktDictI18nSeedData : ITaktSeedDataCoordinator
{
    /// <summary>执行顺序（在通用翻译之后、部门翻译之前）</summary>
    public int Order => 50;

    /// <summary>初始化字典项国际化翻译种子</summary>
    /// <param name="serviceProvider">服务提供者</param>
    /// <param name="tenantCode">租户编码</param>
    /// <returns>插入数与更新数</returns>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(IServiceProvider serviceProvider, string? tenantCode = null)
    {
        TaktLogger.Information("开始初始化字典项国际化翻译种子...");

        if (string.IsNullOrEmpty(tenantCode))
        {
            TaktLogger.Warning("租户编码为空，跳过字典项国际化翻译种子");
            return (0, 0);
        }

        var repository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktTranslation>>();
        var cultureRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktCulture>>();
        var cultureIdByCode = (await cultureRepository.GetListAsync(c => c.TenantCode == tenantCode))
            .ToDictionary(c => c.CultureCode, c => c.Id);
        int insertCount = 0;
        int updateCount = 0;

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 dict.* 翻译...", tenantCode);

        foreach (var row in GetDictTranslations())
        {
            if (!cultureIdByCode.TryGetValue(row.CultureCode, out var cultureId))
            {
                TaktLogger.Warning("未找到区域文化 {CultureCode}，跳过翻译 {I18nKey}", row.CultureCode, row.I18nKey);
                continue;
            }

            var item = new TranslationSeedItem(row.I18nKey, row.CultureCode, row.TranslationText, row.ContextNote);
            var (_, i, u) = await CreateOrUpdateTranslationAsync(repository, tenantCode, cultureId, item);
            insertCount += i;
            updateCount += u;
        }

        TaktLogger.Information("字典项国际化翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);

        return (insertCount, updateCount);
    }

    /// <summary>字典项翻译列表（en-US / ja-JP / zh-CN / zh-HK）</summary>
    private static List<(string I18nKey, string CultureCode, string TranslationText, string? ContextNote)> GetDictTranslations()
    {
        return new List<(string, string, string, string?)>
        {
            // dict.accounting.account.category.asset
            ("dict.accounting.account.category.asset", "en-US", "资产类_us", "科目类别.资产类"),
            // dict.accounting.account.category.asset
            ("dict.accounting.account.category.asset", "ja-JP", "资产类_jp", "科目类别.资产类"),
            // dict.accounting.account.category.asset
            ("dict.accounting.account.category.asset", "zh-CN", "资产类", "科目类别.资产类"),
            // dict.accounting.account.category.asset
            ("dict.accounting.account.category.asset", "zh-HK", "资产类_hk", "科目类别.资产类"),

            // dict.accounting.account.category.liability
            ("dict.accounting.account.category.liability", "en-US", "负债类_us", "科目类别.负债类"),
            // dict.accounting.account.category.liability
            ("dict.accounting.account.category.liability", "ja-JP", "负债类_jp", "科目类别.负债类"),
            // dict.accounting.account.category.liability
            ("dict.accounting.account.category.liability", "zh-CN", "负债类", "科目类别.负债类"),
            // dict.accounting.account.category.liability
            ("dict.accounting.account.category.liability", "zh-HK", "负债类_hk", "科目类别.负债类"),

            // dict.accounting.account.category.equity
            ("dict.accounting.account.category.equity", "en-US", "权益类_us", "科目类别.权益类"),
            // dict.accounting.account.category.equity
            ("dict.accounting.account.category.equity", "ja-JP", "权益类_jp", "科目类别.权益类"),
            // dict.accounting.account.category.equity
            ("dict.accounting.account.category.equity", "zh-CN", "权益类", "科目类别.权益类"),
            // dict.accounting.account.category.equity
            ("dict.accounting.account.category.equity", "zh-HK", "权益类_hk", "科目类别.权益类"),

            // dict.accounting.account.category.cost
            ("dict.accounting.account.category.cost", "en-US", "成本类_us", "科目类别.成本类"),
            // dict.accounting.account.category.cost
            ("dict.accounting.account.category.cost", "ja-JP", "成本类_jp", "科目类别.成本类"),
            // dict.accounting.account.category.cost
            ("dict.accounting.account.category.cost", "zh-CN", "成本类", "科目类别.成本类"),
            // dict.accounting.account.category.cost
            ("dict.accounting.account.category.cost", "zh-HK", "成本类_hk", "科目类别.成本类"),

            // dict.accounting.account.category.profit_loss
            ("dict.accounting.account.category.profit_loss", "en-US", "损益类_us", "科目类别.损益类"),
            // dict.accounting.account.category.profit_loss
            ("dict.accounting.account.category.profit_loss", "ja-JP", "损益类_jp", "科目类别.损益类"),
            // dict.accounting.account.category.profit_loss
            ("dict.accounting.account.category.profit_loss", "zh-CN", "损益类", "科目类别.损益类"),
            // dict.accounting.account.category.profit_loss
            ("dict.accounting.account.category.profit_loss", "zh-HK", "损益类_hk", "科目类别.损益类"),

            // dict.accounting.account.category.revenue
            ("dict.accounting.account.category.revenue", "en-US", "收入类_us", "科目类别.收入类"),
            // dict.accounting.account.category.revenue
            ("dict.accounting.account.category.revenue", "ja-JP", "收入类_jp", "科目类别.收入类"),
            // dict.accounting.account.category.revenue
            ("dict.accounting.account.category.revenue", "zh-CN", "收入类", "科目类别.收入类"),
            // dict.accounting.account.category.revenue
            ("dict.accounting.account.category.revenue", "zh-HK", "收入类_hk", "科目类别.收入类"),

            // dict.accounting.account.category.expense
            ("dict.accounting.account.category.expense", "en-US", "费用类_us", "科目类别.费用类"),
            // dict.accounting.account.category.expense
            ("dict.accounting.account.category.expense", "ja-JP", "费用类_jp", "科目类别.费用类"),
            // dict.accounting.account.category.expense
            ("dict.accounting.account.category.expense", "zh-CN", "费用类", "科目类别.费用类"),
            // dict.accounting.account.category.expense
            ("dict.accounting.account.category.expense", "zh-HK", "费用类_hk", "科目类别.费用类"),

            // dict.accounting.asset.category.building
            ("dict.accounting.asset.category.building", "en-US", "房屋建筑_us", "资产类别.房屋建筑"),
            // dict.accounting.asset.category.building
            ("dict.accounting.asset.category.building", "ja-JP", "房屋建筑_jp", "资产类别.房屋建筑"),
            // dict.accounting.asset.category.building
            ("dict.accounting.asset.category.building", "zh-CN", "房屋建筑", "资产类别.房屋建筑"),
            // dict.accounting.asset.category.building
            ("dict.accounting.asset.category.building", "zh-HK", "房屋建筑_hk", "资产类别.房屋建筑"),

            // dict.accounting.asset.category.machinery
            ("dict.accounting.asset.category.machinery", "en-US", "机器设备_us", "资产类别.机器设备"),
            // dict.accounting.asset.category.machinery
            ("dict.accounting.asset.category.machinery", "ja-JP", "机器设备_jp", "资产类别.机器设备"),
            // dict.accounting.asset.category.machinery
            ("dict.accounting.asset.category.machinery", "zh-CN", "机器设备", "资产类别.机器设备"),
            // dict.accounting.asset.category.machinery
            ("dict.accounting.asset.category.machinery", "zh-HK", "机器设备_hk", "资产类别.机器设备"),

            // dict.accounting.asset.category.vehicle
            ("dict.accounting.asset.category.vehicle", "en-US", "运输工具_us", "资产类别.运输工具"),
            // dict.accounting.asset.category.vehicle
            ("dict.accounting.asset.category.vehicle", "ja-JP", "运输工具_jp", "资产类别.运输工具"),
            // dict.accounting.asset.category.vehicle
            ("dict.accounting.asset.category.vehicle", "zh-CN", "运输工具", "资产类别.运输工具"),
            // dict.accounting.asset.category.vehicle
            ("dict.accounting.asset.category.vehicle", "zh-HK", "运输工具_hk", "资产类别.运输工具"),

            // dict.accounting.asset.category.electronic
            ("dict.accounting.asset.category.electronic", "en-US", "电子设备_us", "资产类别.电子设备"),
            // dict.accounting.asset.category.electronic
            ("dict.accounting.asset.category.electronic", "ja-JP", "电子设备_jp", "资产类别.电子设备"),
            // dict.accounting.asset.category.electronic
            ("dict.accounting.asset.category.electronic", "zh-CN", "电子设备", "资产类别.电子设备"),
            // dict.accounting.asset.category.electronic
            ("dict.accounting.asset.category.electronic", "zh-HK", "电子设备_hk", "资产类别.电子设备"),

            // dict.accounting.asset.category.office_equip
            ("dict.accounting.asset.category.office_equip", "en-US", "办公设备_us", "资产类别.办公设备"),
            // dict.accounting.asset.category.office_equip
            ("dict.accounting.asset.category.office_equip", "ja-JP", "办公设备_jp", "资产类别.办公设备"),
            // dict.accounting.asset.category.office_equip
            ("dict.accounting.asset.category.office_equip", "zh-CN", "办公设备", "资产类别.办公设备"),
            // dict.accounting.asset.category.office_equip
            ("dict.accounting.asset.category.office_equip", "zh-HK", "办公设备_hk", "资产类别.办公设备"),

            // dict.accounting.asset.category.furniture
            ("dict.accounting.asset.category.furniture", "en-US", "家具用具_us", "资产类别.家具用具"),
            // dict.accounting.asset.category.furniture
            ("dict.accounting.asset.category.furniture", "ja-JP", "家具用具_jp", "资产类别.家具用具"),
            // dict.accounting.asset.category.furniture
            ("dict.accounting.asset.category.furniture", "zh-CN", "家具用具", "资产类别.家具用具"),
            // dict.accounting.asset.category.furniture
            ("dict.accounting.asset.category.furniture", "zh-HK", "家具用具_hk", "资产类别.家具用具"),

            // dict.accounting.asset.category.intangible
            ("dict.accounting.asset.category.intangible", "en-US", "无形资产_us", "资产类别.无形资产"),
            // dict.accounting.asset.category.intangible
            ("dict.accounting.asset.category.intangible", "ja-JP", "无形资产_jp", "资产类别.无形资产"),
            // dict.accounting.asset.category.intangible
            ("dict.accounting.asset.category.intangible", "zh-CN", "无形资产", "资产类别.无形资产"),
            // dict.accounting.asset.category.intangible
            ("dict.accounting.asset.category.intangible", "zh-HK", "无形资产_hk", "资产类别.无形资产"),

            // dict.accounting.asset.category.land_use_right
            ("dict.accounting.asset.category.land_use_right", "en-US", "土地使用权_us", "资产类别.土地使用权"),
            // dict.accounting.asset.category.land_use_right
            ("dict.accounting.asset.category.land_use_right", "ja-JP", "土地使用权_jp", "资产类别.土地使用权"),
            // dict.accounting.asset.category.land_use_right
            ("dict.accounting.asset.category.land_use_right", "zh-CN", "土地使用权", "资产类别.土地使用权"),
            // dict.accounting.asset.category.land_use_right
            ("dict.accounting.asset.category.land_use_right", "zh-HK", "土地使用权_hk", "资产类别.土地使用权"),

            // dict.accounting.asset.category.software
            ("dict.accounting.asset.category.software", "en-US", "软件系统_us", "资产类别.软件系统"),
            // dict.accounting.asset.category.software
            ("dict.accounting.asset.category.software", "ja-JP", "软件系统_jp", "资产类别.软件系统"),
            // dict.accounting.asset.category.software
            ("dict.accounting.asset.category.software", "zh-CN", "软件系统", "资产类别.软件系统"),
            // dict.accounting.asset.category.software
            ("dict.accounting.asset.category.software", "zh-HK", "软件系统_hk", "资产类别.软件系统"),

            // dict.accounting.asset.category.other
            ("dict.accounting.asset.category.other", "en-US", "其他资产_us", "资产类别.其他资产"),
            // dict.accounting.asset.category.other
            ("dict.accounting.asset.category.other", "ja-JP", "其他资产_jp", "资产类别.其他资产"),
            // dict.accounting.asset.category.other
            ("dict.accounting.asset.category.other", "zh-CN", "其他资产", "资产类别.其他资产"),
            // dict.accounting.asset.category.other
            ("dict.accounting.asset.category.other", "zh-HK", "其他资产_hk", "资产类别.其他资产"),

            // dict.accounting.asset.type.fixed
            ("dict.accounting.asset.type.fixed", "en-US", "固定资产_us", "资产类型.固定资产"),
            // dict.accounting.asset.type.fixed
            ("dict.accounting.asset.type.fixed", "ja-JP", "固定资产_jp", "资产类型.固定资产"),
            // dict.accounting.asset.type.fixed
            ("dict.accounting.asset.type.fixed", "zh-CN", "固定资产", "资产类型.固定资产"),
            // dict.accounting.asset.type.fixed
            ("dict.accounting.asset.type.fixed", "zh-HK", "固定资产_hk", "资产类型.固定资产"),

            // dict.accounting.asset.type.intangible
            ("dict.accounting.asset.type.intangible", "en-US", "无形资产_us", "资产类型.无形资产"),
            // dict.accounting.asset.type.intangible
            ("dict.accounting.asset.type.intangible", "ja-JP", "无形资产_jp", "资产类型.无形资产"),
            // dict.accounting.asset.type.intangible
            ("dict.accounting.asset.type.intangible", "zh-CN", "无形资产", "资产类型.无形资产"),
            // dict.accounting.asset.type.intangible
            ("dict.accounting.asset.type.intangible", "zh-HK", "无形资产_hk", "资产类型.无形资产"),

            // dict.accounting.asset.type.investment
            ("dict.accounting.asset.type.investment", "en-US", "投资性房地产_us", "资产类型.投资性房地产"),
            // dict.accounting.asset.type.investment
            ("dict.accounting.asset.type.investment", "ja-JP", "投资性房地产_jp", "资产类型.投资性房地产"),
            // dict.accounting.asset.type.investment
            ("dict.accounting.asset.type.investment", "zh-CN", "投资性房地产", "资产类型.投资性房地产"),
            // dict.accounting.asset.type.investment
            ("dict.accounting.asset.type.investment", "zh-HK", "投资性房地产_hk", "资产类型.投资性房地产"),

            // dict.accounting.asset.type.construction
            ("dict.accounting.asset.type.construction", "en-US", "在建工程_us", "资产类型.在建工程"),
            // dict.accounting.asset.type.construction
            ("dict.accounting.asset.type.construction", "ja-JP", "在建工程_jp", "资产类型.在建工程"),
            // dict.accounting.asset.type.construction
            ("dict.accounting.asset.type.construction", "zh-CN", "在建工程", "资产类型.在建工程"),
            // dict.accounting.asset.type.construction
            ("dict.accounting.asset.type.construction", "zh-HK", "在建工程_hk", "资产类型.在建工程"),

            // dict.accounting.asset.type.lowvalue
            ("dict.accounting.asset.type.lowvalue", "en-US", "低值易耗品_us", "资产类型.低值易耗品"),
            // dict.accounting.asset.type.lowvalue
            ("dict.accounting.asset.type.lowvalue", "ja-JP", "低值易耗品_jp", "资产类型.低值易耗品"),
            // dict.accounting.asset.type.lowvalue
            ("dict.accounting.asset.type.lowvalue", "zh-CN", "低值易耗品", "资产类型.低值易耗品"),
            // dict.accounting.asset.type.lowvalue
            ("dict.accounting.asset.type.lowvalue", "zh-HK", "低值易耗品_hk", "资产类型.低值易耗品"),

            // dict.accounting.asset.type.deferred
            ("dict.accounting.asset.type.deferred", "en-US", "长期待摊费用_us", "资产类型.长期待摊费用"),
            // dict.accounting.asset.type.deferred
            ("dict.accounting.asset.type.deferred", "ja-JP", "长期待摊费用_jp", "资产类型.长期待摊费用"),
            // dict.accounting.asset.type.deferred
            ("dict.accounting.asset.type.deferred", "zh-CN", "长期待摊费用", "资产类型.长期待摊费用"),
            // dict.accounting.asset.type.deferred
            ("dict.accounting.asset.type.deferred", "zh-HK", "长期待摊费用_hk", "资产类型.长期待摊费用"),

            // dict.accounting.asset.type.other
            ("dict.accounting.asset.type.other", "en-US", "其他_us", "资产类型.其他"),
            // dict.accounting.asset.type.other
            ("dict.accounting.asset.type.other", "ja-JP", "其他_jp", "资产类型.其他"),
            // dict.accounting.asset.type.other
            ("dict.accounting.asset.type.other", "zh-CN", "其他", "资产类型.其他"),
            // dict.accounting.asset.type.other
            ("dict.accounting.asset.type.other", "zh-HK", "其他_hk", "资产类型.其他"),

            // dict.accounting.cost.center.category.pro
            ("dict.accounting.cost.center.category.pro", "en-US", "专业级_us", "成本中心类别.专业级"),
            // dict.accounting.cost.center.category.pro
            ("dict.accounting.cost.center.category.pro", "ja-JP", "专业级_jp", "成本中心类别.专业级"),
            // dict.accounting.cost.center.category.pro
            ("dict.accounting.cost.center.category.pro", "zh-CN", "专业级", "成本中心类别.专业级"),
            // dict.accounting.cost.center.category.pro
            ("dict.accounting.cost.center.category.pro", "zh-HK", "专业级_hk", "成本中心类别.专业级"),

            // dict.accounting.cost.center.category.cons
            ("dict.accounting.cost.center.category.cons", "en-US", "消费级_us", "成本中心类别.消费级"),
            // dict.accounting.cost.center.category.cons
            ("dict.accounting.cost.center.category.cons", "ja-JP", "消费级_jp", "成本中心类别.消费级"),
            // dict.accounting.cost.center.category.cons
            ("dict.accounting.cost.center.category.cons", "zh-CN", "消费级", "成本中心类别.消费级"),
            // dict.accounting.cost.center.category.cons
            ("dict.accounting.cost.center.category.cons", "zh-HK", "消费级_hk", "成本中心类别.消费级"),

            // dict.accounting.cost.center.category.medi
            ("dict.accounting.cost.center.category.medi", "en-US", "医用级_us", "成本中心类别.医用级"),
            // dict.accounting.cost.center.category.medi
            ("dict.accounting.cost.center.category.medi", "ja-JP", "医用级_jp", "成本中心类别.医用级"),
            // dict.accounting.cost.center.category.medi
            ("dict.accounting.cost.center.category.medi", "zh-CN", "医用级", "成本中心类别.医用级"),
            // dict.accounting.cost.center.category.medi
            ("dict.accounting.cost.center.category.medi", "zh-HK", "医用级_hk", "成本中心类别.医用级"),

            // dict.accounting.cost.center.category.info
            ("dict.accounting.cost.center.category.info", "en-US", "信息类_us", "成本中心类别.信息类"),
            // dict.accounting.cost.center.category.info
            ("dict.accounting.cost.center.category.info", "ja-JP", "信息类_jp", "成本中心类别.信息类"),
            // dict.accounting.cost.center.category.info
            ("dict.accounting.cost.center.category.info", "zh-CN", "信息类", "成本中心类别.信息类"),
            // dict.accounting.cost.center.category.info
            ("dict.accounting.cost.center.category.info", "zh-HK", "信息类_hk", "成本中心类别.信息类"),

            // dict.accounting.cost.center.category.ems
            ("dict.accounting.cost.center.category.ems", "en-US", "ems_us", "成本中心类别.ems"),
            // dict.accounting.cost.center.category.ems
            ("dict.accounting.cost.center.category.ems", "ja-JP", "ems_jp", "成本中心类别.ems"),
            // dict.accounting.cost.center.category.ems
            ("dict.accounting.cost.center.category.ems", "zh-CN", "ems", "成本中心类别.ems"),
            // dict.accounting.cost.center.category.ems
            ("dict.accounting.cost.center.category.ems", "zh-HK", "ems_hk", "成本中心类别.ems"),

            // dict.accounting.cost.element.category.direct_material
            ("dict.accounting.cost.element.category.direct_material", "en-US", "直接材料_us", "成本要素类别.直接材料"),
            // dict.accounting.cost.element.category.direct_material
            ("dict.accounting.cost.element.category.direct_material", "ja-JP", "直接材料_jp", "成本要素类别.直接材料"),
            // dict.accounting.cost.element.category.direct_material
            ("dict.accounting.cost.element.category.direct_material", "zh-CN", "直接材料", "成本要素类别.直接材料"),
            // dict.accounting.cost.element.category.direct_material
            ("dict.accounting.cost.element.category.direct_material", "zh-HK", "直接材料_hk", "成本要素类别.直接材料"),

            // dict.accounting.cost.element.category.direct_labor
            ("dict.accounting.cost.element.category.direct_labor", "en-US", "直接人工_us", "成本要素类别.直接人工"),
            // dict.accounting.cost.element.category.direct_labor
            ("dict.accounting.cost.element.category.direct_labor", "ja-JP", "直接人工_jp", "成本要素类别.直接人工"),
            // dict.accounting.cost.element.category.direct_labor
            ("dict.accounting.cost.element.category.direct_labor", "zh-CN", "直接人工", "成本要素类别.直接人工"),
            // dict.accounting.cost.element.category.direct_labor
            ("dict.accounting.cost.element.category.direct_labor", "zh-HK", "直接人工_hk", "成本要素类别.直接人工"),

            // dict.accounting.cost.element.category.manufacturing_overhead
            ("dict.accounting.cost.element.category.manufacturing_overhead", "en-US", "制造费用_us", "成本要素类别.制造费用"),
            // dict.accounting.cost.element.category.manufacturing_overhead
            ("dict.accounting.cost.element.category.manufacturing_overhead", "ja-JP", "制造费用_jp", "成本要素类别.制造费用"),
            // dict.accounting.cost.element.category.manufacturing_overhead
            ("dict.accounting.cost.element.category.manufacturing_overhead", "zh-CN", "制造费用", "成本要素类别.制造费用"),
            // dict.accounting.cost.element.category.manufacturing_overhead
            ("dict.accounting.cost.element.category.manufacturing_overhead", "zh-HK", "制造费用_hk", "成本要素类别.制造费用"),

            // dict.accounting.cost.element.category.depreciation
            ("dict.accounting.cost.element.category.depreciation", "en-US", "折旧费_us", "成本要素类别.折旧费"),
            // dict.accounting.cost.element.category.depreciation
            ("dict.accounting.cost.element.category.depreciation", "ja-JP", "折旧费_jp", "成本要素类别.折旧费"),
            // dict.accounting.cost.element.category.depreciation
            ("dict.accounting.cost.element.category.depreciation", "zh-CN", "折旧费", "成本要素类别.折旧费"),
            // dict.accounting.cost.element.category.depreciation
            ("dict.accounting.cost.element.category.depreciation", "zh-HK", "折旧费_hk", "成本要素类别.折旧费"),

            // dict.accounting.cost.element.category.energy
            ("dict.accounting.cost.element.category.energy", "en-US", "能源费_us", "成本要素类别.能源费"),
            // dict.accounting.cost.element.category.energy
            ("dict.accounting.cost.element.category.energy", "ja-JP", "能源费_jp", "成本要素类别.能源费"),
            // dict.accounting.cost.element.category.energy
            ("dict.accounting.cost.element.category.energy", "zh-CN", "能源费", "成本要素类别.能源费"),
            // dict.accounting.cost.element.category.energy
            ("dict.accounting.cost.element.category.energy", "zh-HK", "能源费_hk", "成本要素类别.能源费"),

            // dict.accounting.cost.element.category.maintenance
            ("dict.accounting.cost.element.category.maintenance", "en-US", "维修费_us", "成本要素类别.维修费"),
            // dict.accounting.cost.element.category.maintenance
            ("dict.accounting.cost.element.category.maintenance", "ja-JP", "维修费_jp", "成本要素类别.维修费"),
            // dict.accounting.cost.element.category.maintenance
            ("dict.accounting.cost.element.category.maintenance", "zh-CN", "维修费", "成本要素类别.维修费"),
            // dict.accounting.cost.element.category.maintenance
            ("dict.accounting.cost.element.category.maintenance", "zh-HK", "维修费_hk", "成本要素类别.维修费"),

            // dict.accounting.cost.element.category.indirect_material
            ("dict.accounting.cost.element.category.indirect_material", "en-US", "辅助材料_us", "成本要素类别.辅助材料"),
            // dict.accounting.cost.element.category.indirect_material
            ("dict.accounting.cost.element.category.indirect_material", "ja-JP", "辅助材料_jp", "成本要素类别.辅助材料"),
            // dict.accounting.cost.element.category.indirect_material
            ("dict.accounting.cost.element.category.indirect_material", "zh-CN", "辅助材料", "成本要素类别.辅助材料"),
            // dict.accounting.cost.element.category.indirect_material
            ("dict.accounting.cost.element.category.indirect_material", "zh-HK", "辅助材料_hk", "成本要素类别.辅助材料"),

            // dict.accounting.cost.element.category.other
            ("dict.accounting.cost.element.category.other", "en-US", "其他费用_us", "成本要素类别.其他费用"),
            // dict.accounting.cost.element.category.other
            ("dict.accounting.cost.element.category.other", "ja-JP", "其他费用_jp", "成本要素类别.其他费用"),
            // dict.accounting.cost.element.category.other
            ("dict.accounting.cost.element.category.other", "zh-CN", "其他费用", "成本要素类别.其他费用"),
            // dict.accounting.cost.element.category.other
            ("dict.accounting.cost.element.category.other", "zh-HK", "其他费用_hk", "成本要素类别.其他费用"),

            // dict.accounting.currency.code.cny
            ("dict.accounting.currency.code.cny", "en-US", "人民币_us", "币种.人民币"),
            // dict.accounting.currency.code.cny
            ("dict.accounting.currency.code.cny", "ja-JP", "人民币_jp", "币种.人民币"),
            // dict.accounting.currency.code.cny
            ("dict.accounting.currency.code.cny", "zh-CN", "人民币", "币种.人民币"),
            // dict.accounting.currency.code.cny
            ("dict.accounting.currency.code.cny", "zh-HK", "人民币_hk", "币种.人民币"),

            // dict.accounting.currency.code.usd
            ("dict.accounting.currency.code.usd", "en-US", "美元_us", "币种.美元"),
            // dict.accounting.currency.code.usd
            ("dict.accounting.currency.code.usd", "ja-JP", "美元_jp", "币种.美元"),
            // dict.accounting.currency.code.usd
            ("dict.accounting.currency.code.usd", "zh-CN", "美元", "币种.美元"),
            // dict.accounting.currency.code.usd
            ("dict.accounting.currency.code.usd", "zh-HK", "美元_hk", "币种.美元"),

            // dict.accounting.currency.code.eur
            ("dict.accounting.currency.code.eur", "en-US", "欧元_us", "币种.欧元"),
            // dict.accounting.currency.code.eur
            ("dict.accounting.currency.code.eur", "ja-JP", "欧元_jp", "币种.欧元"),
            // dict.accounting.currency.code.eur
            ("dict.accounting.currency.code.eur", "zh-CN", "欧元", "币种.欧元"),
            // dict.accounting.currency.code.eur
            ("dict.accounting.currency.code.eur", "zh-HK", "欧元_hk", "币种.欧元"),

            // dict.accounting.currency.code.jpy
            ("dict.accounting.currency.code.jpy", "en-US", "日元_us", "币种.日元"),
            // dict.accounting.currency.code.jpy
            ("dict.accounting.currency.code.jpy", "ja-JP", "日元_jp", "币种.日元"),
            // dict.accounting.currency.code.jpy
            ("dict.accounting.currency.code.jpy", "zh-CN", "日元", "币种.日元"),
            // dict.accounting.currency.code.jpy
            ("dict.accounting.currency.code.jpy", "zh-HK", "日元_hk", "币种.日元"),

            // dict.accounting.currency.code.gbp
            ("dict.accounting.currency.code.gbp", "en-US", "英镑_us", "币种.英镑"),
            // dict.accounting.currency.code.gbp
            ("dict.accounting.currency.code.gbp", "ja-JP", "英镑_jp", "币种.英镑"),
            // dict.accounting.currency.code.gbp
            ("dict.accounting.currency.code.gbp", "zh-CN", "英镑", "币种.英镑"),
            // dict.accounting.currency.code.gbp
            ("dict.accounting.currency.code.gbp", "zh-HK", "英镑_hk", "币种.英镑"),

            // dict.accounting.currency.code.hkd
            ("dict.accounting.currency.code.hkd", "en-US", "港币_us", "币种.港币"),
            // dict.accounting.currency.code.hkd
            ("dict.accounting.currency.code.hkd", "ja-JP", "港币_jp", "币种.港币"),
            // dict.accounting.currency.code.hkd
            ("dict.accounting.currency.code.hkd", "zh-CN", "港币", "币种.港币"),
            // dict.accounting.currency.code.hkd
            ("dict.accounting.currency.code.hkd", "zh-HK", "港币_hk", "币种.港币"),

            // dict.accounting.currency.code.krw
            ("dict.accounting.currency.code.krw", "en-US", "韩元_us", "币种.韩元"),
            // dict.accounting.currency.code.krw
            ("dict.accounting.currency.code.krw", "ja-JP", "韩元_jp", "币种.韩元"),
            // dict.accounting.currency.code.krw
            ("dict.accounting.currency.code.krw", "zh-CN", "韩元", "币种.韩元"),
            // dict.accounting.currency.code.krw
            ("dict.accounting.currency.code.krw", "zh-HK", "韩元_hk", "币种.韩元"),

            // dict.accounting.currency.code.aud
            ("dict.accounting.currency.code.aud", "en-US", "澳元_us", "币种.澳元"),
            // dict.accounting.currency.code.aud
            ("dict.accounting.currency.code.aud", "ja-JP", "澳元_jp", "币种.澳元"),
            // dict.accounting.currency.code.aud
            ("dict.accounting.currency.code.aud", "zh-CN", "澳元", "币种.澳元"),
            // dict.accounting.currency.code.aud
            ("dict.accounting.currency.code.aud", "zh-HK", "澳元_hk", "币种.澳元"),

            // dict.accounting.currency.code.cad
            ("dict.accounting.currency.code.cad", "en-US", "加元_us", "币种.加元"),
            // dict.accounting.currency.code.cad
            ("dict.accounting.currency.code.cad", "ja-JP", "加元_jp", "币种.加元"),
            // dict.accounting.currency.code.cad
            ("dict.accounting.currency.code.cad", "zh-CN", "加元", "币种.加元"),
            // dict.accounting.currency.code.cad
            ("dict.accounting.currency.code.cad", "zh-HK", "加元_hk", "币种.加元"),

            // dict.accounting.currency.code.chf
            ("dict.accounting.currency.code.chf", "en-US", "瑞士法郎_us", "币种.瑞士法郎"),
            // dict.accounting.currency.code.chf
            ("dict.accounting.currency.code.chf", "ja-JP", "瑞士法郎_jp", "币种.瑞士法郎"),
            // dict.accounting.currency.code.chf
            ("dict.accounting.currency.code.chf", "zh-CN", "瑞士法郎", "币种.瑞士法郎"),
            // dict.accounting.currency.code.chf
            ("dict.accounting.currency.code.chf", "zh-HK", "瑞士法郎_hk", "币种.瑞士法郎"),

            // dict.accounting.payment.terms.param.adv100
            ("dict.accounting.payment.terms.param.adv100", "en-US", "预付全款_us", "付款条件.预付全款"),
            // dict.accounting.payment.terms.param.adv100
            ("dict.accounting.payment.terms.param.adv100", "ja-JP", "预付全款_jp", "付款条件.预付全款"),
            // dict.accounting.payment.terms.param.adv100
            ("dict.accounting.payment.terms.param.adv100", "zh-CN", "预付全款", "付款条件.预付全款"),
            // dict.accounting.payment.terms.param.adv100
            ("dict.accounting.payment.terms.param.adv100", "zh-HK", "预付全款_hk", "付款条件.预付全款"),

            // dict.accounting.payment.terms.param.adv50
            ("dict.accounting.payment.terms.param.adv50", "en-US", "预付50%_us", "付款条件.预付50%"),
            // dict.accounting.payment.terms.param.adv50
            ("dict.accounting.payment.terms.param.adv50", "ja-JP", "预付50%_jp", "付款条件.预付50%"),
            // dict.accounting.payment.terms.param.adv50
            ("dict.accounting.payment.terms.param.adv50", "zh-CN", "预付50%", "付款条件.预付50%"),
            // dict.accounting.payment.terms.param.adv50
            ("dict.accounting.payment.terms.param.adv50", "zh-HK", "预付50%_hk", "付款条件.预付50%"),

            // dict.accounting.payment.terms.param.cod
            ("dict.accounting.payment.terms.param.cod", "en-US", "货到付款_us", "付款条件.货到付款"),
            // dict.accounting.payment.terms.param.cod
            ("dict.accounting.payment.terms.param.cod", "ja-JP", "货到付款_jp", "付款条件.货到付款"),
            // dict.accounting.payment.terms.param.cod
            ("dict.accounting.payment.terms.param.cod", "zh-CN", "货到付款", "付款条件.货到付款"),
            // dict.accounting.payment.terms.param.cod
            ("dict.accounting.payment.terms.param.cod", "zh-HK", "货到付款_hk", "付款条件.货到付款"),

            // dict.accounting.payment.terms.param.net30
            ("dict.accounting.payment.terms.param.net30", "en-US", "月结30天_us", "付款条件.月结30天"),
            // dict.accounting.payment.terms.param.net30
            ("dict.accounting.payment.terms.param.net30", "ja-JP", "月结30天_jp", "付款条件.月结30天"),
            // dict.accounting.payment.terms.param.net30
            ("dict.accounting.payment.terms.param.net30", "zh-CN", "月结30天", "付款条件.月结30天"),
            // dict.accounting.payment.terms.param.net30
            ("dict.accounting.payment.terms.param.net30", "zh-HK", "月结30天_hk", "付款条件.月结30天"),

            // dict.accounting.payment.terms.param.net60
            ("dict.accounting.payment.terms.param.net60", "en-US", "月结60天_us", "付款条件.月结60天"),
            // dict.accounting.payment.terms.param.net60
            ("dict.accounting.payment.terms.param.net60", "ja-JP", "月结60天_jp", "付款条件.月结60天"),
            // dict.accounting.payment.terms.param.net60
            ("dict.accounting.payment.terms.param.net60", "zh-CN", "月结60天", "付款条件.月结60天"),
            // dict.accounting.payment.terms.param.net60
            ("dict.accounting.payment.terms.param.net60", "zh-HK", "月结60天_hk", "付款条件.月结60天"),

            // dict.accounting.payment.terms.param.net90
            ("dict.accounting.payment.terms.param.net90", "en-US", "月结90天_us", "付款条件.月结90天"),
            // dict.accounting.payment.terms.param.net90
            ("dict.accounting.payment.terms.param.net90", "ja-JP", "月结90天_jp", "付款条件.月结90天"),
            // dict.accounting.payment.terms.param.net90
            ("dict.accounting.payment.terms.param.net90", "zh-CN", "月结90天", "付款条件.月结90天"),
            // dict.accounting.payment.terms.param.net90
            ("dict.accounting.payment.terms.param.net90", "zh-HK", "月结90天_hk", "付款条件.月结90天"),

            // dict.accounting.payment.terms.param.sight
            ("dict.accounting.payment.terms.param.sight", "en-US", "见票即付_us", "付款条件.见票即付"),
            // dict.accounting.payment.terms.param.sight
            ("dict.accounting.payment.terms.param.sight", "ja-JP", "见票即付_jp", "付款条件.见票即付"),
            // dict.accounting.payment.terms.param.sight
            ("dict.accounting.payment.terms.param.sight", "zh-CN", "见票即付", "付款条件.见票即付"),
            // dict.accounting.payment.terms.param.sight
            ("dict.accounting.payment.terms.param.sight", "zh-HK", "见票即付_hk", "付款条件.见票即付"),

            // dict.accounting.payment.terms.param.tt
            ("dict.accounting.payment.terms.param.tt", "en-US", "电汇_us", "付款条件.电汇"),
            // dict.accounting.payment.terms.param.tt
            ("dict.accounting.payment.terms.param.tt", "ja-JP", "电汇_jp", "付款条件.电汇"),
            // dict.accounting.payment.terms.param.tt
            ("dict.accounting.payment.terms.param.tt", "zh-CN", "电汇", "付款条件.电汇"),
            // dict.accounting.payment.terms.param.tt
            ("dict.accounting.payment.terms.param.tt", "zh-HK", "电汇_hk", "付款条件.电汇"),

            // dict.accounting.payment.terms.param.lc
            ("dict.accounting.payment.terms.param.lc", "en-US", "信用证_us", "付款条件.信用证"),
            // dict.accounting.payment.terms.param.lc
            ("dict.accounting.payment.terms.param.lc", "ja-JP", "信用证_jp", "付款条件.信用证"),
            // dict.accounting.payment.terms.param.lc
            ("dict.accounting.payment.terms.param.lc", "zh-CN", "信用证", "付款条件.信用证"),
            // dict.accounting.payment.terms.param.lc
            ("dict.accounting.payment.terms.param.lc", "zh-HK", "信用证_hk", "付款条件.信用证"),

            // dict.accounting.profit.center.category.pro
            ("dict.accounting.profit.center.category.pro", "en-US", "专业级_us", "利润中心类别.专业级"),
            // dict.accounting.profit.center.category.pro
            ("dict.accounting.profit.center.category.pro", "ja-JP", "专业级_jp", "利润中心类别.专业级"),
            // dict.accounting.profit.center.category.pro
            ("dict.accounting.profit.center.category.pro", "zh-CN", "专业级", "利润中心类别.专业级"),
            // dict.accounting.profit.center.category.pro
            ("dict.accounting.profit.center.category.pro", "zh-HK", "专业级_hk", "利润中心类别.专业级"),

            // dict.accounting.profit.center.category.cons
            ("dict.accounting.profit.center.category.cons", "en-US", "消费级_us", "利润中心类别.消费级"),
            // dict.accounting.profit.center.category.cons
            ("dict.accounting.profit.center.category.cons", "ja-JP", "消费级_jp", "利润中心类别.消费级"),
            // dict.accounting.profit.center.category.cons
            ("dict.accounting.profit.center.category.cons", "zh-CN", "消费级", "利润中心类别.消费级"),
            // dict.accounting.profit.center.category.cons
            ("dict.accounting.profit.center.category.cons", "zh-HK", "消费级_hk", "利润中心类别.消费级"),

            // dict.accounting.profit.center.category.medi
            ("dict.accounting.profit.center.category.medi", "en-US", "医用级_us", "利润中心类别.医用级"),
            // dict.accounting.profit.center.category.medi
            ("dict.accounting.profit.center.category.medi", "ja-JP", "医用级_jp", "利润中心类别.医用级"),
            // dict.accounting.profit.center.category.medi
            ("dict.accounting.profit.center.category.medi", "zh-CN", "医用级", "利润中心类别.医用级"),
            // dict.accounting.profit.center.category.medi
            ("dict.accounting.profit.center.category.medi", "zh-HK", "医用级_hk", "利润中心类别.医用级"),

            // dict.accounting.profit.center.category.info
            ("dict.accounting.profit.center.category.info", "en-US", "信息类_us", "利润中心类别.信息类"),
            // dict.accounting.profit.center.category.info
            ("dict.accounting.profit.center.category.info", "ja-JP", "信息类_jp", "利润中心类别.信息类"),
            // dict.accounting.profit.center.category.info
            ("dict.accounting.profit.center.category.info", "zh-CN", "信息类", "利润中心类别.信息类"),
            // dict.accounting.profit.center.category.info
            ("dict.accounting.profit.center.category.info", "zh-HK", "信息类_hk", "利润中心类别.信息类"),

            // dict.accounting.profit.center.category.ems
            ("dict.accounting.profit.center.category.ems", "en-US", "ems_us", "利润中心类别.ems"),
            // dict.accounting.profit.center.category.ems
            ("dict.accounting.profit.center.category.ems", "ja-JP", "ems_jp", "利润中心类别.ems"),
            // dict.accounting.profit.center.category.ems
            ("dict.accounting.profit.center.category.ems", "zh-CN", "ems", "利润中心类别.ems"),
            // dict.accounting.profit.center.category.ems
            ("dict.accounting.profit.center.category.ems", "zh-HK", "ems_hk", "利润中心类别.ems"),

            // dict.accounting.tax.code.vat13
            ("dict.accounting.tax.code.vat13", "en-US", "增值税13%_us", "税码.增值税13%"),
            // dict.accounting.tax.code.vat13
            ("dict.accounting.tax.code.vat13", "ja-JP", "增值税13%_jp", "税码.增值税13%"),
            // dict.accounting.tax.code.vat13
            ("dict.accounting.tax.code.vat13", "zh-CN", "增值税13%", "税码.增值税13%"),
            // dict.accounting.tax.code.vat13
            ("dict.accounting.tax.code.vat13", "zh-HK", "增值税13%_hk", "税码.增值税13%"),

            // dict.accounting.tax.code.vat9
            ("dict.accounting.tax.code.vat9", "en-US", "增值税9%_us", "税码.增值税9%"),
            // dict.accounting.tax.code.vat9
            ("dict.accounting.tax.code.vat9", "ja-JP", "增值税9%_jp", "税码.增值税9%"),
            // dict.accounting.tax.code.vat9
            ("dict.accounting.tax.code.vat9", "zh-CN", "增值税9%", "税码.增值税9%"),
            // dict.accounting.tax.code.vat9
            ("dict.accounting.tax.code.vat9", "zh-HK", "增值税9%_hk", "税码.增值税9%"),

            // dict.accounting.tax.code.vat6
            ("dict.accounting.tax.code.vat6", "en-US", "增值税6%_us", "税码.增值税6%"),
            // dict.accounting.tax.code.vat6
            ("dict.accounting.tax.code.vat6", "ja-JP", "增值税6%_jp", "税码.增值税6%"),
            // dict.accounting.tax.code.vat6
            ("dict.accounting.tax.code.vat6", "zh-CN", "增值税6%", "税码.增值税6%"),
            // dict.accounting.tax.code.vat6
            ("dict.accounting.tax.code.vat6", "zh-HK", "增值税6%_hk", "税码.增值税6%"),

            // dict.accounting.tax.code.vat3
            ("dict.accounting.tax.code.vat3", "en-US", "增值税3%_us", "税码.增值税3%"),
            // dict.accounting.tax.code.vat3
            ("dict.accounting.tax.code.vat3", "ja-JP", "增值税3%_jp", "税码.增值税3%"),
            // dict.accounting.tax.code.vat3
            ("dict.accounting.tax.code.vat3", "zh-CN", "增值税3%", "税码.增值税3%"),
            // dict.accounting.tax.code.vat3
            ("dict.accounting.tax.code.vat3", "zh-HK", "增值税3%_hk", "税码.增值税3%"),

            // dict.accounting.tax.code.vat0
            ("dict.accounting.tax.code.vat0", "en-US", "增值税0%_us", "税码.增值税0%"),
            // dict.accounting.tax.code.vat0
            ("dict.accounting.tax.code.vat0", "ja-JP", "增值税0%_jp", "税码.增值税0%"),
            // dict.accounting.tax.code.vat0
            ("dict.accounting.tax.code.vat0", "zh-CN", "增值税0%", "税码.增值税0%"),
            // dict.accounting.tax.code.vat0
            ("dict.accounting.tax.code.vat0", "zh-HK", "增值税0%_hk", "税码.增值税0%"),

            // dict.accounting.tax.code.taxfree
            ("dict.accounting.tax.code.taxfree", "en-US", "免税_us", "税码.免税"),
            // dict.accounting.tax.code.taxfree
            ("dict.accounting.tax.code.taxfree", "ja-JP", "免税_jp", "税码.免税"),
            // dict.accounting.tax.code.taxfree
            ("dict.accounting.tax.code.taxfree", "zh-CN", "免税", "税码.免税"),
            // dict.accounting.tax.code.taxfree
            ("dict.accounting.tax.code.taxfree", "zh-HK", "免税_hk", "税码.免税"),

            // dict.accounting.tax.code.input
            ("dict.accounting.tax.code.input", "en-US", "进项税_us", "税码.进项税"),
            // dict.accounting.tax.code.input
            ("dict.accounting.tax.code.input", "ja-JP", "进项税_jp", "税码.进项税"),
            // dict.accounting.tax.code.input
            ("dict.accounting.tax.code.input", "zh-CN", "进项税", "税码.进项税"),
            // dict.accounting.tax.code.input
            ("dict.accounting.tax.code.input", "zh-HK", "进项税_hk", "税码.进项税"),

            // dict.accounting.tax.code.output
            ("dict.accounting.tax.code.output", "en-US", "销项税_us", "税码.销项税"),
            // dict.accounting.tax.code.output
            ("dict.accounting.tax.code.output", "ja-JP", "销项税_jp", "税码.销项税"),
            // dict.accounting.tax.code.output
            ("dict.accounting.tax.code.output", "zh-CN", "销项税", "税码.销项税"),
            // dict.accounting.tax.code.output
            ("dict.accounting.tax.code.output", "zh-HK", "销项税_hk", "税码.销项税"),

            // dict.accounting.tax.rate.param.13
            ("dict.accounting.tax.rate.param.13", "en-US", "13%_us", "税率.13%"),
            // dict.accounting.tax.rate.param.13
            ("dict.accounting.tax.rate.param.13", "ja-JP", "13%_jp", "税率.13%"),
            // dict.accounting.tax.rate.param.13
            ("dict.accounting.tax.rate.param.13", "zh-CN", "13%", "税率.13%"),
            // dict.accounting.tax.rate.param.13
            ("dict.accounting.tax.rate.param.13", "zh-HK", "13%_hk", "税率.13%"),

            // dict.accounting.tax.rate.param.9
            ("dict.accounting.tax.rate.param.9", "en-US", "9%_us", "税率.9%"),
            // dict.accounting.tax.rate.param.9
            ("dict.accounting.tax.rate.param.9", "ja-JP", "9%_jp", "税率.9%"),
            // dict.accounting.tax.rate.param.9
            ("dict.accounting.tax.rate.param.9", "zh-CN", "9%", "税率.9%"),
            // dict.accounting.tax.rate.param.9
            ("dict.accounting.tax.rate.param.9", "zh-HK", "9%_hk", "税率.9%"),

            // dict.accounting.tax.rate.param.6
            ("dict.accounting.tax.rate.param.6", "en-US", "6%_us", "税率.6%"),
            // dict.accounting.tax.rate.param.6
            ("dict.accounting.tax.rate.param.6", "ja-JP", "6%_jp", "税率.6%"),
            // dict.accounting.tax.rate.param.6
            ("dict.accounting.tax.rate.param.6", "zh-CN", "6%", "税率.6%"),
            // dict.accounting.tax.rate.param.6
            ("dict.accounting.tax.rate.param.6", "zh-HK", "6%_hk", "税率.6%"),

            // dict.accounting.tax.rate.param.5
            ("dict.accounting.tax.rate.param.5", "en-US", "5%_us", "税率.5%"),
            // dict.accounting.tax.rate.param.5
            ("dict.accounting.tax.rate.param.5", "ja-JP", "5%_jp", "税率.5%"),
            // dict.accounting.tax.rate.param.5
            ("dict.accounting.tax.rate.param.5", "zh-CN", "5%", "税率.5%"),
            // dict.accounting.tax.rate.param.5
            ("dict.accounting.tax.rate.param.5", "zh-HK", "5%_hk", "税率.5%"),

            // dict.accounting.tax.rate.param.3
            ("dict.accounting.tax.rate.param.3", "en-US", "3%_us", "税率.3%"),
            // dict.accounting.tax.rate.param.3
            ("dict.accounting.tax.rate.param.3", "ja-JP", "3%_jp", "税率.3%"),
            // dict.accounting.tax.rate.param.3
            ("dict.accounting.tax.rate.param.3", "zh-CN", "3%", "税率.3%"),
            // dict.accounting.tax.rate.param.3
            ("dict.accounting.tax.rate.param.3", "zh-HK", "3%_hk", "税率.3%"),

            // dict.accounting.tax.rate.param.2
            ("dict.accounting.tax.rate.param.2", "en-US", "2%_us", "税率.2%"),
            // dict.accounting.tax.rate.param.2
            ("dict.accounting.tax.rate.param.2", "ja-JP", "2%_jp", "税率.2%"),
            // dict.accounting.tax.rate.param.2
            ("dict.accounting.tax.rate.param.2", "zh-CN", "2%", "税率.2%"),
            // dict.accounting.tax.rate.param.2
            ("dict.accounting.tax.rate.param.2", "zh-HK", "2%_hk", "税率.2%"),

            // dict.accounting.tax.rate.param.1
            ("dict.accounting.tax.rate.param.1", "en-US", "1%_us", "税率.1%"),
            // dict.accounting.tax.rate.param.1
            ("dict.accounting.tax.rate.param.1", "ja-JP", "1%_jp", "税率.1%"),
            // dict.accounting.tax.rate.param.1
            ("dict.accounting.tax.rate.param.1", "zh-CN", "1%", "税率.1%"),
            // dict.accounting.tax.rate.param.1
            ("dict.accounting.tax.rate.param.1", "zh-HK", "1%_hk", "税率.1%"),

            // dict.accounting.tax.rate.param.0
            ("dict.accounting.tax.rate.param.0", "en-US", "0%_us", "税率.0%"),
            // dict.accounting.tax.rate.param.0
            ("dict.accounting.tax.rate.param.0", "ja-JP", "0%_jp", "税率.0%"),
            // dict.accounting.tax.rate.param.0
            ("dict.accounting.tax.rate.param.0", "zh-CN", "0%", "税率.0%"),
            // dict.accounting.tax.rate.param.0
            ("dict.accounting.tax.rate.param.0", "zh-HK", "0%_hk", "税率.0%"),

            // dict.gen.button.category.query
            ("dict.gen.button.category.query", "en-US", "查询_us", "代码生成操作后缀.查询"),
            // dict.gen.button.category.query
            ("dict.gen.button.category.query", "ja-JP", "查询_jp", "代码生成操作后缀.查询"),
            // dict.gen.button.category.query
            ("dict.gen.button.category.query", "zh-CN", "查询", "代码生成操作后缀.查询"),
            // dict.gen.button.category.query
            ("dict.gen.button.category.query", "zh-HK", "查询_hk", "代码生成操作后缀.查询"),

            // dict.gen.button.category.create
            ("dict.gen.button.category.create", "en-US", "新增_us", "代码生成操作后缀.新增"),
            // dict.gen.button.category.create
            ("dict.gen.button.category.create", "ja-JP", "新增_jp", "代码生成操作后缀.新增"),
            // dict.gen.button.category.create
            ("dict.gen.button.category.create", "zh-CN", "新增", "代码生成操作后缀.新增"),
            // dict.gen.button.category.create
            ("dict.gen.button.category.create", "zh-HK", "新增_hk", "代码生成操作后缀.新增"),

            // dict.gen.button.category.update
            ("dict.gen.button.category.update", "en-US", "修改_us", "代码生成操作后缀.修改"),
            // dict.gen.button.category.update
            ("dict.gen.button.category.update", "ja-JP", "修改_jp", "代码生成操作后缀.修改"),
            // dict.gen.button.category.update
            ("dict.gen.button.category.update", "zh-CN", "修改", "代码生成操作后缀.修改"),
            // dict.gen.button.category.update
            ("dict.gen.button.category.update", "zh-HK", "修改_hk", "代码生成操作后缀.修改"),

            // dict.gen.button.category.delete
            ("dict.gen.button.category.delete", "en-US", "删除_us", "代码生成操作后缀.删除"),
            // dict.gen.button.category.delete
            ("dict.gen.button.category.delete", "ja-JP", "删除_jp", "代码生成操作后缀.删除"),
            // dict.gen.button.category.delete
            ("dict.gen.button.category.delete", "zh-CN", "删除", "代码生成操作后缀.删除"),
            // dict.gen.button.category.delete
            ("dict.gen.button.category.delete", "zh-HK", "删除_hk", "代码生成操作后缀.删除"),

            // dict.gen.button.category.detail
            ("dict.gen.button.category.detail", "en-US", "详情_us", "代码生成操作后缀.详情"),
            // dict.gen.button.category.detail
            ("dict.gen.button.category.detail", "ja-JP", "详情_jp", "代码生成操作后缀.详情"),
            // dict.gen.button.category.detail
            ("dict.gen.button.category.detail", "zh-CN", "详情", "代码生成操作后缀.详情"),
            // dict.gen.button.category.detail
            ("dict.gen.button.category.detail", "zh-HK", "详情_hk", "代码生成操作后缀.详情"),

            // dict.gen.button.category.preview
            ("dict.gen.button.category.preview", "en-US", "预览_us", "代码生成操作后缀.预览"),
            // dict.gen.button.category.preview
            ("dict.gen.button.category.preview", "ja-JP", "预览_jp", "代码生成操作后缀.预览"),
            // dict.gen.button.category.preview
            ("dict.gen.button.category.preview", "zh-CN", "预览", "代码生成操作后缀.预览"),
            // dict.gen.button.category.preview
            ("dict.gen.button.category.preview", "zh-HK", "预览_hk", "代码生成操作后缀.预览"),

            // dict.gen.button.category.print
            ("dict.gen.button.category.print", "en-US", "打印_us", "代码生成操作后缀.打印"),
            // dict.gen.button.category.print
            ("dict.gen.button.category.print", "ja-JP", "打印_jp", "代码生成操作后缀.打印"),
            // dict.gen.button.category.print
            ("dict.gen.button.category.print", "zh-CN", "打印", "代码生成操作后缀.打印"),
            // dict.gen.button.category.print
            ("dict.gen.button.category.print", "zh-HK", "打印_hk", "代码生成操作后缀.打印"),

            // dict.gen.button.category.import
            ("dict.gen.button.category.import", "en-US", "导入_us", "代码生成操作后缀.导入"),
            // dict.gen.button.category.import
            ("dict.gen.button.category.import", "ja-JP", "导入_jp", "代码生成操作后缀.导入"),
            // dict.gen.button.category.import
            ("dict.gen.button.category.import", "zh-CN", "导入", "代码生成操作后缀.导入"),
            // dict.gen.button.category.import
            ("dict.gen.button.category.import", "zh-HK", "导入_hk", "代码生成操作后缀.导入"),

            // dict.gen.button.category.export
            ("dict.gen.button.category.export", "en-US", "导出_us", "代码生成操作后缀.导出"),
            // dict.gen.button.category.export
            ("dict.gen.button.category.export", "ja-JP", "导出_jp", "代码生成操作后缀.导出"),
            // dict.gen.button.category.export
            ("dict.gen.button.category.export", "zh-CN", "导出", "代码生成操作后缀.导出"),
            // dict.gen.button.category.export
            ("dict.gen.button.category.export", "zh-HK", "导出_hk", "代码生成操作后缀.导出"),

            // dict.gen.button.category.template
            ("dict.gen.button.category.template", "en-US", "模板_us", "代码生成操作后缀.模板"),
            // dict.gen.button.category.template
            ("dict.gen.button.category.template", "ja-JP", "模板_jp", "代码生成操作后缀.模板"),
            // dict.gen.button.category.template
            ("dict.gen.button.category.template", "zh-CN", "模板", "代码生成操作后缀.模板"),
            // dict.gen.button.category.template
            ("dict.gen.button.category.template", "zh-HK", "模板_hk", "代码生成操作后缀.模板"),

            // dict.gen.button.category.approve
            ("dict.gen.button.category.approve", "en-US", "审批_us", "代码生成操作后缀.审批"),
            // dict.gen.button.category.approve
            ("dict.gen.button.category.approve", "ja-JP", "审批_jp", "代码生成操作后缀.审批"),
            // dict.gen.button.category.approve
            ("dict.gen.button.category.approve", "zh-CN", "审批", "代码生成操作后缀.审批"),
            // dict.gen.button.category.approve
            ("dict.gen.button.category.approve", "zh-HK", "审批_hk", "代码生成操作后缀.审批"),

            // dict.gen.button.category.revoke
            ("dict.gen.button.category.revoke", "en-US", "撤销_us", "代码生成操作后缀.撤销"),
            // dict.gen.button.category.revoke
            ("dict.gen.button.category.revoke", "ja-JP", "撤销_jp", "代码生成操作后缀.撤销"),
            // dict.gen.button.category.revoke
            ("dict.gen.button.category.revoke", "zh-CN", "撤销", "代码生成操作后缀.撤销"),
            // dict.gen.button.category.revoke
            ("dict.gen.button.category.revoke", "zh-HK", "撤销_hk", "代码生成操作后缀.撤销"),

            // dict.gen.button.category.authorize
            ("dict.gen.button.category.authorize", "en-US", "授权_us", "代码生成操作后缀.授权"),
            // dict.gen.button.category.authorize
            ("dict.gen.button.category.authorize", "ja-JP", "授权_jp", "代码生成操作后缀.授权"),
            // dict.gen.button.category.authorize
            ("dict.gen.button.category.authorize", "zh-CN", "授权", "代码生成操作后缀.授权"),
            // dict.gen.button.category.authorize
            ("dict.gen.button.category.authorize", "zh-HK", "授权_hk", "代码生成操作后缀.授权"),

            // dict.gen.button.category.allocate
            ("dict.gen.button.category.allocate", "en-US", "分配_us", "代码生成操作后缀.分配"),
            // dict.gen.button.category.allocate
            ("dict.gen.button.category.allocate", "ja-JP", "分配_jp", "代码生成操作后缀.分配"),
            // dict.gen.button.category.allocate
            ("dict.gen.button.category.allocate", "zh-CN", "分配", "代码生成操作后缀.分配"),
            // dict.gen.button.category.allocate
            ("dict.gen.button.category.allocate", "zh-HK", "分配_hk", "代码生成操作后缀.分配"),

            // dict.gen.button.category.resetpwd
            ("dict.gen.button.category.resetpwd", "en-US", "重置密码_us", "代码生成操作后缀.重置密码"),
            // dict.gen.button.category.resetpwd
            ("dict.gen.button.category.resetpwd", "ja-JP", "重置密码_jp", "代码生成操作后缀.重置密码"),
            // dict.gen.button.category.resetpwd
            ("dict.gen.button.category.resetpwd", "zh-CN", "重置密码", "代码生成操作后缀.重置密码"),
            // dict.gen.button.category.resetpwd
            ("dict.gen.button.category.resetpwd", "zh-HK", "重置密码_hk", "代码生成操作后缀.重置密码"),

            // dict.gen.button.category.changepwd
            ("dict.gen.button.category.changepwd", "en-US", "变更密码_us", "代码生成操作后缀.变更密码"),
            // dict.gen.button.category.changepwd
            ("dict.gen.button.category.changepwd", "ja-JP", "变更密码_jp", "代码生成操作后缀.变更密码"),
            // dict.gen.button.category.changepwd
            ("dict.gen.button.category.changepwd", "zh-CN", "变更密码", "代码生成操作后缀.变更密码"),
            // dict.gen.button.category.changepwd
            ("dict.gen.button.category.changepwd", "zh-HK", "变更密码_hk", "代码生成操作后缀.变更密码"),

            // dict.gen.button.category.empty
            ("dict.gen.button.category.empty", "en-US", "清空_us", "代码生成操作后缀.清空"),
            // dict.gen.button.category.empty
            ("dict.gen.button.category.empty", "ja-JP", "清空_jp", "代码生成操作后缀.清空"),
            // dict.gen.button.category.empty
            ("dict.gen.button.category.empty", "zh-CN", "清空", "代码生成操作后缀.清空"),
            // dict.gen.button.category.empty
            ("dict.gen.button.category.empty", "zh-HK", "清空_hk", "代码生成操作后缀.清空"),

            // dict.gen.button.category.truncate
            ("dict.gen.button.category.truncate", "en-US", "截断_us", "代码生成操作后缀.截断"),
            // dict.gen.button.category.truncate
            ("dict.gen.button.category.truncate", "ja-JP", "截断_jp", "代码生成操作后缀.截断"),
            // dict.gen.button.category.truncate
            ("dict.gen.button.category.truncate", "zh-CN", "截断", "代码生成操作后缀.截断"),
            // dict.gen.button.category.truncate
            ("dict.gen.button.category.truncate", "zh-HK", "截断_hk", "代码生成操作后缀.截断"),

            // dict.gen.button.category.unlock
            ("dict.gen.button.category.unlock", "en-US", "解锁_us", "代码生成操作后缀.解锁"),
            // dict.gen.button.category.unlock
            ("dict.gen.button.category.unlock", "ja-JP", "解锁_jp", "代码生成操作后缀.解锁"),
            // dict.gen.button.category.unlock
            ("dict.gen.button.category.unlock", "zh-CN", "解锁", "代码生成操作后缀.解锁"),
            // dict.gen.button.category.unlock
            ("dict.gen.button.category.unlock", "zh-HK", "解锁_hk", "代码生成操作后缀.解锁"),

            // dict.gen.button.category.disable
            ("dict.gen.button.category.disable", "en-US", "禁用_us", "代码生成操作后缀.禁用"),
            // dict.gen.button.category.disable
            ("dict.gen.button.category.disable", "ja-JP", "禁用_jp", "代码生成操作后缀.禁用"),
            // dict.gen.button.category.disable
            ("dict.gen.button.category.disable", "zh-CN", "禁用", "代码生成操作后缀.禁用"),
            // dict.gen.button.category.disable
            ("dict.gen.button.category.disable", "zh-HK", "禁用_hk", "代码生成操作后缀.禁用"),

            // dict.gen.button.category.generate
            ("dict.gen.button.category.generate", "en-US", "生成_us", "代码生成操作后缀.生成"),
            // dict.gen.button.category.generate
            ("dict.gen.button.category.generate", "ja-JP", "生成_jp", "代码生成操作后缀.生成"),
            // dict.gen.button.category.generate
            ("dict.gen.button.category.generate", "zh-CN", "生成", "代码生成操作后缀.生成"),
            // dict.gen.button.category.generate
            ("dict.gen.button.category.generate", "zh-HK", "生成_hk", "代码生成操作后缀.生成"),

            // dict.gen.button.category.download
            ("dict.gen.button.category.download", "en-US", "下载_us", "代码生成操作后缀.下载"),
            // dict.gen.button.category.download
            ("dict.gen.button.category.download", "ja-JP", "下载_jp", "代码生成操作后缀.下载"),
            // dict.gen.button.category.download
            ("dict.gen.button.category.download", "zh-CN", "下载", "代码生成操作后缀.下载"),
            // dict.gen.button.category.download
            ("dict.gen.button.category.download", "zh-HK", "下载_hk", "代码生成操作后缀.下载"),

            // dict.gen.button.category.sync
            ("dict.gen.button.category.sync", "en-US", "同步_us", "代码生成操作后缀.同步"),
            // dict.gen.button.category.sync
            ("dict.gen.button.category.sync", "ja-JP", "同步_jp", "代码生成操作后缀.同步"),
            // dict.gen.button.category.sync
            ("dict.gen.button.category.sync", "zh-CN", "同步", "代码生成操作后缀.同步"),
            // dict.gen.button.category.sync
            ("dict.gen.button.category.sync", "zh-HK", "同步_hk", "代码生成操作后缀.同步"),

            // dict.gen.button.category.columns
            ("dict.gen.button.category.columns", "en-US", "字段_us", "代码生成操作后缀.字段"),
            // dict.gen.button.category.columns
            ("dict.gen.button.category.columns", "ja-JP", "字段_jp", "代码生成操作后缀.字段"),
            // dict.gen.button.category.columns
            ("dict.gen.button.category.columns", "zh-CN", "字段", "代码生成操作后缀.字段"),
            // dict.gen.button.category.columns
            ("dict.gen.button.category.columns", "zh-HK", "字段_hk", "代码生成操作后缀.字段"),

            // dict.gen.button.category.tables
            ("dict.gen.button.category.tables", "en-US", "表_us", "代码生成操作后缀.表"),
            // dict.gen.button.category.tables
            ("dict.gen.button.category.tables", "ja-JP", "表_jp", "代码生成操作后缀.表"),
            // dict.gen.button.category.tables
            ("dict.gen.button.category.tables", "zh-CN", "表", "代码生成操作后缀.表"),
            // dict.gen.button.category.tables
            ("dict.gen.button.category.tables", "zh-HK", "表_hk", "代码生成操作后缀.表"),

            // dict.gen.button.category.databases
            ("dict.gen.button.category.databases", "en-US", "数据库_us", "代码生成操作后缀.数据库"),
            // dict.gen.button.category.databases
            ("dict.gen.button.category.databases", "ja-JP", "数据库_jp", "代码生成操作后缀.数据库"),
            // dict.gen.button.category.databases
            ("dict.gen.button.category.databases", "zh-CN", "数据库", "代码生成操作后缀.数据库"),
            // dict.gen.button.category.databases
            ("dict.gen.button.category.databases", "zh-HK", "数据库_hk", "代码生成操作后缀.数据库"),

            // dict.gen.button.category.initialize
            ("dict.gen.button.category.initialize", "en-US", "初始化_us", "代码生成操作后缀.初始化"),
            // dict.gen.button.category.initialize
            ("dict.gen.button.category.initialize", "ja-JP", "初始化_jp", "代码生成操作后缀.初始化"),
            // dict.gen.button.category.initialize
            ("dict.gen.button.category.initialize", "zh-CN", "初始化", "代码生成操作后缀.初始化"),
            // dict.gen.button.category.initialize
            ("dict.gen.button.category.initialize", "zh-HK", "初始化_hk", "代码生成操作后缀.初始化"),

            // dict.gen.button.category.clone
            ("dict.gen.button.category.clone", "en-US", "克隆_us", "代码生成操作后缀.克隆"),
            // dict.gen.button.category.clone
            ("dict.gen.button.category.clone", "ja-JP", "克隆_jp", "代码生成操作后缀.克隆"),
            // dict.gen.button.category.clone
            ("dict.gen.button.category.clone", "zh-CN", "克隆", "代码生成操作后缀.克隆"),
            // dict.gen.button.category.clone
            ("dict.gen.button.category.clone", "zh-HK", "克隆_hk", "代码生成操作后缀.克隆"),

            // dict.gen.button.category.copy
            ("dict.gen.button.category.copy", "en-US", "复制_us", "代码生成操作后缀.复制"),
            // dict.gen.button.category.copy
            ("dict.gen.button.category.copy", "ja-JP", "复制_jp", "代码生成操作后缀.复制"),
            // dict.gen.button.category.copy
            ("dict.gen.button.category.copy", "zh-CN", "复制", "代码生成操作后缀.复制"),
            // dict.gen.button.category.copy
            ("dict.gen.button.category.copy", "zh-HK", "复制_hk", "代码生成操作后缀.复制"),

            // dict.gen.button.category.suspend
            ("dict.gen.button.category.suspend", "en-US", "暂停_us", "代码生成操作后缀.暂停"),
            // dict.gen.button.category.suspend
            ("dict.gen.button.category.suspend", "ja-JP", "暂停_jp", "代码生成操作后缀.暂停"),
            // dict.gen.button.category.suspend
            ("dict.gen.button.category.suspend", "zh-CN", "暂停", "代码生成操作后缀.暂停"),
            // dict.gen.button.category.suspend
            ("dict.gen.button.category.suspend", "zh-HK", "暂停_hk", "代码生成操作后缀.暂停"),

            // dict.gen.button.category.resume
            ("dict.gen.button.category.resume", "en-US", "恢复_us", "代码生成操作后缀.恢复"),
            // dict.gen.button.category.resume
            ("dict.gen.button.category.resume", "ja-JP", "恢复_jp", "代码生成操作后缀.恢复"),
            // dict.gen.button.category.resume
            ("dict.gen.button.category.resume", "zh-CN", "恢复", "代码生成操作后缀.恢复"),
            // dict.gen.button.category.resume
            ("dict.gen.button.category.resume", "zh-HK", "恢复_hk", "代码生成操作后缀.恢复"),

            // dict.gen.button.category.submit
            ("dict.gen.button.category.submit", "en-US", "提交_us", "代码生成操作后缀.提交"),
            // dict.gen.button.category.submit
            ("dict.gen.button.category.submit", "ja-JP", "提交_jp", "代码生成操作后缀.提交"),
            // dict.gen.button.category.submit
            ("dict.gen.button.category.submit", "zh-CN", "提交", "代码生成操作后缀.提交"),
            // dict.gen.button.category.submit
            ("dict.gen.button.category.submit", "zh-HK", "提交_hk", "代码生成操作后缀.提交"),

            // dict.gen.button.category.withdraw
            ("dict.gen.button.category.withdraw", "en-US", "撤回_us", "代码生成操作后缀.撤回"),
            // dict.gen.button.category.withdraw
            ("dict.gen.button.category.withdraw", "ja-JP", "撤回_jp", "代码生成操作后缀.撤回"),
            // dict.gen.button.category.withdraw
            ("dict.gen.button.category.withdraw", "zh-CN", "撤回", "代码生成操作后缀.撤回"),
            // dict.gen.button.category.withdraw
            ("dict.gen.button.category.withdraw", "zh-HK", "撤回_hk", "代码生成操作后缀.撤回"),

            // dict.gen.button.category.transfer
            ("dict.gen.button.category.transfer", "en-US", "转办_us", "代码生成操作后缀.转办"),
            // dict.gen.button.category.transfer
            ("dict.gen.button.category.transfer", "ja-JP", "转办_jp", "代码生成操作后缀.转办"),
            // dict.gen.button.category.transfer
            ("dict.gen.button.category.transfer", "zh-CN", "转办", "代码生成操作后缀.转办"),
            // dict.gen.button.category.transfer
            ("dict.gen.button.category.transfer", "zh-HK", "转办_hk", "代码生成操作后缀.转办"),

            // dict.gen.button.category.delegate
            ("dict.gen.button.category.delegate", "en-US", "委托_us", "代码生成操作后缀.委托"),
            // dict.gen.button.category.delegate
            ("dict.gen.button.category.delegate", "ja-JP", "委托_jp", "代码生成操作后缀.委托"),
            // dict.gen.button.category.delegate
            ("dict.gen.button.category.delegate", "zh-CN", "委托", "代码生成操作后缀.委托"),
            // dict.gen.button.category.delegate
            ("dict.gen.button.category.delegate", "zh-HK", "委托_hk", "代码生成操作后缀.委托"),

            // dict.gen.button.category.return
            ("dict.gen.button.category.return", "en-US", "退回_us", "代码生成操作后缀.退回"),
            // dict.gen.button.category.return
            ("dict.gen.button.category.return", "ja-JP", "退回_jp", "代码生成操作后缀.退回"),
            // dict.gen.button.category.return
            ("dict.gen.button.category.return", "zh-CN", "退回", "代码生成操作后缀.退回"),
            // dict.gen.button.category.return
            ("dict.gen.button.category.return", "zh-HK", "退回_hk", "代码生成操作后缀.退回"),

            // dict.gen.button.category.urge
            ("dict.gen.button.category.urge", "en-US", "催办_us", "代码生成操作后缀.催办"),
            // dict.gen.button.category.urge
            ("dict.gen.button.category.urge", "ja-JP", "催办_jp", "代码生成操作后缀.催办"),
            // dict.gen.button.category.urge
            ("dict.gen.button.category.urge", "zh-CN", "催办", "代码生成操作后缀.催办"),
            // dict.gen.button.category.urge
            ("dict.gen.button.category.urge", "zh-HK", "催办_hk", "代码生成操作后缀.催办"),

            // dict.gen.button.category.addsign
            ("dict.gen.button.category.addsign", "en-US", "加签_us", "代码生成操作后缀.加签"),
            // dict.gen.button.category.addsign
            ("dict.gen.button.category.addsign", "ja-JP", "加签_jp", "代码生成操作后缀.加签"),
            // dict.gen.button.category.addsign
            ("dict.gen.button.category.addsign", "zh-CN", "加签", "代码生成操作后缀.加签"),
            // dict.gen.button.category.addsign
            ("dict.gen.button.category.addsign", "zh-HK", "加签_hk", "代码生成操作后缀.加签"),

            // dict.gen.button.category.reducesign
            ("dict.gen.button.category.reducesign", "en-US", "减签_us", "代码生成操作后缀.减签"),
            // dict.gen.button.category.reducesign
            ("dict.gen.button.category.reducesign", "ja-JP", "减签_jp", "代码生成操作后缀.减签"),
            // dict.gen.button.category.reducesign
            ("dict.gen.button.category.reducesign", "zh-CN", "减签", "代码生成操作后缀.减签"),
            // dict.gen.button.category.reducesign
            ("dict.gen.button.category.reducesign", "zh-HK", "减签_hk", "代码生成操作后缀.减签"),

            // dict.gen.button.category.progress
            ("dict.gen.button.category.progress", "en-US", "进度_us", "代码生成操作后缀.进度"),
            // dict.gen.button.category.progress
            ("dict.gen.button.category.progress", "ja-JP", "进度_jp", "代码生成操作后缀.进度"),
            // dict.gen.button.category.progress
            ("dict.gen.button.category.progress", "zh-CN", "进度", "代码生成操作后缀.进度"),
            // dict.gen.button.category.progress
            ("dict.gen.button.category.progress", "zh-HK", "进度_hk", "代码生成操作后缀.进度"),

            // dict.gen.button.category.history
            ("dict.gen.button.category.history", "en-US", "历史_us", "代码生成操作后缀.历史"),
            // dict.gen.button.category.history
            ("dict.gen.button.category.history", "ja-JP", "历史_jp", "代码生成操作后缀.历史"),
            // dict.gen.button.category.history
            ("dict.gen.button.category.history", "zh-CN", "历史", "代码生成操作后缀.历史"),
            // dict.gen.button.category.history
            ("dict.gen.button.category.history", "zh-HK", "历史_hk", "代码生成操作后缀.历史"),

            // dict.gen.button.category.publish
            ("dict.gen.button.category.publish", "en-US", "发布_us", "代码生成操作后缀.发布"),
            // dict.gen.button.category.publish
            ("dict.gen.button.category.publish", "ja-JP", "发布_jp", "代码生成操作后缀.发布"),
            // dict.gen.button.category.publish
            ("dict.gen.button.category.publish", "zh-CN", "发布", "代码生成操作后缀.发布"),
            // dict.gen.button.category.publish
            ("dict.gen.button.category.publish", "zh-HK", "发布_hk", "代码生成操作后缀.发布"),

            // dict.gen.button.category.enable
            ("dict.gen.button.category.enable", "en-US", "启用_us", "代码生成操作后缀.启用"),
            // dict.gen.button.category.enable
            ("dict.gen.button.category.enable", "ja-JP", "启用_jp", "代码生成操作后缀.启用"),
            // dict.gen.button.category.enable
            ("dict.gen.button.category.enable", "zh-CN", "启用", "代码生成操作后缀.启用"),
            // dict.gen.button.category.enable
            ("dict.gen.button.category.enable", "zh-HK", "启用_hk", "代码生成操作后缀.启用"),

            // dict.gen.button.category.version
            ("dict.gen.button.category.version", "en-US", "版本_us", "代码生成操作后缀.版本"),
            // dict.gen.button.category.version
            ("dict.gen.button.category.version", "ja-JP", "版本_jp", "代码生成操作后缀.版本"),
            // dict.gen.button.category.version
            ("dict.gen.button.category.version", "zh-CN", "版本", "代码生成操作后缀.版本"),
            // dict.gen.button.category.version
            ("dict.gen.button.category.version", "zh-HK", "版本_hk", "代码生成操作后缀.版本"),

            // dict.gen.button.category.design
            ("dict.gen.button.category.design", "en-US", "设计_us", "代码生成操作后缀.设计"),
            // dict.gen.button.category.design
            ("dict.gen.button.category.design", "ja-JP", "设计_jp", "代码生成操作后缀.设计"),
            // dict.gen.button.category.design
            ("dict.gen.button.category.design", "zh-CN", "设计", "代码生成操作后缀.设计"),
            // dict.gen.button.category.design
            ("dict.gen.button.category.design", "zh-HK", "设计_hk", "代码生成操作后缀.设计"),

            // dict.gen.button.category.config
            ("dict.gen.button.category.config", "en-US", "配置_us", "代码生成操作后缀.配置"),
            // dict.gen.button.category.config
            ("dict.gen.button.category.config", "ja-JP", "配置_jp", "代码生成操作后缀.配置"),
            // dict.gen.button.category.config
            ("dict.gen.button.category.config", "zh-CN", "配置", "代码生成操作后缀.配置"),
            // dict.gen.button.category.config
            ("dict.gen.button.category.config", "zh-HK", "配置_hk", "代码生成操作后缀.配置"),

            // dict.gen.button.category.validate
            ("dict.gen.button.category.validate", "en-US", "验证_us", "代码生成操作后缀.验证"),
            // dict.gen.button.category.validate
            ("dict.gen.button.category.validate", "ja-JP", "验证_jp", "代码生成操作后缀.验证"),
            // dict.gen.button.category.validate
            ("dict.gen.button.category.validate", "zh-CN", "验证", "代码生成操作后缀.验证"),
            // dict.gen.button.category.validate
            ("dict.gen.button.category.validate", "zh-HK", "验证_hk", "代码生成操作后缀.验证"),

            // dict.gen.button.category.start
            ("dict.gen.button.category.start", "en-US", "启动_us", "代码生成操作后缀.启动"),
            // dict.gen.button.category.start
            ("dict.gen.button.category.start", "ja-JP", "启动_jp", "代码生成操作后缀.启动"),
            // dict.gen.button.category.start
            ("dict.gen.button.category.start", "zh-CN", "启动", "代码生成操作后缀.启动"),
            // dict.gen.button.category.start
            ("dict.gen.button.category.start", "zh-HK", "启动_hk", "代码生成操作后缀.启动"),

            // dict.gen.button.category.terminate
            ("dict.gen.button.category.terminate", "en-US", "终止_us", "代码生成操作后缀.终止"),
            // dict.gen.button.category.terminate
            ("dict.gen.button.category.terminate", "ja-JP", "终止_jp", "代码生成操作后缀.终止"),
            // dict.gen.button.category.terminate
            ("dict.gen.button.category.terminate", "zh-CN", "终止", "代码生成操作后缀.终止"),
            // dict.gen.button.category.terminate
            ("dict.gen.button.category.terminate", "zh-HK", "终止_hk", "代码生成操作后缀.终止"),

            // dict.gen.button.category.field
            ("dict.gen.button.category.field", "en-US", "字段管理_us", "代码生成操作后缀.字段管理"),
            // dict.gen.button.category.field
            ("dict.gen.button.category.field", "ja-JP", "字段管理_jp", "代码生成操作后缀.字段管理"),
            // dict.gen.button.category.field
            ("dict.gen.button.category.field", "zh-CN", "字段管理", "代码生成操作后缀.字段管理"),
            // dict.gen.button.category.field
            ("dict.gen.button.category.field", "zh-HK", "字段管理_hk", "代码生成操作后缀.字段管理"),

            // dict.gen.button.category.permission
            ("dict.gen.button.category.permission", "en-US", "权限设置_us", "代码生成操作后缀.权限设置"),
            // dict.gen.button.category.permission
            ("dict.gen.button.category.permission", "ja-JP", "权限设置_jp", "代码生成操作后缀.权限设置"),
            // dict.gen.button.category.permission
            ("dict.gen.button.category.permission", "zh-CN", "权限设置", "代码生成操作后缀.权限设置"),
            // dict.gen.button.category.permission
            ("dict.gen.button.category.permission", "zh-HK", "权限设置_hk", "代码生成操作后缀.权限设置"),

            // dict.gen.button.category.datasource
            ("dict.gen.button.category.datasource", "en-US", "数据源配置_us", "代码生成操作后缀.数据源配置"),
            // dict.gen.button.category.datasource
            ("dict.gen.button.category.datasource", "ja-JP", "数据源配置_jp", "代码生成操作后缀.数据源配置"),
            // dict.gen.button.category.datasource
            ("dict.gen.button.category.datasource", "zh-CN", "数据源配置", "代码生成操作后缀.数据源配置"),
            // dict.gen.button.category.datasource
            ("dict.gen.button.category.datasource", "zh-HK", "数据源配置_hk", "代码生成操作后缀.数据源配置"),

            // dict.gen.button.category.theme
            ("dict.gen.button.category.theme", "en-US", "主题设置_us", "代码生成操作后缀.主题设置"),
            // dict.gen.button.category.theme
            ("dict.gen.button.category.theme", "ja-JP", "主题设置_jp", "代码生成操作后缀.主题设置"),
            // dict.gen.button.category.theme
            ("dict.gen.button.category.theme", "zh-CN", "主题设置", "代码生成操作后缀.主题设置"),
            // dict.gen.button.category.theme
            ("dict.gen.button.category.theme", "zh-HK", "主题设置_hk", "代码生成操作后缀.主题设置"),

            // dict.gen.button.category.data
            ("dict.gen.button.category.data", "en-US", "表单数据_us", "代码生成操作后缀.表单数据"),
            // dict.gen.button.category.data
            ("dict.gen.button.category.data", "ja-JP", "表单数据_jp", "代码生成操作后缀.表单数据"),
            // dict.gen.button.category.data
            ("dict.gen.button.category.data", "zh-CN", "表单数据", "代码生成操作后缀.表单数据"),
            // dict.gen.button.category.data
            ("dict.gen.button.category.data", "zh-HK", "表单数据_hk", "代码生成操作后缀.表单数据"),

            // dict.gen.button.category.archive
            ("dict.gen.button.category.archive", "en-US", "流转归档_us", "代码生成操作后缀.流转归档"),
            // dict.gen.button.category.archive
            ("dict.gen.button.category.archive", "ja-JP", "流转归档_jp", "代码生成操作后缀.流转归档"),
            // dict.gen.button.category.archive
            ("dict.gen.button.category.archive", "zh-CN", "流转归档", "代码生成操作后缀.流转归档"),
            // dict.gen.button.category.archive
            ("dict.gen.button.category.archive", "zh-HK", "流转归档_hk", "代码生成操作后缀.流转归档"),

            // dict.gen.button.category.clean
            ("dict.gen.button.category.clean", "en-US", "流转清理_us", "代码生成操作后缀.流转清理"),
            // dict.gen.button.category.clean
            ("dict.gen.button.category.clean", "ja-JP", "流转清理_jp", "代码生成操作后缀.流转清理"),
            // dict.gen.button.category.clean
            ("dict.gen.button.category.clean", "zh-CN", "流转清理", "代码生成操作后缀.流转清理"),
            // dict.gen.button.category.clean
            ("dict.gen.button.category.clean", "zh-HK", "流转清理_hk", "代码生成操作后缀.流转清理"),

            // dict.gen.button.category.draft
            ("dict.gen.button.category.draft", "en-US", "保存草稿_us", "代码生成操作后缀.保存草稿"),
            // dict.gen.button.category.draft
            ("dict.gen.button.category.draft", "ja-JP", "保存草稿_jp", "代码生成操作后缀.保存草稿"),
            // dict.gen.button.category.draft
            ("dict.gen.button.category.draft", "zh-CN", "保存草稿", "代码生成操作后缀.保存草稿"),
            // dict.gen.button.category.draft
            ("dict.gen.button.category.draft", "zh-HK", "保存草稿_hk", "代码生成操作后缀.保存草稿"),

            // dict.gen.button.category.deletedraft
            ("dict.gen.button.category.deletedraft", "en-US", "删除草稿_us", "代码生成操作后缀.删除草稿"),
            // dict.gen.button.category.deletedraft
            ("dict.gen.button.category.deletedraft", "ja-JP", "删除草稿_jp", "代码生成操作后缀.删除草稿"),
            // dict.gen.button.category.deletedraft
            ("dict.gen.button.category.deletedraft", "zh-CN", "删除草稿", "代码生成操作后缀.删除草稿"),
            // dict.gen.button.category.deletedraft
            ("dict.gen.button.category.deletedraft", "zh-HK", "删除草稿_hk", "代码生成操作后缀.删除草稿"),

            // dict.gen.button.category.send
            ("dict.gen.button.category.send", "en-US", "发送_us", "代码生成操作后缀.发送"),
            // dict.gen.button.category.send
            ("dict.gen.button.category.send", "ja-JP", "发送_jp", "代码生成操作后缀.发送"),
            // dict.gen.button.category.send
            ("dict.gen.button.category.send", "zh-CN", "发送", "代码生成操作后缀.发送"),
            // dict.gen.button.category.send
            ("dict.gen.button.category.send", "zh-HK", "发送_hk", "代码生成操作后缀.发送"),

            // dict.gen.button.category.forward
            ("dict.gen.button.category.forward", "en-US", "转发_us", "代码生成操作后缀.转发"),
            // dict.gen.button.category.forward
            ("dict.gen.button.category.forward", "ja-JP", "转发_jp", "代码生成操作后缀.转发"),
            // dict.gen.button.category.forward
            ("dict.gen.button.category.forward", "zh-CN", "转发", "代码生成操作后缀.转发"),
            // dict.gen.button.category.forward
            ("dict.gen.button.category.forward", "zh-HK", "转发_hk", "代码生成操作后缀.转发"),

            // dict.gen.button.category.reply
            ("dict.gen.button.category.reply", "en-US", "回复_us", "代码生成操作后缀.回复"),
            // dict.gen.button.category.reply
            ("dict.gen.button.category.reply", "ja-JP", "回复_jp", "代码生成操作后缀.回复"),
            // dict.gen.button.category.reply
            ("dict.gen.button.category.reply", "zh-CN", "回复", "代码生成操作后缀.回复"),
            // dict.gen.button.category.reply
            ("dict.gen.button.category.reply", "zh-HK", "回复_hk", "代码生成操作后缀.回复"),

            // dict.gen.button.category.read
            ("dict.gen.button.category.read", "en-US", "已读_us", "代码生成操作后缀.已读"),
            // dict.gen.button.category.read
            ("dict.gen.button.category.read", "ja-JP", "已读_jp", "代码生成操作后缀.已读"),
            // dict.gen.button.category.read
            ("dict.gen.button.category.read", "zh-CN", "已读", "代码生成操作后缀.已读"),
            // dict.gen.button.category.read
            ("dict.gen.button.category.read", "zh-HK", "已读_hk", "代码生成操作后缀.已读"),

            // dict.gen.button.category.unread
            ("dict.gen.button.category.unread", "en-US", "未读_us", "代码生成操作后缀.未读"),
            // dict.gen.button.category.unread
            ("dict.gen.button.category.unread", "ja-JP", "未读_jp", "代码生成操作后缀.未读"),
            // dict.gen.button.category.unread
            ("dict.gen.button.category.unread", "zh-CN", "未读", "代码生成操作后缀.未读"),
            // dict.gen.button.category.unread
            ("dict.gen.button.category.unread", "zh-HK", "未读_hk", "代码生成操作后缀.未读"),

            // dict.gen.button.category.circulate
            ("dict.gen.button.category.circulate", "en-US", "传阅_us", "代码生成操作后缀.传阅"),
            // dict.gen.button.category.circulate
            ("dict.gen.button.category.circulate", "ja-JP", "传阅_jp", "代码生成操作后缀.传阅"),
            // dict.gen.button.category.circulate
            ("dict.gen.button.category.circulate", "zh-CN", "传阅", "代码生成操作后缀.传阅"),
            // dict.gen.button.category.circulate
            ("dict.gen.button.category.circulate", "zh-HK", "传阅_hk", "代码生成操作后缀.传阅"),

            // dict.gen.button.category.sign
            ("dict.gen.button.category.sign", "en-US", "签收_us", "代码生成操作后缀.签收"),
            // dict.gen.button.category.sign
            ("dict.gen.button.category.sign", "ja-JP", "签收_jp", "代码生成操作后缀.签收"),
            // dict.gen.button.category.sign
            ("dict.gen.button.category.sign", "zh-CN", "签收", "代码生成操作后缀.签收"),
            // dict.gen.button.category.sign
            ("dict.gen.button.category.sign", "zh-HK", "签收_hk", "代码生成操作后缀.签收"),

            // dict.gen.button.category.confirm
            ("dict.gen.button.category.confirm", "en-US", "确认_us", "代码生成操作后缀.确认"),
            // dict.gen.button.category.confirm
            ("dict.gen.button.category.confirm", "ja-JP", "确认_jp", "代码生成操作后缀.确认"),
            // dict.gen.button.category.confirm
            ("dict.gen.button.category.confirm", "zh-CN", "确认", "代码生成操作后缀.确认"),
            // dict.gen.button.category.confirm
            ("dict.gen.button.category.confirm", "zh-HK", "确认_hk", "代码生成操作后缀.确认"),

            // dict.gen.button.category.like
            ("dict.gen.button.category.like", "en-US", "点赞_us", "代码生成操作后缀.点赞"),
            // dict.gen.button.category.like
            ("dict.gen.button.category.like", "ja-JP", "点赞_jp", "代码生成操作后缀.点赞"),
            // dict.gen.button.category.like
            ("dict.gen.button.category.like", "zh-CN", "点赞", "代码生成操作后缀.点赞"),
            // dict.gen.button.category.like
            ("dict.gen.button.category.like", "zh-HK", "点赞_hk", "代码生成操作后缀.点赞"),

            // dict.gen.button.category.unlike
            ("dict.gen.button.category.unlike", "en-US", "取消点赞_us", "代码生成操作后缀.取消点赞"),
            // dict.gen.button.category.unlike
            ("dict.gen.button.category.unlike", "ja-JP", "取消点赞_jp", "代码生成操作后缀.取消点赞"),
            // dict.gen.button.category.unlike
            ("dict.gen.button.category.unlike", "zh-CN", "取消点赞", "代码生成操作后缀.取消点赞"),
            // dict.gen.button.category.unlike
            ("dict.gen.button.category.unlike", "zh-HK", "取消点赞_hk", "代码生成操作后缀.取消点赞"),

            // dict.gen.button.category.favorite
            ("dict.gen.button.category.favorite", "en-US", "收藏_us", "代码生成操作后缀.收藏"),
            // dict.gen.button.category.favorite
            ("dict.gen.button.category.favorite", "ja-JP", "收藏_jp", "代码生成操作后缀.收藏"),
            // dict.gen.button.category.favorite
            ("dict.gen.button.category.favorite", "zh-CN", "收藏", "代码生成操作后缀.收藏"),
            // dict.gen.button.category.favorite
            ("dict.gen.button.category.favorite", "zh-HK", "收藏_hk", "代码生成操作后缀.收藏"),

            // dict.gen.button.category.unfavorite
            ("dict.gen.button.category.unfavorite", "en-US", "取消收藏_us", "代码生成操作后缀.取消收藏"),
            // dict.gen.button.category.unfavorite
            ("dict.gen.button.category.unfavorite", "ja-JP", "取消收藏_jp", "代码生成操作后缀.取消收藏"),
            // dict.gen.button.category.unfavorite
            ("dict.gen.button.category.unfavorite", "zh-CN", "取消收藏", "代码生成操作后缀.取消收藏"),
            // dict.gen.button.category.unfavorite
            ("dict.gen.button.category.unfavorite", "zh-HK", "取消收藏_hk", "代码生成操作后缀.取消收藏"),

            // dict.gen.button.category.share
            ("dict.gen.button.category.share", "en-US", "分享_us", "代码生成操作后缀.分享"),
            // dict.gen.button.category.share
            ("dict.gen.button.category.share", "ja-JP", "分享_jp", "代码生成操作后缀.分享"),
            // dict.gen.button.category.share
            ("dict.gen.button.category.share", "zh-CN", "分享", "代码生成操作后缀.分享"),
            // dict.gen.button.category.share
            ("dict.gen.button.category.share", "zh-HK", "分享_hk", "代码生成操作后缀.分享"),

            // dict.gen.button.category.unshare
            ("dict.gen.button.category.unshare", "en-US", "取消分享_us", "代码生成操作后缀.取消分享"),
            // dict.gen.button.category.unshare
            ("dict.gen.button.category.unshare", "ja-JP", "取消分享_jp", "代码生成操作后缀.取消分享"),
            // dict.gen.button.category.unshare
            ("dict.gen.button.category.unshare", "zh-CN", "取消分享", "代码生成操作后缀.取消分享"),
            // dict.gen.button.category.unshare
            ("dict.gen.button.category.unshare", "zh-HK", "取消分享_hk", "代码生成操作后缀.取消分享"),

            // dict.gen.button.category.comment
            ("dict.gen.button.category.comment", "en-US", "评论_us", "代码生成操作后缀.评论"),
            // dict.gen.button.category.comment
            ("dict.gen.button.category.comment", "ja-JP", "评论_jp", "代码生成操作后缀.评论"),
            // dict.gen.button.category.comment
            ("dict.gen.button.category.comment", "zh-CN", "评论", "代码生成操作后缀.评论"),
            // dict.gen.button.category.comment
            ("dict.gen.button.category.comment", "zh-HK", "评论_hk", "代码生成操作后缀.评论"),

            // dict.gen.button.category.uncomment
            ("dict.gen.button.category.uncomment", "en-US", "取消评论_us", "代码生成操作后缀.取消评论"),
            // dict.gen.button.category.uncomment
            ("dict.gen.button.category.uncomment", "ja-JP", "取消评论_jp", "代码生成操作后缀.取消评论"),
            // dict.gen.button.category.uncomment
            ("dict.gen.button.category.uncomment", "zh-CN", "取消评论", "代码生成操作后缀.取消评论"),
            // dict.gen.button.category.uncomment
            ("dict.gen.button.category.uncomment", "zh-HK", "取消评论_hk", "代码生成操作后缀.取消评论"),

            // dict.gen.button.category.flagging
            ("dict.gen.button.category.flagging", "en-US", "举报_us", "代码生成操作后缀.举报"),
            // dict.gen.button.category.flagging
            ("dict.gen.button.category.flagging", "ja-JP", "举报_jp", "代码生成操作后缀.举报"),
            // dict.gen.button.category.flagging
            ("dict.gen.button.category.flagging", "zh-CN", "举报", "代码生成操作后缀.举报"),
            // dict.gen.button.category.flagging
            ("dict.gen.button.category.flagging", "zh-HK", "举报_hk", "代码生成操作后缀.举报"),

            // dict.gen.button.category.unflagging
            ("dict.gen.button.category.unflagging", "en-US", "取消举报_us", "代码生成操作后缀.取消举报"),
            // dict.gen.button.category.unflagging
            ("dict.gen.button.category.unflagging", "ja-JP", "取消举报_jp", "代码生成操作后缀.取消举报"),
            // dict.gen.button.category.unflagging
            ("dict.gen.button.category.unflagging", "zh-CN", "取消举报", "代码生成操作后缀.取消举报"),
            // dict.gen.button.category.unflagging
            ("dict.gen.button.category.unflagging", "zh-HK", "取消举报_hk", "代码生成操作后缀.取消举报"),

            // dict.gen.button.category.follow
            ("dict.gen.button.category.follow", "en-US", "关注_us", "代码生成操作后缀.关注"),
            // dict.gen.button.category.follow
            ("dict.gen.button.category.follow", "ja-JP", "关注_jp", "代码生成操作后缀.关注"),
            // dict.gen.button.category.follow
            ("dict.gen.button.category.follow", "zh-CN", "关注", "代码生成操作后缀.关注"),
            // dict.gen.button.category.follow
            ("dict.gen.button.category.follow", "zh-HK", "关注_hk", "代码生成操作后缀.关注"),

            // dict.gen.button.category.unfollow
            ("dict.gen.button.category.unfollow", "en-US", "取消关注_us", "代码生成操作后缀.取消关注"),
            // dict.gen.button.category.unfollow
            ("dict.gen.button.category.unfollow", "ja-JP", "取消关注_jp", "代码生成操作后缀.取消关注"),
            // dict.gen.button.category.unfollow
            ("dict.gen.button.category.unfollow", "zh-CN", "取消关注", "代码生成操作后缀.取消关注"),
            // dict.gen.button.category.unfollow
            ("dict.gen.button.category.unfollow", "zh-HK", "取消关注_hk", "代码生成操作后缀.取消关注"),

            // dict.gen.button.category.upload
            ("dict.gen.button.category.upload", "en-US", "上传_us", "代码生成操作后缀.上传"),
            // dict.gen.button.category.upload
            ("dict.gen.button.category.upload", "ja-JP", "上传_jp", "代码生成操作后缀.上传"),
            // dict.gen.button.category.upload
            ("dict.gen.button.category.upload", "zh-CN", "上传", "代码生成操作后缀.上传"),
            // dict.gen.button.category.upload
            ("dict.gen.button.category.upload", "zh-HK", "上传_hk", "代码生成操作后缀.上传"),

            // dict.gen.button.category.destroy
            ("dict.gen.button.category.destroy", "en-US", "销毁_us", "代码生成操作后缀.销毁"),
            // dict.gen.button.category.destroy
            ("dict.gen.button.category.destroy", "ja-JP", "销毁_jp", "代码生成操作后缀.销毁"),
            // dict.gen.button.category.destroy
            ("dict.gen.button.category.destroy", "zh-CN", "销毁", "代码生成操作后缀.销毁"),
            // dict.gen.button.category.destroy
            ("dict.gen.button.category.destroy", "zh-HK", "销毁_hk", "代码生成操作后缀.销毁"),

            // dict.gen.button.category.run
            ("dict.gen.button.category.run", "en-US", "运行_us", "代码生成操作后缀.运行"),
            // dict.gen.button.category.run
            ("dict.gen.button.category.run", "ja-JP", "运行_jp", "代码生成操作后缀.运行"),
            // dict.gen.button.category.run
            ("dict.gen.button.category.run", "zh-CN", "运行", "代码生成操作后缀.运行"),
            // dict.gen.button.category.run
            ("dict.gen.button.category.run", "zh-HK", "运行_hk", "代码生成操作后缀.运行"),

            // dict.gen.button.category.stop
            ("dict.gen.button.category.stop", "en-US", "停止_us", "代码生成操作后缀.停止"),
            // dict.gen.button.category.stop
            ("dict.gen.button.category.stop", "ja-JP", "停止_jp", "代码生成操作后缀.停止"),
            // dict.gen.button.category.stop
            ("dict.gen.button.category.stop", "zh-CN", "停止", "代码生成操作后缀.停止"),
            // dict.gen.button.category.stop
            ("dict.gen.button.category.stop", "zh-HK", "停止_hk", "代码生成操作后缀.停止"),

            // dict.gen.button.category.restart
            ("dict.gen.button.category.restart", "en-US", "重启_us", "代码生成操作后缀.重启"),
            // dict.gen.button.category.restart
            ("dict.gen.button.category.restart", "ja-JP", "重启_jp", "代码生成操作后缀.重启"),
            // dict.gen.button.category.restart
            ("dict.gen.button.category.restart", "zh-CN", "重启", "代码生成操作后缀.重启"),
            // dict.gen.button.category.restart
            ("dict.gen.button.category.restart", "zh-HK", "重启_hk", "代码生成操作后缀.重启"),

            // dict.gen.button.category.refresh
            ("dict.gen.button.category.refresh", "en-US", "刷新_us", "代码生成操作后缀.刷新"),
            // dict.gen.button.category.refresh
            ("dict.gen.button.category.refresh", "ja-JP", "刷新_jp", "代码生成操作后缀.刷新"),
            // dict.gen.button.category.refresh
            ("dict.gen.button.category.refresh", "zh-CN", "刷新", "代码生成操作后缀.刷新"),
            // dict.gen.button.category.refresh
            ("dict.gen.button.category.refresh", "zh-HK", "刷新_hk", "代码生成操作后缀.刷新"),

            // dict.gen.button.category.reset
            ("dict.gen.button.category.reset", "en-US", "重置_us", "代码生成操作后缀.重置"),
            // dict.gen.button.category.reset
            ("dict.gen.button.category.reset", "ja-JP", "重置_jp", "代码生成操作后缀.重置"),
            // dict.gen.button.category.reset
            ("dict.gen.button.category.reset", "zh-CN", "重置", "代码生成操作后缀.重置"),
            // dict.gen.button.category.reset
            ("dict.gen.button.category.reset", "zh-HK", "重置_hk", "代码生成操作后缀.重置"),

            // dict.gen.button.category.calculate
            ("dict.gen.button.category.calculate", "en-US", "核算_us", "代码生成操作后缀.核算"),
            // dict.gen.button.category.calculate
            ("dict.gen.button.category.calculate", "ja-JP", "核算_jp", "代码生成操作后缀.核算"),
            // dict.gen.button.category.calculate
            ("dict.gen.button.category.calculate", "zh-CN", "核算", "代码生成操作后缀.核算"),
            // dict.gen.button.category.calculate
            ("dict.gen.button.category.calculate", "zh-HK", "核算_hk", "代码生成操作后缀.核算"),

            // dict.gen.button.category.book
            ("dict.gen.button.category.book", "en-US", "记账_us", "代码生成操作后缀.记账"),
            // dict.gen.button.category.book
            ("dict.gen.button.category.book", "ja-JP", "记账_jp", "代码生成操作后缀.记账"),
            // dict.gen.button.category.book
            ("dict.gen.button.category.book", "zh-CN", "记账", "代码生成操作后缀.记账"),
            // dict.gen.button.category.book
            ("dict.gen.button.category.book", "zh-HK", "记账_hk", "代码生成操作后缀.记账"),

            // dict.gen.button.category.closing
            ("dict.gen.button.category.closing", "en-US", "结账_us", "代码生成操作后缀.结账"),
            // dict.gen.button.category.closing
            ("dict.gen.button.category.closing", "ja-JP", "结账_jp", "代码生成操作后缀.结账"),
            // dict.gen.button.category.closing
            ("dict.gen.button.category.closing", "zh-CN", "结账", "代码生成操作后缀.结账"),
            // dict.gen.button.category.closing
            ("dict.gen.button.category.closing", "zh-HK", "结账_hk", "代码生成操作后缀.结账"),

            // dict.gen.button.category.reconcile
            ("dict.gen.button.category.reconcile", "en-US", "对账_us", "代码生成操作后缀.对账"),
            // dict.gen.button.category.reconcile
            ("dict.gen.button.category.reconcile", "ja-JP", "对账_jp", "代码生成操作后缀.对账"),
            // dict.gen.button.category.reconcile
            ("dict.gen.button.category.reconcile", "zh-CN", "对账", "代码生成操作后缀.对账"),
            // dict.gen.button.category.reconcile
            ("dict.gen.button.category.reconcile", "zh-HK", "对账_hk", "代码生成操作后缀.对账"),

            // dict.gen.button.category.payment
            ("dict.gen.button.category.payment", "en-US", "支付_us", "代码生成操作后缀.支付"),
            // dict.gen.button.category.payment
            ("dict.gen.button.category.payment", "ja-JP", "支付_jp", "代码生成操作后缀.支付"),
            // dict.gen.button.category.payment
            ("dict.gen.button.category.payment", "zh-CN", "支付", "代码生成操作后缀.支付"),
            // dict.gen.button.category.payment
            ("dict.gen.button.category.payment", "zh-HK", "支付_hk", "代码生成操作后缀.支付"),

            // dict.gen.button.category.depreciation
            ("dict.gen.button.category.depreciation", "en-US", "折旧_us", "代码生成操作后缀.折旧"),
            // dict.gen.button.category.depreciation
            ("dict.gen.button.category.depreciation", "ja-JP", "折旧_jp", "代码生成操作后缀.折旧"),
            // dict.gen.button.category.depreciation
            ("dict.gen.button.category.depreciation", "zh-CN", "折旧", "代码生成操作后缀.折旧"),
            // dict.gen.button.category.depreciation
            ("dict.gen.button.category.depreciation", "zh-HK", "折旧_hk", "代码生成操作后缀.折旧"),

            // dict.gen.button.category.reimburse
            ("dict.gen.button.category.reimburse", "en-US", "报销_us", "代码生成操作后缀.报销"),
            // dict.gen.button.category.reimburse
            ("dict.gen.button.category.reimburse", "ja-JP", "报销_jp", "代码生成操作后缀.报销"),
            // dict.gen.button.category.reimburse
            ("dict.gen.button.category.reimburse", "zh-CN", "报销", "代码生成操作后缀.报销"),
            // dict.gen.button.category.reimburse
            ("dict.gen.button.category.reimburse", "zh-HK", "报销_hk", "代码生成操作后缀.报销"),

            // dict.gen.button.category.reversal
            ("dict.gen.button.category.reversal", "en-US", "冲销_us", "代码生成操作后缀.冲销"),
            // dict.gen.button.category.reversal
            ("dict.gen.button.category.reversal", "ja-JP", "冲销_jp", "代码生成操作后缀.冲销"),
            // dict.gen.button.category.reversal
            ("dict.gen.button.category.reversal", "zh-CN", "冲销", "代码生成操作后缀.冲销"),
            // dict.gen.button.category.reversal
            ("dict.gen.button.category.reversal", "zh-HK", "冲销_hk", "代码生成操作后缀.冲销"),

            // dict.gen.button.category.accrual
            ("dict.gen.button.category.accrual", "en-US", "计提_us", "代码生成操作后缀.计提"),
            // dict.gen.button.category.accrual
            ("dict.gen.button.category.accrual", "ja-JP", "计提_jp", "代码生成操作后缀.计提"),
            // dict.gen.button.category.accrual
            ("dict.gen.button.category.accrual", "zh-CN", "计提", "代码生成操作后缀.计提"),
            // dict.gen.button.category.accrual
            ("dict.gen.button.category.accrual", "zh-HK", "计提_hk", "代码生成操作后缀.计提"),

            // dict.gen.button.category.period
            ("dict.gen.button.category.period", "en-US", "账期_us", "代码生成操作后缀.账期"),
            // dict.gen.button.category.period
            ("dict.gen.button.category.period", "ja-JP", "账期_jp", "代码生成操作后缀.账期"),
            // dict.gen.button.category.period
            ("dict.gen.button.category.period", "zh-CN", "账期", "代码生成操作后缀.账期"),
            // dict.gen.button.category.period
            ("dict.gen.button.category.period", "zh-HK", "账期_hk", "代码生成操作后缀.账期"),

            // dict.gen.button.category.carryforward
            ("dict.gen.button.category.carryforward", "en-US", "结转_us", "代码生成操作后缀.结转"),
            // dict.gen.button.category.carryforward
            ("dict.gen.button.category.carryforward", "ja-JP", "结转_jp", "代码生成操作后缀.结转"),
            // dict.gen.button.category.carryforward
            ("dict.gen.button.category.carryforward", "zh-CN", "结转", "代码生成操作后缀.结转"),
            // dict.gen.button.category.carryforward
            ("dict.gen.button.category.carryforward", "zh-HK", "结转_hk", "代码生成操作后缀.结转"),

            // dict.gen.button.category.cancel
            ("dict.gen.button.category.cancel", "en-US", "作废_us", "代码生成操作后缀.作废"),
            // dict.gen.button.category.cancel
            ("dict.gen.button.category.cancel", "ja-JP", "作废_jp", "代码生成操作后缀.作废"),
            // dict.gen.button.category.cancel
            ("dict.gen.button.category.cancel", "zh-CN", "作废", "代码生成操作后缀.作废"),
            // dict.gen.button.category.cancel
            ("dict.gen.button.category.cancel", "zh-HK", "作废_hk", "代码生成操作后缀.作废"),

            // dict.gen.button.category.change
            ("dict.gen.button.category.change", "en-US", "变更_us", "代码生成操作后缀.变更"),
            // dict.gen.button.category.change
            ("dict.gen.button.category.change", "ja-JP", "变更_jp", "代码生成操作后缀.变更"),
            // dict.gen.button.category.change
            ("dict.gen.button.category.change", "zh-CN", "变更", "代码生成操作后缀.变更"),
            // dict.gen.button.category.change
            ("dict.gen.button.category.change", "zh-HK", "变更_hk", "代码生成操作后缀.变更"),

            // dict.gen.button.style.config.0
            ("dict.gen.button.style.config.0", "en-US", "文本_us", "操作按钮样式.文本"),
            // dict.gen.button.style.config.0
            ("dict.gen.button.style.config.0", "ja-JP", "文本_jp", "操作按钮样式.文本"),
            // dict.gen.button.style.config.0
            ("dict.gen.button.style.config.0", "zh-CN", "文本", "操作按钮样式.文本"),
            // dict.gen.button.style.config.0
            ("dict.gen.button.style.config.0", "zh-HK", "文本_hk", "操作按钮样式.文本"),

            // dict.gen.button.style.config.1
            ("dict.gen.button.style.config.1", "en-US", "标准_us", "操作按钮样式.标准"),
            // dict.gen.button.style.config.1
            ("dict.gen.button.style.config.1", "ja-JP", "标准_jp", "操作按钮样式.标准"),
            // dict.gen.button.style.config.1
            ("dict.gen.button.style.config.1", "zh-CN", "标准", "操作按钮样式.标准"),
            // dict.gen.button.style.config.1
            ("dict.gen.button.style.config.1", "zh-HK", "标准_hk", "操作按钮样式.标准"),

            // dict.gen.csharp.data.type.bool
            ("dict.gen.csharp.data.type.bool", "en-US", "bool_us", "c#数据类型.bool"),
            // dict.gen.csharp.data.type.bool
            ("dict.gen.csharp.data.type.bool", "ja-JP", "bool_jp", "c#数据类型.bool"),
            // dict.gen.csharp.data.type.bool
            ("dict.gen.csharp.data.type.bool", "zh-CN", "bool", "c#数据类型.bool"),
            // dict.gen.csharp.data.type.bool
            ("dict.gen.csharp.data.type.bool", "zh-HK", "bool_hk", "c#数据类型.bool"),

            // dict.gen.csharp.data.type.byte
            ("dict.gen.csharp.data.type.byte", "en-US", "byte_us", "c#数据类型.byte"),
            // dict.gen.csharp.data.type.byte
            ("dict.gen.csharp.data.type.byte", "ja-JP", "byte_jp", "c#数据类型.byte"),
            // dict.gen.csharp.data.type.byte
            ("dict.gen.csharp.data.type.byte", "zh-CN", "byte", "c#数据类型.byte"),
            // dict.gen.csharp.data.type.byte
            ("dict.gen.csharp.data.type.byte", "zh-HK", "byte_hk", "c#数据类型.byte"),

            // dict.gen.csharp.data.type.datetime
            ("dict.gen.csharp.data.type.datetime", "en-US", "datetime_us", "c#数据类型.datetime"),
            // dict.gen.csharp.data.type.datetime
            ("dict.gen.csharp.data.type.datetime", "ja-JP", "datetime_jp", "c#数据类型.datetime"),
            // dict.gen.csharp.data.type.datetime
            ("dict.gen.csharp.data.type.datetime", "zh-CN", "datetime", "c#数据类型.datetime"),
            // dict.gen.csharp.data.type.datetime
            ("dict.gen.csharp.data.type.datetime", "zh-HK", "datetime_hk", "c#数据类型.datetime"),

            // dict.gen.csharp.data.type.decimal
            ("dict.gen.csharp.data.type.decimal", "en-US", "decimal_us", "c#数据类型.decimal"),
            // dict.gen.csharp.data.type.decimal
            ("dict.gen.csharp.data.type.decimal", "ja-JP", "decimal_jp", "c#数据类型.decimal"),
            // dict.gen.csharp.data.type.decimal
            ("dict.gen.csharp.data.type.decimal", "zh-CN", "decimal", "c#数据类型.decimal"),
            // dict.gen.csharp.data.type.decimal
            ("dict.gen.csharp.data.type.decimal", "zh-HK", "decimal_hk", "c#数据类型.decimal"),

            // dict.gen.csharp.data.type.double
            ("dict.gen.csharp.data.type.double", "en-US", "double_us", "c#数据类型.double"),
            // dict.gen.csharp.data.type.double
            ("dict.gen.csharp.data.type.double", "ja-JP", "double_jp", "c#数据类型.double"),
            // dict.gen.csharp.data.type.double
            ("dict.gen.csharp.data.type.double", "zh-CN", "double", "c#数据类型.double"),
            // dict.gen.csharp.data.type.double
            ("dict.gen.csharp.data.type.double", "zh-HK", "double_hk", "c#数据类型.double"),

            // dict.gen.csharp.data.type.float
            ("dict.gen.csharp.data.type.float", "en-US", "float_us", "c#数据类型.float"),
            // dict.gen.csharp.data.type.float
            ("dict.gen.csharp.data.type.float", "ja-JP", "float_jp", "c#数据类型.float"),
            // dict.gen.csharp.data.type.float
            ("dict.gen.csharp.data.type.float", "zh-CN", "float", "c#数据类型.float"),
            // dict.gen.csharp.data.type.float
            ("dict.gen.csharp.data.type.float", "zh-HK", "float_hk", "c#数据类型.float"),

            // dict.gen.csharp.data.type.guid
            ("dict.gen.csharp.data.type.guid", "en-US", "guid_us", "c#数据类型.guid"),
            // dict.gen.csharp.data.type.guid
            ("dict.gen.csharp.data.type.guid", "ja-JP", "guid_jp", "c#数据类型.guid"),
            // dict.gen.csharp.data.type.guid
            ("dict.gen.csharp.data.type.guid", "zh-CN", "guid", "c#数据类型.guid"),
            // dict.gen.csharp.data.type.guid
            ("dict.gen.csharp.data.type.guid", "zh-HK", "guid_hk", "c#数据类型.guid"),

            // dict.gen.csharp.data.type.int
            ("dict.gen.csharp.data.type.int", "en-US", "int_us", "c#数据类型.int"),
            // dict.gen.csharp.data.type.int
            ("dict.gen.csharp.data.type.int", "ja-JP", "int_jp", "c#数据类型.int"),
            // dict.gen.csharp.data.type.int
            ("dict.gen.csharp.data.type.int", "zh-CN", "int", "c#数据类型.int"),
            // dict.gen.csharp.data.type.int
            ("dict.gen.csharp.data.type.int", "zh-HK", "int_hk", "c#数据类型.int"),

            // dict.gen.csharp.data.type.long
            ("dict.gen.csharp.data.type.long", "en-US", "long_us", "c#数据类型.long"),
            // dict.gen.csharp.data.type.long
            ("dict.gen.csharp.data.type.long", "ja-JP", "long_jp", "c#数据类型.long"),
            // dict.gen.csharp.data.type.long
            ("dict.gen.csharp.data.type.long", "zh-CN", "long", "c#数据类型.long"),
            // dict.gen.csharp.data.type.long
            ("dict.gen.csharp.data.type.long", "zh-HK", "long_hk", "c#数据类型.long"),

            // dict.gen.csharp.data.type.string
            ("dict.gen.csharp.data.type.string", "en-US", "string_us", "c#数据类型.string"),
            // dict.gen.csharp.data.type.string
            ("dict.gen.csharp.data.type.string", "ja-JP", "string_jp", "c#数据类型.string"),
            // dict.gen.csharp.data.type.string
            ("dict.gen.csharp.data.type.string", "zh-CN", "string", "c#数据类型.string"),
            // dict.gen.csharp.data.type.string
            ("dict.gen.csharp.data.type.string", "zh-HK", "string_hk", "c#数据类型.string"),

            // dict.gen.display.type.input
            ("dict.gen.display.type.input", "en-US", "文本框_us", "显示类型.文本框"),
            // dict.gen.display.type.input
            ("dict.gen.display.type.input", "ja-JP", "文本框_jp", "显示类型.文本框"),
            // dict.gen.display.type.input
            ("dict.gen.display.type.input", "zh-CN", "文本框", "显示类型.文本框"),
            // dict.gen.display.type.input
            ("dict.gen.display.type.input", "zh-HK", "文本框_hk", "显示类型.文本框"),

            // dict.gen.display.type.inputnumber
            ("dict.gen.display.type.inputnumber", "en-US", "数字输入框_us", "显示类型.数字输入框"),
            // dict.gen.display.type.inputnumber
            ("dict.gen.display.type.inputnumber", "ja-JP", "数字输入框_jp", "显示类型.数字输入框"),
            // dict.gen.display.type.inputnumber
            ("dict.gen.display.type.inputnumber", "zh-CN", "数字输入框", "显示类型.数字输入框"),
            // dict.gen.display.type.inputnumber
            ("dict.gen.display.type.inputnumber", "zh-HK", "数字输入框_hk", "显示类型.数字输入框"),

            // dict.gen.display.type.select
            ("dict.gen.display.type.select", "en-US", "下拉框_us", "显示类型.下拉框"),
            // dict.gen.display.type.select
            ("dict.gen.display.type.select", "ja-JP", "下拉框_jp", "显示类型.下拉框"),
            // dict.gen.display.type.select
            ("dict.gen.display.type.select", "zh-CN", "下拉框", "显示类型.下拉框"),
            // dict.gen.display.type.select
            ("dict.gen.display.type.select", "zh-HK", "下拉框_hk", "显示类型.下拉框"),

            // dict.gen.display.type.checkbox
            ("dict.gen.display.type.checkbox", "en-US", "复选框_us", "显示类型.复选框"),
            // dict.gen.display.type.checkbox
            ("dict.gen.display.type.checkbox", "ja-JP", "复选框_jp", "显示类型.复选框"),
            // dict.gen.display.type.checkbox
            ("dict.gen.display.type.checkbox", "zh-CN", "复选框", "显示类型.复选框"),
            // dict.gen.display.type.checkbox
            ("dict.gen.display.type.checkbox", "zh-HK", "复选框_hk", "显示类型.复选框"),

            // dict.gen.display.type.radio
            ("dict.gen.display.type.radio", "en-US", "单选框_us", "显示类型.单选框"),
            // dict.gen.display.type.radio
            ("dict.gen.display.type.radio", "ja-JP", "单选框_jp", "显示类型.单选框"),
            // dict.gen.display.type.radio
            ("dict.gen.display.type.radio", "zh-CN", "单选框", "显示类型.单选框"),
            // dict.gen.display.type.radio
            ("dict.gen.display.type.radio", "zh-HK", "单选框_hk", "显示类型.单选框"),

            // dict.gen.display.type.date
            ("dict.gen.display.type.date", "en-US", "日期控件_us", "显示类型.日期控件"),
            // dict.gen.display.type.date
            ("dict.gen.display.type.date", "ja-JP", "日期控件_jp", "显示类型.日期控件"),
            // dict.gen.display.type.date
            ("dict.gen.display.type.date", "zh-CN", "日期控件", "显示类型.日期控件"),
            // dict.gen.display.type.date
            ("dict.gen.display.type.date", "zh-HK", "日期控件_hk", "显示类型.日期控件"),

            // dict.gen.display.type.time
            ("dict.gen.display.type.time", "en-US", "时间控件_us", "显示类型.时间控件"),
            // dict.gen.display.type.time
            ("dict.gen.display.type.time", "ja-JP", "时间控件_jp", "显示类型.时间控件"),
            // dict.gen.display.type.time
            ("dict.gen.display.type.time", "zh-CN", "时间控件", "显示类型.时间控件"),
            // dict.gen.display.type.time
            ("dict.gen.display.type.time", "zh-HK", "时间控件_hk", "显示类型.时间控件"),

            // dict.gen.display.type.image
            ("dict.gen.display.type.image", "en-US", "图片上传_us", "显示类型.图片上传"),
            // dict.gen.display.type.image
            ("dict.gen.display.type.image", "ja-JP", "图片上传_jp", "显示类型.图片上传"),
            // dict.gen.display.type.image
            ("dict.gen.display.type.image", "zh-CN", "图片上传", "显示类型.图片上传"),
            // dict.gen.display.type.image
            ("dict.gen.display.type.image", "zh-HK", "图片上传_hk", "显示类型.图片上传"),

            // dict.gen.display.type.file
            ("dict.gen.display.type.file", "en-US", "文件上传_us", "显示类型.文件上传"),
            // dict.gen.display.type.file
            ("dict.gen.display.type.file", "ja-JP", "文件上传_jp", "显示类型.文件上传"),
            // dict.gen.display.type.file
            ("dict.gen.display.type.file", "zh-CN", "文件上传", "显示类型.文件上传"),
            // dict.gen.display.type.file
            ("dict.gen.display.type.file", "zh-HK", "文件上传_hk", "显示类型.文件上传"),

            // dict.gen.display.type.slider
            ("dict.gen.display.type.slider", "en-US", "滑块_us", "显示类型.滑块"),
            // dict.gen.display.type.slider
            ("dict.gen.display.type.slider", "ja-JP", "滑块_jp", "显示类型.滑块"),
            // dict.gen.display.type.slider
            ("dict.gen.display.type.slider", "zh-CN", "滑块", "显示类型.滑块"),
            // dict.gen.display.type.slider
            ("dict.gen.display.type.slider", "zh-HK", "滑块_hk", "显示类型.滑块"),

            // dict.gen.display.type.switch
            ("dict.gen.display.type.switch", "en-US", "开关_us", "显示类型.开关"),
            // dict.gen.display.type.switch
            ("dict.gen.display.type.switch", "ja-JP", "开关_jp", "显示类型.开关"),
            // dict.gen.display.type.switch
            ("dict.gen.display.type.switch", "zh-CN", "开关", "显示类型.开关"),
            // dict.gen.display.type.switch
            ("dict.gen.display.type.switch", "zh-HK", "开关_hk", "显示类型.开关"),

            // dict.gen.display.type.rate
            ("dict.gen.display.type.rate", "en-US", "评分_us", "显示类型.评分"),
            // dict.gen.display.type.rate
            ("dict.gen.display.type.rate", "ja-JP", "评分_jp", "显示类型.评分"),
            // dict.gen.display.type.rate
            ("dict.gen.display.type.rate", "zh-CN", "评分", "显示类型.评分"),
            // dict.gen.display.type.rate
            ("dict.gen.display.type.rate", "zh-HK", "评分_hk", "显示类型.评分"),

            // dict.gen.display.type.textarea
            ("dict.gen.display.type.textarea", "en-US", "文本域_us", "显示类型.文本域"),
            // dict.gen.display.type.textarea
            ("dict.gen.display.type.textarea", "ja-JP", "文本域_jp", "显示类型.文本域"),
            // dict.gen.display.type.textarea
            ("dict.gen.display.type.textarea", "zh-CN", "文本域", "显示类型.文本域"),
            // dict.gen.display.type.textarea
            ("dict.gen.display.type.textarea", "zh-HK", "文本域_hk", "显示类型.文本域"),

            // dict.gen.display.type.editor
            ("dict.gen.display.type.editor", "en-US", "富文本编辑器_us", "显示类型.富文本编辑器"),
            // dict.gen.display.type.editor
            ("dict.gen.display.type.editor", "ja-JP", "富文本编辑器_jp", "显示类型.富文本编辑器"),
            // dict.gen.display.type.editor
            ("dict.gen.display.type.editor", "zh-CN", "富文本编辑器", "显示类型.富文本编辑器"),
            // dict.gen.display.type.editor
            ("dict.gen.display.type.editor", "zh-HK", "富文本编辑器_hk", "显示类型.富文本编辑器"),

            // dict.gen.frontend.form.layout.config.12
            ("dict.gen.frontend.form.layout.config.12", "en-US", "一行一列_us", "前端表单布局.一行一列"),
            // dict.gen.frontend.form.layout.config.12
            ("dict.gen.frontend.form.layout.config.12", "ja-JP", "一行一列_jp", "前端表单布局.一行一列"),
            // dict.gen.frontend.form.layout.config.12
            ("dict.gen.frontend.form.layout.config.12", "zh-CN", "一行一列", "前端表单布局.一行一列"),
            // dict.gen.frontend.form.layout.config.12
            ("dict.gen.frontend.form.layout.config.12", "zh-HK", "一行一列_hk", "前端表单布局.一行一列"),

            // dict.gen.frontend.form.layout.config.24
            ("dict.gen.frontend.form.layout.config.24", "en-US", "一行两列_us", "前端表单布局.一行两列"),
            // dict.gen.frontend.form.layout.config.24
            ("dict.gen.frontend.form.layout.config.24", "ja-JP", "一行两列_jp", "前端表单布局.一行两列"),
            // dict.gen.frontend.form.layout.config.24
            ("dict.gen.frontend.form.layout.config.24", "zh-CN", "一行两列", "前端表单布局.一行两列"),
            // dict.gen.frontend.form.layout.config.24
            ("dict.gen.frontend.form.layout.config.24", "zh-HK", "一行两列_hk", "前端表单布局.一行两列"),

            // dict.gen.frontend.ui.type.1
            ("dict.gen.frontend.ui.type.1", "en-US", "element plus_us", "前端ui框架.element plus"),
            // dict.gen.frontend.ui.type.1
            ("dict.gen.frontend.ui.type.1", "ja-JP", "element plus_jp", "前端ui框架.element plus"),
            // dict.gen.frontend.ui.type.1
            ("dict.gen.frontend.ui.type.1", "zh-CN", "element plus", "前端ui框架.element plus"),
            // dict.gen.frontend.ui.type.1
            ("dict.gen.frontend.ui.type.1", "zh-HK", "element plus_hk", "前端ui框架.element plus"),

            // dict.gen.frontend.ui.type.2
            ("dict.gen.frontend.ui.type.2", "en-US", "ant design vue_us", "前端ui框架.ant design vue"),
            // dict.gen.frontend.ui.type.2
            ("dict.gen.frontend.ui.type.2", "ja-JP", "ant design vue_jp", "前端ui框架.ant design vue"),
            // dict.gen.frontend.ui.type.2
            ("dict.gen.frontend.ui.type.2", "zh-CN", "ant design vue", "前端ui框架.ant design vue"),
            // dict.gen.frontend.ui.type.2
            ("dict.gen.frontend.ui.type.2", "zh-HK", "ant design vue_hk", "前端ui框架.ant design vue"),

            // dict.gen.function.type.query
            ("dict.gen.function.type.query", "en-US", "查询_us", "生成功能.查询"),
            // dict.gen.function.type.query
            ("dict.gen.function.type.query", "ja-JP", "查询_jp", "生成功能.查询"),
            // dict.gen.function.type.query
            ("dict.gen.function.type.query", "zh-CN", "查询", "生成功能.查询"),
            // dict.gen.function.type.query
            ("dict.gen.function.type.query", "zh-HK", "查询_hk", "生成功能.查询"),

            // dict.gen.function.type.create
            ("dict.gen.function.type.create", "en-US", "新增_us", "生成功能.新增"),
            // dict.gen.function.type.create
            ("dict.gen.function.type.create", "ja-JP", "新增_jp", "生成功能.新增"),
            // dict.gen.function.type.create
            ("dict.gen.function.type.create", "zh-CN", "新增", "生成功能.新增"),
            // dict.gen.function.type.create
            ("dict.gen.function.type.create", "zh-HK", "新增_hk", "生成功能.新增"),

            // dict.gen.function.type.update
            ("dict.gen.function.type.update", "en-US", "更新_us", "生成功能.更新"),
            // dict.gen.function.type.update
            ("dict.gen.function.type.update", "ja-JP", "更新_jp", "生成功能.更新"),
            // dict.gen.function.type.update
            ("dict.gen.function.type.update", "zh-CN", "更新", "生成功能.更新"),
            // dict.gen.function.type.update
            ("dict.gen.function.type.update", "zh-HK", "更新_hk", "生成功能.更新"),

            // dict.gen.function.type.delete
            ("dict.gen.function.type.delete", "en-US", "删除_us", "生成功能.删除"),
            // dict.gen.function.type.delete
            ("dict.gen.function.type.delete", "ja-JP", "删除_jp", "生成功能.删除"),
            // dict.gen.function.type.delete
            ("dict.gen.function.type.delete", "zh-CN", "删除", "生成功能.删除"),
            // dict.gen.function.type.delete
            ("dict.gen.function.type.delete", "zh-HK", "删除_hk", "生成功能.删除"),

            // dict.gen.function.type.status
            ("dict.gen.function.type.status", "en-US", "状态_us", "生成功能.状态"),
            // dict.gen.function.type.status
            ("dict.gen.function.type.status", "ja-JP", "状态_jp", "生成功能.状态"),
            // dict.gen.function.type.status
            ("dict.gen.function.type.status", "zh-CN", "状态", "生成功能.状态"),
            // dict.gen.function.type.status
            ("dict.gen.function.type.status", "zh-HK", "状态_hk", "生成功能.状态"),

            // dict.gen.function.type.sort
            ("dict.gen.function.type.sort", "en-US", "排序_us", "生成功能.排序"),
            // dict.gen.function.type.sort
            ("dict.gen.function.type.sort", "ja-JP", "排序_jp", "生成功能.排序"),
            // dict.gen.function.type.sort
            ("dict.gen.function.type.sort", "zh-CN", "排序", "生成功能.排序"),
            // dict.gen.function.type.sort
            ("dict.gen.function.type.sort", "zh-HK", "排序_hk", "生成功能.排序"),

            // dict.gen.function.type.template
            ("dict.gen.function.type.template", "en-US", "模板_us", "生成功能.模板"),
            // dict.gen.function.type.template
            ("dict.gen.function.type.template", "ja-JP", "模板_jp", "生成功能.模板"),
            // dict.gen.function.type.template
            ("dict.gen.function.type.template", "zh-CN", "模板", "生成功能.模板"),
            // dict.gen.function.type.template
            ("dict.gen.function.type.template", "zh-HK", "模板_hk", "生成功能.模板"),

            // dict.gen.function.type.import
            ("dict.gen.function.type.import", "en-US", "导入_us", "生成功能.导入"),
            // dict.gen.function.type.import
            ("dict.gen.function.type.import", "ja-JP", "导入_jp", "生成功能.导入"),
            // dict.gen.function.type.import
            ("dict.gen.function.type.import", "zh-CN", "导入", "生成功能.导入"),
            // dict.gen.function.type.import
            ("dict.gen.function.type.import", "zh-HK", "导入_hk", "生成功能.导入"),

            // dict.gen.function.type.export
            ("dict.gen.function.type.export", "en-US", "导出_us", "生成功能.导出"),
            // dict.gen.function.type.export
            ("dict.gen.function.type.export", "ja-JP", "导出_jp", "生成功能.导出"),
            // dict.gen.function.type.export
            ("dict.gen.function.type.export", "zh-CN", "导出", "生成功能.导出"),
            // dict.gen.function.type.export
            ("dict.gen.function.type.export", "zh-HK", "导出_hk", "生成功能.导出"),

            // dict.gen.method.type.0
            ("dict.gen.method.type.0", "en-US", "zip 压缩包_us", "生成方式.zip 压缩包"),
            // dict.gen.method.type.0
            ("dict.gen.method.type.0", "ja-JP", "zip 压缩包_jp", "生成方式.zip 压缩包"),
            // dict.gen.method.type.0
            ("dict.gen.method.type.0", "zh-CN", "zip 压缩包", "生成方式.zip 压缩包"),
            // dict.gen.method.type.0
            ("dict.gen.method.type.0", "zh-HK", "zip 压缩包_hk", "生成方式.zip 压缩包"),

            // dict.gen.method.type.1
            ("dict.gen.method.type.1", "en-US", "自定义路径_us", "生成方式.自定义路径"),
            // dict.gen.method.type.1
            ("dict.gen.method.type.1", "ja-JP", "自定义路径_jp", "生成方式.自定义路径"),
            // dict.gen.method.type.1
            ("dict.gen.method.type.1", "zh-CN", "自定义路径", "生成方式.自定义路径"),
            // dict.gen.method.type.1
            ("dict.gen.method.type.1", "zh-HK", "自定义路径_hk", "生成方式.自定义路径"),

            // dict.gen.method.type.2
            ("dict.gen.method.type.2", "en-US", "当前项目_us", "生成方式.当前项目"),
            // dict.gen.method.type.2
            ("dict.gen.method.type.2", "ja-JP", "当前项目_jp", "生成方式.当前项目"),
            // dict.gen.method.type.2
            ("dict.gen.method.type.2", "zh-CN", "当前项目", "生成方式.当前项目"),
            // dict.gen.method.type.2
            ("dict.gen.method.type.2", "zh-HK", "当前项目_hk", "生成方式.当前项目"),

            // dict.gen.query.type.eq
            ("dict.gen.query.type.eq", "en-US", "等于_us", "查询方式.等于"),
            // dict.gen.query.type.eq
            ("dict.gen.query.type.eq", "ja-JP", "等于_jp", "查询方式.等于"),
            // dict.gen.query.type.eq
            ("dict.gen.query.type.eq", "zh-CN", "等于", "查询方式.等于"),
            // dict.gen.query.type.eq
            ("dict.gen.query.type.eq", "zh-HK", "等于_hk", "查询方式.等于"),

            // dict.gen.query.type.ne
            ("dict.gen.query.type.ne", "en-US", "不等于_us", "查询方式.不等于"),
            // dict.gen.query.type.ne
            ("dict.gen.query.type.ne", "ja-JP", "不等于_jp", "查询方式.不等于"),
            // dict.gen.query.type.ne
            ("dict.gen.query.type.ne", "zh-CN", "不等于", "查询方式.不等于"),
            // dict.gen.query.type.ne
            ("dict.gen.query.type.ne", "zh-HK", "不等于_hk", "查询方式.不等于"),

            // dict.gen.query.type.gt
            ("dict.gen.query.type.gt", "en-US", "大于_us", "查询方式.大于"),
            // dict.gen.query.type.gt
            ("dict.gen.query.type.gt", "ja-JP", "大于_jp", "查询方式.大于"),
            // dict.gen.query.type.gt
            ("dict.gen.query.type.gt", "zh-CN", "大于", "查询方式.大于"),
            // dict.gen.query.type.gt
            ("dict.gen.query.type.gt", "zh-HK", "大于_hk", "查询方式.大于"),

            // dict.gen.query.type.gte
            ("dict.gen.query.type.gte", "en-US", "大于等于_us", "查询方式.大于等于"),
            // dict.gen.query.type.gte
            ("dict.gen.query.type.gte", "ja-JP", "大于等于_jp", "查询方式.大于等于"),
            // dict.gen.query.type.gte
            ("dict.gen.query.type.gte", "zh-CN", "大于等于", "查询方式.大于等于"),
            // dict.gen.query.type.gte
            ("dict.gen.query.type.gte", "zh-HK", "大于等于_hk", "查询方式.大于等于"),

            // dict.gen.query.type.lt
            ("dict.gen.query.type.lt", "en-US", "小于_us", "查询方式.小于"),
            // dict.gen.query.type.lt
            ("dict.gen.query.type.lt", "ja-JP", "小于_jp", "查询方式.小于"),
            // dict.gen.query.type.lt
            ("dict.gen.query.type.lt", "zh-CN", "小于", "查询方式.小于"),
            // dict.gen.query.type.lt
            ("dict.gen.query.type.lt", "zh-HK", "小于_hk", "查询方式.小于"),

            // dict.gen.query.type.lte
            ("dict.gen.query.type.lte", "en-US", "小于等于_us", "查询方式.小于等于"),
            // dict.gen.query.type.lte
            ("dict.gen.query.type.lte", "ja-JP", "小于等于_jp", "查询方式.小于等于"),
            // dict.gen.query.type.lte
            ("dict.gen.query.type.lte", "zh-CN", "小于等于", "查询方式.小于等于"),
            // dict.gen.query.type.lte
            ("dict.gen.query.type.lte", "zh-HK", "小于等于_hk", "查询方式.小于等于"),

            // dict.gen.query.type.like
            ("dict.gen.query.type.like", "en-US", "模糊_us", "查询方式.模糊"),
            // dict.gen.query.type.like
            ("dict.gen.query.type.like", "ja-JP", "模糊_jp", "查询方式.模糊"),
            // dict.gen.query.type.like
            ("dict.gen.query.type.like", "zh-CN", "模糊", "查询方式.模糊"),
            // dict.gen.query.type.like
            ("dict.gen.query.type.like", "zh-HK", "模糊_hk", "查询方式.模糊"),

            // dict.gen.query.type.between
            ("dict.gen.query.type.between", "en-US", "范围_us", "查询方式.范围"),
            // dict.gen.query.type.between
            ("dict.gen.query.type.between", "ja-JP", "范围_jp", "查询方式.范围"),
            // dict.gen.query.type.between
            ("dict.gen.query.type.between", "zh-CN", "范围", "查询方式.范围"),
            // dict.gen.query.type.between
            ("dict.gen.query.type.between", "zh-HK", "范围_hk", "查询方式.范围"),

            // dict.gen.template.type.crud
            ("dict.gen.template.type.crud", "en-US", "单表操作_us", "生成模板类型.单表操作"),
            // dict.gen.template.type.crud
            ("dict.gen.template.type.crud", "ja-JP", "单表操作_jp", "生成模板类型.单表操作"),
            // dict.gen.template.type.crud
            ("dict.gen.template.type.crud", "zh-CN", "单表操作", "生成模板类型.单表操作"),
            // dict.gen.template.type.crud
            ("dict.gen.template.type.crud", "zh-HK", "单表操作_hk", "生成模板类型.单表操作"),

            // dict.gen.template.type.tree
            ("dict.gen.template.type.tree", "en-US", "树表操作_us", "生成模板类型.树表操作"),
            // dict.gen.template.type.tree
            ("dict.gen.template.type.tree", "ja-JP", "树表操作_jp", "生成模板类型.树表操作"),
            // dict.gen.template.type.tree
            ("dict.gen.template.type.tree", "zh-CN", "树表操作", "生成模板类型.树表操作"),
            // dict.gen.template.type.tree
            ("dict.gen.template.type.tree", "zh-HK", "树表操作_hk", "生成模板类型.树表操作"),

            // dict.gen.template.type.sub
            ("dict.gen.template.type.sub", "en-US", "主子表操作_us", "生成模板类型.主子表操作"),
            // dict.gen.template.type.sub
            ("dict.gen.template.type.sub", "ja-JP", "主子表操作_jp", "生成模板类型.主子表操作"),
            // dict.gen.template.type.sub
            ("dict.gen.template.type.sub", "zh-CN", "主子表操作", "生成模板类型.主子表操作"),
            // dict.gen.template.type.sub
            ("dict.gen.template.type.sub", "zh-HK", "主子表操作_hk", "生成模板类型.主子表操作"),

            // dict.hr.attendance.correction.type.1
            ("dict.hr.attendance.correction.type.1", "en-US", "上班_us", "补卡类型.上班"),
            // dict.hr.attendance.correction.type.1
            ("dict.hr.attendance.correction.type.1", "ja-JP", "上班_jp", "补卡类型.上班"),
            // dict.hr.attendance.correction.type.1
            ("dict.hr.attendance.correction.type.1", "zh-CN", "上班", "补卡类型.上班"),
            // dict.hr.attendance.correction.type.1
            ("dict.hr.attendance.correction.type.1", "zh-HK", "上班_hk", "补卡类型.上班"),

            // dict.hr.attendance.correction.type.2
            ("dict.hr.attendance.correction.type.2", "en-US", "下班_us", "补卡类型.下班"),
            // dict.hr.attendance.correction.type.2
            ("dict.hr.attendance.correction.type.2", "ja-JP", "下班_jp", "补卡类型.下班"),
            // dict.hr.attendance.correction.type.2
            ("dict.hr.attendance.correction.type.2", "zh-CN", "下班", "补卡类型.下班"),
            // dict.hr.attendance.correction.type.2
            ("dict.hr.attendance.correction.type.2", "zh-HK", "下班_hk", "补卡类型.下班"),

            // dict.hr.attendance.device.brand.category.hikvision
            ("dict.hr.attendance.device.brand.category.hikvision", "en-US", "海康威视_us", "考勤设备品牌.海康威视"),
            // dict.hr.attendance.device.brand.category.hikvision
            ("dict.hr.attendance.device.brand.category.hikvision", "ja-JP", "海康威视_jp", "考勤设备品牌.海康威视"),
            // dict.hr.attendance.device.brand.category.hikvision
            ("dict.hr.attendance.device.brand.category.hikvision", "zh-CN", "海康威视", "考勤设备品牌.海康威视"),
            // dict.hr.attendance.device.brand.category.hikvision
            ("dict.hr.attendance.device.brand.category.hikvision", "zh-HK", "海康威视_hk", "考勤设备品牌.海康威视"),

            // dict.hr.attendance.device.brand.category.deli
            ("dict.hr.attendance.device.brand.category.deli", "en-US", "得力_us", "考勤设备品牌.得力"),
            // dict.hr.attendance.device.brand.category.deli
            ("dict.hr.attendance.device.brand.category.deli", "ja-JP", "得力_jp", "考勤设备品牌.得力"),
            // dict.hr.attendance.device.brand.category.deli
            ("dict.hr.attendance.device.brand.category.deli", "zh-CN", "得力", "考勤设备品牌.得力"),
            // dict.hr.attendance.device.brand.category.deli
            ("dict.hr.attendance.device.brand.category.deli", "zh-HK", "得力_hk", "考勤设备品牌.得力"),

            // dict.hr.attendance.device.brand.category.zkteco
            ("dict.hr.attendance.device.brand.category.zkteco", "en-US", "中控_us", "考勤设备品牌.中控"),
            // dict.hr.attendance.device.brand.category.zkteco
            ("dict.hr.attendance.device.brand.category.zkteco", "ja-JP", "中控_jp", "考勤设备品牌.中控"),
            // dict.hr.attendance.device.brand.category.zkteco
            ("dict.hr.attendance.device.brand.category.zkteco", "zh-CN", "中控", "考勤设备品牌.中控"),
            // dict.hr.attendance.device.brand.category.zkteco
            ("dict.hr.attendance.device.brand.category.zkteco", "zh-HK", "中控_hk", "考勤设备品牌.中控"),

            // dict.hr.attendance.exception.handle.status.0
            ("dict.hr.attendance.exception.handle.status.0", "en-US", "待处理_us", "考勤异常处理状态.待处理"),
            // dict.hr.attendance.exception.handle.status.0
            ("dict.hr.attendance.exception.handle.status.0", "ja-JP", "待处理_jp", "考勤异常处理状态.待处理"),
            // dict.hr.attendance.exception.handle.status.0
            ("dict.hr.attendance.exception.handle.status.0", "zh-CN", "待处理", "考勤异常处理状态.待处理"),
            // dict.hr.attendance.exception.handle.status.0
            ("dict.hr.attendance.exception.handle.status.0", "zh-HK", "待处理_hk", "考勤异常处理状态.待处理"),

            // dict.hr.attendance.exception.handle.status.1
            ("dict.hr.attendance.exception.handle.status.1", "en-US", "已处理_us", "考勤异常处理状态.已处理"),
            // dict.hr.attendance.exception.handle.status.1
            ("dict.hr.attendance.exception.handle.status.1", "ja-JP", "已处理_jp", "考勤异常处理状态.已处理"),
            // dict.hr.attendance.exception.handle.status.1
            ("dict.hr.attendance.exception.handle.status.1", "zh-CN", "已处理", "考勤异常处理状态.已处理"),
            // dict.hr.attendance.exception.handle.status.1
            ("dict.hr.attendance.exception.handle.status.1", "zh-HK", "已处理_hk", "考勤异常处理状态.已处理"),

            // dict.hr.attendance.exception.handle.status.2
            ("dict.hr.attendance.exception.handle.status.2", "en-US", "已忽略_us", "考勤异常处理状态.已忽略"),
            // dict.hr.attendance.exception.handle.status.2
            ("dict.hr.attendance.exception.handle.status.2", "ja-JP", "已忽略_jp", "考勤异常处理状态.已忽略"),
            // dict.hr.attendance.exception.handle.status.2
            ("dict.hr.attendance.exception.handle.status.2", "zh-CN", "已忽略", "考勤异常处理状态.已忽略"),
            // dict.hr.attendance.exception.handle.status.2
            ("dict.hr.attendance.exception.handle.status.2", "zh-HK", "已忽略_hk", "考勤异常处理状态.已忽略"),

            // dict.hr.attendance.exception.type.1
            ("dict.hr.attendance.exception.type.1", "en-US", "上班缺卡_us", "考勤异常类型.上班缺卡"),
            // dict.hr.attendance.exception.type.1
            ("dict.hr.attendance.exception.type.1", "ja-JP", "上班缺卡_jp", "考勤异常类型.上班缺卡"),
            // dict.hr.attendance.exception.type.1
            ("dict.hr.attendance.exception.type.1", "zh-CN", "上班缺卡", "考勤异常类型.上班缺卡"),
            // dict.hr.attendance.exception.type.1
            ("dict.hr.attendance.exception.type.1", "zh-HK", "上班缺卡_hk", "考勤异常类型.上班缺卡"),

            // dict.hr.attendance.exception.type.2
            ("dict.hr.attendance.exception.type.2", "en-US", "下班缺卡_us", "考勤异常类型.下班缺卡"),
            // dict.hr.attendance.exception.type.2
            ("dict.hr.attendance.exception.type.2", "ja-JP", "下班缺卡_jp", "考勤异常类型.下班缺卡"),
            // dict.hr.attendance.exception.type.2
            ("dict.hr.attendance.exception.type.2", "zh-CN", "下班缺卡", "考勤异常类型.下班缺卡"),
            // dict.hr.attendance.exception.type.2
            ("dict.hr.attendance.exception.type.2", "zh-HK", "下班缺卡_hk", "考勤异常类型.下班缺卡"),

            // dict.hr.attendance.exception.type.3
            ("dict.hr.attendance.exception.type.3", "en-US", "迟到_us", "考勤异常类型.迟到"),
            // dict.hr.attendance.exception.type.3
            ("dict.hr.attendance.exception.type.3", "ja-JP", "迟到_jp", "考勤异常类型.迟到"),
            // dict.hr.attendance.exception.type.3
            ("dict.hr.attendance.exception.type.3", "zh-CN", "迟到", "考勤异常类型.迟到"),
            // dict.hr.attendance.exception.type.3
            ("dict.hr.attendance.exception.type.3", "zh-HK", "迟到_hk", "考勤异常类型.迟到"),

            // dict.hr.attendance.exception.type.4
            ("dict.hr.attendance.exception.type.4", "en-US", "早退_us", "考勤异常类型.早退"),
            // dict.hr.attendance.exception.type.4
            ("dict.hr.attendance.exception.type.4", "ja-JP", "早退_jp", "考勤异常类型.早退"),
            // dict.hr.attendance.exception.type.4
            ("dict.hr.attendance.exception.type.4", "zh-CN", "早退", "考勤异常类型.早退"),
            // dict.hr.attendance.exception.type.4
            ("dict.hr.attendance.exception.type.4", "zh-HK", "早退_hk", "考勤异常类型.早退"),

            // dict.hr.attendance.exception.type.5
            ("dict.hr.attendance.exception.type.5", "en-US", "旷工_us", "考勤异常类型.旷工"),
            // dict.hr.attendance.exception.type.5
            ("dict.hr.attendance.exception.type.5", "ja-JP", "旷工_jp", "考勤异常类型.旷工"),
            // dict.hr.attendance.exception.type.5
            ("dict.hr.attendance.exception.type.5", "zh-CN", "旷工", "考勤异常类型.旷工"),
            // dict.hr.attendance.exception.type.5
            ("dict.hr.attendance.exception.type.5", "zh-HK", "旷工_hk", "考勤异常类型.旷工"),

            // dict.hr.attendance.exception.type.9
            ("dict.hr.attendance.exception.type.9", "en-US", "其他_us", "考勤异常类型.其他"),
            // dict.hr.attendance.exception.type.9
            ("dict.hr.attendance.exception.type.9", "ja-JP", "其他_jp", "考勤异常类型.其他"),
            // dict.hr.attendance.exception.type.9
            ("dict.hr.attendance.exception.type.9", "zh-CN", "其他", "考勤异常类型.其他"),
            // dict.hr.attendance.exception.type.9
            ("dict.hr.attendance.exception.type.9", "zh-HK", "其他_hk", "考勤异常类型.其他"),

            // dict.hr.attendance.punch.source.type.0
            ("dict.hr.attendance.punch.source.type.0", "en-US", "后台录入_us", "打卡来源.后台录入"),
            // dict.hr.attendance.punch.source.type.0
            ("dict.hr.attendance.punch.source.type.0", "ja-JP", "后台录入_jp", "打卡来源.后台录入"),
            // dict.hr.attendance.punch.source.type.0
            ("dict.hr.attendance.punch.source.type.0", "zh-CN", "后台录入", "打卡来源.后台录入"),
            // dict.hr.attendance.punch.source.type.0
            ("dict.hr.attendance.punch.source.type.0", "zh-HK", "后台录入_hk", "打卡来源.后台录入"),

            // dict.hr.attendance.punch.source.type.1
            ("dict.hr.attendance.punch.source.type.1", "en-US", "移动端_us", "打卡来源.移动端"),
            // dict.hr.attendance.punch.source.type.1
            ("dict.hr.attendance.punch.source.type.1", "ja-JP", "移动端_jp", "打卡来源.移动端"),
            // dict.hr.attendance.punch.source.type.1
            ("dict.hr.attendance.punch.source.type.1", "zh-CN", "移动端", "打卡来源.移动端"),
            // dict.hr.attendance.punch.source.type.1
            ("dict.hr.attendance.punch.source.type.1", "zh-HK", "移动端_hk", "打卡来源.移动端"),

            // dict.hr.attendance.punch.source.type.2
            ("dict.hr.attendance.punch.source.type.2", "en-US", "导入_us", "打卡来源.导入"),
            // dict.hr.attendance.punch.source.type.2
            ("dict.hr.attendance.punch.source.type.2", "ja-JP", "导入_jp", "打卡来源.导入"),
            // dict.hr.attendance.punch.source.type.2
            ("dict.hr.attendance.punch.source.type.2", "zh-CN", "导入", "打卡来源.导入"),
            // dict.hr.attendance.punch.source.type.2
            ("dict.hr.attendance.punch.source.type.2", "zh-HK", "导入_hk", "打卡来源.导入"),

            // dict.hr.attendance.punch.type.1
            ("dict.hr.attendance.punch.type.1", "en-US", "上班_us", "打卡类型.上班"),
            // dict.hr.attendance.punch.type.1
            ("dict.hr.attendance.punch.type.1", "ja-JP", "上班_jp", "打卡类型.上班"),
            // dict.hr.attendance.punch.type.1
            ("dict.hr.attendance.punch.type.1", "zh-CN", "上班", "打卡类型.上班"),
            // dict.hr.attendance.punch.type.1
            ("dict.hr.attendance.punch.type.1", "zh-HK", "上班_hk", "打卡类型.上班"),

            // dict.hr.attendance.punch.type.2
            ("dict.hr.attendance.punch.type.2", "en-US", "下班_us", "打卡类型.下班"),
            // dict.hr.attendance.punch.type.2
            ("dict.hr.attendance.punch.type.2", "ja-JP", "下班_jp", "打卡类型.下班"),
            // dict.hr.attendance.punch.type.2
            ("dict.hr.attendance.punch.type.2", "zh-CN", "下班", "打卡类型.下班"),
            // dict.hr.attendance.punch.type.2
            ("dict.hr.attendance.punch.type.2", "zh-HK", "下班_hk", "打卡类型.下班"),

            // dict.hr.attendance.punch.type.3
            ("dict.hr.attendance.punch.type.3", "en-US", "外勤_us", "打卡类型.外勤"),
            // dict.hr.attendance.punch.type.3
            ("dict.hr.attendance.punch.type.3", "ja-JP", "外勤_jp", "打卡类型.外勤"),
            // dict.hr.attendance.punch.type.3
            ("dict.hr.attendance.punch.type.3", "zh-CN", "外勤", "打卡类型.外勤"),
            // dict.hr.attendance.punch.type.3
            ("dict.hr.attendance.punch.type.3", "zh-HK", "外勤_hk", "打卡类型.外勤"),

            // dict.hr.attendance.result.status.0
            ("dict.hr.attendance.result.status.0", "en-US", "正常_us", "出勤状态.正常"),
            // dict.hr.attendance.result.status.0
            ("dict.hr.attendance.result.status.0", "ja-JP", "正常_jp", "出勤状态.正常"),
            // dict.hr.attendance.result.status.0
            ("dict.hr.attendance.result.status.0", "zh-CN", "正常", "出勤状态.正常"),
            // dict.hr.attendance.result.status.0
            ("dict.hr.attendance.result.status.0", "zh-HK", "正常_hk", "出勤状态.正常"),

            // dict.hr.attendance.result.status.1
            ("dict.hr.attendance.result.status.1", "en-US", "迟到_us", "出勤状态.迟到"),
            // dict.hr.attendance.result.status.1
            ("dict.hr.attendance.result.status.1", "ja-JP", "迟到_jp", "出勤状态.迟到"),
            // dict.hr.attendance.result.status.1
            ("dict.hr.attendance.result.status.1", "zh-CN", "迟到", "出勤状态.迟到"),
            // dict.hr.attendance.result.status.1
            ("dict.hr.attendance.result.status.1", "zh-HK", "迟到_hk", "出勤状态.迟到"),

            // dict.hr.attendance.result.status.2
            ("dict.hr.attendance.result.status.2", "en-US", "早退_us", "出勤状态.早退"),
            // dict.hr.attendance.result.status.2
            ("dict.hr.attendance.result.status.2", "ja-JP", "早退_jp", "出勤状态.早退"),
            // dict.hr.attendance.result.status.2
            ("dict.hr.attendance.result.status.2", "zh-CN", "早退", "出勤状态.早退"),
            // dict.hr.attendance.result.status.2
            ("dict.hr.attendance.result.status.2", "zh-HK", "早退_hk", "出勤状态.早退"),

            // dict.hr.attendance.result.status.3
            ("dict.hr.attendance.result.status.3", "en-US", "缺卡_us", "出勤状态.缺卡"),
            // dict.hr.attendance.result.status.3
            ("dict.hr.attendance.result.status.3", "ja-JP", "缺卡_jp", "出勤状态.缺卡"),
            // dict.hr.attendance.result.status.3
            ("dict.hr.attendance.result.status.3", "zh-CN", "缺卡", "出勤状态.缺卡"),
            // dict.hr.attendance.result.status.3
            ("dict.hr.attendance.result.status.3", "zh-HK", "缺卡_hk", "出勤状态.缺卡"),

            // dict.hr.attendance.result.status.4
            ("dict.hr.attendance.result.status.4", "en-US", "旷工_us", "出勤状态.旷工"),
            // dict.hr.attendance.result.status.4
            ("dict.hr.attendance.result.status.4", "ja-JP", "旷工_jp", "出勤状态.旷工"),
            // dict.hr.attendance.result.status.4
            ("dict.hr.attendance.result.status.4", "zh-CN", "旷工", "出勤状态.旷工"),
            // dict.hr.attendance.result.status.4
            ("dict.hr.attendance.result.status.4", "zh-HK", "旷工_hk", "出勤状态.旷工"),

            // dict.hr.attendance.result.status.5
            ("dict.hr.attendance.result.status.5", "en-US", "加班_us", "出勤状态.加班"),
            // dict.hr.attendance.result.status.5
            ("dict.hr.attendance.result.status.5", "ja-JP", "加班_jp", "出勤状态.加班"),
            // dict.hr.attendance.result.status.5
            ("dict.hr.attendance.result.status.5", "zh-CN", "加班", "出勤状态.加班"),
            // dict.hr.attendance.result.status.5
            ("dict.hr.attendance.result.status.5", "zh-HK", "加班_hk", "出勤状态.加班"),

            // dict.hr.attendance.verify.type.0
            ("dict.hr.attendance.verify.type.0", "en-US", "未知_us", "考勤验证方式.未知"),
            // dict.hr.attendance.verify.type.0
            ("dict.hr.attendance.verify.type.0", "ja-JP", "未知_jp", "考勤验证方式.未知"),
            // dict.hr.attendance.verify.type.0
            ("dict.hr.attendance.verify.type.0", "zh-CN", "未知", "考勤验证方式.未知"),
            // dict.hr.attendance.verify.type.0
            ("dict.hr.attendance.verify.type.0", "zh-HK", "未知_hk", "考勤验证方式.未知"),

            // dict.hr.attendance.verify.type.1
            ("dict.hr.attendance.verify.type.1", "en-US", "指纹_us", "考勤验证方式.指纹"),
            // dict.hr.attendance.verify.type.1
            ("dict.hr.attendance.verify.type.1", "ja-JP", "指纹_jp", "考勤验证方式.指纹"),
            // dict.hr.attendance.verify.type.1
            ("dict.hr.attendance.verify.type.1", "zh-CN", "指纹", "考勤验证方式.指纹"),
            // dict.hr.attendance.verify.type.1
            ("dict.hr.attendance.verify.type.1", "zh-HK", "指纹_hk", "考勤验证方式.指纹"),

            // dict.hr.attendance.verify.type.2
            ("dict.hr.attendance.verify.type.2", "en-US", "人脸_us", "考勤验证方式.人脸"),
            // dict.hr.attendance.verify.type.2
            ("dict.hr.attendance.verify.type.2", "ja-JP", "人脸_jp", "考勤验证方式.人脸"),
            // dict.hr.attendance.verify.type.2
            ("dict.hr.attendance.verify.type.2", "zh-CN", "人脸", "考勤验证方式.人脸"),
            // dict.hr.attendance.verify.type.2
            ("dict.hr.attendance.verify.type.2", "zh-HK", "人脸_hk", "考勤验证方式.人脸"),

            // dict.hr.attendance.verify.type.3
            ("dict.hr.attendance.verify.type.3", "en-US", "密码_us", "考勤验证方式.密码"),
            // dict.hr.attendance.verify.type.3
            ("dict.hr.attendance.verify.type.3", "ja-JP", "密码_jp", "考勤验证方式.密码"),
            // dict.hr.attendance.verify.type.3
            ("dict.hr.attendance.verify.type.3", "zh-CN", "密码", "考勤验证方式.密码"),
            // dict.hr.attendance.verify.type.3
            ("dict.hr.attendance.verify.type.3", "zh-HK", "密码_hk", "考勤验证方式.密码"),

            // dict.hr.attendance.verify.type.4
            ("dict.hr.attendance.verify.type.4", "en-US", "卡_us", "考勤验证方式.卡"),
            // dict.hr.attendance.verify.type.4
            ("dict.hr.attendance.verify.type.4", "ja-JP", "卡_jp", "考勤验证方式.卡"),
            // dict.hr.attendance.verify.type.4
            ("dict.hr.attendance.verify.type.4", "zh-CN", "卡", "考勤验证方式.卡"),
            // dict.hr.attendance.verify.type.4
            ("dict.hr.attendance.verify.type.4", "zh-HK", "卡_hk", "考勤验证方式.卡"),

            // dict.hr.delegate.type.0
            ("dict.hr.delegate.type.0", "en-US", "直接员工_us", "人事代理模式.直接员工"),
            // dict.hr.delegate.type.0
            ("dict.hr.delegate.type.0", "ja-JP", "直接员工_jp", "人事代理模式.直接员工"),
            // dict.hr.delegate.type.0
            ("dict.hr.delegate.type.0", "zh-CN", "直接员工", "人事代理模式.直接员工"),
            // dict.hr.delegate.type.0
            ("dict.hr.delegate.type.0", "zh-HK", "直接员工_hk", "人事代理模式.直接员工"),

            // dict.hr.delegate.type.1
            ("dict.hr.delegate.type.1", "en-US", "部门规则_us", "人事代理模式.部门规则"),
            // dict.hr.delegate.type.1
            ("dict.hr.delegate.type.1", "ja-JP", "部门规则_jp", "人事代理模式.部门规则"),
            // dict.hr.delegate.type.1
            ("dict.hr.delegate.type.1", "zh-CN", "部门规则", "人事代理模式.部门规则"),
            // dict.hr.delegate.type.1
            ("dict.hr.delegate.type.1", "zh-HK", "部门规则_hk", "人事代理模式.部门规则"),

            // dict.hr.delegate.type.2
            ("dict.hr.delegate.type.2", "en-US", "岗位规则_us", "人事代理模式.岗位规则"),
            // dict.hr.delegate.type.2
            ("dict.hr.delegate.type.2", "ja-JP", "岗位规则_jp", "人事代理模式.岗位规则"),
            // dict.hr.delegate.type.2
            ("dict.hr.delegate.type.2", "zh-CN", "岗位规则", "人事代理模式.岗位规则"),
            // dict.hr.delegate.type.2
            ("dict.hr.delegate.type.2", "zh-HK", "岗位规则_hk", "人事代理模式.岗位规则"),

            // dict.hr.employee.status.1
            ("dict.hr.employee.status.1", "en-US", "试用期_us", "员工状态.试用期"),
            // dict.hr.employee.status.1
            ("dict.hr.employee.status.1", "ja-JP", "试用期_jp", "员工状态.试用期"),
            // dict.hr.employee.status.1
            ("dict.hr.employee.status.1", "zh-CN", "试用期", "员工状态.试用期"),
            // dict.hr.employee.status.1
            ("dict.hr.employee.status.1", "zh-HK", "试用期_hk", "员工状态.试用期"),

            // dict.hr.employee.status.2
            ("dict.hr.employee.status.2", "en-US", "正式_us", "员工状态.正式"),
            // dict.hr.employee.status.2
            ("dict.hr.employee.status.2", "ja-JP", "正式_jp", "员工状态.正式"),
            // dict.hr.employee.status.2
            ("dict.hr.employee.status.2", "zh-CN", "正式", "员工状态.正式"),
            // dict.hr.employee.status.2
            ("dict.hr.employee.status.2", "zh-HK", "正式_hk", "员工状态.正式"),

            // dict.hr.employee.status.3
            ("dict.hr.employee.status.3", "en-US", "离职_us", "员工状态.离职"),
            // dict.hr.employee.status.3
            ("dict.hr.employee.status.3", "ja-JP", "离职_jp", "员工状态.离职"),
            // dict.hr.employee.status.3
            ("dict.hr.employee.status.3", "zh-CN", "离职", "员工状态.离职"),
            // dict.hr.employee.status.3
            ("dict.hr.employee.status.3", "zh-HK", "离职_hk", "员工状态.离职"),

            // dict.hr.employee.status.4
            ("dict.hr.employee.status.4", "en-US", "退休_us", "员工状态.退休"),
            // dict.hr.employee.status.4
            ("dict.hr.employee.status.4", "ja-JP", "退休_jp", "员工状态.退休"),
            // dict.hr.employee.status.4
            ("dict.hr.employee.status.4", "zh-CN", "退休", "员工状态.退休"),
            // dict.hr.employee.status.4
            ("dict.hr.employee.status.4", "zh-HK", "退休_hk", "员工状态.退休"),

            // dict.hr.education.level.category.1
            ("dict.hr.education.level.category.1", "en-US", "高中及以下_us", "学历.高中及以下"),
            // dict.hr.education.level.category.1
            ("dict.hr.education.level.category.1", "ja-JP", "高中及以下_jp", "学历.高中及以下"),
            // dict.hr.education.level.category.1
            ("dict.hr.education.level.category.1", "zh-CN", "高中及以下", "学历.高中及以下"),
            // dict.hr.education.level.category.1
            ("dict.hr.education.level.category.1", "zh-HK", "高中及以下_hk", "学历.高中及以下"),

            // dict.hr.education.level.category.2
            ("dict.hr.education.level.category.2", "en-US", "大专_us", "学历.大专"),
            // dict.hr.education.level.category.2
            ("dict.hr.education.level.category.2", "ja-JP", "大专_jp", "学历.大专"),
            // dict.hr.education.level.category.2
            ("dict.hr.education.level.category.2", "zh-CN", "大专", "学历.大专"),
            // dict.hr.education.level.category.2
            ("dict.hr.education.level.category.2", "zh-HK", "大专_hk", "学历.大专"),

            // dict.hr.education.level.category.3
            ("dict.hr.education.level.category.3", "en-US", "本科_us", "学历.本科"),
            // dict.hr.education.level.category.3
            ("dict.hr.education.level.category.3", "ja-JP", "本科_jp", "学历.本科"),
            // dict.hr.education.level.category.3
            ("dict.hr.education.level.category.3", "zh-CN", "本科", "学历.本科"),
            // dict.hr.education.level.category.3
            ("dict.hr.education.level.category.3", "zh-HK", "本科_hk", "学历.本科"),

            // dict.hr.education.level.category.4
            ("dict.hr.education.level.category.4", "en-US", "硕士_us", "学历.硕士"),
            // dict.hr.education.level.category.4
            ("dict.hr.education.level.category.4", "ja-JP", "硕士_jp", "学历.硕士"),
            // dict.hr.education.level.category.4
            ("dict.hr.education.level.category.4", "zh-CN", "硕士", "学历.硕士"),
            // dict.hr.education.level.category.4
            ("dict.hr.education.level.category.4", "zh-HK", "硕士_hk", "学历.硕士"),

            // dict.hr.education.level.category.5
            ("dict.hr.education.level.category.5", "en-US", "博士_us", "学历.博士"),
            // dict.hr.education.level.category.5
            ("dict.hr.education.level.category.5", "ja-JP", "博士_jp", "学历.博士"),
            // dict.hr.education.level.category.5
            ("dict.hr.education.level.category.5", "zh-CN", "博士", "学历.博士"),
            // dict.hr.education.level.category.5
            ("dict.hr.education.level.category.5", "zh-HK", "博士_hk", "学历.博士"),

            // dict.hr.ethnic.code.1
            ("dict.hr.ethnic.code.1", "en-US", "汉族_us", "民族.汉族"),
            // dict.hr.ethnic.code.1
            ("dict.hr.ethnic.code.1", "ja-JP", "汉族_jp", "民族.汉族"),
            // dict.hr.ethnic.code.1
            ("dict.hr.ethnic.code.1", "zh-CN", "汉族", "民族.汉族"),
            // dict.hr.ethnic.code.1
            ("dict.hr.ethnic.code.1", "zh-HK", "汉族_hk", "民族.汉族"),

            // dict.hr.ethnic.code.2
            ("dict.hr.ethnic.code.2", "en-US", "蒙古族_us", "民族.蒙古族"),
            // dict.hr.ethnic.code.2
            ("dict.hr.ethnic.code.2", "ja-JP", "蒙古族_jp", "民族.蒙古族"),
            // dict.hr.ethnic.code.2
            ("dict.hr.ethnic.code.2", "zh-CN", "蒙古族", "民族.蒙古族"),
            // dict.hr.ethnic.code.2
            ("dict.hr.ethnic.code.2", "zh-HK", "蒙古族_hk", "民族.蒙古族"),

            // dict.hr.ethnic.code.3
            ("dict.hr.ethnic.code.3", "en-US", "回族_us", "民族.回族"),
            // dict.hr.ethnic.code.3
            ("dict.hr.ethnic.code.3", "ja-JP", "回族_jp", "民族.回族"),
            // dict.hr.ethnic.code.3
            ("dict.hr.ethnic.code.3", "zh-CN", "回族", "民族.回族"),
            // dict.hr.ethnic.code.3
            ("dict.hr.ethnic.code.3", "zh-HK", "回族_hk", "民族.回族"),

            // dict.hr.ethnic.code.4
            ("dict.hr.ethnic.code.4", "en-US", "藏族_us", "民族.藏族"),
            // dict.hr.ethnic.code.4
            ("dict.hr.ethnic.code.4", "ja-JP", "藏族_jp", "民族.藏族"),
            // dict.hr.ethnic.code.4
            ("dict.hr.ethnic.code.4", "zh-CN", "藏族", "民族.藏族"),
            // dict.hr.ethnic.code.4
            ("dict.hr.ethnic.code.4", "zh-HK", "藏族_hk", "民族.藏族"),

            // dict.hr.ethnic.code.5
            ("dict.hr.ethnic.code.5", "en-US", "维吾尔族_us", "民族.维吾尔族"),
            // dict.hr.ethnic.code.5
            ("dict.hr.ethnic.code.5", "ja-JP", "维吾尔族_jp", "民族.维吾尔族"),
            // dict.hr.ethnic.code.5
            ("dict.hr.ethnic.code.5", "zh-CN", "维吾尔族", "民族.维吾尔族"),
            // dict.hr.ethnic.code.5
            ("dict.hr.ethnic.code.5", "zh-HK", "维吾尔族_hk", "民族.维吾尔族"),

            // dict.hr.ethnic.code.6
            ("dict.hr.ethnic.code.6", "en-US", "苗族_us", "民族.苗族"),
            // dict.hr.ethnic.code.6
            ("dict.hr.ethnic.code.6", "ja-JP", "苗族_jp", "民族.苗族"),
            // dict.hr.ethnic.code.6
            ("dict.hr.ethnic.code.6", "zh-CN", "苗族", "民族.苗族"),
            // dict.hr.ethnic.code.6
            ("dict.hr.ethnic.code.6", "zh-HK", "苗族_hk", "民族.苗族"),

            // dict.hr.ethnic.code.7
            ("dict.hr.ethnic.code.7", "en-US", "彝族_us", "民族.彝族"),
            // dict.hr.ethnic.code.7
            ("dict.hr.ethnic.code.7", "ja-JP", "彝族_jp", "民族.彝族"),
            // dict.hr.ethnic.code.7
            ("dict.hr.ethnic.code.7", "zh-CN", "彝族", "民族.彝族"),
            // dict.hr.ethnic.code.7
            ("dict.hr.ethnic.code.7", "zh-HK", "彝族_hk", "民族.彝族"),

            // dict.hr.ethnic.code.8
            ("dict.hr.ethnic.code.8", "en-US", "壮族_us", "民族.壮族"),
            // dict.hr.ethnic.code.8
            ("dict.hr.ethnic.code.8", "ja-JP", "壮族_jp", "民族.壮族"),
            // dict.hr.ethnic.code.8
            ("dict.hr.ethnic.code.8", "zh-CN", "壮族", "民族.壮族"),
            // dict.hr.ethnic.code.8
            ("dict.hr.ethnic.code.8", "zh-HK", "壮族_hk", "民族.壮族"),

            // dict.hr.ethnic.code.9
            ("dict.hr.ethnic.code.9", "en-US", "布依族_us", "民族.布依族"),
            // dict.hr.ethnic.code.9
            ("dict.hr.ethnic.code.9", "ja-JP", "布依族_jp", "民族.布依族"),
            // dict.hr.ethnic.code.9
            ("dict.hr.ethnic.code.9", "zh-CN", "布依族", "民族.布依族"),
            // dict.hr.ethnic.code.9
            ("dict.hr.ethnic.code.9", "zh-HK", "布依族_hk", "民族.布依族"),

            // dict.hr.ethnic.code.10
            ("dict.hr.ethnic.code.10", "en-US", "朝鲜族_us", "民族.朝鲜族"),
            // dict.hr.ethnic.code.10
            ("dict.hr.ethnic.code.10", "ja-JP", "朝鲜族_jp", "民族.朝鲜族"),
            // dict.hr.ethnic.code.10
            ("dict.hr.ethnic.code.10", "zh-CN", "朝鲜族", "民族.朝鲜族"),
            // dict.hr.ethnic.code.10
            ("dict.hr.ethnic.code.10", "zh-HK", "朝鲜族_hk", "民族.朝鲜族"),

            // dict.hr.ethnic.code.11
            ("dict.hr.ethnic.code.11", "en-US", "满族_us", "民族.满族"),
            // dict.hr.ethnic.code.11
            ("dict.hr.ethnic.code.11", "ja-JP", "满族_jp", "民族.满族"),
            // dict.hr.ethnic.code.11
            ("dict.hr.ethnic.code.11", "zh-CN", "满族", "民族.满族"),
            // dict.hr.ethnic.code.11
            ("dict.hr.ethnic.code.11", "zh-HK", "满族_hk", "民族.满族"),

            // dict.hr.ethnic.code.12
            ("dict.hr.ethnic.code.12", "en-US", "侗族_us", "民族.侗族"),
            // dict.hr.ethnic.code.12
            ("dict.hr.ethnic.code.12", "ja-JP", "侗族_jp", "民族.侗族"),
            // dict.hr.ethnic.code.12
            ("dict.hr.ethnic.code.12", "zh-CN", "侗族", "民族.侗族"),
            // dict.hr.ethnic.code.12
            ("dict.hr.ethnic.code.12", "zh-HK", "侗族_hk", "民族.侗族"),

            // dict.hr.ethnic.code.13
            ("dict.hr.ethnic.code.13", "en-US", "瑶族_us", "民族.瑶族"),
            // dict.hr.ethnic.code.13
            ("dict.hr.ethnic.code.13", "ja-JP", "瑶族_jp", "民族.瑶族"),
            // dict.hr.ethnic.code.13
            ("dict.hr.ethnic.code.13", "zh-CN", "瑶族", "民族.瑶族"),
            // dict.hr.ethnic.code.13
            ("dict.hr.ethnic.code.13", "zh-HK", "瑶族_hk", "民族.瑶族"),

            // dict.hr.ethnic.code.14
            ("dict.hr.ethnic.code.14", "en-US", "白族_us", "民族.白族"),
            // dict.hr.ethnic.code.14
            ("dict.hr.ethnic.code.14", "ja-JP", "白族_jp", "民族.白族"),
            // dict.hr.ethnic.code.14
            ("dict.hr.ethnic.code.14", "zh-CN", "白族", "民族.白族"),
            // dict.hr.ethnic.code.14
            ("dict.hr.ethnic.code.14", "zh-HK", "白族_hk", "民族.白族"),

            // dict.hr.ethnic.code.15
            ("dict.hr.ethnic.code.15", "en-US", "土家族_us", "民族.土家族"),
            // dict.hr.ethnic.code.15
            ("dict.hr.ethnic.code.15", "ja-JP", "土家族_jp", "民族.土家族"),
            // dict.hr.ethnic.code.15
            ("dict.hr.ethnic.code.15", "zh-CN", "土家族", "民族.土家族"),
            // dict.hr.ethnic.code.15
            ("dict.hr.ethnic.code.15", "zh-HK", "土家族_hk", "民族.土家族"),

            // dict.hr.ethnic.code.16
            ("dict.hr.ethnic.code.16", "en-US", "哈尼族_us", "民族.哈尼族"),
            // dict.hr.ethnic.code.16
            ("dict.hr.ethnic.code.16", "ja-JP", "哈尼族_jp", "民族.哈尼族"),
            // dict.hr.ethnic.code.16
            ("dict.hr.ethnic.code.16", "zh-CN", "哈尼族", "民族.哈尼族"),
            // dict.hr.ethnic.code.16
            ("dict.hr.ethnic.code.16", "zh-HK", "哈尼族_hk", "民族.哈尼族"),

            // dict.hr.ethnic.code.17
            ("dict.hr.ethnic.code.17", "en-US", "哈萨克族_us", "民族.哈萨克族"),
            // dict.hr.ethnic.code.17
            ("dict.hr.ethnic.code.17", "ja-JP", "哈萨克族_jp", "民族.哈萨克族"),
            // dict.hr.ethnic.code.17
            ("dict.hr.ethnic.code.17", "zh-CN", "哈萨克族", "民族.哈萨克族"),
            // dict.hr.ethnic.code.17
            ("dict.hr.ethnic.code.17", "zh-HK", "哈萨克族_hk", "民族.哈萨克族"),

            // dict.hr.ethnic.code.18
            ("dict.hr.ethnic.code.18", "en-US", "傣族_us", "民族.傣族"),
            // dict.hr.ethnic.code.18
            ("dict.hr.ethnic.code.18", "ja-JP", "傣族_jp", "民族.傣族"),
            // dict.hr.ethnic.code.18
            ("dict.hr.ethnic.code.18", "zh-CN", "傣族", "民族.傣族"),
            // dict.hr.ethnic.code.18
            ("dict.hr.ethnic.code.18", "zh-HK", "傣族_hk", "民族.傣族"),

            // dict.hr.ethnic.code.19
            ("dict.hr.ethnic.code.19", "en-US", "黎族_us", "民族.黎族"),
            // dict.hr.ethnic.code.19
            ("dict.hr.ethnic.code.19", "ja-JP", "黎族_jp", "民族.黎族"),
            // dict.hr.ethnic.code.19
            ("dict.hr.ethnic.code.19", "zh-CN", "黎族", "民族.黎族"),
            // dict.hr.ethnic.code.19
            ("dict.hr.ethnic.code.19", "zh-HK", "黎族_hk", "民族.黎族"),

            // dict.hr.ethnic.code.20
            ("dict.hr.ethnic.code.20", "en-US", "傈僳族_us", "民族.傈僳族"),
            // dict.hr.ethnic.code.20
            ("dict.hr.ethnic.code.20", "ja-JP", "傈僳族_jp", "民族.傈僳族"),
            // dict.hr.ethnic.code.20
            ("dict.hr.ethnic.code.20", "zh-CN", "傈僳族", "民族.傈僳族"),
            // dict.hr.ethnic.code.20
            ("dict.hr.ethnic.code.20", "zh-HK", "傈僳族_hk", "民族.傈僳族"),

            // dict.hr.ethnic.code.21
            ("dict.hr.ethnic.code.21", "en-US", "佤族_us", "民族.佤族"),
            // dict.hr.ethnic.code.21
            ("dict.hr.ethnic.code.21", "ja-JP", "佤族_jp", "民族.佤族"),
            // dict.hr.ethnic.code.21
            ("dict.hr.ethnic.code.21", "zh-CN", "佤族", "民族.佤族"),
            // dict.hr.ethnic.code.21
            ("dict.hr.ethnic.code.21", "zh-HK", "佤族_hk", "民族.佤族"),

            // dict.hr.ethnic.code.22
            ("dict.hr.ethnic.code.22", "en-US", "畲族_us", "民族.畲族"),
            // dict.hr.ethnic.code.22
            ("dict.hr.ethnic.code.22", "ja-JP", "畲族_jp", "民族.畲族"),
            // dict.hr.ethnic.code.22
            ("dict.hr.ethnic.code.22", "zh-CN", "畲族", "民族.畲族"),
            // dict.hr.ethnic.code.22
            ("dict.hr.ethnic.code.22", "zh-HK", "畲族_hk", "民族.畲族"),

            // dict.hr.ethnic.code.23
            ("dict.hr.ethnic.code.23", "en-US", "高山族_us", "民族.高山族"),
            // dict.hr.ethnic.code.23
            ("dict.hr.ethnic.code.23", "ja-JP", "高山族_jp", "民族.高山族"),
            // dict.hr.ethnic.code.23
            ("dict.hr.ethnic.code.23", "zh-CN", "高山族", "民族.高山族"),
            // dict.hr.ethnic.code.23
            ("dict.hr.ethnic.code.23", "zh-HK", "高山族_hk", "民族.高山族"),

            // dict.hr.ethnic.code.24
            ("dict.hr.ethnic.code.24", "en-US", "拉祜族_us", "民族.拉祜族"),
            // dict.hr.ethnic.code.24
            ("dict.hr.ethnic.code.24", "ja-JP", "拉祜族_jp", "民族.拉祜族"),
            // dict.hr.ethnic.code.24
            ("dict.hr.ethnic.code.24", "zh-CN", "拉祜族", "民族.拉祜族"),
            // dict.hr.ethnic.code.24
            ("dict.hr.ethnic.code.24", "zh-HK", "拉祜族_hk", "民族.拉祜族"),

            // dict.hr.ethnic.code.25
            ("dict.hr.ethnic.code.25", "en-US", "水族_us", "民族.水族"),
            // dict.hr.ethnic.code.25
            ("dict.hr.ethnic.code.25", "ja-JP", "水族_jp", "民族.水族"),
            // dict.hr.ethnic.code.25
            ("dict.hr.ethnic.code.25", "zh-CN", "水族", "民族.水族"),
            // dict.hr.ethnic.code.25
            ("dict.hr.ethnic.code.25", "zh-HK", "水族_hk", "民族.水族"),

            // dict.hr.ethnic.code.26
            ("dict.hr.ethnic.code.26", "en-US", "东乡族_us", "民族.东乡族"),
            // dict.hr.ethnic.code.26
            ("dict.hr.ethnic.code.26", "ja-JP", "东乡族_jp", "民族.东乡族"),
            // dict.hr.ethnic.code.26
            ("dict.hr.ethnic.code.26", "zh-CN", "东乡族", "民族.东乡族"),
            // dict.hr.ethnic.code.26
            ("dict.hr.ethnic.code.26", "zh-HK", "东乡族_hk", "民族.东乡族"),

            // dict.hr.ethnic.code.27
            ("dict.hr.ethnic.code.27", "en-US", "纳西族_us", "民族.纳西族"),
            // dict.hr.ethnic.code.27
            ("dict.hr.ethnic.code.27", "ja-JP", "纳西族_jp", "民族.纳西族"),
            // dict.hr.ethnic.code.27
            ("dict.hr.ethnic.code.27", "zh-CN", "纳西族", "民族.纳西族"),
            // dict.hr.ethnic.code.27
            ("dict.hr.ethnic.code.27", "zh-HK", "纳西族_hk", "民族.纳西族"),

            // dict.hr.ethnic.code.28
            ("dict.hr.ethnic.code.28", "en-US", "景颇族_us", "民族.景颇族"),
            // dict.hr.ethnic.code.28
            ("dict.hr.ethnic.code.28", "ja-JP", "景颇族_jp", "民族.景颇族"),
            // dict.hr.ethnic.code.28
            ("dict.hr.ethnic.code.28", "zh-CN", "景颇族", "民族.景颇族"),
            // dict.hr.ethnic.code.28
            ("dict.hr.ethnic.code.28", "zh-HK", "景颇族_hk", "民族.景颇族"),

            // dict.hr.ethnic.code.29
            ("dict.hr.ethnic.code.29", "en-US", "柯尔克孜族_us", "民族.柯尔克孜族"),
            // dict.hr.ethnic.code.29
            ("dict.hr.ethnic.code.29", "ja-JP", "柯尔克孜族_jp", "民族.柯尔克孜族"),
            // dict.hr.ethnic.code.29
            ("dict.hr.ethnic.code.29", "zh-CN", "柯尔克孜族", "民族.柯尔克孜族"),
            // dict.hr.ethnic.code.29
            ("dict.hr.ethnic.code.29", "zh-HK", "柯尔克孜族_hk", "民族.柯尔克孜族"),

            // dict.hr.ethnic.code.30
            ("dict.hr.ethnic.code.30", "en-US", "土族_us", "民族.土族"),
            // dict.hr.ethnic.code.30
            ("dict.hr.ethnic.code.30", "ja-JP", "土族_jp", "民族.土族"),
            // dict.hr.ethnic.code.30
            ("dict.hr.ethnic.code.30", "zh-CN", "土族", "民族.土族"),
            // dict.hr.ethnic.code.30
            ("dict.hr.ethnic.code.30", "zh-HK", "土族_hk", "民族.土族"),

            // dict.hr.ethnic.code.31
            ("dict.hr.ethnic.code.31", "en-US", "达斡尔族_us", "民族.达斡尔族"),
            // dict.hr.ethnic.code.31
            ("dict.hr.ethnic.code.31", "ja-JP", "达斡尔族_jp", "民族.达斡尔族"),
            // dict.hr.ethnic.code.31
            ("dict.hr.ethnic.code.31", "zh-CN", "达斡尔族", "民族.达斡尔族"),
            // dict.hr.ethnic.code.31
            ("dict.hr.ethnic.code.31", "zh-HK", "达斡尔族_hk", "民族.达斡尔族"),

            // dict.hr.ethnic.code.32
            ("dict.hr.ethnic.code.32", "en-US", "仫佬族_us", "民族.仫佬族"),
            // dict.hr.ethnic.code.32
            ("dict.hr.ethnic.code.32", "ja-JP", "仫佬族_jp", "民族.仫佬族"),
            // dict.hr.ethnic.code.32
            ("dict.hr.ethnic.code.32", "zh-CN", "仫佬族", "民族.仫佬族"),
            // dict.hr.ethnic.code.32
            ("dict.hr.ethnic.code.32", "zh-HK", "仫佬族_hk", "民族.仫佬族"),

            // dict.hr.ethnic.code.33
            ("dict.hr.ethnic.code.33", "en-US", "羌族_us", "民族.羌族"),
            // dict.hr.ethnic.code.33
            ("dict.hr.ethnic.code.33", "ja-JP", "羌族_jp", "民族.羌族"),
            // dict.hr.ethnic.code.33
            ("dict.hr.ethnic.code.33", "zh-CN", "羌族", "民族.羌族"),
            // dict.hr.ethnic.code.33
            ("dict.hr.ethnic.code.33", "zh-HK", "羌族_hk", "民族.羌族"),

            // dict.hr.ethnic.code.34
            ("dict.hr.ethnic.code.34", "en-US", "布朗族_us", "民族.布朗族"),
            // dict.hr.ethnic.code.34
            ("dict.hr.ethnic.code.34", "ja-JP", "布朗族_jp", "民族.布朗族"),
            // dict.hr.ethnic.code.34
            ("dict.hr.ethnic.code.34", "zh-CN", "布朗族", "民族.布朗族"),
            // dict.hr.ethnic.code.34
            ("dict.hr.ethnic.code.34", "zh-HK", "布朗族_hk", "民族.布朗族"),

            // dict.hr.ethnic.code.35
            ("dict.hr.ethnic.code.35", "en-US", "撒拉族_us", "民族.撒拉族"),
            // dict.hr.ethnic.code.35
            ("dict.hr.ethnic.code.35", "ja-JP", "撒拉族_jp", "民族.撒拉族"),
            // dict.hr.ethnic.code.35
            ("dict.hr.ethnic.code.35", "zh-CN", "撒拉族", "民族.撒拉族"),
            // dict.hr.ethnic.code.35
            ("dict.hr.ethnic.code.35", "zh-HK", "撒拉族_hk", "民族.撒拉族"),

            // dict.hr.ethnic.code.36
            ("dict.hr.ethnic.code.36", "en-US", "毛南族_us", "民族.毛南族"),
            // dict.hr.ethnic.code.36
            ("dict.hr.ethnic.code.36", "ja-JP", "毛南族_jp", "民族.毛南族"),
            // dict.hr.ethnic.code.36
            ("dict.hr.ethnic.code.36", "zh-CN", "毛南族", "民族.毛南族"),
            // dict.hr.ethnic.code.36
            ("dict.hr.ethnic.code.36", "zh-HK", "毛南族_hk", "民族.毛南族"),

            // dict.hr.ethnic.code.37
            ("dict.hr.ethnic.code.37", "en-US", "仡佬族_us", "民族.仡佬族"),
            // dict.hr.ethnic.code.37
            ("dict.hr.ethnic.code.37", "ja-JP", "仡佬族_jp", "民族.仡佬族"),
            // dict.hr.ethnic.code.37
            ("dict.hr.ethnic.code.37", "zh-CN", "仡佬族", "民族.仡佬族"),
            // dict.hr.ethnic.code.37
            ("dict.hr.ethnic.code.37", "zh-HK", "仡佬族_hk", "民族.仡佬族"),

            // dict.hr.ethnic.code.38
            ("dict.hr.ethnic.code.38", "en-US", "锡伯族_us", "民族.锡伯族"),
            // dict.hr.ethnic.code.38
            ("dict.hr.ethnic.code.38", "ja-JP", "锡伯族_jp", "民族.锡伯族"),
            // dict.hr.ethnic.code.38
            ("dict.hr.ethnic.code.38", "zh-CN", "锡伯族", "民族.锡伯族"),
            // dict.hr.ethnic.code.38
            ("dict.hr.ethnic.code.38", "zh-HK", "锡伯族_hk", "民族.锡伯族"),

            // dict.hr.ethnic.code.39
            ("dict.hr.ethnic.code.39", "en-US", "阿昌族_us", "民族.阿昌族"),
            // dict.hr.ethnic.code.39
            ("dict.hr.ethnic.code.39", "ja-JP", "阿昌族_jp", "民族.阿昌族"),
            // dict.hr.ethnic.code.39
            ("dict.hr.ethnic.code.39", "zh-CN", "阿昌族", "民族.阿昌族"),
            // dict.hr.ethnic.code.39
            ("dict.hr.ethnic.code.39", "zh-HK", "阿昌族_hk", "民族.阿昌族"),

            // dict.hr.ethnic.code.40
            ("dict.hr.ethnic.code.40", "en-US", "普米族_us", "民族.普米族"),
            // dict.hr.ethnic.code.40
            ("dict.hr.ethnic.code.40", "ja-JP", "普米族_jp", "民族.普米族"),
            // dict.hr.ethnic.code.40
            ("dict.hr.ethnic.code.40", "zh-CN", "普米族", "民族.普米族"),
            // dict.hr.ethnic.code.40
            ("dict.hr.ethnic.code.40", "zh-HK", "普米族_hk", "民族.普米族"),

            // dict.hr.ethnic.code.41
            ("dict.hr.ethnic.code.41", "en-US", "塔吉克族_us", "民族.塔吉克族"),
            // dict.hr.ethnic.code.41
            ("dict.hr.ethnic.code.41", "ja-JP", "塔吉克族_jp", "民族.塔吉克族"),
            // dict.hr.ethnic.code.41
            ("dict.hr.ethnic.code.41", "zh-CN", "塔吉克族", "民族.塔吉克族"),
            // dict.hr.ethnic.code.41
            ("dict.hr.ethnic.code.41", "zh-HK", "塔吉克族_hk", "民族.塔吉克族"),

            // dict.hr.ethnic.code.42
            ("dict.hr.ethnic.code.42", "en-US", "怒族_us", "民族.怒族"),
            // dict.hr.ethnic.code.42
            ("dict.hr.ethnic.code.42", "ja-JP", "怒族_jp", "民族.怒族"),
            // dict.hr.ethnic.code.42
            ("dict.hr.ethnic.code.42", "zh-CN", "怒族", "民族.怒族"),
            // dict.hr.ethnic.code.42
            ("dict.hr.ethnic.code.42", "zh-HK", "怒族_hk", "民族.怒族"),

            // dict.hr.ethnic.code.43
            ("dict.hr.ethnic.code.43", "en-US", "乌孜别克族_us", "民族.乌孜别克族"),
            // dict.hr.ethnic.code.43
            ("dict.hr.ethnic.code.43", "ja-JP", "乌孜别克族_jp", "民族.乌孜别克族"),
            // dict.hr.ethnic.code.43
            ("dict.hr.ethnic.code.43", "zh-CN", "乌孜别克族", "民族.乌孜别克族"),
            // dict.hr.ethnic.code.43
            ("dict.hr.ethnic.code.43", "zh-HK", "乌孜别克族_hk", "民族.乌孜别克族"),

            // dict.hr.ethnic.code.44
            ("dict.hr.ethnic.code.44", "en-US", "俄罗斯族_us", "民族.俄罗斯族"),
            // dict.hr.ethnic.code.44
            ("dict.hr.ethnic.code.44", "ja-JP", "俄罗斯族_jp", "民族.俄罗斯族"),
            // dict.hr.ethnic.code.44
            ("dict.hr.ethnic.code.44", "zh-CN", "俄罗斯族", "民族.俄罗斯族"),
            // dict.hr.ethnic.code.44
            ("dict.hr.ethnic.code.44", "zh-HK", "俄罗斯族_hk", "民族.俄罗斯族"),

            // dict.hr.ethnic.code.45
            ("dict.hr.ethnic.code.45", "en-US", "鄂温克族_us", "民族.鄂温克族"),
            // dict.hr.ethnic.code.45
            ("dict.hr.ethnic.code.45", "ja-JP", "鄂温克族_jp", "民族.鄂温克族"),
            // dict.hr.ethnic.code.45
            ("dict.hr.ethnic.code.45", "zh-CN", "鄂温克族", "民族.鄂温克族"),
            // dict.hr.ethnic.code.45
            ("dict.hr.ethnic.code.45", "zh-HK", "鄂温克族_hk", "民族.鄂温克族"),

            // dict.hr.ethnic.code.46
            ("dict.hr.ethnic.code.46", "en-US", "德昂族_us", "民族.德昂族"),
            // dict.hr.ethnic.code.46
            ("dict.hr.ethnic.code.46", "ja-JP", "德昂族_jp", "民族.德昂族"),
            // dict.hr.ethnic.code.46
            ("dict.hr.ethnic.code.46", "zh-CN", "德昂族", "民族.德昂族"),
            // dict.hr.ethnic.code.46
            ("dict.hr.ethnic.code.46", "zh-HK", "德昂族_hk", "民族.德昂族"),

            // dict.hr.ethnic.code.47
            ("dict.hr.ethnic.code.47", "en-US", "保安族_us", "民族.保安族"),
            // dict.hr.ethnic.code.47
            ("dict.hr.ethnic.code.47", "ja-JP", "保安族_jp", "民族.保安族"),
            // dict.hr.ethnic.code.47
            ("dict.hr.ethnic.code.47", "zh-CN", "保安族", "民族.保安族"),
            // dict.hr.ethnic.code.47
            ("dict.hr.ethnic.code.47", "zh-HK", "保安族_hk", "民族.保安族"),

            // dict.hr.ethnic.code.48
            ("dict.hr.ethnic.code.48", "en-US", "裕固族_us", "民族.裕固族"),
            // dict.hr.ethnic.code.48
            ("dict.hr.ethnic.code.48", "ja-JP", "裕固族_jp", "民族.裕固族"),
            // dict.hr.ethnic.code.48
            ("dict.hr.ethnic.code.48", "zh-CN", "裕固族", "民族.裕固族"),
            // dict.hr.ethnic.code.48
            ("dict.hr.ethnic.code.48", "zh-HK", "裕固族_hk", "民族.裕固族"),

            // dict.hr.ethnic.code.49
            ("dict.hr.ethnic.code.49", "en-US", "京族_us", "民族.京族"),
            // dict.hr.ethnic.code.49
            ("dict.hr.ethnic.code.49", "ja-JP", "京族_jp", "民族.京族"),
            // dict.hr.ethnic.code.49
            ("dict.hr.ethnic.code.49", "zh-CN", "京族", "民族.京族"),
            // dict.hr.ethnic.code.49
            ("dict.hr.ethnic.code.49", "zh-HK", "京族_hk", "民族.京族"),

            // dict.hr.ethnic.code.50
            ("dict.hr.ethnic.code.50", "en-US", "塔塔尔族_us", "民族.塔塔尔族"),
            // dict.hr.ethnic.code.50
            ("dict.hr.ethnic.code.50", "ja-JP", "塔塔尔族_jp", "民族.塔塔尔族"),
            // dict.hr.ethnic.code.50
            ("dict.hr.ethnic.code.50", "zh-CN", "塔塔尔族", "民族.塔塔尔族"),
            // dict.hr.ethnic.code.50
            ("dict.hr.ethnic.code.50", "zh-HK", "塔塔尔族_hk", "民族.塔塔尔族"),

            // dict.hr.ethnic.code.51
            ("dict.hr.ethnic.code.51", "en-US", "独龙族_us", "民族.独龙族"),
            // dict.hr.ethnic.code.51
            ("dict.hr.ethnic.code.51", "ja-JP", "独龙族_jp", "民族.独龙族"),
            // dict.hr.ethnic.code.51
            ("dict.hr.ethnic.code.51", "zh-CN", "独龙族", "民族.独龙族"),
            // dict.hr.ethnic.code.51
            ("dict.hr.ethnic.code.51", "zh-HK", "独龙族_hk", "民族.独龙族"),

            // dict.hr.ethnic.code.52
            ("dict.hr.ethnic.code.52", "en-US", "鄂伦春族_us", "民族.鄂伦春族"),
            // dict.hr.ethnic.code.52
            ("dict.hr.ethnic.code.52", "ja-JP", "鄂伦春族_jp", "民族.鄂伦春族"),
            // dict.hr.ethnic.code.52
            ("dict.hr.ethnic.code.52", "zh-CN", "鄂伦春族", "民族.鄂伦春族"),
            // dict.hr.ethnic.code.52
            ("dict.hr.ethnic.code.52", "zh-HK", "鄂伦春族_hk", "民族.鄂伦春族"),

            // dict.hr.ethnic.code.53
            ("dict.hr.ethnic.code.53", "en-US", "赫哲族_us", "民族.赫哲族"),
            // dict.hr.ethnic.code.53
            ("dict.hr.ethnic.code.53", "ja-JP", "赫哲族_jp", "民族.赫哲族"),
            // dict.hr.ethnic.code.53
            ("dict.hr.ethnic.code.53", "zh-CN", "赫哲族", "民族.赫哲族"),
            // dict.hr.ethnic.code.53
            ("dict.hr.ethnic.code.53", "zh-HK", "赫哲族_hk", "民族.赫哲族"),

            // dict.hr.ethnic.code.54
            ("dict.hr.ethnic.code.54", "en-US", "门巴族_us", "民族.门巴族"),
            // dict.hr.ethnic.code.54
            ("dict.hr.ethnic.code.54", "ja-JP", "门巴族_jp", "民族.门巴族"),
            // dict.hr.ethnic.code.54
            ("dict.hr.ethnic.code.54", "zh-CN", "门巴族", "民族.门巴族"),
            // dict.hr.ethnic.code.54
            ("dict.hr.ethnic.code.54", "zh-HK", "门巴族_hk", "民族.门巴族"),

            // dict.hr.ethnic.code.55
            ("dict.hr.ethnic.code.55", "en-US", "珞巴族_us", "民族.珞巴族"),
            // dict.hr.ethnic.code.55
            ("dict.hr.ethnic.code.55", "ja-JP", "珞巴族_jp", "民族.珞巴族"),
            // dict.hr.ethnic.code.55
            ("dict.hr.ethnic.code.55", "zh-CN", "珞巴族", "民族.珞巴族"),
            // dict.hr.ethnic.code.55
            ("dict.hr.ethnic.code.55", "zh-HK", "珞巴族_hk", "民族.珞巴族"),

            // dict.hr.ethnic.code.56
            ("dict.hr.ethnic.code.56", "en-US", "基诺族_us", "民族.基诺族"),
            // dict.hr.ethnic.code.56
            ("dict.hr.ethnic.code.56", "ja-JP", "基诺族_jp", "民族.基诺族"),
            // dict.hr.ethnic.code.56
            ("dict.hr.ethnic.code.56", "zh-CN", "基诺族", "民族.基诺族"),
            // dict.hr.ethnic.code.56
            ("dict.hr.ethnic.code.56", "zh-HK", "基诺族_hk", "民族.基诺族"),

            // dict.hr.holiday.working.day.type.0
            ("dict.hr.holiday.working.day.type.0", "en-US", "非工作日_us", "假日是否工作日.非工作日"),
            // dict.hr.holiday.working.day.type.0
            ("dict.hr.holiday.working.day.type.0", "ja-JP", "非工作日_jp", "假日是否工作日.非工作日"),
            // dict.hr.holiday.working.day.type.0
            ("dict.hr.holiday.working.day.type.0", "zh-CN", "非工作日", "假日是否工作日.非工作日"),
            // dict.hr.holiday.working.day.type.0
            ("dict.hr.holiday.working.day.type.0", "zh-HK", "非工作日_hk", "假日是否工作日.非工作日"),

            // dict.hr.holiday.working.day.type.1
            ("dict.hr.holiday.working.day.type.1", "en-US", "工作日_us", "假日是否工作日.工作日"),
            // dict.hr.holiday.working.day.type.1
            ("dict.hr.holiday.working.day.type.1", "ja-JP", "工作日_jp", "假日是否工作日.工作日"),
            // dict.hr.holiday.working.day.type.1
            ("dict.hr.holiday.working.day.type.1", "zh-CN", "工作日", "假日是否工作日.工作日"),
            // dict.hr.holiday.working.day.type.1
            ("dict.hr.holiday.working.day.type.1", "zh-HK", "工作日_hk", "假日是否工作日.工作日"),

            // dict.hr.holiday.working.day.type.2
            ("dict.hr.holiday.working.day.type.2", "en-US", "半天等_us", "假日是否工作日.半天等"),
            // dict.hr.holiday.working.day.type.2
            ("dict.hr.holiday.working.day.type.2", "ja-JP", "半天等_jp", "假日是否工作日.半天等"),
            // dict.hr.holiday.working.day.type.2
            ("dict.hr.holiday.working.day.type.2", "zh-CN", "半天等", "假日是否工作日.半天等"),
            // dict.hr.holiday.working.day.type.2
            ("dict.hr.holiday.working.day.type.2", "zh-HK", "半天等_hk", "假日是否工作日.半天等"),

            // dict.hr.holiday.category.0
            ("dict.hr.holiday.category.0", "en-US", "法定_us", "假日类型.法定"),
            // dict.hr.holiday.category.0
            ("dict.hr.holiday.category.0", "ja-JP", "法定_jp", "假日类型.法定"),
            // dict.hr.holiday.category.0
            ("dict.hr.holiday.category.0", "zh-CN", "法定", "假日类型.法定"),
            // dict.hr.holiday.category.0
            ("dict.hr.holiday.category.0", "zh-HK", "法定_hk", "假日类型.法定"),

            // dict.hr.holiday.category.1
            ("dict.hr.holiday.category.1", "en-US", "调休_us", "假日类型.调休"),
            // dict.hr.holiday.category.1
            ("dict.hr.holiday.category.1", "ja-JP", "调休_jp", "假日类型.调休"),
            // dict.hr.holiday.category.1
            ("dict.hr.holiday.category.1", "zh-CN", "调休", "假日类型.调休"),
            // dict.hr.holiday.category.1
            ("dict.hr.holiday.category.1", "zh-HK", "调休_hk", "假日类型.调休"),

            // dict.hr.holiday.category.2
            ("dict.hr.holiday.category.2", "en-US", "公司_us", "假日类型.公司"),
            // dict.hr.holiday.category.2
            ("dict.hr.holiday.category.2", "ja-JP", "公司_jp", "假日类型.公司"),
            // dict.hr.holiday.category.2
            ("dict.hr.holiday.category.2", "zh-CN", "公司", "假日类型.公司"),
            // dict.hr.holiday.category.2
            ("dict.hr.holiday.category.2", "zh-HK", "公司_hk", "假日类型.公司"),

            // dict.hr.marital.status.0
            ("dict.hr.marital.status.0", "en-US", "未婚_us", "婚姻状况.未婚"),
            // dict.hr.marital.status.0
            ("dict.hr.marital.status.0", "ja-JP", "未婚_jp", "婚姻状况.未婚"),
            // dict.hr.marital.status.0
            ("dict.hr.marital.status.0", "zh-CN", "未婚", "婚姻状况.未婚"),
            // dict.hr.marital.status.0
            ("dict.hr.marital.status.0", "zh-HK", "未婚_hk", "婚姻状况.未婚"),

            // dict.hr.marital.status.1
            ("dict.hr.marital.status.1", "en-US", "已婚_us", "婚姻状况.已婚"),
            // dict.hr.marital.status.1
            ("dict.hr.marital.status.1", "ja-JP", "已婚_jp", "婚姻状况.已婚"),
            // dict.hr.marital.status.1
            ("dict.hr.marital.status.1", "zh-CN", "已婚", "婚姻状况.已婚"),
            // dict.hr.marital.status.1
            ("dict.hr.marital.status.1", "zh-HK", "已婚_hk", "婚姻状况.已婚"),

            // dict.hr.marital.status.2
            ("dict.hr.marital.status.2", "en-US", "离异_us", "婚姻状况.离异"),
            // dict.hr.marital.status.2
            ("dict.hr.marital.status.2", "ja-JP", "离异_jp", "婚姻状况.离异"),
            // dict.hr.marital.status.2
            ("dict.hr.marital.status.2", "zh-CN", "离异", "婚姻状况.离异"),
            // dict.hr.marital.status.2
            ("dict.hr.marital.status.2", "zh-HK", "离异_hk", "婚姻状况.离异"),

            // dict.hr.marital.status.3
            ("dict.hr.marital.status.3", "en-US", "丧偶_us", "婚姻状况.丧偶"),
            // dict.hr.marital.status.3
            ("dict.hr.marital.status.3", "ja-JP", "丧偶_jp", "婚姻状况.丧偶"),
            // dict.hr.marital.status.3
            ("dict.hr.marital.status.3", "zh-CN", "丧偶", "婚姻状况.丧偶"),
            // dict.hr.marital.status.3
            ("dict.hr.marital.status.3", "zh-HK", "丧偶_hk", "婚姻状况.丧偶"),

            // dict.hr.native.place.code.110000
            ("dict.hr.native.place.code.110000", "en-US", "北京市_us", "籍贯.北京市"),
            // dict.hr.native.place.code.110000
            ("dict.hr.native.place.code.110000", "ja-JP", "北京市_jp", "籍贯.北京市"),
            // dict.hr.native.place.code.110000
            ("dict.hr.native.place.code.110000", "zh-CN", "北京市", "籍贯.北京市"),
            // dict.hr.native.place.code.110000
            ("dict.hr.native.place.code.110000", "zh-HK", "北京市_hk", "籍贯.北京市"),

            // dict.hr.native.place.code.120000
            ("dict.hr.native.place.code.120000", "en-US", "天津市_us", "籍贯.天津市"),
            // dict.hr.native.place.code.120000
            ("dict.hr.native.place.code.120000", "ja-JP", "天津市_jp", "籍贯.天津市"),
            // dict.hr.native.place.code.120000
            ("dict.hr.native.place.code.120000", "zh-CN", "天津市", "籍贯.天津市"),
            // dict.hr.native.place.code.120000
            ("dict.hr.native.place.code.120000", "zh-HK", "天津市_hk", "籍贯.天津市"),

            // dict.hr.native.place.code.130000
            ("dict.hr.native.place.code.130000", "en-US", "河北省_us", "籍贯.河北省"),
            // dict.hr.native.place.code.130000
            ("dict.hr.native.place.code.130000", "ja-JP", "河北省_jp", "籍贯.河北省"),
            // dict.hr.native.place.code.130000
            ("dict.hr.native.place.code.130000", "zh-CN", "河北省", "籍贯.河北省"),
            // dict.hr.native.place.code.130000
            ("dict.hr.native.place.code.130000", "zh-HK", "河北省_hk", "籍贯.河北省"),

            // dict.hr.native.place.code.140000
            ("dict.hr.native.place.code.140000", "en-US", "山西省_us", "籍贯.山西省"),
            // dict.hr.native.place.code.140000
            ("dict.hr.native.place.code.140000", "ja-JP", "山西省_jp", "籍贯.山西省"),
            // dict.hr.native.place.code.140000
            ("dict.hr.native.place.code.140000", "zh-CN", "山西省", "籍贯.山西省"),
            // dict.hr.native.place.code.140000
            ("dict.hr.native.place.code.140000", "zh-HK", "山西省_hk", "籍贯.山西省"),

            // dict.hr.native.place.code.150000
            ("dict.hr.native.place.code.150000", "en-US", "内蒙古自治区_us", "籍贯.内蒙古自治区"),
            // dict.hr.native.place.code.150000
            ("dict.hr.native.place.code.150000", "ja-JP", "内蒙古自治区_jp", "籍贯.内蒙古自治区"),
            // dict.hr.native.place.code.150000
            ("dict.hr.native.place.code.150000", "zh-CN", "内蒙古自治区", "籍贯.内蒙古自治区"),
            // dict.hr.native.place.code.150000
            ("dict.hr.native.place.code.150000", "zh-HK", "内蒙古自治区_hk", "籍贯.内蒙古自治区"),

            // dict.hr.native.place.code.210000
            ("dict.hr.native.place.code.210000", "en-US", "辽宁省_us", "籍贯.辽宁省"),
            // dict.hr.native.place.code.210000
            ("dict.hr.native.place.code.210000", "ja-JP", "辽宁省_jp", "籍贯.辽宁省"),
            // dict.hr.native.place.code.210000
            ("dict.hr.native.place.code.210000", "zh-CN", "辽宁省", "籍贯.辽宁省"),
            // dict.hr.native.place.code.210000
            ("dict.hr.native.place.code.210000", "zh-HK", "辽宁省_hk", "籍贯.辽宁省"),

            // dict.hr.native.place.code.220000
            ("dict.hr.native.place.code.220000", "en-US", "吉林省_us", "籍贯.吉林省"),
            // dict.hr.native.place.code.220000
            ("dict.hr.native.place.code.220000", "ja-JP", "吉林省_jp", "籍贯.吉林省"),
            // dict.hr.native.place.code.220000
            ("dict.hr.native.place.code.220000", "zh-CN", "吉林省", "籍贯.吉林省"),
            // dict.hr.native.place.code.220000
            ("dict.hr.native.place.code.220000", "zh-HK", "吉林省_hk", "籍贯.吉林省"),

            // dict.hr.native.place.code.230000
            ("dict.hr.native.place.code.230000", "en-US", "黑龙江省_us", "籍贯.黑龙江省"),
            // dict.hr.native.place.code.230000
            ("dict.hr.native.place.code.230000", "ja-JP", "黑龙江省_jp", "籍贯.黑龙江省"),
            // dict.hr.native.place.code.230000
            ("dict.hr.native.place.code.230000", "zh-CN", "黑龙江省", "籍贯.黑龙江省"),
            // dict.hr.native.place.code.230000
            ("dict.hr.native.place.code.230000", "zh-HK", "黑龙江省_hk", "籍贯.黑龙江省"),

            // dict.hr.native.place.code.310000
            ("dict.hr.native.place.code.310000", "en-US", "上海市_us", "籍贯.上海市"),
            // dict.hr.native.place.code.310000
            ("dict.hr.native.place.code.310000", "ja-JP", "上海市_jp", "籍贯.上海市"),
            // dict.hr.native.place.code.310000
            ("dict.hr.native.place.code.310000", "zh-CN", "上海市", "籍贯.上海市"),
            // dict.hr.native.place.code.310000
            ("dict.hr.native.place.code.310000", "zh-HK", "上海市_hk", "籍贯.上海市"),

            // dict.hr.native.place.code.320000
            ("dict.hr.native.place.code.320000", "en-US", "江苏省_us", "籍贯.江苏省"),
            // dict.hr.native.place.code.320000
            ("dict.hr.native.place.code.320000", "ja-JP", "江苏省_jp", "籍贯.江苏省"),
            // dict.hr.native.place.code.320000
            ("dict.hr.native.place.code.320000", "zh-CN", "江苏省", "籍贯.江苏省"),
            // dict.hr.native.place.code.320000
            ("dict.hr.native.place.code.320000", "zh-HK", "江苏省_hk", "籍贯.江苏省"),

            // dict.hr.native.place.code.330000
            ("dict.hr.native.place.code.330000", "en-US", "浙江省_us", "籍贯.浙江省"),
            // dict.hr.native.place.code.330000
            ("dict.hr.native.place.code.330000", "ja-JP", "浙江省_jp", "籍贯.浙江省"),
            // dict.hr.native.place.code.330000
            ("dict.hr.native.place.code.330000", "zh-CN", "浙江省", "籍贯.浙江省"),
            // dict.hr.native.place.code.330000
            ("dict.hr.native.place.code.330000", "zh-HK", "浙江省_hk", "籍贯.浙江省"),

            // dict.hr.native.place.code.340000
            ("dict.hr.native.place.code.340000", "en-US", "安徽省_us", "籍贯.安徽省"),
            // dict.hr.native.place.code.340000
            ("dict.hr.native.place.code.340000", "ja-JP", "安徽省_jp", "籍贯.安徽省"),
            // dict.hr.native.place.code.340000
            ("dict.hr.native.place.code.340000", "zh-CN", "安徽省", "籍贯.安徽省"),
            // dict.hr.native.place.code.340000
            ("dict.hr.native.place.code.340000", "zh-HK", "安徽省_hk", "籍贯.安徽省"),

            // dict.hr.native.place.code.350000
            ("dict.hr.native.place.code.350000", "en-US", "福建省_us", "籍贯.福建省"),
            // dict.hr.native.place.code.350000
            ("dict.hr.native.place.code.350000", "ja-JP", "福建省_jp", "籍贯.福建省"),
            // dict.hr.native.place.code.350000
            ("dict.hr.native.place.code.350000", "zh-CN", "福建省", "籍贯.福建省"),
            // dict.hr.native.place.code.350000
            ("dict.hr.native.place.code.350000", "zh-HK", "福建省_hk", "籍贯.福建省"),

            // dict.hr.native.place.code.360000
            ("dict.hr.native.place.code.360000", "en-US", "江西省_us", "籍贯.江西省"),
            // dict.hr.native.place.code.360000
            ("dict.hr.native.place.code.360000", "ja-JP", "江西省_jp", "籍贯.江西省"),
            // dict.hr.native.place.code.360000
            ("dict.hr.native.place.code.360000", "zh-CN", "江西省", "籍贯.江西省"),
            // dict.hr.native.place.code.360000
            ("dict.hr.native.place.code.360000", "zh-HK", "江西省_hk", "籍贯.江西省"),

            // dict.hr.native.place.code.370000
            ("dict.hr.native.place.code.370000", "en-US", "山东省_us", "籍贯.山东省"),
            // dict.hr.native.place.code.370000
            ("dict.hr.native.place.code.370000", "ja-JP", "山东省_jp", "籍贯.山东省"),
            // dict.hr.native.place.code.370000
            ("dict.hr.native.place.code.370000", "zh-CN", "山东省", "籍贯.山东省"),
            // dict.hr.native.place.code.370000
            ("dict.hr.native.place.code.370000", "zh-HK", "山东省_hk", "籍贯.山东省"),

            // dict.hr.native.place.code.410000
            ("dict.hr.native.place.code.410000", "en-US", "河南省_us", "籍贯.河南省"),
            // dict.hr.native.place.code.410000
            ("dict.hr.native.place.code.410000", "ja-JP", "河南省_jp", "籍贯.河南省"),
            // dict.hr.native.place.code.410000
            ("dict.hr.native.place.code.410000", "zh-CN", "河南省", "籍贯.河南省"),
            // dict.hr.native.place.code.410000
            ("dict.hr.native.place.code.410000", "zh-HK", "河南省_hk", "籍贯.河南省"),

            // dict.hr.native.place.code.420000
            ("dict.hr.native.place.code.420000", "en-US", "湖北省_us", "籍贯.湖北省"),
            // dict.hr.native.place.code.420000
            ("dict.hr.native.place.code.420000", "ja-JP", "湖北省_jp", "籍贯.湖北省"),
            // dict.hr.native.place.code.420000
            ("dict.hr.native.place.code.420000", "zh-CN", "湖北省", "籍贯.湖北省"),
            // dict.hr.native.place.code.420000
            ("dict.hr.native.place.code.420000", "zh-HK", "湖北省_hk", "籍贯.湖北省"),

            // dict.hr.native.place.code.430000
            ("dict.hr.native.place.code.430000", "en-US", "湖南省_us", "籍贯.湖南省"),
            // dict.hr.native.place.code.430000
            ("dict.hr.native.place.code.430000", "ja-JP", "湖南省_jp", "籍贯.湖南省"),
            // dict.hr.native.place.code.430000
            ("dict.hr.native.place.code.430000", "zh-CN", "湖南省", "籍贯.湖南省"),
            // dict.hr.native.place.code.430000
            ("dict.hr.native.place.code.430000", "zh-HK", "湖南省_hk", "籍贯.湖南省"),

            // dict.hr.native.place.code.440000
            ("dict.hr.native.place.code.440000", "en-US", "广东省_us", "籍贯.广东省"),
            // dict.hr.native.place.code.440000
            ("dict.hr.native.place.code.440000", "ja-JP", "广东省_jp", "籍贯.广东省"),
            // dict.hr.native.place.code.440000
            ("dict.hr.native.place.code.440000", "zh-CN", "广东省", "籍贯.广东省"),
            // dict.hr.native.place.code.440000
            ("dict.hr.native.place.code.440000", "zh-HK", "广东省_hk", "籍贯.广东省"),

            // dict.hr.native.place.code.450000
            ("dict.hr.native.place.code.450000", "en-US", "广西壮族自治区_us", "籍贯.广西壮族自治区"),
            // dict.hr.native.place.code.450000
            ("dict.hr.native.place.code.450000", "ja-JP", "广西壮族自治区_jp", "籍贯.广西壮族自治区"),
            // dict.hr.native.place.code.450000
            ("dict.hr.native.place.code.450000", "zh-CN", "广西壮族自治区", "籍贯.广西壮族自治区"),
            // dict.hr.native.place.code.450000
            ("dict.hr.native.place.code.450000", "zh-HK", "广西壮族自治区_hk", "籍贯.广西壮族自治区"),

            // dict.hr.native.place.code.460000
            ("dict.hr.native.place.code.460000", "en-US", "海南省_us", "籍贯.海南省"),
            // dict.hr.native.place.code.460000
            ("dict.hr.native.place.code.460000", "ja-JP", "海南省_jp", "籍贯.海南省"),
            // dict.hr.native.place.code.460000
            ("dict.hr.native.place.code.460000", "zh-CN", "海南省", "籍贯.海南省"),
            // dict.hr.native.place.code.460000
            ("dict.hr.native.place.code.460000", "zh-HK", "海南省_hk", "籍贯.海南省"),

            // dict.hr.native.place.code.500000
            ("dict.hr.native.place.code.500000", "en-US", "重庆市_us", "籍贯.重庆市"),
            // dict.hr.native.place.code.500000
            ("dict.hr.native.place.code.500000", "ja-JP", "重庆市_jp", "籍贯.重庆市"),
            // dict.hr.native.place.code.500000
            ("dict.hr.native.place.code.500000", "zh-CN", "重庆市", "籍贯.重庆市"),
            // dict.hr.native.place.code.500000
            ("dict.hr.native.place.code.500000", "zh-HK", "重庆市_hk", "籍贯.重庆市"),

            // dict.hr.native.place.code.510000
            ("dict.hr.native.place.code.510000", "en-US", "四川省_us", "籍贯.四川省"),
            // dict.hr.native.place.code.510000
            ("dict.hr.native.place.code.510000", "ja-JP", "四川省_jp", "籍贯.四川省"),
            // dict.hr.native.place.code.510000
            ("dict.hr.native.place.code.510000", "zh-CN", "四川省", "籍贯.四川省"),
            // dict.hr.native.place.code.510000
            ("dict.hr.native.place.code.510000", "zh-HK", "四川省_hk", "籍贯.四川省"),

            // dict.hr.native.place.code.520000
            ("dict.hr.native.place.code.520000", "en-US", "贵州省_us", "籍贯.贵州省"),
            // dict.hr.native.place.code.520000
            ("dict.hr.native.place.code.520000", "ja-JP", "贵州省_jp", "籍贯.贵州省"),
            // dict.hr.native.place.code.520000
            ("dict.hr.native.place.code.520000", "zh-CN", "贵州省", "籍贯.贵州省"),
            // dict.hr.native.place.code.520000
            ("dict.hr.native.place.code.520000", "zh-HK", "贵州省_hk", "籍贯.贵州省"),

            // dict.hr.native.place.code.530000
            ("dict.hr.native.place.code.530000", "en-US", "云南省_us", "籍贯.云南省"),
            // dict.hr.native.place.code.530000
            ("dict.hr.native.place.code.530000", "ja-JP", "云南省_jp", "籍贯.云南省"),
            // dict.hr.native.place.code.530000
            ("dict.hr.native.place.code.530000", "zh-CN", "云南省", "籍贯.云南省"),
            // dict.hr.native.place.code.530000
            ("dict.hr.native.place.code.530000", "zh-HK", "云南省_hk", "籍贯.云南省"),

            // dict.hr.native.place.code.540000
            ("dict.hr.native.place.code.540000", "en-US", "西藏自治区_us", "籍贯.西藏自治区"),
            // dict.hr.native.place.code.540000
            ("dict.hr.native.place.code.540000", "ja-JP", "西藏自治区_jp", "籍贯.西藏自治区"),
            // dict.hr.native.place.code.540000
            ("dict.hr.native.place.code.540000", "zh-CN", "西藏自治区", "籍贯.西藏自治区"),
            // dict.hr.native.place.code.540000
            ("dict.hr.native.place.code.540000", "zh-HK", "西藏自治区_hk", "籍贯.西藏自治区"),

            // dict.hr.native.place.code.610000
            ("dict.hr.native.place.code.610000", "en-US", "陕西省_us", "籍贯.陕西省"),
            // dict.hr.native.place.code.610000
            ("dict.hr.native.place.code.610000", "ja-JP", "陕西省_jp", "籍贯.陕西省"),
            // dict.hr.native.place.code.610000
            ("dict.hr.native.place.code.610000", "zh-CN", "陕西省", "籍贯.陕西省"),
            // dict.hr.native.place.code.610000
            ("dict.hr.native.place.code.610000", "zh-HK", "陕西省_hk", "籍贯.陕西省"),

            // dict.hr.native.place.code.620000
            ("dict.hr.native.place.code.620000", "en-US", "甘肃省_us", "籍贯.甘肃省"),
            // dict.hr.native.place.code.620000
            ("dict.hr.native.place.code.620000", "ja-JP", "甘肃省_jp", "籍贯.甘肃省"),
            // dict.hr.native.place.code.620000
            ("dict.hr.native.place.code.620000", "zh-CN", "甘肃省", "籍贯.甘肃省"),
            // dict.hr.native.place.code.620000
            ("dict.hr.native.place.code.620000", "zh-HK", "甘肃省_hk", "籍贯.甘肃省"),

            // dict.hr.native.place.code.630000
            ("dict.hr.native.place.code.630000", "en-US", "青海省_us", "籍贯.青海省"),
            // dict.hr.native.place.code.630000
            ("dict.hr.native.place.code.630000", "ja-JP", "青海省_jp", "籍贯.青海省"),
            // dict.hr.native.place.code.630000
            ("dict.hr.native.place.code.630000", "zh-CN", "青海省", "籍贯.青海省"),
            // dict.hr.native.place.code.630000
            ("dict.hr.native.place.code.630000", "zh-HK", "青海省_hk", "籍贯.青海省"),

            // dict.hr.native.place.code.640000
            ("dict.hr.native.place.code.640000", "en-US", "宁夏回族自治区_us", "籍贯.宁夏回族自治区"),
            // dict.hr.native.place.code.640000
            ("dict.hr.native.place.code.640000", "ja-JP", "宁夏回族自治区_jp", "籍贯.宁夏回族自治区"),
            // dict.hr.native.place.code.640000
            ("dict.hr.native.place.code.640000", "zh-CN", "宁夏回族自治区", "籍贯.宁夏回族自治区"),
            // dict.hr.native.place.code.640000
            ("dict.hr.native.place.code.640000", "zh-HK", "宁夏回族自治区_hk", "籍贯.宁夏回族自治区"),

            // dict.hr.native.place.code.650000
            ("dict.hr.native.place.code.650000", "en-US", "新疆维吾尔自治区_us", "籍贯.新疆维吾尔自治区"),
            // dict.hr.native.place.code.650000
            ("dict.hr.native.place.code.650000", "ja-JP", "新疆维吾尔自治区_jp", "籍贯.新疆维吾尔自治区"),
            // dict.hr.native.place.code.650000
            ("dict.hr.native.place.code.650000", "zh-CN", "新疆维吾尔自治区", "籍贯.新疆维吾尔自治区"),
            // dict.hr.native.place.code.650000
            ("dict.hr.native.place.code.650000", "zh-HK", "新疆维吾尔自治区_hk", "籍贯.新疆维吾尔自治区"),

            // dict.hr.native.place.code.710000
            ("dict.hr.native.place.code.710000", "en-US", "台湾省_us", "籍贯.台湾省"),
            // dict.hr.native.place.code.710000
            ("dict.hr.native.place.code.710000", "ja-JP", "台湾省_jp", "籍贯.台湾省"),
            // dict.hr.native.place.code.710000
            ("dict.hr.native.place.code.710000", "zh-CN", "台湾省", "籍贯.台湾省"),
            // dict.hr.native.place.code.710000
            ("dict.hr.native.place.code.710000", "zh-HK", "台湾省_hk", "籍贯.台湾省"),

            // dict.hr.native.place.code.810000
            ("dict.hr.native.place.code.810000", "en-US", "香港特别行政区_us", "籍贯.香港特别行政区"),
            // dict.hr.native.place.code.810000
            ("dict.hr.native.place.code.810000", "ja-JP", "香港特别行政区_jp", "籍贯.香港特别行政区"),
            // dict.hr.native.place.code.810000
            ("dict.hr.native.place.code.810000", "zh-CN", "香港特别行政区", "籍贯.香港特别行政区"),
            // dict.hr.native.place.code.810000
            ("dict.hr.native.place.code.810000", "zh-HK", "香港特别行政区_hk", "籍贯.香港特别行政区"),

            // dict.hr.native.place.code.820000
            ("dict.hr.native.place.code.820000", "en-US", "澳门特别行政区_us", "籍贯.澳门特别行政区"),
            // dict.hr.native.place.code.820000
            ("dict.hr.native.place.code.820000", "ja-JP", "澳门特别行政区_jp", "籍贯.澳门特别行政区"),
            // dict.hr.native.place.code.820000
            ("dict.hr.native.place.code.820000", "zh-CN", "澳门特别行政区", "籍贯.澳门特别行政区"),
            // dict.hr.native.place.code.820000
            ("dict.hr.native.place.code.820000", "zh-HK", "澳门特别行政区_hk", "籍贯.澳门特别行政区"),

            // dict.hr.overtime.type.0
            ("dict.hr.overtime.type.0", "en-US", "工作日加班_us", "加班类型.工作日加班"),
            // dict.hr.overtime.type.0
            ("dict.hr.overtime.type.0", "ja-JP", "工作日加班_jp", "加班类型.工作日加班"),
            // dict.hr.overtime.type.0
            ("dict.hr.overtime.type.0", "zh-CN", "工作日加班", "加班类型.工作日加班"),
            // dict.hr.overtime.type.0
            ("dict.hr.overtime.type.0", "zh-HK", "工作日加班_hk", "加班类型.工作日加班"),

            // dict.hr.overtime.type.1
            ("dict.hr.overtime.type.1", "en-US", "休息日加班_us", "加班类型.休息日加班"),
            // dict.hr.overtime.type.1
            ("dict.hr.overtime.type.1", "ja-JP", "休息日加班_jp", "加班类型.休息日加班"),
            // dict.hr.overtime.type.1
            ("dict.hr.overtime.type.1", "zh-CN", "休息日加班", "加班类型.休息日加班"),
            // dict.hr.overtime.type.1
            ("dict.hr.overtime.type.1", "zh-HK", "休息日加班_hk", "加班类型.休息日加班"),

            // dict.hr.overtime.type.2
            ("dict.hr.overtime.type.2", "en-US", "法定节假日加班_us", "加班类型.法定节假日加班"),
            // dict.hr.overtime.type.2
            ("dict.hr.overtime.type.2", "ja-JP", "法定节假日加班_jp", "加班类型.法定节假日加班"),
            // dict.hr.overtime.type.2
            ("dict.hr.overtime.type.2", "zh-CN", "法定节假日加班", "加班类型.法定节假日加班"),
            // dict.hr.overtime.type.2
            ("dict.hr.overtime.type.2", "zh-HK", "法定节假日加班_hk", "加班类型.法定节假日加班"),

            // dict.hr.political.status.0
            ("dict.hr.political.status.0", "en-US", "群众_us", "政治面貌.群众"),
            // dict.hr.political.status.0
            ("dict.hr.political.status.0", "ja-JP", "群众_jp", "政治面貌.群众"),
            // dict.hr.political.status.0
            ("dict.hr.political.status.0", "zh-CN", "群众", "政治面貌.群众"),
            // dict.hr.political.status.0
            ("dict.hr.political.status.0", "zh-HK", "群众_hk", "政治面貌.群众"),

            // dict.hr.political.status.1
            ("dict.hr.political.status.1", "en-US", "共青团员_us", "政治面貌.共青团员"),
            // dict.hr.political.status.1
            ("dict.hr.political.status.1", "ja-JP", "共青团员_jp", "政治面貌.共青团员"),
            // dict.hr.political.status.1
            ("dict.hr.political.status.1", "zh-CN", "共青团员", "政治面貌.共青团员"),
            // dict.hr.political.status.1
            ("dict.hr.political.status.1", "zh-HK", "共青团员_hk", "政治面貌.共青团员"),

            // dict.hr.political.status.2
            ("dict.hr.political.status.2", "en-US", "中共党员_us", "政治面貌.中共党员"),
            // dict.hr.political.status.2
            ("dict.hr.political.status.2", "ja-JP", "中共党员_jp", "政治面貌.中共党员"),
            // dict.hr.political.status.2
            ("dict.hr.political.status.2", "zh-CN", "中共党员", "政治面貌.中共党员"),
            // dict.hr.political.status.2
            ("dict.hr.political.status.2", "zh-HK", "中共党员_hk", "政治面貌.中共党员"),

            // dict.hr.political.status.3
            ("dict.hr.political.status.3", "en-US", "中共预备党员_us", "政治面貌.中共预备党员"),
            // dict.hr.political.status.3
            ("dict.hr.political.status.3", "ja-JP", "中共预备党员_jp", "政治面貌.中共预备党员"),
            // dict.hr.political.status.3
            ("dict.hr.political.status.3", "zh-CN", "中共预备党员", "政治面貌.中共预备党员"),
            // dict.hr.political.status.3
            ("dict.hr.political.status.3", "zh-HK", "中共预备党员_hk", "政治面貌.中共预备党员"),

            // dict.hr.political.status.4
            ("dict.hr.political.status.4", "en-US", "民革党员_us", "政治面貌.民革党员"),
            // dict.hr.political.status.4
            ("dict.hr.political.status.4", "ja-JP", "民革党员_jp", "政治面貌.民革党员"),
            // dict.hr.political.status.4
            ("dict.hr.political.status.4", "zh-CN", "民革党员", "政治面貌.民革党员"),
            // dict.hr.political.status.4
            ("dict.hr.political.status.4", "zh-HK", "民革党员_hk", "政治面貌.民革党员"),

            // dict.hr.political.status.5
            ("dict.hr.political.status.5", "en-US", "民盟盟员_us", "政治面貌.民盟盟员"),
            // dict.hr.political.status.5
            ("dict.hr.political.status.5", "ja-JP", "民盟盟员_jp", "政治面貌.民盟盟员"),
            // dict.hr.political.status.5
            ("dict.hr.political.status.5", "zh-CN", "民盟盟员", "政治面貌.民盟盟员"),
            // dict.hr.political.status.5
            ("dict.hr.political.status.5", "zh-HK", "民盟盟员_hk", "政治面貌.民盟盟员"),

            // dict.hr.political.status.6
            ("dict.hr.political.status.6", "en-US", "民建会员_us", "政治面貌.民建会员"),
            // dict.hr.political.status.6
            ("dict.hr.political.status.6", "ja-JP", "民建会员_jp", "政治面貌.民建会员"),
            // dict.hr.political.status.6
            ("dict.hr.political.status.6", "zh-CN", "民建会员", "政治面貌.民建会员"),
            // dict.hr.political.status.6
            ("dict.hr.political.status.6", "zh-HK", "民建会员_hk", "政治面貌.民建会员"),

            // dict.hr.political.status.7
            ("dict.hr.political.status.7", "en-US", "民进会员_us", "政治面貌.民进会员"),
            // dict.hr.political.status.7
            ("dict.hr.political.status.7", "ja-JP", "民进会员_jp", "政治面貌.民进会员"),
            // dict.hr.political.status.7
            ("dict.hr.political.status.7", "zh-CN", "民进会员", "政治面貌.民进会员"),
            // dict.hr.political.status.7
            ("dict.hr.political.status.7", "zh-HK", "民进会员_hk", "政治面貌.民进会员"),

            // dict.hr.political.status.8
            ("dict.hr.political.status.8", "en-US", "农工党党员_us", "政治面貌.农工党党员"),
            // dict.hr.political.status.8
            ("dict.hr.political.status.8", "ja-JP", "农工党党员_jp", "政治面貌.农工党党员"),
            // dict.hr.political.status.8
            ("dict.hr.political.status.8", "zh-CN", "农工党党员", "政治面貌.农工党党员"),
            // dict.hr.political.status.8
            ("dict.hr.political.status.8", "zh-HK", "农工党党员_hk", "政治面貌.农工党党员"),

            // dict.hr.political.status.9
            ("dict.hr.political.status.9", "en-US", "致公党党员_us", "政治面貌.致公党党员"),
            // dict.hr.political.status.9
            ("dict.hr.political.status.9", "ja-JP", "致公党党员_jp", "政治面貌.致公党党员"),
            // dict.hr.political.status.9
            ("dict.hr.political.status.9", "zh-CN", "致公党党员", "政治面貌.致公党党员"),
            // dict.hr.political.status.9
            ("dict.hr.political.status.9", "zh-HK", "致公党党员_hk", "政治面貌.致公党党员"),

            // dict.hr.political.status.10
            ("dict.hr.political.status.10", "en-US", "九三学社社员_us", "政治面貌.九三学社社员"),
            // dict.hr.political.status.10
            ("dict.hr.political.status.10", "ja-JP", "九三学社社员_jp", "政治面貌.九三学社社员"),
            // dict.hr.political.status.10
            ("dict.hr.political.status.10", "zh-CN", "九三学社社员", "政治面貌.九三学社社员"),
            // dict.hr.political.status.10
            ("dict.hr.political.status.10", "zh-HK", "九三学社社员_hk", "政治面貌.九三学社社员"),

            // dict.hr.political.status.11
            ("dict.hr.political.status.11", "en-US", "台盟盟员_us", "政治面貌.台盟盟员"),
            // dict.hr.political.status.11
            ("dict.hr.political.status.11", "ja-JP", "台盟盟员_jp", "政治面貌.台盟盟员"),
            // dict.hr.political.status.11
            ("dict.hr.political.status.11", "zh-CN", "台盟盟员", "政治面貌.台盟盟员"),
            // dict.hr.political.status.11
            ("dict.hr.political.status.11", "zh-HK", "台盟盟员_hk", "政治面貌.台盟盟员"),

            // dict.hr.political.status.12
            ("dict.hr.political.status.12", "en-US", "无党派民主人士_us", "政治面貌.无党派民主人士"),
            // dict.hr.political.status.12
            ("dict.hr.political.status.12", "ja-JP", "无党派民主人士_jp", "政治面貌.无党派民主人士"),
            // dict.hr.political.status.12
            ("dict.hr.political.status.12", "zh-CN", "无党派民主人士", "政治面貌.无党派民主人士"),
            // dict.hr.political.status.12
            ("dict.hr.political.status.12", "zh-HK", "无党派民主人士_hk", "政治面貌.无党派民主人士"),

            // dict.hr.schedule.type.0
            ("dict.hr.schedule.type.0", "en-US", "部门_us", "排班类别.部门"),
            // dict.hr.schedule.type.0
            ("dict.hr.schedule.type.0", "ja-JP", "部门_jp", "排班类别.部门"),
            // dict.hr.schedule.type.0
            ("dict.hr.schedule.type.0", "zh-CN", "部门", "排班类别.部门"),
            // dict.hr.schedule.type.0
            ("dict.hr.schedule.type.0", "zh-HK", "部门_hk", "排班类别.部门"),

            // dict.hr.schedule.type.1
            ("dict.hr.schedule.type.1", "en-US", "人员_us", "排班类别.人员"),
            // dict.hr.schedule.type.1
            ("dict.hr.schedule.type.1", "ja-JP", "人员_jp", "排班类别.人员"),
            // dict.hr.schedule.type.1
            ("dict.hr.schedule.type.1", "zh-CN", "人员", "排班类别.人员"),
            // dict.hr.schedule.type.1
            ("dict.hr.schedule.type.1", "zh-HK", "人员_hk", "排班类别.人员"),

            // dict.hr.reassignment.type.0
            ("dict.hr.reassignment.type.0", "en-US", "转岗_us", "调动类型.转岗"),
            // dict.hr.reassignment.type.0
            ("dict.hr.reassignment.type.0", "ja-JP", "转岗_jp", "调动类型.转岗"),
            // dict.hr.reassignment.type.0
            ("dict.hr.reassignment.type.0", "zh-CN", "转岗", "调动类型.转岗"),
            // dict.hr.reassignment.type.0
            ("dict.hr.reassignment.type.0", "zh-HK", "转岗_hk", "调动类型.转岗"),

            // dict.hr.reassignment.type.1
            ("dict.hr.reassignment.type.1", "en-US", "调岗_us", "调动类型.调岗"),
            // dict.hr.reassignment.type.1
            ("dict.hr.reassignment.type.1", "ja-JP", "调岗_jp", "调动类型.调岗"),
            // dict.hr.reassignment.type.1
            ("dict.hr.reassignment.type.1", "zh-CN", "调岗", "调动类型.调岗"),
            // dict.hr.reassignment.type.1
            ("dict.hr.reassignment.type.1", "zh-HK", "调岗_hk", "调动类型.调岗"),

            // dict.hr.talent.jobposting.status.0
            ("dict.hr.talent.jobposting.status.0", "en-US", "草稿_us", "职位发布状态.草稿"),
            // dict.hr.talent.jobposting.status.0
            ("dict.hr.talent.jobposting.status.0", "ja-JP", "草稿_jp", "职位发布状态.草稿"),
            // dict.hr.talent.jobposting.status.0
            ("dict.hr.talent.jobposting.status.0", "zh-CN", "草稿", "职位发布状态.草稿"),
            // dict.hr.talent.jobposting.status.0
            ("dict.hr.talent.jobposting.status.0", "zh-HK", "草稿_hk", "职位发布状态.草稿"),

            // dict.hr.talent.jobposting.status.1
            ("dict.hr.talent.jobposting.status.1", "en-US", "招聘中_us", "职位发布状态.招聘中"),
            // dict.hr.talent.jobposting.status.1
            ("dict.hr.talent.jobposting.status.1", "ja-JP", "招聘中_jp", "职位发布状态.招聘中"),
            // dict.hr.talent.jobposting.status.1
            ("dict.hr.talent.jobposting.status.1", "zh-CN", "招聘中", "职位发布状态.招聘中"),
            // dict.hr.talent.jobposting.status.1
            ("dict.hr.talent.jobposting.status.1", "zh-HK", "招聘中_hk", "职位发布状态.招聘中"),

            // dict.hr.talent.jobposting.status.2
            ("dict.hr.talent.jobposting.status.2", "en-US", "已暂停_us", "职位发布状态.已暂停"),
            // dict.hr.talent.jobposting.status.2
            ("dict.hr.talent.jobposting.status.2", "ja-JP", "已暂停_jp", "职位发布状态.已暂停"),
            // dict.hr.talent.jobposting.status.2
            ("dict.hr.talent.jobposting.status.2", "zh-CN", "已暂停", "职位发布状态.已暂停"),
            // dict.hr.talent.jobposting.status.2
            ("dict.hr.talent.jobposting.status.2", "zh-HK", "已暂停_hk", "职位发布状态.已暂停"),

            // dict.hr.talent.jobposting.status.3
            ("dict.hr.talent.jobposting.status.3", "en-US", "已关闭_us", "职位发布状态.已关闭"),
            // dict.hr.talent.jobposting.status.3
            ("dict.hr.talent.jobposting.status.3", "ja-JP", "已关闭_jp", "职位发布状态.已关闭"),
            // dict.hr.talent.jobposting.status.3
            ("dict.hr.talent.jobposting.status.3", "zh-CN", "已关闭", "职位发布状态.已关闭"),
            // dict.hr.talent.jobposting.status.3
            ("dict.hr.talent.jobposting.status.3", "zh-HK", "已关闭_hk", "职位发布状态.已关闭"),

            // dict.hr.talent.publish.channel.type.0
            ("dict.hr.talent.publish.channel.type.0", "en-US", "官网_us", "职位发布渠道.官网"),
            // dict.hr.talent.publish.channel.type.0
            ("dict.hr.talent.publish.channel.type.0", "ja-JP", "官网_jp", "职位发布渠道.官网"),
            // dict.hr.talent.publish.channel.type.0
            ("dict.hr.talent.publish.channel.type.0", "zh-CN", "官网", "职位发布渠道.官网"),
            // dict.hr.talent.publish.channel.type.0
            ("dict.hr.talent.publish.channel.type.0", "zh-HK", "官网_hk", "职位发布渠道.官网"),

            // dict.hr.talent.publish.channel.type.1
            ("dict.hr.talent.publish.channel.type.1", "en-US", "招聘网站_us", "职位发布渠道.招聘网站"),
            // dict.hr.talent.publish.channel.type.1
            ("dict.hr.talent.publish.channel.type.1", "ja-JP", "招聘网站_jp", "职位发布渠道.招聘网站"),
            // dict.hr.talent.publish.channel.type.1
            ("dict.hr.talent.publish.channel.type.1", "zh-CN", "招聘网站", "职位发布渠道.招聘网站"),
            // dict.hr.talent.publish.channel.type.1
            ("dict.hr.talent.publish.channel.type.1", "zh-HK", "招聘网站_hk", "职位发布渠道.招聘网站"),

            // dict.hr.talent.publish.channel.type.2
            ("dict.hr.talent.publish.channel.type.2", "en-US", "内推_us", "职位发布渠道.内推"),
            // dict.hr.talent.publish.channel.type.2
            ("dict.hr.talent.publish.channel.type.2", "ja-JP", "内推_jp", "职位发布渠道.内推"),
            // dict.hr.talent.publish.channel.type.2
            ("dict.hr.talent.publish.channel.type.2", "zh-CN", "内推", "职位发布渠道.内推"),
            // dict.hr.talent.publish.channel.type.2
            ("dict.hr.talent.publish.channel.type.2", "zh-HK", "内推_hk", "职位发布渠道.内推"),

            // dict.hr.talent.publish.channel.type.3
            ("dict.hr.talent.publish.channel.type.3", "en-US", "校园_us", "职位发布渠道.校园"),
            // dict.hr.talent.publish.channel.type.3
            ("dict.hr.talent.publish.channel.type.3", "ja-JP", "校园_jp", "职位发布渠道.校园"),
            // dict.hr.talent.publish.channel.type.3
            ("dict.hr.talent.publish.channel.type.3", "zh-CN", "校园", "职位发布渠道.校园"),
            // dict.hr.talent.publish.channel.type.3
            ("dict.hr.talent.publish.channel.type.3", "zh-HK", "校园_hk", "职位发布渠道.校园"),

            // dict.hr.talent.publish.channel.type.9
            ("dict.hr.talent.publish.channel.type.9", "en-US", "其他_us", "职位发布渠道.其他"),
            // dict.hr.talent.publish.channel.type.9
            ("dict.hr.talent.publish.channel.type.9", "ja-JP", "其他_jp", "职位发布渠道.其他"),
            // dict.hr.talent.publish.channel.type.9
            ("dict.hr.talent.publish.channel.type.9", "zh-CN", "其他", "职位发布渠道.其他"),
            // dict.hr.talent.publish.channel.type.9
            ("dict.hr.talent.publish.channel.type.9", "zh-HK", "其他_hk", "职位发布渠道.其他"),

            // dict.hr.talent.interview.status.0
            ("dict.hr.talent.interview.status.0", "en-US", "草稿_us", "面试安排状态.草稿"),
            // dict.hr.talent.interview.status.0
            ("dict.hr.talent.interview.status.0", "ja-JP", "草稿_jp", "面试安排状态.草稿"),
            // dict.hr.talent.interview.status.0
            ("dict.hr.talent.interview.status.0", "zh-CN", "草稿", "面试安排状态.草稿"),
            // dict.hr.talent.interview.status.0
            ("dict.hr.talent.interview.status.0", "zh-HK", "草稿_hk", "面试安排状态.草稿"),

            // dict.hr.talent.interview.status.1
            ("dict.hr.talent.interview.status.1", "en-US", "已安排_us", "面试安排状态.已安排"),
            // dict.hr.talent.interview.status.1
            ("dict.hr.talent.interview.status.1", "ja-JP", "已安排_jp", "面试安排状态.已安排"),
            // dict.hr.talent.interview.status.1
            ("dict.hr.talent.interview.status.1", "zh-CN", "已安排", "面试安排状态.已安排"),
            // dict.hr.talent.interview.status.1
            ("dict.hr.talent.interview.status.1", "zh-HK", "已安排_hk", "面试安排状态.已安排"),

            // dict.hr.talent.interview.status.2
            ("dict.hr.talent.interview.status.2", "en-US", "已完成_us", "面试安排状态.已完成"),
            // dict.hr.talent.interview.status.2
            ("dict.hr.talent.interview.status.2", "ja-JP", "已完成_jp", "面试安排状态.已完成"),
            // dict.hr.talent.interview.status.2
            ("dict.hr.talent.interview.status.2", "zh-CN", "已完成", "面试安排状态.已完成"),
            // dict.hr.talent.interview.status.2
            ("dict.hr.talent.interview.status.2", "zh-HK", "已完成_hk", "面试安排状态.已完成"),

            // dict.hr.talent.interview.status.3
            ("dict.hr.talent.interview.status.3", "en-US", "未通过_us", "面试安排状态.未通过"),
            // dict.hr.talent.interview.status.3
            ("dict.hr.talent.interview.status.3", "ja-JP", "未通过_jp", "面试安排状态.未通过"),
            // dict.hr.talent.interview.status.3
            ("dict.hr.talent.interview.status.3", "zh-CN", "未通过", "面试安排状态.未通过"),
            // dict.hr.talent.interview.status.3
            ("dict.hr.talent.interview.status.3", "zh-HK", "未通过_hk", "面试安排状态.未通过"),

            // dict.hr.talent.interview.status.4
            ("dict.hr.talent.interview.status.4", "en-US", "已取消_us", "面试安排状态.已取消"),
            // dict.hr.talent.interview.status.4
            ("dict.hr.talent.interview.status.4", "ja-JP", "已取消_jp", "面试安排状态.已取消"),
            // dict.hr.talent.interview.status.4
            ("dict.hr.talent.interview.status.4", "zh-CN", "已取消", "面试安排状态.已取消"),
            // dict.hr.talent.interview.status.4
            ("dict.hr.talent.interview.status.4", "zh-HK", "已取消_hk", "面试安排状态.已取消"),

            // dict.hr.talent.interview.round.type.1
            ("dict.hr.talent.interview.round.type.1", "en-US", "初试_us", "面试轮次.初试"),
            // dict.hr.talent.interview.round.type.1
            ("dict.hr.talent.interview.round.type.1", "ja-JP", "初试_jp", "面试轮次.初试"),
            // dict.hr.talent.interview.round.type.1
            ("dict.hr.talent.interview.round.type.1", "zh-CN", "初试", "面试轮次.初试"),
            // dict.hr.talent.interview.round.type.1
            ("dict.hr.talent.interview.round.type.1", "zh-HK", "初试_hk", "面试轮次.初试"),

            // dict.hr.talent.interview.round.type.2
            ("dict.hr.talent.interview.round.type.2", "en-US", "复试_us", "面试轮次.复试"),
            // dict.hr.talent.interview.round.type.2
            ("dict.hr.talent.interview.round.type.2", "ja-JP", "复试_jp", "面试轮次.复试"),
            // dict.hr.talent.interview.round.type.2
            ("dict.hr.talent.interview.round.type.2", "zh-CN", "复试", "面试轮次.复试"),
            // dict.hr.talent.interview.round.type.2
            ("dict.hr.talent.interview.round.type.2", "zh-HK", "复试_hk", "面试轮次.复试"),

            // dict.hr.talent.interview.round.type.3
            ("dict.hr.talent.interview.round.type.3", "en-US", "终试_us", "面试轮次.终试"),
            // dict.hr.talent.interview.round.type.3
            ("dict.hr.talent.interview.round.type.3", "ja-JP", "终试_jp", "面试轮次.终试"),
            // dict.hr.talent.interview.round.type.3
            ("dict.hr.talent.interview.round.type.3", "zh-CN", "终试", "面试轮次.终试"),
            // dict.hr.talent.interview.round.type.3
            ("dict.hr.talent.interview.round.type.3", "zh-HK", "终试_hk", "面试轮次.终试"),

            // dict.hr.personnel.onboarding.status.0
            ("dict.hr.personnel.onboarding.status.0", "en-US", "待办理_us", "入职待办状态.待办理"),
            // dict.hr.personnel.onboarding.status.0
            ("dict.hr.personnel.onboarding.status.0", "ja-JP", "待办理_jp", "入职待办状态.待办理"),
            // dict.hr.personnel.onboarding.status.0
            ("dict.hr.personnel.onboarding.status.0", "zh-CN", "待办理", "入职待办状态.待办理"),
            // dict.hr.personnel.onboarding.status.0
            ("dict.hr.personnel.onboarding.status.0", "zh-HK", "待办理_hk", "入职待办状态.待办理"),

            // dict.hr.personnel.onboarding.status.1
            ("dict.hr.personnel.onboarding.status.1", "en-US", "办理中_us", "入职待办状态.办理中"),
            // dict.hr.personnel.onboarding.status.1
            ("dict.hr.personnel.onboarding.status.1", "ja-JP", "办理中_jp", "入职待办状态.办理中"),
            // dict.hr.personnel.onboarding.status.1
            ("dict.hr.personnel.onboarding.status.1", "zh-CN", "办理中", "入职待办状态.办理中"),
            // dict.hr.personnel.onboarding.status.1
            ("dict.hr.personnel.onboarding.status.1", "zh-HK", "办理中_hk", "入职待办状态.办理中"),

            // dict.hr.personnel.onboarding.status.2
            ("dict.hr.personnel.onboarding.status.2", "en-US", "已完成_us", "入职待办状态.已完成"),
            // dict.hr.personnel.onboarding.status.2
            ("dict.hr.personnel.onboarding.status.2", "ja-JP", "已完成_jp", "入职待办状态.已完成"),
            // dict.hr.personnel.onboarding.status.2
            ("dict.hr.personnel.onboarding.status.2", "zh-CN", "已完成", "入职待办状态.已完成"),
            // dict.hr.personnel.onboarding.status.2
            ("dict.hr.personnel.onboarding.status.2", "zh-HK", "已完成_hk", "入职待办状态.已完成"),

            // dict.hr.personnel.onboarding.status.3
            ("dict.hr.personnel.onboarding.status.3", "en-US", "已取消_us", "入职待办状态.已取消"),
            // dict.hr.personnel.onboarding.status.3
            ("dict.hr.personnel.onboarding.status.3", "ja-JP", "已取消_jp", "入职待办状态.已取消"),
            // dict.hr.personnel.onboarding.status.3
            ("dict.hr.personnel.onboarding.status.3", "zh-CN", "已取消", "入职待办状态.已取消"),
            // dict.hr.personnel.onboarding.status.3
            ("dict.hr.personnel.onboarding.status.3", "zh-HK", "已取消_hk", "入职待办状态.已取消"),

            // dict.hr.resignation.category.0
            ("dict.hr.resignation.category.0", "en-US", "主动辞职_us", "离职类型.主动辞职"),
            // dict.hr.resignation.category.0
            ("dict.hr.resignation.category.0", "ja-JP", "主动辞职_jp", "离职类型.主动辞职"),
            // dict.hr.resignation.category.0
            ("dict.hr.resignation.category.0", "zh-CN", "主动辞职", "离职类型.主动辞职"),
            // dict.hr.resignation.category.0
            ("dict.hr.resignation.category.0", "zh-HK", "主动辞职_hk", "离职类型.主动辞职"),

            // dict.hr.resignation.category.1
            ("dict.hr.resignation.category.1", "en-US", "公司辞退_us", "离职类型.公司辞退"),
            // dict.hr.resignation.category.1
            ("dict.hr.resignation.category.1", "ja-JP", "公司辞退_jp", "离职类型.公司辞退"),
            // dict.hr.resignation.category.1
            ("dict.hr.resignation.category.1", "zh-CN", "公司辞退", "离职类型.公司辞退"),
            // dict.hr.resignation.category.1
            ("dict.hr.resignation.category.1", "zh-HK", "公司辞退_hk", "离职类型.公司辞退"),

            // dict.hr.resignation.category.2
            ("dict.hr.resignation.category.2", "en-US", "合同到期_us", "离职类型.合同到期"),
            // dict.hr.resignation.category.2
            ("dict.hr.resignation.category.2", "ja-JP", "合同到期_jp", "离职类型.合同到期"),
            // dict.hr.resignation.category.2
            ("dict.hr.resignation.category.2", "zh-CN", "合同到期", "离职类型.合同到期"),
            // dict.hr.resignation.category.2
            ("dict.hr.resignation.category.2", "zh-HK", "合同到期_hk", "离职类型.合同到期"),

            // dict.hr.resignation.category.3
            ("dict.hr.resignation.category.3", "en-US", "退休_us", "离职类型.退休"),
            // dict.hr.resignation.category.3
            ("dict.hr.resignation.category.3", "ja-JP", "退休_jp", "离职类型.退休"),
            // dict.hr.resignation.category.3
            ("dict.hr.resignation.category.3", "zh-CN", "退休", "离职类型.退休"),
            // dict.hr.resignation.category.3
            ("dict.hr.resignation.category.3", "zh-HK", "退休_hk", "离职类型.退休"),

            // dict.hr.resignation.category.9
            ("dict.hr.resignation.category.9", "en-US", "其他_us", "离职类型.其他"),
            // dict.hr.resignation.category.9
            ("dict.hr.resignation.category.9", "ja-JP", "其他_jp", "离职类型.其他"),
            // dict.hr.resignation.category.9
            ("dict.hr.resignation.category.9", "zh-CN", "其他", "离职类型.其他"),
            // dict.hr.resignation.category.9
            ("dict.hr.resignation.category.9", "zh-HK", "其他_hk", "离职类型.其他"),

            // dict.logistics.batch.management.type.0
            ("dict.logistics.batch.management.type.0", "en-US", "否_us", "批次管理标识.否"),
            // dict.logistics.batch.management.type.0
            ("dict.logistics.batch.management.type.0", "ja-JP", "否_jp", "批次管理标识.否"),
            // dict.logistics.batch.management.type.0
            ("dict.logistics.batch.management.type.0", "zh-CN", "否", "批次管理标识.否"),
            // dict.logistics.batch.management.type.0
            ("dict.logistics.batch.management.type.0", "zh-HK", "否_hk", "批次管理标识.否"),

            // dict.logistics.batch.management.type.1
            ("dict.logistics.batch.management.type.1", "en-US", "是_us", "批次管理标识.是"),
            // dict.logistics.batch.management.type.1
            ("dict.logistics.batch.management.type.1", "ja-JP", "是_jp", "批次管理标识.是"),
            // dict.logistics.batch.management.type.1
            ("dict.logistics.batch.management.type.1", "zh-CN", "是", "批次管理标识.是"),
            // dict.logistics.batch.management.type.1
            ("dict.logistics.batch.management.type.1", "zh-HK", "是_hk", "批次管理标识.是"),

            // dict.logistics.bulk.material.type.0
            ("dict.logistics.bulk.material.type.0", "en-US", "否_us", "散装物料标识.否"),
            // dict.logistics.bulk.material.type.0
            ("dict.logistics.bulk.material.type.0", "ja-JP", "否_jp", "散装物料标识.否"),
            // dict.logistics.bulk.material.type.0
            ("dict.logistics.bulk.material.type.0", "zh-CN", "否", "散装物料标识.否"),
            // dict.logistics.bulk.material.type.0
            ("dict.logistics.bulk.material.type.0", "zh-HK", "否_hk", "散装物料标识.否"),

            // dict.logistics.bulk.material.type.1
            ("dict.logistics.bulk.material.type.1", "en-US", "是_us", "散装物料标识.是"),
            // dict.logistics.bulk.material.type.1
            ("dict.logistics.bulk.material.type.1", "ja-JP", "是_jp", "散装物料标识.是"),
            // dict.logistics.bulk.material.type.1
            ("dict.logistics.bulk.material.type.1", "zh-CN", "是", "散装物料标识.是"),
            // dict.logistics.bulk.material.type.1
            ("dict.logistics.bulk.material.type.1", "zh-HK", "是_hk", "散装物料标识.是"),

            // dict.logistics.credit.rating.category.0
            ("dict.logistics.credit.rating.category.0", "en-US", "无_us", "信用等级.无"),
            // dict.logistics.credit.rating.category.0
            ("dict.logistics.credit.rating.category.0", "ja-JP", "无_jp", "信用等级.无"),
            // dict.logistics.credit.rating.category.0
            ("dict.logistics.credit.rating.category.0", "zh-CN", "无", "信用等级.无"),
            // dict.logistics.credit.rating.category.0
            ("dict.logistics.credit.rating.category.0", "zh-HK", "无_hk", "信用等级.无"),

            // dict.logistics.credit.rating.category.1
            ("dict.logistics.credit.rating.category.1", "en-US", "A级_us", "信用等级.A级"),
            // dict.logistics.credit.rating.category.1
            ("dict.logistics.credit.rating.category.1", "ja-JP", "A级_jp", "信用等级.A级"),
            // dict.logistics.credit.rating.category.1
            ("dict.logistics.credit.rating.category.1", "zh-CN", "A级", "信用等级.A级"),
            // dict.logistics.credit.rating.category.1
            ("dict.logistics.credit.rating.category.1", "zh-HK", "A级_hk", "信用等级.A级"),

            // dict.logistics.credit.rating.category.2
            ("dict.logistics.credit.rating.category.2", "en-US", "AA级_us", "信用等级.AA级"),
            // dict.logistics.credit.rating.category.2
            ("dict.logistics.credit.rating.category.2", "ja-JP", "AA级_jp", "信用等级.AA级"),
            // dict.logistics.credit.rating.category.2
            ("dict.logistics.credit.rating.category.2", "zh-CN", "AA级", "信用等级.AA级"),
            // dict.logistics.credit.rating.category.2
            ("dict.logistics.credit.rating.category.2", "zh-HK", "AA级_hk", "信用等级.AA级"),

            // dict.logistics.credit.rating.category.3
            ("dict.logistics.credit.rating.category.3", "en-US", "AAA级_us", "信用等级.AAA级"),
            // dict.logistics.credit.rating.category.3
            ("dict.logistics.credit.rating.category.3", "ja-JP", "AAA级_jp", "信用等级.AAA级"),
            // dict.logistics.credit.rating.category.3
            ("dict.logistics.credit.rating.category.3", "zh-CN", "AAA级", "信用等级.AAA级"),
            // dict.logistics.credit.rating.category.3
            ("dict.logistics.credit.rating.category.3", "zh-HK", "AAA级_hk", "信用等级.AAA级"),

            // dict.logistics.credit.rating.category.4
            ("dict.logistics.credit.rating.category.4", "en-US", "B级_us", "信用等级.B级"),
            // dict.logistics.credit.rating.category.4
            ("dict.logistics.credit.rating.category.4", "ja-JP", "B级_jp", "信用等级.B级"),
            // dict.logistics.credit.rating.category.4
            ("dict.logistics.credit.rating.category.4", "zh-CN", "B级", "信用等级.B级"),
            // dict.logistics.credit.rating.category.4
            ("dict.logistics.credit.rating.category.4", "zh-HK", "B级_hk", "信用等级.B级"),

            // dict.logistics.credit.rating.category.5
            ("dict.logistics.credit.rating.category.5", "en-US", "C级_us", "信用等级.C级"),
            // dict.logistics.credit.rating.category.5
            ("dict.logistics.credit.rating.category.5", "ja-JP", "C级_jp", "信用等级.C级"),
            // dict.logistics.credit.rating.category.5
            ("dict.logistics.credit.rating.category.5", "zh-CN", "C级", "信用等级.C级"),
            // dict.logistics.credit.rating.category.5
            ("dict.logistics.credit.rating.category.5", "zh-HK", "C级_hk", "信用等级.C级"),

            // dict.logistics.cycle.counting.category.a
            ("dict.logistics.cycle.counting.category.a", "en-US", "12月_us", "周期盘点标识.12月"),
            // dict.logistics.cycle.counting.category.a
            ("dict.logistics.cycle.counting.category.a", "ja-JP", "12月_jp", "周期盘点标识.12月"),
            // dict.logistics.cycle.counting.category.a
            ("dict.logistics.cycle.counting.category.a", "zh-CN", "12月", "周期盘点标识.12月"),
            // dict.logistics.cycle.counting.category.a
            ("dict.logistics.cycle.counting.category.a", "zh-HK", "12月_hk", "周期盘点标识.12月"),

            // dict.logistics.cycle.counting.category.b
            ("dict.logistics.cycle.counting.category.b", "en-US", "6月_us", "周期盘点标识.6月"),
            // dict.logistics.cycle.counting.category.b
            ("dict.logistics.cycle.counting.category.b", "ja-JP", "6月_jp", "周期盘点标识.6月"),
            // dict.logistics.cycle.counting.category.b
            ("dict.logistics.cycle.counting.category.b", "zh-CN", "6月", "周期盘点标识.6月"),
            // dict.logistics.cycle.counting.category.b
            ("dict.logistics.cycle.counting.category.b", "zh-HK", "6月_hk", "周期盘点标识.6月"),

            // dict.logistics.cycle.counting.category.c
            ("dict.logistics.cycle.counting.category.c", "en-US", "3月_us", "周期盘点标识.3月"),
            // dict.logistics.cycle.counting.category.c
            ("dict.logistics.cycle.counting.category.c", "ja-JP", "3月_jp", "周期盘点标识.3月"),
            // dict.logistics.cycle.counting.category.c
            ("dict.logistics.cycle.counting.category.c", "zh-CN", "3月", "周期盘点标识.3月"),
            // dict.logistics.cycle.counting.category.c
            ("dict.logistics.cycle.counting.category.c", "zh-HK", "3月_hk", "周期盘点标识.3月"),

            // dict.logistics.cycle.counting.category.d
            ("dict.logistics.cycle.counting.category.d", "en-US", "1月_us", "周期盘点标识.1月"),
            // dict.logistics.cycle.counting.category.d
            ("dict.logistics.cycle.counting.category.d", "ja-JP", "1月_jp", "周期盘点标识.1月"),
            // dict.logistics.cycle.counting.category.d
            ("dict.logistics.cycle.counting.category.d", "zh-CN", "1月", "周期盘点标识.1月"),
            // dict.logistics.cycle.counting.category.d
            ("dict.logistics.cycle.counting.category.d", "zh-HK", "1月_hk", "周期盘点标识.1月"),

            // dict.logistics.defect.level.category.critical
            ("dict.logistics.defect.level.category.critical", "en-US", "致命缺陷_us", "缺点等级.致命缺陷"),
            // dict.logistics.defect.level.category.critical
            ("dict.logistics.defect.level.category.critical", "ja-JP", "致命缺陷_jp", "缺点等级.致命缺陷"),
            // dict.logistics.defect.level.category.critical
            ("dict.logistics.defect.level.category.critical", "zh-CN", "致命缺陷", "缺点等级.致命缺陷"),
            // dict.logistics.defect.level.category.critical
            ("dict.logistics.defect.level.category.critical", "zh-HK", "致命缺陷_hk", "缺点等级.致命缺陷"),

            // dict.logistics.defect.level.category.major
            ("dict.logistics.defect.level.category.major", "en-US", "严重缺陷_us", "缺点等级.严重缺陷"),
            // dict.logistics.defect.level.category.major
            ("dict.logistics.defect.level.category.major", "ja-JP", "严重缺陷_jp", "缺点等级.严重缺陷"),
            // dict.logistics.defect.level.category.major
            ("dict.logistics.defect.level.category.major", "zh-CN", "严重缺陷", "缺点等级.严重缺陷"),
            // dict.logistics.defect.level.category.major
            ("dict.logistics.defect.level.category.major", "zh-HK", "严重缺陷_hk", "缺点等级.严重缺陷"),

            // dict.logistics.defect.level.category.minor
            ("dict.logistics.defect.level.category.minor", "en-US", "轻微缺陷_us", "缺点等级.轻微缺陷"),
            // dict.logistics.defect.level.category.minor
            ("dict.logistics.defect.level.category.minor", "ja-JP", "轻微缺陷_jp", "缺点等级.轻微缺陷"),
            // dict.logistics.defect.level.category.minor
            ("dict.logistics.defect.level.category.minor", "zh-CN", "轻微缺陷", "缺点等级.轻微缺陷"),
            // dict.logistics.defect.level.category.minor
            ("dict.logistics.defect.level.category.minor", "zh-HK", "轻微缺陷_hk", "缺点等级.轻微缺陷"),

            // dict.logistics.defect.level.category.suggestion
            ("dict.logistics.defect.level.category.suggestion", "en-US", "建议改进_us", "缺点等级.建议改进"),
            // dict.logistics.defect.level.category.suggestion
            ("dict.logistics.defect.level.category.suggestion", "ja-JP", "建议改进_jp", "缺点等级.建议改进"),
            // dict.logistics.defect.level.category.suggestion
            ("dict.logistics.defect.level.category.suggestion", "zh-CN", "建议改进", "缺点等级.建议改进"),
            // dict.logistics.defect.level.category.suggestion
            ("dict.logistics.defect.level.category.suggestion", "zh-HK", "建议改进_hk", "缺点等级.建议改进"),

            // dict.logistics.equipment.category.production
            ("dict.logistics.equipment.category.production", "en-US", "生产设备_us", "设备类别.生产设备"),
            // dict.logistics.equipment.category.production
            ("dict.logistics.equipment.category.production", "ja-JP", "生产设备_jp", "设备类别.生产设备"),
            // dict.logistics.equipment.category.production
            ("dict.logistics.equipment.category.production", "zh-CN", "生产设备", "设备类别.生产设备"),
            // dict.logistics.equipment.category.production
            ("dict.logistics.equipment.category.production", "zh-HK", "生产设备_hk", "设备类别.生产设备"),

            // dict.logistics.equipment.category.inspection
            ("dict.logistics.equipment.category.inspection", "en-US", "检测设备_us", "设备类别.检测设备"),
            // dict.logistics.equipment.category.inspection
            ("dict.logistics.equipment.category.inspection", "ja-JP", "检测设备_jp", "设备类别.检测设备"),
            // dict.logistics.equipment.category.inspection
            ("dict.logistics.equipment.category.inspection", "zh-CN", "检测设备", "设备类别.检测设备"),
            // dict.logistics.equipment.category.inspection
            ("dict.logistics.equipment.category.inspection", "zh-HK", "检测设备_hk", "设备类别.检测设备"),

            // dict.logistics.equipment.category.packaging
            ("dict.logistics.equipment.category.packaging", "en-US", "包装设备_us", "设备类别.包装设备"),
            // dict.logistics.equipment.category.packaging
            ("dict.logistics.equipment.category.packaging", "ja-JP", "包装设备_jp", "设备类别.包装设备"),
            // dict.logistics.equipment.category.packaging
            ("dict.logistics.equipment.category.packaging", "zh-CN", "包装设备", "设备类别.包装设备"),
            // dict.logistics.equipment.category.packaging
            ("dict.logistics.equipment.category.packaging", "zh-HK", "包装设备_hk", "设备类别.包装设备"),

            // dict.logistics.equipment.category.warehouse
            ("dict.logistics.equipment.category.warehouse", "en-US", "仓储设备_us", "设备类别.仓储设备"),
            // dict.logistics.equipment.category.warehouse
            ("dict.logistics.equipment.category.warehouse", "ja-JP", "仓储设备_jp", "设备类别.仓储设备"),
            // dict.logistics.equipment.category.warehouse
            ("dict.logistics.equipment.category.warehouse", "zh-CN", "仓储设备", "设备类别.仓储设备"),
            // dict.logistics.equipment.category.warehouse
            ("dict.logistics.equipment.category.warehouse", "zh-HK", "仓储设备_hk", "设备类别.仓储设备"),

            // dict.logistics.equipment.category.transport
            ("dict.logistics.equipment.category.transport", "en-US", "运输设备_us", "设备类别.运输设备"),
            // dict.logistics.equipment.category.transport
            ("dict.logistics.equipment.category.transport", "ja-JP", "运输设备_jp", "设备类别.运输设备"),
            // dict.logistics.equipment.category.transport
            ("dict.logistics.equipment.category.transport", "zh-CN", "运输设备", "设备类别.运输设备"),
            // dict.logistics.equipment.category.transport
            ("dict.logistics.equipment.category.transport", "zh-HK", "运输设备_hk", "设备类别.运输设备"),

            // dict.logistics.equipment.category.office
            ("dict.logistics.equipment.category.office", "en-US", "办公设备_us", "设备类别.办公设备"),
            // dict.logistics.equipment.category.office
            ("dict.logistics.equipment.category.office", "ja-JP", "办公设备_jp", "设备类别.办公设备"),
            // dict.logistics.equipment.category.office
            ("dict.logistics.equipment.category.office", "zh-CN", "办公设备", "设备类别.办公设备"),
            // dict.logistics.equipment.category.office
            ("dict.logistics.equipment.category.office", "zh-HK", "办公设备_hk", "设备类别.办公设备"),

            // dict.logistics.equipment.category.it
            ("dict.logistics.equipment.category.it", "en-US", "it设备_us", "设备类别.it设备"),
            // dict.logistics.equipment.category.it
            ("dict.logistics.equipment.category.it", "ja-JP", "it设备_jp", "设备类别.it设备"),
            // dict.logistics.equipment.category.it
            ("dict.logistics.equipment.category.it", "zh-CN", "it设备", "设备类别.it设备"),
            // dict.logistics.equipment.category.it
            ("dict.logistics.equipment.category.it", "zh-HK", "it设备_hk", "设备类别.it设备"),

            // dict.logistics.equipment.category.power
            ("dict.logistics.equipment.category.power", "en-US", "动力设备_us", "设备类别.动力设备"),
            // dict.logistics.equipment.category.power
            ("dict.logistics.equipment.category.power", "ja-JP", "动力设备_jp", "设备类别.动力设备"),
            // dict.logistics.equipment.category.power
            ("dict.logistics.equipment.category.power", "zh-CN", "动力设备", "设备类别.动力设备"),
            // dict.logistics.equipment.category.power
            ("dict.logistics.equipment.category.power", "zh-HK", "动力设备_hk", "设备类别.动力设备"),

            // dict.logistics.equipment.category.environmental
            ("dict.logistics.equipment.category.environmental", "en-US", "环保设备_us", "设备类别.环保设备"),
            // dict.logistics.equipment.category.environmental
            ("dict.logistics.equipment.category.environmental", "ja-JP", "环保设备_jp", "设备类别.环保设备"),
            // dict.logistics.equipment.category.environmental
            ("dict.logistics.equipment.category.environmental", "zh-CN", "环保设备", "设备类别.环保设备"),
            // dict.logistics.equipment.category.environmental
            ("dict.logistics.equipment.category.environmental", "zh-HK", "环保设备_hk", "设备类别.环保设备"),

            // dict.logistics.equipment.category.special
            ("dict.logistics.equipment.category.special", "en-US", "特种设备_us", "设备类别.特种设备"),
            // dict.logistics.equipment.category.special
            ("dict.logistics.equipment.category.special", "ja-JP", "特种设备_jp", "设备类别.特种设备"),
            // dict.logistics.equipment.category.special
            ("dict.logistics.equipment.category.special", "zh-CN", "特种设备", "设备类别.特种设备"),
            // dict.logistics.equipment.category.special
            ("dict.logistics.equipment.category.special", "zh-HK", "特种设备_hk", "设备类别.特种设备"),

            // dict.logistics.grade.category.0
            ("dict.logistics.grade.category.0", "en-US", "普通_us", "等级类别.普通"),
            // dict.logistics.grade.category.0
            ("dict.logistics.grade.category.0", "ja-JP", "普通_jp", "等级类别.普通"),
            // dict.logistics.grade.category.0
            ("dict.logistics.grade.category.0", "zh-CN", "普通", "等级类别.普通"),
            // dict.logistics.grade.category.0
            ("dict.logistics.grade.category.0", "zh-HK", "普通_hk", "等级类别.普通"),

            // dict.logistics.grade.category.1
            ("dict.logistics.grade.category.1", "en-US", "优选_us", "等级类别.优选"),
            // dict.logistics.grade.category.1
            ("dict.logistics.grade.category.1", "ja-JP", "优选_jp", "等级类别.优选"),
            // dict.logistics.grade.category.1
            ("dict.logistics.grade.category.1", "zh-CN", "优选", "等级类别.优选"),
            // dict.logistics.grade.category.1
            ("dict.logistics.grade.category.1", "zh-HK", "优选_hk", "等级类别.优选"),

            // dict.logistics.grade.category.2
            ("dict.logistics.grade.category.2", "en-US", "战略_us", "等级类别.战略"),
            // dict.logistics.grade.category.2
            ("dict.logistics.grade.category.2", "ja-JP", "战略_jp", "等级类别.战略"),
            // dict.logistics.grade.category.2
            ("dict.logistics.grade.category.2", "zh-CN", "战略", "等级类别.战略"),
            // dict.logistics.grade.category.2
            ("dict.logistics.grade.category.2", "zh-HK", "战略_hk", "等级类别.战略"),

            // dict.logistics.grade.category.3
            ("dict.logistics.grade.category.3", "en-US", "临时_us", "等级类别.临时"),
            // dict.logistics.grade.category.3
            ("dict.logistics.grade.category.3", "ja-JP", "临时_jp", "等级类别.临时"),
            // dict.logistics.grade.category.3
            ("dict.logistics.grade.category.3", "zh-CN", "临时", "等级类别.临时"),
            // dict.logistics.grade.category.3
            ("dict.logistics.grade.category.3", "zh-HK", "临时_hk", "等级类别.临时"),

            // dict.logistics.handling.plan.category.rework
            ("dict.logistics.handling.plan.category.rework", "en-US", "返工_us", "处理方案.返工"),
            // dict.logistics.handling.plan.category.rework
            ("dict.logistics.handling.plan.category.rework", "ja-JP", "返工_jp", "处理方案.返工"),
            // dict.logistics.handling.plan.category.rework
            ("dict.logistics.handling.plan.category.rework", "zh-CN", "返工", "处理方案.返工"),
            // dict.logistics.handling.plan.category.rework
            ("dict.logistics.handling.plan.category.rework", "zh-HK", "返工_hk", "处理方案.返工"),

            // dict.logistics.handling.plan.category.repair
            ("dict.logistics.handling.plan.category.repair", "en-US", "返修_us", "处理方案.返修"),
            // dict.logistics.handling.plan.category.repair
            ("dict.logistics.handling.plan.category.repair", "ja-JP", "返修_jp", "处理方案.返修"),
            // dict.logistics.handling.plan.category.repair
            ("dict.logistics.handling.plan.category.repair", "zh-CN", "返修", "处理方案.返修"),
            // dict.logistics.handling.plan.category.repair
            ("dict.logistics.handling.plan.category.repair", "zh-HK", "返修_hk", "处理方案.返修"),

            // dict.logistics.handling.plan.category.scrap
            ("dict.logistics.handling.plan.category.scrap", "en-US", "报废_us", "处理方案.报废"),
            // dict.logistics.handling.plan.category.scrap
            ("dict.logistics.handling.plan.category.scrap", "ja-JP", "报废_jp", "处理方案.报废"),
            // dict.logistics.handling.plan.category.scrap
            ("dict.logistics.handling.plan.category.scrap", "zh-CN", "报废", "处理方案.报废"),
            // dict.logistics.handling.plan.category.scrap
            ("dict.logistics.handling.plan.category.scrap", "zh-HK", "报废_hk", "处理方案.报废"),

            // dict.logistics.handling.plan.category.return
            ("dict.logistics.handling.plan.category.return", "en-US", "退货_us", "处理方案.退货"),
            // dict.logistics.handling.plan.category.return
            ("dict.logistics.handling.plan.category.return", "ja-JP", "退货_jp", "处理方案.退货"),
            // dict.logistics.handling.plan.category.return
            ("dict.logistics.handling.plan.category.return", "zh-CN", "退货", "处理方案.退货"),
            // dict.logistics.handling.plan.category.return
            ("dict.logistics.handling.plan.category.return", "zh-HK", "退货_hk", "处理方案.退货"),

            // dict.logistics.handling.plan.category.exchange
            ("dict.logistics.handling.plan.category.exchange", "en-US", "换货_us", "处理方案.换货"),
            // dict.logistics.handling.plan.category.exchange
            ("dict.logistics.handling.plan.category.exchange", "ja-JP", "换货_jp", "处理方案.换货"),
            // dict.logistics.handling.plan.category.exchange
            ("dict.logistics.handling.plan.category.exchange", "zh-CN", "换货", "处理方案.换货"),
            // dict.logistics.handling.plan.category.exchange
            ("dict.logistics.handling.plan.category.exchange", "zh-HK", "换货_hk", "处理方案.换货"),

            // dict.logistics.handling.plan.category.concession
            ("dict.logistics.handling.plan.category.concession", "en-US", "让步接收_us", "处理方案.让步接收"),
            // dict.logistics.handling.plan.category.concession
            ("dict.logistics.handling.plan.category.concession", "ja-JP", "让步接收_jp", "处理方案.让步接收"),
            // dict.logistics.handling.plan.category.concession
            ("dict.logistics.handling.plan.category.concession", "zh-CN", "让步接收", "处理方案.让步接收"),
            // dict.logistics.handling.plan.category.concession
            ("dict.logistics.handling.plan.category.concession", "zh-HK", "让步接收_hk", "处理方案.让步接收"),

            // dict.logistics.handling.plan.category.downgrade
            ("dict.logistics.handling.plan.category.downgrade", "en-US", "降级使用_us", "处理方案.降级使用"),
            // dict.logistics.handling.plan.category.downgrade
            ("dict.logistics.handling.plan.category.downgrade", "ja-JP", "降级使用_jp", "处理方案.降级使用"),
            // dict.logistics.handling.plan.category.downgrade
            ("dict.logistics.handling.plan.category.downgrade", "zh-CN", "降级使用", "处理方案.降级使用"),
            // dict.logistics.handling.plan.category.downgrade
            ("dict.logistics.handling.plan.category.downgrade", "zh-HK", "降级使用_hk", "处理方案.降级使用"),

            // dict.logistics.handling.plan.category.sorting
            ("dict.logistics.handling.plan.category.sorting", "en-US", "挑选使用_us", "处理方案.挑选使用"),
            // dict.logistics.handling.plan.category.sorting
            ("dict.logistics.handling.plan.category.sorting", "ja-JP", "挑选使用_jp", "处理方案.挑选使用"),
            // dict.logistics.handling.plan.category.sorting
            ("dict.logistics.handling.plan.category.sorting", "zh-CN", "挑选使用", "处理方案.挑选使用"),
            // dict.logistics.handling.plan.category.sorting
            ("dict.logistics.handling.plan.category.sorting", "zh-HK", "挑选使用_hk", "处理方案.挑选使用"),

            // dict.logistics.handling.plan.category.special_accept
            ("dict.logistics.handling.plan.category.special_accept", "en-US", "特采_us", "处理方案.特采"),
            // dict.logistics.handling.plan.category.special_accept
            ("dict.logistics.handling.plan.category.special_accept", "ja-JP", "特采_jp", "处理方案.特采"),
            // dict.logistics.handling.plan.category.special_accept
            ("dict.logistics.handling.plan.category.special_accept", "zh-CN", "特采", "处理方案.特采"),
            // dict.logistics.handling.plan.category.special_accept
            ("dict.logistics.handling.plan.category.special_accept", "zh-HK", "特采_hk", "处理方案.特采"),

            // dict.logistics.inhouse.production.days.param.2
            ("dict.logistics.inhouse.production.days.param.2", "en-US", "2天_us", "自制生产天数.2天"),
            // dict.logistics.inhouse.production.days.param.2
            ("dict.logistics.inhouse.production.days.param.2", "ja-JP", "2天_jp", "自制生产天数.2天"),
            // dict.logistics.inhouse.production.days.param.2
            ("dict.logistics.inhouse.production.days.param.2", "zh-CN", "2天", "自制生产天数.2天"),
            // dict.logistics.inhouse.production.days.param.2
            ("dict.logistics.inhouse.production.days.param.2", "zh-HK", "2天_hk", "自制生产天数.2天"),

            // dict.logistics.inhouse.production.days.param.5
            ("dict.logistics.inhouse.production.days.param.5", "en-US", "5天_us", "自制生产天数.5天"),
            // dict.logistics.inhouse.production.days.param.5
            ("dict.logistics.inhouse.production.days.param.5", "ja-JP", "5天_jp", "自制生产天数.5天"),
            // dict.logistics.inhouse.production.days.param.5
            ("dict.logistics.inhouse.production.days.param.5", "zh-CN", "5天", "自制生产天数.5天"),
            // dict.logistics.inhouse.production.days.param.5
            ("dict.logistics.inhouse.production.days.param.5", "zh-HK", "5天_hk", "自制生产天数.5天"),

            // dict.logistics.inspection.category.iqc
            ("dict.logistics.inspection.category.iqc", "en-US", "进料检验_us", "检验类型.进料检验"),
            // dict.logistics.inspection.category.iqc
            ("dict.logistics.inspection.category.iqc", "ja-JP", "进料检验_jp", "检验类型.进料检验"),
            // dict.logistics.inspection.category.iqc
            ("dict.logistics.inspection.category.iqc", "zh-CN", "进料检验", "检验类型.进料检验"),
            // dict.logistics.inspection.category.iqc
            ("dict.logistics.inspection.category.iqc", "zh-HK", "进料检验_hk", "检验类型.进料检验"),

            // dict.logistics.inspection.category.ipqc
            ("dict.logistics.inspection.category.ipqc", "en-US", "过程检验_us", "检验类型.过程检验"),
            // dict.logistics.inspection.category.ipqc
            ("dict.logistics.inspection.category.ipqc", "ja-JP", "过程检验_jp", "检验类型.过程检验"),
            // dict.logistics.inspection.category.ipqc
            ("dict.logistics.inspection.category.ipqc", "zh-CN", "过程检验", "检验类型.过程检验"),
            // dict.logistics.inspection.category.ipqc
            ("dict.logistics.inspection.category.ipqc", "zh-HK", "过程检验_hk", "检验类型.过程检验"),

            // dict.logistics.inspection.category.fqc
            ("dict.logistics.inspection.category.fqc", "en-US", "最终检验_us", "检验类型.最终检验"),
            // dict.logistics.inspection.category.fqc
            ("dict.logistics.inspection.category.fqc", "ja-JP", "最终检验_jp", "检验类型.最终检验"),
            // dict.logistics.inspection.category.fqc
            ("dict.logistics.inspection.category.fqc", "zh-CN", "最终检验", "检验类型.最终检验"),
            // dict.logistics.inspection.category.fqc
            ("dict.logistics.inspection.category.fqc", "zh-HK", "最终检验_hk", "检验类型.最终检验"),

            // dict.logistics.inspection.category.oqc
            ("dict.logistics.inspection.category.oqc", "en-US", "出货检验_us", "检验类型.出货检验"),
            // dict.logistics.inspection.category.oqc
            ("dict.logistics.inspection.category.oqc", "ja-JP", "出货检验_jp", "检验类型.出货检验"),
            // dict.logistics.inspection.category.oqc
            ("dict.logistics.inspection.category.oqc", "zh-CN", "出货检验", "检验类型.出货检验"),
            // dict.logistics.inspection.category.oqc
            ("dict.logistics.inspection.category.oqc", "zh-HK", "出货检验_hk", "检验类型.出货检验"),

            // dict.logistics.inspection.category.fai
            ("dict.logistics.inspection.category.fai", "en-US", "首件检验_us", "检验类型.首件检验"),
            // dict.logistics.inspection.category.fai
            ("dict.logistics.inspection.category.fai", "ja-JP", "首件检验_jp", "检验类型.首件检验"),
            // dict.logistics.inspection.category.fai
            ("dict.logistics.inspection.category.fai", "zh-CN", "首件检验", "检验类型.首件检验"),
            // dict.logistics.inspection.category.fai
            ("dict.logistics.inspection.category.fai", "zh-HK", "首件检验_hk", "检验类型.首件检验"),

            // dict.logistics.inspection.category.patrol
            ("dict.logistics.inspection.category.patrol", "en-US", "巡检_us", "检验类型.巡检"),
            // dict.logistics.inspection.category.patrol
            ("dict.logistics.inspection.category.patrol", "ja-JP", "巡检_jp", "检验类型.巡检"),
            // dict.logistics.inspection.category.patrol
            ("dict.logistics.inspection.category.patrol", "zh-CN", "巡检", "检验类型.巡检"),
            // dict.logistics.inspection.category.patrol
            ("dict.logistics.inspection.category.patrol", "zh-HK", "巡检_hk", "检验类型.巡检"),

            // dict.logistics.inspection.category.full
            ("dict.logistics.inspection.category.full", "en-US", "全检_us", "检验类型.全检"),
            // dict.logistics.inspection.category.full
            ("dict.logistics.inspection.category.full", "ja-JP", "全检_jp", "检验类型.全检"),
            // dict.logistics.inspection.category.full
            ("dict.logistics.inspection.category.full", "zh-CN", "全检", "检验类型.全检"),
            // dict.logistics.inspection.category.full
            ("dict.logistics.inspection.category.full", "zh-HK", "全检_hk", "检验类型.全检"),

            // dict.logistics.inspection.category.sampling
            ("dict.logistics.inspection.category.sampling", "en-US", "抽样检验_us", "检验类型.抽样检验"),
            // dict.logistics.inspection.category.sampling
            ("dict.logistics.inspection.category.sampling", "ja-JP", "抽样检验_jp", "检验类型.抽样检验"),
            // dict.logistics.inspection.category.sampling
            ("dict.logistics.inspection.category.sampling", "zh-CN", "抽样检验", "检验类型.抽样检验"),
            // dict.logistics.inspection.category.sampling
            ("dict.logistics.inspection.category.sampling", "zh-HK", "抽样检验_hk", "检验类型.抽样检验"),

            // dict.logistics.inspection.category.type_test
            ("dict.logistics.inspection.category.type_test", "en-US", "型式试验_us", "检验类型.型式试验"),
            // dict.logistics.inspection.category.type_test
            ("dict.logistics.inspection.category.type_test", "ja-JP", "型式试验_jp", "检验类型.型式试验"),
            // dict.logistics.inspection.category.type_test
            ("dict.logistics.inspection.category.type_test", "zh-CN", "型式试验", "检验类型.型式试验"),
            // dict.logistics.inspection.category.type_test
            ("dict.logistics.inspection.category.type_test", "zh-HK", "型式试验_hk", "检验类型.型式试验"),

            // dict.logistics.inspection.category.reliability
            ("dict.logistics.inspection.category.reliability", "en-US", "可靠性试验_us", "检验类型.可靠性试验"),
            // dict.logistics.inspection.category.reliability
            ("dict.logistics.inspection.category.reliability", "ja-JP", "可靠性试验_jp", "检验类型.可靠性试验"),
            // dict.logistics.inspection.category.reliability
            ("dict.logistics.inspection.category.reliability", "zh-CN", "可靠性试验", "检验类型.可靠性试验"),
            // dict.logistics.inspection.category.reliability
            ("dict.logistics.inspection.category.reliability", "zh-HK", "可靠性试验_hk", "检验类型.可靠性试验"),

            // dict.logistics.inspection.item.type.dimension
            ("dict.logistics.inspection.item.type.dimension", "en-US", "尺寸检验_us", "检验项目类型.尺寸检验"),
            // dict.logistics.inspection.item.type.dimension
            ("dict.logistics.inspection.item.type.dimension", "ja-JP", "尺寸检验_jp", "检验项目类型.尺寸检验"),
            // dict.logistics.inspection.item.type.dimension
            ("dict.logistics.inspection.item.type.dimension", "zh-CN", "尺寸检验", "检验项目类型.尺寸检验"),
            // dict.logistics.inspection.item.type.dimension
            ("dict.logistics.inspection.item.type.dimension", "zh-HK", "尺寸检验_hk", "检验项目类型.尺寸检验"),

            // dict.logistics.inspection.item.type.appearance
            ("dict.logistics.inspection.item.type.appearance", "en-US", "外观检验_us", "检验项目类型.外观检验"),
            // dict.logistics.inspection.item.type.appearance
            ("dict.logistics.inspection.item.type.appearance", "ja-JP", "外观检验_jp", "检验项目类型.外观检验"),
            // dict.logistics.inspection.item.type.appearance
            ("dict.logistics.inspection.item.type.appearance", "zh-CN", "外观检验", "检验项目类型.外观检验"),
            // dict.logistics.inspection.item.type.appearance
            ("dict.logistics.inspection.item.type.appearance", "zh-HK", "外观检验_hk", "检验项目类型.外观检验"),

            // dict.logistics.inspection.item.type.performance
            ("dict.logistics.inspection.item.type.performance", "en-US", "性能检验_us", "检验项目类型.性能检验"),
            // dict.logistics.inspection.item.type.performance
            ("dict.logistics.inspection.item.type.performance", "ja-JP", "性能检验_jp", "检验项目类型.性能检验"),
            // dict.logistics.inspection.item.type.performance
            ("dict.logistics.inspection.item.type.performance", "zh-CN", "性能检验", "检验项目类型.性能检验"),
            // dict.logistics.inspection.item.type.performance
            ("dict.logistics.inspection.item.type.performance", "zh-HK", "性能检验_hk", "检验项目类型.性能检验"),

            // dict.logistics.inspection.item.type.function
            ("dict.logistics.inspection.item.type.function", "en-US", "功能检验_us", "检验项目类型.功能检验"),
            // dict.logistics.inspection.item.type.function
            ("dict.logistics.inspection.item.type.function", "ja-JP", "功能检验_jp", "检验项目类型.功能检验"),
            // dict.logistics.inspection.item.type.function
            ("dict.logistics.inspection.item.type.function", "zh-CN", "功能检验", "检验项目类型.功能检验"),
            // dict.logistics.inspection.item.type.function
            ("dict.logistics.inspection.item.type.function", "zh-HK", "功能检验_hk", "检验项目类型.功能检验"),

            // dict.logistics.inspection.item.type.material
            ("dict.logistics.inspection.item.type.material", "en-US", "材质检验_us", "检验项目类型.材质检验"),
            // dict.logistics.inspection.item.type.material
            ("dict.logistics.inspection.item.type.material", "ja-JP", "材质检验_jp", "检验项目类型.材质检验"),
            // dict.logistics.inspection.item.type.material
            ("dict.logistics.inspection.item.type.material", "zh-CN", "材质检验", "检验项目类型.材质检验"),
            // dict.logistics.inspection.item.type.material
            ("dict.logistics.inspection.item.type.material", "zh-HK", "材质检验_hk", "检验项目类型.材质检验"),

            // dict.logistics.inspection.item.type.structure
            ("dict.logistics.inspection.item.type.structure", "en-US", "结构检验_us", "检验项目类型.结构检验"),
            // dict.logistics.inspection.item.type.structure
            ("dict.logistics.inspection.item.type.structure", "ja-JP", "结构检验_jp", "检验项目类型.结构检验"),
            // dict.logistics.inspection.item.type.structure
            ("dict.logistics.inspection.item.type.structure", "zh-CN", "结构检验", "检验项目类型.结构检验"),
            // dict.logistics.inspection.item.type.structure
            ("dict.logistics.inspection.item.type.structure", "zh-HK", "结构检验_hk", "检验项目类型.结构检验"),

            // dict.logistics.inspection.item.type.packaging
            ("dict.logistics.inspection.item.type.packaging", "en-US", "包装检验_us", "检验项目类型.包装检验"),
            // dict.logistics.inspection.item.type.packaging
            ("dict.logistics.inspection.item.type.packaging", "ja-JP", "包装检验_jp", "检验项目类型.包装检验"),
            // dict.logistics.inspection.item.type.packaging
            ("dict.logistics.inspection.item.type.packaging", "zh-CN", "包装检验", "检验项目类型.包装检验"),
            // dict.logistics.inspection.item.type.packaging
            ("dict.logistics.inspection.item.type.packaging", "zh-HK", "包装检验_hk", "检验项目类型.包装检验"),

            // dict.logistics.inspection.item.type.labeling
            ("dict.logistics.inspection.item.type.labeling", "en-US", "标识检验_us", "检验项目类型.标识检验"),
            // dict.logistics.inspection.item.type.labeling
            ("dict.logistics.inspection.item.type.labeling", "ja-JP", "标识检验_jp", "检验项目类型.标识检验"),
            // dict.logistics.inspection.item.type.labeling
            ("dict.logistics.inspection.item.type.labeling", "zh-CN", "标识检验", "检验项目类型.标识检验"),
            // dict.logistics.inspection.item.type.labeling
            ("dict.logistics.inspection.item.type.labeling", "zh-HK", "标识检验_hk", "检验项目类型.标识检验"),

            // dict.logistics.inspection.item.type.safety
            ("dict.logistics.inspection.item.type.safety", "en-US", "安全检验_us", "检验项目类型.安全检验"),
            // dict.logistics.inspection.item.type.safety
            ("dict.logistics.inspection.item.type.safety", "ja-JP", "安全检验_jp", "检验项目类型.安全检验"),
            // dict.logistics.inspection.item.type.safety
            ("dict.logistics.inspection.item.type.safety", "zh-CN", "安全检验", "检验项目类型.安全检验"),
            // dict.logistics.inspection.item.type.safety
            ("dict.logistics.inspection.item.type.safety", "zh-HK", "安全检验_hk", "检验项目类型.安全检验"),

            // dict.logistics.inspection.item.type.environment
            ("dict.logistics.inspection.item.type.environment", "en-US", "环境检验_us", "检验项目类型.环境检验"),
            // dict.logistics.inspection.item.type.environment
            ("dict.logistics.inspection.item.type.environment", "ja-JP", "环境检验_jp", "检验项目类型.环境检验"),
            // dict.logistics.inspection.item.type.environment
            ("dict.logistics.inspection.item.type.environment", "zh-CN", "环境检验", "检验项目类型.环境检验"),
            // dict.logistics.inspection.item.type.environment
            ("dict.logistics.inspection.item.type.environment", "zh-HK", "环境检验_hk", "检验项目类型.环境检验"),

            // dict.logistics.inspection.method.type.full
            ("dict.logistics.inspection.method.type.full", "en-US", "全检_us", "检验方式.全检"),
            // dict.logistics.inspection.method.type.full
            ("dict.logistics.inspection.method.type.full", "ja-JP", "全检_jp", "检验方式.全检"),
            // dict.logistics.inspection.method.type.full
            ("dict.logistics.inspection.method.type.full", "zh-CN", "全检", "检验方式.全检"),
            // dict.logistics.inspection.method.type.full
            ("dict.logistics.inspection.method.type.full", "zh-HK", "全检_hk", "检验方式.全检"),

            // dict.logistics.inspection.method.type.sampling
            ("dict.logistics.inspection.method.type.sampling", "en-US", "抽样检验_us", "检验方式.抽样检验"),
            // dict.logistics.inspection.method.type.sampling
            ("dict.logistics.inspection.method.type.sampling", "ja-JP", "抽样检验_jp", "检验方式.抽样检验"),
            // dict.logistics.inspection.method.type.sampling
            ("dict.logistics.inspection.method.type.sampling", "zh-CN", "抽样检验", "检验方式.抽样检验"),
            // dict.logistics.inspection.method.type.sampling
            ("dict.logistics.inspection.method.type.sampling", "zh-HK", "抽样检验_hk", "检验方式.抽样检验"),

            // dict.logistics.inspection.method.type.skip
            ("dict.logistics.inspection.method.type.skip", "en-US", "免检_us", "检验方式.免检"),
            // dict.logistics.inspection.method.type.skip
            ("dict.logistics.inspection.method.type.skip", "ja-JP", "免检_jp", "检验方式.免检"),
            // dict.logistics.inspection.method.type.skip
            ("dict.logistics.inspection.method.type.skip", "zh-CN", "免检", "检验方式.免检"),
            // dict.logistics.inspection.method.type.skip
            ("dict.logistics.inspection.method.type.skip", "zh-HK", "免检_hk", "检验方式.免检"),

            // dict.logistics.inspection.method.type.visual
            ("dict.logistics.inspection.method.type.visual", "en-US", "目视检验_us", "检验方式.目视检验"),
            // dict.logistics.inspection.method.type.visual
            ("dict.logistics.inspection.method.type.visual", "ja-JP", "目视检验_jp", "检验方式.目视检验"),
            // dict.logistics.inspection.method.type.visual
            ("dict.logistics.inspection.method.type.visual", "zh-CN", "目视检验", "检验方式.目视检验"),
            // dict.logistics.inspection.method.type.visual
            ("dict.logistics.inspection.method.type.visual", "zh-HK", "目视检验_hk", "检验方式.目视检验"),

            // dict.logistics.inspection.method.type.instrument
            ("dict.logistics.inspection.method.type.instrument", "en-US", "仪器检验_us", "检验方式.仪器检验"),
            // dict.logistics.inspection.method.type.instrument
            ("dict.logistics.inspection.method.type.instrument", "ja-JP", "仪器检验_jp", "检验方式.仪器检验"),
            // dict.logistics.inspection.method.type.instrument
            ("dict.logistics.inspection.method.type.instrument", "zh-CN", "仪器检验", "检验方式.仪器检验"),
            // dict.logistics.inspection.method.type.instrument
            ("dict.logistics.inspection.method.type.instrument", "zh-HK", "仪器检验_hk", "检验方式.仪器检验"),

            // dict.logistics.inspection.method.type.destructive
            ("dict.logistics.inspection.method.type.destructive", "en-US", "破坏性检验_us", "检验方式.破坏性检验"),
            // dict.logistics.inspection.method.type.destructive
            ("dict.logistics.inspection.method.type.destructive", "ja-JP", "破坏性检验_jp", "检验方式.破坏性检验"),
            // dict.logistics.inspection.method.type.destructive
            ("dict.logistics.inspection.method.type.destructive", "zh-CN", "破坏性检验", "检验方式.破坏性检验"),
            // dict.logistics.inspection.method.type.destructive
            ("dict.logistics.inspection.method.type.destructive", "zh-HK", "破坏性检验_hk", "检验方式.破坏性检验"),

            // dict.logistics.inspection.method.type.non_destructive
            ("dict.logistics.inspection.method.type.non_destructive", "en-US", "非破坏性检验_us", "检验方式.非破坏性检验"),
            // dict.logistics.inspection.method.type.non_destructive
            ("dict.logistics.inspection.method.type.non_destructive", "ja-JP", "非破坏性检验_jp", "检验方式.非破坏性检验"),
            // dict.logistics.inspection.method.type.non_destructive
            ("dict.logistics.inspection.method.type.non_destructive", "zh-CN", "非破坏性检验", "检验方式.非破坏性检验"),
            // dict.logistics.inspection.method.type.non_destructive
            ("dict.logistics.inspection.method.type.non_destructive", "zh-HK", "非破坏性检验_hk", "检验方式.非破坏性检验"),

            // dict.logistics.inspection.severity.category.normal
            ("dict.logistics.inspection.severity.category.normal", "en-US", "正常检验_us", "检验严格度.正常检验"),
            // dict.logistics.inspection.severity.category.normal
            ("dict.logistics.inspection.severity.category.normal", "ja-JP", "正常检验_jp", "检验严格度.正常检验"),
            // dict.logistics.inspection.severity.category.normal
            ("dict.logistics.inspection.severity.category.normal", "zh-CN", "正常检验", "检验严格度.正常检验"),
            // dict.logistics.inspection.severity.category.normal
            ("dict.logistics.inspection.severity.category.normal", "zh-HK", "正常检验_hk", "检验严格度.正常检验"),

            // dict.logistics.inspection.severity.category.tightened
            ("dict.logistics.inspection.severity.category.tightened", "en-US", "加严检验_us", "检验严格度.加严检验"),
            // dict.logistics.inspection.severity.category.tightened
            ("dict.logistics.inspection.severity.category.tightened", "ja-JP", "加严检验_jp", "检验严格度.加严检验"),
            // dict.logistics.inspection.severity.category.tightened
            ("dict.logistics.inspection.severity.category.tightened", "zh-CN", "加严检验", "检验严格度.加严检验"),
            // dict.logistics.inspection.severity.category.tightened
            ("dict.logistics.inspection.severity.category.tightened", "zh-HK", "加严检验_hk", "检验严格度.加严检验"),

            // dict.logistics.inspection.severity.category.reduced
            ("dict.logistics.inspection.severity.category.reduced", "en-US", "放宽检验_us", "检验严格度.放宽检验"),
            // dict.logistics.inspection.severity.category.reduced
            ("dict.logistics.inspection.severity.category.reduced", "ja-JP", "放宽检验_jp", "检验严格度.放宽检验"),
            // dict.logistics.inspection.severity.category.reduced
            ("dict.logistics.inspection.severity.category.reduced", "zh-CN", "放宽检验", "检验严格度.放宽检验"),
            // dict.logistics.inspection.severity.category.reduced
            ("dict.logistics.inspection.severity.category.reduced", "zh-HK", "放宽检验_hk", "检验严格度.放宽检验"),

            // dict.logistics.inspection.tool.category.caliper
            ("dict.logistics.inspection.tool.category.caliper", "en-US", "卡尺_us", "检验工具.卡尺"),
            // dict.logistics.inspection.tool.category.caliper
            ("dict.logistics.inspection.tool.category.caliper", "ja-JP", "卡尺_jp", "检验工具.卡尺"),
            // dict.logistics.inspection.tool.category.caliper
            ("dict.logistics.inspection.tool.category.caliper", "zh-CN", "卡尺", "检验工具.卡尺"),
            // dict.logistics.inspection.tool.category.caliper
            ("dict.logistics.inspection.tool.category.caliper", "zh-HK", "卡尺_hk", "检验工具.卡尺"),

            // dict.logistics.inspection.tool.category.micrometer
            ("dict.logistics.inspection.tool.category.micrometer", "en-US", "千分尺_us", "检验工具.千分尺"),
            // dict.logistics.inspection.tool.category.micrometer
            ("dict.logistics.inspection.tool.category.micrometer", "ja-JP", "千分尺_jp", "检验工具.千分尺"),
            // dict.logistics.inspection.tool.category.micrometer
            ("dict.logistics.inspection.tool.category.micrometer", "zh-CN", "千分尺", "检验工具.千分尺"),
            // dict.logistics.inspection.tool.category.micrometer
            ("dict.logistics.inspection.tool.category.micrometer", "zh-HK", "千分尺_hk", "检验工具.千分尺"),

            // dict.logistics.inspection.tool.category.height_gauge
            ("dict.logistics.inspection.tool.category.height_gauge", "en-US", "高度尺_us", "检验工具.高度尺"),
            // dict.logistics.inspection.tool.category.height_gauge
            ("dict.logistics.inspection.tool.category.height_gauge", "ja-JP", "高度尺_jp", "检验工具.高度尺"),
            // dict.logistics.inspection.tool.category.height_gauge
            ("dict.logistics.inspection.tool.category.height_gauge", "zh-CN", "高度尺", "检验工具.高度尺"),
            // dict.logistics.inspection.tool.category.height_gauge
            ("dict.logistics.inspection.tool.category.height_gauge", "zh-HK", "高度尺_hk", "检验工具.高度尺"),

            // dict.logistics.inspection.tool.category.feeler_gauge
            ("dict.logistics.inspection.tool.category.feeler_gauge", "en-US", "塞尺_us", "检验工具.塞尺"),
            // dict.logistics.inspection.tool.category.feeler_gauge
            ("dict.logistics.inspection.tool.category.feeler_gauge", "ja-JP", "塞尺_jp", "检验工具.塞尺"),
            // dict.logistics.inspection.tool.category.feeler_gauge
            ("dict.logistics.inspection.tool.category.feeler_gauge", "zh-CN", "塞尺", "检验工具.塞尺"),
            // dict.logistics.inspection.tool.category.feeler_gauge
            ("dict.logistics.inspection.tool.category.feeler_gauge", "zh-HK", "塞尺_hk", "检验工具.塞尺"),

            // dict.logistics.inspection.tool.category.thread_gauge
            ("dict.logistics.inspection.tool.category.thread_gauge", "en-US", "螺纹规_us", "检验工具.螺纹规"),
            // dict.logistics.inspection.tool.category.thread_gauge
            ("dict.logistics.inspection.tool.category.thread_gauge", "ja-JP", "螺纹规_jp", "检验工具.螺纹规"),
            // dict.logistics.inspection.tool.category.thread_gauge
            ("dict.logistics.inspection.tool.category.thread_gauge", "zh-CN", "螺纹规", "检验工具.螺纹规"),
            // dict.logistics.inspection.tool.category.thread_gauge
            ("dict.logistics.inspection.tool.category.thread_gauge", "zh-HK", "螺纹规_hk", "检验工具.螺纹规"),

            // dict.logistics.inspection.tool.category.hardness_tester
            ("dict.logistics.inspection.tool.category.hardness_tester", "en-US", "硬度计_us", "检验工具.硬度计"),
            // dict.logistics.inspection.tool.category.hardness_tester
            ("dict.logistics.inspection.tool.category.hardness_tester", "ja-JP", "硬度计_jp", "检验工具.硬度计"),
            // dict.logistics.inspection.tool.category.hardness_tester
            ("dict.logistics.inspection.tool.category.hardness_tester", "zh-CN", "硬度计", "检验工具.硬度计"),
            // dict.logistics.inspection.tool.category.hardness_tester
            ("dict.logistics.inspection.tool.category.hardness_tester", "zh-HK", "硬度计_hk", "检验工具.硬度计"),

            // dict.logistics.inspection.tool.category.roughness_tester
            ("dict.logistics.inspection.tool.category.roughness_tester", "en-US", "粗糙度仪_us", "检验工具.粗糙度仪"),
            // dict.logistics.inspection.tool.category.roughness_tester
            ("dict.logistics.inspection.tool.category.roughness_tester", "ja-JP", "粗糙度仪_jp", "检验工具.粗糙度仪"),
            // dict.logistics.inspection.tool.category.roughness_tester
            ("dict.logistics.inspection.tool.category.roughness_tester", "zh-CN", "粗糙度仪", "检验工具.粗糙度仪"),
            // dict.logistics.inspection.tool.category.roughness_tester
            ("dict.logistics.inspection.tool.category.roughness_tester", "zh-HK", "粗糙度仪_hk", "检验工具.粗糙度仪"),

            // dict.logistics.inspection.tool.category.cmm
            ("dict.logistics.inspection.tool.category.cmm", "en-US", "三坐标测量机_us", "检验工具.三坐标测量机"),
            // dict.logistics.inspection.tool.category.cmm
            ("dict.logistics.inspection.tool.category.cmm", "ja-JP", "三坐标测量机_jp", "检验工具.三坐标测量机"),
            // dict.logistics.inspection.tool.category.cmm
            ("dict.logistics.inspection.tool.category.cmm", "zh-CN", "三坐标测量机", "检验工具.三坐标测量机"),
            // dict.logistics.inspection.tool.category.cmm
            ("dict.logistics.inspection.tool.category.cmm", "zh-HK", "三坐标测量机_hk", "检验工具.三坐标测量机"),

            // dict.logistics.inspection.tool.category.projector
            ("dict.logistics.inspection.tool.category.projector", "en-US", "投影仪_us", "检验工具.投影仪"),
            // dict.logistics.inspection.tool.category.projector
            ("dict.logistics.inspection.tool.category.projector", "ja-JP", "投影仪_jp", "检验工具.投影仪"),
            // dict.logistics.inspection.tool.category.projector
            ("dict.logistics.inspection.tool.category.projector", "zh-CN", "投影仪", "检验工具.投影仪"),
            // dict.logistics.inspection.tool.category.projector
            ("dict.logistics.inspection.tool.category.projector", "zh-HK", "投影仪_hk", "检验工具.投影仪"),

            // dict.logistics.inspection.tool.category.tensile_tester
            ("dict.logistics.inspection.tool.category.tensile_tester", "en-US", "拉力试验机_us", "检验工具.拉力试验机"),
            // dict.logistics.inspection.tool.category.tensile_tester
            ("dict.logistics.inspection.tool.category.tensile_tester", "ja-JP", "拉力试验机_jp", "检验工具.拉力试验机"),
            // dict.logistics.inspection.tool.category.tensile_tester
            ("dict.logistics.inspection.tool.category.tensile_tester", "zh-CN", "拉力试验机", "检验工具.拉力试验机"),
            // dict.logistics.inspection.tool.category.tensile_tester
            ("dict.logistics.inspection.tool.category.tensile_tester", "zh-HK", "拉力试验机_hk", "检验工具.拉力试验机"),

            // dict.logistics.inspection.tool.category.multimeter
            ("dict.logistics.inspection.tool.category.multimeter", "en-US", "万用表_us", "检验工具.万用表"),
            // dict.logistics.inspection.tool.category.multimeter
            ("dict.logistics.inspection.tool.category.multimeter", "ja-JP", "万用表_jp", "检验工具.万用表"),
            // dict.logistics.inspection.tool.category.multimeter
            ("dict.logistics.inspection.tool.category.multimeter", "zh-CN", "万用表", "检验工具.万用表"),
            // dict.logistics.inspection.tool.category.multimeter
            ("dict.logistics.inspection.tool.category.multimeter", "zh-HK", "万用表_hk", "检验工具.万用表"),

            // dict.logistics.inspection.tool.category.oscilloscope
            ("dict.logistics.inspection.tool.category.oscilloscope", "en-US", "示波器_us", "检验工具.示波器"),
            // dict.logistics.inspection.tool.category.oscilloscope
            ("dict.logistics.inspection.tool.category.oscilloscope", "ja-JP", "示波器_jp", "检验工具.示波器"),
            // dict.logistics.inspection.tool.category.oscilloscope
            ("dict.logistics.inspection.tool.category.oscilloscope", "zh-CN", "示波器", "检验工具.示波器"),
            // dict.logistics.inspection.tool.category.oscilloscope
            ("dict.logistics.inspection.tool.category.oscilloscope", "zh-HK", "示波器_hk", "检验工具.示波器"),

            // dict.logistics.inspection.tool.category.colorimeter
            ("dict.logistics.inspection.tool.category.colorimeter", "en-US", "色差仪_us", "检验工具.色差仪"),
            // dict.logistics.inspection.tool.category.colorimeter
            ("dict.logistics.inspection.tool.category.colorimeter", "ja-JP", "色差仪_jp", "检验工具.色差仪"),
            // dict.logistics.inspection.tool.category.colorimeter
            ("dict.logistics.inspection.tool.category.colorimeter", "zh-CN", "色差仪", "检验工具.色差仪"),
            // dict.logistics.inspection.tool.category.colorimeter
            ("dict.logistics.inspection.tool.category.colorimeter", "zh-HK", "色差仪_hk", "检验工具.色差仪"),

            // dict.logistics.inspection.tool.category.glossmeter
            ("dict.logistics.inspection.tool.category.glossmeter", "en-US", "光泽度计_us", "检验工具.光泽度计"),
            // dict.logistics.inspection.tool.category.glossmeter
            ("dict.logistics.inspection.tool.category.glossmeter", "ja-JP", "光泽度计_jp", "检验工具.光泽度计"),
            // dict.logistics.inspection.tool.category.glossmeter
            ("dict.logistics.inspection.tool.category.glossmeter", "zh-CN", "光泽度计", "检验工具.光泽度计"),
            // dict.logistics.inspection.tool.category.glossmeter
            ("dict.logistics.inspection.tool.category.glossmeter", "zh-HK", "光泽度计_hk", "检验工具.光泽度计"),

            // dict.logistics.inspection.tool.category.thickness_gauge
            ("dict.logistics.inspection.tool.category.thickness_gauge", "en-US", "厚度计_us", "检验工具.厚度计"),
            // dict.logistics.inspection.tool.category.thickness_gauge
            ("dict.logistics.inspection.tool.category.thickness_gauge", "ja-JP", "厚度计_jp", "检验工具.厚度计"),
            // dict.logistics.inspection.tool.category.thickness_gauge
            ("dict.logistics.inspection.tool.category.thickness_gauge", "zh-CN", "厚度计", "检验工具.厚度计"),
            // dict.logistics.inspection.tool.category.thickness_gauge
            ("dict.logistics.inspection.tool.category.thickness_gauge", "zh-HK", "厚度计_hk", "检验工具.厚度计"),

            // dict.logistics.inspection.type.0
            ("dict.logistics.inspection.type.0", "en-US", "免检_us", "检验类别.免检"),
            // dict.logistics.inspection.type.0
            ("dict.logistics.inspection.type.0", "ja-JP", "免检_jp", "检验类别.免检"),
            // dict.logistics.inspection.type.0
            ("dict.logistics.inspection.type.0", "zh-CN", "免检", "检验类别.免检"),
            // dict.logistics.inspection.type.0
            ("dict.logistics.inspection.type.0", "zh-HK", "免检_hk", "检验类别.免检"),

            // dict.logistics.inspection.type.1
            ("dict.logistics.inspection.type.1", "en-US", "必检_us", "检验类别.必检"),
            // dict.logistics.inspection.type.1
            ("dict.logistics.inspection.type.1", "ja-JP", "必检_jp", "检验类别.必检"),
            // dict.logistics.inspection.type.1
            ("dict.logistics.inspection.type.1", "zh-CN", "必检", "检验类别.必检"),
            // dict.logistics.inspection.type.1
            ("dict.logistics.inspection.type.1", "zh-HK", "必检_hk", "检验类别.必检"),

            // dict.logistics.judgment.category.pass
            ("dict.logistics.judgment.category.pass", "en-US", "合格_us", "判定类别.合格"),
            // dict.logistics.judgment.category.pass
            ("dict.logistics.judgment.category.pass", "ja-JP", "合格_jp", "判定类别.合格"),
            // dict.logistics.judgment.category.pass
            ("dict.logistics.judgment.category.pass", "zh-CN", "合格", "判定类别.合格"),
            // dict.logistics.judgment.category.pass
            ("dict.logistics.judgment.category.pass", "zh-HK", "合格_hk", "判定类别.合格"),

            // dict.logistics.judgment.category.fail
            ("dict.logistics.judgment.category.fail", "en-US", "不合格_us", "判定类别.不合格"),
            // dict.logistics.judgment.category.fail
            ("dict.logistics.judgment.category.fail", "ja-JP", "不合格_jp", "判定类别.不合格"),
            // dict.logistics.judgment.category.fail
            ("dict.logistics.judgment.category.fail", "zh-CN", "不合格", "判定类别.不合格"),
            // dict.logistics.judgment.category.fail
            ("dict.logistics.judgment.category.fail", "zh-HK", "不合格_hk", "判定类别.不合格"),

            // dict.logistics.judgment.category.pending
            ("dict.logistics.judgment.category.pending", "en-US", "待判定_us", "判定类别.待判定"),
            // dict.logistics.judgment.category.pending
            ("dict.logistics.judgment.category.pending", "ja-JP", "待判定_jp", "判定类别.待判定"),
            // dict.logistics.judgment.category.pending
            ("dict.logistics.judgment.category.pending", "zh-CN", "待判定", "判定类别.待判定"),
            // dict.logistics.judgment.category.pending
            ("dict.logistics.judgment.category.pending", "zh-HK", "待判定_hk", "判定类别.待判定"),

            // dict.logistics.judgment.category.concession
            ("dict.logistics.judgment.category.concession", "en-US", "让步接收_us", "判定类别.让步接收"),
            // dict.logistics.judgment.category.concession
            ("dict.logistics.judgment.category.concession", "ja-JP", "让步接收_jp", "判定类别.让步接收"),
            // dict.logistics.judgment.category.concession
            ("dict.logistics.judgment.category.concession", "zh-CN", "让步接收", "判定类别.让步接收"),
            // dict.logistics.judgment.category.concession
            ("dict.logistics.judgment.category.concession", "zh-HK", "让步接收_hk", "判定类别.让步接收"),

            // dict.logistics.judgment.category.special_accept
            ("dict.logistics.judgment.category.special_accept", "en-US", "特采_us", "判定类别.特采"),
            // dict.logistics.judgment.category.special_accept
            ("dict.logistics.judgment.category.special_accept", "ja-JP", "特采_jp", "判定类别.特采"),
            // dict.logistics.judgment.category.special_accept
            ("dict.logistics.judgment.category.special_accept", "zh-CN", "特采", "判定类别.特采"),
            // dict.logistics.judgment.category.special_accept
            ("dict.logistics.judgment.category.special_accept", "zh-HK", "特采_hk", "判定类别.特采"),

            // dict.logistics.judgment.category.return
            ("dict.logistics.judgment.category.return", "en-US", "退货_us", "判定类别.退货"),
            // dict.logistics.judgment.category.return
            ("dict.logistics.judgment.category.return", "ja-JP", "退货_jp", "判定类别.退货"),
            // dict.logistics.judgment.category.return
            ("dict.logistics.judgment.category.return", "zh-CN", "退货", "判定类别.退货"),
            // dict.logistics.judgment.category.return
            ("dict.logistics.judgment.category.return", "zh-HK", "退货_hk", "判定类别.退货"),

            // dict.logistics.judgment.category.sorting
            ("dict.logistics.judgment.category.sorting", "en-US", "挑选使用_us", "判定类别.挑选使用"),
            // dict.logistics.judgment.category.sorting
            ("dict.logistics.judgment.category.sorting", "ja-JP", "挑选使用_jp", "判定类别.挑选使用"),
            // dict.logistics.judgment.category.sorting
            ("dict.logistics.judgment.category.sorting", "zh-CN", "挑选使用", "判定类别.挑选使用"),
            // dict.logistics.judgment.category.sorting
            ("dict.logistics.judgment.category.sorting", "zh-HK", "挑选使用_hk", "判定类别.挑选使用"),

            // dict.logistics.judgment.category.rework
            ("dict.logistics.judgment.category.rework", "en-US", "返工_us", "判定类别.返工"),
            // dict.logistics.judgment.category.rework
            ("dict.logistics.judgment.category.rework", "ja-JP", "返工_jp", "判定类别.返工"),
            // dict.logistics.judgment.category.rework
            ("dict.logistics.judgment.category.rework", "zh-CN", "返工", "判定类别.返工"),
            // dict.logistics.judgment.category.rework
            ("dict.logistics.judgment.category.rework", "zh-HK", "返工_hk", "判定类别.返工"),

            // dict.logistics.judgment.category.scrap
            ("dict.logistics.judgment.category.scrap", "en-US", "报废_us", "判定类别.报废"),
            // dict.logistics.judgment.category.scrap
            ("dict.logistics.judgment.category.scrap", "ja-JP", "报废_jp", "判定类别.报废"),
            // dict.logistics.judgment.category.scrap
            ("dict.logistics.judgment.category.scrap", "zh-CN", "报废", "判定类别.报废"),
            // dict.logistics.judgment.category.scrap
            ("dict.logistics.judgment.category.scrap", "zh-HK", "报废_hk", "判定类别.报废"),

            // dict.logistics.maintenance.category.preventive
            ("dict.logistics.maintenance.category.preventive", "en-US", "预防性维护_us", "维护类别.预防性维护"),
            // dict.logistics.maintenance.category.preventive
            ("dict.logistics.maintenance.category.preventive", "ja-JP", "预防性维护_jp", "维护类别.预防性维护"),
            // dict.logistics.maintenance.category.preventive
            ("dict.logistics.maintenance.category.preventive", "zh-CN", "预防性维护", "维护类别.预防性维护"),
            // dict.logistics.maintenance.category.preventive
            ("dict.logistics.maintenance.category.preventive", "zh-HK", "预防性维护_hk", "维护类别.预防性维护"),

            // dict.logistics.maintenance.category.corrective
            ("dict.logistics.maintenance.category.corrective", "en-US", "corrective维护_us", "维护类别. corrective维护"),
            // dict.logistics.maintenance.category.corrective
            ("dict.logistics.maintenance.category.corrective", "ja-JP", "corrective维护_jp", "维护类别. corrective维护"),
            // dict.logistics.maintenance.category.corrective
            ("dict.logistics.maintenance.category.corrective", "zh-CN", "corrective维护", "维护类别. corrective维护"),
            // dict.logistics.maintenance.category.corrective
            ("dict.logistics.maintenance.category.corrective", "zh-HK", "corrective维护_hk", "维护类别. corrective维护"),

            // dict.logistics.maintenance.category.predictive
            ("dict.logistics.maintenance.category.predictive", "en-US", "预测性维护_us", "维护类别.预测性维护"),
            // dict.logistics.maintenance.category.predictive
            ("dict.logistics.maintenance.category.predictive", "ja-JP", "预测性维护_jp", "维护类别.预测性维护"),
            // dict.logistics.maintenance.category.predictive
            ("dict.logistics.maintenance.category.predictive", "zh-CN", "预测性维护", "维护类别.预测性维护"),
            // dict.logistics.maintenance.category.predictive
            ("dict.logistics.maintenance.category.predictive", "zh-HK", "预测性维护_hk", "维护类别.预测性维护"),

            // dict.logistics.maintenance.category.emergency
            ("dict.logistics.maintenance.category.emergency", "en-US", "紧急维修_us", "维护类别.紧急维修"),
            // dict.logistics.maintenance.category.emergency
            ("dict.logistics.maintenance.category.emergency", "ja-JP", "紧急维修_jp", "维护类别.紧急维修"),
            // dict.logistics.maintenance.category.emergency
            ("dict.logistics.maintenance.category.emergency", "zh-CN", "紧急维修", "维护类别.紧急维修"),
            // dict.logistics.maintenance.category.emergency
            ("dict.logistics.maintenance.category.emergency", "zh-HK", "紧急维修_hk", "维护类别.紧急维修"),

            // dict.logistics.maintenance.category.regular
            ("dict.logistics.maintenance.category.regular", "en-US", "定期保养_us", "维护类别.定期保养"),
            // dict.logistics.maintenance.category.regular
            ("dict.logistics.maintenance.category.regular", "ja-JP", "定期保养_jp", "维护类别.定期保养"),
            // dict.logistics.maintenance.category.regular
            ("dict.logistics.maintenance.category.regular", "zh-CN", "定期保养", "维护类别.定期保养"),
            // dict.logistics.maintenance.category.regular
            ("dict.logistics.maintenance.category.regular", "zh-HK", "定期保养_hk", "维护类别.定期保养"),

            // dict.logistics.maintenance.category.overhaul
            ("dict.logistics.maintenance.category.overhaul", "en-US", "大修_us", "维护类别.大修"),
            // dict.logistics.maintenance.category.overhaul
            ("dict.logistics.maintenance.category.overhaul", "ja-JP", "大修_jp", "维护类别.大修"),
            // dict.logistics.maintenance.category.overhaul
            ("dict.logistics.maintenance.category.overhaul", "zh-CN", "大修", "维护类别.大修"),
            // dict.logistics.maintenance.category.overhaul
            ("dict.logistics.maintenance.category.overhaul", "zh-HK", "大修_hk", "维护类别.大修"),

            // dict.logistics.maintenance.category.upgrade
            ("dict.logistics.maintenance.category.upgrade", "en-US", "改造升级_us", "维护类别.改造升级"),
            // dict.logistics.maintenance.category.upgrade
            ("dict.logistics.maintenance.category.upgrade", "ja-JP", "改造升级_jp", "维护类别.改造升级"),
            // dict.logistics.maintenance.category.upgrade
            ("dict.logistics.maintenance.category.upgrade", "zh-CN", "改造升级", "维护类别.改造升级"),
            // dict.logistics.maintenance.category.upgrade
            ("dict.logistics.maintenance.category.upgrade", "zh-HK", "改造升级_hk", "维护类别.改造升级"),

            // dict.logistics.material.type.abf
            ("dict.logistics.material.type.abf", "en-US", "废料_us", "物料类型.废料"),
            // dict.logistics.material.type.abf
            ("dict.logistics.material.type.abf", "ja-JP", "废料_jp", "物料类型.废料"),
            // dict.logistics.material.type.abf
            ("dict.logistics.material.type.abf", "zh-CN", "废料", "物料类型.废料"),
            // dict.logistics.material.type.abf
            ("dict.logistics.material.type.abf", "zh-HK", "废料_hk", "物料类型.废料"),

            // dict.logistics.material.type.cbau
            ("dict.logistics.material.type.cbau", "en-US", "兼容设备_us", "物料类型.兼容设备"),
            // dict.logistics.material.type.cbau
            ("dict.logistics.material.type.cbau", "ja-JP", "兼容设备_jp", "物料类型.兼容设备"),
            // dict.logistics.material.type.cbau
            ("dict.logistics.material.type.cbau", "zh-CN", "兼容设备", "物料类型.兼容设备"),
            // dict.logistics.material.type.cbau
            ("dict.logistics.material.type.cbau", "zh-HK", "兼容设备_hk", "物料类型.兼容设备"),

            // dict.logistics.material.type.ch00
            ("dict.logistics.material.type.ch00", "en-US", "ch合同操作_us", "物料类型.ch合同操作"),
            // dict.logistics.material.type.ch00
            ("dict.logistics.material.type.ch00", "ja-JP", "ch合同操作_jp", "物料类型.ch合同操作"),
            // dict.logistics.material.type.ch00
            ("dict.logistics.material.type.ch00", "zh-CN", "ch合同操作", "物料类型.ch合同操作"),
            // dict.logistics.material.type.ch00
            ("dict.logistics.material.type.ch00", "zh-HK", "ch合同操作_hk", "物料类型.ch合同操作"),

            // dict.logistics.material.type.cont
            ("dict.logistics.material.type.cont", "en-US", "看板容器_us", "物料类型.看板容器"),
            // dict.logistics.material.type.cont
            ("dict.logistics.material.type.cont", "ja-JP", "看板容器_jp", "物料类型.看板容器"),
            // dict.logistics.material.type.cont
            ("dict.logistics.material.type.cont", "zh-CN", "看板容器", "物料类型.看板容器"),
            // dict.logistics.material.type.cont
            ("dict.logistics.material.type.cont", "zh-HK", "看板容器_hk", "物料类型.看板容器"),

            // dict.logistics.material.type.coup
            ("dict.logistics.material.type.coup", "en-US", "优惠券_us", "物料类型.优惠券"),
            // dict.logistics.material.type.coup
            ("dict.logistics.material.type.coup", "ja-JP", "优惠券_jp", "物料类型.优惠券"),
            // dict.logistics.material.type.coup
            ("dict.logistics.material.type.coup", "zh-CN", "优惠券", "物料类型.优惠券"),
            // dict.logistics.material.type.coup
            ("dict.logistics.material.type.coup", "zh-HK", "优惠券_hk", "物料类型.优惠券"),

            // dict.logistics.material.type.dien
            ("dict.logistics.material.type.dien", "en-US", "服务_us", "物料类型.服务"),
            // dict.logistics.material.type.dien
            ("dict.logistics.material.type.dien", "ja-JP", "服务_jp", "物料类型.服务"),
            // dict.logistics.material.type.dien
            ("dict.logistics.material.type.dien", "zh-CN", "服务", "物料类型.服务"),
            // dict.logistics.material.type.dien
            ("dict.logistics.material.type.dien", "zh-HK", "服务_hk", "物料类型.服务"),

            // dict.logistics.material.type.epa
            ("dict.logistics.material.type.epa", "en-US", "设备包装_us", "物料类型.设备包装"),
            // dict.logistics.material.type.epa
            ("dict.logistics.material.type.epa", "ja-JP", "设备包装_jp", "物料类型.设备包装"),
            // dict.logistics.material.type.epa
            ("dict.logistics.material.type.epa", "zh-CN", "设备包装", "物料类型.设备包装"),
            // dict.logistics.material.type.epa
            ("dict.logistics.material.type.epa", "zh-HK", "设备包装_hk", "物料类型.设备包装"),

            // dict.logistics.material.type.ersa
            ("dict.logistics.material.type.ersa", "en-US", "备件_us", "物料类型.备件"),
            // dict.logistics.material.type.ersa
            ("dict.logistics.material.type.ersa", "ja-JP", "备件_jp", "物料类型.备件"),
            // dict.logistics.material.type.ersa
            ("dict.logistics.material.type.ersa", "zh-CN", "备件", "物料类型.备件"),
            // dict.logistics.material.type.ersa
            ("dict.logistics.material.type.ersa", "zh-HK", "备件_hk", "物料类型.备件"),

            // dict.logistics.material.type.fert
            ("dict.logistics.material.type.fert", "en-US", "成品_us", "物料类型.成品"),
            // dict.logistics.material.type.fert
            ("dict.logistics.material.type.fert", "ja-JP", "成品_jp", "物料类型.成品"),
            // dict.logistics.material.type.fert
            ("dict.logistics.material.type.fert", "zh-CN", "成品", "物料类型.成品"),
            // dict.logistics.material.type.fert
            ("dict.logistics.material.type.fert", "zh-HK", "成品_hk", "物料类型.成品"),

            // dict.logistics.material.type.fgtr
            ("dict.logistics.material.type.fgtr", "en-US", "饮料_us", "物料类型.饮料"),
            // dict.logistics.material.type.fgtr
            ("dict.logistics.material.type.fgtr", "ja-JP", "饮料_jp", "物料类型.饮料"),
            // dict.logistics.material.type.fgtr
            ("dict.logistics.material.type.fgtr", "zh-CN", "饮料", "物料类型.饮料"),
            // dict.logistics.material.type.fgtr
            ("dict.logistics.material.type.fgtr", "zh-HK", "饮料_hk", "物料类型.饮料"),

            // dict.logistics.material.type.fhmi
            ("dict.logistics.material.type.fhmi", "en-US", "生产资源/工具_us", "物料类型.生产资源/工具"),
            // dict.logistics.material.type.fhmi
            ("dict.logistics.material.type.fhmi", "ja-JP", "生产资源/工具_jp", "物料类型.生产资源/工具"),
            // dict.logistics.material.type.fhmi
            ("dict.logistics.material.type.fhmi", "zh-CN", "生产资源/工具", "物料类型.生产资源/工具"),
            // dict.logistics.material.type.fhmi
            ("dict.logistics.material.type.fhmi", "zh-HK", "生产资源/工具_hk", "物料类型.生产资源/工具"),

            // dict.logistics.material.type.food
            ("dict.logistics.material.type.food", "en-US", "食品_us", "物料类型.食品"),
            // dict.logistics.material.type.food
            ("dict.logistics.material.type.food", "ja-JP", "食品_jp", "物料类型.食品"),
            // dict.logistics.material.type.food
            ("dict.logistics.material.type.food", "zh-CN", "食品", "物料类型.食品"),
            // dict.logistics.material.type.food
            ("dict.logistics.material.type.food", "zh-HK", "食品_hk", "物料类型.食品"),

            // dict.logistics.material.type.frip
            ("dict.logistics.material.type.frip", "en-US", "易腐品_us", "物料类型.易腐品"),
            // dict.logistics.material.type.frip
            ("dict.logistics.material.type.frip", "ja-JP", "易腐品_jp", "物料类型.易腐品"),
            // dict.logistics.material.type.frip
            ("dict.logistics.material.type.frip", "zh-CN", "易腐品", "物料类型.易腐品"),
            // dict.logistics.material.type.frip
            ("dict.logistics.material.type.frip", "zh-HK", "易腐品_hk", "物料类型.易腐品"),

            // dict.logistics.material.type.halb
            ("dict.logistics.material.type.halb", "en-US", "半成品_us", "物料类型.半成品"),
            // dict.logistics.material.type.halb
            ("dict.logistics.material.type.halb", "ja-JP", "半成品_jp", "物料类型.半成品"),
            // dict.logistics.material.type.halb
            ("dict.logistics.material.type.halb", "zh-CN", "半成品", "物料类型.半成品"),
            // dict.logistics.material.type.halb
            ("dict.logistics.material.type.halb", "zh-HK", "半成品_hk", "物料类型.半成品"),

            // dict.logistics.material.type.hawa
            ("dict.logistics.material.type.hawa", "en-US", "贸易货物_us", "物料类型.贸易货物"),
            // dict.logistics.material.type.hawa
            ("dict.logistics.material.type.hawa", "ja-JP", "贸易货物_jp", "物料类型.贸易货物"),
            // dict.logistics.material.type.hawa
            ("dict.logistics.material.type.hawa", "zh-CN", "贸易货物", "物料类型.贸易货物"),
            // dict.logistics.material.type.hawa
            ("dict.logistics.material.type.hawa", "zh-HK", "贸易货物_hk", "物料类型.贸易货物"),

            // dict.logistics.material.type.hers
            ("dict.logistics.material.type.hers", "en-US", "制造商部分_us", "物料类型.制造商部分"),
            // dict.logistics.material.type.hers
            ("dict.logistics.material.type.hers", "ja-JP", "制造商部分_jp", "物料类型.制造商部分"),
            // dict.logistics.material.type.hers
            ("dict.logistics.material.type.hers", "zh-CN", "制造商部分", "物料类型.制造商部分"),
            // dict.logistics.material.type.hers
            ("dict.logistics.material.type.hers", "zh-HK", "制造商部分_hk", "物料类型.制造商部分"),

            // dict.logistics.material.type.hibe
            ("dict.logistics.material.type.hibe", "en-US", "经营供应_us", "物料类型.经营供应"),
            // dict.logistics.material.type.hibe
            ("dict.logistics.material.type.hibe", "ja-JP", "经营供应_jp", "物料类型.经营供应"),
            // dict.logistics.material.type.hibe
            ("dict.logistics.material.type.hibe", "zh-CN", "经营供应", "物料类型.经营供应"),
            // dict.logistics.material.type.hibe
            ("dict.logistics.material.type.hibe", "zh-HK", "经营供应_hk", "物料类型.经营供应"),

            // dict.logistics.material.type.ibau
            ("dict.logistics.material.type.ibau", "en-US", "维护装配_us", "物料类型.维护装配"),
            // dict.logistics.material.type.ibau
            ("dict.logistics.material.type.ibau", "ja-JP", "维护装配_jp", "物料类型.维护装配"),
            // dict.logistics.material.type.ibau
            ("dict.logistics.material.type.ibau", "zh-CN", "维护装配", "物料类型.维护装配"),
            // dict.logistics.material.type.ibau
            ("dict.logistics.material.type.ibau", "zh-HK", "维护装配_hk", "物料类型.维护装配"),

            // dict.logistics.material.type.intr
            ("dict.logistics.material.type.intr", "en-US", "内部物料_us", "物料类型.内部物料"),
            // dict.logistics.material.type.intr
            ("dict.logistics.material.type.intr", "ja-JP", "内部物料_jp", "物料类型.内部物料"),
            // dict.logistics.material.type.intr
            ("dict.logistics.material.type.intr", "zh-CN", "内部物料", "物料类型.内部物料"),
            // dict.logistics.material.type.intr
            ("dict.logistics.material.type.intr", "zh-HK", "内部物料_hk", "物料类型.内部物料"),

            // dict.logistics.material.type.kmat
            ("dict.logistics.material.type.kmat", "en-US", "可配置物料_us", "物料类型.可配置物料"),
            // dict.logistics.material.type.kmat
            ("dict.logistics.material.type.kmat", "ja-JP", "可配置物料_jp", "物料类型.可配置物料"),
            // dict.logistics.material.type.kmat
            ("dict.logistics.material.type.kmat", "zh-CN", "可配置物料", "物料类型.可配置物料"),
            // dict.logistics.material.type.kmat
            ("dict.logistics.material.type.kmat", "zh-HK", "可配置物料_hk", "物料类型.可配置物料"),

            // dict.logistics.material.type.leer
            ("dict.logistics.material.type.leer", "en-US", "虚拟件_us", "物料类型.虚拟件"),
            // dict.logistics.material.type.leer
            ("dict.logistics.material.type.leer", "ja-JP", "虚拟件_jp", "物料类型.虚拟件"),
            // dict.logistics.material.type.leer
            ("dict.logistics.material.type.leer", "zh-CN", "虚拟件", "物料类型.虚拟件"),
            // dict.logistics.material.type.leer
            ("dict.logistics.material.type.leer", "zh-HK", "虚拟件_hk", "物料类型.虚拟件"),

            // dict.logistics.material.type.leih
            ("dict.logistics.material.type.leih", "en-US", "可反复利用包装_us", "物料类型.可反复利用包装"),
            // dict.logistics.material.type.leih
            ("dict.logistics.material.type.leih", "ja-JP", "可反复利用包装_jp", "物料类型.可反复利用包装"),
            // dict.logistics.material.type.leih
            ("dict.logistics.material.type.leih", "zh-CN", "可反复利用包装", "物料类型.可反复利用包装"),
            // dict.logistics.material.type.leih
            ("dict.logistics.material.type.leih", "zh-HK", "可反复利用包装_hk", "物料类型.可反复利用包装"),

            // dict.logistics.material.type.lgut
            ("dict.logistics.material.type.lgut", "en-US", "空零售_us", "物料类型.空零售"),
            // dict.logistics.material.type.lgut
            ("dict.logistics.material.type.lgut", "ja-JP", "空零售_jp", "物料类型.空零售"),
            // dict.logistics.material.type.lgut
            ("dict.logistics.material.type.lgut", "zh-CN", "空零售", "物料类型.空零售"),
            // dict.logistics.material.type.lgut
            ("dict.logistics.material.type.lgut", "zh-HK", "空零售_hk", "物料类型.空零售"),

            // dict.logistics.material.type.mode
            ("dict.logistics.material.type.mode", "en-US", "衣物_us", "物料类型.衣物"),
            // dict.logistics.material.type.mode
            ("dict.logistics.material.type.mode", "ja-JP", "衣物_jp", "物料类型.衣物"),
            // dict.logistics.material.type.mode
            ("dict.logistics.material.type.mode", "zh-CN", "衣物", "物料类型.衣物"),
            // dict.logistics.material.type.mode
            ("dict.logistics.material.type.mode", "zh-HK", "衣物_hk", "物料类型.衣物"),

            // dict.logistics.material.type.mpo
            ("dict.logistics.material.type.mpo", "en-US", "物料计划对象_us", "物料类型.物料计划对象"),
            // dict.logistics.material.type.mpo
            ("dict.logistics.material.type.mpo", "ja-JP", "物料计划对象_jp", "物料类型.物料计划对象"),
            // dict.logistics.material.type.mpo
            ("dict.logistics.material.type.mpo", "zh-CN", "物料计划对象", "物料类型.物料计划对象"),
            // dict.logistics.material.type.mpo
            ("dict.logistics.material.type.mpo", "zh-HK", "物料计划对象_hk", "物料类型.物料计划对象"),

            // dict.logistics.material.type.nlag
            ("dict.logistics.material.type.nlag", "en-US", "非存储物料_us", "物料类型.非存储物料"),
            // dict.logistics.material.type.nlag
            ("dict.logistics.material.type.nlag", "ja-JP", "非存储物料_jp", "物料类型.非存储物料"),
            // dict.logistics.material.type.nlag
            ("dict.logistics.material.type.nlag", "zh-CN", "非存储物料", "物料类型.非存储物料"),
            // dict.logistics.material.type.nlag
            ("dict.logistics.material.type.nlag", "zh-HK", "非存储物料_hk", "物料类型.非存储物料"),

            // dict.logistics.material.type.nof1
            ("dict.logistics.material.type.nof1", "en-US", "非食品_us", "物料类型.非食品"),
            // dict.logistics.material.type.nof1
            ("dict.logistics.material.type.nof1", "ja-JP", "非食品_jp", "物料类型.非食品"),
            // dict.logistics.material.type.nof1
            ("dict.logistics.material.type.nof1", "zh-CN", "非食品", "物料类型.非食品"),
            // dict.logistics.material.type.nof1
            ("dict.logistics.material.type.nof1", "zh-HK", "非食品_hk", "物料类型.非食品"),

            // dict.logistics.material.type.pipe
            ("dict.logistics.material.type.pipe", "en-US", "管线物料_us", "物料类型.管线物料"),
            // dict.logistics.material.type.pipe
            ("dict.logistics.material.type.pipe", "ja-JP", "管线物料_jp", "物料类型.管线物料"),
            // dict.logistics.material.type.pipe
            ("dict.logistics.material.type.pipe", "zh-CN", "管线物料", "物料类型.管线物料"),
            // dict.logistics.material.type.pipe
            ("dict.logistics.material.type.pipe", "zh-HK", "管线物料_hk", "物料类型.管线物料"),

            // dict.logistics.material.type.plan
            ("dict.logistics.material.type.plan", "en-US", "贸易货物计划_us", "物料类型.贸易货物计划"),
            // dict.logistics.material.type.plan
            ("dict.logistics.material.type.plan", "ja-JP", "贸易货物计划_jp", "物料类型.贸易货物计划"),
            // dict.logistics.material.type.plan
            ("dict.logistics.material.type.plan", "zh-CN", "贸易货物计划", "物料类型.贸易货物计划"),
            // dict.logistics.material.type.plan
            ("dict.logistics.material.type.plan", "zh-HK", "贸易货物计划_hk", "物料类型.贸易货物计划"),

            // dict.logistics.material.type.proc
            ("dict.logistics.material.type.proc", "en-US", "过程物料_us", "物料类型.过程物料"),
            // dict.logistics.material.type.proc
            ("dict.logistics.material.type.proc", "ja-JP", "过程物料_jp", "物料类型.过程物料"),
            // dict.logistics.material.type.proc
            ("dict.logistics.material.type.proc", "zh-CN", "过程物料", "物料类型.过程物料"),
            // dict.logistics.material.type.proc
            ("dict.logistics.material.type.proc", "zh-HK", "过程物料_hk", "物料类型.过程物料"),

            // dict.logistics.material.type.prod
            ("dict.logistics.material.type.prod", "en-US", "产品组_us", "物料类型.产品组"),
            // dict.logistics.material.type.prod
            ("dict.logistics.material.type.prod", "ja-JP", "产品组_jp", "物料类型.产品组"),
            // dict.logistics.material.type.prod
            ("dict.logistics.material.type.prod", "zh-CN", "产品组", "物料类型.产品组"),
            // dict.logistics.material.type.prod
            ("dict.logistics.material.type.prod", "zh-HK", "产品组_hk", "物料类型.产品组"),

            // dict.logistics.material.type.roh
            ("dict.logistics.material.type.roh", "en-US", "原材料_us", "物料类型.原材料"),
            // dict.logistics.material.type.roh
            ("dict.logistics.material.type.roh", "ja-JP", "原材料_jp", "物料类型.原材料"),
            // dict.logistics.material.type.roh
            ("dict.logistics.material.type.roh", "zh-CN", "原材料", "物料类型.原材料"),
            // dict.logistics.material.type.roh
            ("dict.logistics.material.type.roh", "zh-HK", "原材料_hk", "物料类型.原材料"),

            // dict.logistics.material.type.unbw
            ("dict.logistics.material.type.unbw", "en-US", "未估价物料_us", "物料类型.未估价物料"),
            // dict.logistics.material.type.unbw
            ("dict.logistics.material.type.unbw", "ja-JP", "未估价物料_jp", "物料类型.未估价物料"),
            // dict.logistics.material.type.unbw
            ("dict.logistics.material.type.unbw", "zh-CN", "未估价物料", "物料类型.未估价物料"),
            // dict.logistics.material.type.unbw
            ("dict.logistics.material.type.unbw", "zh-HK", "未估价物料_hk", "物料类型.未估价物料"),

            // dict.logistics.material.type.verp
            ("dict.logistics.material.type.verp", "en-US", "包装_us", "物料类型.包装"),
            // dict.logistics.material.type.verp
            ("dict.logistics.material.type.verp", "ja-JP", "包装_jp", "物料类型.包装"),
            // dict.logistics.material.type.verp
            ("dict.logistics.material.type.verp", "zh-CN", "包装", "物料类型.包装"),
            // dict.logistics.material.type.verp
            ("dict.logistics.material.type.verp", "zh-HK", "包装_hk", "物料类型.包装"),

            // dict.logistics.material.type.vkhm
            ("dict.logistics.material.type.vkhm", "en-US", "附加_us", "物料类型.附加"),
            // dict.logistics.material.type.vkhm
            ("dict.logistics.material.type.vkhm", "ja-JP", "附加_jp", "物料类型.附加"),
            // dict.logistics.material.type.vkhm
            ("dict.logistics.material.type.vkhm", "zh-CN", "附加", "物料类型.附加"),
            // dict.logistics.material.type.vkhm
            ("dict.logistics.material.type.vkhm", "zh-HK", "附加_hk", "物料类型.附加"),

            // dict.logistics.material.type.voll
            ("dict.logistics.material.type.voll", "en-US", "全部产品_us", "物料类型.全部产品"),
            // dict.logistics.material.type.voll
            ("dict.logistics.material.type.voll", "ja-JP", "全部产品_jp", "物料类型.全部产品"),
            // dict.logistics.material.type.voll
            ("dict.logistics.material.type.voll", "zh-CN", "全部产品", "物料类型.全部产品"),
            // dict.logistics.material.type.voll
            ("dict.logistics.material.type.voll", "zh-HK", "全部产品_hk", "物料类型.全部产品"),

            // dict.logistics.material.type.werb
            ("dict.logistics.material.type.werb", "en-US", "产品目录_us", "物料类型.产品目录"),
            // dict.logistics.material.type.werb
            ("dict.logistics.material.type.werb", "ja-JP", "产品目录_jp", "物料类型.产品目录"),
            // dict.logistics.material.type.werb
            ("dict.logistics.material.type.werb", "zh-CN", "产品目录", "物料类型.产品目录"),
            // dict.logistics.material.type.werb
            ("dict.logistics.material.type.werb", "zh-HK", "产品目录_hk", "物料类型.产品目录"),

            // dict.logistics.material.type.wert
            ("dict.logistics.material.type.wert", "en-US", "只有价值物料_us", "物料类型.只有价值物料"),
            // dict.logistics.material.type.wert
            ("dict.logistics.material.type.wert", "ja-JP", "只有价值物料_jp", "物料类型.只有价值物料"),
            // dict.logistics.material.type.wert
            ("dict.logistics.material.type.wert", "zh-CN", "只有价值物料", "物料类型.只有价值物料"),
            // dict.logistics.material.type.wert
            ("dict.logistics.material.type.wert", "zh-HK", "只有价值物料_hk", "物料类型.只有价值物料"),

            // dict.logistics.material.type.wett
            ("dict.logistics.material.type.wett", "en-US", "竞争产品_us", "物料类型.竞争产品"),
            // dict.logistics.material.type.wett
            ("dict.logistics.material.type.wett", "ja-JP", "竞争产品_jp", "物料类型.竞争产品"),
            // dict.logistics.material.type.wett
            ("dict.logistics.material.type.wett", "zh-CN", "竞争产品", "物料类型.竞争产品"),
            // dict.logistics.material.type.wett
            ("dict.logistics.material.type.wett", "zh-HK", "竞争产品_hk", "物料类型.竞争产品"),

            // dict.logistics.planned.delivery.days.param.7
            ("dict.logistics.planned.delivery.days.param.7", "en-US", "7天_us", "计划交货天数.7天"),
            // dict.logistics.planned.delivery.days.param.7
            ("dict.logistics.planned.delivery.days.param.7", "ja-JP", "7天_jp", "计划交货天数.7天"),
            // dict.logistics.planned.delivery.days.param.7
            ("dict.logistics.planned.delivery.days.param.7", "zh-CN", "7天", "计划交货天数.7天"),
            // dict.logistics.planned.delivery.days.param.7
            ("dict.logistics.planned.delivery.days.param.7", "zh-HK", "7天_hk", "计划交货天数.7天"),

            // dict.logistics.planned.delivery.days.param.30
            ("dict.logistics.planned.delivery.days.param.30", "en-US", "30天_us", "计划交货天数.30天"),
            // dict.logistics.planned.delivery.days.param.30
            ("dict.logistics.planned.delivery.days.param.30", "ja-JP", "30天_jp", "计划交货天数.30天"),
            // dict.logistics.planned.delivery.days.param.30
            ("dict.logistics.planned.delivery.days.param.30", "zh-CN", "30天", "计划交货天数.30天"),
            // dict.logistics.planned.delivery.days.param.30
            ("dict.logistics.planned.delivery.days.param.30", "zh-HK", "30天_hk", "计划交货天数.30天"),

            // dict.logistics.planned.delivery.days.param.60
            ("dict.logistics.planned.delivery.days.param.60", "en-US", "60天_us", "计划交货天数.60天"),
            // dict.logistics.planned.delivery.days.param.60
            ("dict.logistics.planned.delivery.days.param.60", "ja-JP", "60天_jp", "计划交货天数.60天"),
            // dict.logistics.planned.delivery.days.param.60
            ("dict.logistics.planned.delivery.days.param.60", "zh-CN", "60天", "计划交货天数.60天"),
            // dict.logistics.planned.delivery.days.param.60
            ("dict.logistics.planned.delivery.days.param.60", "zh-HK", "60天_hk", "计划交货天数.60天"),

            // dict.logistics.planned.delivery.days.param.90
            ("dict.logistics.planned.delivery.days.param.90", "en-US", "90天_us", "计划交货天数.90天"),
            // dict.logistics.planned.delivery.days.param.90
            ("dict.logistics.planned.delivery.days.param.90", "ja-JP", "90天_jp", "计划交货天数.90天"),
            // dict.logistics.planned.delivery.days.param.90
            ("dict.logistics.planned.delivery.days.param.90", "zh-CN", "90天", "计划交货天数.90天"),
            // dict.logistics.planned.delivery.days.param.90
            ("dict.logistics.planned.delivery.days.param.90", "zh-HK", "90天_hk", "计划交货天数.90天"),

            // dict.logistics.planned.delivery.days.param.120
            ("dict.logistics.planned.delivery.days.param.120", "en-US", "120天_us", "计划交货天数.120天"),
            // dict.logistics.planned.delivery.days.param.120
            ("dict.logistics.planned.delivery.days.param.120", "ja-JP", "120天_jp", "计划交货天数.120天"),
            // dict.logistics.planned.delivery.days.param.120
            ("dict.logistics.planned.delivery.days.param.120", "zh-CN", "120天", "计划交货天数.120天"),
            // dict.logistics.planned.delivery.days.param.120
            ("dict.logistics.planned.delivery.days.param.120", "zh-HK", "120天_hk", "计划交货天数.120天"),

            // dict.logistics.price.control.type.s
            ("dict.logistics.price.control.type.s", "en-US", "标准价格_us", "价格控制.标准价格"),
            // dict.logistics.price.control.type.s
            ("dict.logistics.price.control.type.s", "ja-JP", "标准价格_jp", "价格控制.标准价格"),
            // dict.logistics.price.control.type.s
            ("dict.logistics.price.control.type.s", "zh-CN", "标准价格", "价格控制.标准价格"),
            // dict.logistics.price.control.type.s
            ("dict.logistics.price.control.type.s", "zh-HK", "标准价格_hk", "价格控制.标准价格"),

            // dict.logistics.price.control.type.v
            ("dict.logistics.price.control.type.v", "en-US", "移动平均价_us", "价格控制.移动平均价"),
            // dict.logistics.price.control.type.v
            ("dict.logistics.price.control.type.v", "ja-JP", "移动平均价_jp", "价格控制.移动平均价"),
            // dict.logistics.price.control.type.v
            ("dict.logistics.price.control.type.v", "zh-CN", "移动平均价", "价格控制.移动平均价"),
            // dict.logistics.price.control.type.v
            ("dict.logistics.price.control.type.v", "zh-HK", "移动平均价_hk", "价格控制.移动平均价"),

            // dict.logistics.price.type.0
            ("dict.logistics.price.type.0", "en-US", "标准价格_us", "价格类型.标准价格"),
            // dict.logistics.price.type.0
            ("dict.logistics.price.type.0", "ja-JP", "标准价格_jp", "价格类型.标准价格"),
            // dict.logistics.price.type.0
            ("dict.logistics.price.type.0", "zh-CN", "标准价格", "价格类型.标准价格"),
            // dict.logistics.price.type.0
            ("dict.logistics.price.type.0", "zh-HK", "标准价格_hk", "价格类型.标准价格"),

            // dict.logistics.price.type.1
            ("dict.logistics.price.type.1", "en-US", "合同价格_us", "价格类型.合同价格"),
            // dict.logistics.price.type.1
            ("dict.logistics.price.type.1", "ja-JP", "合同价格_jp", "价格类型.合同价格"),
            // dict.logistics.price.type.1
            ("dict.logistics.price.type.1", "zh-CN", "合同价格", "价格类型.合同价格"),
            // dict.logistics.price.type.1
            ("dict.logistics.price.type.1", "zh-HK", "合同价格_hk", "价格类型.合同价格"),

            // dict.logistics.price.type.2
            ("dict.logistics.price.type.2", "en-US", "临时价格_us", "价格类型.临时价格"),
            // dict.logistics.price.type.2
            ("dict.logistics.price.type.2", "ja-JP", "临时价格_jp", "价格类型.临时价格"),
            // dict.logistics.price.type.2
            ("dict.logistics.price.type.2", "zh-CN", "临时价格", "价格类型.临时价格"),
            // dict.logistics.price.type.2
            ("dict.logistics.price.type.2", "zh-HK", "临时价格_hk", "价格类型.临时价格"),

            // dict.logistics.price.type.3
            ("dict.logistics.price.type.3", "en-US", "询价价格_us", "价格类型.询价价格"),
            // dict.logistics.price.type.3
            ("dict.logistics.price.type.3", "ja-JP", "询价价格_jp", "价格类型.询价价格"),
            // dict.logistics.price.type.3
            ("dict.logistics.price.type.3", "zh-CN", "询价价格", "价格类型.询价价格"),
            // dict.logistics.price.type.3
            ("dict.logistics.price.type.3", "zh-HK", "询价价格_hk", "价格类型.询价价格"),

            // dict.logistics.price.type.4
            ("dict.logistics.price.type.4", "en-US", "历史价格_us", "价格类型.历史价格"),
            // dict.logistics.price.type.4
            ("dict.logistics.price.type.4", "ja-JP", "历史价格_jp", "价格类型.历史价格"),
            // dict.logistics.price.type.4
            ("dict.logistics.price.type.4", "zh-CN", "历史价格", "价格类型.历史价格"),
            // dict.logistics.price.type.4
            ("dict.logistics.price.type.4", "zh-HK", "历史价格_hk", "价格类型.历史价格"),

            // dict.logistics.price.type.5
            ("dict.logistics.price.type.5", "en-US", "客户价格_us", "价格类型.客户价格"),
            // dict.logistics.price.type.5
            ("dict.logistics.price.type.5", "ja-JP", "客户价格_jp", "价格类型.客户价格"),
            // dict.logistics.price.type.5
            ("dict.logistics.price.type.5", "zh-CN", "客户价格", "价格类型.客户价格"),
            // dict.logistics.price.type.5
            ("dict.logistics.price.type.5", "zh-HK", "客户价格_hk", "价格类型.客户价格"),

            // dict.logistics.price.type.6
            ("dict.logistics.price.type.6", "en-US", "促销价格_us", "价格类型.促销价格"),
            // dict.logistics.price.type.6
            ("dict.logistics.price.type.6", "ja-JP", "促销价格_jp", "价格类型.促销价格"),
            // dict.logistics.price.type.6
            ("dict.logistics.price.type.6", "zh-CN", "促销价格", "价格类型.促销价格"),
            // dict.logistics.price.type.6
            ("dict.logistics.price.type.6", "zh-HK", "促销价格_hk", "价格类型.促销价格"),

            // dict.logistics.price.type.7
            ("dict.logistics.price.type.7", "en-US", "成本价_us", "价格类型.成本价"),
            // dict.logistics.price.type.7
            ("dict.logistics.price.type.7", "ja-JP", "成本价_jp", "价格类型.成本价"),
            // dict.logistics.price.type.7
            ("dict.logistics.price.type.7", "zh-CN", "成本价", "价格类型.成本价"),
            // dict.logistics.price.type.7
            ("dict.logistics.price.type.7", "zh-HK", "成本价_hk", "价格类型.成本价"),

            // dict.logistics.price.type.8
            ("dict.logistics.price.type.8", "en-US", "批发价_us", "价格类型.批发价"),
            // dict.logistics.price.type.8
            ("dict.logistics.price.type.8", "ja-JP", "批发价_jp", "价格类型.批发价"),
            // dict.logistics.price.type.8
            ("dict.logistics.price.type.8", "zh-CN", "批发价", "价格类型.批发价"),
            // dict.logistics.price.type.8
            ("dict.logistics.price.type.8", "zh-HK", "批发价_hk", "价格类型.批发价"),

            // dict.logistics.price.type.9
            ("dict.logistics.price.type.9", "en-US", "零售价_us", "价格类型.零售价"),
            // dict.logistics.price.type.9
            ("dict.logistics.price.type.9", "ja-JP", "零售价_jp", "价格类型.零售价"),
            // dict.logistics.price.type.9
            ("dict.logistics.price.type.9", "zh-CN", "零售价", "价格类型.零售价"),
            // dict.logistics.price.type.9
            ("dict.logistics.price.type.9", "zh-HK", "零售价_hk", "价格类型.零售价"),

            // dict.logistics.price.type.10
            ("dict.logistics.price.type.10", "en-US", "协议价_us", "价格类型.协议价"),
            // dict.logistics.price.type.10
            ("dict.logistics.price.type.10", "ja-JP", "协议价_jp", "价格类型.协议价"),
            // dict.logistics.price.type.10
            ("dict.logistics.price.type.10", "zh-CN", "协议价", "价格类型.协议价"),
            // dict.logistics.price.type.10
            ("dict.logistics.price.type.10", "zh-HK", "协议价_hk", "价格类型.协议价"),

            // dict.logistics.price.unit.param.1
            ("dict.logistics.price.unit.param.1", "en-US", "1_us", "价格单位.1"),
            // dict.logistics.price.unit.param.1
            ("dict.logistics.price.unit.param.1", "ja-JP", "1_jp", "价格单位.1"),
            // dict.logistics.price.unit.param.1
            ("dict.logistics.price.unit.param.1", "zh-CN", "1", "价格单位.1"),
            // dict.logistics.price.unit.param.1
            ("dict.logistics.price.unit.param.1", "zh-HK", "1_hk", "价格单位.1"),

            // dict.logistics.price.unit.param.10
            ("dict.logistics.price.unit.param.10", "en-US", "10_us", "价格单位.10"),
            // dict.logistics.price.unit.param.10
            ("dict.logistics.price.unit.param.10", "ja-JP", "10_jp", "价格单位.10"),
            // dict.logistics.price.unit.param.10
            ("dict.logistics.price.unit.param.10", "zh-CN", "10", "价格单位.10"),
            // dict.logistics.price.unit.param.10
            ("dict.logistics.price.unit.param.10", "zh-HK", "10_hk", "价格单位.10"),

            // dict.logistics.price.unit.param.100
            ("dict.logistics.price.unit.param.100", "en-US", "100_us", "价格单位.100"),
            // dict.logistics.price.unit.param.100
            ("dict.logistics.price.unit.param.100", "ja-JP", "100_jp", "价格单位.100"),
            // dict.logistics.price.unit.param.100
            ("dict.logistics.price.unit.param.100", "zh-CN", "100", "价格单位.100"),
            // dict.logistics.price.unit.param.100
            ("dict.logistics.price.unit.param.100", "zh-HK", "100_hk", "价格单位.100"),

            // dict.logistics.price.unit.param.1000
            ("dict.logistics.price.unit.param.1000", "en-US", "1000_us", "价格单位.1000"),
            // dict.logistics.price.unit.param.1000
            ("dict.logistics.price.unit.param.1000", "ja-JP", "1000_jp", "价格单位.1000"),
            // dict.logistics.price.unit.param.1000
            ("dict.logistics.price.unit.param.1000", "zh-CN", "1000", "价格单位.1000"),
            // dict.logistics.price.unit.param.1000
            ("dict.logistics.price.unit.param.1000", "zh-HK", "1000_hk", "价格单位.1000"),

            // dict.logistics.procurement.type.e
            ("dict.logistics.procurement.type.e", "en-US", "自制生产_us", "采购类别.自制生产"),
            // dict.logistics.procurement.type.e
            ("dict.logistics.procurement.type.e", "ja-JP", "自制生产_jp", "采购类别.自制生产"),
            // dict.logistics.procurement.type.e
            ("dict.logistics.procurement.type.e", "zh-CN", "自制生产", "采购类别.自制生产"),
            // dict.logistics.procurement.type.e
            ("dict.logistics.procurement.type.e", "zh-HK", "自制生产_hk", "采购类别.自制生产"),

            // dict.logistics.procurement.type.f
            ("dict.logistics.procurement.type.f", "en-US", "外部采购_us", "采购类别.外部采购"),
            // dict.logistics.procurement.type.f
            ("dict.logistics.procurement.type.f", "ja-JP", "外部采购_jp", "采购类别.外部采购"),
            // dict.logistics.procurement.type.f
            ("dict.logistics.procurement.type.f", "zh-CN", "外部采购", "采购类别.外部采购"),
            // dict.logistics.procurement.type.f
            ("dict.logistics.procurement.type.f", "zh-HK", "外部采购_hk", "采购类别.外部采购"),

            // dict.logistics.procurement.type.x
            ("dict.logistics.procurement.type.x", "en-US", "两种采购类型_us", "采购类别.两种采购类型"),
            // dict.logistics.procurement.type.x
            ("dict.logistics.procurement.type.x", "ja-JP", "两种采购类型_jp", "采购类别.两种采购类型"),
            // dict.logistics.procurement.type.x
            ("dict.logistics.procurement.type.x", "zh-CN", "两种采购类型", "采购类别.两种采购类型"),
            // dict.logistics.procurement.type.x
            ("dict.logistics.procurement.type.x", "zh-HK", "两种采购类型_hk", "采购类别.两种采购类型"),

            // dict.logistics.sampling.scheme.type.gb2828
            ("dict.logistics.sampling.scheme.type.gb2828", "en-US", "gb/t 2828.1_us", "抽样方案类型.gb/t 2828.1"),
            // dict.logistics.sampling.scheme.type.gb2828
            ("dict.logistics.sampling.scheme.type.gb2828", "ja-JP", "gb/t 2828.1_jp", "抽样方案类型.gb/t 2828.1"),
            // dict.logistics.sampling.scheme.type.gb2828
            ("dict.logistics.sampling.scheme.type.gb2828", "zh-CN", "gb/t 2828.1", "抽样方案类型.gb/t 2828.1"),
            // dict.logistics.sampling.scheme.type.gb2828
            ("dict.logistics.sampling.scheme.type.gb2828", "zh-HK", "gb/t 2828.1_hk", "抽样方案类型.gb/t 2828.1"),

            // dict.logistics.sampling.scheme.type.mil105e
            ("dict.logistics.sampling.scheme.type.mil105e", "en-US", "mil-std-105e_us", "抽样方案类型.mil-std-105e"),
            // dict.logistics.sampling.scheme.type.mil105e
            ("dict.logistics.sampling.scheme.type.mil105e", "ja-JP", "mil-std-105e_jp", "抽样方案类型.mil-std-105e"),
            // dict.logistics.sampling.scheme.type.mil105e
            ("dict.logistics.sampling.scheme.type.mil105e", "zh-CN", "mil-std-105e", "抽样方案类型.mil-std-105e"),
            // dict.logistics.sampling.scheme.type.mil105e
            ("dict.logistics.sampling.scheme.type.mil105e", "zh-HK", "mil-std-105e_hk", "抽样方案类型.mil-std-105e"),

            // dict.logistics.sampling.scheme.type.iso2859
            ("dict.logistics.sampling.scheme.type.iso2859", "en-US", "iso 2859-1_us", "抽样方案类型.iso 2859-1"),
            // dict.logistics.sampling.scheme.type.iso2859
            ("dict.logistics.sampling.scheme.type.iso2859", "ja-JP", "iso 2859-1_jp", "抽样方案类型.iso 2859-1"),
            // dict.logistics.sampling.scheme.type.iso2859
            ("dict.logistics.sampling.scheme.type.iso2859", "zh-CN", "iso 2859-1", "抽样方案类型.iso 2859-1"),
            // dict.logistics.sampling.scheme.type.iso2859
            ("dict.logistics.sampling.scheme.type.iso2859", "zh-HK", "iso 2859-1_hk", "抽样方案类型.iso 2859-1"),

            // dict.logistics.sampling.scheme.type.gb2829
            ("dict.logistics.sampling.scheme.type.gb2829", "en-US", "gb/t 2829_us", "抽样方案类型.gb/t 2829"),
            // dict.logistics.sampling.scheme.type.gb2829
            ("dict.logistics.sampling.scheme.type.gb2829", "ja-JP", "gb/t 2829_jp", "抽样方案类型.gb/t 2829"),
            // dict.logistics.sampling.scheme.type.gb2829
            ("dict.logistics.sampling.scheme.type.gb2829", "zh-CN", "gb/t 2829", "抽样方案类型.gb/t 2829"),
            // dict.logistics.sampling.scheme.type.gb2829
            ("dict.logistics.sampling.scheme.type.gb2829", "zh-HK", "gb/t 2829_hk", "抽样方案类型.gb/t 2829"),

            // dict.logistics.sampling.scheme.type.c_zero
            ("dict.logistics.sampling.scheme.type.c_zero", "en-US", "c=0抽样_us", "抽样方案类型.c=0抽样"),
            // dict.logistics.sampling.scheme.type.c_zero
            ("dict.logistics.sampling.scheme.type.c_zero", "ja-JP", "c=0抽样_jp", "抽样方案类型.c=0抽样"),
            // dict.logistics.sampling.scheme.type.c_zero
            ("dict.logistics.sampling.scheme.type.c_zero", "zh-CN", "c=0抽样", "抽样方案类型.c=0抽样"),
            // dict.logistics.sampling.scheme.type.c_zero
            ("dict.logistics.sampling.scheme.type.c_zero", "zh-HK", "c=0抽样_hk", "抽样方案类型.c=0抽样"),

            // dict.logistics.sampling.scheme.type.continuous
            ("dict.logistics.sampling.scheme.type.continuous", "en-US", "连续抽样_us", "抽样方案类型.连续抽样"),
            // dict.logistics.sampling.scheme.type.continuous
            ("dict.logistics.sampling.scheme.type.continuous", "ja-JP", "连续抽样_jp", "抽样方案类型.连续抽样"),
            // dict.logistics.sampling.scheme.type.continuous
            ("dict.logistics.sampling.scheme.type.continuous", "zh-CN", "连续抽样", "抽样方案类型.连续抽样"),
            // dict.logistics.sampling.scheme.type.continuous
            ("dict.logistics.sampling.scheme.type.continuous", "zh-HK", "连续抽样_hk", "抽样方案类型.连续抽样"),

            // dict.logistics.sampling.scheme.type.skip_lot
            ("dict.logistics.sampling.scheme.type.skip_lot", "en-US", "跳批抽样_us", "抽样方案类型.跳批抽样"),
            // dict.logistics.sampling.scheme.type.skip_lot
            ("dict.logistics.sampling.scheme.type.skip_lot", "ja-JP", "跳批抽样_jp", "抽样方案类型.跳批抽样"),
            // dict.logistics.sampling.scheme.type.skip_lot
            ("dict.logistics.sampling.scheme.type.skip_lot", "zh-CN", "跳批抽样", "抽样方案类型.跳批抽样"),
            // dict.logistics.sampling.scheme.type.skip_lot
            ("dict.logistics.sampling.scheme.type.skip_lot", "zh-HK", "跳批抽样_hk", "抽样方案类型.跳批抽样"),

            // dict.logistics.special.procurement.type.10
            ("dict.logistics.special.procurement.type.10", "en-US", "寄售_us", "特殊采购类别.寄售"),
            // dict.logistics.special.procurement.type.10
            ("dict.logistics.special.procurement.type.10", "ja-JP", "寄售_jp", "特殊采购类别.寄售"),
            // dict.logistics.special.procurement.type.10
            ("dict.logistics.special.procurement.type.10", "zh-CN", "寄售", "特殊采购类别.寄售"),
            // dict.logistics.special.procurement.type.10
            ("dict.logistics.special.procurement.type.10", "zh-HK", "寄售_hk", "特殊采购类别.寄售"),

            // dict.logistics.special.procurement.type.30
            ("dict.logistics.special.procurement.type.30", "en-US", "外协加工_us", "特殊采购类别.外协加工"),
            // dict.logistics.special.procurement.type.30
            ("dict.logistics.special.procurement.type.30", "ja-JP", "外协加工_jp", "特殊采购类别.外协加工"),
            // dict.logistics.special.procurement.type.30
            ("dict.logistics.special.procurement.type.30", "zh-CN", "外协加工", "特殊采购类别.外协加工"),
            // dict.logistics.special.procurement.type.30
            ("dict.logistics.special.procurement.type.30", "zh-HK", "外协加工_hk", "特殊采购类别.外协加工"),

            // dict.logistics.special.procurement.type.50
            ("dict.logistics.special.procurement.type.50", "en-US", "虚设品号_us", "特殊采购类别.虚设品号"),
            // dict.logistics.special.procurement.type.50
            ("dict.logistics.special.procurement.type.50", "ja-JP", "虚设品号_jp", "特殊采购类别.虚设品号"),
            // dict.logistics.special.procurement.type.50
            ("dict.logistics.special.procurement.type.50", "zh-CN", "虚设品号", "特殊采购类别.虚设品号"),
            // dict.logistics.special.procurement.type.50
            ("dict.logistics.special.procurement.type.50", "zh-HK", "虚设品号_hk", "特殊采购类别.虚设品号"),

            // dict.logistics.unit.of.measure.code.pc
            ("dict.logistics.unit.of.measure.code.pc", "en-US", "件_us", "基本单位类别.件"),
            // dict.logistics.unit.of.measure.code.pc
            ("dict.logistics.unit.of.measure.code.pc", "ja-JP", "件_jp", "基本单位类别.件"),
            // dict.logistics.unit.of.measure.code.pc
            ("dict.logistics.unit.of.measure.code.pc", "zh-CN", "件", "基本单位类别.件"),
            // dict.logistics.unit.of.measure.code.pc
            ("dict.logistics.unit.of.measure.code.pc", "zh-HK", "件_hk", "基本单位类别.件"),

            // dict.logistics.unit.of.measure.code.kg
            ("dict.logistics.unit.of.measure.code.kg", "en-US", "千克_us", "基本单位类别.千克"),
            // dict.logistics.unit.of.measure.code.kg
            ("dict.logistics.unit.of.measure.code.kg", "ja-JP", "千克_jp", "基本单位类别.千克"),
            // dict.logistics.unit.of.measure.code.kg
            ("dict.logistics.unit.of.measure.code.kg", "zh-CN", "千克", "基本单位类别.千克"),
            // dict.logistics.unit.of.measure.code.kg
            ("dict.logistics.unit.of.measure.code.kg", "zh-HK", "千克_hk", "基本单位类别.千克"),

            // dict.logistics.unit.of.measure.code.g
            ("dict.logistics.unit.of.measure.code.g", "en-US", "克_us", "基本单位类别.克"),
            // dict.logistics.unit.of.measure.code.g
            ("dict.logistics.unit.of.measure.code.g", "ja-JP", "克_jp", "基本单位类别.克"),
            // dict.logistics.unit.of.measure.code.g
            ("dict.logistics.unit.of.measure.code.g", "zh-CN", "克", "基本单位类别.克"),
            // dict.logistics.unit.of.measure.code.g
            ("dict.logistics.unit.of.measure.code.g", "zh-HK", "克_hk", "基本单位类别.克"),

            // dict.logistics.unit.of.measure.code.t
            ("dict.logistics.unit.of.measure.code.t", "en-US", "吨_us", "基本单位类别.吨"),
            // dict.logistics.unit.of.measure.code.t
            ("dict.logistics.unit.of.measure.code.t", "ja-JP", "吨_jp", "基本单位类别.吨"),
            // dict.logistics.unit.of.measure.code.t
            ("dict.logistics.unit.of.measure.code.t", "zh-CN", "吨", "基本单位类别.吨"),
            // dict.logistics.unit.of.measure.code.t
            ("dict.logistics.unit.of.measure.code.t", "zh-HK", "吨_hk", "基本单位类别.吨"),

            // dict.logistics.unit.of.measure.code.m
            ("dict.logistics.unit.of.measure.code.m", "en-US", "米_us", "基本单位类别.米"),
            // dict.logistics.unit.of.measure.code.m
            ("dict.logistics.unit.of.measure.code.m", "ja-JP", "米_jp", "基本单位类别.米"),
            // dict.logistics.unit.of.measure.code.m
            ("dict.logistics.unit.of.measure.code.m", "zh-CN", "米", "基本单位类别.米"),
            // dict.logistics.unit.of.measure.code.m
            ("dict.logistics.unit.of.measure.code.m", "zh-HK", "米_hk", "基本单位类别.米"),

            // dict.logistics.unit.of.measure.code.cm
            ("dict.logistics.unit.of.measure.code.cm", "en-US", "厘米_us", "基本单位类别.厘米"),
            // dict.logistics.unit.of.measure.code.cm
            ("dict.logistics.unit.of.measure.code.cm", "ja-JP", "厘米_jp", "基本单位类别.厘米"),
            // dict.logistics.unit.of.measure.code.cm
            ("dict.logistics.unit.of.measure.code.cm", "zh-CN", "厘米", "基本单位类别.厘米"),
            // dict.logistics.unit.of.measure.code.cm
            ("dict.logistics.unit.of.measure.code.cm", "zh-HK", "厘米_hk", "基本单位类别.厘米"),

            // dict.logistics.unit.of.measure.code.mm
            ("dict.logistics.unit.of.measure.code.mm", "en-US", "毫米_us", "基本单位类别.毫米"),
            // dict.logistics.unit.of.measure.code.mm
            ("dict.logistics.unit.of.measure.code.mm", "ja-JP", "毫米_jp", "基本单位类别.毫米"),
            // dict.logistics.unit.of.measure.code.mm
            ("dict.logistics.unit.of.measure.code.mm", "zh-CN", "毫米", "基本单位类别.毫米"),
            // dict.logistics.unit.of.measure.code.mm
            ("dict.logistics.unit.of.measure.code.mm", "zh-HK", "毫米_hk", "基本单位类别.毫米"),

            // dict.logistics.unit.of.measure.code.km
            ("dict.logistics.unit.of.measure.code.km", "en-US", "千米_us", "基本单位类别.千米"),
            // dict.logistics.unit.of.measure.code.km
            ("dict.logistics.unit.of.measure.code.km", "ja-JP", "千米_jp", "基本单位类别.千米"),
            // dict.logistics.unit.of.measure.code.km
            ("dict.logistics.unit.of.measure.code.km", "zh-CN", "千米", "基本单位类别.千米"),
            // dict.logistics.unit.of.measure.code.km
            ("dict.logistics.unit.of.measure.code.km", "zh-HK", "千米_hk", "基本单位类别.千米"),

            // dict.logistics.unit.of.measure.code.l
            ("dict.logistics.unit.of.measure.code.l", "en-US", "升_us", "基本单位类别.升"),
            // dict.logistics.unit.of.measure.code.l
            ("dict.logistics.unit.of.measure.code.l", "ja-JP", "升_jp", "基本单位类别.升"),
            // dict.logistics.unit.of.measure.code.l
            ("dict.logistics.unit.of.measure.code.l", "zh-CN", "升", "基本单位类别.升"),
            // dict.logistics.unit.of.measure.code.l
            ("dict.logistics.unit.of.measure.code.l", "zh-HK", "升_hk", "基本单位类别.升"),

            // dict.logistics.unit.of.measure.code.ml
            ("dict.logistics.unit.of.measure.code.ml", "en-US", "毫升_us", "基本单位类别.毫升"),
            // dict.logistics.unit.of.measure.code.ml
            ("dict.logistics.unit.of.measure.code.ml", "ja-JP", "毫升_jp", "基本单位类别.毫升"),
            // dict.logistics.unit.of.measure.code.ml
            ("dict.logistics.unit.of.measure.code.ml", "zh-CN", "毫升", "基本单位类别.毫升"),
            // dict.logistics.unit.of.measure.code.ml
            ("dict.logistics.unit.of.measure.code.ml", "zh-HK", "毫升_hk", "基本单位类别.毫升"),

            // dict.logistics.unit.of.measure.code.m3
            ("dict.logistics.unit.of.measure.code.m3", "en-US", "立方米_us", "基本单位类别.立方米"),
            // dict.logistics.unit.of.measure.code.m3
            ("dict.logistics.unit.of.measure.code.m3", "ja-JP", "立方米_jp", "基本单位类别.立方米"),
            // dict.logistics.unit.of.measure.code.m3
            ("dict.logistics.unit.of.measure.code.m3", "zh-CN", "立方米", "基本单位类别.立方米"),
            // dict.logistics.unit.of.measure.code.m3
            ("dict.logistics.unit.of.measure.code.m3", "zh-HK", "立方米_hk", "基本单位类别.立方米"),

            // dict.logistics.unit.of.measure.code.m2
            ("dict.logistics.unit.of.measure.code.m2", "en-US", "平方米_us", "基本单位类别.平方米"),
            // dict.logistics.unit.of.measure.code.m2
            ("dict.logistics.unit.of.measure.code.m2", "ja-JP", "平方米_jp", "基本单位类别.平方米"),
            // dict.logistics.unit.of.measure.code.m2
            ("dict.logistics.unit.of.measure.code.m2", "zh-CN", "平方米", "基本单位类别.平方米"),
            // dict.logistics.unit.of.measure.code.m2
            ("dict.logistics.unit.of.measure.code.m2", "zh-HK", "平方米_hk", "基本单位类别.平方米"),

            // dict.logistics.unit.of.measure.code.set
            ("dict.logistics.unit.of.measure.code.set", "en-US", "套_us", "基本单位类别.套"),
            // dict.logistics.unit.of.measure.code.set
            ("dict.logistics.unit.of.measure.code.set", "ja-JP", "套_jp", "基本单位类别.套"),
            // dict.logistics.unit.of.measure.code.set
            ("dict.logistics.unit.of.measure.code.set", "zh-CN", "套", "基本单位类别.套"),
            // dict.logistics.unit.of.measure.code.set
            ("dict.logistics.unit.of.measure.code.set", "zh-HK", "套_hk", "基本单位类别.套"),

            // dict.logistics.unit.of.measure.code.pr
            ("dict.logistics.unit.of.measure.code.pr", "en-US", "对_us", "基本单位类别.对"),
            // dict.logistics.unit.of.measure.code.pr
            ("dict.logistics.unit.of.measure.code.pr", "ja-JP", "对_jp", "基本单位类别.对"),
            // dict.logistics.unit.of.measure.code.pr
            ("dict.logistics.unit.of.measure.code.pr", "zh-CN", "对", "基本单位类别.对"),
            // dict.logistics.unit.of.measure.code.pr
            ("dict.logistics.unit.of.measure.code.pr", "zh-HK", "对_hk", "基本单位类别.对"),

            // dict.logistics.unit.of.measure.code.dz
            ("dict.logistics.unit.of.measure.code.dz", "en-US", "打_us", "基本单位类别.打"),
            // dict.logistics.unit.of.measure.code.dz
            ("dict.logistics.unit.of.measure.code.dz", "ja-JP", "打_jp", "基本单位类别.打"),
            // dict.logistics.unit.of.measure.code.dz
            ("dict.logistics.unit.of.measure.code.dz", "zh-CN", "打", "基本单位类别.打"),
            // dict.logistics.unit.of.measure.code.dz
            ("dict.logistics.unit.of.measure.code.dz", "zh-HK", "打_hk", "基本单位类别.打"),

            // dict.logistics.unit.of.measure.code.rol
            ("dict.logistics.unit.of.measure.code.rol", "en-US", "卷_us", "基本单位类别.卷"),
            // dict.logistics.unit.of.measure.code.rol
            ("dict.logistics.unit.of.measure.code.rol", "ja-JP", "卷_jp", "基本单位类别.卷"),
            // dict.logistics.unit.of.measure.code.rol
            ("dict.logistics.unit.of.measure.code.rol", "zh-CN", "卷", "基本单位类别.卷"),
            // dict.logistics.unit.of.measure.code.rol
            ("dict.logistics.unit.of.measure.code.rol", "zh-HK", "卷_hk", "基本单位类别.卷"),

            // dict.logistics.unit.of.measure.code.ct
            ("dict.logistics.unit.of.measure.code.ct", "en-US", "箱_us", "基本单位类别.箱"),
            // dict.logistics.unit.of.measure.code.ct
            ("dict.logistics.unit.of.measure.code.ct", "ja-JP", "箱_jp", "基本单位类别.箱"),
            // dict.logistics.unit.of.measure.code.ct
            ("dict.logistics.unit.of.measure.code.ct", "zh-CN", "箱", "基本单位类别.箱"),
            // dict.logistics.unit.of.measure.code.ct
            ("dict.logistics.unit.of.measure.code.ct", "zh-HK", "箱_hk", "基本单位类别.箱"),

            // dict.logistics.unit.of.measure.code.pk
            ("dict.logistics.unit.of.measure.code.pk", "en-US", "包_us", "基本单位类别.包"),
            // dict.logistics.unit.of.measure.code.pk
            ("dict.logistics.unit.of.measure.code.pk", "ja-JP", "包_jp", "基本单位类别.包"),
            // dict.logistics.unit.of.measure.code.pk
            ("dict.logistics.unit.of.measure.code.pk", "zh-CN", "包", "基本单位类别.包"),
            // dict.logistics.unit.of.measure.code.pk
            ("dict.logistics.unit.of.measure.code.pk", "zh-HK", "包_hk", "基本单位类别.包"),

            // dict.logistics.unit.of.measure.code.dr
            ("dict.logistics.unit.of.measure.code.dr", "en-US", "桶_us", "基本单位类别.桶"),
            // dict.logistics.unit.of.measure.code.dr
            ("dict.logistics.unit.of.measure.code.dr", "ja-JP", "桶_jp", "基本单位类别.桶"),
            // dict.logistics.unit.of.measure.code.dr
            ("dict.logistics.unit.of.measure.code.dr", "zh-CN", "桶", "基本单位类别.桶"),
            // dict.logistics.unit.of.measure.code.dr
            ("dict.logistics.unit.of.measure.code.dr", "zh-HK", "桶_hk", "基本单位类别.桶"),

            // dict.logistics.unit.of.measure.code.bo
            ("dict.logistics.unit.of.measure.code.bo", "en-US", "瓶_us", "基本单位类别.瓶"),
            // dict.logistics.unit.of.measure.code.bo
            ("dict.logistics.unit.of.measure.code.bo", "ja-JP", "瓶_jp", "基本单位类别.瓶"),
            // dict.logistics.unit.of.measure.code.bo
            ("dict.logistics.unit.of.measure.code.bo", "zh-CN", "瓶", "基本单位类别.瓶"),
            // dict.logistics.unit.of.measure.code.bo
            ("dict.logistics.unit.of.measure.code.bo", "zh-HK", "瓶_hk", "基本单位类别.瓶"),

            // dict.logistics.valuation.class.category.7920
            ("dict.logistics.valuation.class.category.7920", "en-US", "成品_us", "评估类别.成品"),
            // dict.logistics.valuation.class.category.7920
            ("dict.logistics.valuation.class.category.7920", "ja-JP", "成品_jp", "评估类别.成品"),
            // dict.logistics.valuation.class.category.7920
            ("dict.logistics.valuation.class.category.7920", "zh-CN", "成品", "评估类别.成品"),
            // dict.logistics.valuation.class.category.7920
            ("dict.logistics.valuation.class.category.7920", "zh-HK", "成品_hk", "评估类别.成品"),

            // dict.logistics.valuation.class.category.z300
            ("dict.logistics.valuation.class.category.z300", "en-US", "原材料(cn)_us", "评估类别.原材料(cn)"),
            // dict.logistics.valuation.class.category.z300
            ("dict.logistics.valuation.class.category.z300", "ja-JP", "原材料(cn)_jp", "评估类别.原材料(cn)"),
            // dict.logistics.valuation.class.category.z300
            ("dict.logistics.valuation.class.category.z300", "zh-CN", "原材料(cn)", "评估类别.原材料(cn)"),
            // dict.logistics.valuation.class.category.z300
            ("dict.logistics.valuation.class.category.z300", "zh-HK", "原材料(cn)_hk", "评估类别.原材料(cn)"),

            // dict.logistics.valuation.class.category.z790
            ("dict.logistics.valuation.class.category.z790", "en-US", "半成品(cn)_us", "评估类别.半成品(cn)"),
            // dict.logistics.valuation.class.category.z790
            ("dict.logistics.valuation.class.category.z790", "ja-JP", "半成品(cn)_jp", "评估类别.半成品(cn)"),
            // dict.logistics.valuation.class.category.z790
            ("dict.logistics.valuation.class.category.z790", "zh-CN", "半成品(cn)", "评估类别.半成品(cn)"),
            // dict.logistics.valuation.class.category.z790
            ("dict.logistics.valuation.class.category.z790", "zh-HK", "半成品(cn)_hk", "评估类别.半成品(cn)"),

            // dict.logistics.valuation.class.category.z792
            ("dict.logistics.valuation.class.category.z792", "en-US", "成品(cn)_us", "评估类别.成品(cn)"),
            // dict.logistics.valuation.class.category.z792
            ("dict.logistics.valuation.class.category.z792", "ja-JP", "成品(cn)_jp", "评估类别.成品(cn)"),
            // dict.logistics.valuation.class.category.z792
            ("dict.logistics.valuation.class.category.z792", "zh-CN", "成品(cn)", "评估类别.成品(cn)"),
            // dict.logistics.valuation.class.category.z792
            ("dict.logistics.valuation.class.category.z792", "zh-HK", "成品(cn)_hk", "评估类别.成品(cn)"),

            // dict.logistics.inbound.type.0
            ("dict.logistics.inbound.type.0", "en-US", "采购入库_us", "入库类型.采购入库"),
            // dict.logistics.inbound.type.0
            ("dict.logistics.inbound.type.0", "ja-JP", "采购入库_jp", "入库类型.采购入库"),
            // dict.logistics.inbound.type.0
            ("dict.logistics.inbound.type.0", "zh-CN", "采购入库", "入库类型.采购入库"),
            // dict.logistics.inbound.type.0
            ("dict.logistics.inbound.type.0", "zh-HK", "采购入库_hk", "入库类型.采购入库"),

            // dict.logistics.inbound.type.1
            ("dict.logistics.inbound.type.1", "en-US", "生产入库_us", "入库类型.生产入库"),
            // dict.logistics.inbound.type.1
            ("dict.logistics.inbound.type.1", "ja-JP", "生产入库_jp", "入库类型.生产入库"),
            // dict.logistics.inbound.type.1
            ("dict.logistics.inbound.type.1", "zh-CN", "生产入库", "入库类型.生产入库"),
            // dict.logistics.inbound.type.1
            ("dict.logistics.inbound.type.1", "zh-HK", "生产入库_hk", "入库类型.生产入库"),

            // dict.logistics.inbound.type.2
            ("dict.logistics.inbound.type.2", "en-US", "退货入库_us", "入库类型.退货入库"),
            // dict.logistics.inbound.type.2
            ("dict.logistics.inbound.type.2", "ja-JP", "退货入库_jp", "入库类型.退货入库"),
            // dict.logistics.inbound.type.2
            ("dict.logistics.inbound.type.2", "zh-CN", "退货入库", "入库类型.退货入库"),
            // dict.logistics.inbound.type.2
            ("dict.logistics.inbound.type.2", "zh-HK", "退货入库_hk", "入库类型.退货入库"),

            // dict.logistics.inbound.type.3
            ("dict.logistics.inbound.type.3", "en-US", "调拨入库_us", "入库类型.调拨入库"),
            // dict.logistics.inbound.type.3
            ("dict.logistics.inbound.type.3", "ja-JP", "调拨入库_jp", "入库类型.调拨入库"),
            // dict.logistics.inbound.type.3
            ("dict.logistics.inbound.type.3", "zh-CN", "调拨入库", "入库类型.调拨入库"),
            // dict.logistics.inbound.type.3
            ("dict.logistics.inbound.type.3", "zh-HK", "调拨入库_hk", "入库类型.调拨入库"),

            // dict.logistics.inbound.type.4
            ("dict.logistics.inbound.type.4", "en-US", "序列号入库_us", "入库类型.序列号入库"),
            // dict.logistics.inbound.type.4
            ("dict.logistics.inbound.type.4", "ja-JP", "序列号入库_jp", "入库类型.序列号入库"),
            // dict.logistics.inbound.type.4
            ("dict.logistics.inbound.type.4", "zh-CN", "序列号入库", "入库类型.序列号入库"),
            // dict.logistics.inbound.type.4
            ("dict.logistics.inbound.type.4", "zh-HK", "序列号入库_hk", "入库类型.序列号入库"),

            // dict.logistics.inbound.type.5
            ("dict.logistics.inbound.type.5", "en-US", "其他_us", "入库类型.其他"),
            // dict.logistics.inbound.type.5
            ("dict.logistics.inbound.type.5", "ja-JP", "其他_jp", "入库类型.其他"),
            // dict.logistics.inbound.type.5
            ("dict.logistics.inbound.type.5", "zh-CN", "其他", "入库类型.其他"),
            // dict.logistics.inbound.type.5
            ("dict.logistics.inbound.type.5", "zh-HK", "其他_hk", "入库类型.其他"),

            // dict.logistics.outbound.type.0
            ("dict.logistics.outbound.type.0", "en-US", "销售出库_us", "出库类型.销售出库"),
            // dict.logistics.outbound.type.0
            ("dict.logistics.outbound.type.0", "ja-JP", "销售出库_jp", "出库类型.销售出库"),
            // dict.logistics.outbound.type.0
            ("dict.logistics.outbound.type.0", "zh-CN", "销售出库", "出库类型.销售出库"),
            // dict.logistics.outbound.type.0
            ("dict.logistics.outbound.type.0", "zh-HK", "销售出库_hk", "出库类型.销售出库"),

            // dict.logistics.outbound.type.1
            ("dict.logistics.outbound.type.1", "en-US", "生产领料_us", "出库类型.生产领料"),
            // dict.logistics.outbound.type.1
            ("dict.logistics.outbound.type.1", "ja-JP", "生产领料_jp", "出库类型.生产领料"),
            // dict.logistics.outbound.type.1
            ("dict.logistics.outbound.type.1", "zh-CN", "生产领料", "出库类型.生产领料"),
            // dict.logistics.outbound.type.1
            ("dict.logistics.outbound.type.1", "zh-HK", "生产领料_hk", "出库类型.生产领料"),

            // dict.logistics.outbound.type.2
            ("dict.logistics.outbound.type.2", "en-US", "退货出库_us", "出库类型.退货出库"),
            // dict.logistics.outbound.type.2
            ("dict.logistics.outbound.type.2", "ja-JP", "退货出库_jp", "出库类型.退货出库"),
            // dict.logistics.outbound.type.2
            ("dict.logistics.outbound.type.2", "zh-CN", "退货出库", "出库类型.退货出库"),
            // dict.logistics.outbound.type.2
            ("dict.logistics.outbound.type.2", "zh-HK", "退货出库_hk", "出库类型.退货出库"),

            // dict.logistics.outbound.type.3
            ("dict.logistics.outbound.type.3", "en-US", "调拨出库_us", "出库类型.调拨出库"),
            // dict.logistics.outbound.type.3
            ("dict.logistics.outbound.type.3", "ja-JP", "调拨出库_jp", "出库类型.调拨出库"),
            // dict.logistics.outbound.type.3
            ("dict.logistics.outbound.type.3", "zh-CN", "调拨出库", "出库类型.调拨出库"),
            // dict.logistics.outbound.type.3
            ("dict.logistics.outbound.type.3", "zh-HK", "调拨出库_hk", "出库类型.调拨出库"),

            // dict.logistics.outbound.type.4
            ("dict.logistics.outbound.type.4", "en-US", "报废出库_us", "出库类型.报废出库"),
            // dict.logistics.outbound.type.4
            ("dict.logistics.outbound.type.4", "ja-JP", "报废出库_jp", "出库类型.报废出库"),
            // dict.logistics.outbound.type.4
            ("dict.logistics.outbound.type.4", "zh-CN", "报废出库", "出库类型.报废出库"),
            // dict.logistics.outbound.type.4
            ("dict.logistics.outbound.type.4", "zh-HK", "报废出库_hk", "出库类型.报废出库"),

            // dict.logistics.outbound.type.5
            ("dict.logistics.outbound.type.5", "en-US", "序列号出库_us", "出库类型.序列号出库"),
            // dict.logistics.outbound.type.5
            ("dict.logistics.outbound.type.5", "ja-JP", "序列号出库_jp", "出库类型.序列号出库"),
            // dict.logistics.outbound.type.5
            ("dict.logistics.outbound.type.5", "zh-CN", "序列号出库", "出库类型.序列号出库"),
            // dict.logistics.outbound.type.5
            ("dict.logistics.outbound.type.5", "zh-HK", "序列号出库_hk", "出库类型.序列号出库"),

            // dict.logistics.outbound.type.6
            ("dict.logistics.outbound.type.6", "en-US", "其他_us", "出库类型.其他"),
            // dict.logistics.outbound.type.6
            ("dict.logistics.outbound.type.6", "ja-JP", "其他_jp", "出库类型.其他"),
            // dict.logistics.outbound.type.6
            ("dict.logistics.outbound.type.6", "zh-CN", "其他", "出库类型.其他"),
            // dict.logistics.outbound.type.6
            ("dict.logistics.outbound.type.6", "zh-HK", "其他_hk", "出库类型.其他"),

            // dict.logistics.shipping.method.type.0
            ("dict.logistics.shipping.method.type.0", "en-US", "海运_us", "运输方式.海运"),
            // dict.logistics.shipping.method.type.0
            ("dict.logistics.shipping.method.type.0", "ja-JP", "海运_jp", "运输方式.海运"),
            // dict.logistics.shipping.method.type.0
            ("dict.logistics.shipping.method.type.0", "zh-CN", "海运", "运输方式.海运"),
            // dict.logistics.shipping.method.type.0
            ("dict.logistics.shipping.method.type.0", "zh-HK", "海运_hk", "运输方式.海运"),

            // dict.logistics.shipping.method.type.1
            ("dict.logistics.shipping.method.type.1", "en-US", "空运_us", "运输方式.空运"),
            // dict.logistics.shipping.method.type.1
            ("dict.logistics.shipping.method.type.1", "ja-JP", "空运_jp", "运输方式.空运"),
            // dict.logistics.shipping.method.type.1
            ("dict.logistics.shipping.method.type.1", "zh-CN", "空运", "运输方式.空运"),
            // dict.logistics.shipping.method.type.1
            ("dict.logistics.shipping.method.type.1", "zh-HK", "空运_hk", "运输方式.空运"),

            // dict.logistics.shipping.method.type.2
            ("dict.logistics.shipping.method.type.2", "en-US", "陆运_us", "运输方式.陆运"),
            // dict.logistics.shipping.method.type.2
            ("dict.logistics.shipping.method.type.2", "ja-JP", "陆运_jp", "运输方式.陆运"),
            // dict.logistics.shipping.method.type.2
            ("dict.logistics.shipping.method.type.2", "zh-CN", "陆运", "运输方式.陆运"),
            // dict.logistics.shipping.method.type.2
            ("dict.logistics.shipping.method.type.2", "zh-HK", "陆运_hk", "运输方式.陆运"),

            // dict.logistics.shipping.method.type.3
            ("dict.logistics.shipping.method.type.3", "en-US", "铁路_us", "运输方式.铁路"),
            // dict.logistics.shipping.method.type.3
            ("dict.logistics.shipping.method.type.3", "ja-JP", "铁路_jp", "运输方式.铁路"),
            // dict.logistics.shipping.method.type.3
            ("dict.logistics.shipping.method.type.3", "zh-CN", "铁路", "运输方式.铁路"),
            // dict.logistics.shipping.method.type.3
            ("dict.logistics.shipping.method.type.3", "zh-HK", "铁路_hk", "运输方式.铁路"),

            // dict.logistics.shipping.method.type.4
            ("dict.logistics.shipping.method.type.4", "en-US", "快递_us", "运输方式.快递"),
            // dict.logistics.shipping.method.type.4
            ("dict.logistics.shipping.method.type.4", "ja-JP", "快递_jp", "运输方式.快递"),
            // dict.logistics.shipping.method.type.4
            ("dict.logistics.shipping.method.type.4", "zh-CN", "快递", "运输方式.快递"),
            // dict.logistics.shipping.method.type.4
            ("dict.logistics.shipping.method.type.4", "zh-HK", "快递_hk", "运输方式.快递"),

            // dict.logistics.shipping.method.type.5
            ("dict.logistics.shipping.method.type.5", "en-US", "其他_us", "运输方式.其他"),
            // dict.logistics.shipping.method.type.5
            ("dict.logistics.shipping.method.type.5", "ja-JP", "其他_jp", "运输方式.其他"),
            // dict.logistics.shipping.method.type.5
            ("dict.logistics.shipping.method.type.5", "zh-CN", "其他", "运输方式.其他"),
            // dict.logistics.shipping.method.type.5
            ("dict.logistics.shipping.method.type.5", "zh-HK", "其他_hk", "运输方式.其他"),

            // dict.logistics.delivery.status.0
            ("dict.logistics.delivery.status.0", "en-US", "未交货_us", "交货状态.未交货"),
            // dict.logistics.delivery.status.0
            ("dict.logistics.delivery.status.0", "ja-JP", "未交货_jp", "交货状态.未交货"),
            // dict.logistics.delivery.status.0
            ("dict.logistics.delivery.status.0", "zh-CN", "未交货", "交货状态.未交货"),
            // dict.logistics.delivery.status.0
            ("dict.logistics.delivery.status.0", "zh-HK", "未交货_hk", "交货状态.未交货"),

            // dict.logistics.delivery.status.1
            ("dict.logistics.delivery.status.1", "en-US", "部分交货_us", "交货状态.部分交货"),
            // dict.logistics.delivery.status.1
            ("dict.logistics.delivery.status.1", "ja-JP", "部分交货_jp", "交货状态.部分交货"),
            // dict.logistics.delivery.status.1
            ("dict.logistics.delivery.status.1", "zh-CN", "部分交货", "交货状态.部分交货"),
            // dict.logistics.delivery.status.1
            ("dict.logistics.delivery.status.1", "zh-HK", "部分交货_hk", "交货状态.部分交货"),

            // dict.logistics.delivery.status.2
            ("dict.logistics.delivery.status.2", "en-US", "全部交货_us", "交货状态.全部交货"),
            // dict.logistics.delivery.status.2
            ("dict.logistics.delivery.status.2", "ja-JP", "全部交货_jp", "交货状态.全部交货"),
            // dict.logistics.delivery.status.2
            ("dict.logistics.delivery.status.2", "zh-CN", "全部交货", "交货状态.全部交货"),
            // dict.logistics.delivery.status.2
            ("dict.logistics.delivery.status.2", "zh-HK", "全部交货_hk", "交货状态.全部交货"),

            // dict.logistics.payment.method.type.0
            ("dict.logistics.payment.method.type.0", "en-US", "现金_us", "支付方式.现金"),
            // dict.logistics.payment.method.type.0
            ("dict.logistics.payment.method.type.0", "ja-JP", "现金_jp", "支付方式.现金"),
            // dict.logistics.payment.method.type.0
            ("dict.logistics.payment.method.type.0", "zh-CN", "现金", "支付方式.现金"),
            // dict.logistics.payment.method.type.0
            ("dict.logistics.payment.method.type.0", "zh-HK", "现金_hk", "支付方式.现金"),

            // dict.logistics.payment.method.type.1
            ("dict.logistics.payment.method.type.1", "en-US", "银行转账_us", "支付方式.银行转账"),
            // dict.logistics.payment.method.type.1
            ("dict.logistics.payment.method.type.1", "ja-JP", "银行转账_jp", "支付方式.银行转账"),
            // dict.logistics.payment.method.type.1
            ("dict.logistics.payment.method.type.1", "zh-CN", "银行转账", "支付方式.银行转账"),
            // dict.logistics.payment.method.type.1
            ("dict.logistics.payment.method.type.1", "zh-HK", "银行转账_hk", "支付方式.银行转账"),

            // dict.logistics.payment.method.type.2
            ("dict.logistics.payment.method.type.2", "en-US", "支票_us", "支付方式.支票"),
            // dict.logistics.payment.method.type.2
            ("dict.logistics.payment.method.type.2", "ja-JP", "支票_jp", "支付方式.支票"),
            // dict.logistics.payment.method.type.2
            ("dict.logistics.payment.method.type.2", "zh-CN", "支票", "支付方式.支票"),
            // dict.logistics.payment.method.type.2
            ("dict.logistics.payment.method.type.2", "zh-HK", "支票_hk", "支付方式.支票"),

            // dict.logistics.payment.method.type.3
            ("dict.logistics.payment.method.type.3", "en-US", "信用证_us", "支付方式.信用证"),
            // dict.logistics.payment.method.type.3
            ("dict.logistics.payment.method.type.3", "ja-JP", "信用证_jp", "支付方式.信用证"),
            // dict.logistics.payment.method.type.3
            ("dict.logistics.payment.method.type.3", "zh-CN", "信用证", "支付方式.信用证"),
            // dict.logistics.payment.method.type.3
            ("dict.logistics.payment.method.type.3", "zh-HK", "信用证_hk", "支付方式.信用证"),

            // dict.logistics.payment.method.type.4
            ("dict.logistics.payment.method.type.4", "en-US", "其他_us", "支付方式.其他"),
            // dict.logistics.payment.method.type.4
            ("dict.logistics.payment.method.type.4", "ja-JP", "其他_jp", "支付方式.其他"),
            // dict.logistics.payment.method.type.4
            ("dict.logistics.payment.method.type.4", "zh-CN", "其他", "支付方式.其他"),
            // dict.logistics.payment.method.type.4
            ("dict.logistics.payment.method.type.4", "zh-HK", "其他_hk", "支付方式.其他"),

            // dict.logistics.discount.rate.param.0
            ("dict.logistics.discount.rate.param.0", "en-US", "0%_us", "折扣率.0%"),
            // dict.logistics.discount.rate.param.0
            ("dict.logistics.discount.rate.param.0", "ja-JP", "0%_jp", "折扣率.0%"),
            // dict.logistics.discount.rate.param.0
            ("dict.logistics.discount.rate.param.0", "zh-CN", "0%", "折扣率.0%"),
            // dict.logistics.discount.rate.param.0
            ("dict.logistics.discount.rate.param.0", "zh-HK", "0%_hk", "折扣率.0%"),

            // dict.logistics.discount.rate.param.5
            ("dict.logistics.discount.rate.param.5", "en-US", "5%_us", "折扣率.5%"),
            // dict.logistics.discount.rate.param.5
            ("dict.logistics.discount.rate.param.5", "ja-JP", "5%_jp", "折扣率.5%"),
            // dict.logistics.discount.rate.param.5
            ("dict.logistics.discount.rate.param.5", "zh-CN", "5%", "折扣率.5%"),
            // dict.logistics.discount.rate.param.5
            ("dict.logistics.discount.rate.param.5", "zh-HK", "5%_hk", "折扣率.5%"),

            // dict.logistics.discount.rate.param.10
            ("dict.logistics.discount.rate.param.10", "en-US", "10%_us", "折扣率.10%"),
            // dict.logistics.discount.rate.param.10
            ("dict.logistics.discount.rate.param.10", "ja-JP", "10%_jp", "折扣率.10%"),
            // dict.logistics.discount.rate.param.10
            ("dict.logistics.discount.rate.param.10", "zh-CN", "10%", "折扣率.10%"),
            // dict.logistics.discount.rate.param.10
            ("dict.logistics.discount.rate.param.10", "zh-HK", "10%_hk", "折扣率.10%"),

            // dict.logistics.discount.rate.param.15
            ("dict.logistics.discount.rate.param.15", "en-US", "15%_us", "折扣率.15%"),
            // dict.logistics.discount.rate.param.15
            ("dict.logistics.discount.rate.param.15", "ja-JP", "15%_jp", "折扣率.15%"),
            // dict.logistics.discount.rate.param.15
            ("dict.logistics.discount.rate.param.15", "zh-CN", "15%", "折扣率.15%"),
            // dict.logistics.discount.rate.param.15
            ("dict.logistics.discount.rate.param.15", "zh-HK", "15%_hk", "折扣率.15%"),

            // dict.logistics.discount.rate.param.20
            ("dict.logistics.discount.rate.param.20", "en-US", "20%_us", "折扣率.20%"),
            // dict.logistics.discount.rate.param.20
            ("dict.logistics.discount.rate.param.20", "ja-JP", "20%_jp", "折扣率.20%"),
            // dict.logistics.discount.rate.param.20
            ("dict.logistics.discount.rate.param.20", "zh-CN", "20%", "折扣率.20%"),
            // dict.logistics.discount.rate.param.20
            ("dict.logistics.discount.rate.param.20", "zh-HK", "20%_hk", "折扣率.20%"),

            // dict.logistics.discount.rate.param.25
            ("dict.logistics.discount.rate.param.25", "en-US", "25%_us", "折扣率.25%"),
            // dict.logistics.discount.rate.param.25
            ("dict.logistics.discount.rate.param.25", "ja-JP", "25%_jp", "折扣率.25%"),
            // dict.logistics.discount.rate.param.25
            ("dict.logistics.discount.rate.param.25", "zh-CN", "25%", "折扣率.25%"),
            // dict.logistics.discount.rate.param.25
            ("dict.logistics.discount.rate.param.25", "zh-HK", "25%_hk", "折扣率.25%"),

            // dict.logistics.discount.rate.param.30
            ("dict.logistics.discount.rate.param.30", "en-US", "30%_us", "折扣率.30%"),
            // dict.logistics.discount.rate.param.30
            ("dict.logistics.discount.rate.param.30", "ja-JP", "30%_jp", "折扣率.30%"),
            // dict.logistics.discount.rate.param.30
            ("dict.logistics.discount.rate.param.30", "zh-CN", "30%", "折扣率.30%"),
            // dict.logistics.discount.rate.param.30
            ("dict.logistics.discount.rate.param.30", "zh-HK", "30%_hk", "折扣率.30%"),

            // dict.logistics.discount.rate.param.50
            ("dict.logistics.discount.rate.param.50", "en-US", "50%_us", "折扣率.50%"),
            // dict.logistics.discount.rate.param.50
            ("dict.logistics.discount.rate.param.50", "ja-JP", "50%_jp", "折扣率.50%"),
            // dict.logistics.discount.rate.param.50
            ("dict.logistics.discount.rate.param.50", "zh-CN", "50%", "折扣率.50%"),
            // dict.logistics.discount.rate.param.50
            ("dict.logistics.discount.rate.param.50", "zh-HK", "50%_hk", "折扣率.50%"),

            // dict.logistics.discount.rate.param.100
            ("dict.logistics.discount.rate.param.100", "en-US", "100%_us", "折扣率.100%"),
            // dict.logistics.discount.rate.param.100
            ("dict.logistics.discount.rate.param.100", "ja-JP", "100%_jp", "折扣率.100%"),
            // dict.logistics.discount.rate.param.100
            ("dict.logistics.discount.rate.param.100", "zh-CN", "100%", "折扣率.100%"),
            // dict.logistics.discount.rate.param.100
            ("dict.logistics.discount.rate.param.100", "zh-HK", "100%_hk", "折扣率.100%"),

            // dict.logistics.tax.rate.param.0
            ("dict.logistics.tax.rate.param.0", "en-US", "0%_us", "税费率.0%"),
            // dict.logistics.tax.rate.param.0
            ("dict.logistics.tax.rate.param.0", "ja-JP", "0%_jp", "税费率.0%"),
            // dict.logistics.tax.rate.param.0
            ("dict.logistics.tax.rate.param.0", "zh-CN", "0%", "税费率.0%"),
            // dict.logistics.tax.rate.param.0
            ("dict.logistics.tax.rate.param.0", "zh-HK", "0%_hk", "税费率.0%"),

            // dict.logistics.tax.rate.param.3
            ("dict.logistics.tax.rate.param.3", "en-US", "3%_us", "税费率.3%"),
            // dict.logistics.tax.rate.param.3
            ("dict.logistics.tax.rate.param.3", "ja-JP", "3%_jp", "税费率.3%"),
            // dict.logistics.tax.rate.param.3
            ("dict.logistics.tax.rate.param.3", "zh-CN", "3%", "税费率.3%"),
            // dict.logistics.tax.rate.param.3
            ("dict.logistics.tax.rate.param.3", "zh-HK", "3%_hk", "税费率.3%"),

            // dict.logistics.tax.rate.param.6
            ("dict.logistics.tax.rate.param.6", "en-US", "6%_us", "税费率.6%"),
            // dict.logistics.tax.rate.param.6
            ("dict.logistics.tax.rate.param.6", "ja-JP", "6%_jp", "税费率.6%"),
            // dict.logistics.tax.rate.param.6
            ("dict.logistics.tax.rate.param.6", "zh-CN", "6%", "税费率.6%"),
            // dict.logistics.tax.rate.param.6
            ("dict.logistics.tax.rate.param.6", "zh-HK", "6%_hk", "税费率.6%"),

            // dict.logistics.tax.rate.param.9
            ("dict.logistics.tax.rate.param.9", "en-US", "9%_us", "税费率.9%"),
            // dict.logistics.tax.rate.param.9
            ("dict.logistics.tax.rate.param.9", "ja-JP", "9%_jp", "税费率.9%"),
            // dict.logistics.tax.rate.param.9
            ("dict.logistics.tax.rate.param.9", "zh-CN", "9%", "税费率.9%"),
            // dict.logistics.tax.rate.param.9
            ("dict.logistics.tax.rate.param.9", "zh-HK", "9%_hk", "税费率.9%"),

            // dict.logistics.tax.rate.param.13
            ("dict.logistics.tax.rate.param.13", "en-US", "13%_us", "税费率.13%"),
            // dict.logistics.tax.rate.param.13
            ("dict.logistics.tax.rate.param.13", "ja-JP", "13%_jp", "税费率.13%"),
            // dict.logistics.tax.rate.param.13
            ("dict.logistics.tax.rate.param.13", "zh-CN", "13%", "税费率.13%"),
            // dict.logistics.tax.rate.param.13
            ("dict.logistics.tax.rate.param.13", "zh-HK", "13%_hk", "税费率.13%"),

            // dict.logistics.supplier.category.0
            ("dict.logistics.supplier.category.0", "en-US", "生产商_us", "供货商类型.生产商"),
            // dict.logistics.supplier.category.0
            ("dict.logistics.supplier.category.0", "ja-JP", "生产商_jp", "供货商类型.生产商"),
            // dict.logistics.supplier.category.0
            ("dict.logistics.supplier.category.0", "zh-CN", "生产商", "供货商类型.生产商"),
            // dict.logistics.supplier.category.0
            ("dict.logistics.supplier.category.0", "zh-HK", "生产商_hk", "供货商类型.生产商"),

            // dict.logistics.supplier.category.1
            ("dict.logistics.supplier.category.1", "en-US", "代理商_us", "供货商类型.代理商"),
            // dict.logistics.supplier.category.1
            ("dict.logistics.supplier.category.1", "ja-JP", "代理商_jp", "供货商类型.代理商"),
            // dict.logistics.supplier.category.1
            ("dict.logistics.supplier.category.1", "zh-CN", "代理商", "供货商类型.代理商"),
            // dict.logistics.supplier.category.1
            ("dict.logistics.supplier.category.1", "zh-HK", "代理商_hk", "供货商类型.代理商"),

            // dict.logistics.supplier.category.2
            ("dict.logistics.supplier.category.2", "en-US", "经销商_us", "供货商类型.经销商"),
            // dict.logistics.supplier.category.2
            ("dict.logistics.supplier.category.2", "ja-JP", "经销商_jp", "供货商类型.经销商"),
            // dict.logistics.supplier.category.2
            ("dict.logistics.supplier.category.2", "zh-CN", "经销商", "供货商类型.经销商"),
            // dict.logistics.supplier.category.2
            ("dict.logistics.supplier.category.2", "zh-HK", "经销商_hk", "供货商类型.经销商"),

            // dict.logistics.supplier.category.3
            ("dict.logistics.supplier.category.3", "en-US", "贸易商_us", "供货商类型.贸易商"),
            // dict.logistics.supplier.category.3
            ("dict.logistics.supplier.category.3", "ja-JP", "贸易商_jp", "供货商类型.贸易商"),
            // dict.logistics.supplier.category.3
            ("dict.logistics.supplier.category.3", "zh-CN", "贸易商", "供货商类型.贸易商"),
            // dict.logistics.supplier.category.3
            ("dict.logistics.supplier.category.3", "zh-HK", "贸易商_hk", "供货商类型.贸易商"),

            // dict.logistics.supplier.category.4
            ("dict.logistics.supplier.category.4", "en-US", "其他_us", "供货商类型.其他"),
            // dict.logistics.supplier.category.4
            ("dict.logistics.supplier.category.4", "ja-JP", "其他_jp", "供货商类型.其他"),
            // dict.logistics.supplier.category.4
            ("dict.logistics.supplier.category.4", "zh-CN", "其他", "供货商类型.其他"),
            // dict.logistics.supplier.category.4
            ("dict.logistics.supplier.category.4", "zh-HK", "其他_hk", "供货商类型.其他"),

            // dict.logistics.payment.terms.param.0
            ("dict.logistics.payment.terms.param.0", "en-US", "款到发货_us", "付款条件.款到发货"),
            // dict.logistics.payment.terms.param.0
            ("dict.logistics.payment.terms.param.0", "ja-JP", "款到发货_jp", "付款条件.款到发货"),
            // dict.logistics.payment.terms.param.0
            ("dict.logistics.payment.terms.param.0", "zh-CN", "款到发货", "付款条件.款到发货"),
            // dict.logistics.payment.terms.param.0
            ("dict.logistics.payment.terms.param.0", "zh-HK", "款到发货_hk", "付款条件.款到发货"),

            // dict.logistics.payment.terms.param.1
            ("dict.logistics.payment.terms.param.1", "en-US", "货到付款_us", "付款条件.货到付款"),
            // dict.logistics.payment.terms.param.1
            ("dict.logistics.payment.terms.param.1", "ja-JP", "货到付款_jp", "付款条件.货到付款"),
            // dict.logistics.payment.terms.param.1
            ("dict.logistics.payment.terms.param.1", "zh-CN", "货到付款", "付款条件.货到付款"),
            // dict.logistics.payment.terms.param.1
            ("dict.logistics.payment.terms.param.1", "zh-HK", "货到付款_hk", "付款条件.货到付款"),

            // dict.logistics.payment.terms.param.2
            ("dict.logistics.payment.terms.param.2", "en-US", "月结30天_us", "付款条件.月结30天"),
            // dict.logistics.payment.terms.param.2
            ("dict.logistics.payment.terms.param.2", "ja-JP", "月结30天_jp", "付款条件.月结30天"),
            // dict.logistics.payment.terms.param.2
            ("dict.logistics.payment.terms.param.2", "zh-CN", "月结30天", "付款条件.月结30天"),
            // dict.logistics.payment.terms.param.2
            ("dict.logistics.payment.terms.param.2", "zh-HK", "月结30天_hk", "付款条件.月结30天"),

            // dict.logistics.payment.terms.param.3
            ("dict.logistics.payment.terms.param.3", "en-US", "月结60天_us", "付款条件.月结60天"),
            // dict.logistics.payment.terms.param.3
            ("dict.logistics.payment.terms.param.3", "ja-JP", "月结60天_jp", "付款条件.月结60天"),
            // dict.logistics.payment.terms.param.3
            ("dict.logistics.payment.terms.param.3", "zh-CN", "月结60天", "付款条件.月结60天"),
            // dict.logistics.payment.terms.param.3
            ("dict.logistics.payment.terms.param.3", "zh-HK", "月结60天_hk", "付款条件.月结60天"),

            // dict.logistics.payment.terms.param.4
            ("dict.logistics.payment.terms.param.4", "en-US", "月结90天_us", "付款条件.月结90天"),
            // dict.logistics.payment.terms.param.4
            ("dict.logistics.payment.terms.param.4", "ja-JP", "月结90天_jp", "付款条件.月结90天"),
            // dict.logistics.payment.terms.param.4
            ("dict.logistics.payment.terms.param.4", "zh-CN", "月结90天", "付款条件.月结90天"),
            // dict.logistics.payment.terms.param.4
            ("dict.logistics.payment.terms.param.4", "zh-HK", "月结90天_hk", "付款条件.月结90天"),

            // dict.logistics.payment.terms.param.5
            ("dict.logistics.payment.terms.param.5", "en-US", "其他_us", "付款条件.其他"),
            // dict.logistics.payment.terms.param.5
            ("dict.logistics.payment.terms.param.5", "ja-JP", "其他_jp", "付款条件.其他"),
            // dict.logistics.payment.terms.param.5
            ("dict.logistics.payment.terms.param.5", "zh-CN", "其他", "付款条件.其他"),
            // dict.logistics.payment.terms.param.5
            ("dict.logistics.payment.terms.param.5", "zh-HK", "其他_hk", "付款条件.其他"),

            // dict.logistics.vendor.category.0
            ("dict.logistics.vendor.category.0", "en-US", "授权经销商_us", "经销商类型.授权经销商"),
            // dict.logistics.vendor.category.0
            ("dict.logistics.vendor.category.0", "ja-JP", "授权经销商_jp", "经销商类型.授权经销商"),
            // dict.logistics.vendor.category.0
            ("dict.logistics.vendor.category.0", "zh-CN", "授权经销商", "经销商类型.授权经销商"),
            // dict.logistics.vendor.category.0
            ("dict.logistics.vendor.category.0", "zh-HK", "授权经销商_hk", "经销商类型.授权经销商"),

            // dict.logistics.vendor.category.1
            ("dict.logistics.vendor.category.1", "en-US", "一般经销商_us", "经销商类型.一般经销商"),
            // dict.logistics.vendor.category.1
            ("dict.logistics.vendor.category.1", "ja-JP", "一般经销商_jp", "经销商类型.一般经销商"),
            // dict.logistics.vendor.category.1
            ("dict.logistics.vendor.category.1", "zh-CN", "一般经销商", "经销商类型.一般经销商"),
            // dict.logistics.vendor.category.1
            ("dict.logistics.vendor.category.1", "zh-HK", "一般经销商_hk", "经销商类型.一般经销商"),

            // dict.logistics.vendor.category.2
            ("dict.logistics.vendor.category.2", "en-US", "代理商_us", "经销商类型.代理商"),
            // dict.logistics.vendor.category.2
            ("dict.logistics.vendor.category.2", "ja-JP", "代理商_jp", "经销商类型.代理商"),
            // dict.logistics.vendor.category.2
            ("dict.logistics.vendor.category.2", "zh-CN", "代理商", "经销商类型.代理商"),
            // dict.logistics.vendor.category.2
            ("dict.logistics.vendor.category.2", "zh-HK", "代理商_hk", "经销商类型.代理商"),

            // dict.logistics.vendor.category.3
            ("dict.logistics.vendor.category.3", "en-US", "零售商_us", "经销商类型.零售商"),
            // dict.logistics.vendor.category.3
            ("dict.logistics.vendor.category.3", "ja-JP", "零售商_jp", "经销商类型.零售商"),
            // dict.logistics.vendor.category.3
            ("dict.logistics.vendor.category.3", "zh-CN", "零售商", "经销商类型.零售商"),
            // dict.logistics.vendor.category.3
            ("dict.logistics.vendor.category.3", "zh-HK", "零售商_hk", "经销商类型.零售商"),

            // dict.logistics.vendor.category.4
            ("dict.logistics.vendor.category.4", "en-US", "其他_us", "经销商类型.其他"),
            // dict.logistics.vendor.category.4
            ("dict.logistics.vendor.category.4", "ja-JP", "其他_jp", "经销商类型.其他"),
            // dict.logistics.vendor.category.4
            ("dict.logistics.vendor.category.4", "zh-CN", "其他", "经销商类型.其他"),
            // dict.logistics.vendor.category.4
            ("dict.logistics.vendor.category.4", "zh-HK", "其他_hk", "经销商类型.其他"),

            // dict.logistics.client.category.0
            ("dict.logistics.client.category.0", "en-US", "终端客户_us", "客户端类型.终端客户"),
            // dict.logistics.client.category.0
            ("dict.logistics.client.category.0", "ja-JP", "终端客户_jp", "客户端类型.终端客户"),
            // dict.logistics.client.category.0
            ("dict.logistics.client.category.0", "zh-CN", "终端客户", "客户端类型.终端客户"),
            // dict.logistics.client.category.0
            ("dict.logistics.client.category.0", "zh-HK", "终端客户_hk", "客户端类型.终端客户"),

            // dict.logistics.client.category.1
            ("dict.logistics.client.category.1", "en-US", "分销商_us", "客户端类型.分销商"),
            // dict.logistics.client.category.1
            ("dict.logistics.client.category.1", "ja-JP", "分销商_jp", "客户端类型.分销商"),
            // dict.logistics.client.category.1
            ("dict.logistics.client.category.1", "zh-CN", "分销商", "客户端类型.分销商"),
            // dict.logistics.client.category.1
            ("dict.logistics.client.category.1", "zh-HK", "分销商_hk", "客户端类型.分销商"),

            // dict.logistics.client.category.2
            ("dict.logistics.client.category.2", "en-US", "零售商_us", "客户端类型.零售商"),
            // dict.logistics.client.category.2
            ("dict.logistics.client.category.2", "ja-JP", "零售商_jp", "客户端类型.零售商"),
            // dict.logistics.client.category.2
            ("dict.logistics.client.category.2", "zh-CN", "零售商", "客户端类型.零售商"),
            // dict.logistics.client.category.2
            ("dict.logistics.client.category.2", "zh-HK", "零售商_hk", "客户端类型.零售商"),

            // dict.logistics.client.category.3
            ("dict.logistics.client.category.3", "en-US", "电商平台_us", "客户端类型.电商平台"),
            // dict.logistics.client.category.3
            ("dict.logistics.client.category.3", "ja-JP", "电商平台_jp", "客户端类型.电商平台"),
            // dict.logistics.client.category.3
            ("dict.logistics.client.category.3", "zh-CN", "电商平台", "客户端类型.电商平台"),
            // dict.logistics.client.category.3
            ("dict.logistics.client.category.3", "zh-HK", "电商平台_hk", "客户端类型.电商平台"),

            // dict.logistics.client.category.4
            ("dict.logistics.client.category.4", "en-US", "其他_us", "客户端类型.其他"),
            // dict.logistics.client.category.4
            ("dict.logistics.client.category.4", "ja-JP", "其他_jp", "客户端类型.其他"),
            // dict.logistics.client.category.4
            ("dict.logistics.client.category.4", "zh-CN", "其他", "客户端类型.其他"),
            // dict.logistics.client.category.4
            ("dict.logistics.client.category.4", "zh-HK", "其他_hk", "客户端类型.其他"),

            // dict.logistics.sales.channel.type.0
            ("dict.logistics.sales.channel.type.0", "en-US", "直销_us", "销售渠道.直销"),
            // dict.logistics.sales.channel.type.0
            ("dict.logistics.sales.channel.type.0", "ja-JP", "直销_jp", "销售渠道.直销"),
            // dict.logistics.sales.channel.type.0
            ("dict.logistics.sales.channel.type.0", "zh-CN", "直销", "销售渠道.直销"),
            // dict.logistics.sales.channel.type.0
            ("dict.logistics.sales.channel.type.0", "zh-HK", "直销_hk", "销售渠道.直销"),

            // dict.logistics.sales.channel.type.1
            ("dict.logistics.sales.channel.type.1", "en-US", "经销_us", "销售渠道.经销"),
            // dict.logistics.sales.channel.type.1
            ("dict.logistics.sales.channel.type.1", "ja-JP", "经销_jp", "销售渠道.经销"),
            // dict.logistics.sales.channel.type.1
            ("dict.logistics.sales.channel.type.1", "zh-CN", "经销", "销售渠道.经销"),
            // dict.logistics.sales.channel.type.1
            ("dict.logistics.sales.channel.type.1", "zh-HK", "经销_hk", "销售渠道.经销"),

            // dict.logistics.sales.channel.type.2
            ("dict.logistics.sales.channel.type.2", "en-US", "代销_us", "销售渠道.代销"),
            // dict.logistics.sales.channel.type.2
            ("dict.logistics.sales.channel.type.2", "ja-JP", "代销_jp", "销售渠道.代销"),
            // dict.logistics.sales.channel.type.2
            ("dict.logistics.sales.channel.type.2", "zh-CN", "代销", "销售渠道.代销"),
            // dict.logistics.sales.channel.type.2
            ("dict.logistics.sales.channel.type.2", "zh-HK", "代销_hk", "销售渠道.代销"),

            // dict.logistics.sales.channel.type.3
            ("dict.logistics.sales.channel.type.3", "en-US", "电商_us", "销售渠道.电商"),
            // dict.logistics.sales.channel.type.3
            ("dict.logistics.sales.channel.type.3", "ja-JP", "电商_jp", "销售渠道.电商"),
            // dict.logistics.sales.channel.type.3
            ("dict.logistics.sales.channel.type.3", "zh-CN", "电商", "销售渠道.电商"),
            // dict.logistics.sales.channel.type.3
            ("dict.logistics.sales.channel.type.3", "zh-HK", "电商_hk", "销售渠道.电商"),

            // dict.logistics.sales.channel.type.4
            ("dict.logistics.sales.channel.type.4", "en-US", "其他_us", "销售渠道.其他"),
            // dict.logistics.sales.channel.type.4
            ("dict.logistics.sales.channel.type.4", "ja-JP", "其他_jp", "销售渠道.其他"),
            // dict.logistics.sales.channel.type.4
            ("dict.logistics.sales.channel.type.4", "zh-CN", "其他", "销售渠道.其他"),
            // dict.logistics.sales.channel.type.4
            ("dict.logistics.sales.channel.type.4", "zh-HK", "其他_hk", "销售渠道.其他"),

            // dict.logistics.customer.level.category.0
            ("dict.logistics.customer.level.category.0", "en-US", "普通_us", "客户等级.普通"),
            // dict.logistics.customer.level.category.0
            ("dict.logistics.customer.level.category.0", "ja-JP", "普通_jp", "客户等级.普通"),
            // dict.logistics.customer.level.category.0
            ("dict.logistics.customer.level.category.0", "zh-CN", "普通", "客户等级.普通"),
            // dict.logistics.customer.level.category.0
            ("dict.logistics.customer.level.category.0", "zh-HK", "普通_hk", "客户等级.普通"),

            // dict.logistics.customer.level.category.1
            ("dict.logistics.customer.level.category.1", "en-US", "重要_us", "客户等级.重要"),
            // dict.logistics.customer.level.category.1
            ("dict.logistics.customer.level.category.1", "ja-JP", "重要_jp", "客户等级.重要"),
            // dict.logistics.customer.level.category.1
            ("dict.logistics.customer.level.category.1", "zh-CN", "重要", "客户等级.重要"),
            // dict.logistics.customer.level.category.1
            ("dict.logistics.customer.level.category.1", "zh-HK", "重要_hk", "客户等级.重要"),

            // dict.logistics.customer.level.category.2
            ("dict.logistics.customer.level.category.2", "en-US", "VIP_us", "客户等级.VIP"),
            // dict.logistics.customer.level.category.2
            ("dict.logistics.customer.level.category.2", "ja-JP", "VIP_jp", "客户等级.VIP"),
            // dict.logistics.customer.level.category.2
            ("dict.logistics.customer.level.category.2", "zh-CN", "VIP", "客户等级.VIP"),
            // dict.logistics.customer.level.category.2
            ("dict.logistics.customer.level.category.2", "zh-HK", "VIP_hk", "客户等级.VIP"),

            // dict.logistics.customer.level.category.3
            ("dict.logistics.customer.level.category.3", "en-US", "战略_us", "客户等级.战略"),
            // dict.logistics.customer.level.category.3
            ("dict.logistics.customer.level.category.3", "ja-JP", "战略_jp", "客户等级.战略"),
            // dict.logistics.customer.level.category.3
            ("dict.logistics.customer.level.category.3", "zh-CN", "战略", "客户等级.战略"),
            // dict.logistics.customer.level.category.3
            ("dict.logistics.customer.level.category.3", "zh-HK", "战略_hk", "客户等级.战略"),

            // dict.logistics.customer.category.0
            ("dict.logistics.customer.category.0", "en-US", "企业客户_us", "客户类型.企业客户"),
            // dict.logistics.customer.category.0
            ("dict.logistics.customer.category.0", "ja-JP", "企业客户_jp", "客户类型.企业客户"),
            // dict.logistics.customer.category.0
            ("dict.logistics.customer.category.0", "zh-CN", "企业客户", "客户类型.企业客户"),
            // dict.logistics.customer.category.0
            ("dict.logistics.customer.category.0", "zh-HK", "企业客户_hk", "客户类型.企业客户"),

            // dict.logistics.customer.category.1
            ("dict.logistics.customer.category.1", "en-US", "个人客户_us", "客户类型.个人客户"),
            // dict.logistics.customer.category.1
            ("dict.logistics.customer.category.1", "ja-JP", "个人客户_jp", "客户类型.个人客户"),
            // dict.logistics.customer.category.1
            ("dict.logistics.customer.category.1", "zh-CN", "个人客户", "客户类型.个人客户"),
            // dict.logistics.customer.category.1
            ("dict.logistics.customer.category.1", "zh-HK", "个人客户_hk", "客户类型.个人客户"),

            // dict.logistics.customer.category.2
            ("dict.logistics.customer.category.2", "en-US", "政府机构_us", "客户类型.政府机构"),
            // dict.logistics.customer.category.2
            ("dict.logistics.customer.category.2", "ja-JP", "政府机构_jp", "客户类型.政府机构"),
            // dict.logistics.customer.category.2
            ("dict.logistics.customer.category.2", "zh-CN", "政府机构", "客户类型.政府机构"),
            // dict.logistics.customer.category.2
            ("dict.logistics.customer.category.2", "zh-HK", "政府机构_hk", "客户类型.政府机构"),

            // dict.logistics.customer.category.3
            ("dict.logistics.customer.category.3", "en-US", "其他_us", "客户类型.其他"),
            // dict.logistics.customer.category.3
            ("dict.logistics.customer.category.3", "ja-JP", "其他_jp", "客户类型.其他"),
            // dict.logistics.customer.category.3
            ("dict.logistics.customer.category.3", "zh-CN", "其他", "客户类型.其他"),
            // dict.logistics.customer.category.3
            ("dict.logistics.customer.category.3", "zh-HK", "其他_hk", "客户类型.其他"),

            // dict.logistics.invoice.status.0
            ("dict.logistics.invoice.status.0", "en-US", "草稿_us", "发票状态.草稿"),
            // dict.logistics.invoice.status.0
            ("dict.logistics.invoice.status.0", "ja-JP", "草稿_jp", "发票状态.草稿"),
            // dict.logistics.invoice.status.0
            ("dict.logistics.invoice.status.0", "zh-CN", "草稿", "发票状态.草稿"),
            // dict.logistics.invoice.status.0
            ("dict.logistics.invoice.status.0", "zh-HK", "草稿_hk", "发票状态.草稿"),

            // dict.logistics.invoice.status.1
            ("dict.logistics.invoice.status.1", "en-US", "已开票_us", "发票状态.已开票"),
            // dict.logistics.invoice.status.1
            ("dict.logistics.invoice.status.1", "ja-JP", "已开票_jp", "发票状态.已开票"),
            // dict.logistics.invoice.status.1
            ("dict.logistics.invoice.status.1", "zh-CN", "已开票", "发票状态.已开票"),
            // dict.logistics.invoice.status.1
            ("dict.logistics.invoice.status.1", "zh-HK", "已开票_hk", "发票状态.已开票"),

            // dict.logistics.invoice.status.2
            ("dict.logistics.invoice.status.2", "en-US", "已收款_us", "发票状态.已收款"),
            // dict.logistics.invoice.status.2
            ("dict.logistics.invoice.status.2", "ja-JP", "已收款_jp", "发票状态.已收款"),
            // dict.logistics.invoice.status.2
            ("dict.logistics.invoice.status.2", "zh-CN", "已收款", "发票状态.已收款"),
            // dict.logistics.invoice.status.2
            ("dict.logistics.invoice.status.2", "zh-HK", "已收款_hk", "发票状态.已收款"),

            // dict.logistics.invoice.status.3
            ("dict.logistics.invoice.status.3", "en-US", "已作废_us", "发票状态.已作废"),
            // dict.logistics.invoice.status.3
            ("dict.logistics.invoice.status.3", "ja-JP", "已作废_jp", "发票状态.已作废"),
            // dict.logistics.invoice.status.3
            ("dict.logistics.invoice.status.3", "zh-CN", "已作废", "发票状态.已作废"),
            // dict.logistics.invoice.status.3
            ("dict.logistics.invoice.status.3", "zh-HK", "已作废_hk", "发票状态.已作废"),

            // dict.logistics.delivery.method.type.0
            ("dict.logistics.delivery.method.type.0", "en-US", "自提_us", "交货方式.自提"),
            // dict.logistics.delivery.method.type.0
            ("dict.logistics.delivery.method.type.0", "ja-JP", "自提_jp", "交货方式.自提"),
            // dict.logistics.delivery.method.type.0
            ("dict.logistics.delivery.method.type.0", "zh-CN", "自提", "交货方式.自提"),
            // dict.logistics.delivery.method.type.0
            ("dict.logistics.delivery.method.type.0", "zh-HK", "自提_hk", "交货方式.自提"),

            // dict.logistics.delivery.method.type.1
            ("dict.logistics.delivery.method.type.1", "en-US", "送货上门_us", "交货方式.送货上门"),
            // dict.logistics.delivery.method.type.1
            ("dict.logistics.delivery.method.type.1", "ja-JP", "送货上门_jp", "交货方式.送货上门"),
            // dict.logistics.delivery.method.type.1
            ("dict.logistics.delivery.method.type.1", "zh-CN", "送货上门", "交货方式.送货上门"),
            // dict.logistics.delivery.method.type.1
            ("dict.logistics.delivery.method.type.1", "zh-HK", "送货上门_hk", "交货方式.送货上门"),

            // dict.logistics.delivery.method.type.2
            ("dict.logistics.delivery.method.type.2", "en-US", "物流配送_us", "交货方式.物流配送"),
            // dict.logistics.delivery.method.type.2
            ("dict.logistics.delivery.method.type.2", "ja-JP", "物流配送_jp", "交货方式.物流配送"),
            // dict.logistics.delivery.method.type.2
            ("dict.logistics.delivery.method.type.2", "zh-CN", "物流配送", "交货方式.物流配送"),
            // dict.logistics.delivery.method.type.2
            ("dict.logistics.delivery.method.type.2", "zh-HK", "物流配送_hk", "交货方式.物流配送"),

            // dict.logistics.delivery.method.type.3
            ("dict.logistics.delivery.method.type.3", "en-US", "快递_us", "交货方式.快递"),
            // dict.logistics.delivery.method.type.3
            ("dict.logistics.delivery.method.type.3", "ja-JP", "快递_jp", "交货方式.快递"),
            // dict.logistics.delivery.method.type.3
            ("dict.logistics.delivery.method.type.3", "zh-CN", "快递", "交货方式.快递"),
            // dict.logistics.delivery.method.type.3
            ("dict.logistics.delivery.method.type.3", "zh-HK", "快递_hk", "交货方式.快递"),

            // dict.logistics.sales.price.type.0
            ("dict.logistics.sales.price.type.0", "en-US", "标准价格_us", "销售价格类型.标准价格"),
            // dict.logistics.sales.price.type.0
            ("dict.logistics.sales.price.type.0", "ja-JP", "标准价格_jp", "销售价格类型.标准价格"),
            // dict.logistics.sales.price.type.0
            ("dict.logistics.sales.price.type.0", "zh-CN", "标准价格", "销售价格类型.标准价格"),
            // dict.logistics.sales.price.type.0
            ("dict.logistics.sales.price.type.0", "zh-HK", "标准价格_hk", "销售价格类型.标准价格"),

            // dict.logistics.sales.price.type.1
            ("dict.logistics.sales.price.type.1", "en-US", "客户价格_us", "销售价格类型.客户价格"),
            // dict.logistics.sales.price.type.1
            ("dict.logistics.sales.price.type.1", "ja-JP", "客户价格_jp", "销售价格类型.客户价格"),
            // dict.logistics.sales.price.type.1
            ("dict.logistics.sales.price.type.1", "zh-CN", "客户价格", "销售价格类型.客户价格"),
            // dict.logistics.sales.price.type.1
            ("dict.logistics.sales.price.type.1", "zh-HK", "客户价格_hk", "销售价格类型.客户价格"),

            // dict.logistics.sales.price.type.2
            ("dict.logistics.sales.price.type.2", "en-US", "促销价格_us", "销售价格类型.促销价格"),
            // dict.logistics.sales.price.type.2
            ("dict.logistics.sales.price.type.2", "ja-JP", "促销价格_jp", "销售价格类型.促销价格"),
            // dict.logistics.sales.price.type.2
            ("dict.logistics.sales.price.type.2", "zh-CN", "促销价格", "销售价格类型.促销价格"),
            // dict.logistics.sales.price.type.2
            ("dict.logistics.sales.price.type.2", "zh-HK", "促销价格_hk", "销售价格类型.促销价格"),

            // dict.logistics.sales.price.type.3
            ("dict.logistics.sales.price.type.3", "en-US", "合同价格_us", "销售价格类型.合同价格"),
            // dict.logistics.sales.price.type.3
            ("dict.logistics.sales.price.type.3", "ja-JP", "合同价格_jp", "销售价格类型.合同价格"),
            // dict.logistics.sales.price.type.3
            ("dict.logistics.sales.price.type.3", "zh-CN", "合同价格", "销售价格类型.合同价格"),
            // dict.logistics.sales.price.type.3
            ("dict.logistics.sales.price.type.3", "zh-HK", "合同价格_hk", "销售价格类型.合同价格"),

            // dict.logistics.sales.price.type.4
            ("dict.logistics.sales.price.type.4", "en-US", "临时价格_us", "销售价格类型.临时价格"),
            // dict.logistics.sales.price.type.4
            ("dict.logistics.sales.price.type.4", "ja-JP", "临时价格_jp", "销售价格类型.临时价格"),
            // dict.logistics.sales.price.type.4
            ("dict.logistics.sales.price.type.4", "zh-CN", "临时价格", "销售价格类型.临时价格"),
            // dict.logistics.sales.price.type.4
            ("dict.logistics.sales.price.type.4", "zh-HK", "临时价格_hk", "销售价格类型.临时价格"),

            // dict.logistics.quotation.status.0
            ("dict.logistics.quotation.status.0", "en-US", "草稿_us", "报价状态.草稿"),
            // dict.logistics.quotation.status.0
            ("dict.logistics.quotation.status.0", "ja-JP", "草稿_jp", "报价状态.草稿"),
            // dict.logistics.quotation.status.0
            ("dict.logistics.quotation.status.0", "zh-CN", "草稿", "报价状态.草稿"),
            // dict.logistics.quotation.status.0
            ("dict.logistics.quotation.status.0", "zh-HK", "草稿_hk", "报价状态.草稿"),

            // dict.logistics.quotation.status.1
            ("dict.logistics.quotation.status.1", "en-US", "已发送_us", "报价状态.已发送"),
            // dict.logistics.quotation.status.1
            ("dict.logistics.quotation.status.1", "ja-JP", "已发送_jp", "报价状态.已发送"),
            // dict.logistics.quotation.status.1
            ("dict.logistics.quotation.status.1", "zh-CN", "已发送", "报价状态.已发送"),
            // dict.logistics.quotation.status.1
            ("dict.logistics.quotation.status.1", "zh-HK", "已发送_hk", "报价状态.已发送"),

            // dict.logistics.quotation.status.2
            ("dict.logistics.quotation.status.2", "en-US", "已接受_us", "报价状态.已接受"),
            // dict.logistics.quotation.status.2
            ("dict.logistics.quotation.status.2", "ja-JP", "已接受_jp", "报价状态.已接受"),
            // dict.logistics.quotation.status.2
            ("dict.logistics.quotation.status.2", "zh-CN", "已接受", "报价状态.已接受"),
            // dict.logistics.quotation.status.2
            ("dict.logistics.quotation.status.2", "zh-HK", "已接受_hk", "报价状态.已接受"),

            // dict.logistics.quotation.status.3
            ("dict.logistics.quotation.status.3", "en-US", "已拒绝_us", "报价状态.已拒绝"),
            // dict.logistics.quotation.status.3
            ("dict.logistics.quotation.status.3", "ja-JP", "已拒绝_jp", "报价状态.已拒绝"),
            // dict.logistics.quotation.status.3
            ("dict.logistics.quotation.status.3", "zh-CN", "已拒绝", "报价状态.已拒绝"),
            // dict.logistics.quotation.status.3
            ("dict.logistics.quotation.status.3", "zh-HK", "已拒绝_hk", "报价状态.已拒绝"),

            // dict.logistics.quotation.status.4
            ("dict.logistics.quotation.status.4", "en-US", "已过期_us", "报价状态.已过期"),
            // dict.logistics.quotation.status.4
            ("dict.logistics.quotation.status.4", "ja-JP", "已过期_jp", "报价状态.已过期"),
            // dict.logistics.quotation.status.4
            ("dict.logistics.quotation.status.4", "zh-CN", "已过期", "报价状态.已过期"),
            // dict.logistics.quotation.status.4
            ("dict.logistics.quotation.status.4", "zh-HK", "已过期_hk", "报价状态.已过期"),

            // dict.logistics.quotation.status.5
            ("dict.logistics.quotation.status.5", "en-US", "已作废_us", "报价状态.已作废"),
            // dict.logistics.quotation.status.5
            ("dict.logistics.quotation.status.5", "ja-JP", "已作废_jp", "报价状态.已作废"),
            // dict.logistics.quotation.status.5
            ("dict.logistics.quotation.status.5", "zh-CN", "已作废", "报价状态.已作废"),
            // dict.logistics.quotation.status.5
            ("dict.logistics.quotation.status.5", "zh-HK", "已作废_hk", "报价状态.已作废"),

            // dict.logistics.aoi.inspection.line.category.1
            ("dict.logistics.aoi.inspection.line.category.1", "en-US", "1_us", "aoi线别.1"),
            // dict.logistics.aoi.inspection.line.category.1
            ("dict.logistics.aoi.inspection.line.category.1", "ja-JP", "1_jp", "aoi线别.1"),
            // dict.logistics.aoi.inspection.line.category.1
            ("dict.logistics.aoi.inspection.line.category.1", "zh-CN", "1", "aoi线别.1"),
            // dict.logistics.aoi.inspection.line.category.1
            ("dict.logistics.aoi.inspection.line.category.1", "zh-HK", "1_hk", "aoi线别.1"),

            // dict.logistics.aoi.inspection.line.category.2
            ("dict.logistics.aoi.inspection.line.category.2", "en-US", "2_us", "aoi线别.2"),
            // dict.logistics.aoi.inspection.line.category.2
            ("dict.logistics.aoi.inspection.line.category.2", "ja-JP", "2_jp", "aoi线别.2"),
            // dict.logistics.aoi.inspection.line.category.2
            ("dict.logistics.aoi.inspection.line.category.2", "zh-CN", "2", "aoi线别.2"),
            // dict.logistics.aoi.inspection.line.category.2
            ("dict.logistics.aoi.inspection.line.category.2", "zh-HK", "2_hk", "aoi线别.2"),

            // dict.logistics.aoi.inspection.line.category.1a
            ("dict.logistics.aoi.inspection.line.category.1a", "en-US", "1a_us", "aoi线别.1a"),
            // dict.logistics.aoi.inspection.line.category.1a
            ("dict.logistics.aoi.inspection.line.category.1a", "ja-JP", "1a_jp", "aoi线别.1a"),
            // dict.logistics.aoi.inspection.line.category.1a
            ("dict.logistics.aoi.inspection.line.category.1a", "zh-CN", "1a", "aoi线别.1a"),
            // dict.logistics.aoi.inspection.line.category.1a
            ("dict.logistics.aoi.inspection.line.category.1a", "zh-HK", "1a_hk", "aoi线别.1a"),

            // dict.logistics.assy.location.category.1
            ("dict.logistics.assy.location.category.1", "en-US", "自插_us", "assy个所.自插"),
            // dict.logistics.assy.location.category.1
            ("dict.logistics.assy.location.category.1", "ja-JP", "自插_jp", "assy个所.自插"),
            // dict.logistics.assy.location.category.1
            ("dict.logistics.assy.location.category.1", "zh-CN", "自插", "assy个所.自插"),
            // dict.logistics.assy.location.category.1
            ("dict.logistics.assy.location.category.1", "zh-HK", "自插_hk", "assy个所.自插"),

            // dict.logistics.assy.location.category.2
            ("dict.logistics.assy.location.category.2", "en-US", "部品_us", "assy个所.部品"),
            // dict.logistics.assy.location.category.2
            ("dict.logistics.assy.location.category.2", "ja-JP", "部品_jp", "assy个所.部品"),
            // dict.logistics.assy.location.category.2
            ("dict.logistics.assy.location.category.2", "zh-CN", "部品", "assy个所.部品"),
            // dict.logistics.assy.location.category.2
            ("dict.logistics.assy.location.category.2", "zh-HK", "部品_hk", "assy个所.部品"),

            // dict.logistics.assy.location.category.3
            ("dict.logistics.assy.location.category.3", "en-US", "设计_us", "assy个所.设计"),
            // dict.logistics.assy.location.category.3
            ("dict.logistics.assy.location.category.3", "ja-JP", "设计_jp", "assy个所.设计"),
            // dict.logistics.assy.location.category.3
            ("dict.logistics.assy.location.category.3", "zh-CN", "设计", "assy个所.设计"),
            // dict.logistics.assy.location.category.3
            ("dict.logistics.assy.location.category.3", "zh-HK", "设计_hk", "assy个所.设计"),

            // dict.logistics.assy.location.category.4
            ("dict.logistics.assy.location.category.4", "en-US", "修正_us", "assy个所.修正"),
            // dict.logistics.assy.location.category.4
            ("dict.logistics.assy.location.category.4", "ja-JP", "修正_jp", "assy个所.修正"),
            // dict.logistics.assy.location.category.4
            ("dict.logistics.assy.location.category.4", "zh-CN", "修正", "assy个所.修正"),
            // dict.logistics.assy.location.category.4
            ("dict.logistics.assy.location.category.4", "zh-HK", "修正_hk", "assy个所.修正"),

            // dict.logistics.assy.location.category.5
            ("dict.logistics.assy.location.category.5", "en-US", "加工_us", "assy个所.加工"),
            // dict.logistics.assy.location.category.5
            ("dict.logistics.assy.location.category.5", "ja-JP", "加工_jp", "assy个所.加工"),
            // dict.logistics.assy.location.category.5
            ("dict.logistics.assy.location.category.5", "zh-CN", "加工", "assy个所.加工"),
            // dict.logistics.assy.location.category.5
            ("dict.logistics.assy.location.category.5", "zh-HK", "加工_hk", "assy个所.加工"),

            // dict.logistics.assy.location.category.6
            ("dict.logistics.assy.location.category.6", "en-US", "手插_us", "assy个所.手插"),
            // dict.logistics.assy.location.category.6
            ("dict.logistics.assy.location.category.6", "ja-JP", "手插_jp", "assy个所.手插"),
            // dict.logistics.assy.location.category.6
            ("dict.logistics.assy.location.category.6", "zh-CN", "手插", "assy个所.手插"),
            // dict.logistics.assy.location.category.6
            ("dict.logistics.assy.location.category.6", "zh-HK", "手插_hk", "assy个所.手插"),

            // dict.logistics.assy.location.category.7
            ("dict.logistics.assy.location.category.7", "en-US", "组立_us", "assy个所.组立"),
            // dict.logistics.assy.location.category.7
            ("dict.logistics.assy.location.category.7", "ja-JP", "组立_jp", "assy个所.组立"),
            // dict.logistics.assy.location.category.7
            ("dict.logistics.assy.location.category.7", "zh-CN", "组立", "assy个所.组立"),
            // dict.logistics.assy.location.category.7
            ("dict.logistics.assy.location.category.7", "zh-HK", "组立_hk", "assy个所.组立"),

            // dict.logistics.assy.location.category.8
            ("dict.logistics.assy.location.category.8", "en-US", "smt_us", "assy个所.smt"),
            // dict.logistics.assy.location.category.8
            ("dict.logistics.assy.location.category.8", "ja-JP", "smt_jp", "assy个所.smt"),
            // dict.logistics.assy.location.category.8
            ("dict.logistics.assy.location.category.8", "zh-CN", "smt", "assy个所.smt"),
            // dict.logistics.assy.location.category.8
            ("dict.logistics.assy.location.category.8", "zh-HK", "smt_hk", "assy个所.smt"),

            // dict.logistics.assy.location.category.9
            ("dict.logistics.assy.location.category.9", "en-US", "其他_us", "assy个所.其他"),
            // dict.logistics.assy.location.category.9
            ("dict.logistics.assy.location.category.9", "ja-JP", "其他_jp", "assy个所.其他"),
            // dict.logistics.assy.location.category.9
            ("dict.logistics.assy.location.category.9", "zh-CN", "其他", "assy个所.其他"),
            // dict.logistics.assy.location.category.9
            ("dict.logistics.assy.location.category.9", "zh-HK", "其他_hk", "assy个所.其他"),

            // dict.logistics.ec.distinction.category.1
            ("dict.logistics.ec.distinction.category.1", "en-US", "全仕向_us", "设变管理区分.全仕向"),
            // dict.logistics.ec.distinction.category.1
            ("dict.logistics.ec.distinction.category.1", "ja-JP", "全仕向_jp", "设变管理区分.全仕向"),
            // dict.logistics.ec.distinction.category.1
            ("dict.logistics.ec.distinction.category.1", "zh-CN", "全仕向", "设变管理区分.全仕向"),
            // dict.logistics.ec.distinction.category.1
            ("dict.logistics.ec.distinction.category.1", "zh-HK", "全仕向_hk", "设变管理区分.全仕向"),

            // dict.logistics.ec.distinction.category.2
            ("dict.logistics.ec.distinction.category.2", "en-US", "部管_us", "设变管理区分.部管"),
            // dict.logistics.ec.distinction.category.2
            ("dict.logistics.ec.distinction.category.2", "ja-JP", "部管_jp", "设变管理区分.部管"),
            // dict.logistics.ec.distinction.category.2
            ("dict.logistics.ec.distinction.category.2", "zh-CN", "部管", "设变管理区分.部管"),
            // dict.logistics.ec.distinction.category.2
            ("dict.logistics.ec.distinction.category.2", "zh-HK", "部管_hk", "设变管理区分.部管"),

            // dict.logistics.ec.distinction.category.3
            ("dict.logistics.ec.distinction.category.3", "en-US", "内部_us", "设变管理区分.内部"),
            // dict.logistics.ec.distinction.category.3
            ("dict.logistics.ec.distinction.category.3", "ja-JP", "内部_jp", "设变管理区分.内部"),
            // dict.logistics.ec.distinction.category.3
            ("dict.logistics.ec.distinction.category.3", "zh-CN", "内部", "设变管理区分.内部"),
            // dict.logistics.ec.distinction.category.3
            ("dict.logistics.ec.distinction.category.3", "zh-HK", "内部_hk", "设变管理区分.内部"),

            // dict.logistics.ec.distinction.category.4
            ("dict.logistics.ec.distinction.category.4", "en-US", "技术_us", "设变管理区分.技术"),
            // dict.logistics.ec.distinction.category.4
            ("dict.logistics.ec.distinction.category.4", "ja-JP", "技术_jp", "设变管理区分.技术"),
            // dict.logistics.ec.distinction.category.4
            ("dict.logistics.ec.distinction.category.4", "zh-CN", "技术", "设变管理区分.技术"),
            // dict.logistics.ec.distinction.category.4
            ("dict.logistics.ec.distinction.category.4", "zh-HK", "技术_hk", "设变管理区分.技术"),

            // dict.logistics.ec.status.1
            ("dict.logistics.ec.status.1", "en-US", "工作的_us", "设变状态.工作的"),
            // dict.logistics.ec.status.1
            ("dict.logistics.ec.status.1", "ja-JP", "工作的_jp", "设变状态.工作的"),
            // dict.logistics.ec.status.1
            ("dict.logistics.ec.status.1", "zh-CN", "工作的", "设变状态.工作的"),
            // dict.logistics.ec.status.1
            ("dict.logistics.ec.status.1", "zh-HK", "工作的_hk", "设变状态.工作的"),

            // dict.logistics.ec.status.2
            ("dict.logistics.ec.status.2", "en-US", "取消的_us", "设变状态.取消的"),
            // dict.logistics.ec.status.2
            ("dict.logistics.ec.status.2", "ja-JP", "取消的_jp", "设变状态.取消的"),
            // dict.logistics.ec.status.2
            ("dict.logistics.ec.status.2", "zh-CN", "取消的", "设变状态.取消的"),
            // dict.logistics.ec.status.2
            ("dict.logistics.ec.status.2", "zh-HK", "取消的_hk", "设变状态.取消的"),

            // dict.logistics.ec.status.3
            ("dict.logistics.ec.status.3", "en-US", "发行的_us", "设变状态.发行的"),
            // dict.logistics.ec.status.3
            ("dict.logistics.ec.status.3", "ja-JP", "发行的_jp", "设变状态.发行的"),
            // dict.logistics.ec.status.3
            ("dict.logistics.ec.status.3", "zh-CN", "发行的", "设变状态.发行的"),
            // dict.logistics.ec.status.3
            ("dict.logistics.ec.status.3", "zh-HK", "发行的_hk", "设变状态.发行的"),

            // dict.logistics.ec.status.4
            ("dict.logistics.ec.status.4", "en-US", "p.p中变更的_us", "设变状态.p.p中变更的"),
            // dict.logistics.ec.status.4
            ("dict.logistics.ec.status.4", "ja-JP", "p.p中变更的_jp", "设变状态.p.p中变更的"),
            // dict.logistics.ec.status.4
            ("dict.logistics.ec.status.4", "zh-CN", "p.p中变更的", "设变状态.p.p中变更的"),
            // dict.logistics.ec.status.4
            ("dict.logistics.ec.status.4", "zh-HK", "p.p中变更的_hk", "设变状态.p.p中变更的"),

            // dict.logistics.ec.status.5
            ("dict.logistics.ec.status.5", "en-US", "固定的_us", "设变状态.固定的"),
            // dict.logistics.ec.status.5
            ("dict.logistics.ec.status.5", "ja-JP", "固定的_jp", "设变状态.固定的"),
            // dict.logistics.ec.status.5
            ("dict.logistics.ec.status.5", "zh-CN", "固定的", "设变状态.固定的"),
            // dict.logistics.ec.status.5
            ("dict.logistics.ec.status.5", "zh-HK", "固定的_hk", "设变状态.固定的"),

            // dict.logistics.ec.status.6
            ("dict.logistics.ec.status.6", "en-US", "挂起的_us", "设变状态.挂起的"),
            // dict.logistics.ec.status.6
            ("dict.logistics.ec.status.6", "ja-JP", "挂起的_jp", "设变状态.挂起的"),
            // dict.logistics.ec.status.6
            ("dict.logistics.ec.status.6", "zh-CN", "挂起的", "设变状态.挂起的"),
            // dict.logistics.ec.status.6
            ("dict.logistics.ec.status.6", "zh-HK", "挂起的_hk", "设变状态.挂起的"),

            // dict.logistics.ec.status.7
            ("dict.logistics.ec.status.7", "en-US", "拒绝的_us", "设变状态.拒绝的"),
            // dict.logistics.ec.status.7
            ("dict.logistics.ec.status.7", "ja-JP", "拒绝的_jp", "设变状态.拒绝的"),
            // dict.logistics.ec.status.7
            ("dict.logistics.ec.status.7", "zh-CN", "拒绝的", "设变状态.拒绝的"),
            // dict.logistics.ec.status.7
            ("dict.logistics.ec.status.7", "zh-HK", "拒绝的_hk", "设变状态.拒绝的"),

            // dict.sys.equipment.status.0
            ("dict.sys.equipment.status.0", "en-US", "运行中_us", "设备状态.运行中"),
            // dict.sys.equipment.status.0
            ("dict.sys.equipment.status.0", "ja-JP", "运行中_jp", "设备状态.运行中"),
            // dict.sys.equipment.status.0
            ("dict.sys.equipment.status.0", "zh-CN", "运行中", "设备状态.运行中"),
            // dict.sys.equipment.status.0
            ("dict.sys.equipment.status.0", "zh-HK", "运行中_hk", "设备状态.运行中"),

            // dict.sys.equipment.status.1
            ("dict.sys.equipment.status.1", "en-US", "停机_us", "设备状态.停机"),
            // dict.sys.equipment.status.1
            ("dict.sys.equipment.status.1", "ja-JP", "停机_jp", "设备状态.停机"),
            // dict.sys.equipment.status.1
            ("dict.sys.equipment.status.1", "zh-CN", "停机", "设备状态.停机"),
            // dict.sys.equipment.status.1
            ("dict.sys.equipment.status.1", "zh-HK", "停机_hk", "设备状态.停机"),

            // dict.sys.equipment.status.2
            ("dict.sys.equipment.status.2", "en-US", "维修中_us", "设备状态.维修中"),
            // dict.sys.equipment.status.2
            ("dict.sys.equipment.status.2", "ja-JP", "维修中_jp", "设备状态.维修中"),
            // dict.sys.equipment.status.2
            ("dict.sys.equipment.status.2", "zh-CN", "维修中", "设备状态.维修中"),
            // dict.sys.equipment.status.2
            ("dict.sys.equipment.status.2", "zh-HK", "维修中_hk", "设备状态.维修中"),

            // dict.sys.equipment.status.3
            ("dict.sys.equipment.status.3", "en-US", "故障_us", "设备状态.故障"),
            // dict.sys.equipment.status.3
            ("dict.sys.equipment.status.3", "ja-JP", "故障_jp", "设备状态.故障"),
            // dict.sys.equipment.status.3
            ("dict.sys.equipment.status.3", "zh-CN", "故障", "设备状态.故障"),
            // dict.sys.equipment.status.3
            ("dict.sys.equipment.status.3", "zh-HK", "故障_hk", "设备状态.故障"),

            // dict.sys.equipment.status.4
            ("dict.sys.equipment.status.4", "en-US", "待报废_us", "设备状态.待报废"),
            // dict.sys.equipment.status.4
            ("dict.sys.equipment.status.4", "ja-JP", "待报废_jp", "设备状态.待报废"),
            // dict.sys.equipment.status.4
            ("dict.sys.equipment.status.4", "zh-CN", "待报废", "设备状态.待报废"),
            // dict.sys.equipment.status.4
            ("dict.sys.equipment.status.4", "zh-HK", "待报废_hk", "设备状态.待报废"),

            // dict.sys.equipment.status.5
            ("dict.sys.equipment.status.5", "en-US", "已报废_us", "设备状态.已报废"),
            // dict.sys.equipment.status.5
            ("dict.sys.equipment.status.5", "ja-JP", "已报废_jp", "设备状态.已报废"),
            // dict.sys.equipment.status.5
            ("dict.sys.equipment.status.5", "zh-CN", "已报废", "设备状态.已报废"),
            // dict.sys.equipment.status.5
            ("dict.sys.equipment.status.5", "zh-HK", "已报废_hk", "设备状态.已报废"),

            // dict.logistics.equipment.type.0
            ("dict.logistics.equipment.type.0", "en-US", "生产设备_us", "设备类型.生产设备"),
            // dict.logistics.equipment.type.0
            ("dict.logistics.equipment.type.0", "ja-JP", "生产设备_jp", "设备类型.生产设备"),
            // dict.logistics.equipment.type.0
            ("dict.logistics.equipment.type.0", "zh-CN", "生产设备", "设备类型.生产设备"),
            // dict.logistics.equipment.type.0
            ("dict.logistics.equipment.type.0", "zh-HK", "生产设备_hk", "设备类型.生产设备"),

            // dict.logistics.equipment.type.1
            ("dict.logistics.equipment.type.1", "en-US", "检测设备_us", "设备类型.检测设备"),
            // dict.logistics.equipment.type.1
            ("dict.logistics.equipment.type.1", "ja-JP", "检测设备_jp", "设备类型.检测设备"),
            // dict.logistics.equipment.type.1
            ("dict.logistics.equipment.type.1", "zh-CN", "检测设备", "设备类型.检测设备"),
            // dict.logistics.equipment.type.1
            ("dict.logistics.equipment.type.1", "zh-HK", "检测设备_hk", "设备类型.检测设备"),

            // dict.logistics.equipment.type.2
            ("dict.logistics.equipment.type.2", "en-US", "包装设备_us", "设备类型.包装设备"),
            // dict.logistics.equipment.type.2
            ("dict.logistics.equipment.type.2", "ja-JP", "包装设备_jp", "设备类型.包装设备"),
            // dict.logistics.equipment.type.2
            ("dict.logistics.equipment.type.2", "zh-CN", "包装设备", "设备类型.包装设备"),
            // dict.logistics.equipment.type.2
            ("dict.logistics.equipment.type.2", "zh-HK", "包装设备_hk", "设备类型.包装设备"),

            // dict.logistics.equipment.type.3
            ("dict.logistics.equipment.type.3", "en-US", "物流设备_us", "设备类型.物流设备"),
            // dict.logistics.equipment.type.3
            ("dict.logistics.equipment.type.3", "ja-JP", "物流设备_jp", "设备类型.物流设备"),
            // dict.logistics.equipment.type.3
            ("dict.logistics.equipment.type.3", "zh-CN", "物流设备", "设备类型.物流设备"),
            // dict.logistics.equipment.type.3
            ("dict.logistics.equipment.type.3", "zh-HK", "物流设备_hk", "设备类型.物流设备"),

            // dict.logistics.equipment.type.4
            ("dict.logistics.equipment.type.4", "en-US", "辅助设备_us", "设备类型.辅助设备"),
            // dict.logistics.equipment.type.4
            ("dict.logistics.equipment.type.4", "ja-JP", "辅助设备_jp", "设备类型.辅助设备"),
            // dict.logistics.equipment.type.4
            ("dict.logistics.equipment.type.4", "zh-CN", "辅助设备", "设备类型.辅助设备"),
            // dict.logistics.equipment.type.4
            ("dict.logistics.equipment.type.4", "zh-HK", "辅助设备_hk", "设备类型.辅助设备"),

            // dict.logistics.maintenance.type.0
            ("dict.logistics.maintenance.type.0", "en-US", "定期保养_us", "维护类型.定期保养"),
            // dict.logistics.maintenance.type.0
            ("dict.logistics.maintenance.type.0", "ja-JP", "定期保养_jp", "维护类型.定期保养"),
            // dict.logistics.maintenance.type.0
            ("dict.logistics.maintenance.type.0", "zh-CN", "定期保养", "维护类型.定期保养"),
            // dict.logistics.maintenance.type.0
            ("dict.logistics.maintenance.type.0", "zh-HK", "定期保养_hk", "维护类型.定期保养"),

            // dict.logistics.maintenance.type.1
            ("dict.logistics.maintenance.type.1", "en-US", "故障维修_us", "维护类型.故障维修"),
            // dict.logistics.maintenance.type.1
            ("dict.logistics.maintenance.type.1", "ja-JP", "故障维修_jp", "维护类型.故障维修"),
            // dict.logistics.maintenance.type.1
            ("dict.logistics.maintenance.type.1", "zh-CN", "故障维修", "维护类型.故障维修"),
            // dict.logistics.maintenance.type.1
            ("dict.logistics.maintenance.type.1", "zh-HK", "故障维修_hk", "维护类型.故障维修"),

            // dict.logistics.maintenance.type.2
            ("dict.logistics.maintenance.type.2", "en-US", "大修_us", "维护类型.大修"),
            // dict.logistics.maintenance.type.2
            ("dict.logistics.maintenance.type.2", "ja-JP", "大修_jp", "维护类型.大修"),
            // dict.logistics.maintenance.type.2
            ("dict.logistics.maintenance.type.2", "zh-CN", "大修", "维护类型.大修"),
            // dict.logistics.maintenance.type.2
            ("dict.logistics.maintenance.type.2", "zh-HK", "大修_hk", "维护类型.大修"),

            // dict.logistics.maintenance.type.3
            ("dict.logistics.maintenance.type.3", "en-US", "改造升级_us", "维护类型.改造升级"),
            // dict.logistics.maintenance.type.3
            ("dict.logistics.maintenance.type.3", "ja-JP", "改造升级_jp", "维护类型.改造升级"),
            // dict.logistics.maintenance.type.3
            ("dict.logistics.maintenance.type.3", "zh-CN", "改造升级", "维护类型.改造升级"),
            // dict.logistics.maintenance.type.3
            ("dict.logistics.maintenance.type.3", "zh-HK", "改造升级_hk", "维护类型.改造升级"),

            // dict.logistics.maintenance.type.4
            ("dict.logistics.maintenance.type.4", "en-US", "其他_us", "维护类型.其他"),
            // dict.logistics.maintenance.type.4
            ("dict.logistics.maintenance.type.4", "ja-JP", "其他_jp", "维护类型.其他"),
            // dict.logistics.maintenance.type.4
            ("dict.logistics.maintenance.type.4", "zh-CN", "其他", "维护类型.其他"),
            // dict.logistics.maintenance.type.4
            ("dict.logistics.maintenance.type.4", "zh-HK", "其他_hk", "维护类型.其他"),

            // dict.logistics.nonachievement.reason.category.1
            ("dict.logistics.nonachievement.reason.category.1", "en-US", "清机_us", "未达成原因.清机"),
            // dict.logistics.nonachievement.reason.category.1
            ("dict.logistics.nonachievement.reason.category.1", "ja-JP", "清机_jp", "未达成原因.清机"),
            // dict.logistics.nonachievement.reason.category.1
            ("dict.logistics.nonachievement.reason.category.1", "zh-CN", "清机", "未达成原因.清机"),
            // dict.logistics.nonachievement.reason.category.1
            ("dict.logistics.nonachievement.reason.category.1", "zh-HK", "清机_hk", "未达成原因.清机"),

            // dict.logistics.nonachievement.reason.category.2
            ("dict.logistics.nonachievement.reason.category.2", "en-US", "测试慢,测试修理机_us", "未达成原因.测试慢,测试修理机"),
            // dict.logistics.nonachievement.reason.category.2
            ("dict.logistics.nonachievement.reason.category.2", "ja-JP", "测试慢,测试修理机_jp", "未达成原因.测试慢,测试修理机"),
            // dict.logistics.nonachievement.reason.category.2
            ("dict.logistics.nonachievement.reason.category.2", "zh-CN", "测试慢,测试修理机", "未达成原因.测试慢,测试修理机"),
            // dict.logistics.nonachievement.reason.category.2
            ("dict.logistics.nonachievement.reason.category.2", "zh-HK", "测试慢,测试修理机_hk", "未达成原因.测试慢,测试修理机"),

            // dict.logistics.nonachievement.reason.category.3
            ("dict.logistics.nonachievement.reason.category.3", "en-US", "修理试机_us", "未达成原因.修理试机"),
            // dict.logistics.nonachievement.reason.category.3
            ("dict.logistics.nonachievement.reason.category.3", "ja-JP", "修理试机_jp", "未达成原因.修理试机"),
            // dict.logistics.nonachievement.reason.category.3
            ("dict.logistics.nonachievement.reason.category.3", "zh-CN", "修理试机", "未达成原因.修理试机"),
            // dict.logistics.nonachievement.reason.category.3
            ("dict.logistics.nonachievement.reason.category.3", "zh-HK", "修理试机_hk", "未达成原因.修理试机"),

            // dict.logistics.nonachievement.reason.category.4
            ("dict.logistics.nonachievement.reason.category.4", "en-US", "转机_us", "未达成原因.转机"),
            // dict.logistics.nonachievement.reason.category.4
            ("dict.logistics.nonachievement.reason.category.4", "ja-JP", "转机_jp", "未达成原因.转机"),
            // dict.logistics.nonachievement.reason.category.4
            ("dict.logistics.nonachievement.reason.category.4", "zh-CN", "转机", "未达成原因.转机"),
            // dict.logistics.nonachievement.reason.category.4
            ("dict.logistics.nonachievement.reason.category.4", "zh-HK", "转机_hk", "未达成原因.转机"),

            // dict.logistics.nonachievement.reason.category.5
            ("dict.logistics.nonachievement.reason.category.5", "en-US", "人员欠缺_us", "未达成原因.人员欠缺"),
            // dict.logistics.nonachievement.reason.category.5
            ("dict.logistics.nonachievement.reason.category.5", "ja-JP", "人员欠缺_jp", "未达成原因.人员欠缺"),
            // dict.logistics.nonachievement.reason.category.5
            ("dict.logistics.nonachievement.reason.category.5", "zh-CN", "人员欠缺", "未达成原因.人员欠缺"),
            // dict.logistics.nonachievement.reason.category.5
            ("dict.logistics.nonachievement.reason.category.5", "zh-HK", "人员欠缺_hk", "未达成原因.人员欠缺"),

            // dict.logistics.nonachievement.reason.category.6
            ("dict.logistics.nonachievement.reason.category.6", "en-US", "部品不良,欠料_us", "未达成原因.部品不良,欠料"),
            // dict.logistics.nonachievement.reason.category.6
            ("dict.logistics.nonachievement.reason.category.6", "ja-JP", "部品不良,欠料_jp", "未达成原因.部品不良,欠料"),
            // dict.logistics.nonachievement.reason.category.6
            ("dict.logistics.nonachievement.reason.category.6", "zh-CN", "部品不良,欠料", "未达成原因.部品不良,欠料"),
            // dict.logistics.nonachievement.reason.category.6
            ("dict.logistics.nonachievement.reason.category.6", "zh-HK", "部品不良,欠料_hk", "未达成原因.部品不良,欠料"),

            // dict.logistics.nonachievement.reason.category.7
            ("dict.logistics.nonachievement.reason.category.7", "en-US", "st差异大_us", "未达成原因.st差异大"),
            // dict.logistics.nonachievement.reason.category.7
            ("dict.logistics.nonachievement.reason.category.7", "ja-JP", "st差异大_jp", "未达成原因.st差异大"),
            // dict.logistics.nonachievement.reason.category.7
            ("dict.logistics.nonachievement.reason.category.7", "zh-CN", "st差异大", "未达成原因.st差异大"),
            // dict.logistics.nonachievement.reason.category.7
            ("dict.logistics.nonachievement.reason.category.7", "zh-HK", "st差异大_hk", "未达成原因.st差异大"),

            // dict.logistics.nonachievement.reason.category.8
            ("dict.logistics.nonachievement.reason.category.8", "en-US", "仪器设备,设置,调试,检查,故障,切换_us", "未达成原因.仪器设备,设置,调试,检查,故障,切换"),
            // dict.logistics.nonachievement.reason.category.8
            ("dict.logistics.nonachievement.reason.category.8", "ja-JP", "仪器设备,设置,调试,检查,故障,切换_jp", "未达成原因.仪器设备,设置,调试,检查,故障,切换"),
            // dict.logistics.nonachievement.reason.category.8
            ("dict.logistics.nonachievement.reason.category.8", "zh-CN", "仪器设备,设置,调试,检查,故障,切换", "未达成原因.仪器设备,设置,调试,检查,故障,切换"),
            // dict.logistics.nonachievement.reason.category.8
            ("dict.logistics.nonachievement.reason.category.8", "zh-HK", "仪器设备,设置,调试,检查,故障,切换_hk", "未达成原因.仪器设备,设置,调试,检查,故障,切换"),

            // dict.logistics.nonachievement.reason.category.9
            ("dict.logistics.nonachievement.reason.category.9", "en-US", "请假,旷工_us", "未达成原因.请假,旷工"),
            // dict.logistics.nonachievement.reason.category.9
            ("dict.logistics.nonachievement.reason.category.9", "ja-JP", "请假,旷工_jp", "未达成原因.请假,旷工"),
            // dict.logistics.nonachievement.reason.category.9
            ("dict.logistics.nonachievement.reason.category.9", "zh-CN", "请假,旷工", "未达成原因.请假,旷工"),
            // dict.logistics.nonachievement.reason.category.9
            ("dict.logistics.nonachievement.reason.category.9", "zh-HK", "请假,旷工_hk", "未达成原因.请假,旷工"),

            // dict.logistics.nonachievement.reason.category.10
            ("dict.logistics.nonachievement.reason.category.10", "en-US", "其他_us", "未达成原因.其他"),
            // dict.logistics.nonachievement.reason.category.10
            ("dict.logistics.nonachievement.reason.category.10", "ja-JP", "其他_jp", "未达成原因.其他"),
            // dict.logistics.nonachievement.reason.category.10
            ("dict.logistics.nonachievement.reason.category.10", "zh-CN", "其他", "未达成原因.其他"),
            // dict.logistics.nonachievement.reason.category.10
            ("dict.logistics.nonachievement.reason.category.10", "zh-HK", "其他_hk", "未达成原因.其他"),

            // dict.logistics.nonachievement.reason.category.11
            ("dict.logistics.nonachievement.reason.category.11", "en-US", "切换机种,仕向_us", "未达成原因.切换机种,仕向"),
            // dict.logistics.nonachievement.reason.category.11
            ("dict.logistics.nonachievement.reason.category.11", "ja-JP", "切换机种,仕向_jp", "未达成原因.切换机种,仕向"),
            // dict.logistics.nonachievement.reason.category.11
            ("dict.logistics.nonachievement.reason.category.11", "zh-CN", "切换机种,仕向", "未达成原因.切换机种,仕向"),
            // dict.logistics.nonachievement.reason.category.11
            ("dict.logistics.nonachievement.reason.category.11", "zh-HK", "切换机种,仕向_hk", "未达成原因.切换机种,仕向"),

            // dict.logistics.nonachievement.reason.category.12
            ("dict.logistics.nonachievement.reason.category.12", "en-US", "组立慢,加工多,工程多,下机慢,作业困难,升级慢_us", "未达成原因.组立慢,加工多,工程多,下机慢,作业困难,升级慢"),
            // dict.logistics.nonachievement.reason.category.12
            ("dict.logistics.nonachievement.reason.category.12", "ja-JP", "组立慢,加工多,工程多,下机慢,作业困难,升级慢_jp", "未达成原因.组立慢,加工多,工程多,下机慢,作业困难,升级慢"),
            // dict.logistics.nonachievement.reason.category.12
            ("dict.logistics.nonachievement.reason.category.12", "zh-CN", "组立慢,加工多,工程多,下机慢,作业困难,升级慢", "未达成原因.组立慢,加工多,工程多,下机慢,作业困难,升级慢"),
            // dict.logistics.nonachievement.reason.category.12
            ("dict.logistics.nonachievement.reason.category.12", "zh-HK", "组立慢,加工多,工程多,下机慢,作业困难,升级慢_hk", "未达成原因.组立慢,加工多,工程多,下机慢,作业困难,升级慢"),

            // dict.logistics.nonachievement.reason.category.13
            ("dict.logistics.nonachievement.reason.category.13", "en-US", "改修_us", "未达成原因.改修"),
            // dict.logistics.nonachievement.reason.category.13
            ("dict.logistics.nonachievement.reason.category.13", "ja-JP", "改修_jp", "未达成原因.改修"),
            // dict.logistics.nonachievement.reason.category.13
            ("dict.logistics.nonachievement.reason.category.13", "zh-CN", "改修", "未达成原因.改修"),
            // dict.logistics.nonachievement.reason.category.13
            ("dict.logistics.nonachievement.reason.category.13", "zh-HK", "改修_hk", "未达成原因.改修"),

            // dict.logistics.nonachievement.reason.category.14
            ("dict.logistics.nonachievement.reason.category.14", "en-US", "坏机多,不良多_us", "未达成原因.坏机多,不良多"),
            // dict.logistics.nonachievement.reason.category.14
            ("dict.logistics.nonachievement.reason.category.14", "ja-JP", "坏机多,不良多_jp", "未达成原因.坏机多,不良多"),
            // dict.logistics.nonachievement.reason.category.14
            ("dict.logistics.nonachievement.reason.category.14", "zh-CN", "坏机多,不良多", "未达成原因.坏机多,不良多"),
            // dict.logistics.nonachievement.reason.category.14
            ("dict.logistics.nonachievement.reason.category.14", "zh-HK", "坏机多,不良多_hk", "未达成原因.坏机多,不良多"),

            // dict.logistics.nonachievement.reason.category.15
            ("dict.logistics.nonachievement.reason.category.15", "en-US", "人员借调_us", "未达成原因.人员借调"),
            // dict.logistics.nonachievement.reason.category.15
            ("dict.logistics.nonachievement.reason.category.15", "ja-JP", "人员借调_jp", "未达成原因.人员借调"),
            // dict.logistics.nonachievement.reason.category.15
            ("dict.logistics.nonachievement.reason.category.15", "zh-CN", "人员借调", "未达成原因.人员借调"),
            // dict.logistics.nonachievement.reason.category.15
            ("dict.logistics.nonachievement.reason.category.15", "zh-HK", "人员借调_hk", "未达成原因.人员借调"),

            // dict.logistics.nonachievement.reason.category.16
            ("dict.logistics.nonachievement.reason.category.16", "en-US", "返工_us", "未达成原因.返工"),
            // dict.logistics.nonachievement.reason.category.16
            ("dict.logistics.nonachievement.reason.category.16", "ja-JP", "返工_jp", "未达成原因.返工"),
            // dict.logistics.nonachievement.reason.category.16
            ("dict.logistics.nonachievement.reason.category.16", "zh-CN", "返工", "未达成原因.返工"),
            // dict.logistics.nonachievement.reason.category.16
            ("dict.logistics.nonachievement.reason.category.16", "zh-HK", "返工_hk", "未达成原因.返工"),

            // dict.logistics.nonachievement.reason.category.17
            ("dict.logistics.nonachievement.reason.category.17", "en-US", "下机慢_us", "未达成原因.下机慢"),
            // dict.logistics.nonachievement.reason.category.17
            ("dict.logistics.nonachievement.reason.category.17", "ja-JP", "下机慢_jp", "未达成原因.下机慢"),
            // dict.logistics.nonachievement.reason.category.17
            ("dict.logistics.nonachievement.reason.category.17", "zh-CN", "下机慢", "未达成原因.下机慢"),
            // dict.logistics.nonachievement.reason.category.17
            ("dict.logistics.nonachievement.reason.category.17", "zh-HK", "下机慢_hk", "未达成原因.下机慢"),

            // dict.logistics.nonachievement.reason.category.18
            ("dict.logistics.nonachievement.reason.category.18", "en-US", "学习中,新人员学习,开会_us", "未达成原因.学习中,新人员学习,开会"),
            // dict.logistics.nonachievement.reason.category.18
            ("dict.logistics.nonachievement.reason.category.18", "ja-JP", "学习中,新人员学习,开会_jp", "未达成原因.学习中,新人员学习,开会"),
            // dict.logistics.nonachievement.reason.category.18
            ("dict.logistics.nonachievement.reason.category.18", "zh-CN", "学习中,新人员学习,开会", "未达成原因.学习中,新人员学习,开会"),
            // dict.logistics.nonachievement.reason.category.18
            ("dict.logistics.nonachievement.reason.category.18", "zh-HK", "学习中,新人员学习,开会_hk", "未达成原因.学习中,新人员学习,开会"),

            // dict.logistics.nonachievement.reason.category.19
            ("dict.logistics.nonachievement.reason.category.19", "en-US", "正常_us", "未达成原因.正常"),
            // dict.logistics.nonachievement.reason.category.19
            ("dict.logistics.nonachievement.reason.category.19", "ja-JP", "正常_jp", "未达成原因.正常"),
            // dict.logistics.nonachievement.reason.category.19
            ("dict.logistics.nonachievement.reason.category.19", "zh-CN", "正常", "未达成原因.正常"),
            // dict.logistics.nonachievement.reason.category.19
            ("dict.logistics.nonachievement.reason.category.19", "zh-HK", "正常_hk", "未达成原因.正常"),

            // dict.logistics.pcb.location.category.1
            ("dict.logistics.pcb.location.category.1", "en-US", "翘脚_us", "pcb个所.翘脚"),
            // dict.logistics.pcb.location.category.1
            ("dict.logistics.pcb.location.category.1", "ja-JP", "翘脚_jp", "pcb个所.翘脚"),
            // dict.logistics.pcb.location.category.1
            ("dict.logistics.pcb.location.category.1", "zh-CN", "翘脚", "pcb个所.翘脚"),
            // dict.logistics.pcb.location.category.1
            ("dict.logistics.pcb.location.category.1", "zh-HK", "翘脚_hk", "pcb个所.翘脚"),

            // dict.logistics.pcb.location.category.2
            ("dict.logistics.pcb.location.category.2", "en-US", "生锡_us", "pcb个所.生锡"),
            // dict.logistics.pcb.location.category.2
            ("dict.logistics.pcb.location.category.2", "ja-JP", "生锡_jp", "pcb个所.生锡"),
            // dict.logistics.pcb.location.category.2
            ("dict.logistics.pcb.location.category.2", "zh-CN", "生锡", "pcb个所.生锡"),
            // dict.logistics.pcb.location.category.2
            ("dict.logistics.pcb.location.category.2", "zh-HK", "生锡_hk", "pcb个所.生锡"),

            // dict.logistics.pcb.location.category.3
            ("dict.logistics.pcb.location.category.3", "en-US", "锡量过多_us", "pcb个所.锡量过多"),
            // dict.logistics.pcb.location.category.3
            ("dict.logistics.pcb.location.category.3", "ja-JP", "锡量过多_jp", "pcb个所.锡量过多"),
            // dict.logistics.pcb.location.category.3
            ("dict.logistics.pcb.location.category.3", "zh-CN", "锡量过多", "pcb个所.锡量过多"),
            // dict.logistics.pcb.location.category.3
            ("dict.logistics.pcb.location.category.3", "zh-HK", "锡量过多_hk", "pcb个所.锡量过多"),

            // dict.logistics.pcb.location.category.4
            ("dict.logistics.pcb.location.category.4", "en-US", "空焊_us", "pcb个所.空焊"),
            // dict.logistics.pcb.location.category.4
            ("dict.logistics.pcb.location.category.4", "ja-JP", "空焊_jp", "pcb个所.空焊"),
            // dict.logistics.pcb.location.category.4
            ("dict.logistics.pcb.location.category.4", "zh-CN", "空焊", "pcb个所.空焊"),
            // dict.logistics.pcb.location.category.4
            ("dict.logistics.pcb.location.category.4", "zh-HK", "空焊_hk", "pcb个所.空焊"),

            // dict.logistics.pcb.location.category.5
            ("dict.logistics.pcb.location.category.5", "en-US", "漏件_us", "pcb个所.漏件"),
            // dict.logistics.pcb.location.category.5
            ("dict.logistics.pcb.location.category.5", "ja-JP", "漏件_jp", "pcb个所.漏件"),
            // dict.logistics.pcb.location.category.5
            ("dict.logistics.pcb.location.category.5", "zh-CN", "漏件", "pcb个所.漏件"),
            // dict.logistics.pcb.location.category.5
            ("dict.logistics.pcb.location.category.5", "zh-HK", "漏件_hk", "pcb个所.漏件"),

            // dict.logistics.pcb.location.category.6
            ("dict.logistics.pcb.location.category.6", "en-US", "发黄_us", "pcb个所.发黄"),
            // dict.logistics.pcb.location.category.6
            ("dict.logistics.pcb.location.category.6", "ja-JP", "发黄_jp", "pcb个所.发黄"),
            // dict.logistics.pcb.location.category.6
            ("dict.logistics.pcb.location.category.6", "zh-CN", "发黄", "pcb个所.发黄"),
            // dict.logistics.pcb.location.category.6
            ("dict.logistics.pcb.location.category.6", "zh-HK", "发黄_hk", "pcb个所.发黄"),

            // dict.logistics.pcb.location.category.7
            ("dict.logistics.pcb.location.category.7", "en-US", "ic pin 竖立_us", "pcb个所.ic pin 竖立"),
            // dict.logistics.pcb.location.category.7
            ("dict.logistics.pcb.location.category.7", "ja-JP", "ic pin 竖立_jp", "pcb个所.ic pin 竖立"),
            // dict.logistics.pcb.location.category.7
            ("dict.logistics.pcb.location.category.7", "zh-CN", "ic pin 竖立", "pcb个所.ic pin 竖立"),
            // dict.logistics.pcb.location.category.7
            ("dict.logistics.pcb.location.category.7", "zh-HK", "ic pin 竖立_hk", "pcb个所.ic pin 竖立"),

            // dict.logistics.pcb.location.category.8
            ("dict.logistics.pcb.location.category.8", "en-US", "连锡_us", "pcb个所.连锡"),
            // dict.logistics.pcb.location.category.8
            ("dict.logistics.pcb.location.category.8", "ja-JP", "连锡_jp", "pcb个所.连锡"),
            // dict.logistics.pcb.location.category.8
            ("dict.logistics.pcb.location.category.8", "zh-CN", "连锡", "pcb个所.连锡"),
            // dict.logistics.pcb.location.category.8
            ("dict.logistics.pcb.location.category.8", "zh-HK", "连锡_hk", "pcb个所.连锡"),

            // dict.logistics.pcb.location.category.9
            ("dict.logistics.pcb.location.category.9", "en-US", "异物附着_us", "pcb个所.异物附着"),
            // dict.logistics.pcb.location.category.9
            ("dict.logistics.pcb.location.category.9", "ja-JP", "异物附着_jp", "pcb个所.异物附着"),
            // dict.logistics.pcb.location.category.9
            ("dict.logistics.pcb.location.category.9", "zh-CN", "异物附着", "pcb个所.异物附着"),
            // dict.logistics.pcb.location.category.9
            ("dict.logistics.pcb.location.category.9", "zh-HK", "异物附着_hk", "pcb个所.异物附着"),

            // dict.logistics.pcb.location.category.10
            ("dict.logistics.pcb.location.category.10", "en-US", "底下有部品_us", "pcb个所.底下有部品"),
            // dict.logistics.pcb.location.category.10
            ("dict.logistics.pcb.location.category.10", "ja-JP", "底下有部品_jp", "pcb个所.底下有部品"),
            // dict.logistics.pcb.location.category.10
            ("dict.logistics.pcb.location.category.10", "zh-CN", "底下有部品", "pcb个所.底下有部品"),
            // dict.logistics.pcb.location.category.10
            ("dict.logistics.pcb.location.category.10", "zh-HK", "底下有部品_hk", "pcb个所.底下有部品"),

            // dict.logistics.pcb.location.category.11
            ("dict.logistics.pcb.location.category.11", "en-US", "基板不良_us", "pcb个所.基板不良"),
            // dict.logistics.pcb.location.category.11
            ("dict.logistics.pcb.location.category.11", "ja-JP", "基板不良_jp", "pcb个所.基板不良"),
            // dict.logistics.pcb.location.category.11
            ("dict.logistics.pcb.location.category.11", "zh-CN", "基板不良", "pcb个所.基板不良"),
            // dict.logistics.pcb.location.category.11
            ("dict.logistics.pcb.location.category.11", "zh-HK", "基板不良_hk", "pcb个所.基板不良"),

            // dict.logistics.pcb.location.category.12
            ("dict.logistics.pcb.location.category.12", "en-US", "ic pin 浮高_us", "pcb个所.ic pin 浮高"),
            // dict.logistics.pcb.location.category.12
            ("dict.logistics.pcb.location.category.12", "ja-JP", "ic pin 浮高_jp", "pcb个所.ic pin 浮高"),
            // dict.logistics.pcb.location.category.12
            ("dict.logistics.pcb.location.category.12", "zh-CN", "ic pin 浮高", "pcb个所.ic pin 浮高"),
            // dict.logistics.pcb.location.category.12
            ("dict.logistics.pcb.location.category.12", "zh-HK", "ic pin 浮高_hk", "pcb个所.ic pin 浮高"),

            // dict.logistics.pcb.location.category.13
            ("dict.logistics.pcb.location.category.13", "en-US", "红胶不良_us", "pcb个所.红胶不良"),
            // dict.logistics.pcb.location.category.13
            ("dict.logistics.pcb.location.category.13", "ja-JP", "红胶不良_jp", "pcb个所.红胶不良"),
            // dict.logistics.pcb.location.category.13
            ("dict.logistics.pcb.location.category.13", "zh-CN", "红胶不良", "pcb个所.红胶不良"),
            // dict.logistics.pcb.location.category.13
            ("dict.logistics.pcb.location.category.13", "zh-HK", "红胶不良_hk", "pcb个所.红胶不良"),

            // dict.logistics.pcb.location.category.14
            ("dict.logistics.pcb.location.category.14", "en-US", "反面_us", "pcb个所.反面"),
            // dict.logistics.pcb.location.category.14
            ("dict.logistics.pcb.location.category.14", "ja-JP", "反面_jp", "pcb个所.反面"),
            // dict.logistics.pcb.location.category.14
            ("dict.logistics.pcb.location.category.14", "zh-CN", "反面", "pcb个所.反面"),
            // dict.logistics.pcb.location.category.14
            ("dict.logistics.pcb.location.category.14", "zh-HK", "反面_hk", "pcb个所.反面"),

            // dict.logistics.pcb.location.category.15
            ("dict.logistics.pcb.location.category.15", "en-US", "位置偏移_us", "pcb个所.位置偏移"),
            // dict.logistics.pcb.location.category.15
            ("dict.logistics.pcb.location.category.15", "ja-JP", "位置偏移_jp", "pcb个所.位置偏移"),
            // dict.logistics.pcb.location.category.15
            ("dict.logistics.pcb.location.category.15", "zh-CN", "位置偏移", "pcb个所.位置偏移"),
            // dict.logistics.pcb.location.category.15
            ("dict.logistics.pcb.location.category.15", "zh-HK", "位置偏移_hk", "pcb个所.位置偏移"),

            // dict.logistics.pcb.location.category.16
            ("dict.logistics.pcb.location.category.16", "en-US", "部品不良_us", "pcb个所.部品不良"),
            // dict.logistics.pcb.location.category.16
            ("dict.logistics.pcb.location.category.16", "ja-JP", "部品不良_jp", "pcb个所.部品不良"),
            // dict.logistics.pcb.location.category.16
            ("dict.logistics.pcb.location.category.16", "zh-CN", "部品不良", "pcb个所.部品不良"),
            // dict.logistics.pcb.location.category.16
            ("dict.logistics.pcb.location.category.16", "zh-HK", "部品不良_hk", "pcb个所.部品不良"),

            // dict.logistics.pcb.location.category.17
            ("dict.logistics.pcb.location.category.17", "en-US", "部品破损_us", "pcb个所.部品破损"),
            // dict.logistics.pcb.location.category.17
            ("dict.logistics.pcb.location.category.17", "ja-JP", "部品破损_jp", "pcb个所.部品破损"),
            // dict.logistics.pcb.location.category.17
            ("dict.logistics.pcb.location.category.17", "zh-CN", "部品破损", "pcb个所.部品破损"),
            // dict.logistics.pcb.location.category.17
            ("dict.logistics.pcb.location.category.17", "zh-HK", "部品破损_hk", "pcb个所.部品破损"),

            // dict.logistics.pcb.location.category.18
            ("dict.logistics.pcb.location.category.18", "en-US", "立碑_us", "pcb个所.立碑"),
            // dict.logistics.pcb.location.category.18
            ("dict.logistics.pcb.location.category.18", "ja-JP", "立碑_jp", "pcb个所.立碑"),
            // dict.logistics.pcb.location.category.18
            ("dict.logistics.pcb.location.category.18", "zh-CN", "立碑", "pcb个所.立碑"),
            // dict.logistics.pcb.location.category.18
            ("dict.logistics.pcb.location.category.18", "zh-HK", "立碑_hk", "pcb个所.立碑"),

            // dict.logistics.pcb.location.category.19
            ("dict.logistics.pcb.location.category.19", "en-US", "翻面_us", "pcb个所.翻面"),
            // dict.logistics.pcb.location.category.19
            ("dict.logistics.pcb.location.category.19", "ja-JP", "翻面_jp", "pcb个所.翻面"),
            // dict.logistics.pcb.location.category.19
            ("dict.logistics.pcb.location.category.19", "zh-CN", "翻面", "pcb个所.翻面"),
            // dict.logistics.pcb.location.category.19
            ("dict.logistics.pcb.location.category.19", "zh-HK", "翻面_hk", "pcb个所.翻面"),

            // dict.logistics.pcb.location.category.20
            ("dict.logistics.pcb.location.category.20", "en-US", "撞件_us", "pcb个所.撞件"),
            // dict.logistics.pcb.location.category.20
            ("dict.logistics.pcb.location.category.20", "ja-JP", "撞件_jp", "pcb个所.撞件"),
            // dict.logistics.pcb.location.category.20
            ("dict.logistics.pcb.location.category.20", "zh-CN", "撞件", "pcb个所.撞件"),
            // dict.logistics.pcb.location.category.20
            ("dict.logistics.pcb.location.category.20", "zh-HK", "撞件_hk", "pcb个所.撞件"),

            // dict.logistics.pcb.location.category.21
            ("dict.logistics.pcb.location.category.21", "en-US", "错料_us", "pcb个所.错料"),
            // dict.logistics.pcb.location.category.21
            ("dict.logistics.pcb.location.category.21", "ja-JP", "错料_jp", "pcb个所.错料"),
            // dict.logistics.pcb.location.category.21
            ("dict.logistics.pcb.location.category.21", "zh-CN", "错料", "pcb个所.错料"),
            // dict.logistics.pcb.location.category.21
            ("dict.logistics.pcb.location.category.21", "zh-HK", "错料_hk", "pcb个所.错料"),

            // dict.logistics.pcb.location.category.22
            ("dict.logistics.pcb.location.category.22", "en-US", "侧立_us", "pcb个所.侧立"),
            // dict.logistics.pcb.location.category.22
            ("dict.logistics.pcb.location.category.22", "ja-JP", "侧立_jp", "pcb个所.侧立"),
            // dict.logistics.pcb.location.category.22
            ("dict.logistics.pcb.location.category.22", "zh-CN", "侧立", "pcb个所.侧立"),
            // dict.logistics.pcb.location.category.22
            ("dict.logistics.pcb.location.category.22", "zh-HK", "侧立_hk", "pcb个所.侧立"),

            // dict.logistics.pcb.location.category.23
            ("dict.logistics.pcb.location.category.23", "en-US", "反向_us", "pcb个所.反向"),
            // dict.logistics.pcb.location.category.23
            ("dict.logistics.pcb.location.category.23", "ja-JP", "反向_jp", "pcb个所.反向"),
            // dict.logistics.pcb.location.category.23
            ("dict.logistics.pcb.location.category.23", "zh-CN", "反向", "pcb个所.反向"),
            // dict.logistics.pcb.location.category.23
            ("dict.logistics.pcb.location.category.23", "zh-HK", "反向_hk", "pcb个所.反向"),

            // dict.logistics.pcb.location.category.24
            ("dict.logistics.pcb.location.category.24", "en-US", "pcb不良_us", "pcb个所.pcb不良"),
            // dict.logistics.pcb.location.category.24
            ("dict.logistics.pcb.location.category.24", "ja-JP", "pcb不良_jp", "pcb个所.pcb不良"),
            // dict.logistics.pcb.location.category.24
            ("dict.logistics.pcb.location.category.24", "zh-CN", "pcb不良", "pcb个所.pcb不良"),
            // dict.logistics.pcb.location.category.24
            ("dict.logistics.pcb.location.category.24", "zh-HK", "pcb不良_hk", "pcb个所.pcb不良"),

            // dict.logistics.pcb.location.category.25
            ("dict.logistics.pcb.location.category.25", "en-US", "焊接不良_us", "pcb个所.焊接不良"),
            // dict.logistics.pcb.location.category.25
            ("dict.logistics.pcb.location.category.25", "ja-JP", "焊接不良_jp", "pcb个所.焊接不良"),
            // dict.logistics.pcb.location.category.25
            ("dict.logistics.pcb.location.category.25", "zh-CN", "焊接不良", "pcb个所.焊接不良"),
            // dict.logistics.pcb.location.category.25
            ("dict.logistics.pcb.location.category.25", "zh-HK", "焊接不良_hk", "pcb个所.焊接不良"),

            // dict.logistics.pcb.location.category.26
            ("dict.logistics.pcb.location.category.26", "en-US", "极性相违_us", "pcb个所.极性相违"),
            // dict.logistics.pcb.location.category.26
            ("dict.logistics.pcb.location.category.26", "ja-JP", "极性相违_jp", "pcb个所.极性相违"),
            // dict.logistics.pcb.location.category.26
            ("dict.logistics.pcb.location.category.26", "zh-CN", "极性相违", "pcb个所.极性相违"),
            // dict.logistics.pcb.location.category.26
            ("dict.logistics.pcb.location.category.26", "zh-HK", "极性相违_hk", "pcb个所.极性相违"),

            // dict.logistics.pcb.location.category.27
            ("dict.logistics.pcb.location.category.27", "en-US", "多件_us", "pcb个所.多件"),
            // dict.logistics.pcb.location.category.27
            ("dict.logistics.pcb.location.category.27", "ja-JP", "多件_jp", "pcb个所.多件"),
            // dict.logistics.pcb.location.category.27
            ("dict.logistics.pcb.location.category.27", "zh-CN", "多件", "pcb个所.多件"),
            // dict.logistics.pcb.location.category.27
            ("dict.logistics.pcb.location.category.27", "zh-HK", "多件_hk", "pcb个所.多件"),

            // dict.logistics.pcb.location.category.28
            ("dict.logistics.pcb.location.category.28", "en-US", "锡少_us", "pcb个所.锡少"),
            // dict.logistics.pcb.location.category.28
            ("dict.logistics.pcb.location.category.28", "ja-JP", "锡少_jp", "pcb个所.锡少"),
            // dict.logistics.pcb.location.category.28
            ("dict.logistics.pcb.location.category.28", "zh-CN", "锡少", "pcb个所.锡少"),
            // dict.logistics.pcb.location.category.28
            ("dict.logistics.pcb.location.category.28", "zh-HK", "锡少_hk", "pcb个所.锡少"),

            // dict.logistics.pcba.function.category.1
            ("dict.logistics.pcba.function.category.1", "en-US", "a_us", "pcba功能类别.a"),
            // dict.logistics.pcba.function.category.1
            ("dict.logistics.pcba.function.category.1", "ja-JP", "a_jp", "pcba功能类别.a"),
            // dict.logistics.pcba.function.category.1
            ("dict.logistics.pcba.function.category.1", "zh-CN", "a", "pcba功能类别.a"),
            // dict.logistics.pcba.function.category.1
            ("dict.logistics.pcba.function.category.1", "zh-HK", "a_hk", "pcba功能类别.a"),

            // dict.logistics.pcba.function.category.2
            ("dict.logistics.pcba.function.category.2", "en-US", "adoc_us", "pcba功能类别.adoc"),
            // dict.logistics.pcba.function.category.2
            ("dict.logistics.pcba.function.category.2", "ja-JP", "adoc_jp", "pcba功能类别.adoc"),
            // dict.logistics.pcba.function.category.2
            ("dict.logistics.pcba.function.category.2", "zh-CN", "adoc", "pcba功能类别.adoc"),
            // dict.logistics.pcba.function.category.2
            ("dict.logistics.pcba.function.category.2", "zh-HK", "adoc_hk", "pcba功能类别.adoc"),

            // dict.logistics.pcba.function.category.3
            ("dict.logistics.pcba.function.category.3", "en-US", "ana_us", "pcba功能类别.ana"),
            // dict.logistics.pcba.function.category.3
            ("dict.logistics.pcba.function.category.3", "ja-JP", "ana_jp", "pcba功能类别.ana"),
            // dict.logistics.pcba.function.category.3
            ("dict.logistics.pcba.function.category.3", "zh-CN", "ana", "pcba功能类别.ana"),
            // dict.logistics.pcba.function.category.3
            ("dict.logistics.pcba.function.category.3", "zh-HK", "ana_hk", "pcba功能类别.ana"),

            // dict.logistics.pcba.function.category.4
            ("dict.logistics.pcba.function.category.4", "en-US", "audio_us", "pcba功能类别.audio"),
            // dict.logistics.pcba.function.category.4
            ("dict.logistics.pcba.function.category.4", "ja-JP", "audio_jp", "pcba功能类别.audio"),
            // dict.logistics.pcba.function.category.4
            ("dict.logistics.pcba.function.category.4", "zh-CN", "audio", "pcba功能类别.audio"),
            // dict.logistics.pcba.function.category.4
            ("dict.logistics.pcba.function.category.4", "zh-HK", "audio_hk", "pcba功能类别.audio"),

            // dict.logistics.pcba.function.category.5
            ("dict.logistics.pcba.function.category.5", "en-US", "b_us", "pcba功能类别.b"),
            // dict.logistics.pcba.function.category.5
            ("dict.logistics.pcba.function.category.5", "ja-JP", "b_jp", "pcba功能类别.b"),
            // dict.logistics.pcba.function.category.5
            ("dict.logistics.pcba.function.category.5", "zh-CN", "b", "pcba功能类别.b"),
            // dict.logistics.pcba.function.category.5
            ("dict.logistics.pcba.function.category.5", "zh-HK", "b_hk", "pcba功能类别.b"),

            // dict.logistics.pcba.function.category.6
            ("dict.logistics.pcba.function.category.6", "en-US", "bottom_us", "pcba功能类别.bottom"),
            // dict.logistics.pcba.function.category.6
            ("dict.logistics.pcba.function.category.6", "ja-JP", "bottom_jp", "pcba功能类别.bottom"),
            // dict.logistics.pcba.function.category.6
            ("dict.logistics.pcba.function.category.6", "zh-CN", "bottom", "pcba功能类别.bottom"),
            // dict.logistics.pcba.function.category.6
            ("dict.logistics.pcba.function.category.6", "zh-HK", "bottom_hk", "pcba功能类别.bottom"),

            // dict.logistics.pcba.function.category.7
            ("dict.logistics.pcba.function.category.7", "en-US", "btice_us", "pcba功能类别.btice"),
            // dict.logistics.pcba.function.category.7
            ("dict.logistics.pcba.function.category.7", "ja-JP", "btice_jp", "pcba功能类别.btice"),
            // dict.logistics.pcba.function.category.7
            ("dict.logistics.pcba.function.category.7", "zh-CN", "btice", "pcba功能类别.btice"),
            // dict.logistics.pcba.function.category.7
            ("dict.logistics.pcba.function.category.7", "zh-HK", "btice_hk", "pcba功能类别.btice"),

            // dict.logistics.pcba.function.category.8
            ("dict.logistics.pcba.function.category.8", "en-US", "c_us", "pcba功能类别.c"),
            // dict.logistics.pcba.function.category.8
            ("dict.logistics.pcba.function.category.8", "ja-JP", "c_jp", "pcba功能类别.c"),
            // dict.logistics.pcba.function.category.8
            ("dict.logistics.pcba.function.category.8", "zh-CN", "c", "pcba功能类别.c"),
            // dict.logistics.pcba.function.category.8
            ("dict.logistics.pcba.function.category.8", "zh-HK", "c_hk", "pcba功能类别.c"),

            // dict.logistics.pcba.function.category.9
            ("dict.logistics.pcba.function.category.9", "en-US", "dspl_us", "pcba功能类别.dspl"),
            // dict.logistics.pcba.function.category.9
            ("dict.logistics.pcba.function.category.9", "ja-JP", "dspl_jp", "pcba功能类别.dspl"),
            // dict.logistics.pcba.function.category.9
            ("dict.logistics.pcba.function.category.9", "zh-CN", "dspl", "pcba功能类别.dspl"),
            // dict.logistics.pcba.function.category.9
            ("dict.logistics.pcba.function.category.9", "zh-HK", "dspl_hk", "pcba功能类别.dspl"),

            // dict.logistics.pcba.function.category.10
            ("dict.logistics.pcba.function.category.10", "en-US", "enc_us", "pcba功能类别.enc"),
            // dict.logistics.pcba.function.category.10
            ("dict.logistics.pcba.function.category.10", "ja-JP", "enc_jp", "pcba功能类别.enc"),
            // dict.logistics.pcba.function.category.10
            ("dict.logistics.pcba.function.category.10", "zh-CN", "enc", "pcba功能类别.enc"),
            // dict.logistics.pcba.function.category.10
            ("dict.logistics.pcba.function.category.10", "zh-HK", "enc_hk", "pcba功能类别.enc"),

            // dict.logistics.pcba.function.category.11
            ("dict.logistics.pcba.function.category.11", "en-US", "front_us", "pcba功能类别.front"),
            // dict.logistics.pcba.function.category.11
            ("dict.logistics.pcba.function.category.11", "ja-JP", "front_jp", "pcba功能类别.front"),
            // dict.logistics.pcba.function.category.11
            ("dict.logistics.pcba.function.category.11", "zh-CN", "front", "pcba功能类别.front"),
            // dict.logistics.pcba.function.category.11
            ("dict.logistics.pcba.function.category.11", "zh-HK", "front_hk", "pcba功能类别.front"),

            // dict.logistics.pcba.function.category.12
            ("dict.logistics.pcba.function.category.12", "en-US", "input_us", "pcba功能类别.input"),
            // dict.logistics.pcba.function.category.12
            ("dict.logistics.pcba.function.category.12", "ja-JP", "input_jp", "pcba功能类别.input"),
            // dict.logistics.pcba.function.category.12
            ("dict.logistics.pcba.function.category.12", "zh-CN", "input", "pcba功能类别.input"),
            // dict.logistics.pcba.function.category.12
            ("dict.logistics.pcba.function.category.12", "zh-HK", "input_hk", "pcba功能类别.input"),

            // dict.logistics.pcba.function.category.13
            ("dict.logistics.pcba.function.category.13", "en-US", "io_us", "pcba功能类别.io"),
            // dict.logistics.pcba.function.category.13
            ("dict.logistics.pcba.function.category.13", "ja-JP", "io_jp", "pcba功能类别.io"),
            // dict.logistics.pcba.function.category.13
            ("dict.logistics.pcba.function.category.13", "zh-CN", "io", "pcba功能类别.io"),
            // dict.logistics.pcba.function.category.13
            ("dict.logistics.pcba.function.category.13", "zh-HK", "io_hk", "pcba功能类别.io"),

            // dict.logistics.pcba.function.category.14
            ("dict.logistics.pcba.function.category.14", "en-US", "jack_us", "pcba功能类别.jack"),
            // dict.logistics.pcba.function.category.14
            ("dict.logistics.pcba.function.category.14", "ja-JP", "jack_jp", "pcba功能类别.jack"),
            // dict.logistics.pcba.function.category.14
            ("dict.logistics.pcba.function.category.14", "zh-CN", "jack", "pcba功能类别.jack"),
            // dict.logistics.pcba.function.category.14
            ("dict.logistics.pcba.function.category.14", "zh-HK", "jack_hk", "pcba功能类别.jack"),

            // dict.logistics.pcba.function.category.15
            ("dict.logistics.pcba.function.category.15", "en-US", "l_us", "pcba功能类别.l"),
            // dict.logistics.pcba.function.category.15
            ("dict.logistics.pcba.function.category.15", "ja-JP", "l_jp", "pcba功能类别.l"),
            // dict.logistics.pcba.function.category.15
            ("dict.logistics.pcba.function.category.15", "zh-CN", "l", "pcba功能类别.l"),
            // dict.logistics.pcba.function.category.15
            ("dict.logistics.pcba.function.category.15", "zh-HK", "l_hk", "pcba功能类别.l"),

            // dict.logistics.pcba.function.category.16
            ("dict.logistics.pcba.function.category.16", "en-US", "lcd_us", "pcba功能类别.lcd"),
            // dict.logistics.pcba.function.category.16
            ("dict.logistics.pcba.function.category.16", "ja-JP", "lcd_jp", "pcba功能类别.lcd"),
            // dict.logistics.pcba.function.category.16
            ("dict.logistics.pcba.function.category.16", "zh-CN", "lcd", "pcba功能类别.lcd"),
            // dict.logistics.pcba.function.category.16
            ("dict.logistics.pcba.function.category.16", "zh-HK", "lcd_hk", "pcba功能类别.lcd"),

            // dict.logistics.pcba.function.category.17
            ("dict.logistics.pcba.function.category.17", "en-US", "main_us", "pcba功能类别.main"),
            // dict.logistics.pcba.function.category.17
            ("dict.logistics.pcba.function.category.17", "ja-JP", "main_jp", "pcba功能类别.main"),
            // dict.logistics.pcba.function.category.17
            ("dict.logistics.pcba.function.category.17", "zh-CN", "main", "pcba功能类别.main"),
            // dict.logistics.pcba.function.category.17
            ("dict.logistics.pcba.function.category.17", "zh-HK", "main_hk", "pcba功能类别.main"),

            // dict.logistics.pcba.function.category.18
            ("dict.logistics.pcba.function.category.18", "en-US", "panel_us", "pcba功能类别.panel"),
            // dict.logistics.pcba.function.category.18
            ("dict.logistics.pcba.function.category.18", "ja-JP", "panel_jp", "pcba功能类别.panel"),
            // dict.logistics.pcba.function.category.18
            ("dict.logistics.pcba.function.category.18", "zh-CN", "panel", "pcba功能类别.panel"),
            // dict.logistics.pcba.function.category.18
            ("dict.logistics.pcba.function.category.18", "zh-HK", "panel_hk", "pcba功能类别.panel"),

            // dict.logistics.pcba.function.category.19
            ("dict.logistics.pcba.function.category.19", "en-US", "power_us", "pcba功能类别.power"),
            // dict.logistics.pcba.function.category.19
            ("dict.logistics.pcba.function.category.19", "ja-JP", "power_jp", "pcba功能类别.power"),
            // dict.logistics.pcba.function.category.19
            ("dict.logistics.pcba.function.category.19", "zh-CN", "power", "pcba功能类别.power"),
            // dict.logistics.pcba.function.category.19
            ("dict.logistics.pcba.function.category.19", "zh-HK", "power_hk", "pcba功能类别.power"),

            // dict.logistics.pcba.function.category.20
            ("dict.logistics.pcba.function.category.20", "en-US", "rear_us", "pcba功能类别.rear"),
            // dict.logistics.pcba.function.category.20
            ("dict.logistics.pcba.function.category.20", "ja-JP", "rear_jp", "pcba功能类别.rear"),
            // dict.logistics.pcba.function.category.20
            ("dict.logistics.pcba.function.category.20", "zh-CN", "rear", "pcba功能类别.rear"),
            // dict.logistics.pcba.function.category.20
            ("dict.logistics.pcba.function.category.20", "zh-HK", "rear_hk", "pcba功能类别.rear"),

            // dict.logistics.pcba.function.category.21
            ("dict.logistics.pcba.function.category.21", "en-US", "rmn-1_us", "pcba功能类别.rmn-1"),
            // dict.logistics.pcba.function.category.21
            ("dict.logistics.pcba.function.category.21", "ja-JP", "rmn-1_jp", "pcba功能类别.rmn-1"),
            // dict.logistics.pcba.function.category.21
            ("dict.logistics.pcba.function.category.21", "zh-CN", "rmn-1", "pcba功能类别.rmn-1"),
            // dict.logistics.pcba.function.category.21
            ("dict.logistics.pcba.function.category.21", "zh-HK", "rmn-1_hk", "pcba功能类别.rmn-1"),

            // dict.logistics.pcba.function.category.22
            ("dict.logistics.pcba.function.category.22", "en-US", "sata_us", "pcba功能类别.sata"),
            // dict.logistics.pcba.function.category.22
            ("dict.logistics.pcba.function.category.22", "ja-JP", "sata_jp", "pcba功能类别.sata"),
            // dict.logistics.pcba.function.category.22
            ("dict.logistics.pcba.function.category.22", "zh-CN", "sata", "pcba功能类别.sata"),
            // dict.logistics.pcba.function.category.22
            ("dict.logistics.pcba.function.category.22", "zh-HK", "sata_hk", "pcba功能类别.sata"),

            // dict.logistics.pcba.function.category.23
            ("dict.logistics.pcba.function.category.23", "en-US", "seq_us", "pcba功能类别.seq"),
            // dict.logistics.pcba.function.category.23
            ("dict.logistics.pcba.function.category.23", "ja-JP", "seq_jp", "pcba功能类别.seq"),
            // dict.logistics.pcba.function.category.23
            ("dict.logistics.pcba.function.category.23", "zh-CN", "seq", "pcba功能类别.seq"),
            // dict.logistics.pcba.function.category.23
            ("dict.logistics.pcba.function.category.23", "zh-HK", "seq_hk", "pcba功能类别.seq"),

            // dict.logistics.pcba.function.category.24
            ("dict.logistics.pcba.function.category.24", "en-US", "sys_us", "pcba功能类别.sys"),
            // dict.logistics.pcba.function.category.24
            ("dict.logistics.pcba.function.category.24", "ja-JP", "sys_jp", "pcba功能类别.sys"),
            // dict.logistics.pcba.function.category.24
            ("dict.logistics.pcba.function.category.24", "zh-CN", "sys", "pcba功能类别.sys"),
            // dict.logistics.pcba.function.category.24
            ("dict.logistics.pcba.function.category.24", "zh-HK", "sys_hk", "pcba功能类别.sys"),

            // dict.logistics.pcba.function.category.25
            ("dict.logistics.pcba.function.category.25", "en-US", "top_us", "pcba功能类别.top"),
            // dict.logistics.pcba.function.category.25
            ("dict.logistics.pcba.function.category.25", "ja-JP", "top_jp", "pcba功能类别.top"),
            // dict.logistics.pcba.function.category.25
            ("dict.logistics.pcba.function.category.25", "zh-CN", "top", "pcba功能类别.top"),
            // dict.logistics.pcba.function.category.25
            ("dict.logistics.pcba.function.category.25", "zh-HK", "top_hk", "pcba功能类别.top"),

            // dict.logistics.pcba.function.category.26
            ("dict.logistics.pcba.function.category.26", "en-US", "usb_us", "pcba功能类别.usb"),
            // dict.logistics.pcba.function.category.26
            ("dict.logistics.pcba.function.category.26", "ja-JP", "usb_jp", "pcba功能类别.usb"),
            // dict.logistics.pcba.function.category.26
            ("dict.logistics.pcba.function.category.26", "zh-CN", "usb", "pcba功能类别.usb"),
            // dict.logistics.pcba.function.category.26
            ("dict.logistics.pcba.function.category.26", "zh-HK", "usb_hk", "pcba功能类别.usb"),

            // dict.logistics.pcba.panel.category.1
            ("dict.logistics.pcba.panel.category.1", "en-US", "a2io_us", "pcba板位类别.a2io"),
            // dict.logistics.pcba.panel.category.1
            ("dict.logistics.pcba.panel.category.1", "ja-JP", "a2io_jp", "pcba板位类别.a2io"),
            // dict.logistics.pcba.panel.category.1
            ("dict.logistics.pcba.panel.category.1", "zh-CN", "a2io", "pcba板位类别.a2io"),
            // dict.logistics.pcba.panel.category.1
            ("dict.logistics.pcba.panel.category.1", "zh-HK", "a2io_hk", "pcba板位类别.a2io"),

            // dict.logistics.pcba.panel.category.2
            ("dict.logistics.pcba.panel.category.2", "en-US", "a2io b_us", "pcba板位类别.a2io b"),
            // dict.logistics.pcba.panel.category.2
            ("dict.logistics.pcba.panel.category.2", "ja-JP", "a2io b_jp", "pcba板位类别.a2io b"),
            // dict.logistics.pcba.panel.category.2
            ("dict.logistics.pcba.panel.category.2", "zh-CN", "a2io b", "pcba板位类别.a2io b"),
            // dict.logistics.pcba.panel.category.2
            ("dict.logistics.pcba.panel.category.2", "zh-HK", "a2io b_hk", "pcba板位类别.a2io b"),

            // dict.logistics.pcba.panel.category.3
            ("dict.logistics.pcba.panel.category.3", "en-US", "a2io t_us", "pcba板位类别.a2io t"),
            // dict.logistics.pcba.panel.category.3
            ("dict.logistics.pcba.panel.category.3", "ja-JP", "a2io t_jp", "pcba板位类别.a2io t"),
            // dict.logistics.pcba.panel.category.3
            ("dict.logistics.pcba.panel.category.3", "zh-CN", "a2io t", "pcba板位类别.a2io t"),
            // dict.logistics.pcba.panel.category.3
            ("dict.logistics.pcba.panel.category.3", "zh-HK", "a2io t_hk", "pcba板位类别.a2io t"),

            // dict.logistics.pcba.panel.category.4
            ("dict.logistics.pcba.panel.category.4", "en-US", "a4in b_us", "pcba板位类别.a4in b"),
            // dict.logistics.pcba.panel.category.4
            ("dict.logistics.pcba.panel.category.4", "ja-JP", "a4in b_jp", "pcba板位类别.a4in b"),
            // dict.logistics.pcba.panel.category.4
            ("dict.logistics.pcba.panel.category.4", "zh-CN", "a4in b", "pcba板位类别.a4in b"),
            // dict.logistics.pcba.panel.category.4
            ("dict.logistics.pcba.panel.category.4", "zh-HK", "a4in b_hk", "pcba板位类别.a4in b"),

            // dict.logistics.pcba.panel.category.5
            ("dict.logistics.pcba.panel.category.5", "en-US", "a4in t_us", "pcba板位类别.a4in t"),
            // dict.logistics.pcba.panel.category.5
            ("dict.logistics.pcba.panel.category.5", "ja-JP", "a4in t_jp", "pcba板位类别.a4in t"),
            // dict.logistics.pcba.panel.category.5
            ("dict.logistics.pcba.panel.category.5", "zh-CN", "a4in t", "pcba板位类别.a4in t"),
            // dict.logistics.pcba.panel.category.5
            ("dict.logistics.pcba.panel.category.5", "zh-HK", "a4in t_hk", "pcba板位类别.a4in t"),

            // dict.logistics.pcba.panel.category.6
            ("dict.logistics.pcba.panel.category.6", "en-US", "a4out b_us", "pcba板位类别.a4out b"),
            // dict.logistics.pcba.panel.category.6
            ("dict.logistics.pcba.panel.category.6", "ja-JP", "a4out b_jp", "pcba板位类别.a4out b"),
            // dict.logistics.pcba.panel.category.6
            ("dict.logistics.pcba.panel.category.6", "zh-CN", "a4out b", "pcba板位类别.a4out b"),
            // dict.logistics.pcba.panel.category.6
            ("dict.logistics.pcba.panel.category.6", "zh-HK", "a4out b_hk", "pcba板位类别.a4out b"),

            // dict.logistics.pcba.panel.category.7
            ("dict.logistics.pcba.panel.category.7", "en-US", "a4out t_us", "pcba板位类别.a4out t"),
            // dict.logistics.pcba.panel.category.7
            ("dict.logistics.pcba.panel.category.7", "ja-JP", "a4out t_jp", "pcba板位类别.a4out t"),
            // dict.logistics.pcba.panel.category.7
            ("dict.logistics.pcba.panel.category.7", "zh-CN", "a4out t", "pcba板位类别.a4out t"),
            // dict.logistics.pcba.panel.category.7
            ("dict.logistics.pcba.panel.category.7", "zh-HK", "a4out t_hk", "pcba板位类别.a4out t"),

            // dict.logistics.pcba.panel.category.8
            ("dict.logistics.pcba.panel.category.8", "en-US", "ad04 t_us", "pcba板位类别.ad04 t"),
            // dict.logistics.pcba.panel.category.8
            ("dict.logistics.pcba.panel.category.8", "ja-JP", "ad04 t_jp", "pcba板位类别.ad04 t"),
            // dict.logistics.pcba.panel.category.8
            ("dict.logistics.pcba.panel.category.8", "zh-CN", "ad04 t", "pcba板位类别.ad04 t"),
            // dict.logistics.pcba.panel.category.8
            ("dict.logistics.pcba.panel.category.8", "zh-HK", "ad04 t_hk", "pcba板位类别.ad04 t"),

            // dict.logistics.pcba.panel.category.9
            ("dict.logistics.pcba.panel.category.9", "en-US", "adda b_us", "pcba板位类别.adda b"),
            // dict.logistics.pcba.panel.category.9
            ("dict.logistics.pcba.panel.category.9", "ja-JP", "adda b_jp", "pcba板位类别.adda b"),
            // dict.logistics.pcba.panel.category.9
            ("dict.logistics.pcba.panel.category.9", "zh-CN", "adda b", "pcba板位类别.adda b"),
            // dict.logistics.pcba.panel.category.9
            ("dict.logistics.pcba.panel.category.9", "zh-HK", "adda b_hk", "pcba板位类别.adda b"),

            // dict.logistics.pcba.panel.category.10
            ("dict.logistics.pcba.panel.category.10", "en-US", "adda b/t_us", "pcba板位类别.adda b/t"),
            // dict.logistics.pcba.panel.category.10
            ("dict.logistics.pcba.panel.category.10", "ja-JP", "adda b/t_jp", "pcba板位类别.adda b/t"),
            // dict.logistics.pcba.panel.category.10
            ("dict.logistics.pcba.panel.category.10", "zh-CN", "adda b/t", "pcba板位类别.adda b/t"),
            // dict.logistics.pcba.panel.category.10
            ("dict.logistics.pcba.panel.category.10", "zh-HK", "adda b/t_hk", "pcba板位类别.adda b/t"),

            // dict.logistics.pcba.panel.category.11
            ("dict.logistics.pcba.panel.category.11", "en-US", "adda t_us", "pcba板位类别.adda t"),
            // dict.logistics.pcba.panel.category.11
            ("dict.logistics.pcba.panel.category.11", "ja-JP", "adda t_jp", "pcba板位类别.adda t"),
            // dict.logistics.pcba.panel.category.11
            ("dict.logistics.pcba.panel.category.11", "zh-CN", "adda t", "pcba板位类别.adda t"),
            // dict.logistics.pcba.panel.category.11
            ("dict.logistics.pcba.panel.category.11", "zh-HK", "adda t_hk", "pcba板位类别.adda t"),

            // dict.logistics.pcba.panel.category.12
            ("dict.logistics.pcba.panel.category.12", "en-US", "adoc_us", "pcba板位类别.adoc"),
            // dict.logistics.pcba.panel.category.12
            ("dict.logistics.pcba.panel.category.12", "ja-JP", "adoc_jp", "pcba板位类别.adoc"),
            // dict.logistics.pcba.panel.category.12
            ("dict.logistics.pcba.panel.category.12", "zh-CN", "adoc", "pcba板位类别.adoc"),
            // dict.logistics.pcba.panel.category.12
            ("dict.logistics.pcba.panel.category.12", "zh-HK", "adoc_hk", "pcba板位类别.adoc"),

            // dict.logistics.pcba.panel.category.13
            ("dict.logistics.pcba.panel.category.13", "en-US", "adoc b_us", "pcba板位类别.adoc b"),
            // dict.logistics.pcba.panel.category.13
            ("dict.logistics.pcba.panel.category.13", "ja-JP", "adoc b_jp", "pcba板位类别.adoc b"),
            // dict.logistics.pcba.panel.category.13
            ("dict.logistics.pcba.panel.category.13", "zh-CN", "adoc b", "pcba板位类别.adoc b"),
            // dict.logistics.pcba.panel.category.13
            ("dict.logistics.pcba.panel.category.13", "zh-HK", "adoc b_hk", "pcba板位类别.adoc b"),

            // dict.logistics.pcba.panel.category.14
            ("dict.logistics.pcba.panel.category.14", "en-US", "adoc b/t_us", "pcba板位类别.adoc b/t"),
            // dict.logistics.pcba.panel.category.14
            ("dict.logistics.pcba.panel.category.14", "ja-JP", "adoc b/t_jp", "pcba板位类别.adoc b/t"),
            // dict.logistics.pcba.panel.category.14
            ("dict.logistics.pcba.panel.category.14", "zh-CN", "adoc b/t", "pcba板位类别.adoc b/t"),
            // dict.logistics.pcba.panel.category.14
            ("dict.logistics.pcba.panel.category.14", "zh-HK", "adoc b/t_hk", "pcba板位类别.adoc b/t"),

            // dict.logistics.pcba.panel.category.15
            ("dict.logistics.pcba.panel.category.15", "en-US", "adoc t_us", "pcba板位类别.adoc t"),
            // dict.logistics.pcba.panel.category.15
            ("dict.logistics.pcba.panel.category.15", "ja-JP", "adoc t_jp", "pcba板位类别.adoc t"),
            // dict.logistics.pcba.panel.category.15
            ("dict.logistics.pcba.panel.category.15", "zh-CN", "adoc t", "pcba板位类别.adoc t"),
            // dict.logistics.pcba.panel.category.15
            ("dict.logistics.pcba.panel.category.15", "zh-HK", "adoc t_hk", "pcba板位类别.adoc t"),

            // dict.logistics.pcba.panel.category.16
            ("dict.logistics.pcba.panel.category.16", "en-US", "aes4 b_us", "pcba板位类别.aes4 b"),
            // dict.logistics.pcba.panel.category.16
            ("dict.logistics.pcba.panel.category.16", "ja-JP", "aes4 b_jp", "pcba板位类别.aes4 b"),
            // dict.logistics.pcba.panel.category.16
            ("dict.logistics.pcba.panel.category.16", "zh-CN", "aes4 b", "pcba板位类别.aes4 b"),
            // dict.logistics.pcba.panel.category.16
            ("dict.logistics.pcba.panel.category.16", "zh-HK", "aes4 b_hk", "pcba板位类别.aes4 b"),

            // dict.logistics.pcba.panel.category.17
            ("dict.logistics.pcba.panel.category.17", "en-US", "aes4 b/t_us", "pcba板位类别.aes4 b/t"),
            // dict.logistics.pcba.panel.category.17
            ("dict.logistics.pcba.panel.category.17", "ja-JP", "aes4 b/t_jp", "pcba板位类别.aes4 b/t"),
            // dict.logistics.pcba.panel.category.17
            ("dict.logistics.pcba.panel.category.17", "zh-CN", "aes4 b/t", "pcba板位类别.aes4 b/t"),
            // dict.logistics.pcba.panel.category.17
            ("dict.logistics.pcba.panel.category.17", "zh-HK", "aes4 b/t_hk", "pcba板位类别.aes4 b/t"),

            // dict.logistics.pcba.panel.category.18
            ("dict.logistics.pcba.panel.category.18", "en-US", "aes4 t_us", "pcba板位类别.aes4 t"),
            // dict.logistics.pcba.panel.category.18
            ("dict.logistics.pcba.panel.category.18", "ja-JP", "aes4 t_jp", "pcba板位类别.aes4 t"),
            // dict.logistics.pcba.panel.category.18
            ("dict.logistics.pcba.panel.category.18", "zh-CN", "aes4 t", "pcba板位类别.aes4 t"),
            // dict.logistics.pcba.panel.category.18
            ("dict.logistics.pcba.panel.category.18", "zh-HK", "aes4 t_hk", "pcba板位类别.aes4 t"),

            // dict.logistics.pcba.panel.category.19
            ("dict.logistics.pcba.panel.category.19", "en-US", "ana_us", "pcba板位类别.ana"),
            // dict.logistics.pcba.panel.category.19
            ("dict.logistics.pcba.panel.category.19", "ja-JP", "ana_jp", "pcba板位类别.ana"),
            // dict.logistics.pcba.panel.category.19
            ("dict.logistics.pcba.panel.category.19", "zh-CN", "ana", "pcba板位类别.ana"),
            // dict.logistics.pcba.panel.category.19
            ("dict.logistics.pcba.panel.category.19", "zh-HK", "ana_hk", "pcba板位类别.ana"),

            // dict.logistics.pcba.panel.category.24
            ("dict.logistics.pcba.panel.category.24", "en-US", "ana a_us", "pcba板位类别.ana a"),
            // dict.logistics.pcba.panel.category.24
            ("dict.logistics.pcba.panel.category.24", "ja-JP", "ana a_jp", "pcba板位类别.ana a"),
            // dict.logistics.pcba.panel.category.24
            ("dict.logistics.pcba.panel.category.24", "zh-CN", "ana a", "pcba板位类别.ana a"),
            // dict.logistics.pcba.panel.category.24
            ("dict.logistics.pcba.panel.category.24", "zh-HK", "ana a_hk", "pcba板位类别.ana a"),

            // dict.logistics.pcba.panel.category.25
            ("dict.logistics.pcba.panel.category.25", "en-US", "ana b_us", "pcba板位类别.ana b"),
            // dict.logistics.pcba.panel.category.25
            ("dict.logistics.pcba.panel.category.25", "ja-JP", "ana b_jp", "pcba板位类别.ana b"),
            // dict.logistics.pcba.panel.category.25
            ("dict.logistics.pcba.panel.category.25", "zh-CN", "ana b", "pcba板位类别.ana b"),
            // dict.logistics.pcba.panel.category.25
            ("dict.logistics.pcba.panel.category.25", "zh-HK", "ana b_hk", "pcba板位类别.ana b"),

            // dict.logistics.pcba.panel.category.26
            ("dict.logistics.pcba.panel.category.26", "en-US", "ana b/t_us", "pcba板位类别.ana b/t"),
            // dict.logistics.pcba.panel.category.26
            ("dict.logistics.pcba.panel.category.26", "ja-JP", "ana b/t_jp", "pcba板位类别.ana b/t"),
            // dict.logistics.pcba.panel.category.26
            ("dict.logistics.pcba.panel.category.26", "zh-CN", "ana b/t", "pcba板位类别.ana b/t"),
            // dict.logistics.pcba.panel.category.26
            ("dict.logistics.pcba.panel.category.26", "zh-HK", "ana b/t_hk", "pcba板位类别.ana b/t"),

            // dict.logistics.pcba.panel.category.27
            ("dict.logistics.pcba.panel.category.27", "en-US", "ana t_us", "pcba板位类别.ana t"),
            // dict.logistics.pcba.panel.category.27
            ("dict.logistics.pcba.panel.category.27", "ja-JP", "ana t_jp", "pcba板位类别.ana t"),
            // dict.logistics.pcba.panel.category.27
            ("dict.logistics.pcba.panel.category.27", "zh-CN", "ana t", "pcba板位类别.ana t"),
            // dict.logistics.pcba.panel.category.27
            ("dict.logistics.pcba.panel.category.27", "zh-HK", "ana t_hk", "pcba板位类别.ana t"),

            // dict.logistics.pcba.panel.category.28
            ("dict.logistics.pcba.panel.category.28", "en-US", "apnel t_us", "pcba板位类别.apnel t"),
            // dict.logistics.pcba.panel.category.28
            ("dict.logistics.pcba.panel.category.28", "ja-JP", "apnel t_jp", "pcba板位类别.apnel t"),
            // dict.logistics.pcba.panel.category.28
            ("dict.logistics.pcba.panel.category.28", "zh-CN", "apnel t", "pcba板位类别.apnel t"),
            // dict.logistics.pcba.panel.category.28
            ("dict.logistics.pcba.panel.category.28", "zh-HK", "apnel t_hk", "pcba板位类别.apnel t"),

            // dict.logistics.pcba.panel.category.29
            ("dict.logistics.pcba.panel.category.29", "en-US", "audio_us", "pcba板位类别.audio"),
            // dict.logistics.pcba.panel.category.29
            ("dict.logistics.pcba.panel.category.29", "ja-JP", "audio_jp", "pcba板位类别.audio"),
            // dict.logistics.pcba.panel.category.29
            ("dict.logistics.pcba.panel.category.29", "zh-CN", "audio", "pcba板位类别.audio"),
            // dict.logistics.pcba.panel.category.29
            ("dict.logistics.pcba.panel.category.29", "zh-HK", "audio_hk", "pcba板位类别.audio"),

            // dict.logistics.pcba.panel.category.30
            ("dict.logistics.pcba.panel.category.30", "en-US", "audio a_us", "pcba板位类别.audio a"),
            // dict.logistics.pcba.panel.category.30
            ("dict.logistics.pcba.panel.category.30", "ja-JP", "audio a_jp", "pcba板位类别.audio a"),
            // dict.logistics.pcba.panel.category.30
            ("dict.logistics.pcba.panel.category.30", "zh-CN", "audio a", "pcba板位类别.audio a"),
            // dict.logistics.pcba.panel.category.30
            ("dict.logistics.pcba.panel.category.30", "zh-HK", "audio a_hk", "pcba板位类别.audio a"),

            // dict.logistics.pcba.panel.category.31
            ("dict.logistics.pcba.panel.category.31", "en-US", "audio alt b_us", "pcba板位类别.audio alt b"),
            // dict.logistics.pcba.panel.category.31
            ("dict.logistics.pcba.panel.category.31", "ja-JP", "audio alt b_jp", "pcba板位类别.audio alt b"),
            // dict.logistics.pcba.panel.category.31
            ("dict.logistics.pcba.panel.category.31", "zh-CN", "audio alt b", "pcba板位类别.audio alt b"),
            // dict.logistics.pcba.panel.category.31
            ("dict.logistics.pcba.panel.category.31", "zh-HK", "audio alt b_hk", "pcba板位类别.audio alt b"),

            // dict.logistics.pcba.panel.category.32
            ("dict.logistics.pcba.panel.category.32", "en-US", "audio alt t_us", "pcba板位类别.audio alt t"),
            // dict.logistics.pcba.panel.category.32
            ("dict.logistics.pcba.panel.category.32", "ja-JP", "audio alt t_jp", "pcba板位类别.audio alt t"),
            // dict.logistics.pcba.panel.category.32
            ("dict.logistics.pcba.panel.category.32", "zh-CN", "audio alt t", "pcba板位类别.audio alt t"),
            // dict.logistics.pcba.panel.category.32
            ("dict.logistics.pcba.panel.category.32", "zh-HK", "audio alt t_hk", "pcba板位类别.audio alt t"),

            // dict.logistics.pcba.panel.category.33
            ("dict.logistics.pcba.panel.category.33", "en-US", "audio b_us", "pcba板位类别.audio b"),
            // dict.logistics.pcba.panel.category.33
            ("dict.logistics.pcba.panel.category.33", "ja-JP", "audio b_jp", "pcba板位类别.audio b"),
            // dict.logistics.pcba.panel.category.33
            ("dict.logistics.pcba.panel.category.33", "zh-CN", "audio b", "pcba板位类别.audio b"),
            // dict.logistics.pcba.panel.category.33
            ("dict.logistics.pcba.panel.category.33", "zh-HK", "audio b_hk", "pcba板位类别.audio b"),

            // dict.logistics.pcba.panel.category.34
            ("dict.logistics.pcba.panel.category.34", "en-US", "audio b/t_us", "pcba板位类别.audio b/t"),
            // dict.logistics.pcba.panel.category.34
            ("dict.logistics.pcba.panel.category.34", "ja-JP", "audio b/t_jp", "pcba板位类别.audio b/t"),
            // dict.logistics.pcba.panel.category.34
            ("dict.logistics.pcba.panel.category.34", "zh-CN", "audio b/t", "pcba板位类别.audio b/t"),
            // dict.logistics.pcba.panel.category.34
            ("dict.logistics.pcba.panel.category.34", "zh-HK", "audio b/t_hk", "pcba板位类别.audio b/t"),

            // dict.logistics.pcba.panel.category.35
            ("dict.logistics.pcba.panel.category.35", "en-US", "audio t_us", "pcba板位类别.audio t"),
            // dict.logistics.pcba.panel.category.35
            ("dict.logistics.pcba.panel.category.35", "ja-JP", "audio t_jp", "pcba板位类别.audio t"),
            // dict.logistics.pcba.panel.category.35
            ("dict.logistics.pcba.panel.category.35", "zh-CN", "audio t", "pcba板位类别.audio t"),
            // dict.logistics.pcba.panel.category.35
            ("dict.logistics.pcba.panel.category.35", "zh-HK", "audio t_hk", "pcba板位类别.audio t"),

            // dict.logistics.pcba.panel.category.36
            ("dict.logistics.pcba.panel.category.36", "en-US", "audio-00-b_us", "pcba板位类别.audio-00-b"),
            // dict.logistics.pcba.panel.category.36
            ("dict.logistics.pcba.panel.category.36", "ja-JP", "audio-00-b_jp", "pcba板位类别.audio-00-b"),
            // dict.logistics.pcba.panel.category.36
            ("dict.logistics.pcba.panel.category.36", "zh-CN", "audio-00-b", "pcba板位类别.audio-00-b"),
            // dict.logistics.pcba.panel.category.36
            ("dict.logistics.pcba.panel.category.36", "zh-HK", "audio-00-b_hk", "pcba板位类别.audio-00-b"),

            // dict.logistics.pcba.panel.category.37
            ("dict.logistics.pcba.panel.category.37", "en-US", "audio-00-t_us", "pcba板位类别.audio-00-t"),
            // dict.logistics.pcba.panel.category.37
            ("dict.logistics.pcba.panel.category.37", "ja-JP", "audio-00-t_jp", "pcba板位类别.audio-00-t"),
            // dict.logistics.pcba.panel.category.37
            ("dict.logistics.pcba.panel.category.37", "zh-CN", "audio-00-t", "pcba板位类别.audio-00-t"),
            // dict.logistics.pcba.panel.category.37
            ("dict.logistics.pcba.panel.category.37", "zh-HK", "audio-00-t_hk", "pcba板位类别.audio-00-t"),

            // dict.logistics.pcba.panel.category.38
            ("dict.logistics.pcba.panel.category.38", "en-US", "audio-10-b_us", "pcba板位类别.audio-10-b"),
            // dict.logistics.pcba.panel.category.38
            ("dict.logistics.pcba.panel.category.38", "ja-JP", "audio-10-b_jp", "pcba板位类别.audio-10-b"),
            // dict.logistics.pcba.panel.category.38
            ("dict.logistics.pcba.panel.category.38", "zh-CN", "audio-10-b", "pcba板位类别.audio-10-b"),
            // dict.logistics.pcba.panel.category.38
            ("dict.logistics.pcba.panel.category.38", "zh-HK", "audio-10-b_hk", "pcba板位类别.audio-10-b"),

            // dict.logistics.pcba.panel.category.39
            ("dict.logistics.pcba.panel.category.39", "en-US", "audio-10-t_us", "pcba板位类别.audio-10-t"),
            // dict.logistics.pcba.panel.category.39
            ("dict.logistics.pcba.panel.category.39", "ja-JP", "audio-10-t_jp", "pcba板位类别.audio-10-t"),
            // dict.logistics.pcba.panel.category.39
            ("dict.logistics.pcba.panel.category.39", "zh-CN", "audio-10-t", "pcba板位类别.audio-10-t"),
            // dict.logistics.pcba.panel.category.39
            ("dict.logistics.pcba.panel.category.39", "zh-HK", "audio-10-t_hk", "pcba板位类别.audio-10-t"),

            // dict.logistics.pcba.panel.category.40
            ("dict.logistics.pcba.panel.category.40", "en-US", "audio-20-b_us", "pcba板位类别.audio-20-b"),
            // dict.logistics.pcba.panel.category.40
            ("dict.logistics.pcba.panel.category.40", "ja-JP", "audio-20-b_jp", "pcba板位类别.audio-20-b"),
            // dict.logistics.pcba.panel.category.40
            ("dict.logistics.pcba.panel.category.40", "zh-CN", "audio-20-b", "pcba板位类别.audio-20-b"),
            // dict.logistics.pcba.panel.category.40
            ("dict.logistics.pcba.panel.category.40", "zh-HK", "audio-20-b_hk", "pcba板位类别.audio-20-b"),

            // dict.logistics.pcba.panel.category.41
            ("dict.logistics.pcba.panel.category.41", "en-US", "audio-20-t_us", "pcba板位类别.audio-20-t"),
            // dict.logistics.pcba.panel.category.41
            ("dict.logistics.pcba.panel.category.41", "ja-JP", "audio-20-t_jp", "pcba板位类别.audio-20-t"),
            // dict.logistics.pcba.panel.category.41
            ("dict.logistics.pcba.panel.category.41", "zh-CN", "audio-20-t", "pcba板位类别.audio-20-t"),
            // dict.logistics.pcba.panel.category.41
            ("dict.logistics.pcba.panel.category.41", "zh-HK", "audio-20-t_hk", "pcba板位类别.audio-20-t"),

            // dict.logistics.pcba.panel.category.42
            ("dict.logistics.pcba.panel.category.42", "en-US", "bottom b_us", "pcba板位类别.bottom b"),
            // dict.logistics.pcba.panel.category.42
            ("dict.logistics.pcba.panel.category.42", "ja-JP", "bottom b_jp", "pcba板位类别.bottom b"),
            // dict.logistics.pcba.panel.category.42
            ("dict.logistics.pcba.panel.category.42", "zh-CN", "bottom b", "pcba板位类别.bottom b"),
            // dict.logistics.pcba.panel.category.42
            ("dict.logistics.pcba.panel.category.42", "zh-HK", "bottom b_hk", "pcba板位类别.bottom b"),

            // dict.logistics.pcba.panel.category.43
            ("dict.logistics.pcba.panel.category.43", "en-US", "ccl b_us", "pcba板位类别.ccl b"),
            // dict.logistics.pcba.panel.category.43
            ("dict.logistics.pcba.panel.category.43", "ja-JP", "ccl b_jp", "pcba板位类别.ccl b"),
            // dict.logistics.pcba.panel.category.43
            ("dict.logistics.pcba.panel.category.43", "zh-CN", "ccl b", "pcba板位类别.ccl b"),
            // dict.logistics.pcba.panel.category.43
            ("dict.logistics.pcba.panel.category.43", "zh-HK", "ccl b_hk", "pcba板位类别.ccl b"),

            // dict.logistics.pcba.panel.category.44
            ("dict.logistics.pcba.panel.category.44", "en-US", "ccl b/t_us", "pcba板位类别.ccl b/t"),
            // dict.logistics.pcba.panel.category.44
            ("dict.logistics.pcba.panel.category.44", "ja-JP", "ccl b/t_jp", "pcba板位类别.ccl b/t"),
            // dict.logistics.pcba.panel.category.44
            ("dict.logistics.pcba.panel.category.44", "zh-CN", "ccl b/t", "pcba板位类别.ccl b/t"),
            // dict.logistics.pcba.panel.category.44
            ("dict.logistics.pcba.panel.category.44", "zh-HK", "ccl b/t_hk", "pcba板位类别.ccl b/t"),

            // dict.logistics.pcba.panel.category.45
            ("dict.logistics.pcba.panel.category.45", "en-US", "ccl t_us", "pcba板位类别.ccl t"),
            // dict.logistics.pcba.panel.category.45
            ("dict.logistics.pcba.panel.category.45", "ja-JP", "ccl t_jp", "pcba板位类别.ccl t"),
            // dict.logistics.pcba.panel.category.45
            ("dict.logistics.pcba.panel.category.45", "zh-CN", "ccl t", "pcba板位类别.ccl t"),
            // dict.logistics.pcba.panel.category.45
            ("dict.logistics.pcba.panel.category.45", "zh-HK", "ccl t_hk", "pcba板位类别.ccl t"),

            // dict.logistics.pcba.panel.category.46
            ("dict.logistics.pcba.panel.category.46", "en-US", "cd b_us", "pcba板位类别.cd b"),
            // dict.logistics.pcba.panel.category.46
            ("dict.logistics.pcba.panel.category.46", "ja-JP", "cd b_jp", "pcba板位类别.cd b"),
            // dict.logistics.pcba.panel.category.46
            ("dict.logistics.pcba.panel.category.46", "zh-CN", "cd b", "pcba板位类别.cd b"),
            // dict.logistics.pcba.panel.category.46
            ("dict.logistics.pcba.panel.category.46", "zh-HK", "cd b_hk", "pcba板位类别.cd b"),

            // dict.logistics.pcba.panel.category.47
            ("dict.logistics.pcba.panel.category.47", "en-US", "cd t_us", "pcba板位类别.cd t"),
            // dict.logistics.pcba.panel.category.47
            ("dict.logistics.pcba.panel.category.47", "ja-JP", "cd t_jp", "pcba板位类别.cd t"),
            // dict.logistics.pcba.panel.category.47
            ("dict.logistics.pcba.panel.category.47", "zh-CN", "cd t", "pcba板位类别.cd t"),
            // dict.logistics.pcba.panel.category.47
            ("dict.logistics.pcba.panel.category.47", "zh-HK", "cd t_hk", "pcba板位类别.cd t"),

            // dict.logistics.pcba.panel.category.48
            ("dict.logistics.pcba.panel.category.48", "en-US", "cd-main_us", "pcba板位类别.cd-main"),
            // dict.logistics.pcba.panel.category.48
            ("dict.logistics.pcba.panel.category.48", "ja-JP", "cd-main_jp", "pcba板位类别.cd-main"),
            // dict.logistics.pcba.panel.category.48
            ("dict.logistics.pcba.panel.category.48", "zh-CN", "cd-main", "pcba板位类别.cd-main"),
            // dict.logistics.pcba.panel.category.48
            ("dict.logistics.pcba.panel.category.48", "zh-HK", "cd-main_hk", "pcba板位类别.cd-main"),

            // dict.logistics.pcba.panel.category.49
            ("dict.logistics.pcba.panel.category.49", "en-US", "cd-main b_us", "pcba板位类别.cd-main b"),
            // dict.logistics.pcba.panel.category.49
            ("dict.logistics.pcba.panel.category.49", "ja-JP", "cd-main b_jp", "pcba板位类别.cd-main b"),
            // dict.logistics.pcba.panel.category.49
            ("dict.logistics.pcba.panel.category.49", "zh-CN", "cd-main b", "pcba板位类别.cd-main b"),
            // dict.logistics.pcba.panel.category.49
            ("dict.logistics.pcba.panel.category.49", "zh-HK", "cd-main b_hk", "pcba板位类别.cd-main b"),

            // dict.logistics.pcba.panel.category.50
            ("dict.logistics.pcba.panel.category.50", "en-US", "cdmcu_us", "pcba板位类别.cdmcu"),
            // dict.logistics.pcba.panel.category.50
            ("dict.logistics.pcba.panel.category.50", "ja-JP", "cdmcu_jp", "pcba板位类别.cdmcu"),
            // dict.logistics.pcba.panel.category.50
            ("dict.logistics.pcba.panel.category.50", "zh-CN", "cdmcu", "pcba板位类别.cdmcu"),
            // dict.logistics.pcba.panel.category.50
            ("dict.logistics.pcba.panel.category.50", "zh-HK", "cdmcu_hk", "pcba板位类别.cdmcu"),

            // dict.logistics.pcba.panel.category.51
            ("dict.logistics.pcba.panel.category.51", "en-US", "cdmcu b_us", "pcba板位类别.cdmcu b"),
            // dict.logistics.pcba.panel.category.51
            ("dict.logistics.pcba.panel.category.51", "ja-JP", "cdmcu b_jp", "pcba板位类别.cdmcu b"),
            // dict.logistics.pcba.panel.category.51
            ("dict.logistics.pcba.panel.category.51", "zh-CN", "cdmcu b", "pcba板位类别.cdmcu b"),
            // dict.logistics.pcba.panel.category.51
            ("dict.logistics.pcba.panel.category.51", "zh-HK", "cdmcu b_hk", "pcba板位类别.cdmcu b"),

            // dict.logistics.pcba.panel.category.52
            ("dict.logistics.pcba.panel.category.52", "en-US", "cdmcu b/t_us", "pcba板位类别.cdmcu b/t"),
            // dict.logistics.pcba.panel.category.52
            ("dict.logistics.pcba.panel.category.52", "ja-JP", "cdmcu b/t_jp", "pcba板位类别.cdmcu b/t"),
            // dict.logistics.pcba.panel.category.52
            ("dict.logistics.pcba.panel.category.52", "zh-CN", "cdmcu b/t", "pcba板位类别.cdmcu b/t"),
            // dict.logistics.pcba.panel.category.52
            ("dict.logistics.pcba.panel.category.52", "zh-HK", "cdmcu b/t_hk", "pcba板位类别.cdmcu b/t"),

            // dict.logistics.pcba.panel.category.53
            ("dict.logistics.pcba.panel.category.53", "en-US", "cdmcu t_us", "pcba板位类别.cdmcu t"),
            // dict.logistics.pcba.panel.category.53
            ("dict.logistics.pcba.panel.category.53", "ja-JP", "cdmcu t_jp", "pcba板位类别.cdmcu t"),
            // dict.logistics.pcba.panel.category.53
            ("dict.logistics.pcba.panel.category.53", "zh-CN", "cdmcu t", "pcba板位类别.cdmcu t"),
            // dict.logistics.pcba.panel.category.53
            ("dict.logistics.pcba.panel.category.53", "zh-HK", "cdmcu t_hk", "pcba板位类别.cdmcu t"),

            // dict.logistics.pcba.panel.category.54
            ("dict.logistics.pcba.panel.category.54", "en-US", "comb b_us", "pcba板位类别.comb b"),
            // dict.logistics.pcba.panel.category.54
            ("dict.logistics.pcba.panel.category.54", "ja-JP", "comb b_jp", "pcba板位类别.comb b"),
            // dict.logistics.pcba.panel.category.54
            ("dict.logistics.pcba.panel.category.54", "zh-CN", "comb b", "pcba板位类别.comb b"),
            // dict.logistics.pcba.panel.category.54
            ("dict.logistics.pcba.panel.category.54", "zh-HK", "comb b_hk", "pcba板位类别.comb b"),

            // dict.logistics.pcba.panel.category.55
            ("dict.logistics.pcba.panel.category.55", "en-US", "comb t_us", "pcba板位类别.comb t"),
            // dict.logistics.pcba.panel.category.55
            ("dict.logistics.pcba.panel.category.55", "ja-JP", "comb t_jp", "pcba板位类别.comb t"),
            // dict.logistics.pcba.panel.category.55
            ("dict.logistics.pcba.panel.category.55", "zh-CN", "comb t", "pcba板位类别.comb t"),
            // dict.logistics.pcba.panel.category.55
            ("dict.logistics.pcba.panel.category.55", "zh-HK", "comb t_hk", "pcba板位类别.comb t"),

            // dict.logistics.pcba.panel.category.56
            ("dict.logistics.pcba.panel.category.56", "en-US", "combo b_us", "pcba板位类别.combo b"),
            // dict.logistics.pcba.panel.category.56
            ("dict.logistics.pcba.panel.category.56", "ja-JP", "combo b_jp", "pcba板位类别.combo b"),
            // dict.logistics.pcba.panel.category.56
            ("dict.logistics.pcba.panel.category.56", "zh-CN", "combo b", "pcba板位类别.combo b"),
            // dict.logistics.pcba.panel.category.56
            ("dict.logistics.pcba.panel.category.56", "zh-HK", "combo b_hk", "pcba板位类别.combo b"),

            // dict.logistics.pcba.panel.category.57
            ("dict.logistics.pcba.panel.category.57", "en-US", "combo t_us", "pcba板位类别.combo t"),
            // dict.logistics.pcba.panel.category.57
            ("dict.logistics.pcba.panel.category.57", "ja-JP", "combo t_jp", "pcba板位类别.combo t"),
            // dict.logistics.pcba.panel.category.57
            ("dict.logistics.pcba.panel.category.57", "zh-CN", "combo t", "pcba板位类别.combo t"),
            // dict.logistics.pcba.panel.category.57
            ("dict.logistics.pcba.panel.category.57", "zh-HK", "combo t_hk", "pcba板位类别.combo t"),

            // dict.logistics.pcba.panel.category.58
            ("dict.logistics.pcba.panel.category.58", "en-US", "conn_us", "pcba板位类别.conn"),
            // dict.logistics.pcba.panel.category.58
            ("dict.logistics.pcba.panel.category.58", "ja-JP", "conn_jp", "pcba板位类别.conn"),
            // dict.logistics.pcba.panel.category.58
            ("dict.logistics.pcba.panel.category.58", "zh-CN", "conn", "pcba板位类别.conn"),
            // dict.logistics.pcba.panel.category.58
            ("dict.logistics.pcba.panel.category.58", "zh-HK", "conn_hk", "pcba板位类别.conn"),

            // dict.logistics.pcba.panel.category.59
            ("dict.logistics.pcba.panel.category.59", "en-US", "conn a_us", "pcba板位类别.conn a"),
            // dict.logistics.pcba.panel.category.59
            ("dict.logistics.pcba.panel.category.59", "ja-JP", "conn a_jp", "pcba板位类别.conn a"),
            // dict.logistics.pcba.panel.category.59
            ("dict.logistics.pcba.panel.category.59", "zh-CN", "conn a", "pcba板位类别.conn a"),
            // dict.logistics.pcba.panel.category.59
            ("dict.logistics.pcba.panel.category.59", "zh-HK", "conn a_hk", "pcba板位类别.conn a"),

            // dict.logistics.pcba.panel.category.60
            ("dict.logistics.pcba.panel.category.60", "en-US", "conn b_us", "pcba板位类别.conn b"),
            // dict.logistics.pcba.panel.category.60
            ("dict.logistics.pcba.panel.category.60", "ja-JP", "conn b_jp", "pcba板位类别.conn b"),
            // dict.logistics.pcba.panel.category.60
            ("dict.logistics.pcba.panel.category.60", "zh-CN", "conn b", "pcba板位类别.conn b"),
            // dict.logistics.pcba.panel.category.60
            ("dict.logistics.pcba.panel.category.60", "zh-HK", "conn b_hk", "pcba板位类别.conn b"),

            // dict.logistics.pcba.panel.category.61
            ("dict.logistics.pcba.panel.category.61", "en-US", "conn b/t_us", "pcba板位类别.conn b/t"),
            // dict.logistics.pcba.panel.category.61
            ("dict.logistics.pcba.panel.category.61", "ja-JP", "conn b/t_jp", "pcba板位类别.conn b/t"),
            // dict.logistics.pcba.panel.category.61
            ("dict.logistics.pcba.panel.category.61", "zh-CN", "conn b/t", "pcba板位类别.conn b/t"),
            // dict.logistics.pcba.panel.category.61
            ("dict.logistics.pcba.panel.category.61", "zh-HK", "conn b/t_hk", "pcba板位类别.conn b/t"),

            // dict.logistics.pcba.panel.category.62
            ("dict.logistics.pcba.panel.category.62", "en-US", "conn t_us", "pcba板位类别.conn t"),
            // dict.logistics.pcba.panel.category.62
            ("dict.logistics.pcba.panel.category.62", "ja-JP", "conn t_jp", "pcba板位类别.conn t"),
            // dict.logistics.pcba.panel.category.62
            ("dict.logistics.pcba.panel.category.62", "zh-CN", "conn t", "pcba板位类别.conn t"),
            // dict.logistics.pcba.panel.category.62
            ("dict.logistics.pcba.panel.category.62", "zh-HK", "conn t_hk", "pcba板位类别.conn t"),

            // dict.logistics.pcba.panel.category.63
            ("dict.logistics.pcba.panel.category.63", "en-US", "contact_us", "pcba板位类别.contact"),
            // dict.logistics.pcba.panel.category.63
            ("dict.logistics.pcba.panel.category.63", "ja-JP", "contact_jp", "pcba板位类别.contact"),
            // dict.logistics.pcba.panel.category.63
            ("dict.logistics.pcba.panel.category.63", "zh-CN", "contact", "pcba板位类别.contact"),
            // dict.logistics.pcba.panel.category.63
            ("dict.logistics.pcba.panel.category.63", "zh-HK", "contact_hk", "pcba板位类别.contact"),

            // dict.logistics.pcba.panel.category.64
            ("dict.logistics.pcba.panel.category.64", "en-US", "da_us", "pcba板位类别.da"),
            // dict.logistics.pcba.panel.category.64
            ("dict.logistics.pcba.panel.category.64", "ja-JP", "da_jp", "pcba板位类别.da"),
            // dict.logistics.pcba.panel.category.64
            ("dict.logistics.pcba.panel.category.64", "zh-CN", "da", "pcba板位类别.da"),
            // dict.logistics.pcba.panel.category.64
            ("dict.logistics.pcba.panel.category.64", "zh-HK", "da_hk", "pcba板位类别.da"),

            // dict.logistics.pcba.panel.category.65
            ("dict.logistics.pcba.panel.category.65", "en-US", "da b_us", "pcba板位类别.da b"),
            // dict.logistics.pcba.panel.category.65
            ("dict.logistics.pcba.panel.category.65", "ja-JP", "da b_jp", "pcba板位类别.da b"),
            // dict.logistics.pcba.panel.category.65
            ("dict.logistics.pcba.panel.category.65", "zh-CN", "da b", "pcba板位类别.da b"),
            // dict.logistics.pcba.panel.category.65
            ("dict.logistics.pcba.panel.category.65", "zh-HK", "da b_hk", "pcba板位类别.da b"),

            // dict.logistics.pcba.panel.category.66
            ("dict.logistics.pcba.panel.category.66", "en-US", "da t_us", "pcba板位类别.da t"),
            // dict.logistics.pcba.panel.category.66
            ("dict.logistics.pcba.panel.category.66", "ja-JP", "da t_jp", "pcba板位类别.da t"),
            // dict.logistics.pcba.panel.category.66
            ("dict.logistics.pcba.panel.category.66", "zh-CN", "da t", "pcba板位类别.da t"),
            // dict.logistics.pcba.panel.category.66
            ("dict.logistics.pcba.panel.category.66", "zh-HK", "da t_hk", "pcba板位类别.da t"),

            // dict.logistics.pcba.panel.category.67
            ("dict.logistics.pcba.panel.category.67", "en-US", "da t/b_us", "pcba板位类别.da t/b"),
            // dict.logistics.pcba.panel.category.67
            ("dict.logistics.pcba.panel.category.67", "ja-JP", "da t/b_jp", "pcba板位类别.da t/b"),
            // dict.logistics.pcba.panel.category.67
            ("dict.logistics.pcba.panel.category.67", "zh-CN", "da t/b", "pcba板位类别.da t/b"),
            // dict.logistics.pcba.panel.category.67
            ("dict.logistics.pcba.panel.category.67", "zh-HK", "da t/b_hk", "pcba板位类别.da t/b"),

            // dict.logistics.pcba.panel.category.68
            ("dict.logistics.pcba.panel.category.68", "en-US", "dany b_us", "pcba板位类别.dany b"),
            // dict.logistics.pcba.panel.category.68
            ("dict.logistics.pcba.panel.category.68", "ja-JP", "dany b_jp", "pcba板位类别.dany b"),
            // dict.logistics.pcba.panel.category.68
            ("dict.logistics.pcba.panel.category.68", "zh-CN", "dany b", "pcba板位类别.dany b"),
            // dict.logistics.pcba.panel.category.68
            ("dict.logistics.pcba.panel.category.68", "zh-HK", "dany b_hk", "pcba板位类别.dany b"),

            // dict.logistics.pcba.panel.category.70
            ("dict.logistics.pcba.panel.category.70", "en-US", "dsp b_us", "pcba板位类别.dsp b"),
            // dict.logistics.pcba.panel.category.70
            ("dict.logistics.pcba.panel.category.70", "ja-JP", "dsp b_jp", "pcba板位类别.dsp b"),
            // dict.logistics.pcba.panel.category.70
            ("dict.logistics.pcba.panel.category.70", "zh-CN", "dsp b", "pcba板位类别.dsp b"),
            // dict.logistics.pcba.panel.category.70
            ("dict.logistics.pcba.panel.category.70", "zh-HK", "dsp b_hk", "pcba板位类别.dsp b"),

            // dict.logistics.pcba.panel.category.71
            ("dict.logistics.pcba.panel.category.71", "en-US", "dsp t_us", "pcba板位类别.dsp t"),
            // dict.logistics.pcba.panel.category.71
            ("dict.logistics.pcba.panel.category.71", "ja-JP", "dsp t_jp", "pcba板位类别.dsp t"),
            // dict.logistics.pcba.panel.category.71
            ("dict.logistics.pcba.panel.category.71", "zh-CN", "dsp t", "pcba板位类别.dsp t"),
            // dict.logistics.pcba.panel.category.71
            ("dict.logistics.pcba.panel.category.71", "zh-HK", "dsp t_hk", "pcba板位类别.dsp t"),

            // dict.logistics.pcba.panel.category.72
            ("dict.logistics.pcba.panel.category.72", "en-US", "dspl  t_us", "pcba板位类别.dspl  t"),
            // dict.logistics.pcba.panel.category.72
            ("dict.logistics.pcba.panel.category.72", "ja-JP", "dspl  t_jp", "pcba板位类别.dspl  t"),
            // dict.logistics.pcba.panel.category.72
            ("dict.logistics.pcba.panel.category.72", "zh-CN", "dspl  t", "pcba板位类别.dspl  t"),
            // dict.logistics.pcba.panel.category.72
            ("dict.logistics.pcba.panel.category.72", "zh-HK", "dspl  t_hk", "pcba板位类别.dspl  t"),

            // dict.logistics.pcba.panel.category.73
            ("dict.logistics.pcba.panel.category.73", "en-US", "dspl a_us", "pcba板位类别.dspl a"),
            // dict.logistics.pcba.panel.category.73
            ("dict.logistics.pcba.panel.category.73", "ja-JP", "dspl a_jp", "pcba板位类别.dspl a"),
            // dict.logistics.pcba.panel.category.73
            ("dict.logistics.pcba.panel.category.73", "zh-CN", "dspl a", "pcba板位类别.dspl a"),
            // dict.logistics.pcba.panel.category.73
            ("dict.logistics.pcba.panel.category.73", "zh-HK", "dspl a_hk", "pcba板位类别.dspl a"),

            // dict.logistics.pcba.panel.category.74
            ("dict.logistics.pcba.panel.category.74", "en-US", "dspl b_us", "pcba板位类别.dspl b"),
            // dict.logistics.pcba.panel.category.74
            ("dict.logistics.pcba.panel.category.74", "ja-JP", "dspl b_jp", "pcba板位类别.dspl b"),
            // dict.logistics.pcba.panel.category.74
            ("dict.logistics.pcba.panel.category.74", "zh-CN", "dspl b", "pcba板位类别.dspl b"),
            // dict.logistics.pcba.panel.category.74
            ("dict.logistics.pcba.panel.category.74", "zh-HK", "dspl b_hk", "pcba板位类别.dspl b"),

            // dict.logistics.pcba.panel.category.75
            ("dict.logistics.pcba.panel.category.75", "en-US", "dspl b/t_us", "pcba板位类别.dspl b/t"),
            // dict.logistics.pcba.panel.category.75
            ("dict.logistics.pcba.panel.category.75", "ja-JP", "dspl b/t_jp", "pcba板位类别.dspl b/t"),
            // dict.logistics.pcba.panel.category.75
            ("dict.logistics.pcba.panel.category.75", "zh-CN", "dspl b/t", "pcba板位类别.dspl b/t"),
            // dict.logistics.pcba.panel.category.75
            ("dict.logistics.pcba.panel.category.75", "zh-HK", "dspl b/t_hk", "pcba板位类别.dspl b/t"),

            // dict.logistics.pcba.panel.category.76
            ("dict.logistics.pcba.panel.category.76", "en-US", "dspl t_us", "pcba板位类别.dspl t"),
            // dict.logistics.pcba.panel.category.76
            ("dict.logistics.pcba.panel.category.76", "ja-JP", "dspl t_jp", "pcba板位类别.dspl t"),
            // dict.logistics.pcba.panel.category.76
            ("dict.logistics.pcba.panel.category.76", "zh-CN", "dspl t", "pcba板位类别.dspl t"),
            // dict.logistics.pcba.panel.category.76
            ("dict.logistics.pcba.panel.category.76", "zh-HK", "dspl t_hk", "pcba板位类别.dspl t"),

            // dict.logistics.pcba.panel.category.77
            ("dict.logistics.pcba.panel.category.77", "en-US", "dsub b_us", "pcba板位类别.dsub b"),
            // dict.logistics.pcba.panel.category.77
            ("dict.logistics.pcba.panel.category.77", "ja-JP", "dsub b_jp", "pcba板位类别.dsub b"),
            // dict.logistics.pcba.panel.category.77
            ("dict.logistics.pcba.panel.category.77", "zh-CN", "dsub b", "pcba板位类别.dsub b"),
            // dict.logistics.pcba.panel.category.77
            ("dict.logistics.pcba.panel.category.77", "zh-HK", "dsub b_hk", "pcba板位类别.dsub b"),

            // dict.logistics.pcba.panel.category.78
            ("dict.logistics.pcba.panel.category.78", "en-US", "dsub t_us", "pcba板位类别.dsub t"),
            // dict.logistics.pcba.panel.category.78
            ("dict.logistics.pcba.panel.category.78", "ja-JP", "dsub t_jp", "pcba板位类别.dsub t"),
            // dict.logistics.pcba.panel.category.78
            ("dict.logistics.pcba.panel.category.78", "zh-CN", "dsub t", "pcba板位类别.dsub t"),
            // dict.logistics.pcba.panel.category.78
            ("dict.logistics.pcba.panel.category.78", "zh-HK", "dsub t_hk", "pcba板位类别.dsub t"),

            // dict.logistics.pcba.panel.category.79
            ("dict.logistics.pcba.panel.category.79", "en-US", "dyna b_us", "pcba板位类别.dyna b"),
            // dict.logistics.pcba.panel.category.79
            ("dict.logistics.pcba.panel.category.79", "ja-JP", "dyna b_jp", "pcba板位类别.dyna b"),
            // dict.logistics.pcba.panel.category.79
            ("dict.logistics.pcba.panel.category.79", "zh-CN", "dyna b", "pcba板位类别.dyna b"),
            // dict.logistics.pcba.panel.category.79
            ("dict.logistics.pcba.panel.category.79", "zh-HK", "dyna b_hk", "pcba板位类别.dyna b"),

            // dict.logistics.pcba.panel.category.80
            ("dict.logistics.pcba.panel.category.80", "en-US", "dyna t_us", "pcba板位类别.dyna t"),
            // dict.logistics.pcba.panel.category.80
            ("dict.logistics.pcba.panel.category.80", "ja-JP", "dyna t_jp", "pcba板位类别.dyna t"),
            // dict.logistics.pcba.panel.category.80
            ("dict.logistics.pcba.panel.category.80", "zh-CN", "dyna t", "pcba板位类别.dyna t"),
            // dict.logistics.pcba.panel.category.80
            ("dict.logistics.pcba.panel.category.80", "zh-HK", "dyna t_hk", "pcba板位类别.dyna t"),

            // dict.logistics.pcba.panel.category.81
            ("dict.logistics.pcba.panel.category.81", "en-US", "dyna t/b_us", "pcba板位类别.dyna t/b"),
            // dict.logistics.pcba.panel.category.81
            ("dict.logistics.pcba.panel.category.81", "ja-JP", "dyna t/b_jp", "pcba板位类别.dyna t/b"),
            // dict.logistics.pcba.panel.category.81
            ("dict.logistics.pcba.panel.category.81", "zh-CN", "dyna t/b", "pcba板位类别.dyna t/b"),
            // dict.logistics.pcba.panel.category.81
            ("dict.logistics.pcba.panel.category.81", "zh-HK", "dyna t/b_hk", "pcba板位类别.dyna t/b"),

            // dict.logistics.pcba.panel.category.82
            ("dict.logistics.pcba.panel.category.82", "en-US", "encoder_us", "pcba板位类别.encoder"),
            // dict.logistics.pcba.panel.category.82
            ("dict.logistics.pcba.panel.category.82", "ja-JP", "encoder_jp", "pcba板位类别.encoder"),
            // dict.logistics.pcba.panel.category.82
            ("dict.logistics.pcba.panel.category.82", "zh-CN", "encoder", "pcba板位类别.encoder"),
            // dict.logistics.pcba.panel.category.82
            ("dict.logistics.pcba.panel.category.82", "zh-HK", "encoder_hk", "pcba板位类别.encoder"),

            // dict.logistics.pcba.panel.category.83
            ("dict.logistics.pcba.panel.category.83", "en-US", "encoger_us", "pcba板位类别.encoger"),
            // dict.logistics.pcba.panel.category.83
            ("dict.logistics.pcba.panel.category.83", "ja-JP", "encoger_jp", "pcba板位类别.encoger"),
            // dict.logistics.pcba.panel.category.83
            ("dict.logistics.pcba.panel.category.83", "zh-CN", "encoger", "pcba板位类别.encoger"),
            // dict.logistics.pcba.panel.category.83
            ("dict.logistics.pcba.panel.category.83", "zh-HK", "encoger_hk", "pcba板位类别.encoger"),

            // dict.logistics.pcba.panel.category.84
            ("dict.logistics.pcba.panel.category.84", "en-US", "ether_us", "pcba板位类别.ether"),
            // dict.logistics.pcba.panel.category.84
            ("dict.logistics.pcba.panel.category.84", "ja-JP", "ether_jp", "pcba板位类别.ether"),
            // dict.logistics.pcba.panel.category.84
            ("dict.logistics.pcba.panel.category.84", "zh-CN", "ether", "pcba板位类别.ether"),
            // dict.logistics.pcba.panel.category.84
            ("dict.logistics.pcba.panel.category.84", "zh-HK", "ether_hk", "pcba板位类别.ether"),

            // dict.logistics.pcba.panel.category.85
            ("dict.logistics.pcba.panel.category.85", "en-US", "ether b_us", "pcba板位类别.ether b"),
            // dict.logistics.pcba.panel.category.85
            ("dict.logistics.pcba.panel.category.85", "ja-JP", "ether b_jp", "pcba板位类别.ether b"),
            // dict.logistics.pcba.panel.category.85
            ("dict.logistics.pcba.panel.category.85", "zh-CN", "ether b", "pcba板位类别.ether b"),
            // dict.logistics.pcba.panel.category.85
            ("dict.logistics.pcba.panel.category.85", "zh-HK", "ether b_hk", "pcba板位类别.ether b"),

            // dict.logistics.pcba.panel.category.86
            ("dict.logistics.pcba.panel.category.86", "en-US", "ether t_us", "pcba板位类别.ether t"),
            // dict.logistics.pcba.panel.category.86
            ("dict.logistics.pcba.panel.category.86", "ja-JP", "ether t_jp", "pcba板位类别.ether t"),
            // dict.logistics.pcba.panel.category.86
            ("dict.logistics.pcba.panel.category.86", "zh-CN", "ether t", "pcba板位类别.ether t"),
            // dict.logistics.pcba.panel.category.86
            ("dict.logistics.pcba.panel.category.86", "zh-HK", "ether t_hk", "pcba板位类别.ether t"),

            // dict.logistics.pcba.panel.category.87
            ("dict.logistics.pcba.panel.category.87", "en-US", "euro_us", "pcba板位类别.euro"),
            // dict.logistics.pcba.panel.category.87
            ("dict.logistics.pcba.panel.category.87", "ja-JP", "euro_jp", "pcba板位类别.euro"),
            // dict.logistics.pcba.panel.category.87
            ("dict.logistics.pcba.panel.category.87", "zh-CN", "euro", "pcba板位类别.euro"),
            // dict.logistics.pcba.panel.category.87
            ("dict.logistics.pcba.panel.category.87", "zh-HK", "euro_hk", "pcba板位类别.euro"),

            // dict.logistics.pcba.panel.category.88
            ("dict.logistics.pcba.panel.category.88", "en-US", "euro b_us", "pcba板位类别.euro b"),
            // dict.logistics.pcba.panel.category.88
            ("dict.logistics.pcba.panel.category.88", "ja-JP", "euro b_jp", "pcba板位类别.euro b"),
            // dict.logistics.pcba.panel.category.88
            ("dict.logistics.pcba.panel.category.88", "zh-CN", "euro b", "pcba板位类别.euro b"),
            // dict.logistics.pcba.panel.category.88
            ("dict.logistics.pcba.panel.category.88", "zh-HK", "euro b_hk", "pcba板位类别.euro b"),

            // dict.logistics.pcba.panel.category.89
            ("dict.logistics.pcba.panel.category.89", "en-US", "euro b/t_us", "pcba板位类别.euro b/t"),
            // dict.logistics.pcba.panel.category.89
            ("dict.logistics.pcba.panel.category.89", "ja-JP", "euro b/t_jp", "pcba板位类别.euro b/t"),
            // dict.logistics.pcba.panel.category.89
            ("dict.logistics.pcba.panel.category.89", "zh-CN", "euro b/t", "pcba板位类别.euro b/t"),
            // dict.logistics.pcba.panel.category.89
            ("dict.logistics.pcba.panel.category.89", "zh-HK", "euro b/t_hk", "pcba板位类别.euro b/t"),

            // dict.logistics.pcba.panel.category.90
            ("dict.logistics.pcba.panel.category.90", "en-US", "euro t_us", "pcba板位类别.euro t"),
            // dict.logistics.pcba.panel.category.90
            ("dict.logistics.pcba.panel.category.90", "ja-JP", "euro t_jp", "pcba板位类别.euro t"),
            // dict.logistics.pcba.panel.category.90
            ("dict.logistics.pcba.panel.category.90", "zh-CN", "euro t", "pcba板位类别.euro t"),
            // dict.logistics.pcba.panel.category.90
            ("dict.logistics.pcba.panel.category.90", "zh-HK", "euro t_hk", "pcba板位类别.euro t"),

            // dict.logistics.pcba.panel.category.91
            ("dict.logistics.pcba.panel.category.91", "en-US", "fader b_us", "pcba板位类别.fader b"),
            // dict.logistics.pcba.panel.category.91
            ("dict.logistics.pcba.panel.category.91", "ja-JP", "fader b_jp", "pcba板位类别.fader b"),
            // dict.logistics.pcba.panel.category.91
            ("dict.logistics.pcba.panel.category.91", "zh-CN", "fader b", "pcba板位类别.fader b"),
            // dict.logistics.pcba.panel.category.91
            ("dict.logistics.pcba.panel.category.91", "zh-HK", "fader b_hk", "pcba板位类别.fader b"),

            // dict.logistics.pcba.panel.category.92
            ("dict.logistics.pcba.panel.category.92", "en-US", "fader b/t_us", "pcba板位类别.fader b/t"),
            // dict.logistics.pcba.panel.category.92
            ("dict.logistics.pcba.panel.category.92", "ja-JP", "fader b/t_jp", "pcba板位类别.fader b/t"),
            // dict.logistics.pcba.panel.category.92
            ("dict.logistics.pcba.panel.category.92", "zh-CN", "fader b/t", "pcba板位类别.fader b/t"),
            // dict.logistics.pcba.panel.category.92
            ("dict.logistics.pcba.panel.category.92", "zh-HK", "fader b/t_hk", "pcba板位类别.fader b/t"),

            // dict.logistics.pcba.panel.category.93
            ("dict.logistics.pcba.panel.category.93", "en-US", "fader t_us", "pcba板位类别.fader t"),
            // dict.logistics.pcba.panel.category.93
            ("dict.logistics.pcba.panel.category.93", "ja-JP", "fader t_jp", "pcba板位类别.fader t"),
            // dict.logistics.pcba.panel.category.93
            ("dict.logistics.pcba.panel.category.93", "zh-CN", "fader t", "pcba板位类别.fader t"),
            // dict.logistics.pcba.panel.category.93
            ("dict.logistics.pcba.panel.category.93", "zh-HK", "fader t_hk", "pcba板位类别.fader t"),

            // dict.logistics.pcba.panel.category.94
            ("dict.logistics.pcba.panel.category.94", "en-US", "faether b_us", "pcba板位类别.faether b"),
            // dict.logistics.pcba.panel.category.94
            ("dict.logistics.pcba.panel.category.94", "ja-JP", "faether b_jp", "pcba板位类别.faether b"),
            // dict.logistics.pcba.panel.category.94
            ("dict.logistics.pcba.panel.category.94", "zh-CN", "faether b", "pcba板位类别.faether b"),
            // dict.logistics.pcba.panel.category.94
            ("dict.logistics.pcba.panel.category.94", "zh-HK", "faether b_hk", "pcba板位类别.faether b"),

            // dict.logistics.pcba.panel.category.95
            ("dict.logistics.pcba.panel.category.95", "en-US", "faether t_us", "pcba板位类别.faether t"),
            // dict.logistics.pcba.panel.category.95
            ("dict.logistics.pcba.panel.category.95", "ja-JP", "faether t_jp", "pcba板位类别.faether t"),
            // dict.logistics.pcba.panel.category.95
            ("dict.logistics.pcba.panel.category.95", "zh-CN", "faether t", "pcba板位类别.faether t"),
            // dict.logistics.pcba.panel.category.95
            ("dict.logistics.pcba.panel.category.95", "zh-HK", "faether t_hk", "pcba板位类别.faether t"),

            // dict.logistics.pcba.panel.category.96
            ("dict.logistics.pcba.panel.category.96", "en-US", "front_us", "pcba板位类别.front"),
            // dict.logistics.pcba.panel.category.96
            ("dict.logistics.pcba.panel.category.96", "ja-JP", "front_jp", "pcba板位类别.front"),
            // dict.logistics.pcba.panel.category.96
            ("dict.logistics.pcba.panel.category.96", "zh-CN", "front", "pcba板位类别.front"),
            // dict.logistics.pcba.panel.category.96
            ("dict.logistics.pcba.panel.category.96", "zh-HK", "front_hk", "pcba板位类别.front"),

            // dict.logistics.pcba.panel.category.97
            ("dict.logistics.pcba.panel.category.97", "en-US", "front a_us", "pcba板位类别.front a"),
            // dict.logistics.pcba.panel.category.97
            ("dict.logistics.pcba.panel.category.97", "ja-JP", "front a_jp", "pcba板位类别.front a"),
            // dict.logistics.pcba.panel.category.97
            ("dict.logistics.pcba.panel.category.97", "zh-CN", "front a", "pcba板位类别.front a"),
            // dict.logistics.pcba.panel.category.97
            ("dict.logistics.pcba.panel.category.97", "zh-HK", "front a_hk", "pcba板位类别.front a"),

            // dict.logistics.pcba.panel.category.98
            ("dict.logistics.pcba.panel.category.98", "en-US", "front b_us", "pcba板位类别.front b"),
            // dict.logistics.pcba.panel.category.98
            ("dict.logistics.pcba.panel.category.98", "ja-JP", "front b_jp", "pcba板位类别.front b"),
            // dict.logistics.pcba.panel.category.98
            ("dict.logistics.pcba.panel.category.98", "zh-CN", "front b", "pcba板位类别.front b"),
            // dict.logistics.pcba.panel.category.98
            ("dict.logistics.pcba.panel.category.98", "zh-HK", "front b_hk", "pcba板位类别.front b"),

            // dict.logistics.pcba.panel.category.99
            ("dict.logistics.pcba.panel.category.99", "en-US", "front b/t_us", "pcba板位类别.front b/t"),
            // dict.logistics.pcba.panel.category.99
            ("dict.logistics.pcba.panel.category.99", "ja-JP", "front b/t_jp", "pcba板位类别.front b/t"),
            // dict.logistics.pcba.panel.category.99
            ("dict.logistics.pcba.panel.category.99", "zh-CN", "front b/t", "pcba板位类别.front b/t"),
            // dict.logistics.pcba.panel.category.99
            ("dict.logistics.pcba.panel.category.99", "zh-HK", "front b/t_hk", "pcba板位类别.front b/t"),

            // dict.logistics.pcba.panel.category.100
            ("dict.logistics.pcba.panel.category.100", "en-US", "front sys t_us", "pcba板位类别.front sys t"),
            // dict.logistics.pcba.panel.category.100
            ("dict.logistics.pcba.panel.category.100", "ja-JP", "front sys t_jp", "pcba板位类别.front sys t"),
            // dict.logistics.pcba.panel.category.100
            ("dict.logistics.pcba.panel.category.100", "zh-CN", "front sys t", "pcba板位类别.front sys t"),
            // dict.logistics.pcba.panel.category.100
            ("dict.logistics.pcba.panel.category.100", "zh-HK", "front sys t_hk", "pcba板位类别.front sys t"),

            // dict.logistics.pcba.panel.category.101
            ("dict.logistics.pcba.panel.category.101", "en-US", "front t_us", "pcba板位类别.front t"),
            // dict.logistics.pcba.panel.category.101
            ("dict.logistics.pcba.panel.category.101", "ja-JP", "front t_jp", "pcba板位类别.front t"),
            // dict.logistics.pcba.panel.category.101
            ("dict.logistics.pcba.panel.category.101", "zh-CN", "front t", "pcba板位类别.front t"),
            // dict.logistics.pcba.panel.category.101
            ("dict.logistics.pcba.panel.category.101", "zh-HK", "front t_hk", "pcba板位类别.front t"),

            // dict.logistics.pcba.panel.category.102
            ("dict.logistics.pcba.panel.category.102", "en-US", "front-a_us", "pcba板位类别.front-a"),
            // dict.logistics.pcba.panel.category.102
            ("dict.logistics.pcba.panel.category.102", "ja-JP", "front-a_jp", "pcba板位类别.front-a"),
            // dict.logistics.pcba.panel.category.102
            ("dict.logistics.pcba.panel.category.102", "zh-CN", "front-a", "pcba板位类别.front-a"),
            // dict.logistics.pcba.panel.category.102
            ("dict.logistics.pcba.panel.category.102", "zh-HK", "front-a_hk", "pcba板位类别.front-a"),

            // dict.logistics.pcba.panel.category.103
            ("dict.logistics.pcba.panel.category.103", "en-US", "frotn b_us", "pcba板位类别.frotn b"),
            // dict.logistics.pcba.panel.category.103
            ("dict.logistics.pcba.panel.category.103", "ja-JP", "frotn b_jp", "pcba板位类别.frotn b"),
            // dict.logistics.pcba.panel.category.103
            ("dict.logistics.pcba.panel.category.103", "zh-CN", "frotn b", "pcba板位类别.frotn b"),
            // dict.logistics.pcba.panel.category.103
            ("dict.logistics.pcba.panel.category.103", "zh-HK", "frotn b_hk", "pcba板位类别.frotn b"),

            // dict.logistics.pcba.panel.category.104
            ("dict.logistics.pcba.panel.category.104", "en-US", "gather_us", "pcba板位类别.gather"),
            // dict.logistics.pcba.panel.category.104
            ("dict.logistics.pcba.panel.category.104", "ja-JP", "gather_jp", "pcba板位类别.gather"),
            // dict.logistics.pcba.panel.category.104
            ("dict.logistics.pcba.panel.category.104", "zh-CN", "gather", "pcba板位类别.gather"),
            // dict.logistics.pcba.panel.category.104
            ("dict.logistics.pcba.panel.category.104", "zh-HK", "gather_hk", "pcba板位类别.gather"),

            // dict.logistics.pcba.panel.category.105
            ("dict.logistics.pcba.panel.category.105", "en-US", "gather a_us", "pcba板位类别.gather a"),
            // dict.logistics.pcba.panel.category.105
            ("dict.logistics.pcba.panel.category.105", "ja-JP", "gather a_jp", "pcba板位类别.gather a"),
            // dict.logistics.pcba.panel.category.105
            ("dict.logistics.pcba.panel.category.105", "zh-CN", "gather a", "pcba板位类别.gather a"),
            // dict.logistics.pcba.panel.category.105
            ("dict.logistics.pcba.panel.category.105", "zh-HK", "gather a_hk", "pcba板位类别.gather a"),

            // dict.logistics.pcba.panel.category.106
            ("dict.logistics.pcba.panel.category.106", "en-US", "gather alt b_us", "pcba板位类别.gather alt b"),
            // dict.logistics.pcba.panel.category.106
            ("dict.logistics.pcba.panel.category.106", "ja-JP", "gather alt b_jp", "pcba板位类别.gather alt b"),
            // dict.logistics.pcba.panel.category.106
            ("dict.logistics.pcba.panel.category.106", "zh-CN", "gather alt b", "pcba板位类别.gather alt b"),
            // dict.logistics.pcba.panel.category.106
            ("dict.logistics.pcba.panel.category.106", "zh-HK", "gather alt b_hk", "pcba板位类别.gather alt b"),

            // dict.logistics.pcba.panel.category.107
            ("dict.logistics.pcba.panel.category.107", "en-US", "gather alt t_us", "pcba板位类别.gather alt t"),
            // dict.logistics.pcba.panel.category.107
            ("dict.logistics.pcba.panel.category.107", "ja-JP", "gather alt t_jp", "pcba板位类别.gather alt t"),
            // dict.logistics.pcba.panel.category.107
            ("dict.logistics.pcba.panel.category.107", "zh-CN", "gather alt t", "pcba板位类别.gather alt t"),
            // dict.logistics.pcba.panel.category.107
            ("dict.logistics.pcba.panel.category.107", "zh-HK", "gather alt t_hk", "pcba板位类别.gather alt t"),

            // dict.logistics.pcba.panel.category.108
            ("dict.logistics.pcba.panel.category.108", "en-US", "gather b_us", "pcba板位类别.gather b"),
            // dict.logistics.pcba.panel.category.108
            ("dict.logistics.pcba.panel.category.108", "ja-JP", "gather b_jp", "pcba板位类别.gather b"),
            // dict.logistics.pcba.panel.category.108
            ("dict.logistics.pcba.panel.category.108", "zh-CN", "gather b", "pcba板位类别.gather b"),
            // dict.logistics.pcba.panel.category.108
            ("dict.logistics.pcba.panel.category.108", "zh-HK", "gather b_hk", "pcba板位类别.gather b"),

            // dict.logistics.pcba.panel.category.109
            ("dict.logistics.pcba.panel.category.109", "en-US", "gather b/t_us", "pcba板位类别.gather b/t"),
            // dict.logistics.pcba.panel.category.109
            ("dict.logistics.pcba.panel.category.109", "ja-JP", "gather b/t_jp", "pcba板位类别.gather b/t"),
            // dict.logistics.pcba.panel.category.109
            ("dict.logistics.pcba.panel.category.109", "zh-CN", "gather b/t", "pcba板位类别.gather b/t"),
            // dict.logistics.pcba.panel.category.109
            ("dict.logistics.pcba.panel.category.109", "zh-HK", "gather b/t_hk", "pcba板位类别.gather b/t"),

            // dict.logistics.pcba.panel.category.110
            ("dict.logistics.pcba.panel.category.110", "en-US", "gather c_us", "pcba板位类别.gather c"),
            // dict.logistics.pcba.panel.category.110
            ("dict.logistics.pcba.panel.category.110", "ja-JP", "gather c_jp", "pcba板位类别.gather c"),
            // dict.logistics.pcba.panel.category.110
            ("dict.logistics.pcba.panel.category.110", "zh-CN", "gather c", "pcba板位类别.gather c"),
            // dict.logistics.pcba.panel.category.110
            ("dict.logistics.pcba.panel.category.110", "zh-HK", "gather c_hk", "pcba板位类别.gather c"),

            // dict.logistics.pcba.panel.category.111
            ("dict.logistics.pcba.panel.category.111", "en-US", "gather t_us", "pcba板位类别.gather t"),
            // dict.logistics.pcba.panel.category.111
            ("dict.logistics.pcba.panel.category.111", "ja-JP", "gather t_jp", "pcba板位类别.gather t"),
            // dict.logistics.pcba.panel.category.111
            ("dict.logistics.pcba.panel.category.111", "zh-CN", "gather t", "pcba板位类别.gather t"),
            // dict.logistics.pcba.panel.category.111
            ("dict.logistics.pcba.panel.category.111", "zh-HK", "gather t_hk", "pcba板位类别.gather t"),

            // dict.logistics.pcba.panel.category.112
            ("dict.logistics.pcba.panel.category.112", "en-US", "gather-c_us", "pcba板位类别.gather-c"),
            // dict.logistics.pcba.panel.category.112
            ("dict.logistics.pcba.panel.category.112", "ja-JP", "gather-c_jp", "pcba板位类别.gather-c"),
            // dict.logistics.pcba.panel.category.112
            ("dict.logistics.pcba.panel.category.112", "zh-CN", "gather-c", "pcba板位类别.gather-c"),
            // dict.logistics.pcba.panel.category.112
            ("dict.logistics.pcba.panel.category.112", "zh-HK", "gather-c_hk", "pcba板位类别.gather-c"),

            // dict.logistics.pcba.panel.category.113
            ("dict.logistics.pcba.panel.category.113", "en-US", "gather-j_us", "pcba板位类别.gather-j"),
            // dict.logistics.pcba.panel.category.113
            ("dict.logistics.pcba.panel.category.113", "ja-JP", "gather-j_jp", "pcba板位类别.gather-j"),
            // dict.logistics.pcba.panel.category.113
            ("dict.logistics.pcba.panel.category.113", "zh-CN", "gather-j", "pcba板位类别.gather-j"),
            // dict.logistics.pcba.panel.category.113
            ("dict.logistics.pcba.panel.category.113", "zh-HK", "gather-j_hk", "pcba板位类别.gather-j"),

            // dict.logistics.pcba.panel.category.114
            ("dict.logistics.pcba.panel.category.114", "en-US", "if_us", "pcba板位类别.if"),
            // dict.logistics.pcba.panel.category.114
            ("dict.logistics.pcba.panel.category.114", "ja-JP", "if_jp", "pcba板位类别.if"),
            // dict.logistics.pcba.panel.category.114
            ("dict.logistics.pcba.panel.category.114", "zh-CN", "if", "pcba板位类别.if"),
            // dict.logistics.pcba.panel.category.114
            ("dict.logistics.pcba.panel.category.114", "zh-HK", "if_hk", "pcba板位类别.if"),

            // dict.logistics.pcba.panel.category.117
            ("dict.logistics.pcba.panel.category.117", "en-US", "if b_us", "pcba板位类别.if b"),
            // dict.logistics.pcba.panel.category.117
            ("dict.logistics.pcba.panel.category.117", "ja-JP", "if b_jp", "pcba板位类别.if b"),
            // dict.logistics.pcba.panel.category.117
            ("dict.logistics.pcba.panel.category.117", "zh-CN", "if b", "pcba板位类别.if b"),
            // dict.logistics.pcba.panel.category.117
            ("dict.logistics.pcba.panel.category.117", "zh-HK", "if b_hk", "pcba板位类别.if b"),

            // dict.logistics.pcba.panel.category.118
            ("dict.logistics.pcba.panel.category.118", "en-US", "if t_us", "pcba板位类别.if t"),
            // dict.logistics.pcba.panel.category.118
            ("dict.logistics.pcba.panel.category.118", "ja-JP", "if t_jp", "pcba板位类别.if t"),
            // dict.logistics.pcba.panel.category.118
            ("dict.logistics.pcba.panel.category.118", "zh-CN", "if t", "pcba板位类别.if t"),
            // dict.logistics.pcba.panel.category.118
            ("dict.logistics.pcba.panel.category.118", "zh-HK", "if t_hk", "pcba板位类别.if t"),

            // dict.logistics.pcba.panel.category.119
            ("dict.logistics.pcba.panel.category.119", "en-US", "input_us", "pcba板位类别.input"),
            // dict.logistics.pcba.panel.category.119
            ("dict.logistics.pcba.panel.category.119", "ja-JP", "input_jp", "pcba板位类别.input"),
            // dict.logistics.pcba.panel.category.119
            ("dict.logistics.pcba.panel.category.119", "zh-CN", "input", "pcba板位类别.input"),
            // dict.logistics.pcba.panel.category.119
            ("dict.logistics.pcba.panel.category.119", "zh-HK", "input_hk", "pcba板位类别.input"),

            // dict.logistics.pcba.panel.category.120
            ("dict.logistics.pcba.panel.category.120", "en-US", "io_us", "pcba板位类别.io"),
            // dict.logistics.pcba.panel.category.120
            ("dict.logistics.pcba.panel.category.120", "ja-JP", "io_jp", "pcba板位类别.io"),
            // dict.logistics.pcba.panel.category.120
            ("dict.logistics.pcba.panel.category.120", "zh-CN", "io", "pcba板位类别.io"),
            // dict.logistics.pcba.panel.category.120
            ("dict.logistics.pcba.panel.category.120", "zh-HK", "io_hk", "pcba板位类别.io"),

            // dict.logistics.pcba.panel.category.121
            ("dict.logistics.pcba.panel.category.121", "en-US", "io b/t_us", "pcba板位类别.io b/t"),
            // dict.logistics.pcba.panel.category.121
            ("dict.logistics.pcba.panel.category.121", "ja-JP", "io b/t_jp", "pcba板位类别.io b/t"),
            // dict.logistics.pcba.panel.category.121
            ("dict.logistics.pcba.panel.category.121", "zh-CN", "io b/t", "pcba板位类别.io b/t"),
            // dict.logistics.pcba.panel.category.121
            ("dict.logistics.pcba.panel.category.121", "zh-HK", "io b/t_hk", "pcba板位类别.io b/t"),

            // dict.logistics.pcba.panel.category.122
            ("dict.logistics.pcba.panel.category.122", "en-US", "io t_us", "pcba板位类别.io t"),
            // dict.logistics.pcba.panel.category.122
            ("dict.logistics.pcba.panel.category.122", "ja-JP", "io t_jp", "pcba板位类别.io t"),
            // dict.logistics.pcba.panel.category.122
            ("dict.logistics.pcba.panel.category.122", "zh-CN", "io t", "pcba板位类别.io t"),
            // dict.logistics.pcba.panel.category.122
            ("dict.logistics.pcba.panel.category.122", "zh-HK", "io t_hk", "pcba板位类别.io t"),

            // dict.logistics.pcba.panel.category.123
            ("dict.logistics.pcba.panel.category.123", "en-US", "jack_us", "pcba板位类别.jack"),
            // dict.logistics.pcba.panel.category.123
            ("dict.logistics.pcba.panel.category.123", "ja-JP", "jack_jp", "pcba板位类别.jack"),
            // dict.logistics.pcba.panel.category.123
            ("dict.logistics.pcba.panel.category.123", "zh-CN", "jack", "pcba板位类别.jack"),
            // dict.logistics.pcba.panel.category.123
            ("dict.logistics.pcba.panel.category.123", "zh-HK", "jack_hk", "pcba板位类别.jack"),

            // dict.logistics.pcba.panel.category.124
            ("dict.logistics.pcba.panel.category.124", "en-US", "jack a_us", "pcba板位类别.jack a"),
            // dict.logistics.pcba.panel.category.124
            ("dict.logistics.pcba.panel.category.124", "ja-JP", "jack a_jp", "pcba板位类别.jack a"),
            // dict.logistics.pcba.panel.category.124
            ("dict.logistics.pcba.panel.category.124", "zh-CN", "jack a", "pcba板位类别.jack a"),
            // dict.logistics.pcba.panel.category.124
            ("dict.logistics.pcba.panel.category.124", "zh-HK", "jack a_hk", "pcba板位类别.jack a"),

            // dict.logistics.pcba.panel.category.125
            ("dict.logistics.pcba.panel.category.125", "en-US", "jack b_us", "pcba板位类别.jack b"),
            // dict.logistics.pcba.panel.category.125
            ("dict.logistics.pcba.panel.category.125", "ja-JP", "jack b_jp", "pcba板位类别.jack b"),
            // dict.logistics.pcba.panel.category.125
            ("dict.logistics.pcba.panel.category.125", "zh-CN", "jack b", "pcba板位类别.jack b"),
            // dict.logistics.pcba.panel.category.125
            ("dict.logistics.pcba.panel.category.125", "zh-HK", "jack b_hk", "pcba板位类别.jack b"),

            // dict.logistics.pcba.panel.category.126
            ("dict.logistics.pcba.panel.category.126", "en-US", "jack b/t_us", "pcba板位类别.jack b/t"),
            // dict.logistics.pcba.panel.category.126
            ("dict.logistics.pcba.panel.category.126", "ja-JP", "jack b/t_jp", "pcba板位类别.jack b/t"),
            // dict.logistics.pcba.panel.category.126
            ("dict.logistics.pcba.panel.category.126", "zh-CN", "jack b/t", "pcba板位类别.jack b/t"),
            // dict.logistics.pcba.panel.category.126
            ("dict.logistics.pcba.panel.category.126", "zh-HK", "jack b/t_hk", "pcba板位类别.jack b/t"),

            // dict.logistics.pcba.panel.category.127
            ("dict.logistics.pcba.panel.category.127", "en-US", "jack t_us", "pcba板位类别.jack t"),
            // dict.logistics.pcba.panel.category.127
            ("dict.logistics.pcba.panel.category.127", "ja-JP", "jack t_jp", "pcba板位类别.jack t"),
            // dict.logistics.pcba.panel.category.127
            ("dict.logistics.pcba.panel.category.127", "zh-CN", "jack t", "pcba板位类别.jack t"),
            // dict.logistics.pcba.panel.category.127
            ("dict.logistics.pcba.panel.category.127", "zh-HK", "jack t_hk", "pcba板位类别.jack t"),

            // dict.logistics.pcba.panel.category.128
            ("dict.logistics.pcba.panel.category.128", "en-US", "jack-00 b_us", "pcba板位类别.jack-00 b"),
            // dict.logistics.pcba.panel.category.128
            ("dict.logistics.pcba.panel.category.128", "ja-JP", "jack-00 b_jp", "pcba板位类别.jack-00 b"),
            // dict.logistics.pcba.panel.category.128
            ("dict.logistics.pcba.panel.category.128", "zh-CN", "jack-00 b", "pcba板位类别.jack-00 b"),
            // dict.logistics.pcba.panel.category.128
            ("dict.logistics.pcba.panel.category.128", "zh-HK", "jack-00 b_hk", "pcba板位类别.jack-00 b"),

            // dict.logistics.pcba.panel.category.132
            ("dict.logistics.pcba.panel.category.132", "en-US", "jack-00 t_us", "pcba板位类别.jack-00 t"),
            // dict.logistics.pcba.panel.category.132
            ("dict.logistics.pcba.panel.category.132", "ja-JP", "jack-00 t_jp", "pcba板位类别.jack-00 t"),
            // dict.logistics.pcba.panel.category.132
            ("dict.logistics.pcba.panel.category.132", "zh-CN", "jack-00 t", "pcba板位类别.jack-00 t"),
            // dict.logistics.pcba.panel.category.132
            ("dict.logistics.pcba.panel.category.132", "zh-HK", "jack-00 t_hk", "pcba板位类别.jack-00 t"),

            // dict.logistics.pcba.panel.category.133
            ("dict.logistics.pcba.panel.category.133", "en-US", "jack-10 b_us", "pcba板位类别.jack-10 b"),
            // dict.logistics.pcba.panel.category.133
            ("dict.logistics.pcba.panel.category.133", "ja-JP", "jack-10 b_jp", "pcba板位类别.jack-10 b"),
            // dict.logistics.pcba.panel.category.133
            ("dict.logistics.pcba.panel.category.133", "zh-CN", "jack-10 b", "pcba板位类别.jack-10 b"),
            // dict.logistics.pcba.panel.category.133
            ("dict.logistics.pcba.panel.category.133", "zh-HK", "jack-10 b_hk", "pcba板位类别.jack-10 b"),

            // dict.logistics.pcba.panel.category.134
            ("dict.logistics.pcba.panel.category.134", "en-US", "jack-10 t_us", "pcba板位类别.jack-10 t"),
            // dict.logistics.pcba.panel.category.134
            ("dict.logistics.pcba.panel.category.134", "ja-JP", "jack-10 t_jp", "pcba板位类别.jack-10 t"),
            // dict.logistics.pcba.panel.category.134
            ("dict.logistics.pcba.panel.category.134", "zh-CN", "jack-10 t", "pcba板位类别.jack-10 t"),
            // dict.logistics.pcba.panel.category.134
            ("dict.logistics.pcba.panel.category.134", "zh-HK", "jack-10 t_hk", "pcba板位类别.jack-10 t"),

            // dict.logistics.pcba.panel.category.135
            ("dict.logistics.pcba.panel.category.135", "en-US", "jack-20 b_us", "pcba板位类别.jack-20 b"),
            // dict.logistics.pcba.panel.category.135
            ("dict.logistics.pcba.panel.category.135", "ja-JP", "jack-20 b_jp", "pcba板位类别.jack-20 b"),
            // dict.logistics.pcba.panel.category.135
            ("dict.logistics.pcba.panel.category.135", "zh-CN", "jack-20 b", "pcba板位类别.jack-20 b"),
            // dict.logistics.pcba.panel.category.135
            ("dict.logistics.pcba.panel.category.135", "zh-HK", "jack-20 b_hk", "pcba板位类别.jack-20 b"),

            // dict.logistics.pcba.panel.category.136
            ("dict.logistics.pcba.panel.category.136", "en-US", "jack-20 t_us", "pcba板位类别.jack-20 t"),
            // dict.logistics.pcba.panel.category.136
            ("dict.logistics.pcba.panel.category.136", "ja-JP", "jack-20 t_jp", "pcba板位类别.jack-20 t"),
            // dict.logistics.pcba.panel.category.136
            ("dict.logistics.pcba.panel.category.136", "zh-CN", "jack-20 t", "pcba板位类别.jack-20 t"),
            // dict.logistics.pcba.panel.category.136
            ("dict.logistics.pcba.panel.category.136", "zh-HK", "jack-20 t_hk", "pcba板位类别.jack-20 t"),

            // dict.logistics.pcba.panel.category.137
            ("dict.logistics.pcba.panel.category.137", "en-US", "jack-30 b_us", "pcba板位类别.jack-30 b"),
            // dict.logistics.pcba.panel.category.137
            ("dict.logistics.pcba.panel.category.137", "ja-JP", "jack-30 b_jp", "pcba板位类别.jack-30 b"),
            // dict.logistics.pcba.panel.category.137
            ("dict.logistics.pcba.panel.category.137", "zh-CN", "jack-30 b", "pcba板位类别.jack-30 b"),
            // dict.logistics.pcba.panel.category.137
            ("dict.logistics.pcba.panel.category.137", "zh-HK", "jack-30 b_hk", "pcba板位类别.jack-30 b"),

            // dict.logistics.pcba.panel.category.138
            ("dict.logistics.pcba.panel.category.138", "en-US", "jack-30 t_us", "pcba板位类别.jack-30 t"),
            // dict.logistics.pcba.panel.category.138
            ("dict.logistics.pcba.panel.category.138", "ja-JP", "jack-30 t_jp", "pcba板位类别.jack-30 t"),
            // dict.logistics.pcba.panel.category.138
            ("dict.logistics.pcba.panel.category.138", "zh-CN", "jack-30 t", "pcba板位类别.jack-30 t"),
            // dict.logistics.pcba.panel.category.138
            ("dict.logistics.pcba.panel.category.138", "zh-HK", "jack-30 t_hk", "pcba板位类别.jack-30 t"),

            // dict.logistics.pcba.panel.category.139
            ("dict.logistics.pcba.panel.category.139", "en-US", "join_us", "pcba板位类别.join"),
            // dict.logistics.pcba.panel.category.139
            ("dict.logistics.pcba.panel.category.139", "ja-JP", "join_jp", "pcba板位类别.join"),
            // dict.logistics.pcba.panel.category.139
            ("dict.logistics.pcba.panel.category.139", "zh-CN", "join", "pcba板位类别.join"),
            // dict.logistics.pcba.panel.category.139
            ("dict.logistics.pcba.panel.category.139", "zh-HK", "join_hk", "pcba板位类别.join"),

            // dict.logistics.pcba.panel.category.140
            ("dict.logistics.pcba.panel.category.140", "en-US", "jointc a_us", "pcba板位类别.jointc a"),
            // dict.logistics.pcba.panel.category.140
            ("dict.logistics.pcba.panel.category.140", "ja-JP", "jointc a_jp", "pcba板位类别.jointc a"),
            // dict.logistics.pcba.panel.category.140
            ("dict.logistics.pcba.panel.category.140", "zh-CN", "jointc a", "pcba板位类别.jointc a"),
            // dict.logistics.pcba.panel.category.140
            ("dict.logistics.pcba.panel.category.140", "zh-HK", "jointc a_hk", "pcba板位类别.jointc a"),

            // dict.logistics.pcba.panel.category.141
            ("dict.logistics.pcba.panel.category.141", "en-US", "jointc b_us", "pcba板位类别.jointc b"),
            // dict.logistics.pcba.panel.category.141
            ("dict.logistics.pcba.panel.category.141", "ja-JP", "jointc b_jp", "pcba板位类别.jointc b"),
            // dict.logistics.pcba.panel.category.141
            ("dict.logistics.pcba.panel.category.141", "zh-CN", "jointc b", "pcba板位类别.jointc b"),
            // dict.logistics.pcba.panel.category.141
            ("dict.logistics.pcba.panel.category.141", "zh-HK", "jointc b_hk", "pcba板位类别.jointc b"),

            // dict.logistics.pcba.panel.category.142
            ("dict.logistics.pcba.panel.category.142", "en-US", "jointc t_us", "pcba板位类别.jointc t"),
            // dict.logistics.pcba.panel.category.142
            ("dict.logistics.pcba.panel.category.142", "ja-JP", "jointc t_jp", "pcba板位类别.jointc t"),
            // dict.logistics.pcba.panel.category.142
            ("dict.logistics.pcba.panel.category.142", "zh-CN", "jointc t", "pcba板位类别.jointc t"),
            // dict.logistics.pcba.panel.category.142
            ("dict.logistics.pcba.panel.category.142", "zh-HK", "jointc t_hk", "pcba板位类别.jointc t"),

            // dict.logistics.pcba.panel.category.143
            ("dict.logistics.pcba.panel.category.143", "en-US", "jointf a_us", "pcba板位类别.jointf a"),
            // dict.logistics.pcba.panel.category.143
            ("dict.logistics.pcba.panel.category.143", "ja-JP", "jointf a_jp", "pcba板位类别.jointf a"),
            // dict.logistics.pcba.panel.category.143
            ("dict.logistics.pcba.panel.category.143", "zh-CN", "jointf a", "pcba板位类别.jointf a"),
            // dict.logistics.pcba.panel.category.143
            ("dict.logistics.pcba.panel.category.143", "zh-HK", "jointf a_hk", "pcba板位类别.jointf a"),

            // dict.logistics.pcba.panel.category.144
            ("dict.logistics.pcba.panel.category.144", "en-US", "jointf b_us", "pcba板位类别.jointf b"),
            // dict.logistics.pcba.panel.category.144
            ("dict.logistics.pcba.panel.category.144", "ja-JP", "jointf b_jp", "pcba板位类别.jointf b"),
            // dict.logistics.pcba.panel.category.144
            ("dict.logistics.pcba.panel.category.144", "zh-CN", "jointf b", "pcba板位类别.jointf b"),
            // dict.logistics.pcba.panel.category.144
            ("dict.logistics.pcba.panel.category.144", "zh-HK", "jointf b_hk", "pcba板位类别.jointf b"),

            // dict.logistics.pcba.panel.category.145
            ("dict.logistics.pcba.panel.category.145", "en-US", "jointf t_us", "pcba板位类别.jointf t"),
            // dict.logistics.pcba.panel.category.145
            ("dict.logistics.pcba.panel.category.145", "ja-JP", "jointf t_jp", "pcba板位类别.jointf t"),
            // dict.logistics.pcba.panel.category.145
            ("dict.logistics.pcba.panel.category.145", "zh-CN", "jointf t", "pcba板位类别.jointf t"),
            // dict.logistics.pcba.panel.category.145
            ("dict.logistics.pcba.panel.category.145", "zh-HK", "jointf t_hk", "pcba板位类别.jointf t"),

            // dict.logistics.pcba.panel.category.146
            ("dict.logistics.pcba.panel.category.146", "en-US", "joints_us", "pcba板位类别.joints"),
            // dict.logistics.pcba.panel.category.146
            ("dict.logistics.pcba.panel.category.146", "ja-JP", "joints_jp", "pcba板位类别.joints"),
            // dict.logistics.pcba.panel.category.146
            ("dict.logistics.pcba.panel.category.146", "zh-CN", "joints", "pcba板位类别.joints"),
            // dict.logistics.pcba.panel.category.146
            ("dict.logistics.pcba.panel.category.146", "zh-HK", "joints_hk", "pcba板位类别.joints"),

            // dict.logistics.pcba.panel.category.147
            ("dict.logistics.pcba.panel.category.147", "en-US", "key_us", "pcba板位类别.key"),
            // dict.logistics.pcba.panel.category.147
            ("dict.logistics.pcba.panel.category.147", "ja-JP", "key_jp", "pcba板位类别.key"),
            // dict.logistics.pcba.panel.category.147
            ("dict.logistics.pcba.panel.category.147", "zh-CN", "key", "pcba板位类别.key"),
            // dict.logistics.pcba.panel.category.147
            ("dict.logistics.pcba.panel.category.147", "zh-HK", "key_hk", "pcba板位类别.key"),

            // dict.logistics.pcba.panel.category.148
            ("dict.logistics.pcba.panel.category.148", "en-US", "key b_us", "pcba板位类别.key b"),
            // dict.logistics.pcba.panel.category.148
            ("dict.logistics.pcba.panel.category.148", "ja-JP", "key b_jp", "pcba板位类别.key b"),
            // dict.logistics.pcba.panel.category.148
            ("dict.logistics.pcba.panel.category.148", "zh-CN", "key b", "pcba板位类别.key b"),
            // dict.logistics.pcba.panel.category.148
            ("dict.logistics.pcba.panel.category.148", "zh-HK", "key b_hk", "pcba板位类别.key b"),

            // dict.logistics.pcba.panel.category.149
            ("dict.logistics.pcba.panel.category.149", "en-US", "key b/t_us", "pcba板位类别.key b/t"),
            // dict.logistics.pcba.panel.category.149
            ("dict.logistics.pcba.panel.category.149", "ja-JP", "key b/t_jp", "pcba板位类别.key b/t"),
            // dict.logistics.pcba.panel.category.149
            ("dict.logistics.pcba.panel.category.149", "zh-CN", "key b/t", "pcba板位类别.key b/t"),
            // dict.logistics.pcba.panel.category.149
            ("dict.logistics.pcba.panel.category.149", "zh-HK", "key b/t_hk", "pcba板位类别.key b/t"),

            // dict.logistics.pcba.panel.category.150
            ("dict.logistics.pcba.panel.category.150", "en-US", "key t_us", "pcba板位类别.key t"),
            // dict.logistics.pcba.panel.category.150
            ("dict.logistics.pcba.panel.category.150", "ja-JP", "key t_jp", "pcba板位类别.key t"),
            // dict.logistics.pcba.panel.category.150
            ("dict.logistics.pcba.panel.category.150", "zh-CN", "key t", "pcba板位类别.key t"),
            // dict.logistics.pcba.panel.category.150
            ("dict.logistics.pcba.panel.category.150", "zh-HK", "key t_hk", "pcba板位类别.key t"),

            // dict.logistics.pcba.panel.category.151
            ("dict.logistics.pcba.panel.category.151", "en-US", "lcd a_us", "pcba板位类别.lcd a"),
            // dict.logistics.pcba.panel.category.151
            ("dict.logistics.pcba.panel.category.151", "ja-JP", "lcd a_jp", "pcba板位类别.lcd a"),
            // dict.logistics.pcba.panel.category.151
            ("dict.logistics.pcba.panel.category.151", "zh-CN", "lcd a", "pcba板位类别.lcd a"),
            // dict.logistics.pcba.panel.category.151
            ("dict.logistics.pcba.panel.category.151", "zh-HK", "lcd a_hk", "pcba板位类别.lcd a"),

            // dict.logistics.pcba.panel.category.152
            ("dict.logistics.pcba.panel.category.152", "en-US", "lcd b_us", "pcba板位类别.lcd b"),
            // dict.logistics.pcba.panel.category.152
            ("dict.logistics.pcba.panel.category.152", "ja-JP", "lcd b_jp", "pcba板位类别.lcd b"),
            // dict.logistics.pcba.panel.category.152
            ("dict.logistics.pcba.panel.category.152", "zh-CN", "lcd b", "pcba板位类别.lcd b"),
            // dict.logistics.pcba.panel.category.152
            ("dict.logistics.pcba.panel.category.152", "zh-HK", "lcd b_hk", "pcba板位类别.lcd b"),

            // dict.logistics.pcba.panel.category.153
            ("dict.logistics.pcba.panel.category.153", "en-US", "lcd b/t_us", "pcba板位类别.lcd b/t"),
            // dict.logistics.pcba.panel.category.153
            ("dict.logistics.pcba.panel.category.153", "ja-JP", "lcd b/t_jp", "pcba板位类别.lcd b/t"),
            // dict.logistics.pcba.panel.category.153
            ("dict.logistics.pcba.panel.category.153", "zh-CN", "lcd b/t", "pcba板位类别.lcd b/t"),
            // dict.logistics.pcba.panel.category.153
            ("dict.logistics.pcba.panel.category.153", "zh-HK", "lcd b/t_hk", "pcba板位类别.lcd b/t"),

            // dict.logistics.pcba.panel.category.154
            ("dict.logistics.pcba.panel.category.154", "en-US", "lcd ex_us", "pcba板位类别.lcd ex"),
            // dict.logistics.pcba.panel.category.154
            ("dict.logistics.pcba.panel.category.154", "ja-JP", "lcd ex_jp", "pcba板位类别.lcd ex"),
            // dict.logistics.pcba.panel.category.154
            ("dict.logistics.pcba.panel.category.154", "zh-CN", "lcd ex", "pcba板位类别.lcd ex"),
            // dict.logistics.pcba.panel.category.154
            ("dict.logistics.pcba.panel.category.154", "zh-HK", "lcd ex_hk", "pcba板位类别.lcd ex"),

            // dict.logistics.pcba.panel.category.155
            ("dict.logistics.pcba.panel.category.155", "en-US", "lcd ex b_us", "pcba板位类别.lcd ex b"),
            // dict.logistics.pcba.panel.category.155
            ("dict.logistics.pcba.panel.category.155", "ja-JP", "lcd ex b_jp", "pcba板位类别.lcd ex b"),
            // dict.logistics.pcba.panel.category.155
            ("dict.logistics.pcba.panel.category.155", "zh-CN", "lcd ex b", "pcba板位类别.lcd ex b"),
            // dict.logistics.pcba.panel.category.155
            ("dict.logistics.pcba.panel.category.155", "zh-HK", "lcd ex b_hk", "pcba板位类别.lcd ex b"),

            // dict.logistics.pcba.panel.category.156
            ("dict.logistics.pcba.panel.category.156", "en-US", "lcd ex b/t_us", "pcba板位类别.lcd ex b/t"),
            // dict.logistics.pcba.panel.category.156
            ("dict.logistics.pcba.panel.category.156", "ja-JP", "lcd ex b/t_jp", "pcba板位类别.lcd ex b/t"),
            // dict.logistics.pcba.panel.category.156
            ("dict.logistics.pcba.panel.category.156", "zh-CN", "lcd ex b/t", "pcba板位类别.lcd ex b/t"),
            // dict.logistics.pcba.panel.category.156
            ("dict.logistics.pcba.panel.category.156", "zh-HK", "lcd ex b/t_hk", "pcba板位类别.lcd ex b/t"),

            // dict.logistics.pcba.panel.category.157
            ("dict.logistics.pcba.panel.category.157", "en-US", "lcd ex t_us", "pcba板位类别.lcd ex t"),
            // dict.logistics.pcba.panel.category.157
            ("dict.logistics.pcba.panel.category.157", "ja-JP", "lcd ex t_jp", "pcba板位类别.lcd ex t"),
            // dict.logistics.pcba.panel.category.157
            ("dict.logistics.pcba.panel.category.157", "zh-CN", "lcd ex t", "pcba板位类别.lcd ex t"),
            // dict.logistics.pcba.panel.category.157
            ("dict.logistics.pcba.panel.category.157", "zh-HK", "lcd ex t_hk", "pcba板位类别.lcd ex t"),

            // dict.logistics.pcba.panel.category.158
            ("dict.logistics.pcba.panel.category.158", "en-US", "madi b_us", "pcba板位类别.madi b"),
            // dict.logistics.pcba.panel.category.158
            ("dict.logistics.pcba.panel.category.158", "ja-JP", "madi b_jp", "pcba板位类别.madi b"),
            // dict.logistics.pcba.panel.category.158
            ("dict.logistics.pcba.panel.category.158", "zh-CN", "madi b", "pcba板位类别.madi b"),
            // dict.logistics.pcba.panel.category.158
            ("dict.logistics.pcba.panel.category.158", "zh-HK", "madi b_hk", "pcba板位类别.madi b"),

            // dict.logistics.pcba.panel.category.161
            ("dict.logistics.pcba.panel.category.161", "en-US", "madi b/t_us", "pcba板位类别.madi b/t"),
            // dict.logistics.pcba.panel.category.161
            ("dict.logistics.pcba.panel.category.161", "ja-JP", "madi b/t_jp", "pcba板位类别.madi b/t"),
            // dict.logistics.pcba.panel.category.161
            ("dict.logistics.pcba.panel.category.161", "zh-CN", "madi b/t", "pcba板位类别.madi b/t"),
            // dict.logistics.pcba.panel.category.161
            ("dict.logistics.pcba.panel.category.161", "zh-HK", "madi b/t_hk", "pcba板位类别.madi b/t"),

            // dict.logistics.pcba.panel.category.162
            ("dict.logistics.pcba.panel.category.162", "en-US", "madi t_us", "pcba板位类别.madi t"),
            // dict.logistics.pcba.panel.category.162
            ("dict.logistics.pcba.panel.category.162", "ja-JP", "madi t_jp", "pcba板位类别.madi t"),
            // dict.logistics.pcba.panel.category.162
            ("dict.logistics.pcba.panel.category.162", "zh-CN", "madi t", "pcba板位类别.madi t"),
            // dict.logistics.pcba.panel.category.162
            ("dict.logistics.pcba.panel.category.162", "zh-HK", "madi t_hk", "pcba板位类别.madi t"),

            // dict.logistics.pcba.panel.category.163
            ("dict.logistics.pcba.panel.category.163", "en-US", "mafad a_us", "pcba板位类别.mafad a"),
            // dict.logistics.pcba.panel.category.163
            ("dict.logistics.pcba.panel.category.163", "ja-JP", "mafad a_jp", "pcba板位类别.mafad a"),
            // dict.logistics.pcba.panel.category.163
            ("dict.logistics.pcba.panel.category.163", "zh-CN", "mafad a", "pcba板位类别.mafad a"),
            // dict.logistics.pcba.panel.category.163
            ("dict.logistics.pcba.panel.category.163", "zh-HK", "mafad a_hk", "pcba板位类别.mafad a"),

            // dict.logistics.pcba.panel.category.164
            ("dict.logistics.pcba.panel.category.164", "en-US", "mafad b_us", "pcba板位类别.mafad b"),
            // dict.logistics.pcba.panel.category.164
            ("dict.logistics.pcba.panel.category.164", "ja-JP", "mafad b_jp", "pcba板位类别.mafad b"),
            // dict.logistics.pcba.panel.category.164
            ("dict.logistics.pcba.panel.category.164", "zh-CN", "mafad b", "pcba板位类别.mafad b"),
            // dict.logistics.pcba.panel.category.164
            ("dict.logistics.pcba.panel.category.164", "zh-HK", "mafad b_hk", "pcba板位类别.mafad b"),

            // dict.logistics.pcba.panel.category.165
            ("dict.logistics.pcba.panel.category.165", "en-US", "ma-fad b_us", "pcba板位类别.ma-fad b"),
            // dict.logistics.pcba.panel.category.165
            ("dict.logistics.pcba.panel.category.165", "ja-JP", "ma-fad b_jp", "pcba板位类别.ma-fad b"),
            // dict.logistics.pcba.panel.category.165
            ("dict.logistics.pcba.panel.category.165", "zh-CN", "ma-fad b", "pcba板位类别.ma-fad b"),
            // dict.logistics.pcba.panel.category.165
            ("dict.logistics.pcba.panel.category.165", "zh-HK", "ma-fad b_hk", "pcba板位类别.ma-fad b"),

            // dict.logistics.pcba.panel.category.166
            ("dict.logistics.pcba.panel.category.166", "en-US", "mafad b/t_us", "pcba板位类别.mafad b/t"),
            // dict.logistics.pcba.panel.category.166
            ("dict.logistics.pcba.panel.category.166", "ja-JP", "mafad b/t_jp", "pcba板位类别.mafad b/t"),
            // dict.logistics.pcba.panel.category.166
            ("dict.logistics.pcba.panel.category.166", "zh-CN", "mafad b/t", "pcba板位类别.mafad b/t"),
            // dict.logistics.pcba.panel.category.166
            ("dict.logistics.pcba.panel.category.166", "zh-HK", "mafad b/t_hk", "pcba板位类别.mafad b/t"),

            // dict.logistics.pcba.panel.category.167
            ("dict.logistics.pcba.panel.category.167", "en-US", "ma-fad t_us", "pcba板位类别.ma-fad t"),
            // dict.logistics.pcba.panel.category.167
            ("dict.logistics.pcba.panel.category.167", "ja-JP", "ma-fad t_jp", "pcba板位类别.ma-fad t"),
            // dict.logistics.pcba.panel.category.167
            ("dict.logistics.pcba.panel.category.167", "zh-CN", "ma-fad t", "pcba板位类别.ma-fad t"),
            // dict.logistics.pcba.panel.category.167
            ("dict.logistics.pcba.panel.category.167", "zh-HK", "ma-fad t_hk", "pcba板位类别.ma-fad t"),

            // dict.logistics.pcba.panel.category.168
            ("dict.logistics.pcba.panel.category.168", "en-US", "main_us", "pcba板位类别.main"),
            // dict.logistics.pcba.panel.category.168
            ("dict.logistics.pcba.panel.category.168", "ja-JP", "main_jp", "pcba板位类别.main"),
            // dict.logistics.pcba.panel.category.168
            ("dict.logistics.pcba.panel.category.168", "zh-CN", "main", "pcba板位类别.main"),
            // dict.logistics.pcba.panel.category.168
            ("dict.logistics.pcba.panel.category.168", "zh-HK", "main_hk", "pcba板位类别.main"),

            // dict.logistics.pcba.panel.category.171
            ("dict.logistics.pcba.panel.category.171", "en-US", "main a_us", "pcba板位类别.main a"),
            // dict.logistics.pcba.panel.category.171
            ("dict.logistics.pcba.panel.category.171", "ja-JP", "main a_jp", "pcba板位类别.main a"),
            // dict.logistics.pcba.panel.category.171
            ("dict.logistics.pcba.panel.category.171", "zh-CN", "main a", "pcba板位类别.main a"),
            // dict.logistics.pcba.panel.category.171
            ("dict.logistics.pcba.panel.category.171", "zh-HK", "main a_hk", "pcba板位类别.main a"),

            // dict.logistics.pcba.panel.category.172
            ("dict.logistics.pcba.panel.category.172", "en-US", "main alt b_us", "pcba板位类别.main alt b"),
            // dict.logistics.pcba.panel.category.172
            ("dict.logistics.pcba.panel.category.172", "ja-JP", "main alt b_jp", "pcba板位类别.main alt b"),
            // dict.logistics.pcba.panel.category.172
            ("dict.logistics.pcba.panel.category.172", "zh-CN", "main alt b", "pcba板位类别.main alt b"),
            // dict.logistics.pcba.panel.category.172
            ("dict.logistics.pcba.panel.category.172", "zh-HK", "main alt b_hk", "pcba板位类别.main alt b"),

            // dict.logistics.pcba.panel.category.173
            ("dict.logistics.pcba.panel.category.173", "en-US", "main alt t_us", "pcba板位类别.main alt t"),
            // dict.logistics.pcba.panel.category.173
            ("dict.logistics.pcba.panel.category.173", "ja-JP", "main alt t_jp", "pcba板位类别.main alt t"),
            // dict.logistics.pcba.panel.category.173
            ("dict.logistics.pcba.panel.category.173", "zh-CN", "main alt t", "pcba板位类别.main alt t"),
            // dict.logistics.pcba.panel.category.173
            ("dict.logistics.pcba.panel.category.173", "zh-HK", "main alt t_hk", "pcba板位类别.main alt t"),

            // dict.logistics.pcba.panel.category.174
            ("dict.logistics.pcba.panel.category.174", "en-US", "main b_us", "pcba板位类别.main b"),
            // dict.logistics.pcba.panel.category.174
            ("dict.logistics.pcba.panel.category.174", "ja-JP", "main b_jp", "pcba板位类别.main b"),
            // dict.logistics.pcba.panel.category.174
            ("dict.logistics.pcba.panel.category.174", "zh-CN", "main b", "pcba板位类别.main b"),
            // dict.logistics.pcba.panel.category.174
            ("dict.logistics.pcba.panel.category.174", "zh-HK", "main b_hk", "pcba板位类别.main b"),

            // dict.logistics.pcba.panel.category.175
            ("dict.logistics.pcba.panel.category.175", "en-US", "main b/t_us", "pcba板位类别.main b/t"),
            // dict.logistics.pcba.panel.category.175
            ("dict.logistics.pcba.panel.category.175", "ja-JP", "main b/t_jp", "pcba板位类别.main b/t"),
            // dict.logistics.pcba.panel.category.175
            ("dict.logistics.pcba.panel.category.175", "zh-CN", "main b/t", "pcba板位类别.main b/t"),
            // dict.logistics.pcba.panel.category.175
            ("dict.logistics.pcba.panel.category.175", "zh-HK", "main b/t_hk", "pcba板位类别.main b/t"),

            // dict.logistics.pcba.panel.category.176
            ("dict.logistics.pcba.panel.category.176", "en-US", "mather b/t_us", "pcba板位类别.mather b/t"),
            // dict.logistics.pcba.panel.category.176
            ("dict.logistics.pcba.panel.category.176", "ja-JP", "mather b/t_jp", "pcba板位类别.mather b/t"),
            // dict.logistics.pcba.panel.category.176
            ("dict.logistics.pcba.panel.category.176", "zh-CN", "mather b/t", "pcba板位类别.mather b/t"),
            // dict.logistics.pcba.panel.category.176
            ("dict.logistics.pcba.panel.category.176", "zh-HK", "mather b/t_hk", "pcba板位类别.mather b/t"),

            // dict.logistics.pcba.panel.category.179
            ("dict.logistics.pcba.panel.category.179", "en-US", "meter_us", "pcba板位类别.meter"),
            // dict.logistics.pcba.panel.category.179
            ("dict.logistics.pcba.panel.category.179", "ja-JP", "meter_jp", "pcba板位类别.meter"),
            // dict.logistics.pcba.panel.category.179
            ("dict.logistics.pcba.panel.category.179", "zh-CN", "meter", "pcba板位类别.meter"),
            // dict.logistics.pcba.panel.category.179
            ("dict.logistics.pcba.panel.category.179", "zh-HK", "meter_hk", "pcba板位类别.meter"),

            // dict.logistics.pcba.panel.category.180
            ("dict.logistics.pcba.panel.category.180", "en-US", "mic_us", "pcba板位类别.mic"),
            // dict.logistics.pcba.panel.category.180
            ("dict.logistics.pcba.panel.category.180", "ja-JP", "mic_jp", "pcba板位类别.mic"),
            // dict.logistics.pcba.panel.category.180
            ("dict.logistics.pcba.panel.category.180", "zh-CN", "mic", "pcba板位类别.mic"),
            // dict.logistics.pcba.panel.category.180
            ("dict.logistics.pcba.panel.category.180", "zh-HK", "mic_hk", "pcba板位类别.mic"),

            // dict.logistics.pcba.panel.category.181
            ("dict.logistics.pcba.panel.category.181", "en-US", "naub b_us", "pcba板位类别.naub b"),
            // dict.logistics.pcba.panel.category.181
            ("dict.logistics.pcba.panel.category.181", "ja-JP", "naub b_jp", "pcba板位类别.naub b"),
            // dict.logistics.pcba.panel.category.181
            ("dict.logistics.pcba.panel.category.181", "zh-CN", "naub b", "pcba板位类别.naub b"),
            // dict.logistics.pcba.panel.category.181
            ("dict.logistics.pcba.panel.category.181", "zh-HK", "naub b_hk", "pcba板位类别.naub b"),

            // dict.logistics.pcba.panel.category.182
            ("dict.logistics.pcba.panel.category.182", "en-US", "panel_us", "pcba板位类别.panel"),
            // dict.logistics.pcba.panel.category.182
            ("dict.logistics.pcba.panel.category.182", "ja-JP", "panel_jp", "pcba板位类别.panel"),
            // dict.logistics.pcba.panel.category.182
            ("dict.logistics.pcba.panel.category.182", "zh-CN", "panel", "pcba板位类别.panel"),
            // dict.logistics.pcba.panel.category.182
            ("dict.logistics.pcba.panel.category.182", "zh-HK", "panel_hk", "pcba板位类别.panel"),

            // dict.logistics.pcba.panel.category.183
            ("dict.logistics.pcba.panel.category.183", "en-US", "panel a_us", "pcba板位类别.panel a"),
            // dict.logistics.pcba.panel.category.183
            ("dict.logistics.pcba.panel.category.183", "ja-JP", "panel a_jp", "pcba板位类别.panel a"),
            // dict.logistics.pcba.panel.category.183
            ("dict.logistics.pcba.panel.category.183", "zh-CN", "panel a", "pcba板位类别.panel a"),
            // dict.logistics.pcba.panel.category.183
            ("dict.logistics.pcba.panel.category.183", "zh-HK", "panel a_hk", "pcba板位类别.panel a"),

            // dict.logistics.pcba.panel.category.184
            ("dict.logistics.pcba.panel.category.184", "en-US", "panel b_us", "pcba板位类别.panel b"),
            // dict.logistics.pcba.panel.category.184
            ("dict.logistics.pcba.panel.category.184", "ja-JP", "panel b_jp", "pcba板位类别.panel b"),
            // dict.logistics.pcba.panel.category.184
            ("dict.logistics.pcba.panel.category.184", "zh-CN", "panel b", "pcba板位类别.panel b"),
            // dict.logistics.pcba.panel.category.184
            ("dict.logistics.pcba.panel.category.184", "zh-HK", "panel b_hk", "pcba板位类别.panel b"),

            // dict.logistics.pcba.panel.category.185
            ("dict.logistics.pcba.panel.category.185", "en-US", "panel b/t_us", "pcba板位类别.panel b/t"),
            // dict.logistics.pcba.panel.category.185
            ("dict.logistics.pcba.panel.category.185", "ja-JP", "panel b/t_jp", "pcba板位类别.panel b/t"),
            // dict.logistics.pcba.panel.category.185
            ("dict.logistics.pcba.panel.category.185", "zh-CN", "panel b/t", "pcba板位类别.panel b/t"),
            // dict.logistics.pcba.panel.category.185
            ("dict.logistics.pcba.panel.category.185", "zh-HK", "panel b/t_hk", "pcba板位类别.panel b/t"),

            // dict.logistics.pcba.panel.category.186
            ("dict.logistics.pcba.panel.category.186", "en-US", "panel l_us", "pcba板位类别.panel l"),
            // dict.logistics.pcba.panel.category.186
            ("dict.logistics.pcba.panel.category.186", "ja-JP", "panel l_jp", "pcba板位类别.panel l"),
            // dict.logistics.pcba.panel.category.186
            ("dict.logistics.pcba.panel.category.186", "zh-CN", "panel l", "pcba板位类别.panel l"),
            // dict.logistics.pcba.panel.category.186
            ("dict.logistics.pcba.panel.category.186", "zh-HK", "panel l_hk", "pcba板位类别.panel l"),

            // dict.logistics.pcba.panel.category.187
            ("dict.logistics.pcba.panel.category.187", "en-US", "panel r_us", "pcba板位类别.panel r"),
            // dict.logistics.pcba.panel.category.187
            ("dict.logistics.pcba.panel.category.187", "ja-JP", "panel r_jp", "pcba板位类别.panel r"),
            // dict.logistics.pcba.panel.category.187
            ("dict.logistics.pcba.panel.category.187", "zh-CN", "panel r", "pcba板位类别.panel r"),
            // dict.logistics.pcba.panel.category.187
            ("dict.logistics.pcba.panel.category.187", "zh-HK", "panel r_hk", "pcba板位类别.panel r"),

            // dict.logistics.pcba.panel.category.188
            ("dict.logistics.pcba.panel.category.188", "en-US", "panel t_us", "pcba板位类别.panel t"),
            // dict.logistics.pcba.panel.category.188
            ("dict.logistics.pcba.panel.category.188", "ja-JP", "panel t_jp", "pcba板位类别.panel t"),
            // dict.logistics.pcba.panel.category.188
            ("dict.logistics.pcba.panel.category.188", "zh-CN", "panel t", "pcba板位类别.panel t"),
            // dict.logistics.pcba.panel.category.188
            ("dict.logistics.pcba.panel.category.188", "zh-HK", "panel t_hk", "pcba板位类别.panel t"),

            // dict.logistics.pcba.panel.category.189
            ("dict.logistics.pcba.panel.category.189", "en-US", "phone_us", "pcba板位类别.phone"),
            // dict.logistics.pcba.panel.category.189
            ("dict.logistics.pcba.panel.category.189", "ja-JP", "phone_jp", "pcba板位类别.phone"),
            // dict.logistics.pcba.panel.category.189
            ("dict.logistics.pcba.panel.category.189", "zh-CN", "phone", "pcba板位类别.phone"),
            // dict.logistics.pcba.panel.category.189
            ("dict.logistics.pcba.panel.category.189", "zh-HK", "phone_hk", "pcba板位类别.phone"),

            // dict.logistics.pcba.panel.category.190
            ("dict.logistics.pcba.panel.category.190", "en-US", "power_us", "pcba板位类别.power"),
            // dict.logistics.pcba.panel.category.190
            ("dict.logistics.pcba.panel.category.190", "ja-JP", "power_jp", "pcba板位类别.power"),
            // dict.logistics.pcba.panel.category.190
            ("dict.logistics.pcba.panel.category.190", "zh-CN", "power", "pcba板位类别.power"),
            // dict.logistics.pcba.panel.category.190
            ("dict.logistics.pcba.panel.category.190", "zh-HK", "power_hk", "pcba板位类别.power"),

            // dict.logistics.pcba.panel.category.191
            ("dict.logistics.pcba.panel.category.191", "en-US", "power a_us", "pcba板位类别.power a"),
            // dict.logistics.pcba.panel.category.191
            ("dict.logistics.pcba.panel.category.191", "ja-JP", "power a_jp", "pcba板位类别.power a"),
            // dict.logistics.pcba.panel.category.191
            ("dict.logistics.pcba.panel.category.191", "zh-CN", "power a", "pcba板位类别.power a"),
            // dict.logistics.pcba.panel.category.191
            ("dict.logistics.pcba.panel.category.191", "zh-HK", "power a_hk", "pcba板位类别.power a"),

            // dict.logistics.pcba.panel.category.192
            ("dict.logistics.pcba.panel.category.192", "en-US", "power b_us", "pcba板位类别.power b"),
            // dict.logistics.pcba.panel.category.192
            ("dict.logistics.pcba.panel.category.192", "ja-JP", "power b_jp", "pcba板位类别.power b"),
            // dict.logistics.pcba.panel.category.192
            ("dict.logistics.pcba.panel.category.192", "zh-CN", "power b", "pcba板位类别.power b"),
            // dict.logistics.pcba.panel.category.192
            ("dict.logistics.pcba.panel.category.192", "zh-HK", "power b_hk", "pcba板位类别.power b"),

            // dict.logistics.pcba.panel.category.193
            ("dict.logistics.pcba.panel.category.193", "en-US", "power b/t_us", "pcba板位类别.power b/t"),
            // dict.logistics.pcba.panel.category.193
            ("dict.logistics.pcba.panel.category.193", "ja-JP", "power b/t_jp", "pcba板位类别.power b/t"),
            // dict.logistics.pcba.panel.category.193
            ("dict.logistics.pcba.panel.category.193", "zh-CN", "power b/t", "pcba板位类别.power b/t"),
            // dict.logistics.pcba.panel.category.193
            ("dict.logistics.pcba.panel.category.193", "zh-HK", "power b/t_hk", "pcba板位类别.power b/t"),

            // dict.logistics.pcba.panel.category.194
            ("dict.logistics.pcba.panel.category.194", "en-US", "power t_us", "pcba板位类别.power t"),
            // dict.logistics.pcba.panel.category.194
            ("dict.logistics.pcba.panel.category.194", "ja-JP", "power t_jp", "pcba板位类别.power t"),
            // dict.logistics.pcba.panel.category.194
            ("dict.logistics.pcba.panel.category.194", "zh-CN", "power t", "pcba板位类别.power t"),
            // dict.logistics.pcba.panel.category.194
            ("dict.logistics.pcba.panel.category.194", "zh-HK", "power t_hk", "pcba板位类别.power t"),

            // dict.logistics.pcba.panel.category.195
            ("dict.logistics.pcba.panel.category.195", "en-US", "prm b_us", "pcba板位类别.prm b"),
            // dict.logistics.pcba.panel.category.195
            ("dict.logistics.pcba.panel.category.195", "ja-JP", "prm b_jp", "pcba板位类别.prm b"),
            // dict.logistics.pcba.panel.category.195
            ("dict.logistics.pcba.panel.category.195", "zh-CN", "prm b", "pcba板位类别.prm b"),
            // dict.logistics.pcba.panel.category.195
            ("dict.logistics.pcba.panel.category.195", "zh-HK", "prm b_hk", "pcba板位类别.prm b"),

            // dict.logistics.pcba.panel.category.196
            ("dict.logistics.pcba.panel.category.196", "en-US", "prm b/t_us", "pcba板位类别.prm b/t"),
            // dict.logistics.pcba.panel.category.196
            ("dict.logistics.pcba.panel.category.196", "ja-JP", "prm b/t_jp", "pcba板位类别.prm b/t"),
            // dict.logistics.pcba.panel.category.196
            ("dict.logistics.pcba.panel.category.196", "zh-CN", "prm b/t", "pcba板位类别.prm b/t"),
            // dict.logistics.pcba.panel.category.196
            ("dict.logistics.pcba.panel.category.196", "zh-HK", "prm b/t_hk", "pcba板位类别.prm b/t"),

            // dict.logistics.pcba.panel.category.197
            ("dict.logistics.pcba.panel.category.197", "en-US", "prm t_us", "pcba板位类别.prm t"),
            // dict.logistics.pcba.panel.category.197
            ("dict.logistics.pcba.panel.category.197", "ja-JP", "prm t_jp", "pcba板位类别.prm t"),
            // dict.logistics.pcba.panel.category.197
            ("dict.logistics.pcba.panel.category.197", "zh-CN", "prm t", "pcba板位类别.prm t"),
            // dict.logistics.pcba.panel.category.197
            ("dict.logistics.pcba.panel.category.197", "zh-HK", "prm t_hk", "pcba板位类别.prm t"),

            // dict.logistics.pcba.panel.category.198
            ("dict.logistics.pcba.panel.category.198", "en-US", "psl_us", "pcba板位类别.psl"),
            // dict.logistics.pcba.panel.category.198
            ("dict.logistics.pcba.panel.category.198", "ja-JP", "psl_jp", "pcba板位类别.psl"),
            // dict.logistics.pcba.panel.category.198
            ("dict.logistics.pcba.panel.category.198", "zh-CN", "psl", "pcba板位类别.psl"),
            // dict.logistics.pcba.panel.category.198
            ("dict.logistics.pcba.panel.category.198", "zh-HK", "psl_hk", "pcba板位类别.psl"),

            // dict.logistics.pcba.panel.category.199
            ("dict.logistics.pcba.panel.category.199", "en-US", "psl b_us", "pcba板位类别.psl b"),
            // dict.logistics.pcba.panel.category.199
            ("dict.logistics.pcba.panel.category.199", "ja-JP", "psl b_jp", "pcba板位类别.psl b"),
            // dict.logistics.pcba.panel.category.199
            ("dict.logistics.pcba.panel.category.199", "zh-CN", "psl b", "pcba板位类别.psl b"),
            // dict.logistics.pcba.panel.category.199
            ("dict.logistics.pcba.panel.category.199", "zh-HK", "psl b_hk", "pcba板位类别.psl b"),

            // dict.logistics.pcba.panel.category.200
            ("dict.logistics.pcba.panel.category.200", "en-US", "psl b/t_us", "pcba板位类别.psl b/t"),
            // dict.logistics.pcba.panel.category.200
            ("dict.logistics.pcba.panel.category.200", "ja-JP", "psl b/t_jp", "pcba板位类别.psl b/t"),
            // dict.logistics.pcba.panel.category.200
            ("dict.logistics.pcba.panel.category.200", "zh-CN", "psl b/t", "pcba板位类别.psl b/t"),
            // dict.logistics.pcba.panel.category.200
            ("dict.logistics.pcba.panel.category.200", "zh-HK", "psl b/t_hk", "pcba板位类别.psl b/t"),

            // dict.logistics.pcba.panel.category.201
            ("dict.logistics.pcba.panel.category.201", "en-US", "psl t_us", "pcba板位类别.psl t"),
            // dict.logistics.pcba.panel.category.201
            ("dict.logistics.pcba.panel.category.201", "ja-JP", "psl t_jp", "pcba板位类别.psl t"),
            // dict.logistics.pcba.panel.category.201
            ("dict.logistics.pcba.panel.category.201", "zh-CN", "psl t", "pcba板位类别.psl t"),
            // dict.logistics.pcba.panel.category.201
            ("dict.logistics.pcba.panel.category.201", "zh-HK", "psl t_hk", "pcba板位类别.psl t"),

            // dict.logistics.pcba.panel.category.202
            ("dict.logistics.pcba.panel.category.202", "en-US", "ptst_us", "pcba板位类别.ptst"),
            // dict.logistics.pcba.panel.category.202
            ("dict.logistics.pcba.panel.category.202", "ja-JP", "ptst_jp", "pcba板位类别.ptst"),
            // dict.logistics.pcba.panel.category.202
            ("dict.logistics.pcba.panel.category.202", "zh-CN", "ptst", "pcba板位类别.ptst"),
            // dict.logistics.pcba.panel.category.202
            ("dict.logistics.pcba.panel.category.202", "zh-HK", "ptst_hk", "pcba板位类别.ptst"),

            // dict.logistics.pcba.panel.category.203
            ("dict.logistics.pcba.panel.category.203", "en-US", "ptst b_us", "pcba板位类别.ptst b"),
            // dict.logistics.pcba.panel.category.203
            ("dict.logistics.pcba.panel.category.203", "ja-JP", "ptst b_jp", "pcba板位类别.ptst b"),
            // dict.logistics.pcba.panel.category.203
            ("dict.logistics.pcba.panel.category.203", "zh-CN", "ptst b", "pcba板位类别.ptst b"),
            // dict.logistics.pcba.panel.category.203
            ("dict.logistics.pcba.panel.category.203", "zh-HK", "ptst b_hk", "pcba板位类别.ptst b"),

            // dict.logistics.pcba.panel.category.204
            ("dict.logistics.pcba.panel.category.204", "en-US", "ptst b/t_us", "pcba板位类别.ptst b/t"),
            // dict.logistics.pcba.panel.category.204
            ("dict.logistics.pcba.panel.category.204", "ja-JP", "ptst b/t_jp", "pcba板位类别.ptst b/t"),
            // dict.logistics.pcba.panel.category.204
            ("dict.logistics.pcba.panel.category.204", "zh-CN", "ptst b/t", "pcba板位类别.ptst b/t"),
            // dict.logistics.pcba.panel.category.204
            ("dict.logistics.pcba.panel.category.204", "zh-HK", "ptst b/t_hk", "pcba板位类别.ptst b/t"),

            // dict.logistics.pcba.panel.category.205
            ("dict.logistics.pcba.panel.category.205", "en-US", "ptst t_us", "pcba板位类别.ptst t"),
            // dict.logistics.pcba.panel.category.205
            ("dict.logistics.pcba.panel.category.205", "ja-JP", "ptst t_jp", "pcba板位类别.ptst t"),
            // dict.logistics.pcba.panel.category.205
            ("dict.logistics.pcba.panel.category.205", "zh-CN", "ptst t", "pcba板位类别.ptst t"),
            // dict.logistics.pcba.panel.category.205
            ("dict.logistics.pcba.panel.category.205", "zh-HK", "ptst t_hk", "pcba板位类别.ptst t"),

            // dict.logistics.pcba.panel.category.206
            ("dict.logistics.pcba.panel.category.206", "en-US", "pwrsub_us", "pcba板位类别.pwrsub"),
            // dict.logistics.pcba.panel.category.206
            ("dict.logistics.pcba.panel.category.206", "ja-JP", "pwrsub_jp", "pcba板位类别.pwrsub"),
            // dict.logistics.pcba.panel.category.206
            ("dict.logistics.pcba.panel.category.206", "zh-CN", "pwrsub", "pcba板位类别.pwrsub"),
            // dict.logistics.pcba.panel.category.206
            ("dict.logistics.pcba.panel.category.206", "zh-HK", "pwrsub_hk", "pcba板位类别.pwrsub"),

            // dict.logistics.pcba.panel.category.207
            ("dict.logistics.pcba.panel.category.207", "en-US", "rear_us", "pcba板位类别.rear"),
            // dict.logistics.pcba.panel.category.207
            ("dict.logistics.pcba.panel.category.207", "ja-JP", "rear_jp", "pcba板位类别.rear"),
            // dict.logistics.pcba.panel.category.207
            ("dict.logistics.pcba.panel.category.207", "zh-CN", "rear", "pcba板位类别.rear"),
            // dict.logistics.pcba.panel.category.207
            ("dict.logistics.pcba.panel.category.207", "zh-HK", "rear_hk", "pcba板位类别.rear"),

            // dict.logistics.pcba.panel.category.208
            ("dict.logistics.pcba.panel.category.208", "en-US", "rear a_us", "pcba板位类别.rear a"),
            // dict.logistics.pcba.panel.category.208
            ("dict.logistics.pcba.panel.category.208", "ja-JP", "rear a_jp", "pcba板位类别.rear a"),
            // dict.logistics.pcba.panel.category.208
            ("dict.logistics.pcba.panel.category.208", "zh-CN", "rear a", "pcba板位类别.rear a"),
            // dict.logistics.pcba.panel.category.208
            ("dict.logistics.pcba.panel.category.208", "zh-HK", "rear a_hk", "pcba板位类别.rear a"),

            // dict.logistics.pcba.panel.category.209
            ("dict.logistics.pcba.panel.category.209", "en-US", "rear b_us", "pcba板位类别.rear b"),
            // dict.logistics.pcba.panel.category.209
            ("dict.logistics.pcba.panel.category.209", "ja-JP", "rear b_jp", "pcba板位类别.rear b"),
            // dict.logistics.pcba.panel.category.209
            ("dict.logistics.pcba.panel.category.209", "zh-CN", "rear b", "pcba板位类别.rear b"),
            // dict.logistics.pcba.panel.category.209
            ("dict.logistics.pcba.panel.category.209", "zh-HK", "rear b_hk", "pcba板位类别.rear b"),

            // dict.logistics.pcba.panel.category.210
            ("dict.logistics.pcba.panel.category.210", "en-US", "rear t_us", "pcba板位类别.rear t"),
            // dict.logistics.pcba.panel.category.210
            ("dict.logistics.pcba.panel.category.210", "ja-JP", "rear t_jp", "pcba板位类别.rear t"),
            // dict.logistics.pcba.panel.category.210
            ("dict.logistics.pcba.panel.category.210", "zh-CN", "rear t", "pcba板位类别.rear t"),
            // dict.logistics.pcba.panel.category.210
            ("dict.logistics.pcba.panel.category.210", "zh-HK", "rear t_hk", "pcba板位类别.rear t"),

            // dict.logistics.pcba.panel.category.211
            ("dict.logistics.pcba.panel.category.211", "en-US", "relay_us", "pcba板位类别.relay"),
            // dict.logistics.pcba.panel.category.211
            ("dict.logistics.pcba.panel.category.211", "ja-JP", "relay_jp", "pcba板位类别.relay"),
            // dict.logistics.pcba.panel.category.211
            ("dict.logistics.pcba.panel.category.211", "zh-CN", "relay", "pcba板位类别.relay"),
            // dict.logistics.pcba.panel.category.211
            ("dict.logistics.pcba.panel.category.211", "zh-HK", "relay_hk", "pcba板位类别.relay"),

            // dict.logistics.pcba.panel.category.212
            ("dict.logistics.pcba.panel.category.212", "en-US", "rfp a_us", "pcba板位类别.rfp a"),
            // dict.logistics.pcba.panel.category.212
            ("dict.logistics.pcba.panel.category.212", "ja-JP", "rfp a_jp", "pcba板位类别.rfp a"),
            // dict.logistics.pcba.panel.category.212
            ("dict.logistics.pcba.panel.category.212", "zh-CN", "rfp a", "pcba板位类别.rfp a"),
            // dict.logistics.pcba.panel.category.212
            ("dict.logistics.pcba.panel.category.212", "zh-HK", "rfp a_hk", "pcba板位类别.rfp a"),

            // dict.logistics.pcba.panel.category.213
            ("dict.logistics.pcba.panel.category.213", "en-US", "rfp b_us", "pcba板位类别.rfp b"),
            // dict.logistics.pcba.panel.category.213
            ("dict.logistics.pcba.panel.category.213", "ja-JP", "rfp b_jp", "pcba板位类别.rfp b"),
            // dict.logistics.pcba.panel.category.213
            ("dict.logistics.pcba.panel.category.213", "zh-CN", "rfp b", "pcba板位类别.rfp b"),
            // dict.logistics.pcba.panel.category.213
            ("dict.logistics.pcba.panel.category.213", "zh-HK", "rfp b_hk", "pcba板位类别.rfp b"),

            // dict.logistics.pcba.panel.category.214
            ("dict.logistics.pcba.panel.category.214", "en-US", "rfp b/t_us", "pcba板位类别.rfp b/t"),
            // dict.logistics.pcba.panel.category.214
            ("dict.logistics.pcba.panel.category.214", "ja-JP", "rfp b/t_jp", "pcba板位类别.rfp b/t"),
            // dict.logistics.pcba.panel.category.214
            ("dict.logistics.pcba.panel.category.214", "zh-CN", "rfp b/t", "pcba板位类别.rfp b/t"),
            // dict.logistics.pcba.panel.category.214
            ("dict.logistics.pcba.panel.category.214", "zh-HK", "rfp b/t_hk", "pcba板位类别.rfp b/t"),

            // dict.logistics.pcba.panel.category.215
            ("dict.logistics.pcba.panel.category.215", "en-US", "rfp t_us", "pcba板位类别.rfp t"),
            // dict.logistics.pcba.panel.category.215
            ("dict.logistics.pcba.panel.category.215", "ja-JP", "rfp t_jp", "pcba板位类别.rfp t"),
            // dict.logistics.pcba.panel.category.215
            ("dict.logistics.pcba.panel.category.215", "zh-CN", "rfp t", "pcba板位类别.rfp t"),
            // dict.logistics.pcba.panel.category.215
            ("dict.logistics.pcba.panel.category.215", "zh-HK", "rfp t_hk", "pcba板位类别.rfp t"),

            // dict.logistics.pcba.panel.category.216
            ("dict.logistics.pcba.panel.category.216", "en-US", "rmn b_us", "pcba板位类别.rmn b"),
            // dict.logistics.pcba.panel.category.216
            ("dict.logistics.pcba.panel.category.216", "ja-JP", "rmn b_jp", "pcba板位类别.rmn b"),
            // dict.logistics.pcba.panel.category.216
            ("dict.logistics.pcba.panel.category.216", "zh-CN", "rmn b", "pcba板位类别.rmn b"),
            // dict.logistics.pcba.panel.category.216
            ("dict.logistics.pcba.panel.category.216", "zh-HK", "rmn b_hk", "pcba板位类别.rmn b"),

            // dict.logistics.pcba.panel.category.217
            ("dict.logistics.pcba.panel.category.217", "en-US", "rmn b/t_us", "pcba板位类别.rmn b/t"),
            // dict.logistics.pcba.panel.category.217
            ("dict.logistics.pcba.panel.category.217", "ja-JP", "rmn b/t_jp", "pcba板位类别.rmn b/t"),
            // dict.logistics.pcba.panel.category.217
            ("dict.logistics.pcba.panel.category.217", "zh-CN", "rmn b/t", "pcba板位类别.rmn b/t"),
            // dict.logistics.pcba.panel.category.217
            ("dict.logistics.pcba.panel.category.217", "zh-HK", "rmn b/t_hk", "pcba板位类别.rmn b/t"),

            // dict.logistics.pcba.panel.category.218
            ("dict.logistics.pcba.panel.category.218", "en-US", "rmn t_us", "pcba板位类别.rmn t"),
            // dict.logistics.pcba.panel.category.218
            ("dict.logistics.pcba.panel.category.218", "ja-JP", "rmn t_jp", "pcba板位类别.rmn t"),
            // dict.logistics.pcba.panel.category.218
            ("dict.logistics.pcba.panel.category.218", "zh-CN", "rmn t", "pcba板位类别.rmn t"),
            // dict.logistics.pcba.panel.category.218
            ("dict.logistics.pcba.panel.category.218", "zh-HK", "rmn t_hk", "pcba板位类别.rmn t"),

            // dict.logistics.pcba.panel.category.219
            ("dict.logistics.pcba.panel.category.219", "en-US", "rmt_us", "pcba板位类别.rmt"),
            // dict.logistics.pcba.panel.category.219
            ("dict.logistics.pcba.panel.category.219", "ja-JP", "rmt_jp", "pcba板位类别.rmt"),
            // dict.logistics.pcba.panel.category.219
            ("dict.logistics.pcba.panel.category.219", "zh-CN", "rmt", "pcba板位类别.rmt"),
            // dict.logistics.pcba.panel.category.219
            ("dict.logistics.pcba.panel.category.219", "zh-HK", "rmt_hk", "pcba板位类别.rmt"),

            // dict.logistics.pcba.panel.category.220
            ("dict.logistics.pcba.panel.category.220", "en-US", "rsb b_us", "pcba板位类别.rsb b"),
            // dict.logistics.pcba.panel.category.220
            ("dict.logistics.pcba.panel.category.220", "ja-JP", "rsb b_jp", "pcba板位类别.rsb b"),
            // dict.logistics.pcba.panel.category.220
            ("dict.logistics.pcba.panel.category.220", "zh-CN", "rsb b", "pcba板位类别.rsb b"),
            // dict.logistics.pcba.panel.category.220
            ("dict.logistics.pcba.panel.category.220", "zh-HK", "rsb b_hk", "pcba板位类别.rsb b"),

            // dict.logistics.pcba.panel.category.221
            ("dict.logistics.pcba.panel.category.221", "en-US", "rsb b/t_us", "pcba板位类别.rsb b/t"),
            // dict.logistics.pcba.panel.category.221
            ("dict.logistics.pcba.panel.category.221", "ja-JP", "rsb b/t_jp", "pcba板位类别.rsb b/t"),
            // dict.logistics.pcba.panel.category.221
            ("dict.logistics.pcba.panel.category.221", "zh-CN", "rsb b/t", "pcba板位类别.rsb b/t"),
            // dict.logistics.pcba.panel.category.221
            ("dict.logistics.pcba.panel.category.221", "zh-HK", "rsb b/t_hk", "pcba板位类别.rsb b/t"),

            // dict.logistics.pcba.panel.category.222
            ("dict.logistics.pcba.panel.category.222", "en-US", "rsb t_us", "pcba板位类别.rsb t"),
            // dict.logistics.pcba.panel.category.222
            ("dict.logistics.pcba.panel.category.222", "ja-JP", "rsb t_jp", "pcba板位类别.rsb t"),
            // dict.logistics.pcba.panel.category.222
            ("dict.logistics.pcba.panel.category.222", "zh-CN", "rsb t", "pcba板位类别.rsb t"),
            // dict.logistics.pcba.panel.category.222
            ("dict.logistics.pcba.panel.category.222", "zh-HK", "rsb t_hk", "pcba板位类别.rsb t"),

            // dict.logistics.pcba.panel.category.223
            ("dict.logistics.pcba.panel.category.223", "en-US", "sata_us", "pcba板位类别.sata"),
            // dict.logistics.pcba.panel.category.223
            ("dict.logistics.pcba.panel.category.223", "ja-JP", "sata_jp", "pcba板位类别.sata"),
            // dict.logistics.pcba.panel.category.223
            ("dict.logistics.pcba.panel.category.223", "zh-CN", "sata", "pcba板位类别.sata"),
            // dict.logistics.pcba.panel.category.223
            ("dict.logistics.pcba.panel.category.223", "zh-HK", "sata_hk", "pcba板位类别.sata"),

            // dict.logistics.pcba.panel.category.224
            ("dict.logistics.pcba.panel.category.224", "en-US", "sbty_us", "pcba板位类别.sbty"),
            // dict.logistics.pcba.panel.category.224
            ("dict.logistics.pcba.panel.category.224", "ja-JP", "sbty_jp", "pcba板位类别.sbty"),
            // dict.logistics.pcba.panel.category.224
            ("dict.logistics.pcba.panel.category.224", "zh-CN", "sbty", "pcba板位类别.sbty"),
            // dict.logistics.pcba.panel.category.224
            ("dict.logistics.pcba.panel.category.224", "zh-HK", "sbty_hk", "pcba板位类别.sbty"),

            // dict.logistics.pcba.panel.category.225
            ("dict.logistics.pcba.panel.category.225", "en-US", "seq_us", "pcba板位类别.seq"),
            // dict.logistics.pcba.panel.category.225
            ("dict.logistics.pcba.panel.category.225", "ja-JP", "seq_jp", "pcba板位类别.seq"),
            // dict.logistics.pcba.panel.category.225
            ("dict.logistics.pcba.panel.category.225", "zh-CN", "seq", "pcba板位类别.seq"),
            // dict.logistics.pcba.panel.category.225
            ("dict.logistics.pcba.panel.category.225", "zh-HK", "seq_hk", "pcba板位类别.seq"),

            // dict.logistics.pcba.panel.category.226
            ("dict.logistics.pcba.panel.category.226", "en-US", "slot_us", "pcba板位类别.slot"),
            // dict.logistics.pcba.panel.category.226
            ("dict.logistics.pcba.panel.category.226", "ja-JP", "slot_jp", "pcba板位类别.slot"),
            // dict.logistics.pcba.panel.category.226
            ("dict.logistics.pcba.panel.category.226", "zh-CN", "slot", "pcba板位类别.slot"),
            // dict.logistics.pcba.panel.category.226
            ("dict.logistics.pcba.panel.category.226", "zh-HK", "slot_hk", "pcba板位类别.slot"),

            // dict.logistics.pcba.panel.category.227
            ("dict.logistics.pcba.panel.category.227", "en-US", "slot a_us", "pcba板位类别.slot a"),
            // dict.logistics.pcba.panel.category.227
            ("dict.logistics.pcba.panel.category.227", "ja-JP", "slot a_jp", "pcba板位类别.slot a"),
            // dict.logistics.pcba.panel.category.227
            ("dict.logistics.pcba.panel.category.227", "zh-CN", "slot a", "pcba板位类别.slot a"),
            // dict.logistics.pcba.panel.category.227
            ("dict.logistics.pcba.panel.category.227", "zh-HK", "slot a_hk", "pcba板位类别.slot a"),

            // dict.logistics.pcba.panel.category.228
            ("dict.logistics.pcba.panel.category.228", "en-US", "slot b_us", "pcba板位类别.slot b"),
            // dict.logistics.pcba.panel.category.228
            ("dict.logistics.pcba.panel.category.228", "ja-JP", "slot b_jp", "pcba板位类别.slot b"),
            // dict.logistics.pcba.panel.category.228
            ("dict.logistics.pcba.panel.category.228", "zh-CN", "slot b", "pcba板位类别.slot b"),
            // dict.logistics.pcba.panel.category.228
            ("dict.logistics.pcba.panel.category.228", "zh-HK", "slot b_hk", "pcba板位类别.slot b"),

            // dict.logistics.pcba.panel.category.229
            ("dict.logistics.pcba.panel.category.229", "en-US", "slot b/t_us", "pcba板位类别.slot b/t"),
            // dict.logistics.pcba.panel.category.229
            ("dict.logistics.pcba.panel.category.229", "ja-JP", "slot b/t_jp", "pcba板位类别.slot b/t"),
            // dict.logistics.pcba.panel.category.229
            ("dict.logistics.pcba.panel.category.229", "zh-CN", "slot b/t", "pcba板位类别.slot b/t"),
            // dict.logistics.pcba.panel.category.229
            ("dict.logistics.pcba.panel.category.229", "zh-HK", "slot b/t_hk", "pcba板位类别.slot b/t"),

            // dict.logistics.pcba.panel.category.230
            ("dict.logistics.pcba.panel.category.230", "en-US", "slot t_us", "pcba板位类别.slot t"),
            // dict.logistics.pcba.panel.category.230
            ("dict.logistics.pcba.panel.category.230", "ja-JP", "slot t_jp", "pcba板位类别.slot t"),
            // dict.logistics.pcba.panel.category.230
            ("dict.logistics.pcba.panel.category.230", "zh-CN", "slot t", "pcba板位类别.slot t"),
            // dict.logistics.pcba.panel.category.230
            ("dict.logistics.pcba.panel.category.230", "zh-HK", "slot t_hk", "pcba板位类别.slot t"),

            // dict.logistics.pcba.panel.category.231
            ("dict.logistics.pcba.panel.category.231", "en-US", "spl t_us", "pcba板位类别.spl t"),
            // dict.logistics.pcba.panel.category.231
            ("dict.logistics.pcba.panel.category.231", "ja-JP", "spl t_jp", "pcba板位类别.spl t"),
            // dict.logistics.pcba.panel.category.231
            ("dict.logistics.pcba.panel.category.231", "zh-CN", "spl t", "pcba板位类别.spl t"),
            // dict.logistics.pcba.panel.category.231
            ("dict.logistics.pcba.panel.category.231", "zh-HK", "spl t_hk", "pcba板位类别.spl t"),

            // dict.logistics.pcba.panel.category.232
            ("dict.logistics.pcba.panel.category.232", "en-US", "stby_us", "pcba板位类别.stby"),
            // dict.logistics.pcba.panel.category.232
            ("dict.logistics.pcba.panel.category.232", "ja-JP", "stby_jp", "pcba板位类别.stby"),
            // dict.logistics.pcba.panel.category.232
            ("dict.logistics.pcba.panel.category.232", "zh-CN", "stby", "pcba板位类别.stby"),
            // dict.logistics.pcba.panel.category.232
            ("dict.logistics.pcba.panel.category.232", "zh-HK", "stby_hk", "pcba板位类别.stby"),

            // dict.logistics.pcba.panel.category.233
            ("dict.logistics.pcba.panel.category.233", "en-US", "sts b_us", "pcba板位类别.sts b"),
            // dict.logistics.pcba.panel.category.233
            ("dict.logistics.pcba.panel.category.233", "ja-JP", "sts b_jp", "pcba板位类别.sts b"),
            // dict.logistics.pcba.panel.category.233
            ("dict.logistics.pcba.panel.category.233", "zh-CN", "sts b", "pcba板位类别.sts b"),
            // dict.logistics.pcba.panel.category.233
            ("dict.logistics.pcba.panel.category.233", "zh-HK", "sts b_hk", "pcba板位类别.sts b"),

            // dict.logistics.pcba.panel.category.234
            ("dict.logistics.pcba.panel.category.234", "en-US", "swusb_us", "pcba板位类别.swusb"),
            // dict.logistics.pcba.panel.category.234
            ("dict.logistics.pcba.panel.category.234", "ja-JP", "swusb_jp", "pcba板位类别.swusb"),
            // dict.logistics.pcba.panel.category.234
            ("dict.logistics.pcba.panel.category.234", "zh-CN", "swusb", "pcba板位类别.swusb"),
            // dict.logistics.pcba.panel.category.234
            ("dict.logistics.pcba.panel.category.234", "zh-HK", "swusb_hk", "pcba板位类别.swusb"),

            // dict.logistics.pcba.panel.category.235
            ("dict.logistics.pcba.panel.category.235", "en-US", "swusb akm b_us", "pcba板位类别.swusb akm b"),
            // dict.logistics.pcba.panel.category.235
            ("dict.logistics.pcba.panel.category.235", "ja-JP", "swusb akm b_jp", "pcba板位类别.swusb akm b"),
            // dict.logistics.pcba.panel.category.235
            ("dict.logistics.pcba.panel.category.235", "zh-CN", "swusb akm b", "pcba板位类别.swusb akm b"),
            // dict.logistics.pcba.panel.category.235
            ("dict.logistics.pcba.panel.category.235", "zh-HK", "swusb akm b_hk", "pcba板位类别.swusb akm b"),

            // dict.logistics.pcba.panel.category.236
            ("dict.logistics.pcba.panel.category.236", "en-US", "swusb akm b/t_us", "pcba板位类别.swusb akm b/t"),
            // dict.logistics.pcba.panel.category.236
            ("dict.logistics.pcba.panel.category.236", "ja-JP", "swusb akm b/t_jp", "pcba板位类别.swusb akm b/t"),
            // dict.logistics.pcba.panel.category.236
            ("dict.logistics.pcba.panel.category.236", "zh-CN", "swusb akm b/t", "pcba板位类别.swusb akm b/t"),
            // dict.logistics.pcba.panel.category.236
            ("dict.logistics.pcba.panel.category.236", "zh-HK", "swusb akm b/t_hk", "pcba板位类别.swusb akm b/t"),

            // dict.logistics.pcba.panel.category.237
            ("dict.logistics.pcba.panel.category.237", "en-US", "swusb akm t_us", "pcba板位类别.swusb akm t"),
            // dict.logistics.pcba.panel.category.237
            ("dict.logistics.pcba.panel.category.237", "ja-JP", "swusb akm t_jp", "pcba板位类别.swusb akm t"),
            // dict.logistics.pcba.panel.category.237
            ("dict.logistics.pcba.panel.category.237", "zh-CN", "swusb akm t", "pcba板位类别.swusb akm t"),
            // dict.logistics.pcba.panel.category.237
            ("dict.logistics.pcba.panel.category.237", "zh-HK", "swusb akm t_hk", "pcba板位类别.swusb akm t"),

            // dict.logistics.pcba.panel.category.238
            ("dict.logistics.pcba.panel.category.238", "en-US", "swusb b_us", "pcba板位类别.swusb b"),
            // dict.logistics.pcba.panel.category.238
            ("dict.logistics.pcba.panel.category.238", "ja-JP", "swusb b_jp", "pcba板位类别.swusb b"),
            // dict.logistics.pcba.panel.category.238
            ("dict.logistics.pcba.panel.category.238", "zh-CN", "swusb b", "pcba板位类别.swusb b"),
            // dict.logistics.pcba.panel.category.238
            ("dict.logistics.pcba.panel.category.238", "zh-HK", "swusb b_hk", "pcba板位类别.swusb b"),

            // dict.logistics.pcba.panel.category.239
            ("dict.logistics.pcba.panel.category.239", "en-US", "swusb b/t_us", "pcba板位类别.swusb b/t"),
            // dict.logistics.pcba.panel.category.239
            ("dict.logistics.pcba.panel.category.239", "ja-JP", "swusb b/t_jp", "pcba板位类别.swusb b/t"),
            // dict.logistics.pcba.panel.category.239
            ("dict.logistics.pcba.panel.category.239", "zh-CN", "swusb b/t", "pcba板位类别.swusb b/t"),
            // dict.logistics.pcba.panel.category.239
            ("dict.logistics.pcba.panel.category.239", "zh-HK", "swusb b/t_hk", "pcba板位类别.swusb b/t"),

            // dict.logistics.pcba.panel.category.240
            ("dict.logistics.pcba.panel.category.240", "en-US", "swusb t_us", "pcba板位类别.swusb t"),
            // dict.logistics.pcba.panel.category.240
            ("dict.logistics.pcba.panel.category.240", "ja-JP", "swusb t_jp", "pcba板位类别.swusb t"),
            // dict.logistics.pcba.panel.category.240
            ("dict.logistics.pcba.panel.category.240", "zh-CN", "swusb t", "pcba板位类别.swusb t"),
            // dict.logistics.pcba.panel.category.240
            ("dict.logistics.pcba.panel.category.240", "zh-HK", "swusb t_hk", "pcba板位类别.swusb t"),

            // dict.logistics.pcba.panel.category.241
            ("dict.logistics.pcba.panel.category.241", "en-US", "sys b_us", "pcba板位类别.sys b"),
            // dict.logistics.pcba.panel.category.241
            ("dict.logistics.pcba.panel.category.241", "ja-JP", "sys b_jp", "pcba板位类别.sys b"),
            // dict.logistics.pcba.panel.category.241
            ("dict.logistics.pcba.panel.category.241", "zh-CN", "sys b", "pcba板位类别.sys b"),
            // dict.logistics.pcba.panel.category.241
            ("dict.logistics.pcba.panel.category.241", "zh-HK", "sys b_hk", "pcba板位类别.sys b"),

            // dict.logistics.pcba.panel.category.242
            ("dict.logistics.pcba.panel.category.242", "en-US", "sys t_us", "pcba板位类别.sys t"),
            // dict.logistics.pcba.panel.category.242
            ("dict.logistics.pcba.panel.category.242", "ja-JP", "sys t_jp", "pcba板位类别.sys t"),
            // dict.logistics.pcba.panel.category.242
            ("dict.logistics.pcba.panel.category.242", "zh-CN", "sys t", "pcba板位类别.sys t"),
            // dict.logistics.pcba.panel.category.242
            ("dict.logistics.pcba.panel.category.242", "zh-HK", "sys t_hk", "pcba板位类别.sys t"),

            // dict.logistics.pcba.panel.category.243
            ("dict.logistics.pcba.panel.category.243", "en-US", "top_us", "pcba板位类别.top"),
            // dict.logistics.pcba.panel.category.243
            ("dict.logistics.pcba.panel.category.243", "ja-JP", "top_jp", "pcba板位类别.top"),
            // dict.logistics.pcba.panel.category.243
            ("dict.logistics.pcba.panel.category.243", "zh-CN", "top", "pcba板位类别.top"),
            // dict.logistics.pcba.panel.category.243
            ("dict.logistics.pcba.panel.category.243", "zh-HK", "top_hk", "pcba板位类别.top"),

            // dict.logistics.pcba.panel.category.244
            ("dict.logistics.pcba.panel.category.244", "en-US", "usb b_us", "pcba板位类别.usb b"),
            // dict.logistics.pcba.panel.category.244
            ("dict.logistics.pcba.panel.category.244", "ja-JP", "usb b_jp", "pcba板位类别.usb b"),
            // dict.logistics.pcba.panel.category.244
            ("dict.logistics.pcba.panel.category.244", "zh-CN", "usb b", "pcba板位类别.usb b"),
            // dict.logistics.pcba.panel.category.244
            ("dict.logistics.pcba.panel.category.244", "zh-HK", "usb b_hk", "pcba板位类别.usb b"),

            // dict.logistics.pcba.panel.category.245
            ("dict.logistics.pcba.panel.category.245", "en-US", "usb b/t_us", "pcba板位类别.usb b/t"),
            // dict.logistics.pcba.panel.category.245
            ("dict.logistics.pcba.panel.category.245", "ja-JP", "usb b/t_jp", "pcba板位类别.usb b/t"),
            // dict.logistics.pcba.panel.category.245
            ("dict.logistics.pcba.panel.category.245", "zh-CN", "usb b/t", "pcba板位类别.usb b/t"),
            // dict.logistics.pcba.panel.category.245
            ("dict.logistics.pcba.panel.category.245", "zh-HK", "usb b/t_hk", "pcba板位类别.usb b/t"),

            // dict.logistics.pcba.panel.category.246
            ("dict.logistics.pcba.panel.category.246", "en-US", "xlr_us", "pcba板位类别.xlr"),
            // dict.logistics.pcba.panel.category.246
            ("dict.logistics.pcba.panel.category.246", "ja-JP", "xlr_jp", "pcba板位类别.xlr"),
            // dict.logistics.pcba.panel.category.246
            ("dict.logistics.pcba.panel.category.246", "zh-CN", "xlr", "pcba板位类别.xlr"),
            // dict.logistics.pcba.panel.category.246
            ("dict.logistics.pcba.panel.category.246", "zh-HK", "xlr_hk", "pcba板位类别.xlr"),

            // dict.logistics.pcba.panel.category.249
            ("dict.logistics.pcba.panel.category.249", "en-US", "xlr a_us", "pcba板位类别.xlr a"),
            // dict.logistics.pcba.panel.category.249
            ("dict.logistics.pcba.panel.category.249", "ja-JP", "xlr a_jp", "pcba板位类别.xlr a"),
            // dict.logistics.pcba.panel.category.249
            ("dict.logistics.pcba.panel.category.249", "zh-CN", "xlr a", "pcba板位类别.xlr a"),
            // dict.logistics.pcba.panel.category.249
            ("dict.logistics.pcba.panel.category.249", "zh-HK", "xlr a_hk", "pcba板位类别.xlr a"),

            // dict.logistics.pcba.panel.category.250
            ("dict.logistics.pcba.panel.category.250", "en-US", "xlr b_us", "pcba板位类别.xlr b"),
            // dict.logistics.pcba.panel.category.250
            ("dict.logistics.pcba.panel.category.250", "ja-JP", "xlr b_jp", "pcba板位类别.xlr b"),
            // dict.logistics.pcba.panel.category.250
            ("dict.logistics.pcba.panel.category.250", "zh-CN", "xlr b", "pcba板位类别.xlr b"),
            // dict.logistics.pcba.panel.category.250
            ("dict.logistics.pcba.panel.category.250", "zh-HK", "xlr b_hk", "pcba板位类别.xlr b"),

            // dict.logistics.pcba.panel.category.251
            ("dict.logistics.pcba.panel.category.251", "en-US", "xlr t_us", "pcba板位类别.xlr t"),
            // dict.logistics.pcba.panel.category.251
            ("dict.logistics.pcba.panel.category.251", "ja-JP", "xlr t_jp", "pcba板位类别.xlr t"),
            // dict.logistics.pcba.panel.category.251
            ("dict.logistics.pcba.panel.category.251", "zh-CN", "xlr t", "pcba板位类别.xlr t"),
            // dict.logistics.pcba.panel.category.251
            ("dict.logistics.pcba.panel.category.251", "zh-HK", "xlr t_hk", "pcba板位类别.xlr t"),

            // dict.logistics.pcba.panel.category.252
            ("dict.logistics.pcba.panel.category.252", "en-US", "xlrin b_us", "pcba板位类别.xlrin b"),
            // dict.logistics.pcba.panel.category.252
            ("dict.logistics.pcba.panel.category.252", "ja-JP", "xlrin b_jp", "pcba板位类别.xlrin b"),
            // dict.logistics.pcba.panel.category.252
            ("dict.logistics.pcba.panel.category.252", "zh-CN", "xlrin b", "pcba板位类别.xlrin b"),
            // dict.logistics.pcba.panel.category.252
            ("dict.logistics.pcba.panel.category.252", "zh-HK", "xlrin b_hk", "pcba板位类别.xlrin b"),

            // dict.logistics.pcba.panel.category.253
            ("dict.logistics.pcba.panel.category.253", "en-US", "xlrin b/t_us", "pcba板位类别.xlrin b/t"),
            // dict.logistics.pcba.panel.category.253
            ("dict.logistics.pcba.panel.category.253", "ja-JP", "xlrin b/t_jp", "pcba板位类别.xlrin b/t"),
            // dict.logistics.pcba.panel.category.253
            ("dict.logistics.pcba.panel.category.253", "zh-CN", "xlrin b/t", "pcba板位类别.xlrin b/t"),
            // dict.logistics.pcba.panel.category.253
            ("dict.logistics.pcba.panel.category.253", "zh-HK", "xlrin b/t_hk", "pcba板位类别.xlrin b/t"),

            // dict.logistics.pcba.panel.category.254
            ("dict.logistics.pcba.panel.category.254", "en-US", "xlrin t_us", "pcba板位类别.xlrin t"),
            // dict.logistics.pcba.panel.category.254
            ("dict.logistics.pcba.panel.category.254", "ja-JP", "xlrin t_jp", "pcba板位类别.xlrin t"),
            // dict.logistics.pcba.panel.category.254
            ("dict.logistics.pcba.panel.category.254", "zh-CN", "xlrin t", "pcba板位类别.xlrin t"),
            // dict.logistics.pcba.panel.category.254
            ("dict.logistics.pcba.panel.category.254", "zh-HK", "xlrin t_hk", "pcba板位类别.xlrin t"),

            // dict.logistics.pcba.panel.category.255
            ("dict.logistics.pcba.panel.category.255", "en-US", "xlrio b_us", "pcba板位类别.xlrio b"),
            // dict.logistics.pcba.panel.category.255
            ("dict.logistics.pcba.panel.category.255", "ja-JP", "xlrio b_jp", "pcba板位类别.xlrio b"),
            // dict.logistics.pcba.panel.category.255
            ("dict.logistics.pcba.panel.category.255", "zh-CN", "xlrio b", "pcba板位类别.xlrio b"),
            // dict.logistics.pcba.panel.category.255
            ("dict.logistics.pcba.panel.category.255", "zh-HK", "xlrio b_hk", "pcba板位类别.xlrio b"),

            // dict.logistics.pcba.panel.category.256
            ("dict.logistics.pcba.panel.category.256", "en-US", "xlrio b/t_us", "pcba板位类别.xlrio b/t"),
            // dict.logistics.pcba.panel.category.256
            ("dict.logistics.pcba.panel.category.256", "ja-JP", "xlrio b/t_jp", "pcba板位类别.xlrio b/t"),
            // dict.logistics.pcba.panel.category.256
            ("dict.logistics.pcba.panel.category.256", "zh-CN", "xlrio b/t", "pcba板位类别.xlrio b/t"),
            // dict.logistics.pcba.panel.category.256
            ("dict.logistics.pcba.panel.category.256", "zh-HK", "xlrio b/t_hk", "pcba板位类别.xlrio b/t"),

            // dict.logistics.pcba.panel.category.257
            ("dict.logistics.pcba.panel.category.257", "en-US", "xlrio t_us", "pcba板位类别.xlrio t"),
            // dict.logistics.pcba.panel.category.257
            ("dict.logistics.pcba.panel.category.257", "ja-JP", "xlrio t_jp", "pcba板位类别.xlrio t"),
            // dict.logistics.pcba.panel.category.257
            ("dict.logistics.pcba.panel.category.257", "zh-CN", "xlrio t", "pcba板位类别.xlrio t"),
            // dict.logistics.pcba.panel.category.257
            ("dict.logistics.pcba.panel.category.257", "zh-HK", "xlrio t_hk", "pcba板位类别.xlrio t"),

            // dict.logistics.pcba.panel.category.258
            ("dict.logistics.pcba.panel.category.258", "en-US", "xlrout_us", "pcba板位类别.xlrout"),
            // dict.logistics.pcba.panel.category.258
            ("dict.logistics.pcba.panel.category.258", "ja-JP", "xlrout_jp", "pcba板位类别.xlrout"),
            // dict.logistics.pcba.panel.category.258
            ("dict.logistics.pcba.panel.category.258", "zh-CN", "xlrout", "pcba板位类别.xlrout"),
            // dict.logistics.pcba.panel.category.258
            ("dict.logistics.pcba.panel.category.258", "zh-HK", "xlrout_hk", "pcba板位类别.xlrout"),

            // dict.logistics.pcba.side.category.b
            ("dict.logistics.pcba.side.category.b", "en-US", "b面_us", "pcba面别.b面"),
            // dict.logistics.pcba.side.category.b
            ("dict.logistics.pcba.side.category.b", "ja-JP", "b面_jp", "pcba面别.b面"),
            // dict.logistics.pcba.side.category.b
            ("dict.logistics.pcba.side.category.b", "zh-CN", "b面", "pcba面别.b面"),
            // dict.logistics.pcba.side.category.b
            ("dict.logistics.pcba.side.category.b", "zh-HK", "b面_hk", "pcba面别.b面"),

            // dict.logistics.pcba.side.category.t
            ("dict.logistics.pcba.side.category.t", "en-US", "t面_us", "pcba面别.t面"),
            // dict.logistics.pcba.side.category.t
            ("dict.logistics.pcba.side.category.t", "ja-JP", "t面_jp", "pcba面别.t面"),
            // dict.logistics.pcba.side.category.t
            ("dict.logistics.pcba.side.category.t", "zh-CN", "t面", "pcba面别.t面"),
            // dict.logistics.pcba.side.category.t
            ("dict.logistics.pcba.side.category.t", "zh-HK", "t面_hk", "pcba面别.t面"),

            // dict.logistics.shift.category.1
            ("dict.logistics.shift.category.1", "en-US", "早_us", "生产班别.早"),
            // dict.logistics.shift.category.1
            ("dict.logistics.shift.category.1", "ja-JP", "早_jp", "生产班别.早"),
            // dict.logistics.shift.category.1
            ("dict.logistics.shift.category.1", "zh-CN", "早", "生产班别.早"),
            // dict.logistics.shift.category.1
            ("dict.logistics.shift.category.1", "zh-HK", "早_hk", "生产班别.早"),

            // dict.logistics.shift.category.2
            ("dict.logistics.shift.category.2", "en-US", "中_us", "生产班别.中"),
            // dict.logistics.shift.category.2
            ("dict.logistics.shift.category.2", "ja-JP", "中_jp", "生产班别.中"),
            // dict.logistics.shift.category.2
            ("dict.logistics.shift.category.2", "zh-CN", "中", "生产班别.中"),
            // dict.logistics.shift.category.2
            ("dict.logistics.shift.category.2", "zh-HK", "中_hk", "生产班别.中"),

            // dict.logistics.shift.category.3
            ("dict.logistics.shift.category.3", "en-US", "晚_us", "生产班别.晚"),
            // dict.logistics.shift.category.3
            ("dict.logistics.shift.category.3", "ja-JP", "晚_jp", "生产班别.晚"),
            // dict.logistics.shift.category.3
            ("dict.logistics.shift.category.3", "zh-CN", "晚", "生产班别.晚"),
            // dict.logistics.shift.category.3
            ("dict.logistics.shift.category.3", "zh-HK", "晚_hk", "生产班别.晚"),

            // dict.logistics.shift.category.4
            ("dict.logistics.shift.category.4", "en-US", "白班_us", "生产班别.白班"),
            // dict.logistics.shift.category.4
            ("dict.logistics.shift.category.4", "ja-JP", "白班_jp", "生产班别.白班"),
            // dict.logistics.shift.category.4
            ("dict.logistics.shift.category.4", "zh-CN", "白班", "生产班别.白班"),
            // dict.logistics.shift.category.4
            ("dict.logistics.shift.category.4", "zh-HK", "白班_hk", "生产班别.白班"),

            // dict.logistics.shift.category.5
            ("dict.logistics.shift.category.5", "en-US", "夜班_us", "生产班别.夜班"),
            // dict.logistics.shift.category.5
            ("dict.logistics.shift.category.5", "ja-JP", "夜班_jp", "生产班别.夜班"),
            // dict.logistics.shift.category.5
            ("dict.logistics.shift.category.5", "zh-CN", "夜班", "生产班别.夜班"),
            // dict.logistics.shift.category.5
            ("dict.logistics.shift.category.5", "zh-HK", "夜班_hk", "生产班别.夜班"),

            // dict.logistics.stop.reason.category.1
            ("dict.logistics.stop.reason.category.1", "en-US", "切换停止时间_us", "停线原因.切换停止时间"),
            // dict.logistics.stop.reason.category.1
            ("dict.logistics.stop.reason.category.1", "ja-JP", "切换停止时间_jp", "停线原因.切换停止时间"),
            // dict.logistics.stop.reason.category.1
            ("dict.logistics.stop.reason.category.1", "zh-CN", "切换停止时间", "停线原因.切换停止时间"),
            // dict.logistics.stop.reason.category.1
            ("dict.logistics.stop.reason.category.1", "zh-HK", "切换停止时间_hk", "停线原因.切换停止时间"),

            // dict.logistics.stop.reason.category.2
            ("dict.logistics.stop.reason.category.2", "en-US", "周会_us", "停线原因.周会"),
            // dict.logistics.stop.reason.category.2
            ("dict.logistics.stop.reason.category.2", "ja-JP", "周会_jp", "停线原因.周会"),
            // dict.logistics.stop.reason.category.2
            ("dict.logistics.stop.reason.category.2", "zh-CN", "周会", "停线原因.周会"),
            // dict.logistics.stop.reason.category.2
            ("dict.logistics.stop.reason.category.2", "zh-HK", "周会_hk", "停线原因.周会"),

            // dict.logistics.stop.reason.category.3
            ("dict.logistics.stop.reason.category.3", "en-US", "其他_us", "停线原因.其他"),
            // dict.logistics.stop.reason.category.3
            ("dict.logistics.stop.reason.category.3", "ja-JP", "其他_jp", "停线原因.其他"),
            // dict.logistics.stop.reason.category.3
            ("dict.logistics.stop.reason.category.3", "zh-CN", "其他", "停线原因.其他"),
            // dict.logistics.stop.reason.category.3
            ("dict.logistics.stop.reason.category.3", "zh-HK", "其他_hk", "停线原因.其他"),

            // dict.logistics.stop.reason.category.4
            ("dict.logistics.stop.reason.category.4", "en-US", "欠料_us", "停线原因.欠料"),
            // dict.logistics.stop.reason.category.4
            ("dict.logistics.stop.reason.category.4", "ja-JP", "欠料_jp", "停线原因.欠料"),
            // dict.logistics.stop.reason.category.4
            ("dict.logistics.stop.reason.category.4", "zh-CN", "欠料", "停线原因.欠料"),
            // dict.logistics.stop.reason.category.4
            ("dict.logistics.stop.reason.category.4", "zh-HK", "欠料_hk", "停线原因.欠料"),

            // dict.logistics.stop.reason.category.5
            ("dict.logistics.stop.reason.category.5", "en-US", "停电_us", "停线原因.停电"),
            // dict.logistics.stop.reason.category.5
            ("dict.logistics.stop.reason.category.5", "ja-JP", "停电_jp", "停线原因.停电"),
            // dict.logistics.stop.reason.category.5
            ("dict.logistics.stop.reason.category.5", "zh-CN", "停电", "停线原因.停电"),
            // dict.logistics.stop.reason.category.5
            ("dict.logistics.stop.reason.category.5", "zh-HK", "停电_hk", "停线原因.停电"),

            // dict.logistics.stop.reason.category.6
            ("dict.logistics.stop.reason.category.6", "en-US", "班会_us", "停线原因.班会"),
            // dict.logistics.stop.reason.category.6
            ("dict.logistics.stop.reason.category.6", "ja-JP", "班会_jp", "停线原因.班会"),
            // dict.logistics.stop.reason.category.6
            ("dict.logistics.stop.reason.category.6", "zh-CN", "班会", "停线原因.班会"),
            // dict.logistics.stop.reason.category.6
            ("dict.logistics.stop.reason.category.6", "zh-HK", "班会_hk", "停线原因.班会"),

            // dict.logistics.stop.reason.category.7
            ("dict.logistics.stop.reason.category.7", "en-US", "切换机种_us", "停线原因.切换机种"),
            // dict.logistics.stop.reason.category.7
            ("dict.logistics.stop.reason.category.7", "ja-JP", "切换机种_jp", "停线原因.切换机种"),
            // dict.logistics.stop.reason.category.7
            ("dict.logistics.stop.reason.category.7", "zh-CN", "切换机种", "停线原因.切换机种"),
            // dict.logistics.stop.reason.category.7
            ("dict.logistics.stop.reason.category.7", "zh-HK", "切换机种_hk", "停线原因.切换机种"),

            // dict.logistics.stop.reason.category.8
            ("dict.logistics.stop.reason.category.8", "en-US", "早会_us", "停线原因.早会"),
            // dict.logistics.stop.reason.category.8
            ("dict.logistics.stop.reason.category.8", "ja-JP", "早会_jp", "停线原因.早会"),
            // dict.logistics.stop.reason.category.8
            ("dict.logistics.stop.reason.category.8", "zh-CN", "早会", "停线原因.早会"),
            // dict.logistics.stop.reason.category.8
            ("dict.logistics.stop.reason.category.8", "zh-HK", "早会_hk", "停线原因.早会"),

            // dict.logistics.stop.reason.category.9
            ("dict.logistics.stop.reason.category.9", "en-US", "组立_us", "停线原因.组立"),
            // dict.logistics.stop.reason.category.9
            ("dict.logistics.stop.reason.category.9", "ja-JP", "组立_jp", "停线原因.组立"),
            // dict.logistics.stop.reason.category.9
            ("dict.logistics.stop.reason.category.9", "zh-CN", "组立", "停线原因.组立"),
            // dict.logistics.stop.reason.category.9
            ("dict.logistics.stop.reason.category.9", "zh-HK", "组立_hk", "停线原因.组立"),

            // dict.logistics.stop.reason.category.10
            ("dict.logistics.stop.reason.category.10", "en-US", "学习_us", "停线原因.学习"),
            // dict.logistics.stop.reason.category.10
            ("dict.logistics.stop.reason.category.10", "ja-JP", "学习_jp", "停线原因.学习"),
            // dict.logistics.stop.reason.category.10
            ("dict.logistics.stop.reason.category.10", "zh-CN", "学习", "停线原因.学习"),
            // dict.logistics.stop.reason.category.10
            ("dict.logistics.stop.reason.category.10", "zh-HK", "学习_hk", "停线原因.学习"),

            // dict.logistics.stop.reason.category.11
            ("dict.logistics.stop.reason.category.11", "en-US", "仪设_us", "停线原因.仪设"),
            // dict.logistics.stop.reason.category.11
            ("dict.logistics.stop.reason.category.11", "ja-JP", "仪设_jp", "停线原因.仪设"),
            // dict.logistics.stop.reason.category.11
            ("dict.logistics.stop.reason.category.11", "zh-CN", "仪设", "停线原因.仪设"),
            // dict.logistics.stop.reason.category.11
            ("dict.logistics.stop.reason.category.11", "zh-HK", "仪设_hk", "停线原因.仪设"),

            // dict.logistics.stop.reason.category.12
            ("dict.logistics.stop.reason.category.12", "en-US", "清洁_us", "停线原因.清洁"),
            // dict.logistics.stop.reason.category.12
            ("dict.logistics.stop.reason.category.12", "ja-JP", "清洁_jp", "停线原因.清洁"),
            // dict.logistics.stop.reason.category.12
            ("dict.logistics.stop.reason.category.12", "zh-CN", "清洁", "停线原因.清洁"),
            // dict.logistics.stop.reason.category.12
            ("dict.logistics.stop.reason.category.12", "zh-HK", "清洁_hk", "停线原因.清洁"),

            // dict.logistics.visual.inspection.line.category.1
            ("dict.logistics.visual.inspection.line.category.1", "en-US", "1_us", "目视线别.1"),
            // dict.logistics.visual.inspection.line.category.1
            ("dict.logistics.visual.inspection.line.category.1", "ja-JP", "1_jp", "目视线别.1"),
            // dict.logistics.visual.inspection.line.category.1
            ("dict.logistics.visual.inspection.line.category.1", "zh-CN", "1", "目视线别.1"),
            // dict.logistics.visual.inspection.line.category.1
            ("dict.logistics.visual.inspection.line.category.1", "zh-HK", "1_hk", "目视线别.1"),

            // dict.logistics.visual.inspection.line.category.2
            ("dict.logistics.visual.inspection.line.category.2", "en-US", "2_us", "目视线别.2"),
            // dict.logistics.visual.inspection.line.category.2
            ("dict.logistics.visual.inspection.line.category.2", "ja-JP", "2_jp", "目视线别.2"),
            // dict.logistics.visual.inspection.line.category.2
            ("dict.logistics.visual.inspection.line.category.2", "zh-CN", "2", "目视线别.2"),
            // dict.logistics.visual.inspection.line.category.2
            ("dict.logistics.visual.inspection.line.category.2", "zh-HK", "2_hk", "目视线别.2"),

            // dict.logistics.warranty.status.0
            ("dict.logistics.warranty.status.0", "en-US", "无保修_us", "保修状态.无保修"),
            // dict.logistics.warranty.status.0
            ("dict.logistics.warranty.status.0", "ja-JP", "无保修_jp", "保修状态.无保修"),
            // dict.logistics.warranty.status.0
            ("dict.logistics.warranty.status.0", "zh-CN", "无保修", "保修状态.无保修"),
            // dict.logistics.warranty.status.0
            ("dict.logistics.warranty.status.0", "zh-HK", "无保修_hk", "保修状态.无保修"),

            // dict.logistics.warranty.status.1
            ("dict.logistics.warranty.status.1", "en-US", "保修期内_us", "保修状态.保修期内"),
            // dict.logistics.warranty.status.1
            ("dict.logistics.warranty.status.1", "ja-JP", "保修期内_jp", "保修状态.保修期内"),
            // dict.logistics.warranty.status.1
            ("dict.logistics.warranty.status.1", "zh-CN", "保修期内", "保修状态.保修期内"),
            // dict.logistics.warranty.status.1
            ("dict.logistics.warranty.status.1", "zh-HK", "保修期内_hk", "保修状态.保修期内"),

            // dict.logistics.warranty.status.2
            ("dict.logistics.warranty.status.2", "en-US", "保修期外_us", "保修状态.保修期外"),
            // dict.logistics.warranty.status.2
            ("dict.logistics.warranty.status.2", "ja-JP", "保修期外_jp", "保修状态.保修期外"),
            // dict.logistics.warranty.status.2
            ("dict.logistics.warranty.status.2", "zh-CN", "保修期外", "保修状态.保修期外"),
            // dict.logistics.warranty.status.2
            ("dict.logistics.warranty.status.2", "zh-HK", "保修期外_hk", "保修状态.保修期外"),

            // dict.logistics.warranty.status.3
            ("dict.logistics.warranty.status.3", "en-US", "延保中_us", "保修状态.延保中"),
            // dict.logistics.warranty.status.3
            ("dict.logistics.warranty.status.3", "ja-JP", "延保中_jp", "保修状态.延保中"),
            // dict.logistics.warranty.status.3
            ("dict.logistics.warranty.status.3", "zh-CN", "延保中", "保修状态.延保中"),
            // dict.logistics.warranty.status.3
            ("dict.logistics.warranty.status.3", "zh-HK", "延保中_hk", "保修状态.延保中"),

            // dict.sys.culture.code.af-za
            ("dict.sys.culture.code.af-za", "en-US", "Afrikaans", "区域文化编码.Afrikaans"),
            // dict.sys.culture.code.af-za
            ("dict.sys.culture.code.af-za", "ja-JP", "Afrikaans", "区域文化编码.Afrikaans"),
            // dict.sys.culture.code.af-za
            ("dict.sys.culture.code.af-za", "zh-CN", "Afrikaans", "区域文化编码.Afrikaans"),
            // dict.sys.culture.code.af-za
            ("dict.sys.culture.code.af-za", "zh-HK", "Afrikaans", "区域文化编码.Afrikaans"),

            // dict.sys.culture.code.ar-ae
            ("dict.sys.culture.code.ar-ae", "en-US", "العربية", "区域文化编码.العربية"),
            // dict.sys.culture.code.ar-ae
            ("dict.sys.culture.code.ar-ae", "ja-JP", "العربية", "区域文化编码.العربية"),
            // dict.sys.culture.code.ar-ae
            ("dict.sys.culture.code.ar-ae", "zh-CN", "العربية", "区域文化编码.العربية"),
            // dict.sys.culture.code.ar-ae
            ("dict.sys.culture.code.ar-ae", "zh-HK", "العربية", "区域文化编码.العربية"),

            // dict.sys.culture.code.be-by
            ("dict.sys.culture.code.be-by", "en-US", "Беларуская", "区域文化编码.Беларуская (BY)"),
            // dict.sys.culture.code.be-by
            ("dict.sys.culture.code.be-by", "ja-JP", "Беларуская", "区域文化编码.Беларуская (BY)"),
            // dict.sys.culture.code.be-by
            ("dict.sys.culture.code.be-by", "zh-CN", "Беларуская", "区域文化编码.Беларуская (BY)"),
            // dict.sys.culture.code.be-by
            ("dict.sys.culture.code.be-by", "zh-HK", "Беларуская", "区域文化编码.Беларуская (BY)"),

            // dict.sys.culture.code.cs-cz
            ("dict.sys.culture.code.cs-cz", "en-US", "čeština", "区域文化编码.čeština"),
            // dict.sys.culture.code.cs-cz
            ("dict.sys.culture.code.cs-cz", "ja-JP", "čeština", "区域文化编码.čeština"),
            // dict.sys.culture.code.cs-cz
            ("dict.sys.culture.code.cs-cz", "zh-CN", "čeština", "区域文化编码.čeština"),
            // dict.sys.culture.code.cs-cz
            ("dict.sys.culture.code.cs-cz", "zh-HK", "čeština", "区域文化编码.čeština"),

            // dict.sys.culture.code.cy-gb
            ("dict.sys.culture.code.cy-gb", "en-US", "Cymraeg", "区域文化编码.Cymraeg"),
            // dict.sys.culture.code.cy-gb
            ("dict.sys.culture.code.cy-gb", "ja-JP", "Cymraeg", "区域文化编码.Cymraeg"),
            // dict.sys.culture.code.cy-gb
            ("dict.sys.culture.code.cy-gb", "zh-CN", "Cymraeg", "区域文化编码.Cymraeg"),
            // dict.sys.culture.code.cy-gb
            ("dict.sys.culture.code.cy-gb", "zh-HK", "Cymraeg", "区域文化编码.Cymraeg"),

            // dict.sys.culture.code.da-dk
            ("dict.sys.culture.code.da-dk", "en-US", "dansk", "区域文化编码.dansk"),
            // dict.sys.culture.code.da-dk
            ("dict.sys.culture.code.da-dk", "ja-JP", "dansk", "区域文化编码.dansk"),
            // dict.sys.culture.code.da-dk
            ("dict.sys.culture.code.da-dk", "zh-CN", "dansk", "区域文化编码.dansk"),
            // dict.sys.culture.code.da-dk
            ("dict.sys.culture.code.da-dk", "zh-HK", "dansk", "区域文化编码.dansk"),

            // dict.sys.culture.code.de-de
            ("dict.sys.culture.code.de-de", "en-US", "Deutsch", "区域文化编码.Deutsch"),
            // dict.sys.culture.code.de-de
            ("dict.sys.culture.code.de-de", "ja-JP", "Deutsch", "区域文化编码.Deutsch"),
            // dict.sys.culture.code.de-de
            ("dict.sys.culture.code.de-de", "zh-CN", "Deutsch", "区域文化编码.Deutsch"),
            // dict.sys.culture.code.de-de
            ("dict.sys.culture.code.de-de", "zh-HK", "Deutsch", "区域文化编码.Deutsch"),

            // dict.sys.culture.code.el-gr
            ("dict.sys.culture.code.el-gr", "en-US", "Ελληνικά", "区域文化编码.Ελληνικά"),
            // dict.sys.culture.code.el-gr
            ("dict.sys.culture.code.el-gr", "ja-JP", "Ελληνικά", "区域文化编码.Ελληνικά"),
            // dict.sys.culture.code.el-gr
            ("dict.sys.culture.code.el-gr", "zh-CN", "Ελληνικά", "区域文化编码.Ελληνικά"),
            // dict.sys.culture.code.el-gr
            ("dict.sys.culture.code.el-gr", "zh-HK", "Ελληνικά", "区域文化编码.Ελληνικά"),

            // dict.sys.culture.code.en-au
            ("dict.sys.culture.code.en-au", "en-US", "English(AU)", "区域文化编码.English (AU)"),
            // dict.sys.culture.code.en-au
            ("dict.sys.culture.code.en-au", "ja-JP", "English(AU)", "区域文化编码.English (AU)"),
            // dict.sys.culture.code.en-au
            ("dict.sys.culture.code.en-au", "zh-CN", "English(AU)", "区域文化编码.English (AU)"),
            // dict.sys.culture.code.en-au
            ("dict.sys.culture.code.en-au", "zh-HK", "English(AU)", "区域文化编码.English (AU)"),

            // dict.sys.culture.code.en-gb
            ("dict.sys.culture.code.en-gb", "en-US", "English(UK)", "区域文化编码.English (UK)"),
            // dict.sys.culture.code.en-gb
            ("dict.sys.culture.code.en-gb", "ja-JP", "English(UK)", "区域文化编码.English (UK)"),
            // dict.sys.culture.code.en-gb
            ("dict.sys.culture.code.en-gb", "zh-CN", "English(UK)", "区域文化编码.English (UK)"),
            // dict.sys.culture.code.en-gb
            ("dict.sys.culture.code.en-gb", "zh-HK", "English(UK)", "区域文化编码.English (UK)"),

            // dict.sys.culture.code.en-us
            ("dict.sys.culture.code.en-us", "en-US", "English(US)", "区域文化编码.English (US)"),
            // dict.sys.culture.code.en-us
            ("dict.sys.culture.code.en-us", "ja-JP", "English(US)", "区域文化编码.English (US)"),
            // dict.sys.culture.code.en-us
            ("dict.sys.culture.code.en-us", "zh-CN", "English(US)", "区域文化编码.English (US)"),
            // dict.sys.culture.code.en-us
            ("dict.sys.culture.code.en-us", "zh-HK", "English(US)", "区域文化编码.English (US)"),

            // dict.sys.culture.code.es-es
            ("dict.sys.culture.code.es-es", "en-US", "Español", "区域文化编码.Español"),
            // dict.sys.culture.code.es-es
            ("dict.sys.culture.code.es-es", "ja-JP", "Español", "区域文化编码.Español"),
            // dict.sys.culture.code.es-es
            ("dict.sys.culture.code.es-es", "zh-CN", "Español", "区域文化编码.Español"),
            // dict.sys.culture.code.es-es
            ("dict.sys.culture.code.es-es", "zh-HK", "Español", "区域文化编码.Español"),

            // dict.sys.culture.code.et-ee
            ("dict.sys.culture.code.et-ee", "en-US", "eesti", "区域文化编码.eesti"),
            // dict.sys.culture.code.et-ee
            ("dict.sys.culture.code.et-ee", "ja-JP", "eesti", "区域文化编码.eesti"),
            // dict.sys.culture.code.et-ee
            ("dict.sys.culture.code.et-ee", "zh-CN", "eesti", "区域文化编码.eesti"),
            // dict.sys.culture.code.et-ee
            ("dict.sys.culture.code.et-ee", "zh-HK", "eesti", "区域文化编码.eesti"),

            // dict.sys.culture.code.fi-fi
            ("dict.sys.culture.code.fi-fi", "en-US", "suomi", "区域文化编码.suomi"),
            // dict.sys.culture.code.fi-fi
            ("dict.sys.culture.code.fi-fi", "ja-JP", "suomi", "区域文化编码.suomi"),
            // dict.sys.culture.code.fi-fi
            ("dict.sys.culture.code.fi-fi", "zh-CN", "suomi", "区域文化编码.suomi"),
            // dict.sys.culture.code.fi-fi
            ("dict.sys.culture.code.fi-fi", "zh-HK", "suomi", "区域文化编码.suomi"),

            // dict.sys.culture.code.fr-fr
            ("dict.sys.culture.code.fr-fr", "en-US", "français", "区域文化编码.français"),
            // dict.sys.culture.code.fr-fr
            ("dict.sys.culture.code.fr-fr", "ja-JP", "français", "区域文化编码.français"),
            // dict.sys.culture.code.fr-fr
            ("dict.sys.culture.code.fr-fr", "zh-CN", "français", "区域文化编码.français"),
            // dict.sys.culture.code.fr-fr
            ("dict.sys.culture.code.fr-fr", "zh-HK", "français", "区域文化编码.français"),

            // dict.sys.culture.code.hi-in
            ("dict.sys.culture.code.hi-in", "en-US", "हिंदी", "区域文化编码.हिंदी"),
            // dict.sys.culture.code.hi-in
            ("dict.sys.culture.code.hi-in", "ja-JP", "हिंदी", "区域文化编码.हिंदी"),
            // dict.sys.culture.code.hi-in
            ("dict.sys.culture.code.hi-in", "zh-CN", "हिंदी", "区域文化编码.हिंदी"),
            // dict.sys.culture.code.hi-in
            ("dict.sys.culture.code.hi-in", "zh-HK", "हिंदी", "区域文化编码.हिंदी"),

            // dict.sys.culture.code.hr-hr
            ("dict.sys.culture.code.hr-hr", "en-US", "hrvatski", "区域文化编码.hrvatski"),
            // dict.sys.culture.code.hr-hr
            ("dict.sys.culture.code.hr-hr", "ja-JP", "hrvatski", "区域文化编码.hrvatski"),
            // dict.sys.culture.code.hr-hr
            ("dict.sys.culture.code.hr-hr", "zh-CN", "hrvatski", "区域文化编码.hrvatski"),
            // dict.sys.culture.code.hr-hr
            ("dict.sys.culture.code.hr-hr", "zh-HK", "hrvatski", "区域文化编码.hrvatski"),

            // dict.sys.culture.code.it-it
            ("dict.sys.culture.code.it-it", "en-US", "italiano", "区域文化编码.italiano"),
            // dict.sys.culture.code.it-it
            ("dict.sys.culture.code.it-it", "ja-JP", "italiano", "区域文化编码.italiano"),
            // dict.sys.culture.code.it-it
            ("dict.sys.culture.code.it-it", "zh-CN", "italiano", "区域文化编码.italiano"),
            // dict.sys.culture.code.it-it
            ("dict.sys.culture.code.it-it", "zh-HK", "italiano", "区域文化编码.italiano"),

            // dict.sys.culture.code.ja-jp
            ("dict.sys.culture.code.ja-jp", "en-US", "日本語", "区域文化编码.日本語"),
            // dict.sys.culture.code.ja-jp
            ("dict.sys.culture.code.ja-jp", "ja-JP", "日本語", "区域文化编码.日本語"),
            // dict.sys.culture.code.ja-jp
            ("dict.sys.culture.code.ja-jp", "zh-CN", "日本語", "区域文化编码.日本語"),
            // dict.sys.culture.code.ja-jp
            ("dict.sys.culture.code.ja-jp", "zh-HK", "日本語", "区域文化编码.日本語"),

            // dict.sys.culture.code.ka-ge
            ("dict.sys.culture.code.ka-ge", "en-US", "ქართული", "区域文化编码.ქართული"),
            // dict.sys.culture.code.ka-ge
            ("dict.sys.culture.code.ka-ge", "ja-JP", "ქართული", "区域文化编码.ქართული"),
            // dict.sys.culture.code.ka-ge
            ("dict.sys.culture.code.ka-ge", "zh-CN", "ქართული", "区域文化编码.ქართული"),
            // dict.sys.culture.code.ka-ge
            ("dict.sys.culture.code.ka-ge", "zh-HK", "ქართული", "区域文化编码.ქართული"),

            // dict.sys.culture.code.ko-kr
            ("dict.sys.culture.code.ko-kr", "en-US", "한국어", "区域文化编码.한국어"),
            // dict.sys.culture.code.ko-kr
            ("dict.sys.culture.code.ko-kr", "ja-JP", "한국어", "区域文化编码.한국어"),
            // dict.sys.culture.code.ko-kr
            ("dict.sys.culture.code.ko-kr", "zh-CN", "한국어", "区域文化编码.한국어"),
            // dict.sys.culture.code.ko-kr
            ("dict.sys.culture.code.ko-kr", "zh-HK", "한국어", "区域文化编码.한국어"),

            // dict.sys.culture.code.nl-nl
            ("dict.sys.culture.code.nl-nl", "en-US", "Nederlands", "区域文化编码.Nederlands"),
            // dict.sys.culture.code.nl-nl
            ("dict.sys.culture.code.nl-nl", "ja-JP", "Nederlands", "区域文化编码.Nederlands"),
            // dict.sys.culture.code.nl-nl
            ("dict.sys.culture.code.nl-nl", "zh-CN", "Nederlands", "区域文化编码.Nederlands"),
            // dict.sys.culture.code.nl-nl
            ("dict.sys.culture.code.nl-nl", "zh-HK", "Nederlands", "区域文化编码.Nederlands"),

            // dict.sys.culture.code.pl-pl
            ("dict.sys.culture.code.pl-pl", "en-US", "Polski", "区域文化编码.Polski"),
            // dict.sys.culture.code.pl-pl
            ("dict.sys.culture.code.pl-pl", "ja-JP", "Polski", "区域文化编码.Polski"),
            // dict.sys.culture.code.pl-pl
            ("dict.sys.culture.code.pl-pl", "zh-CN", "Polski", "区域文化编码.Polski"),
            // dict.sys.culture.code.pl-pl
            ("dict.sys.culture.code.pl-pl", "zh-HK", "Polski", "区域文化编码.Polski"),

            // dict.sys.culture.code.ro-ro
            ("dict.sys.culture.code.ro-ro", "en-US", "română", "区域文化编码.română"),
            // dict.sys.culture.code.ro-ro
            ("dict.sys.culture.code.ro-ro", "ja-JP", "română", "区域文化编码.română"),
            // dict.sys.culture.code.ro-ro
            ("dict.sys.culture.code.ro-ro", "zh-CN", "română", "区域文化编码.română"),
            // dict.sys.culture.code.ro-ro
            ("dict.sys.culture.code.ro-ro", "zh-HK", "română", "区域文化编码.română"),

            // dict.sys.culture.code.ru-ru
            ("dict.sys.culture.code.ru-ru", "en-US", "Русский", "区域文化编码.Русский"),
            // dict.sys.culture.code.ru-ru
            ("dict.sys.culture.code.ru-ru", "ja-JP", "Русский", "区域文化编码.Русский"),
            // dict.sys.culture.code.ru-ru
            ("dict.sys.culture.code.ru-ru", "zh-CN", "Русский", "区域文化编码.Русский"),
            // dict.sys.culture.code.ru-ru
            ("dict.sys.culture.code.ru-ru", "zh-HK", "Русский", "区域文化编码.Русский"),

            // dict.sys.culture.code.sv-se
            ("dict.sys.culture.code.sv-se", "en-US", "svenska", "区域文化编码.svenska"),
            // dict.sys.culture.code.sv-se
            ("dict.sys.culture.code.sv-se", "ja-JP", "svenska", "区域文化编码.svenska"),
            // dict.sys.culture.code.sv-se
            ("dict.sys.culture.code.sv-se", "zh-CN", "svenska", "区域文化编码.svenska"),
            // dict.sys.culture.code.sv-se
            ("dict.sys.culture.code.sv-se", "zh-HK", "svenska", "区域文化编码.svenska"),

            // dict.sys.culture.code.th-th
            ("dict.sys.culture.code.th-th", "en-US", "ไทย", "区域文化编码.ไทย"),
            // dict.sys.culture.code.th-th
            ("dict.sys.culture.code.th-th", "ja-JP", "ไทย", "区域文化编码.ไทย"),
            // dict.sys.culture.code.th-th
            ("dict.sys.culture.code.th-th", "zh-CN", "ไทย", "区域文化编码.ไทย"),
            // dict.sys.culture.code.th-th
            ("dict.sys.culture.code.th-th", "zh-HK", "ไทย", "区域文化编码.ไทย"),

            // dict.sys.culture.code.vi-vn
            ("dict.sys.culture.code.vi-vn", "en-US", "Tiếng Việt", "区域文化编码.Tiếng Việt"),
            // dict.sys.culture.code.vi-vn
            ("dict.sys.culture.code.vi-vn", "ja-JP", "Tiếng Việt", "区域文化编码.Tiếng Việt"),
            // dict.sys.culture.code.vi-vn
            ("dict.sys.culture.code.vi-vn", "zh-CN", "Tiếng Việt", "区域文化编码.Tiếng Việt"),
            // dict.sys.culture.code.vi-vn
            ("dict.sys.culture.code.vi-vn", "zh-HK", "Tiếng Việt", "区域文化编码.Tiếng Việt"),

            // dict.sys.culture.code.zh-cn
            ("dict.sys.culture.code.zh-cn", "en-US", "中文(简体)", "区域文化编码.中文 (简体)"),
            // dict.sys.culture.code.zh-cn
            ("dict.sys.culture.code.zh-cn", "ja-JP", "中文(简体)", "区域文化编码.中文 (简体)"),
            // dict.sys.culture.code.zh-cn
            ("dict.sys.culture.code.zh-cn", "zh-CN", "中文(简体)", "区域文化编码.中文 (简体)"),
            // dict.sys.culture.code.zh-cn
            ("dict.sys.culture.code.zh-cn", "zh-HK", "中文(简体)", "区域文化编码.中文 (简体)"),

            // dict.sys.culture.code.zh-hk
            ("dict.sys.culture.code.zh-hk", "en-US", "中文(香港)", "区域文化编码.中文(香港)"),
            // dict.sys.culture.code.zh-hk
            ("dict.sys.culture.code.zh-hk", "ja-JP", "中文(香港)", "区域文化编码.中文(香港)"),
            // dict.sys.culture.code.zh-hk
            ("dict.sys.culture.code.zh-hk", "zh-CN", "中文(香港)", "区域文化编码.中文(香港)"),
            // dict.sys.culture.code.zh-hk
            ("dict.sys.culture.code.zh-hk", "zh-HK", "中文(香港)", "区域文化编码.中文(香港)"),

            // dict.sys.culture.code.zh-mo
            ("dict.sys.culture.code.zh-mo", "en-US", "中文(澳门)", "区域文化编码.中文(澳门)"),
            // dict.sys.culture.code.zh-mo
            ("dict.sys.culture.code.zh-mo", "ja-JP", "中文(澳门)", "区域文化编码.中文(澳门)"),
            // dict.sys.culture.code.zh-mo
            ("dict.sys.culture.code.zh-mo", "zh-CN", "中文(澳门)", "区域文化编码.中文(澳门)"),
            // dict.sys.culture.code.zh-mo
            ("dict.sys.culture.code.zh-mo", "zh-HK", "中文(澳门)", "区域文化编码.中文(澳门)"),

            // dict.sys.culture.code.zh-sg
            ("dict.sys.culture.code.zh-sg", "en-US", "中文(新加坡)", "区域文化编码.中文(新加坡)"),
            // dict.sys.culture.code.zh-sg
            ("dict.sys.culture.code.zh-sg", "ja-JP", "中文(新加坡)", "区域文化编码.中文(新加坡)"),
            // dict.sys.culture.code.zh-sg
            ("dict.sys.culture.code.zh-sg", "zh-CN", "中文(新加坡)", "区域文化编码.中文(新加坡)"),
            // dict.sys.culture.code.zh-sg
            ("dict.sys.culture.code.zh-sg", "zh-HK", "中文(新加坡)", "区域文化编码.中文(新加坡)"),

            // dict.sys.culture.code.zh-tw
            ("dict.sys.culture.code.zh-tw", "en-US", "中文(繁體)", "区域文化编码.中文(繁體)"),
            // dict.sys.culture.code.zh-tw
            ("dict.sys.culture.code.zh-tw", "ja-JP", "中文(繁體)", "区域文化编码.中文(繁體)"),
            // dict.sys.culture.code.zh-tw
            ("dict.sys.culture.code.zh-tw", "zh-CN", "中文(繁體)", "区域文化编码.中文(繁體)"),
            // dict.sys.culture.code.zh-tw
            ("dict.sys.culture.code.zh-tw", "zh-HK", "中文(繁體)", "区域文化编码.中文(繁體)"),

            // dict.sys.data.scope.type.0
            ("dict.sys.data.scope.type.0", "en-US", "全部数据_us", "数据权限.全部数据"),
            // dict.sys.data.scope.type.0
            ("dict.sys.data.scope.type.0", "ja-JP", "全部数据_jp", "数据权限.全部数据"),
            // dict.sys.data.scope.type.0
            ("dict.sys.data.scope.type.0", "zh-CN", "全部数据", "数据权限.全部数据"),
            // dict.sys.data.scope.type.0
            ("dict.sys.data.scope.type.0", "zh-HK", "全部数据_hk", "数据权限.全部数据"),

            // dict.sys.data.scope.type.1
            ("dict.sys.data.scope.type.1", "en-US", "本部门数据_us", "数据权限.本部门数据"),
            // dict.sys.data.scope.type.1
            ("dict.sys.data.scope.type.1", "ja-JP", "本部门数据_jp", "数据权限.本部门数据"),
            // dict.sys.data.scope.type.1
            ("dict.sys.data.scope.type.1", "zh-CN", "本部门数据", "数据权限.本部门数据"),
            // dict.sys.data.scope.type.1
            ("dict.sys.data.scope.type.1", "zh-HK", "本部门数据_hk", "数据权限.本部门数据"),

            // dict.sys.data.scope.type.2
            ("dict.sys.data.scope.type.2", "en-US", "本部门及以下数据_us", "数据权限.本部门及以下数据"),
            // dict.sys.data.scope.type.2
            ("dict.sys.data.scope.type.2", "ja-JP", "本部门及以下数据_jp", "数据权限.本部门及以下数据"),
            // dict.sys.data.scope.type.2
            ("dict.sys.data.scope.type.2", "zh-CN", "本部门及以下数据", "数据权限.本部门及以下数据"),
            // dict.sys.data.scope.type.2
            ("dict.sys.data.scope.type.2", "zh-HK", "本部门及以下数据_hk", "数据权限.本部门及以下数据"),

            // dict.sys.data.scope.type.3
            ("dict.sys.data.scope.type.3", "en-US", "仅本人数据_us", "数据权限.仅本人数据"),
            // dict.sys.data.scope.type.3
            ("dict.sys.data.scope.type.3", "ja-JP", "仅本人数据_jp", "数据权限.仅本人数据"),
            // dict.sys.data.scope.type.3
            ("dict.sys.data.scope.type.3", "zh-CN", "仅本人数据", "数据权限.仅本人数据"),
            // dict.sys.data.scope.type.3
            ("dict.sys.data.scope.type.3", "zh-HK", "仅本人数据_hk", "数据权限.仅本人数据"),

            // dict.sys.data.scope.type.4
            ("dict.sys.data.scope.type.4", "en-US", "自定义数据范围_us", "数据权限.自定义数据范围"),
            // dict.sys.data.scope.type.4
            ("dict.sys.data.scope.type.4", "ja-JP", "自定义数据范围_jp", "数据权限.自定义数据范围"),
            // dict.sys.data.scope.type.4
            ("dict.sys.data.scope.type.4", "zh-CN", "自定义数据范围", "数据权限.自定义数据范围"),
            // dict.sys.data.scope.type.4
            ("dict.sys.data.scope.type.4", "zh-HK", "自定义数据范围_hk", "数据权限.自定义数据范围"),

            // dict.sys.data.source.type.0
            ("dict.sys.data.source.type.0", "en-US", "系统表_us", "数据源.系统表"),
            // dict.sys.data.source.type.0
            ("dict.sys.data.source.type.0", "ja-JP", "系统表_jp", "数据源.系统表"),
            // dict.sys.data.source.type.0
            ("dict.sys.data.source.type.0", "zh-CN", "系统表", "数据源.系统表"),
            // dict.sys.data.source.type.0
            ("dict.sys.data.source.type.0", "zh-HK", "系统表_hk", "数据源.系统表"),

            // dict.sys.data.source.type.1
            ("dict.sys.data.source.type.1", "en-US", "sql查询_us", "数据源.sql查询"),
            // dict.sys.data.source.type.1
            ("dict.sys.data.source.type.1", "ja-JP", "sql查询_jp", "数据源.sql查询"),
            // dict.sys.data.source.type.1
            ("dict.sys.data.source.type.1", "zh-CN", "sql查询", "数据源.sql查询"),
            // dict.sys.data.source.type.1
            ("dict.sys.data.source.type.1", "zh-HK", "sql查询_hk", "数据源.sql查询"),

            // dict.sys.db.data.type.bigint
            ("dict.sys.db.data.type.bigint", "en-US", "bigint_us", "数据库数据类型.bigint"),
            // dict.sys.db.data.type.bigint
            ("dict.sys.db.data.type.bigint", "ja-JP", "bigint_jp", "数据库数据类型.bigint"),
            // dict.sys.db.data.type.bigint
            ("dict.sys.db.data.type.bigint", "zh-CN", "bigint", "数据库数据类型.bigint"),
            // dict.sys.db.data.type.bigint
            ("dict.sys.db.data.type.bigint", "zh-HK", "bigint_hk", "数据库数据类型.bigint"),

            // dict.sys.db.data.type.bit
            ("dict.sys.db.data.type.bit", "en-US", "bit_us", "数据库数据类型.bit"),
            // dict.sys.db.data.type.bit
            ("dict.sys.db.data.type.bit", "ja-JP", "bit_jp", "数据库数据类型.bit"),
            // dict.sys.db.data.type.bit
            ("dict.sys.db.data.type.bit", "zh-CN", "bit", "数据库数据类型.bit"),
            // dict.sys.db.data.type.bit
            ("dict.sys.db.data.type.bit", "zh-HK", "bit_hk", "数据库数据类型.bit"),

            // dict.sys.db.data.type.datetime
            ("dict.sys.db.data.type.datetime", "en-US", "datetime_us", "数据库数据类型.datetime"),
            // dict.sys.db.data.type.datetime
            ("dict.sys.db.data.type.datetime", "ja-JP", "datetime_jp", "数据库数据类型.datetime"),
            // dict.sys.db.data.type.datetime
            ("dict.sys.db.data.type.datetime", "zh-CN", "datetime", "数据库数据类型.datetime"),
            // dict.sys.db.data.type.datetime
            ("dict.sys.db.data.type.datetime", "zh-HK", "datetime_hk", "数据库数据类型.datetime"),

            // dict.sys.db.data.type.decimal
            ("dict.sys.db.data.type.decimal", "en-US", "decimal_us", "数据库数据类型.decimal"),
            // dict.sys.db.data.type.decimal
            ("dict.sys.db.data.type.decimal", "ja-JP", "decimal_jp", "数据库数据类型.decimal"),
            // dict.sys.db.data.type.decimal
            ("dict.sys.db.data.type.decimal", "zh-CN", "decimal", "数据库数据类型.decimal"),
            // dict.sys.db.data.type.decimal
            ("dict.sys.db.data.type.decimal", "zh-HK", "decimal_hk", "数据库数据类型.decimal"),

            // dict.sys.db.data.type.int
            ("dict.sys.db.data.type.int", "en-US", "int_us", "数据库数据类型.int"),
            // dict.sys.db.data.type.int
            ("dict.sys.db.data.type.int", "ja-JP", "int_jp", "数据库数据类型.int"),
            // dict.sys.db.data.type.int
            ("dict.sys.db.data.type.int", "zh-CN", "int", "数据库数据类型.int"),
            // dict.sys.db.data.type.int
            ("dict.sys.db.data.type.int", "zh-HK", "int_hk", "数据库数据类型.int"),

            // dict.sys.db.data.type.ntext
            ("dict.sys.db.data.type.ntext", "en-US", "ntext_us", "数据库数据类型.ntext"),
            // dict.sys.db.data.type.ntext
            ("dict.sys.db.data.type.ntext", "ja-JP", "ntext_jp", "数据库数据类型.ntext"),
            // dict.sys.db.data.type.ntext
            ("dict.sys.db.data.type.ntext", "zh-CN", "ntext", "数据库数据类型.ntext"),
            // dict.sys.db.data.type.ntext
            ("dict.sys.db.data.type.ntext", "zh-HK", "ntext_hk", "数据库数据类型.ntext"),

            // dict.sys.db.data.type.nvarchar
            ("dict.sys.db.data.type.nvarchar", "en-US", "nvarchar_us", "数据库数据类型.nvarchar"),
            // dict.sys.db.data.type.nvarchar
            ("dict.sys.db.data.type.nvarchar", "ja-JP", "nvarchar_jp", "数据库数据类型.nvarchar"),
            // dict.sys.db.data.type.nvarchar
            ("dict.sys.db.data.type.nvarchar", "zh-CN", "nvarchar", "数据库数据类型.nvarchar"),
            // dict.sys.db.data.type.nvarchar
            ("dict.sys.db.data.type.nvarchar", "zh-HK", "nvarchar_hk", "数据库数据类型.nvarchar"),

            // dict.sys.db.data.type.text
            ("dict.sys.db.data.type.text", "en-US", "text_us", "数据库数据类型.text"),
            // dict.sys.db.data.type.text
            ("dict.sys.db.data.type.text", "ja-JP", "text_jp", "数据库数据类型.text"),
            // dict.sys.db.data.type.text
            ("dict.sys.db.data.type.text", "zh-CN", "text", "数据库数据类型.text"),
            // dict.sys.db.data.type.text
            ("dict.sys.db.data.type.text", "zh-HK", "text_hk", "数据库数据类型.text"),

            // dict.sys.db.data.type.uniqueidentifier
            ("dict.sys.db.data.type.uniqueidentifier", "en-US", "uniqueidentifier_us", "数据库数据类型.uniqueidentifier"),
            // dict.sys.db.data.type.uniqueidentifier
            ("dict.sys.db.data.type.uniqueidentifier", "ja-JP", "uniqueidentifier_jp", "数据库数据类型.uniqueidentifier"),
            // dict.sys.db.data.type.uniqueidentifier
            ("dict.sys.db.data.type.uniqueidentifier", "zh-CN", "uniqueidentifier", "数据库数据类型.uniqueidentifier"),
            // dict.sys.db.data.type.uniqueidentifier
            ("dict.sys.db.data.type.uniqueidentifier", "zh-HK", "uniqueidentifier_hk", "数据库数据类型.uniqueidentifier"),

            // dict.sys.db.data.type.varchar
            ("dict.sys.db.data.type.varchar", "en-US", "varchar_us", "数据库数据类型.varchar"),
            // dict.sys.db.data.type.varchar
            ("dict.sys.db.data.type.varchar", "ja-JP", "varchar_jp", "数据库数据类型.varchar"),
            // dict.sys.db.data.type.varchar
            ("dict.sys.db.data.type.varchar", "zh-CN", "varchar", "数据库数据类型.varchar"),
            // dict.sys.db.data.type.varchar
            ("dict.sys.db.data.type.varchar", "zh-HK", "varchar_hk", "数据库数据类型.varchar"),

            // dict.sys.dept.type.0
            ("dict.sys.dept.type.0", "en-US", "直接_us", "部门类型.直接"),
            // dict.sys.dept.type.0
            ("dict.sys.dept.type.0", "ja-JP", "直接_jp", "部门类型.直接"),
            // dict.sys.dept.type.0
            ("dict.sys.dept.type.0", "zh-CN", "直接", "部门类型.直接"),
            // dict.sys.dept.type.0
            ("dict.sys.dept.type.0", "zh-HK", "直接_hk", "部门类型.直接"),

            // dict.sys.dept.type.1
            ("dict.sys.dept.type.1", "en-US", "间接_us", "部门类型.间接"),
            // dict.sys.dept.type.1
            ("dict.sys.dept.type.1", "ja-JP", "间接_jp", "部门类型.间接"),
            // dict.sys.dept.type.1
            ("dict.sys.dept.type.1", "zh-CN", "间接", "部门类型.间接"),
            // dict.sys.dept.type.1
            ("dict.sys.dept.type.1", "zh-HK", "间接_hk", "部门类型.间接"),

            // dict.sys.flow.category.0
            ("dict.sys.flow.category.0", "en-US", "通用流程_us", "流程分类.通用流程"),
            // dict.sys.flow.category.0
            ("dict.sys.flow.category.0", "ja-JP", "通用流程_jp", "流程分类.通用流程"),
            // dict.sys.flow.category.0
            ("dict.sys.flow.category.0", "zh-CN", "通用流程", "流程分类.通用流程"),
            // dict.sys.flow.category.0
            ("dict.sys.flow.category.0", "zh-HK", "通用流程_hk", "流程分类.通用流程"),

            // dict.sys.flow.category.1
            ("dict.sys.flow.category.1", "en-US", "业务流程_us", "流程分类.业务流程"),
            // dict.sys.flow.category.1
            ("dict.sys.flow.category.1", "ja-JP", "业务流程_jp", "流程分类.业务流程"),
            // dict.sys.flow.category.1
            ("dict.sys.flow.category.1", "zh-CN", "业务流程", "流程分类.业务流程"),
            // dict.sys.flow.category.1
            ("dict.sys.flow.category.1", "zh-HK", "业务流程_hk", "流程分类.业务流程"),

            // dict.sys.flow.category.2
            ("dict.sys.flow.category.2", "en-US", "系统流程_us", "流程分类.系统流程"),
            // dict.sys.flow.category.2
            ("dict.sys.flow.category.2", "ja-JP", "系统流程_jp", "流程分类.系统流程"),
            // dict.sys.flow.category.2
            ("dict.sys.flow.category.2", "zh-CN", "系统流程", "流程分类.系统流程"),
            // dict.sys.flow.category.2
            ("dict.sys.flow.category.2", "zh-HK", "系统流程_hk", "流程分类.系统流程"),

            // dict.sys.flow.status.0
            ("dict.sys.flow.status.0", "en-US", "运行中_us", "流程状态.运行中"),
            // dict.sys.flow.status.0
            ("dict.sys.flow.status.0", "ja-JP", "运行中_jp", "流程状态.运行中"),
            // dict.sys.flow.status.0
            ("dict.sys.flow.status.0", "zh-CN", "运行中", "流程状态.运行中"),
            // dict.sys.flow.status.0
            ("dict.sys.flow.status.0", "zh-HK", "运行中_hk", "流程状态.运行中"),

            // dict.sys.flow.status.1
            ("dict.sys.flow.status.1", "en-US", "已完成_us", "流程状态.已完成"),
            // dict.sys.flow.status.1
            ("dict.sys.flow.status.1", "ja-JP", "已完成_jp", "流程状态.已完成"),
            // dict.sys.flow.status.1
            ("dict.sys.flow.status.1", "zh-CN", "已完成", "流程状态.已完成"),
            // dict.sys.flow.status.1
            ("dict.sys.flow.status.1", "zh-HK", "已完成_hk", "流程状态.已完成"),

            // dict.sys.flow.status.2
            ("dict.sys.flow.status.2", "en-US", "已终止_us", "流程状态.已终止"),
            // dict.sys.flow.status.2
            ("dict.sys.flow.status.2", "ja-JP", "已终止_jp", "流程状态.已终止"),
            // dict.sys.flow.status.2
            ("dict.sys.flow.status.2", "zh-CN", "已终止", "流程状态.已终止"),
            // dict.sys.flow.status.2
            ("dict.sys.flow.status.2", "zh-HK", "已终止_hk", "流程状态.已终止"),

            // dict.sys.flow.status.3
            ("dict.sys.flow.status.3", "en-US", "已挂起_us", "流程状态.已挂起"),
            // dict.sys.flow.status.3
            ("dict.sys.flow.status.3", "ja-JP", "已挂起_jp", "流程状态.已挂起"),
            // dict.sys.flow.status.3
            ("dict.sys.flow.status.3", "zh-CN", "已挂起", "流程状态.已挂起"),
            // dict.sys.flow.status.3
            ("dict.sys.flow.status.3", "zh-HK", "已挂起_hk", "流程状态.已挂起"),

            // dict.sys.flow.status.4
            ("dict.sys.flow.status.4", "en-US", "已撤回_us", "流程状态.已撤回"),
            // dict.sys.flow.status.4
            ("dict.sys.flow.status.4", "ja-JP", "已撤回_jp", "流程状态.已撤回"),
            // dict.sys.flow.status.4
            ("dict.sys.flow.status.4", "zh-CN", "已撤回", "流程状态.已撤回"),
            // dict.sys.flow.status.4
            ("dict.sys.flow.status.4", "zh-HK", "已撤回_hk", "流程状态.已撤回"),

            // dict.sys.flow.status.5
            ("dict.sys.flow.status.5", "en-US", "草稿_us", "流程状态.草稿"),
            // dict.sys.flow.status.5
            ("dict.sys.flow.status.5", "ja-JP", "草稿_jp", "流程状态.草稿"),
            // dict.sys.flow.status.5
            ("dict.sys.flow.status.5", "zh-CN", "草稿", "流程状态.草稿"),
            // dict.sys.flow.status.5
            ("dict.sys.flow.status.5", "zh-HK", "草稿_hk", "流程状态.草稿"),

            // dict.sys.form.category.0
            ("dict.sys.form.category.0", "en-US", "通用表单_us", "表单分类.通用表单"),
            // dict.sys.form.category.0
            ("dict.sys.form.category.0", "ja-JP", "通用表单_jp", "表单分类.通用表单"),
            // dict.sys.form.category.0
            ("dict.sys.form.category.0", "zh-CN", "通用表单", "表单分类.通用表单"),
            // dict.sys.form.category.0
            ("dict.sys.form.category.0", "zh-HK", "通用表单_hk", "表单分类.通用表单"),

            // dict.sys.form.category.1
            ("dict.sys.form.category.1", "en-US", "业务表单_us", "表单分类.业务表单"),
            // dict.sys.form.category.1
            ("dict.sys.form.category.1", "ja-JP", "业务表单_jp", "表单分类.业务表单"),
            // dict.sys.form.category.1
            ("dict.sys.form.category.1", "zh-CN", "业务表单", "表单分类.业务表单"),
            // dict.sys.form.category.1
            ("dict.sys.form.category.1", "zh-HK", "业务表单_hk", "表单分类.业务表单"),

            // dict.sys.form.category.2
            ("dict.sys.form.category.2", "en-US", "系统表单_us", "表单分类.系统表单"),
            // dict.sys.form.category.2
            ("dict.sys.form.category.2", "ja-JP", "系统表单_jp", "表单分类.系统表单"),
            // dict.sys.form.category.2
            ("dict.sys.form.category.2", "zh-CN", "系统表单", "表单分类.系统表单"),
            // dict.sys.form.category.2
            ("dict.sys.form.category.2", "zh-HK", "系统表单_hk", "表单分类.系统表单"),

            // dict.sys.form.type.0
            ("dict.sys.form.type.0", "en-US", "动态表单_us", "表单类型.动态表单"),
            // dict.sys.form.type.0
            ("dict.sys.form.type.0", "ja-JP", "动态表单_jp", "表单类型.动态表单"),
            // dict.sys.form.type.0
            ("dict.sys.form.type.0", "zh-CN", "动态表单", "表单类型.动态表单"),
            // dict.sys.form.type.0
            ("dict.sys.form.type.0", "zh-HK", "动态表单_hk", "表单类型.动态表单"),

            // dict.sys.form.type.1
            ("dict.sys.form.type.1", "en-US", "静态表单_us", "表单类型.静态表单"),
            // dict.sys.form.type.1
            ("dict.sys.form.type.1", "ja-JP", "静态表单_jp", "表单类型.静态表单"),
            // dict.sys.form.type.1
            ("dict.sys.form.type.1", "zh-CN", "静态表单", "表单类型.静态表单"),
            // dict.sys.form.type.1
            ("dict.sys.form.type.1", "zh-HK", "静态表单_hk", "表单类型.静态表单"),

            // dict.sys.form.type.2
            ("dict.sys.form.type.2", "en-US", "自定义表单_us", "表单类型.自定义表单"),
            // dict.sys.form.type.2
            ("dict.sys.form.type.2", "ja-JP", "自定义表单_jp", "表单类型.自定义表单"),
            // dict.sys.form.type.2
            ("dict.sys.form.type.2", "zh-CN", "自定义表单", "表单类型.自定义表单"),
            // dict.sys.form.type.2
            ("dict.sys.form.type.2", "zh-HK", "自定义表单_hk", "表单类型.自定义表单"),

            // dict.sys.ftp.provider.type.teac_cn
            ("dict.sys.ftp.provider.type.teac_cn", "en-US", "teac ftp中国_us", "ftp服务提供商.teac ftp中国"),
            // dict.sys.ftp.provider.type.teac_cn
            ("dict.sys.ftp.provider.type.teac_cn", "ja-JP", "teac ftp中国_jp", "ftp服务提供商.teac ftp中国"),
            // dict.sys.ftp.provider.type.teac_cn
            ("dict.sys.ftp.provider.type.teac_cn", "zh-CN", "teac ftp中国", "ftp服务提供商.teac ftp中国"),
            // dict.sys.ftp.provider.type.teac_cn
            ("dict.sys.ftp.provider.type.teac_cn", "zh-HK", "teac ftp中国_hk", "ftp服务提供商.teac ftp中国"),

            // dict.sys.ftp.provider.type.teac_jp
            ("dict.sys.ftp.provider.type.teac_jp", "en-US", "teac ftp日本_us", "ftp服务提供商.teac ftp日本"),
            // dict.sys.ftp.provider.type.teac_jp
            ("dict.sys.ftp.provider.type.teac_jp", "ja-JP", "teac ftp日本_jp", "ftp服务提供商.teac ftp日本"),
            // dict.sys.ftp.provider.type.teac_jp
            ("dict.sys.ftp.provider.type.teac_jp", "zh-CN", "teac ftp日本", "ftp服务提供商.teac ftp日本"),
            // dict.sys.ftp.provider.type.teac_jp
            ("dict.sys.ftp.provider.type.teac_jp", "zh-HK", "teac ftp日本_hk", "ftp服务提供商.teac ftp日本"),

            // dict.sys.is.builtin.type.1
            ("dict.sys.is.builtin.type.1", "en-US", "是_us", "是否内置.是"),
            // dict.sys.is.builtin.type.1
            ("dict.sys.is.builtin.type.1", "ja-JP", "是_jp", "是否内置.是"),
            // dict.sys.is.builtin.type.1
            ("dict.sys.is.builtin.type.1", "zh-CN", "是", "是否内置.是"),
            // dict.sys.is.builtin.type.1
            ("dict.sys.is.builtin.type.1", "zh-HK", "是_hk", "是否内置.是"),

            // dict.sys.is.builtin.type.0
            ("dict.sys.is.builtin.type.0", "en-US", "否_us", "是否内置.否"),
            // dict.sys.is.builtin.type.0
            ("dict.sys.is.builtin.type.0", "ja-JP", "否_jp", "是否内置.否"),
            // dict.sys.is.builtin.type.0
            ("dict.sys.is.builtin.type.0", "zh-CN", "否", "是否内置.否"),
            // dict.sys.is.builtin.type.0
            ("dict.sys.is.builtin.type.0", "zh-HK", "否_hk", "是否内置.否"),

            // dict.sys.is.default.type.1
            ("dict.sys.is.default.type.1", "en-US", "是_us", "是否默认.是"),
            // dict.sys.is.default.type.1
            ("dict.sys.is.default.type.1", "ja-JP", "是_jp", "是否默认.是"),
            // dict.sys.is.default.type.1
            ("dict.sys.is.default.type.1", "zh-CN", "是", "是否默认.是"),
            // dict.sys.is.default.type.1
            ("dict.sys.is.default.type.1", "zh-HK", "是_hk", "是否默认.是"),

            // dict.sys.is.default.type.0
            ("dict.sys.is.default.type.0", "en-US", "否_us", "是否默认.否"),
            // dict.sys.is.default.type.0
            ("dict.sys.is.default.type.0", "ja-JP", "否_jp", "是否默认.否"),
            // dict.sys.is.default.type.0
            ("dict.sys.is.default.type.0", "zh-CN", "否", "是否默认.否"),
            // dict.sys.is.default.type.0
            ("dict.sys.is.default.type.0", "zh-HK", "否_hk", "是否默认.否"),

            // dict.sys.is.public.type.0
            ("dict.sys.is.public.type.0", "en-US", "公开_us", "公开.公开"),
            // dict.sys.is.public.type.0
            ("dict.sys.is.public.type.0", "ja-JP", "公开_jp", "公开.公开"),
            // dict.sys.is.public.type.0
            ("dict.sys.is.public.type.0", "zh-CN", "公开", "公开.公开"),
            // dict.sys.is.public.type.0
            ("dict.sys.is.public.type.0", "zh-HK", "公开_hk", "公开.公开"),

            // dict.sys.is.public.type.1
            ("dict.sys.is.public.type.1", "en-US", "私有_us", "公开.私有"),
            // dict.sys.is.public.type.1
            ("dict.sys.is.public.type.1", "ja-JP", "私有_jp", "公开.私有"),
            // dict.sys.is.public.type.1
            ("dict.sys.is.public.type.1", "zh-CN", "私有", "公开.私有"),
            // dict.sys.is.public.type.1
            ("dict.sys.is.public.type.1", "zh-HK", "私有_hk", "公开.私有"),

            // dict.sys.leave.type.affair
            ("dict.sys.leave.type.affair", "en-US", "事假_us", "请假类型.事假"),
            // dict.sys.leave.type.affair
            ("dict.sys.leave.type.affair", "ja-JP", "事假_jp", "请假类型.事假"),
            // dict.sys.leave.type.affair
            ("dict.sys.leave.type.affair", "zh-CN", "事假", "请假类型.事假"),
            // dict.sys.leave.type.affair
            ("dict.sys.leave.type.affair", "zh-HK", "事假_hk", "请假类型.事假"),

            // dict.sys.leave.type.sick
            ("dict.sys.leave.type.sick", "en-US", "病假_us", "请假类型.病假"),
            // dict.sys.leave.type.sick
            ("dict.sys.leave.type.sick", "ja-JP", "病假_jp", "请假类型.病假"),
            // dict.sys.leave.type.sick
            ("dict.sys.leave.type.sick", "zh-CN", "病假", "请假类型.病假"),
            // dict.sys.leave.type.sick
            ("dict.sys.leave.type.sick", "zh-HK", "病假_hk", "请假类型.病假"),

            // dict.sys.leave.type.annual
            ("dict.sys.leave.type.annual", "en-US", "年假_us", "请假类型.年假"),
            // dict.sys.leave.type.annual
            ("dict.sys.leave.type.annual", "ja-JP", "年假_jp", "请假类型.年假"),
            // dict.sys.leave.type.annual
            ("dict.sys.leave.type.annual", "zh-CN", "年假", "请假类型.年假"),
            // dict.sys.leave.type.annual
            ("dict.sys.leave.type.annual", "zh-HK", "年假_hk", "请假类型.年假"),

            // dict.sys.leave.type.marriage
            ("dict.sys.leave.type.marriage", "en-US", "婚假_us", "请假类型.婚假"),
            // dict.sys.leave.type.marriage
            ("dict.sys.leave.type.marriage", "ja-JP", "婚假_jp", "请假类型.婚假"),
            // dict.sys.leave.type.marriage
            ("dict.sys.leave.type.marriage", "zh-CN", "婚假", "请假类型.婚假"),
            // dict.sys.leave.type.marriage
            ("dict.sys.leave.type.marriage", "zh-HK", "婚假_hk", "请假类型.婚假"),

            // dict.sys.leave.type.maternity
            ("dict.sys.leave.type.maternity", "en-US", "产假_us", "请假类型.产假"),
            // dict.sys.leave.type.maternity
            ("dict.sys.leave.type.maternity", "ja-JP", "产假_jp", "请假类型.产假"),
            // dict.sys.leave.type.maternity
            ("dict.sys.leave.type.maternity", "zh-CN", "产假", "请假类型.产假"),
            // dict.sys.leave.type.maternity
            ("dict.sys.leave.type.maternity", "zh-HK", "产假_hk", "请假类型.产假"),

            // dict.sys.leave.type.paternity
            ("dict.sys.leave.type.paternity", "en-US", "陪产假_us", "请假类型.陪产假"),
            // dict.sys.leave.type.paternity
            ("dict.sys.leave.type.paternity", "ja-JP", "陪产假_jp", "请假类型.陪产假"),
            // dict.sys.leave.type.paternity
            ("dict.sys.leave.type.paternity", "zh-CN", "陪产假", "请假类型.陪产假"),
            // dict.sys.leave.type.paternity
            ("dict.sys.leave.type.paternity", "zh-HK", "陪产假_hk", "请假类型.陪产假"),

            // dict.sys.leave.type.bereavement
            ("dict.sys.leave.type.bereavement", "en-US", "丧假_us", "请假类型.丧假"),
            // dict.sys.leave.type.bereavement
            ("dict.sys.leave.type.bereavement", "ja-JP", "丧假_jp", "请假类型.丧假"),
            // dict.sys.leave.type.bereavement
            ("dict.sys.leave.type.bereavement", "zh-CN", "丧假", "请假类型.丧假"),
            // dict.sys.leave.type.bereavement
            ("dict.sys.leave.type.bereavement", "zh-HK", "丧假_hk", "请假类型.丧假"),

            // dict.sys.leave.type.compensatory
            ("dict.sys.leave.type.compensatory", "en-US", "调休_us", "请假类型.调休"),
            // dict.sys.leave.type.compensatory
            ("dict.sys.leave.type.compensatory", "ja-JP", "调休_jp", "请假类型.调休"),
            // dict.sys.leave.type.compensatory
            ("dict.sys.leave.type.compensatory", "zh-CN", "调休", "请假类型.调休"),
            // dict.sys.leave.type.compensatory
            ("dict.sys.leave.type.compensatory", "zh-HK", "调休_hk", "请假类型.调休"),

            // dict.sys.leave.type.personal
            ("dict.sys.leave.type.personal", "en-US", "私假_us", "请假类型.私假"),
            // dict.sys.leave.type.personal
            ("dict.sys.leave.type.personal", "ja-JP", "私假_jp", "请假类型.私假"),
            // dict.sys.leave.type.personal
            ("dict.sys.leave.type.personal", "zh-CN", "私假", "请假类型.私假"),
            // dict.sys.leave.type.personal
            ("dict.sys.leave.type.personal", "zh-HK", "私假_hk", "请假类型.私假"),

            // dict.sys.leave.type.other
            ("dict.sys.leave.type.other", "en-US", "其他_us", "请假类型.其他"),
            // dict.sys.leave.type.other
            ("dict.sys.leave.type.other", "ja-JP", "其他_jp", "请假类型.其他"),
            // dict.sys.leave.type.other
            ("dict.sys.leave.type.other", "zh-CN", "其他", "请假类型.其他"),
            // dict.sys.leave.type.other
            ("dict.sys.leave.type.other", "zh-HK", "其他_hk", "请假类型.其他"),

            // dict.sys.mail.status.0
            ("dict.sys.mail.status.0", "en-US", "草稿_us", "邮件状态.草稿"),
            // dict.sys.mail.status.0
            ("dict.sys.mail.status.0", "ja-JP", "草稿_jp", "邮件状态.草稿"),
            // dict.sys.mail.status.0
            ("dict.sys.mail.status.0", "zh-CN", "草稿", "邮件状态.草稿"),
            // dict.sys.mail.status.0
            ("dict.sys.mail.status.0", "zh-HK", "草稿_hk", "邮件状态.草稿"),

            // dict.sys.mail.status.1
            ("dict.sys.mail.status.1", "en-US", "已发送_us", "邮件状态.已发送"),
            // dict.sys.mail.status.1
            ("dict.sys.mail.status.1", "ja-JP", "已发送_jp", "邮件状态.已发送"),
            // dict.sys.mail.status.1
            ("dict.sys.mail.status.1", "zh-CN", "已发送", "邮件状态.已发送"),
            // dict.sys.mail.status.1
            ("dict.sys.mail.status.1", "zh-HK", "已发送_hk", "邮件状态.已发送"),

            // dict.sys.mail.status.2
            ("dict.sys.mail.status.2", "en-US", "发送失败_us", "邮件状态.发送失败"),
            // dict.sys.mail.status.2
            ("dict.sys.mail.status.2", "ja-JP", "发送失败_jp", "邮件状态.发送失败"),
            // dict.sys.mail.status.2
            ("dict.sys.mail.status.2", "zh-CN", "发送失败", "邮件状态.发送失败"),
            // dict.sys.mail.status.2
            ("dict.sys.mail.status.2", "zh-HK", "发送失败_hk", "邮件状态.发送失败"),

            // dict.sys.mail.status.3
            ("dict.sys.mail.status.3", "en-US", "已撤回_us", "邮件状态.已撤回"),
            // dict.sys.mail.status.3
            ("dict.sys.mail.status.3", "ja-JP", "已撤回_jp", "邮件状态.已撤回"),
            // dict.sys.mail.status.3
            ("dict.sys.mail.status.3", "zh-CN", "已撤回", "邮件状态.已撤回"),
            // dict.sys.mail.status.3
            ("dict.sys.mail.status.3", "zh-HK", "已撤回_hk", "邮件状态.已撤回"),

            // dict.sys.mail.status.4
            ("dict.sys.mail.status.4", "en-US", "定时发送中_us", "邮件状态.定时发送中"),
            // dict.sys.mail.status.4
            ("dict.sys.mail.status.4", "ja-JP", "定时发送中_jp", "邮件状态.定时发送中"),
            // dict.sys.mail.status.4
            ("dict.sys.mail.status.4", "zh-CN", "定时发送中", "邮件状态.定时发送中"),
            // dict.sys.mail.status.4
            ("dict.sys.mail.status.4", "zh-HK", "定时发送中_hk", "邮件状态.定时发送中"),

            // dict.sys.mail.type.0
            ("dict.sys.mail.type.0", "en-US", "普通邮件_us", "邮件类型.普通邮件"),
            // dict.sys.mail.type.0
            ("dict.sys.mail.type.0", "ja-JP", "普通邮件_jp", "邮件类型.普通邮件"),
            // dict.sys.mail.type.0
            ("dict.sys.mail.type.0", "zh-CN", "普通邮件", "邮件类型.普通邮件"),
            // dict.sys.mail.type.0
            ("dict.sys.mail.type.0", "zh-HK", "普通邮件_hk", "邮件类型.普通邮件"),

            // dict.sys.mail.type.1
            ("dict.sys.mail.type.1", "en-US", "系统邮件_us", "邮件类型.系统邮件"),
            // dict.sys.mail.type.1
            ("dict.sys.mail.type.1", "ja-JP", "系统邮件_jp", "邮件类型.系统邮件"),
            // dict.sys.mail.type.1
            ("dict.sys.mail.type.1", "zh-CN", "系统邮件", "邮件类型.系统邮件"),
            // dict.sys.mail.type.1
            ("dict.sys.mail.type.1", "zh-HK", "系统邮件_hk", "邮件类型.系统邮件"),

            // dict.sys.mail.type.2
            ("dict.sys.mail.type.2", "en-US", "通知邮件_us", "邮件类型.通知邮件"),
            // dict.sys.mail.type.2
            ("dict.sys.mail.type.2", "ja-JP", "通知邮件_jp", "邮件类型.通知邮件"),
            // dict.sys.mail.type.2
            ("dict.sys.mail.type.2", "zh-CN", "通知邮件", "邮件类型.通知邮件"),
            // dict.sys.mail.type.2
            ("dict.sys.mail.type.2", "zh-HK", "通知邮件_hk", "邮件类型.通知邮件"),

            // dict.sys.mail.type.3
            ("dict.sys.mail.type.3", "en-US", "提醒邮件_us", "邮件类型.提醒邮件"),
            // dict.sys.mail.type.3
            ("dict.sys.mail.type.3", "ja-JP", "提醒邮件_jp", "邮件类型.提醒邮件"),
            // dict.sys.mail.type.3
            ("dict.sys.mail.type.3", "zh-CN", "提醒邮件", "邮件类型.提醒邮件"),
            // dict.sys.mail.type.3
            ("dict.sys.mail.type.3", "zh-HK", "提醒邮件_hk", "邮件类型.提醒邮件"),

            // dict.sys.menu.type.0
            ("dict.sys.menu.type.0", "en-US", "目录_us", "菜单类型.目录"),
            // dict.sys.menu.type.0
            ("dict.sys.menu.type.0", "ja-JP", "目录_jp", "菜单类型.目录"),
            // dict.sys.menu.type.0
            ("dict.sys.menu.type.0", "zh-CN", "目录", "菜单类型.目录"),
            // dict.sys.menu.type.0
            ("dict.sys.menu.type.0", "zh-HK", "目录_hk", "菜单类型.目录"),

            // dict.sys.menu.type.1
            ("dict.sys.menu.type.1", "en-US", "菜单_us", "菜单类型.菜单"),
            // dict.sys.menu.type.1
            ("dict.sys.menu.type.1", "ja-JP", "菜单_jp", "菜单类型.菜单"),
            // dict.sys.menu.type.1
            ("dict.sys.menu.type.1", "zh-CN", "菜单", "菜单类型.菜单"),
            // dict.sys.menu.type.1
            ("dict.sys.menu.type.1", "zh-HK", "菜单_hk", "菜单类型.菜单"),

            // dict.sys.menu.type.2
            ("dict.sys.menu.type.2", "en-US", "按钮_us", "菜单类型.按钮"),
            // dict.sys.menu.type.2
            ("dict.sys.menu.type.2", "ja-JP", "按钮_jp", "菜单类型.按钮"),
            // dict.sys.menu.type.2
            ("dict.sys.menu.type.2", "zh-CN", "按钮", "菜单类型.按钮"),
            // dict.sys.menu.type.2
            ("dict.sys.menu.type.2", "zh-HK", "按钮_hk", "菜单类型.按钮"),

            // dict.sys.message.group.category.collaboration
            ("dict.sys.message.group.category.collaboration", "en-US", "协同_us", "消息分组.协同"),
            // dict.sys.message.group.category.collaboration
            ("dict.sys.message.group.category.collaboration", "ja-JP", "协同_jp", "消息分组.协同"),
            // dict.sys.message.group.category.collaboration
            ("dict.sys.message.group.category.collaboration", "zh-CN", "协同", "消息分组.协同"),
            // dict.sys.message.group.category.collaboration
            ("dict.sys.message.group.category.collaboration", "zh-HK", "协同_hk", "消息分组.协同"),

            // dict.sys.message.group.category.officialdoc
            ("dict.sys.message.group.category.officialdoc", "en-US", "公文_us", "消息分组.公文"),
            // dict.sys.message.group.category.officialdoc
            ("dict.sys.message.group.category.officialdoc", "ja-JP", "公文_jp", "消息分组.公文"),
            // dict.sys.message.group.category.officialdoc
            ("dict.sys.message.group.category.officialdoc", "zh-CN", "公文", "消息分组.公文"),
            // dict.sys.message.group.category.officialdoc
            ("dict.sys.message.group.category.officialdoc", "zh-HK", "公文_hk", "消息分组.公文"),

            // dict.sys.message.group.category.document
            ("dict.sys.message.group.category.document", "en-US", "文档_us", "消息分组.文档"),
            // dict.sys.message.group.category.document
            ("dict.sys.message.group.category.document", "ja-JP", "文档_jp", "消息分组.文档"),
            // dict.sys.message.group.category.document
            ("dict.sys.message.group.category.document", "zh-CN", "文档", "消息分组.文档"),
            // dict.sys.message.group.category.document
            ("dict.sys.message.group.category.document", "zh-HK", "文档_hk", "消息分组.文档"),

            // dict.sys.message.group.category.announcement
            ("dict.sys.message.group.category.announcement", "en-US", "公告_us", "消息分组.公告"),
            // dict.sys.message.group.category.announcement
            ("dict.sys.message.group.category.announcement", "ja-JP", "公告_jp", "消息分组.公告"),
            // dict.sys.message.group.category.announcement
            ("dict.sys.message.group.category.announcement", "zh-CN", "公告", "消息分组.公告"),
            // dict.sys.message.group.category.announcement
            ("dict.sys.message.group.category.announcement", "zh-HK", "公告_hk", "消息分组.公告"),

            // dict.sys.message.group.category.other
            ("dict.sys.message.group.category.other", "en-US", "其他_us", "消息分组.其他"),
            // dict.sys.message.group.category.other
            ("dict.sys.message.group.category.other", "ja-JP", "其他_jp", "消息分组.其他"),
            // dict.sys.message.group.category.other
            ("dict.sys.message.group.category.other", "zh-CN", "其他", "消息分组.其他"),
            // dict.sys.message.group.category.other
            ("dict.sys.message.group.category.other", "zh-HK", "其他_hk", "消息分组.其他"),

            // dict.sys.message.group.category.message
            ("dict.sys.message.group.category.message", "en-US", "消息_us", "消息分组.消息"),
            // dict.sys.message.group.category.message
            ("dict.sys.message.group.category.message", "ja-JP", "消息_jp", "消息分组.消息"),
            // dict.sys.message.group.category.message
            ("dict.sys.message.group.category.message", "zh-CN", "消息", "消息分组.消息"),
            // dict.sys.message.group.category.message
            ("dict.sys.message.group.category.message", "zh-HK", "消息_hk", "消息分组.消息"),

            // dict.sys.message.group.category.reminder
            ("dict.sys.message.group.category.reminder", "en-US", "提醒_us", "消息分组.提醒"),
            // dict.sys.message.group.category.reminder
            ("dict.sys.message.group.category.reminder", "ja-JP", "提醒_jp", "消息分组.提醒"),
            // dict.sys.message.group.category.reminder
            ("dict.sys.message.group.category.reminder", "zh-CN", "提醒", "消息分组.提醒"),
            // dict.sys.message.group.category.reminder
            ("dict.sys.message.group.category.reminder", "zh-HK", "提醒_hk", "消息分组.提醒"),

            // dict.sys.message.type.text
            ("dict.sys.message.type.text", "en-US", "文本_us", "消息类型.文本"),
            // dict.sys.message.type.text
            ("dict.sys.message.type.text", "ja-JP", "文本_jp", "消息类型.文本"),
            // dict.sys.message.type.text
            ("dict.sys.message.type.text", "zh-CN", "文本", "消息类型.文本"),
            // dict.sys.message.type.text
            ("dict.sys.message.type.text", "zh-HK", "文本_hk", "消息类型.文本"),

            // dict.sys.message.type.image
            ("dict.sys.message.type.image", "en-US", "图片_us", "消息类型.图片"),
            // dict.sys.message.type.image
            ("dict.sys.message.type.image", "ja-JP", "图片_jp", "消息类型.图片"),
            // dict.sys.message.type.image
            ("dict.sys.message.type.image", "zh-CN", "图片", "消息类型.图片"),
            // dict.sys.message.type.image
            ("dict.sys.message.type.image", "zh-HK", "图片_hk", "消息类型.图片"),

            // dict.sys.message.type.file
            ("dict.sys.message.type.file", "en-US", "文件_us", "消息类型.文件"),
            // dict.sys.message.type.file
            ("dict.sys.message.type.file", "ja-JP", "文件_jp", "消息类型.文件"),
            // dict.sys.message.type.file
            ("dict.sys.message.type.file", "zh-CN", "文件", "消息类型.文件"),
            // dict.sys.message.type.file
            ("dict.sys.message.type.file", "zh-HK", "文件_hk", "消息类型.文件"),

            // dict.sys.message.type.takt365
            ("dict.sys.message.type.takt365", "en-US", "系统消息_us", "消息类型.系统消息"),
            // dict.sys.message.type.takt365
            ("dict.sys.message.type.takt365", "ja-JP", "系统消息_jp", "消息类型.系统消息"),
            // dict.sys.message.type.takt365
            ("dict.sys.message.type.takt365", "zh-CN", "系统消息", "消息类型.系统消息"),
            // dict.sys.message.type.takt365
            ("dict.sys.message.type.takt365", "zh-HK", "系统消息_hk", "消息类型.系统消息"),

            // dict.sys.message.type.video
            ("dict.sys.message.type.video", "en-US", "视频_us", "消息类型.视频"),
            // dict.sys.message.type.video
            ("dict.sys.message.type.video", "ja-JP", "视频_jp", "消息类型.视频"),
            // dict.sys.message.type.video
            ("dict.sys.message.type.video", "zh-CN", "视频", "消息类型.视频"),
            // dict.sys.message.type.video
            ("dict.sys.message.type.video", "zh-HK", "视频_hk", "消息类型.视频"),

            // dict.sys.message.type.voice
            ("dict.sys.message.type.voice", "en-US", "语音_us", "消息类型.语音"),
            // dict.sys.message.type.voice
            ("dict.sys.message.type.voice", "ja-JP", "语音_jp", "消息类型.语音"),
            // dict.sys.message.type.voice
            ("dict.sys.message.type.voice", "zh-CN", "语音", "消息类型.语音"),
            // dict.sys.message.type.voice
            ("dict.sys.message.type.voice", "zh-HK", "语音_hk", "消息类型.语音"),

            // dict.sys.news.category.0
            ("dict.sys.news.category.0", "en-US", "公司新闻_us", "新闻分类.公司新闻"),
            // dict.sys.news.category.0
            ("dict.sys.news.category.0", "ja-JP", "公司新闻_jp", "新闻分类.公司新闻"),
            // dict.sys.news.category.0
            ("dict.sys.news.category.0", "zh-CN", "公司新闻", "新闻分类.公司新闻"),
            // dict.sys.news.category.0
            ("dict.sys.news.category.0", "zh-HK", "公司新闻_hk", "新闻分类.公司新闻"),

            // dict.sys.news.category.1
            ("dict.sys.news.category.1", "en-US", "行业动态_us", "新闻分类.行业动态"),
            // dict.sys.news.category.1
            ("dict.sys.news.category.1", "ja-JP", "行业动态_jp", "新闻分类.行业动态"),
            // dict.sys.news.category.1
            ("dict.sys.news.category.1", "zh-CN", "行业动态", "新闻分类.行业动态"),
            // dict.sys.news.category.1
            ("dict.sys.news.category.1", "zh-HK", "行业动态_hk", "新闻分类.行业动态"),

            // dict.sys.news.category.2
            ("dict.sys.news.category.2", "en-US", "技术分享_us", "新闻分类.技术分享"),
            // dict.sys.news.category.2
            ("dict.sys.news.category.2", "ja-JP", "技术分享_jp", "新闻分类.技术分享"),
            // dict.sys.news.category.2
            ("dict.sys.news.category.2", "zh-CN", "技术分享", "新闻分类.技术分享"),
            // dict.sys.news.category.2
            ("dict.sys.news.category.2", "zh-HK", "技术分享_hk", "新闻分类.技术分享"),

            // dict.sys.news.category.3
            ("dict.sys.news.category.3", "en-US", "产品发布_us", "新闻分类.产品发布"),
            // dict.sys.news.category.3
            ("dict.sys.news.category.3", "ja-JP", "产品发布_jp", "新闻分类.产品发布"),
            // dict.sys.news.category.3
            ("dict.sys.news.category.3", "zh-CN", "产品发布", "新闻分类.产品发布"),
            // dict.sys.news.category.3
            ("dict.sys.news.category.3", "zh-HK", "产品发布_hk", "新闻分类.产品发布"),

            // dict.sys.news.category.4
            ("dict.sys.news.category.4", "en-US", "活动资讯_us", "新闻分类.活动资讯"),
            // dict.sys.news.category.4
            ("dict.sys.news.category.4", "ja-JP", "活动资讯_jp", "新闻分类.活动资讯"),
            // dict.sys.news.category.4
            ("dict.sys.news.category.4", "zh-CN", "活动资讯", "新闻分类.活动资讯"),
            // dict.sys.news.category.4
            ("dict.sys.news.category.4", "zh-HK", "活动资讯_hk", "新闻分类.活动资讯"),

            // dict.sys.news.category.5
            ("dict.sys.news.category.5", "en-US", "其他_us", "新闻分类.其他"),
            // dict.sys.news.category.5
            ("dict.sys.news.category.5", "ja-JP", "其他_jp", "新闻分类.其他"),
            // dict.sys.news.category.5
            ("dict.sys.news.category.5", "zh-CN", "其他", "新闻分类.其他"),
            // dict.sys.news.category.5
            ("dict.sys.news.category.5", "zh-HK", "其他_hk", "新闻分类.其他"),

            // dict.sys.publish.status.0
            ("dict.sys.publish.status.0", "en-US", "草稿_us", "发布状态.草稿"),
            // dict.sys.publish.status.0
            ("dict.sys.publish.status.0", "ja-JP", "草稿_jp", "发布状态.草稿"),
            // dict.sys.publish.status.0
            ("dict.sys.publish.status.0", "zh-CN", "草稿", "发布状态.草稿"),
            // dict.sys.publish.status.0
            ("dict.sys.publish.status.0", "zh-HK", "草稿_hk", "发布状态.草稿"),

            // dict.sys.publish.status.1
            ("dict.sys.publish.status.1", "en-US", "已发布_us", "发布状态.已发布"),
            // dict.sys.publish.status.1
            ("dict.sys.publish.status.1", "ja-JP", "已发布_jp", "发布状态.已发布"),
            // dict.sys.publish.status.1
            ("dict.sys.publish.status.1", "zh-CN", "已发布", "发布状态.已发布"),
            // dict.sys.publish.status.1
            ("dict.sys.publish.status.1", "zh-HK", "已发布_hk", "发布状态.已发布"),

            // dict.sys.publish.status.2
            ("dict.sys.publish.status.2", "en-US", "已撤回_us", "发布状态.已撤回"),
            // dict.sys.publish.status.2
            ("dict.sys.publish.status.2", "ja-JP", "已撤回_jp", "发布状态.已撤回"),
            // dict.sys.publish.status.2
            ("dict.sys.publish.status.2", "zh-CN", "已撤回", "发布状态.已撤回"),
            // dict.sys.publish.status.2
            ("dict.sys.publish.status.2", "zh-HK", "已撤回_hk", "发布状态.已撤回"),

            // dict.sys.publish.status.3
            ("dict.sys.publish.status.3", "en-US", "已过期_us", "发布状态.已过期"),
            // dict.sys.publish.status.3
            ("dict.sys.publish.status.3", "ja-JP", "已过期_jp", "发布状态.已过期"),
            // dict.sys.publish.status.3
            ("dict.sys.publish.status.3", "zh-CN", "已过期", "发布状态.已过期"),
            // dict.sys.publish.status.3
            ("dict.sys.publish.status.3", "zh-HK", "已过期_hk", "发布状态.已过期"),

            // dict.sys.approval.status.0
            ("dict.sys.approval.status.0", "en-US", "待审批_us", "审批状态.待审批"),
            // dict.sys.approval.status.0
            ("dict.sys.approval.status.0", "ja-JP", "待审批_jp", "审批状态.待审批"),
            // dict.sys.approval.status.0
            ("dict.sys.approval.status.0", "zh-CN", "待审批", "审批状态.待审批"),
            // dict.sys.approval.status.0
            ("dict.sys.approval.status.0", "zh-HK", "待审批_hk", "审批状态.待审批"),

            // dict.sys.approval.status.1
            ("dict.sys.approval.status.1", "en-US", "审批中_us", "审批状态.审批中"),
            // dict.sys.approval.status.1
            ("dict.sys.approval.status.1", "ja-JP", "审批中_jp", "审批状态.审批中"),
            // dict.sys.approval.status.1
            ("dict.sys.approval.status.1", "zh-CN", "审批中", "审批状态.审批中"),
            // dict.sys.approval.status.1
            ("dict.sys.approval.status.1", "zh-HK", "审批中_hk", "审批状态.审批中"),

            // dict.sys.approval.status.2
            ("dict.sys.approval.status.2", "en-US", "已通过_us", "审批状态.已通过"),
            // dict.sys.approval.status.2
            ("dict.sys.approval.status.2", "ja-JP", "已通过_jp", "审批状态.已通过"),
            // dict.sys.approval.status.2
            ("dict.sys.approval.status.2", "zh-CN", "已通过", "审批状态.已通过"),
            // dict.sys.approval.status.2
            ("dict.sys.approval.status.2", "zh-HK", "已通过_hk", "审批状态.已通过"),

            // dict.sys.approval.status.3
            ("dict.sys.approval.status.3", "en-US", "已驳回_us", "审批状态.已驳回"),
            // dict.sys.approval.status.3
            ("dict.sys.approval.status.3", "ja-JP", "已驳回_jp", "审批状态.已驳回"),
            // dict.sys.approval.status.3
            ("dict.sys.approval.status.3", "zh-CN", "已驳回", "审批状态.已驳回"),
            // dict.sys.approval.status.3
            ("dict.sys.approval.status.3", "zh-HK", "已驳回_hk", "审批状态.已驳回"),

            // dict.sys.approval.status.4
            ("dict.sys.approval.status.4", "en-US", "已撤回_us", "审批状态.已撤回"),
            // dict.sys.approval.status.4
            ("dict.sys.approval.status.4", "ja-JP", "已撤回_jp", "审批状态.已撤回"),
            // dict.sys.approval.status.4
            ("dict.sys.approval.status.4", "zh-CN", "已撤回", "审批状态.已撤回"),
            // dict.sys.approval.status.4
            ("dict.sys.approval.status.4", "zh-HK", "已撤回_hk", "审批状态.已撤回"),

            // dict.sys.approval.status.5
            ("dict.sys.approval.status.5", "en-US", "已终止_us", "审批状态.已终止"),
            // dict.sys.approval.status.5
            ("dict.sys.approval.status.5", "ja-JP", "已终止_jp", "审批状态.已终止"),
            // dict.sys.approval.status.5
            ("dict.sys.approval.status.5", "zh-CN", "已终止", "审批状态.已终止"),
            // dict.sys.approval.status.5
            ("dict.sys.approval.status.5", "zh-HK", "已终止_hk", "审批状态.已终止"),

            // dict.sys.ticket.status.0
            ("dict.sys.ticket.status.0", "en-US", "新建_us", "工单状态.新建"),
            // dict.sys.ticket.status.0
            ("dict.sys.ticket.status.0", "ja-JP", "新建_jp", "工单状态.新建"),
            // dict.sys.ticket.status.0
            ("dict.sys.ticket.status.0", "zh-CN", "新建", "工单状态.新建"),
            // dict.sys.ticket.status.0
            ("dict.sys.ticket.status.0", "zh-HK", "新建_hk", "工单状态.新建"),

            // dict.sys.ticket.status.1
            ("dict.sys.ticket.status.1", "en-US", "已分配_us", "工单状态.已分配"),
            // dict.sys.ticket.status.1
            ("dict.sys.ticket.status.1", "ja-JP", "已分配_jp", "工单状态.已分配"),
            // dict.sys.ticket.status.1
            ("dict.sys.ticket.status.1", "zh-CN", "已分配", "工单状态.已分配"),
            // dict.sys.ticket.status.1
            ("dict.sys.ticket.status.1", "zh-HK", "已分配_hk", "工单状态.已分配"),

            // dict.sys.ticket.status.2
            ("dict.sys.ticket.status.2", "en-US", "处理中_us", "工单状态.处理中"),
            // dict.sys.ticket.status.2
            ("dict.sys.ticket.status.2", "ja-JP", "处理中_jp", "工单状态.处理中"),
            // dict.sys.ticket.status.2
            ("dict.sys.ticket.status.2", "zh-CN", "处理中", "工单状态.处理中"),
            // dict.sys.ticket.status.2
            ("dict.sys.ticket.status.2", "zh-HK", "处理中_hk", "工单状态.处理中"),

            // dict.sys.ticket.status.3
            ("dict.sys.ticket.status.3", "en-US", "待确认_us", "工单状态.待确认"),
            // dict.sys.ticket.status.3
            ("dict.sys.ticket.status.3", "ja-JP", "待确认_jp", "工单状态.待确认"),
            // dict.sys.ticket.status.3
            ("dict.sys.ticket.status.3", "zh-CN", "待确认", "工单状态.待确认"),
            // dict.sys.ticket.status.3
            ("dict.sys.ticket.status.3", "zh-HK", "待确认_hk", "工单状态.待确认"),

            // dict.sys.ticket.status.4
            ("dict.sys.ticket.status.4", "en-US", "已完成_us", "工单状态.已完成"),
            // dict.sys.ticket.status.4
            ("dict.sys.ticket.status.4", "ja-JP", "已完成_jp", "工单状态.已完成"),
            // dict.sys.ticket.status.4
            ("dict.sys.ticket.status.4", "zh-CN", "已完成", "工单状态.已完成"),
            // dict.sys.ticket.status.4
            ("dict.sys.ticket.status.4", "zh-HK", "已完成_hk", "工单状态.已完成"),

            // dict.sys.ticket.status.5
            ("dict.sys.ticket.status.5", "en-US", "已关闭_us", "工单状态.已关闭"),
            // dict.sys.ticket.status.5
            ("dict.sys.ticket.status.5", "ja-JP", "已关闭_jp", "工单状态.已关闭"),
            // dict.sys.ticket.status.5
            ("dict.sys.ticket.status.5", "zh-CN", "已关闭", "工单状态.已关闭"),
            // dict.sys.ticket.status.5
            ("dict.sys.ticket.status.5", "zh-HK", "已关闭_hk", "工单状态.已关闭"),

            // dict.sys.ticket.status.6
            ("dict.sys.ticket.status.6", "en-US", "已取消_us", "工单状态.已取消"),
            // dict.sys.ticket.status.6
            ("dict.sys.ticket.status.6", "ja-JP", "已取消_jp", "工单状态.已取消"),
            // dict.sys.ticket.status.6
            ("dict.sys.ticket.status.6", "zh-CN", "已取消", "工单状态.已取消"),
            // dict.sys.ticket.status.6
            ("dict.sys.ticket.status.6", "zh-HK", "已取消_hk", "工单状态.已取消"),

            // dict.sys.ticket.status.7
            ("dict.sys.ticket.status.7", "en-US", "重新打开_us", "工单状态.重新打开"),
            // dict.sys.ticket.status.7
            ("dict.sys.ticket.status.7", "ja-JP", "重新打开_jp", "工单状态.重新打开"),
            // dict.sys.ticket.status.7
            ("dict.sys.ticket.status.7", "zh-CN", "重新打开", "工单状态.重新打开"),
            // dict.sys.ticket.status.7
            ("dict.sys.ticket.status.7", "zh-HK", "重新打开_hk", "工单状态.重新打开"),

            // dict.sys.normal.disable.status.1
            ("dict.sys.normal.disable.status.1", "en-US", "启用_us", "默认状态.启用"),
            // dict.sys.normal.disable.status.1
            ("dict.sys.normal.disable.status.1", "ja-JP", "启用_jp", "默认状态.启用"),
            // dict.sys.normal.disable.status.1
            ("dict.sys.normal.disable.status.1", "zh-CN", "启用", "默认状态.启用"),
            // dict.sys.normal.disable.status.1
            ("dict.sys.normal.disable.status.1", "zh-HK", "启用_hk", "默认状态.启用"),

            // dict.sys.normal.disable.status.0
            ("dict.sys.normal.disable.status.0", "en-US", "禁用_us", "默认状态.禁用"),
            // dict.sys.normal.disable.status.0
            ("dict.sys.normal.disable.status.0", "ja-JP", "禁用_jp", "默认状态.禁用"),
            // dict.sys.normal.disable.status.0
            ("dict.sys.normal.disable.status.0", "zh-CN", "禁用", "默认状态.禁用"),
            // dict.sys.normal.disable.status.0
            ("dict.sys.normal.disable.status.0", "zh-HK", "禁用_hk", "默认状态.禁用"),

            // dict.sys.normal.disable.status.2
            ("dict.sys.normal.disable.status.2", "en-US", "锁定_us", "默认状态.锁定"),
            // dict.sys.normal.disable.status.2
            ("dict.sys.normal.disable.status.2", "ja-JP", "锁定_jp", "默认状态.锁定"),
            // dict.sys.normal.disable.status.2
            ("dict.sys.normal.disable.status.2", "zh-CN", "锁定", "默认状态.锁定"),
            // dict.sys.normal.disable.status.2
            ("dict.sys.normal.disable.status.2", "zh-HK", "锁定_hk", "默认状态.锁定"),

            // dict.sys.announcement.category.1
            ("dict.sys.announcement.category.1", "en-US", "紧急通知_us", "公告类型.紧急通知"),
            // dict.sys.announcement.category.1
            ("dict.sys.announcement.category.1", "ja-JP", "紧急通知_jp", "公告类型.紧急通知"),
            // dict.sys.announcement.category.1
            ("dict.sys.announcement.category.1", "zh-CN", "紧急通知", "公告类型.紧急通知"),
            // dict.sys.announcement.category.1
            ("dict.sys.announcement.category.1", "zh-HK", "紧急通知_hk", "公告类型.紧急通知"),

            // dict.sys.announcement.category.2
            ("dict.sys.announcement.category.2", "en-US", "公告_us", "公告类型.公告"),
            // dict.sys.announcement.category.2
            ("dict.sys.announcement.category.2", "ja-JP", "公告_jp", "公告类型.公告"),
            // dict.sys.announcement.category.2
            ("dict.sys.announcement.category.2", "zh-CN", "公告", "公告类型.公告"),
            // dict.sys.announcement.category.2
            ("dict.sys.announcement.category.2", "zh-HK", "公告_hk", "公告类型.公告"),

            // dict.sys.announcement.category.3
            ("dict.sys.announcement.category.3", "en-US", "通知_us", "公告类型.通知"),
            // dict.sys.announcement.category.3
            ("dict.sys.announcement.category.3", "ja-JP", "通知_jp", "公告类型.通知"),
            // dict.sys.announcement.category.3
            ("dict.sys.announcement.category.3", "zh-CN", "通知", "公告类型.通知"),
            // dict.sys.announcement.category.3
            ("dict.sys.announcement.category.3", "zh-HK", "通知_hk", "公告类型.通知"),

            // dict.sys.announcement.category.4
            ("dict.sys.announcement.category.4", "en-US", "决议_us", "公告类型.决议"),
            // dict.sys.announcement.category.4
            ("dict.sys.announcement.category.4", "ja-JP", "决议_jp", "公告类型.决议"),
            // dict.sys.announcement.category.4
            ("dict.sys.announcement.category.4", "zh-CN", "决议", "公告类型.决议"),
            // dict.sys.announcement.category.4
            ("dict.sys.announcement.category.4", "zh-HK", "决议_hk", "公告类型.决议"),

            // dict.sys.announcement.category.5
            ("dict.sys.announcement.category.5", "en-US", "活动_us", "公告类型.活动"),
            // dict.sys.announcement.category.5
            ("dict.sys.announcement.category.5", "ja-JP", "活动_jp", "公告类型.活动"),
            // dict.sys.announcement.category.5
            ("dict.sys.announcement.category.5", "zh-CN", "活动", "公告类型.活动"),
            // dict.sys.announcement.category.5
            ("dict.sys.announcement.category.5", "zh-HK", "活动_hk", "公告类型.活动"),

            // dict.sys.announcement.category.6
            ("dict.sys.announcement.category.6", "en-US", "安全通告_us", "公告类型.安全通告"),
            // dict.sys.announcement.category.6
            ("dict.sys.announcement.category.6", "ja-JP", "安全通告_jp", "公告类型.安全通告"),
            // dict.sys.announcement.category.6
            ("dict.sys.announcement.category.6", "zh-CN", "安全通告", "公告类型.安全通告"),
            // dict.sys.announcement.category.6
            ("dict.sys.announcement.category.6", "zh-HK", "安全通告_hk", "公告类型.安全通告"),

            // dict.sys.announcement.category.7
            ("dict.sys.announcement.category.7", "en-US", "运维通知_us", "公告类型.运维通知"),
            // dict.sys.announcement.category.7
            ("dict.sys.announcement.category.7", "ja-JP", "运维通知_jp", "公告类型.运维通知"),
            // dict.sys.announcement.category.7
            ("dict.sys.announcement.category.7", "zh-CN", "运维通知", "公告类型.运维通知"),
            // dict.sys.announcement.category.7
            ("dict.sys.announcement.category.7", "zh-HK", "运维通知_hk", "公告类型.运维通知"),

            // dict.sys.announcement.category.8
            ("dict.sys.announcement.category.8", "en-US", "系统公告_us", "公告类型.系统公告"),
            // dict.sys.announcement.category.8
            ("dict.sys.announcement.category.8", "ja-JP", "系统公告_jp", "公告类型.系统公告"),
            // dict.sys.announcement.category.8
            ("dict.sys.announcement.category.8", "zh-CN", "系统公告", "公告类型.系统公告"),
            // dict.sys.announcement.category.8
            ("dict.sys.announcement.category.8", "zh-HK", "系统公告_hk", "公告类型.系统公告"),

            // dict.sys.online.status.0
            ("dict.sys.online.status.0", "en-US", "在线_us", "在线状态.在线"),
            // dict.sys.online.status.0
            ("dict.sys.online.status.0", "ja-JP", "在线_jp", "在线状态.在线"),
            // dict.sys.online.status.0
            ("dict.sys.online.status.0", "zh-CN", "在线", "在线状态.在线"),
            // dict.sys.online.status.0
            ("dict.sys.online.status.0", "zh-HK", "在线_hk", "在线状态.在线"),

            // dict.sys.online.status.1
            ("dict.sys.online.status.1", "en-US", "离线_us", "在线状态.离线"),
            // dict.sys.online.status.1
            ("dict.sys.online.status.1", "ja-JP", "离线_jp", "在线状态.离线"),
            // dict.sys.online.status.1
            ("dict.sys.online.status.1", "zh-CN", "离线", "在线状态.离线"),
            // dict.sys.online.status.1
            ("dict.sys.online.status.1", "zh-HK", "离线_hk", "在线状态.离线"),

            // dict.sys.online.status.2
            ("dict.sys.online.status.2", "en-US", "离开_us", "在线状态.离开"),
            // dict.sys.online.status.2
            ("dict.sys.online.status.2", "ja-JP", "离开_jp", "在线状态.离开"),
            // dict.sys.online.status.2
            ("dict.sys.online.status.2", "zh-CN", "离开", "在线状态.离开"),
            // dict.sys.online.status.2
            ("dict.sys.online.status.2", "zh-HK", "离开_hk", "在线状态.离开"),

            // dict.sys.oper.type.1
            ("dict.sys.oper.type.1", "en-US", "新增_us", "操作类型.新增"),
            // dict.sys.oper.type.1
            ("dict.sys.oper.type.1", "ja-JP", "新增_jp", "操作类型.新增"),
            // dict.sys.oper.type.1
            ("dict.sys.oper.type.1", "zh-CN", "新增", "操作类型.新增"),
            // dict.sys.oper.type.1
            ("dict.sys.oper.type.1", "zh-HK", "新增_hk", "操作类型.新增"),

            // dict.sys.oper.type.2
            ("dict.sys.oper.type.2", "en-US", "修改_us", "操作类型.修改"),
            // dict.sys.oper.type.2
            ("dict.sys.oper.type.2", "ja-JP", "修改_jp", "操作类型.修改"),
            // dict.sys.oper.type.2
            ("dict.sys.oper.type.2", "zh-CN", "修改", "操作类型.修改"),
            // dict.sys.oper.type.2
            ("dict.sys.oper.type.2", "zh-HK", "修改_hk", "操作类型.修改"),

            // dict.sys.oper.type.3
            ("dict.sys.oper.type.3", "en-US", "删除_us", "操作类型.删除"),
            // dict.sys.oper.type.3
            ("dict.sys.oper.type.3", "ja-JP", "删除_jp", "操作类型.删除"),
            // dict.sys.oper.type.3
            ("dict.sys.oper.type.3", "zh-CN", "删除", "操作类型.删除"),
            // dict.sys.oper.type.3
            ("dict.sys.oper.type.3", "zh-HK", "删除_hk", "操作类型.删除"),

            // dict.sys.oper.type.4
            ("dict.sys.oper.type.4", "en-US", "查询_us", "操作类型.查询"),
            // dict.sys.oper.type.4
            ("dict.sys.oper.type.4", "ja-JP", "查询_jp", "操作类型.查询"),
            // dict.sys.oper.type.4
            ("dict.sys.oper.type.4", "zh-CN", "查询", "操作类型.查询"),
            // dict.sys.oper.type.4
            ("dict.sys.oper.type.4", "zh-HK", "查询_hk", "操作类型.查询"),

            // dict.sys.oper.type.5
            ("dict.sys.oper.type.5", "en-US", "导出_us", "操作类型.导出"),
            // dict.sys.oper.type.5
            ("dict.sys.oper.type.5", "ja-JP", "导出_jp", "操作类型.导出"),
            // dict.sys.oper.type.5
            ("dict.sys.oper.type.5", "zh-CN", "导出", "操作类型.导出"),
            // dict.sys.oper.type.5
            ("dict.sys.oper.type.5", "zh-HK", "导出_hk", "操作类型.导出"),

            // dict.sys.oper.type.6
            ("dict.sys.oper.type.6", "en-US", "导入_us", "操作类型.导入"),
            // dict.sys.oper.type.6
            ("dict.sys.oper.type.6", "ja-JP", "导入_jp", "操作类型.导入"),
            // dict.sys.oper.type.6
            ("dict.sys.oper.type.6", "zh-CN", "导入", "操作类型.导入"),
            // dict.sys.oper.type.6
            ("dict.sys.oper.type.6", "zh-HK", "导入_hk", "操作类型.导入"),

            // dict.sys.oper.type.7
            ("dict.sys.oper.type.7", "en-US", "授权_us", "操作类型.授权"),
            // dict.sys.oper.type.7
            ("dict.sys.oper.type.7", "ja-JP", "授权_jp", "操作类型.授权"),
            // dict.sys.oper.type.7
            ("dict.sys.oper.type.7", "zh-CN", "授权", "操作类型.授权"),
            // dict.sys.oper.type.7
            ("dict.sys.oper.type.7", "zh-HK", "授权_hk", "操作类型.授权"),

            // dict.sys.oper.type.8
            ("dict.sys.oper.type.8", "en-US", "强退_us", "操作类型.强退"),
            // dict.sys.oper.type.8
            ("dict.sys.oper.type.8", "ja-JP", "强退_jp", "操作类型.强退"),
            // dict.sys.oper.type.8
            ("dict.sys.oper.type.8", "zh-CN", "强退", "操作类型.强退"),
            // dict.sys.oper.type.8
            ("dict.sys.oper.type.8", "zh-HK", "强退_hk", "操作类型.强退"),

            // dict.sys.oper.type.9
            ("dict.sys.oper.type.9", "en-US", "生成代码_us", "操作类型.生成代码"),
            // dict.sys.oper.type.9
            ("dict.sys.oper.type.9", "ja-JP", "生成代码_jp", "操作类型.生成代码"),
            // dict.sys.oper.type.9
            ("dict.sys.oper.type.9", "zh-CN", "生成代码", "操作类型.生成代码"),
            // dict.sys.oper.type.9
            ("dict.sys.oper.type.9", "zh-HK", "生成代码_hk", "操作类型.生成代码"),

            // dict.sys.oper.type.10
            ("dict.sys.oper.type.10", "en-US", "清空数据_us", "操作类型.清空数据"),
            // dict.sys.oper.type.10
            ("dict.sys.oper.type.10", "ja-JP", "清空数据_jp", "操作类型.清空数据"),
            // dict.sys.oper.type.10
            ("dict.sys.oper.type.10", "zh-CN", "清空数据", "操作类型.清空数据"),
            // dict.sys.oper.type.10
            ("dict.sys.oper.type.10", "zh-HK", "清空数据_hk", "操作类型.清空数据"),

            // dict.sys.oss.provider.type.aliyun
            ("dict.sys.oss.provider.type.aliyun", "en-US", "阿里云oss_us", "oss提供商类型.阿里云oss"),
            // dict.sys.oss.provider.type.aliyun
            ("dict.sys.oss.provider.type.aliyun", "ja-JP", "阿里云oss_jp", "oss提供商类型.阿里云oss"),
            // dict.sys.oss.provider.type.aliyun
            ("dict.sys.oss.provider.type.aliyun", "zh-CN", "阿里云oss", "oss提供商类型.阿里云oss"),
            // dict.sys.oss.provider.type.aliyun
            ("dict.sys.oss.provider.type.aliyun", "zh-HK", "阿里云oss_hk", "oss提供商类型.阿里云oss"),

            // dict.sys.oss.provider.type.tencent
            ("dict.sys.oss.provider.type.tencent", "en-US", "腾讯云cos_us", "oss提供商类型.腾讯云cos"),
            // dict.sys.oss.provider.type.tencent
            ("dict.sys.oss.provider.type.tencent", "ja-JP", "腾讯云cos_jp", "oss提供商类型.腾讯云cos"),
            // dict.sys.oss.provider.type.tencent
            ("dict.sys.oss.provider.type.tencent", "zh-CN", "腾讯云cos", "oss提供商类型.腾讯云cos"),
            // dict.sys.oss.provider.type.tencent
            ("dict.sys.oss.provider.type.tencent", "zh-HK", "腾讯云cos_hk", "oss提供商类型.腾讯云cos"),

            // dict.sys.oss.provider.type.huawei
            ("dict.sys.oss.provider.type.huawei", "en-US", "华为云obs_us", "oss提供商类型.华为云obs"),
            // dict.sys.oss.provider.type.huawei
            ("dict.sys.oss.provider.type.huawei", "ja-JP", "华为云obs_jp", "oss提供商类型.华为云obs"),
            // dict.sys.oss.provider.type.huawei
            ("dict.sys.oss.provider.type.huawei", "zh-CN", "华为云obs", "oss提供商类型.华为云obs"),
            // dict.sys.oss.provider.type.huawei
            ("dict.sys.oss.provider.type.huawei", "zh-HK", "华为云obs_hk", "oss提供商类型.华为云obs"),

            // dict.sys.oss.provider.type.aws
            ("dict.sys.oss.provider.type.aws", "en-US", "aws s3_us", "oss提供商类型.aws s3"),
            // dict.sys.oss.provider.type.aws
            ("dict.sys.oss.provider.type.aws", "ja-JP", "aws s3_jp", "oss提供商类型.aws s3"),
            // dict.sys.oss.provider.type.aws
            ("dict.sys.oss.provider.type.aws", "zh-CN", "aws s3", "oss提供商类型.aws s3"),
            // dict.sys.oss.provider.type.aws
            ("dict.sys.oss.provider.type.aws", "zh-HK", "aws s3_hk", "oss提供商类型.aws s3"),

            // dict.sys.post.category.mgt
            ("dict.sys.post.category.mgt", "en-US", "管理岗_us", "岗位类别.管理岗"),
            // dict.sys.post.category.mgt
            ("dict.sys.post.category.mgt", "ja-JP", "管理岗_jp", "岗位类别.管理岗"),
            // dict.sys.post.category.mgt
            ("dict.sys.post.category.mgt", "zh-CN", "管理岗", "岗位类别.管理岗"),
            // dict.sys.post.category.mgt
            ("dict.sys.post.category.mgt", "zh-HK", "管理岗_hk", "岗位类别.管理岗"),

            // dict.sys.post.category.pro
            ("dict.sys.post.category.pro", "en-US", "专业岗_us", "岗位类别.专业岗（财/人/法/市 专家层）"),
            // dict.sys.post.category.pro
            ("dict.sys.post.category.pro", "ja-JP", "专业岗_jp", "岗位类别.专业岗（财/人/法/市 专家层）"),
            // dict.sys.post.category.pro
            ("dict.sys.post.category.pro", "zh-CN", "专业岗", "岗位类别.专业岗（财/人/法/市 专家层）"),
            // dict.sys.post.category.pro
            ("dict.sys.post.category.pro", "zh-HK", "专业岗_hk", "岗位类别.专业岗（财/人/法/市 专家层）"),

            // dict.sys.post.category.tec
            ("dict.sys.post.category.tec", "en-US", "技术岗_us", "岗位类别.技术岗（研发/工程/IT高阶）"),
            // dict.sys.post.category.tec
            ("dict.sys.post.category.tec", "ja-JP", "技术岗_jp", "岗位类别.技术岗（研发/工程/IT高阶）"),
            // dict.sys.post.category.tec
            ("dict.sys.post.category.tec", "zh-CN", "技术岗", "岗位类别.技术岗（研发/工程/IT高阶）"),
            // dict.sys.post.category.tec
            ("dict.sys.post.category.tec", "zh-HK", "技术岗_hk", "岗位类别.技术岗（研发/工程/IT高阶）"),

            // dict.sys.post.category.sup
            ("dict.sys.post.category.sup", "en-US", "支持岗_us", "岗位类别.支持岗（事务/保障/辅助）"),
            // dict.sys.post.category.sup
            ("dict.sys.post.category.sup", "ja-JP", "支持岗_jp", "岗位类别.支持岗（事务/保障/辅助）"),
            // dict.sys.post.category.sup
            ("dict.sys.post.category.sup", "zh-CN", "支持岗", "岗位类别.支持岗（事务/保障/辅助）"),
            // dict.sys.post.category.sup
            ("dict.sys.post.category.sup", "zh-HK", "支持岗_hk", "岗位类别.支持岗（事务/保障/辅助）"),

            // dict.sys.post.category.ops
            ("dict.sys.post.category.ops", "en-US", "操作岗_us", "岗位类别.操作岗（产线/直接作业）"),
            // dict.sys.post.category.ops
            ("dict.sys.post.category.ops", "ja-JP", "操作岗_jp", "岗位类别.操作岗（产线/直接作业）"),
            // dict.sys.post.category.ops
            ("dict.sys.post.category.ops", "zh-CN", "操作岗", "岗位类别.操作岗（产线/直接作业）"),
            // dict.sys.post.category.ops
            ("dict.sys.post.category.ops", "zh-HK", "操作岗_hk", "岗位类别.操作岗（产线/直接作业）"),

            // dict.sys.post.level.category.p1
            ("dict.sys.post.level.category.p1", "en-US", "助理_us", "P序列.助理"),
            // dict.sys.post.level.category.p1
            ("dict.sys.post.level.category.p1", "ja-JP", "助理_jp", "P序列.助理"),
            // dict.sys.post.level.category.p1
            ("dict.sys.post.level.category.p1", "zh-CN", "助理", "P序列.助理"),
            // dict.sys.post.level.category.p1
            ("dict.sys.post.level.category.p1", "zh-HK", "助理_hk", "P序列.助理"),

            // dict.sys.post.level.category.p2
            ("dict.sys.post.level.category.p2", "en-US", "专员/工程师_us", "P序列.专员/工程师"),
            // dict.sys.post.level.category.p2
            ("dict.sys.post.level.category.p2", "ja-JP", "专员/工程师_jp", "P序列.专员/工程师"),
            // dict.sys.post.level.category.p2
            ("dict.sys.post.level.category.p2", "zh-CN", "专员/工程师", "P序列.专员/工程师"),
            // dict.sys.post.level.category.p2
            ("dict.sys.post.level.category.p2", "zh-HK", "专员/工程师_hk", "P序列.专员/工程师"),

            // dict.sys.post.level.category.p3
            ("dict.sys.post.level.category.p3", "en-US", "高级专员/高级工程师_us", "P序列.高级专员/高级工程师"),
            // dict.sys.post.level.category.p3
            ("dict.sys.post.level.category.p3", "ja-JP", "高级专员/高级工程师_jp", "P序列.高级专员/高级工程师"),
            // dict.sys.post.level.category.p3
            ("dict.sys.post.level.category.p3", "zh-CN", "高级专员/高级工程师", "P序列.高级专员/高级工程师"),
            // dict.sys.post.level.category.p3
            ("dict.sys.post.level.category.p3", "zh-HK", "高级专员/高级工程师_hk", "P序列.高级专员/高级工程师"),

            // dict.sys.post.level.category.p4
            ("dict.sys.post.level.category.p4", "en-US", "专家/资深工程师_us", "P序列.专家/资深工程师"),
            // dict.sys.post.level.category.p4
            ("dict.sys.post.level.category.p4", "ja-JP", "专家/资深工程师_jp", "P序列.专家/资深工程师"),
            // dict.sys.post.level.category.p4
            ("dict.sys.post.level.category.p4", "zh-CN", "专家/资深工程师", "P序列.专家/资深工程师"),
            // dict.sys.post.level.category.p4
            ("dict.sys.post.level.category.p4", "zh-HK", "专家/资深工程师_hk", "P序列.专家/资深工程师"),

            // dict.sys.post.level.category.m1
            ("dict.sys.post.level.category.m1", "en-US", "主管_us", "M序列.主管"),
            // dict.sys.post.level.category.m1
            ("dict.sys.post.level.category.m1", "ja-JP", "主管_jp", "M序列.主管"),
            // dict.sys.post.level.category.m1
            ("dict.sys.post.level.category.m1", "zh-CN", "主管", "M序列.主管"),
            // dict.sys.post.level.category.m1
            ("dict.sys.post.level.category.m1", "zh-HK", "主管_hk", "M序列.主管"),

            // dict.sys.post.level.category.m2
            ("dict.sys.post.level.category.m2", "en-US", "经理_us", "M序列.经理"),
            // dict.sys.post.level.category.m2
            ("dict.sys.post.level.category.m2", "ja-JP", "经理_jp", "M序列.经理"),
            // dict.sys.post.level.category.m2
            ("dict.sys.post.level.category.m2", "zh-CN", "经理", "M序列.经理"),
            // dict.sys.post.level.category.m2
            ("dict.sys.post.level.category.m2", "zh-HK", "经理_hk", "M序列.经理"),

            // dict.sys.post.level.category.m3
            ("dict.sys.post.level.category.m3", "en-US", "总监_us", "M序列.总监"),
            // dict.sys.post.level.category.m3
            ("dict.sys.post.level.category.m3", "ja-JP", "总监_jp", "M序列.总监"),
            // dict.sys.post.level.category.m3
            ("dict.sys.post.level.category.m3", "zh-CN", "总监", "M序列.总监"),
            // dict.sys.post.level.category.m3
            ("dict.sys.post.level.category.m3", "zh-HK", "总监_hk", "M序列.总监"),

            // dict.sys.post.level.category.m4
            ("dict.sys.post.level.category.m4", "en-US", "副总裁_us", "M序列.副总裁"),
            // dict.sys.post.level.category.m4
            ("dict.sys.post.level.category.m4", "ja-JP", "副总裁_jp", "M序列.副总裁"),
            // dict.sys.post.level.category.m4
            ("dict.sys.post.level.category.m4", "zh-CN", "副总裁", "M序列.副总裁"),
            // dict.sys.post.level.category.m4
            ("dict.sys.post.level.category.m4", "zh-HK", "副总裁_hk", "M序列.副总裁"),

            // dict.sys.post.level.category.m5
            ("dict.sys.post.level.category.m5", "en-US", "C-Level_us", "M序列.C-Level"),
            // dict.sys.post.level.category.m5
            ("dict.sys.post.level.category.m5", "ja-JP", "C-Level_jp", "M序列.C-Level"),
            // dict.sys.post.level.category.m5
            ("dict.sys.post.level.category.m5", "zh-CN", "C-Level", "M序列.C-Level"),
            // dict.sys.post.level.category.m5
            ("dict.sys.post.level.category.m5", "zh-HK", "C-Level_hk", "M序列.C-Level"),

            // dict.sys.priority.level.category.1
            ("dict.sys.priority.level.category.1", "en-US", "最高_us", "优先级.最高"),
            // dict.sys.priority.level.category.1
            ("dict.sys.priority.level.category.1", "ja-JP", "最高_jp", "优先级.最高"),
            // dict.sys.priority.level.category.1
            ("dict.sys.priority.level.category.1", "zh-CN", "最高", "优先级.最高"),
            // dict.sys.priority.level.category.1
            ("dict.sys.priority.level.category.1", "zh-HK", "最高_hk", "优先级.最高"),

            // dict.sys.priority.level.category.2
            ("dict.sys.priority.level.category.2", "en-US", "高_us", "优先级.高"),
            // dict.sys.priority.level.category.2
            ("dict.sys.priority.level.category.2", "ja-JP", "高_jp", "优先级.高"),
            // dict.sys.priority.level.category.2
            ("dict.sys.priority.level.category.2", "zh-CN", "高", "优先级.高"),
            // dict.sys.priority.level.category.2
            ("dict.sys.priority.level.category.2", "zh-HK", "高_hk", "优先级.高"),

            // dict.sys.priority.level.category.3
            ("dict.sys.priority.level.category.3", "en-US", "普通_us", "优先级.普通"),
            // dict.sys.priority.level.category.3
            ("dict.sys.priority.level.category.3", "ja-JP", "普通_jp", "优先级.普通"),
            // dict.sys.priority.level.category.3
            ("dict.sys.priority.level.category.3", "zh-CN", "普通", "优先级.普通"),
            // dict.sys.priority.level.category.3
            ("dict.sys.priority.level.category.3", "zh-HK", "普通_hk", "优先级.普通"),

            // dict.sys.priority.level.category.4
            ("dict.sys.priority.level.category.4", "en-US", "低_us", "优先级.低"),
            // dict.sys.priority.level.category.4
            ("dict.sys.priority.level.category.4", "ja-JP", "低_jp", "优先级.低"),
            // dict.sys.priority.level.category.4
            ("dict.sys.priority.level.category.4", "zh-CN", "低", "优先级.低"),
            // dict.sys.priority.level.category.4
            ("dict.sys.priority.level.category.4", "zh-HK", "低_hk", "优先级.低"),

            // dict.sys.publish.scope.type.0
            ("dict.sys.publish.scope.type.0", "en-US", "全部_us", "发布范围.全部"),
            // dict.sys.publish.scope.type.0
            ("dict.sys.publish.scope.type.0", "ja-JP", "全部_jp", "发布范围.全部"),
            // dict.sys.publish.scope.type.0
            ("dict.sys.publish.scope.type.0", "zh-CN", "全部", "发布范围.全部"),
            // dict.sys.publish.scope.type.0
            ("dict.sys.publish.scope.type.0", "zh-HK", "全部_hk", "发布范围.全部"),

            // dict.sys.publish.scope.type.1
            ("dict.sys.publish.scope.type.1", "en-US", "指定部门_us", "发布范围.指定部门"),
            // dict.sys.publish.scope.type.1
            ("dict.sys.publish.scope.type.1", "ja-JP", "指定部门_jp", "发布范围.指定部门"),
            // dict.sys.publish.scope.type.1
            ("dict.sys.publish.scope.type.1", "zh-CN", "指定部门", "发布范围.指定部门"),
            // dict.sys.publish.scope.type.1
            ("dict.sys.publish.scope.type.1", "zh-HK", "指定部门_hk", "发布范围.指定部门"),

            // dict.sys.publish.scope.type.2
            ("dict.sys.publish.scope.type.2", "en-US", "指定用户_us", "发布范围.指定用户"),
            // dict.sys.publish.scope.type.2
            ("dict.sys.publish.scope.type.2", "ja-JP", "指定用户_jp", "发布范围.指定用户"),
            // dict.sys.publish.scope.type.2
            ("dict.sys.publish.scope.type.2", "zh-CN", "指定用户", "发布范围.指定用户"),
            // dict.sys.publish.scope.type.2
            ("dict.sys.publish.scope.type.2", "zh-HK", "指定用户_hk", "发布范围.指定用户"),

            // dict.sys.publish.scope.type.3
            ("dict.sys.publish.scope.type.3", "en-US", "指定角色_us", "发布范围.指定角色"),
            // dict.sys.publish.scope.type.3
            ("dict.sys.publish.scope.type.3", "ja-JP", "指定角色_jp", "发布范围.指定角色"),
            // dict.sys.publish.scope.type.3
            ("dict.sys.publish.scope.type.3", "zh-CN", "指定角色", "发布范围.指定角色"),
            // dict.sys.publish.scope.type.3
            ("dict.sys.publish.scope.type.3", "zh-HK", "指定角色_hk", "发布范围.指定角色"),

            // dict.sys.read.status.0
            ("dict.sys.read.status.0", "en-US", "未读_us", "读取状态.未读"),
            // dict.sys.read.status.0
            ("dict.sys.read.status.0", "ja-JP", "未读_jp", "读取状态.未读"),
            // dict.sys.read.status.0
            ("dict.sys.read.status.0", "zh-CN", "未读", "读取状态.未读"),
            // dict.sys.read.status.0
            ("dict.sys.read.status.0", "zh-HK", "未读_hk", "读取状态.未读"),

            // dict.sys.read.status.1
            ("dict.sys.read.status.1", "en-US", "已读_us", "读取状态.已读"),
            // dict.sys.read.status.1
            ("dict.sys.read.status.1", "ja-JP", "已读_jp", "读取状态.已读"),
            // dict.sys.read.status.1
            ("dict.sys.read.status.1", "zh-CN", "已读", "读取状态.已读"),
            // dict.sys.read.status.1
            ("dict.sys.read.status.1", "zh-HK", "已读_hk", "读取状态.已读"),

            // dict.sys.reset.period.config.none
            ("dict.sys.reset.period.config.none", "en-US", "不重置_us", "重置周期.不重置"),
            // dict.sys.reset.period.config.none
            ("dict.sys.reset.period.config.none", "ja-JP", "不重置_jp", "重置周期.不重置"),
            // dict.sys.reset.period.config.none
            ("dict.sys.reset.period.config.none", "zh-CN", "不重置", "重置周期.不重置"),
            // dict.sys.reset.period.config.none
            ("dict.sys.reset.period.config.none", "zh-HK", "不重置_hk", "重置周期.不重置"),

            // dict.sys.reset.period.config.year
            ("dict.sys.reset.period.config.year", "en-US", "按年_us", "重置周期.按年"),
            // dict.sys.reset.period.config.year
            ("dict.sys.reset.period.config.year", "ja-JP", "按年_jp", "重置周期.按年"),
            // dict.sys.reset.period.config.year
            ("dict.sys.reset.period.config.year", "zh-CN", "按年", "重置周期.按年"),
            // dict.sys.reset.period.config.year
            ("dict.sys.reset.period.config.year", "zh-HK", "按年_hk", "重置周期.按年"),

            // dict.sys.reset.period.config.month
            ("dict.sys.reset.period.config.month", "en-US", "按月_us", "重置周期.按月"),
            // dict.sys.reset.period.config.month
            ("dict.sys.reset.period.config.month", "ja-JP", "按月_jp", "重置周期.按月"),
            // dict.sys.reset.period.config.month
            ("dict.sys.reset.period.config.month", "zh-CN", "按月", "重置周期.按月"),
            // dict.sys.reset.period.config.month
            ("dict.sys.reset.period.config.month", "zh-HK", "按月_hk", "重置周期.按月"),

            // dict.sys.reset.period.config.day
            ("dict.sys.reset.period.config.day", "en-US", "按日_us", "重置周期.按日"),
            // dict.sys.reset.period.config.day
            ("dict.sys.reset.period.config.day", "ja-JP", "按日_jp", "重置周期.按日"),
            // dict.sys.reset.period.config.day
            ("dict.sys.reset.period.config.day", "zh-CN", "按日", "重置周期.按日"),
            // dict.sys.reset.period.config.day
            ("dict.sys.reset.period.config.day", "zh-HK", "按日_hk", "重置周期.按日"),

            // dict.sys.reset.period.config.hour
            ("dict.sys.reset.period.config.hour", "en-US", "按时_us", "重置周期.按时"),
            // dict.sys.reset.period.config.hour
            ("dict.sys.reset.period.config.hour", "ja-JP", "按时_jp", "重置周期.按时"),
            // dict.sys.reset.period.config.hour
            ("dict.sys.reset.period.config.hour", "zh-CN", "按时", "重置周期.按时"),
            // dict.sys.reset.period.config.hour
            ("dict.sys.reset.period.config.hour", "zh-HK", "按时_hk", "重置周期.按时"),

            // dict.sys.resource.type.frontend
            ("dict.sys.resource.type.frontend", "en-US", "前端_us", "资源类型.前端（frontend）"),
            // dict.sys.resource.type.frontend
            ("dict.sys.resource.type.frontend", "ja-JP", "前端_jp", "资源类型.前端（frontend）"),
            // dict.sys.resource.type.frontend
            ("dict.sys.resource.type.frontend", "zh-CN", "前端", "资源类型.前端（frontend）"),
            // dict.sys.resource.type.frontend
            ("dict.sys.resource.type.frontend", "zh-HK", "前端_hk", "资源类型.前端（frontend）"),

            // dict.sys.resource.type.backend
            ("dict.sys.resource.type.backend", "en-US", "后端_us", "资源类型.后端（backend）"),
            // dict.sys.resource.type.backend
            ("dict.sys.resource.type.backend", "ja-JP", "后端_jp", "资源类型.后端（backend）"),
            // dict.sys.resource.type.backend
            ("dict.sys.resource.type.backend", "zh-CN", "后端", "资源类型.后端（backend）"),
            // dict.sys.resource.type.backend
            ("dict.sys.resource.type.backend", "zh-HK", "后端_hk", "资源类型.后端（backend）"),

            // dict.sys.scheme.status.0
            ("dict.sys.scheme.status.0", "en-US", "草稿_us", "方案状态.草稿"),
            // dict.sys.scheme.status.0
            ("dict.sys.scheme.status.0", "ja-JP", "草稿_jp", "方案状态.草稿"),
            // dict.sys.scheme.status.0
            ("dict.sys.scheme.status.0", "zh-CN", "草稿", "方案状态.草稿"),
            // dict.sys.scheme.status.0
            ("dict.sys.scheme.status.0", "zh-HK", "草稿_hk", "方案状态.草稿"),

            // dict.sys.scheme.status.1
            ("dict.sys.scheme.status.1", "en-US", "已发布_us", "方案状态.已发布"),
            // dict.sys.scheme.status.1
            ("dict.sys.scheme.status.1", "ja-JP", "已发布_jp", "方案状态.已发布"),
            // dict.sys.scheme.status.1
            ("dict.sys.scheme.status.1", "zh-CN", "已发布", "方案状态.已发布"),
            // dict.sys.scheme.status.1
            ("dict.sys.scheme.status.1", "zh-HK", "已发布_hk", "方案状态.已发布"),

            // dict.sys.scheme.status.2
            ("dict.sys.scheme.status.2", "en-US", "已禁用_us", "方案状态.已禁用"),
            // dict.sys.scheme.status.2
            ("dict.sys.scheme.status.2", "ja-JP", "已禁用_jp", "方案状态.已禁用"),
            // dict.sys.scheme.status.2
            ("dict.sys.scheme.status.2", "zh-CN", "已禁用", "方案状态.已禁用"),
            // dict.sys.scheme.status.2
            ("dict.sys.scheme.status.2", "zh-HK", "已禁用_hk", "方案状态.已禁用"),

            // dict.sys.setting.group.category.backend
            ("dict.sys.setting.group.category.backend", "en-US", "后端_us", "设置分组.后端"),
            // dict.sys.setting.group.category.backend
            ("dict.sys.setting.group.category.backend", "ja-JP", "后端_jp", "设置分组.后端"),
            // dict.sys.setting.group.category.backend
            ("dict.sys.setting.group.category.backend", "zh-CN", "后端", "设置分组.后端"),
            // dict.sys.setting.group.category.backend
            ("dict.sys.setting.group.category.backend", "zh-HK", "后端_hk", "设置分组.后端"),

            // dict.sys.setting.group.category.frontend
            ("dict.sys.setting.group.category.frontend", "en-US", "前端_us", "设置分组.前端"),
            // dict.sys.setting.group.category.frontend
            ("dict.sys.setting.group.category.frontend", "ja-JP", "前端_jp", "设置分组.前端"),
            // dict.sys.setting.group.category.frontend
            ("dict.sys.setting.group.category.frontend", "zh-CN", "前端", "设置分组.前端"),
            // dict.sys.setting.group.category.frontend
            ("dict.sys.setting.group.category.frontend", "zh-HK", "前端_hk", "设置分组.前端"),

            // dict.sys.sort.type.asc
            ("dict.sys.sort.type.asc", "en-US", "升序_us", "排序类型.升序"),
            // dict.sys.sort.type.asc
            ("dict.sys.sort.type.asc", "ja-JP", "升序_jp", "排序类型.升序"),
            // dict.sys.sort.type.asc
            ("dict.sys.sort.type.asc", "zh-CN", "升序", "排序类型.升序"),
            // dict.sys.sort.type.asc
            ("dict.sys.sort.type.asc", "zh-HK", "升序_hk", "排序类型.升序"),

            // dict.sys.sort.type.desc
            ("dict.sys.sort.type.desc", "en-US", "降序_us", "排序类型.降序"),
            // dict.sys.sort.type.desc
            ("dict.sys.sort.type.desc", "ja-JP", "降序_jp", "排序类型.降序"),
            // dict.sys.sort.type.desc
            ("dict.sys.sort.type.desc", "zh-CN", "降序", "排序类型.降序"),
            // dict.sys.sort.type.desc
            ("dict.sys.sort.type.desc", "zh-HK", "降序_hk", "排序类型.降序"),

            // dict.sys.storage.naming.config.0
            ("dict.sys.storage.naming.config.0", "en-US", "原文件+哈希值_us", "存储命名规则.原文件+哈希值"),
            // dict.sys.storage.naming.config.0
            ("dict.sys.storage.naming.config.0", "ja-JP", "原文件+哈希值_jp", "存储命名规则.原文件+哈希值"),
            // dict.sys.storage.naming.config.0
            ("dict.sys.storage.naming.config.0", "zh-CN", "原文件+哈希值", "存储命名规则.原文件+哈希值"),
            // dict.sys.storage.naming.config.0
            ("dict.sys.storage.naming.config.0", "zh-HK", "原文件+哈希值_hk", "存储命名规则.原文件+哈希值"),

            // dict.sys.storage.naming.config.1
            ("dict.sys.storage.naming.config.1", "en-US", "自动生成_us", "存储命名规则.自动生成"),
            // dict.sys.storage.naming.config.1
            ("dict.sys.storage.naming.config.1", "ja-JP", "自动生成_jp", "存储命名规则.自动生成"),
            // dict.sys.storage.naming.config.1
            ("dict.sys.storage.naming.config.1", "zh-CN", "自动生成", "存储命名规则.自动生成"),
            // dict.sys.storage.naming.config.1
            ("dict.sys.storage.naming.config.1", "zh-HK", "自动生成_hk", "存储命名规则.自动生成"),

            // dict.sys.storage.naming.config.2
            ("dict.sys.storage.naming.config.2", "en-US", "自定义_us", "存储命名规则.自定义"),
            // dict.sys.storage.naming.config.2
            ("dict.sys.storage.naming.config.2", "ja-JP", "自定义_jp", "存储命名规则.自定义"),
            // dict.sys.storage.naming.config.2
            ("dict.sys.storage.naming.config.2", "zh-CN", "自定义", "存储命名规则.自定义"),
            // dict.sys.storage.naming.config.2
            ("dict.sys.storage.naming.config.2", "zh-HK", "自定义_hk", "存储命名规则.自定义"),

            // dict.sys.storage.type.0
            ("dict.sys.storage.type.0", "en-US", "本地存储_us", "存储方式.本地存储"),
            // dict.sys.storage.type.0
            ("dict.sys.storage.type.0", "ja-JP", "本地存储_jp", "存储方式.本地存储"),
            // dict.sys.storage.type.0
            ("dict.sys.storage.type.0", "zh-CN", "本地存储", "存储方式.本地存储"),
            // dict.sys.storage.type.0
            ("dict.sys.storage.type.0", "zh-HK", "本地存储_hk", "存储方式.本地存储"),

            // dict.sys.storage.type.1
            ("dict.sys.storage.type.1", "en-US", "oss对象存储_us", "存储方式.oss对象存储"),
            // dict.sys.storage.type.1
            ("dict.sys.storage.type.1", "ja-JP", "oss对象存储_jp", "存储方式.oss对象存储"),
            // dict.sys.storage.type.1
            ("dict.sys.storage.type.1", "zh-CN", "oss对象存储", "存储方式.oss对象存储"),
            // dict.sys.storage.type.1
            ("dict.sys.storage.type.1", "zh-HK", "oss对象存储_hk", "存储方式.oss对象存储"),

            // dict.sys.storage.type.2
            ("dict.sys.storage.type.2", "en-US", "ftp_us", "存储方式.ftp"),
            // dict.sys.storage.type.2
            ("dict.sys.storage.type.2", "ja-JP", "ftp_jp", "存储方式.ftp"),
            // dict.sys.storage.type.2
            ("dict.sys.storage.type.2", "zh-CN", "ftp", "存储方式.ftp"),
            // dict.sys.storage.type.2
            ("dict.sys.storage.type.2", "zh-HK", "ftp_hk", "存储方式.ftp"),

            // dict.sys.urgency.level.category.1
            ("dict.sys.urgency.level.category.1", "en-US", "高_us", "紧急度.高"),
            // dict.sys.urgency.level.category.1
            ("dict.sys.urgency.level.category.1", "ja-JP", "高_jp", "紧急度.高"),
            // dict.sys.urgency.level.category.1
            ("dict.sys.urgency.level.category.1", "zh-CN", "高", "紧急度.高"),
            // dict.sys.urgency.level.category.1
            ("dict.sys.urgency.level.category.1", "zh-HK", "高_hk", "紧急度.高"),

            // dict.sys.urgency.level.category.2
            ("dict.sys.urgency.level.category.2", "en-US", "中_us", "紧急度.中"),
            // dict.sys.urgency.level.category.2
            ("dict.sys.urgency.level.category.2", "ja-JP", "中_jp", "紧急度.中"),
            // dict.sys.urgency.level.category.2
            ("dict.sys.urgency.level.category.2", "zh-CN", "中", "紧急度.中"),
            // dict.sys.urgency.level.category.2
            ("dict.sys.urgency.level.category.2", "zh-HK", "中_hk", "紧急度.中"),

            // dict.sys.urgency.level.category.3
            ("dict.sys.urgency.level.category.3", "en-US", "低_us", "紧急度.低"),
            // dict.sys.urgency.level.category.3
            ("dict.sys.urgency.level.category.3", "ja-JP", "低_jp", "紧急度.低"),
            // dict.sys.urgency.level.category.3
            ("dict.sys.urgency.level.category.3", "zh-CN", "低", "紧急度.低"),
            // dict.sys.urgency.level.category.3
            ("dict.sys.urgency.level.category.3", "zh-HK", "低_hk", "紧急度.低"),

            // dict.sys.impact.level.category.1
            ("dict.sys.impact.level.category.1", "en-US", "高_us", "影响范围.高"),
            // dict.sys.impact.level.category.1
            ("dict.sys.impact.level.category.1", "ja-JP", "高_jp", "影响范围.高"),
            // dict.sys.impact.level.category.1
            ("dict.sys.impact.level.category.1", "zh-CN", "高", "影响范围.高"),
            // dict.sys.impact.level.category.1
            ("dict.sys.impact.level.category.1", "zh-HK", "高_hk", "影响范围.高"),

            // dict.sys.impact.level.category.2
            ("dict.sys.impact.level.category.2", "en-US", "中_us", "影响范围.中"),
            // dict.sys.impact.level.category.2
            ("dict.sys.impact.level.category.2", "ja-JP", "中_jp", "影响范围.中"),
            // dict.sys.impact.level.category.2
            ("dict.sys.impact.level.category.2", "zh-CN", "中", "影响范围.中"),
            // dict.sys.impact.level.category.2
            ("dict.sys.impact.level.category.2", "zh-HK", "中_hk", "影响范围.中"),

            // dict.sys.impact.level.category.3
            ("dict.sys.impact.level.category.3", "en-US", "低_us", "影响范围.低"),
            // dict.sys.impact.level.category.3
            ("dict.sys.impact.level.category.3", "ja-JP", "低_jp", "影响范围.低"),
            // dict.sys.impact.level.category.3
            ("dict.sys.impact.level.category.3", "zh-CN", "低", "影响范围.低"),
            // dict.sys.impact.level.category.3
            ("dict.sys.impact.level.category.3", "zh-HK", "低_hk", "影响范围.低"),

            // dict.sys.user.gender.category.0
            ("dict.sys.user.gender.category.0", "en-US", "未知_us", "用户性别.未知"),
            // dict.sys.user.gender.category.0
            ("dict.sys.user.gender.category.0", "ja-JP", "未知_jp", "用户性别.未知"),
            // dict.sys.user.gender.category.0
            ("dict.sys.user.gender.category.0", "zh-CN", "未知", "用户性别.未知"),
            // dict.sys.user.gender.category.0
            ("dict.sys.user.gender.category.0", "zh-HK", "未知_hk", "用户性别.未知"),

            // dict.sys.user.gender.category.1
            ("dict.sys.user.gender.category.1", "en-US", "男_us", "用户性别.男"),
            // dict.sys.user.gender.category.1
            ("dict.sys.user.gender.category.1", "ja-JP", "男_jp", "用户性别.男"),
            // dict.sys.user.gender.category.1
            ("dict.sys.user.gender.category.1", "zh-CN", "男", "用户性别.男"),
            // dict.sys.user.gender.category.1
            ("dict.sys.user.gender.category.1", "zh-HK", "男_hk", "用户性别.男"),

            // dict.sys.user.gender.category.2
            ("dict.sys.user.gender.category.2", "en-US", "女_us", "用户性别.女"),
            // dict.sys.user.gender.category.2
            ("dict.sys.user.gender.category.2", "ja-JP", "女_jp", "用户性别.女"),
            // dict.sys.user.gender.category.2
            ("dict.sys.user.gender.category.2", "zh-CN", "女", "用户性别.女"),
            // dict.sys.user.gender.category.2
            ("dict.sys.user.gender.category.2", "zh-HK", "女_hk", "用户性别.女"),

            // dict.sys.user.type.0
            ("dict.sys.user.type.0", "en-US", "普通用户_us", "用户类型.普通用户"),
            // dict.sys.user.type.0
            ("dict.sys.user.type.0", "ja-JP", "普通用户_jp", "用户类型.普通用户"),
            // dict.sys.user.type.0
            ("dict.sys.user.type.0", "zh-CN", "普通用户", "用户类型.普通用户"),
            // dict.sys.user.type.0
            ("dict.sys.user.type.0", "zh-HK", "普通用户_hk", "用户类型.普通用户"),

            // dict.sys.user.type.1
            ("dict.sys.user.type.1", "en-US", "管理员_us", "用户类型.管理员"),
            // dict.sys.user.type.1
            ("dict.sys.user.type.1", "ja-JP", "管理员_jp", "用户类型.管理员"),
            // dict.sys.user.type.1
            ("dict.sys.user.type.1", "zh-CN", "管理员", "用户类型.管理员"),
            // dict.sys.user.type.1
            ("dict.sys.user.type.1", "zh-HK", "管理员_hk", "用户类型.管理员"),

            // dict.sys.user.type.2
            ("dict.sys.user.type.2", "en-US", "超级管理员_us", "用户类型.超级管理员"),
            // dict.sys.user.type.2
            ("dict.sys.user.type.2", "ja-JP", "超级管理员_jp", "用户类型.超级管理员"),
            // dict.sys.user.type.2
            ("dict.sys.user.type.2", "zh-CN", "超级管理员", "用户类型.超级管理员"),
            // dict.sys.user.type.2
            ("dict.sys.user.type.2", "zh-HK", "超级管理员_hk", "用户类型.超级管理员"),

            // dict.sys.word.category.1
            ("dict.sys.word.category.1", "en-US", "政治敏感_us", "敏感词词性类别.政治敏感"),
            // dict.sys.word.category.1
            ("dict.sys.word.category.1", "ja-JP", "政治敏感_jp", "敏感词词性类别.政治敏感"),
            // dict.sys.word.category.1
            ("dict.sys.word.category.1", "zh-CN", "政治敏感", "敏感词词性类别.政治敏感"),
            // dict.sys.word.category.1
            ("dict.sys.word.category.1", "zh-HK", "政治敏感_hk", "敏感词词性类别.政治敏感"),

            // dict.sys.word.category.2
            ("dict.sys.word.category.2", "en-US", "暴力恐怖_us", "敏感词词性类别.暴力恐怖"),
            // dict.sys.word.category.2
            ("dict.sys.word.category.2", "ja-JP", "暴力恐怖_jp", "敏感词词性类别.暴力恐怖"),
            // dict.sys.word.category.2
            ("dict.sys.word.category.2", "zh-CN", "暴力恐怖", "敏感词词性类别.暴力恐怖"),
            // dict.sys.word.category.2
            ("dict.sys.word.category.2", "zh-HK", "暴力恐怖_hk", "敏感词词性类别.暴力恐怖"),

            // dict.sys.word.category.3
            ("dict.sys.word.category.3", "en-US", "色情低俗_us", "敏感词词性类别.色情低俗"),
            // dict.sys.word.category.3
            ("dict.sys.word.category.3", "ja-JP", "色情低俗_jp", "敏感词词性类别.色情低俗"),
            // dict.sys.word.category.3
            ("dict.sys.word.category.3", "zh-CN", "色情低俗", "敏感词词性类别.色情低俗"),
            // dict.sys.word.category.3
            ("dict.sys.word.category.3", "zh-HK", "色情低俗_hk", "敏感词词性类别.色情低俗"),

            // dict.sys.word.category.4
            ("dict.sys.word.category.4", "en-US", "广告营销_us", "敏感词词性类别.广告营销"),
            // dict.sys.word.category.4
            ("dict.sys.word.category.4", "ja-JP", "广告营销_jp", "敏感词词性类别.广告营销"),
            // dict.sys.word.category.4
            ("dict.sys.word.category.4", "zh-CN", "广告营销", "敏感词词性类别.广告营销"),
            // dict.sys.word.category.4
            ("dict.sys.word.category.4", "zh-HK", "广告营销_hk", "敏感词词性类别.广告营销"),

            // dict.sys.word.category.5
            ("dict.sys.word.category.5", "en-US", "辱骂歧视_us", "敏感词词性类别.辱骂歧视"),
            // dict.sys.word.category.5
            ("dict.sys.word.category.5", "ja-JP", "辱骂歧视_jp", "敏感词词性类别.辱骂歧视"),
            // dict.sys.word.category.5
            ("dict.sys.word.category.5", "zh-CN", "辱骂歧视", "敏感词词性类别.辱骂歧视"),
            // dict.sys.word.category.5
            ("dict.sys.word.category.5", "zh-HK", "辱骂歧视_hk", "敏感词词性类别.辱骂歧视"),

            // dict.sys.iso.code.category.1
            ("dict.sys.iso.code.category.1", "en-US", "部门_us", "ISO编码类别.部门"),
            // dict.sys.iso.code.category.1
            ("dict.sys.iso.code.category.1", "ja-JP", "部门_jp", "ISO编码类别.部门"),
            // dict.sys.iso.code.category.1
            ("dict.sys.iso.code.category.1", "zh-CN", "部门", "ISO编码类别.部门"),
            // dict.sys.iso.code.category.1
            ("dict.sys.iso.code.category.1", "zh-HK", "部门_hk", "ISO编码类别.部门"),

            // dict.sys.iso.code.category.2
            ("dict.sys.iso.code.category.2", "en-US", "公司_us", "ISO编码类别.公司"),
            // dict.sys.iso.code.category.2
            ("dict.sys.iso.code.category.2", "ja-JP", "公司_jp", "ISO编码类别.公司"),
            // dict.sys.iso.code.category.2
            ("dict.sys.iso.code.category.2", "zh-CN", "公司", "ISO编码类别.公司"),
            // dict.sys.iso.code.category.2
            ("dict.sys.iso.code.category.2", "zh-HK", "公司_hk", "ISO编码类别.公司"),

            // dict.sys.iso.code.category.3
            ("dict.sys.iso.code.category.3", "en-US", "产品_us", "ISO编码类别.产品"),
            // dict.sys.iso.code.category.3
            ("dict.sys.iso.code.category.3", "ja-JP", "产品_jp", "ISO编码类别.产品"),
            // dict.sys.iso.code.category.3
            ("dict.sys.iso.code.category.3", "zh-CN", "产品", "ISO编码类别.产品"),
            // dict.sys.iso.code.category.3
            ("dict.sys.iso.code.category.3", "zh-HK", "产品_hk", "ISO编码类别.产品"),

            // dict.sys.iso.code.category.4
            ("dict.sys.iso.code.category.4", "en-US", "通用_us", "ISO编码类别.通用"),
            // dict.sys.iso.code.category.4
            ("dict.sys.iso.code.category.4", "ja-JP", "通用_jp", "ISO编码类别.通用"),
            // dict.sys.iso.code.category.4
            ("dict.sys.iso.code.category.4", "zh-CN", "通用", "ISO编码类别.通用"),
            // dict.sys.iso.code.category.4
            ("dict.sys.iso.code.category.4", "zh-HK", "通用_hk", "ISO编码类别.通用"),

            // dict.sys.word.filter.level.category.1
            ("dict.sys.word.filter.level.category.1", "en-US", "低_us", "敏感词过滤等级.低"),
            // dict.sys.word.filter.level.category.1
            ("dict.sys.word.filter.level.category.1", "ja-JP", "低_jp", "敏感词过滤等级.低"),
            // dict.sys.word.filter.level.category.1
            ("dict.sys.word.filter.level.category.1", "zh-CN", "低", "敏感词过滤等级.低"),
            // dict.sys.word.filter.level.category.1
            ("dict.sys.word.filter.level.category.1", "zh-HK", "低_hk", "敏感词过滤等级.低"),

            // dict.sys.word.filter.level.category.2
            ("dict.sys.word.filter.level.category.2", "en-US", "中_us", "敏感词过滤等级.中"),
            // dict.sys.word.filter.level.category.2
            ("dict.sys.word.filter.level.category.2", "ja-JP", "中_jp", "敏感词过滤等级.中"),
            // dict.sys.word.filter.level.category.2
            ("dict.sys.word.filter.level.category.2", "zh-CN", "中", "敏感词过滤等级.中"),
            // dict.sys.word.filter.level.category.2
            ("dict.sys.word.filter.level.category.2", "zh-HK", "中_hk", "敏感词过滤等级.中"),

            // dict.sys.word.filter.level.category.3
            ("dict.sys.word.filter.level.category.3", "en-US", "高_us", "敏感词过滤等级.高"),
            // dict.sys.word.filter.level.category.3
            ("dict.sys.word.filter.level.category.3", "ja-JP", "高_jp", "敏感词过滤等级.高"),
            // dict.sys.word.filter.level.category.3
            ("dict.sys.word.filter.level.category.3", "zh-CN", "高", "敏感词过滤等级.高"),
            // dict.sys.word.filter.level.category.3
            ("dict.sys.word.filter.level.category.3", "zh-HK", "高_hk", "敏感词过滤等级.高"),

            // dict.sys.yes.no.type.1
            ("dict.sys.yes.no.type.1", "en-US", "是_us", "是否.是"),
            // dict.sys.yes.no.type.1
            ("dict.sys.yes.no.type.1", "ja-JP", "是_jp", "是否.是"),
            // dict.sys.yes.no.type.1
            ("dict.sys.yes.no.type.1", "zh-CN", "是", "是否.是"),
            // dict.sys.yes.no.type.1
            ("dict.sys.yes.no.type.1", "zh-HK", "是_hk", "是否.是"),

            // dict.sys.yes.no.type.0
            ("dict.sys.yes.no.type.0", "en-US", "否_us", "是否.否"),
            // dict.sys.yes.no.type.0
            ("dict.sys.yes.no.type.0", "ja-JP", "否_jp", "是否.否"),
            // dict.sys.yes.no.type.0
            ("dict.sys.yes.no.type.0", "zh-CN", "否", "是否.否"),
            // dict.sys.yes.no.type.0
            ("dict.sys.yes.no.type.0", "zh-HK", "否_hk", "是否.否"),

            // dict.sys.numbering.date.format.config.none
            ("dict.sys.numbering.date.format.config.none", "en-US", "不使用_us", "编号日期格式.不使用"),
            // dict.sys.numbering.date.format.config.none
            ("dict.sys.numbering.date.format.config.none", "ja-JP", "不使用_jp", "编号日期格式.不使用"),
            // dict.sys.numbering.date.format.config.none
            ("dict.sys.numbering.date.format.config.none", "zh-CN", "不使用", "编号日期格式.不使用"),
            // dict.sys.numbering.date.format.config.none
            ("dict.sys.numbering.date.format.config.none", "zh-HK", "不使用_hk", "编号日期格式.不使用"),

            // dict.sys.numbering.date.format.config.yyyy
            ("dict.sys.numbering.date.format.config.yyyy", "en-US", "年(yyyy)_us", "编号日期格式.年"),
            // dict.sys.numbering.date.format.config.yyyy
            ("dict.sys.numbering.date.format.config.yyyy", "ja-JP", "年(yyyy)_jp", "编号日期格式.年"),
            // dict.sys.numbering.date.format.config.yyyy
            ("dict.sys.numbering.date.format.config.yyyy", "zh-CN", "年(yyyy)", "编号日期格式.年"),
            // dict.sys.numbering.date.format.config.yyyy
            ("dict.sys.numbering.date.format.config.yyyy", "zh-HK", "年(yyyy)_hk", "编号日期格式.年"),

            // dict.sys.numbering.date.format.config.yyyymm
            ("dict.sys.numbering.date.format.config.yyyymm", "en-US", "年月(yyyyMM)_us", "编号日期格式.年月"),
            // dict.sys.numbering.date.format.config.yyyymm
            ("dict.sys.numbering.date.format.config.yyyymm", "ja-JP", "年月(yyyyMM)_jp", "编号日期格式.年月"),
            // dict.sys.numbering.date.format.config.yyyymm
            ("dict.sys.numbering.date.format.config.yyyymm", "zh-CN", "年月(yyyyMM)", "编号日期格式.年月"),
            // dict.sys.numbering.date.format.config.yyyymm
            ("dict.sys.numbering.date.format.config.yyyymm", "zh-HK", "年月(yyyyMM)_hk", "编号日期格式.年月"),

            // dict.sys.numbering.date.format.config.yyyymmdd
            ("dict.sys.numbering.date.format.config.yyyymmdd", "en-US", "年月日(yyyyMMdd)_us", "编号日期格式.年月日"),
            // dict.sys.numbering.date.format.config.yyyymmdd
            ("dict.sys.numbering.date.format.config.yyyymmdd", "ja-JP", "年月日(yyyyMMdd)_jp", "编号日期格式.年月日"),
            // dict.sys.numbering.date.format.config.yyyymmdd
            ("dict.sys.numbering.date.format.config.yyyymmdd", "zh-CN", "年月日(yyyyMMdd)", "编号日期格式.年月日"),
            // dict.sys.numbering.date.format.config.yyyymmdd
            ("dict.sys.numbering.date.format.config.yyyymmdd", "zh-HK", "年月日(yyyyMMdd)_hk", "编号日期格式.年月日"),

            // dict.sys.numbering.date.format.config.yyyymmddhh
            ("dict.sys.numbering.date.format.config.yyyymmddhh", "en-US", "年月日时(yyyyMMddHH)_us", "编号日期格式.年月日时"),
            // dict.sys.numbering.date.format.config.yyyymmddhh
            ("dict.sys.numbering.date.format.config.yyyymmddhh", "ja-JP", "年月日时(yyyyMMddHH)_jp", "编号日期格式.年月日时"),
            // dict.sys.numbering.date.format.config.yyyymmddhh
            ("dict.sys.numbering.date.format.config.yyyymmddhh", "zh-CN", "年月日时(yyyyMMddHH)", "编号日期格式.年月日时"),
            // dict.sys.numbering.date.format.config.yyyymmddhh
            ("dict.sys.numbering.date.format.config.yyyymmddhh", "zh-HK", "年月日时(yyyyMMddHH)_hk", "编号日期格式.年月日时"),

            // dict.routine.ticket.source.type.0
            ("dict.routine.ticket.source.type.0", "en-US", "门户网站_us", "工单来源.门户"),
            // dict.routine.ticket.source.type.0
            ("dict.routine.ticket.source.type.0", "ja-JP", "门户网站_jp", "工单来源.门户"),
            // dict.routine.ticket.source.type.0
            ("dict.routine.ticket.source.type.0", "zh-CN", "门户网站", "工单来源.门户"),
            // dict.routine.ticket.source.type.0
            ("dict.routine.ticket.source.type.0", "zh-HK", "门户网站_hk", "工单来源.门户"),

            // dict.routine.ticket.source.type.1
            ("dict.routine.ticket.source.type.1", "en-US", "邮件_us", "工单来源.邮件"),
            // dict.routine.ticket.source.type.1
            ("dict.routine.ticket.source.type.1", "ja-JP", "邮件_jp", "工单来源.邮件"),
            // dict.routine.ticket.source.type.1
            ("dict.routine.ticket.source.type.1", "zh-CN", "邮件", "工单来源.邮件"),
            // dict.routine.ticket.source.type.1
            ("dict.routine.ticket.source.type.1", "zh-HK", "邮件_hk", "工单来源.邮件"),

            // dict.routine.ticket.source.type.2
            ("dict.routine.ticket.source.type.2", "en-US", "电话_us", "工单来源.电话"),
            // dict.routine.ticket.source.type.2
            ("dict.routine.ticket.source.type.2", "ja-JP", "电话_jp", "工单来源.电话"),
            // dict.routine.ticket.source.type.2
            ("dict.routine.ticket.source.type.2", "zh-CN", "电话", "工单来源.电话"),
            // dict.routine.ticket.source.type.2
            ("dict.routine.ticket.source.type.2", "zh-HK", "电话_hk", "工单来源.电话"),

            // dict.routine.ticket.source.type.3
            ("dict.routine.ticket.source.type.3", "en-US", "API接入_us", "工单来源.API"),
            // dict.routine.ticket.source.type.3
            ("dict.routine.ticket.source.type.3", "ja-JP", "API接入_jp", "工单来源.API"),
            // dict.routine.ticket.source.type.3
            ("dict.routine.ticket.source.type.3", "zh-CN", "API接入", "工单来源.API"),
            // dict.routine.ticket.source.type.3
            ("dict.routine.ticket.source.type.3", "zh-HK", "API接入_hk", "工单来源.API"),

            // dict.sys.warrantytype.0
            ("dict.sys.warrantytype.0", "en-US", "原厂保修_us", "保修类型.原厂保修"),
            // dict.sys.warrantytype.0
            ("dict.sys.warrantytype.0", "ja-JP", "原厂保修_jp", "保修类型.原厂保修"),
            // dict.sys.warrantytype.0
            ("dict.sys.warrantytype.0", "zh-CN", "原厂保修", "保修类型.原厂保修"),
            // dict.sys.warrantytype.0
            ("dict.sys.warrantytype.0", "zh-HK", "原厂保修_hk", "保修类型.原厂保修"),

            // dict.sys.warrantytype.1
            ("dict.sys.warrantytype.1", "en-US", "延长保修_us", "保修类型.延长保修"),
            // dict.sys.warrantytype.1
            ("dict.sys.warrantytype.1", "ja-JP", "延长保修_jp", "保修类型.延长保修"),
            // dict.sys.warrantytype.1
            ("dict.sys.warrantytype.1", "zh-CN", "延长保修", "保修类型.延长保修"),
            // dict.sys.warrantytype.1
            ("dict.sys.warrantytype.1", "zh-HK", "延长保修_hk", "保修类型.延长保修"),

            // dict.sys.warrantytype.2
            ("dict.sys.warrantytype.2", "en-US", "上门保修_us", "保修类型.上门保修"),
            // dict.sys.warrantytype.2
            ("dict.sys.warrantytype.2", "ja-JP", "上门保修_jp", "保修类型.上门保修"),
            // dict.sys.warrantytype.2
            ("dict.sys.warrantytype.2", "zh-CN", "上门保修", "保修类型.上门保修"),
            // dict.sys.warrantytype.2
            ("dict.sys.warrantytype.2", "zh-HK", "上门保修_hk", "保修类型.上门保修"),

            // dict.sys.warrantytype.3
            ("dict.sys.warrantytype.3", "en-US", "寄修保修_us", "保修类型.寄修保修"),
            // dict.sys.warrantytype.3
            ("dict.sys.warrantytype.3", "ja-JP", "寄修保修_jp", "保修类型.寄修保修"),
            // dict.sys.warrantytype.3
            ("dict.sys.warrantytype.3", "zh-CN", "寄修保修", "保修类型.寄修保修"),
            // dict.sys.warrantytype.3
            ("dict.sys.warrantytype.3", "zh-HK", "寄修保修_hk", "保修类型.寄修保修"),

            // dict.sys.warrantytype.4
            ("dict.sys.warrantytype.4", "en-US", "维保合同_us", "保修类型.维保合同"),
            // dict.sys.warrantytype.4
            ("dict.sys.warrantytype.4", "ja-JP", "维保合同_jp", "保修类型.维保合同"),
            // dict.sys.warrantytype.4
            ("dict.sys.warrantytype.4", "zh-CN", "维保合同", "保修类型.维保合同"),
            // dict.sys.warrantytype.4
            ("dict.sys.warrantytype.4", "zh-HK", "维保合同_hk", "保修类型.维保合同"),

            // dict.sys.warrantytype.5
            ("dict.sys.warrantytype.5", "en-US", "付费保养_us", "保修类型.付费保养"),
            // dict.sys.warrantytype.5
            ("dict.sys.warrantytype.5", "ja-JP", "付费保养_jp", "保修类型.付费保养"),
            // dict.sys.warrantytype.5
            ("dict.sys.warrantytype.5", "zh-CN", "付费保养", "保修类型.付费保养"),
            // dict.sys.warrantytype.5
            ("dict.sys.warrantytype.5", "zh-HK", "付费保养_hk", "保修类型.付费保养"),

            // dict.sys.lifecycle.status.1
            ("dict.sys.lifecycle.status.1", "en-US", "编制中_us", "生命周期.编制中"),
            // dict.sys.lifecycle.status.1
            ("dict.sys.lifecycle.status.1", "ja-JP", "编制中_jp", "生命周期.编制中"),
            // dict.sys.lifecycle.status.1
            ("dict.sys.lifecycle.status.1", "zh-CN", "编制中", "生命周期.编制中"),
            // dict.sys.lifecycle.status.1
            ("dict.sys.lifecycle.status.1", "zh-HK", "编制中_hk", "生命周期.编制中"),

            // dict.sys.lifecycle.status.2
            ("dict.sys.lifecycle.status.2", "en-US", "审核中_us", "生命周期.审核中"),
            // dict.sys.lifecycle.status.2
            ("dict.sys.lifecycle.status.2", "ja-JP", "审核中_jp", "生命周期.审核中"),
            // dict.sys.lifecycle.status.2
            ("dict.sys.lifecycle.status.2", "zh-CN", "审核中", "生命周期.审核中"),
            // dict.sys.lifecycle.status.2
            ("dict.sys.lifecycle.status.2", "zh-HK", "审核中_hk", "生命周期.审核中"),

            // dict.sys.lifecycle.status.3
            ("dict.sys.lifecycle.status.3", "en-US", "已生效_us", "生命周期.已生效"),
            // dict.sys.lifecycle.status.3
            ("dict.sys.lifecycle.status.3", "ja-JP", "已生效_jp", "生命周期.已生效"),
            // dict.sys.lifecycle.status.3
            ("dict.sys.lifecycle.status.3", "zh-CN", "已生效", "生命周期.已生效"),
            // dict.sys.lifecycle.status.3
            ("dict.sys.lifecycle.status.3", "zh-HK", "已生效_hk", "生命周期.已生效"),

            // dict.sys.lifecycle.status.4
            ("dict.sys.lifecycle.status.4", "en-US", "已废止_us", "生命周期.已废止"),
            // dict.sys.lifecycle.status.4
            ("dict.sys.lifecycle.status.4", "ja-JP", "已废止_jp", "生命周期.已废止"),
            // dict.sys.lifecycle.status.4
            ("dict.sys.lifecycle.status.4", "zh-CN", "已废止", "生命周期.已废止"),
            // dict.sys.lifecycle.status.4
            ("dict.sys.lifecycle.status.4", "zh-HK", "已废止_hk", "生命周期.已废止"),

            // dict.sys.attachment.file.type.1
            ("dict.sys.attachment.file.type.1", "en-US", "图片_us", "附件文件类型.图片"),
            // dict.sys.attachment.file.type.1
            ("dict.sys.attachment.file.type.1", "ja-JP", "图片_jp", "附件文件类型.图片"),
            // dict.sys.attachment.file.type.1
            ("dict.sys.attachment.file.type.1", "zh-CN", "图片", "附件文件类型.图片"),
            // dict.sys.attachment.file.type.1
            ("dict.sys.attachment.file.type.1", "zh-HK", "图片_hk", "附件文件类型.图片"),

            // dict.sys.attachment.file.type.2
            ("dict.sys.attachment.file.type.2", "en-US", "视频_us", "附件文件类型.视频"),
            // dict.sys.attachment.file.type.2
            ("dict.sys.attachment.file.type.2", "ja-JP", "视频_jp", "附件文件类型.视频"),
            // dict.sys.attachment.file.type.2
            ("dict.sys.attachment.file.type.2", "zh-CN", "视频", "附件文件类型.视频"),
            // dict.sys.attachment.file.type.2
            ("dict.sys.attachment.file.type.2", "zh-HK", "视频_hk", "附件文件类型.视频"),

            // dict.sys.attachment.file.type.3
            ("dict.sys.attachment.file.type.3", "en-US", "文档_us", "附件文件类型.文档"),
            // dict.sys.attachment.file.type.3
            ("dict.sys.attachment.file.type.3", "ja-JP", "文档_jp", "附件文件类型.文档"),
            // dict.sys.attachment.file.type.3
            ("dict.sys.attachment.file.type.3", "zh-CN", "文档", "附件文件类型.文档"),
            // dict.sys.attachment.file.type.3
            ("dict.sys.attachment.file.type.3", "zh-HK", "文档_hk", "附件文件类型.文档"),

            // dict.sys.workstation.type.1
            ("dict.sys.workstation.type.1", "en-US", "装配_us", "工位类型.装配"),
            // dict.sys.workstation.type.1
            ("dict.sys.workstation.type.1", "ja-JP", "装配_jp", "工位类型.装配"),
            // dict.sys.workstation.type.1
            ("dict.sys.workstation.type.1", "zh-CN", "装配", "工位类型.装配"),
            // dict.sys.workstation.type.1
            ("dict.sys.workstation.type.1", "zh-HK", "装配_hk", "工位类型.装配"),

            // dict.sys.workstation.type.2
            ("dict.sys.workstation.type.2", "en-US", "检验_us", "工位类型.检验"),
            // dict.sys.workstation.type.2
            ("dict.sys.workstation.type.2", "ja-JP", "检验_jp", "工位类型.检验"),
            // dict.sys.workstation.type.2
            ("dict.sys.workstation.type.2", "zh-CN", "检验", "工位类型.检验"),
            // dict.sys.workstation.type.2
            ("dict.sys.workstation.type.2", "zh-HK", "检验_hk", "工位类型.检验"),

            // dict.sys.workstation.type.3
            ("dict.sys.workstation.type.3", "en-US", "包装_us", "工位类型.包装"),
            // dict.sys.workstation.type.3
            ("dict.sys.workstation.type.3", "ja-JP", "包装_jp", "工位类型.包装"),
            // dict.sys.workstation.type.3
            ("dict.sys.workstation.type.3", "zh-CN", "包装", "工位类型.包装"),
            // dict.sys.workstation.type.3
            ("dict.sys.workstation.type.3", "zh-HK", "包装_hk", "工位类型.包装"),

            // dict.sys.workstation.type.4
            ("dict.sys.workstation.type.4", "en-US", "测试_us", "工位类型.测试"),
            // dict.sys.workstation.type.4
            ("dict.sys.workstation.type.4", "ja-JP", "测试_jp", "工位类型.测试"),
            // dict.sys.workstation.type.4
            ("dict.sys.workstation.type.4", "zh-CN", "测试", "工位类型.测试"),
            // dict.sys.workstation.type.4
            ("dict.sys.workstation.type.4", "zh-HK", "测试_hk", "工位类型.测试"),

            // dict.sys.workstation.type.5
            ("dict.sys.workstation.type.5", "en-US", "其他_us", "工位类型.其他"),
            // dict.sys.workstation.type.5
            ("dict.sys.workstation.type.5", "ja-JP", "其他_jp", "工位类型.其他"),
            // dict.sys.workstation.type.5
            ("dict.sys.workstation.type.5", "zh-CN", "其他", "工位类型.其他"),
            // dict.sys.workstation.type.5
            ("dict.sys.workstation.type.5", "zh-HK", "其他_hk", "工位类型.其他"),

            // dict.hr.benefit.category.
            ("dict.hr.benefit.category.", "en-US", "保险_us", "福利大类.保险"),
            // dict.hr.benefit.category.
            ("dict.hr.benefit.category.", "ja-JP", "保险_jp", "福利大类.保险"),
            // dict.hr.benefit.category.
            ("dict.hr.benefit.category.", "zh-CN", "保险", "福利大类.保险"),
            // dict.hr.benefit.category.
            ("dict.hr.benefit.category.", "zh-HK", "保险_hk", "福利大类.保险"),

            // dict.hr.benefit.type.
            ("dict.hr.benefit.type.", "en-US", "社保_us", "福利类型.社保"),
            // dict.hr.benefit.type.
            ("dict.hr.benefit.type.", "ja-JP", "社保_jp", "福利类型.社保"),
            // dict.hr.benefit.type.
            ("dict.hr.benefit.type.", "zh-CN", "社保", "福利类型.社保"),
            // dict.hr.benefit.type.
            ("dict.hr.benefit.type.", "zh-HK", "社保_hk", "福利类型.社保"),

            // dict.hr.benefit.payment.cycle.type.
            ("dict.hr.benefit.payment.cycle.type.", "en-US", "月度_us", "福利发放周期.月度"),
            // dict.hr.benefit.payment.cycle.type.
            ("dict.hr.benefit.payment.cycle.type.", "ja-JP", "月度_jp", "福利发放周期.月度"),
            // dict.hr.benefit.payment.cycle.type.
            ("dict.hr.benefit.payment.cycle.type.", "zh-CN", "月度", "福利发放周期.月度"),
            // dict.hr.benefit.payment.cycle.type.
            ("dict.hr.benefit.payment.cycle.type.", "zh-HK", "月度_hk", "福利发放周期.月度"),

            // dict.hr.emp.benefit.plan.status.
            ("dict.hr.emp.benefit.plan.status.", "en-US", "待生效_us", "员工福利方案状态.待生效"),
            // dict.hr.emp.benefit.plan.status.
            ("dict.hr.emp.benefit.plan.status.", "ja-JP", "待生效_jp", "员工福利方案状态.待生效"),
            // dict.hr.emp.benefit.plan.status.
            ("dict.hr.emp.benefit.plan.status.", "zh-CN", "待生效", "员工福利方案状态.待生效"),
            // dict.hr.emp.benefit.plan.status.
            ("dict.hr.emp.benefit.plan.status.", "zh-HK", "待生效_hk", "员工福利方案状态.待生效"),

            // dict.hr.comp.bonus.type.
            ("dict.hr.comp.bonus.type.", "en-US", "绩效奖金_us", "奖金类型.绩效奖金"),
            // dict.hr.comp.bonus.type.
            ("dict.hr.comp.bonus.type.", "ja-JP", "绩效奖金_jp", "奖金类型.绩效奖金"),
            // dict.hr.comp.bonus.type.
            ("dict.hr.comp.bonus.type.", "zh-CN", "绩效奖金", "奖金类型.绩效奖金"),
            // dict.hr.comp.bonus.type.
            ("dict.hr.comp.bonus.type.", "zh-HK", "绩效奖金_hk", "奖金类型.绩效奖金"),

            // dict.hr.comp.bonus.calc.method.type.
            ("dict.hr.comp.bonus.calc.method.type.", "en-US", "固定金额_us", "奖金计算方式.固定金额"),
            // dict.hr.comp.bonus.calc.method.type.
            ("dict.hr.comp.bonus.calc.method.type.", "ja-JP", "固定金额_jp", "奖金计算方式.固定金额"),
            // dict.hr.comp.bonus.calc.method.type.
            ("dict.hr.comp.bonus.calc.method.type.", "zh-CN", "固定金额", "奖金计算方式.固定金额"),
            // dict.hr.comp.bonus.calc.method.type.
            ("dict.hr.comp.bonus.calc.method.type.", "zh-HK", "固定金额_hk", "奖金计算方式.固定金额"),

            // dict.hr.salary.item.type.
            ("dict.hr.salary.item.type.", "en-US", "基本工资_us", "薪资项目类型.基本工资"),
            // dict.hr.salary.item.type.
            ("dict.hr.salary.item.type.", "ja-JP", "基本工资_jp", "薪资项目类型.基本工资"),
            // dict.hr.salary.item.type.
            ("dict.hr.salary.item.type.", "zh-CN", "基本工资", "薪资项目类型.基本工资"),
            // dict.hr.salary.item.type.
            ("dict.hr.salary.item.type.", "zh-HK", "基本工资_hk", "薪资项目类型.基本工资"),

            // dict.hr.salary.calc.method.type.
            ("dict.hr.salary.calc.method.type.", "en-US", "固定金额_us", "薪资计算方式.固定金额"),
            // dict.hr.salary.calc.method.type.
            ("dict.hr.salary.calc.method.type.", "ja-JP", "固定金额_jp", "薪资计算方式.固定金额"),
            // dict.hr.salary.calc.method.type.
            ("dict.hr.salary.calc.method.type.", "zh-CN", "固定金额", "薪资计算方式.固定金额"),
            // dict.hr.salary.calc.method.type.
            ("dict.hr.salary.calc.method.type.", "zh-HK", "固定金额_hk", "薪资计算方式.固定金额"),

            // dict.hr.salary.formula.step.type.
            ("dict.hr.salary.formula.step.type.", "en-US", "应发_us", "薪资公式步骤.应发"),
            // dict.hr.salary.formula.step.type.
            ("dict.hr.salary.formula.step.type.", "ja-JP", "应发_jp", "薪资公式步骤.应发"),
            // dict.hr.salary.formula.step.type.
            ("dict.hr.salary.formula.step.type.", "zh-CN", "应发", "薪资公式步骤.应发"),
            // dict.hr.salary.formula.step.type.
            ("dict.hr.salary.formula.step.type.", "zh-HK", "应发_hk", "薪资公式步骤.应发"),

            // dict.hr.social.insurance.pay.status.
            ("dict.hr.social.insurance.pay.status.", "en-US", "待缴纳_us", "社保缴纳状态.待缴纳"),
            // dict.hr.social.insurance.pay.status.
            ("dict.hr.social.insurance.pay.status.", "ja-JP", "待缴纳_jp", "社保缴纳状态.待缴纳"),
            // dict.hr.social.insurance.pay.status.
            ("dict.hr.social.insurance.pay.status.", "zh-CN", "待缴纳", "社保缴纳状态.待缴纳"),
            // dict.hr.social.insurance.pay.status.
            ("dict.hr.social.insurance.pay.status.", "zh-HK", "待缴纳_hk", "社保缴纳状态.待缴纳"),

            // dict.logistics.process.segment.type.
            ("dict.logistics.process.segment.type.", "en-US", "SMT_us", "工艺段类型.SMT"),
            // dict.logistics.process.segment.type.
            ("dict.logistics.process.segment.type.", "ja-JP", "SMT_jp", "工艺段类型.SMT"),
            // dict.logistics.process.segment.type.
            ("dict.logistics.process.segment.type.", "zh-CN", "SMT", "工艺段类型.SMT"),
            // dict.logistics.process.segment.type.
            ("dict.logistics.process.segment.type.", "zh-HK", "SMT_hk", "工艺段类型.SMT"),

            // dict.logistics.sop.andon.type.
            ("dict.logistics.sop.andon.type.", "en-US", "班长_us", "SOP安灯呼叫类型.班长"),
            // dict.logistics.sop.andon.type.
            ("dict.logistics.sop.andon.type.", "ja-JP", "班长_jp", "SOP安灯呼叫类型.班长"),
            // dict.logistics.sop.andon.type.
            ("dict.logistics.sop.andon.type.", "zh-CN", "班长", "SOP安灯呼叫类型.班长"),
            // dict.logistics.sop.andon.type.
            ("dict.logistics.sop.andon.type.", "zh-HK", "班长_hk", "SOP安灯呼叫类型.班长"),

            // dict.logistics.sop.andon.status.
            ("dict.logistics.sop.andon.status.", "en-US", "待响应_us", "SOP安灯呼叫状态.待响应"),
            // dict.logistics.sop.andon.status.
            ("dict.logistics.sop.andon.status.", "ja-JP", "待响应_jp", "SOP安灯呼叫状态.待响应"),
            // dict.logistics.sop.andon.status.
            ("dict.logistics.sop.andon.status.", "zh-CN", "待响应", "SOP安灯呼叫状态.待响应"),
            // dict.logistics.sop.andon.status.
            ("dict.logistics.sop.andon.status.", "zh-HK", "待响应_hk", "SOP安灯呼叫状态.待响应"),

            // dict.logistics.sop.exec.status.
            ("dict.logistics.sop.exec.status.", "en-US", "进行中_us", "SOP执行状态.进行中"),
            // dict.logistics.sop.exec.status.
            ("dict.logistics.sop.exec.status.", "ja-JP", "进行中_jp", "SOP执行状态.进行中"),
            // dict.logistics.sop.exec.status.
            ("dict.logistics.sop.exec.status.", "zh-CN", "进行中", "SOP执行状态.进行中"),
            // dict.logistics.sop.exec.status.
            ("dict.logistics.sop.exec.status.", "zh-HK", "进行中_hk", "SOP执行状态.进行中"),

            // dict.logistics.sop.check.result.type.
            ("dict.logistics.sop.check.result.type.", "en-US", "合格_us", "SOP检验结果.合格"),
            // dict.logistics.sop.check.result.type.
            ("dict.logistics.sop.check.result.type.", "ja-JP", "合格_jp", "SOP检验结果.合格"),
            // dict.logistics.sop.check.result.type.
            ("dict.logistics.sop.check.result.type.", "zh-CN", "合格", "SOP检验结果.合格"),
            // dict.logistics.sop.check.result.type.
            ("dict.logistics.sop.check.result.type.", "zh-HK", "合格_hk", "SOP检验结果.合格"),

            // dict.logistics.sop.scan.result.type.
            ("dict.logistics.sop.scan.result.type.", "en-US", "PASS_us", "SOP扫码结果.PASS"),
            // dict.logistics.sop.scan.result.type.
            ("dict.logistics.sop.scan.result.type.", "ja-JP", "PASS_jp", "SOP扫码结果.PASS"),
            // dict.logistics.sop.scan.result.type.
            ("dict.logistics.sop.scan.result.type.", "zh-CN", "PASS", "SOP扫码结果.PASS"),
            // dict.logistics.sop.scan.result.type.
            ("dict.logistics.sop.scan.result.type.", "zh-HK", "PASS_hk", "SOP扫码结果.PASS"),
        };
    }

    /// <summary>填充 TaktTranslation 全部业务字段（含租户基类字段）</summary>
    private static void ApplyTranslationFields(
        TaktTranslation translation,
        string tenantCode,
        long cultureId,
        TranslationSeedItem item)
    {
        translation.TenantCode = tenantCode;
        translation.CultureId = cultureId;
        translation.CultureCode = item.CultureCode;
        translation.I18nKey = item.I18nKey;
        translation.TranslationText = item.TranslationText;
        translation.ResourceGroup = "Foundation";
        translation.ResourceType = "frontend";
        translation.ContextNote = item.ContextNote;
        translation.ExtField = null;
        translation.Remark = null;
        translation.IsDeleted = 0;
        translation.DeletedBy = null;
        translation.DeletedAt = null;
    }

    private static async Task<(TaktTranslation Translation, int InsertCount, int UpdateCount)> CreateOrUpdateTranslationAsync(
        ITaktTenantSeedRepository<TaktTranslation> repository,
        string tenantCode,
        long cultureId,
        TranslationSeedItem item)
    {
        var translation = await repository.FirstAsync(t =>
            t.TenantCode == tenantCode &&
            t.I18nKey == item.I18nKey &&
            t.CultureCode == item.CultureCode);

        if (translation == null)
        {
            translation = new TaktTranslation();
            ApplyTranslationFields(translation, tenantCode, cultureId, item);
            translation = await repository.CreateAsync(translation);
            return (translation, 1, 0);
        }

        ApplyTranslationFields(translation, tenantCode, cultureId, item);
        await repository.UpdateAsync(translation);
        return (translation, 0, 1);
    }

    /// <summary>翻译种子项（CultureId 由 SeedAsync 解析）</summary>
    private sealed record TranslationSeedItem(
        string I18nKey,
        string CultureCode,
        string TranslationText,
        string? ContextNote);
}
