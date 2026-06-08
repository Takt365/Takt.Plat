// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Performance
// 文件名称：TaktSchemeMetricI18nSeedData.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSchemeMetric 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktSchemeMetric 实体国际化翻译种子（键前缀 entity.schemeMetric.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSchemeMetricI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSchemeMetric 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 schemeMetric 实体翻译...", tenantCode);

        foreach (var item in GetSchemeMetricTranslations())
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

        TaktLogger.Information("TaktSchemeMetric 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSchemeMetric 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.schemeMetric._self / entity.schemeMetric.{{field}}；ResourceGroup=TaktModule.HumanResource；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSchemeMetricTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.schemeMetric._self
            new TranslationSeedItem("entity.schemeMetric._self", "en-US", "Scheme Metric Information", "实体名称"),
            // entity.schemeMetric._self
            new TranslationSeedItem("entity.schemeMetric._self", "ja-JP", "绩效方案指标信息", "实体名称"),
            // entity.schemeMetric._self
            new TranslationSeedItem("entity.schemeMetric._self", "zh-CN", "绩效方案指标信息", "实体名称"),
            // entity.schemeMetric._self
            new TranslationSeedItem("entity.schemeMetric._self", "zh-HK", "绩效方案指标信息", "实体名称"),

            // entity.schemeMetric.schemecode
            new TranslationSeedItem("entity.schemeMetric.schemecode", "en-US", "方案编码", "方案编码"),
            // entity.schemeMetric.schemecode
            new TranslationSeedItem("entity.schemeMetric.schemecode", "ja-JP", "方案编码", "方案编码"),
            // entity.schemeMetric.schemecode
            new TranslationSeedItem("entity.schemeMetric.schemecode", "zh-CN", "方案编码", "方案编码"),
            // entity.schemeMetric.schemecode
            new TranslationSeedItem("entity.schemeMetric.schemecode", "zh-HK", "方案编码", "方案编码"),

            // entity.schemeMetric.schemename
            new TranslationSeedItem("entity.schemeMetric.schemename", "en-US", "方案名称", "方案名称"),
            // entity.schemeMetric.schemename
            new TranslationSeedItem("entity.schemeMetric.schemename", "ja-JP", "方案名称", "方案名称"),
            // entity.schemeMetric.schemename
            new TranslationSeedItem("entity.schemeMetric.schemename", "zh-CN", "方案名称", "方案名称"),
            // entity.schemeMetric.schemename
            new TranslationSeedItem("entity.schemeMetric.schemename", "zh-HK", "方案名称", "方案名称"),

            // entity.schemeMetric.applicabledepartment
            new TranslationSeedItem("entity.schemeMetric.applicabledepartment", "en-US", "适用部门", "适用部门"),
            // entity.schemeMetric.applicabledepartment
            new TranslationSeedItem("entity.schemeMetric.applicabledepartment", "ja-JP", "适用部门", "适用部门"),
            // entity.schemeMetric.applicabledepartment
            new TranslationSeedItem("entity.schemeMetric.applicabledepartment", "zh-CN", "适用部门", "适用部门"),
            // entity.schemeMetric.applicabledepartment
            new TranslationSeedItem("entity.schemeMetric.applicabledepartment", "zh-HK", "适用部门", "适用部门"),

            // entity.schemeMetric.cycletype
            new TranslationSeedItem("entity.schemeMetric.cycletype", "en-US", "考核周期类型", "考核周期类型（月度/季度/半年度/年度）"),
            // entity.schemeMetric.cycletype
            new TranslationSeedItem("entity.schemeMetric.cycletype", "ja-JP", "考核周期类型", "考核周期类型（月度/季度/半年度/年度）"),
            // entity.schemeMetric.cycletype
            new TranslationSeedItem("entity.schemeMetric.cycletype", "zh-CN", "考核周期类型", "考核周期类型（月度/季度/半年度/年度）"),
            // entity.schemeMetric.cycletype
            new TranslationSeedItem("entity.schemeMetric.cycletype", "zh-HK", "考核周期类型", "考核周期类型（月度/季度/半年度/年度）"),

            // entity.schemeMetric.scoringstandard
            new TranslationSeedItem("entity.schemeMetric.scoringstandard", "en-US", "评分标准", "评分标准（百分制/五分制/等级制）"),
            // entity.schemeMetric.scoringstandard
            new TranslationSeedItem("entity.schemeMetric.scoringstandard", "ja-JP", "评分标准", "评分标准（百分制/五分制/等级制）"),
            // entity.schemeMetric.scoringstandard
            new TranslationSeedItem("entity.schemeMetric.scoringstandard", "zh-CN", "评分标准", "评分标准（百分制/五分制/等级制）"),
            // entity.schemeMetric.scoringstandard
            new TranslationSeedItem("entity.schemeMetric.scoringstandard", "zh-HK", "评分标准", "评分标准（百分制/五分制/等级制）"),

            // entity.schemeMetric.selfevaluationweight
            new TranslationSeedItem("entity.schemeMetric.selfevaluationweight", "en-US", "自评权重", "自评权重（%）"),
            // entity.schemeMetric.selfevaluationweight
            new TranslationSeedItem("entity.schemeMetric.selfevaluationweight", "ja-JP", "自评权重", "自评权重（%）"),
            // entity.schemeMetric.selfevaluationweight
            new TranslationSeedItem("entity.schemeMetric.selfevaluationweight", "zh-CN", "自评权重", "自评权重（%）"),
            // entity.schemeMetric.selfevaluationweight
            new TranslationSeedItem("entity.schemeMetric.selfevaluationweight", "zh-HK", "自评权重", "自评权重（%）"),

            // entity.schemeMetric.supervisorweight
            new TranslationSeedItem("entity.schemeMetric.supervisorweight", "en-US", "主管评分权重", "主管评分权重（%）"),
            // entity.schemeMetric.supervisorweight
            new TranslationSeedItem("entity.schemeMetric.supervisorweight", "ja-JP", "主管评分权重", "主管评分权重（%）"),
            // entity.schemeMetric.supervisorweight
            new TranslationSeedItem("entity.schemeMetric.supervisorweight", "zh-CN", "主管评分权重", "主管评分权重（%）"),
            // entity.schemeMetric.supervisorweight
            new TranslationSeedItem("entity.schemeMetric.supervisorweight", "zh-HK", "主管评分权重", "主管评分权重（%）"),

            // entity.schemeMetric.metriccode
            new TranslationSeedItem("entity.schemeMetric.metriccode", "en-US", "指标编码", "指标编码"),
            // entity.schemeMetric.metriccode
            new TranslationSeedItem("entity.schemeMetric.metriccode", "ja-JP", "指标编码", "指标编码"),
            // entity.schemeMetric.metriccode
            new TranslationSeedItem("entity.schemeMetric.metriccode", "zh-CN", "指标编码", "指标编码"),
            // entity.schemeMetric.metriccode
            new TranslationSeedItem("entity.schemeMetric.metriccode", "zh-HK", "指标编码", "指标编码"),

            // entity.schemeMetric.metricname
            new TranslationSeedItem("entity.schemeMetric.metricname", "en-US", "指标名称", "指标名称"),
            // entity.schemeMetric.metricname
            new TranslationSeedItem("entity.schemeMetric.metricname", "ja-JP", "指标名称", "指标名称"),
            // entity.schemeMetric.metricname
            new TranslationSeedItem("entity.schemeMetric.metricname", "zh-CN", "指标名称", "指标名称"),
            // entity.schemeMetric.metricname
            new TranslationSeedItem("entity.schemeMetric.metricname", "zh-HK", "指标名称", "指标名称"),

            // entity.schemeMetric.category
            new TranslationSeedItem("entity.schemeMetric.category", "en-US", "指标类别", "指标类别（业绩/能力/态度/管理/创新/质量/效率/安全）"),
            // entity.schemeMetric.category
            new TranslationSeedItem("entity.schemeMetric.category", "ja-JP", "指标类别", "指标类别（业绩/能力/态度/管理/创新/质量/效率/安全）"),
            // entity.schemeMetric.category
            new TranslationSeedItem("entity.schemeMetric.category", "zh-CN", "指标类别", "指标类别（业绩/能力/态度/管理/创新/质量/效率/安全）"),
            // entity.schemeMetric.category
            new TranslationSeedItem("entity.schemeMetric.category", "zh-HK", "指标类别", "指标类别（业绩/能力/态度/管理/创新/质量/效率/安全）"),

            // entity.schemeMetric.metrictype
            new TranslationSeedItem("entity.schemeMetric.metrictype", "en-US", "指标类型", "指标类型（定量/定性）"),
            // entity.schemeMetric.metrictype
            new TranslationSeedItem("entity.schemeMetric.metrictype", "ja-JP", "指标类型", "指标类型（定量/定性）"),
            // entity.schemeMetric.metrictype
            new TranslationSeedItem("entity.schemeMetric.metrictype", "zh-CN", "指标类型", "指标类型（定量/定性）"),
            // entity.schemeMetric.metrictype
            new TranslationSeedItem("entity.schemeMetric.metrictype", "zh-HK", "指标类型", "指标类型（定量/定性）"),

            // entity.schemeMetric.scoringcriteria
            new TranslationSeedItem("entity.schemeMetric.scoringcriteria", "en-US", "评分标准说明", "评分标准说明"),
            // entity.schemeMetric.scoringcriteria
            new TranslationSeedItem("entity.schemeMetric.scoringcriteria", "ja-JP", "评分标准说明", "评分标准说明"),
            // entity.schemeMetric.scoringcriteria
            new TranslationSeedItem("entity.schemeMetric.scoringcriteria", "zh-CN", "评分标准说明", "评分标准说明"),
            // entity.schemeMetric.scoringcriteria
            new TranslationSeedItem("entity.schemeMetric.scoringcriteria", "zh-HK", "评分标准说明", "评分标准说明"),

            // entity.schemeMetric.standardweight
            new TranslationSeedItem("entity.schemeMetric.standardweight", "en-US", "标准权重", "标准权重（%）"),
            // entity.schemeMetric.standardweight
            new TranslationSeedItem("entity.schemeMetric.standardweight", "ja-JP", "标准权重", "标准权重（%）"),
            // entity.schemeMetric.standardweight
            new TranslationSeedItem("entity.schemeMetric.standardweight", "zh-CN", "标准权重", "标准权重（%）"),
            // entity.schemeMetric.standardweight
            new TranslationSeedItem("entity.schemeMetric.standardweight", "zh-HK", "标准权重", "标准权重（%）"),

            // entity.schemeMetric.sortorder
            new TranslationSeedItem("entity.schemeMetric.sortorder", "en-US", "排序号", "排序号"),
            // entity.schemeMetric.sortorder
            new TranslationSeedItem("entity.schemeMetric.sortorder", "ja-JP", "排序号", "排序号"),
            // entity.schemeMetric.sortorder
            new TranslationSeedItem("entity.schemeMetric.sortorder", "zh-CN", "排序号", "排序号"),
            // entity.schemeMetric.sortorder
            new TranslationSeedItem("entity.schemeMetric.sortorder", "zh-HK", "排序号", "排序号"),

            // entity.schemeMetric.status
            new TranslationSeedItem("entity.schemeMetric.status", "en-US", "状态", "状态（0=启用 1=停用）"),
            // entity.schemeMetric.status
            new TranslationSeedItem("entity.schemeMetric.status", "ja-JP", "状态", "状态（0=启用 1=停用）"),
            // entity.schemeMetric.status
            new TranslationSeedItem("entity.schemeMetric.status", "zh-CN", "状态", "状态（0=启用 1=停用）"),
            // entity.schemeMetric.status
            new TranslationSeedItem("entity.schemeMetric.status", "zh-HK", "状态", "状态（0=启用 1=停用）"),

            // entity.schemeMetric.relatedplant
            new TranslationSeedItem("entity.schemeMetric.relatedplant", "en-US", "关联工厂", "关联工厂"),
            // entity.schemeMetric.relatedplant
            new TranslationSeedItem("entity.schemeMetric.relatedplant", "ja-JP", "关联工厂", "关联工厂"),
            // entity.schemeMetric.relatedplant
            new TranslationSeedItem("entity.schemeMetric.relatedplant", "zh-CN", "关联工厂", "关联工厂"),
            // entity.schemeMetric.relatedplant
            new TranslationSeedItem("entity.schemeMetric.relatedplant", "zh-HK", "关联工厂", "关联工厂"),
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
