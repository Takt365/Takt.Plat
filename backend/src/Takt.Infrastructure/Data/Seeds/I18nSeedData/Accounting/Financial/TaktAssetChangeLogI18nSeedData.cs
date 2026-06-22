// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Accounting.Financial
// 文件名称：TaktAssetChangeLogI18nSeedData.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktAssetChangeLog 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Accounting.Financial;

/// <summary>
/// TaktAssetChangeLog 实体国际化翻译种子（键前缀 entity.assetchangelog.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktAssetChangeLogI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktAssetChangeLog 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 assetchangelog 实体翻译...", tenantCode);

        foreach (var item in GetAssetChangeLogTranslations())
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

        TaktLogger.Information("TaktAssetChangeLog 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktAssetChangeLog 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.assetchangelog._self / entity.assetchangelog.{{field}}；ResourceGroup=Financial；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetAssetChangeLogTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.assetchangelog._self
            new TranslationSeedItem("entity.assetchangelog._self", "en-US", "Asset Change Log Information_us", "实体名称"),
            // entity.assetchangelog._self
            new TranslationSeedItem("entity.assetchangelog._self", "ja-JP", "资产变更记录信息_jp", "实体名称"),
            // entity.assetchangelog._self
            new TranslationSeedItem("entity.assetchangelog._self", "zh-CN", "资产变更记录信息", "实体名称"),
            // entity.assetchangelog._self
            new TranslationSeedItem("entity.assetchangelog._self", "zh-HK", "资产变更记录信息_hk", "实体名称"),

            // entity.assetchangelog.assetid
            new TranslationSeedItem("entity.assetchangelog.assetid", "en-US", "资产ID_us", "资产 ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）"),
            // entity.assetchangelog.assetid
            new TranslationSeedItem("entity.assetchangelog.assetid", "ja-JP", "资产ID_jp", "资产 ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）"),
            // entity.assetchangelog.assetid
            new TranslationSeedItem("entity.assetchangelog.assetid", "zh-CN", "资产ID", "资产 ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）"),
            // entity.assetchangelog.assetid
            new TranslationSeedItem("entity.assetchangelog.assetid", "zh-HK", "资产ID_hk", "资产 ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）"),

            // entity.assetchangelog.assetcode
            new TranslationSeedItem("entity.assetchangelog.assetcode", "en-US", "资产编码_us", "资产编码（冗余）"),
            // entity.assetchangelog.assetcode
            new TranslationSeedItem("entity.assetchangelog.assetcode", "ja-JP", "资产编码_jp", "资产编码（冗余）"),
            // entity.assetchangelog.assetcode
            new TranslationSeedItem("entity.assetchangelog.assetcode", "zh-CN", "资产编码", "资产编码（冗余）"),
            // entity.assetchangelog.assetcode
            new TranslationSeedItem("entity.assetchangelog.assetcode", "zh-HK", "资产编码_hk", "资产编码（冗余）"),

            // entity.assetchangelog.changefields
            new TranslationSeedItem("entity.assetchangelog.changefields", "en-US", "变更字段列表_us", "变更字段列表 JSON"),
            // entity.assetchangelog.changefields
            new TranslationSeedItem("entity.assetchangelog.changefields", "ja-JP", "变更字段列表_jp", "变更字段列表 JSON"),
            // entity.assetchangelog.changefields
            new TranslationSeedItem("entity.assetchangelog.changefields", "zh-CN", "变更字段列表", "变更字段列表 JSON"),
            // entity.assetchangelog.changefields
            new TranslationSeedItem("entity.assetchangelog.changefields", "zh-HK", "变更字段列表_hk", "变更字段列表 JSON"),

            // entity.assetchangelog.changetime
            new TranslationSeedItem("entity.assetchangelog.changetime", "en-US", "变更时间_us", "变更时间"),
            // entity.assetchangelog.changetime
            new TranslationSeedItem("entity.assetchangelog.changetime", "ja-JP", "变更时间_jp", "变更时间"),
            // entity.assetchangelog.changetime
            new TranslationSeedItem("entity.assetchangelog.changetime", "zh-CN", "变更时间", "变更时间"),
            // entity.assetchangelog.changetime
            new TranslationSeedItem("entity.assetchangelog.changetime", "zh-HK", "变更时间_hk", "变更时间"),

            // entity.assetchangelog.changeby
            new TranslationSeedItem("entity.assetchangelog.changeby", "en-US", "变更人_us", "变更人"),
            // entity.assetchangelog.changeby
            new TranslationSeedItem("entity.assetchangelog.changeby", "ja-JP", "变更人_jp", "变更人"),
            // entity.assetchangelog.changeby
            new TranslationSeedItem("entity.assetchangelog.changeby", "zh-CN", "变更人", "变更人"),
            // entity.assetchangelog.changeby
            new TranslationSeedItem("entity.assetchangelog.changeby", "zh-HK", "变更人_hk", "变更人"),

            // entity.assetchangelog.changereason
            new TranslationSeedItem("entity.assetchangelog.changereason", "en-US", "变更原因_us", "变更原因"),
            // entity.assetchangelog.changereason
            new TranslationSeedItem("entity.assetchangelog.changereason", "ja-JP", "变更原因_jp", "变更原因"),
            // entity.assetchangelog.changereason
            new TranslationSeedItem("entity.assetchangelog.changereason", "zh-CN", "变更原因", "变更原因"),
            // entity.assetchangelog.changereason
            new TranslationSeedItem("entity.assetchangelog.changereason", "zh-HK", "变更原因_hk", "变更原因"),

            // entity.assetchangelog.asset
            new TranslationSeedItem("entity.assetchangelog.asset", "en-US", "资产主表_us", "资产主表"),
            // entity.assetchangelog.asset
            new TranslationSeedItem("entity.assetchangelog.asset", "ja-JP", "资产主表_jp", "资产主表"),
            // entity.assetchangelog.asset
            new TranslationSeedItem("entity.assetchangelog.asset", "zh-CN", "资产主表", "资产主表"),
            // entity.assetchangelog.asset
            new TranslationSeedItem("entity.assetchangelog.asset", "zh-HK", "资产主表_hk", "资产主表"),
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
        translation.ResourceGroup = "Financial";
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
