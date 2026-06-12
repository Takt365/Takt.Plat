// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Sales
// 文件名称：TaktSalesOrderItemI18nSeedData.cs
// 创建时间：2026-06-12
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
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Sales;

/// <summary>
/// TaktSalesOrderItem 实体国际化翻译种子（键前缀 entity.salesorderitem.*）
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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 salesorderitem 实体翻译...", tenantCode);

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
    /// I18nKey：entity.salesorderitem._self / entity.salesorderitem.{{field}}；ResourceGroup=4；ResourceType=0
    /// </summary>
    private static List<TranslationSeedItem> GetSalesOrderItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.salesorderitem._self
            new TranslationSeedItem("entity.salesorderitem._self", "en-US", "Sales Order Item Information", "实体名称"),
            // entity.salesorderitem._self
            new TranslationSeedItem("entity.salesorderitem._self", "ja-JP", "Takt销售订单明细信息", "实体名称"),
            // entity.salesorderitem._self
            new TranslationSeedItem("entity.salesorderitem._self", "zh-CN", "Takt销售订单明细信息", "实体名称"),
            // entity.salesorderitem._self
            new TranslationSeedItem("entity.salesorderitem._self", "zh-HK", "Takt销售订单明细信息", "实体名称"),

            // entity.salesorderitem.salesorderid
            new TranslationSeedItem("entity.salesorderitem.salesorderid", "en-US", "销售订单ID", "销售订单ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.salesorderitem.salesorderid
            new TranslationSeedItem("entity.salesorderitem.salesorderid", "ja-JP", "销售订单ID", "销售订单ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.salesorderitem.salesorderid
            new TranslationSeedItem("entity.salesorderitem.salesorderid", "zh-CN", "销售订单ID", "销售订单ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.salesorderitem.salesorderid
            new TranslationSeedItem("entity.salesorderitem.salesorderid", "zh-HK", "销售订单ID", "销售订单ID（主子表关系，序列化为string以避免Javascript精度问题）"),

            // entity.salesorderitem.salesordercode
            new TranslationSeedItem("entity.salesorderitem.salesordercode", "en-US", "销售订单编码", "销售订单编码（冗余字段，便于查询）"),
            // entity.salesorderitem.salesordercode
            new TranslationSeedItem("entity.salesorderitem.salesordercode", "ja-JP", "销售订单编码", "销售订单编码（冗余字段，便于查询）"),
            // entity.salesorderitem.salesordercode
            new TranslationSeedItem("entity.salesorderitem.salesordercode", "zh-CN", "销售订单编码", "销售订单编码（冗余字段，便于查询）"),
            // entity.salesorderitem.salesordercode
            new TranslationSeedItem("entity.salesorderitem.salesordercode", "zh-HK", "销售订单编码", "销售订单编码（冗余字段，便于查询）"),

            // entity.salesorderitem.linenumber
            new TranslationSeedItem("entity.salesorderitem.linenumber", "en-US", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.salesorderitem.linenumber
            new TranslationSeedItem("entity.salesorderitem.linenumber", "ja-JP", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.salesorderitem.linenumber
            new TranslationSeedItem("entity.salesorderitem.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.salesorderitem.linenumber
            new TranslationSeedItem("entity.salesorderitem.linenumber", "zh-HK", "行号", "行号（项号/序号，固定步长=10）"),

            // entity.salesorderitem.materialcode
            new TranslationSeedItem("entity.salesorderitem.materialcode", "en-US", "物料编码", "物料编码"),
            // entity.salesorderitem.materialcode
            new TranslationSeedItem("entity.salesorderitem.materialcode", "ja-JP", "物料编码", "物料编码"),
            // entity.salesorderitem.materialcode
            new TranslationSeedItem("entity.salesorderitem.materialcode", "zh-CN", "物料编码", "物料编码"),
            // entity.salesorderitem.materialcode
            new TranslationSeedItem("entity.salesorderitem.materialcode", "zh-HK", "物料编码", "物料编码"),

            // entity.salesorderitem.materialname
            new TranslationSeedItem("entity.salesorderitem.materialname", "en-US", "物料名称", "物料名称"),
            // entity.salesorderitem.materialname
            new TranslationSeedItem("entity.salesorderitem.materialname", "ja-JP", "物料名称", "物料名称"),
            // entity.salesorderitem.materialname
            new TranslationSeedItem("entity.salesorderitem.materialname", "zh-CN", "物料名称", "物料名称"),
            // entity.salesorderitem.materialname
            new TranslationSeedItem("entity.salesorderitem.materialname", "zh-HK", "物料名称", "物料名称"),

            // entity.salesorderitem.materialspecification
            new TranslationSeedItem("entity.salesorderitem.materialspecification", "en-US", "物料规格", "物料规格"),
            // entity.salesorderitem.materialspecification
            new TranslationSeedItem("entity.salesorderitem.materialspecification", "ja-JP", "物料规格", "物料规格"),
            // entity.salesorderitem.materialspecification
            new TranslationSeedItem("entity.salesorderitem.materialspecification", "zh-CN", "物料规格", "物料规格"),
            // entity.salesorderitem.materialspecification
            new TranslationSeedItem("entity.salesorderitem.materialspecification", "zh-HK", "物料规格", "物料规格"),

            // entity.salesorderitem.salesunit
            new TranslationSeedItem("entity.salesorderitem.salesunit", "en-US", "销售单位", "销售单位"),
            // entity.salesorderitem.salesunit
            new TranslationSeedItem("entity.salesorderitem.salesunit", "ja-JP", "销售单位", "销售单位"),
            // entity.salesorderitem.salesunit
            new TranslationSeedItem("entity.salesorderitem.salesunit", "zh-CN", "销售单位", "销售单位"),
            // entity.salesorderitem.salesunit
            new TranslationSeedItem("entity.salesorderitem.salesunit", "zh-HK", "销售单位", "销售单位"),

            // entity.salesorderitem.orderquantity
            new TranslationSeedItem("entity.salesorderitem.orderquantity", "en-US", "订购数量", "订购数量（基本单位数量）"),
            // entity.salesorderitem.orderquantity
            new TranslationSeedItem("entity.salesorderitem.orderquantity", "ja-JP", "订购数量", "订购数量（基本单位数量）"),
            // entity.salesorderitem.orderquantity
            new TranslationSeedItem("entity.salesorderitem.orderquantity", "zh-CN", "订购数量", "订购数量（基本单位数量）"),
            // entity.salesorderitem.orderquantity
            new TranslationSeedItem("entity.salesorderitem.orderquantity", "zh-HK", "订购数量", "订购数量（基本单位数量）"),

            // entity.salesorderitem.shippedquantity
            new TranslationSeedItem("entity.salesorderitem.shippedquantity", "en-US", "已发货数量", "已发货数量（基本单位数量）"),
            // entity.salesorderitem.shippedquantity
            new TranslationSeedItem("entity.salesorderitem.shippedquantity", "ja-JP", "已发货数量", "已发货数量（基本单位数量）"),
            // entity.salesorderitem.shippedquantity
            new TranslationSeedItem("entity.salesorderitem.shippedquantity", "zh-CN", "已发货数量", "已发货数量（基本单位数量）"),
            // entity.salesorderitem.shippedquantity
            new TranslationSeedItem("entity.salesorderitem.shippedquantity", "zh-HK", "已发货数量", "已发货数量（基本单位数量）"),

            // entity.salesorderitem.unitprice
            new TranslationSeedItem("entity.salesorderitem.unitprice", "en-US", "单价", "单价（精确到分，存储为整数，单位为分）"),
            // entity.salesorderitem.unitprice
            new TranslationSeedItem("entity.salesorderitem.unitprice", "ja-JP", "单价", "单价（精确到分，存储为整数，单位为分）"),
            // entity.salesorderitem.unitprice
            new TranslationSeedItem("entity.salesorderitem.unitprice", "zh-CN", "单价", "单价（精确到分，存储为整数，单位为分）"),
            // entity.salesorderitem.unitprice
            new TranslationSeedItem("entity.salesorderitem.unitprice", "zh-HK", "单价", "单价（精确到分，存储为整数，单位为分）"),

            // entity.salesorderitem.discountrate
            new TranslationSeedItem("entity.salesorderitem.discountrate", "en-US", "折扣率", "折扣率（0-100，表示折扣百分比）"),
            // entity.salesorderitem.discountrate
            new TranslationSeedItem("entity.salesorderitem.discountrate", "ja-JP", "折扣率", "折扣率（0-100，表示折扣百分比）"),
            // entity.salesorderitem.discountrate
            new TranslationSeedItem("entity.salesorderitem.discountrate", "zh-CN", "折扣率", "折扣率（0-100，表示折扣百分比）"),
            // entity.salesorderitem.discountrate
            new TranslationSeedItem("entity.salesorderitem.discountrate", "zh-HK", "折扣率", "折扣率（0-100，表示折扣百分比）"),

            // entity.salesorderitem.discountamount
            new TranslationSeedItem("entity.salesorderitem.discountamount", "en-US", "折扣金额", "折扣金额（精确到分，存储为整数，单位为分）"),
            // entity.salesorderitem.discountamount
            new TranslationSeedItem("entity.salesorderitem.discountamount", "ja-JP", "折扣金额", "折扣金额（精确到分，存储为整数，单位为分）"),
            // entity.salesorderitem.discountamount
            new TranslationSeedItem("entity.salesorderitem.discountamount", "zh-CN", "折扣金额", "折扣金额（精确到分，存储为整数，单位为分）"),
            // entity.salesorderitem.discountamount
            new TranslationSeedItem("entity.salesorderitem.discountamount", "zh-HK", "折扣金额", "折扣金额（精确到分，存储为整数，单位为分）"),

            // entity.salesorderitem.taxrate
            new TranslationSeedItem("entity.salesorderitem.taxrate", "en-US", "税费率", "税费率（0-100，表示税费百分比）"),
            // entity.salesorderitem.taxrate
            new TranslationSeedItem("entity.salesorderitem.taxrate", "ja-JP", "税费率", "税费率（0-100，表示税费百分比）"),
            // entity.salesorderitem.taxrate
            new TranslationSeedItem("entity.salesorderitem.taxrate", "zh-CN", "税费率", "税费率（0-100，表示税费百分比）"),
            // entity.salesorderitem.taxrate
            new TranslationSeedItem("entity.salesorderitem.taxrate", "zh-HK", "税费率", "税费率（0-100，表示税费百分比）"),

            // entity.salesorderitem.taxamount
            new TranslationSeedItem("entity.salesorderitem.taxamount", "en-US", "税费", "税费（精确到分，存储为整数，单位为分）"),
            // entity.salesorderitem.taxamount
            new TranslationSeedItem("entity.salesorderitem.taxamount", "ja-JP", "税费", "税费（精确到分，存储为整数，单位为分）"),
            // entity.salesorderitem.taxamount
            new TranslationSeedItem("entity.salesorderitem.taxamount", "zh-CN", "税费", "税费（精确到分，存储为整数，单位为分）"),
            // entity.salesorderitem.taxamount
            new TranslationSeedItem("entity.salesorderitem.taxamount", "zh-HK", "税费", "税费（精确到分，存储为整数，单位为分）"),

            // entity.salesorderitem.subtotalamount
            new TranslationSeedItem("entity.salesorderitem.subtotalamount", "en-US", "小计金额", "小计金额（精确到分，存储为整数，单位为分）"),
            // entity.salesorderitem.subtotalamount
            new TranslationSeedItem("entity.salesorderitem.subtotalamount", "ja-JP", "小计金额", "小计金额（精确到分，存储为整数，单位为分）"),
            // entity.salesorderitem.subtotalamount
            new TranslationSeedItem("entity.salesorderitem.subtotalamount", "zh-CN", "小计金额", "小计金额（精确到分，存储为整数，单位为分）"),
            // entity.salesorderitem.subtotalamount
            new TranslationSeedItem("entity.salesorderitem.subtotalamount", "zh-HK", "小计金额", "小计金额（精确到分，存储为整数，单位为分）"),

            // entity.salesorderitem.deliverystatus
            new TranslationSeedItem("entity.salesorderitem.deliverystatus", "en-US", "行交货状态", "行交货状态（0=未交货，1=部分交货，2=全部交货）"),
            // entity.salesorderitem.deliverystatus
            new TranslationSeedItem("entity.salesorderitem.deliverystatus", "ja-JP", "行交货状态", "行交货状态（0=未交货，1=部分交货，2=全部交货）"),
            // entity.salesorderitem.deliverystatus
            new TranslationSeedItem("entity.salesorderitem.deliverystatus", "zh-CN", "行交货状态", "行交货状态（0=未交货，1=部分交货，2=全部交货）"),
            // entity.salesorderitem.deliverystatus
            new TranslationSeedItem("entity.salesorderitem.deliverystatus", "zh-HK", "行交货状态", "行交货状态（0=未交货，1=部分交货，2=全部交货）"),
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
