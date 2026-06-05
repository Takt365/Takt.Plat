// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Materials
// 文件名称：TaktPurchasePriceI18nSeedData.cs
// 创建时间：2026-06-05
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
using Takt.Shared.Enums;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Materials;

/// <summary>
/// TaktPurchasePrice 实体国际化翻译种子（键前缀 entity.purchasePrice.*）
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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 purchasePrice 实体翻译...", tenantCode);

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
    /// I18nKey：entity.purchasePrice._self / entity.purchasePrice.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetPurchasePriceTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.purchasePrice._self
            new TranslationSeedItem("entity.purchasePrice._self", "en-US", "Purchase Price Information", "实体名称"),
            // entity.purchasePrice._self
            new TranslationSeedItem("entity.purchasePrice._self", "ja-JP", "Takt采购价格信息", "实体名称"),
            // entity.purchasePrice._self
            new TranslationSeedItem("entity.purchasePrice._self", "zh-CN", "Takt采购价格信息", "实体名称"),
            // entity.purchasePrice._self
            new TranslationSeedItem("entity.purchasePrice._self", "zh-HK", "Takt采购价格信息", "实体名称"),

            // entity.purchasePrice.plantcode
            new TranslationSeedItem("entity.purchasePrice.plantcode", "en-US", "工厂代码", "工厂代码（不可空）"),
            // entity.purchasePrice.plantcode
            new TranslationSeedItem("entity.purchasePrice.plantcode", "ja-JP", "工厂代码", "工厂代码（不可空）"),
            // entity.purchasePrice.plantcode
            new TranslationSeedItem("entity.purchasePrice.plantcode", "zh-CN", "工厂代码", "工厂代码（不可空）"),
            // entity.purchasePrice.plantcode
            new TranslationSeedItem("entity.purchasePrice.plantcode", "zh-HK", "工厂代码", "工厂代码（不可空）"),

            // entity.purchasePrice.code
            new TranslationSeedItem("entity.purchasePrice.code", "en-US", "采购价格编码", "采购价格编码（唯一索引）"),
            // entity.purchasePrice.code
            new TranslationSeedItem("entity.purchasePrice.code", "ja-JP", "采购价格编码", "采购价格编码（唯一索引）"),
            // entity.purchasePrice.code
            new TranslationSeedItem("entity.purchasePrice.code", "zh-CN", "采购价格编码", "采购价格编码（唯一索引）"),
            // entity.purchasePrice.code
            new TranslationSeedItem("entity.purchasePrice.code", "zh-HK", "采购价格编码", "采购价格编码（唯一索引）"),

            // entity.purchasePrice.suppliercode
            new TranslationSeedItem("entity.purchasePrice.suppliercode", "en-US", "供应商编码", "供应商编码"),
            // entity.purchasePrice.suppliercode
            new TranslationSeedItem("entity.purchasePrice.suppliercode", "ja-JP", "供应商编码", "供应商编码"),
            // entity.purchasePrice.suppliercode
            new TranslationSeedItem("entity.purchasePrice.suppliercode", "zh-CN", "供应商编码", "供应商编码"),
            // entity.purchasePrice.suppliercode
            new TranslationSeedItem("entity.purchasePrice.suppliercode", "zh-HK", "供应商编码", "供应商编码"),

            // entity.purchasePrice.pricetype
            new TranslationSeedItem("entity.purchasePrice.pricetype", "en-US", "价格类型", "价格类型（0=标准价格，1=合同价格，2=临时价格，3=询价价格，4=历史价格）"),
            // entity.purchasePrice.pricetype
            new TranslationSeedItem("entity.purchasePrice.pricetype", "ja-JP", "价格类型", "价格类型（0=标准价格，1=合同价格，2=临时价格，3=询价价格，4=历史价格）"),
            // entity.purchasePrice.pricetype
            new TranslationSeedItem("entity.purchasePrice.pricetype", "zh-CN", "价格类型", "价格类型（0=标准价格，1=合同价格，2=临时价格，3=询价价格，4=历史价格）"),
            // entity.purchasePrice.pricetype
            new TranslationSeedItem("entity.purchasePrice.pricetype", "zh-HK", "价格类型", "价格类型（0=标准价格，1=合同价格，2=临时价格，3=询价价格，4=历史价格）"),

            // entity.purchasePrice.effectivestartdate
            new TranslationSeedItem("entity.purchasePrice.effectivestartdate", "en-US", "生效日期", "生效日期"),
            // entity.purchasePrice.effectivestartdate
            new TranslationSeedItem("entity.purchasePrice.effectivestartdate", "ja-JP", "生效日期", "生效日期"),
            // entity.purchasePrice.effectivestartdate
            new TranslationSeedItem("entity.purchasePrice.effectivestartdate", "zh-CN", "生效日期", "生效日期"),
            // entity.purchasePrice.effectivestartdate
            new TranslationSeedItem("entity.purchasePrice.effectivestartdate", "zh-HK", "生效日期", "生效日期"),

            // entity.purchasePrice.effectiveenddate
            new TranslationSeedItem("entity.purchasePrice.effectiveenddate", "en-US", "失效日期", "失效日期（空表示长期有效）"),
            // entity.purchasePrice.effectiveenddate
            new TranslationSeedItem("entity.purchasePrice.effectiveenddate", "ja-JP", "失效日期", "失效日期（空表示长期有效）"),
            // entity.purchasePrice.effectiveenddate
            new TranslationSeedItem("entity.purchasePrice.effectiveenddate", "zh-CN", "失效日期", "失效日期（空表示长期有效）"),
            // entity.purchasePrice.effectiveenddate
            new TranslationSeedItem("entity.purchasePrice.effectiveenddate", "zh-HK", "失效日期", "失效日期（空表示长期有效）"),

            // entity.purchasePrice.pricestatus
            new TranslationSeedItem("entity.purchasePrice.pricestatus", "en-US", "价格状态", "价格状态（1=启用，0=禁用）"),
            // entity.purchasePrice.pricestatus
            new TranslationSeedItem("entity.purchasePrice.pricestatus", "ja-JP", "价格状态", "价格状态（1=启用，0=禁用）"),
            // entity.purchasePrice.pricestatus
            new TranslationSeedItem("entity.purchasePrice.pricestatus", "zh-CN", "价格状态", "价格状态（1=启用，0=禁用）"),
            // entity.purchasePrice.pricestatus
            new TranslationSeedItem("entity.purchasePrice.pricestatus", "zh-HK", "价格状态", "价格状态（1=启用，0=禁用）"),

            // entity.purchasePrice.items
            new TranslationSeedItem("entity.purchasePrice.items", "en-US", "items", "物料价格明细列表（主子表关系，一个供应商价格可以有多个物料价格）"),
            // entity.purchasePrice.items
            new TranslationSeedItem("entity.purchasePrice.items", "ja-JP", "items", "物料价格明细列表（主子表关系，一个供应商价格可以有多个物料价格）"),
            // entity.purchasePrice.items
            new TranslationSeedItem("entity.purchasePrice.items", "zh-CN", "items", "物料价格明细列表（主子表关系，一个供应商价格可以有多个物料价格）"),
            // entity.purchasePrice.items
            new TranslationSeedItem("entity.purchasePrice.items", "zh-HK", "items", "物料价格明细列表（主子表关系，一个供应商价格可以有多个物料价格）"),
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
