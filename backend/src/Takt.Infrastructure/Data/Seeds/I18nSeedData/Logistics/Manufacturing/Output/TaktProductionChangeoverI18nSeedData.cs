// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Output
// 文件名称：TaktProductionChangeoverI18nSeedData.cs
// 创建时间：2026-07-02
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktProductionChangeover 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Output;

/// <summary>
/// TaktProductionChangeover 实体国际化翻译种子（键前缀 entity.productionchangeover.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktProductionChangeoverI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktProductionChangeover 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 productionchangeover 实体翻译...", tenantCode);

        foreach (var item in GetProductionChangeoverTranslations())
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

        TaktLogger.Information("TaktProductionChangeover 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktProductionChangeover 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.productionchangeover._self / entity.productionchangeover.{{field}}；ResourceGroup=Output；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetProductionChangeoverTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.productionchangeover._self
            new TranslationSeedItem("entity.productionchangeover._self", "en-US", "Production Changeover Information_us", "实体名称"),
            // entity.productionchangeover._self
            new TranslationSeedItem("entity.productionchangeover._self", "ja-JP", "生产切换记录信息_jp", "实体名称"),
            // entity.productionchangeover._self
            new TranslationSeedItem("entity.productionchangeover._self", "zh-CN", "生产切换记录信息", "实体名称"),
            // entity.productionchangeover._self
            new TranslationSeedItem("entity.productionchangeover._self", "zh-HK", "生产切换记录信息_hk", "实体名称"),

            // entity.productionchangeover.plantcode
            new TranslationSeedItem("entity.productionchangeover.plantcode", "en-US", "生产工厂_us", "生产工厂（关联 TaktPlant.PlantCode，选项 TaktPlants/options）"),
            // entity.productionchangeover.plantcode
            new TranslationSeedItem("entity.productionchangeover.plantcode", "ja-JP", "生产工厂_jp", "生产工厂（关联 TaktPlant.PlantCode，选项 TaktPlants/options）"),
            // entity.productionchangeover.plantcode
            new TranslationSeedItem("entity.productionchangeover.plantcode", "zh-CN", "生产工厂", "生产工厂（关联 TaktPlant.PlantCode，选项 TaktPlants/options）"),
            // entity.productionchangeover.plantcode
            new TranslationSeedItem("entity.productionchangeover.plantcode", "zh-HK", "生产工厂_hk", "生产工厂（关联 TaktPlant.PlantCode，选项 TaktPlants/options）"),

            // entity.productionchangeover.prodcategory
            new TranslationSeedItem("entity.productionchangeover.prodcategory", "en-US", "生产类别_us", "生产类别（字典 logistics_prod_category，存 DictValue）"),
            // entity.productionchangeover.prodcategory
            new TranslationSeedItem("entity.productionchangeover.prodcategory", "ja-JP", "生产类别_jp", "生产类别（字典 logistics_prod_category，存 DictValue）"),
            // entity.productionchangeover.prodcategory
            new TranslationSeedItem("entity.productionchangeover.prodcategory", "zh-CN", "生产类别", "生产类别（字典 logistics_prod_category，存 DictValue）"),
            // entity.productionchangeover.prodcategory
            new TranslationSeedItem("entity.productionchangeover.prodcategory", "zh-HK", "生产类别_hk", "生产类别（字典 logistics_prod_category，存 DictValue）"),

            // entity.productionchangeover.proddate
            new TranslationSeedItem("entity.productionchangeover.proddate", "en-US", "生产日期_us", "生产日期"),
            // entity.productionchangeover.proddate
            new TranslationSeedItem("entity.productionchangeover.proddate", "ja-JP", "生产日期_jp", "生产日期"),
            // entity.productionchangeover.proddate
            new TranslationSeedItem("entity.productionchangeover.proddate", "zh-CN", "生产日期", "生产日期"),
            // entity.productionchangeover.proddate
            new TranslationSeedItem("entity.productionchangeover.proddate", "zh-HK", "生产日期_hk", "生产日期"),

            // entity.productionchangeover.prodteam
            new TranslationSeedItem("entity.productionchangeover.prodteam", "en-US", "生产班组_us", "生产班组（选项 TaktProductionTeams/options，存 TeamCode，ExtValue=PlantCode 过滤）"),
            // entity.productionchangeover.prodteam
            new TranslationSeedItem("entity.productionchangeover.prodteam", "ja-JP", "生产班组_jp", "生产班组（选项 TaktProductionTeams/options，存 TeamCode，ExtValue=PlantCode 过滤）"),
            // entity.productionchangeover.prodteam
            new TranslationSeedItem("entity.productionchangeover.prodteam", "zh-CN", "生产班组", "生产班组（选项 TaktProductionTeams/options，存 TeamCode，ExtValue=PlantCode 过滤）"),
            // entity.productionchangeover.prodteam
            new TranslationSeedItem("entity.productionchangeover.prodteam", "zh-HK", "生产班组_hk", "生产班组（选项 TaktProductionTeams/options，存 TeamCode，ExtValue=PlantCode 过滤）"),

            // entity.productionchangeover.readsoptime
            new TranslationSeedItem("entity.productionchangeover.readsoptime", "en-US", "读取SOP时间_us", "读取SOP时间"),
            // entity.productionchangeover.readsoptime
            new TranslationSeedItem("entity.productionchangeover.readsoptime", "ja-JP", "读取SOP时间_jp", "读取SOP时间"),
            // entity.productionchangeover.readsoptime
            new TranslationSeedItem("entity.productionchangeover.readsoptime", "zh-CN", "读取SOP时间", "读取SOP时间"),
            // entity.productionchangeover.readsoptime
            new TranslationSeedItem("entity.productionchangeover.readsoptime", "zh-HK", "读取SOP时间_hk", "读取SOP时间"),

            // entity.productionchangeover.personcount
            new TranslationSeedItem("entity.productionchangeover.personcount", "en-US", "人数_us", "人数"),
            // entity.productionchangeover.personcount
            new TranslationSeedItem("entity.productionchangeover.personcount", "ja-JP", "人数_jp", "人数"),
            // entity.productionchangeover.personcount
            new TranslationSeedItem("entity.productionchangeover.personcount", "zh-CN", "人数", "人数"),
            // entity.productionchangeover.personcount
            new TranslationSeedItem("entity.productionchangeover.personcount", "zh-HK", "人数_hk", "人数"),

            // entity.productionchangeover.totalsoptime
            new TranslationSeedItem("entity.productionchangeover.totalsoptime", "en-US", "SOP总时间_us", "SOP总时间"),
            // entity.productionchangeover.totalsoptime
            new TranslationSeedItem("entity.productionchangeover.totalsoptime", "ja-JP", "SOP总时间_jp", "SOP总时间"),
            // entity.productionchangeover.totalsoptime
            new TranslationSeedItem("entity.productionchangeover.totalsoptime", "zh-CN", "SOP总时间", "SOP总时间"),
            // entity.productionchangeover.totalsoptime
            new TranslationSeedItem("entity.productionchangeover.totalsoptime", "zh-HK", "SOP总时间_hk", "SOP总时间"),

            // entity.productionchangeover.changeovercount
            new TranslationSeedItem("entity.productionchangeover.changeovercount", "en-US", "切换次数_us", "切换次数"),
            // entity.productionchangeover.changeovercount
            new TranslationSeedItem("entity.productionchangeover.changeovercount", "ja-JP", "切换次数_jp", "切换次数"),
            // entity.productionchangeover.changeovercount
            new TranslationSeedItem("entity.productionchangeover.changeovercount", "zh-CN", "切换次数", "切换次数"),
            // entity.productionchangeover.changeovercount
            new TranslationSeedItem("entity.productionchangeover.changeovercount", "zh-HK", "切换次数_hk", "切换次数"),

            // entity.productionchangeover.changeovertime
            new TranslationSeedItem("entity.productionchangeover.changeovertime", "en-US", "切换时间_us", "切换时间（单次）"),
            // entity.productionchangeover.changeovertime
            new TranslationSeedItem("entity.productionchangeover.changeovertime", "ja-JP", "切换时间_jp", "切换时间（单次）"),
            // entity.productionchangeover.changeovertime
            new TranslationSeedItem("entity.productionchangeover.changeovertime", "zh-CN", "切换时间", "切换时间（单次）"),
            // entity.productionchangeover.changeovertime
            new TranslationSeedItem("entity.productionchangeover.changeovertime", "zh-HK", "切换时间_hk", "切换时间（单次）"),

            // entity.productionchangeover.totalchangeovertime
            new TranslationSeedItem("entity.productionchangeover.totalchangeovertime", "en-US", "切换总时间_us", "切换总时间"),
            // entity.productionchangeover.totalchangeovertime
            new TranslationSeedItem("entity.productionchangeover.totalchangeovertime", "ja-JP", "切换总时间_jp", "切换总时间"),
            // entity.productionchangeover.totalchangeovertime
            new TranslationSeedItem("entity.productionchangeover.totalchangeovertime", "zh-CN", "切换总时间", "切换总时间"),
            // entity.productionchangeover.totalchangeovertime
            new TranslationSeedItem("entity.productionchangeover.totalchangeovertime", "zh-HK", "切换总时间_hk", "切换总时间"),
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
        translation.ResourceGroup = "Output";
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
