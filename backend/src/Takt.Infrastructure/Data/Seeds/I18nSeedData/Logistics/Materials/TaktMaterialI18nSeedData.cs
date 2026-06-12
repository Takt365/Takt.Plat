// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Materials
// 文件名称：TaktMaterialI18nSeedData.cs
// 创建时间：2026-06-12
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktMaterial 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktMaterial 实体国际化翻译种子（键前缀 entity.material.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktMaterialI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktMaterial 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 material 实体翻译...", tenantCode);

        foreach (var item in GetMaterialTranslations())
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

        TaktLogger.Information("TaktMaterial 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktMaterial 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.material._self / entity.material.{{field}}；ResourceGroup=4；ResourceType=0
    /// </summary>
    private static List<TranslationSeedItem> GetMaterialTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.material._self
            new TranslationSeedItem("entity.material._self", "en-US", "Material Information", "实体名称"),
            // entity.material._self
            new TranslationSeedItem("entity.material._self", "ja-JP", "Takt物料信息", "实体名称"),
            // entity.material._self
            new TranslationSeedItem("entity.material._self", "zh-CN", "Takt物料信息", "实体名称"),
            // entity.material._self
            new TranslationSeedItem("entity.material._self", "zh-HK", "Takt物料信息", "实体名称"),

            // entity.material.plantcode
            new TranslationSeedItem("entity.material.plantcode", "en-US", "工厂代码", "工厂代码"),
            // entity.material.plantcode
            new TranslationSeedItem("entity.material.plantcode", "ja-JP", "工厂代码", "工厂代码"),
            // entity.material.plantcode
            new TranslationSeedItem("entity.material.plantcode", "zh-CN", "工厂代码", "工厂代码"),
            // entity.material.plantcode
            new TranslationSeedItem("entity.material.plantcode", "zh-HK", "工厂代码", "工厂代码"),

            // entity.material.code
            new TranslationSeedItem("entity.material.code", "en-US", "物料编码", "物料编码（唯一索引）"),
            // entity.material.code
            new TranslationSeedItem("entity.material.code", "ja-JP", "物料编码", "物料编码（唯一索引）"),
            // entity.material.code
            new TranslationSeedItem("entity.material.code", "zh-CN", "物料编码", "物料编码（唯一索引）"),
            // entity.material.code
            new TranslationSeedItem("entity.material.code", "zh-HK", "物料编码", "物料编码（唯一索引）"),

            // entity.material.name
            new TranslationSeedItem("entity.material.name", "en-US", "物料名称", "物料名称"),
            // entity.material.name
            new TranslationSeedItem("entity.material.name", "ja-JP", "物料名称", "物料名称"),
            // entity.material.name
            new TranslationSeedItem("entity.material.name", "zh-CN", "物料名称", "物料名称"),
            // entity.material.name
            new TranslationSeedItem("entity.material.name", "zh-HK", "物料名称", "物料名称"),

            // entity.material.specification
            new TranslationSeedItem("entity.material.specification", "en-US", "物料规格", "物料规格"),
            // entity.material.specification
            new TranslationSeedItem("entity.material.specification", "ja-JP", "物料规格", "物料规格"),
            // entity.material.specification
            new TranslationSeedItem("entity.material.specification", "zh-CN", "物料规格", "物料规格"),
            // entity.material.specification
            new TranslationSeedItem("entity.material.specification", "zh-HK", "物料规格", "物料规格"),

            // entity.material.description
            new TranslationSeedItem("entity.material.description", "en-US", "物料描述", "物料描述"),
            // entity.material.description
            new TranslationSeedItem("entity.material.description", "ja-JP", "物料描述", "物料描述"),
            // entity.material.description
            new TranslationSeedItem("entity.material.description", "zh-CN", "物料描述", "物料描述"),
            // entity.material.description
            new TranslationSeedItem("entity.material.description", "zh-HK", "物料描述", "物料描述"),

            // entity.material.industrysector
            new TranslationSeedItem("entity.material.industrysector", "en-US", "行业领域", "行业领域"),
            // entity.material.industrysector
            new TranslationSeedItem("entity.material.industrysector", "ja-JP", "行业领域", "行业领域"),
            // entity.material.industrysector
            new TranslationSeedItem("entity.material.industrysector", "zh-CN", "行业领域", "行业领域"),
            // entity.material.industrysector
            new TranslationSeedItem("entity.material.industrysector", "zh-HK", "行业领域", "行业领域"),

            // entity.material.hierarchy
            new TranslationSeedItem("entity.material.hierarchy", "en-US", "品目阶层", "品目阶层"),
            // entity.material.hierarchy
            new TranslationSeedItem("entity.material.hierarchy", "ja-JP", "品目阶层", "品目阶层"),
            // entity.material.hierarchy
            new TranslationSeedItem("entity.material.hierarchy", "zh-CN", "品目阶层", "品目阶层"),
            // entity.material.hierarchy
            new TranslationSeedItem("entity.material.hierarchy", "zh-HK", "品目阶层", "品目阶层"),

            // entity.material.groupcode
            new TranslationSeedItem("entity.material.groupcode", "en-US", "品目组代码", "品目组代码"),
            // entity.material.groupcode
            new TranslationSeedItem("entity.material.groupcode", "ja-JP", "品目组代码", "品目组代码"),
            // entity.material.groupcode
            new TranslationSeedItem("entity.material.groupcode", "zh-CN", "品目组代码", "品目组代码"),
            // entity.material.groupcode
            new TranslationSeedItem("entity.material.groupcode", "zh-HK", "品目组代码", "品目组代码"),

            // entity.material.type
            new TranslationSeedItem("entity.material.type", "en-US", "物料类型", "物料类型（0=原材料，1=半成品，2=成品，3=辅料，4=包装材料，5=其他）"),
            // entity.material.type
            new TranslationSeedItem("entity.material.type", "ja-JP", "物料类型", "物料类型（0=原材料，1=半成品，2=成品，3=辅料，4=包装材料，5=其他）"),
            // entity.material.type
            new TranslationSeedItem("entity.material.type", "zh-CN", "物料类型", "物料类型（0=原材料，1=半成品，2=成品，3=辅料，4=包装材料，5=其他）"),
            // entity.material.type
            new TranslationSeedItem("entity.material.type", "zh-HK", "物料类型", "物料类型（0=原材料，1=半成品，2=成品，3=辅料，4=包装材料，5=其他）"),

            // entity.material.model
            new TranslationSeedItem("entity.material.model", "en-US", "物料型号", "物料型号"),
            // entity.material.model
            new TranslationSeedItem("entity.material.model", "ja-JP", "物料型号", "物料型号"),
            // entity.material.model
            new TranslationSeedItem("entity.material.model", "zh-CN", "物料型号", "物料型号"),
            // entity.material.model
            new TranslationSeedItem("entity.material.model", "zh-HK", "物料型号", "物料型号"),

            // entity.material.brand
            new TranslationSeedItem("entity.material.brand", "en-US", "物料品牌", "物料品牌"),
            // entity.material.brand
            new TranslationSeedItem("entity.material.brand", "ja-JP", "物料品牌", "物料品牌"),
            // entity.material.brand
            new TranslationSeedItem("entity.material.brand", "zh-CN", "物料品牌", "物料品牌"),
            // entity.material.brand
            new TranslationSeedItem("entity.material.brand", "zh-HK", "物料品牌", "物料品牌"),

            // entity.material.baseunit
            new TranslationSeedItem("entity.material.baseunit", "en-US", "基本单位", "基本单位（主单位）"),
            // entity.material.baseunit
            new TranslationSeedItem("entity.material.baseunit", "ja-JP", "基本单位", "基本单位（主单位）"),
            // entity.material.baseunit
            new TranslationSeedItem("entity.material.baseunit", "zh-CN", "基本单位", "基本单位（主单位）"),
            // entity.material.baseunit
            new TranslationSeedItem("entity.material.baseunit", "zh-HK", "基本单位", "基本单位（主单位）"),

            // entity.material.purchasegroup
            new TranslationSeedItem("entity.material.purchasegroup", "en-US", "采购组", "采购组"),
            // entity.material.purchasegroup
            new TranslationSeedItem("entity.material.purchasegroup", "ja-JP", "采购组", "采购组"),
            // entity.material.purchasegroup
            new TranslationSeedItem("entity.material.purchasegroup", "zh-CN", "采购组", "采购组"),
            // entity.material.purchasegroup
            new TranslationSeedItem("entity.material.purchasegroup", "zh-HK", "采购组", "采购组"),

            // entity.material.purchasetype
            new TranslationSeedItem("entity.material.purchasetype", "en-US", "采购类型", "采购类型（0=内部采购，1=外部采购，2=委外加工，3=其他）"),
            // entity.material.purchasetype
            new TranslationSeedItem("entity.material.purchasetype", "ja-JP", "采购类型", "采购类型（0=内部采购，1=外部采购，2=委外加工，3=其他）"),
            // entity.material.purchasetype
            new TranslationSeedItem("entity.material.purchasetype", "zh-CN", "采购类型", "采购类型（0=内部采购，1=外部采购，2=委外加工，3=其他）"),
            // entity.material.purchasetype
            new TranslationSeedItem("entity.material.purchasetype", "zh-HK", "采购类型", "采购类型（0=内部采购，1=外部采购，2=委外加工，3=其他）"),

            // entity.material.specialprocurement
            new TranslationSeedItem("entity.material.specialprocurement", "en-US", "特殊采购", "特殊采购（0=标准采购，1=寄售，2=库存转移，3=其他）"),
            // entity.material.specialprocurement
            new TranslationSeedItem("entity.material.specialprocurement", "ja-JP", "特殊采购", "特殊采购（0=标准采购，1=寄售，2=库存转移，3=其他）"),
            // entity.material.specialprocurement
            new TranslationSeedItem("entity.material.specialprocurement", "zh-CN", "特殊采购", "特殊采购（0=标准采购，1=寄售，2=库存转移，3=其他）"),
            // entity.material.specialprocurement
            new TranslationSeedItem("entity.material.specialprocurement", "zh-HK", "特殊采购", "特殊采购（0=标准采购，1=寄售，2=库存转移，3=其他）"),

            // entity.material.isbulk
            new TranslationSeedItem("entity.material.isbulk", "en-US", "是否散装", "是否散装（0=否，1=是）"),
            // entity.material.isbulk
            new TranslationSeedItem("entity.material.isbulk", "ja-JP", "是否散装", "是否散装（0=否，1=是）"),
            // entity.material.isbulk
            new TranslationSeedItem("entity.material.isbulk", "zh-CN", "是否散装", "是否散装（0=否，1=是）"),
            // entity.material.isbulk
            new TranslationSeedItem("entity.material.isbulk", "zh-HK", "是否散装", "是否散装（0=否，1=是）"),

            // entity.material.minorderquantity
            new TranslationSeedItem("entity.material.minorderquantity", "en-US", "最小起订量", "最小起订量（基本单位数量）"),
            // entity.material.minorderquantity
            new TranslationSeedItem("entity.material.minorderquantity", "ja-JP", "最小起订量", "最小起订量（基本单位数量）"),
            // entity.material.minorderquantity
            new TranslationSeedItem("entity.material.minorderquantity", "zh-CN", "最小起订量", "最小起订量（基本单位数量）"),
            // entity.material.minorderquantity
            new TranslationSeedItem("entity.material.minorderquantity", "zh-HK", "最小起订量", "最小起订量（基本单位数量）"),

            // entity.material.roundingvalue
            new TranslationSeedItem("entity.material.roundingvalue", "en-US", "舍入值", "舍入值（基本单位数量，用于数量舍入）"),
            // entity.material.roundingvalue
            new TranslationSeedItem("entity.material.roundingvalue", "ja-JP", "舍入值", "舍入值（基本单位数量，用于数量舍入）"),
            // entity.material.roundingvalue
            new TranslationSeedItem("entity.material.roundingvalue", "zh-CN", "舍入值", "舍入值（基本单位数量，用于数量舍入）"),
            // entity.material.roundingvalue
            new TranslationSeedItem("entity.material.roundingvalue", "zh-HK", "舍入值", "舍入值（基本单位数量，用于数量舍入）"),

            // entity.material.planneddeliverytimedays
            new TranslationSeedItem("entity.material.planneddeliverytimedays", "en-US", "计划交货时间", "计划交货时间（天数）"),
            // entity.material.planneddeliverytimedays
            new TranslationSeedItem("entity.material.planneddeliverytimedays", "ja-JP", "计划交货时间", "计划交货时间（天数）"),
            // entity.material.planneddeliverytimedays
            new TranslationSeedItem("entity.material.planneddeliverytimedays", "zh-CN", "计划交货时间", "计划交货时间（天数）"),
            // entity.material.planneddeliverytimedays
            new TranslationSeedItem("entity.material.planneddeliverytimedays", "zh-HK", "计划交货时间", "计划交货时间（天数）"),

            // entity.material.inhouseproductiondays
            new TranslationSeedItem("entity.material.inhouseproductiondays", "en-US", "自制生产天数", "自制生产天数（内部生产所需天数）"),
            // entity.material.inhouseproductiondays
            new TranslationSeedItem("entity.material.inhouseproductiondays", "ja-JP", "自制生产天数", "自制生产天数（内部生产所需天数）"),
            // entity.material.inhouseproductiondays
            new TranslationSeedItem("entity.material.inhouseproductiondays", "zh-CN", "自制生产天数", "自制生产天数（内部生产所需天数）"),
            // entity.material.inhouseproductiondays
            new TranslationSeedItem("entity.material.inhouseproductiondays", "zh-HK", "自制生产天数", "自制生产天数（内部生产所需天数）"),

            // entity.material.manufacturer
            new TranslationSeedItem("entity.material.manufacturer", "en-US", "制造商", "制造商"),
            // entity.material.manufacturer
            new TranslationSeedItem("entity.material.manufacturer", "ja-JP", "制造商", "制造商"),
            // entity.material.manufacturer
            new TranslationSeedItem("entity.material.manufacturer", "zh-CN", "制造商", "制造商"),
            // entity.material.manufacturer
            new TranslationSeedItem("entity.material.manufacturer", "zh-HK", "制造商", "制造商"),

            // entity.material.manufacturerpartnumber
            new TranslationSeedItem("entity.material.manufacturerpartnumber", "en-US", "制造商零件编号", "制造商零件编号"),
            // entity.material.manufacturerpartnumber
            new TranslationSeedItem("entity.material.manufacturerpartnumber", "ja-JP", "制造商零件编号", "制造商零件编号"),
            // entity.material.manufacturerpartnumber
            new TranslationSeedItem("entity.material.manufacturerpartnumber", "zh-CN", "制造商零件编号", "制造商零件编号"),
            // entity.material.manufacturerpartnumber
            new TranslationSeedItem("entity.material.manufacturerpartnumber", "zh-HK", "制造商零件编号", "制造商零件编号"),

            // entity.material.currencycode
            new TranslationSeedItem("entity.material.currencycode", "en-US", "币种代码", "币种代码"),
            // entity.material.currencycode
            new TranslationSeedItem("entity.material.currencycode", "ja-JP", "币种代码", "币种代码"),
            // entity.material.currencycode
            new TranslationSeedItem("entity.material.currencycode", "zh-CN", "币种代码", "币种代码"),
            // entity.material.currencycode
            new TranslationSeedItem("entity.material.currencycode", "zh-HK", "币种代码", "币种代码"),

            // entity.material.pricecontrol
            new TranslationSeedItem("entity.material.pricecontrol", "en-US", "价格控制", "价格控制（0=标准价格，1=移动平均价格，2=其他）"),
            // entity.material.pricecontrol
            new TranslationSeedItem("entity.material.pricecontrol", "ja-JP", "价格控制", "价格控制（0=标准价格，1=移动平均价格，2=其他）"),
            // entity.material.pricecontrol
            new TranslationSeedItem("entity.material.pricecontrol", "zh-CN", "价格控制", "价格控制（0=标准价格，1=移动平均价格，2=其他）"),
            // entity.material.pricecontrol
            new TranslationSeedItem("entity.material.pricecontrol", "zh-HK", "价格控制", "价格控制（0=标准价格，1=移动平均价格，2=其他）"),

            // entity.material.priceunit
            new TranslationSeedItem("entity.material.priceunit", "en-US", "价格单位", "价格单位（价格对应的单位数量，如：1表示每1个，10表示每10个）"),
            // entity.material.priceunit
            new TranslationSeedItem("entity.material.priceunit", "ja-JP", "价格单位", "价格单位（价格对应的单位数量，如：1表示每1个，10表示每10个）"),
            // entity.material.priceunit
            new TranslationSeedItem("entity.material.priceunit", "zh-CN", "价格单位", "价格单位（价格对应的单位数量，如：1表示每1个，10表示每10个）"),
            // entity.material.priceunit
            new TranslationSeedItem("entity.material.priceunit", "zh-HK", "价格单位", "价格单位（价格对应的单位数量，如：1表示每1个，10表示每10个）"),

            // entity.material.valuationcategory
            new TranslationSeedItem("entity.material.valuationcategory", "en-US", "评估类别代码", "评估类别代码"),
            // entity.material.valuationcategory
            new TranslationSeedItem("entity.material.valuationcategory", "ja-JP", "评估类别代码", "评估类别代码"),
            // entity.material.valuationcategory
            new TranslationSeedItem("entity.material.valuationcategory", "zh-CN", "评估类别代码", "评估类别代码"),
            // entity.material.valuationcategory
            new TranslationSeedItem("entity.material.valuationcategory", "zh-HK", "评估类别代码", "评估类别代码"),

            // entity.material.differencecode
            new TranslationSeedItem("entity.material.differencecode", "en-US", "差异码", "差异码"),
            // entity.material.differencecode
            new TranslationSeedItem("entity.material.differencecode", "ja-JP", "差异码", "差异码"),
            // entity.material.differencecode
            new TranslationSeedItem("entity.material.differencecode", "zh-CN", "差异码", "差异码"),
            // entity.material.differencecode
            new TranslationSeedItem("entity.material.differencecode", "zh-HK", "差异码", "差异码"),

            // entity.material.profitcenter
            new TranslationSeedItem("entity.material.profitcenter", "en-US", "利润中心", "利润中心"),
            // entity.material.profitcenter
            new TranslationSeedItem("entity.material.profitcenter", "ja-JP", "利润中心", "利润中心"),
            // entity.material.profitcenter
            new TranslationSeedItem("entity.material.profitcenter", "zh-CN", "利润中心", "利润中心"),
            // entity.material.profitcenter
            new TranslationSeedItem("entity.material.profitcenter", "zh-HK", "利润中心", "利润中心"),

            // entity.material.latestpurchaseprice
            new TranslationSeedItem("entity.material.latestpurchaseprice", "en-US", "最新采购价", "最新采购价（精确到分，存储为整数，单位为分）"),
            // entity.material.latestpurchaseprice
            new TranslationSeedItem("entity.material.latestpurchaseprice", "ja-JP", "最新采购价", "最新采购价（精确到分，存储为整数，单位为分）"),
            // entity.material.latestpurchaseprice
            new TranslationSeedItem("entity.material.latestpurchaseprice", "zh-CN", "最新采购价", "最新采购价（精确到分，存储为整数，单位为分）"),
            // entity.material.latestpurchaseprice
            new TranslationSeedItem("entity.material.latestpurchaseprice", "zh-HK", "最新采购价", "最新采购价（精确到分，存储为整数，单位为分）"),

            // entity.material.salesprice
            new TranslationSeedItem("entity.material.salesprice", "en-US", "销售价格", "销售价格（精确到分，存储为整数，单位为分）"),
            // entity.material.salesprice
            new TranslationSeedItem("entity.material.salesprice", "ja-JP", "销售价格", "销售价格（精确到分，存储为整数，单位为分）"),
            // entity.material.salesprice
            new TranslationSeedItem("entity.material.salesprice", "zh-CN", "销售价格", "销售价格（精确到分，存储为整数，单位为分）"),
            // entity.material.salesprice
            new TranslationSeedItem("entity.material.salesprice", "zh-HK", "销售价格", "销售价格（精确到分，存储为整数，单位为分）"),

            // entity.material.safetystock
            new TranslationSeedItem("entity.material.safetystock", "en-US", "安全库存", "安全库存（基本单位数量）"),
            // entity.material.safetystock
            new TranslationSeedItem("entity.material.safetystock", "ja-JP", "安全库存", "安全库存（基本单位数量）"),
            // entity.material.safetystock
            new TranslationSeedItem("entity.material.safetystock", "zh-CN", "安全库存", "安全库存（基本单位数量）"),
            // entity.material.safetystock
            new TranslationSeedItem("entity.material.safetystock", "zh-HK", "安全库存", "安全库存（基本单位数量）"),

            // entity.material.maxstock
            new TranslationSeedItem("entity.material.maxstock", "en-US", "最大库存", "最大库存（基本单位数量）"),
            // entity.material.maxstock
            new TranslationSeedItem("entity.material.maxstock", "ja-JP", "最大库存", "最大库存（基本单位数量）"),
            // entity.material.maxstock
            new TranslationSeedItem("entity.material.maxstock", "zh-CN", "最大库存", "最大库存（基本单位数量）"),
            // entity.material.maxstock
            new TranslationSeedItem("entity.material.maxstock", "zh-HK", "最大库存", "最大库存（基本单位数量）"),

            // entity.material.minstock
            new TranslationSeedItem("entity.material.minstock", "en-US", "最小库存", "最小库存（基本单位数量）"),
            // entity.material.minstock
            new TranslationSeedItem("entity.material.minstock", "ja-JP", "最小库存", "最小库存（基本单位数量）"),
            // entity.material.minstock
            new TranslationSeedItem("entity.material.minstock", "zh-CN", "最小库存", "最小库存（基本单位数量）"),
            // entity.material.minstock
            new TranslationSeedItem("entity.material.minstock", "zh-HK", "最小库存", "最小库存（基本单位数量）"),

            // entity.material.currentstock
            new TranslationSeedItem("entity.material.currentstock", "en-US", "当前库存", "当前库存（基本单位数量）"),
            // entity.material.currentstock
            new TranslationSeedItem("entity.material.currentstock", "ja-JP", "当前库存", "当前库存（基本单位数量）"),
            // entity.material.currentstock
            new TranslationSeedItem("entity.material.currentstock", "zh-CN", "当前库存", "当前库存（基本单位数量）"),
            // entity.material.currentstock
            new TranslationSeedItem("entity.material.currentstock", "zh-HK", "当前库存", "当前库存（基本单位数量）"),

            // entity.material.productionlocation
            new TranslationSeedItem("entity.material.productionlocation", "en-US", "生产地点", "生产地点"),
            // entity.material.productionlocation
            new TranslationSeedItem("entity.material.productionlocation", "ja-JP", "生产地点", "生产地点"),
            // entity.material.productionlocation
            new TranslationSeedItem("entity.material.productionlocation", "zh-CN", "生产地点", "生产地点"),
            // entity.material.productionlocation
            new TranslationSeedItem("entity.material.productionlocation", "zh-HK", "生产地点", "生产地点"),

            // entity.material.purchasinglocation
            new TranslationSeedItem("entity.material.purchasinglocation", "en-US", "采购地点", "采购地点"),
            // entity.material.purchasinglocation
            new TranslationSeedItem("entity.material.purchasinglocation", "ja-JP", "采购地点", "采购地点"),
            // entity.material.purchasinglocation
            new TranslationSeedItem("entity.material.purchasinglocation", "zh-CN", "采购地点", "采购地点"),
            // entity.material.purchasinglocation
            new TranslationSeedItem("entity.material.purchasinglocation", "zh-HK", "采购地点", "采购地点"),

            // entity.material.inspectionrequired
            new TranslationSeedItem("entity.material.inspectionrequired", "en-US", "是否检验", "是否检验（0=否，1=是）"),
            // entity.material.inspectionrequired
            new TranslationSeedItem("entity.material.inspectionrequired", "ja-JP", "是否检验", "是否检验（0=否，1=是）"),
            // entity.material.inspectionrequired
            new TranslationSeedItem("entity.material.inspectionrequired", "zh-CN", "是否检验", "是否检验（0=否，1=是）"),
            // entity.material.inspectionrequired
            new TranslationSeedItem("entity.material.inspectionrequired", "zh-HK", "是否检验", "是否检验（0=否，1=是）"),

            // entity.material.isbatch
            new TranslationSeedItem("entity.material.isbatch", "en-US", "是否批次管理", "是否批次管理（0=否，1=是）"),
            // entity.material.isbatch
            new TranslationSeedItem("entity.material.isbatch", "ja-JP", "是否批次管理", "是否批次管理（0=否，1=是）"),
            // entity.material.isbatch
            new TranslationSeedItem("entity.material.isbatch", "zh-CN", "是否批次管理", "是否批次管理（0=否，1=是）"),
            // entity.material.isbatch
            new TranslationSeedItem("entity.material.isbatch", "zh-HK", "是否批次管理", "是否批次管理（0=否，1=是）"),

            // entity.material.isexpiry
            new TranslationSeedItem("entity.material.isexpiry", "en-US", "是否保质期管理", "是否保质期管理（0=否，1=是）"),
            // entity.material.isexpiry
            new TranslationSeedItem("entity.material.isexpiry", "ja-JP", "是否保质期管理", "是否保质期管理（0=否，1=是）"),
            // entity.material.isexpiry
            new TranslationSeedItem("entity.material.isexpiry", "zh-CN", "是否保质期管理", "是否保质期管理（0=否，1=是）"),
            // entity.material.isexpiry
            new TranslationSeedItem("entity.material.isexpiry", "zh-HK", "是否保质期管理", "是否保质期管理（0=否，1=是）"),

            // entity.material.expirydays
            new TranslationSeedItem("entity.material.expirydays", "en-US", "保质期天数", "保质期天数（如果启用保质期管理）"),
            // entity.material.expirydays
            new TranslationSeedItem("entity.material.expirydays", "ja-JP", "保质期天数", "保质期天数（如果启用保质期管理）"),
            // entity.material.expirydays
            new TranslationSeedItem("entity.material.expirydays", "zh-CN", "保质期天数", "保质期天数（如果启用保质期管理）"),
            // entity.material.expirydays
            new TranslationSeedItem("entity.material.expirydays", "zh-HK", "保质期天数", "保质期天数（如果启用保质期管理）"),

            // entity.material.status
            new TranslationSeedItem("entity.material.status", "en-US", "物料状态", "物料状态（1=启用，0=禁用）"),
            // entity.material.status
            new TranslationSeedItem("entity.material.status", "ja-JP", "物料状态", "物料状态（1=启用，0=禁用）"),
            // entity.material.status
            new TranslationSeedItem("entity.material.status", "zh-CN", "物料状态", "物料状态（1=启用，0=禁用）"),
            // entity.material.status
            new TranslationSeedItem("entity.material.status", "zh-HK", "物料状态", "物料状态（1=启用，0=禁用）"),

            // entity.material.attributes
            new TranslationSeedItem("entity.material.attributes", "en-US", "物料属性", "物料属性（JSON格式，存储物料自定义属性）"),
            // entity.material.attributes
            new TranslationSeedItem("entity.material.attributes", "ja-JP", "物料属性", "物料属性（JSON格式，存储物料自定义属性）"),
            // entity.material.attributes
            new TranslationSeedItem("entity.material.attributes", "zh-CN", "物料属性", "物料属性（JSON格式，存储物料自定义属性）"),
            // entity.material.attributes
            new TranslationSeedItem("entity.material.attributes", "zh-HK", "物料属性", "物料属性（JSON格式，存储物料自定义属性）"),

            // entity.material.isendoflife
            new TranslationSeedItem("entity.material.isendoflife", "en-US", "停产状态", "停产状态（EOL，End Of Life）（01=采购/仓库已锁定，02=任务清单/BOM已锁定，Z0=计划物料，ZM=当前库存需确认，ZP=制造中止，ZQ=生产结束（产品），ZW=PC MRP对象外，ZX=PC 中介专用品，ZY=PC 断开连接(MRP对象外)，ZZ=PC 有替代物料）"),
            // entity.material.isendoflife
            new TranslationSeedItem("entity.material.isendoflife", "ja-JP", "停产状态", "停产状态（EOL，End Of Life）（01=采购/仓库已锁定，02=任务清单/BOM已锁定，Z0=计划物料，ZM=当前库存需确认，ZP=制造中止，ZQ=生产结束（产品），ZW=PC MRP对象外，ZX=PC 中介专用品，ZY=PC 断开连接(MRP对象外)，ZZ=PC 有替代物料）"),
            // entity.material.isendoflife
            new TranslationSeedItem("entity.material.isendoflife", "zh-CN", "停产状态", "停产状态（EOL，End Of Life）（01=采购/仓库已锁定，02=任务清单/BOM已锁定，Z0=计划物料，ZM=当前库存需确认，ZP=制造中止，ZQ=生产结束（产品），ZW=PC MRP对象外，ZX=PC 中介专用品，ZY=PC 断开连接(MRP对象外)，ZZ=PC 有替代物料）"),
            // entity.material.isendoflife
            new TranslationSeedItem("entity.material.isendoflife", "zh-HK", "停产状态", "停产状态（EOL，End Of Life）（01=采购/仓库已锁定，02=任务清单/BOM已锁定，Z0=计划物料，ZM=当前库存需确认，ZP=制造中止，ZQ=生产结束（产品），ZW=PC MRP对象外，ZX=PC 中介专用品，ZY=PC 断开连接(MRP对象外)，ZZ=PC 有替代物料）"),

            // entity.material.endoflifedate
            new TranslationSeedItem("entity.material.endoflifedate", "en-US", "停产日期", "停产日期"),
            // entity.material.endoflifedate
            new TranslationSeedItem("entity.material.endoflifedate", "ja-JP", "停产日期", "停产日期"),
            // entity.material.endoflifedate
            new TranslationSeedItem("entity.material.endoflifedate", "zh-CN", "停产日期", "停产日期"),
            // entity.material.endoflifedate
            new TranslationSeedItem("entity.material.endoflifedate", "zh-HK", "停产日期", "停产日期"),
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
