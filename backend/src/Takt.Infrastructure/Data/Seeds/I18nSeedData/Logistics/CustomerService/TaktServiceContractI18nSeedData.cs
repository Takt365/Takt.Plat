// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.CustomerService
// 文件名称：TaktServiceContractI18nSeedData.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktServiceContract 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.CustomerService;

/// <summary>
/// TaktServiceContract 实体国际化翻译种子（键前缀 entity.serviceContract.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktServiceContractI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktServiceContract 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 serviceContract 实体翻译...", tenantCode);

        foreach (var item in GetServiceContractTranslations())
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

        TaktLogger.Information("TaktServiceContract 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktServiceContract 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.serviceContract._self / entity.serviceContract.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetServiceContractTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.serviceContract._self
            new TranslationSeedItem("entity.serviceContract._self", "en-US", "Service Contract Information", "实体名称"),
            // entity.serviceContract._self
            new TranslationSeedItem("entity.serviceContract._self", "ja-JP", "服务合同信息", "实体名称"),
            // entity.serviceContract._self
            new TranslationSeedItem("entity.serviceContract._self", "zh-CN", "服务合同信息", "实体名称"),
            // entity.serviceContract._self
            new TranslationSeedItem("entity.serviceContract._self", "zh-HK", "服务合同信息", "实体名称"),

            // entity.serviceContract.plantcode
            new TranslationSeedItem("entity.serviceContract.plantcode", "en-US", "工厂代码", "工厂代码"),
            // entity.serviceContract.plantcode
            new TranslationSeedItem("entity.serviceContract.plantcode", "ja-JP", "工厂代码", "工厂代码"),
            // entity.serviceContract.plantcode
            new TranslationSeedItem("entity.serviceContract.plantcode", "zh-CN", "工厂代码", "工厂代码"),
            // entity.serviceContract.plantcode
            new TranslationSeedItem("entity.serviceContract.plantcode", "zh-HK", "工厂代码", "工厂代码"),

            // entity.serviceContract.code
            new TranslationSeedItem("entity.serviceContract.code", "en-US", "服务合同编码", "服务合同编码（组合唯一索引）"),
            // entity.serviceContract.code
            new TranslationSeedItem("entity.serviceContract.code", "ja-JP", "服务合同编码", "服务合同编码（组合唯一索引）"),
            // entity.serviceContract.code
            new TranslationSeedItem("entity.serviceContract.code", "zh-CN", "服务合同编码", "服务合同编码（组合唯一索引）"),
            // entity.serviceContract.code
            new TranslationSeedItem("entity.serviceContract.code", "zh-HK", "服务合同编码", "服务合同编码（组合唯一索引）"),

            // entity.serviceContract.contractname
            new TranslationSeedItem("entity.serviceContract.contractname", "en-US", "合同名称", "合同名称"),
            // entity.serviceContract.contractname
            new TranslationSeedItem("entity.serviceContract.contractname", "ja-JP", "合同名称", "合同名称"),
            // entity.serviceContract.contractname
            new TranslationSeedItem("entity.serviceContract.contractname", "zh-CN", "合同名称", "合同名称"),
            // entity.serviceContract.contractname
            new TranslationSeedItem("entity.serviceContract.contractname", "zh-HK", "合同名称", "合同名称"),

            // entity.serviceContract.clientid
            new TranslationSeedItem("entity.serviceContract.clientid", "en-US", "客户端ID", "客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）"),
            // entity.serviceContract.clientid
            new TranslationSeedItem("entity.serviceContract.clientid", "ja-JP", "客户端ID", "客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）"),
            // entity.serviceContract.clientid
            new TranslationSeedItem("entity.serviceContract.clientid", "zh-CN", "客户端ID", "客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）"),
            // entity.serviceContract.clientid
            new TranslationSeedItem("entity.serviceContract.clientid", "zh-HK", "客户端ID", "客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）"),

            // entity.serviceContract.clientcode
            new TranslationSeedItem("entity.serviceContract.clientcode", "en-US", "客户端编码", "客户端编码（冗余字段，便于查询）"),
            // entity.serviceContract.clientcode
            new TranslationSeedItem("entity.serviceContract.clientcode", "ja-JP", "客户端编码", "客户端编码（冗余字段，便于查询）"),
            // entity.serviceContract.clientcode
            new TranslationSeedItem("entity.serviceContract.clientcode", "zh-CN", "客户端编码", "客户端编码（冗余字段，便于查询）"),
            // entity.serviceContract.clientcode
            new TranslationSeedItem("entity.serviceContract.clientcode", "zh-HK", "客户端编码", "客户端编码（冗余字段，便于查询）"),

            // entity.serviceContract.clientname
            new TranslationSeedItem("entity.serviceContract.clientname", "en-US", "客户端名称", "客户端名称（冗余字段，便于查询）"),
            // entity.serviceContract.clientname
            new TranslationSeedItem("entity.serviceContract.clientname", "ja-JP", "客户端名称", "客户端名称（冗余字段，便于查询）"),
            // entity.serviceContract.clientname
            new TranslationSeedItem("entity.serviceContract.clientname", "zh-CN", "客户端名称", "客户端名称（冗余字段，便于查询）"),
            // entity.serviceContract.clientname
            new TranslationSeedItem("entity.serviceContract.clientname", "zh-HK", "客户端名称", "客户端名称（冗余字段，便于查询）"),

            // entity.serviceContract.contracttype
            new TranslationSeedItem("entity.serviceContract.contracttype", "en-US", "合同类型", "合同类型（0=维保，1=单次，2=框架，3=SLA，4=其他）"),
            // entity.serviceContract.contracttype
            new TranslationSeedItem("entity.serviceContract.contracttype", "ja-JP", "合同类型", "合同类型（0=维保，1=单次，2=框架，3=SLA，4=其他）"),
            // entity.serviceContract.contracttype
            new TranslationSeedItem("entity.serviceContract.contracttype", "zh-CN", "合同类型", "合同类型（0=维保，1=单次，2=框架，3=SLA，4=其他）"),
            // entity.serviceContract.contracttype
            new TranslationSeedItem("entity.serviceContract.contracttype", "zh-HK", "合同类型", "合同类型（0=维保，1=单次，2=框架，3=SLA，4=其他）"),

            // entity.serviceContract.contractstatus
            new TranslationSeedItem("entity.serviceContract.contractstatus", "en-US", "合同状态", "合同状态（0=草稿，1=生效，2=暂停，3=到期，4=终止）"),
            // entity.serviceContract.contractstatus
            new TranslationSeedItem("entity.serviceContract.contractstatus", "ja-JP", "合同状态", "合同状态（0=草稿，1=生效，2=暂停，3=到期，4=终止）"),
            // entity.serviceContract.contractstatus
            new TranslationSeedItem("entity.serviceContract.contractstatus", "zh-CN", "合同状态", "合同状态（0=草稿，1=生效，2=暂停，3=到期，4=终止）"),
            // entity.serviceContract.contractstatus
            new TranslationSeedItem("entity.serviceContract.contractstatus", "zh-HK", "合同状态", "合同状态（0=草稿，1=生效，2=暂停，3=到期，4=终止）"),

            // entity.serviceContract.signdate
            new TranslationSeedItem("entity.serviceContract.signdate", "en-US", "签订日期", "签订日期"),
            // entity.serviceContract.signdate
            new TranslationSeedItem("entity.serviceContract.signdate", "ja-JP", "签订日期", "签订日期"),
            // entity.serviceContract.signdate
            new TranslationSeedItem("entity.serviceContract.signdate", "zh-CN", "签订日期", "签订日期"),
            // entity.serviceContract.signdate
            new TranslationSeedItem("entity.serviceContract.signdate", "zh-HK", "签订日期", "签订日期"),

            // entity.serviceContract.effectivedate
            new TranslationSeedItem("entity.serviceContract.effectivedate", "en-US", "生效日期", "生效日期"),
            // entity.serviceContract.effectivedate
            new TranslationSeedItem("entity.serviceContract.effectivedate", "ja-JP", "生效日期", "生效日期"),
            // entity.serviceContract.effectivedate
            new TranslationSeedItem("entity.serviceContract.effectivedate", "zh-CN", "生效日期", "生效日期"),
            // entity.serviceContract.effectivedate
            new TranslationSeedItem("entity.serviceContract.effectivedate", "zh-HK", "生效日期", "生效日期"),

            // entity.serviceContract.expirydate
            new TranslationSeedItem("entity.serviceContract.expirydate", "en-US", "到期日期", "到期日期"),
            // entity.serviceContract.expirydate
            new TranslationSeedItem("entity.serviceContract.expirydate", "ja-JP", "到期日期", "到期日期"),
            // entity.serviceContract.expirydate
            new TranslationSeedItem("entity.serviceContract.expirydate", "zh-CN", "到期日期", "到期日期"),
            // entity.serviceContract.expirydate
            new TranslationSeedItem("entity.serviceContract.expirydate", "zh-HK", "到期日期", "到期日期"),

            // entity.serviceContract.contractamount
            new TranslationSeedItem("entity.serviceContract.contractamount", "en-US", "合同金额", "合同金额"),
            // entity.serviceContract.contractamount
            new TranslationSeedItem("entity.serviceContract.contractamount", "ja-JP", "合同金额", "合同金额"),
            // entity.serviceContract.contractamount
            new TranslationSeedItem("entity.serviceContract.contractamount", "zh-CN", "合同金额", "合同金额"),
            // entity.serviceContract.contractamount
            new TranslationSeedItem("entity.serviceContract.contractamount", "zh-HK", "合同金额", "合同金额"),

            // entity.serviceContract.currencycode
            new TranslationSeedItem("entity.serviceContract.currencycode", "en-US", "结算币种代码", "结算币种代码"),
            // entity.serviceContract.currencycode
            new TranslationSeedItem("entity.serviceContract.currencycode", "ja-JP", "结算币种代码", "结算币种代码"),
            // entity.serviceContract.currencycode
            new TranslationSeedItem("entity.serviceContract.currencycode", "zh-CN", "结算币种代码", "结算币种代码"),
            // entity.serviceContract.currencycode
            new TranslationSeedItem("entity.serviceContract.currencycode", "zh-HK", "结算币种代码", "结算币种代码"),

            // entity.serviceContract.paymentterms
            new TranslationSeedItem("entity.serviceContract.paymentterms", "en-US", "付款条件", "付款条件（0=预付，1=后付，2=月结30天，3=月结60天，4=其他）"),
            // entity.serviceContract.paymentterms
            new TranslationSeedItem("entity.serviceContract.paymentterms", "ja-JP", "付款条件", "付款条件（0=预付，1=后付，2=月结30天，3=月结60天，4=其他）"),
            // entity.serviceContract.paymentterms
            new TranslationSeedItem("entity.serviceContract.paymentterms", "zh-CN", "付款条件", "付款条件（0=预付，1=后付，2=月结30天，3=月结60天，4=其他）"),
            // entity.serviceContract.paymentterms
            new TranslationSeedItem("entity.serviceContract.paymentterms", "zh-HK", "付款条件", "付款条件（0=预付，1=后付，2=月结30天，3=月结60天，4=其他）"),

            // entity.serviceContract.servicescope
            new TranslationSeedItem("entity.serviceContract.servicescope", "en-US", "服务范围描述", "服务范围描述"),
            // entity.serviceContract.servicescope
            new TranslationSeedItem("entity.serviceContract.servicescope", "ja-JP", "服务范围描述", "服务范围描述"),
            // entity.serviceContract.servicescope
            new TranslationSeedItem("entity.serviceContract.servicescope", "zh-CN", "服务范围描述", "服务范围描述"),
            // entity.serviceContract.servicescope
            new TranslationSeedItem("entity.serviceContract.servicescope", "zh-HK", "服务范围描述", "服务范围描述"),

            // entity.serviceContract.slaresponsehours
            new TranslationSeedItem("entity.serviceContract.slaresponsehours", "en-US", "SLA响应时限（小时）", "SLA 响应时限（小时）"),
            // entity.serviceContract.slaresponsehours
            new TranslationSeedItem("entity.serviceContract.slaresponsehours", "ja-JP", "SLA响应时限（小时）", "SLA 响应时限（小时）"),
            // entity.serviceContract.slaresponsehours
            new TranslationSeedItem("entity.serviceContract.slaresponsehours", "zh-CN", "SLA响应时限（小时）", "SLA 响应时限（小时）"),
            // entity.serviceContract.slaresponsehours
            new TranslationSeedItem("entity.serviceContract.slaresponsehours", "zh-HK", "SLA响应时限（小时）", "SLA 响应时限（小时）"),

            // entity.serviceContract.slaresolvehours
            new TranslationSeedItem("entity.serviceContract.slaresolvehours", "en-US", "SLA解决时限（小时）", "SLA 解决时限（小时）"),
            // entity.serviceContract.slaresolvehours
            new TranslationSeedItem("entity.serviceContract.slaresolvehours", "ja-JP", "SLA解决时限（小时）", "SLA 解决时限（小时）"),
            // entity.serviceContract.slaresolvehours
            new TranslationSeedItem("entity.serviceContract.slaresolvehours", "zh-CN", "SLA解决时限（小时）", "SLA 解决时限（小时）"),
            // entity.serviceContract.slaresolvehours
            new TranslationSeedItem("entity.serviceContract.slaresolvehours", "zh-HK", "SLA解决时限（小时）", "SLA 解决时限（小时）"),

            // entity.serviceContract.accountmanager
            new TranslationSeedItem("entity.serviceContract.accountmanager", "en-US", "客户经理", "客户经理（人员代码）"),
            // entity.serviceContract.accountmanager
            new TranslationSeedItem("entity.serviceContract.accountmanager", "ja-JP", "客户经理", "客户经理（人员代码）"),
            // entity.serviceContract.accountmanager
            new TranslationSeedItem("entity.serviceContract.accountmanager", "zh-CN", "客户经理", "客户经理（人员代码）"),
            // entity.serviceContract.accountmanager
            new TranslationSeedItem("entity.serviceContract.accountmanager", "zh-HK", "客户经理", "客户经理（人员代码）"),

            // entity.serviceContract.sortorder
            new TranslationSeedItem("entity.serviceContract.sortorder", "en-US", "排序号", "排序号（越小越靠前）"),
            // entity.serviceContract.sortorder
            new TranslationSeedItem("entity.serviceContract.sortorder", "ja-JP", "排序号", "排序号（越小越靠前）"),
            // entity.serviceContract.sortorder
            new TranslationSeedItem("entity.serviceContract.sortorder", "zh-CN", "排序号", "排序号（越小越靠前）"),
            // entity.serviceContract.sortorder
            new TranslationSeedItem("entity.serviceContract.sortorder", "zh-HK", "排序号", "排序号（越小越靠前）"),

            // entity.serviceContract.serviceorders
            new TranslationSeedItem("entity.serviceContract.serviceorders", "en-US", "服务订单列表", "服务订单列表（外键在子表 <see cref=\"TaktServiceOrder.ServiceContractId\"/>）"),
            // entity.serviceContract.serviceorders
            new TranslationSeedItem("entity.serviceContract.serviceorders", "ja-JP", "服务订单列表", "服务订单列表（外键在子表 <see cref=\"TaktServiceOrder.ServiceContractId\"/>）"),
            // entity.serviceContract.serviceorders
            new TranslationSeedItem("entity.serviceContract.serviceorders", "zh-CN", "服务订单列表", "服务订单列表（外键在子表 <see cref=\"TaktServiceOrder.ServiceContractId\"/>）"),
            // entity.serviceContract.serviceorders
            new TranslationSeedItem("entity.serviceContract.serviceorders", "zh-HK", "服务订单列表", "服务订单列表（外键在子表 <see cref=\"TaktServiceOrder.ServiceContractId\"/>）"),

            // entity.serviceContract.servicerequests
            new TranslationSeedItem("entity.serviceContract.servicerequests", "en-US", "服务请求列表", "服务请求列表（外键在子表 <see cref=\"TaktServiceRequest.ServiceContractId\"/>）"),
            // entity.serviceContract.servicerequests
            new TranslationSeedItem("entity.serviceContract.servicerequests", "ja-JP", "服务请求列表", "服务请求列表（外键在子表 <see cref=\"TaktServiceRequest.ServiceContractId\"/>）"),
            // entity.serviceContract.servicerequests
            new TranslationSeedItem("entity.serviceContract.servicerequests", "zh-CN", "服务请求列表", "服务请求列表（外键在子表 <see cref=\"TaktServiceRequest.ServiceContractId\"/>）"),
            // entity.serviceContract.servicerequests
            new TranslationSeedItem("entity.serviceContract.servicerequests", "zh-HK", "服务请求列表", "服务请求列表（外键在子表 <see cref=\"TaktServiceRequest.ServiceContractId\"/>）"),
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
