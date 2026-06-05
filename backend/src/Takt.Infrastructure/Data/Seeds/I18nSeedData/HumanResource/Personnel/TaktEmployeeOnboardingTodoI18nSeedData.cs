// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Personnel
// 文件名称：TaktEmployeeOnboardingTodoI18nSeedData.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktEmployeeOnboardingTodo 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Personnel;

/// <summary>
/// TaktEmployeeOnboardingTodo 实体国际化翻译种子（键前缀 entity.employeeOnboardingTodo.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktEmployeeOnboardingTodoI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktEmployeeOnboardingTodo 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 employeeOnboardingTodo 实体翻译...", tenantCode);

        foreach (var item in GetEmployeeOnboardingTodoTranslations())
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

        TaktLogger.Information("TaktEmployeeOnboardingTodo 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktEmployeeOnboardingTodo 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.employeeOnboardingTodo._self / entity.employeeOnboardingTodo.{{field}}；ResourceGroup=TaktModule.HumanResource；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetEmployeeOnboardingTodoTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.employeeOnboardingTodo._self
            new TranslationSeedItem("entity.employeeOnboardingTodo._self", "en-US", "Employee Onboarding Todo Information", "实体名称"),
            // entity.employeeOnboardingTodo._self
            new TranslationSeedItem("entity.employeeOnboardingTodo._self", "ja-JP", "入职待办信息", "实体名称"),
            // entity.employeeOnboardingTodo._self
            new TranslationSeedItem("entity.employeeOnboardingTodo._self", "zh-CN", "入职待办信息", "实体名称"),
            // entity.employeeOnboardingTodo._self
            new TranslationSeedItem("entity.employeeOnboardingTodo._self", "zh-HK", "入职待办信息", "实体名称"),

            // entity.employeeOnboardingTodo.offerid
            new TranslationSeedItem("entity.employeeOnboardingTodo.offerid", "en-US", "录用信息ID", "录用信息ID（人才管理 TaktTalentOffer）"),
            // entity.employeeOnboardingTodo.offerid
            new TranslationSeedItem("entity.employeeOnboardingTodo.offerid", "ja-JP", "录用信息ID", "录用信息ID（人才管理 TaktTalentOffer）"),
            // entity.employeeOnboardingTodo.offerid
            new TranslationSeedItem("entity.employeeOnboardingTodo.offerid", "zh-CN", "录用信息ID", "录用信息ID（人才管理 TaktTalentOffer）"),
            // entity.employeeOnboardingTodo.offerid
            new TranslationSeedItem("entity.employeeOnboardingTodo.offerid", "zh-HK", "录用信息ID", "录用信息ID（人才管理 TaktTalentOffer）"),

            // entity.employeeOnboardingTodo.todono
            new TranslationSeedItem("entity.employeeOnboardingTodo.todono", "en-US", "待办单号", "待办单号（租户+公司内业务编号）"),
            // entity.employeeOnboardingTodo.todono
            new TranslationSeedItem("entity.employeeOnboardingTodo.todono", "ja-JP", "待办单号", "待办单号（租户+公司内业务编号）"),
            // entity.employeeOnboardingTodo.todono
            new TranslationSeedItem("entity.employeeOnboardingTodo.todono", "zh-CN", "待办单号", "待办单号（租户+公司内业务编号）"),
            // entity.employeeOnboardingTodo.todono
            new TranslationSeedItem("entity.employeeOnboardingTodo.todono", "zh-HK", "待办单号", "待办单号（租户+公司内业务编号）"),

            // entity.employeeOnboardingTodo.todostatus
            new TranslationSeedItem("entity.employeeOnboardingTodo.todostatus", "en-US", "待办状态", "待办状态（0=待办理，1=办理中，2=已完成，3=已取消）"),
            // entity.employeeOnboardingTodo.todostatus
            new TranslationSeedItem("entity.employeeOnboardingTodo.todostatus", "ja-JP", "待办状态", "待办状态（0=待办理，1=办理中，2=已完成，3=已取消）"),
            // entity.employeeOnboardingTodo.todostatus
            new TranslationSeedItem("entity.employeeOnboardingTodo.todostatus", "zh-CN", "待办状态", "待办状态（0=待办理，1=办理中，2=已完成，3=已取消）"),
            // entity.employeeOnboardingTodo.todostatus
            new TranslationSeedItem("entity.employeeOnboardingTodo.todostatus", "zh-HK", "待办状态", "待办状态（0=待办理，1=办理中，2=已完成，3=已取消）"),

            // entity.employeeOnboardingTodo.plannedjoineddate
            new TranslationSeedItem("entity.employeeOnboardingTodo.plannedjoineddate", "en-US", "计划上岗日期", "计划上岗日期（JoinedDate 计划值）"),
            // entity.employeeOnboardingTodo.plannedjoineddate
            new TranslationSeedItem("entity.employeeOnboardingTodo.plannedjoineddate", "ja-JP", "计划上岗日期", "计划上岗日期（JoinedDate 计划值）"),
            // entity.employeeOnboardingTodo.plannedjoineddate
            new TranslationSeedItem("entity.employeeOnboardingTodo.plannedjoineddate", "zh-CN", "计划上岗日期", "计划上岗日期（JoinedDate 计划值）"),
            // entity.employeeOnboardingTodo.plannedjoineddate
            new TranslationSeedItem("entity.employeeOnboardingTodo.plannedjoineddate", "zh-HK", "计划上岗日期", "计划上岗日期（JoinedDate 计划值）"),

            // entity.employeeOnboardingTodo.candidatename
            new TranslationSeedItem("entity.employeeOnboardingTodo.candidatename", "en-US", "候选人姓名", "候选人姓名（快照）"),
            // entity.employeeOnboardingTodo.candidatename
            new TranslationSeedItem("entity.employeeOnboardingTodo.candidatename", "ja-JP", "候选人姓名", "候选人姓名（快照）"),
            // entity.employeeOnboardingTodo.candidatename
            new TranslationSeedItem("entity.employeeOnboardingTodo.candidatename", "zh-CN", "候选人姓名", "候选人姓名（快照）"),
            // entity.employeeOnboardingTodo.candidatename
            new TranslationSeedItem("entity.employeeOnboardingTodo.candidatename", "zh-HK", "候选人姓名", "候选人姓名（快照）"),

            // entity.employeeOnboardingTodo.mobile
            new TranslationSeedItem("entity.employeeOnboardingTodo.mobile", "en-US", "候选人手机", "候选人手机（快照）"),
            // entity.employeeOnboardingTodo.mobile
            new TranslationSeedItem("entity.employeeOnboardingTodo.mobile", "ja-JP", "候选人手机", "候选人手机（快照）"),
            // entity.employeeOnboardingTodo.mobile
            new TranslationSeedItem("entity.employeeOnboardingTodo.mobile", "zh-CN", "候选人手机", "候选人手机（快照）"),
            // entity.employeeOnboardingTodo.mobile
            new TranslationSeedItem("entity.employeeOnboardingTodo.mobile", "zh-HK", "候选人手机", "候选人手机（快照）"),

            // entity.employeeOnboardingTodo.employeeid
            new TranslationSeedItem("entity.employeeOnboardingTodo.employeeid", "en-US", "关联员工ID", "关联员工ID（建档后回填，可空）"),
            // entity.employeeOnboardingTodo.employeeid
            new TranslationSeedItem("entity.employeeOnboardingTodo.employeeid", "ja-JP", "关联员工ID", "关联员工ID（建档后回填，可空）"),
            // entity.employeeOnboardingTodo.employeeid
            new TranslationSeedItem("entity.employeeOnboardingTodo.employeeid", "zh-CN", "关联员工ID", "关联员工ID（建档后回填，可空）"),
            // entity.employeeOnboardingTodo.employeeid
            new TranslationSeedItem("entity.employeeOnboardingTodo.employeeid", "zh-HK", "关联员工ID", "关联员工ID（建档后回填，可空）"),

            // entity.employeeOnboardingTodo.employeejoinedid
            new TranslationSeedItem("entity.employeeOnboardingTodo.employeejoinedid", "en-US", "入职上岗单ID", "入职上岗单ID（待办完成后回填，可空）"),
            // entity.employeeOnboardingTodo.employeejoinedid
            new TranslationSeedItem("entity.employeeOnboardingTodo.employeejoinedid", "ja-JP", "入职上岗单ID", "入职上岗单ID（待办完成后回填，可空）"),
            // entity.employeeOnboardingTodo.employeejoinedid
            new TranslationSeedItem("entity.employeeOnboardingTodo.employeejoinedid", "zh-CN", "入职上岗单ID", "入职上岗单ID（待办完成后回填，可空）"),
            // entity.employeeOnboardingTodo.employeejoinedid
            new TranslationSeedItem("entity.employeeOnboardingTodo.employeejoinedid", "zh-HK", "入职上岗单ID", "入职上岗单ID（待办完成后回填，可空）"),

            // entity.employeeOnboardingTodo.reason
            new TranslationSeedItem("entity.employeeOnboardingTodo.reason", "en-US", "待办说明", "待办说明"),
            // entity.employeeOnboardingTodo.reason
            new TranslationSeedItem("entity.employeeOnboardingTodo.reason", "ja-JP", "待办说明", "待办说明"),
            // entity.employeeOnboardingTodo.reason
            new TranslationSeedItem("entity.employeeOnboardingTodo.reason", "zh-CN", "待办说明", "待办说明"),
            // entity.employeeOnboardingTodo.reason
            new TranslationSeedItem("entity.employeeOnboardingTodo.reason", "zh-HK", "待办说明", "待办说明"),
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
