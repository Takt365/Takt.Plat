// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Sales
// 文件名称：TaktSalesQuotationItemI18nSeedData.cs
// 创建时间：2026-06-12
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
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Sales;

/// <summary>
/// TaktSalesQuotationItem 实体国际化翻译种子（键前缀 entity.salesquotationitem.*）
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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 salesquotationitem 实体翻译...", tenantCode);

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
    /// I18nKey：entity.salesquotationitem._self / entity.salesquotationitem.{{field}}；ResourceGroup=4；ResourceType=0
    /// </summary>
    private static List<TranslationSeedItem> GetSalesQuotationItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.salesquotationitem._self
            new TranslationSeedItem("entity.salesquotationitem._self", "en-US", "Sales Quotation Item Information", "实体名称"),
            // entity.salesquotationitem._self
            new TranslationSeedItem("entity.salesquotationitem._self", "ja-JP", "Takt销售报价明细信息", "实体名称"),
            // entity.salesquotationitem._self
            new TranslationSeedItem("entity.salesquotationitem._self", "zh-CN", "Takt销售报价明细信息", "实体名称"),
            // entity.salesquotationitem._self
            new TranslationSeedItem("entity.salesquotationitem._self", "zh-HK", "Takt销售报价明细信息", "实体名称"),

            // entity.salesquotationitem.salesquotationid
            new TranslationSeedItem("entity.salesquotationitem.salesquotationid", "en-US", "销售报价ID", "销售报价ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.salesquotationitem.salesquotationid
            new TranslationSeedItem("entity.salesquotationitem.salesquotationid", "ja-JP", "销售报价ID", "销售报价ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.salesquotationitem.salesquotationid
            new TranslationSeedItem("entity.salesquotationitem.salesquotationid", "zh-CN", "销售报价ID", "销售报价ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.salesquotationitem.salesquotationid
            new TranslationSeedItem("entity.salesquotationitem.salesquotationid", "zh-HK", "销售报价ID", "销售报价ID（主子表关系，序列化为string以避免Javascript精度问题）"),

            // entity.salesquotationitem.salesquotationcode
            new TranslationSeedItem("entity.salesquotationitem.salesquotationcode", "en-US", "销售报价编码", "销售报价编码（冗余字段，便于查询）"),
            // entity.salesquotationitem.salesquotationcode
            new TranslationSeedItem("entity.salesquotationitem.salesquotationcode", "ja-JP", "销售报价编码", "销售报价编码（冗余字段，便于查询）"),
            // entity.salesquotationitem.salesquotationcode
            new TranslationSeedItem("entity.salesquotationitem.salesquotationcode", "zh-CN", "销售报价编码", "销售报价编码（冗余字段，便于查询）"),
            // entity.salesquotationitem.salesquotationcode
            new TranslationSeedItem("entity.salesquotationitem.salesquotationcode", "zh-HK", "销售报价编码", "销售报价编码（冗余字段，便于查询）"),

            // entity.salesquotationitem.linenumber
            new TranslationSeedItem("entity.salesquotationitem.linenumber", "en-US", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.salesquotationitem.linenumber
            new TranslationSeedItem("entity.salesquotationitem.linenumber", "ja-JP", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.salesquotationitem.linenumber
            new TranslationSeedItem("entity.salesquotationitem.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.salesquotationitem.linenumber
            new TranslationSeedItem("entity.salesquotationitem.linenumber", "zh-HK", "行号", "行号（项号/序号，固定步长=10）"),

            // entity.salesquotationitem.materialcode
            new TranslationSeedItem("entity.salesquotationitem.materialcode", "en-US", "物料编码", "物料编码"),
            // entity.salesquotationitem.materialcode
            new TranslationSeedItem("entity.salesquotationitem.materialcode", "ja-JP", "物料编码", "物料编码"),
            // entity.salesquotationitem.materialcode
            new TranslationSeedItem("entity.salesquotationitem.materialcode", "zh-CN", "物料编码", "物料编码"),
            // entity.salesquotationitem.materialcode
            new TranslationSeedItem("entity.salesquotationitem.materialcode", "zh-HK", "物料编码", "物料编码"),

            // entity.salesquotationitem.materialname
            new TranslationSeedItem("entity.salesquotationitem.materialname", "en-US", "物料名称", "物料名称"),
            // entity.salesquotationitem.materialname
            new TranslationSeedItem("entity.salesquotationitem.materialname", "ja-JP", "物料名称", "物料名称"),
            // entity.salesquotationitem.materialname
            new TranslationSeedItem("entity.salesquotationitem.materialname", "zh-CN", "物料名称", "物料名称"),
            // entity.salesquotationitem.materialname
            new TranslationSeedItem("entity.salesquotationitem.materialname", "zh-HK", "物料名称", "物料名称"),

            // entity.salesquotationitem.materialspecification
            new TranslationSeedItem("entity.salesquotationitem.materialspecification", "en-US", "物料规格", "物料规格"),
            // entity.salesquotationitem.materialspecification
            new TranslationSeedItem("entity.salesquotationitem.materialspecification", "ja-JP", "物料规格", "物料规格"),
            // entity.salesquotationitem.materialspecification
            new TranslationSeedItem("entity.salesquotationitem.materialspecification", "zh-CN", "物料规格", "物料规格"),
            // entity.salesquotationitem.materialspecification
            new TranslationSeedItem("entity.salesquotationitem.materialspecification", "zh-HK", "物料规格", "物料规格"),

            // entity.salesquotationitem.salesunit
            new TranslationSeedItem("entity.salesquotationitem.salesunit", "en-US", "销售单位", "销售单位"),
            // entity.salesquotationitem.salesunit
            new TranslationSeedItem("entity.salesquotationitem.salesunit", "ja-JP", "销售单位", "销售单位"),
            // entity.salesquotationitem.salesunit
            new TranslationSeedItem("entity.salesquotationitem.salesunit", "zh-CN", "销售单位", "销售单位"),
            // entity.salesquotationitem.salesunit
            new TranslationSeedItem("entity.salesquotationitem.salesunit", "zh-HK", "销售单位", "销售单位"),

            // entity.salesquotationitem.quotationquantity
            new TranslationSeedItem("entity.salesquotationitem.quotationquantity", "en-US", "报价数量", "报价数量（基本单位数量）"),
            // entity.salesquotationitem.quotationquantity
            new TranslationSeedItem("entity.salesquotationitem.quotationquantity", "ja-JP", "报价数量", "报价数量（基本单位数量）"),
            // entity.salesquotationitem.quotationquantity
            new TranslationSeedItem("entity.salesquotationitem.quotationquantity", "zh-CN", "报价数量", "报价数量（基本单位数量）"),
            // entity.salesquotationitem.quotationquantity
            new TranslationSeedItem("entity.salesquotationitem.quotationquantity", "zh-HK", "报价数量", "报价数量（基本单位数量）"),

            // entity.salesquotationitem.unitprice
            new TranslationSeedItem("entity.salesquotationitem.unitprice", "en-US", "单价", "单价"),
            // entity.salesquotationitem.unitprice
            new TranslationSeedItem("entity.salesquotationitem.unitprice", "ja-JP", "单价", "单价"),
            // entity.salesquotationitem.unitprice
            new TranslationSeedItem("entity.salesquotationitem.unitprice", "zh-CN", "单价", "单价"),
            // entity.salesquotationitem.unitprice
            new TranslationSeedItem("entity.salesquotationitem.unitprice", "zh-HK", "单价", "单价"),

            // entity.salesquotationitem.discountrate
            new TranslationSeedItem("entity.salesquotationitem.discountrate", "en-US", "折扣率", "折扣率（0-100，表示折扣百分比）"),
            // entity.salesquotationitem.discountrate
            new TranslationSeedItem("entity.salesquotationitem.discountrate", "ja-JP", "折扣率", "折扣率（0-100，表示折扣百分比）"),
            // entity.salesquotationitem.discountrate
            new TranslationSeedItem("entity.salesquotationitem.discountrate", "zh-CN", "折扣率", "折扣率（0-100，表示折扣百分比）"),
            // entity.salesquotationitem.discountrate
            new TranslationSeedItem("entity.salesquotationitem.discountrate", "zh-HK", "折扣率", "折扣率（0-100，表示折扣百分比）"),

            // entity.salesquotationitem.discountamount
            new TranslationSeedItem("entity.salesquotationitem.discountamount", "en-US", "折扣金额", "折扣金额"),
            // entity.salesquotationitem.discountamount
            new TranslationSeedItem("entity.salesquotationitem.discountamount", "ja-JP", "折扣金额", "折扣金额"),
            // entity.salesquotationitem.discountamount
            new TranslationSeedItem("entity.salesquotationitem.discountamount", "zh-CN", "折扣金额", "折扣金额"),
            // entity.salesquotationitem.discountamount
            new TranslationSeedItem("entity.salesquotationitem.discountamount", "zh-HK", "折扣金额", "折扣金额"),

            // entity.salesquotationitem.taxrate
            new TranslationSeedItem("entity.salesquotationitem.taxrate", "en-US", "税费率", "税费率（0-100，表示税费百分比）"),
            // entity.salesquotationitem.taxrate
            new TranslationSeedItem("entity.salesquotationitem.taxrate", "ja-JP", "税费率", "税费率（0-100，表示税费百分比）"),
            // entity.salesquotationitem.taxrate
            new TranslationSeedItem("entity.salesquotationitem.taxrate", "zh-CN", "税费率", "税费率（0-100，表示税费百分比）"),
            // entity.salesquotationitem.taxrate
            new TranslationSeedItem("entity.salesquotationitem.taxrate", "zh-HK", "税费率", "税费率（0-100，表示税费百分比）"),

            // entity.salesquotationitem.taxamount
            new TranslationSeedItem("entity.salesquotationitem.taxamount", "en-US", "税费", "税费"),
            // entity.salesquotationitem.taxamount
            new TranslationSeedItem("entity.salesquotationitem.taxamount", "ja-JP", "税费", "税费"),
            // entity.salesquotationitem.taxamount
            new TranslationSeedItem("entity.salesquotationitem.taxamount", "zh-CN", "税费", "税费"),
            // entity.salesquotationitem.taxamount
            new TranslationSeedItem("entity.salesquotationitem.taxamount", "zh-HK", "税费", "税费"),

            // entity.salesquotationitem.subtotalamount
            new TranslationSeedItem("entity.salesquotationitem.subtotalamount", "en-US", "小计金额", "小计金额"),
            // entity.salesquotationitem.subtotalamount
            new TranslationSeedItem("entity.salesquotationitem.subtotalamount", "ja-JP", "小计金额", "小计金额"),
            // entity.salesquotationitem.subtotalamount
            new TranslationSeedItem("entity.salesquotationitem.subtotalamount", "zh-CN", "小计金额", "小计金额"),
            // entity.salesquotationitem.subtotalamount
            new TranslationSeedItem("entity.salesquotationitem.subtotalamount", "zh-HK", "小计金额", "小计金额"),
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
