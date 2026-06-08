// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Attendance
// 文件名称：TaktOvertimeItemI18nSeedData.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktOvertimeItem 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Attendance;

/// <summary>
/// TaktOvertimeItem 实体国际化翻译种子（键前缀 entity.overtimeItem.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktOvertimeItemI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktOvertimeItem 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 overtimeItem 实体翻译...", tenantCode);

        foreach (var item in GetOvertimeItemTranslations())
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

        TaktLogger.Information("TaktOvertimeItem 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktOvertimeItem 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.overtimeItem._self / entity.overtimeItem.{{field}}；ResourceGroup=TaktModule.HumanResource；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetOvertimeItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.overtimeItem._self
            new TranslationSeedItem("entity.overtimeItem._self", "en-US", "Overtime Item Information", "实体名称"),
            // entity.overtimeItem._self
            new TranslationSeedItem("entity.overtimeItem._self", "ja-JP", "加班申请明细信息", "实体名称"),
            // entity.overtimeItem._self
            new TranslationSeedItem("entity.overtimeItem._self", "zh-CN", "加班申请明细信息", "实体名称"),
            // entity.overtimeItem._self
            new TranslationSeedItem("entity.overtimeItem._self", "zh-HK", "加班申请明细信息", "实体名称"),

            // entity.overtimeItem.overtimeid
            new TranslationSeedItem("entity.overtimeItem.overtimeid", "en-US", "加班申请单ID", "加班申请单 ID"),
            // entity.overtimeItem.overtimeid
            new TranslationSeedItem("entity.overtimeItem.overtimeid", "ja-JP", "加班申请单ID", "加班申请单 ID"),
            // entity.overtimeItem.overtimeid
            new TranslationSeedItem("entity.overtimeItem.overtimeid", "zh-CN", "加班申请单ID", "加班申请单 ID"),
            // entity.overtimeItem.overtimeid
            new TranslationSeedItem("entity.overtimeItem.overtimeid", "zh-HK", "加班申请单ID", "加班申请单 ID"),

            // entity.overtimeItem.linenumber
            new TranslationSeedItem("entity.overtimeItem.linenumber", "en-US", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.overtimeItem.linenumber
            new TranslationSeedItem("entity.overtimeItem.linenumber", "ja-JP", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.overtimeItem.linenumber
            new TranslationSeedItem("entity.overtimeItem.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.overtimeItem.linenumber
            new TranslationSeedItem("entity.overtimeItem.linenumber", "zh-HK", "行号", "行号（项号/序号，固定步长=10）"),

            // entity.overtimeItem.employeeid
            new TranslationSeedItem("entity.overtimeItem.employeeid", "en-US", "员工ID", "员工 ID"),
            // entity.overtimeItem.employeeid
            new TranslationSeedItem("entity.overtimeItem.employeeid", "ja-JP", "员工ID", "员工 ID"),
            // entity.overtimeItem.employeeid
            new TranslationSeedItem("entity.overtimeItem.employeeid", "zh-CN", "员工ID", "员工 ID"),
            // entity.overtimeItem.employeeid
            new TranslationSeedItem("entity.overtimeItem.employeeid", "zh-HK", "员工ID", "员工 ID"),

            // entity.overtimeItem.employeename
            new TranslationSeedItem("entity.overtimeItem.employeename", "en-US", "员工姓名", "员工姓名"),
            // entity.overtimeItem.employeename
            new TranslationSeedItem("entity.overtimeItem.employeename", "ja-JP", "员工姓名", "员工姓名"),
            // entity.overtimeItem.employeename
            new TranslationSeedItem("entity.overtimeItem.employeename", "zh-CN", "员工姓名", "员工姓名"),
            // entity.overtimeItem.employeename
            new TranslationSeedItem("entity.overtimeItem.employeename", "zh-HK", "员工姓名", "员工姓名"),

            // entity.overtimeItem.plannedhours
            new TranslationSeedItem("entity.overtimeItem.plannedhours", "en-US", "计划小时数", "计划加班小时数"),
            // entity.overtimeItem.plannedhours
            new TranslationSeedItem("entity.overtimeItem.plannedhours", "ja-JP", "计划小时数", "计划加班小时数"),
            // entity.overtimeItem.plannedhours
            new TranslationSeedItem("entity.overtimeItem.plannedhours", "zh-CN", "计划小时数", "计划加班小时数"),
            // entity.overtimeItem.plannedhours
            new TranslationSeedItem("entity.overtimeItem.plannedhours", "zh-HK", "计划小时数", "计划加班小时数"),

            // entity.overtimeItem.actualstarttime
            new TranslationSeedItem("entity.overtimeItem.actualstarttime", "en-US", "实际开始时间", "实际加班开始时间"),
            // entity.overtimeItem.actualstarttime
            new TranslationSeedItem("entity.overtimeItem.actualstarttime", "ja-JP", "实际开始时间", "实际加班开始时间"),
            // entity.overtimeItem.actualstarttime
            new TranslationSeedItem("entity.overtimeItem.actualstarttime", "zh-CN", "实际开始时间", "实际加班开始时间"),
            // entity.overtimeItem.actualstarttime
            new TranslationSeedItem("entity.overtimeItem.actualstarttime", "zh-HK", "实际开始时间", "实际加班开始时间"),

            // entity.overtimeItem.actualendtime
            new TranslationSeedItem("entity.overtimeItem.actualendtime", "en-US", "实际结束时间", "实际加班结束时间"),
            // entity.overtimeItem.actualendtime
            new TranslationSeedItem("entity.overtimeItem.actualendtime", "ja-JP", "实际结束时间", "实际加班结束时间"),
            // entity.overtimeItem.actualendtime
            new TranslationSeedItem("entity.overtimeItem.actualendtime", "zh-CN", "实际结束时间", "实际加班结束时间"),
            // entity.overtimeItem.actualendtime
            new TranslationSeedItem("entity.overtimeItem.actualendtime", "zh-HK", "实际结束时间", "实际加班结束时间"),

            // entity.overtimeItem.actualhours
            new TranslationSeedItem("entity.overtimeItem.actualhours", "en-US", "实际小时数", "实际加班小时数"),
            // entity.overtimeItem.actualhours
            new TranslationSeedItem("entity.overtimeItem.actualhours", "ja-JP", "实际小时数", "实际加班小时数"),
            // entity.overtimeItem.actualhours
            new TranslationSeedItem("entity.overtimeItem.actualhours", "zh-CN", "实际小时数", "实际加班小时数"),
            // entity.overtimeItem.actualhours
            new TranslationSeedItem("entity.overtimeItem.actualhours", "zh-HK", "实际小时数", "实际加班小时数"),

            // entity.overtimeItem.overtime
            new TranslationSeedItem("entity.overtimeItem.overtime", "en-US", "加班主表", "加班主表"),
            // entity.overtimeItem.overtime
            new TranslationSeedItem("entity.overtimeItem.overtime", "ja-JP", "加班主表", "加班主表"),
            // entity.overtimeItem.overtime
            new TranslationSeedItem("entity.overtimeItem.overtime", "zh-CN", "加班主表", "加班主表"),
            // entity.overtimeItem.overtime
            new TranslationSeedItem("entity.overtimeItem.overtime", "zh-HK", "加班主表", "加班主表"),
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
