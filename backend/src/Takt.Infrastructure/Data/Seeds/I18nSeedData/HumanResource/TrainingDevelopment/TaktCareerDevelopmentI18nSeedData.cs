// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.TrainingDevelopment
// 文件名称：TaktCareerDevelopmentI18nSeedData.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktCareerDevelopment 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.TrainingDevelopment;

/// <summary>
/// TaktCareerDevelopment 实体国际化翻译种子（键前缀 entity.careerDevelopment.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktCareerDevelopmentI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktCareerDevelopment 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 careerDevelopment 实体翻译...", tenantCode);

        foreach (var item in GetCareerDevelopmentTranslations())
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

        TaktLogger.Information("TaktCareerDevelopment 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktCareerDevelopment 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.careerDevelopment._self / entity.careerDevelopment.{{field}}；ResourceGroup=TaktModule.HumanResource；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetCareerDevelopmentTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.careerDevelopment._self
            new TranslationSeedItem("entity.careerDevelopment._self", "en-US", "Career Development Information", "实体名称"),
            // entity.careerDevelopment._self
            new TranslationSeedItem("entity.careerDevelopment._self", "ja-JP", "员工职业发展规划与技能评估信息", "实体名称"),
            // entity.careerDevelopment._self
            new TranslationSeedItem("entity.careerDevelopment._self", "zh-CN", "员工职业发展规划与技能评估信息", "实体名称"),
            // entity.careerDevelopment._self
            new TranslationSeedItem("entity.careerDevelopment._self", "zh-HK", "员工职业发展规划与技能评估信息", "实体名称"),

            // entity.careerDevelopment.employeeid
            new TranslationSeedItem("entity.careerDevelopment.employeeid", "en-US", "员工ID", "员工 ID"),
            // entity.careerDevelopment.employeeid
            new TranslationSeedItem("entity.careerDevelopment.employeeid", "ja-JP", "员工ID", "员工 ID"),
            // entity.careerDevelopment.employeeid
            new TranslationSeedItem("entity.careerDevelopment.employeeid", "zh-CN", "员工ID", "员工 ID"),
            // entity.careerDevelopment.employeeid
            new TranslationSeedItem("entity.careerDevelopment.employeeid", "zh-HK", "员工ID", "员工 ID"),

            // entity.careerDevelopment.employeename
            new TranslationSeedItem("entity.careerDevelopment.employeename", "en-US", "员工姓名", "员工姓名"),
            // entity.careerDevelopment.employeename
            new TranslationSeedItem("entity.careerDevelopment.employeename", "ja-JP", "员工姓名", "员工姓名"),
            // entity.careerDevelopment.employeename
            new TranslationSeedItem("entity.careerDevelopment.employeename", "zh-CN", "员工姓名", "员工姓名"),
            // entity.careerDevelopment.employeename
            new TranslationSeedItem("entity.careerDevelopment.employeename", "zh-HK", "员工姓名", "员工姓名"),

            // entity.careerDevelopment.skillcategory
            new TranslationSeedItem("entity.careerDevelopment.skillcategory", "en-US", "技能类别", "技能类别"),
            // entity.careerDevelopment.skillcategory
            new TranslationSeedItem("entity.careerDevelopment.skillcategory", "ja-JP", "技能类别", "技能类别"),
            // entity.careerDevelopment.skillcategory
            new TranslationSeedItem("entity.careerDevelopment.skillcategory", "zh-CN", "技能类别", "技能类别"),
            // entity.careerDevelopment.skillcategory
            new TranslationSeedItem("entity.careerDevelopment.skillcategory", "zh-HK", "技能类别", "技能类别"),

            // entity.careerDevelopment.skillname
            new TranslationSeedItem("entity.careerDevelopment.skillname", "en-US", "技能名称", "技能名称"),
            // entity.careerDevelopment.skillname
            new TranslationSeedItem("entity.careerDevelopment.skillname", "ja-JP", "技能名称", "技能名称"),
            // entity.careerDevelopment.skillname
            new TranslationSeedItem("entity.careerDevelopment.skillname", "zh-CN", "技能名称", "技能名称"),
            // entity.careerDevelopment.skillname
            new TranslationSeedItem("entity.careerDevelopment.skillname", "zh-HK", "技能名称", "技能名称"),

            // entity.careerDevelopment.assessmentdate
            new TranslationSeedItem("entity.careerDevelopment.assessmentdate", "en-US", "评估日期", "评估日期"),
            // entity.careerDevelopment.assessmentdate
            new TranslationSeedItem("entity.careerDevelopment.assessmentdate", "ja-JP", "评估日期", "评估日期"),
            // entity.careerDevelopment.assessmentdate
            new TranslationSeedItem("entity.careerDevelopment.assessmentdate", "zh-CN", "评估日期", "评估日期"),
            // entity.careerDevelopment.assessmentdate
            new TranslationSeedItem("entity.careerDevelopment.assessmentdate", "zh-HK", "评估日期", "评估日期"),

            // entity.careerDevelopment.assessmentmethod
            new TranslationSeedItem("entity.careerDevelopment.assessmentmethod", "en-US", "评估方式", "评估方式"),
            // entity.careerDevelopment.assessmentmethod
            new TranslationSeedItem("entity.careerDevelopment.assessmentmethod", "ja-JP", "评估方式", "评估方式"),
            // entity.careerDevelopment.assessmentmethod
            new TranslationSeedItem("entity.careerDevelopment.assessmentmethod", "zh-CN", "评估方式", "评估方式"),
            // entity.careerDevelopment.assessmentmethod
            new TranslationSeedItem("entity.careerDevelopment.assessmentmethod", "zh-HK", "评估方式", "评估方式"),

            // entity.careerDevelopment.assessmentscore
            new TranslationSeedItem("entity.careerDevelopment.assessmentscore", "en-US", "评估得分", "评估得分"),
            // entity.careerDevelopment.assessmentscore
            new TranslationSeedItem("entity.careerDevelopment.assessmentscore", "ja-JP", "评估得分", "评估得分"),
            // entity.careerDevelopment.assessmentscore
            new TranslationSeedItem("entity.careerDevelopment.assessmentscore", "zh-CN", "评估得分", "评估得分"),
            // entity.careerDevelopment.assessmentscore
            new TranslationSeedItem("entity.careerDevelopment.assessmentscore", "zh-HK", "评估得分", "评估得分"),

            // entity.careerDevelopment.skilllevel
            new TranslationSeedItem("entity.careerDevelopment.skilllevel", "en-US", "技能等级", "技能等级"),
            // entity.careerDevelopment.skilllevel
            new TranslationSeedItem("entity.careerDevelopment.skilllevel", "ja-JP", "技能等级", "技能等级"),
            // entity.careerDevelopment.skilllevel
            new TranslationSeedItem("entity.careerDevelopment.skilllevel", "zh-CN", "技能等级", "技能等级"),
            // entity.careerDevelopment.skilllevel
            new TranslationSeedItem("entity.careerDevelopment.skilllevel", "zh-HK", "技能等级", "技能等级"),

            // entity.careerDevelopment.targetposition
            new TranslationSeedItem("entity.careerDevelopment.targetposition", "en-US", "目标岗位", "目标岗位"),
            // entity.careerDevelopment.targetposition
            new TranslationSeedItem("entity.careerDevelopment.targetposition", "ja-JP", "目标岗位", "目标岗位"),
            // entity.careerDevelopment.targetposition
            new TranslationSeedItem("entity.careerDevelopment.targetposition", "zh-CN", "目标岗位", "目标岗位"),
            // entity.careerDevelopment.targetposition
            new TranslationSeedItem("entity.careerDevelopment.targetposition", "zh-HK", "目标岗位", "目标岗位"),

            // entity.careerDevelopment.developmentplan
            new TranslationSeedItem("entity.careerDevelopment.developmentplan", "en-US", "发展计划", "发展计划"),
            // entity.careerDevelopment.developmentplan
            new TranslationSeedItem("entity.careerDevelopment.developmentplan", "ja-JP", "发展计划", "发展计划"),
            // entity.careerDevelopment.developmentplan
            new TranslationSeedItem("entity.careerDevelopment.developmentplan", "zh-CN", "发展计划", "发展计划"),
            // entity.careerDevelopment.developmentplan
            new TranslationSeedItem("entity.careerDevelopment.developmentplan", "zh-HK", "发展计划", "发展计划"),

            // entity.careerDevelopment.improvementsuggestions
            new TranslationSeedItem("entity.careerDevelopment.improvementsuggestions", "en-US", "改进建议", "改进建议"),
            // entity.careerDevelopment.improvementsuggestions
            new TranslationSeedItem("entity.careerDevelopment.improvementsuggestions", "ja-JP", "改进建议", "改进建议"),
            // entity.careerDevelopment.improvementsuggestions
            new TranslationSeedItem("entity.careerDevelopment.improvementsuggestions", "zh-CN", "改进建议", "改进建议"),
            // entity.careerDevelopment.improvementsuggestions
            new TranslationSeedItem("entity.careerDevelopment.improvementsuggestions", "zh-HK", "改进建议", "改进建议"),

            // entity.careerDevelopment.nextassessmentdate
            new TranslationSeedItem("entity.careerDevelopment.nextassessmentdate", "en-US", "下次评估日期", "下次评估日期"),
            // entity.careerDevelopment.nextassessmentdate
            new TranslationSeedItem("entity.careerDevelopment.nextassessmentdate", "ja-JP", "下次评估日期", "下次评估日期"),
            // entity.careerDevelopment.nextassessmentdate
            new TranslationSeedItem("entity.careerDevelopment.nextassessmentdate", "zh-CN", "下次评估日期", "下次评估日期"),
            // entity.careerDevelopment.nextassessmentdate
            new TranslationSeedItem("entity.careerDevelopment.nextassessmentdate", "zh-HK", "下次评估日期", "下次评估日期"),

            // entity.careerDevelopment.status
            new TranslationSeedItem("entity.careerDevelopment.status", "en-US", "状态", "状态（1=进行中 0=已归档）"),
            // entity.careerDevelopment.status
            new TranslationSeedItem("entity.careerDevelopment.status", "ja-JP", "状态", "状态（1=进行中 0=已归档）"),
            // entity.careerDevelopment.status
            new TranslationSeedItem("entity.careerDevelopment.status", "zh-CN", "状态", "状态（1=进行中 0=已归档）"),
            // entity.careerDevelopment.status
            new TranslationSeedItem("entity.careerDevelopment.status", "zh-HK", "状态", "状态（1=进行中 0=已归档）"),

            // entity.careerDevelopment.relatedplant
            new TranslationSeedItem("entity.careerDevelopment.relatedplant", "en-US", "关联工厂", "关联工厂"),
            // entity.careerDevelopment.relatedplant
            new TranslationSeedItem("entity.careerDevelopment.relatedplant", "ja-JP", "关联工厂", "关联工厂"),
            // entity.careerDevelopment.relatedplant
            new TranslationSeedItem("entity.careerDevelopment.relatedplant", "zh-CN", "关联工厂", "关联工厂"),
            // entity.careerDevelopment.relatedplant
            new TranslationSeedItem("entity.careerDevelopment.relatedplant", "zh-HK", "关联工厂", "关联工厂"),
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
