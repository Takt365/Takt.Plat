// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.CustomerService
// 文件名称：TaktServiceTicketI18nSeedData.cs
// 创建时间：2026-07-09
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
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.CustomerService;

/// <summary>
/// TaktServiceTicket 实体国际化翻译种子（键前缀 entity.serviceticket.*）
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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 serviceticket 实体翻译...", tenantCode);

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
    /// I18nKey：entity.serviceticket._self / entity.serviceticket.{{field}}；ResourceGroup=CustomerService；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetServiceTicketTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.serviceticket._self
            new TranslationSeedItem("entity.serviceticket._self", "en-US", "Service Ticket Information_us", "实体名称"),
            // entity.serviceticket._self
            new TranslationSeedItem("entity.serviceticket._self", "ja-JP", "服务工单信息_jp", "实体名称"),
            // entity.serviceticket._self
            new TranslationSeedItem("entity.serviceticket._self", "zh-CN", "服务工单信息", "实体名称"),
            // entity.serviceticket._self
            new TranslationSeedItem("entity.serviceticket._self", "zh-HK", "服务工单信息_hk", "实体名称"),

            // entity.serviceticket.plantcode
            new TranslationSeedItem("entity.serviceticket.plantcode", "en-US", "工厂代码_us", "工厂代码"),
            // entity.serviceticket.plantcode
            new TranslationSeedItem("entity.serviceticket.plantcode", "ja-JP", "工厂代码_jp", "工厂代码"),
            // entity.serviceticket.plantcode
            new TranslationSeedItem("entity.serviceticket.plantcode", "zh-CN", "工厂代码", "工厂代码"),
            // entity.serviceticket.plantcode
            new TranslationSeedItem("entity.serviceticket.plantcode", "zh-HK", "工厂代码_hk", "工厂代码"),

            // entity.serviceticket.code
            new TranslationSeedItem("entity.serviceticket.code", "en-US", "服务工单编码_us", "服务工单编码（组合唯一索引）"),
            // entity.serviceticket.code
            new TranslationSeedItem("entity.serviceticket.code", "ja-JP", "服务工单编码_jp", "服务工单编码（组合唯一索引）"),
            // entity.serviceticket.code
            new TranslationSeedItem("entity.serviceticket.code", "zh-CN", "服务工单编码", "服务工单编码（组合唯一索引）"),
            // entity.serviceticket.code
            new TranslationSeedItem("entity.serviceticket.code", "zh-HK", "服务工单编码_hk", "服务工单编码（组合唯一索引）"),

            // entity.serviceticket.clientid
            new TranslationSeedItem("entity.serviceticket.clientid", "en-US", "客户端ID_us", "客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）"),
            // entity.serviceticket.clientid
            new TranslationSeedItem("entity.serviceticket.clientid", "ja-JP", "客户端ID_jp", "客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）"),
            // entity.serviceticket.clientid
            new TranslationSeedItem("entity.serviceticket.clientid", "zh-CN", "客户端ID", "客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）"),
            // entity.serviceticket.clientid
            new TranslationSeedItem("entity.serviceticket.clientid", "zh-HK", "客户端ID_hk", "客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）"),

            // entity.serviceticket.clientcode
            new TranslationSeedItem("entity.serviceticket.clientcode", "en-US", "客户端编码_us", "客户端编码（冗余字段，便于查询）"),
            // entity.serviceticket.clientcode
            new TranslationSeedItem("entity.serviceticket.clientcode", "ja-JP", "客户端编码_jp", "客户端编码（冗余字段，便于查询）"),
            // entity.serviceticket.clientcode
            new TranslationSeedItem("entity.serviceticket.clientcode", "zh-CN", "客户端编码", "客户端编码（冗余字段，便于查询）"),
            // entity.serviceticket.clientcode
            new TranslationSeedItem("entity.serviceticket.clientcode", "zh-HK", "客户端编码_hk", "客户端编码（冗余字段，便于查询）"),

            // entity.serviceticket.clientname
            new TranslationSeedItem("entity.serviceticket.clientname", "en-US", "客户端名称_us", "客户端名称（冗余字段，便于查询）"),
            // entity.serviceticket.clientname
            new TranslationSeedItem("entity.serviceticket.clientname", "ja-JP", "客户端名称_jp", "客户端名称（冗余字段，便于查询）"),
            // entity.serviceticket.clientname
            new TranslationSeedItem("entity.serviceticket.clientname", "zh-CN", "客户端名称", "客户端名称（冗余字段，便于查询）"),
            // entity.serviceticket.clientname
            new TranslationSeedItem("entity.serviceticket.clientname", "zh-HK", "客户端名称_hk", "客户端名称（冗余字段，便于查询）"),

            // entity.serviceticket.servicerequestid
            new TranslationSeedItem("entity.serviceticket.servicerequestid", "en-US", "关联服务请求ID_us", "关联服务请求ID（序列化为string以避免Javascript精度问题）"),
            // entity.serviceticket.servicerequestid
            new TranslationSeedItem("entity.serviceticket.servicerequestid", "ja-JP", "关联服务请求ID_jp", "关联服务请求ID（序列化为string以避免Javascript精度问题）"),
            // entity.serviceticket.servicerequestid
            new TranslationSeedItem("entity.serviceticket.servicerequestid", "zh-CN", "关联服务请求ID", "关联服务请求ID（序列化为string以避免Javascript精度问题）"),
            // entity.serviceticket.servicerequestid
            new TranslationSeedItem("entity.serviceticket.servicerequestid", "zh-HK", "关联服务请求ID_hk", "关联服务请求ID（序列化为string以避免Javascript精度问题）"),

            // entity.serviceticket.servicerequestcode
            new TranslationSeedItem("entity.serviceticket.servicerequestcode", "en-US", "关联服务请求单号_us", "关联服务请求单号（冗余字段，便于查询）"),
            // entity.serviceticket.servicerequestcode
            new TranslationSeedItem("entity.serviceticket.servicerequestcode", "ja-JP", "关联服务请求单号_jp", "关联服务请求单号（冗余字段，便于查询）"),
            // entity.serviceticket.servicerequestcode
            new TranslationSeedItem("entity.serviceticket.servicerequestcode", "zh-CN", "关联服务请求单号", "关联服务请求单号（冗余字段，便于查询）"),
            // entity.serviceticket.servicerequestcode
            new TranslationSeedItem("entity.serviceticket.servicerequestcode", "zh-HK", "关联服务请求单号_hk", "关联服务请求单号（冗余字段，便于查询）"),

            // entity.serviceticket.serviceorderid
            new TranslationSeedItem("entity.serviceticket.serviceorderid", "en-US", "关联服务订单ID_us", "关联服务订单ID（序列化为string以避免Javascript精度问题）"),
            // entity.serviceticket.serviceorderid
            new TranslationSeedItem("entity.serviceticket.serviceorderid", "ja-JP", "关联服务订单ID_jp", "关联服务订单ID（序列化为string以避免Javascript精度问题）"),
            // entity.serviceticket.serviceorderid
            new TranslationSeedItem("entity.serviceticket.serviceorderid", "zh-CN", "关联服务订单ID", "关联服务订单ID（序列化为string以避免Javascript精度问题）"),
            // entity.serviceticket.serviceorderid
            new TranslationSeedItem("entity.serviceticket.serviceorderid", "zh-HK", "关联服务订单ID_hk", "关联服务订单ID（序列化为string以避免Javascript精度问题）"),

            // entity.serviceticket.serviceordercode
            new TranslationSeedItem("entity.serviceticket.serviceordercode", "en-US", "关联服务订单编码_us", "关联服务订单编码（冗余字段，便于查询）"),
            // entity.serviceticket.serviceordercode
            new TranslationSeedItem("entity.serviceticket.serviceordercode", "ja-JP", "关联服务订单编码_jp", "关联服务订单编码（冗余字段，便于查询）"),
            // entity.serviceticket.serviceordercode
            new TranslationSeedItem("entity.serviceticket.serviceordercode", "zh-CN", "关联服务订单编码", "关联服务订单编码（冗余字段，便于查询）"),
            // entity.serviceticket.serviceordercode
            new TranslationSeedItem("entity.serviceticket.serviceordercode", "zh-HK", "关联服务订单编码_hk", "关联服务订单编码（冗余字段，便于查询）"),

            // entity.serviceticket.servicecontractid
            new TranslationSeedItem("entity.serviceticket.servicecontractid", "en-US", "关联服务合同ID_us", "关联服务合同ID（序列化为string以避免Javascript精度问题）"),
            // entity.serviceticket.servicecontractid
            new TranslationSeedItem("entity.serviceticket.servicecontractid", "ja-JP", "关联服务合同ID_jp", "关联服务合同ID（序列化为string以避免Javascript精度问题）"),
            // entity.serviceticket.servicecontractid
            new TranslationSeedItem("entity.serviceticket.servicecontractid", "zh-CN", "关联服务合同ID", "关联服务合同ID（序列化为string以避免Javascript精度问题）"),
            // entity.serviceticket.servicecontractid
            new TranslationSeedItem("entity.serviceticket.servicecontractid", "zh-HK", "关联服务合同ID_hk", "关联服务合同ID（序列化为string以避免Javascript精度问题）"),

            // entity.serviceticket.servicecontractcode
            new TranslationSeedItem("entity.serviceticket.servicecontractcode", "en-US", "关联服务合同编码_us", "关联服务合同编码（冗余字段，便于查询）"),
            // entity.serviceticket.servicecontractcode
            new TranslationSeedItem("entity.serviceticket.servicecontractcode", "ja-JP", "关联服务合同编码_jp", "关联服务合同编码（冗余字段，便于查询）"),
            // entity.serviceticket.servicecontractcode
            new TranslationSeedItem("entity.serviceticket.servicecontractcode", "zh-CN", "关联服务合同编码", "关联服务合同编码（冗余字段，便于查询）"),
            // entity.serviceticket.servicecontractcode
            new TranslationSeedItem("entity.serviceticket.servicecontractcode", "zh-HK", "关联服务合同编码_hk", "关联服务合同编码（冗余字段，便于查询）"),

            // entity.serviceticket.tickettype
            new TranslationSeedItem("entity.serviceticket.tickettype", "en-US", "工单类型_us", "工单类型（0=维修，1=巡检，2=安装，3=升级，4=其他）"),
            // entity.serviceticket.tickettype
            new TranslationSeedItem("entity.serviceticket.tickettype", "ja-JP", "工单类型_jp", "工单类型（0=维修，1=巡检，2=安装，3=升级，4=其他）"),
            // entity.serviceticket.tickettype
            new TranslationSeedItem("entity.serviceticket.tickettype", "zh-CN", "工单类型", "工单类型（0=维修，1=巡检，2=安装，3=升级，4=其他）"),
            // entity.serviceticket.tickettype
            new TranslationSeedItem("entity.serviceticket.tickettype", "zh-HK", "工单类型_hk", "工单类型（0=维修，1=巡检，2=安装，3=升级，4=其他）"),

            // entity.serviceticket.priority
            new TranslationSeedItem("entity.serviceticket.priority", "en-US", "优先级_us", "优先级（字典 sys_priority_level_category）"),
            // entity.serviceticket.priority
            new TranslationSeedItem("entity.serviceticket.priority", "ja-JP", "优先级_jp", "优先级（字典 sys_priority_level_category）"),
            // entity.serviceticket.priority
            new TranslationSeedItem("entity.serviceticket.priority", "zh-CN", "优先级", "优先级（字典 sys_priority_level_category）"),
            // entity.serviceticket.priority
            new TranslationSeedItem("entity.serviceticket.priority", "zh-HK", "优先级_hk", "优先级（字典 sys_priority_level_category）"),

            // entity.serviceticket.ticketstatus
            new TranslationSeedItem("entity.serviceticket.ticketstatus", "en-US", "工单状态_us", "工单状态（字典 sys_ticket_status；0=新建，1=已分配，2=处理中，3=待确认，4=已完成，5=已关闭，6=已取消）"),
            // entity.serviceticket.ticketstatus
            new TranslationSeedItem("entity.serviceticket.ticketstatus", "ja-JP", "工单状态_jp", "工单状态（字典 sys_ticket_status；0=新建，1=已分配，2=处理中，3=待确认，4=已完成，5=已关闭，6=已取消）"),
            // entity.serviceticket.ticketstatus
            new TranslationSeedItem("entity.serviceticket.ticketstatus", "zh-CN", "工单状态", "工单状态（字典 sys_ticket_status；0=新建，1=已分配，2=处理中，3=待确认，4=已完成，5=已关闭，6=已取消）"),
            // entity.serviceticket.ticketstatus
            new TranslationSeedItem("entity.serviceticket.ticketstatus", "zh-HK", "工单状态_hk", "工单状态（字典 sys_ticket_status；0=新建，1=已分配，2=处理中，3=待确认，4=已完成，5=已关闭，6=已取消）"),

            // entity.serviceticket.ticketsubject
            new TranslationSeedItem("entity.serviceticket.ticketsubject", "en-US", "工单主题_us", "工单主题"),
            // entity.serviceticket.ticketsubject
            new TranslationSeedItem("entity.serviceticket.ticketsubject", "ja-JP", "工单主题_jp", "工单主题"),
            // entity.serviceticket.ticketsubject
            new TranslationSeedItem("entity.serviceticket.ticketsubject", "zh-CN", "工单主题", "工单主题"),
            // entity.serviceticket.ticketsubject
            new TranslationSeedItem("entity.serviceticket.ticketsubject", "zh-HK", "工单主题_hk", "工单主题"),

            // entity.serviceticket.faultdescription
            new TranslationSeedItem("entity.serviceticket.faultdescription", "en-US", "故障描述_us", "故障/问题描述"),
            // entity.serviceticket.faultdescription
            new TranslationSeedItem("entity.serviceticket.faultdescription", "ja-JP", "故障描述_jp", "故障/问题描述"),
            // entity.serviceticket.faultdescription
            new TranslationSeedItem("entity.serviceticket.faultdescription", "zh-CN", "故障描述", "故障/问题描述"),
            // entity.serviceticket.faultdescription
            new TranslationSeedItem("entity.serviceticket.faultdescription", "zh-HK", "故障描述_hk", "故障/问题描述"),

            // entity.serviceticket.solutiondescription
            new TranslationSeedItem("entity.serviceticket.solutiondescription", "en-US", "处理方案_us", "处理方案/解决说明"),
            // entity.serviceticket.solutiondescription
            new TranslationSeedItem("entity.serviceticket.solutiondescription", "ja-JP", "处理方案_jp", "处理方案/解决说明"),
            // entity.serviceticket.solutiondescription
            new TranslationSeedItem("entity.serviceticket.solutiondescription", "zh-CN", "处理方案", "处理方案/解决说明"),
            // entity.serviceticket.solutiondescription
            new TranslationSeedItem("entity.serviceticket.solutiondescription", "zh-HK", "处理方案_hk", "处理方案/解决说明"),

            // entity.serviceticket.servicelocation
            new TranslationSeedItem("entity.serviceticket.servicelocation", "en-US", "服务地点_us", "服务地点"),
            // entity.serviceticket.servicelocation
            new TranslationSeedItem("entity.serviceticket.servicelocation", "ja-JP", "服务地点_jp", "服务地点"),
            // entity.serviceticket.servicelocation
            new TranslationSeedItem("entity.serviceticket.servicelocation", "zh-CN", "服务地点", "服务地点"),
            // entity.serviceticket.servicelocation
            new TranslationSeedItem("entity.serviceticket.servicelocation", "zh-HK", "服务地点_hk", "服务地点"),

            // entity.serviceticket.assignedemployeeid
            new TranslationSeedItem("entity.serviceticket.assignedemployeeid", "en-US", "指派服务人员工ID_us", "指派服务人员工ID（序列化为string以避免Javascript精度问题）"),
            // entity.serviceticket.assignedemployeeid
            new TranslationSeedItem("entity.serviceticket.assignedemployeeid", "ja-JP", "指派服务人员工ID_jp", "指派服务人员工ID（序列化为string以避免Javascript精度问题）"),
            // entity.serviceticket.assignedemployeeid
            new TranslationSeedItem("entity.serviceticket.assignedemployeeid", "zh-CN", "指派服务人员工ID", "指派服务人员工ID（序列化为string以避免Javascript精度问题）"),
            // entity.serviceticket.assignedemployeeid
            new TranslationSeedItem("entity.serviceticket.assignedemployeeid", "zh-HK", "指派服务人员工ID_hk", "指派服务人员工ID（序列化为string以避免Javascript精度问题）"),

            // entity.serviceticket.assignedemployeename
            new TranslationSeedItem("entity.serviceticket.assignedemployeename", "en-US", "指派服务人员姓名_us", "指派服务人员姓名"),
            // entity.serviceticket.assignedemployeename
            new TranslationSeedItem("entity.serviceticket.assignedemployeename", "ja-JP", "指派服务人员姓名_jp", "指派服务人员姓名"),
            // entity.serviceticket.assignedemployeename
            new TranslationSeedItem("entity.serviceticket.assignedemployeename", "zh-CN", "指派服务人员姓名", "指派服务人员姓名"),
            // entity.serviceticket.assignedemployeename
            new TranslationSeedItem("entity.serviceticket.assignedemployeename", "zh-HK", "指派服务人员姓名_hk", "指派服务人员姓名"),

            // entity.serviceticket.scheduledstarttime
            new TranslationSeedItem("entity.serviceticket.scheduledstarttime", "en-US", "计划开始时间_us", "计划开始时间"),
            // entity.serviceticket.scheduledstarttime
            new TranslationSeedItem("entity.serviceticket.scheduledstarttime", "ja-JP", "计划开始时间_jp", "计划开始时间"),
            // entity.serviceticket.scheduledstarttime
            new TranslationSeedItem("entity.serviceticket.scheduledstarttime", "zh-CN", "计划开始时间", "计划开始时间"),
            // entity.serviceticket.scheduledstarttime
            new TranslationSeedItem("entity.serviceticket.scheduledstarttime", "zh-HK", "计划开始时间_hk", "计划开始时间"),

            // entity.serviceticket.scheduledendtime
            new TranslationSeedItem("entity.serviceticket.scheduledendtime", "en-US", "计划结束时间_us", "计划结束时间"),
            // entity.serviceticket.scheduledendtime
            new TranslationSeedItem("entity.serviceticket.scheduledendtime", "ja-JP", "计划结束时间_jp", "计划结束时间"),
            // entity.serviceticket.scheduledendtime
            new TranslationSeedItem("entity.serviceticket.scheduledendtime", "zh-CN", "计划结束时间", "计划结束时间"),
            // entity.serviceticket.scheduledendtime
            new TranslationSeedItem("entity.serviceticket.scheduledendtime", "zh-HK", "计划结束时间_hk", "计划结束时间"),

            // entity.serviceticket.actualstarttime
            new TranslationSeedItem("entity.serviceticket.actualstarttime", "en-US", "实际开始时间_us", "实际开始时间"),
            // entity.serviceticket.actualstarttime
            new TranslationSeedItem("entity.serviceticket.actualstarttime", "ja-JP", "实际开始时间_jp", "实际开始时间"),
            // entity.serviceticket.actualstarttime
            new TranslationSeedItem("entity.serviceticket.actualstarttime", "zh-CN", "实际开始时间", "实际开始时间"),
            // entity.serviceticket.actualstarttime
            new TranslationSeedItem("entity.serviceticket.actualstarttime", "zh-HK", "实际开始时间_hk", "实际开始时间"),

            // entity.serviceticket.actualendtime
            new TranslationSeedItem("entity.serviceticket.actualendtime", "en-US", "实际结束时间_us", "实际结束时间"),
            // entity.serviceticket.actualendtime
            new TranslationSeedItem("entity.serviceticket.actualendtime", "ja-JP", "实际结束时间_jp", "实际结束时间"),
            // entity.serviceticket.actualendtime
            new TranslationSeedItem("entity.serviceticket.actualendtime", "zh-CN", "实际结束时间", "实际结束时间"),
            // entity.serviceticket.actualendtime
            new TranslationSeedItem("entity.serviceticket.actualendtime", "zh-HK", "实际结束时间_hk", "实际结束时间"),

            // entity.serviceticket.acceptanceresult
            new TranslationSeedItem("entity.serviceticket.acceptanceresult", "en-US", "验收结果_us", "验收结果（0=不合格，1=合格，2=部分合格）"),
            // entity.serviceticket.acceptanceresult
            new TranslationSeedItem("entity.serviceticket.acceptanceresult", "ja-JP", "验收结果_jp", "验收结果（0=不合格，1=合格，2=部分合格）"),
            // entity.serviceticket.acceptanceresult
            new TranslationSeedItem("entity.serviceticket.acceptanceresult", "zh-CN", "验收结果", "验收结果（0=不合格，1=合格，2=部分合格）"),
            // entity.serviceticket.acceptanceresult
            new TranslationSeedItem("entity.serviceticket.acceptanceresult", "zh-HK", "验收结果_hk", "验收结果（0=不合格，1=合格，2=部分合格）"),

            // entity.serviceticket.acceptedby
            new TranslationSeedItem("entity.serviceticket.acceptedby", "en-US", "验收人_us", "验收人"),
            // entity.serviceticket.acceptedby
            new TranslationSeedItem("entity.serviceticket.acceptedby", "ja-JP", "验收人_jp", "验收人"),
            // entity.serviceticket.acceptedby
            new TranslationSeedItem("entity.serviceticket.acceptedby", "zh-CN", "验收人", "验收人"),
            // entity.serviceticket.acceptedby
            new TranslationSeedItem("entity.serviceticket.acceptedby", "zh-HK", "验收人_hk", "验收人"),

            // entity.serviceticket.acceptedat
            new TranslationSeedItem("entity.serviceticket.acceptedat", "en-US", "验收时间_us", "验收时间"),
            // entity.serviceticket.acceptedat
            new TranslationSeedItem("entity.serviceticket.acceptedat", "ja-JP", "验收时间_jp", "验收时间"),
            // entity.serviceticket.acceptedat
            new TranslationSeedItem("entity.serviceticket.acceptedat", "zh-CN", "验收时间", "验收时间"),
            // entity.serviceticket.acceptedat
            new TranslationSeedItem("entity.serviceticket.acceptedat", "zh-HK", "验收时间_hk", "验收时间"),

            // entity.serviceticket.sortorder
            new TranslationSeedItem("entity.serviceticket.sortorder", "en-US", "排序号_us", "排序号（越小越靠前）"),
            // entity.serviceticket.sortorder
            new TranslationSeedItem("entity.serviceticket.sortorder", "ja-JP", "排序号_jp", "排序号（越小越靠前）"),
            // entity.serviceticket.sortorder
            new TranslationSeedItem("entity.serviceticket.sortorder", "zh-CN", "排序号", "排序号（越小越靠前）"),
            // entity.serviceticket.sortorder
            new TranslationSeedItem("entity.serviceticket.sortorder", "zh-HK", "排序号_hk", "排序号（越小越靠前）"),

            // entity.serviceticket.servicerequest
            new TranslationSeedItem("entity.serviceticket.servicerequest", "en-US", "关联服务请求_us", "关联服务请求"),
            // entity.serviceticket.servicerequest
            new TranslationSeedItem("entity.serviceticket.servicerequest", "ja-JP", "关联服务请求_jp", "关联服务请求"),
            // entity.serviceticket.servicerequest
            new TranslationSeedItem("entity.serviceticket.servicerequest", "zh-CN", "关联服务请求", "关联服务请求"),
            // entity.serviceticket.servicerequest
            new TranslationSeedItem("entity.serviceticket.servicerequest", "zh-HK", "关联服务请求_hk", "关联服务请求"),

            // entity.serviceticket.serviceorder
            new TranslationSeedItem("entity.serviceticket.serviceorder", "en-US", "关联服务订单_us", "关联服务订单"),
            // entity.serviceticket.serviceorder
            new TranslationSeedItem("entity.serviceticket.serviceorder", "ja-JP", "关联服务订单_jp", "关联服务订单"),
            // entity.serviceticket.serviceorder
            new TranslationSeedItem("entity.serviceticket.serviceorder", "zh-CN", "关联服务订单", "关联服务订单"),
            // entity.serviceticket.serviceorder
            new TranslationSeedItem("entity.serviceticket.serviceorder", "zh-HK", "关联服务订单_hk", "关联服务订单"),

            // entity.serviceticket.servicecontract
            new TranslationSeedItem("entity.serviceticket.servicecontract", "en-US", "关联服务合同_us", "关联服务合同"),
            // entity.serviceticket.servicecontract
            new TranslationSeedItem("entity.serviceticket.servicecontract", "ja-JP", "关联服务合同_jp", "关联服务合同"),
            // entity.serviceticket.servicecontract
            new TranslationSeedItem("entity.serviceticket.servicecontract", "zh-CN", "关联服务合同", "关联服务合同"),
            // entity.serviceticket.servicecontract
            new TranslationSeedItem("entity.serviceticket.servicecontract", "zh-HK", "关联服务合同_hk", "关联服务合同"),
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
