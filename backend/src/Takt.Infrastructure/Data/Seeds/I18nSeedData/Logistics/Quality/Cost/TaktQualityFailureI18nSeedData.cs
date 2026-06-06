// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Cost
// 文件名称：TaktQualityFailureI18nSeedData.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktQualityFailure 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktQualityFailure 实体国际化翻译种子（键前缀 entity.qualityFailure.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktQualityFailureI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktQualityFailure 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 qualityFailure 实体翻译...", tenantCode);

        foreach (var item in GetQualityFailureTranslations())
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

        TaktLogger.Information("TaktQualityFailure 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktQualityFailure 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.qualityFailure._self / entity.qualityFailure.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetQualityFailureTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.qualityFailure._self
            new TranslationSeedItem("entity.qualityFailure._self", "en-US", "Quality Failure Information", "实体名称"),
            // entity.qualityFailure._self
            new TranslationSeedItem("entity.qualityFailure._self", "ja-JP", "品质问题应对主表信息", "实体名称"),
            // entity.qualityFailure._self
            new TranslationSeedItem("entity.qualityFailure._self", "zh-CN", "品质问题应对主表信息", "实体名称"),
            // entity.qualityFailure._self
            new TranslationSeedItem("entity.qualityFailure._self", "zh-HK", "品质问题应对主表信息", "实体名称"),

            // entity.qualityFailure.plantcode
            new TranslationSeedItem("entity.qualityFailure.plantcode", "en-US", "工厂代码", "工厂代码"),
            // entity.qualityFailure.plantcode
            new TranslationSeedItem("entity.qualityFailure.plantcode", "ja-JP", "工厂代码", "工厂代码"),
            // entity.qualityFailure.plantcode
            new TranslationSeedItem("entity.qualityFailure.plantcode", "zh-CN", "工厂代码", "工厂代码"),
            // entity.qualityFailure.plantcode
            new TranslationSeedItem("entity.qualityFailure.plantcode", "zh-HK", "工厂代码", "工厂代码"),

            // entity.qualityFailure.code
            new TranslationSeedItem("entity.qualityFailure.code", "en-US", "品质问题编码", "品质问题编码（唯一，如：QF-2026-0001）"),
            // entity.qualityFailure.code
            new TranslationSeedItem("entity.qualityFailure.code", "ja-JP", "品质问题编码", "品质问题编码（唯一，如：QF-2026-0001）"),
            // entity.qualityFailure.code
            new TranslationSeedItem("entity.qualityFailure.code", "zh-CN", "品质问题编码", "品质问题编码（唯一，如：QF-2026-0001）"),
            // entity.qualityFailure.code
            new TranslationSeedItem("entity.qualityFailure.code", "zh-HK", "品质问题编码", "品质问题编码（唯一，如：QF-2026-0001）"),

            // entity.qualityFailure.failuredate
            new TranslationSeedItem("entity.qualityFailure.failuredate", "en-US", "问题日期", "问题日期"),
            // entity.qualityFailure.failuredate
            new TranslationSeedItem("entity.qualityFailure.failuredate", "ja-JP", "问题日期", "问题日期"),
            // entity.qualityFailure.failuredate
            new TranslationSeedItem("entity.qualityFailure.failuredate", "zh-CN", "问题日期", "问题日期"),
            // entity.qualityFailure.failuredate
            new TranslationSeedItem("entity.qualityFailure.failuredate", "zh-HK", "问题日期", "问题日期"),

            // entity.qualityFailure.model
            new TranslationSeedItem("entity.qualityFailure.model", "en-US", "机种", "机种/产品型号"),
            // entity.qualityFailure.model
            new TranslationSeedItem("entity.qualityFailure.model", "ja-JP", "机种", "机种/产品型号"),
            // entity.qualityFailure.model
            new TranslationSeedItem("entity.qualityFailure.model", "zh-CN", "机种", "机种/产品型号"),
            // entity.qualityFailure.model
            new TranslationSeedItem("entity.qualityFailure.model", "zh-HK", "机种", "机种/产品型号"),

            // entity.qualityFailure.lot
            new TranslationSeedItem("entity.qualityFailure.lot", "en-US", "批次号", "批次号/Lot No"),
            // entity.qualityFailure.lot
            new TranslationSeedItem("entity.qualityFailure.lot", "ja-JP", "批次号", "批次号/Lot No"),
            // entity.qualityFailure.lot
            new TranslationSeedItem("entity.qualityFailure.lot", "zh-CN", "批次号", "批次号/Lot No"),
            // entity.qualityFailure.lot
            new TranslationSeedItem("entity.qualityFailure.lot", "zh-HK", "批次号", "批次号/Lot No"),

            // entity.qualityFailure.qualityproblemsresponse
            new TranslationSeedItem("entity.qualityFailure.qualityproblemsresponse", "en-US", "品质问题应对", "品质问题应对摘要(汇总说明)"),
            // entity.qualityFailure.qualityproblemsresponse
            new TranslationSeedItem("entity.qualityFailure.qualityproblemsresponse", "ja-JP", "品质问题应对", "品质问题应对摘要(汇总说明)"),
            // entity.qualityFailure.qualityproblemsresponse
            new TranslationSeedItem("entity.qualityFailure.qualityproblemsresponse", "zh-CN", "品质问题应对", "品质问题应对摘要(汇总说明)"),
            // entity.qualityFailure.qualityproblemsresponse
            new TranslationSeedItem("entity.qualityFailure.qualityproblemsresponse", "zh-HK", "品质问题应对", "品质问题应对摘要(汇总说明)"),

            // entity.qualityFailure.reworkduetodefects
            new TranslationSeedItem("entity.qualityFailure.reworkduetodefects", "en-US", "不良改修应对", "不良改修应对摘要(汇总说明)"),
            // entity.qualityFailure.reworkduetodefects
            new TranslationSeedItem("entity.qualityFailure.reworkduetodefects", "ja-JP", "不良改修应对", "不良改修应对摘要(汇总说明)"),
            // entity.qualityFailure.reworkduetodefects
            new TranslationSeedItem("entity.qualityFailure.reworkduetodefects", "zh-CN", "不良改修应对", "不良改修应对摘要(汇总说明)"),
            // entity.qualityFailure.reworkduetodefects
            new TranslationSeedItem("entity.qualityFailure.reworkduetodefects", "zh-HK", "不良改修应对", "不良改修应对摘要(汇总说明)"),

            // entity.qualityFailure.needrework
            new TranslationSeedItem("entity.qualityFailure.needrework", "en-US", "是否需要不良改修应对", "是否需要不良改修应对(Y/N)"),
            // entity.qualityFailure.needrework
            new TranslationSeedItem("entity.qualityFailure.needrework", "ja-JP", "是否需要不良改修应对", "是否需要不良改修应对(Y/N)"),
            // entity.qualityFailure.needrework
            new TranslationSeedItem("entity.qualityFailure.needrework", "zh-CN", "是否需要不良改修应对", "是否需要不良改修应对(Y/N)"),
            // entity.qualityFailure.needrework
            new TranslationSeedItem("entity.qualityFailure.needrework", "zh-HK", "是否需要不良改修应对", "是否需要不良改修应对(Y/N)"),

            // entity.qualityFailure.totaltimeminutes
            new TranslationSeedItem("entity.qualityFailure.totaltimeminutes", "en-US", "总时间", "总时间(分钟,自动计算 = 各子表时间合计)"),
            // entity.qualityFailure.totaltimeminutes
            new TranslationSeedItem("entity.qualityFailure.totaltimeminutes", "ja-JP", "总时间", "总时间(分钟,自动计算 = 各子表时间合计)"),
            // entity.qualityFailure.totaltimeminutes
            new TranslationSeedItem("entity.qualityFailure.totaltimeminutes", "zh-CN", "总时间", "总时间(分钟,自动计算 = 各子表时间合计)"),
            // entity.qualityFailure.totaltimeminutes
            new TranslationSeedItem("entity.qualityFailure.totaltimeminutes", "zh-HK", "总时间", "总时间(分钟,自动计算 = 各子表时间合计)"),

            // entity.qualityFailure.totalcost
            new TranslationSeedItem("entity.qualityFailure.totalcost", "en-US", "总费用", "总费用(元,自动计算 = 各子表费用合计)"),
            // entity.qualityFailure.totalcost
            new TranslationSeedItem("entity.qualityFailure.totalcost", "ja-JP", "总费用", "总费用(元,自动计算 = 各子表费用合计)"),
            // entity.qualityFailure.totalcost
            new TranslationSeedItem("entity.qualityFailure.totalcost", "zh-CN", "总费用", "总费用(元,自动计算 = 各子表费用合计)"),
            // entity.qualityFailure.totalcost
            new TranslationSeedItem("entity.qualityFailure.totalcost", "zh-HK", "总费用", "总费用(元,自动计算 = 各子表费用合计)"),

            // entity.qualityFailure.costcurrency
            new TranslationSeedItem("entity.qualityFailure.costcurrency", "en-US", "成本币种", "成本币种（CNY/USD/JPY等）"),
            // entity.qualityFailure.costcurrency
            new TranslationSeedItem("entity.qualityFailure.costcurrency", "ja-JP", "成本币种", "成本币种（CNY/USD/JPY等）"),
            // entity.qualityFailure.costcurrency
            new TranslationSeedItem("entity.qualityFailure.costcurrency", "zh-CN", "成本币种", "成本币种（CNY/USD/JPY等）"),
            // entity.qualityFailure.costcurrency
            new TranslationSeedItem("entity.qualityFailure.costcurrency", "zh-HK", "成本币种", "成本币种（CNY/USD/JPY等）"),

            // entity.qualityFailure.meetingitems
            new TranslationSeedItem("entity.qualityFailure.meetingitems", "en-US", "meetingItems", "会议/调查/试验费用明细列表"),
            // entity.qualityFailure.meetingitems
            new TranslationSeedItem("entity.qualityFailure.meetingitems", "ja-JP", "meetingItems", "会议/调查/试验费用明细列表"),
            // entity.qualityFailure.meetingitems
            new TranslationSeedItem("entity.qualityFailure.meetingitems", "zh-CN", "meetingItems", "会议/调查/试验费用明细列表"),
            // entity.qualityFailure.meetingitems
            new TranslationSeedItem("entity.qualityFailure.meetingitems", "zh-HK", "meetingItems", "会议/调查/试验费用明细列表"),
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
