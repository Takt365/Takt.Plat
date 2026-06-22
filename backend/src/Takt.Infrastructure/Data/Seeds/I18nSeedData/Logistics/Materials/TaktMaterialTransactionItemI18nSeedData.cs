// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Materials
// 文件名称：TaktMaterialTransactionItemI18nSeedData.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktMaterialTransactionItem 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Materials;

/// <summary>
/// TaktMaterialTransactionItem 实体国际化翻译种子（键前缀 entity.materialtransactionitem.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktMaterialTransactionItemI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktMaterialTransactionItem 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 materialtransactionitem 实体翻译...", tenantCode);

        foreach (var item in GetMaterialTransactionItemTranslations())
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

        TaktLogger.Information("TaktMaterialTransactionItem 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktMaterialTransactionItem 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.materialtransactionitem._self / entity.materialtransactionitem.{{field}}；ResourceGroup=Materials；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetMaterialTransactionItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.materialtransactionitem._self
            new TranslationSeedItem("entity.materialtransactionitem._self", "en-US", "Material Transaction Item Information_us", "实体名称"),
            // entity.materialtransactionitem._self
            new TranslationSeedItem("entity.materialtransactionitem._self", "ja-JP", "Takt物料交易明细信息_jp", "实体名称"),
            // entity.materialtransactionitem._self
            new TranslationSeedItem("entity.materialtransactionitem._self", "zh-CN", "Takt物料交易明细信息", "实体名称"),
            // entity.materialtransactionitem._self
            new TranslationSeedItem("entity.materialtransactionitem._self", "zh-HK", "Takt物料交易明细信息_hk", "实体名称"),

            // entity.materialtransactionitem.materialtransactionid
            new TranslationSeedItem("entity.materialtransactionitem.materialtransactionid", "en-US", "物料交易ID_us", "物料交易ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.materialtransactionitem.materialtransactionid
            new TranslationSeedItem("entity.materialtransactionitem.materialtransactionid", "ja-JP", "物料交易ID_jp", "物料交易ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.materialtransactionitem.materialtransactionid
            new TranslationSeedItem("entity.materialtransactionitem.materialtransactionid", "zh-CN", "物料交易ID", "物料交易ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.materialtransactionitem.materialtransactionid
            new TranslationSeedItem("entity.materialtransactionitem.materialtransactionid", "zh-HK", "物料交易ID_hk", "物料交易ID（主子表关系，序列化为string以避免Javascript精度问题）"),

            // entity.materialtransactionitem.materialtransactioncode
            new TranslationSeedItem("entity.materialtransactionitem.materialtransactioncode", "en-US", "物料交易单号_us", "物料交易单号（冗余字段，便于查询）"),
            // entity.materialtransactionitem.materialtransactioncode
            new TranslationSeedItem("entity.materialtransactionitem.materialtransactioncode", "ja-JP", "物料交易单号_jp", "物料交易单号（冗余字段，便于查询）"),
            // entity.materialtransactionitem.materialtransactioncode
            new TranslationSeedItem("entity.materialtransactionitem.materialtransactioncode", "zh-CN", "物料交易单号", "物料交易单号（冗余字段，便于查询）"),
            // entity.materialtransactionitem.materialtransactioncode
            new TranslationSeedItem("entity.materialtransactionitem.materialtransactioncode", "zh-HK", "物料交易单号_hk", "物料交易单号（冗余字段，便于查询）"),

            // entity.materialtransactionitem.linenumber
            new TranslationSeedItem("entity.materialtransactionitem.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.materialtransactionitem.linenumber
            new TranslationSeedItem("entity.materialtransactionitem.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.materialtransactionitem.linenumber
            new TranslationSeedItem("entity.materialtransactionitem.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.materialtransactionitem.linenumber
            new TranslationSeedItem("entity.materialtransactionitem.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.materialtransactionitem.sourcecode
            new TranslationSeedItem("entity.materialtransactionitem.sourcecode", "en-US", "来源单号_us", "来源单号（采购订单、销售订单等业务来源编码）"),
            // entity.materialtransactionitem.sourcecode
            new TranslationSeedItem("entity.materialtransactionitem.sourcecode", "ja-JP", "来源单号_jp", "来源单号（采购订单、销售订单等业务来源编码）"),
            // entity.materialtransactionitem.sourcecode
            new TranslationSeedItem("entity.materialtransactionitem.sourcecode", "zh-CN", "来源单号", "来源单号（采购订单、销售订单等业务来源编码）"),
            // entity.materialtransactionitem.sourcecode
            new TranslationSeedItem("entity.materialtransactionitem.sourcecode", "zh-HK", "来源单号_hk", "来源单号（采购订单、销售订单等业务来源编码）"),

            // entity.materialtransactionitem.sourcelinenumber
            new TranslationSeedItem("entity.materialtransactionitem.sourcelinenumber", "en-US", "来源单行号_us", "来源单行号"),
            // entity.materialtransactionitem.sourcelinenumber
            new TranslationSeedItem("entity.materialtransactionitem.sourcelinenumber", "ja-JP", "来源单行号_jp", "来源单行号"),
            // entity.materialtransactionitem.sourcelinenumber
            new TranslationSeedItem("entity.materialtransactionitem.sourcelinenumber", "zh-CN", "来源单行号", "来源单行号"),
            // entity.materialtransactionitem.sourcelinenumber
            new TranslationSeedItem("entity.materialtransactionitem.sourcelinenumber", "zh-HK", "来源单行号_hk", "来源单行号"),

            // entity.materialtransactionitem.materialcode
            new TranslationSeedItem("entity.materialtransactionitem.materialcode", "en-US", "物料编码_us", "物料编码"),
            // entity.materialtransactionitem.materialcode
            new TranslationSeedItem("entity.materialtransactionitem.materialcode", "ja-JP", "物料编码_jp", "物料编码"),
            // entity.materialtransactionitem.materialcode
            new TranslationSeedItem("entity.materialtransactionitem.materialcode", "zh-CN", "物料编码", "物料编码"),
            // entity.materialtransactionitem.materialcode
            new TranslationSeedItem("entity.materialtransactionitem.materialcode", "zh-HK", "物料编码_hk", "物料编码"),

            // entity.materialtransactionitem.materialname
            new TranslationSeedItem("entity.materialtransactionitem.materialname", "en-US", "物料名称_us", "物料名称"),
            // entity.materialtransactionitem.materialname
            new TranslationSeedItem("entity.materialtransactionitem.materialname", "ja-JP", "物料名称_jp", "物料名称"),
            // entity.materialtransactionitem.materialname
            new TranslationSeedItem("entity.materialtransactionitem.materialname", "zh-CN", "物料名称", "物料名称"),
            // entity.materialtransactionitem.materialname
            new TranslationSeedItem("entity.materialtransactionitem.materialname", "zh-HK", "物料名称_hk", "物料名称"),

            // entity.materialtransactionitem.materialspecification
            new TranslationSeedItem("entity.materialtransactionitem.materialspecification", "en-US", "物料规格_us", "物料规格"),
            // entity.materialtransactionitem.materialspecification
            new TranslationSeedItem("entity.materialtransactionitem.materialspecification", "ja-JP", "物料规格_jp", "物料规格"),
            // entity.materialtransactionitem.materialspecification
            new TranslationSeedItem("entity.materialtransactionitem.materialspecification", "zh-CN", "物料规格", "物料规格"),
            // entity.materialtransactionitem.materialspecification
            new TranslationSeedItem("entity.materialtransactionitem.materialspecification", "zh-HK", "物料规格_hk", "物料规格"),

            // entity.materialtransactionitem.transactionunit
            new TranslationSeedItem("entity.materialtransactionitem.transactionunit", "en-US", "交易单位_us", "交易单位"),
            // entity.materialtransactionitem.transactionunit
            new TranslationSeedItem("entity.materialtransactionitem.transactionunit", "ja-JP", "交易单位_jp", "交易单位"),
            // entity.materialtransactionitem.transactionunit
            new TranslationSeedItem("entity.materialtransactionitem.transactionunit", "zh-CN", "交易单位", "交易单位"),
            // entity.materialtransactionitem.transactionunit
            new TranslationSeedItem("entity.materialtransactionitem.transactionunit", "zh-HK", "交易单位_hk", "交易单位"),

            // entity.materialtransactionitem.transactionquantity
            new TranslationSeedItem("entity.materialtransactionitem.transactionquantity", "en-US", "交易数量_us", "交易数量（基本单位数量）"),
            // entity.materialtransactionitem.transactionquantity
            new TranslationSeedItem("entity.materialtransactionitem.transactionquantity", "ja-JP", "交易数量_jp", "交易数量（基本单位数量）"),
            // entity.materialtransactionitem.transactionquantity
            new TranslationSeedItem("entity.materialtransactionitem.transactionquantity", "zh-CN", "交易数量", "交易数量（基本单位数量）"),
            // entity.materialtransactionitem.transactionquantity
            new TranslationSeedItem("entity.materialtransactionitem.transactionquantity", "zh-HK", "交易数量_hk", "交易数量（基本单位数量）"),

            // entity.materialtransactionitem.batchno
            new TranslationSeedItem("entity.materialtransactionitem.batchno", "en-US", "批次号_us", "批次号"),
            // entity.materialtransactionitem.batchno
            new TranslationSeedItem("entity.materialtransactionitem.batchno", "ja-JP", "批次号_jp", "批次号"),
            // entity.materialtransactionitem.batchno
            new TranslationSeedItem("entity.materialtransactionitem.batchno", "zh-CN", "批次号", "批次号"),
            // entity.materialtransactionitem.batchno
            new TranslationSeedItem("entity.materialtransactionitem.batchno", "zh-HK", "批次号_hk", "批次号"),

            // entity.materialtransactionitem.warehousecode
            new TranslationSeedItem("entity.materialtransactionitem.warehousecode", "en-US", "源仓库编码_us", "源仓库编码（行级可覆盖主表，关联 TaktWarehouse.WarehouseCode）"),
            // entity.materialtransactionitem.warehousecode
            new TranslationSeedItem("entity.materialtransactionitem.warehousecode", "ja-JP", "源仓库编码_jp", "源仓库编码（行级可覆盖主表，关联 TaktWarehouse.WarehouseCode）"),
            // entity.materialtransactionitem.warehousecode
            new TranslationSeedItem("entity.materialtransactionitem.warehousecode", "zh-CN", "源仓库编码", "源仓库编码（行级可覆盖主表，关联 TaktWarehouse.WarehouseCode）"),
            // entity.materialtransactionitem.warehousecode
            new TranslationSeedItem("entity.materialtransactionitem.warehousecode", "zh-HK", "源仓库编码_hk", "源仓库编码（行级可覆盖主表，关联 TaktWarehouse.WarehouseCode）"),

            // entity.materialtransactionitem.locationcode
            new TranslationSeedItem("entity.materialtransactionitem.locationcode", "en-US", "源库位编码_us", "源库位编码（行级可覆盖主表，关联 TaktStorageLocation.LocationCode）"),
            // entity.materialtransactionitem.locationcode
            new TranslationSeedItem("entity.materialtransactionitem.locationcode", "ja-JP", "源库位编码_jp", "源库位编码（行级可覆盖主表，关联 TaktStorageLocation.LocationCode）"),
            // entity.materialtransactionitem.locationcode
            new TranslationSeedItem("entity.materialtransactionitem.locationcode", "zh-CN", "源库位编码", "源库位编码（行级可覆盖主表，关联 TaktStorageLocation.LocationCode）"),
            // entity.materialtransactionitem.locationcode
            new TranslationSeedItem("entity.materialtransactionitem.locationcode", "zh-HK", "源库位编码_hk", "源库位编码（行级可覆盖主表，关联 TaktStorageLocation.LocationCode）"),

            // entity.materialtransactionitem.targetwarehousecode
            new TranslationSeedItem("entity.materialtransactionitem.targetwarehousecode", "en-US", "目标仓库编码_us", "目标仓库编码（移库/调拨时使用）"),
            // entity.materialtransactionitem.targetwarehousecode
            new TranslationSeedItem("entity.materialtransactionitem.targetwarehousecode", "ja-JP", "目标仓库编码_jp", "目标仓库编码（移库/调拨时使用）"),
            // entity.materialtransactionitem.targetwarehousecode
            new TranslationSeedItem("entity.materialtransactionitem.targetwarehousecode", "zh-CN", "目标仓库编码", "目标仓库编码（移库/调拨时使用）"),
            // entity.materialtransactionitem.targetwarehousecode
            new TranslationSeedItem("entity.materialtransactionitem.targetwarehousecode", "zh-HK", "目标仓库编码_hk", "目标仓库编码（移库/调拨时使用）"),

            // entity.materialtransactionitem.targetlocationcode
            new TranslationSeedItem("entity.materialtransactionitem.targetlocationcode", "en-US", "目标库位编码_us", "目标库位编码（移库/调拨时使用）"),
            // entity.materialtransactionitem.targetlocationcode
            new TranslationSeedItem("entity.materialtransactionitem.targetlocationcode", "ja-JP", "目标库位编码_jp", "目标库位编码（移库/调拨时使用）"),
            // entity.materialtransactionitem.targetlocationcode
            new TranslationSeedItem("entity.materialtransactionitem.targetlocationcode", "zh-CN", "目标库位编码", "目标库位编码（移库/调拨时使用）"),
            // entity.materialtransactionitem.targetlocationcode
            new TranslationSeedItem("entity.materialtransactionitem.targetlocationcode", "zh-HK", "目标库位编码_hk", "目标库位编码（移库/调拨时使用）"),

            // entity.materialtransactionitem.unitprice
            new TranslationSeedItem("entity.materialtransactionitem.unitprice", "en-US", "单价_us", "单价"),
            // entity.materialtransactionitem.unitprice
            new TranslationSeedItem("entity.materialtransactionitem.unitprice", "ja-JP", "单价_jp", "单价"),
            // entity.materialtransactionitem.unitprice
            new TranslationSeedItem("entity.materialtransactionitem.unitprice", "zh-CN", "单价", "单价"),
            // entity.materialtransactionitem.unitprice
            new TranslationSeedItem("entity.materialtransactionitem.unitprice", "zh-HK", "单价_hk", "单价"),

            // entity.materialtransactionitem.lineamount
            new TranslationSeedItem("entity.materialtransactionitem.lineamount", "en-US", "行金额_us", "行金额"),
            // entity.materialtransactionitem.lineamount
            new TranslationSeedItem("entity.materialtransactionitem.lineamount", "ja-JP", "行金额_jp", "行金额"),
            // entity.materialtransactionitem.lineamount
            new TranslationSeedItem("entity.materialtransactionitem.lineamount", "zh-CN", "行金额", "行金额"),
            // entity.materialtransactionitem.lineamount
            new TranslationSeedItem("entity.materialtransactionitem.lineamount", "zh-HK", "行金额_hk", "行金额"),

            // entity.materialtransactionitem.materialtransaction
            new TranslationSeedItem("entity.materialtransactionitem.materialtransaction", "en-US", "物料交易主表_us", "物料交易主表"),
            // entity.materialtransactionitem.materialtransaction
            new TranslationSeedItem("entity.materialtransactionitem.materialtransaction", "ja-JP", "物料交易主表_jp", "物料交易主表"),
            // entity.materialtransactionitem.materialtransaction
            new TranslationSeedItem("entity.materialtransactionitem.materialtransaction", "zh-CN", "物料交易主表", "物料交易主表"),
            // entity.materialtransactionitem.materialtransaction
            new TranslationSeedItem("entity.materialtransactionitem.materialtransaction", "zh-HK", "物料交易主表_hk", "物料交易主表"),
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
        translation.ResourceGroup = "Materials";
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
