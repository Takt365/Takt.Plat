// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Procurement
// 文件名称：TaktPurchaseRequestI18nSeedData.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktPurchaseRequest 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktPurchaseRequest 实体国际化翻译种子（键前缀 entity.purchaserequest.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktPurchaseRequestI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktPurchaseRequest 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 purchaserequest 实体翻译...", tenantCode);

        foreach (var item in GetPurchaseRequestTranslations())
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

        TaktLogger.Information("TaktPurchaseRequest 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktPurchaseRequest 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.purchaserequest._self / entity.purchaserequest.{{field}}；ResourceGroup=Procurement；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetPurchaseRequestTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.purchaserequest._self
            new TranslationSeedItem("entity.purchaserequest._self", "en-US", "Purchase Request Information_us", "实体名称"),
            // entity.purchaserequest._self
            new TranslationSeedItem("entity.purchaserequest._self", "ja-JP", "Takt采购申请信息_jp", "实体名称"),
            // entity.purchaserequest._self
            new TranslationSeedItem("entity.purchaserequest._self", "zh-CN", "Takt采购申请信息", "实体名称"),
            // entity.purchaserequest._self
            new TranslationSeedItem("entity.purchaserequest._self", "zh-HK", "Takt采购申请信息_hk", "实体名称"),

            // entity.purchaserequest.plantcode
            new TranslationSeedItem("entity.purchaserequest.plantcode", "en-US", "工厂代码_us", "工厂代码（不可空）"),
            // entity.purchaserequest.plantcode
            new TranslationSeedItem("entity.purchaserequest.plantcode", "ja-JP", "工厂代码_jp", "工厂代码（不可空）"),
            // entity.purchaserequest.plantcode
            new TranslationSeedItem("entity.purchaserequest.plantcode", "zh-CN", "工厂代码", "工厂代码（不可空）"),
            // entity.purchaserequest.plantcode
            new TranslationSeedItem("entity.purchaserequest.plantcode", "zh-HK", "工厂代码_hk", "工厂代码（不可空）"),

            // entity.purchaserequest.code
            new TranslationSeedItem("entity.purchaserequest.code", "en-US", "采购申请编码_us", "采购申请编码（唯一索引）"),
            // entity.purchaserequest.code
            new TranslationSeedItem("entity.purchaserequest.code", "ja-JP", "采购申请编码_jp", "采购申请编码（唯一索引）"),
            // entity.purchaserequest.code
            new TranslationSeedItem("entity.purchaserequest.code", "zh-CN", "采购申请编码", "采购申请编码（唯一索引）"),
            // entity.purchaserequest.code
            new TranslationSeedItem("entity.purchaserequest.code", "zh-HK", "采购申请编码_hk", "采购申请编码（唯一索引）"),

            // entity.purchaserequest.requestdate
            new TranslationSeedItem("entity.purchaserequest.requestdate", "en-US", "申请日期_us", "申请日期"),
            // entity.purchaserequest.requestdate
            new TranslationSeedItem("entity.purchaserequest.requestdate", "ja-JP", "申请日期_jp", "申请日期"),
            // entity.purchaserequest.requestdate
            new TranslationSeedItem("entity.purchaserequest.requestdate", "zh-CN", "申请日期", "申请日期"),
            // entity.purchaserequest.requestdate
            new TranslationSeedItem("entity.purchaserequest.requestdate", "zh-HK", "申请日期_hk", "申请日期"),

            // entity.purchaserequest.requiredarrivaldate
            new TranslationSeedItem("entity.purchaserequest.requiredarrivaldate", "en-US", "要求到货日期_us", "要求到货日期"),
            // entity.purchaserequest.requiredarrivaldate
            new TranslationSeedItem("entity.purchaserequest.requiredarrivaldate", "ja-JP", "要求到货日期_jp", "要求到货日期"),
            // entity.purchaserequest.requiredarrivaldate
            new TranslationSeedItem("entity.purchaserequest.requiredarrivaldate", "zh-CN", "要求到货日期", "要求到货日期"),
            // entity.purchaserequest.requiredarrivaldate
            new TranslationSeedItem("entity.purchaserequest.requiredarrivaldate", "zh-HK", "要求到货日期_hk", "要求到货日期"),

            // entity.purchaserequest.requestid
            new TranslationSeedItem("entity.purchaserequest.requestid", "en-US", "申请人员工ID_us", "申请人员工ID（关联 TaktEmployee，序列化为 string 以避免 Javascript 精度问题）"),
            // entity.purchaserequest.requestid
            new TranslationSeedItem("entity.purchaserequest.requestid", "ja-JP", "申请人员工ID_jp", "申请人员工ID（关联 TaktEmployee，序列化为 string 以避免 Javascript 精度问题）"),
            // entity.purchaserequest.requestid
            new TranslationSeedItem("entity.purchaserequest.requestid", "zh-CN", "申请人员工ID", "申请人员工ID（关联 TaktEmployee，序列化为 string 以避免 Javascript 精度问题）"),
            // entity.purchaserequest.requestid
            new TranslationSeedItem("entity.purchaserequest.requestid", "zh-HK", "申请人员工ID_hk", "申请人员工ID（关联 TaktEmployee，序列化为 string 以避免 Javascript 精度问题）"),

            // entity.purchaserequest.requestby
            new TranslationSeedItem("entity.purchaserequest.requestby", "en-US", "申请人_us", "申请人（人员代码）"),
            // entity.purchaserequest.requestby
            new TranslationSeedItem("entity.purchaserequest.requestby", "ja-JP", "申请人_jp", "申请人（人员代码）"),
            // entity.purchaserequest.requestby
            new TranslationSeedItem("entity.purchaserequest.requestby", "zh-CN", "申请人", "申请人（人员代码）"),
            // entity.purchaserequest.requestby
            new TranslationSeedItem("entity.purchaserequest.requestby", "zh-HK", "申请人_hk", "申请人（人员代码）"),

            // entity.purchaserequest.totalquantity
            new TranslationSeedItem("entity.purchaserequest.totalquantity", "en-US", "申请总数量_us", "申请总数量（基本单位数量）"),
            // entity.purchaserequest.totalquantity
            new TranslationSeedItem("entity.purchaserequest.totalquantity", "ja-JP", "申请总数量_jp", "申请总数量（基本单位数量）"),
            // entity.purchaserequest.totalquantity
            new TranslationSeedItem("entity.purchaserequest.totalquantity", "zh-CN", "申请总数量", "申请总数量（基本单位数量）"),
            // entity.purchaserequest.totalquantity
            new TranslationSeedItem("entity.purchaserequest.totalquantity", "zh-HK", "申请总数量_hk", "申请总数量（基本单位数量）"),

            // entity.purchaserequest.totalamount
            new TranslationSeedItem("entity.purchaserequest.totalamount", "en-US", "申请总金额_us", "申请总金额（精确到分，存储为整数，单位为分）"),
            // entity.purchaserequest.totalamount
            new TranslationSeedItem("entity.purchaserequest.totalamount", "ja-JP", "申请总金额_jp", "申请总金额（精确到分，存储为整数，单位为分）"),
            // entity.purchaserequest.totalamount
            new TranslationSeedItem("entity.purchaserequest.totalamount", "zh-CN", "申请总金额", "申请总金额（精确到分，存储为整数，单位为分）"),
            // entity.purchaserequest.totalamount
            new TranslationSeedItem("entity.purchaserequest.totalamount", "zh-HK", "申请总金额_hk", "申请总金额（精确到分，存储为整数，单位为分）"),

            // entity.purchaserequest.convertedquantity
            new TranslationSeedItem("entity.purchaserequest.convertedquantity", "en-US", "已转订单数量_us", "已转订单数量（基本单位数量）"),
            // entity.purchaserequest.convertedquantity
            new TranslationSeedItem("entity.purchaserequest.convertedquantity", "ja-JP", "已转订单数量_jp", "已转订单数量（基本单位数量）"),
            // entity.purchaserequest.convertedquantity
            new TranslationSeedItem("entity.purchaserequest.convertedquantity", "zh-CN", "已转订单数量", "已转订单数量（基本单位数量）"),
            // entity.purchaserequest.convertedquantity
            new TranslationSeedItem("entity.purchaserequest.convertedquantity", "zh-HK", "已转订单数量_hk", "已转订单数量（基本单位数量）"),

            // entity.purchaserequest.convertedamount
            new TranslationSeedItem("entity.purchaserequest.convertedamount", "en-US", "已转订单金额_us", "已转订单金额（精确到分，存储为整数，单位为分）"),
            // entity.purchaserequest.convertedamount
            new TranslationSeedItem("entity.purchaserequest.convertedamount", "ja-JP", "已转订单金额_jp", "已转订单金额（精确到分，存储为整数，单位为分）"),
            // entity.purchaserequest.convertedamount
            new TranslationSeedItem("entity.purchaserequest.convertedamount", "zh-CN", "已转订单金额", "已转订单金额（精确到分，存储为整数，单位为分）"),
            // entity.purchaserequest.convertedamount
            new TranslationSeedItem("entity.purchaserequest.convertedamount", "zh-HK", "已转订单金额_hk", "已转订单金额（精确到分，存储为整数，单位为分）"),

            // entity.purchaserequest.requeststatus
            new TranslationSeedItem("entity.purchaserequest.requeststatus", "en-US", "申请状态_us", "申请状态（1=启用，0=禁用）"),
            // entity.purchaserequest.requeststatus
            new TranslationSeedItem("entity.purchaserequest.requeststatus", "ja-JP", "申请状态_jp", "申请状态（1=启用，0=禁用）"),
            // entity.purchaserequest.requeststatus
            new TranslationSeedItem("entity.purchaserequest.requeststatus", "zh-CN", "申请状态", "申请状态（1=启用，0=禁用）"),
            // entity.purchaserequest.requeststatus
            new TranslationSeedItem("entity.purchaserequest.requeststatus", "zh-HK", "申请状态_hk", "申请状态（1=启用，0=禁用）"),

            // entity.purchaserequest.convertedstatus
            new TranslationSeedItem("entity.purchaserequest.convertedstatus", "en-US", "转订单状态_us", "转订单状态（0=未转订单，1=部分转订单，2=全部转订单）"),
            // entity.purchaserequest.convertedstatus
            new TranslationSeedItem("entity.purchaserequest.convertedstatus", "ja-JP", "转订单状态_jp", "转订单状态（0=未转订单，1=部分转订单，2=全部转订单）"),
            // entity.purchaserequest.convertedstatus
            new TranslationSeedItem("entity.purchaserequest.convertedstatus", "zh-CN", "转订单状态", "转订单状态（0=未转订单，1=部分转订单，2=全部转订单）"),
            // entity.purchaserequest.convertedstatus
            new TranslationSeedItem("entity.purchaserequest.convertedstatus", "zh-HK", "转订单状态_hk", "转订单状态（0=未转订单，1=部分转订单，2=全部转订单）"),

            // entity.purchaserequest.requestreason
            new TranslationSeedItem("entity.purchaserequest.requestreason", "en-US", "申请原因_us", "申请原因"),
            // entity.purchaserequest.requestreason
            new TranslationSeedItem("entity.purchaserequest.requestreason", "ja-JP", "申请原因_jp", "申请原因"),
            // entity.purchaserequest.requestreason
            new TranslationSeedItem("entity.purchaserequest.requestreason", "zh-CN", "申请原因", "申请原因"),
            // entity.purchaserequest.requestreason
            new TranslationSeedItem("entity.purchaserequest.requestreason", "zh-HK", "申请原因_hk", "申请原因"),

            // entity.purchaserequest.items
            new TranslationSeedItem("entity.purchaserequest.items", "en-US", "采购申请明细列表_us", "采购申请明细列表（主子表关系，一个申请可以有多个明细）"),
            // entity.purchaserequest.items
            new TranslationSeedItem("entity.purchaserequest.items", "ja-JP", "采购申请明细列表_jp", "采购申请明细列表（主子表关系，一个申请可以有多个明细）"),
            // entity.purchaserequest.items
            new TranslationSeedItem("entity.purchaserequest.items", "zh-CN", "采购申请明细列表", "采购申请明细列表（主子表关系，一个申请可以有多个明细）"),
            // entity.purchaserequest.items
            new TranslationSeedItem("entity.purchaserequest.items", "zh-HK", "采购申请明细列表_hk", "采购申请明细列表（主子表关系，一个申请可以有多个明细）"),

            // entity.purchaserequest.changelogs
            new TranslationSeedItem("entity.purchaserequest.changelogs", "en-US", "采购申请变更记录列表_us", "采购申请变更记录列表（外键在子表 TaktPurchaseRequestChangeLog.RequestId）"),
            // entity.purchaserequest.changelogs
            new TranslationSeedItem("entity.purchaserequest.changelogs", "ja-JP", "采购申请变更记录列表_jp", "采购申请变更记录列表（外键在子表 TaktPurchaseRequestChangeLog.RequestId）"),
            // entity.purchaserequest.changelogs
            new TranslationSeedItem("entity.purchaserequest.changelogs", "zh-CN", "采购申请变更记录列表", "采购申请变更记录列表（外键在子表 TaktPurchaseRequestChangeLog.RequestId）"),
            // entity.purchaserequest.changelogs
            new TranslationSeedItem("entity.purchaserequest.changelogs", "zh-HK", "采购申请变更记录列表_hk", "采购申请变更记录列表（外键在子表 TaktPurchaseRequestChangeLog.RequestId）"),
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
