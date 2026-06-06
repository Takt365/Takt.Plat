// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Bom
// 文件名称：TaktStandardOperationTimeI18nSeedData.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktStandardOperationTime 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Bom;

/// <summary>
/// TaktStandardOperationTime 实体国际化翻译种子（键前缀 entity.standardOperationTime.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktStandardOperationTimeI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktStandardOperationTime 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 standardOperationTime 实体翻译...", tenantCode);

        foreach (var item in GetStandardOperationTimeTranslations())
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

        TaktLogger.Information("TaktStandardOperationTime 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktStandardOperationTime 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.standardOperationTime._self / entity.standardOperationTime.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetStandardOperationTimeTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.standardOperationTime._self
            new TranslationSeedItem("entity.standardOperationTime._self", "en-US", "Standard Operation Time Information", "实体名称"),
            // entity.standardOperationTime._self
            new TranslationSeedItem("entity.standardOperationTime._self", "ja-JP", "标准工序时间信息", "实体名称"),
            // entity.standardOperationTime._self
            new TranslationSeedItem("entity.standardOperationTime._self", "zh-CN", "标准工序时间信息", "实体名称"),
            // entity.standardOperationTime._self
            new TranslationSeedItem("entity.standardOperationTime._self", "zh-HK", "标准工序时间信息", "实体名称"),

            // entity.standardOperationTime.plantcode
            new TranslationSeedItem("entity.standardOperationTime.plantcode", "en-US", "工厂代码", "工厂代码"),
            // entity.standardOperationTime.plantcode
            new TranslationSeedItem("entity.standardOperationTime.plantcode", "ja-JP", "工厂代码", "工厂代码"),
            // entity.standardOperationTime.plantcode
            new TranslationSeedItem("entity.standardOperationTime.plantcode", "zh-CN", "工厂代码", "工厂代码"),
            // entity.standardOperationTime.plantcode
            new TranslationSeedItem("entity.standardOperationTime.plantcode", "zh-HK", "工厂代码", "工厂代码"),

            // entity.standardOperationTime.materialcode
            new TranslationSeedItem("entity.standardOperationTime.materialcode", "en-US", "物料编码", "物料编码"),
            // entity.standardOperationTime.materialcode
            new TranslationSeedItem("entity.standardOperationTime.materialcode", "ja-JP", "物料编码", "物料编码"),
            // entity.standardOperationTime.materialcode
            new TranslationSeedItem("entity.standardOperationTime.materialcode", "zh-CN", "物料编码", "物料编码"),
            // entity.standardOperationTime.materialcode
            new TranslationSeedItem("entity.standardOperationTime.materialcode", "zh-HK", "物料编码", "物料编码"),

            // entity.standardOperationTime.workcenter
            new TranslationSeedItem("entity.standardOperationTime.workcenter", "en-US", "工作中心", "工作中心"),
            // entity.standardOperationTime.workcenter
            new TranslationSeedItem("entity.standardOperationTime.workcenter", "ja-JP", "工作中心", "工作中心"),
            // entity.standardOperationTime.workcenter
            new TranslationSeedItem("entity.standardOperationTime.workcenter", "zh-CN", "工作中心", "工作中心"),
            // entity.standardOperationTime.workcenter
            new TranslationSeedItem("entity.standardOperationTime.workcenter", "zh-HK", "工作中心", "工作中心"),

            // entity.standardOperationTime.operationdesc
            new TranslationSeedItem("entity.standardOperationTime.operationdesc", "en-US", "工序描述", "工序描述"),
            // entity.standardOperationTime.operationdesc
            new TranslationSeedItem("entity.standardOperationTime.operationdesc", "ja-JP", "工序描述", "工序描述"),
            // entity.standardOperationTime.operationdesc
            new TranslationSeedItem("entity.standardOperationTime.operationdesc", "zh-CN", "工序描述", "工序描述"),
            // entity.standardOperationTime.operationdesc
            new TranslationSeedItem("entity.standardOperationTime.operationdesc", "zh-HK", "工序描述", "工序描述"),

            // entity.standardOperationTime.standardminutes
            new TranslationSeedItem("entity.standardOperationTime.standardminutes", "en-US", "标准工时", "标准工时（分钟）"),
            // entity.standardOperationTime.standardminutes
            new TranslationSeedItem("entity.standardOperationTime.standardminutes", "ja-JP", "标准工时", "标准工时（分钟）"),
            // entity.standardOperationTime.standardminutes
            new TranslationSeedItem("entity.standardOperationTime.standardminutes", "zh-CN", "标准工时", "标准工时（分钟）"),
            // entity.standardOperationTime.standardminutes
            new TranslationSeedItem("entity.standardOperationTime.standardminutes", "zh-HK", "标准工时", "标准工时（分钟）"),

            // entity.standardOperationTime.timeunit
            new TranslationSeedItem("entity.standardOperationTime.timeunit", "en-US", "工时单位", "工时单位"),
            // entity.standardOperationTime.timeunit
            new TranslationSeedItem("entity.standardOperationTime.timeunit", "ja-JP", "工时单位", "工时单位"),
            // entity.standardOperationTime.timeunit
            new TranslationSeedItem("entity.standardOperationTime.timeunit", "zh-CN", "工时单位", "工时单位"),
            // entity.standardOperationTime.timeunit
            new TranslationSeedItem("entity.standardOperationTime.timeunit", "zh-HK", "工时单位", "工时单位"),

            // entity.standardOperationTime.standardshorts
            new TranslationSeedItem("entity.standardOperationTime.standardshorts", "en-US", "标准点数", "标准点数"),
            // entity.standardOperationTime.standardshorts
            new TranslationSeedItem("entity.standardOperationTime.standardshorts", "ja-JP", "标准点数", "标准点数"),
            // entity.standardOperationTime.standardshorts
            new TranslationSeedItem("entity.standardOperationTime.standardshorts", "zh-CN", "标准点数", "标准点数"),
            // entity.standardOperationTime.standardshorts
            new TranslationSeedItem("entity.standardOperationTime.standardshorts", "zh-HK", "标准点数", "标准点数"),

            // entity.standardOperationTime.pointsunit
            new TranslationSeedItem("entity.standardOperationTime.pointsunit", "en-US", "点数单位", "点数单位"),
            // entity.standardOperationTime.pointsunit
            new TranslationSeedItem("entity.standardOperationTime.pointsunit", "ja-JP", "点数单位", "点数单位"),
            // entity.standardOperationTime.pointsunit
            new TranslationSeedItem("entity.standardOperationTime.pointsunit", "zh-CN", "点数单位", "点数单位"),
            // entity.standardOperationTime.pointsunit
            new TranslationSeedItem("entity.standardOperationTime.pointsunit", "zh-HK", "点数单位", "点数单位"),

            // entity.standardOperationTime.pointstominutesrate
            new TranslationSeedItem("entity.standardOperationTime.pointstominutesrate", "en-US", "转换汇率", "点数转分钟汇率（1 点数 = 多少分钟）"),
            // entity.standardOperationTime.pointstominutesrate
            new TranslationSeedItem("entity.standardOperationTime.pointstominutesrate", "ja-JP", "转换汇率", "点数转分钟汇率（1 点数 = 多少分钟）"),
            // entity.standardOperationTime.pointstominutesrate
            new TranslationSeedItem("entity.standardOperationTime.pointstominutesrate", "zh-CN", "转换汇率", "点数转分钟汇率（1 点数 = 多少分钟）"),
            // entity.standardOperationTime.pointstominutesrate
            new TranslationSeedItem("entity.standardOperationTime.pointstominutesrate", "zh-HK", "转换汇率", "点数转分钟汇率（1 点数 = 多少分钟）"),

            // entity.standardOperationTime.convertedminutes
            new TranslationSeedItem("entity.standardOperationTime.convertedminutes", "en-US", "转换工时", "转换后标准工时（分钟）"),
            // entity.standardOperationTime.convertedminutes
            new TranslationSeedItem("entity.standardOperationTime.convertedminutes", "ja-JP", "转换工时", "转换后标准工时（分钟）"),
            // entity.standardOperationTime.convertedminutes
            new TranslationSeedItem("entity.standardOperationTime.convertedminutes", "zh-CN", "转换工时", "转换后标准工时（分钟）"),
            // entity.standardOperationTime.convertedminutes
            new TranslationSeedItem("entity.standardOperationTime.convertedminutes", "zh-HK", "转换工时", "转换后标准工时（分钟）"),

            // entity.standardOperationTime.effectivedate
            new TranslationSeedItem("entity.standardOperationTime.effectivedate", "en-US", "生效日期", "生效日期"),
            // entity.standardOperationTime.effectivedate
            new TranslationSeedItem("entity.standardOperationTime.effectivedate", "ja-JP", "生效日期", "生效日期"),
            // entity.standardOperationTime.effectivedate
            new TranslationSeedItem("entity.standardOperationTime.effectivedate", "zh-CN", "生效日期", "生效日期"),
            // entity.standardOperationTime.effectivedate
            new TranslationSeedItem("entity.standardOperationTime.effectivedate", "zh-HK", "生效日期", "生效日期"),

            // entity.standardOperationTime.expirydate
            new TranslationSeedItem("entity.standardOperationTime.expirydate", "en-US", "失效日期", "失效日期"),
            // entity.standardOperationTime.expirydate
            new TranslationSeedItem("entity.standardOperationTime.expirydate", "ja-JP", "失效日期", "失效日期"),
            // entity.standardOperationTime.expirydate
            new TranslationSeedItem("entity.standardOperationTime.expirydate", "zh-CN", "失效日期", "失效日期"),
            // entity.standardOperationTime.expirydate
            new TranslationSeedItem("entity.standardOperationTime.expirydate", "zh-HK", "失效日期", "失效日期"),
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
