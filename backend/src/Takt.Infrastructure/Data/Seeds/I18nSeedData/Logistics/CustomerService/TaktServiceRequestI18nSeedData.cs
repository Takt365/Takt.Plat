// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.CustomerService
// 文件名称：TaktServiceRequestI18nSeedData.cs
// 创建时间：2026-06-08
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
using Takt.Shared.Enums;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.CustomerService;

/// <summary>
/// TaktServiceRequest 实体国际化翻译种子（键前缀 entity.serviceRequest.*）
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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 serviceRequest 实体翻译...", tenantCode);

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
    /// I18nKey：entity.serviceRequest._self / entity.serviceRequest.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetServiceRequestTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.serviceRequest._self
            new TranslationSeedItem("entity.serviceRequest._self", "en-US", "Service Request Information", "实体名称"),
            // entity.serviceRequest._self
            new TranslationSeedItem("entity.serviceRequest._self", "ja-JP", "服务请求信息", "实体名称"),
            // entity.serviceRequest._self
            new TranslationSeedItem("entity.serviceRequest._self", "zh-CN", "服务请求信息", "实体名称"),
            // entity.serviceRequest._self
            new TranslationSeedItem("entity.serviceRequest._self", "zh-HK", "服务请求信息", "实体名称"),

            // entity.serviceRequest.plantcode
            new TranslationSeedItem("entity.serviceRequest.plantcode", "en-US", "工厂代码", "工厂代码"),
            // entity.serviceRequest.plantcode
            new TranslationSeedItem("entity.serviceRequest.plantcode", "ja-JP", "工厂代码", "工厂代码"),
            // entity.serviceRequest.plantcode
            new TranslationSeedItem("entity.serviceRequest.plantcode", "zh-CN", "工厂代码", "工厂代码"),
            // entity.serviceRequest.plantcode
            new TranslationSeedItem("entity.serviceRequest.plantcode", "zh-HK", "工厂代码", "工厂代码"),

            // entity.serviceRequest.code
            new TranslationSeedItem("entity.serviceRequest.code", "en-US", "服务请求单号", "服务请求单号（组合唯一索引）"),
            // entity.serviceRequest.code
            new TranslationSeedItem("entity.serviceRequest.code", "ja-JP", "服务请求单号", "服务请求单号（组合唯一索引）"),
            // entity.serviceRequest.code
            new TranslationSeedItem("entity.serviceRequest.code", "zh-CN", "服务请求单号", "服务请求单号（组合唯一索引）"),
            // entity.serviceRequest.code
            new TranslationSeedItem("entity.serviceRequest.code", "zh-HK", "服务请求单号", "服务请求单号（组合唯一索引）"),

            // entity.serviceRequest.clientid
            new TranslationSeedItem("entity.serviceRequest.clientid", "en-US", "客户端ID", "客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）"),
            // entity.serviceRequest.clientid
            new TranslationSeedItem("entity.serviceRequest.clientid", "ja-JP", "客户端ID", "客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）"),
            // entity.serviceRequest.clientid
            new TranslationSeedItem("entity.serviceRequest.clientid", "zh-CN", "客户端ID", "客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）"),
            // entity.serviceRequest.clientid
            new TranslationSeedItem("entity.serviceRequest.clientid", "zh-HK", "客户端ID", "客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）"),

            // entity.serviceRequest.clientcode
            new TranslationSeedItem("entity.serviceRequest.clientcode", "en-US", "客户端编码", "客户端编码（冗余字段，便于查询）"),
            // entity.serviceRequest.clientcode
            new TranslationSeedItem("entity.serviceRequest.clientcode", "ja-JP", "客户端编码", "客户端编码（冗余字段，便于查询）"),
            // entity.serviceRequest.clientcode
            new TranslationSeedItem("entity.serviceRequest.clientcode", "zh-CN", "客户端编码", "客户端编码（冗余字段，便于查询）"),
            // entity.serviceRequest.clientcode
            new TranslationSeedItem("entity.serviceRequest.clientcode", "zh-HK", "客户端编码", "客户端编码（冗余字段，便于查询）"),

            // entity.serviceRequest.clientname
            new TranslationSeedItem("entity.serviceRequest.clientname", "en-US", "客户端名称", "客户端名称（冗余字段，便于查询）"),
            // entity.serviceRequest.clientname
            new TranslationSeedItem("entity.serviceRequest.clientname", "ja-JP", "客户端名称", "客户端名称（冗余字段，便于查询）"),
            // entity.serviceRequest.clientname
            new TranslationSeedItem("entity.serviceRequest.clientname", "zh-CN", "客户端名称", "客户端名称（冗余字段，便于查询）"),
            // entity.serviceRequest.clientname
            new TranslationSeedItem("entity.serviceRequest.clientname", "zh-HK", "客户端名称", "客户端名称（冗余字段，便于查询）"),

            // entity.serviceRequest.servicecontractid
            new TranslationSeedItem("entity.serviceRequest.servicecontractid", "en-US", "关联服务合同ID", "关联服务合同ID（序列化为string以避免Javascript精度问题）"),
            // entity.serviceRequest.servicecontractid
            new TranslationSeedItem("entity.serviceRequest.servicecontractid", "ja-JP", "关联服务合同ID", "关联服务合同ID（序列化为string以避免Javascript精度问题）"),
            // entity.serviceRequest.servicecontractid
            new TranslationSeedItem("entity.serviceRequest.servicecontractid", "zh-CN", "关联服务合同ID", "关联服务合同ID（序列化为string以避免Javascript精度问题）"),
            // entity.serviceRequest.servicecontractid
            new TranslationSeedItem("entity.serviceRequest.servicecontractid", "zh-HK", "关联服务合同ID", "关联服务合同ID（序列化为string以避免Javascript精度问题）"),

            // entity.serviceRequest.servicecontractcode
            new TranslationSeedItem("entity.serviceRequest.servicecontractcode", "en-US", "关联服务合同编码", "关联服务合同编码（冗余字段，便于查询）"),
            // entity.serviceRequest.servicecontractcode
            new TranslationSeedItem("entity.serviceRequest.servicecontractcode", "ja-JP", "关联服务合同编码", "关联服务合同编码（冗余字段，便于查询）"),
            // entity.serviceRequest.servicecontractcode
            new TranslationSeedItem("entity.serviceRequest.servicecontractcode", "zh-CN", "关联服务合同编码", "关联服务合同编码（冗余字段，便于查询）"),
            // entity.serviceRequest.servicecontractcode
            new TranslationSeedItem("entity.serviceRequest.servicecontractcode", "zh-HK", "关联服务合同编码", "关联服务合同编码（冗余字段，便于查询）"),

            // entity.serviceRequest.requestdate
            new TranslationSeedItem("entity.serviceRequest.requestdate", "en-US", "请求日期", "请求日期"),
            // entity.serviceRequest.requestdate
            new TranslationSeedItem("entity.serviceRequest.requestdate", "ja-JP", "请求日期", "请求日期"),
            // entity.serviceRequest.requestdate
            new TranslationSeedItem("entity.serviceRequest.requestdate", "zh-CN", "请求日期", "请求日期"),
            // entity.serviceRequest.requestdate
            new TranslationSeedItem("entity.serviceRequest.requestdate", "zh-HK", "请求日期", "请求日期"),

            // entity.serviceRequest.expectedservicedate
            new TranslationSeedItem("entity.serviceRequest.expectedservicedate", "en-US", "期望服务日期", "期望服务日期"),
            // entity.serviceRequest.expectedservicedate
            new TranslationSeedItem("entity.serviceRequest.expectedservicedate", "ja-JP", "期望服务日期", "期望服务日期"),
            // entity.serviceRequest.expectedservicedate
            new TranslationSeedItem("entity.serviceRequest.expectedservicedate", "zh-CN", "期望服务日期", "期望服务日期"),
            // entity.serviceRequest.expectedservicedate
            new TranslationSeedItem("entity.serviceRequest.expectedservicedate", "zh-HK", "期望服务日期", "期望服务日期"),

            // entity.serviceRequest.requesttype
            new TranslationSeedItem("entity.serviceRequest.requesttype", "en-US", "请求类型", "请求类型（0=咨询，1=报修，2=投诉，3=安装，4=巡检，5=其他）"),
            // entity.serviceRequest.requesttype
            new TranslationSeedItem("entity.serviceRequest.requesttype", "ja-JP", "请求类型", "请求类型（0=咨询，1=报修，2=投诉，3=安装，4=巡检，5=其他）"),
            // entity.serviceRequest.requesttype
            new TranslationSeedItem("entity.serviceRequest.requesttype", "zh-CN", "请求类型", "请求类型（0=咨询，1=报修，2=投诉，3=安装，4=巡检，5=其他）"),
            // entity.serviceRequest.requesttype
            new TranslationSeedItem("entity.serviceRequest.requesttype", "zh-HK", "请求类型", "请求类型（0=咨询，1=报修，2=投诉，3=安装，4=巡检，5=其他）"),

            // entity.serviceRequest.sourcechannel
            new TranslationSeedItem("entity.serviceRequest.sourcechannel", "en-US", "请求来源", "请求来源（0=电话，1=邮件，2=门户，3=现场，4=其他）"),
            // entity.serviceRequest.sourcechannel
            new TranslationSeedItem("entity.serviceRequest.sourcechannel", "ja-JP", "请求来源", "请求来源（0=电话，1=邮件，2=门户，3=现场，4=其他）"),
            // entity.serviceRequest.sourcechannel
            new TranslationSeedItem("entity.serviceRequest.sourcechannel", "zh-CN", "请求来源", "请求来源（0=电话，1=邮件，2=门户，3=现场，4=其他）"),
            // entity.serviceRequest.sourcechannel
            new TranslationSeedItem("entity.serviceRequest.sourcechannel", "zh-HK", "请求来源", "请求来源（0=电话，1=邮件，2=门户，3=现场，4=其他）"),

            // entity.serviceRequest.priority
            new TranslationSeedItem("entity.serviceRequest.priority", "en-US", "优先级", "优先级（0=低，1=中，2=高，3=紧急）"),
            // entity.serviceRequest.priority
            new TranslationSeedItem("entity.serviceRequest.priority", "ja-JP", "优先级", "优先级（0=低，1=中，2=高，3=紧急）"),
            // entity.serviceRequest.priority
            new TranslationSeedItem("entity.serviceRequest.priority", "zh-CN", "优先级", "优先级（0=低，1=中，2=高，3=紧急）"),
            // entity.serviceRequest.priority
            new TranslationSeedItem("entity.serviceRequest.priority", "zh-HK", "优先级", "优先级（0=低，1=中，2=高，3=紧急）"),

            // entity.serviceRequest.requeststatus
            new TranslationSeedItem("entity.serviceRequest.requeststatus", "en-US", "请求状态", "请求状态（0=草稿，1=已提交，2=处理中，3=已完成，4=已关闭，5=已取消）"),
            // entity.serviceRequest.requeststatus
            new TranslationSeedItem("entity.serviceRequest.requeststatus", "ja-JP", "请求状态", "请求状态（0=草稿，1=已提交，2=处理中，3=已完成，4=已关闭，5=已取消）"),
            // entity.serviceRequest.requeststatus
            new TranslationSeedItem("entity.serviceRequest.requeststatus", "zh-CN", "请求状态", "请求状态（0=草稿，1=已提交，2=处理中，3=已完成，4=已关闭，5=已取消）"),
            // entity.serviceRequest.requeststatus
            new TranslationSeedItem("entity.serviceRequest.requeststatus", "zh-HK", "请求状态", "请求状态（0=草稿，1=已提交，2=处理中，3=已完成，4=已关闭，5=已取消）"),

            // entity.serviceRequest.requestsubject
            new TranslationSeedItem("entity.serviceRequest.requestsubject", "en-US", "请求主题", "请求主题"),
            // entity.serviceRequest.requestsubject
            new TranslationSeedItem("entity.serviceRequest.requestsubject", "ja-JP", "请求主题", "请求主题"),
            // entity.serviceRequest.requestsubject
            new TranslationSeedItem("entity.serviceRequest.requestsubject", "zh-CN", "请求主题", "请求主题"),
            // entity.serviceRequest.requestsubject
            new TranslationSeedItem("entity.serviceRequest.requestsubject", "zh-HK", "请求主题", "请求主题"),

            // entity.serviceRequest.requestdescription
            new TranslationSeedItem("entity.serviceRequest.requestdescription", "en-US", "请求描述", "请求描述"),
            // entity.serviceRequest.requestdescription
            new TranslationSeedItem("entity.serviceRequest.requestdescription", "ja-JP", "请求描述", "请求描述"),
            // entity.serviceRequest.requestdescription
            new TranslationSeedItem("entity.serviceRequest.requestdescription", "zh-CN", "请求描述", "请求描述"),
            // entity.serviceRequest.requestdescription
            new TranslationSeedItem("entity.serviceRequest.requestdescription", "zh-HK", "请求描述", "请求描述"),

            // entity.serviceRequest.contactperson
            new TranslationSeedItem("entity.serviceRequest.contactperson", "en-US", "联系人", "联系人"),
            // entity.serviceRequest.contactperson
            new TranslationSeedItem("entity.serviceRequest.contactperson", "ja-JP", "联系人", "联系人"),
            // entity.serviceRequest.contactperson
            new TranslationSeedItem("entity.serviceRequest.contactperson", "zh-CN", "联系人", "联系人"),
            // entity.serviceRequest.contactperson
            new TranslationSeedItem("entity.serviceRequest.contactperson", "zh-HK", "联系人", "联系人"),

            // entity.serviceRequest.contactphone
            new TranslationSeedItem("entity.serviceRequest.contactphone", "en-US", "联系电话", "联系电话"),
            // entity.serviceRequest.contactphone
            new TranslationSeedItem("entity.serviceRequest.contactphone", "ja-JP", "联系电话", "联系电话"),
            // entity.serviceRequest.contactphone
            new TranslationSeedItem("entity.serviceRequest.contactphone", "zh-CN", "联系电话", "联系电话"),
            // entity.serviceRequest.contactphone
            new TranslationSeedItem("entity.serviceRequest.contactphone", "zh-HK", "联系电话", "联系电话"),

            // entity.serviceRequest.contactemail
            new TranslationSeedItem("entity.serviceRequest.contactemail", "en-US", "联系邮箱", "联系邮箱"),
            // entity.serviceRequest.contactemail
            new TranslationSeedItem("entity.serviceRequest.contactemail", "ja-JP", "联系邮箱", "联系邮箱"),
            // entity.serviceRequest.contactemail
            new TranslationSeedItem("entity.serviceRequest.contactemail", "zh-CN", "联系邮箱", "联系邮箱"),
            // entity.serviceRequest.contactemail
            new TranslationSeedItem("entity.serviceRequest.contactemail", "zh-HK", "联系邮箱", "联系邮箱"),

            // entity.serviceRequest.serviceaddress
            new TranslationSeedItem("entity.serviceRequest.serviceaddress", "en-US", "服务地址", "服务地址"),
            // entity.serviceRequest.serviceaddress
            new TranslationSeedItem("entity.serviceRequest.serviceaddress", "ja-JP", "服务地址", "服务地址"),
            // entity.serviceRequest.serviceaddress
            new TranslationSeedItem("entity.serviceRequest.serviceaddress", "zh-CN", "服务地址", "服务地址"),
            // entity.serviceRequest.serviceaddress
            new TranslationSeedItem("entity.serviceRequest.serviceaddress", "zh-HK", "服务地址", "服务地址"),

            // entity.serviceRequest.assignedemployeeid
            new TranslationSeedItem("entity.serviceRequest.assignedemployeeid", "en-US", "受理人员工ID", "受理人员工ID（序列化为string以避免Javascript精度问题）"),
            // entity.serviceRequest.assignedemployeeid
            new TranslationSeedItem("entity.serviceRequest.assignedemployeeid", "ja-JP", "受理人员工ID", "受理人员工ID（序列化为string以避免Javascript精度问题）"),
            // entity.serviceRequest.assignedemployeeid
            new TranslationSeedItem("entity.serviceRequest.assignedemployeeid", "zh-CN", "受理人员工ID", "受理人员工ID（序列化为string以避免Javascript精度问题）"),
            // entity.serviceRequest.assignedemployeeid
            new TranslationSeedItem("entity.serviceRequest.assignedemployeeid", "zh-HK", "受理人员工ID", "受理人员工ID（序列化为string以避免Javascript精度问题）"),

            // entity.serviceRequest.assignedemployeename
            new TranslationSeedItem("entity.serviceRequest.assignedemployeename", "en-US", "受理人姓名", "受理人姓名"),
            // entity.serviceRequest.assignedemployeename
            new TranslationSeedItem("entity.serviceRequest.assignedemployeename", "ja-JP", "受理人姓名", "受理人姓名"),
            // entity.serviceRequest.assignedemployeename
            new TranslationSeedItem("entity.serviceRequest.assignedemployeename", "zh-CN", "受理人姓名", "受理人姓名"),
            // entity.serviceRequest.assignedemployeename
            new TranslationSeedItem("entity.serviceRequest.assignedemployeename", "zh-HK", "受理人姓名", "受理人姓名"),

            // entity.serviceRequest.assignedat
            new TranslationSeedItem("entity.serviceRequest.assignedat", "en-US", "受理时间", "受理时间"),
            // entity.serviceRequest.assignedat
            new TranslationSeedItem("entity.serviceRequest.assignedat", "ja-JP", "受理时间", "受理时间"),
            // entity.serviceRequest.assignedat
            new TranslationSeedItem("entity.serviceRequest.assignedat", "zh-CN", "受理时间", "受理时间"),
            // entity.serviceRequest.assignedat
            new TranslationSeedItem("entity.serviceRequest.assignedat", "zh-HK", "受理时间", "受理时间"),

            // entity.serviceRequest.closedat
            new TranslationSeedItem("entity.serviceRequest.closedat", "en-US", "关闭时间", "关闭时间"),
            // entity.serviceRequest.closedat
            new TranslationSeedItem("entity.serviceRequest.closedat", "ja-JP", "关闭时间", "关闭时间"),
            // entity.serviceRequest.closedat
            new TranslationSeedItem("entity.serviceRequest.closedat", "zh-CN", "关闭时间", "关闭时间"),
            // entity.serviceRequest.closedat
            new TranslationSeedItem("entity.serviceRequest.closedat", "zh-HK", "关闭时间", "关闭时间"),

            // entity.serviceRequest.sortorder
            new TranslationSeedItem("entity.serviceRequest.sortorder", "en-US", "排序号", "排序号（越小越靠前）"),
            // entity.serviceRequest.sortorder
            new TranslationSeedItem("entity.serviceRequest.sortorder", "ja-JP", "排序号", "排序号（越小越靠前）"),
            // entity.serviceRequest.sortorder
            new TranslationSeedItem("entity.serviceRequest.sortorder", "zh-CN", "排序号", "排序号（越小越靠前）"),
            // entity.serviceRequest.sortorder
            new TranslationSeedItem("entity.serviceRequest.sortorder", "zh-HK", "排序号", "排序号（越小越靠前）"),

            // entity.serviceRequest.tickets
            new TranslationSeedItem("entity.serviceRequest.tickets", "en-US", "服务工单列表", "服务工单列表（外键在子表 <see cref=\"TaktServiceTicket.ServiceRequestId\"/>）"),
            // entity.serviceRequest.tickets
            new TranslationSeedItem("entity.serviceRequest.tickets", "ja-JP", "服务工单列表", "服务工单列表（外键在子表 <see cref=\"TaktServiceTicket.ServiceRequestId\"/>）"),
            // entity.serviceRequest.tickets
            new TranslationSeedItem("entity.serviceRequest.tickets", "zh-CN", "服务工单列表", "服务工单列表（外键在子表 <see cref=\"TaktServiceTicket.ServiceRequestId\"/>）"),
            // entity.serviceRequest.tickets
            new TranslationSeedItem("entity.serviceRequest.tickets", "zh-HK", "服务工单列表", "服务工单列表（外键在子表 <see cref=\"TaktServiceTicket.ServiceRequestId\"/>）"),
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
