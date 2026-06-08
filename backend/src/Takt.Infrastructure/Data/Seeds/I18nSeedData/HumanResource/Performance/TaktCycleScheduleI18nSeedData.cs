// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Performance
// 文件名称：TaktCycleScheduleI18nSeedData.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktCycleSchedule 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktCycleSchedule 实体国际化翻译种子（键前缀 entity.cycleSchedule.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktCycleScheduleI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktCycleSchedule 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 cycleSchedule 实体翻译...", tenantCode);

        foreach (var item in GetCycleScheduleTranslations())
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

        TaktLogger.Information("TaktCycleSchedule 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktCycleSchedule 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.cycleSchedule._self / entity.cycleSchedule.{{field}}；ResourceGroup=TaktModule.HumanResource；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetCycleScheduleTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.cycleSchedule._self
            new TranslationSeedItem("entity.cycleSchedule._self", "en-US", "Cycle Schedule Information", "实体名称"),
            // entity.cycleSchedule._self
            new TranslationSeedItem("entity.cycleSchedule._self", "ja-JP", "绩效考核周期日程安排信息", "实体名称"),
            // entity.cycleSchedule._self
            new TranslationSeedItem("entity.cycleSchedule._self", "zh-CN", "绩效考核周期日程安排信息", "实体名称"),
            // entity.cycleSchedule._self
            new TranslationSeedItem("entity.cycleSchedule._self", "zh-HK", "绩效考核周期日程安排信息", "实体名称"),

            // entity.cycleSchedule.cyclecode
            new TranslationSeedItem("entity.cycleSchedule.cyclecode", "en-US", "周期编码", "周期编码（租户+公司内唯一）"),
            // entity.cycleSchedule.cyclecode
            new TranslationSeedItem("entity.cycleSchedule.cyclecode", "ja-JP", "周期编码", "周期编码（租户+公司内唯一）"),
            // entity.cycleSchedule.cyclecode
            new TranslationSeedItem("entity.cycleSchedule.cyclecode", "zh-CN", "周期编码", "周期编码（租户+公司内唯一）"),
            // entity.cycleSchedule.cyclecode
            new TranslationSeedItem("entity.cycleSchedule.cyclecode", "zh-HK", "周期编码", "周期编码（租户+公司内唯一）"),

            // entity.cycleSchedule.cyclename
            new TranslationSeedItem("entity.cycleSchedule.cyclename", "en-US", "周期名称", "周期名称"),
            // entity.cycleSchedule.cyclename
            new TranslationSeedItem("entity.cycleSchedule.cyclename", "ja-JP", "周期名称", "周期名称"),
            // entity.cycleSchedule.cyclename
            new TranslationSeedItem("entity.cycleSchedule.cyclename", "zh-CN", "周期名称", "周期名称"),
            // entity.cycleSchedule.cyclename
            new TranslationSeedItem("entity.cycleSchedule.cyclename", "zh-HK", "周期名称", "周期名称"),

            // entity.cycleSchedule.cycletype
            new TranslationSeedItem("entity.cycleSchedule.cycletype", "en-US", "周期类型", "周期类型（月度/季度/半年度/年度）"),
            // entity.cycleSchedule.cycletype
            new TranslationSeedItem("entity.cycleSchedule.cycletype", "ja-JP", "周期类型", "周期类型（月度/季度/半年度/年度）"),
            // entity.cycleSchedule.cycletype
            new TranslationSeedItem("entity.cycleSchedule.cycletype", "zh-CN", "周期类型", "周期类型（月度/季度/半年度/年度）"),
            // entity.cycleSchedule.cycletype
            new TranslationSeedItem("entity.cycleSchedule.cycletype", "zh-HK", "周期类型", "周期类型（月度/季度/半年度/年度）"),

            // entity.cycleSchedule.cycleyear
            new TranslationSeedItem("entity.cycleSchedule.cycleyear", "en-US", "周期年度", "周期年度"),
            // entity.cycleSchedule.cycleyear
            new TranslationSeedItem("entity.cycleSchedule.cycleyear", "ja-JP", "周期年度", "周期年度"),
            // entity.cycleSchedule.cycleyear
            new TranslationSeedItem("entity.cycleSchedule.cycleyear", "zh-CN", "周期年度", "周期年度"),
            // entity.cycleSchedule.cycleyear
            new TranslationSeedItem("entity.cycleSchedule.cycleyear", "zh-HK", "周期年度", "周期年度"),

            // entity.cycleSchedule.cyclesequence
            new TranslationSeedItem("entity.cycleSchedule.cyclesequence", "en-US", "周期序号", "周期序号"),
            // entity.cycleSchedule.cyclesequence
            new TranslationSeedItem("entity.cycleSchedule.cyclesequence", "ja-JP", "周期序号", "周期序号"),
            // entity.cycleSchedule.cyclesequence
            new TranslationSeedItem("entity.cycleSchedule.cyclesequence", "zh-CN", "周期序号", "周期序号"),
            // entity.cycleSchedule.cyclesequence
            new TranslationSeedItem("entity.cycleSchedule.cyclesequence", "zh-HK", "周期序号", "周期序号"),

            // entity.cycleSchedule.startdate
            new TranslationSeedItem("entity.cycleSchedule.startdate", "en-US", "开始日期", "开始日期"),
            // entity.cycleSchedule.startdate
            new TranslationSeedItem("entity.cycleSchedule.startdate", "ja-JP", "开始日期", "开始日期"),
            // entity.cycleSchedule.startdate
            new TranslationSeedItem("entity.cycleSchedule.startdate", "zh-CN", "开始日期", "开始日期"),
            // entity.cycleSchedule.startdate
            new TranslationSeedItem("entity.cycleSchedule.startdate", "zh-HK", "开始日期", "开始日期"),

            // entity.cycleSchedule.enddate
            new TranslationSeedItem("entity.cycleSchedule.enddate", "en-US", "结束日期", "结束日期"),
            // entity.cycleSchedule.enddate
            new TranslationSeedItem("entity.cycleSchedule.enddate", "ja-JP", "结束日期", "结束日期"),
            // entity.cycleSchedule.enddate
            new TranslationSeedItem("entity.cycleSchedule.enddate", "zh-CN", "结束日期", "结束日期"),
            // entity.cycleSchedule.enddate
            new TranslationSeedItem("entity.cycleSchedule.enddate", "zh-HK", "结束日期", "结束日期"),

            // entity.cycleSchedule.goalsettingduedate
            new TranslationSeedItem("entity.cycleSchedule.goalsettingduedate", "en-US", "目标设定截止日期", "目标设定截止日期"),
            // entity.cycleSchedule.goalsettingduedate
            new TranslationSeedItem("entity.cycleSchedule.goalsettingduedate", "ja-JP", "目标设定截止日期", "目标设定截止日期"),
            // entity.cycleSchedule.goalsettingduedate
            new TranslationSeedItem("entity.cycleSchedule.goalsettingduedate", "zh-CN", "目标设定截止日期", "目标设定截止日期"),
            // entity.cycleSchedule.goalsettingduedate
            new TranslationSeedItem("entity.cycleSchedule.goalsettingduedate", "zh-HK", "目标设定截止日期", "目标设定截止日期"),

            // entity.cycleSchedule.selfevaluationduedate
            new TranslationSeedItem("entity.cycleSchedule.selfevaluationduedate", "en-US", "自评截止日期", "自评截止日期"),
            // entity.cycleSchedule.selfevaluationduedate
            new TranslationSeedItem("entity.cycleSchedule.selfevaluationduedate", "ja-JP", "自评截止日期", "自评截止日期"),
            // entity.cycleSchedule.selfevaluationduedate
            new TranslationSeedItem("entity.cycleSchedule.selfevaluationduedate", "zh-CN", "自评截止日期", "自评截止日期"),
            // entity.cycleSchedule.selfevaluationduedate
            new TranslationSeedItem("entity.cycleSchedule.selfevaluationduedate", "zh-HK", "自评截止日期", "自评截止日期"),

            // entity.cycleSchedule.supervisorreviewduedate
            new TranslationSeedItem("entity.cycleSchedule.supervisorreviewduedate", "en-US", "主管评审截止日期", "主管评审截止日期"),
            // entity.cycleSchedule.supervisorreviewduedate
            new TranslationSeedItem("entity.cycleSchedule.supervisorreviewduedate", "ja-JP", "主管评审截止日期", "主管评审截止日期"),
            // entity.cycleSchedule.supervisorreviewduedate
            new TranslationSeedItem("entity.cycleSchedule.supervisorreviewduedate", "zh-CN", "主管评审截止日期", "主管评审截止日期"),
            // entity.cycleSchedule.supervisorreviewduedate
            new TranslationSeedItem("entity.cycleSchedule.supervisorreviewduedate", "zh-HK", "主管评审截止日期", "主管评审截止日期"),

            // entity.cycleSchedule.interviewduedate
            new TranslationSeedItem("entity.cycleSchedule.interviewduedate", "en-US", "面谈截止日期", "面谈截止日期"),
            // entity.cycleSchedule.interviewduedate
            new TranslationSeedItem("entity.cycleSchedule.interviewduedate", "ja-JP", "面谈截止日期", "面谈截止日期"),
            // entity.cycleSchedule.interviewduedate
            new TranslationSeedItem("entity.cycleSchedule.interviewduedate", "zh-CN", "面谈截止日期", "面谈截止日期"),
            // entity.cycleSchedule.interviewduedate
            new TranslationSeedItem("entity.cycleSchedule.interviewduedate", "zh-HK", "面谈截止日期", "面谈截止日期"),

            // entity.cycleSchedule.resultconfirmationduedate
            new TranslationSeedItem("entity.cycleSchedule.resultconfirmationduedate", "en-US", "结果确认截止日期", "结果确认截止日期"),
            // entity.cycleSchedule.resultconfirmationduedate
            new TranslationSeedItem("entity.cycleSchedule.resultconfirmationduedate", "ja-JP", "结果确认截止日期", "结果确认截止日期"),
            // entity.cycleSchedule.resultconfirmationduedate
            new TranslationSeedItem("entity.cycleSchedule.resultconfirmationduedate", "zh-CN", "结果确认截止日期", "结果确认截止日期"),
            // entity.cycleSchedule.resultconfirmationduedate
            new TranslationSeedItem("entity.cycleSchedule.resultconfirmationduedate", "zh-HK", "结果确认截止日期", "结果确认截止日期"),

            // entity.cycleSchedule.applicabledepartment
            new TranslationSeedItem("entity.cycleSchedule.applicabledepartment", "en-US", "适用部门", "适用部门"),
            // entity.cycleSchedule.applicabledepartment
            new TranslationSeedItem("entity.cycleSchedule.applicabledepartment", "ja-JP", "适用部门", "适用部门"),
            // entity.cycleSchedule.applicabledepartment
            new TranslationSeedItem("entity.cycleSchedule.applicabledepartment", "zh-CN", "适用部门", "适用部门"),
            // entity.cycleSchedule.applicabledepartment
            new TranslationSeedItem("entity.cycleSchedule.applicabledepartment", "zh-HK", "适用部门", "适用部门"),

            // entity.cycleSchedule.description
            new TranslationSeedItem("entity.cycleSchedule.description", "en-US", "周期说明", "周期说明"),
            // entity.cycleSchedule.description
            new TranslationSeedItem("entity.cycleSchedule.description", "ja-JP", "周期说明", "周期说明"),
            // entity.cycleSchedule.description
            new TranslationSeedItem("entity.cycleSchedule.description", "zh-CN", "周期说明", "周期说明"),
            // entity.cycleSchedule.description
            new TranslationSeedItem("entity.cycleSchedule.description", "zh-HK", "周期说明", "周期说明"),

            // entity.cycleSchedule.status
            new TranslationSeedItem("entity.cycleSchedule.status", "en-US", "状态", "状态（0=待启动 1=目标设定中 2=进行中 3=评审中 4=已完成 5=已归档）"),
            // entity.cycleSchedule.status
            new TranslationSeedItem("entity.cycleSchedule.status", "ja-JP", "状态", "状态（0=待启动 1=目标设定中 2=进行中 3=评审中 4=已完成 5=已归档）"),
            // entity.cycleSchedule.status
            new TranslationSeedItem("entity.cycleSchedule.status", "zh-CN", "状态", "状态（0=待启动 1=目标设定中 2=进行中 3=评审中 4=已完成 5=已归档）"),
            // entity.cycleSchedule.status
            new TranslationSeedItem("entity.cycleSchedule.status", "zh-HK", "状态", "状态（0=待启动 1=目标设定中 2=进行中 3=评审中 4=已完成 5=已归档）"),

            // entity.cycleSchedule.relatedplant
            new TranslationSeedItem("entity.cycleSchedule.relatedplant", "en-US", "关联工厂", "关联工厂"),
            // entity.cycleSchedule.relatedplant
            new TranslationSeedItem("entity.cycleSchedule.relatedplant", "ja-JP", "关联工厂", "关联工厂"),
            // entity.cycleSchedule.relatedplant
            new TranslationSeedItem("entity.cycleSchedule.relatedplant", "zh-CN", "关联工厂", "关联工厂"),
            // entity.cycleSchedule.relatedplant
            new TranslationSeedItem("entity.cycleSchedule.relatedplant", "zh-HK", "关联工厂", "关联工厂"),
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
