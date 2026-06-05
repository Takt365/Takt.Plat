// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Personnel
// 文件名称：TaktEmployeeDelegationI18nSeedData.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktEmployeeDelegation 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktEmployeeDelegation 实体国际化翻译种子（键前缀 entity.employeeDelegation.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktEmployeeDelegationI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktEmployeeDelegation 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 employeeDelegation 实体翻译...", tenantCode);

        foreach (var item in GetEmployeeDelegationTranslations())
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

        TaktLogger.Information("TaktEmployeeDelegation 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktEmployeeDelegation 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.employeeDelegation._self / entity.employeeDelegation.{{field}}；ResourceGroup=TaktModule.HumanResource；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetEmployeeDelegationTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.employeeDelegation._self
            new TranslationSeedItem("entity.employeeDelegation._self", "en-US", "Employee Delegation Information", "实体名称"),
            // entity.employeeDelegation._self
            new TranslationSeedItem("entity.employeeDelegation._self", "ja-JP", "员工代理关系信息", "实体名称"),
            // entity.employeeDelegation._self
            new TranslationSeedItem("entity.employeeDelegation._self", "zh-CN", "员工代理关系信息", "实体名称"),
            // entity.employeeDelegation._self
            new TranslationSeedItem("entity.employeeDelegation._self", "zh-HK", "员工代理关系信息", "实体名称"),

            // entity.employeeDelegation.proxyemployeeid
            new TranslationSeedItem("entity.employeeDelegation.proxyemployeeid", "en-US", "代理人ID", "代理人ID（代替别人处理工作的人）"),
            // entity.employeeDelegation.proxyemployeeid
            new TranslationSeedItem("entity.employeeDelegation.proxyemployeeid", "ja-JP", "代理人ID", "代理人ID（代替别人处理工作的人）"),
            // entity.employeeDelegation.proxyemployeeid
            new TranslationSeedItem("entity.employeeDelegation.proxyemployeeid", "zh-CN", "代理人ID", "代理人ID（代替别人处理工作的人）"),
            // entity.employeeDelegation.proxyemployeeid
            new TranslationSeedItem("entity.employeeDelegation.proxyemployeeid", "zh-HK", "代理人ID", "代理人ID（代替别人处理工作的人）"),

            // entity.employeeDelegation.originalemployeeid
            new TranslationSeedItem("entity.employeeDelegation.originalemployeeid", "en-US", "被代理人ID", "被代理人ID（需要别人代替的人）"),
            // entity.employeeDelegation.originalemployeeid
            new TranslationSeedItem("entity.employeeDelegation.originalemployeeid", "ja-JP", "被代理人ID", "被代理人ID（需要别人代替的人）"),
            // entity.employeeDelegation.originalemployeeid
            new TranslationSeedItem("entity.employeeDelegation.originalemployeeid", "zh-CN", "被代理人ID", "被代理人ID（需要别人代替的人）"),
            // entity.employeeDelegation.originalemployeeid
            new TranslationSeedItem("entity.employeeDelegation.originalemployeeid", "zh-HK", "被代理人ID", "被代理人ID（需要别人代替的人）"),

            // entity.employeeDelegation.delegationtype
            new TranslationSeedItem("entity.employeeDelegation.delegationtype", "en-US", "代理类型", "代理类型 1 = 完全代理（代理人拥有被代理人的所有权限） 2 = 部分代理（仅代理特定部门/岗位的权限） 3 = 审批代理（仅代理审批流程）"),
            // entity.employeeDelegation.delegationtype
            new TranslationSeedItem("entity.employeeDelegation.delegationtype", "ja-JP", "代理类型", "代理类型 1 = 完全代理（代理人拥有被代理人的所有权限） 2 = 部分代理（仅代理特定部门/岗位的权限） 3 = 审批代理（仅代理审批流程）"),
            // entity.employeeDelegation.delegationtype
            new TranslationSeedItem("entity.employeeDelegation.delegationtype", "zh-CN", "代理类型", "代理类型 1 = 完全代理（代理人拥有被代理人的所有权限） 2 = 部分代理（仅代理特定部门/岗位的权限） 3 = 审批代理（仅代理审批流程）"),
            // entity.employeeDelegation.delegationtype
            new TranslationSeedItem("entity.employeeDelegation.delegationtype", "zh-HK", "代理类型", "代理类型 1 = 完全代理（代理人拥有被代理人的所有权限） 2 = 部分代理（仅代理特定部门/岗位的权限） 3 = 审批代理（仅代理审批流程）"),

            // entity.employeeDelegation.scopetype
            new TranslationSeedItem("entity.employeeDelegation.scopetype", "en-US", "代理范围类型", "代理范围类型 1 = 部门级别（代理被代理人在特定部门的所有权限） 2 = 岗位级别（代理被代理人在特定岗位的所有权限） 3 = 全局代理（代理被代理人的所有权限） 4 = 特定业务（仅代理特定业务流程）"),
            // entity.employeeDelegation.scopetype
            new TranslationSeedItem("entity.employeeDelegation.scopetype", "ja-JP", "代理范围类型", "代理范围类型 1 = 部门级别（代理被代理人在特定部门的所有权限） 2 = 岗位级别（代理被代理人在特定岗位的所有权限） 3 = 全局代理（代理被代理人的所有权限） 4 = 特定业务（仅代理特定业务流程）"),
            // entity.employeeDelegation.scopetype
            new TranslationSeedItem("entity.employeeDelegation.scopetype", "zh-CN", "代理范围类型", "代理范围类型 1 = 部门级别（代理被代理人在特定部门的所有权限） 2 = 岗位级别（代理被代理人在特定岗位的所有权限） 3 = 全局代理（代理被代理人的所有权限） 4 = 特定业务（仅代理特定业务流程）"),
            // entity.employeeDelegation.scopetype
            new TranslationSeedItem("entity.employeeDelegation.scopetype", "zh-HK", "代理范围类型", "代理范围类型 1 = 部门级别（代理被代理人在特定部门的所有权限） 2 = 岗位级别（代理被代理人在特定岗位的所有权限） 3 = 全局代理（代理被代理人的所有权限） 4 = 特定业务（仅代理特定业务流程）"),

            // entity.employeeDelegation.scopeid
            new TranslationSeedItem("entity.employeeDelegation.scopeid", "en-US", "代理范围ID", "代理范围ID 当 ScopeType=1 时，表示部门ID 当 ScopeType=2 时，表示岗位ID 当 ScopeType=4 时，表示业务ID（如：工作流定义ID）"),
            // entity.employeeDelegation.scopeid
            new TranslationSeedItem("entity.employeeDelegation.scopeid", "ja-JP", "代理范围ID", "代理范围ID 当 ScopeType=1 时，表示部门ID 当 ScopeType=2 时，表示岗位ID 当 ScopeType=4 时，表示业务ID（如：工作流定义ID）"),
            // entity.employeeDelegation.scopeid
            new TranslationSeedItem("entity.employeeDelegation.scopeid", "zh-CN", "代理范围ID", "代理范围ID 当 ScopeType=1 时，表示部门ID 当 ScopeType=2 时，表示岗位ID 当 ScopeType=4 时，表示业务ID（如：工作流定义ID）"),
            // entity.employeeDelegation.scopeid
            new TranslationSeedItem("entity.employeeDelegation.scopeid", "zh-HK", "代理范围ID", "代理范围ID 当 ScopeType=1 时，表示部门ID 当 ScopeType=2 时，表示岗位ID 当 ScopeType=4 时，表示业务ID（如：工作流定义ID）"),

            // entity.employeeDelegation.reason
            new TranslationSeedItem("entity.employeeDelegation.reason", "en-US", "代理原因", "代理原因 如：休假、出差、培训、岗位空缺、病假等"),
            // entity.employeeDelegation.reason
            new TranslationSeedItem("entity.employeeDelegation.reason", "ja-JP", "代理原因", "代理原因 如：休假、出差、培训、岗位空缺、病假等"),
            // entity.employeeDelegation.reason
            new TranslationSeedItem("entity.employeeDelegation.reason", "zh-CN", "代理原因", "代理原因 如：休假、出差、培训、岗位空缺、病假等"),
            // entity.employeeDelegation.reason
            new TranslationSeedItem("entity.employeeDelegation.reason", "zh-HK", "代理原因", "代理原因 如：休假、出差、培训、岗位空缺、病假等"),

            // entity.employeeDelegation.startdate
            new TranslationSeedItem("entity.employeeDelegation.startdate", "en-US", "代理开始时间", "代理开始时间"),
            // entity.employeeDelegation.startdate
            new TranslationSeedItem("entity.employeeDelegation.startdate", "ja-JP", "代理开始时间", "代理开始时间"),
            // entity.employeeDelegation.startdate
            new TranslationSeedItem("entity.employeeDelegation.startdate", "zh-CN", "代理开始时间", "代理开始时间"),
            // entity.employeeDelegation.startdate
            new TranslationSeedItem("entity.employeeDelegation.startdate", "zh-HK", "代理开始时间", "代理开始时间"),

            // entity.employeeDelegation.enddate
            new TranslationSeedItem("entity.employeeDelegation.enddate", "en-US", "代理结束时间", "代理结束时间 null = 长期有效，直到手动删除"),
            // entity.employeeDelegation.enddate
            new TranslationSeedItem("entity.employeeDelegation.enddate", "ja-JP", "代理结束时间", "代理结束时间 null = 长期有效，直到手动删除"),
            // entity.employeeDelegation.enddate
            new TranslationSeedItem("entity.employeeDelegation.enddate", "zh-CN", "代理结束时间", "代理结束时间 null = 长期有效，直到手动删除"),
            // entity.employeeDelegation.enddate
            new TranslationSeedItem("entity.employeeDelegation.enddate", "zh-HK", "代理结束时间", "代理结束时间 null = 长期有效，直到手动删除"),
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
