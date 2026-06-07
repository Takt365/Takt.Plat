// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine.NewsCenter
// 文件名称：TaktNewsCommentLikeI18nSeedData.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktNewsCommentLike 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine.NewsCenter;

/// <summary>
/// TaktNewsCommentLike 实体国际化翻译种子（键前缀 entity.newsCommentLike.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktNewsCommentLikeI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktNewsCommentLike 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 newsCommentLike 实体翻译...", tenantCode);

        foreach (var item in GetNewsCommentLikeTranslations())
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

        TaktLogger.Information("TaktNewsCommentLike 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktNewsCommentLike 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.newsCommentLike._self / entity.newsCommentLike.{{field}}；ResourceGroup=TaktModule.Routine；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetNewsCommentLikeTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.newsCommentLike._self
            new TranslationSeedItem("entity.newsCommentLike._self", "en-US", "News Comment Like Information", "实体名称"),
            // entity.newsCommentLike._self
            new TranslationSeedItem("entity.newsCommentLike._self", "ja-JP", "新闻中心评论点赞记录信息", "实体名称"),
            // entity.newsCommentLike._self
            new TranslationSeedItem("entity.newsCommentLike._self", "zh-CN", "新闻中心评论点赞记录信息", "实体名称"),
            // entity.newsCommentLike._self
            new TranslationSeedItem("entity.newsCommentLike._self", "zh-HK", "新闻中心评论点赞记录信息", "实体名称"),

            // entity.newsCommentLike.commentid
            new TranslationSeedItem("entity.newsCommentLike.commentid", "en-US", "评论ID", "评论 ID"),
            // entity.newsCommentLike.commentid
            new TranslationSeedItem("entity.newsCommentLike.commentid", "ja-JP", "评论ID", "评论 ID"),
            // entity.newsCommentLike.commentid
            new TranslationSeedItem("entity.newsCommentLike.commentid", "zh-CN", "评论ID", "评论 ID"),
            // entity.newsCommentLike.commentid
            new TranslationSeedItem("entity.newsCommentLike.commentid", "zh-HK", "评论ID", "评论 ID"),

            // entity.newsCommentLike.userid
            new TranslationSeedItem("entity.newsCommentLike.userid", "en-US", "用户ID", "用户 ID"),
            // entity.newsCommentLike.userid
            new TranslationSeedItem("entity.newsCommentLike.userid", "ja-JP", "用户ID", "用户 ID"),
            // entity.newsCommentLike.userid
            new TranslationSeedItem("entity.newsCommentLike.userid", "zh-CN", "用户ID", "用户 ID"),
            // entity.newsCommentLike.userid
            new TranslationSeedItem("entity.newsCommentLike.userid", "zh-HK", "用户ID", "用户 ID"),

            // entity.newsCommentLike.username
            new TranslationSeedItem("entity.newsCommentLike.username", "en-US", "用户姓名", "用户姓名"),
            // entity.newsCommentLike.username
            new TranslationSeedItem("entity.newsCommentLike.username", "ja-JP", "用户姓名", "用户姓名"),
            // entity.newsCommentLike.username
            new TranslationSeedItem("entity.newsCommentLike.username", "zh-CN", "用户姓名", "用户姓名"),
            // entity.newsCommentLike.username
            new TranslationSeedItem("entity.newsCommentLike.username", "zh-HK", "用户姓名", "用户姓名"),

            // entity.newsCommentLike.liketime
            new TranslationSeedItem("entity.newsCommentLike.liketime", "en-US", "点赞时间", "点赞时间"),
            // entity.newsCommentLike.liketime
            new TranslationSeedItem("entity.newsCommentLike.liketime", "ja-JP", "点赞时间", "点赞时间"),
            // entity.newsCommentLike.liketime
            new TranslationSeedItem("entity.newsCommentLike.liketime", "zh-CN", "点赞时间", "点赞时间"),
            // entity.newsCommentLike.liketime
            new TranslationSeedItem("entity.newsCommentLike.liketime", "zh-HK", "点赞时间", "点赞时间"),
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
        translation.ResourceGroup = TaktModule.Routine;
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
