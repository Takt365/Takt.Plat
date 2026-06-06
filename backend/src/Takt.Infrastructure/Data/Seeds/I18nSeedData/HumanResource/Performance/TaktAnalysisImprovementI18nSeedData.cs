// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Performance
// 文件名称：TaktAnalysisImprovementI18nSeedData.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktAnalysisImprovement 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Performance;

/// <summary>
/// TaktAnalysisImprovement 实体国际化翻译种子（键前缀 entity.analysisImprovement.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktAnalysisImprovementI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktAnalysisImprovement 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 analysisImprovement 实体翻译...", tenantCode);

        foreach (var item in GetAnalysisImprovementTranslations())
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

        TaktLogger.Information("TaktAnalysisImprovement 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktAnalysisImprovement 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.analysisImprovement._self / entity.analysisImprovement.{{field}}；ResourceGroup=TaktModule.HumanResource；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetAnalysisImprovementTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.analysisImprovement._self
            new TranslationSeedItem("entity.analysisImprovement._self", "en-US", "Analysis Improvement Information", "实体名称"),
            // entity.analysisImprovement._self
            new TranslationSeedItem("entity.analysisImprovement._self", "ja-JP", "绩效分析改进计划信息", "实体名称"),
            // entity.analysisImprovement._self
            new TranslationSeedItem("entity.analysisImprovement._self", "zh-CN", "绩效分析改进计划信息", "实体名称"),
            // entity.analysisImprovement._self
            new TranslationSeedItem("entity.analysisImprovement._self", "zh-HK", "绩效分析改进计划信息", "实体名称"),

            // entity.analysisImprovement.employeeid
            new TranslationSeedItem("entity.analysisImprovement.employeeid", "en-US", "员工ID", "员工 ID"),
            // entity.analysisImprovement.employeeid
            new TranslationSeedItem("entity.analysisImprovement.employeeid", "ja-JP", "员工ID", "员工 ID"),
            // entity.analysisImprovement.employeeid
            new TranslationSeedItem("entity.analysisImprovement.employeeid", "zh-CN", "员工ID", "员工 ID"),
            // entity.analysisImprovement.employeeid
            new TranslationSeedItem("entity.analysisImprovement.employeeid", "zh-HK", "员工ID", "员工 ID"),

            // entity.analysisImprovement.employeename
            new TranslationSeedItem("entity.analysisImprovement.employeename", "en-US", "员工姓名", "员工姓名"),
            // entity.analysisImprovement.employeename
            new TranslationSeedItem("entity.analysisImprovement.employeename", "ja-JP", "员工姓名", "员工姓名"),
            // entity.analysisImprovement.employeename
            new TranslationSeedItem("entity.analysisImprovement.employeename", "zh-CN", "员工姓名", "员工姓名"),
            // entity.analysisImprovement.employeename
            new TranslationSeedItem("entity.analysisImprovement.employeename", "zh-HK", "员工姓名", "员工姓名"),

            // entity.analysisImprovement.assessmentid
            new TranslationSeedItem("entity.analysisImprovement.assessmentid", "en-US", "考核评估ID", "关联考核评估 ID"),
            // entity.analysisImprovement.assessmentid
            new TranslationSeedItem("entity.analysisImprovement.assessmentid", "ja-JP", "考核评估ID", "关联考核评估 ID"),
            // entity.analysisImprovement.assessmentid
            new TranslationSeedItem("entity.analysisImprovement.assessmentid", "zh-CN", "考核评估ID", "关联考核评估 ID"),
            // entity.analysisImprovement.assessmentid
            new TranslationSeedItem("entity.analysisImprovement.assessmentid", "zh-HK", "考核评估ID", "关联考核评估 ID"),

            // entity.analysisImprovement.plantitle
            new TranslationSeedItem("entity.analysisImprovement.plantitle", "en-US", "改进计划标题", "改进计划标题"),
            // entity.analysisImprovement.plantitle
            new TranslationSeedItem("entity.analysisImprovement.plantitle", "ja-JP", "改进计划标题", "改进计划标题"),
            // entity.analysisImprovement.plantitle
            new TranslationSeedItem("entity.analysisImprovement.plantitle", "zh-CN", "改进计划标题", "改进计划标题"),
            // entity.analysisImprovement.plantitle
            new TranslationSeedItem("entity.analysisImprovement.plantitle", "zh-HK", "改进计划标题", "改进计划标题"),

            // entity.analysisImprovement.improvementarea
            new TranslationSeedItem("entity.analysisImprovement.improvementarea", "en-US", "改进领域", "改进领域"),
            // entity.analysisImprovement.improvementarea
            new TranslationSeedItem("entity.analysisImprovement.improvementarea", "ja-JP", "改进领域", "改进领域"),
            // entity.analysisImprovement.improvementarea
            new TranslationSeedItem("entity.analysisImprovement.improvementarea", "zh-CN", "改进领域", "改进领域"),
            // entity.analysisImprovement.improvementarea
            new TranslationSeedItem("entity.analysisImprovement.improvementarea", "zh-HK", "改进领域", "改进领域"),

            // entity.analysisImprovement.currentsituation
            new TranslationSeedItem("entity.analysisImprovement.currentsituation", "en-US", "当前状况描述", "当前状况描述"),
            // entity.analysisImprovement.currentsituation
            new TranslationSeedItem("entity.analysisImprovement.currentsituation", "ja-JP", "当前状况描述", "当前状况描述"),
            // entity.analysisImprovement.currentsituation
            new TranslationSeedItem("entity.analysisImprovement.currentsituation", "zh-CN", "当前状况描述", "当前状况描述"),
            // entity.analysisImprovement.currentsituation
            new TranslationSeedItem("entity.analysisImprovement.currentsituation", "zh-HK", "当前状况描述", "当前状况描述"),

            // entity.analysisImprovement.improvementgoal
            new TranslationSeedItem("entity.analysisImprovement.improvementgoal", "en-US", "改进目标", "改进目标"),
            // entity.analysisImprovement.improvementgoal
            new TranslationSeedItem("entity.analysisImprovement.improvementgoal", "ja-JP", "改进目标", "改进目标"),
            // entity.analysisImprovement.improvementgoal
            new TranslationSeedItem("entity.analysisImprovement.improvementgoal", "zh-CN", "改进目标", "改进目标"),
            // entity.analysisImprovement.improvementgoal
            new TranslationSeedItem("entity.analysisImprovement.improvementgoal", "zh-HK", "改进目标", "改进目标"),

            // entity.analysisImprovement.improvementactions
            new TranslationSeedItem("entity.analysisImprovement.improvementactions", "en-US", "改进措施", "改进措施"),
            // entity.analysisImprovement.improvementactions
            new TranslationSeedItem("entity.analysisImprovement.improvementactions", "ja-JP", "改进措施", "改进措施"),
            // entity.analysisImprovement.improvementactions
            new TranslationSeedItem("entity.analysisImprovement.improvementactions", "zh-CN", "改进措施", "改进措施"),
            // entity.analysisImprovement.improvementactions
            new TranslationSeedItem("entity.analysisImprovement.improvementactions", "zh-HK", "改进措施", "改进措施"),

            // entity.analysisImprovement.plandate
            new TranslationSeedItem("entity.analysisImprovement.plandate", "en-US", "计划制定日期", "计划制定日期"),
            // entity.analysisImprovement.plandate
            new TranslationSeedItem("entity.analysisImprovement.plandate", "ja-JP", "计划制定日期", "计划制定日期"),
            // entity.analysisImprovement.plandate
            new TranslationSeedItem("entity.analysisImprovement.plandate", "zh-CN", "计划制定日期", "计划制定日期"),
            // entity.analysisImprovement.plandate
            new TranslationSeedItem("entity.analysisImprovement.plandate", "zh-HK", "计划制定日期", "计划制定日期"),

            // entity.analysisImprovement.targetcompletiondate
            new TranslationSeedItem("entity.analysisImprovement.targetcompletiondate", "en-US", "目标完成日期", "目标完成日期"),
            // entity.analysisImprovement.targetcompletiondate
            new TranslationSeedItem("entity.analysisImprovement.targetcompletiondate", "ja-JP", "目标完成日期", "目标完成日期"),
            // entity.analysisImprovement.targetcompletiondate
            new TranslationSeedItem("entity.analysisImprovement.targetcompletiondate", "zh-CN", "目标完成日期", "目标完成日期"),
            // entity.analysisImprovement.targetcompletiondate
            new TranslationSeedItem("entity.analysisImprovement.targetcompletiondate", "zh-HK", "目标完成日期", "目标完成日期"),

            // entity.analysisImprovement.progresspercentage
            new TranslationSeedItem("entity.analysisImprovement.progresspercentage", "en-US", "进度百分比", "进度百分比（%）"),
            // entity.analysisImprovement.progresspercentage
            new TranslationSeedItem("entity.analysisImprovement.progresspercentage", "ja-JP", "进度百分比", "进度百分比（%）"),
            // entity.analysisImprovement.progresspercentage
            new TranslationSeedItem("entity.analysisImprovement.progresspercentage", "zh-CN", "进度百分比", "进度百分比（%）"),
            // entity.analysisImprovement.progresspercentage
            new TranslationSeedItem("entity.analysisImprovement.progresspercentage", "zh-HK", "进度百分比", "进度百分比（%）"),

            // entity.analysisImprovement.resultdescription
            new TranslationSeedItem("entity.analysisImprovement.resultdescription", "en-US", "改进结果说明", "改进结果说明"),
            // entity.analysisImprovement.resultdescription
            new TranslationSeedItem("entity.analysisImprovement.resultdescription", "ja-JP", "改进结果说明", "改进结果说明"),
            // entity.analysisImprovement.resultdescription
            new TranslationSeedItem("entity.analysisImprovement.resultdescription", "zh-CN", "改进结果说明", "改进结果说明"),
            // entity.analysisImprovement.resultdescription
            new TranslationSeedItem("entity.analysisImprovement.resultdescription", "zh-HK", "改进结果说明", "改进结果说明"),

            // entity.analysisImprovement.mentorid
            new TranslationSeedItem("entity.analysisImprovement.mentorid", "en-US", "指导老师ID", "指导老师 ID"),
            // entity.analysisImprovement.mentorid
            new TranslationSeedItem("entity.analysisImprovement.mentorid", "ja-JP", "指导老师ID", "指导老师 ID"),
            // entity.analysisImprovement.mentorid
            new TranslationSeedItem("entity.analysisImprovement.mentorid", "zh-CN", "指导老师ID", "指导老师 ID"),
            // entity.analysisImprovement.mentorid
            new TranslationSeedItem("entity.analysisImprovement.mentorid", "zh-HK", "指导老师ID", "指导老师 ID"),

            // entity.analysisImprovement.improvementstatus
            new TranslationSeedItem("entity.analysisImprovement.improvementstatus", "en-US", "业务状态", "业务状态（0=待审批 1=进行中 2=已完成 3=已关闭）"),
            // entity.analysisImprovement.improvementstatus
            new TranslationSeedItem("entity.analysisImprovement.improvementstatus", "ja-JP", "业务状态", "业务状态（0=待审批 1=进行中 2=已完成 3=已关闭）"),
            // entity.analysisImprovement.improvementstatus
            new TranslationSeedItem("entity.analysisImprovement.improvementstatus", "zh-CN", "业务状态", "业务状态（0=待审批 1=进行中 2=已完成 3=已关闭）"),
            // entity.analysisImprovement.improvementstatus
            new TranslationSeedItem("entity.analysisImprovement.improvementstatus", "zh-HK", "业务状态", "业务状态（0=待审批 1=进行中 2=已完成 3=已关闭）"),

            // entity.analysisImprovement.relatedplant
            new TranslationSeedItem("entity.analysisImprovement.relatedplant", "en-US", "关联工厂", "关联工厂"),
            // entity.analysisImprovement.relatedplant
            new TranslationSeedItem("entity.analysisImprovement.relatedplant", "ja-JP", "关联工厂", "关联工厂"),
            // entity.analysisImprovement.relatedplant
            new TranslationSeedItem("entity.analysisImprovement.relatedplant", "zh-CN", "关联工厂", "关联工厂"),
            // entity.analysisImprovement.relatedplant
            new TranslationSeedItem("entity.analysisImprovement.relatedplant", "zh-HK", "关联工厂", "关联工厂"),
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
        translation.ResourceGroup = TaktModule.HumanResource;
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
