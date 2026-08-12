// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Sales
// 文件名称：TaktSalesPriceItemI18nSeedData.cs
// 创建时间：2026-08-12
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSalesPriceItem 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktSalesPriceItem 实体国际化翻译种子（键前缀 entity.salespriceitem.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSalesPriceItemI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSalesPriceItem 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 salespriceitem 实体翻译...", tenantCode);

        foreach (var item in GetSalesPriceItemTranslations())
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

        TaktLogger.Information("TaktSalesPriceItem 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSalesPriceItem 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.salespriceitem._self / entity.salespriceitem.{{field}}；ResourceGroup=Sales；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSalesPriceItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.salespriceitem._self
            new TranslationSeedItem("entity.salespriceitem._self", "en-US", "Sales Price Item Information_us", "实体名称"),
            // entity.salespriceitem._self
            new TranslationSeedItem("entity.salespriceitem._self", "ja-JP", "Takt销售价格明细信息_jp", "实体名称"),
            // entity.salespriceitem._self
            new TranslationSeedItem("entity.salespriceitem._self", "zh-CN", "Takt销售价格明细信息", "实体名称"),
            // entity.salespriceitem._self
            new TranslationSeedItem("entity.salespriceitem._self", "zh-HK", "Takt销售价格明细信息_hk", "实体名称"),

            // entity.salespriceitem.salespriceid
            new TranslationSeedItem("entity.salespriceitem.salespriceid", "en-US", "销售价格ID_us", "销售价格 ID（主子表关系；选项 TaktSalesPrices/options，DictValue=Id）"),
            // entity.salespriceitem.salespriceid
            new TranslationSeedItem("entity.salespriceitem.salespriceid", "ja-JP", "销售价格ID_jp", "销售价格 ID（主子表关系；选项 TaktSalesPrices/options，DictValue=Id）"),
            // entity.salespriceitem.salespriceid
            new TranslationSeedItem("entity.salespriceitem.salespriceid", "zh-CN", "销售价格ID", "销售价格 ID（主子表关系；选项 TaktSalesPrices/options，DictValue=Id）"),
            // entity.salespriceitem.salespriceid
            new TranslationSeedItem("entity.salespriceitem.salespriceid", "zh-HK", "销售价格ID_hk", "销售价格 ID（主子表关系；选项 TaktSalesPrices/options，DictValue=Id）"),

            // entity.salespriceitem.salespricecode
            new TranslationSeedItem("entity.salespriceitem.salespricecode", "en-US", "定价记录号_us", "定价记录号（冗余；与主表 SalesPriceCode 一致，长度 20）"),
            // entity.salespriceitem.salespricecode
            new TranslationSeedItem("entity.salespriceitem.salespricecode", "ja-JP", "定价记录号_jp", "定价记录号（冗余；与主表 SalesPriceCode 一致，长度 20）"),
            // entity.salespriceitem.salespricecode
            new TranslationSeedItem("entity.salespriceitem.salespricecode", "zh-CN", "定价记录号", "定价记录号（冗余；与主表 SalesPriceCode 一致，长度 20）"),
            // entity.salespriceitem.salespricecode
            new TranslationSeedItem("entity.salespriceitem.salespricecode", "zh-HK", "定价记录号_hk", "定价记录号（冗余；与主表 SalesPriceCode 一致，长度 20）"),

            // entity.salespriceitem.salespriceseq
            new TranslationSeedItem("entity.salespriceitem.salespriceseq", "en-US", "定价序号_us", "定价序号（项号/序号，固定步长=10）"),
            // entity.salespriceitem.salespriceseq
            new TranslationSeedItem("entity.salespriceitem.salespriceseq", "ja-JP", "定价序号_jp", "定价序号（项号/序号，固定步长=10）"),
            // entity.salespriceitem.salespriceseq
            new TranslationSeedItem("entity.salespriceitem.salespriceseq", "zh-CN", "定价序号", "定价序号（项号/序号，固定步长=10）"),
            // entity.salespriceitem.salespriceseq
            new TranslationSeedItem("entity.salespriceitem.salespriceseq", "zh-HK", "定价序号_hk", "定价序号（项号/序号，固定步长=10）"),

            // entity.salespriceitem.pricetype
            new TranslationSeedItem("entity.salespriceitem.pricetype", "en-US", "条件类型_us", "条件类型（冗余；字典 logistics_price_type；与主表 PriceType 一致，PB00/PR00/MWST/MWRK/NLXV）"),
            // entity.salespriceitem.pricetype
            new TranslationSeedItem("entity.salespriceitem.pricetype", "ja-JP", "条件类型_jp", "条件类型（冗余；字典 logistics_price_type；与主表 PriceType 一致，PB00/PR00/MWST/MWRK/NLXV）"),
            // entity.salespriceitem.pricetype
            new TranslationSeedItem("entity.salespriceitem.pricetype", "zh-CN", "条件类型", "条件类型（冗余；字典 logistics_price_type；与主表 PriceType 一致，PB00/PR00/MWST/MWRK/NLXV）"),
            // entity.salespriceitem.pricetype
            new TranslationSeedItem("entity.salespriceitem.pricetype", "zh-HK", "条件类型_hk", "条件类型（冗余；字典 logistics_price_type；与主表 PriceType 一致，PB00/PR00/MWST/MWRK/NLXV）"),

            // entity.salespriceitem.scaletype
            new TranslationSeedItem("entity.salespriceitem.scaletype", "en-US", "等级类型_us", "等级类型（字典 logistics_scale_type；A=基础等级，B=到等级，C=未使用，D=累进间隔等级）"),
            // entity.salespriceitem.scaletype
            new TranslationSeedItem("entity.salespriceitem.scaletype", "ja-JP", "等级类型_jp", "等级类型（字典 logistics_scale_type；A=基础等级，B=到等级，C=未使用，D=累进间隔等级）"),
            // entity.salespriceitem.scaletype
            new TranslationSeedItem("entity.salespriceitem.scaletype", "zh-CN", "等级类型", "等级类型（字典 logistics_scale_type；A=基础等级，B=到等级，C=未使用，D=累进间隔等级）"),
            // entity.salespriceitem.scaletype
            new TranslationSeedItem("entity.salespriceitem.scaletype", "zh-HK", "等级类型_hk", "等级类型（字典 logistics_scale_type；A=基础等级，B=到等级，C=未使用，D=累进间隔等级）"),

            // entity.salespriceitem.scalebasis
            new TranslationSeedItem("entity.salespriceitem.scalebasis", "en-US", "等级基础_us", "等级基础（字典 logistics_scale_basis；B=价值等级，C=数量规模，…）"),
            // entity.salespriceitem.scalebasis
            new TranslationSeedItem("entity.salespriceitem.scalebasis", "ja-JP", "等级基础_jp", "等级基础（字典 logistics_scale_basis；B=价值等级，C=数量规模，…）"),
            // entity.salespriceitem.scalebasis
            new TranslationSeedItem("entity.salespriceitem.scalebasis", "zh-CN", "等级基础", "等级基础（字典 logistics_scale_basis；B=价值等级，C=数量规模，…）"),
            // entity.salespriceitem.scalebasis
            new TranslationSeedItem("entity.salespriceitem.scalebasis", "zh-HK", "等级基础_hk", "等级基础（字典 logistics_scale_basis；B=价值等级，C=数量规模，…）"),

            // entity.salespriceitem.scalequantity
            new TranslationSeedItem("entity.salespriceitem.scalequantity", "en-US", "等级数量_us", "等级数量"),
            // entity.salespriceitem.scalequantity
            new TranslationSeedItem("entity.salespriceitem.scalequantity", "ja-JP", "等级数量_jp", "等级数量"),
            // entity.salespriceitem.scalequantity
            new TranslationSeedItem("entity.salespriceitem.scalequantity", "zh-CN", "等级数量", "等级数量"),
            // entity.salespriceitem.scalequantity
            new TranslationSeedItem("entity.salespriceitem.scalequantity", "zh-HK", "等级数量_hk", "等级数量"),

            // entity.salespriceitem.scaleunit
            new TranslationSeedItem("entity.salespriceitem.scaleunit", "en-US", "等级单位_us", "等级单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等）"),
            // entity.salespriceitem.scaleunit
            new TranslationSeedItem("entity.salespriceitem.scaleunit", "ja-JP", "等级单位_jp", "等级单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等）"),
            // entity.salespriceitem.scaleunit
            new TranslationSeedItem("entity.salespriceitem.scaleunit", "zh-CN", "等级单位", "等级单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等）"),
            // entity.salespriceitem.scaleunit
            new TranslationSeedItem("entity.salespriceitem.scaleunit", "zh-HK", "等级单位_hk", "等级单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等）"),

            // entity.salespriceitem.scalevalue
            new TranslationSeedItem("entity.salespriceitem.scalevalue", "en-US", "等级值_us", "等级值"),
            // entity.salespriceitem.scalevalue
            new TranslationSeedItem("entity.salespriceitem.scalevalue", "ja-JP", "等级值_jp", "等级值"),
            // entity.salespriceitem.scalevalue
            new TranslationSeedItem("entity.salespriceitem.scalevalue", "zh-CN", "等级值", "等级值"),
            // entity.salespriceitem.scalevalue
            new TranslationSeedItem("entity.salespriceitem.scalevalue", "zh-HK", "等级值_hk", "等级值"),

            // entity.salespriceitem.scalecurrencycode
            new TranslationSeedItem("entity.salespriceitem.scalecurrencycode", "en-US", "等级货币_us", "等级货币（字典 accounting_currency_code；DictValue=CNY/USD 等）"),
            // entity.salespriceitem.scalecurrencycode
            new TranslationSeedItem("entity.salespriceitem.scalecurrencycode", "ja-JP", "等级货币_jp", "等级货币（字典 accounting_currency_code；DictValue=CNY/USD 等）"),
            // entity.salespriceitem.scalecurrencycode
            new TranslationSeedItem("entity.salespriceitem.scalecurrencycode", "zh-CN", "等级货币", "等级货币（字典 accounting_currency_code；DictValue=CNY/USD 等）"),
            // entity.salespriceitem.scalecurrencycode
            new TranslationSeedItem("entity.salespriceitem.scalecurrencycode", "zh-HK", "等级货币_hk", "等级货币（字典 accounting_currency_code；DictValue=CNY/USD 等）"),

            // entity.salespriceitem.calculationtype
            new TranslationSeedItem("entity.salespriceitem.calculationtype", "en-US", "计算类型_us", "计算类型（字典 logistics_calculation_type；默认 A=百分数）"),
            // entity.salespriceitem.calculationtype
            new TranslationSeedItem("entity.salespriceitem.calculationtype", "ja-JP", "计算类型_jp", "计算类型（字典 logistics_calculation_type；默认 A=百分数）"),
            // entity.salespriceitem.calculationtype
            new TranslationSeedItem("entity.salespriceitem.calculationtype", "zh-CN", "计算类型", "计算类型（字典 logistics_calculation_type；默认 A=百分数）"),
            // entity.salespriceitem.calculationtype
            new TranslationSeedItem("entity.salespriceitem.calculationtype", "zh-HK", "计算类型_hk", "计算类型（字典 logistics_calculation_type；默认 A=百分数）"),

            // entity.salespriceitem.price
            new TranslationSeedItem("entity.salespriceitem.price", "en-US", "价格_us", "价格"),
            // entity.salespriceitem.price
            new TranslationSeedItem("entity.salespriceitem.price", "ja-JP", "价格_jp", "价格"),
            // entity.salespriceitem.price
            new TranslationSeedItem("entity.salespriceitem.price", "zh-CN", "价格", "价格"),
            // entity.salespriceitem.price
            new TranslationSeedItem("entity.salespriceitem.price", "zh-HK", "价格_hk", "价格"),

            // entity.salespriceitem.untaxedprice
            new TranslationSeedItem("entity.salespriceitem.untaxedprice", "en-US", "未税价格_us", "未税价格（冗余；可由 Price 与税码推算后回写）"),
            // entity.salespriceitem.untaxedprice
            new TranslationSeedItem("entity.salespriceitem.untaxedprice", "ja-JP", "未税价格_jp", "未税价格（冗余；可由 Price 与税码推算后回写）"),
            // entity.salespriceitem.untaxedprice
            new TranslationSeedItem("entity.salespriceitem.untaxedprice", "zh-CN", "未税价格", "未税价格（冗余；可由 Price 与税码推算后回写）"),
            // entity.salespriceitem.untaxedprice
            new TranslationSeedItem("entity.salespriceitem.untaxedprice", "zh-HK", "未税价格_hk", "未税价格（冗余；可由 Price 与税码推算后回写）"),

            // entity.salespriceitem.taxincludedprice
            new TranslationSeedItem("entity.salespriceitem.taxincludedprice", "en-US", "含税价格_us", "含税价格（冗余；可由 Price 与税码推算后回写）"),
            // entity.salespriceitem.taxincludedprice
            new TranslationSeedItem("entity.salespriceitem.taxincludedprice", "ja-JP", "含税价格_jp", "含税价格（冗余；可由 Price 与税码推算后回写）"),
            // entity.salespriceitem.taxincludedprice
            new TranslationSeedItem("entity.salespriceitem.taxincludedprice", "zh-CN", "含税价格", "含税价格（冗余；可由 Price 与税码推算后回写）"),
            // entity.salespriceitem.taxincludedprice
            new TranslationSeedItem("entity.salespriceitem.taxincludedprice", "zh-HK", "含税价格_hk", "含税价格（冗余；可由 Price 与税码推算后回写）"),

            // entity.salespriceitem.taxamount
            new TranslationSeedItem("entity.salespriceitem.taxamount", "en-US", "税费_us", "税费（冗余；含税−未税，打印用）"),
            // entity.salespriceitem.taxamount
            new TranslationSeedItem("entity.salespriceitem.taxamount", "ja-JP", "税费_jp", "税费（冗余；含税−未税，打印用）"),
            // entity.salespriceitem.taxamount
            new TranslationSeedItem("entity.salespriceitem.taxamount", "zh-CN", "税费", "税费（冗余；含税−未税，打印用）"),
            // entity.salespriceitem.taxamount
            new TranslationSeedItem("entity.salespriceitem.taxamount", "zh-HK", "税费_hk", "税费（冗余；含税−未税，打印用）"),

            // entity.salespriceitem.conditioncurrencycode
            new TranslationSeedItem("entity.salespriceitem.conditioncurrencycode", "en-US", "条件货币_us", "条件货币（字典 accounting_currency_code；DictValue=CNY/USD 等；默认 CNY）"),
            // entity.salespriceitem.conditioncurrencycode
            new TranslationSeedItem("entity.salespriceitem.conditioncurrencycode", "ja-JP", "条件货币_jp", "条件货币（字典 accounting_currency_code；DictValue=CNY/USD 等；默认 CNY）"),
            // entity.salespriceitem.conditioncurrencycode
            new TranslationSeedItem("entity.salespriceitem.conditioncurrencycode", "zh-CN", "条件货币", "条件货币（字典 accounting_currency_code；DictValue=CNY/USD 等；默认 CNY）"),
            // entity.salespriceitem.conditioncurrencycode
            new TranslationSeedItem("entity.salespriceitem.conditioncurrencycode", "zh-HK", "条件货币_hk", "条件货币（字典 accounting_currency_code；DictValue=CNY/USD 等；默认 CNY）"),

            // entity.salespriceitem.priceunit
            new TranslationSeedItem("entity.salespriceitem.priceunit", "en-US", "定价单位_us", "定价单位（字典 logistics_price_unit_param；1/10/100/1000；默认 1000）"),
            // entity.salespriceitem.priceunit
            new TranslationSeedItem("entity.salespriceitem.priceunit", "ja-JP", "定价单位_jp", "定价单位（字典 logistics_price_unit_param；1/10/100/1000；默认 1000）"),
            // entity.salespriceitem.priceunit
            new TranslationSeedItem("entity.salespriceitem.priceunit", "zh-CN", "定价单位", "定价单位（字典 logistics_price_unit_param；1/10/100/1000；默认 1000）"),
            // entity.salespriceitem.priceunit
            new TranslationSeedItem("entity.salespriceitem.priceunit", "zh-HK", "定价单位_hk", "定价单位（字典 logistics_price_unit_param；1/10/100/1000；默认 1000）"),

            // entity.salespriceitem.unitofmeasure
            new TranslationSeedItem("entity.salespriceitem.unitofmeasure", "en-US", "计量单位_us", "计量单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),
            // entity.salespriceitem.unitofmeasure
            new TranslationSeedItem("entity.salespriceitem.unitofmeasure", "ja-JP", "计量单位_jp", "计量单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),
            // entity.salespriceitem.unitofmeasure
            new TranslationSeedItem("entity.salespriceitem.unitofmeasure", "zh-CN", "计量单位", "计量单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),
            // entity.salespriceitem.unitofmeasure
            new TranslationSeedItem("entity.salespriceitem.unitofmeasure", "zh-HK", "计量单位_hk", "计量单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),

            // entity.salespriceitem.minorderquantity
            new TranslationSeedItem("entity.salespriceitem.minorderquantity", "en-US", "最小起订量_us", "最小起订量（计量单位数量，整数）"),
            // entity.salespriceitem.minorderquantity
            new TranslationSeedItem("entity.salespriceitem.minorderquantity", "ja-JP", "最小起订量_jp", "最小起订量（计量单位数量，整数）"),
            // entity.salespriceitem.minorderquantity
            new TranslationSeedItem("entity.salespriceitem.minorderquantity", "zh-CN", "最小起订量", "最小起订量（计量单位数量，整数）"),
            // entity.salespriceitem.minorderquantity
            new TranslationSeedItem("entity.salespriceitem.minorderquantity", "zh-HK", "最小起订量_hk", "最小起订量（计量单位数量，整数）"),

            // entity.salespriceitem.roundingvalue
            new TranslationSeedItem("entity.salespriceitem.roundingvalue", "en-US", "舍入值_us", "舍入值（基本单位数量，用于数量舍入，整数）"),
            // entity.salespriceitem.roundingvalue
            new TranslationSeedItem("entity.salespriceitem.roundingvalue", "ja-JP", "舍入值_jp", "舍入值（基本单位数量，用于数量舍入，整数）"),
            // entity.salespriceitem.roundingvalue
            new TranslationSeedItem("entity.salespriceitem.roundingvalue", "zh-CN", "舍入值", "舍入值（基本单位数量，用于数量舍入，整数）"),
            // entity.salespriceitem.roundingvalue
            new TranslationSeedItem("entity.salespriceitem.roundingvalue", "zh-HK", "舍入值_hk", "舍入值（基本单位数量，用于数量舍入，整数）"),

            // entity.salespriceitem.planneddeliverytimedays
            new TranslationSeedItem("entity.salespriceitem.planneddeliverytimedays", "en-US", "计划交货时间_us", "计划交货时间（天数，整数）"),
            // entity.salespriceitem.planneddeliverytimedays
            new TranslationSeedItem("entity.salespriceitem.planneddeliverytimedays", "ja-JP", "计划交货时间_jp", "计划交货时间（天数，整数）"),
            // entity.salespriceitem.planneddeliverytimedays
            new TranslationSeedItem("entity.salespriceitem.planneddeliverytimedays", "zh-CN", "计划交货时间", "计划交货时间（天数，整数）"),
            // entity.salespriceitem.planneddeliverytimedays
            new TranslationSeedItem("entity.salespriceitem.planneddeliverytimedays", "zh-HK", "计划交货时间_hk", "计划交货时间（天数，整数）"),

            // entity.salespriceitem.isobsolete
            new TranslationSeedItem("entity.salespriceitem.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.salespriceitem.isobsolete
            new TranslationSeedItem("entity.salespriceitem.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.salespriceitem.isobsolete
            new TranslationSeedItem("entity.salespriceitem.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.salespriceitem.isobsolete
            new TranslationSeedItem("entity.salespriceitem.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）"),

            // entity.salespriceitem.scalequantities
            new TranslationSeedItem("entity.salespriceitem.scalequantities", "en-US", "数量等级行列表_us", "数量等级行列表（；主子表关系）"),
            // entity.salespriceitem.scalequantities
            new TranslationSeedItem("entity.salespriceitem.scalequantities", "ja-JP", "数量等级行列表_jp", "数量等级行列表（；主子表关系）"),
            // entity.salespriceitem.scalequantities
            new TranslationSeedItem("entity.salespriceitem.scalequantities", "zh-CN", "数量等级行列表", "数量等级行列表（；主子表关系）"),
            // entity.salespriceitem.scalequantities
            new TranslationSeedItem("entity.salespriceitem.scalequantities", "zh-HK", "数量等级行列表_hk", "数量等级行列表（；主子表关系）"),

            // entity.salespriceitem.scalevalues
            new TranslationSeedItem("entity.salespriceitem.scalevalues", "en-US", "价值等级行列表_us", "价值等级行列表（；主子表关系）"),
            // entity.salespriceitem.scalevalues
            new TranslationSeedItem("entity.salespriceitem.scalevalues", "ja-JP", "价值等级行列表_jp", "价值等级行列表（；主子表关系）"),
            // entity.salespriceitem.scalevalues
            new TranslationSeedItem("entity.salespriceitem.scalevalues", "zh-CN", "价值等级行列表", "价值等级行列表（；主子表关系）"),
            // entity.salespriceitem.scalevalues
            new TranslationSeedItem("entity.salespriceitem.scalevalues", "zh-HK", "价值等级行列表_hk", "价值等级行列表（；主子表关系）"),
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
