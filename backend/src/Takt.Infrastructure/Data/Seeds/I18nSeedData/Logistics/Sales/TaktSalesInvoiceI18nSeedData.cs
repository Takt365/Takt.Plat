// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Sales
// 文件名称：TaktSalesInvoiceI18nSeedData.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSalesInvoice 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktSalesInvoice 实体国际化翻译种子（键前缀 entity.salesinvoice.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSalesInvoiceI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSalesInvoice 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 salesinvoice 实体翻译...", tenantCode);

        foreach (var item in GetSalesInvoiceTranslations())
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

        TaktLogger.Information("TaktSalesInvoice 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSalesInvoice 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.salesinvoice._self / entity.salesinvoice.{{field}}；ResourceGroup=Sales；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSalesInvoiceTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.salesinvoice._self
            new TranslationSeedItem("entity.salesinvoice._self", "en-US", "Sales Invoice Information_us", "实体名称"),
            // entity.salesinvoice._self
            new TranslationSeedItem("entity.salesinvoice._self", "ja-JP", "Takt销售发票信息_jp", "实体名称"),
            // entity.salesinvoice._self
            new TranslationSeedItem("entity.salesinvoice._self", "zh-CN", "Takt销售发票信息", "实体名称"),
            // entity.salesinvoice._self
            new TranslationSeedItem("entity.salesinvoice._self", "zh-HK", "Takt销售发票信息_hk", "实体名称"),

            // entity.salesinvoice.plantcode
            new TranslationSeedItem("entity.salesinvoice.plantcode", "en-US", "工厂代码_us", "工厂代码"),
            // entity.salesinvoice.plantcode
            new TranslationSeedItem("entity.salesinvoice.plantcode", "ja-JP", "工厂代码_jp", "工厂代码"),
            // entity.salesinvoice.plantcode
            new TranslationSeedItem("entity.salesinvoice.plantcode", "zh-CN", "工厂代码", "工厂代码"),
            // entity.salesinvoice.plantcode
            new TranslationSeedItem("entity.salesinvoice.plantcode", "zh-HK", "工厂代码_hk", "工厂代码"),

            // entity.salesinvoice.code
            new TranslationSeedItem("entity.salesinvoice.code", "en-US", "销售发票编码_us", "销售发票编码（唯一索引）"),
            // entity.salesinvoice.code
            new TranslationSeedItem("entity.salesinvoice.code", "ja-JP", "销售发票编码_jp", "销售发票编码（唯一索引）"),
            // entity.salesinvoice.code
            new TranslationSeedItem("entity.salesinvoice.code", "zh-CN", "销售发票编码", "销售发票编码（唯一索引）"),
            // entity.salesinvoice.code
            new TranslationSeedItem("entity.salesinvoice.code", "zh-HK", "销售发票编码_hk", "销售发票编码（唯一索引）"),

            // entity.salesinvoice.salesordercode
            new TranslationSeedItem("entity.salesinvoice.salesordercode", "en-US", "销售订单编码_us", "关联销售订单编码"),
            // entity.salesinvoice.salesordercode
            new TranslationSeedItem("entity.salesinvoice.salesordercode", "ja-JP", "销售订单编码_jp", "关联销售订单编码"),
            // entity.salesinvoice.salesordercode
            new TranslationSeedItem("entity.salesinvoice.salesordercode", "zh-CN", "销售订单编码", "关联销售订单编码"),
            // entity.salesinvoice.salesordercode
            new TranslationSeedItem("entity.salesinvoice.salesordercode", "zh-HK", "销售订单编码_hk", "关联销售订单编码"),

            // entity.salesinvoice.customercode
            new TranslationSeedItem("entity.salesinvoice.customercode", "en-US", "客户编码_us", "客户编码"),
            // entity.salesinvoice.customercode
            new TranslationSeedItem("entity.salesinvoice.customercode", "ja-JP", "客户编码_jp", "客户编码"),
            // entity.salesinvoice.customercode
            new TranslationSeedItem("entity.salesinvoice.customercode", "zh-CN", "客户编码", "客户编码"),
            // entity.salesinvoice.customercode
            new TranslationSeedItem("entity.salesinvoice.customercode", "zh-HK", "客户编码_hk", "客户编码"),

            // entity.salesinvoice.customername
            new TranslationSeedItem("entity.salesinvoice.customername", "en-US", "客户名称_us", "客户名称"),
            // entity.salesinvoice.customername
            new TranslationSeedItem("entity.salesinvoice.customername", "ja-JP", "客户名称_jp", "客户名称"),
            // entity.salesinvoice.customername
            new TranslationSeedItem("entity.salesinvoice.customername", "zh-CN", "客户名称", "客户名称"),
            // entity.salesinvoice.customername
            new TranslationSeedItem("entity.salesinvoice.customername", "zh-HK", "客户名称_hk", "客户名称"),

            // entity.salesinvoice.invoicedate
            new TranslationSeedItem("entity.salesinvoice.invoicedate", "en-US", "开票日期_us", "开票日期"),
            // entity.salesinvoice.invoicedate
            new TranslationSeedItem("entity.salesinvoice.invoicedate", "ja-JP", "开票日期_jp", "开票日期"),
            // entity.salesinvoice.invoicedate
            new TranslationSeedItem("entity.salesinvoice.invoicedate", "zh-CN", "开票日期", "开票日期"),
            // entity.salesinvoice.invoicedate
            new TranslationSeedItem("entity.salesinvoice.invoicedate", "zh-HK", "开票日期_hk", "开票日期"),

            // entity.salesinvoice.totalamount
            new TranslationSeedItem("entity.salesinvoice.totalamount", "en-US", "发票总金额_us", "发票总金额"),
            // entity.salesinvoice.totalamount
            new TranslationSeedItem("entity.salesinvoice.totalamount", "ja-JP", "发票总金额_jp", "发票总金额"),
            // entity.salesinvoice.totalamount
            new TranslationSeedItem("entity.salesinvoice.totalamount", "zh-CN", "发票总金额", "发票总金额"),
            // entity.salesinvoice.totalamount
            new TranslationSeedItem("entity.salesinvoice.totalamount", "zh-HK", "发票总金额_hk", "发票总金额"),

            // entity.salesinvoice.taxamount
            new TranslationSeedItem("entity.salesinvoice.taxamount", "en-US", "税费_us", "税费"),
            // entity.salesinvoice.taxamount
            new TranslationSeedItem("entity.salesinvoice.taxamount", "ja-JP", "税费_jp", "税费"),
            // entity.salesinvoice.taxamount
            new TranslationSeedItem("entity.salesinvoice.taxamount", "zh-CN", "税费", "税费"),
            // entity.salesinvoice.taxamount
            new TranslationSeedItem("entity.salesinvoice.taxamount", "zh-HK", "税费_hk", "税费"),

            // entity.salesinvoice.actualamount
            new TranslationSeedItem("entity.salesinvoice.actualamount", "en-US", "发票实付金额_us", "发票实付金额"),
            // entity.salesinvoice.actualamount
            new TranslationSeedItem("entity.salesinvoice.actualamount", "ja-JP", "发票实付金额_jp", "发票实付金额"),
            // entity.salesinvoice.actualamount
            new TranslationSeedItem("entity.salesinvoice.actualamount", "zh-CN", "发票实付金额", "发票实付金额"),
            // entity.salesinvoice.actualamount
            new TranslationSeedItem("entity.salesinvoice.actualamount", "zh-HK", "发票实付金额_hk", "发票实付金额"),

            // entity.salesinvoice.invoicestatus
            new TranslationSeedItem("entity.salesinvoice.invoicestatus", "en-US", "发票状态_us", "发票状态（字典 logistics_invoice_status；0=草稿，1=已开票，2=已收款，3=已作废）"),
            // entity.salesinvoice.invoicestatus
            new TranslationSeedItem("entity.salesinvoice.invoicestatus", "ja-JP", "发票状态_jp", "发票状态（字典 logistics_invoice_status；0=草稿，1=已开票，2=已收款，3=已作废）"),
            // entity.salesinvoice.invoicestatus
            new TranslationSeedItem("entity.salesinvoice.invoicestatus", "zh-CN", "发票状态", "发票状态（字典 logistics_invoice_status；0=草稿，1=已开票，2=已收款，3=已作废）"),
            // entity.salesinvoice.invoicestatus
            new TranslationSeedItem("entity.salesinvoice.invoicestatus", "zh-HK", "发票状态_hk", "发票状态（字典 logistics_invoice_status；0=草稿，1=已开票，2=已收款，3=已作废）"),

            // entity.salesinvoice.paymentmethod
            new TranslationSeedItem("entity.salesinvoice.paymentmethod", "en-US", "收款方式_us", "收款方式（字典 logistics_payment_method_type；0=现金，1=银行转账，2=支票，3=信用证，4=其他）"),
            // entity.salesinvoice.paymentmethod
            new TranslationSeedItem("entity.salesinvoice.paymentmethod", "ja-JP", "收款方式_jp", "收款方式（字典 logistics_payment_method_type；0=现金，1=银行转账，2=支票，3=信用证，4=其他）"),
            // entity.salesinvoice.paymentmethod
            new TranslationSeedItem("entity.salesinvoice.paymentmethod", "zh-CN", "收款方式", "收款方式（字典 logistics_payment_method_type；0=现金，1=银行转账，2=支票，3=信用证，4=其他）"),
            // entity.salesinvoice.paymentmethod
            new TranslationSeedItem("entity.salesinvoice.paymentmethod", "zh-HK", "收款方式_hk", "收款方式（字典 logistics_payment_method_type；0=现金，1=银行转账，2=支票，3=信用证，4=其他）"),

            // entity.salesinvoice.taxinvoiceno
            new TranslationSeedItem("entity.salesinvoice.taxinvoiceno", "en-US", "税务发票号码_us", "发票号码（税务系统票号）"),
            // entity.salesinvoice.taxinvoiceno
            new TranslationSeedItem("entity.salesinvoice.taxinvoiceno", "ja-JP", "税务发票号码_jp", "发票号码（税务系统票号）"),
            // entity.salesinvoice.taxinvoiceno
            new TranslationSeedItem("entity.salesinvoice.taxinvoiceno", "zh-CN", "税务发票号码", "发票号码（税务系统票号）"),
            // entity.salesinvoice.taxinvoiceno
            new TranslationSeedItem("entity.salesinvoice.taxinvoiceno", "zh-HK", "税务发票号码_hk", "发票号码（税务系统票号）"),

            // entity.salesinvoice.items
            new TranslationSeedItem("entity.salesinvoice.items", "en-US", "销售发票明细列表_us", "销售发票明细列表（主子表关系，一张发票可有多个明细行）"),
            // entity.salesinvoice.items
            new TranslationSeedItem("entity.salesinvoice.items", "ja-JP", "销售发票明细列表_jp", "销售发票明细列表（主子表关系，一张发票可有多个明细行）"),
            // entity.salesinvoice.items
            new TranslationSeedItem("entity.salesinvoice.items", "zh-CN", "销售发票明细列表", "销售发票明细列表（主子表关系，一张发票可有多个明细行）"),
            // entity.salesinvoice.items
            new TranslationSeedItem("entity.salesinvoice.items", "zh-HK", "销售发票明细列表_hk", "销售发票明细列表（主子表关系，一张发票可有多个明细行）"),
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
