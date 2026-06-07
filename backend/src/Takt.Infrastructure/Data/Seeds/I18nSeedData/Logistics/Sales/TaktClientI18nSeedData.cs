// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Sales
// 文件名称：TaktClientI18nSeedData.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktClient 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktClient 实体国际化翻译种子（键前缀 entity.client.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktClientI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktClient 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 client 实体翻译...", tenantCode);

        foreach (var item in GetClientTranslations())
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

        TaktLogger.Information("TaktClient 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktClient 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.client._self / entity.client.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetClientTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.client._self
            new TranslationSeedItem("entity.client._self", "en-US", "Client Information", "实体名称"),
            // entity.client._self
            new TranslationSeedItem("entity.client._self", "ja-JP", "Takt客户端信息信息", "实体名称"),
            // entity.client._self
            new TranslationSeedItem("entity.client._self", "zh-CN", "Takt客户端信息信息", "实体名称"),
            // entity.client._self
            new TranslationSeedItem("entity.client._self", "zh-HK", "Takt客户端信息信息", "实体名称"),

            // entity.client.plantcode
            new TranslationSeedItem("entity.client.plantcode", "en-US", "工厂代码", "工厂代码"),
            // entity.client.plantcode
            new TranslationSeedItem("entity.client.plantcode", "ja-JP", "工厂代码", "工厂代码"),
            // entity.client.plantcode
            new TranslationSeedItem("entity.client.plantcode", "zh-CN", "工厂代码", "工厂代码"),
            // entity.client.plantcode
            new TranslationSeedItem("entity.client.plantcode", "zh-HK", "工厂代码", "工厂代码"),

            // entity.client.code
            new TranslationSeedItem("entity.client.code", "en-US", "客户端编码", "客户端编码（唯一索引）"),
            // entity.client.code
            new TranslationSeedItem("entity.client.code", "ja-JP", "客户端编码", "客户端编码（唯一索引）"),
            // entity.client.code
            new TranslationSeedItem("entity.client.code", "zh-CN", "客户端编码", "客户端编码（唯一索引）"),
            // entity.client.code
            new TranslationSeedItem("entity.client.code", "zh-HK", "客户端编码", "客户端编码（唯一索引）"),

            // entity.client.name
            new TranslationSeedItem("entity.client.name", "en-US", "客户端名称", "客户端名称"),
            // entity.client.name
            new TranslationSeedItem("entity.client.name", "ja-JP", "客户端名称", "客户端名称"),
            // entity.client.name
            new TranslationSeedItem("entity.client.name", "zh-CN", "客户端名称", "客户端名称"),
            // entity.client.name
            new TranslationSeedItem("entity.client.name", "zh-HK", "客户端名称", "客户端名称"),

            // entity.client.shortname
            new TranslationSeedItem("entity.client.shortname", "en-US", "客户端简称", "客户端简称"),
            // entity.client.shortname
            new TranslationSeedItem("entity.client.shortname", "ja-JP", "客户端简称", "客户端简称"),
            // entity.client.shortname
            new TranslationSeedItem("entity.client.shortname", "zh-CN", "客户端简称", "客户端简称"),
            // entity.client.shortname
            new TranslationSeedItem("entity.client.shortname", "zh-HK", "客户端简称", "客户端简称"),

            // entity.client.type
            new TranslationSeedItem("entity.client.type", "en-US", "客户端类型", "客户端类型（0=终端客户，1=分销商，2=零售商，3=电商平台，4=其他）"),
            // entity.client.type
            new TranslationSeedItem("entity.client.type", "ja-JP", "客户端类型", "客户端类型（0=终端客户，1=分销商，2=零售商，3=电商平台，4=其他）"),
            // entity.client.type
            new TranslationSeedItem("entity.client.type", "zh-CN", "客户端类型", "客户端类型（0=终端客户，1=分销商，2=零售商，3=电商平台，4=其他）"),
            // entity.client.type
            new TranslationSeedItem("entity.client.type", "zh-HK", "客户端类型", "客户端类型（0=终端客户，1=分销商，2=零售商，3=电商平台，4=其他）"),

            // entity.client.industrysector
            new TranslationSeedItem("entity.client.industrysector", "en-US", "行业领域", "行业领域"),
            // entity.client.industrysector
            new TranslationSeedItem("entity.client.industrysector", "ja-JP", "行业领域", "行业领域"),
            // entity.client.industrysector
            new TranslationSeedItem("entity.client.industrysector", "zh-CN", "行业领域", "行业领域"),
            // entity.client.industrysector
            new TranslationSeedItem("entity.client.industrysector", "zh-HK", "行业领域", "行业领域"),

            // entity.client.taxnumber
            new TranslationSeedItem("entity.client.taxnumber", "en-US", "客户端标识", "客户端标识（税务登记证号/统一社会信用代码）"),
            // entity.client.taxnumber
            new TranslationSeedItem("entity.client.taxnumber", "ja-JP", "客户端标识", "客户端标识（税务登记证号/统一社会信用代码）"),
            // entity.client.taxnumber
            new TranslationSeedItem("entity.client.taxnumber", "zh-CN", "客户端标识", "客户端标识（税务登记证号/统一社会信用代码）"),
            // entity.client.taxnumber
            new TranslationSeedItem("entity.client.taxnumber", "zh-HK", "客户端标识", "客户端标识（税务登记证号/统一社会信用代码）"),

            // entity.client.registrationcountry
            new TranslationSeedItem("entity.client.registrationcountry", "en-US", "注册国家", "注册国家（ISO 3166-1 alpha-2两位代码）"),
            // entity.client.registrationcountry
            new TranslationSeedItem("entity.client.registrationcountry", "ja-JP", "注册国家", "注册国家（ISO 3166-1 alpha-2两位代码）"),
            // entity.client.registrationcountry
            new TranslationSeedItem("entity.client.registrationcountry", "zh-CN", "注册国家", "注册国家（ISO 3166-1 alpha-2两位代码）"),
            // entity.client.registrationcountry
            new TranslationSeedItem("entity.client.registrationcountry", "zh-HK", "注册国家", "注册国家（ISO 3166-1 alpha-2两位代码）"),

            // entity.client.registrationaddress1
            new TranslationSeedItem("entity.client.registrationaddress1", "en-US", "注册地址1", "注册地址1"),
            // entity.client.registrationaddress1
            new TranslationSeedItem("entity.client.registrationaddress1", "ja-JP", "注册地址1", "注册地址1"),
            // entity.client.registrationaddress1
            new TranslationSeedItem("entity.client.registrationaddress1", "zh-CN", "注册地址1", "注册地址1"),
            // entity.client.registrationaddress1
            new TranslationSeedItem("entity.client.registrationaddress1", "zh-HK", "注册地址1", "注册地址1"),

            // entity.client.registrationaddress2
            new TranslationSeedItem("entity.client.registrationaddress2", "en-US", "注册地址2", "注册地址2"),
            // entity.client.registrationaddress2
            new TranslationSeedItem("entity.client.registrationaddress2", "ja-JP", "注册地址2", "注册地址2"),
            // entity.client.registrationaddress2
            new TranslationSeedItem("entity.client.registrationaddress2", "zh-CN", "注册地址2", "注册地址2"),
            // entity.client.registrationaddress2
            new TranslationSeedItem("entity.client.registrationaddress2", "zh-HK", "注册地址2", "注册地址2"),

            // entity.client.registrationaddress3
            new TranslationSeedItem("entity.client.registrationaddress3", "en-US", "注册地址3", "注册地址3"),
            // entity.client.registrationaddress3
            new TranslationSeedItem("entity.client.registrationaddress3", "ja-JP", "注册地址3", "注册地址3"),
            // entity.client.registrationaddress3
            new TranslationSeedItem("entity.client.registrationaddress3", "zh-CN", "注册地址3", "注册地址3"),
            // entity.client.registrationaddress3
            new TranslationSeedItem("entity.client.registrationaddress3", "zh-HK", "注册地址3", "注册地址3"),

            // entity.client.phone
            new TranslationSeedItem("entity.client.phone", "en-US", "客户端电话", "客户端电话"),
            // entity.client.phone
            new TranslationSeedItem("entity.client.phone", "ja-JP", "客户端电话", "客户端电话"),
            // entity.client.phone
            new TranslationSeedItem("entity.client.phone", "zh-CN", "客户端电话", "客户端电话"),
            // entity.client.phone
            new TranslationSeedItem("entity.client.phone", "zh-HK", "客户端电话", "客户端电话"),

            // entity.client.fax
            new TranslationSeedItem("entity.client.fax", "en-US", "客户端传真", "客户端传真"),
            // entity.client.fax
            new TranslationSeedItem("entity.client.fax", "ja-JP", "客户端传真", "客户端传真"),
            // entity.client.fax
            new TranslationSeedItem("entity.client.fax", "zh-CN", "客户端传真", "客户端传真"),
            // entity.client.fax
            new TranslationSeedItem("entity.client.fax", "zh-HK", "客户端传真", "客户端传真"),

            // entity.client.email
            new TranslationSeedItem("entity.client.email", "en-US", "客户端邮箱", "客户端邮箱"),
            // entity.client.email
            new TranslationSeedItem("entity.client.email", "ja-JP", "客户端邮箱", "客户端邮箱"),
            // entity.client.email
            new TranslationSeedItem("entity.client.email", "zh-CN", "客户端邮箱", "客户端邮箱"),
            // entity.client.email
            new TranslationSeedItem("entity.client.email", "zh-HK", "客户端邮箱", "客户端邮箱"),

            // entity.client.website
            new TranslationSeedItem("entity.client.website", "en-US", "客户端网站", "客户端网站"),
            // entity.client.website
            new TranslationSeedItem("entity.client.website", "ja-JP", "客户端网站", "客户端网站"),
            // entity.client.website
            new TranslationSeedItem("entity.client.website", "zh-CN", "客户端网站", "客户端网站"),
            // entity.client.website
            new TranslationSeedItem("entity.client.website", "zh-HK", "客户端网站", "客户端网站"),

            // entity.client.contactperson
            new TranslationSeedItem("entity.client.contactperson", "en-US", "联系人", "联系人"),
            // entity.client.contactperson
            new TranslationSeedItem("entity.client.contactperson", "ja-JP", "联系人", "联系人"),
            // entity.client.contactperson
            new TranslationSeedItem("entity.client.contactperson", "zh-CN", "联系人", "联系人"),
            // entity.client.contactperson
            new TranslationSeedItem("entity.client.contactperson", "zh-HK", "联系人", "联系人"),

            // entity.client.contactphone
            new TranslationSeedItem("entity.client.contactphone", "en-US", "联系人电话", "联系人电话"),
            // entity.client.contactphone
            new TranslationSeedItem("entity.client.contactphone", "ja-JP", "联系人电话", "联系人电话"),
            // entity.client.contactphone
            new TranslationSeedItem("entity.client.contactphone", "zh-CN", "联系人电话", "联系人电话"),
            // entity.client.contactphone
            new TranslationSeedItem("entity.client.contactphone", "zh-HK", "联系人电话", "联系人电话"),

            // entity.client.contactemail
            new TranslationSeedItem("entity.client.contactemail", "en-US", "联系人邮箱", "联系人邮箱"),
            // entity.client.contactemail
            new TranslationSeedItem("entity.client.contactemail", "ja-JP", "联系人邮箱", "联系人邮箱"),
            // entity.client.contactemail
            new TranslationSeedItem("entity.client.contactemail", "zh-CN", "联系人邮箱", "联系人邮箱"),
            // entity.client.contactemail
            new TranslationSeedItem("entity.client.contactemail", "zh-HK", "联系人邮箱", "联系人邮箱"),

            // entity.client.currencycode
            new TranslationSeedItem("entity.client.currencycode", "en-US", "结算币种代码", "结算币种代码"),
            // entity.client.currencycode
            new TranslationSeedItem("entity.client.currencycode", "ja-JP", "结算币种代码", "结算币种代码"),
            // entity.client.currencycode
            new TranslationSeedItem("entity.client.currencycode", "zh-CN", "结算币种代码", "结算币种代码"),
            // entity.client.currencycode
            new TranslationSeedItem("entity.client.currencycode", "zh-HK", "结算币种代码", "结算币种代码"),

            // entity.client.paymentterms
            new TranslationSeedItem("entity.client.paymentterms", "en-US", "付款条件", "付款条件（0=款到发货，1=货到付款，2=月结30天，3=月结60天，4=月结90天，5=其他）"),
            // entity.client.paymentterms
            new TranslationSeedItem("entity.client.paymentterms", "ja-JP", "付款条件", "付款条件（0=款到发货，1=货到付款，2=月结30天，3=月结60天，4=月结90天，5=其他）"),
            // entity.client.paymentterms
            new TranslationSeedItem("entity.client.paymentterms", "zh-CN", "付款条件", "付款条件（0=款到发货，1=货到付款，2=月结30天，3=月结60天，4=月结90天，5=其他）"),
            // entity.client.paymentterms
            new TranslationSeedItem("entity.client.paymentterms", "zh-HK", "付款条件", "付款条件（0=款到发货，1=货到付款，2=月结30天，3=月结60天，4=月结90天，5=其他）"),

            // entity.client.saleschannel
            new TranslationSeedItem("entity.client.saleschannel", "en-US", "销售渠道", "销售渠道（0=直销，1=经销，2=代销，3=电商，4=其他）"),
            // entity.client.saleschannel
            new TranslationSeedItem("entity.client.saleschannel", "ja-JP", "销售渠道", "销售渠道（0=直销，1=经销，2=代销，3=电商，4=其他）"),
            // entity.client.saleschannel
            new TranslationSeedItem("entity.client.saleschannel", "zh-CN", "销售渠道", "销售渠道（0=直销，1=经销，2=代销，3=电商，4=其他）"),
            // entity.client.saleschannel
            new TranslationSeedItem("entity.client.saleschannel", "zh-HK", "销售渠道", "销售渠道（0=直销，1=经销，2=代销，3=电商，4=其他）"),

            // entity.client.platformname
            new TranslationSeedItem("entity.client.platformname", "en-US", "平台名称", "平台名称（电商平台名称）"),
            // entity.client.platformname
            new TranslationSeedItem("entity.client.platformname", "ja-JP", "平台名称", "平台名称（电商平台名称）"),
            // entity.client.platformname
            new TranslationSeedItem("entity.client.platformname", "zh-CN", "平台名称", "平台名称（电商平台名称）"),
            // entity.client.platformname
            new TranslationSeedItem("entity.client.platformname", "zh-HK", "平台名称", "平台名称（电商平台名称）"),

            // entity.client.storename
            new TranslationSeedItem("entity.client.storename", "en-US", "店铺名称", "店铺名称"),
            // entity.client.storename
            new TranslationSeedItem("entity.client.storename", "ja-JP", "店铺名称", "店铺名称"),
            // entity.client.storename
            new TranslationSeedItem("entity.client.storename", "zh-CN", "店铺名称", "店铺名称"),
            // entity.client.storename
            new TranslationSeedItem("entity.client.storename", "zh-HK", "店铺名称", "店铺名称"),

            // entity.client.level
            new TranslationSeedItem("entity.client.level", "en-US", "客户端等级", "客户端等级（0=普通，1=重要，2=VIP，3=战略）"),
            // entity.client.level
            new TranslationSeedItem("entity.client.level", "ja-JP", "客户端等级", "客户端等级（0=普通，1=重要，2=VIP，3=战略）"),
            // entity.client.level
            new TranslationSeedItem("entity.client.level", "zh-CN", "客户端等级", "客户端等级（0=普通，1=重要，2=VIP，3=战略）"),
            // entity.client.level
            new TranslationSeedItem("entity.client.level", "zh-HK", "客户端等级", "客户端等级（0=普通，1=重要，2=VIP，3=战略）"),

            // entity.client.evaluationscore
            new TranslationSeedItem("entity.client.evaluationscore", "en-US", "评价分数", "评价分数（0-100分）"),
            // entity.client.evaluationscore
            new TranslationSeedItem("entity.client.evaluationscore", "ja-JP", "评价分数", "评价分数（0-100分）"),
            // entity.client.evaluationscore
            new TranslationSeedItem("entity.client.evaluationscore", "zh-CN", "评价分数", "评价分数（0-100分）"),
            // entity.client.evaluationscore
            new TranslationSeedItem("entity.client.evaluationscore", "zh-HK", "评价分数", "评价分数（0-100分）"),

            // entity.client.isqualified
            new TranslationSeedItem("entity.client.isqualified", "en-US", "是否合格客户端", "是否合格客户端（0=否，1=是）"),
            // entity.client.isqualified
            new TranslationSeedItem("entity.client.isqualified", "ja-JP", "是否合格客户端", "是否合格客户端（0=否，1=是）"),
            // entity.client.isqualified
            new TranslationSeedItem("entity.client.isqualified", "zh-CN", "是否合格客户端", "是否合格客户端（0=否，1=是）"),
            // entity.client.isqualified
            new TranslationSeedItem("entity.client.isqualified", "zh-HK", "是否合格客户端", "是否合格客户端（0=否，1=是）"),

            // entity.client.status
            new TranslationSeedItem("entity.client.status", "en-US", "客户端状态", "客户端状态（1=启用，0=禁用）"),
            // entity.client.status
            new TranslationSeedItem("entity.client.status", "ja-JP", "客户端状态", "客户端状态（1=启用，0=禁用）"),
            // entity.client.status
            new TranslationSeedItem("entity.client.status", "zh-CN", "客户端状态", "客户端状态（1=启用，0=禁用）"),
            // entity.client.status
            new TranslationSeedItem("entity.client.status", "zh-HK", "客户端状态", "客户端状态（1=启用，0=禁用）"),

            // entity.client.sortorder
            new TranslationSeedItem("entity.client.sortorder", "en-US", "排序号", "排序号（越小越靠前）"),
            // entity.client.sortorder
            new TranslationSeedItem("entity.client.sortorder", "ja-JP", "排序号", "排序号（越小越靠前）"),
            // entity.client.sortorder
            new TranslationSeedItem("entity.client.sortorder", "zh-CN", "排序号", "排序号（越小越靠前）"),
            // entity.client.sortorder
            new TranslationSeedItem("entity.client.sortorder", "zh-HK", "排序号", "排序号（越小越靠前）"),
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
