// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Materials
// 文件名称：TaktPurchasePriceItemI18nSeedData.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktPurchasePriceItem 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Enums;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Materials;

/// <summary>
/// TaktPurchasePriceItem 实体国际化翻译种子（键前缀 entity.purchasePriceItem.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktPurchasePriceItemI18nSeedData : ITaktSeedDataCoordinator
{
    /// <summary>
    /// 执行顺序（实体翻译种子，位于部门翻译之后）
    /// </summary>
    public int Order => 52;

    /// <summary>
    /// 初始化实体字段翻译种子
    /// </summary>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(IServiceProvider serviceProvider, string? tenantCode = null)
    {
        TaktLogger.Information("开始初始化 TaktPurchasePriceItem 实体国际化翻译种子...");

        if (string.IsNullOrEmpty(tenantCode))
        {
            TaktLogger.Warning("租户编码为空，跳过实体国际化翻译种子初始化");
            return (0, 0);
        }

        var repository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktTranslation>>();
        var cultureRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktCulture>>();
        var cultureIdByCode = (await cultureRepository.GetListAsync(c => c.TenantCode == tenantCode))
            .ToDictionary(c => c.CultureCode, c => c.Id);
        int insertCount = 0;
        int updateCount = 0;

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 purchasePriceItem 实体翻译...", tenantCode);

        foreach (var item in GetPurchasePriceItemTranslations())
        {
            if (!cultureIdByCode.TryGetValue(item.CultureCode, out var cultureId))
            {
                TaktLogger.Warning("未找到区域文化 {CultureCode}，跳过翻译 {I18nKey}", item.CultureCode, item.I18nKey);
                continue;
            }

            var (translation, i, u) = await CreateOrUpdateTranslationAsync(
                repository,
                tenantCode,
                cultureId,
                item);
            insertCount += i;
            updateCount += u;
        }

        TaktLogger.Information("TaktPurchasePriceItem 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktPurchasePriceItem 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.purchasePriceItem._self / entity.purchasePriceItem.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetPurchasePriceItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.purchasePriceItem._self
            new TranslationSeedItem("entity.purchasePriceItem._self", "en-US", "Purchase Price Item Information", "实体名称"),
            // entity.purchasePriceItem._self
            new TranslationSeedItem("entity.purchasePriceItem._self", "ja-JP", "Takt采购价格明细信息", "实体名称"),
            // entity.purchasePriceItem._self
            new TranslationSeedItem("entity.purchasePriceItem._self", "zh-CN", "Takt采购价格明细信息", "实体名称"),
            // entity.purchasePriceItem._self
            new TranslationSeedItem("entity.purchasePriceItem._self", "zh-HK", "Takt采购价格明细信息", "实体名称"),

            // entity.purchasePriceItem.purchasepriceid
            new TranslationSeedItem("entity.purchasePriceItem.purchasepriceid", "en-US", "采购价格ID", "采购价格ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.purchasePriceItem.purchasepriceid
            new TranslationSeedItem("entity.purchasePriceItem.purchasepriceid", "ja-JP", "采购价格ID", "采购价格ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.purchasePriceItem.purchasepriceid
            new TranslationSeedItem("entity.purchasePriceItem.purchasepriceid", "zh-CN", "采购价格ID", "采购价格ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.purchasePriceItem.purchasepriceid
            new TranslationSeedItem("entity.purchasePriceItem.purchasepriceid", "zh-HK", "采购价格ID", "采购价格ID（主子表关系，序列化为string以避免Javascript精度问题）"),

            // entity.purchasePriceItem.purchasepricecode
            new TranslationSeedItem("entity.purchasePriceItem.purchasepricecode", "en-US", "采购价格编码", "采购价格编码（冗余字段，便于查询）"),
            // entity.purchasePriceItem.purchasepricecode
            new TranslationSeedItem("entity.purchasePriceItem.purchasepricecode", "ja-JP", "采购价格编码", "采购价格编码（冗余字段，便于查询）"),
            // entity.purchasePriceItem.purchasepricecode
            new TranslationSeedItem("entity.purchasePriceItem.purchasepricecode", "zh-CN", "采购价格编码", "采购价格编码（冗余字段，便于查询）"),
            // entity.purchasePriceItem.purchasepricecode
            new TranslationSeedItem("entity.purchasePriceItem.purchasepricecode", "zh-HK", "采购价格编码", "采购价格编码（冗余字段，便于查询）"),

            // entity.purchasePriceItem.linenumber
            new TranslationSeedItem("entity.purchasePriceItem.linenumber", "en-US", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.purchasePriceItem.linenumber
            new TranslationSeedItem("entity.purchasePriceItem.linenumber", "ja-JP", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.purchasePriceItem.linenumber
            new TranslationSeedItem("entity.purchasePriceItem.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.purchasePriceItem.linenumber
            new TranslationSeedItem("entity.purchasePriceItem.linenumber", "zh-HK", "行号", "行号（项号/序号，固定步长=10）"),

            // entity.purchasePriceItem.materialcode
            new TranslationSeedItem("entity.purchasePriceItem.materialcode", "en-US", "物料编码", "物料编码"),
            // entity.purchasePriceItem.materialcode
            new TranslationSeedItem("entity.purchasePriceItem.materialcode", "ja-JP", "物料编码", "物料编码"),
            // entity.purchasePriceItem.materialcode
            new TranslationSeedItem("entity.purchasePriceItem.materialcode", "zh-CN", "物料编码", "物料编码"),
            // entity.purchasePriceItem.materialcode
            new TranslationSeedItem("entity.purchasePriceItem.materialcode", "zh-HK", "物料编码", "物料编码"),

            // entity.purchasePriceItem.materialname
            new TranslationSeedItem("entity.purchasePriceItem.materialname", "en-US", "物料名称", "物料名称"),
            // entity.purchasePriceItem.materialname
            new TranslationSeedItem("entity.purchasePriceItem.materialname", "ja-JP", "物料名称", "物料名称"),
            // entity.purchasePriceItem.materialname
            new TranslationSeedItem("entity.purchasePriceItem.materialname", "zh-CN", "物料名称", "物料名称"),
            // entity.purchasePriceItem.materialname
            new TranslationSeedItem("entity.purchasePriceItem.materialname", "zh-HK", "物料名称", "物料名称"),

            // entity.purchasePriceItem.materialspecification
            new TranslationSeedItem("entity.purchasePriceItem.materialspecification", "en-US", "物料规格", "物料规格"),
            // entity.purchasePriceItem.materialspecification
            new TranslationSeedItem("entity.purchasePriceItem.materialspecification", "ja-JP", "物料规格", "物料规格"),
            // entity.purchasePriceItem.materialspecification
            new TranslationSeedItem("entity.purchasePriceItem.materialspecification", "zh-CN", "物料规格", "物料规格"),
            // entity.purchasePriceItem.materialspecification
            new TranslationSeedItem("entity.purchasePriceItem.materialspecification", "zh-HK", "物料规格", "物料规格"),

            // entity.purchasePriceItem.purchaseunit
            new TranslationSeedItem("entity.purchasePriceItem.purchaseunit", "en-US", "采购单位", "采购单位"),
            // entity.purchasePriceItem.purchaseunit
            new TranslationSeedItem("entity.purchasePriceItem.purchaseunit", "ja-JP", "采购单位", "采购单位"),
            // entity.purchasePriceItem.purchaseunit
            new TranslationSeedItem("entity.purchasePriceItem.purchaseunit", "zh-CN", "采购单位", "采购单位"),
            // entity.purchasePriceItem.purchaseunit
            new TranslationSeedItem("entity.purchasePriceItem.purchaseunit", "zh-HK", "采购单位", "采购单位"),

            // entity.purchasePriceItem.purchaseprice
            new TranslationSeedItem("entity.purchasePriceItem.purchaseprice", "en-US", "采购价格", "采购价格（精确到分，存储为整数，单位为分）"),
            // entity.purchasePriceItem.purchaseprice
            new TranslationSeedItem("entity.purchasePriceItem.purchaseprice", "ja-JP", "采购价格", "采购价格（精确到分，存储为整数，单位为分）"),
            // entity.purchasePriceItem.purchaseprice
            new TranslationSeedItem("entity.purchasePriceItem.purchaseprice", "zh-CN", "采购价格", "采购价格（精确到分，存储为整数，单位为分）"),
            // entity.purchasePriceItem.purchaseprice
            new TranslationSeedItem("entity.purchasePriceItem.purchaseprice", "zh-HK", "采购价格", "采购价格（精确到分，存储为整数，单位为分）"),

            // entity.purchasePriceItem.minpurchasequantity
            new TranslationSeedItem("entity.purchasePriceItem.minpurchasequantity", "en-US", "最小采购量", "最小采购量（基本单位数量）"),
            // entity.purchasePriceItem.minpurchasequantity
            new TranslationSeedItem("entity.purchasePriceItem.minpurchasequantity", "ja-JP", "最小采购量", "最小采购量（基本单位数量）"),
            // entity.purchasePriceItem.minpurchasequantity
            new TranslationSeedItem("entity.purchasePriceItem.minpurchasequantity", "zh-CN", "最小采购量", "最小采购量（基本单位数量）"),
            // entity.purchasePriceItem.minpurchasequantity
            new TranslationSeedItem("entity.purchasePriceItem.minpurchasequantity", "zh-HK", "最小采购量", "最小采购量（基本单位数量）"),

            // entity.purchasePriceItem.maxpurchasequantity
            new TranslationSeedItem("entity.purchasePriceItem.maxpurchasequantity", "en-US", "最大采购量", "最大采购量（基本单位数量，0表示无限制）"),
            // entity.purchasePriceItem.maxpurchasequantity
            new TranslationSeedItem("entity.purchasePriceItem.maxpurchasequantity", "ja-JP", "最大采购量", "最大采购量（基本单位数量，0表示无限制）"),
            // entity.purchasePriceItem.maxpurchasequantity
            new TranslationSeedItem("entity.purchasePriceItem.maxpurchasequantity", "zh-CN", "最大采购量", "最大采购量（基本单位数量，0表示无限制）"),
            // entity.purchasePriceItem.maxpurchasequantity
            new TranslationSeedItem("entity.purchasePriceItem.maxpurchasequantity", "zh-HK", "最大采购量", "最大采购量（基本单位数量，0表示无限制）"),

            // entity.purchasePriceItem.sortorder
            new TranslationSeedItem("entity.purchasePriceItem.sortorder", "en-US", "排序号", "排序号（越小越靠前）"),
            // entity.purchasePriceItem.sortorder
            new TranslationSeedItem("entity.purchasePriceItem.sortorder", "ja-JP", "排序号", "排序号（越小越靠前）"),
            // entity.purchasePriceItem.sortorder
            new TranslationSeedItem("entity.purchasePriceItem.sortorder", "zh-CN", "排序号", "排序号（越小越靠前）"),
            // entity.purchasePriceItem.sortorder
            new TranslationSeedItem("entity.purchasePriceItem.sortorder", "zh-HK", "排序号", "排序号（越小越靠前）"),

            // entity.purchasePriceItem.scales
            new TranslationSeedItem("entity.purchasePriceItem.scales", "en-US", "scales", "价格阶梯列表（主子表关系，一个物料价格可以有多个阶梯）"),
            // entity.purchasePriceItem.scales
            new TranslationSeedItem("entity.purchasePriceItem.scales", "ja-JP", "scales", "价格阶梯列表（主子表关系，一个物料价格可以有多个阶梯）"),
            // entity.purchasePriceItem.scales
            new TranslationSeedItem("entity.purchasePriceItem.scales", "zh-CN", "scales", "价格阶梯列表（主子表关系，一个物料价格可以有多个阶梯）"),
            // entity.purchasePriceItem.scales
            new TranslationSeedItem("entity.purchasePriceItem.scales", "zh-HK", "scales", "价格阶梯列表（主子表关系，一个物料价格可以有多个阶梯）"),
        };
    }

    /// <summary>
    /// 填充 TaktTranslation 全部业务字段（含租户基类字段）
    /// </summary>
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
        translation.ResourceGroup = TaktModule.Logistics;
        translation.ResourceType = TaktAppSide.Frontend;
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

    /// <summary>
    /// 翻译种子项（对应 TaktTranslation 全部可写字段，CultureId 由 SeedAsync 解析）
    /// </summary>
    private sealed record TranslationSeedItem(
        string I18nKey,
        string CultureCode,
        string TranslationText,
        string? ContextNote);
}
