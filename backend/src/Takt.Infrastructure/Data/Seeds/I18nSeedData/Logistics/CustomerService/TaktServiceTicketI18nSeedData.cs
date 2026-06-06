// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.CustomerService
// 文件名称：TaktServiceTicketI18nSeedData.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktServiceTicket 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktServiceTicket 实体国际化翻译种子（键前缀 entity.serviceTicket.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktServiceTicketI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktServiceTicket 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 serviceTicket 实体翻译...", tenantCode);

        foreach (var item in GetServiceTicketTranslations())
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

        TaktLogger.Information("TaktServiceTicket 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktServiceTicket 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.serviceTicket._self / entity.serviceTicket.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetServiceTicketTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.serviceTicket._self
            new TranslationSeedItem("entity.serviceTicket._self", "en-US", "Service Ticket Information", "实体名称"),
            // entity.serviceTicket._self
            new TranslationSeedItem("entity.serviceTicket._self", "ja-JP", "服务工单信息", "实体名称"),
            // entity.serviceTicket._self
            new TranslationSeedItem("entity.serviceTicket._self", "zh-CN", "服务工单信息", "实体名称"),
            // entity.serviceTicket._self
            new TranslationSeedItem("entity.serviceTicket._self", "zh-HK", "服务工单信息", "实体名称"),

            // entity.serviceTicket.plantcode
            new TranslationSeedItem("entity.serviceTicket.plantcode", "en-US", "工厂代码", "工厂代码"),
            // entity.serviceTicket.plantcode
            new TranslationSeedItem("entity.serviceTicket.plantcode", "ja-JP", "工厂代码", "工厂代码"),
            // entity.serviceTicket.plantcode
            new TranslationSeedItem("entity.serviceTicket.plantcode", "zh-CN", "工厂代码", "工厂代码"),
            // entity.serviceTicket.plantcode
            new TranslationSeedItem("entity.serviceTicket.plantcode", "zh-HK", "工厂代码", "工厂代码"),

            // entity.serviceTicket.code
            new TranslationSeedItem("entity.serviceTicket.code", "en-US", "服务工单编码", "服务工单编码（组合唯一索引）"),
            // entity.serviceTicket.code
            new TranslationSeedItem("entity.serviceTicket.code", "ja-JP", "服务工单编码", "服务工单编码（组合唯一索引）"),
            // entity.serviceTicket.code
            new TranslationSeedItem("entity.serviceTicket.code", "zh-CN", "服务工单编码", "服务工单编码（组合唯一索引）"),
            // entity.serviceTicket.code
            new TranslationSeedItem("entity.serviceTicket.code", "zh-HK", "服务工单编码", "服务工单编码（组合唯一索引）"),

            // entity.serviceTicket.clientid
            new TranslationSeedItem("entity.serviceTicket.clientid", "en-US", "客户端ID", "客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）"),
            // entity.serviceTicket.clientid
            new TranslationSeedItem("entity.serviceTicket.clientid", "ja-JP", "客户端ID", "客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）"),
            // entity.serviceTicket.clientid
            new TranslationSeedItem("entity.serviceTicket.clientid", "zh-CN", "客户端ID", "客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）"),
            // entity.serviceTicket.clientid
            new TranslationSeedItem("entity.serviceTicket.clientid", "zh-HK", "客户端ID", "客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）"),

            // entity.serviceTicket.clientcode
            new TranslationSeedItem("entity.serviceTicket.clientcode", "en-US", "客户端编码", "客户端编码（冗余字段，便于查询）"),
            // entity.serviceTicket.clientcode
            new TranslationSeedItem("entity.serviceTicket.clientcode", "ja-JP", "客户端编码", "客户端编码（冗余字段，便于查询）"),
            // entity.serviceTicket.clientcode
            new TranslationSeedItem("entity.serviceTicket.clientcode", "zh-CN", "客户端编码", "客户端编码（冗余字段，便于查询）"),
            // entity.serviceTicket.clientcode
            new TranslationSeedItem("entity.serviceTicket.clientcode", "zh-HK", "客户端编码", "客户端编码（冗余字段，便于查询）"),

            // entity.serviceTicket.clientname
            new TranslationSeedItem("entity.serviceTicket.clientname", "en-US", "客户端名称", "客户端名称（冗余字段，便于查询）"),
            // entity.serviceTicket.clientname
            new TranslationSeedItem("entity.serviceTicket.clientname", "ja-JP", "客户端名称", "客户端名称（冗余字段，便于查询）"),
            // entity.serviceTicket.clientname
            new TranslationSeedItem("entity.serviceTicket.clientname", "zh-CN", "客户端名称", "客户端名称（冗余字段，便于查询）"),
            // entity.serviceTicket.clientname
            new TranslationSeedItem("entity.serviceTicket.clientname", "zh-HK", "客户端名称", "客户端名称（冗余字段，便于查询）"),

            // entity.serviceTicket.servicerequestid
            new TranslationSeedItem("entity.serviceTicket.servicerequestid", "en-US", "关联服务请求ID", "关联服务请求ID（序列化为string以避免Javascript精度问题）"),
            // entity.serviceTicket.servicerequestid
            new TranslationSeedItem("entity.serviceTicket.servicerequestid", "ja-JP", "关联服务请求ID", "关联服务请求ID（序列化为string以避免Javascript精度问题）"),
            // entity.serviceTicket.servicerequestid
            new TranslationSeedItem("entity.serviceTicket.servicerequestid", "zh-CN", "关联服务请求ID", "关联服务请求ID（序列化为string以避免Javascript精度问题）"),
            // entity.serviceTicket.servicerequestid
            new TranslationSeedItem("entity.serviceTicket.servicerequestid", "zh-HK", "关联服务请求ID", "关联服务请求ID（序列化为string以避免Javascript精度问题）"),

            // entity.serviceTicket.servicerequestcode
            new TranslationSeedItem("entity.serviceTicket.servicerequestcode", "en-US", "关联服务请求单号", "关联服务请求单号（冗余字段，便于查询）"),
            // entity.serviceTicket.servicerequestcode
            new TranslationSeedItem("entity.serviceTicket.servicerequestcode", "ja-JP", "关联服务请求单号", "关联服务请求单号（冗余字段，便于查询）"),
            // entity.serviceTicket.servicerequestcode
            new TranslationSeedItem("entity.serviceTicket.servicerequestcode", "zh-CN", "关联服务请求单号", "关联服务请求单号（冗余字段，便于查询）"),
            // entity.serviceTicket.servicerequestcode
            new TranslationSeedItem("entity.serviceTicket.servicerequestcode", "zh-HK", "关联服务请求单号", "关联服务请求单号（冗余字段，便于查询）"),

            // entity.serviceTicket.serviceorderid
            new TranslationSeedItem("entity.serviceTicket.serviceorderid", "en-US", "关联服务订单ID", "关联服务订单ID（序列化为string以避免Javascript精度问题）"),
            // entity.serviceTicket.serviceorderid
            new TranslationSeedItem("entity.serviceTicket.serviceorderid", "ja-JP", "关联服务订单ID", "关联服务订单ID（序列化为string以避免Javascript精度问题）"),
            // entity.serviceTicket.serviceorderid
            new TranslationSeedItem("entity.serviceTicket.serviceorderid", "zh-CN", "关联服务订单ID", "关联服务订单ID（序列化为string以避免Javascript精度问题）"),
            // entity.serviceTicket.serviceorderid
            new TranslationSeedItem("entity.serviceTicket.serviceorderid", "zh-HK", "关联服务订单ID", "关联服务订单ID（序列化为string以避免Javascript精度问题）"),

            // entity.serviceTicket.serviceordercode
            new TranslationSeedItem("entity.serviceTicket.serviceordercode", "en-US", "关联服务订单编码", "关联服务订单编码（冗余字段，便于查询）"),
            // entity.serviceTicket.serviceordercode
            new TranslationSeedItem("entity.serviceTicket.serviceordercode", "ja-JP", "关联服务订单编码", "关联服务订单编码（冗余字段，便于查询）"),
            // entity.serviceTicket.serviceordercode
            new TranslationSeedItem("entity.serviceTicket.serviceordercode", "zh-CN", "关联服务订单编码", "关联服务订单编码（冗余字段，便于查询）"),
            // entity.serviceTicket.serviceordercode
            new TranslationSeedItem("entity.serviceTicket.serviceordercode", "zh-HK", "关联服务订单编码", "关联服务订单编码（冗余字段，便于查询）"),

            // entity.serviceTicket.servicecontractid
            new TranslationSeedItem("entity.serviceTicket.servicecontractid", "en-US", "关联服务合同ID", "关联服务合同ID（序列化为string以避免Javascript精度问题）"),
            // entity.serviceTicket.servicecontractid
            new TranslationSeedItem("entity.serviceTicket.servicecontractid", "ja-JP", "关联服务合同ID", "关联服务合同ID（序列化为string以避免Javascript精度问题）"),
            // entity.serviceTicket.servicecontractid
            new TranslationSeedItem("entity.serviceTicket.servicecontractid", "zh-CN", "关联服务合同ID", "关联服务合同ID（序列化为string以避免Javascript精度问题）"),
            // entity.serviceTicket.servicecontractid
            new TranslationSeedItem("entity.serviceTicket.servicecontractid", "zh-HK", "关联服务合同ID", "关联服务合同ID（序列化为string以避免Javascript精度问题）"),

            // entity.serviceTicket.servicecontractcode
            new TranslationSeedItem("entity.serviceTicket.servicecontractcode", "en-US", "关联服务合同编码", "关联服务合同编码（冗余字段，便于查询）"),
            // entity.serviceTicket.servicecontractcode
            new TranslationSeedItem("entity.serviceTicket.servicecontractcode", "ja-JP", "关联服务合同编码", "关联服务合同编码（冗余字段，便于查询）"),
            // entity.serviceTicket.servicecontractcode
            new TranslationSeedItem("entity.serviceTicket.servicecontractcode", "zh-CN", "关联服务合同编码", "关联服务合同编码（冗余字段，便于查询）"),
            // entity.serviceTicket.servicecontractcode
            new TranslationSeedItem("entity.serviceTicket.servicecontractcode", "zh-HK", "关联服务合同编码", "关联服务合同编码（冗余字段，便于查询）"),

            // entity.serviceTicket.tickettype
            new TranslationSeedItem("entity.serviceTicket.tickettype", "en-US", "工单类型", "工单类型（0=维修，1=巡检，2=安装，3=升级，4=其他）"),
            // entity.serviceTicket.tickettype
            new TranslationSeedItem("entity.serviceTicket.tickettype", "ja-JP", "工单类型", "工单类型（0=维修，1=巡检，2=安装，3=升级，4=其他）"),
            // entity.serviceTicket.tickettype
            new TranslationSeedItem("entity.serviceTicket.tickettype", "zh-CN", "工单类型", "工单类型（0=维修，1=巡检，2=安装，3=升级，4=其他）"),
            // entity.serviceTicket.tickettype
            new TranslationSeedItem("entity.serviceTicket.tickettype", "zh-HK", "工单类型", "工单类型（0=维修，1=巡检，2=安装，3=升级，4=其他）"),

            // entity.serviceTicket.priority
            new TranslationSeedItem("entity.serviceTicket.priority", "en-US", "优先级", "优先级（0=低，1=中，2=高，3=紧急）"),
            // entity.serviceTicket.priority
            new TranslationSeedItem("entity.serviceTicket.priority", "ja-JP", "优先级", "优先级（0=低，1=中，2=高，3=紧急）"),
            // entity.serviceTicket.priority
            new TranslationSeedItem("entity.serviceTicket.priority", "zh-CN", "优先级", "优先级（0=低，1=中，2=高，3=紧急）"),
            // entity.serviceTicket.priority
            new TranslationSeedItem("entity.serviceTicket.priority", "zh-HK", "优先级", "优先级（0=低，1=中，2=高，3=紧急）"),

            // entity.serviceTicket.ticketstatus
            new TranslationSeedItem("entity.serviceTicket.ticketstatus", "en-US", "工单状态", "工单状态（0=待派工，1=已派工，2=处理中，3=待验收，4=已完成，5=已关闭，6=已取消）"),
            // entity.serviceTicket.ticketstatus
            new TranslationSeedItem("entity.serviceTicket.ticketstatus", "ja-JP", "工单状态", "工单状态（0=待派工，1=已派工，2=处理中，3=待验收，4=已完成，5=已关闭，6=已取消）"),
            // entity.serviceTicket.ticketstatus
            new TranslationSeedItem("entity.serviceTicket.ticketstatus", "zh-CN", "工单状态", "工单状态（0=待派工，1=已派工，2=处理中，3=待验收，4=已完成，5=已关闭，6=已取消）"),
            // entity.serviceTicket.ticketstatus
            new TranslationSeedItem("entity.serviceTicket.ticketstatus", "zh-HK", "工单状态", "工单状态（0=待派工，1=已派工，2=处理中，3=待验收，4=已完成，5=已关闭，6=已取消）"),

            // entity.serviceTicket.ticketsubject
            new TranslationSeedItem("entity.serviceTicket.ticketsubject", "en-US", "工单主题", "工单主题"),
            // entity.serviceTicket.ticketsubject
            new TranslationSeedItem("entity.serviceTicket.ticketsubject", "ja-JP", "工单主题", "工单主题"),
            // entity.serviceTicket.ticketsubject
            new TranslationSeedItem("entity.serviceTicket.ticketsubject", "zh-CN", "工单主题", "工单主题"),
            // entity.serviceTicket.ticketsubject
            new TranslationSeedItem("entity.serviceTicket.ticketsubject", "zh-HK", "工单主题", "工单主题"),

            // entity.serviceTicket.faultdescription
            new TranslationSeedItem("entity.serviceTicket.faultdescription", "en-US", "故障描述", "故障/问题描述"),
            // entity.serviceTicket.faultdescription
            new TranslationSeedItem("entity.serviceTicket.faultdescription", "ja-JP", "故障描述", "故障/问题描述"),
            // entity.serviceTicket.faultdescription
            new TranslationSeedItem("entity.serviceTicket.faultdescription", "zh-CN", "故障描述", "故障/问题描述"),
            // entity.serviceTicket.faultdescription
            new TranslationSeedItem("entity.serviceTicket.faultdescription", "zh-HK", "故障描述", "故障/问题描述"),

            // entity.serviceTicket.solutiondescription
            new TranslationSeedItem("entity.serviceTicket.solutiondescription", "en-US", "处理方案", "处理方案/解决说明"),
            // entity.serviceTicket.solutiondescription
            new TranslationSeedItem("entity.serviceTicket.solutiondescription", "ja-JP", "处理方案", "处理方案/解决说明"),
            // entity.serviceTicket.solutiondescription
            new TranslationSeedItem("entity.serviceTicket.solutiondescription", "zh-CN", "处理方案", "处理方案/解决说明"),
            // entity.serviceTicket.solutiondescription
            new TranslationSeedItem("entity.serviceTicket.solutiondescription", "zh-HK", "处理方案", "处理方案/解决说明"),

            // entity.serviceTicket.servicelocation
            new TranslationSeedItem("entity.serviceTicket.servicelocation", "en-US", "服务地点", "服务地点"),
            // entity.serviceTicket.servicelocation
            new TranslationSeedItem("entity.serviceTicket.servicelocation", "ja-JP", "服务地点", "服务地点"),
            // entity.serviceTicket.servicelocation
            new TranslationSeedItem("entity.serviceTicket.servicelocation", "zh-CN", "服务地点", "服务地点"),
            // entity.serviceTicket.servicelocation
            new TranslationSeedItem("entity.serviceTicket.servicelocation", "zh-HK", "服务地点", "服务地点"),

            // entity.serviceTicket.assignedemployeeid
            new TranslationSeedItem("entity.serviceTicket.assignedemployeeid", "en-US", "指派服务人员工ID", "指派服务人员工ID（序列化为string以避免Javascript精度问题）"),
            // entity.serviceTicket.assignedemployeeid
            new TranslationSeedItem("entity.serviceTicket.assignedemployeeid", "ja-JP", "指派服务人员工ID", "指派服务人员工ID（序列化为string以避免Javascript精度问题）"),
            // entity.serviceTicket.assignedemployeeid
            new TranslationSeedItem("entity.serviceTicket.assignedemployeeid", "zh-CN", "指派服务人员工ID", "指派服务人员工ID（序列化为string以避免Javascript精度问题）"),
            // entity.serviceTicket.assignedemployeeid
            new TranslationSeedItem("entity.serviceTicket.assignedemployeeid", "zh-HK", "指派服务人员工ID", "指派服务人员工ID（序列化为string以避免Javascript精度问题）"),

            // entity.serviceTicket.assignedemployeename
            new TranslationSeedItem("entity.serviceTicket.assignedemployeename", "en-US", "指派服务人员姓名", "指派服务人员姓名"),
            // entity.serviceTicket.assignedemployeename
            new TranslationSeedItem("entity.serviceTicket.assignedemployeename", "ja-JP", "指派服务人员姓名", "指派服务人员姓名"),
            // entity.serviceTicket.assignedemployeename
            new TranslationSeedItem("entity.serviceTicket.assignedemployeename", "zh-CN", "指派服务人员姓名", "指派服务人员姓名"),
            // entity.serviceTicket.assignedemployeename
            new TranslationSeedItem("entity.serviceTicket.assignedemployeename", "zh-HK", "指派服务人员姓名", "指派服务人员姓名"),

            // entity.serviceTicket.scheduledstarttime
            new TranslationSeedItem("entity.serviceTicket.scheduledstarttime", "en-US", "计划开始时间", "计划开始时间"),
            // entity.serviceTicket.scheduledstarttime
            new TranslationSeedItem("entity.serviceTicket.scheduledstarttime", "ja-JP", "计划开始时间", "计划开始时间"),
            // entity.serviceTicket.scheduledstarttime
            new TranslationSeedItem("entity.serviceTicket.scheduledstarttime", "zh-CN", "计划开始时间", "计划开始时间"),
            // entity.serviceTicket.scheduledstarttime
            new TranslationSeedItem("entity.serviceTicket.scheduledstarttime", "zh-HK", "计划开始时间", "计划开始时间"),

            // entity.serviceTicket.scheduledendtime
            new TranslationSeedItem("entity.serviceTicket.scheduledendtime", "en-US", "计划结束时间", "计划结束时间"),
            // entity.serviceTicket.scheduledendtime
            new TranslationSeedItem("entity.serviceTicket.scheduledendtime", "ja-JP", "计划结束时间", "计划结束时间"),
            // entity.serviceTicket.scheduledendtime
            new TranslationSeedItem("entity.serviceTicket.scheduledendtime", "zh-CN", "计划结束时间", "计划结束时间"),
            // entity.serviceTicket.scheduledendtime
            new TranslationSeedItem("entity.serviceTicket.scheduledendtime", "zh-HK", "计划结束时间", "计划结束时间"),

            // entity.serviceTicket.actualstarttime
            new TranslationSeedItem("entity.serviceTicket.actualstarttime", "en-US", "实际开始时间", "实际开始时间"),
            // entity.serviceTicket.actualstarttime
            new TranslationSeedItem("entity.serviceTicket.actualstarttime", "ja-JP", "实际开始时间", "实际开始时间"),
            // entity.serviceTicket.actualstarttime
            new TranslationSeedItem("entity.serviceTicket.actualstarttime", "zh-CN", "实际开始时间", "实际开始时间"),
            // entity.serviceTicket.actualstarttime
            new TranslationSeedItem("entity.serviceTicket.actualstarttime", "zh-HK", "实际开始时间", "实际开始时间"),

            // entity.serviceTicket.actualendtime
            new TranslationSeedItem("entity.serviceTicket.actualendtime", "en-US", "实际结束时间", "实际结束时间"),
            // entity.serviceTicket.actualendtime
            new TranslationSeedItem("entity.serviceTicket.actualendtime", "ja-JP", "实际结束时间", "实际结束时间"),
            // entity.serviceTicket.actualendtime
            new TranslationSeedItem("entity.serviceTicket.actualendtime", "zh-CN", "实际结束时间", "实际结束时间"),
            // entity.serviceTicket.actualendtime
            new TranslationSeedItem("entity.serviceTicket.actualendtime", "zh-HK", "实际结束时间", "实际结束时间"),

            // entity.serviceTicket.acceptanceresult
            new TranslationSeedItem("entity.serviceTicket.acceptanceresult", "en-US", "验收结果", "验收结果（0=不合格，1=合格，2=部分合格）"),
            // entity.serviceTicket.acceptanceresult
            new TranslationSeedItem("entity.serviceTicket.acceptanceresult", "ja-JP", "验收结果", "验收结果（0=不合格，1=合格，2=部分合格）"),
            // entity.serviceTicket.acceptanceresult
            new TranslationSeedItem("entity.serviceTicket.acceptanceresult", "zh-CN", "验收结果", "验收结果（0=不合格，1=合格，2=部分合格）"),
            // entity.serviceTicket.acceptanceresult
            new TranslationSeedItem("entity.serviceTicket.acceptanceresult", "zh-HK", "验收结果", "验收结果（0=不合格，1=合格，2=部分合格）"),

            // entity.serviceTicket.acceptedby
            new TranslationSeedItem("entity.serviceTicket.acceptedby", "en-US", "验收人", "验收人"),
            // entity.serviceTicket.acceptedby
            new TranslationSeedItem("entity.serviceTicket.acceptedby", "ja-JP", "验收人", "验收人"),
            // entity.serviceTicket.acceptedby
            new TranslationSeedItem("entity.serviceTicket.acceptedby", "zh-CN", "验收人", "验收人"),
            // entity.serviceTicket.acceptedby
            new TranslationSeedItem("entity.serviceTicket.acceptedby", "zh-HK", "验收人", "验收人"),

            // entity.serviceTicket.acceptedat
            new TranslationSeedItem("entity.serviceTicket.acceptedat", "en-US", "验收时间", "验收时间"),
            // entity.serviceTicket.acceptedat
            new TranslationSeedItem("entity.serviceTicket.acceptedat", "ja-JP", "验收时间", "验收时间"),
            // entity.serviceTicket.acceptedat
            new TranslationSeedItem("entity.serviceTicket.acceptedat", "zh-CN", "验收时间", "验收时间"),
            // entity.serviceTicket.acceptedat
            new TranslationSeedItem("entity.serviceTicket.acceptedat", "zh-HK", "验收时间", "验收时间"),

            // entity.serviceTicket.sortorder
            new TranslationSeedItem("entity.serviceTicket.sortorder", "en-US", "排序号", "排序号（越小越靠前）"),
            // entity.serviceTicket.sortorder
            new TranslationSeedItem("entity.serviceTicket.sortorder", "ja-JP", "排序号", "排序号（越小越靠前）"),
            // entity.serviceTicket.sortorder
            new TranslationSeedItem("entity.serviceTicket.sortorder", "zh-CN", "排序号", "排序号（越小越靠前）"),
            // entity.serviceTicket.sortorder
            new TranslationSeedItem("entity.serviceTicket.sortorder", "zh-HK", "排序号", "排序号（越小越靠前）"),
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
