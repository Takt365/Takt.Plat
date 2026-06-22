// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.CustomerService
// 文件名称：TaktServiceContractI18nSeedData.cs
// 创建时间：2026-06-22
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
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.CustomerService;

/// <summary>
/// TaktServiceContract 实体国际化翻译种子（键前缀 entity.servicecontract.*）
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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 servicecontract 实体翻译...", tenantCode);

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
    /// I18nKey：entity.servicecontract._self / entity.servicecontract.{{field}}；ResourceGroup=CustomerService；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetServiceContractTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.servicecontract._self
            new TranslationSeedItem("entity.servicecontract._self", "en-US", "Service Contract Information_us", "实体名称"),
            // entity.servicecontract._self
            new TranslationSeedItem("entity.servicecontract._self", "ja-JP", "服务合同信息_jp", "实体名称"),
            // entity.servicecontract._self
            new TranslationSeedItem("entity.servicecontract._self", "zh-CN", "服务合同信息", "实体名称"),
            // entity.servicecontract._self
            new TranslationSeedItem("entity.servicecontract._self", "zh-HK", "服务合同信息_hk", "实体名称"),

            // entity.servicecontract.plantcode
            new TranslationSeedItem("entity.servicecontract.plantcode", "en-US", "工厂代码_us", "工厂代码"),
            // entity.servicecontract.plantcode
            new TranslationSeedItem("entity.servicecontract.plantcode", "ja-JP", "工厂代码_jp", "工厂代码"),
            // entity.servicecontract.plantcode
            new TranslationSeedItem("entity.servicecontract.plantcode", "zh-CN", "工厂代码", "工厂代码"),
            // entity.servicecontract.plantcode
            new TranslationSeedItem("entity.servicecontract.plantcode", "zh-HK", "工厂代码_hk", "工厂代码"),

            // entity.servicecontract.code
            new TranslationSeedItem("entity.servicecontract.code", "en-US", "服务合同编码_us", "服务合同编码（组合唯一索引）"),
            // entity.servicecontract.code
            new TranslationSeedItem("entity.servicecontract.code", "ja-JP", "服务合同编码_jp", "服务合同编码（组合唯一索引）"),
            // entity.servicecontract.code
            new TranslationSeedItem("entity.servicecontract.code", "zh-CN", "服务合同编码", "服务合同编码（组合唯一索引）"),
            // entity.servicecontract.code
            new TranslationSeedItem("entity.servicecontract.code", "zh-HK", "服务合同编码_hk", "服务合同编码（组合唯一索引）"),

            // entity.servicecontract.contractname
            new TranslationSeedItem("entity.servicecontract.contractname", "en-US", "合同名称_us", "合同名称"),
            // entity.servicecontract.contractname
            new TranslationSeedItem("entity.servicecontract.contractname", "ja-JP", "合同名称_jp", "合同名称"),
            // entity.servicecontract.contractname
            new TranslationSeedItem("entity.servicecontract.contractname", "zh-CN", "合同名称", "合同名称"),
            // entity.servicecontract.contractname
            new TranslationSeedItem("entity.servicecontract.contractname", "zh-HK", "合同名称_hk", "合同名称"),

            // entity.servicecontract.clientid
            new TranslationSeedItem("entity.servicecontract.clientid", "en-US", "客户端ID_us", "客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）"),
            // entity.servicecontract.clientid
            new TranslationSeedItem("entity.servicecontract.clientid", "ja-JP", "客户端ID_jp", "客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）"),
            // entity.servicecontract.clientid
            new TranslationSeedItem("entity.servicecontract.clientid", "zh-CN", "客户端ID", "客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）"),
            // entity.servicecontract.clientid
            new TranslationSeedItem("entity.servicecontract.clientid", "zh-HK", "客户端ID_hk", "客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）"),

            // entity.servicecontract.clientcode
            new TranslationSeedItem("entity.servicecontract.clientcode", "en-US", "客户端编码_us", "客户端编码（冗余字段，便于查询）"),
            // entity.servicecontract.clientcode
            new TranslationSeedItem("entity.servicecontract.clientcode", "ja-JP", "客户端编码_jp", "客户端编码（冗余字段，便于查询）"),
            // entity.servicecontract.clientcode
            new TranslationSeedItem("entity.servicecontract.clientcode", "zh-CN", "客户端编码", "客户端编码（冗余字段，便于查询）"),
            // entity.servicecontract.clientcode
            new TranslationSeedItem("entity.servicecontract.clientcode", "zh-HK", "客户端编码_hk", "客户端编码（冗余字段，便于查询）"),

            // entity.servicecontract.clientname
            new TranslationSeedItem("entity.servicecontract.clientname", "en-US", "客户端名称_us", "客户端名称（冗余字段，便于查询）"),
            // entity.servicecontract.clientname
            new TranslationSeedItem("entity.servicecontract.clientname", "ja-JP", "客户端名称_jp", "客户端名称（冗余字段，便于查询）"),
            // entity.servicecontract.clientname
            new TranslationSeedItem("entity.servicecontract.clientname", "zh-CN", "客户端名称", "客户端名称（冗余字段，便于查询）"),
            // entity.servicecontract.clientname
            new TranslationSeedItem("entity.servicecontract.clientname", "zh-HK", "客户端名称_hk", "客户端名称（冗余字段，便于查询）"),

            // entity.servicecontract.contracttype
            new TranslationSeedItem("entity.servicecontract.contracttype", "en-US", "合同类型_us", "合同类型（0=维保，1=单次，2=框架，3=SLA，4=其他）"),
            // entity.servicecontract.contracttype
            new TranslationSeedItem("entity.servicecontract.contracttype", "ja-JP", "合同类型_jp", "合同类型（0=维保，1=单次，2=框架，3=SLA，4=其他）"),
            // entity.servicecontract.contracttype
            new TranslationSeedItem("entity.servicecontract.contracttype", "zh-CN", "合同类型", "合同类型（0=维保，1=单次，2=框架，3=SLA，4=其他）"),
            // entity.servicecontract.contracttype
            new TranslationSeedItem("entity.servicecontract.contracttype", "zh-HK", "合同类型_hk", "合同类型（0=维保，1=单次，2=框架，3=SLA，4=其他）"),

            // entity.servicecontract.contractstatus
            new TranslationSeedItem("entity.servicecontract.contractstatus", "en-US", "合同状态_us", "合同状态（0=草稿，1=生效，2=暂停，3=到期，4=终止）"),
            // entity.servicecontract.contractstatus
            new TranslationSeedItem("entity.servicecontract.contractstatus", "ja-JP", "合同状态_jp", "合同状态（0=草稿，1=生效，2=暂停，3=到期，4=终止）"),
            // entity.servicecontract.contractstatus
            new TranslationSeedItem("entity.servicecontract.contractstatus", "zh-CN", "合同状态", "合同状态（0=草稿，1=生效，2=暂停，3=到期，4=终止）"),
            // entity.servicecontract.contractstatus
            new TranslationSeedItem("entity.servicecontract.contractstatus", "zh-HK", "合同状态_hk", "合同状态（0=草稿，1=生效，2=暂停，3=到期，4=终止）"),

            // entity.servicecontract.signdate
            new TranslationSeedItem("entity.servicecontract.signdate", "en-US", "签订日期_us", "签订日期"),
            // entity.servicecontract.signdate
            new TranslationSeedItem("entity.servicecontract.signdate", "ja-JP", "签订日期_jp", "签订日期"),
            // entity.servicecontract.signdate
            new TranslationSeedItem("entity.servicecontract.signdate", "zh-CN", "签订日期", "签订日期"),
            // entity.servicecontract.signdate
            new TranslationSeedItem("entity.servicecontract.signdate", "zh-HK", "签订日期_hk", "签订日期"),

            // entity.servicecontract.effectivedate
            new TranslationSeedItem("entity.servicecontract.effectivedate", "en-US", "生效日期_us", "生效日期"),
            // entity.servicecontract.effectivedate
            new TranslationSeedItem("entity.servicecontract.effectivedate", "ja-JP", "生效日期_jp", "生效日期"),
            // entity.servicecontract.effectivedate
            new TranslationSeedItem("entity.servicecontract.effectivedate", "zh-CN", "生效日期", "生效日期"),
            // entity.servicecontract.effectivedate
            new TranslationSeedItem("entity.servicecontract.effectivedate", "zh-HK", "生效日期_hk", "生效日期"),

            // entity.servicecontract.expirydate
            new TranslationSeedItem("entity.servicecontract.expirydate", "en-US", "到期日期_us", "到期日期"),
            // entity.servicecontract.expirydate
            new TranslationSeedItem("entity.servicecontract.expirydate", "ja-JP", "到期日期_jp", "到期日期"),
            // entity.servicecontract.expirydate
            new TranslationSeedItem("entity.servicecontract.expirydate", "zh-CN", "到期日期", "到期日期"),
            // entity.servicecontract.expirydate
            new TranslationSeedItem("entity.servicecontract.expirydate", "zh-HK", "到期日期_hk", "到期日期"),

            // entity.servicecontract.contractamount
            new TranslationSeedItem("entity.servicecontract.contractamount", "en-US", "合同金额_us", "合同金额"),
            // entity.servicecontract.contractamount
            new TranslationSeedItem("entity.servicecontract.contractamount", "ja-JP", "合同金额_jp", "合同金额"),
            // entity.servicecontract.contractamount
            new TranslationSeedItem("entity.servicecontract.contractamount", "zh-CN", "合同金额", "合同金额"),
            // entity.servicecontract.contractamount
            new TranslationSeedItem("entity.servicecontract.contractamount", "zh-HK", "合同金额_hk", "合同金额"),

            // entity.servicecontract.currencycode
            new TranslationSeedItem("entity.servicecontract.currencycode", "en-US", "结算币种代码_us", "结算币种代码"),
            // entity.servicecontract.currencycode
            new TranslationSeedItem("entity.servicecontract.currencycode", "ja-JP", "结算币种代码_jp", "结算币种代码"),
            // entity.servicecontract.currencycode
            new TranslationSeedItem("entity.servicecontract.currencycode", "zh-CN", "结算币种代码", "结算币种代码"),
            // entity.servicecontract.currencycode
            new TranslationSeedItem("entity.servicecontract.currencycode", "zh-HK", "结算币种代码_hk", "结算币种代码"),

            // entity.servicecontract.paymentterms
            new TranslationSeedItem("entity.servicecontract.paymentterms", "en-US", "付款条件_us", "付款条件（0=预付，1=后付，2=月结30天，3=月结60天，4=其他）"),
            // entity.servicecontract.paymentterms
            new TranslationSeedItem("entity.servicecontract.paymentterms", "ja-JP", "付款条件_jp", "付款条件（0=预付，1=后付，2=月结30天，3=月结60天，4=其他）"),
            // entity.servicecontract.paymentterms
            new TranslationSeedItem("entity.servicecontract.paymentterms", "zh-CN", "付款条件", "付款条件（0=预付，1=后付，2=月结30天，3=月结60天，4=其他）"),
            // entity.servicecontract.paymentterms
            new TranslationSeedItem("entity.servicecontract.paymentterms", "zh-HK", "付款条件_hk", "付款条件（0=预付，1=后付，2=月结30天，3=月结60天，4=其他）"),

            // entity.servicecontract.servicescope
            new TranslationSeedItem("entity.servicecontract.servicescope", "en-US", "服务范围描述_us", "服务范围描述"),
            // entity.servicecontract.servicescope
            new TranslationSeedItem("entity.servicecontract.servicescope", "ja-JP", "服务范围描述_jp", "服务范围描述"),
            // entity.servicecontract.servicescope
            new TranslationSeedItem("entity.servicecontract.servicescope", "zh-CN", "服务范围描述", "服务范围描述"),
            // entity.servicecontract.servicescope
            new TranslationSeedItem("entity.servicecontract.servicescope", "zh-HK", "服务范围描述_hk", "服务范围描述"),

            // entity.servicecontract.slaresponsehours
            new TranslationSeedItem("entity.servicecontract.slaresponsehours", "en-US", "SLA响应时限（小时）_us", "SLA 响应时限（小时）"),
            // entity.servicecontract.slaresponsehours
            new TranslationSeedItem("entity.servicecontract.slaresponsehours", "ja-JP", "SLA响应时限（小时）_jp", "SLA 响应时限（小时）"),
            // entity.servicecontract.slaresponsehours
            new TranslationSeedItem("entity.servicecontract.slaresponsehours", "zh-CN", "SLA响应时限（小时）", "SLA 响应时限（小时）"),
            // entity.servicecontract.slaresponsehours
            new TranslationSeedItem("entity.servicecontract.slaresponsehours", "zh-HK", "SLA响应时限（小时）_hk", "SLA 响应时限（小时）"),

            // entity.servicecontract.slaresolvehours
            new TranslationSeedItem("entity.servicecontract.slaresolvehours", "en-US", "SLA解决时限（小时）_us", "SLA 解决时限（小时）"),
            // entity.servicecontract.slaresolvehours
            new TranslationSeedItem("entity.servicecontract.slaresolvehours", "ja-JP", "SLA解决时限（小时）_jp", "SLA 解决时限（小时）"),
            // entity.servicecontract.slaresolvehours
            new TranslationSeedItem("entity.servicecontract.slaresolvehours", "zh-CN", "SLA解决时限（小时）", "SLA 解决时限（小时）"),
            // entity.servicecontract.slaresolvehours
            new TranslationSeedItem("entity.servicecontract.slaresolvehours", "zh-HK", "SLA解决时限（小时）_hk", "SLA 解决时限（小时）"),

            // entity.servicecontract.accountmanager
            new TranslationSeedItem("entity.servicecontract.accountmanager", "en-US", "客户经理_us", "客户经理（人员代码）"),
            // entity.servicecontract.accountmanager
            new TranslationSeedItem("entity.servicecontract.accountmanager", "ja-JP", "客户经理_jp", "客户经理（人员代码）"),
            // entity.servicecontract.accountmanager
            new TranslationSeedItem("entity.servicecontract.accountmanager", "zh-CN", "客户经理", "客户经理（人员代码）"),
            // entity.servicecontract.accountmanager
            new TranslationSeedItem("entity.servicecontract.accountmanager", "zh-HK", "客户经理_hk", "客户经理（人员代码）"),

            // entity.servicecontract.sortorder
            new TranslationSeedItem("entity.servicecontract.sortorder", "en-US", "排序号_us", "排序号（越小越靠前）"),
            // entity.servicecontract.sortorder
            new TranslationSeedItem("entity.servicecontract.sortorder", "ja-JP", "排序号_jp", "排序号（越小越靠前）"),
            // entity.servicecontract.sortorder
            new TranslationSeedItem("entity.servicecontract.sortorder", "zh-CN", "排序号", "排序号（越小越靠前）"),
            // entity.servicecontract.sortorder
            new TranslationSeedItem("entity.servicecontract.sortorder", "zh-HK", "排序号_hk", "排序号（越小越靠前）"),

            // entity.servicecontract.serviceorders
            new TranslationSeedItem("entity.servicecontract.serviceorders", "en-US", "服务订单列表_us", "服务订单列表（外键在子表 TaktServiceOrder.ServiceContractId）"),
            // entity.servicecontract.serviceorders
            new TranslationSeedItem("entity.servicecontract.serviceorders", "ja-JP", "服务订单列表_jp", "服务订单列表（外键在子表 TaktServiceOrder.ServiceContractId）"),
            // entity.servicecontract.serviceorders
            new TranslationSeedItem("entity.servicecontract.serviceorders", "zh-CN", "服务订单列表", "服务订单列表（外键在子表 TaktServiceOrder.ServiceContractId）"),
            // entity.servicecontract.serviceorders
            new TranslationSeedItem("entity.servicecontract.serviceorders", "zh-HK", "服务订单列表_hk", "服务订单列表（外键在子表 TaktServiceOrder.ServiceContractId）"),

            // entity.servicecontract.servicerequests
            new TranslationSeedItem("entity.servicecontract.servicerequests", "en-US", "服务请求列表_us", "服务请求列表（外键在子表 TaktServiceRequest.ServiceContractId）"),
            // entity.servicecontract.servicerequests
            new TranslationSeedItem("entity.servicecontract.servicerequests", "ja-JP", "服务请求列表_jp", "服务请求列表（外键在子表 TaktServiceRequest.ServiceContractId）"),
            // entity.servicecontract.servicerequests
            new TranslationSeedItem("entity.servicecontract.servicerequests", "zh-CN", "服务请求列表", "服务请求列表（外键在子表 TaktServiceRequest.ServiceContractId）"),
            // entity.servicecontract.servicerequests
            new TranslationSeedItem("entity.servicecontract.servicerequests", "zh-HK", "服务请求列表_hk", "服务请求列表（外键在子表 TaktServiceRequest.ServiceContractId）"),
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
        translation.ResourceGroup = "CustomerService";
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
