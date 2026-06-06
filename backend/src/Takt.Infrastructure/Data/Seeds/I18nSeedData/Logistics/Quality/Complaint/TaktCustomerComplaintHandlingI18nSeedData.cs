// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Complaint
// 文件名称：TaktCustomerComplaintHandlingI18nSeedData.cs
// 创建时间：2026-06-06
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
using Takt.Shared.Enums;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Complaint;

/// <summary>
/// TaktCustomerComplaintHandling 实体国际化翻译种子（键前缀 entity.customerComplaintHandling.*）
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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 customerComplaintHandling 实体翻译...", tenantCode);

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
    /// I18nKey：entity.customerComplaintHandling._self / entity.customerComplaintHandling.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetCustomerComplaintHandlingTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.customerComplaintHandling._self
            new TranslationSeedItem("entity.customerComplaintHandling._self", "en-US", "Customer Complaint Handling Information", "实体名称"),
            // entity.customerComplaintHandling._self
            new TranslationSeedItem("entity.customerComplaintHandling._self", "ja-JP", "客诉处理记录信息", "实体名称"),
            // entity.customerComplaintHandling._self
            new TranslationSeedItem("entity.customerComplaintHandling._self", "zh-CN", "客诉处理记录信息", "实体名称"),
            // entity.customerComplaintHandling._self
            new TranslationSeedItem("entity.customerComplaintHandling._self", "zh-HK", "客诉处理记录信息", "实体名称"),

            // entity.customerComplaintHandling.complainthandlingcode
            new TranslationSeedItem("entity.customerComplaintHandling.complainthandlingcode", "en-US", "客诉处理记录编码", "客诉处理记录编码（唯一索引）"),
            // entity.customerComplaintHandling.complainthandlingcode
            new TranslationSeedItem("entity.customerComplaintHandling.complainthandlingcode", "ja-JP", "客诉处理记录编码", "客诉处理记录编码（唯一索引）"),
            // entity.customerComplaintHandling.complainthandlingcode
            new TranslationSeedItem("entity.customerComplaintHandling.complainthandlingcode", "zh-CN", "客诉处理记录编码", "客诉处理记录编码（唯一索引）"),
            // entity.customerComplaintHandling.complainthandlingcode
            new TranslationSeedItem("entity.customerComplaintHandling.complainthandlingcode", "zh-HK", "客诉处理记录编码", "客诉处理记录编码（唯一索引）"),

            // entity.customerComplaintHandling.complaintid
            new TranslationSeedItem("entity.customerComplaintHandling.complaintid", "en-US", "客诉ID", "客诉ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.customerComplaintHandling.complaintid
            new TranslationSeedItem("entity.customerComplaintHandling.complaintid", "ja-JP", "客诉ID", "客诉ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.customerComplaintHandling.complaintid
            new TranslationSeedItem("entity.customerComplaintHandling.complaintid", "zh-CN", "客诉ID", "客诉ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.customerComplaintHandling.complaintid
            new TranslationSeedItem("entity.customerComplaintHandling.complaintid", "zh-HK", "客诉ID", "客诉ID（主子表关系，序列化为string以避免Javascript精度问题）"),

            // entity.customerComplaintHandling.complaintno
            new TranslationSeedItem("entity.customerComplaintHandling.complaintno", "en-US", "客诉单号", "客诉单号（冗余字段，便于查询）"),
            // entity.customerComplaintHandling.complaintno
            new TranslationSeedItem("entity.customerComplaintHandling.complaintno", "ja-JP", "客诉单号", "客诉单号（冗余字段，便于查询）"),
            // entity.customerComplaintHandling.complaintno
            new TranslationSeedItem("entity.customerComplaintHandling.complaintno", "zh-CN", "客诉单号", "客诉单号（冗余字段，便于查询）"),
            // entity.customerComplaintHandling.complaintno
            new TranslationSeedItem("entity.customerComplaintHandling.complaintno", "zh-HK", "客诉单号", "客诉单号（冗余字段，便于查询）"),

            // entity.customerComplaintHandling.complaintitemid
            new TranslationSeedItem("entity.customerComplaintHandling.complaintitemid", "en-US", "客诉明细ID", "客诉明细ID（可选，关联到具体不良项目，序列化为string以避免Javascript精度问题）"),
            // entity.customerComplaintHandling.complaintitemid
            new TranslationSeedItem("entity.customerComplaintHandling.complaintitemid", "ja-JP", "客诉明细ID", "客诉明细ID（可选，关联到具体不良项目，序列化为string以避免Javascript精度问题）"),
            // entity.customerComplaintHandling.complaintitemid
            new TranslationSeedItem("entity.customerComplaintHandling.complaintitemid", "zh-CN", "客诉明细ID", "客诉明细ID（可选，关联到具体不良项目，序列化为string以避免Javascript精度问题）"),
            // entity.customerComplaintHandling.complaintitemid
            new TranslationSeedItem("entity.customerComplaintHandling.complaintitemid", "zh-HK", "客诉明细ID", "客诉明细ID（可选，关联到具体不良项目，序列化为string以避免Javascript精度问题）"),

            // entity.customerComplaintHandling.handlingstage
            new TranslationSeedItem("entity.customerComplaintHandling.handlingstage", "en-US", "处理阶段", "处理阶段（0=初步响应，1=原因分析，2=改善对策，3=效果验证，4=结案）"),
            // entity.customerComplaintHandling.handlingstage
            new TranslationSeedItem("entity.customerComplaintHandling.handlingstage", "ja-JP", "处理阶段", "处理阶段（0=初步响应，1=原因分析，2=改善对策，3=效果验证，4=结案）"),
            // entity.customerComplaintHandling.handlingstage
            new TranslationSeedItem("entity.customerComplaintHandling.handlingstage", "zh-CN", "处理阶段", "处理阶段（0=初步响应，1=原因分析，2=改善对策，3=效果验证，4=结案）"),
            // entity.customerComplaintHandling.handlingstage
            new TranslationSeedItem("entity.customerComplaintHandling.handlingstage", "zh-HK", "处理阶段", "处理阶段（0=初步响应，1=原因分析，2=改善对策，3=效果验证，4=结案）"),

            // entity.customerComplaintHandling.handlingmethod
            new TranslationSeedItem("entity.customerComplaintHandling.handlingmethod", "en-US", "处理方式", "处理方式（0=返工，1=返修，2=补货，3=退货，4=退款，5=折扣，6=其他）"),
            // entity.customerComplaintHandling.handlingmethod
            new TranslationSeedItem("entity.customerComplaintHandling.handlingmethod", "ja-JP", "处理方式", "处理方式（0=返工，1=返修，2=补货，3=退货，4=退款，5=折扣，6=其他）"),
            // entity.customerComplaintHandling.handlingmethod
            new TranslationSeedItem("entity.customerComplaintHandling.handlingmethod", "zh-CN", "处理方式", "处理方式（0=返工，1=返修，2=补货，3=退货，4=退款，5=折扣，6=其他）"),
            // entity.customerComplaintHandling.handlingmethod
            new TranslationSeedItem("entity.customerComplaintHandling.handlingmethod", "zh-HK", "处理方式", "处理方式（0=返工，1=返修，2=补货，3=退货，4=退款，5=折扣，6=其他）"),

            // entity.customerComplaintHandling.handlingdescription
            new TranslationSeedItem("entity.customerComplaintHandling.handlingdescription", "en-US", "处理说明", "处理说明"),
            // entity.customerComplaintHandling.handlingdescription
            new TranslationSeedItem("entity.customerComplaintHandling.handlingdescription", "ja-JP", "处理说明", "处理说明"),
            // entity.customerComplaintHandling.handlingdescription
            new TranslationSeedItem("entity.customerComplaintHandling.handlingdescription", "zh-CN", "处理说明", "处理说明"),
            // entity.customerComplaintHandling.handlingdescription
            new TranslationSeedItem("entity.customerComplaintHandling.handlingdescription", "zh-HK", "处理说明", "处理说明"),

            // entity.customerComplaintHandling.causeanalysis
            new TranslationSeedItem("entity.customerComplaintHandling.causeanalysis", "en-US", "原因分析", "原因分析"),
            // entity.customerComplaintHandling.causeanalysis
            new TranslationSeedItem("entity.customerComplaintHandling.causeanalysis", "ja-JP", "原因分析", "原因分析"),
            // entity.customerComplaintHandling.causeanalysis
            new TranslationSeedItem("entity.customerComplaintHandling.causeanalysis", "zh-CN", "原因分析", "原因分析"),
            // entity.customerComplaintHandling.causeanalysis
            new TranslationSeedItem("entity.customerComplaintHandling.causeanalysis", "zh-HK", "原因分析", "原因分析"),

            // entity.customerComplaintHandling.correctiveaction
            new TranslationSeedItem("entity.customerComplaintHandling.correctiveaction", "en-US", "纠正措施", "改善对策/纠正措施"),
            // entity.customerComplaintHandling.correctiveaction
            new TranslationSeedItem("entity.customerComplaintHandling.correctiveaction", "ja-JP", "纠正措施", "改善对策/纠正措施"),
            // entity.customerComplaintHandling.correctiveaction
            new TranslationSeedItem("entity.customerComplaintHandling.correctiveaction", "zh-CN", "纠正措施", "改善对策/纠正措施"),
            // entity.customerComplaintHandling.correctiveaction
            new TranslationSeedItem("entity.customerComplaintHandling.correctiveaction", "zh-HK", "纠正措施", "改善对策/纠正措施"),

            // entity.customerComplaintHandling.preventiveaction
            new TranslationSeedItem("entity.customerComplaintHandling.preventiveaction", "en-US", "预防措施", "预防措施"),
            // entity.customerComplaintHandling.preventiveaction
            new TranslationSeedItem("entity.customerComplaintHandling.preventiveaction", "ja-JP", "预防措施", "预防措施"),
            // entity.customerComplaintHandling.preventiveaction
            new TranslationSeedItem("entity.customerComplaintHandling.preventiveaction", "zh-CN", "预防措施", "预防措施"),
            // entity.customerComplaintHandling.preventiveaction
            new TranslationSeedItem("entity.customerComplaintHandling.preventiveaction", "zh-HK", "预防措施", "预防措施"),

            // entity.customerComplaintHandling.responsibledept
            new TranslationSeedItem("entity.customerComplaintHandling.responsibledept", "en-US", "责任部门", "责任部门"),
            // entity.customerComplaintHandling.responsibledept
            new TranslationSeedItem("entity.customerComplaintHandling.responsibledept", "ja-JP", "责任部门", "责任部门"),
            // entity.customerComplaintHandling.responsibledept
            new TranslationSeedItem("entity.customerComplaintHandling.responsibledept", "zh-CN", "责任部门", "责任部门"),
            // entity.customerComplaintHandling.responsibledept
            new TranslationSeedItem("entity.customerComplaintHandling.responsibledept", "zh-HK", "责任部门", "责任部门"),

            // entity.customerComplaintHandling.responsibleby
            new TranslationSeedItem("entity.customerComplaintHandling.responsibleby", "en-US", "责任人", "责任人（人员代码）"),
            // entity.customerComplaintHandling.responsibleby
            new TranslationSeedItem("entity.customerComplaintHandling.responsibleby", "ja-JP", "责任人", "责任人（人员代码）"),
            // entity.customerComplaintHandling.responsibleby
            new TranslationSeedItem("entity.customerComplaintHandling.responsibleby", "zh-CN", "责任人", "责任人（人员代码）"),
            // entity.customerComplaintHandling.responsibleby
            new TranslationSeedItem("entity.customerComplaintHandling.responsibleby", "zh-HK", "责任人", "责任人（人员代码）"),

            // entity.customerComplaintHandling.handlerby
            new TranslationSeedItem("entity.customerComplaintHandling.handlerby", "en-US", "处理人", "处理人（人员代码）"),
            // entity.customerComplaintHandling.handlerby
            new TranslationSeedItem("entity.customerComplaintHandling.handlerby", "ja-JP", "处理人", "处理人（人员代码）"),
            // entity.customerComplaintHandling.handlerby
            new TranslationSeedItem("entity.customerComplaintHandling.handlerby", "zh-CN", "处理人", "处理人（人员代码）"),
            // entity.customerComplaintHandling.handlerby
            new TranslationSeedItem("entity.customerComplaintHandling.handlerby", "zh-HK", "处理人", "处理人（人员代码）"),

            // entity.customerComplaintHandling.handlingat
            new TranslationSeedItem("entity.customerComplaintHandling.handlingat", "en-US", "处理时间", "处理时间"),
            // entity.customerComplaintHandling.handlingat
            new TranslationSeedItem("entity.customerComplaintHandling.handlingat", "ja-JP", "处理时间", "处理时间"),
            // entity.customerComplaintHandling.handlingat
            new TranslationSeedItem("entity.customerComplaintHandling.handlingat", "zh-CN", "处理时间", "处理时间"),
            // entity.customerComplaintHandling.handlingat
            new TranslationSeedItem("entity.customerComplaintHandling.handlingat", "zh-HK", "处理时间", "处理时间"),

            // entity.customerComplaintHandling.plannedcompletiondate
            new TranslationSeedItem("entity.customerComplaintHandling.plannedcompletiondate", "en-US", "计划完成日期", "计划完成日期"),
            // entity.customerComplaintHandling.plannedcompletiondate
            new TranslationSeedItem("entity.customerComplaintHandling.plannedcompletiondate", "ja-JP", "计划完成日期", "计划完成日期"),
            // entity.customerComplaintHandling.plannedcompletiondate
            new TranslationSeedItem("entity.customerComplaintHandling.plannedcompletiondate", "zh-CN", "计划完成日期", "计划完成日期"),
            // entity.customerComplaintHandling.plannedcompletiondate
            new TranslationSeedItem("entity.customerComplaintHandling.plannedcompletiondate", "zh-HK", "计划完成日期", "计划完成日期"),

            // entity.customerComplaintHandling.actualcompletiondate
            new TranslationSeedItem("entity.customerComplaintHandling.actualcompletiondate", "en-US", "实际完成日期", "实际完成日期"),
            // entity.customerComplaintHandling.actualcompletiondate
            new TranslationSeedItem("entity.customerComplaintHandling.actualcompletiondate", "ja-JP", "实际完成日期", "实际完成日期"),
            // entity.customerComplaintHandling.actualcompletiondate
            new TranslationSeedItem("entity.customerComplaintHandling.actualcompletiondate", "zh-CN", "实际完成日期", "实际完成日期"),
            // entity.customerComplaintHandling.actualcompletiondate
            new TranslationSeedItem("entity.customerComplaintHandling.actualcompletiondate", "zh-HK", "实际完成日期", "实际完成日期"),

            // entity.customerComplaintHandling.handlingstatus
            new TranslationSeedItem("entity.customerComplaintHandling.handlingstatus", "en-US", "处理状态", "处理状态（0=待处理，1=处理中，2=已完成，3=已关闭，4=已驳回）"),
            // entity.customerComplaintHandling.handlingstatus
            new TranslationSeedItem("entity.customerComplaintHandling.handlingstatus", "ja-JP", "处理状态", "处理状态（0=待处理，1=处理中，2=已完成，3=已关闭，4=已驳回）"),
            // entity.customerComplaintHandling.handlingstatus
            new TranslationSeedItem("entity.customerComplaintHandling.handlingstatus", "zh-CN", "处理状态", "处理状态（0=待处理，1=处理中，2=已完成，3=已关闭，4=已驳回）"),
            // entity.customerComplaintHandling.handlingstatus
            new TranslationSeedItem("entity.customerComplaintHandling.handlingstatus", "zh-HK", "处理状态", "处理状态（0=待处理，1=处理中，2=已完成，3=已关闭，4=已驳回）"),

            // entity.customerComplaintHandling.handlingcost
            new TranslationSeedItem("entity.customerComplaintHandling.handlingcost", "en-US", "处理成本", "处理成本/损失金额"),
            // entity.customerComplaintHandling.handlingcost
            new TranslationSeedItem("entity.customerComplaintHandling.handlingcost", "ja-JP", "处理成本", "处理成本/损失金额"),
            // entity.customerComplaintHandling.handlingcost
            new TranslationSeedItem("entity.customerComplaintHandling.handlingcost", "zh-CN", "处理成本", "处理成本/损失金额"),
            // entity.customerComplaintHandling.handlingcost
            new TranslationSeedItem("entity.customerComplaintHandling.handlingcost", "zh-HK", "处理成本", "处理成本/损失金额"),

            // entity.customerComplaintHandling.customerfeedback
            new TranslationSeedItem("entity.customerComplaintHandling.customerfeedback", "en-US", "客户反馈", "客户反馈"),
            // entity.customerComplaintHandling.customerfeedback
            new TranslationSeedItem("entity.customerComplaintHandling.customerfeedback", "ja-JP", "客户反馈", "客户反馈"),
            // entity.customerComplaintHandling.customerfeedback
            new TranslationSeedItem("entity.customerComplaintHandling.customerfeedback", "zh-CN", "客户反馈", "客户反馈"),
            // entity.customerComplaintHandling.customerfeedback
            new TranslationSeedItem("entity.customerComplaintHandling.customerfeedback", "zh-HK", "客户反馈", "客户反馈"),

            // entity.customerComplaintHandling.customersatisfaction
            new TranslationSeedItem("entity.customerComplaintHandling.customersatisfaction", "en-US", "客户满意度", "客户满意度（0=不满意，1=一般，2=满意，3=非常满意）"),
            // entity.customerComplaintHandling.customersatisfaction
            new TranslationSeedItem("entity.customerComplaintHandling.customersatisfaction", "ja-JP", "客户满意度", "客户满意度（0=不满意，1=一般，2=满意，3=非常满意）"),
            // entity.customerComplaintHandling.customersatisfaction
            new TranslationSeedItem("entity.customerComplaintHandling.customersatisfaction", "zh-CN", "客户满意度", "客户满意度（0=不满意，1=一般，2=满意，3=非常满意）"),
            // entity.customerComplaintHandling.customersatisfaction
            new TranslationSeedItem("entity.customerComplaintHandling.customersatisfaction", "zh-HK", "客户满意度", "客户满意度（0=不满意，1=一般，2=满意，3=非常满意）"),

            // entity.customerComplaintHandling.attachmentpaths
            new TranslationSeedItem("entity.customerComplaintHandling.attachmentpaths", "en-US", "附件路径", "附件路径（JSON格式，存储相关文件URL列表）"),
            // entity.customerComplaintHandling.attachmentpaths
            new TranslationSeedItem("entity.customerComplaintHandling.attachmentpaths", "ja-JP", "附件路径", "附件路径（JSON格式，存储相关文件URL列表）"),
            // entity.customerComplaintHandling.attachmentpaths
            new TranslationSeedItem("entity.customerComplaintHandling.attachmentpaths", "zh-CN", "附件路径", "附件路径（JSON格式，存储相关文件URL列表）"),
            // entity.customerComplaintHandling.attachmentpaths
            new TranslationSeedItem("entity.customerComplaintHandling.attachmentpaths", "zh-HK", "附件路径", "附件路径（JSON格式，存储相关文件URL列表）"),
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
