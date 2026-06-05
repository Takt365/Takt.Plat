// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Serial
// 文件名称：TaktProductSerialInboundI18nSeedData.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktProductSerialInbound 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Serial;

/// <summary>
/// TaktProductSerialInbound 实体国际化翻译种子（键前缀 entity.productSerialInbound.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktProductSerialInboundI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktProductSerialInbound 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 productSerialInbound 实体翻译...", tenantCode);

        foreach (var item in GetProductSerialInboundTranslations())
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

        TaktLogger.Information("TaktProductSerialInbound 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktProductSerialInbound 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.productSerialInbound._self / entity.productSerialInbound.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetProductSerialInboundTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.productSerialInbound._self
            new TranslationSeedItem("entity.productSerialInbound._self", "en-US", "Product Serial Inbound Information", "实体名称"),
            // entity.productSerialInbound._self
            new TranslationSeedItem("entity.productSerialInbound._self", "ja-JP", "产品序列号入库主表信息", "实体名称"),
            // entity.productSerialInbound._self
            new TranslationSeedItem("entity.productSerialInbound._self", "zh-CN", "产品序列号入库主表信息", "实体名称"),
            // entity.productSerialInbound._self
            new TranslationSeedItem("entity.productSerialInbound._self", "zh-HK", "产品序列号入库主表信息", "实体名称"),

            // entity.productSerialInbound.plantcode
            new TranslationSeedItem("entity.productSerialInbound.plantcode", "en-US", "工厂代码", "工厂代码(4位字母数字组合)"),
            // entity.productSerialInbound.plantcode
            new TranslationSeedItem("entity.productSerialInbound.plantcode", "ja-JP", "工厂代码", "工厂代码(4位字母数字组合)"),
            // entity.productSerialInbound.plantcode
            new TranslationSeedItem("entity.productSerialInbound.plantcode", "zh-CN", "工厂代码", "工厂代码(4位字母数字组合)"),
            // entity.productSerialInbound.plantcode
            new TranslationSeedItem("entity.productSerialInbound.plantcode", "zh-HK", "工厂代码", "工厂代码(4位字母数字组合)"),

            // entity.productSerialInbound.inboundno
            new TranslationSeedItem("entity.productSerialInbound.inboundno", "en-US", "入库单号", "入库单号（组合唯一索引：PlantCode + InboundNo）"),
            // entity.productSerialInbound.inboundno
            new TranslationSeedItem("entity.productSerialInbound.inboundno", "ja-JP", "入库单号", "入库单号（组合唯一索引：PlantCode + InboundNo）"),
            // entity.productSerialInbound.inboundno
            new TranslationSeedItem("entity.productSerialInbound.inboundno", "zh-CN", "入库单号", "入库单号（组合唯一索引：PlantCode + InboundNo）"),
            // entity.productSerialInbound.inboundno
            new TranslationSeedItem("entity.productSerialInbound.inboundno", "zh-HK", "入库单号", "入库单号（组合唯一索引：PlantCode + InboundNo）"),

            // entity.productSerialInbound.inbounddate
            new TranslationSeedItem("entity.productSerialInbound.inbounddate", "en-US", "入库日期", "入库日期"),
            // entity.productSerialInbound.inbounddate
            new TranslationSeedItem("entity.productSerialInbound.inbounddate", "ja-JP", "入库日期", "入库日期"),
            // entity.productSerialInbound.inbounddate
            new TranslationSeedItem("entity.productSerialInbound.inbounddate", "zh-CN", "入库日期", "入库日期"),
            // entity.productSerialInbound.inbounddate
            new TranslationSeedItem("entity.productSerialInbound.inbounddate", "zh-HK", "入库日期", "入库日期"),

            // entity.productSerialInbound.inboundtype
            new TranslationSeedItem("entity.productSerialInbound.inboundtype", "en-US", "入库类型", "入库类型(0=采购入库,1=生产入库,2=退货入库,3=调拨入库,4=序列号入库,5=其他)"),
            // entity.productSerialInbound.inboundtype
            new TranslationSeedItem("entity.productSerialInbound.inboundtype", "ja-JP", "入库类型", "入库类型(0=采购入库,1=生产入库,2=退货入库,3=调拨入库,4=序列号入库,5=其他)"),
            // entity.productSerialInbound.inboundtype
            new TranslationSeedItem("entity.productSerialInbound.inboundtype", "zh-CN", "入库类型", "入库类型(0=采购入库,1=生产入库,2=退货入库,3=调拨入库,4=序列号入库,5=其他)"),
            // entity.productSerialInbound.inboundtype
            new TranslationSeedItem("entity.productSerialInbound.inboundtype", "zh-HK", "入库类型", "入库类型(0=采购入库,1=生产入库,2=退货入库,3=调拨入库,4=序列号入库,5=其他)"),

            // entity.productSerialInbound.warehousecode
            new TranslationSeedItem("entity.productSerialInbound.warehousecode", "en-US", "仓库编码", "仓库编码"),
            // entity.productSerialInbound.warehousecode
            new TranslationSeedItem("entity.productSerialInbound.warehousecode", "ja-JP", "仓库编码", "仓库编码"),
            // entity.productSerialInbound.warehousecode
            new TranslationSeedItem("entity.productSerialInbound.warehousecode", "zh-CN", "仓库编码", "仓库编码"),
            // entity.productSerialInbound.warehousecode
            new TranslationSeedItem("entity.productSerialInbound.warehousecode", "zh-HK", "仓库编码", "仓库编码"),

            // entity.productSerialInbound.locationcode
            new TranslationSeedItem("entity.productSerialInbound.locationcode", "en-US", "库位编码", "库位编码"),
            // entity.productSerialInbound.locationcode
            new TranslationSeedItem("entity.productSerialInbound.locationcode", "ja-JP", "库位编码", "库位编码"),
            // entity.productSerialInbound.locationcode
            new TranslationSeedItem("entity.productSerialInbound.locationcode", "zh-CN", "库位编码", "库位编码"),
            // entity.productSerialInbound.locationcode
            new TranslationSeedItem("entity.productSerialInbound.locationcode", "zh-HK", "库位编码", "库位编码"),

            // entity.productSerialInbound.totalquantity
            new TranslationSeedItem("entity.productSerialInbound.totalquantity", "en-US", "总数量", "总数量"),
            // entity.productSerialInbound.totalquantity
            new TranslationSeedItem("entity.productSerialInbound.totalquantity", "ja-JP", "总数量", "总数量"),
            // entity.productSerialInbound.totalquantity
            new TranslationSeedItem("entity.productSerialInbound.totalquantity", "zh-CN", "总数量", "总数量"),
            // entity.productSerialInbound.totalquantity
            new TranslationSeedItem("entity.productSerialInbound.totalquantity", "zh-HK", "总数量", "总数量"),

            // entity.productSerialInbound.relatedcompany
            new TranslationSeedItem("entity.productSerialInbound.relatedcompany", "en-US", "关联公司", "关联公司"),
            // entity.productSerialInbound.relatedcompany
            new TranslationSeedItem("entity.productSerialInbound.relatedcompany", "ja-JP", "关联公司", "关联公司"),
            // entity.productSerialInbound.relatedcompany
            new TranslationSeedItem("entity.productSerialInbound.relatedcompany", "zh-CN", "关联公司", "关联公司"),
            // entity.productSerialInbound.relatedcompany
            new TranslationSeedItem("entity.productSerialInbound.relatedcompany", "zh-HK", "关联公司", "关联公司"),

            // entity.productSerialInbound.items
            new TranslationSeedItem("entity.productSerialInbound.items", "en-US", "items", "产品序列号入库明细列表(主子表关系)"),
            // entity.productSerialInbound.items
            new TranslationSeedItem("entity.productSerialInbound.items", "ja-JP", "items", "产品序列号入库明细列表(主子表关系)"),
            // entity.productSerialInbound.items
            new TranslationSeedItem("entity.productSerialInbound.items", "zh-CN", "items", "产品序列号入库明细列表(主子表关系)"),
            // entity.productSerialInbound.items
            new TranslationSeedItem("entity.productSerialInbound.items", "zh-HK", "items", "产品序列号入库明细列表(主子表关系)"),
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
