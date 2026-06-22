// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Output
// 文件名称：TaktStandardOperationRateChangeLogI18nSeedData.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktStandardOperationRateChangeLog 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktStandardOperationRateChangeLog 实体国际化翻译种子（键前缀 entity.standardoperationratechangelog.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktStandardOperationRateChangeLogI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktStandardOperationRateChangeLog 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 standardoperationratechangelog 实体翻译...", tenantCode);

        foreach (var item in GetStandardOperationRateChangeLogTranslations())
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

        TaktLogger.Information("TaktStandardOperationRateChangeLog 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktStandardOperationRateChangeLog 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.standardoperationratechangelog._self / entity.standardoperationratechangelog.{{field}}；ResourceGroup=Output；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetStandardOperationRateChangeLogTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.standardoperationratechangelog._self
            new TranslationSeedItem("entity.standardoperationratechangelog._self", "en-US", "Standard Operation Rate Change Log Information_us", "实体名称"),
            // entity.standardoperationratechangelog._self
            new TranslationSeedItem("entity.standardoperationratechangelog._self", "ja-JP", "标准生产稼动率变更记录信息_jp", "实体名称"),
            // entity.standardoperationratechangelog._self
            new TranslationSeedItem("entity.standardoperationratechangelog._self", "zh-CN", "标准生产稼动率变更记录信息", "实体名称"),
            // entity.standardoperationratechangelog._self
            new TranslationSeedItem("entity.standardoperationratechangelog._self", "zh-HK", "标准生产稼动率变更记录信息_hk", "实体名称"),

            // entity.standardoperationratechangelog.standardoperationrateid
            new TranslationSeedItem("entity.standardoperationratechangelog.standardoperationrateid", "en-US", "标准生产稼动率ID_us", "标准生产稼动率ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.standardoperationratechangelog.standardoperationrateid
            new TranslationSeedItem("entity.standardoperationratechangelog.standardoperationrateid", "ja-JP", "标准生产稼动率ID_jp", "标准生产稼动率ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.standardoperationratechangelog.standardoperationrateid
            new TranslationSeedItem("entity.standardoperationratechangelog.standardoperationrateid", "zh-CN", "标准生产稼动率ID", "标准生产稼动率ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.standardoperationratechangelog.standardoperationrateid
            new TranslationSeedItem("entity.standardoperationratechangelog.standardoperationrateid", "zh-HK", "标准生产稼动率ID_hk", "标准生产稼动率ID（主子表关系，序列化为string以避免Javascript精度问题）"),

            // entity.standardoperationratechangelog.plantcode
            new TranslationSeedItem("entity.standardoperationratechangelog.plantcode", "en-US", "工厂代码_us", "工厂代码（冗余）"),
            // entity.standardoperationratechangelog.plantcode
            new TranslationSeedItem("entity.standardoperationratechangelog.plantcode", "ja-JP", "工厂代码_jp", "工厂代码（冗余）"),
            // entity.standardoperationratechangelog.plantcode
            new TranslationSeedItem("entity.standardoperationratechangelog.plantcode", "zh-CN", "工厂代码", "工厂代码（冗余）"),
            // entity.standardoperationratechangelog.plantcode
            new TranslationSeedItem("entity.standardoperationratechangelog.plantcode", "zh-HK", "工厂代码_hk", "工厂代码（冗余）"),

            // entity.standardoperationratechangelog.changefields
            new TranslationSeedItem("entity.standardoperationratechangelog.changefields", "en-US", "变更字段列表_us", "变更字段列表（JSON数组格式，记录同一时间点修改的所有字段及其旧值、新值）"),
            // entity.standardoperationratechangelog.changefields
            new TranslationSeedItem("entity.standardoperationratechangelog.changefields", "ja-JP", "变更字段列表_jp", "变更字段列表（JSON数组格式，记录同一时间点修改的所有字段及其旧值、新值）"),
            // entity.standardoperationratechangelog.changefields
            new TranslationSeedItem("entity.standardoperationratechangelog.changefields", "zh-CN", "变更字段列表", "变更字段列表（JSON数组格式，记录同一时间点修改的所有字段及其旧值、新值）"),
            // entity.standardoperationratechangelog.changefields
            new TranslationSeedItem("entity.standardoperationratechangelog.changefields", "zh-HK", "变更字段列表_hk", "变更字段列表（JSON数组格式，记录同一时间点修改的所有字段及其旧值、新值）"),

            // entity.standardoperationratechangelog.changetime
            new TranslationSeedItem("entity.standardoperationratechangelog.changetime", "en-US", "变更时间_us", "变更时间"),
            // entity.standardoperationratechangelog.changetime
            new TranslationSeedItem("entity.standardoperationratechangelog.changetime", "ja-JP", "变更时间_jp", "变更时间"),
            // entity.standardoperationratechangelog.changetime
            new TranslationSeedItem("entity.standardoperationratechangelog.changetime", "zh-CN", "变更时间", "变更时间"),
            // entity.standardoperationratechangelog.changetime
            new TranslationSeedItem("entity.standardoperationratechangelog.changetime", "zh-HK", "变更时间_hk", "变更时间"),

            // entity.standardoperationratechangelog.changeby
            new TranslationSeedItem("entity.standardoperationratechangelog.changeby", "en-US", "变更人_us", "变更人（人员代码）"),
            // entity.standardoperationratechangelog.changeby
            new TranslationSeedItem("entity.standardoperationratechangelog.changeby", "ja-JP", "变更人_jp", "变更人（人员代码）"),
            // entity.standardoperationratechangelog.changeby
            new TranslationSeedItem("entity.standardoperationratechangelog.changeby", "zh-CN", "变更人", "变更人（人员代码）"),
            // entity.standardoperationratechangelog.changeby
            new TranslationSeedItem("entity.standardoperationratechangelog.changeby", "zh-HK", "变更人_hk", "变更人（人员代码）"),

            // entity.standardoperationratechangelog.changereason
            new TranslationSeedItem("entity.standardoperationratechangelog.changereason", "en-US", "变更原因_us", "变更原因"),
            // entity.standardoperationratechangelog.changereason
            new TranslationSeedItem("entity.standardoperationratechangelog.changereason", "ja-JP", "变更原因_jp", "变更原因"),
            // entity.standardoperationratechangelog.changereason
            new TranslationSeedItem("entity.standardoperationratechangelog.changereason", "zh-CN", "变更原因", "变更原因"),
            // entity.standardoperationratechangelog.changereason
            new TranslationSeedItem("entity.standardoperationratechangelog.changereason", "zh-HK", "变更原因_hk", "变更原因"),

            // entity.standardoperationratechangelog.standardoperationrate
            new TranslationSeedItem("entity.standardoperationratechangelog.standardoperationrate", "en-US", "标准生产稼动率主表_us", "标准生产稼动率主表"),
            // entity.standardoperationratechangelog.standardoperationrate
            new TranslationSeedItem("entity.standardoperationratechangelog.standardoperationrate", "ja-JP", "标准生产稼动率主表_jp", "标准生产稼动率主表"),
            // entity.standardoperationratechangelog.standardoperationrate
            new TranslationSeedItem("entity.standardoperationratechangelog.standardoperationrate", "zh-CN", "标准生产稼动率主表", "标准生产稼动率主表"),
            // entity.standardoperationratechangelog.standardoperationrate
            new TranslationSeedItem("entity.standardoperationratechangelog.standardoperationrate", "zh-HK", "标准生产稼动率主表_hk", "标准生产稼动率主表"),
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
