// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：TaktBomVarianceCostTrendService.cs
// 创建时间：2026-08-07
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 差异成本推移（机种可多选；有无差异组件×移动单价月度推移）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Bom;
using Takt.Domain.Entities.Logistics.Manufacturing.Bom;
using Takt.Domain.Entities.Logistics.Materials;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Bom;

/// <summary>
/// BOM 差异成本推移分析服务（最后月相对前月有无差异组件；列为移动单价推移）
/// </summary>
public class TaktBomVarianceCostTrendService : TaktServiceBase, ITaktBomVarianceCostTrendService
{
    private const string BomItemYearShardBaseTable = "takt_logistics_manufacturing_bom_material_cost_item";

    private readonly ITaktCompanyRepository<TaktBomMaterialCostItem> _bomMaterialCostItemRepository;
    private readonly ITaktCompanyRepository<TaktBomMaterialCost> _bomMaterialCostRepository;
    private readonly ITaktTenantRepository<TaktModelDestination> _modelDestinationRepository;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="bomMaterialCostItemRepository">明细仓储</param>
    /// <param name="bomMaterialCostRepository">汇总仓储</param>
    /// <param name="modelDestinationRepository">型号目的地</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化</param>
    public TaktBomVarianceCostTrendService(
        ITaktCompanyRepository<TaktBomMaterialCostItem> bomMaterialCostItemRepository,
        ITaktCompanyRepository<TaktBomMaterialCost> bomMaterialCostRepository,
        ITaktTenantRepository<TaktModelDestination> modelDestinationRepository,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _bomMaterialCostItemRepository = bomMaterialCostItemRepository;
        _bomMaterialCostRepository = bomMaterialCostRepository;
        _modelDestinationRepository = modelDestinationRepository;
    }

    /// <inheritdoc />
    public async Task<List<TaktSelectOption>> GetBomVarianceCostTrendModelOptionsAsync(
        TaktBomVarianceCostTrendOptionsQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.PlantCode);
        EnsureThreeLayerContext();
        if (!TryParsePeriodMonth(queryDto.FocusPeriod, out var lastMonth))
        {
            return new List<TaktSelectOption>();
        }
        var type = NormalizeMaterialTypeFilter(queryDto.MaterialType);
        var periodKey = lastMonth.ToString("yyyy-MM");
        var plant = queryDto.PlantCode.Trim();
        var exp = Expressionable.Create<TaktBomMaterialCost>();
        exp = exp.And(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.PlantCode == plant
            && x.CostingPeriod == periodKey
            && x.ModelCode != null
            && x.ModelCode != "");
        if (type != null)
        {
            var mt = type;
            exp = exp.And(x => x.MaterialType == mt);
        }
        var headers = await _bomMaterialCostRepository.GetListAsync(exp.ToExpression());
        return headers
            .Select(h => h.ModelCode!.Trim())
            .Where(c => !string.IsNullOrWhiteSpace(c))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(c => c, StringComparer.Ordinal)
            .Select(code => new TaktSelectOption { DictValue = code, DictLabel = code })
            .ToList();
    }

    /// <inheritdoc />
    public async Task<List<TaktSelectOption>> GetBomVarianceCostTrendProductOptionsAsync(
        TaktBomVarianceCostTrendOptionsQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.PlantCode);
        EnsureThreeLayerContext();
        if (!TryParsePeriodMonth(queryDto.FocusPeriod, out var lastMonth))
        {
            return new List<TaktSelectOption>();
        }
        var modelCodes = ParseModelCodes(queryDto.ModelCodes, queryDto.ModelCode);
        if (modelCodes.Count == 0)
        {
            return new List<TaktSelectOption>();
        }
        var type = NormalizeMaterialTypeFilter(queryDto.MaterialType);
        var periodKey = lastMonth.ToString("yyyy-MM");
        var plant = queryDto.PlantCode.Trim();
        var modelSet = new HashSet<string>(modelCodes, StringComparer.OrdinalIgnoreCase);
        var exp = Expressionable.Create<TaktBomMaterialCost>();
        exp = exp.And(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.PlantCode == plant
            && x.CostingPeriod == periodKey
            && x.ProductCode != null
            && x.ProductCode != ""
            && x.ModelCode != null
            && x.ModelCode != "");
        if (type != null)
        {
            var mt = type;
            exp = exp.And(x => x.MaterialType == mt);
        }
        var headers = await _bomMaterialCostRepository.GetListAsync(exp.ToExpression());
        return headers
            .Where(h => !string.IsNullOrWhiteSpace(h.ProductCode)
                && !string.IsNullOrWhiteSpace(h.ModelCode)
                && modelSet.Contains(h.ModelCode.Trim()))
            .GroupBy(h => h.ProductCode!.Trim(), StringComparer.OrdinalIgnoreCase)
            .OrderBy(g => g.Key, StringComparer.Ordinal)
            .Select(g =>
            {
                var first = g.OrderByDescending(x => x.CostingDate).First();
                var description = first.ProductDescription?.Trim();
                var label = string.IsNullOrWhiteSpace(description) ? g.Key : $"{g.Key} - {description}";
                return new TaktSelectOption
                {
                    DictValue = g.Key,
                    DictLabel = label,
                    ExtValue = first.ModelCode?.Trim() ?? string.Empty,
                    ExtLabel = first.MaterialType?.Trim() ?? string.Empty,
                };
            })
            .ToList();
    }

    /// <inheritdoc />
    public async Task<TaktBomVarianceCostTrendResultDto> GetBomVarianceCostTrendAnalysisAsync(
        TaktBomVarianceCostTrendQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        var pageIndex = TaktPagedClamp.NormalizePageIndex(queryDto.PageIndex);
        var pageSize = TaktPagedClamp.NormalizePageSize(queryDto.PageSize);
        var skip = TaktPagedClamp.ComputeSkip(pageIndex, pageSize);
        var built = await BuildVarianceCostTrendAnalysisAsync(queryDto);
        var pageRows = built.OrderedRows.Skip(skip).Take(pageSize).ToList();
        return new TaktBomVarianceCostTrendResultDto
        {
            Paged = TaktPagedResult<TaktBomVarianceCostTrendDto>.Create(
                pageRows, built.OrderedRows.Count, pageIndex, pageSize),
            PeriodOrder = built.PeriodOrder,
            ProductCodes = built.ProductCodes,
            ComponentCount = built.OrderedRows.Count,
            BasePeriod = built.BasePeriod,
            ComparePeriod = built.ComparePeriod,
            UpCount = built.UpCount,
            DownCount = built.DownCount,
            FlatCount = built.FlatCount,
            NewCount = built.NewCount,
            RemovedCount = built.RemovedCount,
            VersionCount = built.VersionCount,
            NoneCount = built.NoneCount,
        };
    }

    /// <inheritdoc />
    public async Task<(string fileName, byte[] fileContent)> ExportBomVarianceCostTrendAnalysisAsync(
        TaktBomVarianceCostTrendQueryDto query,
        string? sheetName = null,
        string? fileName = null)
    {
        ArgumentNullException.ThrowIfNull(query);
        var built = await BuildVarianceCostTrendAnalysisAsync(query);
        var columnKeys = new List<string>
        {
            "plantCode", "modelCode", "productCode", "sequenceCode", "bomLevel", "bomItemCode",
            "previousComponentCode", "componentCode", "componentDescription", "componentQuantity",
            "productionRelated", "purchaseType",
        };
        var columnLabels = new List<string>
        {
            "工厂代码", "机种", "产品编码", "序号", "层级", "BOM项目",
            "基准月组件", "组件编码", "组件描述", "组件数量",
            "生产相关", "采购类型",
        };
        foreach (var period in built.PeriodOrder)
        {
            columnKeys.Add($"period_{period}");
            columnLabels.Add(period);
        }
        columnKeys.AddRange(new[] { "currencyCode", "basePeriod", "comparePeriod", "varianceAmount", "variancePercent", "trend" });
        columnLabels.AddRange(new[] { "货币", "基准月", "对比月", "移动价格差额", "环比%", "差异" });
        var exportRows = built.OrderedRows
            .Select(row =>
            {
                var dict = new Dictionary<string, object?>(StringComparer.Ordinal)
                {
                    ["plantCode"] = row.PlantCode,
                    ["modelCode"] = row.ModelCode,
                    ["productCode"] = row.ProductCode,
                    ["sequenceCode"] = row.SequenceCode,
                    ["bomLevel"] = row.BomLevel,
                    ["bomItemCode"] = row.BomItemCode,
                    ["previousComponentCode"] = row.PreviousComponentCode,
                    ["componentCode"] = row.ComponentCode,
                    ["componentDescription"] = row.ComponentDescription,
                    ["componentQuantity"] = row.ComponentQuantity,
                    ["productionRelated"] = row.ProductionRelated,
                    ["purchaseType"] = row.PurchaseType,
                    ["currencyCode"] = row.CurrencyCode,
                    ["basePeriod"] = row.BasePeriod,
                    ["comparePeriod"] = row.ComparePeriod,
                    ["varianceAmount"] = row.VarianceAmount,
                    ["variancePercent"] = TaktBomMaterialCostItemLineCostHelper.ToExcelPercent(row.VariancePercent),
                    ["trend"] = row.Trend,
                };
                foreach (var period in built.PeriodOrder)
                {
                    dict[$"period_{period}"] = row.PeriodMovingPrices.TryGetValue(period, out var price)
                        ? price
                        : null;
                }
                return (IReadOnlyDictionary<string, object?>)dict;
            })
            .ToList();
        const string defaultSheet = "DTA BOM组件差异成本推移表";
        return await TaktExcelHelper.ExportDictionaryRowsAsync(
            exportRows,
            columnKeys,
            columnLabels,
            sheetName ?? defaultSheet,
            fileName ?? $"{defaultSheet}.xlsx");
    }

    /// <summary>
    /// 构建差异成本推移（两端月均有明细才判定新增/剔除/版本变更；期间列为移动单价）
    /// </summary>
    /// <param name="queryDto">查询</param>
    /// <returns>构建结果</returns>
    private async Task<VarianceBuilt> BuildVarianceCostTrendAnalysisAsync(
        TaktBomVarianceCostTrendQueryDto queryDto)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.PlantCode);
        EnsureThreeLayerContext();
        var plantCode = queryDto.PlantCode.Trim();
        var materialType = NormalizeMaterialTypeFilter(queryDto.MaterialType) ?? "FERT";
        var (periodStart, periodEnd) = NormalizePeriodBounds(queryDto.PeriodDateStart, queryDto.PeriodDateEnd);
        if (!periodStart.HasValue && !periodEnd.HasValue)
        {
            return VarianceBuilt.Empty();
        }
        var rangeStart = periodStart ?? periodEnd!.Value;
        var rangeEnd = periodEnd ?? periodStart!.Value;
        var periodOrder = BuildPeriodOrder(Array.Empty<TaktBomMaterialCostItem>(), rangeStart, rangeEnd);
        if (periodOrder.Count < 2)
        {
            return VarianceBuilt.Empty();
        }
        var focusPeriod = ResolveFocusPeriod(queryDto.FocusPeriod, periodOrder);
        var basePeriod = ResolveBasePeriod(focusPeriod, periodOrder);
        if (string.IsNullOrWhiteSpace(focusPeriod)
            || string.IsNullOrWhiteSpace(basePeriod)
            || !TryParsePeriodMonth(basePeriod, out var baseMonth)
            || !TryParsePeriodMonth(focusPeriod, out var focusMonth))
        {
            return VarianceBuilt.Empty();
        }

        var modelCodes = ParseModelCodes(queryDto.ModelCodes, queryDto.ModelCode);
        // 未选机种 = 对比月全部机种（避免多选机种时 GET 查询串截断导致导出不全）
        if (modelCodes.Count == 0)
        {
            modelCodes = await LoadFocusMonthModelCodesAsync(plantCode, focusMonth, materialType);
        }
        if (modelCodes.Count == 0)
        {
            return VarianceBuilt.Empty();
        }

        var modelNameLookup = await BuildModelNameLookupAsync();
        var productCodeFilter = ParseModelCodes(queryDto.ProductCodes, queryDto.ProductCode);
        var productFilterSet = productCodeFilter.Count > 0
            ? new HashSet<string>(productCodeFilter, StringComparer.OrdinalIgnoreCase)
            : null;
        var allProductCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var allRows = new List<TaktBomVarianceCostTrendDto>();

        foreach (var modelCode in modelCodes)
        {
            var modelName = modelNameLookup.TryGetValue(modelCode, out var mn) && !string.IsNullOrWhiteSpace(mn)
                ? mn.Trim()
                : modelCode;
            // 产品宇宙：基准月 ∪ 对比月（缺一侧明细不算「新增」）；可选产品多选再收窄
            var productCodesBase = await LoadModelProductCodesAsync(
                plantCode, modelCode, baseMonth, baseMonth, materialType);
            var productCodesFocus = await LoadModelProductCodesAsync(
                plantCode, modelCode, focusMonth, focusMonth, materialType);
            var productCodes = productCodesBase
                .Concat(productCodesFocus)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Where(pc => productFilterSet == null || productFilterSet.Contains(pc))
                .OrderBy(c => c, StringComparer.Ordinal)
                .ToList();
            if (productCodes.Count == 0)
            {
                continue;
            }
            foreach (var pc in productCodes)
            {
                allProductCodes.Add(pc);
            }
            var costItems = await LoadBomCostItemsForProductsAsync(
                plantCode, productCodes, rangeStart, rangeEnd);
            if (costItems.Count == 0)
            {
                continue;
            }
            var hasBaseDetails = costItems.Any(r => ToPeriodKey(r.CostingDate) == basePeriod);
            var hasFocusDetails = costItems.Any(r => ToPeriodKey(r.CostingDate) == focusPeriod);
            // 任一月无明细：不产出新增/剔除/版本变更（缺数 ≠ 变更）
            if (!hasBaseDetails || !hasFocusDetails)
            {
                continue;
            }
            allRows.AddRange(
                BuildModelPresenceVarianceRows(
                    plantCode,
                    modelCode,
                    modelName,
                    costItems,
                    periodOrder,
                    focusPeriod,
                    basePeriod));
        }

        var filtered = FilterByTrend(allRows, queryDto.TrendFilter);
        var ordered = OrderVarianceCostTrendRows(filtered, queryDto.SortBy);
        var productList = allProductCodes.OrderBy(c => c, StringComparer.Ordinal).ToList();
        return new VarianceBuilt
        {
            OrderedRows = ordered,
            PeriodOrder = periodOrder,
            ProductCodes = productList,
            BasePeriod = basePeriod,
            ComparePeriod = focusPeriod,
            UpCount = 0,
            DownCount = 0,
            FlatCount = 0,
            NewCount = allRows.Count(r => r.Trend == "new"),
            RemovedCount = allRows.Count(r => r.Trend == "removed"),
            VersionCount = allRows.Count(r => r.Trend == "version"),
            NoneCount = 0,
        };
    }

    /// <summary>
    /// 槽位 ProductCode+SequenceCode+BomLevel+BomItemCode 内，逐一对比 ComponentCode：
    /// ① 编码完全相同→跳过；② 仅「末位版本字母」可配对的一部分→version；③ 其余→正常 new/removed。
    /// </summary>
    private static List<TaktBomVarianceCostTrendDto> BuildModelPresenceVarianceRows(
        string plantCode,
        string modelCode,
        string modelName,
        IReadOnlyList<TaktBomMaterialCostItem> costItems,
        IReadOnlyList<string> periodOrder,
        string focusPeriod,
        string basePeriod)
    {
        var rows = new List<TaktBomVarianceCostTrendDto>();
        var slots = costItems
            .Where(r => !string.IsNullOrWhiteSpace(r.ComponentCode))
            .GroupBy(r => BuildBomSlotKey(r), StringComparer.Ordinal)
            .OrderBy(g => g.Key, StringComparer.Ordinal);
        foreach (var slot in slots)
        {
            var slotItems = slot.ToList();
            var baseMap = BuildPeriodComponentMap(slotItems, basePeriod);
            var focusMap = BuildPeriodComponentMap(slotItems, focusPeriod);
            if (baseMap.Count == 0 && focusMap.Count == 0)
            {
                continue;
            }
            var matchedBase = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var matchedFocus = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            // ① 组件编码完全相同：两端都有 → 无有无差异
            foreach (var code in baseMap.Keys.ToList())
            {
                if (!focusMap.ContainsKey(code))
                {
                    continue;
                }
                matchedBase.Add(code);
                matchedFocus.Add(code);
            }

            // ② 仅一部分：双方均带末位版本字母 A～Z，且 stem 相同、字母不同 → 版本变更
            foreach (var focusCode in focusMap.Keys.Where(c => !matchedFocus.Contains(c)).ToList())
            {
                if (!TrySplitComponentVersion(focusCode, out var focusStem, out var focusLetter))
                {
                    continue; // 无版本字母 → 留给 ③ 正常新增
                }
                var baseCandidate = baseMap.Keys
                    .Where(c => !matchedBase.Contains(c))
                    .Select(c =>
                    {
                        var ok = TrySplitComponentVersion(c, out var stem, out var letter);
                        return (Code: c, Ok: ok, Stem: stem, Letter: letter);
                    })
                    .Where(x => x.Ok
                        && string.Equals(x.Stem, focusStem, StringComparison.OrdinalIgnoreCase)
                        && x.Letter != focusLetter)
                    .OrderBy(x => x.Code, StringComparer.OrdinalIgnoreCase)
                    .FirstOrDefault();
                if (baseCandidate.Code == null)
                {
                    continue; // 对不上版本对 → 留给 ③/④
                }
                matchedBase.Add(baseCandidate.Code);
                matchedFocus.Add(focusCode);
                var pairedItems = slotItems
                    .Where(r =>
                    {
                        var code = r.ComponentCode?.Trim() ?? string.Empty;
                        return string.Equals(code, baseCandidate.Code, StringComparison.OrdinalIgnoreCase)
                            || string.Equals(code, focusCode, StringComparison.OrdinalIgnoreCase);
                    })
                    .ToList();
                rows.Add(
                    BuildVarianceRow(
                        plantCode,
                        modelCode,
                        modelName,
                        pairedItems,
                        periodOrder,
                        focusPeriod,
                        basePeriod,
                        forceTrend: "version",
                        displayComponentCode: focusCode,
                        previousComponentCode: baseCandidate.Code,
                        identityPreferPeriod: focusPeriod));
            }

            // ③ 对比月未配对组件 → 正常新增（含无版本字母、或 stem 对不上的）
            foreach (var focusCode in focusMap.Keys.Where(c => !matchedFocus.Contains(c)))
            {
                var items = focusMap[focusCode];
                rows.Add(
                    BuildVarianceRow(
                        plantCode,
                        modelCode,
                        modelName,
                        items,
                        periodOrder,
                        focusPeriod,
                        basePeriod,
                        forceTrend: "new",
                        displayComponentCode: focusCode,
                        previousComponentCode: null,
                        identityPreferPeriod: focusPeriod));
            }

            // ④ 基准月未配对组件 → 正常剔除
            foreach (var baseCode in baseMap.Keys.Where(c => !matchedBase.Contains(c)))
            {
                var items = baseMap[baseCode];
                rows.Add(
                    BuildVarianceRow(
                        plantCode,
                        modelCode,
                        modelName,
                        items,
                        periodOrder,
                        focusPeriod,
                        basePeriod,
                        forceTrend: "removed",
                        displayComponentCode: baseCode,
                        previousComponentCode: null,
                        identityPreferPeriod: basePeriod));
            }
        }
        return rows;
    }

    /// <summary>
    /// 某月内同槽位：组件编码 → 明细行
    /// </summary>
    private static Dictionary<string, List<TaktBomMaterialCostItem>> BuildPeriodComponentMap(
        IReadOnlyList<TaktBomMaterialCostItem> slotItems,
        string periodKey)
    {
        return slotItems
            .Where(r => ToPeriodKey(r.CostingDate) == periodKey && !string.IsNullOrWhiteSpace(r.ComponentCode))
            .GroupBy(r => r.ComponentCode.Trim(), StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => g.ToList(), StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 构建单行差异（期间=移动单价；trend 由 forceTrend 指定）
    /// </summary>
    private static TaktBomVarianceCostTrendDto BuildVarianceRow(
        string plantCode,
        string modelCode,
        string modelName,
        IReadOnlyList<TaktBomMaterialCostItem> keyItems,
        IReadOnlyList<string> periodOrder,
        string? focusPeriod,
        string? basePeriod,
        string forceTrend,
        string displayComponentCode,
        string? previousComponentCode,
        string identityPreferPeriod)
    {
        var identity = keyItems
            .Where(r => ToPeriodKey(r.CostingDate) == identityPreferPeriod)
            .OrderByDescending(r => r.CostingDate)
            .ThenByDescending(r => r.Id)
            .FirstOrDefault()
            ?? keyItems
                .OrderByDescending(r => r.CostingDate)
                .ThenByDescending(r => r.Id)
                .First();
        var productCode = identity.ProductCode?.Trim() ?? string.Empty;
        var sequenceCode = identity.SequenceCode?.Trim() ?? string.Empty;
        var periodPrices = new Dictionary<string, decimal>(StringComparer.Ordinal);
        foreach (var period in periodOrder)
        {
            var price = ResolveMovingPriceForPeriod(keyItems, period);
            if (price == null)
            {
                continue;
            }
            periodPrices[period] = price.Value;
        }
        var changeTypes = BuildPeriodChangeTypes(periodOrder, periodPrices);
        ApplyMovingPriceFocusTrend(
            periodPrices,
            focusPeriod,
            basePeriod,
            out _,
            out var resolvedBase,
            out var resolvedCompare,
            out var varianceAmount,
            out var variancePercent);
        if (string.Equals(forceTrend, "version", StringComparison.Ordinal)
            && !string.IsNullOrWhiteSpace(resolvedBase)
            && !string.IsNullOrWhiteSpace(resolvedCompare))
        {
            changeTypes[resolvedBase!] = "version";
            changeTypes[resolvedCompare!] = "version";
        }
        decimal? focusPrice = null;
        decimal? focusQty = null;
        if (!string.IsNullOrWhiteSpace(resolvedCompare)
            && periodPrices.TryGetValue(resolvedCompare!, out var comparePrice))
        {
            focusPrice = comparePrice;
            focusQty = ResolveQuantityForPeriod(keyItems, resolvedCompare!);
        }
        else if (!string.IsNullOrWhiteSpace(resolvedBase)
            && periodPrices.TryGetValue(resolvedBase!, out var basePrice))
        {
            focusPrice = basePrice;
            focusQty = ResolveQuantityForPeriod(keyItems, resolvedBase!);
        }
        return new TaktBomVarianceCostTrendDto
        {
            PlantCode = plantCode,
            ModelCode = modelCode,
            ModelName = modelName,
            ProductCode = productCode,
            SequenceCode = sequenceCode,
            BomLevel = identity.BomLevel?.Trim() ?? string.Empty,
            BomItemCode = identity.BomItemCode?.Trim() ?? string.Empty,
            ComponentCode = displayComponentCode,
            PreviousComponentCode = previousComponentCode,
            ComponentDescription = identity.ComponentDescription?.Trim() ?? string.Empty,
            ComponentQuantity = focusQty,
            MovingPrice = focusPrice,
            CurrencyCode = identity.MovingPriceCurrencyCode?.Trim() ?? string.Empty,
            ProductionRelated = identity.ProductionRelated?.Trim(),
            PurchaseType = identity.PurchaseType?.Trim() ?? string.Empty,
            ProductCodes = productCode,
            ProductCount = string.IsNullOrEmpty(productCode) ? 0 : 1,
            PeriodMovingPrices = periodPrices,
            PeriodChangeTypes = changeTypes,
            Trend = forceTrend,
            BasePeriod = resolvedBase,
            ComparePeriod = resolvedCompare,
            VarianceAmount = varianceAmount,
            VariancePercent = variancePercent,
        };
    }

    /// <summary>
    /// 某月移动单价：核算日最新一行 → 移动平均价÷移动价格单位
    /// </summary>
    private static decimal? ResolveMovingPriceForPeriod(
        IReadOnlyList<TaktBomMaterialCostItem> keyItems,
        string periodKey)
    {
        var picked = keyItems
            .Where(r => ToPeriodKey(r.CostingDate) == periodKey)
            .OrderByDescending(r => r.CostingDate)
            .ThenByDescending(r => r.Id)
            .FirstOrDefault();
        if (picked == null)
        {
            return null;
        }
        return TaktBomMaterialCostItemLineCostHelper.ResolvePerBaseUnitPrice(picked);
    }

    /// <summary>
    /// 某月组件数量（展示用）
    /// </summary>
    private static decimal? ResolveQuantityForPeriod(
        IReadOnlyList<TaktBomMaterialCostItem> keyItems,
        string periodKey)
    {
        var picked = keyItems
            .Where(r => ToPeriodKey(r.CostingDate) == periodKey)
            .OrderByDescending(r => r.CostingDate)
            .ThenByDescending(r => r.Id)
            .FirstOrDefault();
        return picked == null ? null : picked.ComponentQuantity;
    }

    /// <summary>
    /// 各月有无/移动单价涨跌码
    /// </summary>
    private static Dictionary<string, string> BuildPeriodChangeTypes(
        IReadOnlyList<string> periodOrder,
        IReadOnlyDictionary<string, decimal> periodPrices)
    {
        var result = new Dictionary<string, string>(StringComparer.Ordinal);
        for (var i = 0; i < periodOrder.Count; i++)
        {
            var period = periodOrder[i];
            var hasCurrent = periodPrices.ContainsKey(period);
            var hasPrevious = i > 0 && periodPrices.ContainsKey(periodOrder[i - 1]);
            if (!hasCurrent && !hasPrevious)
            {
                result[period] = "absent";
                continue;
            }
            if (!hasCurrent && hasPrevious)
            {
                result[period] = "removed";
                continue;
            }
            if (hasCurrent && !hasPrevious)
            {
                result[period] = i == 0 ? "present" : "new";
                continue;
            }
            var current = periodPrices[period];
            var previous = periodPrices[periodOrder[i - 1]];
            if (current > previous)
            {
                result[period] = "up";
            }
            else if (current < previous)
            {
                result[period] = "down";
            }
            else
            {
                result[period] = "flat";
            }
        }
        return result;
    }

    /// <summary>
    /// 关注月相对基准月：推算移动单价差额（行级 Trend 由外层 forceTrend 指定）
    /// </summary>
    private static void ApplyMovingPriceFocusTrend(
        IReadOnlyDictionary<string, decimal> periodPrices,
        string? focusPeriod,
        string? basePeriodHint,
        out string trend,
        out string? basePeriod,
        out string? comparePeriod,
        out decimal? varianceAmount,
        out decimal? variancePercent)
    {
        trend = "none";
        basePeriod = null;
        comparePeriod = null;
        varianceAmount = null;
        variancePercent = null;
        if (string.IsNullOrWhiteSpace(focusPeriod))
        {
            return;
        }
        comparePeriod = focusPeriod.Trim();
        basePeriod = string.IsNullOrWhiteSpace(basePeriodHint) ? null : basePeriodHint.Trim();
        var hasCompare = periodPrices.TryGetValue(comparePeriod, out var comparePrice);
        decimal basePrice = 0m;
        var hasBase = !string.IsNullOrWhiteSpace(basePeriod)
            && periodPrices.TryGetValue(basePeriod!, out basePrice);
        if (hasCompare && !hasBase)
        {
            trend = "new";
            varianceAmount = comparePrice;
            return;
        }
        if (!hasCompare && hasBase)
        {
            trend = "removed";
            varianceAmount = TaktBomMaterialCostItemLineCostHelper.RoundCost(-basePrice);
            return;
        }
        if (!hasCompare || !hasBase)
        {
            return;
        }
        varianceAmount = TaktBomMaterialCostItemLineCostHelper.RoundCost(comparePrice - basePrice);
        if (basePrice != 0m)
        {
            variancePercent = TaktBomMaterialCostItemLineCostHelper.RoundPercentPoints(
                varianceAmount.Value / basePrice);
        }
        if (comparePrice > basePrice)
        {
            trend = "up";
        }
        else if (comparePrice < basePrice)
        {
            trend = "down";
        }
        else
        {
            trend = "flat";
        }
    }

    private static List<TaktBomVarianceCostTrendDto> FilterByTrend(
        IReadOnlyList<TaktBomVarianceCostTrendDto> rows,
        string? trendFilter)
    {
        if (string.IsNullOrWhiteSpace(trendFilter))
        {
            return rows.ToList();
        }
        var filter = trendFilter.Trim().ToLowerInvariant();
        if (filter is "changed" or "presence")
        {
            return rows.Where(r => r.Trend is "new" or "removed" or "version").ToList();
        }
        return rows.Where(r => string.Equals(r.Trend, filter, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    /// <summary>
    /// 差异成本推移全量排序（分页前）：trend（默认）/ varianceDesc / componentCode
    /// </summary>
    /// <param name="rows">已筛选行</param>
    /// <param name="sortBy">排序码</param>
    /// <returns>排序后列表</returns>
    private static List<TaktBomVarianceCostTrendDto> OrderVarianceCostTrendRows(
        IReadOnlyList<TaktBomVarianceCostTrendDto> rows,
        string? sortBy)
    {
        static int Rank(string? trend) => trend switch
        {
            "new" => 0,
            "removed" => 1,
            "version" => 2,
            _ => 3,
        };
        var mode = (sortBy ?? string.Empty).Trim().ToLowerInvariant();
        IOrderedEnumerable<TaktBomVarianceCostTrendDto> ordered = mode switch
        {
            "variancedesc" => rows
                .OrderByDescending(r => Math.Abs(r.VarianceAmount ?? 0m))
                .ThenBy(r => Rank(r.Trend)),
            "componentcode" => rows.OrderBy(r => r.ComponentCode, StringComparer.Ordinal),
            _ => rows.OrderBy(r => Rank(r.Trend)), // trend 默认
        };
        return ordered
            .ThenBy(r => r.ModelCode, StringComparer.Ordinal)
            .ThenBy(r => r.ProductCode, StringComparer.Ordinal)
            .ThenBy(r => r.SequenceCode, StringComparer.Ordinal)
            .ThenBy(r => r.BomLevel, StringComparer.Ordinal)
            .ThenBy(r => r.BomItemCode, StringComparer.Ordinal)
            .ThenBy(r => r.ComponentCode, StringComparer.Ordinal)
            .ToList();
    }

    /// <summary>
    /// 差异类型默认序（兼容旧调用名）
    /// </summary>
    /// <param name="rows">行</param>
    /// <returns>排序后列表</returns>
    private static List<TaktBomVarianceCostTrendDto> OrderByTrend(
        IReadOnlyList<TaktBomVarianceCostTrendDto> rows)
        => OrderVarianceCostTrendRows(rows, "trend");

    /// <summary>
    /// 解析机种多选（ModelCodes + 兼容 ModelCode）
    /// </summary>
    /// <param name="multiCodes">多选逗号串</param>
    /// <param name="singleCode">兼容单值</param>
    /// <returns>去重后的机种列表</returns>
    private static List<string> ParseModelCodes(string? multiCodes, string? singleCode)
    {
        var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        void AddRaw(string? raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
            {
                return;
            }
            foreach (var part in raw.Split(
                         new[] { ',', ';' },
                         StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            {
                if (!string.IsNullOrWhiteSpace(part))
                {
                    set.Add(part);
                }
            }
        }
        AddRaw(multiCodes);
        AddRaw(singleCode);
        return set.OrderBy(c => c, StringComparer.Ordinal).ToList();
    }

    /// <summary>
    /// 结构槽位键：产品 + 序号 + 层级 + BOM 项目号；组件编码在槽内逐一对比
    /// </summary>
    private static string BuildBomSlotKey(TaktBomMaterialCostItem item) =>
        string.Join(
            "|",
            item.ProductCode?.Trim() ?? string.Empty,
            item.SequenceCode?.Trim() ?? string.Empty,
            item.BomLevel?.Trim() ?? string.Empty,
            item.BomItemCode?.Trim() ?? string.Empty);

    /// <summary>
    /// 拆组件版本：仅末位 A～Z 视为版本字母；无则返回 false（走正常新增/剔除，不参与版本配对）
    /// </summary>
    private static bool TrySplitComponentVersion(
        string componentCode,
        out string stem,
        out char versionLetter)
    {
        stem = string.Empty;
        versionLetter = '\0';
        if (string.IsNullOrWhiteSpace(componentCode) || componentCode.Length < 2)
        {
            return false;
        }
        var code = componentCode.Trim();
        var last = char.ToUpperInvariant(code[^1]);
        if (last is < 'A' or > 'Z')
        {
            return false;
        }
        stem = code[..^1];
        versionLetter = last;
        return stem.Length > 0;
    }

    private static string? NormalizeMaterialTypeFilter(string? materialType)
    {
        var trimmed = materialType?.Trim();
        return string.IsNullOrEmpty(trimmed) ? null : trimmed;
    }

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
        return (start, end);
    }

    private static bool TryParsePeriodMonth(string? period, out DateTime monthStart)
    {
        monthStart = default;
        if (string.IsNullOrWhiteSpace(period))
        {
            return false;
        }
        if (!DateTime.TryParseExact(
                period.Trim() + "-01",
                "yyyy-MM-dd",
                null,
                System.Globalization.DateTimeStyles.None,
                out monthStart))
        {
            return false;
        }
        monthStart = new DateTime(monthStart.Year, monthStart.Month, 1);
        return true;
    }

    private static string ToPeriodKey(DateTime costingDate) =>
        new DateTime(costingDate.Year, costingDate.Month, 1).ToString("yyyy-MM");

    private static List<string> BuildPeriodOrder(
        IReadOnlyList<TaktBomMaterialCostItem> items,
        DateTime rangeStart,
        DateTime rangeEnd)
    {
        var ordered = new List<string>();
        for (var m = rangeStart; m <= rangeEnd; m = m.AddMonths(1))
        {
            ordered.Add(m.ToString("yyyy-MM"));
        }
        if (ordered.Count > 0)
        {
            return ordered;
        }
        return items
            .Select(r => ToPeriodKey(r.CostingDate))
            .Distinct(StringComparer.Ordinal)
            .OrderBy(c => c, StringComparer.Ordinal)
            .ToList();
    }

    private static string? ResolveFocusPeriod(string? focusPeriod, IReadOnlyList<string> periodOrder)
    {
        if (!string.IsNullOrWhiteSpace(focusPeriod)
            && periodOrder.Contains(focusPeriod.Trim(), StringComparer.Ordinal))
        {
            return focusPeriod.Trim();
        }
        return periodOrder.Count > 0 ? periodOrder[^1] : null;
    }

    private static string? ResolveBasePeriod(string? focusPeriod, IReadOnlyList<string> periodOrder)
    {
        if (string.IsNullOrWhiteSpace(focusPeriod) || periodOrder.Count < 2)
        {
            return periodOrder.Count >= 2 ? periodOrder[^2] : null;
        }
        var idx = periodOrder.ToList().FindIndex(p =>
            string.Equals(p, focusPeriod, StringComparison.Ordinal));
        if (idx > 0)
        {
            return periodOrder[idx - 1];
        }
        return periodOrder.Count >= 2 ? periodOrder[^2] : null;
    }

    private async Task<Dictionary<string, string>> BuildModelNameLookupAsync()
    {
        var list = await _modelDestinationRepository.GetListAsync(x => x.TenantCode == CurrentTenantCode);
        return list
            .Where(x => !string.IsNullOrWhiteSpace(x.ModelCode))
            .GroupBy(x => x.ModelCode!.Trim(), StringComparer.OrdinalIgnoreCase)
            .ToDictionary(
                g => g.Key,
                g => g.First().ModelName?.Trim() ?? g.Key,
                StringComparer.OrdinalIgnoreCase);
    }

    private async Task<List<string>> LoadFocusMonthModelCodesAsync(
        string plantCode,
        DateTime focusMonth,
        string materialType)
    {
        var periodKey = focusMonth.ToString("yyyy-MM");
        var exp = Expressionable.Create<TaktBomMaterialCost>();
        exp = exp.And(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.PlantCode == plantCode
            && x.CostingPeriod == periodKey
            && x.MaterialType == materialType
            && x.ModelCode != null
            && x.ModelCode != "");
        var headers = await _bomMaterialCostRepository.GetListAsync(exp.ToExpression());
        return headers
            .Select(h => h.ModelCode!.Trim())
            .Where(c => !string.IsNullOrWhiteSpace(c))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(c => c, StringComparer.Ordinal)
            .ToList();
    }

    private async Task<List<string>> LoadModelProductCodesAsync(
        string plantCode,
        string modelCode,
        DateTime costingMonthStart,
        DateTime costingMonthEnd,
        string materialType)
    {
        var startPeriod = costingMonthStart.ToString("yyyy-MM");
        var endPeriod = costingMonthEnd.ToString("yyyy-MM");
        var headers = await _bomMaterialCostRepository.GetListAsync(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.PlantCode == plantCode
            && x.ModelCode == modelCode
            && x.MaterialType == materialType
            && x.CostingPeriod != null
            && x.CostingPeriod.CompareTo(startPeriod) >= 0
            && x.CostingPeriod.CompareTo(endPeriod) <= 0);
        return headers
            .Where(h => !string.IsNullOrWhiteSpace(h.ProductCode))
            .Select(h => h.ProductCode.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(c => c, StringComparer.Ordinal)
            .ToList();
    }

    private async Task<List<TaktBomMaterialCostItem>> LoadBomCostItemsForProductsAsync(
        string plantCode,
        IReadOnlyList<string> productCodes,
        DateTime costingMonthStart,
        DateTime costingMonthEnd)
    {
        var allItems = new List<TaktBomMaterialCostItem>();
        const int chunkSize = 200;
        var costingExclusiveEnd = costingMonthEnd.AddMonths(1);
        var lookupCodes = productCodes
            .SelectMany(TaktBomMaterialCostItemLineCostHelper.ExpandProductCodeLookupVariants)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        for (var i = 0; i < lookupCodes.Count; i += chunkSize)
        {
            var chunk = lookupCodes.Skip(i).Take(chunkSize).ToList();
            var exp = Expressionable.Create<TaktBomMaterialCostItem>();
            exp = exp.And(x =>
                x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode == plantCode
                && x.ProductionRelated == "X"
                && x.PurchaseType == "F"
                && chunk.Contains(x.ProductCode)
                && x.CostingDate >= costingMonthStart
                && x.CostingDate < costingExclusiveEnd);
            var part = await GetBomItemListForRangeAsync(
                exp.ToExpression(), costingMonthStart, costingMonthEnd);
            allItems.AddRange(TaktBomMaterialCostItemLineCostHelper.FilterBomMaterialCostItemRows(part));
        }
        return allItems;
    }

    private static string BuildBomItemYearTable(int year) =>
        TaktYearShardTableHelper.BuildYearTableName(BomItemYearShardBaseTable, year);

    private async Task<string?> ResolveBomItemPhysicalTableAsync(int year)
    {
        var table = BuildBomItemYearTable(year);
        return await _bomMaterialCostItemRepository.PhysicalTableExistsAsync(table) ? table : null;
    }

    private async Task<List<TaktBomMaterialCostItem>> GetBomItemListForRangeAsync(
        Expression<Func<TaktBomMaterialCostItem, bool>> predicate,
        DateTime? start,
        DateTime? end)
    {
        var years = TaktYearShardTableHelper.ResolveYears(start, end);
        var result = new List<TaktBomMaterialCostItem>();
        var seenIds = new HashSet<long>();
        void Append(IEnumerable<TaktBomMaterialCostItem> rows)
        {
            foreach (var row in rows)
            {
                if (!seenIds.Add(row.Id))
                {
                    continue;
                }
                result.Add(row);
            }
        }
        foreach (var year in years)
        {
            var table = await ResolveBomItemPhysicalTableAsync(year);
            if (table == null)
            {
                continue;
            }
            Append(await _bomMaterialCostItemRepository.GetListAsync(predicate, table));
        }
        // 年分表与基表合并：SAP 同步常写基表，年分表可能仅部分数据；按 Id 去重
        var basePart = await _bomMaterialCostItemRepository.GetListAsync(predicate);
        var yearSet = years.ToHashSet();
        Append(basePart.Where(r => yearSet.Contains(r.CostingDate.Year)));
        return result;
    }

    /// <summary>
    /// 内存构建结果
    /// </summary>
    private sealed class VarianceBuilt
    {
        public List<TaktBomVarianceCostTrendDto> OrderedRows { get; init; } = new();
        public List<string> PeriodOrder { get; init; } = new();
        public List<string> ProductCodes { get; init; } = new();
        public string? BasePeriod { get; init; }
        public string? ComparePeriod { get; init; }
        public int UpCount { get; init; }
        public int DownCount { get; init; }
        public int FlatCount { get; init; }
        public int NewCount { get; init; }
        public int RemovedCount { get; init; }
        public int VersionCount { get; init; }
        public int NoneCount { get; init; }

        public static VarianceBuilt Empty(List<string>? productCodes = null) => new()
        {
            ProductCodes = productCodes ?? new List<string>(),
        };
    }
}
