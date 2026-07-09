// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.CustomerService
// 文件名称：TaktServiceRequestI18nSeedData.cs
// 创建时间：2026-07-09
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktServiceRequest 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktServiceRequest 实体国际化翻译种子（键前缀 entity.servicerequest.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktServiceRequestI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktServiceRequest 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 servicerequest 实体翻译...", tenantCode);

        foreach (var item in GetServiceRequestTranslations())
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

        TaktLogger.Information("TaktServiceRequest 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktServiceRequest 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.servicerequest._self / entity.servicerequest.{{field}}；ResourceGroup=CustomerService；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetServiceRequestTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.servicerequest._self
            new TranslationSeedItem("entity.servicerequest._self", "en-US", "Service Request Information_us", "实体名称"),
            // entity.servicerequest._self
            new TranslationSeedItem("entity.servicerequest._self", "ja-JP", "服务请求信息_jp", "实体名称"),
            // entity.servicerequest._self
            new TranslationSeedItem("entity.servicerequest._self", "zh-CN", "服务请求信息", "实体名称"),
            // entity.servicerequest._self
            new TranslationSeedItem("entity.servicerequest._self", "zh-HK", "服务请求信息_hk", "实体名称"),

            // entity.servicerequest.plantcode
            new TranslationSeedItem("entity.servicerequest.plantcode", "en-US", "工厂代码_us", "工厂代码"),
            // entity.servicerequest.plantcode
            new TranslationSeedItem("entity.servicerequest.plantcode", "ja-JP", "工厂代码_jp", "工厂代码"),
            // entity.servicerequest.plantcode
            new TranslationSeedItem("entity.servicerequest.plantcode", "zh-CN", "工厂代码", "工厂代码"),
            // entity.servicerequest.plantcode
            new TranslationSeedItem("entity.servicerequest.plantcode", "zh-HK", "工厂代码_hk", "工厂代码"),

            // entity.servicerequest.code
            new TranslationSeedItem("entity.servicerequest.code", "en-US", "服务请求单号_us", "服务请求单号（组合唯一索引）"),
            // entity.servicerequest.code
            new TranslationSeedItem("entity.servicerequest.code", "ja-JP", "服务请求单号_jp", "服务请求单号（组合唯一索引）"),
            // entity.servicerequest.code
            new TranslationSeedItem("entity.servicerequest.code", "zh-CN", "服务请求单号", "服务请求单号（组合唯一索引）"),
            // entity.servicerequest.code
            new TranslationSeedItem("entity.servicerequest.code", "zh-HK", "服务请求单号_hk", "服务请求单号（组合唯一索引）"),

            // entity.servicerequest.clientid
            new TranslationSeedItem("entity.servicerequest.clientid", "en-US", "客户端ID_us", "客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）"),
            // entity.servicerequest.clientid
            new TranslationSeedItem("entity.servicerequest.clientid", "ja-JP", "客户端ID_jp", "客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）"),
            // entity.servicerequest.clientid
            new TranslationSeedItem("entity.servicerequest.clientid", "zh-CN", "客户端ID", "客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）"),
            // entity.servicerequest.clientid
            new TranslationSeedItem("entity.servicerequest.clientid", "zh-HK", "客户端ID_hk", "客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）"),

            // entity.servicerequest.clientcode
            new TranslationSeedItem("entity.servicerequest.clientcode", "en-US", "客户端编码_us", "客户端编码（冗余字段，便于查询）"),
            // entity.servicerequest.clientcode
            new TranslationSeedItem("entity.servicerequest.clientcode", "ja-JP", "客户端编码_jp", "客户端编码（冗余字段，便于查询）"),
            // entity.servicerequest.clientcode
            new TranslationSeedItem("entity.servicerequest.clientcode", "zh-CN", "客户端编码", "客户端编码（冗余字段，便于查询）"),
            // entity.servicerequest.clientcode
            new TranslationSeedItem("entity.servicerequest.clientcode", "zh-HK", "客户端编码_hk", "客户端编码（冗余字段，便于查询）"),

            // entity.servicerequest.clientname
            new TranslationSeedItem("entity.servicerequest.clientname", "en-US", "客户端名称_us", "客户端名称（冗余字段，便于查询）"),
            // entity.servicerequest.clientname
            new TranslationSeedItem("entity.servicerequest.clientname", "ja-JP", "客户端名称_jp", "客户端名称（冗余字段，便于查询）"),
            // entity.servicerequest.clientname
            new TranslationSeedItem("entity.servicerequest.clientname", "zh-CN", "客户端名称", "客户端名称（冗余字段，便于查询）"),
            // entity.servicerequest.clientname
            new TranslationSeedItem("entity.servicerequest.clientname", "zh-HK", "客户端名称_hk", "客户端名称（冗余字段，便于查询）"),

            // entity.servicerequest.servicecontractid
            new TranslationSeedItem("entity.servicerequest.servicecontractid", "en-US", "关联服务合同ID_us", "关联服务合同ID（序列化为string以避免Javascript精度问题）"),
            // entity.servicerequest.servicecontractid
            new TranslationSeedItem("entity.servicerequest.servicecontractid", "ja-JP", "关联服务合同ID_jp", "关联服务合同ID（序列化为string以避免Javascript精度问题）"),
            // entity.servicerequest.servicecontractid
            new TranslationSeedItem("entity.servicerequest.servicecontractid", "zh-CN", "关联服务合同ID", "关联服务合同ID（序列化为string以避免Javascript精度问题）"),
            // entity.servicerequest.servicecontractid
            new TranslationSeedItem("entity.servicerequest.servicecontractid", "zh-HK", "关联服务合同ID_hk", "关联服务合同ID（序列化为string以避免Javascript精度问题）"),

            // entity.servicerequest.servicecontractcode
            new TranslationSeedItem("entity.servicerequest.servicecontractcode", "en-US", "关联服务合同编码_us", "关联服务合同编码（冗余字段，便于查询）"),
            // entity.servicerequest.servicecontractcode
            new TranslationSeedItem("entity.servicerequest.servicecontractcode", "ja-JP", "关联服务合同编码_jp", "关联服务合同编码（冗余字段，便于查询）"),
            // entity.servicerequest.servicecontractcode
            new TranslationSeedItem("entity.servicerequest.servicecontractcode", "zh-CN", "关联服务合同编码", "关联服务合同编码（冗余字段，便于查询）"),
            // entity.servicerequest.servicecontractcode
            new TranslationSeedItem("entity.servicerequest.servicecontractcode", "zh-HK", "关联服务合同编码_hk", "关联服务合同编码（冗余字段，便于查询）"),

            // entity.servicerequest.requestdate
            new TranslationSeedItem("entity.servicerequest.requestdate", "en-US", "请求日期_us", "请求日期"),
            // entity.servicerequest.requestdate
            new TranslationSeedItem("entity.servicerequest.requestdate", "ja-JP", "请求日期_jp", "请求日期"),
            // entity.servicerequest.requestdate
            new TranslationSeedItem("entity.servicerequest.requestdate", "zh-CN", "请求日期", "请求日期"),
            // entity.servicerequest.requestdate
            new TranslationSeedItem("entity.servicerequest.requestdate", "zh-HK", "请求日期_hk", "请求日期"),

            // entity.servicerequest.expectedservicedate
            new TranslationSeedItem("entity.servicerequest.expectedservicedate", "en-US", "期望服务日期_us", "期望服务日期"),
            // entity.servicerequest.expectedservicedate
            new TranslationSeedItem("entity.servicerequest.expectedservicedate", "ja-JP", "期望服务日期_jp", "期望服务日期"),
            // entity.servicerequest.expectedservicedate
            new TranslationSeedItem("entity.servicerequest.expectedservicedate", "zh-CN", "期望服务日期", "期望服务日期"),
            // entity.servicerequest.expectedservicedate
            new TranslationSeedItem("entity.servicerequest.expectedservicedate", "zh-HK", "期望服务日期_hk", "期望服务日期"),

            // entity.servicerequest.requesttype
            new TranslationSeedItem("entity.servicerequest.requesttype", "en-US", "请求类型_us", "请求类型（0=咨询，1=报修，2=投诉，3=安装，4=巡检，5=其他）"),
            // entity.servicerequest.requesttype
            new TranslationSeedItem("entity.servicerequest.requesttype", "ja-JP", "请求类型_jp", "请求类型（0=咨询，1=报修，2=投诉，3=安装，4=巡检，5=其他）"),
            // entity.servicerequest.requesttype
            new TranslationSeedItem("entity.servicerequest.requesttype", "zh-CN", "请求类型", "请求类型（0=咨询，1=报修，2=投诉，3=安装，4=巡检，5=其他）"),
            // entity.servicerequest.requesttype
            new TranslationSeedItem("entity.servicerequest.requesttype", "zh-HK", "请求类型_hk", "请求类型（0=咨询，1=报修，2=投诉，3=安装，4=巡检，5=其他）"),

            // entity.servicerequest.sourcechannel
            new TranslationSeedItem("entity.servicerequest.sourcechannel", "en-US", "请求来源_us", "请求来源（0=电话，1=邮件，2=门户，3=现场，4=其他）"),
            // entity.servicerequest.sourcechannel
            new TranslationSeedItem("entity.servicerequest.sourcechannel", "ja-JP", "请求来源_jp", "请求来源（0=电话，1=邮件，2=门户，3=现场，4=其他）"),
            // entity.servicerequest.sourcechannel
            new TranslationSeedItem("entity.servicerequest.sourcechannel", "zh-CN", "请求来源", "请求来源（0=电话，1=邮件，2=门户，3=现场，4=其他）"),
            // entity.servicerequest.sourcechannel
            new TranslationSeedItem("entity.servicerequest.sourcechannel", "zh-HK", "请求来源_hk", "请求来源（0=电话，1=邮件，2=门户，3=现场，4=其他）"),

            // entity.servicerequest.priority
            new TranslationSeedItem("entity.servicerequest.priority", "en-US", "优先级_us", "优先级（字典 sys_priority_level_category）"),
            // entity.servicerequest.priority
            new TranslationSeedItem("entity.servicerequest.priority", "ja-JP", "优先级_jp", "优先级（字典 sys_priority_level_category）"),
            // entity.servicerequest.priority
            new TranslationSeedItem("entity.servicerequest.priority", "zh-CN", "优先级", "优先级（字典 sys_priority_level_category）"),
            // entity.servicerequest.priority
            new TranslationSeedItem("entity.servicerequest.priority", "zh-HK", "优先级_hk", "优先级（字典 sys_priority_level_category）"),

            // entity.servicerequest.requeststatus
            new TranslationSeedItem("entity.servicerequest.requeststatus", "en-US", "请求状态_us", "请求状态（0=草稿，1=已提交，2=处理中，3=已完成，4=已关闭，5=已取消）"),
            // entity.servicerequest.requeststatus
            new TranslationSeedItem("entity.servicerequest.requeststatus", "ja-JP", "请求状态_jp", "请求状态（0=草稿，1=已提交，2=处理中，3=已完成，4=已关闭，5=已取消）"),
            // entity.servicerequest.requeststatus
            new TranslationSeedItem("entity.servicerequest.requeststatus", "zh-CN", "请求状态", "请求状态（0=草稿，1=已提交，2=处理中，3=已完成，4=已关闭，5=已取消）"),
            // entity.servicerequest.requeststatus
            new TranslationSeedItem("entity.servicerequest.requeststatus", "zh-HK", "请求状态_hk", "请求状态（0=草稿，1=已提交，2=处理中，3=已完成，4=已关闭，5=已取消）"),

            // entity.servicerequest.requestsubject
            new TranslationSeedItem("entity.servicerequest.requestsubject", "en-US", "请求主题_us", "请求主题"),
            // entity.servicerequest.requestsubject
            new TranslationSeedItem("entity.servicerequest.requestsubject", "ja-JP", "请求主题_jp", "请求主题"),
            // entity.servicerequest.requestsubject
            new TranslationSeedItem("entity.servicerequest.requestsubject", "zh-CN", "请求主题", "请求主题"),
            // entity.servicerequest.requestsubject
            new TranslationSeedItem("entity.servicerequest.requestsubject", "zh-HK", "请求主题_hk", "请求主题"),

            // entity.servicerequest.requestdescription
            new TranslationSeedItem("entity.servicerequest.requestdescription", "en-US", "请求描述_us", "请求描述"),
            // entity.servicerequest.requestdescription
            new TranslationSeedItem("entity.servicerequest.requestdescription", "ja-JP", "请求描述_jp", "请求描述"),
            // entity.servicerequest.requestdescription
            new TranslationSeedItem("entity.servicerequest.requestdescription", "zh-CN", "请求描述", "请求描述"),
            // entity.servicerequest.requestdescription
            new TranslationSeedItem("entity.servicerequest.requestdescription", "zh-HK", "请求描述_hk", "请求描述"),

            // entity.servicerequest.contactperson
            new TranslationSeedItem("entity.servicerequest.contactperson", "en-US", "联系人_us", "联系人"),
            // entity.servicerequest.contactperson
            new TranslationSeedItem("entity.servicerequest.contactperson", "ja-JP", "联系人_jp", "联系人"),
            // entity.servicerequest.contactperson
            new TranslationSeedItem("entity.servicerequest.contactperson", "zh-CN", "联系人", "联系人"),
            // entity.servicerequest.contactperson
            new TranslationSeedItem("entity.servicerequest.contactperson", "zh-HK", "联系人_hk", "联系人"),

            // entity.servicerequest.contactphone
            new TranslationSeedItem("entity.servicerequest.contactphone", "en-US", "联系电话_us", "联系电话"),
            // entity.servicerequest.contactphone
            new TranslationSeedItem("entity.servicerequest.contactphone", "ja-JP", "联系电话_jp", "联系电话"),
            // entity.servicerequest.contactphone
            new TranslationSeedItem("entity.servicerequest.contactphone", "zh-CN", "联系电话", "联系电话"),
            // entity.servicerequest.contactphone
            new TranslationSeedItem("entity.servicerequest.contactphone", "zh-HK", "联系电话_hk", "联系电话"),

            // entity.servicerequest.contactemail
            new TranslationSeedItem("entity.servicerequest.contactemail", "en-US", "联系邮箱_us", "联系邮箱"),
            // entity.servicerequest.contactemail
            new TranslationSeedItem("entity.servicerequest.contactemail", "ja-JP", "联系邮箱_jp", "联系邮箱"),
            // entity.servicerequest.contactemail
            new TranslationSeedItem("entity.servicerequest.contactemail", "zh-CN", "联系邮箱", "联系邮箱"),
            // entity.servicerequest.contactemail
            new TranslationSeedItem("entity.servicerequest.contactemail", "zh-HK", "联系邮箱_hk", "联系邮箱"),

            // entity.servicerequest.serviceaddress
            new TranslationSeedItem("entity.servicerequest.serviceaddress", "en-US", "服务地址_us", "服务地址"),
            // entity.servicerequest.serviceaddress
            new TranslationSeedItem("entity.servicerequest.serviceaddress", "ja-JP", "服务地址_jp", "服务地址"),
            // entity.servicerequest.serviceaddress
            new TranslationSeedItem("entity.servicerequest.serviceaddress", "zh-CN", "服务地址", "服务地址"),
            // entity.servicerequest.serviceaddress
            new TranslationSeedItem("entity.servicerequest.serviceaddress", "zh-HK", "服务地址_hk", "服务地址"),

            // entity.servicerequest.assignedemployeeid
            new TranslationSeedItem("entity.servicerequest.assignedemployeeid", "en-US", "受理人员工ID_us", "受理人员工ID（序列化为string以避免Javascript精度问题）"),
            // entity.servicerequest.assignedemployeeid
            new TranslationSeedItem("entity.servicerequest.assignedemployeeid", "ja-JP", "受理人员工ID_jp", "受理人员工ID（序列化为string以避免Javascript精度问题）"),
            // entity.servicerequest.assignedemployeeid
            new TranslationSeedItem("entity.servicerequest.assignedemployeeid", "zh-CN", "受理人员工ID", "受理人员工ID（序列化为string以避免Javascript精度问题）"),
            // entity.servicerequest.assignedemployeeid
            new TranslationSeedItem("entity.servicerequest.assignedemployeeid", "zh-HK", "受理人员工ID_hk", "受理人员工ID（序列化为string以避免Javascript精度问题）"),

            // entity.servicerequest.assignedemployeename
            new TranslationSeedItem("entity.servicerequest.assignedemployeename", "en-US", "受理人姓名_us", "受理人姓名"),
            // entity.servicerequest.assignedemployeename
            new TranslationSeedItem("entity.servicerequest.assignedemployeename", "ja-JP", "受理人姓名_jp", "受理人姓名"),
            // entity.servicerequest.assignedemployeename
            new TranslationSeedItem("entity.servicerequest.assignedemployeename", "zh-CN", "受理人姓名", "受理人姓名"),
            // entity.servicerequest.assignedemployeename
            new TranslationSeedItem("entity.servicerequest.assignedemployeename", "zh-HK", "受理人姓名_hk", "受理人姓名"),

            // entity.servicerequest.assignedat
            new TranslationSeedItem("entity.servicerequest.assignedat", "en-US", "受理时间_us", "受理时间"),
            // entity.servicerequest.assignedat
            new TranslationSeedItem("entity.servicerequest.assignedat", "ja-JP", "受理时间_jp", "受理时间"),
            // entity.servicerequest.assignedat
            new TranslationSeedItem("entity.servicerequest.assignedat", "zh-CN", "受理时间", "受理时间"),
            // entity.servicerequest.assignedat
            new TranslationSeedItem("entity.servicerequest.assignedat", "zh-HK", "受理时间_hk", "受理时间"),

            // entity.servicerequest.closedat
            new TranslationSeedItem("entity.servicerequest.closedat", "en-US", "关闭时间_us", "关闭时间"),
            // entity.servicerequest.closedat
            new TranslationSeedItem("entity.servicerequest.closedat", "ja-JP", "关闭时间_jp", "关闭时间"),
            // entity.servicerequest.closedat
            new TranslationSeedItem("entity.servicerequest.closedat", "zh-CN", "关闭时间", "关闭时间"),
            // entity.servicerequest.closedat
            new TranslationSeedItem("entity.servicerequest.closedat", "zh-HK", "关闭时间_hk", "关闭时间"),

            // entity.servicerequest.sortorder
            new TranslationSeedItem("entity.servicerequest.sortorder", "en-US", "排序号_us", "排序号（越小越靠前）"),
            // entity.servicerequest.sortorder
            new TranslationSeedItem("entity.servicerequest.sortorder", "ja-JP", "排序号_jp", "排序号（越小越靠前）"),
            // entity.servicerequest.sortorder
            new TranslationSeedItem("entity.servicerequest.sortorder", "zh-CN", "排序号", "排序号（越小越靠前）"),
            // entity.servicerequest.sortorder
            new TranslationSeedItem("entity.servicerequest.sortorder", "zh-HK", "排序号_hk", "排序号（越小越靠前）"),

            // entity.servicerequest.servicecontract
            new TranslationSeedItem("entity.servicerequest.servicecontract", "en-US", "关联服务合同_us", "关联服务合同"),
            // entity.servicerequest.servicecontract
            new TranslationSeedItem("entity.servicerequest.servicecontract", "ja-JP", "关联服务合同_jp", "关联服务合同"),
            // entity.servicerequest.servicecontract
            new TranslationSeedItem("entity.servicerequest.servicecontract", "zh-CN", "关联服务合同", "关联服务合同"),
            // entity.servicerequest.servicecontract
            new TranslationSeedItem("entity.servicerequest.servicecontract", "zh-HK", "关联服务合同_hk", "关联服务合同"),

            // entity.servicerequest.serviceorders
            new TranslationSeedItem("entity.servicerequest.serviceorders", "en-US", "关联服务订单列表_us", "关联服务订单列表（外键在子表 TaktServiceOrder.ServiceRequestId）"),
            // entity.servicerequest.serviceorders
            new TranslationSeedItem("entity.servicerequest.serviceorders", "ja-JP", "关联服务订单列表_jp", "关联服务订单列表（外键在子表 TaktServiceOrder.ServiceRequestId）"),
            // entity.servicerequest.serviceorders
            new TranslationSeedItem("entity.servicerequest.serviceorders", "zh-CN", "关联服务订单列表", "关联服务订单列表（外键在子表 TaktServiceOrder.ServiceRequestId）"),
            // entity.servicerequest.serviceorders
            new TranslationSeedItem("entity.servicerequest.serviceorders", "zh-HK", "关联服务订单列表_hk", "关联服务订单列表（外键在子表 TaktServiceOrder.ServiceRequestId）"),

            // entity.servicerequest.tickets
            new TranslationSeedItem("entity.servicerequest.tickets", "en-US", "服务工单列表_us", "服务工单列表（外键在子表 TaktServiceTicket.ServiceRequestId）"),
            // entity.servicerequest.tickets
            new TranslationSeedItem("entity.servicerequest.tickets", "ja-JP", "服务工单列表_jp", "服务工单列表（外键在子表 TaktServiceTicket.ServiceRequestId）"),
            // entity.servicerequest.tickets
            new TranslationSeedItem("entity.servicerequest.tickets", "zh-CN", "服务工单列表", "服务工单列表（外键在子表 TaktServiceTicket.ServiceRequestId）"),
            // entity.servicerequest.tickets
            new TranslationSeedItem("entity.servicerequest.tickets", "zh-HK", "服务工单列表_hk", "服务工单列表（外键在子表 TaktServiceTicket.ServiceRequestId）"),
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
