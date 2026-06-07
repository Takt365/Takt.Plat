// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.CustomerService
// 文件名称：TaktServiceOrderI18nSeedData.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktServiceOrder 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktServiceOrder 实体国际化翻译种子（键前缀 entity.serviceOrder.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktServiceOrderI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktServiceOrder 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 serviceOrder 实体翻译...", tenantCode);

        foreach (var item in GetServiceOrderTranslations())
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

        TaktLogger.Information("TaktServiceOrder 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktServiceOrder 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.serviceOrder._self / entity.serviceOrder.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetServiceOrderTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.serviceOrder._self
            new TranslationSeedItem("entity.serviceOrder._self", "en-US", "Service Order Information", "实体名称"),
            // entity.serviceOrder._self
            new TranslationSeedItem("entity.serviceOrder._self", "ja-JP", "服务订单信息", "实体名称"),
            // entity.serviceOrder._self
            new TranslationSeedItem("entity.serviceOrder._self", "zh-CN", "服务订单信息", "实体名称"),
            // entity.serviceOrder._self
            new TranslationSeedItem("entity.serviceOrder._self", "zh-HK", "服务订单信息", "实体名称"),

            // entity.serviceOrder.plantcode
            new TranslationSeedItem("entity.serviceOrder.plantcode", "en-US", "工厂代码", "工厂代码"),
            // entity.serviceOrder.plantcode
            new TranslationSeedItem("entity.serviceOrder.plantcode", "ja-JP", "工厂代码", "工厂代码"),
            // entity.serviceOrder.plantcode
            new TranslationSeedItem("entity.serviceOrder.plantcode", "zh-CN", "工厂代码", "工厂代码"),
            // entity.serviceOrder.plantcode
            new TranslationSeedItem("entity.serviceOrder.plantcode", "zh-HK", "工厂代码", "工厂代码"),

            // entity.serviceOrder.code
            new TranslationSeedItem("entity.serviceOrder.code", "en-US", "服务订单编码", "服务订单编码（组合唯一索引）"),
            // entity.serviceOrder.code
            new TranslationSeedItem("entity.serviceOrder.code", "ja-JP", "服务订单编码", "服务订单编码（组合唯一索引）"),
            // entity.serviceOrder.code
            new TranslationSeedItem("entity.serviceOrder.code", "zh-CN", "服务订单编码", "服务订单编码（组合唯一索引）"),
            // entity.serviceOrder.code
            new TranslationSeedItem("entity.serviceOrder.code", "zh-HK", "服务订单编码", "服务订单编码（组合唯一索引）"),

            // entity.serviceOrder.clientid
            new TranslationSeedItem("entity.serviceOrder.clientid", "en-US", "客户端ID", "客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）"),
            // entity.serviceOrder.clientid
            new TranslationSeedItem("entity.serviceOrder.clientid", "ja-JP", "客户端ID", "客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）"),
            // entity.serviceOrder.clientid
            new TranslationSeedItem("entity.serviceOrder.clientid", "zh-CN", "客户端ID", "客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）"),
            // entity.serviceOrder.clientid
            new TranslationSeedItem("entity.serviceOrder.clientid", "zh-HK", "客户端ID", "客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）"),

            // entity.serviceOrder.clientcode
            new TranslationSeedItem("entity.serviceOrder.clientcode", "en-US", "客户端编码", "客户端编码（冗余字段，便于查询）"),
            // entity.serviceOrder.clientcode
            new TranslationSeedItem("entity.serviceOrder.clientcode", "ja-JP", "客户端编码", "客户端编码（冗余字段，便于查询）"),
            // entity.serviceOrder.clientcode
            new TranslationSeedItem("entity.serviceOrder.clientcode", "zh-CN", "客户端编码", "客户端编码（冗余字段，便于查询）"),
            // entity.serviceOrder.clientcode
            new TranslationSeedItem("entity.serviceOrder.clientcode", "zh-HK", "客户端编码", "客户端编码（冗余字段，便于查询）"),

            // entity.serviceOrder.clientname
            new TranslationSeedItem("entity.serviceOrder.clientname", "en-US", "客户端名称", "客户端名称（冗余字段，便于查询）"),
            // entity.serviceOrder.clientname
            new TranslationSeedItem("entity.serviceOrder.clientname", "ja-JP", "客户端名称", "客户端名称（冗余字段，便于查询）"),
            // entity.serviceOrder.clientname
            new TranslationSeedItem("entity.serviceOrder.clientname", "zh-CN", "客户端名称", "客户端名称（冗余字段，便于查询）"),
            // entity.serviceOrder.clientname
            new TranslationSeedItem("entity.serviceOrder.clientname", "zh-HK", "客户端名称", "客户端名称（冗余字段，便于查询）"),

            // entity.serviceOrder.servicecontractid
            new TranslationSeedItem("entity.serviceOrder.servicecontractid", "en-US", "关联服务合同ID", "关联服务合同ID（序列化为string以避免Javascript精度问题）"),
            // entity.serviceOrder.servicecontractid
            new TranslationSeedItem("entity.serviceOrder.servicecontractid", "ja-JP", "关联服务合同ID", "关联服务合同ID（序列化为string以避免Javascript精度问题）"),
            // entity.serviceOrder.servicecontractid
            new TranslationSeedItem("entity.serviceOrder.servicecontractid", "zh-CN", "关联服务合同ID", "关联服务合同ID（序列化为string以避免Javascript精度问题）"),
            // entity.serviceOrder.servicecontractid
            new TranslationSeedItem("entity.serviceOrder.servicecontractid", "zh-HK", "关联服务合同ID", "关联服务合同ID（序列化为string以避免Javascript精度问题）"),

            // entity.serviceOrder.servicecontractcode
            new TranslationSeedItem("entity.serviceOrder.servicecontractcode", "en-US", "关联服务合同编码", "关联服务合同编码（冗余字段，便于查询）"),
            // entity.serviceOrder.servicecontractcode
            new TranslationSeedItem("entity.serviceOrder.servicecontractcode", "ja-JP", "关联服务合同编码", "关联服务合同编码（冗余字段，便于查询）"),
            // entity.serviceOrder.servicecontractcode
            new TranslationSeedItem("entity.serviceOrder.servicecontractcode", "zh-CN", "关联服务合同编码", "关联服务合同编码（冗余字段，便于查询）"),
            // entity.serviceOrder.servicecontractcode
            new TranslationSeedItem("entity.serviceOrder.servicecontractcode", "zh-HK", "关联服务合同编码", "关联服务合同编码（冗余字段，便于查询）"),

            // entity.serviceOrder.servicerequestid
            new TranslationSeedItem("entity.serviceOrder.servicerequestid", "en-US", "关联服务请求ID", "关联服务请求ID（序列化为string以避免Javascript精度问题）"),
            // entity.serviceOrder.servicerequestid
            new TranslationSeedItem("entity.serviceOrder.servicerequestid", "ja-JP", "关联服务请求ID", "关联服务请求ID（序列化为string以避免Javascript精度问题）"),
            // entity.serviceOrder.servicerequestid
            new TranslationSeedItem("entity.serviceOrder.servicerequestid", "zh-CN", "关联服务请求ID", "关联服务请求ID（序列化为string以避免Javascript精度问题）"),
            // entity.serviceOrder.servicerequestid
            new TranslationSeedItem("entity.serviceOrder.servicerequestid", "zh-HK", "关联服务请求ID", "关联服务请求ID（序列化为string以避免Javascript精度问题）"),

            // entity.serviceOrder.servicerequestcode
            new TranslationSeedItem("entity.serviceOrder.servicerequestcode", "en-US", "关联服务请求单号", "关联服务请求单号（冗余字段，便于查询）"),
            // entity.serviceOrder.servicerequestcode
            new TranslationSeedItem("entity.serviceOrder.servicerequestcode", "ja-JP", "关联服务请求单号", "关联服务请求单号（冗余字段，便于查询）"),
            // entity.serviceOrder.servicerequestcode
            new TranslationSeedItem("entity.serviceOrder.servicerequestcode", "zh-CN", "关联服务请求单号", "关联服务请求单号（冗余字段，便于查询）"),
            // entity.serviceOrder.servicerequestcode
            new TranslationSeedItem("entity.serviceOrder.servicerequestcode", "zh-HK", "关联服务请求单号", "关联服务请求单号（冗余字段，便于查询）"),

            // entity.serviceOrder.orderdate
            new TranslationSeedItem("entity.serviceOrder.orderdate", "en-US", "订单日期", "订单日期"),
            // entity.serviceOrder.orderdate
            new TranslationSeedItem("entity.serviceOrder.orderdate", "ja-JP", "订单日期", "订单日期"),
            // entity.serviceOrder.orderdate
            new TranslationSeedItem("entity.serviceOrder.orderdate", "zh-CN", "订单日期", "订单日期"),
            // entity.serviceOrder.orderdate
            new TranslationSeedItem("entity.serviceOrder.orderdate", "zh-HK", "订单日期", "订单日期"),

            // entity.serviceOrder.ordertype
            new TranslationSeedItem("entity.serviceOrder.ordertype", "en-US", "订单类型", "订单类型（0=现场服务，1=远程支持，2=备件更换，3=安装调试，4=其他）"),
            // entity.serviceOrder.ordertype
            new TranslationSeedItem("entity.serviceOrder.ordertype", "ja-JP", "订单类型", "订单类型（0=现场服务，1=远程支持，2=备件更换，3=安装调试，4=其他）"),
            // entity.serviceOrder.ordertype
            new TranslationSeedItem("entity.serviceOrder.ordertype", "zh-CN", "订单类型", "订单类型（0=现场服务，1=远程支持，2=备件更换，3=安装调试，4=其他）"),
            // entity.serviceOrder.ordertype
            new TranslationSeedItem("entity.serviceOrder.ordertype", "zh-HK", "订单类型", "订单类型（0=现场服务，1=远程支持，2=备件更换，3=安装调试，4=其他）"),

            // entity.serviceOrder.orderstatus
            new TranslationSeedItem("entity.serviceOrder.orderstatus", "en-US", "订单状态", "订单状态（0=草稿，1=已确认，2=执行中，3=已完成，4=已结算，5=已取消）"),
            // entity.serviceOrder.orderstatus
            new TranslationSeedItem("entity.serviceOrder.orderstatus", "ja-JP", "订单状态", "订单状态（0=草稿，1=已确认，2=执行中，3=已完成，4=已结算，5=已取消）"),
            // entity.serviceOrder.orderstatus
            new TranslationSeedItem("entity.serviceOrder.orderstatus", "zh-CN", "订单状态", "订单状态（0=草稿，1=已确认，2=执行中，3=已完成，4=已结算，5=已取消）"),
            // entity.serviceOrder.orderstatus
            new TranslationSeedItem("entity.serviceOrder.orderstatus", "zh-HK", "订单状态", "订单状态（0=草稿，1=已确认，2=执行中，3=已完成，4=已结算，5=已取消）"),

            // entity.serviceOrder.totalamount
            new TranslationSeedItem("entity.serviceOrder.totalamount", "en-US", "订单总金额", "订单总金额"),
            // entity.serviceOrder.totalamount
            new TranslationSeedItem("entity.serviceOrder.totalamount", "ja-JP", "订单总金额", "订单总金额"),
            // entity.serviceOrder.totalamount
            new TranslationSeedItem("entity.serviceOrder.totalamount", "zh-CN", "订单总金额", "订单总金额"),
            // entity.serviceOrder.totalamount
            new TranslationSeedItem("entity.serviceOrder.totalamount", "zh-HK", "订单总金额", "订单总金额"),

            // entity.serviceOrder.discountamount
            new TranslationSeedItem("entity.serviceOrder.discountamount", "en-US", "折扣金额", "折扣金额"),
            // entity.serviceOrder.discountamount
            new TranslationSeedItem("entity.serviceOrder.discountamount", "ja-JP", "折扣金额", "折扣金额"),
            // entity.serviceOrder.discountamount
            new TranslationSeedItem("entity.serviceOrder.discountamount", "zh-CN", "折扣金额", "折扣金额"),
            // entity.serviceOrder.discountamount
            new TranslationSeedItem("entity.serviceOrder.discountamount", "zh-HK", "折扣金额", "折扣金额"),

            // entity.serviceOrder.taxamount
            new TranslationSeedItem("entity.serviceOrder.taxamount", "en-US", "税费", "税费"),
            // entity.serviceOrder.taxamount
            new TranslationSeedItem("entity.serviceOrder.taxamount", "ja-JP", "税费", "税费"),
            // entity.serviceOrder.taxamount
            new TranslationSeedItem("entity.serviceOrder.taxamount", "zh-CN", "税费", "税费"),
            // entity.serviceOrder.taxamount
            new TranslationSeedItem("entity.serviceOrder.taxamount", "zh-HK", "税费", "税费"),

            // entity.serviceOrder.actualamount
            new TranslationSeedItem("entity.serviceOrder.actualamount", "en-US", "订单实付金额", "订单实付金额"),
            // entity.serviceOrder.actualamount
            new TranslationSeedItem("entity.serviceOrder.actualamount", "ja-JP", "订单实付金额", "订单实付金额"),
            // entity.serviceOrder.actualamount
            new TranslationSeedItem("entity.serviceOrder.actualamount", "zh-CN", "订单实付金额", "订单实付金额"),
            // entity.serviceOrder.actualamount
            new TranslationSeedItem("entity.serviceOrder.actualamount", "zh-HK", "订单实付金额", "订单实付金额"),

            // entity.serviceOrder.currencycode
            new TranslationSeedItem("entity.serviceOrder.currencycode", "en-US", "结算币种代码", "结算币种代码"),
            // entity.serviceOrder.currencycode
            new TranslationSeedItem("entity.serviceOrder.currencycode", "ja-JP", "结算币种代码", "结算币种代码"),
            // entity.serviceOrder.currencycode
            new TranslationSeedItem("entity.serviceOrder.currencycode", "zh-CN", "结算币种代码", "结算币种代码"),
            // entity.serviceOrder.currencycode
            new TranslationSeedItem("entity.serviceOrder.currencycode", "zh-HK", "结算币种代码", "结算币种代码"),

            // entity.serviceOrder.plannedstartdate
            new TranslationSeedItem("entity.serviceOrder.plannedstartdate", "en-US", "计划开始日期", "计划开始日期"),
            // entity.serviceOrder.plannedstartdate
            new TranslationSeedItem("entity.serviceOrder.plannedstartdate", "ja-JP", "计划开始日期", "计划开始日期"),
            // entity.serviceOrder.plannedstartdate
            new TranslationSeedItem("entity.serviceOrder.plannedstartdate", "zh-CN", "计划开始日期", "计划开始日期"),
            // entity.serviceOrder.plannedstartdate
            new TranslationSeedItem("entity.serviceOrder.plannedstartdate", "zh-HK", "计划开始日期", "计划开始日期"),

            // entity.serviceOrder.plannedenddate
            new TranslationSeedItem("entity.serviceOrder.plannedenddate", "en-US", "计划结束日期", "计划结束日期"),
            // entity.serviceOrder.plannedenddate
            new TranslationSeedItem("entity.serviceOrder.plannedenddate", "ja-JP", "计划结束日期", "计划结束日期"),
            // entity.serviceOrder.plannedenddate
            new TranslationSeedItem("entity.serviceOrder.plannedenddate", "zh-CN", "计划结束日期", "计划结束日期"),
            // entity.serviceOrder.plannedenddate
            new TranslationSeedItem("entity.serviceOrder.plannedenddate", "zh-HK", "计划结束日期", "计划结束日期"),

            // entity.serviceOrder.actualstartdate
            new TranslationSeedItem("entity.serviceOrder.actualstartdate", "en-US", "实际开始日期", "实际开始日期"),
            // entity.serviceOrder.actualstartdate
            new TranslationSeedItem("entity.serviceOrder.actualstartdate", "ja-JP", "实际开始日期", "实际开始日期"),
            // entity.serviceOrder.actualstartdate
            new TranslationSeedItem("entity.serviceOrder.actualstartdate", "zh-CN", "实际开始日期", "实际开始日期"),
            // entity.serviceOrder.actualstartdate
            new TranslationSeedItem("entity.serviceOrder.actualstartdate", "zh-HK", "实际开始日期", "实际开始日期"),

            // entity.serviceOrder.actualenddate
            new TranslationSeedItem("entity.serviceOrder.actualenddate", "en-US", "实际结束日期", "实际结束日期"),
            // entity.serviceOrder.actualenddate
            new TranslationSeedItem("entity.serviceOrder.actualenddate", "ja-JP", "实际结束日期", "实际结束日期"),
            // entity.serviceOrder.actualenddate
            new TranslationSeedItem("entity.serviceOrder.actualenddate", "zh-CN", "实际结束日期", "实际结束日期"),
            // entity.serviceOrder.actualenddate
            new TranslationSeedItem("entity.serviceOrder.actualenddate", "zh-HK", "实际结束日期", "实际结束日期"),

            // entity.serviceOrder.serviceby
            new TranslationSeedItem("entity.serviceOrder.serviceby", "en-US", "服务负责人", "服务负责人（人员代码）"),
            // entity.serviceOrder.serviceby
            new TranslationSeedItem("entity.serviceOrder.serviceby", "ja-JP", "服务负责人", "服务负责人（人员代码）"),
            // entity.serviceOrder.serviceby
            new TranslationSeedItem("entity.serviceOrder.serviceby", "zh-CN", "服务负责人", "服务负责人（人员代码）"),
            // entity.serviceOrder.serviceby
            new TranslationSeedItem("entity.serviceOrder.serviceby", "zh-HK", "服务负责人", "服务负责人（人员代码）"),

            // entity.serviceOrder.sortorder
            new TranslationSeedItem("entity.serviceOrder.sortorder", "en-US", "排序号", "排序号（越小越靠前）"),
            // entity.serviceOrder.sortorder
            new TranslationSeedItem("entity.serviceOrder.sortorder", "ja-JP", "排序号", "排序号（越小越靠前）"),
            // entity.serviceOrder.sortorder
            new TranslationSeedItem("entity.serviceOrder.sortorder", "zh-CN", "排序号", "排序号（越小越靠前）"),
            // entity.serviceOrder.sortorder
            new TranslationSeedItem("entity.serviceOrder.sortorder", "zh-HK", "排序号", "排序号（越小越靠前）"),

            // entity.serviceOrder.tickets
            new TranslationSeedItem("entity.serviceOrder.tickets", "en-US", "tickets", "服务工单列表（外键在子表 <see cref=\"TaktServiceTicket.ServiceOrderId\"/>）"),
            // entity.serviceOrder.tickets
            new TranslationSeedItem("entity.serviceOrder.tickets", "ja-JP", "tickets", "服务工单列表（外键在子表 <see cref=\"TaktServiceTicket.ServiceOrderId\"/>）"),
            // entity.serviceOrder.tickets
            new TranslationSeedItem("entity.serviceOrder.tickets", "zh-CN", "tickets", "服务工单列表（外键在子表 <see cref=\"TaktServiceTicket.ServiceOrderId\"/>）"),
            // entity.serviceOrder.tickets
            new TranslationSeedItem("entity.serviceOrder.tickets", "zh-HK", "tickets", "服务工单列表（外键在子表 <see cref=\"TaktServiceTicket.ServiceOrderId\"/>）"),
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
