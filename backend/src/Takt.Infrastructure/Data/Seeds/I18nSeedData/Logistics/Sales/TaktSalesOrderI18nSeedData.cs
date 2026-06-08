// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Sales
// 文件名称：TaktSalesOrderI18nSeedData.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSalesOrder 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktSalesOrder 实体国际化翻译种子（键前缀 entity.salesOrder.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSalesOrderI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSalesOrder 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 salesOrder 实体翻译...", tenantCode);

        foreach (var item in GetSalesOrderTranslations())
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

        TaktLogger.Information("TaktSalesOrder 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSalesOrder 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.salesOrder._self / entity.salesOrder.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSalesOrderTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.salesOrder._self
            new TranslationSeedItem("entity.salesOrder._self", "en-US", "Sales Order Information", "实体名称"),
            // entity.salesOrder._self
            new TranslationSeedItem("entity.salesOrder._self", "ja-JP", "Takt销售订单信息", "实体名称"),
            // entity.salesOrder._self
            new TranslationSeedItem("entity.salesOrder._self", "zh-CN", "Takt销售订单信息", "实体名称"),
            // entity.salesOrder._self
            new TranslationSeedItem("entity.salesOrder._self", "zh-HK", "Takt销售订单信息", "实体名称"),

            // entity.salesOrder.plantcode
            new TranslationSeedItem("entity.salesOrder.plantcode", "en-US", "工厂代码", "工厂代码"),
            // entity.salesOrder.plantcode
            new TranslationSeedItem("entity.salesOrder.plantcode", "ja-JP", "工厂代码", "工厂代码"),
            // entity.salesOrder.plantcode
            new TranslationSeedItem("entity.salesOrder.plantcode", "zh-CN", "工厂代码", "工厂代码"),
            // entity.salesOrder.plantcode
            new TranslationSeedItem("entity.salesOrder.plantcode", "zh-HK", "工厂代码", "工厂代码"),

            // entity.salesOrder.code
            new TranslationSeedItem("entity.salesOrder.code", "en-US", "销售订单编码", "销售订单编码（唯一索引）"),
            // entity.salesOrder.code
            new TranslationSeedItem("entity.salesOrder.code", "ja-JP", "销售订单编码", "销售订单编码（唯一索引）"),
            // entity.salesOrder.code
            new TranslationSeedItem("entity.salesOrder.code", "zh-CN", "销售订单编码", "销售订单编码（唯一索引）"),
            // entity.salesOrder.code
            new TranslationSeedItem("entity.salesOrder.code", "zh-HK", "销售订单编码", "销售订单编码（唯一索引）"),

            // entity.salesOrder.customercode
            new TranslationSeedItem("entity.salesOrder.customercode", "en-US", "客户编码", "客户编码"),
            // entity.salesOrder.customercode
            new TranslationSeedItem("entity.salesOrder.customercode", "ja-JP", "客户编码", "客户编码"),
            // entity.salesOrder.customercode
            new TranslationSeedItem("entity.salesOrder.customercode", "zh-CN", "客户编码", "客户编码"),
            // entity.salesOrder.customercode
            new TranslationSeedItem("entity.salesOrder.customercode", "zh-HK", "客户编码", "客户编码"),

            // entity.salesOrder.customername
            new TranslationSeedItem("entity.salesOrder.customername", "en-US", "客户名称", "客户名称"),
            // entity.salesOrder.customername
            new TranslationSeedItem("entity.salesOrder.customername", "ja-JP", "客户名称", "客户名称"),
            // entity.salesOrder.customername
            new TranslationSeedItem("entity.salesOrder.customername", "zh-CN", "客户名称", "客户名称"),
            // entity.salesOrder.customername
            new TranslationSeedItem("entity.salesOrder.customername", "zh-HK", "客户名称", "客户名称"),

            // entity.salesOrder.orderdate
            new TranslationSeedItem("entity.salesOrder.orderdate", "en-US", "订单日期", "订单日期"),
            // entity.salesOrder.orderdate
            new TranslationSeedItem("entity.salesOrder.orderdate", "ja-JP", "订单日期", "订单日期"),
            // entity.salesOrder.orderdate
            new TranslationSeedItem("entity.salesOrder.orderdate", "zh-CN", "订单日期", "订单日期"),
            // entity.salesOrder.orderdate
            new TranslationSeedItem("entity.salesOrder.orderdate", "zh-HK", "订单日期", "订单日期"),

            // entity.salesOrder.requireddeliverydate
            new TranslationSeedItem("entity.salesOrder.requireddeliverydate", "en-US", "要求交货日期", "要求交货日期"),
            // entity.salesOrder.requireddeliverydate
            new TranslationSeedItem("entity.salesOrder.requireddeliverydate", "ja-JP", "要求交货日期", "要求交货日期"),
            // entity.salesOrder.requireddeliverydate
            new TranslationSeedItem("entity.salesOrder.requireddeliverydate", "zh-CN", "要求交货日期", "要求交货日期"),
            // entity.salesOrder.requireddeliverydate
            new TranslationSeedItem("entity.salesOrder.requireddeliverydate", "zh-HK", "要求交货日期", "要求交货日期"),

            // entity.salesOrder.actualdeliverydate
            new TranslationSeedItem("entity.salesOrder.actualdeliverydate", "en-US", "实际交货日期", "实际交货日期"),
            // entity.salesOrder.actualdeliverydate
            new TranslationSeedItem("entity.salesOrder.actualdeliverydate", "ja-JP", "实际交货日期", "实际交货日期"),
            // entity.salesOrder.actualdeliverydate
            new TranslationSeedItem("entity.salesOrder.actualdeliverydate", "zh-CN", "实际交货日期", "实际交货日期"),
            // entity.salesOrder.actualdeliverydate
            new TranslationSeedItem("entity.salesOrder.actualdeliverydate", "zh-HK", "实际交货日期", "实际交货日期"),

            // entity.salesOrder.salesby
            new TranslationSeedItem("entity.salesOrder.salesby", "en-US", "销售员", "销售员（人员代码）"),
            // entity.salesOrder.salesby
            new TranslationSeedItem("entity.salesOrder.salesby", "ja-JP", "销售员", "销售员（人员代码）"),
            // entity.salesOrder.salesby
            new TranslationSeedItem("entity.salesOrder.salesby", "zh-CN", "销售员", "销售员（人员代码）"),
            // entity.salesOrder.salesby
            new TranslationSeedItem("entity.salesOrder.salesby", "zh-HK", "销售员", "销售员（人员代码）"),

            // entity.salesOrder.totalquantity
            new TranslationSeedItem("entity.salesOrder.totalquantity", "en-US", "订单总数量", "订单总数量（基本单位数量）"),
            // entity.salesOrder.totalquantity
            new TranslationSeedItem("entity.salesOrder.totalquantity", "ja-JP", "订单总数量", "订单总数量（基本单位数量）"),
            // entity.salesOrder.totalquantity
            new TranslationSeedItem("entity.salesOrder.totalquantity", "zh-CN", "订单总数量", "订单总数量（基本单位数量）"),
            // entity.salesOrder.totalquantity
            new TranslationSeedItem("entity.salesOrder.totalquantity", "zh-HK", "订单总数量", "订单总数量（基本单位数量）"),

            // entity.salesOrder.totalamount
            new TranslationSeedItem("entity.salesOrder.totalamount", "en-US", "订单总金额", "订单总金额（精确到分，存储为整数，单位为分）"),
            // entity.salesOrder.totalamount
            new TranslationSeedItem("entity.salesOrder.totalamount", "ja-JP", "订单总金额", "订单总金额（精确到分，存储为整数，单位为分）"),
            // entity.salesOrder.totalamount
            new TranslationSeedItem("entity.salesOrder.totalamount", "zh-CN", "订单总金额", "订单总金额（精确到分，存储为整数，单位为分）"),
            // entity.salesOrder.totalamount
            new TranslationSeedItem("entity.salesOrder.totalamount", "zh-HK", "订单总金额", "订单总金额（精确到分，存储为整数，单位为分）"),

            // entity.salesOrder.discountamount
            new TranslationSeedItem("entity.salesOrder.discountamount", "en-US", "折扣金额", "折扣金额（精确到分，存储为整数，单位为分）"),
            // entity.salesOrder.discountamount
            new TranslationSeedItem("entity.salesOrder.discountamount", "ja-JP", "折扣金额", "折扣金额（精确到分，存储为整数，单位为分）"),
            // entity.salesOrder.discountamount
            new TranslationSeedItem("entity.salesOrder.discountamount", "zh-CN", "折扣金额", "折扣金额（精确到分，存储为整数，单位为分）"),
            // entity.salesOrder.discountamount
            new TranslationSeedItem("entity.salesOrder.discountamount", "zh-HK", "折扣金额", "折扣金额（精确到分，存储为整数，单位为分）"),

            // entity.salesOrder.taxamount
            new TranslationSeedItem("entity.salesOrder.taxamount", "en-US", "税费", "税费（精确到分，存储为整数，单位为分）"),
            // entity.salesOrder.taxamount
            new TranslationSeedItem("entity.salesOrder.taxamount", "ja-JP", "税费", "税费（精确到分，存储为整数，单位为分）"),
            // entity.salesOrder.taxamount
            new TranslationSeedItem("entity.salesOrder.taxamount", "zh-CN", "税费", "税费（精确到分，存储为整数，单位为分）"),
            // entity.salesOrder.taxamount
            new TranslationSeedItem("entity.salesOrder.taxamount", "zh-HK", "税费", "税费（精确到分，存储为整数，单位为分）"),

            // entity.salesOrder.actualamount
            new TranslationSeedItem("entity.salesOrder.actualamount", "en-US", "订单实付金额", "订单实付金额（精确到分，存储为整数，单位为分）"),
            // entity.salesOrder.actualamount
            new TranslationSeedItem("entity.salesOrder.actualamount", "ja-JP", "订单实付金额", "订单实付金额（精确到分，存储为整数，单位为分）"),
            // entity.salesOrder.actualamount
            new TranslationSeedItem("entity.salesOrder.actualamount", "zh-CN", "订单实付金额", "订单实付金额（精确到分，存储为整数，单位为分）"),
            // entity.salesOrder.actualamount
            new TranslationSeedItem("entity.salesOrder.actualamount", "zh-HK", "订单实付金额", "订单实付金额（精确到分，存储为整数，单位为分）"),

            // entity.salesOrder.shippedquantity
            new TranslationSeedItem("entity.salesOrder.shippedquantity", "en-US", "已发货数量", "已发货数量（基本单位数量）"),
            // entity.salesOrder.shippedquantity
            new TranslationSeedItem("entity.salesOrder.shippedquantity", "ja-JP", "已发货数量", "已发货数量（基本单位数量）"),
            // entity.salesOrder.shippedquantity
            new TranslationSeedItem("entity.salesOrder.shippedquantity", "zh-CN", "已发货数量", "已发货数量（基本单位数量）"),
            // entity.salesOrder.shippedquantity
            new TranslationSeedItem("entity.salesOrder.shippedquantity", "zh-HK", "已发货数量", "已发货数量（基本单位数量）"),

            // entity.salesOrder.shippedamount
            new TranslationSeedItem("entity.salesOrder.shippedamount", "en-US", "已发货金额", "已发货金额（精确到分，存储为整数，单位为分）"),
            // entity.salesOrder.shippedamount
            new TranslationSeedItem("entity.salesOrder.shippedamount", "ja-JP", "已发货金额", "已发货金额（精确到分，存储为整数，单位为分）"),
            // entity.salesOrder.shippedamount
            new TranslationSeedItem("entity.salesOrder.shippedamount", "zh-CN", "已发货金额", "已发货金额（精确到分，存储为整数，单位为分）"),
            // entity.salesOrder.shippedamount
            new TranslationSeedItem("entity.salesOrder.shippedamount", "zh-HK", "已发货金额", "已发货金额（精确到分，存储为整数，单位为分）"),

            // entity.salesOrder.receivedamount
            new TranslationSeedItem("entity.salesOrder.receivedamount", "en-US", "已收款金额", "已收款金额（精确到分，存储为整数，单位为分）"),
            // entity.salesOrder.receivedamount
            new TranslationSeedItem("entity.salesOrder.receivedamount", "ja-JP", "已收款金额", "已收款金额（精确到分，存储为整数，单位为分）"),
            // entity.salesOrder.receivedamount
            new TranslationSeedItem("entity.salesOrder.receivedamount", "zh-CN", "已收款金额", "已收款金额（精确到分，存储为整数，单位为分）"),
            // entity.salesOrder.receivedamount
            new TranslationSeedItem("entity.salesOrder.receivedamount", "zh-HK", "已收款金额", "已收款金额（精确到分，存储为整数，单位为分）"),

            // entity.salesOrder.orderstatus
            new TranslationSeedItem("entity.salesOrder.orderstatus", "en-US", "订单状态", "订单状态（1=启用，0=禁用）"),
            // entity.salesOrder.orderstatus
            new TranslationSeedItem("entity.salesOrder.orderstatus", "ja-JP", "订单状态", "订单状态（1=启用，0=禁用）"),
            // entity.salesOrder.orderstatus
            new TranslationSeedItem("entity.salesOrder.orderstatus", "zh-CN", "订单状态", "订单状态（1=启用，0=禁用）"),
            // entity.salesOrder.orderstatus
            new TranslationSeedItem("entity.salesOrder.orderstatus", "zh-HK", "订单状态", "订单状态（1=启用，0=禁用）"),

            // entity.salesOrder.deliverystatus
            new TranslationSeedItem("entity.salesOrder.deliverystatus", "en-US", "交货状态", "交货状态（0=未交货，1=部分交货，2=全部交货）"),
            // entity.salesOrder.deliverystatus
            new TranslationSeedItem("entity.salesOrder.deliverystatus", "ja-JP", "交货状态", "交货状态（0=未交货，1=部分交货，2=全部交货）"),
            // entity.salesOrder.deliverystatus
            new TranslationSeedItem("entity.salesOrder.deliverystatus", "zh-CN", "交货状态", "交货状态（0=未交货，1=部分交货，2=全部交货）"),
            // entity.salesOrder.deliverystatus
            new TranslationSeedItem("entity.salesOrder.deliverystatus", "zh-HK", "交货状态", "交货状态（0=未交货，1=部分交货，2=全部交货）"),

            // entity.salesOrder.deliverymethod
            new TranslationSeedItem("entity.salesOrder.deliverymethod", "en-US", "交货方式", "交货方式（0=自提，1=送货上门，2=物流配送，3=快递）"),
            // entity.salesOrder.deliverymethod
            new TranslationSeedItem("entity.salesOrder.deliverymethod", "ja-JP", "交货方式", "交货方式（0=自提，1=送货上门，2=物流配送，3=快递）"),
            // entity.salesOrder.deliverymethod
            new TranslationSeedItem("entity.salesOrder.deliverymethod", "zh-CN", "交货方式", "交货方式（0=自提，1=送货上门，2=物流配送，3=快递）"),
            // entity.salesOrder.deliverymethod
            new TranslationSeedItem("entity.salesOrder.deliverymethod", "zh-HK", "交货方式", "交货方式（0=自提，1=送货上门，2=物流配送，3=快递）"),

            // entity.salesOrder.paymentmethod
            new TranslationSeedItem("entity.salesOrder.paymentmethod", "en-US", "收款方式", "收款方式（0=现金，1=银行转账，2=支票，3=信用证，4=其他）"),
            // entity.salesOrder.paymentmethod
            new TranslationSeedItem("entity.salesOrder.paymentmethod", "ja-JP", "收款方式", "收款方式（0=现金，1=银行转账，2=支票，3=信用证，4=其他）"),
            // entity.salesOrder.paymentmethod
            new TranslationSeedItem("entity.salesOrder.paymentmethod", "zh-CN", "收款方式", "收款方式（0=现金，1=银行转账，2=支票，3=信用证，4=其他）"),
            // entity.salesOrder.paymentmethod
            new TranslationSeedItem("entity.salesOrder.paymentmethod", "zh-HK", "收款方式", "收款方式（0=现金，1=银行转账，2=支票，3=信用证，4=其他）"),

            // entity.salesOrder.deliveryaddress
            new TranslationSeedItem("entity.salesOrder.deliveryaddress", "en-US", "交货地址", "交货地址"),
            // entity.salesOrder.deliveryaddress
            new TranslationSeedItem("entity.salesOrder.deliveryaddress", "ja-JP", "交货地址", "交货地址"),
            // entity.salesOrder.deliveryaddress
            new TranslationSeedItem("entity.salesOrder.deliveryaddress", "zh-CN", "交货地址", "交货地址"),
            // entity.salesOrder.deliveryaddress
            new TranslationSeedItem("entity.salesOrder.deliveryaddress", "zh-HK", "交货地址", "交货地址"),

            // entity.salesOrder.items
            new TranslationSeedItem("entity.salesOrder.items", "en-US", "销售订单明细列表", "销售订单明细列表（主子表关系，一个订单可以有多个明细）"),
            // entity.salesOrder.items
            new TranslationSeedItem("entity.salesOrder.items", "ja-JP", "销售订单明细列表", "销售订单明细列表（主子表关系，一个订单可以有多个明细）"),
            // entity.salesOrder.items
            new TranslationSeedItem("entity.salesOrder.items", "zh-CN", "销售订单明细列表", "销售订单明细列表（主子表关系，一个订单可以有多个明细）"),
            // entity.salesOrder.items
            new TranslationSeedItem("entity.salesOrder.items", "zh-HK", "销售订单明细列表", "销售订单明细列表（主子表关系，一个订单可以有多个明细）"),

            // entity.salesOrder.changelogs
            new TranslationSeedItem("entity.salesOrder.changelogs", "en-US", "销售订单变更记录列表", "销售订单变更记录列表（外键在子表 <see cref=\"TaktSalesOrderChangeLog.OrderId\"/>）"),
            // entity.salesOrder.changelogs
            new TranslationSeedItem("entity.salesOrder.changelogs", "ja-JP", "销售订单变更记录列表", "销售订单变更记录列表（外键在子表 <see cref=\"TaktSalesOrderChangeLog.OrderId\"/>）"),
            // entity.salesOrder.changelogs
            new TranslationSeedItem("entity.salesOrder.changelogs", "zh-CN", "销售订单变更记录列表", "销售订单变更记录列表（外键在子表 <see cref=\"TaktSalesOrderChangeLog.OrderId\"/>）"),
            // entity.salesOrder.changelogs
            new TranslationSeedItem("entity.salesOrder.changelogs", "zh-HK", "销售订单变更记录列表", "销售订单变更记录列表（外键在子表 <see cref=\"TaktSalesOrderChangeLog.OrderId\"/>）"),
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
