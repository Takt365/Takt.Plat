// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Procurement
// 文件名称：TaktPurchasePriceI18nSeedData.cs
// 创建时间：2026-07-02
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktPurchasePrice 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktPurchasePrice 实体国际化翻译种子（键前缀 entity.purchaseprice.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktPurchasePriceI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktPurchasePrice 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 purchaseprice 实体翻译...", tenantCode);

        foreach (var item in GetPurchasePriceTranslations())
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

        TaktLogger.Information("TaktPurchasePrice 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktPurchasePrice 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.purchaseprice._self / entity.purchaseprice.{{field}}；ResourceGroup=Procurement；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetPurchasePriceTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.purchaseprice._self
            new TranslationSeedItem("entity.purchaseprice._self", "en-US", "Purchase Price Information_us", "实体名称"),
            // entity.purchaseprice._self
            new TranslationSeedItem("entity.purchaseprice._self", "ja-JP", "Takt采购价格信息_jp", "实体名称"),
            // entity.purchaseprice._self
            new TranslationSeedItem("entity.purchaseprice._self", "zh-CN", "Takt采购价格信息", "实体名称"),
            // entity.purchaseprice._self
            new TranslationSeedItem("entity.purchaseprice._self", "zh-HK", "Takt采购价格信息_hk", "实体名称"),

            // entity.purchaseprice.plantcode
            new TranslationSeedItem("entity.purchaseprice.plantcode", "en-US", "工厂代码_us", "工厂代码（选项 TaktPlants/options，DictValue=PlantCode）"),
            // entity.purchaseprice.plantcode
            new TranslationSeedItem("entity.purchaseprice.plantcode", "ja-JP", "工厂代码_jp", "工厂代码（选项 TaktPlants/options，DictValue=PlantCode）"),
            // entity.purchaseprice.plantcode
            new TranslationSeedItem("entity.purchaseprice.plantcode", "zh-CN", "工厂代码", "工厂代码（选项 TaktPlants/options，DictValue=PlantCode）"),
            // entity.purchaseprice.plantcode
            new TranslationSeedItem("entity.purchaseprice.plantcode", "zh-HK", "工厂代码_hk", "工厂代码（选项 TaktPlants/options，DictValue=PlantCode）"),

            // entity.purchaseprice.code
            new TranslationSeedItem("entity.purchaseprice.code", "en-US", "采购价格编码_us", "采购价格编码（唯一索引）"),
            // entity.purchaseprice.code
            new TranslationSeedItem("entity.purchaseprice.code", "ja-JP", "采购价格编码_jp", "采购价格编码（唯一索引）"),
            // entity.purchaseprice.code
            new TranslationSeedItem("entity.purchaseprice.code", "zh-CN", "采购价格编码", "采购价格编码（唯一索引）"),
            // entity.purchaseprice.code
            new TranslationSeedItem("entity.purchaseprice.code", "zh-HK", "采购价格编码_hk", "采购价格编码（唯一索引）"),

            // entity.purchaseprice.suppliercode
            new TranslationSeedItem("entity.purchaseprice.suppliercode", "en-US", "供应商编码_us", "供应商编码（选项 TaktSuppliers/options，DictValue=SupplierCode）"),
            // entity.purchaseprice.suppliercode
            new TranslationSeedItem("entity.purchaseprice.suppliercode", "ja-JP", "供应商编码_jp", "供应商编码（选项 TaktSuppliers/options，DictValue=SupplierCode）"),
            // entity.purchaseprice.suppliercode
            new TranslationSeedItem("entity.purchaseprice.suppliercode", "zh-CN", "供应商编码", "供应商编码（选项 TaktSuppliers/options，DictValue=SupplierCode）"),
            // entity.purchaseprice.suppliercode
            new TranslationSeedItem("entity.purchaseprice.suppliercode", "zh-HK", "供应商编码_hk", "供应商编码（选项 TaktSuppliers/options，DictValue=SupplierCode）"),

            // entity.purchaseprice.purchaseinquiryid
            new TranslationSeedItem("entity.purchaseprice.purchaseinquiryid", "en-US", "来源采购询价ID_us", "来源采购询价 ID（关联 TaktPurchaseInquiry.Id，选项 TaktPurchaseInquirys/options）"),
            // entity.purchaseprice.purchaseinquiryid
            new TranslationSeedItem("entity.purchaseprice.purchaseinquiryid", "ja-JP", "来源采购询价ID_jp", "来源采购询价 ID（关联 TaktPurchaseInquiry.Id，选项 TaktPurchaseInquirys/options）"),
            // entity.purchaseprice.purchaseinquiryid
            new TranslationSeedItem("entity.purchaseprice.purchaseinquiryid", "zh-CN", "来源采购询价ID", "来源采购询价 ID（关联 TaktPurchaseInquiry.Id，选项 TaktPurchaseInquirys/options）"),
            // entity.purchaseprice.purchaseinquiryid
            new TranslationSeedItem("entity.purchaseprice.purchaseinquiryid", "zh-HK", "来源采购询价ID_hk", "来源采购询价 ID（关联 TaktPurchaseInquiry.Id，选项 TaktPurchaseInquirys/options）"),

            // entity.purchaseprice.purchaseinquirycode
            new TranslationSeedItem("entity.purchaseprice.purchaseinquirycode", "en-US", "来源采购询价编码_us", "来源采购询价编码（冗余）"),
            // entity.purchaseprice.purchaseinquirycode
            new TranslationSeedItem("entity.purchaseprice.purchaseinquirycode", "ja-JP", "来源采购询价编码_jp", "来源采购询价编码（冗余）"),
            // entity.purchaseprice.purchaseinquirycode
            new TranslationSeedItem("entity.purchaseprice.purchaseinquirycode", "zh-CN", "来源采购询价编码", "来源采购询价编码（冗余）"),
            // entity.purchaseprice.purchaseinquirycode
            new TranslationSeedItem("entity.purchaseprice.purchaseinquirycode", "zh-HK", "来源采购询价编码_hk", "来源采购询价编码（冗余）"),

            // entity.purchaseprice.pricetype
            new TranslationSeedItem("entity.purchaseprice.pricetype", "en-US", "价格类型_us", "价格类型（字典 logistics_price_type；0=标准价格，1=合同价格，2=临时价格，3=询价价格，4=历史价格）"),
            // entity.purchaseprice.pricetype
            new TranslationSeedItem("entity.purchaseprice.pricetype", "ja-JP", "价格类型_jp", "价格类型（字典 logistics_price_type；0=标准价格，1=合同价格，2=临时价格，3=询价价格，4=历史价格）"),
            // entity.purchaseprice.pricetype
            new TranslationSeedItem("entity.purchaseprice.pricetype", "zh-CN", "价格类型", "价格类型（字典 logistics_price_type；0=标准价格，1=合同价格，2=临时价格，3=询价价格，4=历史价格）"),
            // entity.purchaseprice.pricetype
            new TranslationSeedItem("entity.purchaseprice.pricetype", "zh-HK", "价格类型_hk", "价格类型（字典 logistics_price_type；0=标准价格，1=合同价格，2=临时价格，3=询价价格，4=历史价格）"),

            // entity.purchaseprice.effectivestartdate
            new TranslationSeedItem("entity.purchaseprice.effectivestartdate", "en-US", "生效日期_us", "生效日期"),
            // entity.purchaseprice.effectivestartdate
            new TranslationSeedItem("entity.purchaseprice.effectivestartdate", "ja-JP", "生效日期_jp", "生效日期"),
            // entity.purchaseprice.effectivestartdate
            new TranslationSeedItem("entity.purchaseprice.effectivestartdate", "zh-CN", "生效日期", "生效日期"),
            // entity.purchaseprice.effectivestartdate
            new TranslationSeedItem("entity.purchaseprice.effectivestartdate", "zh-HK", "生效日期_hk", "生效日期"),

            // entity.purchaseprice.effectiveenddate
            new TranslationSeedItem("entity.purchaseprice.effectiveenddate", "en-US", "失效日期_us", "失效日期（空表示长期有效）"),
            // entity.purchaseprice.effectiveenddate
            new TranslationSeedItem("entity.purchaseprice.effectiveenddate", "ja-JP", "失效日期_jp", "失效日期（空表示长期有效）"),
            // entity.purchaseprice.effectiveenddate
            new TranslationSeedItem("entity.purchaseprice.effectiveenddate", "zh-CN", "失效日期", "失效日期（空表示长期有效）"),
            // entity.purchaseprice.effectiveenddate
            new TranslationSeedItem("entity.purchaseprice.effectiveenddate", "zh-HK", "失效日期_hk", "失效日期（空表示长期有效）"),

            // entity.purchaseprice.pricestatus
            new TranslationSeedItem("entity.purchaseprice.pricestatus", "en-US", "价格状态_us", "价格状态（字典 sys_normal_disable_status；1=启用，0=禁用）"),
            // entity.purchaseprice.pricestatus
            new TranslationSeedItem("entity.purchaseprice.pricestatus", "ja-JP", "价格状态_jp", "价格状态（字典 sys_normal_disable_status；1=启用，0=禁用）"),
            // entity.purchaseprice.pricestatus
            new TranslationSeedItem("entity.purchaseprice.pricestatus", "zh-CN", "价格状态", "价格状态（字典 sys_normal_disable_status；1=启用，0=禁用）"),
            // entity.purchaseprice.pricestatus
            new TranslationSeedItem("entity.purchaseprice.pricestatus", "zh-HK", "价格状态_hk", "价格状态（字典 sys_normal_disable_status；1=启用，0=禁用）"),

            // entity.purchaseprice.items
            new TranslationSeedItem("entity.purchaseprice.items", "en-US", "物料价格明细列表_us", "物料价格明细列表（主子表关系，一个供应商价格可以有多个物料价格）"),
            // entity.purchaseprice.items
            new TranslationSeedItem("entity.purchaseprice.items", "ja-JP", "物料价格明细列表_jp", "物料价格明细列表（主子表关系，一个供应商价格可以有多个物料价格）"),
            // entity.purchaseprice.items
            new TranslationSeedItem("entity.purchaseprice.items", "zh-CN", "物料价格明细列表", "物料价格明细列表（主子表关系，一个供应商价格可以有多个物料价格）"),
            // entity.purchaseprice.items
            new TranslationSeedItem("entity.purchaseprice.items", "zh-HK", "物料价格明细列表_hk", "物料价格明细列表（主子表关系，一个供应商价格可以有多个物料价格）"),

            // entity.purchaseprice.changelogs
            new TranslationSeedItem("entity.purchaseprice.changelogs", "en-US", "采购价格变更记录列表_us", "采购价格变更记录列表（外键在子表 TaktPurchasePriceChangeLog.PurchasePriceId）"),
            // entity.purchaseprice.changelogs
            new TranslationSeedItem("entity.purchaseprice.changelogs", "ja-JP", "采购价格变更记录列表_jp", "采购价格变更记录列表（外键在子表 TaktPurchasePriceChangeLog.PurchasePriceId）"),
            // entity.purchaseprice.changelogs
            new TranslationSeedItem("entity.purchaseprice.changelogs", "zh-CN", "采购价格变更记录列表", "采购价格变更记录列表（外键在子表 TaktPurchasePriceChangeLog.PurchasePriceId）"),
            // entity.purchaseprice.changelogs
            new TranslationSeedItem("entity.purchaseprice.changelogs", "zh-HK", "采购价格变更记录列表_hk", "采购价格变更记录列表（外键在子表 TaktPurchasePriceChangeLog.PurchasePriceId）"),
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
