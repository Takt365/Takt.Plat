// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Output
// 文件名称：TaktAssyOutputDetailI18nSeedData.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktAssyOutputDetail 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Output;

/// <summary>
/// TaktAssyOutputDetail 实体国际化翻译种子（键前缀 entity.assyOutputDetail.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktAssyOutputDetailI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktAssyOutputDetail 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 assyOutputDetail 实体翻译...", tenantCode);

        foreach (var item in GetAssyOutputDetailTranslations())
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

        TaktLogger.Information("TaktAssyOutputDetail 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktAssyOutputDetail 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.assyOutputDetail._self / entity.assyOutputDetail.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetAssyOutputDetailTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.assyOutputDetail._self
            new TranslationSeedItem("entity.assyOutputDetail._self", "en-US", "Assy Output Detail Information", "实体名称"),
            // entity.assyOutputDetail._self
            new TranslationSeedItem("entity.assyOutputDetail._self", "ja-JP", "组立日报明细信息", "实体名称"),
            // entity.assyOutputDetail._self
            new TranslationSeedItem("entity.assyOutputDetail._self", "zh-CN", "组立日报明细信息", "实体名称"),
            // entity.assyOutputDetail._self
            new TranslationSeedItem("entity.assyOutputDetail._self", "zh-HK", "组立日报明细信息", "实体名称"),

            // entity.assyOutputDetail.assyoutputid
            new TranslationSeedItem("entity.assyOutputDetail.assyoutputid", "en-US", "组立日报ID", "组立日报ID（主表主键,序列化为string以避免Javascript精度问题）"),
            // entity.assyOutputDetail.assyoutputid
            new TranslationSeedItem("entity.assyOutputDetail.assyoutputid", "ja-JP", "组立日报ID", "组立日报ID（主表主键,序列化为string以避免Javascript精度问题）"),
            // entity.assyOutputDetail.assyoutputid
            new TranslationSeedItem("entity.assyOutputDetail.assyoutputid", "zh-CN", "组立日报ID", "组立日报ID（主表主键,序列化为string以避免Javascript精度问题）"),
            // entity.assyOutputDetail.assyoutputid
            new TranslationSeedItem("entity.assyOutputDetail.assyoutputid", "zh-HK", "组立日报ID", "组立日报ID（主表主键,序列化为string以避免Javascript精度问题）"),

            // entity.assyOutputDetail.prodordercode
            new TranslationSeedItem("entity.assyOutputDetail.prodordercode", "en-US", "生产工单号", "生产工单号（冗余字段,便于查询）"),
            // entity.assyOutputDetail.prodordercode
            new TranslationSeedItem("entity.assyOutputDetail.prodordercode", "ja-JP", "生产工单号", "生产工单号（冗余字段,便于查询）"),
            // entity.assyOutputDetail.prodordercode
            new TranslationSeedItem("entity.assyOutputDetail.prodordercode", "zh-CN", "生产工单号", "生产工单号（冗余字段,便于查询）"),
            // entity.assyOutputDetail.prodordercode
            new TranslationSeedItem("entity.assyOutputDetail.prodordercode", "zh-HK", "生产工单号", "生产工单号（冗余字段,便于查询）"),

            // entity.assyOutputDetail.linenumber
            new TranslationSeedItem("entity.assyOutputDetail.linenumber", "en-US", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.assyOutputDetail.linenumber
            new TranslationSeedItem("entity.assyOutputDetail.linenumber", "ja-JP", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.assyOutputDetail.linenumber
            new TranslationSeedItem("entity.assyOutputDetail.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.assyOutputDetail.linenumber
            new TranslationSeedItem("entity.assyOutputDetail.linenumber", "zh-HK", "行号", "行号（项号/序号，固定步长=10）"),

            // entity.assyOutputDetail.timeperiod
            new TranslationSeedItem("entity.assyOutputDetail.timeperiod", "en-US", "生产时段", "生产时段"),
            // entity.assyOutputDetail.timeperiod
            new TranslationSeedItem("entity.assyOutputDetail.timeperiod", "ja-JP", "生产时段", "生产时段"),
            // entity.assyOutputDetail.timeperiod
            new TranslationSeedItem("entity.assyOutputDetail.timeperiod", "zh-CN", "生产时段", "生产时段"),
            // entity.assyOutputDetail.timeperiod
            new TranslationSeedItem("entity.assyOutputDetail.timeperiod", "zh-HK", "生产时段", "生产时段"),

            // entity.assyOutputDetail.prodactualqty
            new TranslationSeedItem("entity.assyOutputDetail.prodactualqty", "en-US", "实际生产数量", "实际生产数量"),
            // entity.assyOutputDetail.prodactualqty
            new TranslationSeedItem("entity.assyOutputDetail.prodactualqty", "ja-JP", "实际生产数量", "实际生产数量"),
            // entity.assyOutputDetail.prodactualqty
            new TranslationSeedItem("entity.assyOutputDetail.prodactualqty", "zh-CN", "实际生产数量", "实际生产数量"),
            // entity.assyOutputDetail.prodactualqty
            new TranslationSeedItem("entity.assyOutputDetail.prodactualqty", "zh-HK", "实际生产数量", "实际生产数量"),

            // entity.assyOutputDetail.downtimeminutes
            new TranslationSeedItem("entity.assyOutputDetail.downtimeminutes", "en-US", "停线时间", "停线时间(分钟)"),
            // entity.assyOutputDetail.downtimeminutes
            new TranslationSeedItem("entity.assyOutputDetail.downtimeminutes", "ja-JP", "停线时间", "停线时间(分钟)"),
            // entity.assyOutputDetail.downtimeminutes
            new TranslationSeedItem("entity.assyOutputDetail.downtimeminutes", "zh-CN", "停线时间", "停线时间(分钟)"),
            // entity.assyOutputDetail.downtimeminutes
            new TranslationSeedItem("entity.assyOutputDetail.downtimeminutes", "zh-HK", "停线时间", "停线时间(分钟)"),

            // entity.assyOutputDetail.downtimereason
            new TranslationSeedItem("entity.assyOutputDetail.downtimereason", "en-US", "停线原因", "停线原因"),
            // entity.assyOutputDetail.downtimereason
            new TranslationSeedItem("entity.assyOutputDetail.downtimereason", "ja-JP", "停线原因", "停线原因"),
            // entity.assyOutputDetail.downtimereason
            new TranslationSeedItem("entity.assyOutputDetail.downtimereason", "zh-CN", "停线原因", "停线原因"),
            // entity.assyOutputDetail.downtimereason
            new TranslationSeedItem("entity.assyOutputDetail.downtimereason", "zh-HK", "停线原因", "停线原因"),

            // entity.assyOutputDetail.downtimedescription
            new TranslationSeedItem("entity.assyOutputDetail.downtimedescription", "en-US", "停线说明", "停线说明"),
            // entity.assyOutputDetail.downtimedescription
            new TranslationSeedItem("entity.assyOutputDetail.downtimedescription", "ja-JP", "停线说明", "停线说明"),
            // entity.assyOutputDetail.downtimedescription
            new TranslationSeedItem("entity.assyOutputDetail.downtimedescription", "zh-CN", "停线说明", "停线说明"),
            // entity.assyOutputDetail.downtimedescription
            new TranslationSeedItem("entity.assyOutputDetail.downtimedescription", "zh-HK", "停线说明", "停线说明"),

            // entity.assyOutputDetail.unachievedreason
            new TranslationSeedItem("entity.assyOutputDetail.unachievedreason", "en-US", "未达成原因", "未达成原因"),
            // entity.assyOutputDetail.unachievedreason
            new TranslationSeedItem("entity.assyOutputDetail.unachievedreason", "ja-JP", "未达成原因", "未达成原因"),
            // entity.assyOutputDetail.unachievedreason
            new TranslationSeedItem("entity.assyOutputDetail.unachievedreason", "zh-CN", "未达成原因", "未达成原因"),
            // entity.assyOutputDetail.unachievedreason
            new TranslationSeedItem("entity.assyOutputDetail.unachievedreason", "zh-HK", "未达成原因", "未达成原因"),

            // entity.assyOutputDetail.unachieveddescription
            new TranslationSeedItem("entity.assyOutputDetail.unachieveddescription", "en-US", "未达成说明", "未达成说明"),
            // entity.assyOutputDetail.unachieveddescription
            new TranslationSeedItem("entity.assyOutputDetail.unachieveddescription", "ja-JP", "未达成说明", "未达成说明"),
            // entity.assyOutputDetail.unachieveddescription
            new TranslationSeedItem("entity.assyOutputDetail.unachieveddescription", "zh-CN", "未达成说明", "未达成说明"),
            // entity.assyOutputDetail.unachieveddescription
            new TranslationSeedItem("entity.assyOutputDetail.unachieveddescription", "zh-HK", "未达成说明", "未达成说明"),

            // entity.assyOutputDetail.inputminutes
            new TranslationSeedItem("entity.assyOutputDetail.inputminutes", "en-US", "投入工时", "投入工时(分钟)"),
            // entity.assyOutputDetail.inputminutes
            new TranslationSeedItem("entity.assyOutputDetail.inputminutes", "ja-JP", "投入工时", "投入工时(分钟)"),
            // entity.assyOutputDetail.inputminutes
            new TranslationSeedItem("entity.assyOutputDetail.inputminutes", "zh-CN", "投入工时", "投入工时(分钟)"),
            // entity.assyOutputDetail.inputminutes
            new TranslationSeedItem("entity.assyOutputDetail.inputminutes", "zh-HK", "投入工时", "投入工时(分钟)"),

            // entity.assyOutputDetail.prodminutes
            new TranslationSeedItem("entity.assyOutputDetail.prodminutes", "en-US", "生产工时", "生产工时(分钟)"),
            // entity.assyOutputDetail.prodminutes
            new TranslationSeedItem("entity.assyOutputDetail.prodminutes", "ja-JP", "生产工时", "生产工时(分钟)"),
            // entity.assyOutputDetail.prodminutes
            new TranslationSeedItem("entity.assyOutputDetail.prodminutes", "zh-CN", "生产工时", "生产工时(分钟)"),
            // entity.assyOutputDetail.prodminutes
            new TranslationSeedItem("entity.assyOutputDetail.prodminutes", "zh-HK", "生产工时", "生产工时(分钟)"),

            // entity.assyOutputDetail.actualminutes
            new TranslationSeedItem("entity.assyOutputDetail.actualminutes", "en-US", "实际工时", "实际工时(分钟)"),
            // entity.assyOutputDetail.actualminutes
            new TranslationSeedItem("entity.assyOutputDetail.actualminutes", "ja-JP", "实际工时", "实际工时(分钟)"),
            // entity.assyOutputDetail.actualminutes
            new TranslationSeedItem("entity.assyOutputDetail.actualminutes", "zh-CN", "实际工时", "实际工时(分钟)"),
            // entity.assyOutputDetail.actualminutes
            new TranslationSeedItem("entity.assyOutputDetail.actualminutes", "zh-HK", "实际工时", "实际工时(分钟)"),

            // entity.assyOutputDetail.achievementrate
            new TranslationSeedItem("entity.assyOutputDetail.achievementrate", "en-US", "达成率", "达成率(%)"),
            // entity.assyOutputDetail.achievementrate
            new TranslationSeedItem("entity.assyOutputDetail.achievementrate", "ja-JP", "达成率", "达成率(%)"),
            // entity.assyOutputDetail.achievementrate
            new TranslationSeedItem("entity.assyOutputDetail.achievementrate", "zh-CN", "达成率", "达成率(%)"),
            // entity.assyOutputDetail.achievementrate
            new TranslationSeedItem("entity.assyOutputDetail.achievementrate", "zh-HK", "达成率", "达成率(%)"),
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
