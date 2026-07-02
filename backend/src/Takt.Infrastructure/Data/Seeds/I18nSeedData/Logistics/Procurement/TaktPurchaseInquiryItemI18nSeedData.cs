// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Procurement
// 文件名称：TaktPurchaseInquiryItemI18nSeedData.cs
// 创建时间：2026-07-02
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktPurchaseInquiryItem 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktPurchaseInquiryItem 实体国际化翻译种子（键前缀 entity.purchaseinquiryitem.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktPurchaseInquiryItemI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktPurchaseInquiryItem 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 purchaseinquiryitem 实体翻译...", tenantCode);

        foreach (var item in GetPurchaseInquiryItemTranslations())
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

        TaktLogger.Information("TaktPurchaseInquiryItem 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktPurchaseInquiryItem 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.purchaseinquiryitem._self / entity.purchaseinquiryitem.{{field}}；ResourceGroup=Procurement；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetPurchaseInquiryItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.purchaseinquiryitem._self
            new TranslationSeedItem("entity.purchaseinquiryitem._self", "en-US", "Purchase Inquiry Item Information_us", "实体名称"),
            // entity.purchaseinquiryitem._self
            new TranslationSeedItem("entity.purchaseinquiryitem._self", "ja-JP", "采购询价明细信息_jp", "实体名称"),
            // entity.purchaseinquiryitem._self
            new TranslationSeedItem("entity.purchaseinquiryitem._self", "zh-CN", "采购询价明细信息", "实体名称"),
            // entity.purchaseinquiryitem._self
            new TranslationSeedItem("entity.purchaseinquiryitem._self", "zh-HK", "采购询价明细信息_hk", "实体名称"),

            // entity.purchaseinquiryitem.purchaseinquiryid
            new TranslationSeedItem("entity.purchaseinquiryitem.purchaseinquiryid", "en-US", "采购询价ID_us", "采购询价 ID（关联 TaktPurchaseInquiry.Id，选项 TaktPurchaseInquirys/options）"),
            // entity.purchaseinquiryitem.purchaseinquiryid
            new TranslationSeedItem("entity.purchaseinquiryitem.purchaseinquiryid", "ja-JP", "采购询价ID_jp", "采购询价 ID（关联 TaktPurchaseInquiry.Id，选项 TaktPurchaseInquirys/options）"),
            // entity.purchaseinquiryitem.purchaseinquiryid
            new TranslationSeedItem("entity.purchaseinquiryitem.purchaseinquiryid", "zh-CN", "采购询价ID", "采购询价 ID（关联 TaktPurchaseInquiry.Id，选项 TaktPurchaseInquirys/options）"),
            // entity.purchaseinquiryitem.purchaseinquiryid
            new TranslationSeedItem("entity.purchaseinquiryitem.purchaseinquiryid", "zh-HK", "采购询价ID_hk", "采购询价 ID（关联 TaktPurchaseInquiry.Id，选项 TaktPurchaseInquirys/options）"),

            // entity.purchaseinquiryitem.purchaseinquirycode
            new TranslationSeedItem("entity.purchaseinquiryitem.purchaseinquirycode", "en-US", "采购询价编码_us", "采购询价编码（冗余，便于查询）"),
            // entity.purchaseinquiryitem.purchaseinquirycode
            new TranslationSeedItem("entity.purchaseinquiryitem.purchaseinquirycode", "ja-JP", "采购询价编码_jp", "采购询价编码（冗余，便于查询）"),
            // entity.purchaseinquiryitem.purchaseinquirycode
            new TranslationSeedItem("entity.purchaseinquiryitem.purchaseinquirycode", "zh-CN", "采购询价编码", "采购询价编码（冗余，便于查询）"),
            // entity.purchaseinquiryitem.purchaseinquirycode
            new TranslationSeedItem("entity.purchaseinquiryitem.purchaseinquirycode", "zh-HK", "采购询价编码_hk", "采购询价编码（冗余，便于查询）"),

            // entity.purchaseinquiryitem.linenumber
            new TranslationSeedItem("entity.purchaseinquiryitem.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.purchaseinquiryitem.linenumber
            new TranslationSeedItem("entity.purchaseinquiryitem.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.purchaseinquiryitem.linenumber
            new TranslationSeedItem("entity.purchaseinquiryitem.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.purchaseinquiryitem.linenumber
            new TranslationSeedItem("entity.purchaseinquiryitem.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.purchaseinquiryitem.allocationcategory
            new TranslationSeedItem("entity.purchaseinquiryitem.allocationcategory", "en-US", "分配类别_us", "分配类别（字典 logistics_allocation_category；A=资产，K=成本中心，F=订单）"),
            // entity.purchaseinquiryitem.allocationcategory
            new TranslationSeedItem("entity.purchaseinquiryitem.allocationcategory", "ja-JP", "分配类别_jp", "分配类别（字典 logistics_allocation_category；A=资产，K=成本中心，F=订单）"),
            // entity.purchaseinquiryitem.allocationcategory
            new TranslationSeedItem("entity.purchaseinquiryitem.allocationcategory", "zh-CN", "分配类别", "分配类别（字典 logistics_allocation_category；A=资产，K=成本中心，F=订单）"),
            // entity.purchaseinquiryitem.allocationcategory
            new TranslationSeedItem("entity.purchaseinquiryitem.allocationcategory", "zh-HK", "分配类别_hk", "分配类别（字典 logistics_allocation_category；A=资产，K=成本中心，F=订单）"),

            // entity.purchaseinquiryitem.materialcode
            new TranslationSeedItem("entity.purchaseinquiryitem.materialcode", "en-US", "物料编码_us", "物料编码（选项 TaktMaterials/options，DictValue=MaterialCode）"),
            // entity.purchaseinquiryitem.materialcode
            new TranslationSeedItem("entity.purchaseinquiryitem.materialcode", "ja-JP", "物料编码_jp", "物料编码（选项 TaktMaterials/options，DictValue=MaterialCode）"),
            // entity.purchaseinquiryitem.materialcode
            new TranslationSeedItem("entity.purchaseinquiryitem.materialcode", "zh-CN", "物料编码", "物料编码（选项 TaktMaterials/options，DictValue=MaterialCode）"),
            // entity.purchaseinquiryitem.materialcode
            new TranslationSeedItem("entity.purchaseinquiryitem.materialcode", "zh-HK", "物料编码_hk", "物料编码（选项 TaktMaterials/options，DictValue=MaterialCode）"),

            // entity.purchaseinquiryitem.materialname
            new TranslationSeedItem("entity.purchaseinquiryitem.materialname", "en-US", "物料名称_us", "物料名称"),
            // entity.purchaseinquiryitem.materialname
            new TranslationSeedItem("entity.purchaseinquiryitem.materialname", "ja-JP", "物料名称_jp", "物料名称"),
            // entity.purchaseinquiryitem.materialname
            new TranslationSeedItem("entity.purchaseinquiryitem.materialname", "zh-CN", "物料名称", "物料名称"),
            // entity.purchaseinquiryitem.materialname
            new TranslationSeedItem("entity.purchaseinquiryitem.materialname", "zh-HK", "物料名称_hk", "物料名称"),

            // entity.purchaseinquiryitem.materialspecification
            new TranslationSeedItem("entity.purchaseinquiryitem.materialspecification", "en-US", "物料规格_us", "物料规格"),
            // entity.purchaseinquiryitem.materialspecification
            new TranslationSeedItem("entity.purchaseinquiryitem.materialspecification", "ja-JP", "物料规格_jp", "物料规格"),
            // entity.purchaseinquiryitem.materialspecification
            new TranslationSeedItem("entity.purchaseinquiryitem.materialspecification", "zh-CN", "物料规格", "物料规格"),
            // entity.purchaseinquiryitem.materialspecification
            new TranslationSeedItem("entity.purchaseinquiryitem.materialspecification", "zh-HK", "物料规格_hk", "物料规格"),

            // entity.purchaseinquiryitem.inquiryunit
            new TranslationSeedItem("entity.purchaseinquiryitem.inquiryunit", "en-US", "询价单位_us", "询价单位"),
            // entity.purchaseinquiryitem.inquiryunit
            new TranslationSeedItem("entity.purchaseinquiryitem.inquiryunit", "ja-JP", "询价单位_jp", "询价单位"),
            // entity.purchaseinquiryitem.inquiryunit
            new TranslationSeedItem("entity.purchaseinquiryitem.inquiryunit", "zh-CN", "询价单位", "询价单位"),
            // entity.purchaseinquiryitem.inquiryunit
            new TranslationSeedItem("entity.purchaseinquiryitem.inquiryunit", "zh-HK", "询价单位_hk", "询价单位"),

            // entity.purchaseinquiryitem.inquiryquantity
            new TranslationSeedItem("entity.purchaseinquiryitem.inquiryquantity", "en-US", "询价数量_us", "询价数量（基本单位数量，decimal(18,5)）"),
            // entity.purchaseinquiryitem.inquiryquantity
            new TranslationSeedItem("entity.purchaseinquiryitem.inquiryquantity", "ja-JP", "询价数量_jp", "询价数量（基本单位数量，decimal(18,5)）"),
            // entity.purchaseinquiryitem.inquiryquantity
            new TranslationSeedItem("entity.purchaseinquiryitem.inquiryquantity", "zh-CN", "询价数量", "询价数量（基本单位数量，decimal(18,5)）"),
            // entity.purchaseinquiryitem.inquiryquantity
            new TranslationSeedItem("entity.purchaseinquiryitem.inquiryquantity", "zh-HK", "询价数量_hk", "询价数量（基本单位数量，decimal(18,5)）"),

            // entity.purchaseinquiryitem.purchaseperunit
            new TranslationSeedItem("entity.purchaseinquiryitem.purchaseperunit", "en-US", "价格单位_us", "价格单位（字典 logistics_price_unit_param：1/100/1000/10000；默认 1000）"),
            // entity.purchaseinquiryitem.purchaseperunit
            new TranslationSeedItem("entity.purchaseinquiryitem.purchaseperunit", "ja-JP", "价格单位_jp", "价格单位（字典 logistics_price_unit_param：1/100/1000/10000；默认 1000）"),
            // entity.purchaseinquiryitem.purchaseperunit
            new TranslationSeedItem("entity.purchaseinquiryitem.purchaseperunit", "zh-CN", "价格单位", "价格单位（字典 logistics_price_unit_param：1/100/1000/10000；默认 1000）"),
            // entity.purchaseinquiryitem.purchaseperunit
            new TranslationSeedItem("entity.purchaseinquiryitem.purchaseperunit", "zh-HK", "价格单位_hk", "价格单位（字典 logistics_price_unit_param：1/100/1000/10000；默认 1000）"),

            // entity.purchaseinquiryitem.quotedunitprice
            new TranslationSeedItem("entity.purchaseinquiryitem.quotedunitprice", "en-US", "报价单价_us", "报价单价"),
            // entity.purchaseinquiryitem.quotedunitprice
            new TranslationSeedItem("entity.purchaseinquiryitem.quotedunitprice", "ja-JP", "报价单价_jp", "报价单价"),
            // entity.purchaseinquiryitem.quotedunitprice
            new TranslationSeedItem("entity.purchaseinquiryitem.quotedunitprice", "zh-CN", "报价单价", "报价单价"),
            // entity.purchaseinquiryitem.quotedunitprice
            new TranslationSeedItem("entity.purchaseinquiryitem.quotedunitprice", "zh-HK", "报价单价_hk", "报价单价"),

            // entity.purchaseinquiryitem.quotedamount
            new TranslationSeedItem("entity.purchaseinquiryitem.quotedamount", "en-US", "报价金额_us", "报价金额"),
            // entity.purchaseinquiryitem.quotedamount
            new TranslationSeedItem("entity.purchaseinquiryitem.quotedamount", "ja-JP", "报价金额_jp", "报价金额"),
            // entity.purchaseinquiryitem.quotedamount
            new TranslationSeedItem("entity.purchaseinquiryitem.quotedamount", "zh-CN", "报价金额", "报价金额"),
            // entity.purchaseinquiryitem.quotedamount
            new TranslationSeedItem("entity.purchaseinquiryitem.quotedamount", "zh-HK", "报价金额_hk", "报价金额"),

            // entity.purchaseinquiryitem.targetsuppliercode
            new TranslationSeedItem("entity.purchaseinquiryitem.targetsuppliercode", "en-US", "目标供应商编码_us", "目标供应商编码（选项 TaktSuppliers/options，DictValue=SupplierCode）"),
            // entity.purchaseinquiryitem.targetsuppliercode
            new TranslationSeedItem("entity.purchaseinquiryitem.targetsuppliercode", "ja-JP", "目标供应商编码_jp", "目标供应商编码（选项 TaktSuppliers/options，DictValue=SupplierCode）"),
            // entity.purchaseinquiryitem.targetsuppliercode
            new TranslationSeedItem("entity.purchaseinquiryitem.targetsuppliercode", "zh-CN", "目标供应商编码", "目标供应商编码（选项 TaktSuppliers/options，DictValue=SupplierCode）"),
            // entity.purchaseinquiryitem.targetsuppliercode
            new TranslationSeedItem("entity.purchaseinquiryitem.targetsuppliercode", "zh-HK", "目标供应商编码_hk", "目标供应商编码（选项 TaktSuppliers/options，DictValue=SupplierCode）"),

            // entity.purchaseinquiryitem.targetsuppliername
            new TranslationSeedItem("entity.purchaseinquiryitem.targetsuppliername", "en-US", "目标供应商名称_us", "目标供应商名称"),
            // entity.purchaseinquiryitem.targetsuppliername
            new TranslationSeedItem("entity.purchaseinquiryitem.targetsuppliername", "ja-JP", "目标供应商名称_jp", "目标供应商名称"),
            // entity.purchaseinquiryitem.targetsuppliername
            new TranslationSeedItem("entity.purchaseinquiryitem.targetsuppliername", "zh-CN", "目标供应商名称", "目标供应商名称"),
            // entity.purchaseinquiryitem.targetsuppliername
            new TranslationSeedItem("entity.purchaseinquiryitem.targetsuppliername", "zh-HK", "目标供应商名称_hk", "目标供应商名称"),
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
