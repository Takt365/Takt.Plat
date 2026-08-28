// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Procurement
// 文件名称：TaktPurchasePriceTrendMonthlyAnalysisBuilder.cs
// 创建时间：2026-08-23
// 创建人：Takt365(Cursor AI)
// 功能描述：采购价格月推移转置分析构建器（主表/明细筛选、期间列、涨跌环比）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Procurement;
using Takt.Domain.Entities.Logistics.Materials;
using Takt.Domain.Entities.Logistics.Procurement;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Services.Logistics.Procurement;

/// <summary>
/// 采购价格月推移转置分析构建器
/// </summary>
public class TaktPurchasePriceTrendMonthlyAnalysisBuilder : TaktServiceBase, ITaktPurchasePriceTrendMonthlyAnalysisBuilder
{
    /// <summary>
    /// 物料/供应商名称按编码分批查询，避免超长 IN 列表
    /// </summary>
    private const int MaterialNameLookupBatchSize = 500;

    private readonly ITaktCompanyRepository<TaktPurchasePrice> _purchasePriceRepository;
    private readonly ITaktCompanyRepository<TaktPurchasePriceItem> _purchasePriceItemRepository;
    private readonly ITaktCompanyRepository<TaktMaterialPlant> _materialPlantRepository;
    private readonly ITaktCompanyRepository<TaktSupplier> _supplierRepository;

    public TaktPurchasePriceTrendMonthlyAnalysisBuilder(
        ITaktCompanyRepository<TaktPurchasePrice> purchasePriceRepository,
        ITaktCompanyRepository<TaktPurchasePriceItem> purchasePriceItemRepository,
        ITaktCompanyRepository<TaktMaterialPlant> materialPlantRepository,
        ITaktCompanyRepository<TaktSupplier> supplierRepository,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _purchasePriceRepository = purchasePriceRepository;
        _purchasePriceItemRepository = purchasePriceItemRepository;
        _materialPlantRepository = materialPlantRepository;
        _supplierRepository = supplierRepository;
    }

    /// <summary>
    /// 构建采购价格月推移转置分析全量结果
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>内存构建结果</returns>
    public async Task<TaktPurchasePriceTrendAnalysisBuilt> BuildAsync(
        TaktPurchasePriceTrendQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.PlantCode);
        EnsureThreeLayerContext();
        var plantCode = queryDto.PlantCode.Trim();
        var supplierFilter = string.IsNullOrWhiteSpace(queryDto.SupplierCode) ? null : queryDto.SupplierCode.Trim();
        var materialFilter = string.IsNullOrWhiteSpace(queryDto.MaterialCode) ? null : queryDto.MaterialCode.Trim();
        var priceTypeFilter = string.IsNullOrWhiteSpace(queryDto.PriceType) ? null : queryDto.PriceType.Trim();
        var masterExp = BuildPurchasePriceTrendMasterExpression(
            plantCode,
            supplierFilter,
            materialFilter,
            priceTypeFilter);
        var masters = await _purchasePriceRepository.GetListAsync(masterExp);
        if (masters.Count == 0)
        {
            return TaktPurchasePriceTrendAnalysisBuilt.Empty();
        }
        var masterById = masters.ToDictionary(m => m.Id);
        var masterIds = masters.Select(m => m.Id).ToList();
        var itemExp = BuildPurchasePriceTrendItemExpression(masterIds);
        var items = await _purchasePriceItemRepository.GetListAsync(itemExp);
        if (items.Count == 0)
        {
            return TaktPurchasePriceTrendAnalysisBuilt.Empty();
        }
        var sourceRows = items
            .Where(i => masterById.ContainsKey(i.PurchasePriceId))
            .Select(i => new PurchasePriceTrendSourceRow
            {
                Master = masterById[i.PurchasePriceId],
                Item = i,
            })
            .ToList();
        var (rangeStart, rangeEnd, periodOrder) = ResolvePurchasePriceTrendRange(queryDto);
        var focusPeriod = ResolvePurchasePriceFocusPeriod(queryDto.FocusPeriod, periodOrder);
        var allRows = sourceRows
            .GroupBy(
                r => new PurchasePriceTrendRowKey(
                    r.Master.PlantCode.Trim(),
                    r.Master.MaterialCode.Trim(),
                    r.Master.SupplierCode.Trim()),
                PurchasePriceTrendRowKeyComparer.Instance)
            .Select(g => BuildPurchasePriceTrendRow(
                g.Key,
                g.ToList(),
                periodOrder,
                focusPeriod,
                rangeStart,
                rangeEnd))
            .ToList();
        await FillPurchasePriceTrendDisplayNamesAsync(plantCode, allRows);
        var filtered = FilterPurchasePriceTrendRows(allRows, queryDto.TrendFilter);
        var ordered = OrderPurchasePriceTrendRows(filtered);
        return new TaktPurchasePriceTrendAnalysisBuilt
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
    /// 构建采购价格推移主表筛选条件（OnlyEnabled 已无 PriceStatus，忽略）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="supplierFilter">供应商编码包含</param>
    /// <param name="materialFilter">物料编码包含</param>
    /// <param name="priceType">价格类型（如 PB00）</param>
    /// <returns>表达式</returns>
    private Expression<Func<TaktPurchasePrice, bool>> BuildPurchasePriceTrendMasterExpression(
        string plantCode,
        string? supplierFilter,
        string? materialFilter,
        string? priceType)
    {
        var exp = Expressionable.Create<TaktPurchasePrice>();
        exp = exp.And(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.PlantCode == plantCode);
        if (!string.IsNullOrWhiteSpace(supplierFilter))
        {
            var supplier = supplierFilter.Trim();
            exp = exp.And(x => x.SupplierCode == supplier);
        }
        if (!string.IsNullOrWhiteSpace(materialFilter))
        {
            var material = materialFilter.Trim();
            exp = exp.And(x => x.MaterialCode == material);
        }
        if (!string.IsNullOrWhiteSpace(priceType))
        {
            exp = exp.And(x => x.PriceType == priceType);
        }
        return exp.ToExpression();
    }

    /// <summary>
    /// 构建采购价格推移明细筛选条件（跳过作废行）
    /// </summary>
    /// <param name="masterIds">主表 ID 列表</param>
    /// <returns>表达式</returns>
    private Expression<Func<TaktPurchasePriceItem, bool>> BuildPurchasePriceTrendItemExpression(
        IReadOnlyList<long> masterIds)
    {
        var exp = Expressionable.Create<TaktPurchasePriceItem>();
        exp = exp.And(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && masterIds.Contains(x.PurchasePriceId)
            && x.IsObsolete == 0);
        return exp.ToExpression();
    }

    /// <summary>
    /// 解析采购价格推移分析日期区间与期间列
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>区间起止与期间列顺序</returns>
    private static (DateTime RangeStart, DateTime RangeEnd, List<string> PeriodOrder) ResolvePurchasePriceTrendRange(
        TaktPurchasePriceTrendQueryDto queryDto)
    {
        var (periodStart, periodEnd) = NormalizePurchasePricePeriodBounds(
            queryDto.PeriodDateStart,
            queryDto.PeriodDateEnd);
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
            var rangeStart = startMonth;
            var rangeEnd = endMonth.AddMonths(1).AddDays(-1);
            var periodOrder = BuildConsecutivePeriodOrder(startMonth, endMonth);
            return (rangeStart, rangeEnd, periodOrder);
        }
        var (resolvedStart, resolvedEnd) = TaktPriceTrendAnalysisHelper.ResolveTrendDateRange(null, null);
        var start = new DateTime(resolvedStart.Year, resolvedStart.Month, 1);
        var endMonthFirst = new DateTime(resolvedEnd.Year, resolvedEnd.Month, 1);
        return (resolvedStart, resolvedEnd, BuildConsecutivePeriodOrder(start, endMonthFirst));
    }

    /// <summary>
    /// 归一化期间起止到月初
    /// </summary>
    /// <param name="periodDateStart">期间起</param>
    /// <param name="periodDateEnd">期间止</param>
    /// <returns>月初起止</returns>
    private static (DateTime? Start, DateTime? End) NormalizePurchasePricePeriodBounds(
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
    private static string? ResolvePurchasePriceFocusPeriod(string? focusPeriod, IReadOnlyList<string> periodOrder)
    {
        if (!string.IsNullOrWhiteSpace(focusPeriod))
        {
            return focusPeriod.Trim();
        }
        return periodOrder.Count > 0 ? periodOrder[^1] : null;
    }

    /// <summary>
    /// 构建单行采购价格月推移
    /// </summary>
    /// <param name="key">行键</param>
    /// <param name="groupRows">同键明细</param>
    /// <param name="periodOrder">期间列</param>
    /// <param name="focusPeriod">关注期间</param>
    /// <param name="rangeStart">分析区间起</param>
    /// <param name="rangeEnd">分析区间止</param>
    /// <returns>转置行</returns>
    private static TaktPurchasePriceTrendDto BuildPurchasePriceTrendRow(
        PurchasePriceTrendRowKey key,
        IReadOnlyList<PurchasePriceTrendSourceRow> groupRows,
        IReadOnlyList<string> periodOrder,
        string? focusPeriod,
        DateTime rangeStart,
        DateTime rangeEnd)
    {
        var entries = groupRows.Select(r => new TaktPriceTrendEntry
        {
            EffectiveStartDate = r.Master.ValidFrom,
            EffectiveEndDate = r.Master.ValidTo,
            RawPrice = r.Item.Price,
            PerUnit = r.Item.PriceUnit,
            Unit = r.Item.UnitOfMeasure ?? string.Empty,
            ReferenceCode = r.Master.SupplierCode,
        }).ToList();
        // 缺月回填最近有效价（与物料移动价格推移一致；回填写入最近价格日期供前端 * 悬停）
        var points = TaktPriceTrendAnalysisHelper.BuildMonthlyTrendPoints(
            entries,
            rangeStart,
            rangeEnd,
            carryForwardMissingMonths: true);
        var pointByMonth = points.ToDictionary(p => p.YearMonth, StringComparer.Ordinal);
        // 无自定义比较器，避免 JSON 序列化后前端读不到来源日期（* 标记）
        var periodUnitPrices = new Dictionary<string, decimal>();
        var periodPriceSourcePeriods = new Dictionary<string, string>();
        var row = new TaktPurchasePriceTrendDto
        {
            PlantCode = key.PlantCode,
            MaterialCode = key.MaterialCode,
            SupplierCode = key.SupplierCode,
            CurrencyCode = groupRows
                .Select(r => r.Item.ConditionCurrencyCode?.Trim())
                .FirstOrDefault(c => !string.IsNullOrWhiteSpace(c)) ?? string.Empty,
            Unit = entries.FirstOrDefault(e => !string.IsNullOrWhiteSpace(e.Unit))?.Unit ?? string.Empty,
            Trend = "none",
            PeriodUnitPrices = periodUnitPrices,
            PeriodPriceSourcePeriods = periodPriceSourcePeriods,
        };
        foreach (var period in periodOrder)
        {
            if (!pointByMonth.TryGetValue(period, out var point) || !point.HasPrice)
            {
                continue;
            }
            periodUnitPrices[period] = RoundPurchasePriceUnitPrice(point.UnitPrice);
            // 当月有价=yyyy-MM；缺月回填=最近价格日期 yyyy-MM-dd（与移动价 * 说明一致）
            periodPriceSourcePeriods[period] = TaktPriceTrendAnalysisHelper.ResolvePeriodPriceSourceLabel(point);
            if (!string.IsNullOrWhiteSpace(point.Unit))
            {
                row.Unit = point.Unit;
            }
        }
        ApplyPurchasePriceFocusTrend(row, focusPeriod);
        return row;
    }

    /// <summary>
    /// 回填物料描述与供应商名称
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="rows">推移行</param>
    private async Task FillPurchasePriceTrendDisplayNamesAsync(
        string plantCode,
        List<TaktPurchasePriceTrendDto> rows)
    {
        if (rows.Count == 0)
        {
            return;
        }
        var materialCodes = rows
            .Select(r => r.MaterialCode)
            .Where(c => !string.IsNullOrWhiteSpace(c))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        var supplierCodes = rows
            .Select(r => r.SupplierCode)
            .Where(c => !string.IsNullOrWhiteSpace(c))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        var materialNames = await LoadMaterialNameLookupAsync(plantCode, materialCodes);
        var supplierNames = await LoadSupplierNameLookupAsync(plantCode, supplierCodes);
        foreach (var row in rows)
        {
            if (materialNames.TryGetValue(row.MaterialCode, out var materialDescription))
            {
                row.MaterialDescription = materialDescription;
            }
            if (supplierNames.TryGetValue(row.SupplierCode, out var supplierName))
            {
                row.SupplierName = supplierName;
            }
        }
    }

    /// <summary>
    /// 加载工厂物料描述字典
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="materialCodes">物料编码</param>
    /// <returns>编码→名称</returns>
    private async Task<Dictionary<string, string>> LoadMaterialNameLookupAsync(
        string plantCode,
        IReadOnlyList<string> materialCodes)
    {
        if (materialCodes.Count == 0)
        {
            return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }
        var codes = materialCodes.Where(c => !string.IsNullOrWhiteSpace(c)).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
        var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        for (var offset = 0; offset < codes.Count; offset = checked(offset + MaterialNameLookupBatchSize))
        {
            var batch = codes.Skip(offset).Take(MaterialNameLookupBatchSize).ToList();
            var plants = await _materialPlantRepository.GetListAsync(
                x => x.TenantCode == CurrentTenantCode
                    && x.CompanyCode == CurrentCompanyCode
                    && x.PlantCode == plantCode
                    && batch.Contains(x.MaterialCode));
            foreach (var group in plants
                .Where(p => !string.IsNullOrWhiteSpace(p.MaterialCode))
                .GroupBy(p => p.MaterialCode.Trim(), StringComparer.OrdinalIgnoreCase))
            {
                if (map.ContainsKey(group.Key))
                {
                    continue;
                }
                map[group.Key] = group.Select(x => x.MaterialDescription)
                    .FirstOrDefault(n => !string.IsNullOrWhiteSpace(n))?.Trim() ?? string.Empty;
            }
        }
        return map;
    }

    /// <summary>
    /// 加载指定工厂下存在于供应商主数据的编码集合（用于级联交叉过滤）
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="supplierCodes">候选供应商编码</param>
    /// <returns>该工厂主数据中存在的编码；主数据无该厂记录时返回空集（调用方回退为本表全集）</returns>
    private async Task<HashSet<string>> LoadPlantScopedSupplierCodesAsync(
        string plantCode,
        IReadOnlyList<string> supplierCodes)
    {
        var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        if (string.IsNullOrWhiteSpace(plantCode) || supplierCodes.Count == 0)
        {
            return result;
        }
        var plant = plantCode.Trim();
        var codes = supplierCodes.Where(c => !string.IsNullOrWhiteSpace(c)).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
        for (var offset = 0; offset < codes.Count; offset = checked(offset + MaterialNameLookupBatchSize))
        {
            var batch = codes.Skip(offset).Take(MaterialNameLookupBatchSize).ToList();
            var suppliers = await _supplierRepository.GetListAsync(
                x => x.TenantCode == CurrentTenantCode
                    && x.CompanyCode == CurrentCompanyCode
                    && x.PlantCode == plant
                    && batch.Contains(x.SupplierCode));
            foreach (var code in suppliers
                .Select(s => s.SupplierCode?.Trim())
                .Where(c => !string.IsNullOrWhiteSpace(c))!)
            {
                result.Add(code!);
            }
        }
        return result;
    }

    /// <summary>
    /// 加载供应商名称字典（优先同工厂主数据）
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="supplierCodes">供应商编码</param>
    /// <returns>编码→名称</returns>
    private async Task<Dictionary<string, string>> LoadSupplierNameLookupAsync(
        string plantCode,
        IReadOnlyList<string> supplierCodes)
    {
        if (supplierCodes.Count == 0)
        {
            return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }
        var plant = plantCode?.Trim() ?? string.Empty;
        var codes = supplierCodes.Where(c => !string.IsNullOrWhiteSpace(c)).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
        var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        for (var offset = 0; offset < codes.Count; offset = checked(offset + MaterialNameLookupBatchSize))
        {
            var batch = codes.Skip(offset).Take(MaterialNameLookupBatchSize).ToList();
            var suppliers = string.IsNullOrEmpty(plant)
                ? await _supplierRepository.GetListAsync(
                    x => x.TenantCode == CurrentTenantCode
                        && x.CompanyCode == CurrentCompanyCode
                        && batch.Contains(x.SupplierCode))
                : await _supplierRepository.GetListAsync(
                    x => x.TenantCode == CurrentTenantCode
                        && x.CompanyCode == CurrentCompanyCode
                        && x.PlantCode == plant
                        && batch.Contains(x.SupplierCode));
            // 同厂无命中时回退公司级编码（价目可能先于主数据 PlantCode 对齐）
            if (suppliers.Count == 0 && !string.IsNullOrEmpty(plant))
            {
                suppliers = await _supplierRepository.GetListAsync(
                    x => x.TenantCode == CurrentTenantCode
                        && x.CompanyCode == CurrentCompanyCode
                        && batch.Contains(x.SupplierCode));
            }
            foreach (var group in suppliers
                .Where(s => !string.IsNullOrWhiteSpace(s.SupplierCode))
                .GroupBy(s => s.SupplierCode.Trim(), StringComparer.OrdinalIgnoreCase))
            {
                if (map.ContainsKey(group.Key))
                {
                    continue;
                }
                map[group.Key] = group.Select(x => x.SupplierName1)
                    .FirstOrDefault(n => !string.IsNullOrWhiteSpace(n))?.Trim() ?? string.Empty;
            }
        }
        return map;
    }

    /// <summary>
    /// 按关注月计算环比涨跌
    /// </summary>
    /// <param name="row">转置行</param>
    /// <param name="focusPeriod">关注期间 yyyy-MM</param>
    private static void ApplyPurchasePriceFocusTrend(TaktPurchasePriceTrendDto row, string? focusPeriod)
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
        if (!row.PeriodUnitPrices.TryGetValue(basePeriod, out var basePrice)
            || !row.PeriodUnitPrices.TryGetValue(comparePeriod, out var comparePrice))
        {
            row.Trend = "none";
            return;
        }
        row.VarianceAmount = RoundPurchasePriceUnitPrice(comparePrice - basePrice);
        if (basePrice != 0m)
        {
            row.VariancePercent = Math.Round(
                row.VarianceAmount.Value / basePrice,
                4,
                MidpointRounding.AwayFromZero);
        }
        if (comparePrice > basePrice)
        {
            row.Trend = "up";
        }
        else if (comparePrice < basePrice)
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
    /// <param name="trendFilter">筛选码：空/all/leading=不按涨跌码过滤；up/down/flat/none/changed</param>
    /// <returns>筛选后行</returns>
    private static List<TaktPurchasePriceTrendDto> FilterPurchasePriceTrendRows(
        IReadOnlyList<TaktPurchasePriceTrendDto> rows,
        string? trendFilter)
    {
        if (string.IsNullOrWhiteSpace(trendFilter))
        {
            return rows.ToList();
        }
        var filter = trendFilter.Trim().ToLowerInvariant();
        if (filter is "all" or "leading")
        {
            return rows.ToList();
        }
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
    private static List<TaktPurchasePriceTrendDto> OrderPurchasePriceTrendRows(
        IReadOnlyList<TaktPurchasePriceTrendDto> rows)
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
            .ThenBy(r => r.MaterialCode, StringComparer.Ordinal)
            .ThenBy(r => r.SupplierCode, StringComparer.Ordinal)
            .ToList();
    }

    /// <summary>
    /// 单价四舍五入至 5 位
    /// </summary>
    /// <param name="value">单价</param>
    /// <returns>四舍五入后单价</returns>
    private static decimal RoundPurchasePriceUnitPrice(decimal value) =>
        Math.Round(value, 5, MidpointRounding.AwayFromZero);
    /// <summary>
    /// 采购价格推移行键
    /// </summary>
    /// <param name="PlantCode">工厂代码</param>
    /// <param name="MaterialCode">物料编码</param>
    /// <param name="SupplierCode">供应商编码</param>
    private sealed record PurchasePriceTrendRowKey(string PlantCode, string MaterialCode, string SupplierCode);

    /// <summary>
    /// 采购价格推移行键比较器
    /// </summary>
    private sealed class PurchasePriceTrendRowKeyComparer : IEqualityComparer<PurchasePriceTrendRowKey>
    {
        /// <summary>
        /// 单例
        /// </summary>
        public static PurchasePriceTrendRowKeyComparer Instance { get; } = new();

        /// <summary>
        /// 判断两行键是否相等（工厂/物料/供应商，忽略大小写）
        /// </summary>
        /// <param name="x">左值</param>
        /// <param name="y">右值</param>
        /// <returns>是否相等</returns>
        public bool Equals(PurchasePriceTrendRowKey? x, PurchasePriceTrendRowKey? y)
        {
            if (x is null || y is null)
            {
                return false;
            }
            return string.Equals(x.PlantCode, y.PlantCode, StringComparison.OrdinalIgnoreCase)
                && string.Equals(x.MaterialCode, y.MaterialCode, StringComparison.OrdinalIgnoreCase)
                && string.Equals(x.SupplierCode, y.SupplierCode, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 计算行键哈希（工厂/物料/供应商，忽略大小写）
        /// </summary>
        /// <param name="obj">行键</param>
        /// <returns>哈希码</returns>
        public int GetHashCode(PurchasePriceTrendRowKey obj) =>
            HashCode.Combine(
                StringComparer.OrdinalIgnoreCase.GetHashCode(obj.PlantCode),
                StringComparer.OrdinalIgnoreCase.GetHashCode(obj.MaterialCode),
                StringComparer.OrdinalIgnoreCase.GetHashCode(obj.SupplierCode));
    }

    /// <summary>
    /// 采购价格推移源行（主表 + 明细）
    /// </summary>
    private sealed class PurchasePriceTrendSourceRow
    {
        /// <summary>
        /// 采购价格主表
        /// </summary>
        public TaktPurchasePrice Master { get; init; } = null!;

        /// <summary>
        /// 采购价格明细
        /// </summary>
        public TaktPurchasePriceItem Item { get; init; } = null!;
    }
}
