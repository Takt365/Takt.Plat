// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Materials
// 文件名称：TaktPurchaseRequestI18nSeedData.cs
// 创建时间：2026-06-07
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
using Takt.Shared.Enums;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Materials;

/// <summary>
/// TaktPurchaseRequest 实体国际化翻译种子（键前缀 entity.purchaseRequest.*）
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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 purchaseRequest 实体翻译...", tenantCode);

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
    /// I18nKey：entity.purchaseRequest._self / entity.purchaseRequest.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetPurchaseRequestTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.purchaseRequest._self
            new TranslationSeedItem("entity.purchaseRequest._self", "en-US", "Purchase Request Information", "实体名称"),
            // entity.purchaseRequest._self
            new TranslationSeedItem("entity.purchaseRequest._self", "ja-JP", "Takt采购申请信息", "实体名称"),
            // entity.purchaseRequest._self
            new TranslationSeedItem("entity.purchaseRequest._self", "zh-CN", "Takt采购申请信息", "实体名称"),
            // entity.purchaseRequest._self
            new TranslationSeedItem("entity.purchaseRequest._self", "zh-HK", "Takt采购申请信息", "实体名称"),

            // entity.purchaseRequest.plantcode
            new TranslationSeedItem("entity.purchaseRequest.plantcode", "en-US", "工厂代码", "工厂代码（不可空）"),
            // entity.purchaseRequest.plantcode
            new TranslationSeedItem("entity.purchaseRequest.plantcode", "ja-JP", "工厂代码", "工厂代码（不可空）"),
            // entity.purchaseRequest.plantcode
            new TranslationSeedItem("entity.purchaseRequest.plantcode", "zh-CN", "工厂代码", "工厂代码（不可空）"),
            // entity.purchaseRequest.plantcode
            new TranslationSeedItem("entity.purchaseRequest.plantcode", "zh-HK", "工厂代码", "工厂代码（不可空）"),

            // entity.purchaseRequest.code
            new TranslationSeedItem("entity.purchaseRequest.code", "en-US", "采购申请编码", "采购申请编码（唯一索引）"),
            // entity.purchaseRequest.code
            new TranslationSeedItem("entity.purchaseRequest.code", "ja-JP", "采购申请编码", "采购申请编码（唯一索引）"),
            // entity.purchaseRequest.code
            new TranslationSeedItem("entity.purchaseRequest.code", "zh-CN", "采购申请编码", "采购申请编码（唯一索引）"),
            // entity.purchaseRequest.code
            new TranslationSeedItem("entity.purchaseRequest.code", "zh-HK", "采购申请编码", "采购申请编码（唯一索引）"),

            // entity.purchaseRequest.requestdate
            new TranslationSeedItem("entity.purchaseRequest.requestdate", "en-US", "申请日期", "申请日期"),
            // entity.purchaseRequest.requestdate
            new TranslationSeedItem("entity.purchaseRequest.requestdate", "ja-JP", "申请日期", "申请日期"),
            // entity.purchaseRequest.requestdate
            new TranslationSeedItem("entity.purchaseRequest.requestdate", "zh-CN", "申请日期", "申请日期"),
            // entity.purchaseRequest.requestdate
            new TranslationSeedItem("entity.purchaseRequest.requestdate", "zh-HK", "申请日期", "申请日期"),

            // entity.purchaseRequest.requiredarrivaldate
            new TranslationSeedItem("entity.purchaseRequest.requiredarrivaldate", "en-US", "要求到货日期", "要求到货日期"),
            // entity.purchaseRequest.requiredarrivaldate
            new TranslationSeedItem("entity.purchaseRequest.requiredarrivaldate", "ja-JP", "要求到货日期", "要求到货日期"),
            // entity.purchaseRequest.requiredarrivaldate
            new TranslationSeedItem("entity.purchaseRequest.requiredarrivaldate", "zh-CN", "要求到货日期", "要求到货日期"),
            // entity.purchaseRequest.requiredarrivaldate
            new TranslationSeedItem("entity.purchaseRequest.requiredarrivaldate", "zh-HK", "要求到货日期", "要求到货日期"),

            // entity.purchaseRequest.requestid
            new TranslationSeedItem("entity.purchaseRequest.requestid", "en-US", "申请人员工ID", "申请人员工ID（关联 TaktEmployee，序列化为 string 以避免 Javascript 精度问题）"),
            // entity.purchaseRequest.requestid
            new TranslationSeedItem("entity.purchaseRequest.requestid", "ja-JP", "申请人员工ID", "申请人员工ID（关联 TaktEmployee，序列化为 string 以避免 Javascript 精度问题）"),
            // entity.purchaseRequest.requestid
            new TranslationSeedItem("entity.purchaseRequest.requestid", "zh-CN", "申请人员工ID", "申请人员工ID（关联 TaktEmployee，序列化为 string 以避免 Javascript 精度问题）"),
            // entity.purchaseRequest.requestid
            new TranslationSeedItem("entity.purchaseRequest.requestid", "zh-HK", "申请人员工ID", "申请人员工ID（关联 TaktEmployee，序列化为 string 以避免 Javascript 精度问题）"),

            // entity.purchaseRequest.requestby
            new TranslationSeedItem("entity.purchaseRequest.requestby", "en-US", "申请人", "申请人（人员代码）"),
            // entity.purchaseRequest.requestby
            new TranslationSeedItem("entity.purchaseRequest.requestby", "ja-JP", "申请人", "申请人（人员代码）"),
            // entity.purchaseRequest.requestby
            new TranslationSeedItem("entity.purchaseRequest.requestby", "zh-CN", "申请人", "申请人（人员代码）"),
            // entity.purchaseRequest.requestby
            new TranslationSeedItem("entity.purchaseRequest.requestby", "zh-HK", "申请人", "申请人（人员代码）"),

            // entity.purchaseRequest.totalquantity
            new TranslationSeedItem("entity.purchaseRequest.totalquantity", "en-US", "申请总数量", "申请总数量（基本单位数量）"),
            // entity.purchaseRequest.totalquantity
            new TranslationSeedItem("entity.purchaseRequest.totalquantity", "ja-JP", "申请总数量", "申请总数量（基本单位数量）"),
            // entity.purchaseRequest.totalquantity
            new TranslationSeedItem("entity.purchaseRequest.totalquantity", "zh-CN", "申请总数量", "申请总数量（基本单位数量）"),
            // entity.purchaseRequest.totalquantity
            new TranslationSeedItem("entity.purchaseRequest.totalquantity", "zh-HK", "申请总数量", "申请总数量（基本单位数量）"),

            // entity.purchaseRequest.totalamount
            new TranslationSeedItem("entity.purchaseRequest.totalamount", "en-US", "申请总金额", "申请总金额（精确到分，存储为整数，单位为分）"),
            // entity.purchaseRequest.totalamount
            new TranslationSeedItem("entity.purchaseRequest.totalamount", "ja-JP", "申请总金额", "申请总金额（精确到分，存储为整数，单位为分）"),
            // entity.purchaseRequest.totalamount
            new TranslationSeedItem("entity.purchaseRequest.totalamount", "zh-CN", "申请总金额", "申请总金额（精确到分，存储为整数，单位为分）"),
            // entity.purchaseRequest.totalamount
            new TranslationSeedItem("entity.purchaseRequest.totalamount", "zh-HK", "申请总金额", "申请总金额（精确到分，存储为整数，单位为分）"),

            // entity.purchaseRequest.convertedquantity
            new TranslationSeedItem("entity.purchaseRequest.convertedquantity", "en-US", "已转订单数量", "已转订单数量（基本单位数量）"),
            // entity.purchaseRequest.convertedquantity
            new TranslationSeedItem("entity.purchaseRequest.convertedquantity", "ja-JP", "已转订单数量", "已转订单数量（基本单位数量）"),
            // entity.purchaseRequest.convertedquantity
            new TranslationSeedItem("entity.purchaseRequest.convertedquantity", "zh-CN", "已转订单数量", "已转订单数量（基本单位数量）"),
            // entity.purchaseRequest.convertedquantity
            new TranslationSeedItem("entity.purchaseRequest.convertedquantity", "zh-HK", "已转订单数量", "已转订单数量（基本单位数量）"),

            // entity.purchaseRequest.convertedamount
            new TranslationSeedItem("entity.purchaseRequest.convertedamount", "en-US", "已转订单金额", "已转订单金额（精确到分，存储为整数，单位为分）"),
            // entity.purchaseRequest.convertedamount
            new TranslationSeedItem("entity.purchaseRequest.convertedamount", "ja-JP", "已转订单金额", "已转订单金额（精确到分，存储为整数，单位为分）"),
            // entity.purchaseRequest.convertedamount
            new TranslationSeedItem("entity.purchaseRequest.convertedamount", "zh-CN", "已转订单金额", "已转订单金额（精确到分，存储为整数，单位为分）"),
            // entity.purchaseRequest.convertedamount
            new TranslationSeedItem("entity.purchaseRequest.convertedamount", "zh-HK", "已转订单金额", "已转订单金额（精确到分，存储为整数，单位为分）"),

            // entity.purchaseRequest.requeststatus
            new TranslationSeedItem("entity.purchaseRequest.requeststatus", "en-US", "申请状态", "申请状态（1=启用，0=禁用）"),
            // entity.purchaseRequest.requeststatus
            new TranslationSeedItem("entity.purchaseRequest.requeststatus", "ja-JP", "申请状态", "申请状态（1=启用，0=禁用）"),
            // entity.purchaseRequest.requeststatus
            new TranslationSeedItem("entity.purchaseRequest.requeststatus", "zh-CN", "申请状态", "申请状态（1=启用，0=禁用）"),
            // entity.purchaseRequest.requeststatus
            new TranslationSeedItem("entity.purchaseRequest.requeststatus", "zh-HK", "申请状态", "申请状态（1=启用，0=禁用）"),

            // entity.purchaseRequest.convertedstatus
            new TranslationSeedItem("entity.purchaseRequest.convertedstatus", "en-US", "转订单状态", "转订单状态（0=未转订单，1=部分转订单，2=全部转订单）"),
            // entity.purchaseRequest.convertedstatus
            new TranslationSeedItem("entity.purchaseRequest.convertedstatus", "ja-JP", "转订单状态", "转订单状态（0=未转订单，1=部分转订单，2=全部转订单）"),
            // entity.purchaseRequest.convertedstatus
            new TranslationSeedItem("entity.purchaseRequest.convertedstatus", "zh-CN", "转订单状态", "转订单状态（0=未转订单，1=部分转订单，2=全部转订单）"),
            // entity.purchaseRequest.convertedstatus
            new TranslationSeedItem("entity.purchaseRequest.convertedstatus", "zh-HK", "转订单状态", "转订单状态（0=未转订单，1=部分转订单，2=全部转订单）"),

            // entity.purchaseRequest.flowinstanceid
            new TranslationSeedItem("entity.purchaseRequest.flowinstanceid", "en-US", "流程实例ID", "流程实例ID（关联 TaktFlowInstance，发起审批后由业务写入，用于审批流程）"),
            // entity.purchaseRequest.flowinstanceid
            new TranslationSeedItem("entity.purchaseRequest.flowinstanceid", "ja-JP", "流程实例ID", "流程实例ID（关联 TaktFlowInstance，发起审批后由业务写入，用于审批流程）"),
            // entity.purchaseRequest.flowinstanceid
            new TranslationSeedItem("entity.purchaseRequest.flowinstanceid", "zh-CN", "流程实例ID", "流程实例ID（关联 TaktFlowInstance，发起审批后由业务写入，用于审批流程）"),
            // entity.purchaseRequest.flowinstanceid
            new TranslationSeedItem("entity.purchaseRequest.flowinstanceid", "zh-HK", "流程实例ID", "流程实例ID（关联 TaktFlowInstance，发起审批后由业务写入，用于审批流程）"),

            // entity.purchaseRequest.requestreason
            new TranslationSeedItem("entity.purchaseRequest.requestreason", "en-US", "申请原因", "申请原因"),
            // entity.purchaseRequest.requestreason
            new TranslationSeedItem("entity.purchaseRequest.requestreason", "ja-JP", "申请原因", "申请原因"),
            // entity.purchaseRequest.requestreason
            new TranslationSeedItem("entity.purchaseRequest.requestreason", "zh-CN", "申请原因", "申请原因"),
            // entity.purchaseRequest.requestreason
            new TranslationSeedItem("entity.purchaseRequest.requestreason", "zh-HK", "申请原因", "申请原因"),

            // entity.purchaseRequest.items
            new TranslationSeedItem("entity.purchaseRequest.items", "en-US", "items", "采购申请明细列表（主子表关系，一个申请可以有多个明细）"),
            // entity.purchaseRequest.items
            new TranslationSeedItem("entity.purchaseRequest.items", "ja-JP", "items", "采购申请明细列表（主子表关系，一个申请可以有多个明细）"),
            // entity.purchaseRequest.items
            new TranslationSeedItem("entity.purchaseRequest.items", "zh-CN", "items", "采购申请明细列表（主子表关系，一个申请可以有多个明细）"),
            // entity.purchaseRequest.items
            new TranslationSeedItem("entity.purchaseRequest.items", "zh-HK", "items", "采购申请明细列表（主子表关系，一个申请可以有多个明细）"),
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
