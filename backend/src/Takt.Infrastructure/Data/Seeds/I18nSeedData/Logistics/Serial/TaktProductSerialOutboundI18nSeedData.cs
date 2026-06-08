// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Serial
// 文件名称：TaktProductSerialOutboundI18nSeedData.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktProductSerialOutbound 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktProductSerialOutbound 实体国际化翻译种子（键前缀 entity.productSerialOutbound.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktProductSerialOutboundI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktProductSerialOutbound 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 productSerialOutbound 实体翻译...", tenantCode);

        foreach (var item in GetProductSerialOutboundTranslations())
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

        TaktLogger.Information("TaktProductSerialOutbound 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktProductSerialOutbound 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.productSerialOutbound._self / entity.productSerialOutbound.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetProductSerialOutboundTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.productSerialOutbound._self
            new TranslationSeedItem("entity.productSerialOutbound._self", "en-US", "Product Serial Outbound Information", "实体名称"),
            // entity.productSerialOutbound._self
            new TranslationSeedItem("entity.productSerialOutbound._self", "ja-JP", "产品序列号出库主表信息", "实体名称"),
            // entity.productSerialOutbound._self
            new TranslationSeedItem("entity.productSerialOutbound._self", "zh-CN", "产品序列号出库主表信息", "实体名称"),
            // entity.productSerialOutbound._self
            new TranslationSeedItem("entity.productSerialOutbound._self", "zh-HK", "产品序列号出库主表信息", "实体名称"),

            // entity.productSerialOutbound.plantcode
            new TranslationSeedItem("entity.productSerialOutbound.plantcode", "en-US", "工厂代码", "工厂代码(4位字母数字组合)"),
            // entity.productSerialOutbound.plantcode
            new TranslationSeedItem("entity.productSerialOutbound.plantcode", "ja-JP", "工厂代码", "工厂代码(4位字母数字组合)"),
            // entity.productSerialOutbound.plantcode
            new TranslationSeedItem("entity.productSerialOutbound.plantcode", "zh-CN", "工厂代码", "工厂代码(4位字母数字组合)"),
            // entity.productSerialOutbound.plantcode
            new TranslationSeedItem("entity.productSerialOutbound.plantcode", "zh-HK", "工厂代码", "工厂代码(4位字母数字组合)"),

            // entity.productSerialOutbound.outboundno
            new TranslationSeedItem("entity.productSerialOutbound.outboundno", "en-US", "出库单号", "出库单号(组合唯一索引:PlantCode + OutboundNo)"),
            // entity.productSerialOutbound.outboundno
            new TranslationSeedItem("entity.productSerialOutbound.outboundno", "ja-JP", "出库单号", "出库单号(组合唯一索引:PlantCode + OutboundNo)"),
            // entity.productSerialOutbound.outboundno
            new TranslationSeedItem("entity.productSerialOutbound.outboundno", "zh-CN", "出库单号", "出库单号(组合唯一索引:PlantCode + OutboundNo)"),
            // entity.productSerialOutbound.outboundno
            new TranslationSeedItem("entity.productSerialOutbound.outboundno", "zh-HK", "出库单号", "出库单号(组合唯一索引:PlantCode + OutboundNo)"),

            // entity.productSerialOutbound.shippinginvoiceno
            new TranslationSeedItem("entity.productSerialOutbound.shippinginvoiceno", "en-US", "出货发票号", "出货发票号"),
            // entity.productSerialOutbound.shippinginvoiceno
            new TranslationSeedItem("entity.productSerialOutbound.shippinginvoiceno", "ja-JP", "出货发票号", "出货发票号"),
            // entity.productSerialOutbound.shippinginvoiceno
            new TranslationSeedItem("entity.productSerialOutbound.shippinginvoiceno", "zh-CN", "出货发票号", "出货发票号"),
            // entity.productSerialOutbound.shippinginvoiceno
            new TranslationSeedItem("entity.productSerialOutbound.shippinginvoiceno", "zh-HK", "出货发票号", "出货发票号"),

            // entity.productSerialOutbound.outbounddate
            new TranslationSeedItem("entity.productSerialOutbound.outbounddate", "en-US", "出库日期", "出库日期"),
            // entity.productSerialOutbound.outbounddate
            new TranslationSeedItem("entity.productSerialOutbound.outbounddate", "ja-JP", "出库日期", "出库日期"),
            // entity.productSerialOutbound.outbounddate
            new TranslationSeedItem("entity.productSerialOutbound.outbounddate", "zh-CN", "出库日期", "出库日期"),
            // entity.productSerialOutbound.outbounddate
            new TranslationSeedItem("entity.productSerialOutbound.outbounddate", "zh-HK", "出库日期", "出库日期"),

            // entity.productSerialOutbound.destination
            new TranslationSeedItem("entity.productSerialOutbound.destination", "en-US", "仕向地", "仕向地(目的地)"),
            // entity.productSerialOutbound.destination
            new TranslationSeedItem("entity.productSerialOutbound.destination", "ja-JP", "仕向地", "仕向地(目的地)"),
            // entity.productSerialOutbound.destination
            new TranslationSeedItem("entity.productSerialOutbound.destination", "zh-CN", "仕向地", "仕向地(目的地)"),
            // entity.productSerialOutbound.destination
            new TranslationSeedItem("entity.productSerialOutbound.destination", "zh-HK", "仕向地", "仕向地(目的地)"),

            // entity.productSerialOutbound.shippingmethod
            new TranslationSeedItem("entity.productSerialOutbound.shippingmethod", "en-US", "运输方式", "运输方式(0=海运,1=空运,2=陆运,3=铁路,4=快递,5=其他)"),
            // entity.productSerialOutbound.shippingmethod
            new TranslationSeedItem("entity.productSerialOutbound.shippingmethod", "ja-JP", "运输方式", "运输方式(0=海运,1=空运,2=陆运,3=铁路,4=快递,5=其他)"),
            // entity.productSerialOutbound.shippingmethod
            new TranslationSeedItem("entity.productSerialOutbound.shippingmethod", "zh-CN", "运输方式", "运输方式(0=海运,1=空运,2=陆运,3=铁路,4=快递,5=其他)"),
            // entity.productSerialOutbound.shippingmethod
            new TranslationSeedItem("entity.productSerialOutbound.shippingmethod", "zh-HK", "运输方式", "运输方式(0=海运,1=空运,2=陆运,3=铁路,4=快递,5=其他)"),

            // entity.productSerialOutbound.destinationport
            new TranslationSeedItem("entity.productSerialOutbound.destinationport", "en-US", "目的地港", "目的地港"),
            // entity.productSerialOutbound.destinationport
            new TranslationSeedItem("entity.productSerialOutbound.destinationport", "ja-JP", "目的地港", "目的地港"),
            // entity.productSerialOutbound.destinationport
            new TranslationSeedItem("entity.productSerialOutbound.destinationport", "zh-CN", "目的地港", "目的地港"),
            // entity.productSerialOutbound.destinationport
            new TranslationSeedItem("entity.productSerialOutbound.destinationport", "zh-HK", "目的地港", "目的地港"),

            // entity.productSerialOutbound.outboundtype
            new TranslationSeedItem("entity.productSerialOutbound.outboundtype", "en-US", "出库类型", "出库类型(0=销售出库,1=生产领料,2=退货出库,3=调拨出库,4=报废出库,5=序列号出库,6=其他)"),
            // entity.productSerialOutbound.outboundtype
            new TranslationSeedItem("entity.productSerialOutbound.outboundtype", "ja-JP", "出库类型", "出库类型(0=销售出库,1=生产领料,2=退货出库,3=调拨出库,4=报废出库,5=序列号出库,6=其他)"),
            // entity.productSerialOutbound.outboundtype
            new TranslationSeedItem("entity.productSerialOutbound.outboundtype", "zh-CN", "出库类型", "出库类型(0=销售出库,1=生产领料,2=退货出库,3=调拨出库,4=报废出库,5=序列号出库,6=其他)"),
            // entity.productSerialOutbound.outboundtype
            new TranslationSeedItem("entity.productSerialOutbound.outboundtype", "zh-HK", "出库类型", "出库类型(0=销售出库,1=生产领料,2=退货出库,3=调拨出库,4=报废出库,5=序列号出库,6=其他)"),

            // entity.productSerialOutbound.warehousecode
            new TranslationSeedItem("entity.productSerialOutbound.warehousecode", "en-US", "仓库编码", "仓库编码"),
            // entity.productSerialOutbound.warehousecode
            new TranslationSeedItem("entity.productSerialOutbound.warehousecode", "ja-JP", "仓库编码", "仓库编码"),
            // entity.productSerialOutbound.warehousecode
            new TranslationSeedItem("entity.productSerialOutbound.warehousecode", "zh-CN", "仓库编码", "仓库编码"),
            // entity.productSerialOutbound.warehousecode
            new TranslationSeedItem("entity.productSerialOutbound.warehousecode", "zh-HK", "仓库编码", "仓库编码"),

            // entity.productSerialOutbound.locationcode
            new TranslationSeedItem("entity.productSerialOutbound.locationcode", "en-US", "库位编码", "库位编码"),
            // entity.productSerialOutbound.locationcode
            new TranslationSeedItem("entity.productSerialOutbound.locationcode", "ja-JP", "库位编码", "库位编码"),
            // entity.productSerialOutbound.locationcode
            new TranslationSeedItem("entity.productSerialOutbound.locationcode", "zh-CN", "库位编码", "库位编码"),
            // entity.productSerialOutbound.locationcode
            new TranslationSeedItem("entity.productSerialOutbound.locationcode", "zh-HK", "库位编码", "库位编码"),

            // entity.productSerialOutbound.relatedcompany
            new TranslationSeedItem("entity.productSerialOutbound.relatedcompany", "en-US", "关联公司", "关联公司"),
            // entity.productSerialOutbound.relatedcompany
            new TranslationSeedItem("entity.productSerialOutbound.relatedcompany", "ja-JP", "关联公司", "关联公司"),
            // entity.productSerialOutbound.relatedcompany
            new TranslationSeedItem("entity.productSerialOutbound.relatedcompany", "zh-CN", "关联公司", "关联公司"),
            // entity.productSerialOutbound.relatedcompany
            new TranslationSeedItem("entity.productSerialOutbound.relatedcompany", "zh-HK", "关联公司", "关联公司"),

            // entity.productSerialOutbound.totalquantity
            new TranslationSeedItem("entity.productSerialOutbound.totalquantity", "en-US", "总数量", "总数量"),
            // entity.productSerialOutbound.totalquantity
            new TranslationSeedItem("entity.productSerialOutbound.totalquantity", "ja-JP", "总数量", "总数量"),
            // entity.productSerialOutbound.totalquantity
            new TranslationSeedItem("entity.productSerialOutbound.totalquantity", "zh-CN", "总数量", "总数量"),
            // entity.productSerialOutbound.totalquantity
            new TranslationSeedItem("entity.productSerialOutbound.totalquantity", "zh-HK", "总数量", "总数量"),

            // entity.productSerialOutbound.items
            new TranslationSeedItem("entity.productSerialOutbound.items", "en-US", "产品序列号出库明细列表", "产品序列号出库明细列表（主子表关系）"),
            // entity.productSerialOutbound.items
            new TranslationSeedItem("entity.productSerialOutbound.items", "ja-JP", "产品序列号出库明细列表", "产品序列号出库明细列表（主子表关系）"),
            // entity.productSerialOutbound.items
            new TranslationSeedItem("entity.productSerialOutbound.items", "zh-CN", "产品序列号出库明细列表", "产品序列号出库明细列表（主子表关系）"),
            // entity.productSerialOutbound.items
            new TranslationSeedItem("entity.productSerialOutbound.items", "zh-HK", "产品序列号出库明细列表", "产品序列号出库明细列表（主子表关系）"),
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
