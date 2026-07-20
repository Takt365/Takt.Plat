// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Procurement
// 文件名称：TaktPurchasePriceItemI18nSeedData.cs
// 创建时间：2026-07-20
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktPurchasePriceItem 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktPurchasePriceItem 实体国际化翻译种子（键前缀 entity.purchasepriceitem.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktPurchasePriceItemI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktPurchasePriceItem 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 purchasepriceitem 实体翻译...", tenantCode);

        foreach (var item in GetPurchasePriceItemTranslations())
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

        TaktLogger.Information("TaktPurchasePriceItem 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktPurchasePriceItem 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.purchasepriceitem._self / entity.purchasepriceitem.{{field}}；ResourceGroup=Procurement；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetPurchasePriceItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.purchasepriceitem._self
            new TranslationSeedItem("entity.purchasepriceitem._self", "en-US", "Purchase Price Item Information_us", "实体名称"),
            // entity.purchasepriceitem._self
            new TranslationSeedItem("entity.purchasepriceitem._self", "ja-JP", "Takt采购价格明细信息_jp", "实体名称"),
            // entity.purchasepriceitem._self
            new TranslationSeedItem("entity.purchasepriceitem._self", "zh-CN", "Takt采购价格明细信息", "实体名称"),
            // entity.purchasepriceitem._self
            new TranslationSeedItem("entity.purchasepriceitem._self", "zh-HK", "Takt采购价格明细信息_hk", "实体名称"),

            // entity.purchasepriceitem.purchasepriceid
            new TranslationSeedItem("entity.purchasepriceitem.purchasepriceid", "en-US", "采购价格ID_us", "采购价格 ID（主子表关系；选项 TaktPurchasePrices/options，DictValue=Id）"),
            // entity.purchasepriceitem.purchasepriceid
            new TranslationSeedItem("entity.purchasepriceitem.purchasepriceid", "ja-JP", "采购价格ID_jp", "采购价格 ID（主子表关系；选项 TaktPurchasePrices/options，DictValue=Id）"),
            // entity.purchasepriceitem.purchasepriceid
            new TranslationSeedItem("entity.purchasepriceitem.purchasepriceid", "zh-CN", "采购价格ID", "采购价格 ID（主子表关系；选项 TaktPurchasePrices/options，DictValue=Id）"),
            // entity.purchasepriceitem.purchasepriceid
            new TranslationSeedItem("entity.purchasepriceitem.purchasepriceid", "zh-HK", "采购价格ID_hk", "采购价格 ID（主子表关系；选项 TaktPurchasePrices/options，DictValue=Id）"),

            // entity.purchasepriceitem.purchasepricecode
            new TranslationSeedItem("entity.purchasepriceitem.purchasepricecode", "en-US", "定价记录号_us", "定价记录号（冗余；与主表 PurchasePriceCode 一致，长度 20）"),
            // entity.purchasepriceitem.purchasepricecode
            new TranslationSeedItem("entity.purchasepriceitem.purchasepricecode", "ja-JP", "定价记录号_jp", "定价记录号（冗余；与主表 PurchasePriceCode 一致，长度 20）"),
            // entity.purchasepriceitem.purchasepricecode
            new TranslationSeedItem("entity.purchasepriceitem.purchasepricecode", "zh-CN", "定价记录号", "定价记录号（冗余；与主表 PurchasePriceCode 一致，长度 20）"),
            // entity.purchasepriceitem.purchasepricecode
            new TranslationSeedItem("entity.purchasepriceitem.purchasepricecode", "zh-HK", "定价记录号_hk", "定价记录号（冗余；与主表 PurchasePriceCode 一致，长度 20）"),

            // entity.purchasepriceitem.purchasepriceseq
            new TranslationSeedItem("entity.purchasepriceitem.purchasepriceseq", "en-US", "定价序号_us", "定价序号（项号/序号，固定步长=10）"),
            // entity.purchasepriceitem.purchasepriceseq
            new TranslationSeedItem("entity.purchasepriceitem.purchasepriceseq", "ja-JP", "定价序号_jp", "定价序号（项号/序号，固定步长=10）"),
            // entity.purchasepriceitem.purchasepriceseq
            new TranslationSeedItem("entity.purchasepriceitem.purchasepriceseq", "zh-CN", "定价序号", "定价序号（项号/序号，固定步长=10）"),
            // entity.purchasepriceitem.purchasepriceseq
            new TranslationSeedItem("entity.purchasepriceitem.purchasepriceseq", "zh-HK", "定价序号_hk", "定价序号（项号/序号，固定步长=10）"),

            // entity.purchasepriceitem.pricetype
            new TranslationSeedItem("entity.purchasepriceitem.pricetype", "en-US", "条件类型_us", "条件类型（冗余；字典 logistics_price_type；与主表 PriceType 一致，PB00/PR00/MWST/MWRK/NLXV）"),
            // entity.purchasepriceitem.pricetype
            new TranslationSeedItem("entity.purchasepriceitem.pricetype", "ja-JP", "条件类型_jp", "条件类型（冗余；字典 logistics_price_type；与主表 PriceType 一致，PB00/PR00/MWST/MWRK/NLXV）"),
            // entity.purchasepriceitem.pricetype
            new TranslationSeedItem("entity.purchasepriceitem.pricetype", "zh-CN", "条件类型", "条件类型（冗余；字典 logistics_price_type；与主表 PriceType 一致，PB00/PR00/MWST/MWRK/NLXV）"),
            // entity.purchasepriceitem.pricetype
            new TranslationSeedItem("entity.purchasepriceitem.pricetype", "zh-HK", "条件类型_hk", "条件类型（冗余；字典 logistics_price_type；与主表 PriceType 一致，PB00/PR00/MWST/MWRK/NLXV）"),

            // entity.purchasepriceitem.scaletype
            new TranslationSeedItem("entity.purchasepriceitem.scaletype", "en-US", "等级类型_us", "等级类型（字典 logistics_scale_type；SAP STFKZ；A=基础等级，B=到等级，C=未使用，D=累进间隔等级）"),
            // entity.purchasepriceitem.scaletype
            new TranslationSeedItem("entity.purchasepriceitem.scaletype", "ja-JP", "等级类型_jp", "等级类型（字典 logistics_scale_type；SAP STFKZ；A=基础等级，B=到等级，C=未使用，D=累进间隔等级）"),
            // entity.purchasepriceitem.scaletype
            new TranslationSeedItem("entity.purchasepriceitem.scaletype", "zh-CN", "等级类型", "等级类型（字典 logistics_scale_type；SAP STFKZ；A=基础等级，B=到等级，C=未使用，D=累进间隔等级）"),
            // entity.purchasepriceitem.scaletype
            new TranslationSeedItem("entity.purchasepriceitem.scaletype", "zh-HK", "等级类型_hk", "等级类型（字典 logistics_scale_type；SAP STFKZ；A=基础等级，B=到等级，C=未使用，D=累进间隔等级）"),

            // entity.purchasepriceitem.scalebasis
            new TranslationSeedItem("entity.purchasepriceitem.scalebasis", "en-US", "等级基础_us", "等级基础（字典 logistics_scale_basis；SAP KZBZG；B=价值等级，C=数量规模，…）"),
            // entity.purchasepriceitem.scalebasis
            new TranslationSeedItem("entity.purchasepriceitem.scalebasis", "ja-JP", "等级基础_jp", "等级基础（字典 logistics_scale_basis；SAP KZBZG；B=价值等级，C=数量规模，…）"),
            // entity.purchasepriceitem.scalebasis
            new TranslationSeedItem("entity.purchasepriceitem.scalebasis", "zh-CN", "等级基础", "等级基础（字典 logistics_scale_basis；SAP KZBZG；B=价值等级，C=数量规模，…）"),
            // entity.purchasepriceitem.scalebasis
            new TranslationSeedItem("entity.purchasepriceitem.scalebasis", "zh-HK", "等级基础_hk", "等级基础（字典 logistics_scale_basis；SAP KZBZG；B=价值等级，C=数量规模，…）"),

            // entity.purchasepriceitem.scalequantity
            new TranslationSeedItem("entity.purchasepriceitem.scalequantity", "en-US", "等级数量_us", "等级数量"),
            // entity.purchasepriceitem.scalequantity
            new TranslationSeedItem("entity.purchasepriceitem.scalequantity", "ja-JP", "等级数量_jp", "等级数量"),
            // entity.purchasepriceitem.scalequantity
            new TranslationSeedItem("entity.purchasepriceitem.scalequantity", "zh-CN", "等级数量", "等级数量"),
            // entity.purchasepriceitem.scalequantity
            new TranslationSeedItem("entity.purchasepriceitem.scalequantity", "zh-HK", "等级数量_hk", "等级数量"),

            // entity.purchasepriceitem.scaleunit
            new TranslationSeedItem("entity.purchasepriceitem.scaleunit", "en-US", "等级单位_us", "等级单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等）"),
            // entity.purchasepriceitem.scaleunit
            new TranslationSeedItem("entity.purchasepriceitem.scaleunit", "ja-JP", "等级单位_jp", "等级单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等）"),
            // entity.purchasepriceitem.scaleunit
            new TranslationSeedItem("entity.purchasepriceitem.scaleunit", "zh-CN", "等级单位", "等级单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等）"),
            // entity.purchasepriceitem.scaleunit
            new TranslationSeedItem("entity.purchasepriceitem.scaleunit", "zh-HK", "等级单位_hk", "等级单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等）"),

            // entity.purchasepriceitem.scalevalue
            new TranslationSeedItem("entity.purchasepriceitem.scalevalue", "en-US", "等级值_us", "等级值"),
            // entity.purchasepriceitem.scalevalue
            new TranslationSeedItem("entity.purchasepriceitem.scalevalue", "ja-JP", "等级值_jp", "等级值"),
            // entity.purchasepriceitem.scalevalue
            new TranslationSeedItem("entity.purchasepriceitem.scalevalue", "zh-CN", "等级值", "等级值"),
            // entity.purchasepriceitem.scalevalue
            new TranslationSeedItem("entity.purchasepriceitem.scalevalue", "zh-HK", "等级值_hk", "等级值"),

            // entity.purchasepriceitem.scalecurrency
            new TranslationSeedItem("entity.purchasepriceitem.scalecurrency", "en-US", "等级货币_us", "等级货币（字典 accounting_currency_code，DictValue=CNY/USD 等）"),
            // entity.purchasepriceitem.scalecurrency
            new TranslationSeedItem("entity.purchasepriceitem.scalecurrency", "ja-JP", "等级货币_jp", "等级货币（字典 accounting_currency_code，DictValue=CNY/USD 等）"),
            // entity.purchasepriceitem.scalecurrency
            new TranslationSeedItem("entity.purchasepriceitem.scalecurrency", "zh-CN", "等级货币", "等级货币（字典 accounting_currency_code，DictValue=CNY/USD 等）"),
            // entity.purchasepriceitem.scalecurrency
            new TranslationSeedItem("entity.purchasepriceitem.scalecurrency", "zh-HK", "等级货币_hk", "等级货币（字典 accounting_currency_code，DictValue=CNY/USD 等）"),

            // entity.purchasepriceitem.calculationtype
            new TranslationSeedItem("entity.purchasepriceitem.calculationtype", "en-US", "计算类型_us", "计算类型（字典 logistics_calculation_type；SAP KRECH；默认 A=百分数）"),
            // entity.purchasepriceitem.calculationtype
            new TranslationSeedItem("entity.purchasepriceitem.calculationtype", "ja-JP", "计算类型_jp", "计算类型（字典 logistics_calculation_type；SAP KRECH；默认 A=百分数）"),
            // entity.purchasepriceitem.calculationtype
            new TranslationSeedItem("entity.purchasepriceitem.calculationtype", "zh-CN", "计算类型", "计算类型（字典 logistics_calculation_type；SAP KRECH；默认 A=百分数）"),
            // entity.purchasepriceitem.calculationtype
            new TranslationSeedItem("entity.purchasepriceitem.calculationtype", "zh-HK", "计算类型_hk", "计算类型（字典 logistics_calculation_type；SAP KRECH；默认 A=百分数）"),

            // entity.purchasepriceitem.price
            new TranslationSeedItem("entity.purchasepriceitem.price", "en-US", "价格_us", "价格"),
            // entity.purchasepriceitem.price
            new TranslationSeedItem("entity.purchasepriceitem.price", "ja-JP", "价格_jp", "价格"),
            // entity.purchasepriceitem.price
            new TranslationSeedItem("entity.purchasepriceitem.price", "zh-CN", "价格", "价格"),
            // entity.purchasepriceitem.price
            new TranslationSeedItem("entity.purchasepriceitem.price", "zh-HK", "价格_hk", "价格"),

            // entity.purchasepriceitem.taxcode
            new TranslationSeedItem("entity.purchasepriceitem.taxcode", "en-US", "税码_us", "税码（字典 accounting_tax_code，DictValue=J0/J1/J2…；SAP MWSKZ）"),
            // entity.purchasepriceitem.taxcode
            new TranslationSeedItem("entity.purchasepriceitem.taxcode", "ja-JP", "税码_jp", "税码（字典 accounting_tax_code，DictValue=J0/J1/J2…；SAP MWSKZ）"),
            // entity.purchasepriceitem.taxcode
            new TranslationSeedItem("entity.purchasepriceitem.taxcode", "zh-CN", "税码", "税码（字典 accounting_tax_code，DictValue=J0/J1/J2…；SAP MWSKZ）"),
            // entity.purchasepriceitem.taxcode
            new TranslationSeedItem("entity.purchasepriceitem.taxcode", "zh-HK", "税码_hk", "税码（字典 accounting_tax_code，DictValue=J0/J1/J2…；SAP MWSKZ）"),

            // entity.purchasepriceitem.isobsolete
            new TranslationSeedItem("entity.purchasepriceitem.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）"),
            // entity.purchasepriceitem.isobsolete
            new TranslationSeedItem("entity.purchasepriceitem.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）"),
            // entity.purchasepriceitem.isobsolete
            new TranslationSeedItem("entity.purchasepriceitem.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）"),
            // entity.purchasepriceitem.isobsolete
            new TranslationSeedItem("entity.purchasepriceitem.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）"),

            // entity.purchasepriceitem.scalequantities
            new TranslationSeedItem("entity.purchasepriceitem.scalequantities", "en-US", "数量等级行列表_us", "数量等级行列表（SAP KONM；主子表关系）"),
            // entity.purchasepriceitem.scalequantities
            new TranslationSeedItem("entity.purchasepriceitem.scalequantities", "ja-JP", "数量等级行列表_jp", "数量等级行列表（SAP KONM；主子表关系）"),
            // entity.purchasepriceitem.scalequantities
            new TranslationSeedItem("entity.purchasepriceitem.scalequantities", "zh-CN", "数量等级行列表", "数量等级行列表（SAP KONM；主子表关系）"),
            // entity.purchasepriceitem.scalequantities
            new TranslationSeedItem("entity.purchasepriceitem.scalequantities", "zh-HK", "数量等级行列表_hk", "数量等级行列表（SAP KONM；主子表关系）"),

            // entity.purchasepriceitem.scalevalues
            new TranslationSeedItem("entity.purchasepriceitem.scalevalues", "en-US", "价值等级行列表_us", "价值等级行列表（SAP KONW；主子表关系）"),
            // entity.purchasepriceitem.scalevalues
            new TranslationSeedItem("entity.purchasepriceitem.scalevalues", "ja-JP", "价值等级行列表_jp", "价值等级行列表（SAP KONW；主子表关系）"),
            // entity.purchasepriceitem.scalevalues
            new TranslationSeedItem("entity.purchasepriceitem.scalevalues", "zh-CN", "价值等级行列表", "价值等级行列表（SAP KONW；主子表关系）"),
            // entity.purchasepriceitem.scalevalues
            new TranslationSeedItem("entity.purchasepriceitem.scalevalues", "zh-HK", "价值等级行列表_hk", "价值等级行列表（SAP KONW；主子表关系）"),
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
