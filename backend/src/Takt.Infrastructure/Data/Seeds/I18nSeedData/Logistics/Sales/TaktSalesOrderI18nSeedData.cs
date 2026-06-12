// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Sales
// 文件名称：TaktSalesOrderI18nSeedData.cs
// 创建时间：2026-06-12
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
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Sales;

/// <summary>
/// TaktSalesOrder 实体国际化翻译种子（键前缀 entity.salesorder.*）
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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 salesorder 实体翻译...", tenantCode);

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
    /// I18nKey：entity.salesorder._self / entity.salesorder.{{field}}；ResourceGroup=4；ResourceType=0
    /// </summary>
    private static List<TranslationSeedItem> GetSalesOrderTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.salesorder._self
            new TranslationSeedItem("entity.salesorder._self", "en-US", "Sales Order Information", "实体名称"),
            // entity.salesorder._self
            new TranslationSeedItem("entity.salesorder._self", "ja-JP", "Takt销售订单信息", "实体名称"),
            // entity.salesorder._self
            new TranslationSeedItem("entity.salesorder._self", "zh-CN", "Takt销售订单信息", "实体名称"),
            // entity.salesorder._self
            new TranslationSeedItem("entity.salesorder._self", "zh-HK", "Takt销售订单信息", "实体名称"),

            // entity.salesorder.plantcode
            new TranslationSeedItem("entity.salesorder.plantcode", "en-US", "工厂代码", "工厂代码"),
            // entity.salesorder.plantcode
            new TranslationSeedItem("entity.salesorder.plantcode", "ja-JP", "工厂代码", "工厂代码"),
            // entity.salesorder.plantcode
            new TranslationSeedItem("entity.salesorder.plantcode", "zh-CN", "工厂代码", "工厂代码"),
            // entity.salesorder.plantcode
            new TranslationSeedItem("entity.salesorder.plantcode", "zh-HK", "工厂代码", "工厂代码"),

            // entity.salesorder.code
            new TranslationSeedItem("entity.salesorder.code", "en-US", "销售订单编码", "销售订单编码（唯一索引）"),
            // entity.salesorder.code
            new TranslationSeedItem("entity.salesorder.code", "ja-JP", "销售订单编码", "销售订单编码（唯一索引）"),
            // entity.salesorder.code
            new TranslationSeedItem("entity.salesorder.code", "zh-CN", "销售订单编码", "销售订单编码（唯一索引）"),
            // entity.salesorder.code
            new TranslationSeedItem("entity.salesorder.code", "zh-HK", "销售订单编码", "销售订单编码（唯一索引）"),

            // entity.salesorder.customercode
            new TranslationSeedItem("entity.salesorder.customercode", "en-US", "客户编码", "客户编码"),
            // entity.salesorder.customercode
            new TranslationSeedItem("entity.salesorder.customercode", "ja-JP", "客户编码", "客户编码"),
            // entity.salesorder.customercode
            new TranslationSeedItem("entity.salesorder.customercode", "zh-CN", "客户编码", "客户编码"),
            // entity.salesorder.customercode
            new TranslationSeedItem("entity.salesorder.customercode", "zh-HK", "客户编码", "客户编码"),

            // entity.salesorder.customername
            new TranslationSeedItem("entity.salesorder.customername", "en-US", "客户名称", "客户名称"),
            // entity.salesorder.customername
            new TranslationSeedItem("entity.salesorder.customername", "ja-JP", "客户名称", "客户名称"),
            // entity.salesorder.customername
            new TranslationSeedItem("entity.salesorder.customername", "zh-CN", "客户名称", "客户名称"),
            // entity.salesorder.customername
            new TranslationSeedItem("entity.salesorder.customername", "zh-HK", "客户名称", "客户名称"),

            // entity.salesorder.orderdate
            new TranslationSeedItem("entity.salesorder.orderdate", "en-US", "订单日期", "订单日期"),
            // entity.salesorder.orderdate
            new TranslationSeedItem("entity.salesorder.orderdate", "ja-JP", "订单日期", "订单日期"),
            // entity.salesorder.orderdate
            new TranslationSeedItem("entity.salesorder.orderdate", "zh-CN", "订单日期", "订单日期"),
            // entity.salesorder.orderdate
            new TranslationSeedItem("entity.salesorder.orderdate", "zh-HK", "订单日期", "订单日期"),

            // entity.salesorder.requireddeliverydate
            new TranslationSeedItem("entity.salesorder.requireddeliverydate", "en-US", "要求交货日期", "要求交货日期"),
            // entity.salesorder.requireddeliverydate
            new TranslationSeedItem("entity.salesorder.requireddeliverydate", "ja-JP", "要求交货日期", "要求交货日期"),
            // entity.salesorder.requireddeliverydate
            new TranslationSeedItem("entity.salesorder.requireddeliverydate", "zh-CN", "要求交货日期", "要求交货日期"),
            // entity.salesorder.requireddeliverydate
            new TranslationSeedItem("entity.salesorder.requireddeliverydate", "zh-HK", "要求交货日期", "要求交货日期"),

            // entity.salesorder.actualdeliverydate
            new TranslationSeedItem("entity.salesorder.actualdeliverydate", "en-US", "实际交货日期", "实际交货日期"),
            // entity.salesorder.actualdeliverydate
            new TranslationSeedItem("entity.salesorder.actualdeliverydate", "ja-JP", "实际交货日期", "实际交货日期"),
            // entity.salesorder.actualdeliverydate
            new TranslationSeedItem("entity.salesorder.actualdeliverydate", "zh-CN", "实际交货日期", "实际交货日期"),
            // entity.salesorder.actualdeliverydate
            new TranslationSeedItem("entity.salesorder.actualdeliverydate", "zh-HK", "实际交货日期", "实际交货日期"),

            // entity.salesorder.salesby
            new TranslationSeedItem("entity.salesorder.salesby", "en-US", "销售员", "销售员（人员代码）"),
            // entity.salesorder.salesby
            new TranslationSeedItem("entity.salesorder.salesby", "ja-JP", "销售员", "销售员（人员代码）"),
            // entity.salesorder.salesby
            new TranslationSeedItem("entity.salesorder.salesby", "zh-CN", "销售员", "销售员（人员代码）"),
            // entity.salesorder.salesby
            new TranslationSeedItem("entity.salesorder.salesby", "zh-HK", "销售员", "销售员（人员代码）"),

            // entity.salesorder.totalquantity
            new TranslationSeedItem("entity.salesorder.totalquantity", "en-US", "订单总数量", "订单总数量（基本单位数量）"),
            // entity.salesorder.totalquantity
            new TranslationSeedItem("entity.salesorder.totalquantity", "ja-JP", "订单总数量", "订单总数量（基本单位数量）"),
            // entity.salesorder.totalquantity
            new TranslationSeedItem("entity.salesorder.totalquantity", "zh-CN", "订单总数量", "订单总数量（基本单位数量）"),
            // entity.salesorder.totalquantity
            new TranslationSeedItem("entity.salesorder.totalquantity", "zh-HK", "订单总数量", "订单总数量（基本单位数量）"),

            // entity.salesorder.totalamount
            new TranslationSeedItem("entity.salesorder.totalamount", "en-US", "订单总金额", "订单总金额（精确到分，存储为整数，单位为分）"),
            // entity.salesorder.totalamount
            new TranslationSeedItem("entity.salesorder.totalamount", "ja-JP", "订单总金额", "订单总金额（精确到分，存储为整数，单位为分）"),
            // entity.salesorder.totalamount
            new TranslationSeedItem("entity.salesorder.totalamount", "zh-CN", "订单总金额", "订单总金额（精确到分，存储为整数，单位为分）"),
            // entity.salesorder.totalamount
            new TranslationSeedItem("entity.salesorder.totalamount", "zh-HK", "订单总金额", "订单总金额（精确到分，存储为整数，单位为分）"),

            // entity.salesorder.discountamount
            new TranslationSeedItem("entity.salesorder.discountamount", "en-US", "折扣金额", "折扣金额（精确到分，存储为整数，单位为分）"),
            // entity.salesorder.discountamount
            new TranslationSeedItem("entity.salesorder.discountamount", "ja-JP", "折扣金额", "折扣金额（精确到分，存储为整数，单位为分）"),
            // entity.salesorder.discountamount
            new TranslationSeedItem("entity.salesorder.discountamount", "zh-CN", "折扣金额", "折扣金额（精确到分，存储为整数，单位为分）"),
            // entity.salesorder.discountamount
            new TranslationSeedItem("entity.salesorder.discountamount", "zh-HK", "折扣金额", "折扣金额（精确到分，存储为整数，单位为分）"),

            // entity.salesorder.taxamount
            new TranslationSeedItem("entity.salesorder.taxamount", "en-US", "税费", "税费（精确到分，存储为整数，单位为分）"),
            // entity.salesorder.taxamount
            new TranslationSeedItem("entity.salesorder.taxamount", "ja-JP", "税费", "税费（精确到分，存储为整数，单位为分）"),
            // entity.salesorder.taxamount
            new TranslationSeedItem("entity.salesorder.taxamount", "zh-CN", "税费", "税费（精确到分，存储为整数，单位为分）"),
            // entity.salesorder.taxamount
            new TranslationSeedItem("entity.salesorder.taxamount", "zh-HK", "税费", "税费（精确到分，存储为整数，单位为分）"),

            // entity.salesorder.actualamount
            new TranslationSeedItem("entity.salesorder.actualamount", "en-US", "订单实付金额", "订单实付金额（精确到分，存储为整数，单位为分）"),
            // entity.salesorder.actualamount
            new TranslationSeedItem("entity.salesorder.actualamount", "ja-JP", "订单实付金额", "订单实付金额（精确到分，存储为整数，单位为分）"),
            // entity.salesorder.actualamount
            new TranslationSeedItem("entity.salesorder.actualamount", "zh-CN", "订单实付金额", "订单实付金额（精确到分，存储为整数，单位为分）"),
            // entity.salesorder.actualamount
            new TranslationSeedItem("entity.salesorder.actualamount", "zh-HK", "订单实付金额", "订单实付金额（精确到分，存储为整数，单位为分）"),

            // entity.salesorder.shippedquantity
            new TranslationSeedItem("entity.salesorder.shippedquantity", "en-US", "已发货数量", "已发货数量（基本单位数量）"),
            // entity.salesorder.shippedquantity
            new TranslationSeedItem("entity.salesorder.shippedquantity", "ja-JP", "已发货数量", "已发货数量（基本单位数量）"),
            // entity.salesorder.shippedquantity
            new TranslationSeedItem("entity.salesorder.shippedquantity", "zh-CN", "已发货数量", "已发货数量（基本单位数量）"),
            // entity.salesorder.shippedquantity
            new TranslationSeedItem("entity.salesorder.shippedquantity", "zh-HK", "已发货数量", "已发货数量（基本单位数量）"),

            // entity.salesorder.shippedamount
            new TranslationSeedItem("entity.salesorder.shippedamount", "en-US", "已发货金额", "已发货金额（精确到分，存储为整数，单位为分）"),
            // entity.salesorder.shippedamount
            new TranslationSeedItem("entity.salesorder.shippedamount", "ja-JP", "已发货金额", "已发货金额（精确到分，存储为整数，单位为分）"),
            // entity.salesorder.shippedamount
            new TranslationSeedItem("entity.salesorder.shippedamount", "zh-CN", "已发货金额", "已发货金额（精确到分，存储为整数，单位为分）"),
            // entity.salesorder.shippedamount
            new TranslationSeedItem("entity.salesorder.shippedamount", "zh-HK", "已发货金额", "已发货金额（精确到分，存储为整数，单位为分）"),

            // entity.salesorder.receivedamount
            new TranslationSeedItem("entity.salesorder.receivedamount", "en-US", "已收款金额", "已收款金额（精确到分，存储为整数，单位为分）"),
            // entity.salesorder.receivedamount
            new TranslationSeedItem("entity.salesorder.receivedamount", "ja-JP", "已收款金额", "已收款金额（精确到分，存储为整数，单位为分）"),
            // entity.salesorder.receivedamount
            new TranslationSeedItem("entity.salesorder.receivedamount", "zh-CN", "已收款金额", "已收款金额（精确到分，存储为整数，单位为分）"),
            // entity.salesorder.receivedamount
            new TranslationSeedItem("entity.salesorder.receivedamount", "zh-HK", "已收款金额", "已收款金额（精确到分，存储为整数，单位为分）"),

            // entity.salesorder.orderstatus
            new TranslationSeedItem("entity.salesorder.orderstatus", "en-US", "订单状态", "订单状态（1=启用，0=禁用）"),
            // entity.salesorder.orderstatus
            new TranslationSeedItem("entity.salesorder.orderstatus", "ja-JP", "订单状态", "订单状态（1=启用，0=禁用）"),
            // entity.salesorder.orderstatus
            new TranslationSeedItem("entity.salesorder.orderstatus", "zh-CN", "订单状态", "订单状态（1=启用，0=禁用）"),
            // entity.salesorder.orderstatus
            new TranslationSeedItem("entity.salesorder.orderstatus", "zh-HK", "订单状态", "订单状态（1=启用，0=禁用）"),

            // entity.salesorder.deliverystatus
            new TranslationSeedItem("entity.salesorder.deliverystatus", "en-US", "交货状态", "交货状态（0=未交货，1=部分交货，2=全部交货）"),
            // entity.salesorder.deliverystatus
            new TranslationSeedItem("entity.salesorder.deliverystatus", "ja-JP", "交货状态", "交货状态（0=未交货，1=部分交货，2=全部交货）"),
            // entity.salesorder.deliverystatus
            new TranslationSeedItem("entity.salesorder.deliverystatus", "zh-CN", "交货状态", "交货状态（0=未交货，1=部分交货，2=全部交货）"),
            // entity.salesorder.deliverystatus
            new TranslationSeedItem("entity.salesorder.deliverystatus", "zh-HK", "交货状态", "交货状态（0=未交货，1=部分交货，2=全部交货）"),

            // entity.salesorder.deliverymethod
            new TranslationSeedItem("entity.salesorder.deliverymethod", "en-US", "交货方式", "交货方式（0=自提，1=送货上门，2=物流配送，3=快递）"),
            // entity.salesorder.deliverymethod
            new TranslationSeedItem("entity.salesorder.deliverymethod", "ja-JP", "交货方式", "交货方式（0=自提，1=送货上门，2=物流配送，3=快递）"),
            // entity.salesorder.deliverymethod
            new TranslationSeedItem("entity.salesorder.deliverymethod", "zh-CN", "交货方式", "交货方式（0=自提，1=送货上门，2=物流配送，3=快递）"),
            // entity.salesorder.deliverymethod
            new TranslationSeedItem("entity.salesorder.deliverymethod", "zh-HK", "交货方式", "交货方式（0=自提，1=送货上门，2=物流配送，3=快递）"),

            // entity.salesorder.paymentmethod
            new TranslationSeedItem("entity.salesorder.paymentmethod", "en-US", "收款方式", "收款方式（0=现金，1=银行转账，2=支票，3=信用证，4=其他）"),
            // entity.salesorder.paymentmethod
            new TranslationSeedItem("entity.salesorder.paymentmethod", "ja-JP", "收款方式", "收款方式（0=现金，1=银行转账，2=支票，3=信用证，4=其他）"),
            // entity.salesorder.paymentmethod
            new TranslationSeedItem("entity.salesorder.paymentmethod", "zh-CN", "收款方式", "收款方式（0=现金，1=银行转账，2=支票，3=信用证，4=其他）"),
            // entity.salesorder.paymentmethod
            new TranslationSeedItem("entity.salesorder.paymentmethod", "zh-HK", "收款方式", "收款方式（0=现金，1=银行转账，2=支票，3=信用证，4=其他）"),

            // entity.salesorder.deliveryaddress
            new TranslationSeedItem("entity.salesorder.deliveryaddress", "en-US", "交货地址", "交货地址"),
            // entity.salesorder.deliveryaddress
            new TranslationSeedItem("entity.salesorder.deliveryaddress", "ja-JP", "交货地址", "交货地址"),
            // entity.salesorder.deliveryaddress
            new TranslationSeedItem("entity.salesorder.deliveryaddress", "zh-CN", "交货地址", "交货地址"),
            // entity.salesorder.deliveryaddress
            new TranslationSeedItem("entity.salesorder.deliveryaddress", "zh-HK", "交货地址", "交货地址"),

            // entity.salesorder.items
            new TranslationSeedItem("entity.salesorder.items", "en-US", "销售订单明细列表", "销售订单明细列表（主子表关系，一个订单可以有多个明细）"),
            // entity.salesorder.items
            new TranslationSeedItem("entity.salesorder.items", "ja-JP", "销售订单明细列表", "销售订单明细列表（主子表关系，一个订单可以有多个明细）"),
            // entity.salesorder.items
            new TranslationSeedItem("entity.salesorder.items", "zh-CN", "销售订单明细列表", "销售订单明细列表（主子表关系，一个订单可以有多个明细）"),
            // entity.salesorder.items
            new TranslationSeedItem("entity.salesorder.items", "zh-HK", "销售订单明细列表", "销售订单明细列表（主子表关系，一个订单可以有多个明细）"),

            // entity.salesorder.changelogs
            new TranslationSeedItem("entity.salesorder.changelogs", "en-US", "销售订单变更记录列表", "销售订单变更记录列表（外键在子表 TaktSalesOrderChangeLog.OrderId）"),
            // entity.salesorder.changelogs
            new TranslationSeedItem("entity.salesorder.changelogs", "ja-JP", "销售订单变更记录列表", "销售订单变更记录列表（外键在子表 TaktSalesOrderChangeLog.OrderId）"),
            // entity.salesorder.changelogs
            new TranslationSeedItem("entity.salesorder.changelogs", "zh-CN", "销售订单变更记录列表", "销售订单变更记录列表（外键在子表 TaktSalesOrderChangeLog.OrderId）"),
            // entity.salesorder.changelogs
            new TranslationSeedItem("entity.salesorder.changelogs", "zh-HK", "销售订单变更记录列表", "销售订单变更记录列表（外键在子表 TaktSalesOrderChangeLog.OrderId）"),
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
