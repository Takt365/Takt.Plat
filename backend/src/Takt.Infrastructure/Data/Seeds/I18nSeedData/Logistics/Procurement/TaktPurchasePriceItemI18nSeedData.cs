// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Procurement
// 文件名称：TaktPurchasePriceItemI18nSeedData.cs
// 创建时间：2026-07-09
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
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Procurement;

/// <summary>
/// TaktPurchasePriceItem 实体国际化翻译种子（键前缀 entity.purchasepriceitem.*）
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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 purchasepriceitem 实体翻译...", tenantCode);

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
    /// I18nKey：entity.purchasepriceitem._self / entity.purchasepriceitem.{{field}}；ResourceGroup=Procurement；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetPurchasePriceItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.purchasepriceitem._self
            new TranslationSeedItem("entity.purchasepriceitem._self", "en-US", "Purchase Price Item Information_us", "实体名称"),
            // entity.purchasepriceitem._self
            new TranslationSeedItem("entity.purchasepriceitem._self", "ja-JP", "Takt采购价格明细信息_jp", "实体名称"),
            // entity.purchasepriceitem._self
            new TranslationSeedItem("entity.purchasepriceitem._self", "zh-CN", "Takt采购价格明细信息", "实体名称"),
            // entity.purchasepriceitem._self
            new TranslationSeedItem("entity.purchasepriceitem._self", "zh-HK", "Takt采购价格明细信息_hk", "实体名称"),

            // entity.purchasepriceitem.purchasepriceid
            new TranslationSeedItem("entity.purchasepriceitem.purchasepriceid", "en-US", "采购价格ID_us", "采购价格 ID（关联 TaktPurchasePrice.Id，选项 TaktPurchasePrices/options）"),
            // entity.purchasepriceitem.purchasepriceid
            new TranslationSeedItem("entity.purchasepriceitem.purchasepriceid", "ja-JP", "采购价格ID_jp", "采购价格 ID（关联 TaktPurchasePrice.Id，选项 TaktPurchasePrices/options）"),
            // entity.purchasepriceitem.purchasepriceid
            new TranslationSeedItem("entity.purchasepriceitem.purchasepriceid", "zh-CN", "采购价格ID", "采购价格 ID（关联 TaktPurchasePrice.Id，选项 TaktPurchasePrices/options）"),
            // entity.purchasepriceitem.purchasepriceid
            new TranslationSeedItem("entity.purchasepriceitem.purchasepriceid", "zh-HK", "采购价格ID_hk", "采购价格 ID（关联 TaktPurchasePrice.Id，选项 TaktPurchasePrices/options）"),

            // entity.purchasepriceitem.purchasepricecode
            new TranslationSeedItem("entity.purchasepriceitem.purchasepricecode", "en-US", "采购价格编码_us", "采购价格编码（冗余字段，便于查询）"),
            // entity.purchasepriceitem.purchasepricecode
            new TranslationSeedItem("entity.purchasepriceitem.purchasepricecode", "ja-JP", "采购价格编码_jp", "采购价格编码（冗余字段，便于查询）"),
            // entity.purchasepriceitem.purchasepricecode
            new TranslationSeedItem("entity.purchasepriceitem.purchasepricecode", "zh-CN", "采购价格编码", "采购价格编码（冗余字段，便于查询）"),
            // entity.purchasepriceitem.purchasepricecode
            new TranslationSeedItem("entity.purchasepriceitem.purchasepricecode", "zh-HK", "采购价格编码_hk", "采购价格编码（冗余字段，便于查询）"),

            // entity.purchasepriceitem.linenumber
            new TranslationSeedItem("entity.purchasepriceitem.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.purchasepriceitem.linenumber
            new TranslationSeedItem("entity.purchasepriceitem.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.purchasepriceitem.linenumber
            new TranslationSeedItem("entity.purchasepriceitem.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.purchasepriceitem.linenumber
            new TranslationSeedItem("entity.purchasepriceitem.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.purchasepriceitem.materialcode
            new TranslationSeedItem("entity.purchasepriceitem.materialcode", "en-US", "物料编码_us", "物料编码"),
            // entity.purchasepriceitem.materialcode
            new TranslationSeedItem("entity.purchasepriceitem.materialcode", "ja-JP", "物料编码_jp", "物料编码"),
            // entity.purchasepriceitem.materialcode
            new TranslationSeedItem("entity.purchasepriceitem.materialcode", "zh-CN", "物料编码", "物料编码"),
            // entity.purchasepriceitem.materialcode
            new TranslationSeedItem("entity.purchasepriceitem.materialcode", "zh-HK", "物料编码_hk", "物料编码"),

            // entity.purchasepriceitem.materialname
            new TranslationSeedItem("entity.purchasepriceitem.materialname", "en-US", "物料名称_us", "物料名称"),
            // entity.purchasepriceitem.materialname
            new TranslationSeedItem("entity.purchasepriceitem.materialname", "ja-JP", "物料名称_jp", "物料名称"),
            // entity.purchasepriceitem.materialname
            new TranslationSeedItem("entity.purchasepriceitem.materialname", "zh-CN", "物料名称", "物料名称"),
            // entity.purchasepriceitem.materialname
            new TranslationSeedItem("entity.purchasepriceitem.materialname", "zh-HK", "物料名称_hk", "物料名称"),

            // entity.purchasepriceitem.materialspecification
            new TranslationSeedItem("entity.purchasepriceitem.materialspecification", "en-US", "物料规格_us", "物料规格"),
            // entity.purchasepriceitem.materialspecification
            new TranslationSeedItem("entity.purchasepriceitem.materialspecification", "ja-JP", "物料规格_jp", "物料规格"),
            // entity.purchasepriceitem.materialspecification
            new TranslationSeedItem("entity.purchasepriceitem.materialspecification", "zh-CN", "物料规格", "物料规格"),
            // entity.purchasepriceitem.materialspecification
            new TranslationSeedItem("entity.purchasepriceitem.materialspecification", "zh-HK", "物料规格_hk", "物料规格"),

            // entity.purchasepriceitem.purchaseunit
            new TranslationSeedItem("entity.purchasepriceitem.purchaseunit", "en-US", "采购单位_us", "采购单位"),
            // entity.purchasepriceitem.purchaseunit
            new TranslationSeedItem("entity.purchasepriceitem.purchaseunit", "ja-JP", "采购单位_jp", "采购单位"),
            // entity.purchasepriceitem.purchaseunit
            new TranslationSeedItem("entity.purchasepriceitem.purchaseunit", "zh-CN", "采购单位", "采购单位"),
            // entity.purchasepriceitem.purchaseunit
            new TranslationSeedItem("entity.purchasepriceitem.purchaseunit", "zh-HK", "采购单位_hk", "采购单位"),

            // entity.purchasepriceitem.purchaseperunit
            new TranslationSeedItem("entity.purchasepriceitem.purchaseperunit", "en-US", "价格单位_us", "价格单位（字典 logistics_price_unit_param：1/100/1000/10000；默认 1000）"),
            // entity.purchasepriceitem.purchaseperunit
            new TranslationSeedItem("entity.purchasepriceitem.purchaseperunit", "ja-JP", "价格单位_jp", "价格单位（字典 logistics_price_unit_param：1/100/1000/10000；默认 1000）"),
            // entity.purchasepriceitem.purchaseperunit
            new TranslationSeedItem("entity.purchasepriceitem.purchaseperunit", "zh-CN", "价格单位", "价格单位（字典 logistics_price_unit_param：1/100/1000/10000；默认 1000）"),
            // entity.purchasepriceitem.purchaseperunit
            new TranslationSeedItem("entity.purchasepriceitem.purchaseperunit", "zh-HK", "价格单位_hk", "价格单位（字典 logistics_price_unit_param：1/100/1000/10000；默认 1000）"),

            // entity.purchasepriceitem.purchaseprice
            new TranslationSeedItem("entity.purchasepriceitem.purchaseprice", "en-US", "采购价格_us", "采购价格（decimal(18,5)）"),
            // entity.purchasepriceitem.purchaseprice
            new TranslationSeedItem("entity.purchasepriceitem.purchaseprice", "ja-JP", "采购价格_jp", "采购价格（decimal(18,5)）"),
            // entity.purchasepriceitem.purchaseprice
            new TranslationSeedItem("entity.purchasepriceitem.purchaseprice", "zh-CN", "采购价格", "采购价格（decimal(18,5)）"),
            // entity.purchasepriceitem.purchaseprice
            new TranslationSeedItem("entity.purchasepriceitem.purchaseprice", "zh-HK", "采购价格_hk", "采购价格（decimal(18,5)）"),

            // entity.purchasepriceitem.minpurchasequantity
            new TranslationSeedItem("entity.purchasepriceitem.minpurchasequantity", "en-US", "最小采购量_us", "最小采购量（基本单位数量）"),
            // entity.purchasepriceitem.minpurchasequantity
            new TranslationSeedItem("entity.purchasepriceitem.minpurchasequantity", "ja-JP", "最小采购量_jp", "最小采购量（基本单位数量）"),
            // entity.purchasepriceitem.minpurchasequantity
            new TranslationSeedItem("entity.purchasepriceitem.minpurchasequantity", "zh-CN", "最小采购量", "最小采购量（基本单位数量）"),
            // entity.purchasepriceitem.minpurchasequantity
            new TranslationSeedItem("entity.purchasepriceitem.minpurchasequantity", "zh-HK", "最小采购量_hk", "最小采购量（基本单位数量）"),

            // entity.purchasepriceitem.maxpurchasequantity
            new TranslationSeedItem("entity.purchasepriceitem.maxpurchasequantity", "en-US", "最大采购量_us", "最大采购量（基本单位数量，0表示无限制）"),
            // entity.purchasepriceitem.maxpurchasequantity
            new TranslationSeedItem("entity.purchasepriceitem.maxpurchasequantity", "ja-JP", "最大采购量_jp", "最大采购量（基本单位数量，0表示无限制）"),
            // entity.purchasepriceitem.maxpurchasequantity
            new TranslationSeedItem("entity.purchasepriceitem.maxpurchasequantity", "zh-CN", "最大采购量", "最大采购量（基本单位数量，0表示无限制）"),
            // entity.purchasepriceitem.maxpurchasequantity
            new TranslationSeedItem("entity.purchasepriceitem.maxpurchasequantity", "zh-HK", "最大采购量_hk", "最大采购量（基本单位数量，0表示无限制）"),

            // entity.purchasepriceitem.sortorder
            new TranslationSeedItem("entity.purchasepriceitem.sortorder", "en-US", "排序号_us", "排序号（越小越靠前）"),
            // entity.purchasepriceitem.sortorder
            new TranslationSeedItem("entity.purchasepriceitem.sortorder", "ja-JP", "排序号_jp", "排序号（越小越靠前）"),
            // entity.purchasepriceitem.sortorder
            new TranslationSeedItem("entity.purchasepriceitem.sortorder", "zh-CN", "排序号", "排序号（越小越靠前）"),
            // entity.purchasepriceitem.sortorder
            new TranslationSeedItem("entity.purchasepriceitem.sortorder", "zh-HK", "排序号_hk", "排序号（越小越靠前）"),

            // entity.purchasepriceitem.isobsolete
            new TranslationSeedItem("entity.purchasepriceitem.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）"),
            // entity.purchasepriceitem.isobsolete
            new TranslationSeedItem("entity.purchasepriceitem.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）"),
            // entity.purchasepriceitem.isobsolete
            new TranslationSeedItem("entity.purchasepriceitem.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）"),
            // entity.purchasepriceitem.isobsolete
            new TranslationSeedItem("entity.purchasepriceitem.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）"),

            // entity.purchasepriceitem.scales
            new TranslationSeedItem("entity.purchasepriceitem.scales", "en-US", "价格阶梯列表_us", "价格阶梯列表（主子表关系，一个物料价格可以有多个阶梯）"),
            // entity.purchasepriceitem.scales
            new TranslationSeedItem("entity.purchasepriceitem.scales", "ja-JP", "价格阶梯列表_jp", "价格阶梯列表（主子表关系，一个物料价格可以有多个阶梯）"),
            // entity.purchasepriceitem.scales
            new TranslationSeedItem("entity.purchasepriceitem.scales", "zh-CN", "价格阶梯列表", "价格阶梯列表（主子表关系，一个物料价格可以有多个阶梯）"),
            // entity.purchasepriceitem.scales
            new TranslationSeedItem("entity.purchasepriceitem.scales", "zh-HK", "价格阶梯列表_hk", "价格阶梯列表（主子表关系，一个物料价格可以有多个阶梯）"),
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
        translation.ResourceGroup = "Procurement";
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

    /// <summary>
    /// 翻译种子项（对应 TaktTranslation 全部可写字段，CultureId 由 SeedAsync 解析）
    /// </summary>
    private sealed record TranslationSeedItem(
        string I18nKey,
        string CultureCode,
        string TranslationText,
        string? ContextNote);
}
