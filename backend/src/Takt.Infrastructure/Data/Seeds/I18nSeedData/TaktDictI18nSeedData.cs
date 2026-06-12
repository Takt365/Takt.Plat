// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData
// 文件名称：TaktDictI18nSeedData.cs
// 创建时间：2026-06-01
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
using Takt.Shared.Enums;
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
            ("dict.accounting.account.category.asset", "en-US", "ASSET", "科目类别.资产类"),
            // dict.accounting.account.category.asset
            ("dict.accounting.account.category.asset", "ja-JP", "ASSET", "科目类别.资产类"),
            // dict.accounting.account.category.asset
            ("dict.accounting.account.category.asset", "zh-CN", "资产类", "科目类别.资产类"),
            // dict.accounting.account.category.asset
            ("dict.accounting.account.category.asset", "zh-HK", "资产类", "科目类别.资产类"),

            // dict.accounting.account.category.liability
            ("dict.accounting.account.category.liability", "en-US", "liability", "科目类别.负债类"),
            // dict.accounting.account.category.liability
            ("dict.accounting.account.category.liability", "ja-JP", "liability", "科目类别.负债类"),
            // dict.accounting.account.category.liability
            ("dict.accounting.account.category.liability", "zh-CN", "负债类", "科目类别.负债类"),
            // dict.accounting.account.category.liability
            ("dict.accounting.account.category.liability", "zh-HK", "负债类", "科目类别.负债类"),

            // dict.accounting.account.category.equity
            ("dict.accounting.account.category.equity", "en-US", "equity", "科目类别.权益类"),
            // dict.accounting.account.category.equity
            ("dict.accounting.account.category.equity", "ja-JP", "equity", "科目类别.权益类"),
            // dict.accounting.account.category.equity
            ("dict.accounting.account.category.equity", "zh-CN", "权益类", "科目类别.权益类"),
            // dict.accounting.account.category.equity
            ("dict.accounting.account.category.equity", "zh-HK", "权益类", "科目类别.权益类"),

            // dict.accounting.account.category.cost
            ("dict.accounting.account.category.cost", "en-US", "cost", "科目类别.成本类"),
            // dict.accounting.account.category.cost
            ("dict.accounting.account.category.cost", "ja-JP", "cost", "科目类别.成本类"),
            // dict.accounting.account.category.cost
            ("dict.accounting.account.category.cost", "zh-CN", "成本类", "科目类别.成本类"),
            // dict.accounting.account.category.cost
            ("dict.accounting.account.category.cost", "zh-HK", "成本类", "科目类别.成本类"),

            // dict.accounting.account.category.profit_loss
            ("dict.accounting.account.category.profit_loss", "en-US", "profit_loss", "科目类别.损益类"),
            // dict.accounting.account.category.profit_loss
            ("dict.accounting.account.category.profit_loss", "ja-JP", "profit_loss", "科目类别.损益类"),
            // dict.accounting.account.category.profit_loss
            ("dict.accounting.account.category.profit_loss", "zh-CN", "损益类", "科目类别.损益类"),
            // dict.accounting.account.category.profit_loss
            ("dict.accounting.account.category.profit_loss", "zh-HK", "损益类", "科目类别.损益类"),

            // dict.accounting.account.category.revenue
            ("dict.accounting.account.category.revenue", "en-US", "revenue", "科目类别.收入类"),
            // dict.accounting.account.category.revenue
            ("dict.accounting.account.category.revenue", "ja-JP", "revenue", "科目类别.收入类"),
            // dict.accounting.account.category.revenue
            ("dict.accounting.account.category.revenue", "zh-CN", "收入类", "科目类别.收入类"),
            // dict.accounting.account.category.revenue
            ("dict.accounting.account.category.revenue", "zh-HK", "收入类", "科目类别.收入类"),

            // dict.accounting.account.category.expense
            ("dict.accounting.account.category.expense", "en-US", "expense", "科目类别.费用类"),
            // dict.accounting.account.category.expense
            ("dict.accounting.account.category.expense", "ja-JP", "expense", "科目类别.费用类"),
            // dict.accounting.account.category.expense
            ("dict.accounting.account.category.expense", "zh-CN", "费用类", "科目类别.费用类"),
            // dict.accounting.account.category.expense
            ("dict.accounting.account.category.expense", "zh-HK", "费用类", "科目类别.费用类"),

            // dict.accounting.asset.category.building
            ("dict.accounting.asset.category.building", "en-US", "building", "资产类别.房屋建筑"),
            // dict.accounting.asset.category.building
            ("dict.accounting.asset.category.building", "ja-JP", "building", "资产类别.房屋建筑"),
            // dict.accounting.asset.category.building
            ("dict.accounting.asset.category.building", "zh-CN", "房屋建筑", "资产类别.房屋建筑"),
            // dict.accounting.asset.category.building
            ("dict.accounting.asset.category.building", "zh-HK", "房屋建筑", "资产类别.房屋建筑"),

            // dict.accounting.asset.category.machinery
            ("dict.accounting.asset.category.machinery", "en-US", "machinery", "资产类别.机器设备"),
            // dict.accounting.asset.category.machinery
            ("dict.accounting.asset.category.machinery", "ja-JP", "machinery", "资产类别.机器设备"),
            // dict.accounting.asset.category.machinery
            ("dict.accounting.asset.category.machinery", "zh-CN", "机器设备", "资产类别.机器设备"),
            // dict.accounting.asset.category.machinery
            ("dict.accounting.asset.category.machinery", "zh-HK", "机器设备", "资产类别.机器设备"),

            // dict.accounting.asset.category.vehicle
            ("dict.accounting.asset.category.vehicle", "en-US", "vehicle", "资产类别.运输工具"),
            // dict.accounting.asset.category.vehicle
            ("dict.accounting.asset.category.vehicle", "ja-JP", "vehicle", "资产类别.运输工具"),
            // dict.accounting.asset.category.vehicle
            ("dict.accounting.asset.category.vehicle", "zh-CN", "运输工具", "资产类别.运输工具"),
            // dict.accounting.asset.category.vehicle
            ("dict.accounting.asset.category.vehicle", "zh-HK", "运输工具", "资产类别.运输工具"),

            // dict.accounting.asset.category.electronic
            ("dict.accounting.asset.category.electronic", "en-US", "electronic", "资产类别.电子设备"),
            // dict.accounting.asset.category.electronic
            ("dict.accounting.asset.category.electronic", "ja-JP", "electronic", "资产类别.电子设备"),
            // dict.accounting.asset.category.electronic
            ("dict.accounting.asset.category.electronic", "zh-CN", "电子设备", "资产类别.电子设备"),
            // dict.accounting.asset.category.electronic
            ("dict.accounting.asset.category.electronic", "zh-HK", "电子设备", "资产类别.电子设备"),

            // dict.accounting.asset.category.office_equip
            ("dict.accounting.asset.category.office_equip", "en-US", "office_equip", "资产类别.办公设备"),
            // dict.accounting.asset.category.office_equip
            ("dict.accounting.asset.category.office_equip", "ja-JP", "office_equip", "资产类别.办公设备"),
            // dict.accounting.asset.category.office_equip
            ("dict.accounting.asset.category.office_equip", "zh-CN", "办公设备", "资产类别.办公设备"),
            // dict.accounting.asset.category.office_equip
            ("dict.accounting.asset.category.office_equip", "zh-HK", "办公设备", "资产类别.办公设备"),

            // dict.accounting.asset.category.furniture
            ("dict.accounting.asset.category.furniture", "en-US", "furniture", "资产类别.家具用具"),
            // dict.accounting.asset.category.furniture
            ("dict.accounting.asset.category.furniture", "ja-JP", "furniture", "资产类别.家具用具"),
            // dict.accounting.asset.category.furniture
            ("dict.accounting.asset.category.furniture", "zh-CN", "家具用具", "资产类别.家具用具"),
            // dict.accounting.asset.category.furniture
            ("dict.accounting.asset.category.furniture", "zh-HK", "家具用具", "资产类别.家具用具"),

            // dict.accounting.asset.category.intangible
            ("dict.accounting.asset.category.intangible", "en-US", "intangible", "资产类别.无形资产"),
            // dict.accounting.asset.category.intangible
            ("dict.accounting.asset.category.intangible", "ja-JP", "intangible", "资产类别.无形资产"),
            // dict.accounting.asset.category.intangible
            ("dict.accounting.asset.category.intangible", "zh-CN", "无形资产", "资产类别.无形资产"),
            // dict.accounting.asset.category.intangible
            ("dict.accounting.asset.category.intangible", "zh-HK", "无形资产", "资产类别.无形资产"),

            // dict.accounting.asset.category.land_use_right
            ("dict.accounting.asset.category.land_use_right", "en-US", "land_use_right", "资产类别.土地使用权"),
            // dict.accounting.asset.category.land_use_right
            ("dict.accounting.asset.category.land_use_right", "ja-JP", "land_use_right", "资产类别.土地使用权"),
            // dict.accounting.asset.category.land_use_right
            ("dict.accounting.asset.category.land_use_right", "zh-CN", "土地使用权", "资产类别.土地使用权"),
            // dict.accounting.asset.category.land_use_right
            ("dict.accounting.asset.category.land_use_right", "zh-HK", "土地使用权", "资产类别.土地使用权"),

            // dict.accounting.asset.category.software
            ("dict.accounting.asset.category.software", "en-US", "software", "资产类别.软件系统"),
            // dict.accounting.asset.category.software
            ("dict.accounting.asset.category.software", "ja-JP", "software", "资产类别.软件系统"),
            // dict.accounting.asset.category.software
            ("dict.accounting.asset.category.software", "zh-CN", "软件系统", "资产类别.软件系统"),
            // dict.accounting.asset.category.software
            ("dict.accounting.asset.category.software", "zh-HK", "软件系统", "资产类别.软件系统"),

            // dict.accounting.asset.category.other
            ("dict.accounting.asset.category.other", "en-US", "other", "资产类别.其他资产"),
            // dict.accounting.asset.category.other
            ("dict.accounting.asset.category.other", "ja-JP", "other", "资产类别.其他资产"),
            // dict.accounting.asset.category.other
            ("dict.accounting.asset.category.other", "zh-CN", "其他资产", "资产类别.其他资产"),
            // dict.accounting.asset.category.other
            ("dict.accounting.asset.category.other", "zh-HK", "其他资产", "资产类别.其他资产"),

            // dict.accounting.cost.center.category.pro
            ("dict.accounting.cost.center.category.pro", "en-US", "pro", "成本中心类别.专业级"),
            // dict.accounting.cost.center.category.pro
            ("dict.accounting.cost.center.category.pro", "ja-JP", "pro", "成本中心类别.专业级"),
            // dict.accounting.cost.center.category.pro
            ("dict.accounting.cost.center.category.pro", "zh-CN", "专业级", "成本中心类别.专业级"),
            // dict.accounting.cost.center.category.pro
            ("dict.accounting.cost.center.category.pro", "zh-HK", "专业级", "成本中心类别.专业级"),

            // dict.accounting.cost.center.category.cons
            ("dict.accounting.cost.center.category.cons", "en-US", "cons", "成本中心类别.消费级"),
            // dict.accounting.cost.center.category.cons
            ("dict.accounting.cost.center.category.cons", "ja-JP", "cons", "成本中心类别.消费级"),
            // dict.accounting.cost.center.category.cons
            ("dict.accounting.cost.center.category.cons", "zh-CN", "消费级", "成本中心类别.消费级"),
            // dict.accounting.cost.center.category.cons
            ("dict.accounting.cost.center.category.cons", "zh-HK", "消费级", "成本中心类别.消费级"),

            // dict.accounting.cost.center.category.medi
            ("dict.accounting.cost.center.category.medi", "en-US", "medi", "成本中心类别.医用级"),
            // dict.accounting.cost.center.category.medi
            ("dict.accounting.cost.center.category.medi", "ja-JP", "medi", "成本中心类别.医用级"),
            // dict.accounting.cost.center.category.medi
            ("dict.accounting.cost.center.category.medi", "zh-CN", "医用级", "成本中心类别.医用级"),
            // dict.accounting.cost.center.category.medi
            ("dict.accounting.cost.center.category.medi", "zh-HK", "医用级", "成本中心类别.医用级"),

            // dict.accounting.cost.center.category.info
            ("dict.accounting.cost.center.category.info", "en-US", "info", "成本中心类别.信息类"),
            // dict.accounting.cost.center.category.info
            ("dict.accounting.cost.center.category.info", "ja-JP", "info", "成本中心类别.信息类"),
            // dict.accounting.cost.center.category.info
            ("dict.accounting.cost.center.category.info", "zh-CN", "信息类", "成本中心类别.信息类"),
            // dict.accounting.cost.center.category.info
            ("dict.accounting.cost.center.category.info", "zh-HK", "信息类", "成本中心类别.信息类"),

            // dict.accounting.cost.center.category.ems
            ("dict.accounting.cost.center.category.ems", "en-US", "ems", "成本中心类别.ems"),
            // dict.accounting.cost.center.category.ems
            ("dict.accounting.cost.center.category.ems", "ja-JP", "ems", "成本中心类别.ems"),
            // dict.accounting.cost.center.category.ems
            ("dict.accounting.cost.center.category.ems", "zh-CN", "ems", "成本中心类别.ems"),
            // dict.accounting.cost.center.category.ems
            ("dict.accounting.cost.center.category.ems", "zh-HK", "ems", "成本中心类别.ems"),

            // dict.accounting.cost.element.category.direct_material
            ("dict.accounting.cost.element.category.direct_material", "en-US", "direct_material", "成本要素类别.直接材料"),
            // dict.accounting.cost.element.category.direct_material
            ("dict.accounting.cost.element.category.direct_material", "ja-JP", "direct_material", "成本要素类别.直接材料"),
            // dict.accounting.cost.element.category.direct_material
            ("dict.accounting.cost.element.category.direct_material", "zh-CN", "直接材料", "成本要素类别.直接材料"),
            // dict.accounting.cost.element.category.direct_material
            ("dict.accounting.cost.element.category.direct_material", "zh-HK", "直接材料", "成本要素类别.直接材料"),

            // dict.accounting.cost.element.category.direct_labor
            ("dict.accounting.cost.element.category.direct_labor", "en-US", "direct_labor", "成本要素类别.直接人工"),
            // dict.accounting.cost.element.category.direct_labor
            ("dict.accounting.cost.element.category.direct_labor", "ja-JP", "direct_labor", "成本要素类别.直接人工"),
            // dict.accounting.cost.element.category.direct_labor
            ("dict.accounting.cost.element.category.direct_labor", "zh-CN", "直接人工", "成本要素类别.直接人工"),
            // dict.accounting.cost.element.category.direct_labor
            ("dict.accounting.cost.element.category.direct_labor", "zh-HK", "直接人工", "成本要素类别.直接人工"),

            // dict.accounting.cost.element.category.manufacturing_overhead
            ("dict.accounting.cost.element.category.manufacturing_overhead", "en-US", "manufacturing_overhead", "成本要素类别.制造费用"),
            // dict.accounting.cost.element.category.manufacturing_overhead
            ("dict.accounting.cost.element.category.manufacturing_overhead", "ja-JP", "manufacturing_overhead", "成本要素类别.制造费用"),
            // dict.accounting.cost.element.category.manufacturing_overhead
            ("dict.accounting.cost.element.category.manufacturing_overhead", "zh-CN", "制造费用", "成本要素类别.制造费用"),
            // dict.accounting.cost.element.category.manufacturing_overhead
            ("dict.accounting.cost.element.category.manufacturing_overhead", "zh-HK", "制造费用", "成本要素类别.制造费用"),

            // dict.accounting.cost.element.category.depreciation
            ("dict.accounting.cost.element.category.depreciation", "en-US", "depreciation", "成本要素类别.折旧费"),
            // dict.accounting.cost.element.category.depreciation
            ("dict.accounting.cost.element.category.depreciation", "ja-JP", "depreciation", "成本要素类别.折旧费"),
            // dict.accounting.cost.element.category.depreciation
            ("dict.accounting.cost.element.category.depreciation", "zh-CN", "折旧费", "成本要素类别.折旧费"),
            // dict.accounting.cost.element.category.depreciation
            ("dict.accounting.cost.element.category.depreciation", "zh-HK", "折旧费", "成本要素类别.折旧费"),

            // dict.accounting.cost.element.category.energy
            ("dict.accounting.cost.element.category.energy", "en-US", "energy", "成本要素类别.能源费"),
            // dict.accounting.cost.element.category.energy
            ("dict.accounting.cost.element.category.energy", "ja-JP", "energy", "成本要素类别.能源费"),
            // dict.accounting.cost.element.category.energy
            ("dict.accounting.cost.element.category.energy", "zh-CN", "能源费", "成本要素类别.能源费"),
            // dict.accounting.cost.element.category.energy
            ("dict.accounting.cost.element.category.energy", "zh-HK", "能源费", "成本要素类别.能源费"),

            // dict.accounting.cost.element.category.maintenance
            ("dict.accounting.cost.element.category.maintenance", "en-US", "maintenance", "成本要素类别.维修费"),
            // dict.accounting.cost.element.category.maintenance
            ("dict.accounting.cost.element.category.maintenance", "ja-JP", "maintenance", "成本要素类别.维修费"),
            // dict.accounting.cost.element.category.maintenance
            ("dict.accounting.cost.element.category.maintenance", "zh-CN", "维修费", "成本要素类别.维修费"),
            // dict.accounting.cost.element.category.maintenance
            ("dict.accounting.cost.element.category.maintenance", "zh-HK", "维修费", "成本要素类别.维修费"),

            // dict.accounting.cost.element.category.indirect_material
            ("dict.accounting.cost.element.category.indirect_material", "en-US", "indirect_material", "成本要素类别.辅助材料"),
            // dict.accounting.cost.element.category.indirect_material
            ("dict.accounting.cost.element.category.indirect_material", "ja-JP", "indirect_material", "成本要素类别.辅助材料"),
            // dict.accounting.cost.element.category.indirect_material
            ("dict.accounting.cost.element.category.indirect_material", "zh-CN", "辅助材料", "成本要素类别.辅助材料"),
            // dict.accounting.cost.element.category.indirect_material
            ("dict.accounting.cost.element.category.indirect_material", "zh-HK", "辅助材料", "成本要素类别.辅助材料"),

            // dict.accounting.cost.element.category.other
            ("dict.accounting.cost.element.category.other", "en-US", "other", "成本要素类别.其他费用"),
            // dict.accounting.cost.element.category.other
            ("dict.accounting.cost.element.category.other", "ja-JP", "other", "成本要素类别.其他费用"),
            // dict.accounting.cost.element.category.other
            ("dict.accounting.cost.element.category.other", "zh-CN", "其他费用", "成本要素类别.其他费用"),
            // dict.accounting.cost.element.category.other
            ("dict.accounting.cost.element.category.other", "zh-HK", "其他费用", "成本要素类别.其他费用"),

            // dict.accounting.currency.cny
            ("dict.accounting.currency.cny", "en-US", "cny", "币种.人民币"),
            // dict.accounting.currency.cny
            ("dict.accounting.currency.cny", "ja-JP", "cny", "币种.人民币"),
            // dict.accounting.currency.cny
            ("dict.accounting.currency.cny", "zh-CN", "人民币", "币种.人民币"),
            // dict.accounting.currency.cny
            ("dict.accounting.currency.cny", "zh-HK", "人民币", "币种.人民币"),

            // dict.accounting.currency.usd
            ("dict.accounting.currency.usd", "en-US", "usd", "币种.美元"),
            // dict.accounting.currency.usd
            ("dict.accounting.currency.usd", "ja-JP", "usd", "币种.美元"),
            // dict.accounting.currency.usd
            ("dict.accounting.currency.usd", "zh-CN", "美元", "币种.美元"),
            // dict.accounting.currency.usd
            ("dict.accounting.currency.usd", "zh-HK", "美元", "币种.美元"),

            // dict.accounting.currency.eur
            ("dict.accounting.currency.eur", "en-US", "eur", "币种.欧元"),
            // dict.accounting.currency.eur
            ("dict.accounting.currency.eur", "ja-JP", "eur", "币种.欧元"),
            // dict.accounting.currency.eur
            ("dict.accounting.currency.eur", "zh-CN", "欧元", "币种.欧元"),
            // dict.accounting.currency.eur
            ("dict.accounting.currency.eur", "zh-HK", "欧元", "币种.欧元"),

            // dict.accounting.currency.jpy
            ("dict.accounting.currency.jpy", "en-US", "jpy", "币种.日元"),
            // dict.accounting.currency.jpy
            ("dict.accounting.currency.jpy", "ja-JP", "jpy", "币种.日元"),
            // dict.accounting.currency.jpy
            ("dict.accounting.currency.jpy", "zh-CN", "日元", "币种.日元"),
            // dict.accounting.currency.jpy
            ("dict.accounting.currency.jpy", "zh-HK", "日元", "币种.日元"),

            // dict.accounting.currency.gbp
            ("dict.accounting.currency.gbp", "en-US", "gbp", "币种.英镑"),
            // dict.accounting.currency.gbp
            ("dict.accounting.currency.gbp", "ja-JP", "gbp", "币种.英镑"),
            // dict.accounting.currency.gbp
            ("dict.accounting.currency.gbp", "zh-CN", "英镑", "币种.英镑"),
            // dict.accounting.currency.gbp
            ("dict.accounting.currency.gbp", "zh-HK", "英镑", "币种.英镑"),

            // dict.accounting.currency.hkd
            ("dict.accounting.currency.hkd", "en-US", "hkd", "币种.港币"),
            // dict.accounting.currency.hkd
            ("dict.accounting.currency.hkd", "ja-JP", "hkd", "币种.港币"),
            // dict.accounting.currency.hkd
            ("dict.accounting.currency.hkd", "zh-CN", "港币", "币种.港币"),
            // dict.accounting.currency.hkd
            ("dict.accounting.currency.hkd", "zh-HK", "港币", "币种.港币"),

            // dict.accounting.currency.krw
            ("dict.accounting.currency.krw", "en-US", "krw", "币种.韩元"),
            // dict.accounting.currency.krw
            ("dict.accounting.currency.krw", "ja-JP", "krw", "币种.韩元"),
            // dict.accounting.currency.krw
            ("dict.accounting.currency.krw", "zh-CN", "韩元", "币种.韩元"),
            // dict.accounting.currency.krw
            ("dict.accounting.currency.krw", "zh-HK", "韩元", "币种.韩元"),

            // dict.accounting.currency.aud
            ("dict.accounting.currency.aud", "en-US", "aud", "币种.澳元"),
            // dict.accounting.currency.aud
            ("dict.accounting.currency.aud", "ja-JP", "aud", "币种.澳元"),
            // dict.accounting.currency.aud
            ("dict.accounting.currency.aud", "zh-CN", "澳元", "币种.澳元"),
            // dict.accounting.currency.aud
            ("dict.accounting.currency.aud", "zh-HK", "澳元", "币种.澳元"),

            // dict.accounting.currency.cad
            ("dict.accounting.currency.cad", "en-US", "cad", "币种.加元"),
            // dict.accounting.currency.cad
            ("dict.accounting.currency.cad", "ja-JP", "cad", "币种.加元"),
            // dict.accounting.currency.cad
            ("dict.accounting.currency.cad", "zh-CN", "加元", "币种.加元"),
            // dict.accounting.currency.cad
            ("dict.accounting.currency.cad", "zh-HK", "加元", "币种.加元"),

            // dict.accounting.currency.chf
            ("dict.accounting.currency.chf", "en-US", "chf", "币种.瑞士法郎"),
            // dict.accounting.currency.chf
            ("dict.accounting.currency.chf", "ja-JP", "chf", "币种.瑞士法郎"),
            // dict.accounting.currency.chf
            ("dict.accounting.currency.chf", "zh-CN", "瑞士法郎", "币种.瑞士法郎"),
            // dict.accounting.currency.chf
            ("dict.accounting.currency.chf", "zh-HK", "瑞士法郎", "币种.瑞士法郎"),

            // dict.accounting.payment.terms.adv100
            ("dict.accounting.payment.terms.adv100", "en-US", "adv100", "付款条件.预付全款"),
            // dict.accounting.payment.terms.adv100
            ("dict.accounting.payment.terms.adv100", "ja-JP", "adv100", "付款条件.预付全款"),
            // dict.accounting.payment.terms.adv100
            ("dict.accounting.payment.terms.adv100", "zh-CN", "预付全款", "付款条件.预付全款"),
            // dict.accounting.payment.terms.adv100
            ("dict.accounting.payment.terms.adv100", "zh-HK", "预付全款", "付款条件.预付全款"),

            // dict.accounting.payment.terms.adv50
            ("dict.accounting.payment.terms.adv50", "en-US", "adv50", "付款条件.预付50%"),
            // dict.accounting.payment.terms.adv50
            ("dict.accounting.payment.terms.adv50", "ja-JP", "adv50", "付款条件.预付50%"),
            // dict.accounting.payment.terms.adv50
            ("dict.accounting.payment.terms.adv50", "zh-CN", "预付50%", "付款条件.预付50%"),
            // dict.accounting.payment.terms.adv50
            ("dict.accounting.payment.terms.adv50", "zh-HK", "预付50%", "付款条件.预付50%"),

            // dict.accounting.payment.terms.cod
            ("dict.accounting.payment.terms.cod", "en-US", "cod", "付款条件.货到付款"),
            // dict.accounting.payment.terms.cod
            ("dict.accounting.payment.terms.cod", "ja-JP", "cod", "付款条件.货到付款"),
            // dict.accounting.payment.terms.cod
            ("dict.accounting.payment.terms.cod", "zh-CN", "货到付款", "付款条件.货到付款"),
            // dict.accounting.payment.terms.cod
            ("dict.accounting.payment.terms.cod", "zh-HK", "货到付款", "付款条件.货到付款"),

            // dict.accounting.payment.terms.net30
            ("dict.accounting.payment.terms.net30", "en-US", "net30", "付款条件.月结30天"),
            // dict.accounting.payment.terms.net30
            ("dict.accounting.payment.terms.net30", "ja-JP", "net30", "付款条件.月结30天"),
            // dict.accounting.payment.terms.net30
            ("dict.accounting.payment.terms.net30", "zh-CN", "月结30天", "付款条件.月结30天"),
            // dict.accounting.payment.terms.net30
            ("dict.accounting.payment.terms.net30", "zh-HK", "月结30天", "付款条件.月结30天"),

            // dict.accounting.payment.terms.net60
            ("dict.accounting.payment.terms.net60", "en-US", "net60", "付款条件.月结60天"),
            // dict.accounting.payment.terms.net60
            ("dict.accounting.payment.terms.net60", "ja-JP", "net60", "付款条件.月结60天"),
            // dict.accounting.payment.terms.net60
            ("dict.accounting.payment.terms.net60", "zh-CN", "月结60天", "付款条件.月结60天"),
            // dict.accounting.payment.terms.net60
            ("dict.accounting.payment.terms.net60", "zh-HK", "月结60天", "付款条件.月结60天"),

            // dict.accounting.payment.terms.net90
            ("dict.accounting.payment.terms.net90", "en-US", "net90", "付款条件.月结90天"),
            // dict.accounting.payment.terms.net90
            ("dict.accounting.payment.terms.net90", "ja-JP", "net90", "付款条件.月结90天"),
            // dict.accounting.payment.terms.net90
            ("dict.accounting.payment.terms.net90", "zh-CN", "月结90天", "付款条件.月结90天"),
            // dict.accounting.payment.terms.net90
            ("dict.accounting.payment.terms.net90", "zh-HK", "月结90天", "付款条件.月结90天"),

            // dict.accounting.payment.terms.sight
            ("dict.accounting.payment.terms.sight", "en-US", "sight", "付款条件.见票即付"),
            // dict.accounting.payment.terms.sight
            ("dict.accounting.payment.terms.sight", "ja-JP", "sight", "付款条件.见票即付"),
            // dict.accounting.payment.terms.sight
            ("dict.accounting.payment.terms.sight", "zh-CN", "见票即付", "付款条件.见票即付"),
            // dict.accounting.payment.terms.sight
            ("dict.accounting.payment.terms.sight", "zh-HK", "见票即付", "付款条件.见票即付"),

            // dict.accounting.payment.terms.tt
            ("dict.accounting.payment.terms.tt", "en-US", "tt", "付款条件.电汇"),
            // dict.accounting.payment.terms.tt
            ("dict.accounting.payment.terms.tt", "ja-JP", "tt", "付款条件.电汇"),
            // dict.accounting.payment.terms.tt
            ("dict.accounting.payment.terms.tt", "zh-CN", "电汇", "付款条件.电汇"),
            // dict.accounting.payment.terms.tt
            ("dict.accounting.payment.terms.tt", "zh-HK", "电汇", "付款条件.电汇"),

            // dict.accounting.payment.terms.lc
            ("dict.accounting.payment.terms.lc", "en-US", "lc", "付款条件.信用证"),
            // dict.accounting.payment.terms.lc
            ("dict.accounting.payment.terms.lc", "ja-JP", "lc", "付款条件.信用证"),
            // dict.accounting.payment.terms.lc
            ("dict.accounting.payment.terms.lc", "zh-CN", "信用证", "付款条件.信用证"),
            // dict.accounting.payment.terms.lc
            ("dict.accounting.payment.terms.lc", "zh-HK", "信用证", "付款条件.信用证"),

            // dict.accounting.profit.center.category.pro
            ("dict.accounting.profit.center.category.pro", "en-US", "pro", "利润中心类别.专业级"),
            // dict.accounting.profit.center.category.pro
            ("dict.accounting.profit.center.category.pro", "ja-JP", "pro", "利润中心类别.专业级"),
            // dict.accounting.profit.center.category.pro
            ("dict.accounting.profit.center.category.pro", "zh-CN", "专业级", "利润中心类别.专业级"),
            // dict.accounting.profit.center.category.pro
            ("dict.accounting.profit.center.category.pro", "zh-HK", "专业级", "利润中心类别.专业级"),

            // dict.accounting.profit.center.category.cons
            ("dict.accounting.profit.center.category.cons", "en-US", "cons", "利润中心类别.消费级"),
            // dict.accounting.profit.center.category.cons
            ("dict.accounting.profit.center.category.cons", "ja-JP", "cons", "利润中心类别.消费级"),
            // dict.accounting.profit.center.category.cons
            ("dict.accounting.profit.center.category.cons", "zh-CN", "消费级", "利润中心类别.消费级"),
            // dict.accounting.profit.center.category.cons
            ("dict.accounting.profit.center.category.cons", "zh-HK", "消费级", "利润中心类别.消费级"),

            // dict.accounting.profit.center.category.medi
            ("dict.accounting.profit.center.category.medi", "en-US", "medi", "利润中心类别.医用级"),
            // dict.accounting.profit.center.category.medi
            ("dict.accounting.profit.center.category.medi", "ja-JP", "medi", "利润中心类别.医用级"),
            // dict.accounting.profit.center.category.medi
            ("dict.accounting.profit.center.category.medi", "zh-CN", "医用级", "利润中心类别.医用级"),
            // dict.accounting.profit.center.category.medi
            ("dict.accounting.profit.center.category.medi", "zh-HK", "医用级", "利润中心类别.医用级"),

            // dict.accounting.profit.center.category.info
            ("dict.accounting.profit.center.category.info", "en-US", "info", "利润中心类别.信息类"),
            // dict.accounting.profit.center.category.info
            ("dict.accounting.profit.center.category.info", "ja-JP", "info", "利润中心类别.信息类"),
            // dict.accounting.profit.center.category.info
            ("dict.accounting.profit.center.category.info", "zh-CN", "信息类", "利润中心类别.信息类"),
            // dict.accounting.profit.center.category.info
            ("dict.accounting.profit.center.category.info", "zh-HK", "信息类", "利润中心类别.信息类"),

            // dict.accounting.profit.center.category.ems
            ("dict.accounting.profit.center.category.ems", "en-US", "ems", "利润中心类别.ems"),
            // dict.accounting.profit.center.category.ems
            ("dict.accounting.profit.center.category.ems", "ja-JP", "ems", "利润中心类别.ems"),
            // dict.accounting.profit.center.category.ems
            ("dict.accounting.profit.center.category.ems", "zh-CN", "ems", "利润中心类别.ems"),
            // dict.accounting.profit.center.category.ems
            ("dict.accounting.profit.center.category.ems", "zh-HK", "ems", "利润中心类别.ems"),

            // dict.accounting.tax.code.vat13
            ("dict.accounting.tax.code.vat13", "en-US", "vat13", "税码.增值税13%"),
            // dict.accounting.tax.code.vat13
            ("dict.accounting.tax.code.vat13", "ja-JP", "vat13", "税码.增值税13%"),
            // dict.accounting.tax.code.vat13
            ("dict.accounting.tax.code.vat13", "zh-CN", "增值税13%", "税码.增值税13%"),
            // dict.accounting.tax.code.vat13
            ("dict.accounting.tax.code.vat13", "zh-HK", "增值税13%", "税码.增值税13%"),

            // dict.accounting.tax.code.vat9
            ("dict.accounting.tax.code.vat9", "en-US", "vat9", "税码.增值税9%"),
            // dict.accounting.tax.code.vat9
            ("dict.accounting.tax.code.vat9", "ja-JP", "vat9", "税码.增值税9%"),
            // dict.accounting.tax.code.vat9
            ("dict.accounting.tax.code.vat9", "zh-CN", "增值税9%", "税码.增值税9%"),
            // dict.accounting.tax.code.vat9
            ("dict.accounting.tax.code.vat9", "zh-HK", "增值税9%", "税码.增值税9%"),

            // dict.accounting.tax.code.vat6
            ("dict.accounting.tax.code.vat6", "en-US", "vat6", "税码.增值税6%"),
            // dict.accounting.tax.code.vat6
            ("dict.accounting.tax.code.vat6", "ja-JP", "vat6", "税码.增值税6%"),
            // dict.accounting.tax.code.vat6
            ("dict.accounting.tax.code.vat6", "zh-CN", "增值税6%", "税码.增值税6%"),
            // dict.accounting.tax.code.vat6
            ("dict.accounting.tax.code.vat6", "zh-HK", "增值税6%", "税码.增值税6%"),

            // dict.accounting.tax.code.vat3
            ("dict.accounting.tax.code.vat3", "en-US", "vat3", "税码.增值税3%"),
            // dict.accounting.tax.code.vat3
            ("dict.accounting.tax.code.vat3", "ja-JP", "vat3", "税码.增值税3%"),
            // dict.accounting.tax.code.vat3
            ("dict.accounting.tax.code.vat3", "zh-CN", "增值税3%", "税码.增值税3%"),
            // dict.accounting.tax.code.vat3
            ("dict.accounting.tax.code.vat3", "zh-HK", "增值税3%", "税码.增值税3%"),

            // dict.accounting.tax.code.vat0
            ("dict.accounting.tax.code.vat0", "en-US", "vat0", "税码.增值税0%"),
            // dict.accounting.tax.code.vat0
            ("dict.accounting.tax.code.vat0", "ja-JP", "vat0", "税码.增值税0%"),
            // dict.accounting.tax.code.vat0
            ("dict.accounting.tax.code.vat0", "zh-CN", "增值税0%", "税码.增值税0%"),
            // dict.accounting.tax.code.vat0
            ("dict.accounting.tax.code.vat0", "zh-HK", "增值税0%", "税码.增值税0%"),

            // dict.accounting.tax.code.taxfree
            ("dict.accounting.tax.code.taxfree", "en-US", "taxfree", "税码.免税"),
            // dict.accounting.tax.code.taxfree
            ("dict.accounting.tax.code.taxfree", "ja-JP", "taxfree", "税码.免税"),
            // dict.accounting.tax.code.taxfree
            ("dict.accounting.tax.code.taxfree", "zh-CN", "免税", "税码.免税"),
            // dict.accounting.tax.code.taxfree
            ("dict.accounting.tax.code.taxfree", "zh-HK", "免税", "税码.免税"),

            // dict.accounting.tax.code.input
            ("dict.accounting.tax.code.input", "en-US", "input", "税码.进项税"),
            // dict.accounting.tax.code.input
            ("dict.accounting.tax.code.input", "ja-JP", "input", "税码.进项税"),
            // dict.accounting.tax.code.input
            ("dict.accounting.tax.code.input", "zh-CN", "进项税", "税码.进项税"),
            // dict.accounting.tax.code.input
            ("dict.accounting.tax.code.input", "zh-HK", "进项税", "税码.进项税"),

            // dict.accounting.tax.code.output
            ("dict.accounting.tax.code.output", "en-US", "output", "税码.销项税"),
            // dict.accounting.tax.code.output
            ("dict.accounting.tax.code.output", "ja-JP", "output", "税码.销项税"),
            // dict.accounting.tax.code.output
            ("dict.accounting.tax.code.output", "zh-CN", "销项税", "税码.销项税"),
            // dict.accounting.tax.code.output
            ("dict.accounting.tax.code.output", "zh-HK", "销项税", "税码.销项税"),

            // dict.accounting.tax.rate.13
            ("dict.accounting.tax.rate.13", "en-US", "13%", "税率.13%"),
            // dict.accounting.tax.rate.13
            ("dict.accounting.tax.rate.13", "ja-JP", "13%", "税率.13%"),
            // dict.accounting.tax.rate.13
            ("dict.accounting.tax.rate.13", "zh-CN", "13%", "税率.13%"),
            // dict.accounting.tax.rate.13
            ("dict.accounting.tax.rate.13", "zh-HK", "13%", "税率.13%"),

            // dict.accounting.tax.rate.9
            ("dict.accounting.tax.rate.9", "en-US", "9%", "税率.9%"),
            // dict.accounting.tax.rate.9
            ("dict.accounting.tax.rate.9", "ja-JP", "9%", "税率.9%"),
            // dict.accounting.tax.rate.9
            ("dict.accounting.tax.rate.9", "zh-CN", "9%", "税率.9%"),
            // dict.accounting.tax.rate.9
            ("dict.accounting.tax.rate.9", "zh-HK", "9%", "税率.9%"),

            // dict.accounting.tax.rate.6
            ("dict.accounting.tax.rate.6", "en-US", "6%", "税率.6%"),
            // dict.accounting.tax.rate.6
            ("dict.accounting.tax.rate.6", "ja-JP", "6%", "税率.6%"),
            // dict.accounting.tax.rate.6
            ("dict.accounting.tax.rate.6", "zh-CN", "6%", "税率.6%"),
            // dict.accounting.tax.rate.6
            ("dict.accounting.tax.rate.6", "zh-HK", "6%", "税率.6%"),

            // dict.accounting.tax.rate.5
            ("dict.accounting.tax.rate.5", "en-US", "5%", "税率.5%"),
            // dict.accounting.tax.rate.5
            ("dict.accounting.tax.rate.5", "ja-JP", "5%", "税率.5%"),
            // dict.accounting.tax.rate.5
            ("dict.accounting.tax.rate.5", "zh-CN", "5%", "税率.5%"),
            // dict.accounting.tax.rate.5
            ("dict.accounting.tax.rate.5", "zh-HK", "5%", "税率.5%"),

            // dict.accounting.tax.rate.3
            ("dict.accounting.tax.rate.3", "en-US", "3%", "税率.3%"),
            // dict.accounting.tax.rate.3
            ("dict.accounting.tax.rate.3", "ja-JP", "3%", "税率.3%"),
            // dict.accounting.tax.rate.3
            ("dict.accounting.tax.rate.3", "zh-CN", "3%", "税率.3%"),
            // dict.accounting.tax.rate.3
            ("dict.accounting.tax.rate.3", "zh-HK", "3%", "税率.3%"),

            // dict.accounting.tax.rate.2
            ("dict.accounting.tax.rate.2", "en-US", "2%", "税率.2%"),
            // dict.accounting.tax.rate.2
            ("dict.accounting.tax.rate.2", "ja-JP", "2%", "税率.2%"),
            // dict.accounting.tax.rate.2
            ("dict.accounting.tax.rate.2", "zh-CN", "2%", "税率.2%"),
            // dict.accounting.tax.rate.2
            ("dict.accounting.tax.rate.2", "zh-HK", "2%", "税率.2%"),

            // dict.accounting.tax.rate.1
            ("dict.accounting.tax.rate.1", "en-US", "1%", "税率.1%"),
            // dict.accounting.tax.rate.1
            ("dict.accounting.tax.rate.1", "ja-JP", "1%", "税率.1%"),
            // dict.accounting.tax.rate.1
            ("dict.accounting.tax.rate.1", "zh-CN", "1%", "税率.1%"),
            // dict.accounting.tax.rate.1
            ("dict.accounting.tax.rate.1", "zh-HK", "1%", "税率.1%"),

            // dict.accounting.tax.rate.0
            ("dict.accounting.tax.rate.0", "en-US", "0%", "税率.0%"),
            // dict.accounting.tax.rate.0
            ("dict.accounting.tax.rate.0", "ja-JP", "0%", "税率.0%"),
            // dict.accounting.tax.rate.0
            ("dict.accounting.tax.rate.0", "zh-CN", "0%", "税率.0%"),
            // dict.accounting.tax.rate.0
            ("dict.accounting.tax.rate.0", "zh-HK", "0%", "税率.0%"),

            // dict.gen.button.category.query
            ("dict.gen.button.category.query", "en-US", "query", "代码生成操作后缀.查询"),
            // dict.gen.button.category.query
            ("dict.gen.button.category.query", "ja-JP", "query", "代码生成操作后缀.查询"),
            // dict.gen.button.category.query
            ("dict.gen.button.category.query", "zh-CN", "查询", "代码生成操作后缀.查询"),
            // dict.gen.button.category.query
            ("dict.gen.button.category.query", "zh-HK", "查询", "代码生成操作后缀.查询"),

            // dict.gen.button.category.create
            ("dict.gen.button.category.create", "en-US", "create", "代码生成操作后缀.新增"),
            // dict.gen.button.category.create
            ("dict.gen.button.category.create", "ja-JP", "create", "代码生成操作后缀.新增"),
            // dict.gen.button.category.create
            ("dict.gen.button.category.create", "zh-CN", "新增", "代码生成操作后缀.新增"),
            // dict.gen.button.category.create
            ("dict.gen.button.category.create", "zh-HK", "新增", "代码生成操作后缀.新增"),

            // dict.gen.button.category.update
            ("dict.gen.button.category.update", "en-US", "update", "代码生成操作后缀.修改"),
            // dict.gen.button.category.update
            ("dict.gen.button.category.update", "ja-JP", "update", "代码生成操作后缀.修改"),
            // dict.gen.button.category.update
            ("dict.gen.button.category.update", "zh-CN", "修改", "代码生成操作后缀.修改"),
            // dict.gen.button.category.update
            ("dict.gen.button.category.update", "zh-HK", "修改", "代码生成操作后缀.修改"),

            // dict.gen.button.category.delete
            ("dict.gen.button.category.delete", "en-US", "delete", "代码生成操作后缀.删除"),
            // dict.gen.button.category.delete
            ("dict.gen.button.category.delete", "ja-JP", "delete", "代码生成操作后缀.删除"),
            // dict.gen.button.category.delete
            ("dict.gen.button.category.delete", "zh-CN", "删除", "代码生成操作后缀.删除"),
            // dict.gen.button.category.delete
            ("dict.gen.button.category.delete", "zh-HK", "删除", "代码生成操作后缀.删除"),

            // dict.gen.button.category.detail
            ("dict.gen.button.category.detail", "en-US", "detail", "代码生成操作后缀.详情"),
            // dict.gen.button.category.detail
            ("dict.gen.button.category.detail", "ja-JP", "detail", "代码生成操作后缀.详情"),
            // dict.gen.button.category.detail
            ("dict.gen.button.category.detail", "zh-CN", "详情", "代码生成操作后缀.详情"),
            // dict.gen.button.category.detail
            ("dict.gen.button.category.detail", "zh-HK", "详情", "代码生成操作后缀.详情"),

            // dict.gen.button.category.preview
            ("dict.gen.button.category.preview", "en-US", "preview", "代码生成操作后缀.预览"),
            // dict.gen.button.category.preview
            ("dict.gen.button.category.preview", "ja-JP", "preview", "代码生成操作后缀.预览"),
            // dict.gen.button.category.preview
            ("dict.gen.button.category.preview", "zh-CN", "预览", "代码生成操作后缀.预览"),
            // dict.gen.button.category.preview
            ("dict.gen.button.category.preview", "zh-HK", "预览", "代码生成操作后缀.预览"),

            // dict.gen.button.category.print
            ("dict.gen.button.category.print", "en-US", "print", "代码生成操作后缀.打印"),
            // dict.gen.button.category.print
            ("dict.gen.button.category.print", "ja-JP", "print", "代码生成操作后缀.打印"),
            // dict.gen.button.category.print
            ("dict.gen.button.category.print", "zh-CN", "打印", "代码生成操作后缀.打印"),
            // dict.gen.button.category.print
            ("dict.gen.button.category.print", "zh-HK", "打印", "代码生成操作后缀.打印"),

            // dict.gen.button.category.import
            ("dict.gen.button.category.import", "en-US", "import", "代码生成操作后缀.导入"),
            // dict.gen.button.category.import
            ("dict.gen.button.category.import", "ja-JP", "import", "代码生成操作后缀.导入"),
            // dict.gen.button.category.import
            ("dict.gen.button.category.import", "zh-CN", "导入", "代码生成操作后缀.导入"),
            // dict.gen.button.category.import
            ("dict.gen.button.category.import", "zh-HK", "导入", "代码生成操作后缀.导入"),

            // dict.gen.button.category.export
            ("dict.gen.button.category.export", "en-US", "export", "代码生成操作后缀.导出"),
            // dict.gen.button.category.export
            ("dict.gen.button.category.export", "ja-JP", "export", "代码生成操作后缀.导出"),
            // dict.gen.button.category.export
            ("dict.gen.button.category.export", "zh-CN", "导出", "代码生成操作后缀.导出"),
            // dict.gen.button.category.export
            ("dict.gen.button.category.export", "zh-HK", "导出", "代码生成操作后缀.导出"),

            // dict.gen.button.category.template
            ("dict.gen.button.category.template", "en-US", "template", "代码生成操作后缀.模板"),
            // dict.gen.button.category.template
            ("dict.gen.button.category.template", "ja-JP", "template", "代码生成操作后缀.模板"),
            // dict.gen.button.category.template
            ("dict.gen.button.category.template", "zh-CN", "模板", "代码生成操作后缀.模板"),
            // dict.gen.button.category.template
            ("dict.gen.button.category.template", "zh-HK", "模板", "代码生成操作后缀.模板"),

            // dict.gen.button.category.approve
            ("dict.gen.button.category.approve", "en-US", "approve", "代码生成操作后缀.审批"),
            // dict.gen.button.category.approve
            ("dict.gen.button.category.approve", "ja-JP", "approve", "代码生成操作后缀.审批"),
            // dict.gen.button.category.approve
            ("dict.gen.button.category.approve", "zh-CN", "审批", "代码生成操作后缀.审批"),
            // dict.gen.button.category.approve
            ("dict.gen.button.category.approve", "zh-HK", "审批", "代码生成操作后缀.审批"),

            // dict.gen.button.category.revoke
            ("dict.gen.button.category.revoke", "en-US", "revoke", "代码生成操作后缀.撤销"),
            // dict.gen.button.category.revoke
            ("dict.gen.button.category.revoke", "ja-JP", "revoke", "代码生成操作后缀.撤销"),
            // dict.gen.button.category.revoke
            ("dict.gen.button.category.revoke", "zh-CN", "撤销", "代码生成操作后缀.撤销"),
            // dict.gen.button.category.revoke
            ("dict.gen.button.category.revoke", "zh-HK", "撤销", "代码生成操作后缀.撤销"),

            // dict.gen.button.category.authorize
            ("dict.gen.button.category.authorize", "en-US", "authorize", "代码生成操作后缀.授权"),
            // dict.gen.button.category.authorize
            ("dict.gen.button.category.authorize", "ja-JP", "authorize", "代码生成操作后缀.授权"),
            // dict.gen.button.category.authorize
            ("dict.gen.button.category.authorize", "zh-CN", "授权", "代码生成操作后缀.授权"),
            // dict.gen.button.category.authorize
            ("dict.gen.button.category.authorize", "zh-HK", "授权", "代码生成操作后缀.授权"),

            // dict.gen.button.category.allocate
            ("dict.gen.button.category.allocate", "en-US", "allocate", "代码生成操作后缀.分配"),
            // dict.gen.button.category.allocate
            ("dict.gen.button.category.allocate", "ja-JP", "allocate", "代码生成操作后缀.分配"),
            // dict.gen.button.category.allocate
            ("dict.gen.button.category.allocate", "zh-CN", "分配", "代码生成操作后缀.分配"),
            // dict.gen.button.category.allocate
            ("dict.gen.button.category.allocate", "zh-HK", "分配", "代码生成操作后缀.分配"),

            // dict.gen.button.category.resetpwd
            ("dict.gen.button.category.resetpwd", "en-US", "resetpwd", "代码生成操作后缀.重置密码"),
            // dict.gen.button.category.resetpwd
            ("dict.gen.button.category.resetpwd", "ja-JP", "resetpwd", "代码生成操作后缀.重置密码"),
            // dict.gen.button.category.resetpwd
            ("dict.gen.button.category.resetpwd", "zh-CN", "重置密码", "代码生成操作后缀.重置密码"),
            // dict.gen.button.category.resetpwd
            ("dict.gen.button.category.resetpwd", "zh-HK", "重置密码", "代码生成操作后缀.重置密码"),

            // dict.gen.button.category.changepwd
            ("dict.gen.button.category.changepwd", "en-US", "changepwd", "代码生成操作后缀.变更密码"),
            // dict.gen.button.category.changepwd
            ("dict.gen.button.category.changepwd", "ja-JP", "changepwd", "代码生成操作后缀.变更密码"),
            // dict.gen.button.category.changepwd
            ("dict.gen.button.category.changepwd", "zh-CN", "变更密码", "代码生成操作后缀.变更密码"),
            // dict.gen.button.category.changepwd
            ("dict.gen.button.category.changepwd", "zh-HK", "变更密码", "代码生成操作后缀.变更密码"),

            // dict.gen.button.category.empty
            ("dict.gen.button.category.empty", "en-US", "empty", "代码生成操作后缀.清空"),
            // dict.gen.button.category.empty
            ("dict.gen.button.category.empty", "ja-JP", "empty", "代码生成操作后缀.清空"),
            // dict.gen.button.category.empty
            ("dict.gen.button.category.empty", "zh-CN", "清空", "代码生成操作后缀.清空"),
            // dict.gen.button.category.empty
            ("dict.gen.button.category.empty", "zh-HK", "清空", "代码生成操作后缀.清空"),

            // dict.gen.button.category.truncate
            ("dict.gen.button.category.truncate", "en-US", "truncate", "代码生成操作后缀.截断"),
            // dict.gen.button.category.truncate
            ("dict.gen.button.category.truncate", "ja-JP", "truncate", "代码生成操作后缀.截断"),
            // dict.gen.button.category.truncate
            ("dict.gen.button.category.truncate", "zh-CN", "截断", "代码生成操作后缀.截断"),
            // dict.gen.button.category.truncate
            ("dict.gen.button.category.truncate", "zh-HK", "截断", "代码生成操作后缀.截断"),

            // dict.gen.button.category.unlock
            ("dict.gen.button.category.unlock", "en-US", "unlock", "代码生成操作后缀.解锁"),
            // dict.gen.button.category.unlock
            ("dict.gen.button.category.unlock", "ja-JP", "unlock", "代码生成操作后缀.解锁"),
            // dict.gen.button.category.unlock
            ("dict.gen.button.category.unlock", "zh-CN", "解锁", "代码生成操作后缀.解锁"),
            // dict.gen.button.category.unlock
            ("dict.gen.button.category.unlock", "zh-HK", "解锁", "代码生成操作后缀.解锁"),

            // dict.gen.button.category.disable
            ("dict.gen.button.category.disable", "en-US", "disable", "代码生成操作后缀.禁用"),
            // dict.gen.button.category.disable
            ("dict.gen.button.category.disable", "ja-JP", "disable", "代码生成操作后缀.禁用"),
            // dict.gen.button.category.disable
            ("dict.gen.button.category.disable", "zh-CN", "禁用", "代码生成操作后缀.禁用"),
            // dict.gen.button.category.disable
            ("dict.gen.button.category.disable", "zh-HK", "禁用", "代码生成操作后缀.禁用"),

            // dict.gen.button.category.generate
            ("dict.gen.button.category.generate", "en-US", "generate", "代码生成操作后缀.生成"),
            // dict.gen.button.category.generate
            ("dict.gen.button.category.generate", "ja-JP", "generate", "代码生成操作后缀.生成"),
            // dict.gen.button.category.generate
            ("dict.gen.button.category.generate", "zh-CN", "生成", "代码生成操作后缀.生成"),
            // dict.gen.button.category.generate
            ("dict.gen.button.category.generate", "zh-HK", "生成", "代码生成操作后缀.生成"),

            // dict.gen.button.category.download
            ("dict.gen.button.category.download", "en-US", "download", "代码生成操作后缀.下载"),
            // dict.gen.button.category.download
            ("dict.gen.button.category.download", "ja-JP", "download", "代码生成操作后缀.下载"),
            // dict.gen.button.category.download
            ("dict.gen.button.category.download", "zh-CN", "下载", "代码生成操作后缀.下载"),
            // dict.gen.button.category.download
            ("dict.gen.button.category.download", "zh-HK", "下载", "代码生成操作后缀.下载"),

            // dict.gen.button.category.sync
            ("dict.gen.button.category.sync", "en-US", "sync", "代码生成操作后缀.同步"),
            // dict.gen.button.category.sync
            ("dict.gen.button.category.sync", "ja-JP", "sync", "代码生成操作后缀.同步"),
            // dict.gen.button.category.sync
            ("dict.gen.button.category.sync", "zh-CN", "同步", "代码生成操作后缀.同步"),
            // dict.gen.button.category.sync
            ("dict.gen.button.category.sync", "zh-HK", "同步", "代码生成操作后缀.同步"),

            // dict.gen.button.category.columns
            ("dict.gen.button.category.columns", "en-US", "columns", "代码生成操作后缀.字段"),
            // dict.gen.button.category.columns
            ("dict.gen.button.category.columns", "ja-JP", "columns", "代码生成操作后缀.字段"),
            // dict.gen.button.category.columns
            ("dict.gen.button.category.columns", "zh-CN", "字段", "代码生成操作后缀.字段"),
            // dict.gen.button.category.columns
            ("dict.gen.button.category.columns", "zh-HK", "字段", "代码生成操作后缀.字段"),

            // dict.gen.button.category.tables
            ("dict.gen.button.category.tables", "en-US", "tables", "代码生成操作后缀.表"),
            // dict.gen.button.category.tables
            ("dict.gen.button.category.tables", "ja-JP", "tables", "代码生成操作后缀.表"),
            // dict.gen.button.category.tables
            ("dict.gen.button.category.tables", "zh-CN", "表", "代码生成操作后缀.表"),
            // dict.gen.button.category.tables
            ("dict.gen.button.category.tables", "zh-HK", "表", "代码生成操作后缀.表"),

            // dict.gen.button.category.databases
            ("dict.gen.button.category.databases", "en-US", "databases", "代码生成操作后缀.数据库"),
            // dict.gen.button.category.databases
            ("dict.gen.button.category.databases", "ja-JP", "databases", "代码生成操作后缀.数据库"),
            // dict.gen.button.category.databases
            ("dict.gen.button.category.databases", "zh-CN", "数据库", "代码生成操作后缀.数据库"),
            // dict.gen.button.category.databases
            ("dict.gen.button.category.databases", "zh-HK", "数据库", "代码生成操作后缀.数据库"),

            // dict.gen.button.category.initialize
            ("dict.gen.button.category.initialize", "en-US", "initialize", "代码生成操作后缀.初始化"),
            // dict.gen.button.category.initialize
            ("dict.gen.button.category.initialize", "ja-JP", "initialize", "代码生成操作后缀.初始化"),
            // dict.gen.button.category.initialize
            ("dict.gen.button.category.initialize", "zh-CN", "初始化", "代码生成操作后缀.初始化"),
            // dict.gen.button.category.initialize
            ("dict.gen.button.category.initialize", "zh-HK", "初始化", "代码生成操作后缀.初始化"),

            // dict.gen.button.category.clone
            ("dict.gen.button.category.clone", "en-US", "clone", "代码生成操作后缀.克隆"),
            // dict.gen.button.category.clone
            ("dict.gen.button.category.clone", "ja-JP", "clone", "代码生成操作后缀.克隆"),
            // dict.gen.button.category.clone
            ("dict.gen.button.category.clone", "zh-CN", "克隆", "代码生成操作后缀.克隆"),
            // dict.gen.button.category.clone
            ("dict.gen.button.category.clone", "zh-HK", "克隆", "代码生成操作后缀.克隆"),

            // dict.gen.button.category.copy
            ("dict.gen.button.category.copy", "en-US", "copy", "代码生成操作后缀.复制"),
            // dict.gen.button.category.copy
            ("dict.gen.button.category.copy", "ja-JP", "copy", "代码生成操作后缀.复制"),
            // dict.gen.button.category.copy
            ("dict.gen.button.category.copy", "zh-CN", "复制", "代码生成操作后缀.复制"),
            // dict.gen.button.category.copy
            ("dict.gen.button.category.copy", "zh-HK", "复制", "代码生成操作后缀.复制"),

            // dict.gen.button.category.suspend
            ("dict.gen.button.category.suspend", "en-US", "suspend", "代码生成操作后缀.暂停"),
            // dict.gen.button.category.suspend
            ("dict.gen.button.category.suspend", "ja-JP", "suspend", "代码生成操作后缀.暂停"),
            // dict.gen.button.category.suspend
            ("dict.gen.button.category.suspend", "zh-CN", "暂停", "代码生成操作后缀.暂停"),
            // dict.gen.button.category.suspend
            ("dict.gen.button.category.suspend", "zh-HK", "暂停", "代码生成操作后缀.暂停"),

            // dict.gen.button.category.resume
            ("dict.gen.button.category.resume", "en-US", "resume", "代码生成操作后缀.恢复"),
            // dict.gen.button.category.resume
            ("dict.gen.button.category.resume", "ja-JP", "resume", "代码生成操作后缀.恢复"),
            // dict.gen.button.category.resume
            ("dict.gen.button.category.resume", "zh-CN", "恢复", "代码生成操作后缀.恢复"),
            // dict.gen.button.category.resume
            ("dict.gen.button.category.resume", "zh-HK", "恢复", "代码生成操作后缀.恢复"),

            // dict.gen.button.category.submit
            ("dict.gen.button.category.submit", "en-US", "submit", "代码生成操作后缀.提交"),
            // dict.gen.button.category.submit
            ("dict.gen.button.category.submit", "ja-JP", "submit", "代码生成操作后缀.提交"),
            // dict.gen.button.category.submit
            ("dict.gen.button.category.submit", "zh-CN", "提交", "代码生成操作后缀.提交"),
            // dict.gen.button.category.submit
            ("dict.gen.button.category.submit", "zh-HK", "提交", "代码生成操作后缀.提交"),

            // dict.gen.button.category.withdraw
            ("dict.gen.button.category.withdraw", "en-US", "withdraw", "代码生成操作后缀.撤回"),
            // dict.gen.button.category.withdraw
            ("dict.gen.button.category.withdraw", "ja-JP", "withdraw", "代码生成操作后缀.撤回"),
            // dict.gen.button.category.withdraw
            ("dict.gen.button.category.withdraw", "zh-CN", "撤回", "代码生成操作后缀.撤回"),
            // dict.gen.button.category.withdraw
            ("dict.gen.button.category.withdraw", "zh-HK", "撤回", "代码生成操作后缀.撤回"),

            // dict.gen.button.category.transfer
            ("dict.gen.button.category.transfer", "en-US", "transfer", "代码生成操作后缀.转办"),
            // dict.gen.button.category.transfer
            ("dict.gen.button.category.transfer", "ja-JP", "transfer", "代码生成操作后缀.转办"),
            // dict.gen.button.category.transfer
            ("dict.gen.button.category.transfer", "zh-CN", "转办", "代码生成操作后缀.转办"),
            // dict.gen.button.category.transfer
            ("dict.gen.button.category.transfer", "zh-HK", "转办", "代码生成操作后缀.转办"),

            // dict.gen.button.category.delegate
            ("dict.gen.button.category.delegate", "en-US", "delegate", "代码生成操作后缀.委托"),
            // dict.gen.button.category.delegate
            ("dict.gen.button.category.delegate", "ja-JP", "delegate", "代码生成操作后缀.委托"),
            // dict.gen.button.category.delegate
            ("dict.gen.button.category.delegate", "zh-CN", "委托", "代码生成操作后缀.委托"),
            // dict.gen.button.category.delegate
            ("dict.gen.button.category.delegate", "zh-HK", "委托", "代码生成操作后缀.委托"),

            // dict.gen.button.category.return
            ("dict.gen.button.category.return", "en-US", "return", "代码生成操作后缀.退回"),
            // dict.gen.button.category.return
            ("dict.gen.button.category.return", "ja-JP", "return", "代码生成操作后缀.退回"),
            // dict.gen.button.category.return
            ("dict.gen.button.category.return", "zh-CN", "退回", "代码生成操作后缀.退回"),
            // dict.gen.button.category.return
            ("dict.gen.button.category.return", "zh-HK", "退回", "代码生成操作后缀.退回"),

            // dict.gen.button.category.urge
            ("dict.gen.button.category.urge", "en-US", "urge", "代码生成操作后缀.催办"),
            // dict.gen.button.category.urge
            ("dict.gen.button.category.urge", "ja-JP", "urge", "代码生成操作后缀.催办"),
            // dict.gen.button.category.urge
            ("dict.gen.button.category.urge", "zh-CN", "催办", "代码生成操作后缀.催办"),
            // dict.gen.button.category.urge
            ("dict.gen.button.category.urge", "zh-HK", "催办", "代码生成操作后缀.催办"),

            // dict.gen.button.category.addsign
            ("dict.gen.button.category.addsign", "en-US", "addsign", "代码生成操作后缀.加签"),
            // dict.gen.button.category.addsign
            ("dict.gen.button.category.addsign", "ja-JP", "addsign", "代码生成操作后缀.加签"),
            // dict.gen.button.category.addsign
            ("dict.gen.button.category.addsign", "zh-CN", "加签", "代码生成操作后缀.加签"),
            // dict.gen.button.category.addsign
            ("dict.gen.button.category.addsign", "zh-HK", "加签", "代码生成操作后缀.加签"),

            // dict.gen.button.category.reducesign
            ("dict.gen.button.category.reducesign", "en-US", "reducesign", "代码生成操作后缀.减签"),
            // dict.gen.button.category.reducesign
            ("dict.gen.button.category.reducesign", "ja-JP", "reducesign", "代码生成操作后缀.减签"),
            // dict.gen.button.category.reducesign
            ("dict.gen.button.category.reducesign", "zh-CN", "减签", "代码生成操作后缀.减签"),
            // dict.gen.button.category.reducesign
            ("dict.gen.button.category.reducesign", "zh-HK", "减签", "代码生成操作后缀.减签"),

            // dict.gen.button.category.progress
            ("dict.gen.button.category.progress", "en-US", "progress", "代码生成操作后缀.进度"),
            // dict.gen.button.category.progress
            ("dict.gen.button.category.progress", "ja-JP", "progress", "代码生成操作后缀.进度"),
            // dict.gen.button.category.progress
            ("dict.gen.button.category.progress", "zh-CN", "进度", "代码生成操作后缀.进度"),
            // dict.gen.button.category.progress
            ("dict.gen.button.category.progress", "zh-HK", "进度", "代码生成操作后缀.进度"),

            // dict.gen.button.category.history
            ("dict.gen.button.category.history", "en-US", "history", "代码生成操作后缀.历史"),
            // dict.gen.button.category.history
            ("dict.gen.button.category.history", "ja-JP", "history", "代码生成操作后缀.历史"),
            // dict.gen.button.category.history
            ("dict.gen.button.category.history", "zh-CN", "历史", "代码生成操作后缀.历史"),
            // dict.gen.button.category.history
            ("dict.gen.button.category.history", "zh-HK", "历史", "代码生成操作后缀.历史"),

            // dict.gen.button.category.publish
            ("dict.gen.button.category.publish", "en-US", "publish", "代码生成操作后缀.发布"),
            // dict.gen.button.category.publish
            ("dict.gen.button.category.publish", "ja-JP", "publish", "代码生成操作后缀.发布"),
            // dict.gen.button.category.publish
            ("dict.gen.button.category.publish", "zh-CN", "发布", "代码生成操作后缀.发布"),
            // dict.gen.button.category.publish
            ("dict.gen.button.category.publish", "zh-HK", "发布", "代码生成操作后缀.发布"),

            // dict.gen.button.category.enable
            ("dict.gen.button.category.enable", "en-US", "enable", "代码生成操作后缀.启用"),
            // dict.gen.button.category.enable
            ("dict.gen.button.category.enable", "ja-JP", "enable", "代码生成操作后缀.启用"),
            // dict.gen.button.category.enable
            ("dict.gen.button.category.enable", "zh-CN", "启用", "代码生成操作后缀.启用"),
            // dict.gen.button.category.enable
            ("dict.gen.button.category.enable", "zh-HK", "启用", "代码生成操作后缀.启用"),

            // dict.gen.button.category.version
            ("dict.gen.button.category.version", "en-US", "version", "代码生成操作后缀.版本"),
            // dict.gen.button.category.version
            ("dict.gen.button.category.version", "ja-JP", "version", "代码生成操作后缀.版本"),
            // dict.gen.button.category.version
            ("dict.gen.button.category.version", "zh-CN", "版本", "代码生成操作后缀.版本"),
            // dict.gen.button.category.version
            ("dict.gen.button.category.version", "zh-HK", "版本", "代码生成操作后缀.版本"),

            // dict.gen.button.category.design
            ("dict.gen.button.category.design", "en-US", "design", "代码生成操作后缀.设计"),
            // dict.gen.button.category.design
            ("dict.gen.button.category.design", "ja-JP", "design", "代码生成操作后缀.设计"),
            // dict.gen.button.category.design
            ("dict.gen.button.category.design", "zh-CN", "设计", "代码生成操作后缀.设计"),
            // dict.gen.button.category.design
            ("dict.gen.button.category.design", "zh-HK", "设计", "代码生成操作后缀.设计"),

            // dict.gen.button.category.config
            ("dict.gen.button.category.config", "en-US", "config", "代码生成操作后缀.配置"),
            // dict.gen.button.category.config
            ("dict.gen.button.category.config", "ja-JP", "config", "代码生成操作后缀.配置"),
            // dict.gen.button.category.config
            ("dict.gen.button.category.config", "zh-CN", "配置", "代码生成操作后缀.配置"),
            // dict.gen.button.category.config
            ("dict.gen.button.category.config", "zh-HK", "配置", "代码生成操作后缀.配置"),

            // dict.gen.button.category.validate
            ("dict.gen.button.category.validate", "en-US", "validate", "代码生成操作后缀.验证"),
            // dict.gen.button.category.validate
            ("dict.gen.button.category.validate", "ja-JP", "validate", "代码生成操作后缀.验证"),
            // dict.gen.button.category.validate
            ("dict.gen.button.category.validate", "zh-CN", "验证", "代码生成操作后缀.验证"),
            // dict.gen.button.category.validate
            ("dict.gen.button.category.validate", "zh-HK", "验证", "代码生成操作后缀.验证"),

            // dict.gen.button.category.start
            ("dict.gen.button.category.start", "en-US", "start", "代码生成操作后缀.启动"),
            // dict.gen.button.category.start
            ("dict.gen.button.category.start", "ja-JP", "start", "代码生成操作后缀.启动"),
            // dict.gen.button.category.start
            ("dict.gen.button.category.start", "zh-CN", "启动", "代码生成操作后缀.启动"),
            // dict.gen.button.category.start
            ("dict.gen.button.category.start", "zh-HK", "启动", "代码生成操作后缀.启动"),

            // dict.gen.button.category.terminate
            ("dict.gen.button.category.terminate", "en-US", "terminate", "代码生成操作后缀.终止"),
            // dict.gen.button.category.terminate
            ("dict.gen.button.category.terminate", "ja-JP", "terminate", "代码生成操作后缀.终止"),
            // dict.gen.button.category.terminate
            ("dict.gen.button.category.terminate", "zh-CN", "终止", "代码生成操作后缀.终止"),
            // dict.gen.button.category.terminate
            ("dict.gen.button.category.terminate", "zh-HK", "终止", "代码生成操作后缀.终止"),

            // dict.gen.button.category.field
            ("dict.gen.button.category.field", "en-US", "field", "代码生成操作后缀.字段管理"),
            // dict.gen.button.category.field
            ("dict.gen.button.category.field", "ja-JP", "field", "代码生成操作后缀.字段管理"),
            // dict.gen.button.category.field
            ("dict.gen.button.category.field", "zh-CN", "字段管理", "代码生成操作后缀.字段管理"),
            // dict.gen.button.category.field
            ("dict.gen.button.category.field", "zh-HK", "字段管理", "代码生成操作后缀.字段管理"),

            // dict.gen.button.category.permission
            ("dict.gen.button.category.permission", "en-US", "permission", "代码生成操作后缀.权限设置"),
            // dict.gen.button.category.permission
            ("dict.gen.button.category.permission", "ja-JP", "permission", "代码生成操作后缀.权限设置"),
            // dict.gen.button.category.permission
            ("dict.gen.button.category.permission", "zh-CN", "权限设置", "代码生成操作后缀.权限设置"),
            // dict.gen.button.category.permission
            ("dict.gen.button.category.permission", "zh-HK", "权限设置", "代码生成操作后缀.权限设置"),

            // dict.gen.button.category.datasource
            ("dict.gen.button.category.datasource", "en-US", "datasource", "代码生成操作后缀.数据源配置"),
            // dict.gen.button.category.datasource
            ("dict.gen.button.category.datasource", "ja-JP", "datasource", "代码生成操作后缀.数据源配置"),
            // dict.gen.button.category.datasource
            ("dict.gen.button.category.datasource", "zh-CN", "数据源配置", "代码生成操作后缀.数据源配置"),
            // dict.gen.button.category.datasource
            ("dict.gen.button.category.datasource", "zh-HK", "数据源配置", "代码生成操作后缀.数据源配置"),

            // dict.gen.button.category.theme
            ("dict.gen.button.category.theme", "en-US", "theme", "代码生成操作后缀.主题设置"),
            // dict.gen.button.category.theme
            ("dict.gen.button.category.theme", "ja-JP", "theme", "代码生成操作后缀.主题设置"),
            // dict.gen.button.category.theme
            ("dict.gen.button.category.theme", "zh-CN", "主题设置", "代码生成操作后缀.主题设置"),
            // dict.gen.button.category.theme
            ("dict.gen.button.category.theme", "zh-HK", "主题设置", "代码生成操作后缀.主题设置"),

            // dict.gen.button.category.data
            ("dict.gen.button.category.data", "en-US", "data", "代码生成操作后缀.表单数据"),
            // dict.gen.button.category.data
            ("dict.gen.button.category.data", "ja-JP", "data", "代码生成操作后缀.表单数据"),
            // dict.gen.button.category.data
            ("dict.gen.button.category.data", "zh-CN", "表单数据", "代码生成操作后缀.表单数据"),
            // dict.gen.button.category.data
            ("dict.gen.button.category.data", "zh-HK", "表单数据", "代码生成操作后缀.表单数据"),

            // dict.gen.button.category.archive
            ("dict.gen.button.category.archive", "en-US", "archive", "代码生成操作后缀.流转归档"),
            // dict.gen.button.category.archive
            ("dict.gen.button.category.archive", "ja-JP", "archive", "代码生成操作后缀.流转归档"),
            // dict.gen.button.category.archive
            ("dict.gen.button.category.archive", "zh-CN", "流转归档", "代码生成操作后缀.流转归档"),
            // dict.gen.button.category.archive
            ("dict.gen.button.category.archive", "zh-HK", "流转归档", "代码生成操作后缀.流转归档"),

            // dict.gen.button.category.clean
            ("dict.gen.button.category.clean", "en-US", "clean", "代码生成操作后缀.流转清理"),
            // dict.gen.button.category.clean
            ("dict.gen.button.category.clean", "ja-JP", "clean", "代码生成操作后缀.流转清理"),
            // dict.gen.button.category.clean
            ("dict.gen.button.category.clean", "zh-CN", "流转清理", "代码生成操作后缀.流转清理"),
            // dict.gen.button.category.clean
            ("dict.gen.button.category.clean", "zh-HK", "流转清理", "代码生成操作后缀.流转清理"),

            // dict.gen.button.category.draft
            ("dict.gen.button.category.draft", "en-US", "draft", "代码生成操作后缀.保存草稿"),
            // dict.gen.button.category.draft
            ("dict.gen.button.category.draft", "ja-JP", "draft", "代码生成操作后缀.保存草稿"),
            // dict.gen.button.category.draft
            ("dict.gen.button.category.draft", "zh-CN", "保存草稿", "代码生成操作后缀.保存草稿"),
            // dict.gen.button.category.draft
            ("dict.gen.button.category.draft", "zh-HK", "保存草稿", "代码生成操作后缀.保存草稿"),

            // dict.gen.button.category.deletedraft
            ("dict.gen.button.category.deletedraft", "en-US", "deletedraft", "代码生成操作后缀.删除草稿"),
            // dict.gen.button.category.deletedraft
            ("dict.gen.button.category.deletedraft", "ja-JP", "deletedraft", "代码生成操作后缀.删除草稿"),
            // dict.gen.button.category.deletedraft
            ("dict.gen.button.category.deletedraft", "zh-CN", "删除草稿", "代码生成操作后缀.删除草稿"),
            // dict.gen.button.category.deletedraft
            ("dict.gen.button.category.deletedraft", "zh-HK", "删除草稿", "代码生成操作后缀.删除草稿"),

            // dict.gen.button.category.send
            ("dict.gen.button.category.send", "en-US", "send", "代码生成操作后缀.发送"),
            // dict.gen.button.category.send
            ("dict.gen.button.category.send", "ja-JP", "send", "代码生成操作后缀.发送"),
            // dict.gen.button.category.send
            ("dict.gen.button.category.send", "zh-CN", "发送", "代码生成操作后缀.发送"),
            // dict.gen.button.category.send
            ("dict.gen.button.category.send", "zh-HK", "发送", "代码生成操作后缀.发送"),

            // dict.gen.button.category.forward
            ("dict.gen.button.category.forward", "en-US", "forward", "代码生成操作后缀.转发"),
            // dict.gen.button.category.forward
            ("dict.gen.button.category.forward", "ja-JP", "forward", "代码生成操作后缀.转发"),
            // dict.gen.button.category.forward
            ("dict.gen.button.category.forward", "zh-CN", "转发", "代码生成操作后缀.转发"),
            // dict.gen.button.category.forward
            ("dict.gen.button.category.forward", "zh-HK", "转发", "代码生成操作后缀.转发"),

            // dict.gen.button.category.reply
            ("dict.gen.button.category.reply", "en-US", "reply", "代码生成操作后缀.回复"),
            // dict.gen.button.category.reply
            ("dict.gen.button.category.reply", "ja-JP", "reply", "代码生成操作后缀.回复"),
            // dict.gen.button.category.reply
            ("dict.gen.button.category.reply", "zh-CN", "回复", "代码生成操作后缀.回复"),
            // dict.gen.button.category.reply
            ("dict.gen.button.category.reply", "zh-HK", "回复", "代码生成操作后缀.回复"),

            // dict.gen.button.category.read
            ("dict.gen.button.category.read", "en-US", "read", "代码生成操作后缀.已读"),
            // dict.gen.button.category.read
            ("dict.gen.button.category.read", "ja-JP", "read", "代码生成操作后缀.已读"),
            // dict.gen.button.category.read
            ("dict.gen.button.category.read", "zh-CN", "已读", "代码生成操作后缀.已读"),
            // dict.gen.button.category.read
            ("dict.gen.button.category.read", "zh-HK", "已读", "代码生成操作后缀.已读"),

            // dict.gen.button.category.unread
            ("dict.gen.button.category.unread", "en-US", "unread", "代码生成操作后缀.未读"),
            // dict.gen.button.category.unread
            ("dict.gen.button.category.unread", "ja-JP", "unread", "代码生成操作后缀.未读"),
            // dict.gen.button.category.unread
            ("dict.gen.button.category.unread", "zh-CN", "未读", "代码生成操作后缀.未读"),
            // dict.gen.button.category.unread
            ("dict.gen.button.category.unread", "zh-HK", "未读", "代码生成操作后缀.未读"),

            // dict.gen.button.category.circulate
            ("dict.gen.button.category.circulate", "en-US", "circulate", "代码生成操作后缀.传阅"),
            // dict.gen.button.category.circulate
            ("dict.gen.button.category.circulate", "ja-JP", "circulate", "代码生成操作后缀.传阅"),
            // dict.gen.button.category.circulate
            ("dict.gen.button.category.circulate", "zh-CN", "传阅", "代码生成操作后缀.传阅"),
            // dict.gen.button.category.circulate
            ("dict.gen.button.category.circulate", "zh-HK", "传阅", "代码生成操作后缀.传阅"),

            // dict.gen.button.category.sign
            ("dict.gen.button.category.sign", "en-US", "sign", "代码生成操作后缀.签收"),
            // dict.gen.button.category.sign
            ("dict.gen.button.category.sign", "ja-JP", "sign", "代码生成操作后缀.签收"),
            // dict.gen.button.category.sign
            ("dict.gen.button.category.sign", "zh-CN", "签收", "代码生成操作后缀.签收"),
            // dict.gen.button.category.sign
            ("dict.gen.button.category.sign", "zh-HK", "签收", "代码生成操作后缀.签收"),

            // dict.gen.button.category.confirm
            ("dict.gen.button.category.confirm", "en-US", "confirm", "代码生成操作后缀.确认"),
            // dict.gen.button.category.confirm
            ("dict.gen.button.category.confirm", "ja-JP", "confirm", "代码生成操作后缀.确认"),
            // dict.gen.button.category.confirm
            ("dict.gen.button.category.confirm", "zh-CN", "确认", "代码生成操作后缀.确认"),
            // dict.gen.button.category.confirm
            ("dict.gen.button.category.confirm", "zh-HK", "确认", "代码生成操作后缀.确认"),

            // dict.gen.button.category.like
            ("dict.gen.button.category.like", "en-US", "like", "代码生成操作后缀.点赞"),
            // dict.gen.button.category.like
            ("dict.gen.button.category.like", "ja-JP", "like", "代码生成操作后缀.点赞"),
            // dict.gen.button.category.like
            ("dict.gen.button.category.like", "zh-CN", "点赞", "代码生成操作后缀.点赞"),
            // dict.gen.button.category.like
            ("dict.gen.button.category.like", "zh-HK", "点赞", "代码生成操作后缀.点赞"),

            // dict.gen.button.category.unlike
            ("dict.gen.button.category.unlike", "en-US", "unlike", "代码生成操作后缀.取消点赞"),
            // dict.gen.button.category.unlike
            ("dict.gen.button.category.unlike", "ja-JP", "unlike", "代码生成操作后缀.取消点赞"),
            // dict.gen.button.category.unlike
            ("dict.gen.button.category.unlike", "zh-CN", "取消点赞", "代码生成操作后缀.取消点赞"),
            // dict.gen.button.category.unlike
            ("dict.gen.button.category.unlike", "zh-HK", "取消点赞", "代码生成操作后缀.取消点赞"),

            // dict.gen.button.category.favorite
            ("dict.gen.button.category.favorite", "en-US", "favorite", "代码生成操作后缀.收藏"),
            // dict.gen.button.category.favorite
            ("dict.gen.button.category.favorite", "ja-JP", "favorite", "代码生成操作后缀.收藏"),
            // dict.gen.button.category.favorite
            ("dict.gen.button.category.favorite", "zh-CN", "收藏", "代码生成操作后缀.收藏"),
            // dict.gen.button.category.favorite
            ("dict.gen.button.category.favorite", "zh-HK", "收藏", "代码生成操作后缀.收藏"),

            // dict.gen.button.category.unfavorite
            ("dict.gen.button.category.unfavorite", "en-US", "unfavorite", "代码生成操作后缀.取消收藏"),
            // dict.gen.button.category.unfavorite
            ("dict.gen.button.category.unfavorite", "ja-JP", "unfavorite", "代码生成操作后缀.取消收藏"),
            // dict.gen.button.category.unfavorite
            ("dict.gen.button.category.unfavorite", "zh-CN", "取消收藏", "代码生成操作后缀.取消收藏"),
            // dict.gen.button.category.unfavorite
            ("dict.gen.button.category.unfavorite", "zh-HK", "取消收藏", "代码生成操作后缀.取消收藏"),

            // dict.gen.button.category.share
            ("dict.gen.button.category.share", "en-US", "share", "代码生成操作后缀.分享"),
            // dict.gen.button.category.share
            ("dict.gen.button.category.share", "ja-JP", "share", "代码生成操作后缀.分享"),
            // dict.gen.button.category.share
            ("dict.gen.button.category.share", "zh-CN", "分享", "代码生成操作后缀.分享"),
            // dict.gen.button.category.share
            ("dict.gen.button.category.share", "zh-HK", "分享", "代码生成操作后缀.分享"),

            // dict.gen.button.category.unshare
            ("dict.gen.button.category.unshare", "en-US", "unshare", "代码生成操作后缀.取消分享"),
            // dict.gen.button.category.unshare
            ("dict.gen.button.category.unshare", "ja-JP", "unshare", "代码生成操作后缀.取消分享"),
            // dict.gen.button.category.unshare
            ("dict.gen.button.category.unshare", "zh-CN", "取消分享", "代码生成操作后缀.取消分享"),
            // dict.gen.button.category.unshare
            ("dict.gen.button.category.unshare", "zh-HK", "取消分享", "代码生成操作后缀.取消分享"),

            // dict.gen.button.category.comment
            ("dict.gen.button.category.comment", "en-US", "comment", "代码生成操作后缀.评论"),
            // dict.gen.button.category.comment
            ("dict.gen.button.category.comment", "ja-JP", "comment", "代码生成操作后缀.评论"),
            // dict.gen.button.category.comment
            ("dict.gen.button.category.comment", "zh-CN", "评论", "代码生成操作后缀.评论"),
            // dict.gen.button.category.comment
            ("dict.gen.button.category.comment", "zh-HK", "评论", "代码生成操作后缀.评论"),

            // dict.gen.button.category.uncomment
            ("dict.gen.button.category.uncomment", "en-US", "uncomment", "代码生成操作后缀.取消评论"),
            // dict.gen.button.category.uncomment
            ("dict.gen.button.category.uncomment", "ja-JP", "uncomment", "代码生成操作后缀.取消评论"),
            // dict.gen.button.category.uncomment
            ("dict.gen.button.category.uncomment", "zh-CN", "取消评论", "代码生成操作后缀.取消评论"),
            // dict.gen.button.category.uncomment
            ("dict.gen.button.category.uncomment", "zh-HK", "取消评论", "代码生成操作后缀.取消评论"),

            // dict.gen.button.category.flagging
            ("dict.gen.button.category.flagging", "en-US", "flagging", "代码生成操作后缀.举报"),
            // dict.gen.button.category.flagging
            ("dict.gen.button.category.flagging", "ja-JP", "flagging", "代码生成操作后缀.举报"),
            // dict.gen.button.category.flagging
            ("dict.gen.button.category.flagging", "zh-CN", "举报", "代码生成操作后缀.举报"),
            // dict.gen.button.category.flagging
            ("dict.gen.button.category.flagging", "zh-HK", "举报", "代码生成操作后缀.举报"),

            // dict.gen.button.category.unflagging
            ("dict.gen.button.category.unflagging", "en-US", "unflagging", "代码生成操作后缀.取消举报"),
            // dict.gen.button.category.unflagging
            ("dict.gen.button.category.unflagging", "ja-JP", "unflagging", "代码生成操作后缀.取消举报"),
            // dict.gen.button.category.unflagging
            ("dict.gen.button.category.unflagging", "zh-CN", "取消举报", "代码生成操作后缀.取消举报"),
            // dict.gen.button.category.unflagging
            ("dict.gen.button.category.unflagging", "zh-HK", "取消举报", "代码生成操作后缀.取消举报"),

            // dict.gen.button.category.follow
            ("dict.gen.button.category.follow", "en-US", "follow", "代码生成操作后缀.关注"),
            // dict.gen.button.category.follow
            ("dict.gen.button.category.follow", "ja-JP", "follow", "代码生成操作后缀.关注"),
            // dict.gen.button.category.follow
            ("dict.gen.button.category.follow", "zh-CN", "关注", "代码生成操作后缀.关注"),
            // dict.gen.button.category.follow
            ("dict.gen.button.category.follow", "zh-HK", "关注", "代码生成操作后缀.关注"),

            // dict.gen.button.category.unfollow
            ("dict.gen.button.category.unfollow", "en-US", "unfollow", "代码生成操作后缀.取消关注"),
            // dict.gen.button.category.unfollow
            ("dict.gen.button.category.unfollow", "ja-JP", "unfollow", "代码生成操作后缀.取消关注"),
            // dict.gen.button.category.unfollow
            ("dict.gen.button.category.unfollow", "zh-CN", "取消关注", "代码生成操作后缀.取消关注"),
            // dict.gen.button.category.unfollow
            ("dict.gen.button.category.unfollow", "zh-HK", "取消关注", "代码生成操作后缀.取消关注"),

            // dict.gen.button.category.upload
            ("dict.gen.button.category.upload", "en-US", "upload", "代码生成操作后缀.上传"),
            // dict.gen.button.category.upload
            ("dict.gen.button.category.upload", "ja-JP", "upload", "代码生成操作后缀.上传"),
            // dict.gen.button.category.upload
            ("dict.gen.button.category.upload", "zh-CN", "上传", "代码生成操作后缀.上传"),
            // dict.gen.button.category.upload
            ("dict.gen.button.category.upload", "zh-HK", "上传", "代码生成操作后缀.上传"),

            // dict.gen.button.category.destroy
            ("dict.gen.button.category.destroy", "en-US", "destroy", "代码生成操作后缀.销毁"),
            // dict.gen.button.category.destroy
            ("dict.gen.button.category.destroy", "ja-JP", "destroy", "代码生成操作后缀.销毁"),
            // dict.gen.button.category.destroy
            ("dict.gen.button.category.destroy", "zh-CN", "销毁", "代码生成操作后缀.销毁"),
            // dict.gen.button.category.destroy
            ("dict.gen.button.category.destroy", "zh-HK", "销毁", "代码生成操作后缀.销毁"),

            // dict.gen.button.category.run
            ("dict.gen.button.category.run", "en-US", "run", "代码生成操作后缀.运行"),
            // dict.gen.button.category.run
            ("dict.gen.button.category.run", "ja-JP", "run", "代码生成操作后缀.运行"),
            // dict.gen.button.category.run
            ("dict.gen.button.category.run", "zh-CN", "运行", "代码生成操作后缀.运行"),
            // dict.gen.button.category.run
            ("dict.gen.button.category.run", "zh-HK", "运行", "代码生成操作后缀.运行"),

            // dict.gen.button.category.stop
            ("dict.gen.button.category.stop", "en-US", "stop", "代码生成操作后缀.停止"),
            // dict.gen.button.category.stop
            ("dict.gen.button.category.stop", "ja-JP", "stop", "代码生成操作后缀.停止"),
            // dict.gen.button.category.stop
            ("dict.gen.button.category.stop", "zh-CN", "停止", "代码生成操作后缀.停止"),
            // dict.gen.button.category.stop
            ("dict.gen.button.category.stop", "zh-HK", "停止", "代码生成操作后缀.停止"),

            // dict.gen.button.category.restart
            ("dict.gen.button.category.restart", "en-US", "restart", "代码生成操作后缀.重启"),
            // dict.gen.button.category.restart
            ("dict.gen.button.category.restart", "ja-JP", "restart", "代码生成操作后缀.重启"),
            // dict.gen.button.category.restart
            ("dict.gen.button.category.restart", "zh-CN", "重启", "代码生成操作后缀.重启"),
            // dict.gen.button.category.restart
            ("dict.gen.button.category.restart", "zh-HK", "重启", "代码生成操作后缀.重启"),

            // dict.gen.button.category.refresh
            ("dict.gen.button.category.refresh", "en-US", "refresh", "代码生成操作后缀.刷新"),
            // dict.gen.button.category.refresh
            ("dict.gen.button.category.refresh", "ja-JP", "refresh", "代码生成操作后缀.刷新"),
            // dict.gen.button.category.refresh
            ("dict.gen.button.category.refresh", "zh-CN", "刷新", "代码生成操作后缀.刷新"),
            // dict.gen.button.category.refresh
            ("dict.gen.button.category.refresh", "zh-HK", "刷新", "代码生成操作后缀.刷新"),

            // dict.gen.button.category.reset
            ("dict.gen.button.category.reset", "en-US", "reset", "代码生成操作后缀.重置"),
            // dict.gen.button.category.reset
            ("dict.gen.button.category.reset", "ja-JP", "reset", "代码生成操作后缀.重置"),
            // dict.gen.button.category.reset
            ("dict.gen.button.category.reset", "zh-CN", "重置", "代码生成操作后缀.重置"),
            // dict.gen.button.category.reset
            ("dict.gen.button.category.reset", "zh-HK", "重置", "代码生成操作后缀.重置"),

            // dict.gen.button.category.calculate
            ("dict.gen.button.category.calculate", "en-US", "calculate", "代码生成操作后缀.核算"),
            // dict.gen.button.category.calculate
            ("dict.gen.button.category.calculate", "ja-JP", "calculate", "代码生成操作后缀.核算"),
            // dict.gen.button.category.calculate
            ("dict.gen.button.category.calculate", "zh-CN", "核算", "代码生成操作后缀.核算"),
            // dict.gen.button.category.calculate
            ("dict.gen.button.category.calculate", "zh-HK", "核算", "代码生成操作后缀.核算"),

            // dict.gen.button.category.book
            ("dict.gen.button.category.book", "en-US", "book", "代码生成操作后缀.记账"),
            // dict.gen.button.category.book
            ("dict.gen.button.category.book", "ja-JP", "book", "代码生成操作后缀.记账"),
            // dict.gen.button.category.book
            ("dict.gen.button.category.book", "zh-CN", "记账", "代码生成操作后缀.记账"),
            // dict.gen.button.category.book
            ("dict.gen.button.category.book", "zh-HK", "记账", "代码生成操作后缀.记账"),

            // dict.gen.button.category.closing
            ("dict.gen.button.category.closing", "en-US", "closing", "代码生成操作后缀.结账"),
            // dict.gen.button.category.closing
            ("dict.gen.button.category.closing", "ja-JP", "closing", "代码生成操作后缀.结账"),
            // dict.gen.button.category.closing
            ("dict.gen.button.category.closing", "zh-CN", "结账", "代码生成操作后缀.结账"),
            // dict.gen.button.category.closing
            ("dict.gen.button.category.closing", "zh-HK", "结账", "代码生成操作后缀.结账"),

            // dict.gen.button.category.reconcile
            ("dict.gen.button.category.reconcile", "en-US", "reconcile", "代码生成操作后缀.对账"),
            // dict.gen.button.category.reconcile
            ("dict.gen.button.category.reconcile", "ja-JP", "reconcile", "代码生成操作后缀.对账"),
            // dict.gen.button.category.reconcile
            ("dict.gen.button.category.reconcile", "zh-CN", "对账", "代码生成操作后缀.对账"),
            // dict.gen.button.category.reconcile
            ("dict.gen.button.category.reconcile", "zh-HK", "对账", "代码生成操作后缀.对账"),

            // dict.gen.button.category.payment
            ("dict.gen.button.category.payment", "en-US", "payment", "代码生成操作后缀.支付"),
            // dict.gen.button.category.payment
            ("dict.gen.button.category.payment", "ja-JP", "payment", "代码生成操作后缀.支付"),
            // dict.gen.button.category.payment
            ("dict.gen.button.category.payment", "zh-CN", "支付", "代码生成操作后缀.支付"),
            // dict.gen.button.category.payment
            ("dict.gen.button.category.payment", "zh-HK", "支付", "代码生成操作后缀.支付"),

            // dict.gen.button.category.depreciation
            ("dict.gen.button.category.depreciation", "en-US", "depreciation", "代码生成操作后缀.折旧"),
            // dict.gen.button.category.depreciation
            ("dict.gen.button.category.depreciation", "ja-JP", "depreciation", "代码生成操作后缀.折旧"),
            // dict.gen.button.category.depreciation
            ("dict.gen.button.category.depreciation", "zh-CN", "折旧", "代码生成操作后缀.折旧"),
            // dict.gen.button.category.depreciation
            ("dict.gen.button.category.depreciation", "zh-HK", "折旧", "代码生成操作后缀.折旧"),

            // dict.gen.button.category.reimburse
            ("dict.gen.button.category.reimburse", "en-US", "reimburse", "代码生成操作后缀.报销"),
            // dict.gen.button.category.reimburse
            ("dict.gen.button.category.reimburse", "ja-JP", "reimburse", "代码生成操作后缀.报销"),
            // dict.gen.button.category.reimburse
            ("dict.gen.button.category.reimburse", "zh-CN", "报销", "代码生成操作后缀.报销"),
            // dict.gen.button.category.reimburse
            ("dict.gen.button.category.reimburse", "zh-HK", "报销", "代码生成操作后缀.报销"),

            // dict.gen.button.category.reversal
            ("dict.gen.button.category.reversal", "en-US", "reversal", "代码生成操作后缀.冲销"),
            // dict.gen.button.category.reversal
            ("dict.gen.button.category.reversal", "ja-JP", "reversal", "代码生成操作后缀.冲销"),
            // dict.gen.button.category.reversal
            ("dict.gen.button.category.reversal", "zh-CN", "冲销", "代码生成操作后缀.冲销"),
            // dict.gen.button.category.reversal
            ("dict.gen.button.category.reversal", "zh-HK", "冲销", "代码生成操作后缀.冲销"),

            // dict.gen.button.category.accrual
            ("dict.gen.button.category.accrual", "en-US", "accrual", "代码生成操作后缀.计提"),
            // dict.gen.button.category.accrual
            ("dict.gen.button.category.accrual", "ja-JP", "accrual", "代码生成操作后缀.计提"),
            // dict.gen.button.category.accrual
            ("dict.gen.button.category.accrual", "zh-CN", "计提", "代码生成操作后缀.计提"),
            // dict.gen.button.category.accrual
            ("dict.gen.button.category.accrual", "zh-HK", "计提", "代码生成操作后缀.计提"),

            // dict.gen.button.category.period
            ("dict.gen.button.category.period", "en-US", "period", "代码生成操作后缀.账期"),
            // dict.gen.button.category.period
            ("dict.gen.button.category.period", "ja-JP", "period", "代码生成操作后缀.账期"),
            // dict.gen.button.category.period
            ("dict.gen.button.category.period", "zh-CN", "账期", "代码生成操作后缀.账期"),
            // dict.gen.button.category.period
            ("dict.gen.button.category.period", "zh-HK", "账期", "代码生成操作后缀.账期"),

            // dict.gen.button.category.carryforward
            ("dict.gen.button.category.carryforward", "en-US", "carryforward", "代码生成操作后缀.结转"),
            // dict.gen.button.category.carryforward
            ("dict.gen.button.category.carryforward", "ja-JP", "carryforward", "代码生成操作后缀.结转"),
            // dict.gen.button.category.carryforward
            ("dict.gen.button.category.carryforward", "zh-CN", "结转", "代码生成操作后缀.结转"),
            // dict.gen.button.category.carryforward
            ("dict.gen.button.category.carryforward", "zh-HK", "结转", "代码生成操作后缀.结转"),

            // dict.gen.button.category.cancel
            ("dict.gen.button.category.cancel", "en-US", "cancel", "代码生成操作后缀.作废"),
            // dict.gen.button.category.cancel
            ("dict.gen.button.category.cancel", "ja-JP", "cancel", "代码生成操作后缀.作废"),
            // dict.gen.button.category.cancel
            ("dict.gen.button.category.cancel", "zh-CN", "作废", "代码生成操作后缀.作废"),
            // dict.gen.button.category.cancel
            ("dict.gen.button.category.cancel", "zh-HK", "作废", "代码生成操作后缀.作废"),

            // dict.gen.button.category.change
            ("dict.gen.button.category.change", "en-US", "change", "代码生成操作后缀.变更"),
            // dict.gen.button.category.change
            ("dict.gen.button.category.change", "ja-JP", "change", "代码生成操作后缀.变更"),
            // dict.gen.button.category.change
            ("dict.gen.button.category.change", "zh-CN", "变更", "代码生成操作后缀.变更"),
            // dict.gen.button.category.change
            ("dict.gen.button.category.change", "zh-HK", "变更", "代码生成操作后缀.变更"),

            // dict.gen.button.style.0
            ("dict.gen.button.style.0", "en-US", "文本", "操作按钮样式.文本"),
            // dict.gen.button.style.0
            ("dict.gen.button.style.0", "ja-JP", "文本", "操作按钮样式.文本"),
            // dict.gen.button.style.0
            ("dict.gen.button.style.0", "zh-CN", "文本", "操作按钮样式.文本"),
            // dict.gen.button.style.0
            ("dict.gen.button.style.0", "zh-HK", "文本", "操作按钮样式.文本"),

            // dict.gen.button.style.1
            ("dict.gen.button.style.1", "en-US", "标准", "操作按钮样式.标准"),
            // dict.gen.button.style.1
            ("dict.gen.button.style.1", "ja-JP", "标准", "操作按钮样式.标准"),
            // dict.gen.button.style.1
            ("dict.gen.button.style.1", "zh-CN", "标准", "操作按钮样式.标准"),
            // dict.gen.button.style.1
            ("dict.gen.button.style.1", "zh-HK", "标准", "操作按钮样式.标准"),

            // dict.gen.csharp.data.type.bool
            ("dict.gen.csharp.data.type.bool", "en-US", "bool", "c#数据类型.bool"),
            // dict.gen.csharp.data.type.bool
            ("dict.gen.csharp.data.type.bool", "ja-JP", "bool", "c#数据类型.bool"),
            // dict.gen.csharp.data.type.bool
            ("dict.gen.csharp.data.type.bool", "zh-CN", "bool", "c#数据类型.bool"),
            // dict.gen.csharp.data.type.bool
            ("dict.gen.csharp.data.type.bool", "zh-HK", "bool", "c#数据类型.bool"),

            // dict.gen.csharp.data.type.byte
            ("dict.gen.csharp.data.type.byte", "en-US", "byte", "c#数据类型.byte"),
            // dict.gen.csharp.data.type.byte
            ("dict.gen.csharp.data.type.byte", "ja-JP", "byte", "c#数据类型.byte"),
            // dict.gen.csharp.data.type.byte
            ("dict.gen.csharp.data.type.byte", "zh-CN", "byte", "c#数据类型.byte"),
            // dict.gen.csharp.data.type.byte
            ("dict.gen.csharp.data.type.byte", "zh-HK", "byte", "c#数据类型.byte"),

            // dict.gen.csharp.data.type.datetime
            ("dict.gen.csharp.data.type.datetime", "en-US", "datetime", "c#数据类型.datetime"),
            // dict.gen.csharp.data.type.datetime
            ("dict.gen.csharp.data.type.datetime", "ja-JP", "datetime", "c#数据类型.datetime"),
            // dict.gen.csharp.data.type.datetime
            ("dict.gen.csharp.data.type.datetime", "zh-CN", "datetime", "c#数据类型.datetime"),
            // dict.gen.csharp.data.type.datetime
            ("dict.gen.csharp.data.type.datetime", "zh-HK", "datetime", "c#数据类型.datetime"),

            // dict.gen.csharp.data.type.decimal
            ("dict.gen.csharp.data.type.decimal", "en-US", "decimal", "c#数据类型.decimal"),
            // dict.gen.csharp.data.type.decimal
            ("dict.gen.csharp.data.type.decimal", "ja-JP", "decimal", "c#数据类型.decimal"),
            // dict.gen.csharp.data.type.decimal
            ("dict.gen.csharp.data.type.decimal", "zh-CN", "decimal", "c#数据类型.decimal"),
            // dict.gen.csharp.data.type.decimal
            ("dict.gen.csharp.data.type.decimal", "zh-HK", "decimal", "c#数据类型.decimal"),

            // dict.gen.csharp.data.type.double
            ("dict.gen.csharp.data.type.double", "en-US", "double", "c#数据类型.double"),
            // dict.gen.csharp.data.type.double
            ("dict.gen.csharp.data.type.double", "ja-JP", "double", "c#数据类型.double"),
            // dict.gen.csharp.data.type.double
            ("dict.gen.csharp.data.type.double", "zh-CN", "double", "c#数据类型.double"),
            // dict.gen.csharp.data.type.double
            ("dict.gen.csharp.data.type.double", "zh-HK", "double", "c#数据类型.double"),

            // dict.gen.csharp.data.type.float
            ("dict.gen.csharp.data.type.float", "en-US", "float", "c#数据类型.float"),
            // dict.gen.csharp.data.type.float
            ("dict.gen.csharp.data.type.float", "ja-JP", "float", "c#数据类型.float"),
            // dict.gen.csharp.data.type.float
            ("dict.gen.csharp.data.type.float", "zh-CN", "float", "c#数据类型.float"),
            // dict.gen.csharp.data.type.float
            ("dict.gen.csharp.data.type.float", "zh-HK", "float", "c#数据类型.float"),

            // dict.gen.csharp.data.type.guid
            ("dict.gen.csharp.data.type.guid", "en-US", "guid", "c#数据类型.guid"),
            // dict.gen.csharp.data.type.guid
            ("dict.gen.csharp.data.type.guid", "ja-JP", "guid", "c#数据类型.guid"),
            // dict.gen.csharp.data.type.guid
            ("dict.gen.csharp.data.type.guid", "zh-CN", "guid", "c#数据类型.guid"),
            // dict.gen.csharp.data.type.guid
            ("dict.gen.csharp.data.type.guid", "zh-HK", "guid", "c#数据类型.guid"),

            // dict.gen.csharp.data.type.int
            ("dict.gen.csharp.data.type.int", "en-US", "int", "c#数据类型.int"),
            // dict.gen.csharp.data.type.int
            ("dict.gen.csharp.data.type.int", "ja-JP", "int", "c#数据类型.int"),
            // dict.gen.csharp.data.type.int
            ("dict.gen.csharp.data.type.int", "zh-CN", "int", "c#数据类型.int"),
            // dict.gen.csharp.data.type.int
            ("dict.gen.csharp.data.type.int", "zh-HK", "int", "c#数据类型.int"),

            // dict.gen.csharp.data.type.long
            ("dict.gen.csharp.data.type.long", "en-US", "long", "c#数据类型.long"),
            // dict.gen.csharp.data.type.long
            ("dict.gen.csharp.data.type.long", "ja-JP", "long", "c#数据类型.long"),
            // dict.gen.csharp.data.type.long
            ("dict.gen.csharp.data.type.long", "zh-CN", "long", "c#数据类型.long"),
            // dict.gen.csharp.data.type.long
            ("dict.gen.csharp.data.type.long", "zh-HK", "long", "c#数据类型.long"),

            // dict.gen.csharp.data.type.string
            ("dict.gen.csharp.data.type.string", "en-US", "string", "c#数据类型.string"),
            // dict.gen.csharp.data.type.string
            ("dict.gen.csharp.data.type.string", "ja-JP", "string", "c#数据类型.string"),
            // dict.gen.csharp.data.type.string
            ("dict.gen.csharp.data.type.string", "zh-CN", "string", "c#数据类型.string"),
            // dict.gen.csharp.data.type.string
            ("dict.gen.csharp.data.type.string", "zh-HK", "string", "c#数据类型.string"),

            // dict.gen.display.type.input
            ("dict.gen.display.type.input", "en-US", "input", "显示类型.文本框"),
            // dict.gen.display.type.input
            ("dict.gen.display.type.input", "ja-JP", "input", "显示类型.文本框"),
            // dict.gen.display.type.input
            ("dict.gen.display.type.input", "zh-CN", "文本框", "显示类型.文本框"),
            // dict.gen.display.type.input
            ("dict.gen.display.type.input", "zh-HK", "文本框", "显示类型.文本框"),

            // dict.gen.display.type.inputnumber
            ("dict.gen.display.type.inputnumber", "en-US", "inputnumber", "显示类型.数字输入框"),
            // dict.gen.display.type.inputnumber
            ("dict.gen.display.type.inputnumber", "ja-JP", "inputnumber", "显示类型.数字输入框"),
            // dict.gen.display.type.inputnumber
            ("dict.gen.display.type.inputnumber", "zh-CN", "数字输入框", "显示类型.数字输入框"),
            // dict.gen.display.type.inputnumber
            ("dict.gen.display.type.inputnumber", "zh-HK", "数字输入框", "显示类型.数字输入框"),

            // dict.gen.display.type.select
            ("dict.gen.display.type.select", "en-US", "select", "显示类型.下拉框"),
            // dict.gen.display.type.select
            ("dict.gen.display.type.select", "ja-JP", "select", "显示类型.下拉框"),
            // dict.gen.display.type.select
            ("dict.gen.display.type.select", "zh-CN", "下拉框", "显示类型.下拉框"),
            // dict.gen.display.type.select
            ("dict.gen.display.type.select", "zh-HK", "下拉框", "显示类型.下拉框"),

            // dict.gen.display.type.checkbox
            ("dict.gen.display.type.checkbox", "en-US", "checkbox", "显示类型.复选框"),
            // dict.gen.display.type.checkbox
            ("dict.gen.display.type.checkbox", "ja-JP", "checkbox", "显示类型.复选框"),
            // dict.gen.display.type.checkbox
            ("dict.gen.display.type.checkbox", "zh-CN", "复选框", "显示类型.复选框"),
            // dict.gen.display.type.checkbox
            ("dict.gen.display.type.checkbox", "zh-HK", "复选框", "显示类型.复选框"),

            // dict.gen.display.type.radio
            ("dict.gen.display.type.radio", "en-US", "radio", "显示类型.单选框"),
            // dict.gen.display.type.radio
            ("dict.gen.display.type.radio", "ja-JP", "radio", "显示类型.单选框"),
            // dict.gen.display.type.radio
            ("dict.gen.display.type.radio", "zh-CN", "单选框", "显示类型.单选框"),
            // dict.gen.display.type.radio
            ("dict.gen.display.type.radio", "zh-HK", "单选框", "显示类型.单选框"),

            // dict.gen.display.type.date
            ("dict.gen.display.type.date", "en-US", "date", "显示类型.日期控件"),
            // dict.gen.display.type.date
            ("dict.gen.display.type.date", "ja-JP", "date", "显示类型.日期控件"),
            // dict.gen.display.type.date
            ("dict.gen.display.type.date", "zh-CN", "日期控件", "显示类型.日期控件"),
            // dict.gen.display.type.date
            ("dict.gen.display.type.date", "zh-HK", "日期控件", "显示类型.日期控件"),

            // dict.gen.display.type.time
            ("dict.gen.display.type.time", "en-US", "time", "显示类型.时间控件"),
            // dict.gen.display.type.time
            ("dict.gen.display.type.time", "ja-JP", "time", "显示类型.时间控件"),
            // dict.gen.display.type.time
            ("dict.gen.display.type.time", "zh-CN", "时间控件", "显示类型.时间控件"),
            // dict.gen.display.type.time
            ("dict.gen.display.type.time", "zh-HK", "时间控件", "显示类型.时间控件"),

            // dict.gen.display.type.image
            ("dict.gen.display.type.image", "en-US", "image", "显示类型.图片上传"),
            // dict.gen.display.type.image
            ("dict.gen.display.type.image", "ja-JP", "image", "显示类型.图片上传"),
            // dict.gen.display.type.image
            ("dict.gen.display.type.image", "zh-CN", "图片上传", "显示类型.图片上传"),
            // dict.gen.display.type.image
            ("dict.gen.display.type.image", "zh-HK", "图片上传", "显示类型.图片上传"),

            // dict.gen.display.type.file
            ("dict.gen.display.type.file", "en-US", "file", "显示类型.文件上传"),
            // dict.gen.display.type.file
            ("dict.gen.display.type.file", "ja-JP", "file", "显示类型.文件上传"),
            // dict.gen.display.type.file
            ("dict.gen.display.type.file", "zh-CN", "文件上传", "显示类型.文件上传"),
            // dict.gen.display.type.file
            ("dict.gen.display.type.file", "zh-HK", "文件上传", "显示类型.文件上传"),

            // dict.gen.display.type.slider
            ("dict.gen.display.type.slider", "en-US", "slider", "显示类型.滑块"),
            // dict.gen.display.type.slider
            ("dict.gen.display.type.slider", "ja-JP", "slider", "显示类型.滑块"),
            // dict.gen.display.type.slider
            ("dict.gen.display.type.slider", "zh-CN", "滑块", "显示类型.滑块"),
            // dict.gen.display.type.slider
            ("dict.gen.display.type.slider", "zh-HK", "滑块", "显示类型.滑块"),

            // dict.gen.display.type.switch
            ("dict.gen.display.type.switch", "en-US", "switch", "显示类型.开关"),
            // dict.gen.display.type.switch
            ("dict.gen.display.type.switch", "ja-JP", "switch", "显示类型.开关"),
            // dict.gen.display.type.switch
            ("dict.gen.display.type.switch", "zh-CN", "开关", "显示类型.开关"),
            // dict.gen.display.type.switch
            ("dict.gen.display.type.switch", "zh-HK", "开关", "显示类型.开关"),

            // dict.gen.display.type.rate
            ("dict.gen.display.type.rate", "en-US", "rate", "显示类型.评分"),
            // dict.gen.display.type.rate
            ("dict.gen.display.type.rate", "ja-JP", "rate", "显示类型.评分"),
            // dict.gen.display.type.rate
            ("dict.gen.display.type.rate", "zh-CN", "评分", "显示类型.评分"),
            // dict.gen.display.type.rate
            ("dict.gen.display.type.rate", "zh-HK", "评分", "显示类型.评分"),

            // dict.gen.display.type.textarea
            ("dict.gen.display.type.textarea", "en-US", "textarea", "显示类型.文本域"),
            // dict.gen.display.type.textarea
            ("dict.gen.display.type.textarea", "ja-JP", "textarea", "显示类型.文本域"),
            // dict.gen.display.type.textarea
            ("dict.gen.display.type.textarea", "zh-CN", "文本域", "显示类型.文本域"),
            // dict.gen.display.type.textarea
            ("dict.gen.display.type.textarea", "zh-HK", "文本域", "显示类型.文本域"),

            // dict.gen.display.type.editor
            ("dict.gen.display.type.editor", "en-US", "editor", "显示类型.富文本编辑器"),
            // dict.gen.display.type.editor
            ("dict.gen.display.type.editor", "ja-JP", "editor", "显示类型.富文本编辑器"),
            // dict.gen.display.type.editor
            ("dict.gen.display.type.editor", "zh-CN", "富文本编辑器", "显示类型.富文本编辑器"),
            // dict.gen.display.type.editor
            ("dict.gen.display.type.editor", "zh-HK", "富文本编辑器", "显示类型.富文本编辑器"),

            // dict.gen.frontend.form.layout.12
            ("dict.gen.frontend.form.layout.12", "en-US", "一行一列", "前端表单布局.一行一列"),
            // dict.gen.frontend.form.layout.12
            ("dict.gen.frontend.form.layout.12", "ja-JP", "一行一列", "前端表单布局.一行一列"),
            // dict.gen.frontend.form.layout.12
            ("dict.gen.frontend.form.layout.12", "zh-CN", "一行一列", "前端表单布局.一行一列"),
            // dict.gen.frontend.form.layout.12
            ("dict.gen.frontend.form.layout.12", "zh-HK", "一行一列", "前端表单布局.一行一列"),

            // dict.gen.frontend.form.layout.24
            ("dict.gen.frontend.form.layout.24", "en-US", "一行两列", "前端表单布局.一行两列"),
            // dict.gen.frontend.form.layout.24
            ("dict.gen.frontend.form.layout.24", "ja-JP", "一行两列", "前端表单布局.一行两列"),
            // dict.gen.frontend.form.layout.24
            ("dict.gen.frontend.form.layout.24", "zh-CN", "一行两列", "前端表单布局.一行两列"),
            // dict.gen.frontend.form.layout.24
            ("dict.gen.frontend.form.layout.24", "zh-HK", "一行两列", "前端表单布局.一行两列"),

            // dict.gen.frontend.ui.1
            ("dict.gen.frontend.ui.1", "en-US", "element plus", "前端ui框架.element plus"),
            // dict.gen.frontend.ui.1
            ("dict.gen.frontend.ui.1", "ja-JP", "element plus", "前端ui框架.element plus"),
            // dict.gen.frontend.ui.1
            ("dict.gen.frontend.ui.1", "zh-CN", "element plus", "前端ui框架.element plus"),
            // dict.gen.frontend.ui.1
            ("dict.gen.frontend.ui.1", "zh-HK", "element plus", "前端ui框架.element plus"),

            // dict.gen.frontend.ui.2
            ("dict.gen.frontend.ui.2", "en-US", "ant design vue", "前端ui框架.ant design vue"),
            // dict.gen.frontend.ui.2
            ("dict.gen.frontend.ui.2", "ja-JP", "ant design vue", "前端ui框架.ant design vue"),
            // dict.gen.frontend.ui.2
            ("dict.gen.frontend.ui.2", "zh-CN", "ant design vue", "前端ui框架.ant design vue"),
            // dict.gen.frontend.ui.2
            ("dict.gen.frontend.ui.2", "zh-HK", "ant design vue", "前端ui框架.ant design vue"),

            // dict.gen.function.query
            ("dict.gen.function.query", "en-US", "query", "生成功能.查询"),
            // dict.gen.function.query
            ("dict.gen.function.query", "ja-JP", "query", "生成功能.查询"),
            // dict.gen.function.query
            ("dict.gen.function.query", "zh-CN", "查询", "生成功能.查询"),
            // dict.gen.function.query
            ("dict.gen.function.query", "zh-HK", "查询", "生成功能.查询"),

            // dict.gen.function.create
            ("dict.gen.function.create", "en-US", "create", "生成功能.新增"),
            // dict.gen.function.create
            ("dict.gen.function.create", "ja-JP", "create", "生成功能.新增"),
            // dict.gen.function.create
            ("dict.gen.function.create", "zh-CN", "新增", "生成功能.新增"),
            // dict.gen.function.create
            ("dict.gen.function.create", "zh-HK", "新增", "生成功能.新增"),

            // dict.gen.function.update
            ("dict.gen.function.update", "en-US", "update", "生成功能.更新"),
            // dict.gen.function.update
            ("dict.gen.function.update", "ja-JP", "update", "生成功能.更新"),
            // dict.gen.function.update
            ("dict.gen.function.update", "zh-CN", "更新", "生成功能.更新"),
            // dict.gen.function.update
            ("dict.gen.function.update", "zh-HK", "更新", "生成功能.更新"),

            // dict.gen.function.delete
            ("dict.gen.function.delete", "en-US", "delete", "生成功能.删除"),
            // dict.gen.function.delete
            ("dict.gen.function.delete", "ja-JP", "delete", "生成功能.删除"),
            // dict.gen.function.delete
            ("dict.gen.function.delete", "zh-CN", "删除", "生成功能.删除"),
            // dict.gen.function.delete
            ("dict.gen.function.delete", "zh-HK", "删除", "生成功能.删除"),

            // dict.gen.function.status
            ("dict.gen.function.status", "en-US", "status", "生成功能.状态"),
            // dict.gen.function.status
            ("dict.gen.function.status", "ja-JP", "status", "生成功能.状态"),
            // dict.gen.function.status
            ("dict.gen.function.status", "zh-CN", "状态", "生成功能.状态"),
            // dict.gen.function.status
            ("dict.gen.function.status", "zh-HK", "状态", "生成功能.状态"),

            // dict.gen.function.sort
            ("dict.gen.function.sort", "en-US", "sort", "生成功能.排序"),
            // dict.gen.function.sort
            ("dict.gen.function.sort", "ja-JP", "sort", "生成功能.排序"),
            // dict.gen.function.sort
            ("dict.gen.function.sort", "zh-CN", "排序", "生成功能.排序"),
            // dict.gen.function.sort
            ("dict.gen.function.sort", "zh-HK", "排序", "生成功能.排序"),

            // dict.gen.function.template
            ("dict.gen.function.template", "en-US", "template", "生成功能.模板"),
            // dict.gen.function.template
            ("dict.gen.function.template", "ja-JP", "template", "生成功能.模板"),
            // dict.gen.function.template
            ("dict.gen.function.template", "zh-CN", "模板", "生成功能.模板"),
            // dict.gen.function.template
            ("dict.gen.function.template", "zh-HK", "模板", "生成功能.模板"),

            // dict.gen.function.import
            ("dict.gen.function.import", "en-US", "import", "生成功能.导入"),
            // dict.gen.function.import
            ("dict.gen.function.import", "ja-JP", "import", "生成功能.导入"),
            // dict.gen.function.import
            ("dict.gen.function.import", "zh-CN", "导入", "生成功能.导入"),
            // dict.gen.function.import
            ("dict.gen.function.import", "zh-HK", "导入", "生成功能.导入"),

            // dict.gen.function.export
            ("dict.gen.function.export", "en-US", "export", "生成功能.导出"),
            // dict.gen.function.export
            ("dict.gen.function.export", "ja-JP", "export", "生成功能.导出"),
            // dict.gen.function.export
            ("dict.gen.function.export", "zh-CN", "导出", "生成功能.导出"),
            // dict.gen.function.export
            ("dict.gen.function.export", "zh-HK", "导出", "生成功能.导出"),

            // dict.gen.method.0
            ("dict.gen.method.0", "en-US", "zip 压缩包", "生成方式.zip 压缩包"),
            // dict.gen.method.0
            ("dict.gen.method.0", "ja-JP", "zip 压缩包", "生成方式.zip 压缩包"),
            // dict.gen.method.0
            ("dict.gen.method.0", "zh-CN", "zip 压缩包", "生成方式.zip 压缩包"),
            // dict.gen.method.0
            ("dict.gen.method.0", "zh-HK", "zip 压缩包", "生成方式.zip 压缩包"),

            // dict.gen.method.1
            ("dict.gen.method.1", "en-US", "自定义路径", "生成方式.自定义路径"),
            // dict.gen.method.1
            ("dict.gen.method.1", "ja-JP", "自定义路径", "生成方式.自定义路径"),
            // dict.gen.method.1
            ("dict.gen.method.1", "zh-CN", "自定义路径", "生成方式.自定义路径"),
            // dict.gen.method.1
            ("dict.gen.method.1", "zh-HK", "自定义路径", "生成方式.自定义路径"),

            // dict.gen.method.2
            ("dict.gen.method.2", "en-US", "当前项目", "生成方式.当前项目"),
            // dict.gen.method.2
            ("dict.gen.method.2", "ja-JP", "当前项目", "生成方式.当前项目"),
            // dict.gen.method.2
            ("dict.gen.method.2", "zh-CN", "当前项目", "生成方式.当前项目"),
            // dict.gen.method.2
            ("dict.gen.method.2", "zh-HK", "当前项目", "生成方式.当前项目"),

            // dict.gen.query.type.eq
            ("dict.gen.query.type.eq", "en-US", "eq", "查询方式.等于"),
            // dict.gen.query.type.eq
            ("dict.gen.query.type.eq", "ja-JP", "eq", "查询方式.等于"),
            // dict.gen.query.type.eq
            ("dict.gen.query.type.eq", "zh-CN", "等于", "查询方式.等于"),
            // dict.gen.query.type.eq
            ("dict.gen.query.type.eq", "zh-HK", "等于", "查询方式.等于"),

            // dict.gen.query.type.ne
            ("dict.gen.query.type.ne", "en-US", "ne", "查询方式.不等于"),
            // dict.gen.query.type.ne
            ("dict.gen.query.type.ne", "ja-JP", "ne", "查询方式.不等于"),
            // dict.gen.query.type.ne
            ("dict.gen.query.type.ne", "zh-CN", "不等于", "查询方式.不等于"),
            // dict.gen.query.type.ne
            ("dict.gen.query.type.ne", "zh-HK", "不等于", "查询方式.不等于"),

            // dict.gen.query.type.gt
            ("dict.gen.query.type.gt", "en-US", "gt", "查询方式.大于"),
            // dict.gen.query.type.gt
            ("dict.gen.query.type.gt", "ja-JP", "gt", "查询方式.大于"),
            // dict.gen.query.type.gt
            ("dict.gen.query.type.gt", "zh-CN", "大于", "查询方式.大于"),
            // dict.gen.query.type.gt
            ("dict.gen.query.type.gt", "zh-HK", "大于", "查询方式.大于"),

            // dict.gen.query.type.gte
            ("dict.gen.query.type.gte", "en-US", "gte", "查询方式.大于等于"),
            // dict.gen.query.type.gte
            ("dict.gen.query.type.gte", "ja-JP", "gte", "查询方式.大于等于"),
            // dict.gen.query.type.gte
            ("dict.gen.query.type.gte", "zh-CN", "大于等于", "查询方式.大于等于"),
            // dict.gen.query.type.gte
            ("dict.gen.query.type.gte", "zh-HK", "大于等于", "查询方式.大于等于"),

            // dict.gen.query.type.lt
            ("dict.gen.query.type.lt", "en-US", "lt", "查询方式.小于"),
            // dict.gen.query.type.lt
            ("dict.gen.query.type.lt", "ja-JP", "lt", "查询方式.小于"),
            // dict.gen.query.type.lt
            ("dict.gen.query.type.lt", "zh-CN", "小于", "查询方式.小于"),
            // dict.gen.query.type.lt
            ("dict.gen.query.type.lt", "zh-HK", "小于", "查询方式.小于"),

            // dict.gen.query.type.lte
            ("dict.gen.query.type.lte", "en-US", "lte", "查询方式.小于等于"),
            // dict.gen.query.type.lte
            ("dict.gen.query.type.lte", "ja-JP", "lte", "查询方式.小于等于"),
            // dict.gen.query.type.lte
            ("dict.gen.query.type.lte", "zh-CN", "小于等于", "查询方式.小于等于"),
            // dict.gen.query.type.lte
            ("dict.gen.query.type.lte", "zh-HK", "小于等于", "查询方式.小于等于"),

            // dict.gen.query.type.like
            ("dict.gen.query.type.like", "en-US", "like", "查询方式.模糊"),
            // dict.gen.query.type.like
            ("dict.gen.query.type.like", "ja-JP", "like", "查询方式.模糊"),
            // dict.gen.query.type.like
            ("dict.gen.query.type.like", "zh-CN", "模糊", "查询方式.模糊"),
            // dict.gen.query.type.like
            ("dict.gen.query.type.like", "zh-HK", "模糊", "查询方式.模糊"),

            // dict.gen.query.type.between
            ("dict.gen.query.type.between", "en-US", "between", "查询方式.范围"),
            // dict.gen.query.type.between
            ("dict.gen.query.type.between", "ja-JP", "between", "查询方式.范围"),
            // dict.gen.query.type.between
            ("dict.gen.query.type.between", "zh-CN", "范围", "查询方式.范围"),
            // dict.gen.query.type.between
            ("dict.gen.query.type.between", "zh-HK", "范围", "查询方式.范围"),

            // dict.gen.template.type.crud
            ("dict.gen.template.type.crud", "en-US", "crud", "生成模板类型.单表操作"),
            // dict.gen.template.type.crud
            ("dict.gen.template.type.crud", "ja-JP", "crud", "生成模板类型.单表操作"),
            // dict.gen.template.type.crud
            ("dict.gen.template.type.crud", "zh-CN", "单表操作", "生成模板类型.单表操作"),
            // dict.gen.template.type.crud
            ("dict.gen.template.type.crud", "zh-HK", "单表操作", "生成模板类型.单表操作"),

            // dict.gen.template.type.tree
            ("dict.gen.template.type.tree", "en-US", "tree", "生成模板类型.树表操作"),
            // dict.gen.template.type.tree
            ("dict.gen.template.type.tree", "ja-JP", "tree", "生成模板类型.树表操作"),
            // dict.gen.template.type.tree
            ("dict.gen.template.type.tree", "zh-CN", "树表操作", "生成模板类型.树表操作"),
            // dict.gen.template.type.tree
            ("dict.gen.template.type.tree", "zh-HK", "树表操作", "生成模板类型.树表操作"),

            // dict.gen.template.type.sub
            ("dict.gen.template.type.sub", "en-US", "sub", "生成模板类型.主子表操作"),
            // dict.gen.template.type.sub
            ("dict.gen.template.type.sub", "ja-JP", "sub", "生成模板类型.主子表操作"),
            // dict.gen.template.type.sub
            ("dict.gen.template.type.sub", "zh-CN", "主子表操作", "生成模板类型.主子表操作"),
            // dict.gen.template.type.sub
            ("dict.gen.template.type.sub", "zh-HK", "主子表操作", "生成模板类型.主子表操作"),

            // dict.hr.attendance.correction.approval.0
            ("dict.hr.attendance.correction.approval.0", "en-US", "草稿", "补卡审批状态.草稿"),
            // dict.hr.attendance.correction.approval.0
            ("dict.hr.attendance.correction.approval.0", "ja-JP", "草稿", "补卡审批状态.草稿"),
            // dict.hr.attendance.correction.approval.0
            ("dict.hr.attendance.correction.approval.0", "zh-CN", "草稿", "补卡审批状态.草稿"),
            // dict.hr.attendance.correction.approval.0
            ("dict.hr.attendance.correction.approval.0", "zh-HK", "草稿", "补卡审批状态.草稿"),

            // dict.hr.attendance.correction.approval.1
            ("dict.hr.attendance.correction.approval.1", "en-US", "待审", "补卡审批状态.待审"),
            // dict.hr.attendance.correction.approval.1
            ("dict.hr.attendance.correction.approval.1", "ja-JP", "待审", "补卡审批状态.待审"),
            // dict.hr.attendance.correction.approval.1
            ("dict.hr.attendance.correction.approval.1", "zh-CN", "待审", "补卡审批状态.待审"),
            // dict.hr.attendance.correction.approval.1
            ("dict.hr.attendance.correction.approval.1", "zh-HK", "待审", "补卡审批状态.待审"),

            // dict.hr.attendance.correction.approval.2
            ("dict.hr.attendance.correction.approval.2", "en-US", "已通过", "补卡审批状态.已通过"),
            // dict.hr.attendance.correction.approval.2
            ("dict.hr.attendance.correction.approval.2", "ja-JP", "已通过", "补卡审批状态.已通过"),
            // dict.hr.attendance.correction.approval.2
            ("dict.hr.attendance.correction.approval.2", "zh-CN", "已通过", "补卡审批状态.已通过"),
            // dict.hr.attendance.correction.approval.2
            ("dict.hr.attendance.correction.approval.2", "zh-HK", "已通过", "补卡审批状态.已通过"),

            // dict.hr.attendance.correction.approval.3
            ("dict.hr.attendance.correction.approval.3", "en-US", "已驳回", "补卡审批状态.已驳回"),
            // dict.hr.attendance.correction.approval.3
            ("dict.hr.attendance.correction.approval.3", "ja-JP", "已驳回", "补卡审批状态.已驳回"),
            // dict.hr.attendance.correction.approval.3
            ("dict.hr.attendance.correction.approval.3", "zh-CN", "已驳回", "补卡审批状态.已驳回"),
            // dict.hr.attendance.correction.approval.3
            ("dict.hr.attendance.correction.approval.3", "zh-HK", "已驳回", "补卡审批状态.已驳回"),

            // dict.hr.attendance.correction.kind.1
            ("dict.hr.attendance.correction.kind.1", "en-US", "上班", "补卡类型.上班"),
            // dict.hr.attendance.correction.kind.1
            ("dict.hr.attendance.correction.kind.1", "ja-JP", "上班", "补卡类型.上班"),
            // dict.hr.attendance.correction.kind.1
            ("dict.hr.attendance.correction.kind.1", "zh-CN", "上班", "补卡类型.上班"),
            // dict.hr.attendance.correction.kind.1
            ("dict.hr.attendance.correction.kind.1", "zh-HK", "上班", "补卡类型.上班"),

            // dict.hr.attendance.correction.kind.2
            ("dict.hr.attendance.correction.kind.2", "en-US", "下班", "补卡类型.下班"),
            // dict.hr.attendance.correction.kind.2
            ("dict.hr.attendance.correction.kind.2", "ja-JP", "下班", "补卡类型.下班"),
            // dict.hr.attendance.correction.kind.2
            ("dict.hr.attendance.correction.kind.2", "zh-CN", "下班", "补卡类型.下班"),
            // dict.hr.attendance.correction.kind.2
            ("dict.hr.attendance.correction.kind.2", "zh-HK", "下班", "补卡类型.下班"),

            // dict.hr.attendance.device.brand.hikvision
            ("dict.hr.attendance.device.brand.hikvision", "en-US", "hikvision", "考勤设备品牌.海康威视"),
            // dict.hr.attendance.device.brand.hikvision
            ("dict.hr.attendance.device.brand.hikvision", "ja-JP", "hikvision", "考勤设备品牌.海康威视"),
            // dict.hr.attendance.device.brand.hikvision
            ("dict.hr.attendance.device.brand.hikvision", "zh-CN", "海康威视", "考勤设备品牌.海康威视"),
            // dict.hr.attendance.device.brand.hikvision
            ("dict.hr.attendance.device.brand.hikvision", "zh-HK", "海康威视", "考勤设备品牌.海康威视"),

            // dict.hr.attendance.device.brand.deli
            ("dict.hr.attendance.device.brand.deli", "en-US", "deli", "考勤设备品牌.得力"),
            // dict.hr.attendance.device.brand.deli
            ("dict.hr.attendance.device.brand.deli", "ja-JP", "deli", "考勤设备品牌.得力"),
            // dict.hr.attendance.device.brand.deli
            ("dict.hr.attendance.device.brand.deli", "zh-CN", "得力", "考勤设备品牌.得力"),
            // dict.hr.attendance.device.brand.deli
            ("dict.hr.attendance.device.brand.deli", "zh-HK", "得力", "考勤设备品牌.得力"),

            // dict.hr.attendance.device.brand.zkteco
            ("dict.hr.attendance.device.brand.zkteco", "en-US", "zkteco", "考勤设备品牌.中控"),
            // dict.hr.attendance.device.brand.zkteco
            ("dict.hr.attendance.device.brand.zkteco", "ja-JP", "zkteco", "考勤设备品牌.中控"),
            // dict.hr.attendance.device.brand.zkteco
            ("dict.hr.attendance.device.brand.zkteco", "zh-CN", "中控", "考勤设备品牌.中控"),
            // dict.hr.attendance.device.brand.zkteco
            ("dict.hr.attendance.device.brand.zkteco", "zh-HK", "中控", "考勤设备品牌.中控"),

            // dict.hr.attendance.device.status.0
            ("dict.hr.attendance.device.status.0", "en-US", "停用", "考勤设备状态.停用"),
            // dict.hr.attendance.device.status.0
            ("dict.hr.attendance.device.status.0", "ja-JP", "停用", "考勤设备状态.停用"),
            // dict.hr.attendance.device.status.0
            ("dict.hr.attendance.device.status.0", "zh-CN", "停用", "考勤设备状态.停用"),
            // dict.hr.attendance.device.status.0
            ("dict.hr.attendance.device.status.0", "zh-HK", "停用", "考勤设备状态.停用"),

            // dict.hr.attendance.device.status.1
            ("dict.hr.attendance.device.status.1", "en-US", "正常", "考勤设备状态.正常"),
            // dict.hr.attendance.device.status.1
            ("dict.hr.attendance.device.status.1", "ja-JP", "正常", "考勤设备状态.正常"),
            // dict.hr.attendance.device.status.1
            ("dict.hr.attendance.device.status.1", "zh-CN", "正常", "考勤设备状态.正常"),
            // dict.hr.attendance.device.status.1
            ("dict.hr.attendance.device.status.1", "zh-HK", "正常", "考勤设备状态.正常"),

            // dict.hr.attendance.device.status.2
            ("dict.hr.attendance.device.status.2", "en-US", "故障", "考勤设备状态.故障"),
            // dict.hr.attendance.device.status.2
            ("dict.hr.attendance.device.status.2", "ja-JP", "故障", "考勤设备状态.故障"),
            // dict.hr.attendance.device.status.2
            ("dict.hr.attendance.device.status.2", "zh-CN", "故障", "考勤设备状态.故障"),
            // dict.hr.attendance.device.status.2
            ("dict.hr.attendance.device.status.2", "zh-HK", "故障", "考勤设备状态.故障"),

            // dict.hr.attendance.exception.handle.status.0
            ("dict.hr.attendance.exception.handle.status.0", "en-US", "待处理", "考勤异常处理状态.待处理"),
            // dict.hr.attendance.exception.handle.status.0
            ("dict.hr.attendance.exception.handle.status.0", "ja-JP", "待处理", "考勤异常处理状态.待处理"),
            // dict.hr.attendance.exception.handle.status.0
            ("dict.hr.attendance.exception.handle.status.0", "zh-CN", "待处理", "考勤异常处理状态.待处理"),
            // dict.hr.attendance.exception.handle.status.0
            ("dict.hr.attendance.exception.handle.status.0", "zh-HK", "待处理", "考勤异常处理状态.待处理"),

            // dict.hr.attendance.exception.handle.status.1
            ("dict.hr.attendance.exception.handle.status.1", "en-US", "已处理", "考勤异常处理状态.已处理"),
            // dict.hr.attendance.exception.handle.status.1
            ("dict.hr.attendance.exception.handle.status.1", "ja-JP", "已处理", "考勤异常处理状态.已处理"),
            // dict.hr.attendance.exception.handle.status.1
            ("dict.hr.attendance.exception.handle.status.1", "zh-CN", "已处理", "考勤异常处理状态.已处理"),
            // dict.hr.attendance.exception.handle.status.1
            ("dict.hr.attendance.exception.handle.status.1", "zh-HK", "已处理", "考勤异常处理状态.已处理"),

            // dict.hr.attendance.exception.handle.status.2
            ("dict.hr.attendance.exception.handle.status.2", "en-US", "已忽略", "考勤异常处理状态.已忽略"),
            // dict.hr.attendance.exception.handle.status.2
            ("dict.hr.attendance.exception.handle.status.2", "ja-JP", "已忽略", "考勤异常处理状态.已忽略"),
            // dict.hr.attendance.exception.handle.status.2
            ("dict.hr.attendance.exception.handle.status.2", "zh-CN", "已忽略", "考勤异常处理状态.已忽略"),
            // dict.hr.attendance.exception.handle.status.2
            ("dict.hr.attendance.exception.handle.status.2", "zh-HK", "已忽略", "考勤异常处理状态.已忽略"),

            // dict.hr.attendance.exception.type.1
            ("dict.hr.attendance.exception.type.1", "en-US", "上班缺卡", "考勤异常类型.上班缺卡"),
            // dict.hr.attendance.exception.type.1
            ("dict.hr.attendance.exception.type.1", "ja-JP", "上班缺卡", "考勤异常类型.上班缺卡"),
            // dict.hr.attendance.exception.type.1
            ("dict.hr.attendance.exception.type.1", "zh-CN", "上班缺卡", "考勤异常类型.上班缺卡"),
            // dict.hr.attendance.exception.type.1
            ("dict.hr.attendance.exception.type.1", "zh-HK", "上班缺卡", "考勤异常类型.上班缺卡"),

            // dict.hr.attendance.exception.type.2
            ("dict.hr.attendance.exception.type.2", "en-US", "下班缺卡", "考勤异常类型.下班缺卡"),
            // dict.hr.attendance.exception.type.2
            ("dict.hr.attendance.exception.type.2", "ja-JP", "下班缺卡", "考勤异常类型.下班缺卡"),
            // dict.hr.attendance.exception.type.2
            ("dict.hr.attendance.exception.type.2", "zh-CN", "下班缺卡", "考勤异常类型.下班缺卡"),
            // dict.hr.attendance.exception.type.2
            ("dict.hr.attendance.exception.type.2", "zh-HK", "下班缺卡", "考勤异常类型.下班缺卡"),

            // dict.hr.attendance.exception.type.3
            ("dict.hr.attendance.exception.type.3", "en-US", "迟到", "考勤异常类型.迟到"),
            // dict.hr.attendance.exception.type.3
            ("dict.hr.attendance.exception.type.3", "ja-JP", "迟到", "考勤异常类型.迟到"),
            // dict.hr.attendance.exception.type.3
            ("dict.hr.attendance.exception.type.3", "zh-CN", "迟到", "考勤异常类型.迟到"),
            // dict.hr.attendance.exception.type.3
            ("dict.hr.attendance.exception.type.3", "zh-HK", "迟到", "考勤异常类型.迟到"),

            // dict.hr.attendance.exception.type.4
            ("dict.hr.attendance.exception.type.4", "en-US", "早退", "考勤异常类型.早退"),
            // dict.hr.attendance.exception.type.4
            ("dict.hr.attendance.exception.type.4", "ja-JP", "早退", "考勤异常类型.早退"),
            // dict.hr.attendance.exception.type.4
            ("dict.hr.attendance.exception.type.4", "zh-CN", "早退", "考勤异常类型.早退"),
            // dict.hr.attendance.exception.type.4
            ("dict.hr.attendance.exception.type.4", "zh-HK", "早退", "考勤异常类型.早退"),

            // dict.hr.attendance.exception.type.5
            ("dict.hr.attendance.exception.type.5", "en-US", "旷工", "考勤异常类型.旷工"),
            // dict.hr.attendance.exception.type.5
            ("dict.hr.attendance.exception.type.5", "ja-JP", "旷工", "考勤异常类型.旷工"),
            // dict.hr.attendance.exception.type.5
            ("dict.hr.attendance.exception.type.5", "zh-CN", "旷工", "考勤异常类型.旷工"),
            // dict.hr.attendance.exception.type.5
            ("dict.hr.attendance.exception.type.5", "zh-HK", "旷工", "考勤异常类型.旷工"),

            // dict.hr.attendance.exception.type.9
            ("dict.hr.attendance.exception.type.9", "en-US", "其他", "考勤异常类型.其他"),
            // dict.hr.attendance.exception.type.9
            ("dict.hr.attendance.exception.type.9", "ja-JP", "其他", "考勤异常类型.其他"),
            // dict.hr.attendance.exception.type.9
            ("dict.hr.attendance.exception.type.9", "zh-CN", "其他", "考勤异常类型.其他"),
            // dict.hr.attendance.exception.type.9
            ("dict.hr.attendance.exception.type.9", "zh-HK", "其他", "考勤异常类型.其他"),

            // dict.hr.attendance.punch.source.0
            ("dict.hr.attendance.punch.source.0", "en-US", "后台录入", "打卡来源.后台录入"),
            // dict.hr.attendance.punch.source.0
            ("dict.hr.attendance.punch.source.0", "ja-JP", "后台录入", "打卡来源.后台录入"),
            // dict.hr.attendance.punch.source.0
            ("dict.hr.attendance.punch.source.0", "zh-CN", "后台录入", "打卡来源.后台录入"),
            // dict.hr.attendance.punch.source.0
            ("dict.hr.attendance.punch.source.0", "zh-HK", "后台录入", "打卡来源.后台录入"),

            // dict.hr.attendance.punch.source.1
            ("dict.hr.attendance.punch.source.1", "en-US", "移动端", "打卡来源.移动端"),
            // dict.hr.attendance.punch.source.1
            ("dict.hr.attendance.punch.source.1", "ja-JP", "移动端", "打卡来源.移动端"),
            // dict.hr.attendance.punch.source.1
            ("dict.hr.attendance.punch.source.1", "zh-CN", "移动端", "打卡来源.移动端"),
            // dict.hr.attendance.punch.source.1
            ("dict.hr.attendance.punch.source.1", "zh-HK", "移动端", "打卡来源.移动端"),

            // dict.hr.attendance.punch.source.2
            ("dict.hr.attendance.punch.source.2", "en-US", "导入", "打卡来源.导入"),
            // dict.hr.attendance.punch.source.2
            ("dict.hr.attendance.punch.source.2", "ja-JP", "导入", "打卡来源.导入"),
            // dict.hr.attendance.punch.source.2
            ("dict.hr.attendance.punch.source.2", "zh-CN", "导入", "打卡来源.导入"),
            // dict.hr.attendance.punch.source.2
            ("dict.hr.attendance.punch.source.2", "zh-HK", "导入", "打卡来源.导入"),

            // dict.hr.attendance.punch.type.1
            ("dict.hr.attendance.punch.type.1", "en-US", "上班", "打卡类型.上班"),
            // dict.hr.attendance.punch.type.1
            ("dict.hr.attendance.punch.type.1", "ja-JP", "上班", "打卡类型.上班"),
            // dict.hr.attendance.punch.type.1
            ("dict.hr.attendance.punch.type.1", "zh-CN", "上班", "打卡类型.上班"),
            // dict.hr.attendance.punch.type.1
            ("dict.hr.attendance.punch.type.1", "zh-HK", "上班", "打卡类型.上班"),

            // dict.hr.attendance.punch.type.2
            ("dict.hr.attendance.punch.type.2", "en-US", "下班", "打卡类型.下班"),
            // dict.hr.attendance.punch.type.2
            ("dict.hr.attendance.punch.type.2", "ja-JP", "下班", "打卡类型.下班"),
            // dict.hr.attendance.punch.type.2
            ("dict.hr.attendance.punch.type.2", "zh-CN", "下班", "打卡类型.下班"),
            // dict.hr.attendance.punch.type.2
            ("dict.hr.attendance.punch.type.2", "zh-HK", "下班", "打卡类型.下班"),

            // dict.hr.attendance.punch.type.3
            ("dict.hr.attendance.punch.type.3", "en-US", "外勤", "打卡类型.外勤"),
            // dict.hr.attendance.punch.type.3
            ("dict.hr.attendance.punch.type.3", "ja-JP", "外勤", "打卡类型.外勤"),
            // dict.hr.attendance.punch.type.3
            ("dict.hr.attendance.punch.type.3", "zh-CN", "外勤", "打卡类型.外勤"),
            // dict.hr.attendance.punch.type.3
            ("dict.hr.attendance.punch.type.3", "zh-HK", "外勤", "打卡类型.外勤"),

            // dict.hr.attendance.result.status.0
            ("dict.hr.attendance.result.status.0", "en-US", "正常", "出勤状态.正常"),
            // dict.hr.attendance.result.status.0
            ("dict.hr.attendance.result.status.0", "ja-JP", "正常", "出勤状态.正常"),
            // dict.hr.attendance.result.status.0
            ("dict.hr.attendance.result.status.0", "zh-CN", "正常", "出勤状态.正常"),
            // dict.hr.attendance.result.status.0
            ("dict.hr.attendance.result.status.0", "zh-HK", "正常", "出勤状态.正常"),

            // dict.hr.attendance.result.status.1
            ("dict.hr.attendance.result.status.1", "en-US", "迟到", "出勤状态.迟到"),
            // dict.hr.attendance.result.status.1
            ("dict.hr.attendance.result.status.1", "ja-JP", "迟到", "出勤状态.迟到"),
            // dict.hr.attendance.result.status.1
            ("dict.hr.attendance.result.status.1", "zh-CN", "迟到", "出勤状态.迟到"),
            // dict.hr.attendance.result.status.1
            ("dict.hr.attendance.result.status.1", "zh-HK", "迟到", "出勤状态.迟到"),

            // dict.hr.attendance.result.status.2
            ("dict.hr.attendance.result.status.2", "en-US", "早退", "出勤状态.早退"),
            // dict.hr.attendance.result.status.2
            ("dict.hr.attendance.result.status.2", "ja-JP", "早退", "出勤状态.早退"),
            // dict.hr.attendance.result.status.2
            ("dict.hr.attendance.result.status.2", "zh-CN", "早退", "出勤状态.早退"),
            // dict.hr.attendance.result.status.2
            ("dict.hr.attendance.result.status.2", "zh-HK", "早退", "出勤状态.早退"),

            // dict.hr.attendance.result.status.3
            ("dict.hr.attendance.result.status.3", "en-US", "缺卡", "出勤状态.缺卡"),
            // dict.hr.attendance.result.status.3
            ("dict.hr.attendance.result.status.3", "ja-JP", "缺卡", "出勤状态.缺卡"),
            // dict.hr.attendance.result.status.3
            ("dict.hr.attendance.result.status.3", "zh-CN", "缺卡", "出勤状态.缺卡"),
            // dict.hr.attendance.result.status.3
            ("dict.hr.attendance.result.status.3", "zh-HK", "缺卡", "出勤状态.缺卡"),

            // dict.hr.attendance.result.status.4
            ("dict.hr.attendance.result.status.4", "en-US", "旷工", "出勤状态.旷工"),
            // dict.hr.attendance.result.status.4
            ("dict.hr.attendance.result.status.4", "ja-JP", "旷工", "出勤状态.旷工"),
            // dict.hr.attendance.result.status.4
            ("dict.hr.attendance.result.status.4", "zh-CN", "旷工", "出勤状态.旷工"),
            // dict.hr.attendance.result.status.4
            ("dict.hr.attendance.result.status.4", "zh-HK", "旷工", "出勤状态.旷工"),

            // dict.hr.attendance.result.status.5
            ("dict.hr.attendance.result.status.5", "en-US", "加班", "出勤状态.加班"),
            // dict.hr.attendance.result.status.5
            ("dict.hr.attendance.result.status.5", "ja-JP", "加班", "出勤状态.加班"),
            // dict.hr.attendance.result.status.5
            ("dict.hr.attendance.result.status.5", "zh-CN", "加班", "出勤状态.加班"),
            // dict.hr.attendance.result.status.5
            ("dict.hr.attendance.result.status.5", "zh-HK", "加班", "出勤状态.加班"),

            // dict.hr.attendance.verify.mode.0
            ("dict.hr.attendance.verify.mode.0", "en-US", "未知", "考勤验证方式.未知"),
            // dict.hr.attendance.verify.mode.0
            ("dict.hr.attendance.verify.mode.0", "ja-JP", "未知", "考勤验证方式.未知"),
            // dict.hr.attendance.verify.mode.0
            ("dict.hr.attendance.verify.mode.0", "zh-CN", "未知", "考勤验证方式.未知"),
            // dict.hr.attendance.verify.mode.0
            ("dict.hr.attendance.verify.mode.0", "zh-HK", "未知", "考勤验证方式.未知"),

            // dict.hr.attendance.verify.mode.1
            ("dict.hr.attendance.verify.mode.1", "en-US", "指纹", "考勤验证方式.指纹"),
            // dict.hr.attendance.verify.mode.1
            ("dict.hr.attendance.verify.mode.1", "ja-JP", "指纹", "考勤验证方式.指纹"),
            // dict.hr.attendance.verify.mode.1
            ("dict.hr.attendance.verify.mode.1", "zh-CN", "指纹", "考勤验证方式.指纹"),
            // dict.hr.attendance.verify.mode.1
            ("dict.hr.attendance.verify.mode.1", "zh-HK", "指纹", "考勤验证方式.指纹"),

            // dict.hr.attendance.verify.mode.2
            ("dict.hr.attendance.verify.mode.2", "en-US", "人脸", "考勤验证方式.人脸"),
            // dict.hr.attendance.verify.mode.2
            ("dict.hr.attendance.verify.mode.2", "ja-JP", "人脸", "考勤验证方式.人脸"),
            // dict.hr.attendance.verify.mode.2
            ("dict.hr.attendance.verify.mode.2", "zh-CN", "人脸", "考勤验证方式.人脸"),
            // dict.hr.attendance.verify.mode.2
            ("dict.hr.attendance.verify.mode.2", "zh-HK", "人脸", "考勤验证方式.人脸"),

            // dict.hr.attendance.verify.mode.3
            ("dict.hr.attendance.verify.mode.3", "en-US", "密码", "考勤验证方式.密码"),
            // dict.hr.attendance.verify.mode.3
            ("dict.hr.attendance.verify.mode.3", "ja-JP", "密码", "考勤验证方式.密码"),
            // dict.hr.attendance.verify.mode.3
            ("dict.hr.attendance.verify.mode.3", "zh-CN", "密码", "考勤验证方式.密码"),
            // dict.hr.attendance.verify.mode.3
            ("dict.hr.attendance.verify.mode.3", "zh-HK", "密码", "考勤验证方式.密码"),

            // dict.hr.attendance.verify.mode.4
            ("dict.hr.attendance.verify.mode.4", "en-US", "卡", "考勤验证方式.卡"),
            // dict.hr.attendance.verify.mode.4
            ("dict.hr.attendance.verify.mode.4", "ja-JP", "卡", "考勤验证方式.卡"),
            // dict.hr.attendance.verify.mode.4
            ("dict.hr.attendance.verify.mode.4", "zh-CN", "卡", "考勤验证方式.卡"),
            // dict.hr.attendance.verify.mode.4
            ("dict.hr.attendance.verify.mode.4", "zh-HK", "卡", "考勤验证方式.卡"),

            // dict.hr.delegate.mode.0
            ("dict.hr.delegate.mode.0", "en-US", "直接员工", "人事代理模式.直接员工"),
            // dict.hr.delegate.mode.0
            ("dict.hr.delegate.mode.0", "ja-JP", "直接员工", "人事代理模式.直接员工"),
            // dict.hr.delegate.mode.0
            ("dict.hr.delegate.mode.0", "zh-CN", "直接员工", "人事代理模式.直接员工"),
            // dict.hr.delegate.mode.0
            ("dict.hr.delegate.mode.0", "zh-HK", "直接员工", "人事代理模式.直接员工"),

            // dict.hr.delegate.mode.1
            ("dict.hr.delegate.mode.1", "en-US", "部门规则", "人事代理模式.部门规则"),
            // dict.hr.delegate.mode.1
            ("dict.hr.delegate.mode.1", "ja-JP", "部门规则", "人事代理模式.部门规则"),
            // dict.hr.delegate.mode.1
            ("dict.hr.delegate.mode.1", "zh-CN", "部门规则", "人事代理模式.部门规则"),
            // dict.hr.delegate.mode.1
            ("dict.hr.delegate.mode.1", "zh-HK", "部门规则", "人事代理模式.部门规则"),

            // dict.hr.delegate.mode.2
            ("dict.hr.delegate.mode.2", "en-US", "岗位规则", "人事代理模式.岗位规则"),
            // dict.hr.delegate.mode.2
            ("dict.hr.delegate.mode.2", "ja-JP", "岗位规则", "人事代理模式.岗位规则"),
            // dict.hr.delegate.mode.2
            ("dict.hr.delegate.mode.2", "zh-CN", "岗位规则", "人事代理模式.岗位规则"),
            // dict.hr.delegate.mode.2
            ("dict.hr.delegate.mode.2", "zh-HK", "岗位规则", "人事代理模式.岗位规则"),

            // dict.hr.employee.status.0
            ("dict.hr.employee.status.0", "en-US", "在职", "员工状态.在职"),
            // dict.hr.employee.status.0
            ("dict.hr.employee.status.0", "ja-JP", "在职", "员工状态.在职"),
            // dict.hr.employee.status.0
            ("dict.hr.employee.status.0", "zh-CN", "在职", "员工状态.在职"),
            // dict.hr.employee.status.0
            ("dict.hr.employee.status.0", "zh-HK", "在职", "员工状态.在职"),

            // dict.hr.employee.status.1
            ("dict.hr.employee.status.1", "en-US", "离职", "员工状态.离职"),
            // dict.hr.employee.status.1
            ("dict.hr.employee.status.1", "ja-JP", "离职", "员工状态.离职"),
            // dict.hr.employee.status.1
            ("dict.hr.employee.status.1", "zh-CN", "离职", "员工状态.离职"),
            // dict.hr.employee.status.1
            ("dict.hr.employee.status.1", "zh-HK", "离职", "员工状态.离职"),

            // dict.hr.employee.status.2
            ("dict.hr.employee.status.2", "en-US", "停薪留职", "员工状态.停薪留职"),
            // dict.hr.employee.status.2
            ("dict.hr.employee.status.2", "ja-JP", "停薪留职", "员工状态.停薪留职"),
            // dict.hr.employee.status.2
            ("dict.hr.employee.status.2", "zh-CN", "停薪留职", "员工状态.停薪留职"),
            // dict.hr.employee.status.2
            ("dict.hr.employee.status.2", "zh-HK", "停薪留职", "员工状态.停薪留职"),

            // dict.hr.employee.status.3
            ("dict.hr.employee.status.3", "en-US", "退休", "员工状态.退休"),
            // dict.hr.employee.status.3
            ("dict.hr.employee.status.3", "ja-JP", "退休", "员工状态.退休"),
            // dict.hr.employee.status.3
            ("dict.hr.employee.status.3", "zh-CN", "退休", "员工状态.退休"),
            // dict.hr.employee.status.3
            ("dict.hr.employee.status.3", "zh-HK", "退休", "员工状态.退休"),

            // dict.hr.ethnic.group.1
            ("dict.hr.ethnic.group.1", "en-US", "汉族", "民族.汉族"),
            // dict.hr.ethnic.group.1
            ("dict.hr.ethnic.group.1", "ja-JP", "汉族", "民族.汉族"),
            // dict.hr.ethnic.group.1
            ("dict.hr.ethnic.group.1", "zh-CN", "汉族", "民族.汉族"),
            // dict.hr.ethnic.group.1
            ("dict.hr.ethnic.group.1", "zh-HK", "汉族", "民族.汉族"),

            // dict.hr.ethnic.group.2
            ("dict.hr.ethnic.group.2", "en-US", "蒙古族", "民族.蒙古族"),
            // dict.hr.ethnic.group.2
            ("dict.hr.ethnic.group.2", "ja-JP", "蒙古族", "民族.蒙古族"),
            // dict.hr.ethnic.group.2
            ("dict.hr.ethnic.group.2", "zh-CN", "蒙古族", "民族.蒙古族"),
            // dict.hr.ethnic.group.2
            ("dict.hr.ethnic.group.2", "zh-HK", "蒙古族", "民族.蒙古族"),

            // dict.hr.ethnic.group.3
            ("dict.hr.ethnic.group.3", "en-US", "回族", "民族.回族"),
            // dict.hr.ethnic.group.3
            ("dict.hr.ethnic.group.3", "ja-JP", "回族", "民族.回族"),
            // dict.hr.ethnic.group.3
            ("dict.hr.ethnic.group.3", "zh-CN", "回族", "民族.回族"),
            // dict.hr.ethnic.group.3
            ("dict.hr.ethnic.group.3", "zh-HK", "回族", "民族.回族"),

            // dict.hr.ethnic.group.4
            ("dict.hr.ethnic.group.4", "en-US", "藏族", "民族.藏族"),
            // dict.hr.ethnic.group.4
            ("dict.hr.ethnic.group.4", "ja-JP", "藏族", "民族.藏族"),
            // dict.hr.ethnic.group.4
            ("dict.hr.ethnic.group.4", "zh-CN", "藏族", "民族.藏族"),
            // dict.hr.ethnic.group.4
            ("dict.hr.ethnic.group.4", "zh-HK", "藏族", "民族.藏族"),

            // dict.hr.ethnic.group.5
            ("dict.hr.ethnic.group.5", "en-US", "维吾尔族", "民族.维吾尔族"),
            // dict.hr.ethnic.group.5
            ("dict.hr.ethnic.group.5", "ja-JP", "维吾尔族", "民族.维吾尔族"),
            // dict.hr.ethnic.group.5
            ("dict.hr.ethnic.group.5", "zh-CN", "维吾尔族", "民族.维吾尔族"),
            // dict.hr.ethnic.group.5
            ("dict.hr.ethnic.group.5", "zh-HK", "维吾尔族", "民族.维吾尔族"),

            // dict.hr.ethnic.group.6
            ("dict.hr.ethnic.group.6", "en-US", "苗族", "民族.苗族"),
            // dict.hr.ethnic.group.6
            ("dict.hr.ethnic.group.6", "ja-JP", "苗族", "民族.苗族"),
            // dict.hr.ethnic.group.6
            ("dict.hr.ethnic.group.6", "zh-CN", "苗族", "民族.苗族"),
            // dict.hr.ethnic.group.6
            ("dict.hr.ethnic.group.6", "zh-HK", "苗族", "民族.苗族"),

            // dict.hr.ethnic.group.7
            ("dict.hr.ethnic.group.7", "en-US", "彝族", "民族.彝族"),
            // dict.hr.ethnic.group.7
            ("dict.hr.ethnic.group.7", "ja-JP", "彝族", "民族.彝族"),
            // dict.hr.ethnic.group.7
            ("dict.hr.ethnic.group.7", "zh-CN", "彝族", "民族.彝族"),
            // dict.hr.ethnic.group.7
            ("dict.hr.ethnic.group.7", "zh-HK", "彝族", "民族.彝族"),

            // dict.hr.ethnic.group.8
            ("dict.hr.ethnic.group.8", "en-US", "壮族", "民族.壮族"),
            // dict.hr.ethnic.group.8
            ("dict.hr.ethnic.group.8", "ja-JP", "壮族", "民族.壮族"),
            // dict.hr.ethnic.group.8
            ("dict.hr.ethnic.group.8", "zh-CN", "壮族", "民族.壮族"),
            // dict.hr.ethnic.group.8
            ("dict.hr.ethnic.group.8", "zh-HK", "壮族", "民族.壮族"),

            // dict.hr.ethnic.group.9
            ("dict.hr.ethnic.group.9", "en-US", "布依族", "民族.布依族"),
            // dict.hr.ethnic.group.9
            ("dict.hr.ethnic.group.9", "ja-JP", "布依族", "民族.布依族"),
            // dict.hr.ethnic.group.9
            ("dict.hr.ethnic.group.9", "zh-CN", "布依族", "民族.布依族"),
            // dict.hr.ethnic.group.9
            ("dict.hr.ethnic.group.9", "zh-HK", "布依族", "民族.布依族"),

            // dict.hr.ethnic.group.10
            ("dict.hr.ethnic.group.10", "en-US", "朝鲜族", "民族.朝鲜族"),
            // dict.hr.ethnic.group.10
            ("dict.hr.ethnic.group.10", "ja-JP", "朝鲜族", "民族.朝鲜族"),
            // dict.hr.ethnic.group.10
            ("dict.hr.ethnic.group.10", "zh-CN", "朝鲜族", "民族.朝鲜族"),
            // dict.hr.ethnic.group.10
            ("dict.hr.ethnic.group.10", "zh-HK", "朝鲜族", "民族.朝鲜族"),

            // dict.hr.ethnic.group.11
            ("dict.hr.ethnic.group.11", "en-US", "满族", "民族.满族"),
            // dict.hr.ethnic.group.11
            ("dict.hr.ethnic.group.11", "ja-JP", "满族", "民族.满族"),
            // dict.hr.ethnic.group.11
            ("dict.hr.ethnic.group.11", "zh-CN", "满族", "民族.满族"),
            // dict.hr.ethnic.group.11
            ("dict.hr.ethnic.group.11", "zh-HK", "满族", "民族.满族"),

            // dict.hr.ethnic.group.12
            ("dict.hr.ethnic.group.12", "en-US", "侗族", "民族.侗族"),
            // dict.hr.ethnic.group.12
            ("dict.hr.ethnic.group.12", "ja-JP", "侗族", "民族.侗族"),
            // dict.hr.ethnic.group.12
            ("dict.hr.ethnic.group.12", "zh-CN", "侗族", "民族.侗族"),
            // dict.hr.ethnic.group.12
            ("dict.hr.ethnic.group.12", "zh-HK", "侗族", "民族.侗族"),

            // dict.hr.ethnic.group.13
            ("dict.hr.ethnic.group.13", "en-US", "瑶族", "民族.瑶族"),
            // dict.hr.ethnic.group.13
            ("dict.hr.ethnic.group.13", "ja-JP", "瑶族", "民族.瑶族"),
            // dict.hr.ethnic.group.13
            ("dict.hr.ethnic.group.13", "zh-CN", "瑶族", "民族.瑶族"),
            // dict.hr.ethnic.group.13
            ("dict.hr.ethnic.group.13", "zh-HK", "瑶族", "民族.瑶族"),

            // dict.hr.ethnic.group.14
            ("dict.hr.ethnic.group.14", "en-US", "白族", "民族.白族"),
            // dict.hr.ethnic.group.14
            ("dict.hr.ethnic.group.14", "ja-JP", "白族", "民族.白族"),
            // dict.hr.ethnic.group.14
            ("dict.hr.ethnic.group.14", "zh-CN", "白族", "民族.白族"),
            // dict.hr.ethnic.group.14
            ("dict.hr.ethnic.group.14", "zh-HK", "白族", "民族.白族"),

            // dict.hr.ethnic.group.15
            ("dict.hr.ethnic.group.15", "en-US", "土家族", "民族.土家族"),
            // dict.hr.ethnic.group.15
            ("dict.hr.ethnic.group.15", "ja-JP", "土家族", "民族.土家族"),
            // dict.hr.ethnic.group.15
            ("dict.hr.ethnic.group.15", "zh-CN", "土家族", "民族.土家族"),
            // dict.hr.ethnic.group.15
            ("dict.hr.ethnic.group.15", "zh-HK", "土家族", "民族.土家族"),

            // dict.hr.ethnic.group.16
            ("dict.hr.ethnic.group.16", "en-US", "哈尼族", "民族.哈尼族"),
            // dict.hr.ethnic.group.16
            ("dict.hr.ethnic.group.16", "ja-JP", "哈尼族", "民族.哈尼族"),
            // dict.hr.ethnic.group.16
            ("dict.hr.ethnic.group.16", "zh-CN", "哈尼族", "民族.哈尼族"),
            // dict.hr.ethnic.group.16
            ("dict.hr.ethnic.group.16", "zh-HK", "哈尼族", "民族.哈尼族"),

            // dict.hr.ethnic.group.17
            ("dict.hr.ethnic.group.17", "en-US", "哈萨克族", "民族.哈萨克族"),
            // dict.hr.ethnic.group.17
            ("dict.hr.ethnic.group.17", "ja-JP", "哈萨克族", "民族.哈萨克族"),
            // dict.hr.ethnic.group.17
            ("dict.hr.ethnic.group.17", "zh-CN", "哈萨克族", "民族.哈萨克族"),
            // dict.hr.ethnic.group.17
            ("dict.hr.ethnic.group.17", "zh-HK", "哈萨克族", "民族.哈萨克族"),

            // dict.hr.ethnic.group.18
            ("dict.hr.ethnic.group.18", "en-US", "傣族", "民族.傣族"),
            // dict.hr.ethnic.group.18
            ("dict.hr.ethnic.group.18", "ja-JP", "傣族", "民族.傣族"),
            // dict.hr.ethnic.group.18
            ("dict.hr.ethnic.group.18", "zh-CN", "傣族", "民族.傣族"),
            // dict.hr.ethnic.group.18
            ("dict.hr.ethnic.group.18", "zh-HK", "傣族", "民族.傣族"),

            // dict.hr.ethnic.group.19
            ("dict.hr.ethnic.group.19", "en-US", "黎族", "民族.黎族"),
            // dict.hr.ethnic.group.19
            ("dict.hr.ethnic.group.19", "ja-JP", "黎族", "民族.黎族"),
            // dict.hr.ethnic.group.19
            ("dict.hr.ethnic.group.19", "zh-CN", "黎族", "民族.黎族"),
            // dict.hr.ethnic.group.19
            ("dict.hr.ethnic.group.19", "zh-HK", "黎族", "民族.黎族"),

            // dict.hr.ethnic.group.20
            ("dict.hr.ethnic.group.20", "en-US", "傈僳族", "民族.傈僳族"),
            // dict.hr.ethnic.group.20
            ("dict.hr.ethnic.group.20", "ja-JP", "傈僳族", "民族.傈僳族"),
            // dict.hr.ethnic.group.20
            ("dict.hr.ethnic.group.20", "zh-CN", "傈僳族", "民族.傈僳族"),
            // dict.hr.ethnic.group.20
            ("dict.hr.ethnic.group.20", "zh-HK", "傈僳族", "民族.傈僳族"),

            // dict.hr.ethnic.group.21
            ("dict.hr.ethnic.group.21", "en-US", "佤族", "民族.佤族"),
            // dict.hr.ethnic.group.21
            ("dict.hr.ethnic.group.21", "ja-JP", "佤族", "民族.佤族"),
            // dict.hr.ethnic.group.21
            ("dict.hr.ethnic.group.21", "zh-CN", "佤族", "民族.佤族"),
            // dict.hr.ethnic.group.21
            ("dict.hr.ethnic.group.21", "zh-HK", "佤族", "民族.佤族"),

            // dict.hr.ethnic.group.22
            ("dict.hr.ethnic.group.22", "en-US", "畲族", "民族.畲族"),
            // dict.hr.ethnic.group.22
            ("dict.hr.ethnic.group.22", "ja-JP", "畲族", "民族.畲族"),
            // dict.hr.ethnic.group.22
            ("dict.hr.ethnic.group.22", "zh-CN", "畲族", "民族.畲族"),
            // dict.hr.ethnic.group.22
            ("dict.hr.ethnic.group.22", "zh-HK", "畲族", "民族.畲族"),

            // dict.hr.ethnic.group.23
            ("dict.hr.ethnic.group.23", "en-US", "高山族", "民族.高山族"),
            // dict.hr.ethnic.group.23
            ("dict.hr.ethnic.group.23", "ja-JP", "高山族", "民族.高山族"),
            // dict.hr.ethnic.group.23
            ("dict.hr.ethnic.group.23", "zh-CN", "高山族", "民族.高山族"),
            // dict.hr.ethnic.group.23
            ("dict.hr.ethnic.group.23", "zh-HK", "高山族", "民族.高山族"),

            // dict.hr.ethnic.group.24
            ("dict.hr.ethnic.group.24", "en-US", "拉祜族", "民族.拉祜族"),
            // dict.hr.ethnic.group.24
            ("dict.hr.ethnic.group.24", "ja-JP", "拉祜族", "民族.拉祜族"),
            // dict.hr.ethnic.group.24
            ("dict.hr.ethnic.group.24", "zh-CN", "拉祜族", "民族.拉祜族"),
            // dict.hr.ethnic.group.24
            ("dict.hr.ethnic.group.24", "zh-HK", "拉祜族", "民族.拉祜族"),

            // dict.hr.ethnic.group.25
            ("dict.hr.ethnic.group.25", "en-US", "水族", "民族.水族"),
            // dict.hr.ethnic.group.25
            ("dict.hr.ethnic.group.25", "ja-JP", "水族", "民族.水族"),
            // dict.hr.ethnic.group.25
            ("dict.hr.ethnic.group.25", "zh-CN", "水族", "民族.水族"),
            // dict.hr.ethnic.group.25
            ("dict.hr.ethnic.group.25", "zh-HK", "水族", "民族.水族"),

            // dict.hr.ethnic.group.26
            ("dict.hr.ethnic.group.26", "en-US", "东乡族", "民族.东乡族"),
            // dict.hr.ethnic.group.26
            ("dict.hr.ethnic.group.26", "ja-JP", "东乡族", "民族.东乡族"),
            // dict.hr.ethnic.group.26
            ("dict.hr.ethnic.group.26", "zh-CN", "东乡族", "民族.东乡族"),
            // dict.hr.ethnic.group.26
            ("dict.hr.ethnic.group.26", "zh-HK", "东乡族", "民族.东乡族"),

            // dict.hr.ethnic.group.27
            ("dict.hr.ethnic.group.27", "en-US", "纳西族", "民族.纳西族"),
            // dict.hr.ethnic.group.27
            ("dict.hr.ethnic.group.27", "ja-JP", "纳西族", "民族.纳西族"),
            // dict.hr.ethnic.group.27
            ("dict.hr.ethnic.group.27", "zh-CN", "纳西族", "民族.纳西族"),
            // dict.hr.ethnic.group.27
            ("dict.hr.ethnic.group.27", "zh-HK", "纳西族", "民族.纳西族"),

            // dict.hr.ethnic.group.28
            ("dict.hr.ethnic.group.28", "en-US", "景颇族", "民族.景颇族"),
            // dict.hr.ethnic.group.28
            ("dict.hr.ethnic.group.28", "ja-JP", "景颇族", "民族.景颇族"),
            // dict.hr.ethnic.group.28
            ("dict.hr.ethnic.group.28", "zh-CN", "景颇族", "民族.景颇族"),
            // dict.hr.ethnic.group.28
            ("dict.hr.ethnic.group.28", "zh-HK", "景颇族", "民族.景颇族"),

            // dict.hr.ethnic.group.29
            ("dict.hr.ethnic.group.29", "en-US", "柯尔克孜族", "民族.柯尔克孜族"),
            // dict.hr.ethnic.group.29
            ("dict.hr.ethnic.group.29", "ja-JP", "柯尔克孜族", "民族.柯尔克孜族"),
            // dict.hr.ethnic.group.29
            ("dict.hr.ethnic.group.29", "zh-CN", "柯尔克孜族", "民族.柯尔克孜族"),
            // dict.hr.ethnic.group.29
            ("dict.hr.ethnic.group.29", "zh-HK", "柯尔克孜族", "民族.柯尔克孜族"),

            // dict.hr.ethnic.group.30
            ("dict.hr.ethnic.group.30", "en-US", "土族", "民族.土族"),
            // dict.hr.ethnic.group.30
            ("dict.hr.ethnic.group.30", "ja-JP", "土族", "民族.土族"),
            // dict.hr.ethnic.group.30
            ("dict.hr.ethnic.group.30", "zh-CN", "土族", "民族.土族"),
            // dict.hr.ethnic.group.30
            ("dict.hr.ethnic.group.30", "zh-HK", "土族", "民族.土族"),

            // dict.hr.ethnic.group.31
            ("dict.hr.ethnic.group.31", "en-US", "达斡尔族", "民族.达斡尔族"),
            // dict.hr.ethnic.group.31
            ("dict.hr.ethnic.group.31", "ja-JP", "达斡尔族", "民族.达斡尔族"),
            // dict.hr.ethnic.group.31
            ("dict.hr.ethnic.group.31", "zh-CN", "达斡尔族", "民族.达斡尔族"),
            // dict.hr.ethnic.group.31
            ("dict.hr.ethnic.group.31", "zh-HK", "达斡尔族", "民族.达斡尔族"),

            // dict.hr.ethnic.group.32
            ("dict.hr.ethnic.group.32", "en-US", "仫佬族", "民族.仫佬族"),
            // dict.hr.ethnic.group.32
            ("dict.hr.ethnic.group.32", "ja-JP", "仫佬族", "民族.仫佬族"),
            // dict.hr.ethnic.group.32
            ("dict.hr.ethnic.group.32", "zh-CN", "仫佬族", "民族.仫佬族"),
            // dict.hr.ethnic.group.32
            ("dict.hr.ethnic.group.32", "zh-HK", "仫佬族", "民族.仫佬族"),

            // dict.hr.ethnic.group.33
            ("dict.hr.ethnic.group.33", "en-US", "羌族", "民族.羌族"),
            // dict.hr.ethnic.group.33
            ("dict.hr.ethnic.group.33", "ja-JP", "羌族", "民族.羌族"),
            // dict.hr.ethnic.group.33
            ("dict.hr.ethnic.group.33", "zh-CN", "羌族", "民族.羌族"),
            // dict.hr.ethnic.group.33
            ("dict.hr.ethnic.group.33", "zh-HK", "羌族", "民族.羌族"),

            // dict.hr.ethnic.group.34
            ("dict.hr.ethnic.group.34", "en-US", "布朗族", "民族.布朗族"),
            // dict.hr.ethnic.group.34
            ("dict.hr.ethnic.group.34", "ja-JP", "布朗族", "民族.布朗族"),
            // dict.hr.ethnic.group.34
            ("dict.hr.ethnic.group.34", "zh-CN", "布朗族", "民族.布朗族"),
            // dict.hr.ethnic.group.34
            ("dict.hr.ethnic.group.34", "zh-HK", "布朗族", "民族.布朗族"),

            // dict.hr.ethnic.group.35
            ("dict.hr.ethnic.group.35", "en-US", "撒拉族", "民族.撒拉族"),
            // dict.hr.ethnic.group.35
            ("dict.hr.ethnic.group.35", "ja-JP", "撒拉族", "民族.撒拉族"),
            // dict.hr.ethnic.group.35
            ("dict.hr.ethnic.group.35", "zh-CN", "撒拉族", "民族.撒拉族"),
            // dict.hr.ethnic.group.35
            ("dict.hr.ethnic.group.35", "zh-HK", "撒拉族", "民族.撒拉族"),

            // dict.hr.ethnic.group.36
            ("dict.hr.ethnic.group.36", "en-US", "毛南族", "民族.毛南族"),
            // dict.hr.ethnic.group.36
            ("dict.hr.ethnic.group.36", "ja-JP", "毛南族", "民族.毛南族"),
            // dict.hr.ethnic.group.36
            ("dict.hr.ethnic.group.36", "zh-CN", "毛南族", "民族.毛南族"),
            // dict.hr.ethnic.group.36
            ("dict.hr.ethnic.group.36", "zh-HK", "毛南族", "民族.毛南族"),

            // dict.hr.ethnic.group.37
            ("dict.hr.ethnic.group.37", "en-US", "仡佬族", "民族.仡佬族"),
            // dict.hr.ethnic.group.37
            ("dict.hr.ethnic.group.37", "ja-JP", "仡佬族", "民族.仡佬族"),
            // dict.hr.ethnic.group.37
            ("dict.hr.ethnic.group.37", "zh-CN", "仡佬族", "民族.仡佬族"),
            // dict.hr.ethnic.group.37
            ("dict.hr.ethnic.group.37", "zh-HK", "仡佬族", "民族.仡佬族"),

            // dict.hr.ethnic.group.38
            ("dict.hr.ethnic.group.38", "en-US", "锡伯族", "民族.锡伯族"),
            // dict.hr.ethnic.group.38
            ("dict.hr.ethnic.group.38", "ja-JP", "锡伯族", "民族.锡伯族"),
            // dict.hr.ethnic.group.38
            ("dict.hr.ethnic.group.38", "zh-CN", "锡伯族", "民族.锡伯族"),
            // dict.hr.ethnic.group.38
            ("dict.hr.ethnic.group.38", "zh-HK", "锡伯族", "民族.锡伯族"),

            // dict.hr.ethnic.group.39
            ("dict.hr.ethnic.group.39", "en-US", "阿昌族", "民族.阿昌族"),
            // dict.hr.ethnic.group.39
            ("dict.hr.ethnic.group.39", "ja-JP", "阿昌族", "民族.阿昌族"),
            // dict.hr.ethnic.group.39
            ("dict.hr.ethnic.group.39", "zh-CN", "阿昌族", "民族.阿昌族"),
            // dict.hr.ethnic.group.39
            ("dict.hr.ethnic.group.39", "zh-HK", "阿昌族", "民族.阿昌族"),

            // dict.hr.ethnic.group.40
            ("dict.hr.ethnic.group.40", "en-US", "普米族", "民族.普米族"),
            // dict.hr.ethnic.group.40
            ("dict.hr.ethnic.group.40", "ja-JP", "普米族", "民族.普米族"),
            // dict.hr.ethnic.group.40
            ("dict.hr.ethnic.group.40", "zh-CN", "普米族", "民族.普米族"),
            // dict.hr.ethnic.group.40
            ("dict.hr.ethnic.group.40", "zh-HK", "普米族", "民族.普米族"),

            // dict.hr.ethnic.group.41
            ("dict.hr.ethnic.group.41", "en-US", "塔吉克族", "民族.塔吉克族"),
            // dict.hr.ethnic.group.41
            ("dict.hr.ethnic.group.41", "ja-JP", "塔吉克族", "民族.塔吉克族"),
            // dict.hr.ethnic.group.41
            ("dict.hr.ethnic.group.41", "zh-CN", "塔吉克族", "民族.塔吉克族"),
            // dict.hr.ethnic.group.41
            ("dict.hr.ethnic.group.41", "zh-HK", "塔吉克族", "民族.塔吉克族"),

            // dict.hr.ethnic.group.42
            ("dict.hr.ethnic.group.42", "en-US", "怒族", "民族.怒族"),
            // dict.hr.ethnic.group.42
            ("dict.hr.ethnic.group.42", "ja-JP", "怒族", "民族.怒族"),
            // dict.hr.ethnic.group.42
            ("dict.hr.ethnic.group.42", "zh-CN", "怒族", "民族.怒族"),
            // dict.hr.ethnic.group.42
            ("dict.hr.ethnic.group.42", "zh-HK", "怒族", "民族.怒族"),

            // dict.hr.ethnic.group.43
            ("dict.hr.ethnic.group.43", "en-US", "乌孜别克族", "民族.乌孜别克族"),
            // dict.hr.ethnic.group.43
            ("dict.hr.ethnic.group.43", "ja-JP", "乌孜别克族", "民族.乌孜别克族"),
            // dict.hr.ethnic.group.43
            ("dict.hr.ethnic.group.43", "zh-CN", "乌孜别克族", "民族.乌孜别克族"),
            // dict.hr.ethnic.group.43
            ("dict.hr.ethnic.group.43", "zh-HK", "乌孜别克族", "民族.乌孜别克族"),

            // dict.hr.ethnic.group.44
            ("dict.hr.ethnic.group.44", "en-US", "俄罗斯族", "民族.俄罗斯族"),
            // dict.hr.ethnic.group.44
            ("dict.hr.ethnic.group.44", "ja-JP", "俄罗斯族", "民族.俄罗斯族"),
            // dict.hr.ethnic.group.44
            ("dict.hr.ethnic.group.44", "zh-CN", "俄罗斯族", "民族.俄罗斯族"),
            // dict.hr.ethnic.group.44
            ("dict.hr.ethnic.group.44", "zh-HK", "俄罗斯族", "民族.俄罗斯族"),

            // dict.hr.ethnic.group.45
            ("dict.hr.ethnic.group.45", "en-US", "鄂温克族", "民族.鄂温克族"),
            // dict.hr.ethnic.group.45
            ("dict.hr.ethnic.group.45", "ja-JP", "鄂温克族", "民族.鄂温克族"),
            // dict.hr.ethnic.group.45
            ("dict.hr.ethnic.group.45", "zh-CN", "鄂温克族", "民族.鄂温克族"),
            // dict.hr.ethnic.group.45
            ("dict.hr.ethnic.group.45", "zh-HK", "鄂温克族", "民族.鄂温克族"),

            // dict.hr.ethnic.group.46
            ("dict.hr.ethnic.group.46", "en-US", "德昂族", "民族.德昂族"),
            // dict.hr.ethnic.group.46
            ("dict.hr.ethnic.group.46", "ja-JP", "德昂族", "民族.德昂族"),
            // dict.hr.ethnic.group.46
            ("dict.hr.ethnic.group.46", "zh-CN", "德昂族", "民族.德昂族"),
            // dict.hr.ethnic.group.46
            ("dict.hr.ethnic.group.46", "zh-HK", "德昂族", "民族.德昂族"),

            // dict.hr.ethnic.group.47
            ("dict.hr.ethnic.group.47", "en-US", "保安族", "民族.保安族"),
            // dict.hr.ethnic.group.47
            ("dict.hr.ethnic.group.47", "ja-JP", "保安族", "民族.保安族"),
            // dict.hr.ethnic.group.47
            ("dict.hr.ethnic.group.47", "zh-CN", "保安族", "民族.保安族"),
            // dict.hr.ethnic.group.47
            ("dict.hr.ethnic.group.47", "zh-HK", "保安族", "民族.保安族"),

            // dict.hr.ethnic.group.48
            ("dict.hr.ethnic.group.48", "en-US", "裕固族", "民族.裕固族"),
            // dict.hr.ethnic.group.48
            ("dict.hr.ethnic.group.48", "ja-JP", "裕固族", "民族.裕固族"),
            // dict.hr.ethnic.group.48
            ("dict.hr.ethnic.group.48", "zh-CN", "裕固族", "民族.裕固族"),
            // dict.hr.ethnic.group.48
            ("dict.hr.ethnic.group.48", "zh-HK", "裕固族", "民族.裕固族"),

            // dict.hr.ethnic.group.49
            ("dict.hr.ethnic.group.49", "en-US", "京族", "民族.京族"),
            // dict.hr.ethnic.group.49
            ("dict.hr.ethnic.group.49", "ja-JP", "京族", "民族.京族"),
            // dict.hr.ethnic.group.49
            ("dict.hr.ethnic.group.49", "zh-CN", "京族", "民族.京族"),
            // dict.hr.ethnic.group.49
            ("dict.hr.ethnic.group.49", "zh-HK", "京族", "民族.京族"),

            // dict.hr.ethnic.group.50
            ("dict.hr.ethnic.group.50", "en-US", "塔塔尔族", "民族.塔塔尔族"),
            // dict.hr.ethnic.group.50
            ("dict.hr.ethnic.group.50", "ja-JP", "塔塔尔族", "民族.塔塔尔族"),
            // dict.hr.ethnic.group.50
            ("dict.hr.ethnic.group.50", "zh-CN", "塔塔尔族", "民族.塔塔尔族"),
            // dict.hr.ethnic.group.50
            ("dict.hr.ethnic.group.50", "zh-HK", "塔塔尔族", "民族.塔塔尔族"),

            // dict.hr.ethnic.group.51
            ("dict.hr.ethnic.group.51", "en-US", "独龙族", "民族.独龙族"),
            // dict.hr.ethnic.group.51
            ("dict.hr.ethnic.group.51", "ja-JP", "独龙族", "民族.独龙族"),
            // dict.hr.ethnic.group.51
            ("dict.hr.ethnic.group.51", "zh-CN", "独龙族", "民族.独龙族"),
            // dict.hr.ethnic.group.51
            ("dict.hr.ethnic.group.51", "zh-HK", "独龙族", "民族.独龙族"),

            // dict.hr.ethnic.group.52
            ("dict.hr.ethnic.group.52", "en-US", "鄂伦春族", "民族.鄂伦春族"),
            // dict.hr.ethnic.group.52
            ("dict.hr.ethnic.group.52", "ja-JP", "鄂伦春族", "民族.鄂伦春族"),
            // dict.hr.ethnic.group.52
            ("dict.hr.ethnic.group.52", "zh-CN", "鄂伦春族", "民族.鄂伦春族"),
            // dict.hr.ethnic.group.52
            ("dict.hr.ethnic.group.52", "zh-HK", "鄂伦春族", "民族.鄂伦春族"),

            // dict.hr.ethnic.group.53
            ("dict.hr.ethnic.group.53", "en-US", "赫哲族", "民族.赫哲族"),
            // dict.hr.ethnic.group.53
            ("dict.hr.ethnic.group.53", "ja-JP", "赫哲族", "民族.赫哲族"),
            // dict.hr.ethnic.group.53
            ("dict.hr.ethnic.group.53", "zh-CN", "赫哲族", "民族.赫哲族"),
            // dict.hr.ethnic.group.53
            ("dict.hr.ethnic.group.53", "zh-HK", "赫哲族", "民族.赫哲族"),

            // dict.hr.ethnic.group.54
            ("dict.hr.ethnic.group.54", "en-US", "门巴族", "民族.门巴族"),
            // dict.hr.ethnic.group.54
            ("dict.hr.ethnic.group.54", "ja-JP", "门巴族", "民族.门巴族"),
            // dict.hr.ethnic.group.54
            ("dict.hr.ethnic.group.54", "zh-CN", "门巴族", "民族.门巴族"),
            // dict.hr.ethnic.group.54
            ("dict.hr.ethnic.group.54", "zh-HK", "门巴族", "民族.门巴族"),

            // dict.hr.ethnic.group.55
            ("dict.hr.ethnic.group.55", "en-US", "珞巴族", "民族.珞巴族"),
            // dict.hr.ethnic.group.55
            ("dict.hr.ethnic.group.55", "ja-JP", "珞巴族", "民族.珞巴族"),
            // dict.hr.ethnic.group.55
            ("dict.hr.ethnic.group.55", "zh-CN", "珞巴族", "民族.珞巴族"),
            // dict.hr.ethnic.group.55
            ("dict.hr.ethnic.group.55", "zh-HK", "珞巴族", "民族.珞巴族"),

            // dict.hr.ethnic.group.56
            ("dict.hr.ethnic.group.56", "en-US", "基诺族", "民族.基诺族"),
            // dict.hr.ethnic.group.56
            ("dict.hr.ethnic.group.56", "ja-JP", "基诺族", "民族.基诺族"),
            // dict.hr.ethnic.group.56
            ("dict.hr.ethnic.group.56", "zh-CN", "基诺族", "民族.基诺族"),
            // dict.hr.ethnic.group.56
            ("dict.hr.ethnic.group.56", "zh-HK", "基诺族", "民族.基诺族"),

            // dict.hr.holiday.is.working.day.0
            ("dict.hr.holiday.is.working.day.0", "en-US", "非工作日", "假日是否工作日.非工作日"),
            // dict.hr.holiday.is.working.day.0
            ("dict.hr.holiday.is.working.day.0", "ja-JP", "非工作日", "假日是否工作日.非工作日"),
            // dict.hr.holiday.is.working.day.0
            ("dict.hr.holiday.is.working.day.0", "zh-CN", "非工作日", "假日是否工作日.非工作日"),
            // dict.hr.holiday.is.working.day.0
            ("dict.hr.holiday.is.working.day.0", "zh-HK", "非工作日", "假日是否工作日.非工作日"),

            // dict.hr.holiday.is.working.day.1
            ("dict.hr.holiday.is.working.day.1", "en-US", "工作日", "假日是否工作日.工作日"),
            // dict.hr.holiday.is.working.day.1
            ("dict.hr.holiday.is.working.day.1", "ja-JP", "工作日", "假日是否工作日.工作日"),
            // dict.hr.holiday.is.working.day.1
            ("dict.hr.holiday.is.working.day.1", "zh-CN", "工作日", "假日是否工作日.工作日"),
            // dict.hr.holiday.is.working.day.1
            ("dict.hr.holiday.is.working.day.1", "zh-HK", "工作日", "假日是否工作日.工作日"),

            // dict.hr.holiday.is.working.day.2
            ("dict.hr.holiday.is.working.day.2", "en-US", "半天等", "假日是否工作日.半天等"),
            // dict.hr.holiday.is.working.day.2
            ("dict.hr.holiday.is.working.day.2", "ja-JP", "半天等", "假日是否工作日.半天等"),
            // dict.hr.holiday.is.working.day.2
            ("dict.hr.holiday.is.working.day.2", "zh-CN", "半天等", "假日是否工作日.半天等"),
            // dict.hr.holiday.is.working.day.2
            ("dict.hr.holiday.is.working.day.2", "zh-HK", "半天等", "假日是否工作日.半天等"),

            // dict.hr.holiday.type.0
            ("dict.hr.holiday.type.0", "en-US", "法定", "假日类型.法定"),
            // dict.hr.holiday.type.0
            ("dict.hr.holiday.type.0", "ja-JP", "法定", "假日类型.法定"),
            // dict.hr.holiday.type.0
            ("dict.hr.holiday.type.0", "zh-CN", "法定", "假日类型.法定"),
            // dict.hr.holiday.type.0
            ("dict.hr.holiday.type.0", "zh-HK", "法定", "假日类型.法定"),

            // dict.hr.holiday.type.1
            ("dict.hr.holiday.type.1", "en-US", "调休", "假日类型.调休"),
            // dict.hr.holiday.type.1
            ("dict.hr.holiday.type.1", "ja-JP", "调休", "假日类型.调休"),
            // dict.hr.holiday.type.1
            ("dict.hr.holiday.type.1", "zh-CN", "调休", "假日类型.调休"),
            // dict.hr.holiday.type.1
            ("dict.hr.holiday.type.1", "zh-HK", "调休", "假日类型.调休"),

            // dict.hr.holiday.type.2
            ("dict.hr.holiday.type.2", "en-US", "公司", "假日类型.公司"),
            // dict.hr.holiday.type.2
            ("dict.hr.holiday.type.2", "ja-JP", "公司", "假日类型.公司"),
            // dict.hr.holiday.type.2
            ("dict.hr.holiday.type.2", "zh-CN", "公司", "假日类型.公司"),
            // dict.hr.holiday.type.2
            ("dict.hr.holiday.type.2", "zh-HK", "公司", "假日类型.公司"),

            // dict.hr.leave.status.0
            ("dict.hr.leave.status.0", "en-US", "草稿", "请假状态.草稿"),
            // dict.hr.leave.status.0
            ("dict.hr.leave.status.0", "ja-JP", "草稿", "请假状态.草稿"),
            // dict.hr.leave.status.0
            ("dict.hr.leave.status.0", "zh-CN", "草稿", "请假状态.草稿"),
            // dict.hr.leave.status.0
            ("dict.hr.leave.status.0", "zh-HK", "草稿", "请假状态.草稿"),

            // dict.hr.leave.status.1
            ("dict.hr.leave.status.1", "en-US", "审批中", "请假状态.审批中"),
            // dict.hr.leave.status.1
            ("dict.hr.leave.status.1", "ja-JP", "审批中", "请假状态.审批中"),
            // dict.hr.leave.status.1
            ("dict.hr.leave.status.1", "zh-CN", "审批中", "请假状态.审批中"),
            // dict.hr.leave.status.1
            ("dict.hr.leave.status.1", "zh-HK", "审批中", "请假状态.审批中"),

            // dict.hr.leave.status.2
            ("dict.hr.leave.status.2", "en-US", "已通过", "请假状态.已通过"),
            // dict.hr.leave.status.2
            ("dict.hr.leave.status.2", "ja-JP", "已通过", "请假状态.已通过"),
            // dict.hr.leave.status.2
            ("dict.hr.leave.status.2", "zh-CN", "已通过", "请假状态.已通过"),
            // dict.hr.leave.status.2
            ("dict.hr.leave.status.2", "zh-HK", "已通过", "请假状态.已通过"),

            // dict.hr.leave.status.3
            ("dict.hr.leave.status.3", "en-US", "已驳回", "请假状态.已驳回"),
            // dict.hr.leave.status.3
            ("dict.hr.leave.status.3", "ja-JP", "已驳回", "请假状态.已驳回"),
            // dict.hr.leave.status.3
            ("dict.hr.leave.status.3", "zh-CN", "已驳回", "请假状态.已驳回"),
            // dict.hr.leave.status.3
            ("dict.hr.leave.status.3", "zh-HK", "已驳回", "请假状态.已驳回"),

            // dict.hr.leave.status.4
            ("dict.hr.leave.status.4", "en-US", "已撤回", "请假状态.已撤回"),
            // dict.hr.leave.status.4
            ("dict.hr.leave.status.4", "ja-JP", "已撤回", "请假状态.已撤回"),
            // dict.hr.leave.status.4
            ("dict.hr.leave.status.4", "zh-CN", "已撤回", "请假状态.已撤回"),
            // dict.hr.leave.status.4
            ("dict.hr.leave.status.4", "zh-HK", "已撤回", "请假状态.已撤回"),

            // dict.hr.marital.status.0
            ("dict.hr.marital.status.0", "en-US", "未婚", "婚姻状况.未婚"),
            // dict.hr.marital.status.0
            ("dict.hr.marital.status.0", "ja-JP", "未婚", "婚姻状况.未婚"),
            // dict.hr.marital.status.0
            ("dict.hr.marital.status.0", "zh-CN", "未婚", "婚姻状况.未婚"),
            // dict.hr.marital.status.0
            ("dict.hr.marital.status.0", "zh-HK", "未婚", "婚姻状况.未婚"),

            // dict.hr.marital.status.1
            ("dict.hr.marital.status.1", "en-US", "已婚", "婚姻状况.已婚"),
            // dict.hr.marital.status.1
            ("dict.hr.marital.status.1", "ja-JP", "已婚", "婚姻状况.已婚"),
            // dict.hr.marital.status.1
            ("dict.hr.marital.status.1", "zh-CN", "已婚", "婚姻状况.已婚"),
            // dict.hr.marital.status.1
            ("dict.hr.marital.status.1", "zh-HK", "已婚", "婚姻状况.已婚"),

            // dict.hr.marital.status.2
            ("dict.hr.marital.status.2", "en-US", "离异", "婚姻状况.离异"),
            // dict.hr.marital.status.2
            ("dict.hr.marital.status.2", "ja-JP", "离异", "婚姻状况.离异"),
            // dict.hr.marital.status.2
            ("dict.hr.marital.status.2", "zh-CN", "离异", "婚姻状况.离异"),
            // dict.hr.marital.status.2
            ("dict.hr.marital.status.2", "zh-HK", "离异", "婚姻状况.离异"),

            // dict.hr.marital.status.3
            ("dict.hr.marital.status.3", "en-US", "丧偶", "婚姻状况.丧偶"),
            // dict.hr.marital.status.3
            ("dict.hr.marital.status.3", "ja-JP", "丧偶", "婚姻状况.丧偶"),
            // dict.hr.marital.status.3
            ("dict.hr.marital.status.3", "zh-CN", "丧偶", "婚姻状况.丧偶"),
            // dict.hr.marital.status.3
            ("dict.hr.marital.status.3", "zh-HK", "丧偶", "婚姻状况.丧偶"),

            // dict.hr.native.place.110000
            ("dict.hr.native.place.110000", "en-US", "北京市", "籍贯.北京市"),
            // dict.hr.native.place.110000
            ("dict.hr.native.place.110000", "ja-JP", "北京市", "籍贯.北京市"),
            // dict.hr.native.place.110000
            ("dict.hr.native.place.110000", "zh-CN", "北京市", "籍贯.北京市"),
            // dict.hr.native.place.110000
            ("dict.hr.native.place.110000", "zh-HK", "北京市", "籍贯.北京市"),

            // dict.hr.native.place.120000
            ("dict.hr.native.place.120000", "en-US", "天津市", "籍贯.天津市"),
            // dict.hr.native.place.120000
            ("dict.hr.native.place.120000", "ja-JP", "天津市", "籍贯.天津市"),
            // dict.hr.native.place.120000
            ("dict.hr.native.place.120000", "zh-CN", "天津市", "籍贯.天津市"),
            // dict.hr.native.place.120000
            ("dict.hr.native.place.120000", "zh-HK", "天津市", "籍贯.天津市"),

            // dict.hr.native.place.130000
            ("dict.hr.native.place.130000", "en-US", "河北省", "籍贯.河北省"),
            // dict.hr.native.place.130000
            ("dict.hr.native.place.130000", "ja-JP", "河北省", "籍贯.河北省"),
            // dict.hr.native.place.130000
            ("dict.hr.native.place.130000", "zh-CN", "河北省", "籍贯.河北省"),
            // dict.hr.native.place.130000
            ("dict.hr.native.place.130000", "zh-HK", "河北省", "籍贯.河北省"),

            // dict.hr.native.place.140000
            ("dict.hr.native.place.140000", "en-US", "山西省", "籍贯.山西省"),
            // dict.hr.native.place.140000
            ("dict.hr.native.place.140000", "ja-JP", "山西省", "籍贯.山西省"),
            // dict.hr.native.place.140000
            ("dict.hr.native.place.140000", "zh-CN", "山西省", "籍贯.山西省"),
            // dict.hr.native.place.140000
            ("dict.hr.native.place.140000", "zh-HK", "山西省", "籍贯.山西省"),

            // dict.hr.native.place.150000
            ("dict.hr.native.place.150000", "en-US", "内蒙古自治区", "籍贯.内蒙古自治区"),
            // dict.hr.native.place.150000
            ("dict.hr.native.place.150000", "ja-JP", "内蒙古自治区", "籍贯.内蒙古自治区"),
            // dict.hr.native.place.150000
            ("dict.hr.native.place.150000", "zh-CN", "内蒙古自治区", "籍贯.内蒙古自治区"),
            // dict.hr.native.place.150000
            ("dict.hr.native.place.150000", "zh-HK", "内蒙古自治区", "籍贯.内蒙古自治区"),

            // dict.hr.native.place.210000
            ("dict.hr.native.place.210000", "en-US", "辽宁省", "籍贯.辽宁省"),
            // dict.hr.native.place.210000
            ("dict.hr.native.place.210000", "ja-JP", "辽宁省", "籍贯.辽宁省"),
            // dict.hr.native.place.210000
            ("dict.hr.native.place.210000", "zh-CN", "辽宁省", "籍贯.辽宁省"),
            // dict.hr.native.place.210000
            ("dict.hr.native.place.210000", "zh-HK", "辽宁省", "籍贯.辽宁省"),

            // dict.hr.native.place.220000
            ("dict.hr.native.place.220000", "en-US", "吉林省", "籍贯.吉林省"),
            // dict.hr.native.place.220000
            ("dict.hr.native.place.220000", "ja-JP", "吉林省", "籍贯.吉林省"),
            // dict.hr.native.place.220000
            ("dict.hr.native.place.220000", "zh-CN", "吉林省", "籍贯.吉林省"),
            // dict.hr.native.place.220000
            ("dict.hr.native.place.220000", "zh-HK", "吉林省", "籍贯.吉林省"),

            // dict.hr.native.place.230000
            ("dict.hr.native.place.230000", "en-US", "黑龙江省", "籍贯.黑龙江省"),
            // dict.hr.native.place.230000
            ("dict.hr.native.place.230000", "ja-JP", "黑龙江省", "籍贯.黑龙江省"),
            // dict.hr.native.place.230000
            ("dict.hr.native.place.230000", "zh-CN", "黑龙江省", "籍贯.黑龙江省"),
            // dict.hr.native.place.230000
            ("dict.hr.native.place.230000", "zh-HK", "黑龙江省", "籍贯.黑龙江省"),

            // dict.hr.native.place.310000
            ("dict.hr.native.place.310000", "en-US", "上海市", "籍贯.上海市"),
            // dict.hr.native.place.310000
            ("dict.hr.native.place.310000", "ja-JP", "上海市", "籍贯.上海市"),
            // dict.hr.native.place.310000
            ("dict.hr.native.place.310000", "zh-CN", "上海市", "籍贯.上海市"),
            // dict.hr.native.place.310000
            ("dict.hr.native.place.310000", "zh-HK", "上海市", "籍贯.上海市"),

            // dict.hr.native.place.320000
            ("dict.hr.native.place.320000", "en-US", "江苏省", "籍贯.江苏省"),
            // dict.hr.native.place.320000
            ("dict.hr.native.place.320000", "ja-JP", "江苏省", "籍贯.江苏省"),
            // dict.hr.native.place.320000
            ("dict.hr.native.place.320000", "zh-CN", "江苏省", "籍贯.江苏省"),
            // dict.hr.native.place.320000
            ("dict.hr.native.place.320000", "zh-HK", "江苏省", "籍贯.江苏省"),

            // dict.hr.native.place.330000
            ("dict.hr.native.place.330000", "en-US", "浙江省", "籍贯.浙江省"),
            // dict.hr.native.place.330000
            ("dict.hr.native.place.330000", "ja-JP", "浙江省", "籍贯.浙江省"),
            // dict.hr.native.place.330000
            ("dict.hr.native.place.330000", "zh-CN", "浙江省", "籍贯.浙江省"),
            // dict.hr.native.place.330000
            ("dict.hr.native.place.330000", "zh-HK", "浙江省", "籍贯.浙江省"),

            // dict.hr.native.place.340000
            ("dict.hr.native.place.340000", "en-US", "安徽省", "籍贯.安徽省"),
            // dict.hr.native.place.340000
            ("dict.hr.native.place.340000", "ja-JP", "安徽省", "籍贯.安徽省"),
            // dict.hr.native.place.340000
            ("dict.hr.native.place.340000", "zh-CN", "安徽省", "籍贯.安徽省"),
            // dict.hr.native.place.340000
            ("dict.hr.native.place.340000", "zh-HK", "安徽省", "籍贯.安徽省"),

            // dict.hr.native.place.350000
            ("dict.hr.native.place.350000", "en-US", "福建省", "籍贯.福建省"),
            // dict.hr.native.place.350000
            ("dict.hr.native.place.350000", "ja-JP", "福建省", "籍贯.福建省"),
            // dict.hr.native.place.350000
            ("dict.hr.native.place.350000", "zh-CN", "福建省", "籍贯.福建省"),
            // dict.hr.native.place.350000
            ("dict.hr.native.place.350000", "zh-HK", "福建省", "籍贯.福建省"),

            // dict.hr.native.place.360000
            ("dict.hr.native.place.360000", "en-US", "江西省", "籍贯.江西省"),
            // dict.hr.native.place.360000
            ("dict.hr.native.place.360000", "ja-JP", "江西省", "籍贯.江西省"),
            // dict.hr.native.place.360000
            ("dict.hr.native.place.360000", "zh-CN", "江西省", "籍贯.江西省"),
            // dict.hr.native.place.360000
            ("dict.hr.native.place.360000", "zh-HK", "江西省", "籍贯.江西省"),

            // dict.hr.native.place.370000
            ("dict.hr.native.place.370000", "en-US", "山东省", "籍贯.山东省"),
            // dict.hr.native.place.370000
            ("dict.hr.native.place.370000", "ja-JP", "山东省", "籍贯.山东省"),
            // dict.hr.native.place.370000
            ("dict.hr.native.place.370000", "zh-CN", "山东省", "籍贯.山东省"),
            // dict.hr.native.place.370000
            ("dict.hr.native.place.370000", "zh-HK", "山东省", "籍贯.山东省"),

            // dict.hr.native.place.410000
            ("dict.hr.native.place.410000", "en-US", "河南省", "籍贯.河南省"),
            // dict.hr.native.place.410000
            ("dict.hr.native.place.410000", "ja-JP", "河南省", "籍贯.河南省"),
            // dict.hr.native.place.410000
            ("dict.hr.native.place.410000", "zh-CN", "河南省", "籍贯.河南省"),
            // dict.hr.native.place.410000
            ("dict.hr.native.place.410000", "zh-HK", "河南省", "籍贯.河南省"),

            // dict.hr.native.place.420000
            ("dict.hr.native.place.420000", "en-US", "湖北省", "籍贯.湖北省"),
            // dict.hr.native.place.420000
            ("dict.hr.native.place.420000", "ja-JP", "湖北省", "籍贯.湖北省"),
            // dict.hr.native.place.420000
            ("dict.hr.native.place.420000", "zh-CN", "湖北省", "籍贯.湖北省"),
            // dict.hr.native.place.420000
            ("dict.hr.native.place.420000", "zh-HK", "湖北省", "籍贯.湖北省"),

            // dict.hr.native.place.430000
            ("dict.hr.native.place.430000", "en-US", "湖南省", "籍贯.湖南省"),
            // dict.hr.native.place.430000
            ("dict.hr.native.place.430000", "ja-JP", "湖南省", "籍贯.湖南省"),
            // dict.hr.native.place.430000
            ("dict.hr.native.place.430000", "zh-CN", "湖南省", "籍贯.湖南省"),
            // dict.hr.native.place.430000
            ("dict.hr.native.place.430000", "zh-HK", "湖南省", "籍贯.湖南省"),

            // dict.hr.native.place.440000
            ("dict.hr.native.place.440000", "en-US", "广东省", "籍贯.广东省"),
            // dict.hr.native.place.440000
            ("dict.hr.native.place.440000", "ja-JP", "广东省", "籍贯.广东省"),
            // dict.hr.native.place.440000
            ("dict.hr.native.place.440000", "zh-CN", "广东省", "籍贯.广东省"),
            // dict.hr.native.place.440000
            ("dict.hr.native.place.440000", "zh-HK", "广东省", "籍贯.广东省"),

            // dict.hr.native.place.450000
            ("dict.hr.native.place.450000", "en-US", "广西壮族自治区", "籍贯.广西壮族自治区"),
            // dict.hr.native.place.450000
            ("dict.hr.native.place.450000", "ja-JP", "广西壮族自治区", "籍贯.广西壮族自治区"),
            // dict.hr.native.place.450000
            ("dict.hr.native.place.450000", "zh-CN", "广西壮族自治区", "籍贯.广西壮族自治区"),
            // dict.hr.native.place.450000
            ("dict.hr.native.place.450000", "zh-HK", "广西壮族自治区", "籍贯.广西壮族自治区"),

            // dict.hr.native.place.460000
            ("dict.hr.native.place.460000", "en-US", "海南省", "籍贯.海南省"),
            // dict.hr.native.place.460000
            ("dict.hr.native.place.460000", "ja-JP", "海南省", "籍贯.海南省"),
            // dict.hr.native.place.460000
            ("dict.hr.native.place.460000", "zh-CN", "海南省", "籍贯.海南省"),
            // dict.hr.native.place.460000
            ("dict.hr.native.place.460000", "zh-HK", "海南省", "籍贯.海南省"),

            // dict.hr.native.place.500000
            ("dict.hr.native.place.500000", "en-US", "重庆市", "籍贯.重庆市"),
            // dict.hr.native.place.500000
            ("dict.hr.native.place.500000", "ja-JP", "重庆市", "籍贯.重庆市"),
            // dict.hr.native.place.500000
            ("dict.hr.native.place.500000", "zh-CN", "重庆市", "籍贯.重庆市"),
            // dict.hr.native.place.500000
            ("dict.hr.native.place.500000", "zh-HK", "重庆市", "籍贯.重庆市"),

            // dict.hr.native.place.510000
            ("dict.hr.native.place.510000", "en-US", "四川省", "籍贯.四川省"),
            // dict.hr.native.place.510000
            ("dict.hr.native.place.510000", "ja-JP", "四川省", "籍贯.四川省"),
            // dict.hr.native.place.510000
            ("dict.hr.native.place.510000", "zh-CN", "四川省", "籍贯.四川省"),
            // dict.hr.native.place.510000
            ("dict.hr.native.place.510000", "zh-HK", "四川省", "籍贯.四川省"),

            // dict.hr.native.place.520000
            ("dict.hr.native.place.520000", "en-US", "贵州省", "籍贯.贵州省"),
            // dict.hr.native.place.520000
            ("dict.hr.native.place.520000", "ja-JP", "贵州省", "籍贯.贵州省"),
            // dict.hr.native.place.520000
            ("dict.hr.native.place.520000", "zh-CN", "贵州省", "籍贯.贵州省"),
            // dict.hr.native.place.520000
            ("dict.hr.native.place.520000", "zh-HK", "贵州省", "籍贯.贵州省"),

            // dict.hr.native.place.530000
            ("dict.hr.native.place.530000", "en-US", "云南省", "籍贯.云南省"),
            // dict.hr.native.place.530000
            ("dict.hr.native.place.530000", "ja-JP", "云南省", "籍贯.云南省"),
            // dict.hr.native.place.530000
            ("dict.hr.native.place.530000", "zh-CN", "云南省", "籍贯.云南省"),
            // dict.hr.native.place.530000
            ("dict.hr.native.place.530000", "zh-HK", "云南省", "籍贯.云南省"),

            // dict.hr.native.place.540000
            ("dict.hr.native.place.540000", "en-US", "西藏自治区", "籍贯.西藏自治区"),
            // dict.hr.native.place.540000
            ("dict.hr.native.place.540000", "ja-JP", "西藏自治区", "籍贯.西藏自治区"),
            // dict.hr.native.place.540000
            ("dict.hr.native.place.540000", "zh-CN", "西藏自治区", "籍贯.西藏自治区"),
            // dict.hr.native.place.540000
            ("dict.hr.native.place.540000", "zh-HK", "西藏自治区", "籍贯.西藏自治区"),

            // dict.hr.native.place.610000
            ("dict.hr.native.place.610000", "en-US", "陕西省", "籍贯.陕西省"),
            // dict.hr.native.place.610000
            ("dict.hr.native.place.610000", "ja-JP", "陕西省", "籍贯.陕西省"),
            // dict.hr.native.place.610000
            ("dict.hr.native.place.610000", "zh-CN", "陕西省", "籍贯.陕西省"),
            // dict.hr.native.place.610000
            ("dict.hr.native.place.610000", "zh-HK", "陕西省", "籍贯.陕西省"),

            // dict.hr.native.place.620000
            ("dict.hr.native.place.620000", "en-US", "甘肃省", "籍贯.甘肃省"),
            // dict.hr.native.place.620000
            ("dict.hr.native.place.620000", "ja-JP", "甘肃省", "籍贯.甘肃省"),
            // dict.hr.native.place.620000
            ("dict.hr.native.place.620000", "zh-CN", "甘肃省", "籍贯.甘肃省"),
            // dict.hr.native.place.620000
            ("dict.hr.native.place.620000", "zh-HK", "甘肃省", "籍贯.甘肃省"),

            // dict.hr.native.place.630000
            ("dict.hr.native.place.630000", "en-US", "青海省", "籍贯.青海省"),
            // dict.hr.native.place.630000
            ("dict.hr.native.place.630000", "ja-JP", "青海省", "籍贯.青海省"),
            // dict.hr.native.place.630000
            ("dict.hr.native.place.630000", "zh-CN", "青海省", "籍贯.青海省"),
            // dict.hr.native.place.630000
            ("dict.hr.native.place.630000", "zh-HK", "青海省", "籍贯.青海省"),

            // dict.hr.native.place.640000
            ("dict.hr.native.place.640000", "en-US", "宁夏回族自治区", "籍贯.宁夏回族自治区"),
            // dict.hr.native.place.640000
            ("dict.hr.native.place.640000", "ja-JP", "宁夏回族自治区", "籍贯.宁夏回族自治区"),
            // dict.hr.native.place.640000
            ("dict.hr.native.place.640000", "zh-CN", "宁夏回族自治区", "籍贯.宁夏回族自治区"),
            // dict.hr.native.place.640000
            ("dict.hr.native.place.640000", "zh-HK", "宁夏回族自治区", "籍贯.宁夏回族自治区"),

            // dict.hr.native.place.650000
            ("dict.hr.native.place.650000", "en-US", "新疆维吾尔自治区", "籍贯.新疆维吾尔自治区"),
            // dict.hr.native.place.650000
            ("dict.hr.native.place.650000", "ja-JP", "新疆维吾尔自治区", "籍贯.新疆维吾尔自治区"),
            // dict.hr.native.place.650000
            ("dict.hr.native.place.650000", "zh-CN", "新疆维吾尔自治区", "籍贯.新疆维吾尔自治区"),
            // dict.hr.native.place.650000
            ("dict.hr.native.place.650000", "zh-HK", "新疆维吾尔自治区", "籍贯.新疆维吾尔自治区"),

            // dict.hr.native.place.710000
            ("dict.hr.native.place.710000", "en-US", "台湾省", "籍贯.台湾省"),
            // dict.hr.native.place.710000
            ("dict.hr.native.place.710000", "ja-JP", "台湾省", "籍贯.台湾省"),
            // dict.hr.native.place.710000
            ("dict.hr.native.place.710000", "zh-CN", "台湾省", "籍贯.台湾省"),
            // dict.hr.native.place.710000
            ("dict.hr.native.place.710000", "zh-HK", "台湾省", "籍贯.台湾省"),

            // dict.hr.native.place.810000
            ("dict.hr.native.place.810000", "en-US", "香港特别行政区", "籍贯.香港特别行政区"),
            // dict.hr.native.place.810000
            ("dict.hr.native.place.810000", "ja-JP", "香港特别行政区", "籍贯.香港特别行政区"),
            // dict.hr.native.place.810000
            ("dict.hr.native.place.810000", "zh-CN", "香港特别行政区", "籍贯.香港特别行政区"),
            // dict.hr.native.place.810000
            ("dict.hr.native.place.810000", "zh-HK", "香港特别行政区", "籍贯.香港特别行政区"),

            // dict.hr.native.place.820000
            ("dict.hr.native.place.820000", "en-US", "澳门特别行政区", "籍贯.澳门特别行政区"),
            // dict.hr.native.place.820000
            ("dict.hr.native.place.820000", "ja-JP", "澳门特别行政区", "籍贯.澳门特别行政区"),
            // dict.hr.native.place.820000
            ("dict.hr.native.place.820000", "zh-CN", "澳门特别行政区", "籍贯.澳门特别行政区"),
            // dict.hr.native.place.820000
            ("dict.hr.native.place.820000", "zh-HK", "澳门特别行政区", "籍贯.澳门特别行政区"),

            // dict.hr.overtime.status.0
            ("dict.hr.overtime.status.0", "en-US", "草稿", "加班状态.草稿"),
            // dict.hr.overtime.status.0
            ("dict.hr.overtime.status.0", "ja-JP", "草稿", "加班状态.草稿"),
            // dict.hr.overtime.status.0
            ("dict.hr.overtime.status.0", "zh-CN", "草稿", "加班状态.草稿"),
            // dict.hr.overtime.status.0
            ("dict.hr.overtime.status.0", "zh-HK", "草稿", "加班状态.草稿"),

            // dict.hr.overtime.status.1
            ("dict.hr.overtime.status.1", "en-US", "已提交", "加班状态.已提交"),
            // dict.hr.overtime.status.1
            ("dict.hr.overtime.status.1", "ja-JP", "已提交", "加班状态.已提交"),
            // dict.hr.overtime.status.1
            ("dict.hr.overtime.status.1", "zh-CN", "已提交", "加班状态.已提交"),
            // dict.hr.overtime.status.1
            ("dict.hr.overtime.status.1", "zh-HK", "已提交", "加班状态.已提交"),

            // dict.hr.overtime.status.2
            ("dict.hr.overtime.status.2", "en-US", "已通过", "加班状态.已通过"),
            // dict.hr.overtime.status.2
            ("dict.hr.overtime.status.2", "ja-JP", "已通过", "加班状态.已通过"),
            // dict.hr.overtime.status.2
            ("dict.hr.overtime.status.2", "zh-CN", "已通过", "加班状态.已通过"),
            // dict.hr.overtime.status.2
            ("dict.hr.overtime.status.2", "zh-HK", "已通过", "加班状态.已通过"),

            // dict.hr.overtime.status.3
            ("dict.hr.overtime.status.3", "en-US", "已驳回", "加班状态.已驳回"),
            // dict.hr.overtime.status.3
            ("dict.hr.overtime.status.3", "ja-JP", "已驳回", "加班状态.已驳回"),
            // dict.hr.overtime.status.3
            ("dict.hr.overtime.status.3", "zh-CN", "已驳回", "加班状态.已驳回"),
            // dict.hr.overtime.status.3
            ("dict.hr.overtime.status.3", "zh-HK", "已驳回", "加班状态.已驳回"),

            // dict.hr.overtime.type.0
            ("dict.hr.overtime.type.0", "en-US", "工作日加班", "加班类型.工作日加班"),
            // dict.hr.overtime.type.0
            ("dict.hr.overtime.type.0", "ja-JP", "工作日加班", "加班类型.工作日加班"),
            // dict.hr.overtime.type.0
            ("dict.hr.overtime.type.0", "zh-CN", "工作日加班", "加班类型.工作日加班"),
            // dict.hr.overtime.type.0
            ("dict.hr.overtime.type.0", "zh-HK", "工作日加班", "加班类型.工作日加班"),

            // dict.hr.overtime.type.1
            ("dict.hr.overtime.type.1", "en-US", "休息日加班", "加班类型.休息日加班"),
            // dict.hr.overtime.type.1
            ("dict.hr.overtime.type.1", "ja-JP", "休息日加班", "加班类型.休息日加班"),
            // dict.hr.overtime.type.1
            ("dict.hr.overtime.type.1", "zh-CN", "休息日加班", "加班类型.休息日加班"),
            // dict.hr.overtime.type.1
            ("dict.hr.overtime.type.1", "zh-HK", "休息日加班", "加班类型.休息日加班"),

            // dict.hr.overtime.type.2
            ("dict.hr.overtime.type.2", "en-US", "法定节假日加班", "加班类型.法定节假日加班"),
            // dict.hr.overtime.type.2
            ("dict.hr.overtime.type.2", "ja-JP", "法定节假日加班", "加班类型.法定节假日加班"),
            // dict.hr.overtime.type.2
            ("dict.hr.overtime.type.2", "zh-CN", "法定节假日加班", "加班类型.法定节假日加班"),
            // dict.hr.overtime.type.2
            ("dict.hr.overtime.type.2", "zh-HK", "法定节假日加班", "加班类型.法定节假日加班"),

            // dict.hr.political.status.0
            ("dict.hr.political.status.0", "en-US", "群众", "政治面貌.群众"),
            // dict.hr.political.status.0
            ("dict.hr.political.status.0", "ja-JP", "群众", "政治面貌.群众"),
            // dict.hr.political.status.0
            ("dict.hr.political.status.0", "zh-CN", "群众", "政治面貌.群众"),
            // dict.hr.political.status.0
            ("dict.hr.political.status.0", "zh-HK", "群众", "政治面貌.群众"),

            // dict.hr.political.status.1
            ("dict.hr.political.status.1", "en-US", "共青团员", "政治面貌.共青团员"),
            // dict.hr.political.status.1
            ("dict.hr.political.status.1", "ja-JP", "共青团员", "政治面貌.共青团员"),
            // dict.hr.political.status.1
            ("dict.hr.political.status.1", "zh-CN", "共青团员", "政治面貌.共青团员"),
            // dict.hr.political.status.1
            ("dict.hr.political.status.1", "zh-HK", "共青团员", "政治面貌.共青团员"),

            // dict.hr.political.status.2
            ("dict.hr.political.status.2", "en-US", "中共党员", "政治面貌.中共党员"),
            // dict.hr.political.status.2
            ("dict.hr.political.status.2", "ja-JP", "中共党员", "政治面貌.中共党员"),
            // dict.hr.political.status.2
            ("dict.hr.political.status.2", "zh-CN", "中共党员", "政治面貌.中共党员"),
            // dict.hr.political.status.2
            ("dict.hr.political.status.2", "zh-HK", "中共党员", "政治面貌.中共党员"),

            // dict.hr.political.status.3
            ("dict.hr.political.status.3", "en-US", "中共预备党员", "政治面貌.中共预备党员"),
            // dict.hr.political.status.3
            ("dict.hr.political.status.3", "ja-JP", "中共预备党员", "政治面貌.中共预备党员"),
            // dict.hr.political.status.3
            ("dict.hr.political.status.3", "zh-CN", "中共预备党员", "政治面貌.中共预备党员"),
            // dict.hr.political.status.3
            ("dict.hr.political.status.3", "zh-HK", "中共预备党员", "政治面貌.中共预备党员"),

            // dict.hr.political.status.4
            ("dict.hr.political.status.4", "en-US", "民革党员", "政治面貌.民革党员"),
            // dict.hr.political.status.4
            ("dict.hr.political.status.4", "ja-JP", "民革党员", "政治面貌.民革党员"),
            // dict.hr.political.status.4
            ("dict.hr.political.status.4", "zh-CN", "民革党员", "政治面貌.民革党员"),
            // dict.hr.political.status.4
            ("dict.hr.political.status.4", "zh-HK", "民革党员", "政治面貌.民革党员"),

            // dict.hr.political.status.5
            ("dict.hr.political.status.5", "en-US", "民盟盟员", "政治面貌.民盟盟员"),
            // dict.hr.political.status.5
            ("dict.hr.political.status.5", "ja-JP", "民盟盟员", "政治面貌.民盟盟员"),
            // dict.hr.political.status.5
            ("dict.hr.political.status.5", "zh-CN", "民盟盟员", "政治面貌.民盟盟员"),
            // dict.hr.political.status.5
            ("dict.hr.political.status.5", "zh-HK", "民盟盟员", "政治面貌.民盟盟员"),

            // dict.hr.political.status.6
            ("dict.hr.political.status.6", "en-US", "民建会员", "政治面貌.民建会员"),
            // dict.hr.political.status.6
            ("dict.hr.political.status.6", "ja-JP", "民建会员", "政治面貌.民建会员"),
            // dict.hr.political.status.6
            ("dict.hr.political.status.6", "zh-CN", "民建会员", "政治面貌.民建会员"),
            // dict.hr.political.status.6
            ("dict.hr.political.status.6", "zh-HK", "民建会员", "政治面貌.民建会员"),

            // dict.hr.political.status.7
            ("dict.hr.political.status.7", "en-US", "民进会员", "政治面貌.民进会员"),
            // dict.hr.political.status.7
            ("dict.hr.political.status.7", "ja-JP", "民进会员", "政治面貌.民进会员"),
            // dict.hr.political.status.7
            ("dict.hr.political.status.7", "zh-CN", "民进会员", "政治面貌.民进会员"),
            // dict.hr.political.status.7
            ("dict.hr.political.status.7", "zh-HK", "民进会员", "政治面貌.民进会员"),

            // dict.hr.political.status.8
            ("dict.hr.political.status.8", "en-US", "农工党党员", "政治面貌.农工党党员"),
            // dict.hr.political.status.8
            ("dict.hr.political.status.8", "ja-JP", "农工党党员", "政治面貌.农工党党员"),
            // dict.hr.political.status.8
            ("dict.hr.political.status.8", "zh-CN", "农工党党员", "政治面貌.农工党党员"),
            // dict.hr.political.status.8
            ("dict.hr.political.status.8", "zh-HK", "农工党党员", "政治面貌.农工党党员"),

            // dict.hr.political.status.9
            ("dict.hr.political.status.9", "en-US", "致公党党员", "政治面貌.致公党党员"),
            // dict.hr.political.status.9
            ("dict.hr.political.status.9", "ja-JP", "致公党党员", "政治面貌.致公党党员"),
            // dict.hr.political.status.9
            ("dict.hr.political.status.9", "zh-CN", "致公党党员", "政治面貌.致公党党员"),
            // dict.hr.political.status.9
            ("dict.hr.political.status.9", "zh-HK", "致公党党员", "政治面貌.致公党党员"),

            // dict.hr.political.status.10
            ("dict.hr.political.status.10", "en-US", "九三学社社员", "政治面貌.九三学社社员"),
            // dict.hr.political.status.10
            ("dict.hr.political.status.10", "ja-JP", "九三学社社员", "政治面貌.九三学社社员"),
            // dict.hr.political.status.10
            ("dict.hr.political.status.10", "zh-CN", "九三学社社员", "政治面貌.九三学社社员"),
            // dict.hr.political.status.10
            ("dict.hr.political.status.10", "zh-HK", "九三学社社员", "政治面貌.九三学社社员"),

            // dict.hr.political.status.11
            ("dict.hr.political.status.11", "en-US", "台盟盟员", "政治面貌.台盟盟员"),
            // dict.hr.political.status.11
            ("dict.hr.political.status.11", "ja-JP", "台盟盟员", "政治面貌.台盟盟员"),
            // dict.hr.political.status.11
            ("dict.hr.political.status.11", "zh-CN", "台盟盟员", "政治面貌.台盟盟员"),
            // dict.hr.political.status.11
            ("dict.hr.political.status.11", "zh-HK", "台盟盟员", "政治面貌.台盟盟员"),

            // dict.hr.political.status.12
            ("dict.hr.political.status.12", "en-US", "无党派民主人士", "政治面貌.无党派民主人士"),
            // dict.hr.political.status.12
            ("dict.hr.political.status.12", "ja-JP", "无党派民主人士", "政治面貌.无党派民主人士"),
            // dict.hr.political.status.12
            ("dict.hr.political.status.12", "zh-CN", "无党派民主人士", "政治面貌.无党派民主人士"),
            // dict.hr.political.status.12
            ("dict.hr.political.status.12", "zh-HK", "无党派民主人士", "政治面貌.无党派民主人士"),

            // dict.hr.schedule.type.0
            ("dict.hr.schedule.type.0", "en-US", "部门", "排班类别.部门"),
            // dict.hr.schedule.type.0
            ("dict.hr.schedule.type.0", "ja-JP", "部门", "排班类别.部门"),
            // dict.hr.schedule.type.0
            ("dict.hr.schedule.type.0", "zh-CN", "部门", "排班类别.部门"),
            // dict.hr.schedule.type.0
            ("dict.hr.schedule.type.0", "zh-HK", "部门", "排班类别.部门"),

            // dict.hr.schedule.type.1
            ("dict.hr.schedule.type.1", "en-US", "人员", "排班类别.人员"),
            // dict.hr.schedule.type.1
            ("dict.hr.schedule.type.1", "ja-JP", "人员", "排班类别.人员"),
            // dict.hr.schedule.type.1
            ("dict.hr.schedule.type.1", "zh-CN", "人员", "排班类别.人员"),
            // dict.hr.schedule.type.1
            ("dict.hr.schedule.type.1", "zh-HK", "人员", "排班类别.人员"),

            // dict.hr.reassignment.status.0
            ("dict.hr.reassignment.status.0", "en-US", "草稿", "调动状态.草稿"),
            // dict.hr.reassignment.status.0
            ("dict.hr.reassignment.status.0", "ja-JP", "草稿", "调动状态.草稿"),
            // dict.hr.reassignment.status.0
            ("dict.hr.reassignment.status.0", "zh-CN", "草稿", "调动状态.草稿"),
            // dict.hr.reassignment.status.0
            ("dict.hr.reassignment.status.0", "zh-HK", "草稿", "调动状态.草稿"),

            // dict.hr.reassignment.status.1
            ("dict.hr.reassignment.status.1", "en-US", "审批中", "调动状态.审批中"),
            // dict.hr.reassignment.status.1
            ("dict.hr.reassignment.status.1", "ja-JP", "审批中", "调动状态.审批中"),
            // dict.hr.reassignment.status.1
            ("dict.hr.reassignment.status.1", "zh-CN", "审批中", "调动状态.审批中"),
            // dict.hr.reassignment.status.1
            ("dict.hr.reassignment.status.1", "zh-HK", "审批中", "调动状态.审批中"),

            // dict.hr.reassignment.status.2
            ("dict.hr.reassignment.status.2", "en-US", "已通过", "调动状态.已通过"),
            // dict.hr.reassignment.status.2
            ("dict.hr.reassignment.status.2", "ja-JP", "已通过", "调动状态.已通过"),
            // dict.hr.reassignment.status.2
            ("dict.hr.reassignment.status.2", "zh-CN", "已通过", "调动状态.已通过"),
            // dict.hr.reassignment.status.2
            ("dict.hr.reassignment.status.2", "zh-HK", "已通过", "调动状态.已通过"),

            // dict.hr.reassignment.status.3
            ("dict.hr.reassignment.status.3", "en-US", "已驳回", "调动状态.已驳回"),
            // dict.hr.reassignment.status.3
            ("dict.hr.reassignment.status.3", "ja-JP", "已驳回", "调动状态.已驳回"),
            // dict.hr.reassignment.status.3
            ("dict.hr.reassignment.status.3", "zh-CN", "已驳回", "调动状态.已驳回"),
            // dict.hr.reassignment.status.3
            ("dict.hr.reassignment.status.3", "zh-HK", "已驳回", "调动状态.已驳回"),

            // dict.hr.reassignment.status.4
            ("dict.hr.reassignment.status.4", "en-US", "已撤回", "调动状态.已撤回"),
            // dict.hr.reassignment.status.4
            ("dict.hr.reassignment.status.4", "ja-JP", "已撤回", "调动状态.已撤回"),
            // dict.hr.reassignment.status.4
            ("dict.hr.reassignment.status.4", "zh-CN", "已撤回", "调动状态.已撤回"),
            // dict.hr.reassignment.status.4
            ("dict.hr.reassignment.status.4", "zh-HK", "已撤回", "调动状态.已撤回"),

            // dict.hr.reassignment.type.0
            ("dict.hr.reassignment.type.0", "en-US", "转岗", "调动类型.转岗"),
            // dict.hr.reassignment.type.0
            ("dict.hr.reassignment.type.0", "ja-JP", "转岗", "调动类型.转岗"),
            // dict.hr.reassignment.type.0
            ("dict.hr.reassignment.type.0", "zh-CN", "转岗", "调动类型.转岗"),
            // dict.hr.reassignment.type.0
            ("dict.hr.reassignment.type.0", "zh-HK", "转岗", "调动类型.转岗"),

            // dict.hr.reassignment.type.1
            ("dict.hr.reassignment.type.1", "en-US", "调岗", "调动类型.调岗"),
            // dict.hr.reassignment.type.1
            ("dict.hr.reassignment.type.1", "ja-JP", "调岗", "调动类型.调岗"),
            // dict.hr.reassignment.type.1
            ("dict.hr.reassignment.type.1", "zh-CN", "调岗", "调动类型.调岗"),
            // dict.hr.reassignment.type.1
            ("dict.hr.reassignment.type.1", "zh-HK", "调岗", "调动类型.调岗"),

            // dict.logistics.batch.management.0
            ("dict.logistics.batch.management.0", "en-US", "否", "批次管理标识.否"),
            // dict.logistics.batch.management.0
            ("dict.logistics.batch.management.0", "ja-JP", "否", "批次管理标识.否"),
            // dict.logistics.batch.management.0
            ("dict.logistics.batch.management.0", "zh-CN", "否", "批次管理标识.否"),
            // dict.logistics.batch.management.0
            ("dict.logistics.batch.management.0", "zh-HK", "否", "批次管理标识.否"),

            // dict.logistics.batch.management.1
            ("dict.logistics.batch.management.1", "en-US", "是", "批次管理标识.是"),
            // dict.logistics.batch.management.1
            ("dict.logistics.batch.management.1", "ja-JP", "是", "批次管理标识.是"),
            // dict.logistics.batch.management.1
            ("dict.logistics.batch.management.1", "zh-CN", "是", "批次管理标识.是"),
            // dict.logistics.batch.management.1
            ("dict.logistics.batch.management.1", "zh-HK", "是", "批次管理标识.是"),

            // dict.logistics.bulk.material.0
            ("dict.logistics.bulk.material.0", "en-US", "否", "散装物料标识.否"),
            // dict.logistics.bulk.material.0
            ("dict.logistics.bulk.material.0", "ja-JP", "否", "散装物料标识.否"),
            // dict.logistics.bulk.material.0
            ("dict.logistics.bulk.material.0", "zh-CN", "否", "散装物料标识.否"),
            // dict.logistics.bulk.material.0
            ("dict.logistics.bulk.material.0", "zh-HK", "否", "散装物料标识.否"),

            // dict.logistics.bulk.material.1
            ("dict.logistics.bulk.material.1", "en-US", "是", "散装物料标识.是"),
            // dict.logistics.bulk.material.1
            ("dict.logistics.bulk.material.1", "ja-JP", "是", "散装物料标识.是"),
            // dict.logistics.bulk.material.1
            ("dict.logistics.bulk.material.1", "zh-CN", "是", "散装物料标识.是"),
            // dict.logistics.bulk.material.1
            ("dict.logistics.bulk.material.1", "zh-HK", "是", "散装物料标识.是"),

            // dict.logistics.credit.rating.aaa
            ("dict.logistics.credit.rating.aaa", "en-US", "aaa", "信用等级.aaa级"),
            // dict.logistics.credit.rating.aaa
            ("dict.logistics.credit.rating.aaa", "ja-JP", "aaa", "信用等级.aaa级"),
            // dict.logistics.credit.rating.aaa
            ("dict.logistics.credit.rating.aaa", "zh-CN", "aaa级", "信用等级.aaa级"),
            // dict.logistics.credit.rating.aaa
            ("dict.logistics.credit.rating.aaa", "zh-HK", "aaa级", "信用等级.aaa级"),

            // dict.logistics.credit.rating.aa
            ("dict.logistics.credit.rating.aa", "en-US", "aa", "信用等级.aa级"),
            // dict.logistics.credit.rating.aa
            ("dict.logistics.credit.rating.aa", "ja-JP", "aa", "信用等级.aa级"),
            // dict.logistics.credit.rating.aa
            ("dict.logistics.credit.rating.aa", "zh-CN", "aa级", "信用等级.aa级"),
            // dict.logistics.credit.rating.aa
            ("dict.logistics.credit.rating.aa", "zh-HK", "aa级", "信用等级.aa级"),

            // dict.logistics.credit.rating.a
            ("dict.logistics.credit.rating.a", "en-US", "a", "信用等级.a级"),
            // dict.logistics.credit.rating.a
            ("dict.logistics.credit.rating.a", "ja-JP", "a", "信用等级.a级"),
            // dict.logistics.credit.rating.a
            ("dict.logistics.credit.rating.a", "zh-CN", "a级", "信用等级.a级"),
            // dict.logistics.credit.rating.a
            ("dict.logistics.credit.rating.a", "zh-HK", "a级", "信用等级.a级"),

            // dict.logistics.credit.rating.bbb
            ("dict.logistics.credit.rating.bbb", "en-US", "bbb", "信用等级.bbb级"),
            // dict.logistics.credit.rating.bbb
            ("dict.logistics.credit.rating.bbb", "ja-JP", "bbb", "信用等级.bbb级"),
            // dict.logistics.credit.rating.bbb
            ("dict.logistics.credit.rating.bbb", "zh-CN", "bbb级", "信用等级.bbb级"),
            // dict.logistics.credit.rating.bbb
            ("dict.logistics.credit.rating.bbb", "zh-HK", "bbb级", "信用等级.bbb级"),

            // dict.logistics.credit.rating.bb
            ("dict.logistics.credit.rating.bb", "en-US", "bb", "信用等级.bb级"),
            // dict.logistics.credit.rating.bb
            ("dict.logistics.credit.rating.bb", "ja-JP", "bb", "信用等级.bb级"),
            // dict.logistics.credit.rating.bb
            ("dict.logistics.credit.rating.bb", "zh-CN", "bb级", "信用等级.bb级"),
            // dict.logistics.credit.rating.bb
            ("dict.logistics.credit.rating.bb", "zh-HK", "bb级", "信用等级.bb级"),

            // dict.logistics.credit.rating.b
            ("dict.logistics.credit.rating.b", "en-US", "b", "信用等级.b级"),
            // dict.logistics.credit.rating.b
            ("dict.logistics.credit.rating.b", "ja-JP", "b", "信用等级.b级"),
            // dict.logistics.credit.rating.b
            ("dict.logistics.credit.rating.b", "zh-CN", "b级", "信用等级.b级"),
            // dict.logistics.credit.rating.b
            ("dict.logistics.credit.rating.b", "zh-HK", "b级", "信用等级.b级"),

            // dict.logistics.credit.rating.ccc
            ("dict.logistics.credit.rating.ccc", "en-US", "ccc", "信用等级.ccc级"),
            // dict.logistics.credit.rating.ccc
            ("dict.logistics.credit.rating.ccc", "ja-JP", "ccc", "信用等级.ccc级"),
            // dict.logistics.credit.rating.ccc
            ("dict.logistics.credit.rating.ccc", "zh-CN", "ccc级", "信用等级.ccc级"),
            // dict.logistics.credit.rating.ccc
            ("dict.logistics.credit.rating.ccc", "zh-HK", "ccc级", "信用等级.ccc级"),

            // dict.logistics.credit.rating.cc
            ("dict.logistics.credit.rating.cc", "en-US", "cc", "信用等级.cc级"),
            // dict.logistics.credit.rating.cc
            ("dict.logistics.credit.rating.cc", "ja-JP", "cc", "信用等级.cc级"),
            // dict.logistics.credit.rating.cc
            ("dict.logistics.credit.rating.cc", "zh-CN", "cc级", "信用等级.cc级"),
            // dict.logistics.credit.rating.cc
            ("dict.logistics.credit.rating.cc", "zh-HK", "cc级", "信用等级.cc级"),

            // dict.logistics.credit.rating.c
            ("dict.logistics.credit.rating.c", "en-US", "c", "信用等级.c级"),
            // dict.logistics.credit.rating.c
            ("dict.logistics.credit.rating.c", "ja-JP", "c", "信用等级.c级"),
            // dict.logistics.credit.rating.c
            ("dict.logistics.credit.rating.c", "zh-CN", "c级", "信用等级.c级"),
            // dict.logistics.credit.rating.c
            ("dict.logistics.credit.rating.c", "zh-HK", "c级", "信用等级.c级"),

            // dict.logistics.customer.category.strategic
            ("dict.logistics.customer.category.strategic", "en-US", "strategic", "客户类别.战略客户"),
            // dict.logistics.customer.category.strategic
            ("dict.logistics.customer.category.strategic", "ja-JP", "strategic", "客户类别.战略客户"),
            // dict.logistics.customer.category.strategic
            ("dict.logistics.customer.category.strategic", "zh-CN", "战略客户", "客户类别.战略客户"),
            // dict.logistics.customer.category.strategic
            ("dict.logistics.customer.category.strategic", "zh-HK", "战略客户", "客户类别.战略客户"),

            // dict.logistics.customer.category.key
            ("dict.logistics.customer.category.key", "en-US", "key", "客户类别.重点客户"),
            // dict.logistics.customer.category.key
            ("dict.logistics.customer.category.key", "ja-JP", "key", "客户类别.重点客户"),
            // dict.logistics.customer.category.key
            ("dict.logistics.customer.category.key", "zh-CN", "重点客户", "客户类别.重点客户"),
            // dict.logistics.customer.category.key
            ("dict.logistics.customer.category.key", "zh-HK", "重点客户", "客户类别.重点客户"),

            // dict.logistics.customer.category.normal
            ("dict.logistics.customer.category.normal", "en-US", "normal", "客户类别.普通客户"),
            // dict.logistics.customer.category.normal
            ("dict.logistics.customer.category.normal", "ja-JP", "normal", "客户类别.普通客户"),
            // dict.logistics.customer.category.normal
            ("dict.logistics.customer.category.normal", "zh-CN", "普通客户", "客户类别.普通客户"),
            // dict.logistics.customer.category.normal
            ("dict.logistics.customer.category.normal", "zh-HK", "普通客户", "客户类别.普通客户"),

            // dict.logistics.customer.category.potential
            ("dict.logistics.customer.category.potential", "en-US", "potential", "客户类别.潜在客户"),
            // dict.logistics.customer.category.potential
            ("dict.logistics.customer.category.potential", "ja-JP", "potential", "客户类别.潜在客户"),
            // dict.logistics.customer.category.potential
            ("dict.logistics.customer.category.potential", "zh-CN", "潜在客户", "客户类别.潜在客户"),
            // dict.logistics.customer.category.potential
            ("dict.logistics.customer.category.potential", "zh-HK", "潜在客户", "客户类别.潜在客户"),

            // dict.logistics.customer.category.temporary
            ("dict.logistics.customer.category.temporary", "en-US", "temporary", "客户类别.临时客户"),
            // dict.logistics.customer.category.temporary
            ("dict.logistics.customer.category.temporary", "ja-JP", "temporary", "客户类别.临时客户"),
            // dict.logistics.customer.category.temporary
            ("dict.logistics.customer.category.temporary", "zh-CN", "临时客户", "客户类别.临时客户"),
            // dict.logistics.customer.category.temporary
            ("dict.logistics.customer.category.temporary", "zh-HK", "临时客户", "客户类别.临时客户"),

            // dict.logistics.customer.category.dealer
            ("dict.logistics.customer.category.dealer", "en-US", "dealer", "客户类别.经销商"),
            // dict.logistics.customer.category.dealer
            ("dict.logistics.customer.category.dealer", "ja-JP", "dealer", "客户类别.经销商"),
            // dict.logistics.customer.category.dealer
            ("dict.logistics.customer.category.dealer", "zh-CN", "经销商", "客户类别.经销商"),
            // dict.logistics.customer.category.dealer
            ("dict.logistics.customer.category.dealer", "zh-HK", "经销商", "客户类别.经销商"),

            // dict.logistics.customer.category.agent
            ("dict.logistics.customer.category.agent", "en-US", "agent", "客户类别.代理商"),
            // dict.logistics.customer.category.agent
            ("dict.logistics.customer.category.agent", "ja-JP", "agent", "客户类别.代理商"),
            // dict.logistics.customer.category.agent
            ("dict.logistics.customer.category.agent", "zh-CN", "代理商", "客户类别.代理商"),
            // dict.logistics.customer.category.agent
            ("dict.logistics.customer.category.agent", "zh-HK", "代理商", "客户类别.代理商"),

            // dict.logistics.customer.category.enduser
            ("dict.logistics.customer.category.enduser", "en-US", "enduser", "客户类别.终端客户"),
            // dict.logistics.customer.category.enduser
            ("dict.logistics.customer.category.enduser", "ja-JP", "enduser", "客户类别.终端客户"),
            // dict.logistics.customer.category.enduser
            ("dict.logistics.customer.category.enduser", "zh-CN", "终端客户", "客户类别.终端客户"),
            // dict.logistics.customer.category.enduser
            ("dict.logistics.customer.category.enduser", "zh-HK", "终端客户", "客户类别.终端客户"),

            // dict.logistics.cycle.counting.a
            ("dict.logistics.cycle.counting.a", "en-US", "a", "周期盘点标识.12月"),
            // dict.logistics.cycle.counting.a
            ("dict.logistics.cycle.counting.a", "ja-JP", "a", "周期盘点标识.12月"),
            // dict.logistics.cycle.counting.a
            ("dict.logistics.cycle.counting.a", "zh-CN", "12月", "周期盘点标识.12月"),
            // dict.logistics.cycle.counting.a
            ("dict.logistics.cycle.counting.a", "zh-HK", "12月", "周期盘点标识.12月"),

            // dict.logistics.cycle.counting.b
            ("dict.logistics.cycle.counting.b", "en-US", "b", "周期盘点标识.6月"),
            // dict.logistics.cycle.counting.b
            ("dict.logistics.cycle.counting.b", "ja-JP", "b", "周期盘点标识.6月"),
            // dict.logistics.cycle.counting.b
            ("dict.logistics.cycle.counting.b", "zh-CN", "6月", "周期盘点标识.6月"),
            // dict.logistics.cycle.counting.b
            ("dict.logistics.cycle.counting.b", "zh-HK", "6月", "周期盘点标识.6月"),

            // dict.logistics.cycle.counting.c
            ("dict.logistics.cycle.counting.c", "en-US", "c", "周期盘点标识.3月"),
            // dict.logistics.cycle.counting.c
            ("dict.logistics.cycle.counting.c", "ja-JP", "c", "周期盘点标识.3月"),
            // dict.logistics.cycle.counting.c
            ("dict.logistics.cycle.counting.c", "zh-CN", "3月", "周期盘点标识.3月"),
            // dict.logistics.cycle.counting.c
            ("dict.logistics.cycle.counting.c", "zh-HK", "3月", "周期盘点标识.3月"),

            // dict.logistics.cycle.counting.d
            ("dict.logistics.cycle.counting.d", "en-US", "d", "周期盘点标识.1月"),
            // dict.logistics.cycle.counting.d
            ("dict.logistics.cycle.counting.d", "ja-JP", "d", "周期盘点标识.1月"),
            // dict.logistics.cycle.counting.d
            ("dict.logistics.cycle.counting.d", "zh-CN", "1月", "周期盘点标识.1月"),
            // dict.logistics.cycle.counting.d
            ("dict.logistics.cycle.counting.d", "zh-HK", "1月", "周期盘点标识.1月"),

            // dict.logistics.defect.level.critical
            ("dict.logistics.defect.level.critical", "en-US", "critical", "缺点等级.致命缺陷"),
            // dict.logistics.defect.level.critical
            ("dict.logistics.defect.level.critical", "ja-JP", "critical", "缺点等级.致命缺陷"),
            // dict.logistics.defect.level.critical
            ("dict.logistics.defect.level.critical", "zh-CN", "致命缺陷", "缺点等级.致命缺陷"),
            // dict.logistics.defect.level.critical
            ("dict.logistics.defect.level.critical", "zh-HK", "致命缺陷", "缺点等级.致命缺陷"),

            // dict.logistics.defect.level.major
            ("dict.logistics.defect.level.major", "en-US", "major", "缺点等级.严重缺陷"),
            // dict.logistics.defect.level.major
            ("dict.logistics.defect.level.major", "ja-JP", "major", "缺点等级.严重缺陷"),
            // dict.logistics.defect.level.major
            ("dict.logistics.defect.level.major", "zh-CN", "严重缺陷", "缺点等级.严重缺陷"),
            // dict.logistics.defect.level.major
            ("dict.logistics.defect.level.major", "zh-HK", "严重缺陷", "缺点等级.严重缺陷"),

            // dict.logistics.defect.level.minor
            ("dict.logistics.defect.level.minor", "en-US", "minor", "缺点等级.轻微缺陷"),
            // dict.logistics.defect.level.minor
            ("dict.logistics.defect.level.minor", "ja-JP", "minor", "缺点等级.轻微缺陷"),
            // dict.logistics.defect.level.minor
            ("dict.logistics.defect.level.minor", "zh-CN", "轻微缺陷", "缺点等级.轻微缺陷"),
            // dict.logistics.defect.level.minor
            ("dict.logistics.defect.level.minor", "zh-HK", "轻微缺陷", "缺点等级.轻微缺陷"),

            // dict.logistics.defect.level.suggestion
            ("dict.logistics.defect.level.suggestion", "en-US", "suggestion", "缺点等级.建议改进"),
            // dict.logistics.defect.level.suggestion
            ("dict.logistics.defect.level.suggestion", "ja-JP", "suggestion", "缺点等级.建议改进"),
            // dict.logistics.defect.level.suggestion
            ("dict.logistics.defect.level.suggestion", "zh-CN", "建议改进", "缺点等级.建议改进"),
            // dict.logistics.defect.level.suggestion
            ("dict.logistics.defect.level.suggestion", "zh-HK", "建议改进", "缺点等级.建议改进"),

            // dict.logistics.equipment.category.production
            ("dict.logistics.equipment.category.production", "en-US", "production", "设备类别.生产设备"),
            // dict.logistics.equipment.category.production
            ("dict.logistics.equipment.category.production", "ja-JP", "production", "设备类别.生产设备"),
            // dict.logistics.equipment.category.production
            ("dict.logistics.equipment.category.production", "zh-CN", "生产设备", "设备类别.生产设备"),
            // dict.logistics.equipment.category.production
            ("dict.logistics.equipment.category.production", "zh-HK", "生产设备", "设备类别.生产设备"),

            // dict.logistics.equipment.category.inspection
            ("dict.logistics.equipment.category.inspection", "en-US", "inspection", "设备类别.检测设备"),
            // dict.logistics.equipment.category.inspection
            ("dict.logistics.equipment.category.inspection", "ja-JP", "inspection", "设备类别.检测设备"),
            // dict.logistics.equipment.category.inspection
            ("dict.logistics.equipment.category.inspection", "zh-CN", "检测设备", "设备类别.检测设备"),
            // dict.logistics.equipment.category.inspection
            ("dict.logistics.equipment.category.inspection", "zh-HK", "检测设备", "设备类别.检测设备"),

            // dict.logistics.equipment.category.packaging
            ("dict.logistics.equipment.category.packaging", "en-US", "packaging", "设备类别.包装设备"),
            // dict.logistics.equipment.category.packaging
            ("dict.logistics.equipment.category.packaging", "ja-JP", "packaging", "设备类别.包装设备"),
            // dict.logistics.equipment.category.packaging
            ("dict.logistics.equipment.category.packaging", "zh-CN", "包装设备", "设备类别.包装设备"),
            // dict.logistics.equipment.category.packaging
            ("dict.logistics.equipment.category.packaging", "zh-HK", "包装设备", "设备类别.包装设备"),

            // dict.logistics.equipment.category.warehouse
            ("dict.logistics.equipment.category.warehouse", "en-US", "warehouse", "设备类别.仓储设备"),
            // dict.logistics.equipment.category.warehouse
            ("dict.logistics.equipment.category.warehouse", "ja-JP", "warehouse", "设备类别.仓储设备"),
            // dict.logistics.equipment.category.warehouse
            ("dict.logistics.equipment.category.warehouse", "zh-CN", "仓储设备", "设备类别.仓储设备"),
            // dict.logistics.equipment.category.warehouse
            ("dict.logistics.equipment.category.warehouse", "zh-HK", "仓储设备", "设备类别.仓储设备"),

            // dict.logistics.equipment.category.transport
            ("dict.logistics.equipment.category.transport", "en-US", "transport", "设备类别.运输设备"),
            // dict.logistics.equipment.category.transport
            ("dict.logistics.equipment.category.transport", "ja-JP", "transport", "设备类别.运输设备"),
            // dict.logistics.equipment.category.transport
            ("dict.logistics.equipment.category.transport", "zh-CN", "运输设备", "设备类别.运输设备"),
            // dict.logistics.equipment.category.transport
            ("dict.logistics.equipment.category.transport", "zh-HK", "运输设备", "设备类别.运输设备"),

            // dict.logistics.equipment.category.office
            ("dict.logistics.equipment.category.office", "en-US", "office", "设备类别.办公设备"),
            // dict.logistics.equipment.category.office
            ("dict.logistics.equipment.category.office", "ja-JP", "office", "设备类别.办公设备"),
            // dict.logistics.equipment.category.office
            ("dict.logistics.equipment.category.office", "zh-CN", "办公设备", "设备类别.办公设备"),
            // dict.logistics.equipment.category.office
            ("dict.logistics.equipment.category.office", "zh-HK", "办公设备", "设备类别.办公设备"),

            // dict.logistics.equipment.category.it
            ("dict.logistics.equipment.category.it", "en-US", "it", "设备类别.it设备"),
            // dict.logistics.equipment.category.it
            ("dict.logistics.equipment.category.it", "ja-JP", "it", "设备类别.it设备"),
            // dict.logistics.equipment.category.it
            ("dict.logistics.equipment.category.it", "zh-CN", "it设备", "设备类别.it设备"),
            // dict.logistics.equipment.category.it
            ("dict.logistics.equipment.category.it", "zh-HK", "it设备", "设备类别.it设备"),

            // dict.logistics.equipment.category.power
            ("dict.logistics.equipment.category.power", "en-US", "power", "设备类别.动力设备"),
            // dict.logistics.equipment.category.power
            ("dict.logistics.equipment.category.power", "ja-JP", "power", "设备类别.动力设备"),
            // dict.logistics.equipment.category.power
            ("dict.logistics.equipment.category.power", "zh-CN", "动力设备", "设备类别.动力设备"),
            // dict.logistics.equipment.category.power
            ("dict.logistics.equipment.category.power", "zh-HK", "动力设备", "设备类别.动力设备"),

            // dict.logistics.equipment.category.environmental
            ("dict.logistics.equipment.category.environmental", "en-US", "environmental", "设备类别.环保设备"),
            // dict.logistics.equipment.category.environmental
            ("dict.logistics.equipment.category.environmental", "ja-JP", "environmental", "设备类别.环保设备"),
            // dict.logistics.equipment.category.environmental
            ("dict.logistics.equipment.category.environmental", "zh-CN", "环保设备", "设备类别.环保设备"),
            // dict.logistics.equipment.category.environmental
            ("dict.logistics.equipment.category.environmental", "zh-HK", "环保设备", "设备类别.环保设备"),

            // dict.logistics.equipment.category.special
            ("dict.logistics.equipment.category.special", "en-US", "special", "设备类别.特种设备"),
            // dict.logistics.equipment.category.special
            ("dict.logistics.equipment.category.special", "ja-JP", "special", "设备类别.特种设备"),
            // dict.logistics.equipment.category.special
            ("dict.logistics.equipment.category.special", "zh-CN", "特种设备", "设备类别.特种设备"),
            // dict.logistics.equipment.category.special
            ("dict.logistics.equipment.category.special", "zh-HK", "特种设备", "设备类别.特种设备"),

            // dict.logistics.grade.category.a
            ("dict.logistics.grade.category.a", "en-US", "a", "等级类别.a级"),
            // dict.logistics.grade.category.a
            ("dict.logistics.grade.category.a", "ja-JP", "a", "等级类别.a级"),
            // dict.logistics.grade.category.a
            ("dict.logistics.grade.category.a", "zh-CN", "a级", "等级类别.a级"),
            // dict.logistics.grade.category.a
            ("dict.logistics.grade.category.a", "zh-HK", "a级", "等级类别.a级"),

            // dict.logistics.grade.category.b
            ("dict.logistics.grade.category.b", "en-US", "b", "等级类别.b级"),
            // dict.logistics.grade.category.b
            ("dict.logistics.grade.category.b", "ja-JP", "b", "等级类别.b级"),
            // dict.logistics.grade.category.b
            ("dict.logistics.grade.category.b", "zh-CN", "b级", "等级类别.b级"),
            // dict.logistics.grade.category.b
            ("dict.logistics.grade.category.b", "zh-HK", "b级", "等级类别.b级"),

            // dict.logistics.grade.category.c
            ("dict.logistics.grade.category.c", "en-US", "c", "等级类别.c级"),
            // dict.logistics.grade.category.c
            ("dict.logistics.grade.category.c", "ja-JP", "c", "等级类别.c级"),
            // dict.logistics.grade.category.c
            ("dict.logistics.grade.category.c", "zh-CN", "c级", "等级类别.c级"),
            // dict.logistics.grade.category.c
            ("dict.logistics.grade.category.c", "zh-HK", "c级", "等级类别.c级"),

            // dict.logistics.grade.category.d
            ("dict.logistics.grade.category.d", "en-US", "d", "等级类别.d级"),
            // dict.logistics.grade.category.d
            ("dict.logistics.grade.category.d", "ja-JP", "d", "等级类别.d级"),
            // dict.logistics.grade.category.d
            ("dict.logistics.grade.category.d", "zh-CN", "d级", "等级类别.d级"),
            // dict.logistics.grade.category.d
            ("dict.logistics.grade.category.d", "zh-HK", "d级", "等级类别.d级"),

            // dict.logistics.grade.category.e
            ("dict.logistics.grade.category.e", "en-US", "e", "等级类别.e级"),
            // dict.logistics.grade.category.e
            ("dict.logistics.grade.category.e", "ja-JP", "e", "等级类别.e级"),
            // dict.logistics.grade.category.e
            ("dict.logistics.grade.category.e", "zh-CN", "e级", "等级类别.e级"),
            // dict.logistics.grade.category.e
            ("dict.logistics.grade.category.e", "zh-HK", "e级", "等级类别.e级"),

            // dict.logistics.handling.plan.rework
            ("dict.logistics.handling.plan.rework", "en-US", "rework", "处理方案.返工"),
            // dict.logistics.handling.plan.rework
            ("dict.logistics.handling.plan.rework", "ja-JP", "rework", "处理方案.返工"),
            // dict.logistics.handling.plan.rework
            ("dict.logistics.handling.plan.rework", "zh-CN", "返工", "处理方案.返工"),
            // dict.logistics.handling.plan.rework
            ("dict.logistics.handling.plan.rework", "zh-HK", "返工", "处理方案.返工"),

            // dict.logistics.handling.plan.repair
            ("dict.logistics.handling.plan.repair", "en-US", "repair", "处理方案.返修"),
            // dict.logistics.handling.plan.repair
            ("dict.logistics.handling.plan.repair", "ja-JP", "repair", "处理方案.返修"),
            // dict.logistics.handling.plan.repair
            ("dict.logistics.handling.plan.repair", "zh-CN", "返修", "处理方案.返修"),
            // dict.logistics.handling.plan.repair
            ("dict.logistics.handling.plan.repair", "zh-HK", "返修", "处理方案.返修"),

            // dict.logistics.handling.plan.scrap
            ("dict.logistics.handling.plan.scrap", "en-US", "scrap", "处理方案.报废"),
            // dict.logistics.handling.plan.scrap
            ("dict.logistics.handling.plan.scrap", "ja-JP", "scrap", "处理方案.报废"),
            // dict.logistics.handling.plan.scrap
            ("dict.logistics.handling.plan.scrap", "zh-CN", "报废", "处理方案.报废"),
            // dict.logistics.handling.plan.scrap
            ("dict.logistics.handling.plan.scrap", "zh-HK", "报废", "处理方案.报废"),

            // dict.logistics.handling.plan.return
            ("dict.logistics.handling.plan.return", "en-US", "return", "处理方案.退货"),
            // dict.logistics.handling.plan.return
            ("dict.logistics.handling.plan.return", "ja-JP", "return", "处理方案.退货"),
            // dict.logistics.handling.plan.return
            ("dict.logistics.handling.plan.return", "zh-CN", "退货", "处理方案.退货"),
            // dict.logistics.handling.plan.return
            ("dict.logistics.handling.plan.return", "zh-HK", "退货", "处理方案.退货"),

            // dict.logistics.handling.plan.exchange
            ("dict.logistics.handling.plan.exchange", "en-US", "exchange", "处理方案.换货"),
            // dict.logistics.handling.plan.exchange
            ("dict.logistics.handling.plan.exchange", "ja-JP", "exchange", "处理方案.换货"),
            // dict.logistics.handling.plan.exchange
            ("dict.logistics.handling.plan.exchange", "zh-CN", "换货", "处理方案.换货"),
            // dict.logistics.handling.plan.exchange
            ("dict.logistics.handling.plan.exchange", "zh-HK", "换货", "处理方案.换货"),

            // dict.logistics.handling.plan.concession
            ("dict.logistics.handling.plan.concession", "en-US", "concession", "处理方案.让步接收"),
            // dict.logistics.handling.plan.concession
            ("dict.logistics.handling.plan.concession", "ja-JP", "concession", "处理方案.让步接收"),
            // dict.logistics.handling.plan.concession
            ("dict.logistics.handling.plan.concession", "zh-CN", "让步接收", "处理方案.让步接收"),
            // dict.logistics.handling.plan.concession
            ("dict.logistics.handling.plan.concession", "zh-HK", "让步接收", "处理方案.让步接收"),

            // dict.logistics.handling.plan.downgrade
            ("dict.logistics.handling.plan.downgrade", "en-US", "downgrade", "处理方案.降级使用"),
            // dict.logistics.handling.plan.downgrade
            ("dict.logistics.handling.plan.downgrade", "ja-JP", "downgrade", "处理方案.降级使用"),
            // dict.logistics.handling.plan.downgrade
            ("dict.logistics.handling.plan.downgrade", "zh-CN", "降级使用", "处理方案.降级使用"),
            // dict.logistics.handling.plan.downgrade
            ("dict.logistics.handling.plan.downgrade", "zh-HK", "降级使用", "处理方案.降级使用"),

            // dict.logistics.handling.plan.sorting
            ("dict.logistics.handling.plan.sorting", "en-US", "sorting", "处理方案.挑选使用"),
            // dict.logistics.handling.plan.sorting
            ("dict.logistics.handling.plan.sorting", "ja-JP", "sorting", "处理方案.挑选使用"),
            // dict.logistics.handling.plan.sorting
            ("dict.logistics.handling.plan.sorting", "zh-CN", "挑选使用", "处理方案.挑选使用"),
            // dict.logistics.handling.plan.sorting
            ("dict.logistics.handling.plan.sorting", "zh-HK", "挑选使用", "处理方案.挑选使用"),

            // dict.logistics.handling.plan.special_accept
            ("dict.logistics.handling.plan.special_accept", "en-US", "special_accept", "处理方案.特采"),
            // dict.logistics.handling.plan.special_accept
            ("dict.logistics.handling.plan.special_accept", "ja-JP", "special_accept", "处理方案.特采"),
            // dict.logistics.handling.plan.special_accept
            ("dict.logistics.handling.plan.special_accept", "zh-CN", "特采", "处理方案.特采"),
            // dict.logistics.handling.plan.special_accept
            ("dict.logistics.handling.plan.special_accept", "zh-HK", "特采", "处理方案.特采"),

            // dict.logistics.inhouse.production.days.2
            ("dict.logistics.inhouse.production.days.2", "en-US", "2天", "自制生产天数.2天"),
            // dict.logistics.inhouse.production.days.2
            ("dict.logistics.inhouse.production.days.2", "ja-JP", "2天", "自制生产天数.2天"),
            // dict.logistics.inhouse.production.days.2
            ("dict.logistics.inhouse.production.days.2", "zh-CN", "2天", "自制生产天数.2天"),
            // dict.logistics.inhouse.production.days.2
            ("dict.logistics.inhouse.production.days.2", "zh-HK", "2天", "自制生产天数.2天"),

            // dict.logistics.inhouse.production.days.5
            ("dict.logistics.inhouse.production.days.5", "en-US", "5天", "自制生产天数.5天"),
            // dict.logistics.inhouse.production.days.5
            ("dict.logistics.inhouse.production.days.5", "ja-JP", "5天", "自制生产天数.5天"),
            // dict.logistics.inhouse.production.days.5
            ("dict.logistics.inhouse.production.days.5", "zh-CN", "5天", "自制生产天数.5天"),
            // dict.logistics.inhouse.production.days.5
            ("dict.logistics.inhouse.production.days.5", "zh-HK", "5天", "自制生产天数.5天"),

            // dict.logistics.inspection.category.iqc
            ("dict.logistics.inspection.category.iqc", "en-US", "iqc", "检验类型.进料检验"),
            // dict.logistics.inspection.category.iqc
            ("dict.logistics.inspection.category.iqc", "ja-JP", "iqc", "检验类型.进料检验"),
            // dict.logistics.inspection.category.iqc
            ("dict.logistics.inspection.category.iqc", "zh-CN", "进料检验", "检验类型.进料检验"),
            // dict.logistics.inspection.category.iqc
            ("dict.logistics.inspection.category.iqc", "zh-HK", "进料检验", "检验类型.进料检验"),

            // dict.logistics.inspection.category.ipqc
            ("dict.logistics.inspection.category.ipqc", "en-US", "ipqc", "检验类型.过程检验"),
            // dict.logistics.inspection.category.ipqc
            ("dict.logistics.inspection.category.ipqc", "ja-JP", "ipqc", "检验类型.过程检验"),
            // dict.logistics.inspection.category.ipqc
            ("dict.logistics.inspection.category.ipqc", "zh-CN", "过程检验", "检验类型.过程检验"),
            // dict.logistics.inspection.category.ipqc
            ("dict.logistics.inspection.category.ipqc", "zh-HK", "过程检验", "检验类型.过程检验"),

            // dict.logistics.inspection.category.fqc
            ("dict.logistics.inspection.category.fqc", "en-US", "fqc", "检验类型.最终检验"),
            // dict.logistics.inspection.category.fqc
            ("dict.logistics.inspection.category.fqc", "ja-JP", "fqc", "检验类型.最终检验"),
            // dict.logistics.inspection.category.fqc
            ("dict.logistics.inspection.category.fqc", "zh-CN", "最终检验", "检验类型.最终检验"),
            // dict.logistics.inspection.category.fqc
            ("dict.logistics.inspection.category.fqc", "zh-HK", "最终检验", "检验类型.最终检验"),

            // dict.logistics.inspection.category.oqc
            ("dict.logistics.inspection.category.oqc", "en-US", "oqc", "检验类型.出货检验"),
            // dict.logistics.inspection.category.oqc
            ("dict.logistics.inspection.category.oqc", "ja-JP", "oqc", "检验类型.出货检验"),
            // dict.logistics.inspection.category.oqc
            ("dict.logistics.inspection.category.oqc", "zh-CN", "出货检验", "检验类型.出货检验"),
            // dict.logistics.inspection.category.oqc
            ("dict.logistics.inspection.category.oqc", "zh-HK", "出货检验", "检验类型.出货检验"),

            // dict.logistics.inspection.category.fai
            ("dict.logistics.inspection.category.fai", "en-US", "fai", "检验类型.首件检验"),
            // dict.logistics.inspection.category.fai
            ("dict.logistics.inspection.category.fai", "ja-JP", "fai", "检验类型.首件检验"),
            // dict.logistics.inspection.category.fai
            ("dict.logistics.inspection.category.fai", "zh-CN", "首件检验", "检验类型.首件检验"),
            // dict.logistics.inspection.category.fai
            ("dict.logistics.inspection.category.fai", "zh-HK", "首件检验", "检验类型.首件检验"),

            // dict.logistics.inspection.category.patrol
            ("dict.logistics.inspection.category.patrol", "en-US", "patrol", "检验类型.巡检"),
            // dict.logistics.inspection.category.patrol
            ("dict.logistics.inspection.category.patrol", "ja-JP", "patrol", "检验类型.巡检"),
            // dict.logistics.inspection.category.patrol
            ("dict.logistics.inspection.category.patrol", "zh-CN", "巡检", "检验类型.巡检"),
            // dict.logistics.inspection.category.patrol
            ("dict.logistics.inspection.category.patrol", "zh-HK", "巡检", "检验类型.巡检"),

            // dict.logistics.inspection.category.full
            ("dict.logistics.inspection.category.full", "en-US", "full", "检验类型.全检"),
            // dict.logistics.inspection.category.full
            ("dict.logistics.inspection.category.full", "ja-JP", "full", "检验类型.全检"),
            // dict.logistics.inspection.category.full
            ("dict.logistics.inspection.category.full", "zh-CN", "全检", "检验类型.全检"),
            // dict.logistics.inspection.category.full
            ("dict.logistics.inspection.category.full", "zh-HK", "全检", "检验类型.全检"),

            // dict.logistics.inspection.category.sampling
            ("dict.logistics.inspection.category.sampling", "en-US", "sampling", "检验类型.抽样检验"),
            // dict.logistics.inspection.category.sampling
            ("dict.logistics.inspection.category.sampling", "ja-JP", "sampling", "检验类型.抽样检验"),
            // dict.logistics.inspection.category.sampling
            ("dict.logistics.inspection.category.sampling", "zh-CN", "抽样检验", "检验类型.抽样检验"),
            // dict.logistics.inspection.category.sampling
            ("dict.logistics.inspection.category.sampling", "zh-HK", "抽样检验", "检验类型.抽样检验"),

            // dict.logistics.inspection.category.type_test
            ("dict.logistics.inspection.category.type_test", "en-US", "type_test", "检验类型.型式试验"),
            // dict.logistics.inspection.category.type_test
            ("dict.logistics.inspection.category.type_test", "ja-JP", "type_test", "检验类型.型式试验"),
            // dict.logistics.inspection.category.type_test
            ("dict.logistics.inspection.category.type_test", "zh-CN", "型式试验", "检验类型.型式试验"),
            // dict.logistics.inspection.category.type_test
            ("dict.logistics.inspection.category.type_test", "zh-HK", "型式试验", "检验类型.型式试验"),

            // dict.logistics.inspection.category.reliability
            ("dict.logistics.inspection.category.reliability", "en-US", "reliability", "检验类型.可靠性试验"),
            // dict.logistics.inspection.category.reliability
            ("dict.logistics.inspection.category.reliability", "ja-JP", "reliability", "检验类型.可靠性试验"),
            // dict.logistics.inspection.category.reliability
            ("dict.logistics.inspection.category.reliability", "zh-CN", "可靠性试验", "检验类型.可靠性试验"),
            // dict.logistics.inspection.category.reliability
            ("dict.logistics.inspection.category.reliability", "zh-HK", "可靠性试验", "检验类型.可靠性试验"),

            // dict.logistics.inspection.item.type.dimension
            ("dict.logistics.inspection.item.type.dimension", "en-US", "dimension", "检验项目类型.尺寸检验"),
            // dict.logistics.inspection.item.type.dimension
            ("dict.logistics.inspection.item.type.dimension", "ja-JP", "dimension", "检验项目类型.尺寸检验"),
            // dict.logistics.inspection.item.type.dimension
            ("dict.logistics.inspection.item.type.dimension", "zh-CN", "尺寸检验", "检验项目类型.尺寸检验"),
            // dict.logistics.inspection.item.type.dimension
            ("dict.logistics.inspection.item.type.dimension", "zh-HK", "尺寸检验", "检验项目类型.尺寸检验"),

            // dict.logistics.inspection.item.type.appearance
            ("dict.logistics.inspection.item.type.appearance", "en-US", "appearance", "检验项目类型.外观检验"),
            // dict.logistics.inspection.item.type.appearance
            ("dict.logistics.inspection.item.type.appearance", "ja-JP", "appearance", "检验项目类型.外观检验"),
            // dict.logistics.inspection.item.type.appearance
            ("dict.logistics.inspection.item.type.appearance", "zh-CN", "外观检验", "检验项目类型.外观检验"),
            // dict.logistics.inspection.item.type.appearance
            ("dict.logistics.inspection.item.type.appearance", "zh-HK", "外观检验", "检验项目类型.外观检验"),

            // dict.logistics.inspection.item.type.performance
            ("dict.logistics.inspection.item.type.performance", "en-US", "performance", "检验项目类型.性能检验"),
            // dict.logistics.inspection.item.type.performance
            ("dict.logistics.inspection.item.type.performance", "ja-JP", "performance", "检验项目类型.性能检验"),
            // dict.logistics.inspection.item.type.performance
            ("dict.logistics.inspection.item.type.performance", "zh-CN", "性能检验", "检验项目类型.性能检验"),
            // dict.logistics.inspection.item.type.performance
            ("dict.logistics.inspection.item.type.performance", "zh-HK", "性能检验", "检验项目类型.性能检验"),

            // dict.logistics.inspection.item.type.function
            ("dict.logistics.inspection.item.type.function", "en-US", "function", "检验项目类型.功能检验"),
            // dict.logistics.inspection.item.type.function
            ("dict.logistics.inspection.item.type.function", "ja-JP", "function", "检验项目类型.功能检验"),
            // dict.logistics.inspection.item.type.function
            ("dict.logistics.inspection.item.type.function", "zh-CN", "功能检验", "检验项目类型.功能检验"),
            // dict.logistics.inspection.item.type.function
            ("dict.logistics.inspection.item.type.function", "zh-HK", "功能检验", "检验项目类型.功能检验"),

            // dict.logistics.inspection.item.type.material
            ("dict.logistics.inspection.item.type.material", "en-US", "material", "检验项目类型.材质检验"),
            // dict.logistics.inspection.item.type.material
            ("dict.logistics.inspection.item.type.material", "ja-JP", "material", "检验项目类型.材质检验"),
            // dict.logistics.inspection.item.type.material
            ("dict.logistics.inspection.item.type.material", "zh-CN", "材质检验", "检验项目类型.材质检验"),
            // dict.logistics.inspection.item.type.material
            ("dict.logistics.inspection.item.type.material", "zh-HK", "材质检验", "检验项目类型.材质检验"),

            // dict.logistics.inspection.item.type.structure
            ("dict.logistics.inspection.item.type.structure", "en-US", "structure", "检验项目类型.结构检验"),
            // dict.logistics.inspection.item.type.structure
            ("dict.logistics.inspection.item.type.structure", "ja-JP", "structure", "检验项目类型.结构检验"),
            // dict.logistics.inspection.item.type.structure
            ("dict.logistics.inspection.item.type.structure", "zh-CN", "结构检验", "检验项目类型.结构检验"),
            // dict.logistics.inspection.item.type.structure
            ("dict.logistics.inspection.item.type.structure", "zh-HK", "结构检验", "检验项目类型.结构检验"),

            // dict.logistics.inspection.item.type.packaging
            ("dict.logistics.inspection.item.type.packaging", "en-US", "packaging", "检验项目类型.包装检验"),
            // dict.logistics.inspection.item.type.packaging
            ("dict.logistics.inspection.item.type.packaging", "ja-JP", "packaging", "检验项目类型.包装检验"),
            // dict.logistics.inspection.item.type.packaging
            ("dict.logistics.inspection.item.type.packaging", "zh-CN", "包装检验", "检验项目类型.包装检验"),
            // dict.logistics.inspection.item.type.packaging
            ("dict.logistics.inspection.item.type.packaging", "zh-HK", "包装检验", "检验项目类型.包装检验"),

            // dict.logistics.inspection.item.type.labeling
            ("dict.logistics.inspection.item.type.labeling", "en-US", "labeling", "检验项目类型.标识检验"),
            // dict.logistics.inspection.item.type.labeling
            ("dict.logistics.inspection.item.type.labeling", "ja-JP", "labeling", "检验项目类型.标识检验"),
            // dict.logistics.inspection.item.type.labeling
            ("dict.logistics.inspection.item.type.labeling", "zh-CN", "标识检验", "检验项目类型.标识检验"),
            // dict.logistics.inspection.item.type.labeling
            ("dict.logistics.inspection.item.type.labeling", "zh-HK", "标识检验", "检验项目类型.标识检验"),

            // dict.logistics.inspection.item.type.safety
            ("dict.logistics.inspection.item.type.safety", "en-US", "safety", "检验项目类型.安全检验"),
            // dict.logistics.inspection.item.type.safety
            ("dict.logistics.inspection.item.type.safety", "ja-JP", "safety", "检验项目类型.安全检验"),
            // dict.logistics.inspection.item.type.safety
            ("dict.logistics.inspection.item.type.safety", "zh-CN", "安全检验", "检验项目类型.安全检验"),
            // dict.logistics.inspection.item.type.safety
            ("dict.logistics.inspection.item.type.safety", "zh-HK", "安全检验", "检验项目类型.安全检验"),

            // dict.logistics.inspection.item.type.environment
            ("dict.logistics.inspection.item.type.environment", "en-US", "environment", "检验项目类型.环境检验"),
            // dict.logistics.inspection.item.type.environment
            ("dict.logistics.inspection.item.type.environment", "ja-JP", "environment", "检验项目类型.环境检验"),
            // dict.logistics.inspection.item.type.environment
            ("dict.logistics.inspection.item.type.environment", "zh-CN", "环境检验", "检验项目类型.环境检验"),
            // dict.logistics.inspection.item.type.environment
            ("dict.logistics.inspection.item.type.environment", "zh-HK", "环境检验", "检验项目类型.环境检验"),

            // dict.logistics.inspection.method.full
            ("dict.logistics.inspection.method.full", "en-US", "full", "检验方式.全检"),
            // dict.logistics.inspection.method.full
            ("dict.logistics.inspection.method.full", "ja-JP", "full", "检验方式.全检"),
            // dict.logistics.inspection.method.full
            ("dict.logistics.inspection.method.full", "zh-CN", "全检", "检验方式.全检"),
            // dict.logistics.inspection.method.full
            ("dict.logistics.inspection.method.full", "zh-HK", "全检", "检验方式.全检"),

            // dict.logistics.inspection.method.sampling
            ("dict.logistics.inspection.method.sampling", "en-US", "sampling", "检验方式.抽样检验"),
            // dict.logistics.inspection.method.sampling
            ("dict.logistics.inspection.method.sampling", "ja-JP", "sampling", "检验方式.抽样检验"),
            // dict.logistics.inspection.method.sampling
            ("dict.logistics.inspection.method.sampling", "zh-CN", "抽样检验", "检验方式.抽样检验"),
            // dict.logistics.inspection.method.sampling
            ("dict.logistics.inspection.method.sampling", "zh-HK", "抽样检验", "检验方式.抽样检验"),

            // dict.logistics.inspection.method.skip
            ("dict.logistics.inspection.method.skip", "en-US", "skip", "检验方式.免检"),
            // dict.logistics.inspection.method.skip
            ("dict.logistics.inspection.method.skip", "ja-JP", "skip", "检验方式.免检"),
            // dict.logistics.inspection.method.skip
            ("dict.logistics.inspection.method.skip", "zh-CN", "免检", "检验方式.免检"),
            // dict.logistics.inspection.method.skip
            ("dict.logistics.inspection.method.skip", "zh-HK", "免检", "检验方式.免检"),

            // dict.logistics.inspection.method.visual
            ("dict.logistics.inspection.method.visual", "en-US", "visual", "检验方式.目视检验"),
            // dict.logistics.inspection.method.visual
            ("dict.logistics.inspection.method.visual", "ja-JP", "visual", "检验方式.目视检验"),
            // dict.logistics.inspection.method.visual
            ("dict.logistics.inspection.method.visual", "zh-CN", "目视检验", "检验方式.目视检验"),
            // dict.logistics.inspection.method.visual
            ("dict.logistics.inspection.method.visual", "zh-HK", "目视检验", "检验方式.目视检验"),

            // dict.logistics.inspection.method.instrument
            ("dict.logistics.inspection.method.instrument", "en-US", "instrument", "检验方式.仪器检验"),
            // dict.logistics.inspection.method.instrument
            ("dict.logistics.inspection.method.instrument", "ja-JP", "instrument", "检验方式.仪器检验"),
            // dict.logistics.inspection.method.instrument
            ("dict.logistics.inspection.method.instrument", "zh-CN", "仪器检验", "检验方式.仪器检验"),
            // dict.logistics.inspection.method.instrument
            ("dict.logistics.inspection.method.instrument", "zh-HK", "仪器检验", "检验方式.仪器检验"),

            // dict.logistics.inspection.method.destructive
            ("dict.logistics.inspection.method.destructive", "en-US", "destructive", "检验方式.破坏性检验"),
            // dict.logistics.inspection.method.destructive
            ("dict.logistics.inspection.method.destructive", "ja-JP", "destructive", "检验方式.破坏性检验"),
            // dict.logistics.inspection.method.destructive
            ("dict.logistics.inspection.method.destructive", "zh-CN", "破坏性检验", "检验方式.破坏性检验"),
            // dict.logistics.inspection.method.destructive
            ("dict.logistics.inspection.method.destructive", "zh-HK", "破坏性检验", "检验方式.破坏性检验"),

            // dict.logistics.inspection.method.non_destructive
            ("dict.logistics.inspection.method.non_destructive", "en-US", "non_destructive", "检验方式.非破坏性检验"),
            // dict.logistics.inspection.method.non_destructive
            ("dict.logistics.inspection.method.non_destructive", "ja-JP", "non_destructive", "检验方式.非破坏性检验"),
            // dict.logistics.inspection.method.non_destructive
            ("dict.logistics.inspection.method.non_destructive", "zh-CN", "非破坏性检验", "检验方式.非破坏性检验"),
            // dict.logistics.inspection.method.non_destructive
            ("dict.logistics.inspection.method.non_destructive", "zh-HK", "非破坏性检验", "检验方式.非破坏性检验"),

            // dict.logistics.inspection.severity.normal
            ("dict.logistics.inspection.severity.normal", "en-US", "normal", "检验严格度.正常检验"),
            // dict.logistics.inspection.severity.normal
            ("dict.logistics.inspection.severity.normal", "ja-JP", "normal", "检验严格度.正常检验"),
            // dict.logistics.inspection.severity.normal
            ("dict.logistics.inspection.severity.normal", "zh-CN", "正常检验", "检验严格度.正常检验"),
            // dict.logistics.inspection.severity.normal
            ("dict.logistics.inspection.severity.normal", "zh-HK", "正常检验", "检验严格度.正常检验"),

            // dict.logistics.inspection.severity.tightened
            ("dict.logistics.inspection.severity.tightened", "en-US", "tightened", "检验严格度.加严检验"),
            // dict.logistics.inspection.severity.tightened
            ("dict.logistics.inspection.severity.tightened", "ja-JP", "tightened", "检验严格度.加严检验"),
            // dict.logistics.inspection.severity.tightened
            ("dict.logistics.inspection.severity.tightened", "zh-CN", "加严检验", "检验严格度.加严检验"),
            // dict.logistics.inspection.severity.tightened
            ("dict.logistics.inspection.severity.tightened", "zh-HK", "加严检验", "检验严格度.加严检验"),

            // dict.logistics.inspection.severity.reduced
            ("dict.logistics.inspection.severity.reduced", "en-US", "reduced", "检验严格度.放宽检验"),
            // dict.logistics.inspection.severity.reduced
            ("dict.logistics.inspection.severity.reduced", "ja-JP", "reduced", "检验严格度.放宽检验"),
            // dict.logistics.inspection.severity.reduced
            ("dict.logistics.inspection.severity.reduced", "zh-CN", "放宽检验", "检验严格度.放宽检验"),
            // dict.logistics.inspection.severity.reduced
            ("dict.logistics.inspection.severity.reduced", "zh-HK", "放宽检验", "检验严格度.放宽检验"),

            // dict.logistics.inspection.tool.caliper
            ("dict.logistics.inspection.tool.caliper", "en-US", "caliper", "检验工具.卡尺"),
            // dict.logistics.inspection.tool.caliper
            ("dict.logistics.inspection.tool.caliper", "ja-JP", "caliper", "检验工具.卡尺"),
            // dict.logistics.inspection.tool.caliper
            ("dict.logistics.inspection.tool.caliper", "zh-CN", "卡尺", "检验工具.卡尺"),
            // dict.logistics.inspection.tool.caliper
            ("dict.logistics.inspection.tool.caliper", "zh-HK", "卡尺", "检验工具.卡尺"),

            // dict.logistics.inspection.tool.micrometer
            ("dict.logistics.inspection.tool.micrometer", "en-US", "micrometer", "检验工具.千分尺"),
            // dict.logistics.inspection.tool.micrometer
            ("dict.logistics.inspection.tool.micrometer", "ja-JP", "micrometer", "检验工具.千分尺"),
            // dict.logistics.inspection.tool.micrometer
            ("dict.logistics.inspection.tool.micrometer", "zh-CN", "千分尺", "检验工具.千分尺"),
            // dict.logistics.inspection.tool.micrometer
            ("dict.logistics.inspection.tool.micrometer", "zh-HK", "千分尺", "检验工具.千分尺"),

            // dict.logistics.inspection.tool.height_gauge
            ("dict.logistics.inspection.tool.height_gauge", "en-US", "height_gauge", "检验工具.高度尺"),
            // dict.logistics.inspection.tool.height_gauge
            ("dict.logistics.inspection.tool.height_gauge", "ja-JP", "height_gauge", "检验工具.高度尺"),
            // dict.logistics.inspection.tool.height_gauge
            ("dict.logistics.inspection.tool.height_gauge", "zh-CN", "高度尺", "检验工具.高度尺"),
            // dict.logistics.inspection.tool.height_gauge
            ("dict.logistics.inspection.tool.height_gauge", "zh-HK", "高度尺", "检验工具.高度尺"),

            // dict.logistics.inspection.tool.feeler_gauge
            ("dict.logistics.inspection.tool.feeler_gauge", "en-US", "feeler_gauge", "检验工具.塞尺"),
            // dict.logistics.inspection.tool.feeler_gauge
            ("dict.logistics.inspection.tool.feeler_gauge", "ja-JP", "feeler_gauge", "检验工具.塞尺"),
            // dict.logistics.inspection.tool.feeler_gauge
            ("dict.logistics.inspection.tool.feeler_gauge", "zh-CN", "塞尺", "检验工具.塞尺"),
            // dict.logistics.inspection.tool.feeler_gauge
            ("dict.logistics.inspection.tool.feeler_gauge", "zh-HK", "塞尺", "检验工具.塞尺"),

            // dict.logistics.inspection.tool.thread_gauge
            ("dict.logistics.inspection.tool.thread_gauge", "en-US", "thread_gauge", "检验工具.螺纹规"),
            // dict.logistics.inspection.tool.thread_gauge
            ("dict.logistics.inspection.tool.thread_gauge", "ja-JP", "thread_gauge", "检验工具.螺纹规"),
            // dict.logistics.inspection.tool.thread_gauge
            ("dict.logistics.inspection.tool.thread_gauge", "zh-CN", "螺纹规", "检验工具.螺纹规"),
            // dict.logistics.inspection.tool.thread_gauge
            ("dict.logistics.inspection.tool.thread_gauge", "zh-HK", "螺纹规", "检验工具.螺纹规"),

            // dict.logistics.inspection.tool.hardness_tester
            ("dict.logistics.inspection.tool.hardness_tester", "en-US", "hardness_tester", "检验工具.硬度计"),
            // dict.logistics.inspection.tool.hardness_tester
            ("dict.logistics.inspection.tool.hardness_tester", "ja-JP", "hardness_tester", "检验工具.硬度计"),
            // dict.logistics.inspection.tool.hardness_tester
            ("dict.logistics.inspection.tool.hardness_tester", "zh-CN", "硬度计", "检验工具.硬度计"),
            // dict.logistics.inspection.tool.hardness_tester
            ("dict.logistics.inspection.tool.hardness_tester", "zh-HK", "硬度计", "检验工具.硬度计"),

            // dict.logistics.inspection.tool.roughness_tester
            ("dict.logistics.inspection.tool.roughness_tester", "en-US", "roughness_tester", "检验工具.粗糙度仪"),
            // dict.logistics.inspection.tool.roughness_tester
            ("dict.logistics.inspection.tool.roughness_tester", "ja-JP", "roughness_tester", "检验工具.粗糙度仪"),
            // dict.logistics.inspection.tool.roughness_tester
            ("dict.logistics.inspection.tool.roughness_tester", "zh-CN", "粗糙度仪", "检验工具.粗糙度仪"),
            // dict.logistics.inspection.tool.roughness_tester
            ("dict.logistics.inspection.tool.roughness_tester", "zh-HK", "粗糙度仪", "检验工具.粗糙度仪"),

            // dict.logistics.inspection.tool.cmm
            ("dict.logistics.inspection.tool.cmm", "en-US", "cmm", "检验工具.三坐标测量机"),
            // dict.logistics.inspection.tool.cmm
            ("dict.logistics.inspection.tool.cmm", "ja-JP", "cmm", "检验工具.三坐标测量机"),
            // dict.logistics.inspection.tool.cmm
            ("dict.logistics.inspection.tool.cmm", "zh-CN", "三坐标测量机", "检验工具.三坐标测量机"),
            // dict.logistics.inspection.tool.cmm
            ("dict.logistics.inspection.tool.cmm", "zh-HK", "三坐标测量机", "检验工具.三坐标测量机"),

            // dict.logistics.inspection.tool.projector
            ("dict.logistics.inspection.tool.projector", "en-US", "projector", "检验工具.投影仪"),
            // dict.logistics.inspection.tool.projector
            ("dict.logistics.inspection.tool.projector", "ja-JP", "projector", "检验工具.投影仪"),
            // dict.logistics.inspection.tool.projector
            ("dict.logistics.inspection.tool.projector", "zh-CN", "投影仪", "检验工具.投影仪"),
            // dict.logistics.inspection.tool.projector
            ("dict.logistics.inspection.tool.projector", "zh-HK", "投影仪", "检验工具.投影仪"),

            // dict.logistics.inspection.tool.tensile_tester
            ("dict.logistics.inspection.tool.tensile_tester", "en-US", "tensile_tester", "检验工具.拉力试验机"),
            // dict.logistics.inspection.tool.tensile_tester
            ("dict.logistics.inspection.tool.tensile_tester", "ja-JP", "tensile_tester", "检验工具.拉力试验机"),
            // dict.logistics.inspection.tool.tensile_tester
            ("dict.logistics.inspection.tool.tensile_tester", "zh-CN", "拉力试验机", "检验工具.拉力试验机"),
            // dict.logistics.inspection.tool.tensile_tester
            ("dict.logistics.inspection.tool.tensile_tester", "zh-HK", "拉力试验机", "检验工具.拉力试验机"),

            // dict.logistics.inspection.tool.multimeter
            ("dict.logistics.inspection.tool.multimeter", "en-US", "multimeter", "检验工具.万用表"),
            // dict.logistics.inspection.tool.multimeter
            ("dict.logistics.inspection.tool.multimeter", "ja-JP", "multimeter", "检验工具.万用表"),
            // dict.logistics.inspection.tool.multimeter
            ("dict.logistics.inspection.tool.multimeter", "zh-CN", "万用表", "检验工具.万用表"),
            // dict.logistics.inspection.tool.multimeter
            ("dict.logistics.inspection.tool.multimeter", "zh-HK", "万用表", "检验工具.万用表"),

            // dict.logistics.inspection.tool.oscilloscope
            ("dict.logistics.inspection.tool.oscilloscope", "en-US", "oscilloscope", "检验工具.示波器"),
            // dict.logistics.inspection.tool.oscilloscope
            ("dict.logistics.inspection.tool.oscilloscope", "ja-JP", "oscilloscope", "检验工具.示波器"),
            // dict.logistics.inspection.tool.oscilloscope
            ("dict.logistics.inspection.tool.oscilloscope", "zh-CN", "示波器", "检验工具.示波器"),
            // dict.logistics.inspection.tool.oscilloscope
            ("dict.logistics.inspection.tool.oscilloscope", "zh-HK", "示波器", "检验工具.示波器"),

            // dict.logistics.inspection.tool.colorimeter
            ("dict.logistics.inspection.tool.colorimeter", "en-US", "colorimeter", "检验工具.色差仪"),
            // dict.logistics.inspection.tool.colorimeter
            ("dict.logistics.inspection.tool.colorimeter", "ja-JP", "colorimeter", "检验工具.色差仪"),
            // dict.logistics.inspection.tool.colorimeter
            ("dict.logistics.inspection.tool.colorimeter", "zh-CN", "色差仪", "检验工具.色差仪"),
            // dict.logistics.inspection.tool.colorimeter
            ("dict.logistics.inspection.tool.colorimeter", "zh-HK", "色差仪", "检验工具.色差仪"),

            // dict.logistics.inspection.tool.glossmeter
            ("dict.logistics.inspection.tool.glossmeter", "en-US", "glossmeter", "检验工具.光泽度计"),
            // dict.logistics.inspection.tool.glossmeter
            ("dict.logistics.inspection.tool.glossmeter", "ja-JP", "glossmeter", "检验工具.光泽度计"),
            // dict.logistics.inspection.tool.glossmeter
            ("dict.logistics.inspection.tool.glossmeter", "zh-CN", "光泽度计", "检验工具.光泽度计"),
            // dict.logistics.inspection.tool.glossmeter
            ("dict.logistics.inspection.tool.glossmeter", "zh-HK", "光泽度计", "检验工具.光泽度计"),

            // dict.logistics.inspection.tool.thickness_gauge
            ("dict.logistics.inspection.tool.thickness_gauge", "en-US", "thickness_gauge", "检验工具.厚度计"),
            // dict.logistics.inspection.tool.thickness_gauge
            ("dict.logistics.inspection.tool.thickness_gauge", "ja-JP", "thickness_gauge", "检验工具.厚度计"),
            // dict.logistics.inspection.tool.thickness_gauge
            ("dict.logistics.inspection.tool.thickness_gauge", "zh-CN", "厚度计", "检验工具.厚度计"),
            // dict.logistics.inspection.tool.thickness_gauge
            ("dict.logistics.inspection.tool.thickness_gauge", "zh-HK", "厚度计", "检验工具.厚度计"),

            // dict.logistics.inspection.type.0
            ("dict.logistics.inspection.type.0", "en-US", "免检", "检验类别.免检"),
            // dict.logistics.inspection.type.0
            ("dict.logistics.inspection.type.0", "ja-JP", "免检", "检验类别.免检"),
            // dict.logistics.inspection.type.0
            ("dict.logistics.inspection.type.0", "zh-CN", "免检", "检验类别.免检"),
            // dict.logistics.inspection.type.0
            ("dict.logistics.inspection.type.0", "zh-HK", "免检", "检验类别.免检"),

            // dict.logistics.inspection.type.1
            ("dict.logistics.inspection.type.1", "en-US", "必检", "检验类别.必检"),
            // dict.logistics.inspection.type.1
            ("dict.logistics.inspection.type.1", "ja-JP", "必检", "检验类别.必检"),
            // dict.logistics.inspection.type.1
            ("dict.logistics.inspection.type.1", "zh-CN", "必检", "检验类别.必检"),
            // dict.logistics.inspection.type.1
            ("dict.logistics.inspection.type.1", "zh-HK", "必检", "检验类别.必检"),

            // dict.logistics.judgment.category.pass
            ("dict.logistics.judgment.category.pass", "en-US", "pass", "判定类别.合格"),
            // dict.logistics.judgment.category.pass
            ("dict.logistics.judgment.category.pass", "ja-JP", "pass", "判定类别.合格"),
            // dict.logistics.judgment.category.pass
            ("dict.logistics.judgment.category.pass", "zh-CN", "合格", "判定类别.合格"),
            // dict.logistics.judgment.category.pass
            ("dict.logistics.judgment.category.pass", "zh-HK", "合格", "判定类别.合格"),

            // dict.logistics.judgment.category.fail
            ("dict.logistics.judgment.category.fail", "en-US", "fail", "判定类别.不合格"),
            // dict.logistics.judgment.category.fail
            ("dict.logistics.judgment.category.fail", "ja-JP", "fail", "判定类别.不合格"),
            // dict.logistics.judgment.category.fail
            ("dict.logistics.judgment.category.fail", "zh-CN", "不合格", "判定类别.不合格"),
            // dict.logistics.judgment.category.fail
            ("dict.logistics.judgment.category.fail", "zh-HK", "不合格", "判定类别.不合格"),

            // dict.logistics.judgment.category.pending
            ("dict.logistics.judgment.category.pending", "en-US", "pending", "判定类别.待判定"),
            // dict.logistics.judgment.category.pending
            ("dict.logistics.judgment.category.pending", "ja-JP", "pending", "判定类别.待判定"),
            // dict.logistics.judgment.category.pending
            ("dict.logistics.judgment.category.pending", "zh-CN", "待判定", "判定类别.待判定"),
            // dict.logistics.judgment.category.pending
            ("dict.logistics.judgment.category.pending", "zh-HK", "待判定", "判定类别.待判定"),

            // dict.logistics.judgment.category.concession
            ("dict.logistics.judgment.category.concession", "en-US", "concession", "判定类别.让步接收"),
            // dict.logistics.judgment.category.concession
            ("dict.logistics.judgment.category.concession", "ja-JP", "concession", "判定类别.让步接收"),
            // dict.logistics.judgment.category.concession
            ("dict.logistics.judgment.category.concession", "zh-CN", "让步接收", "判定类别.让步接收"),
            // dict.logistics.judgment.category.concession
            ("dict.logistics.judgment.category.concession", "zh-HK", "让步接收", "判定类别.让步接收"),

            // dict.logistics.judgment.category.special_accept
            ("dict.logistics.judgment.category.special_accept", "en-US", "special_accept", "判定类别.特采"),
            // dict.logistics.judgment.category.special_accept
            ("dict.logistics.judgment.category.special_accept", "ja-JP", "special_accept", "判定类别.特采"),
            // dict.logistics.judgment.category.special_accept
            ("dict.logistics.judgment.category.special_accept", "zh-CN", "特采", "判定类别.特采"),
            // dict.logistics.judgment.category.special_accept
            ("dict.logistics.judgment.category.special_accept", "zh-HK", "特采", "判定类别.特采"),

            // dict.logistics.judgment.category.return
            ("dict.logistics.judgment.category.return", "en-US", "return", "判定类别.退货"),
            // dict.logistics.judgment.category.return
            ("dict.logistics.judgment.category.return", "ja-JP", "return", "判定类别.退货"),
            // dict.logistics.judgment.category.return
            ("dict.logistics.judgment.category.return", "zh-CN", "退货", "判定类别.退货"),
            // dict.logistics.judgment.category.return
            ("dict.logistics.judgment.category.return", "zh-HK", "退货", "判定类别.退货"),

            // dict.logistics.judgment.category.sorting
            ("dict.logistics.judgment.category.sorting", "en-US", "sorting", "判定类别.挑选使用"),
            // dict.logistics.judgment.category.sorting
            ("dict.logistics.judgment.category.sorting", "ja-JP", "sorting", "判定类别.挑选使用"),
            // dict.logistics.judgment.category.sorting
            ("dict.logistics.judgment.category.sorting", "zh-CN", "挑选使用", "判定类别.挑选使用"),
            // dict.logistics.judgment.category.sorting
            ("dict.logistics.judgment.category.sorting", "zh-HK", "挑选使用", "判定类别.挑选使用"),

            // dict.logistics.judgment.category.rework
            ("dict.logistics.judgment.category.rework", "en-US", "rework", "判定类别.返工"),
            // dict.logistics.judgment.category.rework
            ("dict.logistics.judgment.category.rework", "ja-JP", "rework", "判定类别.返工"),
            // dict.logistics.judgment.category.rework
            ("dict.logistics.judgment.category.rework", "zh-CN", "返工", "判定类别.返工"),
            // dict.logistics.judgment.category.rework
            ("dict.logistics.judgment.category.rework", "zh-HK", "返工", "判定类别.返工"),

            // dict.logistics.judgment.category.scrap
            ("dict.logistics.judgment.category.scrap", "en-US", "scrap", "判定类别.报废"),
            // dict.logistics.judgment.category.scrap
            ("dict.logistics.judgment.category.scrap", "ja-JP", "scrap", "判定类别.报废"),
            // dict.logistics.judgment.category.scrap
            ("dict.logistics.judgment.category.scrap", "zh-CN", "报废", "判定类别.报废"),
            // dict.logistics.judgment.category.scrap
            ("dict.logistics.judgment.category.scrap", "zh-HK", "报废", "判定类别.报废"),

            // dict.logistics.maintenance.category.preventive
            ("dict.logistics.maintenance.category.preventive", "en-US", "preventive", "维护类别.预防性维护"),
            // dict.logistics.maintenance.category.preventive
            ("dict.logistics.maintenance.category.preventive", "ja-JP", "preventive", "维护类别.预防性维护"),
            // dict.logistics.maintenance.category.preventive
            ("dict.logistics.maintenance.category.preventive", "zh-CN", "预防性维护", "维护类别.预防性维护"),
            // dict.logistics.maintenance.category.preventive
            ("dict.logistics.maintenance.category.preventive", "zh-HK", "预防性维护", "维护类别.预防性维护"),

            // dict.logistics.maintenance.category.corrective
            ("dict.logistics.maintenance.category.corrective", "en-US", "corrective", "维护类别. corrective维护"),
            // dict.logistics.maintenance.category.corrective
            ("dict.logistics.maintenance.category.corrective", "ja-JP", "corrective", "维护类别. corrective维护"),
            // dict.logistics.maintenance.category.corrective
            ("dict.logistics.maintenance.category.corrective", "zh-CN", " corrective维护", "维护类别. corrective维护"),
            // dict.logistics.maintenance.category.corrective
            ("dict.logistics.maintenance.category.corrective", "zh-HK", " corrective维护", "维护类别. corrective维护"),

            // dict.logistics.maintenance.category.predictive
            ("dict.logistics.maintenance.category.predictive", "en-US", "predictive", "维护类别.预测性维护"),
            // dict.logistics.maintenance.category.predictive
            ("dict.logistics.maintenance.category.predictive", "ja-JP", "predictive", "维护类别.预测性维护"),
            // dict.logistics.maintenance.category.predictive
            ("dict.logistics.maintenance.category.predictive", "zh-CN", "预测性维护", "维护类别.预测性维护"),
            // dict.logistics.maintenance.category.predictive
            ("dict.logistics.maintenance.category.predictive", "zh-HK", "预测性维护", "维护类别.预测性维护"),

            // dict.logistics.maintenance.category.emergency
            ("dict.logistics.maintenance.category.emergency", "en-US", "emergency", "维护类别.紧急维修"),
            // dict.logistics.maintenance.category.emergency
            ("dict.logistics.maintenance.category.emergency", "ja-JP", "emergency", "维护类别.紧急维修"),
            // dict.logistics.maintenance.category.emergency
            ("dict.logistics.maintenance.category.emergency", "zh-CN", "紧急维修", "维护类别.紧急维修"),
            // dict.logistics.maintenance.category.emergency
            ("dict.logistics.maintenance.category.emergency", "zh-HK", "紧急维修", "维护类别.紧急维修"),

            // dict.logistics.maintenance.category.regular
            ("dict.logistics.maintenance.category.regular", "en-US", "regular", "维护类别.定期保养"),
            // dict.logistics.maintenance.category.regular
            ("dict.logistics.maintenance.category.regular", "ja-JP", "regular", "维护类别.定期保养"),
            // dict.logistics.maintenance.category.regular
            ("dict.logistics.maintenance.category.regular", "zh-CN", "定期保养", "维护类别.定期保养"),
            // dict.logistics.maintenance.category.regular
            ("dict.logistics.maintenance.category.regular", "zh-HK", "定期保养", "维护类别.定期保养"),

            // dict.logistics.maintenance.category.overhaul
            ("dict.logistics.maintenance.category.overhaul", "en-US", "overhaul", "维护类别.大修"),
            // dict.logistics.maintenance.category.overhaul
            ("dict.logistics.maintenance.category.overhaul", "ja-JP", "overhaul", "维护类别.大修"),
            // dict.logistics.maintenance.category.overhaul
            ("dict.logistics.maintenance.category.overhaul", "zh-CN", "大修", "维护类别.大修"),
            // dict.logistics.maintenance.category.overhaul
            ("dict.logistics.maintenance.category.overhaul", "zh-HK", "大修", "维护类别.大修"),

            // dict.logistics.maintenance.category.upgrade
            ("dict.logistics.maintenance.category.upgrade", "en-US", "upgrade", "维护类别.改造升级"),
            // dict.logistics.maintenance.category.upgrade
            ("dict.logistics.maintenance.category.upgrade", "ja-JP", "upgrade", "维护类别.改造升级"),
            // dict.logistics.maintenance.category.upgrade
            ("dict.logistics.maintenance.category.upgrade", "zh-CN", "改造升级", "维护类别.改造升级"),
            // dict.logistics.maintenance.category.upgrade
            ("dict.logistics.maintenance.category.upgrade", "zh-HK", "改造升级", "维护类别.改造升级"),

            // dict.logistics.material.group.avf
            ("dict.logistics.material.group.avf", "en-US", "avf", "物料组.grease"),
            // dict.logistics.material.group.avf
            ("dict.logistics.material.group.avf", "ja-JP", "avf", "物料组.grease"),
            // dict.logistics.material.group.avf
            ("dict.logistics.material.group.avf", "zh-CN", "grease", "物料组.grease"),
            // dict.logistics.material.group.avf
            ("dict.logistics.material.group.avf", "zh-HK", "grease", "物料组.grease"),

            // dict.logistics.material.group.avo
            ("dict.logistics.material.group.avo", "en-US", "avo", "物料组.oil"),
            // dict.logistics.material.group.avo
            ("dict.logistics.material.group.avo", "ja-JP", "avo", "物料组.oil"),
            // dict.logistics.material.group.avo
            ("dict.logistics.material.group.avo", "zh-CN", "oil", "物料组.oil"),
            // dict.logistics.material.group.avo
            ("dict.logistics.material.group.avo", "zh-HK", "oil", "物料组.oil"),

            // dict.logistics.material.group.baa
            ("dict.logistics.material.group.baa", "en-US", "baa", "物料组.screw"),
            // dict.logistics.material.group.baa
            ("dict.logistics.material.group.baa", "ja-JP", "baa", "物料组.screw"),
            // dict.logistics.material.group.baa
            ("dict.logistics.material.group.baa", "zh-CN", "screw", "物料组.screw"),
            // dict.logistics.material.group.baa
            ("dict.logistics.material.group.baa", "zh-HK", "screw", "物料组.screw"),

            // dict.logistics.material.group.bba
            ("dict.logistics.material.group.bba", "en-US", "bba", "物料组.screw,bpa"),
            // dict.logistics.material.group.bba
            ("dict.logistics.material.group.bba", "ja-JP", "bba", "物料组.screw,bpa"),
            // dict.logistics.material.group.bba
            ("dict.logistics.material.group.bba", "zh-CN", "screw,bpa", "物料组.screw,bpa"),
            // dict.logistics.material.group.bba
            ("dict.logistics.material.group.bba", "zh-HK", "screw,bpa", "物料组.screw,bpa"),

            // dict.logistics.material.group.bbb
            ("dict.logistics.material.group.bbb", "en-US", "bbb", "物料组.screw,bpb"),
            // dict.logistics.material.group.bbb
            ("dict.logistics.material.group.bbb", "ja-JP", "bbb", "物料组.screw,bpb"),
            // dict.logistics.material.group.bbb
            ("dict.logistics.material.group.bbb", "zh-CN", "screw,bpb", "物料组.screw,bpb"),
            // dict.logistics.material.group.bbb
            ("dict.logistics.material.group.bbb", "zh-HK", "screw,bpb", "物料组.screw,bpb"),

            // dict.logistics.material.group.bbc
            ("dict.logistics.material.group.bbc", "en-US", "bbc", "物料组.screw,bpc"),
            // dict.logistics.material.group.bbc
            ("dict.logistics.material.group.bbc", "ja-JP", "bbc", "物料组.screw,bpc"),
            // dict.logistics.material.group.bbc
            ("dict.logistics.material.group.bbc", "zh-CN", "screw,bpc", "物料组.screw,bpc"),
            // dict.logistics.material.group.bbc
            ("dict.logistics.material.group.bbc", "zh-HK", "screw,bpc", "物料组.screw,bpc"),

            // dict.logistics.material.group.bbf
            ("dict.logistics.material.group.bbf", "en-US", "bbf", "物料组.screw,bpf"),
            // dict.logistics.material.group.bbf
            ("dict.logistics.material.group.bbf", "ja-JP", "bbf", "物料组.screw,bpf"),
            // dict.logistics.material.group.bbf
            ("dict.logistics.material.group.bbf", "zh-CN", "screw,bpf", "物料组.screw,bpf"),
            // dict.logistics.material.group.bbf
            ("dict.logistics.material.group.bbf", "zh-HK", "screw,bpf", "物料组.screw,bpf"),

            // dict.logistics.material.group.bbg
            ("dict.logistics.material.group.bbg", "en-US", "bbg", "物料组.screw,bpg"),
            // dict.logistics.material.group.bbg
            ("dict.logistics.material.group.bbg", "ja-JP", "bbg", "物料组.screw,bpg"),
            // dict.logistics.material.group.bbg
            ("dict.logistics.material.group.bbg", "zh-CN", "screw,bpg", "物料组.screw,bpg"),
            // dict.logistics.material.group.bbg
            ("dict.logistics.material.group.bbg", "zh-HK", "screw,bpg", "物料组.screw,bpg"),

            // dict.logistics.material.group.bbh
            ("dict.logistics.material.group.bbh", "en-US", "bbh", "物料组.screw,bph"),
            // dict.logistics.material.group.bbh
            ("dict.logistics.material.group.bbh", "ja-JP", "bbh", "物料组.screw,bph"),
            // dict.logistics.material.group.bbh
            ("dict.logistics.material.group.bbh", "zh-CN", "screw,bph", "物料组.screw,bph"),
            // dict.logistics.material.group.bbh
            ("dict.logistics.material.group.bbh", "zh-HK", "screw,bph", "物料组.screw,bph"),

            // dict.logistics.material.group.bbj
            ("dict.logistics.material.group.bbj", "en-US", "bbj", "物料组.screw,bpj"),
            // dict.logistics.material.group.bbj
            ("dict.logistics.material.group.bbj", "ja-JP", "bbj", "物料组.screw,bpj"),
            // dict.logistics.material.group.bbj
            ("dict.logistics.material.group.bbj", "zh-CN", "screw,bpj", "物料组.screw,bpj"),
            // dict.logistics.material.group.bbj
            ("dict.logistics.material.group.bbj", "zh-HK", "screw,bpj", "物料组.screw,bpj"),

            // dict.logistics.material.group.bbk
            ("dict.logistics.material.group.bbk", "en-US", "bbk", "物料组.screw,bpk"),
            // dict.logistics.material.group.bbk
            ("dict.logistics.material.group.bbk", "ja-JP", "bbk", "物料组.screw,bpk"),
            // dict.logistics.material.group.bbk
            ("dict.logistics.material.group.bbk", "zh-CN", "screw,bpk", "物料组.screw,bpk"),
            // dict.logistics.material.group.bbk
            ("dict.logistics.material.group.bbk", "zh-HK", "screw,bpk", "物料组.screw,bpk"),

            // dict.logistics.material.group.bbl
            ("dict.logistics.material.group.bbl", "en-US", "bbl", "物料组.screw,bpl"),
            // dict.logistics.material.group.bbl
            ("dict.logistics.material.group.bbl", "ja-JP", "bbl", "物料组.screw,bpl"),
            // dict.logistics.material.group.bbl
            ("dict.logistics.material.group.bbl", "zh-CN", "screw,bpl", "物料组.screw,bpl"),
            // dict.logistics.material.group.bbl
            ("dict.logistics.material.group.bbl", "zh-HK", "screw,bpl", "物料组.screw,bpl"),

            // dict.logistics.material.group.bbp
            ("dict.logistics.material.group.bbp", "en-US", "bbp", "物料组.screw,bpp"),
            // dict.logistics.material.group.bbp
            ("dict.logistics.material.group.bbp", "ja-JP", "bbp", "物料组.screw,bpp"),
            // dict.logistics.material.group.bbp
            ("dict.logistics.material.group.bbp", "zh-CN", "screw,bpp", "物料组.screw,bpp"),
            // dict.logistics.material.group.bbp
            ("dict.logistics.material.group.bbp", "zh-HK", "screw,bpp", "物料组.screw,bpp"),

            // dict.logistics.material.group.bbq
            ("dict.logistics.material.group.bbq", "en-US", "bbq", "物料组.screw,bpq"),
            // dict.logistics.material.group.bbq
            ("dict.logistics.material.group.bbq", "ja-JP", "bbq", "物料组.screw,bpq"),
            // dict.logistics.material.group.bbq
            ("dict.logistics.material.group.bbq", "zh-CN", "screw,bpq", "物料组.screw,bpq"),
            // dict.logistics.material.group.bbq
            ("dict.logistics.material.group.bbq", "zh-HK", "screw,bpq", "物料组.screw,bpq"),

            // dict.logistics.material.group.bbs
            ("dict.logistics.material.group.bbs", "en-US", "bbs", "物料组.screw,bps"),
            // dict.logistics.material.group.bbs
            ("dict.logistics.material.group.bbs", "ja-JP", "bbs", "物料组.screw,bps"),
            // dict.logistics.material.group.bbs
            ("dict.logistics.material.group.bbs", "zh-CN", "screw,bps", "物料组.screw,bps"),
            // dict.logistics.material.group.bbs
            ("dict.logistics.material.group.bbs", "zh-HK", "screw,bps", "物料组.screw,bps"),

            // dict.logistics.material.group.bbt
            ("dict.logistics.material.group.bbt", "en-US", "bbt", "物料组.screw,bpt"),
            // dict.logistics.material.group.bbt
            ("dict.logistics.material.group.bbt", "ja-JP", "bbt", "物料组.screw,bpt"),
            // dict.logistics.material.group.bbt
            ("dict.logistics.material.group.bbt", "zh-CN", "screw,bpt", "物料组.screw,bpt"),
            // dict.logistics.material.group.bbt
            ("dict.logistics.material.group.bbt", "zh-HK", "screw,bpt", "物料组.screw,bpt"),

            // dict.logistics.material.group.bbv
            ("dict.logistics.material.group.bbv", "en-US", "bbv", "物料组.screw,bpv"),
            // dict.logistics.material.group.bbv
            ("dict.logistics.material.group.bbv", "ja-JP", "bbv", "物料组.screw,bpv"),
            // dict.logistics.material.group.bbv
            ("dict.logistics.material.group.bbv", "zh-CN", "screw,bpv", "物料组.screw,bpv"),
            // dict.logistics.material.group.bbv
            ("dict.logistics.material.group.bbv", "zh-HK", "screw,bpv", "物料组.screw,bpv"),

            // dict.logistics.material.group.bbz
            ("dict.logistics.material.group.bbz", "en-US", "bbz", "物料组.screw,b"),
            // dict.logistics.material.group.bbz
            ("dict.logistics.material.group.bbz", "ja-JP", "bbz", "物料组.screw,b"),
            // dict.logistics.material.group.bbz
            ("dict.logistics.material.group.bbz", "zh-CN", "screw,b", "物料组.screw,b"),
            // dict.logistics.material.group.bbz
            ("dict.logistics.material.group.bbz", "zh-HK", "screw,b", "物料组.screw,b"),

            // dict.logistics.material.group.bca
            ("dict.logistics.material.group.bca", "en-US", "bca", "物料组.screw,cpa"),
            // dict.logistics.material.group.bca
            ("dict.logistics.material.group.bca", "ja-JP", "bca", "物料组.screw,cpa"),
            // dict.logistics.material.group.bca
            ("dict.logistics.material.group.bca", "zh-CN", "screw,cpa", "物料组.screw,cpa"),
            // dict.logistics.material.group.bca
            ("dict.logistics.material.group.bca", "zh-HK", "screw,cpa", "物料组.screw,cpa"),

            // dict.logistics.material.group.bcf
            ("dict.logistics.material.group.bcf", "en-US", "bcf", "物料组.screw,cpf"),
            // dict.logistics.material.group.bcf
            ("dict.logistics.material.group.bcf", "ja-JP", "bcf", "物料组.screw,cpf"),
            // dict.logistics.material.group.bcf
            ("dict.logistics.material.group.bcf", "zh-CN", "screw,cpf", "物料组.screw,cpf"),
            // dict.logistics.material.group.bcf
            ("dict.logistics.material.group.bcf", "zh-HK", "screw,cpf", "物料组.screw,cpf"),

            // dict.logistics.material.group.bcz
            ("dict.logistics.material.group.bcz", "en-US", "bcz", "物料组.screw,c"),
            // dict.logistics.material.group.bcz
            ("dict.logistics.material.group.bcz", "ja-JP", "bcz", "物料组.screw,c"),
            // dict.logistics.material.group.bcz
            ("dict.logistics.material.group.bcz", "zh-CN", "screw,c", "物料组.screw,c"),
            // dict.logistics.material.group.bcz
            ("dict.logistics.material.group.bcz", "zh-HK", "screw,c", "物料组.screw,c"),

            // dict.logistics.material.group.bda
            ("dict.logistics.material.group.bda", "en-US", "bda", "物料组.screw,dpa"),
            // dict.logistics.material.group.bda
            ("dict.logistics.material.group.bda", "ja-JP", "bda", "物料组.screw,dpa"),
            // dict.logistics.material.group.bda
            ("dict.logistics.material.group.bda", "zh-CN", "screw,dpa", "物料组.screw,dpa"),
            // dict.logistics.material.group.bda
            ("dict.logistics.material.group.bda", "zh-HK", "screw,dpa", "物料组.screw,dpa"),

            // dict.logistics.material.group.bdf
            ("dict.logistics.material.group.bdf", "en-US", "bdf", "物料组.screw,dpf"),
            // dict.logistics.material.group.bdf
            ("dict.logistics.material.group.bdf", "ja-JP", "bdf", "物料组.screw,dpf"),
            // dict.logistics.material.group.bdf
            ("dict.logistics.material.group.bdf", "zh-CN", "screw,dpf", "物料组.screw,dpf"),
            // dict.logistics.material.group.bdf
            ("dict.logistics.material.group.bdf", "zh-HK", "screw,dpf", "物料组.screw,dpf"),

            // dict.logistics.material.group.bdz
            ("dict.logistics.material.group.bdz", "en-US", "bdz", "物料组.screw,d"),
            // dict.logistics.material.group.bdz
            ("dict.logistics.material.group.bdz", "ja-JP", "bdz", "物料组.screw,d"),
            // dict.logistics.material.group.bdz
            ("dict.logistics.material.group.bdz", "zh-CN", "screw,d", "物料组.screw,d"),
            // dict.logistics.material.group.bdz
            ("dict.logistics.material.group.bdz", "zh-HK", "screw,d", "物料组.screw,d"),

            // dict.logistics.material.group.bfa
            ("dict.logistics.material.group.bfa", "en-US", "bfa", "物料组.screw,fpa"),
            // dict.logistics.material.group.bfa
            ("dict.logistics.material.group.bfa", "ja-JP", "bfa", "物料组.screw,fpa"),
            // dict.logistics.material.group.bfa
            ("dict.logistics.material.group.bfa", "zh-CN", "screw,fpa", "物料组.screw,fpa"),
            // dict.logistics.material.group.bfa
            ("dict.logistics.material.group.bfa", "zh-HK", "screw,fpa", "物料组.screw,fpa"),

            // dict.logistics.material.group.bfb
            ("dict.logistics.material.group.bfb", "en-US", "bfb", "物料组.screw,fpb"),
            // dict.logistics.material.group.bfb
            ("dict.logistics.material.group.bfb", "ja-JP", "bfb", "物料组.screw,fpb"),
            // dict.logistics.material.group.bfb
            ("dict.logistics.material.group.bfb", "zh-CN", "screw,fpb", "物料组.screw,fpb"),
            // dict.logistics.material.group.bfb
            ("dict.logistics.material.group.bfb", "zh-HK", "screw,fpb", "物料组.screw,fpb"),

            // dict.logistics.material.group.bfg
            ("dict.logistics.material.group.bfg", "en-US", "bfg", "物料组.screw,fpg"),
            // dict.logistics.material.group.bfg
            ("dict.logistics.material.group.bfg", "ja-JP", "bfg", "物料组.screw,fpg"),
            // dict.logistics.material.group.bfg
            ("dict.logistics.material.group.bfg", "zh-CN", "screw,fpg", "物料组.screw,fpg"),
            // dict.logistics.material.group.bfg
            ("dict.logistics.material.group.bfg", "zh-HK", "screw,fpg", "物料组.screw,fpg"),

            // dict.logistics.material.group.bfh
            ("dict.logistics.material.group.bfh", "en-US", "bfh", "物料组.screw,fph"),
            // dict.logistics.material.group.bfh
            ("dict.logistics.material.group.bfh", "ja-JP", "bfh", "物料组.screw,fph"),
            // dict.logistics.material.group.bfh
            ("dict.logistics.material.group.bfh", "zh-CN", "screw,fph", "物料组.screw,fph"),
            // dict.logistics.material.group.bfh
            ("dict.logistics.material.group.bfh", "zh-HK", "screw,fph", "物料组.screw,fph"),

            // dict.logistics.material.group.bfj
            ("dict.logistics.material.group.bfj", "en-US", "bfj", "物料组.screw,fpj"),
            // dict.logistics.material.group.bfj
            ("dict.logistics.material.group.bfj", "ja-JP", "bfj", "物料组.screw,fpj"),
            // dict.logistics.material.group.bfj
            ("dict.logistics.material.group.bfj", "zh-CN", "screw,fpj", "物料组.screw,fpj"),
            // dict.logistics.material.group.bfj
            ("dict.logistics.material.group.bfj", "zh-HK", "screw,fpj", "物料组.screw,fpj"),

            // dict.logistics.material.group.bfl
            ("dict.logistics.material.group.bfl", "en-US", "bfl", "物料组.screw,fpl"),
            // dict.logistics.material.group.bfl
            ("dict.logistics.material.group.bfl", "ja-JP", "bfl", "物料组.screw,fpl"),
            // dict.logistics.material.group.bfl
            ("dict.logistics.material.group.bfl", "zh-CN", "screw,fpl", "物料组.screw,fpl"),
            // dict.logistics.material.group.bfl
            ("dict.logistics.material.group.bfl", "zh-HK", "screw,fpl", "物料组.screw,fpl"),

            // dict.logistics.material.group.bfp
            ("dict.logistics.material.group.bfp", "en-US", "bfp", "物料组.screw,fpp"),
            // dict.logistics.material.group.bfp
            ("dict.logistics.material.group.bfp", "ja-JP", "bfp", "物料组.screw,fpp"),
            // dict.logistics.material.group.bfp
            ("dict.logistics.material.group.bfp", "zh-CN", "screw,fpp", "物料组.screw,fpp"),
            // dict.logistics.material.group.bfp
            ("dict.logistics.material.group.bfp", "zh-HK", "screw,fpp", "物料组.screw,fpp"),

            // dict.logistics.material.group.bfs
            ("dict.logistics.material.group.bfs", "en-US", "bfs", "物料组.screw,fps"),
            // dict.logistics.material.group.bfs
            ("dict.logistics.material.group.bfs", "ja-JP", "bfs", "物料组.screw,fps"),
            // dict.logistics.material.group.bfs
            ("dict.logistics.material.group.bfs", "zh-CN", "screw,fps", "物料组.screw,fps"),
            // dict.logistics.material.group.bfs
            ("dict.logistics.material.group.bfs", "zh-HK", "screw,fps", "物料组.screw,fps"),

            // dict.logistics.material.group.bfv
            ("dict.logistics.material.group.bfv", "en-US", "bfv", "物料组.screw,fpv"),
            // dict.logistics.material.group.bfv
            ("dict.logistics.material.group.bfv", "ja-JP", "bfv", "物料组.screw,fpv"),
            // dict.logistics.material.group.bfv
            ("dict.logistics.material.group.bfv", "zh-CN", "screw,fpv", "物料组.screw,fpv"),
            // dict.logistics.material.group.bfv
            ("dict.logistics.material.group.bfv", "zh-HK", "screw,fpv", "物料组.screw,fpv"),

            // dict.logistics.material.group.bfz
            ("dict.logistics.material.group.bfz", "en-US", "bfz", "物料组.screw,f"),
            // dict.logistics.material.group.bfz
            ("dict.logistics.material.group.bfz", "ja-JP", "bfz", "物料组.screw,f"),
            // dict.logistics.material.group.bfz
            ("dict.logistics.material.group.bfz", "zh-CN", "screw,f", "物料组.screw,f"),
            // dict.logistics.material.group.bfz
            ("dict.logistics.material.group.bfz", "zh-HK", "screw,f", "物料组.screw,f"),

            // dict.logistics.material.group.bga
            ("dict.logistics.material.group.bga", "en-US", "bga", "物料组.screw,jha"),
            // dict.logistics.material.group.bga
            ("dict.logistics.material.group.bga", "ja-JP", "bga", "物料组.screw,jha"),
            // dict.logistics.material.group.bga
            ("dict.logistics.material.group.bga", "zh-CN", "screw,jha", "物料组.screw,jha"),
            // dict.logistics.material.group.bga
            ("dict.logistics.material.group.bga", "zh-HK", "screw,jha", "物料组.screw,jha"),

            // dict.logistics.material.group.bgf
            ("dict.logistics.material.group.bgf", "en-US", "bgf", "物料组.screw,jhf"),
            // dict.logistics.material.group.bgf
            ("dict.logistics.material.group.bgf", "ja-JP", "bgf", "物料组.screw,jhf"),
            // dict.logistics.material.group.bgf
            ("dict.logistics.material.group.bgf", "zh-CN", "screw,jhf", "物料组.screw,jhf"),
            // dict.logistics.material.group.bgf
            ("dict.logistics.material.group.bgf", "zh-HK", "screw,jhf", "物料组.screw,jhf"),

            // dict.logistics.material.group.bgz
            ("dict.logistics.material.group.bgz", "en-US", "bgz", "物料组.screw,j"),
            // dict.logistics.material.group.bgz
            ("dict.logistics.material.group.bgz", "ja-JP", "bgz", "物料组.screw,j"),
            // dict.logistics.material.group.bgz
            ("dict.logistics.material.group.bgz", "zh-CN", "screw,j", "物料组.screw,j"),
            // dict.logistics.material.group.bgz
            ("dict.logistics.material.group.bgz", "zh-HK", "screw,j", "物料组.screw,j"),

            // dict.logistics.material.group.bha
            ("dict.logistics.material.group.bha", "en-US", "bha", "物料组.bolt,hya"),
            // dict.logistics.material.group.bha
            ("dict.logistics.material.group.bha", "ja-JP", "bha", "物料组.bolt,hya"),
            // dict.logistics.material.group.bha
            ("dict.logistics.material.group.bha", "zh-CN", "bolt,hya", "物料组.bolt,hya"),
            // dict.logistics.material.group.bha
            ("dict.logistics.material.group.bha", "zh-HK", "bolt,hya", "物料组.bolt,hya"),

            // dict.logistics.material.group.bhb
            ("dict.logistics.material.group.bhb", "en-US", "bhb", "物料组.bolt,hyb"),
            // dict.logistics.material.group.bhb
            ("dict.logistics.material.group.bhb", "ja-JP", "bhb", "物料组.bolt,hyb"),
            // dict.logistics.material.group.bhb
            ("dict.logistics.material.group.bhb", "zh-CN", "bolt,hyb", "物料组.bolt,hyb"),
            // dict.logistics.material.group.bhb
            ("dict.logistics.material.group.bhb", "zh-HK", "bolt,hyb", "物料组.bolt,hyb"),

            // dict.logistics.material.group.bhc
            ("dict.logistics.material.group.bhc", "en-US", "bhc", "物料组.bolt,hyc"),
            // dict.logistics.material.group.bhc
            ("dict.logistics.material.group.bhc", "ja-JP", "bhc", "物料组.bolt,hyc"),
            // dict.logistics.material.group.bhc
            ("dict.logistics.material.group.bhc", "zh-CN", "bolt,hyc", "物料组.bolt,hyc"),
            // dict.logistics.material.group.bhc
            ("dict.logistics.material.group.bhc", "zh-HK", "bolt,hyc", "物料组.bolt,hyc"),

            // dict.logistics.material.group.bhf
            ("dict.logistics.material.group.bhf", "en-US", "bhf", "物料组.bolt,hyf"),
            // dict.logistics.material.group.bhf
            ("dict.logistics.material.group.bhf", "ja-JP", "bhf", "物料组.bolt,hyf"),
            // dict.logistics.material.group.bhf
            ("dict.logistics.material.group.bhf", "zh-CN", "bolt,hyf", "物料组.bolt,hyf"),
            // dict.logistics.material.group.bhf
            ("dict.logistics.material.group.bhf", "zh-HK", "bolt,hyf", "物料组.bolt,hyf"),

            // dict.logistics.material.group.bhz
            ("dict.logistics.material.group.bhz", "en-US", "bhz", "物料组.bolt,h"),
            // dict.logistics.material.group.bhz
            ("dict.logistics.material.group.bhz", "ja-JP", "bhz", "物料组.bolt,h"),
            // dict.logistics.material.group.bhz
            ("dict.logistics.material.group.bhz", "zh-CN", "bolt,h", "物料组.bolt,h"),
            // dict.logistics.material.group.bhz
            ("dict.logistics.material.group.bhz", "zh-HK", "bolt,h", "物料组.bolt,h"),

            // dict.logistics.material.group.bja
            ("dict.logistics.material.group.bja", "en-US", "bja", "物料组.screw,ppaa"),
            // dict.logistics.material.group.bja
            ("dict.logistics.material.group.bja", "ja-JP", "bja", "物料组.screw,ppaa"),
            // dict.logistics.material.group.bja
            ("dict.logistics.material.group.bja", "zh-CN", "screw,ppaa", "物料组.screw,ppaa"),
            // dict.logistics.material.group.bja
            ("dict.logistics.material.group.bja", "zh-HK", "screw,ppaa", "物料组.screw,ppaa"),

            // dict.logistics.material.group.bjb
            ("dict.logistics.material.group.bjb", "en-US", "bjb", "物料组.screw,ppab"),
            // dict.logistics.material.group.bjb
            ("dict.logistics.material.group.bjb", "ja-JP", "bjb", "物料组.screw,ppab"),
            // dict.logistics.material.group.bjb
            ("dict.logistics.material.group.bjb", "zh-CN", "screw,ppab", "物料组.screw,ppab"),
            // dict.logistics.material.group.bjb
            ("dict.logistics.material.group.bjb", "zh-HK", "screw,ppab", "物料组.screw,ppab"),

            // dict.logistics.material.group.bjc
            ("dict.logistics.material.group.bjc", "en-US", "bjc", "物料组.screw,ppac"),
            // dict.logistics.material.group.bjc
            ("dict.logistics.material.group.bjc", "ja-JP", "bjc", "物料组.screw,ppac"),
            // dict.logistics.material.group.bjc
            ("dict.logistics.material.group.bjc", "zh-CN", "screw,ppac", "物料组.screw,ppac"),
            // dict.logistics.material.group.bjc
            ("dict.logistics.material.group.bjc", "zh-HK", "screw,ppac", "物料组.screw,ppac"),

            // dict.logistics.material.group.bjf
            ("dict.logistics.material.group.bjf", "en-US", "bjf", "物料组.screw,ppaf"),
            // dict.logistics.material.group.bjf
            ("dict.logistics.material.group.bjf", "ja-JP", "bjf", "物料组.screw,ppaf"),
            // dict.logistics.material.group.bjf
            ("dict.logistics.material.group.bjf", "zh-CN", "screw,ppaf", "物料组.screw,ppaf"),
            // dict.logistics.material.group.bjf
            ("dict.logistics.material.group.bjf", "zh-HK", "screw,ppaf", "物料组.screw,ppaf"),

            // dict.logistics.material.group.bjk
            ("dict.logistics.material.group.bjk", "en-US", "bjk", "物料组.screw,ppak"),
            // dict.logistics.material.group.bjk
            ("dict.logistics.material.group.bjk", "ja-JP", "bjk", "物料组.screw,ppak"),
            // dict.logistics.material.group.bjk
            ("dict.logistics.material.group.bjk", "zh-CN", "screw,ppak", "物料组.screw,ppak"),
            // dict.logistics.material.group.bjk
            ("dict.logistics.material.group.bjk", "zh-HK", "screw,ppak", "物料组.screw,ppak"),

            // dict.logistics.material.group.bjz
            ("dict.logistics.material.group.bjz", "en-US", "bjz", "物料组.screw,p"),
            // dict.logistics.material.group.bjz
            ("dict.logistics.material.group.bjz", "ja-JP", "bjz", "物料组.screw,p"),
            // dict.logistics.material.group.bjz
            ("dict.logistics.material.group.bjz", "zh-CN", "screw,p", "物料组.screw,p"),
            // dict.logistics.material.group.bjz
            ("dict.logistics.material.group.bjz", "zh-HK", "screw,p", "物料组.screw,p"),

            // dict.logistics.material.group.bka
            ("dict.logistics.material.group.bka", "en-US", "bka", "物料组.screw,bpaa"),
            // dict.logistics.material.group.bka
            ("dict.logistics.material.group.bka", "ja-JP", "bka", "物料组.screw,bpaa"),
            // dict.logistics.material.group.bka
            ("dict.logistics.material.group.bka", "zh-CN", "screw,bpaa", "物料组.screw,bpaa"),
            // dict.logistics.material.group.bka
            ("dict.logistics.material.group.bka", "zh-HK", "screw,bpaa", "物料组.screw,bpaa"),

            // dict.logistics.material.group.bkb
            ("dict.logistics.material.group.bkb", "en-US", "bkb", "物料组.screw,bpab"),
            // dict.logistics.material.group.bkb
            ("dict.logistics.material.group.bkb", "ja-JP", "bkb", "物料组.screw,bpab"),
            // dict.logistics.material.group.bkb
            ("dict.logistics.material.group.bkb", "zh-CN", "screw,bpab", "物料组.screw,bpab"),
            // dict.logistics.material.group.bkb
            ("dict.logistics.material.group.bkb", "zh-HK", "screw,bpab", "物料组.screw,bpab"),

            // dict.logistics.material.group.bkc
            ("dict.logistics.material.group.bkc", "en-US", "bkc", "物料组.screw,bpac"),
            // dict.logistics.material.group.bkc
            ("dict.logistics.material.group.bkc", "ja-JP", "bkc", "物料组.screw,bpac"),
            // dict.logistics.material.group.bkc
            ("dict.logistics.material.group.bkc", "zh-CN", "screw,bpac", "物料组.screw,bpac"),
            // dict.logistics.material.group.bkc
            ("dict.logistics.material.group.bkc", "zh-HK", "screw,bpac", "物料组.screw,bpac"),

            // dict.logistics.material.group.bkf
            ("dict.logistics.material.group.bkf", "en-US", "bkf", "物料组.screw,bpaf"),
            // dict.logistics.material.group.bkf
            ("dict.logistics.material.group.bkf", "ja-JP", "bkf", "物料组.screw,bpaf"),
            // dict.logistics.material.group.bkf
            ("dict.logistics.material.group.bkf", "zh-CN", "screw,bpaf", "物料组.screw,bpaf"),
            // dict.logistics.material.group.bkf
            ("dict.logistics.material.group.bkf", "zh-HK", "screw,bpaf", "物料组.screw,bpaf"),

            // dict.logistics.material.group.bkk
            ("dict.logistics.material.group.bkk", "en-US", "bkk", "物料组.screw,bpak"),
            // dict.logistics.material.group.bkk
            ("dict.logistics.material.group.bkk", "ja-JP", "bkk", "物料组.screw,bpak"),
            // dict.logistics.material.group.bkk
            ("dict.logistics.material.group.bkk", "zh-CN", "screw,bpak", "物料组.screw,bpak"),
            // dict.logistics.material.group.bkk
            ("dict.logistics.material.group.bkk", "zh-HK", "screw,bpak", "物料组.screw,bpak"),

            // dict.logistics.material.group.bkw
            ("dict.logistics.material.group.bkw", "en-US", "bkw", "物料组.screw,bpaw"),
            // dict.logistics.material.group.bkw
            ("dict.logistics.material.group.bkw", "ja-JP", "bkw", "物料组.screw,bpaw"),
            // dict.logistics.material.group.bkw
            ("dict.logistics.material.group.bkw", "zh-CN", "screw,bpaw", "物料组.screw,bpaw"),
            // dict.logistics.material.group.bkw
            ("dict.logistics.material.group.bkw", "zh-HK", "screw,bpaw", "物料组.screw,bpaw"),

            // dict.logistics.material.group.blc
            ("dict.logistics.material.group.blc", "en-US", "blc", "物料组.screw,fpaz"),
            // dict.logistics.material.group.blc
            ("dict.logistics.material.group.blc", "ja-JP", "blc", "物料组.screw,fpaz"),
            // dict.logistics.material.group.blc
            ("dict.logistics.material.group.blc", "zh-CN", "screw,fpaz", "物料组.screw,fpaz"),
            // dict.logistics.material.group.blc
            ("dict.logistics.material.group.blc", "zh-HK", "screw,fpaz", "物料组.screw,fpaz"),

            // dict.logistics.material.group.bma
            ("dict.logistics.material.group.bma", "en-US", "bma", "物料组.screw,mpac"),
            // dict.logistics.material.group.bma
            ("dict.logistics.material.group.bma", "ja-JP", "bma", "物料组.screw,mpac"),
            // dict.logistics.material.group.bma
            ("dict.logistics.material.group.bma", "zh-CN", "screw,mpac", "物料组.screw,mpac"),
            // dict.logistics.material.group.bma
            ("dict.logistics.material.group.bma", "zh-HK", "screw,mpac", "物料组.screw,mpac"),

            // dict.logistics.material.group.bmb
            ("dict.logistics.material.group.bmb", "en-US", "bmb", "物料组.screw,mpad"),
            // dict.logistics.material.group.bmb
            ("dict.logistics.material.group.bmb", "ja-JP", "bmb", "物料组.screw,mpad"),
            // dict.logistics.material.group.bmb
            ("dict.logistics.material.group.bmb", "zh-CN", "screw,mpad", "物料组.screw,mpad"),
            // dict.logistics.material.group.bmb
            ("dict.logistics.material.group.bmb", "zh-HK", "screw,mpad", "物料组.screw,mpad"),

            // dict.logistics.material.group.bmc
            ("dict.logistics.material.group.bmc", "en-US", "bmc", "物料组.screw,mpan"),
            // dict.logistics.material.group.bmc
            ("dict.logistics.material.group.bmc", "ja-JP", "bmc", "物料组.screw,mpan"),
            // dict.logistics.material.group.bmc
            ("dict.logistics.material.group.bmc", "zh-CN", "screw,mpan", "物料组.screw,mpan"),
            // dict.logistics.material.group.bmc
            ("dict.logistics.material.group.bmc", "zh-HK", "screw,mpan", "物料组.screw,mpan"),

            // dict.logistics.material.group.bmd
            ("dict.logistics.material.group.bmd", "en-US", "bmd", "物料组.screw,mpao"),
            // dict.logistics.material.group.bmd
            ("dict.logistics.material.group.bmd", "ja-JP", "bmd", "物料组.screw,mpao"),
            // dict.logistics.material.group.bmd
            ("dict.logistics.material.group.bmd", "zh-CN", "screw,mpao", "物料组.screw,mpao"),
            // dict.logistics.material.group.bmd
            ("dict.logistics.material.group.bmd", "zh-HK", "screw,mpao", "物料组.screw,mpao"),

            // dict.logistics.material.group.bme
            ("dict.logistics.material.group.bme", "en-US", "bme", "物料组.screw,mpap"),
            // dict.logistics.material.group.bme
            ("dict.logistics.material.group.bme", "ja-JP", "bme", "物料组.screw,mpap"),
            // dict.logistics.material.group.bme
            ("dict.logistics.material.group.bme", "zh-CN", "screw,mpap", "物料组.screw,mpap"),
            // dict.logistics.material.group.bme
            ("dict.logistics.material.group.bme", "zh-HK", "screw,mpap", "物料组.screw,mpap"),

            // dict.logistics.material.group.bmf
            ("dict.logistics.material.group.bmf", "en-US", "bmf", "物料组.screw,mpaq"),
            // dict.logistics.material.group.bmf
            ("dict.logistics.material.group.bmf", "ja-JP", "bmf", "物料组.screw,mpaq"),
            // dict.logistics.material.group.bmf
            ("dict.logistics.material.group.bmf", "zh-CN", "screw,mpaq", "物料组.screw,mpaq"),
            // dict.logistics.material.group.bmf
            ("dict.logistics.material.group.bmf", "zh-HK", "screw,mpaq", "物料组.screw,mpaq"),

            // dict.logistics.material.group.bmg
            ("dict.logistics.material.group.bmg", "en-US", "bmg", "物料组.screw,mpar"),
            // dict.logistics.material.group.bmg
            ("dict.logistics.material.group.bmg", "ja-JP", "bmg", "物料组.screw,mpar"),
            // dict.logistics.material.group.bmg
            ("dict.logistics.material.group.bmg", "zh-CN", "screw,mpar", "物料组.screw,mpar"),
            // dict.logistics.material.group.bmg
            ("dict.logistics.material.group.bmg", "zh-HK", "screw,mpar", "物料组.screw,mpar"),

            // dict.logistics.material.group.bmz
            ("dict.logistics.material.group.bmz", "en-US", "bmz", "物料组.screw,m"),
            // dict.logistics.material.group.bmz
            ("dict.logistics.material.group.bmz", "ja-JP", "bmz", "物料组.screw,m"),
            // dict.logistics.material.group.bmz
            ("dict.logistics.material.group.bmz", "zh-CN", "screw,m", "物料组.screw,m"),
            // dict.logistics.material.group.bmz
            ("dict.logistics.material.group.bmz", "zh-HK", "screw,m", "物料组.screw,m"),

            // dict.logistics.material.group.bna
            ("dict.logistics.material.group.bna", "en-US", "bna", "物料组.screw,mptc"),
            // dict.logistics.material.group.bna
            ("dict.logistics.material.group.bna", "ja-JP", "bna", "物料组.screw,mptc"),
            // dict.logistics.material.group.bna
            ("dict.logistics.material.group.bna", "zh-CN", "screw,mptc", "物料组.screw,mptc"),
            // dict.logistics.material.group.bna
            ("dict.logistics.material.group.bna", "zh-HK", "screw,mptc", "物料组.screw,mptc"),

            // dict.logistics.material.group.bnb
            ("dict.logistics.material.group.bnb", "en-US", "bnb", "物料组.screw,mptd"),
            // dict.logistics.material.group.bnb
            ("dict.logistics.material.group.bnb", "ja-JP", "bnb", "物料组.screw,mptd"),
            // dict.logistics.material.group.bnb
            ("dict.logistics.material.group.bnb", "zh-CN", "screw,mptd", "物料组.screw,mptd"),
            // dict.logistics.material.group.bnb
            ("dict.logistics.material.group.bnb", "zh-HK", "screw,mptd", "物料组.screw,mptd"),

            // dict.logistics.material.group.bnc
            ("dict.logistics.material.group.bnc", "en-US", "bnc", "物料组.screw,mptn"),
            // dict.logistics.material.group.bnc
            ("dict.logistics.material.group.bnc", "ja-JP", "bnc", "物料组.screw,mptn"),
            // dict.logistics.material.group.bnc
            ("dict.logistics.material.group.bnc", "zh-CN", "screw,mptn", "物料组.screw,mptn"),
            // dict.logistics.material.group.bnc
            ("dict.logistics.material.group.bnc", "zh-HK", "screw,mptn", "物料组.screw,mptn"),

            // dict.logistics.material.group.bnd
            ("dict.logistics.material.group.bnd", "en-US", "bnd", "物料组.screw,mpto"),
            // dict.logistics.material.group.bnd
            ("dict.logistics.material.group.bnd", "ja-JP", "bnd", "物料组.screw,mpto"),
            // dict.logistics.material.group.bnd
            ("dict.logistics.material.group.bnd", "zh-CN", "screw,mpto", "物料组.screw,mpto"),
            // dict.logistics.material.group.bnd
            ("dict.logistics.material.group.bnd", "zh-HK", "screw,mpto", "物料组.screw,mpto"),

            // dict.logistics.material.group.bne
            ("dict.logistics.material.group.bne", "en-US", "bne", "物料组.screw,mptp"),
            // dict.logistics.material.group.bne
            ("dict.logistics.material.group.bne", "ja-JP", "bne", "物料组.screw,mptp"),
            // dict.logistics.material.group.bne
            ("dict.logistics.material.group.bne", "zh-CN", "screw,mptp", "物料组.screw,mptp"),
            // dict.logistics.material.group.bne
            ("dict.logistics.material.group.bne", "zh-HK", "screw,mptp", "物料组.screw,mptp"),

            // dict.logistics.material.group.bnf
            ("dict.logistics.material.group.bnf", "en-US", "bnf", "物料组.screw,mptq"),
            // dict.logistics.material.group.bnf
            ("dict.logistics.material.group.bnf", "ja-JP", "bnf", "物料组.screw,mptq"),
            // dict.logistics.material.group.bnf
            ("dict.logistics.material.group.bnf", "zh-CN", "screw,mptq", "物料组.screw,mptq"),
            // dict.logistics.material.group.bnf
            ("dict.logistics.material.group.bnf", "zh-HK", "screw,mptq", "物料组.screw,mptq"),

            // dict.logistics.material.group.bng
            ("dict.logistics.material.group.bng", "en-US", "bng", "物料组.screw,mptr"),
            // dict.logistics.material.group.bng
            ("dict.logistics.material.group.bng", "ja-JP", "bng", "物料组.screw,mptr"),
            // dict.logistics.material.group.bng
            ("dict.logistics.material.group.bng", "zh-CN", "screw,mptr", "物料组.screw,mptr"),
            // dict.logistics.material.group.bng
            ("dict.logistics.material.group.bng", "zh-HK", "screw,mptr", "物料组.screw,mptr"),

            // dict.logistics.material.group.bnh
            ("dict.logistics.material.group.bnh", "en-US", "bnh", "物料组.screw,mpsc"),
            // dict.logistics.material.group.bnh
            ("dict.logistics.material.group.bnh", "ja-JP", "bnh", "物料组.screw,mpsc"),
            // dict.logistics.material.group.bnh
            ("dict.logistics.material.group.bnh", "zh-CN", "screw,mpsc", "物料组.screw,mpsc"),
            // dict.logistics.material.group.bnh
            ("dict.logistics.material.group.bnh", "zh-HK", "screw,mpsc", "物料组.screw,mpsc"),

            // dict.logistics.material.group.bni
            ("dict.logistics.material.group.bni", "en-US", "bni", "物料组.screw,mpsd"),
            // dict.logistics.material.group.bni
            ("dict.logistics.material.group.bni", "ja-JP", "bni", "物料组.screw,mpsd"),
            // dict.logistics.material.group.bni
            ("dict.logistics.material.group.bni", "zh-CN", "screw,mpsd", "物料组.screw,mpsd"),
            // dict.logistics.material.group.bni
            ("dict.logistics.material.group.bni", "zh-HK", "screw,mpsd", "物料组.screw,mpsd"),

            // dict.logistics.material.group.bnj
            ("dict.logistics.material.group.bnj", "en-US", "bnj", "物料组.screw,mpsp"),
            // dict.logistics.material.group.bnj
            ("dict.logistics.material.group.bnj", "ja-JP", "bnj", "物料组.screw,mpsp"),
            // dict.logistics.material.group.bnj
            ("dict.logistics.material.group.bnj", "zh-CN", "screw,mpsp", "物料组.screw,mpsp"),
            // dict.logistics.material.group.bnj
            ("dict.logistics.material.group.bnj", "zh-HK", "screw,mpsp", "物料组.screw,mpsp"),

            // dict.logistics.material.group.bnk
            ("dict.logistics.material.group.bnk", "en-US", "bnk", "物料组.screw,mpsq"),
            // dict.logistics.material.group.bnk
            ("dict.logistics.material.group.bnk", "ja-JP", "bnk", "物料组.screw,mpsq"),
            // dict.logistics.material.group.bnk
            ("dict.logistics.material.group.bnk", "zh-CN", "screw,mpsq", "物料组.screw,mpsq"),
            // dict.logistics.material.group.bnk
            ("dict.logistics.material.group.bnk", "zh-HK", "screw,mpsq", "物料组.screw,mpsq"),

            // dict.logistics.material.group.bnl
            ("dict.logistics.material.group.bnl", "en-US", "bnl", "物料组.screw,mpsr"),
            // dict.logistics.material.group.bnl
            ("dict.logistics.material.group.bnl", "ja-JP", "bnl", "物料组.screw,mpsr"),
            // dict.logistics.material.group.bnl
            ("dict.logistics.material.group.bnl", "zh-CN", "screw,mpsr", "物料组.screw,mpsr"),
            // dict.logistics.material.group.bnl
            ("dict.logistics.material.group.bnl", "zh-HK", "screw,mpsr", "物料组.screw,mpsr"),

            // dict.logistics.material.group.bnm
            ("dict.logistics.material.group.bnm", "en-US", "bnm", "物料组.screw,mpbc"),
            // dict.logistics.material.group.bnm
            ("dict.logistics.material.group.bnm", "ja-JP", "bnm", "物料组.screw,mpbc"),
            // dict.logistics.material.group.bnm
            ("dict.logistics.material.group.bnm", "zh-CN", "screw,mpbc", "物料组.screw,mpbc"),
            // dict.logistics.material.group.bnm
            ("dict.logistics.material.group.bnm", "zh-HK", "screw,mpbc", "物料组.screw,mpbc"),

            // dict.logistics.material.group.bnn
            ("dict.logistics.material.group.bnn", "en-US", "bnn", "物料组.screw,mpbd"),
            // dict.logistics.material.group.bnn
            ("dict.logistics.material.group.bnn", "ja-JP", "bnn", "物料组.screw,mpbd"),
            // dict.logistics.material.group.bnn
            ("dict.logistics.material.group.bnn", "zh-CN", "screw,mpbd", "物料组.screw,mpbd"),
            // dict.logistics.material.group.bnn
            ("dict.logistics.material.group.bnn", "zh-HK", "screw,mpbd", "物料组.screw,mpbd"),

            // dict.logistics.material.group.bno
            ("dict.logistics.material.group.bno", "en-US", "bno", "物料组.screw,mpbp"),
            // dict.logistics.material.group.bno
            ("dict.logistics.material.group.bno", "ja-JP", "bno", "物料组.screw,mpbp"),
            // dict.logistics.material.group.bno
            ("dict.logistics.material.group.bno", "zh-CN", "screw,mpbp", "物料组.screw,mpbp"),
            // dict.logistics.material.group.bno
            ("dict.logistics.material.group.bno", "zh-HK", "screw,mpbp", "物料组.screw,mpbp"),

            // dict.logistics.material.group.bnp
            ("dict.logistics.material.group.bnp", "en-US", "bnp", "物料组.screw,mpbq"),
            // dict.logistics.material.group.bnp
            ("dict.logistics.material.group.bnp", "ja-JP", "bnp", "物料组.screw,mpbq"),
            // dict.logistics.material.group.bnp
            ("dict.logistics.material.group.bnp", "zh-CN", "screw,mpbq", "物料组.screw,mpbq"),
            // dict.logistics.material.group.bnp
            ("dict.logistics.material.group.bnp", "zh-HK", "screw,mpbq", "物料组.screw,mpbq"),

            // dict.logistics.material.group.bnq
            ("dict.logistics.material.group.bnq", "en-US", "bnq", "物料组.screw,mpbr"),
            // dict.logistics.material.group.bnq
            ("dict.logistics.material.group.bnq", "ja-JP", "bnq", "物料组.screw,mpbr"),
            // dict.logistics.material.group.bnq
            ("dict.logistics.material.group.bnq", "zh-CN", "screw,mpbr", "物料组.screw,mpbr"),
            // dict.logistics.material.group.bnq
            ("dict.logistics.material.group.bnq", "zh-HK", "screw,mpbr", "物料组.screw,mpbr"),

            // dict.logistics.material.group.bnr
            ("dict.logistics.material.group.bnr", "en-US", "bnr", "物料组.screw,mppc"),
            // dict.logistics.material.group.bnr
            ("dict.logistics.material.group.bnr", "ja-JP", "bnr", "物料组.screw,mppc"),
            // dict.logistics.material.group.bnr
            ("dict.logistics.material.group.bnr", "zh-CN", "screw,mppc", "物料组.screw,mppc"),
            // dict.logistics.material.group.bnr
            ("dict.logistics.material.group.bnr", "zh-HK", "screw,mppc", "物料组.screw,mppc"),

            // dict.logistics.material.group.bns
            ("dict.logistics.material.group.bns", "en-US", "bns", "物料组.screw,mppd"),
            // dict.logistics.material.group.bns
            ("dict.logistics.material.group.bns", "ja-JP", "bns", "物料组.screw,mppd"),
            // dict.logistics.material.group.bns
            ("dict.logistics.material.group.bns", "zh-CN", "screw,mppd", "物料组.screw,mppd"),
            // dict.logistics.material.group.bns
            ("dict.logistics.material.group.bns", "zh-HK", "screw,mppd", "物料组.screw,mppd"),

            // dict.logistics.material.group.bnt
            ("dict.logistics.material.group.bnt", "en-US", "bnt", "物料组.screw,mppp"),
            // dict.logistics.material.group.bnt
            ("dict.logistics.material.group.bnt", "ja-JP", "bnt", "物料组.screw,mppp"),
            // dict.logistics.material.group.bnt
            ("dict.logistics.material.group.bnt", "zh-CN", "screw,mppp", "物料组.screw,mppp"),
            // dict.logistics.material.group.bnt
            ("dict.logistics.material.group.bnt", "zh-HK", "screw,mppp", "物料组.screw,mppp"),

            // dict.logistics.material.group.bnu
            ("dict.logistics.material.group.bnu", "en-US", "bnu", "物料组.screw,mppq"),
            // dict.logistics.material.group.bnu
            ("dict.logistics.material.group.bnu", "ja-JP", "bnu", "物料组.screw,mppq"),
            // dict.logistics.material.group.bnu
            ("dict.logistics.material.group.bnu", "zh-CN", "screw,mppq", "物料组.screw,mppq"),
            // dict.logistics.material.group.bnu
            ("dict.logistics.material.group.bnu", "zh-HK", "screw,mppq", "物料组.screw,mppq"),

            // dict.logistics.material.group.bnv
            ("dict.logistics.material.group.bnv", "en-US", "bnv", "物料组.screw,mppr"),
            // dict.logistics.material.group.bnv
            ("dict.logistics.material.group.bnv", "ja-JP", "bnv", "物料组.screw,mppr"),
            // dict.logistics.material.group.bnv
            ("dict.logistics.material.group.bnv", "zh-CN", "screw,mppr", "物料组.screw,mppr"),
            // dict.logistics.material.group.bnv
            ("dict.logistics.material.group.bnv", "zh-HK", "screw,mppr", "物料组.screw,mppr"),

            // dict.logistics.material.group.boa
            ("dict.logistics.material.group.boa", "en-US", "boa", "物料组.screw,opa"),
            // dict.logistics.material.group.boa
            ("dict.logistics.material.group.boa", "ja-JP", "boa", "物料组.screw,opa"),
            // dict.logistics.material.group.boa
            ("dict.logistics.material.group.boa", "zh-CN", "screw,opa", "物料组.screw,opa"),
            // dict.logistics.material.group.boa
            ("dict.logistics.material.group.boa", "zh-HK", "screw,opa", "物料组.screw,opa"),

            // dict.logistics.material.group.bob
            ("dict.logistics.material.group.bob", "en-US", "bob", "物料组.screw,opb"),
            // dict.logistics.material.group.bob
            ("dict.logistics.material.group.bob", "ja-JP", "bob", "物料组.screw,opb"),
            // dict.logistics.material.group.bob
            ("dict.logistics.material.group.bob", "zh-CN", "screw,opb", "物料组.screw,opb"),
            // dict.logistics.material.group.bob
            ("dict.logistics.material.group.bob", "zh-HK", "screw,opb", "物料组.screw,opb"),

            // dict.logistics.material.group.bog
            ("dict.logistics.material.group.bog", "en-US", "bog", "物料组.screw,opg"),
            // dict.logistics.material.group.bog
            ("dict.logistics.material.group.bog", "ja-JP", "bog", "物料组.screw,opg"),
            // dict.logistics.material.group.bog
            ("dict.logistics.material.group.bog", "zh-CN", "screw,opg", "物料组.screw,opg"),
            // dict.logistics.material.group.bog
            ("dict.logistics.material.group.bog", "zh-HK", "screw,opg", "物料组.screw,opg"),

            // dict.logistics.material.group.boh
            ("dict.logistics.material.group.boh", "en-US", "boh", "物料组.screw,oph"),
            // dict.logistics.material.group.boh
            ("dict.logistics.material.group.boh", "ja-JP", "boh", "物料组.screw,oph"),
            // dict.logistics.material.group.boh
            ("dict.logistics.material.group.boh", "zh-CN", "screw,oph", "物料组.screw,oph"),
            // dict.logistics.material.group.boh
            ("dict.logistics.material.group.boh", "zh-HK", "screw,oph", "物料组.screw,oph"),

            // dict.logistics.material.group.boj
            ("dict.logistics.material.group.boj", "en-US", "boj", "物料组.screw,opj"),
            // dict.logistics.material.group.boj
            ("dict.logistics.material.group.boj", "ja-JP", "boj", "物料组.screw,opj"),
            // dict.logistics.material.group.boj
            ("dict.logistics.material.group.boj", "zh-CN", "screw,opj", "物料组.screw,opj"),
            // dict.logistics.material.group.boj
            ("dict.logistics.material.group.boj", "zh-HK", "screw,opj", "物料组.screw,opj"),

            // dict.logistics.material.group.bol
            ("dict.logistics.material.group.bol", "en-US", "bol", "物料组.screw,opl"),
            // dict.logistics.material.group.bol
            ("dict.logistics.material.group.bol", "ja-JP", "bol", "物料组.screw,opl"),
            // dict.logistics.material.group.bol
            ("dict.logistics.material.group.bol", "zh-CN", "screw,opl", "物料组.screw,opl"),
            // dict.logistics.material.group.bol
            ("dict.logistics.material.group.bol", "zh-HK", "screw,opl", "物料组.screw,opl"),

            // dict.logistics.material.group.bop
            ("dict.logistics.material.group.bop", "en-US", "bop", "物料组.screw,opp"),
            // dict.logistics.material.group.bop
            ("dict.logistics.material.group.bop", "ja-JP", "bop", "物料组.screw,opp"),
            // dict.logistics.material.group.bop
            ("dict.logistics.material.group.bop", "zh-CN", "screw,opp", "物料组.screw,opp"),
            // dict.logistics.material.group.bop
            ("dict.logistics.material.group.bop", "zh-HK", "screw,opp", "物料组.screw,opp"),

            // dict.logistics.material.group.bor
            ("dict.logistics.material.group.bor", "en-US", "bor", "物料组.screw,opq"),
            // dict.logistics.material.group.bor
            ("dict.logistics.material.group.bor", "ja-JP", "bor", "物料组.screw,opq"),
            // dict.logistics.material.group.bor
            ("dict.logistics.material.group.bor", "zh-CN", "screw,opq", "物料组.screw,opq"),
            // dict.logistics.material.group.bor
            ("dict.logistics.material.group.bor", "zh-HK", "screw,opq", "物料组.screw,opq"),

            // dict.logistics.material.group.bos
            ("dict.logistics.material.group.bos", "en-US", "bos", "物料组.screw,ops"),
            // dict.logistics.material.group.bos
            ("dict.logistics.material.group.bos", "ja-JP", "bos", "物料组.screw,ops"),
            // dict.logistics.material.group.bos
            ("dict.logistics.material.group.bos", "zh-CN", "screw,ops", "物料组.screw,ops"),
            // dict.logistics.material.group.bos
            ("dict.logistics.material.group.bos", "zh-HK", "screw,ops", "物料组.screw,ops"),

            // dict.logistics.material.group.bov
            ("dict.logistics.material.group.bov", "en-US", "bov", "物料组.screw,opv"),
            // dict.logistics.material.group.bov
            ("dict.logistics.material.group.bov", "ja-JP", "bov", "物料组.screw,opv"),
            // dict.logistics.material.group.bov
            ("dict.logistics.material.group.bov", "zh-CN", "screw,opv", "物料组.screw,opv"),
            // dict.logistics.material.group.bov
            ("dict.logistics.material.group.bov", "zh-HK", "screw,opv", "物料组.screw,opv"),

            // dict.logistics.material.group.boz
            ("dict.logistics.material.group.boz", "en-US", "boz", "物料组.screw,o"),
            // dict.logistics.material.group.boz
            ("dict.logistics.material.group.boz", "ja-JP", "boz", "物料组.screw,o"),
            // dict.logistics.material.group.boz
            ("dict.logistics.material.group.boz", "zh-CN", "screw,o", "物料组.screw,o"),
            // dict.logistics.material.group.boz
            ("dict.logistics.material.group.boz", "zh-HK", "screw,o", "物料组.screw,o"),

            // dict.logistics.material.group.bpa
            ("dict.logistics.material.group.bpa", "en-US", "bpa", "物料组.screw,ppa"),
            // dict.logistics.material.group.bpa
            ("dict.logistics.material.group.bpa", "ja-JP", "bpa", "物料组.screw,ppa"),
            // dict.logistics.material.group.bpa
            ("dict.logistics.material.group.bpa", "zh-CN", "screw,ppa", "物料组.screw,ppa"),
            // dict.logistics.material.group.bpa
            ("dict.logistics.material.group.bpa", "zh-HK", "screw,ppa", "物料组.screw,ppa"),

            // dict.logistics.material.group.bpb
            ("dict.logistics.material.group.bpb", "en-US", "bpb", "物料组.screw,ppb"),
            // dict.logistics.material.group.bpb
            ("dict.logistics.material.group.bpb", "ja-JP", "bpb", "物料组.screw,ppb"),
            // dict.logistics.material.group.bpb
            ("dict.logistics.material.group.bpb", "zh-CN", "screw,ppb", "物料组.screw,ppb"),
            // dict.logistics.material.group.bpb
            ("dict.logistics.material.group.bpb", "zh-HK", "screw,ppb", "物料组.screw,ppb"),

            // dict.logistics.material.group.bpc
            ("dict.logistics.material.group.bpc", "en-US", "bpc", "物料组.screw,ppc"),
            // dict.logistics.material.group.bpc
            ("dict.logistics.material.group.bpc", "ja-JP", "bpc", "物料组.screw,ppc"),
            // dict.logistics.material.group.bpc
            ("dict.logistics.material.group.bpc", "zh-CN", "screw,ppc", "物料组.screw,ppc"),
            // dict.logistics.material.group.bpc
            ("dict.logistics.material.group.bpc", "zh-HK", "screw,ppc", "物料组.screw,ppc"),

            // dict.logistics.material.group.bpf
            ("dict.logistics.material.group.bpf", "en-US", "bpf", "物料组.screw,ppf"),
            // dict.logistics.material.group.bpf
            ("dict.logistics.material.group.bpf", "ja-JP", "bpf", "物料组.screw,ppf"),
            // dict.logistics.material.group.bpf
            ("dict.logistics.material.group.bpf", "zh-CN", "screw,ppf", "物料组.screw,ppf"),
            // dict.logistics.material.group.bpf
            ("dict.logistics.material.group.bpf", "zh-HK", "screw,ppf", "物料组.screw,ppf"),

            // dict.logistics.material.group.bpg
            ("dict.logistics.material.group.bpg", "en-US", "bpg", "物料组.screw,ppg"),
            // dict.logistics.material.group.bpg
            ("dict.logistics.material.group.bpg", "ja-JP", "bpg", "物料组.screw,ppg"),
            // dict.logistics.material.group.bpg
            ("dict.logistics.material.group.bpg", "zh-CN", "screw,ppg", "物料组.screw,ppg"),
            // dict.logistics.material.group.bpg
            ("dict.logistics.material.group.bpg", "zh-HK", "screw,ppg", "物料组.screw,ppg"),

            // dict.logistics.material.group.bph
            ("dict.logistics.material.group.bph", "en-US", "bph", "物料组.screw,pph"),
            // dict.logistics.material.group.bph
            ("dict.logistics.material.group.bph", "ja-JP", "bph", "物料组.screw,pph"),
            // dict.logistics.material.group.bph
            ("dict.logistics.material.group.bph", "zh-CN", "screw,pph", "物料组.screw,pph"),
            // dict.logistics.material.group.bph
            ("dict.logistics.material.group.bph", "zh-HK", "screw,pph", "物料组.screw,pph"),

            // dict.logistics.material.group.bpj
            ("dict.logistics.material.group.bpj", "en-US", "bpj", "物料组.screw,ppj"),
            // dict.logistics.material.group.bpj
            ("dict.logistics.material.group.bpj", "ja-JP", "bpj", "物料组.screw,ppj"),
            // dict.logistics.material.group.bpj
            ("dict.logistics.material.group.bpj", "zh-CN", "screw,ppj", "物料组.screw,ppj"),
            // dict.logistics.material.group.bpj
            ("dict.logistics.material.group.bpj", "zh-HK", "screw,ppj", "物料组.screw,ppj"),

            // dict.logistics.material.group.bpk
            ("dict.logistics.material.group.bpk", "en-US", "bpk", "物料组.screw,ppk"),
            // dict.logistics.material.group.bpk
            ("dict.logistics.material.group.bpk", "ja-JP", "bpk", "物料组.screw,ppk"),
            // dict.logistics.material.group.bpk
            ("dict.logistics.material.group.bpk", "zh-CN", "screw,ppk", "物料组.screw,ppk"),
            // dict.logistics.material.group.bpk
            ("dict.logistics.material.group.bpk", "zh-HK", "screw,ppk", "物料组.screw,ppk"),

            // dict.logistics.material.group.bpl
            ("dict.logistics.material.group.bpl", "en-US", "bpl", "物料组.screw,ppl"),
            // dict.logistics.material.group.bpl
            ("dict.logistics.material.group.bpl", "ja-JP", "bpl", "物料组.screw,ppl"),
            // dict.logistics.material.group.bpl
            ("dict.logistics.material.group.bpl", "zh-CN", "screw,ppl", "物料组.screw,ppl"),
            // dict.logistics.material.group.bpl
            ("dict.logistics.material.group.bpl", "zh-HK", "screw,ppl", "物料组.screw,ppl"),

            // dict.logistics.material.group.bpp
            ("dict.logistics.material.group.bpp", "en-US", "bpp", "物料组.screw,ppp"),
            // dict.logistics.material.group.bpp
            ("dict.logistics.material.group.bpp", "ja-JP", "bpp", "物料组.screw,ppp"),
            // dict.logistics.material.group.bpp
            ("dict.logistics.material.group.bpp", "zh-CN", "screw,ppp", "物料组.screw,ppp"),
            // dict.logistics.material.group.bpp
            ("dict.logistics.material.group.bpp", "zh-HK", "screw,ppp", "物料组.screw,ppp"),

            // dict.logistics.material.group.bpq
            ("dict.logistics.material.group.bpq", "en-US", "bpq", "物料组.screw,ppq"),
            // dict.logistics.material.group.bpq
            ("dict.logistics.material.group.bpq", "ja-JP", "bpq", "物料组.screw,ppq"),
            // dict.logistics.material.group.bpq
            ("dict.logistics.material.group.bpq", "zh-CN", "screw,ppq", "物料组.screw,ppq"),
            // dict.logistics.material.group.bpq
            ("dict.logistics.material.group.bpq", "zh-HK", "screw,ppq", "物料组.screw,ppq"),

            // dict.logistics.material.group.bps
            ("dict.logistics.material.group.bps", "en-US", "bps", "物料组.screw,pps"),
            // dict.logistics.material.group.bps
            ("dict.logistics.material.group.bps", "ja-JP", "bps", "物料组.screw,pps"),
            // dict.logistics.material.group.bps
            ("dict.logistics.material.group.bps", "zh-CN", "screw,pps", "物料组.screw,pps"),
            // dict.logistics.material.group.bps
            ("dict.logistics.material.group.bps", "zh-HK", "screw,pps", "物料组.screw,pps"),

            // dict.logistics.material.group.bpv
            ("dict.logistics.material.group.bpv", "en-US", "bpv", "物料组.screw,ppv"),
            // dict.logistics.material.group.bpv
            ("dict.logistics.material.group.bpv", "ja-JP", "bpv", "物料组.screw,ppv"),
            // dict.logistics.material.group.bpv
            ("dict.logistics.material.group.bpv", "zh-CN", "screw,ppv", "物料组.screw,ppv"),
            // dict.logistics.material.group.bpv
            ("dict.logistics.material.group.bpv", "zh-HK", "screw,ppv", "物料组.screw,ppv"),

            // dict.logistics.material.group.bra
            ("dict.logistics.material.group.bra", "en-US", "bra", "物料组.screw,rpa"),
            // dict.logistics.material.group.bra
            ("dict.logistics.material.group.bra", "ja-JP", "bra", "物料组.screw,rpa"),
            // dict.logistics.material.group.bra
            ("dict.logistics.material.group.bra", "zh-CN", "screw,rpa", "物料组.screw,rpa"),
            // dict.logistics.material.group.bra
            ("dict.logistics.material.group.bra", "zh-HK", "screw,rpa", "物料组.screw,rpa"),

            // dict.logistics.material.group.brv
            ("dict.logistics.material.group.brv", "en-US", "brv", "物料组.screw,rpv"),
            // dict.logistics.material.group.brv
            ("dict.logistics.material.group.brv", "ja-JP", "brv", "物料组.screw,rpv"),
            // dict.logistics.material.group.brv
            ("dict.logistics.material.group.brv", "zh-CN", "screw,rpv", "物料组.screw,rpv"),
            // dict.logistics.material.group.brv
            ("dict.logistics.material.group.brv", "zh-HK", "screw,rpv", "物料组.screw,rpv"),

            // dict.logistics.material.group.brz
            ("dict.logistics.material.group.brz", "en-US", "brz", "物料组.screw,r"),
            // dict.logistics.material.group.brz
            ("dict.logistics.material.group.brz", "ja-JP", "brz", "物料组.screw,r"),
            // dict.logistics.material.group.brz
            ("dict.logistics.material.group.brz", "zh-CN", "screw,r", "物料组.screw,r"),
            // dict.logistics.material.group.brz
            ("dict.logistics.material.group.brz", "zh-HK", "screw,r", "物料组.screw,r"),

            // dict.logistics.material.group.bta
            ("dict.logistics.material.group.bta", "en-US", "bta", "物料组.screw,tpa"),
            // dict.logistics.material.group.bta
            ("dict.logistics.material.group.bta", "ja-JP", "bta", "物料组.screw,tpa"),
            // dict.logistics.material.group.bta
            ("dict.logistics.material.group.bta", "zh-CN", "screw,tpa", "物料组.screw,tpa"),
            // dict.logistics.material.group.bta
            ("dict.logistics.material.group.bta", "zh-HK", "screw,tpa", "物料组.screw,tpa"),

            // dict.logistics.material.group.btz
            ("dict.logistics.material.group.btz", "en-US", "btz", "物料组.screw,t"),
            // dict.logistics.material.group.btz
            ("dict.logistics.material.group.btz", "ja-JP", "btz", "物料组.screw,t"),
            // dict.logistics.material.group.btz
            ("dict.logistics.material.group.btz", "zh-CN", "screw,t", "物料组.screw,t"),
            // dict.logistics.material.group.btz
            ("dict.logistics.material.group.btz", "zh-HK", "screw,t", "物料组.screw,t"),

            // dict.logistics.material.group.bva
            ("dict.logistics.material.group.bva", "en-US", "bva", "物料组.screw,vpa"),
            // dict.logistics.material.group.bva
            ("dict.logistics.material.group.bva", "ja-JP", "bva", "物料组.screw,vpa"),
            // dict.logistics.material.group.bva
            ("dict.logistics.material.group.bva", "zh-CN", "screw,vpa", "物料组.screw,vpa"),
            // dict.logistics.material.group.bva
            ("dict.logistics.material.group.bva", "zh-HK", "screw,vpa", "物料组.screw,vpa"),

            // dict.logistics.material.group.bvb
            ("dict.logistics.material.group.bvb", "en-US", "bvb", "物料组.screw,vpb"),
            // dict.logistics.material.group.bvb
            ("dict.logistics.material.group.bvb", "ja-JP", "bvb", "物料组.screw,vpb"),
            // dict.logistics.material.group.bvb
            ("dict.logistics.material.group.bvb", "zh-CN", "screw,vpb", "物料组.screw,vpb"),
            // dict.logistics.material.group.bvb
            ("dict.logistics.material.group.bvb", "zh-HK", "screw,vpb", "物料组.screw,vpb"),

            // dict.logistics.material.group.bvc
            ("dict.logistics.material.group.bvc", "en-US", "bvc", "物料组.screw,vpc"),
            // dict.logistics.material.group.bvc
            ("dict.logistics.material.group.bvc", "ja-JP", "bvc", "物料组.screw,vpc"),
            // dict.logistics.material.group.bvc
            ("dict.logistics.material.group.bvc", "zh-CN", "screw,vpc", "物料组.screw,vpc"),
            // dict.logistics.material.group.bvc
            ("dict.logistics.material.group.bvc", "zh-HK", "screw,vpc", "物料组.screw,vpc"),

            // dict.logistics.material.group.bvz
            ("dict.logistics.material.group.bvz", "en-US", "bvz", "物料组.screw,v"),
            // dict.logistics.material.group.bvz
            ("dict.logistics.material.group.bvz", "ja-JP", "bvz", "物料组.screw,v"),
            // dict.logistics.material.group.bvz
            ("dict.logistics.material.group.bvz", "zh-CN", "screw,v", "物料组.screw,v"),
            // dict.logistics.material.group.bvz
            ("dict.logistics.material.group.bvz", "zh-HK", "screw,v", "物料组.screw,v"),

            // dict.logistics.material.group.bwa
            ("dict.logistics.material.group.bwa", "en-US", "bwa", "物料组.bolt,w"),
            // dict.logistics.material.group.bwa
            ("dict.logistics.material.group.bwa", "ja-JP", "bwa", "物料组.bolt,w"),
            // dict.logistics.material.group.bwa
            ("dict.logistics.material.group.bwa", "zh-CN", "bolt,w", "物料组.bolt,w"),
            // dict.logistics.material.group.bwa
            ("dict.logistics.material.group.bwa", "zh-HK", "bolt,w", "物料组.bolt,w"),

            // dict.logistics.material.group.bya
            ("dict.logistics.material.group.bya", "en-US", "bya", "物料组.screw,yha"),
            // dict.logistics.material.group.bya
            ("dict.logistics.material.group.bya", "ja-JP", "bya", "物料组.screw,yha"),
            // dict.logistics.material.group.bya
            ("dict.logistics.material.group.bya", "zh-CN", "screw,yha", "物料组.screw,yha"),
            // dict.logistics.material.group.bya
            ("dict.logistics.material.group.bya", "zh-HK", "screw,yha", "物料组.screw,yha"),

            // dict.logistics.material.group.byb
            ("dict.logistics.material.group.byb", "en-US", "byb", "物料组.screw,yhz"),
            // dict.logistics.material.group.byb
            ("dict.logistics.material.group.byb", "ja-JP", "byb", "物料组.screw,yhz"),
            // dict.logistics.material.group.byb
            ("dict.logistics.material.group.byb", "zh-CN", "screw,yhz", "物料组.screw,yhz"),
            // dict.logistics.material.group.byb
            ("dict.logistics.material.group.byb", "zh-HK", "screw,yhz", "物料组.screw,yhz"),

            // dict.logistics.material.group.byf
            ("dict.logistics.material.group.byf", "en-US", "byf", "物料组.screw,yhf"),
            // dict.logistics.material.group.byf
            ("dict.logistics.material.group.byf", "ja-JP", "byf", "物料组.screw,yhf"),
            // dict.logistics.material.group.byf
            ("dict.logistics.material.group.byf", "zh-CN", "screw,yhf", "物料组.screw,yhf"),
            // dict.logistics.material.group.byf
            ("dict.logistics.material.group.byf", "zh-HK", "screw,yhf", "物料组.screw,yhf"),

            // dict.logistics.material.group.byk
            ("dict.logistics.material.group.byk", "en-US", "byk", "物料组.screw,yhk"),
            // dict.logistics.material.group.byk
            ("dict.logistics.material.group.byk", "ja-JP", "byk", "物料组.screw,yhk"),
            // dict.logistics.material.group.byk
            ("dict.logistics.material.group.byk", "zh-CN", "screw,yhk", "物料组.screw,yhk"),
            // dict.logistics.material.group.byk
            ("dict.logistics.material.group.byk", "zh-HK", "screw,yhk", "物料组.screw,yhk"),

            // dict.logistics.material.group.byr
            ("dict.logistics.material.group.byr", "en-US", "byr", "物料组.screw,yhr"),
            // dict.logistics.material.group.byr
            ("dict.logistics.material.group.byr", "ja-JP", "byr", "物料组.screw,yhr"),
            // dict.logistics.material.group.byr
            ("dict.logistics.material.group.byr", "zh-CN", "screw,yhr", "物料组.screw,yhr"),
            // dict.logistics.material.group.byr
            ("dict.logistics.material.group.byr", "zh-HK", "screw,yhr", "物料组.screw,yhr"),

            // dict.logistics.material.group.byt
            ("dict.logistics.material.group.byt", "en-US", "byt", "物料组.screw,yht"),
            // dict.logistics.material.group.byt
            ("dict.logistics.material.group.byt", "ja-JP", "byt", "物料组.screw,yht"),
            // dict.logistics.material.group.byt
            ("dict.logistics.material.group.byt", "zh-CN", "screw,yht", "物料组.screw,yht"),
            // dict.logistics.material.group.byt
            ("dict.logistics.material.group.byt", "zh-HK", "screw,yht", "物料组.screw,yht"),

            // dict.logistics.material.group.byw
            ("dict.logistics.material.group.byw", "en-US", "byw", "物料组.screw,yhw"),
            // dict.logistics.material.group.byw
            ("dict.logistics.material.group.byw", "ja-JP", "byw", "物料组.screw,yhw"),
            // dict.logistics.material.group.byw
            ("dict.logistics.material.group.byw", "zh-CN", "screw,yhw", "物料组.screw,yhw"),
            // dict.logistics.material.group.byw
            ("dict.logistics.material.group.byw", "zh-HK", "screw,yhw", "物料组.screw,yhw"),

            // dict.logistics.material.group.byz
            ("dict.logistics.material.group.byz", "en-US", "byz", "物料组.screw,y"),
            // dict.logistics.material.group.byz
            ("dict.logistics.material.group.byz", "ja-JP", "byz", "物料组.screw,y"),
            // dict.logistics.material.group.byz
            ("dict.logistics.material.group.byz", "zh-CN", "screw,y", "物料组.screw,y"),
            // dict.logistics.material.group.byz
            ("dict.logistics.material.group.byz", "zh-HK", "screw,y", "物料组.screw,y"),

            // dict.logistics.material.group.cab
            ("dict.logistics.material.group.cab", "en-US", "cab", "物料组.cap array"),
            // dict.logistics.material.group.cab
            ("dict.logistics.material.group.cab", "ja-JP", "cab", "物料组.cap array"),
            // dict.logistics.material.group.cab
            ("dict.logistics.material.group.cab", "zh-CN", "cap array", "物料组.cap array"),
            // dict.logistics.material.group.cab
            ("dict.logistics.material.group.cab", "zh-HK", "cap array", "物料组.cap array"),

            // dict.logistics.material.group.ccc
            ("dict.logistics.material.group.ccc", "en-US", "ccc", "物料组.cc"),
            // dict.logistics.material.group.ccc
            ("dict.logistics.material.group.ccc", "ja-JP", "ccc", "物料组.cc"),
            // dict.logistics.material.group.ccc
            ("dict.logistics.material.group.ccc", "zh-CN", "cc", "物料组.cc"),
            // dict.logistics.material.group.ccc
            ("dict.logistics.material.group.ccc", "zh-HK", "cc", "物料组.cc"),

            // dict.logistics.material.group.cec
            ("dict.logistics.material.group.cec", "en-US", "cec", "物料组.ce"),
            // dict.logistics.material.group.cec
            ("dict.logistics.material.group.cec", "ja-JP", "cec", "物料组.ce"),
            // dict.logistics.material.group.cec
            ("dict.logistics.material.group.cec", "zh-CN", "ce", "物料组.ce"),
            // dict.logistics.material.group.cec
            ("dict.logistics.material.group.cec", "zh-HK", "ce", "物料组.ce"),

            // dict.logistics.material.group.cmc
            ("dict.logistics.material.group.cmc", "en-US", "cmc", "物料组.cm"),
            // dict.logistics.material.group.cmc
            ("dict.logistics.material.group.cmc", "ja-JP", "cmc", "物料组.cm"),
            // dict.logistics.material.group.cmc
            ("dict.logistics.material.group.cmc", "zh-CN", "cm", "物料组.cm"),
            // dict.logistics.material.group.cmc
            ("dict.logistics.material.group.cmc", "zh-HK", "cm", "物料组.cm"),

            // dict.logistics.material.group.cna
            ("dict.logistics.material.group.cna", "en-US", "cna", "物料组.cap"),
            // dict.logistics.material.group.cna
            ("dict.logistics.material.group.cna", "ja-JP", "cna", "物料组.cap"),
            // dict.logistics.material.group.cna
            ("dict.logistics.material.group.cna", "zh-CN", "cap", "物料组.cap"),
            // dict.logistics.material.group.cna
            ("dict.logistics.material.group.cna", "zh-HK", "cap", "物料组.cap"),

            // dict.logistics.material.group.cpc
            ("dict.logistics.material.group.cpc", "en-US", "cpc", "物料组.cp"),
            // dict.logistics.material.group.cpc
            ("dict.logistics.material.group.cpc", "ja-JP", "cpc", "物料组.cp"),
            // dict.logistics.material.group.cpc
            ("dict.logistics.material.group.cpc", "zh-CN", "cp", "物料组.cp"),
            // dict.logistics.material.group.cpc
            ("dict.logistics.material.group.cpc", "zh-HK", "cp", "物料组.cp"),

            // dict.logistics.material.group.cqa
            ("dict.logistics.material.group.cqa", "en-US", "cqa", "物料组.cq"),
            // dict.logistics.material.group.cqa
            ("dict.logistics.material.group.cqa", "ja-JP", "cqa", "物料组.cq"),
            // dict.logistics.material.group.cqa
            ("dict.logistics.material.group.cqa", "zh-CN", "cq", "物料组.cq"),
            // dict.logistics.material.group.cqa
            ("dict.logistics.material.group.cqa", "zh-HK", "cq", "物料组.cq"),

            // dict.logistics.material.group.csc
            ("dict.logistics.material.group.csc", "en-US", "csc", "物料组.cs"),
            // dict.logistics.material.group.csc
            ("dict.logistics.material.group.csc", "ja-JP", "csc", "物料组.cs"),
            // dict.logistics.material.group.csc
            ("dict.logistics.material.group.csc", "zh-CN", "cs", "物料组.cs"),
            // dict.logistics.material.group.csc
            ("dict.logistics.material.group.csc", "zh-HK", "cs", "物料组.cs"),

            // dict.logistics.material.group.cvt
            ("dict.logistics.material.group.cvt", "en-US", "cvt", "物料组.var cap"),
            // dict.logistics.material.group.cvt
            ("dict.logistics.material.group.cvt", "ja-JP", "cvt", "物料组.var cap"),
            // dict.logistics.material.group.cvt
            ("dict.logistics.material.group.cvt", "zh-CN", "var cap", "物料组.var cap"),
            // dict.logistics.material.group.cvt
            ("dict.logistics.material.group.cvt", "zh-HK", "var cap", "物料组.var cap"),

            // dict.logistics.material.group.cya
            ("dict.logistics.material.group.cya", "en-US", "cya", "物料组.cy"),
            // dict.logistics.material.group.cya
            ("dict.logistics.material.group.cya", "ja-JP", "cya", "物料组.cy"),
            // dict.logistics.material.group.cya
            ("dict.logistics.material.group.cya", "zh-CN", "cy", "物料组.cy"),
            // dict.logistics.material.group.cya
            ("dict.logistics.material.group.cya", "zh-HK", "cy", "物料组.cy"),

            // dict.logistics.material.group.daa
            ("dict.logistics.material.group.daa", "en-US", "daa", "物料组.drawing"),
            // dict.logistics.material.group.daa
            ("dict.logistics.material.group.daa", "ja-JP", "daa", "物料组.drawing"),
            // dict.logistics.material.group.daa
            ("dict.logistics.material.group.daa", "zh-CN", "drawing", "物料组.drawing"),
            // dict.logistics.material.group.daa
            ("dict.logistics.material.group.daa", "zh-HK", "drawing", "物料组.drawing"),

            // dict.logistics.material.group.dca
            ("dict.logistics.material.group.dca", "en-US", "dca", "物料组.document, a"),
            // dict.logistics.material.group.dca
            ("dict.logistics.material.group.dca", "ja-JP", "dca", "物料组.document, a"),
            // dict.logistics.material.group.dca
            ("dict.logistics.material.group.dca", "zh-CN", "document, a", "物料组.document, a"),
            // dict.logistics.material.group.dca
            ("dict.logistics.material.group.dca", "zh-HK", "document, a", "物料组.document, a"),

            // dict.logistics.material.group.dcb
            ("dict.logistics.material.group.dcb", "en-US", "dcb", "物料组.document, b"),
            // dict.logistics.material.group.dcb
            ("dict.logistics.material.group.dcb", "ja-JP", "dcb", "物料组.document, b"),
            // dict.logistics.material.group.dcb
            ("dict.logistics.material.group.dcb", "zh-CN", "document, b", "物料组.document, b"),
            // dict.logistics.material.group.dcb
            ("dict.logistics.material.group.dcb", "zh-HK", "document, b", "物料组.document, b"),

            // dict.logistics.material.group.dcc
            ("dict.logistics.material.group.dcc", "en-US", "dcc", "物料组.document, c"),
            // dict.logistics.material.group.dcc
            ("dict.logistics.material.group.dcc", "ja-JP", "dcc", "物料组.document, c"),
            // dict.logistics.material.group.dcc
            ("dict.logistics.material.group.dcc", "zh-CN", "document, c", "物料组.document, c"),
            // dict.logistics.material.group.dcc
            ("dict.logistics.material.group.dcc", "zh-HK", "document, c", "物料组.document, c"),

            // dict.logistics.material.group.dcd
            ("dict.logistics.material.group.dcd", "en-US", "dcd", "物料组.document, d"),
            // dict.logistics.material.group.dcd
            ("dict.logistics.material.group.dcd", "ja-JP", "dcd", "物料组.document, d"),
            // dict.logistics.material.group.dcd
            ("dict.logistics.material.group.dcd", "zh-CN", "document, d", "物料组.document, d"),
            // dict.logistics.material.group.dcd
            ("dict.logistics.material.group.dcd", "zh-HK", "document, d", "物料组.document, d"),

            // dict.logistics.material.group.dce
            ("dict.logistics.material.group.dce", "en-US", "dce", "物料组.document, e"),
            // dict.logistics.material.group.dce
            ("dict.logistics.material.group.dce", "ja-JP", "dce", "物料组.document, e"),
            // dict.logistics.material.group.dce
            ("dict.logistics.material.group.dce", "zh-CN", "document, e", "物料组.document, e"),
            // dict.logistics.material.group.dce
            ("dict.logistics.material.group.dce", "zh-HK", "document, e", "物料组.document, e"),

            // dict.logistics.material.group.dcf
            ("dict.logistics.material.group.dcf", "en-US", "dcf", "物料组.document, f"),
            // dict.logistics.material.group.dcf
            ("dict.logistics.material.group.dcf", "ja-JP", "dcf", "物料组.document, f"),
            // dict.logistics.material.group.dcf
            ("dict.logistics.material.group.dcf", "zh-CN", "document, f", "物料组.document, f"),
            // dict.logistics.material.group.dcf
            ("dict.logistics.material.group.dcf", "zh-HK", "document, f", "物料组.document, f"),

            // dict.logistics.material.group.dcg
            ("dict.logistics.material.group.dcg", "en-US", "dcg", "物料组.document, g"),
            // dict.logistics.material.group.dcg
            ("dict.logistics.material.group.dcg", "ja-JP", "dcg", "物料组.document, g"),
            // dict.logistics.material.group.dcg
            ("dict.logistics.material.group.dcg", "zh-CN", "document, g", "物料组.document, g"),
            // dict.logistics.material.group.dcg
            ("dict.logistics.material.group.dcg", "zh-HK", "document, g", "物料组.document, g"),

            // dict.logistics.material.group.dch
            ("dict.logistics.material.group.dch", "en-US", "dch", "物料组.document, h"),
            // dict.logistics.material.group.dch
            ("dict.logistics.material.group.dch", "ja-JP", "dch", "物料组.document, h"),
            // dict.logistics.material.group.dch
            ("dict.logistics.material.group.dch", "zh-CN", "document, h", "物料组.document, h"),
            // dict.logistics.material.group.dch
            ("dict.logistics.material.group.dch", "zh-HK", "document, h", "物料组.document, h"),

            // dict.logistics.material.group.dci
            ("dict.logistics.material.group.dci", "en-US", "dci", "物料组.document, i"),
            // dict.logistics.material.group.dci
            ("dict.logistics.material.group.dci", "ja-JP", "dci", "物料组.document, i"),
            // dict.logistics.material.group.dci
            ("dict.logistics.material.group.dci", "zh-CN", "document, i", "物料组.document, i"),
            // dict.logistics.material.group.dci
            ("dict.logistics.material.group.dci", "zh-HK", "document, i", "物料组.document, i"),

            // dict.logistics.material.group.dcj
            ("dict.logistics.material.group.dcj", "en-US", "dcj", "物料组.document, j"),
            // dict.logistics.material.group.dcj
            ("dict.logistics.material.group.dcj", "ja-JP", "dcj", "物料组.document, j"),
            // dict.logistics.material.group.dcj
            ("dict.logistics.material.group.dcj", "zh-CN", "document, j", "物料组.document, j"),
            // dict.logistics.material.group.dcj
            ("dict.logistics.material.group.dcj", "zh-HK", "document, j", "物料组.document, j"),

            // dict.logistics.material.group.dck
            ("dict.logistics.material.group.dck", "en-US", "dck", "物料组.document, k"),
            // dict.logistics.material.group.dck
            ("dict.logistics.material.group.dck", "ja-JP", "dck", "物料组.document, k"),
            // dict.logistics.material.group.dck
            ("dict.logistics.material.group.dck", "zh-CN", "document, k", "物料组.document, k"),
            // dict.logistics.material.group.dck
            ("dict.logistics.material.group.dck", "zh-HK", "document, k", "物料组.document, k"),

            // dict.logistics.material.group.dcl
            ("dict.logistics.material.group.dcl", "en-US", "dcl", "物料组.document, l"),
            // dict.logistics.material.group.dcl
            ("dict.logistics.material.group.dcl", "ja-JP", "dcl", "物料组.document, l"),
            // dict.logistics.material.group.dcl
            ("dict.logistics.material.group.dcl", "zh-CN", "document, l", "物料组.document, l"),
            // dict.logistics.material.group.dcl
            ("dict.logistics.material.group.dcl", "zh-HK", "document, l", "物料组.document, l"),

            // dict.logistics.material.group.dcm
            ("dict.logistics.material.group.dcm", "en-US", "dcm", "物料组.document, m"),
            // dict.logistics.material.group.dcm
            ("dict.logistics.material.group.dcm", "ja-JP", "dcm", "物料组.document, m"),
            // dict.logistics.material.group.dcm
            ("dict.logistics.material.group.dcm", "zh-CN", "document, m", "物料组.document, m"),
            // dict.logistics.material.group.dcm
            ("dict.logistics.material.group.dcm", "zh-HK", "document, m", "物料组.document, m"),

            // dict.logistics.material.group.dcn
            ("dict.logistics.material.group.dcn", "en-US", "dcn", "物料组.document, n"),
            // dict.logistics.material.group.dcn
            ("dict.logistics.material.group.dcn", "ja-JP", "dcn", "物料组.document, n"),
            // dict.logistics.material.group.dcn
            ("dict.logistics.material.group.dcn", "zh-CN", "document, n", "物料组.document, n"),
            // dict.logistics.material.group.dcn
            ("dict.logistics.material.group.dcn", "zh-HK", "document, n", "物料组.document, n"),

            // dict.logistics.material.group.dco
            ("dict.logistics.material.group.dco", "en-US", "dco", "物料组.document, o"),
            // dict.logistics.material.group.dco
            ("dict.logistics.material.group.dco", "ja-JP", "dco", "物料组.document, o"),
            // dict.logistics.material.group.dco
            ("dict.logistics.material.group.dco", "zh-CN", "document, o", "物料组.document, o"),
            // dict.logistics.material.group.dco
            ("dict.logistics.material.group.dco", "zh-HK", "document, o", "物料组.document, o"),

            // dict.logistics.material.group.dcp
            ("dict.logistics.material.group.dcp", "en-US", "dcp", "物料组.document, p"),
            // dict.logistics.material.group.dcp
            ("dict.logistics.material.group.dcp", "ja-JP", "dcp", "物料组.document, p"),
            // dict.logistics.material.group.dcp
            ("dict.logistics.material.group.dcp", "zh-CN", "document, p", "物料组.document, p"),
            // dict.logistics.material.group.dcp
            ("dict.logistics.material.group.dcp", "zh-HK", "document, p", "物料组.document, p"),

            // dict.logistics.material.group.dcq
            ("dict.logistics.material.group.dcq", "en-US", "dcq", "物料组.document, q"),
            // dict.logistics.material.group.dcq
            ("dict.logistics.material.group.dcq", "ja-JP", "dcq", "物料组.document, q"),
            // dict.logistics.material.group.dcq
            ("dict.logistics.material.group.dcq", "zh-CN", "document, q", "物料组.document, q"),
            // dict.logistics.material.group.dcq
            ("dict.logistics.material.group.dcq", "zh-HK", "document, q", "物料组.document, q"),

            // dict.logistics.material.group.dcr
            ("dict.logistics.material.group.dcr", "en-US", "dcr", "物料组.document, r"),
            // dict.logistics.material.group.dcr
            ("dict.logistics.material.group.dcr", "ja-JP", "dcr", "物料组.document, r"),
            // dict.logistics.material.group.dcr
            ("dict.logistics.material.group.dcr", "zh-CN", "document, r", "物料组.document, r"),
            // dict.logistics.material.group.dcr
            ("dict.logistics.material.group.dcr", "zh-HK", "document, r", "物料组.document, r"),

            // dict.logistics.material.group.dcs
            ("dict.logistics.material.group.dcs", "en-US", "dcs", "物料组.document, s"),
            // dict.logistics.material.group.dcs
            ("dict.logistics.material.group.dcs", "ja-JP", "dcs", "物料组.document, s"),
            // dict.logistics.material.group.dcs
            ("dict.logistics.material.group.dcs", "zh-CN", "document, s", "物料组.document, s"),
            // dict.logistics.material.group.dcs
            ("dict.logistics.material.group.dcs", "zh-HK", "document, s", "物料组.document, s"),

            // dict.logistics.material.group.dct
            ("dict.logistics.material.group.dct", "en-US", "dct", "物料组.document, t"),
            // dict.logistics.material.group.dct
            ("dict.logistics.material.group.dct", "ja-JP", "dct", "物料组.document, t"),
            // dict.logistics.material.group.dct
            ("dict.logistics.material.group.dct", "zh-CN", "document, t", "物料组.document, t"),
            // dict.logistics.material.group.dct
            ("dict.logistics.material.group.dct", "zh-HK", "document, t", "物料组.document, t"),

            // dict.logistics.material.group.dcu
            ("dict.logistics.material.group.dcu", "en-US", "dcu", "物料组.document, u"),
            // dict.logistics.material.group.dcu
            ("dict.logistics.material.group.dcu", "ja-JP", "dcu", "物料组.document, u"),
            // dict.logistics.material.group.dcu
            ("dict.logistics.material.group.dcu", "zh-CN", "document, u", "物料组.document, u"),
            // dict.logistics.material.group.dcu
            ("dict.logistics.material.group.dcu", "zh-HK", "document, u", "物料组.document, u"),

            // dict.logistics.material.group.dcv
            ("dict.logistics.material.group.dcv", "en-US", "dcv", "物料组.document, v"),
            // dict.logistics.material.group.dcv
            ("dict.logistics.material.group.dcv", "ja-JP", "dcv", "物料组.document, v"),
            // dict.logistics.material.group.dcv
            ("dict.logistics.material.group.dcv", "zh-CN", "document, v", "物料组.document, v"),
            // dict.logistics.material.group.dcv
            ("dict.logistics.material.group.dcv", "zh-HK", "document, v", "物料组.document, v"),

            // dict.logistics.material.group.dcw
            ("dict.logistics.material.group.dcw", "en-US", "dcw", "物料组.document, w"),
            // dict.logistics.material.group.dcw
            ("dict.logistics.material.group.dcw", "ja-JP", "dcw", "物料组.document, w"),
            // dict.logistics.material.group.dcw
            ("dict.logistics.material.group.dcw", "zh-CN", "document, w", "物料组.document, w"),
            // dict.logistics.material.group.dcw
            ("dict.logistics.material.group.dcw", "zh-HK", "document, w", "物料组.document, w"),

            // dict.logistics.material.group.dcx
            ("dict.logistics.material.group.dcx", "en-US", "dcx", "物料组.document, x"),
            // dict.logistics.material.group.dcx
            ("dict.logistics.material.group.dcx", "ja-JP", "dcx", "物料组.document, x"),
            // dict.logistics.material.group.dcx
            ("dict.logistics.material.group.dcx", "zh-CN", "document, x", "物料组.document, x"),
            // dict.logistics.material.group.dcx
            ("dict.logistics.material.group.dcx", "zh-HK", "document, x", "物料组.document, x"),

            // dict.logistics.material.group.dcy
            ("dict.logistics.material.group.dcy", "en-US", "dcy", "物料组.document, y"),
            // dict.logistics.material.group.dcy
            ("dict.logistics.material.group.dcy", "ja-JP", "dcy", "物料组.document, y"),
            // dict.logistics.material.group.dcy
            ("dict.logistics.material.group.dcy", "zh-CN", "document, y", "物料组.document, y"),
            // dict.logistics.material.group.dcy
            ("dict.logistics.material.group.dcy", "zh-HK", "document, y", "物料组.document, y"),

            // dict.logistics.material.group.dcz
            ("dict.logistics.material.group.dcz", "en-US", "dcz", "物料组.document, z"),
            // dict.logistics.material.group.dcz
            ("dict.logistics.material.group.dcz", "ja-JP", "dcz", "物料组.document, z"),
            // dict.logistics.material.group.dcz
            ("dict.logistics.material.group.dcz", "zh-CN", "document, z", "物料组.document, z"),
            // dict.logistics.material.group.dcz
            ("dict.logistics.material.group.dcz", "zh-HK", "document, z", "物料组.document, z"),

            // dict.logistics.material.group.dda
            ("dict.logistics.material.group.dda", "en-US", "dda", "物料组.data"),
            // dict.logistics.material.group.dda
            ("dict.logistics.material.group.dda", "ja-JP", "dda", "物料组.data"),
            // dict.logistics.material.group.dda
            ("dict.logistics.material.group.dda", "zh-CN", "data", "物料组.data"),
            // dict.logistics.material.group.dda
            ("dict.logistics.material.group.dda", "zh-HK", "data", "物料组.data"),

            // dict.logistics.material.group.ddk
            ("dict.logistics.material.group.ddk", "en-US", "ddk", "物料组.disk"),
            // dict.logistics.material.group.ddk
            ("dict.logistics.material.group.ddk", "ja-JP", "ddk", "物料组.disk"),
            // dict.logistics.material.group.ddk
            ("dict.logistics.material.group.ddk", "zh-CN", "disk", "物料组.disk"),
            // dict.logistics.material.group.ddk
            ("dict.logistics.material.group.ddk", "zh-HK", "disk", "物料组.disk"),

            // dict.logistics.material.group.dra
            ("dict.logistics.material.group.dra", "en-US", "dra", "物料组.dr"),
            // dict.logistics.material.group.dra
            ("dict.logistics.material.group.dra", "ja-JP", "dra", "物料组.dr"),
            // dict.logistics.material.group.dra
            ("dict.logistics.material.group.dra", "zh-CN", "dr", "物料组.dr"),
            // dict.logistics.material.group.dra
            ("dict.logistics.material.group.dra", "zh-HK", "dr", "物料组.dr"),

            // dict.logistics.material.group.dwa
            ("dict.logistics.material.group.dwa", "en-US", "dwa", "物料组.drawing, a"),
            // dict.logistics.material.group.dwa
            ("dict.logistics.material.group.dwa", "ja-JP", "dwa", "物料组.drawing, a"),
            // dict.logistics.material.group.dwa
            ("dict.logistics.material.group.dwa", "zh-CN", "drawing, a", "物料组.drawing, a"),
            // dict.logistics.material.group.dwa
            ("dict.logistics.material.group.dwa", "zh-HK", "drawing, a", "物料组.drawing, a"),

            // dict.logistics.material.group.dwb
            ("dict.logistics.material.group.dwb", "en-US", "dwb", "物料组.drawing, b"),
            // dict.logistics.material.group.dwb
            ("dict.logistics.material.group.dwb", "ja-JP", "dwb", "物料组.drawing, b"),
            // dict.logistics.material.group.dwb
            ("dict.logistics.material.group.dwb", "zh-CN", "drawing, b", "物料组.drawing, b"),
            // dict.logistics.material.group.dwb
            ("dict.logistics.material.group.dwb", "zh-HK", "drawing, b", "物料组.drawing, b"),

            // dict.logistics.material.group.dwc
            ("dict.logistics.material.group.dwc", "en-US", "dwc", "物料组.drawing, c"),
            // dict.logistics.material.group.dwc
            ("dict.logistics.material.group.dwc", "ja-JP", "dwc", "物料组.drawing, c"),
            // dict.logistics.material.group.dwc
            ("dict.logistics.material.group.dwc", "zh-CN", "drawing, c", "物料组.drawing, c"),
            // dict.logistics.material.group.dwc
            ("dict.logistics.material.group.dwc", "zh-HK", "drawing, c", "物料组.drawing, c"),

            // dict.logistics.material.group.dwd
            ("dict.logistics.material.group.dwd", "en-US", "dwd", "物料组.drawing, d"),
            // dict.logistics.material.group.dwd
            ("dict.logistics.material.group.dwd", "ja-JP", "dwd", "物料组.drawing, d"),
            // dict.logistics.material.group.dwd
            ("dict.logistics.material.group.dwd", "zh-CN", "drawing, d", "物料组.drawing, d"),
            // dict.logistics.material.group.dwd
            ("dict.logistics.material.group.dwd", "zh-HK", "drawing, d", "物料组.drawing, d"),

            // dict.logistics.material.group.dwe
            ("dict.logistics.material.group.dwe", "en-US", "dwe", "物料组.drawing, e"),
            // dict.logistics.material.group.dwe
            ("dict.logistics.material.group.dwe", "ja-JP", "dwe", "物料组.drawing, e"),
            // dict.logistics.material.group.dwe
            ("dict.logistics.material.group.dwe", "zh-CN", "drawing, e", "物料组.drawing, e"),
            // dict.logistics.material.group.dwe
            ("dict.logistics.material.group.dwe", "zh-HK", "drawing, e", "物料组.drawing, e"),

            // dict.logistics.material.group.dwf
            ("dict.logistics.material.group.dwf", "en-US", "dwf", "物料组.drawing, f"),
            // dict.logistics.material.group.dwf
            ("dict.logistics.material.group.dwf", "ja-JP", "dwf", "物料组.drawing, f"),
            // dict.logistics.material.group.dwf
            ("dict.logistics.material.group.dwf", "zh-CN", "drawing, f", "物料组.drawing, f"),
            // dict.logistics.material.group.dwf
            ("dict.logistics.material.group.dwf", "zh-HK", "drawing, f", "物料组.drawing, f"),

            // dict.logistics.material.group.dwg
            ("dict.logistics.material.group.dwg", "en-US", "dwg", "物料组.drawing, g"),
            // dict.logistics.material.group.dwg
            ("dict.logistics.material.group.dwg", "ja-JP", "dwg", "物料组.drawing, g"),
            // dict.logistics.material.group.dwg
            ("dict.logistics.material.group.dwg", "zh-CN", "drawing, g", "物料组.drawing, g"),
            // dict.logistics.material.group.dwg
            ("dict.logistics.material.group.dwg", "zh-HK", "drawing, g", "物料组.drawing, g"),

            // dict.logistics.material.group.dwh
            ("dict.logistics.material.group.dwh", "en-US", "dwh", "物料组.drawing, h"),
            // dict.logistics.material.group.dwh
            ("dict.logistics.material.group.dwh", "ja-JP", "dwh", "物料组.drawing, h"),
            // dict.logistics.material.group.dwh
            ("dict.logistics.material.group.dwh", "zh-CN", "drawing, h", "物料组.drawing, h"),
            // dict.logistics.material.group.dwh
            ("dict.logistics.material.group.dwh", "zh-HK", "drawing, h", "物料组.drawing, h"),

            // dict.logistics.material.group.dwi
            ("dict.logistics.material.group.dwi", "en-US", "dwi", "物料组.drawing, i"),
            // dict.logistics.material.group.dwi
            ("dict.logistics.material.group.dwi", "ja-JP", "dwi", "物料组.drawing, i"),
            // dict.logistics.material.group.dwi
            ("dict.logistics.material.group.dwi", "zh-CN", "drawing, i", "物料组.drawing, i"),
            // dict.logistics.material.group.dwi
            ("dict.logistics.material.group.dwi", "zh-HK", "drawing, i", "物料组.drawing, i"),

            // dict.logistics.material.group.dwj
            ("dict.logistics.material.group.dwj", "en-US", "dwj", "物料组.drawing, j"),
            // dict.logistics.material.group.dwj
            ("dict.logistics.material.group.dwj", "ja-JP", "dwj", "物料组.drawing, j"),
            // dict.logistics.material.group.dwj
            ("dict.logistics.material.group.dwj", "zh-CN", "drawing, j", "物料组.drawing, j"),
            // dict.logistics.material.group.dwj
            ("dict.logistics.material.group.dwj", "zh-HK", "drawing, j", "物料组.drawing, j"),

            // dict.logistics.material.group.dwk
            ("dict.logistics.material.group.dwk", "en-US", "dwk", "物料组.drawing, k"),
            // dict.logistics.material.group.dwk
            ("dict.logistics.material.group.dwk", "ja-JP", "dwk", "物料组.drawing, k"),
            // dict.logistics.material.group.dwk
            ("dict.logistics.material.group.dwk", "zh-CN", "drawing, k", "物料组.drawing, k"),
            // dict.logistics.material.group.dwk
            ("dict.logistics.material.group.dwk", "zh-HK", "drawing, k", "物料组.drawing, k"),

            // dict.logistics.material.group.dwl
            ("dict.logistics.material.group.dwl", "en-US", "dwl", "物料组.drawing, l"),
            // dict.logistics.material.group.dwl
            ("dict.logistics.material.group.dwl", "ja-JP", "dwl", "物料组.drawing, l"),
            // dict.logistics.material.group.dwl
            ("dict.logistics.material.group.dwl", "zh-CN", "drawing, l", "物料组.drawing, l"),
            // dict.logistics.material.group.dwl
            ("dict.logistics.material.group.dwl", "zh-HK", "drawing, l", "物料组.drawing, l"),

            // dict.logistics.material.group.dwm
            ("dict.logistics.material.group.dwm", "en-US", "dwm", "物料组.drawing, m"),
            // dict.logistics.material.group.dwm
            ("dict.logistics.material.group.dwm", "ja-JP", "dwm", "物料组.drawing, m"),
            // dict.logistics.material.group.dwm
            ("dict.logistics.material.group.dwm", "zh-CN", "drawing, m", "物料组.drawing, m"),
            // dict.logistics.material.group.dwm
            ("dict.logistics.material.group.dwm", "zh-HK", "drawing, m", "物料组.drawing, m"),

            // dict.logistics.material.group.dwn
            ("dict.logistics.material.group.dwn", "en-US", "dwn", "物料组.drawing, n"),
            // dict.logistics.material.group.dwn
            ("dict.logistics.material.group.dwn", "ja-JP", "dwn", "物料组.drawing, n"),
            // dict.logistics.material.group.dwn
            ("dict.logistics.material.group.dwn", "zh-CN", "drawing, n", "物料组.drawing, n"),
            // dict.logistics.material.group.dwn
            ("dict.logistics.material.group.dwn", "zh-HK", "drawing, n", "物料组.drawing, n"),

            // dict.logistics.material.group.dwo
            ("dict.logistics.material.group.dwo", "en-US", "dwo", "物料组.drawing, o"),
            // dict.logistics.material.group.dwo
            ("dict.logistics.material.group.dwo", "ja-JP", "dwo", "物料组.drawing, o"),
            // dict.logistics.material.group.dwo
            ("dict.logistics.material.group.dwo", "zh-CN", "drawing, o", "物料组.drawing, o"),
            // dict.logistics.material.group.dwo
            ("dict.logistics.material.group.dwo", "zh-HK", "drawing, o", "物料组.drawing, o"),

            // dict.logistics.material.group.dwp
            ("dict.logistics.material.group.dwp", "en-US", "dwp", "物料组.drawing, p"),
            // dict.logistics.material.group.dwp
            ("dict.logistics.material.group.dwp", "ja-JP", "dwp", "物料组.drawing, p"),
            // dict.logistics.material.group.dwp
            ("dict.logistics.material.group.dwp", "zh-CN", "drawing, p", "物料组.drawing, p"),
            // dict.logistics.material.group.dwp
            ("dict.logistics.material.group.dwp", "zh-HK", "drawing, p", "物料组.drawing, p"),

            // dict.logistics.material.group.dwq
            ("dict.logistics.material.group.dwq", "en-US", "dwq", "物料组.drawing, q"),
            // dict.logistics.material.group.dwq
            ("dict.logistics.material.group.dwq", "ja-JP", "dwq", "物料组.drawing, q"),
            // dict.logistics.material.group.dwq
            ("dict.logistics.material.group.dwq", "zh-CN", "drawing, q", "物料组.drawing, q"),
            // dict.logistics.material.group.dwq
            ("dict.logistics.material.group.dwq", "zh-HK", "drawing, q", "物料组.drawing, q"),

            // dict.logistics.material.group.dwr
            ("dict.logistics.material.group.dwr", "en-US", "dwr", "物料组.drawing, r"),
            // dict.logistics.material.group.dwr
            ("dict.logistics.material.group.dwr", "ja-JP", "dwr", "物料组.drawing, r"),
            // dict.logistics.material.group.dwr
            ("dict.logistics.material.group.dwr", "zh-CN", "drawing, r", "物料组.drawing, r"),
            // dict.logistics.material.group.dwr
            ("dict.logistics.material.group.dwr", "zh-HK", "drawing, r", "物料组.drawing, r"),

            // dict.logistics.material.group.dws
            ("dict.logistics.material.group.dws", "en-US", "dws", "物料组.drawing, s"),
            // dict.logistics.material.group.dws
            ("dict.logistics.material.group.dws", "ja-JP", "dws", "物料组.drawing, s"),
            // dict.logistics.material.group.dws
            ("dict.logistics.material.group.dws", "zh-CN", "drawing, s", "物料组.drawing, s"),
            // dict.logistics.material.group.dws
            ("dict.logistics.material.group.dws", "zh-HK", "drawing, s", "物料组.drawing, s"),

            // dict.logistics.material.group.eaa
            ("dict.logistics.material.group.eaa", "en-US", "eaa", "物料组.electronics"),
            // dict.logistics.material.group.eaa
            ("dict.logistics.material.group.eaa", "ja-JP", "eaa", "物料组.electronics"),
            // dict.logistics.material.group.eaa
            ("dict.logistics.material.group.eaa", "zh-CN", "electronics", "物料组.electronics"),
            // dict.logistics.material.group.eaa
            ("dict.logistics.material.group.eaa", "zh-HK", "electronics", "物料组.electronics"),

            // dict.logistics.material.group.eba
            ("dict.logistics.material.group.eba", "en-US", "eba", "物料组.connector"),
            // dict.logistics.material.group.eba
            ("dict.logistics.material.group.eba", "ja-JP", "eba", "物料组.connector"),
            // dict.logistics.material.group.eba
            ("dict.logistics.material.group.eba", "zh-CN", "connector", "物料组.connector"),
            // dict.logistics.material.group.eba
            ("dict.logistics.material.group.eba", "zh-HK", "connector", "物料组.connector"),

            // dict.logistics.material.group.eca
            ("dict.logistics.material.group.eca", "en-US", "eca", "物料组.connector, a"),
            // dict.logistics.material.group.eca
            ("dict.logistics.material.group.eca", "ja-JP", "eca", "物料组.connector, a"),
            // dict.logistics.material.group.eca
            ("dict.logistics.material.group.eca", "zh-CN", "connector, a", "物料组.connector, a"),
            // dict.logistics.material.group.eca
            ("dict.logistics.material.group.eca", "zh-HK", "connector, a", "物料组.connector, a"),

            // dict.logistics.material.group.ecb
            ("dict.logistics.material.group.ecb", "en-US", "ecb", "物料组.cable"),
            // dict.logistics.material.group.ecb
            ("dict.logistics.material.group.ecb", "ja-JP", "ecb", "物料组.cable"),
            // dict.logistics.material.group.ecb
            ("dict.logistics.material.group.ecb", "zh-CN", "cable", "物料组.cable"),
            // dict.logistics.material.group.ecb
            ("dict.logistics.material.group.ecb", "zh-HK", "cable", "物料组.cable"),

            // dict.logistics.material.group.ecc
            ("dict.logistics.material.group.ecc", "en-US", "ecc", "物料组.connector, c"),
            // dict.logistics.material.group.ecc
            ("dict.logistics.material.group.ecc", "ja-JP", "ecc", "物料组.connector, c"),
            // dict.logistics.material.group.ecc
            ("dict.logistics.material.group.ecc", "zh-CN", "connector, c", "物料组.connector, c"),
            // dict.logistics.material.group.ecc
            ("dict.logistics.material.group.ecc", "zh-HK", "connector, c", "物料组.connector, c"),

            // dict.logistics.material.group.ecd
            ("dict.logistics.material.group.ecd", "en-US", "ecd", "物料组.connector, d"),
            // dict.logistics.material.group.ecd
            ("dict.logistics.material.group.ecd", "ja-JP", "ecd", "物料组.connector, d"),
            // dict.logistics.material.group.ecd
            ("dict.logistics.material.group.ecd", "zh-CN", "connector, d", "物料组.connector, d"),
            // dict.logistics.material.group.ecd
            ("dict.logistics.material.group.ecd", "zh-HK", "connector, d", "物料组.connector, d"),

            // dict.logistics.material.group.ece
            ("dict.logistics.material.group.ece", "en-US", "ece", "物料组.connector, e"),
            // dict.logistics.material.group.ece
            ("dict.logistics.material.group.ece", "ja-JP", "ece", "物料组.connector, e"),
            // dict.logistics.material.group.ece
            ("dict.logistics.material.group.ece", "zh-CN", "connector, e", "物料组.connector, e"),
            // dict.logistics.material.group.ece
            ("dict.logistics.material.group.ece", "zh-HK", "connector, e", "物料组.connector, e"),

            // dict.logistics.material.group.ecf
            ("dict.logistics.material.group.ecf", "en-US", "ecf", "物料组.connector, f"),
            // dict.logistics.material.group.ecf
            ("dict.logistics.material.group.ecf", "ja-JP", "ecf", "物料组.connector, f"),
            // dict.logistics.material.group.ecf
            ("dict.logistics.material.group.ecf", "zh-CN", "connector, f", "物料组.connector, f"),
            // dict.logistics.material.group.ecf
            ("dict.logistics.material.group.ecf", "zh-HK", "connector, f", "物料组.connector, f"),

            // dict.logistics.material.group.ecg
            ("dict.logistics.material.group.ecg", "en-US", "ecg", "物料组.connector, g"),
            // dict.logistics.material.group.ecg
            ("dict.logistics.material.group.ecg", "ja-JP", "ecg", "物料组.connector, g"),
            // dict.logistics.material.group.ecg
            ("dict.logistics.material.group.ecg", "zh-CN", "connector, g", "物料组.connector, g"),
            // dict.logistics.material.group.ecg
            ("dict.logistics.material.group.ecg", "zh-HK", "connector, g", "物料组.connector, g"),

            // dict.logistics.material.group.ech
            ("dict.logistics.material.group.ech", "en-US", "ech", "物料组.connector, h"),
            // dict.logistics.material.group.ech
            ("dict.logistics.material.group.ech", "ja-JP", "ech", "物料组.connector, h"),
            // dict.logistics.material.group.ech
            ("dict.logistics.material.group.ech", "zh-CN", "connector, h", "物料组.connector, h"),
            // dict.logistics.material.group.ech
            ("dict.logistics.material.group.ech", "zh-HK", "connector, h", "物料组.connector, h"),

            // dict.logistics.material.group.eci
            ("dict.logistics.material.group.eci", "en-US", "eci", "物料组.connector, i"),
            // dict.logistics.material.group.eci
            ("dict.logistics.material.group.eci", "ja-JP", "eci", "物料组.connector, i"),
            // dict.logistics.material.group.eci
            ("dict.logistics.material.group.eci", "zh-CN", "connector, i", "物料组.connector, i"),
            // dict.logistics.material.group.eci
            ("dict.logistics.material.group.eci", "zh-HK", "connector, i", "物料组.connector, i"),

            // dict.logistics.material.group.ecj
            ("dict.logistics.material.group.ecj", "en-US", "ecj", "物料组.connector, j"),
            // dict.logistics.material.group.ecj
            ("dict.logistics.material.group.ecj", "ja-JP", "ecj", "物料组.connector, j"),
            // dict.logistics.material.group.ecj
            ("dict.logistics.material.group.ecj", "zh-CN", "connector, j", "物料组.connector, j"),
            // dict.logistics.material.group.ecj
            ("dict.logistics.material.group.ecj", "zh-HK", "connector, j", "物料组.connector, j"),

            // dict.logistics.material.group.eck
            ("dict.logistics.material.group.eck", "en-US", "eck", "物料组.connector, k"),
            // dict.logistics.material.group.eck
            ("dict.logistics.material.group.eck", "ja-JP", "eck", "物料组.connector, k"),
            // dict.logistics.material.group.eck
            ("dict.logistics.material.group.eck", "zh-CN", "connector, k", "物料组.connector, k"),
            // dict.logistics.material.group.eck
            ("dict.logistics.material.group.eck", "zh-HK", "connector, k", "物料组.connector, k"),

            // dict.logistics.material.group.ecl
            ("dict.logistics.material.group.ecl", "en-US", "ecl", "物料组.connector, l"),
            // dict.logistics.material.group.ecl
            ("dict.logistics.material.group.ecl", "ja-JP", "ecl", "物料组.connector, l"),
            // dict.logistics.material.group.ecl
            ("dict.logistics.material.group.ecl", "zh-CN", "connector, l", "物料组.connector, l"),
            // dict.logistics.material.group.ecl
            ("dict.logistics.material.group.ecl", "zh-HK", "connector, l", "物料组.connector, l"),

            // dict.logistics.material.group.ecm
            ("dict.logistics.material.group.ecm", "en-US", "ecm", "物料组.connector, m"),
            // dict.logistics.material.group.ecm
            ("dict.logistics.material.group.ecm", "ja-JP", "ecm", "物料组.connector, m"),
            // dict.logistics.material.group.ecm
            ("dict.logistics.material.group.ecm", "zh-CN", "connector, m", "物料组.connector, m"),
            // dict.logistics.material.group.ecm
            ("dict.logistics.material.group.ecm", "zh-HK", "connector, m", "物料组.connector, m"),

            // dict.logistics.material.group.ecn
            ("dict.logistics.material.group.ecn", "en-US", "ecn", "物料组.connector, n"),
            // dict.logistics.material.group.ecn
            ("dict.logistics.material.group.ecn", "ja-JP", "ecn", "物料组.connector, n"),
            // dict.logistics.material.group.ecn
            ("dict.logistics.material.group.ecn", "zh-CN", "connector, n", "物料组.connector, n"),
            // dict.logistics.material.group.ecn
            ("dict.logistics.material.group.ecn", "zh-HK", "connector, n", "物料组.connector, n"),

            // dict.logistics.material.group.eco
            ("dict.logistics.material.group.eco", "en-US", "eco", "物料组.connector, o"),
            // dict.logistics.material.group.eco
            ("dict.logistics.material.group.eco", "ja-JP", "eco", "物料组.connector, o"),
            // dict.logistics.material.group.eco
            ("dict.logistics.material.group.eco", "zh-CN", "connector, o", "物料组.connector, o"),
            // dict.logistics.material.group.eco
            ("dict.logistics.material.group.eco", "zh-HK", "connector, o", "物料组.connector, o"),

            // dict.logistics.material.group.ecp
            ("dict.logistics.material.group.ecp", "en-US", "ecp", "物料组.connector, p"),
            // dict.logistics.material.group.ecp
            ("dict.logistics.material.group.ecp", "ja-JP", "ecp", "物料组.connector, p"),
            // dict.logistics.material.group.ecp
            ("dict.logistics.material.group.ecp", "zh-CN", "connector, p", "物料组.connector, p"),
            // dict.logistics.material.group.ecp
            ("dict.logistics.material.group.ecp", "zh-HK", "connector, p", "物料组.connector, p"),

            // dict.logistics.material.group.ecq
            ("dict.logistics.material.group.ecq", "en-US", "ecq", "物料组.connector, q"),
            // dict.logistics.material.group.ecq
            ("dict.logistics.material.group.ecq", "ja-JP", "ecq", "物料组.connector, q"),
            // dict.logistics.material.group.ecq
            ("dict.logistics.material.group.ecq", "zh-CN", "connector, q", "物料组.connector, q"),
            // dict.logistics.material.group.ecq
            ("dict.logistics.material.group.ecq", "zh-HK", "connector, q", "物料组.connector, q"),

            // dict.logistics.material.group.ecr
            ("dict.logistics.material.group.ecr", "en-US", "ecr", "物料组.connector, r"),
            // dict.logistics.material.group.ecr
            ("dict.logistics.material.group.ecr", "ja-JP", "ecr", "物料组.connector, r"),
            // dict.logistics.material.group.ecr
            ("dict.logistics.material.group.ecr", "zh-CN", "connector, r", "物料组.connector, r"),
            // dict.logistics.material.group.ecr
            ("dict.logistics.material.group.ecr", "zh-HK", "connector, r", "物料组.connector, r"),

            // dict.logistics.material.group.ecs
            ("dict.logistics.material.group.ecs", "en-US", "ecs", "物料组.connector, s"),
            // dict.logistics.material.group.ecs
            ("dict.logistics.material.group.ecs", "ja-JP", "ecs", "物料组.connector, s"),
            // dict.logistics.material.group.ecs
            ("dict.logistics.material.group.ecs", "zh-CN", "connector, s", "物料组.connector, s"),
            // dict.logistics.material.group.ecs
            ("dict.logistics.material.group.ecs", "zh-HK", "connector, s", "物料组.connector, s"),

            // dict.logistics.material.group.ect
            ("dict.logistics.material.group.ect", "en-US", "ect", "物料组.connector, t"),
            // dict.logistics.material.group.ect
            ("dict.logistics.material.group.ect", "ja-JP", "ect", "物料组.connector, t"),
            // dict.logistics.material.group.ect
            ("dict.logistics.material.group.ect", "zh-CN", "connector, t", "物料组.connector, t"),
            // dict.logistics.material.group.ect
            ("dict.logistics.material.group.ect", "zh-HK", "connector, t", "物料组.connector, t"),

            // dict.logistics.material.group.ecu
            ("dict.logistics.material.group.ecu", "en-US", "ecu", "物料组.connector, u"),
            // dict.logistics.material.group.ecu
            ("dict.logistics.material.group.ecu", "ja-JP", "ecu", "物料组.connector, u"),
            // dict.logistics.material.group.ecu
            ("dict.logistics.material.group.ecu", "zh-CN", "connector, u", "物料组.connector, u"),
            // dict.logistics.material.group.ecu
            ("dict.logistics.material.group.ecu", "zh-HK", "connector, u", "物料组.connector, u"),

            // dict.logistics.material.group.ecv
            ("dict.logistics.material.group.ecv", "en-US", "ecv", "物料组.connector, v"),
            // dict.logistics.material.group.ecv
            ("dict.logistics.material.group.ecv", "ja-JP", "ecv", "物料组.connector, v"),
            // dict.logistics.material.group.ecv
            ("dict.logistics.material.group.ecv", "zh-CN", "connector, v", "物料组.connector, v"),
            // dict.logistics.material.group.ecv
            ("dict.logistics.material.group.ecv", "zh-HK", "connector, v", "物料组.connector, v"),

            // dict.logistics.material.group.ecw
            ("dict.logistics.material.group.ecw", "en-US", "ecw", "物料组.connector, w"),
            // dict.logistics.material.group.ecw
            ("dict.logistics.material.group.ecw", "ja-JP", "ecw", "物料组.connector, w"),
            // dict.logistics.material.group.ecw
            ("dict.logistics.material.group.ecw", "zh-CN", "connector, w", "物料组.connector, w"),
            // dict.logistics.material.group.ecw
            ("dict.logistics.material.group.ecw", "zh-HK", "connector, w", "物料组.connector, w"),

            // dict.logistics.material.group.ecx
            ("dict.logistics.material.group.ecx", "en-US", "ecx", "物料组.connector, x"),
            // dict.logistics.material.group.ecx
            ("dict.logistics.material.group.ecx", "ja-JP", "ecx", "物料组.connector, x"),
            // dict.logistics.material.group.ecx
            ("dict.logistics.material.group.ecx", "zh-CN", "connector, x", "物料组.connector, x"),
            // dict.logistics.material.group.ecx
            ("dict.logistics.material.group.ecx", "zh-HK", "connector, x", "物料组.connector, x"),

            // dict.logistics.material.group.ecy
            ("dict.logistics.material.group.ecy", "en-US", "ecy", "物料组.connector, y"),
            // dict.logistics.material.group.ecy
            ("dict.logistics.material.group.ecy", "ja-JP", "ecy", "物料组.connector, y"),
            // dict.logistics.material.group.ecy
            ("dict.logistics.material.group.ecy", "zh-CN", "connector, y", "物料组.connector, y"),
            // dict.logistics.material.group.ecy
            ("dict.logistics.material.group.ecy", "zh-HK", "connector, y", "物料组.connector, y"),

            // dict.logistics.material.group.ecz
            ("dict.logistics.material.group.ecz", "en-US", "ecz", "物料组.connector, z"),
            // dict.logistics.material.group.ecz
            ("dict.logistics.material.group.ecz", "ja-JP", "ecz", "物料组.connector, z"),
            // dict.logistics.material.group.ecz
            ("dict.logistics.material.group.ecz", "zh-CN", "connector, z", "物料组.connector, z"),
            // dict.logistics.material.group.ecz
            ("dict.logistics.material.group.ecz", "zh-HK", "connector, z", "物料组.connector, z"),

            // dict.logistics.material.group.ena
            ("dict.logistics.material.group.ena", "en-US", "ena", "物料组.sensor"),
            // dict.logistics.material.group.ena
            ("dict.logistics.material.group.ena", "ja-JP", "ena", "物料组.sensor"),
            // dict.logistics.material.group.ena
            ("dict.logistics.material.group.ena", "zh-CN", "sensor", "物料组.sensor"),
            // dict.logistics.material.group.ena
            ("dict.logistics.material.group.ena", "zh-HK", "sensor", "物料组.sensor"),

            // dict.logistics.material.group.esa
            ("dict.logistics.material.group.esa", "en-US", "esa", "物料组.switch"),
            // dict.logistics.material.group.esa
            ("dict.logistics.material.group.esa", "ja-JP", "esa", "物料组.switch"),
            // dict.logistics.material.group.esa
            ("dict.logistics.material.group.esa", "zh-CN", "switch", "物料组.switch"),
            // dict.logistics.material.group.esa
            ("dict.logistics.material.group.esa", "zh-HK", "switch", "物料组.switch"),

            // dict.logistics.material.group.era
            ("dict.logistics.material.group.era", "en-US", "era", "物料组.relay"),
            // dict.logistics.material.group.era
            ("dict.logistics.material.group.era", "ja-JP", "era", "物料组.relay"),
            // dict.logistics.material.group.era
            ("dict.logistics.material.group.era", "zh-CN", "relay", "物料组.relay"),
            // dict.logistics.material.group.era
            ("dict.logistics.material.group.era", "zh-HK", "relay", "物料组.relay"),

            // dict.logistics.material.group.exa
            ("dict.logistics.material.group.exa", "en-US", "exa", "物料组.crystal"),
            // dict.logistics.material.group.exa
            ("dict.logistics.material.group.exa", "ja-JP", "exa", "物料组.crystal"),
            // dict.logistics.material.group.exa
            ("dict.logistics.material.group.exa", "zh-CN", "crystal", "物料组.crystal"),
            // dict.logistics.material.group.exa
            ("dict.logistics.material.group.exa", "zh-HK", "crystal", "物料组.crystal"),

            // dict.logistics.material.group.faa
            ("dict.logistics.material.group.faa", "en-US", "faa", "物料组.display"),
            // dict.logistics.material.group.faa
            ("dict.logistics.material.group.faa", "ja-JP", "faa", "物料组.display"),
            // dict.logistics.material.group.faa
            ("dict.logistics.material.group.faa", "zh-CN", "display", "物料组.display"),
            // dict.logistics.material.group.faa
            ("dict.logistics.material.group.faa", "zh-HK", "display", "物料组.display"),

            // dict.logistics.material.group.fla
            ("dict.logistics.material.group.fla", "en-US", "fla", "物料组.lcd"),
            // dict.logistics.material.group.fla
            ("dict.logistics.material.group.fla", "ja-JP", "fla", "物料组.lcd"),
            // dict.logistics.material.group.fla
            ("dict.logistics.material.group.fla", "zh-CN", "lcd", "物料组.lcd"),
            // dict.logistics.material.group.fla
            ("dict.logistics.material.group.fla", "zh-HK", "lcd", "物料组.lcd"),

            // dict.logistics.material.group.fld
            ("dict.logistics.material.group.fld", "en-US", "fld", "物料组.led"),
            // dict.logistics.material.group.fld
            ("dict.logistics.material.group.fld", "ja-JP", "fld", "物料组.led"),
            // dict.logistics.material.group.fld
            ("dict.logistics.material.group.fld", "zh-CN", "led", "物料组.led"),
            // dict.logistics.material.group.fld
            ("dict.logistics.material.group.fld", "zh-HK", "led", "物料组.led"),

            // dict.logistics.material.group.fma
            ("dict.logistics.material.group.fma", "en-US", "fma", "物料组.pcb"),
            // dict.logistics.material.group.fma
            ("dict.logistics.material.group.fma", "ja-JP", "fma", "物料组.pcb"),
            // dict.logistics.material.group.fma
            ("dict.logistics.material.group.fma", "zh-CN", "pcb", "物料组.pcb"),
            // dict.logistics.material.group.fma
            ("dict.logistics.material.group.fma", "zh-HK", "pcb", "物料组.pcb"),

            // dict.logistics.material.group.fpc
            ("dict.logistics.material.group.fpc", "en-US", "fpc", "物料组.pcba"),
            // dict.logistics.material.group.fpc
            ("dict.logistics.material.group.fpc", "ja-JP", "fpc", "物料组.pcba"),
            // dict.logistics.material.group.fpc
            ("dict.logistics.material.group.fpc", "zh-CN", "pcba", "物料组.pcba"),
            // dict.logistics.material.group.fpc
            ("dict.logistics.material.group.fpc", "zh-HK", "pcba", "物料组.pcba"),

            // dict.logistics.material.group.fxa
            ("dict.logistics.material.group.fxa", "en-US", "fxa", "物料组.flex"),
            // dict.logistics.material.group.fxa
            ("dict.logistics.material.group.fxa", "ja-JP", "fxa", "物料组.flex"),
            // dict.logistics.material.group.fxa
            ("dict.logistics.material.group.fxa", "zh-CN", "flex", "物料组.flex"),
            // dict.logistics.material.group.fxa
            ("dict.logistics.material.group.fxa", "zh-HK", "flex", "物料组.flex"),

            // dict.logistics.material.group.haa
            ("dict.logistics.material.group.haa", "en-US", "haa", "物料组.head"),
            // dict.logistics.material.group.haa
            ("dict.logistics.material.group.haa", "ja-JP", "haa", "物料组.head"),
            // dict.logistics.material.group.haa
            ("dict.logistics.material.group.haa", "zh-CN", "head", "物料组.head"),
            // dict.logistics.material.group.haa
            ("dict.logistics.material.group.haa", "zh-HK", "head", "物料组.head"),

            // dict.logistics.material.group.hca
            ("dict.logistics.material.group.hca", "en-US", "hca", "物料组.core"),
            // dict.logistics.material.group.hca
            ("dict.logistics.material.group.hca", "ja-JP", "hca", "物料组.core"),
            // dict.logistics.material.group.hca
            ("dict.logistics.material.group.hca", "zh-CN", "core", "物料组.core"),
            // dict.logistics.material.group.hca
            ("dict.logistics.material.group.hca", "zh-HK", "core", "物料组.core"),

            // dict.logistics.material.group.kaa
            ("dict.logistics.material.group.kaa", "en-US", "kaa", "物料组.optical"),
            // dict.logistics.material.group.kaa
            ("dict.logistics.material.group.kaa", "ja-JP", "kaa", "物料组.optical"),
            // dict.logistics.material.group.kaa
            ("dict.logistics.material.group.kaa", "zh-CN", "optical", "物料组.optical"),
            // dict.logistics.material.group.kaa
            ("dict.logistics.material.group.kaa", "zh-HK", "optical", "物料组.optical"),

            // dict.logistics.material.group.kla
            ("dict.logistics.material.group.kla", "en-US", "kla", "物料组.lens"),
            // dict.logistics.material.group.kla
            ("dict.logistics.material.group.kla", "ja-JP", "kla", "物料组.lens"),
            // dict.logistics.material.group.kla
            ("dict.logistics.material.group.kla", "zh-CN", "lens", "物料组.lens"),
            // dict.logistics.material.group.kla
            ("dict.logistics.material.group.kla", "zh-HK", "lens", "物料组.lens"),

            // dict.logistics.material.group.kpa
            ("dict.logistics.material.group.kpa", "en-US", "kpa", "物料组.prism"),
            // dict.logistics.material.group.kpa
            ("dict.logistics.material.group.kpa", "ja-JP", "kpa", "物料组.prism"),
            // dict.logistics.material.group.kpa
            ("dict.logistics.material.group.kpa", "zh-CN", "prism", "物料组.prism"),
            // dict.logistics.material.group.kpa
            ("dict.logistics.material.group.kpa", "zh-HK", "prism", "物料组.prism"),

            // dict.logistics.material.group.kfa
            ("dict.logistics.material.group.kfa", "en-US", "kfa", "物料组.filter"),
            // dict.logistics.material.group.kfa
            ("dict.logistics.material.group.kfa", "ja-JP", "kfa", "物料组.filter"),
            // dict.logistics.material.group.kfa
            ("dict.logistics.material.group.kfa", "zh-CN", "filter", "物料组.filter"),
            // dict.logistics.material.group.kfa
            ("dict.logistics.material.group.kfa", "zh-HK", "filter", "物料组.filter"),

            // dict.logistics.material.group.kma
            ("dict.logistics.material.group.kma", "en-US", "kma", "物料组.mirror"),
            // dict.logistics.material.group.kma
            ("dict.logistics.material.group.kma", "ja-JP", "kma", "物料组.mirror"),
            // dict.logistics.material.group.kma
            ("dict.logistics.material.group.kma", "zh-CN", "mirror", "物料组.mirror"),
            // dict.logistics.material.group.kma
            ("dict.logistics.material.group.kma", "zh-HK", "mirror", "物料组.mirror"),

            // dict.logistics.material.group.lma
            ("dict.logistics.material.group.lma", "en-US", "lma", "物料组.motor"),
            // dict.logistics.material.group.lma
            ("dict.logistics.material.group.lma", "ja-JP", "lma", "物料组.motor"),
            // dict.logistics.material.group.lma
            ("dict.logistics.material.group.lma", "zh-CN", "motor", "物料组.motor"),
            // dict.logistics.material.group.lma
            ("dict.logistics.material.group.lma", "zh-HK", "motor", "物料组.motor"),

            // dict.logistics.material.group.lta
            ("dict.logistics.material.group.lta", "en-US", "lta", "物料组.transformer"),
            // dict.logistics.material.group.lta
            ("dict.logistics.material.group.lta", "ja-JP", "lta", "物料组.transformer"),
            // dict.logistics.material.group.lta
            ("dict.logistics.material.group.lta", "zh-CN", "transformer", "物料组.transformer"),
            // dict.logistics.material.group.lta
            ("dict.logistics.material.group.lta", "zh-HK", "transformer", "物料组.transformer"),

            // dict.logistics.material.group.lca
            ("dict.logistics.material.group.lca", "en-US", "lca", "物料组.coil"),
            // dict.logistics.material.group.lca
            ("dict.logistics.material.group.lca", "ja-JP", "lca", "物料组.coil"),
            // dict.logistics.material.group.lca
            ("dict.logistics.material.group.lca", "zh-CN", "coil", "物料组.coil"),
            // dict.logistics.material.group.lca
            ("dict.logistics.material.group.lca", "zh-HK", "coil", "物料组.coil"),

            // dict.logistics.material.group.maa
            ("dict.logistics.material.group.maa", "en-US", "maa", "物料组.mechanical"),
            // dict.logistics.material.group.maa
            ("dict.logistics.material.group.maa", "ja-JP", "maa", "物料组.mechanical"),
            // dict.logistics.material.group.maa
            ("dict.logistics.material.group.maa", "zh-CN", "mechanical", "物料组.mechanical"),
            // dict.logistics.material.group.maa
            ("dict.logistics.material.group.maa", "zh-HK", "mechanical", "物料组.mechanical"),

            // dict.logistics.material.group.msa
            ("dict.logistics.material.group.msa", "en-US", "msa", "物料组.shaft"),
            // dict.logistics.material.group.msa
            ("dict.logistics.material.group.msa", "ja-JP", "msa", "物料组.shaft"),
            // dict.logistics.material.group.msa
            ("dict.logistics.material.group.msa", "zh-CN", "shaft", "物料组.shaft"),
            // dict.logistics.material.group.msa
            ("dict.logistics.material.group.msa", "zh-HK", "shaft", "物料组.shaft"),

            // dict.logistics.material.group.mba
            ("dict.logistics.material.group.mba", "en-US", "mba", "物料组.bearing"),
            // dict.logistics.material.group.mba
            ("dict.logistics.material.group.mba", "ja-JP", "mba", "物料组.bearing"),
            // dict.logistics.material.group.mba
            ("dict.logistics.material.group.mba", "zh-CN", "bearing", "物料组.bearing"),
            // dict.logistics.material.group.mba
            ("dict.logistics.material.group.mba", "zh-HK", "bearing", "物料组.bearing"),

            // dict.logistics.material.group.mga
            ("dict.logistics.material.group.mga", "en-US", "mga", "物料组.gear"),
            // dict.logistics.material.group.mga
            ("dict.logistics.material.group.mga", "ja-JP", "mga", "物料组.gear"),
            // dict.logistics.material.group.mga
            ("dict.logistics.material.group.mga", "zh-CN", "gear", "物料组.gear"),
            // dict.logistics.material.group.mga
            ("dict.logistics.material.group.mga", "zh-HK", "gear", "物料组.gear"),

            // dict.logistics.material.group.mwa
            ("dict.logistics.material.group.mwa", "en-US", "mwa", "物料组.washer"),
            // dict.logistics.material.group.mwa
            ("dict.logistics.material.group.mwa", "ja-JP", "mwa", "物料组.washer"),
            // dict.logistics.material.group.mwa
            ("dict.logistics.material.group.mwa", "zh-CN", "washer", "物料组.washer"),
            // dict.logistics.material.group.mwa
            ("dict.logistics.material.group.mwa", "zh-HK", "washer", "物料组.washer"),

            // dict.logistics.material.group.mna
            ("dict.logistics.material.group.mna", "en-US", "mna", "物料组.nut"),
            // dict.logistics.material.group.mna
            ("dict.logistics.material.group.mna", "ja-JP", "mna", "物料组.nut"),
            // dict.logistics.material.group.mna
            ("dict.logistics.material.group.mna", "zh-CN", "nut", "物料组.nut"),
            // dict.logistics.material.group.mna
            ("dict.logistics.material.group.mna", "zh-HK", "nut", "物料组.nut"),

            // dict.logistics.material.group.naa
            ("dict.logistics.material.group.naa", "en-US", "naa", "物料组.structural"),
            // dict.logistics.material.group.naa
            ("dict.logistics.material.group.naa", "ja-JP", "naa", "物料组.structural"),
            // dict.logistics.material.group.naa
            ("dict.logistics.material.group.naa", "zh-CN", "structural", "物料组.structural"),
            // dict.logistics.material.group.naa
            ("dict.logistics.material.group.naa", "zh-HK", "structural", "物料组.structural"),

            // dict.logistics.material.group.npa
            ("dict.logistics.material.group.npa", "en-US", "npa", "物料组.panel"),
            // dict.logistics.material.group.npa
            ("dict.logistics.material.group.npa", "ja-JP", "npa", "物料组.panel"),
            // dict.logistics.material.group.npa
            ("dict.logistics.material.group.npa", "zh-CN", "panel", "物料组.panel"),
            // dict.logistics.material.group.npa
            ("dict.logistics.material.group.npa", "zh-HK", "panel", "物料组.panel"),

            // dict.logistics.material.group.nca
            ("dict.logistics.material.group.nca", "en-US", "nca", "物料组.case"),
            // dict.logistics.material.group.nca
            ("dict.logistics.material.group.nca", "ja-JP", "nca", "物料组.case"),
            // dict.logistics.material.group.nca
            ("dict.logistics.material.group.nca", "zh-CN", "case", "物料组.case"),
            // dict.logistics.material.group.nca
            ("dict.logistics.material.group.nca", "zh-HK", "case", "物料组.case"),

            // dict.logistics.material.group.ncv
            ("dict.logistics.material.group.ncv", "en-US", "ncv", "物料组.cover"),
            // dict.logistics.material.group.ncv
            ("dict.logistics.material.group.ncv", "ja-JP", "ncv", "物料组.cover"),
            // dict.logistics.material.group.ncv
            ("dict.logistics.material.group.ncv", "zh-CN", "cover", "物料组.cover"),
            // dict.logistics.material.group.ncv
            ("dict.logistics.material.group.ncv", "zh-HK", "cover", "物料组.cover"),

            // dict.logistics.material.group.nfa
            ("dict.logistics.material.group.nfa", "en-US", "nfa", "物料组.frame"),
            // dict.logistics.material.group.nfa
            ("dict.logistics.material.group.nfa", "ja-JP", "nfa", "物料组.frame"),
            // dict.logistics.material.group.nfa
            ("dict.logistics.material.group.nfa", "zh-CN", "frame", "物料组.frame"),
            // dict.logistics.material.group.nfa
            ("dict.logistics.material.group.nfa", "zh-HK", "frame", "物料组.frame"),

            // dict.logistics.material.group.nba
            ("dict.logistics.material.group.nba", "en-US", "nba", "物料组.bracket"),
            // dict.logistics.material.group.nba
            ("dict.logistics.material.group.nba", "ja-JP", "nba", "物料组.bracket"),
            // dict.logistics.material.group.nba
            ("dict.logistics.material.group.nba", "zh-CN", "bracket", "物料组.bracket"),
            // dict.logistics.material.group.nba
            ("dict.logistics.material.group.nba", "zh-HK", "bracket", "物料组.bracket"),

            // dict.logistics.material.group.npk
            ("dict.logistics.material.group.npk", "en-US", "npk", "物料组.packing"),
            // dict.logistics.material.group.npk
            ("dict.logistics.material.group.npk", "ja-JP", "npk", "物料组.packing"),
            // dict.logistics.material.group.npk
            ("dict.logistics.material.group.npk", "zh-CN", "packing", "物料组.packing"),
            // dict.logistics.material.group.npk
            ("dict.logistics.material.group.npk", "zh-HK", "packing", "物料组.packing"),

            // dict.logistics.material.group.oaa
            ("dict.logistics.material.group.oaa", "en-US", "oaa", "物料组.assembly"),
            // dict.logistics.material.group.oaa
            ("dict.logistics.material.group.oaa", "ja-JP", "oaa", "物料组.assembly"),
            // dict.logistics.material.group.oaa
            ("dict.logistics.material.group.oaa", "zh-CN", "assembly", "物料组.assembly"),
            // dict.logistics.material.group.oaa
            ("dict.logistics.material.group.oaa", "zh-HK", "assembly", "物料组.assembly"),

            // dict.logistics.material.group.oma
            ("dict.logistics.material.group.oma", "en-US", "oma", "物料组.module"),
            // dict.logistics.material.group.oma
            ("dict.logistics.material.group.oma", "ja-JP", "oma", "物料组.module"),
            // dict.logistics.material.group.oma
            ("dict.logistics.material.group.oma", "zh-CN", "module", "物料组.module"),
            // dict.logistics.material.group.oma
            ("dict.logistics.material.group.oma", "zh-HK", "module", "物料组.module"),

            // dict.logistics.material.group.osa
            ("dict.logistics.material.group.osa", "en-US", "osa", "物料组.sub assembly"),
            // dict.logistics.material.group.osa
            ("dict.logistics.material.group.osa", "ja-JP", "osa", "物料组.sub assembly"),
            // dict.logistics.material.group.osa
            ("dict.logistics.material.group.osa", "zh-CN", "sub assembly", "物料组.sub assembly"),
            // dict.logistics.material.group.osa
            ("dict.logistics.material.group.osa", "zh-HK", "sub assembly", "物料组.sub assembly"),

            // dict.logistics.material.group.paa
            ("dict.logistics.material.group.paa", "en-US", "paa", "物料组.power"),
            // dict.logistics.material.group.paa
            ("dict.logistics.material.group.paa", "ja-JP", "paa", "物料组.power"),
            // dict.logistics.material.group.paa
            ("dict.logistics.material.group.paa", "zh-CN", "power", "物料组.power"),
            // dict.logistics.material.group.paa
            ("dict.logistics.material.group.paa", "zh-HK", "power", "物料组.power"),

            // dict.logistics.material.group.pba
            ("dict.logistics.material.group.pba", "en-US", "pba", "物料组.battery"),
            // dict.logistics.material.group.pba
            ("dict.logistics.material.group.pba", "ja-JP", "pba", "物料组.battery"),
            // dict.logistics.material.group.pba
            ("dict.logistics.material.group.pba", "zh-CN", "battery", "物料组.battery"),
            // dict.logistics.material.group.pba
            ("dict.logistics.material.group.pba", "zh-HK", "battery", "物料组.battery"),

            // dict.logistics.material.group.pda
            ("dict.logistics.material.group.pda", "en-US", "pda", "物料组.adapter"),
            // dict.logistics.material.group.pda
            ("dict.logistics.material.group.pda", "ja-JP", "pda", "物料组.adapter"),
            // dict.logistics.material.group.pda
            ("dict.logistics.material.group.pda", "zh-CN", "adapter", "物料组.adapter"),
            // dict.logistics.material.group.pda
            ("dict.logistics.material.group.pda", "zh-HK", "adapter", "物料组.adapter"),

            // dict.logistics.material.group.psa
            ("dict.logistics.material.group.psa", "en-US", "psa", "物料组.switch, power"),
            // dict.logistics.material.group.psa
            ("dict.logistics.material.group.psa", "ja-JP", "psa", "物料组.switch, power"),
            // dict.logistics.material.group.psa
            ("dict.logistics.material.group.psa", "zh-CN", "switch, power", "物料组.switch, power"),
            // dict.logistics.material.group.psa
            ("dict.logistics.material.group.psa", "zh-HK", "switch, power", "物料组.switch, power"),

            // dict.logistics.material.group.qaa
            ("dict.logistics.material.group.qaa", "en-US", "qaa", "物料组.ic"),
            // dict.logistics.material.group.qaa
            ("dict.logistics.material.group.qaa", "ja-JP", "qaa", "物料组.ic"),
            // dict.logistics.material.group.qaa
            ("dict.logistics.material.group.qaa", "zh-CN", "ic", "物料组.ic"),
            // dict.logistics.material.group.qaa
            ("dict.logistics.material.group.qaa", "zh-HK", "ic", "物料组.ic"),

            // dict.logistics.material.group.qta
            ("dict.logistics.material.group.qta", "en-US", "qta", "物料组.transistor"),
            // dict.logistics.material.group.qta
            ("dict.logistics.material.group.qta", "ja-JP", "qta", "物料组.transistor"),
            // dict.logistics.material.group.qta
            ("dict.logistics.material.group.qta", "zh-CN", "transistor", "物料组.transistor"),
            // dict.logistics.material.group.qta
            ("dict.logistics.material.group.qta", "zh-HK", "transistor", "物料组.transistor"),

            // dict.logistics.material.group.qda
            ("dict.logistics.material.group.qda", "en-US", "qda", "物料组.diode"),
            // dict.logistics.material.group.qda
            ("dict.logistics.material.group.qda", "ja-JP", "qda", "物料组.diode"),
            // dict.logistics.material.group.qda
            ("dict.logistics.material.group.qda", "zh-CN", "diode", "物料组.diode"),
            // dict.logistics.material.group.qda
            ("dict.logistics.material.group.qda", "zh-HK", "diode", "物料组.diode"),

            // dict.logistics.material.group.raa
            ("dict.logistics.material.group.raa", "en-US", "raa", "物料组.resistor"),
            // dict.logistics.material.group.raa
            ("dict.logistics.material.group.raa", "ja-JP", "raa", "物料组.resistor"),
            // dict.logistics.material.group.raa
            ("dict.logistics.material.group.raa", "zh-CN", "resistor", "物料组.resistor"),
            // dict.logistics.material.group.raa
            ("dict.logistics.material.group.raa", "zh-HK", "resistor", "物料组.resistor"),

            // dict.logistics.material.group.saa
            ("dict.logistics.material.group.saa", "en-US", "saa", "物料组.semi"),
            // dict.logistics.material.group.saa
            ("dict.logistics.material.group.saa", "ja-JP", "saa", "物料组.semi"),
            // dict.logistics.material.group.saa
            ("dict.logistics.material.group.saa", "zh-CN", "semi", "物料组.semi"),
            // dict.logistics.material.group.saa
            ("dict.logistics.material.group.saa", "zh-HK", "semi", "物料组.semi"),

            // dict.logistics.material.group.taa
            ("dict.logistics.material.group.taa", "en-US", "taa", "物料组.tape"),
            // dict.logistics.material.group.taa
            ("dict.logistics.material.group.taa", "ja-JP", "taa", "物料组.tape"),
            // dict.logistics.material.group.taa
            ("dict.logistics.material.group.taa", "zh-CN", "tape", "物料组.tape"),
            // dict.logistics.material.group.taa
            ("dict.logistics.material.group.taa", "zh-HK", "tape", "物料组.tape"),

            // dict.logistics.material.group.tma
            ("dict.logistics.material.group.tma", "en-US", "tma", "物料组.media"),
            // dict.logistics.material.group.tma
            ("dict.logistics.material.group.tma", "ja-JP", "tma", "物料组.media"),
            // dict.logistics.material.group.tma
            ("dict.logistics.material.group.tma", "zh-CN", "media", "物料组.media"),
            // dict.logistics.material.group.tma
            ("dict.logistics.material.group.tma", "zh-HK", "media", "物料组.media"),

            // dict.logistics.material.type.abf
            ("dict.logistics.material.type.abf", "en-US", "abf", "物料类型.废料"),
            // dict.logistics.material.type.abf
            ("dict.logistics.material.type.abf", "ja-JP", "abf", "物料类型.废料"),
            // dict.logistics.material.type.abf
            ("dict.logistics.material.type.abf", "zh-CN", "废料", "物料类型.废料"),
            // dict.logistics.material.type.abf
            ("dict.logistics.material.type.abf", "zh-HK", "废料", "物料类型.废料"),

            // dict.logistics.material.type.cbau
            ("dict.logistics.material.type.cbau", "en-US", "cbau", "物料类型.兼容设备"),
            // dict.logistics.material.type.cbau
            ("dict.logistics.material.type.cbau", "ja-JP", "cbau", "物料类型.兼容设备"),
            // dict.logistics.material.type.cbau
            ("dict.logistics.material.type.cbau", "zh-CN", "兼容设备", "物料类型.兼容设备"),
            // dict.logistics.material.type.cbau
            ("dict.logistics.material.type.cbau", "zh-HK", "兼容设备", "物料类型.兼容设备"),

            // dict.logistics.material.type.ch00
            ("dict.logistics.material.type.ch00", "en-US", "ch00", "物料类型.ch合同操作"),
            // dict.logistics.material.type.ch00
            ("dict.logistics.material.type.ch00", "ja-JP", "ch00", "物料类型.ch合同操作"),
            // dict.logistics.material.type.ch00
            ("dict.logistics.material.type.ch00", "zh-CN", "ch合同操作", "物料类型.ch合同操作"),
            // dict.logistics.material.type.ch00
            ("dict.logistics.material.type.ch00", "zh-HK", "ch合同操作", "物料类型.ch合同操作"),

            // dict.logistics.material.type.cont
            ("dict.logistics.material.type.cont", "en-US", "cont", "物料类型.看板容器"),
            // dict.logistics.material.type.cont
            ("dict.logistics.material.type.cont", "ja-JP", "cont", "物料类型.看板容器"),
            // dict.logistics.material.type.cont
            ("dict.logistics.material.type.cont", "zh-CN", "看板容器", "物料类型.看板容器"),
            // dict.logistics.material.type.cont
            ("dict.logistics.material.type.cont", "zh-HK", "看板容器", "物料类型.看板容器"),

            // dict.logistics.material.type.coup
            ("dict.logistics.material.type.coup", "en-US", "coup", "物料类型.优惠券"),
            // dict.logistics.material.type.coup
            ("dict.logistics.material.type.coup", "ja-JP", "coup", "物料类型.优惠券"),
            // dict.logistics.material.type.coup
            ("dict.logistics.material.type.coup", "zh-CN", "优惠券", "物料类型.优惠券"),
            // dict.logistics.material.type.coup
            ("dict.logistics.material.type.coup", "zh-HK", "优惠券", "物料类型.优惠券"),

            // dict.logistics.material.type.dien
            ("dict.logistics.material.type.dien", "en-US", "dien", "物料类型.服务"),
            // dict.logistics.material.type.dien
            ("dict.logistics.material.type.dien", "ja-JP", "dien", "物料类型.服务"),
            // dict.logistics.material.type.dien
            ("dict.logistics.material.type.dien", "zh-CN", "服务", "物料类型.服务"),
            // dict.logistics.material.type.dien
            ("dict.logistics.material.type.dien", "zh-HK", "服务", "物料类型.服务"),

            // dict.logistics.material.type.epa
            ("dict.logistics.material.type.epa", "en-US", "epa", "物料类型.设备包装"),
            // dict.logistics.material.type.epa
            ("dict.logistics.material.type.epa", "ja-JP", "epa", "物料类型.设备包装"),
            // dict.logistics.material.type.epa
            ("dict.logistics.material.type.epa", "zh-CN", "设备包装", "物料类型.设备包装"),
            // dict.logistics.material.type.epa
            ("dict.logistics.material.type.epa", "zh-HK", "设备包装", "物料类型.设备包装"),

            // dict.logistics.material.type.ersa
            ("dict.logistics.material.type.ersa", "en-US", "ersa", "物料类型.备件"),
            // dict.logistics.material.type.ersa
            ("dict.logistics.material.type.ersa", "ja-JP", "ersa", "物料类型.备件"),
            // dict.logistics.material.type.ersa
            ("dict.logistics.material.type.ersa", "zh-CN", "备件", "物料类型.备件"),
            // dict.logistics.material.type.ersa
            ("dict.logistics.material.type.ersa", "zh-HK", "备件", "物料类型.备件"),

            // dict.logistics.material.type.fert
            ("dict.logistics.material.type.fert", "en-US", "fert", "物料类型.成品"),
            // dict.logistics.material.type.fert
            ("dict.logistics.material.type.fert", "ja-JP", "fert", "物料类型.成品"),
            // dict.logistics.material.type.fert
            ("dict.logistics.material.type.fert", "zh-CN", "成品", "物料类型.成品"),
            // dict.logistics.material.type.fert
            ("dict.logistics.material.type.fert", "zh-HK", "成品", "物料类型.成品"),

            // dict.logistics.material.type.fgtr
            ("dict.logistics.material.type.fgtr", "en-US", "fgtr", "物料类型.饮料"),
            // dict.logistics.material.type.fgtr
            ("dict.logistics.material.type.fgtr", "ja-JP", "fgtr", "物料类型.饮料"),
            // dict.logistics.material.type.fgtr
            ("dict.logistics.material.type.fgtr", "zh-CN", "饮料", "物料类型.饮料"),
            // dict.logistics.material.type.fgtr
            ("dict.logistics.material.type.fgtr", "zh-HK", "饮料", "物料类型.饮料"),

            // dict.logistics.material.type.fhmi
            ("dict.logistics.material.type.fhmi", "en-US", "fhmi", "物料类型.生产资源/工具"),
            // dict.logistics.material.type.fhmi
            ("dict.logistics.material.type.fhmi", "ja-JP", "fhmi", "物料类型.生产资源/工具"),
            // dict.logistics.material.type.fhmi
            ("dict.logistics.material.type.fhmi", "zh-CN", "生产资源/工具", "物料类型.生产资源/工具"),
            // dict.logistics.material.type.fhmi
            ("dict.logistics.material.type.fhmi", "zh-HK", "生产资源/工具", "物料类型.生产资源/工具"),

            // dict.logistics.material.type.food
            ("dict.logistics.material.type.food", "en-US", "food", "物料类型.食品"),
            // dict.logistics.material.type.food
            ("dict.logistics.material.type.food", "ja-JP", "food", "物料类型.食品"),
            // dict.logistics.material.type.food
            ("dict.logistics.material.type.food", "zh-CN", "食品", "物料类型.食品"),
            // dict.logistics.material.type.food
            ("dict.logistics.material.type.food", "zh-HK", "食品", "物料类型.食品"),

            // dict.logistics.material.type.frip
            ("dict.logistics.material.type.frip", "en-US", "frip", "物料类型.易腐品"),
            // dict.logistics.material.type.frip
            ("dict.logistics.material.type.frip", "ja-JP", "frip", "物料类型.易腐品"),
            // dict.logistics.material.type.frip
            ("dict.logistics.material.type.frip", "zh-CN", "易腐品", "物料类型.易腐品"),
            // dict.logistics.material.type.frip
            ("dict.logistics.material.type.frip", "zh-HK", "易腐品", "物料类型.易腐品"),

            // dict.logistics.material.type.halb
            ("dict.logistics.material.type.halb", "en-US", "halb", "物料类型.半成品"),
            // dict.logistics.material.type.halb
            ("dict.logistics.material.type.halb", "ja-JP", "halb", "物料类型.半成品"),
            // dict.logistics.material.type.halb
            ("dict.logistics.material.type.halb", "zh-CN", "半成品", "物料类型.半成品"),
            // dict.logistics.material.type.halb
            ("dict.logistics.material.type.halb", "zh-HK", "半成品", "物料类型.半成品"),

            // dict.logistics.material.type.hawa
            ("dict.logistics.material.type.hawa", "en-US", "hawa", "物料类型.贸易货物"),
            // dict.logistics.material.type.hawa
            ("dict.logistics.material.type.hawa", "ja-JP", "hawa", "物料类型.贸易货物"),
            // dict.logistics.material.type.hawa
            ("dict.logistics.material.type.hawa", "zh-CN", "贸易货物", "物料类型.贸易货物"),
            // dict.logistics.material.type.hawa
            ("dict.logistics.material.type.hawa", "zh-HK", "贸易货物", "物料类型.贸易货物"),

            // dict.logistics.material.type.hers
            ("dict.logistics.material.type.hers", "en-US", "hers", "物料类型.制造商部分"),
            // dict.logistics.material.type.hers
            ("dict.logistics.material.type.hers", "ja-JP", "hers", "物料类型.制造商部分"),
            // dict.logistics.material.type.hers
            ("dict.logistics.material.type.hers", "zh-CN", "制造商部分", "物料类型.制造商部分"),
            // dict.logistics.material.type.hers
            ("dict.logistics.material.type.hers", "zh-HK", "制造商部分", "物料类型.制造商部分"),

            // dict.logistics.material.type.hibe
            ("dict.logistics.material.type.hibe", "en-US", "hibe", "物料类型.经营供应"),
            // dict.logistics.material.type.hibe
            ("dict.logistics.material.type.hibe", "ja-JP", "hibe", "物料类型.经营供应"),
            // dict.logistics.material.type.hibe
            ("dict.logistics.material.type.hibe", "zh-CN", "经营供应", "物料类型.经营供应"),
            // dict.logistics.material.type.hibe
            ("dict.logistics.material.type.hibe", "zh-HK", "经营供应", "物料类型.经营供应"),

            // dict.logistics.material.type.ibau
            ("dict.logistics.material.type.ibau", "en-US", "ibau", "物料类型.维护装配"),
            // dict.logistics.material.type.ibau
            ("dict.logistics.material.type.ibau", "ja-JP", "ibau", "物料类型.维护装配"),
            // dict.logistics.material.type.ibau
            ("dict.logistics.material.type.ibau", "zh-CN", "维护装配", "物料类型.维护装配"),
            // dict.logistics.material.type.ibau
            ("dict.logistics.material.type.ibau", "zh-HK", "维护装配", "物料类型.维护装配"),

            // dict.logistics.material.type.intr
            ("dict.logistics.material.type.intr", "en-US", "intr", "物料类型.内部物料"),
            // dict.logistics.material.type.intr
            ("dict.logistics.material.type.intr", "ja-JP", "intr", "物料类型.内部物料"),
            // dict.logistics.material.type.intr
            ("dict.logistics.material.type.intr", "zh-CN", "内部物料", "物料类型.内部物料"),
            // dict.logistics.material.type.intr
            ("dict.logistics.material.type.intr", "zh-HK", "内部物料", "物料类型.内部物料"),

            // dict.logistics.material.type.kmat
            ("dict.logistics.material.type.kmat", "en-US", "kmat", "物料类型.可配置物料"),
            // dict.logistics.material.type.kmat
            ("dict.logistics.material.type.kmat", "ja-JP", "kmat", "物料类型.可配置物料"),
            // dict.logistics.material.type.kmat
            ("dict.logistics.material.type.kmat", "zh-CN", "可配置物料", "物料类型.可配置物料"),
            // dict.logistics.material.type.kmat
            ("dict.logistics.material.type.kmat", "zh-HK", "可配置物料", "物料类型.可配置物料"),

            // dict.logistics.material.type.leer
            ("dict.logistics.material.type.leer", "en-US", "leer", "物料类型.虚拟件"),
            // dict.logistics.material.type.leer
            ("dict.logistics.material.type.leer", "ja-JP", "leer", "物料类型.虚拟件"),
            // dict.logistics.material.type.leer
            ("dict.logistics.material.type.leer", "zh-CN", "虚拟件", "物料类型.虚拟件"),
            // dict.logistics.material.type.leer
            ("dict.logistics.material.type.leer", "zh-HK", "虚拟件", "物料类型.虚拟件"),

            // dict.logistics.material.type.leih
            ("dict.logistics.material.type.leih", "en-US", "leih", "物料类型.可反复利用包装"),
            // dict.logistics.material.type.leih
            ("dict.logistics.material.type.leih", "ja-JP", "leih", "物料类型.可反复利用包装"),
            // dict.logistics.material.type.leih
            ("dict.logistics.material.type.leih", "zh-CN", "可反复利用包装", "物料类型.可反复利用包装"),
            // dict.logistics.material.type.leih
            ("dict.logistics.material.type.leih", "zh-HK", "可反复利用包装", "物料类型.可反复利用包装"),

            // dict.logistics.material.type.lgut
            ("dict.logistics.material.type.lgut", "en-US", "lgut", "物料类型.空零售"),
            // dict.logistics.material.type.lgut
            ("dict.logistics.material.type.lgut", "ja-JP", "lgut", "物料类型.空零售"),
            // dict.logistics.material.type.lgut
            ("dict.logistics.material.type.lgut", "zh-CN", "空零售", "物料类型.空零售"),
            // dict.logistics.material.type.lgut
            ("dict.logistics.material.type.lgut", "zh-HK", "空零售", "物料类型.空零售"),

            // dict.logistics.material.type.mode
            ("dict.logistics.material.type.mode", "en-US", "mode", "物料类型.衣物"),
            // dict.logistics.material.type.mode
            ("dict.logistics.material.type.mode", "ja-JP", "mode", "物料类型.衣物"),
            // dict.logistics.material.type.mode
            ("dict.logistics.material.type.mode", "zh-CN", "衣物", "物料类型.衣物"),
            // dict.logistics.material.type.mode
            ("dict.logistics.material.type.mode", "zh-HK", "衣物", "物料类型.衣物"),

            // dict.logistics.material.type.mpo
            ("dict.logistics.material.type.mpo", "en-US", "mpo", "物料类型.物料计划对象"),
            // dict.logistics.material.type.mpo
            ("dict.logistics.material.type.mpo", "ja-JP", "mpo", "物料类型.物料计划对象"),
            // dict.logistics.material.type.mpo
            ("dict.logistics.material.type.mpo", "zh-CN", "物料计划对象", "物料类型.物料计划对象"),
            // dict.logistics.material.type.mpo
            ("dict.logistics.material.type.mpo", "zh-HK", "物料计划对象", "物料类型.物料计划对象"),

            // dict.logistics.material.type.nlag
            ("dict.logistics.material.type.nlag", "en-US", "nlag", "物料类型.非存储物料"),
            // dict.logistics.material.type.nlag
            ("dict.logistics.material.type.nlag", "ja-JP", "nlag", "物料类型.非存储物料"),
            // dict.logistics.material.type.nlag
            ("dict.logistics.material.type.nlag", "zh-CN", "非存储物料", "物料类型.非存储物料"),
            // dict.logistics.material.type.nlag
            ("dict.logistics.material.type.nlag", "zh-HK", "非存储物料", "物料类型.非存储物料"),

            // dict.logistics.material.type.nof1
            ("dict.logistics.material.type.nof1", "en-US", "nof1", "物料类型.非食品"),
            // dict.logistics.material.type.nof1
            ("dict.logistics.material.type.nof1", "ja-JP", "nof1", "物料类型.非食品"),
            // dict.logistics.material.type.nof1
            ("dict.logistics.material.type.nof1", "zh-CN", "非食品", "物料类型.非食品"),
            // dict.logistics.material.type.nof1
            ("dict.logistics.material.type.nof1", "zh-HK", "非食品", "物料类型.非食品"),

            // dict.logistics.material.type.pipe
            ("dict.logistics.material.type.pipe", "en-US", "pipe", "物料类型.管线物料"),
            // dict.logistics.material.type.pipe
            ("dict.logistics.material.type.pipe", "ja-JP", "pipe", "物料类型.管线物料"),
            // dict.logistics.material.type.pipe
            ("dict.logistics.material.type.pipe", "zh-CN", "管线物料", "物料类型.管线物料"),
            // dict.logistics.material.type.pipe
            ("dict.logistics.material.type.pipe", "zh-HK", "管线物料", "物料类型.管线物料"),

            // dict.logistics.material.type.plan
            ("dict.logistics.material.type.plan", "en-US", "plan", "物料类型.贸易货物计划"),
            // dict.logistics.material.type.plan
            ("dict.logistics.material.type.plan", "ja-JP", "plan", "物料类型.贸易货物计划"),
            // dict.logistics.material.type.plan
            ("dict.logistics.material.type.plan", "zh-CN", "贸易货物计划", "物料类型.贸易货物计划"),
            // dict.logistics.material.type.plan
            ("dict.logistics.material.type.plan", "zh-HK", "贸易货物计划", "物料类型.贸易货物计划"),

            // dict.logistics.material.type.proc
            ("dict.logistics.material.type.proc", "en-US", "proc", "物料类型.过程物料"),
            // dict.logistics.material.type.proc
            ("dict.logistics.material.type.proc", "ja-JP", "proc", "物料类型.过程物料"),
            // dict.logistics.material.type.proc
            ("dict.logistics.material.type.proc", "zh-CN", "过程物料", "物料类型.过程物料"),
            // dict.logistics.material.type.proc
            ("dict.logistics.material.type.proc", "zh-HK", "过程物料", "物料类型.过程物料"),

            // dict.logistics.material.type.prod
            ("dict.logistics.material.type.prod", "en-US", "prod", "物料类型.产品组"),
            // dict.logistics.material.type.prod
            ("dict.logistics.material.type.prod", "ja-JP", "prod", "物料类型.产品组"),
            // dict.logistics.material.type.prod
            ("dict.logistics.material.type.prod", "zh-CN", "产品组", "物料类型.产品组"),
            // dict.logistics.material.type.prod
            ("dict.logistics.material.type.prod", "zh-HK", "产品组", "物料类型.产品组"),

            // dict.logistics.material.type.roh
            ("dict.logistics.material.type.roh", "en-US", "roh", "物料类型.原材料"),
            // dict.logistics.material.type.roh
            ("dict.logistics.material.type.roh", "ja-JP", "roh", "物料类型.原材料"),
            // dict.logistics.material.type.roh
            ("dict.logistics.material.type.roh", "zh-CN", "原材料", "物料类型.原材料"),
            // dict.logistics.material.type.roh
            ("dict.logistics.material.type.roh", "zh-HK", "原材料", "物料类型.原材料"),

            // dict.logistics.material.type.unbw
            ("dict.logistics.material.type.unbw", "en-US", "unbw", "物料类型.未估价物料"),
            // dict.logistics.material.type.unbw
            ("dict.logistics.material.type.unbw", "ja-JP", "unbw", "物料类型.未估价物料"),
            // dict.logistics.material.type.unbw
            ("dict.logistics.material.type.unbw", "zh-CN", "未估价物料", "物料类型.未估价物料"),
            // dict.logistics.material.type.unbw
            ("dict.logistics.material.type.unbw", "zh-HK", "未估价物料", "物料类型.未估价物料"),

            // dict.logistics.material.type.verp
            ("dict.logistics.material.type.verp", "en-US", "verp", "物料类型.包装"),
            // dict.logistics.material.type.verp
            ("dict.logistics.material.type.verp", "ja-JP", "verp", "物料类型.包装"),
            // dict.logistics.material.type.verp
            ("dict.logistics.material.type.verp", "zh-CN", "包装", "物料类型.包装"),
            // dict.logistics.material.type.verp
            ("dict.logistics.material.type.verp", "zh-HK", "包装", "物料类型.包装"),

            // dict.logistics.material.type.vkhm
            ("dict.logistics.material.type.vkhm", "en-US", "vkhm", "物料类型.附加"),
            // dict.logistics.material.type.vkhm
            ("dict.logistics.material.type.vkhm", "ja-JP", "vkhm", "物料类型.附加"),
            // dict.logistics.material.type.vkhm
            ("dict.logistics.material.type.vkhm", "zh-CN", "附加", "物料类型.附加"),
            // dict.logistics.material.type.vkhm
            ("dict.logistics.material.type.vkhm", "zh-HK", "附加", "物料类型.附加"),

            // dict.logistics.material.type.voll
            ("dict.logistics.material.type.voll", "en-US", "voll", "物料类型.全部产品"),
            // dict.logistics.material.type.voll
            ("dict.logistics.material.type.voll", "ja-JP", "voll", "物料类型.全部产品"),
            // dict.logistics.material.type.voll
            ("dict.logistics.material.type.voll", "zh-CN", "全部产品", "物料类型.全部产品"),
            // dict.logistics.material.type.voll
            ("dict.logistics.material.type.voll", "zh-HK", "全部产品", "物料类型.全部产品"),

            // dict.logistics.material.type.werb
            ("dict.logistics.material.type.werb", "en-US", "werb", "物料类型.产品目录"),
            // dict.logistics.material.type.werb
            ("dict.logistics.material.type.werb", "ja-JP", "werb", "物料类型.产品目录"),
            // dict.logistics.material.type.werb
            ("dict.logistics.material.type.werb", "zh-CN", "产品目录", "物料类型.产品目录"),
            // dict.logistics.material.type.werb
            ("dict.logistics.material.type.werb", "zh-HK", "产品目录", "物料类型.产品目录"),

            // dict.logistics.material.type.wert
            ("dict.logistics.material.type.wert", "en-US", "wert", "物料类型.只有价值物料"),
            // dict.logistics.material.type.wert
            ("dict.logistics.material.type.wert", "ja-JP", "wert", "物料类型.只有价值物料"),
            // dict.logistics.material.type.wert
            ("dict.logistics.material.type.wert", "zh-CN", "只有价值物料", "物料类型.只有价值物料"),
            // dict.logistics.material.type.wert
            ("dict.logistics.material.type.wert", "zh-HK", "只有价值物料", "物料类型.只有价值物料"),

            // dict.logistics.material.type.wett
            ("dict.logistics.material.type.wett", "en-US", "wett", "物料类型.竞争产品"),
            // dict.logistics.material.type.wett
            ("dict.logistics.material.type.wett", "ja-JP", "wett", "物料类型.竞争产品"),
            // dict.logistics.material.type.wett
            ("dict.logistics.material.type.wett", "zh-CN", "竞争产品", "物料类型.竞争产品"),
            // dict.logistics.material.type.wett
            ("dict.logistics.material.type.wett", "zh-HK", "竞争产品", "物料类型.竞争产品"),

            // dict.logistics.planned.delivery.days.7
            ("dict.logistics.planned.delivery.days.7", "en-US", "7天", "计划交货天数.7天"),
            // dict.logistics.planned.delivery.days.7
            ("dict.logistics.planned.delivery.days.7", "ja-JP", "7天", "计划交货天数.7天"),
            // dict.logistics.planned.delivery.days.7
            ("dict.logistics.planned.delivery.days.7", "zh-CN", "7天", "计划交货天数.7天"),
            // dict.logistics.planned.delivery.days.7
            ("dict.logistics.planned.delivery.days.7", "zh-HK", "7天", "计划交货天数.7天"),

            // dict.logistics.planned.delivery.days.30
            ("dict.logistics.planned.delivery.days.30", "en-US", "30天", "计划交货天数.30天"),
            // dict.logistics.planned.delivery.days.30
            ("dict.logistics.planned.delivery.days.30", "ja-JP", "30天", "计划交货天数.30天"),
            // dict.logistics.planned.delivery.days.30
            ("dict.logistics.planned.delivery.days.30", "zh-CN", "30天", "计划交货天数.30天"),
            // dict.logistics.planned.delivery.days.30
            ("dict.logistics.planned.delivery.days.30", "zh-HK", "30天", "计划交货天数.30天"),

            // dict.logistics.planned.delivery.days.60
            ("dict.logistics.planned.delivery.days.60", "en-US", "60天", "计划交货天数.60天"),
            // dict.logistics.planned.delivery.days.60
            ("dict.logistics.planned.delivery.days.60", "ja-JP", "60天", "计划交货天数.60天"),
            // dict.logistics.planned.delivery.days.60
            ("dict.logistics.planned.delivery.days.60", "zh-CN", "60天", "计划交货天数.60天"),
            // dict.logistics.planned.delivery.days.60
            ("dict.logistics.planned.delivery.days.60", "zh-HK", "60天", "计划交货天数.60天"),

            // dict.logistics.planned.delivery.days.90
            ("dict.logistics.planned.delivery.days.90", "en-US", "90天", "计划交货天数.90天"),
            // dict.logistics.planned.delivery.days.90
            ("dict.logistics.planned.delivery.days.90", "ja-JP", "90天", "计划交货天数.90天"),
            // dict.logistics.planned.delivery.days.90
            ("dict.logistics.planned.delivery.days.90", "zh-CN", "90天", "计划交货天数.90天"),
            // dict.logistics.planned.delivery.days.90
            ("dict.logistics.planned.delivery.days.90", "zh-HK", "90天", "计划交货天数.90天"),

            // dict.logistics.planned.delivery.days.120
            ("dict.logistics.planned.delivery.days.120", "en-US", "120天", "计划交货天数.120天"),
            // dict.logistics.planned.delivery.days.120
            ("dict.logistics.planned.delivery.days.120", "ja-JP", "120天", "计划交货天数.120天"),
            // dict.logistics.planned.delivery.days.120
            ("dict.logistics.planned.delivery.days.120", "zh-CN", "120天", "计划交货天数.120天"),
            // dict.logistics.planned.delivery.days.120
            ("dict.logistics.planned.delivery.days.120", "zh-HK", "120天", "计划交货天数.120天"),

            // dict.logistics.price.control.s
            ("dict.logistics.price.control.s", "en-US", "s", "价格控制.标准价格"),
            // dict.logistics.price.control.s
            ("dict.logistics.price.control.s", "ja-JP", "s", "价格控制.标准价格"),
            // dict.logistics.price.control.s
            ("dict.logistics.price.control.s", "zh-CN", "标准价格", "价格控制.标准价格"),
            // dict.logistics.price.control.s
            ("dict.logistics.price.control.s", "zh-HK", "标准价格", "价格控制.标准价格"),

            // dict.logistics.price.control.v
            ("dict.logistics.price.control.v", "en-US", "v", "价格控制.移动平均价"),
            // dict.logistics.price.control.v
            ("dict.logistics.price.control.v", "ja-JP", "v", "价格控制.移动平均价"),
            // dict.logistics.price.control.v
            ("dict.logistics.price.control.v", "zh-CN", "移动平均价", "价格控制.移动平均价"),
            // dict.logistics.price.control.v
            ("dict.logistics.price.control.v", "zh-HK", "移动平均价", "价格控制.移动平均价"),

            // dict.logistics.price.type.purchase
            ("dict.logistics.price.type.purchase", "en-US", "purchase", "价格类型.采购价"),
            // dict.logistics.price.type.purchase
            ("dict.logistics.price.type.purchase", "ja-JP", "purchase", "价格类型.采购价"),
            // dict.logistics.price.type.purchase
            ("dict.logistics.price.type.purchase", "zh-CN", "采购价", "价格类型.采购价"),
            // dict.logistics.price.type.purchase
            ("dict.logistics.price.type.purchase", "zh-HK", "采购价", "价格类型.采购价"),

            // dict.logistics.price.type.sales
            ("dict.logistics.price.type.sales", "en-US", "sales", "价格类型.销售价"),
            // dict.logistics.price.type.sales
            ("dict.logistics.price.type.sales", "ja-JP", "sales", "价格类型.销售价"),
            // dict.logistics.price.type.sales
            ("dict.logistics.price.type.sales", "zh-CN", "销售价", "价格类型.销售价"),
            // dict.logistics.price.type.sales
            ("dict.logistics.price.type.sales", "zh-HK", "销售价", "价格类型.销售价"),

            // dict.logistics.price.type.cost
            ("dict.logistics.price.type.cost", "en-US", "cost", "价格类型.成本价"),
            // dict.logistics.price.type.cost
            ("dict.logistics.price.type.cost", "ja-JP", "cost", "价格类型.成本价"),
            // dict.logistics.price.type.cost
            ("dict.logistics.price.type.cost", "zh-CN", "成本价", "价格类型.成本价"),
            // dict.logistics.price.type.cost
            ("dict.logistics.price.type.cost", "zh-HK", "成本价", "价格类型.成本价"),

            // dict.logistics.price.type.wholesale
            ("dict.logistics.price.type.wholesale", "en-US", "wholesale", "价格类型.批发价"),
            // dict.logistics.price.type.wholesale
            ("dict.logistics.price.type.wholesale", "ja-JP", "wholesale", "价格类型.批发价"),
            // dict.logistics.price.type.wholesale
            ("dict.logistics.price.type.wholesale", "zh-CN", "批发价", "价格类型.批发价"),
            // dict.logistics.price.type.wholesale
            ("dict.logistics.price.type.wholesale", "zh-HK", "批发价", "价格类型.批发价"),

            // dict.logistics.price.type.retail
            ("dict.logistics.price.type.retail", "en-US", "retail", "价格类型.零售价"),
            // dict.logistics.price.type.retail
            ("dict.logistics.price.type.retail", "ja-JP", "retail", "价格类型.零售价"),
            // dict.logistics.price.type.retail
            ("dict.logistics.price.type.retail", "zh-CN", "零售价", "价格类型.零售价"),
            // dict.logistics.price.type.retail
            ("dict.logistics.price.type.retail", "zh-HK", "零售价", "价格类型.零售价"),

            // dict.logistics.price.type.agreement
            ("dict.logistics.price.type.agreement", "en-US", "agreement", "价格类型.协议价"),
            // dict.logistics.price.type.agreement
            ("dict.logistics.price.type.agreement", "ja-JP", "agreement", "价格类型.协议价"),
            // dict.logistics.price.type.agreement
            ("dict.logistics.price.type.agreement", "zh-CN", "协议价", "价格类型.协议价"),
            // dict.logistics.price.type.agreement
            ("dict.logistics.price.type.agreement", "zh-HK", "协议价", "价格类型.协议价"),

            // dict.logistics.price.unit.1
            ("dict.logistics.price.unit.1", "en-US", "1", "价格单位.1"),
            // dict.logistics.price.unit.1
            ("dict.logistics.price.unit.1", "ja-JP", "1", "价格单位.1"),
            // dict.logistics.price.unit.1
            ("dict.logistics.price.unit.1", "zh-CN", "1", "价格单位.1"),
            // dict.logistics.price.unit.1
            ("dict.logistics.price.unit.1", "zh-HK", "1", "价格单位.1"),

            // dict.logistics.price.unit.10
            ("dict.logistics.price.unit.10", "en-US", "10", "价格单位.10"),
            // dict.logistics.price.unit.10
            ("dict.logistics.price.unit.10", "ja-JP", "10", "价格单位.10"),
            // dict.logistics.price.unit.10
            ("dict.logistics.price.unit.10", "zh-CN", "10", "价格单位.10"),
            // dict.logistics.price.unit.10
            ("dict.logistics.price.unit.10", "zh-HK", "10", "价格单位.10"),

            // dict.logistics.price.unit.100
            ("dict.logistics.price.unit.100", "en-US", "100", "价格单位.100"),
            // dict.logistics.price.unit.100
            ("dict.logistics.price.unit.100", "ja-JP", "100", "价格单位.100"),
            // dict.logistics.price.unit.100
            ("dict.logistics.price.unit.100", "zh-CN", "100", "价格单位.100"),
            // dict.logistics.price.unit.100
            ("dict.logistics.price.unit.100", "zh-HK", "100", "价格单位.100"),

            // dict.logistics.price.unit.1000
            ("dict.logistics.price.unit.1000", "en-US", "1000", "价格单位.1000"),
            // dict.logistics.price.unit.1000
            ("dict.logistics.price.unit.1000", "ja-JP", "1000", "价格单位.1000"),
            // dict.logistics.price.unit.1000
            ("dict.logistics.price.unit.1000", "zh-CN", "1000", "价格单位.1000"),
            // dict.logistics.price.unit.1000
            ("dict.logistics.price.unit.1000", "zh-HK", "1000", "价格单位.1000"),

            // dict.logistics.procurement.type.e
            ("dict.logistics.procurement.type.e", "en-US", "e", "采购类别.自制生产"),
            // dict.logistics.procurement.type.e
            ("dict.logistics.procurement.type.e", "ja-JP", "e", "采购类别.自制生产"),
            // dict.logistics.procurement.type.e
            ("dict.logistics.procurement.type.e", "zh-CN", "自制生产", "采购类别.自制生产"),
            // dict.logistics.procurement.type.e
            ("dict.logistics.procurement.type.e", "zh-HK", "自制生产", "采购类别.自制生产"),

            // dict.logistics.procurement.type.f
            ("dict.logistics.procurement.type.f", "en-US", "f", "采购类别.外部采购"),
            // dict.logistics.procurement.type.f
            ("dict.logistics.procurement.type.f", "ja-JP", "f", "采购类别.外部采购"),
            // dict.logistics.procurement.type.f
            ("dict.logistics.procurement.type.f", "zh-CN", "外部采购", "采购类别.外部采购"),
            // dict.logistics.procurement.type.f
            ("dict.logistics.procurement.type.f", "zh-HK", "外部采购", "采购类别.外部采购"),

            // dict.logistics.procurement.type.x
            ("dict.logistics.procurement.type.x", "en-US", "x", "采购类别.两种采购类型"),
            // dict.logistics.procurement.type.x
            ("dict.logistics.procurement.type.x", "ja-JP", "x", "采购类别.两种采购类型"),
            // dict.logistics.procurement.type.x
            ("dict.logistics.procurement.type.x", "zh-CN", "两种采购类型", "采购类别.两种采购类型"),
            // dict.logistics.procurement.type.x
            ("dict.logistics.procurement.type.x", "zh-HK", "两种采购类型", "采购类别.两种采购类型"),

            // dict.logistics.sampling.scheme.type.gb2828
            ("dict.logistics.sampling.scheme.type.gb2828", "en-US", "gb2828", "抽样方案类型.gb/t 2828.1"),
            // dict.logistics.sampling.scheme.type.gb2828
            ("dict.logistics.sampling.scheme.type.gb2828", "ja-JP", "gb2828", "抽样方案类型.gb/t 2828.1"),
            // dict.logistics.sampling.scheme.type.gb2828
            ("dict.logistics.sampling.scheme.type.gb2828", "zh-CN", "gb/t 2828.1", "抽样方案类型.gb/t 2828.1"),
            // dict.logistics.sampling.scheme.type.gb2828
            ("dict.logistics.sampling.scheme.type.gb2828", "zh-HK", "gb/t 2828.1", "抽样方案类型.gb/t 2828.1"),

            // dict.logistics.sampling.scheme.type.mil105e
            ("dict.logistics.sampling.scheme.type.mil105e", "en-US", "mil105e", "抽样方案类型.mil-std-105e"),
            // dict.logistics.sampling.scheme.type.mil105e
            ("dict.logistics.sampling.scheme.type.mil105e", "ja-JP", "mil105e", "抽样方案类型.mil-std-105e"),
            // dict.logistics.sampling.scheme.type.mil105e
            ("dict.logistics.sampling.scheme.type.mil105e", "zh-CN", "mil-std-105e", "抽样方案类型.mil-std-105e"),
            // dict.logistics.sampling.scheme.type.mil105e
            ("dict.logistics.sampling.scheme.type.mil105e", "zh-HK", "mil-std-105e", "抽样方案类型.mil-std-105e"),

            // dict.logistics.sampling.scheme.type.iso2859
            ("dict.logistics.sampling.scheme.type.iso2859", "en-US", "iso2859", "抽样方案类型.iso 2859-1"),
            // dict.logistics.sampling.scheme.type.iso2859
            ("dict.logistics.sampling.scheme.type.iso2859", "ja-JP", "iso2859", "抽样方案类型.iso 2859-1"),
            // dict.logistics.sampling.scheme.type.iso2859
            ("dict.logistics.sampling.scheme.type.iso2859", "zh-CN", "iso 2859-1", "抽样方案类型.iso 2859-1"),
            // dict.logistics.sampling.scheme.type.iso2859
            ("dict.logistics.sampling.scheme.type.iso2859", "zh-HK", "iso 2859-1", "抽样方案类型.iso 2859-1"),

            // dict.logistics.sampling.scheme.type.gb2829
            ("dict.logistics.sampling.scheme.type.gb2829", "en-US", "gb2829", "抽样方案类型.gb/t 2829"),
            // dict.logistics.sampling.scheme.type.gb2829
            ("dict.logistics.sampling.scheme.type.gb2829", "ja-JP", "gb2829", "抽样方案类型.gb/t 2829"),
            // dict.logistics.sampling.scheme.type.gb2829
            ("dict.logistics.sampling.scheme.type.gb2829", "zh-CN", "gb/t 2829", "抽样方案类型.gb/t 2829"),
            // dict.logistics.sampling.scheme.type.gb2829
            ("dict.logistics.sampling.scheme.type.gb2829", "zh-HK", "gb/t 2829", "抽样方案类型.gb/t 2829"),

            // dict.logistics.sampling.scheme.type.c_zero
            ("dict.logistics.sampling.scheme.type.c_zero", "en-US", "c_zero", "抽样方案类型.c=0抽样"),
            // dict.logistics.sampling.scheme.type.c_zero
            ("dict.logistics.sampling.scheme.type.c_zero", "ja-JP", "c_zero", "抽样方案类型.c=0抽样"),
            // dict.logistics.sampling.scheme.type.c_zero
            ("dict.logistics.sampling.scheme.type.c_zero", "zh-CN", "c=0抽样", "抽样方案类型.c=0抽样"),
            // dict.logistics.sampling.scheme.type.c_zero
            ("dict.logistics.sampling.scheme.type.c_zero", "zh-HK", "c=0抽样", "抽样方案类型.c=0抽样"),

            // dict.logistics.sampling.scheme.type.continuous
            ("dict.logistics.sampling.scheme.type.continuous", "en-US", "continuous", "抽样方案类型.连续抽样"),
            // dict.logistics.sampling.scheme.type.continuous
            ("dict.logistics.sampling.scheme.type.continuous", "ja-JP", "continuous", "抽样方案类型.连续抽样"),
            // dict.logistics.sampling.scheme.type.continuous
            ("dict.logistics.sampling.scheme.type.continuous", "zh-CN", "连续抽样", "抽样方案类型.连续抽样"),
            // dict.logistics.sampling.scheme.type.continuous
            ("dict.logistics.sampling.scheme.type.continuous", "zh-HK", "连续抽样", "抽样方案类型.连续抽样"),

            // dict.logistics.sampling.scheme.type.skip_lot
            ("dict.logistics.sampling.scheme.type.skip_lot", "en-US", "skip_lot", "抽样方案类型.跳批抽样"),
            // dict.logistics.sampling.scheme.type.skip_lot
            ("dict.logistics.sampling.scheme.type.skip_lot", "ja-JP", "skip_lot", "抽样方案类型.跳批抽样"),
            // dict.logistics.sampling.scheme.type.skip_lot
            ("dict.logistics.sampling.scheme.type.skip_lot", "zh-CN", "跳批抽样", "抽样方案类型.跳批抽样"),
            // dict.logistics.sampling.scheme.type.skip_lot
            ("dict.logistics.sampling.scheme.type.skip_lot", "zh-HK", "跳批抽样", "抽样方案类型.跳批抽样"),

            // dict.logistics.special.procurement.10
            ("dict.logistics.special.procurement.10", "en-US", "寄售", "特殊采购类别.寄售"),
            // dict.logistics.special.procurement.10
            ("dict.logistics.special.procurement.10", "ja-JP", "寄售", "特殊采购类别.寄售"),
            // dict.logistics.special.procurement.10
            ("dict.logistics.special.procurement.10", "zh-CN", "寄售", "特殊采购类别.寄售"),
            // dict.logistics.special.procurement.10
            ("dict.logistics.special.procurement.10", "zh-HK", "寄售", "特殊采购类别.寄售"),

            // dict.logistics.special.procurement.30
            ("dict.logistics.special.procurement.30", "en-US", "外协加工", "特殊采购类别.外协加工"),
            // dict.logistics.special.procurement.30
            ("dict.logistics.special.procurement.30", "ja-JP", "外协加工", "特殊采购类别.外协加工"),
            // dict.logistics.special.procurement.30
            ("dict.logistics.special.procurement.30", "zh-CN", "外协加工", "特殊采购类别.外协加工"),
            // dict.logistics.special.procurement.30
            ("dict.logistics.special.procurement.30", "zh-HK", "外协加工", "特殊采购类别.外协加工"),

            // dict.logistics.special.procurement.50
            ("dict.logistics.special.procurement.50", "en-US", "虚设品号", "特殊采购类别.虚设品号"),
            // dict.logistics.special.procurement.50
            ("dict.logistics.special.procurement.50", "ja-JP", "虚设品号", "特殊采购类别.虚设品号"),
            // dict.logistics.special.procurement.50
            ("dict.logistics.special.procurement.50", "zh-CN", "虚设品号", "特殊采购类别.虚设品号"),
            // dict.logistics.special.procurement.50
            ("dict.logistics.special.procurement.50", "zh-HK", "虚设品号", "特殊采购类别.虚设品号"),

            // dict.logistics.supplier.category.strategic
            ("dict.logistics.supplier.category.strategic", "en-US", "strategic", "供应商类别.战略供应商"),
            // dict.logistics.supplier.category.strategic
            ("dict.logistics.supplier.category.strategic", "ja-JP", "strategic", "供应商类别.战略供应商"),
            // dict.logistics.supplier.category.strategic
            ("dict.logistics.supplier.category.strategic", "zh-CN", "战略供应商", "供应商类别.战略供应商"),
            // dict.logistics.supplier.category.strategic
            ("dict.logistics.supplier.category.strategic", "zh-HK", "战略供应商", "供应商类别.战略供应商"),

            // dict.logistics.supplier.category.core
            ("dict.logistics.supplier.category.core", "en-US", "core", "供应商类别.核心供应商"),
            // dict.logistics.supplier.category.core
            ("dict.logistics.supplier.category.core", "ja-JP", "core", "供应商类别.核心供应商"),
            // dict.logistics.supplier.category.core
            ("dict.logistics.supplier.category.core", "zh-CN", "核心供应商", "供应商类别.核心供应商"),
            // dict.logistics.supplier.category.core
            ("dict.logistics.supplier.category.core", "zh-HK", "核心供应商", "供应商类别.核心供应商"),

            // dict.logistics.supplier.category.qualified
            ("dict.logistics.supplier.category.qualified", "en-US", "qualified", "供应商类别.合格供应商"),
            // dict.logistics.supplier.category.qualified
            ("dict.logistics.supplier.category.qualified", "ja-JP", "qualified", "供应商类别.合格供应商"),
            // dict.logistics.supplier.category.qualified
            ("dict.logistics.supplier.category.qualified", "zh-CN", "合格供应商", "供应商类别.合格供应商"),
            // dict.logistics.supplier.category.qualified
            ("dict.logistics.supplier.category.qualified", "zh-HK", "合格供应商", "供应商类别.合格供应商"),

            // dict.logistics.supplier.category.temporary
            ("dict.logistics.supplier.category.temporary", "en-US", "temporary", "供应商类别.临时供应商"),
            // dict.logistics.supplier.category.temporary
            ("dict.logistics.supplier.category.temporary", "ja-JP", "temporary", "供应商类别.临时供应商"),
            // dict.logistics.supplier.category.temporary
            ("dict.logistics.supplier.category.temporary", "zh-CN", "临时供应商", "供应商类别.临时供应商"),
            // dict.logistics.supplier.category.temporary
            ("dict.logistics.supplier.category.temporary", "zh-HK", "临时供应商", "供应商类别.临时供应商"),

            // dict.logistics.supplier.category.potential
            ("dict.logistics.supplier.category.potential", "en-US", "potential", "供应商类别.潜在供应商"),
            // dict.logistics.supplier.category.potential
            ("dict.logistics.supplier.category.potential", "ja-JP", "potential", "供应商类别.潜在供应商"),
            // dict.logistics.supplier.category.potential
            ("dict.logistics.supplier.category.potential", "zh-CN", "潜在供应商", "供应商类别.潜在供应商"),
            // dict.logistics.supplier.category.potential
            ("dict.logistics.supplier.category.potential", "zh-HK", "潜在供应商", "供应商类别.潜在供应商"),

            // dict.logistics.supplier.category.backup
            ("dict.logistics.supplier.category.backup", "en-US", "backup", "供应商类别.备选供应商"),
            // dict.logistics.supplier.category.backup
            ("dict.logistics.supplier.category.backup", "ja-JP", "backup", "供应商类别.备选供应商"),
            // dict.logistics.supplier.category.backup
            ("dict.logistics.supplier.category.backup", "zh-CN", "备选供应商", "供应商类别.备选供应商"),
            // dict.logistics.supplier.category.backup
            ("dict.logistics.supplier.category.backup", "zh-HK", "备选供应商", "供应商类别.备选供应商"),

            // dict.logistics.supplier.category.oem
            ("dict.logistics.supplier.category.oem", "en-US", "oem", "供应商类别.oem供应商"),
            // dict.logistics.supplier.category.oem
            ("dict.logistics.supplier.category.oem", "ja-JP", "oem", "供应商类别.oem供应商"),
            // dict.logistics.supplier.category.oem
            ("dict.logistics.supplier.category.oem", "zh-CN", "oem供应商", "供应商类别.oem供应商"),
            // dict.logistics.supplier.category.oem
            ("dict.logistics.supplier.category.oem", "zh-HK", "oem供应商", "供应商类别.oem供应商"),

            // dict.logistics.supplier.category.odm
            ("dict.logistics.supplier.category.odm", "en-US", "odm", "供应商类别.odm供应商"),
            // dict.logistics.supplier.category.odm
            ("dict.logistics.supplier.category.odm", "ja-JP", "odm", "供应商类别.odm供应商"),
            // dict.logistics.supplier.category.odm
            ("dict.logistics.supplier.category.odm", "zh-CN", "odm供应商", "供应商类别.odm供应商"),
            // dict.logistics.supplier.category.odm
            ("dict.logistics.supplier.category.odm", "zh-HK", "odm供应商", "供应商类别.odm供应商"),

            // dict.logistics.unit.of.measure.pc
            ("dict.logistics.unit.of.measure.pc", "en-US", "pc", "基本单位类别.件"),
            // dict.logistics.unit.of.measure.pc
            ("dict.logistics.unit.of.measure.pc", "ja-JP", "pc", "基本单位类别.件"),
            // dict.logistics.unit.of.measure.pc
            ("dict.logistics.unit.of.measure.pc", "zh-CN", "件", "基本单位类别.件"),
            // dict.logistics.unit.of.measure.pc
            ("dict.logistics.unit.of.measure.pc", "zh-HK", "件", "基本单位类别.件"),

            // dict.logistics.unit.of.measure.kg
            ("dict.logistics.unit.of.measure.kg", "en-US", "kg", "基本单位类别.千克"),
            // dict.logistics.unit.of.measure.kg
            ("dict.logistics.unit.of.measure.kg", "ja-JP", "kg", "基本单位类别.千克"),
            // dict.logistics.unit.of.measure.kg
            ("dict.logistics.unit.of.measure.kg", "zh-CN", "千克", "基本单位类别.千克"),
            // dict.logistics.unit.of.measure.kg
            ("dict.logistics.unit.of.measure.kg", "zh-HK", "千克", "基本单位类别.千克"),

            // dict.logistics.unit.of.measure.g
            ("dict.logistics.unit.of.measure.g", "en-US", "g", "基本单位类别.克"),
            // dict.logistics.unit.of.measure.g
            ("dict.logistics.unit.of.measure.g", "ja-JP", "g", "基本单位类别.克"),
            // dict.logistics.unit.of.measure.g
            ("dict.logistics.unit.of.measure.g", "zh-CN", "克", "基本单位类别.克"),
            // dict.logistics.unit.of.measure.g
            ("dict.logistics.unit.of.measure.g", "zh-HK", "克", "基本单位类别.克"),

            // dict.logistics.unit.of.measure.t
            ("dict.logistics.unit.of.measure.t", "en-US", "t", "基本单位类别.吨"),
            // dict.logistics.unit.of.measure.t
            ("dict.logistics.unit.of.measure.t", "ja-JP", "t", "基本单位类别.吨"),
            // dict.logistics.unit.of.measure.t
            ("dict.logistics.unit.of.measure.t", "zh-CN", "吨", "基本单位类别.吨"),
            // dict.logistics.unit.of.measure.t
            ("dict.logistics.unit.of.measure.t", "zh-HK", "吨", "基本单位类别.吨"),

            // dict.logistics.unit.of.measure.m
            ("dict.logistics.unit.of.measure.m", "en-US", "m", "基本单位类别.米"),
            // dict.logistics.unit.of.measure.m
            ("dict.logistics.unit.of.measure.m", "ja-JP", "m", "基本单位类别.米"),
            // dict.logistics.unit.of.measure.m
            ("dict.logistics.unit.of.measure.m", "zh-CN", "米", "基本单位类别.米"),
            // dict.logistics.unit.of.measure.m
            ("dict.logistics.unit.of.measure.m", "zh-HK", "米", "基本单位类别.米"),

            // dict.logistics.unit.of.measure.cm
            ("dict.logistics.unit.of.measure.cm", "en-US", "cm", "基本单位类别.厘米"),
            // dict.logistics.unit.of.measure.cm
            ("dict.logistics.unit.of.measure.cm", "ja-JP", "cm", "基本单位类别.厘米"),
            // dict.logistics.unit.of.measure.cm
            ("dict.logistics.unit.of.measure.cm", "zh-CN", "厘米", "基本单位类别.厘米"),
            // dict.logistics.unit.of.measure.cm
            ("dict.logistics.unit.of.measure.cm", "zh-HK", "厘米", "基本单位类别.厘米"),

            // dict.logistics.unit.of.measure.mm
            ("dict.logistics.unit.of.measure.mm", "en-US", "mm", "基本单位类别.毫米"),
            // dict.logistics.unit.of.measure.mm
            ("dict.logistics.unit.of.measure.mm", "ja-JP", "mm", "基本单位类别.毫米"),
            // dict.logistics.unit.of.measure.mm
            ("dict.logistics.unit.of.measure.mm", "zh-CN", "毫米", "基本单位类别.毫米"),
            // dict.logistics.unit.of.measure.mm
            ("dict.logistics.unit.of.measure.mm", "zh-HK", "毫米", "基本单位类别.毫米"),

            // dict.logistics.unit.of.measure.km
            ("dict.logistics.unit.of.measure.km", "en-US", "km", "基本单位类别.千米"),
            // dict.logistics.unit.of.measure.km
            ("dict.logistics.unit.of.measure.km", "ja-JP", "km", "基本单位类别.千米"),
            // dict.logistics.unit.of.measure.km
            ("dict.logistics.unit.of.measure.km", "zh-CN", "千米", "基本单位类别.千米"),
            // dict.logistics.unit.of.measure.km
            ("dict.logistics.unit.of.measure.km", "zh-HK", "千米", "基本单位类别.千米"),

            // dict.logistics.unit.of.measure.l
            ("dict.logistics.unit.of.measure.l", "en-US", "l", "基本单位类别.升"),
            // dict.logistics.unit.of.measure.l
            ("dict.logistics.unit.of.measure.l", "ja-JP", "l", "基本单位类别.升"),
            // dict.logistics.unit.of.measure.l
            ("dict.logistics.unit.of.measure.l", "zh-CN", "升", "基本单位类别.升"),
            // dict.logistics.unit.of.measure.l
            ("dict.logistics.unit.of.measure.l", "zh-HK", "升", "基本单位类别.升"),

            // dict.logistics.unit.of.measure.ml
            ("dict.logistics.unit.of.measure.ml", "en-US", "ml", "基本单位类别.毫升"),
            // dict.logistics.unit.of.measure.ml
            ("dict.logistics.unit.of.measure.ml", "ja-JP", "ml", "基本单位类别.毫升"),
            // dict.logistics.unit.of.measure.ml
            ("dict.logistics.unit.of.measure.ml", "zh-CN", "毫升", "基本单位类别.毫升"),
            // dict.logistics.unit.of.measure.ml
            ("dict.logistics.unit.of.measure.ml", "zh-HK", "毫升", "基本单位类别.毫升"),

            // dict.logistics.unit.of.measure.m3
            ("dict.logistics.unit.of.measure.m3", "en-US", "m3", "基本单位类别.立方米"),
            // dict.logistics.unit.of.measure.m3
            ("dict.logistics.unit.of.measure.m3", "ja-JP", "m3", "基本单位类别.立方米"),
            // dict.logistics.unit.of.measure.m3
            ("dict.logistics.unit.of.measure.m3", "zh-CN", "立方米", "基本单位类别.立方米"),
            // dict.logistics.unit.of.measure.m3
            ("dict.logistics.unit.of.measure.m3", "zh-HK", "立方米", "基本单位类别.立方米"),

            // dict.logistics.unit.of.measure.m2
            ("dict.logistics.unit.of.measure.m2", "en-US", "m2", "基本单位类别.平方米"),
            // dict.logistics.unit.of.measure.m2
            ("dict.logistics.unit.of.measure.m2", "ja-JP", "m2", "基本单位类别.平方米"),
            // dict.logistics.unit.of.measure.m2
            ("dict.logistics.unit.of.measure.m2", "zh-CN", "平方米", "基本单位类别.平方米"),
            // dict.logistics.unit.of.measure.m2
            ("dict.logistics.unit.of.measure.m2", "zh-HK", "平方米", "基本单位类别.平方米"),

            // dict.logistics.unit.of.measure.set
            ("dict.logistics.unit.of.measure.set", "en-US", "set", "基本单位类别.套"),
            // dict.logistics.unit.of.measure.set
            ("dict.logistics.unit.of.measure.set", "ja-JP", "set", "基本单位类别.套"),
            // dict.logistics.unit.of.measure.set
            ("dict.logistics.unit.of.measure.set", "zh-CN", "套", "基本单位类别.套"),
            // dict.logistics.unit.of.measure.set
            ("dict.logistics.unit.of.measure.set", "zh-HK", "套", "基本单位类别.套"),

            // dict.logistics.unit.of.measure.pr
            ("dict.logistics.unit.of.measure.pr", "en-US", "pr", "基本单位类别.对"),
            // dict.logistics.unit.of.measure.pr
            ("dict.logistics.unit.of.measure.pr", "ja-JP", "pr", "基本单位类别.对"),
            // dict.logistics.unit.of.measure.pr
            ("dict.logistics.unit.of.measure.pr", "zh-CN", "对", "基本单位类别.对"),
            // dict.logistics.unit.of.measure.pr
            ("dict.logistics.unit.of.measure.pr", "zh-HK", "对", "基本单位类别.对"),

            // dict.logistics.unit.of.measure.dz
            ("dict.logistics.unit.of.measure.dz", "en-US", "dz", "基本单位类别.打"),
            // dict.logistics.unit.of.measure.dz
            ("dict.logistics.unit.of.measure.dz", "ja-JP", "dz", "基本单位类别.打"),
            // dict.logistics.unit.of.measure.dz
            ("dict.logistics.unit.of.measure.dz", "zh-CN", "打", "基本单位类别.打"),
            // dict.logistics.unit.of.measure.dz
            ("dict.logistics.unit.of.measure.dz", "zh-HK", "打", "基本单位类别.打"),

            // dict.logistics.unit.of.measure.rol
            ("dict.logistics.unit.of.measure.rol", "en-US", "rol", "基本单位类别.卷"),
            // dict.logistics.unit.of.measure.rol
            ("dict.logistics.unit.of.measure.rol", "ja-JP", "rol", "基本单位类别.卷"),
            // dict.logistics.unit.of.measure.rol
            ("dict.logistics.unit.of.measure.rol", "zh-CN", "卷", "基本单位类别.卷"),
            // dict.logistics.unit.of.measure.rol
            ("dict.logistics.unit.of.measure.rol", "zh-HK", "卷", "基本单位类别.卷"),

            // dict.logistics.unit.of.measure.ct
            ("dict.logistics.unit.of.measure.ct", "en-US", "ct", "基本单位类别.箱"),
            // dict.logistics.unit.of.measure.ct
            ("dict.logistics.unit.of.measure.ct", "ja-JP", "ct", "基本单位类别.箱"),
            // dict.logistics.unit.of.measure.ct
            ("dict.logistics.unit.of.measure.ct", "zh-CN", "箱", "基本单位类别.箱"),
            // dict.logistics.unit.of.measure.ct
            ("dict.logistics.unit.of.measure.ct", "zh-HK", "箱", "基本单位类别.箱"),

            // dict.logistics.unit.of.measure.pk
            ("dict.logistics.unit.of.measure.pk", "en-US", "pk", "基本单位类别.包"),
            // dict.logistics.unit.of.measure.pk
            ("dict.logistics.unit.of.measure.pk", "ja-JP", "pk", "基本单位类别.包"),
            // dict.logistics.unit.of.measure.pk
            ("dict.logistics.unit.of.measure.pk", "zh-CN", "包", "基本单位类别.包"),
            // dict.logistics.unit.of.measure.pk
            ("dict.logistics.unit.of.measure.pk", "zh-HK", "包", "基本单位类别.包"),

            // dict.logistics.unit.of.measure.dr
            ("dict.logistics.unit.of.measure.dr", "en-US", "dr", "基本单位类别.桶"),
            // dict.logistics.unit.of.measure.dr
            ("dict.logistics.unit.of.measure.dr", "ja-JP", "dr", "基本单位类别.桶"),
            // dict.logistics.unit.of.measure.dr
            ("dict.logistics.unit.of.measure.dr", "zh-CN", "桶", "基本单位类别.桶"),
            // dict.logistics.unit.of.measure.dr
            ("dict.logistics.unit.of.measure.dr", "zh-HK", "桶", "基本单位类别.桶"),

            // dict.logistics.unit.of.measure.bo
            ("dict.logistics.unit.of.measure.bo", "en-US", "bo", "基本单位类别.瓶"),
            // dict.logistics.unit.of.measure.bo
            ("dict.logistics.unit.of.measure.bo", "ja-JP", "bo", "基本单位类别.瓶"),
            // dict.logistics.unit.of.measure.bo
            ("dict.logistics.unit.of.measure.bo", "zh-CN", "瓶", "基本单位类别.瓶"),
            // dict.logistics.unit.of.measure.bo
            ("dict.logistics.unit.of.measure.bo", "zh-HK", "瓶", "基本单位类别.瓶"),

            // dict.logistics.valuation.class.7920
            ("dict.logistics.valuation.class.7920", "en-US", "成品", "评估类别.成品"),
            // dict.logistics.valuation.class.7920
            ("dict.logistics.valuation.class.7920", "ja-JP", "成品", "评估类别.成品"),
            // dict.logistics.valuation.class.7920
            ("dict.logistics.valuation.class.7920", "zh-CN", "成品", "评估类别.成品"),
            // dict.logistics.valuation.class.7920
            ("dict.logistics.valuation.class.7920", "zh-HK", "成品", "评估类别.成品"),

            // dict.logistics.valuation.class.z300
            ("dict.logistics.valuation.class.z300", "en-US", "z300", "评估类别.原材料(cn)"),
            // dict.logistics.valuation.class.z300
            ("dict.logistics.valuation.class.z300", "ja-JP", "z300", "评估类别.原材料(cn)"),
            // dict.logistics.valuation.class.z300
            ("dict.logistics.valuation.class.z300", "zh-CN", "原材料(cn)", "评估类别.原材料(cn)"),
            // dict.logistics.valuation.class.z300
            ("dict.logistics.valuation.class.z300", "zh-HK", "原材料(cn)", "评估类别.原材料(cn)"),

            // dict.logistics.valuation.class.z790
            ("dict.logistics.valuation.class.z790", "en-US", "z790", "评估类别.半成品(cn)"),
            // dict.logistics.valuation.class.z790
            ("dict.logistics.valuation.class.z790", "ja-JP", "z790", "评估类别.半成品(cn)"),
            // dict.logistics.valuation.class.z790
            ("dict.logistics.valuation.class.z790", "zh-CN", "半成品(cn)", "评估类别.半成品(cn)"),
            // dict.logistics.valuation.class.z790
            ("dict.logistics.valuation.class.z790", "zh-HK", "半成品(cn)", "评估类别.半成品(cn)"),

            // dict.logistics.valuation.class.z792
            ("dict.logistics.valuation.class.z792", "en-US", "z792", "评估类别.成品(cn)"),
            // dict.logistics.valuation.class.z792
            ("dict.logistics.valuation.class.z792", "ja-JP", "z792", "评估类别.成品(cn)"),
            // dict.logistics.valuation.class.z792
            ("dict.logistics.valuation.class.z792", "zh-CN", "成品(cn)", "评估类别.成品(cn)"),
            // dict.logistics.valuation.class.z792
            ("dict.logistics.valuation.class.z792", "zh-HK", "成品(cn)", "评估类别.成品(cn)"),

            // dict.prod.aoi.inspection.line.1
            ("dict.prod.aoi.inspection.line.1", "en-US", "1", "aoi线别.1"),
            // dict.prod.aoi.inspection.line.1
            ("dict.prod.aoi.inspection.line.1", "ja-JP", "1", "aoi线别.1"),
            // dict.prod.aoi.inspection.line.1
            ("dict.prod.aoi.inspection.line.1", "zh-CN", "1", "aoi线别.1"),
            // dict.prod.aoi.inspection.line.1
            ("dict.prod.aoi.inspection.line.1", "zh-HK", "1", "aoi线别.1"),

            // dict.prod.aoi.inspection.line.2
            ("dict.prod.aoi.inspection.line.2", "en-US", "2", "aoi线别.2"),
            // dict.prod.aoi.inspection.line.2
            ("dict.prod.aoi.inspection.line.2", "ja-JP", "2", "aoi线别.2"),
            // dict.prod.aoi.inspection.line.2
            ("dict.prod.aoi.inspection.line.2", "zh-CN", "2", "aoi线别.2"),
            // dict.prod.aoi.inspection.line.2
            ("dict.prod.aoi.inspection.line.2", "zh-HK", "2", "aoi线别.2"),

            // dict.prod.aoi.inspection.line.1a
            ("dict.prod.aoi.inspection.line.1a", "en-US", "1a", "aoi线别.1a"),
            // dict.prod.aoi.inspection.line.1a
            ("dict.prod.aoi.inspection.line.1a", "ja-JP", "1a", "aoi线别.1a"),
            // dict.prod.aoi.inspection.line.1a
            ("dict.prod.aoi.inspection.line.1a", "zh-CN", "1a", "aoi线别.1a"),
            // dict.prod.aoi.inspection.line.1a
            ("dict.prod.aoi.inspection.line.1a", "zh-HK", "1a", "aoi线别.1a"),

            // dict.prod.assy.location.1
            ("dict.prod.assy.location.1", "en-US", "自插", "assy个所.自插"),
            // dict.prod.assy.location.1
            ("dict.prod.assy.location.1", "ja-JP", "自插", "assy个所.自插"),
            // dict.prod.assy.location.1
            ("dict.prod.assy.location.1", "zh-CN", "自插", "assy个所.自插"),
            // dict.prod.assy.location.1
            ("dict.prod.assy.location.1", "zh-HK", "自插", "assy个所.自插"),

            // dict.prod.assy.location.2
            ("dict.prod.assy.location.2", "en-US", "部品", "assy个所.部品"),
            // dict.prod.assy.location.2
            ("dict.prod.assy.location.2", "ja-JP", "部品", "assy个所.部品"),
            // dict.prod.assy.location.2
            ("dict.prod.assy.location.2", "zh-CN", "部品", "assy个所.部品"),
            // dict.prod.assy.location.2
            ("dict.prod.assy.location.2", "zh-HK", "部品", "assy个所.部品"),

            // dict.prod.assy.location.3
            ("dict.prod.assy.location.3", "en-US", "设计", "assy个所.设计"),
            // dict.prod.assy.location.3
            ("dict.prod.assy.location.3", "ja-JP", "设计", "assy个所.设计"),
            // dict.prod.assy.location.3
            ("dict.prod.assy.location.3", "zh-CN", "设计", "assy个所.设计"),
            // dict.prod.assy.location.3
            ("dict.prod.assy.location.3", "zh-HK", "设计", "assy个所.设计"),

            // dict.prod.assy.location.4
            ("dict.prod.assy.location.4", "en-US", "修正", "assy个所.修正"),
            // dict.prod.assy.location.4
            ("dict.prod.assy.location.4", "ja-JP", "修正", "assy个所.修正"),
            // dict.prod.assy.location.4
            ("dict.prod.assy.location.4", "zh-CN", "修正", "assy个所.修正"),
            // dict.prod.assy.location.4
            ("dict.prod.assy.location.4", "zh-HK", "修正", "assy个所.修正"),

            // dict.prod.assy.location.5
            ("dict.prod.assy.location.5", "en-US", "加工", "assy个所.加工"),
            // dict.prod.assy.location.5
            ("dict.prod.assy.location.5", "ja-JP", "加工", "assy个所.加工"),
            // dict.prod.assy.location.5
            ("dict.prod.assy.location.5", "zh-CN", "加工", "assy个所.加工"),
            // dict.prod.assy.location.5
            ("dict.prod.assy.location.5", "zh-HK", "加工", "assy个所.加工"),

            // dict.prod.assy.location.6
            ("dict.prod.assy.location.6", "en-US", "手插", "assy个所.手插"),
            // dict.prod.assy.location.6
            ("dict.prod.assy.location.6", "ja-JP", "手插", "assy个所.手插"),
            // dict.prod.assy.location.6
            ("dict.prod.assy.location.6", "zh-CN", "手插", "assy个所.手插"),
            // dict.prod.assy.location.6
            ("dict.prod.assy.location.6", "zh-HK", "手插", "assy个所.手插"),

            // dict.prod.assy.location.7
            ("dict.prod.assy.location.7", "en-US", "组立", "assy个所.组立"),
            // dict.prod.assy.location.7
            ("dict.prod.assy.location.7", "ja-JP", "组立", "assy个所.组立"),
            // dict.prod.assy.location.7
            ("dict.prod.assy.location.7", "zh-CN", "组立", "assy个所.组立"),
            // dict.prod.assy.location.7
            ("dict.prod.assy.location.7", "zh-HK", "组立", "assy个所.组立"),

            // dict.prod.assy.location.8
            ("dict.prod.assy.location.8", "en-US", "smt", "assy个所.smt"),
            // dict.prod.assy.location.8
            ("dict.prod.assy.location.8", "ja-JP", "smt", "assy个所.smt"),
            // dict.prod.assy.location.8
            ("dict.prod.assy.location.8", "zh-CN", "smt", "assy个所.smt"),
            // dict.prod.assy.location.8
            ("dict.prod.assy.location.8", "zh-HK", "smt", "assy个所.smt"),

            // dict.prod.assy.location.9
            ("dict.prod.assy.location.9", "en-US", "其他", "assy个所.其他"),
            // dict.prod.assy.location.9
            ("dict.prod.assy.location.9", "ja-JP", "其他", "assy个所.其他"),
            // dict.prod.assy.location.9
            ("dict.prod.assy.location.9", "zh-CN", "其他", "assy个所.其他"),
            // dict.prod.assy.location.9
            ("dict.prod.assy.location.9", "zh-HK", "其他", "assy个所.其他"),

            // dict.prod.ec.distinction.1
            ("dict.prod.ec.distinction.1", "en-US", "全仕向", "设变管理区分.全仕向"),
            // dict.prod.ec.distinction.1
            ("dict.prod.ec.distinction.1", "ja-JP", "全仕向", "设变管理区分.全仕向"),
            // dict.prod.ec.distinction.1
            ("dict.prod.ec.distinction.1", "zh-CN", "全仕向", "设变管理区分.全仕向"),
            // dict.prod.ec.distinction.1
            ("dict.prod.ec.distinction.1", "zh-HK", "全仕向", "设变管理区分.全仕向"),

            // dict.prod.ec.distinction.2
            ("dict.prod.ec.distinction.2", "en-US", "部管", "设变管理区分.部管"),
            // dict.prod.ec.distinction.2
            ("dict.prod.ec.distinction.2", "ja-JP", "部管", "设变管理区分.部管"),
            // dict.prod.ec.distinction.2
            ("dict.prod.ec.distinction.2", "zh-CN", "部管", "设变管理区分.部管"),
            // dict.prod.ec.distinction.2
            ("dict.prod.ec.distinction.2", "zh-HK", "部管", "设变管理区分.部管"),

            // dict.prod.ec.distinction.3
            ("dict.prod.ec.distinction.3", "en-US", "内部", "设变管理区分.内部"),
            // dict.prod.ec.distinction.3
            ("dict.prod.ec.distinction.3", "ja-JP", "内部", "设变管理区分.内部"),
            // dict.prod.ec.distinction.3
            ("dict.prod.ec.distinction.3", "zh-CN", "内部", "设变管理区分.内部"),
            // dict.prod.ec.distinction.3
            ("dict.prod.ec.distinction.3", "zh-HK", "内部", "设变管理区分.内部"),

            // dict.prod.ec.distinction.4
            ("dict.prod.ec.distinction.4", "en-US", "技术", "设变管理区分.技术"),
            // dict.prod.ec.distinction.4
            ("dict.prod.ec.distinction.4", "ja-JP", "技术", "设变管理区分.技术"),
            // dict.prod.ec.distinction.4
            ("dict.prod.ec.distinction.4", "zh-CN", "技术", "设变管理区分.技术"),
            // dict.prod.ec.distinction.4
            ("dict.prod.ec.distinction.4", "zh-HK", "技术", "设变管理区分.技术"),

            // dict.prod.ec.status.1
            ("dict.prod.ec.status.1", "en-US", "工作的", "设变状态.工作的"),
            // dict.prod.ec.status.1
            ("dict.prod.ec.status.1", "ja-JP", "工作的", "设变状态.工作的"),
            // dict.prod.ec.status.1
            ("dict.prod.ec.status.1", "zh-CN", "工作的", "设变状态.工作的"),
            // dict.prod.ec.status.1
            ("dict.prod.ec.status.1", "zh-HK", "工作的", "设变状态.工作的"),

            // dict.prod.ec.status.2
            ("dict.prod.ec.status.2", "en-US", "取消的", "设变状态.取消的"),
            // dict.prod.ec.status.2
            ("dict.prod.ec.status.2", "ja-JP", "取消的", "设变状态.取消的"),
            // dict.prod.ec.status.2
            ("dict.prod.ec.status.2", "zh-CN", "取消的", "设变状态.取消的"),
            // dict.prod.ec.status.2
            ("dict.prod.ec.status.2", "zh-HK", "取消的", "设变状态.取消的"),

            // dict.prod.ec.status.3
            ("dict.prod.ec.status.3", "en-US", "发行的", "设变状态.发行的"),
            // dict.prod.ec.status.3
            ("dict.prod.ec.status.3", "ja-JP", "发行的", "设变状态.发行的"),
            // dict.prod.ec.status.3
            ("dict.prod.ec.status.3", "zh-CN", "发行的", "设变状态.发行的"),
            // dict.prod.ec.status.3
            ("dict.prod.ec.status.3", "zh-HK", "发行的", "设变状态.发行的"),

            // dict.prod.ec.status.4
            ("dict.prod.ec.status.4", "en-US", "p.p中变更的", "设变状态.p.p中变更的"),
            // dict.prod.ec.status.4
            ("dict.prod.ec.status.4", "ja-JP", "p.p中变更的", "设变状态.p.p中变更的"),
            // dict.prod.ec.status.4
            ("dict.prod.ec.status.4", "zh-CN", "p.p中变更的", "设变状态.p.p中变更的"),
            // dict.prod.ec.status.4
            ("dict.prod.ec.status.4", "zh-HK", "p.p中变更的", "设变状态.p.p中变更的"),

            // dict.prod.ec.status.5
            ("dict.prod.ec.status.5", "en-US", "固定的", "设变状态.固定的"),
            // dict.prod.ec.status.5
            ("dict.prod.ec.status.5", "ja-JP", "固定的", "设变状态.固定的"),
            // dict.prod.ec.status.5
            ("dict.prod.ec.status.5", "zh-CN", "固定的", "设变状态.固定的"),
            // dict.prod.ec.status.5
            ("dict.prod.ec.status.5", "zh-HK", "固定的", "设变状态.固定的"),

            // dict.prod.ec.status.6
            ("dict.prod.ec.status.6", "en-US", "挂起的", "设变状态.挂起的"),
            // dict.prod.ec.status.6
            ("dict.prod.ec.status.6", "ja-JP", "挂起的", "设变状态.挂起的"),
            // dict.prod.ec.status.6
            ("dict.prod.ec.status.6", "zh-CN", "挂起的", "设变状态.挂起的"),
            // dict.prod.ec.status.6
            ("dict.prod.ec.status.6", "zh-HK", "挂起的", "设变状态.挂起的"),

            // dict.prod.ec.status.7
            ("dict.prod.ec.status.7", "en-US", "拒绝的", "设变状态.拒绝的"),
            // dict.prod.ec.status.7
            ("dict.prod.ec.status.7", "ja-JP", "拒绝的", "设变状态.拒绝的"),
            // dict.prod.ec.status.7
            ("dict.prod.ec.status.7", "zh-CN", "拒绝的", "设变状态.拒绝的"),
            // dict.prod.ec.status.7
            ("dict.prod.ec.status.7", "zh-HK", "拒绝的", "设变状态.拒绝的"),

            // dict.prod.equipment.status.0
            ("dict.prod.equipment.status.0", "en-US", "运行中", "设备状态.运行中"),
            // dict.prod.equipment.status.0
            ("dict.prod.equipment.status.0", "ja-JP", "运行中", "设备状态.运行中"),
            // dict.prod.equipment.status.0
            ("dict.prod.equipment.status.0", "zh-CN", "运行中", "设备状态.运行中"),
            // dict.prod.equipment.status.0
            ("dict.prod.equipment.status.0", "zh-HK", "运行中", "设备状态.运行中"),

            // dict.prod.equipment.status.1
            ("dict.prod.equipment.status.1", "en-US", "停机", "设备状态.停机"),
            // dict.prod.equipment.status.1
            ("dict.prod.equipment.status.1", "ja-JP", "停机", "设备状态.停机"),
            // dict.prod.equipment.status.1
            ("dict.prod.equipment.status.1", "zh-CN", "停机", "设备状态.停机"),
            // dict.prod.equipment.status.1
            ("dict.prod.equipment.status.1", "zh-HK", "停机", "设备状态.停机"),

            // dict.prod.equipment.status.2
            ("dict.prod.equipment.status.2", "en-US", "维修中", "设备状态.维修中"),
            // dict.prod.equipment.status.2
            ("dict.prod.equipment.status.2", "ja-JP", "维修中", "设备状态.维修中"),
            // dict.prod.equipment.status.2
            ("dict.prod.equipment.status.2", "zh-CN", "维修中", "设备状态.维修中"),
            // dict.prod.equipment.status.2
            ("dict.prod.equipment.status.2", "zh-HK", "维修中", "设备状态.维修中"),

            // dict.prod.equipment.status.3
            ("dict.prod.equipment.status.3", "en-US", "故障", "设备状态.故障"),
            // dict.prod.equipment.status.3
            ("dict.prod.equipment.status.3", "ja-JP", "故障", "设备状态.故障"),
            // dict.prod.equipment.status.3
            ("dict.prod.equipment.status.3", "zh-CN", "故障", "设备状态.故障"),
            // dict.prod.equipment.status.3
            ("dict.prod.equipment.status.3", "zh-HK", "故障", "设备状态.故障"),

            // dict.prod.equipment.status.4
            ("dict.prod.equipment.status.4", "en-US", "待报废", "设备状态.待报废"),
            // dict.prod.equipment.status.4
            ("dict.prod.equipment.status.4", "ja-JP", "待报废", "设备状态.待报废"),
            // dict.prod.equipment.status.4
            ("dict.prod.equipment.status.4", "zh-CN", "待报废", "设备状态.待报废"),
            // dict.prod.equipment.status.4
            ("dict.prod.equipment.status.4", "zh-HK", "待报废", "设备状态.待报废"),

            // dict.prod.equipment.status.5
            ("dict.prod.equipment.status.5", "en-US", "已报废", "设备状态.已报废"),
            // dict.prod.equipment.status.5
            ("dict.prod.equipment.status.5", "ja-JP", "已报废", "设备状态.已报废"),
            // dict.prod.equipment.status.5
            ("dict.prod.equipment.status.5", "zh-CN", "已报废", "设备状态.已报废"),
            // dict.prod.equipment.status.5
            ("dict.prod.equipment.status.5", "zh-HK", "已报废", "设备状态.已报废"),

            // dict.prod.equipment.type.0
            ("dict.prod.equipment.type.0", "en-US", "生产设备", "设备类型.生产设备"),
            // dict.prod.equipment.type.0
            ("dict.prod.equipment.type.0", "ja-JP", "生产设备", "设备类型.生产设备"),
            // dict.prod.equipment.type.0
            ("dict.prod.equipment.type.0", "zh-CN", "生产设备", "设备类型.生产设备"),
            // dict.prod.equipment.type.0
            ("dict.prod.equipment.type.0", "zh-HK", "生产设备", "设备类型.生产设备"),

            // dict.prod.equipment.type.1
            ("dict.prod.equipment.type.1", "en-US", "检测设备", "设备类型.检测设备"),
            // dict.prod.equipment.type.1
            ("dict.prod.equipment.type.1", "ja-JP", "检测设备", "设备类型.检测设备"),
            // dict.prod.equipment.type.1
            ("dict.prod.equipment.type.1", "zh-CN", "检测设备", "设备类型.检测设备"),
            // dict.prod.equipment.type.1
            ("dict.prod.equipment.type.1", "zh-HK", "检测设备", "设备类型.检测设备"),

            // dict.prod.equipment.type.2
            ("dict.prod.equipment.type.2", "en-US", "包装设备", "设备类型.包装设备"),
            // dict.prod.equipment.type.2
            ("dict.prod.equipment.type.2", "ja-JP", "包装设备", "设备类型.包装设备"),
            // dict.prod.equipment.type.2
            ("dict.prod.equipment.type.2", "zh-CN", "包装设备", "设备类型.包装设备"),
            // dict.prod.equipment.type.2
            ("dict.prod.equipment.type.2", "zh-HK", "包装设备", "设备类型.包装设备"),

            // dict.prod.equipment.type.3
            ("dict.prod.equipment.type.3", "en-US", "物流设备", "设备类型.物流设备"),
            // dict.prod.equipment.type.3
            ("dict.prod.equipment.type.3", "ja-JP", "物流设备", "设备类型.物流设备"),
            // dict.prod.equipment.type.3
            ("dict.prod.equipment.type.3", "zh-CN", "物流设备", "设备类型.物流设备"),
            // dict.prod.equipment.type.3
            ("dict.prod.equipment.type.3", "zh-HK", "物流设备", "设备类型.物流设备"),

            // dict.prod.equipment.type.4
            ("dict.prod.equipment.type.4", "en-US", "辅助设备", "设备类型.辅助设备"),
            // dict.prod.equipment.type.4
            ("dict.prod.equipment.type.4", "ja-JP", "辅助设备", "设备类型.辅助设备"),
            // dict.prod.equipment.type.4
            ("dict.prod.equipment.type.4", "zh-CN", "辅助设备", "设备类型.辅助设备"),
            // dict.prod.equipment.type.4
            ("dict.prod.equipment.type.4", "zh-HK", "辅助设备", "设备类型.辅助设备"),

            // dict.prod.maintenance.type.0
            ("dict.prod.maintenance.type.0", "en-US", "定期保养", "维护类型.定期保养"),
            // dict.prod.maintenance.type.0
            ("dict.prod.maintenance.type.0", "ja-JP", "定期保养", "维护类型.定期保养"),
            // dict.prod.maintenance.type.0
            ("dict.prod.maintenance.type.0", "zh-CN", "定期保养", "维护类型.定期保养"),
            // dict.prod.maintenance.type.0
            ("dict.prod.maintenance.type.0", "zh-HK", "定期保养", "维护类型.定期保养"),

            // dict.prod.maintenance.type.1
            ("dict.prod.maintenance.type.1", "en-US", "故障维修", "维护类型.故障维修"),
            // dict.prod.maintenance.type.1
            ("dict.prod.maintenance.type.1", "ja-JP", "故障维修", "维护类型.故障维修"),
            // dict.prod.maintenance.type.1
            ("dict.prod.maintenance.type.1", "zh-CN", "故障维修", "维护类型.故障维修"),
            // dict.prod.maintenance.type.1
            ("dict.prod.maintenance.type.1", "zh-HK", "故障维修", "维护类型.故障维修"),

            // dict.prod.maintenance.type.2
            ("dict.prod.maintenance.type.2", "en-US", "大修", "维护类型.大修"),
            // dict.prod.maintenance.type.2
            ("dict.prod.maintenance.type.2", "ja-JP", "大修", "维护类型.大修"),
            // dict.prod.maintenance.type.2
            ("dict.prod.maintenance.type.2", "zh-CN", "大修", "维护类型.大修"),
            // dict.prod.maintenance.type.2
            ("dict.prod.maintenance.type.2", "zh-HK", "大修", "维护类型.大修"),

            // dict.prod.maintenance.type.3
            ("dict.prod.maintenance.type.3", "en-US", "改造升级", "维护类型.改造升级"),
            // dict.prod.maintenance.type.3
            ("dict.prod.maintenance.type.3", "ja-JP", "改造升级", "维护类型.改造升级"),
            // dict.prod.maintenance.type.3
            ("dict.prod.maintenance.type.3", "zh-CN", "改造升级", "维护类型.改造升级"),
            // dict.prod.maintenance.type.3
            ("dict.prod.maintenance.type.3", "zh-HK", "改造升级", "维护类型.改造升级"),

            // dict.prod.maintenance.type.4
            ("dict.prod.maintenance.type.4", "en-US", "其他", "维护类型.其他"),
            // dict.prod.maintenance.type.4
            ("dict.prod.maintenance.type.4", "ja-JP", "其他", "维护类型.其他"),
            // dict.prod.maintenance.type.4
            ("dict.prod.maintenance.type.4", "zh-CN", "其他", "维护类型.其他"),
            // dict.prod.maintenance.type.4
            ("dict.prod.maintenance.type.4", "zh-HK", "其他", "维护类型.其他"),

            // dict.prod.nonachievement.reason.1
            ("dict.prod.nonachievement.reason.1", "en-US", "清机", "未达成原因.清机"),
            // dict.prod.nonachievement.reason.1
            ("dict.prod.nonachievement.reason.1", "ja-JP", "清机", "未达成原因.清机"),
            // dict.prod.nonachievement.reason.1
            ("dict.prod.nonachievement.reason.1", "zh-CN", "清机", "未达成原因.清机"),
            // dict.prod.nonachievement.reason.1
            ("dict.prod.nonachievement.reason.1", "zh-HK", "清机", "未达成原因.清机"),

            // dict.prod.nonachievement.reason.2
            ("dict.prod.nonachievement.reason.2", "en-US", "测试慢,测试修理机", "未达成原因.测试慢,测试修理机"),
            // dict.prod.nonachievement.reason.2
            ("dict.prod.nonachievement.reason.2", "ja-JP", "测试慢,测试修理机", "未达成原因.测试慢,测试修理机"),
            // dict.prod.nonachievement.reason.2
            ("dict.prod.nonachievement.reason.2", "zh-CN", "测试慢,测试修理机", "未达成原因.测试慢,测试修理机"),
            // dict.prod.nonachievement.reason.2
            ("dict.prod.nonachievement.reason.2", "zh-HK", "测试慢,测试修理机", "未达成原因.测试慢,测试修理机"),

            // dict.prod.nonachievement.reason.3
            ("dict.prod.nonachievement.reason.3", "en-US", "修理试机", "未达成原因.修理试机"),
            // dict.prod.nonachievement.reason.3
            ("dict.prod.nonachievement.reason.3", "ja-JP", "修理试机", "未达成原因.修理试机"),
            // dict.prod.nonachievement.reason.3
            ("dict.prod.nonachievement.reason.3", "zh-CN", "修理试机", "未达成原因.修理试机"),
            // dict.prod.nonachievement.reason.3
            ("dict.prod.nonachievement.reason.3", "zh-HK", "修理试机", "未达成原因.修理试机"),

            // dict.prod.nonachievement.reason.4
            ("dict.prod.nonachievement.reason.4", "en-US", "转机", "未达成原因.转机"),
            // dict.prod.nonachievement.reason.4
            ("dict.prod.nonachievement.reason.4", "ja-JP", "转机", "未达成原因.转机"),
            // dict.prod.nonachievement.reason.4
            ("dict.prod.nonachievement.reason.4", "zh-CN", "转机", "未达成原因.转机"),
            // dict.prod.nonachievement.reason.4
            ("dict.prod.nonachievement.reason.4", "zh-HK", "转机", "未达成原因.转机"),

            // dict.prod.nonachievement.reason.5
            ("dict.prod.nonachievement.reason.5", "en-US", "人员欠缺", "未达成原因.人员欠缺"),
            // dict.prod.nonachievement.reason.5
            ("dict.prod.nonachievement.reason.5", "ja-JP", "人员欠缺", "未达成原因.人员欠缺"),
            // dict.prod.nonachievement.reason.5
            ("dict.prod.nonachievement.reason.5", "zh-CN", "人员欠缺", "未达成原因.人员欠缺"),
            // dict.prod.nonachievement.reason.5
            ("dict.prod.nonachievement.reason.5", "zh-HK", "人员欠缺", "未达成原因.人员欠缺"),

            // dict.prod.nonachievement.reason.6
            ("dict.prod.nonachievement.reason.6", "en-US", "部品不良,欠料", "未达成原因.部品不良,欠料"),
            // dict.prod.nonachievement.reason.6
            ("dict.prod.nonachievement.reason.6", "ja-JP", "部品不良,欠料", "未达成原因.部品不良,欠料"),
            // dict.prod.nonachievement.reason.6
            ("dict.prod.nonachievement.reason.6", "zh-CN", "部品不良,欠料", "未达成原因.部品不良,欠料"),
            // dict.prod.nonachievement.reason.6
            ("dict.prod.nonachievement.reason.6", "zh-HK", "部品不良,欠料", "未达成原因.部品不良,欠料"),

            // dict.prod.nonachievement.reason.7
            ("dict.prod.nonachievement.reason.7", "en-US", "st差异大", "未达成原因.st差异大"),
            // dict.prod.nonachievement.reason.7
            ("dict.prod.nonachievement.reason.7", "ja-JP", "st差异大", "未达成原因.st差异大"),
            // dict.prod.nonachievement.reason.7
            ("dict.prod.nonachievement.reason.7", "zh-CN", "st差异大", "未达成原因.st差异大"),
            // dict.prod.nonachievement.reason.7
            ("dict.prod.nonachievement.reason.7", "zh-HK", "st差异大", "未达成原因.st差异大"),

            // dict.prod.nonachievement.reason.8
            ("dict.prod.nonachievement.reason.8", "en-US", "仪器设备,设置,调试,检查,故障,切换", "未达成原因.仪器设备,设置,调试,检查,故障,切换"),
            // dict.prod.nonachievement.reason.8
            ("dict.prod.nonachievement.reason.8", "ja-JP", "仪器设备,设置,调试,检查,故障,切换", "未达成原因.仪器设备,设置,调试,检查,故障,切换"),
            // dict.prod.nonachievement.reason.8
            ("dict.prod.nonachievement.reason.8", "zh-CN", "仪器设备,设置,调试,检查,故障,切换", "未达成原因.仪器设备,设置,调试,检查,故障,切换"),
            // dict.prod.nonachievement.reason.8
            ("dict.prod.nonachievement.reason.8", "zh-HK", "仪器设备,设置,调试,检查,故障,切换", "未达成原因.仪器设备,设置,调试,检查,故障,切换"),

            // dict.prod.nonachievement.reason.9
            ("dict.prod.nonachievement.reason.9", "en-US", "请假,旷工", "未达成原因.请假,旷工"),
            // dict.prod.nonachievement.reason.9
            ("dict.prod.nonachievement.reason.9", "ja-JP", "请假,旷工", "未达成原因.请假,旷工"),
            // dict.prod.nonachievement.reason.9
            ("dict.prod.nonachievement.reason.9", "zh-CN", "请假,旷工", "未达成原因.请假,旷工"),
            // dict.prod.nonachievement.reason.9
            ("dict.prod.nonachievement.reason.9", "zh-HK", "请假,旷工", "未达成原因.请假,旷工"),

            // dict.prod.nonachievement.reason.10
            ("dict.prod.nonachievement.reason.10", "en-US", "其他", "未达成原因.其他"),
            // dict.prod.nonachievement.reason.10
            ("dict.prod.nonachievement.reason.10", "ja-JP", "其他", "未达成原因.其他"),
            // dict.prod.nonachievement.reason.10
            ("dict.prod.nonachievement.reason.10", "zh-CN", "其他", "未达成原因.其他"),
            // dict.prod.nonachievement.reason.10
            ("dict.prod.nonachievement.reason.10", "zh-HK", "其他", "未达成原因.其他"),

            // dict.prod.nonachievement.reason.11
            ("dict.prod.nonachievement.reason.11", "en-US", "切换机种,仕向", "未达成原因.切换机种,仕向"),
            // dict.prod.nonachievement.reason.11
            ("dict.prod.nonachievement.reason.11", "ja-JP", "切换机种,仕向", "未达成原因.切换机种,仕向"),
            // dict.prod.nonachievement.reason.11
            ("dict.prod.nonachievement.reason.11", "zh-CN", "切换机种,仕向", "未达成原因.切换机种,仕向"),
            // dict.prod.nonachievement.reason.11
            ("dict.prod.nonachievement.reason.11", "zh-HK", "切换机种,仕向", "未达成原因.切换机种,仕向"),

            // dict.prod.nonachievement.reason.12
            ("dict.prod.nonachievement.reason.12", "en-US", "组立慢,加工多,工程多,下机慢,作业困难,升级慢", "未达成原因.组立慢,加工多,工程多,下机慢,作业困难,升级慢"),
            // dict.prod.nonachievement.reason.12
            ("dict.prod.nonachievement.reason.12", "ja-JP", "组立慢,加工多,工程多,下机慢,作业困难,升级慢", "未达成原因.组立慢,加工多,工程多,下机慢,作业困难,升级慢"),
            // dict.prod.nonachievement.reason.12
            ("dict.prod.nonachievement.reason.12", "zh-CN", "组立慢,加工多,工程多,下机慢,作业困难,升级慢", "未达成原因.组立慢,加工多,工程多,下机慢,作业困难,升级慢"),
            // dict.prod.nonachievement.reason.12
            ("dict.prod.nonachievement.reason.12", "zh-HK", "组立慢,加工多,工程多,下机慢,作业困难,升级慢", "未达成原因.组立慢,加工多,工程多,下机慢,作业困难,升级慢"),

            // dict.prod.nonachievement.reason.13
            ("dict.prod.nonachievement.reason.13", "en-US", "改修", "未达成原因.改修"),
            // dict.prod.nonachievement.reason.13
            ("dict.prod.nonachievement.reason.13", "ja-JP", "改修", "未达成原因.改修"),
            // dict.prod.nonachievement.reason.13
            ("dict.prod.nonachievement.reason.13", "zh-CN", "改修", "未达成原因.改修"),
            // dict.prod.nonachievement.reason.13
            ("dict.prod.nonachievement.reason.13", "zh-HK", "改修", "未达成原因.改修"),

            // dict.prod.nonachievement.reason.14
            ("dict.prod.nonachievement.reason.14", "en-US", "坏机多,不良多", "未达成原因.坏机多,不良多"),
            // dict.prod.nonachievement.reason.14
            ("dict.prod.nonachievement.reason.14", "ja-JP", "坏机多,不良多", "未达成原因.坏机多,不良多"),
            // dict.prod.nonachievement.reason.14
            ("dict.prod.nonachievement.reason.14", "zh-CN", "坏机多,不良多", "未达成原因.坏机多,不良多"),
            // dict.prod.nonachievement.reason.14
            ("dict.prod.nonachievement.reason.14", "zh-HK", "坏机多,不良多", "未达成原因.坏机多,不良多"),

            // dict.prod.nonachievement.reason.15
            ("dict.prod.nonachievement.reason.15", "en-US", "人员借调", "未达成原因.人员借调"),
            // dict.prod.nonachievement.reason.15
            ("dict.prod.nonachievement.reason.15", "ja-JP", "人员借调", "未达成原因.人员借调"),
            // dict.prod.nonachievement.reason.15
            ("dict.prod.nonachievement.reason.15", "zh-CN", "人员借调", "未达成原因.人员借调"),
            // dict.prod.nonachievement.reason.15
            ("dict.prod.nonachievement.reason.15", "zh-HK", "人员借调", "未达成原因.人员借调"),

            // dict.prod.nonachievement.reason.16
            ("dict.prod.nonachievement.reason.16", "en-US", "返工", "未达成原因.返工"),
            // dict.prod.nonachievement.reason.16
            ("dict.prod.nonachievement.reason.16", "ja-JP", "返工", "未达成原因.返工"),
            // dict.prod.nonachievement.reason.16
            ("dict.prod.nonachievement.reason.16", "zh-CN", "返工", "未达成原因.返工"),
            // dict.prod.nonachievement.reason.16
            ("dict.prod.nonachievement.reason.16", "zh-HK", "返工", "未达成原因.返工"),

            // dict.prod.nonachievement.reason.17
            ("dict.prod.nonachievement.reason.17", "en-US", "下机慢", "未达成原因.下机慢"),
            // dict.prod.nonachievement.reason.17
            ("dict.prod.nonachievement.reason.17", "ja-JP", "下机慢", "未达成原因.下机慢"),
            // dict.prod.nonachievement.reason.17
            ("dict.prod.nonachievement.reason.17", "zh-CN", "下机慢", "未达成原因.下机慢"),
            // dict.prod.nonachievement.reason.17
            ("dict.prod.nonachievement.reason.17", "zh-HK", "下机慢", "未达成原因.下机慢"),

            // dict.prod.nonachievement.reason.18
            ("dict.prod.nonachievement.reason.18", "en-US", "学习中,新人员学习,开会", "未达成原因.学习中,新人员学习,开会"),
            // dict.prod.nonachievement.reason.18
            ("dict.prod.nonachievement.reason.18", "ja-JP", "学习中,新人员学习,开会", "未达成原因.学习中,新人员学习,开会"),
            // dict.prod.nonachievement.reason.18
            ("dict.prod.nonachievement.reason.18", "zh-CN", "学习中,新人员学习,开会", "未达成原因.学习中,新人员学习,开会"),
            // dict.prod.nonachievement.reason.18
            ("dict.prod.nonachievement.reason.18", "zh-HK", "学习中,新人员学习,开会", "未达成原因.学习中,新人员学习,开会"),

            // dict.prod.nonachievement.reason.19
            ("dict.prod.nonachievement.reason.19", "en-US", "正常", "未达成原因.正常"),
            // dict.prod.nonachievement.reason.19
            ("dict.prod.nonachievement.reason.19", "ja-JP", "正常", "未达成原因.正常"),
            // dict.prod.nonachievement.reason.19
            ("dict.prod.nonachievement.reason.19", "zh-CN", "正常", "未达成原因.正常"),
            // dict.prod.nonachievement.reason.19
            ("dict.prod.nonachievement.reason.19", "zh-HK", "正常", "未达成原因.正常"),

            // dict.prod.pcb.location.1
            ("dict.prod.pcb.location.1", "en-US", "翘脚", "pcb个所.翘脚"),
            // dict.prod.pcb.location.1
            ("dict.prod.pcb.location.1", "ja-JP", "翘脚", "pcb个所.翘脚"),
            // dict.prod.pcb.location.1
            ("dict.prod.pcb.location.1", "zh-CN", "翘脚", "pcb个所.翘脚"),
            // dict.prod.pcb.location.1
            ("dict.prod.pcb.location.1", "zh-HK", "翘脚", "pcb个所.翘脚"),

            // dict.prod.pcb.location.2
            ("dict.prod.pcb.location.2", "en-US", "生锡", "pcb个所.生锡"),
            // dict.prod.pcb.location.2
            ("dict.prod.pcb.location.2", "ja-JP", "生锡", "pcb个所.生锡"),
            // dict.prod.pcb.location.2
            ("dict.prod.pcb.location.2", "zh-CN", "生锡", "pcb个所.生锡"),
            // dict.prod.pcb.location.2
            ("dict.prod.pcb.location.2", "zh-HK", "生锡", "pcb个所.生锡"),

            // dict.prod.pcb.location.3
            ("dict.prod.pcb.location.3", "en-US", "锡量过多", "pcb个所.锡量过多"),
            // dict.prod.pcb.location.3
            ("dict.prod.pcb.location.3", "ja-JP", "锡量过多", "pcb个所.锡量过多"),
            // dict.prod.pcb.location.3
            ("dict.prod.pcb.location.3", "zh-CN", "锡量过多", "pcb个所.锡量过多"),
            // dict.prod.pcb.location.3
            ("dict.prod.pcb.location.3", "zh-HK", "锡量过多", "pcb个所.锡量过多"),

            // dict.prod.pcb.location.4
            ("dict.prod.pcb.location.4", "en-US", "空焊", "pcb个所.空焊"),
            // dict.prod.pcb.location.4
            ("dict.prod.pcb.location.4", "ja-JP", "空焊", "pcb个所.空焊"),
            // dict.prod.pcb.location.4
            ("dict.prod.pcb.location.4", "zh-CN", "空焊", "pcb个所.空焊"),
            // dict.prod.pcb.location.4
            ("dict.prod.pcb.location.4", "zh-HK", "空焊", "pcb个所.空焊"),

            // dict.prod.pcb.location.5
            ("dict.prod.pcb.location.5", "en-US", "漏件", "pcb个所.漏件"),
            // dict.prod.pcb.location.5
            ("dict.prod.pcb.location.5", "ja-JP", "漏件", "pcb个所.漏件"),
            // dict.prod.pcb.location.5
            ("dict.prod.pcb.location.5", "zh-CN", "漏件", "pcb个所.漏件"),
            // dict.prod.pcb.location.5
            ("dict.prod.pcb.location.5", "zh-HK", "漏件", "pcb个所.漏件"),

            // dict.prod.pcb.location.6
            ("dict.prod.pcb.location.6", "en-US", "发黄", "pcb个所.发黄"),
            // dict.prod.pcb.location.6
            ("dict.prod.pcb.location.6", "ja-JP", "发黄", "pcb个所.发黄"),
            // dict.prod.pcb.location.6
            ("dict.prod.pcb.location.6", "zh-CN", "发黄", "pcb个所.发黄"),
            // dict.prod.pcb.location.6
            ("dict.prod.pcb.location.6", "zh-HK", "发黄", "pcb个所.发黄"),

            // dict.prod.pcb.location.7
            ("dict.prod.pcb.location.7", "en-US", "ic pin 竖立", "pcb个所.ic pin 竖立"),
            // dict.prod.pcb.location.7
            ("dict.prod.pcb.location.7", "ja-JP", "ic pin 竖立", "pcb个所.ic pin 竖立"),
            // dict.prod.pcb.location.7
            ("dict.prod.pcb.location.7", "zh-CN", "ic pin 竖立", "pcb个所.ic pin 竖立"),
            // dict.prod.pcb.location.7
            ("dict.prod.pcb.location.7", "zh-HK", "ic pin 竖立", "pcb个所.ic pin 竖立"),

            // dict.prod.pcb.location.8
            ("dict.prod.pcb.location.8", "en-US", "连锡", "pcb个所.连锡"),
            // dict.prod.pcb.location.8
            ("dict.prod.pcb.location.8", "ja-JP", "连锡", "pcb个所.连锡"),
            // dict.prod.pcb.location.8
            ("dict.prod.pcb.location.8", "zh-CN", "连锡", "pcb个所.连锡"),
            // dict.prod.pcb.location.8
            ("dict.prod.pcb.location.8", "zh-HK", "连锡", "pcb个所.连锡"),

            // dict.prod.pcb.location.9
            ("dict.prod.pcb.location.9", "en-US", "异物附着", "pcb个所.异物附着"),
            // dict.prod.pcb.location.9
            ("dict.prod.pcb.location.9", "ja-JP", "异物附着", "pcb个所.异物附着"),
            // dict.prod.pcb.location.9
            ("dict.prod.pcb.location.9", "zh-CN", "异物附着", "pcb个所.异物附着"),
            // dict.prod.pcb.location.9
            ("dict.prod.pcb.location.9", "zh-HK", "异物附着", "pcb个所.异物附着"),

            // dict.prod.pcb.location.10
            ("dict.prod.pcb.location.10", "en-US", "底下有部品", "pcb个所.底下有部品"),
            // dict.prod.pcb.location.10
            ("dict.prod.pcb.location.10", "ja-JP", "底下有部品", "pcb个所.底下有部品"),
            // dict.prod.pcb.location.10
            ("dict.prod.pcb.location.10", "zh-CN", "底下有部品", "pcb个所.底下有部品"),
            // dict.prod.pcb.location.10
            ("dict.prod.pcb.location.10", "zh-HK", "底下有部品", "pcb个所.底下有部品"),

            // dict.prod.pcb.location.11
            ("dict.prod.pcb.location.11", "en-US", "基板不良", "pcb个所.基板不良"),
            // dict.prod.pcb.location.11
            ("dict.prod.pcb.location.11", "ja-JP", "基板不良", "pcb个所.基板不良"),
            // dict.prod.pcb.location.11
            ("dict.prod.pcb.location.11", "zh-CN", "基板不良", "pcb个所.基板不良"),
            // dict.prod.pcb.location.11
            ("dict.prod.pcb.location.11", "zh-HK", "基板不良", "pcb个所.基板不良"),

            // dict.prod.pcb.location.12
            ("dict.prod.pcb.location.12", "en-US", "ic pin 浮高", "pcb个所.ic pin 浮高"),
            // dict.prod.pcb.location.12
            ("dict.prod.pcb.location.12", "ja-JP", "ic pin 浮高", "pcb个所.ic pin 浮高"),
            // dict.prod.pcb.location.12
            ("dict.prod.pcb.location.12", "zh-CN", "ic pin 浮高", "pcb个所.ic pin 浮高"),
            // dict.prod.pcb.location.12
            ("dict.prod.pcb.location.12", "zh-HK", "ic pin 浮高", "pcb个所.ic pin 浮高"),

            // dict.prod.pcb.location.13
            ("dict.prod.pcb.location.13", "en-US", "红胶不良", "pcb个所.红胶不良"),
            // dict.prod.pcb.location.13
            ("dict.prod.pcb.location.13", "ja-JP", "红胶不良", "pcb个所.红胶不良"),
            // dict.prod.pcb.location.13
            ("dict.prod.pcb.location.13", "zh-CN", "红胶不良", "pcb个所.红胶不良"),
            // dict.prod.pcb.location.13
            ("dict.prod.pcb.location.13", "zh-HK", "红胶不良", "pcb个所.红胶不良"),

            // dict.prod.pcb.location.14
            ("dict.prod.pcb.location.14", "en-US", "反面", "pcb个所.反面"),
            // dict.prod.pcb.location.14
            ("dict.prod.pcb.location.14", "ja-JP", "反面", "pcb个所.反面"),
            // dict.prod.pcb.location.14
            ("dict.prod.pcb.location.14", "zh-CN", "反面", "pcb个所.反面"),
            // dict.prod.pcb.location.14
            ("dict.prod.pcb.location.14", "zh-HK", "反面", "pcb个所.反面"),

            // dict.prod.pcb.location.15
            ("dict.prod.pcb.location.15", "en-US", "位置偏移", "pcb个所.位置偏移"),
            // dict.prod.pcb.location.15
            ("dict.prod.pcb.location.15", "ja-JP", "位置偏移", "pcb个所.位置偏移"),
            // dict.prod.pcb.location.15
            ("dict.prod.pcb.location.15", "zh-CN", "位置偏移", "pcb个所.位置偏移"),
            // dict.prod.pcb.location.15
            ("dict.prod.pcb.location.15", "zh-HK", "位置偏移", "pcb个所.位置偏移"),

            // dict.prod.pcb.location.16
            ("dict.prod.pcb.location.16", "en-US", "部品不良", "pcb个所.部品不良"),
            // dict.prod.pcb.location.16
            ("dict.prod.pcb.location.16", "ja-JP", "部品不良", "pcb个所.部品不良"),
            // dict.prod.pcb.location.16
            ("dict.prod.pcb.location.16", "zh-CN", "部品不良", "pcb个所.部品不良"),
            // dict.prod.pcb.location.16
            ("dict.prod.pcb.location.16", "zh-HK", "部品不良", "pcb个所.部品不良"),

            // dict.prod.pcb.location.17
            ("dict.prod.pcb.location.17", "en-US", "部品破损", "pcb个所.部品破损"),
            // dict.prod.pcb.location.17
            ("dict.prod.pcb.location.17", "ja-JP", "部品破损", "pcb个所.部品破损"),
            // dict.prod.pcb.location.17
            ("dict.prod.pcb.location.17", "zh-CN", "部品破损", "pcb个所.部品破损"),
            // dict.prod.pcb.location.17
            ("dict.prod.pcb.location.17", "zh-HK", "部品破损", "pcb个所.部品破损"),

            // dict.prod.pcb.location.18
            ("dict.prod.pcb.location.18", "en-US", "立碑", "pcb个所.立碑"),
            // dict.prod.pcb.location.18
            ("dict.prod.pcb.location.18", "ja-JP", "立碑", "pcb个所.立碑"),
            // dict.prod.pcb.location.18
            ("dict.prod.pcb.location.18", "zh-CN", "立碑", "pcb个所.立碑"),
            // dict.prod.pcb.location.18
            ("dict.prod.pcb.location.18", "zh-HK", "立碑", "pcb个所.立碑"),

            // dict.prod.pcb.location.19
            ("dict.prod.pcb.location.19", "en-US", "翻面", "pcb个所.翻面"),
            // dict.prod.pcb.location.19
            ("dict.prod.pcb.location.19", "ja-JP", "翻面", "pcb个所.翻面"),
            // dict.prod.pcb.location.19
            ("dict.prod.pcb.location.19", "zh-CN", "翻面", "pcb个所.翻面"),
            // dict.prod.pcb.location.19
            ("dict.prod.pcb.location.19", "zh-HK", "翻面", "pcb个所.翻面"),

            // dict.prod.pcb.location.20
            ("dict.prod.pcb.location.20", "en-US", "撞件", "pcb个所.撞件"),
            // dict.prod.pcb.location.20
            ("dict.prod.pcb.location.20", "ja-JP", "撞件", "pcb个所.撞件"),
            // dict.prod.pcb.location.20
            ("dict.prod.pcb.location.20", "zh-CN", "撞件", "pcb个所.撞件"),
            // dict.prod.pcb.location.20
            ("dict.prod.pcb.location.20", "zh-HK", "撞件", "pcb个所.撞件"),

            // dict.prod.pcb.location.21
            ("dict.prod.pcb.location.21", "en-US", "错料", "pcb个所.错料"),
            // dict.prod.pcb.location.21
            ("dict.prod.pcb.location.21", "ja-JP", "错料", "pcb个所.错料"),
            // dict.prod.pcb.location.21
            ("dict.prod.pcb.location.21", "zh-CN", "错料", "pcb个所.错料"),
            // dict.prod.pcb.location.21
            ("dict.prod.pcb.location.21", "zh-HK", "错料", "pcb个所.错料"),

            // dict.prod.pcb.location.22
            ("dict.prod.pcb.location.22", "en-US", "侧立", "pcb个所.侧立"),
            // dict.prod.pcb.location.22
            ("dict.prod.pcb.location.22", "ja-JP", "侧立", "pcb个所.侧立"),
            // dict.prod.pcb.location.22
            ("dict.prod.pcb.location.22", "zh-CN", "侧立", "pcb个所.侧立"),
            // dict.prod.pcb.location.22
            ("dict.prod.pcb.location.22", "zh-HK", "侧立", "pcb个所.侧立"),

            // dict.prod.pcb.location.23
            ("dict.prod.pcb.location.23", "en-US", "反向", "pcb个所.反向"),
            // dict.prod.pcb.location.23
            ("dict.prod.pcb.location.23", "ja-JP", "反向", "pcb个所.反向"),
            // dict.prod.pcb.location.23
            ("dict.prod.pcb.location.23", "zh-CN", "反向", "pcb个所.反向"),
            // dict.prod.pcb.location.23
            ("dict.prod.pcb.location.23", "zh-HK", "反向", "pcb个所.反向"),

            // dict.prod.pcb.location.24
            ("dict.prod.pcb.location.24", "en-US", "pcb不良", "pcb个所.pcb不良"),
            // dict.prod.pcb.location.24
            ("dict.prod.pcb.location.24", "ja-JP", "pcb不良", "pcb个所.pcb不良"),
            // dict.prod.pcb.location.24
            ("dict.prod.pcb.location.24", "zh-CN", "pcb不良", "pcb个所.pcb不良"),
            // dict.prod.pcb.location.24
            ("dict.prod.pcb.location.24", "zh-HK", "pcb不良", "pcb个所.pcb不良"),

            // dict.prod.pcb.location.25
            ("dict.prod.pcb.location.25", "en-US", "焊接不良", "pcb个所.焊接不良"),
            // dict.prod.pcb.location.25
            ("dict.prod.pcb.location.25", "ja-JP", "焊接不良", "pcb个所.焊接不良"),
            // dict.prod.pcb.location.25
            ("dict.prod.pcb.location.25", "zh-CN", "焊接不良", "pcb个所.焊接不良"),
            // dict.prod.pcb.location.25
            ("dict.prod.pcb.location.25", "zh-HK", "焊接不良", "pcb个所.焊接不良"),

            // dict.prod.pcb.location.26
            ("dict.prod.pcb.location.26", "en-US", "极性相违", "pcb个所.极性相违"),
            // dict.prod.pcb.location.26
            ("dict.prod.pcb.location.26", "ja-JP", "极性相违", "pcb个所.极性相违"),
            // dict.prod.pcb.location.26
            ("dict.prod.pcb.location.26", "zh-CN", "极性相违", "pcb个所.极性相违"),
            // dict.prod.pcb.location.26
            ("dict.prod.pcb.location.26", "zh-HK", "极性相违", "pcb个所.极性相违"),

            // dict.prod.pcb.location.27
            ("dict.prod.pcb.location.27", "en-US", "多件", "pcb个所.多件"),
            // dict.prod.pcb.location.27
            ("dict.prod.pcb.location.27", "ja-JP", "多件", "pcb个所.多件"),
            // dict.prod.pcb.location.27
            ("dict.prod.pcb.location.27", "zh-CN", "多件", "pcb个所.多件"),
            // dict.prod.pcb.location.27
            ("dict.prod.pcb.location.27", "zh-HK", "多件", "pcb个所.多件"),

            // dict.prod.pcb.location.28
            ("dict.prod.pcb.location.28", "en-US", "锡少", "pcb个所.锡少"),
            // dict.prod.pcb.location.28
            ("dict.prod.pcb.location.28", "ja-JP", "锡少", "pcb个所.锡少"),
            // dict.prod.pcb.location.28
            ("dict.prod.pcb.location.28", "zh-CN", "锡少", "pcb个所.锡少"),
            // dict.prod.pcb.location.28
            ("dict.prod.pcb.location.28", "zh-HK", "锡少", "pcb个所.锡少"),

            // dict.prod.pcba.function.category.1
            ("dict.prod.pcba.function.category.1", "en-US", "a", "pcba功能类别.a"),
            // dict.prod.pcba.function.category.1
            ("dict.prod.pcba.function.category.1", "ja-JP", "a", "pcba功能类别.a"),
            // dict.prod.pcba.function.category.1
            ("dict.prod.pcba.function.category.1", "zh-CN", "a", "pcba功能类别.a"),
            // dict.prod.pcba.function.category.1
            ("dict.prod.pcba.function.category.1", "zh-HK", "a", "pcba功能类别.a"),

            // dict.prod.pcba.function.category.2
            ("dict.prod.pcba.function.category.2", "en-US", "adoc", "pcba功能类别.adoc"),
            // dict.prod.pcba.function.category.2
            ("dict.prod.pcba.function.category.2", "ja-JP", "adoc", "pcba功能类别.adoc"),
            // dict.prod.pcba.function.category.2
            ("dict.prod.pcba.function.category.2", "zh-CN", "adoc", "pcba功能类别.adoc"),
            // dict.prod.pcba.function.category.2
            ("dict.prod.pcba.function.category.2", "zh-HK", "adoc", "pcba功能类别.adoc"),

            // dict.prod.pcba.function.category.3
            ("dict.prod.pcba.function.category.3", "en-US", "ana", "pcba功能类别.ana"),
            // dict.prod.pcba.function.category.3
            ("dict.prod.pcba.function.category.3", "ja-JP", "ana", "pcba功能类别.ana"),
            // dict.prod.pcba.function.category.3
            ("dict.prod.pcba.function.category.3", "zh-CN", "ana", "pcba功能类别.ana"),
            // dict.prod.pcba.function.category.3
            ("dict.prod.pcba.function.category.3", "zh-HK", "ana", "pcba功能类别.ana"),

            // dict.prod.pcba.function.category.4
            ("dict.prod.pcba.function.category.4", "en-US", "audio", "pcba功能类别.audio"),
            // dict.prod.pcba.function.category.4
            ("dict.prod.pcba.function.category.4", "ja-JP", "audio", "pcba功能类别.audio"),
            // dict.prod.pcba.function.category.4
            ("dict.prod.pcba.function.category.4", "zh-CN", "audio", "pcba功能类别.audio"),
            // dict.prod.pcba.function.category.4
            ("dict.prod.pcba.function.category.4", "zh-HK", "audio", "pcba功能类别.audio"),

            // dict.prod.pcba.function.category.5
            ("dict.prod.pcba.function.category.5", "en-US", "b", "pcba功能类别.b"),
            // dict.prod.pcba.function.category.5
            ("dict.prod.pcba.function.category.5", "ja-JP", "b", "pcba功能类别.b"),
            // dict.prod.pcba.function.category.5
            ("dict.prod.pcba.function.category.5", "zh-CN", "b", "pcba功能类别.b"),
            // dict.prod.pcba.function.category.5
            ("dict.prod.pcba.function.category.5", "zh-HK", "b", "pcba功能类别.b"),

            // dict.prod.pcba.function.category.6
            ("dict.prod.pcba.function.category.6", "en-US", "bottom", "pcba功能类别.bottom"),
            // dict.prod.pcba.function.category.6
            ("dict.prod.pcba.function.category.6", "ja-JP", "bottom", "pcba功能类别.bottom"),
            // dict.prod.pcba.function.category.6
            ("dict.prod.pcba.function.category.6", "zh-CN", "bottom", "pcba功能类别.bottom"),
            // dict.prod.pcba.function.category.6
            ("dict.prod.pcba.function.category.6", "zh-HK", "bottom", "pcba功能类别.bottom"),

            // dict.prod.pcba.function.category.7
            ("dict.prod.pcba.function.category.7", "en-US", "btice", "pcba功能类别.btice"),
            // dict.prod.pcba.function.category.7
            ("dict.prod.pcba.function.category.7", "ja-JP", "btice", "pcba功能类别.btice"),
            // dict.prod.pcba.function.category.7
            ("dict.prod.pcba.function.category.7", "zh-CN", "btice", "pcba功能类别.btice"),
            // dict.prod.pcba.function.category.7
            ("dict.prod.pcba.function.category.7", "zh-HK", "btice", "pcba功能类别.btice"),

            // dict.prod.pcba.function.category.8
            ("dict.prod.pcba.function.category.8", "en-US", "c", "pcba功能类别.c"),
            // dict.prod.pcba.function.category.8
            ("dict.prod.pcba.function.category.8", "ja-JP", "c", "pcba功能类别.c"),
            // dict.prod.pcba.function.category.8
            ("dict.prod.pcba.function.category.8", "zh-CN", "c", "pcba功能类别.c"),
            // dict.prod.pcba.function.category.8
            ("dict.prod.pcba.function.category.8", "zh-HK", "c", "pcba功能类别.c"),

            // dict.prod.pcba.function.category.9
            ("dict.prod.pcba.function.category.9", "en-US", "dspl", "pcba功能类别.dspl"),
            // dict.prod.pcba.function.category.9
            ("dict.prod.pcba.function.category.9", "ja-JP", "dspl", "pcba功能类别.dspl"),
            // dict.prod.pcba.function.category.9
            ("dict.prod.pcba.function.category.9", "zh-CN", "dspl", "pcba功能类别.dspl"),
            // dict.prod.pcba.function.category.9
            ("dict.prod.pcba.function.category.9", "zh-HK", "dspl", "pcba功能类别.dspl"),

            // dict.prod.pcba.function.category.10
            ("dict.prod.pcba.function.category.10", "en-US", "enc", "pcba功能类别.enc"),
            // dict.prod.pcba.function.category.10
            ("dict.prod.pcba.function.category.10", "ja-JP", "enc", "pcba功能类别.enc"),
            // dict.prod.pcba.function.category.10
            ("dict.prod.pcba.function.category.10", "zh-CN", "enc", "pcba功能类别.enc"),
            // dict.prod.pcba.function.category.10
            ("dict.prod.pcba.function.category.10", "zh-HK", "enc", "pcba功能类别.enc"),

            // dict.prod.pcba.function.category.11
            ("dict.prod.pcba.function.category.11", "en-US", "front", "pcba功能类别.front"),
            // dict.prod.pcba.function.category.11
            ("dict.prod.pcba.function.category.11", "ja-JP", "front", "pcba功能类别.front"),
            // dict.prod.pcba.function.category.11
            ("dict.prod.pcba.function.category.11", "zh-CN", "front", "pcba功能类别.front"),
            // dict.prod.pcba.function.category.11
            ("dict.prod.pcba.function.category.11", "zh-HK", "front", "pcba功能类别.front"),

            // dict.prod.pcba.function.category.12
            ("dict.prod.pcba.function.category.12", "en-US", "input", "pcba功能类别.input"),
            // dict.prod.pcba.function.category.12
            ("dict.prod.pcba.function.category.12", "ja-JP", "input", "pcba功能类别.input"),
            // dict.prod.pcba.function.category.12
            ("dict.prod.pcba.function.category.12", "zh-CN", "input", "pcba功能类别.input"),
            // dict.prod.pcba.function.category.12
            ("dict.prod.pcba.function.category.12", "zh-HK", "input", "pcba功能类别.input"),

            // dict.prod.pcba.function.category.13
            ("dict.prod.pcba.function.category.13", "en-US", "io", "pcba功能类别.io"),
            // dict.prod.pcba.function.category.13
            ("dict.prod.pcba.function.category.13", "ja-JP", "io", "pcba功能类别.io"),
            // dict.prod.pcba.function.category.13
            ("dict.prod.pcba.function.category.13", "zh-CN", "io", "pcba功能类别.io"),
            // dict.prod.pcba.function.category.13
            ("dict.prod.pcba.function.category.13", "zh-HK", "io", "pcba功能类别.io"),

            // dict.prod.pcba.function.category.14
            ("dict.prod.pcba.function.category.14", "en-US", "jack", "pcba功能类别.jack"),
            // dict.prod.pcba.function.category.14
            ("dict.prod.pcba.function.category.14", "ja-JP", "jack", "pcba功能类别.jack"),
            // dict.prod.pcba.function.category.14
            ("dict.prod.pcba.function.category.14", "zh-CN", "jack", "pcba功能类别.jack"),
            // dict.prod.pcba.function.category.14
            ("dict.prod.pcba.function.category.14", "zh-HK", "jack", "pcba功能类别.jack"),

            // dict.prod.pcba.function.category.15
            ("dict.prod.pcba.function.category.15", "en-US", "l", "pcba功能类别.l"),
            // dict.prod.pcba.function.category.15
            ("dict.prod.pcba.function.category.15", "ja-JP", "l", "pcba功能类别.l"),
            // dict.prod.pcba.function.category.15
            ("dict.prod.pcba.function.category.15", "zh-CN", "l", "pcba功能类别.l"),
            // dict.prod.pcba.function.category.15
            ("dict.prod.pcba.function.category.15", "zh-HK", "l", "pcba功能类别.l"),

            // dict.prod.pcba.function.category.16
            ("dict.prod.pcba.function.category.16", "en-US", "lcd", "pcba功能类别.lcd"),
            // dict.prod.pcba.function.category.16
            ("dict.prod.pcba.function.category.16", "ja-JP", "lcd", "pcba功能类别.lcd"),
            // dict.prod.pcba.function.category.16
            ("dict.prod.pcba.function.category.16", "zh-CN", "lcd", "pcba功能类别.lcd"),
            // dict.prod.pcba.function.category.16
            ("dict.prod.pcba.function.category.16", "zh-HK", "lcd", "pcba功能类别.lcd"),

            // dict.prod.pcba.function.category.17
            ("dict.prod.pcba.function.category.17", "en-US", "main", "pcba功能类别.main"),
            // dict.prod.pcba.function.category.17
            ("dict.prod.pcba.function.category.17", "ja-JP", "main", "pcba功能类别.main"),
            // dict.prod.pcba.function.category.17
            ("dict.prod.pcba.function.category.17", "zh-CN", "main", "pcba功能类别.main"),
            // dict.prod.pcba.function.category.17
            ("dict.prod.pcba.function.category.17", "zh-HK", "main", "pcba功能类别.main"),

            // dict.prod.pcba.function.category.18
            ("dict.prod.pcba.function.category.18", "en-US", "panel", "pcba功能类别.panel"),
            // dict.prod.pcba.function.category.18
            ("dict.prod.pcba.function.category.18", "ja-JP", "panel", "pcba功能类别.panel"),
            // dict.prod.pcba.function.category.18
            ("dict.prod.pcba.function.category.18", "zh-CN", "panel", "pcba功能类别.panel"),
            // dict.prod.pcba.function.category.18
            ("dict.prod.pcba.function.category.18", "zh-HK", "panel", "pcba功能类别.panel"),

            // dict.prod.pcba.function.category.19
            ("dict.prod.pcba.function.category.19", "en-US", "power", "pcba功能类别.power"),
            // dict.prod.pcba.function.category.19
            ("dict.prod.pcba.function.category.19", "ja-JP", "power", "pcba功能类别.power"),
            // dict.prod.pcba.function.category.19
            ("dict.prod.pcba.function.category.19", "zh-CN", "power", "pcba功能类别.power"),
            // dict.prod.pcba.function.category.19
            ("dict.prod.pcba.function.category.19", "zh-HK", "power", "pcba功能类别.power"),

            // dict.prod.pcba.function.category.20
            ("dict.prod.pcba.function.category.20", "en-US", "rear", "pcba功能类别.rear"),
            // dict.prod.pcba.function.category.20
            ("dict.prod.pcba.function.category.20", "ja-JP", "rear", "pcba功能类别.rear"),
            // dict.prod.pcba.function.category.20
            ("dict.prod.pcba.function.category.20", "zh-CN", "rear", "pcba功能类别.rear"),
            // dict.prod.pcba.function.category.20
            ("dict.prod.pcba.function.category.20", "zh-HK", "rear", "pcba功能类别.rear"),

            // dict.prod.pcba.function.category.21
            ("dict.prod.pcba.function.category.21", "en-US", "rmn-1", "pcba功能类别.rmn-1"),
            // dict.prod.pcba.function.category.21
            ("dict.prod.pcba.function.category.21", "ja-JP", "rmn-1", "pcba功能类别.rmn-1"),
            // dict.prod.pcba.function.category.21
            ("dict.prod.pcba.function.category.21", "zh-CN", "rmn-1", "pcba功能类别.rmn-1"),
            // dict.prod.pcba.function.category.21
            ("dict.prod.pcba.function.category.21", "zh-HK", "rmn-1", "pcba功能类别.rmn-1"),

            // dict.prod.pcba.function.category.22
            ("dict.prod.pcba.function.category.22", "en-US", "sata", "pcba功能类别.sata"),
            // dict.prod.pcba.function.category.22
            ("dict.prod.pcba.function.category.22", "ja-JP", "sata", "pcba功能类别.sata"),
            // dict.prod.pcba.function.category.22
            ("dict.prod.pcba.function.category.22", "zh-CN", "sata", "pcba功能类别.sata"),
            // dict.prod.pcba.function.category.22
            ("dict.prod.pcba.function.category.22", "zh-HK", "sata", "pcba功能类别.sata"),

            // dict.prod.pcba.function.category.23
            ("dict.prod.pcba.function.category.23", "en-US", "seq", "pcba功能类别.seq"),
            // dict.prod.pcba.function.category.23
            ("dict.prod.pcba.function.category.23", "ja-JP", "seq", "pcba功能类别.seq"),
            // dict.prod.pcba.function.category.23
            ("dict.prod.pcba.function.category.23", "zh-CN", "seq", "pcba功能类别.seq"),
            // dict.prod.pcba.function.category.23
            ("dict.prod.pcba.function.category.23", "zh-HK", "seq", "pcba功能类别.seq"),

            // dict.prod.pcba.function.category.24
            ("dict.prod.pcba.function.category.24", "en-US", "sys", "pcba功能类别.sys"),
            // dict.prod.pcba.function.category.24
            ("dict.prod.pcba.function.category.24", "ja-JP", "sys", "pcba功能类别.sys"),
            // dict.prod.pcba.function.category.24
            ("dict.prod.pcba.function.category.24", "zh-CN", "sys", "pcba功能类别.sys"),
            // dict.prod.pcba.function.category.24
            ("dict.prod.pcba.function.category.24", "zh-HK", "sys", "pcba功能类别.sys"),

            // dict.prod.pcba.function.category.25
            ("dict.prod.pcba.function.category.25", "en-US", "top", "pcba功能类别.top"),
            // dict.prod.pcba.function.category.25
            ("dict.prod.pcba.function.category.25", "ja-JP", "top", "pcba功能类别.top"),
            // dict.prod.pcba.function.category.25
            ("dict.prod.pcba.function.category.25", "zh-CN", "top", "pcba功能类别.top"),
            // dict.prod.pcba.function.category.25
            ("dict.prod.pcba.function.category.25", "zh-HK", "top", "pcba功能类别.top"),

            // dict.prod.pcba.function.category.26
            ("dict.prod.pcba.function.category.26", "en-US", "usb", "pcba功能类别.usb"),
            // dict.prod.pcba.function.category.26
            ("dict.prod.pcba.function.category.26", "ja-JP", "usb", "pcba功能类别.usb"),
            // dict.prod.pcba.function.category.26
            ("dict.prod.pcba.function.category.26", "zh-CN", "usb", "pcba功能类别.usb"),
            // dict.prod.pcba.function.category.26
            ("dict.prod.pcba.function.category.26", "zh-HK", "usb", "pcba功能类别.usb"),

            // dict.prod.pcba.panel.category.1
            ("dict.prod.pcba.panel.category.1", "en-US", "a2io", "pcba板位类别.a2io"),
            // dict.prod.pcba.panel.category.1
            ("dict.prod.pcba.panel.category.1", "ja-JP", "a2io", "pcba板位类别.a2io"),
            // dict.prod.pcba.panel.category.1
            ("dict.prod.pcba.panel.category.1", "zh-CN", "a2io", "pcba板位类别.a2io"),
            // dict.prod.pcba.panel.category.1
            ("dict.prod.pcba.panel.category.1", "zh-HK", "a2io", "pcba板位类别.a2io"),

            // dict.prod.pcba.panel.category.2
            ("dict.prod.pcba.panel.category.2", "en-US", "a2io b", "pcba板位类别.a2io b"),
            // dict.prod.pcba.panel.category.2
            ("dict.prod.pcba.panel.category.2", "ja-JP", "a2io b", "pcba板位类别.a2io b"),
            // dict.prod.pcba.panel.category.2
            ("dict.prod.pcba.panel.category.2", "zh-CN", "a2io b", "pcba板位类别.a2io b"),
            // dict.prod.pcba.panel.category.2
            ("dict.prod.pcba.panel.category.2", "zh-HK", "a2io b", "pcba板位类别.a2io b"),

            // dict.prod.pcba.panel.category.3
            ("dict.prod.pcba.panel.category.3", "en-US", "a2io t", "pcba板位类别.a2io t"),
            // dict.prod.pcba.panel.category.3
            ("dict.prod.pcba.panel.category.3", "ja-JP", "a2io t", "pcba板位类别.a2io t"),
            // dict.prod.pcba.panel.category.3
            ("dict.prod.pcba.panel.category.3", "zh-CN", "a2io t", "pcba板位类别.a2io t"),
            // dict.prod.pcba.panel.category.3
            ("dict.prod.pcba.panel.category.3", "zh-HK", "a2io t", "pcba板位类别.a2io t"),

            // dict.prod.pcba.panel.category.4
            ("dict.prod.pcba.panel.category.4", "en-US", "a4in b", "pcba板位类别.a4in b"),
            // dict.prod.pcba.panel.category.4
            ("dict.prod.pcba.panel.category.4", "ja-JP", "a4in b", "pcba板位类别.a4in b"),
            // dict.prod.pcba.panel.category.4
            ("dict.prod.pcba.panel.category.4", "zh-CN", "a4in b", "pcba板位类别.a4in b"),
            // dict.prod.pcba.panel.category.4
            ("dict.prod.pcba.panel.category.4", "zh-HK", "a4in b", "pcba板位类别.a4in b"),

            // dict.prod.pcba.panel.category.5
            ("dict.prod.pcba.panel.category.5", "en-US", "a4in t", "pcba板位类别.a4in t"),
            // dict.prod.pcba.panel.category.5
            ("dict.prod.pcba.panel.category.5", "ja-JP", "a4in t", "pcba板位类别.a4in t"),
            // dict.prod.pcba.panel.category.5
            ("dict.prod.pcba.panel.category.5", "zh-CN", "a4in t", "pcba板位类别.a4in t"),
            // dict.prod.pcba.panel.category.5
            ("dict.prod.pcba.panel.category.5", "zh-HK", "a4in t", "pcba板位类别.a4in t"),

            // dict.prod.pcba.panel.category.6
            ("dict.prod.pcba.panel.category.6", "en-US", "a4out b", "pcba板位类别.a4out b"),
            // dict.prod.pcba.panel.category.6
            ("dict.prod.pcba.panel.category.6", "ja-JP", "a4out b", "pcba板位类别.a4out b"),
            // dict.prod.pcba.panel.category.6
            ("dict.prod.pcba.panel.category.6", "zh-CN", "a4out b", "pcba板位类别.a4out b"),
            // dict.prod.pcba.panel.category.6
            ("dict.prod.pcba.panel.category.6", "zh-HK", "a4out b", "pcba板位类别.a4out b"),

            // dict.prod.pcba.panel.category.7
            ("dict.prod.pcba.panel.category.7", "en-US", "a4out t", "pcba板位类别.a4out t"),
            // dict.prod.pcba.panel.category.7
            ("dict.prod.pcba.panel.category.7", "ja-JP", "a4out t", "pcba板位类别.a4out t"),
            // dict.prod.pcba.panel.category.7
            ("dict.prod.pcba.panel.category.7", "zh-CN", "a4out t", "pcba板位类别.a4out t"),
            // dict.prod.pcba.panel.category.7
            ("dict.prod.pcba.panel.category.7", "zh-HK", "a4out t", "pcba板位类别.a4out t"),

            // dict.prod.pcba.panel.category.8
            ("dict.prod.pcba.panel.category.8", "en-US", "ad04 t", "pcba板位类别.ad04 t"),
            // dict.prod.pcba.panel.category.8
            ("dict.prod.pcba.panel.category.8", "ja-JP", "ad04 t", "pcba板位类别.ad04 t"),
            // dict.prod.pcba.panel.category.8
            ("dict.prod.pcba.panel.category.8", "zh-CN", "ad04 t", "pcba板位类别.ad04 t"),
            // dict.prod.pcba.panel.category.8
            ("dict.prod.pcba.panel.category.8", "zh-HK", "ad04 t", "pcba板位类别.ad04 t"),

            // dict.prod.pcba.panel.category.9
            ("dict.prod.pcba.panel.category.9", "en-US", "adda b", "pcba板位类别.adda b"),
            // dict.prod.pcba.panel.category.9
            ("dict.prod.pcba.panel.category.9", "ja-JP", "adda b", "pcba板位类别.adda b"),
            // dict.prod.pcba.panel.category.9
            ("dict.prod.pcba.panel.category.9", "zh-CN", "adda b", "pcba板位类别.adda b"),
            // dict.prod.pcba.panel.category.9
            ("dict.prod.pcba.panel.category.9", "zh-HK", "adda b", "pcba板位类别.adda b"),

            // dict.prod.pcba.panel.category.10
            ("dict.prod.pcba.panel.category.10", "en-US", "adda b/t", "pcba板位类别.adda b/t"),
            // dict.prod.pcba.panel.category.10
            ("dict.prod.pcba.panel.category.10", "ja-JP", "adda b/t", "pcba板位类别.adda b/t"),
            // dict.prod.pcba.panel.category.10
            ("dict.prod.pcba.panel.category.10", "zh-CN", "adda b/t", "pcba板位类别.adda b/t"),
            // dict.prod.pcba.panel.category.10
            ("dict.prod.pcba.panel.category.10", "zh-HK", "adda b/t", "pcba板位类别.adda b/t"),

            // dict.prod.pcba.panel.category.11
            ("dict.prod.pcba.panel.category.11", "en-US", "adda t", "pcba板位类别.adda t"),
            // dict.prod.pcba.panel.category.11
            ("dict.prod.pcba.panel.category.11", "ja-JP", "adda t", "pcba板位类别.adda t"),
            // dict.prod.pcba.panel.category.11
            ("dict.prod.pcba.panel.category.11", "zh-CN", "adda t", "pcba板位类别.adda t"),
            // dict.prod.pcba.panel.category.11
            ("dict.prod.pcba.panel.category.11", "zh-HK", "adda t", "pcba板位类别.adda t"),

            // dict.prod.pcba.panel.category.12
            ("dict.prod.pcba.panel.category.12", "en-US", "adoc", "pcba板位类别.adoc"),
            // dict.prod.pcba.panel.category.12
            ("dict.prod.pcba.panel.category.12", "ja-JP", "adoc", "pcba板位类别.adoc"),
            // dict.prod.pcba.panel.category.12
            ("dict.prod.pcba.panel.category.12", "zh-CN", "adoc", "pcba板位类别.adoc"),
            // dict.prod.pcba.panel.category.12
            ("dict.prod.pcba.panel.category.12", "zh-HK", "adoc", "pcba板位类别.adoc"),

            // dict.prod.pcba.panel.category.13
            ("dict.prod.pcba.panel.category.13", "en-US", "adoc b", "pcba板位类别.adoc b"),
            // dict.prod.pcba.panel.category.13
            ("dict.prod.pcba.panel.category.13", "ja-JP", "adoc b", "pcba板位类别.adoc b"),
            // dict.prod.pcba.panel.category.13
            ("dict.prod.pcba.panel.category.13", "zh-CN", "adoc b", "pcba板位类别.adoc b"),
            // dict.prod.pcba.panel.category.13
            ("dict.prod.pcba.panel.category.13", "zh-HK", "adoc b", "pcba板位类别.adoc b"),

            // dict.prod.pcba.panel.category.14
            ("dict.prod.pcba.panel.category.14", "en-US", "adoc b/t", "pcba板位类别.adoc b/t"),
            // dict.prod.pcba.panel.category.14
            ("dict.prod.pcba.panel.category.14", "ja-JP", "adoc b/t", "pcba板位类别.adoc b/t"),
            // dict.prod.pcba.panel.category.14
            ("dict.prod.pcba.panel.category.14", "zh-CN", "adoc b/t", "pcba板位类别.adoc b/t"),
            // dict.prod.pcba.panel.category.14
            ("dict.prod.pcba.panel.category.14", "zh-HK", "adoc b/t", "pcba板位类别.adoc b/t"),

            // dict.prod.pcba.panel.category.15
            ("dict.prod.pcba.panel.category.15", "en-US", "adoc t", "pcba板位类别.adoc t"),
            // dict.prod.pcba.panel.category.15
            ("dict.prod.pcba.panel.category.15", "ja-JP", "adoc t", "pcba板位类别.adoc t"),
            // dict.prod.pcba.panel.category.15
            ("dict.prod.pcba.panel.category.15", "zh-CN", "adoc t", "pcba板位类别.adoc t"),
            // dict.prod.pcba.panel.category.15
            ("dict.prod.pcba.panel.category.15", "zh-HK", "adoc t", "pcba板位类别.adoc t"),

            // dict.prod.pcba.panel.category.16
            ("dict.prod.pcba.panel.category.16", "en-US", "aes4 b", "pcba板位类别.aes4 b"),
            // dict.prod.pcba.panel.category.16
            ("dict.prod.pcba.panel.category.16", "ja-JP", "aes4 b", "pcba板位类别.aes4 b"),
            // dict.prod.pcba.panel.category.16
            ("dict.prod.pcba.panel.category.16", "zh-CN", "aes4 b", "pcba板位类别.aes4 b"),
            // dict.prod.pcba.panel.category.16
            ("dict.prod.pcba.panel.category.16", "zh-HK", "aes4 b", "pcba板位类别.aes4 b"),

            // dict.prod.pcba.panel.category.17
            ("dict.prod.pcba.panel.category.17", "en-US", "aes4 b/t", "pcba板位类别.aes4 b/t"),
            // dict.prod.pcba.panel.category.17
            ("dict.prod.pcba.panel.category.17", "ja-JP", "aes4 b/t", "pcba板位类别.aes4 b/t"),
            // dict.prod.pcba.panel.category.17
            ("dict.prod.pcba.panel.category.17", "zh-CN", "aes4 b/t", "pcba板位类别.aes4 b/t"),
            // dict.prod.pcba.panel.category.17
            ("dict.prod.pcba.panel.category.17", "zh-HK", "aes4 b/t", "pcba板位类别.aes4 b/t"),

            // dict.prod.pcba.panel.category.18
            ("dict.prod.pcba.panel.category.18", "en-US", "aes4 t", "pcba板位类别.aes4 t"),
            // dict.prod.pcba.panel.category.18
            ("dict.prod.pcba.panel.category.18", "ja-JP", "aes4 t", "pcba板位类别.aes4 t"),
            // dict.prod.pcba.panel.category.18
            ("dict.prod.pcba.panel.category.18", "zh-CN", "aes4 t", "pcba板位类别.aes4 t"),
            // dict.prod.pcba.panel.category.18
            ("dict.prod.pcba.panel.category.18", "zh-HK", "aes4 t", "pcba板位类别.aes4 t"),

            // dict.prod.pcba.panel.category.19
            ("dict.prod.pcba.panel.category.19", "en-US", "ana", "pcba板位类别.ana"),
            // dict.prod.pcba.panel.category.19
            ("dict.prod.pcba.panel.category.19", "ja-JP", "ana", "pcba板位类别.ana"),
            // dict.prod.pcba.panel.category.19
            ("dict.prod.pcba.panel.category.19", "zh-CN", "ana", "pcba板位类别.ana"),
            // dict.prod.pcba.panel.category.19
            ("dict.prod.pcba.panel.category.19", "zh-HK", "ana", "pcba板位类别.ana"),

            // dict.prod.pcba.panel.category.24
            ("dict.prod.pcba.panel.category.24", "en-US", "ana a", "pcba板位类别.ana a"),
            // dict.prod.pcba.panel.category.24
            ("dict.prod.pcba.panel.category.24", "ja-JP", "ana a", "pcba板位类别.ana a"),
            // dict.prod.pcba.panel.category.24
            ("dict.prod.pcba.panel.category.24", "zh-CN", "ana a", "pcba板位类别.ana a"),
            // dict.prod.pcba.panel.category.24
            ("dict.prod.pcba.panel.category.24", "zh-HK", "ana a", "pcba板位类别.ana a"),

            // dict.prod.pcba.panel.category.25
            ("dict.prod.pcba.panel.category.25", "en-US", "ana b", "pcba板位类别.ana b"),
            // dict.prod.pcba.panel.category.25
            ("dict.prod.pcba.panel.category.25", "ja-JP", "ana b", "pcba板位类别.ana b"),
            // dict.prod.pcba.panel.category.25
            ("dict.prod.pcba.panel.category.25", "zh-CN", "ana b", "pcba板位类别.ana b"),
            // dict.prod.pcba.panel.category.25
            ("dict.prod.pcba.panel.category.25", "zh-HK", "ana b", "pcba板位类别.ana b"),

            // dict.prod.pcba.panel.category.26
            ("dict.prod.pcba.panel.category.26", "en-US", "ana b/t", "pcba板位类别.ana b/t"),
            // dict.prod.pcba.panel.category.26
            ("dict.prod.pcba.panel.category.26", "ja-JP", "ana b/t", "pcba板位类别.ana b/t"),
            // dict.prod.pcba.panel.category.26
            ("dict.prod.pcba.panel.category.26", "zh-CN", "ana b/t", "pcba板位类别.ana b/t"),
            // dict.prod.pcba.panel.category.26
            ("dict.prod.pcba.panel.category.26", "zh-HK", "ana b/t", "pcba板位类别.ana b/t"),

            // dict.prod.pcba.panel.category.27
            ("dict.prod.pcba.panel.category.27", "en-US", "ana t", "pcba板位类别.ana t"),
            // dict.prod.pcba.panel.category.27
            ("dict.prod.pcba.panel.category.27", "ja-JP", "ana t", "pcba板位类别.ana t"),
            // dict.prod.pcba.panel.category.27
            ("dict.prod.pcba.panel.category.27", "zh-CN", "ana t", "pcba板位类别.ana t"),
            // dict.prod.pcba.panel.category.27
            ("dict.prod.pcba.panel.category.27", "zh-HK", "ana t", "pcba板位类别.ana t"),

            // dict.prod.pcba.panel.category.28
            ("dict.prod.pcba.panel.category.28", "en-US", "apnel t", "pcba板位类别.apnel t"),
            // dict.prod.pcba.panel.category.28
            ("dict.prod.pcba.panel.category.28", "ja-JP", "apnel t", "pcba板位类别.apnel t"),
            // dict.prod.pcba.panel.category.28
            ("dict.prod.pcba.panel.category.28", "zh-CN", "apnel t", "pcba板位类别.apnel t"),
            // dict.prod.pcba.panel.category.28
            ("dict.prod.pcba.panel.category.28", "zh-HK", "apnel t", "pcba板位类别.apnel t"),

            // dict.prod.pcba.panel.category.29
            ("dict.prod.pcba.panel.category.29", "en-US", "audio", "pcba板位类别.audio"),
            // dict.prod.pcba.panel.category.29
            ("dict.prod.pcba.panel.category.29", "ja-JP", "audio", "pcba板位类别.audio"),
            // dict.prod.pcba.panel.category.29
            ("dict.prod.pcba.panel.category.29", "zh-CN", "audio", "pcba板位类别.audio"),
            // dict.prod.pcba.panel.category.29
            ("dict.prod.pcba.panel.category.29", "zh-HK", "audio", "pcba板位类别.audio"),

            // dict.prod.pcba.panel.category.30
            ("dict.prod.pcba.panel.category.30", "en-US", "audio a", "pcba板位类别.audio a"),
            // dict.prod.pcba.panel.category.30
            ("dict.prod.pcba.panel.category.30", "ja-JP", "audio a", "pcba板位类别.audio a"),
            // dict.prod.pcba.panel.category.30
            ("dict.prod.pcba.panel.category.30", "zh-CN", "audio a", "pcba板位类别.audio a"),
            // dict.prod.pcba.panel.category.30
            ("dict.prod.pcba.panel.category.30", "zh-HK", "audio a", "pcba板位类别.audio a"),

            // dict.prod.pcba.panel.category.31
            ("dict.prod.pcba.panel.category.31", "en-US", "audio alt b", "pcba板位类别.audio alt b"),
            // dict.prod.pcba.panel.category.31
            ("dict.prod.pcba.panel.category.31", "ja-JP", "audio alt b", "pcba板位类别.audio alt b"),
            // dict.prod.pcba.panel.category.31
            ("dict.prod.pcba.panel.category.31", "zh-CN", "audio alt b", "pcba板位类别.audio alt b"),
            // dict.prod.pcba.panel.category.31
            ("dict.prod.pcba.panel.category.31", "zh-HK", "audio alt b", "pcba板位类别.audio alt b"),

            // dict.prod.pcba.panel.category.32
            ("dict.prod.pcba.panel.category.32", "en-US", "audio alt t", "pcba板位类别.audio alt t"),
            // dict.prod.pcba.panel.category.32
            ("dict.prod.pcba.panel.category.32", "ja-JP", "audio alt t", "pcba板位类别.audio alt t"),
            // dict.prod.pcba.panel.category.32
            ("dict.prod.pcba.panel.category.32", "zh-CN", "audio alt t", "pcba板位类别.audio alt t"),
            // dict.prod.pcba.panel.category.32
            ("dict.prod.pcba.panel.category.32", "zh-HK", "audio alt t", "pcba板位类别.audio alt t"),

            // dict.prod.pcba.panel.category.33
            ("dict.prod.pcba.panel.category.33", "en-US", "audio b", "pcba板位类别.audio b"),
            // dict.prod.pcba.panel.category.33
            ("dict.prod.pcba.panel.category.33", "ja-JP", "audio b", "pcba板位类别.audio b"),
            // dict.prod.pcba.panel.category.33
            ("dict.prod.pcba.panel.category.33", "zh-CN", "audio b", "pcba板位类别.audio b"),
            // dict.prod.pcba.panel.category.33
            ("dict.prod.pcba.panel.category.33", "zh-HK", "audio b", "pcba板位类别.audio b"),

            // dict.prod.pcba.panel.category.34
            ("dict.prod.pcba.panel.category.34", "en-US", "audio b/t", "pcba板位类别.audio b/t"),
            // dict.prod.pcba.panel.category.34
            ("dict.prod.pcba.panel.category.34", "ja-JP", "audio b/t", "pcba板位类别.audio b/t"),
            // dict.prod.pcba.panel.category.34
            ("dict.prod.pcba.panel.category.34", "zh-CN", "audio b/t", "pcba板位类别.audio b/t"),
            // dict.prod.pcba.panel.category.34
            ("dict.prod.pcba.panel.category.34", "zh-HK", "audio b/t", "pcba板位类别.audio b/t"),

            // dict.prod.pcba.panel.category.35
            ("dict.prod.pcba.panel.category.35", "en-US", "audio t", "pcba板位类别.audio t"),
            // dict.prod.pcba.panel.category.35
            ("dict.prod.pcba.panel.category.35", "ja-JP", "audio t", "pcba板位类别.audio t"),
            // dict.prod.pcba.panel.category.35
            ("dict.prod.pcba.panel.category.35", "zh-CN", "audio t", "pcba板位类别.audio t"),
            // dict.prod.pcba.panel.category.35
            ("dict.prod.pcba.panel.category.35", "zh-HK", "audio t", "pcba板位类别.audio t"),

            // dict.prod.pcba.panel.category.36
            ("dict.prod.pcba.panel.category.36", "en-US", "audio-00-b", "pcba板位类别.audio-00-b"),
            // dict.prod.pcba.panel.category.36
            ("dict.prod.pcba.panel.category.36", "ja-JP", "audio-00-b", "pcba板位类别.audio-00-b"),
            // dict.prod.pcba.panel.category.36
            ("dict.prod.pcba.panel.category.36", "zh-CN", "audio-00-b", "pcba板位类别.audio-00-b"),
            // dict.prod.pcba.panel.category.36
            ("dict.prod.pcba.panel.category.36", "zh-HK", "audio-00-b", "pcba板位类别.audio-00-b"),

            // dict.prod.pcba.panel.category.37
            ("dict.prod.pcba.panel.category.37", "en-US", "audio-00-t", "pcba板位类别.audio-00-t"),
            // dict.prod.pcba.panel.category.37
            ("dict.prod.pcba.panel.category.37", "ja-JP", "audio-00-t", "pcba板位类别.audio-00-t"),
            // dict.prod.pcba.panel.category.37
            ("dict.prod.pcba.panel.category.37", "zh-CN", "audio-00-t", "pcba板位类别.audio-00-t"),
            // dict.prod.pcba.panel.category.37
            ("dict.prod.pcba.panel.category.37", "zh-HK", "audio-00-t", "pcba板位类别.audio-00-t"),

            // dict.prod.pcba.panel.category.38
            ("dict.prod.pcba.panel.category.38", "en-US", "audio-10-b", "pcba板位类别.audio-10-b"),
            // dict.prod.pcba.panel.category.38
            ("dict.prod.pcba.panel.category.38", "ja-JP", "audio-10-b", "pcba板位类别.audio-10-b"),
            // dict.prod.pcba.panel.category.38
            ("dict.prod.pcba.panel.category.38", "zh-CN", "audio-10-b", "pcba板位类别.audio-10-b"),
            // dict.prod.pcba.panel.category.38
            ("dict.prod.pcba.panel.category.38", "zh-HK", "audio-10-b", "pcba板位类别.audio-10-b"),

            // dict.prod.pcba.panel.category.39
            ("dict.prod.pcba.panel.category.39", "en-US", "audio-10-t", "pcba板位类别.audio-10-t"),
            // dict.prod.pcba.panel.category.39
            ("dict.prod.pcba.panel.category.39", "ja-JP", "audio-10-t", "pcba板位类别.audio-10-t"),
            // dict.prod.pcba.panel.category.39
            ("dict.prod.pcba.panel.category.39", "zh-CN", "audio-10-t", "pcba板位类别.audio-10-t"),
            // dict.prod.pcba.panel.category.39
            ("dict.prod.pcba.panel.category.39", "zh-HK", "audio-10-t", "pcba板位类别.audio-10-t"),

            // dict.prod.pcba.panel.category.40
            ("dict.prod.pcba.panel.category.40", "en-US", "audio-20-b", "pcba板位类别.audio-20-b"),
            // dict.prod.pcba.panel.category.40
            ("dict.prod.pcba.panel.category.40", "ja-JP", "audio-20-b", "pcba板位类别.audio-20-b"),
            // dict.prod.pcba.panel.category.40
            ("dict.prod.pcba.panel.category.40", "zh-CN", "audio-20-b", "pcba板位类别.audio-20-b"),
            // dict.prod.pcba.panel.category.40
            ("dict.prod.pcba.panel.category.40", "zh-HK", "audio-20-b", "pcba板位类别.audio-20-b"),

            // dict.prod.pcba.panel.category.41
            ("dict.prod.pcba.panel.category.41", "en-US", "audio-20-t", "pcba板位类别.audio-20-t"),
            // dict.prod.pcba.panel.category.41
            ("dict.prod.pcba.panel.category.41", "ja-JP", "audio-20-t", "pcba板位类别.audio-20-t"),
            // dict.prod.pcba.panel.category.41
            ("dict.prod.pcba.panel.category.41", "zh-CN", "audio-20-t", "pcba板位类别.audio-20-t"),
            // dict.prod.pcba.panel.category.41
            ("dict.prod.pcba.panel.category.41", "zh-HK", "audio-20-t", "pcba板位类别.audio-20-t"),

            // dict.prod.pcba.panel.category.42
            ("dict.prod.pcba.panel.category.42", "en-US", "bottom b", "pcba板位类别.bottom b"),
            // dict.prod.pcba.panel.category.42
            ("dict.prod.pcba.panel.category.42", "ja-JP", "bottom b", "pcba板位类别.bottom b"),
            // dict.prod.pcba.panel.category.42
            ("dict.prod.pcba.panel.category.42", "zh-CN", "bottom b", "pcba板位类别.bottom b"),
            // dict.prod.pcba.panel.category.42
            ("dict.prod.pcba.panel.category.42", "zh-HK", "bottom b", "pcba板位类别.bottom b"),

            // dict.prod.pcba.panel.category.43
            ("dict.prod.pcba.panel.category.43", "en-US", "ccl b", "pcba板位类别.ccl b"),
            // dict.prod.pcba.panel.category.43
            ("dict.prod.pcba.panel.category.43", "ja-JP", "ccl b", "pcba板位类别.ccl b"),
            // dict.prod.pcba.panel.category.43
            ("dict.prod.pcba.panel.category.43", "zh-CN", "ccl b", "pcba板位类别.ccl b"),
            // dict.prod.pcba.panel.category.43
            ("dict.prod.pcba.panel.category.43", "zh-HK", "ccl b", "pcba板位类别.ccl b"),

            // dict.prod.pcba.panel.category.44
            ("dict.prod.pcba.panel.category.44", "en-US", "ccl b/t", "pcba板位类别.ccl b/t"),
            // dict.prod.pcba.panel.category.44
            ("dict.prod.pcba.panel.category.44", "ja-JP", "ccl b/t", "pcba板位类别.ccl b/t"),
            // dict.prod.pcba.panel.category.44
            ("dict.prod.pcba.panel.category.44", "zh-CN", "ccl b/t", "pcba板位类别.ccl b/t"),
            // dict.prod.pcba.panel.category.44
            ("dict.prod.pcba.panel.category.44", "zh-HK", "ccl b/t", "pcba板位类别.ccl b/t"),

            // dict.prod.pcba.panel.category.45
            ("dict.prod.pcba.panel.category.45", "en-US", "ccl t", "pcba板位类别.ccl t"),
            // dict.prod.pcba.panel.category.45
            ("dict.prod.pcba.panel.category.45", "ja-JP", "ccl t", "pcba板位类别.ccl t"),
            // dict.prod.pcba.panel.category.45
            ("dict.prod.pcba.panel.category.45", "zh-CN", "ccl t", "pcba板位类别.ccl t"),
            // dict.prod.pcba.panel.category.45
            ("dict.prod.pcba.panel.category.45", "zh-HK", "ccl t", "pcba板位类别.ccl t"),

            // dict.prod.pcba.panel.category.46
            ("dict.prod.pcba.panel.category.46", "en-US", "cd b", "pcba板位类别.cd b"),
            // dict.prod.pcba.panel.category.46
            ("dict.prod.pcba.panel.category.46", "ja-JP", "cd b", "pcba板位类别.cd b"),
            // dict.prod.pcba.panel.category.46
            ("dict.prod.pcba.panel.category.46", "zh-CN", "cd b", "pcba板位类别.cd b"),
            // dict.prod.pcba.panel.category.46
            ("dict.prod.pcba.panel.category.46", "zh-HK", "cd b", "pcba板位类别.cd b"),

            // dict.prod.pcba.panel.category.47
            ("dict.prod.pcba.panel.category.47", "en-US", "cd t", "pcba板位类别.cd t"),
            // dict.prod.pcba.panel.category.47
            ("dict.prod.pcba.panel.category.47", "ja-JP", "cd t", "pcba板位类别.cd t"),
            // dict.prod.pcba.panel.category.47
            ("dict.prod.pcba.panel.category.47", "zh-CN", "cd t", "pcba板位类别.cd t"),
            // dict.prod.pcba.panel.category.47
            ("dict.prod.pcba.panel.category.47", "zh-HK", "cd t", "pcba板位类别.cd t"),

            // dict.prod.pcba.panel.category.48
            ("dict.prod.pcba.panel.category.48", "en-US", "cd-main", "pcba板位类别.cd-main"),
            // dict.prod.pcba.panel.category.48
            ("dict.prod.pcba.panel.category.48", "ja-JP", "cd-main", "pcba板位类别.cd-main"),
            // dict.prod.pcba.panel.category.48
            ("dict.prod.pcba.panel.category.48", "zh-CN", "cd-main", "pcba板位类别.cd-main"),
            // dict.prod.pcba.panel.category.48
            ("dict.prod.pcba.panel.category.48", "zh-HK", "cd-main", "pcba板位类别.cd-main"),

            // dict.prod.pcba.panel.category.49
            ("dict.prod.pcba.panel.category.49", "en-US", "cd-main b", "pcba板位类别.cd-main b"),
            // dict.prod.pcba.panel.category.49
            ("dict.prod.pcba.panel.category.49", "ja-JP", "cd-main b", "pcba板位类别.cd-main b"),
            // dict.prod.pcba.panel.category.49
            ("dict.prod.pcba.panel.category.49", "zh-CN", "cd-main b", "pcba板位类别.cd-main b"),
            // dict.prod.pcba.panel.category.49
            ("dict.prod.pcba.panel.category.49", "zh-HK", "cd-main b", "pcba板位类别.cd-main b"),

            // dict.prod.pcba.panel.category.50
            ("dict.prod.pcba.panel.category.50", "en-US", "cdmcu", "pcba板位类别.cdmcu"),
            // dict.prod.pcba.panel.category.50
            ("dict.prod.pcba.panel.category.50", "ja-JP", "cdmcu", "pcba板位类别.cdmcu"),
            // dict.prod.pcba.panel.category.50
            ("dict.prod.pcba.panel.category.50", "zh-CN", "cdmcu", "pcba板位类别.cdmcu"),
            // dict.prod.pcba.panel.category.50
            ("dict.prod.pcba.panel.category.50", "zh-HK", "cdmcu", "pcba板位类别.cdmcu"),

            // dict.prod.pcba.panel.category.51
            ("dict.prod.pcba.panel.category.51", "en-US", "cdmcu b", "pcba板位类别.cdmcu b"),
            // dict.prod.pcba.panel.category.51
            ("dict.prod.pcba.panel.category.51", "ja-JP", "cdmcu b", "pcba板位类别.cdmcu b"),
            // dict.prod.pcba.panel.category.51
            ("dict.prod.pcba.panel.category.51", "zh-CN", "cdmcu b", "pcba板位类别.cdmcu b"),
            // dict.prod.pcba.panel.category.51
            ("dict.prod.pcba.panel.category.51", "zh-HK", "cdmcu b", "pcba板位类别.cdmcu b"),

            // dict.prod.pcba.panel.category.52
            ("dict.prod.pcba.panel.category.52", "en-US", "cdmcu b/t", "pcba板位类别.cdmcu b/t"),
            // dict.prod.pcba.panel.category.52
            ("dict.prod.pcba.panel.category.52", "ja-JP", "cdmcu b/t", "pcba板位类别.cdmcu b/t"),
            // dict.prod.pcba.panel.category.52
            ("dict.prod.pcba.panel.category.52", "zh-CN", "cdmcu b/t", "pcba板位类别.cdmcu b/t"),
            // dict.prod.pcba.panel.category.52
            ("dict.prod.pcba.panel.category.52", "zh-HK", "cdmcu b/t", "pcba板位类别.cdmcu b/t"),

            // dict.prod.pcba.panel.category.53
            ("dict.prod.pcba.panel.category.53", "en-US", "cdmcu t", "pcba板位类别.cdmcu t"),
            // dict.prod.pcba.panel.category.53
            ("dict.prod.pcba.panel.category.53", "ja-JP", "cdmcu t", "pcba板位类别.cdmcu t"),
            // dict.prod.pcba.panel.category.53
            ("dict.prod.pcba.panel.category.53", "zh-CN", "cdmcu t", "pcba板位类别.cdmcu t"),
            // dict.prod.pcba.panel.category.53
            ("dict.prod.pcba.panel.category.53", "zh-HK", "cdmcu t", "pcba板位类别.cdmcu t"),

            // dict.prod.pcba.panel.category.54
            ("dict.prod.pcba.panel.category.54", "en-US", "comb b", "pcba板位类别.comb b"),
            // dict.prod.pcba.panel.category.54
            ("dict.prod.pcba.panel.category.54", "ja-JP", "comb b", "pcba板位类别.comb b"),
            // dict.prod.pcba.panel.category.54
            ("dict.prod.pcba.panel.category.54", "zh-CN", "comb b", "pcba板位类别.comb b"),
            // dict.prod.pcba.panel.category.54
            ("dict.prod.pcba.panel.category.54", "zh-HK", "comb b", "pcba板位类别.comb b"),

            // dict.prod.pcba.panel.category.55
            ("dict.prod.pcba.panel.category.55", "en-US", "comb t", "pcba板位类别.comb t"),
            // dict.prod.pcba.panel.category.55
            ("dict.prod.pcba.panel.category.55", "ja-JP", "comb t", "pcba板位类别.comb t"),
            // dict.prod.pcba.panel.category.55
            ("dict.prod.pcba.panel.category.55", "zh-CN", "comb t", "pcba板位类别.comb t"),
            // dict.prod.pcba.panel.category.55
            ("dict.prod.pcba.panel.category.55", "zh-HK", "comb t", "pcba板位类别.comb t"),

            // dict.prod.pcba.panel.category.56
            ("dict.prod.pcba.panel.category.56", "en-US", "combo b", "pcba板位类别.combo b"),
            // dict.prod.pcba.panel.category.56
            ("dict.prod.pcba.panel.category.56", "ja-JP", "combo b", "pcba板位类别.combo b"),
            // dict.prod.pcba.panel.category.56
            ("dict.prod.pcba.panel.category.56", "zh-CN", "combo b", "pcba板位类别.combo b"),
            // dict.prod.pcba.panel.category.56
            ("dict.prod.pcba.panel.category.56", "zh-HK", "combo b", "pcba板位类别.combo b"),

            // dict.prod.pcba.panel.category.57
            ("dict.prod.pcba.panel.category.57", "en-US", "combo t", "pcba板位类别.combo t"),
            // dict.prod.pcba.panel.category.57
            ("dict.prod.pcba.panel.category.57", "ja-JP", "combo t", "pcba板位类别.combo t"),
            // dict.prod.pcba.panel.category.57
            ("dict.prod.pcba.panel.category.57", "zh-CN", "combo t", "pcba板位类别.combo t"),
            // dict.prod.pcba.panel.category.57
            ("dict.prod.pcba.panel.category.57", "zh-HK", "combo t", "pcba板位类别.combo t"),

            // dict.prod.pcba.panel.category.58
            ("dict.prod.pcba.panel.category.58", "en-US", "conn", "pcba板位类别.conn"),
            // dict.prod.pcba.panel.category.58
            ("dict.prod.pcba.panel.category.58", "ja-JP", "conn", "pcba板位类别.conn"),
            // dict.prod.pcba.panel.category.58
            ("dict.prod.pcba.panel.category.58", "zh-CN", "conn", "pcba板位类别.conn"),
            // dict.prod.pcba.panel.category.58
            ("dict.prod.pcba.panel.category.58", "zh-HK", "conn", "pcba板位类别.conn"),

            // dict.prod.pcba.panel.category.59
            ("dict.prod.pcba.panel.category.59", "en-US", "conn a", "pcba板位类别.conn a"),
            // dict.prod.pcba.panel.category.59
            ("dict.prod.pcba.panel.category.59", "ja-JP", "conn a", "pcba板位类别.conn a"),
            // dict.prod.pcba.panel.category.59
            ("dict.prod.pcba.panel.category.59", "zh-CN", "conn a", "pcba板位类别.conn a"),
            // dict.prod.pcba.panel.category.59
            ("dict.prod.pcba.panel.category.59", "zh-HK", "conn a", "pcba板位类别.conn a"),

            // dict.prod.pcba.panel.category.60
            ("dict.prod.pcba.panel.category.60", "en-US", "conn b", "pcba板位类别.conn b"),
            // dict.prod.pcba.panel.category.60
            ("dict.prod.pcba.panel.category.60", "ja-JP", "conn b", "pcba板位类别.conn b"),
            // dict.prod.pcba.panel.category.60
            ("dict.prod.pcba.panel.category.60", "zh-CN", "conn b", "pcba板位类别.conn b"),
            // dict.prod.pcba.panel.category.60
            ("dict.prod.pcba.panel.category.60", "zh-HK", "conn b", "pcba板位类别.conn b"),

            // dict.prod.pcba.panel.category.61
            ("dict.prod.pcba.panel.category.61", "en-US", "conn b/t", "pcba板位类别.conn b/t"),
            // dict.prod.pcba.panel.category.61
            ("dict.prod.pcba.panel.category.61", "ja-JP", "conn b/t", "pcba板位类别.conn b/t"),
            // dict.prod.pcba.panel.category.61
            ("dict.prod.pcba.panel.category.61", "zh-CN", "conn b/t", "pcba板位类别.conn b/t"),
            // dict.prod.pcba.panel.category.61
            ("dict.prod.pcba.panel.category.61", "zh-HK", "conn b/t", "pcba板位类别.conn b/t"),

            // dict.prod.pcba.panel.category.62
            ("dict.prod.pcba.panel.category.62", "en-US", "conn t", "pcba板位类别.conn t"),
            // dict.prod.pcba.panel.category.62
            ("dict.prod.pcba.panel.category.62", "ja-JP", "conn t", "pcba板位类别.conn t"),
            // dict.prod.pcba.panel.category.62
            ("dict.prod.pcba.panel.category.62", "zh-CN", "conn t", "pcba板位类别.conn t"),
            // dict.prod.pcba.panel.category.62
            ("dict.prod.pcba.panel.category.62", "zh-HK", "conn t", "pcba板位类别.conn t"),

            // dict.prod.pcba.panel.category.63
            ("dict.prod.pcba.panel.category.63", "en-US", "contact", "pcba板位类别.contact"),
            // dict.prod.pcba.panel.category.63
            ("dict.prod.pcba.panel.category.63", "ja-JP", "contact", "pcba板位类别.contact"),
            // dict.prod.pcba.panel.category.63
            ("dict.prod.pcba.panel.category.63", "zh-CN", "contact", "pcba板位类别.contact"),
            // dict.prod.pcba.panel.category.63
            ("dict.prod.pcba.panel.category.63", "zh-HK", "contact", "pcba板位类别.contact"),

            // dict.prod.pcba.panel.category.64
            ("dict.prod.pcba.panel.category.64", "en-US", "da", "pcba板位类别.da"),
            // dict.prod.pcba.panel.category.64
            ("dict.prod.pcba.panel.category.64", "ja-JP", "da", "pcba板位类别.da"),
            // dict.prod.pcba.panel.category.64
            ("dict.prod.pcba.panel.category.64", "zh-CN", "da", "pcba板位类别.da"),
            // dict.prod.pcba.panel.category.64
            ("dict.prod.pcba.panel.category.64", "zh-HK", "da", "pcba板位类别.da"),

            // dict.prod.pcba.panel.category.65
            ("dict.prod.pcba.panel.category.65", "en-US", "da b", "pcba板位类别.da b"),
            // dict.prod.pcba.panel.category.65
            ("dict.prod.pcba.panel.category.65", "ja-JP", "da b", "pcba板位类别.da b"),
            // dict.prod.pcba.panel.category.65
            ("dict.prod.pcba.panel.category.65", "zh-CN", "da b", "pcba板位类别.da b"),
            // dict.prod.pcba.panel.category.65
            ("dict.prod.pcba.panel.category.65", "zh-HK", "da b", "pcba板位类别.da b"),

            // dict.prod.pcba.panel.category.66
            ("dict.prod.pcba.panel.category.66", "en-US", "da t", "pcba板位类别.da t"),
            // dict.prod.pcba.panel.category.66
            ("dict.prod.pcba.panel.category.66", "ja-JP", "da t", "pcba板位类别.da t"),
            // dict.prod.pcba.panel.category.66
            ("dict.prod.pcba.panel.category.66", "zh-CN", "da t", "pcba板位类别.da t"),
            // dict.prod.pcba.panel.category.66
            ("dict.prod.pcba.panel.category.66", "zh-HK", "da t", "pcba板位类别.da t"),

            // dict.prod.pcba.panel.category.67
            ("dict.prod.pcba.panel.category.67", "en-US", "da t/b", "pcba板位类别.da t/b"),
            // dict.prod.pcba.panel.category.67
            ("dict.prod.pcba.panel.category.67", "ja-JP", "da t/b", "pcba板位类别.da t/b"),
            // dict.prod.pcba.panel.category.67
            ("dict.prod.pcba.panel.category.67", "zh-CN", "da t/b", "pcba板位类别.da t/b"),
            // dict.prod.pcba.panel.category.67
            ("dict.prod.pcba.panel.category.67", "zh-HK", "da t/b", "pcba板位类别.da t/b"),

            // dict.prod.pcba.panel.category.68
            ("dict.prod.pcba.panel.category.68", "en-US", "dany b", "pcba板位类别.dany b"),
            // dict.prod.pcba.panel.category.68
            ("dict.prod.pcba.panel.category.68", "ja-JP", "dany b", "pcba板位类别.dany b"),
            // dict.prod.pcba.panel.category.68
            ("dict.prod.pcba.panel.category.68", "zh-CN", "dany b", "pcba板位类别.dany b"),
            // dict.prod.pcba.panel.category.68
            ("dict.prod.pcba.panel.category.68", "zh-HK", "dany b", "pcba板位类别.dany b"),

            // dict.prod.pcba.panel.category.70
            ("dict.prod.pcba.panel.category.70", "en-US", "dsp b", "pcba板位类别.dsp b"),
            // dict.prod.pcba.panel.category.70
            ("dict.prod.pcba.panel.category.70", "ja-JP", "dsp b", "pcba板位类别.dsp b"),
            // dict.prod.pcba.panel.category.70
            ("dict.prod.pcba.panel.category.70", "zh-CN", "dsp b", "pcba板位类别.dsp b"),
            // dict.prod.pcba.panel.category.70
            ("dict.prod.pcba.panel.category.70", "zh-HK", "dsp b", "pcba板位类别.dsp b"),

            // dict.prod.pcba.panel.category.71
            ("dict.prod.pcba.panel.category.71", "en-US", "dsp t", "pcba板位类别.dsp t"),
            // dict.prod.pcba.panel.category.71
            ("dict.prod.pcba.panel.category.71", "ja-JP", "dsp t", "pcba板位类别.dsp t"),
            // dict.prod.pcba.panel.category.71
            ("dict.prod.pcba.panel.category.71", "zh-CN", "dsp t", "pcba板位类别.dsp t"),
            // dict.prod.pcba.panel.category.71
            ("dict.prod.pcba.panel.category.71", "zh-HK", "dsp t", "pcba板位类别.dsp t"),

            // dict.prod.pcba.panel.category.72
            ("dict.prod.pcba.panel.category.72", "en-US", "dspl  t", "pcba板位类别.dspl  t"),
            // dict.prod.pcba.panel.category.72
            ("dict.prod.pcba.panel.category.72", "ja-JP", "dspl  t", "pcba板位类别.dspl  t"),
            // dict.prod.pcba.panel.category.72
            ("dict.prod.pcba.panel.category.72", "zh-CN", "dspl  t", "pcba板位类别.dspl  t"),
            // dict.prod.pcba.panel.category.72
            ("dict.prod.pcba.panel.category.72", "zh-HK", "dspl  t", "pcba板位类别.dspl  t"),

            // dict.prod.pcba.panel.category.73
            ("dict.prod.pcba.panel.category.73", "en-US", "dspl a", "pcba板位类别.dspl a"),
            // dict.prod.pcba.panel.category.73
            ("dict.prod.pcba.panel.category.73", "ja-JP", "dspl a", "pcba板位类别.dspl a"),
            // dict.prod.pcba.panel.category.73
            ("dict.prod.pcba.panel.category.73", "zh-CN", "dspl a", "pcba板位类别.dspl a"),
            // dict.prod.pcba.panel.category.73
            ("dict.prod.pcba.panel.category.73", "zh-HK", "dspl a", "pcba板位类别.dspl a"),

            // dict.prod.pcba.panel.category.74
            ("dict.prod.pcba.panel.category.74", "en-US", "dspl b", "pcba板位类别.dspl b"),
            // dict.prod.pcba.panel.category.74
            ("dict.prod.pcba.panel.category.74", "ja-JP", "dspl b", "pcba板位类别.dspl b"),
            // dict.prod.pcba.panel.category.74
            ("dict.prod.pcba.panel.category.74", "zh-CN", "dspl b", "pcba板位类别.dspl b"),
            // dict.prod.pcba.panel.category.74
            ("dict.prod.pcba.panel.category.74", "zh-HK", "dspl b", "pcba板位类别.dspl b"),

            // dict.prod.pcba.panel.category.75
            ("dict.prod.pcba.panel.category.75", "en-US", "dspl b/t", "pcba板位类别.dspl b/t"),
            // dict.prod.pcba.panel.category.75
            ("dict.prod.pcba.panel.category.75", "ja-JP", "dspl b/t", "pcba板位类别.dspl b/t"),
            // dict.prod.pcba.panel.category.75
            ("dict.prod.pcba.panel.category.75", "zh-CN", "dspl b/t", "pcba板位类别.dspl b/t"),
            // dict.prod.pcba.panel.category.75
            ("dict.prod.pcba.panel.category.75", "zh-HK", "dspl b/t", "pcba板位类别.dspl b/t"),

            // dict.prod.pcba.panel.category.76
            ("dict.prod.pcba.panel.category.76", "en-US", "dspl t", "pcba板位类别.dspl t"),
            // dict.prod.pcba.panel.category.76
            ("dict.prod.pcba.panel.category.76", "ja-JP", "dspl t", "pcba板位类别.dspl t"),
            // dict.prod.pcba.panel.category.76
            ("dict.prod.pcba.panel.category.76", "zh-CN", "dspl t", "pcba板位类别.dspl t"),
            // dict.prod.pcba.panel.category.76
            ("dict.prod.pcba.panel.category.76", "zh-HK", "dspl t", "pcba板位类别.dspl t"),

            // dict.prod.pcba.panel.category.77
            ("dict.prod.pcba.panel.category.77", "en-US", "dsub b", "pcba板位类别.dsub b"),
            // dict.prod.pcba.panel.category.77
            ("dict.prod.pcba.panel.category.77", "ja-JP", "dsub b", "pcba板位类别.dsub b"),
            // dict.prod.pcba.panel.category.77
            ("dict.prod.pcba.panel.category.77", "zh-CN", "dsub b", "pcba板位类别.dsub b"),
            // dict.prod.pcba.panel.category.77
            ("dict.prod.pcba.panel.category.77", "zh-HK", "dsub b", "pcba板位类别.dsub b"),

            // dict.prod.pcba.panel.category.78
            ("dict.prod.pcba.panel.category.78", "en-US", "dsub t", "pcba板位类别.dsub t"),
            // dict.prod.pcba.panel.category.78
            ("dict.prod.pcba.panel.category.78", "ja-JP", "dsub t", "pcba板位类别.dsub t"),
            // dict.prod.pcba.panel.category.78
            ("dict.prod.pcba.panel.category.78", "zh-CN", "dsub t", "pcba板位类别.dsub t"),
            // dict.prod.pcba.panel.category.78
            ("dict.prod.pcba.panel.category.78", "zh-HK", "dsub t", "pcba板位类别.dsub t"),

            // dict.prod.pcba.panel.category.79
            ("dict.prod.pcba.panel.category.79", "en-US", "dyna b", "pcba板位类别.dyna b"),
            // dict.prod.pcba.panel.category.79
            ("dict.prod.pcba.panel.category.79", "ja-JP", "dyna b", "pcba板位类别.dyna b"),
            // dict.prod.pcba.panel.category.79
            ("dict.prod.pcba.panel.category.79", "zh-CN", "dyna b", "pcba板位类别.dyna b"),
            // dict.prod.pcba.panel.category.79
            ("dict.prod.pcba.panel.category.79", "zh-HK", "dyna b", "pcba板位类别.dyna b"),

            // dict.prod.pcba.panel.category.80
            ("dict.prod.pcba.panel.category.80", "en-US", "dyna t", "pcba板位类别.dyna t"),
            // dict.prod.pcba.panel.category.80
            ("dict.prod.pcba.panel.category.80", "ja-JP", "dyna t", "pcba板位类别.dyna t"),
            // dict.prod.pcba.panel.category.80
            ("dict.prod.pcba.panel.category.80", "zh-CN", "dyna t", "pcba板位类别.dyna t"),
            // dict.prod.pcba.panel.category.80
            ("dict.prod.pcba.panel.category.80", "zh-HK", "dyna t", "pcba板位类别.dyna t"),

            // dict.prod.pcba.panel.category.81
            ("dict.prod.pcba.panel.category.81", "en-US", "dyna t/b", "pcba板位类别.dyna t/b"),
            // dict.prod.pcba.panel.category.81
            ("dict.prod.pcba.panel.category.81", "ja-JP", "dyna t/b", "pcba板位类别.dyna t/b"),
            // dict.prod.pcba.panel.category.81
            ("dict.prod.pcba.panel.category.81", "zh-CN", "dyna t/b", "pcba板位类别.dyna t/b"),
            // dict.prod.pcba.panel.category.81
            ("dict.prod.pcba.panel.category.81", "zh-HK", "dyna t/b", "pcba板位类别.dyna t/b"),

            // dict.prod.pcba.panel.category.82
            ("dict.prod.pcba.panel.category.82", "en-US", "encoder", "pcba板位类别.encoder"),
            // dict.prod.pcba.panel.category.82
            ("dict.prod.pcba.panel.category.82", "ja-JP", "encoder", "pcba板位类别.encoder"),
            // dict.prod.pcba.panel.category.82
            ("dict.prod.pcba.panel.category.82", "zh-CN", "encoder", "pcba板位类别.encoder"),
            // dict.prod.pcba.panel.category.82
            ("dict.prod.pcba.panel.category.82", "zh-HK", "encoder", "pcba板位类别.encoder"),

            // dict.prod.pcba.panel.category.83
            ("dict.prod.pcba.panel.category.83", "en-US", "encoger", "pcba板位类别.encoger"),
            // dict.prod.pcba.panel.category.83
            ("dict.prod.pcba.panel.category.83", "ja-JP", "encoger", "pcba板位类别.encoger"),
            // dict.prod.pcba.panel.category.83
            ("dict.prod.pcba.panel.category.83", "zh-CN", "encoger", "pcba板位类别.encoger"),
            // dict.prod.pcba.panel.category.83
            ("dict.prod.pcba.panel.category.83", "zh-HK", "encoger", "pcba板位类别.encoger"),

            // dict.prod.pcba.panel.category.84
            ("dict.prod.pcba.panel.category.84", "en-US", "ether", "pcba板位类别.ether"),
            // dict.prod.pcba.panel.category.84
            ("dict.prod.pcba.panel.category.84", "ja-JP", "ether", "pcba板位类别.ether"),
            // dict.prod.pcba.panel.category.84
            ("dict.prod.pcba.panel.category.84", "zh-CN", "ether", "pcba板位类别.ether"),
            // dict.prod.pcba.panel.category.84
            ("dict.prod.pcba.panel.category.84", "zh-HK", "ether", "pcba板位类别.ether"),

            // dict.prod.pcba.panel.category.85
            ("dict.prod.pcba.panel.category.85", "en-US", "ether b", "pcba板位类别.ether b"),
            // dict.prod.pcba.panel.category.85
            ("dict.prod.pcba.panel.category.85", "ja-JP", "ether b", "pcba板位类别.ether b"),
            // dict.prod.pcba.panel.category.85
            ("dict.prod.pcba.panel.category.85", "zh-CN", "ether b", "pcba板位类别.ether b"),
            // dict.prod.pcba.panel.category.85
            ("dict.prod.pcba.panel.category.85", "zh-HK", "ether b", "pcba板位类别.ether b"),

            // dict.prod.pcba.panel.category.86
            ("dict.prod.pcba.panel.category.86", "en-US", "ether t", "pcba板位类别.ether t"),
            // dict.prod.pcba.panel.category.86
            ("dict.prod.pcba.panel.category.86", "ja-JP", "ether t", "pcba板位类别.ether t"),
            // dict.prod.pcba.panel.category.86
            ("dict.prod.pcba.panel.category.86", "zh-CN", "ether t", "pcba板位类别.ether t"),
            // dict.prod.pcba.panel.category.86
            ("dict.prod.pcba.panel.category.86", "zh-HK", "ether t", "pcba板位类别.ether t"),

            // dict.prod.pcba.panel.category.87
            ("dict.prod.pcba.panel.category.87", "en-US", "euro", "pcba板位类别.euro"),
            // dict.prod.pcba.panel.category.87
            ("dict.prod.pcba.panel.category.87", "ja-JP", "euro", "pcba板位类别.euro"),
            // dict.prod.pcba.panel.category.87
            ("dict.prod.pcba.panel.category.87", "zh-CN", "euro", "pcba板位类别.euro"),
            // dict.prod.pcba.panel.category.87
            ("dict.prod.pcba.panel.category.87", "zh-HK", "euro", "pcba板位类别.euro"),

            // dict.prod.pcba.panel.category.88
            ("dict.prod.pcba.panel.category.88", "en-US", "euro b", "pcba板位类别.euro b"),
            // dict.prod.pcba.panel.category.88
            ("dict.prod.pcba.panel.category.88", "ja-JP", "euro b", "pcba板位类别.euro b"),
            // dict.prod.pcba.panel.category.88
            ("dict.prod.pcba.panel.category.88", "zh-CN", "euro b", "pcba板位类别.euro b"),
            // dict.prod.pcba.panel.category.88
            ("dict.prod.pcba.panel.category.88", "zh-HK", "euro b", "pcba板位类别.euro b"),

            // dict.prod.pcba.panel.category.89
            ("dict.prod.pcba.panel.category.89", "en-US", "euro b/t", "pcba板位类别.euro b/t"),
            // dict.prod.pcba.panel.category.89
            ("dict.prod.pcba.panel.category.89", "ja-JP", "euro b/t", "pcba板位类别.euro b/t"),
            // dict.prod.pcba.panel.category.89
            ("dict.prod.pcba.panel.category.89", "zh-CN", "euro b/t", "pcba板位类别.euro b/t"),
            // dict.prod.pcba.panel.category.89
            ("dict.prod.pcba.panel.category.89", "zh-HK", "euro b/t", "pcba板位类别.euro b/t"),

            // dict.prod.pcba.panel.category.90
            ("dict.prod.pcba.panel.category.90", "en-US", "euro t", "pcba板位类别.euro t"),
            // dict.prod.pcba.panel.category.90
            ("dict.prod.pcba.panel.category.90", "ja-JP", "euro t", "pcba板位类别.euro t"),
            // dict.prod.pcba.panel.category.90
            ("dict.prod.pcba.panel.category.90", "zh-CN", "euro t", "pcba板位类别.euro t"),
            // dict.prod.pcba.panel.category.90
            ("dict.prod.pcba.panel.category.90", "zh-HK", "euro t", "pcba板位类别.euro t"),

            // dict.prod.pcba.panel.category.91
            ("dict.prod.pcba.panel.category.91", "en-US", "fader b", "pcba板位类别.fader b"),
            // dict.prod.pcba.panel.category.91
            ("dict.prod.pcba.panel.category.91", "ja-JP", "fader b", "pcba板位类别.fader b"),
            // dict.prod.pcba.panel.category.91
            ("dict.prod.pcba.panel.category.91", "zh-CN", "fader b", "pcba板位类别.fader b"),
            // dict.prod.pcba.panel.category.91
            ("dict.prod.pcba.panel.category.91", "zh-HK", "fader b", "pcba板位类别.fader b"),

            // dict.prod.pcba.panel.category.92
            ("dict.prod.pcba.panel.category.92", "en-US", "fader b/t", "pcba板位类别.fader b/t"),
            // dict.prod.pcba.panel.category.92
            ("dict.prod.pcba.panel.category.92", "ja-JP", "fader b/t", "pcba板位类别.fader b/t"),
            // dict.prod.pcba.panel.category.92
            ("dict.prod.pcba.panel.category.92", "zh-CN", "fader b/t", "pcba板位类别.fader b/t"),
            // dict.prod.pcba.panel.category.92
            ("dict.prod.pcba.panel.category.92", "zh-HK", "fader b/t", "pcba板位类别.fader b/t"),

            // dict.prod.pcba.panel.category.93
            ("dict.prod.pcba.panel.category.93", "en-US", "fader t", "pcba板位类别.fader t"),
            // dict.prod.pcba.panel.category.93
            ("dict.prod.pcba.panel.category.93", "ja-JP", "fader t", "pcba板位类别.fader t"),
            // dict.prod.pcba.panel.category.93
            ("dict.prod.pcba.panel.category.93", "zh-CN", "fader t", "pcba板位类别.fader t"),
            // dict.prod.pcba.panel.category.93
            ("dict.prod.pcba.panel.category.93", "zh-HK", "fader t", "pcba板位类别.fader t"),

            // dict.prod.pcba.panel.category.94
            ("dict.prod.pcba.panel.category.94", "en-US", "faether b", "pcba板位类别.faether b"),
            // dict.prod.pcba.panel.category.94
            ("dict.prod.pcba.panel.category.94", "ja-JP", "faether b", "pcba板位类别.faether b"),
            // dict.prod.pcba.panel.category.94
            ("dict.prod.pcba.panel.category.94", "zh-CN", "faether b", "pcba板位类别.faether b"),
            // dict.prod.pcba.panel.category.94
            ("dict.prod.pcba.panel.category.94", "zh-HK", "faether b", "pcba板位类别.faether b"),

            // dict.prod.pcba.panel.category.95
            ("dict.prod.pcba.panel.category.95", "en-US", "faether t", "pcba板位类别.faether t"),
            // dict.prod.pcba.panel.category.95
            ("dict.prod.pcba.panel.category.95", "ja-JP", "faether t", "pcba板位类别.faether t"),
            // dict.prod.pcba.panel.category.95
            ("dict.prod.pcba.panel.category.95", "zh-CN", "faether t", "pcba板位类别.faether t"),
            // dict.prod.pcba.panel.category.95
            ("dict.prod.pcba.panel.category.95", "zh-HK", "faether t", "pcba板位类别.faether t"),

            // dict.prod.pcba.panel.category.96
            ("dict.prod.pcba.panel.category.96", "en-US", "front", "pcba板位类别.front"),
            // dict.prod.pcba.panel.category.96
            ("dict.prod.pcba.panel.category.96", "ja-JP", "front", "pcba板位类别.front"),
            // dict.prod.pcba.panel.category.96
            ("dict.prod.pcba.panel.category.96", "zh-CN", "front", "pcba板位类别.front"),
            // dict.prod.pcba.panel.category.96
            ("dict.prod.pcba.panel.category.96", "zh-HK", "front", "pcba板位类别.front"),

            // dict.prod.pcba.panel.category.97
            ("dict.prod.pcba.panel.category.97", "en-US", "front a", "pcba板位类别.front a"),
            // dict.prod.pcba.panel.category.97
            ("dict.prod.pcba.panel.category.97", "ja-JP", "front a", "pcba板位类别.front a"),
            // dict.prod.pcba.panel.category.97
            ("dict.prod.pcba.panel.category.97", "zh-CN", "front a", "pcba板位类别.front a"),
            // dict.prod.pcba.panel.category.97
            ("dict.prod.pcba.panel.category.97", "zh-HK", "front a", "pcba板位类别.front a"),

            // dict.prod.pcba.panel.category.98
            ("dict.prod.pcba.panel.category.98", "en-US", "front b", "pcba板位类别.front b"),
            // dict.prod.pcba.panel.category.98
            ("dict.prod.pcba.panel.category.98", "ja-JP", "front b", "pcba板位类别.front b"),
            // dict.prod.pcba.panel.category.98
            ("dict.prod.pcba.panel.category.98", "zh-CN", "front b", "pcba板位类别.front b"),
            // dict.prod.pcba.panel.category.98
            ("dict.prod.pcba.panel.category.98", "zh-HK", "front b", "pcba板位类别.front b"),

            // dict.prod.pcba.panel.category.99
            ("dict.prod.pcba.panel.category.99", "en-US", "front b/t", "pcba板位类别.front b/t"),
            // dict.prod.pcba.panel.category.99
            ("dict.prod.pcba.panel.category.99", "ja-JP", "front b/t", "pcba板位类别.front b/t"),
            // dict.prod.pcba.panel.category.99
            ("dict.prod.pcba.panel.category.99", "zh-CN", "front b/t", "pcba板位类别.front b/t"),
            // dict.prod.pcba.panel.category.99
            ("dict.prod.pcba.panel.category.99", "zh-HK", "front b/t", "pcba板位类别.front b/t"),

            // dict.prod.pcba.panel.category.100
            ("dict.prod.pcba.panel.category.100", "en-US", "front sys t", "pcba板位类别.front sys t"),
            // dict.prod.pcba.panel.category.100
            ("dict.prod.pcba.panel.category.100", "ja-JP", "front sys t", "pcba板位类别.front sys t"),
            // dict.prod.pcba.panel.category.100
            ("dict.prod.pcba.panel.category.100", "zh-CN", "front sys t", "pcba板位类别.front sys t"),
            // dict.prod.pcba.panel.category.100
            ("dict.prod.pcba.panel.category.100", "zh-HK", "front sys t", "pcba板位类别.front sys t"),

            // dict.prod.pcba.panel.category.101
            ("dict.prod.pcba.panel.category.101", "en-US", "front t", "pcba板位类别.front t"),
            // dict.prod.pcba.panel.category.101
            ("dict.prod.pcba.panel.category.101", "ja-JP", "front t", "pcba板位类别.front t"),
            // dict.prod.pcba.panel.category.101
            ("dict.prod.pcba.panel.category.101", "zh-CN", "front t", "pcba板位类别.front t"),
            // dict.prod.pcba.panel.category.101
            ("dict.prod.pcba.panel.category.101", "zh-HK", "front t", "pcba板位类别.front t"),

            // dict.prod.pcba.panel.category.102
            ("dict.prod.pcba.panel.category.102", "en-US", "front-a", "pcba板位类别.front-a"),
            // dict.prod.pcba.panel.category.102
            ("dict.prod.pcba.panel.category.102", "ja-JP", "front-a", "pcba板位类别.front-a"),
            // dict.prod.pcba.panel.category.102
            ("dict.prod.pcba.panel.category.102", "zh-CN", "front-a", "pcba板位类别.front-a"),
            // dict.prod.pcba.panel.category.102
            ("dict.prod.pcba.panel.category.102", "zh-HK", "front-a", "pcba板位类别.front-a"),

            // dict.prod.pcba.panel.category.103
            ("dict.prod.pcba.panel.category.103", "en-US", "frotn b", "pcba板位类别.frotn b"),
            // dict.prod.pcba.panel.category.103
            ("dict.prod.pcba.panel.category.103", "ja-JP", "frotn b", "pcba板位类别.frotn b"),
            // dict.prod.pcba.panel.category.103
            ("dict.prod.pcba.panel.category.103", "zh-CN", "frotn b", "pcba板位类别.frotn b"),
            // dict.prod.pcba.panel.category.103
            ("dict.prod.pcba.panel.category.103", "zh-HK", "frotn b", "pcba板位类别.frotn b"),

            // dict.prod.pcba.panel.category.104
            ("dict.prod.pcba.panel.category.104", "en-US", "gather", "pcba板位类别.gather"),
            // dict.prod.pcba.panel.category.104
            ("dict.prod.pcba.panel.category.104", "ja-JP", "gather", "pcba板位类别.gather"),
            // dict.prod.pcba.panel.category.104
            ("dict.prod.pcba.panel.category.104", "zh-CN", "gather", "pcba板位类别.gather"),
            // dict.prod.pcba.panel.category.104
            ("dict.prod.pcba.panel.category.104", "zh-HK", "gather", "pcba板位类别.gather"),

            // dict.prod.pcba.panel.category.105
            ("dict.prod.pcba.panel.category.105", "en-US", "gather a", "pcba板位类别.gather a"),
            // dict.prod.pcba.panel.category.105
            ("dict.prod.pcba.panel.category.105", "ja-JP", "gather a", "pcba板位类别.gather a"),
            // dict.prod.pcba.panel.category.105
            ("dict.prod.pcba.panel.category.105", "zh-CN", "gather a", "pcba板位类别.gather a"),
            // dict.prod.pcba.panel.category.105
            ("dict.prod.pcba.panel.category.105", "zh-HK", "gather a", "pcba板位类别.gather a"),

            // dict.prod.pcba.panel.category.106
            ("dict.prod.pcba.panel.category.106", "en-US", "gather alt b", "pcba板位类别.gather alt b"),
            // dict.prod.pcba.panel.category.106
            ("dict.prod.pcba.panel.category.106", "ja-JP", "gather alt b", "pcba板位类别.gather alt b"),
            // dict.prod.pcba.panel.category.106
            ("dict.prod.pcba.panel.category.106", "zh-CN", "gather alt b", "pcba板位类别.gather alt b"),
            // dict.prod.pcba.panel.category.106
            ("dict.prod.pcba.panel.category.106", "zh-HK", "gather alt b", "pcba板位类别.gather alt b"),

            // dict.prod.pcba.panel.category.107
            ("dict.prod.pcba.panel.category.107", "en-US", "gather alt t", "pcba板位类别.gather alt t"),
            // dict.prod.pcba.panel.category.107
            ("dict.prod.pcba.panel.category.107", "ja-JP", "gather alt t", "pcba板位类别.gather alt t"),
            // dict.prod.pcba.panel.category.107
            ("dict.prod.pcba.panel.category.107", "zh-CN", "gather alt t", "pcba板位类别.gather alt t"),
            // dict.prod.pcba.panel.category.107
            ("dict.prod.pcba.panel.category.107", "zh-HK", "gather alt t", "pcba板位类别.gather alt t"),

            // dict.prod.pcba.panel.category.108
            ("dict.prod.pcba.panel.category.108", "en-US", "gather b", "pcba板位类别.gather b"),
            // dict.prod.pcba.panel.category.108
            ("dict.prod.pcba.panel.category.108", "ja-JP", "gather b", "pcba板位类别.gather b"),
            // dict.prod.pcba.panel.category.108
            ("dict.prod.pcba.panel.category.108", "zh-CN", "gather b", "pcba板位类别.gather b"),
            // dict.prod.pcba.panel.category.108
            ("dict.prod.pcba.panel.category.108", "zh-HK", "gather b", "pcba板位类别.gather b"),

            // dict.prod.pcba.panel.category.109
            ("dict.prod.pcba.panel.category.109", "en-US", "gather b/t", "pcba板位类别.gather b/t"),
            // dict.prod.pcba.panel.category.109
            ("dict.prod.pcba.panel.category.109", "ja-JP", "gather b/t", "pcba板位类别.gather b/t"),
            // dict.prod.pcba.panel.category.109
            ("dict.prod.pcba.panel.category.109", "zh-CN", "gather b/t", "pcba板位类别.gather b/t"),
            // dict.prod.pcba.panel.category.109
            ("dict.prod.pcba.panel.category.109", "zh-HK", "gather b/t", "pcba板位类别.gather b/t"),

            // dict.prod.pcba.panel.category.110
            ("dict.prod.pcba.panel.category.110", "en-US", "gather c", "pcba板位类别.gather c"),
            // dict.prod.pcba.panel.category.110
            ("dict.prod.pcba.panel.category.110", "ja-JP", "gather c", "pcba板位类别.gather c"),
            // dict.prod.pcba.panel.category.110
            ("dict.prod.pcba.panel.category.110", "zh-CN", "gather c", "pcba板位类别.gather c"),
            // dict.prod.pcba.panel.category.110
            ("dict.prod.pcba.panel.category.110", "zh-HK", "gather c", "pcba板位类别.gather c"),

            // dict.prod.pcba.panel.category.111
            ("dict.prod.pcba.panel.category.111", "en-US", "gather t", "pcba板位类别.gather t"),
            // dict.prod.pcba.panel.category.111
            ("dict.prod.pcba.panel.category.111", "ja-JP", "gather t", "pcba板位类别.gather t"),
            // dict.prod.pcba.panel.category.111
            ("dict.prod.pcba.panel.category.111", "zh-CN", "gather t", "pcba板位类别.gather t"),
            // dict.prod.pcba.panel.category.111
            ("dict.prod.pcba.panel.category.111", "zh-HK", "gather t", "pcba板位类别.gather t"),

            // dict.prod.pcba.panel.category.112
            ("dict.prod.pcba.panel.category.112", "en-US", "gather-c", "pcba板位类别.gather-c"),
            // dict.prod.pcba.panel.category.112
            ("dict.prod.pcba.panel.category.112", "ja-JP", "gather-c", "pcba板位类别.gather-c"),
            // dict.prod.pcba.panel.category.112
            ("dict.prod.pcba.panel.category.112", "zh-CN", "gather-c", "pcba板位类别.gather-c"),
            // dict.prod.pcba.panel.category.112
            ("dict.prod.pcba.panel.category.112", "zh-HK", "gather-c", "pcba板位类别.gather-c"),

            // dict.prod.pcba.panel.category.113
            ("dict.prod.pcba.panel.category.113", "en-US", "gather-j", "pcba板位类别.gather-j"),
            // dict.prod.pcba.panel.category.113
            ("dict.prod.pcba.panel.category.113", "ja-JP", "gather-j", "pcba板位类别.gather-j"),
            // dict.prod.pcba.panel.category.113
            ("dict.prod.pcba.panel.category.113", "zh-CN", "gather-j", "pcba板位类别.gather-j"),
            // dict.prod.pcba.panel.category.113
            ("dict.prod.pcba.panel.category.113", "zh-HK", "gather-j", "pcba板位类别.gather-j"),

            // dict.prod.pcba.panel.category.114
            ("dict.prod.pcba.panel.category.114", "en-US", "if", "pcba板位类别.if"),
            // dict.prod.pcba.panel.category.114
            ("dict.prod.pcba.panel.category.114", "ja-JP", "if", "pcba板位类别.if"),
            // dict.prod.pcba.panel.category.114
            ("dict.prod.pcba.panel.category.114", "zh-CN", "if", "pcba板位类别.if"),
            // dict.prod.pcba.panel.category.114
            ("dict.prod.pcba.panel.category.114", "zh-HK", "if", "pcba板位类别.if"),

            // dict.prod.pcba.panel.category.117
            ("dict.prod.pcba.panel.category.117", "en-US", "if b", "pcba板位类别.if b"),
            // dict.prod.pcba.panel.category.117
            ("dict.prod.pcba.panel.category.117", "ja-JP", "if b", "pcba板位类别.if b"),
            // dict.prod.pcba.panel.category.117
            ("dict.prod.pcba.panel.category.117", "zh-CN", "if b", "pcba板位类别.if b"),
            // dict.prod.pcba.panel.category.117
            ("dict.prod.pcba.panel.category.117", "zh-HK", "if b", "pcba板位类别.if b"),

            // dict.prod.pcba.panel.category.118
            ("dict.prod.pcba.panel.category.118", "en-US", "if t", "pcba板位类别.if t"),
            // dict.prod.pcba.panel.category.118
            ("dict.prod.pcba.panel.category.118", "ja-JP", "if t", "pcba板位类别.if t"),
            // dict.prod.pcba.panel.category.118
            ("dict.prod.pcba.panel.category.118", "zh-CN", "if t", "pcba板位类别.if t"),
            // dict.prod.pcba.panel.category.118
            ("dict.prod.pcba.panel.category.118", "zh-HK", "if t", "pcba板位类别.if t"),

            // dict.prod.pcba.panel.category.119
            ("dict.prod.pcba.panel.category.119", "en-US", "input", "pcba板位类别.input"),
            // dict.prod.pcba.panel.category.119
            ("dict.prod.pcba.panel.category.119", "ja-JP", "input", "pcba板位类别.input"),
            // dict.prod.pcba.panel.category.119
            ("dict.prod.pcba.panel.category.119", "zh-CN", "input", "pcba板位类别.input"),
            // dict.prod.pcba.panel.category.119
            ("dict.prod.pcba.panel.category.119", "zh-HK", "input", "pcba板位类别.input"),

            // dict.prod.pcba.panel.category.120
            ("dict.prod.pcba.panel.category.120", "en-US", "io", "pcba板位类别.io"),
            // dict.prod.pcba.panel.category.120
            ("dict.prod.pcba.panel.category.120", "ja-JP", "io", "pcba板位类别.io"),
            // dict.prod.pcba.panel.category.120
            ("dict.prod.pcba.panel.category.120", "zh-CN", "io", "pcba板位类别.io"),
            // dict.prod.pcba.panel.category.120
            ("dict.prod.pcba.panel.category.120", "zh-HK", "io", "pcba板位类别.io"),

            // dict.prod.pcba.panel.category.121
            ("dict.prod.pcba.panel.category.121", "en-US", "io b/t", "pcba板位类别.io b/t"),
            // dict.prod.pcba.panel.category.121
            ("dict.prod.pcba.panel.category.121", "ja-JP", "io b/t", "pcba板位类别.io b/t"),
            // dict.prod.pcba.panel.category.121
            ("dict.prod.pcba.panel.category.121", "zh-CN", "io b/t", "pcba板位类别.io b/t"),
            // dict.prod.pcba.panel.category.121
            ("dict.prod.pcba.panel.category.121", "zh-HK", "io b/t", "pcba板位类别.io b/t"),

            // dict.prod.pcba.panel.category.122
            ("dict.prod.pcba.panel.category.122", "en-US", "io t", "pcba板位类别.io t"),
            // dict.prod.pcba.panel.category.122
            ("dict.prod.pcba.panel.category.122", "ja-JP", "io t", "pcba板位类别.io t"),
            // dict.prod.pcba.panel.category.122
            ("dict.prod.pcba.panel.category.122", "zh-CN", "io t", "pcba板位类别.io t"),
            // dict.prod.pcba.panel.category.122
            ("dict.prod.pcba.panel.category.122", "zh-HK", "io t", "pcba板位类别.io t"),

            // dict.prod.pcba.panel.category.123
            ("dict.prod.pcba.panel.category.123", "en-US", "jack", "pcba板位类别.jack"),
            // dict.prod.pcba.panel.category.123
            ("dict.prod.pcba.panel.category.123", "ja-JP", "jack", "pcba板位类别.jack"),
            // dict.prod.pcba.panel.category.123
            ("dict.prod.pcba.panel.category.123", "zh-CN", "jack", "pcba板位类别.jack"),
            // dict.prod.pcba.panel.category.123
            ("dict.prod.pcba.panel.category.123", "zh-HK", "jack", "pcba板位类别.jack"),

            // dict.prod.pcba.panel.category.124
            ("dict.prod.pcba.panel.category.124", "en-US", "jack a", "pcba板位类别.jack a"),
            // dict.prod.pcba.panel.category.124
            ("dict.prod.pcba.panel.category.124", "ja-JP", "jack a", "pcba板位类别.jack a"),
            // dict.prod.pcba.panel.category.124
            ("dict.prod.pcba.panel.category.124", "zh-CN", "jack a", "pcba板位类别.jack a"),
            // dict.prod.pcba.panel.category.124
            ("dict.prod.pcba.panel.category.124", "zh-HK", "jack a", "pcba板位类别.jack a"),

            // dict.prod.pcba.panel.category.125
            ("dict.prod.pcba.panel.category.125", "en-US", "jack b", "pcba板位类别.jack b"),
            // dict.prod.pcba.panel.category.125
            ("dict.prod.pcba.panel.category.125", "ja-JP", "jack b", "pcba板位类别.jack b"),
            // dict.prod.pcba.panel.category.125
            ("dict.prod.pcba.panel.category.125", "zh-CN", "jack b", "pcba板位类别.jack b"),
            // dict.prod.pcba.panel.category.125
            ("dict.prod.pcba.panel.category.125", "zh-HK", "jack b", "pcba板位类别.jack b"),

            // dict.prod.pcba.panel.category.126
            ("dict.prod.pcba.panel.category.126", "en-US", "jack b/t", "pcba板位类别.jack b/t"),
            // dict.prod.pcba.panel.category.126
            ("dict.prod.pcba.panel.category.126", "ja-JP", "jack b/t", "pcba板位类别.jack b/t"),
            // dict.prod.pcba.panel.category.126
            ("dict.prod.pcba.panel.category.126", "zh-CN", "jack b/t", "pcba板位类别.jack b/t"),
            // dict.prod.pcba.panel.category.126
            ("dict.prod.pcba.panel.category.126", "zh-HK", "jack b/t", "pcba板位类别.jack b/t"),

            // dict.prod.pcba.panel.category.127
            ("dict.prod.pcba.panel.category.127", "en-US", "jack t", "pcba板位类别.jack t"),
            // dict.prod.pcba.panel.category.127
            ("dict.prod.pcba.panel.category.127", "ja-JP", "jack t", "pcba板位类别.jack t"),
            // dict.prod.pcba.panel.category.127
            ("dict.prod.pcba.panel.category.127", "zh-CN", "jack t", "pcba板位类别.jack t"),
            // dict.prod.pcba.panel.category.127
            ("dict.prod.pcba.panel.category.127", "zh-HK", "jack t", "pcba板位类别.jack t"),

            // dict.prod.pcba.panel.category.128
            ("dict.prod.pcba.panel.category.128", "en-US", "jack-00 b", "pcba板位类别.jack-00 b"),
            // dict.prod.pcba.panel.category.128
            ("dict.prod.pcba.panel.category.128", "ja-JP", "jack-00 b", "pcba板位类别.jack-00 b"),
            // dict.prod.pcba.panel.category.128
            ("dict.prod.pcba.panel.category.128", "zh-CN", "jack-00 b", "pcba板位类别.jack-00 b"),
            // dict.prod.pcba.panel.category.128
            ("dict.prod.pcba.panel.category.128", "zh-HK", "jack-00 b", "pcba板位类别.jack-00 b"),

            // dict.prod.pcba.panel.category.132
            ("dict.prod.pcba.panel.category.132", "en-US", "jack-00 t", "pcba板位类别.jack-00 t"),
            // dict.prod.pcba.panel.category.132
            ("dict.prod.pcba.panel.category.132", "ja-JP", "jack-00 t", "pcba板位类别.jack-00 t"),
            // dict.prod.pcba.panel.category.132
            ("dict.prod.pcba.panel.category.132", "zh-CN", "jack-00 t", "pcba板位类别.jack-00 t"),
            // dict.prod.pcba.panel.category.132
            ("dict.prod.pcba.panel.category.132", "zh-HK", "jack-00 t", "pcba板位类别.jack-00 t"),

            // dict.prod.pcba.panel.category.133
            ("dict.prod.pcba.panel.category.133", "en-US", "jack-10 b", "pcba板位类别.jack-10 b"),
            // dict.prod.pcba.panel.category.133
            ("dict.prod.pcba.panel.category.133", "ja-JP", "jack-10 b", "pcba板位类别.jack-10 b"),
            // dict.prod.pcba.panel.category.133
            ("dict.prod.pcba.panel.category.133", "zh-CN", "jack-10 b", "pcba板位类别.jack-10 b"),
            // dict.prod.pcba.panel.category.133
            ("dict.prod.pcba.panel.category.133", "zh-HK", "jack-10 b", "pcba板位类别.jack-10 b"),

            // dict.prod.pcba.panel.category.134
            ("dict.prod.pcba.panel.category.134", "en-US", "jack-10 t", "pcba板位类别.jack-10 t"),
            // dict.prod.pcba.panel.category.134
            ("dict.prod.pcba.panel.category.134", "ja-JP", "jack-10 t", "pcba板位类别.jack-10 t"),
            // dict.prod.pcba.panel.category.134
            ("dict.prod.pcba.panel.category.134", "zh-CN", "jack-10 t", "pcba板位类别.jack-10 t"),
            // dict.prod.pcba.panel.category.134
            ("dict.prod.pcba.panel.category.134", "zh-HK", "jack-10 t", "pcba板位类别.jack-10 t"),

            // dict.prod.pcba.panel.category.135
            ("dict.prod.pcba.panel.category.135", "en-US", "jack-20 b", "pcba板位类别.jack-20 b"),
            // dict.prod.pcba.panel.category.135
            ("dict.prod.pcba.panel.category.135", "ja-JP", "jack-20 b", "pcba板位类别.jack-20 b"),
            // dict.prod.pcba.panel.category.135
            ("dict.prod.pcba.panel.category.135", "zh-CN", "jack-20 b", "pcba板位类别.jack-20 b"),
            // dict.prod.pcba.panel.category.135
            ("dict.prod.pcba.panel.category.135", "zh-HK", "jack-20 b", "pcba板位类别.jack-20 b"),

            // dict.prod.pcba.panel.category.136
            ("dict.prod.pcba.panel.category.136", "en-US", "jack-20 t", "pcba板位类别.jack-20 t"),
            // dict.prod.pcba.panel.category.136
            ("dict.prod.pcba.panel.category.136", "ja-JP", "jack-20 t", "pcba板位类别.jack-20 t"),
            // dict.prod.pcba.panel.category.136
            ("dict.prod.pcba.panel.category.136", "zh-CN", "jack-20 t", "pcba板位类别.jack-20 t"),
            // dict.prod.pcba.panel.category.136
            ("dict.prod.pcba.panel.category.136", "zh-HK", "jack-20 t", "pcba板位类别.jack-20 t"),

            // dict.prod.pcba.panel.category.137
            ("dict.prod.pcba.panel.category.137", "en-US", "jack-30 b", "pcba板位类别.jack-30 b"),
            // dict.prod.pcba.panel.category.137
            ("dict.prod.pcba.panel.category.137", "ja-JP", "jack-30 b", "pcba板位类别.jack-30 b"),
            // dict.prod.pcba.panel.category.137
            ("dict.prod.pcba.panel.category.137", "zh-CN", "jack-30 b", "pcba板位类别.jack-30 b"),
            // dict.prod.pcba.panel.category.137
            ("dict.prod.pcba.panel.category.137", "zh-HK", "jack-30 b", "pcba板位类别.jack-30 b"),

            // dict.prod.pcba.panel.category.138
            ("dict.prod.pcba.panel.category.138", "en-US", "jack-30 t", "pcba板位类别.jack-30 t"),
            // dict.prod.pcba.panel.category.138
            ("dict.prod.pcba.panel.category.138", "ja-JP", "jack-30 t", "pcba板位类别.jack-30 t"),
            // dict.prod.pcba.panel.category.138
            ("dict.prod.pcba.panel.category.138", "zh-CN", "jack-30 t", "pcba板位类别.jack-30 t"),
            // dict.prod.pcba.panel.category.138
            ("dict.prod.pcba.panel.category.138", "zh-HK", "jack-30 t", "pcba板位类别.jack-30 t"),

            // dict.prod.pcba.panel.category.139
            ("dict.prod.pcba.panel.category.139", "en-US", "join", "pcba板位类别.join"),
            // dict.prod.pcba.panel.category.139
            ("dict.prod.pcba.panel.category.139", "ja-JP", "join", "pcba板位类别.join"),
            // dict.prod.pcba.panel.category.139
            ("dict.prod.pcba.panel.category.139", "zh-CN", "join", "pcba板位类别.join"),
            // dict.prod.pcba.panel.category.139
            ("dict.prod.pcba.panel.category.139", "zh-HK", "join", "pcba板位类别.join"),

            // dict.prod.pcba.panel.category.140
            ("dict.prod.pcba.panel.category.140", "en-US", "jointc a", "pcba板位类别.jointc a"),
            // dict.prod.pcba.panel.category.140
            ("dict.prod.pcba.panel.category.140", "ja-JP", "jointc a", "pcba板位类别.jointc a"),
            // dict.prod.pcba.panel.category.140
            ("dict.prod.pcba.panel.category.140", "zh-CN", "jointc a", "pcba板位类别.jointc a"),
            // dict.prod.pcba.panel.category.140
            ("dict.prod.pcba.panel.category.140", "zh-HK", "jointc a", "pcba板位类别.jointc a"),

            // dict.prod.pcba.panel.category.141
            ("dict.prod.pcba.panel.category.141", "en-US", "jointc b", "pcba板位类别.jointc b"),
            // dict.prod.pcba.panel.category.141
            ("dict.prod.pcba.panel.category.141", "ja-JP", "jointc b", "pcba板位类别.jointc b"),
            // dict.prod.pcba.panel.category.141
            ("dict.prod.pcba.panel.category.141", "zh-CN", "jointc b", "pcba板位类别.jointc b"),
            // dict.prod.pcba.panel.category.141
            ("dict.prod.pcba.panel.category.141", "zh-HK", "jointc b", "pcba板位类别.jointc b"),

            // dict.prod.pcba.panel.category.142
            ("dict.prod.pcba.panel.category.142", "en-US", "jointc t", "pcba板位类别.jointc t"),
            // dict.prod.pcba.panel.category.142
            ("dict.prod.pcba.panel.category.142", "ja-JP", "jointc t", "pcba板位类别.jointc t"),
            // dict.prod.pcba.panel.category.142
            ("dict.prod.pcba.panel.category.142", "zh-CN", "jointc t", "pcba板位类别.jointc t"),
            // dict.prod.pcba.panel.category.142
            ("dict.prod.pcba.panel.category.142", "zh-HK", "jointc t", "pcba板位类别.jointc t"),

            // dict.prod.pcba.panel.category.143
            ("dict.prod.pcba.panel.category.143", "en-US", "jointf a", "pcba板位类别.jointf a"),
            // dict.prod.pcba.panel.category.143
            ("dict.prod.pcba.panel.category.143", "ja-JP", "jointf a", "pcba板位类别.jointf a"),
            // dict.prod.pcba.panel.category.143
            ("dict.prod.pcba.panel.category.143", "zh-CN", "jointf a", "pcba板位类别.jointf a"),
            // dict.prod.pcba.panel.category.143
            ("dict.prod.pcba.panel.category.143", "zh-HK", "jointf a", "pcba板位类别.jointf a"),

            // dict.prod.pcba.panel.category.144
            ("dict.prod.pcba.panel.category.144", "en-US", "jointf b", "pcba板位类别.jointf b"),
            // dict.prod.pcba.panel.category.144
            ("dict.prod.pcba.panel.category.144", "ja-JP", "jointf b", "pcba板位类别.jointf b"),
            // dict.prod.pcba.panel.category.144
            ("dict.prod.pcba.panel.category.144", "zh-CN", "jointf b", "pcba板位类别.jointf b"),
            // dict.prod.pcba.panel.category.144
            ("dict.prod.pcba.panel.category.144", "zh-HK", "jointf b", "pcba板位类别.jointf b"),

            // dict.prod.pcba.panel.category.145
            ("dict.prod.pcba.panel.category.145", "en-US", "jointf t", "pcba板位类别.jointf t"),
            // dict.prod.pcba.panel.category.145
            ("dict.prod.pcba.panel.category.145", "ja-JP", "jointf t", "pcba板位类别.jointf t"),
            // dict.prod.pcba.panel.category.145
            ("dict.prod.pcba.panel.category.145", "zh-CN", "jointf t", "pcba板位类别.jointf t"),
            // dict.prod.pcba.panel.category.145
            ("dict.prod.pcba.panel.category.145", "zh-HK", "jointf t", "pcba板位类别.jointf t"),

            // dict.prod.pcba.panel.category.146
            ("dict.prod.pcba.panel.category.146", "en-US", "joints", "pcba板位类别.joints"),
            // dict.prod.pcba.panel.category.146
            ("dict.prod.pcba.panel.category.146", "ja-JP", "joints", "pcba板位类别.joints"),
            // dict.prod.pcba.panel.category.146
            ("dict.prod.pcba.panel.category.146", "zh-CN", "joints", "pcba板位类别.joints"),
            // dict.prod.pcba.panel.category.146
            ("dict.prod.pcba.panel.category.146", "zh-HK", "joints", "pcba板位类别.joints"),

            // dict.prod.pcba.panel.category.147
            ("dict.prod.pcba.panel.category.147", "en-US", "key", "pcba板位类别.key"),
            // dict.prod.pcba.panel.category.147
            ("dict.prod.pcba.panel.category.147", "ja-JP", "key", "pcba板位类别.key"),
            // dict.prod.pcba.panel.category.147
            ("dict.prod.pcba.panel.category.147", "zh-CN", "key", "pcba板位类别.key"),
            // dict.prod.pcba.panel.category.147
            ("dict.prod.pcba.panel.category.147", "zh-HK", "key", "pcba板位类别.key"),

            // dict.prod.pcba.panel.category.148
            ("dict.prod.pcba.panel.category.148", "en-US", "key b", "pcba板位类别.key b"),
            // dict.prod.pcba.panel.category.148
            ("dict.prod.pcba.panel.category.148", "ja-JP", "key b", "pcba板位类别.key b"),
            // dict.prod.pcba.panel.category.148
            ("dict.prod.pcba.panel.category.148", "zh-CN", "key b", "pcba板位类别.key b"),
            // dict.prod.pcba.panel.category.148
            ("dict.prod.pcba.panel.category.148", "zh-HK", "key b", "pcba板位类别.key b"),

            // dict.prod.pcba.panel.category.149
            ("dict.prod.pcba.panel.category.149", "en-US", "key b/t", "pcba板位类别.key b/t"),
            // dict.prod.pcba.panel.category.149
            ("dict.prod.pcba.panel.category.149", "ja-JP", "key b/t", "pcba板位类别.key b/t"),
            // dict.prod.pcba.panel.category.149
            ("dict.prod.pcba.panel.category.149", "zh-CN", "key b/t", "pcba板位类别.key b/t"),
            // dict.prod.pcba.panel.category.149
            ("dict.prod.pcba.panel.category.149", "zh-HK", "key b/t", "pcba板位类别.key b/t"),

            // dict.prod.pcba.panel.category.150
            ("dict.prod.pcba.panel.category.150", "en-US", "key t", "pcba板位类别.key t"),
            // dict.prod.pcba.panel.category.150
            ("dict.prod.pcba.panel.category.150", "ja-JP", "key t", "pcba板位类别.key t"),
            // dict.prod.pcba.panel.category.150
            ("dict.prod.pcba.panel.category.150", "zh-CN", "key t", "pcba板位类别.key t"),
            // dict.prod.pcba.panel.category.150
            ("dict.prod.pcba.panel.category.150", "zh-HK", "key t", "pcba板位类别.key t"),

            // dict.prod.pcba.panel.category.151
            ("dict.prod.pcba.panel.category.151", "en-US", "lcd a", "pcba板位类别.lcd a"),
            // dict.prod.pcba.panel.category.151
            ("dict.prod.pcba.panel.category.151", "ja-JP", "lcd a", "pcba板位类别.lcd a"),
            // dict.prod.pcba.panel.category.151
            ("dict.prod.pcba.panel.category.151", "zh-CN", "lcd a", "pcba板位类别.lcd a"),
            // dict.prod.pcba.panel.category.151
            ("dict.prod.pcba.panel.category.151", "zh-HK", "lcd a", "pcba板位类别.lcd a"),

            // dict.prod.pcba.panel.category.152
            ("dict.prod.pcba.panel.category.152", "en-US", "lcd b", "pcba板位类别.lcd b"),
            // dict.prod.pcba.panel.category.152
            ("dict.prod.pcba.panel.category.152", "ja-JP", "lcd b", "pcba板位类别.lcd b"),
            // dict.prod.pcba.panel.category.152
            ("dict.prod.pcba.panel.category.152", "zh-CN", "lcd b", "pcba板位类别.lcd b"),
            // dict.prod.pcba.panel.category.152
            ("dict.prod.pcba.panel.category.152", "zh-HK", "lcd b", "pcba板位类别.lcd b"),

            // dict.prod.pcba.panel.category.153
            ("dict.prod.pcba.panel.category.153", "en-US", "lcd b/t", "pcba板位类别.lcd b/t"),
            // dict.prod.pcba.panel.category.153
            ("dict.prod.pcba.panel.category.153", "ja-JP", "lcd b/t", "pcba板位类别.lcd b/t"),
            // dict.prod.pcba.panel.category.153
            ("dict.prod.pcba.panel.category.153", "zh-CN", "lcd b/t", "pcba板位类别.lcd b/t"),
            // dict.prod.pcba.panel.category.153
            ("dict.prod.pcba.panel.category.153", "zh-HK", "lcd b/t", "pcba板位类别.lcd b/t"),

            // dict.prod.pcba.panel.category.154
            ("dict.prod.pcba.panel.category.154", "en-US", "lcd ex", "pcba板位类别.lcd ex"),
            // dict.prod.pcba.panel.category.154
            ("dict.prod.pcba.panel.category.154", "ja-JP", "lcd ex", "pcba板位类别.lcd ex"),
            // dict.prod.pcba.panel.category.154
            ("dict.prod.pcba.panel.category.154", "zh-CN", "lcd ex", "pcba板位类别.lcd ex"),
            // dict.prod.pcba.panel.category.154
            ("dict.prod.pcba.panel.category.154", "zh-HK", "lcd ex", "pcba板位类别.lcd ex"),

            // dict.prod.pcba.panel.category.155
            ("dict.prod.pcba.panel.category.155", "en-US", "lcd ex b", "pcba板位类别.lcd ex b"),
            // dict.prod.pcba.panel.category.155
            ("dict.prod.pcba.panel.category.155", "ja-JP", "lcd ex b", "pcba板位类别.lcd ex b"),
            // dict.prod.pcba.panel.category.155
            ("dict.prod.pcba.panel.category.155", "zh-CN", "lcd ex b", "pcba板位类别.lcd ex b"),
            // dict.prod.pcba.panel.category.155
            ("dict.prod.pcba.panel.category.155", "zh-HK", "lcd ex b", "pcba板位类别.lcd ex b"),

            // dict.prod.pcba.panel.category.156
            ("dict.prod.pcba.panel.category.156", "en-US", "lcd ex b/t", "pcba板位类别.lcd ex b/t"),
            // dict.prod.pcba.panel.category.156
            ("dict.prod.pcba.panel.category.156", "ja-JP", "lcd ex b/t", "pcba板位类别.lcd ex b/t"),
            // dict.prod.pcba.panel.category.156
            ("dict.prod.pcba.panel.category.156", "zh-CN", "lcd ex b/t", "pcba板位类别.lcd ex b/t"),
            // dict.prod.pcba.panel.category.156
            ("dict.prod.pcba.panel.category.156", "zh-HK", "lcd ex b/t", "pcba板位类别.lcd ex b/t"),

            // dict.prod.pcba.panel.category.157
            ("dict.prod.pcba.panel.category.157", "en-US", "lcd ex t", "pcba板位类别.lcd ex t"),
            // dict.prod.pcba.panel.category.157
            ("dict.prod.pcba.panel.category.157", "ja-JP", "lcd ex t", "pcba板位类别.lcd ex t"),
            // dict.prod.pcba.panel.category.157
            ("dict.prod.pcba.panel.category.157", "zh-CN", "lcd ex t", "pcba板位类别.lcd ex t"),
            // dict.prod.pcba.panel.category.157
            ("dict.prod.pcba.panel.category.157", "zh-HK", "lcd ex t", "pcba板位类别.lcd ex t"),

            // dict.prod.pcba.panel.category.158
            ("dict.prod.pcba.panel.category.158", "en-US", "madi b", "pcba板位类别.madi b"),
            // dict.prod.pcba.panel.category.158
            ("dict.prod.pcba.panel.category.158", "ja-JP", "madi b", "pcba板位类别.madi b"),
            // dict.prod.pcba.panel.category.158
            ("dict.prod.pcba.panel.category.158", "zh-CN", "madi b", "pcba板位类别.madi b"),
            // dict.prod.pcba.panel.category.158
            ("dict.prod.pcba.panel.category.158", "zh-HK", "madi b", "pcba板位类别.madi b"),

            // dict.prod.pcba.panel.category.161
            ("dict.prod.pcba.panel.category.161", "en-US", "madi b/t", "pcba板位类别.madi b/t"),
            // dict.prod.pcba.panel.category.161
            ("dict.prod.pcba.panel.category.161", "ja-JP", "madi b/t", "pcba板位类别.madi b/t"),
            // dict.prod.pcba.panel.category.161
            ("dict.prod.pcba.panel.category.161", "zh-CN", "madi b/t", "pcba板位类别.madi b/t"),
            // dict.prod.pcba.panel.category.161
            ("dict.prod.pcba.panel.category.161", "zh-HK", "madi b/t", "pcba板位类别.madi b/t"),

            // dict.prod.pcba.panel.category.162
            ("dict.prod.pcba.panel.category.162", "en-US", "madi t", "pcba板位类别.madi t"),
            // dict.prod.pcba.panel.category.162
            ("dict.prod.pcba.panel.category.162", "ja-JP", "madi t", "pcba板位类别.madi t"),
            // dict.prod.pcba.panel.category.162
            ("dict.prod.pcba.panel.category.162", "zh-CN", "madi t", "pcba板位类别.madi t"),
            // dict.prod.pcba.panel.category.162
            ("dict.prod.pcba.panel.category.162", "zh-HK", "madi t", "pcba板位类别.madi t"),

            // dict.prod.pcba.panel.category.163
            ("dict.prod.pcba.panel.category.163", "en-US", "mafad a", "pcba板位类别.mafad a"),
            // dict.prod.pcba.panel.category.163
            ("dict.prod.pcba.panel.category.163", "ja-JP", "mafad a", "pcba板位类别.mafad a"),
            // dict.prod.pcba.panel.category.163
            ("dict.prod.pcba.panel.category.163", "zh-CN", "mafad a", "pcba板位类别.mafad a"),
            // dict.prod.pcba.panel.category.163
            ("dict.prod.pcba.panel.category.163", "zh-HK", "mafad a", "pcba板位类别.mafad a"),

            // dict.prod.pcba.panel.category.164
            ("dict.prod.pcba.panel.category.164", "en-US", "mafad b", "pcba板位类别.mafad b"),
            // dict.prod.pcba.panel.category.164
            ("dict.prod.pcba.panel.category.164", "ja-JP", "mafad b", "pcba板位类别.mafad b"),
            // dict.prod.pcba.panel.category.164
            ("dict.prod.pcba.panel.category.164", "zh-CN", "mafad b", "pcba板位类别.mafad b"),
            // dict.prod.pcba.panel.category.164
            ("dict.prod.pcba.panel.category.164", "zh-HK", "mafad b", "pcba板位类别.mafad b"),

            // dict.prod.pcba.panel.category.165
            ("dict.prod.pcba.panel.category.165", "en-US", "ma-fad b", "pcba板位类别.ma-fad b"),
            // dict.prod.pcba.panel.category.165
            ("dict.prod.pcba.panel.category.165", "ja-JP", "ma-fad b", "pcba板位类别.ma-fad b"),
            // dict.prod.pcba.panel.category.165
            ("dict.prod.pcba.panel.category.165", "zh-CN", "ma-fad b", "pcba板位类别.ma-fad b"),
            // dict.prod.pcba.panel.category.165
            ("dict.prod.pcba.panel.category.165", "zh-HK", "ma-fad b", "pcba板位类别.ma-fad b"),

            // dict.prod.pcba.panel.category.166
            ("dict.prod.pcba.panel.category.166", "en-US", "mafad b/t", "pcba板位类别.mafad b/t"),
            // dict.prod.pcba.panel.category.166
            ("dict.prod.pcba.panel.category.166", "ja-JP", "mafad b/t", "pcba板位类别.mafad b/t"),
            // dict.prod.pcba.panel.category.166
            ("dict.prod.pcba.panel.category.166", "zh-CN", "mafad b/t", "pcba板位类别.mafad b/t"),
            // dict.prod.pcba.panel.category.166
            ("dict.prod.pcba.panel.category.166", "zh-HK", "mafad b/t", "pcba板位类别.mafad b/t"),

            // dict.prod.pcba.panel.category.167
            ("dict.prod.pcba.panel.category.167", "en-US", "ma-fad t", "pcba板位类别.ma-fad t"),
            // dict.prod.pcba.panel.category.167
            ("dict.prod.pcba.panel.category.167", "ja-JP", "ma-fad t", "pcba板位类别.ma-fad t"),
            // dict.prod.pcba.panel.category.167
            ("dict.prod.pcba.panel.category.167", "zh-CN", "ma-fad t", "pcba板位类别.ma-fad t"),
            // dict.prod.pcba.panel.category.167
            ("dict.prod.pcba.panel.category.167", "zh-HK", "ma-fad t", "pcba板位类别.ma-fad t"),

            // dict.prod.pcba.panel.category.168
            ("dict.prod.pcba.panel.category.168", "en-US", "main", "pcba板位类别.main"),
            // dict.prod.pcba.panel.category.168
            ("dict.prod.pcba.panel.category.168", "ja-JP", "main", "pcba板位类别.main"),
            // dict.prod.pcba.panel.category.168
            ("dict.prod.pcba.panel.category.168", "zh-CN", "main", "pcba板位类别.main"),
            // dict.prod.pcba.panel.category.168
            ("dict.prod.pcba.panel.category.168", "zh-HK", "main", "pcba板位类别.main"),

            // dict.prod.pcba.panel.category.171
            ("dict.prod.pcba.panel.category.171", "en-US", "main a", "pcba板位类别.main a"),
            // dict.prod.pcba.panel.category.171
            ("dict.prod.pcba.panel.category.171", "ja-JP", "main a", "pcba板位类别.main a"),
            // dict.prod.pcba.panel.category.171
            ("dict.prod.pcba.panel.category.171", "zh-CN", "main a", "pcba板位类别.main a"),
            // dict.prod.pcba.panel.category.171
            ("dict.prod.pcba.panel.category.171", "zh-HK", "main a", "pcba板位类别.main a"),

            // dict.prod.pcba.panel.category.172
            ("dict.prod.pcba.panel.category.172", "en-US", "main alt b", "pcba板位类别.main alt b"),
            // dict.prod.pcba.panel.category.172
            ("dict.prod.pcba.panel.category.172", "ja-JP", "main alt b", "pcba板位类别.main alt b"),
            // dict.prod.pcba.panel.category.172
            ("dict.prod.pcba.panel.category.172", "zh-CN", "main alt b", "pcba板位类别.main alt b"),
            // dict.prod.pcba.panel.category.172
            ("dict.prod.pcba.panel.category.172", "zh-HK", "main alt b", "pcba板位类别.main alt b"),

            // dict.prod.pcba.panel.category.173
            ("dict.prod.pcba.panel.category.173", "en-US", "main alt t", "pcba板位类别.main alt t"),
            // dict.prod.pcba.panel.category.173
            ("dict.prod.pcba.panel.category.173", "ja-JP", "main alt t", "pcba板位类别.main alt t"),
            // dict.prod.pcba.panel.category.173
            ("dict.prod.pcba.panel.category.173", "zh-CN", "main alt t", "pcba板位类别.main alt t"),
            // dict.prod.pcba.panel.category.173
            ("dict.prod.pcba.panel.category.173", "zh-HK", "main alt t", "pcba板位类别.main alt t"),

            // dict.prod.pcba.panel.category.174
            ("dict.prod.pcba.panel.category.174", "en-US", "main b", "pcba板位类别.main b"),
            // dict.prod.pcba.panel.category.174
            ("dict.prod.pcba.panel.category.174", "ja-JP", "main b", "pcba板位类别.main b"),
            // dict.prod.pcba.panel.category.174
            ("dict.prod.pcba.panel.category.174", "zh-CN", "main b", "pcba板位类别.main b"),
            // dict.prod.pcba.panel.category.174
            ("dict.prod.pcba.panel.category.174", "zh-HK", "main b", "pcba板位类别.main b"),

            // dict.prod.pcba.panel.category.175
            ("dict.prod.pcba.panel.category.175", "en-US", "main b/t", "pcba板位类别.main b/t"),
            // dict.prod.pcba.panel.category.175
            ("dict.prod.pcba.panel.category.175", "ja-JP", "main b/t", "pcba板位类别.main b/t"),
            // dict.prod.pcba.panel.category.175
            ("dict.prod.pcba.panel.category.175", "zh-CN", "main b/t", "pcba板位类别.main b/t"),
            // dict.prod.pcba.panel.category.175
            ("dict.prod.pcba.panel.category.175", "zh-HK", "main b/t", "pcba板位类别.main b/t"),

            // dict.prod.pcba.panel.category.176
            ("dict.prod.pcba.panel.category.176", "en-US", "mather b/t", "pcba板位类别.mather b/t"),
            // dict.prod.pcba.panel.category.176
            ("dict.prod.pcba.panel.category.176", "ja-JP", "mather b/t", "pcba板位类别.mather b/t"),
            // dict.prod.pcba.panel.category.176
            ("dict.prod.pcba.panel.category.176", "zh-CN", "mather b/t", "pcba板位类别.mather b/t"),
            // dict.prod.pcba.panel.category.176
            ("dict.prod.pcba.panel.category.176", "zh-HK", "mather b/t", "pcba板位类别.mather b/t"),

            // dict.prod.pcba.panel.category.179
            ("dict.prod.pcba.panel.category.179", "en-US", "meter", "pcba板位类别.meter"),
            // dict.prod.pcba.panel.category.179
            ("dict.prod.pcba.panel.category.179", "ja-JP", "meter", "pcba板位类别.meter"),
            // dict.prod.pcba.panel.category.179
            ("dict.prod.pcba.panel.category.179", "zh-CN", "meter", "pcba板位类别.meter"),
            // dict.prod.pcba.panel.category.179
            ("dict.prod.pcba.panel.category.179", "zh-HK", "meter", "pcba板位类别.meter"),

            // dict.prod.pcba.panel.category.180
            ("dict.prod.pcba.panel.category.180", "en-US", "mic", "pcba板位类别.mic"),
            // dict.prod.pcba.panel.category.180
            ("dict.prod.pcba.panel.category.180", "ja-JP", "mic", "pcba板位类别.mic"),
            // dict.prod.pcba.panel.category.180
            ("dict.prod.pcba.panel.category.180", "zh-CN", "mic", "pcba板位类别.mic"),
            // dict.prod.pcba.panel.category.180
            ("dict.prod.pcba.panel.category.180", "zh-HK", "mic", "pcba板位类别.mic"),

            // dict.prod.pcba.panel.category.181
            ("dict.prod.pcba.panel.category.181", "en-US", "naub b", "pcba板位类别.naub b"),
            // dict.prod.pcba.panel.category.181
            ("dict.prod.pcba.panel.category.181", "ja-JP", "naub b", "pcba板位类别.naub b"),
            // dict.prod.pcba.panel.category.181
            ("dict.prod.pcba.panel.category.181", "zh-CN", "naub b", "pcba板位类别.naub b"),
            // dict.prod.pcba.panel.category.181
            ("dict.prod.pcba.panel.category.181", "zh-HK", "naub b", "pcba板位类别.naub b"),

            // dict.prod.pcba.panel.category.182
            ("dict.prod.pcba.panel.category.182", "en-US", "panel", "pcba板位类别.panel"),
            // dict.prod.pcba.panel.category.182
            ("dict.prod.pcba.panel.category.182", "ja-JP", "panel", "pcba板位类别.panel"),
            // dict.prod.pcba.panel.category.182
            ("dict.prod.pcba.panel.category.182", "zh-CN", "panel", "pcba板位类别.panel"),
            // dict.prod.pcba.panel.category.182
            ("dict.prod.pcba.panel.category.182", "zh-HK", "panel", "pcba板位类别.panel"),

            // dict.prod.pcba.panel.category.183
            ("dict.prod.pcba.panel.category.183", "en-US", "panel a", "pcba板位类别.panel a"),
            // dict.prod.pcba.panel.category.183
            ("dict.prod.pcba.panel.category.183", "ja-JP", "panel a", "pcba板位类别.panel a"),
            // dict.prod.pcba.panel.category.183
            ("dict.prod.pcba.panel.category.183", "zh-CN", "panel a", "pcba板位类别.panel a"),
            // dict.prod.pcba.panel.category.183
            ("dict.prod.pcba.panel.category.183", "zh-HK", "panel a", "pcba板位类别.panel a"),

            // dict.prod.pcba.panel.category.184
            ("dict.prod.pcba.panel.category.184", "en-US", "panel b", "pcba板位类别.panel b"),
            // dict.prod.pcba.panel.category.184
            ("dict.prod.pcba.panel.category.184", "ja-JP", "panel b", "pcba板位类别.panel b"),
            // dict.prod.pcba.panel.category.184
            ("dict.prod.pcba.panel.category.184", "zh-CN", "panel b", "pcba板位类别.panel b"),
            // dict.prod.pcba.panel.category.184
            ("dict.prod.pcba.panel.category.184", "zh-HK", "panel b", "pcba板位类别.panel b"),

            // dict.prod.pcba.panel.category.185
            ("dict.prod.pcba.panel.category.185", "en-US", "panel b/t", "pcba板位类别.panel b/t"),
            // dict.prod.pcba.panel.category.185
            ("dict.prod.pcba.panel.category.185", "ja-JP", "panel b/t", "pcba板位类别.panel b/t"),
            // dict.prod.pcba.panel.category.185
            ("dict.prod.pcba.panel.category.185", "zh-CN", "panel b/t", "pcba板位类别.panel b/t"),
            // dict.prod.pcba.panel.category.185
            ("dict.prod.pcba.panel.category.185", "zh-HK", "panel b/t", "pcba板位类别.panel b/t"),

            // dict.prod.pcba.panel.category.186
            ("dict.prod.pcba.panel.category.186", "en-US", "panel l", "pcba板位类别.panel l"),
            // dict.prod.pcba.panel.category.186
            ("dict.prod.pcba.panel.category.186", "ja-JP", "panel l", "pcba板位类别.panel l"),
            // dict.prod.pcba.panel.category.186
            ("dict.prod.pcba.panel.category.186", "zh-CN", "panel l", "pcba板位类别.panel l"),
            // dict.prod.pcba.panel.category.186
            ("dict.prod.pcba.panel.category.186", "zh-HK", "panel l", "pcba板位类别.panel l"),

            // dict.prod.pcba.panel.category.187
            ("dict.prod.pcba.panel.category.187", "en-US", "panel r", "pcba板位类别.panel r"),
            // dict.prod.pcba.panel.category.187
            ("dict.prod.pcba.panel.category.187", "ja-JP", "panel r", "pcba板位类别.panel r"),
            // dict.prod.pcba.panel.category.187
            ("dict.prod.pcba.panel.category.187", "zh-CN", "panel r", "pcba板位类别.panel r"),
            // dict.prod.pcba.panel.category.187
            ("dict.prod.pcba.panel.category.187", "zh-HK", "panel r", "pcba板位类别.panel r"),

            // dict.prod.pcba.panel.category.188
            ("dict.prod.pcba.panel.category.188", "en-US", "panel t", "pcba板位类别.panel t"),
            // dict.prod.pcba.panel.category.188
            ("dict.prod.pcba.panel.category.188", "ja-JP", "panel t", "pcba板位类别.panel t"),
            // dict.prod.pcba.panel.category.188
            ("dict.prod.pcba.panel.category.188", "zh-CN", "panel t", "pcba板位类别.panel t"),
            // dict.prod.pcba.panel.category.188
            ("dict.prod.pcba.panel.category.188", "zh-HK", "panel t", "pcba板位类别.panel t"),

            // dict.prod.pcba.panel.category.189
            ("dict.prod.pcba.panel.category.189", "en-US", "phone", "pcba板位类别.phone"),
            // dict.prod.pcba.panel.category.189
            ("dict.prod.pcba.panel.category.189", "ja-JP", "phone", "pcba板位类别.phone"),
            // dict.prod.pcba.panel.category.189
            ("dict.prod.pcba.panel.category.189", "zh-CN", "phone", "pcba板位类别.phone"),
            // dict.prod.pcba.panel.category.189
            ("dict.prod.pcba.panel.category.189", "zh-HK", "phone", "pcba板位类别.phone"),

            // dict.prod.pcba.panel.category.190
            ("dict.prod.pcba.panel.category.190", "en-US", "power", "pcba板位类别.power"),
            // dict.prod.pcba.panel.category.190
            ("dict.prod.pcba.panel.category.190", "ja-JP", "power", "pcba板位类别.power"),
            // dict.prod.pcba.panel.category.190
            ("dict.prod.pcba.panel.category.190", "zh-CN", "power", "pcba板位类别.power"),
            // dict.prod.pcba.panel.category.190
            ("dict.prod.pcba.panel.category.190", "zh-HK", "power", "pcba板位类别.power"),

            // dict.prod.pcba.panel.category.191
            ("dict.prod.pcba.panel.category.191", "en-US", "power a", "pcba板位类别.power a"),
            // dict.prod.pcba.panel.category.191
            ("dict.prod.pcba.panel.category.191", "ja-JP", "power a", "pcba板位类别.power a"),
            // dict.prod.pcba.panel.category.191
            ("dict.prod.pcba.panel.category.191", "zh-CN", "power a", "pcba板位类别.power a"),
            // dict.prod.pcba.panel.category.191
            ("dict.prod.pcba.panel.category.191", "zh-HK", "power a", "pcba板位类别.power a"),

            // dict.prod.pcba.panel.category.192
            ("dict.prod.pcba.panel.category.192", "en-US", "power b", "pcba板位类别.power b"),
            // dict.prod.pcba.panel.category.192
            ("dict.prod.pcba.panel.category.192", "ja-JP", "power b", "pcba板位类别.power b"),
            // dict.prod.pcba.panel.category.192
            ("dict.prod.pcba.panel.category.192", "zh-CN", "power b", "pcba板位类别.power b"),
            // dict.prod.pcba.panel.category.192
            ("dict.prod.pcba.panel.category.192", "zh-HK", "power b", "pcba板位类别.power b"),

            // dict.prod.pcba.panel.category.193
            ("dict.prod.pcba.panel.category.193", "en-US", "power b/t", "pcba板位类别.power b/t"),
            // dict.prod.pcba.panel.category.193
            ("dict.prod.pcba.panel.category.193", "ja-JP", "power b/t", "pcba板位类别.power b/t"),
            // dict.prod.pcba.panel.category.193
            ("dict.prod.pcba.panel.category.193", "zh-CN", "power b/t", "pcba板位类别.power b/t"),
            // dict.prod.pcba.panel.category.193
            ("dict.prod.pcba.panel.category.193", "zh-HK", "power b/t", "pcba板位类别.power b/t"),

            // dict.prod.pcba.panel.category.194
            ("dict.prod.pcba.panel.category.194", "en-US", "power t", "pcba板位类别.power t"),
            // dict.prod.pcba.panel.category.194
            ("dict.prod.pcba.panel.category.194", "ja-JP", "power t", "pcba板位类别.power t"),
            // dict.prod.pcba.panel.category.194
            ("dict.prod.pcba.panel.category.194", "zh-CN", "power t", "pcba板位类别.power t"),
            // dict.prod.pcba.panel.category.194
            ("dict.prod.pcba.panel.category.194", "zh-HK", "power t", "pcba板位类别.power t"),

            // dict.prod.pcba.panel.category.195
            ("dict.prod.pcba.panel.category.195", "en-US", "prm b", "pcba板位类别.prm b"),
            // dict.prod.pcba.panel.category.195
            ("dict.prod.pcba.panel.category.195", "ja-JP", "prm b", "pcba板位类别.prm b"),
            // dict.prod.pcba.panel.category.195
            ("dict.prod.pcba.panel.category.195", "zh-CN", "prm b", "pcba板位类别.prm b"),
            // dict.prod.pcba.panel.category.195
            ("dict.prod.pcba.panel.category.195", "zh-HK", "prm b", "pcba板位类别.prm b"),

            // dict.prod.pcba.panel.category.196
            ("dict.prod.pcba.panel.category.196", "en-US", "prm b/t", "pcba板位类别.prm b/t"),
            // dict.prod.pcba.panel.category.196
            ("dict.prod.pcba.panel.category.196", "ja-JP", "prm b/t", "pcba板位类别.prm b/t"),
            // dict.prod.pcba.panel.category.196
            ("dict.prod.pcba.panel.category.196", "zh-CN", "prm b/t", "pcba板位类别.prm b/t"),
            // dict.prod.pcba.panel.category.196
            ("dict.prod.pcba.panel.category.196", "zh-HK", "prm b/t", "pcba板位类别.prm b/t"),

            // dict.prod.pcba.panel.category.197
            ("dict.prod.pcba.panel.category.197", "en-US", "prm t", "pcba板位类别.prm t"),
            // dict.prod.pcba.panel.category.197
            ("dict.prod.pcba.panel.category.197", "ja-JP", "prm t", "pcba板位类别.prm t"),
            // dict.prod.pcba.panel.category.197
            ("dict.prod.pcba.panel.category.197", "zh-CN", "prm t", "pcba板位类别.prm t"),
            // dict.prod.pcba.panel.category.197
            ("dict.prod.pcba.panel.category.197", "zh-HK", "prm t", "pcba板位类别.prm t"),

            // dict.prod.pcba.panel.category.198
            ("dict.prod.pcba.panel.category.198", "en-US", "psl", "pcba板位类别.psl"),
            // dict.prod.pcba.panel.category.198
            ("dict.prod.pcba.panel.category.198", "ja-JP", "psl", "pcba板位类别.psl"),
            // dict.prod.pcba.panel.category.198
            ("dict.prod.pcba.panel.category.198", "zh-CN", "psl", "pcba板位类别.psl"),
            // dict.prod.pcba.panel.category.198
            ("dict.prod.pcba.panel.category.198", "zh-HK", "psl", "pcba板位类别.psl"),

            // dict.prod.pcba.panel.category.199
            ("dict.prod.pcba.panel.category.199", "en-US", "psl b", "pcba板位类别.psl b"),
            // dict.prod.pcba.panel.category.199
            ("dict.prod.pcba.panel.category.199", "ja-JP", "psl b", "pcba板位类别.psl b"),
            // dict.prod.pcba.panel.category.199
            ("dict.prod.pcba.panel.category.199", "zh-CN", "psl b", "pcba板位类别.psl b"),
            // dict.prod.pcba.panel.category.199
            ("dict.prod.pcba.panel.category.199", "zh-HK", "psl b", "pcba板位类别.psl b"),

            // dict.prod.pcba.panel.category.200
            ("dict.prod.pcba.panel.category.200", "en-US", "psl b/t", "pcba板位类别.psl b/t"),
            // dict.prod.pcba.panel.category.200
            ("dict.prod.pcba.panel.category.200", "ja-JP", "psl b/t", "pcba板位类别.psl b/t"),
            // dict.prod.pcba.panel.category.200
            ("dict.prod.pcba.panel.category.200", "zh-CN", "psl b/t", "pcba板位类别.psl b/t"),
            // dict.prod.pcba.panel.category.200
            ("dict.prod.pcba.panel.category.200", "zh-HK", "psl b/t", "pcba板位类别.psl b/t"),

            // dict.prod.pcba.panel.category.201
            ("dict.prod.pcba.panel.category.201", "en-US", "psl t", "pcba板位类别.psl t"),
            // dict.prod.pcba.panel.category.201
            ("dict.prod.pcba.panel.category.201", "ja-JP", "psl t", "pcba板位类别.psl t"),
            // dict.prod.pcba.panel.category.201
            ("dict.prod.pcba.panel.category.201", "zh-CN", "psl t", "pcba板位类别.psl t"),
            // dict.prod.pcba.panel.category.201
            ("dict.prod.pcba.panel.category.201", "zh-HK", "psl t", "pcba板位类别.psl t"),

            // dict.prod.pcba.panel.category.202
            ("dict.prod.pcba.panel.category.202", "en-US", "ptst", "pcba板位类别.ptst"),
            // dict.prod.pcba.panel.category.202
            ("dict.prod.pcba.panel.category.202", "ja-JP", "ptst", "pcba板位类别.ptst"),
            // dict.prod.pcba.panel.category.202
            ("dict.prod.pcba.panel.category.202", "zh-CN", "ptst", "pcba板位类别.ptst"),
            // dict.prod.pcba.panel.category.202
            ("dict.prod.pcba.panel.category.202", "zh-HK", "ptst", "pcba板位类别.ptst"),

            // dict.prod.pcba.panel.category.203
            ("dict.prod.pcba.panel.category.203", "en-US", "ptst b", "pcba板位类别.ptst b"),
            // dict.prod.pcba.panel.category.203
            ("dict.prod.pcba.panel.category.203", "ja-JP", "ptst b", "pcba板位类别.ptst b"),
            // dict.prod.pcba.panel.category.203
            ("dict.prod.pcba.panel.category.203", "zh-CN", "ptst b", "pcba板位类别.ptst b"),
            // dict.prod.pcba.panel.category.203
            ("dict.prod.pcba.panel.category.203", "zh-HK", "ptst b", "pcba板位类别.ptst b"),

            // dict.prod.pcba.panel.category.204
            ("dict.prod.pcba.panel.category.204", "en-US", "ptst b/t", "pcba板位类别.ptst b/t"),
            // dict.prod.pcba.panel.category.204
            ("dict.prod.pcba.panel.category.204", "ja-JP", "ptst b/t", "pcba板位类别.ptst b/t"),
            // dict.prod.pcba.panel.category.204
            ("dict.prod.pcba.panel.category.204", "zh-CN", "ptst b/t", "pcba板位类别.ptst b/t"),
            // dict.prod.pcba.panel.category.204
            ("dict.prod.pcba.panel.category.204", "zh-HK", "ptst b/t", "pcba板位类别.ptst b/t"),

            // dict.prod.pcba.panel.category.205
            ("dict.prod.pcba.panel.category.205", "en-US", "ptst t", "pcba板位类别.ptst t"),
            // dict.prod.pcba.panel.category.205
            ("dict.prod.pcba.panel.category.205", "ja-JP", "ptst t", "pcba板位类别.ptst t"),
            // dict.prod.pcba.panel.category.205
            ("dict.prod.pcba.panel.category.205", "zh-CN", "ptst t", "pcba板位类别.ptst t"),
            // dict.prod.pcba.panel.category.205
            ("dict.prod.pcba.panel.category.205", "zh-HK", "ptst t", "pcba板位类别.ptst t"),

            // dict.prod.pcba.panel.category.206
            ("dict.prod.pcba.panel.category.206", "en-US", "pwrsub", "pcba板位类别.pwrsub"),
            // dict.prod.pcba.panel.category.206
            ("dict.prod.pcba.panel.category.206", "ja-JP", "pwrsub", "pcba板位类别.pwrsub"),
            // dict.prod.pcba.panel.category.206
            ("dict.prod.pcba.panel.category.206", "zh-CN", "pwrsub", "pcba板位类别.pwrsub"),
            // dict.prod.pcba.panel.category.206
            ("dict.prod.pcba.panel.category.206", "zh-HK", "pwrsub", "pcba板位类别.pwrsub"),

            // dict.prod.pcba.panel.category.207
            ("dict.prod.pcba.panel.category.207", "en-US", "rear", "pcba板位类别.rear"),
            // dict.prod.pcba.panel.category.207
            ("dict.prod.pcba.panel.category.207", "ja-JP", "rear", "pcba板位类别.rear"),
            // dict.prod.pcba.panel.category.207
            ("dict.prod.pcba.panel.category.207", "zh-CN", "rear", "pcba板位类别.rear"),
            // dict.prod.pcba.panel.category.207
            ("dict.prod.pcba.panel.category.207", "zh-HK", "rear", "pcba板位类别.rear"),

            // dict.prod.pcba.panel.category.208
            ("dict.prod.pcba.panel.category.208", "en-US", "rear a", "pcba板位类别.rear a"),
            // dict.prod.pcba.panel.category.208
            ("dict.prod.pcba.panel.category.208", "ja-JP", "rear a", "pcba板位类别.rear a"),
            // dict.prod.pcba.panel.category.208
            ("dict.prod.pcba.panel.category.208", "zh-CN", "rear a", "pcba板位类别.rear a"),
            // dict.prod.pcba.panel.category.208
            ("dict.prod.pcba.panel.category.208", "zh-HK", "rear a", "pcba板位类别.rear a"),

            // dict.prod.pcba.panel.category.209
            ("dict.prod.pcba.panel.category.209", "en-US", "rear b", "pcba板位类别.rear b"),
            // dict.prod.pcba.panel.category.209
            ("dict.prod.pcba.panel.category.209", "ja-JP", "rear b", "pcba板位类别.rear b"),
            // dict.prod.pcba.panel.category.209
            ("dict.prod.pcba.panel.category.209", "zh-CN", "rear b", "pcba板位类别.rear b"),
            // dict.prod.pcba.panel.category.209
            ("dict.prod.pcba.panel.category.209", "zh-HK", "rear b", "pcba板位类别.rear b"),

            // dict.prod.pcba.panel.category.210
            ("dict.prod.pcba.panel.category.210", "en-US", "rear t", "pcba板位类别.rear t"),
            // dict.prod.pcba.panel.category.210
            ("dict.prod.pcba.panel.category.210", "ja-JP", "rear t", "pcba板位类别.rear t"),
            // dict.prod.pcba.panel.category.210
            ("dict.prod.pcba.panel.category.210", "zh-CN", "rear t", "pcba板位类别.rear t"),
            // dict.prod.pcba.panel.category.210
            ("dict.prod.pcba.panel.category.210", "zh-HK", "rear t", "pcba板位类别.rear t"),

            // dict.prod.pcba.panel.category.211
            ("dict.prod.pcba.panel.category.211", "en-US", "relay", "pcba板位类别.relay"),
            // dict.prod.pcba.panel.category.211
            ("dict.prod.pcba.panel.category.211", "ja-JP", "relay", "pcba板位类别.relay"),
            // dict.prod.pcba.panel.category.211
            ("dict.prod.pcba.panel.category.211", "zh-CN", "relay", "pcba板位类别.relay"),
            // dict.prod.pcba.panel.category.211
            ("dict.prod.pcba.panel.category.211", "zh-HK", "relay", "pcba板位类别.relay"),

            // dict.prod.pcba.panel.category.212
            ("dict.prod.pcba.panel.category.212", "en-US", "rfp a", "pcba板位类别.rfp a"),
            // dict.prod.pcba.panel.category.212
            ("dict.prod.pcba.panel.category.212", "ja-JP", "rfp a", "pcba板位类别.rfp a"),
            // dict.prod.pcba.panel.category.212
            ("dict.prod.pcba.panel.category.212", "zh-CN", "rfp a", "pcba板位类别.rfp a"),
            // dict.prod.pcba.panel.category.212
            ("dict.prod.pcba.panel.category.212", "zh-HK", "rfp a", "pcba板位类别.rfp a"),

            // dict.prod.pcba.panel.category.213
            ("dict.prod.pcba.panel.category.213", "en-US", "rfp b", "pcba板位类别.rfp b"),
            // dict.prod.pcba.panel.category.213
            ("dict.prod.pcba.panel.category.213", "ja-JP", "rfp b", "pcba板位类别.rfp b"),
            // dict.prod.pcba.panel.category.213
            ("dict.prod.pcba.panel.category.213", "zh-CN", "rfp b", "pcba板位类别.rfp b"),
            // dict.prod.pcba.panel.category.213
            ("dict.prod.pcba.panel.category.213", "zh-HK", "rfp b", "pcba板位类别.rfp b"),

            // dict.prod.pcba.panel.category.214
            ("dict.prod.pcba.panel.category.214", "en-US", "rfp b/t", "pcba板位类别.rfp b/t"),
            // dict.prod.pcba.panel.category.214
            ("dict.prod.pcba.panel.category.214", "ja-JP", "rfp b/t", "pcba板位类别.rfp b/t"),
            // dict.prod.pcba.panel.category.214
            ("dict.prod.pcba.panel.category.214", "zh-CN", "rfp b/t", "pcba板位类别.rfp b/t"),
            // dict.prod.pcba.panel.category.214
            ("dict.prod.pcba.panel.category.214", "zh-HK", "rfp b/t", "pcba板位类别.rfp b/t"),

            // dict.prod.pcba.panel.category.215
            ("dict.prod.pcba.panel.category.215", "en-US", "rfp t", "pcba板位类别.rfp t"),
            // dict.prod.pcba.panel.category.215
            ("dict.prod.pcba.panel.category.215", "ja-JP", "rfp t", "pcba板位类别.rfp t"),
            // dict.prod.pcba.panel.category.215
            ("dict.prod.pcba.panel.category.215", "zh-CN", "rfp t", "pcba板位类别.rfp t"),
            // dict.prod.pcba.panel.category.215
            ("dict.prod.pcba.panel.category.215", "zh-HK", "rfp t", "pcba板位类别.rfp t"),

            // dict.prod.pcba.panel.category.216
            ("dict.prod.pcba.panel.category.216", "en-US", "rmn b", "pcba板位类别.rmn b"),
            // dict.prod.pcba.panel.category.216
            ("dict.prod.pcba.panel.category.216", "ja-JP", "rmn b", "pcba板位类别.rmn b"),
            // dict.prod.pcba.panel.category.216
            ("dict.prod.pcba.panel.category.216", "zh-CN", "rmn b", "pcba板位类别.rmn b"),
            // dict.prod.pcba.panel.category.216
            ("dict.prod.pcba.panel.category.216", "zh-HK", "rmn b", "pcba板位类别.rmn b"),

            // dict.prod.pcba.panel.category.217
            ("dict.prod.pcba.panel.category.217", "en-US", "rmn b/t", "pcba板位类别.rmn b/t"),
            // dict.prod.pcba.panel.category.217
            ("dict.prod.pcba.panel.category.217", "ja-JP", "rmn b/t", "pcba板位类别.rmn b/t"),
            // dict.prod.pcba.panel.category.217
            ("dict.prod.pcba.panel.category.217", "zh-CN", "rmn b/t", "pcba板位类别.rmn b/t"),
            // dict.prod.pcba.panel.category.217
            ("dict.prod.pcba.panel.category.217", "zh-HK", "rmn b/t", "pcba板位类别.rmn b/t"),

            // dict.prod.pcba.panel.category.218
            ("dict.prod.pcba.panel.category.218", "en-US", "rmn t", "pcba板位类别.rmn t"),
            // dict.prod.pcba.panel.category.218
            ("dict.prod.pcba.panel.category.218", "ja-JP", "rmn t", "pcba板位类别.rmn t"),
            // dict.prod.pcba.panel.category.218
            ("dict.prod.pcba.panel.category.218", "zh-CN", "rmn t", "pcba板位类别.rmn t"),
            // dict.prod.pcba.panel.category.218
            ("dict.prod.pcba.panel.category.218", "zh-HK", "rmn t", "pcba板位类别.rmn t"),

            // dict.prod.pcba.panel.category.219
            ("dict.prod.pcba.panel.category.219", "en-US", "rmt", "pcba板位类别.rmt"),
            // dict.prod.pcba.panel.category.219
            ("dict.prod.pcba.panel.category.219", "ja-JP", "rmt", "pcba板位类别.rmt"),
            // dict.prod.pcba.panel.category.219
            ("dict.prod.pcba.panel.category.219", "zh-CN", "rmt", "pcba板位类别.rmt"),
            // dict.prod.pcba.panel.category.219
            ("dict.prod.pcba.panel.category.219", "zh-HK", "rmt", "pcba板位类别.rmt"),

            // dict.prod.pcba.panel.category.220
            ("dict.prod.pcba.panel.category.220", "en-US", "rsb b", "pcba板位类别.rsb b"),
            // dict.prod.pcba.panel.category.220
            ("dict.prod.pcba.panel.category.220", "ja-JP", "rsb b", "pcba板位类别.rsb b"),
            // dict.prod.pcba.panel.category.220
            ("dict.prod.pcba.panel.category.220", "zh-CN", "rsb b", "pcba板位类别.rsb b"),
            // dict.prod.pcba.panel.category.220
            ("dict.prod.pcba.panel.category.220", "zh-HK", "rsb b", "pcba板位类别.rsb b"),

            // dict.prod.pcba.panel.category.221
            ("dict.prod.pcba.panel.category.221", "en-US", "rsb b/t", "pcba板位类别.rsb b/t"),
            // dict.prod.pcba.panel.category.221
            ("dict.prod.pcba.panel.category.221", "ja-JP", "rsb b/t", "pcba板位类别.rsb b/t"),
            // dict.prod.pcba.panel.category.221
            ("dict.prod.pcba.panel.category.221", "zh-CN", "rsb b/t", "pcba板位类别.rsb b/t"),
            // dict.prod.pcba.panel.category.221
            ("dict.prod.pcba.panel.category.221", "zh-HK", "rsb b/t", "pcba板位类别.rsb b/t"),

            // dict.prod.pcba.panel.category.222
            ("dict.prod.pcba.panel.category.222", "en-US", "rsb t", "pcba板位类别.rsb t"),
            // dict.prod.pcba.panel.category.222
            ("dict.prod.pcba.panel.category.222", "ja-JP", "rsb t", "pcba板位类别.rsb t"),
            // dict.prod.pcba.panel.category.222
            ("dict.prod.pcba.panel.category.222", "zh-CN", "rsb t", "pcba板位类别.rsb t"),
            // dict.prod.pcba.panel.category.222
            ("dict.prod.pcba.panel.category.222", "zh-HK", "rsb t", "pcba板位类别.rsb t"),

            // dict.prod.pcba.panel.category.223
            ("dict.prod.pcba.panel.category.223", "en-US", "sata", "pcba板位类别.sata"),
            // dict.prod.pcba.panel.category.223
            ("dict.prod.pcba.panel.category.223", "ja-JP", "sata", "pcba板位类别.sata"),
            // dict.prod.pcba.panel.category.223
            ("dict.prod.pcba.panel.category.223", "zh-CN", "sata", "pcba板位类别.sata"),
            // dict.prod.pcba.panel.category.223
            ("dict.prod.pcba.panel.category.223", "zh-HK", "sata", "pcba板位类别.sata"),

            // dict.prod.pcba.panel.category.224
            ("dict.prod.pcba.panel.category.224", "en-US", "sbty", "pcba板位类别.sbty"),
            // dict.prod.pcba.panel.category.224
            ("dict.prod.pcba.panel.category.224", "ja-JP", "sbty", "pcba板位类别.sbty"),
            // dict.prod.pcba.panel.category.224
            ("dict.prod.pcba.panel.category.224", "zh-CN", "sbty", "pcba板位类别.sbty"),
            // dict.prod.pcba.panel.category.224
            ("dict.prod.pcba.panel.category.224", "zh-HK", "sbty", "pcba板位类别.sbty"),

            // dict.prod.pcba.panel.category.225
            ("dict.prod.pcba.panel.category.225", "en-US", "seq", "pcba板位类别.seq"),
            // dict.prod.pcba.panel.category.225
            ("dict.prod.pcba.panel.category.225", "ja-JP", "seq", "pcba板位类别.seq"),
            // dict.prod.pcba.panel.category.225
            ("dict.prod.pcba.panel.category.225", "zh-CN", "seq", "pcba板位类别.seq"),
            // dict.prod.pcba.panel.category.225
            ("dict.prod.pcba.panel.category.225", "zh-HK", "seq", "pcba板位类别.seq"),

            // dict.prod.pcba.panel.category.226
            ("dict.prod.pcba.panel.category.226", "en-US", "slot", "pcba板位类别.slot"),
            // dict.prod.pcba.panel.category.226
            ("dict.prod.pcba.panel.category.226", "ja-JP", "slot", "pcba板位类别.slot"),
            // dict.prod.pcba.panel.category.226
            ("dict.prod.pcba.panel.category.226", "zh-CN", "slot", "pcba板位类别.slot"),
            // dict.prod.pcba.panel.category.226
            ("dict.prod.pcba.panel.category.226", "zh-HK", "slot", "pcba板位类别.slot"),

            // dict.prod.pcba.panel.category.227
            ("dict.prod.pcba.panel.category.227", "en-US", "slot a", "pcba板位类别.slot a"),
            // dict.prod.pcba.panel.category.227
            ("dict.prod.pcba.panel.category.227", "ja-JP", "slot a", "pcba板位类别.slot a"),
            // dict.prod.pcba.panel.category.227
            ("dict.prod.pcba.panel.category.227", "zh-CN", "slot a", "pcba板位类别.slot a"),
            // dict.prod.pcba.panel.category.227
            ("dict.prod.pcba.panel.category.227", "zh-HK", "slot a", "pcba板位类别.slot a"),

            // dict.prod.pcba.panel.category.228
            ("dict.prod.pcba.panel.category.228", "en-US", "slot b", "pcba板位类别.slot b"),
            // dict.prod.pcba.panel.category.228
            ("dict.prod.pcba.panel.category.228", "ja-JP", "slot b", "pcba板位类别.slot b"),
            // dict.prod.pcba.panel.category.228
            ("dict.prod.pcba.panel.category.228", "zh-CN", "slot b", "pcba板位类别.slot b"),
            // dict.prod.pcba.panel.category.228
            ("dict.prod.pcba.panel.category.228", "zh-HK", "slot b", "pcba板位类别.slot b"),

            // dict.prod.pcba.panel.category.229
            ("dict.prod.pcba.panel.category.229", "en-US", "slot b/t", "pcba板位类别.slot b/t"),
            // dict.prod.pcba.panel.category.229
            ("dict.prod.pcba.panel.category.229", "ja-JP", "slot b/t", "pcba板位类别.slot b/t"),
            // dict.prod.pcba.panel.category.229
            ("dict.prod.pcba.panel.category.229", "zh-CN", "slot b/t", "pcba板位类别.slot b/t"),
            // dict.prod.pcba.panel.category.229
            ("dict.prod.pcba.panel.category.229", "zh-HK", "slot b/t", "pcba板位类别.slot b/t"),

            // dict.prod.pcba.panel.category.230
            ("dict.prod.pcba.panel.category.230", "en-US", "slot t", "pcba板位类别.slot t"),
            // dict.prod.pcba.panel.category.230
            ("dict.prod.pcba.panel.category.230", "ja-JP", "slot t", "pcba板位类别.slot t"),
            // dict.prod.pcba.panel.category.230
            ("dict.prod.pcba.panel.category.230", "zh-CN", "slot t", "pcba板位类别.slot t"),
            // dict.prod.pcba.panel.category.230
            ("dict.prod.pcba.panel.category.230", "zh-HK", "slot t", "pcba板位类别.slot t"),

            // dict.prod.pcba.panel.category.231
            ("dict.prod.pcba.panel.category.231", "en-US", "spl t", "pcba板位类别.spl t"),
            // dict.prod.pcba.panel.category.231
            ("dict.prod.pcba.panel.category.231", "ja-JP", "spl t", "pcba板位类别.spl t"),
            // dict.prod.pcba.panel.category.231
            ("dict.prod.pcba.panel.category.231", "zh-CN", "spl t", "pcba板位类别.spl t"),
            // dict.prod.pcba.panel.category.231
            ("dict.prod.pcba.panel.category.231", "zh-HK", "spl t", "pcba板位类别.spl t"),

            // dict.prod.pcba.panel.category.232
            ("dict.prod.pcba.panel.category.232", "en-US", "stby", "pcba板位类别.stby"),
            // dict.prod.pcba.panel.category.232
            ("dict.prod.pcba.panel.category.232", "ja-JP", "stby", "pcba板位类别.stby"),
            // dict.prod.pcba.panel.category.232
            ("dict.prod.pcba.panel.category.232", "zh-CN", "stby", "pcba板位类别.stby"),
            // dict.prod.pcba.panel.category.232
            ("dict.prod.pcba.panel.category.232", "zh-HK", "stby", "pcba板位类别.stby"),

            // dict.prod.pcba.panel.category.233
            ("dict.prod.pcba.panel.category.233", "en-US", "sts b", "pcba板位类别.sts b"),
            // dict.prod.pcba.panel.category.233
            ("dict.prod.pcba.panel.category.233", "ja-JP", "sts b", "pcba板位类别.sts b"),
            // dict.prod.pcba.panel.category.233
            ("dict.prod.pcba.panel.category.233", "zh-CN", "sts b", "pcba板位类别.sts b"),
            // dict.prod.pcba.panel.category.233
            ("dict.prod.pcba.panel.category.233", "zh-HK", "sts b", "pcba板位类别.sts b"),

            // dict.prod.pcba.panel.category.234
            ("dict.prod.pcba.panel.category.234", "en-US", "swusb", "pcba板位类别.swusb"),
            // dict.prod.pcba.panel.category.234
            ("dict.prod.pcba.panel.category.234", "ja-JP", "swusb", "pcba板位类别.swusb"),
            // dict.prod.pcba.panel.category.234
            ("dict.prod.pcba.panel.category.234", "zh-CN", "swusb", "pcba板位类别.swusb"),
            // dict.prod.pcba.panel.category.234
            ("dict.prod.pcba.panel.category.234", "zh-HK", "swusb", "pcba板位类别.swusb"),

            // dict.prod.pcba.panel.category.235
            ("dict.prod.pcba.panel.category.235", "en-US", "swusb akm b", "pcba板位类别.swusb akm b"),
            // dict.prod.pcba.panel.category.235
            ("dict.prod.pcba.panel.category.235", "ja-JP", "swusb akm b", "pcba板位类别.swusb akm b"),
            // dict.prod.pcba.panel.category.235
            ("dict.prod.pcba.panel.category.235", "zh-CN", "swusb akm b", "pcba板位类别.swusb akm b"),
            // dict.prod.pcba.panel.category.235
            ("dict.prod.pcba.panel.category.235", "zh-HK", "swusb akm b", "pcba板位类别.swusb akm b"),

            // dict.prod.pcba.panel.category.236
            ("dict.prod.pcba.panel.category.236", "en-US", "swusb akm b/t", "pcba板位类别.swusb akm b/t"),
            // dict.prod.pcba.panel.category.236
            ("dict.prod.pcba.panel.category.236", "ja-JP", "swusb akm b/t", "pcba板位类别.swusb akm b/t"),
            // dict.prod.pcba.panel.category.236
            ("dict.prod.pcba.panel.category.236", "zh-CN", "swusb akm b/t", "pcba板位类别.swusb akm b/t"),
            // dict.prod.pcba.panel.category.236
            ("dict.prod.pcba.panel.category.236", "zh-HK", "swusb akm b/t", "pcba板位类别.swusb akm b/t"),

            // dict.prod.pcba.panel.category.237
            ("dict.prod.pcba.panel.category.237", "en-US", "swusb akm t", "pcba板位类别.swusb akm t"),
            // dict.prod.pcba.panel.category.237
            ("dict.prod.pcba.panel.category.237", "ja-JP", "swusb akm t", "pcba板位类别.swusb akm t"),
            // dict.prod.pcba.panel.category.237
            ("dict.prod.pcba.panel.category.237", "zh-CN", "swusb akm t", "pcba板位类别.swusb akm t"),
            // dict.prod.pcba.panel.category.237
            ("dict.prod.pcba.panel.category.237", "zh-HK", "swusb akm t", "pcba板位类别.swusb akm t"),

            // dict.prod.pcba.panel.category.238
            ("dict.prod.pcba.panel.category.238", "en-US", "swusb b", "pcba板位类别.swusb b"),
            // dict.prod.pcba.panel.category.238
            ("dict.prod.pcba.panel.category.238", "ja-JP", "swusb b", "pcba板位类别.swusb b"),
            // dict.prod.pcba.panel.category.238
            ("dict.prod.pcba.panel.category.238", "zh-CN", "swusb b", "pcba板位类别.swusb b"),
            // dict.prod.pcba.panel.category.238
            ("dict.prod.pcba.panel.category.238", "zh-HK", "swusb b", "pcba板位类别.swusb b"),

            // dict.prod.pcba.panel.category.239
            ("dict.prod.pcba.panel.category.239", "en-US", "swusb b/t", "pcba板位类别.swusb b/t"),
            // dict.prod.pcba.panel.category.239
            ("dict.prod.pcba.panel.category.239", "ja-JP", "swusb b/t", "pcba板位类别.swusb b/t"),
            // dict.prod.pcba.panel.category.239
            ("dict.prod.pcba.panel.category.239", "zh-CN", "swusb b/t", "pcba板位类别.swusb b/t"),
            // dict.prod.pcba.panel.category.239
            ("dict.prod.pcba.panel.category.239", "zh-HK", "swusb b/t", "pcba板位类别.swusb b/t"),

            // dict.prod.pcba.panel.category.240
            ("dict.prod.pcba.panel.category.240", "en-US", "swusb t", "pcba板位类别.swusb t"),
            // dict.prod.pcba.panel.category.240
            ("dict.prod.pcba.panel.category.240", "ja-JP", "swusb t", "pcba板位类别.swusb t"),
            // dict.prod.pcba.panel.category.240
            ("dict.prod.pcba.panel.category.240", "zh-CN", "swusb t", "pcba板位类别.swusb t"),
            // dict.prod.pcba.panel.category.240
            ("dict.prod.pcba.panel.category.240", "zh-HK", "swusb t", "pcba板位类别.swusb t"),

            // dict.prod.pcba.panel.category.241
            ("dict.prod.pcba.panel.category.241", "en-US", "sys b", "pcba板位类别.sys b"),
            // dict.prod.pcba.panel.category.241
            ("dict.prod.pcba.panel.category.241", "ja-JP", "sys b", "pcba板位类别.sys b"),
            // dict.prod.pcba.panel.category.241
            ("dict.prod.pcba.panel.category.241", "zh-CN", "sys b", "pcba板位类别.sys b"),
            // dict.prod.pcba.panel.category.241
            ("dict.prod.pcba.panel.category.241", "zh-HK", "sys b", "pcba板位类别.sys b"),

            // dict.prod.pcba.panel.category.242
            ("dict.prod.pcba.panel.category.242", "en-US", "sys t", "pcba板位类别.sys t"),
            // dict.prod.pcba.panel.category.242
            ("dict.prod.pcba.panel.category.242", "ja-JP", "sys t", "pcba板位类别.sys t"),
            // dict.prod.pcba.panel.category.242
            ("dict.prod.pcba.panel.category.242", "zh-CN", "sys t", "pcba板位类别.sys t"),
            // dict.prod.pcba.panel.category.242
            ("dict.prod.pcba.panel.category.242", "zh-HK", "sys t", "pcba板位类别.sys t"),

            // dict.prod.pcba.panel.category.243
            ("dict.prod.pcba.panel.category.243", "en-US", "top", "pcba板位类别.top"),
            // dict.prod.pcba.panel.category.243
            ("dict.prod.pcba.panel.category.243", "ja-JP", "top", "pcba板位类别.top"),
            // dict.prod.pcba.panel.category.243
            ("dict.prod.pcba.panel.category.243", "zh-CN", "top", "pcba板位类别.top"),
            // dict.prod.pcba.panel.category.243
            ("dict.prod.pcba.panel.category.243", "zh-HK", "top", "pcba板位类别.top"),

            // dict.prod.pcba.panel.category.244
            ("dict.prod.pcba.panel.category.244", "en-US", "usb b", "pcba板位类别.usb b"),
            // dict.prod.pcba.panel.category.244
            ("dict.prod.pcba.panel.category.244", "ja-JP", "usb b", "pcba板位类别.usb b"),
            // dict.prod.pcba.panel.category.244
            ("dict.prod.pcba.panel.category.244", "zh-CN", "usb b", "pcba板位类别.usb b"),
            // dict.prod.pcba.panel.category.244
            ("dict.prod.pcba.panel.category.244", "zh-HK", "usb b", "pcba板位类别.usb b"),

            // dict.prod.pcba.panel.category.245
            ("dict.prod.pcba.panel.category.245", "en-US", "usb b/t", "pcba板位类别.usb b/t"),
            // dict.prod.pcba.panel.category.245
            ("dict.prod.pcba.panel.category.245", "ja-JP", "usb b/t", "pcba板位类别.usb b/t"),
            // dict.prod.pcba.panel.category.245
            ("dict.prod.pcba.panel.category.245", "zh-CN", "usb b/t", "pcba板位类别.usb b/t"),
            // dict.prod.pcba.panel.category.245
            ("dict.prod.pcba.panel.category.245", "zh-HK", "usb b/t", "pcba板位类别.usb b/t"),

            // dict.prod.pcba.panel.category.246
            ("dict.prod.pcba.panel.category.246", "en-US", "xlr", "pcba板位类别.xlr"),
            // dict.prod.pcba.panel.category.246
            ("dict.prod.pcba.panel.category.246", "ja-JP", "xlr", "pcba板位类别.xlr"),
            // dict.prod.pcba.panel.category.246
            ("dict.prod.pcba.panel.category.246", "zh-CN", "xlr", "pcba板位类别.xlr"),
            // dict.prod.pcba.panel.category.246
            ("dict.prod.pcba.panel.category.246", "zh-HK", "xlr", "pcba板位类别.xlr"),

            // dict.prod.pcba.panel.category.249
            ("dict.prod.pcba.panel.category.249", "en-US", "xlr a", "pcba板位类别.xlr a"),
            // dict.prod.pcba.panel.category.249
            ("dict.prod.pcba.panel.category.249", "ja-JP", "xlr a", "pcba板位类别.xlr a"),
            // dict.prod.pcba.panel.category.249
            ("dict.prod.pcba.panel.category.249", "zh-CN", "xlr a", "pcba板位类别.xlr a"),
            // dict.prod.pcba.panel.category.249
            ("dict.prod.pcba.panel.category.249", "zh-HK", "xlr a", "pcba板位类别.xlr a"),

            // dict.prod.pcba.panel.category.250
            ("dict.prod.pcba.panel.category.250", "en-US", "xlr b", "pcba板位类别.xlr b"),
            // dict.prod.pcba.panel.category.250
            ("dict.prod.pcba.panel.category.250", "ja-JP", "xlr b", "pcba板位类别.xlr b"),
            // dict.prod.pcba.panel.category.250
            ("dict.prod.pcba.panel.category.250", "zh-CN", "xlr b", "pcba板位类别.xlr b"),
            // dict.prod.pcba.panel.category.250
            ("dict.prod.pcba.panel.category.250", "zh-HK", "xlr b", "pcba板位类别.xlr b"),

            // dict.prod.pcba.panel.category.251
            ("dict.prod.pcba.panel.category.251", "en-US", "xlr t", "pcba板位类别.xlr t"),
            // dict.prod.pcba.panel.category.251
            ("dict.prod.pcba.panel.category.251", "ja-JP", "xlr t", "pcba板位类别.xlr t"),
            // dict.prod.pcba.panel.category.251
            ("dict.prod.pcba.panel.category.251", "zh-CN", "xlr t", "pcba板位类别.xlr t"),
            // dict.prod.pcba.panel.category.251
            ("dict.prod.pcba.panel.category.251", "zh-HK", "xlr t", "pcba板位类别.xlr t"),

            // dict.prod.pcba.panel.category.252
            ("dict.prod.pcba.panel.category.252", "en-US", "xlrin b", "pcba板位类别.xlrin b"),
            // dict.prod.pcba.panel.category.252
            ("dict.prod.pcba.panel.category.252", "ja-JP", "xlrin b", "pcba板位类别.xlrin b"),
            // dict.prod.pcba.panel.category.252
            ("dict.prod.pcba.panel.category.252", "zh-CN", "xlrin b", "pcba板位类别.xlrin b"),
            // dict.prod.pcba.panel.category.252
            ("dict.prod.pcba.panel.category.252", "zh-HK", "xlrin b", "pcba板位类别.xlrin b"),

            // dict.prod.pcba.panel.category.253
            ("dict.prod.pcba.panel.category.253", "en-US", "xlrin b/t", "pcba板位类别.xlrin b/t"),
            // dict.prod.pcba.panel.category.253
            ("dict.prod.pcba.panel.category.253", "ja-JP", "xlrin b/t", "pcba板位类别.xlrin b/t"),
            // dict.prod.pcba.panel.category.253
            ("dict.prod.pcba.panel.category.253", "zh-CN", "xlrin b/t", "pcba板位类别.xlrin b/t"),
            // dict.prod.pcba.panel.category.253
            ("dict.prod.pcba.panel.category.253", "zh-HK", "xlrin b/t", "pcba板位类别.xlrin b/t"),

            // dict.prod.pcba.panel.category.254
            ("dict.prod.pcba.panel.category.254", "en-US", "xlrin t", "pcba板位类别.xlrin t"),
            // dict.prod.pcba.panel.category.254
            ("dict.prod.pcba.panel.category.254", "ja-JP", "xlrin t", "pcba板位类别.xlrin t"),
            // dict.prod.pcba.panel.category.254
            ("dict.prod.pcba.panel.category.254", "zh-CN", "xlrin t", "pcba板位类别.xlrin t"),
            // dict.prod.pcba.panel.category.254
            ("dict.prod.pcba.panel.category.254", "zh-HK", "xlrin t", "pcba板位类别.xlrin t"),

            // dict.prod.pcba.panel.category.255
            ("dict.prod.pcba.panel.category.255", "en-US", "xlrio b", "pcba板位类别.xlrio b"),
            // dict.prod.pcba.panel.category.255
            ("dict.prod.pcba.panel.category.255", "ja-JP", "xlrio b", "pcba板位类别.xlrio b"),
            // dict.prod.pcba.panel.category.255
            ("dict.prod.pcba.panel.category.255", "zh-CN", "xlrio b", "pcba板位类别.xlrio b"),
            // dict.prod.pcba.panel.category.255
            ("dict.prod.pcba.panel.category.255", "zh-HK", "xlrio b", "pcba板位类别.xlrio b"),

            // dict.prod.pcba.panel.category.256
            ("dict.prod.pcba.panel.category.256", "en-US", "xlrio b/t", "pcba板位类别.xlrio b/t"),
            // dict.prod.pcba.panel.category.256
            ("dict.prod.pcba.panel.category.256", "ja-JP", "xlrio b/t", "pcba板位类别.xlrio b/t"),
            // dict.prod.pcba.panel.category.256
            ("dict.prod.pcba.panel.category.256", "zh-CN", "xlrio b/t", "pcba板位类别.xlrio b/t"),
            // dict.prod.pcba.panel.category.256
            ("dict.prod.pcba.panel.category.256", "zh-HK", "xlrio b/t", "pcba板位类别.xlrio b/t"),

            // dict.prod.pcba.panel.category.257
            ("dict.prod.pcba.panel.category.257", "en-US", "xlrio t", "pcba板位类别.xlrio t"),
            // dict.prod.pcba.panel.category.257
            ("dict.prod.pcba.panel.category.257", "ja-JP", "xlrio t", "pcba板位类别.xlrio t"),
            // dict.prod.pcba.panel.category.257
            ("dict.prod.pcba.panel.category.257", "zh-CN", "xlrio t", "pcba板位类别.xlrio t"),
            // dict.prod.pcba.panel.category.257
            ("dict.prod.pcba.panel.category.257", "zh-HK", "xlrio t", "pcba板位类别.xlrio t"),

            // dict.prod.pcba.panel.category.258
            ("dict.prod.pcba.panel.category.258", "en-US", "xlrout", "pcba板位类别.xlrout"),
            // dict.prod.pcba.panel.category.258
            ("dict.prod.pcba.panel.category.258", "ja-JP", "xlrout", "pcba板位类别.xlrout"),
            // dict.prod.pcba.panel.category.258
            ("dict.prod.pcba.panel.category.258", "zh-CN", "xlrout", "pcba板位类别.xlrout"),
            // dict.prod.pcba.panel.category.258
            ("dict.prod.pcba.panel.category.258", "zh-HK", "xlrout", "pcba板位类别.xlrout"),

            // dict.prod.pcba.side.category.b
            ("dict.prod.pcba.side.category.b", "en-US", "b", "pcba面别.b面"),
            // dict.prod.pcba.side.category.b
            ("dict.prod.pcba.side.category.b", "ja-JP", "b", "pcba面别.b面"),
            // dict.prod.pcba.side.category.b
            ("dict.prod.pcba.side.category.b", "zh-CN", "b面", "pcba面别.b面"),
            // dict.prod.pcba.side.category.b
            ("dict.prod.pcba.side.category.b", "zh-HK", "b面", "pcba面别.b面"),

            // dict.prod.pcba.side.category.t
            ("dict.prod.pcba.side.category.t", "en-US", "t", "pcba面别.t面"),
            // dict.prod.pcba.side.category.t
            ("dict.prod.pcba.side.category.t", "ja-JP", "t", "pcba面别.t面"),
            // dict.prod.pcba.side.category.t
            ("dict.prod.pcba.side.category.t", "zh-CN", "t面", "pcba面别.t面"),
            // dict.prod.pcba.side.category.t
            ("dict.prod.pcba.side.category.t", "zh-HK", "t面", "pcba面别.t面"),

            // dict.prod.shift.category.1
            ("dict.prod.shift.category.1", "en-US", "早", "生产班别.早"),
            // dict.prod.shift.category.1
            ("dict.prod.shift.category.1", "ja-JP", "早", "生产班别.早"),
            // dict.prod.shift.category.1
            ("dict.prod.shift.category.1", "zh-CN", "早", "生产班别.早"),
            // dict.prod.shift.category.1
            ("dict.prod.shift.category.1", "zh-HK", "早", "生产班别.早"),

            // dict.prod.shift.category.2
            ("dict.prod.shift.category.2", "en-US", "中", "生产班别.中"),
            // dict.prod.shift.category.2
            ("dict.prod.shift.category.2", "ja-JP", "中", "生产班别.中"),
            // dict.prod.shift.category.2
            ("dict.prod.shift.category.2", "zh-CN", "中", "生产班别.中"),
            // dict.prod.shift.category.2
            ("dict.prod.shift.category.2", "zh-HK", "中", "生产班别.中"),

            // dict.prod.shift.category.3
            ("dict.prod.shift.category.3", "en-US", "晚", "生产班别.晚"),
            // dict.prod.shift.category.3
            ("dict.prod.shift.category.3", "ja-JP", "晚", "生产班别.晚"),
            // dict.prod.shift.category.3
            ("dict.prod.shift.category.3", "zh-CN", "晚", "生产班别.晚"),
            // dict.prod.shift.category.3
            ("dict.prod.shift.category.3", "zh-HK", "晚", "生产班别.晚"),

            // dict.prod.shift.category.4
            ("dict.prod.shift.category.4", "en-US", "白班", "生产班别.白班"),
            // dict.prod.shift.category.4
            ("dict.prod.shift.category.4", "ja-JP", "白班", "生产班别.白班"),
            // dict.prod.shift.category.4
            ("dict.prod.shift.category.4", "zh-CN", "白班", "生产班别.白班"),
            // dict.prod.shift.category.4
            ("dict.prod.shift.category.4", "zh-HK", "白班", "生产班别.白班"),

            // dict.prod.shift.category.5
            ("dict.prod.shift.category.5", "en-US", "夜班", "生产班别.夜班"),
            // dict.prod.shift.category.5
            ("dict.prod.shift.category.5", "ja-JP", "夜班", "生产班别.夜班"),
            // dict.prod.shift.category.5
            ("dict.prod.shift.category.5", "zh-CN", "夜班", "生产班别.夜班"),
            // dict.prod.shift.category.5
            ("dict.prod.shift.category.5", "zh-HK", "夜班", "生产班别.夜班"),

            // dict.prod.stop.reason.1
            ("dict.prod.stop.reason.1", "en-US", "切换停止时间", "停线原因.切换停止时间"),
            // dict.prod.stop.reason.1
            ("dict.prod.stop.reason.1", "ja-JP", "切换停止时间", "停线原因.切换停止时间"),
            // dict.prod.stop.reason.1
            ("dict.prod.stop.reason.1", "zh-CN", "切换停止时间", "停线原因.切换停止时间"),
            // dict.prod.stop.reason.1
            ("dict.prod.stop.reason.1", "zh-HK", "切换停止时间", "停线原因.切换停止时间"),

            // dict.prod.stop.reason.2
            ("dict.prod.stop.reason.2", "en-US", "周会", "停线原因.周会"),
            // dict.prod.stop.reason.2
            ("dict.prod.stop.reason.2", "ja-JP", "周会", "停线原因.周会"),
            // dict.prod.stop.reason.2
            ("dict.prod.stop.reason.2", "zh-CN", "周会", "停线原因.周会"),
            // dict.prod.stop.reason.2
            ("dict.prod.stop.reason.2", "zh-HK", "周会", "停线原因.周会"),

            // dict.prod.stop.reason.3
            ("dict.prod.stop.reason.3", "en-US", "其他", "停线原因.其他"),
            // dict.prod.stop.reason.3
            ("dict.prod.stop.reason.3", "ja-JP", "其他", "停线原因.其他"),
            // dict.prod.stop.reason.3
            ("dict.prod.stop.reason.3", "zh-CN", "其他", "停线原因.其他"),
            // dict.prod.stop.reason.3
            ("dict.prod.stop.reason.3", "zh-HK", "其他", "停线原因.其他"),

            // dict.prod.stop.reason.4
            ("dict.prod.stop.reason.4", "en-US", "欠料", "停线原因.欠料"),
            // dict.prod.stop.reason.4
            ("dict.prod.stop.reason.4", "ja-JP", "欠料", "停线原因.欠料"),
            // dict.prod.stop.reason.4
            ("dict.prod.stop.reason.4", "zh-CN", "欠料", "停线原因.欠料"),
            // dict.prod.stop.reason.4
            ("dict.prod.stop.reason.4", "zh-HK", "欠料", "停线原因.欠料"),

            // dict.prod.stop.reason.5
            ("dict.prod.stop.reason.5", "en-US", "停电", "停线原因.停电"),
            // dict.prod.stop.reason.5
            ("dict.prod.stop.reason.5", "ja-JP", "停电", "停线原因.停电"),
            // dict.prod.stop.reason.5
            ("dict.prod.stop.reason.5", "zh-CN", "停电", "停线原因.停电"),
            // dict.prod.stop.reason.5
            ("dict.prod.stop.reason.5", "zh-HK", "停电", "停线原因.停电"),

            // dict.prod.stop.reason.6
            ("dict.prod.stop.reason.6", "en-US", "班会", "停线原因.班会"),
            // dict.prod.stop.reason.6
            ("dict.prod.stop.reason.6", "ja-JP", "班会", "停线原因.班会"),
            // dict.prod.stop.reason.6
            ("dict.prod.stop.reason.6", "zh-CN", "班会", "停线原因.班会"),
            // dict.prod.stop.reason.6
            ("dict.prod.stop.reason.6", "zh-HK", "班会", "停线原因.班会"),

            // dict.prod.stop.reason.7
            ("dict.prod.stop.reason.7", "en-US", "切换机种", "停线原因.切换机种"),
            // dict.prod.stop.reason.7
            ("dict.prod.stop.reason.7", "ja-JP", "切换机种", "停线原因.切换机种"),
            // dict.prod.stop.reason.7
            ("dict.prod.stop.reason.7", "zh-CN", "切换机种", "停线原因.切换机种"),
            // dict.prod.stop.reason.7
            ("dict.prod.stop.reason.7", "zh-HK", "切换机种", "停线原因.切换机种"),

            // dict.prod.stop.reason.8
            ("dict.prod.stop.reason.8", "en-US", "早会", "停线原因.早会"),
            // dict.prod.stop.reason.8
            ("dict.prod.stop.reason.8", "ja-JP", "早会", "停线原因.早会"),
            // dict.prod.stop.reason.8
            ("dict.prod.stop.reason.8", "zh-CN", "早会", "停线原因.早会"),
            // dict.prod.stop.reason.8
            ("dict.prod.stop.reason.8", "zh-HK", "早会", "停线原因.早会"),

            // dict.prod.stop.reason.9
            ("dict.prod.stop.reason.9", "en-US", "组立", "停线原因.组立"),
            // dict.prod.stop.reason.9
            ("dict.prod.stop.reason.9", "ja-JP", "组立", "停线原因.组立"),
            // dict.prod.stop.reason.9
            ("dict.prod.stop.reason.9", "zh-CN", "组立", "停线原因.组立"),
            // dict.prod.stop.reason.9
            ("dict.prod.stop.reason.9", "zh-HK", "组立", "停线原因.组立"),

            // dict.prod.stop.reason.10
            ("dict.prod.stop.reason.10", "en-US", "学习", "停线原因.学习"),
            // dict.prod.stop.reason.10
            ("dict.prod.stop.reason.10", "ja-JP", "学习", "停线原因.学习"),
            // dict.prod.stop.reason.10
            ("dict.prod.stop.reason.10", "zh-CN", "学习", "停线原因.学习"),
            // dict.prod.stop.reason.10
            ("dict.prod.stop.reason.10", "zh-HK", "学习", "停线原因.学习"),

            // dict.prod.stop.reason.11
            ("dict.prod.stop.reason.11", "en-US", "仪设", "停线原因.仪设"),
            // dict.prod.stop.reason.11
            ("dict.prod.stop.reason.11", "ja-JP", "仪设", "停线原因.仪设"),
            // dict.prod.stop.reason.11
            ("dict.prod.stop.reason.11", "zh-CN", "仪设", "停线原因.仪设"),
            // dict.prod.stop.reason.11
            ("dict.prod.stop.reason.11", "zh-HK", "仪设", "停线原因.仪设"),

            // dict.prod.stop.reason.12
            ("dict.prod.stop.reason.12", "en-US", "清洁", "停线原因.清洁"),
            // dict.prod.stop.reason.12
            ("dict.prod.stop.reason.12", "ja-JP", "清洁", "停线原因.清洁"),
            // dict.prod.stop.reason.12
            ("dict.prod.stop.reason.12", "zh-CN", "清洁", "停线原因.清洁"),
            // dict.prod.stop.reason.12
            ("dict.prod.stop.reason.12", "zh-HK", "清洁", "停线原因.清洁"),

            // dict.prod.visual.inspection.line.1
            ("dict.prod.visual.inspection.line.1", "en-US", "1", "目视线别.1"),
            // dict.prod.visual.inspection.line.1
            ("dict.prod.visual.inspection.line.1", "ja-JP", "1", "目视线别.1"),
            // dict.prod.visual.inspection.line.1
            ("dict.prod.visual.inspection.line.1", "zh-CN", "1", "目视线别.1"),
            // dict.prod.visual.inspection.line.1
            ("dict.prod.visual.inspection.line.1", "zh-HK", "1", "目视线别.1"),

            // dict.prod.visual.inspection.line.2
            ("dict.prod.visual.inspection.line.2", "en-US", "2", "目视线别.2"),
            // dict.prod.visual.inspection.line.2
            ("dict.prod.visual.inspection.line.2", "ja-JP", "2", "目视线别.2"),
            // dict.prod.visual.inspection.line.2
            ("dict.prod.visual.inspection.line.2", "zh-CN", "2", "目视线别.2"),
            // dict.prod.visual.inspection.line.2
            ("dict.prod.visual.inspection.line.2", "zh-HK", "2", "目视线别.2"),

            // dict.prod.warranty.status.0
            ("dict.prod.warranty.status.0", "en-US", "无保修", "保修状态.无保修"),
            // dict.prod.warranty.status.0
            ("dict.prod.warranty.status.0", "ja-JP", "无保修", "保修状态.无保修"),
            // dict.prod.warranty.status.0
            ("dict.prod.warranty.status.0", "zh-CN", "无保修", "保修状态.无保修"),
            // dict.prod.warranty.status.0
            ("dict.prod.warranty.status.0", "zh-HK", "无保修", "保修状态.无保修"),

            // dict.prod.warranty.status.1
            ("dict.prod.warranty.status.1", "en-US", "保修期内", "保修状态.保修期内"),
            // dict.prod.warranty.status.1
            ("dict.prod.warranty.status.1", "ja-JP", "保修期内", "保修状态.保修期内"),
            // dict.prod.warranty.status.1
            ("dict.prod.warranty.status.1", "zh-CN", "保修期内", "保修状态.保修期内"),
            // dict.prod.warranty.status.1
            ("dict.prod.warranty.status.1", "zh-HK", "保修期内", "保修状态.保修期内"),

            // dict.prod.warranty.status.2
            ("dict.prod.warranty.status.2", "en-US", "保修期外", "保修状态.保修期外"),
            // dict.prod.warranty.status.2
            ("dict.prod.warranty.status.2", "ja-JP", "保修期外", "保修状态.保修期外"),
            // dict.prod.warranty.status.2
            ("dict.prod.warranty.status.2", "zh-CN", "保修期外", "保修状态.保修期外"),
            // dict.prod.warranty.status.2
            ("dict.prod.warranty.status.2", "zh-HK", "保修期外", "保修状态.保修期外"),

            // dict.prod.warranty.status.3
            ("dict.prod.warranty.status.3", "en-US", "延保中", "保修状态.延保中"),
            // dict.prod.warranty.status.3
            ("dict.prod.warranty.status.3", "ja-JP", "延保中", "保修状态.延保中"),
            // dict.prod.warranty.status.3
            ("dict.prod.warranty.status.3", "zh-CN", "延保中", "保修状态.延保中"),
            // dict.prod.warranty.status.3
            ("dict.prod.warranty.status.3", "zh-HK", "延保中", "保修状态.延保中"),

            // dict.sys.data.scope.0
            ("dict.sys.data.scope.0", "en-US", "全部数据", "数据权限.全部数据"),
            // dict.sys.data.scope.0
            ("dict.sys.data.scope.0", "ja-JP", "全部数据", "数据权限.全部数据"),
            // dict.sys.data.scope.0
            ("dict.sys.data.scope.0", "zh-CN", "全部数据", "数据权限.全部数据"),
            // dict.sys.data.scope.0
            ("dict.sys.data.scope.0", "zh-HK", "全部数据", "数据权限.全部数据"),

            // dict.sys.data.scope.1
            ("dict.sys.data.scope.1", "en-US", "本部门数据", "数据权限.本部门数据"),
            // dict.sys.data.scope.1
            ("dict.sys.data.scope.1", "ja-JP", "本部门数据", "数据权限.本部门数据"),
            // dict.sys.data.scope.1
            ("dict.sys.data.scope.1", "zh-CN", "本部门数据", "数据权限.本部门数据"),
            // dict.sys.data.scope.1
            ("dict.sys.data.scope.1", "zh-HK", "本部门数据", "数据权限.本部门数据"),

            // dict.sys.data.scope.2
            ("dict.sys.data.scope.2", "en-US", "本部门及以下数据", "数据权限.本部门及以下数据"),
            // dict.sys.data.scope.2
            ("dict.sys.data.scope.2", "ja-JP", "本部门及以下数据", "数据权限.本部门及以下数据"),
            // dict.sys.data.scope.2
            ("dict.sys.data.scope.2", "zh-CN", "本部门及以下数据", "数据权限.本部门及以下数据"),
            // dict.sys.data.scope.2
            ("dict.sys.data.scope.2", "zh-HK", "本部门及以下数据", "数据权限.本部门及以下数据"),

            // dict.sys.data.scope.3
            ("dict.sys.data.scope.3", "en-US", "仅本人数据", "数据权限.仅本人数据"),
            // dict.sys.data.scope.3
            ("dict.sys.data.scope.3", "ja-JP", "仅本人数据", "数据权限.仅本人数据"),
            // dict.sys.data.scope.3
            ("dict.sys.data.scope.3", "zh-CN", "仅本人数据", "数据权限.仅本人数据"),
            // dict.sys.data.scope.3
            ("dict.sys.data.scope.3", "zh-HK", "仅本人数据", "数据权限.仅本人数据"),

            // dict.sys.data.scope.4
            ("dict.sys.data.scope.4", "en-US", "自定义数据范围", "数据权限.自定义数据范围"),
            // dict.sys.data.scope.4
            ("dict.sys.data.scope.4", "ja-JP", "自定义数据范围", "数据权限.自定义数据范围"),
            // dict.sys.data.scope.4
            ("dict.sys.data.scope.4", "zh-CN", "自定义数据范围", "数据权限.自定义数据范围"),
            // dict.sys.data.scope.4
            ("dict.sys.data.scope.4", "zh-HK", "自定义数据范围", "数据权限.自定义数据范围"),

            // dict.sys.data.source.0
            ("dict.sys.data.source.0", "en-US", "系统表", "数据源.系统表"),
            // dict.sys.data.source.0
            ("dict.sys.data.source.0", "ja-JP", "系统表", "数据源.系统表"),
            // dict.sys.data.source.0
            ("dict.sys.data.source.0", "zh-CN", "系统表", "数据源.系统表"),
            // dict.sys.data.source.0
            ("dict.sys.data.source.0", "zh-HK", "系统表", "数据源.系统表"),

            // dict.sys.data.source.1
            ("dict.sys.data.source.1", "en-US", "sql查询", "数据源.sql查询"),
            // dict.sys.data.source.1
            ("dict.sys.data.source.1", "ja-JP", "sql查询", "数据源.sql查询"),
            // dict.sys.data.source.1
            ("dict.sys.data.source.1", "zh-CN", "sql查询", "数据源.sql查询"),
            // dict.sys.data.source.1
            ("dict.sys.data.source.1", "zh-HK", "sql查询", "数据源.sql查询"),

            // dict.sys.db.data.type.bigint
            ("dict.sys.db.data.type.bigint", "en-US", "bigint", "数据库数据类型.bigint"),
            // dict.sys.db.data.type.bigint
            ("dict.sys.db.data.type.bigint", "ja-JP", "bigint", "数据库数据类型.bigint"),
            // dict.sys.db.data.type.bigint
            ("dict.sys.db.data.type.bigint", "zh-CN", "bigint", "数据库数据类型.bigint"),
            // dict.sys.db.data.type.bigint
            ("dict.sys.db.data.type.bigint", "zh-HK", "bigint", "数据库数据类型.bigint"),

            // dict.sys.db.data.type.bit
            ("dict.sys.db.data.type.bit", "en-US", "bit", "数据库数据类型.bit"),
            // dict.sys.db.data.type.bit
            ("dict.sys.db.data.type.bit", "ja-JP", "bit", "数据库数据类型.bit"),
            // dict.sys.db.data.type.bit
            ("dict.sys.db.data.type.bit", "zh-CN", "bit", "数据库数据类型.bit"),
            // dict.sys.db.data.type.bit
            ("dict.sys.db.data.type.bit", "zh-HK", "bit", "数据库数据类型.bit"),

            // dict.sys.db.data.type.datetime
            ("dict.sys.db.data.type.datetime", "en-US", "datetime", "数据库数据类型.datetime"),
            // dict.sys.db.data.type.datetime
            ("dict.sys.db.data.type.datetime", "ja-JP", "datetime", "数据库数据类型.datetime"),
            // dict.sys.db.data.type.datetime
            ("dict.sys.db.data.type.datetime", "zh-CN", "datetime", "数据库数据类型.datetime"),
            // dict.sys.db.data.type.datetime
            ("dict.sys.db.data.type.datetime", "zh-HK", "datetime", "数据库数据类型.datetime"),

            // dict.sys.db.data.type.decimal
            ("dict.sys.db.data.type.decimal", "en-US", "decimal", "数据库数据类型.decimal"),
            // dict.sys.db.data.type.decimal
            ("dict.sys.db.data.type.decimal", "ja-JP", "decimal", "数据库数据类型.decimal"),
            // dict.sys.db.data.type.decimal
            ("dict.sys.db.data.type.decimal", "zh-CN", "decimal", "数据库数据类型.decimal"),
            // dict.sys.db.data.type.decimal
            ("dict.sys.db.data.type.decimal", "zh-HK", "decimal", "数据库数据类型.decimal"),

            // dict.sys.db.data.type.int
            ("dict.sys.db.data.type.int", "en-US", "int", "数据库数据类型.int"),
            // dict.sys.db.data.type.int
            ("dict.sys.db.data.type.int", "ja-JP", "int", "数据库数据类型.int"),
            // dict.sys.db.data.type.int
            ("dict.sys.db.data.type.int", "zh-CN", "int", "数据库数据类型.int"),
            // dict.sys.db.data.type.int
            ("dict.sys.db.data.type.int", "zh-HK", "int", "数据库数据类型.int"),

            // dict.sys.db.data.type.ntext
            ("dict.sys.db.data.type.ntext", "en-US", "ntext", "数据库数据类型.ntext"),
            // dict.sys.db.data.type.ntext
            ("dict.sys.db.data.type.ntext", "ja-JP", "ntext", "数据库数据类型.ntext"),
            // dict.sys.db.data.type.ntext
            ("dict.sys.db.data.type.ntext", "zh-CN", "ntext", "数据库数据类型.ntext"),
            // dict.sys.db.data.type.ntext
            ("dict.sys.db.data.type.ntext", "zh-HK", "ntext", "数据库数据类型.ntext"),

            // dict.sys.db.data.type.nvarchar
            ("dict.sys.db.data.type.nvarchar", "en-US", "nvarchar", "数据库数据类型.nvarchar"),
            // dict.sys.db.data.type.nvarchar
            ("dict.sys.db.data.type.nvarchar", "ja-JP", "nvarchar", "数据库数据类型.nvarchar"),
            // dict.sys.db.data.type.nvarchar
            ("dict.sys.db.data.type.nvarchar", "zh-CN", "nvarchar", "数据库数据类型.nvarchar"),
            // dict.sys.db.data.type.nvarchar
            ("dict.sys.db.data.type.nvarchar", "zh-HK", "nvarchar", "数据库数据类型.nvarchar"),

            // dict.sys.db.data.type.text
            ("dict.sys.db.data.type.text", "en-US", "text", "数据库数据类型.text"),
            // dict.sys.db.data.type.text
            ("dict.sys.db.data.type.text", "ja-JP", "text", "数据库数据类型.text"),
            // dict.sys.db.data.type.text
            ("dict.sys.db.data.type.text", "zh-CN", "text", "数据库数据类型.text"),
            // dict.sys.db.data.type.text
            ("dict.sys.db.data.type.text", "zh-HK", "text", "数据库数据类型.text"),

            // dict.sys.db.data.type.uniqueidentifier
            ("dict.sys.db.data.type.uniqueidentifier", "en-US", "uniqueidentifier", "数据库数据类型.uniqueidentifier"),
            // dict.sys.db.data.type.uniqueidentifier
            ("dict.sys.db.data.type.uniqueidentifier", "ja-JP", "uniqueidentifier", "数据库数据类型.uniqueidentifier"),
            // dict.sys.db.data.type.uniqueidentifier
            ("dict.sys.db.data.type.uniqueidentifier", "zh-CN", "uniqueidentifier", "数据库数据类型.uniqueidentifier"),
            // dict.sys.db.data.type.uniqueidentifier
            ("dict.sys.db.data.type.uniqueidentifier", "zh-HK", "uniqueidentifier", "数据库数据类型.uniqueidentifier"),

            // dict.sys.db.data.type.varchar
            ("dict.sys.db.data.type.varchar", "en-US", "varchar", "数据库数据类型.varchar"),
            // dict.sys.db.data.type.varchar
            ("dict.sys.db.data.type.varchar", "ja-JP", "varchar", "数据库数据类型.varchar"),
            // dict.sys.db.data.type.varchar
            ("dict.sys.db.data.type.varchar", "zh-CN", "varchar", "数据库数据类型.varchar"),
            // dict.sys.db.data.type.varchar
            ("dict.sys.db.data.type.varchar", "zh-HK", "varchar", "数据库数据类型.varchar"),

            // dict.sys.dept.type.0
            ("dict.sys.dept.type.0", "en-US", "直接", "部门类型.直接"),
            // dict.sys.dept.type.0
            ("dict.sys.dept.type.0", "ja-JP", "直接", "部门类型.直接"),
            // dict.sys.dept.type.0
            ("dict.sys.dept.type.0", "zh-CN", "直接", "部门类型.直接"),
            // dict.sys.dept.type.0
            ("dict.sys.dept.type.0", "zh-HK", "直接", "部门类型.直接"),

            // dict.sys.dept.type.1
            ("dict.sys.dept.type.1", "en-US", "间接", "部门类型.间接"),
            // dict.sys.dept.type.1
            ("dict.sys.dept.type.1", "ja-JP", "间接", "部门类型.间接"),
            // dict.sys.dept.type.1
            ("dict.sys.dept.type.1", "zh-CN", "间接", "部门类型.间接"),
            // dict.sys.dept.type.1
            ("dict.sys.dept.type.1", "zh-HK", "间接", "部门类型.间接"),

            // dict.sys.file.category.0
            ("dict.sys.file.category.0", "en-US", "文档", "文件分类.文档"),
            // dict.sys.file.category.0
            ("dict.sys.file.category.0", "ja-JP", "文档", "文件分类.文档"),
            // dict.sys.file.category.0
            ("dict.sys.file.category.0", "zh-CN", "文档", "文件分类.文档"),
            // dict.sys.file.category.0
            ("dict.sys.file.category.0", "zh-HK", "文档", "文件分类.文档"),

            // dict.sys.file.category.1
            ("dict.sys.file.category.1", "en-US", "图片", "文件分类.图片"),
            // dict.sys.file.category.1
            ("dict.sys.file.category.1", "ja-JP", "图片", "文件分类.图片"),
            // dict.sys.file.category.1
            ("dict.sys.file.category.1", "zh-CN", "图片", "文件分类.图片"),
            // dict.sys.file.category.1
            ("dict.sys.file.category.1", "zh-HK", "图片", "文件分类.图片"),

            // dict.sys.file.category.2
            ("dict.sys.file.category.2", "en-US", "视频", "文件分类.视频"),
            // dict.sys.file.category.2
            ("dict.sys.file.category.2", "ja-JP", "视频", "文件分类.视频"),
            // dict.sys.file.category.2
            ("dict.sys.file.category.2", "zh-CN", "视频", "文件分类.视频"),
            // dict.sys.file.category.2
            ("dict.sys.file.category.2", "zh-HK", "视频", "文件分类.视频"),

            // dict.sys.file.category.3
            ("dict.sys.file.category.3", "en-US", "音频", "文件分类.音频"),
            // dict.sys.file.category.3
            ("dict.sys.file.category.3", "ja-JP", "音频", "文件分类.音频"),
            // dict.sys.file.category.3
            ("dict.sys.file.category.3", "zh-CN", "音频", "文件分类.音频"),
            // dict.sys.file.category.3
            ("dict.sys.file.category.3", "zh-HK", "音频", "文件分类.音频"),

            // dict.sys.file.category.4
            ("dict.sys.file.category.4", "en-US", "压缩包", "文件分类.压缩包"),
            // dict.sys.file.category.4
            ("dict.sys.file.category.4", "ja-JP", "压缩包", "文件分类.压缩包"),
            // dict.sys.file.category.4
            ("dict.sys.file.category.4", "zh-CN", "压缩包", "文件分类.压缩包"),
            // dict.sys.file.category.4
            ("dict.sys.file.category.4", "zh-HK", "压缩包", "文件分类.压缩包"),

            // dict.sys.file.category.5
            ("dict.sys.file.category.5", "en-US", "其他", "文件分类.其他"),
            // dict.sys.file.category.5
            ("dict.sys.file.category.5", "ja-JP", "其他", "文件分类.其他"),
            // dict.sys.file.category.5
            ("dict.sys.file.category.5", "zh-CN", "其他", "文件分类.其他"),
            // dict.sys.file.category.5
            ("dict.sys.file.category.5", "zh-HK", "其他", "文件分类.其他"),

            // dict.sys.file.status.0
            ("dict.sys.file.status.0", "en-US", "正常", "文件状态.正常"),
            // dict.sys.file.status.0
            ("dict.sys.file.status.0", "ja-JP", "正常", "文件状态.正常"),
            // dict.sys.file.status.0
            ("dict.sys.file.status.0", "zh-CN", "正常", "文件状态.正常"),
            // dict.sys.file.status.0
            ("dict.sys.file.status.0", "zh-HK", "正常", "文件状态.正常"),

            // dict.sys.file.status.1
            ("dict.sys.file.status.1", "en-US", "已锁定", "文件状态.已锁定"),
            // dict.sys.file.status.1
            ("dict.sys.file.status.1", "ja-JP", "已锁定", "文件状态.已锁定"),
            // dict.sys.file.status.1
            ("dict.sys.file.status.1", "zh-CN", "已锁定", "文件状态.已锁定"),
            // dict.sys.file.status.1
            ("dict.sys.file.status.1", "zh-HK", "已锁定", "文件状态.已锁定"),

            // dict.sys.file.status.2
            ("dict.sys.file.status.2", "en-US", "已归档", "文件状态.已归档"),
            // dict.sys.file.status.2
            ("dict.sys.file.status.2", "ja-JP", "已归档", "文件状态.已归档"),
            // dict.sys.file.status.2
            ("dict.sys.file.status.2", "zh-CN", "已归档", "文件状态.已归档"),
            // dict.sys.file.status.2
            ("dict.sys.file.status.2", "zh-HK", "已归档", "文件状态.已归档"),

            // dict.sys.file.status.3
            ("dict.sys.file.status.3", "en-US", "已删除", "文件状态.已删除"),
            // dict.sys.file.status.3
            ("dict.sys.file.status.3", "ja-JP", "已删除", "文件状态.已删除"),
            // dict.sys.file.status.3
            ("dict.sys.file.status.3", "zh-CN", "已删除", "文件状态.已删除"),
            // dict.sys.file.status.3
            ("dict.sys.file.status.3", "zh-HK", "已删除", "文件状态.已删除"),

            // dict.sys.flow.category.0
            ("dict.sys.flow.category.0", "en-US", "通用流程", "流程分类.通用流程"),
            // dict.sys.flow.category.0
            ("dict.sys.flow.category.0", "ja-JP", "通用流程", "流程分类.通用流程"),
            // dict.sys.flow.category.0
            ("dict.sys.flow.category.0", "zh-CN", "通用流程", "流程分类.通用流程"),
            // dict.sys.flow.category.0
            ("dict.sys.flow.category.0", "zh-HK", "通用流程", "流程分类.通用流程"),

            // dict.sys.flow.category.1
            ("dict.sys.flow.category.1", "en-US", "业务流程", "流程分类.业务流程"),
            // dict.sys.flow.category.1
            ("dict.sys.flow.category.1", "ja-JP", "业务流程", "流程分类.业务流程"),
            // dict.sys.flow.category.1
            ("dict.sys.flow.category.1", "zh-CN", "业务流程", "流程分类.业务流程"),
            // dict.sys.flow.category.1
            ("dict.sys.flow.category.1", "zh-HK", "业务流程", "流程分类.业务流程"),

            // dict.sys.flow.category.2
            ("dict.sys.flow.category.2", "en-US", "系统流程", "流程分类.系统流程"),
            // dict.sys.flow.category.2
            ("dict.sys.flow.category.2", "ja-JP", "系统流程", "流程分类.系统流程"),
            // dict.sys.flow.category.2
            ("dict.sys.flow.category.2", "zh-CN", "系统流程", "流程分类.系统流程"),
            // dict.sys.flow.category.2
            ("dict.sys.flow.category.2", "zh-HK", "系统流程", "流程分类.系统流程"),

            // dict.sys.flow.status.0
            ("dict.sys.flow.status.0", "en-US", "运行中", "流程状态.运行中"),
            // dict.sys.flow.status.0
            ("dict.sys.flow.status.0", "ja-JP", "运行中", "流程状态.运行中"),
            // dict.sys.flow.status.0
            ("dict.sys.flow.status.0", "zh-CN", "运行中", "流程状态.运行中"),
            // dict.sys.flow.status.0
            ("dict.sys.flow.status.0", "zh-HK", "运行中", "流程状态.运行中"),

            // dict.sys.flow.status.1
            ("dict.sys.flow.status.1", "en-US", "已完成", "流程状态.已完成"),
            // dict.sys.flow.status.1
            ("dict.sys.flow.status.1", "ja-JP", "已完成", "流程状态.已完成"),
            // dict.sys.flow.status.1
            ("dict.sys.flow.status.1", "zh-CN", "已完成", "流程状态.已完成"),
            // dict.sys.flow.status.1
            ("dict.sys.flow.status.1", "zh-HK", "已完成", "流程状态.已完成"),

            // dict.sys.flow.status.2
            ("dict.sys.flow.status.2", "en-US", "已终止", "流程状态.已终止"),
            // dict.sys.flow.status.2
            ("dict.sys.flow.status.2", "ja-JP", "已终止", "流程状态.已终止"),
            // dict.sys.flow.status.2
            ("dict.sys.flow.status.2", "zh-CN", "已终止", "流程状态.已终止"),
            // dict.sys.flow.status.2
            ("dict.sys.flow.status.2", "zh-HK", "已终止", "流程状态.已终止"),

            // dict.sys.flow.status.3
            ("dict.sys.flow.status.3", "en-US", "已挂起", "流程状态.已挂起"),
            // dict.sys.flow.status.3
            ("dict.sys.flow.status.3", "ja-JP", "已挂起", "流程状态.已挂起"),
            // dict.sys.flow.status.3
            ("dict.sys.flow.status.3", "zh-CN", "已挂起", "流程状态.已挂起"),
            // dict.sys.flow.status.3
            ("dict.sys.flow.status.3", "zh-HK", "已挂起", "流程状态.已挂起"),

            // dict.sys.flow.status.4
            ("dict.sys.flow.status.4", "en-US", "已撤回", "流程状态.已撤回"),
            // dict.sys.flow.status.4
            ("dict.sys.flow.status.4", "ja-JP", "已撤回", "流程状态.已撤回"),
            // dict.sys.flow.status.4
            ("dict.sys.flow.status.4", "zh-CN", "已撤回", "流程状态.已撤回"),
            // dict.sys.flow.status.4
            ("dict.sys.flow.status.4", "zh-HK", "已撤回", "流程状态.已撤回"),

            // dict.sys.flow.status.5
            ("dict.sys.flow.status.5", "en-US", "草稿", "流程状态.草稿"),
            // dict.sys.flow.status.5
            ("dict.sys.flow.status.5", "ja-JP", "草稿", "流程状态.草稿"),
            // dict.sys.flow.status.5
            ("dict.sys.flow.status.5", "zh-CN", "草稿", "流程状态.草稿"),
            // dict.sys.flow.status.5
            ("dict.sys.flow.status.5", "zh-HK", "草稿", "流程状态.草稿"),

            // dict.sys.form.category.0
            ("dict.sys.form.category.0", "en-US", "通用表单", "表单分类.通用表单"),
            // dict.sys.form.category.0
            ("dict.sys.form.category.0", "ja-JP", "通用表单", "表单分类.通用表单"),
            // dict.sys.form.category.0
            ("dict.sys.form.category.0", "zh-CN", "通用表单", "表单分类.通用表单"),
            // dict.sys.form.category.0
            ("dict.sys.form.category.0", "zh-HK", "通用表单", "表单分类.通用表单"),

            // dict.sys.form.category.1
            ("dict.sys.form.category.1", "en-US", "业务表单", "表单分类.业务表单"),
            // dict.sys.form.category.1
            ("dict.sys.form.category.1", "ja-JP", "业务表单", "表单分类.业务表单"),
            // dict.sys.form.category.1
            ("dict.sys.form.category.1", "zh-CN", "业务表单", "表单分类.业务表单"),
            // dict.sys.form.category.1
            ("dict.sys.form.category.1", "zh-HK", "业务表单", "表单分类.业务表单"),

            // dict.sys.form.category.2
            ("dict.sys.form.category.2", "en-US", "系统表单", "表单分类.系统表单"),
            // dict.sys.form.category.2
            ("dict.sys.form.category.2", "ja-JP", "系统表单", "表单分类.系统表单"),
            // dict.sys.form.category.2
            ("dict.sys.form.category.2", "zh-CN", "系统表单", "表单分类.系统表单"),
            // dict.sys.form.category.2
            ("dict.sys.form.category.2", "zh-HK", "系统表单", "表单分类.系统表单"),

            // dict.sys.form.type.0
            ("dict.sys.form.type.0", "en-US", "动态表单", "表单类型.动态表单"),
            // dict.sys.form.type.0
            ("dict.sys.form.type.0", "ja-JP", "动态表单", "表单类型.动态表单"),
            // dict.sys.form.type.0
            ("dict.sys.form.type.0", "zh-CN", "动态表单", "表单类型.动态表单"),
            // dict.sys.form.type.0
            ("dict.sys.form.type.0", "zh-HK", "动态表单", "表单类型.动态表单"),

            // dict.sys.form.type.1
            ("dict.sys.form.type.1", "en-US", "静态表单", "表单类型.静态表单"),
            // dict.sys.form.type.1
            ("dict.sys.form.type.1", "ja-JP", "静态表单", "表单类型.静态表单"),
            // dict.sys.form.type.1
            ("dict.sys.form.type.1", "zh-CN", "静态表单", "表单类型.静态表单"),
            // dict.sys.form.type.1
            ("dict.sys.form.type.1", "zh-HK", "静态表单", "表单类型.静态表单"),

            // dict.sys.form.type.2
            ("dict.sys.form.type.2", "en-US", "自定义表单", "表单类型.自定义表单"),
            // dict.sys.form.type.2
            ("dict.sys.form.type.2", "ja-JP", "自定义表单", "表单类型.自定义表单"),
            // dict.sys.form.type.2
            ("dict.sys.form.type.2", "zh-CN", "自定义表单", "表单类型.自定义表单"),
            // dict.sys.form.type.2
            ("dict.sys.form.type.2", "zh-HK", "自定义表单", "表单类型.自定义表单"),

            // dict.sys.ftp.provider.teac_cn
            ("dict.sys.ftp.provider.teac_cn", "en-US", "teac_cn", "ftp服务提供商.teac ftp中国"),
            // dict.sys.ftp.provider.teac_cn
            ("dict.sys.ftp.provider.teac_cn", "ja-JP", "teac_cn", "ftp服务提供商.teac ftp中国"),
            // dict.sys.ftp.provider.teac_cn
            ("dict.sys.ftp.provider.teac_cn", "zh-CN", "teac ftp中国", "ftp服务提供商.teac ftp中国"),
            // dict.sys.ftp.provider.teac_cn
            ("dict.sys.ftp.provider.teac_cn", "zh-HK", "teac ftp中国", "ftp服务提供商.teac ftp中国"),

            // dict.sys.ftp.provider.teac_jp
            ("dict.sys.ftp.provider.teac_jp", "en-US", "teac_jp", "ftp服务提供商.teac ftp日本"),
            // dict.sys.ftp.provider.teac_jp
            ("dict.sys.ftp.provider.teac_jp", "ja-JP", "teac_jp", "ftp服务提供商.teac ftp日本"),
            // dict.sys.ftp.provider.teac_jp
            ("dict.sys.ftp.provider.teac_jp", "zh-CN", "teac ftp日本", "ftp服务提供商.teac ftp日本"),
            // dict.sys.ftp.provider.teac_jp
            ("dict.sys.ftp.provider.teac_jp", "zh-HK", "teac ftp日本", "ftp服务提供商.teac ftp日本"),

            // dict.sys.is.builtin.1
            ("dict.sys.is.builtin.1", "en-US", "是", "是否内置.是"),
            // dict.sys.is.builtin.1
            ("dict.sys.is.builtin.1", "ja-JP", "是", "是否内置.是"),
            // dict.sys.is.builtin.1
            ("dict.sys.is.builtin.1", "zh-CN", "是", "是否内置.是"),
            // dict.sys.is.builtin.1
            ("dict.sys.is.builtin.1", "zh-HK", "是", "是否内置.是"),

            // dict.sys.is.builtin.0
            ("dict.sys.is.builtin.0", "en-US", "否", "是否内置.否"),
            // dict.sys.is.builtin.0
            ("dict.sys.is.builtin.0", "ja-JP", "否", "是否内置.否"),
            // dict.sys.is.builtin.0
            ("dict.sys.is.builtin.0", "zh-CN", "否", "是否内置.否"),
            // dict.sys.is.builtin.0
            ("dict.sys.is.builtin.0", "zh-HK", "否", "是否内置.否"),

            // dict.sys.is.default.1
            ("dict.sys.is.default.1", "en-US", "是", "是否默认.是"),
            // dict.sys.is.default.1
            ("dict.sys.is.default.1", "ja-JP", "是", "是否默认.是"),
            // dict.sys.is.default.1
            ("dict.sys.is.default.1", "zh-CN", "是", "是否默认.是"),
            // dict.sys.is.default.1
            ("dict.sys.is.default.1", "zh-HK", "是", "是否默认.是"),

            // dict.sys.is.default.0
            ("dict.sys.is.default.0", "en-US", "否", "是否默认.否"),
            // dict.sys.is.default.0
            ("dict.sys.is.default.0", "ja-JP", "否", "是否默认.否"),
            // dict.sys.is.default.0
            ("dict.sys.is.default.0", "zh-CN", "否", "是否默认.否"),
            // dict.sys.is.default.0
            ("dict.sys.is.default.0", "zh-HK", "否", "是否默认.否"),

            // dict.sys.is.public.0
            ("dict.sys.is.public.0", "en-US", "公开", "是否公开.公开"),
            // dict.sys.is.public.0
            ("dict.sys.is.public.0", "ja-JP", "公开", "是否公开.公开"),
            // dict.sys.is.public.0
            ("dict.sys.is.public.0", "zh-CN", "公开", "是否公开.公开"),
            // dict.sys.is.public.0
            ("dict.sys.is.public.0", "zh-HK", "公开", "是否公开.公开"),

            // dict.sys.is.public.1
            ("dict.sys.is.public.1", "en-US", "私有", "是否公开.私有"),
            // dict.sys.is.public.1
            ("dict.sys.is.public.1", "ja-JP", "私有", "是否公开.私有"),
            // dict.sys.is.public.1
            ("dict.sys.is.public.1", "zh-CN", "私有", "是否公开.私有"),
            // dict.sys.is.public.1
            ("dict.sys.is.public.1", "zh-HK", "私有", "是否公开.私有"),

            // dict.sys.culture.code.en-us
            ("dict.sys.culture.code.en-us", "en-US", "English", "区域类别.English"),
            // dict.sys.culture.code.en-us
            ("dict.sys.culture.code.en-us", "ja-JP", "English", "区域类别.English"),
            // dict.sys.culture.code.en-us
            ("dict.sys.culture.code.en-us", "zh-CN", "English", "区域类别.English"),
            // dict.sys.culture.code.en-us
            ("dict.sys.culture.code.en-us", "zh-HK", "English", "区域类别.English"),

            // dict.sys.culture.code.ja-jp
            ("dict.sys.culture.code.ja-jp", "en-US", "日本語", "区域类别.日本語"),
            // dict.sys.culture.code.ja-jp
            ("dict.sys.culture.code.ja-jp", "ja-JP", "日本語", "区域类别.日本語"),
            // dict.sys.culture.code.ja-jp
            ("dict.sys.culture.code.ja-jp", "zh-CN", "日本語", "区域类别.日本語"),
            // dict.sys.culture.code.ja-jp
            ("dict.sys.culture.code.ja-jp", "zh-HK", "日本語", "区域类别.日本語"),

            // dict.sys.culture.code.zh-hk
            ("dict.sys.culture.code.zh-hk", "en-US", "香港繁體", "区域类别.香港繁體"),
            // dict.sys.culture.code.zh-hk
            ("dict.sys.culture.code.zh-hk", "ja-JP", "香港繁體", "区域类别.香港繁體"),
            // dict.sys.culture.code.zh-hk
            ("dict.sys.culture.code.zh-hk", "zh-CN", "香港繁體", "区域类别.香港繁體"),
            // dict.sys.culture.code.zh-hk
            ("dict.sys.culture.code.zh-hk", "zh-HK", "香港繁體", "区域类别.香港繁體"),

            // dict.sys.culture.code.zh-cn
            ("dict.sys.culture.code.zh-cn", "en-US", "简体中文", "区域类别.简体中文"),
            // dict.sys.culture.code.zh-cn
            ("dict.sys.culture.code.zh-cn", "ja-JP", "简体中文", "区域类别.简体中文"),
            // dict.sys.culture.code.zh-cn
            ("dict.sys.culture.code.zh-cn", "zh-CN", "简体中文", "区域类别.简体中文"),
            // dict.sys.culture.code.zh-cn
            ("dict.sys.culture.code.zh-cn", "zh-HK", "简体中文", "区域类别.简体中文"),

            // dict.sys.language.code.en-us
            ("dict.sys.language.code.en-us", "en-US", "en-us", "语言编码.english"),
            // dict.sys.language.code.en-us
            ("dict.sys.language.code.en-us", "ja-JP", "en-us", "语言编码.english"),
            // dict.sys.language.code.en-us
            ("dict.sys.language.code.en-us", "zh-CN", "english", "语言编码.english"),
            // dict.sys.language.code.en-us
            ("dict.sys.language.code.en-us", "zh-HK", "english", "语言编码.english"),

            // dict.sys.language.code.ja-jp
            ("dict.sys.language.code.ja-jp", "en-US", "ja-jp", "语言编码.日本語"),
            // dict.sys.language.code.ja-jp
            ("dict.sys.language.code.ja-jp", "ja-JP", "ja-jp", "语言编码.日本語"),
            // dict.sys.language.code.ja-jp
            ("dict.sys.language.code.ja-jp", "zh-CN", "日本語", "语言编码.日本語"),
            // dict.sys.language.code.ja-jp
            ("dict.sys.language.code.ja-jp", "zh-HK", "日本語", "语言编码.日本語"),

            // dict.sys.language.code.ko-kr
            ("dict.sys.language.code.ko-kr", "en-US", "ko-kr", "语言编码.한국어"),
            // dict.sys.language.code.ko-kr
            ("dict.sys.language.code.ko-kr", "ja-JP", "ko-kr", "语言编码.한국어"),
            // dict.sys.language.code.ko-kr
            ("dict.sys.language.code.ko-kr", "zh-CN", "한국어", "语言编码.한국어"),
            // dict.sys.language.code.ko-kr
            ("dict.sys.language.code.ko-kr", "zh-HK", "한국어", "语言编码.한국어"),

            // dict.sys.language.code.zh-cn
            ("dict.sys.language.code.zh-cn", "en-US", "zh-cn", "语言编码.简体中文"),
            // dict.sys.language.code.zh-cn
            ("dict.sys.language.code.zh-cn", "ja-JP", "zh-cn", "语言编码.简体中文"),
            // dict.sys.language.code.zh-cn
            ("dict.sys.language.code.zh-cn", "zh-CN", "简体中文", "语言编码.简体中文"),
            // dict.sys.language.code.zh-cn
            ("dict.sys.language.code.zh-cn", "zh-HK", "简体中文", "语言编码.简体中文"),

            // dict.sys.language.code.zh-hk
            ("dict.sys.language.code.zh-hk", "en-US", "zh-hk", "语言编码.香港繁體"),
            // dict.sys.language.code.zh-hk
            ("dict.sys.language.code.zh-hk", "ja-JP", "zh-hk", "语言编码.香港繁體"),
            // dict.sys.language.code.zh-hk
            ("dict.sys.language.code.zh-hk", "zh-CN", "香港繁體", "语言编码.香港繁體"),
            // dict.sys.language.code.zh-hk
            ("dict.sys.language.code.zh-hk", "zh-HK", "香港繁體", "语言编码.香港繁體"),

            // dict.sys.language.code.zh-tw
            ("dict.sys.language.code.zh-tw", "en-US", "zh-tw", "语言编码.台灣繁體"),
            // dict.sys.language.code.zh-tw
            ("dict.sys.language.code.zh-tw", "ja-JP", "zh-tw", "语言编码.台灣繁體"),
            // dict.sys.language.code.zh-tw
            ("dict.sys.language.code.zh-tw", "zh-CN", "台灣繁體", "语言编码.台灣繁體"),
            // dict.sys.language.code.zh-tw
            ("dict.sys.language.code.zh-tw", "zh-HK", "台灣繁體", "语言编码.台灣繁體"),

            // dict.sys.leave.category.affair
            ("dict.sys.leave.category.affair", "en-US", "affair", "请假类型.事假"),
            // dict.sys.leave.category.affair
            ("dict.sys.leave.category.affair", "ja-JP", "affair", "请假类型.事假"),
            // dict.sys.leave.category.affair
            ("dict.sys.leave.category.affair", "zh-CN", "事假", "请假类型.事假"),
            // dict.sys.leave.category.affair
            ("dict.sys.leave.category.affair", "zh-HK", "事假", "请假类型.事假"),

            // dict.sys.leave.category.sick
            ("dict.sys.leave.category.sick", "en-US", "sick", "请假类型.病假"),
            // dict.sys.leave.category.sick
            ("dict.sys.leave.category.sick", "ja-JP", "sick", "请假类型.病假"),
            // dict.sys.leave.category.sick
            ("dict.sys.leave.category.sick", "zh-CN", "病假", "请假类型.病假"),
            // dict.sys.leave.category.sick
            ("dict.sys.leave.category.sick", "zh-HK", "病假", "请假类型.病假"),

            // dict.sys.leave.category.annual
            ("dict.sys.leave.category.annual", "en-US", "annual", "请假类型.年假"),
            // dict.sys.leave.category.annual
            ("dict.sys.leave.category.annual", "ja-JP", "annual", "请假类型.年假"),
            // dict.sys.leave.category.annual
            ("dict.sys.leave.category.annual", "zh-CN", "年假", "请假类型.年假"),
            // dict.sys.leave.category.annual
            ("dict.sys.leave.category.annual", "zh-HK", "年假", "请假类型.年假"),

            // dict.sys.leave.category.marriage
            ("dict.sys.leave.category.marriage", "en-US", "marriage", "请假类型.婚假"),
            // dict.sys.leave.category.marriage
            ("dict.sys.leave.category.marriage", "ja-JP", "marriage", "请假类型.婚假"),
            // dict.sys.leave.category.marriage
            ("dict.sys.leave.category.marriage", "zh-CN", "婚假", "请假类型.婚假"),
            // dict.sys.leave.category.marriage
            ("dict.sys.leave.category.marriage", "zh-HK", "婚假", "请假类型.婚假"),

            // dict.sys.leave.category.maternity
            ("dict.sys.leave.category.maternity", "en-US", "maternity", "请假类型.产假"),
            // dict.sys.leave.category.maternity
            ("dict.sys.leave.category.maternity", "ja-JP", "maternity", "请假类型.产假"),
            // dict.sys.leave.category.maternity
            ("dict.sys.leave.category.maternity", "zh-CN", "产假", "请假类型.产假"),
            // dict.sys.leave.category.maternity
            ("dict.sys.leave.category.maternity", "zh-HK", "产假", "请假类型.产假"),

            // dict.sys.leave.category.paternity
            ("dict.sys.leave.category.paternity", "en-US", "paternity", "请假类型.陪产假"),
            // dict.sys.leave.category.paternity
            ("dict.sys.leave.category.paternity", "ja-JP", "paternity", "请假类型.陪产假"),
            // dict.sys.leave.category.paternity
            ("dict.sys.leave.category.paternity", "zh-CN", "陪产假", "请假类型.陪产假"),
            // dict.sys.leave.category.paternity
            ("dict.sys.leave.category.paternity", "zh-HK", "陪产假", "请假类型.陪产假"),

            // dict.sys.leave.category.bereavement
            ("dict.sys.leave.category.bereavement", "en-US", "bereavement", "请假类型.丧假"),
            // dict.sys.leave.category.bereavement
            ("dict.sys.leave.category.bereavement", "ja-JP", "bereavement", "请假类型.丧假"),
            // dict.sys.leave.category.bereavement
            ("dict.sys.leave.category.bereavement", "zh-CN", "丧假", "请假类型.丧假"),
            // dict.sys.leave.category.bereavement
            ("dict.sys.leave.category.bereavement", "zh-HK", "丧假", "请假类型.丧假"),

            // dict.sys.leave.category.compensatory
            ("dict.sys.leave.category.compensatory", "en-US", "compensatory", "请假类型.调休"),
            // dict.sys.leave.category.compensatory
            ("dict.sys.leave.category.compensatory", "ja-JP", "compensatory", "请假类型.调休"),
            // dict.sys.leave.category.compensatory
            ("dict.sys.leave.category.compensatory", "zh-CN", "调休", "请假类型.调休"),
            // dict.sys.leave.category.compensatory
            ("dict.sys.leave.category.compensatory", "zh-HK", "调休", "请假类型.调休"),

            // dict.sys.leave.category.personal
            ("dict.sys.leave.category.personal", "en-US", "personal", "请假类型.私假"),
            // dict.sys.leave.category.personal
            ("dict.sys.leave.category.personal", "ja-JP", "personal", "请假类型.私假"),
            // dict.sys.leave.category.personal
            ("dict.sys.leave.category.personal", "zh-CN", "私假", "请假类型.私假"),
            // dict.sys.leave.category.personal
            ("dict.sys.leave.category.personal", "zh-HK", "私假", "请假类型.私假"),

            // dict.sys.leave.category.other
            ("dict.sys.leave.category.other", "en-US", "other", "请假类型.其他"),
            // dict.sys.leave.category.other
            ("dict.sys.leave.category.other", "ja-JP", "other", "请假类型.其他"),
            // dict.sys.leave.category.other
            ("dict.sys.leave.category.other", "zh-CN", "其他", "请假类型.其他"),
            // dict.sys.leave.category.other
            ("dict.sys.leave.category.other", "zh-HK", "其他", "请假类型.其他"),

            // dict.sys.mail.status.0
            ("dict.sys.mail.status.0", "en-US", "草稿", "邮件状态.草稿"),
            // dict.sys.mail.status.0
            ("dict.sys.mail.status.0", "ja-JP", "草稿", "邮件状态.草稿"),
            // dict.sys.mail.status.0
            ("dict.sys.mail.status.0", "zh-CN", "草稿", "邮件状态.草稿"),
            // dict.sys.mail.status.0
            ("dict.sys.mail.status.0", "zh-HK", "草稿", "邮件状态.草稿"),

            // dict.sys.mail.status.1
            ("dict.sys.mail.status.1", "en-US", "已发送", "邮件状态.已发送"),
            // dict.sys.mail.status.1
            ("dict.sys.mail.status.1", "ja-JP", "已发送", "邮件状态.已发送"),
            // dict.sys.mail.status.1
            ("dict.sys.mail.status.1", "zh-CN", "已发送", "邮件状态.已发送"),
            // dict.sys.mail.status.1
            ("dict.sys.mail.status.1", "zh-HK", "已发送", "邮件状态.已发送"),

            // dict.sys.mail.status.2
            ("dict.sys.mail.status.2", "en-US", "发送失败", "邮件状态.发送失败"),
            // dict.sys.mail.status.2
            ("dict.sys.mail.status.2", "ja-JP", "发送失败", "邮件状态.发送失败"),
            // dict.sys.mail.status.2
            ("dict.sys.mail.status.2", "zh-CN", "发送失败", "邮件状态.发送失败"),
            // dict.sys.mail.status.2
            ("dict.sys.mail.status.2", "zh-HK", "发送失败", "邮件状态.发送失败"),

            // dict.sys.mail.status.3
            ("dict.sys.mail.status.3", "en-US", "已撤回", "邮件状态.已撤回"),
            // dict.sys.mail.status.3
            ("dict.sys.mail.status.3", "ja-JP", "已撤回", "邮件状态.已撤回"),
            // dict.sys.mail.status.3
            ("dict.sys.mail.status.3", "zh-CN", "已撤回", "邮件状态.已撤回"),
            // dict.sys.mail.status.3
            ("dict.sys.mail.status.3", "zh-HK", "已撤回", "邮件状态.已撤回"),

            // dict.sys.mail.status.4
            ("dict.sys.mail.status.4", "en-US", "定时发送中", "邮件状态.定时发送中"),
            // dict.sys.mail.status.4
            ("dict.sys.mail.status.4", "ja-JP", "定时发送中", "邮件状态.定时发送中"),
            // dict.sys.mail.status.4
            ("dict.sys.mail.status.4", "zh-CN", "定时发送中", "邮件状态.定时发送中"),
            // dict.sys.mail.status.4
            ("dict.sys.mail.status.4", "zh-HK", "定时发送中", "邮件状态.定时发送中"),

            // dict.sys.mail.type.0
            ("dict.sys.mail.type.0", "en-US", "普通邮件", "邮件类型.普通邮件"),
            // dict.sys.mail.type.0
            ("dict.sys.mail.type.0", "ja-JP", "普通邮件", "邮件类型.普通邮件"),
            // dict.sys.mail.type.0
            ("dict.sys.mail.type.0", "zh-CN", "普通邮件", "邮件类型.普通邮件"),
            // dict.sys.mail.type.0
            ("dict.sys.mail.type.0", "zh-HK", "普通邮件", "邮件类型.普通邮件"),

            // dict.sys.mail.type.1
            ("dict.sys.mail.type.1", "en-US", "系统邮件", "邮件类型.系统邮件"),
            // dict.sys.mail.type.1
            ("dict.sys.mail.type.1", "ja-JP", "系统邮件", "邮件类型.系统邮件"),
            // dict.sys.mail.type.1
            ("dict.sys.mail.type.1", "zh-CN", "系统邮件", "邮件类型.系统邮件"),
            // dict.sys.mail.type.1
            ("dict.sys.mail.type.1", "zh-HK", "系统邮件", "邮件类型.系统邮件"),

            // dict.sys.mail.type.2
            ("dict.sys.mail.type.2", "en-US", "通知邮件", "邮件类型.通知邮件"),
            // dict.sys.mail.type.2
            ("dict.sys.mail.type.2", "ja-JP", "通知邮件", "邮件类型.通知邮件"),
            // dict.sys.mail.type.2
            ("dict.sys.mail.type.2", "zh-CN", "通知邮件", "邮件类型.通知邮件"),
            // dict.sys.mail.type.2
            ("dict.sys.mail.type.2", "zh-HK", "通知邮件", "邮件类型.通知邮件"),

            // dict.sys.mail.type.3
            ("dict.sys.mail.type.3", "en-US", "提醒邮件", "邮件类型.提醒邮件"),
            // dict.sys.mail.type.3
            ("dict.sys.mail.type.3", "ja-JP", "提醒邮件", "邮件类型.提醒邮件"),
            // dict.sys.mail.type.3
            ("dict.sys.mail.type.3", "zh-CN", "提醒邮件", "邮件类型.提醒邮件"),
            // dict.sys.mail.type.3
            ("dict.sys.mail.type.3", "zh-HK", "提醒邮件", "邮件类型.提醒邮件"),

            // dict.sys.menu.type.0
            ("dict.sys.menu.type.0", "en-US", "目录", "菜单类型.目录"),
            // dict.sys.menu.type.0
            ("dict.sys.menu.type.0", "ja-JP", "目录", "菜单类型.目录"),
            // dict.sys.menu.type.0
            ("dict.sys.menu.type.0", "zh-CN", "目录", "菜单类型.目录"),
            // dict.sys.menu.type.0
            ("dict.sys.menu.type.0", "zh-HK", "目录", "菜单类型.目录"),

            // dict.sys.menu.type.1
            ("dict.sys.menu.type.1", "en-US", "菜单", "菜单类型.菜单"),
            // dict.sys.menu.type.1
            ("dict.sys.menu.type.1", "ja-JP", "菜单", "菜单类型.菜单"),
            // dict.sys.menu.type.1
            ("dict.sys.menu.type.1", "zh-CN", "菜单", "菜单类型.菜单"),
            // dict.sys.menu.type.1
            ("dict.sys.menu.type.1", "zh-HK", "菜单", "菜单类型.菜单"),

            // dict.sys.menu.type.2
            ("dict.sys.menu.type.2", "en-US", "按钮", "菜单类型.按钮"),
            // dict.sys.menu.type.2
            ("dict.sys.menu.type.2", "ja-JP", "按钮", "菜单类型.按钮"),
            // dict.sys.menu.type.2
            ("dict.sys.menu.type.2", "zh-CN", "按钮", "菜单类型.按钮"),
            // dict.sys.menu.type.2
            ("dict.sys.menu.type.2", "zh-HK", "按钮", "菜单类型.按钮"),

            // dict.sys.message.group.collaboration
            ("dict.sys.message.group.collaboration", "en-US", "collaboration", "消息分组.协同"),
            // dict.sys.message.group.collaboration
            ("dict.sys.message.group.collaboration", "ja-JP", "collaboration", "消息分组.协同"),
            // dict.sys.message.group.collaboration
            ("dict.sys.message.group.collaboration", "zh-CN", "协同", "消息分组.协同"),
            // dict.sys.message.group.collaboration
            ("dict.sys.message.group.collaboration", "zh-HK", "協同", "消息分组.协同"),

            // dict.sys.message.group.officialdoc
            ("dict.sys.message.group.officialdoc", "en-US", "official document", "消息分组.公文"),
            // dict.sys.message.group.officialdoc
            ("dict.sys.message.group.officialdoc", "ja-JP", "official document", "消息分组.公文"),
            // dict.sys.message.group.officialdoc
            ("dict.sys.message.group.officialdoc", "zh-CN", "公文", "消息分组.公文"),
            // dict.sys.message.group.officialdoc
            ("dict.sys.message.group.officialdoc", "zh-HK", "公文", "消息分组.公文"),

            // dict.sys.message.group.document
            ("dict.sys.message.group.document", "en-US", "document", "消息分组.文档"),
            // dict.sys.message.group.document
            ("dict.sys.message.group.document", "ja-JP", "document", "消息分组.文档"),
            // dict.sys.message.group.document
            ("dict.sys.message.group.document", "zh-CN", "文档", "消息分组.文档"),
            // dict.sys.message.group.document
            ("dict.sys.message.group.document", "zh-HK", "文檔", "消息分组.文档"),

            // dict.sys.message.group.announcement
            ("dict.sys.message.group.announcement", "en-US", "announcement", "消息分组.公告"),
            // dict.sys.message.group.announcement
            ("dict.sys.message.group.announcement", "ja-JP", "announcement", "消息分组.公告"),
            // dict.sys.message.group.announcement
            ("dict.sys.message.group.announcement", "zh-CN", "公告", "消息分组.公告"),
            // dict.sys.message.group.announcement
            ("dict.sys.message.group.announcement", "zh-HK", "公告", "消息分组.公告"),

            // dict.sys.message.group.other
            ("dict.sys.message.group.other", "en-US", "other", "消息分组.其他"),
            // dict.sys.message.group.other
            ("dict.sys.message.group.other", "ja-JP", "other", "消息分组.其他"),
            // dict.sys.message.group.other
            ("dict.sys.message.group.other", "zh-CN", "其他", "消息分组.其他"),
            // dict.sys.message.group.other
            ("dict.sys.message.group.other", "zh-HK", "其他", "消息分组.其他"),

            // dict.sys.message.group.message
            ("dict.sys.message.group.message", "en-US", "message", "消息分组.消息"),
            // dict.sys.message.group.message
            ("dict.sys.message.group.message", "ja-JP", "message", "消息分组.消息"),
            // dict.sys.message.group.message
            ("dict.sys.message.group.message", "zh-CN", "消息", "消息分组.消息"),
            // dict.sys.message.group.message
            ("dict.sys.message.group.message", "zh-HK", "消息", "消息分组.消息"),

            // dict.sys.message.group.reminder
            ("dict.sys.message.group.reminder", "en-US", "reminder", "消息分组.提醒"),
            // dict.sys.message.group.reminder
            ("dict.sys.message.group.reminder", "ja-JP", "reminder", "消息分组.提醒"),
            // dict.sys.message.group.reminder
            ("dict.sys.message.group.reminder", "zh-CN", "提醒", "消息分组.提醒"),
            // dict.sys.message.group.reminder
            ("dict.sys.message.group.reminder", "zh-HK", "提醒", "消息分组.提醒"),

            // dict.sys.message.type.text
            ("dict.sys.message.type.text", "en-US", "text", "消息类型.文本"),
            // dict.sys.message.type.text
            ("dict.sys.message.type.text", "ja-JP", "text", "消息类型.文本"),
            // dict.sys.message.type.text
            ("dict.sys.message.type.text", "zh-CN", "文本", "消息类型.文本"),
            // dict.sys.message.type.text
            ("dict.sys.message.type.text", "zh-HK", "文本", "消息类型.文本"),

            // dict.sys.message.type.image
            ("dict.sys.message.type.image", "en-US", "image", "消息类型.图片"),
            // dict.sys.message.type.image
            ("dict.sys.message.type.image", "ja-JP", "image", "消息类型.图片"),
            // dict.sys.message.type.image
            ("dict.sys.message.type.image", "zh-CN", "图片", "消息类型.图片"),
            // dict.sys.message.type.image
            ("dict.sys.message.type.image", "zh-HK", "图片", "消息类型.图片"),

            // dict.sys.message.type.file
            ("dict.sys.message.type.file", "en-US", "file", "消息类型.文件"),
            // dict.sys.message.type.file
            ("dict.sys.message.type.file", "ja-JP", "file", "消息类型.文件"),
            // dict.sys.message.type.file
            ("dict.sys.message.type.file", "zh-CN", "文件", "消息类型.文件"),
            // dict.sys.message.type.file
            ("dict.sys.message.type.file", "zh-HK", "文件", "消息类型.文件"),

            // dict.sys.message.type.takt365
            ("dict.sys.message.type.takt365", "en-US", "takt365", "消息类型.系统消息"),
            // dict.sys.message.type.takt365
            ("dict.sys.message.type.takt365", "ja-JP", "takt365", "消息类型.系统消息"),
            // dict.sys.message.type.takt365
            ("dict.sys.message.type.takt365", "zh-CN", "系统消息", "消息类型.系统消息"),
            // dict.sys.message.type.takt365
            ("dict.sys.message.type.takt365", "zh-HK", "系统消息", "消息类型.系统消息"),

            // dict.sys.message.type.video
            ("dict.sys.message.type.video", "en-US", "video", "消息类型.视频"),
            // dict.sys.message.type.video
            ("dict.sys.message.type.video", "ja-JP", "video", "消息类型.视频"),
            // dict.sys.message.type.video
            ("dict.sys.message.type.video", "zh-CN", "视频", "消息类型.视频"),
            // dict.sys.message.type.video
            ("dict.sys.message.type.video", "zh-HK", "视频", "消息类型.视频"),

            // dict.sys.message.type.voice
            ("dict.sys.message.type.voice", "en-US", "voice", "消息类型.语音"),
            // dict.sys.message.type.voice
            ("dict.sys.message.type.voice", "ja-JP", "voice", "消息类型.语音"),
            // dict.sys.message.type.voice
            ("dict.sys.message.type.voice", "zh-CN", "语音", "消息类型.语音"),
            // dict.sys.message.type.voice
            ("dict.sys.message.type.voice", "zh-HK", "语音", "消息类型.语音"),

            // dict.sys.news.category.0
            ("dict.sys.news.category.0", "en-US", "公司新闻", "新闻分类.公司新闻"),
            // dict.sys.news.category.0
            ("dict.sys.news.category.0", "ja-JP", "公司新闻", "新闻分类.公司新闻"),
            // dict.sys.news.category.0
            ("dict.sys.news.category.0", "zh-CN", "公司新闻", "新闻分类.公司新闻"),
            // dict.sys.news.category.0
            ("dict.sys.news.category.0", "zh-HK", "公司新闻", "新闻分类.公司新闻"),

            // dict.sys.news.category.1
            ("dict.sys.news.category.1", "en-US", "行业动态", "新闻分类.行业动态"),
            // dict.sys.news.category.1
            ("dict.sys.news.category.1", "ja-JP", "行业动态", "新闻分类.行业动态"),
            // dict.sys.news.category.1
            ("dict.sys.news.category.1", "zh-CN", "行业动态", "新闻分类.行业动态"),
            // dict.sys.news.category.1
            ("dict.sys.news.category.1", "zh-HK", "行业动态", "新闻分类.行业动态"),

            // dict.sys.news.category.2
            ("dict.sys.news.category.2", "en-US", "技术分享", "新闻分类.技术分享"),
            // dict.sys.news.category.2
            ("dict.sys.news.category.2", "ja-JP", "技术分享", "新闻分类.技术分享"),
            // dict.sys.news.category.2
            ("dict.sys.news.category.2", "zh-CN", "技术分享", "新闻分类.技术分享"),
            // dict.sys.news.category.2
            ("dict.sys.news.category.2", "zh-HK", "技术分享", "新闻分类.技术分享"),

            // dict.sys.news.category.3
            ("dict.sys.news.category.3", "en-US", "产品发布", "新闻分类.产品发布"),
            // dict.sys.news.category.3
            ("dict.sys.news.category.3", "ja-JP", "产品发布", "新闻分类.产品发布"),
            // dict.sys.news.category.3
            ("dict.sys.news.category.3", "zh-CN", "产品发布", "新闻分类.产品发布"),
            // dict.sys.news.category.3
            ("dict.sys.news.category.3", "zh-HK", "产品发布", "新闻分类.产品发布"),

            // dict.sys.news.category.4
            ("dict.sys.news.category.4", "en-US", "活动资讯", "新闻分类.活动资讯"),
            // dict.sys.news.category.4
            ("dict.sys.news.category.4", "ja-JP", "活动资讯", "新闻分类.活动资讯"),
            // dict.sys.news.category.4
            ("dict.sys.news.category.4", "zh-CN", "活动资讯", "新闻分类.活动资讯"),
            // dict.sys.news.category.4
            ("dict.sys.news.category.4", "zh-HK", "活动资讯", "新闻分类.活动资讯"),

            // dict.sys.news.category.5
            ("dict.sys.news.category.5", "en-US", "其他", "新闻分类.其他"),
            // dict.sys.news.category.5
            ("dict.sys.news.category.5", "ja-JP", "其他", "新闻分类.其他"),
            // dict.sys.news.category.5
            ("dict.sys.news.category.5", "zh-CN", "其他", "新闻分类.其他"),
            // dict.sys.news.category.5
            ("dict.sys.news.category.5", "zh-HK", "其他", "新闻分类.其他"),

            // dict.sys.news.status.0
            ("dict.sys.news.status.0", "en-US", "草稿", "新闻状态.草稿"),
            // dict.sys.news.status.0
            ("dict.sys.news.status.0", "ja-JP", "草稿", "新闻状态.草稿"),
            // dict.sys.news.status.0
            ("dict.sys.news.status.0", "zh-CN", "草稿", "新闻状态.草稿"),
            // dict.sys.news.status.0
            ("dict.sys.news.status.0", "zh-HK", "草稿", "新闻状态.草稿"),

            // dict.sys.news.status.1
            ("dict.sys.news.status.1", "en-US", "已发布", "新闻状态.已发布"),
            // dict.sys.news.status.1
            ("dict.sys.news.status.1", "ja-JP", "已发布", "新闻状态.已发布"),
            // dict.sys.news.status.1
            ("dict.sys.news.status.1", "zh-CN", "已发布", "新闻状态.已发布"),
            // dict.sys.news.status.1
            ("dict.sys.news.status.1", "zh-HK", "已发布", "新闻状态.已发布"),

            // dict.sys.news.status.2
            ("dict.sys.news.status.2", "en-US", "已撤回", "新闻状态.已撤回"),
            // dict.sys.news.status.2
            ("dict.sys.news.status.2", "ja-JP", "已撤回", "新闻状态.已撤回"),
            // dict.sys.news.status.2
            ("dict.sys.news.status.2", "zh-CN", "已撤回", "新闻状态.已撤回"),
            // dict.sys.news.status.2
            ("dict.sys.news.status.2", "zh-HK", "已撤回", "新闻状态.已撤回"),

            // dict.sys.news.status.3
            ("dict.sys.news.status.3", "en-US", "已过期", "新闻状态.已过期"),
            // dict.sys.news.status.3
            ("dict.sys.news.status.3", "ja-JP", "已过期", "新闻状态.已过期"),
            // dict.sys.news.status.3
            ("dict.sys.news.status.3", "zh-CN", "已过期", "新闻状态.已过期"),
            // dict.sys.news.status.3
            ("dict.sys.news.status.3", "zh-HK", "已过期", "新闻状态.已过期"),

            // dict.sys.normal.disable.1
            ("dict.sys.normal.disable.1", "en-US", "启用", "默认状态.启用"),
            // dict.sys.normal.disable.1
            ("dict.sys.normal.disable.1", "ja-JP", "启用", "默认状态.启用"),
            // dict.sys.normal.disable.1
            ("dict.sys.normal.disable.1", "zh-CN", "启用", "默认状态.启用"),
            // dict.sys.normal.disable.1
            ("dict.sys.normal.disable.1", "zh-HK", "启用", "默认状态.启用"),

            // dict.sys.normal.disable.0
            ("dict.sys.normal.disable.0", "en-US", "禁用", "默认状态.禁用"),
            // dict.sys.normal.disable.0
            ("dict.sys.normal.disable.0", "ja-JP", "禁用", "默认状态.禁用"),
            // dict.sys.normal.disable.0
            ("dict.sys.normal.disable.0", "zh-CN", "禁用", "默认状态.禁用"),
            // dict.sys.normal.disable.0
            ("dict.sys.normal.disable.0", "zh-HK", "禁用", "默认状态.禁用"),

            // dict.sys.normal.disable.2
            ("dict.sys.normal.disable.2", "en-US", "锁定", "默认状态.锁定"),
            // dict.sys.normal.disable.2
            ("dict.sys.normal.disable.2", "ja-JP", "锁定", "默认状态.锁定"),
            // dict.sys.normal.disable.2
            ("dict.sys.normal.disable.2", "zh-CN", "锁定", "默认状态.锁定"),
            // dict.sys.normal.disable.2
            ("dict.sys.normal.disable.2", "zh-HK", "锁定", "默认状态.锁定"),

            // dict.sys.notice.status.0
            ("dict.sys.notice.status.0", "en-US", "草稿", "公告状态.草稿"),
            // dict.sys.notice.status.0
            ("dict.sys.notice.status.0", "ja-JP", "草稿", "公告状态.草稿"),
            // dict.sys.notice.status.0
            ("dict.sys.notice.status.0", "zh-CN", "草稿", "公告状态.草稿"),
            // dict.sys.notice.status.0
            ("dict.sys.notice.status.0", "zh-HK", "草稿", "公告状态.草稿"),

            // dict.sys.notice.status.1
            ("dict.sys.notice.status.1", "en-US", "已发布", "公告状态.已发布"),
            // dict.sys.notice.status.1
            ("dict.sys.notice.status.1", "ja-JP", "已发布", "公告状态.已发布"),
            // dict.sys.notice.status.1
            ("dict.sys.notice.status.1", "zh-CN", "已发布", "公告状态.已发布"),
            // dict.sys.notice.status.1
            ("dict.sys.notice.status.1", "zh-HK", "已发布", "公告状态.已发布"),

            // dict.sys.notice.status.2
            ("dict.sys.notice.status.2", "en-US", "已撤回", "公告状态.已撤回"),
            // dict.sys.notice.status.2
            ("dict.sys.notice.status.2", "ja-JP", "已撤回", "公告状态.已撤回"),
            // dict.sys.notice.status.2
            ("dict.sys.notice.status.2", "zh-CN", "已撤回", "公告状态.已撤回"),
            // dict.sys.notice.status.2
            ("dict.sys.notice.status.2", "zh-HK", "已撤回", "公告状态.已撤回"),

            // dict.sys.notice.status.3
            ("dict.sys.notice.status.3", "en-US", "已过期", "公告状态.已过期"),
            // dict.sys.notice.status.3
            ("dict.sys.notice.status.3", "ja-JP", "已过期", "公告状态.已过期"),
            // dict.sys.notice.status.3
            ("dict.sys.notice.status.3", "zh-CN", "已过期", "公告状态.已过期"),
            // dict.sys.notice.status.3
            ("dict.sys.notice.status.3", "zh-HK", "已过期", "公告状态.已过期"),

            // dict.sys.notice.type.0
            ("dict.sys.notice.type.0", "en-US", "通知", "公告类型.通知"),
            // dict.sys.notice.type.0
            ("dict.sys.notice.type.0", "ja-JP", "通知", "公告类型.通知"),
            // dict.sys.notice.type.0
            ("dict.sys.notice.type.0", "zh-CN", "通知", "公告类型.通知"),
            // dict.sys.notice.type.0
            ("dict.sys.notice.type.0", "zh-HK", "通知", "公告类型.通知"),

            // dict.sys.notice.type.1
            ("dict.sys.notice.type.1", "en-US", "公告", "公告类型.公告"),
            // dict.sys.notice.type.1
            ("dict.sys.notice.type.1", "ja-JP", "公告", "公告类型.公告"),
            // dict.sys.notice.type.1
            ("dict.sys.notice.type.1", "zh-CN", "公告", "公告类型.公告"),
            // dict.sys.notice.type.1
            ("dict.sys.notice.type.1", "zh-HK", "公告", "公告类型.公告"),

            // dict.sys.notice.type.2
            ("dict.sys.notice.type.2", "en-US", "新闻", "公告类型.新闻"),
            // dict.sys.notice.type.2
            ("dict.sys.notice.type.2", "ja-JP", "新闻", "公告类型.新闻"),
            // dict.sys.notice.type.2
            ("dict.sys.notice.type.2", "zh-CN", "新闻", "公告类型.新闻"),
            // dict.sys.notice.type.2
            ("dict.sys.notice.type.2", "zh-HK", "新闻", "公告类型.新闻"),

            // dict.sys.notice.type.3
            ("dict.sys.notice.type.3", "en-US", "活动", "公告类型.活动"),
            // dict.sys.notice.type.3
            ("dict.sys.notice.type.3", "ja-JP", "活动", "公告类型.活动"),
            // dict.sys.notice.type.3
            ("dict.sys.notice.type.3", "zh-CN", "活动", "公告类型.活动"),
            // dict.sys.notice.type.3
            ("dict.sys.notice.type.3", "zh-HK", "活动", "公告类型.活动"),

            // dict.sys.online.status.0
            ("dict.sys.online.status.0", "en-US", "在线", "在线状态.在线"),
            // dict.sys.online.status.0
            ("dict.sys.online.status.0", "ja-JP", "在线", "在线状态.在线"),
            // dict.sys.online.status.0
            ("dict.sys.online.status.0", "zh-CN", "在线", "在线状态.在线"),
            // dict.sys.online.status.0
            ("dict.sys.online.status.0", "zh-HK", "在线", "在线状态.在线"),

            // dict.sys.online.status.1
            ("dict.sys.online.status.1", "en-US", "离线", "在线状态.离线"),
            // dict.sys.online.status.1
            ("dict.sys.online.status.1", "ja-JP", "离线", "在线状态.离线"),
            // dict.sys.online.status.1
            ("dict.sys.online.status.1", "zh-CN", "离线", "在线状态.离线"),
            // dict.sys.online.status.1
            ("dict.sys.online.status.1", "zh-HK", "离线", "在线状态.离线"),

            // dict.sys.online.status.2
            ("dict.sys.online.status.2", "en-US", "离开", "在线状态.离开"),
            // dict.sys.online.status.2
            ("dict.sys.online.status.2", "ja-JP", "离开", "在线状态.离开"),
            // dict.sys.online.status.2
            ("dict.sys.online.status.2", "zh-CN", "离开", "在线状态.离开"),
            // dict.sys.online.status.2
            ("dict.sys.online.status.2", "zh-HK", "离开", "在线状态.离开"),

            // dict.sys.oper.type.1
            ("dict.sys.oper.type.1", "en-US", "新增", "操作类型.新增"),
            // dict.sys.oper.type.1
            ("dict.sys.oper.type.1", "ja-JP", "新增", "操作类型.新增"),
            // dict.sys.oper.type.1
            ("dict.sys.oper.type.1", "zh-CN", "新增", "操作类型.新增"),
            // dict.sys.oper.type.1
            ("dict.sys.oper.type.1", "zh-HK", "新增", "操作类型.新增"),

            // dict.sys.oper.type.2
            ("dict.sys.oper.type.2", "en-US", "修改", "操作类型.修改"),
            // dict.sys.oper.type.2
            ("dict.sys.oper.type.2", "ja-JP", "修改", "操作类型.修改"),
            // dict.sys.oper.type.2
            ("dict.sys.oper.type.2", "zh-CN", "修改", "操作类型.修改"),
            // dict.sys.oper.type.2
            ("dict.sys.oper.type.2", "zh-HK", "修改", "操作类型.修改"),

            // dict.sys.oper.type.3
            ("dict.sys.oper.type.3", "en-US", "删除", "操作类型.删除"),
            // dict.sys.oper.type.3
            ("dict.sys.oper.type.3", "ja-JP", "删除", "操作类型.删除"),
            // dict.sys.oper.type.3
            ("dict.sys.oper.type.3", "zh-CN", "删除", "操作类型.删除"),
            // dict.sys.oper.type.3
            ("dict.sys.oper.type.3", "zh-HK", "删除", "操作类型.删除"),

            // dict.sys.oper.type.4
            ("dict.sys.oper.type.4", "en-US", "查询", "操作类型.查询"),
            // dict.sys.oper.type.4
            ("dict.sys.oper.type.4", "ja-JP", "查询", "操作类型.查询"),
            // dict.sys.oper.type.4
            ("dict.sys.oper.type.4", "zh-CN", "查询", "操作类型.查询"),
            // dict.sys.oper.type.4
            ("dict.sys.oper.type.4", "zh-HK", "查询", "操作类型.查询"),

            // dict.sys.oper.type.5
            ("dict.sys.oper.type.5", "en-US", "导出", "操作类型.导出"),
            // dict.sys.oper.type.5
            ("dict.sys.oper.type.5", "ja-JP", "导出", "操作类型.导出"),
            // dict.sys.oper.type.5
            ("dict.sys.oper.type.5", "zh-CN", "导出", "操作类型.导出"),
            // dict.sys.oper.type.5
            ("dict.sys.oper.type.5", "zh-HK", "导出", "操作类型.导出"),

            // dict.sys.oper.type.6
            ("dict.sys.oper.type.6", "en-US", "导入", "操作类型.导入"),
            // dict.sys.oper.type.6
            ("dict.sys.oper.type.6", "ja-JP", "导入", "操作类型.导入"),
            // dict.sys.oper.type.6
            ("dict.sys.oper.type.6", "zh-CN", "导入", "操作类型.导入"),
            // dict.sys.oper.type.6
            ("dict.sys.oper.type.6", "zh-HK", "导入", "操作类型.导入"),

            // dict.sys.oper.type.7
            ("dict.sys.oper.type.7", "en-US", "授权", "操作类型.授权"),
            // dict.sys.oper.type.7
            ("dict.sys.oper.type.7", "ja-JP", "授权", "操作类型.授权"),
            // dict.sys.oper.type.7
            ("dict.sys.oper.type.7", "zh-CN", "授权", "操作类型.授权"),
            // dict.sys.oper.type.7
            ("dict.sys.oper.type.7", "zh-HK", "授权", "操作类型.授权"),

            // dict.sys.oper.type.8
            ("dict.sys.oper.type.8", "en-US", "强退", "操作类型.强退"),
            // dict.sys.oper.type.8
            ("dict.sys.oper.type.8", "ja-JP", "强退", "操作类型.强退"),
            // dict.sys.oper.type.8
            ("dict.sys.oper.type.8", "zh-CN", "强退", "操作类型.强退"),
            // dict.sys.oper.type.8
            ("dict.sys.oper.type.8", "zh-HK", "强退", "操作类型.强退"),

            // dict.sys.oper.type.9
            ("dict.sys.oper.type.9", "en-US", "生成代码", "操作类型.生成代码"),
            // dict.sys.oper.type.9
            ("dict.sys.oper.type.9", "ja-JP", "生成代码", "操作类型.生成代码"),
            // dict.sys.oper.type.9
            ("dict.sys.oper.type.9", "zh-CN", "生成代码", "操作类型.生成代码"),
            // dict.sys.oper.type.9
            ("dict.sys.oper.type.9", "zh-HK", "生成代码", "操作类型.生成代码"),

            // dict.sys.oper.type.10
            ("dict.sys.oper.type.10", "en-US", "清空数据", "操作类型.清空数据"),
            // dict.sys.oper.type.10
            ("dict.sys.oper.type.10", "ja-JP", "清空数据", "操作类型.清空数据"),
            // dict.sys.oper.type.10
            ("dict.sys.oper.type.10", "zh-CN", "清空数据", "操作类型.清空数据"),
            // dict.sys.oper.type.10
            ("dict.sys.oper.type.10", "zh-HK", "清空数据", "操作类型.清空数据"),

            // dict.sys.oss.provider.aliyun
            ("dict.sys.oss.provider.aliyun", "en-US", "aliyun", "oss提供商类型.阿里云oss"),
            // dict.sys.oss.provider.aliyun
            ("dict.sys.oss.provider.aliyun", "ja-JP", "aliyun", "oss提供商类型.阿里云oss"),
            // dict.sys.oss.provider.aliyun
            ("dict.sys.oss.provider.aliyun", "zh-CN", "阿里云oss", "oss提供商类型.阿里云oss"),
            // dict.sys.oss.provider.aliyun
            ("dict.sys.oss.provider.aliyun", "zh-HK", "阿里云oss", "oss提供商类型.阿里云oss"),

            // dict.sys.oss.provider.tencent
            ("dict.sys.oss.provider.tencent", "en-US", "tencent", "oss提供商类型.腾讯云cos"),
            // dict.sys.oss.provider.tencent
            ("dict.sys.oss.provider.tencent", "ja-JP", "tencent", "oss提供商类型.腾讯云cos"),
            // dict.sys.oss.provider.tencent
            ("dict.sys.oss.provider.tencent", "zh-CN", "腾讯云cos", "oss提供商类型.腾讯云cos"),
            // dict.sys.oss.provider.tencent
            ("dict.sys.oss.provider.tencent", "zh-HK", "腾讯云cos", "oss提供商类型.腾讯云cos"),

            // dict.sys.oss.provider.huawei
            ("dict.sys.oss.provider.huawei", "en-US", "huawei", "oss提供商类型.华为云obs"),
            // dict.sys.oss.provider.huawei
            ("dict.sys.oss.provider.huawei", "ja-JP", "huawei", "oss提供商类型.华为云obs"),
            // dict.sys.oss.provider.huawei
            ("dict.sys.oss.provider.huawei", "zh-CN", "华为云obs", "oss提供商类型.华为云obs"),
            // dict.sys.oss.provider.huawei
            ("dict.sys.oss.provider.huawei", "zh-HK", "华为云obs", "oss提供商类型.华为云obs"),

            // dict.sys.oss.provider.aws
            ("dict.sys.oss.provider.aws", "en-US", "aws", "oss提供商类型.aws s3"),
            // dict.sys.oss.provider.aws
            ("dict.sys.oss.provider.aws", "ja-JP", "aws", "oss提供商类型.aws s3"),
            // dict.sys.oss.provider.aws
            ("dict.sys.oss.provider.aws", "zh-CN", "aws s3", "oss提供商类型.aws s3"),
            // dict.sys.oss.provider.aws
            ("dict.sys.oss.provider.aws", "zh-HK", "aws s3", "oss提供商类型.aws s3"),

            // dict.sys.post.category.management
            ("dict.sys.post.category.management", "en-US", "management", "岗位类别.管理类"),
            // dict.sys.post.category.management
            ("dict.sys.post.category.management", "ja-JP", "management", "岗位类别.管理类"),
            // dict.sys.post.category.management
            ("dict.sys.post.category.management", "zh-CN", "管理类", "岗位类别.管理类"),
            // dict.sys.post.category.management
            ("dict.sys.post.category.management", "zh-HK", "管理类", "岗位类别.管理类"),

            // dict.sys.post.category.technical
            ("dict.sys.post.category.technical", "en-US", "technical", "岗位类别.技术类"),
            // dict.sys.post.category.technical
            ("dict.sys.post.category.technical", "ja-JP", "technical", "岗位类别.技术类"),
            // dict.sys.post.category.technical
            ("dict.sys.post.category.technical", "zh-CN", "技术类", "岗位类别.技术类"),
            // dict.sys.post.category.technical
            ("dict.sys.post.category.technical", "zh-HK", "技术类", "岗位类别.技术类"),

            // dict.sys.post.category.business
            ("dict.sys.post.category.business", "en-US", "business", "岗位类别.业务类"),
            // dict.sys.post.category.business
            ("dict.sys.post.category.business", "ja-JP", "business", "岗位类别.业务类"),
            // dict.sys.post.category.business
            ("dict.sys.post.category.business", "zh-CN", "业务类", "岗位类别.业务类"),
            // dict.sys.post.category.business
            ("dict.sys.post.category.business", "zh-HK", "业务类", "岗位类别.业务类"),

            // dict.sys.post.category.support
            ("dict.sys.post.category.support", "en-US", "support", "岗位类别.支持类"),
            // dict.sys.post.category.support
            ("dict.sys.post.category.support", "ja-JP", "support", "岗位类别.支持类"),
            // dict.sys.post.category.support
            ("dict.sys.post.category.support", "zh-CN", "支持类", "岗位类别.支持类"),
            // dict.sys.post.category.support
            ("dict.sys.post.category.support", "zh-HK", "支持类", "岗位类别.支持类"),

            // dict.sys.post.level.1
            ("dict.sys.post.level.1", "en-US", "初级", "岗位级别.初级"),
            // dict.sys.post.level.1
            ("dict.sys.post.level.1", "ja-JP", "初级", "岗位级别.初级"),
            // dict.sys.post.level.1
            ("dict.sys.post.level.1", "zh-CN", "初级", "岗位级别.初级"),
            // dict.sys.post.level.1
            ("dict.sys.post.level.1", "zh-HK", "初级", "岗位级别.初级"),

            // dict.sys.post.level.2
            ("dict.sys.post.level.2", "en-US", "中级", "岗位级别.中级"),
            // dict.sys.post.level.2
            ("dict.sys.post.level.2", "ja-JP", "中级", "岗位级别.中级"),
            // dict.sys.post.level.2
            ("dict.sys.post.level.2", "zh-CN", "中级", "岗位级别.中级"),
            // dict.sys.post.level.2
            ("dict.sys.post.level.2", "zh-HK", "中级", "岗位级别.中级"),

            // dict.sys.post.level.3
            ("dict.sys.post.level.3", "en-US", "高级", "岗位级别.高级"),
            // dict.sys.post.level.3
            ("dict.sys.post.level.3", "ja-JP", "高级", "岗位级别.高级"),
            // dict.sys.post.level.3
            ("dict.sys.post.level.3", "zh-CN", "高级", "岗位级别.高级"),
            // dict.sys.post.level.3
            ("dict.sys.post.level.3", "zh-HK", "高级", "岗位级别.高级"),

            // dict.sys.post.level.4
            ("dict.sys.post.level.4", "en-US", "专家", "岗位级别.专家"),
            // dict.sys.post.level.4
            ("dict.sys.post.level.4", "ja-JP", "专家", "岗位级别.专家"),
            // dict.sys.post.level.4
            ("dict.sys.post.level.4", "zh-CN", "专家", "岗位级别.专家"),
            // dict.sys.post.level.4
            ("dict.sys.post.level.4", "zh-HK", "专家", "岗位级别.专家"),

            // dict.sys.post.level.5
            ("dict.sys.post.level.5", "en-US", "资深", "岗位级别.资深"),
            // dict.sys.post.level.5
            ("dict.sys.post.level.5", "ja-JP", "资深", "岗位级别.资深"),
            // dict.sys.post.level.5
            ("dict.sys.post.level.5", "zh-CN", "资深", "岗位级别.资深"),
            // dict.sys.post.level.5
            ("dict.sys.post.level.5", "zh-HK", "资深", "岗位级别.资深"),

            // dict.sys.priority.0
            ("dict.sys.priority.0", "en-US", "低", "优先级.低"),
            // dict.sys.priority.0
            ("dict.sys.priority.0", "ja-JP", "低", "优先级.低"),
            // dict.sys.priority.0
            ("dict.sys.priority.0", "zh-CN", "低", "优先级.低"),
            // dict.sys.priority.0
            ("dict.sys.priority.0", "zh-HK", "低", "优先级.低"),

            // dict.sys.priority.1
            ("dict.sys.priority.1", "en-US", "中", "优先级.中"),
            // dict.sys.priority.1
            ("dict.sys.priority.1", "ja-JP", "中", "优先级.中"),
            // dict.sys.priority.1
            ("dict.sys.priority.1", "zh-CN", "中", "优先级.中"),
            // dict.sys.priority.1
            ("dict.sys.priority.1", "zh-HK", "中", "优先级.中"),

            // dict.sys.priority.2
            ("dict.sys.priority.2", "en-US", "高", "优先级.高"),
            // dict.sys.priority.2
            ("dict.sys.priority.2", "ja-JP", "高", "优先级.高"),
            // dict.sys.priority.2
            ("dict.sys.priority.2", "zh-CN", "高", "优先级.高"),
            // dict.sys.priority.2
            ("dict.sys.priority.2", "zh-HK", "高", "优先级.高"),

            // dict.sys.priority.3
            ("dict.sys.priority.3", "en-US", "紧急", "优先级.紧急"),
            // dict.sys.priority.3
            ("dict.sys.priority.3", "ja-JP", "紧急", "优先级.紧急"),
            // dict.sys.priority.3
            ("dict.sys.priority.3", "zh-CN", "紧急", "优先级.紧急"),
            // dict.sys.priority.3
            ("dict.sys.priority.3", "zh-HK", "紧急", "优先级.紧急"),

            // dict.sys.publish.scope.0
            ("dict.sys.publish.scope.0", "en-US", "全部", "发布范围.全部"),
            // dict.sys.publish.scope.0
            ("dict.sys.publish.scope.0", "ja-JP", "全部", "发布范围.全部"),
            // dict.sys.publish.scope.0
            ("dict.sys.publish.scope.0", "zh-CN", "全部", "发布范围.全部"),
            // dict.sys.publish.scope.0
            ("dict.sys.publish.scope.0", "zh-HK", "全部", "发布范围.全部"),

            // dict.sys.publish.scope.1
            ("dict.sys.publish.scope.1", "en-US", "指定部门", "发布范围.指定部门"),
            // dict.sys.publish.scope.1
            ("dict.sys.publish.scope.1", "ja-JP", "指定部门", "发布范围.指定部门"),
            // dict.sys.publish.scope.1
            ("dict.sys.publish.scope.1", "zh-CN", "指定部门", "发布范围.指定部门"),
            // dict.sys.publish.scope.1
            ("dict.sys.publish.scope.1", "zh-HK", "指定部门", "发布范围.指定部门"),

            // dict.sys.publish.scope.2
            ("dict.sys.publish.scope.2", "en-US", "指定用户", "发布范围.指定用户"),
            // dict.sys.publish.scope.2
            ("dict.sys.publish.scope.2", "ja-JP", "指定用户", "发布范围.指定用户"),
            // dict.sys.publish.scope.2
            ("dict.sys.publish.scope.2", "zh-CN", "指定用户", "发布范围.指定用户"),
            // dict.sys.publish.scope.2
            ("dict.sys.publish.scope.2", "zh-HK", "指定用户", "发布范围.指定用户"),

            // dict.sys.publish.scope.3
            ("dict.sys.publish.scope.3", "en-US", "指定角色", "发布范围.指定角色"),
            // dict.sys.publish.scope.3
            ("dict.sys.publish.scope.3", "ja-JP", "指定角色", "发布范围.指定角色"),
            // dict.sys.publish.scope.3
            ("dict.sys.publish.scope.3", "zh-CN", "指定角色", "发布范围.指定角色"),
            // dict.sys.publish.scope.3
            ("dict.sys.publish.scope.3", "zh-HK", "指定角色", "发布范围.指定角色"),

            // dict.sys.read.status.0
            ("dict.sys.read.status.0", "en-US", "未读", "读取状态.未读"),
            // dict.sys.read.status.0
            ("dict.sys.read.status.0", "ja-JP", "未读", "读取状态.未读"),
            // dict.sys.read.status.0
            ("dict.sys.read.status.0", "zh-CN", "未读", "读取状态.未读"),
            // dict.sys.read.status.0
            ("dict.sys.read.status.0", "zh-HK", "未读", "读取状态.未读"),

            // dict.sys.read.status.1
            ("dict.sys.read.status.1", "en-US", "已读", "读取状态.已读"),
            // dict.sys.read.status.1
            ("dict.sys.read.status.1", "ja-JP", "已读", "读取状态.已读"),
            // dict.sys.read.status.1
            ("dict.sys.read.status.1", "zh-CN", "已读", "读取状态.已读"),
            // dict.sys.read.status.1
            ("dict.sys.read.status.1", "zh-HK", "已读", "读取状态.已读"),

            // dict.sys.resource.type.frontend
            ("dict.sys.resource.type.frontend", "en-US", "frontend", "资源类型.前端"),
            // dict.sys.resource.type.frontend
            ("dict.sys.resource.type.frontend", "ja-JP", "frontend", "资源类型.前端"),
            // dict.sys.resource.type.frontend
            ("dict.sys.resource.type.frontend", "zh-CN", "前端", "资源类型.前端"),
            // dict.sys.resource.type.frontend
            ("dict.sys.resource.type.frontend", "zh-HK", "前端", "资源类型.前端"),

            // dict.sys.resource.type.backend
            ("dict.sys.resource.type.backend", "en-US", "backend", "资源类型.后端"),
            // dict.sys.resource.type.backend
            ("dict.sys.resource.type.backend", "ja-JP", "backend", "资源类型.后端"),
            // dict.sys.resource.type.backend
            ("dict.sys.resource.type.backend", "zh-CN", "后端", "资源类型.后端"),
            // dict.sys.resource.type.backend
            ("dict.sys.resource.type.backend", "zh-HK", "后端", "资源类型.后端"),

            // dict.sys.scheme.status.0
            ("dict.sys.scheme.status.0", "en-US", "草稿", "方案状态.草稿"),
            // dict.sys.scheme.status.0
            ("dict.sys.scheme.status.0", "ja-JP", "草稿", "方案状态.草稿"),
            // dict.sys.scheme.status.0
            ("dict.sys.scheme.status.0", "zh-CN", "草稿", "方案状态.草稿"),
            // dict.sys.scheme.status.0
            ("dict.sys.scheme.status.0", "zh-HK", "草稿", "方案状态.草稿"),

            // dict.sys.scheme.status.1
            ("dict.sys.scheme.status.1", "en-US", "已发布", "方案状态.已发布"),
            // dict.sys.scheme.status.1
            ("dict.sys.scheme.status.1", "ja-JP", "已发布", "方案状态.已发布"),
            // dict.sys.scheme.status.1
            ("dict.sys.scheme.status.1", "zh-CN", "已发布", "方案状态.已发布"),
            // dict.sys.scheme.status.1
            ("dict.sys.scheme.status.1", "zh-HK", "已发布", "方案状态.已发布"),

            // dict.sys.scheme.status.2
            ("dict.sys.scheme.status.2", "en-US", "已禁用", "方案状态.已禁用"),
            // dict.sys.scheme.status.2
            ("dict.sys.scheme.status.2", "ja-JP", "已禁用", "方案状态.已禁用"),
            // dict.sys.scheme.status.2
            ("dict.sys.scheme.status.2", "zh-CN", "已禁用", "方案状态.已禁用"),
            // dict.sys.scheme.status.2
            ("dict.sys.scheme.status.2", "zh-HK", "已禁用", "方案状态.已禁用"),

            // dict.sys.setting.group.backend
            ("dict.sys.setting.group.backend", "en-US", "backend", "设置分组.后端"),
            // dict.sys.setting.group.backend
            ("dict.sys.setting.group.backend", "ja-JP", "backend", "设置分组.后端"),
            // dict.sys.setting.group.backend
            ("dict.sys.setting.group.backend", "zh-CN", "后端", "设置分组.后端"),
            // dict.sys.setting.group.backend
            ("dict.sys.setting.group.backend", "zh-HK", "后端", "设置分组.后端"),

            // dict.sys.setting.group.frontend
            ("dict.sys.setting.group.frontend", "en-US", "frontend", "设置分组.前端"),
            // dict.sys.setting.group.frontend
            ("dict.sys.setting.group.frontend", "ja-JP", "frontend", "设置分组.前端"),
            // dict.sys.setting.group.frontend
            ("dict.sys.setting.group.frontend", "zh-CN", "前端", "设置分组.前端"),
            // dict.sys.setting.group.frontend
            ("dict.sys.setting.group.frontend", "zh-HK", "前端", "设置分组.前端"),

            // dict.sys.sort.type.asc
            ("dict.sys.sort.type.asc", "en-US", "asc", "排序类型.升序"),
            // dict.sys.sort.type.asc
            ("dict.sys.sort.type.asc", "ja-JP", "asc", "排序类型.升序"),
            // dict.sys.sort.type.asc
            ("dict.sys.sort.type.asc", "zh-CN", "升序", "排序类型.升序"),
            // dict.sys.sort.type.asc
            ("dict.sys.sort.type.asc", "zh-HK", "升序", "排序类型.升序"),

            // dict.sys.sort.type.desc
            ("dict.sys.sort.type.desc", "en-US", "desc", "排序类型.降序"),
            // dict.sys.sort.type.desc
            ("dict.sys.sort.type.desc", "ja-JP", "desc", "排序类型.降序"),
            // dict.sys.sort.type.desc
            ("dict.sys.sort.type.desc", "zh-CN", "降序", "排序类型.降序"),
            // dict.sys.sort.type.desc
            ("dict.sys.sort.type.desc", "zh-HK", "降序", "排序类型.降序"),

            // dict.sys.storage.directory.default
            ("dict.sys.storage.directory.default", "en-US", "default", "存储目录.默认目录"),
            // dict.sys.storage.directory.default
            ("dict.sys.storage.directory.default", "ja-JP", "default", "存储目录.默认目录"),
            // dict.sys.storage.directory.default
            ("dict.sys.storage.directory.default", "zh-CN", "默认目录", "存储目录.默认目录"),
            // dict.sys.storage.directory.default
            ("dict.sys.storage.directory.default", "zh-HK", "默认目录", "存储目录.默认目录"),

            // dict.sys.storage.directory.documents
            ("dict.sys.storage.directory.documents", "en-US", "documents", "存储目录.文档目录"),
            // dict.sys.storage.directory.documents
            ("dict.sys.storage.directory.documents", "ja-JP", "documents", "存储目录.文档目录"),
            // dict.sys.storage.directory.documents
            ("dict.sys.storage.directory.documents", "zh-CN", "文档目录", "存储目录.文档目录"),
            // dict.sys.storage.directory.documents
            ("dict.sys.storage.directory.documents", "zh-HK", "文档目录", "存储目录.文档目录"),

            // dict.sys.storage.directory.images
            ("dict.sys.storage.directory.images", "en-US", "images", "存储目录.图片目录"),
            // dict.sys.storage.directory.images
            ("dict.sys.storage.directory.images", "ja-JP", "images", "存储目录.图片目录"),
            // dict.sys.storage.directory.images
            ("dict.sys.storage.directory.images", "zh-CN", "图片目录", "存储目录.图片目录"),
            // dict.sys.storage.directory.images
            ("dict.sys.storage.directory.images", "zh-HK", "图片目录", "存储目录.图片目录"),

            // dict.sys.storage.directory.videos
            ("dict.sys.storage.directory.videos", "en-US", "videos", "存储目录.视频目录"),
            // dict.sys.storage.directory.videos
            ("dict.sys.storage.directory.videos", "ja-JP", "videos", "存储目录.视频目录"),
            // dict.sys.storage.directory.videos
            ("dict.sys.storage.directory.videos", "zh-CN", "视频目录", "存储目录.视频目录"),
            // dict.sys.storage.directory.videos
            ("dict.sys.storage.directory.videos", "zh-HK", "视频目录", "存储目录.视频目录"),

            // dict.sys.storage.directory.audios
            ("dict.sys.storage.directory.audios", "en-US", "audios", "存储目录.音频目录"),
            // dict.sys.storage.directory.audios
            ("dict.sys.storage.directory.audios", "ja-JP", "audios", "存储目录.音频目录"),
            // dict.sys.storage.directory.audios
            ("dict.sys.storage.directory.audios", "zh-CN", "音频目录", "存储目录.音频目录"),
            // dict.sys.storage.directory.audios
            ("dict.sys.storage.directory.audios", "zh-HK", "音频目录", "存储目录.音频目录"),

            // dict.sys.storage.directory.archives
            ("dict.sys.storage.directory.archives", "en-US", "archives", "存储目录.压缩包目录"),
            // dict.sys.storage.directory.archives
            ("dict.sys.storage.directory.archives", "ja-JP", "archives", "存储目录.压缩包目录"),
            // dict.sys.storage.directory.archives
            ("dict.sys.storage.directory.archives", "zh-CN", "压缩包目录", "存储目录.压缩包目录"),
            // dict.sys.storage.directory.archives
            ("dict.sys.storage.directory.archives", "zh-HK", "压缩包目录", "存储目录.压缩包目录"),

            // dict.sys.storage.directory.temp
            ("dict.sys.storage.directory.temp", "en-US", "temp", "存储目录.临时目录"),
            // dict.sys.storage.directory.temp
            ("dict.sys.storage.directory.temp", "ja-JP", "temp", "存储目录.临时目录"),
            // dict.sys.storage.directory.temp
            ("dict.sys.storage.directory.temp", "zh-CN", "临时目录", "存储目录.临时目录"),
            // dict.sys.storage.directory.temp
            ("dict.sys.storage.directory.temp", "zh-HK", "临时目录", "存储目录.临时目录"),

            // dict.sys.storage.naming.0
            ("dict.sys.storage.naming.0", "en-US", "原文件+哈希值", "存储命名规则.原文件+哈希值"),
            // dict.sys.storage.naming.0
            ("dict.sys.storage.naming.0", "ja-JP", "原文件+哈希值", "存储命名规则.原文件+哈希值"),
            // dict.sys.storage.naming.0
            ("dict.sys.storage.naming.0", "zh-CN", "原文件+哈希值", "存储命名规则.原文件+哈希值"),
            // dict.sys.storage.naming.0
            ("dict.sys.storage.naming.0", "zh-HK", "原文件+哈希值", "存储命名规则.原文件+哈希值"),

            // dict.sys.storage.naming.1
            ("dict.sys.storage.naming.1", "en-US", "自动生成", "存储命名规则.自动生成"),
            // dict.sys.storage.naming.1
            ("dict.sys.storage.naming.1", "ja-JP", "自动生成", "存储命名规则.自动生成"),
            // dict.sys.storage.naming.1
            ("dict.sys.storage.naming.1", "zh-CN", "自动生成", "存储命名规则.自动生成"),
            // dict.sys.storage.naming.1
            ("dict.sys.storage.naming.1", "zh-HK", "自动生成", "存储命名规则.自动生成"),

            // dict.sys.storage.naming.2
            ("dict.sys.storage.naming.2", "en-US", "自定义", "存储命名规则.自定义"),
            // dict.sys.storage.naming.2
            ("dict.sys.storage.naming.2", "ja-JP", "自定义", "存储命名规则.自定义"),
            // dict.sys.storage.naming.2
            ("dict.sys.storage.naming.2", "zh-CN", "自定义", "存储命名规则.自定义"),
            // dict.sys.storage.naming.2
            ("dict.sys.storage.naming.2", "zh-HK", "自定义", "存储命名规则.自定义"),

            // dict.sys.storage.type.0
            ("dict.sys.storage.type.0", "en-US", "本地存储", "存储方式.本地存储"),
            // dict.sys.storage.type.0
            ("dict.sys.storage.type.0", "ja-JP", "本地存储", "存储方式.本地存储"),
            // dict.sys.storage.type.0
            ("dict.sys.storage.type.0", "zh-CN", "本地存储", "存储方式.本地存储"),
            // dict.sys.storage.type.0
            ("dict.sys.storage.type.0", "zh-HK", "本地存储", "存储方式.本地存储"),

            // dict.sys.storage.type.1
            ("dict.sys.storage.type.1", "en-US", "oss对象存储", "存储方式.oss对象存储"),
            // dict.sys.storage.type.1
            ("dict.sys.storage.type.1", "ja-JP", "oss对象存储", "存储方式.oss对象存储"),
            // dict.sys.storage.type.1
            ("dict.sys.storage.type.1", "zh-CN", "oss对象存储", "存储方式.oss对象存储"),
            // dict.sys.storage.type.1
            ("dict.sys.storage.type.1", "zh-HK", "oss对象存储", "存储方式.oss对象存储"),

            // dict.sys.storage.type.2
            ("dict.sys.storage.type.2", "en-US", "ftp", "存储方式.ftp"),
            // dict.sys.storage.type.2
            ("dict.sys.storage.type.2", "ja-JP", "ftp", "存储方式.ftp"),
            // dict.sys.storage.type.2
            ("dict.sys.storage.type.2", "zh-CN", "ftp", "存储方式.ftp"),
            // dict.sys.storage.type.2
            ("dict.sys.storage.type.2", "zh-HK", "ftp", "存储方式.ftp"),

            // dict.sys.urgency.level.0
            ("dict.sys.urgency.level.0", "en-US", "一般", "紧急程度.一般"),
            // dict.sys.urgency.level.0
            ("dict.sys.urgency.level.0", "ja-JP", "一般", "紧急程度.一般"),
            // dict.sys.urgency.level.0
            ("dict.sys.urgency.level.0", "zh-CN", "一般", "紧急程度.一般"),
            // dict.sys.urgency.level.0
            ("dict.sys.urgency.level.0", "zh-HK", "一般", "紧急程度.一般"),

            // dict.sys.urgency.level.1
            ("dict.sys.urgency.level.1", "en-US", "紧急", "紧急程度.紧急"),
            // dict.sys.urgency.level.1
            ("dict.sys.urgency.level.1", "ja-JP", "紧急", "紧急程度.紧急"),
            // dict.sys.urgency.level.1
            ("dict.sys.urgency.level.1", "zh-CN", "紧急", "紧急程度.紧急"),
            // dict.sys.urgency.level.1
            ("dict.sys.urgency.level.1", "zh-HK", "紧急", "紧急程度.紧急"),

            // dict.sys.urgency.level.2
            ("dict.sys.urgency.level.2", "en-US", "非常紧急", "紧急程度.非常紧急"),
            // dict.sys.urgency.level.2
            ("dict.sys.urgency.level.2", "ja-JP", "非常紧急", "紧急程度.非常紧急"),
            // dict.sys.urgency.level.2
            ("dict.sys.urgency.level.2", "zh-CN", "非常紧急", "紧急程度.非常紧急"),
            // dict.sys.urgency.level.2
            ("dict.sys.urgency.level.2", "zh-HK", "非常紧急", "紧急程度.非常紧急"),

            // dict.sys.user.gender.0
            ("dict.sys.user.gender.0", "en-US", "未知", "用户性别.未知"),
            // dict.sys.user.gender.0
            ("dict.sys.user.gender.0", "ja-JP", "未知", "用户性别.未知"),
            // dict.sys.user.gender.0
            ("dict.sys.user.gender.0", "zh-CN", "未知", "用户性别.未知"),
            // dict.sys.user.gender.0
            ("dict.sys.user.gender.0", "zh-HK", "未知", "用户性别.未知"),

            // dict.sys.user.gender.1
            ("dict.sys.user.gender.1", "en-US", "男", "用户性别.男"),
            // dict.sys.user.gender.1
            ("dict.sys.user.gender.1", "ja-JP", "男", "用户性别.男"),
            // dict.sys.user.gender.1
            ("dict.sys.user.gender.1", "zh-CN", "男", "用户性别.男"),
            // dict.sys.user.gender.1
            ("dict.sys.user.gender.1", "zh-HK", "男", "用户性别.男"),

            // dict.sys.user.gender.2
            ("dict.sys.user.gender.2", "en-US", "女", "用户性别.女"),
            // dict.sys.user.gender.2
            ("dict.sys.user.gender.2", "ja-JP", "女", "用户性别.女"),
            // dict.sys.user.gender.2
            ("dict.sys.user.gender.2", "zh-CN", "女", "用户性别.女"),
            // dict.sys.user.gender.2
            ("dict.sys.user.gender.2", "zh-HK", "女", "用户性别.女"),

            // dict.sys.user.type.0
            ("dict.sys.user.type.0", "en-US", "普通用户", "用户类型.普通用户"),
            // dict.sys.user.type.0
            ("dict.sys.user.type.0", "ja-JP", "普通用户", "用户类型.普通用户"),
            // dict.sys.user.type.0
            ("dict.sys.user.type.0", "zh-CN", "普通用户", "用户类型.普通用户"),
            // dict.sys.user.type.0
            ("dict.sys.user.type.0", "zh-HK", "普通用户", "用户类型.普通用户"),

            // dict.sys.user.type.1
            ("dict.sys.user.type.1", "en-US", "管理员", "用户类型.管理员"),
            // dict.sys.user.type.1
            ("dict.sys.user.type.1", "ja-JP", "管理员", "用户类型.管理员"),
            // dict.sys.user.type.1
            ("dict.sys.user.type.1", "zh-CN", "管理员", "用户类型.管理员"),
            // dict.sys.user.type.1
            ("dict.sys.user.type.1", "zh-HK", "管理员", "用户类型.管理员"),

            // dict.sys.user.type.2
            ("dict.sys.user.type.2", "en-US", "超级管理员", "用户类型.超级管理员"),
            // dict.sys.user.type.2
            ("dict.sys.user.type.2", "ja-JP", "超级管理员", "用户类型.超级管理员"),
            // dict.sys.user.type.2
            ("dict.sys.user.type.2", "zh-CN", "超级管理员", "用户类型.超级管理员"),
            // dict.sys.user.type.2
            ("dict.sys.user.type.2", "zh-HK", "超级管理员", "用户类型.超级管理员"),

            // dict.sys.word.category.1
            ("dict.sys.word.category.1", "en-US", "政治敏感", "敏感词词性类别.政治敏感"),
            // dict.sys.word.category.1
            ("dict.sys.word.category.1", "ja-JP", "政治敏感", "敏感词词性类别.政治敏感"),
            // dict.sys.word.category.1
            ("dict.sys.word.category.1", "zh-CN", "政治敏感", "敏感词词性类别.政治敏感"),
            // dict.sys.word.category.1
            ("dict.sys.word.category.1", "zh-HK", "政治敏感", "敏感词词性类别.政治敏感"),

            // dict.sys.word.category.2
            ("dict.sys.word.category.2", "en-US", "暴力恐怖", "敏感词词性类别.暴力恐怖"),
            // dict.sys.word.category.2
            ("dict.sys.word.category.2", "ja-JP", "暴力恐怖", "敏感词词性类别.暴力恐怖"),
            // dict.sys.word.category.2
            ("dict.sys.word.category.2", "zh-CN", "暴力恐怖", "敏感词词性类别.暴力恐怖"),
            // dict.sys.word.category.2
            ("dict.sys.word.category.2", "zh-HK", "暴力恐怖", "敏感词词性类别.暴力恐怖"),

            // dict.sys.word.category.3
            ("dict.sys.word.category.3", "en-US", "色情低俗", "敏感词词性类别.色情低俗"),
            // dict.sys.word.category.3
            ("dict.sys.word.category.3", "ja-JP", "色情低俗", "敏感词词性类别.色情低俗"),
            // dict.sys.word.category.3
            ("dict.sys.word.category.3", "zh-CN", "色情低俗", "敏感词词性类别.色情低俗"),
            // dict.sys.word.category.3
            ("dict.sys.word.category.3", "zh-HK", "色情低俗", "敏感词词性类别.色情低俗"),

            // dict.sys.word.category.4
            ("dict.sys.word.category.4", "en-US", "广告营销", "敏感词词性类别.广告营销"),
            // dict.sys.word.category.4
            ("dict.sys.word.category.4", "ja-JP", "广告营销", "敏感词词性类别.广告营销"),
            // dict.sys.word.category.4
            ("dict.sys.word.category.4", "zh-CN", "广告营销", "敏感词词性类别.广告营销"),
            // dict.sys.word.category.4
            ("dict.sys.word.category.4", "zh-HK", "广告营销", "敏感词词性类别.广告营销"),

            // dict.sys.word.category.5
            ("dict.sys.word.category.5", "en-US", "辱骂歧视", "敏感词词性类别.辱骂歧视"),
            // dict.sys.word.category.5
            ("dict.sys.word.category.5", "ja-JP", "辱骂歧视", "敏感词词性类别.辱骂歧视"),
            // dict.sys.word.category.5
            ("dict.sys.word.category.5", "zh-CN", "辱骂歧视", "敏感词词性类别.辱骂歧视"),
            // dict.sys.word.category.5
            ("dict.sys.word.category.5", "zh-HK", "辱骂歧视", "敏感词词性类别.辱骂歧视"),

            // dict.sys.word.filter.level.1
            ("dict.sys.word.filter.level.1", "en-US", "低", "敏感词过滤等级.低"),
            // dict.sys.word.filter.level.1
            ("dict.sys.word.filter.level.1", "ja-JP", "低", "敏感词过滤等级.低"),
            // dict.sys.word.filter.level.1
            ("dict.sys.word.filter.level.1", "zh-CN", "低", "敏感词过滤等级.低"),
            // dict.sys.word.filter.level.1
            ("dict.sys.word.filter.level.1", "zh-HK", "低", "敏感词过滤等级.低"),

            // dict.sys.word.filter.level.2
            ("dict.sys.word.filter.level.2", "en-US", "中", "敏感词过滤等级.中"),
            // dict.sys.word.filter.level.2
            ("dict.sys.word.filter.level.2", "ja-JP", "中", "敏感词过滤等级.中"),
            // dict.sys.word.filter.level.2
            ("dict.sys.word.filter.level.2", "zh-CN", "中", "敏感词过滤等级.中"),
            // dict.sys.word.filter.level.2
            ("dict.sys.word.filter.level.2", "zh-HK", "中", "敏感词过滤等级.中"),

            // dict.sys.word.filter.level.3
            ("dict.sys.word.filter.level.3", "en-US", "高", "敏感词过滤等级.高"),
            // dict.sys.word.filter.level.3
            ("dict.sys.word.filter.level.3", "ja-JP", "高", "敏感词过滤等级.高"),
            // dict.sys.word.filter.level.3
            ("dict.sys.word.filter.level.3", "zh-CN", "高", "敏感词过滤等级.高"),
            // dict.sys.word.filter.level.3
            ("dict.sys.word.filter.level.3", "zh-HK", "高", "敏感词过滤等级.高"),

            // dict.sys.yes.no.1
            ("dict.sys.yes.no.1", "en-US", "是", "是否.是"),
            // dict.sys.yes.no.1
            ("dict.sys.yes.no.1", "ja-JP", "是", "是否.是"),
            // dict.sys.yes.no.1
            ("dict.sys.yes.no.1", "zh-CN", "是", "是否.是"),
            // dict.sys.yes.no.1
            ("dict.sys.yes.no.1", "zh-HK", "是", "是否.是"),

            // dict.sys.yes.no.0
            ("dict.sys.yes.no.0", "en-US", "否", "是否.否"),
            // dict.sys.yes.no.0
            ("dict.sys.yes.no.0", "ja-JP", "否", "是否.否"),
            // dict.sys.yes.no.0
            ("dict.sys.yes.no.0", "zh-CN", "否", "是否.否"),
            // dict.sys.yes.no.0
            ("dict.sys.yes.no.0", "zh-HK", "否", "是否.否"),

            // dict.helpdesk.ticket.status.0
            ("dict.helpdesk.ticket.status.0", "en-US", "Open", "工单状态.新建"),
            ("dict.helpdesk.ticket.status.0", "ja-JP", "新規", "工单状态.新建"),
            ("dict.helpdesk.ticket.status.0", "zh-CN", "新建", "工单状态.新建"),
            ("dict.helpdesk.ticket.status.0", "zh-HK", "新建", "工单状态.新建"),
            ("dict.helpdesk.ticket.status.1", "en-US", "Assigned", "工单状态.已指派"),
            ("dict.helpdesk.ticket.status.1", "ja-JP", "割当済", "工单状态.已指派"),
            ("dict.helpdesk.ticket.status.1", "zh-CN", "已指派", "工单状态.已指派"),
            ("dict.helpdesk.ticket.status.1", "zh-HK", "已指派", "工单状态.已指派"),
            ("dict.helpdesk.ticket.status.2", "en-US", "In Progress", "工单状态.处理中"),
            ("dict.helpdesk.ticket.status.2", "ja-JP", "処理中", "工单状态.处理中"),
            ("dict.helpdesk.ticket.status.2", "zh-CN", "处理中", "工单状态.处理中"),
            ("dict.helpdesk.ticket.status.2", "zh-HK", "處理中", "工单状态.处理中"),
            ("dict.helpdesk.ticket.status.3", "en-US", "Waiting for Requester", "工单状态.等待用户回复"),
            ("dict.helpdesk.ticket.status.3", "ja-JP", "ユーザー返信待ち", "工单状态.等待用户回复"),
            ("dict.helpdesk.ticket.status.3", "zh-CN", "等待用户回复", "工单状态.等待用户回复"),
            ("dict.helpdesk.ticket.status.3", "zh-HK", "等待用戶回覆", "工单状态.等待用户回复"),
            ("dict.helpdesk.ticket.status.4", "en-US", "Resolved", "工单状态.已解决"),
            ("dict.helpdesk.ticket.status.4", "ja-JP", "解決済", "工单状态.已解决"),
            ("dict.helpdesk.ticket.status.4", "zh-CN", "已解决", "工单状态.已解决"),
            ("dict.helpdesk.ticket.status.4", "zh-HK", "已解決", "工单状态.已解决"),
            ("dict.helpdesk.ticket.status.5", "en-US", "Closed", "工单状态.已关闭"),
            ("dict.helpdesk.ticket.status.5", "ja-JP", "クローズ", "工单状态.已关闭"),
            ("dict.helpdesk.ticket.status.5", "zh-CN", "已关闭", "工单状态.已关闭"),
            ("dict.helpdesk.ticket.status.5", "zh-HK", "已關閉", "工单状态.已关闭"),
            ("dict.helpdesk.ticket.status.6", "en-US", "Reopened", "工单状态.重新打开"),
            ("dict.helpdesk.ticket.status.6", "ja-JP", "再オープン", "工单状态.重新打开"),
            ("dict.helpdesk.ticket.status.6", "zh-CN", "重新打开", "工单状态.重新打开"),
            ("dict.helpdesk.ticket.status.6", "zh-HK", "重新打開", "工单状态.重新打开"),
            ("dict.helpdesk.ticket.source.0", "en-US", "Portal", "工单来源.门户"),
            ("dict.helpdesk.ticket.source.0", "ja-JP", "ポータル", "工单来源.门户"),
            ("dict.helpdesk.ticket.source.0", "zh-CN", "门户网站", "工单来源.门户"),
            ("dict.helpdesk.ticket.source.0", "zh-HK", "門戶網站", "工单来源.门户"),
            ("dict.helpdesk.ticket.source.1", "en-US", "Email", "工单来源.邮件"),
            ("dict.helpdesk.ticket.source.1", "ja-JP", "メール", "工单来源.邮件"),
            ("dict.helpdesk.ticket.source.1", "zh-CN", "邮件", "工单来源.邮件"),
            ("dict.helpdesk.ticket.source.1", "zh-HK", "郵件", "工单来源.邮件"),
            ("dict.helpdesk.ticket.source.2", "en-US", "Phone", "工单来源.电话"),
            ("dict.helpdesk.ticket.source.2", "ja-JP", "電話", "工单来源.电话"),
            ("dict.helpdesk.ticket.source.2", "zh-CN", "电话", "工单来源.电话"),
            ("dict.helpdesk.ticket.source.2", "zh-HK", "電話", "工单来源.电话"),
            ("dict.helpdesk.ticket.source.3", "en-US", "API", "工单来源.API"),
            ("dict.helpdesk.ticket.source.3", "ja-JP", "API", "工单来源.API"),
            ("dict.helpdesk.ticket.source.3", "zh-CN", "API接入", "工单来源.API"),
            ("dict.helpdesk.ticket.source.3", "zh-HK", "API接入", "工单来源.API"),
            ("dict.sys.warrantytype.0", "en-US", "Manufacturer Warranty", "保修类型.原厂保修"),
            ("dict.sys.warrantytype.0", "ja-JP", "メーカー標準保証", "保修类型.原厂保修"),
            ("dict.sys.warrantytype.0", "zh-CN", "原厂保修", "保修类型.原厂保修"),
            ("dict.sys.warrantytype.0", "zh-HK", "原廠標準保修", "保修类型.原厂保修"),
            ("dict.sys.warrantytype.1", "en-US", "Extended Warranty", "保修类型.延长保修"),
            ("dict.sys.warrantytype.1", "ja-JP", "延長保証", "保修类型.延长保修"),
            ("dict.sys.warrantytype.1", "zh-CN", "延长保修", "保修类型.延长保修"),
            ("dict.sys.warrantytype.1", "zh-HK", "延長保修", "保修类型.延长保修"),
            ("dict.sys.warrantytype.2", "en-US", "On-Site Warranty", "保修类型.上门保修"),
            ("dict.sys.warrantytype.2", "ja-JP", "オンサイト保証", "保修类型.上门保修"),
            ("dict.sys.warrantytype.2", "zh-CN", "上门保修", "保修类型.上门保修"),
            ("dict.sys.warrantytype.2", "zh-HK", "上門保修", "保修类型.上门保修"),
            ("dict.sys.warrantytype.3", "en-US", "Depot Repair", "保修类型.寄修保修"),
            ("dict.sys.warrantytype.3", "ja-JP", "送付修理", "保修类型.寄修保修"),
            ("dict.sys.warrantytype.3", "zh-CN", "寄修保修", "保修类型.寄修保修"),
            ("dict.sys.warrantytype.3", "zh-HK", "寄修保修", "保修类型.寄修保修"),
            ("dict.sys.warrantytype.4", "en-US", "Maintenance Contract", "保修类型.维保合同"),
            ("dict.sys.warrantytype.4", "ja-JP", "保守契約", "保修类型.维保合同"),
            ("dict.sys.warrantytype.4", "zh-CN", "维保合同", "保修类型.维保合同"),
            ("dict.sys.warrantytype.4", "zh-HK", "維保合同", "保修类型.维保合同"),
            ("dict.sys.warrantytype.5", "en-US", "Paid Warranty", "保修类型.付费保养"),
            ("dict.sys.warrantytype.5", "ja-JP", "有償保証", "保修类型.付费保养"),
            ("dict.sys.warrantytype.5", "zh-CN", "付费保养", "保修类型.付费保养"),
            ("dict.sys.warrantytype.5", "zh-HK", "付費保修", "保修类型.付费保养"),
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
        translation.ResourceGroup = 8;
        translation.ResourceType = 0;
        translation.ContextNote = item.ContextNote;
        translation.ExtFieldJson = null;
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
