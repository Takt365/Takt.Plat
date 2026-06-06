// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Personnel
// 文件名称：TaktEmployeeResignationI18nSeedData.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktEmployeeResignation 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktEmployeeResignation 实体国际化翻译种子（键前缀 entity.employeeResignation.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktEmployeeResignationI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktEmployeeResignation 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 employeeResignation 实体翻译...", tenantCode);

        foreach (var item in GetEmployeeResignationTranslations())
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

        TaktLogger.Information("TaktEmployeeResignation 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktEmployeeResignation 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.employeeResignation._self / entity.employeeResignation.{{field}}；ResourceGroup=TaktModule.HumanResource；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetEmployeeResignationTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.employeeResignation._self
            new TranslationSeedItem("entity.employeeResignation._self", "en-US", "Employee Resignation Information", "实体名称"),
            // entity.employeeResignation._self
            new TranslationSeedItem("entity.employeeResignation._self", "ja-JP", "员工离职办理记录信息", "实体名称"),
            // entity.employeeResignation._self
            new TranslationSeedItem("entity.employeeResignation._self", "zh-CN", "员工离职办理记录信息", "实体名称"),
            // entity.employeeResignation._self
            new TranslationSeedItem("entity.employeeResignation._self", "zh-HK", "员工离职办理记录信息", "实体名称"),

            // entity.employeeResignation.employeeid
            new TranslationSeedItem("entity.employeeResignation.employeeid", "en-US", "员工ID", "员工ID"),
            // entity.employeeResignation.employeeid
            new TranslationSeedItem("entity.employeeResignation.employeeid", "ja-JP", "员工ID", "员工ID"),
            // entity.employeeResignation.employeeid
            new TranslationSeedItem("entity.employeeResignation.employeeid", "zh-CN", "员工ID", "员工ID"),
            // entity.employeeResignation.employeeid
            new TranslationSeedItem("entity.employeeResignation.employeeid", "zh-HK", "员工ID", "员工ID"),

            // entity.employeeResignation.resignationtype
            new TranslationSeedItem("entity.employeeResignation.resignationtype", "en-US", "离职类型", "离职类型（0=主动辞职，1=公司辞退，2=合同到期，3=退休，9=其他）"),
            // entity.employeeResignation.resignationtype
            new TranslationSeedItem("entity.employeeResignation.resignationtype", "ja-JP", "离职类型", "离职类型（0=主动辞职，1=公司辞退，2=合同到期，3=退休，9=其他）"),
            // entity.employeeResignation.resignationtype
            new TranslationSeedItem("entity.employeeResignation.resignationtype", "zh-CN", "离职类型", "离职类型（0=主动辞职，1=公司辞退，2=合同到期，3=退休，9=其他）"),
            // entity.employeeResignation.resignationtype
            new TranslationSeedItem("entity.employeeResignation.resignationtype", "zh-HK", "离职类型", "离职类型（0=主动辞职，1=公司辞退，2=合同到期，3=退休，9=其他）"),

            // entity.employeeResignation.applydate
            new TranslationSeedItem("entity.employeeResignation.applydate", "en-US", "申请日期", "申请日期"),
            // entity.employeeResignation.applydate
            new TranslationSeedItem("entity.employeeResignation.applydate", "ja-JP", "申请日期", "申请日期"),
            // entity.employeeResignation.applydate
            new TranslationSeedItem("entity.employeeResignation.applydate", "zh-CN", "申请日期", "申请日期"),
            // entity.employeeResignation.applydate
            new TranslationSeedItem("entity.employeeResignation.applydate", "zh-HK", "申请日期", "申请日期"),

            // entity.employeeResignation.lastworkdate
            new TranslationSeedItem("entity.employeeResignation.lastworkdate", "en-US", "最后工作日", "最后工作日"),
            // entity.employeeResignation.lastworkdate
            new TranslationSeedItem("entity.employeeResignation.lastworkdate", "ja-JP", "最后工作日", "最后工作日"),
            // entity.employeeResignation.lastworkdate
            new TranslationSeedItem("entity.employeeResignation.lastworkdate", "zh-CN", "最后工作日", "最后工作日"),
            // entity.employeeResignation.lastworkdate
            new TranslationSeedItem("entity.employeeResignation.lastworkdate", "zh-HK", "最后工作日", "最后工作日"),

            // entity.employeeResignation.terminationdate
            new TranslationSeedItem("entity.employeeResignation.terminationdate", "en-US", "实际离职日期", "实际离职日期"),
            // entity.employeeResignation.terminationdate
            new TranslationSeedItem("entity.employeeResignation.terminationdate", "ja-JP", "实际离职日期", "实际离职日期"),
            // entity.employeeResignation.terminationdate
            new TranslationSeedItem("entity.employeeResignation.terminationdate", "zh-CN", "实际离职日期", "实际离职日期"),
            // entity.employeeResignation.terminationdate
            new TranslationSeedItem("entity.employeeResignation.terminationdate", "zh-HK", "实际离职日期", "实际离职日期"),

            // entity.employeeResignation.reason
            new TranslationSeedItem("entity.employeeResignation.reason", "en-US", "离职原因", "离职原因"),
            // entity.employeeResignation.reason
            new TranslationSeedItem("entity.employeeResignation.reason", "ja-JP", "离职原因", "离职原因"),
            // entity.employeeResignation.reason
            new TranslationSeedItem("entity.employeeResignation.reason", "zh-CN", "离职原因", "离职原因"),
            // entity.employeeResignation.reason
            new TranslationSeedItem("entity.employeeResignation.reason", "zh-HK", "离职原因", "离职原因"),

            // entity.employeeResignation.handovernotes
            new TranslationSeedItem("entity.employeeResignation.handovernotes", "en-US", "工作交接说明", "工作交接说明"),
            // entity.employeeResignation.handovernotes
            new TranslationSeedItem("entity.employeeResignation.handovernotes", "ja-JP", "工作交接说明", "工作交接说明"),
            // entity.employeeResignation.handovernotes
            new TranslationSeedItem("entity.employeeResignation.handovernotes", "zh-CN", "工作交接说明", "工作交接说明"),
            // entity.employeeResignation.handovernotes
            new TranslationSeedItem("entity.employeeResignation.handovernotes", "zh-HK", "工作交接说明", "工作交接说明"),
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
