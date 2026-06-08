// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Materials
// 文件名称：TaktPurchasePriceScaleI18nSeedData.cs
// 创建时间：2026-06-08
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
using Takt.Shared.Enums;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Materials;

/// <summary>
/// TaktPurchasePriceScale 实体国际化翻译种子（键前缀 entity.purchasePriceScale.*）
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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 purchasePriceScale 实体翻译...", tenantCode);

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
    /// I18nKey：entity.purchasePriceScale._self / entity.purchasePriceScale.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetPurchasePriceScaleTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.purchasePriceScale._self
            new TranslationSeedItem("entity.purchasePriceScale._self", "en-US", "Purchase Price Scale Information", "实体名称"),
            // entity.purchasePriceScale._self
            new TranslationSeedItem("entity.purchasePriceScale._self", "ja-JP", "Takt采购价格阶梯信息", "实体名称"),
            // entity.purchasePriceScale._self
            new TranslationSeedItem("entity.purchasePriceScale._self", "zh-CN", "Takt采购价格阶梯信息", "实体名称"),
            // entity.purchasePriceScale._self
            new TranslationSeedItem("entity.purchasePriceScale._self", "zh-HK", "Takt采购价格阶梯信息", "实体名称"),

            // entity.purchasePriceScale.purchasepriceitemid
            new TranslationSeedItem("entity.purchasePriceScale.purchasepriceitemid", "en-US", "采购价格明细ID", "采购价格明细ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.purchasePriceScale.purchasepriceitemid
            new TranslationSeedItem("entity.purchasePriceScale.purchasepriceitemid", "ja-JP", "采购价格明细ID", "采购价格明细ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.purchasePriceScale.purchasepriceitemid
            new TranslationSeedItem("entity.purchasePriceScale.purchasepriceitemid", "zh-CN", "采购价格明细ID", "采购价格明细ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.purchasePriceScale.purchasepriceitemid
            new TranslationSeedItem("entity.purchasePriceScale.purchasepriceitemid", "zh-HK", "采购价格明细ID", "采购价格明细ID（主子表关系，序列化为string以避免Javascript精度问题）"),

            // entity.purchasePriceScale.purchasepricecode
            new TranslationSeedItem("entity.purchasePriceScale.purchasepricecode", "en-US", "采购价格编码", "采购价格编码（冗余字段，便于查询）"),
            // entity.purchasePriceScale.purchasepricecode
            new TranslationSeedItem("entity.purchasePriceScale.purchasepricecode", "ja-JP", "采购价格编码", "采购价格编码（冗余字段，便于查询）"),
            // entity.purchasePriceScale.purchasepricecode
            new TranslationSeedItem("entity.purchasePriceScale.purchasepricecode", "zh-CN", "采购价格编码", "采购价格编码（冗余字段，便于查询）"),
            // entity.purchasePriceScale.purchasepricecode
            new TranslationSeedItem("entity.purchasePriceScale.purchasepricecode", "zh-HK", "采购价格编码", "采购价格编码（冗余字段，便于查询）"),

            // entity.purchasePriceScale.linenumber
            new TranslationSeedItem("entity.purchasePriceScale.linenumber", "en-US", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.purchasePriceScale.linenumber
            new TranslationSeedItem("entity.purchasePriceScale.linenumber", "ja-JP", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.purchasePriceScale.linenumber
            new TranslationSeedItem("entity.purchasePriceScale.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.purchasePriceScale.linenumber
            new TranslationSeedItem("entity.purchasePriceScale.linenumber", "zh-HK", "行号", "行号（项号/序号，固定步长=10）"),

            // entity.purchasePriceScale.startquantity
            new TranslationSeedItem("entity.purchasePriceScale.startquantity", "en-US", "起始数量", "起始数量（基本单位数量，包含此数量）"),
            // entity.purchasePriceScale.startquantity
            new TranslationSeedItem("entity.purchasePriceScale.startquantity", "ja-JP", "起始数量", "起始数量（基本单位数量，包含此数量）"),
            // entity.purchasePriceScale.startquantity
            new TranslationSeedItem("entity.purchasePriceScale.startquantity", "zh-CN", "起始数量", "起始数量（基本单位数量，包含此数量）"),
            // entity.purchasePriceScale.startquantity
            new TranslationSeedItem("entity.purchasePriceScale.startquantity", "zh-HK", "起始数量", "起始数量（基本单位数量，包含此数量）"),

            // entity.purchasePriceScale.endquantity
            new TranslationSeedItem("entity.purchasePriceScale.endquantity", "en-US", "结束数量", "结束数量（基本单位数量，包含此数量，0表示无上限）"),
            // entity.purchasePriceScale.endquantity
            new TranslationSeedItem("entity.purchasePriceScale.endquantity", "ja-JP", "结束数量", "结束数量（基本单位数量，包含此数量，0表示无上限）"),
            // entity.purchasePriceScale.endquantity
            new TranslationSeedItem("entity.purchasePriceScale.endquantity", "zh-CN", "结束数量", "结束数量（基本单位数量，包含此数量，0表示无上限）"),
            // entity.purchasePriceScale.endquantity
            new TranslationSeedItem("entity.purchasePriceScale.endquantity", "zh-HK", "结束数量", "结束数量（基本单位数量，包含此数量，0表示无上限）"),

            // entity.purchasePriceScale.scaleprice
            new TranslationSeedItem("entity.purchasePriceScale.scaleprice", "en-US", "阶梯价格", "阶梯价格（精确到分，存储为整数，单位为分）"),
            // entity.purchasePriceScale.scaleprice
            new TranslationSeedItem("entity.purchasePriceScale.scaleprice", "ja-JP", "阶梯价格", "阶梯价格（精确到分，存储为整数，单位为分）"),
            // entity.purchasePriceScale.scaleprice
            new TranslationSeedItem("entity.purchasePriceScale.scaleprice", "zh-CN", "阶梯价格", "阶梯价格（精确到分，存储为整数，单位为分）"),
            // entity.purchasePriceScale.scaleprice
            new TranslationSeedItem("entity.purchasePriceScale.scaleprice", "zh-HK", "阶梯价格", "阶梯价格（精确到分，存储为整数，单位为分）"),

            // entity.purchasePriceScale.sortorder
            new TranslationSeedItem("entity.purchasePriceScale.sortorder", "en-US", "排序号", "排序号（越小越靠前）"),
            // entity.purchasePriceScale.sortorder
            new TranslationSeedItem("entity.purchasePriceScale.sortorder", "ja-JP", "排序号", "排序号（越小越靠前）"),
            // entity.purchasePriceScale.sortorder
            new TranslationSeedItem("entity.purchasePriceScale.sortorder", "zh-CN", "排序号", "排序号（越小越靠前）"),
            // entity.purchasePriceScale.sortorder
            new TranslationSeedItem("entity.purchasePriceScale.sortorder", "zh-HK", "排序号", "排序号（越小越靠前）"),
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
