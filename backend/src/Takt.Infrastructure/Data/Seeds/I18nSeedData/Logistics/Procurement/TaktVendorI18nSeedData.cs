// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Procurement
// 文件名称：TaktVendorI18nSeedData.cs
// 创建时间：2026-07-09
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktVendor 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Procurement;

/// <summary>
/// TaktVendor 实体国际化翻译种子（键前缀 entity.vendor.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktVendorI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktVendor 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 vendor 实体翻译...", tenantCode);

        foreach (var item in GetVendorTranslations())
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

        TaktLogger.Information("TaktVendor 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktVendor 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.vendor._self / entity.vendor.{{field}}；ResourceGroup=Procurement；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetVendorTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.vendor._self
            new TranslationSeedItem("entity.vendor._self", "en-US", "Vendor Information_us", "实体名称"),
            // entity.vendor._self
            new TranslationSeedItem("entity.vendor._self", "ja-JP", "Takt经销商信息_jp", "实体名称"),
            // entity.vendor._self
            new TranslationSeedItem("entity.vendor._self", "zh-CN", "Takt经销商信息", "实体名称"),
            // entity.vendor._self
            new TranslationSeedItem("entity.vendor._self", "zh-HK", "Takt经销商信息_hk", "实体名称"),

            // entity.vendor.plantcode
            new TranslationSeedItem("entity.vendor.plantcode", "en-US", "工厂代码_us", "工厂代码（选项 TaktPlants/options，DictValue=PlantCode）"),
            // entity.vendor.plantcode
            new TranslationSeedItem("entity.vendor.plantcode", "ja-JP", "工厂代码_jp", "工厂代码（选项 TaktPlants/options，DictValue=PlantCode）"),
            // entity.vendor.plantcode
            new TranslationSeedItem("entity.vendor.plantcode", "zh-CN", "工厂代码", "工厂代码（选项 TaktPlants/options，DictValue=PlantCode）"),
            // entity.vendor.plantcode
            new TranslationSeedItem("entity.vendor.plantcode", "zh-HK", "工厂代码_hk", "工厂代码（选项 TaktPlants/options，DictValue=PlantCode）"),

            // entity.vendor.code
            new TranslationSeedItem("entity.vendor.code", "en-US", "经销商编码_us", "经销商编码（唯一索引）"),
            // entity.vendor.code
            new TranslationSeedItem("entity.vendor.code", "ja-JP", "经销商编码_jp", "经销商编码（唯一索引）"),
            // entity.vendor.code
            new TranslationSeedItem("entity.vendor.code", "zh-CN", "经销商编码", "经销商编码（唯一索引）"),
            // entity.vendor.code
            new TranslationSeedItem("entity.vendor.code", "zh-HK", "经销商编码_hk", "经销商编码（唯一索引）"),

            // entity.vendor.name
            new TranslationSeedItem("entity.vendor.name", "en-US", "经销商名称_us", "经销商名称"),
            // entity.vendor.name
            new TranslationSeedItem("entity.vendor.name", "ja-JP", "经销商名称_jp", "经销商名称"),
            // entity.vendor.name
            new TranslationSeedItem("entity.vendor.name", "zh-CN", "经销商名称", "经销商名称"),
            // entity.vendor.name
            new TranslationSeedItem("entity.vendor.name", "zh-HK", "经销商名称_hk", "经销商名称"),

            // entity.vendor.shortname
            new TranslationSeedItem("entity.vendor.shortname", "en-US", "经销商简称_us", "经销商简称"),
            // entity.vendor.shortname
            new TranslationSeedItem("entity.vendor.shortname", "ja-JP", "经销商简称_jp", "经销商简称"),
            // entity.vendor.shortname
            new TranslationSeedItem("entity.vendor.shortname", "zh-CN", "经销商简称", "经销商简称"),
            // entity.vendor.shortname
            new TranslationSeedItem("entity.vendor.shortname", "zh-HK", "经销商简称_hk", "经销商简称"),

            // entity.vendor.type
            new TranslationSeedItem("entity.vendor.type", "en-US", "经销商类型_us", "经销商类型（字典 logistics_vendor_category；0=授权经销商，1=一般经销商，2=代理商，3=零售商，4=其他）"),
            // entity.vendor.type
            new TranslationSeedItem("entity.vendor.type", "ja-JP", "经销商类型_jp", "经销商类型（字典 logistics_vendor_category；0=授权经销商，1=一般经销商，2=代理商，3=零售商，4=其他）"),
            // entity.vendor.type
            new TranslationSeedItem("entity.vendor.type", "zh-CN", "经销商类型", "经销商类型（字典 logistics_vendor_category；0=授权经销商，1=一般经销商，2=代理商，3=零售商，4=其他）"),
            // entity.vendor.type
            new TranslationSeedItem("entity.vendor.type", "zh-HK", "经销商类型_hk", "经销商类型（字典 logistics_vendor_category；0=授权经销商，1=一般经销商，2=代理商，3=零售商，4=其他）"),

            // entity.vendor.industrysector
            new TranslationSeedItem("entity.vendor.industrysector", "en-US", "行业领域_us", "行业领域（字典 logistics_industry_sector，DictValue=A/C/M/P）"),
            // entity.vendor.industrysector
            new TranslationSeedItem("entity.vendor.industrysector", "ja-JP", "行业领域_jp", "行业领域（字典 logistics_industry_sector，DictValue=A/C/M/P）"),
            // entity.vendor.industrysector
            new TranslationSeedItem("entity.vendor.industrysector", "zh-CN", "行业领域", "行业领域（字典 logistics_industry_sector，DictValue=A/C/M/P）"),
            // entity.vendor.industrysector
            new TranslationSeedItem("entity.vendor.industrysector", "zh-HK", "行业领域_hk", "行业领域（字典 logistics_industry_sector，DictValue=A/C/M/P）"),

            // entity.vendor.taxnumber
            new TranslationSeedItem("entity.vendor.taxnumber", "en-US", "经销商标识_us", "经销商标识（税务登记证号/统一社会信用代码）"),
            // entity.vendor.taxnumber
            new TranslationSeedItem("entity.vendor.taxnumber", "ja-JP", "经销商标识_jp", "经销商标识（税务登记证号/统一社会信用代码）"),
            // entity.vendor.taxnumber
            new TranslationSeedItem("entity.vendor.taxnumber", "zh-CN", "经销商标识", "经销商标识（税务登记证号/统一社会信用代码）"),
            // entity.vendor.taxnumber
            new TranslationSeedItem("entity.vendor.taxnumber", "zh-HK", "经销商标识_hk", "经销商标识（税务登记证号/统一社会信用代码）"),

            // entity.vendor.taxrate
            new TranslationSeedItem("entity.vendor.taxrate", "en-US", "税率_us", "税率（字典 accounting_tax_rate_param；13=13%，9=9%，0=0% 等）"),
            // entity.vendor.taxrate
            new TranslationSeedItem("entity.vendor.taxrate", "ja-JP", "税率_jp", "税率（字典 accounting_tax_rate_param；13=13%，9=9%，0=0% 等）"),
            // entity.vendor.taxrate
            new TranslationSeedItem("entity.vendor.taxrate", "zh-CN", "税率", "税率（字典 accounting_tax_rate_param；13=13%，9=9%，0=0% 等）"),
            // entity.vendor.taxrate
            new TranslationSeedItem("entity.vendor.taxrate", "zh-HK", "税率_hk", "税率（字典 accounting_tax_rate_param；13=13%，9=9%，0=0% 等）"),

            // entity.vendor.registrationcountry
            new TranslationSeedItem("entity.vendor.registrationcountry", "en-US", "注册国家_us", "注册国家（ISO 3166-1 alpha-2两位代码）"),
            // entity.vendor.registrationcountry
            new TranslationSeedItem("entity.vendor.registrationcountry", "ja-JP", "注册国家_jp", "注册国家（ISO 3166-1 alpha-2两位代码）"),
            // entity.vendor.registrationcountry
            new TranslationSeedItem("entity.vendor.registrationcountry", "zh-CN", "注册国家", "注册国家（ISO 3166-1 alpha-2两位代码）"),
            // entity.vendor.registrationcountry
            new TranslationSeedItem("entity.vendor.registrationcountry", "zh-HK", "注册国家_hk", "注册国家（ISO 3166-1 alpha-2两位代码）"),

            // entity.vendor.registrationaddress1
            new TranslationSeedItem("entity.vendor.registrationaddress1", "en-US", "注册地址1_us", "注册地址1"),
            // entity.vendor.registrationaddress1
            new TranslationSeedItem("entity.vendor.registrationaddress1", "ja-JP", "注册地址1_jp", "注册地址1"),
            // entity.vendor.registrationaddress1
            new TranslationSeedItem("entity.vendor.registrationaddress1", "zh-CN", "注册地址1", "注册地址1"),
            // entity.vendor.registrationaddress1
            new TranslationSeedItem("entity.vendor.registrationaddress1", "zh-HK", "注册地址1_hk", "注册地址1"),

            // entity.vendor.registrationaddress2
            new TranslationSeedItem("entity.vendor.registrationaddress2", "en-US", "注册地址2_us", "注册地址2"),
            // entity.vendor.registrationaddress2
            new TranslationSeedItem("entity.vendor.registrationaddress2", "ja-JP", "注册地址2_jp", "注册地址2"),
            // entity.vendor.registrationaddress2
            new TranslationSeedItem("entity.vendor.registrationaddress2", "zh-CN", "注册地址2", "注册地址2"),
            // entity.vendor.registrationaddress2
            new TranslationSeedItem("entity.vendor.registrationaddress2", "zh-HK", "注册地址2_hk", "注册地址2"),

            // entity.vendor.registrationaddress3
            new TranslationSeedItem("entity.vendor.registrationaddress3", "en-US", "注册地址3_us", "注册地址3"),
            // entity.vendor.registrationaddress3
            new TranslationSeedItem("entity.vendor.registrationaddress3", "ja-JP", "注册地址3_jp", "注册地址3"),
            // entity.vendor.registrationaddress3
            new TranslationSeedItem("entity.vendor.registrationaddress3", "zh-CN", "注册地址3", "注册地址3"),
            // entity.vendor.registrationaddress3
            new TranslationSeedItem("entity.vendor.registrationaddress3", "zh-HK", "注册地址3_hk", "注册地址3"),

            // entity.vendor.phone
            new TranslationSeedItem("entity.vendor.phone", "en-US", "经销商电话_us", "经销商电话"),
            // entity.vendor.phone
            new TranslationSeedItem("entity.vendor.phone", "ja-JP", "经销商电话_jp", "经销商电话"),
            // entity.vendor.phone
            new TranslationSeedItem("entity.vendor.phone", "zh-CN", "经销商电话", "经销商电话"),
            // entity.vendor.phone
            new TranslationSeedItem("entity.vendor.phone", "zh-HK", "经销商电话_hk", "经销商电话"),

            // entity.vendor.fax
            new TranslationSeedItem("entity.vendor.fax", "en-US", "经销商传真_us", "经销商传真"),
            // entity.vendor.fax
            new TranslationSeedItem("entity.vendor.fax", "ja-JP", "经销商传真_jp", "经销商传真"),
            // entity.vendor.fax
            new TranslationSeedItem("entity.vendor.fax", "zh-CN", "经销商传真", "经销商传真"),
            // entity.vendor.fax
            new TranslationSeedItem("entity.vendor.fax", "zh-HK", "经销商传真_hk", "经销商传真"),

            // entity.vendor.email
            new TranslationSeedItem("entity.vendor.email", "en-US", "经销商邮箱_us", "经销商邮箱"),
            // entity.vendor.email
            new TranslationSeedItem("entity.vendor.email", "ja-JP", "经销商邮箱_jp", "经销商邮箱"),
            // entity.vendor.email
            new TranslationSeedItem("entity.vendor.email", "zh-CN", "经销商邮箱", "经销商邮箱"),
            // entity.vendor.email
            new TranslationSeedItem("entity.vendor.email", "zh-HK", "经销商邮箱_hk", "经销商邮箱"),

            // entity.vendor.website
            new TranslationSeedItem("entity.vendor.website", "en-US", "经销商网站_us", "经销商网站"),
            // entity.vendor.website
            new TranslationSeedItem("entity.vendor.website", "ja-JP", "经销商网站_jp", "经销商网站"),
            // entity.vendor.website
            new TranslationSeedItem("entity.vendor.website", "zh-CN", "经销商网站", "经销商网站"),
            // entity.vendor.website
            new TranslationSeedItem("entity.vendor.website", "zh-HK", "经销商网站_hk", "经销商网站"),

            // entity.vendor.contactperson
            new TranslationSeedItem("entity.vendor.contactperson", "en-US", "联系人_us", "联系人"),
            // entity.vendor.contactperson
            new TranslationSeedItem("entity.vendor.contactperson", "ja-JP", "联系人_jp", "联系人"),
            // entity.vendor.contactperson
            new TranslationSeedItem("entity.vendor.contactperson", "zh-CN", "联系人", "联系人"),
            // entity.vendor.contactperson
            new TranslationSeedItem("entity.vendor.contactperson", "zh-HK", "联系人_hk", "联系人"),

            // entity.vendor.contactphone
            new TranslationSeedItem("entity.vendor.contactphone", "en-US", "联系人电话_us", "联系人电话"),
            // entity.vendor.contactphone
            new TranslationSeedItem("entity.vendor.contactphone", "ja-JP", "联系人电话_jp", "联系人电话"),
            // entity.vendor.contactphone
            new TranslationSeedItem("entity.vendor.contactphone", "zh-CN", "联系人电话", "联系人电话"),
            // entity.vendor.contactphone
            new TranslationSeedItem("entity.vendor.contactphone", "zh-HK", "联系人电话_hk", "联系人电话"),

            // entity.vendor.contactemail
            new TranslationSeedItem("entity.vendor.contactemail", "en-US", "联系人邮箱_us", "联系人邮箱"),
            // entity.vendor.contactemail
            new TranslationSeedItem("entity.vendor.contactemail", "ja-JP", "联系人邮箱_jp", "联系人邮箱"),
            // entity.vendor.contactemail
            new TranslationSeedItem("entity.vendor.contactemail", "zh-CN", "联系人邮箱", "联系人邮箱"),
            // entity.vendor.contactemail
            new TranslationSeedItem("entity.vendor.contactemail", "zh-HK", "联系人邮箱_hk", "联系人邮箱"),

            // entity.vendor.currencycode
            new TranslationSeedItem("entity.vendor.currencycode", "en-US", "结算币种代码_us", "结算币种代码（字典 accounting_currency_code，DictValue=CNY/USD 等）"),
            // entity.vendor.currencycode
            new TranslationSeedItem("entity.vendor.currencycode", "ja-JP", "结算币种代码_jp", "结算币种代码（字典 accounting_currency_code，DictValue=CNY/USD 等）"),
            // entity.vendor.currencycode
            new TranslationSeedItem("entity.vendor.currencycode", "zh-CN", "结算币种代码", "结算币种代码（字典 accounting_currency_code，DictValue=CNY/USD 等）"),
            // entity.vendor.currencycode
            new TranslationSeedItem("entity.vendor.currencycode", "zh-HK", "结算币种代码_hk", "结算币种代码（字典 accounting_currency_code，DictValue=CNY/USD 等）"),

            // entity.vendor.paymentterms
            new TranslationSeedItem("entity.vendor.paymentterms", "en-US", "付款条件_us", "付款条件（字典 accounting_payment_terms_param；DictValue=prepayship/cod/net30 等）"),
            // entity.vendor.paymentterms
            new TranslationSeedItem("entity.vendor.paymentterms", "ja-JP", "付款条件_jp", "付款条件（字典 accounting_payment_terms_param；DictValue=prepayship/cod/net30 等）"),
            // entity.vendor.paymentterms
            new TranslationSeedItem("entity.vendor.paymentterms", "zh-CN", "付款条件", "付款条件（字典 accounting_payment_terms_param；DictValue=prepayship/cod/net30 等）"),
            // entity.vendor.paymentterms
            new TranslationSeedItem("entity.vendor.paymentterms", "zh-HK", "付款条件_hk", "付款条件（字典 accounting_payment_terms_param；DictValue=prepayship/cod/net30 等）"),

            // entity.vendor.creditlevel
            new TranslationSeedItem("entity.vendor.creditlevel", "en-US", "信用等级_us", "信用等级（字典 logistics_credit_rating_category；0=无，1=A级，2=AA级，3=AAA级，4=B级，5=C级）"),
            // entity.vendor.creditlevel
            new TranslationSeedItem("entity.vendor.creditlevel", "ja-JP", "信用等级_jp", "信用等级（字典 logistics_credit_rating_category；0=无，1=A级，2=AA级，3=AAA级，4=B级，5=C级）"),
            // entity.vendor.creditlevel
            new TranslationSeedItem("entity.vendor.creditlevel", "zh-CN", "信用等级", "信用等级（字典 logistics_credit_rating_category；0=无，1=A级，2=AA级，3=AAA级，4=B级，5=C级）"),
            // entity.vendor.creditlevel
            new TranslationSeedItem("entity.vendor.creditlevel", "zh-HK", "信用等级_hk", "信用等级（字典 logistics_credit_rating_category；0=无，1=A级，2=AA级，3=AAA级，4=B级，5=C级）"),

            // entity.vendor.creditamount
            new TranslationSeedItem("entity.vendor.creditamount", "en-US", "信用额度_us", "信用额度（精确到分，存储为整数，单位为分）"),
            // entity.vendor.creditamount
            new TranslationSeedItem("entity.vendor.creditamount", "ja-JP", "信用额度_jp", "信用额度（精确到分，存储为整数，单位为分）"),
            // entity.vendor.creditamount
            new TranslationSeedItem("entity.vendor.creditamount", "zh-CN", "信用额度", "信用额度（精确到分，存储为整数，单位为分）"),
            // entity.vendor.creditamount
            new TranslationSeedItem("entity.vendor.creditamount", "zh-HK", "信用额度_hk", "信用额度（精确到分，存储为整数，单位为分）"),

            // entity.vendor.authorizedbrand
            new TranslationSeedItem("entity.vendor.authorizedbrand", "en-US", "授权品牌_us", "授权品牌"),
            // entity.vendor.authorizedbrand
            new TranslationSeedItem("entity.vendor.authorizedbrand", "ja-JP", "授权品牌_jp", "授权品牌"),
            // entity.vendor.authorizedbrand
            new TranslationSeedItem("entity.vendor.authorizedbrand", "zh-CN", "授权品牌", "授权品牌"),
            // entity.vendor.authorizedbrand
            new TranslationSeedItem("entity.vendor.authorizedbrand", "zh-HK", "授权品牌_hk", "授权品牌"),

            // entity.vendor.agentregion
            new TranslationSeedItem("entity.vendor.agentregion", "en-US", "代理区域_us", "代理区域"),
            // entity.vendor.agentregion
            new TranslationSeedItem("entity.vendor.agentregion", "ja-JP", "代理区域_jp", "代理区域"),
            // entity.vendor.agentregion
            new TranslationSeedItem("entity.vendor.agentregion", "zh-CN", "代理区域", "代理区域"),
            // entity.vendor.agentregion
            new TranslationSeedItem("entity.vendor.agentregion", "zh-HK", "代理区域_hk", "代理区域"),

            // entity.vendor.level
            new TranslationSeedItem("entity.vendor.level", "en-US", "经销商等级_us", "经销商等级（字典 logistics_grade_category；0=普通，1=优选，2=战略，3=临时；业务「核心」对应档位 1）"),
            // entity.vendor.level
            new TranslationSeedItem("entity.vendor.level", "ja-JP", "经销商等级_jp", "经销商等级（字典 logistics_grade_category；0=普通，1=优选，2=战略，3=临时；业务「核心」对应档位 1）"),
            // entity.vendor.level
            new TranslationSeedItem("entity.vendor.level", "zh-CN", "经销商等级", "经销商等级（字典 logistics_grade_category；0=普通，1=优选，2=战略，3=临时；业务「核心」对应档位 1）"),
            // entity.vendor.level
            new TranslationSeedItem("entity.vendor.level", "zh-HK", "经销商等级_hk", "经销商等级（字典 logistics_grade_category；0=普通，1=优选，2=战略，3=临时；业务「核心」对应档位 1）"),

            // entity.vendor.evaluationscore
            new TranslationSeedItem("entity.vendor.evaluationscore", "en-US", "评价分数_us", "评价分数（0-100分）"),
            // entity.vendor.evaluationscore
            new TranslationSeedItem("entity.vendor.evaluationscore", "ja-JP", "评价分数_jp", "评价分数（0-100分）"),
            // entity.vendor.evaluationscore
            new TranslationSeedItem("entity.vendor.evaluationscore", "zh-CN", "评价分数", "评价分数（0-100分）"),
            // entity.vendor.evaluationscore
            new TranslationSeedItem("entity.vendor.evaluationscore", "zh-HK", "评价分数_hk", "评价分数（0-100分）"),

            // entity.vendor.sortorder
            new TranslationSeedItem("entity.vendor.sortorder", "en-US", "排序号_us", "排序号（越小越靠前）"),
            // entity.vendor.sortorder
            new TranslationSeedItem("entity.vendor.sortorder", "ja-JP", "排序号_jp", "排序号（越小越靠前）"),
            // entity.vendor.sortorder
            new TranslationSeedItem("entity.vendor.sortorder", "zh-CN", "排序号", "排序号（越小越靠前）"),
            // entity.vendor.sortorder
            new TranslationSeedItem("entity.vendor.sortorder", "zh-HK", "排序号_hk", "排序号（越小越靠前）"),

            // entity.vendor.status
            new TranslationSeedItem("entity.vendor.status", "en-US", "经销商状态_us", "经销商状态（字典 sys_normal_disable_status；1=启用，0=禁用）"),
            // entity.vendor.status
            new TranslationSeedItem("entity.vendor.status", "ja-JP", "经销商状态_jp", "经销商状态（字典 sys_normal_disable_status；1=启用，0=禁用）"),
            // entity.vendor.status
            new TranslationSeedItem("entity.vendor.status", "zh-CN", "经销商状态", "经销商状态（字典 sys_normal_disable_status；1=启用，0=禁用）"),
            // entity.vendor.status
            new TranslationSeedItem("entity.vendor.status", "zh-HK", "经销商状态_hk", "经销商状态（字典 sys_normal_disable_status；1=启用，0=禁用）"),
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
        translation.ResourceGroup = "Procurement";
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
