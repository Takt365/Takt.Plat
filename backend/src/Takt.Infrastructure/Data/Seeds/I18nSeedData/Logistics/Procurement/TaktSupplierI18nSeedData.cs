// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Materials
// 文件名称：TaktSupplierI18nSeedData.cs
// 创建时间：2026-06-12
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSupplier 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktSupplier 实体国际化翻译种子（键前缀 entity.supplier.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSupplierI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSupplier 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 supplier 实体翻译...", tenantCode);

        foreach (var item in GetSupplierTranslations())
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

        TaktLogger.Information("TaktSupplier 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSupplier 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.supplier._self / entity.supplier.{{field}}；ResourceGroup=4；ResourceType=0
    /// </summary>
    private static List<TranslationSeedItem> GetSupplierTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.supplier._self
            new TranslationSeedItem("entity.supplier._self", "en-US", "Supplier Information", "实体名称"),
            // entity.supplier._self
            new TranslationSeedItem("entity.supplier._self", "ja-JP", "Takt供货商信息", "实体名称"),
            // entity.supplier._self
            new TranslationSeedItem("entity.supplier._self", "zh-CN", "Takt供货商信息", "实体名称"),
            // entity.supplier._self
            new TranslationSeedItem("entity.supplier._self", "zh-HK", "Takt供货商信息", "实体名称"),

            // entity.supplier.plantcode
            new TranslationSeedItem("entity.supplier.plantcode", "en-US", "工厂代码", "工厂代码"),
            // entity.supplier.plantcode
            new TranslationSeedItem("entity.supplier.plantcode", "ja-JP", "工厂代码", "工厂代码"),
            // entity.supplier.plantcode
            new TranslationSeedItem("entity.supplier.plantcode", "zh-CN", "工厂代码", "工厂代码"),
            // entity.supplier.plantcode
            new TranslationSeedItem("entity.supplier.plantcode", "zh-HK", "工厂代码", "工厂代码"),

            // entity.supplier.code
            new TranslationSeedItem("entity.supplier.code", "en-US", "供货商编码", "供货商编码（唯一索引）"),
            // entity.supplier.code
            new TranslationSeedItem("entity.supplier.code", "ja-JP", "供货商编码", "供货商编码（唯一索引）"),
            // entity.supplier.code
            new TranslationSeedItem("entity.supplier.code", "zh-CN", "供货商编码", "供货商编码（唯一索引）"),
            // entity.supplier.code
            new TranslationSeedItem("entity.supplier.code", "zh-HK", "供货商编码", "供货商编码（唯一索引）"),

            // entity.supplier.name
            new TranslationSeedItem("entity.supplier.name", "en-US", "供货商名称", "供货商名称"),
            // entity.supplier.name
            new TranslationSeedItem("entity.supplier.name", "ja-JP", "供货商名称", "供货商名称"),
            // entity.supplier.name
            new TranslationSeedItem("entity.supplier.name", "zh-CN", "供货商名称", "供货商名称"),
            // entity.supplier.name
            new TranslationSeedItem("entity.supplier.name", "zh-HK", "供货商名称", "供货商名称"),

            // entity.supplier.shortname
            new TranslationSeedItem("entity.supplier.shortname", "en-US", "供货商简称", "供货商简称"),
            // entity.supplier.shortname
            new TranslationSeedItem("entity.supplier.shortname", "ja-JP", "供货商简称", "供货商简称"),
            // entity.supplier.shortname
            new TranslationSeedItem("entity.supplier.shortname", "zh-CN", "供货商简称", "供货商简称"),
            // entity.supplier.shortname
            new TranslationSeedItem("entity.supplier.shortname", "zh-HK", "供货商简称", "供货商简称"),

            // entity.supplier.type
            new TranslationSeedItem("entity.supplier.type", "en-US", "供货商类型", "供货商类型（0=生产商，1=代理商，2=经销商，3=贸易商，4=其他）"),
            // entity.supplier.type
            new TranslationSeedItem("entity.supplier.type", "ja-JP", "供货商类型", "供货商类型（0=生产商，1=代理商，2=经销商，3=贸易商，4=其他）"),
            // entity.supplier.type
            new TranslationSeedItem("entity.supplier.type", "zh-CN", "供货商类型", "供货商类型（0=生产商，1=代理商，2=经销商，3=贸易商，4=其他）"),
            // entity.supplier.type
            new TranslationSeedItem("entity.supplier.type", "zh-HK", "供货商类型", "供货商类型（0=生产商，1=代理商，2=经销商，3=贸易商，4=其他）"),

            // entity.supplier.industrysector
            new TranslationSeedItem("entity.supplier.industrysector", "en-US", "行业领域", "行业领域"),
            // entity.supplier.industrysector
            new TranslationSeedItem("entity.supplier.industrysector", "ja-JP", "行业领域", "行业领域"),
            // entity.supplier.industrysector
            new TranslationSeedItem("entity.supplier.industrysector", "zh-CN", "行业领域", "行业领域"),
            // entity.supplier.industrysector
            new TranslationSeedItem("entity.supplier.industrysector", "zh-HK", "行业领域", "行业领域"),

            // entity.supplier.taxnumber
            new TranslationSeedItem("entity.supplier.taxnumber", "en-US", "供货商标识", "供货商标识（税务登记证号/统一社会信用代码）"),
            // entity.supplier.taxnumber
            new TranslationSeedItem("entity.supplier.taxnumber", "ja-JP", "供货商标识", "供货商标识（税务登记证号/统一社会信用代码）"),
            // entity.supplier.taxnumber
            new TranslationSeedItem("entity.supplier.taxnumber", "zh-CN", "供货商标识", "供货商标识（税务登记证号/统一社会信用代码）"),
            // entity.supplier.taxnumber
            new TranslationSeedItem("entity.supplier.taxnumber", "zh-HK", "供货商标识", "供货商标识（税务登记证号/统一社会信用代码）"),

            // entity.supplier.registrationcountry
            new TranslationSeedItem("entity.supplier.registrationcountry", "en-US", "注册国家", "注册国家（ISO 3166-1 alpha-2两位代码）"),
            // entity.supplier.registrationcountry
            new TranslationSeedItem("entity.supplier.registrationcountry", "ja-JP", "注册国家", "注册国家（ISO 3166-1 alpha-2两位代码）"),
            // entity.supplier.registrationcountry
            new TranslationSeedItem("entity.supplier.registrationcountry", "zh-CN", "注册国家", "注册国家（ISO 3166-1 alpha-2两位代码）"),
            // entity.supplier.registrationcountry
            new TranslationSeedItem("entity.supplier.registrationcountry", "zh-HK", "注册国家", "注册国家（ISO 3166-1 alpha-2两位代码）"),

            // entity.supplier.registrationaddress1
            new TranslationSeedItem("entity.supplier.registrationaddress1", "en-US", "注册地址1", "注册地址1"),
            // entity.supplier.registrationaddress1
            new TranslationSeedItem("entity.supplier.registrationaddress1", "ja-JP", "注册地址1", "注册地址1"),
            // entity.supplier.registrationaddress1
            new TranslationSeedItem("entity.supplier.registrationaddress1", "zh-CN", "注册地址1", "注册地址1"),
            // entity.supplier.registrationaddress1
            new TranslationSeedItem("entity.supplier.registrationaddress1", "zh-HK", "注册地址1", "注册地址1"),

            // entity.supplier.registrationaddress2
            new TranslationSeedItem("entity.supplier.registrationaddress2", "en-US", "注册地址2", "注册地址2"),
            // entity.supplier.registrationaddress2
            new TranslationSeedItem("entity.supplier.registrationaddress2", "ja-JP", "注册地址2", "注册地址2"),
            // entity.supplier.registrationaddress2
            new TranslationSeedItem("entity.supplier.registrationaddress2", "zh-CN", "注册地址2", "注册地址2"),
            // entity.supplier.registrationaddress2
            new TranslationSeedItem("entity.supplier.registrationaddress2", "zh-HK", "注册地址2", "注册地址2"),

            // entity.supplier.registrationaddress3
            new TranslationSeedItem("entity.supplier.registrationaddress3", "en-US", "注册地址3", "注册地址3"),
            // entity.supplier.registrationaddress3
            new TranslationSeedItem("entity.supplier.registrationaddress3", "ja-JP", "注册地址3", "注册地址3"),
            // entity.supplier.registrationaddress3
            new TranslationSeedItem("entity.supplier.registrationaddress3", "zh-CN", "注册地址3", "注册地址3"),
            // entity.supplier.registrationaddress3
            new TranslationSeedItem("entity.supplier.registrationaddress3", "zh-HK", "注册地址3", "注册地址3"),

            // entity.supplier.phone
            new TranslationSeedItem("entity.supplier.phone", "en-US", "供货商电话", "供货商电话"),
            // entity.supplier.phone
            new TranslationSeedItem("entity.supplier.phone", "ja-JP", "供货商电话", "供货商电话"),
            // entity.supplier.phone
            new TranslationSeedItem("entity.supplier.phone", "zh-CN", "供货商电话", "供货商电话"),
            // entity.supplier.phone
            new TranslationSeedItem("entity.supplier.phone", "zh-HK", "供货商电话", "供货商电话"),

            // entity.supplier.fax
            new TranslationSeedItem("entity.supplier.fax", "en-US", "供货商传真", "供货商传真"),
            // entity.supplier.fax
            new TranslationSeedItem("entity.supplier.fax", "ja-JP", "供货商传真", "供货商传真"),
            // entity.supplier.fax
            new TranslationSeedItem("entity.supplier.fax", "zh-CN", "供货商传真", "供货商传真"),
            // entity.supplier.fax
            new TranslationSeedItem("entity.supplier.fax", "zh-HK", "供货商传真", "供货商传真"),

            // entity.supplier.email
            new TranslationSeedItem("entity.supplier.email", "en-US", "供货商邮箱", "供货商邮箱"),
            // entity.supplier.email
            new TranslationSeedItem("entity.supplier.email", "ja-JP", "供货商邮箱", "供货商邮箱"),
            // entity.supplier.email
            new TranslationSeedItem("entity.supplier.email", "zh-CN", "供货商邮箱", "供货商邮箱"),
            // entity.supplier.email
            new TranslationSeedItem("entity.supplier.email", "zh-HK", "供货商邮箱", "供货商邮箱"),

            // entity.supplier.website
            new TranslationSeedItem("entity.supplier.website", "en-US", "供货商网站", "供货商网站"),
            // entity.supplier.website
            new TranslationSeedItem("entity.supplier.website", "ja-JP", "供货商网站", "供货商网站"),
            // entity.supplier.website
            new TranslationSeedItem("entity.supplier.website", "zh-CN", "供货商网站", "供货商网站"),
            // entity.supplier.website
            new TranslationSeedItem("entity.supplier.website", "zh-HK", "供货商网站", "供货商网站"),

            // entity.supplier.contactperson
            new TranslationSeedItem("entity.supplier.contactperson", "en-US", "联系人", "联系人"),
            // entity.supplier.contactperson
            new TranslationSeedItem("entity.supplier.contactperson", "ja-JP", "联系人", "联系人"),
            // entity.supplier.contactperson
            new TranslationSeedItem("entity.supplier.contactperson", "zh-CN", "联系人", "联系人"),
            // entity.supplier.contactperson
            new TranslationSeedItem("entity.supplier.contactperson", "zh-HK", "联系人", "联系人"),

            // entity.supplier.contactphone
            new TranslationSeedItem("entity.supplier.contactphone", "en-US", "联系人电话", "联系人电话"),
            // entity.supplier.contactphone
            new TranslationSeedItem("entity.supplier.contactphone", "ja-JP", "联系人电话", "联系人电话"),
            // entity.supplier.contactphone
            new TranslationSeedItem("entity.supplier.contactphone", "zh-CN", "联系人电话", "联系人电话"),
            // entity.supplier.contactphone
            new TranslationSeedItem("entity.supplier.contactphone", "zh-HK", "联系人电话", "联系人电话"),

            // entity.supplier.contactemail
            new TranslationSeedItem("entity.supplier.contactemail", "en-US", "联系人邮箱", "联系人邮箱"),
            // entity.supplier.contactemail
            new TranslationSeedItem("entity.supplier.contactemail", "ja-JP", "联系人邮箱", "联系人邮箱"),
            // entity.supplier.contactemail
            new TranslationSeedItem("entity.supplier.contactemail", "zh-CN", "联系人邮箱", "联系人邮箱"),
            // entity.supplier.contactemail
            new TranslationSeedItem("entity.supplier.contactemail", "zh-HK", "联系人邮箱", "联系人邮箱"),

            // entity.supplier.currencycode
            new TranslationSeedItem("entity.supplier.currencycode", "en-US", "结算币种代码", "结算币种代码"),
            // entity.supplier.currencycode
            new TranslationSeedItem("entity.supplier.currencycode", "ja-JP", "结算币种代码", "结算币种代码"),
            // entity.supplier.currencycode
            new TranslationSeedItem("entity.supplier.currencycode", "zh-CN", "结算币种代码", "结算币种代码"),
            // entity.supplier.currencycode
            new TranslationSeedItem("entity.supplier.currencycode", "zh-HK", "结算币种代码", "结算币种代码"),

            // entity.supplier.paymentterms
            new TranslationSeedItem("entity.supplier.paymentterms", "en-US", "付款条件", "付款条件（0=款到发货，1=货到付款，2=月结30天，3=月结60天，4=月结90天，5=其他）"),
            // entity.supplier.paymentterms
            new TranslationSeedItem("entity.supplier.paymentterms", "ja-JP", "付款条件", "付款条件（0=款到发货，1=货到付款，2=月结30天，3=月结60天，4=月结90天，5=其他）"),
            // entity.supplier.paymentterms
            new TranslationSeedItem("entity.supplier.paymentterms", "zh-CN", "付款条件", "付款条件（0=款到发货，1=货到付款，2=月结30天，3=月结60天，4=月结90天，5=其他）"),
            // entity.supplier.paymentterms
            new TranslationSeedItem("entity.supplier.paymentterms", "zh-HK", "付款条件", "付款条件（0=款到发货，1=货到付款，2=月结30天，3=月结60天，4=月结90天，5=其他）"),

            // entity.supplier.level
            new TranslationSeedItem("entity.supplier.level", "en-US", "供货商等级", "供货商等级（0=普通，1=优选，2=战略，3=临时）"),
            // entity.supplier.level
            new TranslationSeedItem("entity.supplier.level", "ja-JP", "供货商等级", "供货商等级（0=普通，1=优选，2=战略，3=临时）"),
            // entity.supplier.level
            new TranslationSeedItem("entity.supplier.level", "zh-CN", "供货商等级", "供货商等级（0=普通，1=优选，2=战略，3=临时）"),
            // entity.supplier.level
            new TranslationSeedItem("entity.supplier.level", "zh-HK", "供货商等级", "供货商等级（0=普通，1=优选，2=战略，3=临时）"),

            // entity.supplier.evaluationscore
            new TranslationSeedItem("entity.supplier.evaluationscore", "en-US", "评价分数", "评价分数（0-100分）"),
            // entity.supplier.evaluationscore
            new TranslationSeedItem("entity.supplier.evaluationscore", "ja-JP", "评价分数", "评价分数（0-100分）"),
            // entity.supplier.evaluationscore
            new TranslationSeedItem("entity.supplier.evaluationscore", "zh-CN", "评价分数", "评价分数（0-100分）"),
            // entity.supplier.evaluationscore
            new TranslationSeedItem("entity.supplier.evaluationscore", "zh-HK", "评价分数", "评价分数（0-100分）"),

            // entity.supplier.isqualified
            new TranslationSeedItem("entity.supplier.isqualified", "en-US", "是否合格供货商", "是否合格供货商（0=否，1=是）"),
            // entity.supplier.isqualified
            new TranslationSeedItem("entity.supplier.isqualified", "ja-JP", "是否合格供货商", "是否合格供货商（0=否，1=是）"),
            // entity.supplier.isqualified
            new TranslationSeedItem("entity.supplier.isqualified", "zh-CN", "是否合格供货商", "是否合格供货商（0=否，1=是）"),
            // entity.supplier.isqualified
            new TranslationSeedItem("entity.supplier.isqualified", "zh-HK", "是否合格供货商", "是否合格供货商（0=否，1=是）"),

            // entity.supplier.status
            new TranslationSeedItem("entity.supplier.status", "en-US", "供货商状态", "供货商状态（1=启用，0=禁用）"),
            // entity.supplier.status
            new TranslationSeedItem("entity.supplier.status", "ja-JP", "供货商状态", "供货商状态（1=启用，0=禁用）"),
            // entity.supplier.status
            new TranslationSeedItem("entity.supplier.status", "zh-CN", "供货商状态", "供货商状态（1=启用，0=禁用）"),
            // entity.supplier.status
            new TranslationSeedItem("entity.supplier.status", "zh-HK", "供货商状态", "供货商状态（1=启用，0=禁用）"),

            // entity.supplier.sortorder
            new TranslationSeedItem("entity.supplier.sortorder", "en-US", "排序号", "排序号（越小越靠前）"),
            // entity.supplier.sortorder
            new TranslationSeedItem("entity.supplier.sortorder", "ja-JP", "排序号", "排序号（越小越靠前）"),
            // entity.supplier.sortorder
            new TranslationSeedItem("entity.supplier.sortorder", "zh-CN", "排序号", "排序号（越小越靠前）"),
            // entity.supplier.sortorder
            new TranslationSeedItem("entity.supplier.sortorder", "zh-HK", "排序号", "排序号（越小越靠前）"),
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
