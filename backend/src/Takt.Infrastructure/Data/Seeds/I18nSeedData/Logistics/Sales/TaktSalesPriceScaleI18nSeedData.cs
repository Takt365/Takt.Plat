// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Sales
// 文件名称：TaktSalesPriceScaleI18nSeedData.cs
// 创建时间：2026-06-12
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSalesPriceScale 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Sales;

/// <summary>
/// TaktSalesPriceScale 实体国际化翻译种子（键前缀 entity.salespricescale.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSalesPriceScaleI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSalesPriceScale 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 salespricescale 实体翻译...", tenantCode);

        foreach (var item in GetSalesPriceScaleTranslations())
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

        TaktLogger.Information("TaktSalesPriceScale 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSalesPriceScale 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.salespricescale._self / entity.salespricescale.{{field}}；ResourceGroup=4；ResourceType=0
    /// </summary>
    private static List<TranslationSeedItem> GetSalesPriceScaleTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.salespricescale._self
            new TranslationSeedItem("entity.salespricescale._self", "en-US", "Sales Price Scale Information", "实体名称"),
            // entity.salespricescale._self
            new TranslationSeedItem("entity.salespricescale._self", "ja-JP", "Takt销售价格阶梯信息", "实体名称"),
            // entity.salespricescale._self
            new TranslationSeedItem("entity.salespricescale._self", "zh-CN", "Takt销售价格阶梯信息", "实体名称"),
            // entity.salespricescale._self
            new TranslationSeedItem("entity.salespricescale._self", "zh-HK", "Takt销售价格阶梯信息", "实体名称"),

            // entity.salespricescale.itemid
            new TranslationSeedItem("entity.salespricescale.itemid", "en-US", "价格明细ID", "价格明细ID（关联销售价格明细表，序列化为string以避免Javascript精度问题）"),
            // entity.salespricescale.itemid
            new TranslationSeedItem("entity.salespricescale.itemid", "ja-JP", "价格明细ID", "价格明细ID（关联销售价格明细表，序列化为string以避免Javascript精度问题）"),
            // entity.salespricescale.itemid
            new TranslationSeedItem("entity.salespricescale.itemid", "zh-CN", "价格明细ID", "价格明细ID（关联销售价格明细表，序列化为string以避免Javascript精度问题）"),
            // entity.salespricescale.itemid
            new TranslationSeedItem("entity.salespricescale.itemid", "zh-HK", "价格明细ID", "价格明细ID（关联销售价格明细表，序列化为string以避免Javascript精度问题）"),

            // entity.salespricescale.salespricecode
            new TranslationSeedItem("entity.salespricescale.salespricecode", "en-US", "销售价格编码", "销售价格编码（冗余字段，便于查询）"),
            // entity.salespricescale.salespricecode
            new TranslationSeedItem("entity.salespricescale.salespricecode", "ja-JP", "销售价格编码", "销售价格编码（冗余字段，便于查询）"),
            // entity.salespricescale.salespricecode
            new TranslationSeedItem("entity.salespricescale.salespricecode", "zh-CN", "销售价格编码", "销售价格编码（冗余字段，便于查询）"),
            // entity.salespricescale.salespricecode
            new TranslationSeedItem("entity.salespricescale.salespricecode", "zh-HK", "销售价格编码", "销售价格编码（冗余字段，便于查询）"),

            // entity.salespricescale.linenumber
            new TranslationSeedItem("entity.salespricescale.linenumber", "en-US", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.salespricescale.linenumber
            new TranslationSeedItem("entity.salespricescale.linenumber", "ja-JP", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.salespricescale.linenumber
            new TranslationSeedItem("entity.salespricescale.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.salespricescale.linenumber
            new TranslationSeedItem("entity.salespricescale.linenumber", "zh-HK", "行号", "行号（项号/序号，固定步长=10）"),

            // entity.salespricescale.startquantity
            new TranslationSeedItem("entity.salespricescale.startquantity", "en-US", "起始数量", "起始数量（基本单位数量，包含此数量）"),
            // entity.salespricescale.startquantity
            new TranslationSeedItem("entity.salespricescale.startquantity", "ja-JP", "起始数量", "起始数量（基本单位数量，包含此数量）"),
            // entity.salespricescale.startquantity
            new TranslationSeedItem("entity.salespricescale.startquantity", "zh-CN", "起始数量", "起始数量（基本单位数量，包含此数量）"),
            // entity.salespricescale.startquantity
            new TranslationSeedItem("entity.salespricescale.startquantity", "zh-HK", "起始数量", "起始数量（基本单位数量，包含此数量）"),

            // entity.salespricescale.endquantity
            new TranslationSeedItem("entity.salespricescale.endquantity", "en-US", "结束数量", "结束数量（基本单位数量，包含此数量，0表示无上限）"),
            // entity.salespricescale.endquantity
            new TranslationSeedItem("entity.salespricescale.endquantity", "ja-JP", "结束数量", "结束数量（基本单位数量，包含此数量，0表示无上限）"),
            // entity.salespricescale.endquantity
            new TranslationSeedItem("entity.salespricescale.endquantity", "zh-CN", "结束数量", "结束数量（基本单位数量，包含此数量，0表示无上限）"),
            // entity.salespricescale.endquantity
            new TranslationSeedItem("entity.salespricescale.endquantity", "zh-HK", "结束数量", "结束数量（基本单位数量，包含此数量，0表示无上限）"),

            // entity.salespricescale.scaleprice
            new TranslationSeedItem("entity.salespricescale.scaleprice", "en-US", "阶梯价格", "阶梯价格（精确到分，存储为整数，单位为分）"),
            // entity.salespricescale.scaleprice
            new TranslationSeedItem("entity.salespricescale.scaleprice", "ja-JP", "阶梯价格", "阶梯价格（精确到分，存储为整数，单位为分）"),
            // entity.salespricescale.scaleprice
            new TranslationSeedItem("entity.salespricescale.scaleprice", "zh-CN", "阶梯价格", "阶梯价格（精确到分，存储为整数，单位为分）"),
            // entity.salespricescale.scaleprice
            new TranslationSeedItem("entity.salespricescale.scaleprice", "zh-HK", "阶梯价格", "阶梯价格（精确到分，存储为整数，单位为分）"),

            // entity.salespricescale.priceitem
            new TranslationSeedItem("entity.salespricescale.priceitem", "en-US", "销售价格明细", "销售价格明细（主表）"),
            // entity.salespricescale.priceitem
            new TranslationSeedItem("entity.salespricescale.priceitem", "ja-JP", "销售价格明细", "销售价格明细（主表）"),
            // entity.salespricescale.priceitem
            new TranslationSeedItem("entity.salespricescale.priceitem", "zh-CN", "销售价格明细", "销售价格明细（主表）"),
            // entity.salespricescale.priceitem
            new TranslationSeedItem("entity.salespricescale.priceitem", "zh-HK", "销售价格明细", "销售价格明细（主表）"),
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
        translation.ResourceGroup = 4;
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

    /// <summary>
    /// 翻译种子项（对应 TaktTranslation 全部可写字段，CultureId 由 SeedAsync 解析）
    /// </summary>
    private sealed record TranslationSeedItem(
        string I18nKey,
        string CultureCode,
        string TranslationText,
        string? ContextNote);
}
