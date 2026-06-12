// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine
// 文件名称：TaktNewsFavoriteI18nSeedData.cs
// 创建时间：2026-06-04
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktNewsFavorite 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine;

/// <summary>
/// TaktNewsFavorite 实体国际化翻译种子（键前缀 entity.newsFavorite.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktNewsFavoriteI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktNewsFavorite 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 newsFavorite 实体翻译...", tenantCode);

        foreach (var item in GetNewsFavoriteTranslations())
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

        TaktLogger.Information("TaktNewsFavorite 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktNewsFavorite 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.newsFavorite._self / entity.newsFavorite.{{field}}；ResourceGroup=2；ResourceType=0
    /// </summary>
    private static List<TranslationSeedItem> GetNewsFavoriteTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.newsFavorite._self
            new TranslationSeedItem("entity.newsFavorite._self", "en-US", "News Favorite Information", "实体名称"),
            // entity.newsFavorite._self
            new TranslationSeedItem("entity.newsFavorite._self", "ja-JP", "新闻收藏记录信息", "实体名称"),
            // entity.newsFavorite._self
            new TranslationSeedItem("entity.newsFavorite._self", "zh-CN", "新闻收藏记录信息", "实体名称"),
            // entity.newsFavorite._self
            new TranslationSeedItem("entity.newsFavorite._self", "zh-HK", "新闻收藏记录信息", "实体名称"),

            // entity.newsFavorite.newsid
            new TranslationSeedItem("entity.newsFavorite.newsid", "en-US", "新闻ID", "新闻 ID"),
            // entity.newsFavorite.newsid
            new TranslationSeedItem("entity.newsFavorite.newsid", "ja-JP", "新闻ID", "新闻 ID"),
            // entity.newsFavorite.newsid
            new TranslationSeedItem("entity.newsFavorite.newsid", "zh-CN", "新闻ID", "新闻 ID"),
            // entity.newsFavorite.newsid
            new TranslationSeedItem("entity.newsFavorite.newsid", "zh-HK", "新闻ID", "新闻 ID"),

            // entity.newsFavorite.userid
            new TranslationSeedItem("entity.newsFavorite.userid", "en-US", "用户ID", "用户 ID"),
            // entity.newsFavorite.userid
            new TranslationSeedItem("entity.newsFavorite.userid", "ja-JP", "用户ID", "用户 ID"),
            // entity.newsFavorite.userid
            new TranslationSeedItem("entity.newsFavorite.userid", "zh-CN", "用户ID", "用户 ID"),
            // entity.newsFavorite.userid
            new TranslationSeedItem("entity.newsFavorite.userid", "zh-HK", "用户ID", "用户 ID"),

            // entity.newsFavorite.username
            new TranslationSeedItem("entity.newsFavorite.username", "en-US", "用户姓名", "用户姓名"),
            // entity.newsFavorite.username
            new TranslationSeedItem("entity.newsFavorite.username", "ja-JP", "用户姓名", "用户姓名"),
            // entity.newsFavorite.username
            new TranslationSeedItem("entity.newsFavorite.username", "zh-CN", "用户姓名", "用户姓名"),
            // entity.newsFavorite.username
            new TranslationSeedItem("entity.newsFavorite.username", "zh-HK", "用户姓名", "用户姓名"),

            // entity.newsFavorite.favoritetime
            new TranslationSeedItem("entity.newsFavorite.favoritetime", "en-US", "收藏时间", "收藏时间"),
            // entity.newsFavorite.favoritetime
            new TranslationSeedItem("entity.newsFavorite.favoritetime", "ja-JP", "收藏时间", "收藏时间"),
            // entity.newsFavorite.favoritetime
            new TranslationSeedItem("entity.newsFavorite.favoritetime", "zh-CN", "收藏时间", "收藏时间"),
            // entity.newsFavorite.favoritetime
            new TranslationSeedItem("entity.newsFavorite.favoritetime", "zh-HK", "收藏时间", "收藏时间"),
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
        translation.ResourceGroup = 2;
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
