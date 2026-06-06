// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Cost
// 文件名称：TaktQualityIssueI18nSeedData.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktQualityIssue 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktQualityIssue 实体国际化翻译种子（键前缀 entity.qualityIssue.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktQualityIssueI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktQualityIssue 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 qualityIssue 实体翻译...", tenantCode);

        foreach (var item in GetQualityIssueTranslations())
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

        TaktLogger.Information("TaktQualityIssue 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktQualityIssue 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.qualityIssue._self / entity.qualityIssue.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetQualityIssueTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.qualityIssue._self
            new TranslationSeedItem("entity.qualityIssue._self", "en-US", "Quality Issue Information", "实体名称"),
            // entity.qualityIssue._self
            new TranslationSeedItem("entity.qualityIssue._self", "ja-JP", "品质问题应对主表信息", "实体名称"),
            // entity.qualityIssue._self
            new TranslationSeedItem("entity.qualityIssue._self", "zh-CN", "品质问题应对主表信息", "实体名称"),
            // entity.qualityIssue._self
            new TranslationSeedItem("entity.qualityIssue._self", "zh-HK", "品质问题应对主表信息", "实体名称"),

            // entity.qualityIssue.plantcode
            new TranslationSeedItem("entity.qualityIssue.plantcode", "en-US", "工厂代码", "工厂代码"),
            // entity.qualityIssue.plantcode
            new TranslationSeedItem("entity.qualityIssue.plantcode", "ja-JP", "工厂代码", "工厂代码"),
            // entity.qualityIssue.plantcode
            new TranslationSeedItem("entity.qualityIssue.plantcode", "zh-CN", "工厂代码", "工厂代码"),
            // entity.qualityIssue.plantcode
            new TranslationSeedItem("entity.qualityIssue.plantcode", "zh-HK", "工厂代码", "工厂代码"),

            // entity.qualityIssue.code
            new TranslationSeedItem("entity.qualityIssue.code", "en-US", "品质问题编码", "品质问题编码（唯一，如：QI-2026-0001）"),
            // entity.qualityIssue.code
            new TranslationSeedItem("entity.qualityIssue.code", "ja-JP", "品质问题编码", "品质问题编码（唯一，如：QI-2026-0001）"),
            // entity.qualityIssue.code
            new TranslationSeedItem("entity.qualityIssue.code", "zh-CN", "品质问题编码", "品质问题编码（唯一，如：QI-2026-0001）"),
            // entity.qualityIssue.code
            new TranslationSeedItem("entity.qualityIssue.code", "zh-HK", "品质问题编码", "品质问题编码（唯一，如：QI-2026-0001）"),

            // entity.qualityIssue.issuedate
            new TranslationSeedItem("entity.qualityIssue.issuedate", "en-US", "问题日期", "问题日期"),
            // entity.qualityIssue.issuedate
            new TranslationSeedItem("entity.qualityIssue.issuedate", "ja-JP", "问题日期", "问题日期"),
            // entity.qualityIssue.issuedate
            new TranslationSeedItem("entity.qualityIssue.issuedate", "zh-CN", "问题日期", "问题日期"),
            // entity.qualityIssue.issuedate
            new TranslationSeedItem("entity.qualityIssue.issuedate", "zh-HK", "问题日期", "问题日期"),

            // entity.qualityIssue.model
            new TranslationSeedItem("entity.qualityIssue.model", "en-US", "机种", "机种/产品型号"),
            // entity.qualityIssue.model
            new TranslationSeedItem("entity.qualityIssue.model", "ja-JP", "机种", "机种/产品型号"),
            // entity.qualityIssue.model
            new TranslationSeedItem("entity.qualityIssue.model", "zh-CN", "机种", "机种/产品型号"),
            // entity.qualityIssue.model
            new TranslationSeedItem("entity.qualityIssue.model", "zh-HK", "机种", "机种/产品型号"),

            // entity.qualityIssue.lot
            new TranslationSeedItem("entity.qualityIssue.lot", "en-US", "批次号", "批次号/Lot No"),
            // entity.qualityIssue.lot
            new TranslationSeedItem("entity.qualityIssue.lot", "ja-JP", "批次号", "批次号/Lot No"),
            // entity.qualityIssue.lot
            new TranslationSeedItem("entity.qualityIssue.lot", "zh-CN", "批次号", "批次号/Lot No"),
            // entity.qualityIssue.lot
            new TranslationSeedItem("entity.qualityIssue.lot", "zh-HK", "批次号", "批次号/Lot No"),

            // entity.qualityIssue.qualityproblemsresponse
            new TranslationSeedItem("entity.qualityIssue.qualityproblemsresponse", "en-US", "品质问题应对", "品质问题应对摘要(汇总说明)"),
            // entity.qualityIssue.qualityproblemsresponse
            new TranslationSeedItem("entity.qualityIssue.qualityproblemsresponse", "ja-JP", "品质问题应对", "品质问题应对摘要(汇总说明)"),
            // entity.qualityIssue.qualityproblemsresponse
            new TranslationSeedItem("entity.qualityIssue.qualityproblemsresponse", "zh-CN", "品质问题应对", "品质问题应对摘要(汇总说明)"),
            // entity.qualityIssue.qualityproblemsresponse
            new TranslationSeedItem("entity.qualityIssue.qualityproblemsresponse", "zh-HK", "品质问题应对", "品质问题应对摘要(汇总说明)"),

            // entity.qualityIssue.reworkduetodefects
            new TranslationSeedItem("entity.qualityIssue.reworkduetodefects", "en-US", "不良改修应对", "不良改修应对摘要(汇总说明)"),
            // entity.qualityIssue.reworkduetodefects
            new TranslationSeedItem("entity.qualityIssue.reworkduetodefects", "ja-JP", "不良改修应对", "不良改修应对摘要(汇总说明)"),
            // entity.qualityIssue.reworkduetodefects
            new TranslationSeedItem("entity.qualityIssue.reworkduetodefects", "zh-CN", "不良改修应对", "不良改修应对摘要(汇总说明)"),
            // entity.qualityIssue.reworkduetodefects
            new TranslationSeedItem("entity.qualityIssue.reworkduetodefects", "zh-HK", "不良改修应对", "不良改修应对摘要(汇总说明)"),

            // entity.qualityIssue.needrework
            new TranslationSeedItem("entity.qualityIssue.needrework", "en-US", "是否需要不良改修应对", "是否需要不良改修应对(Y/N)"),
            // entity.qualityIssue.needrework
            new TranslationSeedItem("entity.qualityIssue.needrework", "ja-JP", "是否需要不良改修应对", "是否需要不良改修应对(Y/N)"),
            // entity.qualityIssue.needrework
            new TranslationSeedItem("entity.qualityIssue.needrework", "zh-CN", "是否需要不良改修应对", "是否需要不良改修应对(Y/N)"),
            // entity.qualityIssue.needrework
            new TranslationSeedItem("entity.qualityIssue.needrework", "zh-HK", "是否需要不良改修应对", "是否需要不良改修应对(Y/N)"),

            // entity.qualityIssue.totaltimeminutes
            new TranslationSeedItem("entity.qualityIssue.totaltimeminutes", "en-US", "总时间", "总时间(分钟,自动计算 = 各子表时间合计)"),
            // entity.qualityIssue.totaltimeminutes
            new TranslationSeedItem("entity.qualityIssue.totaltimeminutes", "ja-JP", "总时间", "总时间(分钟,自动计算 = 各子表时间合计)"),
            // entity.qualityIssue.totaltimeminutes
            new TranslationSeedItem("entity.qualityIssue.totaltimeminutes", "zh-CN", "总时间", "总时间(分钟,自动计算 = 各子表时间合计)"),
            // entity.qualityIssue.totaltimeminutes
            new TranslationSeedItem("entity.qualityIssue.totaltimeminutes", "zh-HK", "总时间", "总时间(分钟,自动计算 = 各子表时间合计)"),

            // entity.qualityIssue.totalcost
            new TranslationSeedItem("entity.qualityIssue.totalcost", "en-US", "总费用", "总费用(元,自动计算 = 各子表费用合计)"),
            // entity.qualityIssue.totalcost
            new TranslationSeedItem("entity.qualityIssue.totalcost", "ja-JP", "总费用", "总费用(元,自动计算 = 各子表费用合计)"),
            // entity.qualityIssue.totalcost
            new TranslationSeedItem("entity.qualityIssue.totalcost", "zh-CN", "总费用", "总费用(元,自动计算 = 各子表费用合计)"),
            // entity.qualityIssue.totalcost
            new TranslationSeedItem("entity.qualityIssue.totalcost", "zh-HK", "总费用", "总费用(元,自动计算 = 各子表费用合计)"),

            // entity.qualityIssue.costcurrency
            new TranslationSeedItem("entity.qualityIssue.costcurrency", "en-US", "成本币种", "成本币种（CNY/USD/JPY等）"),
            // entity.qualityIssue.costcurrency
            new TranslationSeedItem("entity.qualityIssue.costcurrency", "ja-JP", "成本币种", "成本币种（CNY/USD/JPY等）"),
            // entity.qualityIssue.costcurrency
            new TranslationSeedItem("entity.qualityIssue.costcurrency", "zh-CN", "成本币种", "成本币种（CNY/USD/JPY等）"),
            // entity.qualityIssue.costcurrency
            new TranslationSeedItem("entity.qualityIssue.costcurrency", "zh-HK", "成本币种", "成本币种（CNY/USD/JPY等）"),

            // entity.qualityIssue.meetingitems
            new TranslationSeedItem("entity.qualityIssue.meetingitems", "en-US", "meetingItems", "会议/调查/试验费用明细列表"),
            // entity.qualityIssue.meetingitems
            new TranslationSeedItem("entity.qualityIssue.meetingitems", "ja-JP", "meetingItems", "会议/调查/试验费用明细列表"),
            // entity.qualityIssue.meetingitems
            new TranslationSeedItem("entity.qualityIssue.meetingitems", "zh-CN", "meetingItems", "会议/调查/试验费用明细列表"),
            // entity.qualityIssue.meetingitems
            new TranslationSeedItem("entity.qualityIssue.meetingitems", "zh-HK", "meetingItems", "会议/调查/试验费用明细列表"),
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
