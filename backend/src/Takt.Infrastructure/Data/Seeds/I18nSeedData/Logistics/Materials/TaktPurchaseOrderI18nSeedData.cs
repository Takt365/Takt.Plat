// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Materials
// 文件名称：TaktPurchaseOrderI18nSeedData.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktPurchaseOrder 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Materials;

/// <summary>
/// TaktPurchaseOrder 实体国际化翻译种子（键前缀 entity.purchaseOrder.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktPurchaseOrderI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktPurchaseOrder 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 purchaseOrder 实体翻译...", tenantCode);

        foreach (var item in GetPurchaseOrderTranslations())
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

        TaktLogger.Information("TaktPurchaseOrder 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktPurchaseOrder 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.purchaseOrder._self / entity.purchaseOrder.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetPurchaseOrderTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.purchaseOrder._self
            new TranslationSeedItem("entity.purchaseOrder._self", "en-US", "Purchase Order Information", "实体名称"),
            // entity.purchaseOrder._self
            new TranslationSeedItem("entity.purchaseOrder._self", "ja-JP", "Takt采购订单信息", "实体名称"),
            // entity.purchaseOrder._self
            new TranslationSeedItem("entity.purchaseOrder._self", "zh-CN", "Takt采购订单信息", "实体名称"),
            // entity.purchaseOrder._self
            new TranslationSeedItem("entity.purchaseOrder._self", "zh-HK", "Takt采购订单信息", "实体名称"),

            // entity.purchaseOrder.plantcode
            new TranslationSeedItem("entity.purchaseOrder.plantcode", "en-US", "工厂代码", "工厂代码（不可空）"),
            // entity.purchaseOrder.plantcode
            new TranslationSeedItem("entity.purchaseOrder.plantcode", "ja-JP", "工厂代码", "工厂代码（不可空）"),
            // entity.purchaseOrder.plantcode
            new TranslationSeedItem("entity.purchaseOrder.plantcode", "zh-CN", "工厂代码", "工厂代码（不可空）"),
            // entity.purchaseOrder.plantcode
            new TranslationSeedItem("entity.purchaseOrder.plantcode", "zh-HK", "工厂代码", "工厂代码（不可空）"),

            // entity.purchaseOrder.code
            new TranslationSeedItem("entity.purchaseOrder.code", "en-US", "采购订单编码", "采购订单编码（唯一索引）"),
            // entity.purchaseOrder.code
            new TranslationSeedItem("entity.purchaseOrder.code", "ja-JP", "采购订单编码", "采购订单编码（唯一索引）"),
            // entity.purchaseOrder.code
            new TranslationSeedItem("entity.purchaseOrder.code", "zh-CN", "采购订单编码", "采购订单编码（唯一索引）"),
            // entity.purchaseOrder.code
            new TranslationSeedItem("entity.purchaseOrder.code", "zh-HK", "采购订单编码", "采购订单编码（唯一索引）"),

            // entity.purchaseOrder.suppliercode
            new TranslationSeedItem("entity.purchaseOrder.suppliercode", "en-US", "供应商编码", "供应商编码"),
            // entity.purchaseOrder.suppliercode
            new TranslationSeedItem("entity.purchaseOrder.suppliercode", "ja-JP", "供应商编码", "供应商编码"),
            // entity.purchaseOrder.suppliercode
            new TranslationSeedItem("entity.purchaseOrder.suppliercode", "zh-CN", "供应商编码", "供应商编码"),
            // entity.purchaseOrder.suppliercode
            new TranslationSeedItem("entity.purchaseOrder.suppliercode", "zh-HK", "供应商编码", "供应商编码"),

            // entity.purchaseOrder.suppliername
            new TranslationSeedItem("entity.purchaseOrder.suppliername", "en-US", "供应商名称", "供应商名称"),
            // entity.purchaseOrder.suppliername
            new TranslationSeedItem("entity.purchaseOrder.suppliername", "ja-JP", "供应商名称", "供应商名称"),
            // entity.purchaseOrder.suppliername
            new TranslationSeedItem("entity.purchaseOrder.suppliername", "zh-CN", "供应商名称", "供应商名称"),
            // entity.purchaseOrder.suppliername
            new TranslationSeedItem("entity.purchaseOrder.suppliername", "zh-HK", "供应商名称", "供应商名称"),

            // entity.purchaseOrder.orderdate
            new TranslationSeedItem("entity.purchaseOrder.orderdate", "en-US", "订单日期", "订单日期"),
            // entity.purchaseOrder.orderdate
            new TranslationSeedItem("entity.purchaseOrder.orderdate", "ja-JP", "订单日期", "订单日期"),
            // entity.purchaseOrder.orderdate
            new TranslationSeedItem("entity.purchaseOrder.orderdate", "zh-CN", "订单日期", "订单日期"),
            // entity.purchaseOrder.orderdate
            new TranslationSeedItem("entity.purchaseOrder.orderdate", "zh-HK", "订单日期", "订单日期"),

            // entity.purchaseOrder.requiredarrivaldate
            new TranslationSeedItem("entity.purchaseOrder.requiredarrivaldate", "en-US", "要求到货日期", "要求到货日期"),
            // entity.purchaseOrder.requiredarrivaldate
            new TranslationSeedItem("entity.purchaseOrder.requiredarrivaldate", "ja-JP", "要求到货日期", "要求到货日期"),
            // entity.purchaseOrder.requiredarrivaldate
            new TranslationSeedItem("entity.purchaseOrder.requiredarrivaldate", "zh-CN", "要求到货日期", "要求到货日期"),
            // entity.purchaseOrder.requiredarrivaldate
            new TranslationSeedItem("entity.purchaseOrder.requiredarrivaldate", "zh-HK", "要求到货日期", "要求到货日期"),

            // entity.purchaseOrder.actualarrivaldate
            new TranslationSeedItem("entity.purchaseOrder.actualarrivaldate", "en-US", "实际到货日期", "实际到货日期"),
            // entity.purchaseOrder.actualarrivaldate
            new TranslationSeedItem("entity.purchaseOrder.actualarrivaldate", "ja-JP", "实际到货日期", "实际到货日期"),
            // entity.purchaseOrder.actualarrivaldate
            new TranslationSeedItem("entity.purchaseOrder.actualarrivaldate", "zh-CN", "实际到货日期", "实际到货日期"),
            // entity.purchaseOrder.actualarrivaldate
            new TranslationSeedItem("entity.purchaseOrder.actualarrivaldate", "zh-HK", "实际到货日期", "实际到货日期"),

            // entity.purchaseOrder.purchasegroup
            new TranslationSeedItem("entity.purchaseOrder.purchasegroup", "en-US", "采购组代码", "采购组代码"),
            // entity.purchaseOrder.purchasegroup
            new TranslationSeedItem("entity.purchaseOrder.purchasegroup", "ja-JP", "采购组代码", "采购组代码"),
            // entity.purchaseOrder.purchasegroup
            new TranslationSeedItem("entity.purchaseOrder.purchasegroup", "zh-CN", "采购组代码", "采购组代码"),
            // entity.purchaseOrder.purchasegroup
            new TranslationSeedItem("entity.purchaseOrder.purchasegroup", "zh-HK", "采购组代码", "采购组代码"),

            // entity.purchaseOrder.totalquantity
            new TranslationSeedItem("entity.purchaseOrder.totalquantity", "en-US", "订单总数量", "订单总数量（基本单位数量）"),
            // entity.purchaseOrder.totalquantity
            new TranslationSeedItem("entity.purchaseOrder.totalquantity", "ja-JP", "订单总数量", "订单总数量（基本单位数量）"),
            // entity.purchaseOrder.totalquantity
            new TranslationSeedItem("entity.purchaseOrder.totalquantity", "zh-CN", "订单总数量", "订单总数量（基本单位数量）"),
            // entity.purchaseOrder.totalquantity
            new TranslationSeedItem("entity.purchaseOrder.totalquantity", "zh-HK", "订单总数量", "订单总数量（基本单位数量）"),

            // entity.purchaseOrder.totalamount
            new TranslationSeedItem("entity.purchaseOrder.totalamount", "en-US", "订单总金额", "订单总金额（精确到分，存储为整数，单位为分）"),
            // entity.purchaseOrder.totalamount
            new TranslationSeedItem("entity.purchaseOrder.totalamount", "ja-JP", "订单总金额", "订单总金额（精确到分，存储为整数，单位为分）"),
            // entity.purchaseOrder.totalamount
            new TranslationSeedItem("entity.purchaseOrder.totalamount", "zh-CN", "订单总金额", "订单总金额（精确到分，存储为整数，单位为分）"),
            // entity.purchaseOrder.totalamount
            new TranslationSeedItem("entity.purchaseOrder.totalamount", "zh-HK", "订单总金额", "订单总金额（精确到分，存储为整数，单位为分）"),

            // entity.purchaseOrder.discountamount
            new TranslationSeedItem("entity.purchaseOrder.discountamount", "en-US", "折扣金额", "折扣金额（精确到分，存储为整数，单位为分）"),
            // entity.purchaseOrder.discountamount
            new TranslationSeedItem("entity.purchaseOrder.discountamount", "ja-JP", "折扣金额", "折扣金额（精确到分，存储为整数，单位为分）"),
            // entity.purchaseOrder.discountamount
            new TranslationSeedItem("entity.purchaseOrder.discountamount", "zh-CN", "折扣金额", "折扣金额（精确到分，存储为整数，单位为分）"),
            // entity.purchaseOrder.discountamount
            new TranslationSeedItem("entity.purchaseOrder.discountamount", "zh-HK", "折扣金额", "折扣金额（精确到分，存储为整数，单位为分）"),

            // entity.purchaseOrder.taxamount
            new TranslationSeedItem("entity.purchaseOrder.taxamount", "en-US", "税费", "税费（精确到分，存储为整数，单位为分）"),
            // entity.purchaseOrder.taxamount
            new TranslationSeedItem("entity.purchaseOrder.taxamount", "ja-JP", "税费", "税费（精确到分，存储为整数，单位为分）"),
            // entity.purchaseOrder.taxamount
            new TranslationSeedItem("entity.purchaseOrder.taxamount", "zh-CN", "税费", "税费（精确到分，存储为整数，单位为分）"),
            // entity.purchaseOrder.taxamount
            new TranslationSeedItem("entity.purchaseOrder.taxamount", "zh-HK", "税费", "税费（精确到分，存储为整数，单位为分）"),

            // entity.purchaseOrder.actualamount
            new TranslationSeedItem("entity.purchaseOrder.actualamount", "en-US", "订单实付金额", "订单实付金额（精确到分，存储为整数，单位为分）"),
            // entity.purchaseOrder.actualamount
            new TranslationSeedItem("entity.purchaseOrder.actualamount", "ja-JP", "订单实付金额", "订单实付金额（精确到分，存储为整数，单位为分）"),
            // entity.purchaseOrder.actualamount
            new TranslationSeedItem("entity.purchaseOrder.actualamount", "zh-CN", "订单实付金额", "订单实付金额（精确到分，存储为整数，单位为分）"),
            // entity.purchaseOrder.actualamount
            new TranslationSeedItem("entity.purchaseOrder.actualamount", "zh-HK", "订单实付金额", "订单实付金额（精确到分，存储为整数，单位为分）"),

            // entity.purchaseOrder.receivedquantity
            new TranslationSeedItem("entity.purchaseOrder.receivedquantity", "en-US", "已入库数量", "已入库数量（基本单位数量）"),
            // entity.purchaseOrder.receivedquantity
            new TranslationSeedItem("entity.purchaseOrder.receivedquantity", "ja-JP", "已入库数量", "已入库数量（基本单位数量）"),
            // entity.purchaseOrder.receivedquantity
            new TranslationSeedItem("entity.purchaseOrder.receivedquantity", "zh-CN", "已入库数量", "已入库数量（基本单位数量）"),
            // entity.purchaseOrder.receivedquantity
            new TranslationSeedItem("entity.purchaseOrder.receivedquantity", "zh-HK", "已入库数量", "已入库数量（基本单位数量）"),

            // entity.purchaseOrder.receivedamount
            new TranslationSeedItem("entity.purchaseOrder.receivedamount", "en-US", "已入库金额", "已入库金额（精确到分，存储为整数，单位为分）"),
            // entity.purchaseOrder.receivedamount
            new TranslationSeedItem("entity.purchaseOrder.receivedamount", "ja-JP", "已入库金额", "已入库金额（精确到分，存储为整数，单位为分）"),
            // entity.purchaseOrder.receivedamount
            new TranslationSeedItem("entity.purchaseOrder.receivedamount", "zh-CN", "已入库金额", "已入库金额（精确到分，存储为整数，单位为分）"),
            // entity.purchaseOrder.receivedamount
            new TranslationSeedItem("entity.purchaseOrder.receivedamount", "zh-HK", "已入库金额", "已入库金额（精确到分，存储为整数，单位为分）"),

            // entity.purchaseOrder.paidamount
            new TranslationSeedItem("entity.purchaseOrder.paidamount", "en-US", "已付款金额", "已付款金额（精确到分，存储为整数，单位为分）"),
            // entity.purchaseOrder.paidamount
            new TranslationSeedItem("entity.purchaseOrder.paidamount", "ja-JP", "已付款金额", "已付款金额（精确到分，存储为整数，单位为分）"),
            // entity.purchaseOrder.paidamount
            new TranslationSeedItem("entity.purchaseOrder.paidamount", "zh-CN", "已付款金额", "已付款金额（精确到分，存储为整数，单位为分）"),
            // entity.purchaseOrder.paidamount
            new TranslationSeedItem("entity.purchaseOrder.paidamount", "zh-HK", "已付款金额", "已付款金额（精确到分，存储为整数，单位为分）"),

            // entity.purchaseOrder.orderstatus
            new TranslationSeedItem("entity.purchaseOrder.orderstatus", "en-US", "订单状态", "订单状态（1=启用，0=禁用）"),
            // entity.purchaseOrder.orderstatus
            new TranslationSeedItem("entity.purchaseOrder.orderstatus", "ja-JP", "订单状态", "订单状态（1=启用，0=禁用）"),
            // entity.purchaseOrder.orderstatus
            new TranslationSeedItem("entity.purchaseOrder.orderstatus", "zh-CN", "订单状态", "订单状态（1=启用，0=禁用）"),
            // entity.purchaseOrder.orderstatus
            new TranslationSeedItem("entity.purchaseOrder.orderstatus", "zh-HK", "订单状态", "订单状态（1=启用，0=禁用）"),

            // entity.purchaseOrder.deliverystatus
            new TranslationSeedItem("entity.purchaseOrder.deliverystatus", "en-US", "交货状态", "交货状态（0=未交货，1=部分交货，2=全部交货）"),
            // entity.purchaseOrder.deliverystatus
            new TranslationSeedItem("entity.purchaseOrder.deliverystatus", "ja-JP", "交货状态", "交货状态（0=未交货，1=部分交货，2=全部交货）"),
            // entity.purchaseOrder.deliverystatus
            new TranslationSeedItem("entity.purchaseOrder.deliverystatus", "zh-CN", "交货状态", "交货状态（0=未交货，1=部分交货，2=全部交货）"),
            // entity.purchaseOrder.deliverystatus
            new TranslationSeedItem("entity.purchaseOrder.deliverystatus", "zh-HK", "交货状态", "交货状态（0=未交货，1=部分交货，2=全部交货）"),

            // entity.purchaseOrder.paymentmethod
            new TranslationSeedItem("entity.purchaseOrder.paymentmethod", "en-US", "支付方式", "支付方式（0=现金，1=银行转账，2=支票，3=信用证，4=其他）"),
            // entity.purchaseOrder.paymentmethod
            new TranslationSeedItem("entity.purchaseOrder.paymentmethod", "ja-JP", "支付方式", "支付方式（0=现金，1=银行转账，2=支票，3=信用证，4=其他）"),
            // entity.purchaseOrder.paymentmethod
            new TranslationSeedItem("entity.purchaseOrder.paymentmethod", "zh-CN", "支付方式", "支付方式（0=现金，1=银行转账，2=支票，3=信用证，4=其他）"),
            // entity.purchaseOrder.paymentmethod
            new TranslationSeedItem("entity.purchaseOrder.paymentmethod", "zh-HK", "支付方式", "支付方式（0=现金，1=银行转账，2=支票，3=信用证，4=其他）"),

            // entity.purchaseOrder.deliverymethod
            new TranslationSeedItem("entity.purchaseOrder.deliverymethod", "en-US", "交货方式", "交货方式（0=自提，1=供应商送货，2=物流配送，3=快递）"),
            // entity.purchaseOrder.deliverymethod
            new TranslationSeedItem("entity.purchaseOrder.deliverymethod", "ja-JP", "交货方式", "交货方式（0=自提，1=供应商送货，2=物流配送，3=快递）"),
            // entity.purchaseOrder.deliverymethod
            new TranslationSeedItem("entity.purchaseOrder.deliverymethod", "zh-CN", "交货方式", "交货方式（0=自提，1=供应商送货，2=物流配送，3=快递）"),
            // entity.purchaseOrder.deliverymethod
            new TranslationSeedItem("entity.purchaseOrder.deliverymethod", "zh-HK", "交货方式", "交货方式（0=自提，1=供应商送货，2=物流配送，3=快递）"),

            // entity.purchaseOrder.deliveryaddress
            new TranslationSeedItem("entity.purchaseOrder.deliveryaddress", "en-US", "交货地址", "交货地址"),
            // entity.purchaseOrder.deliveryaddress
            new TranslationSeedItem("entity.purchaseOrder.deliveryaddress", "ja-JP", "交货地址", "交货地址"),
            // entity.purchaseOrder.deliveryaddress
            new TranslationSeedItem("entity.purchaseOrder.deliveryaddress", "zh-CN", "交货地址", "交货地址"),
            // entity.purchaseOrder.deliveryaddress
            new TranslationSeedItem("entity.purchaseOrder.deliveryaddress", "zh-HK", "交货地址", "交货地址"),

            // entity.purchaseOrder.items
            new TranslationSeedItem("entity.purchaseOrder.items", "en-US", "订单明细列表", "订单明细列表（主子表关系，一个订单可以有多个明细）"),
            // entity.purchaseOrder.items
            new TranslationSeedItem("entity.purchaseOrder.items", "ja-JP", "订单明细列表", "订单明细列表（主子表关系，一个订单可以有多个明细）"),
            // entity.purchaseOrder.items
            new TranslationSeedItem("entity.purchaseOrder.items", "zh-CN", "订单明细列表", "订单明细列表（主子表关系，一个订单可以有多个明细）"),
            // entity.purchaseOrder.items
            new TranslationSeedItem("entity.purchaseOrder.items", "zh-HK", "订单明细列表", "订单明细列表（主子表关系，一个订单可以有多个明细）"),

            // entity.purchaseOrder.changelogs
            new TranslationSeedItem("entity.purchaseOrder.changelogs", "en-US", "采购订单变更记录列表", "采购订单变更记录列表（外键在子表 <see cref=\"TaktPurchaseOrderChangeLog.OrderId\"/>）"),
            // entity.purchaseOrder.changelogs
            new TranslationSeedItem("entity.purchaseOrder.changelogs", "ja-JP", "采购订单变更记录列表", "采购订单变更记录列表（外键在子表 <see cref=\"TaktPurchaseOrderChangeLog.OrderId\"/>）"),
            // entity.purchaseOrder.changelogs
            new TranslationSeedItem("entity.purchaseOrder.changelogs", "zh-CN", "采购订单变更记录列表", "采购订单变更记录列表（外键在子表 <see cref=\"TaktPurchaseOrderChangeLog.OrderId\"/>）"),
            // entity.purchaseOrder.changelogs
            new TranslationSeedItem("entity.purchaseOrder.changelogs", "zh-HK", "采购订单变更记录列表", "采购订单变更记录列表（外键在子表 <see cref=\"TaktPurchaseOrderChangeLog.OrderId\"/>）"),
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
