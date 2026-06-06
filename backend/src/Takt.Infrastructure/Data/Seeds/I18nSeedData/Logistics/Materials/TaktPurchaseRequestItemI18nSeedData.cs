// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Materials
// 文件名称：TaktPurchaseRequestItemI18nSeedData.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktPurchaseRequestItem 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktPurchaseRequestItem 实体国际化翻译种子（键前缀 entity.purchaseRequestItem.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktPurchaseRequestItemI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktPurchaseRequestItem 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 purchaseRequestItem 实体翻译...", tenantCode);

        foreach (var item in GetPurchaseRequestItemTranslations())
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

        TaktLogger.Information("TaktPurchaseRequestItem 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktPurchaseRequestItem 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.purchaseRequestItem._self / entity.purchaseRequestItem.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetPurchaseRequestItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.purchaseRequestItem._self
            new TranslationSeedItem("entity.purchaseRequestItem._self", "en-US", "Purchase Request Item Information", "实体名称"),
            // entity.purchaseRequestItem._self
            new TranslationSeedItem("entity.purchaseRequestItem._self", "ja-JP", "Takt采购申请明细信息", "实体名称"),
            // entity.purchaseRequestItem._self
            new TranslationSeedItem("entity.purchaseRequestItem._self", "zh-CN", "Takt采购申请明细信息", "实体名称"),
            // entity.purchaseRequestItem._self
            new TranslationSeedItem("entity.purchaseRequestItem._self", "zh-HK", "Takt采购申请明细信息", "实体名称"),

            // entity.purchaseRequestItem.purchaserequestid
            new TranslationSeedItem("entity.purchaseRequestItem.purchaserequestid", "en-US", "采购申请ID", "采购申请ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.purchaseRequestItem.purchaserequestid
            new TranslationSeedItem("entity.purchaseRequestItem.purchaserequestid", "ja-JP", "采购申请ID", "采购申请ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.purchaseRequestItem.purchaserequestid
            new TranslationSeedItem("entity.purchaseRequestItem.purchaserequestid", "zh-CN", "采购申请ID", "采购申请ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.purchaseRequestItem.purchaserequestid
            new TranslationSeedItem("entity.purchaseRequestItem.purchaserequestid", "zh-HK", "采购申请ID", "采购申请ID（主子表关系，序列化为string以避免Javascript精度问题）"),

            // entity.purchaseRequestItem.purchaserequestcode
            new TranslationSeedItem("entity.purchaseRequestItem.purchaserequestcode", "en-US", "采购申请编码", "采购申请编码（冗余字段，便于查询）"),
            // entity.purchaseRequestItem.purchaserequestcode
            new TranslationSeedItem("entity.purchaseRequestItem.purchaserequestcode", "ja-JP", "采购申请编码", "采购申请编码（冗余字段，便于查询）"),
            // entity.purchaseRequestItem.purchaserequestcode
            new TranslationSeedItem("entity.purchaseRequestItem.purchaserequestcode", "zh-CN", "采购申请编码", "采购申请编码（冗余字段，便于查询）"),
            // entity.purchaseRequestItem.purchaserequestcode
            new TranslationSeedItem("entity.purchaseRequestItem.purchaserequestcode", "zh-HK", "采购申请编码", "采购申请编码（冗余字段，便于查询）"),

            // entity.purchaseRequestItem.linenumber
            new TranslationSeedItem("entity.purchaseRequestItem.linenumber", "en-US", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.purchaseRequestItem.linenumber
            new TranslationSeedItem("entity.purchaseRequestItem.linenumber", "ja-JP", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.purchaseRequestItem.linenumber
            new TranslationSeedItem("entity.purchaseRequestItem.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.purchaseRequestItem.linenumber
            new TranslationSeedItem("entity.purchaseRequestItem.linenumber", "zh-HK", "行号", "行号（项号/序号，固定步长=10）"),

            // entity.purchaseRequestItem.materialcode
            new TranslationSeedItem("entity.purchaseRequestItem.materialcode", "en-US", "物料编码", "物料编码"),
            // entity.purchaseRequestItem.materialcode
            new TranslationSeedItem("entity.purchaseRequestItem.materialcode", "ja-JP", "物料编码", "物料编码"),
            // entity.purchaseRequestItem.materialcode
            new TranslationSeedItem("entity.purchaseRequestItem.materialcode", "zh-CN", "物料编码", "物料编码"),
            // entity.purchaseRequestItem.materialcode
            new TranslationSeedItem("entity.purchaseRequestItem.materialcode", "zh-HK", "物料编码", "物料编码"),

            // entity.purchaseRequestItem.materialname
            new TranslationSeedItem("entity.purchaseRequestItem.materialname", "en-US", "物料名称", "物料名称"),
            // entity.purchaseRequestItem.materialname
            new TranslationSeedItem("entity.purchaseRequestItem.materialname", "ja-JP", "物料名称", "物料名称"),
            // entity.purchaseRequestItem.materialname
            new TranslationSeedItem("entity.purchaseRequestItem.materialname", "zh-CN", "物料名称", "物料名称"),
            // entity.purchaseRequestItem.materialname
            new TranslationSeedItem("entity.purchaseRequestItem.materialname", "zh-HK", "物料名称", "物料名称"),

            // entity.purchaseRequestItem.materialspecification
            new TranslationSeedItem("entity.purchaseRequestItem.materialspecification", "en-US", "物料规格", "物料规格"),
            // entity.purchaseRequestItem.materialspecification
            new TranslationSeedItem("entity.purchaseRequestItem.materialspecification", "ja-JP", "物料规格", "物料规格"),
            // entity.purchaseRequestItem.materialspecification
            new TranslationSeedItem("entity.purchaseRequestItem.materialspecification", "zh-CN", "物料规格", "物料规格"),
            // entity.purchaseRequestItem.materialspecification
            new TranslationSeedItem("entity.purchaseRequestItem.materialspecification", "zh-HK", "物料规格", "物料规格"),

            // entity.purchaseRequestItem.requestunit
            new TranslationSeedItem("entity.purchaseRequestItem.requestunit", "en-US", "申请单位", "申请单位"),
            // entity.purchaseRequestItem.requestunit
            new TranslationSeedItem("entity.purchaseRequestItem.requestunit", "ja-JP", "申请单位", "申请单位"),
            // entity.purchaseRequestItem.requestunit
            new TranslationSeedItem("entity.purchaseRequestItem.requestunit", "zh-CN", "申请单位", "申请单位"),
            // entity.purchaseRequestItem.requestunit
            new TranslationSeedItem("entity.purchaseRequestItem.requestunit", "zh-HK", "申请单位", "申请单位"),

            // entity.purchaseRequestItem.requestquantity
            new TranslationSeedItem("entity.purchaseRequestItem.requestquantity", "en-US", "申请数量", "申请数量（基本单位数量）"),
            // entity.purchaseRequestItem.requestquantity
            new TranslationSeedItem("entity.purchaseRequestItem.requestquantity", "ja-JP", "申请数量", "申请数量（基本单位数量）"),
            // entity.purchaseRequestItem.requestquantity
            new TranslationSeedItem("entity.purchaseRequestItem.requestquantity", "zh-CN", "申请数量", "申请数量（基本单位数量）"),
            // entity.purchaseRequestItem.requestquantity
            new TranslationSeedItem("entity.purchaseRequestItem.requestquantity", "zh-HK", "申请数量", "申请数量（基本单位数量）"),

            // entity.purchaseRequestItem.convertedquantity
            new TranslationSeedItem("entity.purchaseRequestItem.convertedquantity", "en-US", "已转订单数量", "已转订单数量（基本单位数量）"),
            // entity.purchaseRequestItem.convertedquantity
            new TranslationSeedItem("entity.purchaseRequestItem.convertedquantity", "ja-JP", "已转订单数量", "已转订单数量（基本单位数量）"),
            // entity.purchaseRequestItem.convertedquantity
            new TranslationSeedItem("entity.purchaseRequestItem.convertedquantity", "zh-CN", "已转订单数量", "已转订单数量（基本单位数量）"),
            // entity.purchaseRequestItem.convertedquantity
            new TranslationSeedItem("entity.purchaseRequestItem.convertedquantity", "zh-HK", "已转订单数量", "已转订单数量（基本单位数量）"),

            // entity.purchaseRequestItem.estimatedunitprice
            new TranslationSeedItem("entity.purchaseRequestItem.estimatedunitprice", "en-US", "预计单价", "预计单价（精确到分，存储为整数，单位为分）"),
            // entity.purchaseRequestItem.estimatedunitprice
            new TranslationSeedItem("entity.purchaseRequestItem.estimatedunitprice", "ja-JP", "预计单价", "预计单价（精确到分，存储为整数，单位为分）"),
            // entity.purchaseRequestItem.estimatedunitprice
            new TranslationSeedItem("entity.purchaseRequestItem.estimatedunitprice", "zh-CN", "预计单价", "预计单价（精确到分，存储为整数，单位为分）"),
            // entity.purchaseRequestItem.estimatedunitprice
            new TranslationSeedItem("entity.purchaseRequestItem.estimatedunitprice", "zh-HK", "预计单价", "预计单价（精确到分，存储为整数，单位为分）"),

            // entity.purchaseRequestItem.estimatedamount
            new TranslationSeedItem("entity.purchaseRequestItem.estimatedamount", "en-US", "预计金额", "预计金额（精确到分，存储为整数，单位为分）"),
            // entity.purchaseRequestItem.estimatedamount
            new TranslationSeedItem("entity.purchaseRequestItem.estimatedamount", "ja-JP", "预计金额", "预计金额（精确到分，存储为整数，单位为分）"),
            // entity.purchaseRequestItem.estimatedamount
            new TranslationSeedItem("entity.purchaseRequestItem.estimatedamount", "zh-CN", "预计金额", "预计金额（精确到分，存储为整数，单位为分）"),
            // entity.purchaseRequestItem.estimatedamount
            new TranslationSeedItem("entity.purchaseRequestItem.estimatedamount", "zh-HK", "预计金额", "预计金额（精确到分，存储为整数，单位为分）"),

            // entity.purchaseRequestItem.referencesuppliercode
            new TranslationSeedItem("entity.purchaseRequestItem.referencesuppliercode", "en-US", "参考供应商编码", "参考供应商编码"),
            // entity.purchaseRequestItem.referencesuppliercode
            new TranslationSeedItem("entity.purchaseRequestItem.referencesuppliercode", "ja-JP", "参考供应商编码", "参考供应商编码"),
            // entity.purchaseRequestItem.referencesuppliercode
            new TranslationSeedItem("entity.purchaseRequestItem.referencesuppliercode", "zh-CN", "参考供应商编码", "参考供应商编码"),
            // entity.purchaseRequestItem.referencesuppliercode
            new TranslationSeedItem("entity.purchaseRequestItem.referencesuppliercode", "zh-HK", "参考供应商编码", "参考供应商编码"),

            // entity.purchaseRequestItem.referencesuppliername
            new TranslationSeedItem("entity.purchaseRequestItem.referencesuppliername", "en-US", "参考供应商名称", "参考供应商名称"),
            // entity.purchaseRequestItem.referencesuppliername
            new TranslationSeedItem("entity.purchaseRequestItem.referencesuppliername", "ja-JP", "参考供应商名称", "参考供应商名称"),
            // entity.purchaseRequestItem.referencesuppliername
            new TranslationSeedItem("entity.purchaseRequestItem.referencesuppliername", "zh-CN", "参考供应商名称", "参考供应商名称"),
            // entity.purchaseRequestItem.referencesuppliername
            new TranslationSeedItem("entity.purchaseRequestItem.referencesuppliername", "zh-HK", "参考供应商名称", "参考供应商名称"),
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
