// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Sales
// 文件名称：TaktSalesInvoiceItemI18nSeedData.cs
// 创建时间：2026-06-06
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
using Takt.Shared.Enums;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Sales;

/// <summary>
/// TaktSalesInvoiceItem 实体国际化翻译种子（键前缀 entity.salesInvoiceItem.*）
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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 salesInvoiceItem 实体翻译...", tenantCode);

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
    /// I18nKey：entity.salesInvoiceItem._self / entity.salesInvoiceItem.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSalesInvoiceItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.salesInvoiceItem._self
            new TranslationSeedItem("entity.salesInvoiceItem._self", "en-US", "Sales Invoice Item Information", "实体名称"),
            // entity.salesInvoiceItem._self
            new TranslationSeedItem("entity.salesInvoiceItem._self", "ja-JP", "Takt销售发票明细信息", "实体名称"),
            // entity.salesInvoiceItem._self
            new TranslationSeedItem("entity.salesInvoiceItem._self", "zh-CN", "Takt销售发票明细信息", "实体名称"),
            // entity.salesInvoiceItem._self
            new TranslationSeedItem("entity.salesInvoiceItem._self", "zh-HK", "Takt销售发票明细信息", "实体名称"),

            // entity.salesInvoiceItem.salesinvoiceid
            new TranslationSeedItem("entity.salesInvoiceItem.salesinvoiceid", "en-US", "销售发票ID", "销售发票ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.salesInvoiceItem.salesinvoiceid
            new TranslationSeedItem("entity.salesInvoiceItem.salesinvoiceid", "ja-JP", "销售发票ID", "销售发票ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.salesInvoiceItem.salesinvoiceid
            new TranslationSeedItem("entity.salesInvoiceItem.salesinvoiceid", "zh-CN", "销售发票ID", "销售发票ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.salesInvoiceItem.salesinvoiceid
            new TranslationSeedItem("entity.salesInvoiceItem.salesinvoiceid", "zh-HK", "销售发票ID", "销售发票ID（主子表关系，序列化为string以避免Javascript精度问题）"),

            // entity.salesInvoiceItem.salesinvoicecode
            new TranslationSeedItem("entity.salesInvoiceItem.salesinvoicecode", "en-US", "销售发票编码", "销售发票编码（冗余字段，便于查询）"),
            // entity.salesInvoiceItem.salesinvoicecode
            new TranslationSeedItem("entity.salesInvoiceItem.salesinvoicecode", "ja-JP", "销售发票编码", "销售发票编码（冗余字段，便于查询）"),
            // entity.salesInvoiceItem.salesinvoicecode
            new TranslationSeedItem("entity.salesInvoiceItem.salesinvoicecode", "zh-CN", "销售发票编码", "销售发票编码（冗余字段，便于查询）"),
            // entity.salesInvoiceItem.salesinvoicecode
            new TranslationSeedItem("entity.salesInvoiceItem.salesinvoicecode", "zh-HK", "销售发票编码", "销售发票编码（冗余字段，便于查询）"),

            // entity.salesInvoiceItem.linenumber
            new TranslationSeedItem("entity.salesInvoiceItem.linenumber", "en-US", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.salesInvoiceItem.linenumber
            new TranslationSeedItem("entity.salesInvoiceItem.linenumber", "ja-JP", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.salesInvoiceItem.linenumber
            new TranslationSeedItem("entity.salesInvoiceItem.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.salesInvoiceItem.linenumber
            new TranslationSeedItem("entity.salesInvoiceItem.linenumber", "zh-HK", "行号", "行号（项号/序号，固定步长=10）"),

            // entity.salesInvoiceItem.materialcode
            new TranslationSeedItem("entity.salesInvoiceItem.materialcode", "en-US", "物料编码", "物料编码"),
            // entity.salesInvoiceItem.materialcode
            new TranslationSeedItem("entity.salesInvoiceItem.materialcode", "ja-JP", "物料编码", "物料编码"),
            // entity.salesInvoiceItem.materialcode
            new TranslationSeedItem("entity.salesInvoiceItem.materialcode", "zh-CN", "物料编码", "物料编码"),
            // entity.salesInvoiceItem.materialcode
            new TranslationSeedItem("entity.salesInvoiceItem.materialcode", "zh-HK", "物料编码", "物料编码"),

            // entity.salesInvoiceItem.materialname
            new TranslationSeedItem("entity.salesInvoiceItem.materialname", "en-US", "物料名称", "物料名称"),
            // entity.salesInvoiceItem.materialname
            new TranslationSeedItem("entity.salesInvoiceItem.materialname", "ja-JP", "物料名称", "物料名称"),
            // entity.salesInvoiceItem.materialname
            new TranslationSeedItem("entity.salesInvoiceItem.materialname", "zh-CN", "物料名称", "物料名称"),
            // entity.salesInvoiceItem.materialname
            new TranslationSeedItem("entity.salesInvoiceItem.materialname", "zh-HK", "物料名称", "物料名称"),

            // entity.salesInvoiceItem.materialspecification
            new TranslationSeedItem("entity.salesInvoiceItem.materialspecification", "en-US", "物料规格", "物料规格"),
            // entity.salesInvoiceItem.materialspecification
            new TranslationSeedItem("entity.salesInvoiceItem.materialspecification", "ja-JP", "物料规格", "物料规格"),
            // entity.salesInvoiceItem.materialspecification
            new TranslationSeedItem("entity.salesInvoiceItem.materialspecification", "zh-CN", "物料规格", "物料规格"),
            // entity.salesInvoiceItem.materialspecification
            new TranslationSeedItem("entity.salesInvoiceItem.materialspecification", "zh-HK", "物料规格", "物料规格"),

            // entity.salesInvoiceItem.salesunit
            new TranslationSeedItem("entity.salesInvoiceItem.salesunit", "en-US", "销售单位", "销售单位"),
            // entity.salesInvoiceItem.salesunit
            new TranslationSeedItem("entity.salesInvoiceItem.salesunit", "ja-JP", "销售单位", "销售单位"),
            // entity.salesInvoiceItem.salesunit
            new TranslationSeedItem("entity.salesInvoiceItem.salesunit", "zh-CN", "销售单位", "销售单位"),
            // entity.salesInvoiceItem.salesunit
            new TranslationSeedItem("entity.salesInvoiceItem.salesunit", "zh-HK", "销售单位", "销售单位"),

            // entity.salesInvoiceItem.invoicequantity
            new TranslationSeedItem("entity.salesInvoiceItem.invoicequantity", "en-US", "开票数量", "开票数量（基本单位数量）"),
            // entity.salesInvoiceItem.invoicequantity
            new TranslationSeedItem("entity.salesInvoiceItem.invoicequantity", "ja-JP", "开票数量", "开票数量（基本单位数量）"),
            // entity.salesInvoiceItem.invoicequantity
            new TranslationSeedItem("entity.salesInvoiceItem.invoicequantity", "zh-CN", "开票数量", "开票数量（基本单位数量）"),
            // entity.salesInvoiceItem.invoicequantity
            new TranslationSeedItem("entity.salesInvoiceItem.invoicequantity", "zh-HK", "开票数量", "开票数量（基本单位数量）"),

            // entity.salesInvoiceItem.unitprice
            new TranslationSeedItem("entity.salesInvoiceItem.unitprice", "en-US", "单价", "单价"),
            // entity.salesInvoiceItem.unitprice
            new TranslationSeedItem("entity.salesInvoiceItem.unitprice", "ja-JP", "单价", "单价"),
            // entity.salesInvoiceItem.unitprice
            new TranslationSeedItem("entity.salesInvoiceItem.unitprice", "zh-CN", "单价", "单价"),
            // entity.salesInvoiceItem.unitprice
            new TranslationSeedItem("entity.salesInvoiceItem.unitprice", "zh-HK", "单价", "单价"),

            // entity.salesInvoiceItem.discountrate
            new TranslationSeedItem("entity.salesInvoiceItem.discountrate", "en-US", "折扣率", "折扣率（0-100，表示折扣百分比）"),
            // entity.salesInvoiceItem.discountrate
            new TranslationSeedItem("entity.salesInvoiceItem.discountrate", "ja-JP", "折扣率", "折扣率（0-100，表示折扣百分比）"),
            // entity.salesInvoiceItem.discountrate
            new TranslationSeedItem("entity.salesInvoiceItem.discountrate", "zh-CN", "折扣率", "折扣率（0-100，表示折扣百分比）"),
            // entity.salesInvoiceItem.discountrate
            new TranslationSeedItem("entity.salesInvoiceItem.discountrate", "zh-HK", "折扣率", "折扣率（0-100，表示折扣百分比）"),

            // entity.salesInvoiceItem.discountamount
            new TranslationSeedItem("entity.salesInvoiceItem.discountamount", "en-US", "折扣金额", "折扣金额"),
            // entity.salesInvoiceItem.discountamount
            new TranslationSeedItem("entity.salesInvoiceItem.discountamount", "ja-JP", "折扣金额", "折扣金额"),
            // entity.salesInvoiceItem.discountamount
            new TranslationSeedItem("entity.salesInvoiceItem.discountamount", "zh-CN", "折扣金额", "折扣金额"),
            // entity.salesInvoiceItem.discountamount
            new TranslationSeedItem("entity.salesInvoiceItem.discountamount", "zh-HK", "折扣金额", "折扣金额"),

            // entity.salesInvoiceItem.taxrate
            new TranslationSeedItem("entity.salesInvoiceItem.taxrate", "en-US", "税费率", "税费率（0-100，表示税费百分比）"),
            // entity.salesInvoiceItem.taxrate
            new TranslationSeedItem("entity.salesInvoiceItem.taxrate", "ja-JP", "税费率", "税费率（0-100，表示税费百分比）"),
            // entity.salesInvoiceItem.taxrate
            new TranslationSeedItem("entity.salesInvoiceItem.taxrate", "zh-CN", "税费率", "税费率（0-100，表示税费百分比）"),
            // entity.salesInvoiceItem.taxrate
            new TranslationSeedItem("entity.salesInvoiceItem.taxrate", "zh-HK", "税费率", "税费率（0-100，表示税费百分比）"),

            // entity.salesInvoiceItem.taxamount
            new TranslationSeedItem("entity.salesInvoiceItem.taxamount", "en-US", "税费", "税费"),
            // entity.salesInvoiceItem.taxamount
            new TranslationSeedItem("entity.salesInvoiceItem.taxamount", "ja-JP", "税费", "税费"),
            // entity.salesInvoiceItem.taxamount
            new TranslationSeedItem("entity.salesInvoiceItem.taxamount", "zh-CN", "税费", "税费"),
            // entity.salesInvoiceItem.taxamount
            new TranslationSeedItem("entity.salesInvoiceItem.taxamount", "zh-HK", "税费", "税费"),

            // entity.salesInvoiceItem.subtotalamount
            new TranslationSeedItem("entity.salesInvoiceItem.subtotalamount", "en-US", "小计金额", "小计金额"),
            // entity.salesInvoiceItem.subtotalamount
            new TranslationSeedItem("entity.salesInvoiceItem.subtotalamount", "ja-JP", "小计金额", "小计金额"),
            // entity.salesInvoiceItem.subtotalamount
            new TranslationSeedItem("entity.salesInvoiceItem.subtotalamount", "zh-CN", "小计金额", "小计金额"),
            // entity.salesInvoiceItem.subtotalamount
            new TranslationSeedItem("entity.salesInvoiceItem.subtotalamount", "zh-HK", "小计金额", "小计金额"),
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
