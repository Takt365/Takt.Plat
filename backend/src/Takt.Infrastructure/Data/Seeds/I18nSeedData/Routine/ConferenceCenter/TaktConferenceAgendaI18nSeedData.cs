// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine.ConferenceCenter
// 文件名称：TaktConferenceAgendaI18nSeedData.cs
// 创建时间：2026-08-18
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktConferenceAgenda 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine.ConferenceCenter;

/// <summary>
/// TaktConferenceAgenda 实体国际化翻译种子（键前缀 entity.conferenceagenda.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktConferenceAgendaI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktConferenceAgenda 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 conferenceagenda 实体翻译...", tenantCode);

        foreach (var item in GetConferenceAgendaTranslations())
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

        TaktLogger.Information("TaktConferenceAgenda 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktConferenceAgenda 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.conferenceagenda._self / entity.conferenceagenda.{{field}}；ResourceGroup=ConferenceCenter；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetConferenceAgendaTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.conferenceagenda._self
            new TranslationSeedItem("entity.conferenceagenda._self", "en-US", "Conference Agenda Information_us", "实体名称"),
            // entity.conferenceagenda._self
            new TranslationSeedItem("entity.conferenceagenda._self", "ja-JP", "会议议程/纪要信息_jp", "实体名称"),
            // entity.conferenceagenda._self
            new TranslationSeedItem("entity.conferenceagenda._self", "zh-CN", "会议议程/纪要信息", "实体名称"),
            // entity.conferenceagenda._self
            new TranslationSeedItem("entity.conferenceagenda._self", "zh-HK", "会议议程/纪要信息_hk", "实体名称"),

            // entity.conferenceagenda.conferenceid
            new TranslationSeedItem("entity.conferenceagenda.conferenceid", "en-US", "会议ID_us", "会议 ID（选项 TaktConferences/options；DictValue=Id）"),
            // entity.conferenceagenda.conferenceid
            new TranslationSeedItem("entity.conferenceagenda.conferenceid", "ja-JP", "会议ID_jp", "会议 ID（选项 TaktConferences/options；DictValue=Id）"),
            // entity.conferenceagenda.conferenceid
            new TranslationSeedItem("entity.conferenceagenda.conferenceid", "zh-CN", "会议ID", "会议 ID（选项 TaktConferences/options；DictValue=Id）"),
            // entity.conferenceagenda.conferenceid
            new TranslationSeedItem("entity.conferenceagenda.conferenceid", "zh-HK", "会议ID_hk", "会议 ID（选项 TaktConferences/options；DictValue=Id）"),

            // entity.conferenceagenda.recordtype
            new TranslationSeedItem("entity.conferenceagenda.recordtype", "en-US", "记录类型_us", "记录类型（字典 routine_conference_record_type；0=议程项 1=会议纪要）"),
            // entity.conferenceagenda.recordtype
            new TranslationSeedItem("entity.conferenceagenda.recordtype", "ja-JP", "记录类型_jp", "记录类型（字典 routine_conference_record_type；0=议程项 1=会议纪要）"),
            // entity.conferenceagenda.recordtype
            new TranslationSeedItem("entity.conferenceagenda.recordtype", "zh-CN", "记录类型", "记录类型（字典 routine_conference_record_type；0=议程项 1=会议纪要）"),
            // entity.conferenceagenda.recordtype
            new TranslationSeedItem("entity.conferenceagenda.recordtype", "zh-HK", "记录类型_hk", "记录类型（字典 routine_conference_record_type；0=议程项 1=会议纪要）"),

            // entity.conferenceagenda.linenumber
            new TranslationSeedItem("entity.conferenceagenda.linenumber", "en-US", "行号_us", "行号（议程项序号，固定步长=10；纪要通常为 10）"),
            // entity.conferenceagenda.linenumber
            new TranslationSeedItem("entity.conferenceagenda.linenumber", "ja-JP", "行号_jp", "行号（议程项序号，固定步长=10；纪要通常为 10）"),
            // entity.conferenceagenda.linenumber
            new TranslationSeedItem("entity.conferenceagenda.linenumber", "zh-CN", "行号", "行号（议程项序号，固定步长=10；纪要通常为 10）"),
            // entity.conferenceagenda.linenumber
            new TranslationSeedItem("entity.conferenceagenda.linenumber", "zh-HK", "行号_hk", "行号（议程项序号，固定步长=10；纪要通常为 10）"),

            // entity.conferenceagenda.title
            new TranslationSeedItem("entity.conferenceagenda.title", "en-US", "标题_us", "标题（议程议题或纪要标题）"),
            // entity.conferenceagenda.title
            new TranslationSeedItem("entity.conferenceagenda.title", "ja-JP", "标题_jp", "标题（议程议题或纪要标题）"),
            // entity.conferenceagenda.title
            new TranslationSeedItem("entity.conferenceagenda.title", "zh-CN", "标题", "标题（议程议题或纪要标题）"),
            // entity.conferenceagenda.title
            new TranslationSeedItem("entity.conferenceagenda.title", "zh-HK", "标题_hk", "标题（议程议题或纪要标题）"),

            // entity.conferenceagenda.content
            new TranslationSeedItem("entity.conferenceagenda.content", "en-US", "正文_us", "正文（议程说明或会议纪要富文本 HTML）"),
            // entity.conferenceagenda.content
            new TranslationSeedItem("entity.conferenceagenda.content", "ja-JP", "正文_jp", "正文（议程说明或会议纪要富文本 HTML）"),
            // entity.conferenceagenda.content
            new TranslationSeedItem("entity.conferenceagenda.content", "zh-CN", "正文", "正文（议程说明或会议纪要富文本 HTML）"),
            // entity.conferenceagenda.content
            new TranslationSeedItem("entity.conferenceagenda.content", "zh-HK", "正文_hk", "正文（议程说明或会议纪要富文本 HTML）"),

            // entity.conferenceagenda.summary
            new TranslationSeedItem("entity.conferenceagenda.summary", "en-US", "摘要_us", "摘要（纪要列表展示用）"),
            // entity.conferenceagenda.summary
            new TranslationSeedItem("entity.conferenceagenda.summary", "ja-JP", "摘要_jp", "摘要（纪要列表展示用）"),
            // entity.conferenceagenda.summary
            new TranslationSeedItem("entity.conferenceagenda.summary", "zh-CN", "摘要", "摘要（纪要列表展示用）"),
            // entity.conferenceagenda.summary
            new TranslationSeedItem("entity.conferenceagenda.summary", "zh-HK", "摘要_hk", "摘要（纪要列表展示用）"),

            // entity.conferenceagenda.presenterid
            new TranslationSeedItem("entity.conferenceagenda.presenterid", "en-US", "主讲人ID_us", "主讲人/汇报人 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.conferenceagenda.presenterid
            new TranslationSeedItem("entity.conferenceagenda.presenterid", "ja-JP", "主讲人ID_jp", "主讲人/汇报人 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.conferenceagenda.presenterid
            new TranslationSeedItem("entity.conferenceagenda.presenterid", "zh-CN", "主讲人ID", "主讲人/汇报人 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.conferenceagenda.presenterid
            new TranslationSeedItem("entity.conferenceagenda.presenterid", "zh-HK", "主讲人ID_hk", "主讲人/汇报人 ID（选项 TaktUsers/options；DictValue=Id）"),

            // entity.conferenceagenda.presentername
            new TranslationSeedItem("entity.conferenceagenda.presentername", "en-US", "主讲人姓名_us", "主讲人姓名（议程项）"),
            // entity.conferenceagenda.presentername
            new TranslationSeedItem("entity.conferenceagenda.presentername", "ja-JP", "主讲人姓名_jp", "主讲人姓名（议程项）"),
            // entity.conferenceagenda.presentername
            new TranslationSeedItem("entity.conferenceagenda.presentername", "zh-CN", "主讲人姓名", "主讲人姓名（议程项）"),
            // entity.conferenceagenda.presentername
            new TranslationSeedItem("entity.conferenceagenda.presentername", "zh-HK", "主讲人姓名_hk", "主讲人姓名（议程项）"),

            // entity.conferenceagenda.plannedstarttime
            new TranslationSeedItem("entity.conferenceagenda.plannedstarttime", "en-US", "计划开始时间_us", "计划开始时间（议程项）"),
            // entity.conferenceagenda.plannedstarttime
            new TranslationSeedItem("entity.conferenceagenda.plannedstarttime", "ja-JP", "计划开始时间_jp", "计划开始时间（议程项）"),
            // entity.conferenceagenda.plannedstarttime
            new TranslationSeedItem("entity.conferenceagenda.plannedstarttime", "zh-CN", "计划开始时间", "计划开始时间（议程项）"),
            // entity.conferenceagenda.plannedstarttime
            new TranslationSeedItem("entity.conferenceagenda.plannedstarttime", "zh-HK", "计划开始时间_hk", "计划开始时间（议程项）"),

            // entity.conferenceagenda.durationminutes
            new TranslationSeedItem("entity.conferenceagenda.durationminutes", "en-US", "计划时长分钟_us", "计划时长（分钟，议程项）"),
            // entity.conferenceagenda.durationminutes
            new TranslationSeedItem("entity.conferenceagenda.durationminutes", "ja-JP", "计划时长分钟_jp", "计划时长（分钟，议程项）"),
            // entity.conferenceagenda.durationminutes
            new TranslationSeedItem("entity.conferenceagenda.durationminutes", "zh-CN", "计划时长分钟", "计划时长（分钟，议程项）"),
            // entity.conferenceagenda.durationminutes
            new TranslationSeedItem("entity.conferenceagenda.durationminutes", "zh-HK", "计划时长分钟_hk", "计划时长（分钟，议程项）"),

            // entity.conferenceagenda.recorderid
            new TranslationSeedItem("entity.conferenceagenda.recorderid", "en-US", "记录人ID_us", "记录人 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.conferenceagenda.recorderid
            new TranslationSeedItem("entity.conferenceagenda.recorderid", "ja-JP", "记录人ID_jp", "记录人 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.conferenceagenda.recorderid
            new TranslationSeedItem("entity.conferenceagenda.recorderid", "zh-CN", "记录人ID", "记录人 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.conferenceagenda.recorderid
            new TranslationSeedItem("entity.conferenceagenda.recorderid", "zh-HK", "记录人ID_hk", "记录人 ID（选项 TaktUsers/options；DictValue=Id）"),

            // entity.conferenceagenda.recordername
            new TranslationSeedItem("entity.conferenceagenda.recordername", "en-US", "记录人姓名_us", "记录人姓名（会议纪要）"),
            // entity.conferenceagenda.recordername
            new TranslationSeedItem("entity.conferenceagenda.recordername", "ja-JP", "记录人姓名_jp", "记录人姓名（会议纪要）"),
            // entity.conferenceagenda.recordername
            new TranslationSeedItem("entity.conferenceagenda.recordername", "zh-CN", "记录人姓名", "记录人姓名（会议纪要）"),
            // entity.conferenceagenda.recordername
            new TranslationSeedItem("entity.conferenceagenda.recordername", "zh-HK", "记录人姓名_hk", "记录人姓名（会议纪要）"),

            // entity.conferenceagenda.attachments
            new TranslationSeedItem("entity.conferenceagenda.attachments", "en-US", "附件JSON_us", "附件（JSON 列表形式，由 TaktFile 统一上传到服务器）"),
            // entity.conferenceagenda.attachments
            new TranslationSeedItem("entity.conferenceagenda.attachments", "ja-JP", "附件JSON_jp", "附件（JSON 列表形式，由 TaktFile 统一上传到服务器）"),
            // entity.conferenceagenda.attachments
            new TranslationSeedItem("entity.conferenceagenda.attachments", "zh-CN", "附件JSON", "附件（JSON 列表形式，由 TaktFile 统一上传到服务器）"),
            // entity.conferenceagenda.attachments
            new TranslationSeedItem("entity.conferenceagenda.attachments", "zh-HK", "附件JSON_hk", "附件（JSON 列表形式，由 TaktFile 统一上传到服务器）"),

            // entity.conferenceagenda.isobsolete
            new TranslationSeedItem("entity.conferenceagenda.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.conferenceagenda.isobsolete
            new TranslationSeedItem("entity.conferenceagenda.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.conferenceagenda.isobsolete
            new TranslationSeedItem("entity.conferenceagenda.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.conferenceagenda.isobsolete
            new TranslationSeedItem("entity.conferenceagenda.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）"),

            // entity.conferenceagenda.conference
            new TranslationSeedItem("entity.conferenceagenda.conference", "en-US", "会议_us", "会议（主表）"),
            // entity.conferenceagenda.conference
            new TranslationSeedItem("entity.conferenceagenda.conference", "ja-JP", "会议_jp", "会议（主表）"),
            // entity.conferenceagenda.conference
            new TranslationSeedItem("entity.conferenceagenda.conference", "zh-CN", "会议", "会议（主表）"),
            // entity.conferenceagenda.conference
            new TranslationSeedItem("entity.conferenceagenda.conference", "zh-HK", "会议_hk", "会议（主表）"),
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
        translation.ResourceGroup = "ConferenceCenter";
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
