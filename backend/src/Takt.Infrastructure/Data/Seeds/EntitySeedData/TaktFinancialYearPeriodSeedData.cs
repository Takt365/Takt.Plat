// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.EntitySeedData
// 文件名称：TaktFinancialYearPeriodSeedData.cs
// 创建时间：2026-07-06
// 创建人：Takt365(Cursor AI)
// 功能描述：财务年度期间种子（FY2000～FY2099；四类财年；幂等创建或更新）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Entities.Accounting.Financial;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.EntitySeedData;

/// <summary>
/// 财务年度期间种子（租户级；CN/JP/HK/US 四类财年，FY2000～FY2099）
/// </summary>
public class TaktFinancialYearPeriodSeedData : ITaktSeedDataCoordinator
{
    private const int IsBuiltInYes = 1;
    private const int FiscalYearStart = 2000;
    private const int FiscalYearEnd = 2099;
    private const string CategoryChina = "CN";
    private const string CategoryJapan = "JP";
    private const string CategoryHongKong = "HK";
    private const string CategoryUnitedStates = "US";

    private static readonly string[] FinancialYearCategories =
    [
        CategoryChina,
        CategoryJapan,
        CategoryHongKong,
        CategoryUnitedStates,
    ];

    /// <summary>
    /// 执行顺序（汇率种子之后）
    /// </summary>
    public int Order => 462;

    /// <summary>
    /// 初始化财务年度期间种子数据
    /// </summary>
    /// <param name="serviceProvider">服务提供者</param>
    /// <param name="tenantCode">租户编码（由协调器传入）</param>
    /// <returns>返回插入和更新的记录数（插入数, 更新数）</returns>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(
        IServiceProvider serviceProvider,
        string? tenantCode = null)
    {
        TaktLogger.Information("开始初始化财务年度期间种子数据...");
        if (string.IsNullOrEmpty(tenantCode))
        {
            TaktLogger.Warning("租户编码为空，跳过财务年度期间种子数据初始化");
            return (0, 0);
        }
        var repository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktFinancialYearPeriod>>();
        var periods = BuildFinancialYearPeriods();
        var insertCount = 0;
        var updateCount = 0;
        TaktLogger.Information(
            "正在为租户 {TenantCode} 初始化财务年度期间（共 {Count} 条）...",
            tenantCode,
            periods.Count);
        foreach (var seed in periods)
        {
            var (_, inserted, updated) = await CreateOrUpdateFinancialYearPeriodAsync(
                repository,
                tenantCode,
                seed);
            insertCount += inserted;
            updateCount += updated;
        }
        TaktLogger.Information(
            "财务年度期间种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条",
            insertCount,
            updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// 生成四类财年期间列表（每类 FY2000～FY2099，共 4800 条）
    /// </summary>
    /// <returns>期间种子项列表</returns>
    private static List<FinancialYearPeriodSeedItem> BuildFinancialYearPeriods()
    {
        var items = new List<FinancialYearPeriodSeedItem>();
        foreach (var category in FinancialYearCategories)
        {
            items.AddRange(BuildPeriodsForCategory(category));
        }
        return items;
    }

    /// <summary>
    /// 按财年类别生成期间
    /// </summary>
    /// <param name="category">财年类别 DictValue</param>
    /// <returns>该类别期间种子项</returns>
    private static IEnumerable<FinancialYearPeriodSeedItem> BuildPeriodsForCategory(string category)
    {
        return category switch
        {
            CategoryChina => BuildChinaCalendarYearPeriods(),
            CategoryJapan or CategoryHongKong => BuildAprilMarchYearPeriods(category),
            CategoryUnitedStates => BuildOctoberSeptemberYearPeriods(),
            _ => throw new InvalidOperationException($"未支持的财务年度类别 {category}"),
        };
    }

    /// <summary>
    /// 中国财年（自然年 1/1～12/31；FY2000=2000/1/1～2000/12/31）
    /// </summary>
    private static IEnumerable<FinancialYearPeriodSeedItem> BuildChinaCalendarYearPeriods()
    {
        var cursor = new DateTime(FiscalYearStart, 1, 1);
        var end = new DateTime(FiscalYearEnd, 12, 1);
        while (cursor <= end)
        {
            yield return CreateSeedItem(
                CategoryChina,
                cursor.Year,
                cursor.Month,
                $"FY{cursor.Year}",
                ResolveChinaQuarterCode(cursor.Month));
            cursor = cursor.AddMonths(1);
        }
    }

    /// <summary>
    /// 日本/香港财年（4/1～次年 3/31；FY2000=1999/4/1～2000/3/31）
    /// </summary>
    /// <param name="category">JP 或 HK</param>
    private static IEnumerable<FinancialYearPeriodSeedItem> BuildAprilMarchYearPeriods(string category)
    {
        var cursor = new DateTime(FiscalYearStart - 1, 4, 1);
        var end = new DateTime(FiscalYearEnd, 3, 1);
        while (cursor <= end)
        {
            var calendarYear = cursor.Year;
            var calendarMonth = cursor.Month;
            var fiscalYearNumber = calendarMonth >= 4 ? calendarYear + 1 : calendarYear;
            yield return CreateSeedItem(
                category,
                calendarYear,
                calendarMonth,
                $"FY{fiscalYearNumber}",
                ResolveAprilMarchQuarterCode(calendarMonth));
            cursor = cursor.AddMonths(1);
        }
    }

    /// <summary>
    /// 美国财年（10/1～次年 9/30；FY2000=1999/10/1～2000/9/30）
    /// </summary>
    private static IEnumerable<FinancialYearPeriodSeedItem> BuildOctoberSeptemberYearPeriods()
    {
        var cursor = new DateTime(FiscalYearStart - 1, 10, 1);
        var end = new DateTime(FiscalYearEnd, 9, 1);
        while (cursor <= end)
        {
            var calendarYear = cursor.Year;
            var calendarMonth = cursor.Month;
            var fiscalYearNumber = calendarMonth >= 10 ? calendarYear + 1 : calendarYear;
            yield return CreateSeedItem(
                CategoryUnitedStates,
                calendarYear,
                calendarMonth,
                $"FY{fiscalYearNumber}",
                ResolveOctoberSeptemberQuarterCode(calendarMonth));
            cursor = cursor.AddMonths(1);
        }
    }

    /// <summary>
    /// 组装单条期间种子项
    /// </summary>
    private static FinancialYearPeriodSeedItem CreateSeedItem(
        string category,
        int calendarYear,
        int calendarMonth,
        string financialYearCode,
        string financialQuarterCode)
    {
        return new FinancialYearPeriodSeedItem(
            category,
            financialYearCode,
            $"{calendarYear:D4}{calendarMonth:D2}",
            calendarYear,
            calendarMonth,
            financialQuarterCode);
    }

    /// <summary>
    /// 中国财年财季（Q1=1～3，Q2=4～6，Q3=7～9，Q4=10～12）
    /// </summary>
    private static string ResolveChinaQuarterCode(int calendarMonth)
    {
        return calendarMonth switch
        {
            1 or 2 or 3 => "Q1",
            4 or 5 or 6 => "Q2",
            7 or 8 or 9 => "Q3",
            10 or 11 or 12 => "Q4",
            _ => throw new InvalidOperationException($"无效自然月 {calendarMonth}"),
        };
    }

    /// <summary>
    /// 日本/香港财年财季（Q1=4～6，Q2=7～9，Q3=10～12，Q4=1～3）
    /// </summary>
    private static string ResolveAprilMarchQuarterCode(int calendarMonth)
    {
        return calendarMonth switch
        {
            4 or 5 or 6 => "Q1",
            7 or 8 or 9 => "Q2",
            10 or 11 or 12 => "Q3",
            1 or 2 or 3 => "Q4",
            _ => throw new InvalidOperationException($"无效自然月 {calendarMonth}"),
        };
    }

    /// <summary>
    /// 美国财年财季（Q1=10～12，Q2=1～3，Q3=4～6，Q4=7～9）
    /// </summary>
    private static string ResolveOctoberSeptemberQuarterCode(int calendarMonth)
    {
        return calendarMonth switch
        {
            10 or 11 or 12 => "Q1",
            1 or 2 or 3 => "Q2",
            4 or 5 or 6 => "Q3",
            7 or 8 or 9 => "Q4",
            _ => throw new InvalidOperationException($"无效自然月 {calendarMonth}"),
        };
    }

    /// <summary>
    /// 创建或更新财务年度期间
    /// </summary>
    private static async Task<(TaktFinancialYearPeriod Period, int InsertCount, int UpdateCount)> CreateOrUpdateFinancialYearPeriodAsync(
        ITaktTenantSeedRepository<TaktFinancialYearPeriod> repository,
        string tenantCode,
        FinancialYearPeriodSeedItem seed)
    {
        var period = await repository.FirstAsync(x =>
            x.TenantCode == tenantCode
            && x.FinancialYearCategory == seed.FinancialYearCategory
            && x.PeriodCode == seed.PeriodCode);
        if (period == null)
        {
            period = new TaktFinancialYearPeriod
            {
                TenantCode = tenantCode,
                FinancialYearCategory = seed.FinancialYearCategory,
                FinancialYearCode = seed.FinancialYearCode,
                PeriodCode = seed.PeriodCode,
                CalendarYear = seed.CalendarYear,
                CalendarMonth = seed.CalendarMonth,
                FinancialQuarterCode = seed.FinancialQuarterCode,
                IsBuiltIn = IsBuiltInYes,
            };
            period = await repository.CreateAsync(period);
            return (period, 1, 0);
        }
        var needUpdate = false;
        if (period.FinancialYearCode != seed.FinancialYearCode)
        {
            period.FinancialYearCode = seed.FinancialYearCode;
            needUpdate = true;
        }
        if (period.CalendarYear != seed.CalendarYear)
        {
            period.CalendarYear = seed.CalendarYear;
            needUpdate = true;
        }
        if (period.CalendarMonth != seed.CalendarMonth)
        {
            period.CalendarMonth = seed.CalendarMonth;
            needUpdate = true;
        }
        if (period.FinancialQuarterCode != seed.FinancialQuarterCode)
        {
            period.FinancialQuarterCode = seed.FinancialQuarterCode;
            needUpdate = true;
        }
        if (period.IsBuiltIn != IsBuiltInYes)
        {
            period.IsBuiltIn = IsBuiltInYes;
            needUpdate = true;
        }
        if (needUpdate)
        {
            await repository.UpdateAsync(period);
            return (period, 0, 1);
        }
        return (period, 0, 0);
    }

    /// <summary>
    /// 财务年度期间种子项
    /// </summary>
    private sealed record FinancialYearPeriodSeedItem(
        string FinancialYearCategory,
        string FinancialYearCode,
        string PeriodCode,
        int CalendarYear,
        int CalendarMonth,
        string FinancialQuarterCode);
}
