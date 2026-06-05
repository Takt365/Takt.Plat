// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Accounting.Financial
// 文件名称：TaktCompanyI18nSeedData.cs
// 创建时间：2026-06-05
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
using Takt.Shared.Enums;
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
    /// I18nKey：entity.company._self / entity.company.{{field}}；ResourceGroup=TaktModule.Accounting；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetCompanyTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.company._self
            new TranslationSeedItem("entity.company._self", "en-US", "Company Information", "实体名称"),
            // entity.company._self
            new TranslationSeedItem("entity.company._self", "ja-JP", "公司信息", "实体名称"),
            // entity.company._self
            new TranslationSeedItem("entity.company._self", "zh-CN", "公司信息", "实体名称"),
            // entity.company._self
            new TranslationSeedItem("entity.company._self", "zh-HK", "公司信息", "实体名称"),

            // entity.company.name
            new TranslationSeedItem("entity.company.name", "en-US", "公司名称", "公司名称"),
            // entity.company.name
            new TranslationSeedItem("entity.company.name", "ja-JP", "公司名称", "公司名称"),
            // entity.company.name
            new TranslationSeedItem("entity.company.name", "zh-CN", "公司名称", "公司名称"),
            // entity.company.name
            new TranslationSeedItem("entity.company.name", "zh-HK", "公司名称", "公司名称"),

            // entity.company.shortname
            new TranslationSeedItem("entity.company.shortname", "en-US", "公司简称", "公司简称"),
            // entity.company.shortname
            new TranslationSeedItem("entity.company.shortname", "ja-JP", "公司简称", "公司简称"),
            // entity.company.shortname
            new TranslationSeedItem("entity.company.shortname", "zh-CN", "公司简称", "公司简称"),
            // entity.company.shortname
            new TranslationSeedItem("entity.company.shortname", "zh-HK", "公司简称", "公司简称"),

            // entity.company.type
            new TranslationSeedItem("entity.company.type", "en-US", "公司类型", "公司类型"),
            // entity.company.type
            new TranslationSeedItem("entity.company.type", "ja-JP", "公司类型", "公司类型"),
            // entity.company.type
            new TranslationSeedItem("entity.company.type", "zh-CN", "公司类型", "公司类型"),
            // entity.company.type
            new TranslationSeedItem("entity.company.type", "zh-HK", "公司类型", "公司类型"),

            // entity.company.businessscope
            new TranslationSeedItem("entity.company.businessscope", "en-US", "经营范围", "经营范围"),
            // entity.company.businessscope
            new TranslationSeedItem("entity.company.businessscope", "ja-JP", "经营范围", "经营范围"),
            // entity.company.businessscope
            new TranslationSeedItem("entity.company.businessscope", "zh-CN", "经营范围", "经营范围"),
            // entity.company.businessscope
            new TranslationSeedItem("entity.company.businessscope", "zh-HK", "经营范围", "经营范围"),

            // entity.company.registrationaddress1
            new TranslationSeedItem("entity.company.registrationaddress1", "en-US", "注册地址1", "注册地址1"),
            // entity.company.registrationaddress1
            new TranslationSeedItem("entity.company.registrationaddress1", "ja-JP", "注册地址1", "注册地址1"),
            // entity.company.registrationaddress1
            new TranslationSeedItem("entity.company.registrationaddress1", "zh-CN", "注册地址1", "注册地址1"),
            // entity.company.registrationaddress1
            new TranslationSeedItem("entity.company.registrationaddress1", "zh-HK", "注册地址1", "注册地址1"),

            // entity.company.registrationaddress2
            new TranslationSeedItem("entity.company.registrationaddress2", "en-US", "注册地址2", "注册地址2"),
            // entity.company.registrationaddress2
            new TranslationSeedItem("entity.company.registrationaddress2", "ja-JP", "注册地址2", "注册地址2"),
            // entity.company.registrationaddress2
            new TranslationSeedItem("entity.company.registrationaddress2", "zh-CN", "注册地址2", "注册地址2"),
            // entity.company.registrationaddress2
            new TranslationSeedItem("entity.company.registrationaddress2", "zh-HK", "注册地址2", "注册地址2"),

            // entity.company.registrationaddress3
            new TranslationSeedItem("entity.company.registrationaddress3", "en-US", "注册地址3", "注册地址3"),
            // entity.company.registrationaddress3
            new TranslationSeedItem("entity.company.registrationaddress3", "ja-JP", "注册地址3", "注册地址3"),
            // entity.company.registrationaddress3
            new TranslationSeedItem("entity.company.registrationaddress3", "zh-CN", "注册地址3", "注册地址3"),
            // entity.company.registrationaddress3
            new TranslationSeedItem("entity.company.registrationaddress3", "zh-HK", "注册地址3", "注册地址3"),

            // entity.company.registrationregion
            new TranslationSeedItem("entity.company.registrationregion", "en-US", "注册国家", "注册国家"),
            // entity.company.registrationregion
            new TranslationSeedItem("entity.company.registrationregion", "ja-JP", "注册国家", "注册国家"),
            // entity.company.registrationregion
            new TranslationSeedItem("entity.company.registrationregion", "zh-CN", "注册国家", "注册国家"),
            // entity.company.registrationregion
            new TranslationSeedItem("entity.company.registrationregion", "zh-HK", "注册国家", "注册国家"),

            // entity.company.registrationprovince
            new TranslationSeedItem("entity.company.registrationprovince", "en-US", "注册省", "注册省"),
            // entity.company.registrationprovince
            new TranslationSeedItem("entity.company.registrationprovince", "ja-JP", "注册省", "注册省"),
            // entity.company.registrationprovince
            new TranslationSeedItem("entity.company.registrationprovince", "zh-CN", "注册省", "注册省"),
            // entity.company.registrationprovince
            new TranslationSeedItem("entity.company.registrationprovince", "zh-HK", "注册省", "注册省"),

            // entity.company.registrationcity
            new TranslationSeedItem("entity.company.registrationcity", "en-US", "注册市", "注册市"),
            // entity.company.registrationcity
            new TranslationSeedItem("entity.company.registrationcity", "ja-JP", "注册市", "注册市"),
            // entity.company.registrationcity
            new TranslationSeedItem("entity.company.registrationcity", "zh-CN", "注册市", "注册市"),
            // entity.company.registrationcity
            new TranslationSeedItem("entity.company.registrationcity", "zh-HK", "注册市", "注册市"),

            // entity.company.businessregion
            new TranslationSeedItem("entity.company.businessregion", "en-US", "经营国家", "经营国家"),
            // entity.company.businessregion
            new TranslationSeedItem("entity.company.businessregion", "ja-JP", "经营国家", "经营国家"),
            // entity.company.businessregion
            new TranslationSeedItem("entity.company.businessregion", "zh-CN", "经营国家", "经营国家"),
            // entity.company.businessregion
            new TranslationSeedItem("entity.company.businessregion", "zh-HK", "经营国家", "经营国家"),

            // entity.company.businessprovince
            new TranslationSeedItem("entity.company.businessprovince", "en-US", "经营地区-省", "经营地区-省"),
            // entity.company.businessprovince
            new TranslationSeedItem("entity.company.businessprovince", "ja-JP", "经营地区-省", "经营地区-省"),
            // entity.company.businessprovince
            new TranslationSeedItem("entity.company.businessprovince", "zh-CN", "经营地区-省", "经营地区-省"),
            // entity.company.businessprovince
            new TranslationSeedItem("entity.company.businessprovince", "zh-HK", "经营地区-省", "经营地区-省"),

            // entity.company.businesscity
            new TranslationSeedItem("entity.company.businesscity", "en-US", "经营地区-市", "经营地区-市"),
            // entity.company.businesscity
            new TranslationSeedItem("entity.company.businesscity", "ja-JP", "经营地区-市", "经营地区-市"),
            // entity.company.businesscity
            new TranslationSeedItem("entity.company.businesscity", "zh-CN", "经营地区-市", "经营地区-市"),
            // entity.company.businesscity
            new TranslationSeedItem("entity.company.businesscity", "zh-HK", "经营地区-市", "经营地区-市"),

            // entity.company.businessaddress1
            new TranslationSeedItem("entity.company.businessaddress1", "en-US", "经营地址1", "经营地址1"),
            // entity.company.businessaddress1
            new TranslationSeedItem("entity.company.businessaddress1", "ja-JP", "经营地址1", "经营地址1"),
            // entity.company.businessaddress1
            new TranslationSeedItem("entity.company.businessaddress1", "zh-CN", "经营地址1", "经营地址1"),
            // entity.company.businessaddress1
            new TranslationSeedItem("entity.company.businessaddress1", "zh-HK", "经营地址1", "经营地址1"),

            // entity.company.businessaddress2
            new TranslationSeedItem("entity.company.businessaddress2", "en-US", "经营地址2", "经营地址2"),
            // entity.company.businessaddress2
            new TranslationSeedItem("entity.company.businessaddress2", "ja-JP", "经营地址2", "经营地址2"),
            // entity.company.businessaddress2
            new TranslationSeedItem("entity.company.businessaddress2", "zh-CN", "经营地址2", "经营地址2"),
            // entity.company.businessaddress2
            new TranslationSeedItem("entity.company.businessaddress2", "zh-HK", "经营地址2", "经营地址2"),

            // entity.company.businessaddress3
            new TranslationSeedItem("entity.company.businessaddress3", "en-US", "经营地址3", "经营地址3"),
            // entity.company.businessaddress3
            new TranslationSeedItem("entity.company.businessaddress3", "ja-JP", "经营地址3", "经营地址3"),
            // entity.company.businessaddress3
            new TranslationSeedItem("entity.company.businessaddress3", "zh-CN", "经营地址3", "经营地址3"),
            // entity.company.businessaddress3
            new TranslationSeedItem("entity.company.businessaddress3", "zh-HK", "经营地址3", "经营地址3"),

            // entity.company.phone
            new TranslationSeedItem("entity.company.phone", "en-US", "公司电话", "公司电话"),
            // entity.company.phone
            new TranslationSeedItem("entity.company.phone", "ja-JP", "公司电话", "公司电话"),
            // entity.company.phone
            new TranslationSeedItem("entity.company.phone", "zh-CN", "公司电话", "公司电话"),
            // entity.company.phone
            new TranslationSeedItem("entity.company.phone", "zh-HK", "公司电话", "公司电话"),

            // entity.company.email
            new TranslationSeedItem("entity.company.email", "en-US", "公司邮箱", "公司邮箱"),
            // entity.company.email
            new TranslationSeedItem("entity.company.email", "ja-JP", "公司邮箱", "公司邮箱"),
            // entity.company.email
            new TranslationSeedItem("entity.company.email", "zh-CN", "公司邮箱", "公司邮箱"),
            // entity.company.email
            new TranslationSeedItem("entity.company.email", "zh-HK", "公司邮箱", "公司邮箱"),

            // entity.company.fax
            new TranslationSeedItem("entity.company.fax", "en-US", "公司传真", "公司传真"),
            // entity.company.fax
            new TranslationSeedItem("entity.company.fax", "ja-JP", "公司传真", "公司传真"),
            // entity.company.fax
            new TranslationSeedItem("entity.company.fax", "zh-CN", "公司传真", "公司传真"),
            // entity.company.fax
            new TranslationSeedItem("entity.company.fax", "zh-HK", "公司传真", "公司传真"),

            // entity.company.website
            new TranslationSeedItem("entity.company.website", "en-US", "公司网站", "公司网站"),
            // entity.company.website
            new TranslationSeedItem("entity.company.website", "ja-JP", "公司网站", "公司网站"),
            // entity.company.website
            new TranslationSeedItem("entity.company.website", "zh-CN", "公司网站", "公司网站"),
            // entity.company.website
            new TranslationSeedItem("entity.company.website", "zh-HK", "公司网站", "公司网站"),

            // entity.company.unifiedsocialcreditcode
            new TranslationSeedItem("entity.company.unifiedsocialcreditcode", "en-US", "统一社会信用代码", "统一社会信用代码"),
            // entity.company.unifiedsocialcreditcode
            new TranslationSeedItem("entity.company.unifiedsocialcreditcode", "ja-JP", "统一社会信用代码", "统一社会信用代码"),
            // entity.company.unifiedsocialcreditcode
            new TranslationSeedItem("entity.company.unifiedsocialcreditcode", "zh-CN", "统一社会信用代码", "统一社会信用代码"),
            // entity.company.unifiedsocialcreditcode
            new TranslationSeedItem("entity.company.unifiedsocialcreditcode", "zh-HK", "统一社会信用代码", "统一社会信用代码"),

            // entity.company.taxregistrationnumber
            new TranslationSeedItem("entity.company.taxregistrationnumber", "en-US", "税务登记号", "税务登记号"),
            // entity.company.taxregistrationnumber
            new TranslationSeedItem("entity.company.taxregistrationnumber", "ja-JP", "税务登记号", "税务登记号"),
            // entity.company.taxregistrationnumber
            new TranslationSeedItem("entity.company.taxregistrationnumber", "zh-CN", "税务登记号", "税务登记号"),
            // entity.company.taxregistrationnumber
            new TranslationSeedItem("entity.company.taxregistrationnumber", "zh-HK", "税务登记号", "税务登记号"),

            // entity.company.legalrepresentative
            new TranslationSeedItem("entity.company.legalrepresentative", "en-US", "法定代表人", "法定代表人"),
            // entity.company.legalrepresentative
            new TranslationSeedItem("entity.company.legalrepresentative", "ja-JP", "法定代表人", "法定代表人"),
            // entity.company.legalrepresentative
            new TranslationSeedItem("entity.company.legalrepresentative", "zh-CN", "法定代表人", "法定代表人"),
            // entity.company.legalrepresentative
            new TranslationSeedItem("entity.company.legalrepresentative", "zh-HK", "法定代表人", "法定代表人"),

            // entity.company.manager
            new TranslationSeedItem("entity.company.manager", "en-US", "公司负责人", "公司负责人"),
            // entity.company.manager
            new TranslationSeedItem("entity.company.manager", "ja-JP", "公司负责人", "公司负责人"),
            // entity.company.manager
            new TranslationSeedItem("entity.company.manager", "zh-CN", "公司负责人", "公司负责人"),
            // entity.company.manager
            new TranslationSeedItem("entity.company.manager", "zh-HK", "公司负责人", "公司负责人"),

            // entity.company.registeredcapital
            new TranslationSeedItem("entity.company.registeredcapital", "en-US", "注册资本", "注册资本（万元）"),
            // entity.company.registeredcapital
            new TranslationSeedItem("entity.company.registeredcapital", "ja-JP", "注册资本", "注册资本（万元）"),
            // entity.company.registeredcapital
            new TranslationSeedItem("entity.company.registeredcapital", "zh-CN", "注册资本", "注册资本（万元）"),
            // entity.company.registeredcapital
            new TranslationSeedItem("entity.company.registeredcapital", "zh-HK", "注册资本", "注册资本（万元）"),

            // entity.company.establishmentdate
            new TranslationSeedItem("entity.company.establishmentdate", "en-US", "成立日期", "成立日期"),
            // entity.company.establishmentdate
            new TranslationSeedItem("entity.company.establishmentdate", "ja-JP", "成立日期", "成立日期"),
            // entity.company.establishmentdate
            new TranslationSeedItem("entity.company.establishmentdate", "zh-CN", "成立日期", "成立日期"),
            // entity.company.establishmentdate
            new TranslationSeedItem("entity.company.establishmentdate", "zh-HK", "成立日期", "成立日期"),

            // entity.company.closingdate
            new TranslationSeedItem("entity.company.closingdate", "en-US", "关闭日期", "关闭日期（注销/停业；未关闭则为 null）"),
            // entity.company.closingdate
            new TranslationSeedItem("entity.company.closingdate", "ja-JP", "关闭日期", "关闭日期（注销/停业；未关闭则为 null）"),
            // entity.company.closingdate
            new TranslationSeedItem("entity.company.closingdate", "zh-CN", "关闭日期", "关闭日期（注销/停业；未关闭则为 null）"),
            // entity.company.closingdate
            new TranslationSeedItem("entity.company.closingdate", "zh-HK", "关闭日期", "关闭日期（注销/停业；未关闭则为 null）"),

            // entity.company.existence
            new TranslationSeedItem("entity.company.existence", "en-US", "存续状态（登记状态代码）", "存续状态（市场主体登记状态）"),
            // entity.company.existence
            new TranslationSeedItem("entity.company.existence", "ja-JP", "存续状态（登记状态代码）", "存续状态（市场主体登记状态）"),
            // entity.company.existence
            new TranslationSeedItem("entity.company.existence", "zh-CN", "存续状态（登记状态代码）", "存续状态（市场主体登记状态）"),
            // entity.company.existence
            new TranslationSeedItem("entity.company.existence", "zh-HK", "存续状态（登记状态代码）", "存续状态（市场主体登记状态）"),

            // entity.company.relatedplant
            new TranslationSeedItem("entity.company.relatedplant", "en-US", "关联工厂编码", "关联工厂编码（如 0001、C100）"),
            // entity.company.relatedplant
            new TranslationSeedItem("entity.company.relatedplant", "ja-JP", "关联工厂编码", "关联工厂编码（如 0001、C100）"),
            // entity.company.relatedplant
            new TranslationSeedItem("entity.company.relatedplant", "zh-CN", "关联工厂编码", "关联工厂编码（如 0001、C100）"),
            // entity.company.relatedplant
            new TranslationSeedItem("entity.company.relatedplant", "zh-HK", "关联工厂编码", "关联工厂编码（如 0001、C100）"),

            // entity.company.defaultculture
            new TranslationSeedItem("entity.company.defaultculture", "en-US", "默认区域文化编码", "默认区域文化编码（BCP47，如 zh-CN、en-US、ja-JP、zh-HK）"),
            // entity.company.defaultculture
            new TranslationSeedItem("entity.company.defaultculture", "ja-JP", "默认区域文化编码", "默认区域文化编码（BCP47，如 zh-CN、en-US、ja-JP、zh-HK）"),
            // entity.company.defaultculture
            new TranslationSeedItem("entity.company.defaultculture", "zh-CN", "默认区域文化编码", "默认区域文化编码（BCP47，如 zh-CN、en-US、ja-JP、zh-HK）"),
            // entity.company.defaultculture
            new TranslationSeedItem("entity.company.defaultculture", "zh-HK", "默认区域文化编码", "默认区域文化编码（BCP47，如 zh-CN、en-US、ja-JP、zh-HK）"),

            // entity.company.codealias
            new TranslationSeedItem("entity.company.codealias", "en-US", "编码代号", "编码代号（如 TKC、TCJ、DTA；前端字典录入）"),
            // entity.company.codealias
            new TranslationSeedItem("entity.company.codealias", "ja-JP", "编码代号", "编码代号（如 TKC、TCJ、DTA；前端字典录入）"),
            // entity.company.codealias
            new TranslationSeedItem("entity.company.codealias", "zh-CN", "编码代号", "编码代号（如 TKC、TCJ、DTA；前端字典录入）"),
            // entity.company.codealias
            new TranslationSeedItem("entity.company.codealias", "zh-HK", "编码代号", "编码代号（如 TKC、TCJ、DTA；前端字典录入）"),

            // entity.company.status
            new TranslationSeedItem("entity.company.status", "en-US", "公司状态", "公司状态"),
            // entity.company.status
            new TranslationSeedItem("entity.company.status", "ja-JP", "公司状态", "公司状态"),
            // entity.company.status
            new TranslationSeedItem("entity.company.status", "zh-CN", "公司状态", "公司状态"),
            // entity.company.status
            new TranslationSeedItem("entity.company.status", "zh-HK", "公司状态", "公司状态"),

            // entity.company.sortorder
            new TranslationSeedItem("entity.company.sortorder", "en-US", "排序号", "排序号（越小越靠前）"),
            // entity.company.sortorder
            new TranslationSeedItem("entity.company.sortorder", "ja-JP", "排序号", "排序号（越小越靠前）"),
            // entity.company.sortorder
            new TranslationSeedItem("entity.company.sortorder", "zh-CN", "排序号", "排序号（越小越靠前）"),
            // entity.company.sortorder
            new TranslationSeedItem("entity.company.sortorder", "zh-HK", "排序号", "排序号（越小越靠前）"),

            // entity.company.rolecompanies
            new TranslationSeedItem("entity.company.rolecompanies", "en-US", "roleCompanies", "可访问该公司的角色关联（RBAC，表 takt_identity_role_company）"),
            // entity.company.rolecompanies
            new TranslationSeedItem("entity.company.rolecompanies", "ja-JP", "roleCompanies", "可访问该公司的角色关联（RBAC，表 takt_identity_role_company）"),
            // entity.company.rolecompanies
            new TranslationSeedItem("entity.company.rolecompanies", "zh-CN", "roleCompanies", "可访问该公司的角色关联（RBAC，表 takt_identity_role_company）"),
            // entity.company.rolecompanies
            new TranslationSeedItem("entity.company.rolecompanies", "zh-HK", "roleCompanies", "可访问该公司的角色关联（RBAC，表 takt_identity_role_company）"),
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
        translation.ResourceGroup = TaktModule.Accounting;
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
