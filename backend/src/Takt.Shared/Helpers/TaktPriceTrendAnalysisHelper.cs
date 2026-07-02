// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktPriceTrendAnalysisHelper.cs
// 创建时间：2026-07-01
// 创建人：Takt365(Cursor AI)
// 功能描述：物料价格月度波动分析（按主表生效区间解析每月有效单价）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Helpers;

/// <summary>
/// 价格趋势分析单条有效价记录（主表生效区间 + 明细单价）
/// </summary>
public sealed class TaktPriceTrendEntry
{
    /// <summary>
    /// 生效开始日期
    /// </summary>
    public DateTime EffectiveStartDate { get; init; }

    /// <summary>
    /// 生效结束日期（空表示长期有效）
    /// </summary>
    public DateTime? EffectiveEndDate { get; init; }

    /// <summary>
    /// 原始单价（按价格单位 PerUnit 计价）
    /// </summary>
    public decimal RawPrice { get; init; }

    /// <summary>
    /// 价格单位（1/100/1000 等）
    /// </summary>
    public int PerUnit { get; init; }

    /// <summary>
    /// 计量单位
    /// </summary>
    public string Unit { get; init; } = string.Empty;

    /// <summary>
    /// 关联方编码（供应商/客户等，可选）
    /// </summary>
    public string? ReferenceCode { get; init; }
}

/// <summary>
/// 月度价格趋势点
/// </summary>
public sealed class TaktPriceTrendMonthPoint
{
    /// <summary>
    /// 月份 yyyy-MM
    /// </summary>
    public string YearMonth { get; set; } = string.Empty;

    /// <summary>
    /// 当月是否存在有效价格
    /// </summary>
    public bool HasPrice { get; set; }

    /// <summary>
    /// 折算单价（RawPrice / PerUnit；PerUnit 为 0 时等于 RawPrice）
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// 原始单价
    /// </summary>
    public decimal RawPrice { get; set; }

    /// <summary>
    /// 价格单位
    /// </summary>
    public int PerUnit { get; set; }

    /// <summary>
    /// 计量单位
    /// </summary>
    public string Unit { get; set; } = string.Empty;

    /// <summary>
    /// 关联方编码（当月选中记录；多源时为最新生效记录）
    /// </summary>
    public string? ReferenceCode { get; set; }

    /// <summary>
    /// 环比涨跌幅（%）；相对上一有效月份
    /// </summary>
    public decimal? ChangePercent { get; set; }

    /// <summary>
    /// 当月参与汇总的有效价记录数
    /// </summary>
    public int SourceRecordCount { get; set; }
}

/// <summary>
/// 物料价格月度波动分析辅助
/// </summary>
public static class TaktPriceTrendAnalysisHelper
{
    /// <summary>
    /// 默认分析起始年（2016-01-01）
    /// </summary>
    public const int DefaultStartYear = 2016;

    /// <summary>
    /// 最大分析月数（20 年）
    /// </summary>
    public const int MaxTrendMonths = 240;

    /// <summary>
    /// 解析分析日期区间（默认 2016-01-01 至当前月末）
    /// </summary>
    /// <param name="dateStart">开始日期</param>
    /// <param name="dateEnd">结束日期</param>
    /// <returns>闭区间起止日期</returns>
    /// <exception cref="ArgumentException">区间非法或超出月数上限</exception>
    public static (DateTime Start, DateTime End) ResolveTrendDateRange(DateTime? dateStart, DateTime? dateEnd)
    {
        var start = dateStart?.Date ?? new DateTime(DefaultStartYear, 1, 1);
        var endAnchor = dateEnd?.Date ?? DateTime.Now.Date;
        var end = new DateTime(endAnchor.Year, endAnchor.Month, 1).AddMonths(1).AddDays(-1);
        if (end < start)
        {
            throw new ArgumentException("结束日期不能早于开始日期");
        }
        var monthCount = ((end.Year - start.Year) * 12) + end.Month - start.Month + 1;
        if (monthCount > MaxTrendMonths)
        {
            throw new ArgumentException($"分析区间不得超过 {MaxTrendMonths} 个月");
        }
        return (start, end);
    }

    /// <summary>
    /// 折算为每 1 计量单位的单价
    /// </summary>
    /// <param name="rawPrice">原始单价</param>
    /// <param name="perUnit">价格单位</param>
    /// <returns>折算单价</returns>
    public static decimal NormalizeUnitPrice(decimal rawPrice, int perUnit)
    {
        if (perUnit <= 0)
        {
            return rawPrice;
        }
        return Math.Round(rawPrice / perUnit, 5, MidpointRounding.AwayFromZero);
    }

    /// <summary>
    /// 构建月度价格波动序列（每月取生效开始日最新的一条；同日起点多条取均价）
    /// </summary>
    /// <param name="entries">有效价记录</param>
    /// <param name="rangeStart">区间开始</param>
    /// <param name="rangeEnd">区间结束</param>
    /// <returns>按月排序的趋势点</returns>
    public static List<TaktPriceTrendMonthPoint> BuildMonthlyTrendPoints(
        IReadOnlyList<TaktPriceTrendEntry> entries,
        DateTime rangeStart,
        DateTime rangeEnd)
    {
        ArgumentNullException.ThrowIfNull(entries);
        var points = new List<TaktPriceTrendMonthPoint>();
        var cursor = new DateTime(rangeStart.Year, rangeStart.Month, 1);
        var endMonth = new DateTime(rangeEnd.Year, rangeEnd.Month, 1);
        decimal? lastUnitPrice = null;
        while (cursor <= endMonth)
        {
            var monthStart = cursor;
            var monthEnd = cursor.AddMonths(1).AddTicks(-1);
            var yearMonth = cursor.ToString("yyyy-MM");
            var monthEntries = entries
                .Where(x => IsEffectiveInMonth(x, monthStart, monthEnd))
                .ToList();
            if (monthEntries.Count == 0)
            {
                points.Add(new TaktPriceTrendMonthPoint
                {
                    YearMonth = yearMonth,
                    HasPrice = false,
                });
            }
            else
            {
                var latestStart = monthEntries.Max(x => x.EffectiveStartDate.Date);
                var selected = monthEntries
                    .Where(x => x.EffectiveStartDate.Date == latestStart)
                    .ToList();
                var avgRaw = selected.Average(x => x.RawPrice);
                var perUnit = selected[0].PerUnit;
                var unit = selected[0].Unit ?? string.Empty;
                var unitPrice = NormalizeUnitPrice(avgRaw, perUnit);
                decimal? changePercent = null;
                if (lastUnitPrice.HasValue)
                {
                    changePercent = TaktYoYStatHelper.CalculateYoYPercent(unitPrice, lastUnitPrice.Value);
                }
                lastUnitPrice = unitPrice;
                points.Add(new TaktPriceTrendMonthPoint
                {
                    YearMonth = yearMonth,
                    HasPrice = true,
                    UnitPrice = unitPrice,
                    RawPrice = avgRaw,
                    PerUnit = perUnit,
                    Unit = unit,
                    ReferenceCode = selected[0].ReferenceCode,
                    ChangePercent = changePercent,
                    SourceRecordCount = selected.Count,
                });
            }
            cursor = cursor.AddMonths(1);
        }
        return points;
    }

    /// <summary>
    /// 判断价格在自然月内是否有效
    /// </summary>
    /// <param name="entry">价记录</param>
    /// <param name="monthStart">月初</param>
    /// <param name="monthEnd">月末</param>
    /// <returns>是否有效</returns>
    private static bool IsEffectiveInMonth(TaktPriceTrendEntry entry, DateTime monthStart, DateTime monthEnd)
    {
        if (entry.EffectiveStartDate > monthEnd)
        {
            return false;
        }
        if (entry.EffectiveEndDate.HasValue && entry.EffectiveEndDate.Value < monthStart)
        {
            return false;
        }
        return true;
    }
}
