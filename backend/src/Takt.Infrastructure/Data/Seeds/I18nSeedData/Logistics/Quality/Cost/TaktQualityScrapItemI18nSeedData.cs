// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Cost
// 文件名称：TaktQualityScrapItemI18nSeedData.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktQualityScrapItem 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktQualityScrapItem 实体国际化翻译种子（键前缀 entity.qualityScrapItem.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktQualityScrapItemI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktQualityScrapItem 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 qualityScrapItem 实体翻译...", tenantCode);

        foreach (var item in GetQualityScrapItemTranslations())
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

        TaktLogger.Information("TaktQualityScrapItem 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktQualityScrapItem 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.qualityScrapItem._self / entity.qualityScrapItem.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetQualityScrapItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.qualityScrapItem._self
            new TranslationSeedItem("entity.qualityScrapItem._self", "en-US", "Quality Scrap Item Information", "实体名称"),
            // entity.qualityScrapItem._self
            new TranslationSeedItem("entity.qualityScrapItem._self", "ja-JP", "品质废弃明细 - 废弃零件明细行信息", "实体名称"),
            // entity.qualityScrapItem._self
            new TranslationSeedItem("entity.qualityScrapItem._self", "zh-CN", "品质废弃明细 - 废弃零件明细行信息", "实体名称"),
            // entity.qualityScrapItem._self
            new TranslationSeedItem("entity.qualityScrapItem._self", "zh-HK", "品质废弃明细 - 废弃零件明细行信息", "实体名称"),

            // entity.qualityScrapItem.qualityscrapid
            new TranslationSeedItem("entity.qualityScrapItem.qualityscrapid", "en-US", "品质废弃主表ID", "品质废弃主表ID(主子表关系,序列化为string以避免Javascript精度问题)"),
            // entity.qualityScrapItem.qualityscrapid
            new TranslationSeedItem("entity.qualityScrapItem.qualityscrapid", "ja-JP", "品质废弃主表ID", "品质废弃主表ID(主子表关系,序列化为string以避免Javascript精度问题)"),
            // entity.qualityScrapItem.qualityscrapid
            new TranslationSeedItem("entity.qualityScrapItem.qualityscrapid", "zh-CN", "品质废弃主表ID", "品质废弃主表ID(主子表关系,序列化为string以避免Javascript精度问题)"),
            // entity.qualityScrapItem.qualityscrapid
            new TranslationSeedItem("entity.qualityScrapItem.qualityscrapid", "zh-HK", "品质废弃主表ID", "品质废弃主表ID(主子表关系,序列化为string以避免Javascript精度问题)"),

            // entity.qualityScrapItem.qualityscrapcode
            new TranslationSeedItem("entity.qualityScrapItem.qualityscrapcode", "en-US", "品质废弃编码", "品质废弃编码（冗余字段，便于查询）"),
            // entity.qualityScrapItem.qualityscrapcode
            new TranslationSeedItem("entity.qualityScrapItem.qualityscrapcode", "ja-JP", "品质废弃编码", "品质废弃编码（冗余字段，便于查询）"),
            // entity.qualityScrapItem.qualityscrapcode
            new TranslationSeedItem("entity.qualityScrapItem.qualityscrapcode", "zh-CN", "品质废弃编码", "品质废弃编码（冗余字段，便于查询）"),
            // entity.qualityScrapItem.qualityscrapcode
            new TranslationSeedItem("entity.qualityScrapItem.qualityscrapcode", "zh-HK", "品质废弃编码", "品质废弃编码（冗余字段，便于查询）"),

            // entity.qualityScrapItem.linenumber
            new TranslationSeedItem("entity.qualityScrapItem.linenumber", "en-US", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.qualityScrapItem.linenumber
            new TranslationSeedItem("entity.qualityScrapItem.linenumber", "ja-JP", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.qualityScrapItem.linenumber
            new TranslationSeedItem("entity.qualityScrapItem.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.qualityScrapItem.linenumber
            new TranslationSeedItem("entity.qualityScrapItem.linenumber", "zh-HK", "行号", "行号（项号/序号，固定步长=10）"),

            // entity.qualityScrapItem.materialcode
            new TranslationSeedItem("entity.qualityScrapItem.materialcode", "en-US", "物料编码", "物料编码"),
            // entity.qualityScrapItem.materialcode
            new TranslationSeedItem("entity.qualityScrapItem.materialcode", "ja-JP", "物料编码", "物料编码"),
            // entity.qualityScrapItem.materialcode
            new TranslationSeedItem("entity.qualityScrapItem.materialcode", "zh-CN", "物料编码", "物料编码"),
            // entity.qualityScrapItem.materialcode
            new TranslationSeedItem("entity.qualityScrapItem.materialcode", "zh-HK", "物料编码", "物料编码"),

            // entity.qualityScrapItem.materialname
            new TranslationSeedItem("entity.qualityScrapItem.materialname", "en-US", "物料名称", "物料名称"),
            // entity.qualityScrapItem.materialname
            new TranslationSeedItem("entity.qualityScrapItem.materialname", "ja-JP", "物料名称", "物料名称"),
            // entity.qualityScrapItem.materialname
            new TranslationSeedItem("entity.qualityScrapItem.materialname", "zh-CN", "物料名称", "物料名称"),
            // entity.qualityScrapItem.materialname
            new TranslationSeedItem("entity.qualityScrapItem.materialname", "zh-HK", "物料名称", "物料名称"),

            // entity.qualityScrapItem.scrapcost
            new TranslationSeedItem("entity.qualityScrapItem.scrapcost", "en-US", "废弃费用", "废弃费用(元)"),
            // entity.qualityScrapItem.scrapcost
            new TranslationSeedItem("entity.qualityScrapItem.scrapcost", "ja-JP", "废弃费用", "废弃费用(元)"),
            // entity.qualityScrapItem.scrapcost
            new TranslationSeedItem("entity.qualityScrapItem.scrapcost", "zh-CN", "废弃费用", "废弃费用(元)"),
            // entity.qualityScrapItem.scrapcost
            new TranslationSeedItem("entity.qualityScrapItem.scrapcost", "zh-HK", "废弃费用", "废弃费用(元)"),

            // entity.qualityScrapItem.scrapsize
            new TranslationSeedItem("entity.qualityScrapItem.scrapsize", "en-US", "废弃数量", "废弃数量"),
            // entity.qualityScrapItem.scrapsize
            new TranslationSeedItem("entity.qualityScrapItem.scrapsize", "ja-JP", "废弃数量", "废弃数量"),
            // entity.qualityScrapItem.scrapsize
            new TranslationSeedItem("entity.qualityScrapItem.scrapsize", "zh-CN", "废弃数量", "废弃数量"),
            // entity.qualityScrapItem.scrapsize
            new TranslationSeedItem("entity.qualityScrapItem.scrapsize", "zh-HK", "废弃数量", "废弃数量"),

            // entity.qualityScrapItem.partprice
            new TranslationSeedItem("entity.qualityScrapItem.partprice", "en-US", "零件单价", "零件单价(元)"),
            // entity.qualityScrapItem.partprice
            new TranslationSeedItem("entity.qualityScrapItem.partprice", "ja-JP", "零件单价", "零件单价(元)"),
            // entity.qualityScrapItem.partprice
            new TranslationSeedItem("entity.qualityScrapItem.partprice", "zh-CN", "零件单价", "零件单价(元)"),
            // entity.qualityScrapItem.partprice
            new TranslationSeedItem("entity.qualityScrapItem.partprice", "zh-HK", "零件单价", "零件单价(元)"),

            // entity.qualityScrapItem.scrapreasoncost
            new TranslationSeedItem("entity.qualityScrapItem.scrapreasoncost", "en-US", "废弃处理费用", "废弃处理费用(元)"),
            // entity.qualityScrapItem.scrapreasoncost
            new TranslationSeedItem("entity.qualityScrapItem.scrapreasoncost", "ja-JP", "废弃处理费用", "废弃处理费用(元)"),
            // entity.qualityScrapItem.scrapreasoncost
            new TranslationSeedItem("entity.qualityScrapItem.scrapreasoncost", "zh-CN", "废弃处理费用", "废弃处理费用(元)"),
            // entity.qualityScrapItem.scrapreasoncost
            new TranslationSeedItem("entity.qualityScrapItem.scrapreasoncost", "zh-HK", "废弃处理费用", "废弃处理费用(元)"),

            // entity.qualityScrapItem.freightcharges
            new TranslationSeedItem("entity.qualityScrapItem.freightcharges", "en-US", "运费", "运费(元)"),
            // entity.qualityScrapItem.freightcharges
            new TranslationSeedItem("entity.qualityScrapItem.freightcharges", "ja-JP", "运费", "运费(元)"),
            // entity.qualityScrapItem.freightcharges
            new TranslationSeedItem("entity.qualityScrapItem.freightcharges", "zh-CN", "运费", "运费(元)"),
            // entity.qualityScrapItem.freightcharges
            new TranslationSeedItem("entity.qualityScrapItem.freightcharges", "zh-HK", "运费", "运费(元)"),

            // entity.qualityScrapItem.otherexpenses
            new TranslationSeedItem("entity.qualityScrapItem.otherexpenses", "en-US", "其他费用", "其他费用(元)"),
            // entity.qualityScrapItem.otherexpenses
            new TranslationSeedItem("entity.qualityScrapItem.otherexpenses", "ja-JP", "其他费用", "其他费用(元)"),
            // entity.qualityScrapItem.otherexpenses
            new TranslationSeedItem("entity.qualityScrapItem.otherexpenses", "zh-CN", "其他费用", "其他费用(元)"),
            // entity.qualityScrapItem.otherexpenses
            new TranslationSeedItem("entity.qualityScrapItem.otherexpenses", "zh-HK", "其他费用", "其他费用(元)"),

            // entity.qualityScrapItem.reasonworktimeminutes
            new TranslationSeedItem("entity.qualityScrapItem.reasonworktimeminutes", "en-US", "处理作业时间", "处理作业时间(分钟)"),
            // entity.qualityScrapItem.reasonworktimeminutes
            new TranslationSeedItem("entity.qualityScrapItem.reasonworktimeminutes", "ja-JP", "处理作业时间", "处理作业时间(分钟)"),
            // entity.qualityScrapItem.reasonworktimeminutes
            new TranslationSeedItem("entity.qualityScrapItem.reasonworktimeminutes", "zh-CN", "处理作业时间", "处理作业时间(分钟)"),
            // entity.qualityScrapItem.reasonworktimeminutes
            new TranslationSeedItem("entity.qualityScrapItem.reasonworktimeminutes", "zh-HK", "处理作业时间", "处理作业时间(分钟)"),

            // entity.qualityScrapItem.tax
            new TranslationSeedItem("entity.qualityScrapItem.tax", "en-US", "关税", "关税(元)"),
            // entity.qualityScrapItem.tax
            new TranslationSeedItem("entity.qualityScrapItem.tax", "ja-JP", "关税", "关税(元)"),
            // entity.qualityScrapItem.tax
            new TranslationSeedItem("entity.qualityScrapItem.tax", "zh-CN", "关税", "关税(元)"),
            // entity.qualityScrapItem.tax
            new TranslationSeedItem("entity.qualityScrapItem.tax", "zh-HK", "关税", "关税(元)"),

            // entity.qualityScrapItem.reasonotherexpenses
            new TranslationSeedItem("entity.qualityScrapItem.reasonotherexpenses", "en-US", "处理发生其他费用", "处理发生其他费用(元)"),
            // entity.qualityScrapItem.reasonotherexpenses
            new TranslationSeedItem("entity.qualityScrapItem.reasonotherexpenses", "ja-JP", "处理发生其他费用", "处理发生其他费用(元)"),
            // entity.qualityScrapItem.reasonotherexpenses
            new TranslationSeedItem("entity.qualityScrapItem.reasonotherexpenses", "zh-CN", "处理发生其他费用", "处理发生其他费用(元)"),
            // entity.qualityScrapItem.reasonotherexpenses
            new TranslationSeedItem("entity.qualityScrapItem.reasonotherexpenses", "zh-HK", "处理发生其他费用", "处理发生其他费用(元)"),

            // entity.qualityScrapItem.scrapnote
            new TranslationSeedItem("entity.qualityScrapItem.scrapnote", "en-US", "废弃备注", "废弃备注"),
            // entity.qualityScrapItem.scrapnote
            new TranslationSeedItem("entity.qualityScrapItem.scrapnote", "ja-JP", "废弃备注", "废弃备注"),
            // entity.qualityScrapItem.scrapnote
            new TranslationSeedItem("entity.qualityScrapItem.scrapnote", "zh-CN", "废弃备注", "废弃备注"),
            // entity.qualityScrapItem.scrapnote
            new TranslationSeedItem("entity.qualityScrapItem.scrapnote", "zh-HK", "废弃备注", "废弃备注"),
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
