// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Materials
// 文件名称：TaktMaterialPlantI18nSeedData.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktMaterialPlant 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktMaterialPlant 实体国际化翻译种子（键前缀 entity.materialplant.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktMaterialPlantI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktMaterialPlant 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 materialplant 实体翻译...", tenantCode);

        foreach (var item in GetMaterialPlantTranslations())
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

        TaktLogger.Information("TaktMaterialPlant 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktMaterialPlant 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.materialplant._self / entity.materialplant.{{field}}；ResourceGroup=Materials；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetMaterialPlantTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.materialplant._self
            new TranslationSeedItem("entity.materialplant._self", "en-US", "Material Plant Information_us", "实体名称"),
            // entity.materialplant._self
            new TranslationSeedItem("entity.materialplant._self", "ja-JP", "Takt工厂物料信息_jp", "实体名称"),
            // entity.materialplant._self
            new TranslationSeedItem("entity.materialplant._self", "zh-CN", "Takt工厂物料信息", "实体名称"),
            // entity.materialplant._self
            new TranslationSeedItem("entity.materialplant._self", "zh-HK", "Takt工厂物料信息_hk", "实体名称"),

            // entity.materialplant.plantcode
            new TranslationSeedItem("entity.materialplant.plantcode", "en-US", "工厂代码_us", "工厂代码"),
            // entity.materialplant.plantcode
            new TranslationSeedItem("entity.materialplant.plantcode", "ja-JP", "工厂代码_jp", "工厂代码"),
            // entity.materialplant.plantcode
            new TranslationSeedItem("entity.materialplant.plantcode", "zh-CN", "工厂代码", "工厂代码"),
            // entity.materialplant.plantcode
            new TranslationSeedItem("entity.materialplant.plantcode", "zh-HK", "工厂代码_hk", "工厂代码"),

            // entity.materialplant.materialcode
            new TranslationSeedItem("entity.materialplant.materialcode", "en-US", "物料编码_us", "物料编码（唯一索引）"),
            // entity.materialplant.materialcode
            new TranslationSeedItem("entity.materialplant.materialcode", "ja-JP", "物料编码_jp", "物料编码（唯一索引）"),
            // entity.materialplant.materialcode
            new TranslationSeedItem("entity.materialplant.materialcode", "zh-CN", "物料编码", "物料编码（唯一索引）"),
            // entity.materialplant.materialcode
            new TranslationSeedItem("entity.materialplant.materialcode", "zh-HK", "物料编码_hk", "物料编码（唯一索引）"),

            // entity.materialplant.materialname
            new TranslationSeedItem("entity.materialplant.materialname", "en-US", "物料名称_us", "物料名称"),
            // entity.materialplant.materialname
            new TranslationSeedItem("entity.materialplant.materialname", "ja-JP", "物料名称_jp", "物料名称"),
            // entity.materialplant.materialname
            new TranslationSeedItem("entity.materialplant.materialname", "zh-CN", "物料名称", "物料名称"),
            // entity.materialplant.materialname
            new TranslationSeedItem("entity.materialplant.materialname", "zh-HK", "物料名称_hk", "物料名称"),

            // entity.materialplant.materialspecification
            new TranslationSeedItem("entity.materialplant.materialspecification", "en-US", "物料规格_us", "物料规格"),
            // entity.materialplant.materialspecification
            new TranslationSeedItem("entity.materialplant.materialspecification", "ja-JP", "物料规格_jp", "物料规格"),
            // entity.materialplant.materialspecification
            new TranslationSeedItem("entity.materialplant.materialspecification", "zh-CN", "物料规格", "物料规格"),
            // entity.materialplant.materialspecification
            new TranslationSeedItem("entity.materialplant.materialspecification", "zh-HK", "物料规格_hk", "物料规格"),

            // entity.materialplant.materialdescription
            new TranslationSeedItem("entity.materialplant.materialdescription", "en-US", "物料描述_us", "物料描述"),
            // entity.materialplant.materialdescription
            new TranslationSeedItem("entity.materialplant.materialdescription", "ja-JP", "物料描述_jp", "物料描述"),
            // entity.materialplant.materialdescription
            new TranslationSeedItem("entity.materialplant.materialdescription", "zh-CN", "物料描述", "物料描述"),
            // entity.materialplant.materialdescription
            new TranslationSeedItem("entity.materialplant.materialdescription", "zh-HK", "物料描述_hk", "物料描述"),

            // entity.materialplant.industrysector
            new TranslationSeedItem("entity.materialplant.industrysector", "en-US", "行业领域_us", "行业领域"),
            // entity.materialplant.industrysector
            new TranslationSeedItem("entity.materialplant.industrysector", "ja-JP", "行业领域_jp", "行业领域"),
            // entity.materialplant.industrysector
            new TranslationSeedItem("entity.materialplant.industrysector", "zh-CN", "行业领域", "行业领域"),
            // entity.materialplant.industrysector
            new TranslationSeedItem("entity.materialplant.industrysector", "zh-HK", "行业领域_hk", "行业领域"),

            // entity.materialplant.materialhierarchy
            new TranslationSeedItem("entity.materialplant.materialhierarchy", "en-US", "品目阶层_us", "品目阶层"),
            // entity.materialplant.materialhierarchy
            new TranslationSeedItem("entity.materialplant.materialhierarchy", "ja-JP", "品目阶层_jp", "品目阶层"),
            // entity.materialplant.materialhierarchy
            new TranslationSeedItem("entity.materialplant.materialhierarchy", "zh-CN", "品目阶层", "品目阶层"),
            // entity.materialplant.materialhierarchy
            new TranslationSeedItem("entity.materialplant.materialhierarchy", "zh-HK", "品目阶层_hk", "品目阶层"),

            // entity.materialplant.materialgroupcode
            new TranslationSeedItem("entity.materialplant.materialgroupcode", "en-US", "品目组代码_us", "品目组代码（关联 TaktMaterialGroup.MaterialGroupCode）"),
            // entity.materialplant.materialgroupcode
            new TranslationSeedItem("entity.materialplant.materialgroupcode", "ja-JP", "品目组代码_jp", "品目组代码（关联 TaktMaterialGroup.MaterialGroupCode）"),
            // entity.materialplant.materialgroupcode
            new TranslationSeedItem("entity.materialplant.materialgroupcode", "zh-CN", "品目组代码", "品目组代码（关联 TaktMaterialGroup.MaterialGroupCode）"),
            // entity.materialplant.materialgroupcode
            new TranslationSeedItem("entity.materialplant.materialgroupcode", "zh-HK", "品目组代码_hk", "品目组代码（关联 TaktMaterialGroup.MaterialGroupCode）"),

            // entity.materialplant.materialtype
            new TranslationSeedItem("entity.materialplant.materialtype", "en-US", "物料类型_us", "物料类型（0=原材料，1=半成品，2=成品，3=辅料，4=包装材料，5=其他）"),
            // entity.materialplant.materialtype
            new TranslationSeedItem("entity.materialplant.materialtype", "ja-JP", "物料类型_jp", "物料类型（0=原材料，1=半成品，2=成品，3=辅料，4=包装材料，5=其他）"),
            // entity.materialplant.materialtype
            new TranslationSeedItem("entity.materialplant.materialtype", "zh-CN", "物料类型", "物料类型（0=原材料，1=半成品，2=成品，3=辅料，4=包装材料，5=其他）"),
            // entity.materialplant.materialtype
            new TranslationSeedItem("entity.materialplant.materialtype", "zh-HK", "物料类型_hk", "物料类型（0=原材料，1=半成品，2=成品，3=辅料，4=包装材料，5=其他）"),

            // entity.materialplant.materialmodel
            new TranslationSeedItem("entity.materialplant.materialmodel", "en-US", "物料型号_us", "物料型号"),
            // entity.materialplant.materialmodel
            new TranslationSeedItem("entity.materialplant.materialmodel", "ja-JP", "物料型号_jp", "物料型号"),
            // entity.materialplant.materialmodel
            new TranslationSeedItem("entity.materialplant.materialmodel", "zh-CN", "物料型号", "物料型号"),
            // entity.materialplant.materialmodel
            new TranslationSeedItem("entity.materialplant.materialmodel", "zh-HK", "物料型号_hk", "物料型号"),

            // entity.materialplant.materialbrand
            new TranslationSeedItem("entity.materialplant.materialbrand", "en-US", "物料品牌_us", "物料品牌"),
            // entity.materialplant.materialbrand
            new TranslationSeedItem("entity.materialplant.materialbrand", "ja-JP", "物料品牌_jp", "物料品牌"),
            // entity.materialplant.materialbrand
            new TranslationSeedItem("entity.materialplant.materialbrand", "zh-CN", "物料品牌", "物料品牌"),
            // entity.materialplant.materialbrand
            new TranslationSeedItem("entity.materialplant.materialbrand", "zh-HK", "物料品牌_hk", "物料品牌"),

            // entity.materialplant.baseunit
            new TranslationSeedItem("entity.materialplant.baseunit", "en-US", "基本单位_us", "基本单位（主单位）"),
            // entity.materialplant.baseunit
            new TranslationSeedItem("entity.materialplant.baseunit", "ja-JP", "基本单位_jp", "基本单位（主单位）"),
            // entity.materialplant.baseunit
            new TranslationSeedItem("entity.materialplant.baseunit", "zh-CN", "基本单位", "基本单位（主单位）"),
            // entity.materialplant.baseunit
            new TranslationSeedItem("entity.materialplant.baseunit", "zh-HK", "基本单位_hk", "基本单位（主单位）"),

            // entity.materialplant.purchasegroup
            new TranslationSeedItem("entity.materialplant.purchasegroup", "en-US", "采购组_us", "采购组编码（关联 TaktPurchaseGroup.PurchaseGroupCode）"),
            // entity.materialplant.purchasegroup
            new TranslationSeedItem("entity.materialplant.purchasegroup", "ja-JP", "采购组_jp", "采购组编码（关联 TaktPurchaseGroup.PurchaseGroupCode）"),
            // entity.materialplant.purchasegroup
            new TranslationSeedItem("entity.materialplant.purchasegroup", "zh-CN", "采购组", "采购组编码（关联 TaktPurchaseGroup.PurchaseGroupCode）"),
            // entity.materialplant.purchasegroup
            new TranslationSeedItem("entity.materialplant.purchasegroup", "zh-HK", "采购组_hk", "采购组编码（关联 TaktPurchaseGroup.PurchaseGroupCode）"),

            // entity.materialplant.purchasetype
            new TranslationSeedItem("entity.materialplant.purchasetype", "en-US", "采购类型_us", "采购类型（0=内部采购，1=外部采购，2=委外加工，3=其他）"),
            // entity.materialplant.purchasetype
            new TranslationSeedItem("entity.materialplant.purchasetype", "ja-JP", "采购类型_jp", "采购类型（0=内部采购，1=外部采购，2=委外加工，3=其他）"),
            // entity.materialplant.purchasetype
            new TranslationSeedItem("entity.materialplant.purchasetype", "zh-CN", "采购类型", "采购类型（0=内部采购，1=外部采购，2=委外加工，3=其他）"),
            // entity.materialplant.purchasetype
            new TranslationSeedItem("entity.materialplant.purchasetype", "zh-HK", "采购类型_hk", "采购类型（0=内部采购，1=外部采购，2=委外加工，3=其他）"),

            // entity.materialplant.specialprocurement
            new TranslationSeedItem("entity.materialplant.specialprocurement", "en-US", "特殊采购_us", "特殊采购（0=标准采购，1=寄售，2=库存转移，3=其他）"),
            // entity.materialplant.specialprocurement
            new TranslationSeedItem("entity.materialplant.specialprocurement", "ja-JP", "特殊采购_jp", "特殊采购（0=标准采购，1=寄售，2=库存转移，3=其他）"),
            // entity.materialplant.specialprocurement
            new TranslationSeedItem("entity.materialplant.specialprocurement", "zh-CN", "特殊采购", "特殊采购（0=标准采购，1=寄售，2=库存转移，3=其他）"),
            // entity.materialplant.specialprocurement
            new TranslationSeedItem("entity.materialplant.specialprocurement", "zh-HK", "特殊采购_hk", "特殊采购（0=标准采购，1=寄售，2=库存转移，3=其他）"),

            // entity.materialplant.isbulk
            new TranslationSeedItem("entity.materialplant.isbulk", "en-US", "是否散装_us", "是否散装（0=否，1=是）"),
            // entity.materialplant.isbulk
            new TranslationSeedItem("entity.materialplant.isbulk", "ja-JP", "是否散装_jp", "是否散装（0=否，1=是）"),
            // entity.materialplant.isbulk
            new TranslationSeedItem("entity.materialplant.isbulk", "zh-CN", "是否散装", "是否散装（0=否，1=是）"),
            // entity.materialplant.isbulk
            new TranslationSeedItem("entity.materialplant.isbulk", "zh-HK", "是否散装_hk", "是否散装（0=否，1=是）"),

            // entity.materialplant.minorderquantity
            new TranslationSeedItem("entity.materialplant.minorderquantity", "en-US", "最小起订量_us", "最小起订量（基本单位数量）"),
            // entity.materialplant.minorderquantity
            new TranslationSeedItem("entity.materialplant.minorderquantity", "ja-JP", "最小起订量_jp", "最小起订量（基本单位数量）"),
            // entity.materialplant.minorderquantity
            new TranslationSeedItem("entity.materialplant.minorderquantity", "zh-CN", "最小起订量", "最小起订量（基本单位数量）"),
            // entity.materialplant.minorderquantity
            new TranslationSeedItem("entity.materialplant.minorderquantity", "zh-HK", "最小起订量_hk", "最小起订量（基本单位数量）"),

            // entity.materialplant.roundingvalue
            new TranslationSeedItem("entity.materialplant.roundingvalue", "en-US", "舍入值_us", "舍入值（基本单位数量，用于数量舍入）"),
            // entity.materialplant.roundingvalue
            new TranslationSeedItem("entity.materialplant.roundingvalue", "ja-JP", "舍入值_jp", "舍入值（基本单位数量，用于数量舍入）"),
            // entity.materialplant.roundingvalue
            new TranslationSeedItem("entity.materialplant.roundingvalue", "zh-CN", "舍入值", "舍入值（基本单位数量，用于数量舍入）"),
            // entity.materialplant.roundingvalue
            new TranslationSeedItem("entity.materialplant.roundingvalue", "zh-HK", "舍入值_hk", "舍入值（基本单位数量，用于数量舍入）"),

            // entity.materialplant.planneddeliverytimedays
            new TranslationSeedItem("entity.materialplant.planneddeliverytimedays", "en-US", "计划交货时间_us", "计划交货时间（天数）"),
            // entity.materialplant.planneddeliverytimedays
            new TranslationSeedItem("entity.materialplant.planneddeliverytimedays", "ja-JP", "计划交货时间_jp", "计划交货时间（天数）"),
            // entity.materialplant.planneddeliverytimedays
            new TranslationSeedItem("entity.materialplant.planneddeliverytimedays", "zh-CN", "计划交货时间", "计划交货时间（天数）"),
            // entity.materialplant.planneddeliverytimedays
            new TranslationSeedItem("entity.materialplant.planneddeliverytimedays", "zh-HK", "计划交货时间_hk", "计划交货时间（天数）"),

            // entity.materialplant.inhouseproductiondays
            new TranslationSeedItem("entity.materialplant.inhouseproductiondays", "en-US", "自制生产天数_us", "自制生产天数（内部生产所需天数）"),
            // entity.materialplant.inhouseproductiondays
            new TranslationSeedItem("entity.materialplant.inhouseproductiondays", "ja-JP", "自制生产天数_jp", "自制生产天数（内部生产所需天数）"),
            // entity.materialplant.inhouseproductiondays
            new TranslationSeedItem("entity.materialplant.inhouseproductiondays", "zh-CN", "自制生产天数", "自制生产天数（内部生产所需天数）"),
            // entity.materialplant.inhouseproductiondays
            new TranslationSeedItem("entity.materialplant.inhouseproductiondays", "zh-HK", "自制生产天数_hk", "自制生产天数（内部生产所需天数）"),

            // entity.materialplant.manufacturer
            new TranslationSeedItem("entity.materialplant.manufacturer", "en-US", "制造商_us", "制造商"),
            // entity.materialplant.manufacturer
            new TranslationSeedItem("entity.materialplant.manufacturer", "ja-JP", "制造商_jp", "制造商"),
            // entity.materialplant.manufacturer
            new TranslationSeedItem("entity.materialplant.manufacturer", "zh-CN", "制造商", "制造商"),
            // entity.materialplant.manufacturer
            new TranslationSeedItem("entity.materialplant.manufacturer", "zh-HK", "制造商_hk", "制造商"),

            // entity.materialplant.manufacturerpartnumber
            new TranslationSeedItem("entity.materialplant.manufacturerpartnumber", "en-US", "制造商零件编号_us", "制造商零件编号"),
            // entity.materialplant.manufacturerpartnumber
            new TranslationSeedItem("entity.materialplant.manufacturerpartnumber", "ja-JP", "制造商零件编号_jp", "制造商零件编号"),
            // entity.materialplant.manufacturerpartnumber
            new TranslationSeedItem("entity.materialplant.manufacturerpartnumber", "zh-CN", "制造商零件编号", "制造商零件编号"),
            // entity.materialplant.manufacturerpartnumber
            new TranslationSeedItem("entity.materialplant.manufacturerpartnumber", "zh-HK", "制造商零件编号_hk", "制造商零件编号"),

            // entity.materialplant.currencycode
            new TranslationSeedItem("entity.materialplant.currencycode", "en-US", "币种代码_us", "币种代码"),
            // entity.materialplant.currencycode
            new TranslationSeedItem("entity.materialplant.currencycode", "ja-JP", "币种代码_jp", "币种代码"),
            // entity.materialplant.currencycode
            new TranslationSeedItem("entity.materialplant.currencycode", "zh-CN", "币种代码", "币种代码"),
            // entity.materialplant.currencycode
            new TranslationSeedItem("entity.materialplant.currencycode", "zh-HK", "币种代码_hk", "币种代码"),

            // entity.materialplant.pricecontrol
            new TranslationSeedItem("entity.materialplant.pricecontrol", "en-US", "价格控制_us", "价格控制（0=标准价格，1=移动平均价格，2=其他）"),
            // entity.materialplant.pricecontrol
            new TranslationSeedItem("entity.materialplant.pricecontrol", "ja-JP", "价格控制_jp", "价格控制（0=标准价格，1=移动平均价格，2=其他）"),
            // entity.materialplant.pricecontrol
            new TranslationSeedItem("entity.materialplant.pricecontrol", "zh-CN", "价格控制", "价格控制（0=标准价格，1=移动平均价格，2=其他）"),
            // entity.materialplant.pricecontrol
            new TranslationSeedItem("entity.materialplant.pricecontrol", "zh-HK", "价格控制_hk", "价格控制（0=标准价格，1=移动平均价格，2=其他）"),

            // entity.materialplant.priceunit
            new TranslationSeedItem("entity.materialplant.priceunit", "en-US", "价格单位_us", "价格单位（价格对应的单位数量，如：1表示每1个，10表示每10个）"),
            // entity.materialplant.priceunit
            new TranslationSeedItem("entity.materialplant.priceunit", "ja-JP", "价格单位_jp", "价格单位（价格对应的单位数量，如：1表示每1个，10表示每10个）"),
            // entity.materialplant.priceunit
            new TranslationSeedItem("entity.materialplant.priceunit", "zh-CN", "价格单位", "价格单位（价格对应的单位数量，如：1表示每1个，10表示每10个）"),
            // entity.materialplant.priceunit
            new TranslationSeedItem("entity.materialplant.priceunit", "zh-HK", "价格单位_hk", "价格单位（价格对应的单位数量，如：1表示每1个，10表示每10个）"),

            // entity.materialplant.valuationcategory
            new TranslationSeedItem("entity.materialplant.valuationcategory", "en-US", "评估类别代码_us", "评估类别代码"),
            // entity.materialplant.valuationcategory
            new TranslationSeedItem("entity.materialplant.valuationcategory", "ja-JP", "评估类别代码_jp", "评估类别代码"),
            // entity.materialplant.valuationcategory
            new TranslationSeedItem("entity.materialplant.valuationcategory", "zh-CN", "评估类别代码", "评估类别代码"),
            // entity.materialplant.valuationcategory
            new TranslationSeedItem("entity.materialplant.valuationcategory", "zh-HK", "评估类别代码_hk", "评估类别代码"),

            // entity.materialplant.differencecode
            new TranslationSeedItem("entity.materialplant.differencecode", "en-US", "差异码_us", "差异码"),
            // entity.materialplant.differencecode
            new TranslationSeedItem("entity.materialplant.differencecode", "ja-JP", "差异码_jp", "差异码"),
            // entity.materialplant.differencecode
            new TranslationSeedItem("entity.materialplant.differencecode", "zh-CN", "差异码", "差异码"),
            // entity.materialplant.differencecode
            new TranslationSeedItem("entity.materialplant.differencecode", "zh-HK", "差异码_hk", "差异码"),

            // entity.materialplant.profitcenter
            new TranslationSeedItem("entity.materialplant.profitcenter", "en-US", "利润中心_us", "利润中心"),
            // entity.materialplant.profitcenter
            new TranslationSeedItem("entity.materialplant.profitcenter", "ja-JP", "利润中心_jp", "利润中心"),
            // entity.materialplant.profitcenter
            new TranslationSeedItem("entity.materialplant.profitcenter", "zh-CN", "利润中心", "利润中心"),
            // entity.materialplant.profitcenter
            new TranslationSeedItem("entity.materialplant.profitcenter", "zh-HK", "利润中心_hk", "利润中心"),

            // entity.materialplant.latestpurchaseprice
            new TranslationSeedItem("entity.materialplant.latestpurchaseprice", "en-US", "最新采购价_us", "最新采购价（精确到分，存储为整数，单位为分）"),
            // entity.materialplant.latestpurchaseprice
            new TranslationSeedItem("entity.materialplant.latestpurchaseprice", "ja-JP", "最新采购价_jp", "最新采购价（精确到分，存储为整数，单位为分）"),
            // entity.materialplant.latestpurchaseprice
            new TranslationSeedItem("entity.materialplant.latestpurchaseprice", "zh-CN", "最新采购价", "最新采购价（精确到分，存储为整数，单位为分）"),
            // entity.materialplant.latestpurchaseprice
            new TranslationSeedItem("entity.materialplant.latestpurchaseprice", "zh-HK", "最新采购价_hk", "最新采购价（精确到分，存储为整数，单位为分）"),

            // entity.materialplant.salesprice
            new TranslationSeedItem("entity.materialplant.salesprice", "en-US", "销售价格_us", "销售价格（精确到分，存储为整数，单位为分）"),
            // entity.materialplant.salesprice
            new TranslationSeedItem("entity.materialplant.salesprice", "ja-JP", "销售价格_jp", "销售价格（精确到分，存储为整数，单位为分）"),
            // entity.materialplant.salesprice
            new TranslationSeedItem("entity.materialplant.salesprice", "zh-CN", "销售价格", "销售价格（精确到分，存储为整数，单位为分）"),
            // entity.materialplant.salesprice
            new TranslationSeedItem("entity.materialplant.salesprice", "zh-HK", "销售价格_hk", "销售价格（精确到分，存储为整数，单位为分）"),

            // entity.materialplant.safetystock
            new TranslationSeedItem("entity.materialplant.safetystock", "en-US", "安全库存_us", "安全库存（基本单位数量）"),
            // entity.materialplant.safetystock
            new TranslationSeedItem("entity.materialplant.safetystock", "ja-JP", "安全库存_jp", "安全库存（基本单位数量）"),
            // entity.materialplant.safetystock
            new TranslationSeedItem("entity.materialplant.safetystock", "zh-CN", "安全库存", "安全库存（基本单位数量）"),
            // entity.materialplant.safetystock
            new TranslationSeedItem("entity.materialplant.safetystock", "zh-HK", "安全库存_hk", "安全库存（基本单位数量）"),

            // entity.materialplant.maxstock
            new TranslationSeedItem("entity.materialplant.maxstock", "en-US", "最大库存_us", "最大库存（基本单位数量）"),
            // entity.materialplant.maxstock
            new TranslationSeedItem("entity.materialplant.maxstock", "ja-JP", "最大库存_jp", "最大库存（基本单位数量）"),
            // entity.materialplant.maxstock
            new TranslationSeedItem("entity.materialplant.maxstock", "zh-CN", "最大库存", "最大库存（基本单位数量）"),
            // entity.materialplant.maxstock
            new TranslationSeedItem("entity.materialplant.maxstock", "zh-HK", "最大库存_hk", "最大库存（基本单位数量）"),

            // entity.materialplant.minstock
            new TranslationSeedItem("entity.materialplant.minstock", "en-US", "最小库存_us", "最小库存（基本单位数量）"),
            // entity.materialplant.minstock
            new TranslationSeedItem("entity.materialplant.minstock", "ja-JP", "最小库存_jp", "最小库存（基本单位数量）"),
            // entity.materialplant.minstock
            new TranslationSeedItem("entity.materialplant.minstock", "zh-CN", "最小库存", "最小库存（基本单位数量）"),
            // entity.materialplant.minstock
            new TranslationSeedItem("entity.materialplant.minstock", "zh-HK", "最小库存_hk", "最小库存（基本单位数量）"),

            // entity.materialplant.currentstock
            new TranslationSeedItem("entity.materialplant.currentstock", "en-US", "当前库存_us", "当前库存（基本单位数量）"),
            // entity.materialplant.currentstock
            new TranslationSeedItem("entity.materialplant.currentstock", "ja-JP", "当前库存_jp", "当前库存（基本单位数量）"),
            // entity.materialplant.currentstock
            new TranslationSeedItem("entity.materialplant.currentstock", "zh-CN", "当前库存", "当前库存（基本单位数量）"),
            // entity.materialplant.currentstock
            new TranslationSeedItem("entity.materialplant.currentstock", "zh-HK", "当前库存_hk", "当前库存（基本单位数量）"),

            // entity.materialplant.productionlocation
            new TranslationSeedItem("entity.materialplant.productionlocation", "en-US", "生产地点_us", "生产地点"),
            // entity.materialplant.productionlocation
            new TranslationSeedItem("entity.materialplant.productionlocation", "ja-JP", "生产地点_jp", "生产地点"),
            // entity.materialplant.productionlocation
            new TranslationSeedItem("entity.materialplant.productionlocation", "zh-CN", "生产地点", "生产地点"),
            // entity.materialplant.productionlocation
            new TranslationSeedItem("entity.materialplant.productionlocation", "zh-HK", "生产地点_hk", "生产地点"),

            // entity.materialplant.purchasinglocation
            new TranslationSeedItem("entity.materialplant.purchasinglocation", "en-US", "采购地点_us", "采购地点"),
            // entity.materialplant.purchasinglocation
            new TranslationSeedItem("entity.materialplant.purchasinglocation", "ja-JP", "采购地点_jp", "采购地点"),
            // entity.materialplant.purchasinglocation
            new TranslationSeedItem("entity.materialplant.purchasinglocation", "zh-CN", "采购地点", "采购地点"),
            // entity.materialplant.purchasinglocation
            new TranslationSeedItem("entity.materialplant.purchasinglocation", "zh-HK", "采购地点_hk", "采购地点"),

            // entity.materialplant.inspectionrequired
            new TranslationSeedItem("entity.materialplant.inspectionrequired", "en-US", "是否检验_us", "是否检验（0=否，1=是）"),
            // entity.materialplant.inspectionrequired
            new TranslationSeedItem("entity.materialplant.inspectionrequired", "ja-JP", "是否检验_jp", "是否检验（0=否，1=是）"),
            // entity.materialplant.inspectionrequired
            new TranslationSeedItem("entity.materialplant.inspectionrequired", "zh-CN", "是否检验", "是否检验（0=否，1=是）"),
            // entity.materialplant.inspectionrequired
            new TranslationSeedItem("entity.materialplant.inspectionrequired", "zh-HK", "是否检验_hk", "是否检验（0=否，1=是）"),

            // entity.materialplant.isbatch
            new TranslationSeedItem("entity.materialplant.isbatch", "en-US", "是否批次管理_us", "是否批次管理（0=否，1=是）"),
            // entity.materialplant.isbatch
            new TranslationSeedItem("entity.materialplant.isbatch", "ja-JP", "是否批次管理_jp", "是否批次管理（0=否，1=是）"),
            // entity.materialplant.isbatch
            new TranslationSeedItem("entity.materialplant.isbatch", "zh-CN", "是否批次管理", "是否批次管理（0=否，1=是）"),
            // entity.materialplant.isbatch
            new TranslationSeedItem("entity.materialplant.isbatch", "zh-HK", "是否批次管理_hk", "是否批次管理（0=否，1=是）"),

            // entity.materialplant.isexpiry
            new TranslationSeedItem("entity.materialplant.isexpiry", "en-US", "是否保质期管理_us", "是否保质期管理（0=否，1=是）"),
            // entity.materialplant.isexpiry
            new TranslationSeedItem("entity.materialplant.isexpiry", "ja-JP", "是否保质期管理_jp", "是否保质期管理（0=否，1=是）"),
            // entity.materialplant.isexpiry
            new TranslationSeedItem("entity.materialplant.isexpiry", "zh-CN", "是否保质期管理", "是否保质期管理（0=否，1=是）"),
            // entity.materialplant.isexpiry
            new TranslationSeedItem("entity.materialplant.isexpiry", "zh-HK", "是否保质期管理_hk", "是否保质期管理（0=否，1=是）"),

            // entity.materialplant.expirydays
            new TranslationSeedItem("entity.materialplant.expirydays", "en-US", "保质期天数_us", "保质期天数（如果启用保质期管理）"),
            // entity.materialplant.expirydays
            new TranslationSeedItem("entity.materialplant.expirydays", "ja-JP", "保质期天数_jp", "保质期天数（如果启用保质期管理）"),
            // entity.materialplant.expirydays
            new TranslationSeedItem("entity.materialplant.expirydays", "zh-CN", "保质期天数", "保质期天数（如果启用保质期管理）"),
            // entity.materialplant.expirydays
            new TranslationSeedItem("entity.materialplant.expirydays", "zh-HK", "保质期天数_hk", "保质期天数（如果启用保质期管理）"),

            // entity.materialplant.materialstatus
            new TranslationSeedItem("entity.materialplant.materialstatus", "en-US", "物料状态_us", "物料状态（1=启用，0=禁用）"),
            // entity.materialplant.materialstatus
            new TranslationSeedItem("entity.materialplant.materialstatus", "ja-JP", "物料状态_jp", "物料状态（1=启用，0=禁用）"),
            // entity.materialplant.materialstatus
            new TranslationSeedItem("entity.materialplant.materialstatus", "zh-CN", "物料状态", "物料状态（1=启用，0=禁用）"),
            // entity.materialplant.materialstatus
            new TranslationSeedItem("entity.materialplant.materialstatus", "zh-HK", "物料状态_hk", "物料状态（1=启用，0=禁用）"),

            // entity.materialplant.materialattributes
            new TranslationSeedItem("entity.materialplant.materialattributes", "en-US", "物料属性_us", "物料属性（JSON格式，存储物料自定义属性）"),
            // entity.materialplant.materialattributes
            new TranslationSeedItem("entity.materialplant.materialattributes", "ja-JP", "物料属性_jp", "物料属性（JSON格式，存储物料自定义属性）"),
            // entity.materialplant.materialattributes
            new TranslationSeedItem("entity.materialplant.materialattributes", "zh-CN", "物料属性", "物料属性（JSON格式，存储物料自定义属性）"),
            // entity.materialplant.materialattributes
            new TranslationSeedItem("entity.materialplant.materialattributes", "zh-HK", "物料属性_hk", "物料属性（JSON格式，存储物料自定义属性）"),

            // entity.materialplant.isendoflife
            new TranslationSeedItem("entity.materialplant.isendoflife", "en-US", "停产状态_us", "停产状态（EOL，End Of Life）（01=采购/仓库已锁定，02=任务清单/BOM已锁定，Z0=计划物料，ZM=当前库存需确认，ZP=制造中止，ZQ=生产结束（产品），ZW=PC MRP对象外，ZX=PC 中介专用品，ZY=PC 断开连接(MRP对象外)，ZZ=PC 有替代物料）"),
            // entity.materialplant.isendoflife
            new TranslationSeedItem("entity.materialplant.isendoflife", "ja-JP", "停产状态_jp", "停产状态（EOL，End Of Life）（01=采购/仓库已锁定，02=任务清单/BOM已锁定，Z0=计划物料，ZM=当前库存需确认，ZP=制造中止，ZQ=生产结束（产品），ZW=PC MRP对象外，ZX=PC 中介专用品，ZY=PC 断开连接(MRP对象外)，ZZ=PC 有替代物料）"),
            // entity.materialplant.isendoflife
            new TranslationSeedItem("entity.materialplant.isendoflife", "zh-CN", "停产状态", "停产状态（EOL，End Of Life）（01=采购/仓库已锁定，02=任务清单/BOM已锁定，Z0=计划物料，ZM=当前库存需确认，ZP=制造中止，ZQ=生产结束（产品），ZW=PC MRP对象外，ZX=PC 中介专用品，ZY=PC 断开连接(MRP对象外)，ZZ=PC 有替代物料）"),
            // entity.materialplant.isendoflife
            new TranslationSeedItem("entity.materialplant.isendoflife", "zh-HK", "停产状态_hk", "停产状态（EOL，End Of Life）（01=采购/仓库已锁定，02=任务清单/BOM已锁定，Z0=计划物料，ZM=当前库存需确认，ZP=制造中止，ZQ=生产结束（产品），ZW=PC MRP对象外，ZX=PC 中介专用品，ZY=PC 断开连接(MRP对象外)，ZZ=PC 有替代物料）"),

            // entity.materialplant.endoflifedate
            new TranslationSeedItem("entity.materialplant.endoflifedate", "en-US", "停产日期_us", "停产日期"),
            // entity.materialplant.endoflifedate
            new TranslationSeedItem("entity.materialplant.endoflifedate", "ja-JP", "停产日期_jp", "停产日期"),
            // entity.materialplant.endoflifedate
            new TranslationSeedItem("entity.materialplant.endoflifedate", "zh-CN", "停产日期", "停产日期"),
            // entity.materialplant.endoflifedate
            new TranslationSeedItem("entity.materialplant.endoflifedate", "zh-HK", "停产日期_hk", "停产日期"),

            // entity.materialplant.changelogs
            new TranslationSeedItem("entity.materialplant.changelogs", "en-US", "工厂物料变更记录列表_us", "工厂物料变更记录列表（外键在子表 TaktMaterialPlantChangeLog.MaterialPlantId）"),
            // entity.materialplant.changelogs
            new TranslationSeedItem("entity.materialplant.changelogs", "ja-JP", "工厂物料变更记录列表_jp", "工厂物料变更记录列表（外键在子表 TaktMaterialPlantChangeLog.MaterialPlantId）"),
            // entity.materialplant.changelogs
            new TranslationSeedItem("entity.materialplant.changelogs", "zh-CN", "工厂物料变更记录列表", "工厂物料变更记录列表（外键在子表 TaktMaterialPlantChangeLog.MaterialPlantId）"),
            // entity.materialplant.changelogs
            new TranslationSeedItem("entity.materialplant.changelogs", "zh-HK", "工厂物料变更记录列表_hk", "工厂物料变更记录列表（外键在子表 TaktMaterialPlantChangeLog.MaterialPlantId）"),
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
