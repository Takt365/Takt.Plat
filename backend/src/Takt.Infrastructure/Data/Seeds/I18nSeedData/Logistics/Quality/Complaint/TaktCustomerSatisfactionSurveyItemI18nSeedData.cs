// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Complaint
// 文件名称：TaktCustomerSatisfactionSurveyItemI18nSeedData.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktCustomerSatisfactionSurveyItem 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktCustomerSatisfactionSurveyItem 实体国际化翻译种子（键前缀 entity.customerSatisfactionSurveyItem.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktCustomerSatisfactionSurveyItemI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktCustomerSatisfactionSurveyItem 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 customerSatisfactionSurveyItem 实体翻译...", tenantCode);

        foreach (var item in GetCustomerSatisfactionSurveyItemTranslations())
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

        TaktLogger.Information("TaktCustomerSatisfactionSurveyItem 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktCustomerSatisfactionSurveyItem 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.customerSatisfactionSurveyItem._self / entity.customerSatisfactionSurveyItem.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetCustomerSatisfactionSurveyItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.customerSatisfactionSurveyItem._self
            new TranslationSeedItem("entity.customerSatisfactionSurveyItem._self", "en-US", "Customer Satisfaction Survey Item Information", "实体名称"),
            // entity.customerSatisfactionSurveyItem._self
            new TranslationSeedItem("entity.customerSatisfactionSurveyItem._self", "ja-JP", "客户满意度调查项目明细信息", "实体名称"),
            // entity.customerSatisfactionSurveyItem._self
            new TranslationSeedItem("entity.customerSatisfactionSurveyItem._self", "zh-CN", "客户满意度调查项目明细信息", "实体名称"),
            // entity.customerSatisfactionSurveyItem._self
            new TranslationSeedItem("entity.customerSatisfactionSurveyItem._self", "zh-HK", "客户满意度调查项目明细信息", "实体名称"),

            // entity.customerSatisfactionSurveyItem.surveyid
            new TranslationSeedItem("entity.customerSatisfactionSurveyItem.surveyid", "en-US", "调查表ID", "调查表ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.customerSatisfactionSurveyItem.surveyid
            new TranslationSeedItem("entity.customerSatisfactionSurveyItem.surveyid", "ja-JP", "调查表ID", "调查表ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.customerSatisfactionSurveyItem.surveyid
            new TranslationSeedItem("entity.customerSatisfactionSurveyItem.surveyid", "zh-CN", "调查表ID", "调查表ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.customerSatisfactionSurveyItem.surveyid
            new TranslationSeedItem("entity.customerSatisfactionSurveyItem.surveyid", "zh-HK", "调查表ID", "调查表ID（主子表关系，序列化为string以避免Javascript精度问题）"),

            // entity.customerSatisfactionSurveyItem.customersatisfactionsurveycode
            new TranslationSeedItem("entity.customerSatisfactionSurveyItem.customersatisfactionsurveycode", "en-US", "调查表编号", "调查表编号（冗余字段，便于查询）"),
            // entity.customerSatisfactionSurveyItem.customersatisfactionsurveycode
            new TranslationSeedItem("entity.customerSatisfactionSurveyItem.customersatisfactionsurveycode", "ja-JP", "调查表编号", "调查表编号（冗余字段，便于查询）"),
            // entity.customerSatisfactionSurveyItem.customersatisfactionsurveycode
            new TranslationSeedItem("entity.customerSatisfactionSurveyItem.customersatisfactionsurveycode", "zh-CN", "调查表编号", "调查表编号（冗余字段，便于查询）"),
            // entity.customerSatisfactionSurveyItem.customersatisfactionsurveycode
            new TranslationSeedItem("entity.customerSatisfactionSurveyItem.customersatisfactionsurveycode", "zh-HK", "调查表编号", "调查表编号（冗余字段，便于查询）"),

            // entity.customerSatisfactionSurveyItem.linenumber
            new TranslationSeedItem("entity.customerSatisfactionSurveyItem.linenumber", "en-US", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.customerSatisfactionSurveyItem.linenumber
            new TranslationSeedItem("entity.customerSatisfactionSurveyItem.linenumber", "ja-JP", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.customerSatisfactionSurveyItem.linenumber
            new TranslationSeedItem("entity.customerSatisfactionSurveyItem.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.customerSatisfactionSurveyItem.linenumber
            new TranslationSeedItem("entity.customerSatisfactionSurveyItem.linenumber", "zh-HK", "行号", "行号（项号/序号，固定步长=10）"),

            // entity.customerSatisfactionSurveyItem.categorytype
            new TranslationSeedItem("entity.customerSatisfactionSurveyItem.categorytype", "en-US", "调查类别", "调查类别类型（0=产品质量，1=交付服务，2=售后服务，3=技术支持，4=价格，5=其他）"),
            // entity.customerSatisfactionSurveyItem.categorytype
            new TranslationSeedItem("entity.customerSatisfactionSurveyItem.categorytype", "ja-JP", "调查类别", "调查类别类型（0=产品质量，1=交付服务，2=售后服务，3=技术支持，4=价格，5=其他）"),
            // entity.customerSatisfactionSurveyItem.categorytype
            new TranslationSeedItem("entity.customerSatisfactionSurveyItem.categorytype", "zh-CN", "调查类别", "调查类别类型（0=产品质量，1=交付服务，2=售后服务，3=技术支持，4=价格，5=其他）"),
            // entity.customerSatisfactionSurveyItem.categorytype
            new TranslationSeedItem("entity.customerSatisfactionSurveyItem.categorytype", "zh-HK", "调查类别", "调查类别类型（0=产品质量，1=交付服务，2=售后服务，3=技术支持，4=价格，5=其他）"),

            // entity.customerSatisfactionSurveyItem.itemname
            new TranslationSeedItem("entity.customerSatisfactionSurveyItem.itemname", "en-US", "调查项目", "调查项目名称"),
            // entity.customerSatisfactionSurveyItem.itemname
            new TranslationSeedItem("entity.customerSatisfactionSurveyItem.itemname", "ja-JP", "调查项目", "调查项目名称"),
            // entity.customerSatisfactionSurveyItem.itemname
            new TranslationSeedItem("entity.customerSatisfactionSurveyItem.itemname", "zh-CN", "调查项目", "调查项目名称"),
            // entity.customerSatisfactionSurveyItem.itemname
            new TranslationSeedItem("entity.customerSatisfactionSurveyItem.itemname", "zh-HK", "调查项目", "调查项目名称"),

            // entity.customerSatisfactionSurveyItem.itemdescription
            new TranslationSeedItem("entity.customerSatisfactionSurveyItem.itemdescription", "en-US", "项目说明", "调查项目说明"),
            // entity.customerSatisfactionSurveyItem.itemdescription
            new TranslationSeedItem("entity.customerSatisfactionSurveyItem.itemdescription", "ja-JP", "项目说明", "调查项目说明"),
            // entity.customerSatisfactionSurveyItem.itemdescription
            new TranslationSeedItem("entity.customerSatisfactionSurveyItem.itemdescription", "zh-CN", "项目说明", "调查项目说明"),
            // entity.customerSatisfactionSurveyItem.itemdescription
            new TranslationSeedItem("entity.customerSatisfactionSurveyItem.itemdescription", "zh-HK", "项目说明", "调查项目说明"),

            // entity.customerSatisfactionSurveyItem.weight
            new TranslationSeedItem("entity.customerSatisfactionSurveyItem.weight", "en-US", "权重", "权重（%）"),
            // entity.customerSatisfactionSurveyItem.weight
            new TranslationSeedItem("entity.customerSatisfactionSurveyItem.weight", "ja-JP", "权重", "权重（%）"),
            // entity.customerSatisfactionSurveyItem.weight
            new TranslationSeedItem("entity.customerSatisfactionSurveyItem.weight", "zh-CN", "权重", "权重（%）"),
            // entity.customerSatisfactionSurveyItem.weight
            new TranslationSeedItem("entity.customerSatisfactionSurveyItem.weight", "zh-HK", "权重", "权重（%）"),

            // entity.customerSatisfactionSurveyItem.score
            new TranslationSeedItem("entity.customerSatisfactionSurveyItem.score", "en-US", "评分", "评分（0-100分）"),
            // entity.customerSatisfactionSurveyItem.score
            new TranslationSeedItem("entity.customerSatisfactionSurveyItem.score", "ja-JP", "评分", "评分（0-100分）"),
            // entity.customerSatisfactionSurveyItem.score
            new TranslationSeedItem("entity.customerSatisfactionSurveyItem.score", "zh-CN", "评分", "评分（0-100分）"),
            // entity.customerSatisfactionSurveyItem.score
            new TranslationSeedItem("entity.customerSatisfactionSurveyItem.score", "zh-HK", "评分", "评分（0-100分）"),

            // entity.customerSatisfactionSurveyItem.satisfactionlevel
            new TranslationSeedItem("entity.customerSatisfactionSurveyItem.satisfactionlevel", "en-US", "满意度等级", "满意度等级（0=非常不满意，1=不满意，2=一般，3=满意，4=非常满意）"),
            // entity.customerSatisfactionSurveyItem.satisfactionlevel
            new TranslationSeedItem("entity.customerSatisfactionSurveyItem.satisfactionlevel", "ja-JP", "满意度等级", "满意度等级（0=非常不满意，1=不满意，2=一般，3=满意，4=非常满意）"),
            // entity.customerSatisfactionSurveyItem.satisfactionlevel
            new TranslationSeedItem("entity.customerSatisfactionSurveyItem.satisfactionlevel", "zh-CN", "满意度等级", "满意度等级（0=非常不满意，1=不满意，2=一般，3=满意，4=非常满意）"),
            // entity.customerSatisfactionSurveyItem.satisfactionlevel
            new TranslationSeedItem("entity.customerSatisfactionSurveyItem.satisfactionlevel", "zh-HK", "满意度等级", "满意度等级（0=非常不满意，1=不满意，2=一般，3=满意，4=非常满意）"),

            // entity.customerSatisfactionSurveyItem.customerfeedback
            new TranslationSeedItem("entity.customerSatisfactionSurveyItem.customerfeedback", "en-US", "客户反馈", "客户反馈/意见"),
            // entity.customerSatisfactionSurveyItem.customerfeedback
            new TranslationSeedItem("entity.customerSatisfactionSurveyItem.customerfeedback", "ja-JP", "客户反馈", "客户反馈/意见"),
            // entity.customerSatisfactionSurveyItem.customerfeedback
            new TranslationSeedItem("entity.customerSatisfactionSurveyItem.customerfeedback", "zh-CN", "客户反馈", "客户反馈/意见"),
            // entity.customerSatisfactionSurveyItem.customerfeedback
            new TranslationSeedItem("entity.customerSatisfactionSurveyItem.customerfeedback", "zh-HK", "客户反馈", "客户反馈/意见"),

            // entity.customerSatisfactionSurveyItem.improvementsuggestion
            new TranslationSeedItem("entity.customerSatisfactionSurveyItem.improvementsuggestion", "en-US", "改进建议", "改进建议"),
            // entity.customerSatisfactionSurveyItem.improvementsuggestion
            new TranslationSeedItem("entity.customerSatisfactionSurveyItem.improvementsuggestion", "ja-JP", "改进建议", "改进建议"),
            // entity.customerSatisfactionSurveyItem.improvementsuggestion
            new TranslationSeedItem("entity.customerSatisfactionSurveyItem.improvementsuggestion", "zh-CN", "改进建议", "改进建议"),
            // entity.customerSatisfactionSurveyItem.improvementsuggestion
            new TranslationSeedItem("entity.customerSatisfactionSurveyItem.improvementsuggestion", "zh-HK", "改进建议", "改进建议"),

            // entity.customerSatisfactionSurveyItem.followupaction
            new TranslationSeedItem("entity.customerSatisfactionSurveyItem.followupaction", "en-US", "跟进措施", "跟进措施"),
            // entity.customerSatisfactionSurveyItem.followupaction
            new TranslationSeedItem("entity.customerSatisfactionSurveyItem.followupaction", "ja-JP", "跟进措施", "跟进措施"),
            // entity.customerSatisfactionSurveyItem.followupaction
            new TranslationSeedItem("entity.customerSatisfactionSurveyItem.followupaction", "zh-CN", "跟进措施", "跟进措施"),
            // entity.customerSatisfactionSurveyItem.followupaction
            new TranslationSeedItem("entity.customerSatisfactionSurveyItem.followupaction", "zh-HK", "跟进措施", "跟进措施"),

            // entity.customerSatisfactionSurveyItem.followupstatus
            new TranslationSeedItem("entity.customerSatisfactionSurveyItem.followupstatus", "en-US", "跟进状态", "跟进状态（0=无需跟进，1=待跟进，2=跟进中，3=已完成）"),
            // entity.customerSatisfactionSurveyItem.followupstatus
            new TranslationSeedItem("entity.customerSatisfactionSurveyItem.followupstatus", "ja-JP", "跟进状态", "跟进状态（0=无需跟进，1=待跟进，2=跟进中，3=已完成）"),
            // entity.customerSatisfactionSurveyItem.followupstatus
            new TranslationSeedItem("entity.customerSatisfactionSurveyItem.followupstatus", "zh-CN", "跟进状态", "跟进状态（0=无需跟进，1=待跟进，2=跟进中，3=已完成）"),
            // entity.customerSatisfactionSurveyItem.followupstatus
            new TranslationSeedItem("entity.customerSatisfactionSurveyItem.followupstatus", "zh-HK", "跟进状态", "跟进状态（0=无需跟进，1=待跟进，2=跟进中，3=已完成）"),

            // entity.customerSatisfactionSurveyItem.survey
            new TranslationSeedItem("entity.customerSatisfactionSurveyItem.survey", "en-US", "调查表主表", "调查表主表"),
            // entity.customerSatisfactionSurveyItem.survey
            new TranslationSeedItem("entity.customerSatisfactionSurveyItem.survey", "ja-JP", "调查表主表", "调查表主表"),
            // entity.customerSatisfactionSurveyItem.survey
            new TranslationSeedItem("entity.customerSatisfactionSurveyItem.survey", "zh-CN", "调查表主表", "调查表主表"),
            // entity.customerSatisfactionSurveyItem.survey
            new TranslationSeedItem("entity.customerSatisfactionSurveyItem.survey", "zh-HK", "调查表主表", "调查表主表"),
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
