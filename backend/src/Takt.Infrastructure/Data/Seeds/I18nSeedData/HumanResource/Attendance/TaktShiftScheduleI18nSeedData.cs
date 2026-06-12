// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Attendance
// 文件名称：TaktShiftScheduleI18nSeedData.cs
// 创建时间：2026-06-12
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktShiftSchedule 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Attendance;

/// <summary>
/// TaktShiftSchedule 实体国际化翻译种子（键前缀 entity.shiftschedule.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktShiftScheduleI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktShiftSchedule 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 shiftschedule 实体翻译...", tenantCode);

        foreach (var item in GetShiftScheduleTranslations())
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

        TaktLogger.Information("TaktShiftSchedule 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktShiftSchedule 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.shiftschedule._self / entity.shiftschedule.{{field}}；ResourceGroup=5；ResourceType=0
    /// </summary>
    private static List<TranslationSeedItem> GetShiftScheduleTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.shiftschedule._self
            new TranslationSeedItem("entity.shiftschedule._self", "en-US", "Shift Schedule Information", "实体名称"),
            // entity.shiftschedule._self
            new TranslationSeedItem("entity.shiftschedule._self", "ja-JP", "排班计划信息", "实体名称"),
            // entity.shiftschedule._self
            new TranslationSeedItem("entity.shiftschedule._self", "zh-CN", "排班计划信息", "实体名称"),
            // entity.shiftschedule._self
            new TranslationSeedItem("entity.shiftschedule._self", "zh-HK", "排班计划信息", "实体名称"),

            // entity.shiftschedule.scheduletype
            new TranslationSeedItem("entity.shiftschedule.scheduletype", "en-US", "排班类别", "排班类别（0=部门 1=人员）"),
            // entity.shiftschedule.scheduletype
            new TranslationSeedItem("entity.shiftschedule.scheduletype", "ja-JP", "排班类别", "排班类别（0=部门 1=人员）"),
            // entity.shiftschedule.scheduletype
            new TranslationSeedItem("entity.shiftschedule.scheduletype", "zh-CN", "排班类别", "排班类别（0=部门 1=人员）"),
            // entity.shiftschedule.scheduletype
            new TranslationSeedItem("entity.shiftschedule.scheduletype", "zh-HK", "排班类别", "排班类别（0=部门 1=人员）"),

            // entity.shiftschedule.deptid
            new TranslationSeedItem("entity.shiftschedule.deptid", "en-US", "部门ID", "部门 ID（ScheduleType=0 时必填）"),
            // entity.shiftschedule.deptid
            new TranslationSeedItem("entity.shiftschedule.deptid", "ja-JP", "部门ID", "部门 ID（ScheduleType=0 时必填）"),
            // entity.shiftschedule.deptid
            new TranslationSeedItem("entity.shiftschedule.deptid", "zh-CN", "部门ID", "部门 ID（ScheduleType=0 时必填）"),
            // entity.shiftschedule.deptid
            new TranslationSeedItem("entity.shiftschedule.deptid", "zh-HK", "部门ID", "部门 ID（ScheduleType=0 时必填）"),

            // entity.shiftschedule.employeeid
            new TranslationSeedItem("entity.shiftschedule.employeeid", "en-US", "员工ID", "员工 ID（ScheduleType=1 时必填）"),
            // entity.shiftschedule.employeeid
            new TranslationSeedItem("entity.shiftschedule.employeeid", "ja-JP", "员工ID", "员工 ID（ScheduleType=1 时必填）"),
            // entity.shiftschedule.employeeid
            new TranslationSeedItem("entity.shiftschedule.employeeid", "zh-CN", "员工ID", "员工 ID（ScheduleType=1 时必填）"),
            // entity.shiftschedule.employeeid
            new TranslationSeedItem("entity.shiftschedule.employeeid", "zh-HK", "员工ID", "员工 ID（ScheduleType=1 时必填）"),

            // entity.shiftschedule.scheduledate
            new TranslationSeedItem("entity.shiftschedule.scheduledate", "en-US", "排班日期", "排班日期"),
            // entity.shiftschedule.scheduledate
            new TranslationSeedItem("entity.shiftschedule.scheduledate", "ja-JP", "排班日期", "排班日期"),
            // entity.shiftschedule.scheduledate
            new TranslationSeedItem("entity.shiftschedule.scheduledate", "zh-CN", "排班日期", "排班日期"),
            // entity.shiftschedule.scheduledate
            new TranslationSeedItem("entity.shiftschedule.scheduledate", "zh-HK", "排班日期", "排班日期"),

            // entity.shiftschedule.shiftid
            new TranslationSeedItem("entity.shiftschedule.shiftid", "en-US", "班次ID", "班次 ID（TaktWorkShift）"),
            // entity.shiftschedule.shiftid
            new TranslationSeedItem("entity.shiftschedule.shiftid", "ja-JP", "班次ID", "班次 ID（TaktWorkShift）"),
            // entity.shiftschedule.shiftid
            new TranslationSeedItem("entity.shiftschedule.shiftid", "zh-CN", "班次ID", "班次 ID（TaktWorkShift）"),
            // entity.shiftschedule.shiftid
            new TranslationSeedItem("entity.shiftschedule.shiftid", "zh-HK", "班次ID", "班次 ID（TaktWorkShift）"),

            // entity.shiftschedule.relatedplant
            new TranslationSeedItem("entity.shiftschedule.relatedplant", "en-US", "关联工厂", "关联工厂"),
            // entity.shiftschedule.relatedplant
            new TranslationSeedItem("entity.shiftschedule.relatedplant", "ja-JP", "关联工厂", "关联工厂"),
            // entity.shiftschedule.relatedplant
            new TranslationSeedItem("entity.shiftschedule.relatedplant", "zh-CN", "关联工厂", "关联工厂"),
            // entity.shiftschedule.relatedplant
            new TranslationSeedItem("entity.shiftschedule.relatedplant", "zh-HK", "关联工厂", "关联工厂"),
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
