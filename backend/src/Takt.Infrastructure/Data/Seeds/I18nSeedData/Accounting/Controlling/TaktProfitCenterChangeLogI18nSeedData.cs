// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Accounting.Controlling
// 文件名称：TaktProfitCenterChangeLogI18nSeedData.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktProfitCenterChangeLog 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Accounting.Controlling;

/// <summary>
/// TaktProfitCenterChangeLog 实体国际化翻译种子（键前缀 entity.profitcenterchangelog.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktProfitCenterChangeLogI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktProfitCenterChangeLog 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 profitcenterchangelog 实体翻译...", tenantCode);

        foreach (var item in GetProfitCenterChangeLogTranslations())
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

        TaktLogger.Information("TaktProfitCenterChangeLog 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktProfitCenterChangeLog 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.profitcenterchangelog._self / entity.profitcenterchangelog.{{field}}；ResourceGroup=Controlling；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetProfitCenterChangeLogTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.profitcenterchangelog._self
            new TranslationSeedItem("entity.profitcenterchangelog._self", "en-US", "Profit Center Change Log Information_us", "实体名称"),
            // entity.profitcenterchangelog._self
            new TranslationSeedItem("entity.profitcenterchangelog._self", "ja-JP", "利润中心变更记录信息_jp", "实体名称"),
            // entity.profitcenterchangelog._self
            new TranslationSeedItem("entity.profitcenterchangelog._self", "zh-CN", "利润中心变更记录信息", "实体名称"),
            // entity.profitcenterchangelog._self
            new TranslationSeedItem("entity.profitcenterchangelog._self", "zh-HK", "利润中心变更记录信息_hk", "实体名称"),

            // entity.profitcenterchangelog.profitcenterid
            new TranslationSeedItem("entity.profitcenterchangelog.profitcenterid", "en-US", "利润中心ID_us", "利润中心 ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）"),
            // entity.profitcenterchangelog.profitcenterid
            new TranslationSeedItem("entity.profitcenterchangelog.profitcenterid", "ja-JP", "利润中心ID_jp", "利润中心 ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）"),
            // entity.profitcenterchangelog.profitcenterid
            new TranslationSeedItem("entity.profitcenterchangelog.profitcenterid", "zh-CN", "利润中心ID", "利润中心 ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）"),
            // entity.profitcenterchangelog.profitcenterid
            new TranslationSeedItem("entity.profitcenterchangelog.profitcenterid", "zh-HK", "利润中心ID_hk", "利润中心 ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）"),

            // entity.profitcenterchangelog.profitcentercode
            new TranslationSeedItem("entity.profitcenterchangelog.profitcentercode", "en-US", "利润中心编码_us", "利润中心编码（冗余）"),
            // entity.profitcenterchangelog.profitcentercode
            new TranslationSeedItem("entity.profitcenterchangelog.profitcentercode", "ja-JP", "利润中心编码_jp", "利润中心编码（冗余）"),
            // entity.profitcenterchangelog.profitcentercode
            new TranslationSeedItem("entity.profitcenterchangelog.profitcentercode", "zh-CN", "利润中心编码", "利润中心编码（冗余）"),
            // entity.profitcenterchangelog.profitcentercode
            new TranslationSeedItem("entity.profitcenterchangelog.profitcentercode", "zh-HK", "利润中心编码_hk", "利润中心编码（冗余）"),

            // entity.profitcenterchangelog.changefields
            new TranslationSeedItem("entity.profitcenterchangelog.changefields", "en-US", "变更字段列表_us", "变更字段列表 JSON"),
            // entity.profitcenterchangelog.changefields
            new TranslationSeedItem("entity.profitcenterchangelog.changefields", "ja-JP", "变更字段列表_jp", "变更字段列表 JSON"),
            // entity.profitcenterchangelog.changefields
            new TranslationSeedItem("entity.profitcenterchangelog.changefields", "zh-CN", "变更字段列表", "变更字段列表 JSON"),
            // entity.profitcenterchangelog.changefields
            new TranslationSeedItem("entity.profitcenterchangelog.changefields", "zh-HK", "变更字段列表_hk", "变更字段列表 JSON"),

            // entity.profitcenterchangelog.changetime
            new TranslationSeedItem("entity.profitcenterchangelog.changetime", "en-US", "变更时间_us", "变更时间"),
            // entity.profitcenterchangelog.changetime
            new TranslationSeedItem("entity.profitcenterchangelog.changetime", "ja-JP", "变更时间_jp", "变更时间"),
            // entity.profitcenterchangelog.changetime
            new TranslationSeedItem("entity.profitcenterchangelog.changetime", "zh-CN", "变更时间", "变更时间"),
            // entity.profitcenterchangelog.changetime
            new TranslationSeedItem("entity.profitcenterchangelog.changetime", "zh-HK", "变更时间_hk", "变更时间"),

            // entity.profitcenterchangelog.changeby
            new TranslationSeedItem("entity.profitcenterchangelog.changeby", "en-US", "变更人_us", "变更人"),
            // entity.profitcenterchangelog.changeby
            new TranslationSeedItem("entity.profitcenterchangelog.changeby", "ja-JP", "变更人_jp", "变更人"),
            // entity.profitcenterchangelog.changeby
            new TranslationSeedItem("entity.profitcenterchangelog.changeby", "zh-CN", "变更人", "变更人"),
            // entity.profitcenterchangelog.changeby
            new TranslationSeedItem("entity.profitcenterchangelog.changeby", "zh-HK", "变更人_hk", "变更人"),

            // entity.profitcenterchangelog.changereason
            new TranslationSeedItem("entity.profitcenterchangelog.changereason", "en-US", "变更原因_us", "变更原因"),
            // entity.profitcenterchangelog.changereason
            new TranslationSeedItem("entity.profitcenterchangelog.changereason", "ja-JP", "变更原因_jp", "变更原因"),
            // entity.profitcenterchangelog.changereason
            new TranslationSeedItem("entity.profitcenterchangelog.changereason", "zh-CN", "变更原因", "变更原因"),
            // entity.profitcenterchangelog.changereason
            new TranslationSeedItem("entity.profitcenterchangelog.changereason", "zh-HK", "变更原因_hk", "变更原因"),

            // entity.profitcenterchangelog.profitcenter
            new TranslationSeedItem("entity.profitcenterchangelog.profitcenter", "en-US", "利润中心主表_us", "利润中心主表"),
            // entity.profitcenterchangelog.profitcenter
            new TranslationSeedItem("entity.profitcenterchangelog.profitcenter", "ja-JP", "利润中心主表_jp", "利润中心主表"),
            // entity.profitcenterchangelog.profitcenter
            new TranslationSeedItem("entity.profitcenterchangelog.profitcenter", "zh-CN", "利润中心主表", "利润中心主表"),
            // entity.profitcenterchangelog.profitcenter
            new TranslationSeedItem("entity.profitcenterchangelog.profitcenter", "zh-HK", "利润中心主表_hk", "利润中心主表"),
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
        translation.ResourceGroup = "Controlling";
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
