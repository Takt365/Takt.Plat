// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcMonthlyTrendService.cs
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：月设变推移转置分析服务实现（TaktEcGijutsu.EcIssueDate × 件数/损失金额）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 月设变推移转置分析服务
/// </summary>
public class TaktEcMonthlyTrendService : TaktServiceBase, ITaktEcMonthlyTrendService
{
    private const int CompletedTaskStatus = 2;

    private readonly ITaktCompanyRepository<TaktEcGijutsu> _ecGijutsuRepository;
    private readonly ITaktCompanyRepository<TaktEcExecutionTask> _ecExecutionTaskRepository;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ecGijutsuRepository">设变技术课仓储</param>
    /// <param name="ecExecutionTaskRepository">设变执行任务仓储</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktEcMonthlyTrendService(
        ITaktCompanyRepository<TaktEcGijutsu> ecGijutsuRepository,
        ITaktCompanyRepository<TaktEcExecutionTask> ecExecutionTaskRepository,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _ecGijutsuRepository = ecGijutsuRepository;
        _ecExecutionTaskRepository = ecExecutionTaskRepository;
    }

    /// <inheritdoc />
    public async Task<TaktEcMonthlyTrendResultDto> GetEcMonthlyTrendAnalysisAsync(
        TaktEcMonthlyTrendQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        var pageIndex = TaktPagedClamp.NormalizePageIndex(queryDto.PageIndex);
        var pageSize = TaktPagedClamp.NormalizePageSize(queryDto.PageSize);
        var skip = TaktPagedClamp.ComputeSkip(pageIndex, pageSize);
        var built = await BuildEcMonthlyTrendAnalysisAsync(queryDto);
        var pageRows = built.OrderedRows.Skip(skip).Take(pageSize).ToList();
        return new TaktEcMonthlyTrendResultDto
        {
            Paged = TaktPagedResult<TaktEcMonthlyTrendDto>.Create(
                pageRows, built.OrderedRows.Count, pageIndex, pageSize),
            PeriodOrder = built.PeriodOrder,
            RowCount = built.OrderedRows.Count,
            BasePeriod = pageRows.FirstOrDefault()?.BasePeriod ?? built.BasePeriod,
            ComparePeriod = built.ComparePeriod,
            UpCount = built.UpCount,
            DownCount = built.DownCount,
            FlatCount = built.FlatCount,
            NoneCount = built.NoneCount,
        };
    }

    /// <inheritdoc />
    public async Task<(string fileName, byte[] fileContent)> ExportEcMonthlyTrendAnalysisAsync(
        TaktEcMonthlyTrendQueryDto query,
        string? sheetName = null,
        string? fileName = null)
    {
        ArgumentNullException.ThrowIfNull(query);
        var built = await BuildEcMonthlyTrendAnalysisAsync(query);
        var columnKeys = new List<string> { "plantCode", "ecDistinction" };
        var columnLabels = new List<string> { "工厂代码", "区分" };
        foreach (var period in built.PeriodOrder)
        {
            columnKeys.Add($"period_{period}");
            columnLabels.Add($"{period}件数");
            columnKeys.Add($"loss_{period}");
            columnLabels.Add($"{period}损失金额");
        }
        columnKeys.AddRange(new[] { "basePeriod", "comparePeriod", "varianceAmount", "variancePercent", "trend" });
        columnLabels.AddRange(new[] { "基准月", "对比月", "环比差额", "环比%", "涨跌" });
        var exportRows = built.OrderedRows.Select(row =>
        {
            var dict = new Dictionary<string, object?>(StringComparer.Ordinal)
            {
                ["plantCode"] = row.PlantCode,
                ["ecDistinction"] = row.EcDistinction,
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
                dict[$"period_{period}"] = row.PeriodValues.TryGetValue(period, out var count) ? count : 0;
                dict[$"loss_{period}"] = row.PeriodLossAmounts.TryGetValue(period, out var loss)
                    ? Math.Round(loss, 4, MidpointRounding.AwayFromZero)
                    : null;
            }
            return (IReadOnlyDictionary<string, object?>)dict;
        }).ToList();
        return await TaktExcelHelper.ExportDictionaryRowsAsync(
            exportRows,
            columnKeys,
            columnLabels,
            sheetName ?? "月设变推移表",
            fileName ?? "月设变推移表.xlsx");
    }

    /// <inheritdoc />
    public async Task<TaktEcImplementationMonthlyTrendResultDto> GetEcImplementationMonthlyTrendAnalysisAsync(
        TaktEcImplementationMonthlyTrendQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        var pageIndex = TaktPagedClamp.NormalizePageIndex(queryDto.PageIndex);
        var pageSize = TaktPagedClamp.NormalizePageSize(queryDto.PageSize);
        var skip = TaktPagedClamp.ComputeSkip(pageIndex, pageSize);
        var built = await BuildEcImplementationMonthlyTrendAnalysisAsync(queryDto);
        var pageRows = built.OrderedRows.Skip(skip).Take(pageSize).ToList();
        return new TaktEcImplementationMonthlyTrendResultDto
        {
            Paged = TaktPagedResult<TaktEcImplementationMonthlyTrendDto>.Create(
                pageRows, built.OrderedRows.Count, pageIndex, pageSize),
            PeriodOrder = built.PeriodOrder,
            RowCount = built.OrderedRows.Count,
            BasePeriod = pageRows.FirstOrDefault()?.BasePeriod ?? built.BasePeriod,
            ComparePeriod = built.ComparePeriod,
            UpCount = built.UpCount,
            DownCount = built.DownCount,
            FlatCount = built.FlatCount,
            NoneCount = built.NoneCount,
        };
    }

    /// <inheritdoc />
    public async Task<(string fileName, byte[] fileContent)> ExportEcImplementationMonthlyTrendAnalysisAsync(
        TaktEcImplementationMonthlyTrendQueryDto query,
        string? sheetName = null,
        string? fileName = null)
    {
        ArgumentNullException.ThrowIfNull(query);
        var built = await BuildEcImplementationMonthlyTrendAnalysisAsync(query);
        var columnKeys = new List<string> { "plantCode", "deptCode" };
        var columnLabels = new List<string> { "工厂代码", "部门编码" };
        foreach (var period in built.PeriodOrder)
        {
            columnKeys.Add($"period_{period}");
            columnLabels.Add($"{period}件数");
        }
        columnKeys.AddRange(new[] { "basePeriod", "comparePeriod", "varianceAmount", "variancePercent", "trend" });
        columnLabels.AddRange(new[] { "基准月", "对比月", "环比差额", "环比%", "涨跌" });
        var exportRows = built.OrderedRows.Select(row =>
        {
            var dict = new Dictionary<string, object?>(StringComparer.Ordinal)
            {
                ["plantCode"] = row.PlantCode,
                ["deptCode"] = row.DeptCode,
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
                dict[$"period_{period}"] = row.PeriodValues.TryGetValue(period, out var count) ? count : 0;
            }
            return (IReadOnlyDictionary<string, object?>)dict;
        }).ToList();
        return await TaktExcelHelper.ExportDictionaryRowsAsync(
            exportRows,
            columnKeys,
            columnLabels,
            sheetName ?? "月实施推移表",
            fileName ?? "月实施推移表.xlsx");
    }

    /// <summary>
    /// 构建月设变推移转置分析全量结果
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>内存构建结果</returns>
    private async Task<EcMonthlyTrendAnalysisBuilt> BuildEcMonthlyTrendAnalysisAsync(
        TaktEcMonthlyTrendQueryDto queryDto)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.PlantCode);
        EnsureThreeLayerContext();
        var plantCode = queryDto.PlantCode.Trim();
        var periodOrder = ResolvePeriodOrder(queryDto, out var rangeStart, out var rangeEnd);
        if (periodOrder.Count == 0)
        {
            return EcMonthlyTrendAnalysisBuilt.Empty();
        }
        var focusPeriod = ResolveFocusPeriod(queryDto.FocusPeriod, periodOrder);
        var rangeEndExclusive = rangeEnd.AddMonths(1);
        var records = await _ecGijutsuRepository.GetListAsync(
            BuildEcGijutsuTrendExpression(plantCode, queryDto, rangeStart, rangeEndExclusive));
        if (records.Count == 0)
        {
            return EcMonthlyTrendAnalysisBuilt.Empty();
        }
        var periodSet = new HashSet<string>(periodOrder, StringComparer.Ordinal);
        var allRows = records
            .GroupBy(
                r => new EcMonthlyTrendRowKey(r.PlantCode.Trim(), r.EcDistinction),
                EcMonthlyTrendRowKeyComparer.Instance)
            .Select(g => BuildEcMonthlyTrendRow(g.Key, g.ToList(), periodSet, focusPeriod))
            .ToList();
        var filtered = FilterTrendRows(allRows, queryDto.TrendFilter);
        var ordered = OrderTrendRows(filtered);
        return new EcMonthlyTrendAnalysisBuilt
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
    /// 构建月实施推移转置分析全量结果
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>内存构建结果</returns>
    private async Task<EcImplementationMonthlyTrendAnalysisBuilt> BuildEcImplementationMonthlyTrendAnalysisAsync(
        TaktEcImplementationMonthlyTrendQueryDto queryDto)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.PlantCode);
        EnsureThreeLayerContext();
        var plantCode = queryDto.PlantCode.Trim();
        var periodOrder = ResolvePeriodOrder(
            queryDto.PeriodDateStart,
            queryDto.PeriodDateEnd,
            out var rangeStart,
            out var rangeEnd);
        if (periodOrder.Count == 0)
        {
            return EcImplementationMonthlyTrendAnalysisBuilt.Empty();
        }
        var focusPeriod = ResolveFocusPeriod(queryDto.FocusPeriod, periodOrder);
        var rangeEndExclusive = rangeEnd.AddMonths(1);
        var tasks = await _ecExecutionTaskRepository.GetListAsync(
            BuildEcExecutionTaskTrendExpression(queryDto, rangeStart, rangeEndExclusive));
        if (tasks.Count == 0)
        {
            return EcImplementationMonthlyTrendAnalysisBuilt.Empty();
        }
        var ecIds = tasks.Select(t => t.EcId).Distinct().ToList();
        var gijutsuRecords = await _ecGijutsuRepository.GetListAsync(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.PlantCode == plantCode
            && ecIds.Contains(x.Id));
        if (gijutsuRecords.Count == 0)
        {
            return EcImplementationMonthlyTrendAnalysisBuilt.Empty();
        }
        var plantByEcId = gijutsuRecords.ToDictionary(g => g.Id, g => g.PlantCode.Trim());
        var periodSet = new HashSet<string>(periodOrder, StringComparer.Ordinal);
        var joinedTasks = tasks
            .Where(t => plantByEcId.ContainsKey(t.EcId))
            .Select(t => new EcImplementationTaskSnapshot(
                plantByEcId[t.EcId],
                t.DeptCode.Trim(),
                t.CompletedAt!.Value))
            .ToList();
        if (joinedTasks.Count == 0)
        {
            return EcImplementationMonthlyTrendAnalysisBuilt.Empty();
        }
        var allRows = joinedTasks
            .GroupBy(
                t => new EcImplementationMonthlyTrendRowKey(t.PlantCode, t.DeptCode),
                EcImplementationMonthlyTrendRowKeyComparer.Instance)
            .Select(g => BuildEcImplementationMonthlyTrendRow(g.Key, g.ToList(), periodSet, focusPeriod))
            .ToList();
        var filtered = FilterImplementationTrendRows(allRows, queryDto.TrendFilter);
        var ordered = OrderImplementationTrendRows(filtered);
        return new EcImplementationMonthlyTrendAnalysisBuilt
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
    /// 构建设变实施推移筛选条件
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <param name="rangeStart">期间起</param>
    /// <param name="rangeEndExclusive">期间止（不含）</param>
    /// <returns>表达式</returns>
    private Expression<Func<TaktEcExecutionTask, bool>> BuildEcExecutionTaskTrendExpression(
        TaktEcImplementationMonthlyTrendQueryDto queryDto,
        DateTime rangeStart,
        DateTime rangeEndExclusive)
    {
        var exp = Expressionable.Create<TaktEcExecutionTask>();
        exp = exp.And(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.TaskStatus == CompletedTaskStatus
            && x.CompletedAt != null
            && x.CompletedAt >= rangeStart
            && x.CompletedAt < rangeEndExclusive);
        if (!string.IsNullOrWhiteSpace(queryDto.DeptCode))
        {
            var deptCode = queryDto.DeptCode.Trim();
            exp = exp.And(x => x.DeptCode == deptCode);
        }
        return exp.ToExpression();
    }

    /// <summary>
    /// 构建单行月实施推移
    /// </summary>
    /// <param name="key">行键</param>
    /// <param name="groupRows">同键完成任务</param>
    /// <param name="periodSet">展示期间集合</param>
    /// <param name="focusPeriod">关注期间</param>
    /// <returns>转置行</returns>
    private static TaktEcImplementationMonthlyTrendDto BuildEcImplementationMonthlyTrendRow(
        EcImplementationMonthlyTrendRowKey key,
        IReadOnlyList<EcImplementationTaskSnapshot> groupRows,
        IReadOnlySet<string> periodSet,
        string? focusPeriod)
    {
        var row = new TaktEcImplementationMonthlyTrendDto
        {
            PlantCode = key.PlantCode,
            DeptCode = key.DeptCode,
            Trend = "none",
        };
        foreach (var period in groupRows
                     .Select(r => new DateTime(r.CompletedAt.Year, r.CompletedAt.Month, 1).ToString("yyyy-MM"))
                     .Where(periodSet.Contains)
                     .GroupBy(p => p, StringComparer.Ordinal))
        {
            row.PeriodValues[period.Key] = period.Count();
        }
        ApplyImplementationFocusTrend(row, focusPeriod);
        return row;
    }

    /// <summary>
    /// 按关注月计算环比涨跌（基于实施件数）
    /// </summary>
    /// <param name="row">转置行</param>
    /// <param name="focusPeriod">关注期间 yyyy-MM</param>
    private static void ApplyImplementationFocusTrend(
        TaktEcImplementationMonthlyTrendDto row,
        string? focusPeriod)
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
        row.PeriodValues.TryGetValue(basePeriod, out var baseCount);
        row.PeriodValues.TryGetValue(comparePeriod, out var compareCount);
        row.VarianceAmount = compareCount - baseCount;
        if (baseCount != 0)
        {
            row.VariancePercent = Math.Round(
                (decimal)row.VarianceAmount.Value / baseCount,
                4,
                MidpointRounding.AwayFromZero);
        }
        if (compareCount > baseCount)
        {
            row.Trend = "up";
        }
        else if (compareCount < baseCount)
        {
            row.Trend = "down";
        }
        else
        {
            row.Trend = "flat";
        }
    }

    /// <summary>
    /// 实施推移涨跌筛选
    /// </summary>
    /// <param name="rows">全量行</param>
    /// <param name="trendFilter">筛选码</param>
    /// <returns>筛选后行</returns>
    private static List<TaktEcImplementationMonthlyTrendDto> FilterImplementationTrendRows(
        IReadOnlyList<TaktEcImplementationMonthlyTrendDto> rows,
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
    /// 实施推移涨跌优先排序
    /// </summary>
    /// <param name="rows">行集合</param>
    /// <returns>排序后行</returns>
    private static List<TaktEcImplementationMonthlyTrendDto> OrderImplementationTrendRows(
        IReadOnlyList<TaktEcImplementationMonthlyTrendDto> rows)
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
            .ThenByDescending(r => Math.Abs(r.VarianceAmount ?? 0))
            .ThenBy(r => r.DeptCode, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    /// <summary>
    /// 构建设变推移筛选条件
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="queryDto">查询 DTO</param>
    /// <param name="rangeStart">期间起</param>
    /// <param name="rangeEndExclusive">期间止（不含）</param>
    /// <returns>表达式</returns>
    private Expression<Func<TaktEcGijutsu, bool>> BuildEcGijutsuTrendExpression(
        string plantCode,
        TaktEcMonthlyTrendQueryDto queryDto,
        DateTime rangeStart,
        DateTime rangeEndExclusive)
    {
        var exp = Expressionable.Create<TaktEcGijutsu>();
        exp = exp.And(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.PlantCode == plantCode
            && x.EcIssueDate >= rangeStart
            && x.EcIssueDate < rangeEndExclusive);
        if (queryDto.EcDistinction.HasValue)
        {
            var distinction = queryDto.EcDistinction.Value;
            exp = exp.And(x => x.EcDistinction == distinction);
        }
        if (queryDto.ChangeStatus.HasValue)
        {
            var changeStatus = queryDto.ChangeStatus.Value;
            exp = exp.And(x => x.ChangeStatus == changeStatus);
        }
        if (queryDto.EcStatus.HasValue)
        {
            var ecStatus = queryDto.EcStatus.Value;
            exp = exp.And(x => x.EcStatus == ecStatus);
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
        TaktEcMonthlyTrendQueryDto queryDto,
        out DateTime rangeStart,
        out DateTime rangeEnd) =>
        ResolvePeriodOrder(queryDto.PeriodDateStart, queryDto.PeriodDateEnd, out rangeStart, out rangeEnd);

    /// <summary>
    /// 解析期间列顺序
    /// </summary>
    /// <param name="periodDateStart">期间起</param>
    /// <param name="periodDateEnd">期间止</param>
    /// <param name="rangeStart">区间起</param>
    /// <param name="rangeEnd">区间止（月初）</param>
    /// <returns>期间列顺序</returns>
    private static List<string> ResolvePeriodOrder(
        DateTime? periodDateStart,
        DateTime? periodDateEnd,
        out DateTime rangeStart,
        out DateTime rangeEnd)
    {
        var (periodStart, periodEnd) = NormalizePeriodBounds(periodDateStart, periodDateEnd);
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
    /// 构建单行月设变推移
    /// </summary>
    /// <param name="key">行键</param>
    /// <param name="groupRows">同键设变记录</param>
    /// <param name="periodSet">展示期间集合</param>
    /// <param name="focusPeriod">关注期间</param>
    /// <returns>转置行</returns>
    private static TaktEcMonthlyTrendDto BuildEcMonthlyTrendRow(
        EcMonthlyTrendRowKey key,
        IReadOnlyList<TaktEcGijutsu> groupRows,
        IReadOnlySet<string> periodSet,
        string? focusPeriod)
    {
        var row = new TaktEcMonthlyTrendDto
        {
            PlantCode = key.PlantCode,
            EcDistinction = key.EcDistinction,
            Trend = "none",
        };
        foreach (var period in groupRows
                     .Select(r => new
                     {
                         Period = new DateTime(r.EcIssueDate.Year, r.EcIssueDate.Month, 1).ToString("yyyy-MM"),
                         LossAmount = r.EcLossAmount,
                     })
                     .Where(r => periodSet.Contains(r.Period))
                     .GroupBy(r => r.Period, StringComparer.Ordinal))
        {
            row.PeriodValues[period.Key] = period.Count();
            row.PeriodLossAmounts[period.Key] = RoundAmount(period.Sum(r => r.LossAmount));
        }
        ApplyFocusTrend(row, focusPeriod);
        return row;
    }

    /// <summary>
    /// 按关注月计算环比涨跌（基于件数）
    /// </summary>
    /// <param name="row">转置行</param>
    /// <param name="focusPeriod">关注期间 yyyy-MM</param>
    private static void ApplyFocusTrend(TaktEcMonthlyTrendDto row, string? focusPeriod)
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
        row.PeriodValues.TryGetValue(basePeriod, out var baseCount);
        row.PeriodValues.TryGetValue(comparePeriod, out var compareCount);
        row.VarianceAmount = compareCount - baseCount;
        if (baseCount != 0)
        {
            row.VariancePercent = Math.Round(
                (decimal)row.VarianceAmount.Value / baseCount,
                4,
                MidpointRounding.AwayFromZero);
        }
        if (compareCount > baseCount)
        {
            row.Trend = "up";
        }
        else if (compareCount < baseCount)
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
    private static List<TaktEcMonthlyTrendDto> FilterTrendRows(
        IReadOnlyList<TaktEcMonthlyTrendDto> rows,
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
    private static List<TaktEcMonthlyTrendDto> OrderTrendRows(
        IReadOnlyList<TaktEcMonthlyTrendDto> rows)
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
            .ThenByDescending(r => Math.Abs(r.VarianceAmount ?? 0))
            .ThenBy(r => r.EcDistinction)
            .ToList();
    }

    /// <summary>
    /// 金额四舍五入至 4 位
    /// </summary>
    /// <param name="value">金额</param>
    /// <returns>四舍五入后金额</returns>
    private static decimal RoundAmount(decimal value) =>
        Math.Round(value, 4, MidpointRounding.AwayFromZero);

    /// <summary>
    /// 月设变推移行键
    /// </summary>
    /// <param name="PlantCode">工厂代码</param>
    /// <param name="EcDistinction">区分</param>
    private sealed record EcMonthlyTrendRowKey(string PlantCode, int EcDistinction);

    /// <summary>
    /// 月设变推移行键比较器
    /// </summary>
    private sealed class EcMonthlyTrendRowKeyComparer : IEqualityComparer<EcMonthlyTrendRowKey>
    {
        /// <summary>单例</summary>
        public static EcMonthlyTrendRowKeyComparer Instance { get; } = new();

        /// <inheritdoc />
        public bool Equals(EcMonthlyTrendRowKey? x, EcMonthlyTrendRowKey? y)
        {
            if (x is null || y is null)
            {
                return ReferenceEquals(x, y);
            }
            return string.Equals(x.PlantCode, y.PlantCode, StringComparison.OrdinalIgnoreCase)
                && x.EcDistinction == y.EcDistinction;
        }

        /// <inheritdoc />
        public int GetHashCode(EcMonthlyTrendRowKey obj) =>
            HashCode.Combine(obj.PlantCode.ToUpperInvariant(), obj.EcDistinction);
    }

    /// <summary>
    /// 月设变推移分析构建结果
    /// </summary>
    private sealed class EcMonthlyTrendAnalysisBuilt
    {
        /// <summary>排序后全量行</summary>
        public List<TaktEcMonthlyTrendDto> OrderedRows { get; init; } = new();

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
        public static EcMonthlyTrendAnalysisBuilt Empty() => new();
    }

    /// <summary>
    /// 月实施推移行键
    /// </summary>
    /// <param name="PlantCode">工厂代码</param>
    /// <param name="DeptCode">部门编码</param>
    private sealed record EcImplementationMonthlyTrendRowKey(string PlantCode, string DeptCode);

    /// <summary>
    /// 月实施推移行键比较器
    /// </summary>
    private sealed class EcImplementationMonthlyTrendRowKeyComparer : IEqualityComparer<EcImplementationMonthlyTrendRowKey>
    {
        /// <summary>单例</summary>
        public static EcImplementationMonthlyTrendRowKeyComparer Instance { get; } = new();

        /// <inheritdoc />
        public bool Equals(EcImplementationMonthlyTrendRowKey? x, EcImplementationMonthlyTrendRowKey? y)
        {
            if (x is null || y is null)
            {
                return ReferenceEquals(x, y);
            }
            return string.Equals(x.PlantCode, y.PlantCode, StringComparison.OrdinalIgnoreCase)
                && string.Equals(x.DeptCode, y.DeptCode, StringComparison.OrdinalIgnoreCase);
        }

        /// <inheritdoc />
        public int GetHashCode(EcImplementationMonthlyTrendRowKey obj) =>
            HashCode.Combine(obj.PlantCode.ToUpperInvariant(), obj.DeptCode.ToUpperInvariant());
    }

    /// <summary>
    /// 月实施推移任务快照
    /// </summary>
    /// <param name="PlantCode">工厂代码</param>
    /// <param name="DeptCode">部门编码</param>
    /// <param name="CompletedAt">完成时间</param>
    private sealed record EcImplementationTaskSnapshot(string PlantCode, string DeptCode, DateTime CompletedAt);

    /// <summary>
    /// 月实施推移分析构建结果
    /// </summary>
    private sealed class EcImplementationMonthlyTrendAnalysisBuilt
    {
        /// <summary>排序后全量行</summary>
        public List<TaktEcImplementationMonthlyTrendDto> OrderedRows { get; init; } = new();

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
        public static EcImplementationMonthlyTrendAnalysisBuilt Empty() => new();
    }
}
