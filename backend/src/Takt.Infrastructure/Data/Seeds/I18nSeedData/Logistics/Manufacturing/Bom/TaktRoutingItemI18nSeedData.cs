// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Bom
// 文件名称：TaktRoutingItemI18nSeedData.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktRoutingItem 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktRoutingItem 实体国际化翻译种子（键前缀 entity.routingItem.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktRoutingItemI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktRoutingItem 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 routingItem 实体翻译...", tenantCode);

        foreach (var item in GetRoutingItemTranslations())
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

        TaktLogger.Information("TaktRoutingItem 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktRoutingItem 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.routingItem._self / entity.routingItem.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetRoutingItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.routingItem._self
            new TranslationSeedItem("entity.routingItem._self", "en-US", "Routing Item Information", "实体名称"),
            // entity.routingItem._self
            new TranslationSeedItem("entity.routingItem._self", "ja-JP", "工艺路线明细表信息", "实体名称"),
            // entity.routingItem._self
            new TranslationSeedItem("entity.routingItem._self", "zh-CN", "工艺路线明细表信息", "实体名称"),
            // entity.routingItem._self
            new TranslationSeedItem("entity.routingItem._self", "zh-HK", "工艺路线明细表信息", "实体名称"),

            // entity.routingItem.routingid
            new TranslationSeedItem("entity.routingItem.routingid", "en-US", "工艺路线ID", "工艺路线主表ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.routingItem.routingid
            new TranslationSeedItem("entity.routingItem.routingid", "ja-JP", "工艺路线ID", "工艺路线主表ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.routingItem.routingid
            new TranslationSeedItem("entity.routingItem.routingid", "zh-CN", "工艺路线ID", "工艺路线主表ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.routingItem.routingid
            new TranslationSeedItem("entity.routingItem.routingid", "zh-HK", "工艺路线ID", "工艺路线主表ID（主子表关系，序列化为string以避免Javascript精度问题）"),

            // entity.routingItem.routingcode
            new TranslationSeedItem("entity.routingItem.routingcode", "en-US", "工艺路线编码", "工艺路线编码（冗余字段，便于查询）"),
            // entity.routingItem.routingcode
            new TranslationSeedItem("entity.routingItem.routingcode", "ja-JP", "工艺路线编码", "工艺路线编码（冗余字段，便于查询）"),
            // entity.routingItem.routingcode
            new TranslationSeedItem("entity.routingItem.routingcode", "zh-CN", "工艺路线编码", "工艺路线编码（冗余字段，便于查询）"),
            // entity.routingItem.routingcode
            new TranslationSeedItem("entity.routingItem.routingcode", "zh-HK", "工艺路线编码", "工艺路线编码（冗余字段，便于查询）"),

            // entity.routingItem.linenumber
            new TranslationSeedItem("entity.routingItem.linenumber", "en-US", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.routingItem.linenumber
            new TranslationSeedItem("entity.routingItem.linenumber", "ja-JP", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.routingItem.linenumber
            new TranslationSeedItem("entity.routingItem.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.routingItem.linenumber
            new TranslationSeedItem("entity.routingItem.linenumber", "zh-HK", "行号", "行号（项号/序号，固定步长=10）"),

            // entity.routingItem.baseunit
            new TranslationSeedItem("entity.routingItem.baseunit", "en-US", "计量单位", "作业/工序计量单位（PC或EA）"),
            // entity.routingItem.baseunit
            new TranslationSeedItem("entity.routingItem.baseunit", "ja-JP", "计量单位", "作业/工序计量单位（PC或EA）"),
            // entity.routingItem.baseunit
            new TranslationSeedItem("entity.routingItem.baseunit", "zh-CN", "计量单位", "作业/工序计量单位（PC或EA）"),
            // entity.routingItem.baseunit
            new TranslationSeedItem("entity.routingItem.baseunit", "zh-HK", "计量单位", "作业/工序计量单位（PC或EA）"),

            // entity.routingItem.basequantity
            new TranslationSeedItem("entity.routingItem.basequantity", "en-US", "基本数量", "基本数量"),
            // entity.routingItem.basequantity
            new TranslationSeedItem("entity.routingItem.basequantity", "ja-JP", "基本数量", "基本数量"),
            // entity.routingItem.basequantity
            new TranslationSeedItem("entity.routingItem.basequantity", "zh-CN", "基本数量", "基本数量"),
            // entity.routingItem.basequantity
            new TranslationSeedItem("entity.routingItem.basequantity", "zh-HK", "基本数量", "基本数量"),

            // entity.routingItem.standardminutes
            new TranslationSeedItem("entity.routingItem.standardminutes", "en-US", "标准工时", "标准工时（分钟）"),
            // entity.routingItem.standardminutes
            new TranslationSeedItem("entity.routingItem.standardminutes", "ja-JP", "标准工时", "标准工时（分钟）"),
            // entity.routingItem.standardminutes
            new TranslationSeedItem("entity.routingItem.standardminutes", "zh-CN", "标准工时", "标准工时（分钟）"),
            // entity.routingItem.standardminutes
            new TranslationSeedItem("entity.routingItem.standardminutes", "zh-HK", "标准工时", "标准工时（分钟）"),

            // entity.routingItem.timeunit
            new TranslationSeedItem("entity.routingItem.timeunit", "en-US", "工时单位", "工时单位"),
            // entity.routingItem.timeunit
            new TranslationSeedItem("entity.routingItem.timeunit", "ja-JP", "工时单位", "工时单位"),
            // entity.routingItem.timeunit
            new TranslationSeedItem("entity.routingItem.timeunit", "zh-CN", "工时单位", "工时单位"),
            // entity.routingItem.timeunit
            new TranslationSeedItem("entity.routingItem.timeunit", "zh-HK", "工时单位", "工时单位"),

            // entity.routingItem.standardshorts
            new TranslationSeedItem("entity.routingItem.standardshorts", "en-US", "标准点数", "标准点数"),
            // entity.routingItem.standardshorts
            new TranslationSeedItem("entity.routingItem.standardshorts", "ja-JP", "标准点数", "标准点数"),
            // entity.routingItem.standardshorts
            new TranslationSeedItem("entity.routingItem.standardshorts", "zh-CN", "标准点数", "标准点数"),
            // entity.routingItem.standardshorts
            new TranslationSeedItem("entity.routingItem.standardshorts", "zh-HK", "标准点数", "标准点数"),

            // entity.routingItem.pointsunit
            new TranslationSeedItem("entity.routingItem.pointsunit", "en-US", "点数单位", "点数单位"),
            // entity.routingItem.pointsunit
            new TranslationSeedItem("entity.routingItem.pointsunit", "ja-JP", "点数单位", "点数单位"),
            // entity.routingItem.pointsunit
            new TranslationSeedItem("entity.routingItem.pointsunit", "zh-CN", "点数单位", "点数单位"),
            // entity.routingItem.pointsunit
            new TranslationSeedItem("entity.routingItem.pointsunit", "zh-HK", "点数单位", "点数单位"),

            // entity.routingItem.pointstominutesrate
            new TranslationSeedItem("entity.routingItem.pointstominutesrate", "en-US", "转换汇率", "点数转分钟汇率（1 点数 = 多少分钟）"),
            // entity.routingItem.pointstominutesrate
            new TranslationSeedItem("entity.routingItem.pointstominutesrate", "ja-JP", "转换汇率", "点数转分钟汇率（1 点数 = 多少分钟）"),
            // entity.routingItem.pointstominutesrate
            new TranslationSeedItem("entity.routingItem.pointstominutesrate", "zh-CN", "转换汇率", "点数转分钟汇率（1 点数 = 多少分钟）"),
            // entity.routingItem.pointstominutesrate
            new TranslationSeedItem("entity.routingItem.pointstominutesrate", "zh-HK", "转换汇率", "点数转分钟汇率（1 点数 = 多少分钟）"),

            // entity.routingItem.convertedminutes
            new TranslationSeedItem("entity.routingItem.convertedminutes", "en-US", "转换工时", "转换后标准工时（分钟）"),
            // entity.routingItem.convertedminutes
            new TranslationSeedItem("entity.routingItem.convertedminutes", "ja-JP", "转换工时", "转换后标准工时（分钟）"),
            // entity.routingItem.convertedminutes
            new TranslationSeedItem("entity.routingItem.convertedminutes", "zh-CN", "转换工时", "转换后标准工时（分钟）"),
            // entity.routingItem.convertedminutes
            new TranslationSeedItem("entity.routingItem.convertedminutes", "zh-HK", "转换工时", "转换后标准工时（分钟）"),

            // entity.routingItem.setupminutes
            new TranslationSeedItem("entity.routingItem.setupminutes", "en-US", "准备时间", "准备时间（分钟），如换模、调试等"),
            // entity.routingItem.setupminutes
            new TranslationSeedItem("entity.routingItem.setupminutes", "ja-JP", "准备时间", "准备时间（分钟），如换模、调试等"),
            // entity.routingItem.setupminutes
            new TranslationSeedItem("entity.routingItem.setupminutes", "zh-CN", "准备时间", "准备时间（分钟），如换模、调试等"),
            // entity.routingItem.setupminutes
            new TranslationSeedItem("entity.routingItem.setupminutes", "zh-HK", "准备时间", "准备时间（分钟），如换模、调试等"),

            // entity.routingItem.teardownminutes
            new TranslationSeedItem("entity.routingItem.teardownminutes", "en-US", "清理时间", "清理时间（分钟），如清洁、整理等"),
            // entity.routingItem.teardownminutes
            new TranslationSeedItem("entity.routingItem.teardownminutes", "ja-JP", "清理时间", "清理时间（分钟），如清洁、整理等"),
            // entity.routingItem.teardownminutes
            new TranslationSeedItem("entity.routingItem.teardownminutes", "zh-CN", "清理时间", "清理时间（分钟），如清洁、整理等"),
            // entity.routingItem.teardownminutes
            new TranslationSeedItem("entity.routingItem.teardownminutes", "zh-HK", "清理时间", "清理时间（分钟），如清洁、整理等"),

            // entity.routingItem.isqualitycheck
            new TranslationSeedItem("entity.routingItem.isqualitycheck", "en-US", "是否质检点", "是否质量检验点"),
            // entity.routingItem.isqualitycheck
            new TranslationSeedItem("entity.routingItem.isqualitycheck", "ja-JP", "是否质检点", "是否质量检验点"),
            // entity.routingItem.isqualitycheck
            new TranslationSeedItem("entity.routingItem.isqualitycheck", "zh-CN", "是否质检点", "是否质量检验点"),
            // entity.routingItem.isqualitycheck
            new TranslationSeedItem("entity.routingItem.isqualitycheck", "zh-HK", "是否质检点", "是否质量检验点"),

            // entity.routingItem.sortorder
            new TranslationSeedItem("entity.routingItem.sortorder", "en-US", "排序号", "排序号"),
            // entity.routingItem.sortorder
            new TranslationSeedItem("entity.routingItem.sortorder", "ja-JP", "排序号", "排序号"),
            // entity.routingItem.sortorder
            new TranslationSeedItem("entity.routingItem.sortorder", "zh-CN", "排序号", "排序号"),
            // entity.routingItem.sortorder
            new TranslationSeedItem("entity.routingItem.sortorder", "zh-HK", "排序号", "排序号"),

            // entity.routingItem.processdescription
            new TranslationSeedItem("entity.routingItem.processdescription", "en-US", "工序说明", "工序说明"),
            // entity.routingItem.processdescription
            new TranslationSeedItem("entity.routingItem.processdescription", "ja-JP", "工序说明", "工序说明"),
            // entity.routingItem.processdescription
            new TranslationSeedItem("entity.routingItem.processdescription", "zh-CN", "工序说明", "工序说明"),
            // entity.routingItem.processdescription
            new TranslationSeedItem("entity.routingItem.processdescription", "zh-HK", "工序说明", "工序说明"),

            // entity.routingItem.routing
            new TranslationSeedItem("entity.routingItem.routing", "en-US", "工艺路线主表", "工艺路线主表（主表）"),
            // entity.routingItem.routing
            new TranslationSeedItem("entity.routingItem.routing", "ja-JP", "工艺路线主表", "工艺路线主表（主表）"),
            // entity.routingItem.routing
            new TranslationSeedItem("entity.routingItem.routing", "zh-CN", "工艺路线主表", "工艺路线主表（主表）"),
            // entity.routingItem.routing
            new TranslationSeedItem("entity.routingItem.routing", "zh-HK", "工艺路线主表", "工艺路线主表（主表）"),
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
