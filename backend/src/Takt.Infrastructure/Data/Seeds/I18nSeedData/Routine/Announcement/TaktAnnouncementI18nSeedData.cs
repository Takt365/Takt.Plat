// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine.Announcement
// 文件名称：TaktAnnouncementI18nSeedData.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktAnnouncement 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine.Announcement;

/// <summary>
/// TaktAnnouncement 实体国际化翻译种子（键前缀 entity.announcement.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktAnnouncementI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktAnnouncement 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 announcement 实体翻译...", tenantCode);

        foreach (var item in GetAnnouncementTranslations())
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

        TaktLogger.Information("TaktAnnouncement 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktAnnouncement 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.announcement._self / entity.announcement.{{field}}；ResourceGroup=TaktModule.Routine；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetAnnouncementTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.announcement._self
            new TranslationSeedItem("entity.announcement._self", "en-US", "Announcement Information", "实体名称"),
            // entity.announcement._self
            new TranslationSeedItem("entity.announcement._self", "ja-JP", "公告通知信息", "实体名称"),
            // entity.announcement._self
            new TranslationSeedItem("entity.announcement._self", "zh-CN", "公告通知信息", "实体名称"),
            // entity.announcement._self
            new TranslationSeedItem("entity.announcement._self", "zh-HK", "公告通知信息", "实体名称"),

            // entity.announcement.title
            new TranslationSeedItem("entity.announcement.title", "en-US", "公告标题", "公告标题"),
            // entity.announcement.title
            new TranslationSeedItem("entity.announcement.title", "ja-JP", "公告标题", "公告标题"),
            // entity.announcement.title
            new TranslationSeedItem("entity.announcement.title", "zh-CN", "公告标题", "公告标题"),
            // entity.announcement.title
            new TranslationSeedItem("entity.announcement.title", "zh-HK", "公告标题", "公告标题"),

            // entity.announcement.type
            new TranslationSeedItem("entity.announcement.type", "en-US", "公告类型", "公告类型（1=公告，2=通知，3=新闻，4=紧急通知）"),
            // entity.announcement.type
            new TranslationSeedItem("entity.announcement.type", "ja-JP", "公告类型", "公告类型（1=公告，2=通知，3=新闻，4=紧急通知）"),
            // entity.announcement.type
            new TranslationSeedItem("entity.announcement.type", "zh-CN", "公告类型", "公告类型（1=公告，2=通知，3=新闻，4=紧急通知）"),
            // entity.announcement.type
            new TranslationSeedItem("entity.announcement.type", "zh-HK", "公告类型", "公告类型（1=公告，2=通知，3=新闻，4=紧急通知）"),

            // entity.announcement.content
            new TranslationSeedItem("entity.announcement.content", "en-US", "公告内容", "公告内容（富文本 HTML）"),
            // entity.announcement.content
            new TranslationSeedItem("entity.announcement.content", "ja-JP", "公告内容", "公告内容（富文本 HTML）"),
            // entity.announcement.content
            new TranslationSeedItem("entity.announcement.content", "zh-CN", "公告内容", "公告内容（富文本 HTML）"),
            // entity.announcement.content
            new TranslationSeedItem("entity.announcement.content", "zh-HK", "公告内容", "公告内容（富文本 HTML）"),

            // entity.announcement.summary
            new TranslationSeedItem("entity.announcement.summary", "en-US", "公告摘要", "公告摘要（用于列表展示）"),
            // entity.announcement.summary
            new TranslationSeedItem("entity.announcement.summary", "ja-JP", "公告摘要", "公告摘要（用于列表展示）"),
            // entity.announcement.summary
            new TranslationSeedItem("entity.announcement.summary", "zh-CN", "公告摘要", "公告摘要（用于列表展示）"),
            // entity.announcement.summary
            new TranslationSeedItem("entity.announcement.summary", "zh-HK", "公告摘要", "公告摘要（用于列表展示）"),

            // entity.announcement.tags
            new TranslationSeedItem("entity.announcement.tags", "en-US", "标签", "标签（逗号分隔或 JSON 数组存储）"),
            // entity.announcement.tags
            new TranslationSeedItem("entity.announcement.tags", "ja-JP", "标签", "标签（逗号分隔或 JSON 数组存储）"),
            // entity.announcement.tags
            new TranslationSeedItem("entity.announcement.tags", "zh-CN", "标签", "标签（逗号分隔或 JSON 数组存储）"),
            // entity.announcement.tags
            new TranslationSeedItem("entity.announcement.tags", "zh-HK", "标签", "标签（逗号分隔或 JSON 数组存储）"),

            // entity.announcement.attachments
            new TranslationSeedItem("entity.announcement.attachments", "en-US", "附件路径", "附件路径（多个附件用逗号分隔）"),
            // entity.announcement.attachments
            new TranslationSeedItem("entity.announcement.attachments", "ja-JP", "附件路径", "附件路径（多个附件用逗号分隔）"),
            // entity.announcement.attachments
            new TranslationSeedItem("entity.announcement.attachments", "zh-CN", "附件路径", "附件路径（多个附件用逗号分隔）"),
            // entity.announcement.attachments
            new TranslationSeedItem("entity.announcement.attachments", "zh-HK", "附件路径", "附件路径（多个附件用逗号分隔）"),

            // entity.announcement.publishtime
            new TranslationSeedItem("entity.announcement.publishtime", "en-US", "发布时间", "发布时间（定时发布时使用）"),
            // entity.announcement.publishtime
            new TranslationSeedItem("entity.announcement.publishtime", "ja-JP", "发布时间", "发布时间（定时发布时使用）"),
            // entity.announcement.publishtime
            new TranslationSeedItem("entity.announcement.publishtime", "zh-CN", "发布时间", "发布时间（定时发布时使用）"),
            // entity.announcement.publishtime
            new TranslationSeedItem("entity.announcement.publishtime", "zh-HK", "发布时间", "发布时间（定时发布时使用）"),

            // entity.announcement.isscheduled
            new TranslationSeedItem("entity.announcement.isscheduled", "en-US", "是否定时发布", "是否定时发布（1=是，0=否）"),
            // entity.announcement.isscheduled
            new TranslationSeedItem("entity.announcement.isscheduled", "ja-JP", "是否定时发布", "是否定时发布（1=是，0=否）"),
            // entity.announcement.isscheduled
            new TranslationSeedItem("entity.announcement.isscheduled", "zh-CN", "是否定时发布", "是否定时发布（1=是，0=否）"),
            // entity.announcement.isscheduled
            new TranslationSeedItem("entity.announcement.isscheduled", "zh-HK", "是否定时发布", "是否定时发布（1=是，0=否）"),

            // entity.announcement.istop
            new TranslationSeedItem("entity.announcement.istop", "en-US", "是否置顶", "是否置顶（1=是，0=否）"),
            // entity.announcement.istop
            new TranslationSeedItem("entity.announcement.istop", "ja-JP", "是否置顶", "是否置顶（1=是，0=否）"),
            // entity.announcement.istop
            new TranslationSeedItem("entity.announcement.istop", "zh-CN", "是否置顶", "是否置顶（1=是，0=否）"),
            // entity.announcement.istop
            new TranslationSeedItem("entity.announcement.istop", "zh-HK", "是否置顶", "是否置顶（1=是，0=否）"),

            // entity.announcement.toppriority
            new TranslationSeedItem("entity.announcement.toppriority", "en-US", "置顶优先级", "置顶优先级（数字越大越靠前）"),
            // entity.announcement.toppriority
            new TranslationSeedItem("entity.announcement.toppriority", "ja-JP", "置顶优先级", "置顶优先级（数字越大越靠前）"),
            // entity.announcement.toppriority
            new TranslationSeedItem("entity.announcement.toppriority", "zh-CN", "置顶优先级", "置顶优先级（数字越大越靠前）"),
            // entity.announcement.toppriority
            new TranslationSeedItem("entity.announcement.toppriority", "zh-HK", "置顶优先级", "置顶优先级（数字越大越靠前）"),

            // entity.announcement.expiretime
            new TranslationSeedItem("entity.announcement.expiretime", "en-US", "过期时间", "过期时间（过期后自动隐藏）"),
            // entity.announcement.expiretime
            new TranslationSeedItem("entity.announcement.expiretime", "ja-JP", "过期时间", "过期时间（过期后自动隐藏）"),
            // entity.announcement.expiretime
            new TranslationSeedItem("entity.announcement.expiretime", "zh-CN", "过期时间", "过期时间（过期后自动隐藏）"),
            // entity.announcement.expiretime
            new TranslationSeedItem("entity.announcement.expiretime", "zh-HK", "过期时间", "过期时间（过期后自动隐藏）"),

            // entity.announcement.viewcount
            new TranslationSeedItem("entity.announcement.viewcount", "en-US", "查看次数", "查看次数"),
            // entity.announcement.viewcount
            new TranslationSeedItem("entity.announcement.viewcount", "ja-JP", "查看次数", "查看次数"),
            // entity.announcement.viewcount
            new TranslationSeedItem("entity.announcement.viewcount", "zh-CN", "查看次数", "查看次数"),
            // entity.announcement.viewcount
            new TranslationSeedItem("entity.announcement.viewcount", "zh-HK", "查看次数", "查看次数"),

            // entity.announcement.targetscope
            new TranslationSeedItem("entity.announcement.targetscope", "en-US", "目标范围", "目标范围（all=全员，company=本公司，department=本部门，custom=自定义）"),
            // entity.announcement.targetscope
            new TranslationSeedItem("entity.announcement.targetscope", "ja-JP", "目标范围", "目标范围（all=全员，company=本公司，department=本部门，custom=自定义）"),
            // entity.announcement.targetscope
            new TranslationSeedItem("entity.announcement.targetscope", "zh-CN", "目标范围", "目标范围（all=全员，company=本公司，department=本部门，custom=自定义）"),
            // entity.announcement.targetscope
            new TranslationSeedItem("entity.announcement.targetscope", "zh-HK", "目标范围", "目标范围（all=全员，company=本公司，department=本部门，custom=自定义）"),

            // entity.announcement.targetdepartments
            new TranslationSeedItem("entity.announcement.targetdepartments", "en-US", "目标部门编码", "目标部门编码（多个用逗号分隔，当 target_scope=department 时使用）"),
            // entity.announcement.targetdepartments
            new TranslationSeedItem("entity.announcement.targetdepartments", "ja-JP", "目标部门编码", "目标部门编码（多个用逗号分隔，当 target_scope=department 时使用）"),
            // entity.announcement.targetdepartments
            new TranslationSeedItem("entity.announcement.targetdepartments", "zh-CN", "目标部门编码", "目标部门编码（多个用逗号分隔，当 target_scope=department 时使用）"),
            // entity.announcement.targetdepartments
            new TranslationSeedItem("entity.announcement.targetdepartments", "zh-HK", "目标部门编码", "目标部门编码（多个用逗号分隔，当 target_scope=department 时使用）"),

            // entity.announcement.targetusers
            new TranslationSeedItem("entity.announcement.targetusers", "en-US", "目标用户ID", "目标用户 ID（多个用逗号分隔，当 target_scope=custom 时使用）"),
            // entity.announcement.targetusers
            new TranslationSeedItem("entity.announcement.targetusers", "ja-JP", "目标用户ID", "目标用户 ID（多个用逗号分隔，当 target_scope=custom 时使用）"),
            // entity.announcement.targetusers
            new TranslationSeedItem("entity.announcement.targetusers", "zh-CN", "目标用户ID", "目标用户 ID（多个用逗号分隔，当 target_scope=custom 时使用）"),
            // entity.announcement.targetusers
            new TranslationSeedItem("entity.announcement.targetusers", "zh-HK", "目标用户ID", "目标用户 ID（多个用逗号分隔，当 target_scope=custom 时使用）"),

            // entity.announcement.status
            new TranslationSeedItem("entity.announcement.status", "en-US", "状态", "状态（0=草稿，1=已发布，2=已撤回，3=已过期）"),
            // entity.announcement.status
            new TranslationSeedItem("entity.announcement.status", "ja-JP", "状态", "状态（0=草稿，1=已发布，2=已撤回，3=已过期）"),
            // entity.announcement.status
            new TranslationSeedItem("entity.announcement.status", "zh-CN", "状态", "状态（0=草稿，1=已发布，2=已撤回，3=已过期）"),
            // entity.announcement.status
            new TranslationSeedItem("entity.announcement.status", "zh-HK", "状态", "状态（0=草稿，1=已发布，2=已撤回，3=已过期）"),
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
