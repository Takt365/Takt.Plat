// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktProductionStatHelper.cs
// 创建时间：2026-06-29
// 创建人：Takt365(Cursor AI)
// 功能描述：生产统计达成率计算（MonthProdActualQty / MonthStdCapacity）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Helpers;

/// <summary>
/// 生产统计辅助（达成率 = 实际产量 / 标准产能 × 100%）
/// </summary>
public static class TaktProductionStatHelper
{
    /// <summary>
    /// 计算达成率（百分比，保留 2 位小数；标准产能为 0 时返回 0）
    /// </summary>
    /// <param name="prodActualQty">实际生产数量</param>
    /// <param name="stdCapacity">标准产能</param>
    /// <returns>达成率(%)</returns>
    public static decimal CalculateAchievementRatePercent(decimal prodActualQty, decimal stdCapacity)
    {
        if (stdCapacity <= 0)
        {
            return 0;
        }
        var rate = prodActualQty / stdCapacity * 100m;
        return Math.Round(rate, 2, MidpointRounding.AwayFromZero);
    }
}
