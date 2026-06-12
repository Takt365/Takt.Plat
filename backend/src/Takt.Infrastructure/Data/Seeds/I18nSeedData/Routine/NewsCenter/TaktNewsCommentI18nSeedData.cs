// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine.NewsCenter
// 文件名称：TaktNewsCommentI18nSeedData.cs
// 创建时间：2026-06-12
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktNewsComment 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine.NewsCenter;

/// <summary>
/// TaktNewsComment 实体国际化翻译种子（键前缀 entity.newscomment.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktNewsCommentI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktNewsComment 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 newscomment 实体翻译...", tenantCode);

        foreach (var item in GetNewsCommentTranslations())
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

        TaktLogger.Information("TaktNewsComment 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktNewsComment 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.newscomment._self / entity.newscomment.{{field}}；ResourceGroup=2；ResourceType=0
    /// </summary>
    private static List<TranslationSeedItem> GetNewsCommentTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.newscomment._self
            new TranslationSeedItem("entity.newscomment._self", "en-US", "News Comment Information", "实体名称"),
            // entity.newscomment._self
            new TranslationSeedItem("entity.newscomment._self", "ja-JP", "新闻中心评论信息", "实体名称"),
            // entity.newscomment._self
            new TranslationSeedItem("entity.newscomment._self", "zh-CN", "新闻中心评论信息", "实体名称"),
            // entity.newscomment._self
            new TranslationSeedItem("entity.newscomment._self", "zh-HK", "新闻中心评论信息", "实体名称"),

            // entity.newscomment.newsid
            new TranslationSeedItem("entity.newscomment.newsid", "en-US", "新闻ID", "新闻 ID"),
            // entity.newscomment.newsid
            new TranslationSeedItem("entity.newscomment.newsid", "ja-JP", "新闻ID", "新闻 ID"),
            // entity.newscomment.newsid
            new TranslationSeedItem("entity.newscomment.newsid", "zh-CN", "新闻ID", "新闻 ID"),
            // entity.newscomment.newsid
            new TranslationSeedItem("entity.newscomment.newsid", "zh-HK", "新闻ID", "新闻 ID"),

            // entity.newscomment.parentid
            new TranslationSeedItem("entity.newscomment.parentid", "en-US", "父评论ID", "父评论 ID（0 表示顶级评论）"),
            // entity.newscomment.parentid
            new TranslationSeedItem("entity.newscomment.parentid", "ja-JP", "父评论ID", "父评论 ID（0 表示顶级评论）"),
            // entity.newscomment.parentid
            new TranslationSeedItem("entity.newscomment.parentid", "zh-CN", "父评论ID", "父评论 ID（0 表示顶级评论）"),
            // entity.newscomment.parentid
            new TranslationSeedItem("entity.newscomment.parentid", "zh-HK", "父评论ID", "父评论 ID（0 表示顶级评论）"),

            // entity.newscomment.userid
            new TranslationSeedItem("entity.newscomment.userid", "en-US", "评论人ID", "评论人 ID"),
            // entity.newscomment.userid
            new TranslationSeedItem("entity.newscomment.userid", "ja-JP", "评论人ID", "评论人 ID"),
            // entity.newscomment.userid
            new TranslationSeedItem("entity.newscomment.userid", "zh-CN", "评论人ID", "评论人 ID"),
            // entity.newscomment.userid
            new TranslationSeedItem("entity.newscomment.userid", "zh-HK", "评论人ID", "评论人 ID"),

            // entity.newscomment.username
            new TranslationSeedItem("entity.newscomment.username", "en-US", "评论人姓名", "评论人姓名"),
            // entity.newscomment.username
            new TranslationSeedItem("entity.newscomment.username", "ja-JP", "评论人姓名", "评论人姓名"),
            // entity.newscomment.username
            new TranslationSeedItem("entity.newscomment.username", "zh-CN", "评论人姓名", "评论人姓名"),
            // entity.newscomment.username
            new TranslationSeedItem("entity.newscomment.username", "zh-HK", "评论人姓名", "评论人姓名"),

            // entity.newscomment.useravatar
            new TranslationSeedItem("entity.newscomment.useravatar", "en-US", "评论人头像URL", "评论人头像 URL"),
            // entity.newscomment.useravatar
            new TranslationSeedItem("entity.newscomment.useravatar", "ja-JP", "评论人头像URL", "评论人头像 URL"),
            // entity.newscomment.useravatar
            new TranslationSeedItem("entity.newscomment.useravatar", "zh-CN", "评论人头像URL", "评论人头像 URL"),
            // entity.newscomment.useravatar
            new TranslationSeedItem("entity.newscomment.useravatar", "zh-HK", "评论人头像URL", "评论人头像 URL"),

            // entity.newscomment.replytouserid
            new TranslationSeedItem("entity.newscomment.replytouserid", "en-US", "被回复人ID", "被回复人 ID"),
            // entity.newscomment.replytouserid
            new TranslationSeedItem("entity.newscomment.replytouserid", "ja-JP", "被回复人ID", "被回复人 ID"),
            // entity.newscomment.replytouserid
            new TranslationSeedItem("entity.newscomment.replytouserid", "zh-CN", "被回复人ID", "被回复人 ID"),
            // entity.newscomment.replytouserid
            new TranslationSeedItem("entity.newscomment.replytouserid", "zh-HK", "被回复人ID", "被回复人 ID"),

            // entity.newscomment.replytousername
            new TranslationSeedItem("entity.newscomment.replytousername", "en-US", "被回复人姓名", "被回复人姓名"),
            // entity.newscomment.replytousername
            new TranslationSeedItem("entity.newscomment.replytousername", "ja-JP", "被回复人姓名", "被回复人姓名"),
            // entity.newscomment.replytousername
            new TranslationSeedItem("entity.newscomment.replytousername", "zh-CN", "被回复人姓名", "被回复人姓名"),
            // entity.newscomment.replytousername
            new TranslationSeedItem("entity.newscomment.replytousername", "zh-HK", "被回复人姓名", "被回复人姓名"),

            // entity.newscomment.commentcontent
            new TranslationSeedItem("entity.newscomment.commentcontent", "en-US", "评论内容", "评论内容"),
            // entity.newscomment.commentcontent
            new TranslationSeedItem("entity.newscomment.commentcontent", "ja-JP", "评论内容", "评论内容"),
            // entity.newscomment.commentcontent
            new TranslationSeedItem("entity.newscomment.commentcontent", "zh-CN", "评论内容", "评论内容"),
            // entity.newscomment.commentcontent
            new TranslationSeedItem("entity.newscomment.commentcontent", "zh-HK", "评论内容", "评论内容"),

            // entity.newscomment.commenttime
            new TranslationSeedItem("entity.newscomment.commenttime", "en-US", "评论时间", "评论时间"),
            // entity.newscomment.commenttime
            new TranslationSeedItem("entity.newscomment.commenttime", "ja-JP", "评论时间", "评论时间"),
            // entity.newscomment.commenttime
            new TranslationSeedItem("entity.newscomment.commenttime", "zh-CN", "评论时间", "评论时间"),
            // entity.newscomment.commenttime
            new TranslationSeedItem("entity.newscomment.commenttime", "zh-HK", "评论时间", "评论时间"),

            // entity.newscomment.likecount
            new TranslationSeedItem("entity.newscomment.likecount", "en-US", "点赞次数", "点赞次数"),
            // entity.newscomment.likecount
            new TranslationSeedItem("entity.newscomment.likecount", "ja-JP", "点赞次数", "点赞次数"),
            // entity.newscomment.likecount
            new TranslationSeedItem("entity.newscomment.likecount", "zh-CN", "点赞次数", "点赞次数"),
            // entity.newscomment.likecount
            new TranslationSeedItem("entity.newscomment.likecount", "zh-HK", "点赞次数", "点赞次数"),

            // entity.newscomment.replycount
            new TranslationSeedItem("entity.newscomment.replycount", "en-US", "回复次数", "回复次数（子评论数量）"),
            // entity.newscomment.replycount
            new TranslationSeedItem("entity.newscomment.replycount", "ja-JP", "回复次数", "回复次数（子评论数量）"),
            // entity.newscomment.replycount
            new TranslationSeedItem("entity.newscomment.replycount", "zh-CN", "回复次数", "回复次数（子评论数量）"),
            // entity.newscomment.replycount
            new TranslationSeedItem("entity.newscomment.replycount", "zh-HK", "回复次数", "回复次数（子评论数量）"),

            // entity.newscomment.commentlevel
            new TranslationSeedItem("entity.newscomment.commentlevel", "en-US", "评论层级", "评论层级（0=顶级，最多 3 级）"),
            // entity.newscomment.commentlevel
            new TranslationSeedItem("entity.newscomment.commentlevel", "ja-JP", "评论层级", "评论层级（0=顶级，最多 3 级）"),
            // entity.newscomment.commentlevel
            new TranslationSeedItem("entity.newscomment.commentlevel", "zh-CN", "评论层级", "评论层级（0=顶级，最多 3 级）"),
            // entity.newscomment.commentlevel
            new TranslationSeedItem("entity.newscomment.commentlevel", "zh-HK", "评论层级", "评论层级（0=顶级，最多 3 级）"),

            // entity.newscomment.flowinstanceid
            new TranslationSeedItem("entity.newscomment.flowinstanceid", "en-US", "流程实例ID", "流程实例 ID（评论审核工作流）"),
            // entity.newscomment.flowinstanceid
            new TranslationSeedItem("entity.newscomment.flowinstanceid", "ja-JP", "流程实例ID", "流程实例 ID（评论审核工作流）"),
            // entity.newscomment.flowinstanceid
            new TranslationSeedItem("entity.newscomment.flowinstanceid", "zh-CN", "流程实例ID", "流程实例 ID（评论审核工作流）"),
            // entity.newscomment.flowinstanceid
            new TranslationSeedItem("entity.newscomment.flowinstanceid", "zh-HK", "流程实例ID", "流程实例 ID（评论审核工作流）"),

            // entity.newscomment.commentstatus
            new TranslationSeedItem("entity.newscomment.commentstatus", "en-US", "评论状态", "评论状态"),
            // entity.newscomment.commentstatus
            new TranslationSeedItem("entity.newscomment.commentstatus", "ja-JP", "评论状态", "评论状态"),
            // entity.newscomment.commentstatus
            new TranslationSeedItem("entity.newscomment.commentstatus", "zh-CN", "评论状态", "评论状态"),
            // entity.newscomment.commentstatus
            new TranslationSeedItem("entity.newscomment.commentstatus", "zh-HK", "评论状态", "评论状态"),

            // entity.newscomment.news
            new TranslationSeedItem("entity.newscomment.news", "en-US", "新闻", "新闻（主表）"),
            // entity.newscomment.news
            new TranslationSeedItem("entity.newscomment.news", "ja-JP", "新闻", "新闻（主表）"),
            // entity.newscomment.news
            new TranslationSeedItem("entity.newscomment.news", "zh-CN", "新闻", "新闻（主表）"),
            // entity.newscomment.news
            new TranslationSeedItem("entity.newscomment.news", "zh-HK", "新闻", "新闻（主表）"),

            // entity.newscomment.likes
            new TranslationSeedItem("entity.newscomment.likes", "en-US", "评论点赞记录列表", "评论点赞记录列表（主子表关系）"),
            // entity.newscomment.likes
            new TranslationSeedItem("entity.newscomment.likes", "ja-JP", "评论点赞记录列表", "评论点赞记录列表（主子表关系）"),
            // entity.newscomment.likes
            new TranslationSeedItem("entity.newscomment.likes", "zh-CN", "评论点赞记录列表", "评论点赞记录列表（主子表关系）"),
            // entity.newscomment.likes
            new TranslationSeedItem("entity.newscomment.likes", "zh-HK", "评论点赞记录列表", "评论点赞记录列表（主子表关系）"),
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
