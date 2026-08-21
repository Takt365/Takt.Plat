// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：TaktBomPriceDeltaTrendService.cs
// 创建时间：2026-08-13
// 创建人：Takt365(Cursor AI)
// 功能描述：成本差异推移应用服务（独立；产品月成本+0价格组+价格差异组）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Globalization;
using System.Linq.Expressions;
using System.Text;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Bom;
using Takt.Domain.Entities.Accounting.Financial;
using Takt.Domain.Entities.Logistics.Manufacturing.Bom;
using Takt.Domain.Entities.Logistics.Materials;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Bom;

/// <summary>
/// 成本差异推移服务（与成本分析分离）
/// </summary>
public class TaktBomPriceDeltaTrendService : TaktServiceBase, ITaktBomPriceDeltaTrendService
{
    /// <summary>移动价格候选查询分批上限（仅替代价查找；列表产品行数不设上限）</summary>
    private const int MovingPriceLookupMaxRows = 20000;
    /// <summary>列表期间列上限（防横向/JSON 溢出）</summary>
    private const int MaxPeriodMonths = 24;
    /// <summary>列表页组文字段截断长度（导出保留全文）</summary>
    private const int MaxGroupTextLength = 2000;
    private const string BomItemYearShardBaseTable = "takt_logistics_manufacturing_bom_material_cost_item";
    private const string MovingPriceYearShardBaseTable = "takt_logistics_materials_material_moving_price";

    private readonly ITaktCompanyRepository<TaktBomMaterialCostItem> _bomMaterialCostItemRepository;
    private readonly ITaktCompanyRepository<TaktBomMaterialCost> _bomMaterialCostRepository;
    private readonly ITaktCompanyRepository<TaktMaterialMovingPrice> _materialMovingPriceRepository;
    private readonly ITaktTenantRepository<TaktCompany> _companyRepository;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="bomMaterialCostItemRepository">BOM 成本明细仓储</param>
    /// <param name="bomMaterialCostRepository">BOM 成本汇总仓储</param>
    /// <param name="materialMovingPriceRepository">移动价格仓储（0价格组可替代价）</param>
    /// <param name="companyRepository">公司仓储（RelatedPlant）</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktBomPriceDeltaTrendService(
        ITaktCompanyRepository<TaktBomMaterialCostItem> bomMaterialCostItemRepository,
        ITaktCompanyRepository<TaktBomMaterialCost> bomMaterialCostRepository,
        ITaktCompanyRepository<TaktMaterialMovingPrice> materialMovingPriceRepository,
        ITaktTenantRepository<TaktCompany> companyRepository,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _bomMaterialCostItemRepository = bomMaterialCostItemRepository;
        _bomMaterialCostRepository = bomMaterialCostRepository;
        _materialMovingPriceRepository = materialMovingPriceRepository;
        _companyRepository = companyRepository;
    }

    /// <summary>
    /// 查询栏工厂选项：当前公司 RelatedPlant ∩ 成本主表 PlantCode
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetBomPriceDeltaTrendPlantOptionsAsync()
    {
        EnsureThreeLayerContext();
        var companies = await _companyRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode);
        var relatedPlant = companies
            .Select(c => c.RelatedPlant?.Trim() ?? string.Empty)
            .FirstOrDefault(p => !string.IsNullOrEmpty(p))
            ?? string.Empty;
        if (string.IsNullOrEmpty(relatedPlant))
        {
            return new List<TaktSelectOption>();
        }
        var costPlants = await _bomMaterialCostRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode == relatedPlant);
        if (costPlants.Count == 0)
        {
            return new List<TaktSelectOption>();
        }
        return new List<TaktSelectOption>
        {
            new() { DictValue = relatedPlant, DictLabel = relatedPlant },
        };
    }

    /// <summary>
    /// 成本差异推移列表
    /// </summary>
    /// <param name="queryDto">查询</param>
    /// <returns>分页结果</returns>
    public async Task<TaktBomPriceDeltaTrendResultDto> GetBomPriceDeltaTrendListAsync(
        TaktBomPriceDeltaTrendQueryDto queryDto)
    {
        return await BuildBomPriceDeltaTrendResultAsync(
            queryDto,
            forExport: false,
            truncateGroupTexts: true);
    }

    /// <summary>
    /// 导出成本差异推移（全量产品，不分页截断）
    /// </summary>
    /// <param name="query">查询</param>
    /// <param name="sheetName">工作表名</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportBomPriceDeltaTrendAsync(
        TaktBomPriceDeltaTrendQueryDto query,
        string? sheetName = null,
        string? fileName = null)
    {
        ArgumentNullException.ThrowIfNull(query);
        var result = await BuildBomPriceDeltaTrendResultAsync(
            query,
            forExport: true,
            truncateGroupTexts: false);
        var columnKeys = new List<string> { "modelCode", "productCode", "productDescription" };
        var columnLabels = new List<string> { "机种", "产品", "产品描述" };
        foreach (var period in result.PeriodOrder)
        {
            columnKeys.Add($"period_{period}");
            columnLabels.Add(period);
        }
        columnKeys.Add("priceDelta");
        columnLabels.Add("差异");
        columnKeys.Add("zeroPriceGroup");
        columnLabels.Add("0价格组");
        columnKeys.Add("priceDeltaTrend");
        columnLabels.Add("价格差异组");
        columnKeys.Add("componentDeltaGroup");
        columnLabels.Add("组件差异");
        var exportRows = result.Paged.Data.Select(row =>
        {
            var dict = new Dictionary<string, object?>(StringComparer.Ordinal)
            {
                ["modelCode"] = row.ModelCode,
                ["productCode"] = row.ProductCode,
                ["productDescription"] = row.ProductDescription,
                ["priceDelta"] = row.PriceDelta,
                ["zeroPriceGroup"] = row.ZeroPriceGroup,
                ["priceDeltaTrend"] = row.PriceDeltaTrend,
                ["componentDeltaGroup"] = row.ComponentDeltaGroup,
            };
            foreach (var period in result.PeriodOrder)
            {
                row.PeriodCosts.TryGetValue(period, out var cost);
                dict[$"period_{period}"] = cost;
            }
            return (IReadOnlyDictionary<string, object?>)dict;
        }).ToList();
        return await TaktExcelHelper.ExportDictionaryRowsAsync(
            exportRows,
            columnKeys,
            columnLabels,
            sheetName ?? "DTA BOM成本差异推移",
            fileName ?? "DTA BOM成本差异推移.xlsx");
    }

    /// <summary>
    /// 构建列表/导出结果：按产品目录分页（列表）；导出取全量产品，无行数上限
    /// </summary>
    /// <param name="queryDto">查询</param>
    /// <param name="forExport">true=全量行；false=服务端分页</param>
    /// <param name="truncateGroupTexts">列表截断超长组文</param>
    /// <returns>结果</returns>
    private async Task<TaktBomPriceDeltaTrendResultDto> BuildBomPriceDeltaTrendResultAsync(
        TaktBomPriceDeltaTrendQueryDto queryDto,
        bool forExport,
        bool truncateGroupTexts)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.PlantCode);
        EnsureThreeLayerContext();

        var pageIndex = forExport ? 1 : TaktPagedClamp.NormalizePageIndex(queryDto.PageIndex);
        var pageSize = forExport
            ? TaktPagedClamp.DefaultPageSize
            : TaktPagedClamp.NormalizePageSize(
                queryDto.PageSize <= 0 ? TaktPagedClamp.DefaultPageSize : queryDto.PageSize);

        var plantCode = queryDto.PlantCode.Trim();
        var materialType = string.IsNullOrWhiteSpace(queryDto.MaterialType)
            ? TaktBomMaterialCostItemLineCostHelper.FertMaterialTypeCode
            : queryDto.MaterialType.Trim();
        if (!queryDto.CostingDateStart.HasValue || !queryDto.CostingDateEnd.HasValue)
        {
            throw new TaktBusinessException("请选择核算期间");
        }

        // 按核算月整月归一（起=月初，止=月末 23:59:59.999），避免前端 yyyy-MM-dd 绑成 00:00:00 截断当日
        var (rangeStart, rangeEnd) = NormalizeCostingDateRange(
            queryDto.CostingDateStart.Value,
            queryDto.CostingDateEnd.Value);
        var periodOrder = BuildPeriodOrder(rangeStart, rangeEnd);
        if (periodOrder.Count == 0)
        {
            return EmptyResult(pageIndex, pageSize);
        }
        if (periodOrder.Count > MaxPeriodMonths)
        {
            throw new TaktBusinessException(
                $"核算期间最多 {MaxPeriodMonths} 个月（当前 {periodOrder.Count} 个月），请缩小期间以免列表溢出");
        }

        // 差异/组对比：期间最大月 vs 前一月（前一月可能不在展示期间内，仍须加载）
        var (basePeriod, comparePeriod) = ResolveComparePeriods(periodOrder);
        var headers = await LoadCostHeadersAsync(
            queryDto, plantCode, materialType, periodOrder, basePeriod, rangeStart, rangeEnd);
        var productGroups = headers
            .Where(r => !string.IsNullOrWhiteSpace(r.ProductCode))
            .GroupBy(r => NormalizeProductKey(r.ProductCode!), StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => g.ToList(), StringComparer.OrdinalIgnoreCase);

        var catalog = productGroups
            .Where(kv => kv.Value.Any(r => periodOrder.Any(p => HeaderMatchesPeriod(r, p))))
            .Select(kv =>
            {
                var latest = kv.Value.OrderByDescending(r => r.CostingDate).First();
                return (
                    ProductKey: kv.Key,
                    ProductCode: latest.ProductCode?.Trim() ?? kv.Key,
                    ProductDescription: latest.ProductDescription?.Trim() ?? string.Empty,
                    Rows: kv.Value);
            })
            .OrderBy(x => x.ProductCode, StringComparer.Ordinal)
            .ToList();

        if (!string.IsNullOrWhiteSpace(queryDto.ProductCode))
        {
            var productFilter = queryDto.ProductCode.Trim();
            catalog = catalog
                .Where(p => TaktBomMaterialCostItemLineCostHelper.ProductCodeMatches(p.ProductCode, productFilter))
                .ToList();
        }

        var total = catalog.Count;
        List<(string ProductKey, string ProductCode, string ProductDescription, List<TaktBomMaterialCost> Rows)> pageCatalog;
        if (forExport)
        {
            pageCatalog = catalog;
            pageSize = total <= 0 ? TaktPagedClamp.DefaultPageSize : total;
        }
        else
        {
            var skip = TaktPagedClamp.ComputeSkip(pageIndex, pageSize);
            pageCatalog = catalog.Skip(skip).Take(pageSize).ToList();
        }

        var pageRows = pageCatalog
            .Select(item => BuildProductRow(
                plantCode,
                item.ProductCode,
                item.ProductDescription,
                item.Rows,
                periodOrder,
                basePeriod,
                comparePeriod))
            .ToList();

        // 仅当前页（或导出全量页）填充组文，避免未展示行做 BOM 明细全量计算
        await FillComponentGroupTextsAsync(plantCode, pageRows, basePeriod, comparePeriod);

        if (truncateGroupTexts)
        {
            foreach (var row in pageRows)
            {
                TruncateRowGroupTextsForDisplay(row);
            }
        }

        return new TaktBomPriceDeltaTrendResultDto
        {
            Paged = TaktPagedResult<TaktBomPriceDeltaTrendDto>.Create(
                pageRows, total, pageIndex, forExport ? Math.Max(pageSize, 1) : pageSize),
            PeriodOrder = periodOrder,
            BasePeriod = basePeriod,
            ComparePeriod = comparePeriod,
        };
    }

    /// <summary>
    /// 列表页截断超长组文，防止单元格/JSON 撑爆
    /// </summary>
    private static void TruncateRowGroupTextsForDisplay(TaktBomPriceDeltaTrendDto row)
    {
        row.ZeroPriceGroup = TruncateDisplayText(row.ZeroPriceGroup);
        row.PriceDeltaTrend = TruncateDisplayText(row.PriceDeltaTrend);
        row.ComponentDeltaGroup = TruncateDisplayText(row.ComponentDeltaGroup);
    }

    /// <summary>
    /// 截断展示文本
    /// </summary>
    private static string TruncateDisplayText(string? value)
    {
        if (string.IsNullOrEmpty(value) || value.Length <= MaxGroupTextLength)
        {
            return value ?? string.Empty;
        }
        return value[..MaxGroupTextLength] + "…";
    }

    /// <summary>
    /// 核算日起止归一为整月（起日 00:00:00，止日 23:59:59.999）
    /// </summary>
    /// <param name="start">查询起</param>
    /// <param name="end">查询止</param>
    /// <returns>月初、月末末刻</returns>
    private static (DateTime RangeStart, DateTime RangeEnd) NormalizeCostingDateRange(
        DateTime start,
        DateTime end)
    {
        var rangeStart = new DateTime(start.Year, start.Month, 1);
        var endMonth = new DateTime(end.Year, end.Month, 1);
        if (endMonth < rangeStart)
        {
            endMonth = rangeStart;
        }
        var lastDay = DateTime.DaysInMonth(endMonth.Year, endMonth.Month);
        var rangeEnd = new DateTime(endMonth.Year, endMonth.Month, lastDay, 23, 59, 59, 999);
        return (rangeStart, rangeEnd);
    }

    private async Task<List<TaktBomMaterialCost>> LoadCostHeadersAsync(
        TaktBomPriceDeltaTrendQueryDto queryDto,
        string plantCode,
        string materialType,
        IReadOnlyList<string> periodOrder,
        string? basePeriod,
        DateTime rangeStart,
        DateTime rangeEnd)
    {
        var start = rangeStart;
        var end = rangeEnd;
        var loadPeriods = periodOrder.ToList();
        // 前一月不在查询期间内时，仍纳入主表加载，供「差异=最大月−前一月」取数
        if (!string.IsNullOrWhiteSpace(basePeriod)
            && TryParsePeriodMonth(basePeriod, out var baseMonth))
        {
            var baseMonthStart = new DateTime(baseMonth.Year, baseMonth.Month, 1);
            if (baseMonthStart < start)
            {
                start = baseMonthStart;
            }
            if (!loadPeriods.Contains(basePeriod, StringComparer.Ordinal))
            {
                loadPeriods.Insert(0, basePeriod);
            }
        }
        var fert = TaktBomMaterialCostItemLineCostHelper.FertMaterialTypeCode;
        var exp = Expressionable.Create<TaktBomMaterialCost>();
        // 按 CostingDate 整月窗口，或 CostingPeriod∈期间（防止核算日落在窗口外但期间列有值）
        exp = exp.And(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.PlantCode == plantCode
            && x.IsDeleted == 0
            && ((x.CostingDate >= start && x.CostingDate <= end)
                || loadPeriods.Contains(x.CostingPeriod)));
        // FERT：空物料类型与 FERT 同等（避免七月行 MaterialType 空串被过滤掉）
        if (string.Equals(materialType, fert, StringComparison.OrdinalIgnoreCase))
        {
            exp = exp.And(x =>
                x.MaterialType == materialType
                || x.MaterialType == null
                || x.MaterialType == string.Empty);
        }
        else
        {
            exp = exp.And(x => x.MaterialType == materialType);
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ModelCode))
        {
            var model = queryDto.ModelCode.Trim();
            exp = exp.And(x => x.ModelCode == model);
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ProductCode))
        {
            var product = queryDto.ProductCode.Trim();
            exp = exp.And(x => x.ProductCode != null && x.ProductCode.Contains(product));
        }
        var rows = await _bomMaterialCostRepository.GetListAsync(exp.ToExpression());
        // 月份列只允许用 ProductMonthlyCalculation；加载后清零机种月成本，杜绝任何误读 ModelMonthlyAverageCost
        foreach (var row in rows)
        {
            row.ModelMonthlyAverageCost = 0m;
        }
        return rows;
    }

    private static List<string> BuildPeriodOrder(DateTime start, DateTime end)
    {
        var cursor = new DateTime(start.Year, start.Month, 1);
        var last = new DateTime(end.Year, end.Month, 1);
        var list = new List<string>();
        while (cursor <= last)
        {
            list.Add($"{cursor.Year:D4}-{cursor.Month:D2}");
            cursor = cursor.AddMonths(1);
        }
        return list;
    }

    /// <summary>
    /// 对比月 = 查询期间最大月；基准月 = 前一月（供差异与组文）
    /// </summary>
    private static (string? BasePeriod, string? ComparePeriod) ResolveComparePeriods(
        IReadOnlyList<string> periodOrder)
    {
        if (periodOrder.Count == 0)
        {
            return (null, null);
        }
        var compare = periodOrder[^1];
        if (!TryParsePeriodMonth(compare, out var compareMonth))
        {
            return (null, null);
        }
        var basePeriod = compareMonth.AddMonths(-1).ToString("yyyy-MM");
        return (basePeriod, compare);
    }

    private static TaktBomPriceDeltaTrendDto BuildProductRow(
        string plantCode,
        string productCode,
        string productDescription,
        List<TaktBomMaterialCost> productRows,
        IReadOnlyList<string> periodOrder,
        string? basePeriod,
        string? comparePeriod)
    {
        var periodCosts = new Dictionary<string, decimal>(StringComparer.Ordinal);
        foreach (var period in periodOrder)
        {
            // 只取 product_monthly_calculation；ModelMonthlyAverageCost 在 Load 时已清零
            var productMonthlyCost = ResolveProductMonthlyCostColumn(productRows, period);
            if (productMonthlyCost > 0m)
            {
                periodCosts[period] = productMonthlyCost;
            }
        }
        // 展示用机种/描述取期间内最新核算日行（HeaderMatchesPeriod：CostingPeriod 或 CostingDate 月）
        var latestInDisplay = productRows
            .Where(r => periodOrder.Any(p => HeaderMatchesPeriod(r, p)))
            .OrderByDescending(r => r.CostingDate)
            .FirstOrDefault()
            ?? productRows.OrderByDescending(r => r.CostingDate).FirstOrDefault();
        decimal? priceDelta = null;
        if (!string.IsNullOrWhiteSpace(basePeriod) && !string.IsNullOrWhiteSpace(comparePeriod))
        {
            var baseCost = ResolveProductMonthlyCostColumn(productRows, basePeriod);
            var compareCost = ResolveProductMonthlyCostColumn(productRows, comparePeriod);
            // 比较月或基准月产品成本为 0：标记跳过（PriceDelta=null）；有价时先写主表差，Fill 再按明细覆盖
            if (baseCost > 0m && compareCost > 0m)
            {
                priceDelta = TaktBomMaterialCostItemLineCostHelper.RoundCost(compareCost - baseCost);
            }
        }
        return new TaktBomPriceDeltaTrendDto
        {
            PlantCode = plantCode,
            ModelCode = latestInDisplay?.ModelCode?.Trim() ?? string.Empty,
            ProductCode = productCode,
            ProductDescription = string.IsNullOrWhiteSpace(productDescription)
                ? (latestInDisplay?.ProductDescription?.Trim() ?? string.Empty)
                : productDescription,
            PeriodCosts = periodCosts,
            PriceDelta = priceDelta,
            BasePeriod = basePeriod,
            ComparePeriod = comparePeriod,
        };
    }

    /// <summary>
    /// 取指定核算月的 product_monthly_calculation（实体属性 ProductMonthlyCalculation）。
    /// ❌ 绝不读取 ModelMonthlyAverageCost / model_monthly_average_cost。
    /// </summary>
    /// <param name="productRows">该产品主表行</param>
    /// <param name="period">yyyy-MM</param>
    /// <returns>产品月成本；无正价为 0</returns>
    private static decimal ResolveProductMonthlyCostColumn(
        List<TaktBomMaterialCost> productRows,
        string period)
    {
        var header = productRows
            .Where(r => HeaderMatchesPeriod(r, period))
            .OrderByDescending(r => r.ProductMonthlyCalculation)
            .ThenByDescending(r => r.CostingDate)
            .ThenByDescending(r => r.Id)
            .FirstOrDefault();
        if (header == null)
        {
            return 0m;
        }
        // 唯一取价字段：product_monthly_calculation
        var productMonthlyCost = header.ProductMonthlyCalculation;
        return productMonthlyCost > 0m ? productMonthlyCost : 0m;
    }

    /// <summary>
    /// 主表行是否属于核算月（规范化后的 CostingPeriod 或 CostingDate 月份）
    /// </summary>
    /// <param name="header">主表行</param>
    /// <param name="period">yyyy-MM</param>
    /// <returns>属于该月则为 true</returns>
    private static bool HeaderMatchesPeriod(TaktBomMaterialCost header, string period)
    {
        var normalized = NormalizeCostingPeriodKey(header.CostingPeriod);
        if (string.Equals(normalized, period, StringComparison.Ordinal))
        {
            return true;
        }
        return ToPeriodKey(header.CostingDate) == period;
    }

    /// <summary>
    /// 核算期间键规范化为 yyyy-MM（兼容 2026/07、202607）
    /// </summary>
    /// <param name="costingPeriod">原始期间</param>
    /// <returns>yyyy-MM；无法解析则原 Trim 或空</returns>
    private static string NormalizeCostingPeriodKey(string? costingPeriod)
    {
        if (string.IsNullOrWhiteSpace(costingPeriod))
        {
            return string.Empty;
        }
        var raw = costingPeriod.Trim().Replace('/', '-');
        if (raw.Length == 6 && raw.All(char.IsDigit))
        {
            return $"{raw[..4]}-{raw[4..6]}";
        }
        if (raw.Length >= 7 && raw[4] == '-')
        {
            return raw[..7];
        }
        return raw;
    }

    private async Task FillComponentGroupTextsAsync(
        string plantCode,
        List<TaktBomPriceDeltaTrendDto> rows,
        string? basePeriod,
        string? comparePeriod)
    {
        if (rows.Count == 0 || string.IsNullOrWhiteSpace(comparePeriod))
        {
            return;
        }
        if (!TryParsePeriodMonth(comparePeriod, out var compareMonth))
        {
            return;
        }
        DateTime rangeStart = compareMonth;
        DateTime rangeEnd = compareMonth;
        if (!string.IsNullOrWhiteSpace(basePeriod) && TryParsePeriodMonth(basePeriod, out var baseMonth))
        {
            rangeStart = baseMonth < compareMonth ? baseMonth : compareMonth;
            rangeEnd = baseMonth > compareMonth ? baseMonth : compareMonth;
        }
        var productCodes = rows.Select(r => r.ProductCode).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
        var items = await LoadBomCostItemsForProductsAsync(plantCode, productCodes, rangeStart, rangeEnd);
        var byProduct = items
            .Where(r => !string.IsNullOrWhiteSpace(r.ProductCode))
            .GroupBy(r => NormalizeProductKey(r.ProductCode!), StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => g.ToList(), StringComparer.OrdinalIgnoreCase);

        var rawZeroByProduct = new Dictionary<string, List<(string Code, decimal Qty)>>(StringComparer.OrdinalIgnoreCase);
        foreach (var row in rows)
        {
            var skipVariance = !string.IsNullOrWhiteSpace(basePeriod)
                && ShouldSkipVarianceForProduct(row, basePeriod!, comparePeriod!);
            if (skipVariance)
            {
                ClearProductVarianceAndGroupFields(row);
                continue;
            }

            if (!byProduct.TryGetValue(NormalizeProductKey(row.ProductCode), out var productItems))
            {
                // 无明细时若环比差异为 0/空，组文全空
                if (IsZeroOrEmptyVariance(row.PriceDelta))
                {
                    ClearProductVarianceAndGroupFields(row);
                }
                continue;
            }

            if (!string.IsNullOrWhiteSpace(basePeriod))
            {
                var (priceText, priceSummary, componentText, componentSummary) = BuildDeltaGroupTexts(
                    plantCode,
                    row.ProductCode,
                    productItems,
                    basePeriod!,
                    comparePeriod!);
                row.PriceDeltaTrend = priceText;
                row.ComponentDeltaGroup = componentText;
                // 差异列 = 价格差异组 Summary Var + 组件差异 Summary Var（完全一致）
                row.PriceDelta = TaktBomMaterialCostItemLineCostHelper.RoundCost(
                    checked(priceSummary + componentSummary));
            }

            // 环比差异为 0：0价格组 / 价格差异组 / 组件差异组全空
            if (IsZeroOrEmptyVariance(row.PriceDelta))
            {
                ClearProductVarianceAndGroupFields(row);
                continue;
            }

            if (HasPositivePeriodCost(row.PeriodCosts, comparePeriod!))
            {
                rawZeroByProduct[NormalizeProductKey(row.ProductCode)] =
                    CollectZeroPriceEntries(productItems, comparePeriod!);
            }
        }

        var rawZeroCodes = rawZeroByProduct.Values
            .SelectMany(list => list.Select(e => e.Code))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        var headerCostByComponent = await LoadProductMonthlyCostByCodesAsync(
            plantCode, comparePeriod!, rawZeroCodes);
        var pricedComponentCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var code in rawZeroCodes)
        {
            if (TryGetProductMonthlyCost(headerCostByComponent, code, out _))
            {
                pricedComponentCodes.Add(code);
            }
        }

        var zeroByProduct = new Dictionary<string, List<(string Code, decimal Qty)>>(StringComparer.OrdinalIgnoreCase);
        foreach (var (productKey, list) in rawZeroByProduct)
        {
            zeroByProduct[productKey] = list
                .Where(e => !pricedComponentCodes.Contains(e.Code))
                .ToList();
        }

        var substituteByComponent = await ResolveSubstitutePricesAsync(
            plantCode,
            comparePeriod!,
            zeroByProduct.Values.SelectMany(list => list.Select(e => e.Code))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList());

        foreach (var row in rows)
        {
            var productKey = NormalizeProductKey(row.ProductCode);
            if (!zeroByProduct.TryGetValue(productKey, out var zeroEntries))
            {
                continue;
            }
            row.ZeroPriceGroup = BuildZeroPriceGroupText(zeroEntries, substituteByComponent);
        }
    }

    /// <summary>
    /// 是否跳过差异统计：比较月/基准月任一产品成本为 0。
    /// 跳过或算出差异为 0 时：差异列=0，0价格组/价格差异组/组件差异组全空。
    /// </summary>
    private static bool ShouldSkipVarianceForProduct(
        TaktBomPriceDeltaTrendDto row,
        string basePeriod,
        string comparePeriod)
    {
        // 比较月为 0 / 缺失 → 不参与
        if (!HasPositivePeriodCost(row.PeriodCosts, comparePeriod))
        {
            return true;
        }
        // 基准月为 0（有键无正价）→ 不参与
        if (row.PeriodCosts.ContainsKey(basePeriod)
            && !HasPositivePeriodCost(row.PeriodCosts, basePeriod))
        {
            return true;
        }
        // 基准月键缺失：展示期内成本为 0 时 PriceDelta 仍为 null（跳过标记）；
        // 基准月不在展示期且两侧有价时 PriceDelta 已有数值（含 0）
        if (!row.PeriodCosts.ContainsKey(basePeriod))
        {
            return row.PriceDelta == null;
        }
        return false;
    }

    private static bool HasPositivePeriodCost(
        IReadOnlyDictionary<string, decimal> periodCosts,
        string period)
        => periodCosts.TryGetValue(period, out var cost) && cost > 0m;

    /// <summary>
    /// 环比差异为空（跳过）或数值为 0
    /// </summary>
    private static bool IsZeroOrEmptyVariance(decimal? priceDelta)
        => priceDelta == null || priceDelta.Value == 0m;

    /// <summary>
    /// 差异列置 0，三组文全空（比较/基准月成本为 0，或环比差异为 0）
    /// </summary>
    private static void ClearProductVarianceAndGroupFields(TaktBomPriceDeltaTrendDto row)
    {
        row.PriceDelta = 0m;
        row.PriceDeltaTrend = string.Empty;
        row.ComponentDeltaGroup = string.Empty;
        row.ZeroPriceGroup = string.Empty;
    }

    /// <summary>
    /// 关注月零价组件清单（与零价格视图同口径：QualifiesAsZeroPriceListLine = X + PcbSectIndicator 空 + F + 移动价=0；与用量无关；
    /// 同一 ComponentCode 不同位置各自判定，任一笔满足即入组；用量取合格行中最大用量仅供展示）
    /// </summary>
    /// <param name="productItems">该产品已 Filter（生产相关=X、PCB SECT 标识为空、采购类型=F）的明细</param>
    /// <param name="comparePeriod">关注月 yyyy-MM</param>
    /// <returns>组件编码与用量</returns>
    private static List<(string Code, decimal Qty)> CollectZeroPriceEntries(
        IReadOnlyList<TaktBomMaterialCostItem> productItems,
        string comparePeriod)
    {
        return productItems
            .Where(r => ToPeriodKey(r.CostingDate) == comparePeriod
                && !string.IsNullOrWhiteSpace(r.ComponentCode)
                && TaktBomMaterialCostItemLineCostHelper.QualifiesAsZeroPriceListLine(r))
            .GroupBy(r => r.ComponentCode.Trim(), StringComparer.OrdinalIgnoreCase)
            .Select(g =>
            {
                // 多位置：取合格行中用量最大者（避免 0.001 残行若误入时压过用量=1 的真实行）
                var best = g
                    .OrderByDescending(x => x.ComponentQuantity)
                    .ThenByDescending(x => x.CostingDate)
                    .ThenByDescending(x => x.Id)
                    .First();
                return (Code: g.Key, Qty: best.ComponentQuantity);
            })
            .OrderBy(x => x.Code, StringComparer.Ordinal)
            .ToList();
    }

    /// <summary>
    /// 0价格组：物料:用量:可替代物料:替代价格（末字母 Z→A 逆推；无可替代则仅物料:用量）
    /// </summary>
    private static string BuildZeroPriceGroupText(
        IReadOnlyList<(string Code, decimal Qty)> zeroEntries,
        IReadOnlyDictionary<string, (string SubstituteCode, decimal Price)> substituteByComponent)
    {
        var parts = new List<string>();
        foreach (var entry in zeroEntries)
        {
            if (substituteByComponent.TryGetValue(entry.Code, out var sub))
            {
                parts.Add($"{entry.Code}:{FormatQuantity(entry.Qty)}:{sub.SubstituteCode}:{FormatMoney(sub.Price)}");
            }
            else
            {
                parts.Add($"{entry.Code}:{FormatQuantity(entry.Qty)}");
            }
        }
        return FormatGroup(parts);
    }

    /// <summary>
    /// 为零价组件按末字母逆推（C→B→A）：仅查移动价格表；优先关注月同 ValuationPeriod，否则取以前最近有价期间；展示价=MovingPrice÷PriceUnit；不查 cost_item
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="costingPeriod">关注月 / 期间最大月 yyyy-MM</param>
    /// <param name="componentCodes">零价组件编码</param>
    /// <returns>原组件 → (可替代编码, 替代价)</returns>
    private async Task<Dictionary<string, (string SubstituteCode, decimal Price)>> ResolveSubstitutePricesAsync(
        string plantCode,
        string costingPeriod,
        IReadOnlyList<string> componentCodes)
    {
        var result = new Dictionary<string, (string SubstituteCode, decimal Price)>(StringComparer.OrdinalIgnoreCase);
        if (componentCodes.Count == 0 || string.IsNullOrWhiteSpace(costingPeriod))
        {
            return result;
        }
        var periodKey = costingPeriod.Trim();
        if (!TryParsePeriodMonth(periodKey, out var costingMonth))
        {
            return result;
        }
        var lookbackStart = costingMonth.AddMonths(-24).ToString("yyyy-MM");

        var candidateSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var candidatesByComponent = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
        foreach (var code in componentCodes)
        {
            var candidates = EnumeratePreviousLetterRevisions(code).ToList();
            candidatesByComponent[code] = candidates;
            foreach (var candidate in candidates)
            {
                foreach (var variant in TaktBomMaterialCostItemLineCostHelper.ExpandProductCodeLookupVariants(candidate))
                {
                    candidateSet.Add(variant);
                }
            }
        }
        if (candidateSet.Count == 0)
        {
            return result;
        }

        var samePeriodMoving = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase);
        var lookbackMoving = new Dictionary<string, (string Period, decimal Price)>(StringComparer.OrdinalIgnoreCase);
        await FillMovingPriceLookupsAsync(
            plantCode, lookbackStart, periodKey, candidateSet.ToList(), samePeriodMoving, lookbackMoving);

        foreach (var code in componentCodes)
        {
            if (!candidatesByComponent.TryGetValue(code, out var list))
            {
                continue;
            }
            foreach (var candidate in list)
            {
                if (TryGetMovingPriceExactPeriod(samePeriodMoving, candidate, out var exactPrice))
                {
                    result[code] = (candidate, exactPrice);
                    break;
                }
                if (TryGetMovingPriceLookback(lookbackMoving, candidate, out var lookbackPrice))
                {
                    result[code] = (candidate, lookbackPrice);
                    break;
                }
            }
        }
        return result;
    }

    /// <summary>
    /// 加载移动价格：同期间（=关注月）与向前 24 个月回溯（仅 material_moving_price）
    /// </summary>
    private async Task FillMovingPriceLookupsAsync(
        string plantCode,
        string lookbackStart,
        string periodKey,
        IReadOnlyList<string> materialCodes,
        Dictionary<string, decimal> samePeriodMoving,
        Dictionary<string, (string Period, decimal Price)> lookbackMoving)
    {
        if (materialCodes.Count == 0)
        {
            return;
        }
        const int chunkSize = 200;
        for (var i = 0; i < materialCodes.Count; i += chunkSize)
        {
            var chunk = materialCodes.Skip(i).Take(chunkSize).ToList();
            Expression<Func<TaktMaterialMovingPrice, bool>> predicate = x =>
                x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode == plantCode
                && chunk.Contains(x.MaterialCode)
                && x.MovingPrice > 0;
            var remaining = MovingPriceLookupMaxRows
                - Math.Max(samePeriodMoving.Count, lookbackMoving.Count);
            if (remaining <= 0)
            {
                break;
            }
            var part = await GetMovingPriceListForRangeAsync(predicate, lookbackStart, periodKey, remaining);
            foreach (var price in part)
            {
                if (string.IsNullOrWhiteSpace(price.MaterialCode) || price.MovingPrice <= 0m)
                {
                    continue;
                }
                var unitPrice = TaktBomMaterialCostItemLineCostHelper.ResolveMaterialMovingUnitPrice(price);
                if (unitPrice <= 0m)
                {
                    continue;
                }
                var ym = NormalizeCostingPeriodKey(price.ValuationPeriod);
                if (ym.Length == 0
                    || string.CompareOrdinal(ym, lookbackStart) < 0
                    || string.CompareOrdinal(ym, periodKey) > 0)
                {
                    continue;
                }
                foreach (var key in TaktBomMaterialCostItemLineCostHelper.ExpandProductCodeLookupVariants(price.MaterialCode))
                {
                    if (string.Equals(ym, periodKey, StringComparison.Ordinal))
                    {
                        if (!samePeriodMoving.TryGetValue(key, out var existingSame)
                            || unitPrice > existingSame)
                        {
                            samePeriodMoving[key] = unitPrice;
                        }
                    }
                    if (!lookbackMoving.TryGetValue(key, out var existing)
                        || string.CompareOrdinal(ym, existing.Period) > 0
                        || (string.Equals(ym, existing.Period, StringComparison.Ordinal)
                            && unitPrice > existing.Price))
                    {
                        lookbackMoving[key] = (ym, unitPrice);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 取与关注月相同 ValuationPeriod 的移动价
    /// </summary>
    private static bool TryGetMovingPriceExactPeriod(
        IReadOnlyDictionary<string, decimal> samePeriodMoving,
        string materialCode,
        out decimal price)
    {
        price = 0m;
        foreach (var variant in TaktBomMaterialCostItemLineCostHelper.ExpandProductCodeLookupVariants(materialCode))
        {
            if (!samePeriodMoving.TryGetValue(variant, out var hit) || hit <= 0m)
            {
                continue;
            }
            price = hit;
            return true;
        }
        return false;
    }

    /// <summary>
    /// 取关注月及以前最近有价移动价
    /// </summary>
    private static bool TryGetMovingPriceLookback(
        IReadOnlyDictionary<string, (string Period, decimal Price)> lookbackMoving,
        string materialCode,
        out decimal price)
    {
        price = 0m;
        foreach (var variant in TaktBomMaterialCostItemLineCostHelper.ExpandProductCodeLookupVariants(materialCode))
        {
            if (!lookbackMoving.TryGetValue(variant, out var hit) || hit.Price <= 0m)
            {
                continue;
            }
            price = hit.Price;
            return true;
        }
        return false;
    }

    /// <summary>
    /// 加载指定核算月主表 ProductMonthlyCalculation（按产品编码；禁止 ModelMonthlyAverageCost）
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="costingPeriod">核算月 yyyy-MM</param>
    /// <param name="productCodes">产品/组件编码</param>
    /// <returns>编码变体 → ProductMonthlyCost</returns>
    private async Task<Dictionary<string, decimal>> LoadProductMonthlyCostByCodesAsync(
        string plantCode,
        string costingPeriod,
        IReadOnlyList<string> productCodes)
    {
        var result = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase);
        if (productCodes.Count == 0 || string.IsNullOrWhiteSpace(costingPeriod)
            || !TryParsePeriodMonth(costingPeriod, out var monthStart))
        {
            return result;
        }
        var monthEnd = monthStart.AddMonths(1);
        var lookupCodes = productCodes
            .SelectMany(TaktBomMaterialCostItemLineCostHelper.ExpandProductCodeLookupVariants)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        const int chunkSize = 200;
        for (var i = 0; i < lookupCodes.Count; i += chunkSize)
        {
            var chunk = lookupCodes.Skip(i).Take(chunkSize).ToList();
            var part = await _bomMaterialCostRepository.GetListAsync(x =>
                x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode == plantCode
                && x.IsDeleted == 0
                && chunk.Contains(x.ProductCode)
                && x.ProductMonthlyCalculation > 0
                && (x.CostingPeriod == costingPeriod
                    || (x.CostingDate >= monthStart && x.CostingDate < monthEnd)));
            foreach (var header in part)
            {
                if (string.IsNullOrWhiteSpace(header.ProductCode) || header.ProductMonthlyCalculation <= 0m)
                {
                    continue;
                }
                foreach (var key in TaktBomMaterialCostItemLineCostHelper.ExpandProductCodeLookupVariants(header.ProductCode))
                {
                    if (!result.TryGetValue(key, out var existing) || header.ProductMonthlyCalculation > existing)
                    {
                        result[key] = header.ProductMonthlyCalculation;
                    }
                }
            }
        }
        return result;
    }

    /// <summary>
    /// 组件编码是否在主表有 ProductMonthlyCost
    /// </summary>
    /// <param name="costByCode">主表成本字典</param>
    /// <param name="componentCode">组件编码</param>
    /// <param name="cost">ProductMonthlyCost</param>
    /// <returns>有正价则为 true</returns>
    private static bool TryGetProductMonthlyCost(
        IReadOnlyDictionary<string, decimal> costByCode,
        string componentCode,
        out decimal cost)
    {
        cost = 0m;
        foreach (var variant in TaktBomMaterialCostItemLineCostHelper.ExpandProductCodeLookupVariants(componentCode))
        {
            if (!costByCode.TryGetValue(variant, out var hit) || hit <= 0m)
            {
                continue;
            }
            cost = hit;
            return true;
        }
        return false;
    }

    /// <summary>
    /// 构建价格差异组 + 组件差异组；Summary Var 均为行成本差（CalculateLineCost），二者之和写入差异列。
    /// 组内条目：价格差异组按单价 Diff 降序；组件差异按行成本 Diff 降序（同值再按文案）。
    /// </summary>
    /// <returns>价格组文、价格汇总、组件组文、组件汇总</returns>
    private static (string PriceText, decimal PriceSummary, string ComponentText, decimal ComponentSummary) BuildDeltaGroupTexts(
        string plantCode,
        string productCode,
        IReadOnlyList<TaktBomMaterialCostItem> productItems,
        string basePeriod,
        string comparePeriod)
    {
        var baseMap = BuildPeriodComponentRowMap(productItems, plantCode, productCode, basePeriod);
        var compareMap = BuildPeriodComponentRowMap(productItems, plantCode, productCode, comparePeriod);

        var matchedBase = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var matchedCompare = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var sameCodeMatched = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // ① 编码完全相同 → 仅单价有差才进价格差异组（Diff=0 不写）
        foreach (var code in baseMap.Keys)
        {
            if (!compareMap.ContainsKey(code))
            {
                continue;
            }
            matchedBase.Add(code);
            matchedCompare.Add(code);
            sameCodeMatched.Add(code);
        }

        // 组件差异条目：(展示文案, Diff 金额=行成本差)；最终按 Diff 降序
        var componentEntries = new List<(string Text, decimal Diff)>();
        var componentSummary = 0m;
        // ② 末位版本字母 stem 相同、字母不同 → version
        foreach (var compareCode in compareMap.Keys.Where(c => !matchedCompare.Contains(c)).OrderBy(c => c, StringComparer.Ordinal))
        {
            if (!TrySplitComponentVersion(compareCode, out var compareStem, out var compareLetter))
            {
                continue;
            }
            var baseCandidate = baseMap.Keys
                .Where(c => !matchedBase.Contains(c))
                .Select(c =>
                {
                    var ok = TrySplitComponentVersion(c, out var stem, out var letter);
                    return (Code: c, Ok: ok, Stem: stem, Letter: letter);
                })
                .Where(x => x.Ok
                    && string.Equals(x.Stem, compareStem, StringComparison.OrdinalIgnoreCase)
                    && x.Letter != compareLetter)
                .OrderBy(x => x.Code, StringComparer.OrdinalIgnoreCase)
                .FirstOrDefault();
            if (baseCandidate.Code == null)
            {
                continue;
            }
            matchedBase.Add(baseCandidate.Code);
            matchedCompare.Add(compareCode);
            var baseRows = baseMap[baseCandidate.Code];
            var compareRows = compareMap[compareCode];
            var baseRep = PickRepresentativeRow(baseRows);
            var compareRep = PickRepresentativeRow(compareRows);
            var basePrice = TaktBomMaterialCostItemLineCostHelper.ResolvePerBaseUnitPrice(baseRep);
            var comparePrice = TaktBomMaterialCostItemLineCostHelper.ResolvePerBaseUnitPrice(compareRep);
            var lineDelta = TaktBomMaterialCostItemLineCostHelper.RoundCost(
                SumRowsLineCost(compareRows) - SumRowsLineCost(baseRows));
            componentSummary = checked(componentSummary + lineDelta);
            componentEntries.Add((
                $"{baseCandidate.Code}:{FormatQuantity(SumRowsQty(baseRows))}:{FormatMoney(basePrice)}→{compareCode}:{FormatQuantity(SumRowsQty(compareRows))}:{FormatMoney(comparePrice)}→version",
                lineDelta));
        }

        foreach (var c in baseMap.Keys.Where(x => !matchedBase.Contains(x)).OrderBy(x => x, StringComparer.Ordinal))
        {
            var rows = baseMap[c];
            var price = TaktBomMaterialCostItemLineCostHelper.ResolvePerBaseUnitPrice(PickRepresentativeRow(rows));
            var lineDelta = TaktBomMaterialCostItemLineCostHelper.RoundCost(-SumRowsLineCost(rows));
            componentSummary = checked(componentSummary + lineDelta);
            componentEntries.Add((
                $"{c}:{FormatQuantity(SumRowsQty(rows))}:{FormatMoney(price)}→remove",
                lineDelta));
        }
        foreach (var c in compareMap.Keys.Where(x => !matchedCompare.Contains(x)).OrderBy(x => x, StringComparer.Ordinal))
        {
            var rows = compareMap[c];
            var price = TaktBomMaterialCostItemLineCostHelper.ResolvePerBaseUnitPrice(PickRepresentativeRow(rows));
            var lineDelta = TaktBomMaterialCostItemLineCostHelper.RoundCost(SumRowsLineCost(rows));
            componentSummary = checked(componentSummary + lineDelta);
            componentEntries.Add((
                $"{c}:{FormatQuantity(SumRowsQty(rows))}:{FormatMoney(price)}→new",
                lineDelta));
        }

        var componentParts = componentEntries
            .OrderByDescending(e => e.Diff)
            .ThenBy(e => e.Text, StringComparer.Ordinal)
            .Select(e => e.Text)
            .ToList();
        componentSummary = TaktBomMaterialCostItemLineCostHelper.RoundCost(componentSummary);

        // ③ 仅「编码完全相同」配对进价格差异组（不含 version 配对）；按 Diff（单价差）降序
        var priceEntries = new List<(string Text, decimal Diff)>();
        var priceSummary = 0m;
        foreach (var code in sameCodeMatched)
        {
            if (!compareMap.TryGetValue(code, out var compareRows) || !baseMap.TryGetValue(code, out var baseRows))
            {
                continue;
            }
            var baseRep = PickRepresentativeRow(baseRows);
            var compareRep = PickRepresentativeRow(compareRows);
            var basePrice = TaktBomMaterialCostItemLineCostHelper.ResolvePerBaseUnitPrice(baseRep);
            var comparePrice = TaktBomMaterialCostItemLineCostHelper.ResolvePerBaseUnitPrice(compareRep);
            var deltaDisplay = TaktBomMaterialCostItemLineCostHelper.RoundCost(comparePrice - basePrice);
            if (deltaDisplay == 0m)
            {
                continue;
            }
            var lineDelta = TaktBomMaterialCostItemLineCostHelper.RoundCost(
                SumRowsLineCost(compareRows) - SumRowsLineCost(baseRows));
            priceSummary = checked(priceSummary + lineDelta);
            priceEntries.Add((
                $"{code}:{FormatQuantity(SumRowsQty(compareRows))}:{FormatMoney(basePrice)}→{FormatMoney(comparePrice)},Diff:{FormatMoney(deltaDisplay)}",
                deltaDisplay));
        }
        priceSummary = TaktBomMaterialCostItemLineCostHelper.RoundCost(priceSummary);
        var priceParts = priceEntries
            .OrderByDescending(e => e.Diff)
            .ThenBy(e => e.Text, StringComparer.Ordinal)
            .Select(e => e.Text)
            .ToList();

        return (
            FormatGroup(priceParts, priceSummary),
            priceSummary,
            FormatGroup(componentParts, componentSummary),
            componentSummary);
    }

    /// <summary>
    /// 期间最后核算日快照，按组件编码分组（多 BOM 位置保留多行，行成本合计）
    /// </summary>
    private static Dictionary<string, List<TaktBomMaterialCostItem>> BuildPeriodComponentRowMap(
        IReadOnlyList<TaktBomMaterialCostItem> productItems,
        string plantCode,
        string productCode,
        string periodKey)
    {
        var snap = TaktBomMaterialCostItemLineCostHelper.ResolvePeriodSnapshot(
            productItems,
            plantCode,
            productCode,
            periodKey);
        return snap
            .Where(r => !string.IsNullOrWhiteSpace(r.ComponentCode))
            .GroupBy(r => r.ComponentCode.Trim(), StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => g.ToList(), StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 参与成本统计的行成本合计（与 SumSnapshotCost 同口径：排除 PCB SECT 整树 + X + 标识空 + F）
    /// </summary>
    private static decimal SumRowsLineCost(IReadOnlyList<TaktBomMaterialCostItem> rows)
    {
        return TaktBomMaterialCostItemLineCostHelper.SumSnapshotCost(rows);
    }

    private static decimal SumRowsQty(IReadOnlyList<TaktBomMaterialCostItem> rows)
        => rows.Sum(r => r.ComponentQuantity);

    private static TaktBomMaterialCostItem PickRepresentativeRow(IReadOnlyList<TaktBomMaterialCostItem> rows)
        => rows.OrderByDescending(r => r.CostingDate).ThenByDescending(r => r.Id).First();

    /// <summary>
    /// 拆组件版本：仅末位 A～Z 视为版本字母；无则返回 false（走 new/remove）
    /// </summary>
    private static bool TrySplitComponentVersion(string componentCode, out string stem, out char versionLetter)
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

    private static string FormatGroup(IReadOnlyList<string> parts)
    {
        if (parts.Count == 0)
        {
            return string.Empty;
        }
        var sb = new StringBuilder();
        sb.Append('(');
        sb.Append(string.Join(", ", parts));
        sb.Append(')');
        return sb.ToString();
    }

    /// <summary>
    /// 组文 + Summary Var（与差异列同精度 5 位）
    /// </summary>
    private static string FormatGroup(IReadOnlyList<string> parts, decimal summaryVar)
    {
        var body = FormatGroup(parts);
        if (string.IsNullOrEmpty(body))
        {
            return string.Empty;
        }
        return $"{body},Summary Var:{FormatSummaryCost(summaryVar)}";
    }

    private static string FormatQuantity(decimal qty)
    {
        if (qty == decimal.Truncate(qty))
        {
            return ((long)qty).ToString(CultureInfo.InvariantCulture);
        }
        return qty.ToString("0.#####", CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// 单价 / Diff / 行成本：一律 RoundCost 5 位（禁止改成 2 位）
    /// </summary>
    private static string FormatMoney(decimal value)
        => FormatSummaryCost(value);

    /// <summary>
    /// Summary Var / 差异列展示（与 RoundCost 5 位一致）
    /// </summary>
    private static string FormatSummaryCost(decimal value)
        => TaktBomMaterialCostItemLineCostHelper.RoundCost(value)
            .ToString("0.#####", CultureInfo.InvariantCulture);

    /// <summary>
    /// 加载产品 BOM 明细：先拉全量展开，再 Filter（生产相关=X、PCB SECT 标识为空、采购类型=F）。
    /// 与零价格清单同口径；分块仅查询，Filter 合并后执行一次。
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="productCodes">产品编码</param>
    /// <param name="costingMonthStart">核算月初</param>
    /// <param name="costingMonthEnd">核算月末（含该月）</param>
    /// <returns>已 Filter 的明细行</returns>
    private async Task<List<TaktBomMaterialCostItem>> LoadBomCostItemsForProductsAsync(
        string plantCode,
        IReadOnlyList<string> productCodes,
        DateTime costingMonthStart,
        DateTime costingMonthEnd)
    {
        var raw = new List<TaktBomMaterialCostItem>();
        const int chunkSize = 200;
        var costingExclusiveEnd = costingMonthEnd.AddMonths(1);
        var lookupCodes = productCodes
            .SelectMany(TaktBomMaterialCostItemLineCostHelper.ExpandProductCodeLookupVariants)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        if (lookupCodes.Count == 0)
        {
            return raw;
        }
        for (var i = 0; i < lookupCodes.Count; i += chunkSize)
        {
            var chunk = lookupCodes.Skip(i).Take(chunkSize).ToList();
            var exp = Expressionable.Create<TaktBomMaterialCostItem>();
            exp = exp.And(x =>
                x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode == plantCode
                && x.IsDeleted == 0
                && chunk.Contains(x.ProductCode)
                && x.CostingDate >= costingMonthStart
                && x.CostingDate < costingExclusiveEnd);
            var years = TaktYearShardTableHelper.ResolveYears(
                costingMonthStart,
                costingExclusiveEnd.AddDays(-1));
            foreach (var year in years)
            {
                var yearTable = await ResolveBomItemPhysicalTableAsync(year);
                var part = await _bomMaterialCostItemRepository.GetListAsync(exp.ToExpression(), yearTable);
                raw.AddRange(part);
            }
        }
        return TaktBomMaterialCostItemLineCostHelper
            .FilterBomMaterialCostItemRows(
                TaktBomMaterialCostItemLineCostHelper.ExcludePcbSectHierarchyRows(raw))
            .ToList();
    }

    private async Task<string?> ResolveBomItemPhysicalTableAsync(int year)
    {
        var table = TaktYearShardTableHelper.BuildYearTableName(BomItemYearShardBaseTable, year);
        return await _bomMaterialCostItemRepository.PhysicalTableExistsAsync(table) ? table : null;
    }

    private static string NormalizeProductKey(string productCode)
    {
        var trimmed = productCode.Trim();
        var normalized = TaktStringHelper.NormalizeSapNumericMaterialCode(trimmed);
        return string.IsNullOrEmpty(normalized) ? trimmed : normalized;
    }

    private static string ToPeriodKey(DateTime costingDate)
        => $"{costingDate.Year:D4}-{costingDate.Month:D2}";

    private static bool TryParsePeriodMonth(string period, out DateTime month)
    {
        return DateTime.TryParseExact(
            period.Trim() + "-01",
            "yyyy-MM-dd",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out month);
    }

    private static TaktBomPriceDeltaTrendResultDto EmptyResult(int pageIndex, int pageSize)
        => new()
        {
            Paged = TaktPagedResult<TaktBomPriceDeltaTrendDto>.Create(
                new List<TaktBomPriceDeltaTrendDto>(), 0, pageIndex, pageSize),
        };

    /// <summary>
    /// 按年分表查询移动价格（关注月及向前 24 个月）
    /// </summary>
    private async Task<List<TaktMaterialMovingPrice>> GetMovingPriceListForRangeAsync(
        Expression<Func<TaktMaterialMovingPrice, bool>> predicate,
        string? valuationPeriodStart,
        string? valuationPeriodEnd,
        int? maxRows = null)
    {
        var years = TaktYearShardTableHelper.ResolveYearsFromYyyyMmPeriod(valuationPeriodStart, valuationPeriodEnd);
        var result = new List<TaktMaterialMovingPrice>();
        var yearsNeedBase = new List<int>();
        foreach (var year in years)
        {
            var table = await ResolveMovingPricePhysicalTableAsync(year);
            if (table == null)
            {
                yearsNeedBase.Add(year);
                continue;
            }
            if (maxRows.HasValue)
            {
                var remaining = maxRows.Value - result.Count;
                if (remaining <= 0)
                {
                    break;
                }
                var part = await _materialMovingPriceRepository.GetListForExportAsync(predicate, remaining, table);
                result.AddRange(part);
            }
            else
            {
                var part = await _materialMovingPriceRepository.GetListAsync(predicate, table);
                result.AddRange(part);
            }
        }
        if (yearsNeedBase.Count == 0)
        {
            return result;
        }
        if (maxRows.HasValue && result.Count >= maxRows.Value)
        {
            return result;
        }
        List<TaktMaterialMovingPrice> basePart;
        if (maxRows.HasValue)
        {
            var remaining = maxRows.Value - result.Count;
            basePart = await _materialMovingPriceRepository.GetListForExportAsync(predicate, remaining);
        }
        else
        {
            basePart = await _materialMovingPriceRepository.GetListAsync(predicate);
        }
        if (yearsNeedBase.Count == years.Count)
        {
            result.AddRange(basePart);
        }
        else
        {
            var yearSet = yearsNeedBase.ToHashSet();
            result.AddRange(basePart.Where(r =>
                !string.IsNullOrWhiteSpace(r.ValuationPeriod)
                && r.ValuationPeriod.Length >= 4
                && int.TryParse(r.ValuationPeriod.AsSpan(0, 4), out var y)
                && yearSet.Contains(y)));
        }
        return result;
    }

    /// <summary>
    /// 解析移动价格物理表
    /// </summary>
    private async Task<string?> ResolveMovingPricePhysicalTableAsync(int year)
    {
        var table = TaktYearShardTableHelper.BuildYearTableName(MovingPriceYearShardBaseTable, year);
        return await _materialMovingPriceRepository.PhysicalTableExistsAsync(table) ? table : null;
    }

    /// <summary>
    /// 枚举组件编码末字母 Z→A 逆推版本（不含当前字母）
    /// </summary>
    /// <param name="componentCode">组件编码</param>
    /// <returns>按字母逆序的前一版本编码</returns>
    private static IEnumerable<string> EnumeratePreviousLetterRevisions(string componentCode)
    {
        if (string.IsNullOrWhiteSpace(componentCode))
        {
            yield break;
        }
        var code = componentCode.Trim();
        if (code.Length < 2)
        {
            yield break;
        }
        var last = code[^1];
        if (!char.IsAsciiLetter(last))
        {
            yield break;
        }
        var prefix = code[..^1];
        var min = char.IsUpper(last) ? 'A' : 'a';
        for (var c = (char)(last - 1); c >= min; c--)
        {
            yield return prefix + c;
        }
    }
}
