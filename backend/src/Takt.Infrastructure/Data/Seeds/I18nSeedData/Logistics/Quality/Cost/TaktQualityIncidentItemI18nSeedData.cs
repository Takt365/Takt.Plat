// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Cost
// 文件名称：TaktQualityIncidentItemI18nSeedData.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktQualityIncidentItem 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktQualityIncidentItem 实体国际化翻译种子（键前缀 entity.qualityIncidentItem.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktQualityIncidentItemI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktQualityIncidentItem 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 qualityIncidentItem 实体翻译...", tenantCode);

        foreach (var item in GetQualityIncidentItemTranslations())
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

        TaktLogger.Information("TaktQualityIncidentItem 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktQualityIncidentItem 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.qualityIncidentItem._self / entity.qualityIncidentItem.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetQualityIncidentItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.qualityIncidentItem._self
            new TranslationSeedItem("entity.qualityIncidentItem._self", "en-US", "Quality Incident Item Information", "实体名称"),
            // entity.qualityIncidentItem._self
            new TranslationSeedItem("entity.qualityIncidentItem._self", "ja-JP", "品质事故明细 - 废弃零件明细行信息", "实体名称"),
            // entity.qualityIncidentItem._self
            new TranslationSeedItem("entity.qualityIncidentItem._self", "zh-CN", "品质事故明细 - 废弃零件明细行信息", "实体名称"),
            // entity.qualityIncidentItem._self
            new TranslationSeedItem("entity.qualityIncidentItem._self", "zh-HK", "品质事故明细 - 废弃零件明细行信息", "实体名称"),

            // entity.qualityIncidentItem.qualityincidentid
            new TranslationSeedItem("entity.qualityIncidentItem.qualityincidentid", "en-US", "品质事故主表ID", "品质事故主表ID(主子表关系,序列化为string以避免Javascript精度问题)"),
            // entity.qualityIncidentItem.qualityincidentid
            new TranslationSeedItem("entity.qualityIncidentItem.qualityincidentid", "ja-JP", "品质事故主表ID", "品质事故主表ID(主子表关系,序列化为string以避免Javascript精度问题)"),
            // entity.qualityIncidentItem.qualityincidentid
            new TranslationSeedItem("entity.qualityIncidentItem.qualityincidentid", "zh-CN", "品质事故主表ID", "品质事故主表ID(主子表关系,序列化为string以避免Javascript精度问题)"),
            // entity.qualityIncidentItem.qualityincidentid
            new TranslationSeedItem("entity.qualityIncidentItem.qualityincidentid", "zh-HK", "品质事故主表ID", "品质事故主表ID(主子表关系,序列化为string以避免Javascript精度问题)"),

            // entity.qualityIncidentItem.qualityincidentcode
            new TranslationSeedItem("entity.qualityIncidentItem.qualityincidentcode", "en-US", "品质事故编码", "品质事故编码（冗余字段，便于查询）"),
            // entity.qualityIncidentItem.qualityincidentcode
            new TranslationSeedItem("entity.qualityIncidentItem.qualityincidentcode", "ja-JP", "品质事故编码", "品质事故编码（冗余字段，便于查询）"),
            // entity.qualityIncidentItem.qualityincidentcode
            new TranslationSeedItem("entity.qualityIncidentItem.qualityincidentcode", "zh-CN", "品质事故编码", "品质事故编码（冗余字段，便于查询）"),
            // entity.qualityIncidentItem.qualityincidentcode
            new TranslationSeedItem("entity.qualityIncidentItem.qualityincidentcode", "zh-HK", "品质事故编码", "品质事故编码（冗余字段，便于查询）"),

            // entity.qualityIncidentItem.linenumber
            new TranslationSeedItem("entity.qualityIncidentItem.linenumber", "en-US", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.qualityIncidentItem.linenumber
            new TranslationSeedItem("entity.qualityIncidentItem.linenumber", "ja-JP", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.qualityIncidentItem.linenumber
            new TranslationSeedItem("entity.qualityIncidentItem.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.qualityIncidentItem.linenumber
            new TranslationSeedItem("entity.qualityIncidentItem.linenumber", "zh-HK", "行号", "行号（项号/序号，固定步长=10）"),

            // entity.qualityIncidentItem.materialcode
            new TranslationSeedItem("entity.qualityIncidentItem.materialcode", "en-US", "物料编码", "物料编码"),
            // entity.qualityIncidentItem.materialcode
            new TranslationSeedItem("entity.qualityIncidentItem.materialcode", "ja-JP", "物料编码", "物料编码"),
            // entity.qualityIncidentItem.materialcode
            new TranslationSeedItem("entity.qualityIncidentItem.materialcode", "zh-CN", "物料编码", "物料编码"),
            // entity.qualityIncidentItem.materialcode
            new TranslationSeedItem("entity.qualityIncidentItem.materialcode", "zh-HK", "物料编码", "物料编码"),

            // entity.qualityIncidentItem.materialname
            new TranslationSeedItem("entity.qualityIncidentItem.materialname", "en-US", "物料名称", "物料名称"),
            // entity.qualityIncidentItem.materialname
            new TranslationSeedItem("entity.qualityIncidentItem.materialname", "ja-JP", "物料名称", "物料名称"),
            // entity.qualityIncidentItem.materialname
            new TranslationSeedItem("entity.qualityIncidentItem.materialname", "zh-CN", "物料名称", "物料名称"),
            // entity.qualityIncidentItem.materialname
            new TranslationSeedItem("entity.qualityIncidentItem.materialname", "zh-HK", "物料名称", "物料名称"),

            // entity.qualityIncidentItem.scrapcost
            new TranslationSeedItem("entity.qualityIncidentItem.scrapcost", "en-US", "废弃费用", "废弃费用(元)"),
            // entity.qualityIncidentItem.scrapcost
            new TranslationSeedItem("entity.qualityIncidentItem.scrapcost", "ja-JP", "废弃费用", "废弃费用(元)"),
            // entity.qualityIncidentItem.scrapcost
            new TranslationSeedItem("entity.qualityIncidentItem.scrapcost", "zh-CN", "废弃费用", "废弃费用(元)"),
            // entity.qualityIncidentItem.scrapcost
            new TranslationSeedItem("entity.qualityIncidentItem.scrapcost", "zh-HK", "废弃费用", "废弃费用(元)"),

            // entity.qualityIncidentItem.scrapsize
            new TranslationSeedItem("entity.qualityIncidentItem.scrapsize", "en-US", "废弃数量", "废弃数量"),
            // entity.qualityIncidentItem.scrapsize
            new TranslationSeedItem("entity.qualityIncidentItem.scrapsize", "ja-JP", "废弃数量", "废弃数量"),
            // entity.qualityIncidentItem.scrapsize
            new TranslationSeedItem("entity.qualityIncidentItem.scrapsize", "zh-CN", "废弃数量", "废弃数量"),
            // entity.qualityIncidentItem.scrapsize
            new TranslationSeedItem("entity.qualityIncidentItem.scrapsize", "zh-HK", "废弃数量", "废弃数量"),

            // entity.qualityIncidentItem.partprice
            new TranslationSeedItem("entity.qualityIncidentItem.partprice", "en-US", "零件单价", "零件单价(元)"),
            // entity.qualityIncidentItem.partprice
            new TranslationSeedItem("entity.qualityIncidentItem.partprice", "ja-JP", "零件单价", "零件单价(元)"),
            // entity.qualityIncidentItem.partprice
            new TranslationSeedItem("entity.qualityIncidentItem.partprice", "zh-CN", "零件单价", "零件单价(元)"),
            // entity.qualityIncidentItem.partprice
            new TranslationSeedItem("entity.qualityIncidentItem.partprice", "zh-HK", "零件单价", "零件单价(元)"),

            // entity.qualityIncidentItem.scrapreasoncost
            new TranslationSeedItem("entity.qualityIncidentItem.scrapreasoncost", "en-US", "废弃处理费用", "废弃处理费用(元)"),
            // entity.qualityIncidentItem.scrapreasoncost
            new TranslationSeedItem("entity.qualityIncidentItem.scrapreasoncost", "ja-JP", "废弃处理费用", "废弃处理费用(元)"),
            // entity.qualityIncidentItem.scrapreasoncost
            new TranslationSeedItem("entity.qualityIncidentItem.scrapreasoncost", "zh-CN", "废弃处理费用", "废弃处理费用(元)"),
            // entity.qualityIncidentItem.scrapreasoncost
            new TranslationSeedItem("entity.qualityIncidentItem.scrapreasoncost", "zh-HK", "废弃处理费用", "废弃处理费用(元)"),

            // entity.qualityIncidentItem.freightcharges
            new TranslationSeedItem("entity.qualityIncidentItem.freightcharges", "en-US", "运费", "运费(元)"),
            // entity.qualityIncidentItem.freightcharges
            new TranslationSeedItem("entity.qualityIncidentItem.freightcharges", "ja-JP", "运费", "运费(元)"),
            // entity.qualityIncidentItem.freightcharges
            new TranslationSeedItem("entity.qualityIncidentItem.freightcharges", "zh-CN", "运费", "运费(元)"),
            // entity.qualityIncidentItem.freightcharges
            new TranslationSeedItem("entity.qualityIncidentItem.freightcharges", "zh-HK", "运费", "运费(元)"),

            // entity.qualityIncidentItem.otherexpenses
            new TranslationSeedItem("entity.qualityIncidentItem.otherexpenses", "en-US", "其他费用", "其他费用(元)"),
            // entity.qualityIncidentItem.otherexpenses
            new TranslationSeedItem("entity.qualityIncidentItem.otherexpenses", "ja-JP", "其他费用", "其他费用(元)"),
            // entity.qualityIncidentItem.otherexpenses
            new TranslationSeedItem("entity.qualityIncidentItem.otherexpenses", "zh-CN", "其他费用", "其他费用(元)"),
            // entity.qualityIncidentItem.otherexpenses
            new TranslationSeedItem("entity.qualityIncidentItem.otherexpenses", "zh-HK", "其他费用", "其他费用(元)"),

            // entity.qualityIncidentItem.reasonworktimeminutes
            new TranslationSeedItem("entity.qualityIncidentItem.reasonworktimeminutes", "en-US", "处理作业时间", "处理作业时间(分钟)"),
            // entity.qualityIncidentItem.reasonworktimeminutes
            new TranslationSeedItem("entity.qualityIncidentItem.reasonworktimeminutes", "ja-JP", "处理作业时间", "处理作业时间(分钟)"),
            // entity.qualityIncidentItem.reasonworktimeminutes
            new TranslationSeedItem("entity.qualityIncidentItem.reasonworktimeminutes", "zh-CN", "处理作业时间", "处理作业时间(分钟)"),
            // entity.qualityIncidentItem.reasonworktimeminutes
            new TranslationSeedItem("entity.qualityIncidentItem.reasonworktimeminutes", "zh-HK", "处理作业时间", "处理作业时间(分钟)"),

            // entity.qualityIncidentItem.tax
            new TranslationSeedItem("entity.qualityIncidentItem.tax", "en-US", "关税", "关税(元)"),
            // entity.qualityIncidentItem.tax
            new TranslationSeedItem("entity.qualityIncidentItem.tax", "ja-JP", "关税", "关税(元)"),
            // entity.qualityIncidentItem.tax
            new TranslationSeedItem("entity.qualityIncidentItem.tax", "zh-CN", "关税", "关税(元)"),
            // entity.qualityIncidentItem.tax
            new TranslationSeedItem("entity.qualityIncidentItem.tax", "zh-HK", "关税", "关税(元)"),

            // entity.qualityIncidentItem.reasonotherexpenses
            new TranslationSeedItem("entity.qualityIncidentItem.reasonotherexpenses", "en-US", "处理发生其他费用", "处理发生其他费用(元)"),
            // entity.qualityIncidentItem.reasonotherexpenses
            new TranslationSeedItem("entity.qualityIncidentItem.reasonotherexpenses", "ja-JP", "处理发生其他费用", "处理发生其他费用(元)"),
            // entity.qualityIncidentItem.reasonotherexpenses
            new TranslationSeedItem("entity.qualityIncidentItem.reasonotherexpenses", "zh-CN", "处理发生其他费用", "处理发生其他费用(元)"),
            // entity.qualityIncidentItem.reasonotherexpenses
            new TranslationSeedItem("entity.qualityIncidentItem.reasonotherexpenses", "zh-HK", "处理发生其他费用", "处理发生其他费用(元)"),

            // entity.qualityIncidentItem.scrapnote
            new TranslationSeedItem("entity.qualityIncidentItem.scrapnote", "en-US", "废弃备注", "废弃备注"),
            // entity.qualityIncidentItem.scrapnote
            new TranslationSeedItem("entity.qualityIncidentItem.scrapnote", "ja-JP", "废弃备注", "废弃备注"),
            // entity.qualityIncidentItem.scrapnote
            new TranslationSeedItem("entity.qualityIncidentItem.scrapnote", "zh-CN", "废弃备注", "废弃备注"),
            // entity.qualityIncidentItem.scrapnote
            new TranslationSeedItem("entity.qualityIncidentItem.scrapnote", "zh-HK", "废弃备注", "废弃备注"),
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
