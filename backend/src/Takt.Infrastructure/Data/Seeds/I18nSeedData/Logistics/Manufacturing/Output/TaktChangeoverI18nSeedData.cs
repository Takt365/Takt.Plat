// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Output
// 文件名称：TaktChangeoverI18nSeedData.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktChangeover 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Output;

/// <summary>
/// TaktChangeover 实体国际化翻译种子（键前缀 entity.changeover.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktChangeoverI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktChangeover 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 changeover 实体翻译...", tenantCode);

        foreach (var item in GetChangeoverTranslations())
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

        TaktLogger.Information("TaktChangeover 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktChangeover 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.changeover._self / entity.changeover.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetChangeoverTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.changeover._self
            new TranslationSeedItem("entity.changeover._self", "en-US", "Changeover Information", "实体名称"),
            // entity.changeover._self
            new TranslationSeedItem("entity.changeover._self", "ja-JP", "切换记录信息", "实体名称"),
            // entity.changeover._self
            new TranslationSeedItem("entity.changeover._self", "zh-CN", "切换记录信息", "实体名称"),
            // entity.changeover._self
            new TranslationSeedItem("entity.changeover._self", "zh-HK", "切换记录信息", "实体名称"),

            // entity.changeover.plantcode
            new TranslationSeedItem("entity.changeover.plantcode", "en-US", "生产工厂", "生产工厂"),
            // entity.changeover.plantcode
            new TranslationSeedItem("entity.changeover.plantcode", "ja-JP", "生产工厂", "生产工厂"),
            // entity.changeover.plantcode
            new TranslationSeedItem("entity.changeover.plantcode", "zh-CN", "生产工厂", "生产工厂"),
            // entity.changeover.plantcode
            new TranslationSeedItem("entity.changeover.plantcode", "zh-HK", "生产工厂", "生产工厂"),

            // entity.changeover.productioncategory
            new TranslationSeedItem("entity.changeover.productioncategory", "en-US", "生产类别", "生产类别"),
            // entity.changeover.productioncategory
            new TranslationSeedItem("entity.changeover.productioncategory", "ja-JP", "生产类别", "生产类别"),
            // entity.changeover.productioncategory
            new TranslationSeedItem("entity.changeover.productioncategory", "zh-CN", "生产类别", "生产类别"),
            // entity.changeover.productioncategory
            new TranslationSeedItem("entity.changeover.productioncategory", "zh-HK", "生产类别", "生产类别"),

            // entity.changeover.productiondate
            new TranslationSeedItem("entity.changeover.productiondate", "en-US", "生产日期", "生产日期"),
            // entity.changeover.productiondate
            new TranslationSeedItem("entity.changeover.productiondate", "ja-JP", "生产日期", "生产日期"),
            // entity.changeover.productiondate
            new TranslationSeedItem("entity.changeover.productiondate", "zh-CN", "生产日期", "生产日期"),
            // entity.changeover.productiondate
            new TranslationSeedItem("entity.changeover.productiondate", "zh-HK", "生产日期", "生产日期"),

            // entity.changeover.productionline
            new TranslationSeedItem("entity.changeover.productionline", "en-US", "生产线", "生产线"),
            // entity.changeover.productionline
            new TranslationSeedItem("entity.changeover.productionline", "ja-JP", "生产线", "生产线"),
            // entity.changeover.productionline
            new TranslationSeedItem("entity.changeover.productionline", "zh-CN", "生产线", "生产线"),
            // entity.changeover.productionline
            new TranslationSeedItem("entity.changeover.productionline", "zh-HK", "生产线", "生产线"),

            // entity.changeover.readsoptime
            new TranslationSeedItem("entity.changeover.readsoptime", "en-US", "读取SOP时间", "读取SOP时间"),
            // entity.changeover.readsoptime
            new TranslationSeedItem("entity.changeover.readsoptime", "ja-JP", "读取SOP时间", "读取SOP时间"),
            // entity.changeover.readsoptime
            new TranslationSeedItem("entity.changeover.readsoptime", "zh-CN", "读取SOP时间", "读取SOP时间"),
            // entity.changeover.readsoptime
            new TranslationSeedItem("entity.changeover.readsoptime", "zh-HK", "读取SOP时间", "读取SOP时间"),

            // entity.changeover.personcount
            new TranslationSeedItem("entity.changeover.personcount", "en-US", "人数", "人数"),
            // entity.changeover.personcount
            new TranslationSeedItem("entity.changeover.personcount", "ja-JP", "人数", "人数"),
            // entity.changeover.personcount
            new TranslationSeedItem("entity.changeover.personcount", "zh-CN", "人数", "人数"),
            // entity.changeover.personcount
            new TranslationSeedItem("entity.changeover.personcount", "zh-HK", "人数", "人数"),

            // entity.changeover.totalsoptime
            new TranslationSeedItem("entity.changeover.totalsoptime", "en-US", "SOP总时间", "SOP总时间"),
            // entity.changeover.totalsoptime
            new TranslationSeedItem("entity.changeover.totalsoptime", "ja-JP", "SOP总时间", "SOP总时间"),
            // entity.changeover.totalsoptime
            new TranslationSeedItem("entity.changeover.totalsoptime", "zh-CN", "SOP总时间", "SOP总时间"),
            // entity.changeover.totalsoptime
            new TranslationSeedItem("entity.changeover.totalsoptime", "zh-HK", "SOP总时间", "SOP总时间"),

            // entity.changeover.count
            new TranslationSeedItem("entity.changeover.count", "en-US", "切换次数", "切换次数"),
            // entity.changeover.count
            new TranslationSeedItem("entity.changeover.count", "ja-JP", "切换次数", "切换次数"),
            // entity.changeover.count
            new TranslationSeedItem("entity.changeover.count", "zh-CN", "切换次数", "切换次数"),
            // entity.changeover.count
            new TranslationSeedItem("entity.changeover.count", "zh-HK", "切换次数", "切换次数"),

            // entity.changeover.time
            new TranslationSeedItem("entity.changeover.time", "en-US", "切换时间", "切换时间（单次）"),
            // entity.changeover.time
            new TranslationSeedItem("entity.changeover.time", "ja-JP", "切换时间", "切换时间（单次）"),
            // entity.changeover.time
            new TranslationSeedItem("entity.changeover.time", "zh-CN", "切换时间", "切换时间（单次）"),
            // entity.changeover.time
            new TranslationSeedItem("entity.changeover.time", "zh-HK", "切换时间", "切换时间（单次）"),

            // entity.changeover.totalchangeovertime
            new TranslationSeedItem("entity.changeover.totalchangeovertime", "en-US", "切换总时间", "切换总时间"),
            // entity.changeover.totalchangeovertime
            new TranslationSeedItem("entity.changeover.totalchangeovertime", "ja-JP", "切换总时间", "切换总时间"),
            // entity.changeover.totalchangeovertime
            new TranslationSeedItem("entity.changeover.totalchangeovertime", "zh-CN", "切换总时间", "切换总时间"),
            // entity.changeover.totalchangeovertime
            new TranslationSeedItem("entity.changeover.totalchangeovertime", "zh-HK", "切换总时间", "切换总时间"),
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
