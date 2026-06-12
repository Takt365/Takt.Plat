// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Attendance
// 文件名称：TaktCalendarI18nSeedData.cs
// 创建时间：2026-06-12
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktCalendar 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktCalendar 实体国际化翻译种子（键前缀 entity.calendar.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktCalendarI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktCalendar 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 calendar 实体翻译...", tenantCode);

        foreach (var item in GetCalendarTranslations())
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

        TaktLogger.Information("TaktCalendar 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktCalendar 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.calendar._self / entity.calendar.{{field}}；ResourceGroup=5；ResourceType=0
    /// </summary>
    private static List<TranslationSeedItem> GetCalendarTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.calendar._self
            new TranslationSeedItem("entity.calendar._self", "en-US", "Calendar Information", "实体名称"),
            // entity.calendar._self
            new TranslationSeedItem("entity.calendar._self", "ja-JP", "工厂日历信息", "实体名称"),
            // entity.calendar._self
            new TranslationSeedItem("entity.calendar._self", "zh-CN", "工厂日历信息", "实体名称"),
            // entity.calendar._self
            new TranslationSeedItem("entity.calendar._self", "zh-HK", "工厂日历信息", "实体名称"),

            // entity.calendar.date
            new TranslationSeedItem("entity.calendar.date", "en-US", "日历日期", "日历日期"),
            // entity.calendar.date
            new TranslationSeedItem("entity.calendar.date", "ja-JP", "日历日期", "日历日期"),
            // entity.calendar.date
            new TranslationSeedItem("entity.calendar.date", "zh-CN", "日历日期", "日历日期"),
            // entity.calendar.date
            new TranslationSeedItem("entity.calendar.date", "zh-HK", "日历日期", "日历日期"),

            // entity.calendar.isworkingday
            new TranslationSeedItem("entity.calendar.isworkingday", "en-US", "是否工作日", "是否工作日（0=非工作日 1=工作日 2=调休工作日等）"),
            // entity.calendar.isworkingday
            new TranslationSeedItem("entity.calendar.isworkingday", "ja-JP", "是否工作日", "是否工作日（0=非工作日 1=工作日 2=调休工作日等）"),
            // entity.calendar.isworkingday
            new TranslationSeedItem("entity.calendar.isworkingday", "zh-CN", "是否工作日", "是否工作日（0=非工作日 1=工作日 2=调休工作日等）"),
            // entity.calendar.isworkingday
            new TranslationSeedItem("entity.calendar.isworkingday", "zh-HK", "是否工作日", "是否工作日（0=非工作日 1=工作日 2=调休工作日等）"),

            // entity.calendar.holidayid
            new TranslationSeedItem("entity.calendar.holidayid", "en-US", "关联假日ID", "关联假日 ID（TaktHoliday）"),
            // entity.calendar.holidayid
            new TranslationSeedItem("entity.calendar.holidayid", "ja-JP", "关联假日ID", "关联假日 ID（TaktHoliday）"),
            // entity.calendar.holidayid
            new TranslationSeedItem("entity.calendar.holidayid", "zh-CN", "关联假日ID", "关联假日 ID（TaktHoliday）"),
            // entity.calendar.holidayid
            new TranslationSeedItem("entity.calendar.holidayid", "zh-HK", "关联假日ID", "关联假日 ID（TaktHoliday）"),

            // entity.calendar.shiftid
            new TranslationSeedItem("entity.calendar.shiftid", "en-US", "关联班次ID", "关联班次 ID（TaktWorkShift）"),
            // entity.calendar.shiftid
            new TranslationSeedItem("entity.calendar.shiftid", "ja-JP", "关联班次ID", "关联班次 ID（TaktWorkShift）"),
            // entity.calendar.shiftid
            new TranslationSeedItem("entity.calendar.shiftid", "zh-CN", "关联班次ID", "关联班次 ID（TaktWorkShift）"),
            // entity.calendar.shiftid
            new TranslationSeedItem("entity.calendar.shiftid", "zh-HK", "关联班次ID", "关联班次 ID（TaktWorkShift）"),

            // entity.calendar.relatedplant
            new TranslationSeedItem("entity.calendar.relatedplant", "en-US", "关联工厂", "关联工厂（为空表示公司级通用日历）"),
            // entity.calendar.relatedplant
            new TranslationSeedItem("entity.calendar.relatedplant", "ja-JP", "关联工厂", "关联工厂（为空表示公司级通用日历）"),
            // entity.calendar.relatedplant
            new TranslationSeedItem("entity.calendar.relatedplant", "zh-CN", "关联工厂", "关联工厂（为空表示公司级通用日历）"),
            // entity.calendar.relatedplant
            new TranslationSeedItem("entity.calendar.relatedplant", "zh-HK", "关联工厂", "关联工厂（为空表示公司级通用日历）"),
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
