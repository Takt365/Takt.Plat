// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Personnel
// 文件名称：TaktEmployeeReassignmentI18nSeedData.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktEmployeeReassignment 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktEmployeeReassignment 实体国际化翻译种子（键前缀 entity.employeeReassignment.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktEmployeeReassignmentI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktEmployeeReassignment 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 employeeReassignment 实体翻译...", tenantCode);

        foreach (var item in GetEmployeeReassignmentTranslations())
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

        TaktLogger.Information("TaktEmployeeReassignment 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktEmployeeReassignment 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.employeeReassignment._self / entity.employeeReassignment.{{field}}；ResourceGroup=TaktModule.HumanResource；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetEmployeeReassignmentTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.employeeReassignment._self
            new TranslationSeedItem("entity.employeeReassignment._self", "en-US", "Employee Reassignment Information", "实体名称"),
            // entity.employeeReassignment._self
            new TranslationSeedItem("entity.employeeReassignment._self", "ja-JP", "员工调动记录信息", "实体名称"),
            // entity.employeeReassignment._self
            new TranslationSeedItem("entity.employeeReassignment._self", "zh-CN", "员工调动记录信息", "实体名称"),
            // entity.employeeReassignment._self
            new TranslationSeedItem("entity.employeeReassignment._self", "zh-HK", "员工调动记录信息", "实体名称"),

            // entity.employeeReassignment.employeeid
            new TranslationSeedItem("entity.employeeReassignment.employeeid", "en-US", "员工ID", "员工ID"),
            // entity.employeeReassignment.employeeid
            new TranslationSeedItem("entity.employeeReassignment.employeeid", "ja-JP", "员工ID", "员工ID"),
            // entity.employeeReassignment.employeeid
            new TranslationSeedItem("entity.employeeReassignment.employeeid", "zh-CN", "员工ID", "员工ID"),
            // entity.employeeReassignment.employeeid
            new TranslationSeedItem("entity.employeeReassignment.employeeid", "zh-HK", "员工ID", "员工ID"),

            // entity.employeeReassignment.reassignmenttype
            new TranslationSeedItem("entity.employeeReassignment.reassignmenttype", "en-US", "调动类型", "调动类型（0=转岗，1=调岗）"),
            // entity.employeeReassignment.reassignmenttype
            new TranslationSeedItem("entity.employeeReassignment.reassignmenttype", "ja-JP", "调动类型", "调动类型（0=转岗，1=调岗）"),
            // entity.employeeReassignment.reassignmenttype
            new TranslationSeedItem("entity.employeeReassignment.reassignmenttype", "zh-CN", "调动类型", "调动类型（0=转岗，1=调岗）"),
            // entity.employeeReassignment.reassignmenttype
            new TranslationSeedItem("entity.employeeReassignment.reassignmenttype", "zh-HK", "调动类型", "调动类型（0=转岗，1=调岗）"),

            // entity.employeeReassignment.fromdeptid
            new TranslationSeedItem("entity.employeeReassignment.fromdeptid", "en-US", "调出部门ID", "调出部门ID"),
            // entity.employeeReassignment.fromdeptid
            new TranslationSeedItem("entity.employeeReassignment.fromdeptid", "ja-JP", "调出部门ID", "调出部门ID"),
            // entity.employeeReassignment.fromdeptid
            new TranslationSeedItem("entity.employeeReassignment.fromdeptid", "zh-CN", "调出部门ID", "调出部门ID"),
            // entity.employeeReassignment.fromdeptid
            new TranslationSeedItem("entity.employeeReassignment.fromdeptid", "zh-HK", "调出部门ID", "调出部门ID"),

            // entity.employeeReassignment.fromdeptname
            new TranslationSeedItem("entity.employeeReassignment.fromdeptname", "en-US", "调出部门名称", "调出部门名称"),
            // entity.employeeReassignment.fromdeptname
            new TranslationSeedItem("entity.employeeReassignment.fromdeptname", "ja-JP", "调出部门名称", "调出部门名称"),
            // entity.employeeReassignment.fromdeptname
            new TranslationSeedItem("entity.employeeReassignment.fromdeptname", "zh-CN", "调出部门名称", "调出部门名称"),
            // entity.employeeReassignment.fromdeptname
            new TranslationSeedItem("entity.employeeReassignment.fromdeptname", "zh-HK", "调出部门名称", "调出部门名称"),

            // entity.employeeReassignment.frompostid
            new TranslationSeedItem("entity.employeeReassignment.frompostid", "en-US", "调出岗位ID", "调出岗位ID"),
            // entity.employeeReassignment.frompostid
            new TranslationSeedItem("entity.employeeReassignment.frompostid", "ja-JP", "调出岗位ID", "调出岗位ID"),
            // entity.employeeReassignment.frompostid
            new TranslationSeedItem("entity.employeeReassignment.frompostid", "zh-CN", "调出岗位ID", "调出岗位ID"),
            // entity.employeeReassignment.frompostid
            new TranslationSeedItem("entity.employeeReassignment.frompostid", "zh-HK", "调出岗位ID", "调出岗位ID"),

            // entity.employeeReassignment.frompostname
            new TranslationSeedItem("entity.employeeReassignment.frompostname", "en-US", "调出岗位名称", "调出岗位名称"),
            // entity.employeeReassignment.frompostname
            new TranslationSeedItem("entity.employeeReassignment.frompostname", "ja-JP", "调出岗位名称", "调出岗位名称"),
            // entity.employeeReassignment.frompostname
            new TranslationSeedItem("entity.employeeReassignment.frompostname", "zh-CN", "调出岗位名称", "调出岗位名称"),
            // entity.employeeReassignment.frompostname
            new TranslationSeedItem("entity.employeeReassignment.frompostname", "zh-HK", "调出岗位名称", "调出岗位名称"),

            // entity.employeeReassignment.todeptid
            new TranslationSeedItem("entity.employeeReassignment.todeptid", "en-US", "调入部门ID", "调入部门ID"),
            // entity.employeeReassignment.todeptid
            new TranslationSeedItem("entity.employeeReassignment.todeptid", "ja-JP", "调入部门ID", "调入部门ID"),
            // entity.employeeReassignment.todeptid
            new TranslationSeedItem("entity.employeeReassignment.todeptid", "zh-CN", "调入部门ID", "调入部门ID"),
            // entity.employeeReassignment.todeptid
            new TranslationSeedItem("entity.employeeReassignment.todeptid", "zh-HK", "调入部门ID", "调入部门ID"),

            // entity.employeeReassignment.todeptname
            new TranslationSeedItem("entity.employeeReassignment.todeptname", "en-US", "调入部门名称", "调入部门名称"),
            // entity.employeeReassignment.todeptname
            new TranslationSeedItem("entity.employeeReassignment.todeptname", "ja-JP", "调入部门名称", "调入部门名称"),
            // entity.employeeReassignment.todeptname
            new TranslationSeedItem("entity.employeeReassignment.todeptname", "zh-CN", "调入部门名称", "调入部门名称"),
            // entity.employeeReassignment.todeptname
            new TranslationSeedItem("entity.employeeReassignment.todeptname", "zh-HK", "调入部门名称", "调入部门名称"),

            // entity.employeeReassignment.topostid
            new TranslationSeedItem("entity.employeeReassignment.topostid", "en-US", "调入岗位ID", "调入岗位ID"),
            // entity.employeeReassignment.topostid
            new TranslationSeedItem("entity.employeeReassignment.topostid", "ja-JP", "调入岗位ID", "调入岗位ID"),
            // entity.employeeReassignment.topostid
            new TranslationSeedItem("entity.employeeReassignment.topostid", "zh-CN", "调入岗位ID", "调入岗位ID"),
            // entity.employeeReassignment.topostid
            new TranslationSeedItem("entity.employeeReassignment.topostid", "zh-HK", "调入岗位ID", "调入岗位ID"),

            // entity.employeeReassignment.topostname
            new TranslationSeedItem("entity.employeeReassignment.topostname", "en-US", "调入岗位名称", "调入岗位名称"),
            // entity.employeeReassignment.topostname
            new TranslationSeedItem("entity.employeeReassignment.topostname", "ja-JP", "调入岗位名称", "调入岗位名称"),
            // entity.employeeReassignment.topostname
            new TranslationSeedItem("entity.employeeReassignment.topostname", "zh-CN", "调入岗位名称", "调入岗位名称"),
            // entity.employeeReassignment.topostname
            new TranslationSeedItem("entity.employeeReassignment.topostname", "zh-HK", "调入岗位名称", "调入岗位名称"),

            // entity.employeeReassignment.effectivedate
            new TranslationSeedItem("entity.employeeReassignment.effectivedate", "en-US", "生效日期", "生效日期"),
            // entity.employeeReassignment.effectivedate
            new TranslationSeedItem("entity.employeeReassignment.effectivedate", "ja-JP", "生效日期", "生效日期"),
            // entity.employeeReassignment.effectivedate
            new TranslationSeedItem("entity.employeeReassignment.effectivedate", "zh-CN", "生效日期", "生效日期"),
            // entity.employeeReassignment.effectivedate
            new TranslationSeedItem("entity.employeeReassignment.effectivedate", "zh-HK", "生效日期", "生效日期"),

            // entity.employeeReassignment.reason
            new TranslationSeedItem("entity.employeeReassignment.reason", "en-US", "调动原因", "调动原因"),
            // entity.employeeReassignment.reason
            new TranslationSeedItem("entity.employeeReassignment.reason", "ja-JP", "调动原因", "调动原因"),
            // entity.employeeReassignment.reason
            new TranslationSeedItem("entity.employeeReassignment.reason", "zh-CN", "调动原因", "调动原因"),
            // entity.employeeReassignment.reason
            new TranslationSeedItem("entity.employeeReassignment.reason", "zh-HK", "调动原因", "调动原因"),
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
