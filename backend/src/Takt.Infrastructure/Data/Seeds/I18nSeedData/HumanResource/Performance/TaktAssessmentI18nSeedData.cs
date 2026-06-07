// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Performance
// 文件名称：TaktAssessmentI18nSeedData.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktAssessment 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktAssessment 实体国际化翻译种子（键前缀 entity.assessment.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktAssessmentI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktAssessment 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 assessment 实体翻译...", tenantCode);

        foreach (var item in GetAssessmentTranslations())
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

        TaktLogger.Information("TaktAssessment 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktAssessment 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.assessment._self / entity.assessment.{{field}}；ResourceGroup=TaktModule.HumanResource；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetAssessmentTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.assessment._self
            new TranslationSeedItem("entity.assessment._self", "en-US", "Assessment Information", "实体名称"),
            // entity.assessment._self
            new TranslationSeedItem("entity.assessment._self", "ja-JP", "员工绩效考核评估信息", "实体名称"),
            // entity.assessment._self
            new TranslationSeedItem("entity.assessment._self", "zh-CN", "员工绩效考核评估信息", "实体名称"),
            // entity.assessment._self
            new TranslationSeedItem("entity.assessment._self", "zh-HK", "员工绩效考核评估信息", "实体名称"),

            // entity.assessment.employeeid
            new TranslationSeedItem("entity.assessment.employeeid", "en-US", "员工ID", "员工 ID"),
            // entity.assessment.employeeid
            new TranslationSeedItem("entity.assessment.employeeid", "ja-JP", "员工ID", "员工 ID"),
            // entity.assessment.employeeid
            new TranslationSeedItem("entity.assessment.employeeid", "zh-CN", "员工ID", "员工 ID"),
            // entity.assessment.employeeid
            new TranslationSeedItem("entity.assessment.employeeid", "zh-HK", "员工ID", "员工 ID"),

            // entity.assessment.employeename
            new TranslationSeedItem("entity.assessment.employeename", "en-US", "员工姓名", "员工姓名"),
            // entity.assessment.employeename
            new TranslationSeedItem("entity.assessment.employeename", "ja-JP", "员工姓名", "员工姓名"),
            // entity.assessment.employeename
            new TranslationSeedItem("entity.assessment.employeename", "zh-CN", "员工姓名", "员工姓名"),
            // entity.assessment.employeename
            new TranslationSeedItem("entity.assessment.employeename", "zh-HK", "员工姓名", "员工姓名"),

            // entity.assessment.period
            new TranslationSeedItem("entity.assessment.period", "en-US", "考核周期", "考核周期（如 2026-Q1、2026-Annual）"),
            // entity.assessment.period
            new TranslationSeedItem("entity.assessment.period", "ja-JP", "考核周期", "考核周期（如 2026-Q1、2026-Annual）"),
            // entity.assessment.period
            new TranslationSeedItem("entity.assessment.period", "zh-CN", "考核周期", "考核周期（如 2026-Q1、2026-Annual）"),
            // entity.assessment.period
            new TranslationSeedItem("entity.assessment.period", "zh-HK", "考核周期", "考核周期（如 2026-Q1、2026-Annual）"),

            // entity.assessment.date
            new TranslationSeedItem("entity.assessment.date", "en-US", "考核日期", "考核日期"),
            // entity.assessment.date
            new TranslationSeedItem("entity.assessment.date", "ja-JP", "考核日期", "考核日期"),
            // entity.assessment.date
            new TranslationSeedItem("entity.assessment.date", "zh-CN", "考核日期", "考核日期"),
            // entity.assessment.date
            new TranslationSeedItem("entity.assessment.date", "zh-HK", "考核日期", "考核日期"),

            // entity.assessment.schememetricid
            new TranslationSeedItem("entity.assessment.schememetricid", "en-US", "方案指标ID", "方案指标 ID"),
            // entity.assessment.schememetricid
            new TranslationSeedItem("entity.assessment.schememetricid", "ja-JP", "方案指标ID", "方案指标 ID"),
            // entity.assessment.schememetricid
            new TranslationSeedItem("entity.assessment.schememetricid", "zh-CN", "方案指标ID", "方案指标 ID"),
            // entity.assessment.schememetricid
            new TranslationSeedItem("entity.assessment.schememetricid", "zh-HK", "方案指标ID", "方案指标 ID"),

            // entity.assessment.selfscore
            new TranslationSeedItem("entity.assessment.selfscore", "en-US", "自评分数", "自评分数"),
            // entity.assessment.selfscore
            new TranslationSeedItem("entity.assessment.selfscore", "ja-JP", "自评分数", "自评分数"),
            // entity.assessment.selfscore
            new TranslationSeedItem("entity.assessment.selfscore", "zh-CN", "自评分数", "自评分数"),
            // entity.assessment.selfscore
            new TranslationSeedItem("entity.assessment.selfscore", "zh-HK", "自评分数", "自评分数"),

            // entity.assessment.selfevaluationnotes
            new TranslationSeedItem("entity.assessment.selfevaluationnotes", "en-US", "自评说明", "自评说明"),
            // entity.assessment.selfevaluationnotes
            new TranslationSeedItem("entity.assessment.selfevaluationnotes", "ja-JP", "自评说明", "自评说明"),
            // entity.assessment.selfevaluationnotes
            new TranslationSeedItem("entity.assessment.selfevaluationnotes", "zh-CN", "自评说明", "自评说明"),
            // entity.assessment.selfevaluationnotes
            new TranslationSeedItem("entity.assessment.selfevaluationnotes", "zh-HK", "自评说明", "自评说明"),

            // entity.assessment.supervisorscore
            new TranslationSeedItem("entity.assessment.supervisorscore", "en-US", "主管评分", "主管评分"),
            // entity.assessment.supervisorscore
            new TranslationSeedItem("entity.assessment.supervisorscore", "ja-JP", "主管评分", "主管评分"),
            // entity.assessment.supervisorscore
            new TranslationSeedItem("entity.assessment.supervisorscore", "zh-CN", "主管评分", "主管评分"),
            // entity.assessment.supervisorscore
            new TranslationSeedItem("entity.assessment.supervisorscore", "zh-HK", "主管评分", "主管评分"),

            // entity.assessment.supervisorcomments
            new TranslationSeedItem("entity.assessment.supervisorcomments", "en-US", "主管评语", "主管评语"),
            // entity.assessment.supervisorcomments
            new TranslationSeedItem("entity.assessment.supervisorcomments", "ja-JP", "主管评语", "主管评语"),
            // entity.assessment.supervisorcomments
            new TranslationSeedItem("entity.assessment.supervisorcomments", "zh-CN", "主管评语", "主管评语"),
            // entity.assessment.supervisorcomments
            new TranslationSeedItem("entity.assessment.supervisorcomments", "zh-HK", "主管评语", "主管评语"),

            // entity.assessment.finalscore
            new TranslationSeedItem("entity.assessment.finalscore", "en-US", "综合得分", "综合得分"),
            // entity.assessment.finalscore
            new TranslationSeedItem("entity.assessment.finalscore", "ja-JP", "综合得分", "综合得分"),
            // entity.assessment.finalscore
            new TranslationSeedItem("entity.assessment.finalscore", "zh-CN", "综合得分", "综合得分"),
            // entity.assessment.finalscore
            new TranslationSeedItem("entity.assessment.finalscore", "zh-HK", "综合得分", "综合得分"),

            // entity.assessment.performancegrade
            new TranslationSeedItem("entity.assessment.performancegrade", "en-US", "绩效等级", "绩效等级（A/B/C/D/E）"),
            // entity.assessment.performancegrade
            new TranslationSeedItem("entity.assessment.performancegrade", "ja-JP", "绩效等级", "绩效等级（A/B/C/D/E）"),
            // entity.assessment.performancegrade
            new TranslationSeedItem("entity.assessment.performancegrade", "zh-CN", "绩效等级", "绩效等级（A/B/C/D/E）"),
            // entity.assessment.performancegrade
            new TranslationSeedItem("entity.assessment.performancegrade", "zh-HK", "绩效等级", "绩效等级（A/B/C/D/E）"),

            // entity.assessment.reviewerid
            new TranslationSeedItem("entity.assessment.reviewerid", "en-US", "评审人ID", "评审人 ID"),
            // entity.assessment.reviewerid
            new TranslationSeedItem("entity.assessment.reviewerid", "ja-JP", "评审人ID", "评审人 ID"),
            // entity.assessment.reviewerid
            new TranslationSeedItem("entity.assessment.reviewerid", "zh-CN", "评审人ID", "评审人 ID"),
            // entity.assessment.reviewerid
            new TranslationSeedItem("entity.assessment.reviewerid", "zh-HK", "评审人ID", "评审人 ID"),

            // entity.assessment.interviewdate
            new TranslationSeedItem("entity.assessment.interviewdate", "en-US", "面谈日期", "面谈日期"),
            // entity.assessment.interviewdate
            new TranslationSeedItem("entity.assessment.interviewdate", "ja-JP", "面谈日期", "面谈日期"),
            // entity.assessment.interviewdate
            new TranslationSeedItem("entity.assessment.interviewdate", "zh-CN", "面谈日期", "面谈日期"),
            // entity.assessment.interviewdate
            new TranslationSeedItem("entity.assessment.interviewdate", "zh-HK", "面谈日期", "面谈日期"),

            // entity.assessment.interviewnotes
            new TranslationSeedItem("entity.assessment.interviewnotes", "en-US", "面谈记录", "面谈记录"),
            // entity.assessment.interviewnotes
            new TranslationSeedItem("entity.assessment.interviewnotes", "ja-JP", "面谈记录", "面谈记录"),
            // entity.assessment.interviewnotes
            new TranslationSeedItem("entity.assessment.interviewnotes", "zh-CN", "面谈记录", "面谈记录"),
            // entity.assessment.interviewnotes
            new TranslationSeedItem("entity.assessment.interviewnotes", "zh-HK", "面谈记录", "面谈记录"),

            // entity.assessment.status
            new TranslationSeedItem("entity.assessment.status", "en-US", "状态", "状态（0=待自评 1=自评中 2=待主管评审 3=评审中 4=已完成 5=已确认）"),
            // entity.assessment.status
            new TranslationSeedItem("entity.assessment.status", "ja-JP", "状态", "状态（0=待自评 1=自评中 2=待主管评审 3=评审中 4=已完成 5=已确认）"),
            // entity.assessment.status
            new TranslationSeedItem("entity.assessment.status", "zh-CN", "状态", "状态（0=待自评 1=自评中 2=待主管评审 3=评审中 4=已完成 5=已确认）"),
            // entity.assessment.status
            new TranslationSeedItem("entity.assessment.status", "zh-HK", "状态", "状态（0=待自评 1=自评中 2=待主管评审 3=评审中 4=已完成 5=已确认）"),

            // entity.assessment.relatedplant
            new TranslationSeedItem("entity.assessment.relatedplant", "en-US", "关联工厂", "关联工厂"),
            // entity.assessment.relatedplant
            new TranslationSeedItem("entity.assessment.relatedplant", "ja-JP", "关联工厂", "关联工厂"),
            // entity.assessment.relatedplant
            new TranslationSeedItem("entity.assessment.relatedplant", "zh-CN", "关联工厂", "关联工厂"),
            // entity.assessment.relatedplant
            new TranslationSeedItem("entity.assessment.relatedplant", "zh-HK", "关联工厂", "关联工厂"),
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
