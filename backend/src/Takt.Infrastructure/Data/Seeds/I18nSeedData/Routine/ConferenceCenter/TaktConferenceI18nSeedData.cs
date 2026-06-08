// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine.ConferenceCenter
// 文件名称：TaktConferenceI18nSeedData.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktConference 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine.ConferenceCenter;

/// <summary>
/// TaktConference 实体国际化翻译种子（键前缀 entity.conference.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktConferenceI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktConference 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 conference 实体翻译...", tenantCode);

        foreach (var item in GetConferenceTranslations())
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

        TaktLogger.Information("TaktConference 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktConference 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.conference._self / entity.conference.{{field}}；ResourceGroup=TaktModule.Routine；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetConferenceTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.conference._self
            new TranslationSeedItem("entity.conference._self", "en-US", "Conference Information", "实体名称"),
            // entity.conference._self
            new TranslationSeedItem("entity.conference._self", "ja-JP", "会议中心主信息", "实体名称"),
            // entity.conference._self
            new TranslationSeedItem("entity.conference._self", "zh-CN", "会议中心主信息", "实体名称"),
            // entity.conference._self
            new TranslationSeedItem("entity.conference._self", "zh-HK", "会议中心主信息", "实体名称"),

            // entity.conference.code
            new TranslationSeedItem("entity.conference.code", "en-US", "会议编码", "会议编码（租户+公司内唯一）"),
            // entity.conference.code
            new TranslationSeedItem("entity.conference.code", "ja-JP", "会议编码", "会议编码（租户+公司内唯一）"),
            // entity.conference.code
            new TranslationSeedItem("entity.conference.code", "zh-CN", "会议编码", "会议编码（租户+公司内唯一）"),
            // entity.conference.code
            new TranslationSeedItem("entity.conference.code", "zh-HK", "会议编码", "会议编码（租户+公司内唯一）"),

            // entity.conference.title
            new TranslationSeedItem("entity.conference.title", "en-US", "会议标题", "会议标题"),
            // entity.conference.title
            new TranslationSeedItem("entity.conference.title", "ja-JP", "会议标题", "会议标题"),
            // entity.conference.title
            new TranslationSeedItem("entity.conference.title", "zh-CN", "会议标题", "会议标题"),
            // entity.conference.title
            new TranslationSeedItem("entity.conference.title", "zh-HK", "会议标题", "会议标题"),

            // entity.conference.type
            new TranslationSeedItem("entity.conference.type", "en-US", "会议类型", "会议类型"),
            // entity.conference.type
            new TranslationSeedItem("entity.conference.type", "ja-JP", "会议类型", "会议类型"),
            // entity.conference.type
            new TranslationSeedItem("entity.conference.type", "zh-CN", "会议类型", "会议类型"),
            // entity.conference.type
            new TranslationSeedItem("entity.conference.type", "zh-HK", "会议类型", "会议类型"),

            // entity.conference.status
            new TranslationSeedItem("entity.conference.status", "en-US", "会议状态", "会议状态"),
            // entity.conference.status
            new TranslationSeedItem("entity.conference.status", "ja-JP", "会议状态", "会议状态"),
            // entity.conference.status
            new TranslationSeedItem("entity.conference.status", "zh-CN", "会议状态", "会议状态"),
            // entity.conference.status
            new TranslationSeedItem("entity.conference.status", "zh-HK", "会议状态", "会议状态"),

            // entity.conference.starttime
            new TranslationSeedItem("entity.conference.starttime", "en-US", "开始时间", "开始时间"),
            // entity.conference.starttime
            new TranslationSeedItem("entity.conference.starttime", "ja-JP", "开始时间", "开始时间"),
            // entity.conference.starttime
            new TranslationSeedItem("entity.conference.starttime", "zh-CN", "开始时间", "开始时间"),
            // entity.conference.starttime
            new TranslationSeedItem("entity.conference.starttime", "zh-HK", "开始时间", "开始时间"),

            // entity.conference.endtime
            new TranslationSeedItem("entity.conference.endtime", "en-US", "结束时间", "结束时间"),
            // entity.conference.endtime
            new TranslationSeedItem("entity.conference.endtime", "ja-JP", "结束时间", "结束时间"),
            // entity.conference.endtime
            new TranslationSeedItem("entity.conference.endtime", "zh-CN", "结束时间", "结束时间"),
            // entity.conference.endtime
            new TranslationSeedItem("entity.conference.endtime", "zh-HK", "结束时间", "结束时间"),

            // entity.conference.location
            new TranslationSeedItem("entity.conference.location", "en-US", "会议地点", "会议地点（线下会议室名称或地址）"),
            // entity.conference.location
            new TranslationSeedItem("entity.conference.location", "ja-JP", "会议地点", "会议地点（线下会议室名称或地址）"),
            // entity.conference.location
            new TranslationSeedItem("entity.conference.location", "zh-CN", "会议地点", "会议地点（线下会议室名称或地址）"),
            // entity.conference.location
            new TranslationSeedItem("entity.conference.location", "zh-HK", "会议地点", "会议地点（线下会议室名称或地址）"),

            // entity.conference.meetinglink
            new TranslationSeedItem("entity.conference.meetinglink", "en-US", "会议链接", "会议链接（线上会议 URL）"),
            // entity.conference.meetinglink
            new TranslationSeedItem("entity.conference.meetinglink", "ja-JP", "会议链接", "会议链接（线上会议 URL）"),
            // entity.conference.meetinglink
            new TranslationSeedItem("entity.conference.meetinglink", "zh-CN", "会议链接", "会议链接（线上会议 URL）"),
            // entity.conference.meetinglink
            new TranslationSeedItem("entity.conference.meetinglink", "zh-HK", "会议链接", "会议链接（线上会议 URL）"),

            // entity.conference.agenda
            new TranslationSeedItem("entity.conference.agenda", "en-US", "会议议程", "会议议程"),
            // entity.conference.agenda
            new TranslationSeedItem("entity.conference.agenda", "ja-JP", "会议议程", "会议议程"),
            // entity.conference.agenda
            new TranslationSeedItem("entity.conference.agenda", "zh-CN", "会议议程", "会议议程"),
            // entity.conference.agenda
            new TranslationSeedItem("entity.conference.agenda", "zh-HK", "会议议程", "会议议程"),

            // entity.conference.content
            new TranslationSeedItem("entity.conference.content", "en-US", "会议内容", "会议内容（会议纪要正文，富文本 HTML）"),
            // entity.conference.content
            new TranslationSeedItem("entity.conference.content", "ja-JP", "会议内容", "会议内容（会议纪要正文，富文本 HTML）"),
            // entity.conference.content
            new TranslationSeedItem("entity.conference.content", "zh-CN", "会议内容", "会议内容（会议纪要正文，富文本 HTML）"),
            // entity.conference.content
            new TranslationSeedItem("entity.conference.content", "zh-HK", "会议内容", "会议内容（会议纪要正文，富文本 HTML）"),

            // entity.conference.summary
            new TranslationSeedItem("entity.conference.summary", "en-US", "会议纪要摘要", "会议纪要摘要（用于列表展示）"),
            // entity.conference.summary
            new TranslationSeedItem("entity.conference.summary", "ja-JP", "会议纪要摘要", "会议纪要摘要（用于列表展示）"),
            // entity.conference.summary
            new TranslationSeedItem("entity.conference.summary", "zh-CN", "会议纪要摘要", "会议纪要摘要（用于列表展示）"),
            // entity.conference.summary
            new TranslationSeedItem("entity.conference.summary", "zh-HK", "会议纪要摘要", "会议纪要摘要（用于列表展示）"),

            // entity.conference.tags
            new TranslationSeedItem("entity.conference.tags", "en-US", "标签", "标签（逗号分隔或 JSON 数组存储）"),
            // entity.conference.tags
            new TranslationSeedItem("entity.conference.tags", "ja-JP", "标签", "标签（逗号分隔或 JSON 数组存储）"),
            // entity.conference.tags
            new TranslationSeedItem("entity.conference.tags", "zh-CN", "标签", "标签（逗号分隔或 JSON 数组存储）"),
            // entity.conference.tags
            new TranslationSeedItem("entity.conference.tags", "zh-HK", "标签", "标签（逗号分隔或 JSON 数组存储）"),

            // entity.conference.organizerid
            new TranslationSeedItem("entity.conference.organizerid", "en-US", "组织人ID", "组织人 ID"),
            // entity.conference.organizerid
            new TranslationSeedItem("entity.conference.organizerid", "ja-JP", "组织人ID", "组织人 ID"),
            // entity.conference.organizerid
            new TranslationSeedItem("entity.conference.organizerid", "zh-CN", "组织人ID", "组织人 ID"),
            // entity.conference.organizerid
            new TranslationSeedItem("entity.conference.organizerid", "zh-HK", "组织人ID", "组织人 ID"),

            // entity.conference.organizername
            new TranslationSeedItem("entity.conference.organizername", "en-US", "组织人姓名", "组织人姓名"),
            // entity.conference.organizername
            new TranslationSeedItem("entity.conference.organizername", "ja-JP", "组织人姓名", "组织人姓名"),
            // entity.conference.organizername
            new TranslationSeedItem("entity.conference.organizername", "zh-CN", "组织人姓名", "组织人姓名"),
            // entity.conference.organizername
            new TranslationSeedItem("entity.conference.organizername", "zh-HK", "组织人姓名", "组织人姓名"),

            // entity.conference.deptid
            new TranslationSeedItem("entity.conference.deptid", "en-US", "主办部门ID", "主办部门 ID"),
            // entity.conference.deptid
            new TranslationSeedItem("entity.conference.deptid", "ja-JP", "主办部门ID", "主办部门 ID"),
            // entity.conference.deptid
            new TranslationSeedItem("entity.conference.deptid", "zh-CN", "主办部门ID", "主办部门 ID"),
            // entity.conference.deptid
            new TranslationSeedItem("entity.conference.deptid", "zh-HK", "主办部门ID", "主办部门 ID"),

            // entity.conference.deptname
            new TranslationSeedItem("entity.conference.deptname", "en-US", "主办部门名称", "主办部门名称"),
            // entity.conference.deptname
            new TranslationSeedItem("entity.conference.deptname", "ja-JP", "主办部门名称", "主办部门名称"),
            // entity.conference.deptname
            new TranslationSeedItem("entity.conference.deptname", "zh-CN", "主办部门名称", "主办部门名称"),
            // entity.conference.deptname
            new TranslationSeedItem("entity.conference.deptname", "zh-HK", "主办部门名称", "主办部门名称"),

            // entity.conference.maxparticipants
            new TranslationSeedItem("entity.conference.maxparticipants", "en-US", "最大参会人数", "最大参会人数（0 表示不限）"),
            // entity.conference.maxparticipants
            new TranslationSeedItem("entity.conference.maxparticipants", "ja-JP", "最大参会人数", "最大参会人数（0 表示不限）"),
            // entity.conference.maxparticipants
            new TranslationSeedItem("entity.conference.maxparticipants", "zh-CN", "最大参会人数", "最大参会人数（0 表示不限）"),
            // entity.conference.maxparticipants
            new TranslationSeedItem("entity.conference.maxparticipants", "zh-HK", "最大参会人数", "最大参会人数（0 表示不限）"),

            // entity.conference.reminderminutes
            new TranslationSeedItem("entity.conference.reminderminutes", "en-US", "提前提醒分钟数", "提前提醒分钟数（0 表示不提醒）"),
            // entity.conference.reminderminutes
            new TranslationSeedItem("entity.conference.reminderminutes", "ja-JP", "提前提醒分钟数", "提前提醒分钟数（0 表示不提醒）"),
            // entity.conference.reminderminutes
            new TranslationSeedItem("entity.conference.reminderminutes", "zh-CN", "提前提醒分钟数", "提前提醒分钟数（0 表示不提醒）"),
            // entity.conference.reminderminutes
            new TranslationSeedItem("entity.conference.reminderminutes", "zh-HK", "提前提醒分钟数", "提前提醒分钟数（0 表示不提醒）"),

            // entity.conference.flowinstanceid
            new TranslationSeedItem("entity.conference.flowinstanceid", "en-US", "流程实例ID", "流程实例 ID（会议审批工作流）"),
            // entity.conference.flowinstanceid
            new TranslationSeedItem("entity.conference.flowinstanceid", "ja-JP", "流程实例ID", "流程实例 ID（会议审批工作流）"),
            // entity.conference.flowinstanceid
            new TranslationSeedItem("entity.conference.flowinstanceid", "zh-CN", "流程实例ID", "流程实例 ID（会议审批工作流）"),
            // entity.conference.flowinstanceid
            new TranslationSeedItem("entity.conference.flowinstanceid", "zh-HK", "流程实例ID", "流程实例 ID（会议审批工作流）"),

            // entity.conference.participants
            new TranslationSeedItem("entity.conference.participants", "en-US", "参与人列表", "参与人列表（主子表关系）"),
            // entity.conference.participants
            new TranslationSeedItem("entity.conference.participants", "ja-JP", "参与人列表", "参与人列表（主子表关系）"),
            // entity.conference.participants
            new TranslationSeedItem("entity.conference.participants", "zh-CN", "参与人列表", "参与人列表（主子表关系）"),
            // entity.conference.participants
            new TranslationSeedItem("entity.conference.participants", "zh-HK", "参与人列表", "参与人列表（主子表关系）"),
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
