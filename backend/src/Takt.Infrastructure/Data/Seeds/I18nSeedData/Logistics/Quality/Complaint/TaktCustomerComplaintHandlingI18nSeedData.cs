// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Complaint
// 文件名称：TaktCustomerComplaintHandlingI18nSeedData.cs
// 创建时间：2026-08-18
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktCustomerComplaintHandling 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Complaint;

/// <summary>
/// TaktCustomerComplaintHandling 实体国际化翻译种子（键前缀 entity.customercomplainthandling.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktCustomerComplaintHandlingI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktCustomerComplaintHandling 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 customercomplainthandling 实体翻译...", tenantCode);

        foreach (var item in GetCustomerComplaintHandlingTranslations())
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

        TaktLogger.Information("TaktCustomerComplaintHandling 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktCustomerComplaintHandling 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.customercomplainthandling._self / entity.customercomplainthandling.{{field}}；ResourceGroup=Complaint；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetCustomerComplaintHandlingTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.customercomplainthandling._self
            new TranslationSeedItem("entity.customercomplainthandling._self", "en-US", "Customer Complaint Handling Information_us", "实体名称"),
            // entity.customercomplainthandling._self
            new TranslationSeedItem("entity.customercomplainthandling._self", "ja-JP", "客诉处理记录信息_jp", "实体名称"),
            // entity.customercomplainthandling._self
            new TranslationSeedItem("entity.customercomplainthandling._self", "zh-CN", "客诉处理记录信息", "实体名称"),
            // entity.customercomplainthandling._self
            new TranslationSeedItem("entity.customercomplainthandling._self", "zh-HK", "客诉处理记录信息_hk", "实体名称"),

            // entity.customercomplainthandling.complainthandlingcode
            new TranslationSeedItem("entity.customercomplainthandling.complainthandlingcode", "en-US", "客诉处理记录编码_us", "客诉处理记录编码（唯一索引）"),
            // entity.customercomplainthandling.complainthandlingcode
            new TranslationSeedItem("entity.customercomplainthandling.complainthandlingcode", "ja-JP", "客诉处理记录编码_jp", "客诉处理记录编码（唯一索引）"),
            // entity.customercomplainthandling.complainthandlingcode
            new TranslationSeedItem("entity.customercomplainthandling.complainthandlingcode", "zh-CN", "客诉处理记录编码", "客诉处理记录编码（唯一索引）"),
            // entity.customercomplainthandling.complainthandlingcode
            new TranslationSeedItem("entity.customercomplainthandling.complainthandlingcode", "zh-HK", "客诉处理记录编码_hk", "客诉处理记录编码（唯一索引）"),

            // entity.customercomplainthandling.complaintid
            new TranslationSeedItem("entity.customercomplainthandling.complaintid", "en-US", "客诉ID_us", "客诉 ID（选项 TaktCustomerComplaints/options；DictValue=Id）"),
            // entity.customercomplainthandling.complaintid
            new TranslationSeedItem("entity.customercomplainthandling.complaintid", "ja-JP", "客诉ID_jp", "客诉 ID（选项 TaktCustomerComplaints/options；DictValue=Id）"),
            // entity.customercomplainthandling.complaintid
            new TranslationSeedItem("entity.customercomplainthandling.complaintid", "zh-CN", "客诉ID", "客诉 ID（选项 TaktCustomerComplaints/options；DictValue=Id）"),
            // entity.customercomplainthandling.complaintid
            new TranslationSeedItem("entity.customercomplainthandling.complaintid", "zh-HK", "客诉ID_hk", "客诉 ID（选项 TaktCustomerComplaints/options；DictValue=Id）"),

            // entity.customercomplainthandling.complaintcode
            new TranslationSeedItem("entity.customercomplainthandling.complaintcode", "en-US", "客诉单号_us", "客诉单号（冗余字段，便于查询）"),
            // entity.customercomplainthandling.complaintcode
            new TranslationSeedItem("entity.customercomplainthandling.complaintcode", "ja-JP", "客诉单号_jp", "客诉单号（冗余字段，便于查询）"),
            // entity.customercomplainthandling.complaintcode
            new TranslationSeedItem("entity.customercomplainthandling.complaintcode", "zh-CN", "客诉单号", "客诉单号（冗余字段，便于查询）"),
            // entity.customercomplainthandling.complaintcode
            new TranslationSeedItem("entity.customercomplainthandling.complaintcode", "zh-HK", "客诉单号_hk", "客诉单号（冗余字段，便于查询）"),

            // entity.customercomplainthandling.complaintitemid
            new TranslationSeedItem("entity.customercomplainthandling.complaintitemid", "en-US", "客诉明细ID_us", "客诉明细 ID（选项 TaktCustomerComplaintItems/options；DictValue=Id）"),
            // entity.customercomplainthandling.complaintitemid
            new TranslationSeedItem("entity.customercomplainthandling.complaintitemid", "ja-JP", "客诉明细ID_jp", "客诉明细 ID（选项 TaktCustomerComplaintItems/options；DictValue=Id）"),
            // entity.customercomplainthandling.complaintitemid
            new TranslationSeedItem("entity.customercomplainthandling.complaintitemid", "zh-CN", "客诉明细ID", "客诉明细 ID（选项 TaktCustomerComplaintItems/options；DictValue=Id）"),
            // entity.customercomplainthandling.complaintitemid
            new TranslationSeedItem("entity.customercomplainthandling.complaintitemid", "zh-HK", "客诉明细ID_hk", "客诉明细 ID（选项 TaktCustomerComplaintItems/options；DictValue=Id）"),

            // entity.customercomplainthandling.handlingstage
            new TranslationSeedItem("entity.customercomplainthandling.handlingstage", "en-US", "处理阶段_us", "处理阶段（字典 logistics_quality_complaint_handling_stage）"),
            // entity.customercomplainthandling.handlingstage
            new TranslationSeedItem("entity.customercomplainthandling.handlingstage", "ja-JP", "处理阶段_jp", "处理阶段（字典 logistics_quality_complaint_handling_stage）"),
            // entity.customercomplainthandling.handlingstage
            new TranslationSeedItem("entity.customercomplainthandling.handlingstage", "zh-CN", "处理阶段", "处理阶段（字典 logistics_quality_complaint_handling_stage）"),
            // entity.customercomplainthandling.handlingstage
            new TranslationSeedItem("entity.customercomplainthandling.handlingstage", "zh-HK", "处理阶段_hk", "处理阶段（字典 logistics_quality_complaint_handling_stage）"),

            // entity.customercomplainthandling.handlingmethod
            new TranslationSeedItem("entity.customercomplainthandling.handlingmethod", "en-US", "处理方式_us", "处理方式（字典 logistics_quality_complaint_handling_method）"),
            // entity.customercomplainthandling.handlingmethod
            new TranslationSeedItem("entity.customercomplainthandling.handlingmethod", "ja-JP", "处理方式_jp", "处理方式（字典 logistics_quality_complaint_handling_method）"),
            // entity.customercomplainthandling.handlingmethod
            new TranslationSeedItem("entity.customercomplainthandling.handlingmethod", "zh-CN", "处理方式", "处理方式（字典 logistics_quality_complaint_handling_method）"),
            // entity.customercomplainthandling.handlingmethod
            new TranslationSeedItem("entity.customercomplainthandling.handlingmethod", "zh-HK", "处理方式_hk", "处理方式（字典 logistics_quality_complaint_handling_method）"),

            // entity.customercomplainthandling.handlingdescription
            new TranslationSeedItem("entity.customercomplainthandling.handlingdescription", "en-US", "处理说明_us", "处理说明"),
            // entity.customercomplainthandling.handlingdescription
            new TranslationSeedItem("entity.customercomplainthandling.handlingdescription", "ja-JP", "处理说明_jp", "处理说明"),
            // entity.customercomplainthandling.handlingdescription
            new TranslationSeedItem("entity.customercomplainthandling.handlingdescription", "zh-CN", "处理说明", "处理说明"),
            // entity.customercomplainthandling.handlingdescription
            new TranslationSeedItem("entity.customercomplainthandling.handlingdescription", "zh-HK", "处理说明_hk", "处理说明"),

            // entity.customercomplainthandling.causeanalysis
            new TranslationSeedItem("entity.customercomplainthandling.causeanalysis", "en-US", "原因分析_us", "原因分析"),
            // entity.customercomplainthandling.causeanalysis
            new TranslationSeedItem("entity.customercomplainthandling.causeanalysis", "ja-JP", "原因分析_jp", "原因分析"),
            // entity.customercomplainthandling.causeanalysis
            new TranslationSeedItem("entity.customercomplainthandling.causeanalysis", "zh-CN", "原因分析", "原因分析"),
            // entity.customercomplainthandling.causeanalysis
            new TranslationSeedItem("entity.customercomplainthandling.causeanalysis", "zh-HK", "原因分析_hk", "原因分析"),

            // entity.customercomplainthandling.correctiveaction
            new TranslationSeedItem("entity.customercomplainthandling.correctiveaction", "en-US", "纠正措施_us", "改善对策/纠正措施"),
            // entity.customercomplainthandling.correctiveaction
            new TranslationSeedItem("entity.customercomplainthandling.correctiveaction", "ja-JP", "纠正措施_jp", "改善对策/纠正措施"),
            // entity.customercomplainthandling.correctiveaction
            new TranslationSeedItem("entity.customercomplainthandling.correctiveaction", "zh-CN", "纠正措施", "改善对策/纠正措施"),
            // entity.customercomplainthandling.correctiveaction
            new TranslationSeedItem("entity.customercomplainthandling.correctiveaction", "zh-HK", "纠正措施_hk", "改善对策/纠正措施"),

            // entity.customercomplainthandling.preventiveaction
            new TranslationSeedItem("entity.customercomplainthandling.preventiveaction", "en-US", "预防措施_us", "预防措施"),
            // entity.customercomplainthandling.preventiveaction
            new TranslationSeedItem("entity.customercomplainthandling.preventiveaction", "ja-JP", "预防措施_jp", "预防措施"),
            // entity.customercomplainthandling.preventiveaction
            new TranslationSeedItem("entity.customercomplainthandling.preventiveaction", "zh-CN", "预防措施", "预防措施"),
            // entity.customercomplainthandling.preventiveaction
            new TranslationSeedItem("entity.customercomplainthandling.preventiveaction", "zh-HK", "预防措施_hk", "预防措施"),

            // entity.customercomplainthandling.responsibledept
            new TranslationSeedItem("entity.customercomplainthandling.responsibledept", "en-US", "责任部门_us", "责任部门（选项 TaktDepts/tree-options；DictValue=DeptCode）"),
            // entity.customercomplainthandling.responsibledept
            new TranslationSeedItem("entity.customercomplainthandling.responsibledept", "ja-JP", "责任部门_jp", "责任部门（选项 TaktDepts/tree-options；DictValue=DeptCode）"),
            // entity.customercomplainthandling.responsibledept
            new TranslationSeedItem("entity.customercomplainthandling.responsibledept", "zh-CN", "责任部门", "责任部门（选项 TaktDepts/tree-options；DictValue=DeptCode）"),
            // entity.customercomplainthandling.responsibledept
            new TranslationSeedItem("entity.customercomplainthandling.responsibledept", "zh-HK", "责任部门_hk", "责任部门（选项 TaktDepts/tree-options；DictValue=DeptCode）"),

            // entity.customercomplainthandling.responsibleby
            new TranslationSeedItem("entity.customercomplainthandling.responsibleby", "en-US", "责任人_us", "责任人（选项 TaktEmployees/options；DictValue=EmployeeCode）"),
            // entity.customercomplainthandling.responsibleby
            new TranslationSeedItem("entity.customercomplainthandling.responsibleby", "ja-JP", "责任人_jp", "责任人（选项 TaktEmployees/options；DictValue=EmployeeCode）"),
            // entity.customercomplainthandling.responsibleby
            new TranslationSeedItem("entity.customercomplainthandling.responsibleby", "zh-CN", "责任人", "责任人（选项 TaktEmployees/options；DictValue=EmployeeCode）"),
            // entity.customercomplainthandling.responsibleby
            new TranslationSeedItem("entity.customercomplainthandling.responsibleby", "zh-HK", "责任人_hk", "责任人（选项 TaktEmployees/options；DictValue=EmployeeCode）"),

            // entity.customercomplainthandling.handlerby
            new TranslationSeedItem("entity.customercomplainthandling.handlerby", "en-US", "处理人_us", "处理人（选项 TaktEmployees/options；DictValue=EmployeeCode）"),
            // entity.customercomplainthandling.handlerby
            new TranslationSeedItem("entity.customercomplainthandling.handlerby", "ja-JP", "处理人_jp", "处理人（选项 TaktEmployees/options；DictValue=EmployeeCode）"),
            // entity.customercomplainthandling.handlerby
            new TranslationSeedItem("entity.customercomplainthandling.handlerby", "zh-CN", "处理人", "处理人（选项 TaktEmployees/options；DictValue=EmployeeCode）"),
            // entity.customercomplainthandling.handlerby
            new TranslationSeedItem("entity.customercomplainthandling.handlerby", "zh-HK", "处理人_hk", "处理人（选项 TaktEmployees/options；DictValue=EmployeeCode）"),

            // entity.customercomplainthandling.handlingat
            new TranslationSeedItem("entity.customercomplainthandling.handlingat", "en-US", "处理时间_us", "处理时间"),
            // entity.customercomplainthandling.handlingat
            new TranslationSeedItem("entity.customercomplainthandling.handlingat", "ja-JP", "处理时间_jp", "处理时间"),
            // entity.customercomplainthandling.handlingat
            new TranslationSeedItem("entity.customercomplainthandling.handlingat", "zh-CN", "处理时间", "处理时间"),
            // entity.customercomplainthandling.handlingat
            new TranslationSeedItem("entity.customercomplainthandling.handlingat", "zh-HK", "处理时间_hk", "处理时间"),

            // entity.customercomplainthandling.plannedcompletiondate
            new TranslationSeedItem("entity.customercomplainthandling.plannedcompletiondate", "en-US", "计划完成日期_us", "计划完成日期"),
            // entity.customercomplainthandling.plannedcompletiondate
            new TranslationSeedItem("entity.customercomplainthandling.plannedcompletiondate", "ja-JP", "计划完成日期_jp", "计划完成日期"),
            // entity.customercomplainthandling.plannedcompletiondate
            new TranslationSeedItem("entity.customercomplainthandling.plannedcompletiondate", "zh-CN", "计划完成日期", "计划完成日期"),
            // entity.customercomplainthandling.plannedcompletiondate
            new TranslationSeedItem("entity.customercomplainthandling.plannedcompletiondate", "zh-HK", "计划完成日期_hk", "计划完成日期"),

            // entity.customercomplainthandling.actualcompletiondate
            new TranslationSeedItem("entity.customercomplainthandling.actualcompletiondate", "en-US", "实际完成日期_us", "实际完成日期"),
            // entity.customercomplainthandling.actualcompletiondate
            new TranslationSeedItem("entity.customercomplainthandling.actualcompletiondate", "ja-JP", "实际完成日期_jp", "实际完成日期"),
            // entity.customercomplainthandling.actualcompletiondate
            new TranslationSeedItem("entity.customercomplainthandling.actualcompletiondate", "zh-CN", "实际完成日期", "实际完成日期"),
            // entity.customercomplainthandling.actualcompletiondate
            new TranslationSeedItem("entity.customercomplainthandling.actualcompletiondate", "zh-HK", "实际完成日期_hk", "实际完成日期"),

            // entity.customercomplainthandling.handlingcost
            new TranslationSeedItem("entity.customercomplainthandling.handlingcost", "en-US", "处理成本_us", "处理成本/损失金额"),
            // entity.customercomplainthandling.handlingcost
            new TranslationSeedItem("entity.customercomplainthandling.handlingcost", "ja-JP", "处理成本_jp", "处理成本/损失金额"),
            // entity.customercomplainthandling.handlingcost
            new TranslationSeedItem("entity.customercomplainthandling.handlingcost", "zh-CN", "处理成本", "处理成本/损失金额"),
            // entity.customercomplainthandling.handlingcost
            new TranslationSeedItem("entity.customercomplainthandling.handlingcost", "zh-HK", "处理成本_hk", "处理成本/损失金额"),

            // entity.customercomplainthandling.customerfeedback
            new TranslationSeedItem("entity.customercomplainthandling.customerfeedback", "en-US", "客户反馈_us", "客户反馈"),
            // entity.customercomplainthandling.customerfeedback
            new TranslationSeedItem("entity.customercomplainthandling.customerfeedback", "ja-JP", "客户反馈_jp", "客户反馈"),
            // entity.customercomplainthandling.customerfeedback
            new TranslationSeedItem("entity.customercomplainthandling.customerfeedback", "zh-CN", "客户反馈", "客户反馈"),
            // entity.customercomplainthandling.customerfeedback
            new TranslationSeedItem("entity.customercomplainthandling.customerfeedback", "zh-HK", "客户反馈_hk", "客户反馈"),

            // entity.customercomplainthandling.customersatisfaction
            new TranslationSeedItem("entity.customercomplainthandling.customersatisfaction", "en-US", "客户满意度_us", "客户满意度（字典 logistics_quality_customer_satisfaction）"),
            // entity.customercomplainthandling.customersatisfaction
            new TranslationSeedItem("entity.customercomplainthandling.customersatisfaction", "ja-JP", "客户满意度_jp", "客户满意度（字典 logistics_quality_customer_satisfaction）"),
            // entity.customercomplainthandling.customersatisfaction
            new TranslationSeedItem("entity.customercomplainthandling.customersatisfaction", "zh-CN", "客户满意度", "客户满意度（字典 logistics_quality_customer_satisfaction）"),
            // entity.customercomplainthandling.customersatisfaction
            new TranslationSeedItem("entity.customercomplainthandling.customersatisfaction", "zh-HK", "客户满意度_hk", "客户满意度（字典 logistics_quality_customer_satisfaction）"),

            // entity.customercomplainthandling.attachmentpaths
            new TranslationSeedItem("entity.customercomplainthandling.attachmentpaths", "en-US", "附件路径_us", "附件路径（JSON格式，存储相关文件URL列表）"),
            // entity.customercomplainthandling.attachmentpaths
            new TranslationSeedItem("entity.customercomplainthandling.attachmentpaths", "ja-JP", "附件路径_jp", "附件路径（JSON格式，存储相关文件URL列表）"),
            // entity.customercomplainthandling.attachmentpaths
            new TranslationSeedItem("entity.customercomplainthandling.attachmentpaths", "zh-CN", "附件路径", "附件路径（JSON格式，存储相关文件URL列表）"),
            // entity.customercomplainthandling.attachmentpaths
            new TranslationSeedItem("entity.customercomplainthandling.attachmentpaths", "zh-HK", "附件路径_hk", "附件路径（JSON格式，存储相关文件URL列表）"),

            // entity.customercomplainthandling.handlingstatus
            new TranslationSeedItem("entity.customercomplainthandling.handlingstatus", "en-US", "处理状态_us", "处理状态（字典 logistics_quality_complaint_handling_status）"),
            // entity.customercomplainthandling.handlingstatus
            new TranslationSeedItem("entity.customercomplainthandling.handlingstatus", "ja-JP", "处理状态_jp", "处理状态（字典 logistics_quality_complaint_handling_status）"),
            // entity.customercomplainthandling.handlingstatus
            new TranslationSeedItem("entity.customercomplainthandling.handlingstatus", "zh-CN", "处理状态", "处理状态（字典 logistics_quality_complaint_handling_status）"),
            // entity.customercomplainthandling.handlingstatus
            new TranslationSeedItem("entity.customercomplainthandling.handlingstatus", "zh-HK", "处理状态_hk", "处理状态（字典 logistics_quality_complaint_handling_status）"),

            // entity.customercomplainthandling.complaint
            new TranslationSeedItem("entity.customercomplainthandling.complaint", "en-US", "客诉主表_us", "客诉主表"),
            // entity.customercomplainthandling.complaint
            new TranslationSeedItem("entity.customercomplainthandling.complaint", "ja-JP", "客诉主表_jp", "客诉主表"),
            // entity.customercomplainthandling.complaint
            new TranslationSeedItem("entity.customercomplainthandling.complaint", "zh-CN", "客诉主表", "客诉主表"),
            // entity.customercomplainthandling.complaint
            new TranslationSeedItem("entity.customercomplainthandling.complaint", "zh-HK", "客诉主表_hk", "客诉主表"),
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
        translation.ResourceGroup = "Complaint";
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
