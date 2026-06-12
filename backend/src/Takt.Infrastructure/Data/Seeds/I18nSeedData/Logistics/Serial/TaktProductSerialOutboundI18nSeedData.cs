// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Serial
// 文件名称：TaktProductSerialOutboundI18nSeedData.cs
// 创建时间：2026-06-12
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
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Serial;

/// <summary>
/// TaktProductSerialOutbound 实体国际化翻译种子（键前缀 entity.productserialoutbound.*）
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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 productserialoutbound 实体翻译...", tenantCode);

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
    /// I18nKey：entity.productserialoutbound._self / entity.productserialoutbound.{{field}}；ResourceGroup=4；ResourceType=0
    /// </summary>
    private static List<TranslationSeedItem> GetProductSerialOutboundTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.productserialoutbound._self
            new TranslationSeedItem("entity.productserialoutbound._self", "en-US", "Product Serial Outbound Information", "实体名称"),
            // entity.productserialoutbound._self
            new TranslationSeedItem("entity.productserialoutbound._self", "ja-JP", "产品序列号出库主表信息", "实体名称"),
            // entity.productserialoutbound._self
            new TranslationSeedItem("entity.productserialoutbound._self", "zh-CN", "产品序列号出库主表信息", "实体名称"),
            // entity.productserialoutbound._self
            new TranslationSeedItem("entity.productserialoutbound._self", "zh-HK", "产品序列号出库主表信息", "实体名称"),

            // entity.productserialoutbound.plantcode
            new TranslationSeedItem("entity.productserialoutbound.plantcode", "en-US", "工厂代码", "工厂代码(4位字母数字组合)"),
            // entity.productserialoutbound.plantcode
            new TranslationSeedItem("entity.productserialoutbound.plantcode", "ja-JP", "工厂代码", "工厂代码(4位字母数字组合)"),
            // entity.productserialoutbound.plantcode
            new TranslationSeedItem("entity.productserialoutbound.plantcode", "zh-CN", "工厂代码", "工厂代码(4位字母数字组合)"),
            // entity.productserialoutbound.plantcode
            new TranslationSeedItem("entity.productserialoutbound.plantcode", "zh-HK", "工厂代码", "工厂代码(4位字母数字组合)"),

            // entity.productserialoutbound.outboundno
            new TranslationSeedItem("entity.productserialoutbound.outboundno", "en-US", "出库单号", "出库单号(组合唯一索引:PlantCode + OutboundNo)"),
            // entity.productserialoutbound.outboundno
            new TranslationSeedItem("entity.productserialoutbound.outboundno", "ja-JP", "出库单号", "出库单号(组合唯一索引:PlantCode + OutboundNo)"),
            // entity.productserialoutbound.outboundno
            new TranslationSeedItem("entity.productserialoutbound.outboundno", "zh-CN", "出库单号", "出库单号(组合唯一索引:PlantCode + OutboundNo)"),
            // entity.productserialoutbound.outboundno
            new TranslationSeedItem("entity.productserialoutbound.outboundno", "zh-HK", "出库单号", "出库单号(组合唯一索引:PlantCode + OutboundNo)"),

            // entity.productserialoutbound.shippinginvoiceno
            new TranslationSeedItem("entity.productserialoutbound.shippinginvoiceno", "en-US", "出货发票号", "出货发票号"),
            // entity.productserialoutbound.shippinginvoiceno
            new TranslationSeedItem("entity.productserialoutbound.shippinginvoiceno", "ja-JP", "出货发票号", "出货发票号"),
            // entity.productserialoutbound.shippinginvoiceno
            new TranslationSeedItem("entity.productserialoutbound.shippinginvoiceno", "zh-CN", "出货发票号", "出货发票号"),
            // entity.productserialoutbound.shippinginvoiceno
            new TranslationSeedItem("entity.productserialoutbound.shippinginvoiceno", "zh-HK", "出货发票号", "出货发票号"),

            // entity.productserialoutbound.outbounddate
            new TranslationSeedItem("entity.productserialoutbound.outbounddate", "en-US", "出库日期", "出库日期"),
            // entity.productserialoutbound.outbounddate
            new TranslationSeedItem("entity.productserialoutbound.outbounddate", "ja-JP", "出库日期", "出库日期"),
            // entity.productserialoutbound.outbounddate
            new TranslationSeedItem("entity.productserialoutbound.outbounddate", "zh-CN", "出库日期", "出库日期"),
            // entity.productserialoutbound.outbounddate
            new TranslationSeedItem("entity.productserialoutbound.outbounddate", "zh-HK", "出库日期", "出库日期"),

            // entity.productserialoutbound.destination
            new TranslationSeedItem("entity.productserialoutbound.destination", "en-US", "仕向地", "仕向地(目的地)"),
            // entity.productserialoutbound.destination
            new TranslationSeedItem("entity.productserialoutbound.destination", "ja-JP", "仕向地", "仕向地(目的地)"),
            // entity.productserialoutbound.destination
            new TranslationSeedItem("entity.productserialoutbound.destination", "zh-CN", "仕向地", "仕向地(目的地)"),
            // entity.productserialoutbound.destination
            new TranslationSeedItem("entity.productserialoutbound.destination", "zh-HK", "仕向地", "仕向地(目的地)"),

            // entity.productserialoutbound.shippingmethod
            new TranslationSeedItem("entity.productserialoutbound.shippingmethod", "en-US", "运输方式", "运输方式(0=海运,1=空运,2=陆运,3=铁路,4=快递,5=其他)"),
            // entity.productserialoutbound.shippingmethod
            new TranslationSeedItem("entity.productserialoutbound.shippingmethod", "ja-JP", "运输方式", "运输方式(0=海运,1=空运,2=陆运,3=铁路,4=快递,5=其他)"),
            // entity.productserialoutbound.shippingmethod
            new TranslationSeedItem("entity.productserialoutbound.shippingmethod", "zh-CN", "运输方式", "运输方式(0=海运,1=空运,2=陆运,3=铁路,4=快递,5=其他)"),
            // entity.productserialoutbound.shippingmethod
            new TranslationSeedItem("entity.productserialoutbound.shippingmethod", "zh-HK", "运输方式", "运输方式(0=海运,1=空运,2=陆运,3=铁路,4=快递,5=其他)"),

            // entity.productserialoutbound.destinationport
            new TranslationSeedItem("entity.productserialoutbound.destinationport", "en-US", "目的地港", "目的地港"),
            // entity.productserialoutbound.destinationport
            new TranslationSeedItem("entity.productserialoutbound.destinationport", "ja-JP", "目的地港", "目的地港"),
            // entity.productserialoutbound.destinationport
            new TranslationSeedItem("entity.productserialoutbound.destinationport", "zh-CN", "目的地港", "目的地港"),
            // entity.productserialoutbound.destinationport
            new TranslationSeedItem("entity.productserialoutbound.destinationport", "zh-HK", "目的地港", "目的地港"),

            // entity.productserialoutbound.outboundtype
            new TranslationSeedItem("entity.productserialoutbound.outboundtype", "en-US", "出库类型", "出库类型(0=销售出库,1=生产领料,2=退货出库,3=调拨出库,4=报废出库,5=序列号出库,6=其他)"),
            // entity.productserialoutbound.outboundtype
            new TranslationSeedItem("entity.productserialoutbound.outboundtype", "ja-JP", "出库类型", "出库类型(0=销售出库,1=生产领料,2=退货出库,3=调拨出库,4=报废出库,5=序列号出库,6=其他)"),
            // entity.productserialoutbound.outboundtype
            new TranslationSeedItem("entity.productserialoutbound.outboundtype", "zh-CN", "出库类型", "出库类型(0=销售出库,1=生产领料,2=退货出库,3=调拨出库,4=报废出库,5=序列号出库,6=其他)"),
            // entity.productserialoutbound.outboundtype
            new TranslationSeedItem("entity.productserialoutbound.outboundtype", "zh-HK", "出库类型", "出库类型(0=销售出库,1=生产领料,2=退货出库,3=调拨出库,4=报废出库,5=序列号出库,6=其他)"),

            // entity.productserialoutbound.warehousecode
            new TranslationSeedItem("entity.productserialoutbound.warehousecode", "en-US", "仓库编码", "仓库编码"),
            // entity.productserialoutbound.warehousecode
            new TranslationSeedItem("entity.productserialoutbound.warehousecode", "ja-JP", "仓库编码", "仓库编码"),
            // entity.productserialoutbound.warehousecode
            new TranslationSeedItem("entity.productserialoutbound.warehousecode", "zh-CN", "仓库编码", "仓库编码"),
            // entity.productserialoutbound.warehousecode
            new TranslationSeedItem("entity.productserialoutbound.warehousecode", "zh-HK", "仓库编码", "仓库编码"),

            // entity.productserialoutbound.locationcode
            new TranslationSeedItem("entity.productserialoutbound.locationcode", "en-US", "库位编码", "库位编码"),
            // entity.productserialoutbound.locationcode
            new TranslationSeedItem("entity.productserialoutbound.locationcode", "ja-JP", "库位编码", "库位编码"),
            // entity.productserialoutbound.locationcode
            new TranslationSeedItem("entity.productserialoutbound.locationcode", "zh-CN", "库位编码", "库位编码"),
            // entity.productserialoutbound.locationcode
            new TranslationSeedItem("entity.productserialoutbound.locationcode", "zh-HK", "库位编码", "库位编码"),

            // entity.productserialoutbound.relatedcompany
            new TranslationSeedItem("entity.productserialoutbound.relatedcompany", "en-US", "关联公司", "关联公司"),
            // entity.productserialoutbound.relatedcompany
            new TranslationSeedItem("entity.productserialoutbound.relatedcompany", "ja-JP", "关联公司", "关联公司"),
            // entity.productserialoutbound.relatedcompany
            new TranslationSeedItem("entity.productserialoutbound.relatedcompany", "zh-CN", "关联公司", "关联公司"),
            // entity.productserialoutbound.relatedcompany
            new TranslationSeedItem("entity.productserialoutbound.relatedcompany", "zh-HK", "关联公司", "关联公司"),

            // entity.productserialoutbound.totalquantity
            new TranslationSeedItem("entity.productserialoutbound.totalquantity", "en-US", "总数量", "总数量"),
            // entity.productserialoutbound.totalquantity
            new TranslationSeedItem("entity.productserialoutbound.totalquantity", "ja-JP", "总数量", "总数量"),
            // entity.productserialoutbound.totalquantity
            new TranslationSeedItem("entity.productserialoutbound.totalquantity", "zh-CN", "总数量", "总数量"),
            // entity.productserialoutbound.totalquantity
            new TranslationSeedItem("entity.productserialoutbound.totalquantity", "zh-HK", "总数量", "总数量"),

            // entity.productserialoutbound.items
            new TranslationSeedItem("entity.productserialoutbound.items", "en-US", "产品序列号出库明细列表", "产品序列号出库明细列表（主子表关系）"),
            // entity.productserialoutbound.items
            new TranslationSeedItem("entity.productserialoutbound.items", "ja-JP", "产品序列号出库明细列表", "产品序列号出库明细列表（主子表关系）"),
            // entity.productserialoutbound.items
            new TranslationSeedItem("entity.productserialoutbound.items", "zh-CN", "产品序列号出库明细列表", "产品序列号出库明细列表（主子表关系）"),
            // entity.productserialoutbound.items
            new TranslationSeedItem("entity.productserialoutbound.items", "zh-HK", "产品序列号出库明细列表", "产品序列号出库明细列表（主子表关系）"),
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
        translation.ResourceGroup = 4;
        translation.ResourceType = 0;
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
