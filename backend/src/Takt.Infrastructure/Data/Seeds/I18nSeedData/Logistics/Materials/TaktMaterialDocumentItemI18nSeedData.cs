// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Materials
// 文件名称：TaktMaterialDocumentItemI18nSeedData.cs
// 创建时间：2026-07-02
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktMaterialDocumentItem 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktMaterialDocumentItem 实体国际化翻译种子（键前缀 entity.materialdocumentitem.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktMaterialDocumentItemI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktMaterialDocumentItem 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 materialdocumentitem 实体翻译...", tenantCode);

        foreach (var item in GetMaterialDocumentItemTranslations())
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

        TaktLogger.Information("TaktMaterialDocumentItem 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktMaterialDocumentItem 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.materialdocumentitem._self / entity.materialdocumentitem.{{field}}；ResourceGroup=Materials；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetMaterialDocumentItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.materialdocumentitem._self
            new TranslationSeedItem("entity.materialdocumentitem._self", "en-US", "Material Document Item Information_us", "实体名称"),
            // entity.materialdocumentitem._self
            new TranslationSeedItem("entity.materialdocumentitem._self", "ja-JP", "Takt物料凭证行项目信息_jp", "实体名称"),
            // entity.materialdocumentitem._self
            new TranslationSeedItem("entity.materialdocumentitem._self", "zh-CN", "Takt物料凭证行项目信息", "实体名称"),
            // entity.materialdocumentitem._self
            new TranslationSeedItem("entity.materialdocumentitem._self", "zh-HK", "Takt物料凭证行项目信息_hk", "实体名称"),

            // entity.materialdocumentitem.materialdocumentid
            new TranslationSeedItem("entity.materialdocumentitem.materialdocumentid", "en-US", "物料凭证ID_us", "物料凭证 ID（关联 TaktMaterialDocument.Id，选项 TaktMaterialDocuments/options）"),
            // entity.materialdocumentitem.materialdocumentid
            new TranslationSeedItem("entity.materialdocumentitem.materialdocumentid", "ja-JP", "物料凭证ID_jp", "物料凭证 ID（关联 TaktMaterialDocument.Id，选项 TaktMaterialDocuments/options）"),
            // entity.materialdocumentitem.materialdocumentid
            new TranslationSeedItem("entity.materialdocumentitem.materialdocumentid", "zh-CN", "物料凭证ID", "物料凭证 ID（关联 TaktMaterialDocument.Id，选项 TaktMaterialDocuments/options）"),
            // entity.materialdocumentitem.materialdocumentid
            new TranslationSeedItem("entity.materialdocumentitem.materialdocumentid", "zh-HK", "物料凭证ID_hk", "物料凭证 ID（关联 TaktMaterialDocument.Id，选项 TaktMaterialDocuments/options）"),

            // entity.materialdocumentitem.materialdocumentcode
            new TranslationSeedItem("entity.materialdocumentitem.materialdocumentcode", "en-US", "物料凭证号_us", "物料凭证号（冗余）"),
            // entity.materialdocumentitem.materialdocumentcode
            new TranslationSeedItem("entity.materialdocumentitem.materialdocumentcode", "ja-JP", "物料凭证号_jp", "物料凭证号（冗余）"),
            // entity.materialdocumentitem.materialdocumentcode
            new TranslationSeedItem("entity.materialdocumentitem.materialdocumentcode", "zh-CN", "物料凭证号", "物料凭证号（冗余）"),
            // entity.materialdocumentitem.materialdocumentcode
            new TranslationSeedItem("entity.materialdocumentitem.materialdocumentcode", "zh-HK", "物料凭证号_hk", "物料凭证号（冗余）"),

            // entity.materialdocumentitem.linenumber
            new TranslationSeedItem("entity.materialdocumentitem.linenumber", "en-US", "行号_us", "行号（固定步长=10）"),
            // entity.materialdocumentitem.linenumber
            new TranslationSeedItem("entity.materialdocumentitem.linenumber", "ja-JP", "行号_jp", "行号（固定步长=10）"),
            // entity.materialdocumentitem.linenumber
            new TranslationSeedItem("entity.materialdocumentitem.linenumber", "zh-CN", "行号", "行号（固定步长=10）"),
            // entity.materialdocumentitem.linenumber
            new TranslationSeedItem("entity.materialdocumentitem.linenumber", "zh-HK", "行号_hk", "行号（固定步长=10）"),

            // entity.materialdocumentitem.warehousecode
            new TranslationSeedItem("entity.materialdocumentitem.warehousecode", "en-US", "库存地点_us", "库存地点（关联 TaktWarehouse.WarehouseCode，选项 TaktWarehouses/options，DictValue=WarehouseCode）"),
            // entity.materialdocumentitem.warehousecode
            new TranslationSeedItem("entity.materialdocumentitem.warehousecode", "ja-JP", "库存地点_jp", "库存地点（关联 TaktWarehouse.WarehouseCode，选项 TaktWarehouses/options，DictValue=WarehouseCode）"),
            // entity.materialdocumentitem.warehousecode
            new TranslationSeedItem("entity.materialdocumentitem.warehousecode", "zh-CN", "库存地点", "库存地点（关联 TaktWarehouse.WarehouseCode，选项 TaktWarehouses/options，DictValue=WarehouseCode）"),
            // entity.materialdocumentitem.warehousecode
            new TranslationSeedItem("entity.materialdocumentitem.warehousecode", "zh-HK", "库存地点_hk", "库存地点（关联 TaktWarehouse.WarehouseCode，选项 TaktWarehouses/options，DictValue=WarehouseCode）"),

            // entity.materialdocumentitem.movementtype
            new TranslationSeedItem("entity.materialdocumentitem.movementtype", "en-US", "移动类型_us", "移动类型（字典 logistics_movement_type，如 101=收货）"),
            // entity.materialdocumentitem.movementtype
            new TranslationSeedItem("entity.materialdocumentitem.movementtype", "ja-JP", "移动类型_jp", "移动类型（字典 logistics_movement_type，如 101=收货）"),
            // entity.materialdocumentitem.movementtype
            new TranslationSeedItem("entity.materialdocumentitem.movementtype", "zh-CN", "移动类型", "移动类型（字典 logistics_movement_type，如 101=收货）"),
            // entity.materialdocumentitem.movementtype
            new TranslationSeedItem("entity.materialdocumentitem.movementtype", "zh-HK", "移动类型_hk", "移动类型（字典 logistics_movement_type，如 101=收货）"),

            // entity.materialdocumentitem.postingdate
            new TranslationSeedItem("entity.materialdocumentitem.postingdate", "en-US", "过账日期_us", "过账日期"),
            // entity.materialdocumentitem.postingdate
            new TranslationSeedItem("entity.materialdocumentitem.postingdate", "ja-JP", "过账日期_jp", "过账日期"),
            // entity.materialdocumentitem.postingdate
            new TranslationSeedItem("entity.materialdocumentitem.postingdate", "zh-CN", "过账日期", "过账日期"),
            // entity.materialdocumentitem.postingdate
            new TranslationSeedItem("entity.materialdocumentitem.postingdate", "zh-HK", "过账日期_hk", "过账日期"),

            // entity.materialdocumentitem.quantity
            new TranslationSeedItem("entity.materialdocumentitem.quantity", "en-US", "数量_us", "数量（基本单位数量，出库为负由移动类型决定）"),
            // entity.materialdocumentitem.quantity
            new TranslationSeedItem("entity.materialdocumentitem.quantity", "ja-JP", "数量_jp", "数量（基本单位数量，出库为负由移动类型决定）"),
            // entity.materialdocumentitem.quantity
            new TranslationSeedItem("entity.materialdocumentitem.quantity", "zh-CN", "数量", "数量（基本单位数量，出库为负由移动类型决定）"),
            // entity.materialdocumentitem.quantity
            new TranslationSeedItem("entity.materialdocumentitem.quantity", "zh-HK", "数量_hk", "数量（基本单位数量，出库为负由移动类型决定）"),

            // entity.materialdocumentitem.specialstock
            new TranslationSeedItem("entity.materialdocumentitem.specialstock", "en-US", "特殊库存_us", "特殊库存（字典 logistics_special_stock_type，空=非特殊库存）"),
            // entity.materialdocumentitem.specialstock
            new TranslationSeedItem("entity.materialdocumentitem.specialstock", "ja-JP", "特殊库存_jp", "特殊库存（字典 logistics_special_stock_type，空=非特殊库存）"),
            // entity.materialdocumentitem.specialstock
            new TranslationSeedItem("entity.materialdocumentitem.specialstock", "zh-CN", "特殊库存", "特殊库存（字典 logistics_special_stock_type，空=非特殊库存）"),
            // entity.materialdocumentitem.specialstock
            new TranslationSeedItem("entity.materialdocumentitem.specialstock", "zh-HK", "特殊库存_hk", "特殊库存（字典 logistics_special_stock_type，空=非特殊库存）"),

            // entity.materialdocumentitem.purchaseordercode
            new TranslationSeedItem("entity.materialdocumentitem.purchaseordercode", "en-US", "采购订单_us", "采购订单（关联 TaktPurchaseOrder.PurchaseOrderCode）"),
            // entity.materialdocumentitem.purchaseordercode
            new TranslationSeedItem("entity.materialdocumentitem.purchaseordercode", "ja-JP", "采购订单_jp", "采购订单（关联 TaktPurchaseOrder.PurchaseOrderCode）"),
            // entity.materialdocumentitem.purchaseordercode
            new TranslationSeedItem("entity.materialdocumentitem.purchaseordercode", "zh-CN", "采购订单", "采购订单（关联 TaktPurchaseOrder.PurchaseOrderCode）"),
            // entity.materialdocumentitem.purchaseordercode
            new TranslationSeedItem("entity.materialdocumentitem.purchaseordercode", "zh-HK", "采购订单_hk", "采购订单（关联 TaktPurchaseOrder.PurchaseOrderCode）"),

            // entity.materialdocumentitem.productionordercode
            new TranslationSeedItem("entity.materialdocumentitem.productionordercode", "en-US", "生产订单_us", "生产订单"),
            // entity.materialdocumentitem.productionordercode
            new TranslationSeedItem("entity.materialdocumentitem.productionordercode", "ja-JP", "生产订单_jp", "生产订单"),
            // entity.materialdocumentitem.productionordercode
            new TranslationSeedItem("entity.materialdocumentitem.productionordercode", "zh-CN", "生产订单", "生产订单"),
            // entity.materialdocumentitem.productionordercode
            new TranslationSeedItem("entity.materialdocumentitem.productionordercode", "zh-HK", "生产订单_hk", "生产订单"),

            // entity.materialdocumentitem.projectcode
            new TranslationSeedItem("entity.materialdocumentitem.projectcode", "en-US", "项目编号_us", "项目编号（WBS 元素）"),
            // entity.materialdocumentitem.projectcode
            new TranslationSeedItem("entity.materialdocumentitem.projectcode", "ja-JP", "项目编号_jp", "项目编号（WBS 元素）"),
            // entity.materialdocumentitem.projectcode
            new TranslationSeedItem("entity.materialdocumentitem.projectcode", "zh-CN", "项目编号", "项目编号（WBS 元素）"),
            // entity.materialdocumentitem.projectcode
            new TranslationSeedItem("entity.materialdocumentitem.projectcode", "zh-HK", "项目编号_hk", "项目编号（WBS 元素）"),

            // entity.materialdocumentitem.localcurrencyamount
            new TranslationSeedItem("entity.materialdocumentitem.localcurrencyamount", "en-US", "本位币金额_us", "本位币金额"),
            // entity.materialdocumentitem.localcurrencyamount
            new TranslationSeedItem("entity.materialdocumentitem.localcurrencyamount", "ja-JP", "本位币金额_jp", "本位币金额"),
            // entity.materialdocumentitem.localcurrencyamount
            new TranslationSeedItem("entity.materialdocumentitem.localcurrencyamount", "zh-CN", "本位币金额", "本位币金额"),
            // entity.materialdocumentitem.localcurrencyamount
            new TranslationSeedItem("entity.materialdocumentitem.localcurrencyamount", "zh-HK", "本位币金额_hk", "本位币金额"),

            // entity.materialdocumentitem.documentdate
            new TranslationSeedItem("entity.materialdocumentitem.documentdate", "en-US", "凭证日期_us", "凭证日期"),
            // entity.materialdocumentitem.documentdate
            new TranslationSeedItem("entity.materialdocumentitem.documentdate", "ja-JP", "凭证日期_jp", "凭证日期"),
            // entity.materialdocumentitem.documentdate
            new TranslationSeedItem("entity.materialdocumentitem.documentdate", "zh-CN", "凭证日期", "凭证日期"),
            // entity.materialdocumentitem.documentdate
            new TranslationSeedItem("entity.materialdocumentitem.documentdate", "zh-HK", "凭证日期_hk", "凭证日期"),

            // entity.materialdocumentitem.referencedocumentcode
            new TranslationSeedItem("entity.materialdocumentitem.referencedocumentcode", "en-US", "收货/发货单编号_us", "收货/发货单编号"),
            // entity.materialdocumentitem.referencedocumentcode
            new TranslationSeedItem("entity.materialdocumentitem.referencedocumentcode", "ja-JP", "收货/发货单编号_jp", "收货/发货单编号"),
            // entity.materialdocumentitem.referencedocumentcode
            new TranslationSeedItem("entity.materialdocumentitem.referencedocumentcode", "zh-CN", "收货/发货单编号", "收货/发货单编号"),
            // entity.materialdocumentitem.referencedocumentcode
            new TranslationSeedItem("entity.materialdocumentitem.referencedocumentcode", "zh-HK", "收货/发货单编号_hk", "收货/发货单编号"),

            // entity.materialdocumentitem.customercode
            new TranslationSeedItem("entity.materialdocumentitem.customercode", "en-US", "客户_us", "客户（关联 TaktCustomer.CustomerCode，选项 TaktCustomers/options）"),
            // entity.materialdocumentitem.customercode
            new TranslationSeedItem("entity.materialdocumentitem.customercode", "ja-JP", "客户_jp", "客户（关联 TaktCustomer.CustomerCode，选项 TaktCustomers/options）"),
            // entity.materialdocumentitem.customercode
            new TranslationSeedItem("entity.materialdocumentitem.customercode", "zh-CN", "客户", "客户（关联 TaktCustomer.CustomerCode，选项 TaktCustomers/options）"),
            // entity.materialdocumentitem.customercode
            new TranslationSeedItem("entity.materialdocumentitem.customercode", "zh-HK", "客户_hk", "客户（关联 TaktCustomer.CustomerCode，选项 TaktCustomers/options）"),

            // entity.materialdocumentitem.materialtransaction
            new TranslationSeedItem("entity.materialdocumentitem.materialtransaction", "en-US", "物料凭证主表_us", "物料凭证主表"),
            // entity.materialdocumentitem.materialtransaction
            new TranslationSeedItem("entity.materialdocumentitem.materialtransaction", "ja-JP", "物料凭证主表_jp", "物料凭证主表"),
            // entity.materialdocumentitem.materialtransaction
            new TranslationSeedItem("entity.materialdocumentitem.materialtransaction", "zh-CN", "物料凭证主表", "物料凭证主表"),
            // entity.materialdocumentitem.materialtransaction
            new TranslationSeedItem("entity.materialdocumentitem.materialtransaction", "zh-HK", "物料凭证主表_hk", "物料凭证主表"),
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
