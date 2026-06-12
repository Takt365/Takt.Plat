// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.CustomerService
// 文件名称：TaktServiceOrderI18nSeedData.cs
// 创建时间：2026-06-12
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
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.CustomerService;

/// <summary>
/// TaktServiceOrder 实体国际化翻译种子（键前缀 entity.serviceorder.*）
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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 serviceorder 实体翻译...", tenantCode);

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
    /// I18nKey：entity.serviceorder._self / entity.serviceorder.{{field}}；ResourceGroup=4；ResourceType=0
    /// </summary>
    private static List<TranslationSeedItem> GetServiceOrderTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.serviceorder._self
            new TranslationSeedItem("entity.serviceorder._self", "en-US", "Service Order Information", "实体名称"),
            // entity.serviceorder._self
            new TranslationSeedItem("entity.serviceorder._self", "ja-JP", "服务订单信息", "实体名称"),
            // entity.serviceorder._self
            new TranslationSeedItem("entity.serviceorder._self", "zh-CN", "服务订单信息", "实体名称"),
            // entity.serviceorder._self
            new TranslationSeedItem("entity.serviceorder._self", "zh-HK", "服务订单信息", "实体名称"),

            // entity.serviceorder.plantcode
            new TranslationSeedItem("entity.serviceorder.plantcode", "en-US", "工厂代码", "工厂代码"),
            // entity.serviceorder.plantcode
            new TranslationSeedItem("entity.serviceorder.plantcode", "ja-JP", "工厂代码", "工厂代码"),
            // entity.serviceorder.plantcode
            new TranslationSeedItem("entity.serviceorder.plantcode", "zh-CN", "工厂代码", "工厂代码"),
            // entity.serviceorder.plantcode
            new TranslationSeedItem("entity.serviceorder.plantcode", "zh-HK", "工厂代码", "工厂代码"),

            // entity.serviceorder.code
            new TranslationSeedItem("entity.serviceorder.code", "en-US", "服务订单编码", "服务订单编码（组合唯一索引）"),
            // entity.serviceorder.code
            new TranslationSeedItem("entity.serviceorder.code", "ja-JP", "服务订单编码", "服务订单编码（组合唯一索引）"),
            // entity.serviceorder.code
            new TranslationSeedItem("entity.serviceorder.code", "zh-CN", "服务订单编码", "服务订单编码（组合唯一索引）"),
            // entity.serviceorder.code
            new TranslationSeedItem("entity.serviceorder.code", "zh-HK", "服务订单编码", "服务订单编码（组合唯一索引）"),

            // entity.serviceorder.clientid
            new TranslationSeedItem("entity.serviceorder.clientid", "en-US", "客户端ID", "客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）"),
            // entity.serviceorder.clientid
            new TranslationSeedItem("entity.serviceorder.clientid", "ja-JP", "客户端ID", "客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）"),
            // entity.serviceorder.clientid
            new TranslationSeedItem("entity.serviceorder.clientid", "zh-CN", "客户端ID", "客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）"),
            // entity.serviceorder.clientid
            new TranslationSeedItem("entity.serviceorder.clientid", "zh-HK", "客户端ID", "客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）"),

            // entity.serviceorder.clientcode
            new TranslationSeedItem("entity.serviceorder.clientcode", "en-US", "客户端编码", "客户端编码（冗余字段，便于查询）"),
            // entity.serviceorder.clientcode
            new TranslationSeedItem("entity.serviceorder.clientcode", "ja-JP", "客户端编码", "客户端编码（冗余字段，便于查询）"),
            // entity.serviceorder.clientcode
            new TranslationSeedItem("entity.serviceorder.clientcode", "zh-CN", "客户端编码", "客户端编码（冗余字段，便于查询）"),
            // entity.serviceorder.clientcode
            new TranslationSeedItem("entity.serviceorder.clientcode", "zh-HK", "客户端编码", "客户端编码（冗余字段，便于查询）"),

            // entity.serviceorder.clientname
            new TranslationSeedItem("entity.serviceorder.clientname", "en-US", "客户端名称", "客户端名称（冗余字段，便于查询）"),
            // entity.serviceorder.clientname
            new TranslationSeedItem("entity.serviceorder.clientname", "ja-JP", "客户端名称", "客户端名称（冗余字段，便于查询）"),
            // entity.serviceorder.clientname
            new TranslationSeedItem("entity.serviceorder.clientname", "zh-CN", "客户端名称", "客户端名称（冗余字段，便于查询）"),
            // entity.serviceorder.clientname
            new TranslationSeedItem("entity.serviceorder.clientname", "zh-HK", "客户端名称", "客户端名称（冗余字段，便于查询）"),

            // entity.serviceorder.servicecontractid
            new TranslationSeedItem("entity.serviceorder.servicecontractid", "en-US", "关联服务合同ID", "关联服务合同ID（序列化为string以避免Javascript精度问题）"),
            // entity.serviceorder.servicecontractid
            new TranslationSeedItem("entity.serviceorder.servicecontractid", "ja-JP", "关联服务合同ID", "关联服务合同ID（序列化为string以避免Javascript精度问题）"),
            // entity.serviceorder.servicecontractid
            new TranslationSeedItem("entity.serviceorder.servicecontractid", "zh-CN", "关联服务合同ID", "关联服务合同ID（序列化为string以避免Javascript精度问题）"),
            // entity.serviceorder.servicecontractid
            new TranslationSeedItem("entity.serviceorder.servicecontractid", "zh-HK", "关联服务合同ID", "关联服务合同ID（序列化为string以避免Javascript精度问题）"),

            // entity.serviceorder.servicecontractcode
            new TranslationSeedItem("entity.serviceorder.servicecontractcode", "en-US", "关联服务合同编码", "关联服务合同编码（冗余字段，便于查询）"),
            // entity.serviceorder.servicecontractcode
            new TranslationSeedItem("entity.serviceorder.servicecontractcode", "ja-JP", "关联服务合同编码", "关联服务合同编码（冗余字段，便于查询）"),
            // entity.serviceorder.servicecontractcode
            new TranslationSeedItem("entity.serviceorder.servicecontractcode", "zh-CN", "关联服务合同编码", "关联服务合同编码（冗余字段，便于查询）"),
            // entity.serviceorder.servicecontractcode
            new TranslationSeedItem("entity.serviceorder.servicecontractcode", "zh-HK", "关联服务合同编码", "关联服务合同编码（冗余字段，便于查询）"),

            // entity.serviceorder.servicerequestid
            new TranslationSeedItem("entity.serviceorder.servicerequestid", "en-US", "关联服务请求ID", "关联服务请求ID（序列化为string以避免Javascript精度问题）"),
            // entity.serviceorder.servicerequestid
            new TranslationSeedItem("entity.serviceorder.servicerequestid", "ja-JP", "关联服务请求ID", "关联服务请求ID（序列化为string以避免Javascript精度问题）"),
            // entity.serviceorder.servicerequestid
            new TranslationSeedItem("entity.serviceorder.servicerequestid", "zh-CN", "关联服务请求ID", "关联服务请求ID（序列化为string以避免Javascript精度问题）"),
            // entity.serviceorder.servicerequestid
            new TranslationSeedItem("entity.serviceorder.servicerequestid", "zh-HK", "关联服务请求ID", "关联服务请求ID（序列化为string以避免Javascript精度问题）"),

            // entity.serviceorder.servicerequestcode
            new TranslationSeedItem("entity.serviceorder.servicerequestcode", "en-US", "关联服务请求单号", "关联服务请求单号（冗余字段，便于查询）"),
            // entity.serviceorder.servicerequestcode
            new TranslationSeedItem("entity.serviceorder.servicerequestcode", "ja-JP", "关联服务请求单号", "关联服务请求单号（冗余字段，便于查询）"),
            // entity.serviceorder.servicerequestcode
            new TranslationSeedItem("entity.serviceorder.servicerequestcode", "zh-CN", "关联服务请求单号", "关联服务请求单号（冗余字段，便于查询）"),
            // entity.serviceorder.servicerequestcode
            new TranslationSeedItem("entity.serviceorder.servicerequestcode", "zh-HK", "关联服务请求单号", "关联服务请求单号（冗余字段，便于查询）"),

            // entity.serviceorder.orderdate
            new TranslationSeedItem("entity.serviceorder.orderdate", "en-US", "订单日期", "订单日期"),
            // entity.serviceorder.orderdate
            new TranslationSeedItem("entity.serviceorder.orderdate", "ja-JP", "订单日期", "订单日期"),
            // entity.serviceorder.orderdate
            new TranslationSeedItem("entity.serviceorder.orderdate", "zh-CN", "订单日期", "订单日期"),
            // entity.serviceorder.orderdate
            new TranslationSeedItem("entity.serviceorder.orderdate", "zh-HK", "订单日期", "订单日期"),

            // entity.serviceorder.ordertype
            new TranslationSeedItem("entity.serviceorder.ordertype", "en-US", "订单类型", "订单类型（0=现场服务，1=远程支持，2=备件更换，3=安装调试，4=其他）"),
            // entity.serviceorder.ordertype
            new TranslationSeedItem("entity.serviceorder.ordertype", "ja-JP", "订单类型", "订单类型（0=现场服务，1=远程支持，2=备件更换，3=安装调试，4=其他）"),
            // entity.serviceorder.ordertype
            new TranslationSeedItem("entity.serviceorder.ordertype", "zh-CN", "订单类型", "订单类型（0=现场服务，1=远程支持，2=备件更换，3=安装调试，4=其他）"),
            // entity.serviceorder.ordertype
            new TranslationSeedItem("entity.serviceorder.ordertype", "zh-HK", "订单类型", "订单类型（0=现场服务，1=远程支持，2=备件更换，3=安装调试，4=其他）"),

            // entity.serviceorder.orderstatus
            new TranslationSeedItem("entity.serviceorder.orderstatus", "en-US", "订单状态", "订单状态（0=草稿，1=已确认，2=执行中，3=已完成，4=已结算，5=已取消）"),
            // entity.serviceorder.orderstatus
            new TranslationSeedItem("entity.serviceorder.orderstatus", "ja-JP", "订单状态", "订单状态（0=草稿，1=已确认，2=执行中，3=已完成，4=已结算，5=已取消）"),
            // entity.serviceorder.orderstatus
            new TranslationSeedItem("entity.serviceorder.orderstatus", "zh-CN", "订单状态", "订单状态（0=草稿，1=已确认，2=执行中，3=已完成，4=已结算，5=已取消）"),
            // entity.serviceorder.orderstatus
            new TranslationSeedItem("entity.serviceorder.orderstatus", "zh-HK", "订单状态", "订单状态（0=草稿，1=已确认，2=执行中，3=已完成，4=已结算，5=已取消）"),

            // entity.serviceorder.totalamount
            new TranslationSeedItem("entity.serviceorder.totalamount", "en-US", "订单总金额", "订单总金额"),
            // entity.serviceorder.totalamount
            new TranslationSeedItem("entity.serviceorder.totalamount", "ja-JP", "订单总金额", "订单总金额"),
            // entity.serviceorder.totalamount
            new TranslationSeedItem("entity.serviceorder.totalamount", "zh-CN", "订单总金额", "订单总金额"),
            // entity.serviceorder.totalamount
            new TranslationSeedItem("entity.serviceorder.totalamount", "zh-HK", "订单总金额", "订单总金额"),

            // entity.serviceorder.discountamount
            new TranslationSeedItem("entity.serviceorder.discountamount", "en-US", "折扣金额", "折扣金额"),
            // entity.serviceorder.discountamount
            new TranslationSeedItem("entity.serviceorder.discountamount", "ja-JP", "折扣金额", "折扣金额"),
            // entity.serviceorder.discountamount
            new TranslationSeedItem("entity.serviceorder.discountamount", "zh-CN", "折扣金额", "折扣金额"),
            // entity.serviceorder.discountamount
            new TranslationSeedItem("entity.serviceorder.discountamount", "zh-HK", "折扣金额", "折扣金额"),

            // entity.serviceorder.taxamount
            new TranslationSeedItem("entity.serviceorder.taxamount", "en-US", "税费", "税费"),
            // entity.serviceorder.taxamount
            new TranslationSeedItem("entity.serviceorder.taxamount", "ja-JP", "税费", "税费"),
            // entity.serviceorder.taxamount
            new TranslationSeedItem("entity.serviceorder.taxamount", "zh-CN", "税费", "税费"),
            // entity.serviceorder.taxamount
            new TranslationSeedItem("entity.serviceorder.taxamount", "zh-HK", "税费", "税费"),

            // entity.serviceorder.actualamount
            new TranslationSeedItem("entity.serviceorder.actualamount", "en-US", "订单实付金额", "订单实付金额"),
            // entity.serviceorder.actualamount
            new TranslationSeedItem("entity.serviceorder.actualamount", "ja-JP", "订单实付金额", "订单实付金额"),
            // entity.serviceorder.actualamount
            new TranslationSeedItem("entity.serviceorder.actualamount", "zh-CN", "订单实付金额", "订单实付金额"),
            // entity.serviceorder.actualamount
            new TranslationSeedItem("entity.serviceorder.actualamount", "zh-HK", "订单实付金额", "订单实付金额"),

            // entity.serviceorder.currencycode
            new TranslationSeedItem("entity.serviceorder.currencycode", "en-US", "结算币种代码", "结算币种代码"),
            // entity.serviceorder.currencycode
            new TranslationSeedItem("entity.serviceorder.currencycode", "ja-JP", "结算币种代码", "结算币种代码"),
            // entity.serviceorder.currencycode
            new TranslationSeedItem("entity.serviceorder.currencycode", "zh-CN", "结算币种代码", "结算币种代码"),
            // entity.serviceorder.currencycode
            new TranslationSeedItem("entity.serviceorder.currencycode", "zh-HK", "结算币种代码", "结算币种代码"),

            // entity.serviceorder.plannedstartdate
            new TranslationSeedItem("entity.serviceorder.plannedstartdate", "en-US", "计划开始日期", "计划开始日期"),
            // entity.serviceorder.plannedstartdate
            new TranslationSeedItem("entity.serviceorder.plannedstartdate", "ja-JP", "计划开始日期", "计划开始日期"),
            // entity.serviceorder.plannedstartdate
            new TranslationSeedItem("entity.serviceorder.plannedstartdate", "zh-CN", "计划开始日期", "计划开始日期"),
            // entity.serviceorder.plannedstartdate
            new TranslationSeedItem("entity.serviceorder.plannedstartdate", "zh-HK", "计划开始日期", "计划开始日期"),

            // entity.serviceorder.plannedenddate
            new TranslationSeedItem("entity.serviceorder.plannedenddate", "en-US", "计划结束日期", "计划结束日期"),
            // entity.serviceorder.plannedenddate
            new TranslationSeedItem("entity.serviceorder.plannedenddate", "ja-JP", "计划结束日期", "计划结束日期"),
            // entity.serviceorder.plannedenddate
            new TranslationSeedItem("entity.serviceorder.plannedenddate", "zh-CN", "计划结束日期", "计划结束日期"),
            // entity.serviceorder.plannedenddate
            new TranslationSeedItem("entity.serviceorder.plannedenddate", "zh-HK", "计划结束日期", "计划结束日期"),

            // entity.serviceorder.actualstartdate
            new TranslationSeedItem("entity.serviceorder.actualstartdate", "en-US", "实际开始日期", "实际开始日期"),
            // entity.serviceorder.actualstartdate
            new TranslationSeedItem("entity.serviceorder.actualstartdate", "ja-JP", "实际开始日期", "实际开始日期"),
            // entity.serviceorder.actualstartdate
            new TranslationSeedItem("entity.serviceorder.actualstartdate", "zh-CN", "实际开始日期", "实际开始日期"),
            // entity.serviceorder.actualstartdate
            new TranslationSeedItem("entity.serviceorder.actualstartdate", "zh-HK", "实际开始日期", "实际开始日期"),

            // entity.serviceorder.actualenddate
            new TranslationSeedItem("entity.serviceorder.actualenddate", "en-US", "实际结束日期", "实际结束日期"),
            // entity.serviceorder.actualenddate
            new TranslationSeedItem("entity.serviceorder.actualenddate", "ja-JP", "实际结束日期", "实际结束日期"),
            // entity.serviceorder.actualenddate
            new TranslationSeedItem("entity.serviceorder.actualenddate", "zh-CN", "实际结束日期", "实际结束日期"),
            // entity.serviceorder.actualenddate
            new TranslationSeedItem("entity.serviceorder.actualenddate", "zh-HK", "实际结束日期", "实际结束日期"),

            // entity.serviceorder.serviceby
            new TranslationSeedItem("entity.serviceorder.serviceby", "en-US", "服务负责人", "服务负责人（人员代码）"),
            // entity.serviceorder.serviceby
            new TranslationSeedItem("entity.serviceorder.serviceby", "ja-JP", "服务负责人", "服务负责人（人员代码）"),
            // entity.serviceorder.serviceby
            new TranslationSeedItem("entity.serviceorder.serviceby", "zh-CN", "服务负责人", "服务负责人（人员代码）"),
            // entity.serviceorder.serviceby
            new TranslationSeedItem("entity.serviceorder.serviceby", "zh-HK", "服务负责人", "服务负责人（人员代码）"),

            // entity.serviceorder.sortorder
            new TranslationSeedItem("entity.serviceorder.sortorder", "en-US", "排序号", "排序号（越小越靠前）"),
            // entity.serviceorder.sortorder
            new TranslationSeedItem("entity.serviceorder.sortorder", "ja-JP", "排序号", "排序号（越小越靠前）"),
            // entity.serviceorder.sortorder
            new TranslationSeedItem("entity.serviceorder.sortorder", "zh-CN", "排序号", "排序号（越小越靠前）"),
            // entity.serviceorder.sortorder
            new TranslationSeedItem("entity.serviceorder.sortorder", "zh-HK", "排序号", "排序号（越小越靠前）"),

            // entity.serviceorder.tickets
            new TranslationSeedItem("entity.serviceorder.tickets", "en-US", "服务工单列表", "服务工单列表（外键在子表 TaktServiceTicket.ServiceOrderId）"),
            // entity.serviceorder.tickets
            new TranslationSeedItem("entity.serviceorder.tickets", "ja-JP", "服务工单列表", "服务工单列表（外键在子表 TaktServiceTicket.ServiceOrderId）"),
            // entity.serviceorder.tickets
            new TranslationSeedItem("entity.serviceorder.tickets", "zh-CN", "服务工单列表", "服务工单列表（外键在子表 TaktServiceTicket.ServiceOrderId）"),
            // entity.serviceorder.tickets
            new TranslationSeedItem("entity.serviceorder.tickets", "zh-HK", "服务工单列表", "服务工单列表（外键在子表 TaktServiceTicket.ServiceOrderId）"),
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
