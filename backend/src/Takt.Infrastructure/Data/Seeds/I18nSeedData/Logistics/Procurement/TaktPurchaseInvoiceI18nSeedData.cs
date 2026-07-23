// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Procurement
// 文件名称：TaktPurchaseInvoiceI18nSeedData.cs
// 创建时间：2026-07-23
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktPurchaseInvoice 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktPurchaseInvoice 实体国际化翻译种子（键前缀 entity.purchaseinvoice.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktPurchaseInvoiceI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktPurchaseInvoice 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 purchaseinvoice 实体翻译...", tenantCode);

        foreach (var item in GetPurchaseInvoiceTranslations())
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

        TaktLogger.Information("TaktPurchaseInvoice 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktPurchaseInvoice 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.purchaseinvoice._self / entity.purchaseinvoice.{{field}}；ResourceGroup=Procurement；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetPurchaseInvoiceTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.purchaseinvoice._self
            new TranslationSeedItem("entity.purchaseinvoice._self", "en-US", "Purchase Invoice Information_us", "实体名称"),
            // entity.purchaseinvoice._self
            new TranslationSeedItem("entity.purchaseinvoice._self", "ja-JP", "Takt采购发票信息_jp", "实体名称"),
            // entity.purchaseinvoice._self
            new TranslationSeedItem("entity.purchaseinvoice._self", "zh-CN", "Takt采购发票信息", "实体名称"),
            // entity.purchaseinvoice._self
            new TranslationSeedItem("entity.purchaseinvoice._self", "zh-HK", "Takt采购发票信息_hk", "实体名称"),

            // entity.purchaseinvoice.plantcode
            new TranslationSeedItem("entity.purchaseinvoice.plantcode", "en-US", "工厂代码_us", "工厂代码（选项 TaktPlants/options；DictValue=PlantCode）"),
            // entity.purchaseinvoice.plantcode
            new TranslationSeedItem("entity.purchaseinvoice.plantcode", "ja-JP", "工厂代码_jp", "工厂代码（选项 TaktPlants/options；DictValue=PlantCode）"),
            // entity.purchaseinvoice.plantcode
            new TranslationSeedItem("entity.purchaseinvoice.plantcode", "zh-CN", "工厂代码", "工厂代码（选项 TaktPlants/options；DictValue=PlantCode）"),
            // entity.purchaseinvoice.plantcode
            new TranslationSeedItem("entity.purchaseinvoice.plantcode", "zh-HK", "工厂代码_hk", "工厂代码（选项 TaktPlants/options；DictValue=PlantCode）"),

            // entity.purchaseinvoice.code
            new TranslationSeedItem("entity.purchaseinvoice.code", "en-US", "采购发票编码_us", "采购发票编码（唯一索引）"),
            // entity.purchaseinvoice.code
            new TranslationSeedItem("entity.purchaseinvoice.code", "ja-JP", "采购发票编码_jp", "采购发票编码（唯一索引）"),
            // entity.purchaseinvoice.code
            new TranslationSeedItem("entity.purchaseinvoice.code", "zh-CN", "采购发票编码", "采购发票编码（唯一索引）"),
            // entity.purchaseinvoice.code
            new TranslationSeedItem("entity.purchaseinvoice.code", "zh-HK", "采购发票编码_hk", "采购发票编码（唯一索引）"),

            // entity.purchaseinvoice.purchaseordercode
            new TranslationSeedItem("entity.purchaseinvoice.purchaseordercode", "en-US", "采购订单编码_us", "关联采购订单编码（选项 TaktPurchaseOrders/options；DictValue=PurchaseOrderCode）"),
            // entity.purchaseinvoice.purchaseordercode
            new TranslationSeedItem("entity.purchaseinvoice.purchaseordercode", "ja-JP", "采购订单编码_jp", "关联采购订单编码（选项 TaktPurchaseOrders/options；DictValue=PurchaseOrderCode）"),
            // entity.purchaseinvoice.purchaseordercode
            new TranslationSeedItem("entity.purchaseinvoice.purchaseordercode", "zh-CN", "采购订单编码", "关联采购订单编码（选项 TaktPurchaseOrders/options；DictValue=PurchaseOrderCode）"),
            // entity.purchaseinvoice.purchaseordercode
            new TranslationSeedItem("entity.purchaseinvoice.purchaseordercode", "zh-HK", "采购订单编码_hk", "关联采购订单编码（选项 TaktPurchaseOrders/options；DictValue=PurchaseOrderCode）"),

            // entity.purchaseinvoice.suppliercode
            new TranslationSeedItem("entity.purchaseinvoice.suppliercode", "en-US", "供应商编码_us", "供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）"),
            // entity.purchaseinvoice.suppliercode
            new TranslationSeedItem("entity.purchaseinvoice.suppliercode", "ja-JP", "供应商编码_jp", "供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）"),
            // entity.purchaseinvoice.suppliercode
            new TranslationSeedItem("entity.purchaseinvoice.suppliercode", "zh-CN", "供应商编码", "供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）"),
            // entity.purchaseinvoice.suppliercode
            new TranslationSeedItem("entity.purchaseinvoice.suppliercode", "zh-HK", "供应商编码_hk", "供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）"),

            // entity.purchaseinvoice.suppliername1
            new TranslationSeedItem("entity.purchaseinvoice.suppliername1", "en-US", "供应商名称1_us", "供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）"),
            // entity.purchaseinvoice.suppliername1
            new TranslationSeedItem("entity.purchaseinvoice.suppliername1", "ja-JP", "供应商名称1_jp", "供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）"),
            // entity.purchaseinvoice.suppliername1
            new TranslationSeedItem("entity.purchaseinvoice.suppliername1", "zh-CN", "供应商名称1", "供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）"),
            // entity.purchaseinvoice.suppliername1
            new TranslationSeedItem("entity.purchaseinvoice.suppliername1", "zh-HK", "供应商名称1_hk", "供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）"),

            // entity.purchaseinvoice.invoicedate
            new TranslationSeedItem("entity.purchaseinvoice.invoicedate", "en-US", "开票日期_us", "开票日期"),
            // entity.purchaseinvoice.invoicedate
            new TranslationSeedItem("entity.purchaseinvoice.invoicedate", "ja-JP", "开票日期_jp", "开票日期"),
            // entity.purchaseinvoice.invoicedate
            new TranslationSeedItem("entity.purchaseinvoice.invoicedate", "zh-CN", "开票日期", "开票日期"),
            // entity.purchaseinvoice.invoicedate
            new TranslationSeedItem("entity.purchaseinvoice.invoicedate", "zh-HK", "开票日期_hk", "开票日期"),

            // entity.purchaseinvoice.totalamount
            new TranslationSeedItem("entity.purchaseinvoice.totalamount", "en-US", "发票总金额_us", "发票总金额"),
            // entity.purchaseinvoice.totalamount
            new TranslationSeedItem("entity.purchaseinvoice.totalamount", "ja-JP", "发票总金额_jp", "发票总金额"),
            // entity.purchaseinvoice.totalamount
            new TranslationSeedItem("entity.purchaseinvoice.totalamount", "zh-CN", "发票总金额", "发票总金额"),
            // entity.purchaseinvoice.totalamount
            new TranslationSeedItem("entity.purchaseinvoice.totalamount", "zh-HK", "发票总金额_hk", "发票总金额"),

            // entity.purchaseinvoice.currencycode
            new TranslationSeedItem("entity.purchaseinvoice.currencycode", "en-US", "结算币种_us", "结算币种（字典 accounting_currency_code；DictValue=CNY/USD 等；一单一币种）"),
            // entity.purchaseinvoice.currencycode
            new TranslationSeedItem("entity.purchaseinvoice.currencycode", "ja-JP", "结算币种_jp", "结算币种（字典 accounting_currency_code；DictValue=CNY/USD 等；一单一币种）"),
            // entity.purchaseinvoice.currencycode
            new TranslationSeedItem("entity.purchaseinvoice.currencycode", "zh-CN", "结算币种", "结算币种（字典 accounting_currency_code；DictValue=CNY/USD 等；一单一币种）"),
            // entity.purchaseinvoice.currencycode
            new TranslationSeedItem("entity.purchaseinvoice.currencycode", "zh-HK", "结算币种_hk", "结算币种（字典 accounting_currency_code；DictValue=CNY/USD 等；一单一币种）"),

            // entity.purchaseinvoice.taxrate
            new TranslationSeedItem("entity.purchaseinvoice.taxrate", "en-US", "税率_us", "税率（字典 accounting_tax_rate_param；13=13%，9=9%，0=0% 等；一单一税率）"),
            // entity.purchaseinvoice.taxrate
            new TranslationSeedItem("entity.purchaseinvoice.taxrate", "ja-JP", "税率_jp", "税率（字典 accounting_tax_rate_param；13=13%，9=9%，0=0% 等；一单一税率）"),
            // entity.purchaseinvoice.taxrate
            new TranslationSeedItem("entity.purchaseinvoice.taxrate", "zh-CN", "税率", "税率（字典 accounting_tax_rate_param；13=13%，9=9%，0=0% 等；一单一税率）"),
            // entity.purchaseinvoice.taxrate
            new TranslationSeedItem("entity.purchaseinvoice.taxrate", "zh-HK", "税率_hk", "税率（字典 accounting_tax_rate_param；13=13%，9=9%，0=0% 等；一单一税率）"),

            // entity.purchaseinvoice.taxamount
            new TranslationSeedItem("entity.purchaseinvoice.taxamount", "en-US", "税费_us", "税费"),
            // entity.purchaseinvoice.taxamount
            new TranslationSeedItem("entity.purchaseinvoice.taxamount", "ja-JP", "税费_jp", "税费"),
            // entity.purchaseinvoice.taxamount
            new TranslationSeedItem("entity.purchaseinvoice.taxamount", "zh-CN", "税费", "税费"),
            // entity.purchaseinvoice.taxamount
            new TranslationSeedItem("entity.purchaseinvoice.taxamount", "zh-HK", "税费_hk", "税费"),

            // entity.purchaseinvoice.actualamount
            new TranslationSeedItem("entity.purchaseinvoice.actualamount", "en-US", "发票应付金额_us", "发票应付金额"),
            // entity.purchaseinvoice.actualamount
            new TranslationSeedItem("entity.purchaseinvoice.actualamount", "ja-JP", "发票应付金额_jp", "发票应付金额"),
            // entity.purchaseinvoice.actualamount
            new TranslationSeedItem("entity.purchaseinvoice.actualamount", "zh-CN", "发票应付金额", "发票应付金额"),
            // entity.purchaseinvoice.actualamount
            new TranslationSeedItem("entity.purchaseinvoice.actualamount", "zh-HK", "发票应付金额_hk", "发票应付金额"),

            // entity.purchaseinvoice.paidamount
            new TranslationSeedItem("entity.purchaseinvoice.paidamount", "en-US", "已付款金额_us", "已付款金额"),
            // entity.purchaseinvoice.paidamount
            new TranslationSeedItem("entity.purchaseinvoice.paidamount", "ja-JP", "已付款金额_jp", "已付款金额"),
            // entity.purchaseinvoice.paidamount
            new TranslationSeedItem("entity.purchaseinvoice.paidamount", "zh-CN", "已付款金额", "已付款金额"),
            // entity.purchaseinvoice.paidamount
            new TranslationSeedItem("entity.purchaseinvoice.paidamount", "zh-HK", "已付款金额_hk", "已付款金额"),

            // entity.purchaseinvoice.paymentmethod
            new TranslationSeedItem("entity.purchaseinvoice.paymentmethod", "en-US", "付款方式_us", "付款方式（字典 accounting_payment_method_type；0=现金，1=银行转账，2=支票，3=信用证，4=其他）"),
            // entity.purchaseinvoice.paymentmethod
            new TranslationSeedItem("entity.purchaseinvoice.paymentmethod", "ja-JP", "付款方式_jp", "付款方式（字典 accounting_payment_method_type；0=现金，1=银行转账，2=支票，3=信用证，4=其他）"),
            // entity.purchaseinvoice.paymentmethod
            new TranslationSeedItem("entity.purchaseinvoice.paymentmethod", "zh-CN", "付款方式", "付款方式（字典 accounting_payment_method_type；0=现金，1=银行转账，2=支票，3=信用证，4=其他）"),
            // entity.purchaseinvoice.paymentmethod
            new TranslationSeedItem("entity.purchaseinvoice.paymentmethod", "zh-HK", "付款方式_hk", "付款方式（字典 accounting_payment_method_type；0=现金，1=银行转账，2=支票，3=信用证，4=其他）"),

            // entity.purchaseinvoice.taxinvoiceno
            new TranslationSeedItem("entity.purchaseinvoice.taxinvoiceno", "en-US", "税务发票号码_us", "税务发票号码"),
            // entity.purchaseinvoice.taxinvoiceno
            new TranslationSeedItem("entity.purchaseinvoice.taxinvoiceno", "ja-JP", "税务发票号码_jp", "税务发票号码"),
            // entity.purchaseinvoice.taxinvoiceno
            new TranslationSeedItem("entity.purchaseinvoice.taxinvoiceno", "zh-CN", "税务发票号码", "税务发票号码"),
            // entity.purchaseinvoice.taxinvoiceno
            new TranslationSeedItem("entity.purchaseinvoice.taxinvoiceno", "zh-HK", "税务发票号码_hk", "税务发票号码"),

            // entity.purchaseinvoice.invoicestatus
            new TranslationSeedItem("entity.purchaseinvoice.invoicestatus", "en-US", "发票状态_us", "发票状态（字典 logistics_invoice_status；0=草稿，1=已开票，2=已收款，3=已作废）"),
            // entity.purchaseinvoice.invoicestatus
            new TranslationSeedItem("entity.purchaseinvoice.invoicestatus", "ja-JP", "发票状态_jp", "发票状态（字典 logistics_invoice_status；0=草稿，1=已开票，2=已收款，3=已作废）"),
            // entity.purchaseinvoice.invoicestatus
            new TranslationSeedItem("entity.purchaseinvoice.invoicestatus", "zh-CN", "发票状态", "发票状态（字典 logistics_invoice_status；0=草稿，1=已开票，2=已收款，3=已作废）"),
            // entity.purchaseinvoice.invoicestatus
            new TranslationSeedItem("entity.purchaseinvoice.invoicestatus", "zh-HK", "发票状态_hk", "发票状态（字典 logistics_invoice_status；0=草稿，1=已开票，2=已收款，3=已作废）"),

            // entity.purchaseinvoice.items
            new TranslationSeedItem("entity.purchaseinvoice.items", "en-US", "采购发票明细列表_us", "采购发票明细列表（主子表关系，一张发票可有多个明细行）"),
            // entity.purchaseinvoice.items
            new TranslationSeedItem("entity.purchaseinvoice.items", "ja-JP", "采购发票明细列表_jp", "采购发票明细列表（主子表关系，一张发票可有多个明细行）"),
            // entity.purchaseinvoice.items
            new TranslationSeedItem("entity.purchaseinvoice.items", "zh-CN", "采购发票明细列表", "采购发票明细列表（主子表关系，一张发票可有多个明细行）"),
            // entity.purchaseinvoice.items
            new TranslationSeedItem("entity.purchaseinvoice.items", "zh-HK", "采购发票明细列表_hk", "采购发票明细列表（主子表关系，一张发票可有多个明细行）"),
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
