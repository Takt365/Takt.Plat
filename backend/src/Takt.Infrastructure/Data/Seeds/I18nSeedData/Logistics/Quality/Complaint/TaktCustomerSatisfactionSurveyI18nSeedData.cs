// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Complaint
// 文件名称：TaktCustomerSatisfactionSurveyI18nSeedData.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktCustomerSatisfactionSurvey 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktCustomerSatisfactionSurvey 实体国际化翻译种子（键前缀 entity.customerSatisfactionSurvey.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktCustomerSatisfactionSurveyI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktCustomerSatisfactionSurvey 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 customerSatisfactionSurvey 实体翻译...", tenantCode);

        foreach (var item in GetCustomerSatisfactionSurveyTranslations())
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

        TaktLogger.Information("TaktCustomerSatisfactionSurvey 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktCustomerSatisfactionSurvey 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.customerSatisfactionSurvey._self / entity.customerSatisfactionSurvey.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetCustomerSatisfactionSurveyTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.customerSatisfactionSurvey._self
            new TranslationSeedItem("entity.customerSatisfactionSurvey._self", "en-US", "Customer Satisfaction Survey Information", "实体名称"),
            // entity.customerSatisfactionSurvey._self
            new TranslationSeedItem("entity.customerSatisfactionSurvey._self", "ja-JP", "客户满意度调查表主表信息", "实体名称"),
            // entity.customerSatisfactionSurvey._self
            new TranslationSeedItem("entity.customerSatisfactionSurvey._self", "zh-CN", "客户满意度调查表主表信息", "实体名称"),
            // entity.customerSatisfactionSurvey._self
            new TranslationSeedItem("entity.customerSatisfactionSurvey._self", "zh-HK", "客户满意度调查表主表信息", "实体名称"),

            // entity.customerSatisfactionSurvey.code
            new TranslationSeedItem("entity.customerSatisfactionSurvey.code", "en-US", "调查表编号", "调查表编号（组合唯一索引）"),
            // entity.customerSatisfactionSurvey.code
            new TranslationSeedItem("entity.customerSatisfactionSurvey.code", "ja-JP", "调查表编号", "调查表编号（组合唯一索引）"),
            // entity.customerSatisfactionSurvey.code
            new TranslationSeedItem("entity.customerSatisfactionSurvey.code", "zh-CN", "调查表编号", "调查表编号（组合唯一索引）"),
            // entity.customerSatisfactionSurvey.code
            new TranslationSeedItem("entity.customerSatisfactionSurvey.code", "zh-HK", "调查表编号", "调查表编号（组合唯一索引）"),

            // entity.customerSatisfactionSurvey.customerid
            new TranslationSeedItem("entity.customerSatisfactionSurvey.customerid", "en-US", "客户ID", "客户ID（序列化为string以避免Javascript精度问题）"),
            // entity.customerSatisfactionSurvey.customerid
            new TranslationSeedItem("entity.customerSatisfactionSurvey.customerid", "ja-JP", "客户ID", "客户ID（序列化为string以避免Javascript精度问题）"),
            // entity.customerSatisfactionSurvey.customerid
            new TranslationSeedItem("entity.customerSatisfactionSurvey.customerid", "zh-CN", "客户ID", "客户ID（序列化为string以避免Javascript精度问题）"),
            // entity.customerSatisfactionSurvey.customerid
            new TranslationSeedItem("entity.customerSatisfactionSurvey.customerid", "zh-HK", "客户ID", "客户ID（序列化为string以避免Javascript精度问题）"),

            // entity.customerSatisfactionSurvey.customername
            new TranslationSeedItem("entity.customerSatisfactionSurvey.customername", "en-US", "客户名称", "客户名称"),
            // entity.customerSatisfactionSurvey.customername
            new TranslationSeedItem("entity.customerSatisfactionSurvey.customername", "ja-JP", "客户名称", "客户名称"),
            // entity.customerSatisfactionSurvey.customername
            new TranslationSeedItem("entity.customerSatisfactionSurvey.customername", "zh-CN", "客户名称", "客户名称"),
            // entity.customerSatisfactionSurvey.customername
            new TranslationSeedItem("entity.customerSatisfactionSurvey.customername", "zh-HK", "客户名称", "客户名称"),

            // entity.customerSatisfactionSurvey.customercode
            new TranslationSeedItem("entity.customerSatisfactionSurvey.customercode", "en-US", "客户编码", "客户编码"),
            // entity.customerSatisfactionSurvey.customercode
            new TranslationSeedItem("entity.customerSatisfactionSurvey.customercode", "ja-JP", "客户编码", "客户编码"),
            // entity.customerSatisfactionSurvey.customercode
            new TranslationSeedItem("entity.customerSatisfactionSurvey.customercode", "zh-CN", "客户编码", "客户编码"),
            // entity.customerSatisfactionSurvey.customercode
            new TranslationSeedItem("entity.customerSatisfactionSurvey.customercode", "zh-HK", "客户编码", "客户编码"),

            // entity.customerSatisfactionSurvey.surveydate
            new TranslationSeedItem("entity.customerSatisfactionSurvey.surveydate", "en-US", "调查日期", "调查日期"),
            // entity.customerSatisfactionSurvey.surveydate
            new TranslationSeedItem("entity.customerSatisfactionSurvey.surveydate", "ja-JP", "调查日期", "调查日期"),
            // entity.customerSatisfactionSurvey.surveydate
            new TranslationSeedItem("entity.customerSatisfactionSurvey.surveydate", "zh-CN", "调查日期", "调查日期"),
            // entity.customerSatisfactionSurvey.surveydate
            new TranslationSeedItem("entity.customerSatisfactionSurvey.surveydate", "zh-HK", "调查日期", "调查日期"),

            // entity.customerSatisfactionSurvey.surveymethod
            new TranslationSeedItem("entity.customerSatisfactionSurvey.surveymethod", "en-US", "调查方式", "调查方式（0=问卷，1=电话，2=邮件，3=现场，4=在线）"),
            // entity.customerSatisfactionSurvey.surveymethod
            new TranslationSeedItem("entity.customerSatisfactionSurvey.surveymethod", "ja-JP", "调查方式", "调查方式（0=问卷，1=电话，2=邮件，3=现场，4=在线）"),
            // entity.customerSatisfactionSurvey.surveymethod
            new TranslationSeedItem("entity.customerSatisfactionSurvey.surveymethod", "zh-CN", "调查方式", "调查方式（0=问卷，1=电话，2=邮件，3=现场，4=在线）"),
            // entity.customerSatisfactionSurvey.surveymethod
            new TranslationSeedItem("entity.customerSatisfactionSurvey.surveymethod", "zh-HK", "调查方式", "调查方式（0=问卷，1=电话，2=邮件，3=现场，4=在线）"),

            // entity.customerSatisfactionSurvey.surveytype
            new TranslationSeedItem("entity.customerSatisfactionSurvey.surveytype", "en-US", "调查类型", "调查类型（0=定期调查，1=专项调查，2=投诉后调查，3=其他）"),
            // entity.customerSatisfactionSurvey.surveytype
            new TranslationSeedItem("entity.customerSatisfactionSurvey.surveytype", "ja-JP", "调查类型", "调查类型（0=定期调查，1=专项调查，2=投诉后调查，3=其他）"),
            // entity.customerSatisfactionSurvey.surveytype
            new TranslationSeedItem("entity.customerSatisfactionSurvey.surveytype", "zh-CN", "调查类型", "调查类型（0=定期调查，1=专项调查，2=投诉后调查，3=其他）"),
            // entity.customerSatisfactionSurvey.surveytype
            new TranslationSeedItem("entity.customerSatisfactionSurvey.surveytype", "zh-HK", "调查类型", "调查类型（0=定期调查，1=专项调查，2=投诉后调查，3=其他）"),

            // entity.customerSatisfactionSurvey.surveyperiod
            new TranslationSeedItem("entity.customerSatisfactionSurvey.surveyperiod", "en-US", "调查周期", "调查周期（0=月度，1=季度，2=半年度，3=年度）"),
            // entity.customerSatisfactionSurvey.surveyperiod
            new TranslationSeedItem("entity.customerSatisfactionSurvey.surveyperiod", "ja-JP", "调查周期", "调查周期（0=月度，1=季度，2=半年度，3=年度）"),
            // entity.customerSatisfactionSurvey.surveyperiod
            new TranslationSeedItem("entity.customerSatisfactionSurvey.surveyperiod", "zh-CN", "调查周期", "调查周期（0=月度，1=季度，2=半年度，3=年度）"),
            // entity.customerSatisfactionSurvey.surveyperiod
            new TranslationSeedItem("entity.customerSatisfactionSurvey.surveyperiod", "zh-HK", "调查周期", "调查周期（0=月度，1=季度，2=半年度，3=年度）"),

            // entity.customerSatisfactionSurvey.surveyorby
            new TranslationSeedItem("entity.customerSatisfactionSurvey.surveyorby", "en-US", "调查人", "调查人（人员代码）"),
            // entity.customerSatisfactionSurvey.surveyorby
            new TranslationSeedItem("entity.customerSatisfactionSurvey.surveyorby", "ja-JP", "调查人", "调查人（人员代码）"),
            // entity.customerSatisfactionSurvey.surveyorby
            new TranslationSeedItem("entity.customerSatisfactionSurvey.surveyorby", "zh-CN", "调查人", "调查人（人员代码）"),
            // entity.customerSatisfactionSurvey.surveyorby
            new TranslationSeedItem("entity.customerSatisfactionSurvey.surveyorby", "zh-HK", "调查人", "调查人（人员代码）"),

            // entity.customerSatisfactionSurvey.customercontact
            new TranslationSeedItem("entity.customerSatisfactionSurvey.customercontact", "en-US", "客户联系人", "客户联系人"),
            // entity.customerSatisfactionSurvey.customercontact
            new TranslationSeedItem("entity.customerSatisfactionSurvey.customercontact", "ja-JP", "客户联系人", "客户联系人"),
            // entity.customerSatisfactionSurvey.customercontact
            new TranslationSeedItem("entity.customerSatisfactionSurvey.customercontact", "zh-CN", "客户联系人", "客户联系人"),
            // entity.customerSatisfactionSurvey.customercontact
            new TranslationSeedItem("entity.customerSatisfactionSurvey.customercontact", "zh-HK", "客户联系人", "客户联系人"),

            // entity.customerSatisfactionSurvey.customerphone
            new TranslationSeedItem("entity.customerSatisfactionSurvey.customerphone", "en-US", "客户联系电话", "客户联系电话"),
            // entity.customerSatisfactionSurvey.customerphone
            new TranslationSeedItem("entity.customerSatisfactionSurvey.customerphone", "ja-JP", "客户联系电话", "客户联系电话"),
            // entity.customerSatisfactionSurvey.customerphone
            new TranslationSeedItem("entity.customerSatisfactionSurvey.customerphone", "zh-CN", "客户联系电话", "客户联系电话"),
            // entity.customerSatisfactionSurvey.customerphone
            new TranslationSeedItem("entity.customerSatisfactionSurvey.customerphone", "zh-HK", "客户联系电话", "客户联系电话"),

            // entity.customerSatisfactionSurvey.overallsatisfaction
            new TranslationSeedItem("entity.customerSatisfactionSurvey.overallsatisfaction", "en-US", "整体满意度", "整体满意度（0=非常不满意，1=不满意，2=一般，3=满意，4=非常满意）"),
            // entity.customerSatisfactionSurvey.overallsatisfaction
            new TranslationSeedItem("entity.customerSatisfactionSurvey.overallsatisfaction", "ja-JP", "整体满意度", "整体满意度（0=非常不满意，1=不满意，2=一般，3=满意，4=非常满意）"),
            // entity.customerSatisfactionSurvey.overallsatisfaction
            new TranslationSeedItem("entity.customerSatisfactionSurvey.overallsatisfaction", "zh-CN", "整体满意度", "整体满意度（0=非常不满意，1=不满意，2=一般，3=满意，4=非常满意）"),
            // entity.customerSatisfactionSurvey.overallsatisfaction
            new TranslationSeedItem("entity.customerSatisfactionSurvey.overallsatisfaction", "zh-HK", "整体满意度", "整体满意度（0=非常不满意，1=不满意，2=一般，3=满意，4=非常满意）"),

            // entity.customerSatisfactionSurvey.totalscore
            new TranslationSeedItem("entity.customerSatisfactionSurvey.totalscore", "en-US", "综合评分", "综合评分（0-100分）"),
            // entity.customerSatisfactionSurvey.totalscore
            new TranslationSeedItem("entity.customerSatisfactionSurvey.totalscore", "ja-JP", "综合评分", "综合评分（0-100分）"),
            // entity.customerSatisfactionSurvey.totalscore
            new TranslationSeedItem("entity.customerSatisfactionSurvey.totalscore", "zh-CN", "综合评分", "综合评分（0-100分）"),
            // entity.customerSatisfactionSurvey.totalscore
            new TranslationSeedItem("entity.customerSatisfactionSurvey.totalscore", "zh-HK", "综合评分", "综合评分（0-100分）"),

            // entity.customerSatisfactionSurvey.qualityscore
            new TranslationSeedItem("entity.customerSatisfactionSurvey.qualityscore", "en-US", "产品质量评分", "产品质量评分（0-100分）"),
            // entity.customerSatisfactionSurvey.qualityscore
            new TranslationSeedItem("entity.customerSatisfactionSurvey.qualityscore", "ja-JP", "产品质量评分", "产品质量评分（0-100分）"),
            // entity.customerSatisfactionSurvey.qualityscore
            new TranslationSeedItem("entity.customerSatisfactionSurvey.qualityscore", "zh-CN", "产品质量评分", "产品质量评分（0-100分）"),
            // entity.customerSatisfactionSurvey.qualityscore
            new TranslationSeedItem("entity.customerSatisfactionSurvey.qualityscore", "zh-HK", "产品质量评分", "产品质量评分（0-100分）"),

            // entity.customerSatisfactionSurvey.deliveryscore
            new TranslationSeedItem("entity.customerSatisfactionSurvey.deliveryscore", "en-US", "交付准时率评分", "交付准时率评分（0-100分）"),
            // entity.customerSatisfactionSurvey.deliveryscore
            new TranslationSeedItem("entity.customerSatisfactionSurvey.deliveryscore", "ja-JP", "交付准时率评分", "交付准时率评分（0-100分）"),
            // entity.customerSatisfactionSurvey.deliveryscore
            new TranslationSeedItem("entity.customerSatisfactionSurvey.deliveryscore", "zh-CN", "交付准时率评分", "交付准时率评分（0-100分）"),
            // entity.customerSatisfactionSurvey.deliveryscore
            new TranslationSeedItem("entity.customerSatisfactionSurvey.deliveryscore", "zh-HK", "交付准时率评分", "交付准时率评分（0-100分）"),

            // entity.customerSatisfactionSurvey.servicescore
            new TranslationSeedItem("entity.customerSatisfactionSurvey.servicescore", "en-US", "服务质量评分", "服务质量评分（0-100分）"),
            // entity.customerSatisfactionSurvey.servicescore
            new TranslationSeedItem("entity.customerSatisfactionSurvey.servicescore", "ja-JP", "服务质量评分", "服务质量评分（0-100分）"),
            // entity.customerSatisfactionSurvey.servicescore
            new TranslationSeedItem("entity.customerSatisfactionSurvey.servicescore", "zh-CN", "服务质量评分", "服务质量评分（0-100分）"),
            // entity.customerSatisfactionSurvey.servicescore
            new TranslationSeedItem("entity.customerSatisfactionSurvey.servicescore", "zh-HK", "服务质量评分", "服务质量评分（0-100分）"),

            // entity.customerSatisfactionSurvey.pricescore
            new TranslationSeedItem("entity.customerSatisfactionSurvey.pricescore", "en-US", "价格竞争力评分", "价格竞争力评分（0-100分）"),
            // entity.customerSatisfactionSurvey.pricescore
            new TranslationSeedItem("entity.customerSatisfactionSurvey.pricescore", "ja-JP", "价格竞争力评分", "价格竞争力评分（0-100分）"),
            // entity.customerSatisfactionSurvey.pricescore
            new TranslationSeedItem("entity.customerSatisfactionSurvey.pricescore", "zh-CN", "价格竞争力评分", "价格竞争力评分（0-100分）"),
            // entity.customerSatisfactionSurvey.pricescore
            new TranslationSeedItem("entity.customerSatisfactionSurvey.pricescore", "zh-HK", "价格竞争力评分", "价格竞争力评分（0-100分）"),

            // entity.customerSatisfactionSurvey.technicalscore
            new TranslationSeedItem("entity.customerSatisfactionSurvey.technicalscore", "en-US", "技术支持评分", "技术支持评分（0-100分）"),
            // entity.customerSatisfactionSurvey.technicalscore
            new TranslationSeedItem("entity.customerSatisfactionSurvey.technicalscore", "ja-JP", "技术支持评分", "技术支持评分（0-100分）"),
            // entity.customerSatisfactionSurvey.technicalscore
            new TranslationSeedItem("entity.customerSatisfactionSurvey.technicalscore", "zh-CN", "技术支持评分", "技术支持评分（0-100分）"),
            // entity.customerSatisfactionSurvey.technicalscore
            new TranslationSeedItem("entity.customerSatisfactionSurvey.technicalscore", "zh-HK", "技术支持评分", "技术支持评分（0-100分）"),

            // entity.customerSatisfactionSurvey.customerpraise
            new TranslationSeedItem("entity.customerSatisfactionSurvey.customerpraise", "en-US", "客户主要表扬", "客户主要表扬"),
            // entity.customerSatisfactionSurvey.customerpraise
            new TranslationSeedItem("entity.customerSatisfactionSurvey.customerpraise", "ja-JP", "客户主要表扬", "客户主要表扬"),
            // entity.customerSatisfactionSurvey.customerpraise
            new TranslationSeedItem("entity.customerSatisfactionSurvey.customerpraise", "zh-CN", "客户主要表扬", "客户主要表扬"),
            // entity.customerSatisfactionSurvey.customerpraise
            new TranslationSeedItem("entity.customerSatisfactionSurvey.customerpraise", "zh-HK", "客户主要表扬", "客户主要表扬"),

            // entity.customerSatisfactionSurvey.customerfeedback
            new TranslationSeedItem("entity.customerSatisfactionSurvey.customerfeedback", "en-US", "客户意见", "客户主要意见/建议"),
            // entity.customerSatisfactionSurvey.customerfeedback
            new TranslationSeedItem("entity.customerSatisfactionSurvey.customerfeedback", "ja-JP", "客户意见", "客户主要意见/建议"),
            // entity.customerSatisfactionSurvey.customerfeedback
            new TranslationSeedItem("entity.customerSatisfactionSurvey.customerfeedback", "zh-CN", "客户意见", "客户主要意见/建议"),
            // entity.customerSatisfactionSurvey.customerfeedback
            new TranslationSeedItem("entity.customerSatisfactionSurvey.customerfeedback", "zh-HK", "客户意见", "客户主要意见/建议"),

            // entity.customerSatisfactionSurvey.improvementplan
            new TranslationSeedItem("entity.customerSatisfactionSurvey.improvementplan", "en-US", "改进计划", "改进计划/措施"),
            // entity.customerSatisfactionSurvey.improvementplan
            new TranslationSeedItem("entity.customerSatisfactionSurvey.improvementplan", "ja-JP", "改进计划", "改进计划/措施"),
            // entity.customerSatisfactionSurvey.improvementplan
            new TranslationSeedItem("entity.customerSatisfactionSurvey.improvementplan", "zh-CN", "改进计划", "改进计划/措施"),
            // entity.customerSatisfactionSurvey.improvementplan
            new TranslationSeedItem("entity.customerSatisfactionSurvey.improvementplan", "zh-HK", "改进计划", "改进计划/措施"),

            // entity.customerSatisfactionSurvey.surveystatus
            new TranslationSeedItem("entity.customerSatisfactionSurvey.surveystatus", "en-US", "调查状态", "调查状态（0=草稿，1=进行中，2=已完成，3=已归档）"),
            // entity.customerSatisfactionSurvey.surveystatus
            new TranslationSeedItem("entity.customerSatisfactionSurvey.surveystatus", "ja-JP", "调查状态", "调查状态（0=草稿，1=进行中，2=已完成，3=已归档）"),
            // entity.customerSatisfactionSurvey.surveystatus
            new TranslationSeedItem("entity.customerSatisfactionSurvey.surveystatus", "zh-CN", "调查状态", "调查状态（0=草稿，1=进行中，2=已完成，3=已归档）"),
            // entity.customerSatisfactionSurvey.surveystatus
            new TranslationSeedItem("entity.customerSatisfactionSurvey.surveystatus", "zh-HK", "调查状态", "调查状态（0=草稿，1=进行中，2=已完成，3=已归档）"),

            // entity.customerSatisfactionSurvey.followupstatus
            new TranslationSeedItem("entity.customerSatisfactionSurvey.followupstatus", "en-US", "跟进状态", "跟进状态（0=无需跟进，1=待跟进，2=跟进中，3=已完成）"),
            // entity.customerSatisfactionSurvey.followupstatus
            new TranslationSeedItem("entity.customerSatisfactionSurvey.followupstatus", "ja-JP", "跟进状态", "跟进状态（0=无需跟进，1=待跟进，2=跟进中，3=已完成）"),
            // entity.customerSatisfactionSurvey.followupstatus
            new TranslationSeedItem("entity.customerSatisfactionSurvey.followupstatus", "zh-CN", "跟进状态", "跟进状态（0=无需跟进，1=待跟进，2=跟进中，3=已完成）"),
            // entity.customerSatisfactionSurvey.followupstatus
            new TranslationSeedItem("entity.customerSatisfactionSurvey.followupstatus", "zh-HK", "跟进状态", "跟进状态（0=无需跟进，1=待跟进，2=跟进中，3=已完成）"),

            // entity.customerSatisfactionSurvey.relatedcomplaintid
            new TranslationSeedItem("entity.customerSatisfactionSurvey.relatedcomplaintid", "en-US", "关联客诉ID", "关联客诉ID（序列化为string以避免Javascript精度问题）"),
            // entity.customerSatisfactionSurvey.relatedcomplaintid
            new TranslationSeedItem("entity.customerSatisfactionSurvey.relatedcomplaintid", "ja-JP", "关联客诉ID", "关联客诉ID（序列化为string以避免Javascript精度问题）"),
            // entity.customerSatisfactionSurvey.relatedcomplaintid
            new TranslationSeedItem("entity.customerSatisfactionSurvey.relatedcomplaintid", "zh-CN", "关联客诉ID", "关联客诉ID（序列化为string以避免Javascript精度问题）"),
            // entity.customerSatisfactionSurvey.relatedcomplaintid
            new TranslationSeedItem("entity.customerSatisfactionSurvey.relatedcomplaintid", "zh-HK", "关联客诉ID", "关联客诉ID（序列化为string以避免Javascript精度问题）"),

            // entity.customerSatisfactionSurvey.relatedplant
            new TranslationSeedItem("entity.customerSatisfactionSurvey.relatedplant", "en-US", "关联工厂", "关联工厂"),
            // entity.customerSatisfactionSurvey.relatedplant
            new TranslationSeedItem("entity.customerSatisfactionSurvey.relatedplant", "ja-JP", "关联工厂", "关联工厂"),
            // entity.customerSatisfactionSurvey.relatedplant
            new TranslationSeedItem("entity.customerSatisfactionSurvey.relatedplant", "zh-CN", "关联工厂", "关联工厂"),
            // entity.customerSatisfactionSurvey.relatedplant
            new TranslationSeedItem("entity.customerSatisfactionSurvey.relatedplant", "zh-HK", "关联工厂", "关联工厂"),

            // entity.customerSatisfactionSurvey.sortorder
            new TranslationSeedItem("entity.customerSatisfactionSurvey.sortorder", "en-US", "排序号", "排序号（越小越靠前）"),
            // entity.customerSatisfactionSurvey.sortorder
            new TranslationSeedItem("entity.customerSatisfactionSurvey.sortorder", "ja-JP", "排序号", "排序号（越小越靠前）"),
            // entity.customerSatisfactionSurvey.sortorder
            new TranslationSeedItem("entity.customerSatisfactionSurvey.sortorder", "zh-CN", "排序号", "排序号（越小越靠前）"),
            // entity.customerSatisfactionSurvey.sortorder
            new TranslationSeedItem("entity.customerSatisfactionSurvey.sortorder", "zh-HK", "排序号", "排序号（越小越靠前）"),

            // entity.customerSatisfactionSurvey.items
            new TranslationSeedItem("entity.customerSatisfactionSurvey.items", "en-US", "items", "调查项目明细列表（主子表关系）"),
            // entity.customerSatisfactionSurvey.items
            new TranslationSeedItem("entity.customerSatisfactionSurvey.items", "ja-JP", "items", "调查项目明细列表（主子表关系）"),
            // entity.customerSatisfactionSurvey.items
            new TranslationSeedItem("entity.customerSatisfactionSurvey.items", "zh-CN", "items", "调查项目明细列表（主子表关系）"),
            // entity.customerSatisfactionSurvey.items
            new TranslationSeedItem("entity.customerSatisfactionSurvey.items", "zh-HK", "items", "调查项目明细列表（主子表关系）"),
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
