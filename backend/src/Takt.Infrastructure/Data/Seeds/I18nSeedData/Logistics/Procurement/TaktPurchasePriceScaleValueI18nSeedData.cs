// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Procurement
// 文件名称：TaktPurchasePriceScaleValueI18nSeedData.cs
// 创建时间：2026-07-20
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktPurchasePriceScaleValue 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktPurchasePriceScaleValue 实体国际化翻译种子（键前缀 entity.purchasepricescalevalue.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktPurchasePriceScaleValueI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktPurchasePriceScaleValue 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 purchasepricescalevalue 实体翻译...", tenantCode);

        foreach (var item in GetPurchasePriceScaleValueTranslations())
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

        TaktLogger.Information("TaktPurchasePriceScaleValue 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktPurchasePriceScaleValue 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.purchasepricescalevalue._self / entity.purchasepricescalevalue.{{field}}；ResourceGroup=Procurement；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetPurchasePriceScaleValueTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.purchasepricescalevalue._self
            new TranslationSeedItem("entity.purchasepricescalevalue._self", "en-US", "Purchase Price Scale Value Information_us", "实体名称"),
            // entity.purchasepricescalevalue._self
            new TranslationSeedItem("entity.purchasepricescalevalue._self", "ja-JP", "Takt采购价格价值等级信息_jp", "实体名称"),
            // entity.purchasepricescalevalue._self
            new TranslationSeedItem("entity.purchasepricescalevalue._self", "zh-CN", "Takt采购价格价值等级信息", "实体名称"),
            // entity.purchasepricescalevalue._self
            new TranslationSeedItem("entity.purchasepricescalevalue._self", "zh-HK", "Takt采购价格价值等级信息_hk", "实体名称"),

            // entity.purchasepricescalevalue.purchasepriceitemid
            new TranslationSeedItem("entity.purchasepricescalevalue.purchasepriceitemid", "en-US", "采购价格明细ID_us", "采购价格明细 ID（主子表关系；选项 TaktPurchasePriceItems/options，DictValue=Id）"),
            // entity.purchasepricescalevalue.purchasepriceitemid
            new TranslationSeedItem("entity.purchasepricescalevalue.purchasepriceitemid", "ja-JP", "采购价格明细ID_jp", "采购价格明细 ID（主子表关系；选项 TaktPurchasePriceItems/options，DictValue=Id）"),
            // entity.purchasepricescalevalue.purchasepriceitemid
            new TranslationSeedItem("entity.purchasepricescalevalue.purchasepriceitemid", "zh-CN", "采购价格明细ID", "采购价格明细 ID（主子表关系；选项 TaktPurchasePriceItems/options，DictValue=Id）"),
            // entity.purchasepricescalevalue.purchasepriceitemid
            new TranslationSeedItem("entity.purchasepricescalevalue.purchasepriceitemid", "zh-HK", "采购价格明细ID_hk", "采购价格明细 ID（主子表关系；选项 TaktPurchasePriceItems/options，DictValue=Id）"),

            // entity.purchasepricescalevalue.purchasepricecode
            new TranslationSeedItem("entity.purchasepricescalevalue.purchasepricecode", "en-US", "定价记录号_us", "定价记录号（KNUMH；冗余；与主表/明细 PurchasePriceCode 一致，长度 20）"),
            // entity.purchasepricescalevalue.purchasepricecode
            new TranslationSeedItem("entity.purchasepricescalevalue.purchasepricecode", "ja-JP", "定价记录号_jp", "定价记录号（KNUMH；冗余；与主表/明细 PurchasePriceCode 一致，长度 20）"),
            // entity.purchasepricescalevalue.purchasepricecode
            new TranslationSeedItem("entity.purchasepricescalevalue.purchasepricecode", "zh-CN", "定价记录号", "定价记录号（KNUMH；冗余；与主表/明细 PurchasePriceCode 一致，长度 20）"),
            // entity.purchasepricescalevalue.purchasepricecode
            new TranslationSeedItem("entity.purchasepricescalevalue.purchasepricecode", "zh-HK", "定价记录号_hk", "定价记录号（KNUMH；冗余；与主表/明细 PurchasePriceCode 一致，长度 20）"),

            // entity.purchasepricescalevalue.purchasepriceseq
            new TranslationSeedItem("entity.purchasepricescalevalue.purchasepriceseq", "en-US", "条件序列号_us", "条件序列号（KOPOS；冗余；与明细 PurchasePriceSeq 一致）"),
            // entity.purchasepricescalevalue.purchasepriceseq
            new TranslationSeedItem("entity.purchasepricescalevalue.purchasepriceseq", "ja-JP", "条件序列号_jp", "条件序列号（KOPOS；冗余；与明细 PurchasePriceSeq 一致）"),
            // entity.purchasepricescalevalue.purchasepriceseq
            new TranslationSeedItem("entity.purchasepricescalevalue.purchasepriceseq", "zh-CN", "条件序列号", "条件序列号（KOPOS；冗余；与明细 PurchasePriceSeq 一致）"),
            // entity.purchasepricescalevalue.purchasepriceseq
            new TranslationSeedItem("entity.purchasepricescalevalue.purchasepriceseq", "zh-HK", "条件序列号_hk", "条件序列号（KOPOS；冗余；与明细 PurchasePriceSeq 一致）"),

            // entity.purchasepricescalevalue.linenumber
            new TranslationSeedItem("entity.purchasepricescalevalue.linenumber", "en-US", "行号_us", "行号（KLFN1；阶梯行序号，固定步长=10）"),
            // entity.purchasepricescalevalue.linenumber
            new TranslationSeedItem("entity.purchasepricescalevalue.linenumber", "ja-JP", "行号_jp", "行号（KLFN1；阶梯行序号，固定步长=10）"),
            // entity.purchasepricescalevalue.linenumber
            new TranslationSeedItem("entity.purchasepricescalevalue.linenumber", "zh-CN", "行号", "行号（KLFN1；阶梯行序号，固定步长=10）"),
            // entity.purchasepricescalevalue.linenumber
            new TranslationSeedItem("entity.purchasepricescalevalue.linenumber", "zh-HK", "行号_hk", "行号（KLFN1；阶梯行序号，固定步长=10）"),

            // entity.purchasepricescalevalue.scalevalue
            new TranslationSeedItem("entity.purchasepricescalevalue.scalevalue", "en-US", "等级值_us", "等级值（KSTBW；价值等级门槛；对应数量等级表的 ScaleQuantity）"),
            // entity.purchasepricescalevalue.scalevalue
            new TranslationSeedItem("entity.purchasepricescalevalue.scalevalue", "ja-JP", "等级值_jp", "等级值（KSTBW；价值等级门槛；对应数量等级表的 ScaleQuantity）"),
            // entity.purchasepricescalevalue.scalevalue
            new TranslationSeedItem("entity.purchasepricescalevalue.scalevalue", "zh-CN", "等级值", "等级值（KSTBW；价值等级门槛；对应数量等级表的 ScaleQuantity）"),
            // entity.purchasepricescalevalue.scalevalue
            new TranslationSeedItem("entity.purchasepricescalevalue.scalevalue", "zh-HK", "等级值_hk", "等级值（KSTBW；价值等级门槛；对应数量等级表的 ScaleQuantity）"),

            // entity.purchasepricescalevalue.amount
            new TranslationSeedItem("entity.purchasepricescalevalue.amount", "en-US", "金额_us", "金额（KBETR）"),
            // entity.purchasepricescalevalue.amount
            new TranslationSeedItem("entity.purchasepricescalevalue.amount", "ja-JP", "金额_jp", "金额（KBETR）"),
            // entity.purchasepricescalevalue.amount
            new TranslationSeedItem("entity.purchasepricescalevalue.amount", "zh-CN", "金额", "金额（KBETR）"),
            // entity.purchasepricescalevalue.amount
            new TranslationSeedItem("entity.purchasepricescalevalue.amount", "zh-HK", "金额_hk", "金额（KBETR）"),

            // entity.purchasepricescalevalue.isobsolete
            new TranslationSeedItem("entity.purchasepricescalevalue.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）"),
            // entity.purchasepricescalevalue.isobsolete
            new TranslationSeedItem("entity.purchasepricescalevalue.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）"),
            // entity.purchasepricescalevalue.isobsolete
            new TranslationSeedItem("entity.purchasepricescalevalue.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）"),
            // entity.purchasepricescalevalue.isobsolete
            new TranslationSeedItem("entity.purchasepricescalevalue.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）"),
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
