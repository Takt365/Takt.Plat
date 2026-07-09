// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Sales
// 文件名称：TaktSalesPriceI18nSeedData.cs
// 创建时间：2026-07-09
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSalesPrice 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktSalesPrice 实体国际化翻译种子（键前缀 entity.salesprice.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSalesPriceI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSalesPrice 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 salesprice 实体翻译...", tenantCode);

        foreach (var item in GetSalesPriceTranslations())
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

        TaktLogger.Information("TaktSalesPrice 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSalesPrice 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.salesprice._self / entity.salesprice.{{field}}；ResourceGroup=Sales；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSalesPriceTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.salesprice._self
            new TranslationSeedItem("entity.salesprice._self", "en-US", "Sales Price Information_us", "实体名称"),
            // entity.salesprice._self
            new TranslationSeedItem("entity.salesprice._self", "ja-JP", "Takt销售价格信息_jp", "实体名称"),
            // entity.salesprice._self
            new TranslationSeedItem("entity.salesprice._self", "zh-CN", "Takt销售价格信息", "实体名称"),
            // entity.salesprice._self
            new TranslationSeedItem("entity.salesprice._self", "zh-HK", "Takt销售价格信息_hk", "实体名称"),

            // entity.salesprice.plantcode
            new TranslationSeedItem("entity.salesprice.plantcode", "en-US", "工厂代码_us", "工厂代码（选项 TaktPlants/options，DictValue=PlantCode）"),
            // entity.salesprice.plantcode
            new TranslationSeedItem("entity.salesprice.plantcode", "ja-JP", "工厂代码_jp", "工厂代码（选项 TaktPlants/options，DictValue=PlantCode）"),
            // entity.salesprice.plantcode
            new TranslationSeedItem("entity.salesprice.plantcode", "zh-CN", "工厂代码", "工厂代码（选项 TaktPlants/options，DictValue=PlantCode）"),
            // entity.salesprice.plantcode
            new TranslationSeedItem("entity.salesprice.plantcode", "zh-HK", "工厂代码_hk", "工厂代码（选项 TaktPlants/options，DictValue=PlantCode）"),

            // entity.salesprice.code
            new TranslationSeedItem("entity.salesprice.code", "en-US", "销售价格编码_us", "销售价格编码（唯一索引）"),
            // entity.salesprice.code
            new TranslationSeedItem("entity.salesprice.code", "ja-JP", "销售价格编码_jp", "销售价格编码（唯一索引）"),
            // entity.salesprice.code
            new TranslationSeedItem("entity.salesprice.code", "zh-CN", "销售价格编码", "销售价格编码（唯一索引）"),
            // entity.salesprice.code
            new TranslationSeedItem("entity.salesprice.code", "zh-HK", "销售价格编码_hk", "销售价格编码（唯一索引）"),

            // entity.salesprice.customercode
            new TranslationSeedItem("entity.salesprice.customercode", "en-US", "客户编码_us", "客户编码（选项 TaktCustomers/options，DictValue=CustomerCode；为空表示通用价格）"),
            // entity.salesprice.customercode
            new TranslationSeedItem("entity.salesprice.customercode", "ja-JP", "客户编码_jp", "客户编码（选项 TaktCustomers/options，DictValue=CustomerCode；为空表示通用价格）"),
            // entity.salesprice.customercode
            new TranslationSeedItem("entity.salesprice.customercode", "zh-CN", "客户编码", "客户编码（选项 TaktCustomers/options，DictValue=CustomerCode；为空表示通用价格）"),
            // entity.salesprice.customercode
            new TranslationSeedItem("entity.salesprice.customercode", "zh-HK", "客户编码_hk", "客户编码（选项 TaktCustomers/options，DictValue=CustomerCode；为空表示通用价格）"),

            // entity.salesprice.pricetype
            new TranslationSeedItem("entity.salesprice.pricetype", "en-US", "价格类型_us", "价格类型（字典 logistics_sales_price_type；SAP 定价条件类型 KSCHL，如 PR00/PB00；默认 PR00）"),
            // entity.salesprice.pricetype
            new TranslationSeedItem("entity.salesprice.pricetype", "ja-JP", "价格类型_jp", "价格类型（字典 logistics_sales_price_type；SAP 定价条件类型 KSCHL，如 PR00/PB00；默认 PR00）"),
            // entity.salesprice.pricetype
            new TranslationSeedItem("entity.salesprice.pricetype", "zh-CN", "价格类型", "价格类型（字典 logistics_sales_price_type；SAP 定价条件类型 KSCHL，如 PR00/PB00；默认 PR00）"),
            // entity.salesprice.pricetype
            new TranslationSeedItem("entity.salesprice.pricetype", "zh-HK", "价格类型_hk", "价格类型（字典 logistics_sales_price_type；SAP 定价条件类型 KSCHL，如 PR00/PB00；默认 PR00）"),

            // entity.salesprice.effectivestartdate
            new TranslationSeedItem("entity.salesprice.effectivestartdate", "en-US", "生效日期_us", "生效日期"),
            // entity.salesprice.effectivestartdate
            new TranslationSeedItem("entity.salesprice.effectivestartdate", "ja-JP", "生效日期_jp", "生效日期"),
            // entity.salesprice.effectivestartdate
            new TranslationSeedItem("entity.salesprice.effectivestartdate", "zh-CN", "生效日期", "生效日期"),
            // entity.salesprice.effectivestartdate
            new TranslationSeedItem("entity.salesprice.effectivestartdate", "zh-HK", "生效日期_hk", "生效日期"),

            // entity.salesprice.effectiveenddate
            new TranslationSeedItem("entity.salesprice.effectiveenddate", "en-US", "失效日期_us", "失效日期（空表示长期有效）"),
            // entity.salesprice.effectiveenddate
            new TranslationSeedItem("entity.salesprice.effectiveenddate", "ja-JP", "失效日期_jp", "失效日期（空表示长期有效）"),
            // entity.salesprice.effectiveenddate
            new TranslationSeedItem("entity.salesprice.effectiveenddate", "zh-CN", "失效日期", "失效日期（空表示长期有效）"),
            // entity.salesprice.effectiveenddate
            new TranslationSeedItem("entity.salesprice.effectiveenddate", "zh-HK", "失效日期_hk", "失效日期（空表示长期有效）"),

            // entity.salesprice.pricestatus
            new TranslationSeedItem("entity.salesprice.pricestatus", "en-US", "价格状态_us", "价格状态（字典 sys_normal_disable_status；1=启用 0=禁用）"),
            // entity.salesprice.pricestatus
            new TranslationSeedItem("entity.salesprice.pricestatus", "ja-JP", "价格状态_jp", "价格状态（字典 sys_normal_disable_status；1=启用 0=禁用）"),
            // entity.salesprice.pricestatus
            new TranslationSeedItem("entity.salesprice.pricestatus", "zh-CN", "价格状态", "价格状态（字典 sys_normal_disable_status；1=启用 0=禁用）"),
            // entity.salesprice.pricestatus
            new TranslationSeedItem("entity.salesprice.pricestatus", "zh-HK", "价格状态_hk", "价格状态（字典 sys_normal_disable_status；1=启用 0=禁用）"),

            // entity.salesprice.items
            new TranslationSeedItem("entity.salesprice.items", "en-US", "物料价格明细列表_us", "物料价格明细列表（主子表关系，一个客户价格可以有多个物料价格）"),
            // entity.salesprice.items
            new TranslationSeedItem("entity.salesprice.items", "ja-JP", "物料价格明细列表_jp", "物料价格明细列表（主子表关系，一个客户价格可以有多个物料价格）"),
            // entity.salesprice.items
            new TranslationSeedItem("entity.salesprice.items", "zh-CN", "物料价格明细列表", "物料价格明细列表（主子表关系，一个客户价格可以有多个物料价格）"),
            // entity.salesprice.items
            new TranslationSeedItem("entity.salesprice.items", "zh-HK", "物料价格明细列表_hk", "物料价格明细列表（主子表关系，一个客户价格可以有多个物料价格）"),
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
