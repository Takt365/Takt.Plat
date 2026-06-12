// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Cost
// 文件名称：TaktQualityFailureMeetingI18nSeedData.cs
// 创建时间：2026-06-12
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktQualityFailureMeeting 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Cost;

/// <summary>
/// TaktQualityFailureMeeting 实体国际化翻译种子（键前缀 entity.qualityfailuremeeting.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktQualityFailureMeetingI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktQualityFailureMeeting 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 qualityfailuremeeting 实体翻译...", tenantCode);

        foreach (var item in GetQualityFailureMeetingTranslations())
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

        TaktLogger.Information("TaktQualityFailureMeeting 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktQualityFailureMeeting 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.qualityfailuremeeting._self / entity.qualityfailuremeeting.{{field}}；ResourceGroup=4；ResourceType=0
    /// </summary>
    private static List<TranslationSeedItem> GetQualityFailureMeetingTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.qualityfailuremeeting._self
            new TranslationSeedItem("entity.qualityfailuremeeting._self", "en-US", "Quality Failure Meeting Information", "实体名称"),
            // entity.qualityfailuremeeting._self
            new TranslationSeedItem("entity.qualityfailuremeeting._self", "ja-JP", "品质问题应对明细 - 会议/调查/试验费用信息", "实体名称"),
            // entity.qualityfailuremeeting._self
            new TranslationSeedItem("entity.qualityfailuremeeting._self", "zh-CN", "品质问题应对明细 - 会议/调查/试验费用信息", "实体名称"),
            // entity.qualityfailuremeeting._self
            new TranslationSeedItem("entity.qualityfailuremeeting._self", "zh-HK", "品质问题应对明细 - 会议/调查/试验费用信息", "实体名称"),

            // entity.qualityfailuremeeting.qualityfailureid
            new TranslationSeedItem("entity.qualityfailuremeeting.qualityfailureid", "en-US", "品质问题主表ID", "品质问题主表ID(主子表关系,序列化为string以避免Javascript精度问题)"),
            // entity.qualityfailuremeeting.qualityfailureid
            new TranslationSeedItem("entity.qualityfailuremeeting.qualityfailureid", "ja-JP", "品质问题主表ID", "品质问题主表ID(主子表关系,序列化为string以避免Javascript精度问题)"),
            // entity.qualityfailuremeeting.qualityfailureid
            new TranslationSeedItem("entity.qualityfailuremeeting.qualityfailureid", "zh-CN", "品质问题主表ID", "品质问题主表ID(主子表关系,序列化为string以避免Javascript精度问题)"),
            // entity.qualityfailuremeeting.qualityfailureid
            new TranslationSeedItem("entity.qualityfailuremeeting.qualityfailureid", "zh-HK", "品质问题主表ID", "品质问题主表ID(主子表关系,序列化为string以避免Javascript精度问题)"),

            // entity.qualityfailuremeeting.qualityfailurecode
            new TranslationSeedItem("entity.qualityfailuremeeting.qualityfailurecode", "en-US", "品质问题编码", "品质问题编码（冗余字段，便于查询）"),
            // entity.qualityfailuremeeting.qualityfailurecode
            new TranslationSeedItem("entity.qualityfailuremeeting.qualityfailurecode", "ja-JP", "品质问题编码", "品质问题编码（冗余字段，便于查询）"),
            // entity.qualityfailuremeeting.qualityfailurecode
            new TranslationSeedItem("entity.qualityfailuremeeting.qualityfailurecode", "zh-CN", "品质问题编码", "品质问题编码（冗余字段，便于查询）"),
            // entity.qualityfailuremeeting.qualityfailurecode
            new TranslationSeedItem("entity.qualityfailuremeeting.qualityfailurecode", "zh-HK", "品质问题编码", "品质问题编码（冗余字段，便于查询）"),

            // entity.qualityfailuremeeting.linenumber
            new TranslationSeedItem("entity.qualityfailuremeeting.linenumber", "en-US", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.qualityfailuremeeting.linenumber
            new TranslationSeedItem("entity.qualityfailuremeeting.linenumber", "ja-JP", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.qualityfailuremeeting.linenumber
            new TranslationSeedItem("entity.qualityfailuremeeting.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.qualityfailuremeeting.linenumber
            new TranslationSeedItem("entity.qualityfailuremeeting.linenumber", "zh-HK", "行号", "行号（项号/序号，固定步长=10）"),

            // entity.qualityfailuremeeting.directmanpowercostperminute
            new TranslationSeedItem("entity.qualityfailuremeeting.directmanpowercostperminute", "en-US", "直接人员费率", "直接人员费率（元/分钟）"),
            // entity.qualityfailuremeeting.directmanpowercostperminute
            new TranslationSeedItem("entity.qualityfailuremeeting.directmanpowercostperminute", "ja-JP", "直接人员费率", "直接人员费率（元/分钟）"),
            // entity.qualityfailuremeeting.directmanpowercostperminute
            new TranslationSeedItem("entity.qualityfailuremeeting.directmanpowercostperminute", "zh-CN", "直接人员费率", "直接人员费率（元/分钟）"),
            // entity.qualityfailuremeeting.directmanpowercostperminute
            new TranslationSeedItem("entity.qualityfailuremeeting.directmanpowercostperminute", "zh-HK", "直接人员费率", "直接人员费率（元/分钟）"),

            // entity.qualityfailuremeeting.indirectmanpowercostperminute
            new TranslationSeedItem("entity.qualityfailuremeeting.indirectmanpowercostperminute", "en-US", "间接人员费率", "间接人员费率（元/分钟）"),
            // entity.qualityfailuremeeting.indirectmanpowercostperminute
            new TranslationSeedItem("entity.qualityfailuremeeting.indirectmanpowercostperminute", "ja-JP", "间接人员费率", "间接人员费率（元/分钟）"),
            // entity.qualityfailuremeeting.indirectmanpowercostperminute
            new TranslationSeedItem("entity.qualityfailuremeeting.indirectmanpowercostperminute", "zh-CN", "间接人员费率", "间接人员费率（元/分钟）"),
            // entity.qualityfailuremeeting.indirectmanpowercostperminute
            new TranslationSeedItem("entity.qualityfailuremeeting.indirectmanpowercostperminute", "zh-HK", "间接人员费率", "间接人员费率（元/分钟）"),

            // entity.qualityfailuremeeting.meetinginvestigationcontent
            new TranslationSeedItem("entity.qualityfailuremeeting.meetinginvestigationcontent", "en-US", "讨论调查试验内容", "讨论调查试验内容(会议记录)"),
            // entity.qualityfailuremeeting.meetinginvestigationcontent
            new TranslationSeedItem("entity.qualityfailuremeeting.meetinginvestigationcontent", "ja-JP", "讨论调查试验内容", "讨论调查试验内容(会议记录)"),
            // entity.qualityfailuremeeting.meetinginvestigationcontent
            new TranslationSeedItem("entity.qualityfailuremeeting.meetinginvestigationcontent", "zh-CN", "讨论调查试验内容", "讨论调查试验内容(会议记录)"),
            // entity.qualityfailuremeeting.meetinginvestigationcontent
            new TranslationSeedItem("entity.qualityfailuremeeting.meetinginvestigationcontent", "zh-HK", "讨论调查试验内容", "讨论调查试验内容(会议记录)"),

            // entity.qualityfailuremeeting.meetinginvestigationcost
            new TranslationSeedItem("entity.qualityfailuremeeting.meetinginvestigationcost", "en-US", "讨论调查试验费用", "讨论调查试验费用(元)"),
            // entity.qualityfailuremeeting.meetinginvestigationcost
            new TranslationSeedItem("entity.qualityfailuremeeting.meetinginvestigationcost", "ja-JP", "讨论调查试验费用", "讨论调查试验费用(元)"),
            // entity.qualityfailuremeeting.meetinginvestigationcost
            new TranslationSeedItem("entity.qualityfailuremeeting.meetinginvestigationcost", "zh-CN", "讨论调查试验费用", "讨论调查试验费用(元)"),
            // entity.qualityfailuremeeting.meetinginvestigationcost
            new TranslationSeedItem("entity.qualityfailuremeeting.meetinginvestigationcost", "zh-HK", "讨论调查试验费用", "讨论调查试验费用(元)"),

            // entity.qualityfailuremeeting.meetingtimeminutes
            new TranslationSeedItem("entity.qualityfailuremeeting.meetingtimeminutes", "en-US", "检讨会使用时间", "讨论会使用时间(分钟)"),
            // entity.qualityfailuremeeting.meetingtimeminutes
            new TranslationSeedItem("entity.qualityfailuremeeting.meetingtimeminutes", "ja-JP", "检讨会使用时间", "讨论会使用时间(分钟)"),
            // entity.qualityfailuremeeting.meetingtimeminutes
            new TranslationSeedItem("entity.qualityfailuremeeting.meetingtimeminutes", "zh-CN", "检讨会使用时间", "讨论会使用时间(分钟)"),
            // entity.qualityfailuremeeting.meetingtimeminutes
            new TranslationSeedItem("entity.qualityfailuremeeting.meetingtimeminutes", "zh-HK", "检讨会使用时间", "讨论会使用时间(分钟)"),

            // entity.qualityfailuremeeting.directparticipantcount
            new TranslationSeedItem("entity.qualityfailuremeeting.directparticipantcount", "en-US", "直接人员参加人数", "直接人员参加人数"),
            // entity.qualityfailuremeeting.directparticipantcount
            new TranslationSeedItem("entity.qualityfailuremeeting.directparticipantcount", "ja-JP", "直接人员参加人数", "直接人员参加人数"),
            // entity.qualityfailuremeeting.directparticipantcount
            new TranslationSeedItem("entity.qualityfailuremeeting.directparticipantcount", "zh-CN", "直接人员参加人数", "直接人员参加人数"),
            // entity.qualityfailuremeeting.directparticipantcount
            new TranslationSeedItem("entity.qualityfailuremeeting.directparticipantcount", "zh-HK", "直接人员参加人数", "直接人员参加人数"),

            // entity.qualityfailuremeeting.indirectparticipantcount
            new TranslationSeedItem("entity.qualityfailuremeeting.indirectparticipantcount", "en-US", "间接人员参加人数", "间接人员参加人数"),
            // entity.qualityfailuremeeting.indirectparticipantcount
            new TranslationSeedItem("entity.qualityfailuremeeting.indirectparticipantcount", "ja-JP", "间接人员参加人数", "间接人员参加人数"),
            // entity.qualityfailuremeeting.indirectparticipantcount
            new TranslationSeedItem("entity.qualityfailuremeeting.indirectparticipantcount", "zh-CN", "间接人员参加人数", "间接人员参加人数"),
            // entity.qualityfailuremeeting.indirectparticipantcount
            new TranslationSeedItem("entity.qualityfailuremeeting.indirectparticipantcount", "zh-HK", "间接人员参加人数", "间接人员参加人数"),

            // entity.qualityfailuremeeting.investigationworktimeminutes
            new TranslationSeedItem("entity.qualityfailuremeeting.investigationworktimeminutes", "en-US", "调查评价试验工作时间", "调查评价试验工作时间（分钟）"),
            // entity.qualityfailuremeeting.investigationworktimeminutes
            new TranslationSeedItem("entity.qualityfailuremeeting.investigationworktimeminutes", "ja-JP", "调查评价试验工作时间", "调查评价试验工作时间（分钟）"),
            // entity.qualityfailuremeeting.investigationworktimeminutes
            new TranslationSeedItem("entity.qualityfailuremeeting.investigationworktimeminutes", "zh-CN", "调查评价试验工作时间", "调查评价试验工作时间（分钟）"),
            // entity.qualityfailuremeeting.investigationworktimeminutes
            new TranslationSeedItem("entity.qualityfailuremeeting.investigationworktimeminutes", "zh-HK", "调查评价试验工作时间", "调查评价试验工作时间（分钟）"),

            // entity.qualityfailuremeeting.travelcost
            new TranslationSeedItem("entity.qualityfailuremeeting.travelcost", "en-US", "交通费旅费", "交通费、旅费（元）"),
            // entity.qualityfailuremeeting.travelcost
            new TranslationSeedItem("entity.qualityfailuremeeting.travelcost", "ja-JP", "交通费旅费", "交通费、旅费（元）"),
            // entity.qualityfailuremeeting.travelcost
            new TranslationSeedItem("entity.qualityfailuremeeting.travelcost", "zh-CN", "交通费旅费", "交通费、旅费（元）"),
            // entity.qualityfailuremeeting.travelcost
            new TranslationSeedItem("entity.qualityfailuremeeting.travelcost", "zh-HK", "交通费旅费", "交通费、旅费（元）"),

            // entity.qualityfailuremeeting.otherexpenses
            new TranslationSeedItem("entity.qualityfailuremeeting.otherexpenses", "en-US", "其他费用", "其他费用（元）"),
            // entity.qualityfailuremeeting.otherexpenses
            new TranslationSeedItem("entity.qualityfailuremeeting.otherexpenses", "ja-JP", "其他费用", "其他费用（元）"),
            // entity.qualityfailuremeeting.otherexpenses
            new TranslationSeedItem("entity.qualityfailuremeeting.otherexpenses", "zh-CN", "其他费用", "其他费用（元）"),
            // entity.qualityfailuremeeting.otherexpenses
            new TranslationSeedItem("entity.qualityfailuremeeting.otherexpenses", "zh-HK", "其他费用", "其他费用（元）"),

            // entity.qualityfailuremeeting.otherworktimeminutes
            new TranslationSeedItem("entity.qualityfailuremeeting.otherworktimeminutes", "en-US", "其他作业时间", "其他作业時間（分钟）"),
            // entity.qualityfailuremeeting.otherworktimeminutes
            new TranslationSeedItem("entity.qualityfailuremeeting.otherworktimeminutes", "ja-JP", "其他作业时间", "其他作业時間（分钟）"),
            // entity.qualityfailuremeeting.otherworktimeminutes
            new TranslationSeedItem("entity.qualityfailuremeeting.otherworktimeminutes", "zh-CN", "其他作业时间", "其他作业時間（分钟）"),
            // entity.qualityfailuremeeting.otherworktimeminutes
            new TranslationSeedItem("entity.qualityfailuremeeting.otherworktimeminutes", "zh-HK", "其他作业时间", "其他作业時間（分钟）"),

            // entity.qualityfailuremeeting.otherapparatuscost
            new TranslationSeedItem("entity.qualityfailuremeeting.otherapparatuscost", "en-US", "其他设备工程搬运费", "其他设备购入费、工程费、搬运费等（元）"),
            // entity.qualityfailuremeeting.otherapparatuscost
            new TranslationSeedItem("entity.qualityfailuremeeting.otherapparatuscost", "ja-JP", "其他设备工程搬运费", "其他设备购入费、工程费、搬运费等（元）"),
            // entity.qualityfailuremeeting.otherapparatuscost
            new TranslationSeedItem("entity.qualityfailuremeeting.otherapparatuscost", "zh-CN", "其他设备工程搬运费", "其他设备购入费、工程费、搬运费等（元）"),
            // entity.qualityfailuremeeting.otherapparatuscost
            new TranslationSeedItem("entity.qualityfailuremeeting.otherapparatuscost", "zh-HK", "其他设备工程搬运费", "其他设备购入费、工程费、搬运费等（元）"),

            // entity.qualityfailuremeeting.meetingrecorder
            new TranslationSeedItem("entity.qualityfailuremeeting.meetingrecorder", "en-US", "品质问题对应记录者", "品质问题対応记录者（会议调查试验记录者）"),
            // entity.qualityfailuremeeting.meetingrecorder
            new TranslationSeedItem("entity.qualityfailuremeeting.meetingrecorder", "ja-JP", "品质问题对应记录者", "品质问题対応记录者（会议调查试验记录者）"),
            // entity.qualityfailuremeeting.meetingrecorder
            new TranslationSeedItem("entity.qualityfailuremeeting.meetingrecorder", "zh-CN", "品质问题对应记录者", "品质问题対応记录者（会议调查试验记录者）"),
            // entity.qualityfailuremeeting.meetingrecorder
            new TranslationSeedItem("entity.qualityfailuremeeting.meetingrecorder", "zh-HK", "品质问题对应记录者", "品质问题対応记录者（会议调查试验记录者）"),

            // entity.qualityfailuremeeting.issue
            new TranslationSeedItem("entity.qualityfailuremeeting.issue", "en-US", "质量问题主表", "质量问题主表（导航属性）"),
            // entity.qualityfailuremeeting.issue
            new TranslationSeedItem("entity.qualityfailuremeeting.issue", "ja-JP", "质量问题主表", "质量问题主表（导航属性）"),
            // entity.qualityfailuremeeting.issue
            new TranslationSeedItem("entity.qualityfailuremeeting.issue", "zh-CN", "质量问题主表", "质量问题主表（导航属性）"),
            // entity.qualityfailuremeeting.issue
            new TranslationSeedItem("entity.qualityfailuremeeting.issue", "zh-HK", "质量问题主表", "质量问题主表（导航属性）"),
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
        translation.ResourceGroup = 4;
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
