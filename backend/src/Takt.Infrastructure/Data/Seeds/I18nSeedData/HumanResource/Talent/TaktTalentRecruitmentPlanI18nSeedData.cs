// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Talent
// 文件名称：TaktTalentRecruitmentPlanI18nSeedData.cs
// 创建时间：2026-06-12
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktTalentRecruitmentPlan 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Talent;

/// <summary>
/// TaktTalentRecruitmentPlan 实体国际化翻译种子（键前缀 entity.talentrecruitmentplan.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktTalentRecruitmentPlanI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktTalentRecruitmentPlan 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 talentrecruitmentplan 实体翻译...", tenantCode);

        foreach (var item in GetTalentRecruitmentPlanTranslations())
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

        TaktLogger.Information("TaktTalentRecruitmentPlan 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktTalentRecruitmentPlan 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.talentrecruitmentplan._self / entity.talentrecruitmentplan.{{field}}；ResourceGroup=5；ResourceType=0
    /// </summary>
    private static List<TranslationSeedItem> GetTalentRecruitmentPlanTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.talentrecruitmentplan._self
            new TranslationSeedItem("entity.talentrecruitmentplan._self", "en-US", "Talent Recruitment Plan Information", "实体名称"),
            // entity.talentrecruitmentplan._self
            new TranslationSeedItem("entity.talentrecruitmentplan._self", "ja-JP", "招聘计划信息", "实体名称"),
            // entity.talentrecruitmentplan._self
            new TranslationSeedItem("entity.talentrecruitmentplan._self", "zh-CN", "招聘计划信息", "实体名称"),
            // entity.talentrecruitmentplan._self
            new TranslationSeedItem("entity.talentrecruitmentplan._self", "zh-HK", "招聘计划信息", "实体名称"),

            // entity.talentrecruitmentplan.staffingrequirementid
            new TranslationSeedItem("entity.talentrecruitmentplan.staffingrequirementid", "en-US", "用人需求ID", "用人需求ID"),
            // entity.talentrecruitmentplan.staffingrequirementid
            new TranslationSeedItem("entity.talentrecruitmentplan.staffingrequirementid", "ja-JP", "用人需求ID", "用人需求ID"),
            // entity.talentrecruitmentplan.staffingrequirementid
            new TranslationSeedItem("entity.talentrecruitmentplan.staffingrequirementid", "zh-CN", "用人需求ID", "用人需求ID"),
            // entity.talentrecruitmentplan.staffingrequirementid
            new TranslationSeedItem("entity.talentrecruitmentplan.staffingrequirementid", "zh-HK", "用人需求ID", "用人需求ID"),

            // entity.talentrecruitmentplan.planno
            new TranslationSeedItem("entity.talentrecruitmentplan.planno", "en-US", "计划单号", "计划单号（租户+公司内业务编号）"),
            // entity.talentrecruitmentplan.planno
            new TranslationSeedItem("entity.talentrecruitmentplan.planno", "ja-JP", "计划单号", "计划单号（租户+公司内业务编号）"),
            // entity.talentrecruitmentplan.planno
            new TranslationSeedItem("entity.talentrecruitmentplan.planno", "zh-CN", "计划单号", "计划单号（租户+公司内业务编号）"),
            // entity.talentrecruitmentplan.planno
            new TranslationSeedItem("entity.talentrecruitmentplan.planno", "zh-HK", "计划单号", "计划单号（租户+公司内业务编号）"),

            // entity.talentrecruitmentplan.plandate
            new TranslationSeedItem("entity.talentrecruitmentplan.plandate", "en-US", "计划制定日期", "计划制定日期"),
            // entity.talentrecruitmentplan.plandate
            new TranslationSeedItem("entity.talentrecruitmentplan.plandate", "ja-JP", "计划制定日期", "计划制定日期"),
            // entity.talentrecruitmentplan.plandate
            new TranslationSeedItem("entity.talentrecruitmentplan.plandate", "zh-CN", "计划制定日期", "计划制定日期"),
            // entity.talentrecruitmentplan.plandate
            new TranslationSeedItem("entity.talentrecruitmentplan.plandate", "zh-HK", "计划制定日期", "计划制定日期"),

            // entity.talentrecruitmentplan.planstartdate
            new TranslationSeedItem("entity.talentrecruitmentplan.planstartdate", "en-US", "计划招聘开始日期", "计划招聘开始日期"),
            // entity.talentrecruitmentplan.planstartdate
            new TranslationSeedItem("entity.talentrecruitmentplan.planstartdate", "ja-JP", "计划招聘开始日期", "计划招聘开始日期"),
            // entity.talentrecruitmentplan.planstartdate
            new TranslationSeedItem("entity.talentrecruitmentplan.planstartdate", "zh-CN", "计划招聘开始日期", "计划招聘开始日期"),
            // entity.talentrecruitmentplan.planstartdate
            new TranslationSeedItem("entity.talentrecruitmentplan.planstartdate", "zh-HK", "计划招聘开始日期", "计划招聘开始日期"),

            // entity.talentrecruitmentplan.planenddate
            new TranslationSeedItem("entity.talentrecruitmentplan.planenddate", "en-US", "计划招聘结束日期", "计划招聘结束日期"),
            // entity.talentrecruitmentplan.planenddate
            new TranslationSeedItem("entity.talentrecruitmentplan.planenddate", "ja-JP", "计划招聘结束日期", "计划招聘结束日期"),
            // entity.talentrecruitmentplan.planenddate
            new TranslationSeedItem("entity.talentrecruitmentplan.planenddate", "zh-CN", "计划招聘结束日期", "计划招聘结束日期"),
            // entity.talentrecruitmentplan.planenddate
            new TranslationSeedItem("entity.talentrecruitmentplan.planenddate", "zh-HK", "计划招聘结束日期", "计划招聘结束日期"),

            // entity.talentrecruitmentplan.planheadcount
            new TranslationSeedItem("entity.talentrecruitmentplan.planheadcount", "en-US", "计划招聘人数", "计划招聘人数"),
            // entity.talentrecruitmentplan.planheadcount
            new TranslationSeedItem("entity.talentrecruitmentplan.planheadcount", "ja-JP", "计划招聘人数", "计划招聘人数"),
            // entity.talentrecruitmentplan.planheadcount
            new TranslationSeedItem("entity.talentrecruitmentplan.planheadcount", "zh-CN", "计划招聘人数", "计划招聘人数"),
            // entity.talentrecruitmentplan.planheadcount
            new TranslationSeedItem("entity.talentrecruitmentplan.planheadcount", "zh-HK", "计划招聘人数", "计划招聘人数"),

            // entity.talentrecruitmentplan.reason
            new TranslationSeedItem("entity.talentrecruitmentplan.reason", "en-US", "计划说明", "计划说明"),
            // entity.talentrecruitmentplan.reason
            new TranslationSeedItem("entity.talentrecruitmentplan.reason", "ja-JP", "计划说明", "计划说明"),
            // entity.talentrecruitmentplan.reason
            new TranslationSeedItem("entity.talentrecruitmentplan.reason", "zh-CN", "计划说明", "计划说明"),
            // entity.talentrecruitmentplan.reason
            new TranslationSeedItem("entity.talentrecruitmentplan.reason", "zh-HK", "计划说明", "计划说明"),

            // entity.talentrecruitmentplan.staffingrequirement
            new TranslationSeedItem("entity.talentrecruitmentplan.staffingrequirement", "en-US", "用人需求", "用人需求"),
            // entity.talentrecruitmentplan.staffingrequirement
            new TranslationSeedItem("entity.talentrecruitmentplan.staffingrequirement", "ja-JP", "用人需求", "用人需求"),
            // entity.talentrecruitmentplan.staffingrequirement
            new TranslationSeedItem("entity.talentrecruitmentplan.staffingrequirement", "zh-CN", "用人需求", "用人需求"),
            // entity.talentrecruitmentplan.staffingrequirement
            new TranslationSeedItem("entity.talentrecruitmentplan.staffingrequirement", "zh-HK", "用人需求", "用人需求"),

            // entity.talentrecruitmentplan.talentjobpostings
            new TranslationSeedItem("entity.talentrecruitmentplan.talentjobpostings", "en-US", "职位发布", "职位发布"),
            // entity.talentrecruitmentplan.talentjobpostings
            new TranslationSeedItem("entity.talentrecruitmentplan.talentjobpostings", "ja-JP", "职位发布", "职位发布"),
            // entity.talentrecruitmentplan.talentjobpostings
            new TranslationSeedItem("entity.talentrecruitmentplan.talentjobpostings", "zh-CN", "职位发布", "职位发布"),
            // entity.talentrecruitmentplan.talentjobpostings
            new TranslationSeedItem("entity.talentrecruitmentplan.talentjobpostings", "zh-HK", "职位发布", "职位发布"),
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
        translation.ResourceGroup = 5;
        translation.ResourceType = 0;
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
