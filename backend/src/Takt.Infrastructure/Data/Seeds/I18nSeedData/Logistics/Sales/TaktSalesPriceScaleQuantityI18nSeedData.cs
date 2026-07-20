// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Sales
// 文件名称：TaktSalesPriceScaleQuantityI18nSeedData.cs
// 创建时间：2026-07-20
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSalesPriceScaleQuantity 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktSalesPriceScaleQuantity 实体国际化翻译种子（键前缀 entity.salespricescalequantity.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSalesPriceScaleQuantityI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSalesPriceScaleQuantity 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 salespricescalequantity 实体翻译...", tenantCode);

        foreach (var item in GetSalesPriceScaleQuantityTranslations())
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

        TaktLogger.Information("TaktSalesPriceScaleQuantity 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSalesPriceScaleQuantity 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.salespricescalequantity._self / entity.salespricescalequantity.{{field}}；ResourceGroup=Sales；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSalesPriceScaleQuantityTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.salespricescalequantity._self
            new TranslationSeedItem("entity.salespricescalequantity._self", "en-US", "Sales Price Scale Quantity Information_us", "实体名称"),
            // entity.salespricescalequantity._self
            new TranslationSeedItem("entity.salespricescalequantity._self", "ja-JP", "Takt销售价格数量等级信息_jp", "实体名称"),
            // entity.salespricescalequantity._self
            new TranslationSeedItem("entity.salespricescalequantity._self", "zh-CN", "Takt销售价格数量等级信息", "实体名称"),
            // entity.salespricescalequantity._self
            new TranslationSeedItem("entity.salespricescalequantity._self", "zh-HK", "Takt销售价格数量等级信息_hk", "实体名称"),

            // entity.salespricescalequantity.salespriceitemid
            new TranslationSeedItem("entity.salespricescalequantity.salespriceitemid", "en-US", "销售价格明细ID_us", "销售价格明细 ID（主子表关系；选项 TaktSalesPriceItems/options，DictValue=Id）"),
            // entity.salespricescalequantity.salespriceitemid
            new TranslationSeedItem("entity.salespricescalequantity.salespriceitemid", "ja-JP", "销售价格明细ID_jp", "销售价格明细 ID（主子表关系；选项 TaktSalesPriceItems/options，DictValue=Id）"),
            // entity.salespricescalequantity.salespriceitemid
            new TranslationSeedItem("entity.salespricescalequantity.salespriceitemid", "zh-CN", "销售价格明细ID", "销售价格明细 ID（主子表关系；选项 TaktSalesPriceItems/options，DictValue=Id）"),
            // entity.salespricescalequantity.salespriceitemid
            new TranslationSeedItem("entity.salespricescalequantity.salespriceitemid", "zh-HK", "销售价格明细ID_hk", "销售价格明细 ID（主子表关系；选项 TaktSalesPriceItems/options，DictValue=Id）"),

            // entity.salespricescalequantity.salespricecode
            new TranslationSeedItem("entity.salespricescalequantity.salespricecode", "en-US", "定价记录号_us", "定价记录号（KNUMH；冗余；与主表/明细 SalesPriceCode 一致，长度 20）"),
            // entity.salespricescalequantity.salespricecode
            new TranslationSeedItem("entity.salespricescalequantity.salespricecode", "ja-JP", "定价记录号_jp", "定价记录号（KNUMH；冗余；与主表/明细 SalesPriceCode 一致，长度 20）"),
            // entity.salespricescalequantity.salespricecode
            new TranslationSeedItem("entity.salespricescalequantity.salespricecode", "zh-CN", "定价记录号", "定价记录号（KNUMH；冗余；与主表/明细 SalesPriceCode 一致，长度 20）"),
            // entity.salespricescalequantity.salespricecode
            new TranslationSeedItem("entity.salespricescalequantity.salespricecode", "zh-HK", "定价记录号_hk", "定价记录号（KNUMH；冗余；与主表/明细 SalesPriceCode 一致，长度 20）"),

            // entity.salespricescalequantity.salespriceseq
            new TranslationSeedItem("entity.salespricescalequantity.salespriceseq", "en-US", "条件序列号_us", "条件序列号（KOPOS；冗余；与明细 SalesPriceSeq 一致）"),
            // entity.salespricescalequantity.salespriceseq
            new TranslationSeedItem("entity.salespricescalequantity.salespriceseq", "ja-JP", "条件序列号_jp", "条件序列号（KOPOS；冗余；与明细 SalesPriceSeq 一致）"),
            // entity.salespricescalequantity.salespriceseq
            new TranslationSeedItem("entity.salespricescalequantity.salespriceseq", "zh-CN", "条件序列号", "条件序列号（KOPOS；冗余；与明细 SalesPriceSeq 一致）"),
            // entity.salespricescalequantity.salespriceseq
            new TranslationSeedItem("entity.salespricescalequantity.salespriceseq", "zh-HK", "条件序列号_hk", "条件序列号（KOPOS；冗余；与明细 SalesPriceSeq 一致）"),

            // entity.salespricescalequantity.linenumber
            new TranslationSeedItem("entity.salespricescalequantity.linenumber", "en-US", "行号_us", "行号（KLFN1；阶梯行序号，固定步长=10）"),
            // entity.salespricescalequantity.linenumber
            new TranslationSeedItem("entity.salespricescalequantity.linenumber", "ja-JP", "行号_jp", "行号（KLFN1；阶梯行序号，固定步长=10）"),
            // entity.salespricescalequantity.linenumber
            new TranslationSeedItem("entity.salespricescalequantity.linenumber", "zh-CN", "行号", "行号（KLFN1；阶梯行序号，固定步长=10）"),
            // entity.salespricescalequantity.linenumber
            new TranslationSeedItem("entity.salespricescalequantity.linenumber", "zh-HK", "行号_hk", "行号（KLFN1；阶梯行序号，固定步长=10）"),

            // entity.salespricescalequantity.scalequantity
            new TranslationSeedItem("entity.salespricescalequantity.scalequantity", "en-US", "等级数量_us", "等级数量（KSTBM；数量等级门槛；对应价值等级表的 ScaleValue）"),
            // entity.salespricescalequantity.scalequantity
            new TranslationSeedItem("entity.salespricescalequantity.scalequantity", "ja-JP", "等级数量_jp", "等级数量（KSTBM；数量等级门槛；对应价值等级表的 ScaleValue）"),
            // entity.salespricescalequantity.scalequantity
            new TranslationSeedItem("entity.salespricescalequantity.scalequantity", "zh-CN", "等级数量", "等级数量（KSTBM；数量等级门槛；对应价值等级表的 ScaleValue）"),
            // entity.salespricescalequantity.scalequantity
            new TranslationSeedItem("entity.salespricescalequantity.scalequantity", "zh-HK", "等级数量_hk", "等级数量（KSTBM；数量等级门槛；对应价值等级表的 ScaleValue）"),

            // entity.salespricescalequantity.amount
            new TranslationSeedItem("entity.salespricescalequantity.amount", "en-US", "金额_us", "金额（KBETR）"),
            // entity.salespricescalequantity.amount
            new TranslationSeedItem("entity.salespricescalequantity.amount", "ja-JP", "金额_jp", "金额（KBETR）"),
            // entity.salespricescalequantity.amount
            new TranslationSeedItem("entity.salespricescalequantity.amount", "zh-CN", "金额", "金额（KBETR）"),
            // entity.salespricescalequantity.amount
            new TranslationSeedItem("entity.salespricescalequantity.amount", "zh-HK", "金额_hk", "金额（KBETR）"),

            // entity.salespricescalequantity.isobsolete
            new TranslationSeedItem("entity.salespricescalequantity.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）"),
            // entity.salespricescalequantity.isobsolete
            new TranslationSeedItem("entity.salespricescalequantity.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）"),
            // entity.salespricescalequantity.isobsolete
            new TranslationSeedItem("entity.salespricescalequantity.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）"),
            // entity.salespricescalequantity.isobsolete
            new TranslationSeedItem("entity.salespricescalequantity.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）"),
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
        translation.ResourceGroup = "Sales";
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
