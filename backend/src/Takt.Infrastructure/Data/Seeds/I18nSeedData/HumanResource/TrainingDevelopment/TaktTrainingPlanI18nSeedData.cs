// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.TrainingDevelopment
// 文件名称：TaktTrainingPlanI18nSeedData.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktTrainingPlan 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktTrainingPlan 实体国际化翻译种子（键前缀 entity.trainingPlan.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktTrainingPlanI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktTrainingPlan 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 trainingPlan 实体翻译...", tenantCode);

        foreach (var item in GetTrainingPlanTranslations())
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

        TaktLogger.Information("TaktTrainingPlan 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktTrainingPlan 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.trainingPlan._self / entity.trainingPlan.{{field}}；ResourceGroup=TaktModule.HumanResource；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetTrainingPlanTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.trainingPlan._self
            new TranslationSeedItem("entity.trainingPlan._self", "en-US", "Training Plan Information", "实体名称"),
            // entity.trainingPlan._self
            new TranslationSeedItem("entity.trainingPlan._self", "ja-JP", "培训计划信息", "实体名称"),
            // entity.trainingPlan._self
            new TranslationSeedItem("entity.trainingPlan._self", "zh-CN", "培训计划信息", "实体名称"),
            // entity.trainingPlan._self
            new TranslationSeedItem("entity.trainingPlan._self", "zh-HK", "培训计划信息", "实体名称"),

            // entity.trainingPlan.plancode
            new TranslationSeedItem("entity.trainingPlan.plancode", "en-US", "计划编码", "计划编码（租户+公司内唯一）"),
            // entity.trainingPlan.plancode
            new TranslationSeedItem("entity.trainingPlan.plancode", "ja-JP", "计划编码", "计划编码（租户+公司内唯一）"),
            // entity.trainingPlan.plancode
            new TranslationSeedItem("entity.trainingPlan.plancode", "zh-CN", "计划编码", "计划编码（租户+公司内唯一）"),
            // entity.trainingPlan.plancode
            new TranslationSeedItem("entity.trainingPlan.plancode", "zh-HK", "计划编码", "计划编码（租户+公司内唯一）"),

            // entity.trainingPlan.planname
            new TranslationSeedItem("entity.trainingPlan.planname", "en-US", "计划名称", "计划名称"),
            // entity.trainingPlan.planname
            new TranslationSeedItem("entity.trainingPlan.planname", "ja-JP", "计划名称", "计划名称"),
            // entity.trainingPlan.planname
            new TranslationSeedItem("entity.trainingPlan.planname", "zh-CN", "计划名称", "计划名称"),
            // entity.trainingPlan.planname
            new TranslationSeedItem("entity.trainingPlan.planname", "zh-HK", "计划名称", "计划名称"),

            // entity.trainingPlan.planyear
            new TranslationSeedItem("entity.trainingPlan.planyear", "en-US", "计划年度", "计划年度"),
            // entity.trainingPlan.planyear
            new TranslationSeedItem("entity.trainingPlan.planyear", "ja-JP", "计划年度", "计划年度"),
            // entity.trainingPlan.planyear
            new TranslationSeedItem("entity.trainingPlan.planyear", "zh-CN", "计划年度", "计划年度"),
            // entity.trainingPlan.planyear
            new TranslationSeedItem("entity.trainingPlan.planyear", "zh-HK", "计划年度", "计划年度"),

            // entity.trainingPlan.plantype
            new TranslationSeedItem("entity.trainingPlan.plantype", "en-US", "计划类型", "计划类型（年度/季度/月度/专项）"),
            // entity.trainingPlan.plantype
            new TranslationSeedItem("entity.trainingPlan.plantype", "ja-JP", "计划类型", "计划类型（年度/季度/月度/专项）"),
            // entity.trainingPlan.plantype
            new TranslationSeedItem("entity.trainingPlan.plantype", "zh-CN", "计划类型", "计划类型（年度/季度/月度/专项）"),
            // entity.trainingPlan.plantype
            new TranslationSeedItem("entity.trainingPlan.plantype", "zh-HK", "计划类型", "计划类型（年度/季度/月度/专项）"),

            // entity.trainingPlan.applicabledepartment
            new TranslationSeedItem("entity.trainingPlan.applicabledepartment", "en-US", "适用部门", "适用部门"),
            // entity.trainingPlan.applicabledepartment
            new TranslationSeedItem("entity.trainingPlan.applicabledepartment", "ja-JP", "适用部门", "适用部门"),
            // entity.trainingPlan.applicabledepartment
            new TranslationSeedItem("entity.trainingPlan.applicabledepartment", "zh-CN", "适用部门", "适用部门"),
            // entity.trainingPlan.applicabledepartment
            new TranslationSeedItem("entity.trainingPlan.applicabledepartment", "zh-HK", "适用部门", "适用部门"),

            // entity.trainingPlan.startdate
            new TranslationSeedItem("entity.trainingPlan.startdate", "en-US", "计划开始日期", "计划开始日期"),
            // entity.trainingPlan.startdate
            new TranslationSeedItem("entity.trainingPlan.startdate", "ja-JP", "计划开始日期", "计划开始日期"),
            // entity.trainingPlan.startdate
            new TranslationSeedItem("entity.trainingPlan.startdate", "zh-CN", "计划开始日期", "计划开始日期"),
            // entity.trainingPlan.startdate
            new TranslationSeedItem("entity.trainingPlan.startdate", "zh-HK", "计划开始日期", "计划开始日期"),

            // entity.trainingPlan.enddate
            new TranslationSeedItem("entity.trainingPlan.enddate", "en-US", "计划结束日期", "计划结束日期"),
            // entity.trainingPlan.enddate
            new TranslationSeedItem("entity.trainingPlan.enddate", "ja-JP", "计划结束日期", "计划结束日期"),
            // entity.trainingPlan.enddate
            new TranslationSeedItem("entity.trainingPlan.enddate", "zh-CN", "计划结束日期", "计划结束日期"),
            // entity.trainingPlan.enddate
            new TranslationSeedItem("entity.trainingPlan.enddate", "zh-HK", "计划结束日期", "计划结束日期"),

            // entity.trainingPlan.trainingobjectives
            new TranslationSeedItem("entity.trainingPlan.trainingobjectives", "en-US", "培训目标", "培训目标"),
            // entity.trainingPlan.trainingobjectives
            new TranslationSeedItem("entity.trainingPlan.trainingobjectives", "ja-JP", "培训目标", "培训目标"),
            // entity.trainingPlan.trainingobjectives
            new TranslationSeedItem("entity.trainingPlan.trainingobjectives", "zh-CN", "培训目标", "培训目标"),
            // entity.trainingPlan.trainingobjectives
            new TranslationSeedItem("entity.trainingPlan.trainingobjectives", "zh-HK", "培训目标", "培训目标"),

            // entity.trainingPlan.plannedheadcount
            new TranslationSeedItem("entity.trainingPlan.plannedheadcount", "en-US", "计划培训人数", "计划培训人数"),
            // entity.trainingPlan.plannedheadcount
            new TranslationSeedItem("entity.trainingPlan.plannedheadcount", "ja-JP", "计划培训人数", "计划培训人数"),
            // entity.trainingPlan.plannedheadcount
            new TranslationSeedItem("entity.trainingPlan.plannedheadcount", "zh-CN", "计划培训人数", "计划培训人数"),
            // entity.trainingPlan.plannedheadcount
            new TranslationSeedItem("entity.trainingPlan.plannedheadcount", "zh-HK", "计划培训人数", "计划培训人数"),

            // entity.trainingPlan.trainingbudget
            new TranslationSeedItem("entity.trainingPlan.trainingbudget", "en-US", "培训预算", "培训预算（元）"),
            // entity.trainingPlan.trainingbudget
            new TranslationSeedItem("entity.trainingPlan.trainingbudget", "ja-JP", "培训预算", "培训预算（元）"),
            // entity.trainingPlan.trainingbudget
            new TranslationSeedItem("entity.trainingPlan.trainingbudget", "zh-CN", "培训预算", "培训预算（元）"),
            // entity.trainingPlan.trainingbudget
            new TranslationSeedItem("entity.trainingPlan.trainingbudget", "zh-HK", "培训预算", "培训预算（元）"),

            // entity.trainingPlan.description
            new TranslationSeedItem("entity.trainingPlan.description", "en-US", "计划说明", "计划说明"),
            // entity.trainingPlan.description
            new TranslationSeedItem("entity.trainingPlan.description", "ja-JP", "计划说明", "计划说明"),
            // entity.trainingPlan.description
            new TranslationSeedItem("entity.trainingPlan.description", "zh-CN", "计划说明", "计划说明"),
            // entity.trainingPlan.description
            new TranslationSeedItem("entity.trainingPlan.description", "zh-HK", "计划说明", "计划说明"),

            // entity.trainingPlan.status
            new TranslationSeedItem("entity.trainingPlan.status", "en-US", "业务状态", "业务状态（1=启用 0=禁用）"),
            // entity.trainingPlan.status
            new TranslationSeedItem("entity.trainingPlan.status", "ja-JP", "业务状态", "业务状态（1=启用 0=禁用）"),
            // entity.trainingPlan.status
            new TranslationSeedItem("entity.trainingPlan.status", "zh-CN", "业务状态", "业务状态（1=启用 0=禁用）"),
            // entity.trainingPlan.status
            new TranslationSeedItem("entity.trainingPlan.status", "zh-HK", "业务状态", "业务状态（1=启用 0=禁用）"),

            // entity.trainingPlan.relatedplant
            new TranslationSeedItem("entity.trainingPlan.relatedplant", "en-US", "关联工厂", "关联工厂"),
            // entity.trainingPlan.relatedplant
            new TranslationSeedItem("entity.trainingPlan.relatedplant", "ja-JP", "关联工厂", "关联工厂"),
            // entity.trainingPlan.relatedplant
            new TranslationSeedItem("entity.trainingPlan.relatedplant", "zh-CN", "关联工厂", "关联工厂"),
            // entity.trainingPlan.relatedplant
            new TranslationSeedItem("entity.trainingPlan.relatedplant", "zh-HK", "关联工厂", "关联工厂"),
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
