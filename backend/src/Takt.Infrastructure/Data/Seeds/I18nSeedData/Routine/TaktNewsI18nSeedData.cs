// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine
// 文件名称：TaktNewsI18nSeedData.cs
// 创建时间：2026-06-04
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktNews 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktNews 实体国际化翻译种子（键前缀 entity.news.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktNewsI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktNews 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 news 实体翻译...", tenantCode);

        foreach (var item in GetNewsTranslations())
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

        TaktLogger.Information("TaktNews 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktNews 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.news._self / entity.news.{{field}}；ResourceGroup=2；ResourceType=0
    /// </summary>
    private static List<TranslationSeedItem> GetNewsTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.news._self
            new TranslationSeedItem("entity.news._self", "en-US", "News Information", "实体名称"),
            // entity.news._self
            new TranslationSeedItem("entity.news._self", "ja-JP", "新闻中心主信息", "实体名称"),
            // entity.news._self
            new TranslationSeedItem("entity.news._self", "zh-CN", "新闻中心主信息", "实体名称"),
            // entity.news._self
            new TranslationSeedItem("entity.news._self", "zh-HK", "新闻中心主信息", "实体名称"),

            // entity.news.code
            new TranslationSeedItem("entity.news.code", "en-US", "新闻编码", "新闻编码（租户+公司内唯一）"),
            // entity.news.code
            new TranslationSeedItem("entity.news.code", "ja-JP", "新闻编码", "新闻编码（租户+公司内唯一）"),
            // entity.news.code
            new TranslationSeedItem("entity.news.code", "zh-CN", "新闻编码", "新闻编码（租户+公司内唯一）"),
            // entity.news.code
            new TranslationSeedItem("entity.news.code", "zh-HK", "新闻编码", "新闻编码（租户+公司内唯一）"),

            // entity.news.category
            new TranslationSeedItem("entity.news.category", "en-US", "新闻分类", "新闻分类"),
            // entity.news.category
            new TranslationSeedItem("entity.news.category", "ja-JP", "新闻分类", "新闻分类"),
            // entity.news.category
            new TranslationSeedItem("entity.news.category", "zh-CN", "新闻分类", "新闻分类"),
            // entity.news.category
            new TranslationSeedItem("entity.news.category", "zh-HK", "新闻分类", "新闻分类"),

            // entity.news.title
            new TranslationSeedItem("entity.news.title", "en-US", "新闻标题", "新闻标题"),
            // entity.news.title
            new TranslationSeedItem("entity.news.title", "ja-JP", "新闻标题", "新闻标题"),
            // entity.news.title
            new TranslationSeedItem("entity.news.title", "zh-CN", "新闻标题", "新闻标题"),
            // entity.news.title
            new TranslationSeedItem("entity.news.title", "zh-HK", "新闻标题", "新闻标题"),

            // entity.news.summary
            new TranslationSeedItem("entity.news.summary", "en-US", "新闻摘要", "新闻摘要"),
            // entity.news.summary
            new TranslationSeedItem("entity.news.summary", "ja-JP", "新闻摘要", "新闻摘要"),
            // entity.news.summary
            new TranslationSeedItem("entity.news.summary", "zh-CN", "新闻摘要", "新闻摘要"),
            // entity.news.summary
            new TranslationSeedItem("entity.news.summary", "zh-HK", "新闻摘要", "新闻摘要"),

            // entity.news.content
            new TranslationSeedItem("entity.news.content", "en-US", "新闻内容", "新闻内容"),
            // entity.news.content
            new TranslationSeedItem("entity.news.content", "ja-JP", "新闻内容", "新闻内容"),
            // entity.news.content
            new TranslationSeedItem("entity.news.content", "zh-CN", "新闻内容", "新闻内容"),
            // entity.news.content
            new TranslationSeedItem("entity.news.content", "zh-HK", "新闻内容", "新闻内容"),

            // entity.news.coverimage
            new TranslationSeedItem("entity.news.coverimage", "en-US", "新闻封面图片URL", "新闻封面图片 URL"),
            // entity.news.coverimage
            new TranslationSeedItem("entity.news.coverimage", "ja-JP", "新闻封面图片URL", "新闻封面图片 URL"),
            // entity.news.coverimage
            new TranslationSeedItem("entity.news.coverimage", "zh-CN", "新闻封面图片URL", "新闻封面图片 URL"),
            // entity.news.coverimage
            new TranslationSeedItem("entity.news.coverimage", "zh-HK", "新闻封面图片URL", "新闻封面图片 URL"),

            // entity.news.effectivetime
            new TranslationSeedItem("entity.news.effectivetime", "en-US", "生效时间", "生效时间"),
            // entity.news.effectivetime
            new TranslationSeedItem("entity.news.effectivetime", "ja-JP", "生效时间", "生效时间"),
            // entity.news.effectivetime
            new TranslationSeedItem("entity.news.effectivetime", "zh-CN", "生效时间", "生效时间"),
            // entity.news.effectivetime
            new TranslationSeedItem("entity.news.effectivetime", "zh-HK", "生效时间", "生效时间"),

            // entity.news.expiretime
            new TranslationSeedItem("entity.news.expiretime", "en-US", "失效时间", "失效时间"),
            // entity.news.expiretime
            new TranslationSeedItem("entity.news.expiretime", "ja-JP", "失效时间", "失效时间"),
            // entity.news.expiretime
            new TranslationSeedItem("entity.news.expiretime", "zh-CN", "失效时间", "失效时间"),
            // entity.news.expiretime
            new TranslationSeedItem("entity.news.expiretime", "zh-HK", "失效时间", "失效时间"),

            // entity.news.readcount
            new TranslationSeedItem("entity.news.readcount", "en-US", "阅读次数", "阅读次数"),
            // entity.news.readcount
            new TranslationSeedItem("entity.news.readcount", "ja-JP", "阅读次数", "阅读次数"),
            // entity.news.readcount
            new TranslationSeedItem("entity.news.readcount", "zh-CN", "阅读次数", "阅读次数"),
            // entity.news.readcount
            new TranslationSeedItem("entity.news.readcount", "zh-HK", "阅读次数", "阅读次数"),

            // entity.news.likecount
            new TranslationSeedItem("entity.news.likecount", "en-US", "点赞次数", "点赞次数"),
            // entity.news.likecount
            new TranslationSeedItem("entity.news.likecount", "ja-JP", "点赞次数", "点赞次数"),
            // entity.news.likecount
            new TranslationSeedItem("entity.news.likecount", "zh-CN", "点赞次数", "点赞次数"),
            // entity.news.likecount
            new TranslationSeedItem("entity.news.likecount", "zh-HK", "点赞次数", "点赞次数"),

            // entity.news.commentcount
            new TranslationSeedItem("entity.news.commentcount", "en-US", "评论次数", "评论次数"),
            // entity.news.commentcount
            new TranslationSeedItem("entity.news.commentcount", "ja-JP", "评论次数", "评论次数"),
            // entity.news.commentcount
            new TranslationSeedItem("entity.news.commentcount", "zh-CN", "评论次数", "评论次数"),
            // entity.news.commentcount
            new TranslationSeedItem("entity.news.commentcount", "zh-HK", "评论次数", "评论次数"),

            // entity.news.favoritecount
            new TranslationSeedItem("entity.news.favoritecount", "en-US", "收藏次数", "收藏次数"),
            // entity.news.favoritecount
            new TranslationSeedItem("entity.news.favoritecount", "ja-JP", "收藏次数", "收藏次数"),
            // entity.news.favoritecount
            new TranslationSeedItem("entity.news.favoritecount", "zh-CN", "收藏次数", "收藏次数"),
            // entity.news.favoritecount
            new TranslationSeedItem("entity.news.favoritecount", "zh-HK", "收藏次数", "收藏次数"),

            // entity.news.sharecount
            new TranslationSeedItem("entity.news.sharecount", "en-US", "分享次数", "分享次数"),
            // entity.news.sharecount
            new TranslationSeedItem("entity.news.sharecount", "ja-JP", "分享次数", "分享次数"),
            // entity.news.sharecount
            new TranslationSeedItem("entity.news.sharecount", "zh-CN", "分享次数", "分享次数"),
            // entity.news.sharecount
            new TranslationSeedItem("entity.news.sharecount", "zh-HK", "分享次数", "分享次数"),

            // entity.news.attachmentcount
            new TranslationSeedItem("entity.news.attachmentcount", "en-US", "附件数量", "附件数量"),
            // entity.news.attachmentcount
            new TranslationSeedItem("entity.news.attachmentcount", "ja-JP", "附件数量", "附件数量"),
            // entity.news.attachmentcount
            new TranslationSeedItem("entity.news.attachmentcount", "zh-CN", "附件数量", "附件数量"),
            // entity.news.attachmentcount
            new TranslationSeedItem("entity.news.attachmentcount", "zh-HK", "附件数量", "附件数量"),

            // entity.news.deptid
            new TranslationSeedItem("entity.news.deptid", "en-US", "发布部门ID", "发布部门 ID"),
            // entity.news.deptid
            new TranslationSeedItem("entity.news.deptid", "ja-JP", "发布部门ID", "发布部门 ID"),
            // entity.news.deptid
            new TranslationSeedItem("entity.news.deptid", "zh-CN", "发布部门ID", "发布部门 ID"),
            // entity.news.deptid
            new TranslationSeedItem("entity.news.deptid", "zh-HK", "发布部门ID", "发布部门 ID"),

            // entity.news.deptname
            new TranslationSeedItem("entity.news.deptname", "en-US", "发布部门名称", "发布部门名称"),
            // entity.news.deptname
            new TranslationSeedItem("entity.news.deptname", "ja-JP", "发布部门名称", "发布部门名称"),
            // entity.news.deptname
            new TranslationSeedItem("entity.news.deptname", "zh-CN", "发布部门名称", "发布部门名称"),
            // entity.news.deptname
            new TranslationSeedItem("entity.news.deptname", "zh-HK", "发布部门名称", "发布部门名称"),

            // entity.news.publisherid
            new TranslationSeedItem("entity.news.publisherid", "en-US", "发布人ID", "发布人 ID"),
            // entity.news.publisherid
            new TranslationSeedItem("entity.news.publisherid", "ja-JP", "发布人ID", "发布人 ID"),
            // entity.news.publisherid
            new TranslationSeedItem("entity.news.publisherid", "zh-CN", "发布人ID", "发布人 ID"),
            // entity.news.publisherid
            new TranslationSeedItem("entity.news.publisherid", "zh-HK", "发布人ID", "发布人 ID"),

            // entity.news.publishername
            new TranslationSeedItem("entity.news.publishername", "en-US", "发布人姓名", "发布人姓名"),
            // entity.news.publishername
            new TranslationSeedItem("entity.news.publishername", "ja-JP", "发布人姓名", "发布人姓名"),
            // entity.news.publishername
            new TranslationSeedItem("entity.news.publishername", "zh-CN", "发布人姓名", "发布人姓名"),
            // entity.news.publishername
            new TranslationSeedItem("entity.news.publishername", "zh-HK", "发布人姓名", "发布人姓名"),

            // entity.news.publishtime
            new TranslationSeedItem("entity.news.publishtime", "en-US", "发布时间", "发布时间"),
            // entity.news.publishtime
            new TranslationSeedItem("entity.news.publishtime", "ja-JP", "发布时间", "发布时间"),
            // entity.news.publishtime
            new TranslationSeedItem("entity.news.publishtime", "zh-CN", "发布时间", "发布时间"),
            // entity.news.publishtime
            new TranslationSeedItem("entity.news.publishtime", "zh-HK", "发布时间", "发布时间"),

            // entity.news.sortorder
            new TranslationSeedItem("entity.news.sortorder", "en-US", "排序号", "排序号（越小越靠前）"),
            // entity.news.sortorder
            new TranslationSeedItem("entity.news.sortorder", "ja-JP", "排序号", "排序号（越小越靠前）"),
            // entity.news.sortorder
            new TranslationSeedItem("entity.news.sortorder", "zh-CN", "排序号", "排序号（越小越靠前）"),
            // entity.news.sortorder
            new TranslationSeedItem("entity.news.sortorder", "zh-HK", "排序号", "排序号（越小越靠前）"),

            // entity.news.status
            new TranslationSeedItem("entity.news.status", "en-US", "新闻状态", "新闻状态"),
            // entity.news.status
            new TranslationSeedItem("entity.news.status", "ja-JP", "新闻状态", "新闻状态"),
            // entity.news.status
            new TranslationSeedItem("entity.news.status", "zh-CN", "新闻状态", "新闻状态"),
            // entity.news.status
            new TranslationSeedItem("entity.news.status", "zh-HK", "新闻状态", "新闻状态"),

            // entity.news.attachments
            new TranslationSeedItem("entity.news.attachments", "en-US", "attachments", "新闻附件列表"),
            // entity.news.attachments
            new TranslationSeedItem("entity.news.attachments", "ja-JP", "attachments", "新闻附件列表"),
            // entity.news.attachments
            new TranslationSeedItem("entity.news.attachments", "zh-CN", "attachments", "新闻附件列表"),
            // entity.news.attachments
            new TranslationSeedItem("entity.news.attachments", "zh-HK", "attachments", "新闻附件列表"),
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
