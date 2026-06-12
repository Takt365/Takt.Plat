// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Compensation
// 文件名称：TaktEmpSalaryI18nSeedData.cs
// 创建时间：2026-06-12
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktEmpSalary 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Compensation;

/// <summary>
/// TaktEmpSalary 实体国际化翻译种子（键前缀 entity.empsalary.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktEmpSalaryI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktEmpSalary 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 empsalary 实体翻译...", tenantCode);

        foreach (var item in GetEmpSalaryTranslations())
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

        TaktLogger.Information("TaktEmpSalary 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktEmpSalary 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.empsalary._self / entity.empsalary.{{field}}；ResourceGroup=5；ResourceType=0
    /// </summary>
    private static List<TranslationSeedItem> GetEmpSalaryTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.empsalary._self
            new TranslationSeedItem("entity.empsalary._self", "en-US", "Emp Salary Information", "实体名称"),
            // entity.empsalary._self
            new TranslationSeedItem("entity.empsalary._self", "ja-JP", "员工薪酬档案信息", "实体名称"),
            // entity.empsalary._self
            new TranslationSeedItem("entity.empsalary._self", "zh-CN", "员工薪酬档案信息", "实体名称"),
            // entity.empsalary._self
            new TranslationSeedItem("entity.empsalary._self", "zh-HK", "员工薪酬档案信息", "实体名称"),

            // entity.empsalary.employeeid
            new TranslationSeedItem("entity.empsalary.employeeid", "en-US", "员工ID", "员工 ID"),
            // entity.empsalary.employeeid
            new TranslationSeedItem("entity.empsalary.employeeid", "ja-JP", "员工ID", "员工 ID"),
            // entity.empsalary.employeeid
            new TranslationSeedItem("entity.empsalary.employeeid", "zh-CN", "员工ID", "员工 ID"),
            // entity.empsalary.employeeid
            new TranslationSeedItem("entity.empsalary.employeeid", "zh-HK", "员工ID", "员工 ID"),

            // entity.empsalary.employeename
            new TranslationSeedItem("entity.empsalary.employeename", "en-US", "员工姓名", "员工姓名"),
            // entity.empsalary.employeename
            new TranslationSeedItem("entity.empsalary.employeename", "ja-JP", "员工姓名", "员工姓名"),
            // entity.empsalary.employeename
            new TranslationSeedItem("entity.empsalary.employeename", "zh-CN", "员工姓名", "员工姓名"),
            // entity.empsalary.employeename
            new TranslationSeedItem("entity.empsalary.employeename", "zh-HK", "员工姓名", "员工姓名"),

            // entity.empsalary.payrollid
            new TranslationSeedItem("entity.empsalary.payrollid", "en-US", "薪酬体系ID", "关联薪酬体系 ID"),
            // entity.empsalary.payrollid
            new TranslationSeedItem("entity.empsalary.payrollid", "ja-JP", "薪酬体系ID", "关联薪酬体系 ID"),
            // entity.empsalary.payrollid
            new TranslationSeedItem("entity.empsalary.payrollid", "zh-CN", "薪酬体系ID", "关联薪酬体系 ID"),
            // entity.empsalary.payrollid
            new TranslationSeedItem("entity.empsalary.payrollid", "zh-HK", "薪酬体系ID", "关联薪酬体系 ID"),

            // entity.empsalary.payscaleid
            new TranslationSeedItem("entity.empsalary.payscaleid", "en-US", "薪级ID", "关联薪级 ID"),
            // entity.empsalary.payscaleid
            new TranslationSeedItem("entity.empsalary.payscaleid", "ja-JP", "薪级ID", "关联薪级 ID"),
            // entity.empsalary.payscaleid
            new TranslationSeedItem("entity.empsalary.payscaleid", "zh-CN", "薪级ID", "关联薪级 ID"),
            // entity.empsalary.payscaleid
            new TranslationSeedItem("entity.empsalary.payscaleid", "zh-HK", "薪级ID", "关联薪级 ID"),

            // entity.empsalary.basesalary
            new TranslationSeedItem("entity.empsalary.basesalary", "en-US", "基本工资", "基本工资（元）"),
            // entity.empsalary.basesalary
            new TranslationSeedItem("entity.empsalary.basesalary", "ja-JP", "基本工资", "基本工资（元）"),
            // entity.empsalary.basesalary
            new TranslationSeedItem("entity.empsalary.basesalary", "zh-CN", "基本工资", "基本工资（元）"),
            // entity.empsalary.basesalary
            new TranslationSeedItem("entity.empsalary.basesalary", "zh-HK", "基本工资", "基本工资（元）"),

            // entity.empsalary.positionsalary
            new TranslationSeedItem("entity.empsalary.positionsalary", "en-US", "岗位工资", "岗位工资（元）"),
            // entity.empsalary.positionsalary
            new TranslationSeedItem("entity.empsalary.positionsalary", "ja-JP", "岗位工资", "岗位工资（元）"),
            // entity.empsalary.positionsalary
            new TranslationSeedItem("entity.empsalary.positionsalary", "zh-CN", "岗位工资", "岗位工资（元）"),
            // entity.empsalary.positionsalary
            new TranslationSeedItem("entity.empsalary.positionsalary", "zh-HK", "岗位工资", "岗位工资（元）"),

            // entity.empsalary.allowancetotal
            new TranslationSeedItem("entity.empsalary.allowancetotal", "en-US", "津贴合计", "津贴合计（元）"),
            // entity.empsalary.allowancetotal
            new TranslationSeedItem("entity.empsalary.allowancetotal", "ja-JP", "津贴合计", "津贴合计（元）"),
            // entity.empsalary.allowancetotal
            new TranslationSeedItem("entity.empsalary.allowancetotal", "zh-CN", "津贴合计", "津贴合计（元）"),
            // entity.empsalary.allowancetotal
            new TranslationSeedItem("entity.empsalary.allowancetotal", "zh-HK", "津贴合计", "津贴合计（元）"),

            // entity.empsalary.salaryitemid
            new TranslationSeedItem("entity.empsalary.salaryitemid", "en-US", "薪资项目ID", "关联薪资项目 ID（如股权激励项，对应 TaktSalaryItem 中 item_type 为股权激励的记录）"),
            // entity.empsalary.salaryitemid
            new TranslationSeedItem("entity.empsalary.salaryitemid", "ja-JP", "薪资项目ID", "关联薪资项目 ID（如股权激励项，对应 TaktSalaryItem 中 item_type 为股权激励的记录）"),
            // entity.empsalary.salaryitemid
            new TranslationSeedItem("entity.empsalary.salaryitemid", "zh-CN", "薪资项目ID", "关联薪资项目 ID（如股权激励项，对应 TaktSalaryItem 中 item_type 为股权激励的记录）"),
            // entity.empsalary.salaryitemid
            new TranslationSeedItem("entity.empsalary.salaryitemid", "zh-HK", "薪资项目ID", "关联薪资项目 ID（如股权激励项，对应 TaktSalaryItem 中 item_type 为股权激励的记录）"),

            // entity.empsalary.sharecount
            new TranslationSeedItem("entity.empsalary.sharecount", "en-US", "授予股数", "授予股数/份数（股权激励定薪时使用）"),
            // entity.empsalary.sharecount
            new TranslationSeedItem("entity.empsalary.sharecount", "ja-JP", "授予股数", "授予股数/份数（股权激励定薪时使用）"),
            // entity.empsalary.sharecount
            new TranslationSeedItem("entity.empsalary.sharecount", "zh-CN", "授予股数", "授予股数/份数（股权激励定薪时使用）"),
            // entity.empsalary.sharecount
            new TranslationSeedItem("entity.empsalary.sharecount", "zh-HK", "授予股数", "授予股数/份数（股权激励定薪时使用）"),

            // entity.empsalary.effectivedate
            new TranslationSeedItem("entity.empsalary.effectivedate", "en-US", "生效日期", "生效日期"),
            // entity.empsalary.effectivedate
            new TranslationSeedItem("entity.empsalary.effectivedate", "ja-JP", "生效日期", "生效日期"),
            // entity.empsalary.effectivedate
            new TranslationSeedItem("entity.empsalary.effectivedate", "zh-CN", "生效日期", "生效日期"),
            // entity.empsalary.effectivedate
            new TranslationSeedItem("entity.empsalary.effectivedate", "zh-HK", "生效日期", "生效日期"),

            // entity.empsalary.status
            new TranslationSeedItem("entity.empsalary.status", "en-US", "状态", "状态（字典 sys_normal_disable）"),
            // entity.empsalary.status
            new TranslationSeedItem("entity.empsalary.status", "ja-JP", "状态", "状态（字典 sys_normal_disable）"),
            // entity.empsalary.status
            new TranslationSeedItem("entity.empsalary.status", "zh-CN", "状态", "状态（字典 sys_normal_disable）"),
            // entity.empsalary.status
            new TranslationSeedItem("entity.empsalary.status", "zh-HK", "状态", "状态（字典 sys_normal_disable）"),

            // entity.empsalary.relatedplant
            new TranslationSeedItem("entity.empsalary.relatedplant", "en-US", "关联工厂", "关联工厂"),
            // entity.empsalary.relatedplant
            new TranslationSeedItem("entity.empsalary.relatedplant", "ja-JP", "关联工厂", "关联工厂"),
            // entity.empsalary.relatedplant
            new TranslationSeedItem("entity.empsalary.relatedplant", "zh-CN", "关联工厂", "关联工厂"),
            // entity.empsalary.relatedplant
            new TranslationSeedItem("entity.empsalary.relatedplant", "zh-HK", "关联工厂", "关联工厂"),
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
