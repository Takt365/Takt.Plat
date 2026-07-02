// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktDefectStatHelper.cs
// 创建时间：2026-07-01
// 创建人：Takt365(Cursor AI)
// 功能描述：不良统计不良率/直行率计算
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Helpers;

/// <summary>
/// 不良统计辅助（不良率 = 不良数量 / 分母 × 100%；直行率 = 良品 / 分母 × 100%）
/// </summary>
public static class TaktDefectStatHelper
{
    /// <summary>
    /// 计算不良率（百分比，保留 2 位小数；分母为 0 时返回 0）
    /// </summary>
    /// <param name="defectQty">不良数量</param>
    /// <param name="baseQty">统计分母</param>
    /// <returns>不良率(%)</returns>
    public static decimal CalculateDefectRatePercent(decimal defectQty, decimal baseQty)
    {
        if (baseQty <= 0)
        {
            return 0;
        }
        var rate = defectQty / baseQty * 100m;
        return Math.Round(rate, 2, MidpointRounding.AwayFromZero);
    }

    /// <summary>
    /// 计算直行率（百分比，保留 2 位小数；分母为 0 时返回 0）
    /// </summary>
    /// <param name="goodQty">良品/无不良数量</param>
    /// <param name="baseQty">统计分母</param>
    /// <returns>直行率(%)</returns>
    public static decimal CalculateYieldRatePercent(decimal goodQty, decimal baseQty)
    {
        if (baseQty <= 0)
        {
            return 0;
        }
        var rate = goodQty / baseQty * 100m;
        return Math.Round(rate, 2, MidpointRounding.AwayFromZero);
    }
}
