// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Sales
// 文件名称：TaktSalesOrderItemI18nSeedData.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSalesOrderItem 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktSalesOrderItem 实体国际化翻译种子（键前缀 entity.salesOrderItem.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSalesOrderItemI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSalesOrderItem 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 salesOrderItem 实体翻译...", tenantCode);

        foreach (var item in GetSalesOrderItemTranslations())
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

        TaktLogger.Information("TaktSalesOrderItem 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSalesOrderItem 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.salesOrderItem._self / entity.salesOrderItem.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSalesOrderItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.salesOrderItem._self
            new TranslationSeedItem("entity.salesOrderItem._self", "en-US", "Sales Order Item Information", "实体名称"),
            // entity.salesOrderItem._self
            new TranslationSeedItem("entity.salesOrderItem._self", "ja-JP", "Takt销售订单明细信息", "实体名称"),
            // entity.salesOrderItem._self
            new TranslationSeedItem("entity.salesOrderItem._self", "zh-CN", "Takt销售订单明细信息", "实体名称"),
            // entity.salesOrderItem._self
            new TranslationSeedItem("entity.salesOrderItem._self", "zh-HK", "Takt销售订单明细信息", "实体名称"),

            // entity.salesOrderItem.salesorderid
            new TranslationSeedItem("entity.salesOrderItem.salesorderid", "en-US", "销售订单ID", "销售订单ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.salesOrderItem.salesorderid
            new TranslationSeedItem("entity.salesOrderItem.salesorderid", "ja-JP", "销售订单ID", "销售订单ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.salesOrderItem.salesorderid
            new TranslationSeedItem("entity.salesOrderItem.salesorderid", "zh-CN", "销售订单ID", "销售订单ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.salesOrderItem.salesorderid
            new TranslationSeedItem("entity.salesOrderItem.salesorderid", "zh-HK", "销售订单ID", "销售订单ID（主子表关系，序列化为string以避免Javascript精度问题）"),

            // entity.salesOrderItem.salesordercode
            new TranslationSeedItem("entity.salesOrderItem.salesordercode", "en-US", "销售订单编码", "销售订单编码（冗余字段，便于查询）"),
            // entity.salesOrderItem.salesordercode
            new TranslationSeedItem("entity.salesOrderItem.salesordercode", "ja-JP", "销售订单编码", "销售订单编码（冗余字段，便于查询）"),
            // entity.salesOrderItem.salesordercode
            new TranslationSeedItem("entity.salesOrderItem.salesordercode", "zh-CN", "销售订单编码", "销售订单编码（冗余字段，便于查询）"),
            // entity.salesOrderItem.salesordercode
            new TranslationSeedItem("entity.salesOrderItem.salesordercode", "zh-HK", "销售订单编码", "销售订单编码（冗余字段，便于查询）"),

            // entity.salesOrderItem.linenumber
            new TranslationSeedItem("entity.salesOrderItem.linenumber", "en-US", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.salesOrderItem.linenumber
            new TranslationSeedItem("entity.salesOrderItem.linenumber", "ja-JP", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.salesOrderItem.linenumber
            new TranslationSeedItem("entity.salesOrderItem.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.salesOrderItem.linenumber
            new TranslationSeedItem("entity.salesOrderItem.linenumber", "zh-HK", "行号", "行号（项号/序号，固定步长=10）"),

            // entity.salesOrderItem.materialcode
            new TranslationSeedItem("entity.salesOrderItem.materialcode", "en-US", "物料编码", "物料编码"),
            // entity.salesOrderItem.materialcode
            new TranslationSeedItem("entity.salesOrderItem.materialcode", "ja-JP", "物料编码", "物料编码"),
            // entity.salesOrderItem.materialcode
            new TranslationSeedItem("entity.salesOrderItem.materialcode", "zh-CN", "物料编码", "物料编码"),
            // entity.salesOrderItem.materialcode
            new TranslationSeedItem("entity.salesOrderItem.materialcode", "zh-HK", "物料编码", "物料编码"),

            // entity.salesOrderItem.materialname
            new TranslationSeedItem("entity.salesOrderItem.materialname", "en-US", "物料名称", "物料名称"),
            // entity.salesOrderItem.materialname
            new TranslationSeedItem("entity.salesOrderItem.materialname", "ja-JP", "物料名称", "物料名称"),
            // entity.salesOrderItem.materialname
            new TranslationSeedItem("entity.salesOrderItem.materialname", "zh-CN", "物料名称", "物料名称"),
            // entity.salesOrderItem.materialname
            new TranslationSeedItem("entity.salesOrderItem.materialname", "zh-HK", "物料名称", "物料名称"),

            // entity.salesOrderItem.materialspecification
            new TranslationSeedItem("entity.salesOrderItem.materialspecification", "en-US", "物料规格", "物料规格"),
            // entity.salesOrderItem.materialspecification
            new TranslationSeedItem("entity.salesOrderItem.materialspecification", "ja-JP", "物料规格", "物料规格"),
            // entity.salesOrderItem.materialspecification
            new TranslationSeedItem("entity.salesOrderItem.materialspecification", "zh-CN", "物料规格", "物料规格"),
            // entity.salesOrderItem.materialspecification
            new TranslationSeedItem("entity.salesOrderItem.materialspecification", "zh-HK", "物料规格", "物料规格"),

            // entity.salesOrderItem.salesunit
            new TranslationSeedItem("entity.salesOrderItem.salesunit", "en-US", "销售单位", "销售单位"),
            // entity.salesOrderItem.salesunit
            new TranslationSeedItem("entity.salesOrderItem.salesunit", "ja-JP", "销售单位", "销售单位"),
            // entity.salesOrderItem.salesunit
            new TranslationSeedItem("entity.salesOrderItem.salesunit", "zh-CN", "销售单位", "销售单位"),
            // entity.salesOrderItem.salesunit
            new TranslationSeedItem("entity.salesOrderItem.salesunit", "zh-HK", "销售单位", "销售单位"),

            // entity.salesOrderItem.orderquantity
            new TranslationSeedItem("entity.salesOrderItem.orderquantity", "en-US", "订购数量", "订购数量（基本单位数量）"),
            // entity.salesOrderItem.orderquantity
            new TranslationSeedItem("entity.salesOrderItem.orderquantity", "ja-JP", "订购数量", "订购数量（基本单位数量）"),
            // entity.salesOrderItem.orderquantity
            new TranslationSeedItem("entity.salesOrderItem.orderquantity", "zh-CN", "订购数量", "订购数量（基本单位数量）"),
            // entity.salesOrderItem.orderquantity
            new TranslationSeedItem("entity.salesOrderItem.orderquantity", "zh-HK", "订购数量", "订购数量（基本单位数量）"),

            // entity.salesOrderItem.shippedquantity
            new TranslationSeedItem("entity.salesOrderItem.shippedquantity", "en-US", "已发货数量", "已发货数量（基本单位数量）"),
            // entity.salesOrderItem.shippedquantity
            new TranslationSeedItem("entity.salesOrderItem.shippedquantity", "ja-JP", "已发货数量", "已发货数量（基本单位数量）"),
            // entity.salesOrderItem.shippedquantity
            new TranslationSeedItem("entity.salesOrderItem.shippedquantity", "zh-CN", "已发货数量", "已发货数量（基本单位数量）"),
            // entity.salesOrderItem.shippedquantity
            new TranslationSeedItem("entity.salesOrderItem.shippedquantity", "zh-HK", "已发货数量", "已发货数量（基本单位数量）"),

            // entity.salesOrderItem.unitprice
            new TranslationSeedItem("entity.salesOrderItem.unitprice", "en-US", "单价", "单价（精确到分，存储为整数，单位为分）"),
            // entity.salesOrderItem.unitprice
            new TranslationSeedItem("entity.salesOrderItem.unitprice", "ja-JP", "单价", "单价（精确到分，存储为整数，单位为分）"),
            // entity.salesOrderItem.unitprice
            new TranslationSeedItem("entity.salesOrderItem.unitprice", "zh-CN", "单价", "单价（精确到分，存储为整数，单位为分）"),
            // entity.salesOrderItem.unitprice
            new TranslationSeedItem("entity.salesOrderItem.unitprice", "zh-HK", "单价", "单价（精确到分，存储为整数，单位为分）"),

            // entity.salesOrderItem.discountrate
            new TranslationSeedItem("entity.salesOrderItem.discountrate", "en-US", "折扣率", "折扣率（0-100，表示折扣百分比）"),
            // entity.salesOrderItem.discountrate
            new TranslationSeedItem("entity.salesOrderItem.discountrate", "ja-JP", "折扣率", "折扣率（0-100，表示折扣百分比）"),
            // entity.salesOrderItem.discountrate
            new TranslationSeedItem("entity.salesOrderItem.discountrate", "zh-CN", "折扣率", "折扣率（0-100，表示折扣百分比）"),
            // entity.salesOrderItem.discountrate
            new TranslationSeedItem("entity.salesOrderItem.discountrate", "zh-HK", "折扣率", "折扣率（0-100，表示折扣百分比）"),

            // entity.salesOrderItem.discountamount
            new TranslationSeedItem("entity.salesOrderItem.discountamount", "en-US", "折扣金额", "折扣金额（精确到分，存储为整数，单位为分）"),
            // entity.salesOrderItem.discountamount
            new TranslationSeedItem("entity.salesOrderItem.discountamount", "ja-JP", "折扣金额", "折扣金额（精确到分，存储为整数，单位为分）"),
            // entity.salesOrderItem.discountamount
            new TranslationSeedItem("entity.salesOrderItem.discountamount", "zh-CN", "折扣金额", "折扣金额（精确到分，存储为整数，单位为分）"),
            // entity.salesOrderItem.discountamount
            new TranslationSeedItem("entity.salesOrderItem.discountamount", "zh-HK", "折扣金额", "折扣金额（精确到分，存储为整数，单位为分）"),

            // entity.salesOrderItem.taxrate
            new TranslationSeedItem("entity.salesOrderItem.taxrate", "en-US", "税费率", "税费率（0-100，表示税费百分比）"),
            // entity.salesOrderItem.taxrate
            new TranslationSeedItem("entity.salesOrderItem.taxrate", "ja-JP", "税费率", "税费率（0-100，表示税费百分比）"),
            // entity.salesOrderItem.taxrate
            new TranslationSeedItem("entity.salesOrderItem.taxrate", "zh-CN", "税费率", "税费率（0-100，表示税费百分比）"),
            // entity.salesOrderItem.taxrate
            new TranslationSeedItem("entity.salesOrderItem.taxrate", "zh-HK", "税费率", "税费率（0-100，表示税费百分比）"),

            // entity.salesOrderItem.taxamount
            new TranslationSeedItem("entity.salesOrderItem.taxamount", "en-US", "税费", "税费（精确到分，存储为整数，单位为分）"),
            // entity.salesOrderItem.taxamount
            new TranslationSeedItem("entity.salesOrderItem.taxamount", "ja-JP", "税费", "税费（精确到分，存储为整数，单位为分）"),
            // entity.salesOrderItem.taxamount
            new TranslationSeedItem("entity.salesOrderItem.taxamount", "zh-CN", "税费", "税费（精确到分，存储为整数，单位为分）"),
            // entity.salesOrderItem.taxamount
            new TranslationSeedItem("entity.salesOrderItem.taxamount", "zh-HK", "税费", "税费（精确到分，存储为整数，单位为分）"),

            // entity.salesOrderItem.subtotalamount
            new TranslationSeedItem("entity.salesOrderItem.subtotalamount", "en-US", "小计金额", "小计金额（精确到分，存储为整数，单位为分）"),
            // entity.salesOrderItem.subtotalamount
            new TranslationSeedItem("entity.salesOrderItem.subtotalamount", "ja-JP", "小计金额", "小计金额（精确到分，存储为整数，单位为分）"),
            // entity.salesOrderItem.subtotalamount
            new TranslationSeedItem("entity.salesOrderItem.subtotalamount", "zh-CN", "小计金额", "小计金额（精确到分，存储为整数，单位为分）"),
            // entity.salesOrderItem.subtotalamount
            new TranslationSeedItem("entity.salesOrderItem.subtotalamount", "zh-HK", "小计金额", "小计金额（精确到分，存储为整数，单位为分）"),

            // entity.salesOrderItem.deliverystatus
            new TranslationSeedItem("entity.salesOrderItem.deliverystatus", "en-US", "行交货状态", "行交货状态（0=未交货，1=部分交货，2=全部交货）"),
            // entity.salesOrderItem.deliverystatus
            new TranslationSeedItem("entity.salesOrderItem.deliverystatus", "ja-JP", "行交货状态", "行交货状态（0=未交货，1=部分交货，2=全部交货）"),
            // entity.salesOrderItem.deliverystatus
            new TranslationSeedItem("entity.salesOrderItem.deliverystatus", "zh-CN", "行交货状态", "行交货状态（0=未交货，1=部分交货，2=全部交货）"),
            // entity.salesOrderItem.deliverystatus
            new TranslationSeedItem("entity.salesOrderItem.deliverystatus", "zh-HK", "行交货状态", "行交货状态（0=未交货，1=部分交货，2=全部交货）"),
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
