// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Sales
// 文件名称：TaktSalesMonthlyTrendService.cs
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：月销售推移转置分析服务实现（TaktSalesOrder.OrderDate × ActualAmount）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Takt.Application.Dtos.Logistics.Sales;
using Takt.Domain.Entities.Logistics.Sales;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Sales;

/// <summary>
/// 月销售推移转置分析服务（读销售订单本表；与 CRUD 服务分离）
/// </summary>
public class TaktSalesMonthlyTrendService : TaktServiceBase, ITaktSalesMonthlyTrendService
{
    private const decimal CentsPerYuan = 100m;

    private readonly ITaktCompanyRepository<TaktSalesOrder> _salesOrderRepository;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="salesOrderRepository">销售订单仓储</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSalesMonthlyTrendService(
        ITaktCompanyRepository<TaktSalesOrder> salesOrderRepository,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _salesOrderRepository = salesOrderRepository;
    }

    /// <inheritdoc />
    public async Task<List<TaktSelectOption>> GetSalesMonthlyTrendPlantOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _salesOrderRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode != null
                && x.PlantCode != string.Empty);
        return list
            .GroupBy(e => e.PlantCode.Trim(), StringComparer.OrdinalIgnoreCase)
            .OrderBy(g => g.Key, StringComparer.Ordinal)
            .Select(g => new TaktSelectOption
            {
                DictValue = g.Key,
                DictLabel = g.Key,
            })
            .ToList();
    }

    /// <inheritdoc />
    public async Task<List<TaktSelectOption>> GetSalesMonthlyTrendCustomerOptionsAsync(string plantCode)
    {
        EnsureThreeLayerContext();
        var plant = plantCode?.Trim() ?? string.Empty;
        if (string.IsNullOrEmpty(plant))
        {
            return new List<TaktSelectOption>();
        }
        var list = await _salesOrderRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode == plant
                && x.CustomerCode != null
                && x.CustomerCode != string.Empty);
        return list
            .GroupBy(e => e.CustomerCode.Trim(), StringComparer.OrdinalIgnoreCase)
            .OrderBy(g => g.Key, StringComparer.Ordinal)
            .Select(g =>
            {
                var name = g.Select(x => x.CustomerName1)
                    .FirstOrDefault(n => !string.IsNullOrWhiteSpace(n))?.Trim();
                var label = string.IsNullOrWhiteSpace(name) ? g.Key : $"{g.Key} - {name}";
                return new TaktSelectOption
                {
                    DictValue = g.Key,
                    DictLabel = label,
                };
            })
            .ToList();
    }

    /// <inheritdoc />
    public async Task<TaktSalesMonthlyTrendResultDto> GetSalesMonthlyTrendAnalysisAsync(
        TaktSalesMonthlyTrendQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        var pageIndex = TaktPagedClamp.NormalizePageIndex(queryDto.PageIndex);
        var pageSize = TaktPagedClamp.NormalizePageSize(queryDto.PageSize);
        var skip = TaktPagedClamp.ComputeSkip(pageIndex, pageSize);
        var built = await BuildSalesMonthlyTrendAnalysisAsync(queryDto);
        var pageRows = built.OrderedRows.Skip(skip).Take(pageSize).ToList();
        return new TaktSalesMonthlyTrendResultDto
        {
            Paged = TaktPagedResult<TaktSalesMonthlyTrendDto>.Create(
                pageRows, built.OrderedRows.Count, pageIndex, pageSize),
            PeriodOrder = built.PeriodOrder,
            CustomerCount = built.OrderedRows.Count,
            BasePeriod = pageRows.FirstOrDefault()?.BasePeriod ?? built.BasePeriod,
            ComparePeriod = built.ComparePeriod,
            UpCount = built.UpCount,
            DownCount = built.DownCount,
            FlatCount = built.FlatCount,
            NoneCount = built.NoneCount,
        };
    }

    /// <inheritdoc />
    public async Task<(string fileName, byte[] fileContent)> ExportSalesMonthlyTrendAnalysisAsync(
        TaktSalesMonthlyTrendQueryDto query,
        string? sheetName = null,
        string? fileName = null)
    {
        ArgumentNullException.ThrowIfNull(query);
        var built = await BuildSalesMonthlyTrendAnalysisAsync(query);
        var columnKeys = new List<string> { "plantCode", "customerCode", "customerName" };
        var columnLabels = new List<string> { "工厂代码", "客户编码", "客户名称" };
        foreach (var period in built.PeriodOrder)
        {
            columnKeys.Add($"period_{period}");
            columnLabels.Add(period);
        }
        columnKeys.AddRange(new[] { "basePeriod", "comparePeriod", "varianceAmount", "variancePercent", "trend" });
        columnLabels.AddRange(new[] { "基准月", "对比月", "环比差额", "环比%", "涨跌" });
        var exportRows = built.OrderedRows.Select(row =>
        {
            var dict = new Dictionary<string, object?>(StringComparer.Ordinal)
            {
                ["plantCode"] = row.PlantCode,
                ["customerCode"] = row.CustomerCode,
                ["customerName"] = row.CustomerName,
                ["basePeriod"] = row.BasePeriod,
                ["comparePeriod"] = row.ComparePeriod,
                ["varianceAmount"] = row.VarianceAmount,
                ["variancePercent"] = row.VariancePercent.HasValue
                    ? Math.Round(row.VariancePercent.Value, 4, MidpointRounding.AwayFromZero)
                    : null,
                ["trend"] = row.Trend,
            };
            foreach (var period in built.PeriodOrder)
            {
                dict[$"period_{period}"] = row.PeriodAmounts.TryGetValue(period, out var amount)
                    ? amount
                    : null;
            }
            return (IReadOnlyDictionary<string, object?>)dict;
        }).ToList();
        return await TaktExcelHelper.ExportDictionaryRowsAsync(
            exportRows,
            columnKeys,
            columnLabels,
            sheetName ?? "月销售推移表",
            fileName ?? "月销售推移表.xlsx");
    }

    /// <summary>
    /// 构建月销售推移转置分析全量结果
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>内存构建结果</returns>
    private async Task<SalesMonthlyTrendAnalysisBuilt> BuildSalesMonthlyTrendAnalysisAsync(
        TaktSalesMonthlyTrendQueryDto queryDto)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.PlantCode);
        EnsureThreeLayerContext();
        var plantCode = queryDto.PlantCode.Trim();
        var periodOrder = ResolvePeriodOrder(queryDto, out var rangeStart, out var rangeEnd);
        if (periodOrder.Count == 0)
        {
            return SalesMonthlyTrendAnalysisBuilt.Empty();
        }
        var focusPeriod = ResolveFocusPeriod(queryDto.FocusPeriod, periodOrder);
        var rangeEndExclusive = rangeEnd.AddMonths(1);
        var orders = await _salesOrderRepository.GetListAsync(
            BuildSalesOrderTrendExpression(plantCode, queryDto.CustomerCode, rangeStart, rangeEndExclusive));
        if (orders.Count == 0)
        {
            return SalesMonthlyTrendAnalysisBuilt.Empty();
        }
        var periodSet = new HashSet<string>(periodOrder, StringComparer.Ordinal);
        var allRows = orders
            .GroupBy(
                o => new SalesMonthlyTrendRowKey(o.PlantCode.Trim(), o.CustomerCode.Trim()),
                SalesMonthlyTrendRowKeyComparer.Instance)
            .Select(g => BuildSalesMonthlyTrendRow(g.Key, g.ToList(), periodSet, focusPeriod))
            .ToList();
        var filtered = FilterTrendRows(allRows, queryDto.TrendFilter);
        var ordered = OrderTrendRows(filtered);
        return new SalesMonthlyTrendAnalysisBuilt
        {
            OrderedRows = ordered,
            PeriodOrder = periodOrder,
            BasePeriod = allRows.FirstOrDefault()?.BasePeriod,
            ComparePeriod = focusPeriod,
            UpCount = allRows.Count(r => r.Trend == "up"),
            DownCount = allRows.Count(r => r.Trend == "down"),
            FlatCount = allRows.Count(r => r.Trend == "flat"),
            NoneCount = allRows.Count(r => r.Trend == "none"),
        };
    }

    /// <summary>
    /// 构建销售订单推移筛选条件
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="customerCode">客户编码</param>
    /// <param name="rangeStart">期间起</param>
    /// <param name="rangeEndExclusive">期间止（不含）</param>
    /// <returns>表达式</returns>
    private Expression<Func<TaktSalesOrder, bool>> BuildSalesOrderTrendExpression(
        string plantCode,
        string? customerCode,
        DateTime rangeStart,
        DateTime rangeEndExclusive)
    {
        var exp = Expressionable.Create<TaktSalesOrder>();
        exp = exp.And(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.PlantCode == plantCode
            && x.OrderDate >= rangeStart
            && x.OrderDate < rangeEndExclusive);
        if (!string.IsNullOrWhiteSpace(customerCode))
        {
            var code = customerCode.Trim();
            exp = exp.And(x => x.CustomerCode == code);
        }
        return exp.ToExpression();
    }

    /// <summary>
    /// 解析期间列顺序
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <param name="rangeStart">区间起</param>
    /// <param name="rangeEnd">区间止（月初）</param>
    /// <returns>期间列顺序</returns>
    private static List<string> ResolvePeriodOrder(
        TaktSalesMonthlyTrendQueryDto queryDto,
        out DateTime rangeStart,
        out DateTime rangeEnd)
    {
        var (periodStart, periodEnd) = NormalizePeriodBounds(queryDto.PeriodDateStart, queryDto.PeriodDateEnd);
        if (periodStart.HasValue || periodEnd.HasValue)
        {
            var startMonth = periodStart ?? periodEnd!.Value;
            var endMonth = periodEnd ?? periodStart!.Value;
            if (startMonth > endMonth)
            {
                (startMonth, endMonth) = (endMonth, startMonth);
            }
            var monthCount = ((endMonth.Year - startMonth.Year) * 12) + endMonth.Month - startMonth.Month + 1;
            if (monthCount > TaktPriceTrendAnalysisHelper.MaxTrendMonths)
            {
                throw new ArgumentException($"分析区间不得超过 {TaktPriceTrendAnalysisHelper.MaxTrendMonths} 个月");
            }
            rangeStart = startMonth;
            rangeEnd = endMonth;
            return BuildConsecutivePeriodOrder(startMonth, endMonth);
        }
        var (resolvedStart, resolvedEnd) = TaktPriceTrendAnalysisHelper.ResolveTrendDateRange(null, null);
        rangeStart = new DateTime(resolvedStart.Year, resolvedStart.Month, 1);
        rangeEnd = new DateTime(resolvedEnd.Year, resolvedEnd.Month, 1);
        return BuildConsecutivePeriodOrder(rangeStart, rangeEnd);
    }

    /// <summary>
    /// 归一化期间起止到月初
    /// </summary>
    /// <param name="periodDateStart">期间起</param>
    /// <param name="periodDateEnd">期间止</param>
    /// <returns>月初起止</returns>
    private static (DateTime? Start, DateTime? End) NormalizePeriodBounds(
        DateTime? periodDateStart,
        DateTime? periodDateEnd)
    {
        DateTime? start = periodDateStart.HasValue
            ? new DateTime(periodDateStart.Value.Year, periodDateStart.Value.Month, 1)
            : null;
        DateTime? end = periodDateEnd.HasValue
            ? new DateTime(periodDateEnd.Value.Year, periodDateEnd.Value.Month, 1)
            : null;
        if (start.HasValue && end.HasValue && start > end)
        {
            (start, end) = (end, start);
        }
        return (start, end);
    }

    /// <summary>
    /// 构建连续 yyyy-MM 期间列
    /// </summary>
    /// <param name="periodStart">期间起（月初）</param>
    /// <param name="periodEnd">期间止（月初）</param>
    /// <returns>期间列顺序</returns>
    private static List<string> BuildConsecutivePeriodOrder(DateTime periodStart, DateTime periodEnd)
    {
        var order = new List<string>();
        for (var cursor = periodStart; cursor <= periodEnd; cursor = cursor.AddMonths(1))
        {
            order.Add(cursor.ToString("yyyy-MM"));
        }
        return order;
    }

    /// <summary>
    /// 解析关注期间
    /// </summary>
    /// <param name="focusPeriod">关注期间</param>
    /// <param name="periodOrder">期间列</param>
    /// <returns>关注期间 yyyy-MM</returns>
    private static string? ResolveFocusPeriod(string? focusPeriod, IReadOnlyList<string> periodOrder)
    {
        if (!string.IsNullOrWhiteSpace(focusPeriod))
        {
            return focusPeriod.Trim();
        }
        return periodOrder.Count > 0 ? periodOrder[^1] : null;
    }

    /// <summary>
    /// 构建单行月销售推移
    /// </summary>
    /// <param name="key">行键</param>
    /// <param name="groupRows">同键订单</param>
    /// <param name="periodSet">展示期间集合</param>
    /// <param name="focusPeriod">关注期间</param>
    /// <returns>转置行</returns>
    private static TaktSalesMonthlyTrendDto BuildSalesMonthlyTrendRow(
        SalesMonthlyTrendRowKey key,
        IReadOnlyList<TaktSalesOrder> groupRows,
        IReadOnlySet<string> periodSet,
        string? focusPeriod)
    {
        var row = new TaktSalesMonthlyTrendDto
        {
            PlantCode = key.PlantCode,
            CustomerCode = key.CustomerCode,
            CustomerName = groupRows
                .Select(r => r.CustomerName1?.Trim())
                .FirstOrDefault(n => !string.IsNullOrWhiteSpace(n)) ?? string.Empty,
            Trend = "none",
        };
        foreach (var period in groupRows
                     .Select(r => new
                     {
                         Period = new DateTime(r.OrderDate.Year, r.OrderDate.Month, 1).ToString("yyyy-MM"),
                         AmountYuan = ToYuan(r.ActualAmount),
                     })
                     .Where(r => periodSet.Contains(r.Period))
                     .GroupBy(r => r.Period, StringComparer.Ordinal))
        {
            row.PeriodAmounts[period.Key] = RoundAmount(period.Sum(r => r.AmountYuan));
        }
        ApplyFocusTrend(row, focusPeriod);
        return row;
    }

    /// <summary>
    /// 分转元（订单 ActualAmount 存储单位为分）
    /// </summary>
    /// <param name="amountInCents">分</param>
    /// <returns>元</returns>
    private static decimal ToYuan(decimal amountInCents) => amountInCents / CentsPerYuan;

    /// <summary>
    /// 按关注月计算环比涨跌
    /// </summary>
    /// <param name="row">转置行</param>
    /// <param name="focusPeriod">关注期间 yyyy-MM</param>
    private static void ApplyFocusTrend(TaktSalesMonthlyTrendDto row, string? focusPeriod)
    {
        if (string.IsNullOrWhiteSpace(focusPeriod))
        {
            return;
        }
        var comparePeriod = focusPeriod.Trim();
        if (!DateTime.TryParseExact(
                comparePeriod + "-01",
                "yyyy-MM-dd",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out var compareMonth))
        {
            return;
        }
        var basePeriod = compareMonth.AddMonths(-1).ToString("yyyy-MM");
        row.BasePeriod = basePeriod;
        row.ComparePeriod = comparePeriod;
        if (!row.PeriodAmounts.TryGetValue(basePeriod, out var baseAmount)
            || !row.PeriodAmounts.TryGetValue(comparePeriod, out var compareAmount))
        {
            row.Trend = "none";
            return;
        }
        row.VarianceAmount = RoundAmount(compareAmount - baseAmount);
        if (baseAmount != 0m)
        {
            row.VariancePercent = Math.Round(
                row.VarianceAmount.Value / baseAmount,
                4,
                MidpointRounding.AwayFromZero);
        }
        if (compareAmount > baseAmount)
        {
            row.Trend = "up";
        }
        else if (compareAmount < baseAmount)
        {
            row.Trend = "down";
        }
        else
        {
            row.Trend = "flat";
        }
    }

    /// <summary>
    /// 涨跌筛选
    /// </summary>
    /// <param name="rows">全量行</param>
    /// <param name="trendFilter">筛选码</param>
    /// <returns>筛选后行</returns>
    private static List<TaktSalesMonthlyTrendDto> FilterTrendRows(
        IReadOnlyList<TaktSalesMonthlyTrendDto> rows,
        string? trendFilter)
    {
        if (string.IsNullOrWhiteSpace(trendFilter))
        {
            return rows.ToList();
        }
        var filter = trendFilter.Trim().ToLowerInvariant();
        if (filter == "changed")
        {
            return rows.Where(r => r.Trend is "up" or "down").ToList();
        }
        return rows.Where(r => string.Equals(r.Trend, filter, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    /// <summary>
    /// 涨跌优先排序
    /// </summary>
    /// <param name="rows">行集合</param>
    /// <returns>排序后行</returns>
    private static List<TaktSalesMonthlyTrendDto> OrderTrendRows(
        IReadOnlyList<TaktSalesMonthlyTrendDto> rows)
    {
        static int TrendRank(string? trend) => trend switch
        {
            "up" => 0,
            "down" => 1,
            "flat" => 2,
            _ => 3,
        };
        return rows
            .OrderBy(r => TrendRank(r.Trend))
            .ThenByDescending(r => Math.Abs(r.VarianceAmount ?? 0m))
            .ThenBy(r => r.CustomerCode, StringComparer.Ordinal)
            .ToList();
    }

    /// <summary>
    /// 金额四舍五入至 2 位
    /// </summary>
    /// <param name="value">金额（元）</param>
    /// <returns>四舍五入后金额</returns>
    private static decimal RoundAmount(decimal value) =>
        Math.Round(value, 2, MidpointRounding.AwayFromZero);

    /// <summary>
    /// 月销售推移行键
    /// </summary>
    /// <param name="PlantCode">工厂代码</param>
    /// <param name="CustomerCode">客户编码</param>
    private sealed record SalesMonthlyTrendRowKey(string PlantCode, string CustomerCode);

    /// <summary>
    /// 月销售推移行键比较器
    /// </summary>
    private sealed class SalesMonthlyTrendRowKeyComparer : IEqualityComparer<SalesMonthlyTrendRowKey>
    {
        /// <summary>单例</summary>
        public static SalesMonthlyTrendRowKeyComparer Instance { get; } = new();

        /// <inheritdoc />
        public bool Equals(SalesMonthlyTrendRowKey? x, SalesMonthlyTrendRowKey? y)
        {
            if (x is null || y is null)
            {
                return ReferenceEquals(x, y);
            }
            return string.Equals(x.PlantCode, y.PlantCode, StringComparison.OrdinalIgnoreCase)
                && string.Equals(x.CustomerCode, y.CustomerCode, StringComparison.OrdinalIgnoreCase);
        }

        /// <inheritdoc />
        public int GetHashCode(SalesMonthlyTrendRowKey obj) =>
            HashCode.Combine(
                obj.PlantCode.ToUpperInvariant(),
                obj.CustomerCode.ToUpperInvariant());
    }

    /// <summary>
    /// 月销售推移分析构建结果
    /// </summary>
    private sealed class SalesMonthlyTrendAnalysisBuilt
    {
        /// <summary>排序后全量行</summary>
        public List<TaktSalesMonthlyTrendDto> OrderedRows { get; init; } = new();

        /// <summary>期间列顺序</summary>
        public List<string> PeriodOrder { get; init; } = new();

        /// <summary>环比基准期间</summary>
        public string? BasePeriod { get; init; }

        /// <summary>环比对比期间</summary>
        public string? ComparePeriod { get; init; }

        /// <summary>上涨行数</summary>
        public int UpCount { get; init; }

        /// <summary>下跌行数</summary>
        public int DownCount { get; init; }

        /// <summary>持平行数</summary>
        public int FlatCount { get; init; }

        /// <summary>无法比较行数</summary>
        public int NoneCount { get; init; }

        /// <summary>
        /// 空结果
        /// </summary>
        /// <returns>空构建结果</returns>
        public static SalesMonthlyTrendAnalysisBuilt Empty() => new();
    }
}
