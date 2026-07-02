// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktStatMonthRangeHelper.cs
// 创建时间：2026-06-29
// 创建人：Takt365(Cursor AI)
// 功能描述：统计月份区间解析（默认当月）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Helpers;

/// <summary>
/// 统计月份区间解析
/// </summary>
public static class TaktStatMonthRangeHelper
{
    /// <summary>
    /// 解析统计月份区间（默认当月）
    /// </summary>
    /// <param name="dateStart">开始日期</param>
    /// <param name="dateEnd">结束日期</param>
    /// <returns>区间与统计月份 yyyy-MM</returns>
    public static (DateTime Start, DateTime End, string StatMonth) ResolveMonthRange(
        DateTime? dateStart,
        DateTime? dateEnd)
    {
        var now = DateTime.Now;
        var monthStart = new DateTime(now.Year, now.Month, 1);
        var monthEnd = monthStart.AddMonths(1).AddTicks(-1);
        var start = dateStart ?? monthStart;
        var end = dateEnd ?? monthEnd;
        return (start, end, start.ToString("yyyy-MM"));
    }
}
