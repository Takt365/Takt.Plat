// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Sales
// 文件名称：TaktSalesPriceItemI18nSeedData.cs
// 创建时间：2026-06-12
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSalesPriceItem 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Sales;

/// <summary>
/// TaktSalesPriceItem 实体国际化翻译种子（键前缀 entity.salespriceitem.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSalesPriceItemI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSalesPriceItem 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 salespriceitem 实体翻译...", tenantCode);

        foreach (var item in GetSalesPriceItemTranslations())
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

        TaktLogger.Information("TaktSalesPriceItem 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSalesPriceItem 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.salespriceitem._self / entity.salespriceitem.{{field}}；ResourceGroup=4；ResourceType=0
    /// </summary>
    private static List<TranslationSeedItem> GetSalesPriceItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.salespriceitem._self
            new TranslationSeedItem("entity.salespriceitem._self", "en-US", "Sales Price Item Information", "实体名称"),
            // entity.salespriceitem._self
            new TranslationSeedItem("entity.salespriceitem._self", "ja-JP", "Takt销售价格明细信息", "实体名称"),
            // entity.salespriceitem._self
            new TranslationSeedItem("entity.salespriceitem._self", "zh-CN", "Takt销售价格明细信息", "实体名称"),
            // entity.salespriceitem._self
            new TranslationSeedItem("entity.salespriceitem._self", "zh-HK", "Takt销售价格明细信息", "实体名称"),

            // entity.salespriceitem.salespriceid
            new TranslationSeedItem("entity.salespriceitem.salespriceid", "en-US", "销售价格ID", "销售价格ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.salespriceitem.salespriceid
            new TranslationSeedItem("entity.salespriceitem.salespriceid", "ja-JP", "销售价格ID", "销售价格ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.salespriceitem.salespriceid
            new TranslationSeedItem("entity.salespriceitem.salespriceid", "zh-CN", "销售价格ID", "销售价格ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.salespriceitem.salespriceid
            new TranslationSeedItem("entity.salespriceitem.salespriceid", "zh-HK", "销售价格ID", "销售价格ID（主子表关系，序列化为string以避免Javascript精度问题）"),

            // entity.salespriceitem.salespricecode
            new TranslationSeedItem("entity.salespriceitem.salespricecode", "en-US", "销售价格编码", "销售价格编码（冗余字段，便于查询）"),
            // entity.salespriceitem.salespricecode
            new TranslationSeedItem("entity.salespriceitem.salespricecode", "ja-JP", "销售价格编码", "销售价格编码（冗余字段，便于查询）"),
            // entity.salespriceitem.salespricecode
            new TranslationSeedItem("entity.salespriceitem.salespricecode", "zh-CN", "销售价格编码", "销售价格编码（冗余字段，便于查询）"),
            // entity.salespriceitem.salespricecode
            new TranslationSeedItem("entity.salespriceitem.salespricecode", "zh-HK", "销售价格编码", "销售价格编码（冗余字段，便于查询）"),

            // entity.salespriceitem.linenumber
            new TranslationSeedItem("entity.salespriceitem.linenumber", "en-US", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.salespriceitem.linenumber
            new TranslationSeedItem("entity.salespriceitem.linenumber", "ja-JP", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.salespriceitem.linenumber
            new TranslationSeedItem("entity.salespriceitem.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.salespriceitem.linenumber
            new TranslationSeedItem("entity.salespriceitem.linenumber", "zh-HK", "行号", "行号（项号/序号，固定步长=10）"),

            // entity.salespriceitem.materialcode
            new TranslationSeedItem("entity.salespriceitem.materialcode", "en-US", "物料编码", "物料编码"),
            // entity.salespriceitem.materialcode
            new TranslationSeedItem("entity.salespriceitem.materialcode", "ja-JP", "物料编码", "物料编码"),
            // entity.salespriceitem.materialcode
            new TranslationSeedItem("entity.salespriceitem.materialcode", "zh-CN", "物料编码", "物料编码"),
            // entity.salespriceitem.materialcode
            new TranslationSeedItem("entity.salespriceitem.materialcode", "zh-HK", "物料编码", "物料编码"),

            // entity.salespriceitem.salesunit
            new TranslationSeedItem("entity.salespriceitem.salesunit", "en-US", "销售单位", "销售单位"),
            // entity.salespriceitem.salesunit
            new TranslationSeedItem("entity.salespriceitem.salesunit", "ja-JP", "销售单位", "销售单位"),
            // entity.salespriceitem.salesunit
            new TranslationSeedItem("entity.salespriceitem.salesunit", "zh-CN", "销售单位", "销售单位"),
            // entity.salespriceitem.salesunit
            new TranslationSeedItem("entity.salespriceitem.salesunit", "zh-HK", "销售单位", "销售单位"),

            // entity.salespriceitem.salesprice
            new TranslationSeedItem("entity.salespriceitem.salesprice", "en-US", "销售价格", "销售价格（精确到分，存储为整数，单位为分）"),
            // entity.salespriceitem.salesprice
            new TranslationSeedItem("entity.salespriceitem.salesprice", "ja-JP", "销售价格", "销售价格（精确到分，存储为整数，单位为分）"),
            // entity.salespriceitem.salesprice
            new TranslationSeedItem("entity.salespriceitem.salesprice", "zh-CN", "销售价格", "销售价格（精确到分，存储为整数，单位为分）"),
            // entity.salespriceitem.salesprice
            new TranslationSeedItem("entity.salespriceitem.salesprice", "zh-HK", "销售价格", "销售价格（精确到分，存储为整数，单位为分）"),

            // entity.salespriceitem.minorderquantity
            new TranslationSeedItem("entity.salespriceitem.minorderquantity", "en-US", "最小订购量", "最小订购量（基本单位数量）"),
            // entity.salespriceitem.minorderquantity
            new TranslationSeedItem("entity.salespriceitem.minorderquantity", "ja-JP", "最小订购量", "最小订购量（基本单位数量）"),
            // entity.salespriceitem.minorderquantity
            new TranslationSeedItem("entity.salespriceitem.minorderquantity", "zh-CN", "最小订购量", "最小订购量（基本单位数量）"),
            // entity.salespriceitem.minorderquantity
            new TranslationSeedItem("entity.salespriceitem.minorderquantity", "zh-HK", "最小订购量", "最小订购量（基本单位数量）"),

            // entity.salespriceitem.maxorderquantity
            new TranslationSeedItem("entity.salespriceitem.maxorderquantity", "en-US", "最大订购量", "最大订购量（基本单位数量，0表示无限制）"),
            // entity.salespriceitem.maxorderquantity
            new TranslationSeedItem("entity.salespriceitem.maxorderquantity", "ja-JP", "最大订购量", "最大订购量（基本单位数量，0表示无限制）"),
            // entity.salespriceitem.maxorderquantity
            new TranslationSeedItem("entity.salespriceitem.maxorderquantity", "zh-CN", "最大订购量", "最大订购量（基本单位数量，0表示无限制）"),
            // entity.salespriceitem.maxorderquantity
            new TranslationSeedItem("entity.salespriceitem.maxorderquantity", "zh-HK", "最大订购量", "最大订购量（基本单位数量，0表示无限制）"),

            // entity.salespriceitem.scales
            new TranslationSeedItem("entity.salespriceitem.scales", "en-US", "价格阶梯列表", "价格阶梯列表（主子表关系，一个物料价格可以有多个阶梯）"),
            // entity.salespriceitem.scales
            new TranslationSeedItem("entity.salespriceitem.scales", "ja-JP", "价格阶梯列表", "价格阶梯列表（主子表关系，一个物料价格可以有多个阶梯）"),
            // entity.salespriceitem.scales
            new TranslationSeedItem("entity.salespriceitem.scales", "zh-CN", "价格阶梯列表", "价格阶梯列表（主子表关系，一个物料价格可以有多个阶梯）"),
            // entity.salespriceitem.scales
            new TranslationSeedItem("entity.salespriceitem.scales", "zh-HK", "价格阶梯列表", "价格阶梯列表（主子表关系，一个物料价格可以有多个阶梯）"),

            // entity.salespriceitem.price
            new TranslationSeedItem("entity.salespriceitem.price", "en-US", "销售价格", "销售价格（主表）"),
            // entity.salespriceitem.price
            new TranslationSeedItem("entity.salespriceitem.price", "ja-JP", "销售价格", "销售价格（主表）"),
            // entity.salespriceitem.price
            new TranslationSeedItem("entity.salespriceitem.price", "zh-CN", "销售价格", "销售价格（主表）"),
            // entity.salespriceitem.price
            new TranslationSeedItem("entity.salespriceitem.price", "zh-HK", "销售价格", "销售价格（主表）"),
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
