// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Cost
// 文件名称：TaktQualityIssueMeetingI18nSeedData.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktQualityIssueMeeting 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Cost;

/// <summary>
/// TaktQualityIssueMeeting 实体国际化翻译种子（键前缀 entity.qualityIssueMeeting.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktQualityIssueMeetingI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktQualityIssueMeeting 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 qualityIssueMeeting 实体翻译...", tenantCode);

        foreach (var item in GetQualityIssueMeetingTranslations())
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

        TaktLogger.Information("TaktQualityIssueMeeting 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktQualityIssueMeeting 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.qualityIssueMeeting._self / entity.qualityIssueMeeting.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetQualityIssueMeetingTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.qualityIssueMeeting._self
            new TranslationSeedItem("entity.qualityIssueMeeting._self", "en-US", "Quality Issue Meeting Information", "实体名称"),
            // entity.qualityIssueMeeting._self
            new TranslationSeedItem("entity.qualityIssueMeeting._self", "ja-JP", "品质问题应对明细 - 会议/调查/试验费用信息", "实体名称"),
            // entity.qualityIssueMeeting._self
            new TranslationSeedItem("entity.qualityIssueMeeting._self", "zh-CN", "品质问题应对明细 - 会议/调查/试验费用信息", "实体名称"),
            // entity.qualityIssueMeeting._self
            new TranslationSeedItem("entity.qualityIssueMeeting._self", "zh-HK", "品质问题应对明细 - 会议/调查/试验费用信息", "实体名称"),

            // entity.qualityIssueMeeting.qualityissueid
            new TranslationSeedItem("entity.qualityIssueMeeting.qualityissueid", "en-US", "品质问题主表ID", "品质问题主表ID(主子表关系,序列化为string以避免Javascript精度问题)"),
            // entity.qualityIssueMeeting.qualityissueid
            new TranslationSeedItem("entity.qualityIssueMeeting.qualityissueid", "ja-JP", "品质问题主表ID", "品质问题主表ID(主子表关系,序列化为string以避免Javascript精度问题)"),
            // entity.qualityIssueMeeting.qualityissueid
            new TranslationSeedItem("entity.qualityIssueMeeting.qualityissueid", "zh-CN", "品质问题主表ID", "品质问题主表ID(主子表关系,序列化为string以避免Javascript精度问题)"),
            // entity.qualityIssueMeeting.qualityissueid
            new TranslationSeedItem("entity.qualityIssueMeeting.qualityissueid", "zh-HK", "品质问题主表ID", "品质问题主表ID(主子表关系,序列化为string以避免Javascript精度问题)"),

            // entity.qualityIssueMeeting.qualityissuecode
            new TranslationSeedItem("entity.qualityIssueMeeting.qualityissuecode", "en-US", "品质问题编码", "品质问题编码（冗余字段，便于查询）"),
            // entity.qualityIssueMeeting.qualityissuecode
            new TranslationSeedItem("entity.qualityIssueMeeting.qualityissuecode", "ja-JP", "品质问题编码", "品质问题编码（冗余字段，便于查询）"),
            // entity.qualityIssueMeeting.qualityissuecode
            new TranslationSeedItem("entity.qualityIssueMeeting.qualityissuecode", "zh-CN", "品质问题编码", "品质问题编码（冗余字段，便于查询）"),
            // entity.qualityIssueMeeting.qualityissuecode
            new TranslationSeedItem("entity.qualityIssueMeeting.qualityissuecode", "zh-HK", "品质问题编码", "品质问题编码（冗余字段，便于查询）"),

            // entity.qualityIssueMeeting.linenumber
            new TranslationSeedItem("entity.qualityIssueMeeting.linenumber", "en-US", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.qualityIssueMeeting.linenumber
            new TranslationSeedItem("entity.qualityIssueMeeting.linenumber", "ja-JP", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.qualityIssueMeeting.linenumber
            new TranslationSeedItem("entity.qualityIssueMeeting.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.qualityIssueMeeting.linenumber
            new TranslationSeedItem("entity.qualityIssueMeeting.linenumber", "zh-HK", "行号", "行号（项号/序号，固定步长=10）"),

            // entity.qualityIssueMeeting.directmanpowercostperminute
            new TranslationSeedItem("entity.qualityIssueMeeting.directmanpowercostperminute", "en-US", "直接人员费率", "直接人员费率（元/分钟）"),
            // entity.qualityIssueMeeting.directmanpowercostperminute
            new TranslationSeedItem("entity.qualityIssueMeeting.directmanpowercostperminute", "ja-JP", "直接人员费率", "直接人员费率（元/分钟）"),
            // entity.qualityIssueMeeting.directmanpowercostperminute
            new TranslationSeedItem("entity.qualityIssueMeeting.directmanpowercostperminute", "zh-CN", "直接人员费率", "直接人员费率（元/分钟）"),
            // entity.qualityIssueMeeting.directmanpowercostperminute
            new TranslationSeedItem("entity.qualityIssueMeeting.directmanpowercostperminute", "zh-HK", "直接人员费率", "直接人员费率（元/分钟）"),

            // entity.qualityIssueMeeting.indirectmanpowercostperminute
            new TranslationSeedItem("entity.qualityIssueMeeting.indirectmanpowercostperminute", "en-US", "间接人员费率", "间接人员费率（元/分钟）"),
            // entity.qualityIssueMeeting.indirectmanpowercostperminute
            new TranslationSeedItem("entity.qualityIssueMeeting.indirectmanpowercostperminute", "ja-JP", "间接人员费率", "间接人员费率（元/分钟）"),
            // entity.qualityIssueMeeting.indirectmanpowercostperminute
            new TranslationSeedItem("entity.qualityIssueMeeting.indirectmanpowercostperminute", "zh-CN", "间接人员费率", "间接人员费率（元/分钟）"),
            // entity.qualityIssueMeeting.indirectmanpowercostperminute
            new TranslationSeedItem("entity.qualityIssueMeeting.indirectmanpowercostperminute", "zh-HK", "间接人员费率", "间接人员费率（元/分钟）"),

            // entity.qualityIssueMeeting.meetinginvestigationcontent
            new TranslationSeedItem("entity.qualityIssueMeeting.meetinginvestigationcontent", "en-US", "讨论调查试验内容", "讨论调查试验内容(会议记录)"),
            // entity.qualityIssueMeeting.meetinginvestigationcontent
            new TranslationSeedItem("entity.qualityIssueMeeting.meetinginvestigationcontent", "ja-JP", "讨论调查试验内容", "讨论调查试验内容(会议记录)"),
            // entity.qualityIssueMeeting.meetinginvestigationcontent
            new TranslationSeedItem("entity.qualityIssueMeeting.meetinginvestigationcontent", "zh-CN", "讨论调查试验内容", "讨论调查试验内容(会议记录)"),
            // entity.qualityIssueMeeting.meetinginvestigationcontent
            new TranslationSeedItem("entity.qualityIssueMeeting.meetinginvestigationcontent", "zh-HK", "讨论调查试验内容", "讨论调查试验内容(会议记录)"),

            // entity.qualityIssueMeeting.meetinginvestigationcost
            new TranslationSeedItem("entity.qualityIssueMeeting.meetinginvestigationcost", "en-US", "讨论调查试验费用", "讨论调查试验费用(元)"),
            // entity.qualityIssueMeeting.meetinginvestigationcost
            new TranslationSeedItem("entity.qualityIssueMeeting.meetinginvestigationcost", "ja-JP", "讨论调查试验费用", "讨论调查试验费用(元)"),
            // entity.qualityIssueMeeting.meetinginvestigationcost
            new TranslationSeedItem("entity.qualityIssueMeeting.meetinginvestigationcost", "zh-CN", "讨论调查试验费用", "讨论调查试验费用(元)"),
            // entity.qualityIssueMeeting.meetinginvestigationcost
            new TranslationSeedItem("entity.qualityIssueMeeting.meetinginvestigationcost", "zh-HK", "讨论调查试验费用", "讨论调查试验费用(元)"),

            // entity.qualityIssueMeeting.meetingtimeminutes
            new TranslationSeedItem("entity.qualityIssueMeeting.meetingtimeminutes", "en-US", "检讨会使用时间", "讨论会使用时间(分钟)"),
            // entity.qualityIssueMeeting.meetingtimeminutes
            new TranslationSeedItem("entity.qualityIssueMeeting.meetingtimeminutes", "ja-JP", "检讨会使用时间", "讨论会使用时间(分钟)"),
            // entity.qualityIssueMeeting.meetingtimeminutes
            new TranslationSeedItem("entity.qualityIssueMeeting.meetingtimeminutes", "zh-CN", "检讨会使用时间", "讨论会使用时间(分钟)"),
            // entity.qualityIssueMeeting.meetingtimeminutes
            new TranslationSeedItem("entity.qualityIssueMeeting.meetingtimeminutes", "zh-HK", "检讨会使用时间", "讨论会使用时间(分钟)"),

            // entity.qualityIssueMeeting.directparticipantcount
            new TranslationSeedItem("entity.qualityIssueMeeting.directparticipantcount", "en-US", "直接人员参加人数", "直接人员参加人数"),
            // entity.qualityIssueMeeting.directparticipantcount
            new TranslationSeedItem("entity.qualityIssueMeeting.directparticipantcount", "ja-JP", "直接人员参加人数", "直接人员参加人数"),
            // entity.qualityIssueMeeting.directparticipantcount
            new TranslationSeedItem("entity.qualityIssueMeeting.directparticipantcount", "zh-CN", "直接人员参加人数", "直接人员参加人数"),
            // entity.qualityIssueMeeting.directparticipantcount
            new TranslationSeedItem("entity.qualityIssueMeeting.directparticipantcount", "zh-HK", "直接人员参加人数", "直接人员参加人数"),

            // entity.qualityIssueMeeting.indirectparticipantcount
            new TranslationSeedItem("entity.qualityIssueMeeting.indirectparticipantcount", "en-US", "间接人员参加人数", "间接人员参加人数"),
            // entity.qualityIssueMeeting.indirectparticipantcount
            new TranslationSeedItem("entity.qualityIssueMeeting.indirectparticipantcount", "ja-JP", "间接人员参加人数", "间接人员参加人数"),
            // entity.qualityIssueMeeting.indirectparticipantcount
            new TranslationSeedItem("entity.qualityIssueMeeting.indirectparticipantcount", "zh-CN", "间接人员参加人数", "间接人员参加人数"),
            // entity.qualityIssueMeeting.indirectparticipantcount
            new TranslationSeedItem("entity.qualityIssueMeeting.indirectparticipantcount", "zh-HK", "间接人员参加人数", "间接人员参加人数"),

            // entity.qualityIssueMeeting.investigationworktimeminutes
            new TranslationSeedItem("entity.qualityIssueMeeting.investigationworktimeminutes", "en-US", "调查评价试验工作时间", "调查评价试验工作时间（分钟）"),
            // entity.qualityIssueMeeting.investigationworktimeminutes
            new TranslationSeedItem("entity.qualityIssueMeeting.investigationworktimeminutes", "ja-JP", "调查评价试验工作时间", "调查评价试验工作时间（分钟）"),
            // entity.qualityIssueMeeting.investigationworktimeminutes
            new TranslationSeedItem("entity.qualityIssueMeeting.investigationworktimeminutes", "zh-CN", "调查评价试验工作时间", "调查评价试验工作时间（分钟）"),
            // entity.qualityIssueMeeting.investigationworktimeminutes
            new TranslationSeedItem("entity.qualityIssueMeeting.investigationworktimeminutes", "zh-HK", "调查评价试验工作时间", "调查评价试验工作时间（分钟）"),

            // entity.qualityIssueMeeting.travelcost
            new TranslationSeedItem("entity.qualityIssueMeeting.travelcost", "en-US", "交通费旅费", "交通费、旅费（元）"),
            // entity.qualityIssueMeeting.travelcost
            new TranslationSeedItem("entity.qualityIssueMeeting.travelcost", "ja-JP", "交通费旅费", "交通费、旅费（元）"),
            // entity.qualityIssueMeeting.travelcost
            new TranslationSeedItem("entity.qualityIssueMeeting.travelcost", "zh-CN", "交通费旅费", "交通费、旅费（元）"),
            // entity.qualityIssueMeeting.travelcost
            new TranslationSeedItem("entity.qualityIssueMeeting.travelcost", "zh-HK", "交通费旅费", "交通费、旅费（元）"),

            // entity.qualityIssueMeeting.otherexpenses
            new TranslationSeedItem("entity.qualityIssueMeeting.otherexpenses", "en-US", "其他费用", "其他费用（元）"),
            // entity.qualityIssueMeeting.otherexpenses
            new TranslationSeedItem("entity.qualityIssueMeeting.otherexpenses", "ja-JP", "其他费用", "其他费用（元）"),
            // entity.qualityIssueMeeting.otherexpenses
            new TranslationSeedItem("entity.qualityIssueMeeting.otherexpenses", "zh-CN", "其他费用", "其他费用（元）"),
            // entity.qualityIssueMeeting.otherexpenses
            new TranslationSeedItem("entity.qualityIssueMeeting.otherexpenses", "zh-HK", "其他费用", "其他费用（元）"),

            // entity.qualityIssueMeeting.otherworktimeminutes
            new TranslationSeedItem("entity.qualityIssueMeeting.otherworktimeminutes", "en-US", "其他作业时间", "其他作业時間（分钟）"),
            // entity.qualityIssueMeeting.otherworktimeminutes
            new TranslationSeedItem("entity.qualityIssueMeeting.otherworktimeminutes", "ja-JP", "其他作业时间", "其他作业時間（分钟）"),
            // entity.qualityIssueMeeting.otherworktimeminutes
            new TranslationSeedItem("entity.qualityIssueMeeting.otherworktimeminutes", "zh-CN", "其他作业时间", "其他作业時間（分钟）"),
            // entity.qualityIssueMeeting.otherworktimeminutes
            new TranslationSeedItem("entity.qualityIssueMeeting.otherworktimeminutes", "zh-HK", "其他作业时间", "其他作业時間（分钟）"),

            // entity.qualityIssueMeeting.otherapparatuscost
            new TranslationSeedItem("entity.qualityIssueMeeting.otherapparatuscost", "en-US", "其他设备工程搬运费", "其他设备购入费、工程费、搬运费等（元）"),
            // entity.qualityIssueMeeting.otherapparatuscost
            new TranslationSeedItem("entity.qualityIssueMeeting.otherapparatuscost", "ja-JP", "其他设备工程搬运费", "其他设备购入费、工程费、搬运费等（元）"),
            // entity.qualityIssueMeeting.otherapparatuscost
            new TranslationSeedItem("entity.qualityIssueMeeting.otherapparatuscost", "zh-CN", "其他设备工程搬运费", "其他设备购入费、工程费、搬运费等（元）"),
            // entity.qualityIssueMeeting.otherapparatuscost
            new TranslationSeedItem("entity.qualityIssueMeeting.otherapparatuscost", "zh-HK", "其他设备工程搬运费", "其他设备购入费、工程费、搬运费等（元）"),

            // entity.qualityIssueMeeting.meetingrecorder
            new TranslationSeedItem("entity.qualityIssueMeeting.meetingrecorder", "en-US", "品质问题对应记录者", "品质问题対応记录者（会议调查试验记录者）"),
            // entity.qualityIssueMeeting.meetingrecorder
            new TranslationSeedItem("entity.qualityIssueMeeting.meetingrecorder", "ja-JP", "品质问题对应记录者", "品质问题対応记录者（会议调查试验记录者）"),
            // entity.qualityIssueMeeting.meetingrecorder
            new TranslationSeedItem("entity.qualityIssueMeeting.meetingrecorder", "zh-CN", "品质问题对应记录者", "品质问题対応记录者（会议调查试验记录者）"),
            // entity.qualityIssueMeeting.meetingrecorder
            new TranslationSeedItem("entity.qualityIssueMeeting.meetingrecorder", "zh-HK", "品质问题对应记录者", "品质问题対応记录者（会议调查试验记录者）"),
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
        translation.ResourceGroup = TaktModule.Logistics;
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
