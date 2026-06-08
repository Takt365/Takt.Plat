// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Sales
// 文件名称：TaktSalesQuotationItemI18nSeedData.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSalesQuotationItem 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Sales;

/// <summary>
/// TaktSalesQuotationItem 实体国际化翻译种子（键前缀 entity.salesQuotationItem.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSalesQuotationItemI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSalesQuotationItem 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 salesQuotationItem 实体翻译...", tenantCode);

        foreach (var item in GetSalesQuotationItemTranslations())
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

        TaktLogger.Information("TaktSalesQuotationItem 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSalesQuotationItem 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.salesQuotationItem._self / entity.salesQuotationItem.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSalesQuotationItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.salesQuotationItem._self
            new TranslationSeedItem("entity.salesQuotationItem._self", "en-US", "Sales Quotation Item Information", "实体名称"),
            // entity.salesQuotationItem._self
            new TranslationSeedItem("entity.salesQuotationItem._self", "ja-JP", "Takt销售报价明细信息", "实体名称"),
            // entity.salesQuotationItem._self
            new TranslationSeedItem("entity.salesQuotationItem._self", "zh-CN", "Takt销售报价明细信息", "实体名称"),
            // entity.salesQuotationItem._self
            new TranslationSeedItem("entity.salesQuotationItem._self", "zh-HK", "Takt销售报价明细信息", "实体名称"),

            // entity.salesQuotationItem.salesquotationid
            new TranslationSeedItem("entity.salesQuotationItem.salesquotationid", "en-US", "销售报价ID", "销售报价ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.salesQuotationItem.salesquotationid
            new TranslationSeedItem("entity.salesQuotationItem.salesquotationid", "ja-JP", "销售报价ID", "销售报价ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.salesQuotationItem.salesquotationid
            new TranslationSeedItem("entity.salesQuotationItem.salesquotationid", "zh-CN", "销售报价ID", "销售报价ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.salesQuotationItem.salesquotationid
            new TranslationSeedItem("entity.salesQuotationItem.salesquotationid", "zh-HK", "销售报价ID", "销售报价ID（主子表关系，序列化为string以避免Javascript精度问题）"),

            // entity.salesQuotationItem.salesquotationcode
            new TranslationSeedItem("entity.salesQuotationItem.salesquotationcode", "en-US", "销售报价编码", "销售报价编码（冗余字段，便于查询）"),
            // entity.salesQuotationItem.salesquotationcode
            new TranslationSeedItem("entity.salesQuotationItem.salesquotationcode", "ja-JP", "销售报价编码", "销售报价编码（冗余字段，便于查询）"),
            // entity.salesQuotationItem.salesquotationcode
            new TranslationSeedItem("entity.salesQuotationItem.salesquotationcode", "zh-CN", "销售报价编码", "销售报价编码（冗余字段，便于查询）"),
            // entity.salesQuotationItem.salesquotationcode
            new TranslationSeedItem("entity.salesQuotationItem.salesquotationcode", "zh-HK", "销售报价编码", "销售报价编码（冗余字段，便于查询）"),

            // entity.salesQuotationItem.linenumber
            new TranslationSeedItem("entity.salesQuotationItem.linenumber", "en-US", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.salesQuotationItem.linenumber
            new TranslationSeedItem("entity.salesQuotationItem.linenumber", "ja-JP", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.salesQuotationItem.linenumber
            new TranslationSeedItem("entity.salesQuotationItem.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.salesQuotationItem.linenumber
            new TranslationSeedItem("entity.salesQuotationItem.linenumber", "zh-HK", "行号", "行号（项号/序号，固定步长=10）"),

            // entity.salesQuotationItem.materialcode
            new TranslationSeedItem("entity.salesQuotationItem.materialcode", "en-US", "物料编码", "物料编码"),
            // entity.salesQuotationItem.materialcode
            new TranslationSeedItem("entity.salesQuotationItem.materialcode", "ja-JP", "物料编码", "物料编码"),
            // entity.salesQuotationItem.materialcode
            new TranslationSeedItem("entity.salesQuotationItem.materialcode", "zh-CN", "物料编码", "物料编码"),
            // entity.salesQuotationItem.materialcode
            new TranslationSeedItem("entity.salesQuotationItem.materialcode", "zh-HK", "物料编码", "物料编码"),

            // entity.salesQuotationItem.materialname
            new TranslationSeedItem("entity.salesQuotationItem.materialname", "en-US", "物料名称", "物料名称"),
            // entity.salesQuotationItem.materialname
            new TranslationSeedItem("entity.salesQuotationItem.materialname", "ja-JP", "物料名称", "物料名称"),
            // entity.salesQuotationItem.materialname
            new TranslationSeedItem("entity.salesQuotationItem.materialname", "zh-CN", "物料名称", "物料名称"),
            // entity.salesQuotationItem.materialname
            new TranslationSeedItem("entity.salesQuotationItem.materialname", "zh-HK", "物料名称", "物料名称"),

            // entity.salesQuotationItem.materialspecification
            new TranslationSeedItem("entity.salesQuotationItem.materialspecification", "en-US", "物料规格", "物料规格"),
            // entity.salesQuotationItem.materialspecification
            new TranslationSeedItem("entity.salesQuotationItem.materialspecification", "ja-JP", "物料规格", "物料规格"),
            // entity.salesQuotationItem.materialspecification
            new TranslationSeedItem("entity.salesQuotationItem.materialspecification", "zh-CN", "物料规格", "物料规格"),
            // entity.salesQuotationItem.materialspecification
            new TranslationSeedItem("entity.salesQuotationItem.materialspecification", "zh-HK", "物料规格", "物料规格"),

            // entity.salesQuotationItem.salesunit
            new TranslationSeedItem("entity.salesQuotationItem.salesunit", "en-US", "销售单位", "销售单位"),
            // entity.salesQuotationItem.salesunit
            new TranslationSeedItem("entity.salesQuotationItem.salesunit", "ja-JP", "销售单位", "销售单位"),
            // entity.salesQuotationItem.salesunit
            new TranslationSeedItem("entity.salesQuotationItem.salesunit", "zh-CN", "销售单位", "销售单位"),
            // entity.salesQuotationItem.salesunit
            new TranslationSeedItem("entity.salesQuotationItem.salesunit", "zh-HK", "销售单位", "销售单位"),

            // entity.salesQuotationItem.quotationquantity
            new TranslationSeedItem("entity.salesQuotationItem.quotationquantity", "en-US", "报价数量", "报价数量（基本单位数量）"),
            // entity.salesQuotationItem.quotationquantity
            new TranslationSeedItem("entity.salesQuotationItem.quotationquantity", "ja-JP", "报价数量", "报价数量（基本单位数量）"),
            // entity.salesQuotationItem.quotationquantity
            new TranslationSeedItem("entity.salesQuotationItem.quotationquantity", "zh-CN", "报价数量", "报价数量（基本单位数量）"),
            // entity.salesQuotationItem.quotationquantity
            new TranslationSeedItem("entity.salesQuotationItem.quotationquantity", "zh-HK", "报价数量", "报价数量（基本单位数量）"),

            // entity.salesQuotationItem.unitprice
            new TranslationSeedItem("entity.salesQuotationItem.unitprice", "en-US", "单价", "单价"),
            // entity.salesQuotationItem.unitprice
            new TranslationSeedItem("entity.salesQuotationItem.unitprice", "ja-JP", "单价", "单价"),
            // entity.salesQuotationItem.unitprice
            new TranslationSeedItem("entity.salesQuotationItem.unitprice", "zh-CN", "单价", "单价"),
            // entity.salesQuotationItem.unitprice
            new TranslationSeedItem("entity.salesQuotationItem.unitprice", "zh-HK", "单价", "单价"),

            // entity.salesQuotationItem.discountrate
            new TranslationSeedItem("entity.salesQuotationItem.discountrate", "en-US", "折扣率", "折扣率（0-100，表示折扣百分比）"),
            // entity.salesQuotationItem.discountrate
            new TranslationSeedItem("entity.salesQuotationItem.discountrate", "ja-JP", "折扣率", "折扣率（0-100，表示折扣百分比）"),
            // entity.salesQuotationItem.discountrate
            new TranslationSeedItem("entity.salesQuotationItem.discountrate", "zh-CN", "折扣率", "折扣率（0-100，表示折扣百分比）"),
            // entity.salesQuotationItem.discountrate
            new TranslationSeedItem("entity.salesQuotationItem.discountrate", "zh-HK", "折扣率", "折扣率（0-100，表示折扣百分比）"),

            // entity.salesQuotationItem.discountamount
            new TranslationSeedItem("entity.salesQuotationItem.discountamount", "en-US", "折扣金额", "折扣金额"),
            // entity.salesQuotationItem.discountamount
            new TranslationSeedItem("entity.salesQuotationItem.discountamount", "ja-JP", "折扣金额", "折扣金额"),
            // entity.salesQuotationItem.discountamount
            new TranslationSeedItem("entity.salesQuotationItem.discountamount", "zh-CN", "折扣金额", "折扣金额"),
            // entity.salesQuotationItem.discountamount
            new TranslationSeedItem("entity.salesQuotationItem.discountamount", "zh-HK", "折扣金额", "折扣金额"),

            // entity.salesQuotationItem.taxrate
            new TranslationSeedItem("entity.salesQuotationItem.taxrate", "en-US", "税费率", "税费率（0-100，表示税费百分比）"),
            // entity.salesQuotationItem.taxrate
            new TranslationSeedItem("entity.salesQuotationItem.taxrate", "ja-JP", "税费率", "税费率（0-100，表示税费百分比）"),
            // entity.salesQuotationItem.taxrate
            new TranslationSeedItem("entity.salesQuotationItem.taxrate", "zh-CN", "税费率", "税费率（0-100，表示税费百分比）"),
            // entity.salesQuotationItem.taxrate
            new TranslationSeedItem("entity.salesQuotationItem.taxrate", "zh-HK", "税费率", "税费率（0-100，表示税费百分比）"),

            // entity.salesQuotationItem.taxamount
            new TranslationSeedItem("entity.salesQuotationItem.taxamount", "en-US", "税费", "税费"),
            // entity.salesQuotationItem.taxamount
            new TranslationSeedItem("entity.salesQuotationItem.taxamount", "ja-JP", "税费", "税费"),
            // entity.salesQuotationItem.taxamount
            new TranslationSeedItem("entity.salesQuotationItem.taxamount", "zh-CN", "税费", "税费"),
            // entity.salesQuotationItem.taxamount
            new TranslationSeedItem("entity.salesQuotationItem.taxamount", "zh-HK", "税费", "税费"),

            // entity.salesQuotationItem.subtotalamount
            new TranslationSeedItem("entity.salesQuotationItem.subtotalamount", "en-US", "小计金额", "小计金额"),
            // entity.salesQuotationItem.subtotalamount
            new TranslationSeedItem("entity.salesQuotationItem.subtotalamount", "ja-JP", "小计金额", "小计金额"),
            // entity.salesQuotationItem.subtotalamount
            new TranslationSeedItem("entity.salesQuotationItem.subtotalamount", "zh-CN", "小计金额", "小计金额"),
            // entity.salesQuotationItem.subtotalamount
            new TranslationSeedItem("entity.salesQuotationItem.subtotalamount", "zh-HK", "小计金额", "小计金额"),
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
