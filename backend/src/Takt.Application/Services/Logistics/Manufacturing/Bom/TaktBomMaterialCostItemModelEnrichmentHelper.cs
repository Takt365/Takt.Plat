// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：TaktBomMaterialCostItemModelEnrichmentHelper.cs
// 创建时间：2026-07-13
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 物料成本机种元数据回填与机种月平均材料成本计算（纯函数）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities.Logistics.Manufacturing.Bom;

namespace Takt.Application.Services.Logistics.Manufacturing.Bom;

/// <summary>
/// BOM 物料成本机种 enrichment 计算辅助（无 I/O）
/// </summary>
public static class TaktBomMaterialCostItemModelEnrichmentHelper
{
    /// <summary>
    /// 计算机种指定月份平均材料成本（仅 ProductionRelated=X 且 PurchaseType=F；各成品有成本的参与算术平均）
    /// </summary>
    /// <param name="catalogProductCodes">成品编码清单（须与主表产品集一致，勿用型号目的地扩编）</param>
    /// <param name="monthRows">该工厂该月全部 BOM 行</param>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="periodKey">期间键 yyyy-MM</param>
    /// <returns>机种月成本；无有效成品成本时为 0</returns>
    public static decimal ComputeModelMonthlyAverageCost(
        IReadOnlyList<string> catalogProductCodes,
        IReadOnlyList<TaktBomMaterialCostItem> monthRows,
        string plantCode,
        string periodKey)
    {
        ArgumentNullException.ThrowIfNull(catalogProductCodes);
        ArgumentNullException.ThrowIfNull(monthRows);
        ArgumentException.ThrowIfNullOrWhiteSpace(plantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(periodKey);
        if (catalogProductCodes.Count == 0)
        {
            return 0m;
        }
        var productGroups = monthRows
            .Where(TaktBomMaterialCostItemLineCostHelper.CountsTowardBomMaterialCostItem)
            .GroupBy(r => r.ProductCode ?? string.Empty, StringComparer.OrdinalIgnoreCase)
            .Where(g => !string.IsNullOrWhiteSpace(g.Key))
            .ToDictionary(g => g.Key, g => g.ToList(), StringComparer.OrdinalIgnoreCase);
        var costs = new List<decimal>();
        foreach (var catalogProductCode in catalogProductCodes)
        {
            if (string.IsNullOrWhiteSpace(catalogProductCode))
            {
                continue;
            }
            foreach (var (productCode, rows) in productGroups)
            {
                if (!TaktBomMaterialCostItemLineCostHelper.ProductCodeMatches(productCode, catalogProductCode))
                {
                    continue;
                }
                var snapshot = TaktBomMaterialCostItemLineCostHelper.ResolvePeriodSnapshot(
                    rows, plantCode, productCode, periodKey);
                if (snapshot.Count == 0)
                {
                    break;
                }
                costs.Add(TaktBomMaterialCostItemLineCostHelper.SumSnapshotCost(snapshot));
                break;
            }
        }
        if (costs.Count == 0)
        {
            return 0m;
        }
        return TaktBomMaterialCostItemLineCostHelper.RoundCost(costs.Sum() / costs.Count);
    }

    /// <summary>
    /// 按主表已落库的产品月成本计算机种月平均（仅 ProductMonthlyCost &gt; 0 参与）
    /// </summary>
    /// <param name="productMonthlyCosts">同工厂+机种+核算期间下各成品产品月成本</param>
    /// <returns>机种月成本</returns>
    public static decimal ComputeModelMonthlyAverageFromProductCosts(IReadOnlyList<decimal> productMonthlyCosts)
    {
        ArgumentNullException.ThrowIfNull(productMonthlyCosts);
        var costs = productMonthlyCosts.Where(c => c > 0m).ToList();
        if (costs.Count == 0)
        {
            return 0m;
        }
        return TaktBomMaterialCostItemLineCostHelper.RoundCost(costs.Sum() / costs.Count);
    }
}
