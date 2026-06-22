// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Sales
// 文件名称：TaktSalesInvoiceItemI18nSeedData.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSalesInvoiceItem 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktSalesInvoiceItem 实体国际化翻译种子（键前缀 entity.salesinvoiceitem.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSalesInvoiceItemI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSalesInvoiceItem 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 salesinvoiceitem 实体翻译...", tenantCode);

        foreach (var item in GetSalesInvoiceItemTranslations())
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

        TaktLogger.Information("TaktSalesInvoiceItem 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSalesInvoiceItem 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.salesinvoiceitem._self / entity.salesinvoiceitem.{{field}}；ResourceGroup=Sales；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSalesInvoiceItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.salesinvoiceitem._self
            new TranslationSeedItem("entity.salesinvoiceitem._self", "en-US", "Sales Invoice Item Information_us", "实体名称"),
            // entity.salesinvoiceitem._self
            new TranslationSeedItem("entity.salesinvoiceitem._self", "ja-JP", "Takt销售发票明细信息_jp", "实体名称"),
            // entity.salesinvoiceitem._self
            new TranslationSeedItem("entity.salesinvoiceitem._self", "zh-CN", "Takt销售发票明细信息", "实体名称"),
            // entity.salesinvoiceitem._self
            new TranslationSeedItem("entity.salesinvoiceitem._self", "zh-HK", "Takt销售发票明细信息_hk", "实体名称"),

            // entity.salesinvoiceitem.salesinvoiceid
            new TranslationSeedItem("entity.salesinvoiceitem.salesinvoiceid", "en-US", "销售发票ID_us", "销售发票ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.salesinvoiceitem.salesinvoiceid
            new TranslationSeedItem("entity.salesinvoiceitem.salesinvoiceid", "ja-JP", "销售发票ID_jp", "销售发票ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.salesinvoiceitem.salesinvoiceid
            new TranslationSeedItem("entity.salesinvoiceitem.salesinvoiceid", "zh-CN", "销售发票ID", "销售发票ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.salesinvoiceitem.salesinvoiceid
            new TranslationSeedItem("entity.salesinvoiceitem.salesinvoiceid", "zh-HK", "销售发票ID_hk", "销售发票ID（主子表关系，序列化为string以避免Javascript精度问题）"),

            // entity.salesinvoiceitem.salesinvoicecode
            new TranslationSeedItem("entity.salesinvoiceitem.salesinvoicecode", "en-US", "销售发票编码_us", "销售发票编码（冗余字段，便于查询）"),
            // entity.salesinvoiceitem.salesinvoicecode
            new TranslationSeedItem("entity.salesinvoiceitem.salesinvoicecode", "ja-JP", "销售发票编码_jp", "销售发票编码（冗余字段，便于查询）"),
            // entity.salesinvoiceitem.salesinvoicecode
            new TranslationSeedItem("entity.salesinvoiceitem.salesinvoicecode", "zh-CN", "销售发票编码", "销售发票编码（冗余字段，便于查询）"),
            // entity.salesinvoiceitem.salesinvoicecode
            new TranslationSeedItem("entity.salesinvoiceitem.salesinvoicecode", "zh-HK", "销售发票编码_hk", "销售发票编码（冗余字段，便于查询）"),

            // entity.salesinvoiceitem.linenumber
            new TranslationSeedItem("entity.salesinvoiceitem.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.salesinvoiceitem.linenumber
            new TranslationSeedItem("entity.salesinvoiceitem.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.salesinvoiceitem.linenumber
            new TranslationSeedItem("entity.salesinvoiceitem.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.salesinvoiceitem.linenumber
            new TranslationSeedItem("entity.salesinvoiceitem.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.salesinvoiceitem.materialcode
            new TranslationSeedItem("entity.salesinvoiceitem.materialcode", "en-US", "物料编码_us", "物料编码"),
            // entity.salesinvoiceitem.materialcode
            new TranslationSeedItem("entity.salesinvoiceitem.materialcode", "ja-JP", "物料编码_jp", "物料编码"),
            // entity.salesinvoiceitem.materialcode
            new TranslationSeedItem("entity.salesinvoiceitem.materialcode", "zh-CN", "物料编码", "物料编码"),
            // entity.salesinvoiceitem.materialcode
            new TranslationSeedItem("entity.salesinvoiceitem.materialcode", "zh-HK", "物料编码_hk", "物料编码"),

            // entity.salesinvoiceitem.materialname
            new TranslationSeedItem("entity.salesinvoiceitem.materialname", "en-US", "物料名称_us", "物料名称"),
            // entity.salesinvoiceitem.materialname
            new TranslationSeedItem("entity.salesinvoiceitem.materialname", "ja-JP", "物料名称_jp", "物料名称"),
            // entity.salesinvoiceitem.materialname
            new TranslationSeedItem("entity.salesinvoiceitem.materialname", "zh-CN", "物料名称", "物料名称"),
            // entity.salesinvoiceitem.materialname
            new TranslationSeedItem("entity.salesinvoiceitem.materialname", "zh-HK", "物料名称_hk", "物料名称"),

            // entity.salesinvoiceitem.materialspecification
            new TranslationSeedItem("entity.salesinvoiceitem.materialspecification", "en-US", "物料规格_us", "物料规格"),
            // entity.salesinvoiceitem.materialspecification
            new TranslationSeedItem("entity.salesinvoiceitem.materialspecification", "ja-JP", "物料规格_jp", "物料规格"),
            // entity.salesinvoiceitem.materialspecification
            new TranslationSeedItem("entity.salesinvoiceitem.materialspecification", "zh-CN", "物料规格", "物料规格"),
            // entity.salesinvoiceitem.materialspecification
            new TranslationSeedItem("entity.salesinvoiceitem.materialspecification", "zh-HK", "物料规格_hk", "物料规格"),

            // entity.salesinvoiceitem.salesunit
            new TranslationSeedItem("entity.salesinvoiceitem.salesunit", "en-US", "销售单位_us", "销售单位"),
            // entity.salesinvoiceitem.salesunit
            new TranslationSeedItem("entity.salesinvoiceitem.salesunit", "ja-JP", "销售单位_jp", "销售单位"),
            // entity.salesinvoiceitem.salesunit
            new TranslationSeedItem("entity.salesinvoiceitem.salesunit", "zh-CN", "销售单位", "销售单位"),
            // entity.salesinvoiceitem.salesunit
            new TranslationSeedItem("entity.salesinvoiceitem.salesunit", "zh-HK", "销售单位_hk", "销售单位"),

            // entity.salesinvoiceitem.invoicequantity
            new TranslationSeedItem("entity.salesinvoiceitem.invoicequantity", "en-US", "开票数量_us", "开票数量（基本单位数量）"),
            // entity.salesinvoiceitem.invoicequantity
            new TranslationSeedItem("entity.salesinvoiceitem.invoicequantity", "ja-JP", "开票数量_jp", "开票数量（基本单位数量）"),
            // entity.salesinvoiceitem.invoicequantity
            new TranslationSeedItem("entity.salesinvoiceitem.invoicequantity", "zh-CN", "开票数量", "开票数量（基本单位数量）"),
            // entity.salesinvoiceitem.invoicequantity
            new TranslationSeedItem("entity.salesinvoiceitem.invoicequantity", "zh-HK", "开票数量_hk", "开票数量（基本单位数量）"),

            // entity.salesinvoiceitem.unitprice
            new TranslationSeedItem("entity.salesinvoiceitem.unitprice", "en-US", "单价_us", "单价"),
            // entity.salesinvoiceitem.unitprice
            new TranslationSeedItem("entity.salesinvoiceitem.unitprice", "ja-JP", "单价_jp", "单价"),
            // entity.salesinvoiceitem.unitprice
            new TranslationSeedItem("entity.salesinvoiceitem.unitprice", "zh-CN", "单价", "单价"),
            // entity.salesinvoiceitem.unitprice
            new TranslationSeedItem("entity.salesinvoiceitem.unitprice", "zh-HK", "单价_hk", "单价"),

            // entity.salesinvoiceitem.discountrate
            new TranslationSeedItem("entity.salesinvoiceitem.discountrate", "en-US", "折扣率_us", "折扣率（0-100，表示折扣百分比）"),
            // entity.salesinvoiceitem.discountrate
            new TranslationSeedItem("entity.salesinvoiceitem.discountrate", "ja-JP", "折扣率_jp", "折扣率（0-100，表示折扣百分比）"),
            // entity.salesinvoiceitem.discountrate
            new TranslationSeedItem("entity.salesinvoiceitem.discountrate", "zh-CN", "折扣率", "折扣率（0-100，表示折扣百分比）"),
            // entity.salesinvoiceitem.discountrate
            new TranslationSeedItem("entity.salesinvoiceitem.discountrate", "zh-HK", "折扣率_hk", "折扣率（0-100，表示折扣百分比）"),

            // entity.salesinvoiceitem.discountamount
            new TranslationSeedItem("entity.salesinvoiceitem.discountamount", "en-US", "折扣金额_us", "折扣金额"),
            // entity.salesinvoiceitem.discountamount
            new TranslationSeedItem("entity.salesinvoiceitem.discountamount", "ja-JP", "折扣金额_jp", "折扣金额"),
            // entity.salesinvoiceitem.discountamount
            new TranslationSeedItem("entity.salesinvoiceitem.discountamount", "zh-CN", "折扣金额", "折扣金额"),
            // entity.salesinvoiceitem.discountamount
            new TranslationSeedItem("entity.salesinvoiceitem.discountamount", "zh-HK", "折扣金额_hk", "折扣金额"),

            // entity.salesinvoiceitem.taxrate
            new TranslationSeedItem("entity.salesinvoiceitem.taxrate", "en-US", "税费率_us", "税费率（0-100，表示税费百分比）"),
            // entity.salesinvoiceitem.taxrate
            new TranslationSeedItem("entity.salesinvoiceitem.taxrate", "ja-JP", "税费率_jp", "税费率（0-100，表示税费百分比）"),
            // entity.salesinvoiceitem.taxrate
            new TranslationSeedItem("entity.salesinvoiceitem.taxrate", "zh-CN", "税费率", "税费率（0-100，表示税费百分比）"),
            // entity.salesinvoiceitem.taxrate
            new TranslationSeedItem("entity.salesinvoiceitem.taxrate", "zh-HK", "税费率_hk", "税费率（0-100，表示税费百分比）"),

            // entity.salesinvoiceitem.taxamount
            new TranslationSeedItem("entity.salesinvoiceitem.taxamount", "en-US", "税费_us", "税费"),
            // entity.salesinvoiceitem.taxamount
            new TranslationSeedItem("entity.salesinvoiceitem.taxamount", "ja-JP", "税费_jp", "税费"),
            // entity.salesinvoiceitem.taxamount
            new TranslationSeedItem("entity.salesinvoiceitem.taxamount", "zh-CN", "税费", "税费"),
            // entity.salesinvoiceitem.taxamount
            new TranslationSeedItem("entity.salesinvoiceitem.taxamount", "zh-HK", "税费_hk", "税费"),

            // entity.salesinvoiceitem.subtotalamount
            new TranslationSeedItem("entity.salesinvoiceitem.subtotalamount", "en-US", "小计金额_us", "小计金额"),
            // entity.salesinvoiceitem.subtotalamount
            new TranslationSeedItem("entity.salesinvoiceitem.subtotalamount", "ja-JP", "小计金额_jp", "小计金额"),
            // entity.salesinvoiceitem.subtotalamount
            new TranslationSeedItem("entity.salesinvoiceitem.subtotalamount", "zh-CN", "小计金额", "小计金额"),
            // entity.salesinvoiceitem.subtotalamount
            new TranslationSeedItem("entity.salesinvoiceitem.subtotalamount", "zh-HK", "小计金额_hk", "小计金额"),

            // entity.salesinvoiceitem.salesinvoice
            new TranslationSeedItem("entity.salesinvoiceitem.salesinvoice", "en-US", "销售发票主表_us", "销售发票主表"),
            // entity.salesinvoiceitem.salesinvoice
            new TranslationSeedItem("entity.salesinvoiceitem.salesinvoice", "ja-JP", "销售发票主表_jp", "销售发票主表"),
            // entity.salesinvoiceitem.salesinvoice
            new TranslationSeedItem("entity.salesinvoiceitem.salesinvoice", "zh-CN", "销售发票主表", "销售发票主表"),
            // entity.salesinvoiceitem.salesinvoice
            new TranslationSeedItem("entity.salesinvoiceitem.salesinvoice", "zh-HK", "销售发票主表_hk", "销售发票主表"),
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
        translation.ResourceGroup = "Sales";
        translation.ResourceType = "frontend";
        translation.ContextNote = item.ContextNote;
        translation.ExtField = null;
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
