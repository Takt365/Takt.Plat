// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Complaint
// 文件名称：TaktSupplierEvaluationI18nSeedData.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSupplierEvaluation 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktSupplierEvaluation 实体国际化翻译种子（键前缀 entity.supplierEvaluation.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSupplierEvaluationI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSupplierEvaluation 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 supplierEvaluation 实体翻译...", tenantCode);

        foreach (var item in GetSupplierEvaluationTranslations())
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

        TaktLogger.Information("TaktSupplierEvaluation 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSupplierEvaluation 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.supplierEvaluation._self / entity.supplierEvaluation.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSupplierEvaluationTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.supplierEvaluation._self
            new TranslationSeedItem("entity.supplierEvaluation._self", "en-US", "Supplier Evaluation Information", "实体名称"),
            // entity.supplierEvaluation._self
            new TranslationSeedItem("entity.supplierEvaluation._self", "ja-JP", "供应商评价考核主表信息", "实体名称"),
            // entity.supplierEvaluation._self
            new TranslationSeedItem("entity.supplierEvaluation._self", "zh-CN", "供应商评价考核主表信息", "实体名称"),
            // entity.supplierEvaluation._self
            new TranslationSeedItem("entity.supplierEvaluation._self", "zh-HK", "供应商评价考核主表信息", "实体名称"),

            // entity.supplierEvaluation.code
            new TranslationSeedItem("entity.supplierEvaluation.code", "en-US", "评价表编号", "评价表编号（组合唯一索引）"),
            // entity.supplierEvaluation.code
            new TranslationSeedItem("entity.supplierEvaluation.code", "ja-JP", "评价表编号", "评价表编号（组合唯一索引）"),
            // entity.supplierEvaluation.code
            new TranslationSeedItem("entity.supplierEvaluation.code", "zh-CN", "评价表编号", "评价表编号（组合唯一索引）"),
            // entity.supplierEvaluation.code
            new TranslationSeedItem("entity.supplierEvaluation.code", "zh-HK", "评价表编号", "评价表编号（组合唯一索引）"),

            // entity.supplierEvaluation.supplierid
            new TranslationSeedItem("entity.supplierEvaluation.supplierid", "en-US", "供应商ID", "供应商ID（序列化为string以避免Javascript精度问题）"),
            // entity.supplierEvaluation.supplierid
            new TranslationSeedItem("entity.supplierEvaluation.supplierid", "ja-JP", "供应商ID", "供应商ID（序列化为string以避免Javascript精度问题）"),
            // entity.supplierEvaluation.supplierid
            new TranslationSeedItem("entity.supplierEvaluation.supplierid", "zh-CN", "供应商ID", "供应商ID（序列化为string以避免Javascript精度问题）"),
            // entity.supplierEvaluation.supplierid
            new TranslationSeedItem("entity.supplierEvaluation.supplierid", "zh-HK", "供应商ID", "供应商ID（序列化为string以避免Javascript精度问题）"),

            // entity.supplierEvaluation.suppliername
            new TranslationSeedItem("entity.supplierEvaluation.suppliername", "en-US", "供应商名称", "供应商名称"),
            // entity.supplierEvaluation.suppliername
            new TranslationSeedItem("entity.supplierEvaluation.suppliername", "ja-JP", "供应商名称", "供应商名称"),
            // entity.supplierEvaluation.suppliername
            new TranslationSeedItem("entity.supplierEvaluation.suppliername", "zh-CN", "供应商名称", "供应商名称"),
            // entity.supplierEvaluation.suppliername
            new TranslationSeedItem("entity.supplierEvaluation.suppliername", "zh-HK", "供应商名称", "供应商名称"),

            // entity.supplierEvaluation.suppliercode
            new TranslationSeedItem("entity.supplierEvaluation.suppliercode", "en-US", "供应商编码", "供应商编码"),
            // entity.supplierEvaluation.suppliercode
            new TranslationSeedItem("entity.supplierEvaluation.suppliercode", "ja-JP", "供应商编码", "供应商编码"),
            // entity.supplierEvaluation.suppliercode
            new TranslationSeedItem("entity.supplierEvaluation.suppliercode", "zh-CN", "供应商编码", "供应商编码"),
            // entity.supplierEvaluation.suppliercode
            new TranslationSeedItem("entity.supplierEvaluation.suppliercode", "zh-HK", "供应商编码", "供应商编码"),

            // entity.supplierEvaluation.evaluationdate
            new TranslationSeedItem("entity.supplierEvaluation.evaluationdate", "en-US", "评价日期", "评价日期"),
            // entity.supplierEvaluation.evaluationdate
            new TranslationSeedItem("entity.supplierEvaluation.evaluationdate", "ja-JP", "评价日期", "评价日期"),
            // entity.supplierEvaluation.evaluationdate
            new TranslationSeedItem("entity.supplierEvaluation.evaluationdate", "zh-CN", "评价日期", "评价日期"),
            // entity.supplierEvaluation.evaluationdate
            new TranslationSeedItem("entity.supplierEvaluation.evaluationdate", "zh-HK", "评价日期", "评价日期"),

            // entity.supplierEvaluation.evaluationperiod
            new TranslationSeedItem("entity.supplierEvaluation.evaluationperiod", "en-US", "评价周期", "评价周期（0=月度，1=季度，2=半年度，3=年度）"),
            // entity.supplierEvaluation.evaluationperiod
            new TranslationSeedItem("entity.supplierEvaluation.evaluationperiod", "ja-JP", "评价周期", "评价周期（0=月度，1=季度，2=半年度，3=年度）"),
            // entity.supplierEvaluation.evaluationperiod
            new TranslationSeedItem("entity.supplierEvaluation.evaluationperiod", "zh-CN", "评价周期", "评价周期（0=月度，1=季度，2=半年度，3=年度）"),
            // entity.supplierEvaluation.evaluationperiod
            new TranslationSeedItem("entity.supplierEvaluation.evaluationperiod", "zh-HK", "评价周期", "评价周期（0=月度，1=季度，2=半年度，3=年度）"),

            // entity.supplierEvaluation.evaluationtype
            new TranslationSeedItem("entity.supplierEvaluation.evaluationtype", "en-US", "评价类型", "评价类型（0=常规评价，1=准入评价，2=年度评审，3=专项评价）"),
            // entity.supplierEvaluation.evaluationtype
            new TranslationSeedItem("entity.supplierEvaluation.evaluationtype", "ja-JP", "评价类型", "评价类型（0=常规评价，1=准入评价，2=年度评审，3=专项评价）"),
            // entity.supplierEvaluation.evaluationtype
            new TranslationSeedItem("entity.supplierEvaluation.evaluationtype", "zh-CN", "评价类型", "评价类型（0=常规评价，1=准入评价，2=年度评审，3=专项评价）"),
            // entity.supplierEvaluation.evaluationtype
            new TranslationSeedItem("entity.supplierEvaluation.evaluationtype", "zh-HK", "评价类型", "评价类型（0=常规评价，1=准入评价，2=年度评审，3=专项评价）"),

            // entity.supplierEvaluation.evaluatorby
            new TranslationSeedItem("entity.supplierEvaluation.evaluatorby", "en-US", "评价人", "评价人（人员代码）"),
            // entity.supplierEvaluation.evaluatorby
            new TranslationSeedItem("entity.supplierEvaluation.evaluatorby", "ja-JP", "评价人", "评价人（人员代码）"),
            // entity.supplierEvaluation.evaluatorby
            new TranslationSeedItem("entity.supplierEvaluation.evaluatorby", "zh-CN", "评价人", "评价人（人员代码）"),
            // entity.supplierEvaluation.evaluatorby
            new TranslationSeedItem("entity.supplierEvaluation.evaluatorby", "zh-HK", "评价人", "评价人（人员代码）"),

            // entity.supplierEvaluation.evaluationdept
            new TranslationSeedItem("entity.supplierEvaluation.evaluationdept", "en-US", "评价部门", "评价部门"),
            // entity.supplierEvaluation.evaluationdept
            new TranslationSeedItem("entity.supplierEvaluation.evaluationdept", "ja-JP", "评价部门", "评价部门"),
            // entity.supplierEvaluation.evaluationdept
            new TranslationSeedItem("entity.supplierEvaluation.evaluationdept", "zh-CN", "评价部门", "评价部门"),
            // entity.supplierEvaluation.evaluationdept
            new TranslationSeedItem("entity.supplierEvaluation.evaluationdept", "zh-HK", "评价部门", "评价部门"),

            // entity.supplierEvaluation.overallrating
            new TranslationSeedItem("entity.supplierEvaluation.overallrating", "en-US", "总体评级", "总体评级（0=D级-不合格，1=C级-合格，2=B级-良好，3=A级-优秀）"),
            // entity.supplierEvaluation.overallrating
            new TranslationSeedItem("entity.supplierEvaluation.overallrating", "ja-JP", "总体评级", "总体评级（0=D级-不合格，1=C级-合格，2=B级-良好，3=A级-优秀）"),
            // entity.supplierEvaluation.overallrating
            new TranslationSeedItem("entity.supplierEvaluation.overallrating", "zh-CN", "总体评级", "总体评级（0=D级-不合格，1=C级-合格，2=B级-良好，3=A级-优秀）"),
            // entity.supplierEvaluation.overallrating
            new TranslationSeedItem("entity.supplierEvaluation.overallrating", "zh-HK", "总体评级", "总体评级（0=D级-不合格，1=C级-合格，2=B级-良好，3=A级-优秀）"),

            // entity.supplierEvaluation.totalscore
            new TranslationSeedItem("entity.supplierEvaluation.totalscore", "en-US", "综合评分", "综合评分（0-100分）"),
            // entity.supplierEvaluation.totalscore
            new TranslationSeedItem("entity.supplierEvaluation.totalscore", "ja-JP", "综合评分", "综合评分（0-100分）"),
            // entity.supplierEvaluation.totalscore
            new TranslationSeedItem("entity.supplierEvaluation.totalscore", "zh-CN", "综合评分", "综合评分（0-100分）"),
            // entity.supplierEvaluation.totalscore
            new TranslationSeedItem("entity.supplierEvaluation.totalscore", "zh-HK", "综合评分", "综合评分（0-100分）"),

            // entity.supplierEvaluation.qualityscore
            new TranslationSeedItem("entity.supplierEvaluation.qualityscore", "en-US", "质量评分", "质量评分（0-100分）"),
            // entity.supplierEvaluation.qualityscore
            new TranslationSeedItem("entity.supplierEvaluation.qualityscore", "ja-JP", "质量评分", "质量评分（0-100分）"),
            // entity.supplierEvaluation.qualityscore
            new TranslationSeedItem("entity.supplierEvaluation.qualityscore", "zh-CN", "质量评分", "质量评分（0-100分）"),
            // entity.supplierEvaluation.qualityscore
            new TranslationSeedItem("entity.supplierEvaluation.qualityscore", "zh-HK", "质量评分", "质量评分（0-100分）"),

            // entity.supplierEvaluation.deliveryscore
            new TranslationSeedItem("entity.supplierEvaluation.deliveryscore", "en-US", "交付评分", "交付评分（0-100分）"),
            // entity.supplierEvaluation.deliveryscore
            new TranslationSeedItem("entity.supplierEvaluation.deliveryscore", "ja-JP", "交付评分", "交付评分（0-100分）"),
            // entity.supplierEvaluation.deliveryscore
            new TranslationSeedItem("entity.supplierEvaluation.deliveryscore", "zh-CN", "交付评分", "交付评分（0-100分）"),
            // entity.supplierEvaluation.deliveryscore
            new TranslationSeedItem("entity.supplierEvaluation.deliveryscore", "zh-HK", "交付评分", "交付评分（0-100分）"),

            // entity.supplierEvaluation.pricescore
            new TranslationSeedItem("entity.supplierEvaluation.pricescore", "en-US", "价格评分", "价格评分（0-100分）"),
            // entity.supplierEvaluation.pricescore
            new TranslationSeedItem("entity.supplierEvaluation.pricescore", "ja-JP", "价格评分", "价格评分（0-100分）"),
            // entity.supplierEvaluation.pricescore
            new TranslationSeedItem("entity.supplierEvaluation.pricescore", "zh-CN", "价格评分", "价格评分（0-100分）"),
            // entity.supplierEvaluation.pricescore
            new TranslationSeedItem("entity.supplierEvaluation.pricescore", "zh-HK", "价格评分", "价格评分（0-100分）"),

            // entity.supplierEvaluation.servicescore
            new TranslationSeedItem("entity.supplierEvaluation.servicescore", "en-US", "服务评分", "服务评分（0-100分）"),
            // entity.supplierEvaluation.servicescore
            new TranslationSeedItem("entity.supplierEvaluation.servicescore", "ja-JP", "服务评分", "服务评分（0-100分）"),
            // entity.supplierEvaluation.servicescore
            new TranslationSeedItem("entity.supplierEvaluation.servicescore", "zh-CN", "服务评分", "服务评分（0-100分）"),
            // entity.supplierEvaluation.servicescore
            new TranslationSeedItem("entity.supplierEvaluation.servicescore", "zh-HK", "服务评分", "服务评分（0-100分）"),

            // entity.supplierEvaluation.technicalscore
            new TranslationSeedItem("entity.supplierEvaluation.technicalscore", "en-US", "技术能力评分", "技术能力评分（0-100分）"),
            // entity.supplierEvaluation.technicalscore
            new TranslationSeedItem("entity.supplierEvaluation.technicalscore", "ja-JP", "技术能力评分", "技术能力评分（0-100分）"),
            // entity.supplierEvaluation.technicalscore
            new TranslationSeedItem("entity.supplierEvaluation.technicalscore", "zh-CN", "技术能力评分", "技术能力评分（0-100分）"),
            // entity.supplierEvaluation.technicalscore
            new TranslationSeedItem("entity.supplierEvaluation.technicalscore", "zh-HK", "技术能力评分", "技术能力评分（0-100分）"),

            // entity.supplierEvaluation.mainstrengths
            new TranslationSeedItem("entity.supplierEvaluation.mainstrengths", "en-US", "主要优点", "主要优点"),
            // entity.supplierEvaluation.mainstrengths
            new TranslationSeedItem("entity.supplierEvaluation.mainstrengths", "ja-JP", "主要优点", "主要优点"),
            // entity.supplierEvaluation.mainstrengths
            new TranslationSeedItem("entity.supplierEvaluation.mainstrengths", "zh-CN", "主要优点", "主要优点"),
            // entity.supplierEvaluation.mainstrengths
            new TranslationSeedItem("entity.supplierEvaluation.mainstrengths", "zh-HK", "主要优点", "主要优点"),

            // entity.supplierEvaluation.mainissues
            new TranslationSeedItem("entity.supplierEvaluation.mainissues", "en-US", "主要问题", "主要问题/不足"),
            // entity.supplierEvaluation.mainissues
            new TranslationSeedItem("entity.supplierEvaluation.mainissues", "ja-JP", "主要问题", "主要问题/不足"),
            // entity.supplierEvaluation.mainissues
            new TranslationSeedItem("entity.supplierEvaluation.mainissues", "zh-CN", "主要问题", "主要问题/不足"),
            // entity.supplierEvaluation.mainissues
            new TranslationSeedItem("entity.supplierEvaluation.mainissues", "zh-HK", "主要问题", "主要问题/不足"),

            // entity.supplierEvaluation.improvementrequirements
            new TranslationSeedItem("entity.supplierEvaluation.improvementrequirements", "en-US", "改进要求", "改进要求/建议"),
            // entity.supplierEvaluation.improvementrequirements
            new TranslationSeedItem("entity.supplierEvaluation.improvementrequirements", "ja-JP", "改进要求", "改进要求/建议"),
            // entity.supplierEvaluation.improvementrequirements
            new TranslationSeedItem("entity.supplierEvaluation.improvementrequirements", "zh-CN", "改进要求", "改进要求/建议"),
            // entity.supplierEvaluation.improvementrequirements
            new TranslationSeedItem("entity.supplierEvaluation.improvementrequirements", "zh-HK", "改进要求", "改进要求/建议"),

            // entity.supplierEvaluation.evaluationconclusion
            new TranslationSeedItem("entity.supplierEvaluation.evaluationconclusion", "en-US", "考核结论", "考核结论（0=继续合作，1=限期整改，2=减少订单，3=暂停合作，4=取消资格）"),
            // entity.supplierEvaluation.evaluationconclusion
            new TranslationSeedItem("entity.supplierEvaluation.evaluationconclusion", "ja-JP", "考核结论", "考核结论（0=继续合作，1=限期整改，2=减少订单，3=暂停合作，4=取消资格）"),
            // entity.supplierEvaluation.evaluationconclusion
            new TranslationSeedItem("entity.supplierEvaluation.evaluationconclusion", "zh-CN", "考核结论", "考核结论（0=继续合作，1=限期整改，2=减少订单，3=暂停合作，4=取消资格）"),
            // entity.supplierEvaluation.evaluationconclusion
            new TranslationSeedItem("entity.supplierEvaluation.evaluationconclusion", "zh-HK", "考核结论", "考核结论（0=继续合作，1=限期整改，2=减少订单，3=暂停合作，4=取消资格）"),

            // entity.supplierEvaluation.rectificationdeadline
            new TranslationSeedItem("entity.supplierEvaluation.rectificationdeadline", "en-US", "整改期限", "整改期限（要求完成日期）"),
            // entity.supplierEvaluation.rectificationdeadline
            new TranslationSeedItem("entity.supplierEvaluation.rectificationdeadline", "ja-JP", "整改期限", "整改期限（要求完成日期）"),
            // entity.supplierEvaluation.rectificationdeadline
            new TranslationSeedItem("entity.supplierEvaluation.rectificationdeadline", "zh-CN", "整改期限", "整改期限（要求完成日期）"),
            // entity.supplierEvaluation.rectificationdeadline
            new TranslationSeedItem("entity.supplierEvaluation.rectificationdeadline", "zh-HK", "整改期限", "整改期限（要求完成日期）"),

            // entity.supplierEvaluation.evaluationstatus
            new TranslationSeedItem("entity.supplierEvaluation.evaluationstatus", "en-US", "评价状态", "评价状态（0=草稿，1=评价中，2=已完成，3=已归档）"),
            // entity.supplierEvaluation.evaluationstatus
            new TranslationSeedItem("entity.supplierEvaluation.evaluationstatus", "ja-JP", "评价状态", "评价状态（0=草稿，1=评价中，2=已完成，3=已归档）"),
            // entity.supplierEvaluation.evaluationstatus
            new TranslationSeedItem("entity.supplierEvaluation.evaluationstatus", "zh-CN", "评价状态", "评价状态（0=草稿，1=评价中，2=已完成，3=已归档）"),
            // entity.supplierEvaluation.evaluationstatus
            new TranslationSeedItem("entity.supplierEvaluation.evaluationstatus", "zh-HK", "评价状态", "评价状态（0=草稿，1=评价中，2=已完成，3=已归档）"),

            // entity.supplierEvaluation.rectificationstatus
            new TranslationSeedItem("entity.supplierEvaluation.rectificationstatus", "en-US", "整改跟进状态", "整改跟进状态（0=无需整改，1=待整改，2=整改中，3=已完成，4=未通过）"),
            // entity.supplierEvaluation.rectificationstatus
            new TranslationSeedItem("entity.supplierEvaluation.rectificationstatus", "ja-JP", "整改跟进状态", "整改跟进状态（0=无需整改，1=待整改，2=整改中，3=已完成，4=未通过）"),
            // entity.supplierEvaluation.rectificationstatus
            new TranslationSeedItem("entity.supplierEvaluation.rectificationstatus", "zh-CN", "整改跟进状态", "整改跟进状态（0=无需整改，1=待整改，2=整改中，3=已完成，4=未通过）"),
            // entity.supplierEvaluation.rectificationstatus
            new TranslationSeedItem("entity.supplierEvaluation.rectificationstatus", "zh-HK", "整改跟进状态", "整改跟进状态（0=无需整改，1=待整改，2=整改中，3=已完成，4=未通过）"),

            // entity.supplierEvaluation.relatedplant
            new TranslationSeedItem("entity.supplierEvaluation.relatedplant", "en-US", "关联工厂", "关联工厂"),
            // entity.supplierEvaluation.relatedplant
            new TranslationSeedItem("entity.supplierEvaluation.relatedplant", "ja-JP", "关联工厂", "关联工厂"),
            // entity.supplierEvaluation.relatedplant
            new TranslationSeedItem("entity.supplierEvaluation.relatedplant", "zh-CN", "关联工厂", "关联工厂"),
            // entity.supplierEvaluation.relatedplant
            new TranslationSeedItem("entity.supplierEvaluation.relatedplant", "zh-HK", "关联工厂", "关联工厂"),

            // entity.supplierEvaluation.sortorder
            new TranslationSeedItem("entity.supplierEvaluation.sortorder", "en-US", "排序号", "排序号（越小越靠前）"),
            // entity.supplierEvaluation.sortorder
            new TranslationSeedItem("entity.supplierEvaluation.sortorder", "ja-JP", "排序号", "排序号（越小越靠前）"),
            // entity.supplierEvaluation.sortorder
            new TranslationSeedItem("entity.supplierEvaluation.sortorder", "zh-CN", "排序号", "排序号（越小越靠前）"),
            // entity.supplierEvaluation.sortorder
            new TranslationSeedItem("entity.supplierEvaluation.sortorder", "zh-HK", "排序号", "排序号（越小越靠前）"),

            // entity.supplierEvaluation.items
            new TranslationSeedItem("entity.supplierEvaluation.items", "en-US", "items", "评价项目明细列表（主子表关系）"),
            // entity.supplierEvaluation.items
            new TranslationSeedItem("entity.supplierEvaluation.items", "ja-JP", "items", "评价项目明细列表（主子表关系）"),
            // entity.supplierEvaluation.items
            new TranslationSeedItem("entity.supplierEvaluation.items", "zh-CN", "items", "评价项目明细列表（主子表关系）"),
            // entity.supplierEvaluation.items
            new TranslationSeedItem("entity.supplierEvaluation.items", "zh-HK", "items", "评价项目明细列表（主子表关系）"),
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
