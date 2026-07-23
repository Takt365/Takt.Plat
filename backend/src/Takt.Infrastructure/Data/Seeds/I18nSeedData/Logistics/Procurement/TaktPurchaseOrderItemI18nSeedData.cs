// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Procurement
// 文件名称：TaktPurchaseOrderItemI18nSeedData.cs
// 创建时间：2026-07-23
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
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Procurement;

/// <summary>
/// TaktPurchaseOrderItem 实体国际化翻译种子（键前缀 entity.purchaseorderitem.*）
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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 purchaseorderitem 实体翻译...", tenantCode);

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
    /// I18nKey：entity.purchaseorderitem._self / entity.purchaseorderitem.{{field}}；ResourceGroup=Procurement；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetPurchaseOrderItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.purchaseorderitem._self
            new TranslationSeedItem("entity.purchaseorderitem._self", "en-US", "Purchase Order Item Information_us", "实体名称"),
            // entity.purchaseorderitem._self
            new TranslationSeedItem("entity.purchaseorderitem._self", "ja-JP", "Takt采购订单明细信息_jp", "实体名称"),
            // entity.purchaseorderitem._self
            new TranslationSeedItem("entity.purchaseorderitem._self", "zh-CN", "Takt采购订单明细信息", "实体名称"),
            // entity.purchaseorderitem._self
            new TranslationSeedItem("entity.purchaseorderitem._self", "zh-HK", "Takt采购订单明细信息_hk", "实体名称"),

            // entity.purchaseorderitem.purchaseorderid
            new TranslationSeedItem("entity.purchaseorderitem.purchaseorderid", "en-US", "采购订单ID_us", "采购订单 ID（选项 TaktPurchaseOrders/options；DictValue=Id）"),
            // entity.purchaseorderitem.purchaseorderid
            new TranslationSeedItem("entity.purchaseorderitem.purchaseorderid", "ja-JP", "采购订单ID_jp", "采购订单 ID（选项 TaktPurchaseOrders/options；DictValue=Id）"),
            // entity.purchaseorderitem.purchaseorderid
            new TranslationSeedItem("entity.purchaseorderitem.purchaseorderid", "zh-CN", "采购订单ID", "采购订单 ID（选项 TaktPurchaseOrders/options；DictValue=Id）"),
            // entity.purchaseorderitem.purchaseorderid
            new TranslationSeedItem("entity.purchaseorderitem.purchaseorderid", "zh-HK", "采购订单ID_hk", "采购订单 ID（选项 TaktPurchaseOrders/options；DictValue=Id）"),

            // entity.purchaseorderitem.purchaseordercode
            new TranslationSeedItem("entity.purchaseorderitem.purchaseordercode", "en-US", "采购订单编码_us", "采购订单编码（冗余字段，便于查询）"),
            // entity.purchaseorderitem.purchaseordercode
            new TranslationSeedItem("entity.purchaseorderitem.purchaseordercode", "ja-JP", "采购订单编码_jp", "采购订单编码（冗余字段，便于查询）"),
            // entity.purchaseorderitem.purchaseordercode
            new TranslationSeedItem("entity.purchaseorderitem.purchaseordercode", "zh-CN", "采购订单编码", "采购订单编码（冗余字段，便于查询）"),
            // entity.purchaseorderitem.purchaseordercode
            new TranslationSeedItem("entity.purchaseorderitem.purchaseordercode", "zh-HK", "采购订单编码_hk", "采购订单编码（冗余字段，便于查询）"),

            // entity.purchaseorderitem.linenumber
            new TranslationSeedItem("entity.purchaseorderitem.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.purchaseorderitem.linenumber
            new TranslationSeedItem("entity.purchaseorderitem.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.purchaseorderitem.linenumber
            new TranslationSeedItem("entity.purchaseorderitem.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.purchaseorderitem.linenumber
            new TranslationSeedItem("entity.purchaseorderitem.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.purchaseorderitem.requestcode
            new TranslationSeedItem("entity.purchaseorderitem.requestcode", "en-US", "来源请购编码_us", "来源请购编码"),
            // entity.purchaseorderitem.requestcode
            new TranslationSeedItem("entity.purchaseorderitem.requestcode", "ja-JP", "来源请购编码_jp", "来源请购编码"),
            // entity.purchaseorderitem.requestcode
            new TranslationSeedItem("entity.purchaseorderitem.requestcode", "zh-CN", "来源请购编码", "来源请购编码"),
            // entity.purchaseorderitem.requestcode
            new TranslationSeedItem("entity.purchaseorderitem.requestcode", "zh-HK", "来源请购编码_hk", "来源请购编码"),

            // entity.purchaseorderitem.requestlinenumber
            new TranslationSeedItem("entity.purchaseorderitem.requestlinenumber", "en-US", "来源请购行号_us", "来源请购行号"),
            // entity.purchaseorderitem.requestlinenumber
            new TranslationSeedItem("entity.purchaseorderitem.requestlinenumber", "ja-JP", "来源请购行号_jp", "来源请购行号"),
            // entity.purchaseorderitem.requestlinenumber
            new TranslationSeedItem("entity.purchaseorderitem.requestlinenumber", "zh-CN", "来源请购行号", "来源请购行号"),
            // entity.purchaseorderitem.requestlinenumber
            new TranslationSeedItem("entity.purchaseorderitem.requestlinenumber", "zh-HK", "来源请购行号_hk", "来源请购行号"),

            // entity.purchaseorderitem.materialcode
            new TranslationSeedItem("entity.purchaseorderitem.materialcode", "en-US", "物料编码_us", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.purchaseorderitem.materialcode
            new TranslationSeedItem("entity.purchaseorderitem.materialcode", "ja-JP", "物料编码_jp", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.purchaseorderitem.materialcode
            new TranslationSeedItem("entity.purchaseorderitem.materialcode", "zh-CN", "物料编码", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.purchaseorderitem.materialcode
            new TranslationSeedItem("entity.purchaseorderitem.materialcode", "zh-HK", "物料编码_hk", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),

            // entity.purchaseorderitem.materialname
            new TranslationSeedItem("entity.purchaseorderitem.materialname", "en-US", "物料名称_us", "物料名称（回填：随物料）"),
            // entity.purchaseorderitem.materialname
            new TranslationSeedItem("entity.purchaseorderitem.materialname", "ja-JP", "物料名称_jp", "物料名称（回填：随物料）"),
            // entity.purchaseorderitem.materialname
            new TranslationSeedItem("entity.purchaseorderitem.materialname", "zh-CN", "物料名称", "物料名称（回填：随物料）"),
            // entity.purchaseorderitem.materialname
            new TranslationSeedItem("entity.purchaseorderitem.materialname", "zh-HK", "物料名称_hk", "物料名称（回填：随物料）"),

            // entity.purchaseorderitem.materialspecification
            new TranslationSeedItem("entity.purchaseorderitem.materialspecification", "en-US", "物料规格_us", "物料规格（回填：随物料）"),
            // entity.purchaseorderitem.materialspecification
            new TranslationSeedItem("entity.purchaseorderitem.materialspecification", "ja-JP", "物料规格_jp", "物料规格（回填：随物料）"),
            // entity.purchaseorderitem.materialspecification
            new TranslationSeedItem("entity.purchaseorderitem.materialspecification", "zh-CN", "物料规格", "物料规格（回填：随物料）"),
            // entity.purchaseorderitem.materialspecification
            new TranslationSeedItem("entity.purchaseorderitem.materialspecification", "zh-HK", "物料规格_hk", "物料规格（回填：随物料）"),

            // entity.purchaseorderitem.purchaseunit
            new TranslationSeedItem("entity.purchaseorderitem.purchaseunit", "en-US", "采购单位_us", "采购单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),
            // entity.purchaseorderitem.purchaseunit
            new TranslationSeedItem("entity.purchaseorderitem.purchaseunit", "ja-JP", "采购单位_jp", "采购单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),
            // entity.purchaseorderitem.purchaseunit
            new TranslationSeedItem("entity.purchaseorderitem.purchaseunit", "zh-CN", "采购单位", "采购单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),
            // entity.purchaseorderitem.purchaseunit
            new TranslationSeedItem("entity.purchaseorderitem.purchaseunit", "zh-HK", "采购单位_hk", "采购单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),

            // entity.purchaseorderitem.orderquantity
            new TranslationSeedItem("entity.purchaseorderitem.orderquantity", "en-US", "订购数量_us", "订购数量（基本单位数量）"),
            // entity.purchaseorderitem.orderquantity
            new TranslationSeedItem("entity.purchaseorderitem.orderquantity", "ja-JP", "订购数量_jp", "订购数量（基本单位数量）"),
            // entity.purchaseorderitem.orderquantity
            new TranslationSeedItem("entity.purchaseorderitem.orderquantity", "zh-CN", "订购数量", "订购数量（基本单位数量）"),
            // entity.purchaseorderitem.orderquantity
            new TranslationSeedItem("entity.purchaseorderitem.orderquantity", "zh-HK", "订购数量_hk", "订购数量（基本单位数量）"),

            // entity.purchaseorderitem.receivedquantity
            new TranslationSeedItem("entity.purchaseorderitem.receivedquantity", "en-US", "已入库数量_us", "已入库数量（基本单位数量）"),
            // entity.purchaseorderitem.receivedquantity
            new TranslationSeedItem("entity.purchaseorderitem.receivedquantity", "ja-JP", "已入库数量_jp", "已入库数量（基本单位数量）"),
            // entity.purchaseorderitem.receivedquantity
            new TranslationSeedItem("entity.purchaseorderitem.receivedquantity", "zh-CN", "已入库数量", "已入库数量（基本单位数量）"),
            // entity.purchaseorderitem.receivedquantity
            new TranslationSeedItem("entity.purchaseorderitem.receivedquantity", "zh-HK", "已入库数量_hk", "已入库数量（基本单位数量）"),

            // entity.purchaseorderitem.purchaseperunit
            new TranslationSeedItem("entity.purchaseorderitem.purchaseperunit", "en-US", "价格单位_us", "价格单位（字典 logistics_price_unit_param：1/100/1000/10000；默认 1000）"),
            // entity.purchaseorderitem.purchaseperunit
            new TranslationSeedItem("entity.purchaseorderitem.purchaseperunit", "ja-JP", "价格单位_jp", "价格单位（字典 logistics_price_unit_param：1/100/1000/10000；默认 1000）"),
            // entity.purchaseorderitem.purchaseperunit
            new TranslationSeedItem("entity.purchaseorderitem.purchaseperunit", "zh-CN", "价格单位", "价格单位（字典 logistics_price_unit_param：1/100/1000/10000；默认 1000）"),
            // entity.purchaseorderitem.purchaseperunit
            new TranslationSeedItem("entity.purchaseorderitem.purchaseperunit", "zh-HK", "价格单位_hk", "价格单位（字典 logistics_price_unit_param：1/100/1000/10000；默认 1000）"),

            // entity.purchaseorderitem.purchaseunitprice
            new TranslationSeedItem("entity.purchaseorderitem.purchaseunitprice", "en-US", "采购单价_us", "采购单价"),
            // entity.purchaseorderitem.purchaseunitprice
            new TranslationSeedItem("entity.purchaseorderitem.purchaseunitprice", "ja-JP", "采购单价_jp", "采购单价"),
            // entity.purchaseorderitem.purchaseunitprice
            new TranslationSeedItem("entity.purchaseorderitem.purchaseunitprice", "zh-CN", "采购单价", "采购单价"),
            // entity.purchaseorderitem.purchaseunitprice
            new TranslationSeedItem("entity.purchaseorderitem.purchaseunitprice", "zh-HK", "采购单价_hk", "采购单价"),

            // entity.purchaseorderitem.discountrate
            new TranslationSeedItem("entity.purchaseorderitem.discountrate", "en-US", "折扣率_us", "折扣率（字典 logistics_discount_rate_param 预设或手输；0-100，表示折扣百分比）"),
            // entity.purchaseorderitem.discountrate
            new TranslationSeedItem("entity.purchaseorderitem.discountrate", "ja-JP", "折扣率_jp", "折扣率（字典 logistics_discount_rate_param 预设或手输；0-100，表示折扣百分比）"),
            // entity.purchaseorderitem.discountrate
            new TranslationSeedItem("entity.purchaseorderitem.discountrate", "zh-CN", "折扣率", "折扣率（字典 logistics_discount_rate_param 预设或手输；0-100，表示折扣百分比）"),
            // entity.purchaseorderitem.discountrate
            new TranslationSeedItem("entity.purchaseorderitem.discountrate", "zh-HK", "折扣率_hk", "折扣率（字典 logistics_discount_rate_param 预设或手输；0-100，表示折扣百分比）"),

            // entity.purchaseorderitem.discountamount
            new TranslationSeedItem("entity.purchaseorderitem.discountamount", "en-US", "折扣金额_us", "折扣金额（精确到分，存储为整数，单位为分）"),
            // entity.purchaseorderitem.discountamount
            new TranslationSeedItem("entity.purchaseorderitem.discountamount", "ja-JP", "折扣金额_jp", "折扣金额（精确到分，存储为整数，单位为分）"),
            // entity.purchaseorderitem.discountamount
            new TranslationSeedItem("entity.purchaseorderitem.discountamount", "zh-CN", "折扣金额", "折扣金额（精确到分，存储为整数，单位为分）"),
            // entity.purchaseorderitem.discountamount
            new TranslationSeedItem("entity.purchaseorderitem.discountamount", "zh-HK", "折扣金额_hk", "折扣金额（精确到分，存储为整数，单位为分）"),

            // entity.purchaseorderitem.taxincludedamount
            new TranslationSeedItem("entity.purchaseorderitem.taxincludedamount", "en-US", "含税金额_us", "含税金额"),
            // entity.purchaseorderitem.taxincludedamount
            new TranslationSeedItem("entity.purchaseorderitem.taxincludedamount", "ja-JP", "含税金额_jp", "含税金额"),
            // entity.purchaseorderitem.taxincludedamount
            new TranslationSeedItem("entity.purchaseorderitem.taxincludedamount", "zh-CN", "含税金额", "含税金额"),
            // entity.purchaseorderitem.taxincludedamount
            new TranslationSeedItem("entity.purchaseorderitem.taxincludedamount", "zh-HK", "含税金额_hk", "含税金额"),

            // entity.purchaseorderitem.untaxedamount
            new TranslationSeedItem("entity.purchaseorderitem.untaxedamount", "en-US", "未税金额_us", "未税金额"),
            // entity.purchaseorderitem.untaxedamount
            new TranslationSeedItem("entity.purchaseorderitem.untaxedamount", "ja-JP", "未税金额_jp", "未税金额"),
            // entity.purchaseorderitem.untaxedamount
            new TranslationSeedItem("entity.purchaseorderitem.untaxedamount", "zh-CN", "未税金额", "未税金额"),
            // entity.purchaseorderitem.untaxedamount
            new TranslationSeedItem("entity.purchaseorderitem.untaxedamount", "zh-HK", "未税金额_hk", "未税金额"),

            // entity.purchaseorderitem.taxamount
            new TranslationSeedItem("entity.purchaseorderitem.taxamount", "en-US", "税费_us", "税费"),
            // entity.purchaseorderitem.taxamount
            new TranslationSeedItem("entity.purchaseorderitem.taxamount", "ja-JP", "税费_jp", "税费"),
            // entity.purchaseorderitem.taxamount
            new TranslationSeedItem("entity.purchaseorderitem.taxamount", "zh-CN", "税费", "税费"),
            // entity.purchaseorderitem.taxamount
            new TranslationSeedItem("entity.purchaseorderitem.taxamount", "zh-HK", "税费_hk", "税费"),

            // entity.purchaseorderitem.deliverystatus
            new TranslationSeedItem("entity.purchaseorderitem.deliverystatus", "en-US", "行交货状态_us", "行交货状态（字典 logistics_delivery_status；0=未交货，1=部分交货，2=全部交货）"),
            // entity.purchaseorderitem.deliverystatus
            new TranslationSeedItem("entity.purchaseorderitem.deliverystatus", "ja-JP", "行交货状态_jp", "行交货状态（字典 logistics_delivery_status；0=未交货，1=部分交货，2=全部交货）"),
            // entity.purchaseorderitem.deliverystatus
            new TranslationSeedItem("entity.purchaseorderitem.deliverystatus", "zh-CN", "行交货状态", "行交货状态（字典 logistics_delivery_status；0=未交货，1=部分交货，2=全部交货）"),
            // entity.purchaseorderitem.deliverystatus
            new TranslationSeedItem("entity.purchaseorderitem.deliverystatus", "zh-HK", "行交货状态_hk", "行交货状态（字典 logistics_delivery_status；0=未交货，1=部分交货，2=全部交货）"),

            // entity.purchaseorderitem.isobsolete
            new TranslationSeedItem("entity.purchaseorderitem.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.purchaseorderitem.isobsolete
            new TranslationSeedItem("entity.purchaseorderitem.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.purchaseorderitem.isobsolete
            new TranslationSeedItem("entity.purchaseorderitem.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.purchaseorderitem.isobsolete
            new TranslationSeedItem("entity.purchaseorderitem.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）"),
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
