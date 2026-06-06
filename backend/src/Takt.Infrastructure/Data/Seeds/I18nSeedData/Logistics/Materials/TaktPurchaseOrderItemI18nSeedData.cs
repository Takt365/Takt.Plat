// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Materials
// 文件名称：TaktPurchaseOrderItemI18nSeedData.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktPurchaseOrderItem 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktPurchaseOrderItem 实体国际化翻译种子（键前缀 entity.purchaseOrderItem.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktPurchaseOrderItemI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktPurchaseOrderItem 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 purchaseOrderItem 实体翻译...", tenantCode);

        foreach (var item in GetPurchaseOrderItemTranslations())
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

        TaktLogger.Information("TaktPurchaseOrderItem 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktPurchaseOrderItem 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.purchaseOrderItem._self / entity.purchaseOrderItem.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetPurchaseOrderItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.purchaseOrderItem._self
            new TranslationSeedItem("entity.purchaseOrderItem._self", "en-US", "Purchase Order Item Information", "实体名称"),
            // entity.purchaseOrderItem._self
            new TranslationSeedItem("entity.purchaseOrderItem._self", "ja-JP", "Takt采购订单明细信息", "实体名称"),
            // entity.purchaseOrderItem._self
            new TranslationSeedItem("entity.purchaseOrderItem._self", "zh-CN", "Takt采购订单明细信息", "实体名称"),
            // entity.purchaseOrderItem._self
            new TranslationSeedItem("entity.purchaseOrderItem._self", "zh-HK", "Takt采购订单明细信息", "实体名称"),

            // entity.purchaseOrderItem.purchaseorderid
            new TranslationSeedItem("entity.purchaseOrderItem.purchaseorderid", "en-US", "采购订单ID", "采购订单ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.purchaseOrderItem.purchaseorderid
            new TranslationSeedItem("entity.purchaseOrderItem.purchaseorderid", "ja-JP", "采购订单ID", "采购订单ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.purchaseOrderItem.purchaseorderid
            new TranslationSeedItem("entity.purchaseOrderItem.purchaseorderid", "zh-CN", "采购订单ID", "采购订单ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.purchaseOrderItem.purchaseorderid
            new TranslationSeedItem("entity.purchaseOrderItem.purchaseorderid", "zh-HK", "采购订单ID", "采购订单ID（主子表关系，序列化为string以避免Javascript精度问题）"),

            // entity.purchaseOrderItem.purchaseordercode
            new TranslationSeedItem("entity.purchaseOrderItem.purchaseordercode", "en-US", "采购订单编码", "采购订单编码（冗余字段，便于查询）"),
            // entity.purchaseOrderItem.purchaseordercode
            new TranslationSeedItem("entity.purchaseOrderItem.purchaseordercode", "ja-JP", "采购订单编码", "采购订单编码（冗余字段，便于查询）"),
            // entity.purchaseOrderItem.purchaseordercode
            new TranslationSeedItem("entity.purchaseOrderItem.purchaseordercode", "zh-CN", "采购订单编码", "采购订单编码（冗余字段，便于查询）"),
            // entity.purchaseOrderItem.purchaseordercode
            new TranslationSeedItem("entity.purchaseOrderItem.purchaseordercode", "zh-HK", "采购订单编码", "采购订单编码（冗余字段，便于查询）"),

            // entity.purchaseOrderItem.linenumber
            new TranslationSeedItem("entity.purchaseOrderItem.linenumber", "en-US", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.purchaseOrderItem.linenumber
            new TranslationSeedItem("entity.purchaseOrderItem.linenumber", "ja-JP", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.purchaseOrderItem.linenumber
            new TranslationSeedItem("entity.purchaseOrderItem.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.purchaseOrderItem.linenumber
            new TranslationSeedItem("entity.purchaseOrderItem.linenumber", "zh-HK", "行号", "行号（项号/序号，固定步长=10）"),

            // entity.purchaseOrderItem.requestcode
            new TranslationSeedItem("entity.purchaseOrderItem.requestcode", "en-US", "来源请购编码", "来源请购编码"),
            // entity.purchaseOrderItem.requestcode
            new TranslationSeedItem("entity.purchaseOrderItem.requestcode", "ja-JP", "来源请购编码", "来源请购编码"),
            // entity.purchaseOrderItem.requestcode
            new TranslationSeedItem("entity.purchaseOrderItem.requestcode", "zh-CN", "来源请购编码", "来源请购编码"),
            // entity.purchaseOrderItem.requestcode
            new TranslationSeedItem("entity.purchaseOrderItem.requestcode", "zh-HK", "来源请购编码", "来源请购编码"),

            // entity.purchaseOrderItem.requestlinenumber
            new TranslationSeedItem("entity.purchaseOrderItem.requestlinenumber", "en-US", "来源请购行号", "来源请购行号"),
            // entity.purchaseOrderItem.requestlinenumber
            new TranslationSeedItem("entity.purchaseOrderItem.requestlinenumber", "ja-JP", "来源请购行号", "来源请购行号"),
            // entity.purchaseOrderItem.requestlinenumber
            new TranslationSeedItem("entity.purchaseOrderItem.requestlinenumber", "zh-CN", "来源请购行号", "来源请购行号"),
            // entity.purchaseOrderItem.requestlinenumber
            new TranslationSeedItem("entity.purchaseOrderItem.requestlinenumber", "zh-HK", "来源请购行号", "来源请购行号"),

            // entity.purchaseOrderItem.materialcode
            new TranslationSeedItem("entity.purchaseOrderItem.materialcode", "en-US", "物料编码", "物料编码"),
            // entity.purchaseOrderItem.materialcode
            new TranslationSeedItem("entity.purchaseOrderItem.materialcode", "ja-JP", "物料编码", "物料编码"),
            // entity.purchaseOrderItem.materialcode
            new TranslationSeedItem("entity.purchaseOrderItem.materialcode", "zh-CN", "物料编码", "物料编码"),
            // entity.purchaseOrderItem.materialcode
            new TranslationSeedItem("entity.purchaseOrderItem.materialcode", "zh-HK", "物料编码", "物料编码"),

            // entity.purchaseOrderItem.materialname
            new TranslationSeedItem("entity.purchaseOrderItem.materialname", "en-US", "物料名称", "物料名称"),
            // entity.purchaseOrderItem.materialname
            new TranslationSeedItem("entity.purchaseOrderItem.materialname", "ja-JP", "物料名称", "物料名称"),
            // entity.purchaseOrderItem.materialname
            new TranslationSeedItem("entity.purchaseOrderItem.materialname", "zh-CN", "物料名称", "物料名称"),
            // entity.purchaseOrderItem.materialname
            new TranslationSeedItem("entity.purchaseOrderItem.materialname", "zh-HK", "物料名称", "物料名称"),

            // entity.purchaseOrderItem.materialspecification
            new TranslationSeedItem("entity.purchaseOrderItem.materialspecification", "en-US", "物料规格", "物料规格"),
            // entity.purchaseOrderItem.materialspecification
            new TranslationSeedItem("entity.purchaseOrderItem.materialspecification", "ja-JP", "物料规格", "物料规格"),
            // entity.purchaseOrderItem.materialspecification
            new TranslationSeedItem("entity.purchaseOrderItem.materialspecification", "zh-CN", "物料规格", "物料规格"),
            // entity.purchaseOrderItem.materialspecification
            new TranslationSeedItem("entity.purchaseOrderItem.materialspecification", "zh-HK", "物料规格", "物料规格"),

            // entity.purchaseOrderItem.purchaseunit
            new TranslationSeedItem("entity.purchaseOrderItem.purchaseunit", "en-US", "采购单位", "采购单位"),
            // entity.purchaseOrderItem.purchaseunit
            new TranslationSeedItem("entity.purchaseOrderItem.purchaseunit", "ja-JP", "采购单位", "采购单位"),
            // entity.purchaseOrderItem.purchaseunit
            new TranslationSeedItem("entity.purchaseOrderItem.purchaseunit", "zh-CN", "采购单位", "采购单位"),
            // entity.purchaseOrderItem.purchaseunit
            new TranslationSeedItem("entity.purchaseOrderItem.purchaseunit", "zh-HK", "采购单位", "采购单位"),

            // entity.purchaseOrderItem.orderquantity
            new TranslationSeedItem("entity.purchaseOrderItem.orderquantity", "en-US", "订购数量", "订购数量（基本单位数量）"),
            // entity.purchaseOrderItem.orderquantity
            new TranslationSeedItem("entity.purchaseOrderItem.orderquantity", "ja-JP", "订购数量", "订购数量（基本单位数量）"),
            // entity.purchaseOrderItem.orderquantity
            new TranslationSeedItem("entity.purchaseOrderItem.orderquantity", "zh-CN", "订购数量", "订购数量（基本单位数量）"),
            // entity.purchaseOrderItem.orderquantity
            new TranslationSeedItem("entity.purchaseOrderItem.orderquantity", "zh-HK", "订购数量", "订购数量（基本单位数量）"),

            // entity.purchaseOrderItem.receivedquantity
            new TranslationSeedItem("entity.purchaseOrderItem.receivedquantity", "en-US", "已入库数量", "已入库数量（基本单位数量）"),
            // entity.purchaseOrderItem.receivedquantity
            new TranslationSeedItem("entity.purchaseOrderItem.receivedquantity", "ja-JP", "已入库数量", "已入库数量（基本单位数量）"),
            // entity.purchaseOrderItem.receivedquantity
            new TranslationSeedItem("entity.purchaseOrderItem.receivedquantity", "zh-CN", "已入库数量", "已入库数量（基本单位数量）"),
            // entity.purchaseOrderItem.receivedquantity
            new TranslationSeedItem("entity.purchaseOrderItem.receivedquantity", "zh-HK", "已入库数量", "已入库数量（基本单位数量）"),

            // entity.purchaseOrderItem.unitprice
            new TranslationSeedItem("entity.purchaseOrderItem.unitprice", "en-US", "单价", "单价（精确到分，存储为整数，单位为分）"),
            // entity.purchaseOrderItem.unitprice
            new TranslationSeedItem("entity.purchaseOrderItem.unitprice", "ja-JP", "单价", "单价（精确到分，存储为整数，单位为分）"),
            // entity.purchaseOrderItem.unitprice
            new TranslationSeedItem("entity.purchaseOrderItem.unitprice", "zh-CN", "单价", "单价（精确到分，存储为整数，单位为分）"),
            // entity.purchaseOrderItem.unitprice
            new TranslationSeedItem("entity.purchaseOrderItem.unitprice", "zh-HK", "单价", "单价（精确到分，存储为整数，单位为分）"),

            // entity.purchaseOrderItem.discountrate
            new TranslationSeedItem("entity.purchaseOrderItem.discountrate", "en-US", "折扣率", "折扣率（0-100，表示折扣百分比）"),
            // entity.purchaseOrderItem.discountrate
            new TranslationSeedItem("entity.purchaseOrderItem.discountrate", "ja-JP", "折扣率", "折扣率（0-100，表示折扣百分比）"),
            // entity.purchaseOrderItem.discountrate
            new TranslationSeedItem("entity.purchaseOrderItem.discountrate", "zh-CN", "折扣率", "折扣率（0-100，表示折扣百分比）"),
            // entity.purchaseOrderItem.discountrate
            new TranslationSeedItem("entity.purchaseOrderItem.discountrate", "zh-HK", "折扣率", "折扣率（0-100，表示折扣百分比）"),

            // entity.purchaseOrderItem.discountamount
            new TranslationSeedItem("entity.purchaseOrderItem.discountamount", "en-US", "折扣金额", "折扣金额（精确到分，存储为整数，单位为分）"),
            // entity.purchaseOrderItem.discountamount
            new TranslationSeedItem("entity.purchaseOrderItem.discountamount", "ja-JP", "折扣金额", "折扣金额（精确到分，存储为整数，单位为分）"),
            // entity.purchaseOrderItem.discountamount
            new TranslationSeedItem("entity.purchaseOrderItem.discountamount", "zh-CN", "折扣金额", "折扣金额（精确到分，存储为整数，单位为分）"),
            // entity.purchaseOrderItem.discountamount
            new TranslationSeedItem("entity.purchaseOrderItem.discountamount", "zh-HK", "折扣金额", "折扣金额（精确到分，存储为整数，单位为分）"),

            // entity.purchaseOrderItem.taxrate
            new TranslationSeedItem("entity.purchaseOrderItem.taxrate", "en-US", "税费率", "税费率（0-100，表示税费百分比）"),
            // entity.purchaseOrderItem.taxrate
            new TranslationSeedItem("entity.purchaseOrderItem.taxrate", "ja-JP", "税费率", "税费率（0-100，表示税费百分比）"),
            // entity.purchaseOrderItem.taxrate
            new TranslationSeedItem("entity.purchaseOrderItem.taxrate", "zh-CN", "税费率", "税费率（0-100，表示税费百分比）"),
            // entity.purchaseOrderItem.taxrate
            new TranslationSeedItem("entity.purchaseOrderItem.taxrate", "zh-HK", "税费率", "税费率（0-100，表示税费百分比）"),

            // entity.purchaseOrderItem.taxamount
            new TranslationSeedItem("entity.purchaseOrderItem.taxamount", "en-US", "税费", "税费（精确到分，存储为整数，单位为分）"),
            // entity.purchaseOrderItem.taxamount
            new TranslationSeedItem("entity.purchaseOrderItem.taxamount", "ja-JP", "税费", "税费（精确到分，存储为整数，单位为分）"),
            // entity.purchaseOrderItem.taxamount
            new TranslationSeedItem("entity.purchaseOrderItem.taxamount", "zh-CN", "税费", "税费（精确到分，存储为整数，单位为分）"),
            // entity.purchaseOrderItem.taxamount
            new TranslationSeedItem("entity.purchaseOrderItem.taxamount", "zh-HK", "税费", "税费（精确到分，存储为整数，单位为分）"),

            // entity.purchaseOrderItem.subtotalamount
            new TranslationSeedItem("entity.purchaseOrderItem.subtotalamount", "en-US", "小计金额", "小计金额（精确到分，存储为整数，单位为分）"),
            // entity.purchaseOrderItem.subtotalamount
            new TranslationSeedItem("entity.purchaseOrderItem.subtotalamount", "ja-JP", "小计金额", "小计金额（精确到分，存储为整数，单位为分）"),
            // entity.purchaseOrderItem.subtotalamount
            new TranslationSeedItem("entity.purchaseOrderItem.subtotalamount", "zh-CN", "小计金额", "小计金额（精确到分，存储为整数，单位为分）"),
            // entity.purchaseOrderItem.subtotalamount
            new TranslationSeedItem("entity.purchaseOrderItem.subtotalamount", "zh-HK", "小计金额", "小计金额（精确到分，存储为整数，单位为分）"),

            // entity.purchaseOrderItem.deliverystatus
            new TranslationSeedItem("entity.purchaseOrderItem.deliverystatus", "en-US", "行交货状态", "行交货状态（0=未交货，1=部分交货，2=全部交货）"),
            // entity.purchaseOrderItem.deliverystatus
            new TranslationSeedItem("entity.purchaseOrderItem.deliverystatus", "ja-JP", "行交货状态", "行交货状态（0=未交货，1=部分交货，2=全部交货）"),
            // entity.purchaseOrderItem.deliverystatus
            new TranslationSeedItem("entity.purchaseOrderItem.deliverystatus", "zh-CN", "行交货状态", "行交货状态（0=未交货，1=部分交货，2=全部交货）"),
            // entity.purchaseOrderItem.deliverystatus
            new TranslationSeedItem("entity.purchaseOrderItem.deliverystatus", "zh-HK", "行交货状态", "行交货状态（0=未交货，1=部分交货，2=全部交货）"),
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
