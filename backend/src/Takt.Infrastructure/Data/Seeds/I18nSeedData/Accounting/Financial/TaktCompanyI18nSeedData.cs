// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Accounting.Financial
// 文件名称：TaktCompanyI18nSeedData.cs
// 创建时间：2026-07-02
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktCompany 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Accounting.Financial;

/// <summary>
/// TaktCompany 实体国际化翻译种子（键前缀 entity.company.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktCompanyI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktCompany 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 company 实体翻译...", tenantCode);

        foreach (var item in GetCompanyTranslations())
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

        TaktLogger.Information("TaktCompany 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktCompany 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.company._self / entity.company.{{field}}；ResourceGroup=Financial；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetCompanyTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.company._self
            new TranslationSeedItem("entity.company._self", "en-US", "Company Information_us", "实体名称"),
            // entity.company._self
            new TranslationSeedItem("entity.company._self", "ja-JP", "公司信息_jp", "实体名称"),
            // entity.company._self
            new TranslationSeedItem("entity.company._self", "zh-CN", "公司信息", "实体名称"),
            // entity.company._self
            new TranslationSeedItem("entity.company._self", "zh-HK", "公司信息_hk", "实体名称"),

            // entity.company.name
            new TranslationSeedItem("entity.company.name", "en-US", "公司名称_us", "公司名称"),
            // entity.company.name
            new TranslationSeedItem("entity.company.name", "ja-JP", "公司名称_jp", "公司名称"),
            // entity.company.name
            new TranslationSeedItem("entity.company.name", "zh-CN", "公司名称", "公司名称"),
            // entity.company.name
            new TranslationSeedItem("entity.company.name", "zh-HK", "公司名称_hk", "公司名称"),

            // entity.company.shortname
            new TranslationSeedItem("entity.company.shortname", "en-US", "公司简称_us", "公司简称"),
            // entity.company.shortname
            new TranslationSeedItem("entity.company.shortname", "ja-JP", "公司简称_jp", "公司简称"),
            // entity.company.shortname
            new TranslationSeedItem("entity.company.shortname", "zh-CN", "公司简称", "公司简称"),
            // entity.company.shortname
            new TranslationSeedItem("entity.company.shortname", "zh-HK", "公司简称_hk", "公司简称"),

            // entity.company.enterprisenature
            new TranslationSeedItem("entity.company.enterprisenature", "en-US", "企业性质_us", "企业性质（字典 sys_enterprise_nature_type）"),
            // entity.company.enterprisenature
            new TranslationSeedItem("entity.company.enterprisenature", "ja-JP", "企业性质_jp", "企业性质（字典 sys_enterprise_nature_type）"),
            // entity.company.enterprisenature
            new TranslationSeedItem("entity.company.enterprisenature", "zh-CN", "企业性质", "企业性质（字典 sys_enterprise_nature_type）"),
            // entity.company.enterprisenature
            new TranslationSeedItem("entity.company.enterprisenature", "zh-HK", "企业性质_hk", "企业性质（字典 sys_enterprise_nature_type）"),

            // entity.company.industryattribute
            new TranslationSeedItem("entity.company.industryattribute", "en-US", "行业属性_us", "行业属性（字典 sys_industry_attribute_type）"),
            // entity.company.industryattribute
            new TranslationSeedItem("entity.company.industryattribute", "ja-JP", "行业属性_jp", "行业属性（字典 sys_industry_attribute_type）"),
            // entity.company.industryattribute
            new TranslationSeedItem("entity.company.industryattribute", "zh-CN", "行业属性", "行业属性（字典 sys_industry_attribute_type）"),
            // entity.company.industryattribute
            new TranslationSeedItem("entity.company.industryattribute", "zh-HK", "行业属性_hk", "行业属性（字典 sys_industry_attribute_type）"),

            // entity.company.enterprisescale
            new TranslationSeedItem("entity.company.enterprisescale", "en-US", "企业规模_us", "企业规模（字典 sys_enterprise_scale_type）"),
            // entity.company.enterprisescale
            new TranslationSeedItem("entity.company.enterprisescale", "ja-JP", "企业规模_jp", "企业规模（字典 sys_enterprise_scale_type）"),
            // entity.company.enterprisescale
            new TranslationSeedItem("entity.company.enterprisescale", "zh-CN", "企业规模", "企业规模（字典 sys_enterprise_scale_type）"),
            // entity.company.enterprisescale
            new TranslationSeedItem("entity.company.enterprisescale", "zh-HK", "企业规模_hk", "企业规模（字典 sys_enterprise_scale_type）"),

            // entity.company.businessscope
            new TranslationSeedItem("entity.company.businessscope", "en-US", "经营范围_us", "经营范围"),
            // entity.company.businessscope
            new TranslationSeedItem("entity.company.businessscope", "ja-JP", "经营范围_jp", "经营范围"),
            // entity.company.businessscope
            new TranslationSeedItem("entity.company.businessscope", "zh-CN", "经营范围", "经营范围"),
            // entity.company.businessscope
            new TranslationSeedItem("entity.company.businessscope", "zh-HK", "经营范围_hk", "经营范围"),

            // entity.company.registrationaddress1
            new TranslationSeedItem("entity.company.registrationaddress1", "en-US", "注册地址1_us", "注册地址1"),
            // entity.company.registrationaddress1
            new TranslationSeedItem("entity.company.registrationaddress1", "ja-JP", "注册地址1_jp", "注册地址1"),
            // entity.company.registrationaddress1
            new TranslationSeedItem("entity.company.registrationaddress1", "zh-CN", "注册地址1", "注册地址1"),
            // entity.company.registrationaddress1
            new TranslationSeedItem("entity.company.registrationaddress1", "zh-HK", "注册地址1_hk", "注册地址1"),

            // entity.company.registrationaddress2
            new TranslationSeedItem("entity.company.registrationaddress2", "en-US", "注册地址2_us", "注册地址2"),
            // entity.company.registrationaddress2
            new TranslationSeedItem("entity.company.registrationaddress2", "ja-JP", "注册地址2_jp", "注册地址2"),
            // entity.company.registrationaddress2
            new TranslationSeedItem("entity.company.registrationaddress2", "zh-CN", "注册地址2", "注册地址2"),
            // entity.company.registrationaddress2
            new TranslationSeedItem("entity.company.registrationaddress2", "zh-HK", "注册地址2_hk", "注册地址2"),

            // entity.company.registrationaddress3
            new TranslationSeedItem("entity.company.registrationaddress3", "en-US", "注册地址3_us", "注册地址3"),
            // entity.company.registrationaddress3
            new TranslationSeedItem("entity.company.registrationaddress3", "ja-JP", "注册地址3_jp", "注册地址3"),
            // entity.company.registrationaddress3
            new TranslationSeedItem("entity.company.registrationaddress3", "zh-CN", "注册地址3", "注册地址3"),
            // entity.company.registrationaddress3
            new TranslationSeedItem("entity.company.registrationaddress3", "zh-HK", "注册地址3_hk", "注册地址3"),

            // entity.company.registrationregion
            new TranslationSeedItem("entity.company.registrationregion", "en-US", "注册国家_us", "注册国家"),
            // entity.company.registrationregion
            new TranslationSeedItem("entity.company.registrationregion", "ja-JP", "注册国家_jp", "注册国家"),
            // entity.company.registrationregion
            new TranslationSeedItem("entity.company.registrationregion", "zh-CN", "注册国家", "注册国家"),
            // entity.company.registrationregion
            new TranslationSeedItem("entity.company.registrationregion", "zh-HK", "注册国家_hk", "注册国家"),

            // entity.company.registrationprovince
            new TranslationSeedItem("entity.company.registrationprovince", "en-US", "注册省_us", "注册省"),
            // entity.company.registrationprovince
            new TranslationSeedItem("entity.company.registrationprovince", "ja-JP", "注册省_jp", "注册省"),
            // entity.company.registrationprovince
            new TranslationSeedItem("entity.company.registrationprovince", "zh-CN", "注册省", "注册省"),
            // entity.company.registrationprovince
            new TranslationSeedItem("entity.company.registrationprovince", "zh-HK", "注册省_hk", "注册省"),

            // entity.company.registrationcity
            new TranslationSeedItem("entity.company.registrationcity", "en-US", "注册市_us", "注册市"),
            // entity.company.registrationcity
            new TranslationSeedItem("entity.company.registrationcity", "ja-JP", "注册市_jp", "注册市"),
            // entity.company.registrationcity
            new TranslationSeedItem("entity.company.registrationcity", "zh-CN", "注册市", "注册市"),
            // entity.company.registrationcity
            new TranslationSeedItem("entity.company.registrationcity", "zh-HK", "注册市_hk", "注册市"),

            // entity.company.businessregion
            new TranslationSeedItem("entity.company.businessregion", "en-US", "经营国家_us", "经营国家"),
            // entity.company.businessregion
            new TranslationSeedItem("entity.company.businessregion", "ja-JP", "经营国家_jp", "经营国家"),
            // entity.company.businessregion
            new TranslationSeedItem("entity.company.businessregion", "zh-CN", "经营国家", "经营国家"),
            // entity.company.businessregion
            new TranslationSeedItem("entity.company.businessregion", "zh-HK", "经营国家_hk", "经营国家"),

            // entity.company.businessprovince
            new TranslationSeedItem("entity.company.businessprovince", "en-US", "经营地区-省_us", "经营地区-省"),
            // entity.company.businessprovince
            new TranslationSeedItem("entity.company.businessprovince", "ja-JP", "经营地区-省_jp", "经营地区-省"),
            // entity.company.businessprovince
            new TranslationSeedItem("entity.company.businessprovince", "zh-CN", "经营地区-省", "经营地区-省"),
            // entity.company.businessprovince
            new TranslationSeedItem("entity.company.businessprovince", "zh-HK", "经营地区-省_hk", "经营地区-省"),

            // entity.company.businesscity
            new TranslationSeedItem("entity.company.businesscity", "en-US", "经营地区-市_us", "经营地区-市"),
            // entity.company.businesscity
            new TranslationSeedItem("entity.company.businesscity", "ja-JP", "经营地区-市_jp", "经营地区-市"),
            // entity.company.businesscity
            new TranslationSeedItem("entity.company.businesscity", "zh-CN", "经营地区-市", "经营地区-市"),
            // entity.company.businesscity
            new TranslationSeedItem("entity.company.businesscity", "zh-HK", "经营地区-市_hk", "经营地区-市"),

            // entity.company.businessaddress1
            new TranslationSeedItem("entity.company.businessaddress1", "en-US", "经营地址1_us", "经营地址1"),
            // entity.company.businessaddress1
            new TranslationSeedItem("entity.company.businessaddress1", "ja-JP", "经营地址1_jp", "经营地址1"),
            // entity.company.businessaddress1
            new TranslationSeedItem("entity.company.businessaddress1", "zh-CN", "经营地址1", "经营地址1"),
            // entity.company.businessaddress1
            new TranslationSeedItem("entity.company.businessaddress1", "zh-HK", "经营地址1_hk", "经营地址1"),

            // entity.company.businessaddress2
            new TranslationSeedItem("entity.company.businessaddress2", "en-US", "经营地址2_us", "经营地址2"),
            // entity.company.businessaddress2
            new TranslationSeedItem("entity.company.businessaddress2", "ja-JP", "经营地址2_jp", "经营地址2"),
            // entity.company.businessaddress2
            new TranslationSeedItem("entity.company.businessaddress2", "zh-CN", "经营地址2", "经营地址2"),
            // entity.company.businessaddress2
            new TranslationSeedItem("entity.company.businessaddress2", "zh-HK", "经营地址2_hk", "经营地址2"),

            // entity.company.businessaddress3
            new TranslationSeedItem("entity.company.businessaddress3", "en-US", "经营地址3_us", "经营地址3"),
            // entity.company.businessaddress3
            new TranslationSeedItem("entity.company.businessaddress3", "ja-JP", "经营地址3_jp", "经营地址3"),
            // entity.company.businessaddress3
            new TranslationSeedItem("entity.company.businessaddress3", "zh-CN", "经营地址3", "经营地址3"),
            // entity.company.businessaddress3
            new TranslationSeedItem("entity.company.businessaddress3", "zh-HK", "经营地址3_hk", "经营地址3"),

            // entity.company.phone
            new TranslationSeedItem("entity.company.phone", "en-US", "公司电话_us", "公司电话"),
            // entity.company.phone
            new TranslationSeedItem("entity.company.phone", "ja-JP", "公司电话_jp", "公司电话"),
            // entity.company.phone
            new TranslationSeedItem("entity.company.phone", "zh-CN", "公司电话", "公司电话"),
            // entity.company.phone
            new TranslationSeedItem("entity.company.phone", "zh-HK", "公司电话_hk", "公司电话"),

            // entity.company.email
            new TranslationSeedItem("entity.company.email", "en-US", "公司邮箱_us", "公司邮箱"),
            // entity.company.email
            new TranslationSeedItem("entity.company.email", "ja-JP", "公司邮箱_jp", "公司邮箱"),
            // entity.company.email
            new TranslationSeedItem("entity.company.email", "zh-CN", "公司邮箱", "公司邮箱"),
            // entity.company.email
            new TranslationSeedItem("entity.company.email", "zh-HK", "公司邮箱_hk", "公司邮箱"),

            // entity.company.fax
            new TranslationSeedItem("entity.company.fax", "en-US", "公司传真_us", "公司传真"),
            // entity.company.fax
            new TranslationSeedItem("entity.company.fax", "ja-JP", "公司传真_jp", "公司传真"),
            // entity.company.fax
            new TranslationSeedItem("entity.company.fax", "zh-CN", "公司传真", "公司传真"),
            // entity.company.fax
            new TranslationSeedItem("entity.company.fax", "zh-HK", "公司传真_hk", "公司传真"),

            // entity.company.website
            new TranslationSeedItem("entity.company.website", "en-US", "公司网站_us", "公司网站"),
            // entity.company.website
            new TranslationSeedItem("entity.company.website", "ja-JP", "公司网站_jp", "公司网站"),
            // entity.company.website
            new TranslationSeedItem("entity.company.website", "zh-CN", "公司网站", "公司网站"),
            // entity.company.website
            new TranslationSeedItem("entity.company.website", "zh-HK", "公司网站_hk", "公司网站"),

            // entity.company.unifiedsocialcreditcode
            new TranslationSeedItem("entity.company.unifiedsocialcreditcode", "en-US", "统一社会信用代码_us", "统一社会信用代码"),
            // entity.company.unifiedsocialcreditcode
            new TranslationSeedItem("entity.company.unifiedsocialcreditcode", "ja-JP", "统一社会信用代码_jp", "统一社会信用代码"),
            // entity.company.unifiedsocialcreditcode
            new TranslationSeedItem("entity.company.unifiedsocialcreditcode", "zh-CN", "统一社会信用代码", "统一社会信用代码"),
            // entity.company.unifiedsocialcreditcode
            new TranslationSeedItem("entity.company.unifiedsocialcreditcode", "zh-HK", "统一社会信用代码_hk", "统一社会信用代码"),

            // entity.company.taxregistrationnumber
            new TranslationSeedItem("entity.company.taxregistrationnumber", "en-US", "税务登记号_us", "税务登记号"),
            // entity.company.taxregistrationnumber
            new TranslationSeedItem("entity.company.taxregistrationnumber", "ja-JP", "税务登记号_jp", "税务登记号"),
            // entity.company.taxregistrationnumber
            new TranslationSeedItem("entity.company.taxregistrationnumber", "zh-CN", "税务登记号", "税务登记号"),
            // entity.company.taxregistrationnumber
            new TranslationSeedItem("entity.company.taxregistrationnumber", "zh-HK", "税务登记号_hk", "税务登记号"),

            // entity.company.legalrepresentative
            new TranslationSeedItem("entity.company.legalrepresentative", "en-US", "法定代表人_us", "法定代表人"),
            // entity.company.legalrepresentative
            new TranslationSeedItem("entity.company.legalrepresentative", "ja-JP", "法定代表人_jp", "法定代表人"),
            // entity.company.legalrepresentative
            new TranslationSeedItem("entity.company.legalrepresentative", "zh-CN", "法定代表人", "法定代表人"),
            // entity.company.legalrepresentative
            new TranslationSeedItem("entity.company.legalrepresentative", "zh-HK", "法定代表人_hk", "法定代表人"),

            // entity.company.manager
            new TranslationSeedItem("entity.company.manager", "en-US", "公司负责人_us", "公司负责人"),
            // entity.company.manager
            new TranslationSeedItem("entity.company.manager", "ja-JP", "公司负责人_jp", "公司负责人"),
            // entity.company.manager
            new TranslationSeedItem("entity.company.manager", "zh-CN", "公司负责人", "公司负责人"),
            // entity.company.manager
            new TranslationSeedItem("entity.company.manager", "zh-HK", "公司负责人_hk", "公司负责人"),

            // entity.company.registeredcapital
            new TranslationSeedItem("entity.company.registeredcapital", "en-US", "注册资本_us", "注册资本（万元）"),
            // entity.company.registeredcapital
            new TranslationSeedItem("entity.company.registeredcapital", "ja-JP", "注册资本_jp", "注册资本（万元）"),
            // entity.company.registeredcapital
            new TranslationSeedItem("entity.company.registeredcapital", "zh-CN", "注册资本", "注册资本（万元）"),
            // entity.company.registeredcapital
            new TranslationSeedItem("entity.company.registeredcapital", "zh-HK", "注册资本_hk", "注册资本（万元）"),

            // entity.company.establishmentdate
            new TranslationSeedItem("entity.company.establishmentdate", "en-US", "成立日期_us", "成立日期"),
            // entity.company.establishmentdate
            new TranslationSeedItem("entity.company.establishmentdate", "ja-JP", "成立日期_jp", "成立日期"),
            // entity.company.establishmentdate
            new TranslationSeedItem("entity.company.establishmentdate", "zh-CN", "成立日期", "成立日期"),
            // entity.company.establishmentdate
            new TranslationSeedItem("entity.company.establishmentdate", "zh-HK", "成立日期_hk", "成立日期"),

            // entity.company.closingdate
            new TranslationSeedItem("entity.company.closingdate", "en-US", "关闭日期_us", "关闭日期（注销/停业；未关闭则为 null）"),
            // entity.company.closingdate
            new TranslationSeedItem("entity.company.closingdate", "ja-JP", "关闭日期_jp", "关闭日期（注销/停业；未关闭则为 null）"),
            // entity.company.closingdate
            new TranslationSeedItem("entity.company.closingdate", "zh-CN", "关闭日期", "关闭日期（注销/停业；未关闭则为 null）"),
            // entity.company.closingdate
            new TranslationSeedItem("entity.company.closingdate", "zh-HK", "关闭日期_hk", "关闭日期（注销/停业；未关闭则为 null）"),

            // entity.company.existence
            new TranslationSeedItem("entity.company.existence", "en-US", "存续状态_us", "存续状态（字典 sys_entity_existence_status）"),
            // entity.company.existence
            new TranslationSeedItem("entity.company.existence", "ja-JP", "存续状态_jp", "存续状态（字典 sys_entity_existence_status）"),
            // entity.company.existence
            new TranslationSeedItem("entity.company.existence", "zh-CN", "存续状态", "存续状态（字典 sys_entity_existence_status）"),
            // entity.company.existence
            new TranslationSeedItem("entity.company.existence", "zh-HK", "存续状态_hk", "存续状态（字典 sys_entity_existence_status）"),

            // entity.company.defaultculture
            new TranslationSeedItem("entity.company.defaultculture", "en-US", "区域文化_us", "区域文化编码（字典 sys_culture_code）"),
            // entity.company.defaultculture
            new TranslationSeedItem("entity.company.defaultculture", "ja-JP", "区域文化_jp", "区域文化编码（字典 sys_culture_code）"),
            // entity.company.defaultculture
            new TranslationSeedItem("entity.company.defaultculture", "zh-CN", "区域文化", "区域文化编码（字典 sys_culture_code）"),
            // entity.company.defaultculture
            new TranslationSeedItem("entity.company.defaultculture", "zh-HK", "区域文化_hk", "区域文化编码（字典 sys_culture_code）"),

            // entity.company.codealias
            new TranslationSeedItem("entity.company.codealias", "en-US", "编码代号_us", "编码代号（如 TKC、TCJ、DTA；前端字典录入）"),
            // entity.company.codealias
            new TranslationSeedItem("entity.company.codealias", "ja-JP", "编码代号_jp", "编码代号（如 TKC、TCJ、DTA；前端字典录入）"),
            // entity.company.codealias
            new TranslationSeedItem("entity.company.codealias", "zh-CN", "编码代号", "编码代号（如 TKC、TCJ、DTA；前端字典录入）"),
            // entity.company.codealias
            new TranslationSeedItem("entity.company.codealias", "zh-HK", "编码代号_hk", "编码代号（如 TKC、TCJ、DTA；前端字典录入）"),

            // entity.company.relatedplant
            new TranslationSeedItem("entity.company.relatedplant", "en-US", "关联工厂_us", "关联工厂（关联 TaktPlant.PlantCode，选项 TaktPlants/options）"),
            // entity.company.relatedplant
            new TranslationSeedItem("entity.company.relatedplant", "ja-JP", "关联工厂_jp", "关联工厂（关联 TaktPlant.PlantCode，选项 TaktPlants/options）"),
            // entity.company.relatedplant
            new TranslationSeedItem("entity.company.relatedplant", "zh-CN", "关联工厂", "关联工厂（关联 TaktPlant.PlantCode，选项 TaktPlants/options）"),
            // entity.company.relatedplant
            new TranslationSeedItem("entity.company.relatedplant", "zh-HK", "关联工厂_hk", "关联工厂（关联 TaktPlant.PlantCode，选项 TaktPlants/options）"),

            // entity.company.sortorder
            new TranslationSeedItem("entity.company.sortorder", "en-US", "排序号_us", "排序号（越小越靠前）"),
            // entity.company.sortorder
            new TranslationSeedItem("entity.company.sortorder", "ja-JP", "排序号_jp", "排序号（越小越靠前）"),
            // entity.company.sortorder
            new TranslationSeedItem("entity.company.sortorder", "zh-CN", "排序号", "排序号（越小越靠前）"),
            // entity.company.sortorder
            new TranslationSeedItem("entity.company.sortorder", "zh-HK", "排序号_hk", "排序号（越小越靠前）"),

            // entity.company.status
            new TranslationSeedItem("entity.company.status", "en-US", "公司状态_us", "公司状态（字典 sys_normal_disable_status）"),
            // entity.company.status
            new TranslationSeedItem("entity.company.status", "ja-JP", "公司状态_jp", "公司状态（字典 sys_normal_disable_status）"),
            // entity.company.status
            new TranslationSeedItem("entity.company.status", "zh-CN", "公司状态", "公司状态（字典 sys_normal_disable_status）"),
            // entity.company.status
            new TranslationSeedItem("entity.company.status", "zh-HK", "公司状态_hk", "公司状态（字典 sys_normal_disable_status）"),

            // entity.company.rolecompanies
            new TranslationSeedItem("entity.company.rolecompanies", "en-US", "可访问该公司的角色关联_us", "可访问该公司的角色关联（RBAC，表 takt_identity_role_company）"),
            // entity.company.rolecompanies
            new TranslationSeedItem("entity.company.rolecompanies", "ja-JP", "可访问该公司的角色关联_jp", "可访问该公司的角色关联（RBAC，表 takt_identity_role_company）"),
            // entity.company.rolecompanies
            new TranslationSeedItem("entity.company.rolecompanies", "zh-CN", "可访问该公司的角色关联", "可访问该公司的角色关联（RBAC，表 takt_identity_role_company）"),
            // entity.company.rolecompanies
            new TranslationSeedItem("entity.company.rolecompanies", "zh-HK", "可访问该公司的角色关联_hk", "可访问该公司的角色关联（RBAC，表 takt_identity_role_company）"),

            // entity.company.usercompanies
            new TranslationSeedItem("entity.company.usercompanies", "en-US", "可访问该公司的用户关联_us", "可访问该公司的用户关联（RBAC，表 takt_identity_user_company）"),
            // entity.company.usercompanies
            new TranslationSeedItem("entity.company.usercompanies", "ja-JP", "可访问该公司的用户关联_jp", "可访问该公司的用户关联（RBAC，表 takt_identity_user_company）"),
            // entity.company.usercompanies
            new TranslationSeedItem("entity.company.usercompanies", "zh-CN", "可访问该公司的用户关联", "可访问该公司的用户关联（RBAC，表 takt_identity_user_company）"),
            // entity.company.usercompanies
            new TranslationSeedItem("entity.company.usercompanies", "zh-HK", "可访问该公司的用户关联_hk", "可访问该公司的用户关联（RBAC，表 takt_identity_user_company）"),
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
        translation.ResourceGroup = "Financial";
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
