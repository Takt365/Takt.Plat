// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Materials
// 文件名称：TaktPlantI18nSeedData.cs
// 创建时间：2026-06-12
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktPlant 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktPlant 实体国际化翻译种子（键前缀 entity.plant.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktPlantI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktPlant 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 plant 实体翻译...", tenantCode);

        foreach (var item in GetPlantTranslations())
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

        TaktLogger.Information("TaktPlant 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktPlant 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.plant._self / entity.plant.{{field}}；ResourceGroup=4；ResourceType=0
    /// </summary>
    private static List<TranslationSeedItem> GetPlantTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.plant._self
            new TranslationSeedItem("entity.plant._self", "en-US", "Plant Information", "实体名称"),
            // entity.plant._self
            new TranslationSeedItem("entity.plant._self", "ja-JP", "Takt工厂信息", "实体名称"),
            // entity.plant._self
            new TranslationSeedItem("entity.plant._self", "zh-CN", "Takt工厂信息", "实体名称"),
            // entity.plant._self
            new TranslationSeedItem("entity.plant._self", "zh-HK", "Takt工厂信息", "实体名称"),

            // entity.plant.code
            new TranslationSeedItem("entity.plant.code", "en-US", "工厂代码", "工厂代码（唯一索引：租户内唯一，见 ix_plant_code_unique）"),
            // entity.plant.code
            new TranslationSeedItem("entity.plant.code", "ja-JP", "工厂代码", "工厂代码（唯一索引：租户内唯一，见 ix_plant_code_unique）"),
            // entity.plant.code
            new TranslationSeedItem("entity.plant.code", "zh-CN", "工厂代码", "工厂代码（唯一索引：租户内唯一，见 ix_plant_code_unique）"),
            // entity.plant.code
            new TranslationSeedItem("entity.plant.code", "zh-HK", "工厂代码", "工厂代码（唯一索引：租户内唯一，见 ix_plant_code_unique）"),

            // entity.plant.name
            new TranslationSeedItem("entity.plant.name", "en-US", "工厂名称", "工厂名称"),
            // entity.plant.name
            new TranslationSeedItem("entity.plant.name", "ja-JP", "工厂名称", "工厂名称"),
            // entity.plant.name
            new TranslationSeedItem("entity.plant.name", "zh-CN", "工厂名称", "工厂名称"),
            // entity.plant.name
            new TranslationSeedItem("entity.plant.name", "zh-HK", "工厂名称", "工厂名称"),

            // entity.plant.shortname
            new TranslationSeedItem("entity.plant.shortname", "en-US", "工厂简称", "工厂简称"),
            // entity.plant.shortname
            new TranslationSeedItem("entity.plant.shortname", "ja-JP", "工厂简称", "工厂简称"),
            // entity.plant.shortname
            new TranslationSeedItem("entity.plant.shortname", "zh-CN", "工厂简称", "工厂简称"),
            // entity.plant.shortname
            new TranslationSeedItem("entity.plant.shortname", "zh-HK", "工厂简称", "工厂简称"),

            // entity.plant.codealias
            new TranslationSeedItem("entity.plant.codealias", "en-US", "编码代号", "编码代号（如 TKC、TCJ、DTA；前端字典录入）"),
            // entity.plant.codealias
            new TranslationSeedItem("entity.plant.codealias", "ja-JP", "编码代号", "编码代号（如 TKC、TCJ、DTA；前端字典录入）"),
            // entity.plant.codealias
            new TranslationSeedItem("entity.plant.codealias", "zh-CN", "编码代号", "编码代号（如 TKC、TCJ、DTA；前端字典录入）"),
            // entity.plant.codealias
            new TranslationSeedItem("entity.plant.codealias", "zh-HK", "编码代号", "编码代号（如 TKC、TCJ、DTA；前端字典录入）"),

            // entity.plant.defaultculture
            new TranslationSeedItem("entity.plant.defaultculture", "en-US", "默认区域文化编码", "默认区域文化编码（BCP47，如 zh-CN、en-US、ja-JP、zh-HK）"),
            // entity.plant.defaultculture
            new TranslationSeedItem("entity.plant.defaultculture", "ja-JP", "默认区域文化编码", "默认区域文化编码（BCP47，如 zh-CN、en-US、ja-JP、zh-HK）"),
            // entity.plant.defaultculture
            new TranslationSeedItem("entity.plant.defaultculture", "zh-CN", "默认区域文化编码", "默认区域文化编码（BCP47，如 zh-CN、en-US、ja-JP、zh-HK）"),
            // entity.plant.defaultculture
            new TranslationSeedItem("entity.plant.defaultculture", "zh-HK", "默认区域文化编码", "默认区域文化编码（BCP47，如 zh-CN、en-US、ja-JP、zh-HK）"),

            // entity.plant.type
            new TranslationSeedItem("entity.plant.type", "en-US", "工厂类型", "工厂类型"),
            // entity.plant.type
            new TranslationSeedItem("entity.plant.type", "ja-JP", "工厂类型", "工厂类型"),
            // entity.plant.type
            new TranslationSeedItem("entity.plant.type", "zh-CN", "工厂类型", "工厂类型"),
            // entity.plant.type
            new TranslationSeedItem("entity.plant.type", "zh-HK", "工厂类型", "工厂类型"),

            // entity.plant.relatedcompany
            new TranslationSeedItem("entity.plant.relatedcompany", "en-US", "关联公司代码", "关联公司代码（如 2300、2400；与公司 RelatedPlant 对称）"),
            // entity.plant.relatedcompany
            new TranslationSeedItem("entity.plant.relatedcompany", "ja-JP", "关联公司代码", "关联公司代码（如 2300、2400；与公司 RelatedPlant 对称）"),
            // entity.plant.relatedcompany
            new TranslationSeedItem("entity.plant.relatedcompany", "zh-CN", "关联公司代码", "关联公司代码（如 2300、2400；与公司 RelatedPlant 对称）"),
            // entity.plant.relatedcompany
            new TranslationSeedItem("entity.plant.relatedcompany", "zh-HK", "关联公司代码", "关联公司代码（如 2300、2400；与公司 RelatedPlant 对称）"),

            // entity.plant.enterprisenature
            new TranslationSeedItem("entity.plant.enterprisenature", "en-US", "企业性质（登记注册类型代码）", "企业性质（统计用登记注册类型代码，国统字〔1998〕200号）"),
            // entity.plant.enterprisenature
            new TranslationSeedItem("entity.plant.enterprisenature", "ja-JP", "企业性质（登记注册类型代码）", "企业性质（统计用登记注册类型代码，国统字〔1998〕200号）"),
            // entity.plant.enterprisenature
            new TranslationSeedItem("entity.plant.enterprisenature", "zh-CN", "企业性质（登记注册类型代码）", "企业性质（统计用登记注册类型代码，国统字〔1998〕200号）"),
            // entity.plant.enterprisenature
            new TranslationSeedItem("entity.plant.enterprisenature", "zh-HK", "企业性质（登记注册类型代码）", "企业性质（统计用登记注册类型代码，国统字〔1998〕200号）"),

            // entity.plant.industryattribute
            new TranslationSeedItem("entity.plant.industryattribute", "en-US", "行业属性（国民经济行业门类）", "行业属性（GB/T 4754-2017 国民经济行业分类门类）"),
            // entity.plant.industryattribute
            new TranslationSeedItem("entity.plant.industryattribute", "ja-JP", "行业属性（国民经济行业门类）", "行业属性（GB/T 4754-2017 国民经济行业分类门类）"),
            // entity.plant.industryattribute
            new TranslationSeedItem("entity.plant.industryattribute", "zh-CN", "行业属性（国民经济行业门类）", "行业属性（GB/T 4754-2017 国民经济行业分类门类）"),
            // entity.plant.industryattribute
            new TranslationSeedItem("entity.plant.industryattribute", "zh-HK", "行业属性（国民经济行业门类）", "行业属性（GB/T 4754-2017 国民经济行业分类门类）"),

            // entity.plant.enterprisescale
            new TranslationSeedItem("entity.plant.enterprisescale", "en-US", "企业规模（大中小微型代码）", "企业规模（统计上大中小微型划分代码 1–4）"),
            // entity.plant.enterprisescale
            new TranslationSeedItem("entity.plant.enterprisescale", "ja-JP", "企业规模（大中小微型代码）", "企业规模（统计上大中小微型划分代码 1–4）"),
            // entity.plant.enterprisescale
            new TranslationSeedItem("entity.plant.enterprisescale", "zh-CN", "企业规模（大中小微型代码）", "企业规模（统计上大中小微型划分代码 1–4）"),
            // entity.plant.enterprisescale
            new TranslationSeedItem("entity.plant.enterprisescale", "zh-HK", "企业规模（大中小微型代码）", "企业规模（统计上大中小微型划分代码 1–4）"),

            // entity.plant.businessscope
            new TranslationSeedItem("entity.plant.businessscope", "en-US", "经营范围", "经营范围"),
            // entity.plant.businessscope
            new TranslationSeedItem("entity.plant.businessscope", "ja-JP", "经营范围", "经营范围"),
            // entity.plant.businessscope
            new TranslationSeedItem("entity.plant.businessscope", "zh-CN", "经营范围", "经营范围"),
            // entity.plant.businessscope
            new TranslationSeedItem("entity.plant.businessscope", "zh-HK", "经营范围", "经营范围"),

            // entity.plant.registrationaddress1
            new TranslationSeedItem("entity.plant.registrationaddress1", "en-US", "注册地址1", "注册地址1"),
            // entity.plant.registrationaddress1
            new TranslationSeedItem("entity.plant.registrationaddress1", "ja-JP", "注册地址1", "注册地址1"),
            // entity.plant.registrationaddress1
            new TranslationSeedItem("entity.plant.registrationaddress1", "zh-CN", "注册地址1", "注册地址1"),
            // entity.plant.registrationaddress1
            new TranslationSeedItem("entity.plant.registrationaddress1", "zh-HK", "注册地址1", "注册地址1"),

            // entity.plant.registrationaddress2
            new TranslationSeedItem("entity.plant.registrationaddress2", "en-US", "注册地址2", "注册地址2"),
            // entity.plant.registrationaddress2
            new TranslationSeedItem("entity.plant.registrationaddress2", "ja-JP", "注册地址2", "注册地址2"),
            // entity.plant.registrationaddress2
            new TranslationSeedItem("entity.plant.registrationaddress2", "zh-CN", "注册地址2", "注册地址2"),
            // entity.plant.registrationaddress2
            new TranslationSeedItem("entity.plant.registrationaddress2", "zh-HK", "注册地址2", "注册地址2"),

            // entity.plant.registrationaddress3
            new TranslationSeedItem("entity.plant.registrationaddress3", "en-US", "注册地址3", "注册地址3"),
            // entity.plant.registrationaddress3
            new TranslationSeedItem("entity.plant.registrationaddress3", "ja-JP", "注册地址3", "注册地址3"),
            // entity.plant.registrationaddress3
            new TranslationSeedItem("entity.plant.registrationaddress3", "zh-CN", "注册地址3", "注册地址3"),
            // entity.plant.registrationaddress3
            new TranslationSeedItem("entity.plant.registrationaddress3", "zh-HK", "注册地址3", "注册地址3"),

            // entity.plant.registrationregion
            new TranslationSeedItem("entity.plant.registrationregion", "en-US", "注册国家", "注册国家"),
            // entity.plant.registrationregion
            new TranslationSeedItem("entity.plant.registrationregion", "ja-JP", "注册国家", "注册国家"),
            // entity.plant.registrationregion
            new TranslationSeedItem("entity.plant.registrationregion", "zh-CN", "注册国家", "注册国家"),
            // entity.plant.registrationregion
            new TranslationSeedItem("entity.plant.registrationregion", "zh-HK", "注册国家", "注册国家"),

            // entity.plant.registrationprovince
            new TranslationSeedItem("entity.plant.registrationprovince", "en-US", "注册省", "注册省"),
            // entity.plant.registrationprovince
            new TranslationSeedItem("entity.plant.registrationprovince", "ja-JP", "注册省", "注册省"),
            // entity.plant.registrationprovince
            new TranslationSeedItem("entity.plant.registrationprovince", "zh-CN", "注册省", "注册省"),
            // entity.plant.registrationprovince
            new TranslationSeedItem("entity.plant.registrationprovince", "zh-HK", "注册省", "注册省"),

            // entity.plant.registrationcity
            new TranslationSeedItem("entity.plant.registrationcity", "en-US", "注册市", "注册市"),
            // entity.plant.registrationcity
            new TranslationSeedItem("entity.plant.registrationcity", "ja-JP", "注册市", "注册市"),
            // entity.plant.registrationcity
            new TranslationSeedItem("entity.plant.registrationcity", "zh-CN", "注册市", "注册市"),
            // entity.plant.registrationcity
            new TranslationSeedItem("entity.plant.registrationcity", "zh-HK", "注册市", "注册市"),

            // entity.plant.businessregion
            new TranslationSeedItem("entity.plant.businessregion", "en-US", "经营国家", "经营国家"),
            // entity.plant.businessregion
            new TranslationSeedItem("entity.plant.businessregion", "ja-JP", "经营国家", "经营国家"),
            // entity.plant.businessregion
            new TranslationSeedItem("entity.plant.businessregion", "zh-CN", "经营国家", "经营国家"),
            // entity.plant.businessregion
            new TranslationSeedItem("entity.plant.businessregion", "zh-HK", "经营国家", "经营国家"),

            // entity.plant.businessprovince
            new TranslationSeedItem("entity.plant.businessprovince", "en-US", "经营地区-省", "经营地区-省"),
            // entity.plant.businessprovince
            new TranslationSeedItem("entity.plant.businessprovince", "ja-JP", "经营地区-省", "经营地区-省"),
            // entity.plant.businessprovince
            new TranslationSeedItem("entity.plant.businessprovince", "zh-CN", "经营地区-省", "经营地区-省"),
            // entity.plant.businessprovince
            new TranslationSeedItem("entity.plant.businessprovince", "zh-HK", "经营地区-省", "经营地区-省"),

            // entity.plant.businesscity
            new TranslationSeedItem("entity.plant.businesscity", "en-US", "经营地区-市", "经营地区-市"),
            // entity.plant.businesscity
            new TranslationSeedItem("entity.plant.businesscity", "ja-JP", "经营地区-市", "经营地区-市"),
            // entity.plant.businesscity
            new TranslationSeedItem("entity.plant.businesscity", "zh-CN", "经营地区-市", "经营地区-市"),
            // entity.plant.businesscity
            new TranslationSeedItem("entity.plant.businesscity", "zh-HK", "经营地区-市", "经营地区-市"),

            // entity.plant.businessaddress1
            new TranslationSeedItem("entity.plant.businessaddress1", "en-US", "经营地址1", "经营地址1"),
            // entity.plant.businessaddress1
            new TranslationSeedItem("entity.plant.businessaddress1", "ja-JP", "经营地址1", "经营地址1"),
            // entity.plant.businessaddress1
            new TranslationSeedItem("entity.plant.businessaddress1", "zh-CN", "经营地址1", "经营地址1"),
            // entity.plant.businessaddress1
            new TranslationSeedItem("entity.plant.businessaddress1", "zh-HK", "经营地址1", "经营地址1"),

            // entity.plant.businessaddress2
            new TranslationSeedItem("entity.plant.businessaddress2", "en-US", "经营地址2", "经营地址2"),
            // entity.plant.businessaddress2
            new TranslationSeedItem("entity.plant.businessaddress2", "ja-JP", "经营地址2", "经营地址2"),
            // entity.plant.businessaddress2
            new TranslationSeedItem("entity.plant.businessaddress2", "zh-CN", "经营地址2", "经营地址2"),
            // entity.plant.businessaddress2
            new TranslationSeedItem("entity.plant.businessaddress2", "zh-HK", "经营地址2", "经营地址2"),

            // entity.plant.businessaddress3
            new TranslationSeedItem("entity.plant.businessaddress3", "en-US", "经营地址3", "经营地址3"),
            // entity.plant.businessaddress3
            new TranslationSeedItem("entity.plant.businessaddress3", "ja-JP", "经营地址3", "经营地址3"),
            // entity.plant.businessaddress3
            new TranslationSeedItem("entity.plant.businessaddress3", "zh-CN", "经营地址3", "经营地址3"),
            // entity.plant.businessaddress3
            new TranslationSeedItem("entity.plant.businessaddress3", "zh-HK", "经营地址3", "经营地址3"),

            // entity.plant.address1
            new TranslationSeedItem("entity.plant.address1", "en-US", "工厂地址1", "工厂地址1"),
            // entity.plant.address1
            new TranslationSeedItem("entity.plant.address1", "ja-JP", "工厂地址1", "工厂地址1"),
            // entity.plant.address1
            new TranslationSeedItem("entity.plant.address1", "zh-CN", "工厂地址1", "工厂地址1"),
            // entity.plant.address1
            new TranslationSeedItem("entity.plant.address1", "zh-HK", "工厂地址1", "工厂地址1"),

            // entity.plant.address2
            new TranslationSeedItem("entity.plant.address2", "en-US", "工厂地址2", "工厂地址2"),
            // entity.plant.address2
            new TranslationSeedItem("entity.plant.address2", "ja-JP", "工厂地址2", "工厂地址2"),
            // entity.plant.address2
            new TranslationSeedItem("entity.plant.address2", "zh-CN", "工厂地址2", "工厂地址2"),
            // entity.plant.address2
            new TranslationSeedItem("entity.plant.address2", "zh-HK", "工厂地址2", "工厂地址2"),

            // entity.plant.address3
            new TranslationSeedItem("entity.plant.address3", "en-US", "工厂地址3", "工厂地址3"),
            // entity.plant.address3
            new TranslationSeedItem("entity.plant.address3", "ja-JP", "工厂地址3", "工厂地址3"),
            // entity.plant.address3
            new TranslationSeedItem("entity.plant.address3", "zh-CN", "工厂地址3", "工厂地址3"),
            // entity.plant.address3
            new TranslationSeedItem("entity.plant.address3", "zh-HK", "工厂地址3", "工厂地址3"),

            // entity.plant.phone
            new TranslationSeedItem("entity.plant.phone", "en-US", "工厂电话", "工厂电话"),
            // entity.plant.phone
            new TranslationSeedItem("entity.plant.phone", "ja-JP", "工厂电话", "工厂电话"),
            // entity.plant.phone
            new TranslationSeedItem("entity.plant.phone", "zh-CN", "工厂电话", "工厂电话"),
            // entity.plant.phone
            new TranslationSeedItem("entity.plant.phone", "zh-HK", "工厂电话", "工厂电话"),

            // entity.plant.email
            new TranslationSeedItem("entity.plant.email", "en-US", "工厂邮箱", "工厂邮箱"),
            // entity.plant.email
            new TranslationSeedItem("entity.plant.email", "ja-JP", "工厂邮箱", "工厂邮箱"),
            // entity.plant.email
            new TranslationSeedItem("entity.plant.email", "zh-CN", "工厂邮箱", "工厂邮箱"),
            // entity.plant.email
            new TranslationSeedItem("entity.plant.email", "zh-HK", "工厂邮箱", "工厂邮箱"),

            // entity.plant.fax
            new TranslationSeedItem("entity.plant.fax", "en-US", "工厂传真", "工厂传真"),
            // entity.plant.fax
            new TranslationSeedItem("entity.plant.fax", "ja-JP", "工厂传真", "工厂传真"),
            // entity.plant.fax
            new TranslationSeedItem("entity.plant.fax", "zh-CN", "工厂传真", "工厂传真"),
            // entity.plant.fax
            new TranslationSeedItem("entity.plant.fax", "zh-HK", "工厂传真", "工厂传真"),

            // entity.plant.website
            new TranslationSeedItem("entity.plant.website", "en-US", "工厂网站", "工厂网站"),
            // entity.plant.website
            new TranslationSeedItem("entity.plant.website", "ja-JP", "工厂网站", "工厂网站"),
            // entity.plant.website
            new TranslationSeedItem("entity.plant.website", "zh-CN", "工厂网站", "工厂网站"),
            // entity.plant.website
            new TranslationSeedItem("entity.plant.website", "zh-HK", "工厂网站", "工厂网站"),

            // entity.plant.unifiedsocialcreditcode
            new TranslationSeedItem("entity.plant.unifiedsocialcreditcode", "en-US", "统一社会信用代码", "统一社会信用代码"),
            // entity.plant.unifiedsocialcreditcode
            new TranslationSeedItem("entity.plant.unifiedsocialcreditcode", "ja-JP", "统一社会信用代码", "统一社会信用代码"),
            // entity.plant.unifiedsocialcreditcode
            new TranslationSeedItem("entity.plant.unifiedsocialcreditcode", "zh-CN", "统一社会信用代码", "统一社会信用代码"),
            // entity.plant.unifiedsocialcreditcode
            new TranslationSeedItem("entity.plant.unifiedsocialcreditcode", "zh-HK", "统一社会信用代码", "统一社会信用代码"),

            // entity.plant.taxregistrationnumber
            new TranslationSeedItem("entity.plant.taxregistrationnumber", "en-US", "税务登记号", "税务登记号"),
            // entity.plant.taxregistrationnumber
            new TranslationSeedItem("entity.plant.taxregistrationnumber", "ja-JP", "税务登记号", "税务登记号"),
            // entity.plant.taxregistrationnumber
            new TranslationSeedItem("entity.plant.taxregistrationnumber", "zh-CN", "税务登记号", "税务登记号"),
            // entity.plant.taxregistrationnumber
            new TranslationSeedItem("entity.plant.taxregistrationnumber", "zh-HK", "税务登记号", "税务登记号"),

            // entity.plant.legalrepresentative
            new TranslationSeedItem("entity.plant.legalrepresentative", "en-US", "法定代表人", "法定代表人"),
            // entity.plant.legalrepresentative
            new TranslationSeedItem("entity.plant.legalrepresentative", "ja-JP", "法定代表人", "法定代表人"),
            // entity.plant.legalrepresentative
            new TranslationSeedItem("entity.plant.legalrepresentative", "zh-CN", "法定代表人", "法定代表人"),
            // entity.plant.legalrepresentative
            new TranslationSeedItem("entity.plant.legalrepresentative", "zh-HK", "法定代表人", "法定代表人"),

            // entity.plant.manager
            new TranslationSeedItem("entity.plant.manager", "en-US", "工厂负责人", "工厂负责人"),
            // entity.plant.manager
            new TranslationSeedItem("entity.plant.manager", "ja-JP", "工厂负责人", "工厂负责人"),
            // entity.plant.manager
            new TranslationSeedItem("entity.plant.manager", "zh-CN", "工厂负责人", "工厂负责人"),
            // entity.plant.manager
            new TranslationSeedItem("entity.plant.manager", "zh-HK", "工厂负责人", "工厂负责人"),

            // entity.plant.registeredcapital
            new TranslationSeedItem("entity.plant.registeredcapital", "en-US", "注册资本", "注册资本（万元）"),
            // entity.plant.registeredcapital
            new TranslationSeedItem("entity.plant.registeredcapital", "ja-JP", "注册资本", "注册资本（万元）"),
            // entity.plant.registeredcapital
            new TranslationSeedItem("entity.plant.registeredcapital", "zh-CN", "注册资本", "注册资本（万元）"),
            // entity.plant.registeredcapital
            new TranslationSeedItem("entity.plant.registeredcapital", "zh-HK", "注册资本", "注册资本（万元）"),

            // entity.plant.establishmentdate
            new TranslationSeedItem("entity.plant.establishmentdate", "en-US", "成立日期", "成立日期"),
            // entity.plant.establishmentdate
            new TranslationSeedItem("entity.plant.establishmentdate", "ja-JP", "成立日期", "成立日期"),
            // entity.plant.establishmentdate
            new TranslationSeedItem("entity.plant.establishmentdate", "zh-CN", "成立日期", "成立日期"),
            // entity.plant.establishmentdate
            new TranslationSeedItem("entity.plant.establishmentdate", "zh-HK", "成立日期", "成立日期"),

            // entity.plant.closingdate
            new TranslationSeedItem("entity.plant.closingdate", "en-US", "关闭日期", "关闭日期（注销/停业；未关闭则为 null）"),
            // entity.plant.closingdate
            new TranslationSeedItem("entity.plant.closingdate", "ja-JP", "关闭日期", "关闭日期（注销/停业；未关闭则为 null）"),
            // entity.plant.closingdate
            new TranslationSeedItem("entity.plant.closingdate", "zh-CN", "关闭日期", "关闭日期（注销/停业；未关闭则为 null）"),
            // entity.plant.closingdate
            new TranslationSeedItem("entity.plant.closingdate", "zh-HK", "关闭日期", "关闭日期（注销/停业；未关闭则为 null）"),

            // entity.plant.existence
            new TranslationSeedItem("entity.plant.existence", "en-US", "存续状态（登记状态代码）", "存续状态（市场主体登记状态）"),
            // entity.plant.existence
            new TranslationSeedItem("entity.plant.existence", "ja-JP", "存续状态（登记状态代码）", "存续状态（市场主体登记状态）"),
            // entity.plant.existence
            new TranslationSeedItem("entity.plant.existence", "zh-CN", "存续状态（登记状态代码）", "存续状态（市场主体登记状态）"),
            // entity.plant.existence
            new TranslationSeedItem("entity.plant.existence", "zh-HK", "存续状态（登记状态代码）", "存续状态（市场主体登记状态）"),

            // entity.plant.status
            new TranslationSeedItem("entity.plant.status", "en-US", "工厂状态", "工厂状态"),
            // entity.plant.status
            new TranslationSeedItem("entity.plant.status", "ja-JP", "工厂状态", "工厂状态"),
            // entity.plant.status
            new TranslationSeedItem("entity.plant.status", "zh-CN", "工厂状态", "工厂状态"),
            // entity.plant.status
            new TranslationSeedItem("entity.plant.status", "zh-HK", "工厂状态", "工厂状态"),

            // entity.plant.sortorder
            new TranslationSeedItem("entity.plant.sortorder", "en-US", "排序号", "排序号（越小越靠前）"),
            // entity.plant.sortorder
            new TranslationSeedItem("entity.plant.sortorder", "ja-JP", "排序号", "排序号（越小越靠前）"),
            // entity.plant.sortorder
            new TranslationSeedItem("entity.plant.sortorder", "zh-CN", "排序号", "排序号（越小越靠前）"),
            // entity.plant.sortorder
            new TranslationSeedItem("entity.plant.sortorder", "zh-HK", "排序号", "排序号（越小越靠前）"),
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
