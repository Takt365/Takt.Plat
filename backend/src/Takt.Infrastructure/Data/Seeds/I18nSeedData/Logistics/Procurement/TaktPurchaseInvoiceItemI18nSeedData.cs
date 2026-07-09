// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Procurement
// 文件名称：TaktPurchaseInvoiceItemI18nSeedData.cs
// 创建时间：2026-07-09
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktPurchaseInvoiceItem 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Procurement;

/// <summary>
/// TaktPurchaseInvoiceItem 实体国际化翻译种子（键前缀 entity.purchaseinvoiceitem.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktPurchaseInvoiceItemI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktPurchaseInvoiceItem 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 purchaseinvoiceitem 实体翻译...", tenantCode);

        foreach (var item in GetPurchaseInvoiceItemTranslations())
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

        TaktLogger.Information("TaktPurchaseInvoiceItem 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktPurchaseInvoiceItem 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.purchaseinvoiceitem._self / entity.purchaseinvoiceitem.{{field}}；ResourceGroup=Procurement；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetPurchaseInvoiceItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.purchaseinvoiceitem._self
            new TranslationSeedItem("entity.purchaseinvoiceitem._self", "en-US", "Purchase Invoice Item Information_us", "实体名称"),
            // entity.purchaseinvoiceitem._self
            new TranslationSeedItem("entity.purchaseinvoiceitem._self", "ja-JP", "Takt采购发票明细信息_jp", "实体名称"),
            // entity.purchaseinvoiceitem._self
            new TranslationSeedItem("entity.purchaseinvoiceitem._self", "zh-CN", "Takt采购发票明细信息", "实体名称"),
            // entity.purchaseinvoiceitem._self
            new TranslationSeedItem("entity.purchaseinvoiceitem._self", "zh-HK", "Takt采购发票明细信息_hk", "实体名称"),

            // entity.purchaseinvoiceitem.purchaseinvoiceid
            new TranslationSeedItem("entity.purchaseinvoiceitem.purchaseinvoiceid", "en-US", "采购发票ID_us", "采购发票 ID（关联 TaktPurchaseInvoice.Id，选项 TaktPurchaseInvoices/options）"),
            // entity.purchaseinvoiceitem.purchaseinvoiceid
            new TranslationSeedItem("entity.purchaseinvoiceitem.purchaseinvoiceid", "ja-JP", "采购发票ID_jp", "采购发票 ID（关联 TaktPurchaseInvoice.Id，选项 TaktPurchaseInvoices/options）"),
            // entity.purchaseinvoiceitem.purchaseinvoiceid
            new TranslationSeedItem("entity.purchaseinvoiceitem.purchaseinvoiceid", "zh-CN", "采购发票ID", "采购发票 ID（关联 TaktPurchaseInvoice.Id，选项 TaktPurchaseInvoices/options）"),
            // entity.purchaseinvoiceitem.purchaseinvoiceid
            new TranslationSeedItem("entity.purchaseinvoiceitem.purchaseinvoiceid", "zh-HK", "采购发票ID_hk", "采购发票 ID（关联 TaktPurchaseInvoice.Id，选项 TaktPurchaseInvoices/options）"),

            // entity.purchaseinvoiceitem.purchaseinvoicecode
            new TranslationSeedItem("entity.purchaseinvoiceitem.purchaseinvoicecode", "en-US", "采购发票编码_us", "采购发票编码（冗余字段，便于查询）"),
            // entity.purchaseinvoiceitem.purchaseinvoicecode
            new TranslationSeedItem("entity.purchaseinvoiceitem.purchaseinvoicecode", "ja-JP", "采购发票编码_jp", "采购发票编码（冗余字段，便于查询）"),
            // entity.purchaseinvoiceitem.purchaseinvoicecode
            new TranslationSeedItem("entity.purchaseinvoiceitem.purchaseinvoicecode", "zh-CN", "采购发票编码", "采购发票编码（冗余字段，便于查询）"),
            // entity.purchaseinvoiceitem.purchaseinvoicecode
            new TranslationSeedItem("entity.purchaseinvoiceitem.purchaseinvoicecode", "zh-HK", "采购发票编码_hk", "采购发票编码（冗余字段，便于查询）"),

            // entity.purchaseinvoiceitem.linenumber
            new TranslationSeedItem("entity.purchaseinvoiceitem.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.purchaseinvoiceitem.linenumber
            new TranslationSeedItem("entity.purchaseinvoiceitem.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.purchaseinvoiceitem.linenumber
            new TranslationSeedItem("entity.purchaseinvoiceitem.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.purchaseinvoiceitem.linenumber
            new TranslationSeedItem("entity.purchaseinvoiceitem.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.purchaseinvoiceitem.purchaseordercode
            new TranslationSeedItem("entity.purchaseinvoiceitem.purchaseordercode", "en-US", "来源采购订单编码_us", "来源采购订单编码"),
            // entity.purchaseinvoiceitem.purchaseordercode
            new TranslationSeedItem("entity.purchaseinvoiceitem.purchaseordercode", "ja-JP", "来源采购订单编码_jp", "来源采购订单编码"),
            // entity.purchaseinvoiceitem.purchaseordercode
            new TranslationSeedItem("entity.purchaseinvoiceitem.purchaseordercode", "zh-CN", "来源采购订单编码", "来源采购订单编码"),
            // entity.purchaseinvoiceitem.purchaseordercode
            new TranslationSeedItem("entity.purchaseinvoiceitem.purchaseordercode", "zh-HK", "来源采购订单编码_hk", "来源采购订单编码"),

            // entity.purchaseinvoiceitem.purchaseorderlinenumber
            new TranslationSeedItem("entity.purchaseinvoiceitem.purchaseorderlinenumber", "en-US", "来源采购订单行号_us", "来源采购订单行号"),
            // entity.purchaseinvoiceitem.purchaseorderlinenumber
            new TranslationSeedItem("entity.purchaseinvoiceitem.purchaseorderlinenumber", "ja-JP", "来源采购订单行号_jp", "来源采购订单行号"),
            // entity.purchaseinvoiceitem.purchaseorderlinenumber
            new TranslationSeedItem("entity.purchaseinvoiceitem.purchaseorderlinenumber", "zh-CN", "来源采购订单行号", "来源采购订单行号"),
            // entity.purchaseinvoiceitem.purchaseorderlinenumber
            new TranslationSeedItem("entity.purchaseinvoiceitem.purchaseorderlinenumber", "zh-HK", "来源采购订单行号_hk", "来源采购订单行号"),

            // entity.purchaseinvoiceitem.materialcode
            new TranslationSeedItem("entity.purchaseinvoiceitem.materialcode", "en-US", "物料编码_us", "物料编码"),
            // entity.purchaseinvoiceitem.materialcode
            new TranslationSeedItem("entity.purchaseinvoiceitem.materialcode", "ja-JP", "物料编码_jp", "物料编码"),
            // entity.purchaseinvoiceitem.materialcode
            new TranslationSeedItem("entity.purchaseinvoiceitem.materialcode", "zh-CN", "物料编码", "物料编码"),
            // entity.purchaseinvoiceitem.materialcode
            new TranslationSeedItem("entity.purchaseinvoiceitem.materialcode", "zh-HK", "物料编码_hk", "物料编码"),

            // entity.purchaseinvoiceitem.materialname
            new TranslationSeedItem("entity.purchaseinvoiceitem.materialname", "en-US", "物料名称_us", "物料名称"),
            // entity.purchaseinvoiceitem.materialname
            new TranslationSeedItem("entity.purchaseinvoiceitem.materialname", "ja-JP", "物料名称_jp", "物料名称"),
            // entity.purchaseinvoiceitem.materialname
            new TranslationSeedItem("entity.purchaseinvoiceitem.materialname", "zh-CN", "物料名称", "物料名称"),
            // entity.purchaseinvoiceitem.materialname
            new TranslationSeedItem("entity.purchaseinvoiceitem.materialname", "zh-HK", "物料名称_hk", "物料名称"),

            // entity.purchaseinvoiceitem.materialspecification
            new TranslationSeedItem("entity.purchaseinvoiceitem.materialspecification", "en-US", "物料规格_us", "物料规格"),
            // entity.purchaseinvoiceitem.materialspecification
            new TranslationSeedItem("entity.purchaseinvoiceitem.materialspecification", "ja-JP", "物料规格_jp", "物料规格"),
            // entity.purchaseinvoiceitem.materialspecification
            new TranslationSeedItem("entity.purchaseinvoiceitem.materialspecification", "zh-CN", "物料规格", "物料规格"),
            // entity.purchaseinvoiceitem.materialspecification
            new TranslationSeedItem("entity.purchaseinvoiceitem.materialspecification", "zh-HK", "物料规格_hk", "物料规格"),

            // entity.purchaseinvoiceitem.purchaseunit
            new TranslationSeedItem("entity.purchaseinvoiceitem.purchaseunit", "en-US", "采购单位_us", "采购单位"),
            // entity.purchaseinvoiceitem.purchaseunit
            new TranslationSeedItem("entity.purchaseinvoiceitem.purchaseunit", "ja-JP", "采购单位_jp", "采购单位"),
            // entity.purchaseinvoiceitem.purchaseunit
            new TranslationSeedItem("entity.purchaseinvoiceitem.purchaseunit", "zh-CN", "采购单位", "采购单位"),
            // entity.purchaseinvoiceitem.purchaseunit
            new TranslationSeedItem("entity.purchaseinvoiceitem.purchaseunit", "zh-HK", "采购单位_hk", "采购单位"),

            // entity.purchaseinvoiceitem.invoicequantity
            new TranslationSeedItem("entity.purchaseinvoiceitem.invoicequantity", "en-US", "开票数量_us", "开票数量（基本单位数量）"),
            // entity.purchaseinvoiceitem.invoicequantity
            new TranslationSeedItem("entity.purchaseinvoiceitem.invoicequantity", "ja-JP", "开票数量_jp", "开票数量（基本单位数量）"),
            // entity.purchaseinvoiceitem.invoicequantity
            new TranslationSeedItem("entity.purchaseinvoiceitem.invoicequantity", "zh-CN", "开票数量", "开票数量（基本单位数量）"),
            // entity.purchaseinvoiceitem.invoicequantity
            new TranslationSeedItem("entity.purchaseinvoiceitem.invoicequantity", "zh-HK", "开票数量_hk", "开票数量（基本单位数量）"),

            // entity.purchaseinvoiceitem.unitprice
            new TranslationSeedItem("entity.purchaseinvoiceitem.unitprice", "en-US", "单价_us", "单价"),
            // entity.purchaseinvoiceitem.unitprice
            new TranslationSeedItem("entity.purchaseinvoiceitem.unitprice", "ja-JP", "单价_jp", "单价"),
            // entity.purchaseinvoiceitem.unitprice
            new TranslationSeedItem("entity.purchaseinvoiceitem.unitprice", "zh-CN", "单价", "单价"),
            // entity.purchaseinvoiceitem.unitprice
            new TranslationSeedItem("entity.purchaseinvoiceitem.unitprice", "zh-HK", "单价_hk", "单价"),

            // entity.purchaseinvoiceitem.discountrate
            new TranslationSeedItem("entity.purchaseinvoiceitem.discountrate", "en-US", "折扣率_us", "折扣率（字典 logistics_discount_rate_param 预设或手输；0-100，表示折扣百分比）"),
            // entity.purchaseinvoiceitem.discountrate
            new TranslationSeedItem("entity.purchaseinvoiceitem.discountrate", "ja-JP", "折扣率_jp", "折扣率（字典 logistics_discount_rate_param 预设或手输；0-100，表示折扣百分比）"),
            // entity.purchaseinvoiceitem.discountrate
            new TranslationSeedItem("entity.purchaseinvoiceitem.discountrate", "zh-CN", "折扣率", "折扣率（字典 logistics_discount_rate_param 预设或手输；0-100，表示折扣百分比）"),
            // entity.purchaseinvoiceitem.discountrate
            new TranslationSeedItem("entity.purchaseinvoiceitem.discountrate", "zh-HK", "折扣率_hk", "折扣率（字典 logistics_discount_rate_param 预设或手输；0-100，表示折扣百分比）"),

            // entity.purchaseinvoiceitem.discountamount
            new TranslationSeedItem("entity.purchaseinvoiceitem.discountamount", "en-US", "折扣金额_us", "折扣金额"),
            // entity.purchaseinvoiceitem.discountamount
            new TranslationSeedItem("entity.purchaseinvoiceitem.discountamount", "ja-JP", "折扣金额_jp", "折扣金额"),
            // entity.purchaseinvoiceitem.discountamount
            new TranslationSeedItem("entity.purchaseinvoiceitem.discountamount", "zh-CN", "折扣金额", "折扣金额"),
            // entity.purchaseinvoiceitem.discountamount
            new TranslationSeedItem("entity.purchaseinvoiceitem.discountamount", "zh-HK", "折扣金额_hk", "折扣金额"),

            // entity.purchaseinvoiceitem.taxrate
            new TranslationSeedItem("entity.purchaseinvoiceitem.taxrate", "en-US", "税费率_us", "税费率（字典 accounting_tax_rate_param 预设或手输；0-100，表示税费百分比）"),
            // entity.purchaseinvoiceitem.taxrate
            new TranslationSeedItem("entity.purchaseinvoiceitem.taxrate", "ja-JP", "税费率_jp", "税费率（字典 accounting_tax_rate_param 预设或手输；0-100，表示税费百分比）"),
            // entity.purchaseinvoiceitem.taxrate
            new TranslationSeedItem("entity.purchaseinvoiceitem.taxrate", "zh-CN", "税费率", "税费率（字典 accounting_tax_rate_param 预设或手输；0-100，表示税费百分比）"),
            // entity.purchaseinvoiceitem.taxrate
            new TranslationSeedItem("entity.purchaseinvoiceitem.taxrate", "zh-HK", "税费率_hk", "税费率（字典 accounting_tax_rate_param 预设或手输；0-100，表示税费百分比）"),

            // entity.purchaseinvoiceitem.taxamount
            new TranslationSeedItem("entity.purchaseinvoiceitem.taxamount", "en-US", "税费_us", "税费"),
            // entity.purchaseinvoiceitem.taxamount
            new TranslationSeedItem("entity.purchaseinvoiceitem.taxamount", "ja-JP", "税费_jp", "税费"),
            // entity.purchaseinvoiceitem.taxamount
            new TranslationSeedItem("entity.purchaseinvoiceitem.taxamount", "zh-CN", "税费", "税费"),
            // entity.purchaseinvoiceitem.taxamount
            new TranslationSeedItem("entity.purchaseinvoiceitem.taxamount", "zh-HK", "税费_hk", "税费"),

            // entity.purchaseinvoiceitem.subtotalamount
            new TranslationSeedItem("entity.purchaseinvoiceitem.subtotalamount", "en-US", "小计金额_us", "小计金额"),
            // entity.purchaseinvoiceitem.subtotalamount
            new TranslationSeedItem("entity.purchaseinvoiceitem.subtotalamount", "ja-JP", "小计金额_jp", "小计金额"),
            // entity.purchaseinvoiceitem.subtotalamount
            new TranslationSeedItem("entity.purchaseinvoiceitem.subtotalamount", "zh-CN", "小计金额", "小计金额"),
            // entity.purchaseinvoiceitem.subtotalamount
            new TranslationSeedItem("entity.purchaseinvoiceitem.subtotalamount", "zh-HK", "小计金额_hk", "小计金额"),

            // entity.purchaseinvoiceitem.isobsolete
            new TranslationSeedItem("entity.purchaseinvoiceitem.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）"),
            // entity.purchaseinvoiceitem.isobsolete
            new TranslationSeedItem("entity.purchaseinvoiceitem.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）"),
            // entity.purchaseinvoiceitem.isobsolete
            new TranslationSeedItem("entity.purchaseinvoiceitem.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）"),
            // entity.purchaseinvoiceitem.isobsolete
            new TranslationSeedItem("entity.purchaseinvoiceitem.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）"),
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
        translation.ResourceGroup = "Procurement";
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
