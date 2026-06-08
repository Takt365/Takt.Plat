// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.TrainingDevelopment
// 文件名称：TaktTrainingResultI18nSeedData.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktTrainingResult 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktTrainingResult 实体国际化翻译种子（键前缀 entity.trainingResult.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktTrainingResultI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktTrainingResult 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 trainingResult 实体翻译...", tenantCode);

        foreach (var item in GetTrainingResultTranslations())
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

        TaktLogger.Information("TaktTrainingResult 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktTrainingResult 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.trainingResult._self / entity.trainingResult.{{field}}；ResourceGroup=TaktModule.HumanResource；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetTrainingResultTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.trainingResult._self
            new TranslationSeedItem("entity.trainingResult._self", "en-US", "Training Result Information", "实体名称"),
            // entity.trainingResult._self
            new TranslationSeedItem("entity.trainingResult._self", "ja-JP", "员工培训结果记录信息", "实体名称"),
            // entity.trainingResult._self
            new TranslationSeedItem("entity.trainingResult._self", "zh-CN", "员工培训结果记录信息", "实体名称"),
            // entity.trainingResult._self
            new TranslationSeedItem("entity.trainingResult._self", "zh-HK", "员工培训结果记录信息", "实体名称"),

            // entity.trainingResult.employeeid
            new TranslationSeedItem("entity.trainingResult.employeeid", "en-US", "员工ID", "员工 ID"),
            // entity.trainingResult.employeeid
            new TranslationSeedItem("entity.trainingResult.employeeid", "ja-JP", "员工ID", "员工 ID"),
            // entity.trainingResult.employeeid
            new TranslationSeedItem("entity.trainingResult.employeeid", "zh-CN", "员工ID", "员工 ID"),
            // entity.trainingResult.employeeid
            new TranslationSeedItem("entity.trainingResult.employeeid", "zh-HK", "员工ID", "员工 ID"),

            // entity.trainingResult.employeename
            new TranslationSeedItem("entity.trainingResult.employeename", "en-US", "员工姓名", "员工姓名"),
            // entity.trainingResult.employeename
            new TranslationSeedItem("entity.trainingResult.employeename", "ja-JP", "员工姓名", "员工姓名"),
            // entity.trainingResult.employeename
            new TranslationSeedItem("entity.trainingResult.employeename", "zh-CN", "员工姓名", "员工姓名"),
            // entity.trainingResult.employeename
            new TranslationSeedItem("entity.trainingResult.employeename", "zh-HK", "员工姓名", "员工姓名"),

            // entity.trainingResult.trainingcourseid
            new TranslationSeedItem("entity.trainingResult.trainingcourseid", "en-US", "培训课程ID", "培训课程 ID"),
            // entity.trainingResult.trainingcourseid
            new TranslationSeedItem("entity.trainingResult.trainingcourseid", "ja-JP", "培训课程ID", "培训课程 ID"),
            // entity.trainingResult.trainingcourseid
            new TranslationSeedItem("entity.trainingResult.trainingcourseid", "zh-CN", "培训课程ID", "培训课程 ID"),
            // entity.trainingResult.trainingcourseid
            new TranslationSeedItem("entity.trainingResult.trainingcourseid", "zh-HK", "培训课程ID", "培训课程 ID"),

            // entity.trainingResult.coursename
            new TranslationSeedItem("entity.trainingResult.coursename", "en-US", "培训课程名称", "培训课程名称"),
            // entity.trainingResult.coursename
            new TranslationSeedItem("entity.trainingResult.coursename", "ja-JP", "培训课程名称", "培训课程名称"),
            // entity.trainingResult.coursename
            new TranslationSeedItem("entity.trainingResult.coursename", "zh-CN", "培训课程名称", "培训课程名称"),
            // entity.trainingResult.coursename
            new TranslationSeedItem("entity.trainingResult.coursename", "zh-HK", "培训课程名称", "培训课程名称"),

            // entity.trainingResult.trainingtype
            new TranslationSeedItem("entity.trainingResult.trainingtype", "en-US", "培训类型", "培训类型"),
            // entity.trainingResult.trainingtype
            new TranslationSeedItem("entity.trainingResult.trainingtype", "ja-JP", "培训类型", "培训类型"),
            // entity.trainingResult.trainingtype
            new TranslationSeedItem("entity.trainingResult.trainingtype", "zh-CN", "培训类型", "培训类型"),
            // entity.trainingResult.trainingtype
            new TranslationSeedItem("entity.trainingResult.trainingtype", "zh-HK", "培训类型", "培训类型"),

            // entity.trainingResult.instructor
            new TranslationSeedItem("entity.trainingResult.instructor", "en-US", "培训讲师", "培训讲师"),
            // entity.trainingResult.instructor
            new TranslationSeedItem("entity.trainingResult.instructor", "ja-JP", "培训讲师", "培训讲师"),
            // entity.trainingResult.instructor
            new TranslationSeedItem("entity.trainingResult.instructor", "zh-CN", "培训讲师", "培训讲师"),
            // entity.trainingResult.instructor
            new TranslationSeedItem("entity.trainingResult.instructor", "zh-HK", "培训讲师", "培训讲师"),

            // entity.trainingResult.trainingstartdate
            new TranslationSeedItem("entity.trainingResult.trainingstartdate", "en-US", "培训开始日期", "培训开始日期"),
            // entity.trainingResult.trainingstartdate
            new TranslationSeedItem("entity.trainingResult.trainingstartdate", "ja-JP", "培训开始日期", "培训开始日期"),
            // entity.trainingResult.trainingstartdate
            new TranslationSeedItem("entity.trainingResult.trainingstartdate", "zh-CN", "培训开始日期", "培训开始日期"),
            // entity.trainingResult.trainingstartdate
            new TranslationSeedItem("entity.trainingResult.trainingstartdate", "zh-HK", "培训开始日期", "培训开始日期"),

            // entity.trainingResult.trainingenddate
            new TranslationSeedItem("entity.trainingResult.trainingenddate", "en-US", "培训结束日期", "培训结束日期"),
            // entity.trainingResult.trainingenddate
            new TranslationSeedItem("entity.trainingResult.trainingenddate", "ja-JP", "培训结束日期", "培训结束日期"),
            // entity.trainingResult.trainingenddate
            new TranslationSeedItem("entity.trainingResult.trainingenddate", "zh-CN", "培训结束日期", "培训结束日期"),
            // entity.trainingResult.trainingenddate
            new TranslationSeedItem("entity.trainingResult.trainingenddate", "zh-HK", "培训结束日期", "培训结束日期"),

            // entity.trainingResult.trainingdate
            new TranslationSeedItem("entity.trainingResult.trainingdate", "en-US", "培训日期", "培训日期"),
            // entity.trainingResult.trainingdate
            new TranslationSeedItem("entity.trainingResult.trainingdate", "ja-JP", "培训日期", "培训日期"),
            // entity.trainingResult.trainingdate
            new TranslationSeedItem("entity.trainingResult.trainingdate", "zh-CN", "培训日期", "培训日期"),
            // entity.trainingResult.trainingdate
            new TranslationSeedItem("entity.trainingResult.trainingdate", "zh-HK", "培训日期", "培训日期"),

            // entity.trainingResult.traininghours
            new TranslationSeedItem("entity.trainingResult.traininghours", "en-US", "培训时长", "培训时长（小时）"),
            // entity.trainingResult.traininghours
            new TranslationSeedItem("entity.trainingResult.traininghours", "ja-JP", "培训时长", "培训时长（小时）"),
            // entity.trainingResult.traininghours
            new TranslationSeedItem("entity.trainingResult.traininghours", "zh-CN", "培训时长", "培训时长（小时）"),
            // entity.trainingResult.traininghours
            new TranslationSeedItem("entity.trainingResult.traininghours", "zh-HK", "培训时长", "培训时长（小时）"),

            // entity.trainingResult.trainingscore
            new TranslationSeedItem("entity.trainingResult.trainingscore", "en-US", "培训成绩", "培训成绩"),
            // entity.trainingResult.trainingscore
            new TranslationSeedItem("entity.trainingResult.trainingscore", "ja-JP", "培训成绩", "培训成绩"),
            // entity.trainingResult.trainingscore
            new TranslationSeedItem("entity.trainingResult.trainingscore", "zh-CN", "培训成绩", "培训成绩"),
            // entity.trainingResult.trainingscore
            new TranslationSeedItem("entity.trainingResult.trainingscore", "zh-HK", "培训成绩", "培训成绩"),

            // entity.trainingResult.ispassed
            new TranslationSeedItem("entity.trainingResult.ispassed", "en-US", "是否通过", "是否通过（0=否 1=是）"),
            // entity.trainingResult.ispassed
            new TranslationSeedItem("entity.trainingResult.ispassed", "ja-JP", "是否通过", "是否通过（0=否 1=是）"),
            // entity.trainingResult.ispassed
            new TranslationSeedItem("entity.trainingResult.ispassed", "zh-CN", "是否通过", "是否通过（0=否 1=是）"),
            // entity.trainingResult.ispassed
            new TranslationSeedItem("entity.trainingResult.ispassed", "zh-HK", "是否通过", "是否通过（0=否 1=是）"),

            // entity.trainingResult.certificateno
            new TranslationSeedItem("entity.trainingResult.certificateno", "en-US", "证书编号", "证书编号"),
            // entity.trainingResult.certificateno
            new TranslationSeedItem("entity.trainingResult.certificateno", "ja-JP", "证书编号", "证书编号"),
            // entity.trainingResult.certificateno
            new TranslationSeedItem("entity.trainingResult.certificateno", "zh-CN", "证书编号", "证书编号"),
            // entity.trainingResult.certificateno
            new TranslationSeedItem("entity.trainingResult.certificateno", "zh-HK", "证书编号", "证书编号"),

            // entity.trainingResult.trainingevaluation
            new TranslationSeedItem("entity.trainingResult.trainingevaluation", "en-US", "培训评价", "培训评价"),
            // entity.trainingResult.trainingevaluation
            new TranslationSeedItem("entity.trainingResult.trainingevaluation", "ja-JP", "培训评价", "培训评价"),
            // entity.trainingResult.trainingevaluation
            new TranslationSeedItem("entity.trainingResult.trainingevaluation", "zh-CN", "培训评价", "培训评价"),
            // entity.trainingResult.trainingevaluation
            new TranslationSeedItem("entity.trainingResult.trainingevaluation", "zh-HK", "培训评价", "培训评价"),

            // entity.trainingResult.status
            new TranslationSeedItem("entity.trainingResult.status", "en-US", "状态", "状态（1=有效 0=无效）"),
            // entity.trainingResult.status
            new TranslationSeedItem("entity.trainingResult.status", "ja-JP", "状态", "状态（1=有效 0=无效）"),
            // entity.trainingResult.status
            new TranslationSeedItem("entity.trainingResult.status", "zh-CN", "状态", "状态（1=有效 0=无效）"),
            // entity.trainingResult.status
            new TranslationSeedItem("entity.trainingResult.status", "zh-HK", "状态", "状态（1=有效 0=无效）"),

            // entity.trainingResult.relatedplant
            new TranslationSeedItem("entity.trainingResult.relatedplant", "en-US", "关联工厂", "关联工厂"),
            // entity.trainingResult.relatedplant
            new TranslationSeedItem("entity.trainingResult.relatedplant", "ja-JP", "关联工厂", "关联工厂"),
            // entity.trainingResult.relatedplant
            new TranslationSeedItem("entity.trainingResult.relatedplant", "zh-CN", "关联工厂", "关联工厂"),
            // entity.trainingResult.relatedplant
            new TranslationSeedItem("entity.trainingResult.relatedplant", "zh-HK", "关联工厂", "关联工厂"),
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
