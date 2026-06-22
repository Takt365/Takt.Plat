// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Procurement
// 文件名称：TaktPurchasePriceScaleI18nSeedData.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktPurchasePriceScale 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktPurchasePriceScale 实体国际化翻译种子（键前缀 entity.purchasepricescale.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktPurchasePriceScaleI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktPurchasePriceScale 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 purchasepricescale 实体翻译...", tenantCode);

        foreach (var item in GetPurchasePriceScaleTranslations())
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

        TaktLogger.Information("TaktPurchasePriceScale 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktPurchasePriceScale 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.purchasepricescale._self / entity.purchasepricescale.{{field}}；ResourceGroup=Procurement；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetPurchasePriceScaleTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.purchasepricescale._self
            new TranslationSeedItem("entity.purchasepricescale._self", "en-US", "Purchase Price Scale Information_us", "实体名称"),
            // entity.purchasepricescale._self
            new TranslationSeedItem("entity.purchasepricescale._self", "ja-JP", "Takt采购价格阶梯信息_jp", "实体名称"),
            // entity.purchasepricescale._self
            new TranslationSeedItem("entity.purchasepricescale._self", "zh-CN", "Takt采购价格阶梯信息", "实体名称"),
            // entity.purchasepricescale._self
            new TranslationSeedItem("entity.purchasepricescale._self", "zh-HK", "Takt采购价格阶梯信息_hk", "实体名称"),

            // entity.purchasepricescale.purchasepriceitemid
            new TranslationSeedItem("entity.purchasepricescale.purchasepriceitemid", "en-US", "采购价格明细ID_us", "采购价格明细ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.purchasepricescale.purchasepriceitemid
            new TranslationSeedItem("entity.purchasepricescale.purchasepriceitemid", "ja-JP", "采购价格明细ID_jp", "采购价格明细ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.purchasepricescale.purchasepriceitemid
            new TranslationSeedItem("entity.purchasepricescale.purchasepriceitemid", "zh-CN", "采购价格明细ID", "采购价格明细ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.purchasepricescale.purchasepriceitemid
            new TranslationSeedItem("entity.purchasepricescale.purchasepriceitemid", "zh-HK", "采购价格明细ID_hk", "采购价格明细ID（主子表关系，序列化为string以避免Javascript精度问题）"),

            // entity.purchasepricescale.purchasepricecode
            new TranslationSeedItem("entity.purchasepricescale.purchasepricecode", "en-US", "采购价格编码_us", "采购价格编码（冗余字段，便于查询）"),
            // entity.purchasepricescale.purchasepricecode
            new TranslationSeedItem("entity.purchasepricescale.purchasepricecode", "ja-JP", "采购价格编码_jp", "采购价格编码（冗余字段，便于查询）"),
            // entity.purchasepricescale.purchasepricecode
            new TranslationSeedItem("entity.purchasepricescale.purchasepricecode", "zh-CN", "采购价格编码", "采购价格编码（冗余字段，便于查询）"),
            // entity.purchasepricescale.purchasepricecode
            new TranslationSeedItem("entity.purchasepricescale.purchasepricecode", "zh-HK", "采购价格编码_hk", "采购价格编码（冗余字段，便于查询）"),

            // entity.purchasepricescale.linenumber
            new TranslationSeedItem("entity.purchasepricescale.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.purchasepricescale.linenumber
            new TranslationSeedItem("entity.purchasepricescale.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.purchasepricescale.linenumber
            new TranslationSeedItem("entity.purchasepricescale.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.purchasepricescale.linenumber
            new TranslationSeedItem("entity.purchasepricescale.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.purchasepricescale.startquantity
            new TranslationSeedItem("entity.purchasepricescale.startquantity", "en-US", "起始数量_us", "起始数量（基本单位数量，包含此数量）"),
            // entity.purchasepricescale.startquantity
            new TranslationSeedItem("entity.purchasepricescale.startquantity", "ja-JP", "起始数量_jp", "起始数量（基本单位数量，包含此数量）"),
            // entity.purchasepricescale.startquantity
            new TranslationSeedItem("entity.purchasepricescale.startquantity", "zh-CN", "起始数量", "起始数量（基本单位数量，包含此数量）"),
            // entity.purchasepricescale.startquantity
            new TranslationSeedItem("entity.purchasepricescale.startquantity", "zh-HK", "起始数量_hk", "起始数量（基本单位数量，包含此数量）"),

            // entity.purchasepricescale.endquantity
            new TranslationSeedItem("entity.purchasepricescale.endquantity", "en-US", "结束数量_us", "结束数量（基本单位数量，包含此数量，0表示无上限）"),
            // entity.purchasepricescale.endquantity
            new TranslationSeedItem("entity.purchasepricescale.endquantity", "ja-JP", "结束数量_jp", "结束数量（基本单位数量，包含此数量，0表示无上限）"),
            // entity.purchasepricescale.endquantity
            new TranslationSeedItem("entity.purchasepricescale.endquantity", "zh-CN", "结束数量", "结束数量（基本单位数量，包含此数量，0表示无上限）"),
            // entity.purchasepricescale.endquantity
            new TranslationSeedItem("entity.purchasepricescale.endquantity", "zh-HK", "结束数量_hk", "结束数量（基本单位数量，包含此数量，0表示无上限）"),

            // entity.purchasepricescale.scaleprice
            new TranslationSeedItem("entity.purchasepricescale.scaleprice", "en-US", "阶梯价格_us", "阶梯价格（精确到分，存储为整数，单位为分）"),
            // entity.purchasepricescale.scaleprice
            new TranslationSeedItem("entity.purchasepricescale.scaleprice", "ja-JP", "阶梯价格_jp", "阶梯价格（精确到分，存储为整数，单位为分）"),
            // entity.purchasepricescale.scaleprice
            new TranslationSeedItem("entity.purchasepricescale.scaleprice", "zh-CN", "阶梯价格", "阶梯价格（精确到分，存储为整数，单位为分）"),
            // entity.purchasepricescale.scaleprice
            new TranslationSeedItem("entity.purchasepricescale.scaleprice", "zh-HK", "阶梯价格_hk", "阶梯价格（精确到分，存储为整数，单位为分）"),

            // entity.purchasepricescale.sortorder
            new TranslationSeedItem("entity.purchasepricescale.sortorder", "en-US", "排序号_us", "排序号（越小越靠前）"),
            // entity.purchasepricescale.sortorder
            new TranslationSeedItem("entity.purchasepricescale.sortorder", "ja-JP", "排序号_jp", "排序号（越小越靠前）"),
            // entity.purchasepricescale.sortorder
            new TranslationSeedItem("entity.purchasepricescale.sortorder", "zh-CN", "排序号", "排序号（越小越靠前）"),
            // entity.purchasepricescale.sortorder
            new TranslationSeedItem("entity.purchasepricescale.sortorder", "zh-HK", "排序号_hk", "排序号（越小越靠前）"),
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
