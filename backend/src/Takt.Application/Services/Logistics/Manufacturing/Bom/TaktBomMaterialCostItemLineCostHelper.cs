// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：TaktBomMaterialCostItemLineCostHelper.cs
// 创建时间：2026-07-13
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 物料成本行金额与期间快照纯计算辅助；汇总与价格对比仅 ProductionRelated=X 且 PurchaseType=F，行成本=组件数量×(移动平均价÷移动价格单位) 保留 5 位小数（CK40N 口径）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities.Logistics.Manufacturing.Bom;
using Takt.Domain.Entities.Logistics.Materials;
using Takt.Shared.Helpers;

namespace Takt.Application.Services.Logistics.Manufacturing.Bom;

/// <summary>
/// BOM 物料成本行金额计算（无状态纯函数）
/// </summary>
public static class TaktBomMaterialCostItemLineCostHelper
{
    /// <summary>
    /// 成品物料类型（字典 logistics_material_type；成本合计/机种月均仅统计此类型）
    /// </summary>
    public const string FertMaterialTypeCode = "FERT";

    /// <summary>
    /// 成本/单价金额小数位数（行成本、产品月成本、机种月均、差异金额等统一口径）
    /// </summary>
    public const int CostDecimalDigits = 5;

    /// <summary>
    /// 环比百分点小数位数（界面/API：-0.34 表示 -0.34%）
    /// </summary>
    public const int PercentPointsDecimalDigits = 2;

    /// <summary>
    /// Excel 百分比单元格小数位数（÷100 后保留 4 位，匹配 0.00% 两位百分点）
    /// </summary>
    public const int ExcelPercentDecimalDigits = 4;

    /// <summary>
    /// 金额四舍五入（AwayFromZero，保留 CostDecimalDigits 位）
    /// </summary>
    /// <param name="value">原始金额</param>
    /// <returns>舍入后金额</returns>
    public static decimal RoundCost(decimal value)
    {
        return Math.Round(value, CostDecimalDigits, MidpointRounding.AwayFromZero);
    }

    /// <summary>
    /// 将变动比率转为百分点（×100，保留两位）
    /// </summary>
    /// <param name="ratio">差额÷基准（如 -0.0034）</param>
    /// <returns>百分点（如 -0.34）</returns>
    public static decimal RoundPercentPoints(decimal ratio)
    {
        return Math.Round(ratio * 100m, PercentPointsDecimalDigits, MidpointRounding.AwayFromZero);
    }

    /// <summary>
    /// 百分点 → Excel 百分比小数（÷100）；Excel 设为百分比格式后显示与界面一致
    /// </summary>
    /// <param name="percentPoints">百分点（如 -0.34）</param>
    /// <returns>Excel 用小数（如 -0.0034）；空则 null</returns>
    public static decimal? ToExcelPercent(decimal? percentPoints)
    {
        if (!percentPoints.HasValue)
        {
            return null;
        }
        return Math.Round(
            percentPoints.Value / 100m,
            ExcelPercentDecimalDigits,
            MidpointRounding.AwayFromZero);
    }

    /// <summary>
    /// 规范化核算日期：仅保留日历日，Kind=Unspecified（UTC 先转本地再取日，避免串月）
    /// </summary>
    /// <param name="costingDate">原始核算日期</param>
    /// <returns>日期部分（00:00:00 Unspecified）</returns>
    public static DateTime NormalizeCostingDate(DateTime costingDate)
    {
        var local = costingDate.Kind == DateTimeKind.Utc
            ? costingDate.ToLocalTime()
            : costingDate;
        return DateTime.SpecifyKind(local.Date, DateTimeKind.Unspecified);
    }

    /// <summary>
    /// 将核算日期映射为期间键 yyyy-MM（基于规范化后的日历日）
    /// </summary>
    /// <param name="costingDate">核算日期</param>
    /// <returns>期间键</returns>
    public static string ToPeriodKey(DateTime costingDate)
    {
        var d = NormalizeCostingDate(costingDate);
        return $"{d.Year:D4}-{d.Month:D2}";
    }

    /// <summary>
    /// 是否参与 BOM 材料成本汇总（生产相关=X 且采购类型=F）
    /// </summary>
    /// <param name="row">成本行</param>
    /// <returns>是否计入产品 BOM 材料成本</returns>
    public static bool CountsTowardBomMaterialCostItem(TaktBomMaterialCostItem row)
    {
        ArgumentNullException.ThrowIfNull(row);
        return string.Equals(row.ProductionRelated, "X", StringComparison.OrdinalIgnoreCase)
            && string.Equals(row.PurchaseType, "F", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 筛选参与 BOM 材料成本汇总的行（ProductionRelated=X 且 PurchaseType=F）
    /// </summary>
    /// <param name="rows">成本行</param>
    /// <returns>生产相关且外部采购行</returns>
    public static IEnumerable<TaktBomMaterialCostItem> FilterBomMaterialCostItemRows(IEnumerable<TaktBomMaterialCostItem> rows)
    {
        ArgumentNullException.ThrowIfNull(rows);
        return rows.Where(CountsTowardBomMaterialCostItem);
    }

    /// <summary>
    /// 计算单行组件成本（仅生产相关=X 且 F：组件数量×(移动平均价÷移动价格单位)，保留 5 位小数；否则返回 0）
    /// </summary>
    /// <param name="row">成本行</param>
    /// <returns>行成本</returns>
    public static decimal CalculateLineCost(TaktBomMaterialCostItem row)
    {
        ArgumentNullException.ThrowIfNull(row);
        if (!CountsTowardBomMaterialCostItem(row))
        {
            return 0m;
        }
        var priceUnit = ResolveMovingPriceUnit(row);
        var unitPrice = row.MovingAveragePrice / priceUnit;
        return RoundCost(row.ComponentQuantity * unitPrice);
    }

    /// <summary>
    /// 取有效单价（汇总口径仅生产相关=X 且 F：移动平均价，保留 5 位小数；否则返回 0）
    /// </summary>
    /// <param name="row">成本行</param>
    /// <returns>单价</returns>
    public static decimal ResolveEffectiveUnitPrice(TaktBomMaterialCostItem row)
    {
        ArgumentNullException.ThrowIfNull(row);
        if (!CountsTowardBomMaterialCostItem(row))
        {
            return 0m;
        }
        return RoundCost(row.MovingAveragePrice);
    }

    /// <summary>
    /// 取价格单位（汇总口径仅生产相关=X 且 F：移动价格单位，与移动平均价配对）
    /// </summary>
    /// <param name="row">成本行</param>
    /// <returns>价格单位</returns>
    public static int ResolvePriceUnit(TaktBomMaterialCostItem row)
    {
        ArgumentNullException.ThrowIfNull(row);
        if (!CountsTowardBomMaterialCostItem(row))
        {
            return 1;
        }
        return ResolveMovingPriceUnit(row);
    }

    /// <summary>
    /// 取移动价格单位（≤0 时按 1 处理）
    /// </summary>
    /// <param name="row">成本行</param>
    /// <returns>移动价格单位</returns>
    public static int ResolveMovingPriceUnit(TaktBomMaterialCostItem row)
    {
        ArgumentNullException.ThrowIfNull(row);
        return row.MovingPriceUnit <= 0 ? 1 : row.MovingPriceUnit;
    }

    /// <summary>
    /// 取货币码（汇总口径仅生产相关=X 且 F：移动价格货币）
    /// </summary>
    /// <param name="row">成本行</param>
    /// <returns>货币码</returns>
    public static string ResolveCurrency(TaktBomMaterialCostItem row)
    {
        ArgumentNullException.ThrowIfNull(row);
        if (!CountsTowardBomMaterialCostItem(row))
        {
            return string.Empty;
        }
        return row.MovingPriceCurrency ?? string.Empty;
    }

    /// <summary>
    /// 组件行业务键（对齐表唯一键，不含 CostingDate）：Plant+Product+Sequence+BomLevel+BomItem+Component+Quantity+Batch+ProductionRelated+PurchaseType+SpecialProcurement
    /// </summary>
    /// <param name="row">成本行</param>
    /// <returns>组件键</returns>
    public static string BuildComponentKey(TaktBomMaterialCostItem row)
    {
        ArgumentNullException.ThrowIfNull(row);
        return string.Join(
            "|",
            row.PlantCode?.Trim() ?? string.Empty,
            row.ProductCode?.Trim() ?? string.Empty,
            row.SequenceNo?.Trim() ?? string.Empty,
            row.BomLevel?.Trim() ?? string.Empty,
            row.BomItemNo?.Trim() ?? string.Empty,
            row.ComponentCode?.Trim() ?? string.Empty,
            row.ComponentQuantity.ToString("0.##########", System.Globalization.CultureInfo.InvariantCulture),
            row.BatchIndicator?.Trim() ?? string.Empty,
            row.ProductionRelated?.Trim() ?? string.Empty,
            row.PurchaseType?.Trim() ?? string.Empty,
            row.SpecialProcurementType?.Trim() ?? string.Empty);
    }

    /// <summary>
    /// 取产品在指定工厂、指定核算日当天的生产相关外部采购（X+F）快照行（CostingDate 须与明细一致）
    /// </summary>
    /// <param name="rows">候选行</param>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="productCode">产品编码</param>
    /// <param name="costingDate">核算日（与明细 CostingDate 对齐）</param>
    /// <returns>快照行列表</returns>
    public static List<TaktBomMaterialCostItem> ResolveDateSnapshot(
        IEnumerable<TaktBomMaterialCostItem> rows,
        string plantCode,
        string productCode,
        DateTime costingDate)
    {
        ArgumentNullException.ThrowIfNull(rows);
        ArgumentException.ThrowIfNullOrWhiteSpace(plantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(productCode);
        var day = NormalizeCostingDate(costingDate);
        return rows
            .Where(r => string.Equals(r.PlantCode, plantCode, StringComparison.OrdinalIgnoreCase)
                && ProductCodeMatches(r.ProductCode, productCode)
                && NormalizeCostingDate(r.CostingDate) == day)
            .Where(CountsTowardBomMaterialCostItem)
            .GroupBy(BuildComponentKey, StringComparer.Ordinal)
            .Select(g => g.OrderByDescending(r => r.Id).First())
            .ToList();
    }

    /// <summary>
    /// 取产品在指定工厂、指定期间内「最后核算日」当天的生产相关外部采购（X+F）快照行（同月多日只取最后一日整单）
    /// </summary>
    /// <param name="rows">候选行</param>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="productCode">产品编码</param>
    /// <param name="periodKey">期间键 yyyy-MM</param>
    /// <returns>快照行列表</returns>
    public static List<TaktBomMaterialCostItem> ResolvePeriodSnapshot(
        IEnumerable<TaktBomMaterialCostItem> rows,
        string plantCode,
        string productCode,
        string periodKey)
    {
        ArgumentNullException.ThrowIfNull(rows);
        ArgumentException.ThrowIfNullOrWhiteSpace(plantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(productCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(periodKey);
        var latestCostingDate = ResolveLatestCostingDate(rows, plantCode, productCode, periodKey);
        if (latestCostingDate == null)
        {
            return new List<TaktBomMaterialCostItem>();
        }
        return ResolveDateSnapshot(rows, plantCode, productCode, latestCostingDate.Value);
    }

    /// <summary>
    /// 取期间内最后核算日（规范化后的日历日；无数据时返回 null）
    /// </summary>
    /// <param name="rows">候选行</param>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="productCode">产品编码</param>
    /// <param name="periodKey">期间键 yyyy-MM</param>
    /// <returns>最后核算日</returns>
    public static DateTime? ResolveLatestCostingDate(
        IEnumerable<TaktBomMaterialCostItem> rows,
        string plantCode,
        string productCode,
        string periodKey)
    {
        ArgumentNullException.ThrowIfNull(rows);
        ArgumentException.ThrowIfNullOrWhiteSpace(plantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(productCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(periodKey);
        var periodRows = rows
            .Where(r => string.Equals(r.PlantCode, plantCode, StringComparison.OrdinalIgnoreCase)
                && ProductCodeMatches(r.ProductCode, productCode)
                && ToPeriodKey(r.CostingDate) == periodKey)
            .ToList();
        if (periodRows.Count == 0)
        {
            return null;
        }
        return periodRows.Max(r => NormalizeCostingDate(r.CostingDate));
    }

    /// <summary>
    /// 核算期间起止（含首尾）
    /// </summary>
    /// <param name="periodKey">期间键 yyyy-MM</param>
    /// <returns>起止日期</returns>
    public static (DateTime Start, DateTime End) ResolvePeriodDateRange(string periodKey)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(periodKey);
        if (!DateTime.TryParseExact(
                periodKey + "-01",
                "yyyy-MM-dd",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out var start))
        {
            throw new ArgumentException("期间键须为 yyyy-MM", nameof(periodKey));
        }
        start = DateTime.SpecifyKind(start, DateTimeKind.Unspecified);
        var end = start.AddMonths(1).AddTicks(-1);
        return (start, end);
    }

    /// <summary>
    /// 汇总产品 BOM 材料成本（仅生产相关=X 且 F 行求和，结果保留 5 位小数）
    /// </summary>
    /// <param name="snapshotRows">快照行</param>
    /// <returns>总成本</returns>
    public static decimal SumSnapshotCost(IEnumerable<TaktBomMaterialCostItem> snapshotRows)
    {
        ArgumentNullException.ThrowIfNull(snapshotRows);
        return RoundCost(FilterBomMaterialCostItemRows(snapshotRows).Sum(CalculateLineCost));
    }

    /// <summary>
    /// 产品编码是否匹配（18 位 SAP 数字码与 10 位存储码互认）
    /// </summary>
    /// <param name="storedProductCode">库内产品编码</param>
    /// <param name="requestedProductCode">查询产品编码</param>
    /// <returns>是否同一产品</returns>
    public static bool ProductCodeMatches(string? storedProductCode, string requestedProductCode)
    {
        if (string.IsNullOrWhiteSpace(storedProductCode) || string.IsNullOrWhiteSpace(requestedProductCode))
        {
            return false;
        }
        var requested = requestedProductCode.Trim();
        var stored = storedProductCode.Trim();
        if (string.Equals(stored, requested, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }
        var requestedNormalized = TaktStringHelper.NormalizeSapNumericMaterialCode(requested);
        var storedNormalized = TaktStringHelper.NormalizeSapNumericMaterialCode(stored);
        return string.Equals(storedNormalized, requestedNormalized, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 展开产品编码查询变体（原码 / 归一化 10 位 / 18 位零填充），供 Contains 查询命中明细表
    /// </summary>
    /// <param name="productCode">产品编码</param>
    /// <returns>去重后的查询码列表</returns>
    public static IReadOnlyList<string> ExpandProductCodeLookupVariants(string? productCode)
    {
        if (string.IsNullOrWhiteSpace(productCode))
        {
            return Array.Empty<string>();
        }
        var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var trimmed = productCode.Trim();
        set.Add(trimmed);
        var normalized = TaktStringHelper.NormalizeSapNumericMaterialCode(trimmed);
        if (!string.IsNullOrEmpty(normalized))
        {
            set.Add(normalized);
        }
        if (IsTenDigitNumeric(trimmed))
        {
            set.Add(trimmed.PadLeft(18, '0'));
        }
        if (!string.IsNullOrEmpty(normalized)
            && !string.Equals(normalized, trimmed, StringComparison.OrdinalIgnoreCase)
            && IsTenDigitNumeric(normalized))
        {
            set.Add(normalized.PadLeft(18, '0'));
        }
        return set.ToList();
    }

    /// <summary>
    /// 是否 10 位纯数字物料码
    /// </summary>
    /// <param name="code">编码</param>
    /// <returns>是否 10 位数字</returns>
    private static bool IsTenDigitNumeric(string code)
    {
        if (code.Length != 10)
        {
            return false;
        }
        for (var i = 0; i < code.Length; i++)
        {
            if (!char.IsDigit(code[i]))
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 序号排序键（0010→10；非数字用 int.MaxValue 靠后）
    /// </summary>
    /// <param name="sequenceNo">序号</param>
    /// <returns>数值键</returns>
    public static int ParseSequenceSortKey(string? sequenceNo)
    {
        if (string.IsNullOrWhiteSpace(sequenceNo))
        {
            return int.MaxValue;
        }
        var trimmed = sequenceNo.Trim();
        return int.TryParse(trimmed, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out var n)
            ? n
            : int.MaxValue;
    }

    /// <summary>
    /// BOM 层级前导点数（1→0，.1→1，..2→2；与展开显示 1,2,…,.1,..2 对齐）
    /// </summary>
    /// <param name="bomLevel">层级</param>
    /// <returns>前导点数</returns>
    public static int ParseBomLevelDotDepth(string? bomLevel)
    {
        if (string.IsNullOrWhiteSpace(bomLevel))
        {
            return int.MaxValue;
        }
        var s = bomLevel.Trim();
        var depth = 0;
        while (depth < s.Length && s[depth] == '.')
        {
            depth++;
        }
        return depth;
    }

    /// <summary>
    /// BOM 层级去掉前导点后的数字段（.1→1，..2→2，01→1；非数字用 int.MaxValue）
    /// </summary>
    /// <param name="bomLevel">层级</param>
    /// <returns>数字键</returns>
    public static int ParseBomLevelNumericPart(string? bomLevel)
    {
        if (string.IsNullOrWhiteSpace(bomLevel))
        {
            return int.MaxValue;
        }
        var s = bomLevel.Trim();
        var i = 0;
        while (i < s.Length && s[i] == '.')
        {
            i++;
        }
        if (i >= s.Length)
        {
            return int.MaxValue;
        }
        // 兼容 01/02：取首段数字
        var end = i;
        while (end < s.Length && char.IsDigit(s[end]))
        {
            end++;
        }
        if (end == i)
        {
            return int.MaxValue;
        }
        return int.TryParse(
            s.AsSpan(i, end - i),
            System.Globalization.NumberStyles.Integer,
            System.Globalization.CultureInfo.InvariantCulture,
            out var n)
            ? n
            : int.MaxValue;
    }

    /// <summary>
    /// 产品成本明细展开序：ProductCode → SequenceNo → BomLevel（深度再数字，如 1…6 后 .1、..2）
    /// </summary>
    /// <param name="productCodeA">产品 A</param>
    /// <param name="sequenceNoA">序号 A</param>
    /// <param name="bomLevelA">层级 A</param>
    /// <param name="productCodeB">产品 B</param>
    /// <param name="sequenceNoB">序号 B</param>
    /// <param name="bomLevelB">层级 B</param>
    /// <returns>比较结果</returns>
    public static int CompareBomExplosionOrder(
        string? productCodeA,
        string? sequenceNoA,
        string? bomLevelA,
        string? productCodeB,
        string? sequenceNoB,
        string? bomLevelB)
    {
        var productCmp = string.Compare(
            productCodeA?.Trim() ?? string.Empty,
            productCodeB?.Trim() ?? string.Empty,
            StringComparison.OrdinalIgnoreCase);
        if (productCmp != 0)
        {
            return productCmp;
        }
        var seqCmp = ParseSequenceSortKey(sequenceNoA).CompareTo(ParseSequenceSortKey(sequenceNoB));
        if (seqCmp != 0)
        {
            return seqCmp;
        }
        var seqTextCmp = string.Compare(
            sequenceNoA?.Trim() ?? string.Empty,
            sequenceNoB?.Trim() ?? string.Empty,
            StringComparison.Ordinal);
        if (seqTextCmp != 0)
        {
            return seqTextCmp;
        }
        var depthCmp = ParseBomLevelDotDepth(bomLevelA).CompareTo(ParseBomLevelDotDepth(bomLevelB));
        if (depthCmp != 0)
        {
            return depthCmp;
        }
        var levelNumCmp = ParseBomLevelNumericPart(bomLevelA).CompareTo(ParseBomLevelNumericPart(bomLevelB));
        if (levelNumCmp != 0)
        {
            return levelNumCmp;
        }
        return string.Compare(
            bomLevelA?.Trim() ?? string.Empty,
            bomLevelB?.Trim() ?? string.Empty,
            StringComparison.Ordinal);
    }

    /// <summary>
    /// 是否成品物料类型 FERT
    /// </summary>
    /// <param name="materialType">物料类型</param>
    /// <returns>是否 FERT</returns>
    public static bool IsFertMaterialType(string? materialType)
    {
        return string.Equals(
            materialType?.Trim(),
            FertMaterialTypeCode,
            StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 工厂物料是否为可参与成本合计的成品（同工厂 + FERT + 物料编码匹配产品）
    /// </summary>
    /// <param name="materialPlants">工厂物料行（建议已过滤为本厂范围）</param>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="productCode">产品编码</param>
    /// <returns>是否参与统计</returns>
    public static bool IsFertPlantProduct(
        IReadOnlyList<TaktMaterialPlant> materialPlants,
        string plantCode,
        string productCode)
    {
        ArgumentNullException.ThrowIfNull(materialPlants);
        if (string.IsNullOrWhiteSpace(plantCode) || string.IsNullOrWhiteSpace(productCode))
        {
            return false;
        }
        var plant = plantCode.Trim();
        var product = productCode.Trim();
        foreach (var row in materialPlants)
        {
            if (!string.Equals(row.PlantCode, plant, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }
            if (!IsFertMaterialType(row.MaterialType))
            {
                continue;
            }
            if (ProductCodeMatches(row.MaterialCode, product))
            {
                return true;
            }
        }
        return false;
    }
}
