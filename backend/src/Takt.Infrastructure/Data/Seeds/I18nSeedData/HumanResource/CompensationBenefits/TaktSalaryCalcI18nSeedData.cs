// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.CompensationBenefits
// 文件名称：TaktSalaryCalcI18nSeedData.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSalaryCalc 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.CompensationBenefits;

/// <summary>
/// TaktSalaryCalc 实体国际化翻译种子（键前缀 entity.salaryCalc.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSalaryCalcI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSalaryCalc 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 salaryCalc 实体翻译...", tenantCode);

        foreach (var item in GetSalaryCalcTranslations())
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

        TaktLogger.Information("TaktSalaryCalc 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSalaryCalc 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.salaryCalc._self / entity.salaryCalc.{{field}}；ResourceGroup=TaktModule.HumanResource；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSalaryCalcTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.salaryCalc._self
            new TranslationSeedItem("entity.salaryCalc._self", "en-US", "Salary Calc Information", "实体名称"),
            // entity.salaryCalc._self
            new TranslationSeedItem("entity.salaryCalc._self", "ja-JP", "薪资核算批次信息", "实体名称"),
            // entity.salaryCalc._self
            new TranslationSeedItem("entity.salaryCalc._self", "zh-CN", "薪资核算批次信息", "实体名称"),
            // entity.salaryCalc._self
            new TranslationSeedItem("entity.salaryCalc._self", "zh-HK", "薪资核算批次信息", "实体名称"),

            // entity.salaryCalc.calccode
            new TranslationSeedItem("entity.salaryCalc.calccode", "en-US", "核算批次编码", "核算批次编码（租户+公司内唯一）"),
            // entity.salaryCalc.calccode
            new TranslationSeedItem("entity.salaryCalc.calccode", "ja-JP", "核算批次编码", "核算批次编码（租户+公司内唯一）"),
            // entity.salaryCalc.calccode
            new TranslationSeedItem("entity.salaryCalc.calccode", "zh-CN", "核算批次编码", "核算批次编码（租户+公司内唯一）"),
            // entity.salaryCalc.calccode
            new TranslationSeedItem("entity.salaryCalc.calccode", "zh-HK", "核算批次编码", "核算批次编码（租户+公司内唯一）"),

            // entity.salaryCalc.calcname
            new TranslationSeedItem("entity.salaryCalc.calcname", "en-US", "核算批次名称", "核算批次名称"),
            // entity.salaryCalc.calcname
            new TranslationSeedItem("entity.salaryCalc.calcname", "ja-JP", "核算批次名称", "核算批次名称"),
            // entity.salaryCalc.calcname
            new TranslationSeedItem("entity.salaryCalc.calcname", "zh-CN", "核算批次名称", "核算批次名称"),
            // entity.salaryCalc.calcname
            new TranslationSeedItem("entity.salaryCalc.calcname", "zh-HK", "核算批次名称", "核算批次名称"),

            // entity.salaryCalc.payperiod
            new TranslationSeedItem("entity.salaryCalc.payperiod", "en-US", "发薪期间", "发薪期间（如 2026-06）"),
            // entity.salaryCalc.payperiod
            new TranslationSeedItem("entity.salaryCalc.payperiod", "ja-JP", "发薪期间", "发薪期间（如 2026-06）"),
            // entity.salaryCalc.payperiod
            new TranslationSeedItem("entity.salaryCalc.payperiod", "zh-CN", "发薪期间", "发薪期间（如 2026-06）"),
            // entity.salaryCalc.payperiod
            new TranslationSeedItem("entity.salaryCalc.payperiod", "zh-HK", "发薪期间", "发薪期间（如 2026-06）"),

            // entity.salaryCalc.calcdate
            new TranslationSeedItem("entity.salaryCalc.calcdate", "en-US", "核算日期", "核算日期"),
            // entity.salaryCalc.calcdate
            new TranslationSeedItem("entity.salaryCalc.calcdate", "ja-JP", "核算日期", "核算日期"),
            // entity.salaryCalc.calcdate
            new TranslationSeedItem("entity.salaryCalc.calcdate", "zh-CN", "核算日期", "核算日期"),
            // entity.salaryCalc.calcdate
            new TranslationSeedItem("entity.salaryCalc.calcdate", "zh-HK", "核算日期", "核算日期"),

            // entity.salaryCalc.employeecount
            new TranslationSeedItem("entity.salaryCalc.employeecount", "en-US", "参与核算人数", "参与核算人数"),
            // entity.salaryCalc.employeecount
            new TranslationSeedItem("entity.salaryCalc.employeecount", "ja-JP", "参与核算人数", "参与核算人数"),
            // entity.salaryCalc.employeecount
            new TranslationSeedItem("entity.salaryCalc.employeecount", "zh-CN", "参与核算人数", "参与核算人数"),
            // entity.salaryCalc.employeecount
            new TranslationSeedItem("entity.salaryCalc.employeecount", "zh-HK", "参与核算人数", "参与核算人数"),

            // entity.salaryCalc.grossamount
            new TranslationSeedItem("entity.salaryCalc.grossamount", "en-US", "应发合计", "应发合计（元）"),
            // entity.salaryCalc.grossamount
            new TranslationSeedItem("entity.salaryCalc.grossamount", "ja-JP", "应发合计", "应发合计（元）"),
            // entity.salaryCalc.grossamount
            new TranslationSeedItem("entity.salaryCalc.grossamount", "zh-CN", "应发合计", "应发合计（元）"),
            // entity.salaryCalc.grossamount
            new TranslationSeedItem("entity.salaryCalc.grossamount", "zh-HK", "应发合计", "应发合计（元）"),

            // entity.salaryCalc.netamount
            new TranslationSeedItem("entity.salaryCalc.netamount", "en-US", "实发合计", "实发合计（元）"),
            // entity.salaryCalc.netamount
            new TranslationSeedItem("entity.salaryCalc.netamount", "ja-JP", "实发合计", "实发合计（元）"),
            // entity.salaryCalc.netamount
            new TranslationSeedItem("entity.salaryCalc.netamount", "zh-CN", "实发合计", "实发合计（元）"),
            // entity.salaryCalc.netamount
            new TranslationSeedItem("entity.salaryCalc.netamount", "zh-HK", "实发合计", "实发合计（元）"),

            // entity.salaryCalc.calcstatus
            new TranslationSeedItem("entity.salaryCalc.calcstatus", "en-US", "核算状态", "核算状态（0=草稿 1=核算中 2=已完成 3=已归档）"),
            // entity.salaryCalc.calcstatus
            new TranslationSeedItem("entity.salaryCalc.calcstatus", "ja-JP", "核算状态", "核算状态（0=草稿 1=核算中 2=已完成 3=已归档）"),
            // entity.salaryCalc.calcstatus
            new TranslationSeedItem("entity.salaryCalc.calcstatus", "zh-CN", "核算状态", "核算状态（0=草稿 1=核算中 2=已完成 3=已归档）"),
            // entity.salaryCalc.calcstatus
            new TranslationSeedItem("entity.salaryCalc.calcstatus", "zh-HK", "核算状态", "核算状态（0=草稿 1=核算中 2=已完成 3=已归档）"),

            // entity.salaryCalc.relatedplant
            new TranslationSeedItem("entity.salaryCalc.relatedplant", "en-US", "关联工厂", "关联工厂"),
            // entity.salaryCalc.relatedplant
            new TranslationSeedItem("entity.salaryCalc.relatedplant", "ja-JP", "关联工厂", "关联工厂"),
            // entity.salaryCalc.relatedplant
            new TranslationSeedItem("entity.salaryCalc.relatedplant", "zh-CN", "关联工厂", "关联工厂"),
            // entity.salaryCalc.relatedplant
            new TranslationSeedItem("entity.salaryCalc.relatedplant", "zh-HK", "关联工厂", "关联工厂"),
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
