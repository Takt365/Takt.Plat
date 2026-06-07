// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.TrainingDevelopment
// 文件名称：TaktTrainingCourseI18nSeedData.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktTrainingCourse 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktTrainingCourse 实体国际化翻译种子（键前缀 entity.trainingCourse.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktTrainingCourseI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktTrainingCourse 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 trainingCourse 实体翻译...", tenantCode);

        foreach (var item in GetTrainingCourseTranslations())
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

        TaktLogger.Information("TaktTrainingCourse 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktTrainingCourse 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.trainingCourse._self / entity.trainingCourse.{{field}}；ResourceGroup=TaktModule.HumanResource；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetTrainingCourseTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.trainingCourse._self
            new TranslationSeedItem("entity.trainingCourse._self", "en-US", "Training Course Information", "实体名称"),
            // entity.trainingCourse._self
            new TranslationSeedItem("entity.trainingCourse._self", "ja-JP", "培训课程定义信息", "实体名称"),
            // entity.trainingCourse._self
            new TranslationSeedItem("entity.trainingCourse._self", "zh-CN", "培训课程定义信息", "实体名称"),
            // entity.trainingCourse._self
            new TranslationSeedItem("entity.trainingCourse._self", "zh-HK", "培训课程定义信息", "实体名称"),

            // entity.trainingCourse.coursecode
            new TranslationSeedItem("entity.trainingCourse.coursecode", "en-US", "课程编码", "课程编码（租户+公司内唯一）"),
            // entity.trainingCourse.coursecode
            new TranslationSeedItem("entity.trainingCourse.coursecode", "ja-JP", "课程编码", "课程编码（租户+公司内唯一）"),
            // entity.trainingCourse.coursecode
            new TranslationSeedItem("entity.trainingCourse.coursecode", "zh-CN", "课程编码", "课程编码（租户+公司内唯一）"),
            // entity.trainingCourse.coursecode
            new TranslationSeedItem("entity.trainingCourse.coursecode", "zh-HK", "课程编码", "课程编码（租户+公司内唯一）"),

            // entity.trainingCourse.coursename
            new TranslationSeedItem("entity.trainingCourse.coursename", "en-US", "课程名称", "课程名称"),
            // entity.trainingCourse.coursename
            new TranslationSeedItem("entity.trainingCourse.coursename", "ja-JP", "课程名称", "课程名称"),
            // entity.trainingCourse.coursename
            new TranslationSeedItem("entity.trainingCourse.coursename", "zh-CN", "课程名称", "课程名称"),
            // entity.trainingCourse.coursename
            new TranslationSeedItem("entity.trainingCourse.coursename", "zh-HK", "课程名称", "课程名称"),

            // entity.trainingCourse.coursetype
            new TranslationSeedItem("entity.trainingCourse.coursetype", "en-US", "课程类型", "课程类型（入职培训/技能培训/管理培训/安全培训/专业培训）"),
            // entity.trainingCourse.coursetype
            new TranslationSeedItem("entity.trainingCourse.coursetype", "ja-JP", "课程类型", "课程类型（入职培训/技能培训/管理培训/安全培训/专业培训）"),
            // entity.trainingCourse.coursetype
            new TranslationSeedItem("entity.trainingCourse.coursetype", "zh-CN", "课程类型", "课程类型（入职培训/技能培训/管理培训/安全培训/专业培训）"),
            // entity.trainingCourse.coursetype
            new TranslationSeedItem("entity.trainingCourse.coursetype", "zh-HK", "课程类型", "课程类型（入职培训/技能培训/管理培训/安全培训/专业培训）"),

            // entity.trainingCourse.courselevel
            new TranslationSeedItem("entity.trainingCourse.courselevel", "en-US", "课程级别", "课程级别（初级/中级/高级/专家）"),
            // entity.trainingCourse.courselevel
            new TranslationSeedItem("entity.trainingCourse.courselevel", "ja-JP", "课程级别", "课程级别（初级/中级/高级/专家）"),
            // entity.trainingCourse.courselevel
            new TranslationSeedItem("entity.trainingCourse.courselevel", "zh-CN", "课程级别", "课程级别（初级/中级/高级/专家）"),
            // entity.trainingCourse.courselevel
            new TranslationSeedItem("entity.trainingCourse.courselevel", "zh-HK", "课程级别", "课程级别（初级/中级/高级/专家）"),

            // entity.trainingCourse.coursedescription
            new TranslationSeedItem("entity.trainingCourse.coursedescription", "en-US", "课程描述", "课程描述"),
            // entity.trainingCourse.coursedescription
            new TranslationSeedItem("entity.trainingCourse.coursedescription", "ja-JP", "课程描述", "课程描述"),
            // entity.trainingCourse.coursedescription
            new TranslationSeedItem("entity.trainingCourse.coursedescription", "zh-CN", "课程描述", "课程描述"),
            // entity.trainingCourse.coursedescription
            new TranslationSeedItem("entity.trainingCourse.coursedescription", "zh-HK", "课程描述", "课程描述"),

            // entity.trainingCourse.courseobjectives
            new TranslationSeedItem("entity.trainingCourse.courseobjectives", "en-US", "课程目标", "课程目标"),
            // entity.trainingCourse.courseobjectives
            new TranslationSeedItem("entity.trainingCourse.courseobjectives", "ja-JP", "课程目标", "课程目标"),
            // entity.trainingCourse.courseobjectives
            new TranslationSeedItem("entity.trainingCourse.courseobjectives", "zh-CN", "课程目标", "课程目标"),
            // entity.trainingCourse.courseobjectives
            new TranslationSeedItem("entity.trainingCourse.courseobjectives", "zh-HK", "课程目标", "课程目标"),

            // entity.trainingCourse.traininghours
            new TranslationSeedItem("entity.trainingCourse.traininghours", "en-US", "培训时长", "培训时长（小时）"),
            // entity.trainingCourse.traininghours
            new TranslationSeedItem("entity.trainingCourse.traininghours", "ja-JP", "培训时长", "培训时长（小时）"),
            // entity.trainingCourse.traininghours
            new TranslationSeedItem("entity.trainingCourse.traininghours", "zh-CN", "培训时长", "培训时长（小时）"),
            // entity.trainingCourse.traininghours
            new TranslationSeedItem("entity.trainingCourse.traininghours", "zh-HK", "培训时长", "培训时长（小时）"),

            // entity.trainingCourse.maininstructor
            new TranslationSeedItem("entity.trainingCourse.maininstructor", "en-US", "主讲讲师", "主讲讲师"),
            // entity.trainingCourse.maininstructor
            new TranslationSeedItem("entity.trainingCourse.maininstructor", "ja-JP", "主讲讲师", "主讲讲师"),
            // entity.trainingCourse.maininstructor
            new TranslationSeedItem("entity.trainingCourse.maininstructor", "zh-CN", "主讲讲师", "主讲讲师"),
            // entity.trainingCourse.maininstructor
            new TranslationSeedItem("entity.trainingCourse.maininstructor", "zh-HK", "主讲讲师", "主讲讲师"),

            // entity.trainingCourse.trainingmethod
            new TranslationSeedItem("entity.trainingCourse.trainingmethod", "en-US", "培训方式", "培训方式（线下/线上/混合）"),
            // entity.trainingCourse.trainingmethod
            new TranslationSeedItem("entity.trainingCourse.trainingmethod", "ja-JP", "培训方式", "培训方式（线下/线上/混合）"),
            // entity.trainingCourse.trainingmethod
            new TranslationSeedItem("entity.trainingCourse.trainingmethod", "zh-CN", "培训方式", "培训方式（线下/线上/混合）"),
            // entity.trainingCourse.trainingmethod
            new TranslationSeedItem("entity.trainingCourse.trainingmethod", "zh-HK", "培训方式", "培训方式（线下/线上/混合）"),

            // entity.trainingCourse.assessmentmethod
            new TranslationSeedItem("entity.trainingCourse.assessmentmethod", "en-US", "考核方式", "考核方式（考试/实操/作业/无）"),
            // entity.trainingCourse.assessmentmethod
            new TranslationSeedItem("entity.trainingCourse.assessmentmethod", "ja-JP", "考核方式", "考核方式（考试/实操/作业/无）"),
            // entity.trainingCourse.assessmentmethod
            new TranslationSeedItem("entity.trainingCourse.assessmentmethod", "zh-CN", "考核方式", "考核方式（考试/实操/作业/无）"),
            // entity.trainingCourse.assessmentmethod
            new TranslationSeedItem("entity.trainingCourse.assessmentmethod", "zh-HK", "考核方式", "考核方式（考试/实操/作业/无）"),

            // entity.trainingCourse.passingscore
            new TranslationSeedItem("entity.trainingCourse.passingscore", "en-US", "及格分数线", "及格分数线"),
            // entity.trainingCourse.passingscore
            new TranslationSeedItem("entity.trainingCourse.passingscore", "ja-JP", "及格分数线", "及格分数线"),
            // entity.trainingCourse.passingscore
            new TranslationSeedItem("entity.trainingCourse.passingscore", "zh-CN", "及格分数线", "及格分数线"),
            // entity.trainingCourse.passingscore
            new TranslationSeedItem("entity.trainingCourse.passingscore", "zh-HK", "及格分数线", "及格分数线"),

            // entity.trainingCourse.sortorder
            new TranslationSeedItem("entity.trainingCourse.sortorder", "en-US", "排序号", "排序号"),
            // entity.trainingCourse.sortorder
            new TranslationSeedItem("entity.trainingCourse.sortorder", "ja-JP", "排序号", "排序号"),
            // entity.trainingCourse.sortorder
            new TranslationSeedItem("entity.trainingCourse.sortorder", "zh-CN", "排序号", "排序号"),
            // entity.trainingCourse.sortorder
            new TranslationSeedItem("entity.trainingCourse.sortorder", "zh-HK", "排序号", "排序号"),

            // entity.trainingCourse.status
            new TranslationSeedItem("entity.trainingCourse.status", "en-US", "状态", "状态（1=启用 0=禁用）"),
            // entity.trainingCourse.status
            new TranslationSeedItem("entity.trainingCourse.status", "ja-JP", "状态", "状态（1=启用 0=禁用）"),
            // entity.trainingCourse.status
            new TranslationSeedItem("entity.trainingCourse.status", "zh-CN", "状态", "状态（1=启用 0=禁用）"),
            // entity.trainingCourse.status
            new TranslationSeedItem("entity.trainingCourse.status", "zh-HK", "状态", "状态（1=启用 0=禁用）"),

            // entity.trainingCourse.relatedplant
            new TranslationSeedItem("entity.trainingCourse.relatedplant", "en-US", "关联工厂", "关联工厂"),
            // entity.trainingCourse.relatedplant
            new TranslationSeedItem("entity.trainingCourse.relatedplant", "ja-JP", "关联工厂", "关联工厂"),
            // entity.trainingCourse.relatedplant
            new TranslationSeedItem("entity.trainingCourse.relatedplant", "zh-CN", "关联工厂", "关联工厂"),
            // entity.trainingCourse.relatedplant
            new TranslationSeedItem("entity.trainingCourse.relatedplant", "zh-HK", "关联工厂", "关联工厂"),
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
