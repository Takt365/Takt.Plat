// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Cost
// 文件名称：TaktQualityScrapI18nSeedData.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktQualityScrap 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Cost;

/// <summary>
/// TaktQualityScrap 实体国际化翻译种子（键前缀 entity.qualityScrap.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktQualityScrapI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktQualityScrap 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 qualityScrap 实体翻译...", tenantCode);

        foreach (var item in GetQualityScrapTranslations())
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

        TaktLogger.Information("TaktQualityScrap 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktQualityScrap 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.qualityScrap._self / entity.qualityScrap.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetQualityScrapTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.qualityScrap._self
            new TranslationSeedItem("entity.qualityScrap._self", "en-US", "Quality Scrap Information", "实体名称"),
            // entity.qualityScrap._self
            new TranslationSeedItem("entity.qualityScrap._self", "ja-JP", "品质废弃主表信息", "实体名称"),
            // entity.qualityScrap._self
            new TranslationSeedItem("entity.qualityScrap._self", "zh-CN", "品质废弃主表信息", "实体名称"),
            // entity.qualityScrap._self
            new TranslationSeedItem("entity.qualityScrap._self", "zh-HK", "品质废弃主表信息", "实体名称"),

            // entity.qualityScrap.plantcode
            new TranslationSeedItem("entity.qualityScrap.plantcode", "en-US", "工厂代码", "工厂代码"),
            // entity.qualityScrap.plantcode
            new TranslationSeedItem("entity.qualityScrap.plantcode", "ja-JP", "工厂代码", "工厂代码"),
            // entity.qualityScrap.plantcode
            new TranslationSeedItem("entity.qualityScrap.plantcode", "zh-CN", "工厂代码", "工厂代码"),
            // entity.qualityScrap.plantcode
            new TranslationSeedItem("entity.qualityScrap.plantcode", "zh-HK", "工厂代码", "工厂代码"),

            // entity.qualityScrap.code
            new TranslationSeedItem("entity.qualityScrap.code", "en-US", "品质废弃编码", "品质废弃编码(唯一,如:QS-2026-0001)"),
            // entity.qualityScrap.code
            new TranslationSeedItem("entity.qualityScrap.code", "ja-JP", "品质废弃编码", "品质废弃编码(唯一,如:QS-2026-0001)"),
            // entity.qualityScrap.code
            new TranslationSeedItem("entity.qualityScrap.code", "zh-CN", "品质废弃编码", "品质废弃编码(唯一,如:QS-2026-0001)"),
            // entity.qualityScrap.code
            new TranslationSeedItem("entity.qualityScrap.code", "zh-HK", "品质废弃编码", "品质废弃编码(唯一,如:QS-2026-0001)"),

            // entity.qualityScrap.scrapdate
            new TranslationSeedItem("entity.qualityScrap.scrapdate", "en-US", "废弃日期", "废弃日期"),
            // entity.qualityScrap.scrapdate
            new TranslationSeedItem("entity.qualityScrap.scrapdate", "ja-JP", "废弃日期", "废弃日期"),
            // entity.qualityScrap.scrapdate
            new TranslationSeedItem("entity.qualityScrap.scrapdate", "zh-CN", "废弃日期", "废弃日期"),
            // entity.qualityScrap.scrapdate
            new TranslationSeedItem("entity.qualityScrap.scrapdate", "zh-HK", "废弃日期", "废弃日期"),

            // entity.qualityScrap.indirectmanpowercostperminute
            new TranslationSeedItem("entity.qualityScrap.indirectmanpowercostperminute", "en-US", "间接人员费率", "间接人员费率(元/分钟)"),
            // entity.qualityScrap.indirectmanpowercostperminute
            new TranslationSeedItem("entity.qualityScrap.indirectmanpowercostperminute", "ja-JP", "间接人员费率", "间接人员费率(元/分钟)"),
            // entity.qualityScrap.indirectmanpowercostperminute
            new TranslationSeedItem("entity.qualityScrap.indirectmanpowercostperminute", "zh-CN", "间接人员费率", "间接人员费率(元/分钟)"),
            // entity.qualityScrap.indirectmanpowercostperminute
            new TranslationSeedItem("entity.qualityScrap.indirectmanpowercostperminute", "zh-HK", "间接人员费率", "间接人员费率(元/分钟)"),

            // entity.qualityScrap.model
            new TranslationSeedItem("entity.qualityScrap.model", "en-US", "机种", "机种/产品型号"),
            // entity.qualityScrap.model
            new TranslationSeedItem("entity.qualityScrap.model", "ja-JP", "机种", "机种/产品型号"),
            // entity.qualityScrap.model
            new TranslationSeedItem("entity.qualityScrap.model", "zh-CN", "机种", "机种/产品型号"),
            // entity.qualityScrap.model
            new TranslationSeedItem("entity.qualityScrap.model", "zh-HK", "机种", "机种/产品型号"),

            // entity.qualityScrap.scrapreason
            new TranslationSeedItem("entity.qualityScrap.scrapreason", "en-US", "事故内容", "事故内容(废弃原因)"),
            // entity.qualityScrap.scrapreason
            new TranslationSeedItem("entity.qualityScrap.scrapreason", "ja-JP", "事故内容", "事故内容(废弃原因)"),
            // entity.qualityScrap.scrapreason
            new TranslationSeedItem("entity.qualityScrap.scrapreason", "zh-CN", "事故内容", "事故内容(废弃原因)"),
            // entity.qualityScrap.scrapreason
            new TranslationSeedItem("entity.qualityScrap.scrapreason", "zh-HK", "事故内容", "事故内容(废弃原因)"),

            // entity.qualityScrap.totalscrapquantity
            new TranslationSeedItem("entity.qualityScrap.totalscrapquantity", "en-US", "废弃总数", "废弃总数(自动计算 = 各子表废弃数量合计)"),
            // entity.qualityScrap.totalscrapquantity
            new TranslationSeedItem("entity.qualityScrap.totalscrapquantity", "ja-JP", "废弃总数", "废弃总数(自动计算 = 各子表废弃数量合计)"),
            // entity.qualityScrap.totalscrapquantity
            new TranslationSeedItem("entity.qualityScrap.totalscrapquantity", "zh-CN", "废弃总数", "废弃总数(自动计算 = 各子表废弃数量合计)"),
            // entity.qualityScrap.totalscrapquantity
            new TranslationSeedItem("entity.qualityScrap.totalscrapquantity", "zh-HK", "废弃总数", "废弃总数(自动计算 = 各子表废弃数量合计)"),

            // entity.qualityScrap.totalscrapcost
            new TranslationSeedItem("entity.qualityScrap.totalscrapcost", "en-US", "总废弃费用", "总废弃费用(元,自动计算 = 各子表费用合计)"),
            // entity.qualityScrap.totalscrapcost
            new TranslationSeedItem("entity.qualityScrap.totalscrapcost", "ja-JP", "总废弃费用", "总废弃费用(元,自动计算 = 各子表费用合计)"),
            // entity.qualityScrap.totalscrapcost
            new TranslationSeedItem("entity.qualityScrap.totalscrapcost", "zh-CN", "总废弃费用", "总废弃费用(元,自动计算 = 各子表费用合计)"),
            // entity.qualityScrap.totalscrapcost
            new TranslationSeedItem("entity.qualityScrap.totalscrapcost", "zh-HK", "总废弃费用", "总废弃费用(元,自动计算 = 各子表费用合计)"),

            // entity.qualityScrap.costcurrency
            new TranslationSeedItem("entity.qualityScrap.costcurrency", "en-US", "成本币种", "成本币种(CNY/USD/JPY等)"),
            // entity.qualityScrap.costcurrency
            new TranslationSeedItem("entity.qualityScrap.costcurrency", "ja-JP", "成本币种", "成本币种(CNY/USD/JPY等)"),
            // entity.qualityScrap.costcurrency
            new TranslationSeedItem("entity.qualityScrap.costcurrency", "zh-CN", "成本币种", "成本币种(CNY/USD/JPY等)"),
            // entity.qualityScrap.costcurrency
            new TranslationSeedItem("entity.qualityScrap.costcurrency", "zh-HK", "成本币种", "成本币种(CNY/USD/JPY等)"),

            // entity.qualityScrap.scrapitems
            new TranslationSeedItem("entity.qualityScrap.scrapitems", "en-US", "scrapItems", "废弃明细列表"),
            // entity.qualityScrap.scrapitems
            new TranslationSeedItem("entity.qualityScrap.scrapitems", "ja-JP", "scrapItems", "废弃明细列表"),
            // entity.qualityScrap.scrapitems
            new TranslationSeedItem("entity.qualityScrap.scrapitems", "zh-CN", "scrapItems", "废弃明细列表"),
            // entity.qualityScrap.scrapitems
            new TranslationSeedItem("entity.qualityScrap.scrapitems", "zh-HK", "scrapItems", "废弃明细列表"),
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
