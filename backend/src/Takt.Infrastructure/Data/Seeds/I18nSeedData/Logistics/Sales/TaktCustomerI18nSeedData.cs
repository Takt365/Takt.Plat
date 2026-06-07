// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Sales
// 文件名称：TaktCustomerI18nSeedData.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktCustomer 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Sales;

/// <summary>
/// TaktCustomer 实体国际化翻译种子（键前缀 entity.customer.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktCustomerI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktCustomer 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 customer 实体翻译...", tenantCode);

        foreach (var item in GetCustomerTranslations())
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

        TaktLogger.Information("TaktCustomer 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktCustomer 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.customer._self / entity.customer.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetCustomerTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.customer._self
            new TranslationSeedItem("entity.customer._self", "en-US", "Customer Information", "实体名称"),
            // entity.customer._self
            new TranslationSeedItem("entity.customer._self", "ja-JP", "Takt客户信息信息", "实体名称"),
            // entity.customer._self
            new TranslationSeedItem("entity.customer._self", "zh-CN", "Takt客户信息信息", "实体名称"),
            // entity.customer._self
            new TranslationSeedItem("entity.customer._self", "zh-HK", "Takt客户信息信息", "实体名称"),

            // entity.customer.plantcode
            new TranslationSeedItem("entity.customer.plantcode", "en-US", "工厂代码", "工厂代码"),
            // entity.customer.plantcode
            new TranslationSeedItem("entity.customer.plantcode", "ja-JP", "工厂代码", "工厂代码"),
            // entity.customer.plantcode
            new TranslationSeedItem("entity.customer.plantcode", "zh-CN", "工厂代码", "工厂代码"),
            // entity.customer.plantcode
            new TranslationSeedItem("entity.customer.plantcode", "zh-HK", "工厂代码", "工厂代码"),

            // entity.customer.code
            new TranslationSeedItem("entity.customer.code", "en-US", "客户编码", "客户编码（唯一索引）"),
            // entity.customer.code
            new TranslationSeedItem("entity.customer.code", "ja-JP", "客户编码", "客户编码（唯一索引）"),
            // entity.customer.code
            new TranslationSeedItem("entity.customer.code", "zh-CN", "客户编码", "客户编码（唯一索引）"),
            // entity.customer.code
            new TranslationSeedItem("entity.customer.code", "zh-HK", "客户编码", "客户编码（唯一索引）"),

            // entity.customer.name
            new TranslationSeedItem("entity.customer.name", "en-US", "客户名称", "客户名称"),
            // entity.customer.name
            new TranslationSeedItem("entity.customer.name", "ja-JP", "客户名称", "客户名称"),
            // entity.customer.name
            new TranslationSeedItem("entity.customer.name", "zh-CN", "客户名称", "客户名称"),
            // entity.customer.name
            new TranslationSeedItem("entity.customer.name", "zh-HK", "客户名称", "客户名称"),

            // entity.customer.shortname
            new TranslationSeedItem("entity.customer.shortname", "en-US", "客户简称", "客户简称"),
            // entity.customer.shortname
            new TranslationSeedItem("entity.customer.shortname", "ja-JP", "客户简称", "客户简称"),
            // entity.customer.shortname
            new TranslationSeedItem("entity.customer.shortname", "zh-CN", "客户简称", "客户简称"),
            // entity.customer.shortname
            new TranslationSeedItem("entity.customer.shortname", "zh-HK", "客户简称", "客户简称"),

            // entity.customer.type
            new TranslationSeedItem("entity.customer.type", "en-US", "客户类型", "客户类型（0=企业客户，1=个人客户，2=政府机构，3=其他）"),
            // entity.customer.type
            new TranslationSeedItem("entity.customer.type", "ja-JP", "客户类型", "客户类型（0=企业客户，1=个人客户，2=政府机构，3=其他）"),
            // entity.customer.type
            new TranslationSeedItem("entity.customer.type", "zh-CN", "客户类型", "客户类型（0=企业客户，1=个人客户，2=政府机构，3=其他）"),
            // entity.customer.type
            new TranslationSeedItem("entity.customer.type", "zh-HK", "客户类型", "客户类型（0=企业客户，1=个人客户，2=政府机构，3=其他）"),

            // entity.customer.industrysector
            new TranslationSeedItem("entity.customer.industrysector", "en-US", "行业领域", "行业领域"),
            // entity.customer.industrysector
            new TranslationSeedItem("entity.customer.industrysector", "ja-JP", "行业领域", "行业领域"),
            // entity.customer.industrysector
            new TranslationSeedItem("entity.customer.industrysector", "zh-CN", "行业领域", "行业领域"),
            // entity.customer.industrysector
            new TranslationSeedItem("entity.customer.industrysector", "zh-HK", "行业领域", "行业领域"),

            // entity.customer.taxnumber
            new TranslationSeedItem("entity.customer.taxnumber", "en-US", "客户标识", "客户标识（税务登记证号/统一社会信用代码）"),
            // entity.customer.taxnumber
            new TranslationSeedItem("entity.customer.taxnumber", "ja-JP", "客户标识", "客户标识（税务登记证号/统一社会信用代码）"),
            // entity.customer.taxnumber
            new TranslationSeedItem("entity.customer.taxnumber", "zh-CN", "客户标识", "客户标识（税务登记证号/统一社会信用代码）"),
            // entity.customer.taxnumber
            new TranslationSeedItem("entity.customer.taxnumber", "zh-HK", "客户标识", "客户标识（税务登记证号/统一社会信用代码）"),

            // entity.customer.registrationcountry
            new TranslationSeedItem("entity.customer.registrationcountry", "en-US", "注册国家", "注册国家（ISO 3166-1 alpha-2两位代码）"),
            // entity.customer.registrationcountry
            new TranslationSeedItem("entity.customer.registrationcountry", "ja-JP", "注册国家", "注册国家（ISO 3166-1 alpha-2两位代码）"),
            // entity.customer.registrationcountry
            new TranslationSeedItem("entity.customer.registrationcountry", "zh-CN", "注册国家", "注册国家（ISO 3166-1 alpha-2两位代码）"),
            // entity.customer.registrationcountry
            new TranslationSeedItem("entity.customer.registrationcountry", "zh-HK", "注册国家", "注册国家（ISO 3166-1 alpha-2两位代码）"),

            // entity.customer.registrationaddress1
            new TranslationSeedItem("entity.customer.registrationaddress1", "en-US", "注册地址1", "注册地址1"),
            // entity.customer.registrationaddress1
            new TranslationSeedItem("entity.customer.registrationaddress1", "ja-JP", "注册地址1", "注册地址1"),
            // entity.customer.registrationaddress1
            new TranslationSeedItem("entity.customer.registrationaddress1", "zh-CN", "注册地址1", "注册地址1"),
            // entity.customer.registrationaddress1
            new TranslationSeedItem("entity.customer.registrationaddress1", "zh-HK", "注册地址1", "注册地址1"),

            // entity.customer.registrationaddress2
            new TranslationSeedItem("entity.customer.registrationaddress2", "en-US", "注册地址2", "注册地址2"),
            // entity.customer.registrationaddress2
            new TranslationSeedItem("entity.customer.registrationaddress2", "ja-JP", "注册地址2", "注册地址2"),
            // entity.customer.registrationaddress2
            new TranslationSeedItem("entity.customer.registrationaddress2", "zh-CN", "注册地址2", "注册地址2"),
            // entity.customer.registrationaddress2
            new TranslationSeedItem("entity.customer.registrationaddress2", "zh-HK", "注册地址2", "注册地址2"),

            // entity.customer.registrationaddress3
            new TranslationSeedItem("entity.customer.registrationaddress3", "en-US", "注册地址3", "注册地址3"),
            // entity.customer.registrationaddress3
            new TranslationSeedItem("entity.customer.registrationaddress3", "ja-JP", "注册地址3", "注册地址3"),
            // entity.customer.registrationaddress3
            new TranslationSeedItem("entity.customer.registrationaddress3", "zh-CN", "注册地址3", "注册地址3"),
            // entity.customer.registrationaddress3
            new TranslationSeedItem("entity.customer.registrationaddress3", "zh-HK", "注册地址3", "注册地址3"),

            // entity.customer.phone
            new TranslationSeedItem("entity.customer.phone", "en-US", "客户电话", "客户电话"),
            // entity.customer.phone
            new TranslationSeedItem("entity.customer.phone", "ja-JP", "客户电话", "客户电话"),
            // entity.customer.phone
            new TranslationSeedItem("entity.customer.phone", "zh-CN", "客户电话", "客户电话"),
            // entity.customer.phone
            new TranslationSeedItem("entity.customer.phone", "zh-HK", "客户电话", "客户电话"),

            // entity.customer.fax
            new TranslationSeedItem("entity.customer.fax", "en-US", "客户传真", "客户传真"),
            // entity.customer.fax
            new TranslationSeedItem("entity.customer.fax", "ja-JP", "客户传真", "客户传真"),
            // entity.customer.fax
            new TranslationSeedItem("entity.customer.fax", "zh-CN", "客户传真", "客户传真"),
            // entity.customer.fax
            new TranslationSeedItem("entity.customer.fax", "zh-HK", "客户传真", "客户传真"),

            // entity.customer.email
            new TranslationSeedItem("entity.customer.email", "en-US", "客户邮箱", "客户邮箱"),
            // entity.customer.email
            new TranslationSeedItem("entity.customer.email", "ja-JP", "客户邮箱", "客户邮箱"),
            // entity.customer.email
            new TranslationSeedItem("entity.customer.email", "zh-CN", "客户邮箱", "客户邮箱"),
            // entity.customer.email
            new TranslationSeedItem("entity.customer.email", "zh-HK", "客户邮箱", "客户邮箱"),

            // entity.customer.website
            new TranslationSeedItem("entity.customer.website", "en-US", "客户网站", "客户网站"),
            // entity.customer.website
            new TranslationSeedItem("entity.customer.website", "ja-JP", "客户网站", "客户网站"),
            // entity.customer.website
            new TranslationSeedItem("entity.customer.website", "zh-CN", "客户网站", "客户网站"),
            // entity.customer.website
            new TranslationSeedItem("entity.customer.website", "zh-HK", "客户网站", "客户网站"),

            // entity.customer.contactperson
            new TranslationSeedItem("entity.customer.contactperson", "en-US", "联系人", "联系人"),
            // entity.customer.contactperson
            new TranslationSeedItem("entity.customer.contactperson", "ja-JP", "联系人", "联系人"),
            // entity.customer.contactperson
            new TranslationSeedItem("entity.customer.contactperson", "zh-CN", "联系人", "联系人"),
            // entity.customer.contactperson
            new TranslationSeedItem("entity.customer.contactperson", "zh-HK", "联系人", "联系人"),

            // entity.customer.contactphone
            new TranslationSeedItem("entity.customer.contactphone", "en-US", "联系人电话", "联系人电话"),
            // entity.customer.contactphone
            new TranslationSeedItem("entity.customer.contactphone", "ja-JP", "联系人电话", "联系人电话"),
            // entity.customer.contactphone
            new TranslationSeedItem("entity.customer.contactphone", "zh-CN", "联系人电话", "联系人电话"),
            // entity.customer.contactphone
            new TranslationSeedItem("entity.customer.contactphone", "zh-HK", "联系人电话", "联系人电话"),

            // entity.customer.contactemail
            new TranslationSeedItem("entity.customer.contactemail", "en-US", "联系人邮箱", "联系人邮箱"),
            // entity.customer.contactemail
            new TranslationSeedItem("entity.customer.contactemail", "ja-JP", "联系人邮箱", "联系人邮箱"),
            // entity.customer.contactemail
            new TranslationSeedItem("entity.customer.contactemail", "zh-CN", "联系人邮箱", "联系人邮箱"),
            // entity.customer.contactemail
            new TranslationSeedItem("entity.customer.contactemail", "zh-HK", "联系人邮箱", "联系人邮箱"),

            // entity.customer.currencycode
            new TranslationSeedItem("entity.customer.currencycode", "en-US", "结算币种代码", "结算币种代码"),
            // entity.customer.currencycode
            new TranslationSeedItem("entity.customer.currencycode", "ja-JP", "结算币种代码", "结算币种代码"),
            // entity.customer.currencycode
            new TranslationSeedItem("entity.customer.currencycode", "zh-CN", "结算币种代码", "结算币种代码"),
            // entity.customer.currencycode
            new TranslationSeedItem("entity.customer.currencycode", "zh-HK", "结算币种代码", "结算币种代码"),

            // entity.customer.paymentterms
            new TranslationSeedItem("entity.customer.paymentterms", "en-US", "付款条件", "付款条件（0=款到发货，1=货到付款，2=月结30天，3=月结60天，4=月结90天，5=其他）"),
            // entity.customer.paymentterms
            new TranslationSeedItem("entity.customer.paymentterms", "ja-JP", "付款条件", "付款条件（0=款到发货，1=货到付款，2=月结30天，3=月结60天，4=月结90天，5=其他）"),
            // entity.customer.paymentterms
            new TranslationSeedItem("entity.customer.paymentterms", "zh-CN", "付款条件", "付款条件（0=款到发货，1=货到付款，2=月结30天，3=月结60天，4=月结90天，5=其他）"),
            // entity.customer.paymentterms
            new TranslationSeedItem("entity.customer.paymentterms", "zh-HK", "付款条件", "付款条件（0=款到发货，1=货到付款，2=月结30天，3=月结60天，4=月结90天，5=其他）"),

            // entity.customer.creditlevel
            new TranslationSeedItem("entity.customer.creditlevel", "en-US", "信用等级", "信用等级（0=无，1=A级，2=AA级，3=AAA级，4=B级，5=C级）"),
            // entity.customer.creditlevel
            new TranslationSeedItem("entity.customer.creditlevel", "ja-JP", "信用等级", "信用等级（0=无，1=A级，2=AA级，3=AAA级，4=B级，5=C级）"),
            // entity.customer.creditlevel
            new TranslationSeedItem("entity.customer.creditlevel", "zh-CN", "信用等级", "信用等级（0=无，1=A级，2=AA级，3=AAA级，4=B级，5=C级）"),
            // entity.customer.creditlevel
            new TranslationSeedItem("entity.customer.creditlevel", "zh-HK", "信用等级", "信用等级（0=无，1=A级，2=AA级，3=AAA级，4=B级，5=C级）"),

            // entity.customer.creditamount
            new TranslationSeedItem("entity.customer.creditamount", "en-US", "信用额度", "信用额度（精确到分，存储为整数，单位为分）"),
            // entity.customer.creditamount
            new TranslationSeedItem("entity.customer.creditamount", "ja-JP", "信用额度", "信用额度（精确到分，存储为整数，单位为分）"),
            // entity.customer.creditamount
            new TranslationSeedItem("entity.customer.creditamount", "zh-CN", "信用额度", "信用额度（精确到分，存储为整数，单位为分）"),
            // entity.customer.creditamount
            new TranslationSeedItem("entity.customer.creditamount", "zh-HK", "信用额度", "信用额度（精确到分，存储为整数，单位为分）"),

            // entity.customer.discountrate
            new TranslationSeedItem("entity.customer.discountrate", "en-US", "折扣率", "折扣率（百分比，如：5.5表示5.5%折扣）"),
            // entity.customer.discountrate
            new TranslationSeedItem("entity.customer.discountrate", "ja-JP", "折扣率", "折扣率（百分比，如：5.5表示5.5%折扣）"),
            // entity.customer.discountrate
            new TranslationSeedItem("entity.customer.discountrate", "zh-CN", "折扣率", "折扣率（百分比，如：5.5表示5.5%折扣）"),
            // entity.customer.discountrate
            new TranslationSeedItem("entity.customer.discountrate", "zh-HK", "折扣率", "折扣率（百分比，如：5.5表示5.5%折扣）"),

            // entity.customer.salesby
            new TranslationSeedItem("entity.customer.salesby", "en-US", "销售员", "销售员（人员代码）"),
            // entity.customer.salesby
            new TranslationSeedItem("entity.customer.salesby", "ja-JP", "销售员", "销售员（人员代码）"),
            // entity.customer.salesby
            new TranslationSeedItem("entity.customer.salesby", "zh-CN", "销售员", "销售员（人员代码）"),
            // entity.customer.salesby
            new TranslationSeedItem("entity.customer.salesby", "zh-HK", "销售员", "销售员（人员代码）"),

            // entity.customer.level
            new TranslationSeedItem("entity.customer.level", "en-US", "客户等级", "客户等级（0=普通，1=重要，2=VIP，3=战略）"),
            // entity.customer.level
            new TranslationSeedItem("entity.customer.level", "ja-JP", "客户等级", "客户等级（0=普通，1=重要，2=VIP，3=战略）"),
            // entity.customer.level
            new TranslationSeedItem("entity.customer.level", "zh-CN", "客户等级", "客户等级（0=普通，1=重要，2=VIP，3=战略）"),
            // entity.customer.level
            new TranslationSeedItem("entity.customer.level", "zh-HK", "客户等级", "客户等级（0=普通，1=重要，2=VIP，3=战略）"),

            // entity.customer.evaluationscore
            new TranslationSeedItem("entity.customer.evaluationscore", "en-US", "评价分数", "评价分数（0-100分）"),
            // entity.customer.evaluationscore
            new TranslationSeedItem("entity.customer.evaluationscore", "ja-JP", "评价分数", "评价分数（0-100分）"),
            // entity.customer.evaluationscore
            new TranslationSeedItem("entity.customer.evaluationscore", "zh-CN", "评价分数", "评价分数（0-100分）"),
            // entity.customer.evaluationscore
            new TranslationSeedItem("entity.customer.evaluationscore", "zh-HK", "评价分数", "评价分数（0-100分）"),

            // entity.customer.isqualified
            new TranslationSeedItem("entity.customer.isqualified", "en-US", "是否合格客户", "是否合格客户（0=否，1=是）"),
            // entity.customer.isqualified
            new TranslationSeedItem("entity.customer.isqualified", "ja-JP", "是否合格客户", "是否合格客户（0=否，1=是）"),
            // entity.customer.isqualified
            new TranslationSeedItem("entity.customer.isqualified", "zh-CN", "是否合格客户", "是否合格客户（0=否，1=是）"),
            // entity.customer.isqualified
            new TranslationSeedItem("entity.customer.isqualified", "zh-HK", "是否合格客户", "是否合格客户（0=否，1=是）"),

            // entity.customer.status
            new TranslationSeedItem("entity.customer.status", "en-US", "客户状态", "客户状态（1=启用，0=禁用）"),
            // entity.customer.status
            new TranslationSeedItem("entity.customer.status", "ja-JP", "客户状态", "客户状态（1=启用，0=禁用）"),
            // entity.customer.status
            new TranslationSeedItem("entity.customer.status", "zh-CN", "客户状态", "客户状态（1=启用，0=禁用）"),
            // entity.customer.status
            new TranslationSeedItem("entity.customer.status", "zh-HK", "客户状态", "客户状态（1=启用，0=禁用）"),

            // entity.customer.sortorder
            new TranslationSeedItem("entity.customer.sortorder", "en-US", "排序号", "排序号（越小越靠前）"),
            // entity.customer.sortorder
            new TranslationSeedItem("entity.customer.sortorder", "ja-JP", "排序号", "排序号（越小越靠前）"),
            // entity.customer.sortorder
            new TranslationSeedItem("entity.customer.sortorder", "zh-CN", "排序号", "排序号（越小越靠前）"),
            // entity.customer.sortorder
            new TranslationSeedItem("entity.customer.sortorder", "zh-HK", "排序号", "排序号（越小越靠前）"),
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
        translation.ResourceGroup = TaktModule.Logistics;
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
